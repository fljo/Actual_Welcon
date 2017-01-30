Imports System
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
'Imports System.String
'Imports System.Convert




Public Class Form1

    Public Structure Stat
        Public Nr As Integer
        Public Ang As Double
    End Structure


    Dim Direction As Integer = 1    ' bruges til at bestemme retningen når skanninger vises i "play mode"
    Private _right As String
    Private _left As String
    Public RobUnInitCnt As Integer = 0
    Const cnstApp As String = "Inrotech Weld"
    Const cnstSection As String = "setting"
    Public ScanNr As Integer



    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.Height = 720
        Me.Width = 1030
        'initialisering af scanner
        'ShutterTime = 600  ' 1 enhed = 10MicroSec -> 100 = 1 mSec
        ShutterTime = Val(GetSetting(cnstApp, cnstSection, "ShutterTime"))
        TextBox3.Text = 0
        MainLoop_Active = False
        Scan_Distance = 345                 ' afstanden til emnet ved første skan
        DigSearch = False
        RotationsForskel = 10
        GFX.FillRectangle(Brushes.LightGray, 0, 0, PictureBox1.Width, PictureBox1.Height)
        PictureBox1.Image = bmap
        RobotIP = GetSetting(cnstApp, cnstSection, "HostName")
        'RobotIP = "192.168.62.215"
        GetIP()

        PictureBox1.BorderStyle = BorderStyle.FixedSingle
        ActLine = 1
        PicScale = 4.5   '5
        Pict_X_Offs = 420  '420
        Pict_Y_Offs = 1000   '700
        GetScan.Enabled = False
        InitScan.Enabled = True
        Analyze_sel.Enabled = True
        MainLoop_Active = True
        Scan_Draw_Mode = 1

        CommTimer.Interval = 1000
        Start1.Visible = False
        CommTimer.Enabled = True        ' hvis true, så starter programmet op automatisk
        Error_Lbl.Text = ""
        ErrLabel2.Text = ""
        Status_Lbl1.Text = ""
        Status_Lbl2.Text = ""
        ErrorTracker = 0
        ScannerStatus.Text = ""

    End Sub

    Private Sub Set_ShutterTime()
        Dim SH_TimeInt As Integer
        Dim ShutterTime_100 As Integer

        'get host name
        ShutterTime = Val(GetSetting(cnstApp, cnstSection, "ShutterTime"))
        ShutterTime_100 = ShutterTime / 100
        SH_TimeInt = Val(InputBox("Indtast ShutterTime", , ShutterTime_100))
        If SH_TimeInt = 0 And ShutterTime = 0 Then
            ShutterTime = 100
        Else
            If SH_TimeInt <> 0 Then
                ShutterTime = SH_TimeInt * 100
            Else
                ShutterTime = ShutterTime_100 * 100
            End If

        End If
        SaveSetting(cnstApp, cnstSection, "ShutterTime", ShutterTime)

        Init_Steps(10)
        'get host name
        'RobotIP = GetSetting(cnstApp, cnstSection, "HostName")
        ''RobotIP = InputBox("Please input robot host name", , RobotIP)
        'If RobotIP = "" Then
        'End
        'End If
        'SaveSetting(cnstApp, cnstSection, "HostName", RobotIP)

    End Sub

    Private Sub Status_Msg(ByVal StatVal As Integer)
        Dim ProcesStat As String = ""
        Dim ProgramStat As String = ""


        If StatVal >= 64 Then
            ProcesStat = "Systemet Pauser / Venter!"
            LaserPower("OFF")
            StatVal = StatVal - 64
        Else
            If StatVal >= 32 Then
                ProcesStat = "Systemet står med FEJL!"
                LaserPower("OFF")
                StatVal = StatVal - 32
            Else
                If StatVal >= 16 Then
                    ProcesStat = " Lysbue er aktiv"
                    LaserPower("OFF")
                    StatVal = StatVal - 16
                End If
            End If
        End If


        Select Case StatVal
            Case (0)
                ProgramStat = "Klar til nyt svøb"

            Case (1)
                ProgramStat = "Bomhøjden kører"
                LaserPower("ON")

            Case (2)
                ProgramStat = "Vinklen justeres"
                LaserPower("ON")

            Case (3)
                ProgramStat = "Tårnet positioneres"
                LaserPower("ON")

            Case (4)
                ProgramStat = "Søger efter fugen"
                LaserPower("ON")

            Case (5)
                ProgramStat = "Skanner svejsefugen"
                LaserPower("ON")

            Case (6)
                ProgramStat = "Svejseprogram afvikles"
                LaserPower("OFF")


            Case (7)
                ProgramStat = ""

            Case (8)
                ProgramStat = "Renser Svejsepistol"
                LaserPower("OFF")

            Case (9)
                ProgramStat = "Checker fugen med skanner"
                LaserPower("ON")

            Case (10)
                ProgramStat = "Svejsefugen Blæses"
                LaserPower("OFF")

        End Select

        Status_Lbl1.Text = ProgramStat
        Status_Lbl2.ForeColor = Color.Red
        Status_Lbl2.Text = ProcesStat



    End Sub


    Private Sub MainLoop()

        Dim MotCom As Motion_Ctrl
        Dim ActPos As xyzwprExt
        Dim success As Boolean
        Dim Motion As Boolean = True
        Dim command As Integer
        Dim udgang(0) As Short

        On Error GoTo ErrHandler

        While MainLoop_Active = True

            If RobotReady = True Then       ' læser værdierne fra robotten

                WriteRNDRobot() 'check om det returner false, og genstart forbindelse hvis nødvendigt

                'send data
                ReadRNDRobot() 'check om det returner false, og genstart forbindelse hvis nødvendigt

                If OmronConnected Then
                    Lbl_OmronOn.BackColor = Color.Green
                Else
                    Lbl_OmronOn.BackColor = Color.Red
                End If


                RobUnInitCnt = 0
                Lbl_RobOn.BackColor = Color.Green
                ReadRobot()
                lbl_X_Pos.Text = "X = " & Format(RobotVal(1).CartPos.X, "####.#0")
                Lbl_Y_Pos.Text = "Y = " & Format(RobotVal(1).CartPos.Y, "####.#0")
                lbl_Z_Pos.Text = "Z = " & Format(RobotVal(1).CartPos.Z, "####.#0")

                Status_Msg(RealNumReg1(14))     ' skriv status på skærmen

                command = Math.Round(RobotVal(1).NumRegInt)

                If command <> 5 Then
                    ScanCnt = 0         ' når der kaldes på et skannet punkt tæller ScanCnt antallet af valide skan
                End If





                If command <> 0 Then        ' register 1 er <> fra nul - der er en commando


                    If Searching = True Then        'udgang 2 på robotten er sat = searching

                        ActPos = RobotVal(1).CartPos
                        ' hvis robotten 

                        MotCom = ScanID(command)      'håndterer bevægelserne af robotten og finder ud af om målingerne er valide

                        Select Case MotCom.Task

                            Case (1)    ' Task 1 er motion kontrol
                                If Robot_Moving = False Then
                                    ' robotten er ikke i gang med at udføre en bevægelse - derfor kan vi igangsætte en ny bevægelse

                                    Select Case MotCom.Motion_Dir
                                        Case (1)
                                            ActPos.X = ActPos.X + MotCom.Motion_Dist
                                        Case (2)
                                            ActPos.E1 = ActPos.E1 + MotCom.Motion_Dist
                                            ActPos.Y = ActPos.Y + MotCom.Motion_Dist
                                        Case (-2)
                                            ActPos.E1 = ActPos.E1 - MotCom.Motion_Dist
                                            ActPos.Y = ActPos.Y - MotCom.Motion_Dist
                                        Case (3)
                                            ActPos.Z = ActPos.Z + MotCom.Motion_Dist
                                        Case (-3)
                                            ActPos.Z = ActPos.Z - MotCom.Motion_Dist


                                    End Select


                                    SetPosRegX(ActPos, 5)

                                    '  nedenstående gør at der ikke sendes kommandoer
                                    udgang(0) = 1
                                    ErrorTracker = 12
                                    success = cmdSetSDO(1, udgang)
                                    Robot_Moving = True
                                End If
                            Case (2)    ' task 2 terminerer motion
                                udgang(0) = 0
                                ErrorTracker = 15
                                success = cmdSetSDO(1, udgang)
                                success = cmdSetSDO(2, udgang)
                                success = cmdSetSDO(3, udgang)
                                Robot_Moving = False
                                Searching = False
                                ErrorTracker = 1
                                SetIntReg(1, 0)
                                'ReadRobot()

                            Case (3)    ' start søgning i y-retningen
                                ActPos.E1 = ActPos.E1 + MotCom.Motion_Dist
                                ActPos.Y = ActPos.Y + MotCom.Motion_Dist
                                udgang(0) = 1
                                success = cmdSetSDO(3, udgang)
                                DigSearch = True
                                SetPosRegX(ActPos, 5)
                                success = cmdSetSDO(1, udgang)

                            Case (4)    ' stop søgning 
                                If DigSearch = True Then
                                    'MotCom = ScanID(command)
                                Else
                                    udgang(0) = 0
                                    success = cmdSetSDO(1, udgang)
                                    If success = True Then
                                        success = cmdSetSDO(2, udgang)
                                    End If
                                    If success = True Then
                                        success = cmdSetSDO(3, udgang)
                                    End If

                                    Robot_Moving = False
                                    Searching = False

                                    ErrorTracker = 2
                                    SetIntReg(1, 0)
                                    ReadRobot()
                                    End If

                            Case (5)    ' start søgning efter fugen under rotation
                                    If DigSearch = True Then
                                        MotCom = ScanID(command)
                                Else
                                    ErrorTracker = 13
                                    udgang(0) = 1
                                    success = cmdSetSDO(3, udgang)
                                    DigSearch = True
                                    End If
                        End Select

                    End If

                End If    ' seraching er true
            Else
                Lbl_RobOn.BackColor = Color.Red
                RobUnInitCnt = RobUnInitCnt + 1
                If RobUnInitCnt > 10 Then
                    RobotReady = RobComInit()
                End If
            End If   ' robot er ready


            If ScannerConnected = True Then
                Lbl_ScanOn.BackColor = Color.Green
                Scan_Groove()
            Else
                Lbl_ScanOn.BackColor = Color.Red
                delay(2000)
                If InitProc() = True Then
                    ScannerConnected = True
                Else
                    CloseTransfer()
                End If
            End If



            delay(100)
        End While

