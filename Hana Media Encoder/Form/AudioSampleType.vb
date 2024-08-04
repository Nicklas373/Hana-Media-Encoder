Imports Syncfusion.WinForms.Controls

Public Class AudioSampleType
    Inherits SfForm

    Private Sub AudioSampleType_load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        If AudioTEMPSmpRate IsNot "null" Then
            ComboBox1.Text = AudioTEMPSmpRate
        Else
            ComboBox1.SelectedIndex = -1
        End If
        If AudioTEMPChn IsNot "null" Then
            ComboBox33.Text = AudioTEMPChn
        Else
            ComboBox33.SelectedIndex = -1
        End If
        If AudioTEMPChnMapping IsNot "null" Then
            ComboBox34.Text = AudioTEMPChnMapping
        Else
            ComboBox34.SelectedIndex = -1
        End If
        If AudioTEMPBitDepth IsNot "null" Then
            ComboBox18.Text = AudioTEMPBitDepth
        Else
            ComboBox18.SelectedIndex = -1
        End If
        If String.IsNullOrEmpty(AudioTEMPFormatOpt) = False Then
            If AudioTEMPFormatOpt = "Wave PCM (*.wav)" Then
                TrackBar1.Enabled = False
                Label3.Text = "0%"
                ComboBox2.Items.Clear()
                ComboBox2.Items.Add("None")
                ComboBox2.Items.Add("CD Quality")
                ComboBox2.Items.Add("DVD Quality")
                ComboBox2.Items.Add("Hi-Fi Quality")
                ComboBox2.Items.Add("Hi-Res Quality")
            ElseIf AudioTEMPFormatOpt = "Free Lossless Audio Codec (*.flac)" Then
                TrackBar1.Enabled = True
                TrackBar1.Maximum = 10
                If String.IsNullOrEmpty(AudioTEMPCnvRatio) = False Then
                    TrackBar1.Value = AudioTEMPCnvRatio
                    If AudioTEMPCnvRatio = 0 Then
                        Label3.Text = "0%"
                    ElseIf AudioTEMPCnvRatio = 1 Then
                        Label3.Text = "10%"
                    ElseIf AudioTEMPCnvRatio = 2 Then
                        Label3.Text = "20%"
                    ElseIf AudioTEMPCnvRatio = 3 Then
                        Label3.Text = "30%"
                    ElseIf AudioTEMPCnvRatio = 4 Then
                        Label3.Text = "40%"
                    ElseIf AudioTEMPCnvRatio = 5 Then
                        Label3.Text = "50%"
                    ElseIf AudioTEMPCnvRatio = 6 Then
                        Label3.Text = "60%"
                    ElseIf AudioTEMPCnvRatio = 7 Then
                        Label3.Text = "70%"
                    ElseIf AudioTEMPCnvRatio = 8 Then
                        Label3.Text = "80%"
                    ElseIf AudioTEMPCnvRatio = 9 Then
                        Label3.Text = "90%"
                    ElseIf AudioTEMPCnvRatio = 10 Then
                        Label3.Text = "100%"
                    End If
                End If
                ComboBox2.Items.Clear()
                ComboBox2.Items.Add("None")
                ComboBox2.Items.Add("Low Quality")
                ComboBox2.Items.Add("Medium Quality")
                ComboBox2.Items.Add("High Quality")
                ComboBox2.Items.Add("Hi-Fi Quality")
                ComboBox2.Items.Add("Hi-Res Quality")
            Else
                ComboBox2.Items.Clear()
                ComboBox2.Items.Add("None")
                ComboBox2.Items.Add("Low Quality")
                ComboBox2.Items.Add("Medium Quality")
                ComboBox2.Items.Add("High Quality")
                TrackBar1.Enabled = True
                TrackBar1.Maximum = 5
                If AudioTEMPCnvRatio IsNot "" Then
                    TrackBar1.Value = AudioTEMPCnvRatio
                    If AudioTEMPCnvRatio = 0 Then
                        Label3.Text = "0%"
                    ElseIf AudioTEMPCnvRatio = 1 Then
                        Label3.Text = "20%"
                    ElseIf AudioTEMPCnvRatio = 2 Then
                        Label3.Text = "40%"
                    ElseIf AudioTEMPCnvRatio = 3 Then
                        Label3.Text = "60%"
                    ElseIf AudioTEMPCnvRatio = 4 Then
                        Label3.Text = "80%"
                    ElseIf AudioTEMPCnvRatio = 5 Then
                        Label3.Text = "100%"
                    End If
                End If
            End If
        End If
        If audioTEMPProfile IsNot "null" Then
            ComboBox2.Text = audioTEMPProfile
        End If
        AudioFrequencyCheck()
        AudioBitDepthCheck()
    End Sub
    Private Sub ProfileBtn(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If AudioTEMPFormatOpt = "Wave PCM (*.wav)" Then
            If ComboBox2.Text.ToString = "CD Quality" Then
                ComboBox1.SelectedIndex = 3
                TrackBar1.Value = 0
                ComboBox18.SelectedIndex = 0
                ComboBox33.SelectedIndex = 1
                ComboBox34.SelectedIndex = 0
            ElseIf ComboBox2.Text.ToString = "DVD Quality" Then
                ComboBox1.SelectedIndex = 4
                TrackBar1.Value = 0
                ComboBox18.SelectedIndex = 0
                ComboBox33.SelectedIndex = 1
                ComboBox34.SelectedIndex = 0
            ElseIf ComboBox2.Text.ToString = "Hi-Fi Quality" Then
                ComboBox1.SelectedIndex = 4
                TrackBar1.Value = 0
                ComboBox18.SelectedIndex = 1
                ComboBox33.SelectedIndex = 1
                ComboBox34.SelectedIndex = 0
            ElseIf ComboBox2.Text.ToString = "Hi-Res Quality" Then
                ComboBox1.SelectedIndex = 7
                TrackBar1.Value = 0
                ComboBox18.SelectedIndex = 1
                ComboBox33.SelectedIndex = 1
                ComboBox34.SelectedIndex = 0
            Else
                ComboBox1.SelectedIndex = -1
                TrackBar1.Value = 0
                ComboBox18.SelectedIndex = -1
                ComboBox33.SelectedIndex = -1
                ComboBox34.SelectedIndex = -1
            End If
            Label3.Text = "0%"
        ElseIf AudioTEMPFormatOpt = "Free Lossless Audio Codec (*.flac)" Then
            If ComboBox2.Text.ToString = "Low Quality" Then
                ComboBox1.SelectedIndex = 3
                TrackBar1.Value = 8
                ComboBox18.SelectedIndex = 0
                ComboBox33.SelectedIndex = 1
                ComboBox34.SelectedIndex = 0
                Label3.Text = "80%"
            ElseIf ComboBox2.Text.ToString = "Medium Quality" Then
                ComboBox1.SelectedIndex = 3
                TrackBar1.Value = 5
                ComboBox18.SelectedIndex = 0
                ComboBox33.SelectedIndex = 1
                ComboBox34.SelectedIndex = 0
                Label3.Text = "50%"
            ElseIf ComboBox2.Text = "High Quality" Then
                ComboBox1.SelectedIndex = 3
                TrackBar1.Value = 0
                ComboBox18.SelectedIndex = 0
                ComboBox33.SelectedIndex = 1
                ComboBox34.SelectedIndex = 0
                Label3.Text = "0%"
            ElseIf ComboBox2.Text.ToString = "Hi-Fi Quality" Then
                ComboBox1.SelectedIndex = 4
                TrackBar1.Value = 0
                ComboBox18.SelectedIndex = 1
                ComboBox33.SelectedIndex = 1
                ComboBox34.SelectedIndex = 0
                Label3.Text = "0%"
            ElseIf ComboBox2.Text.ToString = "Hi-Res Quality" Then
                ComboBox1.SelectedIndex = 7
                TrackBar1.Value = 0
                ComboBox18.SelectedIndex = 1
                ComboBox33.SelectedIndex = 1
                ComboBox34.SelectedIndex = 0
                Label3.Text = "0%"
            Else
                ComboBox1.SelectedIndex = -1
                TrackBar1.Value = 0
                ComboBox18.SelectedIndex = -1
                ComboBox33.SelectedIndex = -1
                ComboBox34.SelectedIndex = -1
                Label3.Text = "0%"
            End If
        Else
            If ComboBox2.Text.ToString = "Low Quality" Then
                ComboBox1.SelectedIndex = 3
                TrackBar1.Value = 4
                ComboBox18.SelectedIndex = 0
                ComboBox33.SelectedIndex = 1
                ComboBox34.SelectedIndex = 0
                Label3.Text = "80%"
            ElseIf ComboBox2.Text.ToString = "Medium Quality" Then
                ComboBox1.SelectedIndex = 3
                TrackBar1.Value = 2
                ComboBox18.SelectedIndex = 0
                ComboBox33.SelectedIndex = 1
                ComboBox34.SelectedIndex = 0
                Label3.Text = "40%"
            ElseIf ComboBox2.Text.ToString = "High Quality" Then
                ComboBox1.SelectedIndex = 3
                TrackBar1.Value = 0
                ComboBox18.SelectedIndex = 0
                ComboBox33.SelectedIndex = 1
                ComboBox34.SelectedIndex = 0
                Label3.Text = "0%"
            Else
                ComboBox1.SelectedIndex = -1
                TrackBar1.Value = 0
                ComboBox18.SelectedIndex = -1
                ComboBox33.SelectedIndex = -1
                ComboBox34.SelectedIndex = -1
                Label3.Text = "0%"
            End If
        End If
        audioTEMPProfile = ComboBox2.Text.ToString
    End Sub
    Private Sub SaveBtn(sender As Object, e As EventArgs) Handles Button2.Click
        If ComboBox1.SelectedIndex >= 0 Then
            AudioTEMPSmpRate = ComboBox1.Text.ToString
        End If
        AudioTEMPCnvRatio = TrackBar1.Value
        If ComboBox33.SelectedIndex >= 0 Then
            AudioTEMPChn = ComboBox33.Text.ToString
        End If
        If ComboBox34.SelectedIndex >= 0 Then
            AudioTEMPChnMapping = ComboBox34.Text.ToString
        End If
        If ComboBox18.SelectedIndex >= 0 Then
            AudioTEMPBitDepth = ComboBox18.Text.ToString
        End If
        Dim audioProfile = New AudioProfile
        audioProfile.TextBoxExt4.Text = aLibraryTrs(AudioTEMPFormatOpt, ComboBox18.Text.ToString) + ", " + (CInt(ComboBox1.Text.ToString) / 1000).ToString + " KHz, " + ComboBox18.Text.ToString + ", " + ComboBox34.Text.ToString + " channels"
        audioProfile.TextBoxExt5.Text = aLibraryTrs(AudioTEMPFormatOpt, ComboBox18.Text.ToString) + ", " + (CInt(ComboBox1.Text.ToString) / 1000).ToString + " KHz, " + ComboBox18.Text.ToString + ", " + ComboBox34.Text.ToString + " channels"
        audioProfile.TextBoxExt6.Text = "Container File Format   : " + AudioTEMPFormatOpt & vbCrLf +
                                        "Encoding Format" & vbTab + "    : " + aLibraryTrs(AudioTEMPFormatOpt, ComboBox18.Text.ToString) & vbCrLf +
                                        "Bitrate Mode" & vbTab + "    : " + AudioBitrateInfo(TrackBar1.Value) & vbCrLf +
                                        "Bitrate" & vbTab & vbTab + "    : " + AudioBitrateVal(AudioBitrateCalc(AudioTEMPFormatOpt, AudioTEMPCnvRatio), AudioTEMPFormatOpt) & vbCrLf +
                                        "Sample Rate" & vbTab + "    : " + (CInt(ComboBox1.Text.ToString) / 1000).ToString + " KHz "
        audioProfile.Show()
        Close()
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ComboBox1.SelectedIndex >= 0 Then
            AudioTEMPSmpRate = ComboBox1.Text.ToString
        End If
        AudioTEMPCnvRatio = TrackBar1.Value
        If ComboBox33.SelectedIndex >= 0 Then
            AudioTEMPChn = ComboBox33.Text.ToString
        End If
        If ComboBox34.SelectedIndex >= 0 Then
            AudioTEMPChnMapping = ComboBox34.Text.ToString
        End If
        If ComboBox18.SelectedIndex >= 0 Then
            AudioTEMPBitDepth = ComboBox18.Text.ToString
        End If
        Dim audioProfile = New AudioProfile
        audioProfile.Show()
        Close()
    End Sub
    Private Sub ConversionRatioTBR(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        If AudioTEMPFormatOpt = "Free Lossless Audio Codec (*.flac)" Then
            If TrackBar1.Value = 0 Then
                Label3.Text = "0%"
            ElseIf TrackBar1.Value = 1 Then
                Label3.Text = "10%"
            ElseIf TrackBar1.Value = 2 Then
                Label3.Text = "20%"
            ElseIf TrackBar1.Value = 3 Then
                Label3.Text = "30%"
            ElseIf TrackBar1.Value = 4 Then
                Label3.Text = "40%"
            ElseIf TrackBar1.Value = 5 Then
                Label3.Text = "50%"
            ElseIf TrackBar1.Value = 6 Then
                Label3.Text = "60%"
            ElseIf TrackBar1.Value = 7 Then
                Label3.Text = "70%"
            ElseIf TrackBar1.Value = 8 Then
                Label3.Text = "80%"
            ElseIf TrackBar1.Value = 9 Then
                Label3.Text = "90%"
            ElseIf TrackBar1.Value = 10 Then
                Label3.Text = "100%"
            End If
        Else
            If TrackBar1.Value = 0 Then
                Label3.Text = "0%"
            ElseIf TrackBar1.Value = 1 Then
                Label3.Text = "20%"
            ElseIf TrackBar1.Value = 2 Then
                Label3.Text = "40%"
            ElseIf TrackBar1.Value = 3 Then
                Label3.Text = "60%"
            ElseIf TrackBar1.Value = 4 Then
                Label3.Text = "80%"
            ElseIf TrackBar1.Value = 5 Then
                Label3.Text = "100%"
            End If
        End If
    End Sub
    Private Sub ChannelMappingBtn(sender As Object, e As EventArgs) Handles ComboBox33.SelectedIndexChanged
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
        If AudioTEMPFormatOpt IsNot "" Then
            If AudioTEMPFormatOpt = "Advanced Audio Coding (*.aac)" Then
                ComboBox1.Items.Clear()
                ComboBox1.Items.Add("8000")
                ComboBox1.Items.Add("16000")
                ComboBox1.Items.Add("32000")
                ComboBox1.Items.Add("44100")
                ComboBox1.Items.Add("48000")
                ComboBox1.Items.Add("64000")
                ComboBox1.Items.Add("88200")
                ComboBox1.Items.Add("96000")
            ElseIf AudioTEMPFormatOpt = "MP2 Audio (*.mp2)" Or AudioTEMPFormatOpt = "MP3 Audio (*.mp3)" Then
                ComboBox1.Items.Clear()
                ComboBox1.Items.Add("8000")
                ComboBox1.Items.Add("16000")
                ComboBox1.Items.Add("32000")
                ComboBox1.Items.Add("44100")
                ComboBox1.Items.Add("48000")
            ElseIf AudioTEMPFormatOpt = "Opus Audio (*.opus)" Then
                ComboBox1.Items.Clear()
                ComboBox1.Items.Add("8000")
                ComboBox1.Items.Add("16000")
                ComboBox1.Items.Add("48000")
            ElseIf AudioTEMPFormatOpt = "Free Lossless Audio Codec (*.flac)" Or AudioTEMPFormatOpt = "Wave PCM (*.wav)" Then
                ComboBox1.Items.Clear()
                ComboBox1.Items.Add("8000")
                ComboBox1.Items.Add("16000")
                ComboBox1.Items.Add("32000")
                ComboBox1.Items.Add("44100")
                ComboBox1.Items.Add("48000")
                ComboBox1.Items.Add("64000")
                ComboBox1.Items.Add("88200")
                ComboBox1.Items.Add("96000")
                ComboBox1.Items.Add("176400")
                ComboBox1.Items.Add("192000")
            End If
        End If
    End Sub
    Private Sub AudioBitDepthCheck()
        If AudioTEMPFormatOpt = "Free Lossless Audio Codec (*.flac)" Or AudioTEMPFormatOpt = "Wave PCM (*.wav)" Then
            ComboBox18.Items.Clear()
            If AudioTEMPFormatOpt = "Free Lossless Audio Codec (*.flac)" Then
                ComboBox18.Items.Add("16 Bit")
                ComboBox18.Items.Add("24 Bit")
            Else
                ComboBox18.Items.Add("16 Bit")
                ComboBox18.Items.Add("24 Bit")
                ComboBox18.Items.Add("32 Bit")
            End If
        Else
            ComboBox18.Items.Clear()
            ComboBox18.Items.Add("16 Bit")
        End If
    End Sub
End Class