Imports System
Imports System.IO
Imports System.Data
Imports System.Text
Imports System.Math


Public Module FindLine

    Dim PI As Double = 3.1415926535

    Dim MaxError1 As Double = 0.3        ' den maximale afvigelse af et punkt fra den fundne linje gælder for den vandrette flade
    Dim MaxError2 As Double = 0.2        ' den maximale afvigelse af et punkt fra den fundne linje gælder for fugesiderne

    ' variabler med midlertidige beregnede værdier
    Dim X_Act, Z_Act As Double

    ' variabler der indeholder "konstante" værdier (men som kan ændres)
    Dim MinLineLgt As Integer = 60    ' minimum af antal målinger der skal bruges til at identificere en linie.  der går ca.10-12 punkter på 1 mm


    '----------------------------------------------------------------------------------------------------------------------




    ' funktionen finder starten af fugen vha. linear regression (bruges til de  2 vandrette linjer)
    Public Function LineRegress1(ByVal LineNr As Integer, ByVal StartNo As Integer, ByVal SlutNo As Integer) As LinesFound
        Dim MaxTillError As Double = MaxError1

Start:
        Dim X_Sum As Double = 0
        Dim X_Mid As Double = 0
        Dim Z_Sum As Double = 0
        Dim Z_Mid As Double = 0

        Dim XVal_Arr(110) As Double
        Dim ZVal_Arr(110) As Double

        Dim Xm_Sum1 As Double = 0
        Dim Zm_Sum1 As Double = 0
        Dim XMid_Arr(110) As Double
        Dim ZMid_Arr(110) As Double
        Dim n As Integer = 0
        Dim Sel_Point As Integer
        Dim Arr_Index As Integer
        Dim Sign As Integer
        Dim Dist As Double

        Dim b0, b1 As Double
        'Dim content As String
        'Dim FILE_NAME As String = "C:\Data\test\fil7-" + Str(LineNr) + ".txt"
        'Dim objWriter2 As New System.IO.StreamWriter(FILE_NAME)

        Dim EOL_Cnt As Integer = 0

        If StartNo < SlutNo Then
            Sign = 1
        Else
            Sign = -1
        End If



        For i = StartNo To SlutNo Step Sign
            n = n + 1
            X_Act = Laser(LineNr, i).X_Pos / 100
            Z_Act = Laser(LineNr, i).Z_Pos / 100

            If n <= MinLineLgt Then
                ' her findes udgangsliningen for den videre analyse af den rette linje og hvornår den slutter
                ' beregningen bygger på lineær regression (least square method)
                Arr_Index = n
                XVal_Arr(n) = X_Act
                ZVal_Arr(n) = Z_Act

                X_Sum = X_Sum + X_Act       ' summen af X
                Z_Sum = Z_Sum + Z_Act       ' summen af Z
                X_Mid = X_Sum / n           ' gennemsnittet af X
                Z_Mid = Z_Sum / n           ' gennemsnittet af Z
                XMid_Arr(n) = X_Mid
                ZMid_Arr(n) = Z_Mid
                Xm_Sum1 = Xm_Sum1 + X_Mid   ' summen af X's gennemsnit
                Zm_Sum1 = Zm_Sum1 + Z_Mid   ' summen af Z's gennemsnit

                '    content = Str(n) + "; " + Str(X_Act) + "; " + Str(X_Sum) + "; " + Str(Z_Sum) + "; " + Str(Xm_Sum1) + "; " + Str(X_Mid) + "; " + Str(Z_Mid)
                '    objWriter2.WriteLine(content)

                '     linjens ligning hedder: Y = aX + b
                b1 = ((X_Sum - Xm_Sum1) * (Z_Sum - Zm_Sum1)) / ((X_Sum - Xm_Sum1) * (X_Sum - Xm_Sum1))  ' hældningen af linjen (a)
                b0 = Z_Mid - (b1 * X_Mid)                                                               ' skæringspunktet med Y-aksen (b)
                'Zh = b0 + (b1 * X_Mid)                                                                   
                'dZ = Zh - Z_Act

                'FORMEL: Dist = abs(ax + b - y) / (Math.Sqrt(a^2 + 1)
                Dist = Abs(b1 * X_Act + b0 - Z_Act) / Math.Sqrt(b1 ^ 2 + 1)                             ' afstanden mellem det aktuelle punkt og det's X-værdi's repræsentation på den fundne linje

            Else
                b1 = ((X_Sum - Xm_Sum1) * (Z_Sum - Zm_Sum1)) / ((X_Sum - Xm_Sum1) * (X_Sum - Xm_Sum1))  ' hældning af linjen            (a)
                b0 = Z_Mid - (b1 * X_Mid)                                                               ' skæringspunkt med Y-aksen     (b)
                'Zh = b0 + (b1 * X_Mid)
                'dZ = Zh - Z_Act

                'FORMEL: Dist = abs(ax + b - y) / (Math.Sqrt(a^2 + 1)
                Dist = Abs(b1 * X_Act + b0 - Z_Act) / Math.Sqrt(b1 ^ 2 + 1)

                If Dist < MaxTillError Then

                    XVal_Arr(Arr_Index) = X_Act
                    ZVal_Arr(Arr_Index) = Z_Act

                    ' genberegner værdierne der skal bruges til at analysere det nye punkts afvigelse fra linjen.
                    ' der arbejdes hele tiden med et fast antal målinger [MinLineLgt] således at den første måling fratrækkes og den sidste lægges til.
                    ' således bliver der hele tiden sammenlignet med et kort linjestykke, da længere linjestykker har en større afvigelse på grund af svøbets radius.

                    Arr_Index = n - (Int(n / MinLineLgt) * MinLineLgt) + 1
                    X_Sum = X_Sum - XVal_Arr(Arr_Index)     ' træk den gamle værdi fra summen
                    XVal_Arr(Arr_Index) = X_Act             ' gem den nye værdi
                    X_Sum = X_Sum + XVal_Arr(Arr_Index)     ' læg den nye værdi til summen

                    Z_Sum = Z_Sum - ZVal_Arr(Arr_Index)
                    ZVal_Arr(Arr_Index) = Z_Act
                    Z_Sum = Z_Sum + ZVal_Arr(Arr_Index)

                    X_Mid = X_Sum / MinLineLgt
                    Xm_Sum1 = Xm_Sum1 - XMid_Arr(Arr_Index)
                    XMid_Arr(Arr_Index) = X_Mid
                    Xm_Sum1 = Xm_Sum1 + XMid_Arr(Arr_Index)

                    Z_Mid = Z_Sum / MinLineLgt
                    Zm_Sum1 = Zm_Sum1 - ZMid_Arr(Arr_Index)
                    ZMid_Arr(Arr_Index) = Z_Mid
                    Zm_Sum1 = Zm_Sum1 + ZMid_Arr(Arr_Index)


                    b1 = ((X_Sum - Xm_Sum1) * (Z_Sum - Zm_Sum1)) / ((X_Sum - Xm_Sum1) * (X_Sum - Xm_Sum1))
                    b0 = Z_Mid - (b1 * X_Mid)

                    Sel_Point = i
                    EOL_Cnt = 0

                Else

                    ' Hvis afstanden mellem det aktuelle punkt og det's X-værdi's repræsentation på den fundne linje bliver større end den ønskede Max værdi så har vi fundet linjens afslutning
                    ' det sker ikke kun efter en enkel måling men efter et antal på hinanden følgende punkter (20) der ligger udenfor grænsen.
                    EOL_Cnt = EOL_Cnt + 1
                    n = n - 1
                    If EOL_Cnt > 20 Then
                        ' hvis der er meget reflektion, så kan der findes en forkert linjeafslutning ved en kort linje. det klares ved at forøge den tilladelige fejl
                        Dim cntchk As Integer = i + (20 * Sign)
                        Dim chkval As Double = Abs(Laser(LineNr, cntchk).Z_Pos / 100 - Z_Act)
                        If chkval < 0.5 Then
                            'StartNo = i
                            If MaxTillError < 0.6 Then
                                MaxTillError = MaxTillError + 0.05
                                GoTo start
                            End If
                        End If

                        LineRegress1.a = b1
                        LineRegress1.b = b0
                        LineRegress1.SlutPkt.Nr = Sel_Point
                        LineRegress1.SlutPkt.X = Laser(LineNr, Sel_Point).X_Pos / 100
                        LineRegress1.SlutPkt.Z = (b1 * LineRegress1.SlutPkt.X + b0)
                        LineRegress1.SlutPkt.Success = True

                        LineRegress1.StartPkt.Nr = StartNo
                        LineRegress1.StartPkt.X = Laser(LineNr, StartNo).X_Pos / 100
                        LineRegress1.StartPkt.Z = (b1 * LineRegress1.StartPkt.X + b0)
                        LineRegress1.Angle = Atan(b1) * 180 / Math.PI
                        LineRegress1.StartPkt.Success = True

                        'Form1.MakeLine(LineRegress1, Color.Green)
                        'objWriter2.Close()
                        Return LineRegress1
                    End If
                End If
            End If
        Next
        'objWriter2.Close()
 
        LineRegress1.SlutPkt.Success = False
        LineRegress1.StartPkt.Success = False
        LineRegress1.a = b1
        LineRegress1.b = b0
    End Function



    ' funktionen finder starten af fugen vha. linear regression (bruges til V-fugens sider)
    ' for kommentærer se ovenfor i LineRegress1
    Public Function LineRegress2(ByVal LineNr As Integer, ByVal StartNo As Integer, ByVal SlutNo As Integer) As LinesFound
        Dim X_Sum As Double = 0
        Dim X_Mid As Double = 0
        Dim Z_Sum As Double = 0
        Dim Z_Mid As Double = 0

        Dim XVal_Arr(110) As Double
        Dim ZVal_Arr(110) As Double

        Dim Xm_Sum1 As Double = 0
        Dim Zm_Sum1 As Double = 0
        Dim XMid_Arr(110) As Double
        Dim ZMid_Arr(110) As Double
        Dim n As Integer = 0
        Dim Sel_Point As Integer
        Dim Arr_Index As Integer
        Dim Sign As Integer
        Dim Dist As Double

        Dim b0, b1 As Double
        'Dim content As String
        'Dim FILE_NAME As String = "C:\Data\test\fil5-" + Str(LineNr) + ".txt"
        'Dim objWriter1 As New System.IO.StreamWriter(FILE_NAME)

        Dim EOL_Cnt As Integer = 0

        ' næste linje starter nogle punkter efter den fundne kant
        If StartNo < SlutNo Then
            Sign = 1
            StartNo = StartNo + 10
        Else
            Sign = -1
            StartNo = StartNo - 10
        End If
        '---------------------------------------------------


        For i = StartNo To SlutNo Step Sign
            n = n + 1
            X_Act = Laser(LineNr, i).X_Pos / 100
            Z_Act = Laser(LineNr, i).Z_Pos / 100

            If n <= 20 Then

                Arr_Index = n
                XVal_Arr(n) = X_Act
                ZVal_Arr(n) = Z_Act

                X_Sum = X_Sum + X_Act
                Z_Sum = Z_Sum + Z_Act
                X_Mid = X_Sum / n
                Z_Mid = Z_Sum / n
                XMid_Arr(n) = X_Mid
                ZMid_Arr(n) = Z_Mid
                Xm_Sum1 = Xm_Sum1 + X_Mid
                Zm_Sum1 = Zm_Sum1 + Z_Mid

                'content = Str(n) + "; " + Str(X_Act) + "; " + Str(X_Sum) + "; " + Str(Z_Sum) + "; " + Str(Xm_Sum1) + "; " + Str(X_Mid) + "; " + Str(Z_Mid)
                'objWriter1.WriteLine(content)

                b1 = ((X_Sum - Xm_Sum1) * (Z_Sum - Zm_Sum1)) / ((X_Sum - Xm_Sum1) * (X_Sum - Xm_Sum1))
                b0 = Z_Mid - (b1 * X_Mid)
                'Zh = b0 + (b1 * X_Mid)
                'dZ = Zh - Z_Act

                'Dist = abs(ax + b - y) / (Math.Sqrt(a^2 + 1)
                Dist = Abs(b1 * X_Act + b0 - Z_Act) / Math.Sqrt(b1 ^ 2 + 1)
                Dist = Dist

            Else

                b1 = ((X_Sum - Xm_Sum1) * (Z_Sum - Zm_Sum1)) / ((X_Sum - Xm_Sum1) * (X_Sum - Xm_Sum1))  ' hældning af linjen            (a)
                b0 = Z_Mid - (b1 * X_Mid)                                                               ' skæringspunkt med Y-aksen     (b)
                'Zh = b0 + (b1 * X_Mid)
                'dZ = Zh - Z_Act

                Dist = Abs(b1 * X_Act + b0 - Z_Act) / Math.Sqrt(b1 ^ 2 + 1)
                Dim Dist1 = (b1 * X_Act + b0 - Z_Act) / Math.Sqrt(b1 ^ 2 + 1) * (-Sign)

                'If Dist < MaxError2 Then
                If ((Sign = 1) And (Dist1 > (MaxError2 * -1))) Or ((Sign = -1) And (Dist1 < MaxError2)) Then

                    XVal_Arr(Arr_Index) = X_Act
                    ZVal_Arr(Arr_Index) = Z_Act

                    X_Sum = X_Sum + X_Act                   ' læg den nye værdi til summen

                    Z_Sum = Z_Sum + Z_Act

                    X_Mid = X_Sum / n
                    Xm_Sum1 = Xm_Sum1 + X_Mid

                    Z_Mid = Z_Sum / n
                    Zm_Sum1 = Zm_Sum1 + Z_Mid

                    b1 = ((X_Sum - Xm_Sum1) * (Z_Sum - Zm_Sum1)) / ((X_Sum - Xm_Sum1) * (X_Sum - Xm_Sum1))
                    b0 = Z_Mid - (b1 * X_Mid)
                    'Zh = b0 + (b1 * X_Mid)
                    'dZ = Zh - Z_Act

                    'content = Str(i) + "; " + Str(n) + "; " + Str(X_Act) + "; " + Str(X_Sum) + "; " + Str(Z_Sum) + "; " + Str(Xm_Sum1) + "; " + Str(X_Mid) + "; " + Str(Z_Mid) + "; " + Str(Dist)
                    'objWriter1.WriteLine(content)

                    Sel_Point = i
                    EOL_Cnt = 0
                Else
                    EOL_Cnt = EOL_Cnt + 1
                    n = n - 1
                    If EOL_Cnt > 20 Then
                        LineRegress2.a = b1
                        LineRegress2.b = b0
                        LineRegress2.SlutPkt.Nr = Sel_Point
                        LineRegress2.SlutPkt.X = Laser(LineNr, Sel_Point).X_Pos / 100
                        LineRegress2.SlutPkt.Z = (b1 * LineRegress2.SlutPkt.X + b0)

                        LineRegress2.StartPkt.Nr = StartNo
                        LineRegress2.StartPkt.X = Laser(LineNr, StartNo).X_Pos / 100
                        LineRegress2.StartPkt.Z = (b1 * LineRegress2.StartPkt.X + b0)
                        LineRegress2.Angle = Atan(b1) * 180 / Math.PI
                        'Form1.MakeLine(LineRegress2, Color.Green)
                        'objWriter1.Close()
                        Return LineRegress2
                    End If
                End If
            End If
        Next
        'objWriter1.Close()
        LineRegress2.a = b1
        LineRegress2.b = b0
        LineRegress2.SlutPkt.Nr = Sel_Point
        LineRegress2.SlutPkt.X = Laser(LineNr, Sel_Point).X_Pos / 100
        LineRegress2.SlutPkt.Z = (b1 * LineRegress2.SlutPkt.X + b0)

        LineRegress2.StartPkt.Nr = StartNo
        LineRegress2.StartPkt.X = Laser(LineNr, StartNo).X_Pos / 100
        LineRegress2.StartPkt.Z = (b1 * LineRegress2.StartPkt.X + b0)
        'Form1.MakeLine(LineRegress2, Color.Green)
        'objWriter1.Close()
    End Function



    Public Function TK_Areal(ByVal A As Coord, ByVal B As Coord, ByVal C As Coord) As Double
        Dim Side_a As Double
        Dim Side_b As Double
        Dim Side_c As Double

        TK_Areal = 0

        Side_a = Sqrt((B.X - C.X) ^ 2 + (B.Z - C.Z) ^ 2)
        Side_b = Sqrt((A.X - C.X) ^ 2 + (A.Z - C.Z) ^ 2)
        Side_c = Sqrt((B.X - A.X) ^ 2 + (B.Z - A.Z) ^ 2)
        TK_Areal = Abs((A.X * (B.Z - C.Z)) + (B.X * (C.Z - A.Z)) + (C.X * (A.Z - B.Z))) / 2

        TK_Areal = TK_Areal
    End Function



    Public Function ScanID(ByVal Command As Integer) As Motion_Ctrl
        On Error GoTo ErrHandler

        Dim udgang(0) As Short
        Dim success As Boolean
        Scan_Draw_Mode = 1      ' moden ændres fra at vise filer til at vise de aktuelle scans

        ScanID.ErrorNo = 0
        ScanID.Motion_Dir = 0
        ScanID.Motion_Dist = 0
        ScanID.Success = False
        '   ---------------
        Dim ID_Point(9) As Coord
        Dim X_Sum As Double = 0
        Dim Z_Sum As Double = 0
        Dim X_Chk As Double = 0
        Dim Z_Chk As Double = 0
        Dim Z_Diff As Double = 0
        Dim Z_Offs As Double = 0
        Dim X_Offs As Double = 0
        Dim val As Integer = 0
        Dim Surface_Zval As Double = 400
        Dim A1, A2, A3 As Double        ' Initialisering
        'Dim Reg2 As Integer = RobotVal(2).NumRegInt

        Dim ScanNummer As Integer = RobotVal(3).NumRegInt
        Dim SvejseNummer As Integer = RobotVal(5).NumRegInt     ' registernummer 5 indeholder svejsningens nummer
        Dim StrengNummer As Integer = RobotVal(2).NumRegInt     ' funktionen kaldes med streng nummeret (svejsestrengens nummer) i register 2
        Dim errVal As Integer = 0

        Form1.TextBox3.Text = Str(Form1.TextBox3.Text + 1)


        If ValidMeas > 400 Then

            ' find midlede værdier for X og Z i begge ender af den skannede linje
            For i = 1 To 21 Step 5
                X_Sum = X_Sum + Laser(ActLine, i).X_Pos
                Z_Sum = Z_Sum + Laser(ActLine, i).Z_Pos
                X_Chk = X_Chk + Laser(ActLine, i + 100).X_Pos
                Z_Chk = Z_Chk + Laser(ActLine, i + 100).Z_Pos
            Next
            ' startpunkt som er fundet ved at midle 5 værdier i starten
            ID_Point(1).X = X_Sum / 5 / 100
            ID_Point(1).Z = Z_Sum / 5 / 100
            ID_Point(2).X = X_Chk / 5 / 100
            ID_Point(2).Z = Z_Chk / 5 / 100

            '---------------------

            X_Sum = 0
            Z_Sum = 0
            X_Chk = 0
            Z_Chk = 0

            For i = ValidMeas - 1 To (ValidMeas - 22) Step -5
                X_Sum = X_Sum + Laser(ActLine, i).X_Pos
                Z_Sum = Z_Sum + Laser(ActLine, i).Z_Pos
                X_Chk = X_Chk + Laser(ActLine, i - 100).X_Pos
                Z_Chk = Z_Chk + Laser(ActLine, i - 100).Z_Pos
            Next
            ' slutpunkt som er fundet ved at midle 5 værdier i enden
            ID_Point(3).X = X_Sum / 5 / 100
            ID_Point(3).Z = Z_Sum / 5 / 100
            ID_Point(4).X = X_Chk / 5 / 100
            ID_Point(4).Z = Z_Chk / 5 / 100

            '---------------------
            ' finder et midtpunkt
            X_Sum = 0
            Z_Sum = 0

            For i = ((ValidMeas / 2) - 11) To ((ValidMeas / 2) + 11) Step 5
                X_Sum = X_Sum + Laser(ActLine, i).X_Pos
                Z_Sum = Z_Sum + Laser(ActLine, i).Z_Pos
            Next
            ' slutpunkt som er fundet ved at midle 5 værdier i enden
            ID_Point(5).X = X_Sum / 5 / 100
            ID_Point(5).Z = Z_Sum / 5 / 100
            '---------------------


            ' De 4 punkter i begge ender er fundet herover

            ' finder ud af hvilken værdi der er den mindste, det er nemlig overfladens Z-værdi
            For i = 1 To 4
                If ID_Point(i).Z < Surface_Zval Then Surface_Zval = ID_Point(i).Z
            Next

            ' find hældningsvinklen mellem de 2 første punkter, de 2 sidste punkter
            ' det bruges til at finde ud af om begge sider af skanningen ligger på ydersiden af røret og ikke i svejsefugen
            A1 = Atan((ID_Point(2).Z - ID_Point(1).Z) / (ID_Point(2).X - ID_Point(1).X)) * 180 / Math.PI    ' højre side -> de første værdier i scan arrayet (1..1280)
            A2 = Atan((ID_Point(3).Z - ID_Point(4).Z) / (ID_Point(3).X - ID_Point(4).X)) * 180 / Math.PI    ' venstre side -> de sidste værdier i scan arrayet (1280 .. 1)     
            A3 = Atan((ID_Point(1).Z - ID_Point(3).Z) / (ID_Point(1).X - ID_Point(3).X)) * 180 / Math.PI '   ---- beregninger af punkterne og vinklerne der bruges i funktionen ----

        End If


        ' 1) Søg efter emnet i Z-retningen
        If Command = 1 Then    ' Søg efter emnet i Z-retningen
            LaserPower("ON")

            Form1.ScannerStatus.Text = "Søger efter højden af svøbet!"
            '   ---------------
            Scan_Distance = RobotVal(2).NumRegInt       ' NumRegInt ligger fra reg 1 til 5, NumRegReal ligger fra reg 6 til 10

            If ValidMeas < 400 Then
                ScanID.Task = 1                         ' Motion task
                ScanID.ErrorNo = 9000                   ' skanningen er ikke valid - der er nok for stor afstand (ValidMeas er antallet af gode punkter ud af 1280)
                ScanID.Motion_Dir = -3                  ' kør ned i Z-retningen
                ScanID.Motion_Dist = 30                 ' 30mm
                ScanID.Success = True
                Return (ScanID)
            End If


            If Surface_Zval > (Scan_Distance + 5) Then
                ScanID.Task = 1                         ' Motion task
                ScanID.ErrorNo = 0
                ScanID.Motion_Dir = -3                   ' kør ned i Z-retningen
                ScanID.Motion_Dist = Surface_Zval - Scan_Distance     ' afstanden skal være ca. 370mm
                ScanID.Success = False
            End If
            If Surface_Zval < (Scan_Distance - 5) Then
                ScanID.Task = 1                         ' Motion task
                ScanID.ErrorNo = 0
                ScanID.Motion_Dir = 3                   ' kør op i Z-retningen
                ScanID.Motion_Dist = Scan_Distance - Surface_Zval     ' afstanden skal være ca. 370mm
                ScanID.Success = False
            End If
            If Abs(Surface_Zval - Scan_Distance) < 5 Then
                ScanID.Task = 2                         ' terminerer Motion task
                ScanID.ErrorNo = 0
                ScanID.Motion_Dir = 0                   ' kør op i Z-retningen
                ScanID.Motion_Dist = 0                  ' afstanden skal være ca. Scan_Distance [mm]
                ScanID.Success = True


                SetPosRegX(RobotVal(1).CartPos, 1)
            End If
            MinZ_Compare = MinZ
            RotationStartVal = ID_Point(1).Z - ID_Point(4).Z           ' denne værdi er startværdien for højdeforskellen mellem de to yderpunkter på scanneren
        End If


        ' 2) find midten af svejsefugen i X-retningen
        If Command = 2 Then
            LaserPower("ON")

            Form1.ScannerStatus.Text = "Søger efter fugemidten!"

            '   ---------------
            Dim GrooveVal(3) As Coord
            Dim GrooveMid As Double
            Dim Afstand As Double


            Dim Kant(2) As LinesFound
            Kant(1) = LineRegress2(1, 2, ValidMeas)                     ' finder de 2 kanter af fugen ved hjælp af regression
            Kant(2) = LineRegress2(1, ValidMeas, 2)

            If Abs(Kant(1).SlutPkt.Z - Kant(2).SlutPkt.Z) < 5 Then      'når begge linjers Z-værdier er lige høje så har den fundet de to vandrette linjer
                Afstand = (Kant(1).SlutPkt.X + Kant(2).SlutPkt.X) / 2
                ScanID.Motion_Dir = 1                       ' kør ud i X-retningen - fortegnet på nn1 bestemmer

            End If
            If (Kant(1).SlutPkt.Z - Kant(2).SlutPkt.Z) > 5 Then         ' når den 
                ScanID.Motion_Dir = 1                       ' kør ud i -X-retningen
                Afstand = Kant(1).SlutPkt.X
            End If
            If (Kant(2).SlutPkt.Z - Kant(1).SlutPkt.Z) > 5 Then
                Afstand = Kant(2).SlutPkt.X
                ScanID.Motion_Dir = 1                       ' kør ud i X-retningen 
            End If

            If Abs(Kant(1).SlutPkt.X - Kant(2).SlutPkt.X) < 5 Then        ' har fundet samme punkt så
                If Abs(Kant(1).Angle) > Abs(Kant(2).Angle) Then
                    Afstand = Kant(2).StartPkt.X
                    ScanID.Motion_Dir = 1                       ' kør ud i -X-retningen
                Else
                    Afstand = Kant(1).StartPkt.X
                    ScanID.Motion_Dir = 1                       ' kør ud i -X-retningen
                End If
            End If



            If Abs(Afstand) > 3 Then
                ScanID.Motion_Dist = Afstand
                ScanID.Task = 1                         ' Motion task
            Else
                ScanID.Success = True
                ScanID.Motion_Dir = 0
                ScanID.Task = 2                         ' terminate
            End If


            Return (ScanID)

        End If      ' find midten af svejsefugen i X-retningen      


        ' 3) Søg efter Starten i -Z-retningen
        If Command = 3 Then
            LaserPower("ON")

            Form1.ScannerStatus.Text = "Søger efter forkanten af svøbet!"

            If DigSearch = True Then
                If (ValidMeas < 400) Or ((MinZ - 1500) > MinZ_Compare) Then         ' enden er fundet

                    ScanID.Task = 4
                    ScanID.Motion_Dir = 0
                    ScanID.ErrorNo = 0
                    ScanID.Motion_Dist = 0
                    ScanID.Success = True
                    udgang(0) = 0
                    success = cmdSetSDO(1, udgang)
                    success = cmdSetSDO(2, udgang)
                    success = cmdSetSDO(3, udgang)
                    DigSearch = False
                Else
                    ScanID.Task = 4
                    ScanID.Motion_Dir = 0
                    ScanID.ErrorNo = 0
                    ScanID.Motion_Dist = 0
                    ScanID.Success = True
                End If

            Else

                ScanID.Task = 3
                ScanID.Motion_Dir = 0
                ScanID.ErrorNo = 0
                ScanID.Motion_Dist = -RobotVal(2).NumRegInt
                ScanID.Success = False
                MinZ_Compare = MinZ

            End If
        End If



        ' 4) Søg efter enden af svejsningen i Z-retningen
        If Command = 4 Then
            LaserPower("ON")

            Form1.ScannerStatus.Text = "Søger efter bagkanten af svøbet!"

            If DigSearch = True Then
                If ValidMeas < 400 Or ((MinZ - 1500) > MinZ_Compare) Then         ' enden er fundet
                    ScanID.Task = 4
                    ScanID.Motion_Dir = 0
                    ScanID.ErrorNo = 0
                    ScanID.Motion_Dist = 0
                    ScanID.Success = True
                    udgang(0) = 0
                    ErrorTracker = 11
                    success = cmdSetSDO(1, udgang)
                    success = cmdSetSDO(2, udgang)
                    success = cmdSetSDO(3, udgang)
                    DigSearch = False
                Else
                    ScanID.Task = 4
                    ScanID.Motion_Dir = 0
                    ScanID.ErrorNo = 0
                    ScanID.Motion_Dist = 0
                    ScanID.Success = True
                End If

            Else
                ScanID.Task = 3
                ScanID.Motion_Dir = 0
                ScanID.ErrorNo = 0
                ScanID.Motion_Dist = RobotVal(2).NumRegInt
                ScanID.Success = False
                MinZ_Compare = MinZ
            End If
        End If


        ' 5) finder 3 punkter og midler
        If Command = 5 Then
            LaserPower("ON")

            Form1.ScannerStatus.Text = "Finder punkter på fugen!"
            ' resetter forskydning og krymp beregningerne
            SetRealReg(12, 0)
            SetRealReg(13, 0)
            SetInit_X(442, 0)
            SetInit_X(443, 0)
            SetInit_X(444, 0)   ' sætter error registret til 0
            ' -----------------

            ' -----------------
            If Robot_Moving = False Then
                delay(300)
                If Form1.Analyze_Func("full") = True Then
                    ' Form1.UpLoadPict()

                    If ScanCnt > 2 Then     ' 3 successfull scan

                        'ArealCheck(Punkt nummer, scan nummer (ud af 3)).Areal
                        ArealCheck(ScanNummer, 3).Areal = Areal_Beregn          ' Areal_Beregn er beregnet i Analyze_Func
                        ArealCheck(ScanNummer, 3).Pkt1 = RobotVal(2).CartPos    ' punkterne er ligeledes fundet i Analyze_Func
                        ArealCheck(ScanNummer, 3).Pkt2 = RobotVal(3).CartPos
                        ArealCheck(ScanNummer, 3).Pkt3 = RobotVal(4).CartPos


                        ArealCheck(ScanNummer, 4) = Evaluate_Scan(ScanNummer)     ' vælger det bedste scan

                        SetPosRegX(ArealCheck(ScanNummer, 4).Pkt1, 1)            ' gemmer de valgte punkter i registre
                        SetPosRegX(ArealCheck(ScanNummer, 4).Pkt2, 2)
                        SetPosRegX(ArealCheck(ScanNummer, 4).Pkt3, 3)
                        SetScanData(ScanNummer, MeasuredArea)           ' skriv Scanvolumen i register

                        ' ----- nedenstående bruges til at finde en forskydning

                        Dim TopLines(2) As LinesFound
                        TopLines(1) = FindLinje(1, Math.Round(ValidMeas * 2 / 3), 1, 1.0, 4, 5, 100)
                        TopLines(2) = FindLinje(ValidMeas, Math.Round(ValidMeas / 3), -1, 1.0, 4, 5, 100)

                        'TopLines(1) = LineRegress1(1, 2, ValidMeas)         ' vi finder X-værdierne for svejsefugen
                        'TopLines(2) = LineRegress1(1, ValidMeas, 2)         ' fra begge sider

                        SetInit_X(399 + (ScanNummer * 2), TopLines(1).SlutPkt.X)                ' gemmer værdierne i nogle registre så vi kan genstarte programmet uden problemer
                        SetInit_X(400 + (ScanNummer * 2), TopLines(2).SlutPkt.X)                ' x-værdierne for fugen gemmes i registre til efterfølgende sammenligning

                        SetIniHeights((ScanNummer + 370), Laser(ActLine, 100).Z_Pos / 100)      ' gemmer højden i andre registre


                        FilNavn = "c:\Scans\" + "Str-" + Str(SvejseNummer) + "-" + Str(ScanNummer) + "-" + Str(StrengNummer) + ".txt"
                        skrivTxtfil(FilNavn)
                        ' husk at checke for folderen

                        ScanID.Task = 2
                        ScanID.Motion_Dir = 0
                        ScanID.ErrorNo = 0
                        ScanID.Motion_Dist = 0
                        ScanID.Success = True
                        'Form1.UpLoadPict()

                    Else
                        ScanCnt = ScanCnt + 1

                        ScanID.Task = 1                         ' Motion task
                        ScanID.ErrorNo = 0
                        ScanID.Motion_Dir = 2                   ' kør op i Z-retningen
                        ScanID.Motion_Dist = 5                 ' afstanden skal være ca. 5mm
                        ScanID.Success = False
                        ArealCheck(ScanNummer, ScanCnt).Areal = Areal_Beregn
                        ArealCheck(ScanNummer, ScanCnt).Pkt1 = RobotVal(2).CartPos      ' X1 tættest robotten
                        ArealCheck(ScanNummer, ScanCnt).Pkt2 = RobotVal(3).CartPos      ' X2 længst væk
                        ArealCheck(ScanNummer, ScanCnt).Pkt3 = RobotVal(4).CartPos      ' X3 midten
                        MaesArea(ScanCnt) = MeasuredArea
                    End If

                    ' ******* CHECKER SKANNET
                    Dim TestScan As Scan_Result = Scan_Chk()
                    Scan_Valid(ScanNummer, ScanCnt) = TestScan



                    ' **** HVIS FUGEN ER FOR STOR ELLER FOR LILLE
                    If (FugeHgtVal > 15) Then
                        If (TestScan.FugeAng > 85) Or (TestScan.FugeAng < 60) Then
                            errVal = 6
                        End If
                    Else
                        If (TestScan.FugeAng > 95) Or (TestScan.FugeAng < 60) Then
                            errVal = 6
                        End If

                    End If


                    '**** HVIS HØJDE / BREDDE FORHOLDET ER FOR STORT ELLER LILLE (derfor ABS())
                    If (FugeHgtVal > 15) Then
                        If (Abs(TestScan.Hgt_Div) > 24) Then
                            If errVal = 0 Then
                                errVal = 7
                            Else
                                errVal = errVal + 2
                            End If

                        End If
                    Else
                        If (Abs(TestScan.Hgt_Div) > 35) Then
                            If errVal = 0 Then
                                errVal = 7
                            Else
                                errVal = errVal + 2
                            End If

                        End If
                    End If
                    '**** HVIS CENTERFORSKYDNINGEN ER FOR STOR (DOG MINDST 1.5MM)
                    If (Abs(TestScan.Mid_Div) > 10) And (Abs(TestScan.MidtForskyd) > 1.5) Then
                        If errVal = 0 Then
                            'errVal = 9
                        Else
                            'errVal = errVal + 4
                        End If
                    End If

                    If errVal > 0 Then
                        SetInit_X(444, errVal + 600)
                    Else
                        SetInit_X(444, 0)
                    End If

                Else
                    ' hvis skanningen ikke er successful
                    ScanID.Task = 1                         ' Motion task
                    ScanID.ErrorNo = 0
                    ScanID.Motion_Dir = 2                   ' kør i Y-retningen
                    ScanID.Motion_Dist = 5                 ' afstanden skal være ca. 5mm
                    ScanID.Success = False

                End If

            End If
        End If


        ' 6) beregn vinklen af emnet

        If Command = 6 Then
            LaserPower("ON")

            Form1.ScannerStatus.Text = "Finder vinklen af emnet!"

            ScanID.Task = 5
            ScanID.Motion_Dir = 0
            ScanID.ErrorNo = 0
            ScanID.Motion_Dist = 0
            Form1.TextBox2.Text = Format(A3, "####.0")
            SetRealReg(7, A3)
            ScanID.Success = False
        End If


        ' 7) Søg efter svejsefugen ved rotation

        If Command = 7 Then
            LaserPower("ON")

            Form1.ScannerStatus.Text = "Søger fugen under rotation!"
            '   ---------------

            ScanID.Task = 5                         ' holder loop'et kørende
            ScanID.ErrorNo = 0
            ScanID.Motion_Dir = 0                   ' kør op i Z-retningen
            ScanID.Motion_Dist = 0                  ' afstanden skal være ca. Scan_Distance [mm]
            ScanID.Success = False
            SetRealReg(8, ID_Point(1).Z)
            SetRealReg(9, ID_Point(5).Z)
            SetRealReg(10, ID_Point(3).Z)
        End If      ' find midten af svejsefugen i X-retningen      


        ' 8) angiv højden fra skanneren ned til røret - bruges til positionering af bommen
        If Command = 8 Then
            LaserPower("ON")


            Form1.ScannerStatus.Text = "Viser højden ved bomplacering!"

            If ValidMeas > 400 Then
                If Surface_Zval < 400 Then
                    ScanID.Task = 5                         ' holder loop'et kørende
                    ScanID.ErrorNo = 0
                    ScanID.Motion_Dir = 0                   ' 
                    ScanID.Motion_Dist = 0                  ' 
                    ScanID.Success = False

                    SetRealReg(6, Surface_Zval)
                End If
            Else
                SetRealReg(6, 500)
            End If

        End If

        ' 9) flyt tårnet til midten af svøbet
        If Command = 9 Then
            LaserPower("ON")

            Form1.ScannerStatus.Text = "Placerer tårnet!"

            Z_Diff = (ID_Point(3).Z - ID_Point(1).Z)
            ScanID.Task = 5                         ' holder loop'et kørende
            ScanID.ErrorNo = 0
            ScanID.Motion_Dir = 0                   ' kør op i Z-retningen
            ScanID.Motion_Dist = 0                  ' afstanden skal være ca. Scan_Distance [mm]
            ScanID.Success = False
            SetRealReg(8, Z_Diff)
        End If


        ' 10) find svejsepunkter
        If Command = 10 Then
            LaserPower("ON")
            '   ---------------
            Form1.ScannerStatus.Text = "Skanner svejsepunkt til fil!"

            Form1.Scan_Groove()
            FilNavn = "c:\Scans\" + "Str-" + Str(SvejseNummer) + "-" + Str(ScanNummer) + "-" + Str(StrengNummer) + ".txt"
            skrivTxtfil(FilNavn)

            ScanID.Task = 2
            ScanID.Motion_Dir = 0
            ScanID.ErrorNo = 0
            ScanID.Motion_Dist = 0
            ScanID.Success = True

        End If

        ' 11) find svejsepunkter
        If Command = 11 Then
            LaserPower("ON")
            Form1.ScannerStatus.Text = "finder svejsepunkter og gemmer dem!"

            Form1.Analyze_Func("full")

            SetPosRegX(RobotVal(2).CartPos, 1)
            SetPosRegX(RobotVal(3).CartPos, 2)
            SetPosRegX(RobotVal(4).CartPos, 3)
            SetScanData(ScanNummer, MeasuredArea)       ' skriv Scanvolumen i register
            SetScanData(ScanNummer + 14, FugeBundVinkel)       ' skriv FugeBundVinkel i register

            ScanID.Task = 2
            ScanID.Motion_Dir = 0
            ScanID.ErrorNo = 0
            ScanID.Motion_Dist = 0
            ScanID.Success = True

        End If

        ' 12) Tjek positionsoffset for svejsepunktet (fra f.eks. synkning af fugen)
        If Command = 12 Then
            LaserPower("ON")
            Form1.ScannerStatus.Text = "Checker positionsoffset!"

            delay(500)

            Form1.Scan_Groove()
            FilNavn = "c:\Scans\" + "Str-" + Str(SvejseNummer) + "-" + Str(ScanNummer) + "-" + Str(StrengNummer) + ".txt"
            skrivTxtfil(FilNavn)

            XZ_Offset(ScanNummer, StrengNummer)
            If Form1.Analyze_Func("top") = True Then
                SetScanData(ScanNummer, MeasuredArea)       ' skriv Scanvolumen i register
            End If


            ScanID.Task = 2
            ScanID.Motion_Dir = 0
            ScanID.ErrorNo = 0
            ScanID.Motion_Dist = 0
            ScanID.Success = True
            Form1.UpLoadPict()
        End If




        ' 13) find svejsepunkter
        If Command = 13 Then
            LaserPower("ON")
            Form1.ScannerStatus.Text = "finder svejsepunkter og gemmer dem!"

            Form1.Analyze_Func("full")

            SetPosRegX(RobotVal(2).CartPos, 1)
            SetPosRegX(RobotVal(3).CartPos, 2)
            SetPosRegX(RobotVal(4).CartPos, 3)
            SetScanData(ScanNummer, MeasuredArea)       ' skriv Scanvolumen i register
            SetScanData(ScanNummer + 14, FugeBundVinkel)       ' skriv FugeBundVinkel i register

            ScanID.Task = 2
            ScanID.Motion_Dir = 0
            ScanID.ErrorNo = 0
            ScanID.Motion_Dist = 0
            ScanID.Success = True

        End If

        ' 14) find svejsepunkter
        If Command = 14 Then

            LaserPower("ON")
            Form1.ScannerStatus.Text = "finder svejsepunkter og gemmer dem!"

            If Form1.Analyze_Func("top") = True Then

                SetPosRegX(RobotVal(2).CartPos, 1)
                SetPosRegX(RobotVal(3).CartPos, 2)
                SetPosRegX(RobotVal(4).CartPos, 3)
                'SetScanData(ScanNummer, MeasuredArea)
            End If

            If ValidMeas > 400 Then
                If Surface_Zval < 400 Then
                    SetRealReg(6, Surface_Zval)
                End If
            Else
                SetRealReg(6, 500)
            End If

            ScanID.Task = 5
            ScanID.Motion_Dir = 0
            ScanID.ErrorNo = 0
            ScanID.Motion_Dist = 0
            ScanID.Success = True

        End If

        ' 15) beregn X-forskydning og krympning af fugen
        If Command = 15 Then

            Dim lp As Integer
            For lp = 1 To ScanNummer
                FugeForskyd(lp) = XOffs_Arr(lp, 5)
                FugeKrymp(lp) = XOffs_Arr(lp, 6)
            Next

            Dim ActFugeForskyd As StatAnal = Gns_Calc(ScanNummer, FugeForskyd)
            Dim ActFugeKrymp As Double = Gns_Calc(ScanNummer, FugeKrymp).MidVal
            Dim NyForskydVal As Boolean = False
            Dim NyKrympVal As Boolean = False

            LastFugeForskyd = GetInit_X(442)
            LastFugeKrymp = GetInit_X(443)

            Dim dFugeForsk As Double = Abs(ActFugeForskyd.MidVal - LastFugeForskyd)
            Dim dFugeKrymp As Double = Abs(ActFugeKrymp - LastFugeKrymp)

            'ActFugeForskyd.MidVal = LastFugeForskyd    ' hvis de ikke bliver ændret beholder vi de gamle
            'ActFugeKrymp = LastFugeKrymp
            Form1.Error_Lbl.Text = "spredning = " + Str(ActFugeForskyd.Spredning)
            If (ActFugeForskyd.Spredning > 3) Then
                errVal = 4

            End If


            'If (ActFugeForskyd.Spredning < 3) And (ActFugeForskyd.Success = True) Then

            If StrengNummer > 8 Then                                                        ' fra streng nummer 9 og efterfølgende
                If (Abs(ActFugeForskyd.MidVal) < 13) And (Abs(dFugeForsk) < 1) Then         ' det maximale korrektion er på 13mm og forskellen til den forrige svejsning må max. være 1mm
                    NyForskydVal = True
                    SetInit_X(442, ActFugeForskyd.MidVal)
                Else

                    If Abs(dFugeForsk) > 1 Then
                        errVal = errVal + 1
                    End If
                    If Abs(ActFugeForskyd.MidVal) > 13 Then
                        errVal = errVal + 2
                    End If

                End If
                If (Abs(ActFugeKrymp) < 8) And (Abs(dFugeKrymp) < 0.7) Then                 ' fugekrympet (bruges endnu ikke)
                    NyKrympVal = True
                    SetInit_X(443, ActFugeKrymp)
                End If
            End If

            If (StrengNummer > 4) And (StrengNummer < 9) Then                               ' fra streng nummer 5 til 8 
                If (Abs(ActFugeForskyd.MidVal) < 11) And (Abs(dFugeForsk) < 2) Then       ' det maximale korrektion er på 11mm og forskellen til den forrige svejsning må max. være 1.5mm
                    NyForskydVal = True
                    SetInit_X(442, ActFugeForskyd.MidVal)
                Else

                    If Abs(dFugeForsk) > 2 Then
                        errVal = errVal + 1
                    End If
                    If Abs(ActFugeForskyd.MidVal) > 11 Then
                        errVal = errVal + 2
                    End If

                End If
                If (Abs(ActFugeKrymp) < 5) And (Abs(dFugeKrymp) < 1) Then                   ' fugekrympet (bruges endnu ikke)
                    NyKrympVal = True
                    SetInit_X(443, ActFugeKrymp)
                End If
            End If

            If StrengNummer < 5 Then                                                        ' fra streng 1 til 4
                If (Abs(ActFugeForskyd.MidVal) < 8) And (Abs(dFugeForsk) < 2.5) Then        ' det maximale korrektion er på 8mm og forskellen til den forrige svejsning må max. være 3,5mm
                    NyForskydVal = True
                    SetInit_X(442, ActFugeForskyd.MidVal)
                Else

                    If Abs(dFugeForsk) > 2.5 Then
                        errVal = errVal + 1
                    End If
                    If Abs(ActFugeForskyd.MidVal) > 8 Then
                        errVal = errVal + 2
                    End If

                End If
                If (Abs(ActFugeKrymp) < 4) And (Abs(dFugeKrymp) < 1.8) Then                 ' fugekrympet (bruges endnu ikke)
                    NyKrympVal = True
                    SetInit_X(443, ActFugeKrymp)
                End If
            End If



            If (ActFugeForskyd.Spredning < 3) And (ActFugeForskyd.Success = True) Then
            Else
                ' spredning er større end 2
                Form1.Error_Lbl.Text = "Command 15, spredning er større end 3!"
            End If

            If NyForskydVal = False Then ActFugeForskyd.MidVal = LastFugeForskyd
            If NyKrympVal = False Then ActFugeKrymp = LastFugeKrymp


            SetRealReg(12, ActFugeForskyd.MidVal)
            SetRealReg(13, ActFugeKrymp)


            ScanID.Task = 2
            ScanID.Motion_Dir = 0
            ScanID.ErrorNo = 0
            ScanID.Motion_Dist = 0
            ScanID.Success = True

        End If


        ' 16) finder fugen ved hurtig rotation ved at kigge på linjevinklen
        If Command = 16 Then
            LaserPower("ON")
            Dim FugeMidte As Double = AngleChk()
            SetRealReg(11, FugeMidte)

            If ValidMeas > 400 Then
                If Surface_Zval < 400 Then
                    SetRealReg(6, Surface_Zval)
                End If
            Else
                SetRealReg(6, 500)
            End If

        End If

        ' ***********************************************

        If Command = 0 Then
            Form1.ScannerStatus.Text = ""
        End If
        If errVal > 0 Then
            SetInit_X(444, errVal + 600)
        End If
        Return (ScanID)


        ' formlen for linjen der går gennem start og slutpunktet
        'Surfaceline.a = (ID_Point(3).Z - ID_Point(1).Z) / (ID_Point(3).X - ID_Point(1).X)
        'Surfaceline.b = ID_Point(3).Z - (Surfaceline.a * ID_Point(3).X)
