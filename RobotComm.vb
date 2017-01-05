Imports System.Net
Imports System.Net.Sockets

Module RobotComm
    Dim xyzwpr(8) As Single
    Dim config(6) As Short
    Dim joint(8) As Single
    Dim intUF As Short
    Dim intUT As Short
    Dim intValidC As Short
    Dim intValidJ As Short
    Dim blnDT As Boolean
    'RND
    Public RobReadData(50) As Byte
    Public RobWriteData(50) As Byte
    Public OmronConnected As Boolean


    Private Const cnstApp As String = "frrjiftest"
    Private Const cnstSection As String = "setting"



    Public mobjCore As FRRJIf.Core
    Private mobjDataTable As FRRJIf.DataTable
    Private mobjCurPos As FRRJIf.DataCurPos
    Private mobjCurPosUF As FRRJIf.DataCurPos
    Private mobjPosReg As FRRJIf.DataPosReg
    Private PosChk As FRRJIf.DataPosReg
    Private mobjPosRegXyzwpr As FRRJIf.DataPosRegXyzwpr
    Private mobjNumReg As FRRJIf.DataNumReg
    Private IniHeights As FRRJIf.DataNumReg
    Private Init_X As FRRJIf.DataNumReg
    Private mobjNumReg2 As FRRJIf.DataNumReg
    Private ScanArea As FRRJIf.DataNumReg
    'RND
    Private FanucRegR As FRRJIf.DataNumReg
    Private FanucRegW As FRRJIf.DataNumReg
    'Private mobjNumReg3 As FRRJIf.DataNumReg



    Public Function RobComInit() As Boolean
        RobComInit = False

        Dim blnRes As Boolean
        Dim strHost As String
        Dim lngTmp As Integer
        Dim HostName As String


        On Error GoTo Errhandler

        'System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        mobjCore = New FRRJIf.Core

        ' You need to set data table before connecting.
        mobjDataTable = mobjCore.DataTable

        With mobjDataTable
            'mobjCurPos = .AddCurPos(FRRJIf.FRIF_DATA_TYPE.CURPOS, 1)
            mobjCurPosUF = .AddCurPosUF(FRRJIf.FRIF_DATA_TYPE.CURPOS, 1, 15)
            mobjPosReg = .AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 1, 1, 5)  ' 3 sidste cifre: group, StartIndex, EndIndex
            PosChk = .AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 1, 101, 141)  ' 3 sidste cifre: group, StartIndex, EndIndex
            Init_X = .AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_REAL, 401, 444)
            IniHeights = .AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_REAL, 371, 385)
            mobjNumReg = .AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_INT, 1, 5)
            mobjNumReg2 = .AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_REAL, 6, 14)
            mobjPosRegXyzwpr = .AddPosRegXyzwpr(FRRJIf.FRIF_DATA_TYPE.POSREG_XYZWPR, 1, 1, 10)
            FanucRegR = .AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_INT, 200, 219)
            FanucRegW = .AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_INT, 220, 239)
            ScanArea = .AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_REAL, 261, 275)

        End With


        'get host name
        HostName = RobotIP
        strHost = HostName

        'get time out value
        lngTmp = CInt(GetSetting(cnstApp, cnstSection, "TimeOut", "-1"))

        '----------  CONNECT  ----------

        If lngTmp > 0 Then mobjCore.TimeOutValue = lngTmp
        blnRes = mobjCore.Connect(strHost)
        If blnRes = False Then
            Return False
        Else
            Control.CheckForIllegalCrossThreadCalls = False
            Dim NetThread As System.Threading.Thread = New Threading.Thread(AddressOf WThread)
            NetThread.Start()

            Return True
        End If


