Imports System
Imports System.IO
Imports System.Data
Imports System.Text
Imports System.Math


Module Funct3D

  Public Const MAX_INTERFACE_COUNT As Integer = 5
  Public Const MAX_RESOULUTIONS As Integer = 6
  Public ProfileBuffer As Byte() = New Byte(1280 * 16 + 16) {}
  Public mResolution As Integer = 0
  Public DllPointer As Integer = 0
  Public ScanConnected As Boolean = False

  Public m_tscanCONTROLType As ScannerIF.TScannerType
  Public EthernetInterfaceCount As Integer = 0
  Dim EthernetInterfaces As UInteger() = New UInteger(MAX_INTERFACE_COUNT) {}




  Dim Resolutions As UInteger() = New UInteger(MAX_RESOULUTIONS) {}
  Dim IdleTime As Integer = 900
  Public ThrsHld As UInteger = &H1000CFF
  Dim ReturnValue As Integer
  Dim bOK As Boolean = True
  Public Threshld_Val As Integer = 32



  Public Sub LaserPower(ByVal State As String)

    Dim LaserPow As Integer
    Dim result As Integer

    If State = "ON" Then
      LaserPow = 2
    Else
      LaserPow = 0
    End If

    result = ScannerIF.s_SetFeature(DllPointer, ScannerIF.FEATURE_FUNCTION_LASERPOWER, LaserPow)

  End Sub

  Public Function InitProc() As Boolean

    Dim n_Cnt As Integer = 0
    Dim Result As Integer
    Dim Trace_Call As Integer = 0
    Dim TransferMode As Boolean
    InitProc = False



    Form1.ScannerStatus.BackColor = Color.Red

    While n_Cnt < 12
      Result = 0
      n_Cnt = n_Cnt + 1
      Trace_Call = Trace_Call + 1
      Form1.ScannerStatus.Text = "Initializing " & Str(Trace_Call) & " / 13"

      While Result = 0
        delay(100)
        Result = Init_Steps(n_Cnt)
        If Result <> 0 Then
          'n_Cnt = 12

        End If
      End While

    End While

    Form1.ScannerStatus.BackColor = Color.Green

    delay(200)

    n_Cnt = n_Cnt + 1
    'TransferMode = SetTransfer()
    TransferMode = Init_Steps(n_Cnt)

    If TransferMode = True Then
      Form1.ScannerStatus.Text = "TransferMode OK "
      InitProc = True
    Else
      InitProc = False
    End If



  End Function

  Public Function Init_Steps(ByVal Order As Integer) As Integer

    Init_Steps = 0
    ReturnValue = 0
    Dim hexval As Byte() = {&H0, &HC, &H0, &H1}
    hexval(0) = 255
    ThrsHld = BitConverter.ToInt32(hexval, 0)   ' global variabel ThrsHld tildeles ny værdi

    Select Case Order
      Case 1
        Form1.ScannerStatus.Text = "Find the Scanner's Ethernet Connection"
        DllPointer = 0
        DllPointer = ScannerIF.s_CreateLLTDevice(ScannerIF.TInterfaceType.INTF_TYPE_ETERNET)
        ReturnValue = DllPointer

      Case 2
        'Form1.ScannerStatus.Text = "Establish Ethernet Connection"
        'EthernetInterfaceCount = 0
        'EthernetInterfaceCount = ScannerIF.s_GetDeviceInterfaces(DllPointer, EthernetInterfaces, EthernetInterfaces.GetLength(0))
        ReturnValue = 1

      Case 3
        Form1.ScannerStatus.Text = " Set Device Interface"

        ReturnValue = ScannerIF.s_SetDeviceInterface(DllPointer, ScannerIP, 0)
        If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
          OnError("Error during SetDeviceInterface", ReturnValue)
          bOK = False
        End If

      Case 4
        Form1.ScannerStatus.Text = " Connecting to Scanner"
        ReturnValue = ScannerIF.s_Connect(DllPointer)
        If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
          OnError("Error during Connect", ReturnValue)
          bOK = False
        Else
          ScanConnected = True
        End If

      Case 5
        Form1.ScannerStatus.Text = "Get scanCONTROL type"
        ReturnValue = ScannerIF.s_GetLLTType(DllPointer, m_tscanCONTROLType)
        If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
          OnError("Error during GetLLTType", ReturnValue)
          bOK = False
        End If

      Case 6
        Form1.ScannerStatus.Text = "Get all possible resolutions"
        ReturnValue = ScannerIF.s_GetResolutions(DllPointer, Resolutions, Resolutions.GetLength(0))
        If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
          OnError("Error during GetResolutions", ReturnValue)
          bOK = False
        End If
        mResolution = Resolutions(0)
        Return ReturnValue

      Case 7
        Form1.ScannerStatus.Text = "Set resolution to " + Str(mResolution)
        ReturnValue = ScannerIF.s_SetResolution(DllPointer, mResolution)
        If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
          OnError("Error during SetResolution", ReturnValue)
          bOK = False
        End If

      Case 8
        Form1.ScannerStatus.Text = "Set trigger to internal"
        ReturnValue = ScannerIF.s_SetFeature(DllPointer, ScannerIF.FEATURE_FUNCTION_TRIGGER, &H0)
        If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
          OnError("Error during SetFeature(FEATURE_FUNCTION_TRIGGER)", ReturnValue)
          bOK = False
        End If

      Case 9
        Form1.ScannerStatus.Text = "Profile config set to PROFILE"
        ReturnValue = ScannerIF.s_SetProfileConfig(DllPointer, ScannerIF.TProfileConfig.PROFILE)
        If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
          OnError("Error during SetProfileConfig", ReturnValue)
          bOK = False
        End If

      Case 10
        Form1.ScannerStatus.Text = "Set shutter time to " + Str(ShutterTime)
        ReturnValue = ScannerIF.s_SetFeature(DllPointer, ScannerIF.FEATURE_FUNCTION_SHUTTERTIME, ShutterTime)
        If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
          OnError("Error during SetFeature(FEATURE_FUNCTION_SHUTTERTIME)", ReturnValue)
          bOK = False
        End If

      Case 11
        Form1.ScannerStatus.Text = "Set idle time to " + Str(IdleTime)
        ReturnValue = ScannerIF.s_SetFeature(DllPointer, ScannerIF.FEATURE_FUNCTION_IDLETIME, IdleTime)
        If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
          OnError("Error during SetFeature(FEATURE_FUNCTION_IDLETIME)", ReturnValue)
          bOK = False
        End If

      Case 12
        '12 setting transfer mode
        ReturnValue = ScannerIF.s_SetFeature(DllPointer, ScannerIF.FEATURE_FUNCTION_THRESHOLD, ThrsHld)
        If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
          OnError("Error during SetFeature(FEATURE_FUNCTION_THRESHOLD)", ReturnValue)
          bOK = False
        Else
          Form1.Thresh_Lbl.Text = "Threshold = " & Str(Threshld_Val)
        End If

      Case 13
        '13 setting transfer mode
        Form1.ScannerStatus.Text = "Set Transfer Mode "
        ReturnValue = ScannerIF.s_TransferProfiles(DllPointer, ScannerIF.TTransferProfileType.NORMAL_TRANSFER, 1)
        If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
          OnError("Error during SetFeature(FEATURE_FUNCTION_IDLETIME)", ReturnValue)
          Return (False)
        End If

        Return (True)


    End Select

    Return ReturnValue


  End Function


  Public Function SetTransfer() As Boolean
    Dim ReturnValue As Integer
    ReturnValue = ScannerIF.s_TransferProfiles(DllPointer, ScannerIF.TTransferProfileType.NORMAL_TRANSFER, 1)
    If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
      OnError("Error during SetFeature(FEATURE_FUNCTION_IDLETIME)", ReturnValue)
      Return (False)
    End If

    Return (True)

  End Function


  ' Close transfermode
  Public Function CloseTransfer() As Boolean

    Dim ReturnValue As Integer = 0

    ReturnValue = ScannerIF.s_TransferProfiles(DllPointer, ScannerIF.TTransferProfileType.NORMAL_TRANSFER, 0)
    If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
      'OnError("Error during TransferProfiles", ReturnValue);
      Return False
    End If
    'Close connection to the scanner
    If (ScanConnected) Then

      delay(100)

      ReturnValue = ScannerIF.s_Disconnect(DllPointer)
      If ((ReturnValue) < ScannerIF.GENERAL_FUNCTION_OK) Then
        'OnError("Error during Disconnect", ReturnValue)
        Return False
      End If
    End If
    Return True

  End Function


  Public Sub OnError(ByVal strErrorTxt As String, ByVal iErrorValue As Integer)

    Dim acErrorString As Byte() = New Byte(200) {}
    'MsgBox(strErrorTxt)
    Form1.Error_Lbl.Text = strErrorTxt
    ScannerConnected = False


    If (ScannerIF.s_TranslateErrorValue(DllPointer, iErrorValue, acErrorString, acErrorString.GetLength(0)) >= ScannerIF.GENERAL_FUNCTION_OK) Then
      Form1.ErrLabel2.Text = System.Text.Encoding.ASCII.GetString(acErrorString, 0, acErrorString.GetLength(0))
      'MsgBox(System.Text.Encoding.ASCII.GetString(acErrorString, 0, acErrorString.GetLength(0)))
    End If
  End Sub




  Public Function Get_Scan(ByVal ScanMode As String, ByVal Convert As Boolean) As Boolean

    On Error GoTo ErrHandler

    Dim ReturnValue As Integer
    Dim LostProfiles As UInteger = 0
    Dim ScanOk As Boolean = True
    Dim reflecWidth As UShort() = New UShort(mResolution) {}
    Dim Intensities As UShort() = New UShort(mResolution) {}
    Dim Threshold As UShort() = New UShort(mResolution) {}
    Dim M0 As UInteger() = New UInteger(mResolution) {}
    Dim M1 As UInteger() = New UInteger(mResolution) {}
    Dim ValueX As Double() = New Double(1280) {}
    Dim ValueZ As Double() = New Double(1280) {}

    Get_Scan = Nothing

    Select Case ScanMode

      Case "Pure"
        ReturnValue = ScannerIF.s_GetActualProfile(DllPointer, ProfileBuffer, ProfileBuffer.GetLength(0), ScannerIF.TProfileConfig.PURE_PROFILE, LostProfiles)
        If ((ReturnValue) = ProfileBuffer.GetLength(0)) Then
          ScanOk = False
          'Form1.Error_Lbl.Text = "Fejl i Get_Scan (14001)"
          OnError("Fejl i Get_Scan (14001)", ReturnValue)
        End If

      Case "Quarter"
        ReturnValue = ScannerIF.s_GetActualProfile(DllPointer, ProfileBuffer, ProfileBuffer.GetLength(0), ScannerIF.TProfileConfig.QUARTER_PROFILE, LostProfiles)
        If ((ReturnValue) = ProfileBuffer.GetLength(0)) Then
          ScanOk = False
          'Form1.Error_Lbl.Text = "Fejl i Get_Scan (14002)"
          OnError("Fejl i Get_Scan (14002)", ReturnValue)
        End If
        If ReturnValue < 0 Then
          ScanOk = False
          'Form1.Error_Lbl.Text = "Fejl i Get_Scan (14003)"
          OnError("Fejl i Get_Scan (14003)", ReturnValue)
          If ReturnValue = -104 Then
            'Form1.Error_Lbl.Text = "Fejl i Get_Scan (14004)"
            OnError("Fejl i Get_Scan (14004)", ReturnValue)
            ScannerConnected = False
            CloseTransfer()
          End If
        End If

    End Select

    If (Convert = True) And (ScanOk = True) Then

      'Converting of profile data from the first reflection
      ReturnValue = ScannerIF.s_ConvertProfile2Values(DllPointer, ProfileBuffer, mResolution, ScannerIF.TProfileConfig.QUARTER_PROFILE, m_tscanCONTROLType,
                                                              0, 1, reflecWidth, Intensities, Threshold, ValueX, ValueZ, M0, M1)
      If (((ReturnValue & ScannerIF.CONVERT_X) = 0) And ((ReturnValue & ScannerIF.CONVERT_Z) = 0)) Then

      Else
        'Form1.Error_Lbl.Text = "Fejl i Get_Scan (14005)"
        OnError("Fejl i Get_Scan (14005)", ReturnValue)
        Return False
      End If


    End If

    Return ScanOk