ErrHandler:

        Dim nnn As Integer = 0

    End Function

 
    Public Function CalibOffs(ByVal OffsVal As Vector) As Vector
        On Error GoTo errhandler
        Global_Y_Start = 773
        Global_Y_Slut = 3341
        Global_X_Offs = 3.5
        Global_Z_Offs = 6.0
        Dim OffsParam As Double = ((OffsVal.Y - Global_Y_Start) / (Global_Y_Slut - Global_Y_Start))


        CalibOffs.X = OffsVal.X - (OffsParam * Global_X_Offs)
        CalibOffs.Z = OffsVal.Z - (OffsParam * Global_Z_Offs)
        CalibOffs.Y = OffsVal.Y
        Return CalibOffs

errhandler:
        CalibOffs.X = OffsVal.X
        CalibOffs.Z = OffsVal.Z
        CalibOffs.Y = OffsVal.Y

    End Function



    Public Sub XZ_Offset(ByVal ScanNummer As Integer, ByVal Plads As Integer)
        Dim X_Offs As Double = 0
        Dim Z_Offs As Double = 0

        ' henter de første X og Z værdier i registrene
        Dim Org_Z_Val1 As Double = GetIniHeights(ScanNummer + 370)
        ' find højden af svøbet ca. 5mm fra kanten af scannerlinjen
        Dim Act_Z_Val1 As Double = Laser(ActLine, 100).Z_Pos / 100
        Dim TopLines(2) As LinesFound


        Z_Offs = Org_Z_Val1 - Act_Z_Val1

        'finder linjerne først fra den ene side
        Dim StartNo As Integer = 20
        Dim SlutNo As Integer = ValidMeas - 20
        'TopLines(1) = LineRegress1(1, StartNo, SlutNo)
        TopLines(1) = FindLinje(1, Math.Round(ValidMeas * 2 / 3), 1, 1.0, 4, 5, 100)

        ' ....  så fra den anden side
        StartNo = ValidMeas - 20
        SlutNo = 20
        'TopLines(2) = LineRegress1(1, StartNo, SlutNo)
        TopLines(2) = FindLinje(ValidMeas, Math.Round(ValidMeas / 3), -1, 1.0, 4, 5, 100)
        '------------

        '        XOffs_Arr(n, m)  m 1 og 2 er de aktuelle X-værdier, 3 og 4 er de oprindelige X-værdier, 5 er forskydningen, 6 er krymp
        XOffs_Arr(ScanNummer, 1) = TopLines(1).SlutPkt.X                ' x-værdien fra den ene side
        XOffs_Arr(ScanNummer, 2) = TopLines(2).SlutPkt.X                ' x-værdien fra den anden side
        XOffs_Arr(ScanNummer, 3) = GetInit_X(399 + (ScanNummer * 2))    ' det aktuelle scan's udgangsværdi lægges her så vi undgår at hente alle scan's hver gang
        XOffs_Arr(ScanNummer, 4) = GetInit_X(400 + (ScanNummer * 2))
        XOffs_Arr(ScanNummer, 5) = ((XOffs_Arr(ScanNummer, 1) - XOffs_Arr(ScanNummer, 3)) + (XOffs_Arr(ScanNummer, 2) - XOffs_Arr(ScanNummer, 4))) / 2  ' forskydning af fugen
        XOffs_Arr(ScanNummer, 6) = (XOffs_Arr(ScanNummer, 3) - XOffs_Arr(ScanNummer, 4)) - (XOffs_Arr(ScanNummer, 1) - XOffs_Arr(ScanNummer, 2))      ' krympning af fugen
        Form1.ErrLabel2.Text = Str(XOffs_Arr(ScanNummer, 5)) + "    " + Str(XOffs_Arr(ScanNummer, 6))

        X_Offs = XOffs_Arr(ScanNummer, 5)

        Dim Calc_Offset As xyzwprExt

        Calc_Offset.X = X_Offs
        Calc_Offset.Y = 0
        Calc_Offset.Z = Z_Offs
        Calc_Offset.w = 0
        Calc_Offset.p = 0
        Calc_Offset.r = 0
        Calc_Offset.E1 = 0
        Calc_Offset.E2 = 0
        Calc_Offset.E3 = 0
        SetPosRegX(Calc_Offset, 4)


    End Sub


    ' --------------- funktion der beregner gennemsnittet af et array af værdier og sorterer 
    Public Function Gns_Calc(ByVal Antal As Integer, ByVal ArrVal() As Double) As StatAnal
        Dim Sum As Double
        Dim SortArr(20) As Double
        Dim LoopExit As Boolean = False
        Dim StartVal As Integer = 1
        Dim SlutVal As Integer = Antal
        Dim LoopCnt_GNS As Integer = 0
        Dim MaxVal As Double = ArrVal(1)
        Dim MinVal As Double = ArrVal(1)



        If Antal < 4 Then
            Gns_Calc.Success = False
            Exit Function
        End If

        ' ---------- Sorter værdierne efter størrelse - det gør det lettere at sortere yderværdierne fra
        For j = 1 To Antal
            MinVal = ArrVal(1)
            For i = 1 To Antal
                If ArrVal(i) < MinVal Then MinVal = ArrVal(i)
                If ArrVal(i) > MaxVal Then MaxVal = ArrVal(i)
            Next
            SortArr(j) = MinVal
            Dim Cnt_1 As Integer = 1
            While ArrVal(Cnt_1) <> MinVal
                Cnt_1 = Cnt_1 + 1
            End While
            ArrVal(Cnt_1) = MaxVal
            'For i = 1 To Antal
            'If ArrVal(i) = MinVal Then ArrVal(i) = MaxVal
            'Next
        Next
        ' ----------End Sort ----------------------



        For i = StartVal To SlutVal
            Sum = Sum + SortArr(i)
        Next

        Gns_Calc.MinVal = SortArr(StartVal)
        Gns_Calc.MaxVal = SortArr(SlutVal)

        Gns_Calc.MidVal = Sum / Antal
        Gns_Calc.Center = Gns_Calc.MinVal + ((Gns_Calc.MaxVal - Gns_Calc.MinVal) / 2)
        Gns_Calc.Spredning = Gns_Calc.MaxVal - Gns_Calc.MinVal
        ' Fordelingen er forskellen mellem middelværdien af max og min værdi, sat i relation til gennemsnitsværdien
        Gns_Calc.Fordeling = (Gns_Calc.MidVal - Gns_Calc.MinVal) * 100 / Gns_Calc.Spredning


        While LoopExit = False

            If LoopCnt_GNS > 3 Then         ' hvis værdierne ligger for spredt returneres her
                Gns_Calc.Success = False
                LoopExit = True
                Exit Function
            End If

            ' hvis midtpunktet mellem mellem middelværdien af max og min værdien, er væsentlig anderledes end gennemsnitsværdien, så skyldes det formentlig at der er ekstreme værdier
            If Gns_Calc.Fordeling <= 35 Then
                Sum = Sum - SortArr(SlutVal)
                SlutVal = SlutVal - 1
                Antal = Antal - 1
                Gns_Calc.MidVal = Sum / Antal
                Gns_Calc.MaxVal = SortArr(SlutVal)
                Gns_Calc.Center = Gns_Calc.MinVal + ((Gns_Calc.MaxVal - Gns_Calc.MinVal) / 2)
                Gns_Calc.Spredning = Gns_Calc.MaxVal - Gns_Calc.MinVal
                Gns_Calc.Fordeling = (Gns_Calc.MidVal - Gns_Calc.MinVal) * 100 / Gns_Calc.Spredning
                LoopCnt_GNS = LoopCnt_GNS + 1
            End If
            If Gns_Calc.Fordeling >= 65 Then
                Sum = Sum - SortArr(StartVal)
                StartVal = StartVal + 1
                Antal = Antal - 1
                Gns_Calc.MidVal = Sum / Antal
                Gns_Calc.MinVal = SortArr(StartVal)
                Gns_Calc.Center = Gns_Calc.MinVal + ((Gns_Calc.MaxVal - Gns_Calc.MinVal) / 2)
                Gns_Calc.Spredning = Gns_Calc.MaxVal - Gns_Calc.MinVal
                Gns_Calc.Fordeling = (Gns_Calc.MidVal - Gns_Calc.MinVal) * 100 / Gns_Calc.Spredning
                LoopCnt_GNS = LoopCnt_GNS + 1
            End If
            If (Gns_Calc.Fordeling > 35) And (Gns_Calc.Fordeling < 65) Then
                Gns_Calc.Success = True
                LoopExit = True
            End If
        End While

    End Function



    Public Function Evaluate_Scan(ByVal PktNr As Integer) As ScanAry
        Dim areal1 As Double = ArealCheck(RobotVal(3).NumRegInt, 1).Areal
        Dim areal2 As Double = ArealCheck(RobotVal(3).NumRegInt, 2).Areal
        Dim areal3 As Double = ArealCheck(RobotVal(3).NumRegInt, 3).Areal
        Dim DiffVal As Double

        Scan_Accepted = True

        Evaluate_Scan = Nothing
        ArealLimVal = 0.05

        If (Abs(areal1 - areal2)) < (Abs(areal1 - areal3)) And (Abs(areal1 - areal2)) < (Abs(areal2 - areal3)) Then
            Evaluate_Scan.Areal = (areal1 + areal2) / 2
            DiffVal = Abs(Evaluate_Scan.Areal - areal1) / areal1
            If DiffVal > ArealLimVal Then
                Scan_Accepted = False
            End If
            Evaluate_Scan.Pkt1 = ArealCheck(PktNr, 1).Pkt1
            Evaluate_Scan.Pkt2 = ArealCheck(PktNr, 1).Pkt2
            Evaluate_Scan.Pkt3 = ArealCheck(PktNr, 1).Pkt3
            MeasuredArea = MaesArea(1)

        Else
            If (Abs(areal2 - areal3)) < (Abs(areal1 - areal3)) And (Abs(areal2 - areal3)) < (Abs(areal1 - areal2)) Then
                Evaluate_Scan.Areal = (areal2 + areal3) / 2
                DiffVal = Abs(Evaluate_Scan.Areal - areal2) / areal2
                If DiffVal > ArealLimVal Then
                    Scan_Accepted = False
                End If
                Evaluate_Scan.Pkt1 = ArealCheck(PktNr, 2).Pkt1
                Evaluate_Scan.Pkt2 = ArealCheck(PktNr, 2).Pkt2
                Evaluate_Scan.Pkt3 = ArealCheck(PktNr, 2).Pkt3
                MeasuredArea = MaesArea(2)
            Else
                Evaluate_Scan.Areal = (areal1 + areal3) / 2
                DiffVal = Abs(Evaluate_Scan.Areal - areal1) / areal1
                If DiffVal > ArealLimVal Then
                    Scan_Accepted = False
                End If
                Evaluate_Scan.Pkt1 = ArealCheck(PktNr, 1).Pkt1
                Evaluate_Scan.Pkt2 = ArealCheck(PktNr, 1).Pkt2
                Evaluate_Scan.Pkt3 = ArealCheck(PktNr, 1).Pkt3
                MeasuredArea = MaesArea(1)

            End If
        End If

        If PktNr = 1 Then
            MeanAreal = Evaluate_Scan.Areal
        Else
            If (Abs(Evaluate_Scan.Areal - MeanAreal) / MeanAreal) > ArealLimVal Then
                Scan_Accepted = False
            Else
                MeanAreal = (MeanAreal + Evaluate_Scan.Areal) / 2
            End If
        End If


    End Function


End Module