ErrHandler:

        Error_Lbl.Text = "Fejl i MainLoop (15001)"
        delay(2000)
    End Sub



    ' SKALERER BILLEDET
    Public Sub ScalePict()    '(ByVal MinX As Integer, ByVal MaxX As Integer, ByVal MinY As Integer, ByVal MaxY As Integer)
        Dim x, y As Integer
        Dim Min_X As Integer = 10000
        Dim Max_X As Integer = 0
        Dim Min_Y As Integer = 10000
        Dim Max_Y As Integer = 0

        For i = 1 To ValidMeas   '1279
            If Laser(Scan_Draw_Mode, i).Z_Pos > 100 Then
                x = (Laser(Scan_Draw_Mode, i).X_Pos / 100)
                y = (Laser(Scan_Draw_Mode, i).Z_Pos / 100)
                x = Int((Laser(Scan_Draw_Mode, i).X_Pos / 100) * PicScale) + Pict_X_Offs
                y = Int((Laser(Scan_Draw_Mode, i).Z_Pos / 100) * PicScale) '- Pict_Y_Offs
                If x < Min_X Then Min_X = x
                If x > Max_X Then Max_X = x
                If y < Min_Y Then Min_Y = y
                If y > Max_Y Then Max_Y = y
            End If
        Next i

        Pict_X_Offs = Pict_X_Offs + (((PictWidth - (Max_X - Min_X)) / 2) - Min_X)
        Pict_Y_Offs = (((Max_Y - Min_Y) / 2) + Min_Y) - (PictHeight / 2)

    End Sub

    'VISER DEN SKANNEDE PUNKTSKY PÅ display'et
    Public Sub ShowScan(ByVal LineNr As Integer)
        Dim x, y As Integer
        Dim Min_X As Integer = PictWidth
        Dim Max_X As Integer = 0
        Dim Min_Y As Integer = PictHeight
        Dim Max_Y As Integer = 0


        GFX.FillRectangle(Brushes.LightGray, 0, 0, PictureBox1.Width, PictureBox1.Height)
        PictureBox1.Image = bmap
        PictureBox1.Image = Nothing

        For i = 1 To ValidMeas
            x = ((Laser(LineNr, i).X_Pos / 100) * PicScale) + Pict_X_Offs
            y = ((Laser(LineNr, i).Z_Pos / 100) * PicScale) - Pict_Y_Offs

            ' skal fjernes
            If x < Min_X Then Min_X = x
            If x > Max_X Then Max_X = x
            If y < Min_Y Then Min_Y = y
            If y > Max_Y Then Max_Y = y

            If y < 0 Then y = 0
            PictureBox1.Image = bmap

            If (x < PictWidth And y < PictHeight And x > 0 And y > 0) Then
                If checkbox1.Checked = True Then
                    y = PictHeight - y
                End If
                If checkbox2.Checked = True Then
                    x = PictWidth - x
                End If
                If Laser(LineNr, i).PixWidth > 12 Then
                    bmap.SetPixel(x, y, Color.Red)
                Else
                    bmap.SetPixel(x, y, Color.Black)
                End If
            End If

        Next i
        If Analyze = True Then
            Analyze_Func("full")
        End If

    End Sub


    'VISER DEN SKANNEDE PUNKTSKY PÅ display'et
    Public Sub DisplayFile()
        Dim x, y As Integer
        Dim Min_X As Integer = PictWidth
        Dim Max_X As Integer = 0
        Dim Min_Y As Integer = PictHeight
        Dim Max_Y As Integer = 0


        GFX.FillRectangle(Brushes.LightGray, 0, 0, PictureBox1.Width, PictureBox1.Height)
        PictureBox1.Image = bmap
        PictureBox1.Image = Nothing

        For i = 1 To ValidMeas
            x = Int((Laser(2, i).X_Pos / 100) * PicScale) + Pict_X_Offs
            y = Int((Laser(2, i).Z_Pos / 100) * PicScale) - Pict_Y_Offs
            ' skal fjernes
            If x < Min_X Then Min_X = x
            If x > Max_X Then Max_X = x
            If y < Min_Y Then Min_Y = y
            If y > Max_Y Then Max_Y = y

            If y < 0 Then y = 0
            PictureBox1.Image = bmap

            If (x < PictWidth And y < PictHeight And x > 0 And y > 0) Then
                If checkbox1.Checked = True Then
                    y = PictHeight - y
                End If
                If checkbox2.Checked = True Then
                    x = PictWidth - x
                End If
                If Laser(2, i).PixWidth > 12 Then
                    bmap.SetPixel(x, y, Color.Red)
                Else
                    bmap.SetPixel(x, y, Color.Black)
                End If
            End If

        Next i
        If Analyze = True Then
            Analyze_Func("full")
        End If

    End Sub



    ' LAVER LINJER PÅ MONITOREN F.EKS. DE FUNDNE LINJER
    Public Sub MakeLine(ByVal Line As LinesFound, ByVal [color] As Color)

        Dim X1 As Double = Line.StartPkt.X
        Dim X2 As Double = Line.SlutPkt.X
        Dim Y1 As Double = Line.StartPkt.Z
        Dim Y2 As Double = Line.SlutPkt.Z

        Dim X_Plot1, Y_Plot1, X_Plot2, Y_Plot2 As Integer

        X_Plot1 = (X1 * PicScale) + Pict_X_Offs
        Y_Plot1 = (Y1 * PicScale) - Pict_Y_Offs
        X_Plot2 = (X2 * PicScale) + Pict_X_Offs
        Y_Plot2 = (Y2 * PicScale) - Pict_Y_Offs

        If checkbox1.Checked = True Then
            Y_Plot1 = PictHeight - Y_Plot1
            Y_Plot2 = PictHeight - Y_Plot2
        End If
        If checkbox2.Checked = True Then
            X_Plot1 = PictWidth - X_Plot1
            X_Plot2 = PictWidth - X_Plot2
        End If
        If chkval = True Then
            'Dim P As Pen = New Pen(Drawing.Color.Red)
        Else
            'Dim P As Pen = New Pen(Drawing.Color.Green)
        End If

        PictureBox1.Image = bmap
        '--------------------------
        Dim g = Graphics.FromImage(bmap)
        Try
            'Dim P As Pen = New Pen(Drawing.Color.Red)
            Dim P As Pen = New Pen(color)
            g.DrawLine(P, X_Plot1, Y_Plot1, X_Plot2, Y_Plot2)

        Catch
            MsgBox("clsBorder.DrawInPicture Error")
        End Try
        '---------------------



    End Sub



    ' SKRIVER KOORDINATERNE FOR CURSOREN NEDERST PÅ SKÆRMEN. KAN BRUGES TIL AT FINDE X/Y VÆRDIER FOR PUNKTER PÅ LINJEN
    Private Sub PictureBox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        Dim x, y, n As Integer

        MouseX1 = e.X
        MouseY1 = e.Y
        x = Int((Laser(Scan_Draw_Mode, n).X_Pos / 100) * PicScale) + Pict_X_Offs
        y = Int((Laser(Scan_Draw_Mode, n).Z_Pos / 100) * PicScale) - Pict_Y_Offs

        'Label3.Text = Str(MouseX1) + "  ,  " + Str(MouseY1)

    End Sub

    ' ZOOMER OG PANNER VED HJÆLP AF MUSEKNAPPERNE (1 ER PAN, 2 OP ER ZOOM IN, 2 NED ER ZOOM UD
    Public Sub PictureBox1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseUp
        Dim xPos As Integer = e.X
        Dim yPos As Integer = e.Y
        Dim MouseKey As Integer
        Dim CheckBoxVal As Integer = 0

        If checkbox1.Checked = True Then CheckBoxVal = 1
        If checkbox2.Checked = True Then CheckBoxVal = CheckBoxVal + 2

        If e.Button = Windows.Forms.MouseButtons.Right Then
            MouseKey = 2
        End If
        If e.Button = Windows.Forms.MouseButtons.Left Then
            MouseKey = 1
        End If

        If MouseKey = 1 Then
            Select Case CheckBoxVal
                Case 0
                    Pict_X_Offs = Pict_X_Offs - (MouseX1 - xPos)
                    Pict_Y_Offs = Pict_Y_Offs + (MouseY1 - yPos)
                Case 1
                    Pict_X_Offs = Pict_X_Offs - (MouseX1 - xPos)
                    Pict_Y_Offs = Pict_Y_Offs - (MouseY1 - yPos)
                Case 2
                    Pict_X_Offs = Pict_X_Offs + (MouseX1 - xPos)
                    Pict_Y_Offs = Pict_Y_Offs + (MouseY1 - yPos)
                Case 3
                    Pict_X_Offs = Pict_X_Offs + (MouseX1 - xPos)
                    Pict_Y_Offs = Pict_Y_Offs - (MouseY1 - yPos)
                Case Else
                    Debug.WriteLine("Not between 1 and 10, inclusive")
            End Select
        End If

        If MouseKey = 2 Then
            PicScale = PicScale + ((MouseY1 - yPos) / 20)
            ScalePict()
        End If
        ShowScan(Scan_Draw_Mode)
        'DrawLineFct(LineID(1), Color.Green)

    End Sub



    Public Sub Picturebox_Txt(ByVal x As Double, ByVal y As Double, ByVal type As Integer)
        ' SKRIVER KOORDINATERNE FOR F.EKS. SKÆRINGSPUNKTERNE UD FOR SKÆRIINGSPUNKTERNE
        Dim g = Graphics.FromImage(bmap)
        Dim f = New Font("Arial", 10)
        Dim b = New SolidBrush(Color.Black)

        Dim txt1, txt2 As String
        txt1 = Format(x, "##0.0")
        txt2 = Format(y, "##0.0")
        Dim s3 = txt1 + ", " + txt2

        x = (Int(x * PicScale)) + Pict_X_Offs - 30
        If type = 2 Then
            y = (Int(y * PicScale)) - Pict_Y_Offs + 10
        Else
            y = (Int(y * PicScale)) - Pict_Y_Offs - 30
        End If

        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        If checkbox1.Checked = True Then
            y = PictHeight - y
        End If
        If checkbox2.Checked = True Then
            x = PictWidth - x
        End If
        g.DrawString(s3, f, b, New Point(x, y))


        If type = 1 Then
            txt1 = "Opening:  "
            txt2 = Format(CrossSect, "##0.0")
            Dim s2 = txt1 + txt2
            y = y - 30
            If checkbox1.Checked = True Then
                y = PictHeight - y
            End If
            g.DrawString(s2, f, b, New Point((PicScale) + Pict_X_Offs, y))
        End If

        PictureBox1.Image = bmap
    End Sub

    '   Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '       ShowScan(Scan_Draw_Mode)
    '   End Sub

    '   Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '        ShowScan(Scan_Draw_Mode)
    '    End Sub



    Private Sub PictureBox1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        Dim X_Pos As Integer = e.X
        Dim Z_Pos As Integer = e.Y
        Dim x1, y1 As Double


        x1 = (X_Pos - Pict_X_Offs) / PicScale
        y1 = (Z_Pos + Pict_Y_Offs) / PicScale

        Dim txt1 As String = Format(x1, "##0.00")
        Dim txt2 As String = Format(y1, "##0.00")
        Label3.Text = txt1 + "  ,  " + txt2

    End Sub



    '    Private Sub DisconnectBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim Close_Succesful As Boolean

    '        Close_Succesful = CloseTransfer()
    '        If Close_Succesful = True Then
    '           ScannerStatus.Text = "Transfer closed"
    'DisconnectBtn.Enabled = False
    '            GetScan.Enabled = False
    '           InitScan.Enabled = True

    '        Else
    '            ScannerStatus.Text = "Closing Failed "
    '        End If
    '        mobjCore.Disconnect()

    '    End Sub



    ' Scan_Groove udfører en scanning af emnet, viser den på skærmen og finder evt linierne
    Public Sub Scan_Groove()

        Dim Scan_Succesful As Boolean = True
        Dim cnt As Integer = 0

        cnt = cnt + 1
        Scan_Succesful = Get_Scan("Quarter", False)
        If Scan_Succesful = True Then
            ScannerStatus.Text = "Scan Succesful"
            ConvertScan()

            PictureBox1.Image = Nothing
            ShowScan(Scan_Draw_Mode)

            Analyze_sel.Enabled = True

        Else
            ScannerStatus.Text = "Scan Failed "

        End If
        Make_TxtFile.Enabled = True

        ReflectVal.MaxVal = ReflectVal.MaxVal
        If Analyze = True Then
            Analyze_Func("full")
        End If


    End Sub

    Public Function Analyze_Func(AnaType As String) As Boolean
        'Dim antal As Integer
        Dim qq(4) As LineFunc
        Dim LinjePkt(10) As Coord
        Dim IntersectLines(3) As Coord
        Analyze_Func = False
        Dim FugeHgtVal As Double
        Dim FugeWdtVal As Double

        On Error GoTo errhandler


        FoundLine(1) = FindLinje(1, Math.Round(ValidMeas * 2 / 3), 1, 1.0, 4, 5, 100)
        FoundLine(2) = FindLinje(ValidMeas, Math.Round(ValidMeas / 3), -1, 1.0, 4, 5, 100)

        MakeLine(FoundLine(1), Color.Blue)
        MakeLine(FoundLine(2), Color.Blue)

        '------------------------------------------------
        'finder arealet ud fra de fundne kanter og beregner Delta-arealet mellem x-værdierne som så summeres
        MeasuredArea = Area_Cal(FoundLine(1).SlutPkt, FoundLine(2).SlutPkt)

        Dim Display As String = Format(Math.Abs(MeasuredArea), "###.0")
        Areal.Text = Display    ' 523
        '------------------------------------------------

        If AnaType = "full" Then

            FoundLine(3) = FindLinje(FoundLine(1).SlutPkt.Nr + 0, ValidMeas - 100, 1, 0.3, 1, 15, 40)
            FoundLine(3) = FindEnd(FoundLine(3), -1, 0.1)
            FoundLine(3).StartPkt = FoundLine(1).SlutPkt
            MakeLine(FoundLine(3), Color.Green)


            FoundLine(4) = FindLinje(FoundLine(2).SlutPkt.Nr - 0, 100, -1, 0.3, 1, 15, 40)
            FoundLine(4) = FindEnd(FoundLine(4), 1, 0.1)
            FoundLine(4).StartPkt = FoundLine(2).SlutPkt
            MakeLine(FoundLine(4), Color.Green)


            FoundLine(5) = FindLinje(FoundLine(3).SlutPkt.Nr, FoundLine(4).SlutPkt.Nr, 1, 0.5, 3, 90, 40)
            FoundLine(5).StartPkt.Z = FoundLine(5).a * FoundLine(5).StartPkt.X + FoundLine(5).b
            FoundLine(5).SlutPkt.Z = FoundLine(5).a * FoundLine(5).SlutPkt.X + FoundLine(5).b
            MakeLine(FoundLine(5), Color.Blue)

            FugeBundVinkel = FoundLine(5).Angle
            Display = Format(FugeBundVinkel, "###.0")
            Bundvinkel.Text = Display
             FugeWdtVal = FoundLine(1).SlutPkt.X - FoundLine(2).SlutPkt.X
            FugeHgtVal = FugeWdtVal / 2 / 0.7   ' den forventede fugehøjden ved en vinkel på 70 grader (2x35grader) (0.7 er invTan(35) 

            Dim PlotIntersect As Boolean = True

            If FugeHgtVal > 15 Then

                IntersectLines(1) = FoundLine(1).SlutPkt
                IntersectLines(2) = FoundLine(2).SlutPkt
                IntersectLines(3) = InterSect(FoundLine(3), FoundLine(4))
                IntersectLines(3).Nr = FoundLine(3).SlutPkt.Nr
                If IntersectLines(3).Success = False Then
                    PlotIntersect = False
                    Return (False)
                End If

            Else
                IntersectLines(1) = FoundLine(1).SlutPkt
                IntersectLines(2) = FoundLine(2).SlutPkt
                IntersectLines(3).X = ((FoundLine(2).SlutPkt.X - FoundLine(1).SlutPkt.X) / 2) + FoundLine(1).SlutPkt.X
                FugeHgtVal = MeasuredArea / FugeWdtVal * 2
                IntersectLines(3).Z = ((FoundLine(1).SlutPkt.Z + FoundLine(2).SlutPkt.Z) / 2) + FugeHgtVal

            End If
            'Return False
            If PlotIntersect = False Then Return False

            FoundLine(1).SlutPkt = IntersectLines(1)
            FoundLine(3).StartPkt = IntersectLines(1)
            FoundLine(2).SlutPkt = IntersectLines(2)
            FoundLine(4).StartPkt = IntersectLines(2)
            'FoundLine(3).SlutPkt = IntersectLines(3)
            'FoundLine(4).SlutPkt = IntersectLines(3)

            ' viser koordinater for skæringspunkter på kurven
            Picturebox_Txt(IntersectLines(1).X, IntersectLines(1).Z, 0)
            Picturebox_Txt(IntersectLines(2).X, IntersectLines(2).Z, 0)
            Picturebox_Txt(IntersectLines(3).X, IntersectLines(3).Z, 2)

            ' lægger de tre punkter fra scanningen ud i variable
            RobotVal(2).CartPos = RobotVal(1).CartPos
            RobotVal(3).CartPos = RobotVal(1).CartPos
            RobotVal(4).CartPos = RobotVal(1).CartPos

            RobotVal(2).CartPos.X = IntersectLines(3).X + RobotVal(1).CartPos.X
            RobotVal(2).CartPos.Z = -IntersectLines(3).Z + RobotVal(1).CartPos.Z
            RobotVal(2).CartPos.Y = RobotVal(1).CartPos.Y

            RobotVal(3).CartPos.X = IntersectLines(2).X + RobotVal(1).CartPos.X
            RobotVal(3).CartPos.Z = -IntersectLines(2).Z + RobotVal(1).CartPos.Z
            RobotVal(3).CartPos.Y = RobotVal(1).CartPos.Y

            RobotVal(4).CartPos.X = IntersectLines(1).X + RobotVal(1).CartPos.X
            RobotVal(4).CartPos.Z = -IntersectLines(1).Z + RobotVal(1).CartPos.Z
            RobotVal(4).CartPos.Y = RobotVal(1).CartPos.Y

            'Me.MakeLine(FoundLine(1), Color.Red)
            'Me.MakeLine(FoundLine(3), Color.Red)

            'Me.MakeLine(FoundLine(2), Color.Red)
            'Me.MakeLine(FoundLine(4), Color.Red)


            Areal_Beregn = TK_Areal(IntersectLines(1), IntersectLines(2), IntersectLines(3))
            'Areal.Text = Str(Areal_Beregn)


            Return (True)
        Else

            ' hvis der ikke findes en fuge returneres med FALSE
            If FoundLine(1).SlutPkt.Success = False Then Return False
            If FoundLine(2).SlutPkt.Success = False Then Return False

            ' AnaType = "top"
            IntersectLines(1) = FoundLine(1).SlutPkt
            IntersectLines(2) = FoundLine(2).SlutPkt
            IntersectLines(3).X = ((FoundLine(2).SlutPkt.X - FoundLine(1).SlutPkt.X) / 2) + FoundLine(1).SlutPkt.X
            FugeHgtVal = FugeWdtVal * 1.3
            IntersectLines(3).Z = ((FoundLine(1).SlutPkt.Z + FoundLine(2).SlutPkt.Z) / 2) + FugeHgtVal

            ' lægger de tre punkter fra scanningen ud i variable
            RobotVal(2).CartPos = RobotVal(1).CartPos   'lægger de aktuelle robotposition i variablerne
            RobotVal(3).CartPos = RobotVal(1).CartPos
            RobotVal(4).CartPos = RobotVal(1).CartPos

            RobotVal(2).CartPos.X = IntersectLines(3).X + RobotVal(1).CartPos.X     ' tilpasser positionerne
            RobotVal(2).CartPos.Z = -IntersectLines(3).Z + RobotVal(1).CartPos.Z
            RobotVal(2).CartPos.Y = RobotVal(1).CartPos.Y

            RobotVal(3).CartPos.X = IntersectLines(2).X + RobotVal(1).CartPos.X
            RobotVal(3).CartPos.Z = -IntersectLines(2).Z + RobotVal(1).CartPos.Z
            RobotVal(3).CartPos.Y = RobotVal(1).CartPos.Y

            RobotVal(4).CartPos.X = IntersectLines(1).X + RobotVal(1).CartPos.X
            RobotVal(4).CartPos.Z = -IntersectLines(1).Z + RobotVal(1).CartPos.Z
            RobotVal(4).CartPos.Y = RobotVal(1).CartPos.Y

            Return (True)
        End If


