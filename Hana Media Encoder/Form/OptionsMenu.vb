Imports System.IO
Imports System.Net
Imports Newtonsoft.Json.Linq
Imports Syncfusion.Windows.Forms
Imports Syncfusion.WinForms.Controls
Public Class OptionsMenu
    Inherits SfForm
    Private Sub Options_load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        MessageBoxAdv.MessageBoxStyle = MessageBoxAdv.Style.Metro
        General_pnl.Visible = True
        about_pnl.Visible = False
        GetBackPref()
        CheckBox4.Enabled = True
        Label30.Text = My.Application.Info.Title
        Label29.Text = My.Application.Info.Version.ToString
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
        OpenFolderDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
        If OpenFolderDialog.ShowDialog() = DialogResult.OK Then
            If File.Exists(OpenFolderDialog.SelectedPath & "\ffmpeg.exe") And File.Exists(OpenFolderDialog.SelectedPath & "\ffplay.exe") And File.Exists(OpenFolderDialog.SelectedPath & "\ffprobe.exe") Then
                TextBox1.Text = OpenFolderDialog.SelectedPath
                If File.Exists(My.Application.Info.DirectoryPath & "\config.ini") Then
                    FfmpegConf = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "FFMPEG Binary:")
                    If FfmpegConf = "null" Then
                        Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\config.ini", True)
                        writer.WriteLine("FFMPEG Binary:" & TextBox1.Text)
                        writer.Close()
                    Else
                        Dim FFMPEGReaderOldConf As String = File.ReadAllText(My.Application.Info.DirectoryPath & "\config.ini")
                        FFMPEGReaderOldConf = FFMPEGReaderOldConf.Replace(FfmpegConf, "FFMPEG Binary:" & TextBox1.Text)
                        File.WriteAllText(My.Application.Info.DirectoryPath & "\config.ini", FFMPEGReaderOldConf)
                    End If
                Else
                    File.Create(My.Application.Info.DirectoryPath & "\config.ini").Dispose()
                    Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\config.ini", True)
                    writer.WriteLine("FFMPEG Binary:" & TextBox1.Text)
                    writer.Close()
                End If
            Else
                MessageBoxAdv.Show("Make sure that folder have required FFMPEG Files !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub
    Private Sub GPUOverrideCheck(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            ComboBox1.Enabled = True
            CheckBox1.Checked = False
            MessageBoxAdv.Show("Please choose proper GPU Hardware Acceleration that exists on your systems !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            ComboBox1.Enabled = False
        End If
    End Sub
    Private Sub Options_Close(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        MainMenu.Show()
    End Sub
    Private Sub GetBackPref()
        If File.Exists(My.Application.Info.DirectoryPath & "\config.ini") Then
            AddEncConf = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Encode Info:")
            AltEncodeConf = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Alt Encode:")
            DebugMode = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Debug Mode:")
            FrameCount = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Frame Count:")
            FfmpegConfig = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "FFMPEG Binary:")
            Hwdefconfig = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "GPU Engine:")
            If DebugMode = "null" Then
                CheckBox3.Checked = False
            Else
                Newdebugstate = DebugMode.Remove(0, 11)
                CheckBox3.Checked = Newdebugstate
            End If
            If FfmpegConfig = "null" Then
                TextBox1.Text = ""
            Else
                TextBox1.Text = FfmpegConfig.Remove(0, 14)
            End If
            If FrameCount = "null" Then
                CheckBox5.Checked = False
            Else
                Newframestate = FrameCount.Remove(0, 12)
                CheckBox5.Checked = Newframestate
            End If
            If Hwdefconfig = "GPU Engine:cuda" Or Hwdefconfig = "GPU Engine:opencl" Or Hwdefconfig = "GPU Engine:qsv" Then
                CheckBox1.Checked = True
                If Hwdefconfig.Remove(0, 11) = "qsv" Then
                    ComboBox1.SelectedText = "Intel (QuickSync)"
                ElseIf Hwdefconfig.Remove(0, 11) = "opencl" Then
                    ComboBox1.SelectedText = "AMD (OpenCL)"
                ElseIf Hwdefconfig.Remove(0, 11) = "cuda" Then
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
            If AddEncConf = "null" Then
                ComboBox2.Text = ""
            Else
                AddEncTrimConf = AddEncConf.Remove(0, 12)
                ComboBox2.Text = AddEncTrimConf
            End If
            If AltEncodeConf = "null" Then
                CheckBox4.Checked = False
            Else
                AltEncodeTrimConf = AltEncodeConf.Remove(0, 11)
                CheckBox4.Checked = AltEncodeTrimConf
            End If
        End If
    End Sub
    Private Sub GPUHWEnable(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            If File.Exists(My.Application.Info.DirectoryPath & "\config.ini") Then
                Hwdefconfig = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "GPU Engine:")
                If Hwdefconfig = "null" Then
                    If GetGraphicsHWEngine(ComboBox1.Text) = "null" Then
                    Else
                        Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\config.ini", True)
                        writer.WriteLine("GPU Engine:" & GetGraphicsHWEngine(ComboBox1.Text))
                        writer.Close()
                    End If
                Else
                    If GetGraphicsHWEngine(ComboBox1.Text) = "null" Then
                    Else
                        Dim HWDecOldConf As String = File.ReadAllText(My.Application.Info.DirectoryPath & "\config.ini")
                        HWDecOldConf = HWDecOldConf.Replace(Hwdefconfig, "GPU Engine:" & GetGraphicsHWEngine(ComboBox1.Text))
                        File.WriteAllText(My.Application.Info.DirectoryPath & "\config.ini", HWDecOldConf)
                    End If
                End If
            Else
                If GetGraphicsHWEngine(ComboBox1.Text) = "null" Then
                Else
                    File.Create(My.Application.Info.DirectoryPath & "\config.ini").Dispose()
                    Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\config.ini", True)
                    writer.WriteLine("GPU Engine:" & GetGraphicsHWEngine(ComboBox1.Text))
                    writer.Close()
                End If
            End If
            If CheckBox2.Checked Then
                CheckBox2.Checked = False
                ComboBox1.Enabled = False
            End If
        Else
            If File.Exists(My.Application.Info.DirectoryPath & "\config.ini") Then
                Dim HWDecOldConf As String = File.ReadAllText(My.Application.Info.DirectoryPath & "\config.ini")
                HWDecOldConf = HWDecOldConf.Replace(Hwdefconfig, "GPU Engine:")
                File.WriteAllText(My.Application.Info.DirectoryPath & "\config.ini", HWDecOldConf)
            Else
                File.Create(My.Application.Info.DirectoryPath & "\config.ini").Dispose()
                Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\config.ini", True)
                writer.WriteLine("GPU Engine:")
                writer.Close()
            End If
        End If
    End Sub
    Private Sub DebugModeCheck(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If File.Exists(My.Application.Info.DirectoryPath & "\config.ini") Then
            DebugMode = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Debug Mode:")
            If DebugMode = "null" Then
                Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\config.ini", True)
                writer.WriteLine("Debug Mode:" & CheckBox3.Checked)
                writer.Close()
            Else
                Dim debugModeOldConf As String = File.ReadAllText(My.Application.Info.DirectoryPath & "\config.ini")
                debugModeOldConf = debugModeOldConf.Replace(DebugMode, "Debug Mode:" & CheckBox3.Checked)
                File.WriteAllText(My.Application.Info.DirectoryPath & "\config.ini", debugModeOldConf)
            End If
        Else
            File.Create(My.Application.Info.DirectoryPath & "\config.ini").Dispose()
            Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\config.ini", True)
            writer.WriteLine("Debug Mode:" & CheckBox3.Checked)
            writer.Close()
        End If
        If CheckBox3.Checked = True Then
            MainMenu.Text = My.Application.Info.Title.ToString & " (Debug Mode)"
        Else
            MainMenu.Text = My.Application.Info.Title.ToString
        End If
        MainMenu.Refresh()
    End Sub
    Private Sub FrameCountCheck(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        If File.Exists(My.Application.Info.DirectoryPath & "\config.ini") Then
            FrameCount = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Frame Count:")
            If FrameCount = "null" Then
                Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\config.ini", True)
                writer.WriteLine("Frame Count:" & CheckBox5.Checked)
                writer.Close()
            Else
                Dim frameCountOldConf As String = File.ReadAllText(My.Application.Info.DirectoryPath & "\config.ini")
                frameCountOldConf = frameCountOldConf.Replace(FrameCount, "Frame Count:" & CheckBox5.Checked)
                File.WriteAllText(My.Application.Info.DirectoryPath & "\config.ini", frameCountOldConf)
            End If
        Else
            File.Create(My.Application.Info.DirectoryPath & "\config.ini").Dispose()
            Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\config.ini", True)
            writer.WriteLine("Frame Count:" & CheckBox5.Checked)
            writer.Close()
        End If
    End Sub
    Private Sub AlternateEncodeProg(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If File.Exists(My.Application.Info.DirectoryPath & "\config.ini") Then
            AltEncodeStats = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Alt Encode:")
            If AltEncodeStats = "null" Then
                Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\config.ini", True)
                writer.WriteLine("Alt Encode:" & CheckBox4.Checked)
                writer.Close()
            Else
                Dim AltEncodeStatsOldConf As String = File.ReadAllText(My.Application.Info.DirectoryPath & "\config.ini")
                AltEncodeStatsOldConf = AltEncodeStatsOldConf.Replace(AltEncodeStats, "Alt Encode:" & CheckBox4.Checked)
                File.WriteAllText(My.Application.Info.DirectoryPath & "\config.ini", AltEncodeStatsOldConf)
            End If
        Else
            File.Create(My.Application.Info.DirectoryPath & "\config.ini").Dispose()
            Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\config.ini", True)
            writer.WriteLine("Alt Encode:" & CheckBox4.Checked)
            writer.Close()
        End If
    End Sub
    Private Sub AdditionalEncodeInfo(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Dim tempEncodeInfo As String
        If ComboBox2.SelectedIndex = 0 Then
            tempEncodeInfo = "advanced"
        ElseIf ComboBox2.SelectedIndex = 1 Then
            tempEncodeInfo = "percentage"
        Else
            tempEncodeInfo = "none"
        End If
        If File.Exists(My.Application.Info.DirectoryPath & "\config.ini") Then
            AdditionalEncodeStats = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Encode Info:")
            If AdditionalEncodeStats = "null" Then
                Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\config.ini", True)
                writer.WriteLine("Encode Info:" & tempEncodeInfo)
                writer.Close()
            Else
                Dim encodeInfoOldConf As String = File.ReadAllText(My.Application.Info.DirectoryPath & "\config.ini")
                encodeInfoOldConf = encodeInfoOldConf.Replace(AdditionalEncodeStats, "Encode Info:" & tempEncodeInfo)
                File.WriteAllText(My.Application.Info.DirectoryPath & "\config.ini", encodeInfoOldConf)
            End If
        Else
            File.Create(My.Application.Info.DirectoryPath & "\config.ini").Dispose()
            Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\config.ini", True)
            writer.WriteLine("Encode Info:" & tempEncodeInfo)
            writer.Close()
        End If
    End Sub
    Private Sub FFMPEGURL(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        Dim psi As New ProcessStartInfo With {
               .FileName = "https://ffmpeg.org/download.html#build-windows",
               .UseShellExecute = True
           }
        Process.Start(psi)
    End Sub
    Private Sub WebURL(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Dim psi As New ProcessStartInfo With {
               .FileName = "https://github.com/Nicklas373/Hana-Media-Encoder",
               .UseShellExecute = True
           }
        Process.Start(psi)
    End Sub
    Private Sub CopyrightURL(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim psi As New ProcessStartInfo With {
               .FileName = "https://github.com/Nicklas373/Hana-Media-Encoder/issues",
               .UseShellExecute = True
           }
        Process.Start(psi)
    End Sub
    Private Sub OTAMenu(sender As Object, e As EventArgs) Handles Button2.Click
        Dim tryParse As Boolean
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        Try
            OTA = WC.DownloadString("https://raw.githubusercontent.com/Nicklas373/Hana-Media-Encoder/master/OTA")
            Parsejson = JObject.Parse(OTA)
            tryParse = True
        Catch ex As Exception
            tryParse = False
        End Try
        If tryParse Then
            OTA = WC.DownloadString("https://raw.githubusercontent.com/Nicklas373/Hana-Media-Encoder/master/OTA")
            Parsejson = JObject.Parse(OTA)
            AppVer = Parsejson.SelectToken("version").ToString()
            NewParsedVer = AppVer.Split(".")
            CurParsedVer = My.Application.Info.Version.ToString.Split(".")
            MergedNewVer = String.Join(NewParsedVer(0), NewParsedVer(1), NewParsedVer(2))
            MergedCurVer = String.Join(CurParsedVer(0), CurParsedVer(1), CurParsedVer(2))
            If MergedNewVer > MergedCurVer Then
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