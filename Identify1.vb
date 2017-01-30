Imports System
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Math


Module Identify1


    Public MaxClusterGap As Integer = 10

    ' Identificer linier mellem 2 plader der er vinklet mod hinanden, hvor den ene er skærpet.

    Public Sub Find_Alm_Stumpsom_2PL(ByVal No_Lines As Integer)


    End Sub



    Public Sub IdentifyLines(ByVal No_Lines As Integer)
        Dim MaesVal As Double
        Dim Cnt As Integer
        Dim LineDef As Integer = 50
        Dim LineAng As Double
        Dim SumVal As Double
        Dim GnsMaes As Double
        Dim Tolerence As Double
        Dim GapCnt As Integer
        Dim StartPkt, SlutPkt As Integer
        Dim Line3Start, Line3Slut As Integer
        Dim Direction As Integer


        Form1.TextBox8.Text = ""

        While No_Lines > 0
            StartPkt = 1
            Cnt = 0
            GnsMaes = 0
            GapCnt = 0
            SumVal = 0
            ValidMeas = 1279

            While Cnt < ValidMeas
                Cnt = Cnt + 1
                ' vi regner med at starte med en lige line
                If No_Lines = 3 Then
                    MaesVal = Maaling(Cnt)                  ' fra højre (som er fra måling 1 til 1280 
                    Tolerence = 1
                    'Form1.TextBox8.Text = Form1.TextBox8.Text + Str(Cnt) + " - " + Str(MaesVal) + " - " + Str(GnsMaes) + vbCrLf
                End If
                If No_Lines = 2 Then
                    MaesVal = Maaling(ValidMeas - Cnt)      ' fra venstre (som er fra måling 1280 - 1)
                    Tolerence = 2 ' 0.001
                End If

                If Cnt = 1 Then             ' gælder kun for første måling.
                    GnsMaes = MaesVal       ' vi sikrer os at hvis linien er omkring vandret, så kigger vi på en hældning mellem +/- 10 grader
                End If

                If MaesVal > 350 Then
                    MaesVal = MaesVal - 360
                Else
                    If Math.Abs(MaesVal - GnsMaes) > 350 Then   ' når GnsMaes ligger mellem 10 / 350 grader så regnes vinklerne altid i +/- 10 grader
                        MaesVal = MaesVal - 360
                    End If
                End If

                If Cnt <= LineDef Then
                    SumVal = SumVal + MaesVal
                    GnsMaes = SumVal / Cnt
                    LineAng = GnsMaes       ' det er vinklen vi sammenligner med
                End If

                If Cnt > LineDef Then       ' nu kigger vi efter afslutningen på linien
                    If Math.Abs(MaesVal - GnsMaes) > Tolerence Then
                        GapCnt = GapCnt + 1
                    Else
                        SumVal = SumVal + MaesVal
                        GnsMaes = SumVal / Cnt

                        GapCnt = 0
                    End If

                    If GapCnt > MaxClusterGap Then
                        If No_Lines = 3 Then
                            SlutPkt = Cnt + GapCnt + LoopCnt
                            Cnt = ValidMeas
                        End If
                        If No_Lines = 2 Then
                            SlutPkt = ValidMeas - (Cnt + GapCnt + LoopCnt)
                            Cnt = ValidMeas
                        End If


                    End If
                End If

            End While

            ' Find liniens præcise slutpunkt

            If No_Lines = 3 Then    ' her starter vi fra begyndelsen som altid er en lige linie.
                chkval = True
                Tolerence = 3  ' org val 2.8  ' da reflektionerne kan være slemme på denne side er tolerencen størst, men da vinklen er stejl gør det ikke så meget
                Direction = 1
                LineID(0) = Calc_Line(Tolerence, StartPkt, SlutPkt, Direction)          ' Calc_Line(Tolerence, Startpkt, Slutpkt, retning)
                LineID(0).PointData.EndPkt = LineID(0).PointData.EndPkt - 8
                'Form1.DrawLineFct(LineID(0), Color.Green)
            End If
            If No_Lines = 2 Then    ' her starter vi bagfra - altså fra sidste ´punkt og kører modsat
                chkval = True
                Tolerence = 0.5
                Direction = -1
                LineID(1) = Calc_Line(Tolerence, 1279, (SlutPkt - LoopCnt), Direction)  ' Calc_Line(Tolerence, Startpkt, Slutpkt, retning)

                'Form1.DrawLineFct(LineID(1), Color.Green)
            End If

            If No_Lines = 1 Then
                Line3Start = LineID(0).PointData.EndPkt
                Line3Slut = LineID(1).PointData.EndPkt
                If Line3Start < Line3Slut Then
                    Direction = -1
                Else
                    Direction = 1
                End If

                chkval = True
                Tolerence = 2
                'LoopCnt = 150
                'LineID(2) = Calc_Line(Tolerence, Line3Slut - 25, Line3Start, Direction)      ' Calc_Line(Tolerence, Startpkt, Slutpkt, retning)
                'LineID(2).PointData.EndPkt = LineID(2).PointData.EndPkt + 8
                'Form1.DrawLineFct(LineID(2), Color.Green)
            End If


            No_Lines = No_Lines - 1
        End While
        'Return

        ' skæringspunkter for Identificeringspunktet og skæringen med den modstående side
        Dim Intersect1, Intersect2 As Coord
        'Intersect1 = InterSect(LineID(0), LineID(1))    ' vandrett og modstående linier
        'Intersect2 = InterSect(LineID(1), LineID(2))    ' ID punktet

        'Area_Cal(Intersect1, Intersect2, LineID(1))

        ' marker skæringspunkterne mellem linierne
        'Form1.MarkLineEnd(Intersect1.X, LineID(0), Color.Red)
        'Form1.MarkLineEnd(Intersect2.X, LineID(1), Color.Red)


        ' tegne den vandrette linie og markere ID punktet
        Dim x0, y0 As Double
        'x0 = ((Laser(ActLine, LineID(1).PointData.StartPkt).X_Pos / 100))
        'y0 = (((LineID(1).B * x0 + LineID(1).A)))
        'Form1.MakeLine(Intersect1.X, x0, Intersect1.Y, y0, Color.Green)

        'Form1.Picturebox_Txt(Intersect1.X, Intersect1.Y, 0)
        'Form1.Picturebox_Txt(Intersect2.X, Intersect2.Y, 0)
        'Form1.TextBox8.Text = Form1.TextBox8.Text + Str(Intersect1.X) + "  -  " + Str(Intersect1.Y) + "  -  " + Str(Intersect2.X) + "  -  " + Str(Intersect2.Y) + vbCrLf




    End Sub


    Public Sub IdentifyLine(ByVal No_Lines As Integer)
        Dim MaesVal As Double
        Dim Cnt As Integer
        Dim LineDef As Integer = 50
        Dim LineAng As Double
        Dim SumVal As Double
        Dim GnsMaes As Double
        Dim Tolerence As Double
        Dim GapCnt As Integer
        Dim LineCnt As Integer = 0
        Dim StartPkt, SlutPkt As Integer
        Dim Line3Start, Line3Slut As Integer
        Dim Direction As Integer


        Form1.TextBox8.Text = ""

        While LineCnt < No_Lines

            StartPkt = 1
            Cnt = 0
            GnsMaes = 0
            GapCnt = 0
            SumVal = 0
            ValidMeas = 1279

            LineCnt = LineCnt + 1

            While Cnt < ValidMeas
                Cnt = Cnt + 1
                ' vi regner med at starte med en lige line
                If LineCnt = 1 Then
                    MaesVal = Maaling(Cnt)                  ' fra højre (som er fra måling 1 til 1280 
                    Tolerence = 0.5
                    'Form1.TextBox8.Text = Form1.TextBox8.Text + Str(Cnt) + " - " + Str(MaesVal) + " - " + Str(GnsMaes) + vbCrLf
                End If
                If LineCnt = 2 Then
                    MaesVal = Maaling(ValidMeas - Cnt)      ' fra venstre (som er fra måling 1280 - 1)
                    Tolerence = 2 ' 0.001
                End If

                If Cnt = 1 Then             ' gælder kun for første måling.
                    GnsMaes = MaesVal       ' vi sikrer os at hvis linien er omkring vandret, så kigger vi på en hældning mellem +/- 10 grader
                End If

                If MaesVal > 350 Then
                    MaesVal = MaesVal - 360
                Else
                    If Math.Abs(MaesVal - GnsMaes) > 350 Then   ' når GnsMaes ligger mellem 10 / 350 grader så regnes vinklerne altid i +/- 10 grader
                        MaesVal = MaesVal - 360
                    End If
                End If

                If Cnt <= LineDef Then      ' Cnt tæller op og beregner indtil der er nok målinger til at finde linjen
                    SumVal = SumVal + MaesVal
                    GnsMaes = SumVal / Cnt
                    LineAng = GnsMaes       ' det er vinklen vi sammenligner med
                End If

                If Cnt > LineDef Then       ' Cnt overstiger det antal der skal til for at finde en linje, nu kigger vi efter hvornår det ændrer sig.
                    If Math.Abs(MaesVal - GnsMaes) > Tolerence Then
                        GapCnt = GapCnt + 1
                    Else
                        SumVal = SumVal + MaesVal
                        GnsMaes = SumVal / Cnt

                        GapCnt = 0
                    End If

                    If GapCnt > MaxClusterGap Then
                        If No_Lines = 3 Then
                            SlutPkt = Cnt + GapCnt + LoopCnt
                            Cnt = ValidMeas
                        End If
                        If No_Lines = 2 Then
                            SlutPkt = ValidMeas - (Cnt + GapCnt + LoopCnt)
                            Cnt = ValidMeas
                        End If


                    End If
                End If

            End While

            ' Find liniens præcise slutpunkt

            If LineCnt = 1 Then    ' her starter vi fra begyndelsen som altid er en lige linie.
                chkval = True
                Tolerence = 1  ' org val 2.8  ' da reflektionerne kan være slemme på denne side er tolerencen størst, men da vinklen er stejl gør det ikke så meget
                Direction = 1
                LineID(0) = Calc_Line(Tolerence, StartPkt, SlutPkt, Direction)          ' Calc_Line(Tolerence, Startpkt, Slutpkt, retning)
                LineID(0).PointData.EndPkt = LineID(0).PointData.EndPkt - 8
                Form1.DrawLineFct(LineID(0), Color.Green)
            End If
            If LineCnt = 2 Then    ' her starter vi bagfra - altså fra sidste ´punkt og kører modsat
                chkval = True
                Tolerence = 0.5
                Direction = -1
                LineID(1) = Calc_Line(Tolerence, 1279, (SlutPkt - LoopCnt), Direction)  ' Calc_Line(Tolerence, Startpkt, Slutpkt, retning)

                'Form1.DrawLineFct(LineID(1), Color.Green)
            End If

            If LineCnt = 3 Then
                Line3Start = LineID(0).PointData.EndPkt
                Line3Slut = LineID(1).PointData.EndPkt
                If Line3Start < Line3Slut Then
                    Direction = -1
                Else
                    Direction = 1
                End If

                chkval = True
                Tolerence = 2
                'LoopCnt = 150
                LineID(2) = Calc_Line(Tolerence, Line3Slut - 25, Line3Start, Direction)      ' Calc_Line(Tolerence, Startpkt, Slutpkt, retning)
                LineID(2).PointData.EndPkt = LineID(2).PointData.EndPkt + 8
                'Form1.DrawLineFct(LineID(2), Color.Green)
            End If


            No_Lines = No_Lines - 1
        End While
        'Return

        ' skæringspunkter for Identificeringspunktet og skæringen med den modstående side
        Dim Intersect1, Intersect2 As Coord
        'Intersect1 = InterSect(LineID(0), LineID(1))    ' vandrett og modstående linier
        'Intersect2 = InterSect(LineID(1), LineID(2))    ' ID punktet

        'Area_Cal(Intersect1, Intersect2, LineID(1))

        ' marker skæringspunkterne mellem linierne
        'Form1.MarkLineEnd(Intersect1.X, LineID(0), Color.Red)
        'Form1.MarkLineEnd(Intersect2.X, LineID(1), Color.Red)


        ' tegne den vandrette linie og markere ID punktet
        Dim x0, y0 As Double
        'x0 = ((Laser(ActLine, LineID(1).PointData.StartPkt).X_Pos / 100))
        'y0 = (((LineID(1).B * x0 + LineID(1).A)))
        'Form1.MakeLine(Intersect1.X, x0, Intersect1.Y, y0, Color.Green)

        'Form1.Picturebox_Txt(Intersect1.X, Intersect1.Y, 0)
        'Form1.Picturebox_Txt(Intersect2.X, Intersect2.Y, 0)
        'Form1.TextBox8.Text = Form1.TextBox8.Text + Str(Intersect1.X) + "  -  " + Str(Intersect1.Y) + "  -  " + Str(Intersect2.X) + "  -  " + Str(Intersect2.Y) + vbCrLf

    End Sub

    Public Sub Houghs(ByVal No_Lines As Integer)
        Dim N1, N2 As Double
        Dim Hough(180, 1280) As Double
        Dim Hough_Mid(180, 1280) As Double
        Dim Comp_Val As Double
        Dim X1, Y1, x2, y2 As Double
        Dim Range As Double = 0.05
        Dim ang1 As Double
        Dim PI As Double = 3.1415926535
        Dim Ang_select(180) As Integer
        Dim Line_Term As Boolean     ' bliver true når vinkel selectoren bliver opfyldt
        Dim Selector As Integer = 0
        Dim StartPkt, EndPkt As Integer
        Dim Line_lgt As Integer = 50
        Dim Rho_Gns As Double
        Dim Mid_Size As Integer = 15
        Dim PointCnt As Integer
        Dim LineCnt As Integer = 0
        Dim line_select(1280, 3) As Integer
        Dim New_Line As Boolean = False
        Dim Diff1 As Double = 0.12



        For j = 1 To 180
            Line_Term = False
            ang1 = j * PI / 180
            Comp_Val = (Laser(ActLine, 1).X_Pos / 100 * Math.Cos(ang1)) + (Laser(ActLine, 1).Y_Pos / 100 * Math.Sin(ang1))

            Rho_Gns = 0
            For i = 1 To Mid_Size
                X1 = Laser(ActLine, i).X_Pos / 100
                Y1 = Laser(ActLine, i).Y_Pos / 100
                N1 = (X1 * Math.Cos(ang1)) + (Y1 * Math.Sin(ang1))
                Rho_Gns = Rho_Gns + (N1 / Mid_Size)
                If (Math.Abs(N1 - Rho_Gns) < Diff1) Then
                    LineCnt = LineCnt + 1
                    If line_select(j - 1, 1) = j Then
                        line_select(j, 1) = j
                        line_select(j, 2) = N1
                    End If
                    If line_select(j, 1) = 0 Then
                        line_select(j, 1) = j
                        line_select(j, 2) = N1
                    End If
                Else
                    LineCnt = 0
                End If
            Next
            Hough(j, 1) = N1
            Hough(j, 2) = Rho_Gns

        Next


        chk_timer = True
        'Return

        Dim content As String
        'Dim FILE_NAME As String = "C:\data\apco\Hough02" + ".txt"
        Dim FILE_NAME As String = "C:\Data\VB koder\3DScan Vfuge 180gr\Hough_Sel" + ".txt"
        Dim objWriter As New System.IO.StreamWriter(FILE_NAME)
        content = ""

        For j = 1 To 180
            objWriter.WriteLine(Str(j) + " , " + Str(line_select(j, 1)) + " , " + Str(line_select(j, 2)))

        Next

        objWriter.Close()
        objWriter.Dispose()

        Return
        ' --------------------------

        FILE_NAME = "C:\Data\VB koder\3DScan Vfuge 180gr\Hough_Data" + ".txt"
        Dim objWriter2 As New System.IO.StreamWriter(FILE_NAME)

        content = ""

        For j = 1 To 180
            For i = 1 To 1279
                content = Str(j) + " , " + Str(i) + " , " + Str(Hough(j, i)) + " , " + Str(Hough_Mid(j, i))
                objWriter2.WriteLine(content)
            Next
        Next
        objWriter2.Close()
        objWriter2.Dispose()


    End Sub


    Public Sub Houghs2(ByVal No_Lines As Integer)
        Dim N1, N_Chk As Integer
        Dim Hough(180, 1280) As Double
        Dim Hough_Mid(180, 1280) As Double
        Dim Comp_Val As Double
        Dim X1, Y1, x2, x3 As Double
        Dim Range As Double = 0.05
        Dim ang1 As Double
        Dim PI As Double = 3.1415926535
        Dim Ang_select(180) As Integer
        Dim Line_Term As Boolean     ' bliver true når vinkel selectoren bliver opfyldt
        Dim Selector As Integer = 0
        Dim StartPkt, EndPkt As Integer
        Dim Line_lgt As Integer = 50
        Dim Rho_Gns As Double
        Dim RHO As Double
        Dim Mid_Size As Integer = 10    ' den gennemsnitlige værdi af 
        Dim PointCnt As Integer
        Dim LineCnt As Integer = 0
        Dim line_select(100, 3) As Integer
        Dim New_Line As Boolean = False
        Dim RHO_udfald(1280) As Integer
        Dim val1 As Integer
        

        For j = 1 To 180            ' fra 0 til 180 grader
            Line_Term = False
            ang1 = j * PI / 180   ' Theta i Radianer
            'Comp_Val = (Laser(ActLine, 1).X_Pos / 100 * Math.Cos(ang1)) + (Laser(ActLine, 1).Y_Pos / 100 * Math.Sin(ang1))

            Rho_Gns = 0
            For i = 1 To 1280 Step 1
                X1 = Laser(ActLine, i).X_Pos / 100      ' beregn sand X val
                Y1 = Laser(ActLine, i).Y_Pos / 100      ' beregn sand Y val
                RHO = (X1 * Math.Cos(ang1)) + (Y1 * Math.Sin(ang1))     ' Calculating Rho med vinklen angivet i radianer
                val1 = Int(RHO)
                If val1 < 0 Then val1 = 1200 - Int(RHO)

                RHO_udfald(val1) = RHO_udfald(val1) + 1
                Hough(j, i) = RHO
                If i <= Mid_Size Then
                    Rho_Gns = Rho_Gns + (Hough(j, i) / Mid_Size)
                Else
                    Rho_Gns = (((Mid_Size - 1) * Rho_Gns) + (Hough(j, i))) / Mid_Size
                    Hough_Mid(j, i) = Rho_Gns
                    If (Math.Abs(Hough(j, i) - Rho_Gns) < 0.1) Then  ' punktet ligger på en linje
                        PointCnt = PointCnt + 1
                        EndPkt = i
                        StartPkt = EndPkt - PointCnt
                    Else
                        If PointCnt > 50 Then
                            If LineCnt = 0 Then
                                LineCnt = 1
                                line_select(1, 1) = StartPkt
                                line_select(1, 2) = EndPkt
                            End If
                            Dim MidPktNy As Integer = (StartPkt + (PointCnt / 2))
                            Dim MidPktGl As Integer

                            Dim LoopCnt As Integer = 0

                            For k = 1 To LineCnt
                                MidPktGl = (line_select(k, 1) + ((line_select(k, 2) - line_select(k, 1)) / 2))
                                If ((MidPktNy > line_select(k, 1)) And (MidPktNy < line_select(k, 2))) Or _
                                    ((MidPktGl > StartPkt) And (MidPktGl < StartPkt)) Then ' samme linjestykke som fundet før
                                    If (EndPkt - StartPkt) > (line_select(k, 2) - line_select(k, 1)) Then
                                        line_select(k, 1) = StartPkt
                                        line_select(k, 2) = EndPkt
                                        line_select(k, 3) = j
                                    End If
                                    New_Line = True
                                    LoopCnt = LoopCnt + 1
                                End If

                            Next
                            If LoopCnt <> LineCnt Then
                                LineCnt = LineCnt + 1
                                line_select(LineCnt, 1) = StartPkt
                                line_select(LineCnt, 2) = EndPkt
                                line_select(LineCnt, 3) = j
                                New_Line = False
                            End If

                            PointCnt = PointCnt
                        End If
                        PointCnt = 0
                    End If
                    'If (Abs(Hough(j, i) - Comp_Val) > 0.5) And (Line_Term = False) Then   ' vinkel selector
                    'Line_Term = True
                    'Ang_select(j) = i
                    'If i > Selector Then
                    'Selector = i
                    'Best_Ang = j
                    'End If
                    'i = 1280
                    'End If ' ud
                End If
            Next
        Next


        chk_timer = True
        'Return

        Dim content As String
        'Dim FILE_NAME As String = "C:\data\apco\Hough02" + ".txt"
        Dim FILE_NAME As String = "C:\Data\VB koder\3DScan Vfuge 180gr\Hough_Sel2" + ".txt"
        Dim objWriter As New System.IO.StreamWriter(FILE_NAME)
        content = ""

        For j = 1 To 100
            objWriter.WriteLine(Str(j) + " , " + Str(line_select(j, 3)) + " , " + Str(line_select(j, 1)) + " , " + Str(line_select(j, 2)))

        Next

        objWriter.Close()
        objWriter.Dispose()

        ' --------------------------

        FILE_NAME = "C:\Data\VB koder\3DScan Vfuge 180gr\Hough_Data24" + ".txt"
        Dim objWriter2 As New System.IO.StreamWriter(FILE_NAME)

        content = ""

        'For j = 1 To 180
        For i = 1 To 1279
            X1 = Laser(ActLine, i).X_Pos / 100      ' slet
            Y1 = Laser(ActLine, i).Y_Pos / 100      ' slet
            'content = Str(j) + " , " + Str(i) + " , " + Str(Hough(j, i)) + " , " + Str(Hough_Mid(j, i))
            content = Str(i) + " , " + Str(X1) + " , " + Str(Y1)
            objWriter2.WriteLine(content)
        Next
        'Next
        objWriter2.Close()
        objWriter2.Dispose()


    End Sub




End Module
