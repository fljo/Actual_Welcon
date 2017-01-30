Imports System
Imports System.IO
Imports System.Data
'Imports System.Text
Imports System.Math


Public Module Analytics

    Dim CornerPoints(3) As Coord


    Public Sub LayerLevel()
        Dim ang_nn, Dist As Single
        Dim CheckLinje As LinesFound

        FoundLine(1) = LineRegress1(Scan_Draw_Mode, 1, Math.Round(ValidMeas * 2 / 3))
        FoundLine(2) = LineRegress1(Scan_Draw_Mode, ValidMeas, Math.Round(ValidMeas / 3))

        For i = 1 To 30
            'CheckLinje = FindLinje(FoundLine(1).SlutPkt.Nr + i, FoundLine(1).SlutPkt.Nr + (i + 20), 1, 0, 2)
            ang_nn = CheckLinje.Angle
            Dist = CheckLinje.Fejl
        Next
        For i = 1 To 30
            'CheckLinje = FindLinje(FoundLine(2).SlutPkt.Nr - i, FoundLine(2).SlutPkt.Nr - (i + 20), -1, 0, 2)
            ang_nn = CheckLinje.Angle
            Dist = CheckLinje.Fejl
        Next

        'FoundLine(3) = LineRegress3(Scan_Draw_Mode, FoundLine(1).SlutPkt.Nr + 5, ValidMeas - 100)
        'FoundLine(4) = LineRegress3(Scan_Draw_Mode, FoundLine(2).SlutPkt.Nr - 5, 100)

        Form1.MakeLine(FoundLine(1), Color.Green)
        Form1.MakeLine(FoundLine(2), Color.Green)
        Form1.MakeLine(FoundLine(3), Color.Green)
        Form1.MakeLine(FoundLine(4), Color.Green)
        'Return
        CornerPoints(1) = InterSect(FoundLine(1), FoundLine(3))
        CornerPoints(1).Nr = FoundLine(1).SlutPkt.Nr

        CornerPoints(2) = InterSect(FoundLine(2), FoundLine(4))
        CornerPoints(2).Nr = FoundLine(2).SlutPkt.Nr

        FoundLine(1).SlutPkt = CornerPoints(1)
        FoundLine(3).StartPkt = CornerPoints(1)
        'FoundLine(2).StartPkt = CornerPoints(2)
        FoundLine(2).SlutPkt = CornerPoints(2)
        FoundLine(4).StartPkt = CornerPoints(2)
        Dim MinZ As Single
        Dim StartNo, SlutNo As Integer
        Dim Cnt As Integer
        Dim tal As Single
        If FoundLine(3).SlutPkt.Z < FoundLine(4).SlutPkt.Z Then
            MinZ = FoundLine(3).SlutPkt.Z
            StartNo = FoundLine(3).SlutPkt.Nr
            Cnt = FoundLine(4).SlutPkt.Nr
            'tal = Laser(Scan_Draw_Mode, Cnt).Z_Pos / 100

            'While Laser(Scan_Draw_Mode, Cnt).Z_Pos / 100 > MinZ
            'Cnt = Cnt + 1
            'End While

        Else
            MinZ = FoundLine(4).SlutPkt.Z
        End If
        For i = FoundLine(3).SlutPkt.Nr To FoundLine(4).SlutPkt.Nr

        Next
        'FindLinje(FoundLine(3).SlutPkt.Nr, FoundLine(4).SlutPkt.Nr, 1, 0, 2)
        'ScanViewer.MakeLine(FoundLine(3), Color.Green)

        'ScanViewer.MakeLine(FoundLine(4), Color.Green)


    End Sub



    Public Function FindLinje(ByVal Start As Integer, ByVal Slut As Integer, ByVal Sign As Integer, ByVal MaxError As Single, _
                               ByVal Metode As Integer, MinAng As Single, TestLgd As Integer) As LinesFound
        ' Sign angiver retningen vi beregner, altså om det er fra lavere mod højere eller omvendt
        ' MaxError angiver hvor stor afvigelsen må være fra den linje vi har fundet
        ' metode = 1 betyder at vi søger fint
        ' metode = 2 betyder at vi søger lidt grovere. så kan vi altid finsøge bagefter
        ' metode = 3 er fra start til slut - bruges til at finde hældningen på svejsestrengen(e)
        ' metode = 4 her køres regression i længden [TestLgd] indtil der findes en fejl (afvigelse) større end MaxError. 
        '           kan bruges til øverste kant, hvor vinkel offset'et (fejl - vinkel = afstand) kan beregnes
        ' MinAng er minimumsvinklen der skal være mellem de to små linjestykker for at vi antager der er tale om en afslutning. Bruges især på krumme flader
        ' TestLgd er længden (antal punkter) der minimum skal være i linjestykkerne.

        Dim n, Arr_Index As Integer
        Dim X_Act, Z_Act As Double
        Dim X_Frem, Z_Frem As Double
        Dim X_Sum, Z_Sum, X_Mid, Z_Mid, Xm_Sum1, Zm_Sum1, b0, b1 As Double
        Dim FL_X_Sum, FL_Z_Sum, FL_X_Mid, FL_Z_Mid, FL_Xm_Sum1, FL_Zm_Sum1, FL_b0, FL_b1 As Double
        Dim LL_X_Sum, LL_Z_Sum, LL_X_Mid, LL_Z_Mid, LL_Xm_Sum1, LL_Zm_Sum1, LL_b0, LL_b1 As Double
        Dim Linenr As Integer = 1
        Dim Dist As Double
        Dim LoopEnd As Boolean = False
        Dim LoopCnt As Integer
        Dim FL_XVal_Arr(230) As Double
        Dim FL_ZVal_Arr(230) As Double
        Dim FL_XMid_Arr(230) As Double
        Dim FL_ZMid_Arr(230) As Double
        Dim LL_XVal_Arr(230) As Double
        Dim LL_ZVal_Arr(230) As Double
        Dim LL_XMid_Arr(230) As Double
        Dim LL_ZMid_Arr(230) As Double
        Dim LL_Vinkel, FL_Vinkel As Double
        Dim CheckOffs_X As Integer          ' bruges i metode 4, hvor punktet hvor vi målet Z-værdien ligger (MaxError i X-retningen) foran Regressionslinjen
        Dim CheckOffs_Z As Single          ' bruges i metode 4, hvor punktet hvor vi målet Z-værdien ligger (MaxError i X-retningen) foran Regressionslinjen
        Dim X_Error As Single
        Dim Z_Error As Single
        Dim Dist1, Dist2 As Single
        Dim Vinkel_Diff As Double
        Dim X_Tmp, Z_Tmp As Single
        Dim loop1 As Integer
        Dim MaxSecError As Single
        Dim MaxVinkel As Single = 0
        Dim Line_Is_Straight As Boolean = True
        Dim Start1 As Integer = Start
        n = -1
        Arr_Index = 0
        LoopCnt = Start
        While LoopEnd = False
            Arr_Index = Arr_Index + 1
            If Arr_Index > TestLgd Then
                Arr_Index = 1
            End If

            n = n + 1
            X_Act = Laser(Linenr, LoopCnt).X_Pos / 100
            Z_Act = Laser(Linenr, LoopCnt).Z_Pos / 100

            ' herunder beregnes antallet af punkter der skal kigges frem for at 'fejlen' er MaxError stor ved en vinkel på 35%
            CheckOffs_X = Round((MaxError * Tan(35 * PI / 180)) / (Abs(Laser(Linenr, LoopCnt).X_Pos / 100 - Laser(Linenr, LoopCnt + (3 * Sign)).X_Pos / 100) / 3))

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

            ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            ' HER LAVER VI EN PARALLEL BEREGNING OVER ET KORTERE LINJESTYKKE FOR AT FANGE UTILSIGTEDE KRUMNINGER PÅ LINJEN

            ' genberegner værdierne der skal bruges til at analysere det nye punkts afvigelse fra linjen.
            ' der arbejdes hele tiden med et fast antal målinger [MinLineLgt] således at den første måling fratrækkes og den sidste lægges til.
            ' således bliver der hele tiden sammenlignet med et kort linjestykke, da længere linjestykker har en større afvigelse på grund af svøbets radius.
            If n < TestLgd Then
                FL_XVal_Arr(Arr_Index) = X_Act
                FL_X_Sum = FL_X_Sum + X_Act      ' læg den nye værdi til summen

                FL_X_Mid = FL_X_Sum / Arr_Index
                FL_XMid_Arr(Arr_Index) = FL_X_Mid
                FL_Xm_Sum1 = FL_Xm_Sum1 + FL_X_Mid


                FL_ZVal_Arr(Arr_Index) = Z_Act
                FL_Z_Sum = FL_Z_Sum + Z_Act

                FL_Z_Mid = FL_Z_Sum / Arr_Index
                FL_ZMid_Arr(Arr_Index) = FL_Z_Mid
                FL_Zm_Sum1 = FL_Zm_Sum1 + FL_Z_Mid
                If n = 200 Then
                    n = n
                End If
            Else
                'Arr_Index = n - (Int(n / TestLgd) * TestLgd) + 1

                FL_X_Sum = FL_X_Sum - FL_XVal_Arr(Arr_Index) + X_Act     ' træk den gamle værdi fra summen og læg den nye til
                FL_XVal_Arr(Arr_Index) = X_Act             ' gem den nye værdi i array'et

                FL_X_Mid = FL_X_Sum / TestLgd
                FL_Xm_Sum1 = FL_Xm_Sum1 - FL_XMid_Arr(Arr_Index) + FL_X_Mid
                FL_XMid_Arr(Arr_Index) = FL_X_Mid


                FL_Z_Sum = FL_Z_Sum - FL_ZVal_Arr(Arr_Index) + Z_Act
                FL_ZVal_Arr(Arr_Index) = Z_Act

                FL_Z_Mid = FL_Z_Sum / TestLgd
                FL_Zm_Sum1 = FL_Zm_Sum1 - FL_ZMid_Arr(Arr_Index) + FL_Z_Mid
                FL_ZMid_Arr(Arr_Index) = FL_Z_Mid

                FL_b1 = ((FL_X_Sum - FL_Xm_Sum1) * (FL_Z_Sum - FL_Zm_Sum1)) / ((FL_X_Sum - FL_Xm_Sum1) * (FL_X_Sum - FL_Xm_Sum1))
                FL_b0 = FL_Z_Mid - (FL_b1 * FL_X_Mid)
                Dist1 = (FL_b1 * X_Act + FL_b0 - Z_Act) / Math.Sqrt(FL_b1 ^ 2 + 1)                             ' afstanden mellem det aktuelle punkt og det's X-værdi's repræsentation på den fundne linje

                FL_Vinkel = Atan(FL_b1) * 180 / Math.PI

                If n = 79 Then
                    n = n
                End If
            End If


            If Metode <> 4 Then

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX           'Beregning af et lille linjestykke BAGUD

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                ' HER LAVER VI EN PARALLEL BEREGNING OVER ET KORTERE LINJESTYKKE FOR AT FANGE UTILSIGTEDE KRUMNINGER PÅ LINJEN

                ' genberegner værdierne der skal bruges til at analysere det nye punkts afvigelse fra linjen.
                ' der arbejdes hele tiden med et fast antal målinger [MinLineLgt] således at den første måling fratrækkes og den sidste lægges til.
                ' således bliver der hele tiden sammenlignet med et kort linjestykke, da længere linjestykker har en større afvigelse på grund af svøbets radius.

                X_Frem = Laser(Linenr, LoopCnt + (TestLgd * Sign)).X_Pos / 100
                Z_Frem = Laser(Linenr, LoopCnt + (TestLgd * Sign)).Z_Pos / 100


                If n < TestLgd Then
                    LL_XVal_Arr(Arr_Index) = X_Frem
                    LL_X_Sum = LL_X_Sum + X_Frem      ' læg den nye værdi til summen

                    LL_X_Mid = LL_X_Sum / Arr_Index
                    LL_XMid_Arr(Arr_Index) = LL_X_Mid
                    LL_Xm_Sum1 = LL_Xm_Sum1 + LL_X_Mid


                    LL_ZVal_Arr(Arr_Index) = Z_Frem
                    LL_Z_Sum = LL_Z_Sum + Z_Frem

                    LL_Z_Mid = LL_Z_Sum / Arr_Index
                    LL_ZMid_Arr(Arr_Index) = LL_Z_Mid
                    LL_Zm_Sum1 = LL_Zm_Sum1 + LL_Z_Mid
                    If n = 28 Then
                        n = n
                    End If
                Else
                    'Arr_Index = n - (Int(n / TestLgd) * TestLgd) + 1

                    LL_X_Sum = LL_X_Sum - LL_XVal_Arr(Arr_Index) + X_Frem     ' træk den gamle værdi fra summen og læg den nye til
                    LL_XVal_Arr(Arr_Index) = X_Frem             ' gem den nye værdi i array'et

                    LL_X_Mid = LL_X_Sum / TestLgd
                    LL_Xm_Sum1 = LL_Xm_Sum1 - LL_XMid_Arr(Arr_Index) + LL_X_Mid
                    LL_XMid_Arr(Arr_Index) = LL_X_Mid


                    LL_Z_Sum = LL_Z_Sum - LL_ZVal_Arr(Arr_Index) + Z_Frem
                    LL_ZVal_Arr(Arr_Index) = Z_Frem

                    LL_Z_Mid = LL_Z_Sum / TestLgd
                    LL_Zm_Sum1 = LL_Zm_Sum1 - LL_ZMid_Arr(Arr_Index) + LL_Z_Mid
                    LL_ZMid_Arr(Arr_Index) = LL_Z_Mid

                    LL_b1 = ((LL_X_Sum - LL_Xm_Sum1) * (LL_Z_Sum - LL_Zm_Sum1)) / ((LL_X_Sum - LL_Xm_Sum1) * (LL_X_Sum - LL_Xm_Sum1))
                    LL_b0 = LL_Z_Mid - (LL_b1 * LL_X_Mid)
                    Dist2 = (LL_b1 * X_Frem + LL_b0 - Z_Frem) / Math.Sqrt(LL_b1 ^ 2 + 1)                             ' afstanden mellem det aktuelle punkt og det's X-værdi's repræsentation på den fundne linje

                    LL_Vinkel = Atan(LL_b1) * 180 / Math.PI

                    Vinkel_Diff = Abs(LL_Vinkel - FL_Vinkel)
                    If Vinkel_Diff > MinAng Then
                        Vinkel_Diff = Vinkel_Diff
                        If Vinkel_Diff > MaxVinkel Then
                            MaxVinkel = Vinkel_Diff
                        End If
                        If (MaxVinkel - Vinkel_Diff) > 0.5 Then
                            LoopEnd = True
                        End If
                    End If

                End If


                'Beregning af et lille linjestykke FORUD


                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


                'If ((Sign = 1) And (Dist < (MaxError * -1))) Or ((Sign = -1) And Dist > MaxError)) Then
                If (n > TestLgd) And (Metode <> 3) Then  ' vi skal minimum have 20 punkter
                    If Vinkel_Diff > MinAng Then
                        If (Abs(Dist) > MaxError) Then        ' hvis punktet ligger for langt fra den fundne linje
                            If MaxError > 0.4 Then
                                MaxSecError = MaxError / 2
                            Else
                                MaxSecError = MaxError
                            End If


                            If (Abs(Dist1) < (MaxSecError)) And (Metode = 1) Then   '-- vi checker om vi også ligger for langt udenfor det lille linestykke
                                ' hvis afvigelsen ligger indenfor det lille linjestykkes grænser, så er det fordi linjen vi analyserer buer en smule
                                If n > (TestLgd * 2) Then
                                    loop1 = LoopCnt - ((TestLgd * 2) * Sign)
                                    Start = loop1

                                Else
                                    If n > (TestLgd * 1.2) Then
                                        loop1 = Int(Start + (n * 0.2 * Sign))
                                    Else
                                        loop1 = LoopCnt - (TestLgd * Sign)
                                    End If

                                    Start = loop1
                                End If
                                Dim lop1_Cnt As Integer = 0

                                Line_Is_Straight = False    ' Vi ender her fordi linjen ikke er lige, og vi har ikke fundet afslutningen 

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

                                n = lop1_Cnt - 1


                            Else
                                ' hvis punktet ligger udenfor både den lange og den korte linjes grænser, så er det nok bunden, med mindre der kommer en knast på linjen
                                ' hvis der kommer en knast i fugen, så forsætter linjen nok nedenunder. det tjekker vi lige...

                                X_Tmp = Laser(Linenr, LoopCnt + (10 * Sign)).X_Pos / 100
                                Z_Tmp = Laser(Linenr, LoopCnt + (10 * Sign)).Z_Pos / 100
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
                End If

            Else
                Z_Error = (FL_b1 * Laser(Linenr, LoopCnt + (CheckOffs_X * Sign)).X_Pos / 100) + FL_b0

                ' METODE 4 FINDER ENDEN AF EN LINJE DER ER [TestLgd] LANG OG HAR EN FEJL DER ER STØRRE END [MaxError]
                If ((Laser(Linenr, LoopCnt).X_Pos / 100) > -13) And Sign = -1 Then
                    Dist = Dist
                End If
                CheckOffs_Z = Abs(Z_Error - Laser(Linenr, LoopCnt + (CheckOffs_X * Sign)).Z_Pos / 100)
                If n > TestLgd And (CheckOffs_Z > MaxError) Then

                    FindLinje.StartPkt.Nr = LoopCnt - (Sign * TestLgd)
                    FindLinje.StartPkt.X = (Laser(Linenr, FindLinje.StartPkt.Nr).X_Pos / 100)
                    FindLinje.StartPkt.Z = (Laser(Linenr, FindLinje.StartPkt.Nr).Z_Pos / 100)

                    FindLinje.SlutPkt.X = (Laser(Linenr, LoopCnt - Sign).X_Pos / 100)         '+ (X_Error * Sign)
                    FindLinje.SlutPkt.Z = (FL_b1 * Laser(Linenr, (LoopCnt - Sign)).X_Pos / 100) + FL_b0
                    'FindLinje1.SlutPkt.Z = (Laser(Linenr, LoopCnt - Sign).Z_Pos / 100)      
                    FindLinje.SlutPkt.Nr = LoopCnt - Sign
                    FindLinje.a = FL_b1
                    FindLinje.b = FL_b0
                    FindLinje.Angle = Atan(FL_b1) * 180 / Math.PI

                    Return FindLinje
                End If

            End If

            LoopCnt = LoopCnt + Sign
            If LoopCnt = Slut Then
                LoopEnd = True
            End If

        End While

        If Line_Is_Straight = False Then
            FindLinje.StartPkt.Nr = Start1
            FindLinje.StartPkt.X = (Laser(Linenr, Start1).X_Pos / 100)
            FindLinje.StartPkt.Z = (Laser(Linenr, Start1).Z_Pos / 100)
        Else
            FindLinje.StartPkt.Nr = Start
            FindLinje.StartPkt.X = (Laser(Linenr, Start).X_Pos / 100)
            FindLinje.StartPkt.Z = (Laser(Linenr, Start).Z_Pos / 100)
        End If

        FindLinje.SlutPkt.X = (Laser(Linenr, LoopCnt).X_Pos / 100)
        FindLinje.SlutPkt.Z = (Laser(Linenr, LoopCnt).Z_Pos / 100)       ' b1 * (LaserArr( LoopCnt).X_Pos / 100) + b0
        FindLinje.SlutPkt.Nr = LoopCnt
        FindLinje.a = b1
        FindLinje.b = b0
        FindLinje.Angle = Atan(b1) * 180 / Math.PI
        'ScanViewer.MakeLine(FindLinje1, Color.Green)

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
            X_Act = Laser(1, LoopCnt).X_Pos / 100
            Z_Act = Laser(1, LoopCnt).Z_Pos / 100

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
        'Form1.MakeLine(FindEnd, Color.Blue)
        FindEnd.a = ta
        FindEnd.b = tb
        Dist = (b1 * SelLine.StartPkt.X + b0 - SelLine.StartPkt.Z) / Math.Sqrt(b1 ^ 2 + 1)
    End Function

End Module
