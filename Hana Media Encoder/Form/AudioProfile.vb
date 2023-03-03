Imports Syncfusion.WinForms.Controls

Public Class AudioProfile
    Inherits SfForm
    Dim audioSavePath As New FolderBrowserDialog
    Dim allowSave As Boolean = True
    Private Sub AudioProfile_load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        AllowRoundedCorners = True
        StyleManager1.SetTheme(HMESetTheme.ToString)
        MetroSetRadioButton1.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetRadioButton1.BorderColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetRadioButton2.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetRadioButton2.BorderColor = ColorTranslator.FromHtml("#DBDBDB")
        MetroSetCheckBox1.ForeColor = ColorTranslator.FromHtml("#F4A950")
        If AudioTEMPFileNameOpt IsNot "null" Then
            If AudioTEMPFileNameOpt = MetroSetRadioButton1.Text Then
                MetroSetRadioButton1.Checked = True
            ElseIf AudioTEMPFileNameOpt = MetroSetRadioButton2.Text Then
                MetroSetRadioButton2.Checked = True
            End If
        End If
        If AudioTEMPPreValue IsNot "null" Then
            TextBoxExt1.Text = AudioTEMPPreValue
        End If
        If AudioTEMPPostValue IsNot "null" Then
            TextBoxExt2.Text = AudioTEMPPostValue
        End If
        If MediaTEMPFolderLocation IsNot "null" Then
            TextBoxExt3.Text = MediaTEMPFolderLocation
        End If
        If AudioTEMPFormatOpt IsNot "null" Then
            ComboBox1.Text = AudioTEMPFormatOpt
        Else
            ComboBox1.SelectedIndex = -1
        End If
    End Sub
    Private Sub AudioSampleType_Btn(sender As Object, e As EventArgs) Handles Button1.Click
        If MetroSetRadioButton1.Checked = True Then
            AudioTEMPFileNameOpt = MetroSetRadioButton1.Text
            If TextBoxExt1.Text IsNot "null" Then
                AudioTEMPPreValue = TextBoxExt1.Text
            Else
                AudioTEMPPreValue = "null"
            End If
            If TextBoxExt2.Text IsNot "null" Then
                AudioTEMPPostValue = TextBoxExt2.Text
            Else
                AudioTEMPPostValue = "null"
            End If
        Else
            If MetroSetRadioButton2.Checked = True Then
                AudioTEMPFileNameOpt = MetroSetRadioButton2.Text
            Else
                AudioTEMPFileNameOpt = "null"
                allowSave = False
            End If
        End If
        If TextBoxExt3.Text IsNot "null" Then
            MediaTEMPFolderLocation = TextBoxExt3.Text
        Else
            MediaTEMPFolderLocation = "null"
            allowSave = False
        End If
        If ComboBox1.SelectedIndex >= 0 Then
            AudioTEMPFormatOpt = ComboBox1.Text
        Else
            allowSave = False
        End If
        Dim audioSampleType = New AudioSampleType
        audioSampleType.Show()
        Me.Close()
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
            TextBoxExt3.Text = MediaQueueOrigDir(0)
            AudioTEMPChkDir = "True"
        Else
            AudioTEMPChkDir = "False"
        End If
    End Sub
    Private Sub SaveProfile_Btn(sender As Object, e As EventArgs) Handles Button2.Click
        Dim channel_layout As String
        If MetroSetRadioButton1.Checked = True Then
            AudioTEMPFileNameOpt = MetroSetRadioButton1.Text
            If TextBoxExt1.Text IsNot "" Then
                AudioTEMPPreValue = TextBoxExt1.Text
            Else
                AudioTEMPPreValue = "null"
            End If
            If TextBoxExt2.Text IsNot "" Then
                AudioTEMPPostValue = TextBoxExt2.Text
            Else
                AudioTEMPPostValue = "null"
            End If
        Else
            If MetroSetRadioButton2.Checked = True Then
                AudioTEMPFileNameOpt = MetroSetRadioButton2.Text
            Else
                AudioTEMPFileNameOpt = "null"
                allowSave = False
            End If
        End If
        If TextBoxExt3.Text IsNot "" Then
            MediaTEMPFolderLocation = TextBoxExt3.Text
        Else
            MediaTEMPFolderLocation = "null"
            allowSave = False
        End If
        If ComboBox1.SelectedIndex >= 0 Then
            AudioTEMPFormatOpt = ComboBox1.Text
        Else
            allowSave = False
        End If
        If AudioTEMPChnMapping = "" Then
            channel_layout = ""
        Else
            channel_layout = " -filter:a:0 aformat=channel_layouts=" & AudioTEMPChnMapping
        End If
        If allowSave Then
            If ComboBox1.Text.ToString IsNot "" And AudioTEMPFileNameOpt IsNot "" And MediaTEMPFolderLocation IsNot "" Then
                If ComboBox1.Text.ToString = "MP3 Audio (*.mp3)" Or ComboBox1.Text.ToString = "Advanced Audio Coding (*.aac)" Or
                        ComboBox1.Text.ToString = "MP2 Audio (*.mp2)" Or ComboBox1.Text.ToString = "Opus Audio (*.opus)" Then
                    Dim lossy As String
                    If ComboBox1.Text.ToString = "Opus Audio (*.opus)" Then
                        lossy = "OPUS"
                    Else
                        lossy = "MP3"
                    End If
                    AudioTEMPQuickFlags = aCodec(ComboBox1.Text.ToString, "", "0") + aChannel(AudioTEMPChn, "0") + aBitRate(AudioBitrateCalc(ComboBox1.Text.ToString, AudioTEMPCnvRatio), "0", lossy, "CBR") +
                            aSampleRate(AudioTEMPSmpRate, "0") + channel_layout
                ElseIf AudioTEMPFormatOpt = "Free Lossless Audio Codec (*.flac)" Then
                    AudioTEMPQuickFlags = aCodec(ComboBox1.Text.ToString, AudioTEMPBitDepth, "0") + aChannel(AudioTEMPChn, "0") + aBitRate(AudioBitrateCalc(ComboBox1.Text.ToString, AudioTEMPCnvRatio), "0", "FLAC", "") +
                            aSampleRate(AudioTEMPSmpRate, "0") + aBitDepth("FLAC", "0", AudioTEMPBitDepth) + channel_layout
                ElseIf AudioTEMPFormatOpt = "Wave PCM (*.wav)" Then
                    AudioTEMPQuickFlags = aCodec(ComboBox1.Text.ToString, AudioTEMPBitDepth, "0") + aChannel(AudioTEMPChn, "0") + aSampleRate(AudioTEMPSmpRate, "0") + channel_layout
                Else
                    AudioTEMPQuickFlags = ""
                End If
            End If
            AudioTEMPNewSmpType = TextBoxExt4.Text
            ContextMenuStrip1.Show(Button2.PointToScreen(New Point(0, Button2.Height)))
        Else
            NotifyIcon("Hana Media Encoder", "Please fill all option before save quick profile !", 1000, False)
        End If
    End Sub
    Private Sub contextmenustrip_submenu1_onclick(sender As Object, e As EventArgs) Handles SelectedFilesToolStripMenuItem.Click
        For i As Integer = 0 To MainMenu.DataGridView1.Rows.Count - 1
            If MainMenu.DataGridView1.Rows(i).Cells(0).Value = True Then
                If MainMenu.DataGridView1.Rows(i).Cells(0).Value = True And MainMenu.DataGridView1.Rows(i).Cells(5).Value.ToString = "Audio File" Then
                    If MainMenu.DataGridView1.Rows(i).Cells(8).Value.ToString.Contains("Video") = True Then
                        Dim tempVal As String = MainMenu.DataGridView1.Rows(i).Cells(8).Value.ToString
                        Dim tempVal2 As String = MainMenu.DataGridView1.Rows(i).Cells(10).Value.ToString
                        If getBetween(MainMenu.DataGridView1.Rows(i).Cells(8).Value.ToString, "Video:", "Audio:") = "" Then
                            MainMenu.DataGridView1.Rows(i).Cells(8).Value = tempVal + ", Audio: " + TextBoxExt5.Text
                        Else
                            MainMenu.DataGridView1.Rows(i).Cells(8).Value = "Video: " + getBetween(tempVal, "Video:", "Audio:") + ", Audio:" + TextBoxExt5.Text
                        End If
                        If getBetween(MainMenu.DataGridView1.Rows(i).Cells(10).Value.ToString, "-c:v", "-c:a") = "" Then
                            MainMenu.DataGridView1.Rows(i).Cells(10).Value = tempVal2 + " " + AudioTEMPQuickFlags
                        Else
                            MainMenu.DataGridView1.Rows(i).Cells(10).Value = "-c:v" + getBetween(tempVal2, "-c:v", "-c:a") + " " + AudioTEMPQuickFlags
                        End If
                    Else
                        MainMenu.DataGridView1.Rows(i).Cells(8).Value = "Audio: " + TextBoxExt5.Text
                        MainMenu.DataGridView1.Rows(i).Cells(10).Value = AudioTEMPQuickFlags
                    End If
                End If
            End If
        Next
        MainMenu.DataGridView1.Update()
        MainMenu.DataGridView1.Refresh()
        MainMenu.TextBox1.Text = TextBoxExt3.Text
        MainMenu.Update()
        MainMenu.Refresh()
        Me.Close()
    End Sub
    Private Sub contextmenustrip_submenu2_onclick(sender As Object, e As EventArgs) Handles AllFilesToolStripMenuItem.Click
        For i As Integer = 0 To MainMenu.DataGridView1.Rows.Count - 1
            If MainMenu.DataGridView1.Rows(i).Cells(5).Value.ToString = "Audio File" Then
                If MainMenu.DataGridView1.Rows(i).Cells(8).Value.ToString.Contains("Video") = True Then
                    Dim tempVal As String = MainMenu.DataGridView1.Rows(i).Cells(8).Value.ToString
                    Dim tempVal2 As String = MainMenu.DataGridView1.Rows(i).Cells(10).Value.ToString
                    If getBetween(MainMenu.DataGridView1.Rows(i).Cells(8).Value.ToString, "Video:", "Audio:") = "" Then
                        MainMenu.DataGridView1.Rows(i).Cells(8).Value = tempVal + ", Audio: " + TextBoxExt5.ToString
                    Else
                        MainMenu.DataGridView1.Rows(i).Cells(8).Value = "Video: " + getBetween(tempVal, "Video:", "Audio:") + ", Audio: " + TextBoxExt5.Text
                    End If
                    If getBetween(MainMenu.DataGridView1.Rows(i).Cells(10).Value.ToString, "-c:v", "-c:a") = "" Then
                        MainMenu.DataGridView1.Rows(i).Cells(10).Value = tempVal2 + " " + AudioTEMPQuickFlags
                    Else
                        MainMenu.DataGridView1.Rows(i).Cells(10).Value = "-c:v" + getBetween(tempVal2, "-c:v", "-c:a") + " " + AudioTEMPQuickFlags
                    End If
                Else
                    MainMenu.DataGridView1.Rows(i).Cells(8).Value = "Audio: " + TextBoxExt5.Text
                    MainMenu.DataGridView1.Rows(i).Cells(10).Value = AudioTEMPQuickFlags
                End If
            End If
        Next
        MainMenu.DataGridView1.Update()
        MainMenu.DataGridView1.Refresh()
        MainMenu.TextBox1.Text = TextBoxExt3.Text
        MainMenu.Update()
        MainMenu.Refresh()
        Me.Close()
    End Sub
    Private Sub Cancel_Btn(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub
End Class