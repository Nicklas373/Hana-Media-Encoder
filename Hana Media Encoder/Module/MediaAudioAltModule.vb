Module MediaAudioAltModule
    Public Function aCodecAlt(Cmbx As String, bdCmbx As String) As String
        'Combobox15.text & Combobox18.text'
        Dim value As String
        If Cmbx = "Copy" Then
            value = " --audio-copy "
        ElseIf Cmbx = "MP3" Or Cmbx = "MP3 Audio (*.mp3)" Then
            value = " --audio-codec libmp3lame"
        ElseIf Cmbx = "AAC" Or Cmbx = "Advanced Audio Coding (*.aac)" Then
            value = " --audio-codec aac"
        ElseIf Cmbx = "MP2" Or Cmbx = "MP2 Audio (*.mp2)" Then
            value = " --audio-codec libtwolame"
        ElseIf Cmbx = "OPUS" Or Cmbx = "Opus Audio (*.opus)" Then
            value = " --audio-codec libopus"
        ElseIf Cmbx = "FLAC" Or Cmbx = "Free Lossless Audio Codec (*.flac)" Then
            value = " --audio-codec flac"
        ElseIf Cmbx = "WAV" Or Cmbx = "Wave PCM (*.wav)" Then
            If bdCmbx = "16 Bit" Then
                value = " --audio-codec pcm_s16le"
            ElseIf bdCmbx = "24 Bit" Then
                value = " --audio-codec pcm_s24le"
            ElseIf bdCmbx = "32 Bit" Then
                value = " --audio-codec pcm_s32le"
            Else
                value = " --audio-codec pcm_s16le"
            End If
        Else
            value = ""
        End If

        Return value
    End Function
    Public Function aBitRateAlt(Cmbx As String) As String
        'Combobox19.text '
        Dim value As String
        If Cmbx = "" Then
            value = ""
        Else
            value = " --audio-bitrate " & Cmbx & " "
        End If

        Return value
    End Function
    Public Function aSampleRateAlt(Cmbx As String) As String
        'Combobox16.text '
        Dim value As String
        If Cmbx = "" Then
            value = ""
        Else
            value = " --audio-samplerate " & Cmbx & " "
        End If

        Return value
    End Function
End Module
