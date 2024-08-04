Module MediaAudioAltModule
    Public Function aCodecAlt(Cmbx As String, bdCmbx As String, aStream As String) As String
        'Combobox15.text & Combobox18.text'
        Dim value As String
        If Cmbx = "Copy" Then
            value = " --audio-copy " & aStream
        ElseIf Cmbx = "MP3" Or Cmbx = "MP3 Audio (*.mp3)" Then
            value = " --audio-codec " & aStream & "?" & "libmp3lame"
        ElseIf Cmbx = "AAC" Or Cmbx = "Advanced Audio Coding (*.aac)" Then
            value = " --audio-codec " & aStream & "?" & "aac"
        ElseIf Cmbx = "MP2" Or Cmbx = "MP2 Audio (*.mp2)" Then
            value = " --audio-codec " & aStream & "?" & "libtwolame"
        ElseIf Cmbx = "OPUS" Or Cmbx = "Opus Audio (*.opus)" Then
            value = " --audio-codec " & aStream & "?" & "libopus"
        ElseIf Cmbx = "FLAC" Or Cmbx = "Free Lossless Audio Codec (*.flac)" Then
            value = " --audio-codec " & aStream & "?" & "flac"
        ElseIf Cmbx = "WAV" Or Cmbx = "Wave PCM (*.wav)" Then
            If bdCmbx = "16 Bit" Then
                value = " --audio-codec " & aStream & "?" & "pcm_s16le"
            ElseIf bdCmbx = "24 Bit" Then
                value = " --audio-codec " & aStream & "?" & "pcm_s24le"
            ElseIf bdCmbx = "32 Bit" Then
                value = " --audio-codec " & aStream & "?" & "pcm_s32le"
            Else
                value = " --audio-codec " & aStream & "?" & "pcm_s16le"
            End If
        Else
            value = ""
        End If

        Return value
    End Function
    Public Function aBitRateAlt(Cmbx As String, aStream As String) As String
        'Combobox19.text '
        Dim value As String
        If String.IsNullOrEmpty(Cmbx) = True Then
            value = ""
        Else
            value = " --audio-bitrate " & aStream & "?" & Cmbx
        End If

        Return value
    End Function
    Public Function aChannelAlt(Cmbx As String, aStream As String) As String
        'Combobox33.text '
        Dim value As String
        If String.IsNullOrEmpty(Cmbx) = True Then
            value = ""
        Else
            If Cmbx = "2" Then
                value = " --audio-stream " & aStream & "?stereo"
            Else
                value = " --audio-stream " & aStream & "?" & Cmbx
            End If
        End If

        Return value
    End Function
    Public Function aSampleRateAlt(Cmbx As String, aStream As String) As String
        'Combobox16.text '
        Dim value As String
        If String.IsNullOrEmpty(Cmbx) = True Then
            value = ""
        Else
            value = " --audio-samplerate " & aStream & "?" & Cmbx
        End If

        Return value
    End Function
End Module
