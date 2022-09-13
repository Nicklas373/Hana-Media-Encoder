Imports System.IO
Imports System.Text.RegularExpressions
Imports Syncfusion.Windows.Forms
Imports Syncfusion.WinForms.Controls
Public Class MainMenu
    Inherits SfForm
    Private Sub MainMenu_load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        AllowRoundedCorners = True
        Panel1.AllowDrop = True
        Me.KeyPreview = True
        MessageBoxAdv.MessageBoxStyle = MessageBoxAdv.Style.Metro
        MessageBoxAdv.MetroColorTable.DetailsButtonBackColor = ColorTranslator.FromHtml("#F4A950")
        MessageBoxAdv.MetroColorTable.DetailsButtonForeColor = ColorTranslator.FromHtml("#161B21")
        MessageBoxAdv.MetroColorTable.YesButtonBackColor = ColorTranslator.FromHtml("#F4A950")
        MessageBoxAdv.MetroColorTable.YesButtonForeColor = ColorTranslator.FromHtml("#161B21")
        MessageBoxAdv.MetroColorTable.NoButtonBackColor = ColorTranslator.FromHtml("#F4A950")
        MessageBoxAdv.MetroColorTable.NoButtonForeColor = ColorTranslator.FromHtml("#161B21")
        MessageBoxAdv.MetroColorTable.OKButtonBackColor = ColorTranslator.FromHtml("#F4A950")
        MessageBoxAdv.MetroColorTable.OKButtonForeColor = ColorTranslator.FromHtml("#161B21")
        MessageBoxAdv.MetroColorTable.BackColor = ColorTranslator.FromHtml("#161B21")
        MessageBoxAdv.MetroColorTable.CaptionBarColor = ColorTranslator.FromHtml("#161B21")
        MessageBoxAdv.MetroColorTable.CaptionForeColor = ColorTranslator.FromHtml("#F4A950")
        MessageBoxAdv.MetroColorTable.ForeColor = ColorTranslator.FromHtml("#DBDBDB")
        MessageBoxAdv.MetroColorTable.BorderColor = ColorTranslator.FromHtml("#F4A950")
        MessageBoxAdv.CanResize = True
        MessageBoxAdv.MaximumSize = New Size(520, Screen.PrimaryScreen.WorkingArea.Size.Height)
        Me.TabControl1.Region = New Region(New RectangleF(Me.TabPage1.Left, Me.TabPage1.Left, Me.TabPage1.Width, Me.TabPage1.Height))
        Label69.Visible = True
        Label28.Visible = True
        Label28.Text = "Standby"
        Me.Hide()
        InitScreen.Show()
        InitScreen.Close()
        Me.Show()
        Button7.Visible = False
        Button7.Enabled = False
        Button8.Visible = False
        Button8.Enabled = False
        ComboBox36.SelectedIndex = 0
        Dim res1 As String = getBetween(File.ReadAllText("Init_Res.txt"), "START: ", " |")
        Dim res2 As String = getBetween(File.ReadAllText("Init_Res.txt"), "| ", " :END")
        If res1 = "" Then
            If res2 = "" Then
                DebugMode = FindConfig("config.ini", "Debug Mode:")
                FfmpegConfig = FindConfig("config.ini", "FFMPEG Binary:")
                Hwdefconfig = FindConfig("config.ini", "GPU Engine:")
                FrameConfig = FindConfig("config.ini", "Frame Count:")
                FfmpegConf = FfmpegConfig.Remove(0, 14) & "\"
                FfmpegLetter = FfmpegConf.Substring(0, 1) & ":"
                Button1.Enabled = True
                Button3.Enabled = True
                Button4.Enabled = True
                Dim ImgPrev1 As New FileStream(VideoPlaceholder, FileMode.Open, FileAccess.Read)
                PictureBox1.Image = Image.FromStream(ImgPrev1)
                ImgPrev1.Close()
                If DebugMode IsNot "null" Then
                    If DebugMode.Remove(0, 11) = "True" Then
                        Text = My.Application.Info.Title.ToString & " (Debug Mode)"
                    End If
                End If
                Tooltip(Label33, "Configure video codec / encoder that will use for encoding video")
                Tooltip(Label58, "Configure maximum video frame rate / second")
                Tooltip(Label34, "Configure the output conversion for pixel format")
                Tooltip(Label49, "Configure level of support within a profile specifies the maximum picture resolution, frame rate, and bit rate that a decoder may use")
                Tooltip(Label50, "Configure the encoding tier")
                Tooltip(Label46, "Configure the encoding preset (slow mean high, fast mean low) in term of compression")
                Tooltip(Label47, "Configure settings based upon the specifics of your input")
                Tooltip(Label48, "Configure the encoding profile")
                Tooltip(Label38, "Configure the encoding bit rate mode")
                Tooltip(Label43, "Configure the constant rate factor for encoder")
                Tooltip(Label57, "Configure multipass encoding for encoder")
                Tooltip(Label51, "Configure value for overall video bitrate in MB/s")
                Tooltip(Label52, "Configure value for maximum video bitrate in MB/s")
                Tooltip(Label54, "Configure Adaptive quantization for the encoder (Spatial AQ)")
                Tooltip(Label55, "Configure strength value for Adaptive quantization")
                Tooltip(Label56, "Configure temporal value for Adaptive quantization")
                Tooltip(Label108, "Configure video aspect ratio for encoder")
                Tooltip(Label109, "Configure video resolution for encoder")
                Tooltip(Label133, "Configure video scale algorithm for re-scale or upscale video")
                Tooltip(Label75, "Configure Blu-Ray compatibility for encoder")
                Tooltip(Label53, "Configure B-Frames value for encoder")
            Else
                Button1.Enabled = False
                Button3.Enabled = False
                Button4.Enabled = False
                MessageBoxAdv.Show(res2, "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            Button1.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            MessageBoxAdv.Show(res1, "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        CleanEnv("all")
    End Sub
    Private Sub MainMenu_Close(sender As Object, e As EventArgs) Handles MyBase.FormClosed
        CleanEnv("all")
        InitExit()
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        If Label2.Text IsNot "" And TabPage1.Visible = True And File.Exists("thumbnail\1.jpg") Then
            If keyData = Keys.Left Then
                ImagePrev_Prev()
                Return True
            ElseIf keyData = Keys.Right Then
                ImagePrev_Next()
                Return True
            End If
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
    Private Sub Panel1_DragDrop(sender As Object, e As DragEventArgs) Handles Panel1.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        Dim dropStats As Boolean
        If Label2.Text IsNot "" Then
            For Each mediaFiles In files
                Dim replaceMedia As DialogResult = MessageBoxAdv.Show(Me, "Current media file already exists " & vbCrLf & vbCrLf &
                                                                      "Current media file: " & Path.GetFileName(Label2.Text) & vbCrLf &
                                                                      "New media file: " & Path.GetFileName(mediaFiles) & vbCrLf & vbCrLf &
                                                                      "Want to load new media file ? ", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If replaceMedia = DialogResult.Yes Then
                    Label2.Text = mediaFiles
                    dropStats = True
                Else
                    MessageBoxAdv.Show("Abort to load new media file !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    dropStats = False
                End If
            Next
        Else
            For Each path In files
                Label2.Text = path
            Next
            dropStats = True
        End If
        If dropStats = True Then
            If CheckBox3.Checked = True Then
                MessageBoxAdv.Show("Profile locked !, please unlock profile on video tab to open media file !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Label2.Text = ""
            ElseIf CheckBox5.Checked = True Then
                MessageBoxAdv.Show("Profile locked !, please unlock profile on audio tab to open media file !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Label2.Text = ""
            Else
                If Strings.Right(Label2.Text, 4) = ".mkv" Or Strings.Right(Label2.Text, 4) = ".mp4" Or Strings.Right(Label2.Text, 4) = ".avi" Or
                            Strings.Right(Label2.Text, 4) = ".flv" Or Strings.Right(Label2.Text, 3) = ".ts" Or Strings.Right(Label2.Text, 4) = "m2ts" Or
                            Strings.Right(Label2.Text, 4) = ".mov" Or Strings.Right(Label2.Text, 4) = ".mp4" Or Strings.Right(Label2.Text, 4) = ".vob" Or
                            Strings.Right(Label2.Text, 4) = ".3gp" Or Strings.Right(Label2.Text, 4) = ".mxf" Or Strings.Right(Label2.Text, 4) = "flac" Or
                            Strings.Right(Label2.Text, 4) = ".wav" Or Strings.Right(Label2.Text, 4) = ".mp3" Or Strings.Right(Label2.Text, 4) = ".mp2" Or
                            Strings.Right(Label2.Text, 4) = ".aac" Or Strings.Right(Label2.Text, 4) = ".dts" Or Strings.Right(Label2.Text, 4) = ".dsd" Or
                            Strings.Right(Label2.Text, 4) = ".pcm" Or Strings.Right(Label2.Text, 4) = "opus" Or Strings.Right(Label2.Text, 4) = ".ogg" Or
                            Strings.Right(Label2.Text, 4) = ".ape" Or Strings.Right(Label2.Text, 4) = "alac" Or Strings.Right(Label2.Text, 4) = "aiff" Or
                            Strings.Right(Label2.Text, 4) = ".aif" Or Strings.Right(Label2.Text, 4) = ".m4a" Or Strings.Right(Label2.Text, 4) = ".tak" Or
                            Strings.Right(Label2.Text, 4) = ".tta" Or Strings.Right(Label2.Text, 4) = ".wma" Or Strings.Right(Label2.Text, 3) = ".wv" Or
                            Strings.Right(Label2.Text, 4) = "webm" Then
                    OpenMedia_Load()
                Else
                    OpenMedia_Reset()
                    MessageBoxAdv.Show("Media file format are not supported !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        End If
    End Sub
    Private Sub Panel1_DragEnter(sender As Object, e As DragEventArgs) Handles Panel1.DragEnter
        If Button1.Enabled = True AndAlso Button3.Enabled = True AndAlso Button4.Enabled = True Then
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                e.Effect = DragDropEffects.All
            End If
        End If
    End Sub
    Private Sub Information_Btn(sender As Object, e As EventArgs) Handles Button2.Click
        Dim menu_information = New InformationMenu
        menu_information.Show()
    End Sub
    Private Sub Options_Btn(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Hide()
        Dim menu_options = New OptionsMenu
        menu_options.Show()
    End Sub
    Private Sub OpenMedia_Btn(sender As Object, e As EventArgs) Handles Button1.Click
        If CheckBox3.Checked = True Then
            MessageBoxAdv.Show("Profile locked !, please unlock profile on video tab to save media file !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Label2.Text = ""
        ElseIf CheckBox5.Checked = True Then
            MessageBoxAdv.Show("Profile locked !, please unlock profile on audio tab to save media file !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Label2.Text = ""
        Else
            OpenFileDialog.DefaultExt = "*.*|.avi|.mp4|.mkv|.mp2|.m2ts|.ts|.wav|.flac|.aiff|.alac|.mp3"
            OpenFileDialog.FilterIndex = 1
            OpenFileDialog.Filter = "All files (*.*)|*.*|AVI|*.avi|MPEG-4|*.mp4|Matroska Video|*.mkv|MPEG-2|*.m2v;*.m2ts;*.ts|FLAC|*.flac|WAV|*.wav|AIFF|*.aiff|ALAC|*.alac|AAC|*.aac|MP3|*.mp3"
            OpenFileDialog.Title = "Choose Media File"
            OpenFileDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
            If OpenFileDialog.ShowDialog() = DialogResult.OK Then
                If Strings.Right(OpenFileDialog.FileName, 4) = ".mkv" Or Strings.Right(OpenFileDialog.FileName, 4) = ".mp4" Or Strings.Right(OpenFileDialog.FileName, 4) = ".avi" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".flv" Or Strings.Right(OpenFileDialog.FileName, 3) = ".ts" Or Strings.Right(OpenFileDialog.FileName, 4) = "m2ts" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".mov" Or Strings.Right(OpenFileDialog.FileName, 4) = ".mp4" Or Strings.Right(OpenFileDialog.FileName, 4) = ".vob" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".3gp" Or Strings.Right(OpenFileDialog.FileName, 4) = ".mxf" Or Strings.Right(OpenFileDialog.FileName, 4) = "flac" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".wav" Or Strings.Right(OpenFileDialog.FileName, 4) = ".mp3" Or Strings.Right(OpenFileDialog.FileName, 4) = ".mp2" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".aac" Or Strings.Right(OpenFileDialog.FileName, 4) = ".dts" Or Strings.Right(OpenFileDialog.FileName, 4) = ".dsd" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".pcm" Or Strings.Right(OpenFileDialog.FileName, 4) = "opus" Or Strings.Right(OpenFileDialog.FileName, 4) = ".ogg" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".ape" Or Strings.Right(OpenFileDialog.FileName, 4) = "alac" Or Strings.Right(OpenFileDialog.FileName, 4) = "aiff" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".aif" Or Strings.Right(OpenFileDialog.FileName, 4) = ".m4a" Or Strings.Right(OpenFileDialog.FileName, 4) = ".tak" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".tta" Or Strings.Right(OpenFileDialog.FileName, 4) = ".wma" Or Strings.Right(OpenFileDialog.FileName, 3) = ".wv" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = "webm" Then
                    Label2.Text = OpenFileDialog.FileName
                    If ComboBox2.SelectedIndex >= 0 Then
                        ComboBox2.SelectedIndex = 0
                        ComboBox2.Text = ""
                    End If
                    OpenMedia_Load()
                Else
                    OpenMedia_Reset()
                    MessageBoxAdv.Show("Media file format are not supported !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                OpenMedia_Reset()
            End If
        End If
    End Sub
    Private Sub OpenMedia_Load()
        Dim loadInit = New Loading("Media", Label2.Text)
        loadInit.Show()
        ResetInit()
        CleanEnv("all")
        Button1.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = False
        Button7.Visible = False
        Button7.Enabled = False
        Button8.Visible = False
        Button8.Enabled = False
        ComboBox24.Enabled = True
        ComboBox23.Enabled = True
        getVideoSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34), "0")
        getAudioSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34), "0")
        getDurationSummary_Async(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34))
        ComboBox22.Items.Clear()
        ComboBox23.Items.Clear()
        ComboBox24.Items.Clear()
        ComboBox25.Items.Clear()
        ComboBox29.Items.Clear()
        getStreamSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34), "Encoding")
        If ComboBox23.Items.Count > 0 Then
            ComboBox23.SelectedIndex = 0
        End If
        If ComboBox24.Items.Count > 0 Then
            ComboBox24.SelectedIndex = 0
        End If
        CleanEnv("all")
        PictureBox1.Image = Nothing
        PictureBox1.BackColor = Color.Empty
        PictureBox1.Invalidate()
        ChapterReplace("reset", "")
        loadInit.Close()
        getPreviewSummary_Async(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34))
        getSpectrumSummary_Async()
        ComboBox31.Enabled = True
        ComboBox31.SelectedIndex = 0
        CleanEnv("minimal")
        If Label5.Text.Equals("Not Detected") = True Then
            ComboBox24.Text = ""
        End If
        If Label44.Text.Equals("Not Detected") = True Then
            ComboBox23.Text = ""
        End If
        Label76.Visible = True
        Label70.Visible = True
        Label70.Text = GetFileSize(Label2.Text)
        Label71.Visible = False
        Label77.Visible = False
        Label28.Text = "Standby"
    End Sub
    Private Sub OpenMedia_Reset()
        Label2.Text = ""
        ProgressBarAdv1.Visible = False
        Label70.Visible = False
        Label76.Visible = False
        Label71.Visible = False
        Label77.Visible = False
        ComboBox31.Enabled = False
    End Sub
    Private Sub SaveMedia_Btn(sender As Object, e As EventArgs) Handles Button6.Click
        If CheckBox3.Checked = True Then
            MessageBoxAdv.Show("Profile locked !, please unlock profile on video tab to save media file !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf CheckBox5.Checked = True Then
            MessageBoxAdv.Show("Profile locked !, please unlock profile on audio tab to save media file !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            SaveFileDialog.DefaultExt = ".mkv|.wav|.flac|.mp3"
            SaveFileDialog.FilterIndex = 1
            SaveFileDialog.Filter = "MKV|*.mkv|MP4|*.mp4|AAC|*.m4a|FLAC|*.flac|WAV|*.wav|MP3|*.mp3"
            SaveFileDialog.Title = "Save Media File"
            SaveFileDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
            If SaveFileDialog.ShowDialog() = DialogResult.OK Then
                TextBox1.Text = Path.GetFullPath(SaveFileDialog.FileName.ToString)
                OrigSavePath = Path.GetDirectoryName(SaveFileDialog.FileName.ToString)
                OrigSaveExt = Path.GetExtension(SaveFileDialog.FileName.ToString.ToLower)
                OrigSaveName = Path.GetFileNameWithoutExtension(SaveFileDialog.FileName.ToString)
            End If
        End If
    End Sub
    Private Sub ImagePreview_Next(sender As Object, e As EventArgs) Handles Button7.Click
        ImagePrev_Next()
    End Sub
    Private Sub ImagePreview_Prev(sender As Object, e As EventArgs) Handles Button8.Click
        ImagePrev_Prev()
    End Sub
    Private Sub ImagePrev_Next()
        CurPos = CInt(Label96.Text)
        MaxPos = CInt(Label98.Text)
        If ComboBox31.SelectedIndex = 0 Then
            If File.Exists("thumbnail\1.jpg") Then
                If CurPos < MaxPos Then
                    Dim ImgPrev2 As New FileStream("thumbnail\" & CurPos + 1 & ".jpg", FileMode.Open, FileAccess.Read)
                    PictureBox1.Image = Image.FromStream(ImgPrev2)
                    ImgPrev2.Close()
                    Label96.Text = CurPos + 1
                Else
                    MessageBoxAdv.Show("Already on last snapshots !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBoxAdv.Show("Already on last snapshots !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
    Private Sub ImagePrev_Prev()
        CurPos = CInt(Label96.Text)
        MaxPos = CInt(Label98.Text)
        If ComboBox31.SelectedIndex = 0 Then
            If File.Exists("thumbnail\1.jpg") Then
                If CurPos > 1 Then
                    Dim ImgPrev2 As New FileStream("thumbnail\" & CurPos - 1 & ".jpg", FileMode.Open, FileAccess.Read)
                    PictureBox1.Image = Image.FromStream(ImgPrev2)
                    ImgPrev2.Close()
                    Label96.Text = CurPos - 1
                Else
                    MessageBoxAdv.Show("Already on first snapshots !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBoxAdv.Show("Already on first snapshots !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
    Private Sub PreviewOptions(sender As Object, e As EventArgs) Handles ComboBox31.SelectedIndexChanged
        If ComboBox31.SelectedIndex = 0 Then
            If File.Exists("thumbnail\1.jpg") Then
                ImageDir = "thumbnail\1.jpg"
                Label96.Text = "1"
                Label98.Text = TotalScreenshot.ToString
                Button7.Visible = True
                Button7.Enabled = True
                Button8.Visible = True
                Button8.Enabled = True
            Else
                ImageDir = VideoErrorPlaceholder
                Label96.Text = 0
                Label98.Text = 0
                Button7.Visible = False
                Button7.Enabled = False
                Button8.Visible = False
                Button8.Enabled = False
            End If

        ElseIf ComboBox31.SelectedIndex = 1 Then
            If File.Exists("spectrum-temp.jpg") Then
                ImageDir = "spectrum-temp.jpg"
            Else
                ImageDir = SpectrumErrorPlaceholder
            End If
            If TotalSpectrum = 0 Then
                Label96.Text = 0
                Label98.Text = 0
            Else
                Label96.Text = "1"
                Label98.Text = TotalSpectrum.ToString
            End If
            Button7.Visible = False
            Button7.Enabled = False
            Button8.Visible = False
            Button8.Enabled = False
        Else
            ImageDir = VideoErrorPlaceholder
        End If
        Dim ImgPrev1 As New FileStream(ImageDir, FileMode.Open, FileAccess.Read)
        PictureBox1.Image = Image.FromStream(ImgPrev1)
        ImgPrev1.Close()
    End Sub
    Private Sub PicturePreview(sender As Object, e As EventArgs) Handles PictureBox1.Click
        If ComboBox31.SelectedIndex = 0 Then
            If File.Exists("thumbnail\1.jpg") Then
                ImageDir = "thumbnail\" & CInt(Label96.Text) & ".jpg"
            Else
                ImageDir = "null"
            End If
        ElseIf ComboBox31.SelectedIndex = 1 Then
            If File.Exists("spectrum-temp.jpg") Then
                ImageDir = "spectrum-temp.jpg"
                Label96.Text = "1"
                TotalSpectrum = 1
            Else
                ImageDir = "null"
                Label96.Text = "0"
                TotalSpectrum = 0
            End If
        Else
            ImageDir = "null"
        End If
        If ImageDir IsNot "null" Then
            Dim psi As New ProcessStartInfo With {
                .FileName = ImageDir,
                .UseShellExecute = True
            }
            Process.Start(psi)
        End If
    End Sub
    Private Sub PreviewMedia(sender As Object, e As EventArgs) Handles Button4.Click
        If Label2.Text = "" Then
            MessageBoxAdv.Show("Please open media file first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            previewMediaModule(Label2.Text, FfmpegConf & "ffplay.exe", Label5.Text)
        End If
    End Sub
    Private Sub VideoStream_Info(sender As Object, e As EventArgs) Handles ComboBox24.SelectedIndexChanged
        getVideoSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34), (CInt(Strings.Mid(ComboBox24.Text.ToString, 11)) - 1).ToString)
    End Sub
    Public Sub getVideoSummary(ffmpegletter As String, ffmpegbin As String, videoFile As String, videoStream As String)
        Newffargs = "ffprobe -hide_banner " & " -show_streams -select_streams v:" & videoStream & " " & videoFile & " 2>&1 "
        HMEGenerate("HME_Video_Summary.bat", ffmpegletter, ffmpegbin, Newffargs, "")
        Dim psi As New ProcessStartInfo("HME_Video_Summary.bat") With {
           .RedirectStandardError = False,
           .RedirectStandardOutput = True,
           .CreateNoWindow = True,
           .WindowStyle = ProcessWindowStyle.Hidden,
           .UseShellExecute = False
       }
        Dim process As Process = Process.Start(psi)
        While Not process.StandardOutput.EndOfStream
            Dim line As String = process.StandardOutput.ReadToEnd()
            Dim codec_name As String = getBetween(line, "codec_name=", "codec_long_name")
            If RemoveWhitespace(codec_name) = "" Or RemoveWhitespace(codec_name) = "mjpeg" Then
                Label5.Text = "Not Detected"
                Label10.Text = "Not Detected"
                Label6.Text = "Not Detected"
                Label14.Text = "Not Detected"
                Label7.Text = "Not Detected"
                Label16.Text = "Not Detected"
                Label18.Text = "Not Detected"
                Label20.Text = "Not Detected"
                Label22.Text = "Not Detected"
                Label24.Text = "Not Detected"
                Label74.Text = "Not Detected"
                ComboBox24.Enabled = False
            Else
                Dim codec_ln As String = getBetween(line, "codec_name=", "codec_long_name")
                Dim codec_type As String = getBetween(line, "codec_type=", "codec_tag_string")
                Dim codec_b_frames As String = getBetween(line, "has_b_frames=", "sample_aspect_ratio")
                Dim codec_pix_fmt As String = getBetween(line, "pix_fmt=", "level")
                Dim codec_color_range As String = getBetween(line, "color_range=", "color_space")
                Dim codec_color_space As String = getBetween(line, "color_space=", "color_transfer")
                Dim codec_frame_rate As String = getBetween(line, "r_frame_rate=", "avg_frame_rate=")
                Dim codec_new_bit_rate As String = RemoveWhitespace(getBetween(line, "bit_rate=", "max_bit_rate="))
                Dim codec_asp_ratio As String = getBetween(line, "display_aspect_ratio=", "pix_fmt")
                Dim codec_wi As String = getBetween(line, "width=", "height")
                Dim codec_he As String = getBetween(line, "height=", "coded_width")
                Dim codec_profile As String = getBetween(line, "profile=", "codec_type=")
                Label5.Text = codec_ln
                Label10.Text = codec_type
                Label6.Text = RemoveWhitespace(codec_wi & "x" & codec_he)
                Label14.Text = codec_b_frames
                Label7.Text = codec_asp_ratio
                Label16.Text = codec_pix_fmt
                Label18.Text = codec_color_range
                Label20.Text = codec_color_space
                Label22.Text = RemoveWhitespace(Strings.Left(codec_frame_rate, 2)) & " FPS"
                Label74.Text = codec_profile
                Dim newValue As Double
                If codec_new_bit_rate = "N/A" Then
                    Dim codec_alt_bit_rate As String = RemoveWhitespace(getBetween(line, "bitrate:", "kb/s"))
                    If Double.TryParse(codec_alt_bit_rate, newValue) Then
                        Label24.Text = Format(newValue, "###.##").ToString() & " kb/s"
                    Else
                        Label24.Text = RemoveWhitespace(codec_new_bit_rate) & " kb/s"
                    End If
                Else
                    If CInt(codec_new_bit_rate) / 1000 < 100 Then
                        If Double.TryParse(codec_new_bit_rate, newValue) Then
                            Label24.Text = Format(newValue, "###.##").ToString() & " kb/s"
                        Else
                            Label24.Text = RemoveWhitespace(codec_new_bit_rate) & " kb/s"
                        End If
                    Else
                        If Double.TryParse(codec_new_bit_rate, newValue) Then
                            Label24.Text = Format((newValue / 1000), "###.##").ToString() & " kb/s"
                        Else
                            Label24.Text = RemoveWhitespace(CInt(codec_new_bit_rate) / 1000) & " kb/s"
                        End If
                    End If
                End If
                ComboBox24.Enabled = True
            End If
        End While
        process.WaitForExit()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete("HME_Video_Summary.bat")
    End Sub
    Private Sub AudioStream_Info(sender As Object, e As EventArgs) Handles ComboBox23.SelectedIndexChanged
        getAudioSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34), (CInt(Strings.Mid(ComboBox23.Text.ToString, 11)) - 1).ToString)
    End Sub
    Public Sub getAudioSummary(ffmpegletter As String, ffmpegbin As String, audioFile As String, audioStream As String)
        Newffargs = "ffprobe -hide_banner " & " -show_streams -select_streams a:" & audioStream & " " & audioFile & " 2>&1 "
        HMEGenerate("HME_Audio_Summary.bat", ffmpegletter, ffmpegbin, Newffargs, "")
        Dim psi As New ProcessStartInfo("HME_Audio_Summary.bat") With {
           .RedirectStandardError = False,
           .RedirectStandardOutput = True,
           .CreateNoWindow = True,
           .WindowStyle = ProcessWindowStyle.Hidden,
           .UseShellExecute = False
       }
        Dim process As Process = Process.Start(psi)
        While Not process.StandardOutput.EndOfStream
            Dim line As String = process.StandardOutput.ReadToEnd()
            Dim codec_name As String = getBetween(line, "codec_name=", "codec_long_name")
            If RemoveWhitespace(codec_name) = "" Then
                Label44.Text = "Not Detected"
                Label39.Text = "Not Detected"
                Label42.Text = "Not Detected"
                Label35.Text = "Not Detected"
                Label26.Text = "Not Detected"
                Label32.Text = "Not Detected"
                Label30.Text = "Not Detected"
                Label61.Text = "Not Detected"
                Label107.Text = "Not Detected"
                ComboBox23.Enabled = False
            Else
                Dim codec_ln As String = getBetween(line, "codec_name=", "codec_long_name")
                Dim codec_type As String = getBetween(line, "codec_type=", "codec_tag_string")
                Dim codec_sample_fmt As String = getBetween(line, "sample_fmt=", "sample_rate")
                Dim codec_sample_rate As String = getBetween(line, "sample_rate=", "channels")
                Dim codec_channels As String = getBetween(line, "channels=", "channel_layout")
                Dim codec_channels_layout As String = getBetween(line, "channel_layout=", "bits_per_sample")
                Dim codec_bit_per_sample As String = getBetween(line, "bits_per_sample=", "id")
                Dim codec_bit_rate As String = getBetween(line, "bit_rate=", "max_bit_rate=")
                Dim codec_profile As String = getBetween(line, "profile=", "codec_type=")
                Label44.Text = codec_ln
                Label39.Text = codec_type
                If RemoveWhitespace(codec_sample_fmt).Equals("s16") Then
                    Label42.Text = "16 Bit"
                ElseIf RemoveWhitespace(codec_sample_fmt).Equals("s24") Then
                    Label42.Text = "24 Bit"
                ElseIf RemoveWhitespace(codec_sample_fmt).Equals("s32") Then
                    Label42.Text = "32 Bit"
                Else
                    Label42.Text = codec_sample_fmt
                End If
                Label35.Text = RemoveWhitespace(codec_sample_rate) & " hz"
                Label26.Text = codec_channels
                Label32.Text = codec_channels_layout
                Label30.Text = codec_bit_per_sample
                Label107.Text = codec_profile
                If RemoveWhitespace(codec_bit_rate).Equals("N/A") Then
                    Label61.Text = "N/A"
                Else
                    If CInt(codec_bit_rate) / 1000 >= 1000 Then
                        Label61.Text = Strings.Left(codec_bit_rate, 4) & " kb/s"
                    Else
                        Label61.Text = Strings.Left(codec_bit_rate, 3) & " kb/s"
                    End If
                End If
                ComboBox23.Enabled = True
            End If
        End While
        process.WaitForExit()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete("HME_Audio_Summary.bat")
        If Label61.Text = "N/A" Then
            Newffargs = "ffprobe -hide_banner " & " -show_format -select_streams a:" & audioStream & " " & audioFile & " 2>&1 "
            HMEGenerate("HME_Audio_Summary.bat", ffmpegletter, ffmpegbin, Newffargs, "")
            Dim new_psi As New ProcessStartInfo("HME_Audio_Summary.bat") With {
               .RedirectStandardError = False,
               .RedirectStandardOutput = True,
               .CreateNoWindow = True,
               .WindowStyle = ProcessWindowStyle.Hidden,
               .UseShellExecute = False
           }
            Dim new_process As Process = Process.Start(new_psi)
            While Not new_process.StandardOutput.EndOfStream
                Dim new_line As String = new_process.StandardOutput.ReadToEnd()
                Dim codec_bit_rate_alt As String = getBetween(new_line, "bit_rate=", "probe_score=")
                If RemoveWhitespace(codec_bit_rate_alt).Equals("N/A") Then
                    Label61.Text = "N/A"
                Else
                    If CInt(codec_bit_rate_alt) / 1000 >= 1000 Then
                        Label61.Text = Strings.Left(codec_bit_rate_alt, 4) & " kb/s"
                    Else
                        Label61.Text = Strings.Left(codec_bit_rate_alt, 3) & " kb/s"
                    End If
                End If
            End While
            new_process.WaitForExit()
        End If
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete("HME_Audio_Summary.bat")
    End Sub
    Public Async Sub getDurationSummary_Async(ffmpegletter As String, ffmpegbin As String, videoFile As String)
        Newffargs = "ffprobe -hide_banner -i " & videoFile & " 2>&1 "
        HMEGenerate("HME_Duration_Summary.bat", ffmpegletter, ffmpegbin, Newffargs, "")
        Dim psi As New ProcessStartInfo("HME_Duration_Summary.bat") With {
           .RedirectStandardError = False,
           .RedirectStandardOutput = True,
           .CreateNoWindow = True,
           .WindowStyle = ProcessWindowStyle.Hidden,
           .UseShellExecute = False
       }
        Dim process As Process = Process.Start(psi)
        While Not process.StandardOutput.EndOfStream
            Dim line As String = process.StandardOutput.ReadToEnd()
            Dim codec_dur As String
            If getBetween(line, "Duration: ", ", start:") IsNot "" Then
                codec_dur = getBetween(line, "Duration: ", ", start:")
            Else
                codec_dur = getBetween(line, "Duration: ", ", bitrate:")
            End If
            Label80.Text = codec_dur
            Label81.Text = Strings.Left(codec_dur, 8)
        End While
        Await Task.Run(Sub() process.WaitForExit())
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete("HME_Duration_Summary.bat")
    End Sub
    Private Async Sub getSpectrumSummary_Async()
        Dim loadInit = New Loading("Spectrum", Label2.Text)
        loadInit.Show()
        Dim curMediaDur As String() = Label80.Text.Split(":")
        Dim curMediaTime As Integer = TimeConversion(curMediaDur(0), curMediaDur(1), Strings.Left(curMediaDur(2), 2))
        If curMediaTime < 1800 Then
            If File.Exists("spectrum-temp.jpg") Then
                GC.Collect()
                GC.WaitForPendingFinalizers()
                File.Delete("spectrum-temp.jpg")
            End If
            Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & Label2.Text & Chr(34) & " -lavfi showspectrumpic=s=768x768:mode=separate " &
                                Chr(34) & My.Application.Info.DirectoryPath & "\spectrum-temp.jpg" & Chr(34)
            HMEGenerate("HME_Spectrum_Summary.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
            Dim generateSpectrum As New ProcessStartInfo("HME_Spectrum_Summary.bat") With {
                .RedirectStandardError = False,
                .RedirectStandardOutput = False,
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden,
                .UseShellExecute = False
            }
            Dim process As Process = Process.Start(generateSpectrum)
            Await Task.Delay(1500)
            Await Task.Run(Sub() process.WaitForExit())
            GC.Collect()
            GC.WaitForPendingFinalizers()
            File.Delete("HME_Spectrum_Summary.bat")
        End If
        loadInit.Close()
    End Sub
    Public Async Sub getPreviewSummary_Async(ffmpegLetter As String, ffmpegBin As String, videoFile As String)
        Dim curMediaDur As String() = Label80.Text.Split(":")
        Dim curMediaTime As Integer = TimeConversion(curMediaDur(0), curMediaDur(1), Strings.Left(curMediaDur(2), 2))
        Dim loadInit = New Loading("Snapshots", Label2.Text)
        loadInit.Show()
        If Label5.Text.Equals("Not Detected") = False Then
            If RemoveWhitespace(Label5.Text).Equals("png") = True Then
                videoFile = Chr(34) & Label2.Text & Chr(34)
                Newffargs = "ffmpeg -hide_banner -i " & videoFile & " -an -vcodec copy " & Chr(34) & My.Application.Info.DirectoryPath & "\" & "thumbnail\1.jpg"
                TotalScreenshot = 1
                HMEGenerate("HME_Image_Preview_Summary.bat", ffmpegLetter, ffmpegBin, Newffargs, "")
            Else
                If curMediaTime > 1800 Then
                    Newffargs = "ffmpeg -hide_banner -i " & videoFile & " -f image2 -vf " & Chr(34) & "select='not(mod(n,250))'" & Chr(34) & " -vframes 5 -vsync vfr " & Chr(34) & My.Application.Info.DirectoryPath & "\thumbnail\%%d.jpg" & Chr(34)
                    TotalScreenshot = 5
                    HMEGenerate("HME_Image_Preview_Summary.bat", ffmpegLetter, ffmpegBin, Newffargs, "")
                Else
                    Newffargs = "ffprobe -hide_banner -i " & videoFile & " -v error -select_streams v:0 -count_packets -show_entries stream=nb_read_packets -of csv=p=0"
                    HMEGenerate("HME_VF.bat", ffmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs, "")
                    FrameCount = "0"
                    Dim generateFrame As New ProcessStartInfo("HME_VF.bat") With {
                                        .RedirectStandardError = False,
                                        .RedirectStandardOutput = True,
                                        .CreateNoWindow = True,
                                        .WindowStyle = ProcessWindowStyle.Hidden,
                                        .UseShellExecute = False
                                    }
                    Dim process As Process = Process.Start(generateFrame)
                    Do
                        Dim lineReader As StreamReader = process.StandardOutput
                        Dim line As String = Await Task.Run(Function() lineReader.ReadLineAsync)
                        FrameCount = line
                    Loop Until Await Task.Run(Function() process.StandardOutput.EndOfStream)
                    Await Task.Run(Sub() process.WaitForExit())
                    If FrameCount >= 50 Then
                        Newffargs = "ffmpeg -hide_banner -i " & videoFile & " -f image2 -vf " & Chr(34) & "select='not(mod(n," & CInt(FrameCount / 50) & "))'" & Chr(34) & " -vframes 5 -vsync vfr " & Chr(34) & My.Application.Info.DirectoryPath & "\thumbnail\%%d.jpg" & Chr(34)
                        TotalScreenshot = 5
                        HMEGenerate("HME_Image_Preview_Summary.bat", ffmpegLetter, ffmpegBin, Newffargs, "")
                    ElseIf FrameCount < 50 Then
                        Newffargs = "ffmpeg -hide_banner -i " & videoFile & " -ss 00:00:00.000 -vframes 1 -s 1920x1080 -f image2 " & Chr(34) & My.Application.Info.DirectoryPath & "\thumbnail\1.jpg" & Chr(34)
                        Newffargs2 = "ffmpeg -hide_banner -i " & videoFile & " -ss 00:00:01.000 -vframes 1 -s 1920x1080 -f image2 " & Chr(34) & My.Application.Info.DirectoryPath & "\thumbnail\2.jpg" & Chr(34)
                        TotalScreenshot = 2
                        HMEGenerate("HME_Image_Preview_Summary.bat", ffmpegLetter, ffmpegBin, Newffargs, Newffargs2)
                    Else
                        TotalScreenshot = 0
                    End If
                End If
            End If
            Label96.Text = 1
            Label98.Text = 0
            If Newffargs IsNot "" Then
                Dim generateSnapshots As New ProcessStartInfo("HME_Image_Preview_Summary.bat") With {
                                        .RedirectStandardError = False,
                                        .RedirectStandardOutput = False,
                                        .CreateNoWindow = True,
                                        .WindowStyle = ProcessWindowStyle.Hidden,
                                        .UseShellExecute = False
                                    }
                Dim new_process As Process = Process.Start(generateSnapshots)
                Await Task.Delay(1500)
                Await Task.Run(Sub() new_process.WaitForExit())
                If File.Exists("thumbnail\1.jpg") Then
                    Dim ImgPrev1 As New FileStream("thumbnail\1.jpg", FileMode.Open, FileAccess.Read)
                    PictureBox1.Image = Image.FromStream(ImgPrev1)
                    ImgPrev1.Close()
                    If RemoveWhitespace(Label5.Text).Equals("png") = True Then
                        Label96.Text = 0
                        Label98.Text = 0
                        Button7.Visible = False
                        Button7.Enabled = False
                        Button8.Visible = False
                        Button8.Enabled = False
                    Else
                        Label96.Text = 1
                        Label98.Text = TotalScreenshot.ToString
                        Button7.Visible = True
                        Button7.Enabled = True
                        Button8.Visible = True
                        Button8.Enabled = True
                    End If
                Else
                    Dim ImgPrev1 As New FileStream(VideoErrorPlaceholder, FileMode.Open, FileAccess.Read)
                    PictureBox1.Image = Image.FromStream(ImgPrev1)
                    ImgPrev1.Close()
                    Label96.Text = 0
                    Label98.Text = 0
                    Button7.Visible = False
                    Button7.Enabled = False
                    Button8.Visible = False
                    Button8.Enabled = False
                End If
            End If
        Else
            videoFile = Chr(34) & Label2.Text & Chr(34)
            Newffargs = "ffmpeg -hide_banner -i " & videoFile & " -an -vcodec copy " & Chr(34) & My.Application.Info.DirectoryPath & "\" & "thumbnail\1.jpg"
            HMEGenerate("HME_Audio_Only_Summary.bat", ffmpegLetter, FfmpegConf, Newffargs, "")
            RunProcAsync("HME_Audio_Only_Summary.bat")
            If File.Exists("thumbnail\1.jpg") Then
                Dim ImgPrev1 As New FileStream("thumbnail\1.jpg", FileMode.Open, FileAccess.Read)
                PictureBox1.Image = Image.FromStream(ImgPrev1)
                ImgPrev1.Close()
                Label96.Text = 1
                Label98.Text = 1
                Button7.Visible = True
                Button7.Enabled = True
                Button8.Visible = True
                Button8.Enabled = True
            Else
                Dim ImgPrev1 As New FileStream(VideoErrorPlaceholder, FileMode.Open, FileAccess.Read)
                PictureBox1.Image = Image.FromStream(ImgPrev1)
                ImgPrev1.Close()
                Label96.Text = 0
                Label98.Text = 0
                Button7.Visible = False
                Button7.Enabled = False
                Button8.Visible = False
                Button8.Enabled = False
            End If
        End If
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete("HME_Audio_Only_Summary.bat")
        File.Delete("HME_Image_Preview_Summary.bat")
        File.Delete("HME_VF.bat")
        Button1.Enabled = True
        Button3.Enabled = True
        Button4.Enabled = True
        loadInit.Close()
    End Sub
    Public Sub getStreamSummary(ffmpegletter As String, ffmpegbin As String, videoFile As String, ffmpegMode As String)
        Newffargs = "ffmpeg -hide_banner -i " & videoFile & " 2>&1 "
        HMEGenerate("HME_Stream_Summary.bat", ffmpegletter, ffmpegbin, Newffargs, "")
        Dim psi As New ProcessStartInfo("HME_Stream_Summary.bat") With {
           .RedirectStandardError = False,
           .RedirectStandardOutput = True,
           .CreateNoWindow = True,
           .WindowStyle = ProcessWindowStyle.Hidden,
           .UseShellExecute = False
       }
        Dim process As Process = Process.Start(psi)
        While Not process.StandardOutput.EndOfStream
            Dim line As String = process.StandardOutput.ReadToEnd()
            Dim stream_info As String = getBetween(line, "kb/s", "At least one output file must be specified")
            StreamInfo = stream_info
        End While
        process.WaitForExit()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete("HME_Stream_Summary.bat")
        Dim start As Integer
        Dim videoRegex As New Regex(": Video:", RegexOptions.IgnoreCase Or RegexOptions.Singleline)
        Dim audioRegex As New Regex(": Audio:", RegexOptions.IgnoreCase Or RegexOptions.Singleline)
        Dim videoMatches As MatchCollection = videoRegex.Matches(StreamInfo)
        Dim audioMatches As MatchCollection = audioRegex.Matches(StreamInfo)
        If ffmpegMode = "Encoding" Then
            ComboBox22.Items.Clear()
            ComboBox23.Items.Clear()
            ComboBox24.Items.Clear()
            ComboBox25.Items.Clear()
            ComboBox29.Items.Clear()
            If Label5.Text.Equals("Not Detected") = False Then
                For start = 1 To videoMatches.Count
                    ComboBox24.Items.Add("Stream #0:" & start)
                    ComboBox29.Items.Add("Stream #0:" & start)
                Next
            End If
            If Label44.Text.Equals("Not Detected") = False Then
                For start = 1 To audioMatches.Count
                    ComboBox22.Items.Add("Stream #0:" & start)
                    ComboBox25.Items.Add("Stream #0:" & start)
                    ComboBox23.Items.Add("Stream #0:" & start)
                Next
            End If
        ElseIf ffmpegMode = "Muxing" Then
            ComboBox22.Items.Clear()
            ComboBox25.Items.Clear()
            ComboBox29.Items.Clear()
            For start = 1 To videoMatches.Count
                ComboBox29.Items.Add("Stream #0:" & start)
            Next
            For start = 1 To audioMatches.Count
                ComboBox25.Items.Add("Stream #0:" & start)
                ComboBox22.Items.Add("Stream #0:" & start)
            Next
        ElseIf ffmpegMode = "Muxing + Custom" Then
            ComboBox22.Items.Clear()
            ComboBox25.Items.Clear()
            ComboBox29.Items.Clear()
            For start = 1 To videoMatches.Count
                ComboBox29.Items.Add("Stream #0:" & start)
            Next
            For start = 1 To audioMatches.Count + 1
                ComboBox22.Items.Add("Stream #0:" & start)
                ComboBox25.Items.Add("Stream #0:" & start)
            Next
        ElseIf ffmpegMode = "Trim" Then
            ComboBox27.Items.Clear()
            ComboBox22.Items.Clear()
            ComboBox29.Items.Clear()
            If ComboBox28.SelectedIndex = 0 Then
                If Label5.Text.Equals("Not Detected") = False Then
                    For start = 1 To videoMatches.Count
                        ComboBox29.Items.Add("Stream #0:" & start)
                        ComboBox27.Items.Add("Stream #0:" & start)
                    Next
                End If
            Else
                If Label5.Text.Equals("Not Detected") = False Then
                    For start = 1 To videoMatches.Count
                        ComboBox29.Items.Add("Stream #0:" & start)
                    Next
                End If
                If Label44.Text.Equals("Not Detected") = False Then
                    For start = 1 To audioMatches.Count
                        ComboBox22.Items.Add("Stream #0:" & start)
                        ComboBox27.Items.Add("Stream #0:" & start)
                    Next
                End If
            End If
        End If
    End Sub
    Private Async Sub StartEncode(sender As Object, e As EventArgs) Handles Button3.Click
        DebugMode = FindConfig("config.ini", "Debug Mode:")
        FrameConfig = FindConfig("config.ini", "Frame Count:")
        FfmpegConfig = FindConfig("config.ini", "FFMPEG Binary:")
        FfmpegConf = FfmpegConfig.Remove(0, 14) & "\"
        FfmpegLetter = String.Concat(FfmpegConf.AsSpan(0, 1), ":")
        Hwdefconfig = FindConfig("config.ini", "GPU Engine:")
        Button3.Enabled = False
        If Hwdefconfig IsNot "null" And Hwdefconfig.Remove(0, 11) IsNot "" Then
            If Label2.Text IsNot "" Then
                If TextBox1.Text IsNot "" Then
                    If CheckBox11.Checked = False And CheckBox11.Enabled = False And CheckBox8.Checked = True Then
                        VideoFilePath = TextBox15.Text.ToString
                    Else
                        VideoFilePath = Label2.Text.ToString
                    End If
                    If DebugMode IsNot "null" Then
                        Newdebugmode = DebugMode.Remove(0, 11)
                        If FrameConfig IsNot "null" Then
                            FrameMode = FrameConfig.Remove(0, 12)
                        Else
                            FrameMode = "False"
                        End If
                    Else
                        Newdebugmode = "False"
                    End If
                    If Hwdefconfig = "GPU Engine: " Then
                        HwAccelFormat = ""
                        HwAccelDev = ""
                    Else
                        HwAccelFormat = "-hwaccel_output_format " & Hwdefconfig.Remove(0, 11)
                        HwAccelDev = Hwdefconfig.Remove(0, 11)
                    End If
                    If Newdebugmode = "True" Then
                        NotifyIcon("Debug mode was actived !", "Progressbar will not working correctly while encoding", 1000, True)
                    End If
                    If CheckBox1.Checked = False And CheckBox4.Checked = False And CheckBox15.Checked = False And CheckBox8.Checked = False And CheckBox6.Checked = False Then
                        MessageBoxAdv.Show("Current configuration is not valid !" & vbCrLf & vbCrLf & "Please check current configuration", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ElseIf CheckBox15.Checked = True And CheckBox14.Checked = True Then
                        If File.Exists(My.Application.Info.DirectoryPath & "\FFMETADATAFILE") Then
                            If CheckBox1.Checked = True And CheckBox3.Checked = True Then
                                If CheckBox4.Checked = True And CheckBox5.Checked = True Then
                                    If File.Exists(AudioStreamFlagsPath & "HME_Audio_0.txt") And File.Exists(VideoStreamFlagsPath & "HME_Video_0.txt") Then
                                        FlagsCount = ComboBox22.Items.Count
                                        For FlagsStart = 1 To FlagsCount
                                            My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (FlagsStart - 1).ToString & ".txt")), True)
                                        Next
                                        If File.Exists("HME_Audio_Flags.txt") Then
                                            Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                            Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & RichTextBox5.Text & " " & RichTextBox1.Text & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                            HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                        End If
                                    Else
                                        MessageBoxAdv.Show("There are still missing video or audio stream config !" & vbCrLf & vbCrLf & "Please configure video or audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    End If
                                ElseIf CheckBox4.Checked = False And CheckBox5.Checked = False Then
                                    Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & RichTextBox5.Text & " " & RichTextBox1.Text & "  -an " & Chr(34) & TextBox1.Text & Chr(34)
                                    HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                End If
                            Else
                                Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & RichTextBox5.Text & " -c copy " & RichTextBox1.Text & Chr(34) & TextBox1.Text & Chr(34)
                                HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                            End If
                        Else
                            MessageBoxAdv.Show("Please re-lock chapter profile first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    ElseIf CheckBox7.Checked = True And CheckBox8.Checked = True Then
                        If ComboBox1.Text = "Original Quality" Then
                            Newffargs = "ffmpeg -hide_banner " & RichTextBox4.Text & " " & Chr(34) & TextBox1.Text & Chr(34)
                            HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                        ElseIf ComboBox1.Text = "Custom Quality" Then
                            If File.Exists(AudioStreamFlagsPath & "HME_Audio_0.txt") And File.Exists(VideoStreamFlagsPath & "HME_Video_0.txt") Then
                                StreamCount = ComboBox22.Items.Count
                                For StreamStart = 1 To StreamCount
                                    My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (StreamStart - 1).ToString & ".txt")), True)
                                Next
                                Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                If CheckBox4.Checked = True And CheckBox5.Checked = True And CheckBox2.Checked = False Then
                                    Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & RichTextBox4.Text & " " & RichTextBox1.Text & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                    HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                ElseIf CheckBox4.Checked = True And CheckBox5.Checked = False And CheckBox2.Checked = False Then
                                    Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & RichTextBox4.Text & " " & RichTextBox1.Text & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                    HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                End If
                            Else
                                MessageBoxAdv.Show("There are still missing video or audio stream config !" & vbCrLf & vbCrLf & "Please configure video or audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End If
                    ElseIf CheckBox2.Checked = True And CheckBox6.Checked = True Then
                        If ComboBox26.Text = "Original Quality" Then
                            If ComboBox28.Text = "Video Only" Then
                                Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & RichTextBox3.Text & " -map 0:0 -c:v:0 copy -an -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                            ElseIf ComboBox28.Text = "Video + Audio (Specific source)" Then
                                Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & RichTextBox3.Text & " -map 0:0 -map 0:" & CInt(Strings.Mid(ComboBox27.Text.ToString, 11)).ToString & " -c copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                            ElseIf ComboBox28.Text = "Video + Audio (All source)" Then
                                Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & RichTextBox3.Text & " -map 0 -c copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                            ElseIf ComboBox28.Text = "Audio Only (Specific Source)" Then
                                If Label5.Text.Equals("Not Detected") = False Then
                                    Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & RichTextBox3.Text & " -map 0:" & CInt(Strings.Mid(ComboBox27.Text.ToString, 11)).ToString & " -vn -c:a:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & " copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                Else
                                    Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & RichTextBox3.Text & " -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & " -c:a:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11))).ToString & " copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                End If
                            Else
                                Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & RichTextBox3.Text & " -map 0 -c copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                            End If
                            HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                        ElseIf ComboBox26.Text = "Custom Quality" Then
                            If ComboBox28.Text = "Video Only" Then
                                Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & RichTextBox3.Text & " -map 0:0 " & RichTextBox1.Text & " -an " & Chr(34) & TextBox1.Text & Chr(34)
                                HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                            ElseIf ComboBox28.Text = "Video + Audio (Specific source)" Then
                                If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt") And File.Exists(VideoStreamFlagsPath & "HME_Video_0.txt") Then
                                    FlagsCount = ComboBox22.Items.Count
                                    For FlagsStart = 1 To FlagsCount
                                        My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt")), True)
                                    Next
                                    Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                    Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & RichTextBox3.Text & " -map 0:0 -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11))).ToString & RichTextBox1.Text & " " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                    HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                Else
                                    MessageBoxAdv.Show("There are still missing video or audio stream config !" & vbCrLf & vbCrLf & "Please configure video or audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            ElseIf ComboBox28.Text = "Video + Audio (All source)" Then
                                If File.Exists(AudioStreamFlagsPath & "HME_Audio_0.txt") And File.Exists(VideoStreamFlagsPath & "HME_Video_0.txt") Then
                                    FlagsCount = ComboBox22.Items.Count
                                    For FlagsStart = 1 To FlagsCount
                                        My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (FlagsStart - 1).ToString & ".txt")), True)
                                    Next
                                    If File.Exists("HME_Audio_Flags.txt") Then
                                        Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                        Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & RichTextBox3.Text & " -map 0 " & RichTextBox1.Text & " " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                        HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                    End If
                                Else
                                    MessageBoxAdv.Show("There are still missing video or audio stream config !" & vbCrLf & vbCrLf & "Please configure video or audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            ElseIf ComboBox28.Text = "Audio Only (Specific Source)" Then
                                If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt") Then
                                    FlagsCount = ComboBox22.Items.Count
                                    For FlagsStart = 1 To FlagsCount
                                        My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt")), True)
                                    Next
                                    Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                    If Label5.Text.Equals("Not Detected") = False Then
                                        Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & RichTextBox3.Text & " -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11))).ToString & " -vn " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                    Else
                                        Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & RichTextBox3.Text & " -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                    End If
                                    HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                Else
                                    MessageBoxAdv.Show("Audio stream config not found !" & vbCrLf & vbCrLf & "Please configure audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            Else
                                If File.Exists(AudioStreamFlagsPath & "HME_Audio_0.txt") And File.Exists(VideoStreamFlagsPath & "HME_Video_0.txt") Then
                                    FlagsCount = ComboBox22.Items.Count
                                    For FlagsStart = 1 To FlagsCount
                                        My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (FlagsStart - 1).ToString & ".txt")), True)
                                    Next
                                    If File.Exists("HME_Audio_Flags.txt") Then
                                        Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                        Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & RichTextBox3.Text & " -map 0 " & RichTextBox1.Text & " " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                        HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                    End If
                                Else
                                    MessageBoxAdv.Show("There are still missing video or audio stream config !" & vbCrLf & vbCrLf & "Please configure video or audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            End If
                        End If
                    ElseIf CheckBox1.Checked = True And CheckBox3.Checked = True Then
                        If Strings.Right(TextBox1.Text, 4) = "flac" Or Strings.Right(TextBox1.Text, 4) = ".mp3" Or Strings.Right(TextBox1.Text, 4) = ".wav" Then
                            MessageBoxAdv.Show("Invalid file extension for saved media file !" & vbCrLf & vbCrLf &
                                                   "Current file extensions " & vbCrLf &
                                                   Strings.Right(TextBox1.Text, 4) & vbCrLf & vbCrLf &
                                                   "Current available file extensions " & vbCrLf &
                                                   ".mkv", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Else
                            If CheckBox4.Checked = True And CheckBox5.Checked = True Then
                                If File.Exists(AudioStreamFlagsPath & "HME_Audio_0.txt") And File.Exists(VideoStreamFlagsPath & "HME_Video_0.txt") Then
                                    FlagsCount = ComboBox22.Items.Count
                                    For FlagsStart = 1 To FlagsCount
                                        My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (FlagsStart - 1).ToString & ".txt")), True)
                                    Next
                                    If File.Exists("HME_Audio_Flags.txt") Then
                                        Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                        Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & RichTextBox1.Text & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                        HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                    End If
                                Else
                                    MessageBoxAdv.Show("There are still missing video or audio stream config !" & vbCrLf & vbCrLf & "Please configure video or audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            ElseIf CheckBox4.Checked = False And CheckBox5.Checked = False Then
                                Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & RichTextBox1.Text & "  -an " & Chr(34) & TextBox1.Text & Chr(34)
                                HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                            End If
                        End If
                    ElseIf CheckBox1.Checked = False And CheckBox3.Checked = False Then
                        If Strings.Right(TextBox1.Text, 4) = ".mkv" Then
                            MessageBoxAdv.Show("Invalid file extension for saved media file !" & vbCrLf & vbCrLf &
                                                   "Current file extensions " & vbCrLf &
                                                    Strings.Right(TextBox1.Text, 4) & vbCrLf & vbCrLf &
                                                    "Current available file extensions " & vbCrLf &
                                                    ".flac, .wav, .mp3", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Else
                            If CheckBox4.Checked = True And CheckBox5.Checked = True Then
                                If File.Exists(AudioStreamFlagsPath & "HME_Audio_0.txt") Then
                                    FlagsCount = ComboBox22.Items.Count
                                    For FlagsStart = 1 To FlagsCount
                                        My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (FlagsStart - 1).ToString & ".txt")), True)
                                    Next
                                    If File.Exists("HME_Audio_Flags.txt") Then
                                        Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                        Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & VideoFilePath & Chr(34) & " -vn " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                        HMEGenerate("HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                    Else
                                        MessageBoxAdv.Show("Audio stream config not found !" & vbCrLf & vbCrLf & "Please configure audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    End If
                                End If
                            End If
                        End If
                    End If
                Else
                    MessageBoxAdv.Show("Please choose save media file first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBoxAdv.Show("Please choose media file source first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            If File.Exists("HME.bat") Then
                If File.Exists(TextBox1.Text) Then
                    MessageBoxAdv.Show("Current file " & TextBox1.Text & " Already exists !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Dim duplicateFile As DialogResult = MessageBoxAdv.Show(Me, "Remove existing file " & TextBox1.Text & " ? ", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If duplicateFile = DialogResult.Yes Then
                        GC.Collect()
                        GC.WaitForPendingFinalizers()
                        File.Delete(TextBox1.Text)
                    Else
                        MessageBoxAdv.Show("Please move existing file to other location or choose other name or file location !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                Else
                    ProgressBarAdv1.Visible = True
                    Label28.Text = "Calculate frame"
                    Label70.Text = GetFileSize(VideoFilePath)
                    Label71.Visible = False
                    Label77.Visible = False
                    Button1.Enabled = False
                    Button6.Enabled = False
                    TextBox1.Enabled = False
                    ProgressBarAdv1.Refresh()
                    ProgressBarAdv1.Value = 0
                    ProgressBarAdv1.Refresh()
                    If FrameMode = "True" Then
                        FrameCount = "0"
                    ElseIf Label5.Text.Equals("Not Detected") = True Or CheckBox1.Checked = False And CheckBox3.Checked = False Then
                        If CheckBox2.Checked = True And CheckBox6.Checked = True Then
                            FrameCount = TrimEndTime - TrimStartTime
                        Else
                            Dim TimeFrame As String() = Label81.Text.Split(":")
                            FrameCount = TimeConversion(TimeFrame(0), TimeFrame(1), TimeFrame(2))
                        End If
                    Else
                        Dim loadInit = New Loading("Frame", Label2.Text)
                        loadInit.Show()
                        FrameCount = "0"
                        Newffargs = "ffprobe -hide_banner -i " & Chr(34) & VideoFilePath & Chr(34) & " -v error -select_streams v:0 -count_packets -show_entries stream=nb_read_packets -of csv=p=0"
                        HMEGenerate("HME_VF.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs, "")
                        Dim psi As New ProcessStartInfo("HME_VF.bat") With {
                            .RedirectStandardError = False,
                            .RedirectStandardOutput = True,
                            .CreateNoWindow = True,
                            .WindowStyle = ProcessWindowStyle.Hidden,
                            .UseShellExecute = False
                        }
                        Dim process As Process = Process.Start(psi)
                        Do
                            Dim lineReader As StreamReader = process.StandardOutput
                            Dim line As String = Await Task.Run(Function() lineReader.ReadLineAsync)
                            FrameCount = line
                        Loop Until Await Task.Run(Function() process.StandardOutput.EndOfStream)
                        Await Task.Run(Sub() process.WaitForExit())
                        loadInit.Close()
                    End If
                    Label28.Text = "Encoding"
                    EncStartTime = DateTime.Now
                    ProgressBarAdv1.Minimum = 0
                    If FrameMode = "False" Then
                        ProgressBarAdv1.Maximum = FrameCount
                    Else
                        ProgressBarAdv1.Maximum = 100
                    End If
                    Dim new_psi As New ProcessStartInfo("HME.bat") With {
                        .RedirectStandardError = True,
                        .RedirectStandardOutput = False,
                        .CreateNoWindow = True,
                        .WindowStyle = ProcessWindowStyle.Hidden,
                        .UseShellExecute = False
                    }
                    If Newdebugmode = "True" And FrameMode = "False" Then
                        Dim new_process As Process = Process.Start(new_psi)
                        Do
                            Dim lineReader As StreamReader = Await Task.Run(Function() new_process.StandardError)
                            Dim line As String = Await Task.Run(Function() lineReader.ReadLineAsync)
                            If RemoveWhitespace(getBetween(line, "frame= ", " fps")) = "" Or RemoveWhitespace(getBetween(line, "frame= ", " fps")) = "0" Then
                                FfmpegEncStats = "Frame Error!"
                            End If
                            FfmpegErr = Await Task.Run(Function() new_process.StandardError.ReadToEndAsync)
                        Loop Until Await Task.Run(Function() new_process.StandardError.EndOfStream)
                        Await Task.Run(Sub() new_process.WaitForExit())
                    ElseIf Newdebugmode = "True" And FrameMode = "True" Then
                        Dim new_process As Process = Process.Start(new_psi)
                        FfmpegErr = Await Task.Run(Function() new_process.StandardError.ReadToEndAsync)
                        Await Task.Run(Sub() new_process.WaitForExit())
                    ElseIf Newdebugmode = "False" And FrameMode = "True" Then
                        Dim new_process As Process = Process.Start(new_psi)
                        Await Task.Run(Sub() new_process.WaitForExit())
                    ElseIf Newdebugmode = "False" And FrameMode = "False" Then
                        Dim new_process As Process = Process.Start(new_psi)
                        Do
                            Dim lineReader As StreamReader = Await Task.Run(Function() new_process.StandardError)
                            Dim line As String = Await Task.Run(Function() lineReader.ReadLineAsync)
                            Dim encAudioFrame As String()
                            If Label5.Text.Equals("Not Detected") = True Or CheckBox1.Checked = False And CheckBox3.Checked = False Then
                                If RemoveWhitespace(getBetween(line, "time=", " bitrate")).Equals("") = False Then
                                    If RemoveWhitespace(getBetween(line, "time=", " bitrate")) <= FrameCount Then
                                        encAudioFrame = RemoveWhitespace(getBetween(line, "time=", " bitrate")).Split(":")
                                        ProgressBarAdv1.Value = CInt(TimeConversion(encAudioFrame(0), encAudioFrame(1), encAudioFrame(2)))
                                    End If
                                Else
                                    FfmpegEncStats = "Frame Error!"
                                End If
                            Else
                                If RemoveWhitespace(getBetween(line, "frame= ", " fps")) = "" Or RemoveWhitespace(getBetween(line, "frame= ", " fps")) = "0" Then
                                    FfmpegEncStats = "Frame Error!"
                                ElseIf RemoveWhitespace(getBetween(line, "frame= ", " fps")) <= FrameCount Then
                                    ProgressBarAdv1.Value = CInt(RemoveWhitespace(getBetween(line, "frame=", " fps")))
                                End If
                            End If
                        Loop Until Await Task.Run(Function() new_process.StandardError.EndOfStream)
                        Await Task.Run(Sub() new_process.WaitForExit())
                    End If
                    EncEndTime = DateTime.Now
                    If File.Exists(TextBox1.Text) Then
                        Dim destFile As New FileInfo(TextBox1.Text)
                        If destFile.Length / 1024 / 1024 < 1.0 Then
                            If destFile.Length / 1024 < 1.0 Then
                                NotifyIcon("Media File has failed to encoded !", "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), 1000, False)
                                Label28.Text = "Error"
                                ProgressBarAdv1.Value = ProgressBarAdv1.Maximum
                            Else
                                If ProgressBarAdv1.Value <> ProgressBarAdv1.Maximum Then
                                    ProgressBarAdv1.Value = ProgressBarAdv1.Maximum
                                End If
                                If Newdebugmode = "True" Then
                                    MessageBoxAdv.Show("Encoding success !" & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information, FfmpegErr)
                                Else
                                    NotifyIcon("Media File has successfuly to encoded !", "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), 1000, True)
                                End If
                                Label70.Visible = True
                                Label76.Visible = True
                                Label71.Visible = True
                                Label77.Visible = True
                                Label28.Text = "Completed"
                                Label71.Text = "" & GetFileSize(TextBox1.Text)
                            End If
                        Else
                            If ProgressBarAdv1.Value <> ProgressBarAdv1.Maximum Then
                                ProgressBarAdv1.Value = ProgressBarAdv1.Maximum
                            End If
                            If Newdebugmode = "True" Then
                                MessageBoxAdv.Show("Encoding success !" & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information, FfmpegErr)
                            Else
                                NotifyIcon("Media File has successfuly to encoded !", "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), 1000, True)
                            End If
                            Label70.Visible = True
                            Label76.Visible = True
                            Label71.Visible = True
                            Label77.Visible = True
                            Label28.Text = "Completed"
                            Label71.Text = "" & GetFileSize(TextBox1.Text)
                        End If
                    Else
                        NotifyIcon("Media File has failed to encoded !", "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), 1000, False)
                        Label28.Text = "Error"
                        ProgressBarAdv1.Value = ProgressBarAdv1.Maximum
                    End If
                    Button1.Enabled = True
                    Button6.Enabled = True
                    CleanEnv("null")
                    ProgressBarAdv1.Value = 0
                    ProgressBarAdv1.Visible = False
                    OnCompleted(ComboBox36.Text)
                End If
            End If
        Else
            MessageBoxAdv.Show("GPU HW Accelerated are not set !" & vbCrLf & vbCrLf & "Please configure it on options menu before start encoding", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        Button3.Enabled = True
    End Sub
    Private Sub EnableVideoCheck(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            If Label2.Text IsNot "" Or CheckBox8.Checked And TextBox15.Text IsNot "" Then
                If Label5.Text.Equals("Not Detected") = True And TextBox15.Text Is "" Then
                    CheckBox1.Checked = False
                    MessageBoxAdv.Show("Current media file does not contain any video stream !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Dim videoRes As String()
                    videoRes = Label6.Text.Split("x")
                    ComboBox2.Enabled = True
                    CheckBox3.Enabled = True
                    ComboBox29.Enabled = True
                    Button15.Enabled = True
                    Button16.Enabled = True
                    ComboBox29.SelectedIndex = 0
                    Resolution_Width_UpDown.Minimum = 0
                    Resolution_Height_UpDown.Minimum = 0
                    Resolution_Width_UpDown.Maximum = videoRes(0)
                    Resolution_Height_UpDown.Maximum = videoRes(1)
                End If
            Else
                CheckBox1.Checked = False
                MessageBoxAdv.Show("Please choose media file source first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            ComboBox2.SelectedIndex = -1
            ComboBox2.Enabled = False
            CheckBox3.Enabled = False
            CheckBox3.Checked = False
            CRF_VBR_UpDown.Enabled = False
            CRF_VBR_UpDown.Value = 0
            ComboBox21.Enabled = False
            ComboBox21.SelectedIndex = -1
            ComboBox10.Enabled = False
            ComboBox10.SelectedIndex = -1
            ComboBox30.Enabled = False
            ComboBox30.SelectedIndex = -1
            ComboBox8.Enabled = False
            ComboBox8.SelectedIndex = -1
            BitRate_UpDown.Enabled = False
            BitRate_UpDown.Value = 0
            ComboBox14.Enabled = False
            ComboBox14.SelectedIndex = -1
            ComboBox5.Enabled = False
            ComboBox5.SelectedIndex = -1
            ComboBox3.Enabled = False
            ComboBox3.SelectedIndex = -1
            ComboBox7.Enabled = False
            ComboBox7.SelectedIndex = -1
            ComboBox4.Enabled = False
            ComboBox4.SelectedIndex = -1
            ComboBox11.Enabled = False
            ComboBox11.SelectedIndex = -1
            ComboBox12.Enabled = False
            ComboBox12.SelectedIndex = -1
            ComboBox13.Enabled = False
            ComboBox13.SelectedIndex = -1
            MaxBitRate_UpDown.Enabled = False
            MaxBitRate_UpDown.Value = 0
            ComboBox6.Enabled = False
            ComboBox6.SelectedIndex = -1
            ComboBox9.Enabled = False
            ComboBox9.SelectedIndex = -1
            ComboBox29.Enabled = False
            ComboBox29.SelectedIndex = -1
            Button15.Enabled = False
            Button16.Enabled = False
            ComboBox32.SelectedIndex = -1
            ComboBox32.Enabled = False
            Resolution_Height_UpDown.Value = 0
            Resolution_Height_UpDown.Enabled = False
            Resolution_Width_UpDown.Value = 0
            Resolution_Width_UpDown.Enabled = False
            CheckBox12.Enabled = False
            RichTextBox1.Text = ""
        End If
    End Sub
    Private Sub VideoStreamSource(sender As Object, e As EventArgs) Handles ComboBox29.SelectedIndexChanged
        If ComboBox29.SelectedIndex >= 0 Then
            VideoStreamFlags = VideoStreamFlagsPath & "HME_Video_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
            VideoStreamConfig = VideoStreamConfigPath & "HME_Video_Config_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
            If File.Exists(VideoStreamFlags) And File.Exists(VideoStreamConfig) Then
                Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Configuration for video with stream #0:" & CInt(Strings.Mid(ComboBox29.Text.ToString, 11)).ToString & "  already exists !" & vbCrLf & vbCrLf &
                                                                      "Check old video stream configuration ?" & vbCrLf & "NOTE: This will replace existing video configuration", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If configResult = DialogResult.Yes Then
                    VcodecReset()
                    Dim prevVideoBrCompat As String = FindConfig(VideoStreamConfig, "BRCompat=")
                    Dim prevVideoOvr As String = FindConfig(VideoStreamConfig, "OvrBitrate=")
                    Dim prevVideoBref As String = FindConfig(VideoStreamConfig, "Bref=")
                    Dim prevVideoCodec As String = FindConfig(VideoStreamConfig, "Codec=")
                    Dim prevVideoFps As String = FindConfig(VideoStreamConfig, "Fps=")
                    Dim prevVideoLvl As String = FindConfig(VideoStreamConfig, "Level=")
                    Dim prevVideoMaxBr As String = FindConfig(VideoStreamConfig, "MaxBitrate=")
                    Dim prevVideoMp As String = FindConfig(VideoStreamConfig, "Multipass=")
                    Dim prevVideoPreset As String = FindConfig(VideoStreamConfig, "Preset=")
                    Dim prevVideoPixFmt As String = FindConfig(VideoStreamConfig, "PixelFormat=")
                    Dim prevVideoProfile As String = FindConfig(VideoStreamConfig, "Profile=")
                    Dim prevVideoRateCtr As String = FindConfig(VideoStreamConfig, "RateControl=")
                    Dim prevVideoSpatialAQ As String = FindConfig(VideoStreamConfig, "SpatialAQ=")
                    Dim prevVideoAQStrength As String = FindConfig(VideoStreamConfig, "AQStrength=")
                    Dim prevVideoTempAQ As String = FindConfig(VideoStreamConfig, "TemporalAQ=")
                    Dim prevVideoTargetQL As String = FindConfig(VideoStreamConfig, "TargetQL=")
                    Dim prevVideoTier As String = FindConfig(VideoStreamConfig, "Tier=")
                    Dim prevVideoTune As String = FindConfig(VideoStreamConfig, "Tune=")
                    Dim prevVideoAspectRatio As String = FindConfig(VideoStreamConfig, "AspectRatio=")
                    Dim prevVideoResolution As String = FindConfig(VideoStreamConfig, "Resolution=")
                    Dim prevVideoScaleAlgo As String = FindConfig(VideoStreamConfig, "ScaleAlgo=")
                    If Strings.Mid(prevVideoBrCompat, 10) = "" Then
                        ComboBox21.Text = ""
                    Else
                        If RemoveWhitespace(Strings.Mid(prevVideoBrCompat, 26)) = "true" Then
                            ComboBox21.Text = "enable"
                        Else
                            ComboBox21.Text = "disable"
                        End If
                    End If
                    If Strings.Mid(prevVideoOvr, 12) = "" Then
                        BitRate_UpDown.Value = 0
                    Else
                        BitRate_UpDown.Text = CInt(RemoveWhitespace(Strings.Left(Strings.Mid(prevVideoOvr, 18), 1)))
                    End If
                    If Strings.Mid(prevVideoBref, 6) = "" Then
                        ComboBox10.Text = ""
                    Else
                        If RemoveWhitespace(Strings.Mid(prevVideoBref, 18)) = "0" Then
                            ComboBox10.Text = "disabled"
                        ElseIf RemoveWhitespace(Strings.Mid(prevVideoBref, 18)) = "1" Then
                            ComboBox10.Text = "each"
                        ElseIf RemoveWhitespace(Strings.Mid(prevVideoBref, 18)) = "2" Then
                            ComboBox10.Text = "middle"
                        End If
                    End If
                    ComboBox2.Text = Strings.Left(Strings.Mid(prevVideoCodec, 7), 4).ToUpper
                    ComboBox30.Text = Strings.Right(prevVideoFps, 2)
                    If Strings.Mid(prevVideoLvl, 7) = "" Then
                        ComboBox8.Text = ""
                    Else
                        ComboBox8.Text = RemoveWhitespace(Strings.Mid(prevVideoLvl, 14))
                    End If
                    If Strings.Mid(prevVideoMaxBr, 12) = "" Then
                        MaxBitRate_UpDown.Value = 0
                    Else
                        MaxBitRate_UpDown.Text = CInt(RemoveWhitespace(Strings.Left(Strings.Mid(prevVideoMaxBr, 24), 1)))
                    End If
                    If Strings.Mid(prevVideoMp, 11) = "" Then
                        ComboBox14.Text = ""
                    Else
                        If RemoveWhitespace(Strings.Mid(prevVideoMp, 23)) = "0" Then
                            ComboBox14.Text = "1 Pass"
                        ElseIf RemoveWhitespace(Strings.Mid(prevVideoMp, 23)) = "1" Then
                            ComboBox14.Text = "2 Pass (1/4 Resolution)"
                        ElseIf RemoveWhitespace(Strings.Mid(prevVideoMp, 23)) = "2" Then
                            ComboBox14.Text = "2 Pass (Full Resolution)"
                        End If
                    End If
                    Dim tempPreset As String
                    Dim tempTier As String
                    If Strings.Right(Strings.Mid(prevVideoCodec, 7), 3) = "amf" Then
                        If Strings.Mid(prevVideoPreset, 8) = "" Then
                            tempPreset = ""
                            tempTier = ""
                        Else
                            If Strings.Mid(prevVideoPreset, 8) = "" Then
                                tempPreset = ""
                            Else
                                If RemoveWhitespace(Strings.Mid(prevVideoPreset, 18)) = "quality" Then
                                    tempPreset = "slow"
                                ElseIf RemoveWhitespace(Strings.Mid(prevVideoPreset, 18)) = "balanced" Then
                                    tempPreset = "medium"
                                ElseIf RemoveWhitespace(Strings.Mid(prevVideoPreset, 18)) = "speed" Then
                                    tempPreset = "fast"
                                Else
                                    tempPreset = ""
                                End If
                            End If
                            If Strings.Mid(prevVideoTier, 6) = "" Then
                                tempTier = ""
                            Else
                                tempTier = Strings.Mid(prevVideoTier, 21)
                            End If
                        End If
                    Else
                        If Strings.Mid(prevVideoPreset, 8) = "" Then
                            tempPreset = ""
                            tempTier = ""
                        Else
                            If Strings.Mid(prevVideoPreset, 8) = "" Then
                                tempPreset = ""
                            Else
                                tempPreset = Strings.Mid(prevVideoPreset, 17)
                            End If
                            If Strings.Mid(prevVideoTier, 6) = "" Then
                                tempTier = ""
                            Else
                                tempTier = Strings.Mid(prevVideoTier, 13)
                            End If
                        End If
                    End If
                    ComboBox5.Text = tempPreset
                    If Strings.Mid(prevVideoPixFmt, 13) = "" Then
                        ComboBox3.Text = ""
                    Else
                        ComboBox3.Text = Strings.Mid(prevVideoPixFmt, 23)
                    End If
                    If Strings.Mid(prevVideoProfile, 9) = "" Then
                        ComboBox7.Text = ""
                    Else
                        ComboBox7.Text = Strings.Mid(prevVideoProfile, 21)
                    End If
                    If Strings.Mid(prevVideoRateCtr, 13) = "" Then
                        ComboBox4.Text = ""
                    Else
                        If RemoveWhitespace(Strings.Mid(prevVideoRateCtr, 22)) = "vbr" Then
                            ComboBox4.Text = "Variable Bit Rate"
                        ElseIf RemoveWhitespace(Strings.Mid(prevVideoRateCtr, 22)) = "cbr" Then
                            ComboBox4.Text = "Constant Bit Rate"
                        Else
                            ComboBox4.Text = ""
                        End If
                    End If
                    If Strings.Mid(prevVideoSpatialAQ, 11) = "" Then
                        ComboBox11.Text = ""
                    Else
                        If RemoveWhitespace(Strings.Mid(prevVideoSpatialAQ, 23)) = "1" Then
                            ComboBox11.Text = "enable"
                        ElseIf RemoveWhitespace(Strings.Mid(prevVideoSpatialAQ, 23)) = "0" Then
                            ComboBox11.Text = "disable"
                        End If
                    End If
                    If Strings.Mid(prevVideoAQStrength, 12) = "" Then
                        ComboBox12.Text = ""
                    Else
                        ComboBox12.Text = Strings.Mid(prevVideoAQStrength, 26)
                    End If
                    If Strings.Mid(prevVideoTempAQ, 12) = "" Then
                        ComboBox13.Text = ""
                    Else
                        If RemoveWhitespace(Strings.Mid(prevVideoTempAQ, 26)) = "1" Then
                            ComboBox13.Text = "enable"
                        ElseIf RemoveWhitespace(Strings.Mid(prevVideoTempAQ, 26)) = "0" Then
                            ComboBox13.Text = "disable"
                        End If
                    End If
                    If Strings.Mid(prevVideoTargetQL, 10) = "" Then
                        CRF_VBR_UpDown.Text = 0
                    Else
                        CRF_VBR_UpDown.Text = CInt(RemoveWhitespace(Strings.Mid(prevVideoTargetQL, 15)))
                    End If
                    ComboBox9.Text = tempTier
                    If Strings.Mid(prevVideoTune, 6) = "" Then
                        ComboBox6.Text = ""
                    Else
                        If RemoveWhitespace(Strings.Mid(prevVideoTune, 13)) = "hq" Then
                            ComboBox6.Text = "High quality"
                        ElseIf RemoveWhitespace(Strings.Mid(prevVideoTune, 13)) = "ll" Then
                            ComboBox6.Text = "Low latency"
                        ElseIf RemoveWhitespace(Strings.Mid(prevVideoTune, 13)) = "ull" Then
                            ComboBox6.Text = "Ultra low latency"
                        ElseIf RemoveWhitespace(Strings.Mid(prevVideoTune, 13)) = "lossless" Then
                            ComboBox6.Text = "Lossless"
                        End If
                    End If
                    If Strings.Mid(prevVideoResolution, 11) = "x" Then
                        Resolution_Width_UpDown.Value = 0
                        Resolution_Height_UpDown.Value = 0
                    Else
                        Resolution_Width_UpDown.Value = CInt(RemoveWhitespace(getBetween(prevVideoResolution, "n=", "x")))
                        Resolution_Height_UpDown.Value = CInt(RemoveWhitespace(getBetween(prevVideoResolution, "x", "|")))
                    End If
                    If Strings.Mid(prevVideoAspectRatio, 13) = "" Then
                        ComboBox32.Text = ""
                    Else
                        ComboBox32.Text = Strings.Mid(getBetween(prevVideoAspectRatio, "r=", "/"), 5) & ":" & getBetween(prevVideoAspectRatio, "/", ",")
                    End If
                    If Strings.Mid(prevVideoScaleAlgo, 11) = "" Or Strings.Mid(prevVideoScaleAlgo, 11) = "disabled" Then
                        ComboBox35.Text = ""
                    Else
                        ComboBox35.Text = Strings.Mid(prevVideoScaleAlgo, 11)
                    End If
                    RichTextBox1.Text = ""
                    RichTextBox1.Text = File.ReadAllText(VideoStreamFlags)
                End If
                Button15.Enabled = False
                Button16.Enabled = True
            Else
                Button15.Enabled = True
                Button16.Enabled = False
            End If
        End If
    End Sub
    Private Sub VideoCodecCheck(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Dim codecChange As DialogResult = MessageBoxAdv.Show(Me, "Warning !" & vbCrLf & vbCrLf & "Change video will reset current configuration" &
                                                               vbCrLf & "Please save configuration on 'save config stream' options" &
                                                               vbCrLf & vbCrLf & "Change codec ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If codecChange = DialogResult.Yes Then
            If Hwdefconfig = "GPU Engine:" Then
                HwAccelDev = ""
            Else
                HwAccelFormat = "-hwaccel_output_format " & Hwdefconfig.Remove(0, 11)
                HwAccelDev = Hwdefconfig.Remove(0, 11)
            End If
            If ComboBox2.Text = "Copy" Then
                BitRate_UpDown.Enabled = False
                ComboBox21.Enabled = False
                ComboBox10.Enabled = False
                ComboBox30.Enabled = False
                ComboBox8.Enabled = False
                MaxBitRate_UpDown.Enabled = False
                ComboBox14.Enabled = False
                ComboBox5.Enabled = False
                ComboBox3.Enabled = False
                ComboBox7.Enabled = False
                ComboBox4.Enabled = False
                ComboBox11.Enabled = False
                ComboBox12.Enabled = False
                ComboBox13.Enabled = False
                CRF_VBR_UpDown.Enabled = False
                ComboBox6.Enabled = False
                ComboBox9.Enabled = False
                BitRate_UpDown.Value = 0
                ComboBox21.SelectedIndex = -1
                ComboBox10.SelectedIndex = -1
                ComboBox30.SelectedIndex = -1
                ComboBox8.SelectedIndex = -1
                MaxBitRate_UpDown.Value = 0
                ComboBox14.SelectedIndex = -1
                ComboBox5.SelectedIndex = -1
                ComboBox3.SelectedIndex = -1
                ComboBox7.SelectedIndex = -1
                ComboBox4.SelectedIndex = -1
                ComboBox11.SelectedIndex = -1
                ComboBox12.SelectedIndex = -1
                ComboBox13.SelectedIndex = -1
                CRF_VBR_UpDown.Value = 0
                ComboBox6.SelectedIndex = -1
                ComboBox9.SelectedIndex = -1
                ComboBox32.SelectedIndex = -1
                ComboBox32.Enabled = False
                Resolution_Height_UpDown.Value = 0
                Resolution_Height_UpDown.Enabled = False
                Resolution_Width_UpDown.Value = 0
                Resolution_Width_UpDown.Enabled = False
                ComboBox35.SelectedIndex = -1
                ComboBox35.Enabled = False
            ElseIf HwAccelDev = "opencl" Then
                ComboBox21.Enabled = False
                ComboBox10.Enabled = False
                ComboBox14.Enabled = False
                ComboBox3.Text = "yuv420p"
                ComboBox3.Enabled = False
                If ComboBox2.Text = "HEVC" Then
                    ComboBox7.Text = "main"
                    ComboBox7.Enabled = False
                    ComboBox9.Enabled = True
                Else
                    ComboBox7.Enabled = True
                    If ComboBox7.Items.Contains("main10") Then
                        ComboBox7.Items.Remove("main10")
                    End If
                    ComboBox9.Enabled = False
                End If
                ComboBox4.Enabled = False
                ComboBox11.Enabled = False
                ComboBox12.Enabled = False
                ComboBox13.Enabled = False
                ComboBox6.Enabled = False
                CRF_VBR_UpDown.Enabled = False
                BitRate_UpDown.Enabled = True
                ComboBox30.Enabled = True
                ComboBox8.Enabled = True
                MaxBitRate_UpDown.Enabled = True
                ComboBox5.Enabled = True
                ComboBox21.SelectedIndex = -1
                ComboBox10.SelectedIndex = -1
                ComboBox4.SelectedIndex = -1
                ComboBox11.SelectedIndex = -1
                ComboBox12.SelectedIndex = -1
                ComboBox13.SelectedIndex = -1
                CRF_VBR_UpDown.Value = 0
                ComboBox6.SelectedIndex = -1
                ComboBox32.SelectedIndex = -1
                Resolution_Height_UpDown.Value = 0
                Resolution_Width_UpDown.Value = 0
                ComboBox32.Enabled = True
                Resolution_Height_UpDown.Enabled = True
                Resolution_Width_UpDown.Enabled = True
                ComboBox35.SelectedIndex = -1
                ComboBox35.Enabled = True
            ElseIf HwAccelDev = "qsv" Then
                ComboBox21.Enabled = False
                ComboBox10.Enabled = False
                ComboBox14.Enabled = False
                If ComboBox2.Text = "HEVC" Then
                    If ComboBox7.Items.Contains("high") Then
                        ComboBox7.Items.Remove("high")
                    End If
                    If ComboBox7.Items.Contains("main10") = False Then
                        ComboBox7.Items.Add("main10")
                    End If
                Else
                    If ComboBox7.Items.Contains("main10") Then
                        ComboBox7.Items.Remove("main10")
                    End If
                    ComboBox9.Enabled = False
                End If
                ComboBox3.Text = "p010le"
                ComboBox3.Enabled = False
                ComboBox4.Enabled = False
                ComboBox11.Enabled = False
                ComboBox12.Enabled = False
                ComboBox13.Enabled = False
                ComboBox6.Enabled = False
                CRF_VBR_UpDown.Enabled = False
                BitRate_UpDown.Enabled = False
                ComboBox30.Enabled = True
                ComboBox8.Enabled = False
                MaxBitRate_UpDown.Enabled = True
                ComboBox5.Enabled = True
                ComboBox7.Enabled = True
                ComboBox8.SelectedIndex = -1
                ComboBox9.Enabled = False
                ComboBox9.SelectedIndex = -1
                ComboBox21.SelectedIndex = -1
                ComboBox10.SelectedIndex = -1
                ComboBox4.SelectedIndex = -1
                ComboBox11.SelectedIndex = -1
                ComboBox12.SelectedIndex = -1
                ComboBox13.SelectedIndex = -1
                CRF_VBR_UpDown.Value = 0
                ComboBox6.SelectedIndex = -1
                ComboBox32.SelectedIndex = -1
                Resolution_Height_UpDown.Value = 0
                Resolution_Width_UpDown.Value = 0
                ComboBox32.Enabled = True
                Resolution_Height_UpDown.Enabled = True
                Resolution_Width_UpDown.Enabled = True
                ComboBox35.SelectedIndex = -1
                ComboBox35.Enabled = True
            ElseIf HwAccelDev = "cuda" Then
                BitRate_UpDown.Enabled = True
                ComboBox21.Enabled = True
                ComboBox10.Enabled = True
                ComboBox30.Enabled = True
                ComboBox8.Enabled = True
                MaxBitRate_UpDown.Enabled = True
                ComboBox14.Enabled = True
                ComboBox5.Enabled = True
                ComboBox3.Enabled = True
                ComboBox7.Enabled = True
                If ComboBox2.Text = "HEVC" Then
                    If ComboBox7.Items.Contains("high") Then
                        ComboBox7.Items.Remove("high")
                    End If
                    If ComboBox7.Items.Contains("main10") = False Then
                        ComboBox7.Items.Add("main10")
                    End If
                Else
                    If ComboBox7.Items.Contains("main10") Then
                        ComboBox7.Items.Remove("main10")
                    End If
                    ComboBox9.Enabled = False
                End If
                ComboBox4.Enabled = True
                ComboBox11.Enabled = True
                ComboBox12.Enabled = True
                ComboBox13.Enabled = True
                CRF_VBR_UpDown.Enabled = True
                ComboBox6.Enabled = True
                ComboBox9.Enabled = True
                ComboBox32.SelectedIndex = -1
                Resolution_Height_UpDown.Value = 0
                Resolution_Width_UpDown.Value = 0
                ComboBox32.Enabled = True
                Resolution_Height_UpDown.Enabled = True
                Resolution_Width_UpDown.Enabled = True
                ComboBox35.SelectedIndex = -1
                ComboBox35.Enabled = True
            End If
            CheckBox12.Enabled = True
        Else
            MessageBoxAdv.Show("Abort change video codec !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub VideoStreamInitConfig()
        Dim forceDecision As Boolean = False
        If ComboBox29.SelectedIndex >= 0 Then
            Hwdefconfig = FindConfig("config.ini", "GPU Engine:")
            If Hwdefconfig = "GPU Engine:" Then
                HwAccelFormat = ""
                HwAccelDev = ""
            ElseIf Hwdefconfig = "null" Then
                forceDecision = True
            Else
                HwAccelFormat = "-hwaccel_output_format " & Hwdefconfig.Remove(0, 11)
                HwAccelDev = Hwdefconfig.Remove(0, 11)
            End If
            If ComboBox2.Text = "" Then
                MessageBoxAdv.Show("Video encoder codec can not empty !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ReturnVideoStats = False
                forceDecision = False
            End If
            If forceDecision = True Then
                MessageBoxAdv.Show("Please configure GPU HW Acccelerated on options menu first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ReturnVideoStats = False
            Else
                VideoStreamFlags = VideoStreamFlagsPath & "HME_Video_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
                VideoStreamConfig = VideoStreamConfigPath & "HME_Video_Config_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
                VideoStreamSourceList = (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString
                If ComboBox2.Text.Equals("Copy") = True Then
                    HMEStreamProfileGenerate(VideoStreamFlags, " -c:v:" & VideoStreamSourceList & " copy")
                    HMEVideoStreamConfigGenerate(VideoStreamConfig, "", "", "", "Copy", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                    ReturnVideoStats = True
                Else
                    If ComboBox32.SelectedIndex < 0 Or ComboBox32.SelectedIndex = 5 Then
                        AspectRatio = ""
                    Else
                        AspectRatio = " -filter:v setdar=dar=" & vAspectRatio(ComboBox32.Text) & ","
                    End If
                    If Resolution_Height_UpDown.Value = 0 Then
                        VideoWidth = ""
                    Else
                        If AspectRatio = "" Then
                            VideoWidth = " -filter:v scale=" & Resolution_Width_UpDown.Value & "x"
                        Else
                            VideoWidth = "scale=" & Resolution_Width_UpDown.Value & "x"
                        End If
                    End If
                    If Resolution_Width_UpDown.Value = 0 Then
                        VideoHeight = ""
                    Else
                        If VideoWidth = "" Then
                            VideoHeight = ""
                        Else
                            VideoHeight = Resolution_Height_UpDown.Value
                        End If
                    End If
                    If Resolution_Height_UpDown.Value = 0 And Resolution_Width_UpDown.Value = 0 Then
                        If ComboBox35.Text = "disabled" Or ComboBox35.Text = "" Then
                            ScaleAlgo = ""
                        Else
                            ScaleAlgo = " -filter:v scale=flags=" & ComboBox35.Text
                        End If
                    Else
                        If ComboBox35.Text = "disabled" Or ComboBox35.Text = "" Then
                            ScaleAlgo = ""
                        Else
                            ScaleAlgo = ":flags=" & ComboBox35.Text
                        End If
                    End If
                    If ComboBox30.SelectedIndex < 0 Then
                        FPS = ""
                    Else
                        If AspectRatio = "" And VideoWidth = "" And ScaleAlgo = "" Then
                            FPS = " -filter:v fps=fps=" & ComboBox30.Text
                        Else
                            If ScaleAlgo = "" Then
                                FPS = "fps=fps=" & ComboBox30.Text
                            Else
                                FPS = ",fps=fps=" & ComboBox30.Text
                            End If
                        End If
                    End If
                    If BitRate_UpDown.Value = 0 Then
                        BitRate = ""
                    Else
                        BitRate = " -b:v " & BitRate_UpDown.Text & "M"
                    End If
                    If MaxBitRate_UpDown.Value = 0 Then
                        MaxBitRate = ""
                    Else
                        MaxBitRate = " -maxrate:v " & MaxBitRate_UpDown.Text & "M"
                    End If
                    If HwAccelDev = "opencl" Then
                        If ComboBox2.Text = "H264" Then
                            HMEStreamProfileGenerate(VideoStreamFlags, " -c:v:" & VideoStreamSourceList & " " & vCodec(ComboBox2.Text, HwAccelDev) & vPixFmt(ComboBox3.Text) &
                                                         vPreset(ComboBox5.Text, HwAccelDev) & vProfile(ComboBox7.Text) & vLevel(ComboBox8.Text) & BitRate & MaxBitRate & AspectRatio &
                                                         VideoWidth & VideoHeight & ScaleAlgo & FPS)
                            HMEVideoStreamConfigGenerate(VideoStreamConfig, "", BitRate, "", vCodec(ComboBox2.Text, HwAccelDev), FPS, vLevel(ComboBox8.Text), MaxBitRate, "",
                                                         vPreset(ComboBox5.Text, HwAccelDev), "yuv420p", vProfile(ComboBox7.Text), "", "", "", "", "", "", "", AspectRatio,
                                                         Resolution_Width_UpDown.Value & "x" & Resolution_Height_UpDown.Value & "|", ComboBox35.Text)
                            ReturnVideoStats = True
                        Else
                            HMEStreamProfileGenerate(VideoStreamFlags, " -c:v:" & VideoStreamSourceList & " " & vCodec(ComboBox2.Text, HwAccelDev) & vPixFmt(ComboBox3.Text) &
                                                         vPreset(ComboBox5.Text, HwAccelDev) & vProfile(ComboBox7.Text) & vLevel(ComboBox8.Text) & vTier(ComboBox9.Text, HwAccelDev) &
                                                         BitRate & MaxBitRate & AspectRatio & VideoWidth & VideoHeight & ScaleAlgo & FPS)
                            HMEVideoStreamConfigGenerate(VideoStreamConfig, "", BitRate, "", vCodec(ComboBox2.Text, HwAccelDev), FPS, vLevel(ComboBox8.Text), MaxBitRate, "",
                                                         vPreset(ComboBox5.Text, HwAccelDev), "yuv420p", "main", "", "", "", "", "", vTier(ComboBox9.Text, HwAccelDev), "",
                                                         AspectRatio, Resolution_Width_UpDown.Value & "x" & Resolution_Height_UpDown.Value & "|", ComboBox35.Text)
                            ReturnVideoStats = True
                        End If
                    ElseIf HwAccelDev = "qsv" Then
                        HMEStreamProfileGenerate(VideoStreamFlags, " -c:v:" & VideoStreamSourceList & " " & vCodec(ComboBox2.Text, HwAccelDev) & vPixFmt(ComboBox3.Text) &
                                                     vPreset(ComboBox5.Text, HwAccelDev) & vProfile(ComboBox7.Text) & MaxBitRate & AspectRatio & VideoWidth & VideoHeight & ScaleAlgo & FPS & " -low_power false")
                        HMEVideoStreamConfigGenerate(VideoStreamConfig, "", "", "", vCodec(ComboBox2.Text, HwAccelDev), FPS, "", MaxBitRate, "", vPreset(ComboBox5.Text, HwAccelDev),
                                                     vPixFmt(ComboBox3.Text), vProfile(ComboBox7.Text), "", "", "", "", "", "", "", AspectRatio, Resolution_Width_UpDown.Value & "x" & Resolution_Height_UpDown.Value & "|",
                                                     ComboBox35.Text)
                        ReturnVideoStats = True
                    ElseIf HwAccelDev = "cuda" Then
                        If CRF_VBR_UpDown.Value = 0 Then
                            TargetQualityControl = ""
                        Else
                            TargetQualityControl = " -cq " & CRF_VBR_UpDown.Text
                        End If
                        If ComboBox11.Text = "disable" Then
                            HMEStreamProfileGenerate(VideoStreamFlags, " -c:v:" & VideoStreamSourceList & " " & vCodec(ComboBox2.Text, HwAccelDev) & vPixFmt(ComboBox3.Text) &
                                                         vRateControl(ComboBox4.Text) & TargetQualityControl & vPreset(ComboBox5.Text, HwAccelDev) & vTune(ComboBox6.Text) & vProfile(ComboBox7.Text) &
                                                         vLevel(ComboBox8.Text) & vTier(ComboBox9.Text, HwAccelDev) & vBrcompat(ComboBox21.Text) & BitRate & MaxBitRate & bRefMode(ComboBox10.Text) &
                                                         multiPass(ComboBox14.Text) & AspectRatio & VideoWidth & VideoHeight & ScaleAlgo & FPS)
                            HMEVideoStreamConfigGenerate(VideoStreamConfig, vBrcompat(ComboBox21.Text), BitRate, bRefMode(ComboBox10.Text), vCodec(ComboBox2.Text, HwAccelDev), FPS, vLevel(ComboBox8.Text),
                                                             MaxBitRate, multiPass(ComboBox14.Text), vPreset(ComboBox5.Text, HwAccelDev), vPixFmt(ComboBox3.Text), vProfile(ComboBox7.Text), vRateControl(ComboBox4.Text),
                                                             "", "", "", TargetQualityControl, vTier(ComboBox9.Text, HwAccelDev), vTune(ComboBox6.Text), AspectRatio, Resolution_Width_UpDown.Value & "x" & Resolution_Height_UpDown.Value & "|",
                                                             ComboBox35.Text)
                            ReturnVideoStats = True
                        Else
                            HMEStreamProfileGenerate(VideoStreamFlags, " -c:v:" & VideoStreamSourceList & " " & vCodec(ComboBox2.Text, HwAccelDev) & vPixFmt(ComboBox3.Text) & vRateControl(ComboBox4.Text) &
                                                         TargetQualityControl & vPreset(ComboBox5.Text, HwAccelDev) & vTune(ComboBox6.Text) & vProfile(ComboBox7.Text) & vLevel(ComboBox8.Text) & vTier(ComboBox9.Text, HwAccelDev) &
                                                         vBrcompat(ComboBox21.Text) & BitRate & MaxBitRate & bRefMode(ComboBox10.Text) & vSpaTempAQ(ComboBox11.Text) & vAQStrength(ComboBox12.Text) & vTempAQ(ComboBox13.Text) &
                                                         multiPass(ComboBox14.Text) & AspectRatio & VideoWidth & VideoHeight & ScaleAlgo & FPS)
                            HMEVideoStreamConfigGenerate(VideoStreamConfig, vBrcompat(ComboBox21.Text), BitRate, bRefMode(ComboBox10.Text), vCodec(ComboBox2.Text, HwAccelDev), FPS, vLevel(ComboBox8.Text), MaxBitRate,
                                                             multiPass(ComboBox14.Text), vPreset(ComboBox5.Text, HwAccelDev), vPixFmt(ComboBox3.Text), vProfile(ComboBox7.Text), vRateControl(ComboBox4.Text), vSpaTempAQ(ComboBox11.Text),
                                                             vAQStrength(ComboBox12.Text), vTempAQ(ComboBox13.Text), TargetQualityControl, vTier(ComboBox9.Text, HwAccelDev), vTune(ComboBox6.Text),
                                             AspectRatio, Resolution_Width_UpDown.Value & "x" & Resolution_Height_UpDown.Value & "|", ComboBox35.Text)
                            ReturnVideoStats = True
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub SaveVideoStream_Btn(sender As Object, e As EventArgs) Handles Button15.Click
        If ComboBox29.SelectedIndex >= 0 Then
            VideoStreamFlags = VideoStreamFlagsPath & "HME_Video_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
            VideoStreamConfig = VideoStreamConfigPath & "HME_Video_Config_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
            If File.Exists(VideoStreamFlags) And File.Exists(VideoStreamConfig) Then
                Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Configuration for video with stream #0:" & CInt(Strings.Mid(ComboBox29.Text.ToString, 11)).ToString & "  already exists !" & vbCrLf & vbCrLf &
                                                                      "Want to replace old video stream configuration with new video configuration ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If configResult = DialogResult.Yes Then
                    GC.Collect()
                    GC.WaitForPendingFinalizers()
                    File.Delete(VideoStreamFlags)
                    File.Delete(VideoStreamConfig)
                    VcodecReset()
                    VideoStreamInitConfig()
                    If ReturnVideoStats = False Then
                        MessageBoxAdv.Show("Failed to save video stream configuration with stream #0:" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11))).ToString & " !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Button15.Enabled = True
                        Button16.Enabled = False
                    Else
                        RichTextBox1.Text = File.ReadAllText(VideoStreamFlags)
                        MessageBoxAdv.Show("Configuration for video with stream #0:" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11))).ToString & " saved !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Button15.Enabled = False
                        Button16.Enabled = True
                    End If
                End If
            Else
                VideoStreamInitConfig()
                If ReturnVideoStats = False Then
                    MessageBoxAdv.Show("Failed to save video stream configuration with stream #0:" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11))).ToString & " !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Button15.Enabled = True
                    Button16.Enabled = False
                Else
                    RichTextBox1.Text = File.ReadAllText(VideoStreamFlags)
                    MessageBoxAdv.Show("Configuration for video with stream #0:" & CInt(Strings.Mid(ComboBox29.Text.ToString, 11)).ToString & " saved !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Button15.Enabled = False
                    Button16.Enabled = True
                End If
            End If
        Else
            MessageBoxAdv.Show("Please re-select video stream to save configuration ", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub RemoveVideoStream_Btn(sender As Object, e As EventArgs) Handles Button16.Click
        If ComboBox29.SelectedIndex >= 0 Then
            VideoStreamFlags = VideoStreamFlagsPath & "HME_Video_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
            VideoStreamConfig = VideoStreamConfigPath & "HME_Video_Config_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
            If File.Exists(VideoStreamFlags) And File.Exists(VideoStreamConfig) Then
                Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Remove configuration for this video with stream #0:" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11))).ToString & " ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If configResult = DialogResult.Yes Then
                    GC.Collect()
                    GC.WaitForPendingFinalizers()
                    File.Delete(VideoStreamFlags)
                    File.Delete(VideoStreamConfig)
                    VcodecReset()
                    RichTextBox1.Text = ""
                    MessageBoxAdv.Show("Configuration for video with stream #0:" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11))).ToString & " removed !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Button15.Enabled = True
                    Button16.Enabled = False
                End If
            Else
                MessageBoxAdv.Show("Failed to remove configuration for video with stream #0:" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11))).ToString & " !" & vbCrLf &
                                   vbCrLf & "Video configuration with that stream are not exists !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Button15.Enabled = False
                Button16.Enabled = True
            End If
        Else
            MessageBoxAdv.Show("Please re-select video stream to remove configuration ", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Button15.Enabled = False
            Button16.Enabled = True
        End If
    End Sub
    Private Sub VcodecReset()
        Hwdefconfig = FindConfig("config.ini", "GPU Engine:")
        If Hwdefconfig = "GPU Engine:" Then
            HwAccelFormat = ""
            HwAccelDev = ""
        Else
            HwAccelFormat = "-hwaccel_output_format " & Hwdefconfig.Remove(0, 11)
            HwAccelDev = Hwdefconfig.Remove(0, 11)
        End If
        If ComboBox2.Text = "Copy" Then
            BitRate_UpDown.Enabled = False
            ComboBox21.Enabled = False
            ComboBox10.Enabled = False
            ComboBox30.Enabled = False
            ComboBox8.Enabled = False
            MaxBitRate_UpDown.Enabled = False
            ComboBox14.Enabled = False
            ComboBox5.Enabled = False
            ComboBox3.Enabled = False
            ComboBox7.Enabled = False
            ComboBox4.Enabled = False
            ComboBox11.Enabled = False
            ComboBox12.Enabled = False
            ComboBox13.Enabled = False
            CRF_VBR_UpDown.Enabled = False
            ComboBox6.Enabled = False
            ComboBox9.Enabled = False
            ComboBox32.Enabled = False
            Resolution_Height_UpDown.Enabled = False
            Resolution_Width_UpDown.Enabled = False
            ComboBox35.Enabled = False
        ElseIf HwAccelDev = "opencl" Then
            ComboBox21.Enabled = False
            ComboBox10.Enabled = False
            ComboBox14.Enabled = False
            ComboBox3.Text = "yuv420p"
            ComboBox3.Enabled = False
            If ComboBox2.Text = "HEVC" Then
                ComboBox7.Text = "main"
                ComboBox7.Enabled = False
                ComboBox9.Enabled = True
            Else
                ComboBox7.Enabled = True
                If ComboBox7.Items.Contains("main10") Then
                    ComboBox7.Items.Remove("main10")
                End If
                ComboBox9.Enabled = False
            End If
            ComboBox4.Enabled = False
            ComboBox11.Enabled = False
            ComboBox12.Enabled = False
            ComboBox13.Enabled = False
            ComboBox6.Enabled = False
            CRF_VBR_UpDown.Enabled = False
            BitRate_UpDown.Enabled = True
            ComboBox30.Enabled = True
            ComboBox8.Enabled = True
            MaxBitRate_UpDown.Enabled = True
            ComboBox5.Enabled = True
            ComboBox32.Enabled = True
            Resolution_Height_UpDown.Enabled = True
            Resolution_Width_UpDown.Enabled = True
            ComboBox35.Enabled = True
        ElseIf HwAccelDev = "qsv" Then
            ComboBox21.Enabled = False
            ComboBox10.Enabled = False
            ComboBox14.Enabled = False
            If ComboBox2.Text = "HEVC" Then
                If ComboBox7.Items.Contains("high") Then
                    ComboBox7.Items.Remove("high")
                End If
            Else
                If ComboBox7.Items.Contains("main10") Then
                    ComboBox7.Items.Remove("main10")
                End If
                ComboBox9.Enabled = False
            End If
            ComboBox3.Text = "p010le"
            ComboBox3.Enabled = False
            ComboBox4.Enabled = False
            ComboBox11.Enabled = False
            ComboBox12.Enabled = False
            ComboBox13.Enabled = False
            ComboBox6.Enabled = False
            CRF_VBR_UpDown.Enabled = False
            BitRate_UpDown.Enabled = False
            ComboBox30.Enabled = True
            ComboBox8.Enabled = False
            MaxBitRate_UpDown.Enabled = True
            ComboBox5.Enabled = True
            ComboBox7.Enabled = True
            ComboBox9.Enabled = False
            ComboBox32.Enabled = True
            Resolution_Height_UpDown.Enabled = True
            Resolution_Width_UpDown.Enabled = True
            ComboBox35.Enabled = True
        ElseIf HwAccelDev = "cuda" Then
            BitRate_UpDown.Enabled = True
            ComboBox21.Enabled = True
            ComboBox10.Enabled = True
            ComboBox30.Enabled = True
            ComboBox8.Enabled = True
            MaxBitRate_UpDown.Enabled = True
            ComboBox14.Enabled = True
            ComboBox5.Enabled = True
            ComboBox3.Enabled = True
            ComboBox7.Enabled = True
            If ComboBox2.Text = "HEVC" Then
                If ComboBox7.Items.Contains("high") Then
                    ComboBox7.Items.Remove("high")
                End If
            Else
                If ComboBox7.Items.Contains("main10") Then
                    ComboBox7.Items.Remove("main10")
                End If
                ComboBox9.Enabled = False
            End If
            ComboBox4.Enabled = True
            ComboBox11.Enabled = True
            ComboBox12.Enabled = True
            ComboBox13.Enabled = True
            CRF_VBR_UpDown.Enabled = True
            ComboBox6.Enabled = True
            ComboBox9.Enabled = True
            ComboBox32.Enabled = True
            Resolution_Height_UpDown.Enabled = True
            Resolution_Width_UpDown.Enabled = True
            ComboBox35.Enabled = True
        End If
        If CheckBox1.Checked = False Then
            RichTextBox1.Text = ""
        End If
    End Sub
    Private Sub VideoSpatialAQCheck(sender As Object, e As EventArgs) Handles ComboBox11.SelectedIndexChanged
        If ComboBox11.Text = "disable" Then
            ComboBox12.SelectedIndex = -1
            ComboBox13.SelectedIndex = -1
            ComboBox12.Enabled = False
            ComboBox13.Enabled = False
            If ComboBox6.Items.Contains("Lossless") = False Then
                ComboBox6.Items.Add("Lossless")
            End If
        Else
            MessageBoxAdv.Show("Enabling Adaptive Quantization will remove lossless encoding in tier options !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ComboBox12.Enabled = True
            ComboBox13.Enabled = True
            If ComboBox6.Items.Contains("Lossless") Then
                ComboBox6.Items.Remove("Lossless")
            End If
        End If
    End Sub
    Private Sub Allow_Upscaling(sender As Object, e As EventArgs) Handles CheckBox12.CheckedChanged
        If CheckBox12.Checked = True Then
            Resolution_Width_UpDown.Maximum = 7680
            Resolution_Height_UpDown.Maximum = 4320
        ElseIf CheckBox12.Checked = False Then
            Dim videoRes As String()
            videoRes = Label6.Text.Split("x")
            Resolution_Height_UpDown.Value = 0
            Resolution_Width_UpDown.Value = 0
            Resolution_Width_UpDown.Maximum = videoRes(0)
            Resolution_Height_UpDown.Maximum = videoRes(1)
        End If
    End Sub
    Private Sub Expand_Hide_Btn_Video_Codec(sender As Object, e As EventArgs) Handles Button19.Click
        If Button19.Text = "" Then
            Button19.Text = " "
            While Vid_Options_Pnl.Height <= 98
                Vid_Options_Pnl.Height += 1
            End While
            Vid_Encoder_Opt_Pnl.Location = New Point(10, Vid_Options_Pnl.Location.Y + 100)
            If Button20.Text = "" Then
                Vid_Enc_QC_Pnl.Location = New Point(10, Vid_Encoder_Opt_Pnl.Location.Y + 40)
            ElseIf Button20.Text = " " Then
                Vid_Enc_QC_Pnl.Location = New Point(10, Vid_Encoder_Opt_Pnl.Location.Y + 125)
            End If
            If Button22.Text = "" Then
                AQ_Pnl.Location = New Point(10, Vid_Enc_QC_Pnl.Location.Y + 40)
            ElseIf Button22.Text = " " Then
                AQ_Pnl.Location = New Point(10, Vid_Enc_QC_Pnl.Location.Y + 125)
            End If
            If Button24.Text = "" Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 40)
            ElseIf Button24.Text = " " Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 100)
            End If
            If Button23.Text = "" Then
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 40)
            ElseIf Button23.Text = " " Then
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 125)
            End If
            Button19.BackgroundImage = Image.FromFile(UpBtnPath)
        ElseIf Button19.Text = " " Then
            Button19.Text = ""
            While Vid_Options_Pnl.Height >= 38
                Vid_Options_Pnl.Height -= 1
            End While
            Vid_Encoder_Opt_Pnl.Location = New Point(10, Vid_Options_Pnl.Location.Y + 40)
            If Button20.Text = "" Then
                Vid_Enc_QC_Pnl.Location = New Point(10, Vid_Encoder_Opt_Pnl.Location.Y + 40)
            ElseIf Button20.Text = " " Then
                Vid_Enc_QC_Pnl.Location = New Point(10, Vid_Encoder_Opt_Pnl.Location.Y + 125)
            End If
            If Button22.Text = "" Then
                AQ_Pnl.Location = New Point(10, Vid_Enc_QC_Pnl.Location.Y + 40)
            ElseIf Button22.Text = " " Then
                AQ_Pnl.Location = New Point(10, Vid_Enc_QC_Pnl.Location.Y + 125)
            End If
            If Button24.Text = "" Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 40)
            ElseIf Button24.Text = " " Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 100)
            End If
            If Button23.Text = "" Then
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 40)
            ElseIf Button23.Text = " " Then
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 125)
            End If
            Button19.BackgroundImage = Image.FromFile(DownBtnPath)
        End If
    End Sub
    Private Sub Expand_Hide_Btn_Encoder_Opt_Codec(sender As Object, e As EventArgs) Handles Button20.Click
        If Button20.Text = "" Then
            Button20.Text = " "
            While Vid_Encoder_Opt_Pnl.Height <= 123
                Vid_Encoder_Opt_Pnl.Height += 1
            End While
            Vid_Enc_QC_Pnl.Location = New Point(10, Vid_Encoder_Opt_Pnl.Location.Y + 125)
            If Button22.Text = "" Then
                AQ_Pnl.Location = New Point(10, Vid_Enc_QC_Pnl.Location.Y + 40)
            ElseIf Button22.Text = " " Then
                AQ_Pnl.Location = New Point(10, Vid_Enc_QC_Pnl.Location.Y + 125)
            End If
            If Button24.Text = "" Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 40)
            ElseIf Button24.Text = " " Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 100)
            End If
            If Button23.Text = "" Then
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 40)
            ElseIf Button23.Text = " " Then
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 125)
            End If
            Button20.BackgroundImage = Image.FromFile(UpBtnPath)
        ElseIf Button20.Text = " " Then
            Button20.Text = ""
            While Vid_Encoder_Opt_Pnl.Height >= 38
                Vid_Encoder_Opt_Pnl.Height -= 1
            End While
            Vid_Enc_QC_Pnl.Location = New Point(10, Vid_Encoder_Opt_Pnl.Location.Y + 40)
            If Button22.Text = "" Then
                AQ_Pnl.Location = New Point(10, Vid_Enc_QC_Pnl.Location.Y + 40)
            ElseIf Button22.Text = " " Then
                AQ_Pnl.Location = New Point(10, Vid_Enc_QC_Pnl.Location.Y + 125)
            End If
            If Button24.Text = "" Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 40)
            ElseIf Button24.Text = " " Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 100)
            End If
            If Button23.Text = "" Then
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 40)
            ElseIf Button23.Text = " " Then
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 125)
            End If
            Button20.BackgroundImage = Image.FromFile(DownBtnPath)
        End If
    End Sub
    Private Sub Expand_Hide_Btn_Encoder_QC_Codec(sender As Object, e As EventArgs) Handles Button22.Click
        If Button22.Text = "" Then
            Button22.Text = " "
            While Vid_Enc_QC_Pnl.Height <= 123
                Vid_Enc_QC_Pnl.Height += 1
            End While
            AQ_Pnl.Location = New Point(10, Vid_Enc_QC_Pnl.Location.Y + 125)
            If Button24.Text = "" Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 40)
            ElseIf Button24.Text = " " Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 100)
            End If
            If Button23.Text = "" Then
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 40)
            ElseIf Button23.Text = " " Then
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 125)
            End If
            Button22.BackgroundImage = Image.FromFile(UpBtnPath)
        ElseIf Button22.Text = " " Then
            Button22.Text = ""
            While Vid_Enc_QC_Pnl.Height >= 38
                Vid_Enc_QC_Pnl.Height -= 1
            End While
            AQ_Pnl.Location = New Point(10, Vid_Enc_QC_Pnl.Location.Y + 40)
            If Button24.Text = "" Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 40)
            ElseIf Button24.Text = " " Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 100)
            End If
            If Button23.Text = "" Then
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 40)
            ElseIf Button23.Text = " " Then
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 125)
            End If
            Button22.BackgroundImage = Image.FromFile(DownBtnPath)
        End If
    End Sub
    Private Sub Expand_Hide_Btn_Encoder_Add_Codec(sender As Object, e As EventArgs) Handles Button21.Click
        If Button21.Text = "" Then
            Button21.Text = " "
            While Vid_Enc_Add_Pnl.Height <= 98
                Vid_Enc_Add_Pnl.Height += 1
            End While
            Button21.BackgroundImage = Image.FromFile(UpBtnPath)
        ElseIf Button21.Text = " " Then
            Button21.Text = ""
            While Vid_Enc_Add_Pnl.Height >= 38
                Vid_Enc_Add_Pnl.Height -= 1
            End While
            Button21.BackgroundImage = Image.FromFile(DownBtnPath)
        End If
    End Sub
    Private Sub Expand_Hide_Btn_Asp_Ratio(sender As Object, e As EventArgs) Handles Button23.Click
        If Button23.Text = "" Then
            Button23.Text = " "
            While Asp_Res_Pnl.Height <= 123
                Asp_Res_Pnl.Height += 1
            End While
            Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 125)
            Button23.BackgroundImage = Image.FromFile(UpBtnPath)
        ElseIf Button23.Text = " " Then
            Button23.Text = ""
            While Asp_Res_Pnl.Height >= 38
                Asp_Res_Pnl.Height -= 1
            End While
            Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 40)
            Button23.BackgroundImage = Image.FromFile(DownBtnPath)
        End If
    End Sub
    Private Sub Expand_Hide_Btn_AQ(sender As Object, e As EventArgs) Handles Button24.Click
        If Button24.Text = "" Then
            Button24.Text = " "
            While AQ_Pnl.Height <= 98
                AQ_Pnl.Height += 1
            End While
            If Button23.Text = "" Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 100)
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 40)
            ElseIf Button23.Text = " " Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 100)
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 125)
            End If
            Button24.BackgroundImage = Image.FromFile(UpBtnPath)
        ElseIf Button24.Text = " " Then
            Button24.Text = ""
            While AQ_Pnl.Height >= 38
                AQ_Pnl.Height -= 1
            End While
            If Button23.Text = "" Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 40)
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 40)
            ElseIf Button23.Text = " " Then
                Asp_Res_Pnl.Location = New Point(10, AQ_Pnl.Location.Y + 40)
                Vid_Enc_Add_Pnl.Location = New Point(10, Asp_Res_Pnl.Location.Y + 125)
            End If
            Button24.BackgroundImage = Image.FromFile(DownBtnPath)
        End If
    End Sub
    Private Sub LockProfileVideoCheck(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = True Then
            If TextBox1.Text = "" Then
                MessageBoxAdv.Show("Please choose save media file first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CheckBox3.Checked = False
                ComboBox2.Enabled = True
            Else
                FlagsCount = ComboBox29.Items.Count
                FlagsResult = 0
                FlagsValue = 0
                For FlagsStart = 1 To FlagsCount
                    If File.Exists(VideoStreamFlagsPath & "HME_Video_" & (FlagsStart - 1).ToString & ".txt") Then
                        FlagsResult += 1
                    Else
                        MissedFlags(FlagsStart) = FlagsStart
                    End If
                    FlagsValue += 1
                Next
                If FlagsResult = FlagsCount Then
                    BitRate_UpDown.Enabled = False
                    ComboBox21.Enabled = False
                    ComboBox10.Enabled = False
                    ComboBox2.Enabled = False
                    ComboBox30.Enabled = False
                    ComboBox8.Enabled = False
                    MaxBitRate_UpDown.Enabled = False
                    ComboBox14.Enabled = False
                    ComboBox5.Enabled = False
                    ComboBox3.Enabled = False
                    ComboBox7.Enabled = False
                    ComboBox4.Enabled = False
                    ComboBox11.Enabled = False
                    ComboBox12.Enabled = False
                    ComboBox13.Enabled = False
                    CRF_VBR_UpDown.Enabled = False
                    ComboBox6.Enabled = False
                    ComboBox9.Enabled = False
                    ComboBox29.Enabled = False
                    Button15.Enabled = False
                    Button16.Enabled = False
                    ComboBox32.Enabled = False
                    ComboBox35.Enabled = False
                    Resolution_Height_UpDown.Enabled = False
                    Resolution_Width_UpDown.Enabled = False
                    Label28.Text = "READY"
                Else
                    For FlagsStart = 1 To FlagsValue
                        If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                            MessageBoxAdv.Show("Please save configuration for video stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    Next
                    CheckBox3.Checked = False
                End If
            End If
        Else
            VcodecReset()
            ComboBox2.Enabled = True
            ComboBox29.Enabled = True
            Button15.Enabled = True
            Button16.Enabled = True
        End If
    End Sub
    Private Sub EnableAudioCheck(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked = True Then
            If Label2.Text IsNot "" Or CheckBox8.Checked And TextBox15.Text IsNot "" Then
                If Label44.Text.Equals("Not Detected") = True And TextBox16.Text Is "" Then
                    CheckBox4.Checked = False
                    MessageBoxAdv.Show("Current media file does not contain any audio stream !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ComboBox15.Enabled = True
                    CheckBox5.Enabled = True
                    ComboBox22.Enabled = True
                    Button17.Enabled = True
                    Button18.Enabled = True
                    ComboBox22.SelectedIndex = 0
                End If
            Else
                CheckBox4.Checked = False
                MessageBoxAdv.Show("Please choose media file source first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            CheckBox5.Enabled = False
            CheckBox5.Checked = False
            ComboBox15.Enabled = False
            ComboBox15.SelectedIndex = -1
            ComboBox16.Enabled = False
            ComboBox16.SelectedIndex = -1
            ComboBox17.Enabled = False
            ComboBox17.SelectedIndex = -1
            ComboBox18.Enabled = False
            ComboBox18.SelectedIndex = -1
            ComboBox19.Enabled = False
            ComboBox19.SelectedIndex = -1
            ComboBox20.Enabled = False
            ComboBox20.SelectedIndex = -1
            ComboBox33.Enabled = False
            ComboBox33.SelectedIndex = -1
            ComboBox34.Enabled = False
            ComboBox34.SelectedIndex = -1
            ComboBox22.Enabled = False
            ComboBox22.SelectedIndex = -1
            Button17.Enabled = False
            Button18.Enabled = False
            RichTextBox2.Text = ""
        End If
    End Sub
    Private Sub AudioStream_Source(sender As Object, e As EventArgs) Handles ComboBox22.SelectedIndexChanged
        If ComboBox22.SelectedIndex >= 0 Then
            AudiostreamFlags = AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            AudiostreamConfig = AudioStreamConfigPath & "HME_Audio_Config_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            If File.Exists(AudiostreamFlags) And File.Exists(AudiostreamConfig) Then
                Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Configuration for audio with stream #0:" & CInt(Strings.Mid(ComboBox22.Text.ToString, 11)).ToString & "  already exists !" & vbCrLf & vbCrLf &
                                                                      "Check old audio stream configuration ?" & vbCrLf & "NOTE: This will replace existing audio configuration", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If configResult = DialogResult.Yes Then
                    AcodecReset()
                    Dim prevAudioCodec As String = FindConfig(AudiostreamConfig, "Codec=")
                    Dim prevAudioBitDepth As String = FindConfig(AudiostreamConfig, "BitDepth=")
                    Dim prevAudioRateControl As String = FindConfig(AudiostreamConfig, "RateControl=")
                    Dim prevAudioRate As String = FindConfig(AudiostreamConfig, "Rate=")
                    Dim prevAudioChannel As String = FindConfig(AudiostreamConfig, "Channel=")
                    Dim prevAudioChannelLayout As String = FindConfig(AudiostreamConfig, "ChannelLayout=")
                    Dim prevAudioCompLvl As String = FindConfig(AudiostreamConfig, "Compression=")
                    Dim prevAudioFreq As String = FindConfig(AudiostreamConfig, "Frequency=")
                    RichTextBox2.Text = ""
                    RichTextBox2.Text = File.ReadAllText(AudiostreamFlags)
                    ComboBox15.Text = aCodecReverse(Strings.Mid(prevAudioCodec, 15))
                    If Strings.Mid(prevAudioBitDepth, 10) = "pcm_s16le" Then
                        ComboBox18.Text = "16 Bit"
                    ElseIf Strings.Mid(prevAudioBitDepth, 10) = "pcm_s24le" Then
                        ComboBox18.Text = "24 Bit"
                    ElseIf Strings.Mid(prevAudioBitDepth, 10) = "pcm_s32le" Then
                        ComboBox18.Text = "32 Bit"
                    ElseIf Strings.Mid(prevAudioBitDepth, 10) = "s16" Then
                        ComboBox18.Text = "16 Bit"
                    ElseIf Strings.Mid(prevAudioBitDepth, 10) = "s32" Then
                        ComboBox18.Text = "24 Bit"
                    Else
                        ComboBox18.Text = Strings.Mid(prevAudioBitDepth, 10)
                    End If
                    ComboBox20.Text = Strings.Mid(prevAudioRateControl, 13)
                    ComboBox19.Text = Strings.Mid(prevAudioRate, 6)
                    ComboBox33.Text = Strings.Mid(prevAudioChannel, 9)
                    ComboBox34.Text = Strings.Mid(prevAudioChannelLayout, 15)
                    ComboBox17.Text = Strings.Mid(prevAudioCompLvl, 13)
                    ComboBox16.Text = Strings.Mid(prevAudioFreq, 11)
                End If
                Button17.Enabled = False
                Button18.Enabled = True
            Else
                Button17.Enabled = True
                Button18.Enabled = False
            End If
        End If
    End Sub
    Private Sub AudioCodecCheck(sender As Object, e As EventArgs) Handles ComboBox15.SelectedIndexChanged
        Dim codecChange As DialogResult = MessageBoxAdv.Show(Me, "Warning !" & vbCrLf & vbCrLf & "Change audio codec will reset current configuration" &
                                                               vbCrLf & "Change audio codec ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If codecChange = DialogResult.Yes Then
            AudioBitDepthCheck()
            AudioFrequencyCheck()
            If ComboBox15.Text = "WAV" Then
                ComboBox16.Enabled = True
                ComboBox16.SelectedIndex = -1
                ComboBox17.Enabled = False
                ComboBox17.SelectedIndex = -1
                ComboBox18.Enabled = True
                ComboBox18.SelectedIndex = -1
                ComboBox19.Enabled = False
                ComboBox19.SelectedIndex = -1
                ComboBox20.Enabled = False
                ComboBox20.SelectedIndex = -1
                ComboBox33.Enabled = True
                ComboBox33.SelectedIndex = -1
                ComboBox34.Enabled = False
                ComboBox34.SelectedIndex = -1
            ElseIf ComboBox15.Text = "Copy" Then
                ComboBox16.Enabled = False
                ComboBox16.SelectedIndex = -1
                ComboBox17.Enabled = False
                ComboBox17.SelectedIndex = -1
                ComboBox18.Enabled = False
                ComboBox18.SelectedIndex = -1
                ComboBox19.Enabled = False
                ComboBox19.SelectedIndex = -1
                ComboBox20.Enabled = False
                ComboBox20.SelectedIndex = -1
                ComboBox33.Enabled = False
                ComboBox33.SelectedIndex = -1
                ComboBox34.Enabled = False
                ComboBox34.SelectedIndex = -1
            ElseIf ComboBox15.Text = "MP3" Or ComboBox15.Text = "AAC" Then
                ComboBox16.Enabled = True
                ComboBox16.SelectedIndex = -1
                ComboBox33.Enabled = True
                ComboBox33.SelectedIndex = -1
                ComboBox34.Enabled = False
                ComboBox34.SelectedIndex = -1
                ComboBox17.Enabled = False
                ComboBox17.SelectedIndex = -1
                ComboBox18.Enabled = False
                ComboBox18.SelectedIndex = -1
                ComboBox19.Enabled = False
                ComboBox19.SelectedIndex = -1
                ComboBox20.Enabled = True
                ComboBox20.SelectedIndex = -1
            ElseIf ComboBox15.Text = "FLAC" Then
                ComboBox16.Enabled = True
                ComboBox16.SelectedIndex = -1
                ComboBox17.Enabled = True
                ComboBox17.SelectedIndex = -1
                ComboBox18.Enabled = True
                ComboBox18.SelectedIndex = -1
                ComboBox19.Enabled = False
                ComboBox19.SelectedIndex = -1
                ComboBox20.Enabled = False
                ComboBox20.SelectedIndex = -1
                ComboBox33.Enabled = True
                ComboBox33.SelectedIndex = -1
                ComboBox34.Enabled = False
                ComboBox34.Text = ""
            End If
        End If
    End Sub
    Private Sub SaveAudioStream_Btn(sender As Object, e As EventArgs) Handles Button17.Click
        If ComboBox22.SelectedIndex >= 0 Then
            AudiostreamFlags = AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            AudiostreamConfig = AudioStreamConfigPath & "HME_Audio_Config_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            If File.Exists(AudiostreamFlags) And File.Exists(AudiostreamConfig) Then
                Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Configuration for audio with stream #0:" & CInt(Strings.Mid(ComboBox29.Text.ToString, 11)).ToString & "  already exists !" & vbCrLf & vbCrLf &
                                                                      "Want to replace old audio stream configuration with new audio configuration ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If configResult = DialogResult.Yes Then
                    GC.Collect()
                    GC.WaitForPendingFinalizers()
                    File.Delete(AudiostreamFlags)
                    File.Delete(AudiostreamConfig)
                    AcodecReset()
                    AudioStreamInitConfig()
                    If ReturnAudioStats = False Then
                        MessageBoxAdv.Show("Failed to save audio stream configuration with stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString & " !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Button17.Enabled = True
                        Button18.Enabled = False
                    Else
                        RichTextBox2.Text = File.ReadAllText(AudiostreamFlags)
                        MessageBoxAdv.Show("Configuration for audio with stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString & " saved !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Button17.Enabled = False
                        Button18.Enabled = True
                    End If
                End If
            Else
                AudioStreamInitConfig()
                If ReturnAudioStats = False Then
                    MessageBoxAdv.Show("Failed to save audio stream configuration with stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString & " !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Button17.Enabled = True
                    Button18.Enabled = False
                Else
                    RichTextBox2.Text = File.ReadAllText(AudiostreamFlags)
                    MessageBoxAdv.Show("Configuration for audio with stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString & " saved !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Button17.Enabled = False
                    Button18.Enabled = True
                End If
            End If
        Else
            MessageBoxAdv.Show("Please re-select audio stream to save configuration ", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub RemoveAudioStream_Btn(sender As Object, e As EventArgs) Handles Button18.Click
        If ComboBox22.SelectedIndex >= 0 Then
            AudiostreamFlags = AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            AudiostreamConfig = AudioStreamConfigPath & "HME_Audio_Config_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            If File.Exists(AudiostreamFlags) And File.Exists(AudiostreamConfig) Then
                Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Want to remove configuration for this stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString & " ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If configResult = DialogResult.Yes Then
                    GC.Collect()
                    GC.WaitForPendingFinalizers()
                    File.Delete(AudiostreamFlags)
                    File.Delete(AudiostreamConfig)
                    AcodecReset()
                    RichTextBox2.Text = ""
                    MessageBoxAdv.Show("Configuration for audio with stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString & " removed !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Button17.Enabled = True
                    Button18.Enabled = False
                Else
                    MessageBoxAdv.Show("Failed to remove configuration for audio with stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString & " !" & vbCrLf &
                                   vbCrLf & "Audio configuration with that stream are not exists !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Button17.Enabled = False
                    Button18.Enabled = True
                End If
            End If
        Else
            MessageBoxAdv.Show("Please re-select audio stream to remove configuration ", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Button17.Enabled = True
            Button18.Enabled = False
        End If
    End Sub
    Private Sub AudioStreamInitConfig()
        If ComboBox22.SelectedIndex >= 0 Then
            AudiostreamFlags = AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            AudiostreamConfig = AudioStreamConfigPath & "HME_Audio_Config_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            AudioStreamSourceList = (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString
            Dim channel_layout As String
            If ComboBox34.Text = "" Then
                channel_layout = ""
            Else
                channel_layout = " -filter:a:" & AudioStreamSourceList & " aformat=channel_layouts=" & ComboBox34.Text
            End If
            If ComboBox15.Text = "MP3" Or ComboBox15.Text = "AAC" Then
                If ComboBox20.Text = "CBR" Then
                    HMEStreamProfileGenerate(AudiostreamFlags, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList) & aChannel(ComboBox33.Text, AudioStreamSourceList) &
                                                     aBitRate(ComboBox19.Text, AudioStreamSourceList, "MP3", "CBR") & aSampleRate(ComboBox16.Text, AudioStreamSourceList) & channel_layout)
                    HMEAudioStreamConfigGenerate(AudiostreamConfig, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList), "", "CBR", ComboBox19.Text, ComboBox33.Text, "", ComboBox16.Text, ComboBox34.Text)
                    ReturnAudioStats = True
                ElseIf ComboBox20.Text = "VBR" Then
                    If ComboBox15.Text = "MP3" Then
                        HMEStreamProfileGenerate(AudiostreamFlags, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList) & aChannel(ComboBox33.Text, AudioStreamSourceList) &
                                                         aBitRate(ComboBox17.Text, AudioStreamSourceList, "MP3", "VBR") & aSampleRate(ComboBox16.Text, AudioStreamSourceList) & channel_layout)
                    ElseIf ComboBox15.Text = "AAC" Then
                        HMEStreamProfileGenerate(AudiostreamFlags, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList) & aChannel(ComboBox33.Text, AudioStreamSourceList) &
                                                         aBitRate(ComboBox17.Text, AudioStreamSourceList, "AAC", "VBR") & aSampleRate(ComboBox16.Text, AudioStreamSourceList) & channel_layout)
                    End If
                    HMEAudioStreamConfigGenerate(AudiostreamConfig, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList), "", "VBR", ComboBox19.Text, ComboBox33.Text, ComboBox17.Text, ComboBox16.Text, ComboBox34.Text)
                    ReturnAudioStats = True
                Else
                    HMEStreamProfileGenerate(AudiostreamFlags, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList) & aChannel(ComboBox33.Text, AudioStreamSourceList) &
                                                         aBitRate(ComboBox17.Text, AudioStreamSourceList, "", "") & aSampleRate(ComboBox16.Text, AudioStreamSourceList) & channel_layout)
                    ReturnAudioStats = True
                End If
            ElseIf ComboBox15.Text = "FLAC" Then
                ComboBox19.SelectedIndex = -1
                HMEStreamProfileGenerate(AudiostreamFlags, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList) & aChannel(ComboBox33.Text, AudioStreamSourceList) &
                                             aBitRate(ComboBox17.Text, AudioStreamSourceList, "FLAC", "") & aSampleRate(ComboBox16.Text, AudioStreamSourceList) &
                                             aBitDepth(ComboBox15.Text, AudioStreamSourceList, ComboBox18.Text) & channel_layout)
                HMEAudioStreamConfigGenerate(AudiostreamConfig, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList), aBitDepth(ComboBox15.Text, AudioStreamSourceList, ComboBox18.Text), "", "",
                                                ComboBox33.Text, ComboBox17.Text, ComboBox16.Text, ComboBox34.Text)
                ReturnAudioStats = True
            ElseIf ComboBox15.Text = "WAV" Then
                HMEStreamProfileGenerate(AudiostreamFlags, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList) & aChannel(ComboBox33.Text, AudioStreamSourceList) &
                                             aSampleRate(ComboBox16.Text, AudioStreamSourceList) & channel_layout)
                HMEAudioStreamConfigGenerate(AudiostreamConfig, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList), "", "", "", ComboBox33.Text, "", ComboBox16.Text, ComboBox34.Text)
                ReturnAudioStats = True
            ElseIf ComboBox15.Text = "Copy" Then
                HMEStreamProfileGenerate(AudiostreamFlags, " -c:a:" & AudioStreamSourceList & " copy")
                HMEAudioStreamConfigGenerate(AudiostreamConfig, "copy", "", "", "", "", "", "", "")
                ReturnAudioStats = True
            ElseIf ComboBox15.Text Is "" Then
                MessageBoxAdv.Show("Please fill audio codecs !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ReturnAudioStats = False
            End If
            If CheckBox1.Checked = False Then
                Dim getCurrentCodec As String = ComboBox15.Text.ToLower
                If getCurrentCodec = "flac" Then
                    If getCurrentCodec = OrigSaveExt Then

                    Else
                        TextBox1.Text = OrigSavePath & "\" & OrigSaveName & "." & getCurrentCodec
                    End If
                ElseIf getCurrentCodec = "mp3" Or getCurrentCodec = "wav" Then
                    If getCurrentCodec = OrigSaveExt Then

                    Else
                        TextBox1.Text = OrigSavePath & "\" & OrigSaveName & "." & getCurrentCodec
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub AcodecReset()
        AudioBitDepthCheck()
        AudioFrequencyCheck()
        If ComboBox15.Text = "WAV" Then
            ComboBox16.Enabled = True
            ComboBox17.Enabled = False
            ComboBox18.Enabled = True
            ComboBox19.Enabled = False
            ComboBox20.Enabled = False
            ComboBox33.Enabled = True
        ElseIf ComboBox15.Text = "Copy" Then
            ComboBox16.Enabled = False
            ComboBox17.Enabled = False
            ComboBox18.Enabled = False
            ComboBox19.Enabled = False
            ComboBox20.Enabled = False
            ComboBox33.Enabled = False
        ElseIf ComboBox15.Text = "MP3" Or ComboBox15.Text = "AAC" Then
            ComboBox16.Enabled = True
            ComboBox33.Enabled = True
            ComboBox17.Enabled = False
            ComboBox18.Enabled = False
            ComboBox19.Enabled = True
            ComboBox20.Enabled = True
        ElseIf ComboBox15.Text = "FLAC" Then
            ComboBox16.Enabled = True
            ComboBox17.Enabled = True
            ComboBox18.Enabled = True
            ComboBox19.Enabled = False
            ComboBox20.Enabled = False
            ComboBox33.Enabled = True
        End If
        If CheckBox4.Checked = False Then
            RichTextBox2.Text = ""
        End If
        ComboBox15.Enabled = True
    End Sub
    Private Sub AudioChannel_Init(sender As Object, e As EventArgs) Handles ComboBox33.SelectedIndexChanged
        ComboBox34.Items.Clear()
        ComboBox34.Enabled = True
        ComboBox34.Text = ""
        If ComboBox33.Text = "1" Then
            ComboBox34.Items.Add("mono")
        ElseIf ComboBox33.Text = "2" Then
            ComboBox34.Items.Add("stereo")
            ComboBox34.Items.Add("2.1")
        ElseIf ComboBox33.Text = "3" Then
            ComboBox34.Items.Add("3.0")
            ComboBox34.Items.Add("3.1")
        ElseIf ComboBox33.Text = "4" Then
            ComboBox34.Items.Add("4.0")
            ComboBox34.Items.Add("4.1")
            ComboBox34.Items.Add("quad")
        ElseIf ComboBox33.Text = "5" Then
            ComboBox34.Items.Add("5.0")
            ComboBox34.Items.Add("5.1")
        Else
            ComboBox34.Items.Add("mono")
            ComboBox34.Items.Add("stereo")
        End If
    End Sub
    Private Sub AudioFrequencyCheck()
        If ComboBox16.Items.Contains("64000") = True AndAlso ComboBox16.Items.Contains("88200") = True AndAlso ComboBox16.Items.Contains("96000") = True AndAlso
               ComboBox16.Items.Contains("176400") = True AndAlso ComboBox16.Items.Contains("192000") = True Then
            If ComboBox15.Text.Equals("MP3") = True Or ComboBox15.Text.Equals("AAC") = True Then
                ComboBox16.Items.Remove("64000")
                ComboBox16.Items.Remove("88200")
                ComboBox16.Items.Remove("96000")
                ComboBox16.Items.Remove("176400")
                ComboBox16.Items.Remove("192000")
            End If
        Else
            If ComboBox16.Items.Contains("64000") = False AndAlso ComboBox16.Items.Contains("88200") = False AndAlso
                    ComboBox16.Items.Contains("96000") = False AndAlso ComboBox16.Items.Contains("176400") = False AndAlso
                    ComboBox16.Items.Contains("192000") = False Then
                If ComboBox15.Text.Contains("MP3") = False Or ComboBox15.Text.Contains("AAC") = False Then
                    ComboBox16.Items.Add("64000")
                    ComboBox16.Items.Add("88200")
                    ComboBox16.Items.Add("96000")
                    ComboBox16.Items.Add("176400")
                    ComboBox16.Items.Add("192000")
                End If
            End If
        End If
    End Sub
    Private Sub AudioBitDepthCheck()
        If ComboBox18.Items.Contains("32 Bit") = True Then
            If ComboBox15.Text = "FLAC" Then
                ComboBox18.Items.Remove("32 Bit")
            End If
        Else
            If ComboBox15.Text = "FLAC" Then
            Else
                ComboBox18.Items.Add("32 Bit")
            End If
        End If
    End Sub
    Private Sub AudioLossyBitRateCheck(sender As Object, e As EventArgs) Handles ComboBox20.SelectedIndexChanged
        If ComboBox15.Text.Equals("MP3") = True Or ComboBox15.Text.Equals("AAC") = True Then
            If ComboBox20.Text.Equals("CBR") = True Then
                ComboBox19.Enabled = True
                ComboBox17.Enabled = False
                ComboBox17.SelectedIndex = -1
                If ComboBox19.Items.Contains("320") Then
                    If ComboBox15.Text.Contains("AAC") = True Then
                        ComboBox19.Items.Remove("320")
                    End If
                Else
                    If ComboBox15.Text.Contains("AAC") = False Then
                        ComboBox19.Items.Clear()
                        ComboBox19.Items.Add("320")
                        ComboBox19.Items.Add("256")
                        ComboBox19.Items.Add("192")
                        ComboBox19.Items.Add("128")
                    End If
                End If
            ElseIf ComboBox20.Text.Equals("VBR") = True Then
                ComboBox19.Enabled = False
                ComboBox19.SelectedIndex = -1
                ComboBox17.Enabled = True
                If ComboBox17.Items.Contains("0") = True AndAlso ComboBox17.Items.Contains("6") = True AndAlso ComboBox17.Items.Contains("7") = True AndAlso
                        ComboBox17.Items.Contains("8") = True Then
                    If ComboBox15.Text.Equals("AAC") = True Then
                        ComboBox17.Items.Remove("0")
                        ComboBox17.Items.Remove("6")
                        ComboBox17.Items.Remove("7")
                        ComboBox17.Items.Remove("8")
                    End If
                Else
                    If ComboBox15.Text.Equals("AAC") = False Then
                        ComboBox17.Items.Clear()
                        ComboBox17.Items.Add("0")
                        ComboBox17.Items.Add("1")
                        ComboBox17.Items.Add("2")
                        ComboBox17.Items.Add("3")
                        ComboBox17.Items.Add("4")
                        ComboBox17.Items.Add("5")
                        ComboBox17.Items.Add("6")
                        ComboBox17.Items.Add("7")
                        ComboBox17.Items.Add("8")
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub Expand_Hide_Audio_Opt_Btn(sender As Object, e As EventArgs) Handles Button25.Click
        If Button25.Text = "" Then
            Button25.Text = " "
            While Audio_Opt_Pnl.Height <= 98
                Audio_Opt_Pnl.Height += 1
            End While
            If Button26.Text = "" Then
                Audio_Enc_QC_Pnl.Location = New Point(10, Audio_Opt_Pnl.Location.Y + 100)
                Audio_Enc_Ch_Pnl.Location = New Point(10, Audio_Enc_QC_Pnl.Location.Y + 40)
            ElseIf Button26.Text = " " Then
                Audio_Enc_QC_Pnl.Location = New Point(10, Audio_Opt_Pnl.Location.Y + 100)
                Audio_Enc_Ch_Pnl.Location = New Point(10, Audio_Enc_QC_Pnl.Location.Y + 100)
            End If
            Button25.BackgroundImage = Image.FromFile(UpBtnPath)
        ElseIf Button25.Text = " " Then
            Button25.Text = ""
            While Audio_Opt_Pnl.Height >= 38
                Audio_Opt_Pnl.Height -= 1
            End While
            If Button26.Text = "" Then
                Audio_Enc_QC_Pnl.Location = New Point(10, Audio_Opt_Pnl.Location.Y + 40)
                Audio_Enc_Ch_Pnl.Location = New Point(10, Audio_Enc_QC_Pnl.Location.Y + 40)
            ElseIf Button26.Text = " " Then
                Audio_Enc_QC_Pnl.Location = New Point(10, Audio_Opt_Pnl.Location.Y + 40)
                Audio_Enc_Ch_Pnl.Location = New Point(10, Audio_Enc_QC_Pnl.Location.Y + 100)
            End If
            Button25.BackgroundImage = Image.FromFile(DownBtnPath)
        End If
    End Sub
    Private Sub Expand_Hide_Audio_Enc_Opt_Btn(sender As Object, e As EventArgs) Handles Button26.Click
        If Button26.Text = "" Then
            Button26.Text = " "
            While Audio_Enc_QC_Pnl.Height <= 98
                Audio_Enc_QC_Pnl.Height += 1
            End While
            Audio_Enc_Ch_Pnl.Location = New Point(10, Audio_Enc_QC_Pnl.Location.Y + 100)
            Button26.BackgroundImage = Image.FromFile(UpBtnPath)
        ElseIf Button26.Text = " " Then
            Button26.Text = ""
            While Audio_Enc_QC_Pnl.Height >= 38
                Audio_Enc_QC_Pnl.Height -= 1
            End While
            Audio_Enc_Ch_Pnl.Location = New Point(10, Audio_Enc_QC_Pnl.Location.Y + 40)
            Button26.BackgroundImage = Image.FromFile(DownBtnPath)
        End If
    End Sub
    Private Sub Expand_Hide_Audio_Ch_Opt_Btn(sender As Object, e As EventArgs) Handles Button27.Click
        If Button27.Text = "" Then
            Button27.Text = " "
            While Audio_Enc_Ch_Pnl.Height <= 98
                Audio_Enc_Ch_Pnl.Height += 1
            End While
            Button27.BackgroundImage = Image.FromFile(UpBtnPath)
        ElseIf Button27.Text = " " Then
            Button27.Text = ""
            While Audio_Enc_Ch_Pnl.Height >= 38
                Audio_Enc_Ch_Pnl.Height -= 1
            End While
            Button27.BackgroundImage = Image.FromFile(DownBtnPath)
        End If
    End Sub
    Private Sub LockProfileAudioCheck(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked = True Then
            If TextBox1.Text = "" Then
                MessageBoxAdv.Show("Please choose save media file first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CheckBox5.Checked = False
            Else
                FlagsCount = ComboBox22.Items.Count
                If CheckBox6.Checked Then
                    If ComboBox28.SelectedText.ToString IsNot "Video + Audio (Specific source)" Or ComboBox28.SelectedText.ToString IsNot "Audio Only (Specific Source)" Then
                        If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt") Then
                            ComboBox15.Enabled = False
                            ComboBox16.Enabled = False
                            ComboBox17.Enabled = False
                            ComboBox18.Enabled = False
                            ComboBox19.Enabled = False
                            ComboBox20.Enabled = False
                            ComboBox33.Enabled = False
                            ComboBox34.Enabled = False
                            ComboBox22.Enabled = False
                            Button17.Enabled = False
                            Button18.Enabled = False
                            Label28.Text = "READY"
                        Else
                            CheckBox5.Checked = False
                            MessageBoxAdv.Show("Please save configuration for audio stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString, "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    Else
                        FlagsResult = 0
                        FlagsValue = 0
                        For FlagsStart = 1 To FlagsCount
                            If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (FlagsStart - 1).ToString & ".txt") Then
                                FlagsResult += 1
                            Else
                                MissedFlags(FlagsStart) = FlagsStart
                            End If
                            FlagsValue += 1
                        Next
                        If FlagsResult = FlagsCount Then
                            ComboBox15.Enabled = False
                            ComboBox16.Enabled = False
                            ComboBox17.Enabled = False
                            ComboBox18.Enabled = False
                            ComboBox19.Enabled = False
                            ComboBox20.Enabled = False
                            ComboBox33.Enabled = False
                            ComboBox34.Enabled = False
                            ComboBox22.Enabled = False
                            Button17.Enabled = False
                            Button18.Enabled = False
                            Label28.Text = "READY"
                        Else
                            For FlagsStart = 1 To FlagsValue
                                If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                                    MessageBoxAdv.Show("Please save configuration for audio stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            Next
                            CheckBox5.Checked = False
                        End If
                    End If
                Else
                    FlagsResult = 0
                    FlagsValue = 0
                    For FlagsStart = 1 To FlagsCount
                        If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (FlagsStart - 1).ToString & ".txt") Then
                            FlagsResult += 1
                            FlagsValue += 1
                        Else
                            MissedFlags(FlagsStart) = FlagsStart
                            FlagsValue += 1
                        End If
                    Next
                    If FlagsResult = FlagsCount Then
                        ComboBox15.Enabled = False
                        ComboBox16.Enabled = False
                        ComboBox17.Enabled = False
                        ComboBox18.Enabled = False
                        ComboBox19.Enabled = False
                        ComboBox20.Enabled = False
                        ComboBox33.Enabled = False
                        ComboBox34.Enabled = False
                        ComboBox22.Enabled = False
                        Button17.Enabled = False
                        Button18.Enabled = False
                        Label28.Text = "READY"
                    Else
                        For FlagsStart = 1 To FlagsValue
                            If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                                MessageBoxAdv.Show("Please save configuration for audio stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        Next
                        CheckBox5.Checked = False
                    End If
                End If
            End If
        Else
            AcodecReset()
            ComboBox22.Enabled = True
            ComboBox15.Enabled = True
            Button17.Enabled = True
            Button18.Enabled = True
        End If
    End Sub
    Private Sub EnableTrim(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.Checked = True Then
            If Label2.Text IsNot "" Then
                CheckBox2.Enabled = True
                TextBox7.Enabled = True
                TextBox7.Text = "00"
                TextBox8.Enabled = True
                TextBox8.Text = "00"
                TextBox9.Enabled = True
                TextBox9.Text = "00"
                TextBox10.Enabled = True
                TextBox10.Text = "000"
                TextBox11.Enabled = True
                TextBox11.Text = "000"
                TextBox12.Enabled = True
                TextBox12.Text = "00"
                TextBox13.Enabled = True
                TextBox13.Text = "00"
                TextBox13.Text = "00"
                TextBox14.Enabled = True
                TextBox14.Text = "00"
                CheckBox8.Enabled = False
                CheckBox8.Checked = False
                CheckBox15.Enabled = False
                CheckBox15.Checked = False
                ComboBox26.Enabled = True
                ComboBox26.SelectedIndex = -1
                ComboBox28.SelectedIndex = -1
                MessageBoxAdv.Show("Muxing and Chapter options are not available while trim is enable !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                CheckBox6.Checked = False
                MessageBoxAdv.Show("Please choose media file source first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            CheckBox2.Enabled = False
            TextBox7.Enabled = False
            TextBox7.Text = ""
            TextBox8.Enabled = False
            TextBox8.Text = ""
            TextBox9.Enabled = False
            TextBox9.Text = ""
            TextBox10.Enabled = False
            TextBox10.Text = ""
            TextBox11.Enabled = False
            TextBox11.Text = ""
            TextBox12.Enabled = False
            TextBox12.Text = ""
            TextBox13.Enabled = False
            TextBox13.Text = ""
            TextBox14.Enabled = False
            TextBox14.Text = ""
            ComboBox27.Enabled = False
            ComboBox27.SelectedIndex = -1
            ComboBox26.Enabled = False
            ComboBox26.SelectedIndex = -1
            ComboBox28.Enabled = False
            ComboBox28.SelectedIndex = -1
            CheckBox8.Enabled = True
            CheckBox15.Enabled = True
        End If
    End Sub
    Private Sub TrimQualityCombo(sender As Object, e As EventArgs) Handles ComboBox26.SelectedIndexChanged
        If ComboBox26.SelectedIndex = 0 Then
            CheckBox4.Checked = False
            CheckBox4.Enabled = False
            CheckBox1.Checked = False
            CheckBox1.Enabled = False
            TextBox10.Enabled = False
            TextBox11.Enabled = False
            MessageBoxAdv.Show("Milliseconds time are not available while trim using original quality !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf ComboBox26.SelectedIndex = 1 Then
            CheckBox4.Enabled = True
            CheckBox1.Enabled = True
            TextBox10.Enabled = True
            TextBox11.Enabled = True
        End If
        If Label5.Text.Equals("Not Detected") = True Then
            ComboBox28.SelectedIndex = 3
            ComboBox28.Enabled = False
        ElseIf Label44.Text.Equals("Not Detected") = True Then
            ComboBox28.SelectedIndex = 0
            ComboBox28.Enabled = False
        Else
            ComboBox28.Enabled = True
            ComboBox28.SelectedIndex = -1
        End If
    End Sub
    Private Sub TrimOptions(sender As Object, e As EventArgs) Handles ComboBox28.SelectedIndexChanged
        If ComboBox26.SelectedIndex = 1 Then
            MessageBoxAdv.Show("Custom quality require configuration for each video or audio stream" & vbCrLf &
                               vbCrLf & "Please configure video or audio stream profile before lock profile !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        ComboBox27.Items.Clear()
        getStreamSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34), "Trim")
        If ComboBox28.SelectedIndex = 0 Then
            If ComboBox26.SelectedIndex = 1 Then
                CheckBox4.Checked = False
                CheckBox4.Enabled = False
                CheckBox1.Checked = True
                CheckBox1.Enabled = True
            End If
            ComboBox27.Enabled = True
        ElseIf ComboBox28.SelectedIndex = 1 Then
            If ComboBox26.SelectedIndex = 1 Then
                CheckBox4.Checked = True
                CheckBox4.Enabled = True
                CheckBox1.Checked = True
                CheckBox1.Enabled = True
            End If
            ComboBox27.Enabled = True
        ElseIf ComboBox28.SelectedIndex = 2 Then
            If ComboBox26.SelectedIndex = 1 Then
                CheckBox4.Checked = True
                CheckBox4.Enabled = True
                CheckBox1.Checked = True
                CheckBox1.Enabled = True
            End If
            ComboBox27.Enabled = False
        ElseIf ComboBox28.SelectedIndex = 3 Then
            If ComboBox26.SelectedIndex = 1 Then
                CheckBox4.Checked = True
                CheckBox4.Enabled = True
                CheckBox1.Checked = False
                CheckBox1.Enabled = False
            End If
            ComboBox27.Enabled = True
        End If
    End Sub
    Private Sub TrimSource_Reset(sender As Object, e As EventArgs) Handles ComboBox27.SelectedIndexChanged
        If CheckBox4.Enabled = True Then
            If ComboBox28.Text = "Video Only" Or ComboBox28.Text = "Video + Audio (Specific source)" Or ComboBox28.Text = "Audio Only (Specific Source)" Then
                ComboBox22.Items.Clear()
                ComboBox22.Items.Add(ComboBox27.Text.ToString)
            Else
                If ProgressBarAdv1.Value <> 0 Then
                    ComboBox22.Items.Clear()
                    Dim start As Integer
                    For start = 0 To ComboBox27.Items.Count
                        ComboBox22.Items.Add(ComboBox27.SelectedIndex = start)
                    Next
                    ComboBox22.Enabled = True
                End If
            End If
        End If
    End Sub
    Private Sub StartTimeHours_Check(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox7.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub StartTimeMinute_Check(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox8.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub StartTimeSeconds_Check(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox9.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub StartTimeMs_Check(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox10.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub DurTimeHours_Check(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox14.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub EndTimeMinute_Check(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox13.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub EndTimeSeconds_Check(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox12.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub EndTimeMs_Check(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox11.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub StartTime_Minute(sender As Object, e As EventArgs) Handles TextBox8.TextChanged
        If TextBox8.Text IsNot "" Then
            If CInt(TextBox8.Text) > 59 Then
                MessageBoxAdv.Show("Maximum value for minute is 59 !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox8.Text = ""
            End If
        End If
    End Sub
    Private Sub StartTime_Seconds(sender As Object, e As EventArgs) Handles TextBox9.TextChanged
        If TextBox9.Text IsNot "" Then
            If CInt(TextBox9.Text) > 59 Then
                MessageBoxAdv.Show("Maximum value for seconds is 59 !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox9.Text = ""
            End If
        End If
    End Sub
    Private Sub EndTime_Minute(sender As Object, e As EventArgs) Handles TextBox13.TextChanged
        If TextBox13.Text IsNot "" Then
            If CInt(TextBox13.Text) > 59 Then
                MessageBoxAdv.Show("Maximum value for minute is 59 !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox13.Text = ""
            End If
        End If
    End Sub
    Private Sub EndTime_Seconds(sender As Object, e As EventArgs) Handles TextBox12.TextChanged
        If TextBox12.Text IsNot "" Then
            If CInt(TextBox12.Text) > 59 Then
                MessageBoxAdv.Show("Maximum value for seconds is 59 !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox12.Text = ""
            End If
        End If
    End Sub
    Private Sub LockProfile_Trim(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            If TextBox1.Text = "" Then
                MessageBoxAdv.Show("Please choose save media file first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CheckBox2.Checked = False
            ElseIf ComboBox26.Text Is "" Then
                MessageBoxAdv.Show("Please choose trim quality first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CheckBox2.Checked = False
            ElseIf ComboBox28.Text Is "" Then
                MessageBoxAdv.Show("Please choose trim options first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CheckBox2.Checked = False
            Else
                If ComboBox28.SelectedIndex >= 0 And ComboBox28.SelectedIndex < 3 Then
                    If Strings.Right(TextBox1.Text, 4) = "flac" Or Strings.Right(TextBox1.Text, 4) = ".mp3" Or Strings.Right(TextBox1.Text, 4) = ".wav" Or
                            Strings.Right(TextBox1.Text, 4) = ".mp2" Or Strings.Right(TextBox1.Text, 4) = ".aac" Or Strings.Right(TextBox1.Text, 4) = ".dts" Or
                            Strings.Right(TextBox1.Text, 4) = ".dsd" Or Strings.Right(TextBox1.Text, 4) = ".pcm" Or Strings.Right(TextBox1.Text, 4) = "opus" Or
                            Strings.Right(TextBox1.Text, 4) = ".ogg" Or Strings.Right(TextBox1.Text, 4) = ".ape" Or Strings.Right(TextBox1.Text, 4) = "alac" Or
                            Strings.Right(TextBox1.Text, 4) = "aiff" Or Strings.Right(TextBox1.Text, 4) = ".aif" Or Strings.Right(TextBox1.Text, 4) = ".m4a" Or
                            Strings.Right(TextBox1.Text, 4) = ".tak" Or Strings.Right(TextBox1.Text, 4) = ".tta" Or Strings.Right(TextBox1.Text, 4) = ".wma" Or
                            Strings.Right(TextBox1.Text, 3) = ".wv" Then
                        MessageBoxAdv.Show("Invalid file extension for saved media file !" & vbCrLf & vbCrLf &
                                           "Current file extensions " & vbCrLf &
                                           Strings.Right(TextBox1.Text, 4) & vbCrLf & vbCrLf &
                                           "Current available file extensions " & vbCrLf &
                                           ".mkv, .mp4", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        If ComboBox26.SelectedIndex = 1 Then
                            If ComboBox28.SelectedIndex = 0 Then
                                If ComboBox27.Text Is "" Then
                                    MessageBoxAdv.Show("Please choose trim source first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    CheckBox2.Checked = False
                                    TrimCondition = False
                                Else
                                    FlagsResult = 0
                                    FlagsValue = 0
                                    FlagsCount = ComboBox29.Items.Count
                                    For FlagsStart = 1 To FlagsCount
                                        If File.Exists(VideoStreamFlagsPath & "HME_Video_" & (FlagsStart - 1).ToString & ".txt") Then
                                            FlagsResult += 1
                                        Else
                                            MissedFlags(FlagsStart) = FlagsStart
                                        End If
                                        FlagsValue += 1
                                    Next
                                    If FlagsResult = FlagsCount Then
                                        TrimCondition = True
                                    Else
                                        For FlagsStart = 1 To FlagsValue
                                            If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                                                MessageBoxAdv.Show("Please save configuration for video stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            End If
                                        Next
                                        TrimCondition = False
                                    End If
                                End If
                            ElseIf ComboBox28.SelectedIndex = 1 Then
                                If ComboBox27.Text Is "" Then
                                    MessageBoxAdv.Show("Please choose trim source first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    CheckBox2.Checked = False
                                    TrimCondition = False
                                Else
                                    FlagsResult = 0
                                    FlagsVideoValue = 0
                                    FlagsAudioCount = 0
                                    FlagsVideoCount = CInt(Strings.Mid(ComboBox29.Text.ToString, 11))
                                    FlagsAudioCount = CInt(Strings.Mid(ComboBox27.Text.ToString, 11))
                                    For FlagsStart = 1 To FlagsVideoCount
                                        If File.Exists(VideoStreamFlagsPath & "HME_Video_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt") Then
                                            FlagsResult += 1
                                        Else
                                            MissedFlags(FlagsStart) = FlagsStart
                                        End If
                                        FlagsVideoValue += 1
                                    Next
                                    If FlagsResult = FlagsVideoCount Then
                                        TrimPreCondition += 1
                                    Else
                                        For FlagsStart = 1 To FlagsVideoValue
                                            If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                                                MessageBoxAdv.Show("Please save configuration for video Stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            End If
                                        Next
                                        TrimPreCondition = 0
                                    End If
                                    FlagsResult = 0
                                    For FlagsStart = 1 To FlagsAudioCount
                                        If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt") Then
                                            FlagsResult += 1
                                        Else
                                            MissedFlags(FlagsStart) = FlagsStart
                                        End If
                                        FlagsAudioValue += 1
                                    Next
                                    If FlagsResult = FlagsAudioCount Then
                                        TrimPreCondition += 1
                                    Else
                                        For FlagsStart = 1 To FlagsAudioValue
                                            If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                                                MessageBoxAdv.Show("Please save configuration for audio Stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            End If
                                        Next
                                        TrimPreCondition = 0
                                    End If
                                    If TrimPreCondition = 2 Then
                                        TrimCondition = True
                                    ElseIf TrimPreCondition < 2 Then
                                        TrimCondition = False
                                    End If
                                End If
                            ElseIf ComboBox28.SelectedIndex = 2 Then
                                FlagsVideoCount = ComboBox29.Items.Count
                                FlagsAudioCount = ComboBox22.Items.Count
                                FlagsResult = 0
                                FlagsVideoValue = 0
                                FlagsAudioCount = 0
                                For FlagsStart = 1 To FlagsVideoCount
                                    If File.Exists(VideoStreamFlagsPath & "HME_Video_" & (FlagsStart - 1).ToString & ".txt") Then
                                        FlagsResult += 1
                                    Else
                                        MissedFlags(FlagsStart) = FlagsStart
                                    End If
                                    FlagsVideoValue += 1
                                Next
                                If FlagsResult = FlagsVideoCount Then
                                    TrimPreCondition += 1
                                Else
                                    For FlagsStart = 1 To FlagsVideoValue
                                        If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                                            MessageBoxAdv.Show("Please save configuration for video Stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End If
                                    Next
                                    TrimPreCondition = 0
                                End If
                                FlagsResult = 0
                                For FlagsStart = 1 To FlagsAudioCount
                                    If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (FlagsStart - 1).ToString & ".txt") Then
                                        FlagsResult += 1
                                    Else
                                        MissedFlags(FlagsStart) = FlagsStart
                                    End If
                                    FlagsAudioValue += 1
                                Next
                                If FlagsResult = FlagsAudioCount Then
                                    TrimPreCondition += 1
                                Else
                                    For FlagsStart = 1 To FlagsAudioValue
                                        If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                                            MessageBoxAdv.Show("Please save configuration for audio Stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End If
                                    Next
                                    TrimPreCondition = 0
                                End If
                                If TrimPreCondition = 2 Then
                                    TrimCondition = True
                                ElseIf TrimPreCondition < 2 Then
                                    TrimCondition = False
                                End If
                            End If
                        ElseIf ComboBox26.SelectedIndex = 0 Then
                            TrimCondition = True
                        End If
                    End If
                ElseIf ComboBox28.SelectedIndex = 3 Then
                    If Strings.Right(TextBox1.Text, 4) = ".mkv" Or Strings.Right(TextBox1.Text, 4) = ".mp4" Or Strings.Right(TextBox1.Text, 4) = ".avi" Or
                            Strings.Right(TextBox1.Text, 4) = ".flv" Or Strings.Right(TextBox1.Text, 3) = ".ts" Or Strings.Right(TextBox1.Text, 4) = "m2ts" Or
                            Strings.Right(TextBox1.Text, 4) = ".mov" Or Strings.Right(TextBox1.Text, 4) = ".vob" Or Strings.Right(TextBox1.Text, 4) = ".3gp" Or
                            Strings.Right(TextBox1.Text, 4) = ".mxf" Or Strings.Right(TextBox1.Text, 4) = "webm" Then
                        MessageBoxAdv.Show("Invalid file extension for saved media file !" & vbCrLf & vbCrLf &
                                           "Current file extensions " & vbCrLf &
                                           Strings.Right(TextBox1.Text, 4) & vbCrLf & vbCrLf &
                                           "Current available file extensions " & vbCrLf &
                                           ".flac, .mp3., .wav, .aac", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        If ComboBox26.SelectedIndex = 1 Then
                            If ComboBox28.SelectedIndex = 3 Then
                                FlagsResult = 0
                                FlagsValue = 0
                                FlagsCount = ComboBox22.Items.Count
                                For FlagsStart = 1 To FlagsCount
                                    If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt") Then
                                        FlagsResult += 1
                                    Else
                                        MissedFlags(FlagsStart) = FlagsStart
                                    End If
                                    FlagsValue += 1
                                Next
                                If FlagsResult = FlagsCount Then
                                    TrimCondition = True
                                Else
                                    For FlagsStart = 1 To FlagsValue
                                        If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                                            MessageBoxAdv.Show("Please save configuration for audio stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End If
                                    Next
                                    TrimCondition = False
                                End If
                            End If
                        ElseIf ComboBox26.SelectedIndex = 0 Then
                            TrimCondition = True
                        End If
                    End If
                End If
                If TrimCondition = True Then
                    If CheckBox2.Checked = True Then
                        If TextBox7.Text IsNot "" Then
                            If TextBox7.Text = "0" & CInt(TextBox7.Text) Then
                                TextBox7.Text = TextBox7.Text
                            ElseIf CInt(TextBox7.Text) >= 0 And CInt(TextBox7.Text) < 10 Then
                                TextBox7.Text = "0" & CInt(TextBox7.Text)
                            End If
                        Else
                            TextBox7.Text = "00"
                        End If
                        If TextBox8.Text IsNot "" Then
                            If TextBox8.Text = "0" & CInt(TextBox8.Text) Then
                                TextBox8.Text = TextBox8.Text
                            ElseIf CInt(TextBox8.Text) >= 0 And CInt(TextBox8.Text) < 10 Then
                                TextBox8.Text = "0" & TextBox8.Text
                            Else
                                TextBox8.Text = TextBox8.Text
                            End If
                        Else
                            TextBox8.Text = "00"
                        End If
                        If TextBox9.Text IsNot "" Then
                            If TextBox9.Text = "0" & CInt(TextBox9.Text) Then
                                TextBox9.Text = TextBox9.Text
                            ElseIf CInt(TextBox9.Text) >= 0 And CInt(TextBox9.Text) < 10 Then
                                TextBox9.Text = "0" & TextBox9.Text
                            Else
                                TextBox9.Text = TextBox9.Text
                            End If
                        Else
                            TextBox9.Text = "00"
                        End If
                        If TextBox10.Text IsNot "" Then
                            If CInt(TextBox10.Text) >= 10 And CInt(TextBox10.Text) <= 100 Then
                                TextBox10.Text = "0" & CInt(TextBox10.Text)
                            ElseIf CInt(TextBox10.Text) >= 0 And CInt(TextBox10.Text) <= 10 Then
                                TextBox10.Text = "00" & CInt(TextBox10.Text)
                            End If
                        Else
                            TextBox10.Text = "000"
                        End If
                        If TextBox14.Text IsNot "" Then
                            If TextBox14.Text = "0" & CInt(TextBox14.Text) Then
                                TextBox14.Text = TextBox14.Text
                            ElseIf CInt(TextBox14.Text) >= 0 And CInt(TextBox14.Text) < 10 Then
                                TextBox14.Text = "0" & TextBox14.Text
                            Else
                                TextBox14.Text = TextBox14.Text
                            End If
                        Else
                            TextBox14.Text = "00"
                        End If
                        If TextBox13.Text IsNot "" Then
                            If TextBox13.Text = "0" & CInt(TextBox13.Text) Then
                                TextBox13.Text = TextBox13.Text
                            ElseIf CInt(TextBox13.Text) >= 0 And CInt(TextBox13.Text) < 10 Then
                                TextBox13.Text = "0" & TextBox13.Text
                            Else
                                TextBox13.Text = TextBox13.Text
                            End If
                        Else
                            TextBox13.Text = "00"
                        End If
                        If TextBox12.Text IsNot "" Then
                            If TextBox12.Text = "0" & CInt(TextBox12.Text) Then
                                TextBox12.Text = TextBox12.Text
                            ElseIf CInt(TextBox12.Text) >= 0 And CInt(TextBox12.Text) < 10 Then
                                TextBox12.Text = "0" & TextBox12.Text
                            Else
                                TextBox12.Text = TextBox12.Text
                            End If
                        Else
                            TextBox12.Text = "00"
                        End If
                        If TextBox11.Text IsNot "" Then
                            If CInt(TextBox11.Text) >= 10 And CInt(TextBox11.Text) <= 100 Then
                                TextBox11.Text = "0" & CInt(TextBox11.Text)
                            ElseIf CInt(TextBox11.Text) >= 0 And CInt(TextBox11.Text) <= 10 Then
                                TextBox11.Text = "00" & CInt(TextBox11.Text)
                            End If
                        Else
                            TextBox11.Text = "000"
                        End If
                        TimeSplit = Label80.Text.Split(":")
                        OriTime = TimeConversion(TimeSplit(0), TimeSplit(1), TimeSplit(2))
                        TrimEndTime = TimeConversion(CInt(TextBox14.Text), CInt(TextBox13.Text), CInt(TextBox12.Text))
                        TrimStartTime = TimeConversion(CInt(TextBox7.Text), CInt(TextBox8.Text), CInt(TextBox9.Text))
                        If TrimStartTime <= OriTime Then
                            If TrimEndTime <= OriTime Then
                                If TrimEndTime <= TrimStartTime Then
                                    MessageBoxAdv.Show("End time can not less or same than start time duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    CheckBox2.Checked = False
                                Else
                                    If TextBox14.Text IsNot "" AndAlso TextBox13.Text IsNot "" AndAlso TextBox12.Text IsNot "" AndAlso TextBox11.Text IsNot "" AndAlso
                                    TextBox7.Text IsNot "" AndAlso TextBox8.Text IsNot "" AndAlso TextBox9.Text IsNot "" AndAlso TextBox10.Text IsNot "" Then
                                        TextBox7.Enabled = False
                                        TextBox8.Enabled = False
                                        TextBox9.Enabled = False
                                        TextBox10.Enabled = False
                                        TextBox11.Enabled = False
                                        TextBox12.Enabled = False
                                        TextBox13.Enabled = False
                                        TextBox14.Enabled = False
                                        ComboBox26.Enabled = False
                                        ComboBox27.Enabled = False
                                        ComboBox28.Enabled = False
                                        If ComboBox26.SelectedIndex = 0 Then
                                            If TrimStartTime = 0 Then
                                                RichTextBox3.Text = " -i " & Chr(34) & Label2.Text & Chr(34) & " -to " & TextBox14.Text & ":" & TextBox13.Text & ":" & TextBox12.Text & "." & TextBox11.Text
                                            Else
                                                If TrimStartTime > 0 And TrimStartTime < 5 Then
                                                    MessageBoxAdv.Show("Start time can not in range 1 to 4 seconds in original quality ! " & vbCrLf & vbCrLf & "Please use custom quality instead", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                    CheckBox2.Checked = False
                                                    TextBox9.Text = "00"
                                                    TextBox7.Enabled = True
                                                    TextBox8.Enabled = True
                                                    TextBox9.Enabled = True
                                                    TextBox10.Enabled = True
                                                    TextBox11.Enabled = True
                                                    TextBox12.Enabled = True
                                                    TextBox13.Enabled = True
                                                    TextBox14.Enabled = True
                                                    ComboBox26.Enabled = True
                                                    ComboBox27.Enabled = False
                                                    ComboBox28.Enabled = False
                                                    CheckBox2.Checked = False
                                                ElseIf TrimStartTime = 5 Then
                                                    RichTextBox3.Text = " -i " & Chr(34) & Label2.Text & Chr(34) & " -ss " & TrimStartTime & " -to " & TrimEndTime
                                                Else
                                                    Dim newTrimDurTime As Integer = TrimEndTime - TrimStartTime
                                                    RichTextBox3.Text = " -ss " & TrimStartTime & " -i " & Chr(34) & Label2.Text & Chr(34) & " -t " & newTrimDurTime - 1
                                                End If
                                            End If
                                        ElseIf ComboBox26.SelectedIndex = 1 Then
                                            RichTextBox3.Text = " -i " & Chr(34) & Label2.Text & Chr(34) & " -ss " & TextBox7.Text & ":" & TextBox8.Text & ":" & TextBox9.Text & "." & TextBox10.Text &
                                                                " -to " & TextBox14.Text & ":" & TextBox13.Text & ":" & TextBox12.Text & "." & TextBox11.Text
                                        End If
                                    Else
                                        MessageBoxAdv.Show("Please fill all time column first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        CheckBox2.Checked = False
                                        TextBox7.Enabled = True
                                        TextBox8.Enabled = True
                                        TextBox9.Enabled = True
                                        TextBox10.Enabled = True
                                        TextBox11.Enabled = True
                                        TextBox12.Enabled = True
                                        TextBox13.Enabled = True
                                        TextBox14.Enabled = True
                                        ComboBox26.Enabled = True
                                        ComboBox27.Enabled = False
                                        ComboBox28.Enabled = False
                                        ComboBox27.SelectedIndex = -1
                                        ComboBox28.SelectedIndex = -1
                                    End If
                                End If
                            Else
                                MessageBoxAdv.Show("End time can not more than actual video duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                CheckBox2.Checked = False
                            End If
                        Else
                            MessageBoxAdv.Show("Start time can not more than actual video duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            CheckBox2.Checked = False
                        End If
                    Else
                        CheckBox2.Checked = False
                        TextBox7.Enabled = True
                        TextBox8.Enabled = True
                        TextBox9.Enabled = True
                        TextBox10.Enabled = True
                        TextBox11.Enabled = True
                        TextBox12.Enabled = True
                        TextBox13.Enabled = True
                        TextBox14.Enabled = True
                        ComboBox26.Enabled = True
                        ComboBox27.Enabled = False
                        ComboBox28.Enabled = False
                        ComboBox27.SelectedIndex = -1
                        ComboBox28.SelectedIndex = -1
                    End If
                Else
                    MessageBoxAdv.Show("Please check video or audio configuration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox2.Checked = False
                    TextBox7.Enabled = True
                    TextBox8.Enabled = True
                    TextBox9.Enabled = True
                    TextBox10.Enabled = True
                    TextBox11.Enabled = True
                    TextBox12.Enabled = True
                    TextBox13.Enabled = True
                    TextBox14.Enabled = True
                    ComboBox26.Enabled = True
                    ComboBox27.SelectedIndex = -1
                    ComboBox28.SelectedIndex = -1
                    ComboBox27.Enabled = False
                    ComboBox28.Enabled = False
                End If
            End If
        Else
            CheckBox2.Checked = False
            TextBox7.Enabled = True
            TextBox8.Enabled = True
            TextBox9.Enabled = True
            TextBox10.Enabled = True
            TextBox11.Enabled = True
            TextBox12.Enabled = True
            TextBox13.Enabled = True
            TextBox14.Enabled = True
            ComboBox26.Enabled = True
            ComboBox26.SelectedIndex = -1
            ComboBox27.SelectedIndex = -1
            ComboBox28.SelectedIndex = -1
            ComboBox27.Enabled = False
            ComboBox28.Enabled = False
            CheckBox1.Enabled = True
            CheckBox4.Enabled = True
            RichTextBox3.Text = ""
        End If
    End Sub
    Private Sub EnableMuxing_Check(sender As Object, e As EventArgs) Handles CheckBox8.CheckedChanged
        If CheckBox8.Checked = True Then
            TextBox15.Enabled = True
            CheckBox11.Enabled = True
            TextBox16.Enabled = True
            Button9.Enabled = True
            Button10.Enabled = True
            CheckBox9.Enabled = True
            CheckBox10.Enabled = True
            ComboBox1.Enabled = True
            ComboBox1.SelectedIndex = 0
            CheckBox7.Enabled = True
            CheckBox6.Checked = False
            CheckBox6.Enabled = False
            CheckBox15.Checked = False
            CheckBox15.Enabled = False
            MessageBoxAdv.Show("Trim and Chapter options are not available while muxing is enable !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            TextBox15.Text = ""
            TextBox15.Enabled = False
            Button9.Enabled = False
            CheckBox11.Enabled = False
            TextBox16.Enabled = False
            TextBox16.Text = ""
            Button10.Enabled = False
            CheckBox9.Enabled = False
            CheckBox10.Enabled = False
            ComboBox1.Enabled = False
            ComboBox1.SelectedIndex = -1
            CheckBox7.Enabled = False
            CheckBox6.Enabled = True
            CheckBox15.Enabled = True
            ComboBox25.Enabled = False
            ComboBox25.SelectedIndex = -1
        End If
    End Sub
    Private Sub SameMedia_Muxing_Check(sender As Object, e As EventArgs) Handles CheckBox11.CheckedChanged
        If CheckBox11.Checked = True Then
            If Label2.Text IsNot "" Then
                If Strings.Right(Label2.Text, 4) = "flac" Or Strings.Right(Label2.Text, 4) = ".mp3" Or Strings.Right(Label2.Text, 4) = ".wav" Then
                    MessageBoxAdv.Show("Please choose video file only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox11.Checked = False
                Else
                    TextBox15.Text = Label2.Text
                    Button9.Enabled = False
                End If
            Else
                MessageBoxAdv.Show("Please choose open media file first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CheckBox11.Checked = False
            End If
        Else
            Button9.Enabled = True
        End If
    End Sub
    Private Sub BrowseVideo_Muxing(sender As Object, e As EventArgs) Handles Button9.Click
        OpenFileDialog.DefaultExt = "*.*|.avi|.mp4|.mkv|.mp2|.m2ts|.ts"
        OpenFileDialog.FilterIndex = 1
        OpenFileDialog.Filter = "All files (*.*)|*.*|AVI|*.avi|MPEG-4|*.mp4|Matroska Video|*.mkv|MPEG-2|*.m2v;*.m2ts;*.ts"
        OpenFileDialog.Title = "Choose Media File"
        OpenFileDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
        If OpenFileDialog.ShowDialog() = DialogResult.OK Then
            If Strings.Right(OpenFileDialog.FileName, 4) = ".mkv" Or Strings.Right(OpenFileDialog.FileName, 4) = ".mp4" Or Strings.Right(OpenFileDialog.FileName, 4) = ".avi" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".flv" Or Strings.Right(OpenFileDialog.FileName, 3) = ".ts" Or Strings.Right(OpenFileDialog.FileName, 4) = "m2ts" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".mov" Or Strings.Right(OpenFileDialog.FileName, 4) = ".vob" Or Strings.Right(OpenFileDialog.FileName, 4) = "webm" Then
                TextBox15.Text = OpenFileDialog.FileName
                ComboBox25.Items.Clear()
                getStreamSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & TextBox15.Text & Chr(34), "Muxing")
            Else
                MessageBoxAdv.Show("Media file format are not supported !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub
    Private Sub BrowseAudio_Muxing(sender As Object, e As EventArgs) Handles Button10.Click
        OpenFileDialog.DefaultExt = "*.*|.flac|.aiff|.alac|.mp3"
        OpenFileDialog.FilterIndex = 1
        OpenFileDialog.Filter = "All files (*.*)|*.*|FLAC|*.flac|WAV|*.wav|AIFF|*.aiff|ALAC|*.alac|MP3|*.mp3"
        OpenFileDialog.Title = "Choose Media File"
        OpenFileDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
        If OpenFileDialog.ShowDialog() = DialogResult.OK Then
            If Strings.Right(OpenFileDialog.FileName, 4) = ".mp3" Or Strings.Right(OpenFileDialog.FileName, 4) = ".mp2" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".aac" Or Strings.Right(OpenFileDialog.FileName, 4) = ".dts" Or Strings.Right(OpenFileDialog.FileName, 4) = ".dsd" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".pcm" Or Strings.Right(OpenFileDialog.FileName, 4) = "opus" Or Strings.Right(OpenFileDialog.FileName, 4) = ".ogg" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".ape" Or Strings.Right(OpenFileDialog.FileName, 4) = "alac" Or Strings.Right(OpenFileDialog.FileName, 4) = "aiff" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".aif" Or Strings.Right(OpenFileDialog.FileName, 4) = ".m4a" Or Strings.Right(OpenFileDialog.FileName, 4) = ".tak" Or
                            Strings.Right(OpenFileDialog.FileName, 4) = ".tta" Or Strings.Right(OpenFileDialog.FileName, 4) = ".wma" Or Strings.Right(OpenFileDialog.FileName, 3) = ".wv" Then
                TextBox16.Text = OpenFileDialog.FileName
            Else
                MessageBoxAdv.Show("Media file format are not supported !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        End If
    End Sub
    Private Sub ReplaceExistingAudio_Check(sender As Object, e As EventArgs) Handles CheckBox9.CheckedChanged
        If CheckBox9.Checked = True Then
            CheckBox10.Checked = False
            CheckBox10.Enabled = False
            ComboBox25.Enabled = True
            getStreamSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & TextBox15.Text & Chr(34), "Muxing")
        Else
            CheckBox10.Checked = False
            CheckBox10.Enabled = True
            ComboBox25.Enabled = False
            ComboBox25.SelectedIndex = -1
        End If
    End Sub
    Private Sub AddAsNewAudioStream_Check(sender As Object, e As EventArgs) Handles CheckBox10.CheckedChanged
        If CheckBox10.Checked = True Then
            CheckBox9.Checked = False
            CheckBox9.Enabled = False
            ComboBox25.Enabled = False
            ComboBox25.SelectedIndex = -1
            getStreamSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & TextBox15.Text & Chr(34), "Muxing + Custom")
        Else
            ComboBox25.Enabled = False
            ComboBox25.SelectedIndex = -1
            CheckBox9.Checked = False
            CheckBox9.Enabled = True
        End If
    End Sub
    Private Sub LockProfile_Mux(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        If CheckBox7.Checked = True Then
            If TextBox1.Text = "" Then
                MessageBoxAdv.Show("Please choose save media file first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CheckBox7.Checked = False
            Else
                If Label2.Text IsNot "" Or TextBox15.Text IsNot "" Then
                    If TextBox16.Text IsNot "" Then
                        If Strings.Right(TextBox1.Text, 4) = "flac" Or Strings.Right(TextBox1.Text, 4) = ".mp3" Or Strings.Right(TextBox1.Text, 4) = ".wav" Or
                            Strings.Right(TextBox1.Text, 4) = ".mp2" Or Strings.Right(TextBox1.Text, 4) = ".aac" Or Strings.Right(TextBox1.Text, 4) = ".dts" Or
                            Strings.Right(TextBox1.Text, 4) = ".dsd" Or Strings.Right(TextBox1.Text, 4) = ".pcm" Or Strings.Right(TextBox1.Text, 4) = "opus" Or
                            Strings.Right(TextBox1.Text, 4) = ".ogg" Or Strings.Right(TextBox1.Text, 4) = ".ape" Or Strings.Right(TextBox1.Text, 4) = "alac" Or
                            Strings.Right(TextBox1.Text, 4) = "aiff" Or Strings.Right(TextBox1.Text, 4) = ".aif" Or Strings.Right(TextBox1.Text, 4) = ".m4a" Or
                            Strings.Right(TextBox1.Text, 4) = ".tak" Or Strings.Right(TextBox1.Text, 4) = ".tta" Or Strings.Right(TextBox1.Text, 4) = ".wma" Or
                            Strings.Right(TextBox1.Text, 3) = ".wv" Then
                            MessageBoxAdv.Show("Invalid file extension for saved media file !" & vbCrLf & vbCrLf &
                                           "Current file extensions " & vbCrLf &
                                           Strings.Right(TextBox1.Text, 4) & vbCrLf & vbCrLf &
                                           "Current available file extensions " & vbCrLf &
                                           ".mkv, .mp4", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Else
                            Button9.Enabled = False
                            Button10.Enabled = False
                            If ComboBox1.Text Is "" Then
                                ComboBox1.SelectedIndex = 0
                            End If
                            ComboBox1.Enabled = False
                            If CheckBox11.Checked = False Then
                                VideoFilePath = TextBox15.Text.ToString
                            Else
                                VideoFilePath = Label2.Text.ToString
                            End If
                            CheckBox8.Enabled = False
                            CheckBox11.Enabled = False
                            CheckBox9.Enabled = False
                            CheckBox10.Enabled = False
                            ComboBox25.Enabled = False
                            If ComboBox1.SelectedIndex = 1 Then
                                FlagsResult = 0
                                FlagsVideoValue = 0
                                FlagsAudioCount = 0
                                FlagsVideoCount = ComboBox29.Items.Count
                                FlagsAudioCount = ComboBox22.Items.Count
                                For FlagsStart = 1 To FlagsVideoCount
                                    If File.Exists(VideoStreamFlagsPath & "HME_Video_" & (FlagsStart - 1).ToString & ".txt") Then
                                        FlagsResult += 1
                                    Else
                                        MissedFlags(FlagsStart) = FlagsStart
                                    End If
                                    FlagsVideoValue += 1
                                Next
                                If FlagsResult = FlagsVideoCount Then
                                    TrimPreCondition += 1
                                Else
                                    For FlagsStart = 1 To FlagsVideoValue
                                        If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                                            MessageBoxAdv.Show("Please save configuration for video Stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End If
                                    Next
                                    TrimPreCondition = 0
                                End If
                                FlagsResult = 0
                                For FlagsStart = 1 To FlagsAudioCount
                                    If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (FlagsStart - 1).ToString & ".txt") Then
                                        FlagsResult += 1
                                    Else
                                        MissedFlags(FlagsStart) = FlagsStart
                                    End If
                                    FlagsAudioValue += 1
                                Next
                                If FlagsResult = FlagsAudioCount Then
                                    TrimPreCondition += 1
                                Else
                                    For FlagsStart = 1 To FlagsAudioValue
                                        If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                                            MessageBoxAdv.Show("Please save configuration for audio Stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End If
                                    Next
                                    TrimPreCondition = 0
                                End If
                                If TrimPreCondition = 2 Then
                                    TrimCondition = True
                                ElseIf TrimPreCondition < 2 Then
                                    TrimCondition = False
                                End If
                            ElseIf ComboBox1.SelectedIndex = 0 Then
                                TrimCondition = True
                            End If
                            If TrimCondition = True Then
                                If CheckBox9.Checked = True And CheckBox10.Checked = False Then
                                    If ComboBox25.SelectedIndex = -1 Then
                                        MessageBoxAdv.Show("Please choose which audio stream that want to replace !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Else
                                        Dim AudioStream As String = (CInt(Strings.Mid(ComboBox25.Text.ToString, 11)) - 1).ToString
                                        Dim AudioStreamArray As String = CInt(Strings.Mid(ComboBox25.Text.ToString, 11)).ToString
                                        Dim numbers(255) As String
                                        For FlagsStart = 1 To ComboBox25.Items.Count
                                            numbers(FlagsStart) = FlagsStart
                                        Next
                                        If File.Exists("HME_Stream_Replace.txt") Then
                                            GC.Collect()
                                            GC.WaitForPendingFinalizers()
                                            File.Delete("HME_Stream_Replace.txt")
                                            File.Create("HME_Stream_Replace.txt").Dispose()
                                        End If
                                        For FlagsStart = 1 To ComboBox25.Items.Count
                                            File.AppendAllText("HME_Stream_Replace.txt", " -map 0:" & numbers(FlagsStart))
                                        Next
                                        If FindConfig("HME_Stream_Replace.txt", "-map 0:" & AudioStreamArray) IsNot "" Then
                                            Dim ReplaceStream As String = File.ReadAllText("HME_Stream_Replace.txt")
                                            ReplaceStream = ReplaceStream.Replace(" -map 0:" & AudioStreamArray, " -map 1:0 ")
                                            File.WriteAllText("HME_Stream_Replace.txt", ReplaceStream)
                                        End If
                                        If ComboBox1.Text = "Original Quality" Then
                                            RichTextBox4.Text = " -i " & Chr(34) & VideoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0:0 " & File.ReadAllText("HME_Stream_Replace.txt") & " -c copy "
                                        ElseIf ComboBox1.Text = "Custom Quality" Then
                                            RichTextBox4.Text = " -i " & Chr(34) & VideoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0:0 " & File.ReadAllText("HME_Stream_Replace.txt") & ""
                                        End If
                                    End If
                                ElseIf CheckBox9.Checked = False And CheckBox10.Checked = True Then
                                    If ComboBox1.Text = "Original Quality" Then
                                        RichTextBox4.Text = " -i " & Chr(34) & VideoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0 -map 1:a -c copy "
                                    ElseIf ComboBox1.Text = "Custom Quality" Then
                                        RichTextBox4.Text = " -i " & Chr(34) & VideoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0 -map 1:a "
                                    End If
                                ElseIf CheckBox9.Checked = False And CheckBox10.Checked = False Then
                                    If ComboBox1.Text = "Original Quality" Then
                                        RichTextBox4.Text = " -i " & Chr(34) & VideoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0 -map 1:a -c copy "
                                    ElseIf ComboBox1.Text = "Custom Quality" Then
                                        RichTextBox4.Text = " -i " & Chr(34) & VideoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0 -map 1:a "
                                    End If
                                End If
                            Else
                                MessageBoxAdv.Show("Please check video or audio configuration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                CheckBox7.Checked = False
                                CheckBox8.Enabled = True
                                Button10.Enabled = True
                                CheckBox9.Enabled = True
                                CheckBox10.Enabled = True
                                CheckBox11.Enabled = True
                                If CheckBox11.Checked Then
                                    Button9.Enabled = False
                                Else
                                    Button9.Enabled = True
                                End If
                                ComboBox1.Enabled = True
                                ComboBox25.Enabled = True
                                If CheckBox9.Checked Then
                                    CheckBox10.Checked = False
                                    CheckBox10.Enabled = False
                                Else
                                    CheckBox10.Checked = False
                                    CheckBox10.Enabled = True
                                End If
                                If CheckBox10.Checked Then
                                    CheckBox9.Checked = False
                                    CheckBox9.Enabled = False
                                Else
                                    CheckBox9.Checked = False
                                    CheckBox9.Enabled = True
                                End If
                                RichTextBox4.Text = ""
                            End If
                        End If
                    Else
                        MessageBoxAdv.Show("Please choose audio file for muxing !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        CheckBox7.Checked = False
                        CheckBox8.Enabled = True
                        If CheckBox11.Checked Then
                            Button9.Enabled = False
                        Else
                            Button9.Enabled = True
                        End If
                        Button10.Enabled = True
                        CheckBox9.Enabled = True
                        CheckBox10.Enabled = True
                        CheckBox11.Enabled = True
                        ComboBox1.Enabled = True
                        If CheckBox9.Checked Then
                            CheckBox10.Checked = False
                            CheckBox10.Enabled = False
                        Else
                            CheckBox10.Checked = False
                            CheckBox10.Enabled = True
                        End If
                        If CheckBox10.Checked Then
                            CheckBox9.Checked = False
                            CheckBox9.Enabled = False
                        Else
                            CheckBox9.Checked = False
                            CheckBox9.Enabled = True
                        End If
                        RichTextBox4.Text = ""
                    End If
                Else
                    MessageBoxAdv.Show("Please choose media file source or video source first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox7.Checked = False
                    CheckBox8.Enabled = True
                    If CheckBox11.Checked Then
                        Button9.Enabled = False
                    Else
                        Button9.Enabled = True
                    End If
                    Button10.Enabled = True
                    CheckBox9.Enabled = True
                    CheckBox10.Enabled = True
                    CheckBox11.Enabled = True
                    ComboBox1.Enabled = True
                    ComboBox25.Enabled = True
                    If CheckBox9.Checked Then
                        CheckBox10.Checked = False
                        CheckBox10.Enabled = False
                    ElseIf CheckBox10.Checked Then
                        CheckBox9.Checked = False
                        CheckBox9.Enabled = False
                        ComboBox25.Enabled = False
                    End If
                    RichTextBox4.Text = ""
                End If
            End If
        Else
            CheckBox8.Enabled = True
            If CheckBox11.Checked Then
                Button9.Enabled = False
            Else
                Button9.Enabled = True
            End If
            Button10.Enabled = True
            CheckBox9.Enabled = True
            CheckBox10.Enabled = True
            CheckBox11.Enabled = True
            ComboBox1.Enabled = True
            If CheckBox9.Checked Then
                CheckBox10.Checked = False
                CheckBox10.Enabled = False
            ElseIf CheckBox10.Checked Then
                CheckBox9.Checked = False
                CheckBox9.Enabled = False
                ComboBox25.SelectedIndex = -1
                ComboBox25.Enabled = False
            End If
            RichTextBox4.Text = ""
        End If
    End Sub
    Private Sub EnableChapter(sender As Object, e As EventArgs) Handles CheckBox15.CheckedChanged
        If CheckBox15.Checked = True Then
            If Label2.Text IsNot "" Then
                If Label5.Text.Equals("Not Detected") = True And TextBox15.Text Is "" Then
                    CheckBox15.Checked = False
                    MessageBoxAdv.Show("Current media file does not contain any video stream !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    CheckBox14.Enabled = True
                    TextBox5.Enabled = True
                    TextBox17.Enabled = True
                    TextBox18.Enabled = True
                    TextBox19.Enabled = True
                    Button11.Enabled = True
                    Button12.Enabled = True
                    Button13.Enabled = True
                    Button14.Enabled = True
                    CheckBox8.Checked = False
                    CheckBox8.Enabled = False
                    CheckBox6.Checked = False
                    CheckBox6.Enabled = False
                    ChapterReset()
                    GetChapter()
                    MessageBoxAdv.Show("Trim and Muxing options are not available while chapter is enable !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                CheckBox15.Checked = False
                MessageBoxAdv.Show("Please choose media file source first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            CheckBox14.Enabled = False
            TextBox5.Enabled = False
            TextBox5.Text = ""
            TextBox17.Enabled = False
            TextBox17.Text = ""
            TextBox18.Enabled = False
            TextBox18.Text = ""
            TextBox19.Enabled = False
            TextBox19.Text = ""
            Button11.Enabled = False
            Button12.Enabled = False
            Button13.Enabled = False
            Button14.Enabled = False
            RichTextBox5.Text = ""
            CheckBox8.Enabled = True
            CheckBox6.Enabled = True
            ChapterReset()
            ListView1.Items.Clear()
        End If
    End Sub
    Private Sub GetChapter()
        Newffargs = "ffmpeg -i " & Chr(34) & Label2.Text & Chr(34) & " -f ffmetadata " & Chr(34) & My.Application.Info.DirectoryPath & "\FFMETADATAFILE" & Chr(34)
        HMEGenerate("HME_Chapters.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs, "")
        RunProc("HME_Chapters.bat")
        If File.Exists("FFMETADATAFILE") = True Then
            Dim chapterStatus As Boolean
            Dim curLoop As Integer = 1
            Dim cnvTime As String
            Dim count As Integer = 0
            Dim selectedTime As New List(Of String)()
            Dim selectedTitle As New List(Of String)()
            Dim readMetadataLines() As String = File.ReadAllLines("FFMETADATAFILE")
            If File.ReadAllText("FFMETADATAFILE").Contains("START=") = True Then
                Do
                    If readMetadataLines(count).Contains("START=") Then
                        Dim curLines As String = RemoveWhitespace(Strings.Mid(readMetadataLines(count), 7))
                        Dim updatedLines As String
                        If curLines = "0" Then
                            updatedLines = curLines
                        Else
                            updatedLines = curLines.Remove(curLines.Length - 9)
                        End If
                        cnvTime = TimeConversionReverse(updatedLines)
                        selectedTime.Add(cnvTime)
                    End If
                    count += 1
                Loop While count < File.ReadAllLines("FFMETADATAFILE").Length
                chapterStatus = True
            Else
                chapterStatus = False
            End If
            count = 0
            If File.ReadAllText("FFMETADATAFILE").Contains("title=") = True Then
                Do
                    If readMetadataLines(count).Contains("title=") Then
                        Dim curLines As String = Strings.Mid(readMetadataLines(count), 7)
                        selectedTitle.Add(curLines)
                        curLoop += 1
                    End If
                    count += 1
                Loop While count < File.ReadAllLines("FFMETADATAFILE").Length
                chapterStatus = True
            Else
                chapterStatus = False
            End If
            If chapterStatus = True Then
                Dim listOfTitle As List(Of String) = New List(Of String)(selectedTitle)
                Dim n As Integer = 0
                For Each value As String In selectedTime
                    Dim newChapter As New ListViewItem(value)
                    ListView1.Items.Add(newChapter)
                    newChapter.SubItems.Add(listOfTitle(n + 1))
                    n += 1
                Next
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
            File.Delete("FFMETADATAFILE")
        End If
    End Sub
    Private Sub AddChapter(sender As Object, e As EventArgs) Handles Button11.Click
        If TextBox5.Text IsNot "" AndAlso TextBox17.Text IsNot "" AndAlso TextBox18.Text IsNot "" AndAlso TextBox19.Text IsNot "" Then
            If ChapterTimeCheck() = True Then
                Dim curTimeCnv As String()
                Dim newTime As String = TextBox5.Text & ":" & TextBox17.Text & ":" & TextBox18.Text
                Dim newTimeCnv As String() = newTime.Split(":")
                Dim newChapter As New ListViewItem(newTime)
                If ListView1.Items.Count = 0 Then
                    newChapter.SubItems.Add(TextBox19.Text)
                    ListView1.Items.Add(newChapter)
                    MessageBoxAdv.Show("Chapter successfully added !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ListView1.Items(0).Selected = False
                    ListView1.Items(0).Focused = False
                    ListView1.Items(ListView1.Items.Count - 1).Selected = True
                    ListView1.Items(ListView1.Items.Count - 1).Focused = True
                    ListView1.FocusedItem.Selected = True
                    ListView1.Focus()
                    curTimeCnv = ListView1.SelectedItems(0).SubItems(0).Text.Split(":")
                    If TimeConversion(newTimeCnv(0), newTimeCnv(1), newTimeCnv(2)) < TimeConversion(curTimeCnv(0), curTimeCnv(1), curTimeCnv(2)) Or
                       TimeConversion(newTimeCnv(0), newTimeCnv(1), newTimeCnv(2)) = TimeConversion(curTimeCnv(0), curTimeCnv(1), curTimeCnv(2)) Then
                        ListView1.Items(0).Selected = False
                        ListView1.Items(0).Focused = False
                        MessageBoxAdv.Show("New time chapter can not less or same than previous time chapter duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        ListView1.Items(0).Selected = False
                        ListView1.Items(0).Focused = False
                        newChapter.SubItems.Add(TextBox19.Text)
                        ListView1.Items.Add(newChapter)
                        MessageBoxAdv.Show("Chapter successfully added !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            Else
                MessageBoxAdv.Show("Time chapter can not more than video duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            ChapterReset()
        Else
            MessageBoxAdv.Show("Add chapter failed !" & vbCrLf & vbCrLf & "Make sure to fill time and chapter title completely", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub UpdateChapter(sender As Object, e As EventArgs) Handles Button13.Click
        If TextBox5.Text IsNot "" AndAlso TextBox17.Text IsNot "" AndAlso TextBox18.Text IsNot "" AndAlso TextBox19.Text IsNot "" Then
            If ChapterTimeCheck() = True Then
                If ListView1.SelectedItems.Count > 0 Then
                    Dim curTimeCnv As String()
                    Dim newTime As String = TextBox5.Text & ":" & TextBox17.Text & ":" & TextBox18.Text
                    Dim newTimeCnv As String() = newTime.Split(":")
                    If ListView1.FocusedItem.Index = 0 Then
                        If ListView1.Items.Count > 0 Then
                            ListView1.Items(1).Selected = False
                            ListView1.Items(1).Focused = False
                            ListView1.FocusedItem.Selected = False
                            ListView1.Items(ListView1.FocusedItem.Index + 1).Selected = True
                            ListView1.Items(ListView1.FocusedItem.Index + 1).Focused = True
                            ListView1.FocusedItem.Selected = True
                            ListView1.Focus()
                            curTimeCnv = ListView1.SelectedItems(0).SubItems(0).Text.Split(":")
                            If TimeConversion(newTimeCnv(0), newTimeCnv(1), newTimeCnv(2)) > TimeConversion(curTimeCnv(0), curTimeCnv(1), curTimeCnv(2)) Or
                                TimeConversion(newTimeCnv(0), newTimeCnv(1), newTimeCnv(2)) = TimeConversion(curTimeCnv(0), curTimeCnv(1), curTimeCnv(2)) Then
                                If ListView1.FocusedItem.Index + 1 > 1 Then
                                    ListView1.Items(0).Selected = False
                                    ListView1.Items(0).Focused = False
                                Else
                                    ListView1.Items(1).Selected = False
                                    ListView1.Items(1).Focused = False
                                End If
                                ListView1.FocusedItem.Selected = False
                                ListView1.Items(ListView1.FocusedItem.Index - 1).Selected = True
                                ListView1.Items(ListView1.FocusedItem.Index - 1).Focused = True
                                ListView1.FocusedItem.Selected = True
                                ListView1.Focus()
                                MessageBoxAdv.Show("New time chapter can not more or same than next time chapter duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Else
                                If ListView1.FocusedItem.Index + 1 > 1 Then
                                    ListView1.Items(0).Selected = False
                                    ListView1.Items(0).Focused = False
                                Else
                                    ListView1.Items(1).Selected = False
                                    ListView1.Items(1).Focused = False
                                End If
                                ListView1.FocusedItem.Selected = False
                                ListView1.Items(ListView1.FocusedItem.Index - 1).Selected = True
                                ListView1.Items(ListView1.FocusedItem.Index - 1).Focused = True
                                ListView1.FocusedItem.Selected = True
                                ListView1.Focus()
                                ChapterReplace("update", newTime)
                                MessageBoxAdv.Show("Chapter successfully updated !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                ChapterReset()
                            End If
                        Else
                            ChapterReplace("update", newTime)
                            MessageBoxAdv.Show("Chapter successfully updated !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            ChapterReset()
                        End If
                    ElseIf ListView1.FocusedItem.Index + 1 = ListView1.Items.Count Then
                        ListView1.Items(0).Selected = False
                        ListView1.Items(0).Focused = False
                        ListView1.FocusedItem.Selected = False
                        ListView1.Items(ListView1.FocusedItem.Index - 1).Selected = True
                        ListView1.Items(ListView1.FocusedItem.Index - 1).Focused = True
                        ListView1.FocusedItem.Selected = True
                        ListView1.Focus()
                        curTimeCnv = ListView1.SelectedItems(0).SubItems(0).Text.Split(":")
                        If TimeConversion(newTimeCnv(0), newTimeCnv(1), newTimeCnv(2)) < TimeConversion(curTimeCnv(0), curTimeCnv(1), curTimeCnv(2)) Or
                            TimeConversion(newTimeCnv(0), newTimeCnv(1), newTimeCnv(2)) = TimeConversion(curTimeCnv(0), curTimeCnv(1), curTimeCnv(2)) Then
                            If ListView1.FocusedItem.Index + 1 > 1 Then
                                ListView1.Items(0).Selected = False
                                ListView1.Items(0).Focused = False
                            Else
                                ListView1.Items(1).Selected = False
                                ListView1.Items(1).Focused = False
                            End If
                            ListView1.FocusedItem.Selected = False
                            ListView1.Items(ListView1.FocusedItem.Index + 1).Selected = True
                            ListView1.Items(ListView1.FocusedItem.Index + 1).Focused = True
                            ListView1.FocusedItem.Selected = True
                            ListView1.Focus()
                            MessageBoxAdv.Show("New time chapter can not less than previous time chapter duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Else
                            If ListView1.FocusedItem.Index + 1 > 1 Then
                                ListView1.Items(0).Selected = False
                                ListView1.Items(0).Focused = False
                            Else
                                ListView1.Items(1).Selected = False
                                ListView1.Items(1).Focused = False
                            End If
                            ListView1.FocusedItem.Selected = False
                            ListView1.Items(ListView1.FocusedItem.Index + 1).Selected = True
                            ListView1.Items(ListView1.FocusedItem.Index + 1).Focused = True
                            ListView1.FocusedItem.Selected = True
                            ListView1.Focus()
                            ChapterReplace("update", newTime)
                            MessageBoxAdv.Show("Chapter successfully updated !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            ChapterReset()
                        End If
                    ElseIf ListView1.FocusedItem.Index + 1 > 1 And ListView1.FocusedItem.Index < ListView1.Items.Count Then
                        ListView1.Items(0).Selected = False
                        ListView1.Items(0).Focused = False
                        ListView1.FocusedItem.Selected = False
                        ListView1.Items(ListView1.FocusedItem.Index + 1).Selected = True
                        ListView1.Items(ListView1.FocusedItem.Index + 1).Focused = True
                        ListView1.FocusedItem.Selected = True
                        ListView1.Focus()
                        curTimeCnv = ListView1.SelectedItems(0).SubItems(0).Text.Split(":")
                        If TimeConversion(newTimeCnv(0), newTimeCnv(1), newTimeCnv(2)) > TimeConversion(curTimeCnv(0), curTimeCnv(1), curTimeCnv(2)) Or
                            TimeConversion(newTimeCnv(0), newTimeCnv(1), newTimeCnv(2)) = TimeConversion(curTimeCnv(0), curTimeCnv(1), curTimeCnv(2)) Then
                            If ListView1.FocusedItem.Index + 1 > 1 Then
                                ListView1.Items(0).Selected = False
                                ListView1.Items(0).Focused = False
                            Else
                                ListView1.Items(1).Selected = False
                                ListView1.Items(1).Focused = False
                            End If
                            ListView1.FocusedItem.Selected = False
                            ListView1.Items(ListView1.FocusedItem.Index - 1).Selected = True
                            ListView1.Items(ListView1.FocusedItem.Index - 1).Focused = True
                            ListView1.FocusedItem.Selected = True
                            ListView1.Focus()
                            MessageBoxAdv.Show("New time chapter can not more than next time chapter duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Else
                            ListView1.Items(0).Selected = False
                            ListView1.Items(0).Focused = False
                            ListView1.FocusedItem.Selected = False
                            ListView1.Items(ListView1.FocusedItem.Index - 2).Selected = True
                            ListView1.Items(ListView1.FocusedItem.Index - 2).Focused = True
                            ListView1.FocusedItem.Selected = True
                            ListView1.Focus()
                            curTimeCnv = ListView1.SelectedItems(0).SubItems(0).Text.Split(":")
                            If TimeConversion(newTimeCnv(0), newTimeCnv(1), newTimeCnv(2)) < TimeConversion(curTimeCnv(0), curTimeCnv(1), curTimeCnv(2)) Or
                                TimeConversion(newTimeCnv(0), newTimeCnv(1), newTimeCnv(2)) = TimeConversion(curTimeCnv(0), curTimeCnv(1), curTimeCnv(2)) Then
                                If ListView1.FocusedItem.Index + 1 > 1 Then
                                    ListView1.Items(0).Selected = False
                                    ListView1.Items(0).Focused = False
                                Else
                                    ListView1.Items(1).Selected = False
                                    ListView1.Items(1).Focused = False
                                End If
                                ListView1.FocusedItem.Selected = False
                                ListView1.Items(ListView1.FocusedItem.Index + 1).Selected = True
                                ListView1.Items(ListView1.FocusedItem.Index + 1).Focused = True
                                ListView1.FocusedItem.Selected = True
                                ListView1.Focus()
                                MessageBoxAdv.Show("New time chapter can not less than previous time chapter duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Else
                                If ListView1.FocusedItem.Index + 1 > 1 Then
                                    ListView1.Items(0).Selected = False
                                    ListView1.Items(0).Focused = False
                                Else
                                    ListView1.Items(1).Selected = False
                                    ListView1.Items(1).Focused = False
                                End If
                                ListView1.FocusedItem.Selected = False
                                ListView1.Items(ListView1.FocusedItem.Index + 1).Selected = True
                                ListView1.Items(ListView1.FocusedItem.Index + 1).Focused = True
                                ListView1.FocusedItem.Selected = True
                                ListView1.Focus()
                                ChapterReplace("update", newTime)
                                MessageBoxAdv.Show("Chapter successfully updated !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                ChapterReset()
                            End If
                        End If
                    End If
                Else
                    MessageBoxAdv.Show("Please select chapter that want to update !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBoxAdv.Show("Time chapter can not more than video duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBoxAdv.Show("Update chapter failed !" & vbCrLf & vbCrLf & "Make sure to fill time and chapter title completely", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub RemoveChapter(sender As Object, e As EventArgs) Handles Button12.Click
        If ListView1.SelectedItems.Count > 0 Then
            ChapterReplace("remove", "")
        Else
            MessageBoxAdv.Show("Please select chapter that want to remove !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub ChapterReset(sender As Object, e As EventArgs) Handles Button14.Click
        ChapterReset()
        ListView1.Items.Clear()
        GetChapter()
        MessageBoxAdv.Show("Chapter data has been reset !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Private Sub ChapterList_Clicked(sender As Object, e As EventArgs) Handles ListView1.Click
        If TextBox5.Text IsNot "" Or TextBox17.Text IsNot "" Or TextBox18.Text IsNot "" Or TextBox19.Text IsNot "" Then
            Dim chapterResult As DialogResult = MessageBoxAdv.Show(Me, "Want to replace current title " & ListView1.SelectedItems(0).SubItems(1).Text & " ? ", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If chapterResult = DialogResult.Yes Then
                ChapterReplace("show", "")
            End If
        Else
            ChapterReplace("show", "")
        End If
    End Sub
    Private Sub ChapterReplace(cmd As String, newTime As String)
        If ListView1.SelectedItems.Count > 0 Then
            If cmd = "show" Then
                Dim TimeSplit As String()
                TimeSplit = ListView1.SelectedItems(0).SubItems(0).Text.Split(":")
                TextBox5.Text = TimeSplit(0)
                TextBox17.Text = TimeSplit(1)
                TextBox18.Text = TimeSplit(2)
                TextBox19.Text = ListView1.SelectedItems(0).SubItems(1).Text
            ElseIf cmd = "update" Then
                ListView1.SelectedItems(0).SubItems(0).Text = newTime
                ListView1.SelectedItems(0).SubItems(1).Text = TextBox19.Text
            ElseIf cmd = "remove" Then
                MessageBoxAdv.Show("Chapter name " & Chr(34) & ListView1.SelectedItems(0).SubItems(1).Text & Chr(34) & " removed !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ListView1.Items.Remove(ListView1.SelectedItems(0))
            End If
        End If
    End Sub
    Private Function ChapterTimeCheck() As Boolean
        If TextBox5.Text IsNot "" Then
            If TextBox5.Text = "0" & CInt(TextBox5.Text) Then
                TextBox5.Text = TextBox5.Text
            ElseIf CInt(TextBox5.Text) >= 0 And CInt(TextBox5.Text) < 10 Then
                TextBox5.Text = "0" & CInt(TextBox5.Text)
            End If
        Else
            TextBox5.Text = "00"
        End If
        If TextBox17.Text IsNot "" Then
            If TextBox17.Text = "0" & CInt(TextBox17.Text) Then
                TextBox17.Text = TextBox17.Text
            ElseIf CInt(TextBox17.Text) >= 0 And CInt(TextBox17.Text) < 10 Then
                TextBox17.Text = "0" & TextBox17.Text
            Else
                TextBox17.Text = TextBox17.Text
            End If
        Else
            TextBox17.Text = "00"
        End If
        If TextBox18.Text IsNot "" Then
            If TextBox18.Text = "0" & CInt(TextBox18.Text) Then
                TextBox18.Text = TextBox18.Text
            ElseIf CInt(TextBox18.Text) >= 0 And CInt(TextBox18.Text) < 10 Then
                TextBox18.Text = "0" & TextBox18.Text
            Else
                TextBox18.Text = TextBox18.Text
            End If
        Else
            TextBox18.Text = "00"
        End If
        TimeSplit = Label81.Text.Split(":")
        TimeDur = TimeConversion(TimeSplit(0), TimeSplit(1), TimeSplit(2))
        TimeChapter = TimeConversion(CInt(TextBox5.Text), CInt(TextBox17.Text), CInt(TextBox18.Text))
        If TimeChapter <= TimeDur Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub ChapterReset()
        TextBox5.Text = ""
        TextBox5.Text = "00"
        TextBox17.Text = ""
        TextBox17.Text = "00"
        TextBox18.Text = ""
        TextBox18.Text = "00"
        TextBox19.Text = ""
    End Sub
    Private Sub ChapterTimeHours_Check(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox5.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub ChapterTimeMinute_Check(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox17.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub ChapterTimeSeconds_Check(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox18.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub Chapter_Seconds_Validity(sender As Object, e As EventArgs) Handles TextBox18.TextChanged
        If TextBox18.Text IsNot "" Then
            If CInt(TextBox18.Text) > 59 Then
                MessageBoxAdv.Show("Maximum value for seconds is 59 !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox18.Text = ""
            End If
        End If
    End Sub
    Private Sub Chapter_Minute_Validity(sender As Object, e As EventArgs) Handles TextBox17.TextChanged
        If TextBox17.Text IsNot "" Then
            If CInt(TextBox17.Text) > 59 Then
                MessageBoxAdv.Show("Maximum value for seconds is 59 !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox17.Text = ""
            End If
        End If
    End Sub
    Private Sub ChapterLock(sender As Object, e As EventArgs) Handles CheckBox14.CheckedChanged
        If CheckBox14.Checked = True Then
            If ListView1.Items.Count > 0 Then
                If Strings.Right(TextBox1.Text, 4) = "flac" Or Strings.Right(TextBox1.Text, 4) = ".mp3" Or Strings.Right(TextBox1.Text, 4) = ".wav" Then
                    MessageBoxAdv.Show("Invalid file extension for saved media file !" & vbCrLf & vbCrLf &
                                              "Current file extensions " & vbCrLf &
                                              Strings.Right(TextBox1.Text, 4) & vbCrLf & vbCrLf &
                                              "Current available file extensions " & vbCrLf &
                                              ".mkv", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    If File.Exists("FFMETADATAFILE") Then
                        GC.Collect()
                        GC.WaitForPendingFinalizers()
                        File.Delete("FFMETADATAFILE")
                    End If
                    File.Create("FFMETADATAFILE").Dispose()
                    Dim writer As New StreamWriter("FFMETADATAFILE", True)
                    writer.WriteLine(";FFMETADATAFILE1")
                    writer.WriteLine("title=Chapter" & vbCrLf)
                    For cm = 0 To ListView1.Items.Count - 1
                        Dim nextdur As String()
                        Dim selectedDur As String() = ListView1.Items(cm).SubItems(0).Text.Split(":")
                        If cm < ListView1.Items.Count - 1 Then
                            nextdur = ListView1.Items(cm + 1).SubItems(0).Text.Split(":")
                        Else
                            nextdur = Label81.Text.Split(":")
                        End If
                        Dim selectedTitle As String = ListView1.Items(cm).SubItems(1).Text
                        Dim convSelDur As Integer = TimeConversion(selectedDur(0), selectedDur(1), selectedDur(2))
                        Dim convNextDur As Integer = TimeConversion(nextdur(0), nextdur(1), nextdur(2))
                        writer.WriteLine("[CHAPTER]")
                        writer.WriteLine("TIMEBASE=1/1000")
                        writer.WriteLine("START=" & convSelDur * 1000)
                        writer.WriteLine("END=" & (convNextDur - 1) * 1000)
                        writer.WriteLine("Title=" & selectedTitle & vbCrLf)
                    Next
                    writer.Close()
                    RichTextBox5.Text = " -i " & Chr(34) & My.Application.Info.DirectoryPath & "\FFMETADATAFILE" & Chr(34) & " -map_chapters 1 "
                    TextBox5.Enabled = False
                    TextBox17.Enabled = False
                    TextBox18.Enabled = False
                    TextBox19.Enabled = False
                    Button11.Enabled = False
                    Button12.Enabled = False
                    Button13.Enabled = False
                    Button14.Enabled = False
                End If
            Else
                MessageBoxAdv.Show("Please add chapter first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox5.Enabled = True
                TextBox17.Enabled = True
                TextBox18.Enabled = True
                TextBox19.Enabled = True
                Button11.Enabled = True
                Button12.Enabled = True
                Button13.Enabled = True
                Button14.Enabled = True
                RichTextBox5.Enabled = True
            End If
        Else
            TextBox5.Enabled = True
            TextBox17.Enabled = True
            TextBox18.Enabled = True
            TextBox19.Enabled = True
            Button11.Enabled = True
            Button12.Enabled = True
            Button13.Enabled = True
            Button14.Enabled = True
            RichTextBox5.Enabled = True
        End If
    End Sub
    Private Sub ResetInit()
        CheckBox1.Checked = False
        CheckBox2.Checked = False
        CheckBox2.Enabled = False
        CheckBox3.Enabled = False
        CheckBox3.Checked = False
        CheckBox4.Checked = False
        CheckBox5.Checked = False
        CheckBox5.Enabled = False
        CheckBox6.Checked = False
        CheckBox7.Checked = False
        CheckBox7.Enabled = False
        CheckBox8.Checked = False
        CheckBox11.Enabled = False
        CheckBox11.Checked = False
        Button15.Enabled = False
        Button16.Enabled = False
        Button17.Enabled = False
        Button18.Enabled = False
        CheckBox14.Checked = False
        CheckBox15.Checked = False
    End Sub
End Class