Errhandler:
        Form1.Error_Lbl.Text = "Fejl i Robot Initialiseringen (13001)"
        System.Windows.Forms.Cursor.Current = Cursors.Default
        MsgBox(Err.Description)
        Return False
        'End
    End Function


    Sub WThread()
        Dim recv As Integer
        Dim data As Byte() = New Byte(1023) {}


        While True
            'Dim ip As IPAddress = IPAddress.Parse(Plc_Ip)
            'Dim port As Integer = Plc_Port
            'Dim newsock As New TcpListener(ip, port)
            Dim newsock As New TcpListener(4505)
            newsock.Start()

            Dim client As TcpClient = newsock.AcceptTcpClient()
            Dim ns As NetworkStream = client.GetStream()

            OmronConnected = True

            While True
                data = New Byte(50) {}

                'recieve data
                recv = ns.Read(data, 0, data.Length)
                If recv = 0 Then
                    OmronConnected = False
                    Exit While
                End If

                For i = 0 To 10
                    RobWriteData(i) = data(i)
                Next
                'WriteRNDRobot() 'check om det returner false, og genstart forbindelse hvis nødvendigt

                'send data
                'ReadRNDRobot() 'check om det returner false, og genstart forbindelse hvis nødvendigt
                ns.Write(RobReadData, 0, RobReadData.Length)

            End While
            ns.Close()
            client.Close()
            newsock.[Stop]()
        End While
    End Sub



    Public Function GetIniHeights(ByVal RegNr As Integer) As Double
        Dim vntValue As Object = Nothing

        With IniHeights

            If .GetValue(RegNr, vntValue) = True Then
                Global_Heights(RegNr - 370) = vntValue
            Else
                Form1.Error_Lbl.Text = "Fejl i GetIniHeights (13002)"
                disconnect()
                Return 0
            End If
            GetIniHeights = vntValue

        End With

    End Function

    Public Sub SetInit_X(ByVal RegNr As Integer, ByVal Value As Single)

        Dim RealValue(0) As Single
        RealValue(0) = Value

        If Init_X.SetValues(RegNr, RealValue, 1) = False Then
            Form1.Error_Lbl.Text = "Fejl i SetInit_X (13003)"
            disconnect()
            Exit Sub
        End If

    End Sub

    Public Function GetInit_X(ByVal RegNr As Integer) As Double
        Dim vntValue As Object = Nothing
        Dim testval As Double

        With Init_X

            If .GetValue(RegNr, vntValue) = True Then
                Global_XVal_Reg(RegNr - 400) = vntValue
                testval = vntValue
            Else
                Form1.Error_Lbl.Text = "Fejl i GetInit_X (13004)"
                disconnect()
                Return (0)

            End If
            GetInit_X = vntValue
            testval = vntValue
        End With

    End Function

    Public Sub SetIniHeights(ByVal RegNr As Integer, ByVal Value As Single)

        Dim RealValue(0) As Single
        RealValue(0) = Value

        If IniHeights.SetValues(RegNr, RealValue, 1) = False Then
            Form1.Error_Lbl.Text = "Fejl i SetIniHeights (13005)"
            disconnect()
            Exit Sub
        End If

    End Sub


    Public Function GetPosReg(ByVal RegNr As Integer) As xyzwprExt
        Dim pos1(8) As Single
        With PosChk
            If .GetValue(RegNr, pos1, config, joint, intUF, intUT, intValidC, intValidJ) Then
                GetPosReg.X = pos1(0)
                GetPosReg.Y = pos1(1)
                GetPosReg.Z = pos1(2)
                GetPosReg.w = pos1(3)
                GetPosReg.p = pos1(4)
                GetPosReg.r = pos1(5)
                GetPosReg.E1 = pos1(6)
                GetPosReg.E2 = pos1(7)
                GetPosReg.E3 = pos1(8)
            Else
                Form1.Error_Lbl.Text = "Fejl i GetPosReg (13006)"
                disconnect()
                Exit Function
            End If
        End With

    End Function

 
    Public Function ReadRNDRobot() As Boolean
        Dim Refresh As Boolean
        Dim returnV As Object = Nothing
        Dim ArrayPos As Integer

        On Error Resume Next



        With FanucRegR
            ArrayPos = 0
            For i = 200 To 219
                If .GetValue(i, returnV) = True Then
                    '0 byte (ready signal)
                    If i = 200 Then
                        RobReadData(ArrayPos) = returnV
                        ArrayPos = ArrayPos + 1
                    End If
                    '1st byte (state)
                    If i = 201 Then
                        RobReadData(ArrayPos) = returnV
                        ArrayPos = ArrayPos + 1
                    End If
                    '2nd byte (status)
                    If i = 202 Then
                        RobReadData(ArrayPos) = returnV
                        ArrayPos = ArrayPos + 1
                    End If
                    '3rd byte (commando 1)
                    If i = 203 Then
                        RobReadData(ArrayPos) = returnV
                        ArrayPos = ArrayPos + 1
                    End If
                    '4th byte (commando 2)
                    If i = 204 Then
                        RobReadData(ArrayPos) = returnV
                        ArrayPos = ArrayPos + 1
                    End If
                    '5,6,7,8th bytes (errorcodes)
                    If i = 205 Then
                        Dim bytes As Byte() = BitConverter.GetBytes(returnV)
                        RobReadData(ArrayPos) = bytes(0)
                        ArrayPos = ArrayPos + 1
                        RobReadData(ArrayPos) = bytes(1)
                        ArrayPos = ArrayPos + 1
                        RobReadData(ArrayPos) = bytes(2)
                        ArrayPos = ArrayPos + 1
                        RobReadData(ArrayPos) = bytes(3)
                        ArrayPos = ArrayPos + 1
                    End If
                    '9,10th bytes (height)
                    If i = 206 Then
                        Dim bytes As Byte() = BitConverter.GetBytes(returnV)
                        RobReadData(ArrayPos) = bytes(0)
                        ArrayPos = ArrayPos + 1
                        RobReadData(ArrayPos) = bytes(1)
                        ArrayPos = ArrayPos + 1
                    End If
                    '11,12th bytes (angle)
                    If i = 207 Then
                        Dim bytes As Byte() = BitConverter.GetBytes(returnV)
                        RobReadData(ArrayPos) = bytes(0)
                        ArrayPos = ArrayPos + 1
                        RobReadData(ArrayPos) = bytes(1)
                        ArrayPos = ArrayPos + 1
                    End If
                    '13th byte (grove control)
                    If i = 208 Then
                        RobReadData(ArrayPos) = returnV
                        ArrayPos = ArrayPos + 1
                    End If
                    '14th byte (Handshake)
                    If i = 209 Then
                        RobReadData(ArrayPos) = returnV
                        ArrayPos = ArrayPos + 1
                    End If
                    '15,16th byte (estimated time)
                    If i = 210 Then
                        Dim bytes As Byte() = BitConverter.GetBytes(returnV)
                        RobReadData(ArrayPos) = bytes(0)
                        ArrayPos = ArrayPos + 1
                        RobReadData(ArrayPos) = bytes(1)
                        ArrayPos = ArrayPos + 1
                    End If
                    '17,18th byte (remaning time)
                    If i = 211 Then
                        Dim bytes As Byte() = BitConverter.GetBytes(returnV)
                        RobReadData(ArrayPos) = bytes(0)
                        ArrayPos = ArrayPos + 1
                        RobReadData(ArrayPos) = bytes(1)
                        ArrayPos = ArrayPos + 1
                    End If
                    '19th byte (Process %)
                    If i = 212 Then
                        RobReadData(ArrayPos) = returnV
                        ArrayPos = ArrayPos + 1
                    End If
                    '20,21th byte (svøb nummer)
                    If i = 213 Then
                        Dim bytes As Byte() = BitConverter.GetBytes(returnV)
                        RobReadData(ArrayPos) = bytes(0)
                        ArrayPos = ArrayPos + 1
                        RobReadData(ArrayPos) = bytes(1)
                        ArrayPos = ArrayPos + 1
                    End If
                    '22th byte (weld on)
                    If i = 214 Then
                        RobReadData(ArrayPos) = returnV
                        ArrayPos = ArrayPos + 1
                    End If

                End If
            Next i
        End With
        Return True
    End Function



    ' read eller refresh robot data
    Public Function ReadRobot()
        ReadRobot = Nothing
        'Dim intSO(11) As Short
        Dim intSDO(9) As Short
        Dim intSDI(9) As Short
        'Dim intUO(10) As Short
        'Dim blnSO As Boolean
        Dim blnSDO As Boolean
        Dim blnSDI As Boolean
        'Dim blnUO As Boolean
        Dim strTmp As String = ""
        Dim vntValue As Object = Nothing

        On Error Resume Next

        'check
        If mobjCore Is Nothing Then
            Form1.Error_Lbl.Text = "Fejl i ReadRobot (13007)"
            disconnect()
            Exit Function
        End If


        'Refresh data table
        blnDT = mobjDataTable.Refresh
        If blnDT = False Then
            Form1.Error_Lbl.Text = "Fejl i ReadRobot (13008)"
            disconnect()
            Exit Function
        End If


        With mobjCurPosUF
            If .GetValue(xyzwpr, config, joint, intUF, intUT, intValidC, intValidJ) Then
                RobotVal(1).CartPos.X = xyzwpr(0)
                RobotVal(1).CartPos.Y = xyzwpr(1)
                RobotVal(1).CartPos.Z = xyzwpr(2)
                RobotVal(1).CartPos.w = xyzwpr(3)
                RobotVal(1).CartPos.p = xyzwpr(4)
                RobotVal(1).CartPos.r = xyzwpr(5)
                RobotVal(1).CartPos.E1 = xyzwpr(6)
                RobotVal(1).CartPos.E2 = xyzwpr(7)
                RobotVal(1).CartPos.E3 = xyzwpr(8)
            Else
                Form1.Error_Lbl.Text = "Fejl i ReadRobot (13009)"
                disconnect()
                Exit Function
            End If
        End With

        With mobjCurPos
            If .GetValue(xyzwpr, config, joint, intUF, intUT, intValidC, intValidJ) Then
                'strTmp = strTmp & "--- CurPos GP1 World ---" & vbCrLf
                'strTmp = strTmp & mstrPos(xyzwpr, config, joint, intValidC, intValidJ, intUF, intUT)
            Else
                Form1.Error_Lbl.Text = "Fejl i ReadRobot (13010)"
                disconnect()
                Exit Function
            End If
        End With


        '-------------------------
        blnSDO = mobjCore.ReadSDO(1, intSDO, 10)
        If blnSDO = False Then
            System.Windows.Forms.Cursor.Current = Cursors.Default
            Form1.Error_Lbl.Text = "Fejl i ReadRobot (13011)"
            disconnect()
            Exit Function
        End If

        For i = 0 To 9
            RobotVal(i + 1).SOut = intSDO(i)
        Next
        '-------------------------
        ' set variabel-værdier for kontrol udgange
        Robot_Moving = RobotVal(1).SOut
        Searching = RobotVal(2).SOut
        DigSearch = RobotVal(3).SOut
        '-------------------------
        blnSDI = mobjCore.ReadSDI(1, intSDI, 10)
        If blnSDI = False Then
            Form1.Error_Lbl.Text = "Fejl i ReadRobot (13012)"
            disconnect()
            Exit Function
        End If
        For i = 0 To 9
            RobotVal(i + 1).SdIn = intSDI(i)
        Next





        '-------------------------

        With mobjNumReg
            For i = 1 To 5
                If .GetValue(i, vntValue) = True Then
                    RobotVal(i).NumRegInt = vntValue
                Else
                    Form1.Error_Lbl.Text = "Fejl i ReadRobot (13013)"
                    disconnect()
                    Exit Function
                End If
            Next i

        End With

        With mobjNumReg2
            For i = 6 To 14
                If .GetValue(i, vntValue) = True Then
                    RealNumReg1(i) = vntValue
                Else
                    Form1.Error_Lbl.Text = "Fejl i ReadRobot (13014)"
                    disconnect()
                    Exit Function
                End If
            Next i

        End With



    End Function


    Public Function WriteRNDRobot()
        Dim Refresh As Boolean

        On Error Resume Next

        With FanucRegW
            For i = 220 To 239
                If .SetValue(i, RobWriteData(i - 220)) = False Then
                    Form1.Error_Lbl.Text = "Fejl i WriteRNDRobot (13015)"
                    disconnect()
                    Return False
                End If
            Next i
        End With
        Return True
    End Function


    Public Sub SetPosRegX(ByVal NewPos As xyzwprExt, ByVal RegNr As Integer)
        Dim pos1(8) As Single

        pos1(0) = NewPos.X
        pos1(1) = NewPos.Y
        pos1(2) = NewPos.Z
        pos1(3) = NewPos.w
        pos1(4) = NewPos.p
        pos1(5) = NewPos.r
        pos1(6) = NewPos.E1
        pos1(7) = NewPos.E2
        pos1(8) = NewPos.E3

        'Call mobjPosReg.SetValueXyzwpr(ii, sngArray, intConfig, -1, -1)
        Call mobjPosReg.SetValueXyzwpr(RegNr, pos1, config, -1, -1)
        'ReadRobot() ' refresh data

    End Sub

    Public Sub SetScanArea(ByVal RegNr As Integer, ByVal Value As Single)
        Dim RealValue(0) As Single
        RealValue(0) = Value

        If ScanArea.SetValues((RegNr + 260), RealValue, 1) = False Then
            Form1.Error_Lbl.Text = "Fejl i SetRealReg (13016)"
            disconnect()
            Exit Sub
        End If
        'ReadRobot() ' refresh data
    End Sub

    Public Sub SetRealReg(ByVal RegNr As Integer, ByVal Value As Single)
        Dim RealValue(0) As Single
        RealValue(0) = Value

        If mobjNumReg2.SetValues(RegNr, RealValue, 1) = False Then
            Form1.Error_Lbl.Text = "Fejl i SetRealReg (13016)"
            disconnect()
            Exit Sub
        End If
        'ReadRobot() ' refresh data
    End Sub

    Public Sub SetIntReg(ByVal RegNr As Integer, ByVal Value As Integer)
        Dim IntValue(0) As Integer
        IntValue(0) = Value

        If mobjNumReg.SetValues(RegNr, IntValue, 1) = False Then
            Form1.ErrLabel2.Text = "Fejl i SetIntReg (13017)  - " + Str(ErrorTracker) + "  " + Str(RobotReady)
            disconnect()

        End If
        'ReadRobot() ' refresh data
    End Sub


    Public Function SetSO(ByVal PortNr As Short, ByVal Output() As Short) As Boolean
        Dim blnRes As Boolean

        blnRes = mobjCore.WriteSO(PortNr, Output, 1)
        If blnRes = False Then
            Form1.Error_Lbl.Text = "Fejl i SetSO (13018)"
            disconnect()
            Return False
        End If
        Return True
    End Function

    Public Function cmdSetSDO(ByVal PortNr As Short, ByVal Output() As Short) As Boolean
        Dim blnRes As Boolean

        blnRes = mobjCore.WriteSDO(PortNr, Output, 1)

        If blnRes = False Then
            Form1.Error_Lbl.Text = "Fejl i cmdSetSDO (13019)  - " + Str(ErrorTracker) + "  " + Str(RobotReady)
            disconnect()
            Return False
        End If

        Return True
    End Function


    Private Sub disconnect()
        RobotReady = False
        mobjCore.Disconnect()
    End Sub


    Private Function mstrPos(ByRef xyzwpr() As Single, ByRef config() As Short, ByRef joint() As Single, ByVal intValidC As Short, ByVal intValidJ As Short, ByVal UF As Integer, ByVal UT As Integer) As String
        Dim tmp As String = ""
        Dim ii As Integer

        tmp = tmp & "UF = " & UF & ", "
        tmp = tmp & "UT = " & UT & vbCrLf
        If intValidC <> 0 Then
            tmp = tmp & "XYZWPR = "
            For ii = 0 To 8 '5
                tmp = tmp & xyzwpr(ii) & " "
            Next ii

            tmp = tmp & vbCrLf & "CONFIG = "
            If config(0) Then
                tmp = tmp & "F "
            Else
                tmp = tmp & "N "
            End If
            If config(1) Then
                tmp = tmp & "L "
            Else
                tmp = tmp & "R "
            End If
            If config(2) Then
                tmp = tmp & "U "
            Else
                tmp = tmp & "D "
            End If
            If config(3) Then
                tmp = tmp & "T "
            Else
                tmp = tmp & "B "
            End If
            tmp = tmp & config(4) & ", " & config(5) & ", " & config(6) & vbCrLf
        End If

        If intValidJ <> 0 Then
            tmp = tmp & "JOINT = "
            For ii = 0 To 8 '5
                tmp = tmp & joint(ii) & " "
            Next ii
            tmp = tmp & vbCrLf
        End If

        mstrPos = tmp

    End Function





End Module
