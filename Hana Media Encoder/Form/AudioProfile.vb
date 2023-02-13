Imports Syncfusion.WinForms.Controls

Public Class AudioProfile
    Inherits SfForm
    Dim audioSavePath As New FolderBrowserDialog

    Private Sub AudioProfile_load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        AllowRoundedCorners = True
        StyleManager1.SetTheme(HMESetTheme.ToString)
        MetroSetRadioButton1.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetRadioButton1.BorderColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetRadioButton2.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetRadioButton2.BorderColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetCheckBox1.ForeColor = ColorTranslator.FromHtml("#F4A950")
        If AudioTEMPFileNameOpt IsNot "" Then
            If AudioTEMPFileNameOpt = MetroSetRadioButton1.Text Then
                MetroSetRadioButton1.Checked = True
            ElseIf AudioTEMPFileNameOpt = MetroSetRadioButton2.Text Then
                MetroSetRadioButton2.Checked = True
            End If
        End If
        If AudioTEMPPreValue IsNot "" Then
            TextBoxExt1.Text = AudioTEMPPreValue
        End If
        If AudioTEMPPostValue IsNot "" Then
            TextBoxExt2.Text = AudioTEMPPostValue
        End If
        If AudioTEMPFolderLocation IsNot "" Then
            TextBoxExt3.Text = AudioTEMPFolderLocation
        End If
        If AudioTEMPFormatOpt IsNot "" Then
            ComboBox1.Text = AudioTEMPFormatOpt
        Else
            ComboBox1.SelectedIndex = -1
        End If
    End Sub
    Private Sub AudioSampleType_Btn(sender As Object, e As EventArgs) Handles Button1.Click
        If MetroSetRadioButton1.Checked = True Then
            AudioTEMPFileNameOpt = MetroSetRadioButton1.Text
        Else
            If MetroSetRadioButton2.Checked = True Then
                AudioTEMPFileNameOpt = MetroSetRadioButton2.Text
            Else
                AudioTEMPFileNameOpt = ""
            End If
        End If
        If TextBoxExt1.Text IsNot "" Then
            AudioTEMPPreValue = TextBoxExt1.Text
        Else
            AudioTEMPPreValue = ""
        End If
        If TextBoxExt2.Text IsNot "" Then
            AudioTEMPPostValue = TextBoxExt2.Text
        Else
            AudioTEMPPostValue = ""
        End If
        If TextBoxExt3.Text IsNot "" Then
            AudioTEMPFolderLocation = TextBoxExt3.Text
        Else
            AudioTEMPFolderLocation = ""
        End If
        If ComboBox1.SelectedIndex >= 0 Then
            AudioTEMPFormatOpt = ComboBox1.Text
        End If
        Me.Close()
        Dim audioSampleType = New AudioSampleType
        audioSampleType.Show()
    End Sub
    Private Sub PreorPostFileNameRB(sender As Object) Handles MetroSetRadioButton1.CheckedChanged
        If MetroSetRadioButton1.Checked = True Then
            TextBoxExt1.Enabled = True
            TextBoxExt2.Enabled = True
        Else
            TextBoxExt1.Enabled = False
            TextBoxExt2.Enabled = False
        End If
    End Sub
    Private Sub AudioFolderPath_Btn(sender As Object, e As EventArgs) Handles Button6.Click
        If audioSavePath.ShowDialog() = DialogResult.OK Then
            TextBoxExt3.Text = audioSavePath.SelectedPath
        End If
    End Sub
    Private Sub SameFolderLocation_Btn(sender As Object) Handles MetroSetCheckBox1.CheckedChanged
        If MetroSetCheckBox1.Checked = True Then
            TextBoxExt3.Text = AudioQueueOrigDir
            AudioTEMPChkDir = "True"
        Else
            AudioTEMPChkDir = "False"
        End If
    End Sub
    Private Sub SaveProfile_Btn(sender As Object, e As EventArgs) Handles Button2.Click
        Dim channel_layout As String
        If AudioTEMPChnMapping = "" Then
            channel_layout = ""
        Else
            channel_layout = " -filter:a:0 aformat=channel_layouts=" & AudioTEMPChnMapping
        End If
        If ComboBox1.Text.ToString IsNot "" And AudioTEMPFileNameOpt IsNot "" And AudioTEMPFolderLocation IsNot "" Then
            If ComboBox1.Text.ToString = "MP3 Audio (*.mp3)" Or ComboBox1.Text.ToString = "Advanced Audio Coding (*.aac)" Or
                    ComboBox1.Text.ToString = "MP2 Audio (*.mp2)" Or ComboBox1.Text.ToString = "Opus Audio (*.opus)" Then
                AudioTEMPQuickFlags = aCodec(ComboBox1.Text.ToString, "", "0") + aChannel(AudioTEMPChn, "0") + aBitRate(AudioBitrateCalc(ComboBox1.Text.ToString, AudioTEMPCnvRatio), "0", ComboBox1.Text.ToString, "CBR") +
                        aSampleRate(AudioTEMPSmpRate, "0")
            ElseIf AudioTEMPFormatOpt = "Free Lossless Audio Codec (*.flac)" Then
                AudioTEMPQuickFlags = aCodec(ComboBox1.Text.ToString, AudioTEMPBitDepth, "0") + aChannel(AudioTEMPChn, "0") + aBitRate(AudioBitrateCalc(ComboBox1.Text.ToString, AudioTEMPCnvRatio), "0", "FLAC", "") +
                        aSampleRate(AudioTEMPSmpRate, "0") + aBitDepth("FLAC", "0", AudioTEMPBitDepth) + channel_layout
            ElseIf AudioTEMPFormatOpt = "Wave PCM (*.wav)" Then
                AudioTEMPQuickFlags = aCodec(ComboBox1.Text.ToString, AudioTEMPBitDepth, "0") + aChannel(AudioTEMPChn, "0") + aSampleRate(AudioTEMPSmpRate, "0") + channel_layout
            Else
                AudioTEMPQuickFlags = "null"
            End If
        End If
        For Each item As ListViewItem In MainMenu.ListView2.Items
            item.SubItems(3).Text += AudioTEMPQuickFlags
        Next
        AudioTEMPNewSmpType = TextBoxExt4.Text
        MainMenu.ListView2.Update()
        MainMenu.ListView2.Refresh()
        MainMenu.TextBox1.Text = TextBoxExt3.Text
        MainMenu.Update()
        MainMenu.Refresh()
        Me.Close()
    End Sub
    Private Sub Cancel_Btn(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub
    Private Function AudioBitrateCalc(audioFormat As String, audioCompLvl As Integer) As String
        Dim bitrateval As String = ""
        If audioCompLvl = 0 Then
            If audioFormat = "MP3 Audio (*.mp3)" Then
                bitrateval = "320"
            ElseIf audioFormat = "Advanced Audio Coding (*.aac)" Then
                bitrateval = "512"
            ElseIf audioFormat = "MP2 Audio (*.mp2)" Then
                bitrateval = "384"
            ElseIf audioFormat = "Opus Audio (*.opus)" Then
                bitrateval = "510"
            ElseIf audioFormat = "Free Lossless Audio Codec (*.flac)" Then
                bitrateval = "0"
            End If
        ElseIf audioCompLvl = 1 Then
            If audioFormat = "MP3 Audio (*.mp3)" Then
                bitrateval = "256"
            ElseIf audioFormat = "Advanced Audio Coding (*.aac)" Then
                bitrateval = "384"
            ElseIf audioFormat = "MP2 Audio (*.mp2)" Then
                bitrateval = "256"
            ElseIf audioFormat = "Opus Audio (*.opus)" Then
                bitrateval = "192"
            ElseIf audioFormat = "Free Lossless Audio Codec (*.flac)" Then
                bitrateval = "1"
            End If
        ElseIf audioCompLvl = 2 Then
            If audioFormat = "MP3 Audio (*.mp3)" Then
                bitrateval = "192"
            ElseIf audioFormat = "Advanced Audio Coding (*.aac)" Then
                bitrateval = "256"
            ElseIf audioFormat = "MP2 Audio (*.mp2)" Then
                bitrateval = "192"
            ElseIf audioFormat = "Opus Audio (*.opus)" Then
                bitrateval = "160"
            ElseIf audioFormat = "Free Lossless Audio Codec (*.flac)" Then
                bitrateval = "2"
            End If
        ElseIf audioCompLvl = 3 Then
            If audioFormat = "MP3 Audio (*.mp3)" Then
                bitrateval = "160"
            ElseIf audioFormat = "Advanced Audio Coding (*.aac)" Then
                bitrateval = "192"
            ElseIf audioFormat = "MP2 Audio (*.mp2)" Then
                bitrateval = "128"
            ElseIf audioFormat = "Opus Audio (*.opus)" Then
                bitrateval = "96"
            ElseIf audioFormat = "Free Lossless Audio Codec (*.flac)" Then
                bitrateval = "3"
            End If
        ElseIf audioCompLvl = 4 Then
            If audioFormat = "MP3 Audio (*.mp3)" Then
                bitrateval = "128"
            ElseIf audioFormat = "Advanced Audio Coding (*.aac)" Then
                bitrateval = "128"
            ElseIf audioFormat = "MP2 Audio (*.mp2)" Then
                bitrateval = "128"
            ElseIf audioFormat = "Opus Audio (*.opus)" Then
                bitrateval = "96"
            ElseIf audioFormat = "Free Lossless Audio Codec (*.flac)" Then
                bitrateval = "4"
            End If
        ElseIf audioCompLvl = 5 Then
            bitrateval = "5"
        ElseIf audioCompLvl = 6 Then
            bitrateval = "6"
        ElseIf audioCompLvl = 7 Then
            bitrateval = "7"
        ElseIf audioCompLvl = 8 Then
            bitrateval = "8"
        ElseIf audioCompLvl = 9 Then
            bitrateval = "9"
        ElseIf audioCompLvl = 10 Then
            bitrateval = "10"
        End If

        Return bitrateval
    End Function
End Class