Module Weld

    Public Structure ScanPoints
        Public ScanNr As Integer
        Public ScanPkt As ScanAry
        Public PassNo As Integer
    End Structure


    Public Sub Weld_Areal(ByVal Pkt_1 As Coord, ByRef Pkt_2 As Coord, ByVal Line As LinesFound)
        Dim Areal1 As Double
        Dim Areal2 As Double = 0


        Dim X1, X2, Z1, Z2, Z3, dX, dZ, Z_Calc1, Z_Calc2 As Double
        'Form1.TextBox8.Text = ""

        For i = 1 To ValidMeas
            X1 = Laser(i).X_Pos / 100
            Z1 = Laser(i).Z_Pos / 100
            If X2 = 0 Then X2 = X1
            If (Math.Abs(X1 - X2) > 1) Then
                X2 = X2
            End If
            If (X1 < Pkt_1.X) And (X2 > Pkt_2.X) And (Math.Abs(X1 - X2) < 1) And Z1 > 100 Then
                If Z2 = 0 Then Z2 = Z1

                Z_Calc1 = ((Line.b * X1) + Line.a)
                Z_Calc2 = ((Line.b * X2) + Line.a)
                dZ = (((Z2 - Z_Calc2) + (Z1 - Z_Calc1)) / 2)
                dX = (X2 - X1)
                Areal1 = (dX * dZ)
                If dX < 0 Or dZ < 0 Then
                    Areal2 = Areal2
                Else
                    Areal2 = Areal2
                End If
                Areal2 = Areal2 + Areal1

                Dim Disp1, disp2 As String
                Disp1 = Format(Areal1, "###.#####0")
                disp2 = Format(X1, "###.####0")
                'Form1.TextBox8.Text = Form1.TextBox8.Text + Str(X1) + "  ;  " + Str(i) + "  ;  " + Str(Z1) + "  ;  " + Str(Z2) + "  ;  " + Disp1 + vbCrLf
                Z2 = Z1
                X2 = X1

            Else
                If (Math.Abs(X1 - X2) < 1) Then
                    X2 = X1
                End If
            End If

            Z3 = Math.Abs((Line.b * Laser(i).X_Pos) + Line.a)

        Next

    End Sub



End Module
