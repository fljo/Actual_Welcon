<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Areal = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CommTimer = New System.Windows.Forms.Timer(Me.components)
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SettingsMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ThresholdSelect = New System.Windows.Forms.ToolStripMenuItem()
        Me.Thres1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Thres2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Thres3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Thres4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Thres5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Thres6 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Thres7 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShutterTimeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RobotComm = New System.Windows.Forms.ToolStripMenuItem()
        Me.Settings = New System.Windows.Forms.MenuStrip()
        Me.FileMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.Make_TxtFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetTextfileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ScanMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.InitScan = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetScan = New System.Windows.Forms.ToolStripMenuItem()
        Me.ScannerONToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ScannerOFFToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Center_Btn = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.checkbox1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.checkbox2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Analyze_sel = New System.Windows.Forms.ToolStripMenuItem()
        Me.CenterView = New System.Windows.Forms.ToolStripMenuItem()
        Me.Thresh_Lbl = New System.Windows.Forms.Label()
        Me.GoodValues = New System.Windows.Forms.Label()
        Me.lbl_X_Pos = New System.Windows.Forms.Label()
        Me.lbl_Z_Pos = New System.Windows.Forms.Label()
        Me.BindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Start1 = New System.Windows.Forms.Button()
        Me.Afslut = New System.Windows.Forms.Button()
        Me.Lbl_Y_Pos = New System.Windows.Forms.Label()
        Me.Bundvinkel = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Lbl_RobOn = New System.Windows.Forms.Label()
        Me.Lbl_ScanOn = New System.Windows.Forms.Label()
        Me.Lbl_OmronOn = New System.Windows.Forms.Label()
        Me.Error_Lbl = New System.Windows.Forms.Label()
        Me.ErrLabel2 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.StatusBox1 = New System.Windows.Forms.GroupBox()
        Me.Status_Lbl2 = New System.Windows.Forms.Label()
        Me.Status_Lbl1 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.FugeAng = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.WdtError = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.HgtError = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.FugeWdt = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.FugeHgt = New System.Windows.Forms.TextBox()
        Me.ScannerStatus = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Settings.SuspendLayout()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.LightGray
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.PictureBox1.Location = New System.Drawing.Point(14, 49)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(764, 599)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Areal
        '
        Me.Areal.Enabled = False
        Me.Areal.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Areal.Location = New System.Drawing.Point(916, 113)
        Me.Areal.Name = "Areal"
        Me.Areal.Size = New System.Drawing.Size(68, 26)
        Me.Areal.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label1.Location = New System.Drawing.Point(784, 117)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 20)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Measured Area "
        '
        'CommTimer
        '
        Me.CommTimer.Interval = 10
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(311, 627)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 13)
        Me.Label3.TabIndex = 34
        '
        'SettingsMenu
        '
        Me.SettingsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ThresholdSelect, Me.ShutterTimeToolStripMenuItem, Me.RobotComm})
        Me.SettingsMenu.Name = "SettingsMenu"
        Me.SettingsMenu.Size = New System.Drawing.Size(61, 20)
        Me.SettingsMenu.Text = "Settings"
        '
        'ThresholdSelect
        '
        Me.ThresholdSelect.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Thres1, Me.Thres2, Me.Thres3, Me.Thres4, Me.Thres5, Me.Thres6, Me.ToolStripMenuItem4, Me.ToolStripMenuItem3, Me.ToolStripMenuItem1, Me.Thres7})
        Me.ThresholdSelect.Name = "ThresholdSelect"
        Me.ThresholdSelect.Size = New System.Drawing.Size(155, 22)
        Me.ThresholdSelect.Text = "Scan Threshold"
        '
        'Thres1
        '
        Me.Thres1.Name = "Thres1"
        Me.Thres1.Size = New System.Drawing.Size(100, 22)
        Me.Thres1.Text = "16"
        '
        'Thres2
        '
        Me.Thres2.Name = "Thres2"
        Me.Thres2.Size = New System.Drawing.Size(100, 22)
        Me.Thres2.Text = "32"
        '
        'Thres3
        '
        Me.Thres3.Name = "Thres3"
        Me.Thres3.Size = New System.Drawing.Size(100, 22)
        Me.Thres3.Text = "48"
        '
        'Thres4
        '
        Me.Thres4.Name = "Thres4"
        Me.Thres4.Size = New System.Drawing.Size(100, 22)
        Me.Thres4.Text = "64"
        '
        'Thres5
        '
        Me.Thres5.Name = "Thres5"
        Me.Thres5.Size = New System.Drawing.Size(100, 22)
        Me.Thres5.Text = "96"
        '
        'Thres6
        '
        Me.Thres6.Name = "Thres6"
        Me.Thres6.Size = New System.Drawing.Size(100, 22)
        Me.Thres6.Text = "128"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(100, 22)
        Me.ToolStripMenuItem4.Text = "256"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(100, 22)
        Me.ToolStripMenuItem3.Text = "512"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(100, 22)
        Me.ToolStripMenuItem1.Text = "1024"
        '
        'Thres7
        '
        Me.Thres7.Name = "Thres7"
        Me.Thres7.Size = New System.Drawing.Size(100, 22)
        Me.Thres7.Text = "Auto"
        '
        'ShutterTimeToolStripMenuItem
        '
        Me.ShutterTimeToolStripMenuItem.Name = "ShutterTimeToolStripMenuItem"
        Me.ShutterTimeToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.ShutterTimeToolStripMenuItem.Text = "Shutter time"
        '
        'RobotComm
        '
        Me.RobotComm.Name = "RobotComm"
        Me.RobotComm.Size = New System.Drawing.Size(155, 22)
        Me.RobotComm.Text = "Robot Comm"
        '
        'Settings
        '
        Me.Settings.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileMenu, Me.SettingsMenu, Me.ScanMenu, Me.Center_Btn})
        Me.Settings.Location = New System.Drawing.Point(0, 0)
        Me.Settings.Name = "Settings"
        Me.Settings.Padding = New System.Windows.Forms.Padding(4, 2, 0, 2)
        Me.Settings.Size = New System.Drawing.Size(1025, 24)
        Me.Settings.TabIndex = 44
        Me.Settings.Text = "MenuStrip2"
        '
        'FileMenu
        '
        Me.FileMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Make_TxtFile, Me.ExitToolStripMenuItem, Me.GetTextfileToolStripMenuItem})
        Me.FileMenu.Name = "FileMenu"
        Me.FileMenu.Size = New System.Drawing.Size(42, 20)
        Me.FileMenu.Text = "Files"
        '
        'Make_TxtFile
        '
        Me.Make_TxtFile.Name = "Make_TxtFile"
        Me.Make_TxtFile.Size = New System.Drawing.Size(148, 22)
        Me.Make_TxtFile.Text = "Make Text File"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'GetTextfileToolStripMenuItem
        '
        Me.GetTextfileToolStripMenuItem.Name = "GetTextfileToolStripMenuItem"
        Me.GetTextfileToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.GetTextfileToolStripMenuItem.Text = "Get Textfile"
        '
        'ScanMenu
        '
        Me.ScanMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InitScan, Me.GetScan, Me.ScannerONToolStripMenuItem, Me.ScannerOFFToolStripMenuItem})
        Me.ScanMenu.Name = "ScanMenu"
        Me.ScanMenu.Size = New System.Drawing.Size(61, 20)
        Me.ScanMenu.Text = "Scanner"
        '
        'InitScan
        '
        Me.InitScan.Name = "InitScan"
        Me.InitScan.Size = New System.Drawing.Size(162, 22)
        Me.InitScan.Text = "Initialize Scanner"
        '
        'GetScan
        '
        Me.GetScan.Name = "GetScan"
        Me.GetScan.Size = New System.Drawing.Size(162, 22)
        Me.GetScan.Text = "Get Scan"
        '
        'ScannerONToolStripMenuItem
        '
        Me.ScannerONToolStripMenuItem.Name = "ScannerONToolStripMenuItem"
        Me.ScannerONToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.ScannerONToolStripMenuItem.Text = "Scanner ""ON"""
        '
        'ScannerOFFToolStripMenuItem
        '
        Me.ScannerOFFToolStripMenuItem.Name = "ScannerOFFToolStripMenuItem"
        Me.ScannerOFFToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.ScannerOFFToolStripMenuItem.Text = "Scanner ""OFF"""
        '
        'Center_Btn
        '
        Me.Center_Btn.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem2, Me.Analyze_sel, Me.CenterView})
        Me.Center_Btn.Name = "Center_Btn"
        Me.Center_Btn.Size = New System.Drawing.Size(44, 20)
        Me.Center_Btn.Text = "View"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.checkbox1, Me.checkbox2})
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(137, 22)
        Me.ToolStripMenuItem2.Text = "Invert View"
        '
        'checkbox1
        '
        Me.checkbox1.Name = "checkbox1"
        Me.checkbox1.Size = New System.Drawing.Size(171, 22)
        Me.checkbox1.Text = "Invert Horisontally"
        '
        'checkbox2
        '
        Me.checkbox2.Name = "checkbox2"
        Me.checkbox2.Size = New System.Drawing.Size(171, 22)
        Me.checkbox2.Text = "Invert Vertically"
        '
        'Analyze_sel
        '
        Me.Analyze_sel.Name = "Analyze_sel"
        Me.Analyze_sel.Size = New System.Drawing.Size(137, 22)
        Me.Analyze_sel.Text = "Analyze"
        '
        'CenterView
        '
        Me.CenterView.Name = "CenterView"
        Me.CenterView.Size = New System.Drawing.Size(137, 22)
        Me.CenterView.Text = "Center View"
        '
        'Thresh_Lbl
        '
        Me.Thresh_Lbl.AutoSize = True
        Me.Thresh_Lbl.Location = New System.Drawing.Point(28, 66)
        Me.Thresh_Lbl.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Thresh_Lbl.Name = "Thresh_Lbl"
        Me.Thresh_Lbl.Size = New System.Drawing.Size(0, 13)
        Me.Thresh_Lbl.TabIndex = 46
        '
        'GoodValues
        '
        Me.GoodValues.AutoSize = True
        Me.GoodValues.Location = New System.Drawing.Point(11, 652)
        Me.GoodValues.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.GoodValues.Name = "GoodValues"
        Me.GoodValues.Size = New System.Drawing.Size(59, 13)
        Me.GoodValues.TabIndex = 47
        Me.GoodValues.Text = "Maesval = "
        '
        'lbl_X_Pos
        '
        Me.lbl_X_Pos.AutoSize = True
        Me.lbl_X_Pos.Location = New System.Drawing.Point(112, 32)
        Me.lbl_X_Pos.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lbl_X_Pos.Name = "lbl_X_Pos"
        Me.lbl_X_Pos.Size = New System.Drawing.Size(23, 13)
        Me.lbl_X_Pos.TabIndex = 48
        Me.lbl_X_Pos.Text = "X ="
        '
        'lbl_Z_Pos
        '
        Me.lbl_Z_Pos.AutoSize = True
        Me.lbl_Z_Pos.Location = New System.Drawing.Point(256, 32)
        Me.lbl_Z_Pos.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lbl_Z_Pos.Name = "lbl_Z_Pos"
        Me.lbl_Z_Pos.Size = New System.Drawing.Size(23, 13)
        Me.lbl_Z_Pos.TabIndex = 50
        Me.lbl_Z_Pos.Text = "Z ="
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(915, 81)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(69, 26)
        Me.TextBox2.TabIndex = 57
        '
        'TextBox3
        '
        Me.TextBox3.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(1000, 2)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.TextBox3.Size = New System.Drawing.Size(112, 19)
        Me.TextBox3.TabIndex = 58
        '
        'Start1
        '
        Me.Start1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Start1.Location = New System.Drawing.Point(730, 664)
        Me.Start1.Name = "Start1"
        Me.Start1.Size = New System.Drawing.Size(118, 33)
        Me.Start1.TabIndex = 59
        Me.Start1.Text = "START"
        Me.Start1.UseVisualStyleBackColor = True
        Me.Start1.Visible = False
        '
        'Afslut
        '
        Me.Afslut.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Afslut.Location = New System.Drawing.Point(802, 615)
        Me.Afslut.Name = "Afslut"
        Me.Afslut.Size = New System.Drawing.Size(118, 32)
        Me.Afslut.TabIndex = 60
        Me.Afslut.Text = "EXIT"
        Me.Afslut.UseVisualStyleBackColor = True
        '
        'Lbl_Y_Pos
        '
        Me.Lbl_Y_Pos.AutoSize = True
        Me.Lbl_Y_Pos.Location = New System.Drawing.Point(178, 33)
        Me.Lbl_Y_Pos.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Lbl_Y_Pos.Name = "Lbl_Y_Pos"
        Me.Lbl_Y_Pos.Size = New System.Drawing.Size(23, 13)
        Me.Lbl_Y_Pos.TabIndex = 64
        Me.Lbl_Y_Pos.Text = "Y ="
        '
        'Bundvinkel
        '
        Me.Bundvinkel.Enabled = False
        Me.Bundvinkel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Bundvinkel.Location = New System.Drawing.Point(915, 164)
        Me.Bundvinkel.Name = "Bundvinkel"
        Me.Bundvinkel.Size = New System.Drawing.Size(68, 26)
        Me.Bundvinkel.TabIndex = 66
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label4.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label4.Location = New System.Drawing.Point(784, 167)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(94, 20)
        Me.Label4.TabIndex = 67
        Me.Label4.Text = "Bund Vinkel"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label5.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label5.Location = New System.Drawing.Point(784, 87)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(125, 20)
        Me.Label5.TabIndex = 68
        Me.Label5.Text = "Measured Angle"
        '
        'Lbl_RobOn
        '
        Me.Lbl_RobOn.AutoSize = True
        Me.Lbl_RobOn.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lbl_RobOn.Location = New System.Drawing.Point(341, 28)
        Me.Lbl_RobOn.Name = "Lbl_RobOn"
        Me.Lbl_RobOn.Size = New System.Drawing.Size(100, 19)
        Me.Lbl_RobOn.TabIndex = 70
        Me.Lbl_RobOn.Text = "Robot Online"
        '
        'Lbl_ScanOn
        '
        Me.Lbl_ScanOn.AutoSize = True
        Me.Lbl_ScanOn.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lbl_ScanOn.Location = New System.Drawing.Point(458, 28)
        Me.Lbl_ScanOn.Name = "Lbl_ScanOn"
        Me.Lbl_ScanOn.Size = New System.Drawing.Size(113, 19)
        Me.Lbl_ScanOn.TabIndex = 71
        Me.Lbl_ScanOn.Text = "Scanner Online"
        '
        'Lbl_OmronOn
        '
        Me.Lbl_OmronOn.AutoSize = True
        Me.Lbl_OmronOn.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lbl_OmronOn.Location = New System.Drawing.Point(577, 28)
        Me.Lbl_OmronOn.Name = "Lbl_OmronOn"
        Me.Lbl_OmronOn.Size = New System.Drawing.Size(106, 19)
        Me.Lbl_OmronOn.TabIndex = 72
        Me.Lbl_OmronOn.Text = "Omron Online"
        '
        'Error_Lbl
        '
        Me.Error_Lbl.AutoSize = True
        Me.Error_Lbl.ForeColor = System.Drawing.Color.Red
        Me.Error_Lbl.Location = New System.Drawing.Point(140, 652)
        Me.Error_Lbl.Name = "Error_Lbl"
        Me.Error_Lbl.Size = New System.Drawing.Size(54, 13)
        Me.Error_Lbl.TabIndex = 75
        Me.Error_Lbl.Text = "Error label"
        '
        'ErrLabel2
        '
        Me.ErrLabel2.AutoSize = True
        Me.ErrLabel2.ForeColor = System.Drawing.Color.Red
        Me.ErrLabel2.Location = New System.Drawing.Point(140, 672)
        Me.ErrLabel2.Name = "ErrLabel2"
        Me.ErrLabel2.Size = New System.Drawing.Size(54, 13)
        Me.ErrLabel2.TabIndex = 76
        Me.ErrLabel2.Text = "Error label"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(895, 655)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(72, 30)
        Me.Button1.TabIndex = 77
        Me.Button1.Text = "get file"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'StatusBox1
        '
        Me.StatusBox1.Controls.Add(Me.Status_Lbl2)
        Me.StatusBox1.Controls.Add(Me.Status_Lbl1)
        Me.StatusBox1.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusBox1.ForeColor = System.Drawing.Color.Blue
        Me.StatusBox1.Location = New System.Drawing.Point(784, 465)
        Me.StatusBox1.Name = "StatusBox1"
        Me.StatusBox1.Size = New System.Drawing.Size(237, 88)
        Me.StatusBox1.TabIndex = 81
        Me.StatusBox1.TabStop = False
        Me.StatusBox1.Text = "System Status"
        '
        'Status_Lbl2
        '
        Me.Status_Lbl2.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Status_Lbl2.Location = New System.Drawing.Point(13, 56)
        Me.Status_Lbl2.Name = "Status_Lbl2"
        Me.Status_Lbl2.Size = New System.Drawing.Size(213, 19)
        Me.Status_Lbl2.TabIndex = 81
        Me.Status_Lbl2.Text = "Status_Lbl2"
        Me.Status_Lbl2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Status_Lbl1
        '
        Me.Status_Lbl1.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Status_Lbl1.Location = New System.Drawing.Point(11, 31)
        Me.Status_Lbl1.Name = "Status_Lbl1"
        Me.Status_Lbl1.Size = New System.Drawing.Size(215, 19)
        Me.Status_Lbl1.TabIndex = 80
        Me.Status_Lbl1.Text = "Status_Lbl1"
        Me.Status_Lbl1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label8.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label8.Location = New System.Drawing.Point(784, 258)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(93, 20)
        Me.Label8.TabIndex = 131
        Me.Label8.Text = "Fuge Vinkel"
        '
        'FugeAng
        '
        Me.FugeAng.Enabled = False
        Me.FugeAng.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.FugeAng.Location = New System.Drawing.Point(915, 254)
        Me.FugeAng.Name = "FugeAng"
        Me.FugeAng.Size = New System.Drawing.Size(68, 26)
        Me.FugeAng.TabIndex = 130
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label7.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label7.Location = New System.Drawing.Point(785, 318)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(123, 20)
        Me.Label7.TabIndex = 129
        Me.Label7.Text = "Center afvigelse"
        '
        'WdtError
        '
        Me.WdtError.Enabled = False
        Me.WdtError.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.WdtError.Location = New System.Drawing.Point(915, 315)
        Me.WdtError.Name = "WdtError"
        Me.WdtError.Size = New System.Drawing.Size(68, 26)
        Me.WdtError.TabIndex = 128
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label6.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label6.Location = New System.Drawing.Point(784, 287)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(117, 20)
        Me.Label6.TabIndex = 127
        Me.Label6.Text = "Højde afvigelse"
        '
        'HgtError
        '
        Me.HgtError.Enabled = False
        Me.HgtError.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.HgtError.Location = New System.Drawing.Point(915, 284)
        Me.HgtError.Name = "HgtError"
        Me.HgtError.Size = New System.Drawing.Size(68, 26)
        Me.HgtError.TabIndex = 126
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label2.Location = New System.Drawing.Point(784, 226)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 20)
        Me.Label2.TabIndex = 125
        Me.Label2.Text = "Fuge bredde"
        '
        'FugeWdt
        '
        Me.FugeWdt.Enabled = False
        Me.FugeWdt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.FugeWdt.Location = New System.Drawing.Point(915, 224)
        Me.FugeWdt.Name = "FugeWdt"
        Me.FugeWdt.Size = New System.Drawing.Size(68, 26)
        Me.FugeWdt.TabIndex = 124
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label9.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label9.Location = New System.Drawing.Point(784, 196)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(89, 20)
        Me.Label9.TabIndex = 123
        Me.Label9.Text = "Fuge højde"
        '
        'FugeHgt
        '
        Me.FugeHgt.Enabled = False
        Me.FugeHgt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.FugeHgt.Location = New System.Drawing.Point(915, 194)
        Me.FugeHgt.Name = "FugeHgt"
        Me.FugeHgt.Size = New System.Drawing.Size(68, 26)
        Me.FugeHgt.TabIndex = 122
        '
        'ScannerStatus
        '
        Me.ScannerStatus.AutoSize = True
        Me.ScannerStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ScannerStatus.Location = New System.Drawing.Point(786, 396)
        Me.ScannerStatus.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.ScannerStatus.Name = "ScannerStatus"
        Me.ScannerStatus.Size = New System.Drawing.Size(66, 20)
        Me.ScannerStatus.TabIndex = 132
        Me.ScannerStatus.Text = "scanner"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1025, 693)
        Me.ControlBox = False
        Me.Controls.Add(Me.ScannerStatus)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.FugeAng)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.WdtError)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.HgtError)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.FugeWdt)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.FugeHgt)
        Me.Controls.Add(Me.StatusBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ErrLabel2)
        Me.Controls.Add(Me.Error_Lbl)
        Me.Controls.Add(Me.Lbl_OmronOn)
        Me.Controls.Add(Me.Lbl_ScanOn)
        Me.Controls.Add(Me.Lbl_RobOn)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Bundvinkel)
        Me.Controls.Add(Me.Lbl_Y_Pos)
        Me.Controls.Add(Me.Afslut)
        Me.Controls.Add(Me.Start1)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.lbl_Z_Pos)
        Me.Controls.Add(Me.lbl_X_Pos)
        Me.Controls.Add(Me.GoodValues)
        Me.Controls.Add(Me.Thresh_Lbl)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Areal)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Settings)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(1506, 983)
        Me.MinimumSize = New System.Drawing.Size(795, 613)
        Me.Name = "Form1"
        Me.Text = "Inrotech 3D Scanning"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Settings.ResumeLayout(False)
        Me.Settings.PerformLayout()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Areal As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BindingSource1 As System.Windows.Forms.BindingSource
    Friend WithEvents CommTimer As System.Windows.Forms.Timer
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents SettingsMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ThresholdSelect As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Thres1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Thres2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Thres3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Thres4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Thres5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Thres6 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Thres7 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Settings As System.Windows.Forms.MenuStrip
    Friend WithEvents Thresh_Lbl As System.Windows.Forms.Label
    Friend WithEvents GoodValues As System.Windows.Forms.Label
    Friend WithEvents lbl_X_Pos As System.Windows.Forms.Label
    Friend WithEvents lbl_Z_Pos As System.Windows.Forms.Label
    Friend WithEvents FileMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Make_TxtFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ScanMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InitScan As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetScan As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RobotComm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Center_Btn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents checkbox1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents checkbox2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Analyze_sel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CenterView As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Start1 As System.Windows.Forms.Button
    Friend WithEvents Afslut As System.Windows.Forms.Button
    Friend WithEvents ShutterTimeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Lbl_Y_Pos As System.Windows.Forms.Label
    Friend WithEvents Bundvinkel As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Lbl_RobOn As System.Windows.Forms.Label
    Friend WithEvents Lbl_ScanOn As System.Windows.Forms.Label
    Friend WithEvents Lbl_OmronOn As System.Windows.Forms.Label
    Friend WithEvents Error_Lbl As System.Windows.Forms.Label
    Friend WithEvents ErrLabel2 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents StatusBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Status_Lbl2 As System.Windows.Forms.Label
    Friend WithEvents Status_Lbl1 As System.Windows.Forms.Label
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents FugeAng As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents WdtError As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents HgtError As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents FugeWdt As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents FugeHgt As System.Windows.Forms.TextBox
    Friend WithEvents ScannerStatus As System.Windows.Forms.Label
    Friend WithEvents GetTextfileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ScannerONToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ScannerOFFToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
