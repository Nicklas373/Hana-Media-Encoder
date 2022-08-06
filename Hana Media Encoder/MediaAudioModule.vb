Module MediaAudioModule
    Public Function aCodec(Cmbx As String, bdCmbx As String) As String
        'Combobox15.text & Combobox18.text'
        Dim value As String
        If Cmbx = "Copy" Then
            value = "copy"
        ElseIf Cmbx = "MP3" Then
            value = "libmp3lame"
        ElseIf Cmbx = "FLAC" Then
            value = "flac"
        ElseIf Cmbx = "WAV" Then
            If bdCmbx = "16 Bit" Then
                value = "pcm_s16le"
            ElseIf bdCmbx = "24 Bit" Then
                value = "pcm_s24le"
            ElseIf bdCmbx = "32 Bit" Then
                value = "pcm_s32le"
            Else
                value = ""
            End If
        Else
            value = "copy"
        End If

        Return value
    End Function
    Public Function aCodecReverse(codec As String) As String
        Dim reverse As String
        If codec = "pcm_s16le" Or codec = "pcm_s24le" Or codec = "pcm_s32le" Then
            reverse = "WAV"
        ElseIf codec = "libmp3lame" Then
            reverse = "MP3"
        ElseIf codec = "flac" Then
            reverse = "flac"
        ElseIf codec = "copy" Then
            reverse = "Copy"
        Else
            reverse = ""
        End If

        Return reverse
    End Function
    Public Function aBitDepth(Cmbx As String, bdCmbx As String) As String
        'Combobox15.text & Combobox18.text'
        Dim value As String
        If Cmbx = "Original Source" Or Cmbx = "MP3" Or Cmbx = "WAV" Then
            value = ""
        ElseIf Cmbx = "FLAC" Then
            If bdCmbx = "16 Bit" Then
                value = "s16"
            ElseIf bdCmbx = "24 Bit" Then
                value = "s32"
            Else
                value = ""
            End If
        Else
            value = ""
        End If

        Return value
    End Function
End Module