ErrHandler:


  End Function


  Public Function Convert(ByVal nr As Integer, ByVal ByteArr() As Byte) As LaserVal

    Row_Cnt = Row_Cnt

    Convert.res = 192 And ByteArr(0)      ' masker de første 2 bit i første byte
    Convert.PixWidth = ((63 And ByteArr(0)) * 2 ^ 4) + ((240 And ByteArr(1)) / 2 ^ 4)
    Convert.PixHeight = ((15 And ByteArr(1)) * 2 ^ 6) + ((252 And ByteArr(2)) / 2 ^ 2)
    Convert.Threshold = ((3 And ByteArr(2)) * 2 ^ 8) + (ByteArr(3))
    Convert.X_Pos = Int(((ByteArr(4) - 2 ^ 7) * (2 ^ 8 * 0.005) + (ByteArr(5) * 0.005)) * 100)
    Convert.Z_Pos = Int(((ByteArr(6) - 2 ^ 7) * (2 ^ 8 * 0.005) + (ByteArr(7) * 0.005) + 250) * 100)
    Convert.Moment_0 = Int(ByteArr(8) * 2 ^ 24 + ByteArr(9) * 2 ^ 16 + ByteArr(10) * 2 ^ 8 + ByteArr(11))
    Convert.Moment_1 = Int(ByteArr(12) * 2 ^ 24 + ByteArr(13) * 2 ^ 16 + ByteArr(14) * 2 ^ 8 + ByteArr(15))


  End Function


  Public Sub ConvertScan()

    Dim ByteArr As Byte()
    Dim ByteNr(15) As Byte


    'reading from the file

    Dim loop1, loop2 As Integer
    Dim Chk_Val1 As Integer = 0
    Dim Reflekt_Cnt As Integer = 0 '

    MinZ = 0
    ReflectVal.MaxVal = 0
    ReflectVal.MinVal = 200

    ByteArr = ProfileBuffer
    loop1 = 1
    ValidMeas = 0

    'For loop2 = 0 To 20480 Step 16
    For loop2 = 0 To 20479 Step 16
      Array.Copy(ByteArr, loop2, ByteNr, 0, 16)
      If loop2 < 20480 Then
        Laser(ValidMeas) = Convert(0, ByteNr)
        If Laser(ValidMeas).PixWidth < ReflectVal.MinVal Then ReflectVal.MinVal = Laser(ValidMeas).PixWidth
        If Laser(ValidMeas).PixWidth > ReflectVal.MaxVal Then ReflectVal.MaxVal = Laser(ValidMeas).PixWidth
        If loop2 = 0 Then Chk_Val1 = Laser(ValidMeas).Z_Pos
        If Math.Abs(Laser(ValidMeas).Z_Pos) > 15000 Then

          If ReflectVal.MaxVal < Laser(ValidMeas).PixWidth Then
            ReflectVal.MaxVal = Laser(ValidMeas).PixWidth
          End If
          If ReflectVal.MinVal > Laser(ValidMeas).PixWidth Then
            ReflectVal.MinVal = Laser(ValidMeas).PixWidth
          End If
          ' nedenstående holder øje med mindste Z-værdi i skanningen
          If ValidMeas < 51 Then
            MinZ = MinZ + Laser(ValidMeas).Z_Pos
          End If
          'If Laser(loop1, ValidMeas).Z_Pos < MinZ Then
          'MinZ = Laser(loop1, ValidMeas).Z_Pos
          'End If

          If Abs(Laser(ValidMeas).Z_Pos - Chk_Val1) < (500 + (Reflekt_Cnt * 10)) Then   ' hvis der er store reflektioner
            Chk_Val1 = Laser(ValidMeas).Z_Pos
            ValidMeas = ValidMeas + 1
            Reflekt_Cnt = 0
          Else
            Reflekt_Cnt = Reflekt_Cnt + 1
            If Reflekt_Cnt > 25 Then
              Chk_Val1 = Laser(ValidMeas).Z_Pos
            End If
          End If

        Else
          'Laser(loop1, ValidMeas).Z_Pos = 0
          'ValidMeas = ValidMeas + 1
        End If
      End If
    Next
    MinZ = MinZ / 50
    ValidMeas = ValidMeas - 1
    Form1.GoodValues.Text = "ValidMeas = " & Str(ValidMeas)
  End Sub

  Public Function Check_Reflection(ByVal ThrsHld_Selected As String) As Reflection
    Dim hexval As Byte() = {&H0, &HC, &H0, &H1} ' = Threshold NUL-værdien
    Dim ReturnValue As Integer
    Dim EXIT_Loop As Boolean = False
    Dim MostValidMeas As Integer = 0
    Dim BestScan As Integer
    Dim PixWdtMax As Integer = 0
    Dim PixWdtMin As Integer = 0


    If ThrsHld_Selected = "Auto" Then
      'While EXIT_Loop = False
      For i = 10 To 255 Step 10
        Threshld_Val = i
        hexval(0) = Threshld_Val
        ThrsHld = BitConverter.ToInt32(hexval, 0)   ' global variabel ThrsHld tildeles ny værdi
        ReturnValue = Init_Steps(12)                ' indlæser den nye threshold
        delay(100)
        ReturnValue = Get_Scan("Quarter", False)
        If ReturnValue = True Then
          Form1.ScannerStatus.Text = "Scan Succesful"
          ConvertScan()
          'Form1.Analyze_sel.Enabled = True
        End If
        Form1.ShowScan(Scan_Draw_Mode)
        If (MostValidMeas = ValidMeas) Then
          If ReflectVal.MaxVal < PixWdtMax Then
            BestScan = Threshld_Val
          End If
        End If

        If MostValidMeas < ValidMeas Then
          MostValidMeas = ValidMeas
          BestScan = Threshld_Val
          PixWdtMax = ReflectVal.MaxVal
          PixWdtMin = ReflectVal.MinVal

        End If

      Next i
      hexval(0) = Threshld_Val
      ThrsHld = BitConverter.ToInt32(hexval, 0)   ' global variabel ThrsHld tildeles ny værdi
      ReturnValue = Init_Steps(12)                ' indlæser den nye threshold
      delay(100)
      ReturnValue = Get_Scan("Quarter", False)
      If ReturnValue = True Then
        Form1.ScannerStatus.Text = "Scan Succesful"
        ConvertScan()
        'Form1.Analyze_sel.Enabled = True
      End If


    Else

      hexval(0) = Val(ThrsHld_Selected)     ' Thresholdværdien lægges over i hexval's byte(0)
      Threshld_Val = hexval(0)
      ThrsHld = BitConverter.ToInt32(hexval, 0)   ' global variabel ThrsHld tildeles ny værdi

      ReturnValue = Init_Steps(12)                ' Værdien sættes ved at kalde Init_Steps funktionens 12 funktion
      delay(100)
      ReturnValue = Get_Scan("Quarter", False)
      If ReturnValue = True Then
        Form1.ScannerStatus.Text = "Scan Succesful"
        ConvertScan()

        'Form1.Analyze_sel.Enabled = True
      End If

    End If
    Form1.Thresh_Lbl.Text = "Threshold = " & Str(Threshld_Val)
    If ReflectVal.MaxVal > 20 And ReflectVal.MaxVal < 40 Then
      If ValidMeas > ValidMeasLim Then

      End If
      'Analyze = True
    Else
      'Analyze = False
    End If

    Form1.ShowScan(Scan_Draw_Mode)
  End Function


End Module
