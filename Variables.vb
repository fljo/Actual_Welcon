Public Module Variables

    Public Structure xyzwprExt
        Public X As Double
        Public Y As Double
        Public Z As Double
        Public w As Double
        Public p As Double
        Public r As Double
        Public E1 As Double
        Public E2 As Double
        Public E3 As Double

    End Structure

    Public Structure StatAnal
        Dim MinVal As Double
        Dim MaxVal As Double
        Dim MidVal As Double
        Dim SumVal As Double
        Dim Center As Double
        Dim Spredning As Double
        Dim Fordeling As Double
        Dim Success As Boolean
    End Structure

    Public Structure Vector
        Public X As Double
        Public Y As Double
        Public Z As Double
    End Structure


    Public Structure ScanAry
        Public Areal As Double
        Public Pkt1 As xyzwprExt
        Public Pkt2 As xyzwprExt
        Public Pkt3 As xyzwprExt
    End Structure

    Public Structure LinesFound
        Public Success As Boolean
        Public Fejl As Single
        Public a As Double
        Public b As Double
        Public Angle As Double
        Public StartPkt As Coord
        Public SlutPkt As Coord
    End Structure

    Public Structure LineFunc
        Public Success As Boolean
        Public LineVal As LinesFound
    End Structure


    Public Structure LaserVal
        Dim res As Byte             '2 Res. Reserved
        Dim PixWidth As UShort     '10 Width Width of the reflection in Pixel
        Dim PixHeight As UShort    '10 Height Maximum intensity of the reflection above the threshold
        Dim Threshold As UShort        '10 Threshold Actual threshold
        Dim X_Pos As Integer        '16 Position Position co-ordinate (X)
        Dim Z_Pos As Integer        '16 Distance Distance co-ordinate (Z)
        Dim Moment_0 As Integer       '32 Moment 0 Integral intensity of the reflection
        Dim Moment_1 As Integer       '32 Moment 1 1st moment    End Structure
        Private _toInt32 As UInteger
    End Structure


    Public Structure Reflection
        Dim MaxVal As Integer
        Dim MinVal As Integer
        Dim MeanVal As Double
    End Structure

    Public Structure Motion_Ctrl        ' Motion_Dir angives med +1 for +X, -1 for -X; +2 for +Y, -2 for -Y; +4 for +Z, -4 for -Z
        Public ErrorNo As Integer       '                        +8 for +w, -8 for -w; +16 for +p, -16 for -p; +32 for +r, -32 for -r  
        Public Motion_Dir As Integer    '                        +64 for +Ext1, -64 for -Ext1
        Public Motion_Dist As Double    ' afstanden der skal køres
        Public Task As Integer
        Public Success As Boolean       ' om resultatet er valid
    End Structure



    Public Structure RobotData
        Public CartPos As xyzwprExt
        Public NumRegReal As Double
        Public NumRegInt As Integer
        Public SOut As Short
        Public SdIn As Short

    End Structure

    Public Structure Coord
        Public Nr As Integer
        Public X As Double
        Public Y As Double
        Public Z As Double
        Public Disc As Double       ' discribitor value
        Public Success As Boolean
    End Structure

    Public Structure FoundPoint
        Public Angle As Double
        Public StartPkt As Integer
        Public EndPkt As Integer
    End Structure

    Public Structure AnaResult
        Public Success As Boolean
        Public ErrorNr As Integer
    End Structure


    Public Structure Angles_found
        Public Angle As Double
        Public Gns_Ang As Integer
        Public EndPkt As Integer
    End Structure

    Public Structure Scan_Result
        Public FugeAng As Double
        Public Hgt_Div As Double
        Public Mid_Div As Double
        Public MidtForskyd As Double
    End Structure

    Public ActLine As Integer = 1          ' nummeret på den aktuelle laserlinie i array'et
    Public ShutterTime As Integer       ' 1 enhed = 10MicroSec -> 100 = 1 mSec

    Public Ana_Ang(1280) As Angles_found
    ' Public LineID(10) As LinesFound      ' oplysninger om de fundne rette linier (vinkel, start, slut, ligning)
    Public Laser(1280) As LaserVal
    Public LineVal(4) As LineFunc  'FoundPoint
    Public chkval As Boolean
    Public ArealCheck(15, 4) As ScanAry    ' array med de beregnede arealer for skanningerne

    Public Intersect_X, Intersect_Z As Double
    'Public image1 As Bitmap
    Public X_Val As String
    Public Z_Val As String
    Public MouseX1, MouseY1 As Integer
    'Public PictHeight As Integer = 670
    'Public PictWidth As Integer = 855
    Public PictHeight As Integer = Form1.PictureBox1.Height  '670
    Public PictWidth As Integer = Form1.PictureBox1.Width    '855
    Public PicScale As Double
    Public Pict_X_Offs, Pict_Y_Offs As Integer
    'Public RefPos As Coord
    Public bmap As New Drawing.Bitmap(Form1.PictureBox1.Width, Form1.PictureBox1.Height)
    'Public bmap As New Drawing.Bitmap(PictWidth, PictHeight)
    Public GFX As Graphics = Graphics.FromImage(bmap)
    Public CrossSect As Double
    Public FilNavn As String
    Public FileCnt As Integer


    Public Const MaxLoop1 As Integer = 1280
    Public LoopCnt As Integer = 50        ' antallet af punkter der indgår i regressionen
    Public Const ClusterAngLimit As Integer = 5
    Public Const MinClusterSize As Integer = 100

    Public MaxLoop As Integer
    Public StatAng(360) As Integer
    Public ClusterStartAng As Integer       ' vinklen hvor clusterklyngen starter
    Public ClusterEndAng As Integer         ' vinklen hvor clusterklyngen slutter
    Public Maaling(1280) As Double          ' vinklen for for linien som pågældende punkt befinder sig på.
    Public ValidMeas As Integer
    Public Areal_Cnt As Integer = 0
    Public Row_Cnt As Integer

    Dim Areal_Gns As Double = 0
    Dim Areal_Sum As Double = 0
    Public testCnt As Integer
    Public testCnt2 As Integer

    Public chk_timer As Boolean
    Public TimerCnt As Integer = 0
    Public ScannerConnected As Boolean
    Public MainLoop_Active As Boolean
    Public ReflectVal As Reflection
    Public Analyze As Boolean = False       ' skal være true for at skanningen kan analyseres (sættes i <Check_Reflection>)
    Public ValidMeasLim As Integer = 1000   ' der skal være minimum så mange gode værdier i skanningen for at den er accepteret

    Public RobotReady As Boolean
    Public ScannerReady As Boolean
    Public RobotIP As String
    Public ScannerIP As UInteger


    Public Searching As Boolean             ' hvis søgeudgangen er sat på robotten
    Public Robot_Moving As Boolean          ' hvis Motion udgangen er sat på robotten
    Public GrooveFound As Boolean           ' midten af fugen er midt i scanningen
    Public SearchSig As Boolean
    Public DigSearch As Boolean

    Public RobotVal(10) As RobotData
    Public RotationStartVal As Double = 0
    Public GroovePoints(3) As Coord

    Public BoolFlag As Boolean
    Public Areal_Beregn As Double
    Public Scan_Distance As Double

    Public HostName As String
    Public Sh_Time As String

    Public RotationsForskel As Double

    Public MinZ As Double               ' mindste Y værdi fra skanningen 
    Public MinZ_Compare As Double       ' Mindste Y-værdi fra skanningen - bruges til at finde ud af om emnet forsvinder

    Public ScanCnt As Integer           ' tæller antallet af scan der er foretaget til sammenligning

    Public MaxAreal As Double
    Public MinAreal As Double
    Public MeanAreal As Double

    Public ArealLimVal As Double
    Public Scan_Accepted As Boolean
    Public FoundLine(6) As LinesFound

    Public Global_X_Offs As Double      ' er værdien på det kalibreringsoffset i X der er fundet ved at bestemme x-forskydningen mellem 2 forskellige y-værdier 
    Public Global_Z_Offs As Double      ' er værdien på det kalibreringsoffset i Z der er fundet ved at bestemme x-forskydningen mellem 2 forskellige y-værdier 
    Public Global_Y_Start As Double     ' er Y-værdien på startpunktet af kalibreringsoffset'et der er brugt ved at bestemme XZ-forskydningen mellem 2 forskellige y-værdier 
    Public Global_Y_Slut As Double      ' er forskellen på Y-værdien 

    Public Global_Heights(15) As Double
    Public Global_XVal_Reg(44) As Double
    Public RealNumReg1(20) As Double

    Public Scan_Draw_Mode As Integer

    Public XOffs_Arr(20, 6) As Double   ' (n, m)  m 1 0g 2 er de aktuelle X-værdier, 3 og 4 er de oprindelige værdier, 5 er forskydning, 6 er Krymp 
    Public FugeForskyd(20) As Double        ' beregningsværdi af forskydning af fugen
    Public LastFugeForskyd As Double    ' gemmer den sidste beregnede fugeforskydning
    Public FugeKrymp(20) As Double          ' beregningsværdi af krympet af fugen
    Public LastFugeKrymp As Double      ' gemmer den sidste beregningsværdi af krympet af fugen
    Public ErrorTracker As Integer

    Public Scan_Valid(15, 5) As Scan_Result
    Public FugeHgtVal As Double
    Public MeasuredArea As Double
    Public MaesArea(3) As Double
    Public FugeBundVinkel As Single
    Public Fejlskan As Integer
End Module
