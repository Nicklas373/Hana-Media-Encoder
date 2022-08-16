Imports System.IO
Imports System.Net
Imports Newtonsoft.Json.Linq
Imports Syncfusion.Windows.Forms
Imports Syncfusion.WinForms.Controls
Public Class OptionsMenu
    Inherits SfForm
    Dim openfolderDialog As New FolderBrowserDialog
    Dim configState As Boolean
    Dim wc As New WebClient()
    Private Sub Options_load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        Style.TitleBar.TextHorizontalAlignment = HorizontalAlignment.Center
        Style.TitleBar.TextVerticalAlignment = VisualStyles.VerticalAlignment.Center
        MessageBoxAdv.MessageBoxStyle = MessageBoxAdv.Style.Metro
        General_pnl.Visible = True
        about_pnl.Visible = False
        GetGPUInformation()
        GetBackPref()
        Label4.Text = My.Application.Info.Title
        Label20.Text = My.Application.Info.Version.ToString
        Label25.Text = My.Application.Info.Copyright
        Label26.Text = My.Application.Info.Description
    End Sub
    Private Sub General_Btn(sender As Object, e As EventArgs) Handles Button1.Click
        General_pnl.Visible = True
        about_pnl.Visible = False
    End Sub
    Private Sub About_Btn(sender As Object, e As EventArgs) Handles Button3.Click
        about_pnl.Visible = True
    End Sub
    Private Sub Browse_Btn_FFMPEG(sender As Object, e As EventArgs) Handles Button4.Click
        openfolderDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
        If openfolderDialog.ShowDialog() = DialogResult.OK Then
            If File.Exists(openfolderDialog.SelectedPath & "\ffmpeg.exe") And File.Exists(openfolderDialog.SelectedPath & "\ffplay.exe") And File.Exists(openfolderDialog.SelectedPath & "\ffprobe.exe") Then
                TextBox1.Text = openfolderDialog.SelectedPath
                If File.Exists("config.ini") Then
                    Dim FFMPEGConf As String = FindConfig("config.ini", "FFMPEG Binary:")
                    If FFMPEGConf = "null" Then
                        Dim writer As New StreamWriter("config.ini", True)
                        writer.WriteLine("FFMPEG Binary:" & TextBox1.Text)
                        writer.Close()
                    Else
                        Dim FFMPEGReaderOldConf As String = File.ReadAllText("config.ini")
                        FFMPEGReaderOldConf = FFMPEGReaderOldConf.Replace(FFMPEGConf, "FFMPEG Binary:" & TextBox1.Text)
                        File.WriteAllText("config.ini", FFMPEGReaderOldConf)
                    End If
                Else
                    File.Create("config.ini").Dispose()
                    Dim writer As New StreamWriter("config.ini", True)
                    writer.WriteLine("FFMPEG Binary:" & TextBox1.Text)
                    writer.Close()
                End If
                configState = True
            Else
                MessageBoxAdv.Show("Make sure that folder have required FFMPEG Files !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub
    Private Sub GPUOverrideCheck(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
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
        If configState = True Then
            MessageBoxAdv.Show("Application need restart after change some options !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Hide()
            Me.Dispose()
            Application.Restart()
        Else
            MainMenu.Show()
        End If
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
            Dim debugMode As String = FindConfig("config.ini", "Debug Mode:")
            Dim frameCount As String = FindConfig("config.ini", "Frame Count:")
            Dim ffmpegConfig As String = FindConfig("config.ini", "FFMPEG Binary:")
            Dim hwdefConfig As String = FindConfig("config.ini", "GPU Engine:")
            If debugMode = "null" Then
                CheckBox3.Checked = False
            Else
                Dim newDebugState As String = debugMode.Remove(0, 11)
                CheckBox3.Checked = newDebugState
            End If
            If ffmpegConfig = "null" Then
                TextBox1.Text = ""
            Else
                TextBox1.Text = ffmpegConfig.Remove(0, 14)
            End If
            If frameCount = "null" Then
                CheckBox4.Checked = False
            Else
                Dim newFrameState As String = frameCount.Remove(0, 11)
                CheckBox4.Checked = newFrameState
            End If
            If hwdefConfig = "GPU Engine:cuda" Or hwdefConfig = "GPU Engine:opencl" Or hwdefConfig = "GPU Engine:qsv" Then
                CheckBox1.Checked = True
                If hwdefConfig.Remove(0, 11) = "qsv" Then
                    ComboBox1.SelectedText = "Intel (QuickSync)"
                ElseIf hwdefConfig.Remove(0, 11) = "opencl" Then
                    ComboBox1.SelectedText = "AMD (OpenCL)"
                ElseIf hwdefConfig.Remove(0, 11) = "cuda" Then
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
            configState = False
        End If
    End Sub
    Private Sub GPUHWEnable(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Dim HWDecConf As String = FindConfig("config.ini", "GPU Engine:")
        If CheckBox1.Checked Then
            If File.Exists("config.ini") Then
                If HWDecConf = "null" Then
                    If GetGraphicsHWEngine(ComboBox1.Text) = "null" Then
                    Else
                        Dim writer As New StreamWriter("config.ini", True)
                        writer.WriteLine("GPU Engine:" & GetGraphicsHWEngine(ComboBox1.Text))
                        writer.Close()
                    End If
                Else
                    If GetGraphicsHWEngine(ComboBox1.Text) = "null" Then
                    Else
                        Dim HWDecOldConf As String = File.ReadAllText("config.ini")
                        HWDecOldConf = HWDecOldConf.Replace(HWDecConf, "GPU Engine:" & GetGraphicsHWEngine(ComboBox1.Text))
                        File.WriteAllText("config.ini", HWDecOldConf)
                    End If
                End If
            Else
                If GetGraphicsHWEngine(ComboBox1.Text) = "null" Then
                Else
                    File.Create("config.ini").Dispose()
                    Dim writer As New StreamWriter("config.ini", True)
                    writer.WriteLine("GPU Engine:" & GetGraphicsHWEngine(ComboBox1.Text))
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
                HWDecOldConf = HWDecOldConf.Replace(HWDecConf, "GPU Engine:")
                File.WriteAllText("config.ini", HWDecOldConf)
            Else
                File.Create("config.ini").Dispose()
                Dim writer As New StreamWriter("config.ini", True)
                writer.WriteLine("GPU Engine:")
                writer.Close()
            End If
        End If
        configState = True
    End Sub
    Private Sub DebugModeCheck(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        Dim debugMode As String = FindConfig("config.ini", "Debug Mode:")
        If CheckBox3.Checked Then
            CheckBox4.Enabled = True
        Else
            CheckBox4.Enabled = False
        End If
        If File.Exists("config.ini") Then
            If debugMode = "null" Then
                Dim writer As New StreamWriter("config.ini", True)
                writer.WriteLine("Debug Mode:" & CheckBox3.Checked)
                writer.Close()
            Else
                Dim debugModeOldConf As String = File.ReadAllText("config.ini")
                debugModeOldConf = debugModeOldConf.Replace(debugMode, "Debug Mode:" & CheckBox3.Checked)
                File.WriteAllText("config.ini", debugModeOldConf)
            End If
        Else
            File.Create("config.ini").Dispose()
            Dim writer As New StreamWriter("config.ini", True)
            writer.WriteLine("Debug Mode:" & CheckBox3.Checked)
            writer.Close()
        End If
        configState = True
    End Sub
    Private Sub FrameCountCheck(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        Dim frameCount As String = FindConfig("config.ini", "Frame Count:")
        If File.Exists("config.ini") Then
            If frameCount = "null" Then
                Dim writer As New StreamWriter("config.ini", True)
                writer.WriteLine("Frame Count:" & CheckBox4.Checked)
                writer.Close()
            Else
                Dim frameCountOldConf As String = File.ReadAllText("config.ini")
                frameCountOldConf = frameCountOldConf.Replace(frameCount, "Frame Count:" & CheckBox4.Checked)
                File.WriteAllText("config.ini", frameCountOldConf)
            End If
        Else
            File.Create("config.ini").Dispose()
            Dim writer As New StreamWriter("config.ini", True)
            writer.WriteLine("Frame Count:" & CheckBox4.Checked)
            writer.Close()
        End If
        configState = True
    End Sub
    Private Sub WebURL(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Dim psi As ProcessStartInfo = New ProcessStartInfo With {
               .FileName = "https://github.com/Nicklas373/Hana-Media-Encoder",
               .UseShellExecute = True
           }
        Process.Start(psi)
    End Sub
    Private Sub CopyrightURL(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim psi As ProcessStartInfo = New ProcessStartInfo With {
               .FileName = "https://github.com/Nicklas373/Hana-Media-Encoder/issues",
               .UseShellExecute = True
           }
        Process.Start(psi)
    End Sub
    Private Sub OTAMenu(sender As Object, e As EventArgs) Handles Button2.Click
        Dim tryParse As Boolean
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        Try
            Dim OTA As String = wc.DownloadString("https://raw.githubusercontent.com/Nicklas373/Hana-Media-Encoder/master/OTA")
            Dim parsejson As JObject = JObject.Parse(OTA)
            tryParse = True
        Catch ex As Exception
            tryParse = False
        End Try
        If tryParse Then
            Dim OTA As String = wc.DownloadString("https://raw.githubusercontent.com/Nicklas373/Hana-Media-Encoder/master/OTA")
            Dim parsejson As JObject = JObject.Parse(OTA)
            Dim appver = parsejson.SelectToken("version").ToString()
            Dim newParsedVer As String() = appver.Split(".")
            Dim curParsedVer As String() = My.Application.Info.Version.ToString.Split(".")
            Dim mergedNewVer As Integer
            Dim mergedCurVer As Integer
            mergedNewVer = String.Join(newParsedVer(0), newParsedVer(1), newParsedVer(2))
            mergedCurVer = String.Join(curParsedVer(0), curParsedVer(1), curParsedVer(2))
            If mergedNewVer > mergedCurVer Then
                Dim menu_ota = New OTAMenu
                menu_ota.Show()
            Else
                MessageBoxAdv.Show("No updates found !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBoxAdv.Show("Failed to retrieve update status !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
End Class