errhandler:
        Return (False)

    End Function



    Private Sub ThresholdSelect_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ThresholdSelect.DropDownItemClicked

        Dim ThrsHld_Selected As String = e.ClickedItem.Text
        Dim Result As Reflection


        Result = Check_Reflection(e.ClickedItem.Text)
        ThrsHld_Selected = ""
    End Sub



    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Make_TxtFile.Click
        Dim saveFileDialog1 As New SaveFileDialog()
        Dim name As String


        saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        saveFileDialog1.FilterIndex = 2
        saveFileDialog1.RestoreDirectory = True

        If saveFileDialog1.ShowDialog() = DialogResult.OK Then
            name = saveFileDialog1.FileName
            skrivTxtfil(name)
        End If

    End Sub

 
    Private Sub Initialize_System()
        Start1.Enabled = False
        ScannerReady = False
        Make_TxtFile.Enabled = False
        'DisconnectBtn.Enabled = False
        GetScan.Enabled = False

        ScannerConnected = InitProc()


        If ScannerConnected = True Then
            ScannerReady = True
            RobotReady = RobComInit()
            If RobotReady = False Then
                Set_Host()
                RobotReady = RobComInit()
            Else
                Dim udgang(0) As Short

                udgang(0) = 0
                BoolFlag = cmdSetSDO(1, udgang)
                BoolFlag = cmdSetSDO(2, udgang)
                BoolFlag = cmdSetSDO(3, udgang)
                'BoolFlag = cmdSetSDi(1, udgang)
                ErrorTracker = 4
                SetIntReg(1, 0)
            End If

            'DisconnectBtn.Enabled = True
            GetScan.Enabled = True
            InitScan.Enabled = False
            MainLoop()
        End If

    End Sub


    '    Private Sub InitScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InitScan.Click
    '        Initialize_System()
    '    End Sub

    Private Sub GetScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetScan.Click
        Scan_Groove()
        ScalePict()
        ShowScan(Scan_Draw_Mode)

    End Sub

    Private Sub RobotComm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RobotComm.Click
        'Dim RobCommSetup As New Form
        RobComm1.Show()


    End Sub


    Private Sub Analyze_sel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Analyze_sel.Click

        If Analyze = True Then
            Analyze = False
            Analyze_sel.Checked = False
        Else
            Analyze = True
            Analyze_sel.Checked = True
            Analyze_Func("full")
        End If

    End Sub

    Private Sub CenterView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CenterView.Click
        ' centrer visningen
        ScalePict()
        ShowScan(Scan_Draw_Mode)

    End Sub

    Private Sub MakeBINFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MakeBINFileToolStripMenuItem.Click
        skrivBinfil2()
    End Sub

    ' *******************  Læs tekstfil  ***********************




    Private Sub Start1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Start1.Click
        Initialize_System()
        Start1.Enabled = False
    End Sub


    Private Sub Set_Host()


        'get host name
        RobotIP = GetSetting(cnstApp, cnstSection, "HostName")
        RobotIP = InputBox("Please input robot host name", , RobotIP)
        If RobotIP = "" Then
            End
        End If
        SaveSetting(cnstApp, cnstSection, "HostName", RobotIP)

    End Sub

    Private Sub RobotIPAdressToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Set_Host()
    End Sub

    Private Sub Afslut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Afslut.Click
        System.Environment.Exit(-1)
        System.Windows.Forms.Application.Exit()
        End
    End Sub


    Private Sub CommTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CommTimer.Tick

        TimerCnt = TimerCnt + 1
        If TimerCnt > 2 Then
            CommTimer.Enabled = False

            Start1.Enabled = False
            Initialize_System()
        End If

    End Sub



    Public Sub UpLoadPict()

        Dim FilePath As String = Application.StartupPath


        'WPQR
        Return


        bmap.Save(IO.Path.Combine(FilePath, "Screenimg.bmp"), Imaging.ImageFormat.Bmp)
        PictureBox1.Image.Save(IO.Path.Combine(Application.StartupPath, "Screenimg.bmp"), Imaging.ImageFormat.Bmp)

        ' Upload file using FTP
        Dim FilNavn1 As String = FilePath + "\" + "Screenimg.bmp"
        Dim UserName As String = "FTP"
        Dim PassWord As String = "Welcon"

        Dim Ip_Str(4) As String

        Ip_Str(1) = IP_Array(0, 0)

        Ip_Str(1) = IP_Array(0, 3)
        Ip_Str(2) = IP_Array(0, 2)
        Ip_Str(3) = IP_Array(0, 1)
        Ip_Str(4) = IP_Array(0, 0) - 7

        Dim FTP_Str As String = "ftp://" + Ip_Str(1) + "." + Ip_Str(2) + "." + Ip_Str(3) + "." + Ip_Str(4) + "./Screenimg.bmp"


        UploadFile(FilNavn1, FTP_Str, UserName, PassWord)


    End Sub


 
 
    Private Sub ShutterTimeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShutterTimeToolStripMenuItem.Click
        Set_ShutterTime()
        'TekstInputBox.Show()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        System.Environment.Exit(-1)
        System.Windows.Forms.Application.Exit()
        End
    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
 
    End Sub

    Private Sub GetTextfileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GetTextfileToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            'PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)
            FilNavn = OpenFileDialog1.FileName

            Scan_Draw_Mode = 2

            Dim charval As Char
            Dim lgd As Integer = Len(FilNavn)

            For i = 3 To lgd
                charval = Strings.Mid(FilNavn, Len(FilNavn) - (i - 1), 1)
                If charval = " " Then
                    Row_Cnt = Val(Strings.Mid(FilNavn, Len(FilNavn) - (i - 1), i - 2))
                    i = i
                End If
                If charval = "-" Then
                    ScanNr = Val(Strings.Mid(FilNavn, Len(FilNavn) - (i + 1), 2))
                    'TextBox1.Text = Str(ScanNr) + " - " + Str(Row_Cnt)
                    If ScanNr > 9 Then
                        'ScanAntal = ScanNr
                    End If
                    'NewName = Strings.Left(FilNavn, Len(FilNavn) - (i + 1)) + Str(antal) + ".txt"
                    i = lgd
                End If
            Next
            'Lbl_StrNo.Text = Str(Row_Cnt)
            'TextBox3.Text = FilNavn
            GetTxtFile(2, FilNavn)
            ' ScalePict()
            ShowScan(Scan_Draw_Mode)

            Dim tete(2) As LinesFound
            'tete(1) = LineRegress1(2, 2, ValidMeas)
            'tete(2) = LineRegress1(2, ValidMeas, 2)

            Analyze_sel.Enabled = True
            If Analyze = True Then
                Analyze_Func("full")
            End If
        End If

    End Sub

    Private Sub ScannerONToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScannerONToolStripMenuItem.Click
        LaserPower("ON")
    End Sub

    Private Sub ScannerOFFToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScannerOFFToolStripMenuItem.Click
        LaserPower("OFF")
    End Sub
End Class
