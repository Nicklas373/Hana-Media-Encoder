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
        If codec = "pcm_s16le" Or codec = "pcm_s24le" Or codec = "pcm_s32le" Or codec = "WAV" Then
            reverse = "WAV"
        ElseIf codec = "libmp3lame" Or codec = "MP3" Then
            reverse = "MP3"
        ElseIf codec = "aac" Or codec = "AAC" Then
            reverse = "AAC"
        ElseIf codec = "mp2" Or codec = "libtwolame" Or codec = "MP2" Then
            reverse = "MP2"
        ElseIf codec = "opus" Or codec = "libopus" Or codec = "OPUS" Then
            reverse = "OPUS"
        ElseIf codec = "flac" Or codec = "FLAC" Then
            reverse = "FLAC"
        ElseIf codec = "copy" Or codec = "Copy" Then
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
    Public Function aLibraryTrs(aCodec As String, aBd As String) As String
        Dim value As String
        If aCodec = "MP3" Or aCodec = "MP3 Audio (*.mp3)" Then
            value = "libmp3lame"
        ElseIf aCodec = "AAC" Or aCodec = "Advanced Audio Coding (*.aac)" Then
            value = "aac"
        ElseIf aCodec = "MP2" Or aCodec = "MP2 Audio (*.mp2)" Then
            value = "libtwolame"
        ElseIf aCodec = "OPUS" Or aCodec = "Opus Audio (*.opus)" Then
            value = "libopus"
        ElseIf aCodec = "FLAC" Or aCodec = "Free Lossless Audio Codec (*.flac)" Then
            value = "flac"
        ElseIf aCodec = "WAV" Or aCodec = "Wave PCM (*.wav)" Then
            If aBd = "16 Bit" Then
                value = "pcm_s16le"
            ElseIf aBd = "24 Bit" Then
                value = "pcm_s24le"
            ElseIf aBd = "32 Bit" Then
                value = "pcm_s32le"
            Else
                value = "pcm_s16le"
            End If
        ElseIf aCodec = "Copy" Or aCodec = "COPY" Then
            value = "copy"
        Else
            value = ""
        End If

        Return value
    End Function
    Public Function AudioBitrateInfo(audioCompLvl As Integer) As String
        Dim bitrateinf As String = ""
        If audioCompLvl < 5 Then
            bitrateinf = "Constant Bit Rate"
        ElseIf audioCompLvl >= 5 Then
            bitrateinf = "Variable Bit Rate"
        End If

        Return bitrateinf
    End Function
    Public Function AudioBitrateVal(audioBitRate As String, audioCodec As String) As String
        Dim bitrateval As String = ""
        If CInt(RemoveWhitespace(audioBitRate)) > 10 Then
            bitrateval = audioBitRate + " kb/s"
        ElseIf CInt(RemoveWhitespace(audioBitRate)) <= 10 Then
            If audioCodec = "Free Lossless Audio Codec (*.flac)" Then
                bitrateval = "Compression with level " + audioBitRate
            Else
                bitrateval = audioBitRate + " kb/s"
            End If
        End If

        Return bitrateval
    End Function
    Public Function AudioBitrateCalc(audioFormat As String, audioCompLvl As Integer) As String
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
End Module