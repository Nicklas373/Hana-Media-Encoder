Module MediaAudioModule
    Public Function aCodec(Cmbx As String, bdCmbx As String, aStream As String) As String
        'Combobox15.text & Combobox18.text'
        Dim value As String
        If Cmbx = "Copy" Then
            value = " -c:a:" & aStream & " copy"
        ElseIf Cmbx = "MP3" Or Cmbx = "MP3 Audio (*.mp3)" Then
            value = " -c:a:" & aStream & " libmp3lame"
        ElseIf Cmbx = "AAC" Or Cmbx = "Advanced Audio Coding (*.aac)" Then
            value = " -c:a:" & aStream & " aac"
        ElseIf Cmbx = "MP2" Or Cmbx = "MP2 Audio (*.mp2)" Then
            value = " -c:a:" & aStream & " libtwolame"
        ElseIf Cmbx = "OPUS" Or Cmbx = "Opus Audio (*.opus)" Then
            value = " -c:a:" & aStream & " libopus"
        ElseIf Cmbx = "FLAC" Or Cmbx = "Free Lossless Audio Codec (*.flac)" Then
            value = " -c:a:" & aStream & " flac"
        ElseIf Cmbx = "WAV" Or Cmbx = "Wave PCM (*.wav)" Then
            If bdCmbx = "16 Bit" Then
                value = " -c:a:" & aStream & " pcm_s16le"
            ElseIf bdCmbx = "24 Bit" Then
                value = " -c:a:" & aStream & " pcm_s24le"
            ElseIf bdCmbx = "32 Bit" Then
                value = " -c:a:" & aStream & " pcm_s32le"
            Else
                value = " -c:a:" & aStream & " pcm_s16le"
            End If
        Else
            value = ""
        End If

        Return value
    End Function
    Public Function aCodecReverse(codec As String) As String
        Dim reverse As String
        If codec = "pcm_s16le" Or codec = "pcm_s24le" Or codec = "pcm_s32le" Then
            reverse = "WAV"
        ElseIf codec = "libmp3lame" Then
            reverse = "MP3"
        ElseIf codec = "aac" Then
            reverse = "AAC"
        ElseIf codec = "mp2" Then
            reverse = "MP2"
        ElseIf codec = "opus" Then
            reverse = "OPUS"
        ElseIf codec = "flac" Then
            reverse = "FLAC"
        ElseIf codec = "copy" Then
            reverse = "Copy"
        Else
            reverse = ""
        End If

        Return reverse
    End Function
    Public Function aBitDepth(Cmbx As String, aStream As String, bdCmbx As String) As String
        'Combobox15.text & Combobox18.text'
        Dim value As String
        If Cmbx = "Original Source" Or Cmbx = "MP3" Or Cmbx = "MP3 Audio (*.mp3)" Or Cmbx = "WAV" Or Cmbx = "Wave PCM (*.wav)" Then
            value = ""
        ElseIf Cmbx = "FLAC" Or Cmbx = "Free Lossless Audio Codec (*.flac)" Then
            If bdCmbx = "16 Bit" Then
                value = " -sample_fmt:a:" & aStream & " s16"
            ElseIf bdCmbx = "24 Bit" Then
                value = " -sample_fmt:a:" & aStream & " s32"
            Else
                value = ""
            End If
        Else
            value = ""
        End If

        Return value
    End Function
    Public Function aBitRate(Cmbx As String, aStream As String, aCodec As String, rateControl As String) As String
        'Combobox19.text '
        Dim value As String
        If Cmbx = "" Then
            value = ""
        Else
            If rateControl = "CBR" And aCodec = "MP3" Or aCodec = "MP3 Audio (*.mp3)" Then
                value = " -b:a:" & aStream & " " & Cmbx & "k"
            ElseIf rateControl = "CBR" And aCodec = "OPUS" Or aCodec = "Opus Audio (*.opus)" Then
                value = " -b:a:" & aStream & " " & Cmbx & "k -vbr off -application audio"
            ElseIf rateControl = "VBR" And aCodec = "MP3" Or aCodec = "MP3 Audio (*.mp3)" Then
                value = " -q:a:" & aStream & " " & Cmbx
            ElseIf rateControl = "VBR" And aCodec = "OPUS" Or aCodec = "Opus Audio (*.opus)" Then
                value = " -b:a:" & aStream & " " & Cmbx & "k -vbr on -application audio"
            ElseIf rateControl = "" And aCodec = "FLAC" Or aCodec = "Free Lossless Audio Codec (*.flac)" Then
                value = " -compression_level:a:" & aStream & " " & Cmbx
            Else
                value = ""
            End If
        End If

        Return value
    End Function
    Public Function aChannel(Cmbx As String, aStream As String) As String
        'Combobox33.text '
        Dim value As String
        If Cmbx = "" Then
            value = ""
        Else
            value = " -ac:a:" & aStream & " " & Cmbx
        End If

        Return value
    End Function
    Public Function aSampleFormat(Cmbx As String, aStream As String) As String
        'Combobox15.text '
        Dim value As String
        If Cmbx = "" Then
            value = ""
        Else
            value = " -sample_fmt:a:" & aStream & " " & Cmbx
        End If

        Return value
    End Function
    Public Function aSampleRate(Cmbx As String, aStream As String) As String
        'Combobox16.text '
        Dim value As String
        If Cmbx = "" Then
            value = ""
        Else
            value = " -ar:a:" & aStream & " " & Cmbx
        End If

        Return value
    End Function
End Module