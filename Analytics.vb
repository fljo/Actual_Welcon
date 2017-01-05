Imports System
Imports System.IO
Imports System.Data
'Imports System.Text
Imports System.Math


Public Module Analytics

    Dim CornerPoints(3) As Coord




    Public Function FindLinje(ByVal Start As Integer, ByVal Slut As Integer, ByVal Sign As Integer, ByVal MaxError As Single, ByVal Metode As Integer) As LinesFound

        'metode = 1 betyder at vi søger fint
        'metode = 2 betyder at vi søger lidt grovere. så kan vi altid finsøge bagefter
        ' metode 3 er fra start til slut

        Dim n, Arr_Index As Integer
        Dim X_Act, Z_Act As Double
        Dim X_Sum, Z_Sum, X_Mid, Z_Mid, Xm_Sum1, Zm_Sum1, b0, b1 As Double
        Dim SL_X_Sum, SL_Z_Sum, SL_X_Mid, SL_Z_Mid, SL_Xm_Sum1, SL_Zm_Sum1, SL_b0, SL_b1 As Double
        Dim Linenr As Integer = 2
        Dim Dist As Double
        Dim LoopEnd As Boolean = False
        Dim LoopCnt As Integer
        Dim XVal_Arr(130) As Double
        Dim ZVal_Arr(130) As Double
        Dim XMid_Arr(130) As Double
        Dim ZMid_Arr(130) As Double
        Dim TestLgd As Integer = 50
        Dim Dist1 As Single
        Dim X_Tmp, Z_Tmp As Single
        Dim loop1 As Integer

        n = -1
        Arr_Index = 0
        LoopCnt = Start
        While LoopEnd = False
            Arr_Index = Arr_Index + 1
            n = n + 1
            X_Act = Laser(Linenr, LoopCnt).X_Pos / 100
            Z_Act = Laser(Linenr, LoopCnt).Z_Pos / 100

            'MinLineLgt = Val(ScanViewer.TextBox3.Text)

            ' her findes udgangsliningen for den videre analyse af den rette linje og hvornår den slutter
            ' beregningen bygger på lineær regression (least square method)

            ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            '------- beregner for hele linjestykket
            X_Sum = X_Sum + X_Act       ' summen af X
            Z_Sum = Z_Sum + Z_Act       ' summen af Z
            X_Mid = X_Sum / (n + 1)           ' gennemsnittet af X
            Z_Mid = Z_Sum / (n + 1)           ' gennemsnittet af Z
            Xm_Sum1 = Xm_Sum1 + X_Mid   ' summen af X's gennemsnit
            Zm_Sum1 = Zm_Sum1 + Z_Mid   ' summen af Z's gennemsnit


            '     linjens ligning hedder: Y = aX + b
            b1 = ((X_Sum - Xm_Sum1) * (Z_Sum - Zm_Sum1)) / ((X_Sum - Xm_Sum1) * (X_Sum - Xm_Sum1))  ' hældningen af linjen (a)
            b0 = Z_Mid - (b1 * X_Mid)                                                               ' skæringspunktet med Y-aksen (b)

            'FORMEL: Dist = abs(ax + b - y) / (Math.Sqrt(a^2 + 1)
            Dist = (b1 * X_Act + b0 - Z_Act) / Math.Sqrt(b1 ^ 2 + 1)                             ' afstanden mellem det aktuelle punkt og det's X-værdi's repræsentation på den fundne linje
            FindLinje.Fejl = Dist
            ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            ' HER LAVER VI EN PARALLEL BEREGNING OVER ET KORTERE LINJESTYKKE FOR AT FANGE UTILSIGTEDE KRUMNINGER PÅ LINJEN


            ' genberegner værdierne der skal bruges til at analysere det nye punkts afvigelse fra linjen.
            ' der arbejdes hele tiden med et fast antal målinger [MinLineLgt] således at den første måling fratrækkes og den sidste lægges til.
            ' således bliver der hele tiden sammenlignet med et kort linjestykke, da længere linjestykker har en større afvigelse på grund af svøbets radius.
            If n < TestLgd Then
                XVal_Arr(Arr_Index) = X_Act
                SL_X_Sum = SL_X_Sum + X_Act      ' læg den nye værdi til summen

                SL_X_Mid = SL_X_Sum / Arr_Index
                XMid_Arr(Arr_Index) = SL_X_Mid
                SL_Xm_Sum1 = SL_Xm_Sum1 + SL_X_Mid


                ZVal_Arr(Arr_Index) = Z_Act
                SL_Z_Sum = SL_Z_Sum + Z_Act

                SL_Z_Mid = SL_Z_Sum / Arr_Index
                ZMid_Arr(Arr_Index) = SL_Z_Mid
                SL_Zm_Sum1 = SL_Zm_Sum1 + SL_Z_Mid
                If n = 28 Then
                    n = n
                End If
            Else
                Arr_Index = n - (Int(n / TestLgd) * TestLgd) + 1

                SL_X_Sum = SL_X_Sum - XVal_Arr(Arr_Index) + X_Act     ' træk den gamle værdi fra summen
                XVal_Arr(Arr_Index) = X_Act             ' gem den nye værdi

                SL_X_Mid = SL_X_Sum / TestLgd
                SL_Xm_Sum1 = SL_Xm_Sum1 - XMid_Arr(Arr_Index) + SL_X_Mid
                XMid_Arr(Arr_Index) = SL_X_Mid


                SL_Z_Sum = SL_Z_Sum - ZVal_Arr(Arr_Index) + Z_Act
                ZVal_Arr(Arr_Index) = Z_Act

                SL_Z_Mid = SL_Z_Sum / TestLgd
                SL_Zm_Sum1 = SL_Zm_Sum1 - ZMid_Arr(Arr_Index) + SL_Z_Mid
                ZMid_Arr(Arr_Index) = SL_Z_Mid

                SL_b1 = ((SL_X_Sum - SL_Xm_Sum1) * (SL_Z_Sum - SL_Zm_Sum1)) / ((SL_X_Sum - SL_Xm_Sum1) * (SL_X_Sum - SL_Xm_Sum1))
                SL_b0 = SL_Z_Mid - (SL_b1 * SL_X_Mid)
                Dist1 = (SL_b1 * X_Act + SL_b0 - Z_Act) / Math.Sqrt(SL_b1 ^ 2 + 1)                             ' afstanden mellem det aktuelle punkt og det's X-værdi's repræsentation på den fundne linje
                FindLinje.Fejl = Dist1
                Dist1 = Dist1

                If n = 79 Then
                    n = n
                End If
            End If




            ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


            'If ((Sign = 1) And (Dist < (MaxError * -1))) Or ((Sign = -1) And Dist > MaxError)) Then
            If (n > TestLgd) And (Metode <> 3) Then  ' vi skal minimum have 20 punkter
                If (Abs(Dist) > MaxError) Then        ' hvis punktet ligger for langt fra den fundne linje

                    If (Abs(Dist1) < (MaxError / 2)) And (Metode = 1) Then   '-- vi checker om vi også ligger for langt udenfor det lille linestykke
                        ' hvis afvigelsen ligger indenfor det lille linjestykkes grænser, så er det fordi linjen vi analyserer buer en smule
                        If n > (2 * TestLgd) Then
                            loop1 = LoopCnt - Abs((n / 2) * Sign)
                            'n = loop1 - 1
                        Else
                            loop1 = LoopCnt - (TestLgd * Sign)
                            'n = loop1 - 1
                        End If
                        Dim lop1_Cnt As Integer = 0

                        X_Sum = 0
                        Z_Sum = 0
                        X_Mid = 0
                        Z_Mid = 0
                        Xm_Sum1 = 0
                        Zm_Sum1 = 0
                        While loop1 <> LoopCnt
                            lop1_Cnt = lop1_Cnt + 1
                            X_Tmp = Laser(Linenr, loop1).X_Pos / 100
                            Z_Tmp = Laser(Linenr, loop1).Z_Pos / 100

                            X_Sum = X_Sum + X_Tmp       ' summen af X
                            Z_Sum = Z_Sum + Z_Tmp       ' summen af Z
                            X_Mid = X_Sum / lop1_Cnt           ' gennemsnittet af X
                            Z_Mid = Z_Sum / lop1_Cnt           ' gennemsnittet af Z
                            Xm_Sum1 = Xm_Sum1 + X_Mid   ' summen af X's gennemsnit
                            Zm_Sum1 = Zm_Sum1 + Z_Mid   ' summen af Z's gennemsnit


                            loop1 = loop1 + Sign
                        End While
                        b1 = ((X_Sum - Xm_Sum1) * (Z_Sum - Zm_Sum1)) / ((X_Sum - Xm_Sum1) * (X_Sum - Xm_Sum1))  ' hældningen af linjen (a)
                        b0 = Z_Mid - (b1 * X_Mid)                                                               ' skæringspunktet med Y-aksen (b)
                        'FORMEL: Dist = abs(ax + b - y) / (Math.Sqrt(a^2 + 1)
                        Dist = (b1 * X_Act + b0 - Z_Act) / Math.Sqrt(b1 ^ 2 + 1)                             ' afstanden mellem det aktuelle punkt og det's X-værdi's repræsentation på den fundne linje

                        n = lop1_Cnt


                    Else
                        ' hvis punktet ligger udenfor både den lange og den korte linjes grænser, så er det nok bunden, med mindre der kommer en knast på linjen
                        ' hvis der kommer en knast i fugen, så forsætter linjen nok nedenunder. det tjekker vi lige...

                        X_Tmp = Laser(Linenr, LoopCnt + 10).X_Pos / 100
                        Z_Tmp = Laser(Linenr, LoopCnt + 10).Z_Pos / 100
                        Dist = (b1 * X_Tmp + b0 - Z_Tmp) / Math.Sqrt(b1 ^ 2 + 1)
                        If Abs(Dist) > MaxError Then
                            LoopEnd = True
                        Else
                            'LoopEnd = True
                            LoopCnt = LoopCnt + (10 * Sign)
                        End If
                    End If

                Else
                    If LoopCnt = Slut Then
                        LoopEnd = True
                    End If

                End If
            End If
            LoopCnt = LoopCnt + Sign
            If LoopCnt > 498 Then
                LoopCnt = LoopCnt
            End If
            If LoopCnt = Slut Then
                LoopEnd = True
            End If

        End While

        FindLinje.StartPkt.Nr = Start
        FindLinje.StartPkt.X = (Laser(Linenr, Start).X_Pos / 100)
        FindLinje.StartPkt.Z = (Laser(Linenr, Start).Z_Pos / 100)       'b1 * (Laser(Linenr, Start).X_Pos / 100) + b0
        FindLinje.SlutPkt.X = (Laser(Linenr, LoopCnt).X_Pos / 100)
        FindLinje.SlutPkt.Z = (Laser(Linenr, LoopCnt).Z_Pos / 100)       ' b1 * (Laser(Linenr, LoopCnt).X_Pos / 100) + b0
        FindLinje.SlutPkt.Nr = LoopCnt
        FindLinje.a = b1
        FindLinje.b = b0
        FindLinje.Angle = Atan(b1) * 180 / Math.PI
        'ScanViewer.MakeLine(FindLinje, Color.Green)

    End Function


    Public Function FindEnd(ByVal SelLine As LinesFound, ByVal Retning As Integer, ByVal MaxError As Single) As LinesFound
        Dim Dist As Single
        Dim b1 As Single = SelLine.a
        Dim b0 As Single = SelLine.b
        Dim X_Act As Single
        Dim Z_Act As Single
        Dim LoopCnt As Integer
        Dim LoopEnd As Boolean = False

        FindEnd = SelLine
        LoopCnt = SelLine.SlutPkt.Nr

        While LoopEnd = False
            X_Act = Laser(2, LoopCnt).X_Pos / 100
            Z_Act = Laser(2, LoopCnt).Z_Pos / 100

            'FORMEL: Dist = abs(ax + b - y) / (Math.Sqrt(a^2 + 1)
            Dist = Abs(b1 * X_Act + b0 - Z_Act) / Math.Sqrt(b1 ^ 2 + 1)        ' afstanden mellem det aktuelle punkt og det's X-værdi's repræsentation på den fundne linje

            If Dist > MaxError Then
                LoopCnt = LoopCnt + Retning
            Else
                LoopEnd = True
            End If
        End While

        FindEnd.SlutPkt.Nr = LoopCnt
        FindEnd.SlutPkt.X = X_Act
        FindEnd.SlutPkt.Z = b1 * X_Act + b0
        FindEnd.Fejl = Dist
        Dim ta As Double = (SelLine.StartPkt.Z - FindEnd.SlutPkt.Z) / (SelLine.StartPkt.X - FindEnd.SlutPkt.X)
        Dim tb As Double = FindEnd.SlutPkt.Z - (ta * FindEnd.SlutPkt.X)
        ScanViewer.MakeLine(FindEnd, Color.Blue)
        'FindEnd.a = ta
        'FindEnd.b = tb
        'Dist = (b1 * SelLine.StartPkt.X + b0 - SelLine.StartPkt.Z) / Math.Sqrt(b1 ^ 2 + 1)
    End Function

End Module
