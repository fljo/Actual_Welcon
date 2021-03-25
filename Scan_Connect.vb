Module Scan_Connect

  Public IP_Array(4, 4) As Integer
  Dim IP_Val As UInteger
  Public Plc_Port As Integer
  Public Plc_Ip As String



  Public Function GetIP() As Integer
    Dim Cnt As Integer = 0
    Dim path As String = My.Application.Info.DirectoryPath
    Dim FilNavn As String = path & "\InrotechConf.dat"


    Using MyReader As New Microsoft.VisualBasic.
                  FileIO.TextFieldParser(FilNavn)
      MyReader.TextFieldType = FileIO.FieldType.Delimited
      MyReader.SetDelimiters(".")

      Dim currentRow As String()
      While Not MyReader.EndOfData
        Try
          currentRow = MyReader.ReadFields()

          IP_Array(Cnt, 3) = currentRow(1)
          IP_Array(Cnt, 2) = currentRow(2)
          IP_Array(Cnt, 1) = currentRow(3)
          IP_Array(Cnt, 0) = currentRow(4)
          Cnt = Cnt + 1

        Catch ex As Microsoft.VisualBasic.
                               FileIO.MalformedLineException
          MsgBox("Line " & ex.Message &
                        "is not valid and will be skipped.")
        End Try

      End While

    End Using



    GetIP = 0

    '       IP_Array(3) = 192   ' MSB       (192.xxx.xxx.xxx)
    '       IP_Array(2) = 168   '           (xxx.168.xxx.xxx)
    '       IP_Array(1) = 62   '           (xxx.xxx.117.xxx)
    '       IP_Array(0) = 228   ' LSB       (xxx.xxx.xxx.123)

    For i = 0 To 3
      ScannerIP = ScannerIP + IP_Array(0, i) * 256 ^ i
    Next i

    'RobotIP = ""
    'For i = 3 To 0 Step -1
    'RobotIP = RobotIP + Str(IP_Array(1, i))
    'If i > 0 Then RobotIP = RobotIP + "."
    'Next

    Plc_Port = Str(IP_Array(3, 3))
    Plc_Ip = String.Concat(IP_Array(2, 3), ".", IP_Array(2, 2), ".", IP_Array(2, 1), ".", IP_Array(2, 0))
    Form1.Error_Lbl.Text = "Scanner IP: " + Str(ScannerIP)
  End Function

End Module
