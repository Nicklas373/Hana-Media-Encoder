Imports System.IO
Imports Syncfusion.Windows.Forms
Imports Syncfusion.WinForms.Controls
Public Class OptionsMenu
    Inherits SfForm
    Dim openfolderDialog As New FolderBrowserDialog
    Private Sub Options_load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        Style.TitleBar.TextHorizontalAlignment = HorizontalAlignment.Center
        Style.TitleBar.TextVerticalAlignment = VisualStyles.VerticalAlignment.Center
        MessageBoxAdv.MessageBoxStyle = MessageBoxAdv.Style.Metro
        General_pnl.Visible = True
        about_pnl.Visible = False
        GetGPUInformation()
        GetBackPref()
    End Sub
    Private Sub General_Btn(sender As Object, e As EventArgs) Handles Button1.Click
        General_pnl.Visible = True
        about_pnl.Visible = False
    End Sub
    Private Sub About_Btn(sender As Object, e As EventArgs) Handles Button3.Click
        about_pnl.Visible = True
    End Sub
    Private Sub Browse_Btn_FFMPEG(sender As Object, e As EventArgs)
        openfolderDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
        If openfolderDialog.ShowDialog() = DialogResult.OK Then
            If File.Exists(openfolderDialog.SelectedPath & "\ffmpeg.exe") And File.Exists(openfolderDialog.SelectedPath & "\ffplay.exe") And File.Exists(openfolderDialog.SelectedPath & "\ffprobe.exe") Then
                TextBox1.Text = openfolderDialog.SelectedPath
                If File.Exists("config.ini") Then
                    Dim FFMPEGConf As String = FindConfig("config.ini", "FFMPEG Binary: ")
                    If FFMPEGConf = "null" Then
                        Dim writer As New StreamWriter("config.ini", True)
                        writer.WriteLine("FFMPEG Binary: " & TextBox1.Text)
                        writer.Close()
                    Else
                        Dim FFMPEGReaderOldConf As String = File.ReadAllText("config.ini")
                        FFMPEGReaderOldConf = FFMPEGReaderOldConf.Replace(FFMPEGConf, "FFMPEG Binary: " & TextBox1.Text)
                        File.WriteAllText("config.ini", FFMPEGReaderOldConf)
                    End If
                Else
                    File.Create("config.ini").Dispose()
                    Dim writer As New StreamWriter("config.ini", True)
                    writer.WriteLine("FFMPEG Binary: " & TextBox1.Text)
                    writer.Close()
                End If
                MessageBoxAdv.Show("Application need to restart after changes FFMPEG Binary !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
                MainMenu.Show()
                Application.Restart()
            Else
                MessageBoxAdv.Show("Make sure that folder have required FFMPEG Files !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub
    Private Sub Browse_Btn_Python(sender As Object, e As EventArgs) Handles Button2.Click
        openfolderDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
        If openfolderDialog.ShowDialog() = DialogResult.OK Then
            If File.Exists(openfolderDialog.SelectedPath & "\python.exe") Then
                TextBox2.Text = openfolderDialog.SelectedPath
                If File.Exists("config.ini") Then
                    Dim PythonConf As String = FindConfig("config.ini", "Python Binary: ")
                    If PythonConf = "null" Then
                        Dim writer As New StreamWriter("config.ini", True)
                        writer.WriteLine("Python Binary: " & TextBox2.Text)
                        writer.Close()
                    Else
                        Dim PythonReaderOldConf As String = File.ReadAllText("config.ini")
                        PythonReaderOldConf = PythonReaderOldConf.Replace(PythonConf, "Python Binary: " & TextBox2.Text)
                        File.WriteAllText("config.ini", PythonReaderOldConf)
                    End If
                Else
                    File.Create("config.ini").Dispose()
                    Dim writer As New StreamWriter("config.ini", True)
                    writer.WriteLine("Python Binary: " & TextBox2.Text)
                    writer.Close()
                End If
                MessageBoxAdv.Show("Application need to restart after changes Python Binary !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
                MainMenu.Show()
                Application.Restart()
            Else
                MessageBoxAdv.Show("Make sure that folder have required Python Files !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub
    Private Sub GPUOverrideCheck(sender As Object, e As EventArgs)
        If CheckBox2.Checked Then
            ComboBox1.Enabled = True
            CheckBox1.Checked = False
            MessageBoxAdv.Show("Warning: Override GPU Hardware Acceleration will provide other GPU Hardware Acceleration profile are selected !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            MessageBoxAdv.Show("Make sure to choose proper GPU Hardware Acceleration that may exists on your GPU!", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            ComboBox1.Enabled = False
        End If
    End Sub
    Private Sub Options_Close(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        MainMenu.Show()
    End Sub
    Private Sub GetGPUInformation()
        Label7.Text = GetGraphicsCardName("Name")
        Label9.Text = GetGraphicsCardName("Status")
        Label15.Text = GetGraphicsCardName("DriverVersion")
        Label13.Text = GetGraphicsCardName("AdapterDACType")
        If GetGraphicsCardName("AdapterRAM") / 1024 / 1024 / 1024 < 1 Then
            If GetGraphicsCardName("AdapterRAM") / 1024 / 1024 < 1 Then
                Label11.Text = Format(GetGraphicsCardName("AdapterRAM") / 1024, "###.##").ToString & " KB"
            Else
                Label11.Text = Format(GetGraphicsCardName("AdapterRAM") / 1024 / 1024, "###.##").ToString & " MB"
            End If
        Else
            Label11.Text = Format(GetGraphicsCardName("AdapterRAM") / 1024 / 1024 / 1024, "###.##").ToString & " GB"
        End If
    End Sub
    Private Sub GetBackPref()
        If File.Exists("config.ini") Then
            Dim debugMode As String = FindConfig("config.ini", "Debug Mode: ")
            Dim frameCount As String = FindConfig("config.ini", "Frame Count: ")
            Dim ffmpegConfig As String = FindConfig("config.ini", "FFMPEG Binary: ")
            Dim hwdefConfig As String = FindConfig("config.ini", "GPU Engine: ")
            Dim pythonConfig As String = FindConfig("config.ini", "Python Binary: ")
            If debugMode = "null" Then
                CheckBox3.Checked = False
            Else
                Dim newDebugState As String = debugMode.Remove(0, 12)
                CheckBox3.Checked = newDebugState
            End If
            If ffmpegConfig = "null" Then
                TextBox1.Text = ""
            Else
                TextBox1.Text = ffmpegConfig.Remove(0, 15)
            End If
            If pythonConfig = "null" Then
                TextBox2.Text = ""
            Else
                TextBox2.Text = pythonConfig.Remove(0, 15)
            End If
            If hwdefConfig = "GPU Engine: cuda" Or hwdefConfig = "GPU Engine: opencl" Or hwdefConfig = "GPU Engine: qsv" Then
                CheckBox1.Checked = True
                If hwdefConfig.Remove(0, 12) = "qsv" Then
                    ComboBox1.SelectedText = "Intel (QuickSync)"
                ElseIf hwdefConfig.Remove(0, 12) = "opencl" Then
                    ComboBox1.SelectedText = "AMD (OpenCL)"
                ElseIf hwdefConfig.Remove(0, 12) = "cuda" Then
                    ComboBox1.SelectedText = "NVIDIA (NVENC / NVDEC)"
                End If
            Else
                CheckBox1.Checked = False
                If Label7.Text.Contains("Intel(R)") Then
                    ComboBox1.SelectedText = "Intel (QuickSync)"
                ElseIf Label7.Text.Contains("AMD") Then
                    ComboBox1.SelectedText = "AMD (OpenCL)"
                ElseIf Label7.Text.Contains("NVIDIA") Then
                    ComboBox1.SelectedText = "NVIDIA (NVENC / NVDEC)"
                End If
            End If
        End If
    End Sub
    Private Sub GPUHWEnable(sender As Object, e As EventArgs)
        Dim HWDecConf As String = FindConfig("config.ini", "GPU Engine: ")
        If CheckBox1.Checked Then
            If File.Exists("config.ini") Then
                If HWDecConf = "null" Then
                    If GetGraphicsHWEngine(ComboBox1.Text) = "null" Then
                    Else
                        Dim writer As New StreamWriter("config.ini", True)
                        writer.WriteLine("GPU Engine: " & GetGraphicsHWEngine(ComboBox1.Text))
                        writer.Close()
                    End If
                Else
                    If GetGraphicsHWEngine(ComboBox1.Text) = "null" Then
                    Else
                        Dim HWDecOldConf As String = File.ReadAllText("config.ini")
                        HWDecOldConf = HWDecOldConf.Replace(HWDecConf, "GPU Engine: " & GetGraphicsHWEngine(ComboBox1.Text))
                        File.WriteAllText("config.ini", HWDecOldConf)
                    End If
                End If
            Else
                If GetGraphicsHWEngine(ComboBox1.Text) = "null" Then
                Else
                    File.Create("config.ini").Dispose()
                    Dim writer As New StreamWriter("config.ini", True)
                    writer.WriteLine("GPU Engine: " & GetGraphicsHWEngine(ComboBox1.Text))
                    writer.Close()
                End If
            End If
            If CheckBox2.Checked Then
                CheckBox2.Checked = False
                ComboBox1.Enabled = False
            End If
        Else
            If File.Exists("config.ini") Then
                Dim HWDecOldConf As String = File.ReadAllText("config.ini")
                HWDecOldConf = HWDecOldConf.Replace(HWDecConf, "GPU Engine: ")
                File.WriteAllText("config.ini", HWDecOldConf)
            Else
                File.Create("config.ini").Dispose()
                Dim writer As New StreamWriter("config.ini", True)
                writer.WriteLine("GPU Engine: ")
                writer.Close()
            End If
        End If
    End Sub
    Private Sub DebugModeCheck(sender As Object, e As EventArgs)
        Dim debugMode As String = FindConfig("config.ini", "Debug Mode: ")
        If CheckBox3.Checked Then
            CheckBox4.Enabled = True
        Else
            CheckBox4.Enabled = False
        End If
        If File.Exists("config.ini") Then
            If debugMode = "null" Then
                Dim writer As New StreamWriter("config.ini", True)
                writer.WriteLine("Debug Mode: " & CheckBox3.Checked)
                writer.Close()
            Else
                Dim debugModeOldConf As String = File.ReadAllText("config.ini")
                debugModeOldConf = debugModeOldConf.Replace(debugMode, "Debug Mode: " & CheckBox3.Checked)
                File.WriteAllText("config.ini", debugModeOldConf)
            End If
        Else
            File.Create("config.ini").Dispose()
            Dim writer As New StreamWriter("config.ini", True)
            writer.WriteLine("Debug Mode: " & CheckBox3.Checked)
            writer.Close()
        End If
    End Sub
    Private Sub FrameCountCheck(sender As Object, e As EventArgs)
        Dim frameCount As String = FindConfig("config.ini", "Frame Count: ")
        If File.Exists("config.ini") Then
            If frameCount = "null" Then
                Dim writer As New StreamWriter("config.ini", True)
                writer.WriteLine("Frame Count: " & CheckBox4.Checked)
                writer.Close()
            Else
                Dim frameCountOldConf As String = File.ReadAllText("config.ini")
                frameCountOldConf = frameCountOldConf.Replace(frameCount, "Frame Count: " & CheckBox4.Checked)
                File.WriteAllText("config.ini", frameCountOldConf)
            End If
        Else
            File.Create("config.ini").Dispose()
            Dim writer As New StreamWriter("config.ini", True)
            writer.WriteLine("Frame Count: " & CheckBox4.Checked)
            writer.Close()
        End If
    End Sub
End Class