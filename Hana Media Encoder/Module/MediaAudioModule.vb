Module MediaAudioModule
    Public Function aCodec(Cmbx As String, bdCmbx As String, aStream As String) As String
        'Combobox15.text & Combobox18.text'
        Dim value As String
        If Cmbx = "Copy" Then
            value = " -c:a:" & aStream & " copy"
        ElseIf Cmbx = "MP3" Then
            value = " -c:a:" & aStream & " libmp3lame"
        ElseIf Cmbx = "AAC" Then
            value = " -c:a:" & aStream & " aac"
        ElseIf Cmbx = "FLAC" Then
            value = " -c:a:" & aStream & " flac"
        ElseIf Cmbx = "WAV" Then
            If bdCmbx = "16 Bit" Then
                value = " -c:a:" & aStream & " pcm_s16le"
            ElseIf bdCmbx = "24 Bit" Then
                value = " -c:a:" & aStream & " pcm_s24le"
            ElseIf bdCmbx = "32 Bit" Then
                value = " -c:a:" & aStream & " pcm_s32le"
            Else
                value = ""
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
        If Cmbx = "Original Source" Or Cmbx = "MP3" Or Cmbx = "WAV" Then
            value = ""
        ElseIf Cmbx = "FLAC" Then
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
            If rateControl = "CBR" And aCodec = "MP3" Then
                value = " -b:a:" & aStream & " " & Cmbx & "k"
            ElseIf rateControl = "VBR" And aCodec = "MP3" Then
                value = " -q:a:" & aStream & " " & Cmbx
            ElseIf rateControl = "VBR" And aCodec = "AAC" Then
                value = " -vbr:a:" & aStream & " " & Cmbx
            ElseIf rateControl = "" And aCodec = "FLAC" Then
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