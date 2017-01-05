<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RobComm1
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.IPAddress = New System.Windows.Forms.TextBox()
        Me.Cancel1 = New System.Windows.Forms.Button()
        Me.OK1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Cambria", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(136, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Robot IP address:"
        '
        'IPAddress
        '
        Me.IPAddress.Font = New System.Drawing.Font("Cambria", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IPAddress.Location = New System.Drawing.Point(168, 13)
        Me.IPAddress.Name = "IPAddress"
        Me.IPAddress.Size = New System.Drawing.Size(155, 27)
        Me.IPAddress.TabIndex = 1
        '
        'Cancel1
        '
        Me.Cancel1.Font = New System.Drawing.Font("Cambria", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cancel1.Location = New System.Drawing.Point(227, 72)
        Me.Cancel1.Name = "Cancel1"
        Me.Cancel1.Size = New System.Drawing.Size(96, 35)
        Me.Cancel1.TabIndex = 2
        Me.Cancel1.Text = "Cancel"
        Me.Cancel1.UseVisualStyleBackColor = True
        '
        'OK1
        '
        Me.OK1.Font = New System.Drawing.Font("Cambria", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OK1.Location = New System.Drawing.Point(168, 72)
        Me.OK1.Name = "OK1"
        Me.OK1.Size = New System.Drawing.Size(53, 35)
        Me.OK1.TabIndex = 3
        Me.OK1.Text = "OK"
        Me.OK1.UseVisualStyleBackColor = True
        '
        'RobComm1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(351, 119)
        Me.Controls.Add(Me.OK1)
        Me.Controls.Add(Me.Cancel1)
        Me.Controls.Add(Me.IPAddress)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RobComm1"
        Me.Text = "Robot Communication Setup"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents IPAddress As System.Windows.Forms.TextBox
    Friend WithEvents Cancel1 As System.Windows.Forms.Button
    Friend WithEvents OK1 As System.Windows.Forms.Button
End Class
