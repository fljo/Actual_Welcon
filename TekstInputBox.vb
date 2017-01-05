Public Class TekstInputBox


 


    Private Sub TekstInputBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label1.Text = "Indtast ShutterTime [ms]: "
        ShTime.Text = Str(ShutterTime / 100)

    End Sub

    Private Sub Cancel_Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Btn.Click
        Me.Close()
    End Sub

    Private Sub OK_Btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Btn.Click
        ShutterTime = Val(ShTime.Text) * 100
        Init_Steps(10)
        Me.Close()

    End Sub


End Class