Imports System.IO
Imports System.Text.RegularExpressions
Imports Syncfusion.Windows.Forms
Imports Syncfusion.Windows.Forms.Tools
Imports Syncfusion.WinForms.Controls
Public Class MainMenu
    Inherits SfForm
    Dim mediaQueueTable As New DataTable("Media Queue")
    Dim contextstripstate1 As String
    Dim contextstripstate2 As String
    Dim contextstripstate3 As String
    Dim abortSeeking As Boolean
    Dim mediaenginestate As String
    Private Sub MainMenu_load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        AllowRoundedCorners = True
        Panel1.AllowDrop = True
        LoadQueueTable()
        KeyPreview = True
        Me.ProgressBarAdv1.TextOrientation = Orientation.Horizontal
        Me.ProgressBarAdv1.TextAlignment = TextAlignment.Center
        Me.ProgressBarAdv1.TextShadow = False
        MetroSetTabControl1.Font = New Font("Segoe UI Semibold", 9.75, FontStyle.Regular)
        StyleManager1.SetTheme(HMESetTheme.ToString)
        LazyTheme()
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
        ResetInit("nothing")
        Dim res1 As String = File.ReadAllText(My.Application.Info.DirectoryPath & "\Init_Res.txt")
        If RemoveWhitespace(res1).Equals("") = True Then
            DebugMode = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Debug Mode:")
            FfmpegConfig = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "FFMPEG Binary:")
            NVENCCBinary = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "NVENCC Binary:")
            Hwdefconfig = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "GPU Engine:")
            MediaEngine = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Media Engine:")
            FrameConfig = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Frame Count:")
            AlwaysFullscreenConf = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Always Fullscreen:")
            FfmpegConf = FfmpegConfig.Remove(0, 14) & "\"
            FfmpegLetter = FfmpegConf.Substring(0, 1) & ":"
            NVENCCConf = NVENCCBinary.Remove(0, 14) & "\"
            NVENCCLetter = NVENCCConf.Substring(0, 1) & ":"
            If MediaEngine IsNot "null" Then
                mediaenginestate = MediaEngine.Remove(0, 13)
            Else
                mediaenginestate = "FFMPEG"
            End If
            Button1.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            Dim ImgPrev1 As New FileStream(VideoPlaceholder, FileMode.Open, FileAccess.Read)
            PictureBox1.Image = Image.FromStream(ImgPrev1)
            ImgPrev1.Close()
            Text = My.Application.Info.Title.ToString + " v" + My.Application.Info.Version.ToString + " - [" + mediaenginestate + getMediaEngineVersion(mediaenginestate).ToString.Substring(0, getMediaEngineVersion(mediaenginestate).ToString.Length - 1) + "]"
            If DebugMode IsNot "null" Then
                If DebugMode.Remove(0, 11) = "True" Then
                    Text = My.Application.Info.Title.ToString + " v" + My.Application.Info.Version.ToString + " (Debug Mode) - [" + mediaenginestate + getMediaEngineVersion(mediaenginestate).ToString.Substring(0, getMediaEngineVersion(mediaenginestate).ToString.Length - 1) + "]"
                End If
            End If
            If AlwaysFullscreenConf IsNot "null" Then
                Me.WindowState = FormWindowState.Maximized
            End If
            Tooltip(Label33, "Configure video codec / encoder that will use for encoding video")
            Tooltip(Label58, "Configure maximum video frame rate / second")
            Tooltip(Label34, "Configure the output conversion for pixel format")
            Tooltip(Label49, "Configure level of support within a profile specifies the maximum picture resolution, frame rate, and bit rate that a decoder may use")
            Tooltip(Label50, "Configure the encoding tier")
            Tooltip(Label46, "Configure the encoding preset (slow for highest compression, fast for lowest compression)")
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
            Tooltip(Label138, "Configure number of columns and rows in the tile-based encoding (AV1)")
            Tooltip(Label150, "Configure lookahead level. Higher level may improve quality at the expense of performance")
            Tooltip(Label151, "Deinterlace the input video using the yadif or bwdif algorithm, but implemented in CUDA so that it can work as part of a GPU accelerated pipeline with nvdec and/or nvenc")
            Tooltip(Label152, "Specify interlacing mode to adopt")
            Tooltip(Label153, "Specify picture field parity assumed for the input interlaced video")
            Tooltip(Label154, "Specify which frames to deinterlace. All to Deinterlace all frames, Interlaced to only deinterlace frames marked as interlaced")
            InitMedia(Textbox77)
            If Textbox77.Text.Equals("") = False Then
                OpenMedia_Load("Nothing")
            End If
            CleanEnv("all")
        Else
            Button1.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            MessageBoxAdv.Show(res1, "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            CleanEnv("all")
        End If
        MetroSetSwitch3.Enabled = False
        MetroSetSwitch4.Enabled = False
        MetroSetSwitch5.Enabled = False
    End Sub
    Private Sub MainMenu_Close(sender As Object, e As EventArgs) Handles MyBase.FormClosed
        CleanEnv("all")
        InitExit("")
    End Sub
    Private Sub ResetState(sender As Object, e As EventArgs) Handles Button29.Click
        Dim resetProc As DialogResult = MessageBoxAdv.Show(Me, "Reset to initial state ?" & vbCrLf &
                                                                                           vbCrLf & "- Reset all include configuration to initial state (YES)" &
                                                                                           vbCrLf & "- Reset only running process (NO)", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If resetProc = DialogResult.Yes Then
            Dim loadInit = New Loading("Reset", Textbox77.Text)
            loadInit.Show()
            InitExit("reset")
            ResetInit("nothing")
            Dim ImgPrev1 As New FileStream(VideoPlaceholder, FileMode.Open, FileAccess.Read)
            PictureBox1.Image = Image.FromStream(ImgPrev1)
            ImgPrev1.Close()
            OpenMedia_Reset()
            Textbox77.Text = ""
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
            Label44.Text = "Not Detected"
            Label39.Text = "Not Detected"
            Label42.Text = "Not Detected"
            Label35.Text = "Not Detected"
            Label26.Text = "Not Detected"
            Label32.Text = "Not Detected"
            Label30.Text = "Not Detected"
            Label61.Text = "Not Detected"
            Label107.Text = "Not Detected"
            ComboBox23.Text = ""
            ComboBox24.Text = ""
            ComboBox23.Items.Clear()
            ComboBox24.Items.Clear()
            Button1.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            Label96.Text = "0"
            Label98.Text = "0"
            Button7.Visible = False
            Button7.Enabled = False
            Button8.Visible = False
            Button8.Enabled = False
            loadInit.Close()
            NotifyIcon("Hana Media Encoder", "HME already reset to initial state !", 1000, True)
        Else
            Dim loadInit = New Loading("Reset", Textbox77.Text)
            loadInit.Show()
            InitExit("reset")
            Button1.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            loadInit.Close()
            NotifyIcon("Hana Media Encoder", "HME already reset to initial state !", 1000, True)
        End If
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        If Textbox77.Text IsNot "" And TabPage1.Visible = True And File.Exists(My.Application.Info.DirectoryPath & "\thumbnail\1.jpg") Then
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
        If Textbox77.Text.Equals("") = False Then
            OldTitle = Textbox77.Text
        Else
            OldTitle = ""
        End If
        For Each path In files
            Textbox77.Text = path
        Next
        If Strings.Right(Textbox77.Text, 4).ToLower = ".mkv" Or Strings.Right(Textbox77.Text, 4).ToLower = ".mp4" Or Strings.Right(Textbox77.Text, 4).ToLower = ".avi" Or
                            Strings.Right(Textbox77.Text, 4).ToLower = ".flv" Or Strings.Right(Textbox77.Text, 3).ToLower = ".ts" Or Strings.Right(Textbox77.Text, 4).ToLower = "m2ts" Or
                            Strings.Right(Textbox77.Text, 4).ToLower = ".mov" Or Strings.Right(Textbox77.Text, 4).ToLower = ".mp4" Or Strings.Right(Textbox77.Text, 4).ToLower = ".vob" Or
                            Strings.Right(Textbox77.Text, 4).ToLower = ".3gp" Or Strings.Right(Textbox77.Text, 4).ToLower = ".mxf" Or Strings.Right(Textbox77.Text, 4).ToLower = "flac" Or
                            Strings.Right(Textbox77.Text, 4).ToLower = ".wav" Or Strings.Right(Textbox77.Text, 4).ToLower = ".mp3" Or Strings.Right(Textbox77.Text, 4).ToLower = ".mp2" Or
                            Strings.Right(Textbox77.Text, 4).ToLower = ".aac" Or Strings.Right(Textbox77.Text, 4).ToLower = ".dts" Or Strings.Right(Textbox77.Text, 4).ToLower = ".dsd" Or
                            Strings.Right(Textbox77.Text, 4).ToLower = ".pcm" Or Strings.Right(Textbox77.Text, 4).ToLower = "opus" Or Strings.Right(Textbox77.Text, 4).ToLower = ".ogg" Or
                            Strings.Right(Textbox77.Text, 4).ToLower = ".ape" Or Strings.Right(Textbox77.Text, 4).ToLower = "alac" Or Strings.Right(Textbox77.Text, 4).ToLower = "aiff" Or
                            Strings.Right(Textbox77.Text, 4).ToLower = ".aif" Or Strings.Right(Textbox77.Text, 4).ToLower = ".m4a" Or Strings.Right(Textbox77.Text, 4).ToLower = ".tak" Or
                            Strings.Right(Textbox77.Text, 4).ToLower = ".tta" Or Strings.Right(Textbox77.Text, 4).ToLower = ".wma" Or Strings.Right(Textbox77.Text, 3).ToLower = ".wv" Or
                            Strings.Right(Textbox77.Text, 4).ToLower = "webm" Or Strings.Right(Textbox77.Text, 4).ToLower = ".m2v" Or Strings.Right(Textbox77.Text, 4).ToLower = ".mts" Then
            If ComboBox2.SelectedIndex >= 0 Then
                ComboBox2.SelectedIndex = 0
                ComboBox2.Text = ""
            End If
            OpenMedia_Load("nothing")
        Else
            OpenMedia_Reset()
            NotifyIcon("Hana Media Encoder", "Media file format are not supported !", 1000, False)
            Textbox77.Text = OldTitle
        End If
    End Sub
    Private Sub Panel1_DragEnter(sender As Object, e As DragEventArgs) Handles Panel1.DragEnter
        If Button1.Enabled = True AndAlso Button3.Enabled = True AndAlso Button4.Enabled = True Then
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                e.Effect = DragDropEffects.All
            End If
        End If
    End Sub
    Private Sub EncodingFlags_Btn(sender As Object, e As EventArgs) Handles Button28.Click
        Dim menu_eflags = New EFlagsMenu
        menu_eflags.Show()
    End Sub
    Private Sub Information_Btn(sender As Object, e As EventArgs) Handles Button2.Click
        Dim menu_information = New InformationMenu
        menu_information.Show()
    End Sub
    Private Sub Options_Btn(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Hide()
        Dim optionsMenu = New OptionsMenu
        optionsMenu.Show()
    End Sub
    Private Sub OpenMedia_Btn(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog.DefaultExt = "*.*|.avi|.mp4|.mkv|.mp2|.m2ts|.mts|.ts|.wav|.flac|.aiff|.alac|.mp3|.opus"
        OpenFileDialog.FilterIndex = 1
        OpenFileDialog.Filter = "All files (*.*)|*.*|AVI|*.avi|MPEG-4|*.mp4|Matroska Video|*.mkv|MPEG-2|*.m2v;*.m2ts;*.mts;*.ts|FLAC|*.flac|WAV|*.wav|AIFF|*.aiff|ALAC|*.alac|AAC|*.m4a|MP3|*.mp3|MP2|*.mp2|OPUS|*.opus"
        OpenFileDialog.Title = "Choose Media File"
        OpenFileDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
        If OpenFileDialog.ShowDialog() = DialogResult.OK Then
            If Textbox77.Text.Equals("") = False Then
                OldTitle = Textbox77.Text
            Else
                OldTitle = ""
            End If
            If Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mkv" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mp4" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".avi" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".flv" Or Strings.Right(OpenFileDialog.FileName, 3).ToLower = ".ts" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = "m2ts" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mov" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mp4" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".vob" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".3gp" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mxf" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = "flac" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".wav" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mp3" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mp2" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".aac" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".dts" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".dsd" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".pcm" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = "opus" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".ogg" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".ape" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = "alac" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = "aiff" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".aif" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".m4a" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".tak" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".tta" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".wma" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".wv" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = "webm" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".m2v" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mts" Then
                Textbox77.Text = OpenFileDialog.FileName
                If ComboBox2.SelectedIndex >= 0 Then
                    ComboBox2.SelectedIndex = 0
                    ComboBox2.Text = ""
                End If
                OpenMedia_Load("nothing")
            Else
                OpenMedia_Reset()
                NotifyIcon("Hana Media Encoder", "Media file format are not supported !", 1000, False)
                Textbox77.Text = OldTitle
            End If
        End If
    End Sub
    Private Sub OpenMedia_Load(state As String)
        Dim loadInit = New Loading("Media", Textbox77.Text)
        loadInit.Show()
        If state.Equals("muxing") Then
            ResetInit("muxing")
        Else
            ResetInit("nothing")
        End If
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
        If mediaenginestate = "FFMPEG" Then
            MetroSetSwitch3.Enabled = True
            MetroSetSwitch4.Enabled = True
            MetroSetSwitch5.Enabled = True
        ElseIf mediaenginestate = "NVENCC" Then
            MetroSetSwitch3.Enabled = False
            MetroSetSwitch4.Enabled = False
            MetroSetSwitch5.Enabled = False
            NotifyIcon("NVENCC Media Engine", "Some features are limited !", 1000, True)
        End If
        getVideoSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Textbox77.Text & Chr(34), "0")
        getAudioSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Textbox77.Text & Chr(34), "0")
        If abortSeeking = True Then
            Textbox77.Text = ""
            ImageDir = VideoErrorPlaceholder
            Dim ImgPrev1 As New FileStream(ImageDir, FileMode.Open, FileAccess.Read)
            PictureBox1.Image = Image.FromStream(ImgPrev1)
            Button1.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            MetroSetSwitch1.Enabled = False
            MetroSetSwitch2.Enabled = False
            MetroSetSwitch3.Enabled = False
            MetroSetSwitch4.Enabled = False
            MetroSetSwitch5.Enabled = False
            MetroSetSwitch6.Enabled = False
            ImgPrev1.Close()
            loadInit.Close()
            OpenMedia_Reset()
            NotifyIcon("Hana Media Encoder", "Media file format are not supported !", 1000, False)
        Else
            MetroSetComboBox3.Items.Clear()
            ComboBox23.Items.Clear()
            ComboBox24.Items.Clear()
            ComboBox25.Items.Clear()
            MetroSetComboBox1.Items.Clear()
            getDurationSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Textbox77.Text & Chr(34))
            If MetroSetSwitch6.Switched = True And DataGridView1.Rows.Count > 0 Then
                getStreamSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Textbox77.Text & Chr(34), "Queue", "LoadMedia")
            Else
                getStreamSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Textbox77.Text & Chr(34), "Encoding", "")
            End If
            If ComboBox23.Items.Count > 0 Then
                ComboBox23.SelectedIndex = 0
            End If
            If ComboBox24.Items.Count > 0 Then
                ComboBox24.SelectedIndex = 0
            End If
            PictureBox1.Image = Nothing
            PictureBox1.BackColor = Color.Empty
            PictureBox1.Invalidate()
            ChapterReplace("reset", "")
            loadInit.Close()
            getPreviewSummary_Async(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Textbox77.Text & Chr(34))
            getSpectrumSummary_Async()
            ComboBox31.Enabled = True
            ComboBox31.SelectedIndex = 0
            If Label5.Text.Equals("Not Detected") = True Then
                ComboBox24.Text = ""
            End If
            If Label44.Text.Equals("Not Detected") = True Then
                ComboBox23.Text = ""
            End If
            Label76.Visible = True
            Label70.Visible = True
            Label70.Text = GetFileSize(Textbox77.Text)
            Label71.Visible = False
            Label77.Visible = False
            Label28.Text = "Standby"
            MetroSetSwitch6.CheckState = False
            MetroSetSwitch6.Switched = False
        End If
    End Sub
    Private Sub OpenMedia_Reset()
        ProgressBarAdv1.Visible = False
        Label70.Visible = False
        Label76.Visible = False
        Label71.Visible = False
        Label77.Visible = False
        ComboBox31.Enabled = False
    End Sub
    Private Sub SaveMedia_Btn(sender As Object, e As EventArgs) Handles Button6.Click
        If MetroSetSwitch6.Switched = True And DataGridView1.Rows.Count > 0 Then
            Dim mediaSavePath As New FolderBrowserDialog
            If mediaSavePath.ShowDialog() = DialogResult.OK Then
                TextBox1.Text = mediaSavePath.SelectedPath
            End If
        Else
            SaveFileDialog.DefaultExt = ".mkv|.wav|.flac|.mp3"
            SaveFileDialog.FilterIndex = 1
            SaveFileDialog.Filter = "MKV|*.mkv|MP4|*.mp4|AAC|*.m4a|FLAC|*.flac|WAV|*.wav|MP3|*.mp3|MP2|*.mp2|OPUS|*.opus"
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
            If File.Exists(My.Application.Info.DirectoryPath & "\thumbnail\1.jpg") Then
                If CurPos < MaxPos Then
                    Dim ImgPrev2 As New FileStream(My.Application.Info.DirectoryPath & "\thumbnail\" & CurPos + 1 & ".jpg", FileMode.Open, FileAccess.Read)
                    PictureBox1.Image = Image.FromStream(ImgPrev2)
                    ImgPrev2.Close()
                    Label96.Text = CurPos + 1
                End If
            End If
        End If
    End Sub
    Private Sub ImagePrev_Prev()
        CurPos = CInt(Label96.Text)
        MaxPos = CInt(Label98.Text)
        If ComboBox31.SelectedIndex = 0 Then
            If File.Exists(My.Application.Info.DirectoryPath & "\thumbnail\1.jpg") Then
                If CurPos > 1 Then
                    Dim ImgPrev2 As New FileStream(My.Application.Info.DirectoryPath & "\thumbnail\" & CurPos - 1 & ".jpg", FileMode.Open, FileAccess.Read)
                    PictureBox1.Image = Image.FromStream(ImgPrev2)
                    ImgPrev2.Close()
                    Label96.Text = CurPos - 1
                End If
            End If
        End If
    End Sub
    Private Sub PreviewOptions(sender As Object, e As EventArgs) Handles ComboBox31.SelectedIndexChanged
        If ComboBox31.SelectedIndex = 0 Then
            If File.Exists(My.Application.Info.DirectoryPath & "\thumbnail\1.jpg") Then
                ImageDir = My.Application.Info.DirectoryPath & "\thumbnail\1.jpg"
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
            If File.Exists(My.Application.Info.DirectoryPath & "\spectrum-temp.jpg") Then
                ImageDir = My.Application.Info.DirectoryPath & "\spectrum-temp.jpg"
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
            If File.Exists(My.Application.Info.DirectoryPath & "\thumbnail\1.jpg") Then
                ImageDir = My.Application.Info.DirectoryPath & "\thumbnail\" & CInt(Label96.Text) & ".jpg"
            Else
                ImageDir = "null"
            End If
        ElseIf ComboBox31.SelectedIndex = 1 Then
            If File.Exists(My.Application.Info.DirectoryPath & "\spectrum-temp.jpg") Then
                ImageDir = My.Application.Info.DirectoryPath & "\spectrum-temp.jpg"
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
        If Textbox77.Text = "" Then
            NotifyIcon("Preview media", "Please open media file first", 1000, False)
        Else
            previewMediaModule(Textbox77.Text, FfmpegConf & "ffplay.exe", HwAccelDev, Label5.Text)
        End If
    End Sub
    Private Sub VideoStream_Info(sender As Object, e As EventArgs) Handles ComboBox24.SelectedIndexChanged
        getVideoSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Textbox77.Text & Chr(34), (CInt(Strings.Mid(ComboBox24.Text.ToString, 11)) - 1).ToString)
    End Sub
    Public Sub getVideoSummary(ffmpegletter As String, ffmpegbin As String, videoFile As String, videoStream As String)
        Newffargs = "ffprobe -hide_banner " & " -show_streams -select_streams v:" & videoStream & " " & videoFile & " 2>&1 "
        HMEGenerate(HMEEngine & "HME_Video_Summary.bat", ffmpegletter, ffmpegbin, Newffargs, "")
        Dim psi As New ProcessStartInfo(HMEEngine & "HME_Video_Summary.bat") With {
           .RedirectStandardError = False,
           .RedirectStandardOutput = True,
           .CreateNoWindow = True,
           .WindowStyle = ProcessWindowStyle.Hidden,
           .UseShellExecute = False
       }
        Dim process As Process = Process.Start(psi)
        While Not process.StandardOutput.EndOfStream
            Dim line As String = process.StandardOutput.ReadToEnd()
            If line.Contains("Invalid data found when processing input") = False Then
                Dim codec_name As String = getBetween(line, "codec_name=", "codec_long_name")
                If RemoveWhitespace(codec_name) = "" Or RemoveWhitespace(codec_name) = "mjpeg" Then
                    If VideoQueue = False Then
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
                    End If
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
                    If VideoQueue = False Then
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
                        If codec_new_bit_rate = "N/A" Then
                            Dim codec_alt_bit_rate As String = RemoveWhitespace(getBetween(line, "bitrate:", "kb/s"))
                            If New Regex("^[0-9]+$").IsMatch(RemoveWhitespace(codec_alt_bit_rate)) Then
                                Label24.Text = GetFileSizeAlt(CInt(RemoveWhitespace(codec_alt_bit_rate)))
                            Else
                                Label24.Text = "Not Detected"
                            End If
                        Else
                            If New Regex("^[0-9]+$").IsMatch(RemoveWhitespace(codec_new_bit_rate)) Then
                                Label24.Text = GetFileSizeAlt(CInt(RemoveWhitespace(codec_new_bit_rate)))
                            Else
                                Label24.Text = "Not Detected"
                            End If
                        End If
                        ComboBox24.Enabled = True
                    Else
                        Dim bitValue As String
                        If codec_new_bit_rate = "N/A" Then
                            Dim codec_alt_bit_rate As String = RemoveWhitespace(getBetween(line, "bitrate:", "kb/s"))
                            If New Regex("^[0-9]+$").IsMatch(RemoveWhitespace(codec_alt_bit_rate)) Then
                                bitValue = RemoveWhitespace(codec_alt_bit_rate)
                            Else
                                bitValue = "Not Detected"
                            End If
                        Else
                            If New Regex("^[0-9]+$").IsMatch(RemoveWhitespace(codec_new_bit_rate)) Then
                                bitValue = GetFileSizeAlt(CInt(RemoveWhitespace(codec_new_bit_rate)))
                            Else
                                bitValue = "Not Detected"
                            End If
                        End If
                        VideoQueueCodecInf = codec_ln + ", " + RemoveWhitespace(codec_wi & "x" & codec_he) + " [" + codec_asp_ratio + "], " + bitValue + ", " + RemoveWhitespace(Strings.Left(codec_frame_rate, 2)) & " FPS"
                    End If
                End If
                abortSeeking = False
            Else
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
                abortSeeking = True
            End If
        End While
        process.WaitForExit()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete(HMEEngine & "HME_Video_Summary.bat")
    End Sub
    Private Sub AudioStream_Info(sender As Object, e As EventArgs) Handles ComboBox23.SelectedIndexChanged
        getAudioSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Textbox77.Text & Chr(34), (CInt(Strings.Mid(ComboBox23.Text.ToString, 11)) - 1).ToString)
    End Sub
    Public Sub getAudioSummary(ffmpegletter As String, ffmpegbin As String, audioFile As String, audioStream As String)
        Newffargs = "ffprobe -hide_banner " & " -show_streams -select_streams a:" & audioStream & " " & audioFile & " 2>&1 "
        HMEGenerate(HMEEngine & "HME_Audio_Summary.bat", ffmpegletter, ffmpegbin, Newffargs, "")
        Dim psi As New ProcessStartInfo(HMEEngine & "HME_Audio_Summary.bat") With {
           .RedirectStandardError = False,
           .RedirectStandardOutput = True,
           .CreateNoWindow = True,
           .WindowStyle = ProcessWindowStyle.Hidden,
           .UseShellExecute = False
       }
        Dim process As Process = Process.Start(psi)
        While Not process.StandardOutput.EndOfStream
            Dim line As String = process.StandardOutput.ReadToEnd()
            If line.Contains("Invalid data found when processing input") = False Then
                Dim codec_name As String = getBetween(line, "codec_name=", "codec_long_name")
                If RemoveWhitespace(codec_name) = "" Then
                    If AudioQueue = False Then
                        Label44.Text = "Not Detected"
                        Label39.Text = "Not Detected"
                        Label42.Text = "Not Detected"
                        Label35.Text = "Not Detected"
                        Label26.Text = "Not Detected"
                        Label32.Text = "Not Detected"
                        Label30.Text = "Not Detected"
                        Label61.Text = "Not Detected"
                        Label107.Text = "Not Detected"
                    End If
                Else
                    Dim codec_ln As String = getBetween(line, "codec_name=", "codec_long_name")
                    Dim codec_type As String = getBetween(line, "codec_type=", "codec_tag_string")
                    Dim codec_sample_fmt As String = getBetween(line, "sample_fmt=", "sample_rate")
                    Dim codec_sample_rate As String = getBetween(line, "sample_rate=", "channels")
                    Dim codec_channels As String = getBetween(line, "channels=", "channel_layout")
                    Dim codec_channels_layout As String = getBetween(line, "channel_layout=", "bits_per_sample")
                    Dim codec_bit_per_sample As String = getBetween(line, "bits_per_sample=", "initial_padding")
                    Dim codec_bit_rate As String = getBetween(line, "bit_rate=", "max_bit_rate")
                    Dim codec_profile As String = getBetween(line, "profile=", "codec_type")
                    If AudioQueue = False Then
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
                        Label35.Text = RemoveWhitespace(codec_sample_rate) & " Hz"
                        Label26.Text = codec_channels
                        Label32.Text = codec_channels_layout
                        Label30.Text = codec_bit_per_sample
                        Label107.Text = codec_profile
                        If RemoveWhitespace(codec_bit_rate).Equals("N/A") Then
                            Label61.Text = "Not Detected"
                        Else
                            If New Regex("^[0-9]+$").IsMatch(RemoveWhitespace(codec_bit_rate)) Then
                                Label61.Text = GetFileSizeAlt(CInt(RemoveWhitespace(codec_bit_rate)) / 1000)
                            Else
                                Label61.Text = "Not Detected"
                            End If
                        End If
                        ComboBox23.Enabled = True
                    Else
                        Dim QueueCodeBitDepth As String
                        Dim QueueCodecBitRate As String
                        Dim QueueCodecSampleRate As String
                        Dim QueueCodecChannels As String
                        Dim QueueCodecChannelsLayout As String
                        If RemoveWhitespace(codec_sample_fmt).Equals("s16") Then
                            QueueCodeBitDepth = "16 Bit, "
                        ElseIf RemoveWhitespace(codec_sample_fmt).Equals("s24") Then
                            QueueCodeBitDepth = "24 Bit, "
                        ElseIf RemoveWhitespace(codec_sample_fmt).Equals("s32") Then
                            QueueCodeBitDepth = "32 Bit, "
                        Else
                            If RemoveWhitespace(codec_sample_fmt) = "fltp" Then
                                QueueCodeBitDepth = "32 fp, "
                            Else
                                QueueCodeBitDepth = codec_sample_fmt + ", "
                            End If
                        End If
                        If RemoveWhitespace(codec_bit_rate).Equals("N/A") Then
                            QueueCodecBitRate = "N/A, "
                        Else
                            If New Regex("^[0-9]+$").IsMatch(codec_bit_rate) Then
                                QueueCodecBitRate = GetFileSizeAlt(CInt(RemoveWhitespace(codec_bit_rate) / 100))
                            Else
                                QueueCodecBitRate = "Not Detected"
                            End If
                        End If
                        If codec_sample_rate IsNot "" Then
                            QueueCodecSampleRate = ", " + RemoveWhitespace(CInt(RemoveWhitespace(codec_sample_rate) / 1000)) + " KHz, "
                        Else
                            QueueCodecSampleRate = ""
                        End If
                        If codec_channels IsNot "" Then
                            QueueCodecChannels = codec_channels + " channels "
                        Else
                            QueueCodecChannels = ""
                        End If
                        If codec_channels_layout IsNot "" Then
                            QueueCodecChannelsLayout = " (" + codec_channels_layout + ")"
                        Else
                            QueueCodecChannelsLayout = ""
                        End If
                        AudioQueueCodecInf = codec_ln + QueueCodecSampleRate + QueueCodecBitRate + QueueCodeBitDepth + QueueCodecChannels + QueueCodecChannelsLayout
                    End If
                End If
                abortSeeking = False
            Else
                Label44.Text = "Not Detected"
                Label39.Text = "Not Detected"
                Label42.Text = "Not Detected"
                Label35.Text = "Not Detected"
                Label26.Text = "Not Detected"
                Label32.Text = "Not Detected"
                Label30.Text = "Not Detected"
                Label61.Text = "Not Detected"
                Label107.Text = "Not Detected"
                abortSeeking = True
            End If
        End While
        process.WaitForExit()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete(HMEEngine & "HME_Audio_Summary.bat")
        If Label61.Text = "N/A" Then
            Newffargs = "ffprobe -hide_banner " & " -show_format -select_streams a:" & audioStream & " " & audioFile & " 2>&1 "
            HMEGenerate(HMEEngine & "HME_Audio_Summary.bat", ffmpegletter, ffmpegbin, Newffargs, "")
            Dim new_psi As New ProcessStartInfo(HMEEngine & "HME_Audio_Summary.bat") With {
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
                    If New Regex("^[0-9]+$").IsMatch(codec_bit_rate_alt) Then
                        Label61.Text = GetFileSizeAlt(CInt(RemoveWhitespace(codec_bit_rate_alt) / 100))
                    Else
                        Label61.Text = "Not Detected"
                    End If
                End If
            End While
            new_process.WaitForExit()
        End If
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete(HMEEngine & "HME_Audio_Summary.bat")
    End Sub
    Public Sub getDurationSummary(ffmpegletter As String, ffmpegbin As String, videoFile As String)
        Newffargs = "ffprobe -hide_banner -i " & videoFile & " 2>&1 "
        HMEGenerate(HMEEngine & "HME_Duration_Summary.bat", ffmpegletter, ffmpegbin, Newffargs, "")
        Dim psi As New ProcessStartInfo(HMEEngine & "HME_Duration_Summary.bat") With {
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
        process.WaitForExit()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete(HMEEngine & "HME_Duration_Summary.bat")
    End Sub
    Public Function getDurationSummaryAlt(ffmpegletter As String, ffmpegbin As String, videoFile As String) As String
        Dim value As String
        Newffargs = "ffprobe -hide_banner -i " & videoFile & " 2>&1 "
        HMEGenerate(HMEEngine & "HME_Duration_Summary.bat", ffmpegletter, ffmpegbin, Newffargs, "")
        Dim psi As New ProcessStartInfo(HMEEngine & "HME_Duration_Summary.bat") With {
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
            value = Strings.Left(codec_dur, 8)
        End While
        process.WaitForExit()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete(HMEEngine & "HME_Duration_Summary.bat")
        Return value
    End Function
    Private Async Sub getSpectrumSummary_Async()
        Dim loadInit = New Loading("Spectrum", Textbox77.Text)
        loadInit.Show()
        Dim curMediaDur As String() = Label80.Text.Split(":")
        Dim curMediaTime As Integer = TimeConversion(curMediaDur(0), curMediaDur(1), Strings.Left(curMediaDur(2), 2))
        If curMediaTime < 1800 Then
            If File.Exists(My.Application.Info.DirectoryPath & "\spectrum-temp.jpg") Then
                GC.Collect()
                GC.WaitForPendingFinalizers()
                File.Delete(My.Application.Info.DirectoryPath & "\spectrum-temp.jpg")
            End If
            Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & Textbox77.Text & Chr(34) & " -lavfi showspectrumpic=s=768x768:mode=separate " &
                                Chr(34) & My.Application.Info.DirectoryPath & "\spectrum-temp.jpg" & Chr(34)
            HMEGenerate(HMEEngine & "HME_Spectrum_Summary.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
            Dim generateSpectrum As New ProcessStartInfo(HMEEngine & "HME_Spectrum_Summary.bat") With {
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
            File.Delete(HMEEngine & "HME_Spectrum_Summary.bat")
        End If
        loadInit.Close()
    End Sub
    Public Async Sub getPreviewSummary_Async(ffmpegLetter As String, ffmpegBin As String, videoFile As String)
        Dim curMediaDur As String()
        If MetroSetSwitch6.Switched = True And DataGridView1.Rows.Count > 0 Then
            curMediaDur = DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex).Cells(6).Value.ToString.Split(":")
        Else
            curMediaDur = Label80.Text.Split(":")
        End If
        Dim curMediaTime As Integer = TimeConversion(curMediaDur(0), curMediaDur(1), Strings.Left(curMediaDur(2), 2))
        Dim loadInit = New Loading("Snapshots", Textbox77.Text)
        loadInit.Show()
        If Label5.Text.Equals("Not Detected") = False Then
            If RemoveWhitespace(Label5.Text).Equals("png") = True Then
                videoFile = Chr(34) & Textbox77.Text & Chr(34)
                Newffargs = "ffmpeg -hide_banner -i " & videoFile & " -an -vcodec copy " & Chr(34) & My.Application.Info.DirectoryPath & "\thumbnail\1.jpg"
                TotalScreenshot = 1
                HMEGenerate(HMEEngine & "HME_Image_Preview_Summary.bat", ffmpegLetter, ffmpegBin, Newffargs, "")
            Else
                If curMediaTime > 1800 Then
                    Newffargs = "ffmpeg -hide_banner -i " & videoFile & " -f image2 -vf " & Chr(34) & "select='not(mod(n,250))'" & Chr(34) & " -vframes 5 -vsync vfr " & Chr(34) & My.Application.Info.DirectoryPath & "\thumbnail\%%d.jpg" & Chr(34)
                    TotalScreenshot = 5
                    HMEGenerate(HMEEngine & "HME_Image_Preview_Summary.bat", ffmpegLetter, ffmpegBin, Newffargs, "")
                Else
                    Newffargs = "ffprobe -hide_banner -i " & videoFile & " -v error -select_streams v:0 -count_packets -show_entries stream=nb_read_packets -of csv=p=0"
                    HMEGenerate(HMEEngine & "HME_VF.bat", ffmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs, "")
                    FrameCount = "0"
                    Dim generateFrame As New ProcessStartInfo(HMEEngine & "HME_VF.bat") With {
                                        .RedirectStandardError = False,
                                        .RedirectStandardOutput = True,
                                        .CreateNoWindow = True,
                                        .WindowStyle = ProcessWindowStyle.Hidden,
                                        .UseShellExecute = False
                    }
                    Dim process As Process = Process.Start(generateFrame)
                    Do
                        Dim line As String = process.StandardOutput.ReadLine
                        If New Regex("^[0-9]+$").IsMatch(line) Then
                            FrameCount = line
                        Else
                            FrameCount = 0
                        End If
                    Loop Until Await Task.Run(Function() process.StandardOutput.EndOfStream) Or FrameCount > 0
                    Await Task.Delay(1500)
                    Await Task.Run(Sub() process.WaitForExit())
                    If FrameCount >= 50 Then
                        Newffargs = "ffmpeg -hide_banner -i " & videoFile & " -f image2 -vf " & Chr(34) & "select='not(mod(n," & CInt(FrameCount / 50) & "))'" & Chr(34) & " -vframes 5 -vsync vfr " & Chr(34) & My.Application.Info.DirectoryPath & "\thumbnail\%%d.jpg" & Chr(34)
                        TotalScreenshot = 5
                        HMEGenerate(HMEEngine & "HME_Image_Preview_Summary.bat", ffmpegLetter, ffmpegBin, Newffargs, "")
                    ElseIf FrameCount < 50 Then
                        Newffargs = "ffmpeg -hide_banner -i " & videoFile & " -ss 00:00:00.000 -vframes 1 -f image2 " & Chr(34) & My.Application.Info.DirectoryPath & "\thumbnail\1.jpg" & Chr(34)
                        Newffargs2 = "ffmpeg -hide_banner -i " & videoFile & " -ss 00:00:01.000 -vframes 1 -f image2 " & Chr(34) & My.Application.Info.DirectoryPath & "\thumbnail\2.jpg" & Chr(34)
                        TotalScreenshot = 2
                        HMEGenerate(HMEEngine & "HME_Image_Preview_Summary.bat", ffmpegLetter, ffmpegBin, Newffargs, Newffargs2)
                    Else
                        TotalScreenshot = 0
                    End If
                End If
            End If
            Label96.Text = 1
            Label98.Text = 0
            If Newffargs IsNot "" Then
                Dim generateSnapshots As New ProcessStartInfo(HMEEngine & "HME_Image_Preview_Summary.bat") With {
                                        .RedirectStandardError = False,
                                        .RedirectStandardOutput = False,
                                        .CreateNoWindow = True,
                                        .WindowStyle = ProcessWindowStyle.Hidden,
                                        .UseShellExecute = False
                                    }
                Dim new_process As Process = Process.Start(generateSnapshots)
                Await Task.Delay(1500)
                Await Task.Run(Sub() new_process.WaitForExit())
                If File.Exists(My.Application.Info.DirectoryPath & "\thumbnail\1.jpg") Then
                    Dim ImgPrev1 As New FileStream(My.Application.Info.DirectoryPath & "\thumbnail\1.jpg", FileMode.Open, FileAccess.Read)
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
            videoFile = Chr(34) & Textbox77.Text & Chr(34)
            Newffargs = "ffmpeg -hide_banner -i " & videoFile & " -an -vcodec copy " & Chr(34) & My.Application.Info.DirectoryPath & "\thumbnail\1.jpg"
            HMEGenerate(HMEEngine & "HME_Audio_Only_Summary.bat", ffmpegLetter, FfmpegConf, Newffargs, "")
            RunProcAsync(HMEEngine & "HME_Audio_Only_Summary.bat")
            If File.Exists(My.Application.Info.DirectoryPath & "\thumbnail\1.jpg") Then
                Dim ImgPrev1 As New FileStream(My.Application.Info.DirectoryPath & "\thumbnail\1.jpg", FileMode.Open, FileAccess.Read)
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
        File.Delete(HMEEngine & "HME_Audio_Only_Summary.bat")
        File.Delete(HMEEngine & "HME_Image_Preview_Summary.bat")
        File.Delete(HMEEngine & "HME_VF.bat")
        Button1.Enabled = True
        Button3.Enabled = True
        Button4.Enabled = True
        loadInit.Close()
    End Sub
    Public Sub getStreamSummary(ffmpegletter As String, ffmpegbin As String, videoFile As String, ffmpegMode As String, ffmpegHybMode As String)
        Newffargs = "ffmpeg -hide_banner -i " & videoFile & " 2>&1 "
        HMEGenerate(HMEEngine & "HME_Stream_Summary.bat", ffmpegletter, ffmpegbin, Newffargs, "")
        Dim psi As New ProcessStartInfo(HMEEngine & "\HME_Stream_Summary.bat") With {
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
        File.Delete(HMEEngine & "HME_Stream_Summary.bat")
        Dim start As Integer
        Dim videoRegex As New Regex(": Video:", RegexOptions.IgnoreCase Or RegexOptions.Singleline)
        Dim audioRegex As New Regex(": Audio:", RegexOptions.IgnoreCase Or RegexOptions.Singleline)
        Dim videoMatches As MatchCollection = videoRegex.Matches(StreamInfo)
        Dim audioMatches As MatchCollection = audioRegex.Matches(StreamInfo)
        If ffmpegMode = "Encoding" Then
            MetroSetComboBox3.Items.Clear()
            ComboBox23.Items.Clear()
            ComboBox24.Items.Clear()
            ComboBox25.Items.Clear()
            MetroSetComboBox1.Items.Clear()
            If Label5.Text.Equals("Not Detected") = False Then
                For start = 1 To videoMatches.Count
                    ComboBox24.Items.Add("Stream #0:" & start)
                    MetroSetComboBox1.Items.Add("Stream #0:" & start)
                Next
            End If
            If Label44.Text.Equals("Not Detected") = False Then
                For start = 1 To audioMatches.Count
                    MetroSetComboBox3.Items.Add("Stream #0:" & start)
                    ComboBox25.Items.Add("Stream #0:" & start)
                    ComboBox23.Items.Add("Stream #0:" & start)
                Next
            End If
        ElseIf ffmpegMode = "Muxing" Then
            MetroSetComboBox3.Items.Clear()
            ComboBox25.Items.Clear()
            MetroSetComboBox1.Items.Clear()
            For start = 1 To videoMatches.Count
                MetroSetComboBox1.Items.Add("Stream #0:" & start)
            Next
            For start = 1 To audioMatches.Count
                ComboBox25.Items.Add("Stream #0:" & start)
                MetroSetComboBox3.Items.Add("Stream #0:" & start)
            Next
        ElseIf ffmpegMode = "Muxing + Custom" Then
            MetroSetComboBox3.Items.Clear()
            ComboBox25.Items.Clear()
            MetroSetComboBox1.Items.Clear()
            For start = 1 To videoMatches.Count
                MetroSetComboBox1.Items.Add("Stream #0:" & start)
            Next
            For start = 1 To audioMatches.Count + 1
                MetroSetComboBox3.Items.Add("Stream #0:" & start)
                ComboBox25.Items.Add("Stream #0:" & start)
            Next
        ElseIf ffmpegMode = "Trim" Then
            ComboBox27.Items.Clear()
            MetroSetComboBox3.Items.Clear()
            MetroSetComboBox1.Items.Clear()
            If ComboBox28.SelectedIndex = 0 Then
                If Label5.Text.Equals("Not Detected") = False Then
                    For start = 1 To videoMatches.Count
                        MetroSetComboBox1.Items.Add("Stream #0:" & start)
                        ComboBox27.Items.Add("Stream #0:" & start)
                    Next
                End If
            Else
                If Label5.Text.Equals("Not Detected") = False Then
                    For start = 1 To videoMatches.Count
                        MetroSetComboBox1.Items.Add("Stream #0:" & start)
                    Next
                End If
                If Label44.Text.Equals("Not Detected") = False Then
                    For start = 1 To audioMatches.Count
                        MetroSetComboBox3.Items.Add("Stream #0:" & start)
                        ComboBox27.Items.Add("Stream #0:" & start)
                    Next
                End If
            End If
        ElseIf ffmpegMode = "Queue" Then
            If ffmpegHybMode = "LoadMedia" Then
                MetroSetComboBox3.Items.Clear()
                ComboBox23.Items.Clear()
                ComboBox24.Items.Clear()
                MetroSetComboBox1.Items.Clear()
                If Label5.Text.Equals("Not Detected") = False Then
                    For start = 1 To videoMatches.Count
                        ComboBox24.Items.Add("Stream #0:" & start)
                        MetroSetComboBox1.Items.Add("Stream #0:" & start)
                    Next
                End If
                If Label44.Text.Equals("Not Detected") = False Then
                    For start = 1 To audioMatches.Count
                        MetroSetComboBox3.Items.Add("Stream #0:" & start)
                        ComboBox23.Items.Add("Stream #0:" & start)
                    Next
                End If
            Else
                MetroSetComboBox3.Items.Clear()
                ComboBox23.Items.Clear()
                ComboBox24.Items.Clear()
                MetroSetComboBox1.Items.Clear()
                MetroSetComboBox3.Items.Add("Stream #0:0")
                ComboBox23.Items.Add("Stream #0:0")
                ComboBox24.Items.Add("Stream #0:0")
                MetroSetComboBox1.Items.Add("Stream #0:0")
            End If
            NotifyIcon("Hana Media Encoder", "Multi video and audio stream configuration are not supported when media queue is enable !", 1000, True)
        End If
    End Sub
    Private Async Sub StartEncode_SQ() Handles SingleEncodeToolStripMenuItem.Click
        Button1.Enabled = False
        Button3.Enabled = False
        Button6.Enabled = False
        AddEncConf = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Encode Info:")
        AltEncodeConf = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Alt Encode:")
        DebugMode = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Debug Mode:")
        FrameConfig = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Frame Count:")
        If Hwdefconfig IsNot "null" And Hwdefconfig.Remove(0, 11) IsNot "" Then
            If DebugMode.Equals("null") = False Then
                Newdebugmode = DebugMode.Remove(0, 11)
                If FrameConfig.Equals("null") = False Then
                    FrameMode = FrameConfig.Remove(0, 12)
                Else
                    FrameMode = "False"
                End If
            Else
                Newdebugmode = "False"
                FrameMode = "False"
            End If
            If Newdebugmode = "True" Then
                NotifyIcon("Debug mode was actived !", "Progressbar will not working correctly while encoding", 1000, True)
            End If
            If TextBox1.Text IsNot "" Then
                If CheckBox11.Checked = False And MetroSetSwitch4.Switched = True Then
                    VideoFilePath = TextBox15.Text.ToString
                Else
                    VideoFilePath = Textbox77.Text.ToString
                End If
                If MetroSetSwitch1.Switched = False And MetroSetSwitch2.Switched = False And MetroSetSwitch3.Switched = False And MetroSetSwitch4.Switched = False And
                    MetroSetSwitch5.Switched = False And MetroSetSwitch6.Switched = False Then
                    EncPass1 = False
                    NotifyIcon("Media file has failed to encode", "Video or audio codec configuration is not valid", 1000, False)
                Else
                    If Textbox77.Text IsNot "" Then
                        Dim getCurrentVideoCodec As String = MetroSetComboBox3.Text.ToLower
                        Dim getCurrentAudioCodec As String = ComboBox15.Text.ToLower
                        If MetroSetSwitch1.Switched = True Then
                            If VideoCodecLock() = True Then
                                If MetroSetSwitch2.Switched = True Then
                                    If AudioCodecLock() = True Then
                                        If OrigSaveExt.Equals(".mp4") = True Then
                                            If getCurrentAudioCodec.Equals("copy") = True Then
                                                If Label44.Text.ToString.ToLower().Equals("aac") = True Or Label44.Text.ToString.ToLower().Equals("mp2") = True Then
                                                    EncPass1 = True
                                                Else
                                                    EncPass1 = False
                                                End If
                                            ElseIf getCurrentAudioCodec.Equals("aac") = True And Label44.Text.ToString.ToLower().Equals("aac") = True Or
                                                 getCurrentAudioCodec.Equals("aac") = True Or getCurrentAudioCodec.Equals("mp2") = True Then
                                                EncPass1 = True
                                            Else
                                                EncPass1 = False
                                            End If
                                        Else
                                            EncPass1 = True
                                        End If
                                    Else
                                        EncPass1 = False
                                    End If
                                ElseIf MetroSetSwitch2.Switched = False Then
                                    If OrigSaveExt.Equals(".mp4") = True Or OrigSaveExt.Equals(".mkv") = True Then
                                        EncPass1 = True
                                    Else
                                        EncPass1 = False
                                    End If
                                Else
                                    EncPass1 = True
                                End If
                            Else
                                EncPass1 = False
                            End If
                        Else
                            If MetroSetSwitch2.Switched = True Then
                                If AudioCodecLock() = True Then
                                    If getCurrentAudioCodec.Equals(OrigSaveExt) = False Then
                                        If getCurrentAudioCodec.Equals("aac") = True Then
                                            TextBox1.Text = OrigSavePath & "\" & OrigSaveName & ".m4a"
                                        ElseIf getCurrentAudioCodec.Equals("copy") = True Then
                                            TextBox1.Text = OrigSavePath & "\" & OrigSaveName & "." & OrigSaveExt
                                        Else Newffargs = "ffmpeg -hide_banner " & HwAccelFormat
                                            TextBox1.Text = OrigSavePath & "\" & OrigSaveName & "." & getCurrentAudioCodec
                                        End If
                                    End If
                                    EncPass1 = True
                                Else
                                    EncPass1 = False
                                End If
                            Else
                                EncPass1 = True
                            End If
                        End If
                        If EncPass1 = True Then
                            If MetroSetSwitch3.Switched = True Then
                                If ChapterLock() = True Then
                                    HMEStreamProfileGenerate(ChapterStreamConfigPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt", ChapterFlags)
                                    If File.Exists(ChapterStreamConfigPath & "\FFMETADATAFILE") Then
                                        If MetroSetSwitch1.Switched = True Then
                                            If VideoCodecLock() = True Then
                                                If MetroSetSwitch2.Switched = True Then
                                                    If AudioCodecLock() = True Then
                                                        If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") And File.Exists(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") Then
                                                            FlagsCount = MetroSetComboBox3.Items.Count
                                                            If FlagsCount > 1 Then
                                                                For FlagsStart = 1 To FlagsCount
                                                                    My.Computer.FileSystem.WriteAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_" & (FlagsStart - 1).ToString & ".txt")), True)
                                                                Next
                                                                If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt") Then
                                                                    Dim joinAudio As String = File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt")
                                                                    Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & ChapterFlags & " " & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                                End If
                                                            Else
                                                                Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & ChapterFlags & " " & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                            End If
                                                            HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                                            encpass2 = True
                                                        Else
                                                            NotifyIcon("Media file has failed to encode", "Video or audio codec stream configuration not found", 1000, False)
                                                            encpass2 = False
                                                        End If
                                                    Else
                                                        encpass2 = False
                                                    End If
                                                ElseIf MetroSetSwitch2.Switched = False Then
                                                    Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & ChapterFlags & " " & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & "  -an " & Chr(34) & TextBox1.Text & Chr(34)
                                                    HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                                    encpass2 = True
                                                End If
                                            Else
                                                encpass2 = False
                                            End If
                                        Else
                                            Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & ChapterFlags & " -c copy " & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & Chr(34) & TextBox1.Text & Chr(34)
                                            HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                            encpass2 = True
                                        End If
                                    Else
                                        NotifyIcon("Media file has failed to encode", "Please re-lock chapter profile", 1000, False)
                                        encpass2 = False
                                    End If
                                Else
                                    encpass2 = False
                                End If
                            ElseIf MetroSetSwitch4.Switched = True Then
                                If MuxLock() = True Then
                                    HMEStreamProfileGenerate(MuxStreamConfigPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt", MuxFlags)
                                    If ComboBox1.Text = "Original Quality" Then
                                        Newffargs = "ffmpeg -hide_banner " & MuxFlags & " " & Chr(34) & TextBox1.Text & Chr(34)
                                        HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                        encpass2 = True
                                    ElseIf ComboBox1.Text = "Custom Quality" Then
                                        If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") And File.Exists(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath & "_flags_0.txt")) Then
                                            FlagsCount = MetroSetComboBox3.Items.Count
                                            If FlagsCount > 1 Then
                                                For FlagsStart = 1 To FlagsCount
                                                    My.Computer.FileSystem.WriteAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_" & (FlagsStart - 1).ToString & ".txt")), True)
                                                Next
                                                If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt") Then
                                                    Dim joinAudio As String = File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt")
                                                    Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & MuxFlags & " " & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                End If
                                            Else
                                                Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & MuxFlags & " " & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " " & Chr(34) & TextBox1.Text & Chr(34)
                                            End If
                                            HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                            encpass2 = True
                                        Else
                                            NotifyIcon("Media file has failed to encode", "Video or audio codec stream configuration not found", 1000, False)
                                            encpass2 = False
                                        End If
                                    End If
                                Else
                                    encpass2 = False
                                End If
                            ElseIf MetroSetSwitch5.Switched = True Then
                                If TrimLock() = True Then
                                    HMEStreamProfileGenerate(TrimStreamConfigPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt", TrimFlags)
                                    If ComboBox26.Text = "Original Quality" Then
                                        If ComboBox28.Text = "Video Only" Then
                                            Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0:0 -c:v:0 copy -an -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                        ElseIf ComboBox28.Text = "Video + Audio (Specific source)" Then
                                            Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0:0 -map 0:" & CInt(Strings.Mid(ComboBox27.Text.ToString, 11)).ToString & " -c copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                        ElseIf ComboBox28.Text = "Video + Audio (All source)" Then
                                            Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0 -c copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                        ElseIf ComboBox28.Text = "Audio Only (Specific Source)" Then
                                            If Label5.Text.Equals("Not Detected") = False Then
                                                Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0:" & CInt(Strings.Mid(ComboBox27.Text.ToString, 11)).ToString & " -vn -c:a:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & " copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                            Else
                                                Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & " -c:a:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11))).ToString & " copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                            End If
                                        Else
                                            Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0 -c copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                        End If
                                        HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                        encpass2 = True
                                    ElseIf ComboBox26.Text = "Custom Quality" Then
                                        If ComboBox28.Text = "Video Only" Then
                                            Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0:0 " & File.ReadAllText(VideoStreamFlagsPath & "\" & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " -an " & Chr(34) & TextBox1.Text & Chr(34)
                                            HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                            encpass2 = True
                                        ElseIf ComboBox28.Text = "Video + Audio (Specific source)" Then
                                            If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt") And File.Exists(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") Then
                                                FlagsCount = MetroSetComboBox3.Items.Count
                                                If FlagsCount > 1 Then
                                                    For FlagsStart = 1 To FlagsCount
                                                        My.Computer.FileSystem.WriteAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_" & (FlagsStart - 1).ToString & ".txt")), True)
                                                    Next
                                                    If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt") Then
                                                        Dim joinAudio As String = File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt")
                                                        Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0:0 -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11))).ToString & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                    End If
                                                Else
                                                    Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0:0 -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11))).ToString & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                End If
                                                HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                                encpass2 = True
                                            Else
                                                NotifyIcon("Media file has failed to encode", "Video or audio codec stream configuration not found", 1000, False)
                                                encpass2 = False
                                            End If
                                        ElseIf ComboBox28.Text = "Video + Audio (All source)" Then
                                            If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt") And File.Exists(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") Then
                                                FlagsCount = MetroSetComboBox3.Items.Count
                                                If FlagsCount > 1 Then
                                                    For FlagsStart = 1 To FlagsCount
                                                        My.Computer.FileSystem.WriteAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_" & (FlagsStart - 1).ToString & ".txt")), True)
                                                    Next
                                                    If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt") Then
                                                        Dim joinAudio As String = File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt")
                                                        Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0 " & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                    End If
                                                Else
                                                    Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0 " & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                End If
                                                HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                                encpass2 = True
                                            Else
                                                NotifyIcon("Media file has failed to encode", "Video or audio codec stream configuration not found", 1000, False)
                                                encpass2 = False
                                            End If
                                        ElseIf ComboBox28.Text = "Audio Only (Specific Source)" Then
                                            If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt") Then
                                                FlagsCount = MetroSetComboBox3.Items.Count
                                                If FlagsCount > 1 Then
                                                    For FlagsStart = 1 To FlagsCount
                                                        My.Computer.FileSystem.WriteAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_" & (FlagsStart - 1).ToString & ".txt")), True)
                                                    Next
                                                    If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt") Then
                                                        Dim joinAudio As String = File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt")
                                                        If Label5.Text.Equals("Not Detected") = False Then
                                                            Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11))).ToString & " -vn " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                        Else
                                                            Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                        End If
                                                    End If
                                                Else
                                                    If Label5.Text.Equals("Not Detected") = False Then
                                                        Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11))).ToString & " -vn " & File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                    Else
                                                        Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " " & TrimFlags & " -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                    End If
                                                End If
                                                HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                                encpass2 = True
                                            Else
                                                NotifyIcon("Media file has failed to encode", "Audio codec stream configuration not found", 1000, False)
                                                encpass2 = False
                                            End If
                                        Else
                                            If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") And File.Exists(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") Then
                                                FlagsCount = MetroSetComboBox3.Items.Count
                                                If FlagsCount > 1 Then
                                                    For FlagsStart = 1 To FlagsCount
                                                        My.Computer.FileSystem.WriteAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_" & (FlagsStart - 1).ToString & ".txt")), True)
                                                    Next
                                                    If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt") Then
                                                        Dim joinAudio As String = File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt")
                                                        Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & TrimFlags & " -map 0 " & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                    End If
                                                Else
                                                    Newffargs = Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & TrimFlags & " -map 0 " & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                End If
                                                HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                                encpass2 = True
                                            Else
                                                NotifyIcon("Media file has failed to encode", "Video or audio codec stream configuration not found", 1000, False)
                                                encpass2 = False
                                            End If
                                        End If
                                    End If
                                Else
                                    encpass2 = False
                                End If
                            ElseIf MetroSetSwitch1.Switched = True Then
                                If VideoCodecLock() = True Then
                                    If MetroSetSwitch2.Switched = True Then
                                        If AudioCodecLock() = True Then
                                            If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") And File.Exists(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") Then
                                                If FlagsCount > 1 Then
                                                    For FlagsStart = 1 To FlagsCount
                                                        My.Computer.FileSystem.WriteAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_" & (FlagsStart - 1).ToString & ".txt")), True)
                                                    Next
                                                    If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt") Then
                                                        Dim joinAudio As String = File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt")
                                                        If mediaenginestate = "NVENCC" Then
                                                            Newffargs = "nvencc64 -i " & Chr(34) & VideoFilePath & Chr(34) & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & joinAudio & " -o " & Chr(34) & TextBox1.Text & Chr(34)
                                                        Else
                                                            Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                        End If
                                                    End If
                                                Else
                                                    If mediaenginestate = "NVENCC" Then
                                                        Newffargs = "nvencc64 -i " & Chr(34) & VideoFilePath & Chr(34) & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " -o " & Chr(34) & TextBox1.Text & Chr(34)
                                                    Else
                                                        Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                    End If
                                                End If
                                                If mediaenginestate = "NVENCC" Then
                                                    HMEGenerate(HMEEngine & "HME.bat", NVENCCLetter, Chr(34) & NVENCCConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                                Else
                                                    HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                                End If
                                                encpass2 = True
                                            Else
                                                NotifyIcon("Media file has failed to encode", "Video or audio codec stream configuration not found", 1000, False)
                                                encpass2 = False
                                            End If
                                        Else
                                            encpass2 = False
                                        End If
                                    ElseIf MetroSetSwitch2.Switched = False Then
                                        If mediaenginestate = "NVENCC" Then
                                            NotifyIcon("Media file has failed to encode", "NVENCC are not supported to encode video only !", 1000, False)
                                            encpass2 = False
                                        Else
                                            Newffargs = "ffmpeg -hide_banner " & HwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & File.ReadAllText(VideoStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & "  -an " & Chr(34) & TextBox1.Text & Chr(34)
                                            HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                            encpass2 = True
                                        End If
                                    End If
                                Else
                                    encpass2 = False
                                End If
                            ElseIf MetroSetSwitch1.Switched = False Then
                                If MetroSetSwitch2.Switched = True Then
                                    If AudioCodecLock() = True Then
                                        If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") Then
                                            If FlagsCount > 1 Then
                                                For FlagsStart = 1 To FlagsCount
                                                    My.Computer.FileSystem.WriteAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_" & (FlagsStart - 1).ToString & ".txt")), True)
                                                Next
                                                If File.Exists(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt") Then
                                                    Dim joinAudio As String = File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_join.txt")
                                                    Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & VideoFilePath & Chr(34) & " -vn " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                End If
                                            Else
                                                Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & VideoFilePath & Chr(34) & " -vn " & File.ReadAllText(AudioStreamFlagsPath & Path.GetFileName(VideoFilePath) & "_flags_0.txt") & " " & Chr(34) & TextBox1.Text & Chr(34)
                                            End If
                                            If mediaenginestate = "NVENCC" Then
                                                NotifyIcon("Media file has failed to encode", "NVENCC are not supported to encode audio only !", 1000, False)
                                                encpass2 = False
                                            Else
                                                HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                                encpass2 = True
                                            End If
                                        End If
                                    Else
                                        NotifyIcon("Media file has failed to encode", "Video or audio codec stream configuration not found", 1000, False)
                                        encpass2 = False
                                    End If
                                End If
                            Else
                                NotifyIcon("Media file has failed to encode", "Please lock configuration menu before encode", 1000, False)
                                encpass2 = False
                            End If
                        Else
                            NotifyIcon("Media file has failed to encode", "Incompatibility codec configuration with new media file container found", 1000, False)
                            encpass2 = False
                        End If
                    Else
                        NotifyIcon("Media file has failed to encode", "Please open media file before encode", 1000, False)
                        encpass2 = False
                    End If
                End If
            Else
                NotifyIcon("Media file has failed to encode", "Please open media file before encode", 1000, False)
                encpass2 = False
            End If
            If encpass2 = True Then
                If File.Exists(HMEEngine & "HME.bat") Then
                    If File.Exists(TextBox1.Text) Then
                        Dim duplicateFile As DialogResult = MessageBoxAdv.Show(Me, "Selected file location to save as new media file already exists" & vbCrLf &
                                                                                           vbCrLf & "Replace that file ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If duplicateFile = DialogResult.Yes Then
                            GC.Collect()
                            GC.WaitForPendingFinalizers()
                            File.Delete(TextBox1.Text)
                        Else
                            NotifyIcon("Media file has failed to encode", "Please select other location or other file name before encode", 1000, False)
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
                            FrameCount = 0
                        Else
                            If Label5.Text.Equals("Not Detected") = True Or MetroSetSwitch1.Switched = False Then
                                If MetroSetSwitch5.Switched = True Then
                                    FrameCount = TrimEndTime - TrimStartTime
                                Else
                                    Dim TimeFrame As String() = Label81.Text.Split(":")
                                    FrameCount = TimeConversion(TimeFrame(0), TimeFrame(1), TimeFrame(2))
                                End If
                            Else
                                Dim loadInit = New Loading("Frame", Textbox77.Text)
                                loadInit.Show()
                                FrameCount = "0"
                                Newffargs = "ffprobe -hide_banner -i " & Chr(34) & VideoFilePath & Chr(34) & " -v error -select_streams v:0 -count_packets -show_entries stream=nb_read_packets -of csv=p=0"
                                HMEGenerate(HMEEngine & "HME_VF.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs, "")
                                Dim psi As New ProcessStartInfo(HMEEngine & "HME_VF.bat") With {
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
                                    If line IsNot "" Then
                                        If RemoveWhitespace(line) IsNot "" Then
                                            If RemoveWhitespace(line).Contains(",") Then
                                                FrameCount = line.Replace(",", "")
                                            Else
                                                FrameCount = line
                                            End If
                                        End If
                                    End If
                                Loop Until Await Task.Run(Function() process.StandardOutput.EndOfStream)
                                Await Task.Run(Sub() process.WaitForExit())
                                loadInit.Close()
                            End If
                        End If
                        Label28.Text = "Encoding"
                        EncStartTime = DateTime.Now
                        ProgressBarAdv1.Minimum = 0
                        ProgressBarAdv1.Maximum = FrameCount
                        Dim new_psi As New ProcessStartInfo(HMEEngine & "HME.bat") With {
                                                    .RedirectStandardError = True,
                                                    .RedirectStandardOutput = False,
                                                    .CreateNoWindow = True,
                                                    .WindowStyle = ProcessWindowStyle.Hidden,
                                                    .UseShellExecute = False
                                    }
                        If AddEncConf IsNot "null" Then
                            AddEncTrimConf = AddEncConf.Remove(0, 12)
                            If RemoveWhitespace(AddEncTrimConf) = "none" Then
                                AddEncPassConf = "False"
                                ProgressBarAdv1.TextVisible = False
                            ElseIf RemoveWhitespace(AddEncTrimConf) = "advanced" Then
                                AddEncPassConf = "adv"
                                ProgressBarAdv1.TextVisible = True
                            ElseIf RemoveWhitespace(AddEncTrimConf) = "percentage" Then
                                AddEncPassConf = "per"
                                ProgressBarAdv1.TextVisible = True
                            Else
                                AddEncPassConf = "False"
                                ProgressBarAdv1.TextVisible = False
                            End If
                        Else
                            AddEncPassConf = "False"
                            ProgressBarAdv1.TextVisible = False
                        End If
                        If AltEncodeConf IsNot "null" Then
                            AltEncodeTrimConf = AltEncodeConf.Remove(0, 11)
                        Else
                            AltEncodeTrimConf = "False"
                        End If
                        If Newdebugmode = "True" Then
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
                        ElseIf Newdebugmode = "False" Then
                            Dim new_process As Process = Process.Start(new_psi)
                            Do
                                Dim lineReader As StreamReader = Await Task.Run(Function() new_process.StandardError)
                                Dim line As String = Await Task.Run(Function() lineReader.ReadLineAsync)
                                Dim encAudioFrame As String()
                                If mediaenginestate = "NVENCC" Then
                                    ProgressBarAdv1.Minimum = 0
                                    ProgressBarAdv1.Maximum = 100
                                    ProgressBarAdv1.CustomText = "Encoding: " + getBetween(line, "[", "%]") + "%" + " GPU Load:" + getBetween(line, "GPU", "%, est") + "% Estimated size: " + getBetween(line, "size", "MB") + "MB" + " Estimated Time:" + getBetween(line, "remain", ", GPU")
                                    ProgressBarAdv1.TextStyle = ProgressBarTextStyles.Custom
                                    If RemoveWhitespace(getBetween(line, "[", "%]").Length) > 0 And RemoveWhitespace(getBetween(line, "[", "%]").Length) < 4 Then
                                        ProgressBarAdv1.Value = CInt(RemoveWhitespace(getBetween(line, "[", "%]").Substring(0, RemoveWhitespace(getBetween(line, "[", "%]").Length - 1))))
                                    ElseIf RemoveWhitespace(getBetween(line, "[", "%]").Length) = 4 Then
                                        ProgressBarAdv1.Value = CInt(RemoveWhitespace(getBetween(line, "[", "%]").Substring(0, RemoveWhitespace(getBetween(line, "[", "%]").Length - 2))))
                                    Else
                                        ProgressBarAdv1.Value = 0
                                    End If
                                Else
                                    If Label5.Text.Equals("Not Detected") = True Or MetroSetSwitch1.Switched = False Then
                                        If RemoveWhitespace(getBetween(line, "time=", " bitrate")).Equals("") = False Then
                                            If RemoveWhitespace(getBetween(line, "time=", " bitrate")) <= FrameCount Then
                                                encAudioFrame = RemoveWhitespace(getBetween(line, "time=", " bitrate")).Split(":")
                                                If AddEncPassConf = "adv" Then
                                                    ProgressBarAdv1.CustomText = "Encoding speed: " + getBetween(line, "speed=", "x") + "x" + " Estimated size: " +
                                                                                               getBetween(line, "size=", "kiB") + "kB" + " "
                                                    ProgressBarAdv1.TextStyle = ProgressBarTextStyles.Custom
                                                ElseIf AddEncPassConf = "per" Then
                                                    ProgressBarAdv1.TextStyle = ProgressBarTextStyles.Percentage
                                                End If
                                                ProgressBarAdv1.Value = CInt(TimeConversion(encAudioFrame(0), encAudioFrame(1), Strings.Left(encAudioFrame(2), 2)))
                                            End If
                                        Else
                                            FfmpegEncStats = "Frame Error!"
                                        End If
                                    Else
                                        If AltEncodeTrimConf = "True" Then
                                            If RemoveWhitespace(getBetween(line, "time=", " bitrate")).Equals("") = False Then
                                                If RemoveWhitespace(getBetween(line, "time=", " bitrate")) <= FrameCount Then
                                                    encAudioFrame = RemoveWhitespace(getBetween(line, "time=", " bitrate")).Split(":")
                                                    If AddEncPassConf = "adv" Then
                                                        ProgressBarAdv1.CustomText = "Frame time: " + getBetween(line.ToString(), "time=", "bitrate") + "Encoding speed: " +
                                                                                         getBetween(line, "speed=", "x") + "x" + " Estimated size: " + getBetween(line, "size=", "KiB") + "kB" + " "
                                                        ProgressBarAdv1.TextStyle = ProgressBarTextStyles.Custom
                                                    ElseIf AddEncPassConf = "per" Then
                                                        ProgressBarAdv1.TextStyle = ProgressBarTextStyles.Percentage
                                                    End If
                                                    ProgressBarAdv1.Value = CInt(TimeConversion(encAudioFrame(0), encAudioFrame(1), Strings.Left(encAudioFrame(2), 2)))
                                                End If
                                            Else
                                                FfmpegEncStats = "Frame Error!"
                                            End If
                                        Else
                                            If RemoveWhitespace(getBetween(line, "frame= ", " fps")) = "" Or RemoveWhitespace(getBetween(line, "frame= ", " fps")) = "0" Then
                                                FfmpegEncStats = "Frame Error!"
                                            ElseIf RemoveWhitespace(getBetween(line, "frame= ", " fps")) <= FrameCount Then
                                                If AddEncPassConf = "adv" Then
                                                    ProgressBarAdv1.CustomText = "Frame time: " + getBetween(line.ToString(), "time=", "bitrate") + "Encoding speed: " +
                                                                                         getBetween(line, "speed=", "x") + "x" + " Estimated size: " + getBetween(line, "size=", "KiB") + "kB" + " "
                                                    ProgressBarAdv1.TextStyle = ProgressBarTextStyles.Custom
                                                ElseIf AddEncPassConf = "per" Then
                                                    ProgressBarAdv1.TextStyle = ProgressBarTextStyles.Percentage
                                                End If
                                                ProgressBarAdv1.Value = CInt(RemoveWhitespace(getBetween(line, "frame=", " fps")))
                                            End If
                                        End If
                                    End If
                                End If

                            Loop Until Await Task.Run(Function() new_process.StandardError.EndOfStream)
                            Await Task.Run(Sub() new_process.WaitForExit())
                        Else
                            NotifyIcon("Media file has failed to encode", "Unknown error", 1000, False)
                        End If
                        ProgressBarAdv1.TextVisible = False
                        EncEndTime = DateTime.Now
                        If File.Exists(TextBox1.Text) Then
                            Dim destFile As New FileInfo(TextBox1.Text)
                            If destFile.Length / 1024 / 1024 < 1.0 Then
                                If destFile.Length / 1024 < 1.0 Then
                                    If Newdebugmode = "True" Then
                                        MessageBoxAdv.Show("Media File has failed to encoded !" & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error, FfmpegErr)
                                    Else
                                        NotifyIcon("Media File has failed to encoded !", "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), 1000, False)
                                    End If
                                    Label28.Text = "Error"
                                    ProgressBarAdv1.Value = ProgressBarAdv1.Maximum
                                Else
                                    If ProgressBarAdv1.Value <> ProgressBarAdv1.Maximum Then
                                        ProgressBarAdv1.Value = ProgressBarAdv1.Maximum
                                    End If
                                    If Newdebugmode = "True" Then
                                        MessageBoxAdv.Show("Media File has successfuly to encoded !" & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information, FfmpegErr)
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
                                    MessageBoxAdv.Show("Media File has successfuly to encoded !" & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information, FfmpegErr)
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
                            If Newdebugmode = "True" Then
                                MessageBoxAdv.Show("Media File has failed to encoded !" & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error, FfmpegErr)
                            Else
                                NotifyIcon("Media File has failed to encoded !", "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), 1000, False)
                            End If
                            Label28.Text = "Error"
                            ProgressBarAdv1.Value = ProgressBarAdv1.Maximum
                        End If
                        ProgressBarAdv1.Value = 0
                        ProgressBarAdv1.Visible = False
                        OnCompleted(ComboBox36.Text)
                        Button1.Enabled = True
                        Button6.Enabled = True
                        CleanEnv("null")
                    End If
                Else
                    NotifyIcon("Media file has failed to encode", "Encoding profile not found", 1000, False)
                End If
            Else
                NotifyIcon("Media file has failed to encode", "Please select location to save media file before encode", 1000, False)
            End If
        Else
            NotifyIcon("Media file has failed to encode", "Configuration for GPU hardware acceleration not found", 1000, False)
        End If
        Button1.Enabled = True
        Button3.Enabled = True
        Button6.Enabled = True
    End Sub
    Private Async Sub StartEncode_MQ(selectedstats As Boolean)
        Button1.Enabled = False
        Button3.Enabled = False
        Button9.Enabled = False
        AddEncConf = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Encode Info:")
        AltEncodeConf = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Alt Encode:")
        DebugMode = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Debug Mode:")
        FrameConfig = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Frame Count:")
        If Hwdefconfig IsNot "null" And Hwdefconfig.Remove(0, 11) IsNot "" And mediaenginestate IsNot "NVENCC" Then
            If TextBox1.Text.ToString IsNot "" Then
                If MetroSetSwitch6.Switched = True And DataGridView1.Rows.Count > 0 Then
                    Dim encodeCount As Integer = 0
                    Dim encMediaFrame As String()
                    Dim encMediaDur As Integer
                    Dim mediaDur As Integer
                    Dim mediaCurDur As String()
                    Dim mediaTEMPFileNameOpt As String
                    Dim mediaTEMPFormatOpt As String
                    Dim mediaTEMPPreValue As String
                    Dim mediaTEMPPostValue As String
                    Dim mediaTotalDur As Integer
                    Dim newMediaTEMPFormatOpt As String
                    Dim num As Integer = 0
                    Dim resetStatus As Boolean = True
                    Dim totalMedia As Integer
                    ComboBox43.Enabled = False
                    Button32.Enabled = False
                    EncStartTime = DateTime.Now
                    Label28.Text = "Encoding"
                    Label71.Text = ""
                    If selectedstats Then
                        Dim totalDur As Integer
                        Dim curDur As String()
                        For i As Integer = 0 To DataGridView1.Rows.Count - 1
                            If DataGridView1.Rows(i).Cells(0).Value = True Then
                                curDur = DataGridView1.Rows(i).Cells(6).Value.ToString.Split(":")
                                totalDur += TimeConversion(curDur(0), curDur(1), Strings.Left(curDur(2), 2))
                            End If
                        Next
                        For i As Integer = 0 To DataGridView1.Rows.Count - 1
                            If DataGridView1.Rows(i).Cells(0).Value = True Then
                                totalMedia += 1
                            End If
                        Next
                        mediaTotalDur = totalDur
                        FrameCount = mediaTotalDur
                        ProgressBarAdv1.Visible = True
                        ProgressBarAdv1.Minimum = 0
                        ProgressBarAdv1.Value = 0
                        ProgressBarAdv1.Maximum = FrameCount
                        For i As Integer = 0 To DataGridView1.Rows.Count - 1
                            If DataGridView1.Rows(i).Cells(0).Value = True Then
                                Label71.Visible = True
                                Label71.Text = encodeCount.ToString + " / " + totalMedia.ToString
                                If DataGridView1.Rows(i).Cells(5).Value.ToString = "Audio File" Then
                                    If AudioTEMPFileNameOpt = "null" Then
                                        mediaTEMPFileNameOpt = ""
                                    Else
                                        mediaTEMPFileNameOpt = AudioTEMPFileNameOpt
                                    End If
                                    If AudioTEMPPreValue = "null" Or AudioTEMPPreValue = "" Then
                                        mediaTEMPPreValue = ""
                                    Else
                                        mediaTEMPPreValue = AudioTEMPPreValue + "_"
                                    End If
                                    If AudioTEMPPostValue = "null" Or AudioTEMPPostValue = "" Then
                                        mediaTEMPPostValue = ""
                                    Else
                                        mediaTEMPPostValue = "_" + AudioTEMPPostValue
                                    End If
                                ElseIf DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                    mediaTEMPFileNameOpt = VideoTEMPFileNameOpt
                                    mediaTEMPFormatOpt = VideoTEMPFormatOpt
                                    mediaTEMPPreValue = VideoTEMPPreValue
                                    mediaTEMPPostValue = VideoTEMPPostValue
                                Else
                                    mediaTEMPFileNameOpt = ""
                                    mediaTEMPFormatOpt = ""
                                    mediaTEMPPreValue = ""
                                    mediaTEMPPostValue = ""
                                End If
                                If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                    If MetroSetComboBox2.Text.ToString IsNot "" Then
                                        If MetroSetComboBox2.Text.ToString = "MPEG-4" Then
                                            newMediaTEMPFormatOpt = "mp4"
                                        ElseIf MetroSetComboBox2.Text.ToString = "Matroska" Then
                                            newMediaTEMPFormatOpt = "mkv"
                                        Else
                                            newMediaTEMPFormatOpt = "mkv"
                                        End If
                                    Else
                                        newMediaTEMPFormatOpt = "mkv"
                                    End If
                                Else
                                    newMediaTEMPFormatOpt = aCodecReverse(getBetween(DataGridView1.Rows(i).Cells(8).Value.ToString, "Audio: ", ", ").ToLower).ToLower
                                End If
                                If DataGridView1.Rows(i).Cells(8).Value.ToString IsNot "" Then
                                    If File.Exists(TextBox1.Text + "\" + mediaTEMPPreValue + Path.GetFileNameWithoutExtension(MediaQueueOrigDir(num) + "\" +
                                        DataGridView1.Rows(i).Cells(2).Value.ToString) + mediaTEMPPostValue + "." + newMediaTEMPFormatOpt) Then
                                        File.Delete(TextBox1.Text + "\" + mediaTEMPPreValue + Path.GetFileNameWithoutExtension(MediaQueueOrigDir(num) + "\" +
                                           DataGridView1.Rows(i).Cells(2).Value.ToString) + mediaTEMPPostValue + "." + newMediaTEMPFormatOpt)
                                    End If
                                    If mediaTEMPFileNameOpt IsNot "" Then
                                        If mediaTEMPFileNameOpt = " Prefix / Suffix" Then
                                            If mediaTEMPPreValue = "" And mediaTEMPPostValue IsNot "" Then
                                                If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                                    Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) + "\" +
                                                               DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(VideoQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                               File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") + " " & Chr(34) & TextBox1.Text +
                                                               "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & mediaTEMPPostValue & "." + newMediaTEMPFormatOpt & Chr(34)
                                                Else
                                                    Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) + "\" +
                                                              DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") + " " &
                                                              Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & mediaTEMPPostValue & "." + newMediaTEMPFormatOpt & Chr(34)
                                                End If
                                            ElseIf mediaTEMPPreValue IsNot "" And mediaTEMPPostValue = "" Then
                                                If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                                    Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) + "\" +
                                                           DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(VideoQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                           File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") + " " & Chr(34) & TextBox1.Text +
                                                           "\" & mediaTEMPPreValue + Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                                Else
                                                    Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) + "\" +
                                                         DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") + " " &
                                                         Chr(34) & TextBox1.Text + "\" & mediaTEMPPreValue + Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                                End If
                                            Else
                                                If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                                    Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) + "\" +
                                                                DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(VideoQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                                File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") + " " & Chr(34) & TextBox1.Text +
                                                                "\" & mediaTEMPPreValue + Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & mediaTEMPPostValue & "." & newMediaTEMPFormatOpt & Chr(34)
                                                Else
                                                    Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) + "\" +
                                                                DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") +
                                                                " " & Chr(34) & TextBox1.Text + "\" & mediaTEMPPreValue + Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & mediaTEMPPostValue & "." & newMediaTEMPFormatOpt & Chr(34)
                                                End If
                                            End If
                                        ElseIf mediaTEMPFileNameOpt = " Original File Name" Then
                                            If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                                Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) & "\" & DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(VideoQueueFlagsPath &
                                                            Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                            " " & Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                            Else
                                                Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) & "\" & DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                            " " & Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                            End If
                                        Else
                                            If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                                Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) & "\" & DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(VideoQueueFlagsPath &
                                                            Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                            " " & Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                            Else
                                                Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) & "\" & DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                            " " & Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                            End If
                                        End If
                                    Else
                                        If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                            Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) & "\" & DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(VideoQueueFlagsPath &
                                                            Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                            " " & Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                        Else
                                            Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) & "\" & DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                            " " & Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                        End If
                                    End If
                                    HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                    TextBox1.Enabled = False
                                    If FrameMode = "True" Then
                                        FrameCount = 0
                                    Else
                                        mediaCurDur = DataGridView1.Rows(i).Cells(6).Value.ToString.Split(":")
                                        mediaDur = TimeConversion(mediaCurDur(0), mediaCurDur(1), Strings.Left(mediaCurDur(2), 2))
                                    End If
                                    DataGridView1.Rows(i).Cells(3).Value = "Encoding"
                                    Dim new_psi As New ProcessStartInfo(HMEEngine & "HME.bat") With {
                                                            .RedirectStandardError = True,
                                                            .RedirectStandardOutput = False,
                                                            .CreateNoWindow = True,
                                                            .WindowStyle = ProcessWindowStyle.Hidden,
                                                            .UseShellExecute = False
                                            }
                                    If AddEncConf IsNot "null" Then
                                        AddEncTrimConf = AddEncConf.Remove(0, 12)
                                        If RemoveWhitespace(AddEncTrimConf) = "none" Then
                                            AddEncPassConf = "False"
                                            ProgressBarAdv1.TextVisible = False
                                        Else
                                            AddEncPassConf = "adv"
                                            ProgressBarAdv1.TextVisible = True
                                        End If
                                    Else
                                        AddEncPassConf = "False"
                                        ProgressBarAdv1.TextVisible = False
                                    End If
                                    Dim new_process As Process = Process.Start(new_psi)
                                    Do
                                        Dim lineReader As StreamReader = Await Task.Run(Function() new_process.StandardError)
                                        Dim line As String = Await Task.Run(Function() lineReader.ReadLineAsync)
                                        If RemoveWhitespace(getBetween(line, "time=", " bitrate")).Equals("") = False Then
                                            If RemoveWhitespace(getBetween(line, "time=", " bitrate")) <= FrameCount Then
                                                encMediaFrame = RemoveWhitespace(getBetween(line, "time=", " bitrate")).Split(":")
                                                encMediaDur = TimeConversion(encMediaFrame(0), encMediaFrame(1), Strings.Left(encMediaFrame(2), 2))
                                                If AddEncPassConf = "adv" Then
                                                    ProgressBarAdv1.CustomText = "Encoding speed: " + getBetween(line, "speed=", "x") + "x" + " Estimated size: " +
                                                                                                   getBetween(line, "size=", "kB") + "kiB" + " "
                                                    ProgressBarAdv1.TextStyle = ProgressBarTextStyles.Custom
                                                End If
                                                If Math.Round(encMediaDur / FrameCount * 100, 2) <= 0 Then
                                                    DataGridView1.Rows(i).Cells(3).Value = "Encoding 0%"
                                                Else
                                                    DataGridView1.Rows(i).Cells(3).Value = "Encoding " + Math.Round((encMediaDur / mediaDur) * 100, 2).ToString + "%"
                                                End If
                                                If encMediaDur = mediaDur Then
                                                    resetStatus = False
                                                    If encMediaDur <= 0 Then
                                                        ProgressBarAdv1.Value = ProgressBarAdv1.Value
                                                    Else
                                                        If ProgressBarAdv1.Value >= mediaDur Then
                                                            ProgressBarAdv1.Value = LastPBValue + encMediaDur
                                                        Else
                                                            ProgressBarAdv1.Value = encMediaDur
                                                        End If
                                                    End If
                                                    LastValue = ProgressBarAdv1.Value
                                                Else
                                                    resetStatus = True
                                                End If
                                                If resetStatus Then
                                                    If encMediaDur <= 0 Then
                                                        ProgressBarAdv1.Value = LastPBValue + 0
                                                        LastValue = 0
                                                    Else
                                                        ProgressBarAdv1.Value = LastPBValue + encMediaDur
                                                        LastValue = encMediaDur
                                                    End If
                                                Else
                                                    If LastValue > 0 Then
                                                        If encMediaDur <= 0 Then
                                                            ProgressBarAdv1.Value = ProgressBarAdv1.Value + LastValue
                                                            LastValue = 0
                                                        Else
                                                            If encMediaDur = mediaDur Then
                                                                ProgressBarAdv1.Value = LastValue
                                                            Else
                                                                ProgressBarAdv1.Value = ProgressBarAdv1.Value + (LastValue - encMediaDur)
                                                            End If
                                                            LastValue = encMediaDur
                                                        End If
                                                    Else
                                                        ProgressBarAdv1.Value = ProgressBarAdv1.Value + 0
                                                    End If
                                                    LastPBValue = ProgressBarAdv1.Value
                                                End If
                                            Else
                                                FfmpegEncStats = "Frame Error!"
                                            End If
                                        End If
                                    Loop Until Await Task.Run(Function() new_process.StandardError.EndOfStream)
                                    Await Task.Run(Sub() new_process.WaitForExit())
                                Else
                                    DataGridView1.Rows(i).Cells(3).Value = "Flags empty"
                                End If
                                If File.Exists(TextBox1.Text + "\" + mediaTEMPPreValue + Path.GetFileNameWithoutExtension(MediaQueueOrigDir(i) + "\" + DataGridView1.Rows(i).Cells(2).Value.ToString) + mediaTEMPPostValue + "." + newMediaTEMPFormatOpt) Then
                                    Dim destFile As New FileInfo(TextBox1.Text + "\" + mediaTEMPPreValue + Path.GetFileNameWithoutExtension(MediaQueueOrigDir(i) + "\" + DataGridView1.Rows(i).Cells(2).Value.ToString) + mediaTEMPPostValue + "." + newMediaTEMPFormatOpt)
                                    If destFile.Length / 1024 / 1024 < 1.0 Then
                                        DataGridView1.Rows(i).Cells(3).Value = "Error"
                                        DataGridView1.Rows(i).Cells(9).Value = "0.0 MB"
                                    Else
                                        encodeCount += 1
                                        DataGridView1.Rows(i).Cells(3).Value = "Success"
                                        DataGridView1.Rows(i).Cells(9).Value = GetFileSize(TextBox1.Text + "\" + mediaTEMPPreValue + Path.GetFileNameWithoutExtension(MediaQueueOrigDir(num) + "\" + DataGridView1.Rows(i).Cells(2).Value.ToString) + mediaTEMPPostValue + "." + newMediaTEMPFormatOpt)
                                    End If
                                End If
                            End If
                        Next
                    Else
                        Dim totalDur As Integer
                        Dim curDur As String()
                        For i As Integer = 0 To DataGridView1.Rows.Count - 1
                            curDur = DataGridView1.Rows(i).Cells(6).Value.ToString.Split(":")
                            totalDur += TimeConversion(curDur(0), curDur(1), Strings.Left(curDur(2), 2))
                        Next
                        mediaTotalDur = totalDur
                        FrameCount = mediaTotalDur
                        ProgressBarAdv1.Visible = True
                        ProgressBarAdv1.Minimum = 0
                        ProgressBarAdv1.Value = 0
                        ProgressBarAdv1.Maximum = FrameCount
                        totalMedia = DataGridView1.RowCount
                        For i As Integer = 0 To DataGridView1.Rows.Count - 1
                            Label71.Visible = True
                            Label71.Text = encodeCount.ToString + " / " + totalMedia.ToString
                            If DataGridView1.Rows(i).Cells(5).Value.ToString = "Audio File" Then
                                If AudioTEMPFileNameOpt = "null" Then
                                    mediaTEMPFileNameOpt = ""
                                Else
                                    mediaTEMPFileNameOpt = AudioTEMPFileNameOpt
                                End If
                                If AudioTEMPPreValue = "null" Or AudioTEMPPreValue = "" Then
                                    mediaTEMPPreValue = ""
                                Else
                                    mediaTEMPPreValue = AudioTEMPPreValue + "_"
                                End If
                                If AudioTEMPPostValue = "null" Or AudioTEMPPostValue = "" Then
                                    mediaTEMPPostValue = ""
                                Else
                                    mediaTEMPPostValue = "_" + AudioTEMPPostValue
                                End If
                            ElseIf DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                mediaTEMPFileNameOpt = VideoTEMPFileNameOpt
                                mediaTEMPFormatOpt = VideoTEMPFormatOpt
                                mediaTEMPPreValue = VideoTEMPPreValue
                                mediaTEMPPostValue = VideoTEMPPostValue
                            Else
                                mediaTEMPFileNameOpt = ""
                                mediaTEMPFormatOpt = ""
                                mediaTEMPPreValue = ""
                                mediaTEMPPostValue = ""
                            End If
                            If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                If MetroSetComboBox2.Text.ToString IsNot "" Then
                                    If MetroSetComboBox2.Text.ToString = "MPEG-4" Then
                                        newMediaTEMPFormatOpt = "mp4"
                                    ElseIf MetroSetComboBox2.Text.ToString = "Matroska" Then
                                        newMediaTEMPFormatOpt = "mkv"
                                    Else
                                        newMediaTEMPFormatOpt = "mkv"
                                    End If
                                Else
                                    newMediaTEMPFormatOpt = "mkv"
                                End If
                            Else
                                newMediaTEMPFormatOpt = aCodecReverse(getBetween(DataGridView1.Rows(i).Cells(8).Value.ToString, "Audio: ", ", ").ToLower).ToLower
                            End If
                            If DataGridView1.Rows(i).Cells(8).Value.ToString IsNot "" Then
                                If File.Exists(TextBox1.Text + "\" + mediaTEMPPreValue + Path.GetFileNameWithoutExtension(MediaQueueOrigDir(num) + "\" +
                                        DataGridView1.Rows(i).Cells(2).Value.ToString) + mediaTEMPPostValue + "." + newMediaTEMPFormatOpt) Then
                                    File.Delete(TextBox1.Text + "\" + mediaTEMPPreValue + Path.GetFileNameWithoutExtension(MediaQueueOrigDir(num) + "\" +
                                           DataGridView1.Rows(i).Cells(2).Value.ToString) + mediaTEMPPostValue + "." + newMediaTEMPFormatOpt)
                                End If
                                If mediaTEMPFileNameOpt IsNot "" Then
                                    If mediaTEMPFileNameOpt = " Prefix / Suffix" Then
                                        If mediaTEMPPreValue = "" And mediaTEMPPostValue IsNot "" Then
                                            If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                                Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) + "\" +
                                                               DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(VideoQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                               File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") + " " & Chr(34) & TextBox1.Text +
                                                               "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & mediaTEMPPostValue & "." + newMediaTEMPFormatOpt & Chr(34)
                                            Else
                                                Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) + "\" +
                                                              DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") + " " &
                                                              Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & mediaTEMPPostValue & "." + newMediaTEMPFormatOpt & Chr(34)
                                            End If
                                        ElseIf mediaTEMPPreValue IsNot "" And mediaTEMPPostValue = "" Then
                                            If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                                Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) + "\" +
                                                           DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(VideoQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                           File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") + " " & Chr(34) & TextBox1.Text +
                                                           "\" & mediaTEMPPreValue + Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                            Else
                                                Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) + "\" +
                                                         DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") + " " &
                                                         Chr(34) & TextBox1.Text + "\" & mediaTEMPPreValue + Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                            End If
                                        Else
                                            If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                                Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) + "\" +
                                                                DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(VideoQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                                File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") + " " & Chr(34) & TextBox1.Text +
                                                                "\" & mediaTEMPPreValue + Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & mediaTEMPPostValue & "." & newMediaTEMPFormatOpt & Chr(34)
                                            Else
                                                Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) + "\" +
                                                                DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") +
                                                                " " & Chr(34) & TextBox1.Text + "\" & mediaTEMPPreValue + Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & mediaTEMPPostValue & "." & newMediaTEMPFormatOpt & Chr(34)
                                            End If
                                        End If
                                    ElseIf mediaTEMPFileNameOpt = " Original File Name" Then
                                        If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                            Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) & "\" & DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(VideoQueueFlagsPath &
                                                            Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                            " " & Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                        Else
                                            Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) & "\" & DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                            " " & Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                        End If
                                    Else
                                        If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                            Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) & "\" & DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(VideoQueueFlagsPath &
                                                            Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                            " " & Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                        Else
                                            Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) & "\" & DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                            " " & Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                        End If
                                    End If
                                Else
                                    If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                        Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) & "\" & DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(VideoQueueFlagsPath &
                                                            Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                            " " & Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                    Else
                                        Newffargs = "ffmpeg -hide_banner -i " & Chr(34) & MediaQueueOrigDir(num) & "\" & DataGridView1.Rows(i).Cells(2).Value.ToString & Chr(34) & File.ReadAllText(AudioQueueFlagsPath & Path.GetFileName(DataGridView1.Rows(i).Cells(2).Value.ToString) & "_flags_0.txt") &
                                                            " " & Chr(34) & TextBox1.Text + "\" & Path.GetFileNameWithoutExtension(DataGridView1.Rows(i).Cells(2).Value.ToString) & "." + newMediaTEMPFormatOpt & Chr(34)
                                    End If
                                End If
                                HMEGenerate(HMEEngine & "HME.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                TextBox1.Enabled = False
                                If FrameMode = "True" Then
                                    FrameCount = 0
                                Else
                                    mediaCurDur = DataGridView1.Rows(i).Cells(6).Value.ToString.Split(":")
                                    mediaDur = TimeConversion(mediaCurDur(0), mediaCurDur(1), Strings.Left(mediaCurDur(2), 2))
                                End If
                                DataGridView1.Rows(i).Cells(3).Value = "Encoding"
                                Dim new_psi As New ProcessStartInfo(HMEEngine & "HME.bat") With {
                                                        .RedirectStandardError = True,
                                                        .RedirectStandardOutput = False,
                                                        .CreateNoWindow = True,
                                                        .WindowStyle = ProcessWindowStyle.Hidden,
                                                        .UseShellExecute = False
                                        }
                                If AddEncConf IsNot "null" Then
                                    AddEncTrimConf = AddEncConf.Remove(0, 12)
                                    If RemoveWhitespace(AddEncTrimConf) = "none" Then
                                        AddEncPassConf = "False"
                                        ProgressBarAdv1.TextVisible = False
                                    Else
                                        AddEncPassConf = "adv"
                                        ProgressBarAdv1.TextVisible = True
                                    End If
                                Else
                                    AddEncPassConf = "False"
                                    ProgressBarAdv1.TextVisible = False
                                End If
                                Dim new_process As Process = Process.Start(new_psi)
                                Do
                                    Dim lineReader As StreamReader = Await Task.Run(Function() new_process.StandardError)
                                    Dim line As String = Await Task.Run(Function() lineReader.ReadLineAsync)
                                    If RemoveWhitespace(getBetween(line, "time=", " bitrate")).Equals("") = False Then
                                        If RemoveWhitespace(getBetween(line, "time=", " bitrate")) <= FrameCount Then
                                            encMediaFrame = RemoveWhitespace(getBetween(line, "time=", " bitrate")).Split(":")
                                            encMediaDur = TimeConversion(encMediaFrame(0), encMediaFrame(1), Strings.Left(encMediaFrame(2), 2))
                                            If AddEncPassConf = "adv" Then
                                                ProgressBarAdv1.CustomText = "Encoding speed: " + getBetween(line, "speed=", "x") + "x" + " Estimated size: " +
                                                                                               getBetween(line, "size=", "kiB") + "kB" + " "
                                                ProgressBarAdv1.TextStyle = ProgressBarTextStyles.Custom
                                            End If
                                            If Math.Round(encMediaDur / FrameCount * 100, 2) <= 0 Then
                                                DataGridView1.Rows(i).Cells(3).Value = "Encoding 0%"
                                            Else
                                                DataGridView1.Rows(i).Cells(3).Value = "Encoding " + Math.Round((encMediaDur / mediaDur) * 100, 2).ToString + "%"
                                            End If
                                            If encMediaDur = mediaDur Then
                                                resetStatus = False
                                                If encMediaDur <= 0 Then
                                                    ProgressBarAdv1.Value = ProgressBarAdv1.Value
                                                Else
                                                    If ProgressBarAdv1.Value >= mediaDur Then
                                                        ProgressBarAdv1.Value = LastPBValue + encMediaDur
                                                    Else
                                                        ProgressBarAdv1.Value = encMediaDur
                                                    End If
                                                End If
                                                LastValue = ProgressBarAdv1.Value
                                            Else
                                                resetStatus = True
                                            End If
                                            If resetStatus Then
                                                If encMediaDur <= 0 Then
                                                    ProgressBarAdv1.Value = LastPBValue + 0
                                                    LastValue = 0
                                                Else
                                                    ProgressBarAdv1.Value = LastPBValue + encMediaDur
                                                    LastValue = encMediaDur
                                                End If
                                            Else
                                                If LastValue > 0 Then
                                                    If encMediaDur <= 0 Then
                                                        ProgressBarAdv1.Value = ProgressBarAdv1.Value + LastValue
                                                        LastValue = 0
                                                    Else
                                                        If encMediaDur = mediaDur Then
                                                            ProgressBarAdv1.Value = LastValue
                                                        Else
                                                            ProgressBarAdv1.Value = ProgressBarAdv1.Value + (LastValue - encMediaDur)
                                                        End If
                                                        LastValue = encMediaDur
                                                    End If
                                                Else
                                                    ProgressBarAdv1.Value = ProgressBarAdv1.Value + 0
                                                End If
                                                LastPBValue = ProgressBarAdv1.Value
                                            End If
                                        Else
                                            FfmpegEncStats = "Frame Error!"
                                        End If
                                    End If
                                Loop Until Await Task.Run(Function() new_process.StandardError.EndOfStream)
                                Await Task.Run(Sub() new_process.WaitForExit())
                            Else
                                DataGridView1.Rows(i).Cells(3).Value = "Flags empty"
                            End If
                            If File.Exists(TextBox1.Text + "\" + mediaTEMPPreValue + Path.GetFileNameWithoutExtension(MediaQueueOrigDir(num) + "\" + DataGridView1.Rows(i).Cells(2).Value.ToString) + mediaTEMPPostValue + "." + newMediaTEMPFormatOpt) Then
                                Dim destFile As New FileInfo(TextBox1.Text + "\" + mediaTEMPPreValue + Path.GetFileNameWithoutExtension(MediaQueueOrigDir(num) + "\" + DataGridView1.Rows(i).Cells(2).Value.ToString) + mediaTEMPPostValue + "." + newMediaTEMPFormatOpt)
                                If destFile.Length / 1024 / 1024 < 1.0 Then
                                    DataGridView1.Rows(i).Cells(3).Value = "Error"
                                    DataGridView1.Rows(i).Cells(9).Value = "0.0 MB"
                                    If Newdebugmode = "True" Then
                                        MessageBoxAdv.Show("Media File has failed to encoded !" & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error, FfmpegErr)
                                    End If
                                Else
                                    encodeCount += 1
                                    DataGridView1.Rows(i).Cells(3).Value = "Success"
                                    DataGridView1.Rows(i).Cells(9).Value = GetFileSize(TextBox1.Text + "\" + mediaTEMPPreValue + Path.GetFileNameWithoutExtension(MediaQueueOrigDir(num) + "\" + DataGridView1.Rows(i).Cells(2).Value.ToString) + mediaTEMPPostValue + "." + newMediaTEMPFormatOpt)
                                    If Newdebugmode = "True" Then
                                        MessageBoxAdv.Show("Media File has success to encoded !" & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information, FfmpegErr)
                                    End If
                                End If
                            End If
                            num += 1
                        Next
                    End If
                    EncEndTime = DateTime.Now
                    If encodeCount = totalMedia Then
                        NotifyIcon("Queue has successfuly to encoded !", "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), 1000, True)
                        Label28.Text = "Encode Success " + encodeCount.ToString + " / " + totalMedia.ToString
                    Else
                        NotifyIcon("Queue has failed to encoded !", "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), 1000, True)
                        Label28.Text = "Encode Error " + encodeCount.ToString + " / " + totalMedia.ToString
                    End If
                    ProgressBarAdv1.Value = 0
                    ProgressBarAdv1.Visible = False
                    OnCompleted(ComboBox36.Text)
                    ComboBox43.Enabled = True
                    Button32.Enabled = True
                    CleanEnv("null")
                    Label71.Text = ""
                End If
            Else
                NotifyIcon("Media file has failed to encode", "Please select location to save media file before encode", 1000, False)
            End If
        Else
            NotifyIcon("Media file has failed to encode", "Configuration for GPU hardware acceleration not found or Using Experimental Media Engine are not supported !", 1000, False)
        End If
        Button1.Enabled = True
        Button3.Enabled = True
        Button6.Enabled = True
    End Sub
    Private Sub StartEncode_MQ_SQ() Handles EncodeSelectedQueueToolStripMenuItem.Click
        StartEncode_MQ(True)
    End Sub
    Private Sub Startencode_MQ_AQ() Handles EncodeAllQueueToolStripMenuItem.Click
        StartEncode_MQ(False)
    End Sub
    Private Sub StartEncode(sender As Object, e As EventArgs) Handles Button3.Click
        ContextMenuStrip3.Show(Button3.PointToScreen(New Point(0, Button3.Height)))
    End Sub
    Private Sub EnableVideoCheck(sender As Object) Handles MetroSetSwitch1.SwitchedChanged
        If MetroSetSwitch1.Switched = True Then
            If DataGridView1.Rows.Count > 0 Then
                If Hwdefconfig = "GPU Engine:" Or Hwdefconfig = "null" Then
                    HwAccelFormat = ""
                    HwAccelDev = ""
                    MetroSetSwitch1.Switched = False
                    NotifyIcon("Hana Media Encoder", "Please configure GPU hardware acceleration !", 1000, False)
                Else
                    HwAccelFormat = "-hwaccel " & Hwdefconfig.Remove(0, 11)
                    HwAccelDev = Hwdefconfig.Remove(0, 11)
                    ComboBox2.Enabled = True
                    MetroSetComboBox1.Enabled = True
                    Button15.Enabled = True
                    Button16.Enabled = True
                    If Textbox77.Text IsNot "" Then
                        MetroSetComboBox1.SelectedIndex = 0
                    Else
                        MetroSetComboBox1.Items.Add("Stream #0:0")
                        MetroSetComboBox1.SelectedIndex = 0
                    End If
                    MetroSetComboBox3.Enabled = True
                    MetroSetCheckBox1.Enabled = True
                    NotifyIcon("Video Profile", "Multi video stream configuration are not supported when media queue is enable !", 1000, True)
                End If
            Else
                If Label5.Text.Equals("Not Detected") = True And TextBox15.Text Is "" Then
                    MetroSetSwitch1.Switched = False
                    NotifyIcon("Video Profile", "Current media file does not contain any video stream", 1000, True)
                Else
                    MetroSetComboBox3.Enabled = False
                    MetroSetCheckBox1.Enabled = False
                    If Hwdefconfig = "GPU Engine:" Or Hwdefconfig = "null" Then
                        HwAccelDev = ""
                        HwAccelFormat = ""
                        MetroSetSwitch1.Switched = False
                        NotifyIcon("Hana Media Encoder", "Please configure GPU hardware acceleration !", 1000, False)
                    Else
                        HwAccelDev = Hwdefconfig.Remove(0, 11)
                        HwAccelFormat = "-hwaccel " & Hwdefconfig.Remove(0, 11)
                        ComboBox2.Enabled = True
                        MetroSetComboBox1.Enabled = True
                        Button15.Enabled = True
                        Button16.Enabled = False
                        MetroSetComboBox1.SelectedIndex = 0
                    End If
                End If
            End If
        Else
            ComboBox2.SelectedIndex = -1
            ComboBox2.Enabled = False
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
            TileColumnUpDown1.Enabled = False
            TileColumnUpDown1.Value = 0
            TileRowsUpDown1.Enabled = False
            TileRowsUpDown1.Value = 0
            LookaheadUpDown1.Value = 0
            LookaheadUpDown1.Enabled = False
            MetroSetCheckBox3.Checked = False
            MetroSetCheckBox3.Enabled = False
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
            MetroSetComboBox1.Enabled = False
            MetroSetComboBox1.SelectedIndex = -1
            Button15.Enabled = False
            Button16.Enabled = False
            ComboBox32.SelectedIndex = -1
            ComboBox32.Enabled = False
            ComboBox35.Enabled = False
            ComboBox35.SelectedIndex = -1
            ComboBox37.SelectedIndex = -1
            ComboBox37.Enabled = False
            ComboBox37.SelectedIndex = -1
            ComboBox37.Enabled = False
            ComboBox38.SelectedIndex = -1
            ComboBox38.Enabled = False
            ComboBox39.SelectedIndex = -1
            ComboBox39.Enabled = False
            ComboBox40.SelectedIndex = -1
            ComboBox40.Enabled = False
            ComboBox41.SelectedIndex = -1
            ComboBox41.Enabled = False
            MetroSetCheckBox4.Checked = False
            MetroSetCheckBox4.Enabled = False
        End If
    End Sub
    Private Sub VideoStreamSource(sender As Object, e As EventArgs) Handles MetroSetComboBox1.SelectedIndexChanged
        If MetroSetSwitch6.Switched = False Then
            If MetroSetComboBox1.SelectedIndex >= 0 Then
                VideoStreamFlags = VideoStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)) - 1).ToString & ".txt"
                VideoStreamConfig = VideoStreamConfigPath & Path.GetFileName(Textbox77.Text) & "_config_" & (CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)) - 1).ToString & ".txt"
                If File.Exists(VideoStreamFlags) And File.Exists(VideoStreamConfig) Then
                    Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Configuration for video profile with stream #0:" & CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)).ToString & " already exists !" & vbCrLf & vbCrLf &
                                                                          "Check previous configuration for video profile with stream #0:" & CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)).ToString & " ?" &
                                                                          vbCrLf & "NOTE: This will replace current video profile configuration", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
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
                        Dim prevVideoForce10Bit As String = FindConfig(VideoStreamConfig, "Force10Bit=")
                        Dim prevVideoRateCtr As String = FindConfig(VideoStreamConfig, "RateControl=")
                        Dim prevVideoLookRate As String = FindConfig(VideoStreamConfig, "LookAhead=")
                        Dim prevVideoSpatialAQ As String = FindConfig(VideoStreamConfig, "SpatialAQ=")
                        Dim prevVideoAQStrength As String = FindConfig(VideoStreamConfig, "AQStrength=")
                        Dim prevVideoTempAQ As String = FindConfig(VideoStreamConfig, "TemporalAQ=")
                        Dim prevVideoTargetQL As String = FindConfig(VideoStreamConfig, "TargetQL=")
                        Dim prevVideoTier As String = FindConfig(VideoStreamConfig, "Tier=")
                        Dim prevVideoTune As String = FindConfig(VideoStreamConfig, "Tune=")
                        Dim prevVideoTileCol As String = FindConfig(VideoStreamConfig, "TileCol=")
                        Dim prevVideoTileRow As String = FindConfig(VideoStreamConfig, "TileRow=")
                        Dim prevVideoAspectRatio As String = FindConfig(VideoStreamConfig, "AspectRatio=")
                        Dim prevVideoResolution As String = FindConfig(VideoStreamConfig, "Resolution=")
                        Dim prevVideoScaleAlgo As String = FindConfig(VideoStreamConfig, "ScaleAlgo=")
                        Dim prevVideoColorRange As String = FindConfig(VideoStreamConfig, "ColorRange=")
                        Dim prevVideoColorPrimary As String = FindConfig(VideoStreamConfig, "ColorPrimary=")
                        Dim prevVideoColorSpace As String = FindConfig(VideoStreamConfig, "ColorSpace=")
                        Dim prevVideoScaleType As String = FindConfig(VideoStreamConfig, "ScaleType=")
                        Dim prevVideoMetadata As String = FindConfig(VideoStreamConfig, "Metadata=")
                        Dim prevVideoDeinterlaceMode As String = FindConfig(VideoStreamConfig, "DeMode=")
                        Dim prevVideoDeinterlaceParity As String = FindConfig(VideoStreamConfig, "DeParity=")
                        Dim prevVideoDeinterlace As String = FindConfig(VideoStreamConfig, "Deinterlace=")
                        Dim prevVideoDeinterlaceFrame As String = FindConfig(VideoStreamConfig, "DeFrame=")
                        If RemoveWhitespace(Strings.Mid(prevVideoBrCompat, 10)) IsNot "" Then
                            ComboBox21.Text = RemoveWhitespace(Strings.Mid(prevVideoBrCompat, 10))
                        Else
                            ComboBox21.Text = "disable"
                        End If
                        If Strings.Mid(prevVideoOvr, 12) = "" Then
                            BitRate_UpDown.Value = 0
                        Else
                            Dim tempOBR As String = RemoveWhitespace(Strings.Mid(prevVideoOvr, 12))
                            BitRate_UpDown.Text = CInt(tempOBR)
                        End If
                        If Strings.Mid(prevVideoBref, 6) = "" Then
                            ComboBox10.Text = ""
                        Else
                            ComboBox10.Text = Strings.Mid(prevVideoBref, 6)
                        End If
                        If Strings.Mid(prevVideoCodec, 7) = "" Then
                            ComboBox2.Text = ""
                        Else
                            ComboBox2.Text = Strings.Mid(prevVideoCodec, 7).ToUpper.Replace("_", String.Empty)
                        End If
                        ComboBox30.Text = Strings.Mid(prevVideoFps, 5)
                        If Strings.Mid(prevVideoLvl, 7) = "" Then
                            ComboBox8.Text = ""
                        Else
                            ComboBox8.Text = Strings.Mid(prevVideoLvl, 7)
                        End If
                        If Strings.Mid(prevVideoMaxBr, 12) = "" Then
                            MaxBitRate_UpDown.Value = 0
                        Else
                            Dim tempMBR As String = RemoveWhitespace(Strings.Mid(prevVideoMaxBr, 12))
                            MaxBitRate_UpDown.Text = CInt(tempMBR)
                        End If
                        If Strings.Mid(prevVideoMp, 11) = "" Then
                            ComboBox14.Text = ""
                        Else
                            ComboBox14.Text = Strings.Mid(prevVideoMp, 11)
                        End If
                        If Strings.Mid(prevVideoPreset, 8) = "" Then
                            ComboBox5.Text = ""
                        Else
                            ComboBox5.Text = Strings.Mid(prevVideoPreset, 8)
                        End If
                        If Strings.Mid(prevVideoTier, 6) = "" Then
                            ComboBox9.Text = ""
                        Else
                            ComboBox9.Text = Strings.Mid(prevVideoTier, 6)
                        End If
                        If Strings.Mid(prevVideoPixFmt, 13) = "" Then
                            ComboBox3.Text = ""
                        Else
                            ComboBox3.Text = Strings.Mid(prevVideoPixFmt, 13)
                        End If
                        If Strings.Mid(prevVideoProfile, 9) = "" Then
                            ComboBox7.Text = ""
                        Else
                            ComboBox7.Text = Strings.Mid(prevVideoProfile, 9)
                        End If
                        If Strings.Mid(prevVideoForce10Bit, 12) = "" Then
                            MetroSetCheckBox3.Checked = False
                        Else
                            MetroSetCheckBox3.Checked = Boolean.Parse(Strings.Mid(prevVideoForce10Bit, 12))
                        End If
                        If Strings.Mid(prevVideoRateCtr, 13) = "" Then
                            ComboBox4.Text = ""
                        Else
                            ComboBox4.Text = Strings.Mid(prevVideoRateCtr, 13)
                        End If
                        If Strings.Mid(prevVideoLookRate, 11) = "" Then
                            LookaheadUpDown1.Value = 0
                        Else
                            If (Integer.TryParse(Strings.Mid(prevVideoLookRate, 11), vbNull)) Then
                                LookaheadUpDown1.Value = CInt(Strings.Mid(prevVideoLookRate, 11))
                            Else
                                LookaheadUpDown1.Value = 0
                            End If
                        End If
                        If Strings.Mid(prevVideoSpatialAQ, 11) = "" Then
                            ComboBox11.Text = ""
                        Else
                            ComboBox11.Text = Strings.Mid(prevVideoSpatialAQ, 11)
                        End If
                        If Strings.Mid(prevVideoAQStrength, 12) = "" Then
                            ComboBox12.Text = ""
                        Else
                            ComboBox12.Text = Strings.Mid(prevVideoAQStrength, 12)
                        End If
                        If Strings.Mid(prevVideoTempAQ, 12) = "" Then
                            ComboBox13.Text = ""
                        Else
                            ComboBox13.Text = Strings.Mid(prevVideoTempAQ, 12)
                        End If
                        If Strings.Mid(prevVideoTargetQL, 10) = "" Then
                            CRF_VBR_UpDown.Text = 0
                        Else
                            CRF_VBR_UpDown.Text = CInt(RemoveWhitespace(Strings.Mid(prevVideoTargetQL, 10)))
                        End If
                        If Strings.Mid(prevVideoTune, 6) = "" Then
                            ComboBox6.Text = ""
                        Else
                            ComboBox6.Text = Strings.Mid(prevVideoTune, 6)
                        End If
                        If Strings.Mid(prevVideoTileCol, 9) = "" Then
                            TileColumnUpDown1.Value = 0
                        Else
                            TileColumnUpDown1.Value = CInt(Strings.Mid(prevVideoTileCol, 9))
                        End If
                        If Strings.Mid(prevVideoTileRow, 9) = "" Then
                            TileRowsUpDown1.Value = 0
                        Else
                            TileRowsUpDown1.Value = CInt(Strings.Mid(prevVideoTileRow, 9))
                        End If
                        If Strings.Mid(prevVideoAspectRatio, 13) = "" Then
                            ComboBox32.Text = ""
                        Else
                            ComboBox32.Text = Strings.Mid(prevVideoAspectRatio, 13)
                        End If
                        If Strings.Mid(prevVideoResolution, 12) = "" Then
                            ComboBox40.Text = ""
                        Else
                            ComboBox40.Text = Strings.Mid(prevVideoResolution, 12)
                        End If
                        If Strings.Mid(prevVideoScaleAlgo, 11) = "" Then
                            ComboBox35.Text = ""
                        Else
                            ComboBox35.Text = Strings.Mid(prevVideoScaleAlgo, 11)
                        End If
                        If Strings.Mid(prevVideoColorRange, 12) = "" Then
                            ComboBox37.Text = ""
                        Else
                            ComboBox37.Text = Strings.Mid(prevVideoColorRange, 12)
                        End If
                        If Strings.Mid(prevVideoColorPrimary, 14) = "" Then
                            ComboBox38.Text = ""
                        Else
                            ComboBox38.Text = Strings.Mid(prevVideoColorPrimary, 14)
                        End If
                        If Strings.Mid(prevVideoColorSpace, 12) = "" Then
                            ComboBox39.Text = ""
                        Else
                            ComboBox39.Text = Strings.Mid(prevVideoColorSpace, 12)
                        End If
                        If Strings.Mid(prevVideoScaleType, 11) = "" Then
                            ComboBox41.Text = ""
                        Else
                            ComboBox41.Text = Strings.Mid(prevVideoScaleType, 11)
                        End If
                        If Strings.Mid(prevVideoMetadata, 10) = "" Then
                            MetroSetCheckBox4.Checked = False
                        Else
                            If Strings.Mid(prevVideoMetadata, 10) = "True" Then
                                MetroSetCheckBox4.Checked = True
                            Else
                                MetroSetCheckBox4.Checked = False
                            End If
                        End If
                        If RemoveWhitespace(Strings.Mid(prevVideoDeinterlaceMode, 15)) IsNot "" Then
                            ComboBox29.Text = RemoveWhitespace(Strings.Mid(prevVideoDeinterlaceMode, 15))
                        Else
                            ComboBox29.Text = ""
                        End If
                        If RemoveWhitespace(Strings.Mid(prevVideoDeinterlaceFrame, 15)) IsNot "" Then
                            ComboBox44.Text = RemoveWhitespace(Strings.Mid(prevVideoDeinterlaceFrame, 15))
                        Else
                            ComboBox44.Text = ""
                        End If
                        If Strings.Mid(prevVideoDeinterlaceParity, 10) IsNot "" Then
                            ComboBox42.Text = Strings.Mid(prevVideoDeinterlaceParity, 10)
                        Else
                            ComboBox42.Text = ""
                        End If
                        If RemoveWhitespace(Strings.Mid(prevVideoDeinterlace, 13)) IsNot "" Then
                            ComboBox29.Text = RemoveWhitespace(Strings.Mid(prevVideoDeinterlace, 13))
                        Else
                            ComboBox29.Text = ""
                        End If
                    End If
                    If MetroSetSwitch6.Switched = True And DataGridView1.Rows.Count > 0 Then
                        Button15.Enabled = True
                        Button16.Enabled = True
                    Else
                        Button15.Enabled = False
                        Button16.Enabled = True
                    End If
                Else
                    If MetroSetSwitch6.Switched = True And DataGridView1.Rows.Count > 0 Then
                        Button15.Enabled = True
                        Button16.Enabled = True
                    Else
                        Button15.Enabled = True
                        Button16.Enabled = False
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub VideoAspToRes(sender As Object, e As EventArgs) Handles ComboBox32.SelectedIndexChanged
        Dim res4to3 = {"720p (960x720)", "1080p (1440x1080)", "2K (1536x1152)", "3K UHD (2160x1620)", "4K UHD (2880x2160)"}
        Dim res16to9 = {"720p (1280x720)", "1080p (1920x1080)", "2K (2048x1152)", "3K UHD (2880x1620)", "4K UHD (3840x2160)"}
        Dim res21to9 = {"WFHD (2560x1080)", "WFHD+ (2880x1200)", "WQHD (3440x1440)", "WQHD+ (3840x1600)", "UW4K (4320x1800)"}
        ComboBox40.SelectedIndex = -1
        If ComboBox32.SelectedIndex = 0 Then
            ComboBox40.Enabled = True
            ComboBox40.Items.Clear()
            ComboBox40.Items.AddRange(res4to3)
        ElseIf ComboBox32.SelectedIndex = 1 Then
            ComboBox40.Enabled = True
            ComboBox40.Items.Clear()
            ComboBox40.Items.AddRange(res16to9)
        ElseIf ComboBox32.SelectedIndex = 2 Then
            ComboBox40.Enabled = True
            ComboBox40.Items.Clear()
            ComboBox40.Items.AddRange(res21to9)
        Else
            ComboBox40.Enabled = True
            ComboBox40.Items.Clear()
            ComboBox40.Items.AddRange(res16to9)
        End If
    End Sub
    Private Sub VideoScaleTypeAdapt(sender As Object, e As EventArgs) Handles ComboBox40.SelectedIndexChanged
        Dim scaleTypeArr = {"Crop", "Pad", "disable"}
        If ComboBox40.Text IsNot "" Then
            If ComboBox32.SelectedIndex > 2 Then
                ComboBox41.Items.Clear()
                ComboBox41.Items.Add("disable")
            Else
                ComboBox41.Items.Clear()
                ComboBox41.Items.AddRange(scaleTypeArr)
            End If
        Else
            ComboBox41.Items.Clear()
            ComboBox41.Items.Add("disable")
        End If
    End Sub
    Private Sub VideoCodecCheck(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Dim GraphicCardName As String = GetGraphicsCardName("Name")
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
            TileRowsUpDown1.Enabled = False
            TileColumnUpDown1.Enabled = False
            LookaheadUpDown1.Enabled = False
            ComboBox6.Enabled = False
            ComboBox9.Enabled = False
            ComboBox32.Enabled = False
            ComboBox35.Enabled = False
            ComboBox37.Enabled = False
            ComboBox38.Enabled = False
            ComboBox39.Enabled = False
            ComboBox40.Enabled = False
            ComboBox41.Enabled = False
            MetroSetCheckBox3.Enabled = False
            MetroSetCheckBox4.Enabled = False
        ElseIf HwAccelDev = "opencl" Then
            ComboBox21.Enabled = False
            ComboBox10.Enabled = False
            ComboBox14.Enabled = False
            ComboBox3.Text = "yuv420p"
            ComboBox3.Enabled = False
            TileRowsUpDown1.Enabled = False
            TileColumnUpDown1.Enabled = False
            LookaheadUpDown1.Enabled = False
            If ComboBox2.Text = "HEVC" Then
                MetroSetCheckBox3.Enabled = False
                ComboBox4.Enabled = False
                ComboBox7.Text = "main"
                ComboBox7.Enabled = False
                ComboBox21.Enabled = True
                ComboBox9.Enabled = True
                If ComboBox8.Items.Contains("6.2") Then
                    ComboBox8.Items.Remove("6.3")
                    ComboBox8.Items.Remove("7")
                    ComboBox8.Items.Remove("7.0")
                    ComboBox8.Items.Remove("7.1")
                    ComboBox8.Items.Remove("7.2")
                    ComboBox8.Items.Remove("7.3")
                End If
            ElseIf ComboBox2.Text = "AV1" Then
                Dim AMDPattern As String = "RX 7\w\w\w"
                If Regex.IsMatch(GraphicCardName, AMDPattern) = False Then
                    NotifyIcon("Video Codec", "AV1 Encoding are not supported on your GPU !", 1000, True)
                    ComboBox2.SelectedIndex = -1
                    MetroSetCheckBox3.Enabled = False
                Else
                    ComboBox4.Enabled = True
                    If Regex.IsMatch(GraphicCardName, AMDPattern) Then
                        If ComboBox8.Items.Contains("6.2") = True Then
                            ComboBox8.Items.Add("6.3")
                            ComboBox8.Items.Add("7")
                            ComboBox8.Items.Add("7.0")
                            ComboBox8.Items.Add("7.1")
                            ComboBox8.Items.Add("7.2")
                            ComboBox8.Items.Add("7.3")
                        End If
                    Else
                        If ComboBox8.Items.Contains("6.2") = True And ComboBox8.Items.Contains("6.3") = False Then
                            ComboBox8.Items.Remove("6.3")
                            ComboBox8.Items.Remove("7")
                            ComboBox8.Items.Remove("7.0")
                            ComboBox8.Items.Remove("7.1")
                            ComboBox8.Items.Remove("7.2")
                            ComboBox8.Items.Remove("7.3")
                        End If
                    End If
                    ComboBox21.Enabled = False
                    MetroSetCheckBox3.Enabled = True
                End If
                ComboBox4.Enabled = False
                ComboBox7.Text = "main"
                ComboBox7.Enabled = False
                ComboBox9.Enabled = True
            Else
                ComboBox4.Enabled = False
                ComboBox7.Enabled = True
                If ComboBox7.Items.Contains("main10") Then
                    ComboBox7.Items.Remove("main10")
                End If
                ComboBox21.Enabled = True
                ComboBox9.Enabled = False
                MetroSetCheckBox3.Enabled = False
            End If
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
            ComboBox35.Enabled = True
            ComboBox37.Enabled = True
            ComboBox38.Enabled = True
            ComboBox41.Enabled = True
            MetroSetCheckBox4.Enabled = True
        ElseIf HwAccelDev = "qsv" Then
            ComboBox21.Enabled = False
            ComboBox10.Enabled = False
            ComboBox14.Enabled = False
            LookaheadUpDown1.Enabled = False
            If ComboBox2.Text = "HEVC" Then
                ComboBox7.Enabled = True
                If ComboBox8.Items.Contains("6.2") Then
                    ComboBox8.Items.Remove("6.3")
                    ComboBox8.Items.Remove("7")
                    ComboBox8.Items.Remove("7.0")
                    ComboBox8.Items.Remove("7.1")
                    ComboBox8.Items.Remove("7.2")
                    ComboBox8.Items.Remove("7.3")
                End If
                If ComboBox7.Items.Contains("high") Then
                    ComboBox7.Items.Remove("high")
                End If
                If ComboBox7.Items.Contains("main10") = False Then
                    ComboBox7.Items.Add("main10")
                End If
                ComboBox21.Enabled = True
                TileRowsUpDown1.Enabled = False
                TileColumnUpDown1.Enabled = False
                MetroSetCheckBox3.Enabled = False
            ElseIf ComboBox2.Text = "AV1" Then
                If GraphicCardName.Contains("ARC A") = False Then
                    NotifyIcon("Video Codec", "AV1 Encoding are not supported on your GPU !", 1000, True)
                    ComboBox2.SelectedIndex = -1
                    If ComboBox8.Items.Contains("6.2") = True Then
                        ComboBox8.Items.Remove("6.3")
                        ComboBox8.Items.Remove("7")
                        ComboBox8.Items.Remove("7.0")
                        ComboBox8.Items.Remove("7.1")
                        ComboBox8.Items.Remove("7.2")
                        ComboBox8.Items.Remove("7.3")
                    End If
                    MetroSetCheckBox3.Enabled = False
                Else
                    If ComboBox8.Items.Contains("6.2") = True And ComboBox8.Items.Contains("6.3") = False Then
                        ComboBox8.Items.Add("6.3")
                        ComboBox8.Items.Add("7")
                        ComboBox8.Items.Add("7.0")
                        ComboBox8.Items.Add("7.1")
                        ComboBox8.Items.Add("7.2")
                        ComboBox8.Items.Add("7.3")
                    End If
                    ComboBox21.Enabled = False
                    MetroSetCheckBox3.Enabled = True
                End If
                ComboBox7.Enabled = True
                If ComboBox7.Items.Contains("main10") Then
                    ComboBox7.Items.Remove("main10")
                End If
                ComboBox9.Enabled = False
                TileRowsUpDown1.Enabled = True
                TileColumnUpDown1.Enabled = True
            Else
                ComboBox7.Enabled = True
                If ComboBox7.Items.Contains("main10") Then
                    ComboBox7.Items.Remove("main10")
                End If
                ComboBox21.Enabled = True
                ComboBox9.Enabled = False
                TileRowsUpDown1.Enabled = False
                TileColumnUpDown1.Enabled = False
                MetroSetCheckBox3.Enabled = False
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
            ComboBox35.Enabled = True
            ComboBox37.Enabled = True
            ComboBox38.Enabled = True
            ComboBox41.Enabled = True
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
            LookaheadUpDown1.Enabled = True
            If ComboBox2.Text = "HEVC" Then
                If ComboBox8.Items.Contains("6.2") Then
                    ComboBox8.Items.Remove("6.3")
                    ComboBox8.Items.Remove("7")
                    ComboBox8.Items.Remove("7.0")
                    ComboBox8.Items.Remove("7.1")
                    ComboBox8.Items.Remove("7.2")
                    ComboBox8.Items.Remove("7.3")
                End If
                If ComboBox7.Items.Contains("high") Then
                    ComboBox7.Items.Remove("high")
                End If
                If mediaenginestate = "NVENCC" Then
                    If ComboBox7.Items.Contains("main10") Then
                        ComboBox7.Items.Remove("main10")
                    End If
                Else
                    If ComboBox7.Items.Contains("main10") = False Then
                        ComboBox7.Items.Add("main10")
                    End If
                End If
                ComboBox9.Enabled = True
                ComboBox21.Enabled = True
                TileRowsUpDown1.Enabled = False
                TileColumnUpDown1.Enabled = False
                If mediaenginestate = "NVENCC" Then
                    MetroSetCheckBox3.Enabled = True
                Else
                    MetroSetCheckBox3.Enabled = False
                End If
            ElseIf ComboBox2.Text = "AV1" Then
                If GraphicCardName.Contains("RTX 40") = False Then
                    NotifyIcon("Video Codec", "AV1 Encoding are not supported on your GPU !", 1000, True)
                    ComboBox2.SelectedIndex = -1
                    If ComboBox8.Items.Contains("6.2") = True Then
                        ComboBox8.Items.Remove("6.3")
                        ComboBox8.Items.Remove("7")
                        ComboBox8.Items.Remove("7.0")
                        ComboBox8.Items.Remove("7.1")
                        ComboBox8.Items.Remove("7.2")
                        ComboBox8.Items.Remove("7.3")
                    End If
                    MetroSetCheckBox3.Enabled = False
                Else
                    If ComboBox8.Items.Contains("6.2") = True And ComboBox8.Items.Contains("6.3") = False Then
                        ComboBox8.Items.Add("6.3")
                        ComboBox8.Items.Add("7")
                        ComboBox8.Items.Add("7.0")
                        ComboBox8.Items.Add("7.1")
                        ComboBox8.Items.Add("7.2")
                        ComboBox8.Items.Add("7.3")
                    End If
                    ComboBox7.Enabled = False
                    ComboBox21.Enabled = False
                    MetroSetCheckBox3.Enabled = True
                End If
                ComboBox9.Enabled = False
                TileRowsUpDown1.Enabled = True
                TileColumnUpDown1.Enabled = True
            Else
                If ComboBox7.Items.Contains("main10") Then
                    ComboBox7.Items.Remove("main10")
                End If
                If ComboBox7.Items.Contains("high") = False Then
                    ComboBox7.Items.Add("high")
                End If
                ComboBox21.Enabled = True
                ComboBox9.Enabled = False
                TileRowsUpDown1.Enabled = False
                TileColumnUpDown1.Enabled = False
                If mediaenginestate = "NVENCC" Then
                    MetroSetCheckBox3.Enabled = True
                Else
                    MetroSetCheckBox3.Enabled = False
                End If
            End If
            ComboBox4.Enabled = True
            ComboBox11.Enabled = True
            ComboBox12.Enabled = True
            ComboBox13.Enabled = True
            CRF_VBR_UpDown.Enabled = True
            ComboBox6.Enabled = True
            If mediaenginestate = "NVENCC" Then
                ComboBox32.Enabled = False
                ComboBox35.Enabled = False
                ComboBox40.Enabled = False
                ComboBox41.Enabled = False
                Dim NVENCCPixFmt = {"yuv420", "yuv444"}
                ComboBox3.SelectedText = ""
                ComboBox3.Items.Clear()
                ComboBox3.Items.AddRange(NVENCCPixFmt)
            Else
                ComboBox32.Enabled = True
                ComboBox35.Enabled = True
                ComboBox40.Enabled = True
                ComboBox41.Enabled = True
                Dim FFMPEGPixFmt = {"yuv420p", "yuv444p", "p010le", "p016le"}
                ComboBox3.SelectedText = ""
                ComboBox3.Items.Clear()
                ComboBox3.Items.AddRange(FFMPEGPixFmt)
            End If
            ComboBox37.Enabled = True
            ComboBox38.Enabled = True
            MetroSetCheckBox4.Enabled = True
        End If
        If mediaenginestate = "NVENCC" Or ComboBox2.Text = "Copy" Then
            ComboBox22.Enabled = False
            ComboBox44.Enabled = False
            ComboBox29.Enabled = False
            ComboBox42.Enabled = False
        Else
            ComboBox22.Enabled = True
            ComboBox44.Enabled = True
            ComboBox29.Enabled = True
            ComboBox42.Enabled = True
        End If
    End Sub
    Private Sub VideoStreamInitConfig(queue As Boolean, rows As Integer, codec As Boolean)
        Dim forceDecision As Boolean = False
        If MetroSetComboBox1.SelectedIndex >= 0 Then
            If ComboBox2.Text = "" Then
                NotifyIcon("Hana Media Encoder", "Video codec can not empty !", 1000, False)
                ReturnVideoStats = False
                forceDecision = False
            End If
            If forceDecision = True Then
                NotifyIcon("Hana Media Encoder", "Please configure GPU hardware acceleration !", 1000, False)
                ReturnVideoStats = False
            Else
                If codec = False Then
                    HMEStreamProfileGenerate(VideoStreamFlags, " -vn ")
                    HMEVideoStreamConfigGenerate(VideoStreamConfig, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                    ReturnVideoStats = True
                Else
                    If queue Then
                        VideoStreamFlags = VideoQueueFlagsPath & DataGridView1.Rows(rows).Cells(2).Value.ToString & "_flags_" & (CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11))).ToString & ".txt"
                        VideoStreamConfig = VideoQueueConfigPath & DataGridView1.Rows(rows).Cells(2).Value.ToString & "_config_" & (CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11))).ToString & ".txt"
                        VideoStreamSourceList = CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)).ToString
                    Else
                        VideoStreamFlags = VideoStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)) - 1).ToString & ".txt"
                        VideoStreamConfig = VideoStreamConfigPath & Path.GetFileName(Textbox77.Text) & "_config_" & (CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)) - 1).ToString & ".txt"
                        VideoStreamSourceList = (CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)) - 1).ToString
                    End If
                    If ComboBox2.Text.Equals("Copy") = True Then
                        If queue Then
                            VideoQueueFlags = " -c:v:" & VideoStreamSourceList & " copy"
                        End If
                        HMEStreamProfileGenerate(VideoStreamFlags, " -c:v:" & VideoStreamSourceList & " copy")
                        HMEVideoStreamConfigGenerate(VideoStreamConfig, "", "", "", "copy", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                        ReturnVideoStats = True
                    Else
                        If ComboBox32.SelectedIndex >= 0 And ComboBox32.SelectedIndex < 3 Then
                            AspectRatio = " -aspect " & vAspectRatio(ComboBox32.Text)
                            If ComboBox35.Text = "disabled" Or ComboBox35.Text = "" Then
                                ScaleAlgo = ""
                            Else
                                ScaleAlgo = ":flags=" & ComboBox35.Text
                            End If
                            If ComboBox40.SelectedIndex < 0 Then
                                VideoRes = ""
                            Else
                                Dim oriAspRatio As String()
                                If queue Then
                                    oriAspRatio = getBetween(DataGridView1.Rows(rows).Cells(4).Value.ToString, "[", "]").Split(":")
                                Else
                                    oriAspRatio = Label7.Text.Split(":")
                                End If
                                Dim oriAspRatioX As Integer = CInt(oriAspRatio(0))
                                Dim oriAspRatioY As Integer = CInt(oriAspRatio(1))
                                Dim tarAspRatioX As Integer = CInt(getBetween(ComboBox32.Text, "(", ":"))
                                If vAspectRatioCmp(vAspectRatioTrX(oriAspRatioX), tarAspRatioX) = "up" Then
                                    If ComboBox41.SelectedIndex = 0 Then
                                        VideoRes = " -filter:v " & vCropAspUp(getBetween(ComboBox40.Text, "x", ")"), getBetween(ComboBox40.Text, "(", "x"), oriAspRatioX, oriAspRatioY, ScaleAlgo)
                                    ElseIf ComboBox41.SelectedIndex = 1 Then
                                        VideoRes = " -filter:v " & vPadAspUp(getBetween(ComboBox40.Text, "x", ")"), getBetween(ComboBox40.Text, "(", "x"), oriAspRatioX, oriAspRatioY, ScaleAlgo)
                                    Else
                                        VideoRes = " -filter:v scale=" & getBetween(vResTranslate(ComboBox40.Text), "", "x") & ":" & getBetween(vResTranslate(ComboBox40.Text), "x", "y") & ScaleAlgo
                                    End If
                                ElseIf vAspectRatioCmp(vAspectRatioTrX(oriAspRatioX), tarAspRatioX) = "down" Then
                                    If ComboBox41.SelectedIndex = 0 Then
                                        VideoRes = " -filter:v " & vCropAspDown(getBetween(ComboBox40.Text, "x", ")"), getBetween(ComboBox40.Text, "(", "x"), oriAspRatioX, oriAspRatioY, ScaleAlgo)
                                    ElseIf ComboBox41.SelectedIndex = 1 Then
                                        VideoRes = " -filter:v " & vPadAspDown(getBetween(ComboBox40.Text, "x", ")"), getBetween(ComboBox40.Text, "(", "x"), oriAspRatioX, oriAspRatioY, ScaleAlgo)
                                    Else
                                        VideoRes = " -filter:v scale=" & getBetween(vResTranslate(ComboBox40.Text), "", "x") & ":" & getBetween(vResTranslate(ComboBox40.Text), "x", "y") & ScaleAlgo
                                    End If
                                Else
                                    VideoRes = " -filter:v scale=" & getBetween(vResTranslate(ComboBox40.Text), "", "x") & ":" & getBetween(vResTranslate(ComboBox40.Text), "x", "y") & ScaleAlgo
                                End If
                            End If
                        ElseIf ComboBox32.SelectedIndex = 3 Then
                            AspectRatio = ""
                            If ComboBox35.Text Is "disabled" Or ComboBox35.Text Is "" Then
                                ScaleAlgo = ""
                            Else
                                ScaleAlgo = ":flags=" & ComboBox35.Text
                            End If
                            VideoRes = " -filter:v scale=" & getBetween(vResTranslate(ComboBox40.Text), "", "x") & ":" & getBetween(vResTranslate(ComboBox40.Text), "x", "y") & ScaleAlgo
                        Else
                            AspectRatio = ""
                            ScaleAlgo = ""
                            VideoRes = ""
                        End If
                        If AspectRatio = "" And VideoRes = "" And ScaleAlgo = "" And ComboBox37.SelectedIndex < 0 Then
                            ColorRange = ""
                            If ComboBox30.SelectedIndex < 0 Then
                                FPS = ""
                            Else
                                If ComboBox22.SelectedIndex < 0 And ComboBox44.SelectedIndex < 0 And ComboBox29.SelectedIndex < 0 And ComboBox42.SelectedIndex < 0 Then
                                    FPS = " -filter:v fps=fps=" & ComboBox30.Text
                                Else
                                    FPS = ",fps=fps=" & ComboBox30.Text
                                End If
                            End If
                        Else
                            If VideoRes = "" Then
                                If ScaleAlgo Is "" Then
                                    If ComboBox30.SelectedIndex < 0 Then
                                        FPS = ""
                                    Else
                                        If ComboBox22.SelectedIndex < 0 And ComboBox44.SelectedIndex < 0 And ComboBox29.SelectedIndex < 0 And ComboBox42.SelectedIndex < 0 Then
                                            FPS = " -filter:v fps=fps=" & ComboBox30.Text
                                        Else
                                            FPS = ",fps=fps=" & ComboBox30.Text
                                        End If
                                    End If
                                Else
                                    If ComboBox30.SelectedIndex < 0 Then
                                        FPS = ""
                                    Else
                                        FPS = ",fps=fps=" & ComboBox30.Text
                                    End If
                                End If
                            Else
                                If ComboBox30.SelectedIndex < 0 Then
                                    FPS = ""
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
                        TileColumn = TileColumnUpDown1.Value
                        TileRows = TileRowsUpDown1.Value
                        If TileRowsUpDown1.Value > 0 Then
                            If TileColumnUpDown1.Value > 0 Then
                                If mediaenginestate = "NVENCC" Then
                                    TileFlags = " --tile-columns " & TileColumnUpDown1.Value & " --tile-rows " & TileRowsUpDown1.Value
                                Else
                                    TileFlags = " -tile-columns " & TileColumnUpDown1.Value & " -tile-rows " & TileRowsUpDown1.Value
                                End If
                            Else
                                If mediaenginestate = "NVENCC" Then
                                    TileFlags = " --tile-columns auto " & " --tile-rows " & TileRowsUpDown1.Value
                                Else
                                    TileFlags = " -tile-columns -1 " & " -tile-rows " & TileRowsUpDown1.Value
                                End If
                            End If
                        Else
                            If mediaenginestate = "NVENCC" Then
                                TileFlags = " "
                            Else
                                TileFlags = " --tile-columns -1 " & " --tile-rows -1 "
                            End If
                        End If
                        If HwAccelDev = "cuda" Then
                            If CRF_VBR_UpDown.Value = 0 Then
                                TargetQualityControl = ""
                            Else
                                TargetQualityControl = " -cq " & CRF_VBR_UpDown.Text
                            End If
                        End If
                        If mediaenginestate = "NVENCC" Then
                            HMEStreamProfileGenerate(VideoStreamFlags, vCodecAlt(ComboBox2.Text) & vPixFmtAlt(ComboBox3.Text, MetroSetCheckBox3.Checked) &
                                                      vRateControlAlt(ComboBox4.Text, MaxBitRate_UpDown.Text, BitRate_UpDown.Text, CRF_VBR_UpDown.Text) & vPresetAlt(ComboBox5.Text) & vTuneAlt(ComboBox6.Text) & vProfileAlt(ComboBox7.Text) &
                                                      vLevelAlt(ComboBox8.Text) & vTierAlt(ComboBox9.Text, ComboBox2.Text) & bRefModeAlt(ComboBox10.Text) &
                                                      multiPassAlt(ComboBox14.Text) & TileFlags & vColorRangeAlt(ComboBox37.Text) & vColorPrimaryAlt(ComboBox38.Text) & vColorSpaceAlt(ComboBox39.Text) & vFpsAlt(ComboBox30.Text))
                        Else
                            HMEStreamProfileGenerate(VideoStreamFlags, metadata(MetroSetCheckBox4.Checked) & " -c:v:" & VideoStreamSourceList & " " & vCodec(ComboBox2.Text, HwAccelDev) & vPixFmt(ComboBox3.Text, HwAccelDev, ComboBox2.Text, MetroSetCheckBox3.Checked) & vRateControl(ComboBox4.Text, ComboBox2.Text, HwAccelDev) &
                                                          TargetQualityControl & vLookAheadLvl(LookaheadUpDown1.Value) & vPreset(ComboBox5.Text, HwAccelDev, ComboBox2.Text) & vTune(ComboBox6.Text) & vProfile(ComboBox7.Text) & vLevel(ComboBox8.Text) & vTier(ComboBox9.Text, HwAccelDev, ComboBox2.Text) &
                                                          vBrcompat(ComboBox21.Text) & BitRate & MaxBitRate & bRefMode(ComboBox10.Text) & vSpaTempAQ(ComboBox11.Text, ComboBox2.Text) & vAQStrength(ComboBox12.Text) & vTempAQ(ComboBox13.Text, ComboBox2.Text) &
                                                          multiPass(ComboBox14.Text) & TileFlags & vColorRange(ComboBox37.Text) & vColorPrimary(ComboBox38.Text) & vColorSpace(ComboBox39.Text, HwAccelDev) & vDeInterlace(ComboBox22.Text, HwAccelDev, ComboBox29.Text, ComboBox42.Text, ComboBox44.Text) & AspectRatio & VideoRes & FPS)
                        End If
                        HMEVideoStreamConfigGenerate(VideoStreamConfig, ComboBox21.Text, BitRate_UpDown.Text, ComboBox10.Text, ComboBox2.Text, ComboBox30.Text, ComboBox8.Text, MaxBitRate_UpDown.Text,
                                                                             ComboBox14.Text, ComboBox5.Text, ComboBox3.Text, ComboBox7.Text, MetroSetCheckBox3.Checked, ComboBox4.Text, LookaheadUpDown1.Text, ComboBox11.Text,
                                                                             ComboBox12.Text, ComboBox13.Text, CRF_VBR_UpDown.Text, ComboBox9.Text, ComboBox6.Text, TileColumn, TileRows, ComboBox32.Text, ComboBox40.Text, ComboBox35.Text,
                                                                             ComboBox37.Text, ComboBox38.Text, ComboBox39.Text, ComboBox22.Text, ComboBox29.Text, ComboBox42.Text, ComboBox44.Text, ComboBox41.Text, MetroSetCheckBox4.Checked)
                        ReturnVideoStats = True
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub SaveVideoStream_Btn(sender As Object, e As EventArgs) Handles Button15.Click
        Dim interlaceCheck As Boolean
        If ComboBox22.Text IsNot "" Then
            If ComboBox44.SelectedIndex < 0 Or ComboBox29.SelectedIndex < 0 Or ComboBox42.SelectedIndex < 0 Then
                interlaceCheck = False
            Else
                interlaceCheck = True
            End If
        Else
            interlaceCheck = True
        End If
        If interlaceCheck Then
            If MetroSetComboBox1.SelectedIndex >= 0 Then
                If MetroSetSwitch6.Switched = False Then
                    VideoStreamFlags = VideoStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)) - 1).ToString & ".txt"
                    VideoStreamConfig = VideoStreamConfigPath & Path.GetFileName(Textbox77.Text) & "_config_" & (CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)) - 1).ToString & ".txt"
                    If File.Exists(VideoStreamFlags) And File.Exists(VideoStreamConfig) Then
                        Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Configuration for video profile with stream #0:" & CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)).ToString & " already exists !" & vbCrLf &
                                                                              "Overwrite previous configuration ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If configResult = DialogResult.Yes Then
                            GC.Collect()
                            GC.WaitForPendingFinalizers()
                            File.Delete(VideoStreamFlags)
                            File.Delete(VideoStreamConfig)
                            VcodecReset()
                            VideoStreamInitConfig(False, 0, True)
                            If ReturnVideoStats = False Then
                                NotifyIcon("Hana Media Encoder", "Failed to save video profile !", 1000, False)
                                Button15.ForeColor = ColorTranslator.FromHtml("#F4A950")
                                Button15.BackColor = ColorTranslator.FromHtml("#161B21")
                                Button16.ForeColor = ColorTranslator.FromHtml("#161B21")
                                Button16.BackColor = ColorTranslator.FromHtml("#F4A950")
                                Button15.Enabled = True
                                Button16.Enabled = False
                            Else
                                NotifyIcon("Hana Media Encoder", "Video profile has been saved!", 1000, True)
                                Button15.ForeColor = ColorTranslator.FromHtml("#161B21")
                                Button15.BackColor = ColorTranslator.FromHtml("#F4A950")
                                Button16.ForeColor = ColorTranslator.FromHtml("#F4A950")
                                Button16.BackColor = ColorTranslator.FromHtml("#161B21")
                                Button15.Enabled = False
                                Button16.Enabled = True
                            End If
                        End If
                    Else
                        VideoStreamInitConfig(False, 0, True)
                        If ReturnVideoStats = False Then
                            NotifyIcon("Hana Media Encoder", "Failed to save video profile !", 1000, False)
                            Button15.ForeColor = ColorTranslator.FromHtml("#F4A950")
                            Button15.BackColor = ColorTranslator.FromHtml("#161B21")
                            Button16.ForeColor = ColorTranslator.FromHtml("#161B21")
                            Button16.BackColor = ColorTranslator.FromHtml("#F4A950")
                            Button15.Enabled = True
                            Button16.Enabled = False
                        Else
                            NotifyIcon("Hana Media Encoder", "Video profile has been saved!", 1000, True)
                            Button15.ForeColor = ColorTranslator.FromHtml("#161B21")
                            Button15.BackColor = ColorTranslator.FromHtml("#F4A950")
                            Button16.ForeColor = ColorTranslator.FromHtml("#F4A950")
                            Button16.BackColor = ColorTranslator.FromHtml("#161B21")
                            Button15.Enabled = False
                            Button16.Enabled = True
                        End If
                    End If
                Else
                    If DataGridView1.Rows.Count > 0 Then
                        contextstripstate1 = "save"
                        contextstripstate2 = "videotab"
                        ContextMenuStrip1.Show(Button15.PointToScreen(New Point(0, Button15.Height)))
                    Else
                        contextstripstate1 = ""
                        contextstripstate2 = ""
                    End If
                End If
            Else
                NotifyIcon("Hana Media Encoder", "Please re-select video stream to save video profile configuration", 1000, False)
            End If
        Else
            NotifyIcon("Hana Media Encoder", "Please check interlace configuration !", 1000, False)
        End If
    End Sub
    Private Sub RemoveVideoStream_Btn(sender As Object, e As EventArgs) Handles Button16.Click
        If MetroSetComboBox1.SelectedIndex >= 0 Then
            If MetroSetSwitch6.Switched = False Then
                VideoStreamFlags = VideoStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)) - 1).ToString & ".txt"
                VideoStreamConfig = VideoStreamConfigPath & Path.GetFileName(Textbox77.Text) & "_config_" & (CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)) - 1).ToString & ".txt"
                GC.Collect()
                GC.WaitForPendingFinalizers()
                File.Delete(VideoStreamFlags)
                File.Delete(VideoStreamConfig)
                VcodecReset()
                Button15.ForeColor = ColorTranslator.FromHtml("#161B21")
                Button15.BackColor = ColorTranslator.FromHtml("#F4A950")
                Button16.ForeColor = ColorTranslator.FromHtml("#F4A950")
                Button16.BackColor = ColorTranslator.FromHtml("#161B21")
                Button15.Enabled = True
                Button16.Enabled = False
            Else
                If DataGridView1.Rows.Count > 0 Then
                    contextstripstate1 = "remove"
                    contextstripstate2 = "videotab"
                    ContextMenuStrip1.Show(Button16.PointToScreen(New Point(0, Button16.Height)))
                Else
                    contextstripstate1 = ""
                    contextstripstate2 = ""
                End If
            End If
        Else
            NotifyIcon("Hana Media Encoder", "Please re-select video stream to remove video profile configuration", 1000, False)
            Button15.Enabled = False
            Button16.Enabled = True
        End If
    End Sub
    Private Sub VcodecReset()
        Dim GraphicCardName As String = GetGraphicsCardName("Name")
        Hwdefconfig = FindConfig("config.ini", "GPU Engine:")
        If Hwdefconfig = "GPU Engine:" Then
            HwAccelDev = ""
        Else
            HwAccelDev = Hwdefconfig.Remove(0, 11)
        End If
        If ComboBox2.Text = "Copy" Then
            BitRate_UpDown.Enabled = False
            ComboBox21.Enabled = False
            ComboBox10.Enabled = False
            ComboBox30.Enabled = False
            ComboBox8.Enabled = False
            MaxBitRate_UpDown.Enabled = False
            TileRowsUpDown1.Enabled = False
            TileColumnUpDown1.Enabled = False
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
            ComboBox35.Enabled = False
            ComboBox37.Enabled = False
            ComboBox38.Enabled = False
            ComboBox39.Enabled = False
            ComboBox40.Enabled = False
            ComboBox41.Enabled = False
            MetroSetCheckBox3.Enabled = False
            ComboBox22.Enabled = False
            ComboBox44.Enabled = False
            ComboBox29.Enabled = False
            ComboBox42.Enabled = False
        ElseIf HwAccelDev = "opencl" Then
            ComboBox21.Enabled = False
            ComboBox10.Enabled = False
            ComboBox14.Enabled = False
            ComboBox3.Text = "yuv420p"
            ComboBox3.Enabled = False
            TileRowsUpDown1.Enabled = False
            TileColumnUpDown1.Enabled = False
            If ComboBox2.Text = "HEVC" Then
                ComboBox7.Text = "main"
                ComboBox9.Enabled = True
                MetroSetCheckBox3.Enabled = False
            ElseIf ComboBox2.Text = "AV1" Then
                If ComboBox8.Items.Contains("6.2") Then
                    ComboBox8.Items.Remove("6.3")
                    ComboBox8.Items.Remove("7")
                    ComboBox8.Items.Remove("7.0")
                    ComboBox8.Items.Remove("7.1")
                    ComboBox8.Items.Remove("7.2")
                    ComboBox8.Items.Remove("7.3")
                End If
                MetroSetCheckBox3.Enabled = True
            Else
                ComboBox7.Enabled = True
                If ComboBox7.Items.Contains("main10") Then
                    ComboBox7.Items.Remove("main10")
                End If
                ComboBox9.Enabled = False
                MetroSetCheckBox3.Enabled = False
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
            ComboBox35.Enabled = False
            ComboBox37.Enabled = False
            If ComboBox32.SelectedText IsNot "" Then
                ComboBox40.Enabled = True
            Else
                ComboBox40.Enabled = False
            End If
            ComboBox38.Enabled = True
            If ComboBox38.SelectedIndex = 0 Then
                ComboBox39.Enabled = False
            Else
                ComboBox39.Enabled = True
            End If
            ComboBox41.Enabled = True
            ComboBox22.Enabled = True
            ComboBox44.Enabled = True
            ComboBox29.Enabled = True
            ComboBox42.Enabled = True
        ElseIf HwAccelDev = "qsv" Then
            ComboBox21.Enabled = False
            ComboBox10.Enabled = False
            ComboBox14.Enabled = False
            If ComboBox2.Text = "HEVC" Then
                If ComboBox7.Items.Contains("high") Then
                    ComboBox7.Items.Remove("high")
                End If
                TileRowsUpDown1.Enabled = False
                TileColumnUpDown1.Enabled = False
                MetroSetCheckBox3.Enabled = False
            ElseIf ComboBox2.Text = "AV1" Then
                If ComboBox8.Items.Contains("6.2") Then
                    ComboBox8.Items.Remove("6.3")
                    ComboBox8.Items.Remove("7")
                    ComboBox8.Items.Remove("7.0")
                    ComboBox8.Items.Remove("7.1")
                    ComboBox8.Items.Remove("7.2")
                    ComboBox8.Items.Remove("7.3")
                End If
                TileRowsUpDown1.Enabled = True
                TileColumnUpDown1.Enabled = True
                MetroSetCheckBox3.Enabled = True
            Else
                If ComboBox7.Items.Contains("main10") Then
                    ComboBox7.Items.Remove("main10")
                End If
                ComboBox9.Enabled = False
                MetroSetCheckBox3.Enabled = False
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
            If ComboBox32.SelectedText IsNot "" Then
                ComboBox40.Enabled = True
            Else
                ComboBox40.Enabled = False
            End If
            ComboBox38.Enabled = True
            If ComboBox38.SelectedIndex = 0 Then
                ComboBox39.Enabled = False
            Else
                ComboBox39.Enabled = True
            End If
            ComboBox40.Enabled = True
            ComboBox41.Enabled = True
            ComboBox22.Enabled = True
            ComboBox44.Enabled = True
            ComboBox29.Enabled = True
            ComboBox42.Enabled = True
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
                If mediaenginestate = "NVENCC" Then
                    If ComboBox7.Items.Contains("main10") Then
                        ComboBox7.Items.Remove("main10")
                    End If
                Else
                    If ComboBox7.Items.Contains("main10") = False Then
                        ComboBox7.Items.Add("main10")
                    End If
                End If
                ComboBox9.Enabled = True
                TileRowsUpDown1.Enabled = False
                TileColumnUpDown1.Enabled = False
                If mediaenginestate = "NVENCC" Then
                    MetroSetCheckBox3.Enabled = True
                Else
                    MetroSetCheckBox3.Enabled = False
                End If
            ElseIf ComboBox2.Text = "AV1" Then
                If ComboBox8.Items.Contains("6.2") Then
                    ComboBox8.Items.Remove("6.3")
                    ComboBox8.Items.Remove("7")
                    ComboBox8.Items.Remove("7.0")
                    ComboBox8.Items.Remove("7.1")
                    ComboBox8.Items.Remove("7.2")
                    ComboBox8.Items.Remove("7.3")
                End If
                ComboBox7.Enabled = False
                ComboBox9.Enabled = True
                TileRowsUpDown1.Enabled = True
                TileColumnUpDown1.Enabled = True
                MetroSetCheckBox3.Enabled = True
            Else
                If ComboBox7.Items.Contains("main10") Then
                    ComboBox7.Items.Remove("main10")
                End If
                ComboBox9.Enabled = False
                TileRowsUpDown1.Enabled = False
                TileColumnUpDown1.Enabled = False
                If mediaenginestate = "NVENCC" Then
                    MetroSetCheckBox3.Enabled = True
                Else
                    MetroSetCheckBox3.Enabled = False
                End If
            End If
            ComboBox4.Enabled = True
            ComboBox11.Enabled = True
            ComboBox12.Enabled = True
            ComboBox13.Enabled = True
            CRF_VBR_UpDown.Enabled = True
            ComboBox6.Enabled = True
            If mediaenginestate = "NVENCC" Then
                ComboBox32.Enabled = False
                ComboBox35.Enabled = False
                ComboBox40.Enabled = False
                ComboBox41.Enabled = False
                Dim NVENCCPixFmt = {"yuv420", "yuv444"}
                ComboBox3.SelectedText = ""
                ComboBox3.Items.Clear()
                ComboBox3.Items.AddRange(NVENCCPixFmt)
            Else
                ComboBox32.Enabled = True
                If ComboBox32.SelectedText IsNot "" Then
                    ComboBox40.Enabled = True
                Else
                    ComboBox40.Enabled = False
                End If
                ComboBox41.Enabled = True
                Dim FFMPEGPixFmt = {"yuv420p", "yuv444p", "p010le", "p016le"}
                ComboBox3.SelectedText = ""
                ComboBox3.Items.Clear()
                ComboBox3.Items.AddRange(FFMPEGPixFmt)
            End If
            ComboBox38.Enabled = True
            If ComboBox38.SelectedIndex = 0 Then
                ComboBox39.Enabled = False
            Else
                ComboBox39.Enabled = True
            End If
            If mediaenginestate = "NVENCC" Then
                ComboBox22.Enabled = False
                ComboBox44.Enabled = False
                ComboBox29.Enabled = False
                ComboBox42.Enabled = False
            Else
                ComboBox22.Enabled = True
                ComboBox44.Enabled = True
                ComboBox29.Enabled = True
                ComboBox42.Enabled = True
            End If
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
            NotifyIcon("Hana Media Encoder", "Enabling Adaptive Quantization will remove lossless profile in tier options !", 1000, True)
            ComboBox12.Enabled = True
            ComboBox13.Enabled = True
            If ComboBox6.Items.Contains("Lossless") Then
                ComboBox6.Items.Remove("Lossless")
            End If
        End If
    End Sub
    Private Sub ColorSpace_Check(sender As Object, e As EventArgs) Handles ComboBox38.SelectedIndexChanged
        Dim colorSpc_bt709 = {"BT.709"}
        Dim colorSpc_bt2020 = {"BT.2020 Constant", "BT.2020 Non Constant"}
        ComboBox39.Enabled = True
        If ComboBox38.SelectedIndex = 0 Then
            ComboBox39.Items.Clear()
            ComboBox39.Items.AddRange(colorSpc_bt709)
        ElseIf ComboBox38.SelectedIndex = 1 Then
            ComboBox39.Items.Clear()
            ComboBox39.Items.AddRange(colorSpc_bt2020)
        End If
    End Sub
    Private Function VideoCodecLock() As Boolean
        Dim result As Boolean
        If mediaenginestate = "NVENCC" Then
            'Bypassing for now
            result = True
        ElseIf MetroSetSwitch6.Switched = True And DataGridView1.Rows.Count > 0 Then
            result = True
        Else
            FlagsCount = MetroSetComboBox1.Items.Count
            FlagsResult = 0
            FlagsValue = 0
            For FlagsStart = 1 To FlagsCount
                If File.Exists(VideoStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (FlagsStart - 1).ToString & ".txt") Then
                    FlagsResult += 1
                Else
                    MissedFlags(FlagsStart) = FlagsStart
                End If
                FlagsValue += 1
            Next
            If FlagsResult = FlagsCount Then
                result = True
            Else
                For FlagsStart = 1 To FlagsValue
                    If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                        MessageBoxAdv.Show("Configuration for video profile with stream #0:" & MissedFlags(FlagsStart) & " not found", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Next
                result = False
            End If
        End If
        Return result
    End Function
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
                AQ_Pnl.Location = New Point(10, Vid_Enc_QC_Pnl.Location.Y + 205)
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
                AQ_Pnl.Location = New Point(10, Vid_Enc_QC_Pnl.Location.Y + 205)
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
            While Vid_Enc_QC_Pnl.Height <= 205
                Vid_Enc_QC_Pnl.Height += 1
            End While
            AQ_Pnl.Location = New Point(10, Vid_Enc_QC_Pnl.Location.Y + 205)
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
            While Vid_Enc_Add_Pnl.Height <= 163
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
    Private Sub EnableAudioCheck(sender As Object) Handles MetroSetSwitch2.SwitchedChanged
        If MetroSetSwitch2.Switched = True Then
            If Textbox77.Text.Equals("") = True Then
                If DataGridView1.Rows.Count > 0 Then
                    ComboBox15.Enabled = True
                    MetroSetComboBox3.Enabled = True
                    MetroSetCheckBox2.Enabled = True
                    Button17.Enabled = True
                    Button18.Enabled = True
                    MetroSetComboBox3.Items.Add("Stream #0:0")
                    NotifyIcon("Audio Profile", "Multi audio stream configuration are not supported when media queue is enable !", 1000, True)
                    MetroSetComboBox3.SelectedIndex = 0
                Else
                    MetroSetSwitch2.Switched = False
                    NotifyIcon("Audio Profile", "Please load media file before enable audio configuration", 1000, True)
                End If
            ElseIf Label44.Text.Equals("Not Detected") = True And TextBox16.Text Is "" Then
                MetroSetSwitch2.Switched = False
                NotifyIcon("Audio Profile", "Current media file does not contain any audio stream", 1000, True)
            Else
                ComboBox15.Enabled = True
                MetroSetComboBox3.Enabled = True
                MetroSetCheckBox2.Enabled = False
                Button17.Enabled = True
                Button18.Enabled = False
                MetroSetComboBox3.SelectedIndex = 0
            End If
        Else
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
            MetroSetComboBox3.Enabled = False
            MetroSetComboBox3.SelectedIndex = -1
            Button17.Enabled = False
            Button18.Enabled = False
        End If
    End Sub
    Private Sub AudioStream_Source(sender As Object, e As EventArgs) Handles MetroSetComboBox3.SelectedIndexChanged
        If MetroSetSwitch6.Switched = False Then
            If MetroSetComboBox3.SelectedIndex >= 0 Then
                AudiostreamFlags = AudioStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)) - 1).ToString & ".txt"
                AudiostreamConfig = AudioStreamConfigPath & Path.GetFileName(Textbox77.Text) & "_config_" & (CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)) - 1).ToString & ".txt"
                If File.Exists(AudiostreamFlags) And File.Exists(AudiostreamConfig) Then
                    Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Configuration for audio profile with stream #0:" & CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)).ToString & "  already exists !" & vbCrLf & vbCrLf &
                                                                          "Check previous configuration for audio profile with stream #0:" & CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)).ToString & " ?" &
                                                                          vbCrLf & "NOTE: This will replace current audio profile configuration", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
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
                        If RemoveWhitespace(Strings.Mid(prevAudioCodec, 15)) Is "" Then
                            If RemoveWhitespace(Strings.Mid(prevAudioCodec, 7)) IsNot "" Then
                                ComboBox15.Text = aCodecReverse(RemoveWhitespace(Strings.Mid(prevAudioCodec, 7)))
                            Else
                                ComboBox15.Text = "Copy"
                            End If
                        Else
                            ComboBox15.Text = aCodecReverse(Strings.Mid(prevAudioCodec, 15))
                        End If

                        If ComboBox15.Text = "WAV" Then
                            If Strings.Mid(prevAudioCodec, 15) = "pcm_s16le" Then
                                ComboBox18.Text = "16 Bit"
                            ElseIf Strings.Mid(prevAudioCodec, 15) = "pcm_s124le" Then
                                ComboBox18.Text = "24 Bit"
                            ElseIf Strings.Mid(prevAudioCodec, 15) = "pcm_s32le" Then
                                ComboBox18.Text = "32 Bit"
                            Else
                                ComboBox18.Text = "16 Bit"
                            End If
                        ElseIf ComboBox15.Text = "FLAC" Then
                            If RemoveWhitespace(Strings.Mid(prevAudioBitDepth, 10)) = "-sample_fmt:a:0s16" Then
                                ComboBox18.Text = "16 Bit"
                            ElseIf RemoveWhitespace(Strings.Mid(prevAudioBitDepth, 10)) = "-sample_fmt:a:0s32" Then
                                ComboBox18.Text = "24 Bit"
                            Else
                                ComboBox18.Text = "16 Bit"
                            End If
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
        End If
    End Sub
    Private Sub AudioCodecCheck(sender As Object, e As EventArgs) Handles ComboBox15.SelectedIndexChanged
        AudioBitDepthCheck()
        AudioFrequencyCheck()
        If ComboBox15.Text = "WAV" Then
            ComboBox16.Enabled = True
            If mediaenginestate = "NVENCC" Then
                ComboBox33.Enabled = False
                ComboBox34.Enabled = False
                ComboBox20.Enabled = False
                ComboBox17.Enabled = False
                ComboBox18.Enabled = True
            Else
                ComboBox33.Enabled = True
                ComboBox34.Enabled = False
                ComboBox20.Enabled = False
                ComboBox17.Enabled = False
                ComboBox18.Enabled = True
            End If
            ComboBox17.SelectedIndex = -1
            ComboBox19.Enabled = False
            If ComboBox33.Text IsNot "" Then
                ComboBox34.Enabled = True
            End If
        ElseIf ComboBox15.Text = "Copy" Then
            ComboBox16.Enabled = False
            ComboBox17.Enabled = False
            ComboBox18.Enabled = False
            ComboBox19.Enabled = False
            ComboBox20.Enabled = False
            ComboBox33.Enabled = False
            ComboBox34.Enabled = False
        ElseIf ComboBox15.Text = "MP3" Or ComboBox15.Text = "AAC" Or
            ComboBox15.Text = "MP2" Or ComboBox15.Text = "OPUS" Then
            ComboBox16.Enabled = True
            If mediaenginestate = "NVENCC" Then
                ComboBox33.Enabled = False
                ComboBox34.Enabled = False
                ComboBox20.Enabled = False
                ComboBox17.Enabled = False
            Else
                ComboBox33.Enabled = True
                ComboBox34.Enabled = False
                ComboBox17.Enabled = False
                ComboBox20.Enabled = True
            End If
            ComboBox18.Enabled = False
            ComboBox19.Enabled = False
            If ComboBox20.Text = "VBR" Then
                ComboBox17.Enabled = True
            ElseIf ComboBox20.Text = "CBR" Then
                ComboBox19.Enabled = True
            End If
            If ComboBox33.Text IsNot "" Then
                ComboBox34.Enabled = True
            End If
        ElseIf ComboBox15.Text = "FLAC" Then
            ComboBox16.Enabled = True
            If mediaenginestate = "NVENCC" Then
                ComboBox33.Enabled = False
                ComboBox34.Enabled = False
                ComboBox20.Enabled = False
                ComboBox17.Enabled = False
                ComboBox18.Enabled = False
            Else
                ComboBox17.Enabled = True
                ComboBox20.Enabled = False
                ComboBox33.Enabled = True
                ComboBox34.Enabled = False
                ComboBox18.Enabled = True
            End If
            ComboBox19.Enabled = False
            ComboBox20.SelectedIndex = -1
            If ComboBox33.Text IsNot "" Then
                ComboBox34.Enabled = True
            End If
        End If
    End Sub
    Private Sub SaveAudioStream_Btn(sender As Object, e As EventArgs) Handles Button17.Click
        If MetroSetComboBox3.SelectedIndex >= 0 Then
            If MetroSetSwitch6.Switched = False Then
                AudiostreamFlags = AudioStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)) - 1).ToString & ".txt"
                AudiostreamConfig = AudioStreamConfigPath & Path.GetFileName(Textbox77.Text) & "_config_" & (CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)) - 1).ToString & ".txt"
                If File.Exists(AudiostreamFlags) And File.Exists(AudiostreamConfig) Then
                    Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Configuration for audio profile with stream #0:" & CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)).ToString & " already exists !" & vbCrLf & vbCrLf &
                                                                      "Overwrite previous configuration ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If configResult = DialogResult.Yes Then
                        GC.Collect()
                        GC.WaitForPendingFinalizers()
                        File.Delete(AudiostreamFlags)
                        File.Delete(AudiostreamConfig)
                        AcodecReset()
                        AudioStreamInitConfig(False, 0, True)
                        If ReturnAudioStats = False Then
                            NotifyIcon("Hana Media Encoder", "Failed to save audio profile !", 1000, False)
                            Button17.ForeColor = ColorTranslator.FromHtml("#F4A950")
                            Button17.BackColor = ColorTranslator.FromHtml("#161B21")
                            Button18.ForeColor = ColorTranslator.FromHtml("#161B21")
                            Button18.BackColor = ColorTranslator.FromHtml("#F4A950")
                            Button17.Enabled = True
                            Button18.Enabled = False
                        Else
                            NotifyIcon("Hana Media Encoder", "Audio profile has been saved!", 1000, True)
                            Button17.ForeColor = ColorTranslator.FromHtml("#161B21")
                            Button17.BackColor = ColorTranslator.FromHtml("#F4A950")
                            Button18.ForeColor = ColorTranslator.FromHtml("#F4A950")
                            Button18.BackColor = ColorTranslator.FromHtml("#161B21")
                            Button17.Enabled = False
                            Button18.Enabled = True
                        End If
                    End If
                Else
                    AudioStreamInitConfig(False, 0, True)
                    If ReturnAudioStats = False Then
                        NotifyIcon("Hana Media Encoder", "Failed to save audio profile !", 1000, False)
                        Button17.ForeColor = ColorTranslator.FromHtml("#F4A950")
                        Button17.BackColor = ColorTranslator.FromHtml("#161B21")
                        Button18.ForeColor = ColorTranslator.FromHtml("#161B21")
                        Button18.BackColor = ColorTranslator.FromHtml("#F4A950")
                        Button17.Enabled = True
                        Button18.Enabled = False
                    Else
                        NotifyIcon("Hana Media Encoder", "Audio profile has been saved!", 1000, True)
                        Button17.ForeColor = ColorTranslator.FromHtml("#161B21")
                        Button17.BackColor = ColorTranslator.FromHtml("#F4A950")
                        Button18.ForeColor = ColorTranslator.FromHtml("#F4A950")
                        Button18.BackColor = ColorTranslator.FromHtml("#161B21")
                        Button17.Enabled = False
                        Button18.Enabled = True
                    End If
                End If
            Else
                If DataGridView1.Rows.Count > 0 Then
                    contextstripstate1 = "save"
                    If MetroSetComboBox3.Items.Count > 0 And MetroSetComboBox1.Items.Count > 0 Then
                        contextstripstate2 = "audiotab"
                        contextstripstate3 = "video + audio"
                    Else
                        contextstripstate2 = "audiotab"
                        contextstripstate3 = ""
                    End If
                    ContextMenuStrip1.Show(Button17.PointToScreen(New Point(0, Button17.Height)))
                Else
                    contextstripstate1 = ""
                End If
            End If
        Else
            NotifyIcon("Hana Media Encoder", "Please re-select audio stream to save profile configuration", 1000, False)
        End If
    End Sub
    Private Sub videocontainerqueue_Btn(sender As Object, e As EventArgs) Handles MetroSetComboBox2.SelectedIndexChanged
        If MetroSetSwitch6.Switched = True And DataGridView1.Rows.Count > 0 Then
            If MetroSetComboBox2.Text = "MPEG-4" Then
                ComboBox15.SelectedIndex = -1
                ComboBox15.Items.Clear()
                ComboBox15.Items.Add("AAC")
                ComboBox15.Items.Add("MP3")
                ComboBox15.Items.Add("FLAC")
            ElseIf MetroSetComboBox2.Text = "Matroska" Then
                ComboBox15.SelectedIndex = -1
                ComboBox15.Items.Clear()
                ComboBox15.Items.Add("Copy")
                ComboBox15.Items.Add("AAC")
                ComboBox15.Items.Add("MP3")
                ComboBox15.Items.Add("MP2")
                ComboBox15.Items.Add("OPUS")
                ComboBox15.Items.Add("FLAC")
                ComboBox15.Items.Add("WAV")
            End If
        End If
    End Sub
    Private Sub RemoveAudioStream_Btn(sender As Object, e As EventArgs) Handles Button18.Click
        If MetroSetComboBox3.SelectedIndex >= 0 Then
            If MetroSetSwitch6.Switched = False Then
                AudiostreamFlags = AudioStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)) - 1).ToString & ".txt"
                AudiostreamConfig = AudioStreamConfigPath & Path.GetFileName(Textbox77.Text) & "_config_" & (CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)) - 1).ToString & ".txt"
                GC.Collect()
                GC.WaitForPendingFinalizers()
                File.Delete(AudiostreamFlags)
                File.Delete(AudiostreamConfig)
                AcodecReset()
                Button17.ForeColor = ColorTranslator.FromHtml("#161B21")
                Button17.BackColor = ColorTranslator.FromHtml("#F4A950")
                Button18.ForeColor = ColorTranslator.FromHtml("#F4A950")
                Button18.BackColor = ColorTranslator.FromHtml("#161B21")
                Button17.Enabled = True
                Button18.Enabled = False
            Else
                If DataGridView1.Rows.Count > 0 Then
                    contextstripstate1 = "remove"
                    contextstripstate2 = "audiotab"
                    ContextMenuStrip1.Show(Button17.PointToScreen(New Point(0, Button17.Height)))
                Else
                    contextstripstate1 = ""
                    contextstripstate2 = ""
                End If
            End If
        Else
            NotifyIcon("Hana Media Encoder", "Please re-select audio stream to remove profile configuration", 1000, False)
        End If
    End Sub
    Private Sub contextmenustrip_submenu1_onclick(sender As Object, e As EventArgs) Handles SelectedQueueToolStripMenuItem.Click
        If mediaenginestate = "NVENCC" Then
            NotifyIcon("Hana Media Encoder", "This features are not supported when using NVENCC", 1000, True)
        Else
            If MetroSetSwitch6.Switched = True And DataGridView1.Rows.Count > 0 Then
                If contextstripstate2 = "videotab" Then
                    If contextstripstate1 = "save" Then
                        For i As Integer = 0 To DataGridView1.Rows.Count - 1
                            If DataGridView1.Rows(i).Cells(0).Value = True Then
                                VideoStreamFlags = VideoQueueFlagsPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_flags_" & CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)).ToString & ".txt"
                                VideoStreamConfig = VideoQueueConfigPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_config_" & CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)).ToString & ".txt"
                                If File.Exists(VideoStreamFlags) Then
                                    File.Delete(VideoStreamFlags)
                                End If
                                If File.Exists(VideoStreamConfig) Then
                                    File.Delete(VideoStreamConfig)
                                End If
                                Dim tempValue As String = DataGridView1.Rows(i).Cells(8).Value.ToString
                                Dim fps As String
                                Dim maxbitrate As String
                                Dim ovrbitrate As String
                                Dim videoProfile As String
                                Dim videoPixFmt As String
                                Dim videoContainer As String
                                If BitRate_UpDown.Value.ToString = "" Or BitRate_UpDown.Value <= 0 Then
                                    ovrbitrate = ""
                                Else
                                    ovrbitrate = ", bitrate: " + BitRate_UpDown.Value.ToString + "MB/s"
                                End If
                                If MaxBitRate_UpDown.Value.ToString = "" Or MaxBitRate_UpDown.Value <= 0 Then
                                    maxbitrate = ""
                                Else
                                    maxbitrate = ", max bitrate: " + MaxBitRate_UpDown.Value.ToString + "MB/s"
                                End If
                                If ComboBox30.SelectedIndex < 0 Then
                                    fps = ""
                                Else
                                    fps = ", fps: " + ComboBox30.Text.ToString
                                End If
                                If RemoveWhitespace(ComboBox7.Text.ToString) = "" Then
                                    videoProfile = ""
                                Else
                                    videoProfile = ", Profile: " + RemoveWhitespace(ComboBox7.Text.ToString)
                                End If
                                If RemoveWhitespace(ComboBox3.Text.ToString) = "" Then
                                    videoPixFmt = ""
                                Else
                                    videoPixFmt = ", Pixel Format: " + RemoveWhitespace(ComboBox3.Text.ToString)
                                End If
                                If RemoveWhitespace(MetroSetComboBox2.Text.ToString) = "" Then
                                    videoContainer = ""
                                Else
                                    videoContainer = ", Container: " + MetroSetComboBox2.Text.ToString
                                End If

                                If DataGridView1.Rows(i).Cells(5).Value.ToString.Contains("Video File") = True Then
                                    If MetroSetCheckBox1.Checked = True Then
                                        VideoStreamInitConfig(True, i, False)
                                    Else
                                        VideoStreamInitConfig(True, i, True)
                                    End If
                                    If DataGridView1.Rows(i).Cells(8).Value.ToString.Contains("Audio") = True Then
                                        Dim tempVal As String = DataGridView1.Rows(i).Cells(8).Value.ToString
                                        If getBetween(DataGridView1.Rows(i).Cells(8).Value.ToString, "Video:", "Audio:") IsNot "" Then
                                            If MetroSetCheckBox1.Checked = True Then
                                                DataGridView1.Rows(i).Cells(8).Value = "Video: No Video, " + DataGridView1.Rows(i).Cells(8).Value.ToString.Substring(DataGridView1.Rows(i).Cells(8).Value.ToString.LastIndexOf("Audio:"))
                                            Else
                                                DataGridView1.Rows(i).Cells(8).Value = "Video: " + ComboBox2.Text.ToString + videoProfile + videoPixFmt + ovrbitrate + maxbitrate + fps + videoContainer +
                                               ", " + DataGridView1.Rows(i).Cells(8).Value.ToString.Substring(DataGridView1.Rows(i).Cells(8).Value.ToString.LastIndexOf("Audio:"))
                                            End If
                                        Else
                                            If MetroSetCheckBox1.Checked = True Then
                                                DataGridView1.Rows(i).Cells(8).Value = "Video: No Video, " + DataGridView1.Rows(i).Cells(8).Value.ToString.Substring(DataGridView1.Rows(i).Cells(8).Value.ToString.LastIndexOf("Audio:"))
                                            Else
                                                DataGridView1.Rows(i).Cells(8).Value = "Video: " + ComboBox2.Text.ToString + videoProfile + videoPixFmt + ovrbitrate + maxbitrate + fps + videoContainer +
                                                                                       ", " + DataGridView1.Rows(i).Cells(8).Value.ToString.Substring(DataGridView1.Rows(i).Cells(8).Value.ToString.LastIndexOf("Audio:"))
                                            End If
                                        End If
                                    Else
                                        If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                            If MetroSetCheckBox1.Checked = True Then
                                                DataGridView1.Rows(i).Cells(8).Value = "Video: No Video"
                                            Else
                                                DataGridView1.Rows(i).Cells(8).Value = "Video: " + ComboBox2.Text.ToString + videoProfile + videoPixFmt + ovrbitrate + maxbitrate + fps + videoContainer
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next
                        Button15.ForeColor = ColorTranslator.FromHtml("#F4A950")
                        Button15.BackColor = ColorTranslator.FromHtml("#161B21")
                        Button16.ForeColor = ColorTranslator.FromHtml("#161B21")
                        Button16.BackColor = ColorTranslator.FromHtml("#F4A950")
                        Button15.Enabled = False
                        Button16.Enabled = True
                    ElseIf contextstripstate1 = "remove" Then
                        For i As Integer = 0 To DataGridView1.Rows.Count - 1
                            If DataGridView1.Rows(i).Cells(0).Value = True Then
                                VideoStreamFlags = VideoQueueFlagsPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_flags_" & CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)).ToString & ".txt"
                                VideoStreamConfig = VideoQueueConfigPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_config_" & CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)).ToString & ".txt"
                                If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                                    If File.Exists(VideoStreamFlags) Then
                                        File.Delete(VideoStreamFlags)
                                    End If
                                    If File.Exists(VideoStreamConfig) Then
                                        File.Delete(VideoStreamConfig)
                                    End If
                                    If DataGridView1.Rows(i).Cells(8).Value.ToString.Contains("Audio") = True Then
                                        DataGridView1.Rows(i).Cells(8).Value = DataGridView1.Rows(i).Cells(8).Value.ToString.Substring(DataGridView1.Rows(i).Cells(8).Value.ToString.LastIndexOf("Audio:"))
                                    Else
                                        DataGridView1.Rows(i).Cells(8).Value = ""
                                    End If
                                End If
                            End If
                        Next
                        Button15.ForeColor = ColorTranslator.FromHtml("#161B21")
                        Button15.BackColor = ColorTranslator.FromHtml("#F4A950")
                        Button16.ForeColor = ColorTranslator.FromHtml("#F4A950")
                        Button16.BackColor = ColorTranslator.FromHtml("#161B21")
                        Button15.Enabled = True
                        Button16.Enabled = False
                    Else
                        NotifyIcon("Hana Media Encoder", "Invalid option!", 1000, False)
                    End If
                ElseIf contextstripstate2 = "audiotab" Then
                    If contextstripstate1 = "save" Then
                        For i As Integer = 0 To DataGridView1.Rows.Count - 1
                            If DataGridView1.Rows(i).Cells(0).Value = True Then
                                AudiostreamFlags = AudioQueueFlagsPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_flags_" & CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)).ToString & ".txt"
                                AudiostreamConfig = AudioQueueConfigPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_config_" & CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)).ToString & ".txt"
                                Dim bitdepth As String
                                Dim afreq As String
                                Dim achn As String
                                If ComboBox16.Text.ToString IsNot "" Then
                                    If aLibraryTrs(ComboBox15.Text.ToString, ComboBox18.Text.ToString) = "copy" Then
                                        afreq = ""
                                    Else
                                        afreq = ", " + (CInt(ComboBox16.Text.ToString) / 1000).ToString + " KHz, "
                                    End If
                                Else
                                    afreq = ""
                                End If
                                If ComboBox18.Text.ToString IsNot "" Then
                                    bitdepth = ComboBox18.Text.ToString
                                Else
                                    If aLibraryTrs(ComboBox15.Text.ToString, ComboBox18.Text.ToString) = "copy" Then
                                        bitdepth = ""
                                    Else
                                        bitdepth = "16 Bit"
                                    End If
                                End If
                                If ComboBox33.Text.ToString IsNot "" Then
                                    If aLibraryTrs(ComboBox15.Text.ToString, ComboBox18.Text.ToString) = "copy" Then
                                        achn = ""
                                    Else
                                        If ComboBox33.Text.ToString = "stereo" Then
                                            achn = ", " + ComboBox33.Text.ToString
                                        Else
                                            achn = ", " + ComboBox33.Text.ToString + " channels"
                                        End If
                                    End If
                                Else
                                    achn = ""
                                End If
                                If MetroSetCheckBox2.Checked = False Then
                                    AudioStreamInitConfig(True, i, True)
                                Else
                                    AudioStreamInitConfig(True, i, False)
                                End If
                                If ReturnAudioStats Then
                                    If DataGridView1.Rows(i).Cells(8).Value.ToString.Contains("Video") = True Then
                                        Dim tempVal As String = DataGridView1.Rows(i).Cells(8).Value.ToString
                                        If getBetween(DataGridView1.Rows(i).Cells(8).Value.ToString, "Video:", "Audio:") IsNot "" Then
                                            If MetroSetCheckBox2.Checked = False Then
                                                DataGridView1.Rows(i).Cells(8).Value = "Video: " + getBetween(tempVal, "Video:", "Audio:") + ", Audio: " + aLibraryTrs(ComboBox15.Text.ToString, ComboBox18.Text.ToString) +
                                                                                        afreq + bitdepth + achn
                                            Else
                                                DataGridView1.Rows(i).Cells(8).Value = "Video: " + getBetween(tempVal, "Video:", "Audio:") + ", Audio: No Audio"
                                            End If
                                        Else
                                            If MetroSetCheckBox2.Checked = False Then
                                                DataGridView1.Rows(i).Cells(8).Value = tempVal + ", Audio: " + aLibraryTrs(ComboBox15.Text.ToString, ComboBox18.Text.ToString) +
                                                                                            afreq + bitdepth + achn
                                            Else
                                                DataGridView1.Rows(i).Cells(8).Value = tempVal + ", Audio: No Audio"
                                            End If
                                        End If
                                    Else
                                        If MetroSetCheckBox2.Checked = False Then
                                            DataGridView1.Rows(i).Cells(8).Value = "Audio: " + aLibraryTrs(ComboBox15.Text.ToString, ComboBox18.Text.ToString) + afreq + bitdepth + achn
                                        Else
                                            DataGridView1.Rows(i).Cells(8).Value = "Audio: No Audio"
                                        End If
                                    End If
                                    Button17.ForeColor = ColorTranslator.FromHtml("#F4A950")
                                    Button17.BackColor = ColorTranslator.FromHtml("#161B21")
                                    Button18.ForeColor = ColorTranslator.FromHtml("#161B21")
                                    Button18.BackColor = ColorTranslator.FromHtml("#F4A950")
                                    Button17.Enabled = False
                                    Button18.Enabled = True
                                Else
                                    NotifyIcon("Hana Media Encoder", "Failed to save audio profile!", 1000, True)
                                End If
                            End If
                        Next
                    ElseIf contextstripstate1 = "remove" Then
                        For i As Integer = 0 To DataGridView1.Rows.Count - 1
                            If DataGridView1.Rows(i).Cells(0).Value = True Then
                                AudiostreamFlags = AudioQueueFlagsPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_flags_" & CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)).ToString & ".txt"
                                AudiostreamConfig = AudioQueueConfigPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_config_" & CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)).ToString & ".txt"
                                If File.Exists(AudiostreamFlags) Then
                                    File.Delete(AudiostreamFlags)
                                End If
                                If File.Exists(AudiostreamConfig) Then
                                    File.Delete(AudiostreamConfig)
                                End If
                                If DataGridView1.Rows(i).Cells(8).Value.ToString.Contains("Video") = True Then
                                    DataGridView1.Rows(i).Cells(8).Value = "Video: " + getBetween(DataGridView1.Rows(i).Cells(8).Value.ToString, "Video:", ", Audio:")
                                Else
                                    DataGridView1.Rows(i).Cells(8).Value = ""
                                End If
                            End If
                        Next
                        Button17.ForeColor = ColorTranslator.FromHtml("#161B21")
                        Button17.BackColor = ColorTranslator.FromHtml("#F4A950")
                        Button18.ForeColor = ColorTranslator.FromHtml("#F4A950")
                        Button18.BackColor = ColorTranslator.FromHtml("#161B21")
                        Button17.Enabled = True
                        Button18.Enabled = False
                    Else
                        NotifyIcon("Hana Media Encoder", "Invalid option!", 1000, False)
                    End If
                Else
                    NotifyIcon("Hana Media Encoder", "Invalid option!", 1000, False)
                End If
            Else
                NotifyIcon("Hana Media Encoder", "Media queue is empty !", 1000, False)
            End If
            DataGridView1.Update()
            DataGridView1.Refresh()
        End If
    End Sub
    Private Sub contextmenustrip_submenu2_onclick(sender As Object, e As EventArgs) Handles AllQueueToolStripMenuItem.Click
        If mediaenginestate = "NVENCC" Then
            NotifyIcon("Hana Media Encoder", "This features are not supported when using NVENCC", 1000, True)
        Else
            If MetroSetSwitch6.Switched = True And DataGridView1.Rows.Count > 0 Then
                If contextstripstate2 = "videotab" Then
                    If contextstripstate1 = "save" Then
                        For i As Integer = 0 To DataGridView1.Rows.Count - 1
                            VideoStreamFlags = VideoQueueFlagsPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_flags_" & CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)).ToString & ".txt"
                            VideoStreamConfig = VideoQueueConfigPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_config_" & CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)).ToString & ".txt"
                            If File.Exists(VideoStreamFlags) Then
                                File.Delete(VideoStreamFlags)
                            End If
                            If File.Exists(VideoStreamConfig) Then
                                File.Delete(VideoStreamConfig)
                            End If
                            Dim tempValue As String = DataGridView1.Rows(i).Cells(8).Value.ToString
                            Dim fps As String
                            Dim maxbitrate As String
                            Dim ovrbitrate As String
                            Dim videoProfile As String
                            Dim videoPixFmt As String
                            Dim videoContainer As String
                            If BitRate_UpDown.Value.ToString = "" Or BitRate_UpDown.Value <= 0 Then
                                ovrbitrate = ""
                            Else
                                ovrbitrate = ", bitrate: " + BitRate_UpDown.Value.ToString + "MB/s"
                            End If
                            If MaxBitRate_UpDown.Value.ToString = "" Or MaxBitRate_UpDown.Value <= 0 Then
                                maxbitrate = ""
                            Else
                                maxbitrate = ", max bitrate: " + MaxBitRate_UpDown.Value.ToString + "MB/s"
                            End If
                            If ComboBox30.SelectedIndex < 0 Then
                                fps = ""
                            Else
                                fps = ", fps: " + ComboBox30.Text.ToString
                            End If
                            If RemoveWhitespace(ComboBox7.Text.ToString) = "" Then
                                videoProfile = ""
                            Else
                                videoProfile = ", Profile: " + RemoveWhitespace(ComboBox7.Text.ToString)
                            End If
                            If RemoveWhitespace(ComboBox3.Text.ToString) = "" Then
                                videoPixFmt = ""
                            Else
                                videoPixFmt = ", Pixel Format: " + RemoveWhitespace(ComboBox3.Text.ToString)
                            End If
                            If RemoveWhitespace(MetroSetComboBox2.Text.ToString) = "" Then
                                videoContainer = ""
                            Else
                                videoContainer = ", Container: " + MetroSetComboBox2.Text.ToString
                            End If
                            If DataGridView1.Rows(i).Cells(5).Value.ToString.Contains("Video File") = True Then
                                If MetroSetCheckBox1.Checked = False Then
                                    VideoStreamInitConfig(True, i, True)
                                Else
                                    VideoStreamInitConfig(True, i, False)
                                End If
                                If DataGridView1.Rows(i).Cells(8).Value.ToString.Contains("Audio") = True Then
                                    Dim tempVal As String = DataGridView1.Rows(i).Cells(8).Value.ToString
                                    If getBetween(DataGridView1.Rows(i).Cells(8).Value.ToString, "Video:", "Audio:") = "" Then
                                        If MetroSetCheckBox1.Checked = True Then
                                            DataGridView1.Rows(i).Cells(8).Value = "Video: No Video, " + DataGridView1.Rows(i).Cells(8).Value.ToString.Substring(DataGridView1.Rows(i).Cells(8).Value.ToString.LastIndexOf("Audio:"))
                                        Else
                                            DataGridView1.Rows(i).Cells(8).Value = "Video: " + ComboBox2.Text.ToString + videoProfile + videoPixFmt + ovrbitrate + maxbitrate + fps + videoContainer +
                                               ", " + DataGridView1.Rows(i).Cells(8).Value.ToString.Substring(DataGridView1.Rows(i).Cells(8).Value.ToString.LastIndexOf("Audio:"))
                                        End If
                                    Else
                                        If MetroSetCheckBox1.Checked = True Then
                                            DataGridView1.Rows(i).Cells(8).Value = "Video: No Video, " + DataGridView1.Rows(i).Cells(8).Value.ToString.Substring(DataGridView1.Rows(i).Cells(8).Value.ToString.LastIndexOf("Audio:"))
                                        Else
                                            DataGridView1.Rows(i).Cells(8).Value = "Video: " + ComboBox2.Text.ToString + videoProfile + videoPixFmt + ovrbitrate + maxbitrate + fps + videoContainer +
                                                                                       ", " + DataGridView1.Rows(i).Cells(8).Value.ToString.Substring(DataGridView1.Rows(i).Cells(8).Value.ToString.LastIndexOf("Audio:"))
                                        End If
                                    End If
                                Else
                                    If MetroSetCheckBox1.Checked = True Then
                                        DataGridView1.Rows(i).Cells(8).Value = "Video: No Video"
                                    Else
                                        DataGridView1.Rows(i).Cells(8).Value = "Video: " + ComboBox2.Text.ToString + videoProfile + videoPixFmt + ovrbitrate + maxbitrate + fps + videoContainer
                                    End If
                                End If
                            End If
                        Next
                        Button15.ForeColor = ColorTranslator.FromHtml("#F4A950")
                        Button15.BackColor = ColorTranslator.FromHtml("#161B21")
                        Button16.ForeColor = ColorTranslator.FromHtml("#161B21")
                        Button16.BackColor = ColorTranslator.FromHtml("#F4A950")
                        Button15.Enabled = False
                        Button16.Enabled = True
                    ElseIf contextstripstate1 = "remove" Then
                        For i As Integer = 0 To DataGridView1.Rows.Count - 1
                            If DataGridView1.Rows(i).Cells(8).Value.ToString.Contains("Audio") = True Then
                                DataGridView1.Rows(i).Cells(8).Value = DataGridView1.Rows(i).Cells(8).Value.ToString.Substring(DataGridView1.Rows(i).Cells(8).Value.ToString.LastIndexOf("Audio:"))
                            Else
                                DataGridView1.Rows(i).Cells(8).Value = ""
                            End If
                        Next
                        Button15.ForeColor = ColorTranslator.FromHtml("#161B21")
                        Button15.BackColor = ColorTranslator.FromHtml("#F4A950")
                        Button16.ForeColor = ColorTranslator.FromHtml("#F4A950")
                        Button16.BackColor = ColorTranslator.FromHtml("#161B21")
                        Button15.Enabled = True
                        Button16.Enabled = False
                    Else
                        NotifyIcon("Hana Media Encoder", "Invalid option!", 1000, False)
                    End If
                ElseIf contextstripstate2 = "audiotab" Then
                    If contextstripstate1 = "save" Then
                        For i As Integer = 0 To DataGridView1.Rows.Count - 1
                            AudiostreamFlags = AudioQueueFlagsPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_flags_" & CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)).ToString & ".txt"
                            AudiostreamConfig = AudioQueueConfigPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_config_" & CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)).ToString & ".txt"
                            If File.Exists(AudiostreamFlags) Then
                                File.Delete(AudiostreamFlags)
                            End If
                            If File.Exists(AudiostreamConfig) Then
                                File.Delete(AudiostreamConfig)
                            End If
                            Dim bitdepth As String
                            Dim afreq As String
                            Dim achn As String
                            If ComboBox16.Text.ToString IsNot "" Then
                                afreq = ", " + (CInt(ComboBox16.Text.ToString) / 1000).ToString + " KHz, "
                            Else
                                afreq = ""
                            End If
                            If ComboBox18.Text.ToString IsNot "" Then
                                bitdepth = ComboBox18.Text.ToString
                            Else
                                If aLibraryTrs(ComboBox15.Text.ToString, ComboBox18.Text.ToString) = "copy" Then
                                    bitdepth = ""
                                Else
                                    bitdepth = "16 Bit"
                                End If
                            End If
                            If ComboBox33.Text.ToString IsNot "" Then
                                If ComboBox33.Text.ToString = "stereo" Then
                                    achn = ", " + ComboBox33.Text.ToString
                                Else
                                    achn = ", " + ComboBox33.Text.ToString + " channels"
                                End If
                            Else
                                achn = ""
                            End If
                            If MetroSetCheckBox2.Checked = False Then
                                AudioStreamInitConfig(True, i, True)
                            Else
                                AudioStreamInitConfig(True, i, False)
                            End If
                            If ReturnAudioStats Then
                                If DataGridView1.Rows(i).Cells(8).Value.ToString.Contains("Video") = True Then
                                    Dim tempVal As String = DataGridView1.Rows(i).Cells(8).Value.ToString
                                    If getBetween(DataGridView1.Rows(i).Cells(8).Value.ToString, "Video:", "Audio:") = "" Then
                                        If MetroSetCheckBox2.Checked = False Then
                                            DataGridView1.Rows(i).Cells(8).Value = tempVal + ", Audio: " + aLibraryTrs(ComboBox15.Text.ToString, ComboBox18.Text.ToString) +
                                                                                        afreq + bitdepth + achn
                                        Else
                                            DataGridView1.Rows(i).Cells(8).Value = tempVal + ", Audio: No Audio"
                                        End If
                                    Else
                                        If MetroSetCheckBox2.Checked = False Then
                                            DataGridView1.Rows(i).Cells(8).Value = tempVal + ", Audio: " + aLibraryTrs(ComboBox15.Text.ToString, ComboBox18.Text.ToString) +
                                                                                            afreq + bitdepth + achn
                                        Else
                                            DataGridView1.Rows(i).Cells(8).Value = tempVal + ", Audio: No Audio"
                                        End If
                                    End If
                                Else
                                    If MetroSetCheckBox2.Checked = False Then
                                        DataGridView1.Rows(i).Cells(8).Value = "Audio: " + aLibraryTrs(ComboBox15.Text.ToString, ComboBox18.Text.ToString) + afreq + bitdepth + achn
                                    Else
                                        DataGridView1.Rows(i).Cells(8).Value = "Audio: No Audio"
                                    End If
                                End If
                                Button17.ForeColor = ColorTranslator.FromHtml("#F4A950")
                                Button17.BackColor = ColorTranslator.FromHtml("#161B21")
                                Button18.ForeColor = ColorTranslator.FromHtml("#161B21")
                                Button18.BackColor = ColorTranslator.FromHtml("#F4A950")
                                Button17.Enabled = False
                                Button18.Enabled = True
                            Else
                                NotifyIcon("Hana Media Encoder", "Failed to save audio profile!", 1000, True)
                            End If
                        Next
                    ElseIf contextstripstate1 = "remove" Then
                        For i As Integer = 0 To DataGridView1.Rows.Count - 1
                            AudiostreamFlags = AudioQueueFlagsPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_flags_" & CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)).ToString & ".txt"
                            AudiostreamConfig = AudioQueueConfigPath & DataGridView1.Rows(i).Cells(2).Value.ToString & "_config_" & CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)).ToString & ".txt"
                            If File.Exists(AudiostreamFlags) Then
                                File.Delete(AudiostreamFlags)
                            End If
                            If File.Exists(AudiostreamConfig) Then
                                File.Delete(AudiostreamConfig)
                            End If
                            If DataGridView1.Rows(i).Cells(8).Value.ToString.Contains("Video") = True Then
                                DataGridView1.Rows(i).Cells(8).Value = "Video: " + getBetween(DataGridView1.Rows(i).Cells(8).Value.ToString, "Video:", ", Audio:")
                            Else
                                DataGridView1.Rows(i).Cells(8).Value = ""
                            End If
                        Next
                        Button17.ForeColor = ColorTranslator.FromHtml("#161B21")
                        Button17.BackColor = ColorTranslator.FromHtml("#F4A950")
                        Button18.ForeColor = ColorTranslator.FromHtml("#F4A950")
                        Button18.BackColor = ColorTranslator.FromHtml("#161B21")
                        Button17.Enabled = True
                        Button18.Enabled = False
                    Else
                        NotifyIcon("Hana Media Encoder", "Invalid option!", 1000, False)
                    End If
                Else
                    NotifyIcon("Hana Media Encoder", "Invalid option !", 1000, False)
                End If
            Else
                NotifyIcon("Hana Media Encoder", "Media queue is empty !", 1000, False)
            End If
            DataGridView1.Update()
            DataGridView1.Refresh()
        End If
    End Sub
    Private Sub AudioStreamInitConfig(queue As Boolean, rows As Integer, codec As Boolean)
        If MetroSetComboBox3.SelectedIndex >= 0 Then
            If codec = False Then
                HMEStreamProfileGenerate(AudiostreamFlags, " -an ")
                HMEAudioStreamConfigGenerate(AudiostreamConfig, "", "", "", "", "", "", "", "")
                ReturnAudioStats = True
            Else
                If queue Then
                    AudiostreamFlags = AudioQueueFlagsPath & DataGridView1.Rows(rows).Cells(2).Value.ToString & "_flags_" & CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)).ToString & ".txt"
                    AudiostreamConfig = AudioQueueConfigPath & DataGridView1.Rows(rows).Cells(2).Value.ToString & "_config_" & CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)).ToString & ".txt"
                    AudioStreamSourceList = CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)).ToString
                Else
                    AudiostreamFlags = AudioStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)) - 1).ToString & ".txt"
                    AudiostreamConfig = AudioStreamConfigPath & Path.GetFileName(Textbox77.Text) & "_config_" & (CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)) - 1).ToString & ".txt"
                    AudioStreamSourceList = (CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)) - 1).ToString
                End If
                Dim channel_layout As String
                If ComboBox34.Text = "" Then
                    channel_layout = ""
                Else
                    channel_layout = " -filter:a:" & AudioStreamSourceList & " aformat=channel_layouts=" & ComboBox34.Text
                End If
                If mediaenginestate = "NVENCC" Then
                    If ComboBox15.Text = "Copy" Then
                        HMEStreamProfileGenerate(AudiostreamFlags, " --audio-copy")
                        HMEAudioStreamConfigGenerate(AudiostreamConfig, "copy", "", "", "", "", "", "", "")
                    ElseIf ComboBox15.Text = "WAV" Or ComboBox15.Text = "FLAC" Then
                        HMEStreamProfileGenerate(AudiostreamFlags, aCodecAlt(ComboBox15.Text, ComboBox18.Text) & aSampleRateAlt(ComboBox16.Text))
                        HMEAudioStreamConfigGenerate(AudiostreamConfig, ComboBox15.Text, ComboBox18.Text, "", "", "", "", ComboBox16.Text, "")
                    Else
                        HMEStreamProfileGenerate(AudiostreamFlags, aCodecAlt(ComboBox15.Text, ComboBox18.Text) & aBitRateAlt(ComboBox17.Text) & aSampleRateAlt(ComboBox16.Text))
                        HMEAudioStreamConfigGenerate(AudiostreamConfig, ComboBox15.Text, "", ComboBox17.Text, "", "", "", ComboBox16.Text, "")
                    End If
                    ReturnAudioStats = True
                Else
                    If ComboBox15.Text = "MP3" Or ComboBox15.Text = "AAC" Or
                    ComboBox15.Text = "MP2" Or ComboBox15.Text = "OPUS" Then
                        Dim Lossy As String
                        If ComboBox15.Text = "OPUS" Then
                            Lossy = "OPUS"
                        Else
                            Lossy = "MP3"
                        End If
                        If ComboBox20.Text = "CBR" Then
                            HMEStreamProfileGenerate(AudiostreamFlags, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList) & aChannel(ComboBox33.Text, AudioStreamSourceList) &
                                                             aBitRate(ComboBox19.Text, AudioStreamSourceList, Lossy, "CBR") & aSampleRate(ComboBox16.Text, AudioStreamSourceList) & channel_layout)
                            HMEAudioStreamConfigGenerate(AudiostreamConfig, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList), "", "CBR", ComboBox19.Text, ComboBox33.Text, "", ComboBox16.Text, ComboBox34.Text)
                            ReturnAudioStats = True
                        ElseIf ComboBox20.Text = "VBR" Then
                            HMEStreamProfileGenerate(AudiostreamFlags, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList) & aChannel(ComboBox33.Text, AudioStreamSourceList) &
                                                                 aBitRate(ComboBox17.Text, AudioStreamSourceList, Lossy, "VBR") & aSampleRate(ComboBox16.Text, AudioStreamSourceList) & channel_layout)
                            HMEAudioStreamConfigGenerate(AudiostreamConfig, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList), "", "VBR", ComboBox19.Text, ComboBox33.Text, ComboBox17.Text, ComboBox16.Text, ComboBox34.Text)
                            ReturnAudioStats = True
                        Else
                            HMEStreamProfileGenerate(AudiostreamFlags, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList) & aChannel(ComboBox33.Text, AudioStreamSourceList) &
                                                                    aBitRate(ComboBox17.Text, AudioStreamSourceList, "", "") & aSampleRate(ComboBox16.Text, AudioStreamSourceList) & channel_layout)
                            HMEAudioStreamConfigGenerate(AudiostreamConfig, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList), "", "VBR", ComboBox19.Text, ComboBox33.Text, ComboBox17.Text, ComboBox16.Text, ComboBox34.Text)
                            ReturnAudioStats = True
                        End If
                    ElseIf ComboBox15.Text = "FLAC" Then
                        HMEStreamProfileGenerate(AudiostreamFlags, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList) & aChannel(ComboBox33.Text, AudioStreamSourceList) &
                                            aBitRate(ComboBox17.Text, AudioStreamSourceList, "FLAC", "") & aSampleRate(ComboBox16.Text, AudioStreamSourceList) &
                                            aBitDepth(ComboBox15.Text, AudioStreamSourceList, ComboBox18.Text) & channel_layout)
                        HMEAudioStreamConfigGenerate(AudiostreamConfig, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList), aBitDepth(ComboBox15.Text, AudioStreamSourceList, ComboBox18.Text), "", "",
                                                                    ComboBox33.Text, ComboBox17.Text, ComboBox16.Text, ComboBox34.Text)
                        ReturnAudioStats = True
                    ElseIf ComboBox15.Text = "WAV" Then
                        HMEStreamProfileGenerate(AudiostreamFlags, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList) & aChannel(ComboBox33.Text, AudioStreamSourceList) &
                                                  aSampleRate(ComboBox16.Text, AudioStreamSourceList) & channel_layout)
                        HMEAudioStreamConfigGenerate(AudiostreamConfig, aCodec(ComboBox15.Text, ComboBox18.Text, AudioStreamSourceList), ComboBox15.Text, "", "", ComboBox33.Text, "", ComboBox16.Text, ComboBox34.Text)
                        ReturnAudioStats = True
                    ElseIf ComboBox15.Text = "Copy" Then
                        HMEStreamProfileGenerate(AudiostreamFlags, " -c:a:" & AudioStreamSourceList & " copy")
                        HMEAudioStreamConfigGenerate(AudiostreamConfig, "copy", "", "", "", "", "", "", "")
                        ReturnAudioStats = True
                    ElseIf ComboBox15.Text Is "" Then
                        NotifyIcon("Hana Media Encoder", "Audio codec can not empty !", 1000, False)
                        ReturnAudioStats = False
                    Else
                        NotifyIcon("Hana Media Encoder", "Out of range !", 1000, False)
                        ReturnAudioStats = False
                    End If
                End If
            End If
        Else
            ReturnAudioStats = False
        End If
    End Sub
    Private Sub AcodecReset()
        AudioBitDepthCheck()
        AudioFrequencyCheck()
        If ComboBox15.Text = "WAV" Then
            ComboBox16.Enabled = True
            If mediaenginestate = "NVENCC" Then
                ComboBox34.Enabled = False
                ComboBox20.Enabled = False
                ComboBox17.Enabled = False
            Else
                ComboBox17.Enabled = False
                ComboBox20.Enabled = False
                ComboBox34.Enabled = True
            End If
            ComboBox17.SelectedIndex = -1
            ComboBox18.Enabled = True
            ComboBox33.Enabled = True
            ComboBox19.Enabled = False
            If ComboBox33.Text IsNot "" Then
                ComboBox34.Enabled = True
            End If
        ElseIf ComboBox15.Text = "Copy" Then
            ComboBox16.Enabled = False
            ComboBox17.Enabled = False
            ComboBox18.Enabled = False
            ComboBox19.Enabled = False
            ComboBox20.Enabled = False
            ComboBox33.Enabled = False
        ElseIf ComboBox15.Text = "MP3" Or ComboBox15.Text = "AAC" Or
            ComboBox15.Text = "MP2" Or ComboBox15.Text = "OPUS" Then
            ComboBox16.Enabled = True
            If mediaenginestate = "NVENCC" Then
                ComboBox33.Enabled = False
                ComboBox34.Enabled = False
                ComboBox20.Enabled = False
                ComboBox17.Enabled = False
            Else
                ComboBox33.Enabled = True
                ComboBox34.Enabled = False
                ComboBox17.Enabled = False
                ComboBox20.Enabled = True
            End If
            ComboBox18.Enabled = False
            If ComboBox20.Text = "VBR" Then
                ComboBox17.Enabled = True
            ElseIf ComboBox20.Text = "CBR" Then
                ComboBox19.Enabled = True
            End If
            If ComboBox33.Text IsNot "" Then
                ComboBox34.Enabled = True
            End If
        ElseIf ComboBox15.Text = "FLAC" Then
            ComboBox16.Enabled = True
            If mediaenginestate = "NVENCC" Then
                ComboBox34.Enabled = False
                ComboBox20.Enabled = False
                ComboBox17.Enabled = False
            Else
                ComboBox17.Enabled = True
                ComboBox20.Enabled = False
                ComboBox34.Enabled = True
            End If
            ComboBox18.Enabled = True
            ComboBox33.Enabled = True
            ComboBox19.Enabled = False
            ComboBox20.SelectedIndex = -1
            If ComboBox33.Text IsNot "" Then
                ComboBox34.Enabled = True
            End If
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
        ElseIf ComboBox33.Text = "3" Then
            ComboBox34.Items.Add("3.0")
        ElseIf ComboBox33.Text = "4" Then
            ComboBox34.Items.Add("4.0")
            ComboBox34.Items.Add("quad")
        ElseIf ComboBox33.Text = "5" Then
            ComboBox34.Items.Add("5.0")
        Else
            ComboBox34.Items.Add("mono")
            ComboBox34.Items.Add("stereo")
        End If
    End Sub
    Private Sub AudioFrequencyCheck()
        If ComboBox15.Text = "AAC" Then
            ComboBox16.Items.Clear()
            ComboBox16.Items.Add("8000")
            ComboBox16.Items.Add("16000")
            ComboBox16.Items.Add("32000")
            ComboBox16.Items.Add("44100")
            ComboBox16.Items.Add("48000")
            ComboBox16.Items.Add("64000")
            ComboBox16.Items.Add("88200")
            ComboBox16.Items.Add("96000")
        ElseIf ComboBox15.Text = "MP3" Or ComboBox15.Text = "MP2" Then
            ComboBox16.Items.Clear()
            ComboBox16.Items.Add("8000")
            ComboBox16.Items.Add("16000")
            ComboBox16.Items.Add("32000")
            ComboBox16.Items.Add("44100")
            ComboBox16.Items.Add("48000")
        ElseIf ComboBox15.Text = "OPUS" Then
            ComboBox16.Items.Clear()
            ComboBox16.Items.Add("8000")
            ComboBox16.Items.Add("16000")
            ComboBox16.Items.Add("48000")
        ElseIf ComboBox15.Text = "WAV" Or ComboBox15.Text = "FLAC" Then
            ComboBox16.Items.Clear()
            ComboBox16.Items.Add("8000")
            ComboBox16.Items.Add("16000")
            ComboBox16.Items.Add("32000")
            ComboBox16.Items.Add("44100")
            ComboBox16.Items.Add("48000")
            ComboBox16.Items.Add("64000")
            ComboBox16.Items.Add("88200")
            ComboBox16.Items.Add("96000")
            ComboBox16.Items.Add("176400")
            ComboBox16.Items.Add("192000")
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
        If ComboBox20.Text = "CBR" Then
            ComboBox19.Enabled = True
            ComboBox17.Enabled = False
            ComboBox17.SelectedIndex = -1
            If ComboBox15.Text = "MP2" Then
                ComboBox19.Items.Clear()
                ComboBox19.Items.Add("384")
                ComboBox19.Items.Add("256")
                ComboBox19.Items.Add("192")
                ComboBox19.Items.Add("128")
            ElseIf ComboBox15.Text = "MP3" Then
                ComboBox19.Items.Clear()
                ComboBox19.Items.Add("320")
                ComboBox19.Items.Add("256")
                ComboBox19.Items.Add("192")
                ComboBox19.Items.Add("128")
            ElseIf ComboBox15.Text = "AAC" Then
                ComboBox19.Items.Clear()
                ComboBox19.Items.Add("512")
                ComboBox19.Items.Add("384")
                ComboBox19.Items.Add("256")
                ComboBox19.Items.Add("192")
                ComboBox19.Items.Add("128")
            ElseIf ComboBox15.Text = "OPUS" Then
                ComboBox19.Items.Clear()
                ComboBox19.Items.Add("510")
                ComboBox19.Items.Add("192")
                ComboBox19.Items.Add("160")
                ComboBox19.Items.Add("128")
                ComboBox19.Items.Add("96")
                ComboBox19.Items.Add("64")
            End If
        ElseIf ComboBox20.Text = "VBR" Then
            If ComboBox15.Text = "OPUS" Then
                ComboBox19.Enabled = True
                ComboBox17.Enabled = False
                ComboBox17.SelectedIndex = -1
            Else
                ComboBox19.SelectedIndex = -1
                ComboBox19.Enabled = False
                ComboBox17.Enabled = True
            End If
        End If
    End Sub
    Private Function AudioCodecLock() As Boolean
        Dim result As Boolean
        Dim audioflagspath As String = AudioStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)) - 1).ToString & ".txt"
        If mediaenginestate = "NVENCC" Then
            'Bypassing for now
            result = True
        ElseIf MetroSetSwitch6.Switched = True And DataGridView1.Rows.Count > 0 Then
            result = True
        Else
            FlagsCount = MetroSetComboBox3.Items.Count
            If MetroSetSwitch5.Switched = True Then
                If ComboBox28.SelectedText.ToString IsNot "Video + Audio (Specific source)" Or ComboBox28.SelectedText.ToString IsNot "Audio Only (Specific Source)" Then
                    If File.Exists(audioflagspath) Then
                        Label28.Text = "READY"
                        result = True
                    Else
                        result = False
                        MessageBoxAdv.Show("Please save configuration for audio stream #0:" & CInt(Strings.Mid(MetroSetComboBox3.Text.ToString, 11)).ToString, "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    FlagsResult = 0
                    FlagsValue = 0
                    For FlagsStart = 1 To FlagsCount
                        If File.Exists(audioflagspath) Then
                            FlagsResult += 1
                        Else
                            MissedFlags(FlagsStart) = FlagsStart
                        End If
                        FlagsValue += 1
                    Next
                    If FlagsResult = FlagsCount Then
                        result = True
                        Label28.Text = "READY"
                    Else
                        For FlagsStart = 1 To FlagsValue
                            If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                                MessageBoxAdv.Show("Please save configuration for audio stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        Next
                        result = False
                    End If
                End If
            Else
                FlagsResult = 0
                FlagsValue = 0
                For FlagsStart = 1 To FlagsCount
                    If File.Exists(audioflagspath) Then
                        FlagsResult += 1
                        FlagsValue += 1
                    Else
                        MissedFlags(FlagsStart) = FlagsStart
                        FlagsValue += 1
                    End If
                Next
                If FlagsResult = FlagsCount Then
                    result = True
                    Label28.Text = "READY"
                Else
                    For FlagsStart = 1 To FlagsValue
                        If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                            MessageBoxAdv.Show("Please save configuration for audio stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    Next
                    result = False
                End If
            End If
        End If
        Return result
    End Function
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
    Private Sub EnableTrim(sender As Object) Handles MetroSetSwitch5.SwitchedChanged
        If MetroSetSwitch5.Switched = True Then
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
            MetroSetSwitch4.Enabled = False
            MetroSetSwitch4.Switched = False
            MetroSetSwitch3.Enabled = False
            MetroSetSwitch3.Switched = False
            ComboBox26.Enabled = True
            ComboBox26.SelectedIndex = -1
            ComboBox28.SelectedIndex = -1
            NotifyIcon("Trim Profile", "Muxing and Chapter options are not available while trim profile is enable", 1000, True)
        Else
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
            If MetroSetSwitch3.Switched = False And MetroSetSwitch4.Switched = False Then
                MetroSetSwitch3.Enabled = True
                MetroSetSwitch4.Enabled = True
            End If
        End If
    End Sub
    Private Sub TrimQualityCombo(sender As Object, e As EventArgs) Handles ComboBox26.SelectedIndexChanged
        If ComboBox26.SelectedIndex = 0 Then
            MetroSetSwitch2.Switched = False
            MetroSetSwitch2.Enabled = False
            MetroSetSwitch1.Switched = False
            MetroSetSwitch1.Enabled = False
            TextBox10.Enabled = False
            TextBox11.Enabled = False
            NotifyIcon("Trim Profile", "Milliseconds time are not available while trim using original quality !", 1000, True)
        ElseIf ComboBox26.SelectedIndex = 1 Then
            MetroSetSwitch2.Enabled = True
            MetroSetSwitch1.Enabled = True
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
        ComboBox27.Items.Clear()
        getStreamSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & Textbox77.Text & Chr(34), "Trim", "")
        If ComboBox28.SelectedIndex = 0 Then
            If ComboBox26.SelectedIndex = 1 Then
                MetroSetSwitch2.Switched = False
                MetroSetSwitch2.Enabled = False
                MetroSetSwitch1.Switched = True
                MetroSetSwitch1.Enabled = True
            End If
            ComboBox27.Enabled = True
        ElseIf ComboBox28.SelectedIndex = 1 Then
            If ComboBox26.SelectedIndex = 1 Then
                MetroSetSwitch2.Switched = True
                MetroSetSwitch2.Enabled = True
                MetroSetSwitch1.Switched = True
                MetroSetSwitch1.Enabled = True
            End If
            ComboBox27.Enabled = True
        ElseIf ComboBox28.SelectedIndex = 2 Then
            If ComboBox26.SelectedIndex = 1 Then
                MetroSetSwitch2.Switched = True
                MetroSetSwitch2.Enabled = True
                MetroSetSwitch1.Switched = True
                MetroSetSwitch1.Enabled = True
            End If
            ComboBox27.Enabled = False
        ElseIf ComboBox28.SelectedIndex = 3 Then
            If ComboBox26.SelectedIndex = 1 Then
                MetroSetSwitch2.Switched = True
                MetroSetSwitch2.Enabled = True
                MetroSetSwitch1.Switched = False
                MetroSetSwitch1.Enabled = False
            End If
            ComboBox27.Enabled = True
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
    Private Function TrimLock() As Boolean
        Dim result As Boolean
        If ComboBox26.Text Is "" Then
            MessageBoxAdv.Show("Please choose trim quality first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf ComboBox28.Text Is "" Then
            MessageBoxAdv.Show("Please choose trim options first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            If ComboBox28.SelectedIndex >= 0 And ComboBox28.SelectedIndex < 3 Then
                If ComboBox26.SelectedIndex = 1 Then
                    If ComboBox28.SelectedIndex = 0 Then
                        If ComboBox27.Text Is "" Then
                            MessageBoxAdv.Show("Please choose trim source first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            TrimCondition = False
                        Else
                            FlagsResult = 0
                            FlagsValue = 0
                            FlagsCount = MetroSetComboBox1.Items.Count
                            For FlagsStart = 1 To FlagsCount
                                If File.Exists(VideoStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (FlagsStart - 1).ToString & ".txt") Then
                                    FlagsResult += 1
                                Else
                                    MissedFlags(FlagsStart) = FlagsStart
                                End If
                                FlagsValue += 1
                            Next
                            If FlagsResult = FlagsCount Then
                            Else
                                For FlagsStart = 1 To FlagsValue
                                    If MissedFlags(FlagsStart).ToString IsNot "" And CInt(MissedFlags(FlagsStart)) > 0 Then
                                        MessageBoxAdv.Show("Please save configuration for video stream #0:" & MissedFlags(FlagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    End If
                                Next
                            End If
                        End If
                    ElseIf ComboBox28.SelectedIndex = 1 Then
                        If ComboBox27.Text Is "" Then
                            MessageBoxAdv.Show("Please choose trim source first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            TrimCondition = False
                        Else
                            FlagsResult = 0
                            FlagsVideoValue = 0
                            FlagsAudioCount = 0
                            FlagsVideoCount = CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11))
                            FlagsAudioCount = CInt(Strings.Mid(ComboBox27.Text.ToString, 11))
                            For FlagsStart = 1 To FlagsVideoCount
                                If File.Exists(VideoStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (CInt(Strings.Mid(MetroSetComboBox1.Text.ToString, 11)) - 1).ToString & ".txt") Then
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
                                If File.Exists(AudioStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt") Then
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
                        FlagsVideoCount = MetroSetComboBox1.Items.Count
                        FlagsAudioCount = MetroSetComboBox3.Items.Count
                        FlagsResult = 0
                        FlagsVideoValue = 0
                        FlagsAudioCount = 0
                        For FlagsStart = 1 To FlagsVideoCount
                            If File.Exists(VideoStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (FlagsStart - 1).ToString & ".txt") Then
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
                            If File.Exists(AudioStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (FlagsStart - 1).ToString & ".txt") Then
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
            ElseIf ComboBox28.SelectedIndex = 3 Then
                If ComboBox26.SelectedIndex = 1 Then
                    If ComboBox28.SelectedIndex = 3 Then
                        FlagsResult = 0
                        FlagsValue = 0
                        FlagsCount = MetroSetComboBox3.Items.Count
                        For FlagsStart = 1 To FlagsCount
                            If File.Exists(AudioStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt") Then
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
            If TrimCondition = True Then
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
                                        TrimFlags = " -i " & Chr(34) & Textbox77.Text & Chr(34) & " -to " & TextBox14.Text & ":" & TextBox13.Text & ":" & TextBox12.Text & "." & TextBox11.Text
                                        If File.Exists(TrimStreamConfigPath) Then
                                            File.Delete(TrimStreamConfigPath)
                                        End If
                                        HMEStreamProfileGenerate(TrimStreamConfigPath & "\" & Path.GetFileName(Textbox77.Text) & "_trim.txt", TrimFlags)
                                    Else
                                        If TrimStartTime > 0 And TrimStartTime < 5 Then
                                            MessageBoxAdv.Show("Start time can not in range 1 to 4 seconds in original quality ! " & vbCrLf & vbCrLf &
                                                                   "Please use custom quality instead", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                            TextBox9.Text = "00"
                                        ElseIf TrimStartTime = 5 Then
                                            TrimFlags = " -i " & Chr(34) & Textbox77.Text & Chr(34) & " -ss " & TrimStartTime & " -to " & TrimEndTime
                                            If File.Exists(TrimStreamConfigPath & "\" & Path.GetFileName(Textbox77.Text) & "_trim.txt") Then
                                                File.Delete(TrimStreamConfigPath & "\" & Path.GetFileName(Textbox77.Text) & "_trim.txt")
                                            End If
                                            HMEStreamProfileGenerate(TrimStreamConfigPath & "\" & Path.GetFileName(Textbox77.Text) & "_trim.txt", TrimFlags)
                                        Else
                                            Dim newTrimDurTime As Integer = TrimEndTime - TrimStartTime
                                            TrimFlags = " -ss " & TrimStartTime & " -i " & Chr(34) & Textbox77.Text & Chr(34) & " -t " & newTrimDurTime - 1
                                            If File.Exists(TrimStreamConfigPath & "\" & Path.GetFileName(Textbox77.Text) & "_trim.txt") Then
                                                File.Delete(TrimStreamConfigPath & "\" & Path.GetFileName(Textbox77.Text) & "_trim.txt")
                                            End If
                                            HMEStreamProfileGenerate(TrimStreamConfigPath & "\" & Path.GetFileName(Textbox77.Text) & "_trim.txt", TrimFlags)
                                        End If
                                    End If
                                ElseIf ComboBox26.SelectedIndex = 1 Then
                                    TrimFlags = " -i " & Chr(34) & Textbox77.Text & Chr(34) & " -ss " & TextBox7.Text & ":" & TextBox8.Text & ":" & TextBox9.Text & "." & TextBox10.Text &
                                                            " -to " & TextBox14.Text & ":" & TextBox13.Text & ":" & TextBox12.Text & "." & TextBox11.Text
                                    If File.Exists(TrimStreamConfigPath & "\" & Path.GetFileName(Textbox77.Text) & "_trim.txt") Then
                                        File.Delete(TrimStreamConfigPath & "\" & Path.GetFileName(Textbox77.Text) & "_trim.txt")
                                    End If
                                    HMEStreamProfileGenerate(TrimStreamConfigPath & "\" & Path.GetFileName(Textbox77.Text) & "_trim.txt", TrimFlags)
                                End If
                            Else
                                MessageBoxAdv.Show("Please fill time to trim first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                ComboBox27.SelectedIndex = -1
                                ComboBox28.SelectedIndex = -1
                            End If
                        End If
                    Else
                        MessageBoxAdv.Show("End time can not more than actual video duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    MessageBoxAdv.Show("Start time can not more than actual video duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBoxAdv.Show("Please check video or audio configuration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ComboBox27.SelectedIndex = -1
                ComboBox28.SelectedIndex = -1
            End If
        End If
        If TrimFlags = "" Then
            result = False
        Else
            result = True
        End If
        Return result
    End Function
    Private Sub EnableMuxing_Check(sender As Object) Handles MetroSetSwitch4.SwitchedChanged
        If MetroSetSwitch4.Switched = True Then
            TextBox15.Enabled = True
            CheckBox11.Enabled = True
            TextBox16.Enabled = True
            Button9.Enabled = True
            Button10.Enabled = True
            CheckBox9.Enabled = True
            CheckBox10.Enabled = True
            ComboBox1.Enabled = True
            ComboBox1.SelectedIndex = 0
            MetroSetSwitch5.Switched = False
            MetroSetSwitch5.Enabled = False
            MetroSetSwitch3.Switched = False
            MetroSetSwitch3.Enabled = False
            NotifyIcon("Muxing Profile", "Trim and Chapter options are not available while muxing profile is enable", 1000, True)
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
            ComboBox25.Enabled = False
            ComboBox25.SelectedIndex = -1
            If MetroSetSwitch3.Switched = False And MetroSetSwitch5.Switched = False Then
                MetroSetSwitch3.Enabled = True
                MetroSetSwitch5.Enabled = True
            End If
        End If
    End Sub
    Private Sub SameMedia_Muxing_Check(sender As Object, e As EventArgs) Handles CheckBox11.CheckedChanged
        If CheckBox11.Checked = True Then
            If Label5.Text.ToString.Equals("Not Detected") = True Or Label5.Text.ToString.Equals("png") = True Then
                NotifyIcon("Unsupported media format", "Please load video file format only", 1000, False)
                CheckBox11.Checked = False
            Else
                If Textbox77.Text.ToString.Equals("") = False Then
                    TextBox15.Text = Textbox77.Text
                Else
                    NotifyIcon("Media file are not loaded", "Please open media file before", 1000, False)
                End If
                Button9.Enabled = False
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
            TextBox15.Text = OpenFileDialog.FileName
            If Textbox77.Text.ToString.Equals("") = True Then
                Textbox77.Text = TextBox15.Text
                OpenMedia_Load("muxing")
            End If
            ComboBox25.Items.Clear()
            getStreamSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & TextBox15.Text & Chr(34), "Muxing", "")
        End If
    End Sub
    Private Sub BrowseAudio_Muxing(sender As Object, e As EventArgs) Handles Button10.Click
        OpenFileDialog.DefaultExt = "*.*|.flac|.aiff|.alac|.mp3|.opus"
        OpenFileDialog.FilterIndex = 1
        OpenFileDialog.Filter = "All files (*.*)|*.*|FLAC|*.flac|WAV|*.wav|AIFF|*.aiff|ALAC|*.alac|MP3|*.mp3|MP2|*.mp2|OPUS|*.opus"
        OpenFileDialog.Title = "Choose Media File"
        OpenFileDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
        If OpenFileDialog.ShowDialog() = DialogResult.OK Then
            TextBox16.Text = OpenFileDialog.FileName
        End If
    End Sub
    Private Sub ReplaceExistingAudio_Check(sender As Object, e As EventArgs) Handles CheckBox9.CheckedChanged
        If CheckBox9.Checked = True Then
            CheckBox10.Checked = False
            CheckBox10.Enabled = False
            ComboBox25.Enabled = True
            getStreamSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & TextBox15.Text & Chr(34), "Muxing", "")
            If ComboBox25.Items.Count = 0 Then
                MessageBoxAdv.Show("Media file are not contains any audio file" & vbCrLf & vbCrLf & "Please select add as new audio instead", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ComboBox25.Enabled = False
                CheckBox9.Checked = False
            End If
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
            getStreamSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & TextBox15.Text & Chr(34), "Muxing + Custom", "")
        Else
            ComboBox25.Enabled = False
            ComboBox25.SelectedIndex = -1
            CheckBox9.Checked = False
            CheckBox9.Enabled = True
        End If
    End Sub
    Private Function MuxLock() As Boolean
        Dim result As Boolean
        If TextBox15.Text IsNot "" And File.Exists(TextBox15.Text) Then
            If TextBox16.Text IsNot "" And File.Exists(TextBox16.Text) Then
                If ComboBox1.Text Is "" Then
                    ComboBox1.SelectedIndex = 0
                End If
                If CheckBox11.Checked = False Then
                    VideoFilePath = TextBox15.Text.ToString
                Else
                    VideoFilePath = Textbox77.Text.ToString
                End If
                If ComboBox1.SelectedIndex = 1 Then
                    FlagsResult = 0
                    FlagsVideoValue = 0
                    FlagsAudioCount = 0
                    FlagsVideoCount = MetroSetComboBox1.Items.Count
                    FlagsAudioCount = MetroSetComboBox3.Items.Count
                    For FlagsStart = 1 To FlagsVideoCount
                        If File.Exists(VideoStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (FlagsStart - 1).ToString & ".txt") Then
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
                        If File.Exists(AudioStreamFlagsPath & Path.GetFileName(Textbox77.Text) & "_flags_" & (FlagsStart - 1).ToString & ".txt") Then
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
                            If File.Exists(MuxStreamConfigPath & "\HME_Stream_Replace.txt") Then
                                GC.Collect()
                                GC.WaitForPendingFinalizers()
                                File.Delete(MuxStreamConfigPath & "\HME_Stream_Replace.txt")
                                File.Create(MuxStreamConfigPath & "\HME_Stream_Replace.txt").Dispose()
                            End If
                            For FlagsStart = 1 To ComboBox25.Items.Count
                                File.AppendAllText(MuxStreamConfigPath & "\HME_Stream_Replace.txt", " -map 0:" & numbers(FlagsStart))
                            Next
                            If FindConfig(MuxStreamConfigPath & "\HME_Stream_Replace.txt", "-map 0:" & AudioStreamArray) IsNot "" Then
                                Dim ReplaceStream As String = File.ReadAllText(MuxStreamConfigPath & "\HME_Stream_Replace.txt")
                                ReplaceStream = ReplaceStream.Replace(" -map 0:" & AudioStreamArray, " -map 1:0 ")
                                File.WriteAllText(MuxStreamConfigPath & "\HME_Stream_Replace.txt", ReplaceStream)
                            End If
                            If ComboBox1.Text = "Original Quality" Then
                                MuxFlags = " -i " & Chr(34) & VideoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0:0 " & File.ReadAllText(My.Application.Info.DirectoryPath & "\HME_Stream_Replace.txt") & " -c copy "
                                If File.Exists(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt") Then
                                    File.Delete(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt")
                                End If
                                HMEStreamProfileGenerate(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt", MuxFlags)
                            ElseIf ComboBox1.Text = "Custom Quality" Then
                                MuxFlags = " -i " & Chr(34) & VideoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0:0 " & File.ReadAllText(My.Application.Info.DirectoryPath & "\HME_Stream_Replace.txt") & ""
                                If File.Exists(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt") Then
                                    File.Delete(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt")
                                End If
                                HMEStreamProfileGenerate(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt", MuxFlags)
                            End If
                        End If
                    ElseIf CheckBox9.Checked = False And CheckBox10.Checked = True Then
                        If ComboBox1.Text = "Original Quality" Then
                            MuxFlags = " -i " & Chr(34) & VideoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0 -map 1:a -c copy "
                            If File.Exists(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt") Then
                                File.Delete(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt")
                            End If
                            HMEStreamProfileGenerate(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt", MuxFlags)
                        ElseIf ComboBox1.Text = "Custom Quality" Then
                            MuxFlags = " -i " & Chr(34) & VideoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0 -map 1:a "
                            If File.Exists(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt") Then
                                File.Delete(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt")
                            End If
                            HMEStreamProfileGenerate(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt", MuxFlags)
                        End If
                    ElseIf CheckBox9.Checked = False And CheckBox10.Checked = False Then
                        If ComboBox1.Text = "Original Quality" Then
                            MuxFlags = " -i " & Chr(34) & VideoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0 -map 1:a -c copy "
                            If File.Exists(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt") Then
                                File.Delete(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt")
                            End If
                            HMEStreamProfileGenerate(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt", MuxFlags)
                        ElseIf ComboBox1.Text = "Custom Quality" Then
                            MuxFlags = " -i " & Chr(34) & VideoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0 -map 1:a "
                            If File.Exists(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt") Then
                                File.Delete(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt")
                            End If
                            HMEStreamProfileGenerate(MuxStreamConfigPath & "\" & Path.GetFileName(VideoFilePath) & "_mux.txt", MuxFlags)
                        End If
                    End If
                Else
                    MessageBoxAdv.Show("Please check video or audio configuration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBoxAdv.Show("Audio file for muxing are not selected or are not exists !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBoxAdv.Show("Video file for muxing are not selected or are not exists !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        If MuxFlags = "" Then
            result = False
        Else
            result = True
        End If

        Return result
    End Function
    Private Sub EnableChapter(sender As Object) Handles MetroSetSwitch3.SwitchedChanged
        If MetroSetSwitch3.Switched = True Then
            If Label5.Text.Equals("Not Detected") = True And TextBox15.Text Is "" Then
                MetroSetSwitch3.Switched = False
                NotifyIcon("Video Codec", "Current media file does not contain any video stream", 1000, True)
            Else
                TextBox5.Enabled = True
                TextBox17.Enabled = True
                TextBox18.Enabled = True
                TextBox19.Enabled = True
                Button11.Enabled = True
                Button12.Enabled = True
                Button13.Enabled = True
                Button14.Enabled = True
                MetroSetSwitch4.Switched = False
                MetroSetSwitch4.Enabled = False
                MetroSetSwitch5.Switched = False
                MetroSetSwitch5.Enabled = False
                ChapterReset()
                GetChapter()
                NotifyIcon("Chapter Profile", "Trim and Muxing options are not available while chapter is enable", 1000, True)
            End If
        Else
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
            ChapterReset()
            ListView1.Items.Clear()
            If MetroSetSwitch4.Switched = False And MetroSetSwitch5.Switched = False Then
                MetroSetSwitch4.Enabled = True
                MetroSetSwitch5.Enabled = True
            End If
        End If
    End Sub
    Private Sub GetChapter()
        Newffargs = "ffmpeg -i " & Chr(34) & Textbox77.Text & Chr(34) & " -f ffmetadata " & Chr(34) & ChapterStreamConfigPath & "\FFMETADATAFILE" & Chr(34)
        HMEGenerate(HMEEngine & "HME_Chapters.bat", FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Newffargs, "")
        RunProc(HMEEngine & "HME_Chapters.bat")
        If File.Exists(ChapterStreamConfigPath & "\FFMETADATAFILE") = True Then
            Dim chapterStatus As Boolean
            Dim curLoop As Integer = 1
            Dim cnvTime As String
            Dim count As Integer = 0
            Dim selectedTime As New List(Of String)()
            Dim selectedTitle As New List(Of String)()
            Dim readMetadataLines() As String = File.ReadAllLines(ChapterStreamConfigPath & "\FFMETADATAFILE")
            If File.ReadAllText(ChapterStreamConfigPath & "\FFMETADATAFILE").Contains("START=") = True Then
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
                Loop While count < File.ReadAllLines(ChapterStreamConfigPath & "\FFMETADATAFILE").Length
                chapterStatus = True
            Else
                chapterStatus = False
            End If
            count = 0
            If File.ReadAllText(ChapterStreamConfigPath & "\FFMETADATAFILE").Contains("title=") = True Then
                Do
                    If readMetadataLines(count).Contains("title=") Then
                        Dim curLines As String = Strings.Mid(readMetadataLines(count), 7)
                        selectedTitle.Add(curLines)
                        curLoop += 1
                    End If
                    count += 1
                Loop While count < File.ReadAllLines(ChapterStreamConfigPath & "\FFMETADATAFILE").Length
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
                    newChapter.SubItems.Add(listOfTitle(n))
                    n += 1
                Next
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
            File.Delete(ChapterStreamConfigPath & "\FFMETADATAFILE")
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
                    End If
                End If
            Else
                MessageBoxAdv.Show("New time chapter duration can not more or same than video duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            ChapterReset()
        Else
            MessageBoxAdv.Show("Failed to add chapter !" & vbCrLf & vbCrLf & "Make sure to fill time and chapter title completely", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                                MessageBoxAdv.Show("New time chapter duration can not more or same than next time chapter duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                                MessageBoxAdv.Show("Chapter updated successfully !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                ChapterReset()
                            End If
                        Else
                            ChapterReplace("update", newTime)
                            MessageBoxAdv.Show("Chapter updated successfully !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
                            MessageBoxAdv.Show("New time chapter duration can not less than previous time chapter duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                            MessageBoxAdv.Show("Chapter updated successfully !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
                            MessageBoxAdv.Show("New time chapter duration can not more than next time chapter duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                                MessageBoxAdv.Show("New time chapter duration can not less than previous time chapter duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                                MessageBoxAdv.Show("Chapter updated successfully !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                ChapterReset()
                            End If
                        End If
                    End If
                Else
                    MessageBoxAdv.Show("Please select chapter that want to update !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBoxAdv.Show("New time chapter duration can not more than video duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBoxAdv.Show("failed to update chapter  !" & vbCrLf & vbCrLf & "Make sure to fill time and chapter title completely", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub RemoveChapter(sender As Object, e As EventArgs) Handles Button12.Click
        If ListView1.SelectedItems.Count > 0 Then
            ChapterReplace("remove", "")
        Else
            MessageBoxAdv.Show("Please select chapter before remove chapter !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            Dim chapterResult As DialogResult = MessageBoxAdv.Show(Me, "Replace current title " & ListView1.SelectedItems(0).SubItems(1).Text & " ? ", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
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
                MessageBoxAdv.Show("Chapter " & Chr(34) & ListView1.SelectedItems(0).SubItems(1).Text & Chr(34) & " removed !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
        If TimeChapter < TimeDur Then
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
    Private Function ChapterLock() As Boolean
        If ListView1.Items.Count > 0 Then
            If File.Exists(ChapterStreamConfigPath & "\FFMETADATAFILE") Then
                GC.Collect()
                GC.WaitForPendingFinalizers()
                File.Delete(ChapterStreamConfigPath & "\FFMETADATAFILE")
            End If
            File.Create(ChapterStreamConfigPath & "\FFMETADATAFILE").Dispose()
            Dim writer As New StreamWriter(ChapterStreamConfigPath & "\FFMETADATAFILE", True)
            writer.WriteLine(";FFMETADATAFILE1" & vbCrLf)
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
        Else
            If File.Exists(ChapterStreamConfigPath & "\FMETADATAFILE") Then
                GC.Collect()
                GC.WaitForPendingFinalizers()
                File.Delete(ChapterStreamConfigPath & "\FFMETADATAFILE")
            End If
            File.Create(ChapterStreamConfigPath & "\FFMETADATAFILE").Dispose()
            Dim writer As New StreamWriter(ChapterStreamConfigPath & "\FFMETADATAFILE", True)
            writer.WriteLine(";FFMETADATAFILE1" & vbCrLf & vbCrLf)
            writer.Close()
        End If
        ChapterFlags = " -i " & Chr(34) & ChapterStreamConfigPath & "\FFMETADATAFILE" & Chr(34) & " -map_chapters 1 "
        Return True
    End Function
    Private Sub ResetInit(state As String)
        If Textbox77.Text.Equals("") = True Then
            MetroSetSwitch1.Switched = False
            MetroSetSwitch1.Enabled = False
            MetroSetSwitch2.Switched = False
            MetroSetSwitch2.Enabled = False
            MetroSetSwitch3.Switched = False
            MetroSetSwitch3.Enabled = False
            MetroSetSwitch4.Switched = False
            MetroSetSwitch4.Enabled = False
            MetroSetSwitch5.Switched = False
            MetroSetSwitch5.Enabled = False
            MetroSetSwitch6.Switched = False
            MetroSetSwitch6.Enabled = True
            CheckBox11.Enabled = False
            CheckBox11.Checked = False
        Else
            MetroSetSwitch1.Switched = False
            MetroSetSwitch1.Enabled = True
            MetroSetSwitch2.Switched = False
            MetroSetSwitch2.Enabled = True
            MetroSetSwitch3.Switched = False
            MetroSetSwitch3.Enabled = True
            MetroSetSwitch4.Switched = False
            MetroSetSwitch4.Enabled = True
            MetroSetSwitch5.Switched = False
            MetroSetSwitch5.Enabled = True
            MetroSetSwitch6.Switched = False
            MetroSetSwitch6.Enabled = True
            CheckBox11.Enabled = False
            CheckBox11.Checked = False
        End If
        Button15.Enabled = False
        Button16.Enabled = False
        Button17.Enabled = False
        Button18.Enabled = False
    End Sub
    Private Sub MediaQueue_Checkbox(sender As Object) Handles MetroSetSwitch6.SwitchedChanged
        If MetroSetSwitch6.Switched = True Then
            ComboBox43.Enabled = True
            MetroSetSwitch5.Enabled = False
            MetroSetSwitch4.Enabled = False
            MetroSetSwitch3.Enabled = False
            DataGridView1.Enabled = True
            DataGridView1.AllowDrop = True
            ComboBox43.SelectedIndex = -1
            ComboBox43.Items.Clear()
            ComboBox43.Items.Add("Quick Profile")
            ComboBox43.Items.Add("Custom Profile")
        Else
            DataGridView1.Enabled = False
            DataGridView1.AllowDrop = False
            ComboBox43.Enabled = False
            ComboBox43.SelectedIndex = -1
            ComboBox43.Items.Clear()
            MetroSetSwitch5.Enabled = True
            MetroSetSwitch4.Enabled = True
            MetroSetSwitch3.Enabled = True
        End If
    End Sub
    Private Sub MediaQueueProfile_Btn(sender As Object, e As EventArgs) Handles ComboBox43.SelectedIndexChanged
        Button32.Enabled = True
        If ComboBox43.SelectedIndex > 0 Or ComboBox43.Text.ToString = "Custom Profile" Then
            Button32.Text = "Configure Custom Profile"
        Else
            Button32.Text = "Configure Quick Profile"
        End If
    End Sub
    Private Sub DataGridView1_DragDrop(sender As Object, e As DragEventArgs) Handles DataGridView1.DragDrop
        Dim duplicateData As ArrayList = New ArrayList()
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        Dim num As Integer = 0
        Dim order As Integer
        unloadMedia()
        For Each mediapath In files
            If Strings.Right(mediapath, 4).ToLower = "flac" Or Strings.Right(mediapath, 4).ToLower = ".wav" Or Strings.Right(mediapath, 4).ToLower = ".mp3" Or
                Strings.Right(mediapath, 4).ToLower = ".mp2" Or Strings.Right(mediapath, 4).ToLower = ".aac" Or Strings.Right(mediapath, 4).ToLower = ".dts" Or
                Strings.Right(mediapath, 4).ToLower = ".dsd" Or Strings.Right(mediapath, 4).ToLower = ".pcm" Or Strings.Right(mediapath, 4).ToLower = "opus" Or
                Strings.Right(mediapath, 4).ToLower = ".ogg" Or Strings.Right(mediapath, 4).ToLower = ".ape" Or Strings.Right(mediapath, 4).ToLower = "alac" Or
                Strings.Right(mediapath, 4).ToLower = "aiff" Or Strings.Right(mediapath, 4).ToLower = ".aif" Or Strings.Right(mediapath, 4).ToLower = ".m4a" Or
                Strings.Right(mediapath, 4).ToLower = ".tak" Or Strings.Right(mediapath, 4).ToLower = ".tta" Or Strings.Right(mediapath, 4).ToLower = ".wma" Or
                Strings.Right(mediapath, 3).ToLower = ".wv" Or Strings.Right(mediapath, 4).ToLower = ".dff" Then
                AudioQueue = True
                getAudioSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & mediapath & Chr(34), "0")
                MediaQueueOrigDir.Add(Path.GetDirectoryName(mediapath))
                If DataGridView1.Rows.Count <= 0 Then
                    order = 0
                Else
                    If DataGridView1.Rows(DataGridView1.Rows.Count - 1).Cells(1).Value.ToString IsNot "" Then
                        order = CInt(DataGridView1.Rows(DataGridView1.Rows.Count - 1).Cells(1).Value.ToString)
                    Else
                        order = 0
                    End If
                End If
                If DataGridView1.Rows.Count > 0 Then
                    mediaQueueTable.Rows.Add(order + 1, Path.GetFileName(mediapath), "", "Audio: " + AudioQueueCodecInf, "Audio File",
                                         getDurationSummaryAlt(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & mediapath & Chr(34)),
                                         GetFileSize(mediapath), "", "")
                    For i As Integer = 0 To DataGridView1.Rows.Count - 1
                        If DataGridView1.Rows(i).Cells(2).Value.ToString = Path.GetFileName(mediapath) Then
                            duplicateData.Add(i)
                        End If
                    Next
                    If duplicateData.Count > 0 Then
                        For Each i In duplicateData
                            If i < DataGridView1.Rows.Count - 1 Then
                                DataGridView1.Rows.RemoveAt(i)
                            End If
                        Next
                        duplicateData.Clear()
                    End If
                Else
                    mediaQueueTable.Rows.Add(order + 1, Path.GetFileName(mediapath), "", "Audio: " + AudioQueueCodecInf, "Audio File",
                                         getDurationSummaryAlt(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & mediapath & Chr(34)),
                                         GetFileSize(mediapath), "", "")
                End If
                MetroSetCheckBox2.Enabled = True
                MetroSetSwitch2.Enabled = True
                DataGridView1.DataSource = mediaQueueTable
                num += 1
            ElseIf Strings.Right(mediapath, 4).ToLower = ".mkv" Or Strings.Right(mediapath, 4).ToLower = ".mp4" Or
                   Strings.Right(mediapath, 4).ToLower = ".avi" Or Strings.Right(mediapath, 4).ToLower = ".flv" Or
                   Strings.Right(mediapath, 3).ToLower = ".ts" Or Strings.Right(mediapath, 4).ToLower = "m2ts" Or
                   Strings.Right(mediapath, 4).ToLower = ".mov" Or Strings.Right(mediapath, 4).ToLower = ".mp4" Or
                   Strings.Right(mediapath, 4).ToLower = ".vob" Or Strings.Right(mediapath, 4).ToLower = ".3gp" Or
                   Strings.Right(mediapath, 4).ToLower = ".mxf" Or Strings.Right(mediapath, 4).ToLower = "webm" Or
                   Strings.Right(mediapath, 4).ToLower = ".m2v" Or Strings.Right(mediapath, 4).ToLower = ".mts" Then
                VideoQueue = True
                AudioQueue = True
                getVideoSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & mediapath & Chr(34), "0")
                getAudioSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & mediapath & Chr(34), "0")
                MediaQueueOrigDir.Add(Path.GetDirectoryName(mediapath))
                If DataGridView1.Rows.Count <= 0 Then
                    order = 0
                Else
                    If DataGridView1.Rows(DataGridView1.Rows.Count - 1).Cells(1).Value.ToString IsNot "" Then
                        order = CInt(DataGridView1.Rows(DataGridView1.Rows.Count - 1).Cells(1).Value.ToString)
                    Else
                        order = 0
                    End If
                End If
                If DataGridView1.Rows.Count > 0 Then
                    mediaQueueTable.Rows.Add(order + 1, Path.GetFileName(mediapath), "", "Video: " + VideoQueueCodecInf + ", Audio: " + AudioQueueCodecInf, "Video File",
                                             getDurationSummaryAlt(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & mediapath & Chr(34)),
                                             GetFileSize(mediapath), "", "")
                    For i As Integer = 0 To DataGridView1.Rows.Count - 1
                        If DataGridView1.Rows(i).Cells(2).Value.ToString = Path.GetFileName(mediapath) Then
                            duplicateData.Add(i)
                        End If
                    Next
                    If duplicateData.Count > 0 Then
                        For Each i In duplicateData
                            If i < DataGridView1.Rows.Count - 1 Then
                                DataGridView1.Rows.RemoveAt(i)
                            End If
                        Next
                        duplicateData.Clear()
                    End If
                Else
                    mediaQueueTable.Rows.Add(order + 1, Path.GetFileName(mediapath), "", "Video: " + VideoQueueCodecInf + ", Audio: " + AudioQueueCodecInf, "Video File",
                                             getDurationSummaryAlt(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & mediapath & Chr(34)),
                                             GetFileSize(mediapath), "", "")
                End If
                DataGridView1.DataSource = mediaQueueTable
                num += 1
                MetroSetCheckBox1.Enabled = True
                MetroSetCheckBox2.Enabled = True
                MetroSetSwitch1.Enabled = True
                MetroSetSwitch2.Enabled = True
                MetroSetComboBox2.Enabled = True
            Else
                NotifyIcon("Hana Media Encoder", "Media file format are not supported !", 1000, False)
            End If
        Next
        VideoQueue = False
        AudioQueue = False
    End Sub
    Private Sub DataGridView1_DragEnter(sender As Object, e As DragEventArgs) Handles DataGridView1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub
    Private Sub DataGridView1_CellClick(sender As Object, b As DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        Dim index = b.RowIndex
        Dim selectedrow As DataGridViewRow
        selectedrow = DataGridView1.Rows(index)
        Textbox77.Text = MediaQueueOrigDir(index) + "\" + selectedrow.Cells(2).Value.ToString
        OpenMedia_Load("nothing")
    End Sub
    Private Sub AudioPrf_Btn(sender As Object, e As EventArgs) Handles Button32.Click
        Dim audioProfile = New AudioProfile
        Dim audioCount As Integer
        Dim videoCount As Integer
        If Button32.Text = "Configure Custom Profile" Then
            If DataGridView1.Rows.Count > 0 Then
                For i As Integer = 0 To DataGridView1.Rows.Count - 1
                    If DataGridView1.Rows(i).Cells(0).Value = True Then
                        If DataGridView1.Rows(i).Cells(5).Value.ToString = "Video File" Then
                            videoCount += 1
                        ElseIf DataGridView1.Rows(i).Cells(5).Value.ToString = "Audio File" Then
                            audioCount += 1
                        End If
                    End If
                Next
                If videoCount > audioCount Then
                    MetroSetTabControl1.SelectedTab = TabPage2
                ElseIf audioCount > videoCount Then
                    MetroSetTabControl1.SelectedTab = TabPage3
                End If
            Else
                NotifyIcon("Hana Media Encoder", "Please insert media into queue first!", 1000, False)
            End If
        Else
            audioProfile.Show()
        End If
    End Sub
    Private Sub DataGridView1_CellRightClick(sender As Object, b As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDown
        If b.Button = MouseButtons.Right Then
            If b.ColumnIndex <> -1 And b.RowIndex <> -1 Then
                Dim row = DataGridView1.Rows(b.RowIndex)
                Me.DataGridView1.ClearSelection()
                DataGridView1.CurrentCell = row.Cells(If(b.ColumnIndex = -1, 1, b.ColumnIndex))
                row.Selected = True
                DataGridView1.Focus()
                ContextMenuStrip2.Show(row.DataGridView, PointToClient(Cursor.Position))
            End If
        End If
    End Sub
    Private Sub DataGridView1_AddData(sender As Object, e As EventArgs) Handles AddToolStripMenuItem.Click
        OpenFileDialog.Multiselect = True
        OpenFileDialog.DefaultExt = "*.*|.avi|.mp4|.mkv|.mp2|.m2ts|.mts|.ts|.wav|.flac|.aiff|.alac|.mp3|.opus"
        OpenFileDialog.FilterIndex = 1
        OpenFileDialog.Filter = "All files (*.*)|*.*|AVI|*.avi|MPEG-4|*.mp4|Matroska Video|*.mkv|MPEG-2|*.m2v;*.m2ts;*.mts;*.ts|FLAC|*.flac|WAV|*.wav|AIFF|*.aiff|ALAC|*.alac|AAC|*.m4a|MP3|*.mp3|MP2|*.mp2|OPUS|*.opus"
        OpenFileDialog.Title = "Choose Media File"
        OpenFileDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
        If OpenFileDialog.ShowDialog() = DialogResult.OK Then
            unloadMedia()
            If Strings.Right(OpenFileDialog.FileName, 4).ToLower = "flac" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".wav" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mp3" Or
                Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mp2" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".aac" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".dts" Or
                Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".dsd" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".pcm" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = "opus" Or
                Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".ogg" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".ape" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = "alac" Or
                Strings.Right(OpenFileDialog.FileName, 4).ToLower = "aiff" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".aif" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".m4a" Or
                Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".tak" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".tta" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".wma" Or
                Strings.Right(OpenFileDialog.FileName, 3).ToLower = ".wv" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".dff" Then
                Dim duplicateData As ArrayList = New ArrayList()
                AudioQueue = True
                getAudioSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & OpenFileDialog.FileName & Chr(34), "0")
                MediaQueueOrigDir.Add(Path.GetDirectoryName(OpenFileDialog.FileName))
                If DataGridView1.Rows.Count > 0 Then
                    mediaQueueTable.Rows.Add(DataGridView1.Rows.Count + 1, Path.GetFileName(OpenFileDialog.FileName), "", "Audio: " + AudioQueueCodecInf, "Audio File",
                                             getDurationSummaryAlt(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & OpenFileDialog.FileName & Chr(34)),
                                             GetFileSize(OpenFileDialog.FileName), "", "")
                    For i As Integer = 0 To DataGridView1.Rows.Count - 1
                        If DataGridView1.Rows(i).Cells(2).Value.ToString = Path.GetFileName(OpenFileDialog.FileName) Then
                            duplicateData.Add(i)
                        End If
                    Next
                    If duplicateData.Count > 0 Then
                        For Each i In duplicateData
                            If i < DataGridView1.Rows.Count - 1 Then
                                DataGridView1.Rows.RemoveAt(i)
                            End If
                        Next
                        duplicateData.Clear()
                    End If
                Else
                    mediaQueueTable.Rows.Add(DataGridView1.Rows.Count + 1, Path.GetFileName(OpenFileDialog.FileName), "", "Audio: " + AudioQueueCodecInf, "Audio File",
                                             getDurationSummaryAlt(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & OpenFileDialog.FileName & Chr(34)),
                                             GetFileSize(OpenFileDialog.FileName), "", "")
                End If
                DataGridView1.DataSource = mediaQueueTable
                AudioQueue = False
                MetroSetCheckBox2.Enabled = True
                MetroSetSwitch2.Enabled = True
            ElseIf Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mkv" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mp4" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".avi" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".flv" Or Strings.Right(OpenFileDialog.FileName, 3).ToLower = ".ts" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = "m2ts" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mov" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mp4" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".vob" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".3gp" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mxf" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = "webm" Or
                            Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".m2v" Or Strings.Right(OpenFileDialog.FileName, 4).ToLower = ".mts" Then
                Dim duplicateData As ArrayList = New ArrayList()
                AudioQueue = True
                VideoQueue = True
                getVideoSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & OpenFileDialog.FileName & Chr(34), "0")
                getAudioSummary(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & OpenFileDialog.FileName & Chr(34), "0")
                MediaQueueOrigDir.Add(Path.GetDirectoryName(OpenFileDialog.FileName))
                If DataGridView1.Rows.Count > 0 Then
                    mediaQueueTable.Rows.Add(DataGridView1.Rows.Count + 1, Path.GetFileName(OpenFileDialog.FileName), "", "Video: " + VideoQueueCodecInf + ", Audio: " + AudioQueueCodecInf, "Video File",
                                         getDurationSummaryAlt(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & OpenFileDialog.FileName & Chr(34)),
                                         GetFileSize(OpenFileDialog.FileName), "", "")
                    For i As Integer = 0 To DataGridView1.Rows.Count - 1
                        If DataGridView1.Rows(i).Cells(2).Value.ToString = Path.GetFileName(OpenFileDialog.FileName) Then
                            duplicateData.Add(i)
                        End If
                    Next
                    If duplicateData.Count > 0 Then
                        For Each i In duplicateData
                            If i < DataGridView1.Rows.Count - 1 Then
                                DataGridView1.Rows.RemoveAt(i)
                            End If
                        Next
                        duplicateData.Clear()
                    End If
                Else
                    mediaQueueTable.Rows.Add(DataGridView1.Rows.Count + 1, Path.GetFileName(OpenFileDialog.FileName), "", "Video: " + VideoQueueCodecInf + ", Audio: " + AudioQueueCodecInf, "Video File",
                                         getDurationSummaryAlt(FfmpegLetter, Chr(34) & FfmpegConf & Chr(34), Chr(34) & OpenFileDialog.FileName & Chr(34)),
                                         GetFileSize(OpenFileDialog.FileName), "", "")
                End If
                DataGridView1.DataSource = mediaQueueTable
                AudioQueue = False
                VideoQueue = False
                MetroSetCheckBox1.Enabled = True
                MetroSetCheckBox2.Enabled = True
                MetroSetSwitch1.Enabled = True
                MetroSetSwitch2.Enabled = True
                MetroSetComboBox2.Enabled = True
            Else
                NotifyIcon("Hana Media Encoder", "Media file format are not supported !", 1000, False)
            End If
        End If
    End Sub
    Private Sub DataGridView1_RemoveSelectedData(sender As Object, e As EventArgs) Handles RemoveSelectedToolStripMenuItem1.Click
        If DataGridView1.Rows.Count > 0 Then
            If DataGridView1.SelectedRows.Count > 0 Then
                For Each row As DataGridViewRow In DataGridView1.SelectedRows
                    DataGridView1.Rows.Remove(row)
                Next
            Else
                For Each row As DataGridViewRow In DataGridView1.Rows
                    If row.Cells(0).Value = True Then
                        DataGridView1.Rows.Remove(row)
                    End If
                Next
            End If
        Else
            NotifyIcon("Hana Media Encoder", "Please select or checklist which file to remove!", 1000, False)
        End If
    End Sub
    Private Sub DataGridView1_RemoveAllData(sender As Object, e As EventArgs) Handles RemoveAllToolStripMenuItem1.Click
        If DataGridView1.Rows.Count > 0 Then
            mediaQueueTable.Clear()
        End If
    End Sub
    Private Sub LoadQueueTable()
        Dim dgvCheck As New DataGridViewCheckBoxColumn()
        mediaQueueTable.Columns.Add("No", Type.GetType("System.Int32"))
        mediaQueueTable.Columns.Add("File Name", Type.GetType("System.String"))
        mediaQueueTable.Columns.Add("Encode Status", Type.GetType("System.String"))
        mediaQueueTable.Columns.Add("Codec Info", Type.GetType("System.String"))
        mediaQueueTable.Columns.Add("File Type", Type.GetType("System.String"))
        mediaQueueTable.Columns.Add("Duration", Type.GetType("System.String"))
        mediaQueueTable.Columns.Add("Size", Type.GetType("System.String"))
        mediaQueueTable.Columns.Add("New Codec Info", Type.GetType("System.String"))
        mediaQueueTable.Columns.Add("New File Size", Type.GetType("System.String"))
        dgvCheck.HeaderText = "  "
        dgvCheck.ReadOnly = False
        DataGridView1.Columns.Add(dgvCheck)
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
        DataGridView1.EnableHeadersVisualStyles = False
        DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#161B21")
        DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F4A950")
        DataGridView1.DataSource = mediaQueueTable
        mediaQueueTable.Clear()
    End Sub
    Public Sub unloadMedia()
        Textbox77.Text = ""
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
        ComboBox24.Items.Clear()
        ComboBox24.Enabled = False
        Label44.Text = "Not Detected"
        Label39.Text = "Not Detected"
        Label42.Text = "Not Detected"
        Label35.Text = "Not Detected"
        Label26.Text = "Not Detected"
        Label32.Text = "Not Detected"
        Label30.Text = "Not Detected"
        Label61.Text = "Not Detected"
        Label107.Text = "Not Detected"
        ComboBox23.Items.Clear()
        ComboBox23.Enabled = False
        NotifyIcon("Hana Media Encoder", "Unload Media while media queue is active!", 1000, True)
    End Sub
    Private Sub LazyTheme()
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
        MetroSetComboBox1.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox1.SelectedItemBackColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox1.SelectedItemForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetComboBox1.DisabledBackColor = ColorTranslator.FromHtml("#161B21")
        MetroSetComboBox1.DisabledBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox1.DisabledForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox2.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox2.SelectedItemBackColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox2.SelectedItemForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetComboBox2.DisabledBackColor = ColorTranslator.FromHtml("#161B21")
        MetroSetComboBox2.DisabledBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox2.DisabledForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox3.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox3.SelectedItemBackColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox3.SelectedItemForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetComboBox3.DisabledBackColor = ColorTranslator.FromHtml("#161B21")
        MetroSetComboBox3.DisabledBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox3.DisabledForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton1.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton1.NormalBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton1.NormalColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton1.NormalTextColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton1.HoverBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton1.HoverColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton1.HoverTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton1.DisabledBackColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton1.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton1.DisabledForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton1.PressBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton1.PressColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton1.PressTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton2.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton2.NormalBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton2.NormalColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton2.NormalTextColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton2.HoverBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton2.HoverColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton2.HoverTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton2.DisabledBackColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton2.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton2.DisabledForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton2.PressBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton2.PressColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton2.PressTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton3.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton3.NormalBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton3.NormalColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton3.NormalTextColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton3.HoverBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton3.HoverColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton3.HoverTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton3.DisabledBackColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton3.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton3.DisabledForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton3.PressBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton3.PressColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton3.PressTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton4.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton4.NormalBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton4.NormalColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton4.NormalTextColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton4.HoverBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton4.HoverColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton4.HoverTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton4.DisabledBackColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton4.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton4.DisabledForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton4.PressBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton4.PressColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton4.PressTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton5.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton5.NormalBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton5.NormalColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton5.NormalTextColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton5.HoverBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton5.HoverColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton5.HoverTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton5.DisabledBackColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton5.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton5.DisabledForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton5.PressBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton5.PressColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton5.PressTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton6.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton6.NormalBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton6.NormalColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton6.NormalTextColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton6.HoverBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton6.HoverColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton6.HoverTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton6.DisabledBackColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton6.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton6.DisabledForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton6.PressBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton6.PressColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton6.PressTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton7.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton7.NormalBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton7.NormalColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton7.NormalTextColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton7.HoverBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton7.HoverColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton7.HoverTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton7.DisabledBackColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton7.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton7.DisabledForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton7.PressBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetButton7.PressColor = ColorTranslator.FromHtml("#161B21")
        MetroSetButton7.PressTextColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch1.BackgroundColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch1.BorderColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch1.CheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch1.UnCheckColor = ColorTranslator.FromHtml("#161B21")
        MetroSetSwitch1.SymbolColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch1.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetSwitch1.DisabledCheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch1.DisabledUnCheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch2.BackgroundColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch2.BorderColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch2.CheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch2.UnCheckColor = ColorTranslator.FromHtml("#161B21")
        MetroSetSwitch2.SymbolColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch2.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetSwitch2.DisabledCheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch2.DisabledUnCheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch3.BackgroundColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch3.BorderColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch3.CheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch3.UnCheckColor = ColorTranslator.FromHtml("#161B21")
        MetroSetSwitch3.SymbolColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch3.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetSwitch3.DisabledCheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch3.DisabledUnCheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch4.BackgroundColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch4.BorderColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch4.CheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch4.UnCheckColor = ColorTranslator.FromHtml("#161B21")
        MetroSetSwitch4.SymbolColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch4.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetSwitch4.DisabledCheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch4.DisabledUnCheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch5.BackgroundColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch5.BorderColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch5.CheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch5.UnCheckColor = ColorTranslator.FromHtml("#161B21")
        MetroSetSwitch5.SymbolColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch5.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetSwitch5.DisabledCheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch5.DisabledUnCheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch6.BackgroundColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch6.BorderColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch6.CheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch6.UnCheckColor = ColorTranslator.FromHtml("#161B21")
        MetroSetSwitch6.SymbolColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetSwitch6.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetSwitch6.DisabledCheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetSwitch6.DisabledUnCheckColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetCheckBox1.ForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetCheckBox1.BackgroundColor = ColorTranslator.FromHtml("#161B21")
        MetroSetCheckBox1.BorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetCheckBox1.CheckSignColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetCheckBox1.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetCheckBox2.BackgroundColor = ColorTranslator.FromHtml("#161B21")
        MetroSetCheckBox2.BorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetCheckBox2.CheckSignColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetCheckBox2.ForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetCheckBox2.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetCheckBox3.BackgroundColor = ColorTranslator.FromHtml("#161B21")
        MetroSetCheckBox3.BorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetCheckBox3.CheckSignColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetCheckBox3.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetCheckBox3.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
        MetroSetCheckBox4.BackgroundColor = ColorTranslator.FromHtml("#161B21")
        MetroSetCheckBox4.BorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetCheckBox4.CheckSignColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetCheckBox4.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetCheckBox4.DisabledBorderColor = ColorTranslator.FromHtml("#161B21")
    End Sub
End Class