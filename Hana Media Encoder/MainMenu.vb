Imports System.IO
Imports System.Text.RegularExpressions
Imports Syncfusion.Windows.Forms
Imports Syncfusion.WinForms.Controls
Public Class MainMenu
    Inherits SfForm
    Dim AudioStreamFlagsPath As String = "audioStream/"
    Dim AudioStreamConfigPath As String = "audioConfig/"
    Dim debugMode As String
    Dim EncStartTime As DateTime
    Dim EncEndTime As DateTime
    Dim ffmpegConf As String
    Dim ffmpegLetter As String
    Dim ffmpegEncStats As String
    Dim ffmpegErr As String
    Dim frameMode As String
    Dim hwAccelFormat As String
    Dim hwAccelDev As String
    Dim imgPage As Integer
    Dim newdebugmode As String
    Dim openFileDialog As New OpenFileDialog
    Dim ReturnAudioStats As Boolean
    Dim ReturnVideoStats As Boolean
    Dim saveFileDialog As New SaveFileDialog
    Dim StreamInfo As String
    Dim origSavePath As String
    Dim origSaveExt As String
    Dim origSaveName As String
    Dim TimeSplit As String()
    Dim TimeDur As Integer
    Dim TimeChapter As Integer
    Dim VideoStreamFlagsPath As String = "videoStream/"
    Dim VideoStreamConfigPath As String = "videoConfig/"
    Dim VideoFilePath As String
    Private Sub MainMenu_load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        Style.TitleBar.TextHorizontalAlignment = HorizontalAlignment.Center
        Style.TitleBar.TextVerticalAlignment = VisualStyles.VerticalAlignment.Center
        MessageBoxAdv.MessageBoxStyle = MessageBoxAdv.Style.Metro
        MessageBoxAdv.CanResize = True
        MessageBoxAdv.MaximumSize = New Size(520, Screen.PrimaryScreen.WorkingArea.Size.Height)
        MessageBoxAdv.MetroColorTable = New MetroStyleColorTable
        Label69.Visible = True
        Label28.Visible = True
        Label28.Text = "Standby"
        If File.Exists("config.ini") Then
            Dim ffmpegConfig As String = FindConfig("config.ini", "FFMPEG Binary:")
            Dim debugMode As String = FindConfig("config.ini", "Debug Mode:")
            Dim hwdefConfig As String = FindConfig("config.ini", "GPU Engine:")
            If ffmpegConfig = "null" Then
                Button1.Enabled = False
                Button3.Enabled = False
                Button4.Enabled = False
                MessageBoxAdv.Show("FFMPEG Binary was not found !" & vbCrLf & vbCrLf & "Please configure it on options menu !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                ffmpegConf = ffmpegConfig.Remove(0, 14) & "\"
                ffmpegLetter = ffmpegConf.Substring(0, 1) & ":"
                If File.Exists(ffmpegConf & "ffmpeg.exe") AndAlso File.Exists(ffmpegConf & "ffplay.exe") AndAlso File.Exists(ffmpegConf & "ffprobe.exe") Then
                    If hwdefConfig = "null" Then
                        MessageBoxAdv.Show("GPU HW Encoder was not configured !" & vbCrLf & vbCrLf & "Please configure it on options menu !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        MessageBoxAdv.Show("Native encoding are not supported yet !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Button1.Enabled = False
                        Button3.Enabled = False
                        Button4.Enabled = False
                    Else
                        Button1.Enabled = True
                        Button3.Enabled = True
                        Button4.Enabled = True
                        If debugMode IsNot "null" Then
                            If debugMode.Remove(0, 11) = "True" Then
                                Text = "Hana Media Encoder (Debug Mode)"
                            End If
                        End If
                    End If
                Else
                    Button1.Enabled = False
                    Button3.Enabled = False
                    Button4.Enabled = False
                    MessageBoxAdv.Show("FFMPEG Binary is invalid !" & vbCrLf & vbCrLf & "Please configure it on options menu !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
            CleanEnv("all")
        Else
            Button1.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            MessageBoxAdv.Show("Config file was not found !" & vbCrLf & vbCrLf & "Please configure it on options menu !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub MainMenu_Close(sender As Object, e As EventArgs) Handles MyBase.FormClosed
        CleanEnv("all")
        InitExit()
    End Sub
    Private Sub Options_Btn(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Hide()
        Dim menu_options = New OptionsMenu
        menu_options.Show()
    End Sub
    Private Sub OpenMedia_Btn(sender As Object, e As EventArgs) Handles Button1.Click
        If CheckBox3.Checked Then
            MessageBoxAdv.Show("Profile locked !, please unlock profile on video tab to save media file !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf CheckBox5.Checked Then
            MessageBoxAdv.Show("Profile locked !, please unlock profile on audio tab to save media file !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            openFileDialog.DefaultExt = "*.*|.avi|.mp4|.mkv|.mp2|.m2ts|.ts|.wav|.flac|.aiff|.alac|.mp3"
            openFileDialog.FilterIndex = 1
            openFileDialog.Filter = "All files (*.*)|*.*|AVI|*.avi|MPEG-4|*.mp4|Matroska Video|*.mkv|MPEG-2|*.m2v;*.m2ts;*.ts|FLAC|*.flac|WAV|*.wav|AIFF|*.aiff|ALAC|*.alac|MP3|*.mp3"
            openFileDialog.Title = "Choose Media File"
            openFileDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
            If openFileDialog.ShowDialog() = DialogResult.OK Then
                Label2.Text = openFileDialog.FileName
                If ComboBox2.SelectedIndex >= 0 Then
                    ComboBox2.SelectedIndex = 0
                    ComboBox2.Text = ""
                End If
                ResetInit()
                ComboBox24.Enabled = True
                ComboBox23.Enabled = True
                getVideoSummary(ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34), "0")
                getAudioSummary(ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34), "0")
                getDurationSummary(ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34))
                ComboBox22.Items.Clear()
                ComboBox23.Items.Clear()
                ComboBox24.Items.Clear()
                ComboBox25.Items.Clear()
                ComboBox29.Items.Clear()
                getStreamSummary(ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34), "Encoding")
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
                getPreviewSummary(ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34))
                GenerateSpectrum()
                ComboBox31.Enabled = True
                ComboBox31.SelectedIndex = 0
                If File.Exists("thumbnail\1.png") Then
                    Dim ImgPrev1 As New FileStream("thumbnail\1.png", FileMode.Open, FileAccess.Read)
                    PictureBox1.Image = Image.FromStream(ImgPrev1)
                    ImgPrev1.Close()
                Else
                    Dim videofile As String = Chr(34) & Label2.Text & Chr(34)
                    Dim newffargs As String = "ffmpeg -hide_banner -i " & videofile & " -an -vcodec copy " & Chr(34) & My.Application.Info.DirectoryPath & "\" & "thumbnail\1.png"
                    HMEGenerate("HME_Audio_Only_Summary.bat", ffmpegLetter, ffmpegConf, newffargs, "")
                    RunProc("HME_Audio_Only_Summary.bat")
                    GC.Collect()
                    GC.WaitForPendingFinalizers()
                    File.Delete("HME_Audio_Only_Summary.bat")
                    If File.Exists("thumbnail\1.png") Then
                        Dim ImgPrev1 As New FileStream("thumbnail\1.png", FileMode.Open, FileAccess.Read)
                        PictureBox1.Image = Image.FromStream(ImgPrev1)
                        ImgPrev1.Close()
                    End If
                End If
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
                ProgressBar1.Visible = False
                Label71.Visible = False
                Label77.Visible = False
                Label28.Text = "Standby"
            Else
                Label2.Text = ""
                ProgressBar1.Visible = False
                Label70.Visible = False
                Label76.Visible = False
                Label71.Visible = False
                Label77.Visible = False
                ComboBox31.Enabled = False
            End If
        End If
    End Sub
    Private Sub SaveMedia_Btn(sender As Object, e As EventArgs) Handles Button6.Click
        If CheckBox3.Checked Then
            MessageBoxAdv.Show("Profile locked !, please unlock profile on video tab to save media file !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf CheckBox5.Checked Then
            MessageBoxAdv.Show("Profile locked !, please unlock profile on audio tab to save media file !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            saveFileDialog.DefaultExt = ".mkv|.wav|.flac|.mp3"
            saveFileDialog.FilterIndex = 1
            saveFileDialog.Filter = "MKV|*.mkv|FLAC|*.flac|WAV|*.wav|MP3|*.mp3"
            saveFileDialog.Title = "Save Media File"
            saveFileDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                TextBox1.Text = Path.GetFullPath(saveFileDialog.FileName.ToString)
                origSavePath = Path.GetDirectoryName(saveFileDialog.FileName.ToString)
                origSaveExt = Path.GetExtension(saveFileDialog.FileName.ToString.ToLower)
                origSaveName = Path.GetFileNameWithoutExtension(saveFileDialog.FileName.ToString)
            End If
        End If
    End Sub
    Private Sub ImagePreview_Next(sender As Object, e As EventArgs) Handles Button7.Click
        If ComboBox31.SelectedIndex = 0 Then
            If File.Exists("thumbnail\1.png") Then
                Dim ImgPrev2 As New FileStream("thumbnail\2.png", FileMode.Open, FileAccess.Read)
                PictureBox1.Image = Image.FromStream(ImgPrev2)
                ImgPrev2.Close()
                imgPage = 2
            End If
        ElseIf ComboBox31.SelectedIndex = 1 Then
            If File.Exists("spectrum-temp.png") Then
                Dim ImgPrev1 As New FileStream("spectrum-temp.png", FileMode.Open, FileAccess.Read)
                PictureBox1.Image = Image.FromStream(ImgPrev1)
                ImgPrev1.Close()
                imgPage = 1
            End If
        End If
    End Sub
    Private Sub ImagePreview_Prev(sender As Object, e As EventArgs) Handles Button8.Click
        If ComboBox31.SelectedIndex = 0 Then
            If File.Exists("thumbnail\1.png") Then
                Dim ImgPrev1 As New FileStream("thumbnail\1.png", FileMode.Open, FileAccess.Read)
                PictureBox1.Image = Image.FromStream(ImgPrev1)
                ImgPrev1.Close()
                imgPage = 1
            End If
        ElseIf ComboBox31.SelectedIndex = 1 Then
            If File.Exists("spectrum-temp.png") Then
                Dim ImgPrev1 As New FileStream("spectrum-temp.png", FileMode.Open, FileAccess.Read)
                PictureBox1.Image = Image.FromStream(ImgPrev1)
                ImgPrev1.Close()
            Else
                MessageBoxAdv.Show("Please generate spectrum on spectrum menu first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub
    Private Sub PreviewOptions(sender As Object, e As EventArgs) Handles ComboBox31.SelectedIndexChanged
        Dim imageDir As String
        If ComboBox31.SelectedIndex = 0 Then
            If File.Exists("thumbnail\1.png") Then
                imageDir = "thumbnail\1.png"
            Else
                imageDir = ""
            End If
        ElseIf ComboBox31.SelectedIndex = 1 Then
            If File.Exists("spectrum-temp.png") Then
                imageDir = "spectrum-temp.png"
            Else
                imageDir = ""
            End If
        End If
        If imageDir IsNot "" Then
            Dim ImgPrev1 As New FileStream(imageDir, FileMode.Open, FileAccess.Read)
            PictureBox1.Image = Image.FromStream(ImgPrev1)
            ImgPrev1.Close()
        Else
            PictureBox1.Image = Nothing
            PictureBox1.BackColor = Color.Empty
            PictureBox1.Invalidate()
        End If
    End Sub
    Private Sub PicturePreview(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim imageDir As String
        If ComboBox31.SelectedIndex = 0 Then
            If imgPage = 1 Then
                If File.Exists("thumbnail\1.png") Then
                    imageDir = "thumbnail\1.png"
                Else
                    imageDir = ""
                End If
            ElseIf imgPage = 2 Then
                If File.Exists("thumbnail\2.png") Then
                    imageDir = "thumbnail\2.png"
                Else
                    imageDir = ""
                End If
            End If
        ElseIf ComboBox31.SelectedIndex = 1 Then
            If File.Exists("spectrum-temp.png") Then
                imageDir = "spectrum-temp.png"
            Else
                imageDir = ""
            End If
        End If
        If imageDir IsNot "" Then
            Dim psi As ProcessStartInfo = New ProcessStartInfo With {
                .FileName = imageDir,
                .UseShellExecute = True
            }
            Process.Start(psi)
        Else
            MessageBoxAdv.Show("Please open file media first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub PreviewMedia(sender As Object, e As EventArgs) Handles Button4.Click
        If Label2.Text = "" Then
            MessageBoxAdv.Show("Please choose media file source first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            previewMediaModule(Label2.Text, ffmpegConf & "ffplay.exe", Label5.Text)
        End If
    End Sub
    Private Sub VideoStream_Info(sender As Object, e As EventArgs) Handles ComboBox24.SelectedIndexChanged
        getVideoSummary(ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34), (CInt(Strings.Mid(ComboBox24.Text.ToString, 11)) - 1).ToString)
    End Sub
    Public Sub getVideoSummary(ffmpegletter As String, ffmpegbin As String, videoFile As String, videoStream As String)
        Dim newffargs As String = "ffprobe -hide_banner " & " -show_streams -select_streams v:" & videoStream & " " & videoFile & " 2>&1 "
        HMEGenerate("HME_Video_Summary.bat", ffmpegletter, ffmpegbin, newffargs, "")
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
                ComboBox24.Enabled = False
            Else
                Dim codec_ln As String = getBetween(line, "codec_long_name=", "profile")
                Dim codec_type As String = getBetween(line, "codec_type=", "codec_tag_string")
                Dim codec_b_frames As String = getBetween(line, "has_b_frames=", "sample_aspect_ratio")
                Dim codec_pix_fmt As String = getBetween(line, "pix_fmt=", "level")
                Dim codec_color_range As String = getBetween(line, "color_range=", "color_space")
                Dim codec_color_space As String = getBetween(line, "color_space=", "color_transfer")
                Dim codec_frame_rate As String = getBetween(line, "avg_frame_rate=", "/")
                Dim codec_new_bit_rate As String = RemoveWhitespace(getBetween(line, "bit_rate=", "max_bit_rate="))
                'Dim codec_new_bit_rate As String = RemoveWhitespace(getBetween(line, "bitrate:", "kb/s"))
                'Dim codec_old_bit_rate As String = RemoveWhitespace(getBetween(line, "], ", "kb/s"))
                Dim codec_asp_ratio As String = getBetween(line, "display_aspect_ratio=", "pix_fmt")
                Dim codec_wi As String = getBetween(line, "width=", "height")
                Dim codec_he As String = getBetween(line, "height=", "coded_width")
                Label5.Text = codec_ln
                Label10.Text = codec_type
                Label6.Text = RemoveWhitespace(codec_wi & "x" & codec_he)
                Label14.Text = codec_b_frames
                Label7.Text = codec_asp_ratio
                Label16.Text = codec_pix_fmt
                Label18.Text = codec_color_range
                Label20.Text = codec_color_space
                If codec_frame_rate / 1000 < 1 Then
                    Label22.Text = RemoveWhitespace(codec_frame_rate) & " FPS"
                Else
                    Label22.Text = RemoveWhitespace(codec_frame_rate / 1000) & " FPS"
                End If
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
        getAudioSummary(ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34), (CInt(Strings.Mid(ComboBox23.Text.ToString, 11)) - 1).ToString)
    End Sub
    Public Sub getAudioSummary(ffmpegletter As String, ffmpegbin As String, audioFile As String, audioStream As String)
        Dim newffargs As String = "ffprobe -hide_banner " & " -show_streams -select_streams a:" & audioStream & " " & audioFile & " 2>&1 "
        HMEGenerate("HME_Audio_Summary.bat", ffmpegletter, ffmpegbin, newffargs, "")
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
                ComboBox23.Enabled = False
            Else
                Dim codec_ln As String = getBetween(line, "codec_long_name=", "profile")
                Dim codec_type As String = getBetween(line, "codec_type=", "codec_tag_string")
                Dim codec_sample_fmt As String = getBetween(line, "sample_fmt=", "sample_rate")
                Dim codec_sample_rate As String = getBetween(line, "sample_rate=", "channels")
                Dim codec_channels As String = getBetween(line, "channels=", "channel_layout")
                Dim codec_channels_layout As String = getBetween(line, "channel_layout=", "bits_per_sample")
                Dim codec_bit_per_sample As String = getBetween(line, "bits_per_sample=", "id")
                Label44.Text = codec_ln
                Label39.Text = codec_type
                Label42.Text = codec_sample_fmt
                Label35.Text = RemoveWhitespace(codec_sample_rate) & " hz"
                Label26.Text = codec_channels
                Label32.Text = codec_channels_layout
                Label30.Text = codec_bit_per_sample
                ComboBox23.Enabled = True
            End If
        End While
        process.WaitForExit()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete("HME_Audio_Summary.bat")
    End Sub
    Public Sub getDurationSummary(ffmpegletter As String, ffmpegbin As String, videoFile As String)
        Dim newffargs As String = "ffprobe -hide_banner -i " & videoFile & " 2>&1 "
        HMEGenerate("HME_Duration_Summary.bat", ffmpegletter, ffmpegbin, newffargs, "")
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
        process.WaitForExit()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete("HME_Duration_Summary.bat")
    End Sub
    Public Sub getPreviewSummary(ffmpegLetter As String, ffmpegBin As String, videoFile As String)
        Dim newffargs As String = "ffmpeg -hide_banner -i " & videoFile & " -ss 00:00:01.000 -vframes 1 " & Chr(34) & My.Application.Info.DirectoryPath & "\" & "thumbnail\1.png" & Chr(34)
        Dim newffargs2 As String = "ffmpeg -hide_banner -i " & videoFile & " -ss 00:00:05.000 -vframes 1 " & Chr(34) & My.Application.Info.DirectoryPath & "\" & "thumbnail\2.png" & Chr(34)
        HMEGenerate("HME_Image_Preview_Summary.bat", ffmpegLetter, ffmpegBin, newffargs, newffargs2)
        RunProc("HME_Image_Preview_Summary.bat")
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete("HME_Image_Preview_Summary.bat")
    End Sub
    Public Sub getStreamSummary(ffmpegletter As String, ffmpegbin As String, videoFile As String, ffmpegMode As String)
        Dim newffargs As String = "ffmpeg -hide_banner -i " & videoFile & " 2>&1 "
        HMEGenerate("HME_Stream_Summary.bat", ffmpegletter, ffmpegbin, newffargs, "")
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
    Private Sub StartEncode(sender As Object, e As EventArgs) Handles Button3.Click
        If File.Exists("config.ini") Then
            Dim ffmpegConfig As String = FindConfig("config.ini", "FFMPEG Binary:")
            Dim debugMode As String = FindConfig("config.ini", "Debug Mode:")
            Dim hwdefConfig As String = FindConfig("config.ini", "GPU Engine:")
            Dim frameConfig As String = FindConfig("config.ini", "Frame Count:")
            If ffmpegConfig IsNot "null" Then
                ffmpegConf = ffmpegConfig.Remove(0, 14) & "\"
                ffmpegLetter = ffmpegConf.Substring(0, 1) & ":"
                If File.Exists(ffmpegConf & "ffmpeg.exe") AndAlso File.Exists(ffmpegConf & "ffplay.exe") AndAlso File.Exists(ffmpegConf & "ffprobe.exe") Then
                    If hwdefConfig IsNot "null" And hwdefConfig.Remove(0, 11) IsNot "" Then
                        If Label2.Text IsNot "" Then
                            If TextBox1.Text IsNot "" Then
                                If CheckBox11.Checked = False And CheckBox11.Enabled = False And CheckBox8.Checked = True Then
                                    VideoFilePath = TextBox15.Text.ToString
                                Else
                                    VideoFilePath = Label2.Text.ToString
                                End If
                                If debugMode IsNot "null" Then
                                    newdebugmode = FindConfig("config.ini", "Debug Mode:").Remove(0, 11)
                                    If frameConfig IsNot "null" Then
                                        frameMode = FindConfig("config.ini", "Frame Count:").Remove(0, 12)
                                    Else
                                        frameMode = "False"
                                    End If
                                Else
                                    newdebugmode = "False"
                                End If
                                If hwdefConfig = "GPU Engine: " Then
                                    hwAccelFormat = ""
                                    hwAccelDev = ""
                                Else
                                    hwAccelFormat = "-hwaccel_output_format " & hwdefConfig.Remove(0, 11)
                                    hwAccelDev = hwdefConfig.Remove(0, 11)
                                End If
                                If newdebugmode = "True" Then
                                    MessageBoxAdv.Show("Warning: Debug mode was actived !" & vbCrLf & vbCrLf & "Progressbar will not working correctly while encoding" & vbCrLf & "Only use this mode to get some error log", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    MessageBoxAdv.Show("To disable debug mode, go to options then uncheck 'Debug Mode'", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                                If CheckBox1.Checked = False And CheckBox4.Checked = False And CheckBox15.Checked = False And CheckBox8.Checked = False And CheckBox6.Checked = False Then
                                    MessageBoxAdv.Show("Current configuration is not valid !" & vbCrLf & vbCrLf & "Please check current configuration", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                ElseIf CheckBox15.Checked = True And CheckBox14.Checked = True Then
                                    If File.Exists(My.Application.Info.DirectoryPath & "\FFMETADATAFILE") Then
                                        If CheckBox1.Checked = True And CheckBox3.Checked = True Then
                                            If CheckBox4.Checked = True And CheckBox5.Checked = True Then
                                                If File.Exists(AudioStreamFlagsPath & "HME_Audio_0.txt") And File.Exists(VideoStreamFlagsPath & "HME_Video_0.txt") Then
                                                    Dim flagsCount As Integer = ComboBox22.Items.Count
                                                    Dim flagsStart As Integer
                                                    For flagsStart = 1 To flagsCount
                                                        My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (flagsStart - 1).ToString & ".txt")), True)
                                                    Next
                                                    If File.Exists("HME_Audio_Flags.txt") Then
                                                        Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                                        Dim newffargs As String = "ffmpeg -hide_banner " & hwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & RichTextBox5.Text & " " & RichTextBox1.Text & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                        HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                                    End If
                                                Else
                                                    MessageBoxAdv.Show("There are still missing video or audio stream config !" & vbCrLf & vbCrLf & "Please configure video or audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                                End If
                                            ElseIf CheckBox4.Checked = False And CheckBox5.Checked = False Then
                                                Dim newffargs As String = "ffmpeg -hide_banner " & hwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & RichTextBox5.Text & " " & RichTextBox1.Text & "  -an " & Chr(34) & TextBox1.Text & Chr(34)
                                                HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                            End If
                                        Else
                                            Dim newffargs As String = "ffmpeg -hide_banner " & hwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & RichTextBox5.Text & " -c copy " & RichTextBox1.Text & Chr(34) & TextBox1.Text & Chr(34)
                                            HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                        End If
                                    Else
                                        MessageBoxAdv.Show("Please re-lock chapter profile first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    End If
                                ElseIf CheckBox7.Checked = True And CheckBox8.Checked = True Then
                                    If ComboBox1.Text = "Original Quality" Then
                                        Dim newffargs As String = "ffmpeg -hide_banner " & RichTextBox4.Text & " " & Chr(34) & TextBox1.Text & Chr(34)
                                        HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                    ElseIf ComboBox1.Text = "Custom Quality" Then
                                        If File.Exists(AudioStreamFlagsPath & "HME_Audio_0.txt") And File.Exists(VideoStreamFlagsPath & "HME_Video_0.txt") Then
                                            Dim streamCount As Integer = ComboBox22.Items.Count
                                            Dim streamStart As Integer
                                            For streamStart = 1 To streamCount
                                                My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (streamStart - 1).ToString & ".txt")), True)
                                            Next
                                            Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                            If CheckBox4.Checked = True And CheckBox5.Checked = True And CheckBox2.Checked = False Then
                                                Dim newffargs As String = "ffmpeg -hide_banner " & hwAccelFormat & " " & RichTextBox4.Text & " " & RichTextBox1.Text & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                            ElseIf CheckBox4.Checked = True And CheckBox5.Checked = False And CheckBox2.Checked = False Then
                                                Dim newffargs As String = "ffmpeg -hide_banner " & hwAccelFormat & " " & RichTextBox4.Text & " " & RichTextBox1.Text & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                            End If
                                        Else
                                            MessageBoxAdv.Show("There are still missing video or audio stream config !" & vbCrLf & vbCrLf & "Please configure video or audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End If
                                    End If
                                ElseIf CheckBox2.Checked = True And CheckBox6.Checked = True Then
                                    If ComboBox26.Text = "Original Quality" Then
                                        Dim newffargs As String
                                        If ComboBox28.Text = "Video Only" Then
                                            newffargs = "ffmpeg -hide_banner " & hwAccelFormat & " " & RichTextBox3.Text & " -map 0:0 -c:v:0 copy -an -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                        ElseIf ComboBox28.Text = "Video + Audio (Specific source)" Then
                                            newffargs = "ffmpeg -hide_banner " & hwAccelFormat & " " & RichTextBox3.Text & " -map 0:0 -map 0:" & CInt(Strings.Mid(ComboBox27.Text.ToString, 11)).ToString & " -c copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                        ElseIf ComboBox28.Text = "Video + Audio (All source)" Then
                                            newffargs = "ffmpeg -hide_banner " & hwAccelFormat & " " & RichTextBox3.Text & " -map 0 -c copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                        ElseIf ComboBox28.Text = "Audio Only (Specific Source)" Then
                                            If Label5.Text.Equals("Not Detected") = False Then
                                                newffargs = "ffmpeg -hide_banner " & hwAccelFormat & " " & RichTextBox3.Text & " -map 0:" & CInt(Strings.Mid(ComboBox27.Text.ToString, 11)).ToString & " -vn -c:a:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & " copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                            Else
                                                newffargs = "ffmpeg -hide_banner " & hwAccelFormat & " " & RichTextBox3.Text & " -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & " -c:a:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11))).ToString & " copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                            End If
                                        Else
                                            newffargs = "ffmpeg -hide_banner " & hwAccelFormat & " " & RichTextBox3.Text & " -map 0 -c copy -avoid_negative_ts 1 " & Chr(34) & TextBox1.Text & Chr(34)
                                        End If
                                        HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                    ElseIf ComboBox26.Text = "Custom Quality" Then
                                        Dim newffargs As String
                                        If ComboBox28.Text = "Video Only" Then
                                            newffargs = "ffmpeg -hide_banner " & hwAccelFormat & " " & RichTextBox3.Text & " -map 0:0 " & RichTextBox1.Text & " -an " & Chr(34) & TextBox1.Text & Chr(34)
                                            HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                        ElseIf ComboBox28.Text = "Video + Audio (Specific source)" Then
                                            If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt") And File.Exists(VideoStreamFlagsPath & "HME_Video_0.txt") Then
                                                Dim flagsCount As Integer = ComboBox22.Items.Count
                                                Dim flagsStart As Integer
                                                For flagsStart = 1 To flagsCount
                                                    My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt")), True)
                                                Next
                                                Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                                newffargs = "ffmpeg -hide_banner " & hwAccelFormat & " " & RichTextBox3.Text & " -map 0:0 -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11))).ToString & RichTextBox1.Text & " " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                            Else
                                                MessageBoxAdv.Show("There are still missing video or audio stream config !" & vbCrLf & vbCrLf & "Please configure video or audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            End If
                                        ElseIf ComboBox28.Text = "Video + Audio (All source)" Then
                                            If File.Exists(AudioStreamFlagsPath & "HME_Audio_0.txt") And File.Exists(VideoStreamFlagsPath & "HME_Video_0.txt") Then
                                                Dim flagsCount As Integer = ComboBox22.Items.Count
                                                Dim flagsStart As Integer
                                                For flagsStart = 1 To flagsCount
                                                    My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (flagsStart - 1).ToString & ".txt")), True)
                                                Next
                                                If File.Exists("HME_Audio_Flags.txt") Then
                                                    Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                                    newffargs = "ffmpeg -hide_banner " & hwAccelFormat & " " & RichTextBox3.Text & " -map 0 " & RichTextBox1.Text & " " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                    HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                                End If
                                            Else
                                                MessageBoxAdv.Show("There are still missing video or audio stream config !" & vbCrLf & vbCrLf & "Please configure video or audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            End If
                                        ElseIf ComboBox28.Text = "Audio Only (Specific Source)" Then
                                            If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt") Then
                                                Dim flagsCount As Integer = ComboBox22.Items.Count
                                                Dim flagsStart As Integer
                                                For flagsStart = 1 To flagsCount
                                                    My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt")), True)
                                                Next
                                                Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                                If Label5.Text.Equals("Not Detected") = False Then
                                                    newffargs = "ffmpeg -hide_banner " & hwAccelFormat & " " & RichTextBox3.Text & " -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11))).ToString & " -vn " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                Else
                                                    newffargs = "ffmpeg -hide_banner " & hwAccelFormat & " " & RichTextBox3.Text & " -map 0:" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                End If
                                                HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                            Else
                                                MessageBoxAdv.Show("Audio stream config not found !" & vbCrLf & vbCrLf & "Please configure audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            End If
                                        Else
                                            If File.Exists(AudioStreamFlagsPath & "HME_Audio_0.txt") And File.Exists(VideoStreamFlagsPath & "HME_Video_0.txt") Then
                                                Dim flagsCount As Integer = ComboBox22.Items.Count
                                                Dim flagsStart As Integer
                                                For flagsStart = 1 To flagsCount
                                                    My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (flagsStart - 1).ToString & ".txt")), True)
                                                Next
                                                If File.Exists("HME_Audio_Flags.txt") Then
                                                    Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                                    newffargs = "ffmpeg -hide_banner " & hwAccelFormat & RichTextBox3.Text & " -map 0 " & RichTextBox1.Text & " " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                    HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
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
                                                Dim flagsCount As Integer = ComboBox22.Items.Count
                                                Dim flagsStart As Integer
                                                For flagsStart = 1 To flagsCount
                                                    My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (flagsStart - 1).ToString & ".txt")), True)
                                                Next
                                                If File.Exists("HME_Audio_Flags.txt") Then
                                                    Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                                    Dim newffargs As String = "ffmpeg -hide_banner " & hwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & RichTextBox1.Text & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                    HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                                                End If
                                            Else
                                                MessageBoxAdv.Show("There are still missing video or audio stream config !" & vbCrLf & vbCrLf & "Please configure video or audio stream on audio tab", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            End If
                                        ElseIf CheckBox4.Checked = False And CheckBox5.Checked = False Then
                                            Dim newffargs As String = "ffmpeg -hide_banner " & hwAccelFormat & " -i " & Chr(34) & VideoFilePath & Chr(34) & RichTextBox1.Text & "  -an " & Chr(34) & TextBox1.Text & Chr(34)
                                            HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
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
                                                Dim flagsCount As Integer = ComboBox22.Items.Count
                                                Dim flagsStart As Integer
                                                For flagsStart = 1 To flagsCount
                                                    My.Computer.FileSystem.WriteAllText("HME_Audio_Flags.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & "HME_Audio_" & (flagsStart - 1).ToString & ".txt")), True)
                                                Next
                                                If File.Exists("HME_Audio_Flags.txt") Then
                                                    Dim joinAudio As String = File.ReadAllText("HME_Audio_Flags.txt")
                                                    Dim newffargs As String = "ffmpeg -hide_banner -i " & Chr(34) & VideoFilePath & Chr(34) & " -vn " & joinAudio & " " & Chr(34) & TextBox1.Text & Chr(34)
                                                    HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
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
                                ProgressBar1.Visible = True
                                Label28.Text = "ENCODE"
                                Label70.Text = GetFileSize(VideoFilePath)
                                Label71.Visible = False
                                Label77.Visible = False
                                Button1.Enabled = False
                                Button6.Enabled = False
                                TextBox1.Enabled = False
                                ProgressBar1.Value = 0
                                Dim frameCount As String
                                If newdebugmode = "True" Or frameMode = "True" Or Label5.Text.Equals("Not Detected") = True Then
                                    frameCount = "0"
                                Else
                                    Dim newffargs As String = "ffprobe -hide_banner -i " & Chr(34) & VideoFilePath & Chr(34) & " -v error -select_streams v:0 -count_packets -show_entries stream=nb_read_packets -of csv=p=0"
                                    HMEGenerate("HME_VF.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs, "")
                                    Dim psi As New ProcessStartInfo("HME_VF.bat") With {
                                        .RedirectStandardError = False,
                                        .RedirectStandardOutput = True,
                                        .CreateNoWindow = True,
                                        .WindowStyle = ProcessWindowStyle.Hidden,
                                        .UseShellExecute = False
                                    }
                                    Dim process As Process = Process.Start(psi)
                                    frameCount = "0"
                                    While Not process.StandardOutput.EndOfStream
                                        frameCount = process.StandardOutput.ReadLine
                                    End While
                                    process.WaitForExit()
                                End If
                                ProgressBar1.Minimum = 0
                                If Label5.Text.Equals("Not Detected") = False Or TextBox15.Text IsNot "" Or frameMode = "False" Or CheckBox1.Checked = True And CheckBox3.Checked = True Then
                                    ProgressBar1.Maximum = frameCount
                                Else
                                    ProgressBar1.Maximum = 100
                                End If
                                Dim new_psi As New ProcessStartInfo("HME.bat") With {
                                    .RedirectStandardError = True,
                                    .RedirectStandardOutput = False,
                                    .CreateNoWindow = True,
                                    .WindowStyle = ProcessWindowStyle.Hidden,
                                    .UseShellExecute = False
                                }
                                EncStartTime = DateTime.Now
                                If Label5.Text.Equals("Not Detected") = False Or TextBox15.Text IsNot "" Or CheckBox1.Checked = True And CheckBox3.Checked = True Then
                                    If newdebugmode = "True" Then
                                        Dim new_process As Process = Process.Start(new_psi)
                                        Do
                                            Dim line As String = new_process.StandardError.ReadLine
                                            If RemoveWhitespace(getBetween(line, "frame=", " fps")) = "" Or RemoveWhitespace(getBetween(line, "frame=", " fps")) = "0" Then
                                                ffmpegEncStats = "Frame Error!"
                                                ffmpegErr = new_process.StandardError.ReadToEnd
                                            ElseIf RemoveWhitespace(getBetween(line, "frame=", " fps")) <= frameCount Then
                                                ProgressBar1.Value = CInt(RemoveWhitespace(getBetween(line, "frame=", " fps")))
                                            End If
                                        Loop Until new_process.HasExited And new_process.StandardError.ReadLine = Nothing Or new_process.StandardError.ReadLine = ""
                                    Else
                                        Dim new_process As Process = Process.Start(new_psi)
                                        Do
                                            Dim line As String = new_process.StandardError.ReadLine
                                            If RemoveWhitespace(getBetween(line, "frame=", " fps")) = "" Or RemoveWhitespace(getBetween(line, "frame=", " fps")) = "0" Then
                                                ffmpegEncStats = "Frame Error!"
                                                ffmpegErr = new_process.StandardError.ReadToEnd
                                            ElseIf RemoveWhitespace(getBetween(line, "frame=", " fps")) <= frameCount Then
                                                ProgressBar1.Value = CInt(RemoveWhitespace(getBetween(line, "frame=", " fps")))
                                            End If
                                        Loop Until new_process.HasExited And new_process.StandardError.ReadLine = Nothing Or new_process.StandardError.ReadLine = ""
                                    End If
                                Else
                                    If newdebugmode = "True" And frameMode = "True" Or Label5.Text.Equals("Not Detected") Then
                                        Dim new_process As Process = Process.Start(new_psi)
                                        new_process.WaitForExit()
                                    ElseIf newdebugmode = "True" And frameMode = "False" Or frameMode = "null" Then
                                        Dim new_process As Process = Process.Start(new_psi)
                                        While Not new_process.StandardError.EndOfStream
                                            If ProgressBar1.Value < 100 Then
                                                ProgressBar1.Value += 20
                                            Else
                                                ProgressBar1.Value = 100
                                            End If
                                        End While
                                        new_process.WaitForExit()
                                    End If
                                End If
                                EncEndTime = DateTime.Now
                                If File.Exists(TextBox1.Text) Then
                                    Dim destFile As New FileInfo(TextBox1.Text)
                                    If destFile.Length / 1024 / 1024 < 1.0 Then
                                        If destFile.Length / 1024 < 1.0 Then
                                            If debugMode = "True" Then
                                                MessageBoxAdv.Show("Encoding failed: " & ffmpegEncStats & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error, ffmpegErr)
                                            Else
                                                MessageBoxAdv.Show("Encoding failed: " & ffmpegEncStats & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            End If
                                            Label28.Text = "ERROR"
                                            ProgressBar1.Value = ProgressBar1.Maximum
                                            ProgressBar1.ForeColor = Color.Red
                                        Else
                                            If ProgressBar1.Value <> ProgressBar1.Maximum Then
                                                ProgressBar1.Value = ProgressBar1.Maximum
                                            End If
                                            MessageBoxAdv.Show("Encoding success !" & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                            Label70.Visible = True
                                            Label76.Visible = True
                                            Label71.Visible = True
                                            Label77.Visible = True
                                            Label28.Text = "COMPLETED"
                                            Label71.Text = "" & GetFileSize(TextBox1.Text)
                                            Dim previewResult As DialogResult = MessageBoxAdv.Show(Me, "Play " & TextBox1.Text & " ? ", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                            If previewResult = DialogResult.Yes Then
                                                previewMediaModule(TextBox1.Text, ffmpegConf & "ffplay.exe", Label5.Text)
                                            End If
                                        End If
                                    Else
                                        If ProgressBar1.Value <> ProgressBar1.Maximum Then
                                            ProgressBar1.Value = ProgressBar1.Maximum
                                        End If
                                        MessageBoxAdv.Show("Encoding success !" & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        Label70.Visible = True
                                        Label76.Visible = True
                                        Label71.Visible = True
                                        Label77.Visible = True
                                        Label28.Text = "COMPLETED"
                                        Label71.Text = "" & GetFileSize(TextBox1.Text)
                                        Dim previewResult As DialogResult = MessageBoxAdv.Show(Me, "Play " & TextBox1.Text & " ? ", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                        If previewResult = DialogResult.Yes Then
                                            previewMediaModule(TextBox1.Text, ffmpegConf & "ffplay.exe", Label5.Text)
                                        End If
                                    End If
                                Else
                                    If newdebugmode = "True" Then
                                        MessageBoxAdv.Show("Encoding failed: " & ffmpegEncStats & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error, ffmpegErr)
                                    Else
                                        MessageBoxAdv.Show("Encoding failed: " & ffmpegEncStats & vbCrLf & vbCrLf & "Encoding time: " & (EncEndTime - EncStartTime).ToString("hh':'mm':'ss"), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    End If
                                    Label28.Text = "ERROR"
                                    ProgressBar1.Value = ProgressBar1.Maximum
                                    ProgressBar1.ForeColor = Color.Red
                                End If
                                Button1.Enabled = True
                                Button6.Enabled = True
                                CleanEnv("null")
                                ProgressBar1.Value = 0
                                ProgressBar1.Visible = False
                            End If
                        End If
                    Else
                        MessageBoxAdv.Show("GPU HW Accelerated are not set !" & vbCrLf & vbCrLf & "Please configure it on options menu before start encoding", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    MessageBoxAdv.Show("FFMPEG Binary is invalid !" & vbCrLf & vbCrLf & "Please configure it on options menu !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBoxAdv.Show("FFMPEG Binary not found !" & vbCrLf & vbCrLf & "Please configure it on options menu !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBoxAdv.Show("Config file was not found !" & vbCrLf & vbCrLf & "Please configure it on options menu !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub GenerateSpectrum()
        If File.Exists("config.ini") Then
            Dim ffmpegConfig As String = FindConfig("config.ini", "FFMPEG Binary:")
            If ffmpegConfig IsNot "null" Then
                ffmpegConf = ffmpegConfig.Remove(0, 14) & "\"
                ffmpegLetter = ffmpegConf.Substring(0, 1) & ":"
                If File.Exists(ffmpegConf & "ffmpeg.exe") AndAlso File.Exists(ffmpegConf & "ffplay.exe") AndAlso File.Exists(ffmpegConf & "ffprobe.exe") Then
                    If Label2.Text IsNot "" Then
                        If File.Exists("spectrum-temp.png") Then
                            GC.Collect()
                            GC.WaitForPendingFinalizers()
                            File.Delete("spectrum-temp.png")
                        End If
                        Dim newffargs As String = "ffmpeg -hide_banner -i " & Chr(34) & Label2.Text & Chr(34) & " -lavfi showspectrumpic=768x768:mode=separate " &
                            Chr(34) & My.Application.Info.DirectoryPath & "\spectrum-temp.png" & Chr(34)
                        HMEGenerate("HME.bat", ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), newffargs.Replace(vbCr, "").Replace(vbLf, ""), "")
                        RunProc("HME.bat")
                        If File.Exists("spectrum-temp.png") Then
                            Dim destFile As New FileInfo("spectrum-temp.png")
                            If destFile.Length / 1024 < 1.0 Then
                                If debugMode = "True" Then
                                    MessageBoxAdv.Show("Generate spectrum failed: " & ffmpegEncStats, "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error, ffmpegErr)
                                Else
                                    MessageBoxAdv.Show("Generate spectrum failed: " & ffmpegEncStats, "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            Else
                                'MessageBoxAdv.Show("Generate spectrum success !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        Else
                            If debugMode = "True" Then
                                MessageBoxAdv.Show("Generate spectrum failed: " & ffmpegEncStats, "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error, ffmpegErr)
                            Else
                                MessageBoxAdv.Show("Generate spectrum failed: NOT FOUND" & ffmpegEncStats, "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End If
                    Else
                        MessageBoxAdv.Show("Please choose save media file first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    MessageBoxAdv.Show("FFMPEG Binary is invalid !" & vbCrLf & vbCrLf & "Please configure it on options menu !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBoxAdv.Show("FFMPEG Binary not found !" & vbCrLf & vbCrLf & "Please configure it on options menu !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBoxAdv.Show("Config file was not found !" & vbCrLf & vbCrLf & "Please configure it on options menu !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub EnableVideoCheck(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            If Label2.Text IsNot "" Or CheckBox8.Checked And TextBox15.Text IsNot "" Then
                If Label5.Text.Equals("Not Detected") = True And TextBox15.Text Is "" Then
                    CheckBox1.Checked = False
                    MessageBoxAdv.Show("Current media file does not contain any video stream !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ComboBox2.Enabled = True
                    CheckBox3.Enabled = True
                    ComboBox29.Enabled = True
                    CheckBox13.Enabled = True
                    ComboBox29.SelectedIndex = 0
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
            TextBox3.Enabled = False
            TextBox3.Text = ""
            ComboBox21.Enabled = False
            ComboBox21.SelectedIndex = -1
            ComboBox10.Enabled = False
            ComboBox10.SelectedIndex = -1
            ComboBox30.Enabled = False
            ComboBox30.SelectedIndex = -1
            ComboBox8.Enabled = False
            ComboBox8.SelectedIndex = -1
            TextBox4.Enabled = False
            TextBox4.Text = ""
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
            TextBox2.Enabled = False
            TextBox2.Text = ""
            ComboBox6.Enabled = False
            ComboBox6.SelectedIndex = -1
            ComboBox9.Enabled = False
            ComboBox9.SelectedIndex = -1
            ComboBox29.Enabled = False
            ComboBox29.SelectedIndex = -1
            CheckBox13.Checked = False
            CheckBox13.Enabled = False
        End If
    End Sub
    Private Sub LockProfileVideoCheck(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked Then
            If TextBox1.Text = "" Then
                MessageBoxAdv.Show("Please choose save media file first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CheckBox3.Checked = False
                ComboBox2.Enabled = True
            Else
                Dim flagsCount As Integer = ComboBox29.Items.Count
                Dim flagsResult As Integer
                Dim flagsStart As Integer
                Dim flagsValue As Integer
                Dim missedFlags(255) As Integer
                For flagsStart = 1 To flagsCount
                    If File.Exists(VideoStreamFlagsPath & "HME_Video_" & (flagsStart - 1).ToString & ".txt") Then
                        flagsResult += 1
                    Else
                        missedFlags(flagsStart) = flagsStart
                    End If
                    flagsValue += 1
                Next
                If flagsResult = flagsCount Then
                    TextBox3.Enabled = False
                    ComboBox21.Enabled = False
                    ComboBox10.Enabled = False
                    ComboBox2.Enabled = False
                    ComboBox30.Enabled = False
                    ComboBox8.Enabled = False
                    TextBox4.Enabled = False
                    ComboBox14.Enabled = False
                    ComboBox5.Enabled = False
                    ComboBox3.Enabled = False
                    ComboBox7.Enabled = False
                    ComboBox4.Enabled = False
                    ComboBox11.Enabled = False
                    ComboBox12.Enabled = False
                    ComboBox13.Enabled = False
                    TextBox2.Enabled = False
                    ComboBox6.Enabled = False
                    ComboBox9.Enabled = False
                    ComboBox29.Enabled = False
                    CheckBox13.Enabled = False
                    Label28.Text = "READY"
                Else
                    For flagsStart = 1 To flagsValue
                        If missedFlags(flagsStart).ToString IsNot "" And CInt(missedFlags(flagsStart)) > 0 Then
                            MessageBoxAdv.Show("Please save configuration for video stream #0:" & missedFlags(flagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    Next
                    CheckBox3.Checked = False
                End If
            End If
        Else
            vCodecReset()
            ComboBox29.Enabled = True
            CheckBox13.Enabled = True
        End If
    End Sub
    Private Sub VideoStreamInitConfig()
        Dim tempStats As Boolean = True
        If ComboBox29.SelectedIndex >= 0 Then
            Dim hwdefConfig As String = FindConfig("config.ini", "GPU Engine:")
            Dim hwAccelFormat As String
            Dim hwAccelDev As String
            Dim aqStrength As String
            If hwdefConfig = "GPU Engine: " Then
                hwAccelFormat = ""
                hwAccelDev = ""
            Else
                hwAccelFormat = "-hwaccel_output_format " & hwdefConfig.Remove(0, 11)
                hwAccelDev = hwdefConfig.Remove(0, 11)
            End If
            Dim VideoStreamFlags As String = VideoStreamFlagsPath & "HME_Video_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
            Dim VideoStreamConfig As String = VideoStreamConfigPath & "HME_Video_Config_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
            Dim VideoStreamSource As String = (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString
            If ComboBox2.Text.Equals("Copy") = True Then
                tempStats = True
            ElseIf hwAccelDev.Equals("qsv") = True Then
                If TextBox4.Text = "" Then
                    MessageBoxAdv.Show("Please fill video max bitrate !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox3.Checked = False
                    ReturnVideoStats = False
                    tempStats = False
                ElseIf ComboBox30.Text = "" Then
                    MessageBoxAdv.Show("Please fill frame rate !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox3.Checked = False
                    ReturnVideoStats = False
                    tempStats = False
                End If
            Else
                If TextBox3.Text = "" Then
                    MessageBoxAdv.Show("Please fill video bitrate !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox3.Checked = False
                    ReturnVideoStats = False
                    tempStats = False
                ElseIf TextBox4.Text = "" Then
                    MessageBoxAdv.Show("Please fill video max bitrate !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox3.Checked = False
                    ReturnVideoStats = False
                    tempStats = False
                ElseIf ComboBox30.Text = "" Then
                    MessageBoxAdv.Show("Please fill frame rate !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox3.Checked = False
                    ReturnVideoStats = False
                    tempStats = False
                ElseIf CInt(TextBox3.Text) >= CInt(TextBox4.Text) Then
                    MessageBoxAdv.Show("Bitrate can not be more than maximum bitrate !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    TextBox3.Text = ""
                    TextBox4.Text = ""
                    CheckBox3.Checked = False
                    ReturnVideoStats = False
                    tempStats = False
                End If
            End If
            If tempStats = True Then
                If ComboBox2.Text.Equals("Copy") = True Then
                    HMEStreamProfileGenerate(VideoStreamFlags, " -c:v:" & VideoStreamSource & " copy")
                    HMEVideoStreamConfigGenerate(VideoStreamConfig, "", "", "", "Copy", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                    ReturnVideoStats = True
                Else
                    If hwAccelDev = "opencl" Then
                        If ComboBox2.Text = "H264" Then
                            HMEStreamProfileGenerate(VideoStreamFlags, " -c:v:" & VideoStreamSource & " " & vCodec(ComboBox2.Text, hwAccelDev) & " -pix_fmt " &
                                             vPixFmt(ComboBox3.Text) & " -quality " & vPresetAmf(ComboBox5.Text) & " -profile:v " & vProfile(ComboBox7.Text) &
                                             " -level " & vLevel(ComboBox8.Text) & " -b:v " & TextBox3.Text & "M -maxrate:v " & TextBox4.Text & "M -filter:v fps=fps=" & ComboBox30.Text)
                            HMEVideoStreamConfigGenerate(VideoStreamConfig, "", TextBox3.Text, "", ComboBox2.Text, ComboBox30.Text, ComboBox8.Text, TextBox4.Text, "",
                                             ComboBox5.Text, "yuv420p", ComboBox7.Text, "", "", "", "", "", "", "")
                            ReturnVideoStats = True
                        Else
                            HMEStreamProfileGenerate(VideoStreamFlags, " -c:v:" & VideoStreamSource & " " & vCodec(ComboBox2.Text, hwAccelDev) & " -pix_fmt " &
                                             vPixFmt(ComboBox3.Text) & " -quality " & vPresetAmf(ComboBox5.Text) & " -profile:v " & vProfile(ComboBox7.Text) &
                                             " -level " & vLevel(ComboBox8.Text) & " -profile_tier " & vTier(ComboBox9.Text) & " -b:v " & TextBox3.Text &
                                             "M -maxrate:v " & TextBox4.Text & "M -filter:v fps=fps=" & ComboBox30.Text)
                            HMEVideoStreamConfigGenerate(VideoStreamConfig, "", TextBox3.Text, "", ComboBox2.Text, ComboBox30.Text, ComboBox8.Text, TextBox4.Text, "",
                                             ComboBox5.Text, "yuv420p", "main", "", "", "", "", "", ComboBox9.Text, "")
                            ReturnVideoStats = True
                        End If
                    ElseIf hwAccelDev = "qsv" Then
                        HMEStreamProfileGenerate(VideoStreamFlags, " -c:v:" & VideoStreamSource & " " & vCodec(ComboBox2.Text, hwAccelDev) & " -pix_fmt " &
                                              vPixFmt(ComboBox3.Text) & " -preset " & vPreset(ComboBox5.Text) & " -profile:v " & vProfile(ComboBox7.Text) &
                                               " -maxrate:v " & TextBox4.Text & "M -filter:v fps=fps=" & ComboBox30.Text & " -low_power false")
                        HMEVideoStreamConfigGenerate(VideoStreamConfig, "", TextBox3.Text, "", ComboBox2.Text, ComboBox30.Text, "", TextBox4.Text, "", ComboBox5.Text,
                                              ComboBox3.Text, ComboBox7.Text, "", "", "", "", "", "", "")
                        ReturnVideoStats = True
                    ElseIf hwAccelDev = "cuda" Then
                        If TextBox2.Text = "" Then
                            MessageBoxAdv.Show("Please fill video target quality control !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            CheckBox3.Checked = False
                            ReturnVideoStats = False
                        Else
                            If ComboBox2.Text = "H264" Then
                                aqStrength = "-aq_strength"
                            Else
                                aqStrength = "-aq-strength"
                            End If
                            If ComboBox11.Text = "disabled" Then
                                HMEStreamProfileGenerate(VideoStreamFlags, " -c:v:" & VideoStreamSource & " " & vCodec(ComboBox2.Text, hwAccelDev) & " -pix_fmt " &
                                              vPixFmt(ComboBox3.Text) & " -rc:v:0 " & vRateControl(ComboBox4.Text) & " -cq " & TextBox2.Text & " -preset " & vPreset(ComboBox5.Text) & " -tune " &
                                              vTune(ComboBox6.Text) & " -profile:v " & vProfile(ComboBox7.Text) & " -level " & vLevel(ComboBox8.Text) & " -tier " & vTier(ComboBox9.Text) & " -bluray-compat " &
                                              vBrcompat(ComboBox21.Text) & " -b:v " & TextBox3.Text & "M -maxrate:v " & TextBox4.Text & "M -b_ref_mode " & bRefMode(ComboBox10.Text) & " -multipass " &
                                              multiPass(ComboBox14.Text) & " -filter:v fps=fps=" & ComboBox30.Text)
                                HMEVideoStreamConfigGenerate(VideoStreamConfig, ComboBox21.Text, TextBox3.Text, ComboBox10.Text, ComboBox2.Text, ComboBox30.Text, ComboBox8.Text, TextBox4.Text, ComboBox14.Text,
                                              ComboBox5.Text, ComboBox3.Text, ComboBox7.Text, ComboBox4.Text, "", "", "", TextBox2.Text, ComboBox9.Text, ComboBox6.Text)
                                ReturnVideoStats = True

                            Else
                                HMEStreamProfileGenerate(VideoStreamFlags, " -c:v:" & VideoStreamSource & " " & vCodec(ComboBox2.Text, hwAccelDev) & " -pix_fmt " &
                                              vPixFmt(ComboBox3.Text) & " -rc:v:0 " & vRateControl(ComboBox4.Text) & " -cq " & TextBox2.Text & " -preset " & vPreset(ComboBox5.Text) & " -tune " &
                                              vTune(ComboBox6.Text) & " -profile:v " & vProfile(ComboBox7.Text) & " -level " & vLevel(ComboBox8.Text) & " -tier " & vTier(ComboBox9.Text) & " -bluray-compat " &
                                              vBrcompat(ComboBox21.Text) & " -b:v " & TextBox3.Text & "M -maxrate:v " & TextBox4.Text & "M -b_ref_mode " & bRefMode(ComboBox10.Text) & " -spatial_aq " &
                                              vSpaTempAQ(ComboBox11.Text) & " " & aqStrength & " " & vAQStrength(ComboBox12.Text) & " -temporal_aq " & vSpaTempAQ(ComboBox13.Text) & " -multipass " &
                                              multiPass(ComboBox14.Text) & " -filter:v fps=fps=" & ComboBox30.Text)
                                HMEVideoStreamConfigGenerate(VideoStreamConfig, ComboBox21.Text, TextBox3.Text, ComboBox10.Text, ComboBox2.Text, ComboBox30.Text, ComboBox8.Text, TextBox4.Text, ComboBox14.Text,
                                             ComboBox5.Text, ComboBox3.Text, ComboBox7.Text, ComboBox4.Text, ComboBox11.Text, ComboBox12.Text, ComboBox13.Text, TextBox2.Text, ComboBox9.Text, ComboBox6.Text)
                                ReturnVideoStats = True
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub SaveVideoStream_Check(sender As Object, e As EventArgs) Handles CheckBox13.CheckedChanged
        If CheckBox13.Checked Then
            If ComboBox29.SelectedIndex >= 0 Then
                Dim videostreamFlags As String = VideoStreamFlagsPath & "HME_Video_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
                Dim videostreamConfig As String = VideoStreamConfigPath & "HME_Video_Config_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
                If File.Exists(videostreamFlags) And File.Exists(videostreamConfig) Then
                    Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Previous configuration already exists !" & vbCrLf & "Want to override previous configuration ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If configResult = DialogResult.Yes Then
                        GC.Collect()
                        GC.WaitForPendingFinalizers()
                        File.Delete(videostreamFlags)
                        File.Delete(videostreamConfig)
                        vCodecReset()
                        VideoStreamInitConfig()
                        If ReturnVideoStats = False Then
                            MessageBoxAdv.Show("Failed to configure for video stream #0:" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11))).ToString & " !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            CheckBox13.Checked = False
                        Else
                            RichTextBox1.Text = File.ReadAllText(videostreamFlags)
                            MessageBoxAdv.Show("Configuration for video stream #0:" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11))).ToString & " saved !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    Else
                        MessageBoxAdv.Show("Abort override configuration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    VideoStreamInitConfig()
                    If ReturnVideoStats = False Then
                        MessageBoxAdv.Show("Failed to configure for video stream #0:" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11))).ToString & " !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        CheckBox13.Checked = False
                    Else
                        RichTextBox1.Text = File.ReadAllText(videostreamFlags)
                        MessageBoxAdv.Show("Configuration for video stream #0:" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11))).ToString & " saved !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            Else
                MessageBoxAdv.Show("Please re-select video stream to save configuration ", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CheckBox13.Checked = False
            End If
        Else
            If ComboBox29.SelectedIndex >= 0 Then
                Dim videostreamFlags As String = VideoStreamFlagsPath & "HME_Video_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
                Dim videostreamConfig As String = VideoStreamConfigPath & "HME_Video_Config_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
                If File.Exists(videostreamFlags) And File.Exists(videostreamConfig) Then
                    Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Want to remove configuration for this stream #0:" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11))).ToString & " ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If configResult = DialogResult.Yes Then
                        GC.Collect()
                        GC.WaitForPendingFinalizers()
                        File.Delete(videostreamFlags)
                        File.Delete(videostreamConfig)
                        vCodecReset()
                        RichTextBox1.Text = ""
                        MessageBoxAdv.Show("Configuration for video stream #0:" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11))).ToString & " removed !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        CheckBox13.Checked = False
                    Else
                        MessageBoxAdv.Show("Abort remove configuration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            Else
                MessageBoxAdv.Show("Please re-select video stream to remove configuration ", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
    Private Sub VideoStreamSource(sender As Object, e As EventArgs) Handles ComboBox29.SelectedIndexChanged
        If ComboBox29.SelectedIndex >= 0 Then
            Dim videostreamFlags As String = VideoStreamFlagsPath & "HME_Video_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
            Dim videostreamConfig As String = VideoStreamConfigPath & "HME_Video_Config_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt"
            If File.Exists(videostreamFlags) And File.Exists(videostreamConfig) Then
                Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Previous configuration already exists !" & vbCrLf & "Want to load previous configuration ?" &
                                                                              vbCrLf & vbCrLf & "NOTE: This will override current configuration", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If configResult = DialogResult.Yes Then
                    vCodecReset()
                    Dim prevVideoBrCompat As String = FindConfig(videostreamConfig, "BRCompat=")
                    Dim prevVideoOvr As String = FindConfig(videostreamConfig, "OvrBitrate=")
                    Dim prevVideoBref As String = FindConfig(videostreamConfig, "Bref=")
                    Dim prevVideoCodec As String = FindConfig(videostreamConfig, "Codec=")
                    Dim prevVideoFps As String = FindConfig(videostreamConfig, "Fps=")
                    Dim prevVideoLvl As String = FindConfig(videostreamConfig, "Level=")
                    Dim prevVideoMaxBr As String = FindConfig(videostreamConfig, "MaxBitrate=")
                    Dim prevVideoMp As String = FindConfig(videostreamConfig, "Multipass=")
                    Dim prevVideoPreset As String = FindConfig(videostreamConfig, "Preset=")
                    Dim prevVideoPixFmt As String = FindConfig(videostreamConfig, "PixelFormat=")
                    Dim prevVideoProfile As String = FindConfig(videostreamConfig, "Profile=")
                    Dim prevVideoRateCtr As String = FindConfig(videostreamConfig, "RateControl=")
                    Dim prevVideoSpatialAQ As String = FindConfig(videostreamConfig, "SpatialAQ=")
                    Dim prevVideoAQStrength As String = FindConfig(videostreamConfig, "AQStrength=")
                    Dim prevVideoTempAQ As String = FindConfig(videostreamConfig, "TemporalAQ=")
                    Dim prevVideoTargetQL As String = FindConfig(videostreamConfig, "TargetQL=")
                    Dim prevVideoTier As String = FindConfig(videostreamConfig, "Tier=")
                    Dim prevVideoTune As String = FindConfig(videostreamConfig, "Tune=")
                    ComboBox21.Text = Strings.Mid(prevVideoBrCompat, 10)
                    TextBox3.Text = Strings.Mid(prevVideoOvr, 12)
                    ComboBox10.Text = Strings.Mid(prevVideoBref, 6)
                    ComboBox2.Text = Strings.Mid(prevVideoCodec, 7)
                    ComboBox30.Text = Strings.Mid(prevVideoFps, 5)
                    ComboBox8.Text = Strings.Mid(prevVideoLvl, 7)
                    TextBox4.Text = Strings.Mid(prevVideoMaxBr, 12)
                    ComboBox14.Text = Strings.Mid(prevVideoMp, 11)
                    ComboBox5.Text = Strings.Mid(prevVideoPreset, 8)
                    ComboBox3.Text = Strings.Mid(prevVideoPixFmt, 13)
                    ComboBox7.Text = Strings.Mid(prevVideoProfile, 9)
                    ComboBox4.Text = Strings.Mid(prevVideoRateCtr, 13)
                    ComboBox11.Text = Strings.Mid(prevVideoSpatialAQ, 11)
                    ComboBox12.Text = Strings.Mid(prevVideoAQStrength, 12)
                    ComboBox13.Text = Strings.Mid(prevVideoTempAQ, 12)
                    If Strings.Mid(prevVideoTargetQL, 10) = "" Then
                        TextBox2.Text = 0
                    Else
                        TextBox2.Text = Strings.Mid(prevVideoTargetQL, 10)
                    End If
                    ComboBox9.Text = Strings.Mid(prevVideoTier, 6)
                    ComboBox6.Text = Strings.Mid(prevVideoTune, 6)
                    RichTextBox1.Text = ""
                    RichTextBox1.Text = File.ReadAllText(videostreamFlags)
                Else
                    MessageBoxAdv.Show("Abort load previous configuration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBoxAdv.Show(Me, "Configuration profile not found for video stream #0:" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11))).ToString, "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
    Private Sub CodecCheck(sender As Object, e As EventArgs) Handles ComboBox15.SelectedIndexChanged
        Dim codecChange As DialogResult = MessageBoxAdv.Show(Me, "Warning !" & vbCrLf & vbCrLf & "Change audio will reset current configuration" &
                                                               vbCrLf & "Please save configuration on 'save config stream' options" &
                                                               vbCrLf & vbCrLf & "Change codec ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If codecChange = DialogResult.Yes Then
            BitDepthCheck()
            FrequencyCheck()
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
                TextBox6.Enabled = True
                TextBox6.Text = ""
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
                TextBox6.Enabled = False
                TextBox6.Text = ""
            ElseIf ComboBox15.Text = "MP3" Then
                ComboBox16.Enabled = True
                ComboBox16.SelectedIndex = -1
                TextBox6.Enabled = True
                TextBox6.Text = ""
                ComboBox17.Enabled = False
                ComboBox17.SelectedIndex = -1
                ComboBox18.Enabled = False
                ComboBox18.SelectedIndex = -1
                ComboBox19.Enabled = True
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
                TextBox6.Enabled = True
                TextBox6.Text = ""
            End If
        Else
            MessageBoxAdv.Show("Abort change audio codec !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub vCodecReset()
        Dim hwdefConfig As String = FindConfig("config.ini", "GPU Engine:")
        Dim hwAccelFormat As String
        Dim hwAccelDev As String
        If hwdefConfig = "GPU Engine: " Then
            hwAccelFormat = ""
            hwAccelDev = ""
        Else
            hwAccelFormat = "-hwaccel_output_format " & hwdefConfig.Remove(0, 11)
            hwAccelDev = hwdefConfig.Remove(0, 11)
        End If
        If ComboBox2.Text = "Copy" Then
            TextBox3.Enabled = False
            ComboBox21.Enabled = False
            ComboBox10.Enabled = False
            ComboBox30.Enabled = False
            ComboBox8.Enabled = False
            TextBox4.Enabled = False
            ComboBox14.Enabled = False
            ComboBox5.Enabled = False
            ComboBox3.Enabled = False
            ComboBox7.Enabled = False
            ComboBox4.Enabled = False
            ComboBox11.Enabled = False
            ComboBox12.Enabled = False
            ComboBox13.Enabled = False
            TextBox2.Enabled = False
            ComboBox6.Enabled = False
            ComboBox9.Enabled = False
        ElseIf hwAccelDev = "opencl" Then
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
            TextBox2.Enabled = False
            TextBox3.Enabled = True
            ComboBox30.Enabled = True
            ComboBox8.Enabled = True
            TextBox4.Enabled = True
            ComboBox5.Enabled = True
        ElseIf hwAccelDev = "qsv" Then
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
            TextBox2.Enabled = False
            TextBox3.Enabled = False
            ComboBox30.Enabled = True
            ComboBox8.Enabled = True
            TextBox4.Enabled = True
            ComboBox5.Enabled = True
            ComboBox7.Enabled = True
            ComboBox9.Enabled = True
        ElseIf hwAccelDev = "cuda" Then
            TextBox3.Enabled = True
            ComboBox21.Enabled = True
            ComboBox10.Enabled = True
            ComboBox30.Enabled = True
            ComboBox8.Enabled = True
            TextBox4.Enabled = True
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
            TextBox2.Enabled = True
            ComboBox6.Enabled = True
            ComboBox9.Enabled = True
        End If
        RichTextBox1.Text = ""
    End Sub
    Private Sub EnableAudioCheck(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked Then
            If Label2.Text IsNot "" Or CheckBox8.Checked And TextBox15.Text IsNot "" Then
                If Label44.Text.Equals("Not Detected") = True And TextBox16.Text Is "" Then
                    CheckBox4.Checked = False
                    MessageBoxAdv.Show("Current media file does not contain any audio stream !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ComboBox15.Enabled = True
                    CheckBox5.Enabled = True
                    ComboBox22.Enabled = True
                    CheckBox12.Enabled = True
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
            TextBox6.Enabled = False
            TextBox6.Text = ""
            ComboBox22.Enabled = False
            ComboBox22.SelectedIndex = -1
            CheckBox12.Enabled = False
            CheckBox12.Checked = False
        End If
    End Sub
    Private Sub AudioStream_Source(sender As Object, e As EventArgs) Handles ComboBox22.SelectedIndexChanged
        If ComboBox22.SelectedIndex >= 0 Then
            Dim audiostreamFlags As String = AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            Dim audiostreamConfig As String = AudioStreamConfigPath & "HME_Audio_Config_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            If File.Exists(audiostreamFlags) And File.Exists(audiostreamConfig) Then
                Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Previous configuration already exists !" & vbCrLf & "Want to load previous configuration ?" &
                                                                              vbCrLf & vbCrLf & "NOTE: This will override current configuration", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If configResult = DialogResult.Yes Then
                    aCodecReset()
                    Dim prevAudioCodec As String = FindConfig(audiostreamConfig, "Codec=")
                    Dim prevAudioBitDepth As String = FindConfig(audiostreamConfig, "BitDepth=")
                    Dim prevAudioRateControl As String = FindConfig(audiostreamConfig, "RateControl=")
                    Dim prevAudioRate As String = FindConfig(audiostreamConfig, "Rate=")
                    Dim prevAudioChannel As String = FindConfig(audiostreamConfig, "Channel=")
                    Dim prevAudioCompLvl As String = FindConfig(audiostreamConfig, "Compression=")
                    Dim prevAudioFreq As String = FindConfig(audiostreamConfig, "Frequency=")
                    RichTextBox2.Text = ""
                    RichTextBox2.Text = File.ReadAllText(audiostreamFlags)
                    ComboBox15.Text = aCodecReverse(Strings.Mid(prevAudioCodec, 7))
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
                    TextBox6.Text = Strings.Mid(prevAudioChannel, 9)
                    ComboBox17.Text = Strings.Mid(prevAudioCompLvl, 13)
                    ComboBox16.Text = Strings.Mid(prevAudioFreq, 11)
                    CheckBox12.Checked = False
                Else
                    MessageBoxAdv.Show("Abort load previous configuration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBoxAdv.Show(Me, "Configuration profile not found for audio stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString, "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
    Private Sub SaveAudioStream_Source(sender As Object, e As EventArgs) Handles CheckBox12.CheckedChanged
        If ComboBox22.SelectedIndex >= 0 Then
            Dim audiostreamFlags As String = AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            Dim audiostreamConfig As String = AudioStreamConfigPath & "HME_Audio_Config_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            If CheckBox12.Checked Then
                If File.Exists(audiostreamFlags) And File.Exists(audiostreamConfig) Then
                    Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Previous configuration already exists !" & vbCrLf & "Want to override previous configuration ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If configResult = DialogResult.Yes Then
                        GC.Collect()
                        GC.WaitForPendingFinalizers()
                        File.Delete(audiostreamFlags)
                        File.Delete(audiostreamConfig)
                        aCodecReset()
                        AudioStreamInitConfig()
                        If ReturnAudioStats = False Then
                            MessageBoxAdv.Show("Failed to configure for audio stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString & " !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            CheckBox12.Checked = False
                        Else
                            RichTextBox2.Text = File.ReadAllText(audiostreamFlags)
                            MessageBoxAdv.Show("Configuration for audio stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString & " saved !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    Else
                        MessageBoxAdv.Show("Abort override configuration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    AudioStreamInitConfig()
                    If ReturnAudioStats = False Then
                        MessageBoxAdv.Show("Failed to configure for audio stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString & " !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        CheckBox12.Checked = False
                    Else
                        RichTextBox2.Text = File.ReadAllText(audiostreamFlags)
                        MessageBoxAdv.Show("Configuration for audio stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString & " saved !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            Else
                If File.Exists(audiostreamFlags) And File.Exists(audiostreamConfig) Then
                    Dim configResult As DialogResult = MessageBoxAdv.Show(Me, "Want to remove configuration for this stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString & " ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If configResult = DialogResult.Yes Then
                        GC.Collect()
                        GC.WaitForPendingFinalizers()
                        File.Delete(audiostreamFlags)
                        File.Delete(audiostreamConfig)
                        aCodecReset()
                        RichTextBox2.Text = ""
                        MessageBoxAdv.Show("Configuration for audio stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString & " removed !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBoxAdv.Show("Abort remove configuration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End If
        Else
            MessageBoxAdv.Show("Please re-select audio stream to save configuration ", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CheckBox12.Checked = False
        End If
    End Sub
    Private Sub AudioStreamInitConfig()
        If ComboBox22.SelectedIndex >= 0 Then
            Dim AudioStreamFlags As String = AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            Dim AudioStreamConfig As String = AudioStreamConfigPath & "HME_Audio_Config_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt"
            Dim audioStreamSource As String = (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString
            If ComboBox15.Text = "MP3" Then
                If ComboBox16.Text = "" Then
                    MessageBoxAdv.Show("Please choose audio frequency !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox5.Checked = False
                    ReturnAudioStats = False
                ElseIf TextBox6.Text = "" Then
                    MessageBoxAdv.Show("Please fill audio channel !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox5.Checked = False
                    ReturnAudioStats = False
                ElseIf TextBox6.Text = "" Then
                    TextBox6.Text = "2"
                ElseIf ComboBox20.Text = "" Then
                    MessageBoxAdv.Show("Please choose audio bit rate control mode !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox5.Checked = False
                    ReturnAudioStats = False
                Else
                    If ComboBox20.Text = "CBR" Then
                        If ComboBox19.Text = "" Then
                            MessageBoxAdv.Show("Please choose audio bit rate !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            CheckBox5.Checked = False
                            ReturnAudioStats = False
                        Else
                            HMEStreamProfileGenerate(AudioStreamFlags, " -c:a:" & audioStreamSource & " " &
                                                     aCodec(ComboBox15.Text, ComboBox18.Text) & " -ac:a:" & audioStreamSource & " " & TextBox6.Text & " -b:a:" & audioStreamSource & " " & ComboBox19.Text & "k" & " -ar:a:" & audioStreamSource &
                                                     " " & ComboBox16.Text)
                            HMEAudioStreamConfigGenerate(AudioStreamConfig, aCodec(ComboBox15.Text, ComboBox18.Text), "", "CBR", ComboBox19.Text, TextBox6.Text, "", ComboBox16.Text)
                            ReturnAudioStats = True
                        End If
                    ElseIf ComboBox20.Text = "VBR" Then
                        If ComboBox17.Text = "" Then
                            MessageBoxAdv.Show("Please choose audio compression level !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            CheckBox5.Checked = False
                            ReturnAudioStats = False
                        Else
                            HMEStreamProfileGenerate(AudioStreamFlags, " -c:a:" & audioStreamSource & " " &
                                                aCodec(ComboBox15.Text, ComboBox18.Text) & " -ac:a:" & audioStreamSource & " " & TextBox6.Text & " -q:a:" & audioStreamSource & " " & ComboBox17.Text & " -ar:a:" & audioStreamSource & " " &
                                                ComboBox16.Text)
                            HMEAudioStreamConfigGenerate(AudioStreamConfig, aCodec(ComboBox15.Text, ComboBox18.Text), "", "VBR", ComboBox19.Text, TextBox6.Text, ComboBox17.Text, ComboBox16.Text)
                            ReturnAudioStats = True
                        End If
                    End If
                End If
            ElseIf ComboBox15.Text = "FLAC" Then
                If TextBox6.Text = "" Then
                    TextBox6.Text = "2"
                ElseIf ComboBox18.Text = "" Then
                    MessageBoxAdv.Show("Please choose audio bit depth !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox5.Checked = False
                    ReturnAudioStats = False
                ElseIf TextBox6.Text = "" Then
                    MessageBoxAdv.Show("Please fill audio channel !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox5.Checked = False
                    ReturnAudioStats = False
                ElseIf ComboBox17.Text = "" Then
                    MessageBoxAdv.Show("Please choose audio compression level !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox5.Checked = False
                    ReturnAudioStats = False
                ElseIf ComboBox16.Text = "" Then
                    MessageBoxAdv.Show("Please choose audio frequency !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox5.Checked = False
                    ReturnAudioStats = False
                Else
                    ComboBox19.SelectedIndex = -1
                    HMEStreamProfileGenerate(AudioStreamFlags, " -c:a:" & audioStreamSource & " " &
                                             aCodec(ComboBox15.Text, ComboBox18.Text) & " -ac:a:" & audioStreamSource & " " & TextBox6.Text & " -compression_level:a:" & audioStreamSource & " " & ComboBox17.Text & " -ar:a:" & audioStreamSource &
                                             " " & ComboBox16.Text & " -sample_fmt:a:" & audioStreamSource & " " & aBitDepth(ComboBox15.Text, ComboBox18.Text))
                    HMEAudioStreamConfigGenerate(AudioStreamConfig, aCodec(ComboBox15.Text, ComboBox18.Text), aBitDepth(ComboBox15.Text, ComboBox18.Text), "", "",
                                                TextBox6.Text, ComboBox17.Text, ComboBox16.Text)
                    ReturnAudioStats = True
                End If
            ElseIf ComboBox15.Text = "WAV" Then
                If ComboBox18.Text = "" Then
                    MessageBoxAdv.Show("Please choose audio bit depth !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox5.Checked = False
                    ReturnAudioStats = False
                ElseIf TextBox6.Text = "" Then
                    MessageBoxAdv.Show("Please fill audio channel !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox5.Checked = False
                    ReturnAudioStats = False
                ElseIf ComboBox16.Text = "" Then
                    MessageBoxAdv.Show("Please choose audio frequency !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    CheckBox5.Checked = False
                    ReturnAudioStats = False
                Else
                    HMEStreamProfileGenerate(AudioStreamFlags, " -c:a:" & audioStreamSource & " " &
                                              aCodec(ComboBox15.Text, ComboBox18.Text) & " -ac:a:" & audioStreamSource & " " & TextBox6.Text & " -ar:a:" & audioStreamSource & " " &
                                              ComboBox16.Text)
                    HMEAudioStreamConfigGenerate(AudioStreamConfig, aCodec(ComboBox15.Text, ComboBox18.Text), "", "", "", TextBox6.Text, "", ComboBox16.Text)
                    ReturnAudioStats = True
                End If
            ElseIf ComboBox15.Text = "Copy" Then
                HMEStreamProfileGenerate(AudioStreamFlags, " -c:a:" & audioStreamSource & " copy")
                HMEAudioStreamConfigGenerate(AudioStreamConfig, "copy", "", "", "", "", "", "")
                ReturnAudioStats = True
            ElseIf ComboBox15.Text Is "" Then
                MessageBoxAdv.Show("Please fill audio codecs !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ReturnAudioStats = False
            End If
            If CheckBox1.Checked = False Then
                Dim getCurrentCodec As String = ComboBox15.Text.ToLower
                If getCurrentCodec = "flac" Then
                    If getCurrentCodec = origSaveExt Then

                    Else
                        TextBox1.Text = origSavePath & "\" & origSaveName & "." & getCurrentCodec
                    End If
                ElseIf getCurrentCodec = "mp3" Or getCurrentCodec = "wav" Then
                    If getCurrentCodec = origSaveExt Then

                    Else
                        TextBox1.Text = origSavePath & "\" & origSaveName & "." & getCurrentCodec
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub LockProfileAudioCheck(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked Then
            If TextBox1.Text = "" Then
                MessageBoxAdv.Show("Please choose save media file first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CheckBox5.Checked = False
            Else
                Dim flagsCount As Integer = ComboBox22.Items.Count
                Dim flagsResult As Integer
                Dim flagsStart As Integer
                Dim flagsValue As Integer
                Dim missedFlags(255) As Integer
                If CheckBox6.Checked Then
                    If ComboBox28.SelectedText.ToString IsNot "Video + Audio (Specific source)" Or ComboBox28.SelectedText.ToString IsNot "Audio Only (Specific Source)" Then
                        If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11)) - 1).ToString & ".txt") Then
                            ComboBox15.Enabled = False
                            ComboBox16.Enabled = False
                            ComboBox17.Enabled = False
                            ComboBox18.Enabled = False
                            ComboBox19.Enabled = False
                            ComboBox20.Enabled = False
                            TextBox6.Enabled = False
                            ComboBox22.Enabled = False
                            CheckBox12.Enabled = False
                            Label28.Text = "READY"
                        Else
                            CheckBox5.Checked = False
                            MessageBoxAdv.Show("Please save configuration for audio stream #0:" & (CInt(Strings.Mid(ComboBox22.Text.ToString, 11))).ToString, "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    Else
                        For flagsStart = 1 To flagsCount
                            If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (flagsStart - 1).ToString & ".txt") Then
                                flagsResult += 1
                            Else
                                missedFlags(flagsStart) = flagsStart
                            End If
                            flagsValue += 1
                        Next
                        If flagsResult = flagsCount Then
                            ComboBox15.Enabled = False
                            ComboBox16.Enabled = False
                            ComboBox17.Enabled = False
                            ComboBox18.Enabled = False
                            ComboBox19.Enabled = False
                            ComboBox20.Enabled = False
                            TextBox6.Enabled = False
                            ComboBox22.Enabled = False
                            CheckBox12.Enabled = False
                            Label28.Text = "READY"
                        Else
                            For flagsStart = 1 To flagsValue
                                If missedFlags(flagsStart).ToString IsNot "" And CInt(missedFlags(flagsStart)) > 0 Then
                                    MessageBoxAdv.Show("Please save configuration for audio stream #0:" & missedFlags(flagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End If
                            Next
                            CheckBox5.Checked = False
                        End If
                    End If
                Else
                    For flagsStart = 1 To flagsCount
                        If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (flagsStart - 1).ToString & ".txt") Then
                            flagsResult += 1
                            flagsValue += 1
                        Else
                            missedFlags(flagsStart) = flagsStart
                            flagsValue += 1
                        End If
                    Next
                    If flagsResult = flagsCount Then
                        ComboBox15.Enabled = False
                        ComboBox16.Enabled = False
                        ComboBox17.Enabled = False
                        ComboBox18.Enabled = False
                        ComboBox19.Enabled = False
                        ComboBox20.Enabled = False
                        TextBox6.Enabled = False
                        ComboBox22.Enabled = False
                        CheckBox12.Enabled = False
                        Label28.Text = "READY"
                    Else
                        For flagsStart = 1 To flagsValue
                            If missedFlags(flagsStart).ToString IsNot "" And CInt(missedFlags(flagsStart)) > 0 Then
                                MessageBoxAdv.Show("Please save configuration for audio stream #0:" & missedFlags(flagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        Next
                        CheckBox5.Checked = False
                    End If
                End If
            End If
        Else
            aCodecReset()
            ComboBox22.Enabled = True
            ComboBox15.Enabled = True
            CheckBox12.Enabled = True
        End If
    End Sub
    Private Sub BitRateCheck(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub FrameRateCheck(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub MaxBitRateCheck(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox4.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub TargetQualityCheck(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub TargeQualityDefault(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If TextBox2.Text = "" Then
            TextBox2.Text = "0"
        Else
            If Integer.Parse(TextBox2.Text.ToString) < 0 Or Integer.Parse(TextBox2.Text.ToString) > 51 Then
                TextBox2.Text = "0"
                MessageBoxAdv.Show("Value for target quality control is between 0 to 51 !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub
    Private Sub AudioChannelCheck(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox6.KeyPress
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) Then
            MessageBoxAdv.Show("Please enter numbers only !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Handled = True
        End If
    End Sub
    Private Sub AudioChannelCheck_PH2(sender As Object, e As EventArgs) Handles TextBox6.TextChanged
        If TextBox6.Text IsNot "" Then
            If CInt(TextBox6.Text) <= 0 Or CInt(TextBox6.Text) >= 9 Then
                MessageBoxAdv.Show("Value for audio channel is between 1 to 8 !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub
    Private Sub aCodecReset()
        BitDepthCheck()
        FrequencyCheck()
        If ComboBox15.Text = "WAV" Then
            ComboBox16.Enabled = True
            ComboBox17.Enabled = False
            ComboBox18.Enabled = True
            ComboBox19.Enabled = False
            ComboBox20.Enabled = False
            TextBox6.Enabled = True
        ElseIf ComboBox15.Text = "Copy" Then
            ComboBox16.Enabled = False
            ComboBox17.Enabled = False
            ComboBox18.Enabled = False
            ComboBox19.Enabled = False
            ComboBox20.Enabled = False
            TextBox6.Enabled = False
        ElseIf ComboBox15.Text = "MP3" Then
            ComboBox16.Enabled = True
            TextBox6.Enabled = True
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
            TextBox6.Enabled = True
        End If
        RichTextBox2.Text = ""
        ComboBox15.Enabled = True
    End Sub
    Private Sub FrequencyCheck()
        If ComboBox16.Items.Contains("64000") AndAlso ComboBox16.Items.Contains("88200") AndAlso ComboBox16.Items.Contains("96000") AndAlso
               ComboBox16.Items.Contains("176400") AndAlso ComboBox16.Items.Contains("192000") Then
            If ComboBox15.Text = "MP3" Then
                ComboBox16.SelectedIndex = -1
                ComboBox16.Items.Remove("64000")
                ComboBox16.Items.Remove("88200")
                ComboBox16.Items.Remove("96000")
                ComboBox16.Items.Remove("176400")
                ComboBox16.Items.Remove("192000")
            End If
        Else
            If ComboBox15.Text = "MP3" Then
            Else
                If ComboBox16.Items.Contains("64000") = False AndAlso ComboBox16.Items.Contains("88200") = False AndAlso
                    ComboBox16.Items.Contains("96000") = False AndAlso ComboBox16.Items.Contains("176400") = False AndAlso
                    ComboBox16.Items.Contains("192000") = False Then
                    ComboBox16.SelectedIndex = -1
                    ComboBox16.Items.Add("64000")
                    ComboBox16.Items.Add("88200")
                    ComboBox16.Items.Add("96000")
                    ComboBox16.Items.Add("176400")
                    ComboBox16.Items.Add("192000")
                End If
            End If
        End If
    End Sub
    Private Sub BitDepthCheck()
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
    Private Sub MP3BitRateCheck(sender As Object, e As EventArgs) Handles ComboBox20.SelectedIndexChanged
        If ComboBox15.Text = "MP3" Then
            If ComboBox20.Text = "CBR" Then
                ComboBox19.Enabled = True
                ComboBox17.Enabled = False
            ElseIf ComboBox20.Text = "VBR" Then
                ComboBox19.Enabled = False
                ComboBox17.Enabled = True
            End If
        End If
    End Sub
    Private Sub SpatialAQCheck(sender As Object, e As EventArgs) Handles ComboBox11.SelectedIndexChanged
        If ComboBox11.Text = "disabled" Then
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
    Private Sub VideoCodecCheck(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Dim codecChange As DialogResult = MessageBoxAdv.Show(Me, "Warning !" & vbCrLf & vbCrLf & "Change video will reset current configuration" &
                                                               vbCrLf & "Please save configuration on 'save config stream' options" &
                                                               vbCrLf & vbCrLf & "Change codec ?", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If codecChange = DialogResult.Yes Then
            Dim hwdefConfig As String = FindConfig("config.ini", "GPU Engine:")
            Dim hwAccelFormat As String
            Dim hwAccelDev As String
            If hwdefConfig = "GPU Engine:" Then
                hwAccelDev = ""
            Else
                hwAccelFormat = "-hwaccel_output_format " & hwdefConfig.Remove(0, 11)
                hwAccelDev = hwdefConfig.Remove(0, 11)
            End If
            If ComboBox2.Text = "Copy" Then
                TextBox3.Enabled = False
                ComboBox21.Enabled = False
                ComboBox10.Enabled = False
                ComboBox30.Enabled = False
                ComboBox8.Enabled = False
                TextBox4.Enabled = False
                ComboBox14.Enabled = False
                ComboBox5.Enabled = False
                ComboBox3.Enabled = False
                ComboBox7.Enabled = False
                ComboBox4.Enabled = False
                ComboBox11.Enabled = False
                ComboBox12.Enabled = False
                ComboBox13.Enabled = False
                TextBox2.Enabled = False
                ComboBox6.Enabled = False
                ComboBox9.Enabled = False
                TextBox3.Text = ""
                ComboBox21.SelectedIndex = -1
                ComboBox10.SelectedIndex = -1
                ComboBox30.SelectedIndex = -1
                ComboBox8.SelectedIndex = -1
                TextBox4.Text = ""
                ComboBox14.SelectedIndex = -1
                ComboBox5.SelectedIndex = -1
                ComboBox3.SelectedIndex = -1
                ComboBox7.SelectedIndex = -1
                ComboBox4.SelectedIndex = -1
                ComboBox11.SelectedIndex = -1
                ComboBox12.SelectedIndex = -1
                ComboBox13.SelectedIndex = -1
                TextBox2.Text = ""
                ComboBox6.SelectedIndex = -1
                ComboBox9.SelectedIndex = -1
            ElseIf hwAccelDev = "opencl" Then
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
                TextBox2.Enabled = False
                TextBox3.Enabled = True
                ComboBox30.Enabled = True
                ComboBox8.Enabled = True
                TextBox4.Enabled = True
                ComboBox5.Enabled = True
                ComboBox21.SelectedIndex = -1
                ComboBox10.SelectedIndex = -1
                ComboBox4.SelectedIndex = -1
                ComboBox11.SelectedIndex = -1
                ComboBox12.SelectedIndex = -1
                ComboBox13.SelectedIndex = -1
                TextBox2.Text = ""
                ComboBox6.SelectedIndex = -1
            ElseIf hwAccelDev = "qsv" Then
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
                TextBox2.Enabled = False
                TextBox3.Enabled = False
                ComboBox30.Enabled = True
                ComboBox8.Enabled = False
                TextBox4.Enabled = True
                ComboBox5.Enabled = True
                ComboBox7.Enabled = True
                ComboBox9.Enabled = True
                ComboBox21.SelectedIndex = -1
                ComboBox10.SelectedIndex = -1
                ComboBox4.SelectedIndex = -1
                ComboBox11.SelectedIndex = -1
                ComboBox12.SelectedIndex = -1
                ComboBox13.SelectedIndex = -1
                TextBox2.Text = ""
                ComboBox6.SelectedIndex = -1
            ElseIf hwAccelDev = "cuda" Then
                TextBox3.Enabled = True
                ComboBox21.Enabled = True
                ComboBox10.Enabled = True
                ComboBox30.Enabled = True
                ComboBox8.Enabled = True
                TextBox4.Enabled = True
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
                TextBox2.Enabled = True
                ComboBox6.Enabled = True
                ComboBox9.Enabled = True
            End If
        Else
            MessageBoxAdv.Show("Abort change video codec !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim menu_information = New InformationMenu
        menu_information.Show()
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
        getStreamSummary(ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), Chr(34) & Label2.Text & Chr(34), "Trim")
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
                If ProgressBar1.Value <> 0 Then
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
    Private Sub LockProfile_Trim(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        Dim OriTime As Integer
        Dim TrimStartTime As Integer
        Dim TrimEndTime As Integer
        Dim TrimCondition As Boolean
        Dim TrimPreCondition As Integer
        If CheckBox2.Checked Then
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
                    If Strings.Right(TextBox1.Text, 4) = "flac" Or Strings.Right(TextBox1.Text, 4) = ".mp3" Or Strings.Right(TextBox1.Text, 4) = ".wav" Then
                        MessageBoxAdv.Show("Invalid file extension for saved media file !" & vbCrLf & vbCrLf &
                                           "Current file extensions " & vbCrLf &
                                           Strings.Right(TextBox1.Text, 4) & vbCrLf & vbCrLf &
                                           "Current available file extensions " & vbCrLf &
                                           ".mkv", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        If ComboBox26.SelectedIndex = 1 Then
                            If ComboBox28.SelectedIndex = 0 Then
                                If ComboBox27.Text Is "" Then
                                    MessageBoxAdv.Show("Please choose trim source first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    CheckBox2.Checked = False
                                    TrimCondition = False
                                Else
                                    Dim flagsCount As Integer = ComboBox29.Items.Count
                                    Dim flagsResult As Integer
                                    Dim flagsStart As Integer
                                    Dim flagsValue As Integer
                                    Dim missedFlags(255) As Integer
                                    For flagsStart = 1 To flagsCount
                                        If File.Exists(VideoStreamFlagsPath & "HME_Video_" & (flagsStart - 1).ToString & ".txt") Then
                                            flagsResult += 1
                                        Else
                                            missedFlags(flagsStart) = flagsStart
                                        End If
                                        flagsValue += 1
                                    Next
                                    If flagsResult = flagsCount Then
                                        TrimCondition = True
                                    Else
                                        For flagsStart = 1 To flagsValue
                                            If missedFlags(flagsStart).ToString IsNot "" And CInt(missedFlags(flagsStart)) > 0 Then
                                                MessageBoxAdv.Show("Please save configuration for video stream #0:" & missedFlags(flagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                                    Dim flagsVideoCount As Integer = CInt(Strings.Mid(ComboBox29.Text.ToString, 11))
                                    Dim flagsAudioCount As Integer = CInt(Strings.Mid(ComboBox27.Text.ToString, 11))
                                    Dim flagsResult As Integer
                                    Dim flagsStart As Integer
                                    Dim flagsVideoValue As Integer
                                    Dim flagsAudioValue As Integer
                                    Dim missedFlags(255) As Integer
                                    For flagsStart = 1 To flagsVideoCount
                                        If File.Exists(VideoStreamFlagsPath & "HME_Video_" & (CInt(Strings.Mid(ComboBox29.Text.ToString, 11)) - 1).ToString & ".txt") Then
                                            flagsResult += 1
                                        Else
                                            missedFlags(flagsStart) = flagsStart
                                        End If
                                        flagsVideoValue += 1
                                    Next
                                    If flagsResult = flagsVideoCount Then
                                        TrimPreCondition += 1
                                    Else
                                        For flagsStart = 1 To flagsVideoValue
                                            If missedFlags(flagsStart).ToString IsNot "" And CInt(missedFlags(flagsStart)) > 0 Then
                                                MessageBoxAdv.Show("Please save configuration for video Stream #0:" & missedFlags(flagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            End If
                                        Next
                                        TrimPreCondition = 0
                                    End If
                                    flagsResult = 0
                                    For flagsStart = 1 To flagsAudioCount
                                        If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt") Then
                                            flagsResult += 1
                                        Else
                                            missedFlags(flagsStart) = flagsStart
                                        End If
                                        flagsAudioValue += 1
                                    Next
                                    If flagsResult = flagsAudioCount Then
                                        TrimPreCondition += 1
                                    Else
                                        For flagsStart = 1 To flagsAudioValue
                                            If missedFlags(flagsStart).ToString IsNot "" And CInt(missedFlags(flagsStart)) > 0 Then
                                                MessageBoxAdv.Show("Please save configuration for audio Stream #0:" & missedFlags(flagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                                Dim flagsVideoCount As Integer = ComboBox29.Items.Count
                                Dim flagsAudioCount As Integer = ComboBox22.Items.Count
                                Dim flagsResult As Integer
                                Dim flagsStart As Integer
                                Dim flagsVideoValue As Integer
                                Dim flagsAudioValue As Integer
                                Dim missedFlags(255) As Integer
                                For flagsStart = 1 To flagsVideoCount
                                    If File.Exists(VideoStreamFlagsPath & "HME_Video_" & (flagsStart - 1).ToString & ".txt") Then
                                        flagsResult += 1
                                    Else
                                        missedFlags(flagsStart) = flagsStart
                                    End If
                                    flagsVideoValue += 1
                                Next
                                If flagsResult = flagsVideoCount Then
                                    TrimPreCondition += 1
                                Else
                                    For flagsStart = 1 To flagsVideoValue
                                        If missedFlags(flagsStart).ToString IsNot "" And CInt(missedFlags(flagsStart)) > 0 Then
                                            MessageBoxAdv.Show("Please save configuration for video Stream #0:" & missedFlags(flagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End If
                                    Next
                                    TrimPreCondition = 0
                                End If
                                flagsResult = 0
                                For flagsStart = 1 To flagsAudioCount
                                    If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (flagsStart - 1).ToString & ".txt") Then
                                        flagsResult += 1
                                    Else
                                        missedFlags(flagsStart) = flagsStart
                                    End If
                                    flagsAudioValue += 1
                                Next
                                If flagsResult = flagsAudioCount Then
                                    TrimPreCondition += 1
                                Else
                                    For flagsStart = 1 To flagsAudioValue
                                        If missedFlags(flagsStart).ToString IsNot "" And CInt(missedFlags(flagsStart)) > 0 Then
                                            MessageBoxAdv.Show("Please save configuration for audio Stream #0:" & missedFlags(flagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                    If Strings.Right(TextBox1.Text, 4) = ".mkv" Then
                        MessageBoxAdv.Show("Invalid file extension for saved media file !" & vbCrLf & vbCrLf &
                                           "Current file extensions " & vbCrLf &
                                           Strings.Right(TextBox1.Text, 4) & vbCrLf & vbCrLf &
                                           "Current available file extensions " & vbCrLf &
                                           ".flac, .mp3., .wav", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        If ComboBox26.SelectedIndex = 1 Then
                            If ComboBox28.SelectedIndex = 3 Then
                                Dim flagsCount As Integer = ComboBox22.Items.Count
                                Dim flagsResult As Integer
                                Dim flagsStart As Integer
                                Dim flagsValue As Integer
                                Dim missedFlags(255) As Integer
                                For flagsStart = 1 To flagsCount
                                    If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (CInt(Strings.Mid(ComboBox27.Text.ToString, 11)) - 1).ToString & ".txt") Then
                                        flagsResult += 1
                                    Else
                                        missedFlags(flagsStart) = flagsStart
                                    End If
                                    flagsValue += 1
                                Next
                                If flagsResult = flagsCount Then
                                    TrimCondition = True
                                Else
                                    For flagsStart = 1 To flagsValue
                                        If missedFlags(flagsStart).ToString IsNot "" And CInt(missedFlags(flagsStart)) > 0 Then
                                            MessageBoxAdv.Show("Please save configuration for audio stream #0:" & missedFlags(flagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        If CheckBox8.Checked Then
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
        If CheckBox11.Checked Then
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
        openFileDialog.DefaultExt = "*.*|.avi|.mp4|.mkv|.mp2|.m2ts|.ts"
        openFileDialog.FilterIndex = 1
        openFileDialog.Filter = "All files (*.*)|*.*|AVI|*.avi|MPEG-4|*.mp4|Matroska Video|*.mkv|MPEG-2|*.m2v;*.m2ts;*.ts"
        openFileDialog.Title = "Choose Media File"
        openFileDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            TextBox15.Text = openFileDialog.FileName
            ComboBox25.Items.Clear()
            getStreamSummary(ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), Chr(34) & TextBox15.Text & Chr(34), "Muxing")
        End If
    End Sub
    Private Sub BrowseAudio_Muxing(sender As Object, e As EventArgs) Handles Button10.Click
        openFileDialog.DefaultExt = "*.*|.flac|.aiff|.alac|.mp3"
        openFileDialog.FilterIndex = 1
        openFileDialog.Filter = "All files (*.*)|*.*|FLAC|*.flac|WAV|*.wav|AIFF|*.aiff|ALAC|*.alac|MP3|*.mp3"
        openFileDialog.Title = "Choose Media File"
        openFileDialog.InitialDirectory = Environment.SpecialFolder.UserProfile
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            TextBox16.Text = openFileDialog.FileName
        End If
    End Sub
    Private Sub ReplaceExistingAudio_Check(sender As Object, e As EventArgs) Handles CheckBox9.CheckedChanged
        If CheckBox9.Checked Then
            CheckBox10.Checked = False
            CheckBox10.Enabled = False
            ComboBox25.Enabled = True
            getStreamSummary(ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), Chr(34) & TextBox15.Text & Chr(34), "Muxing")
        Else
            CheckBox10.Checked = False
            CheckBox10.Enabled = True
            ComboBox25.Enabled = False
            ComboBox25.SelectedIndex = -1
        End If
    End Sub
    Private Sub AddAsNewAudioStream_Check(sender As Object, e As EventArgs) Handles CheckBox10.CheckedChanged
        If CheckBox10.Checked Then
            CheckBox9.Checked = False
            CheckBox9.Enabled = False
            ComboBox25.Enabled = False
            ComboBox25.SelectedIndex = -1
            getStreamSummary(ffmpegLetter, Chr(34) & ffmpegConf & Chr(34), Chr(34) & TextBox15.Text & Chr(34), "Muxing + Custom")
        Else
            ComboBox25.Enabled = False
            ComboBox25.SelectedIndex = -1
            CheckBox9.Checked = False
            CheckBox9.Enabled = True
        End If
    End Sub
    Private Sub LockProfile_Mux(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        Dim videoFilePath As String
        If CheckBox7.Checked Then
            If TextBox1.Text = "" Then
                MessageBoxAdv.Show("Please choose save media file first !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                CheckBox7.Checked = False
            Else
                If Label2.Text IsNot "" Or TextBox15.Text IsNot "" Then
                    If TextBox16.Text IsNot "" Then
                        If Strings.Right(TextBox1.Text, 4) = "flac" Or Strings.Right(TextBox1.Text, 4) = ".mp3" Or Strings.Right(TextBox1.Text, 4) = ".wav" Then
                            MessageBoxAdv.Show("Current file extensions " & vbCrLf &
                                                Strings.Right(TextBox1.Text, 4) & vbCrLf & vbCrLf &
                                                "Current available file extensions " & vbCrLf &
                                                ".mkv", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Else
                            Dim trimcondition As Boolean
                            Dim trimprecondition As Integer
                            Button9.Enabled = False
                            Button10.Enabled = False
                            If ComboBox1.Text Is "" Then
                                ComboBox1.SelectedIndex = 0
                            End If
                            ComboBox1.Enabled = False
                            If CheckBox11.Checked = False Then
                                videoFilePath = TextBox15.Text.ToString
                            Else
                                videoFilePath = Label2.Text.ToString
                            End If
                            CheckBox8.Enabled = False
                            CheckBox11.Enabled = False
                            CheckBox9.Enabled = False
                            CheckBox10.Enabled = False
                            ComboBox25.Enabled = False
                            If ComboBox1.SelectedIndex = 1 Then
                                Dim flagsVideoCount As Integer = ComboBox29.Items.Count
                                Dim flagsAudioCount As Integer = ComboBox22.Items.Count
                                Dim flagsResult As Integer
                                Dim flagsStart As Integer
                                Dim flagsVideoValue As Integer
                                Dim flagsAudioValue As Integer
                                Dim missedFlags(255) As Integer
                                For flagsStart = 1 To flagsVideoCount
                                    If File.Exists(VideoStreamFlagsPath & "HME_Video_" & (flagsStart - 1).ToString & ".txt") Then
                                        flagsResult += 1
                                    Else
                                        missedFlags(flagsStart) = flagsStart
                                    End If
                                    flagsVideoValue += 1
                                Next
                                If flagsResult = flagsVideoCount Then
                                    trimprecondition += 1
                                Else
                                    For flagsStart = 1 To flagsVideoValue
                                        If missedFlags(flagsStart).ToString IsNot "" And CInt(missedFlags(flagsStart)) > 0 Then
                                            MessageBoxAdv.Show("Please save configuration for video Stream #0:" & missedFlags(flagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End If
                                    Next
                                    trimprecondition = 0
                                End If
                                flagsResult = 0
                                For flagsStart = 1 To flagsAudioCount
                                    If File.Exists(AudioStreamFlagsPath & "HME_Audio_" & (flagsStart - 1).ToString & ".txt") Then
                                        flagsResult += 1
                                    Else
                                        missedFlags(flagsStart) = flagsStart
                                    End If
                                    flagsAudioValue += 1
                                Next
                                If flagsResult = flagsAudioCount Then
                                    trimprecondition += 1
                                Else
                                    For flagsStart = 1 To flagsAudioValue
                                        If missedFlags(flagsStart).ToString IsNot "" And CInt(missedFlags(flagsStart)) > 0 Then
                                            MessageBoxAdv.Show("Please save configuration for audio Stream #0:" & missedFlags(flagsStart), "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        End If
                                    Next
                                    trimprecondition = 0
                                End If
                                If trimprecondition = 2 Then
                                    trimcondition = True
                                ElseIf trimprecondition < 2 Then
                                    trimcondition = False
                                End If
                            ElseIf ComboBox1.SelectedIndex = 0 Then
                                trimcondition = True
                            End If
                            If trimcondition = True Then
                                If CheckBox9.Checked = True And CheckBox10.Checked = False Then
                                    If ComboBox25.SelectedIndex = -1 Then
                                        MessageBoxAdv.Show("Please choose which audio stream that want to replace !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Else
                                        Dim AudioStream As String = (CInt(Strings.Mid(ComboBox25.Text.ToString, 11)) - 1).ToString
                                        Dim AudioStreamArray As String = CInt(Strings.Mid(ComboBox25.Text.ToString, 11)).ToString
                                        Dim numbers(255) As String
                                        For flagsStart = 1 To ComboBox25.Items.Count
                                            numbers(flagsStart) = flagsStart
                                        Next
                                        If File.Exists("HME_Stream_Replace.txt") Then
                                            GC.Collect()
                                            GC.WaitForPendingFinalizers()
                                            File.Delete("HME_Stream_Replace.txt")
                                            File.Create("HME_Stream_Replace.txt").Dispose()
                                        End If
                                        For flagsstart = 1 To ComboBox25.Items.Count
                                            File.AppendAllText("HME_Stream_Replace.txt", " -map 0:" & numbers(flagsstart))
                                        Next
                                        If FindConfig("HME_Stream_Replace.txt", "-map 0:" & AudioStreamArray) IsNot "" Then
                                            Dim ReplaceStream As String = File.ReadAllText("HME_Stream_Replace.txt")
                                            ReplaceStream = ReplaceStream.Replace(" -map 0:" & AudioStreamArray, " -map 1:0 ")
                                            File.WriteAllText("HME_Stream_Replace.txt", ReplaceStream)
                                        End If
                                        If ComboBox1.Text = "Original Quality" Then
                                            RichTextBox4.Text = " -i " & Chr(34) & videoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0:0 " & File.ReadAllText("HME_Stream_Replace.txt") & " -c copy "
                                        ElseIf ComboBox1.Text = "Custom Quality" Then
                                            RichTextBox4.Text = " -i " & Chr(34) & videoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0:0 " & File.ReadAllText("HME_Stream_Replace.txt") & ""
                                        End If
                                    End If
                                ElseIf CheckBox9.Checked = False And CheckBox10.Checked = True Then
                                    If ComboBox1.Text = "Original Quality" Then
                                        RichTextBox4.Text = " -i " & Chr(34) & videoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0 -map 1:a -c copy "
                                    ElseIf ComboBox1.Text = "Custom Quality" Then
                                        RichTextBox4.Text = " -i " & Chr(34) & videoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0 -map 1:a "
                                    End If
                                ElseIf CheckBox9.Checked = False And CheckBox10.Checked = False Then
                                    If ComboBox1.Text = "Original Quality" Then
                                        RichTextBox4.Text = " -i " & Chr(34) & videoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0 -map 1:a -c copy "
                                    ElseIf ComboBox1.Text = "Custom Quality" Then
                                        RichTextBox4.Text = " -i " & Chr(34) & videoFilePath & Chr(34) & " -i " & Chr(34) & TextBox16.Text & Chr(34) & " -map 0 -map 1:a "
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
        If CheckBox15.Checked Then
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
                    RichTextBox5.Enabled = True
                    CheckBox8.Checked = False
                    CheckBox8.Enabled = False
                    CheckBox6.Checked = False
                    CheckBox6.Enabled = False
                    ChapterReset()
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
            RichTextBox5.Enabled = False
            RichTextBox5.Text = ""
            CheckBox8.Enabled = True
            CheckBox6.Enabled = True
            ChapterReset()
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
                    RichTextBox5.Text = " -i " & Chr(34) & My.Application.Info.DirectoryPath & "\FFMETADATAFILE" & Chr(34) & " -map_metadata 1 "
                    TextBox5.Enabled = False
                    TextBox17.Enabled = False
                    TextBox18.Enabled = False
                    TextBox19.Enabled = False
                    Button11.Enabled = False
                    Button12.Enabled = False
                    Button13.Enabled = False
                    Button14.Enabled = False
                    RichTextBox5.Enabled = False
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
    Private Sub AddChapter(sender As Object, e As EventArgs) Handles Button11.Click
        If TextBox5.Text IsNot "" AndAlso TextBox17.Text IsNot "" AndAlso TextBox18.Text IsNot "" AndAlso TextBox19.Text IsNot "" Then
            If ChapterTimeCheck() = True Then
                Dim newTime As String = TextBox5.Text & ":" & TextBox17.Text & ":" & TextBox18.Text
                Dim newChapter As New ListViewItem(newTime)
                newChapter.SubItems.Add(TextBox19.Text)
                ListView1.Items.Add(newChapter)
                MessageBoxAdv.Show("Chapter successfully added !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
                Dim newTime As String = TextBox5.Text & ":" & TextBox17.Text & ":" & TextBox18.Text
                ChapterReplace("update", newTime)
                MessageBoxAdv.Show("Chapter successfully updated !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ChapterReset()
            Else
                MessageBoxAdv.Show("Time chapter can not more than video duration !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBoxAdv.Show("Update chapter failed !" & vbCrLf & vbCrLf & "Make sure to fill time and chapter title completely", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub RemoveChapter(sender As Object, e As EventArgs) Handles Button12.Click
        ChapterReplace("remove", "")
    End Sub
    Private Sub ChapterReset(sender As Object, e As EventArgs) Handles Button14.Click
        MessageBoxAdv.Show("Chapter data has been reset !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ListView1.Items.Clear()
    End Sub
    Private Sub ChapterList_Clicked(sender As Object, e As EventArgs) Handles ListView1.Click
        If TextBox5.Text IsNot "" Or TextBox17.Text IsNot "" Or TextBox18.Text IsNot "" Or TextBox19.Text IsNot "" Then
            Dim chapterResult As DialogResult = MessageBoxAdv.Show(Me, "Want to replace current data" & TextBox1.Text & " ? ", "Hana Media Encoder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
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
    Private Sub ResetInit()
        CheckBox1.Checked = False
        CheckBox3.Enabled = False
        CheckBox3.Checked = False
        CheckBox4.Checked = False
        CheckBox7.Checked = False
        CheckBox8.Checked = False
        CheckBox6.Checked = False
        CheckBox2.Checked = False
        CheckBox2.Enabled = False
        CheckBox12.Enabled = False
        CheckBox12.Checked = False
        CheckBox11.Enabled = False
        CheckBox11.Checked = False
        CheckBox9.Enabled = False
        CheckBox9.Checked = False
        CheckBox10.Enabled = False
        CheckBox10.Checked = False
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
            If CInt(TextBox8.Text) > 60 Then
                MessageBoxAdv.Show("Maximum value for minute is 60 !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox8.Text = ""
            End If
        End If
    End Sub
    Private Sub StartTime_Seconds(sender As Object, e As EventArgs) Handles TextBox9.TextChanged
        If TextBox9.Text IsNot "" Then
            If CInt(TextBox9.Text) > 60 Then
                MessageBoxAdv.Show("Maximum value for minute is 60 !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox9.Text = ""
            End If
        End If
    End Sub
    Private Sub EndTime_Minute(sender As Object, e As EventArgs) Handles TextBox13.TextChanged
        If TextBox13.Text IsNot "" Then
            If CInt(TextBox13.Text) > 60 Then
                MessageBoxAdv.Show("Maximum value for minute is 60 !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox13.Text = ""
            End If
        End If
    End Sub
    Private Sub EndTime_Seconds(sender As Object, e As EventArgs) Handles TextBox12.TextChanged
        If TextBox12.Text IsNot "" Then
            If CInt(TextBox12.Text) > 60 Then
                MessageBoxAdv.Show("Maximum value for minute is 60 !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                TextBox12.Text = ""
            End If
        End If
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
End Class