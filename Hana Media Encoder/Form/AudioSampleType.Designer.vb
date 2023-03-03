<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AudioSampleType
    Inherits Syncfusion.WinForms.Controls.SfForm

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
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(AudioSampleType))
        GroupBox1 = New GroupBox()
        Label3 = New Label()
        Label2 = New Label()
        TrackBar1 = New TrackBar()
        Label1 = New Label()
        ComboBox1 = New ComboBox()
        Label138 = New Label()
        GroupBox2 = New GroupBox()
        ComboBox33 = New ComboBox()
        Label65 = New Label()
        Label111 = New Label()
        ComboBox34 = New ComboBox()
        GroupBox3 = New GroupBox()
        ComboBox18 = New ComboBox()
        Label68 = New Label()
        Button3 = New Button()
        Button2 = New Button()
        GroupBox4 = New GroupBox()
        ComboBox2 = New ComboBox()
        Label4 = New Label()
        GroupBox1.SuspendLayout()
        CType(TrackBar1, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox2.SuspendLayout()
        GroupBox3.SuspendLayout()
        GroupBox4.SuspendLayout()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(Label3)
        GroupBox1.Controls.Add(Label2)
        GroupBox1.Controls.Add(TrackBar1)
        GroupBox1.Controls.Add(Label1)
        GroupBox1.Controls.Add(ComboBox1)
        GroupBox1.Controls.Add(Label138)
        GroupBox1.Font = New Font("Poppins", 12F, FontStyle.Regular, GraphicsUnit.Point)
        GroupBox1.ForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        GroupBox1.Location = New Point(5, 92)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(574, 141)
        GroupBox1.TabIndex = 93
        GroupBox1.TabStop = False
        GroupBox1.Text = "Sample Rate Conversion"' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        Label3.ForeColor = Color.FromArgb(CByte(219), CByte(219), CByte(219))
        Label3.Location = New Point(340, 83)
        Label3.Name = "Label3"
        Label3.Size = New Size(26, 17)
        Label3.TabIndex = 106
        Label3.Text = "0%"' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        Label2.ForeColor = Color.FromArgb(CByte(219), CByte(219), CByte(219))
        Label2.Location = New Point(8, 80)
        Label2.Name = "Label2"
        Label2.Size = New Size(122, 17)
        Label2.TabIndex = 105
        Label2.Text = "Compression Ratio"' 
        ' TrackBar1
        ' 
        TrackBar1.Location = New Point(134, 80)
        TrackBar1.Name = "TrackBar1"
        TrackBar1.Size = New Size(198, 45)
        TrackBar1.TabIndex = 104
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        Label1.ForeColor = Color.FromArgb(CByte(219), CByte(219), CByte(219))
        Label1.Location = New Point(340, 43)
        Label1.Name = "Label1"
        Label1.Size = New Size(24, 17)
        Label1.TabIndex = 103
        Label1.Text = "Hz"' 
        ' ComboBox1
        ' 
        ComboBox1.BackColor = Color.FromArgb(CByte(22), CByte(27), CByte(33))
        ComboBox1.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        ComboBox1.ForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        ComboBox1.FormattingEnabled = True
        ComboBox1.Items.AddRange(New Object() {"8000", "16000", "32000", "44100", "48000", "64000", "88200", "96000", "176400", "192000"})
        ComboBox1.Location = New Point(134, 40)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(198, 25)
        ComboBox1.TabIndex = 102
        ' 
        ' Label138
        ' 
        Label138.AutoSize = True
        Label138.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        Label138.ForeColor = Color.FromArgb(CByte(219), CByte(219), CByte(219))
        Label138.Location = New Point(8, 40)
        Label138.Name = "Label138"
        Label138.Size = New Size(83, 17)
        Label138.TabIndex = 101
        Label138.Text = "Sample Rate"' 
        ' GroupBox2
        ' 
        GroupBox2.Controls.Add(ComboBox33)
        GroupBox2.Controls.Add(Label65)
        GroupBox2.Controls.Add(Label111)
        GroupBox2.Controls.Add(ComboBox34)
        GroupBox2.Font = New Font("Poppins", 12F, FontStyle.Regular, GraphicsUnit.Point)
        GroupBox2.ForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        GroupBox2.Location = New Point(5, 326)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(574, 141)
        GroupBox2.TabIndex = 94
        GroupBox2.TabStop = False
        GroupBox2.Text = "Channels"' 
        ' ComboBox33
        ' 
        ComboBox33.BackColor = Color.FromArgb(CByte(22), CByte(27), CByte(33))
        ComboBox33.Font = New Font("Segoe UI Semilight", 9F, FontStyle.Regular, GraphicsUnit.Point)
        ComboBox33.ForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        ComboBox33.FormattingEnabled = True
        ComboBox33.Items.AddRange(New Object() {"1", "2", "3", "4", "5"})
        ComboBox33.Location = New Point(134, 40)
        ComboBox33.Name = "ComboBox33"
        ComboBox33.Size = New Size(198, 23)
        ComboBox33.TabIndex = 97
        ' 
        ' Label65
        ' 
        Label65.AutoSize = True
        Label65.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        Label65.ForeColor = Color.FromArgb(CByte(219), CByte(219), CByte(219))
        Label65.Location = New Point(8, 40)
        Label65.Name = "Label65"
        Label65.Size = New Size(63, 17)
        Label65.TabIndex = 95
        Label65.Text = "Channels"' 
        ' Label111
        ' 
        Label111.AutoSize = True
        Label111.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        Label111.ForeColor = Color.FromArgb(CByte(219), CByte(219), CByte(219))
        Label111.Location = New Point(8, 80)
        Label111.Name = "Label111"
        Label111.Size = New Size(108, 17)
        Label111.TabIndex = 96
        Label111.Text = "Channels Layout"' 
        ' ComboBox34
        ' 
        ComboBox34.BackColor = Color.FromArgb(CByte(22), CByte(27), CByte(33))
        ComboBox34.Font = New Font("Segoe UI Semilight", 9F, FontStyle.Regular, GraphicsUnit.Point)
        ComboBox34.ForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        ComboBox34.FormattingEnabled = True
        ComboBox34.Location = New Point(134, 80)
        ComboBox34.Name = "ComboBox34"
        ComboBox34.Size = New Size(198, 23)
        ComboBox34.TabIndex = 98
        ' 
        ' GroupBox3
        ' 
        GroupBox3.Controls.Add(ComboBox18)
        GroupBox3.Controls.Add(Label68)
        GroupBox3.Font = New Font("Poppins", 12F, FontStyle.Regular, GraphicsUnit.Point)
        GroupBox3.ForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        GroupBox3.Location = New Point(5, 239)
        GroupBox3.Name = "GroupBox3"
        GroupBox3.Size = New Size(574, 81)
        GroupBox3.TabIndex = 95
        GroupBox3.TabStop = False
        GroupBox3.Text = "Bit Depth"' 
        ' ComboBox18
        ' 
        ComboBox18.BackColor = Color.FromArgb(CByte(22), CByte(27), CByte(33))
        ComboBox18.Font = New Font("Segoe UI Semilight", 9F, FontStyle.Regular, GraphicsUnit.Point)
        ComboBox18.ForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        ComboBox18.FormattingEnabled = True
        ComboBox18.Items.AddRange(New Object() {"16 Bit", "24 Bit", "32 Bit"})
        ComboBox18.Location = New Point(134, 40)
        ComboBox18.Name = "ComboBox18"
        ComboBox18.Size = New Size(198, 23)
        ComboBox18.TabIndex = 97
        ' 
        ' Label68
        ' 
        Label68.AutoSize = True
        Label68.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        Label68.ForeColor = Color.FromArgb(CByte(219), CByte(219), CByte(219))
        Label68.Location = New Point(8, 40)
        Label68.Name = "Label68"
        Label68.Size = New Size(65, 17)
        Label68.TabIndex = 96
        Label68.Text = "Bit Depth"' 
        ' Button3
        ' 
        Button3.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Button3.BackColor = Color.Transparent
        Button3.FlatStyle = FlatStyle.Flat
        Button3.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        Button3.ForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        Button3.Location = New Point(393, 581)
        Button3.Name = "Button3"
        Button3.Size = New Size(90, 35)
        Button3.TabIndex = 106
        Button3.Text = "Cancel"
        Button3.UseVisualStyleBackColor = False
        ' 
        ' Button2
        ' 
        Button2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Button2.BackColor = Color.Transparent
        Button2.FlatStyle = FlatStyle.Flat
        Button2.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        Button2.ForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        Button2.Location = New Point(489, 581)
        Button2.Name = "Button2"
        Button2.Size = New Size(90, 35)
        Button2.TabIndex = 105
        Button2.Text = "Save"
        Button2.UseVisualStyleBackColor = False
        ' 
        ' GroupBox4
        ' 
        GroupBox4.Controls.Add(ComboBox2)
        GroupBox4.Controls.Add(Label4)
        GroupBox4.Font = New Font("Poppins", 12F, FontStyle.Regular, GraphicsUnit.Point)
        GroupBox4.ForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        GroupBox4.Location = New Point(5, 5)
        GroupBox4.Name = "GroupBox4"
        GroupBox4.Size = New Size(574, 81)
        GroupBox4.TabIndex = 107
        GroupBox4.TabStop = False
        GroupBox4.Text = "Profile"' 
        ' ComboBox2
        ' 
        ComboBox2.BackColor = Color.FromArgb(CByte(22), CByte(27), CByte(33))
        ComboBox2.Font = New Font("Segoe UI Semilight", 9F, FontStyle.Regular, GraphicsUnit.Point)
        ComboBox2.ForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        ComboBox2.FormattingEnabled = True
        ComboBox2.Location = New Point(134, 40)
        ComboBox2.Name = "ComboBox2"
        ComboBox2.Size = New Size(198, 23)
        ComboBox2.TabIndex = 97
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point)
        Label4.ForeColor = Color.FromArgb(CByte(219), CByte(219), CByte(219))
        Label4.Location = New Point(8, 40)
        Label4.Name = "Label4"
        Label4.Size = New Size(46, 17)
        Label4.TabIndex = 96
        Label4.Text = "Profile"' 
        ' AudioSampleType
        ' 
        AllowRoundedCorners = True
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(22), CByte(27), CByte(33))
        ClientSize = New Size(584, 631)
        Controls.Add(GroupBox4)
        Controls.Add(Button3)
        Controls.Add(Button2)
        Controls.Add(GroupBox3)
        Controls.Add(GroupBox2)
        Controls.Add(GroupBox1)
        ForeColor = Color.FromArgb(CByte(22), CByte(27), CByte(33))
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        Name = "AudioSampleType"
        StartPosition = FormStartPosition.CenterScreen
        Style.BackColor = Color.FromArgb(CByte(22), CByte(27), CByte(33))
        Style.ForeColor = Color.FromArgb(CByte(22), CByte(27), CByte(33))
        Style.MdiChild.IconHorizontalAlignment = HorizontalAlignment.Center
        Style.MdiChild.IconVerticalAlignment = VisualStyles.VerticalAlignment.Center
        Style.TitleBar.BackColor = Color.FromArgb(CByte(22), CByte(27), CByte(33))
        Style.TitleBar.BottomBorderColor = Color.FromArgb(CByte(22), CByte(27), CByte(33))
        Style.TitleBar.CloseButtonForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        Style.TitleBar.ForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        Style.TitleBar.MaximizeButtonForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        Style.TitleBar.MaximizeButtonHoverBackColor = Color.FromArgb(CByte(22), CByte(23), CByte(33))
        Style.TitleBar.MaximizeButtonPressedBackColor = Color.FromArgb(CByte(22), CByte(23), CByte(33))
        Style.TitleBar.MaximizeButtonPressedForeColor = Color.FromArgb(CByte(22), CByte(23), CByte(33))
        Style.TitleBar.MinimizeButtonForeColor = Color.FromArgb(CByte(244), CByte(169), CByte(80))
        Text = "Sample Type"
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        CType(TrackBar1, ComponentModel.ISupportInitialize).EndInit()
        GroupBox2.ResumeLayout(False)
        GroupBox2.PerformLayout()
        GroupBox3.ResumeLayout(False)
        GroupBox3.PerformLayout()
        GroupBox4.ResumeLayout(False)
        GroupBox4.PerformLayout()
        ResumeLayout(False)
    End Sub
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label138 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents ComboBox33 As ComboBox
    Friend WithEvents Label65 As Label
    Friend WithEvents Label111 As Label
    Friend WithEvents ComboBox34 As ComboBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents ComboBox18 As ComboBox
    Friend WithEvents Label68 As Label
    Friend WithEvents Button3 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Label4 As Label
End Class
