Imports System
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Math
Imports System.Console




public Module  datahandler

 
    Dim filstr As String
    Dim ByteArr1(7000) As Byte
    Dim ByteArr2(7000) As Byte
    Dim ByteArr3(7000) As Byte
    Dim TotalArr(21000) As Byte
    Dim ByteCluster(16) As Byte

    Dim Areal_Gns As Double = 0
    Dim Areal_Sum As Double = 0


 
    Public Function InterSect(ByRef Pkt1 As LinesFound, ByVal Pkt2 As LinesFound) As Coord
        InterSect.Success = True

        If Abs(Pkt1.a - Pkt2.a) < 0.001 Then
            InterSect.Success = False
            Return (InterSect)
        End If
        If Abs(Pkt2.b - Pkt1.b) < 5 Then
            'InterSect.Success = False
            'Return (InterSect)
        End If
        Intersect_X = ((Pkt2.b - Pkt1.b) / (Pkt1.a - Pkt2.a))
        Intersect_Z = (Pkt1.a * Intersect_X) + Pkt1.b

        InterSect.X = Intersect_X
        InterSect.Z = Intersect_Z


    End Function


    Public Function AngleChk() As Double
        AngleChk = 0

        Dim X1, Z1, X2, Z2, a1, ang1 As Double
        Dim Start As Double = 0
        Dim Slut As Double = 0

        For i = 51 To ValidMeas Step 10
            X1 = Laser(1, i).X_Pos / 100
            X2 = Laser(1, i - 50).X_Pos / 100
            Z1 = Laser(1, i).Z_Pos / 100
            Z2 = Laser(1, i - 50).Z_Pos / 100
            a1 = Abs((Z2 - Z1) / (X2 - X1))
            ang1 = Atan(a1) * 180 / Math.PI

            If ang1 > 30 And Start = 0 Then
                Start = X1
            End If

            If ang1 > 30 Then
                Slut = X1
            End If




        Next

        AngleChk = Start - ((Start - Slut) / 2)

        If Start = 0 And Slut = 0 Then AngleChk = 9999

        Return AngleChk

    End Function



    Public Sub skrivTxtfil(ByVal Filnavn As String)
        Dim content As String
        'Dim FilePath As String = "C:\Data\VB koder\3DScan Vfuge 180gr\Data\"
        Dim FILE_NAME As String = Filnavn
        Dim PixWdt, PixHgt, ThrsHld, M0, M1 As Integer
        Dim x1, Z1 As Double
        Dim objWriter As New System.IO.StreamWriter(FILE_NAME)
        Dim PosStr(3) As String
        Dim LineCnt As Integer

        PosStr(1) = Format(RobotVal(1).CartPos.X, "####.#0")
        PosStr(2) = Format(RobotVal(1).CartPos.Y, "####.#0")
        PosStr(3) = Format(RobotVal(1).CartPos.Z, "####.#0")


        For i = 0 To ValidMeas
            LineCnt = i
            x1 = Laser(ActLine, i).X_Pos / 100
            Z1 = Laser(ActLine, i).Z_Pos / 100
            PixWdt = Laser(ActLine, i).PixWidth
            PixHgt = Laser(ActLine, i).PixHeight
            ThrsHld = Laser(ActLine, i).Threshold
            M0 = Laser(ActLine, i).Moment_0
            M1 = Laser(ActLine, i).Moment_1

            content = Str(i) + "; " + Str(x1) + "; " + Str(Z1) + "; " + Str(PixHgt) + "; " + Str(PixWdt) + "; " + Str(ThrsHld) + "; " + Str(M0) + "; " + Str(M1)
            objWriter.WriteLine(content)
        Next
        content = Str(LineCnt + 1) + "; " + PosStr(1) + "; " + PosStr(2) + "; " + PosStr(3) + "; " + Str(0) + "; " + Str(0) + "; " + Str(0) + "; " + Str(0)
        objWriter.WriteLine(content)

        objWriter.Close()


    End Sub

    Public Sub skrivBinfil2()

        Dim fStream As New FileStream("c:\data\byteArray.dat", FileMode.Create)

        Dim bw As New BinaryWriter(fStream)


        bw.Write(ProfileBuffer)

        bw.Close()

        fStream.Close()


    End Sub




    Public Sub GetTxtFile(ByVal LineNr As Integer, ByVal Name As String)

        Using MyReader As New Microsoft.VisualBasic.
                      FileIO.TextFieldParser(
                        Name)
            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters(";")

            Dim currentRow As String()
            While Not MyReader.EndOfData
                Try
                    currentRow = MyReader.ReadFields()

                    Laser(LineNr, currentRow(0)).X_Pos = currentRow(1) * 100
                    Laser(LineNr, currentRow(0)).Z_Pos = currentRow(2) * 100
                    Laser(LineNr, currentRow(0)).PixWidth = currentRow(3)
                    Laser(LineNr, currentRow(0)).PixHeight = currentRow(4)
                    Laser(LineNr, currentRow(0)).Threshold = currentRow(5)
                    Laser(LineNr, currentRow(0)).Moment_0 = currentRow(6)
                    Laser(LineNr, currentRow(0)).Moment_1 = currentRow(6)
                    ValidMeas = currentRow(0) - 1
                Catch ex As Microsoft.VisualBasic.
                            FileIO.MalformedLineException
                    MsgBox("Line " & ex.Message &
                    "is not valid and will be skipped.")
                End Try

            End While

        End Using

    End Sub


    ' Beregner arealet 
    Public Function Area_Cal(ByVal Pkt_1 As Coord, ByRef Pkt_2 As Coord) As Double
        Dim Areal1 As Double
        Dim Areal2 As Double = 0
        Dim Line As LinesFound

        Dim X1, X2, Z1, Z2, dX, dZ, Z_Calc1, Z_Calc2 As Double
        'Form1.TextBox8.Text = ""

        Line.a = (Pkt_2.Z - Pkt_1.Z) / (Pkt_2.X - Pkt_1.X)
        Line.b = Pkt_2.Z - (Line.a * Pkt_2.X)

        For i = Pkt_2.Nr To Pkt_1.Nr Step -1
            X1 = Laser(Scan_Draw_Mode, i).X_Pos / 100
            Z1 = Laser(Scan_Draw_Mode, i).Z_Pos / 100
            If X2 = 0 Then X2 = X1
            If (Math.Abs(X1 - X2) > 1) Then
                X2 = X2
            End If
            'If (X1 < Pkt_2.X) And (X2 > Pkt_1.X) And (Math.Abs(X1 - X2) < 1) And Z1 > 100 Then
            If Z2 = 0 Then Z2 = Z1

            Z_Calc1 = ((Line.a * X1) + Line.b)
            Z_Calc2 = ((Line.a * X2) + Line.b)
            dZ = (((Z2 - Z_Calc2) + (Z1 - Z_Calc1)) / 2)  ' gennemsnittet af z-værdien. lægger de to længder sammen og  dividerer med 2
            dX = (X1 - X2)
            Areal1 = (dX * dZ)

            Areal2 = Areal2 + Areal1

            Z2 = Z1
            X2 = X1

        Next

        Return Areal2

    End Function


    Public Function Scan_Chk() As Scan_Result


        Dim Pkt(3) As xyzwprExt
        Dim FugeWidt As Double
        Dim FugeMidCalc As Double
        Dim FugeHgtCalc As Double
        Dim HgtDiv As Double
        Dim MidDiv As Double
        Dim StdWdtFact As Double = 2 * Math.Tan(36 * Math.PI / 180)



        Pkt(1) = RobotVal(4).CartPos
        Pkt(2) = RobotVal(3).CartPos
        Pkt(3) = RobotVal(2).CartPos

        FugeWidt = Pkt(1).X - Pkt(2).X
        'FugeMidCalc = (Pkt(2).X + Pkt(1).X) / 2
        FugeMidCalc = ((Pkt(2).X - Pkt(1).X) / 2) + Pkt(1).X
        FugeHgtCalc = Abs(Pkt(3).Z - ((Pkt(2).Z + Pkt(1).Z) / 2))
        HgtDiv = (1 - (FugeWidt / StdWdtFact) / FugeHgtCalc) * 100

        Dim a1 As Double = (Pkt(3).Z - Pkt(1).Z) / (Pkt(3).X - Pkt(1).X)
        Dim a2 As Double = (Pkt(2).Z - Pkt(3).Z) / (Pkt(2).X - Pkt(3).X)
        Dim Ang1 As Double = 90 - Atan(a1) * 180 / PI
        Dim Ang2 As Double = 90 + Atan(a2) * 180 / PI
        Dim AngTot As Double = Ang1 + Ang2



        If Pkt(3).X > FugeMidCalc Then
            MidDiv = (Pkt(3).X - FugeMidCalc) / FugeHgtCalc * 100
            Scan_Chk.MidtForskyd = Pkt(3).X - (FugeMidCalc)
        Else
            MidDiv = (FugeMidCalc - Pkt(3).X) / FugeHgtCalc * 100
            Scan_Chk.MidtForskyd = Pkt(3).X - (FugeMidCalc)
        End If


        Dim disp As String = Format(AngTot, "###.0")
        Form1.FugeAng.Text = Str(disp)
        Scan_Chk.FugeAng = AngTot

        disp = Format(Abs(FugeWidt), "###.0")
        Form1.FugeWdt.Text = disp

        disp = Format(Abs(FugeHgtCalc), "###.0")
        Form1.FugeHgt.Text = disp

        disp = Format(Abs(HgtDiv), "###.0")
        Form1.HgtError.Text = disp
        Scan_Chk.Hgt_Div = HgtDiv

        disp = Format(MidDiv, "###.0")
        Form1.WdtError.Text = disp
        Scan_Chk.Mid_Div = MidDiv

    End Function


    ''' <summary>
    ''' Methods to upload file to FTP Server
    ''' </summary>
    ''' <param name="_FileName">local source file name</param>
    ''' <param name="_UploadPath">Upload FTP path including Host name</param>
    ''' <param name="_FTPUser">FTP login username</param>
    ''' <param name="_FTPPass">FTP login password</param>

    Public Sub UploadFile(ByVal _FileName As String, ByVal _UploadPath As String, ByVal _FTPUser As String, ByVal _FTPPass As String)
        Dim _FileInfo As New System.IO.FileInfo(_FileName)

        ' Create FtpWebRequest object from the Uri provided
        Dim _FtpWebRequest As System.Net.FtpWebRequest = CType(System.Net.FtpWebRequest.Create(New Uri(_UploadPath)), System.Net.FtpWebRequest)

        ' Provide the WebPermission Credintials
        _FtpWebRequest.Credentials = New System.Net.NetworkCredential(_FTPUser, _FTPPass)

        ' By default KeepAlive is true, where the control connection is not closed
        ' after a command is executed.
        _FtpWebRequest.KeepAlive = False

        ' set timeout for 20 seconds
        _FtpWebRequest.Timeout = 20000

        ' Specify the command to be executed.
        _FtpWebRequest.Method = System.Net.WebRequestMethods.Ftp.UploadFile

        ' Specify the data transfer type.
        _FtpWebRequest.UseBinary = True

        ' Notify the server about the size of the uploaded file
        _FtpWebRequest.ContentLength = _FileInfo.Length

        ' The buffer size is set to 2kb
        Dim buffLength As Integer = 2048
        Dim buff(buffLength - 1) As Byte

        ' Opens a file stream (System.IO.FileStream) to read the file to be uploaded
        Dim _FileStream As System.IO.FileStream = _FileInfo.OpenRead()

        Try
            ' Stream to which the file to be upload is written
            Dim _Stream As System.IO.Stream = _FtpWebRequest.GetRequestStream()

            ' Read from the file stream 2kb at a time
            Dim contentLen As Integer = _FileStream.Read(buff, 0, buffLength)

            ' Till Stream content ends
            Do While contentLen <> 0
                ' Write Content from the file stream to the FTP Upload Stream
                _Stream.Write(buff, 0, contentLen)
                contentLen = _FileStream.Read(buff, 0, buffLength)
            Loop

            ' Close the file stream and the Request Stream
            _Stream.Close()
            _Stream.Dispose()
            _FileStream.Close()
            _FileStream.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    'HOW TO USE:

    ' Upload file using FTP
    'UploadFile("c:\UploadFile.doc", "ftp://FTPHostName/UploadPath/UploadFile.doc", "UserName", "Password")


End Module
