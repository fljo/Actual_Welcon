Module Delay_Func

    Sub delay(ByVal dblSecs As Double)
        Const One_mSec As Double = 1.0# / (1440.0# * 60.0# / 1000.0#)
        Dim dblwaitTil As Date
        Now.AddMilliseconds(One_mSec)
        dblwaitTil = Now.AddMilliseconds(One_mSec).AddMilliseconds(dblSecs)
        Do Until Now > dblwaitTil
            Application.DoEvents()
        Loop

    End Sub

End Module
