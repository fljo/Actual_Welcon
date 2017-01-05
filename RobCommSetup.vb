Public Class RobComm1

    Public Sub RobComm1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.IPAddress.Text = RobotIP
    End Sub

    Private Sub OK1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK1.Click
        RobotIP = Me.IPAddress.Text
        Me.Close()
    End Sub

    Private Sub Cancel1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel1.Click
        Me.Close()
    End Sub
End Class