<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TekstInputBox
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TekstInputBox))
        Me.ShTime = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.OK_Btn = New System.Windows.Forms.Button()
        Me.Cancel_Btn = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ShTime
        '
        resources.ApplyResources(Me.ShTime, "ShTime")
        Me.ShTime.Name = "ShTime"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'OK_Btn
        '
        resources.ApplyResources(Me.OK_Btn, "OK_Btn")
        Me.OK_Btn.Name = "OK_Btn"
        Me.OK_Btn.UseVisualStyleBackColor = True
        '
        'Cancel_Btn
        '
        resources.ApplyResources(Me.Cancel_Btn, "Cancel_Btn")
        Me.Cancel_Btn.Name = "Cancel_Btn"
        Me.Cancel_Btn.UseVisualStyleBackColor = True
        '
        'TekstInputBox
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.ControlBox = False
        Me.Controls.Add(Me.Cancel_Btn)
        Me.Controls.Add(Me.OK_Btn)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ShTime)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TekstInputBox"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ShTime As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents OK_Btn As System.Windows.Forms.Button
    Friend WithEvents Cancel_Btn As System.Windows.Forms.Button
End Class
