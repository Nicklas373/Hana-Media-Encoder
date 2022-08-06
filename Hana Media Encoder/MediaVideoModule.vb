Module MediaVideoModule
    Public Function bRefMode(Cmbx As String) As String
        'Combobox10.text'
        Dim value As String
        If Cmbx = "disabled" Then
            value = "0"
        ElseIf Cmbx = "each" Then
            value = "1"
        ElseIf Cmbx = "middle" Then
            value = "2"
        Else
            value = "0"
        End If

        Return value
    End Function
    Public Function vCodec(Cmbx As String, Encoder As String) As String
        'Combobox2.text'
        Dim value As String
        If Cmbx = "Copy" Then
            value = "copy"
        ElseIf Cmbx = "H264" Then
            If Encoder = "opencl" Then
                value = "h264_amf"
            ElseIf Encoder = "qsv" Then
                value = "h264_qsv"
            ElseIf Encoder = "cuda" Then
                value = "h264_nvenc"
            Else
                value = "copy"
            End If
        ElseIf Cmbx = "HEVC" Then
            If Encoder = "opencl" Then
                value = "hevc_amf"
            ElseIf Encoder = "qsv" Then
                value = "hevc_qsv"
            ElseIf Encoder = "cuda" Then
                value = "hevc_nvenc"
            Else
                value = "copy"
            End If
        Else
            value = "copy"
        End If

        Return value
    End Function
    Public Function vLevel(Cmbx As String) As String
        'Combobox8.text'
        Dim value As String
        If Cmbx = "" Then
            value = "auto"
        Else
            value = Cmbx
        End If

        Return value
    End Function
    Public Function multiPass(Cmbx As String) As String
        'Combobox14.text'
        Dim value As String
        If Cmbx = "disabled" Then
            value = "0"
        ElseIf Cmbx = "qres" Then
            value = "1"
        ElseIf Cmbx = "fullres" Then
            value = "2"
        Else
            value = "0"
        End If

        Return value
    End Function
    Public Function vPreset(Cmbx As String) As String
        'Combobox5.text'
        Dim value As String
        If Cmbx = "" Then
            value = "default"
        Else
            value = Cmbx
        End If

        Return value
    End Function
    Public Function vPresetAmf(Cmbx As String) As String
        'Combobox5.text'
        Dim value As String
        If Cmbx = "" Then
            value = "balanced"
        ElseIf Cmbx = "default" Then
            value = "balanced"
        ElseIf Cmbx = "slow" Then
            value = "quality"
        ElseIf Cmbx = "medium" Then
            value = "balanced"
        ElseIf Cmbx = "fast" Then
            value = "speed"
        Else
            value = "balanced"
        End If

        Return value
    End Function
    Public Function vPixFmt(Cmbx As String) As String
        'Combobox3.text'
        Dim value As String
        If Cmbx = "" Then
            value = "yuv420p"
        Else
            value = Cmbx
        End If

        Return value
    End Function
    Public Function vProfile(Cmbx As String) As String
        'Combobox7.text'
        Dim value As String
        If Cmbx = "" Then
            value = "main"
        Else
            value = Cmbx
        End If

        Return value
    End Function
    Public Function vRateControl(Cmbx As String) As String
        'Combobox4.text'
        Dim value As String
        If Cmbx = "" Then
            value = "vbr"
        ElseIf Cmbx = "VBR" Then
            value = "vbr"
        ElseIf Cmbx = "CBR" Then
            value = "cbr"
        Else
            value = "vbr"
        End If

        Return value
    End Function
    Public Function vSpaTempAQ(Cmbx As String) As String
        'Combobox11&13.text'
        Dim value As String
        If Cmbx = "disabled" Then
            value = "0"
        ElseIf Cmbx = "enabled" Then
            value = "1"
        Else
            value = "0"
        End If

        Return value
    End Function
    Public Function vAQStrength(Cmbx As String) As String
        'Combobox12.text'
        Dim value As String
        If Cmbx = "" Then
            value = "8"
        Else
            value = Cmbx
        End If

        Return value
    End Function
    Public Function vTune(Cmbx As String) As String
        'Combobox6.text'
        Dim value As String
        If Cmbx = "High quality" Then
            value = "hq"
        ElseIf Cmbx = "Low latency" Then
            value = "ll"
        ElseIf Cmbx = "Ultra low latency" Then
            value = "ull"
        ElseIf Cmbx = "Lossless" Then
            value = "lossless"
        Else
            value = "hq"
        End If

        Return value
    End Function
    Public Function vTier(Cmbx As String) As String
        'Combobox9.text'
        Dim value As String
        If Cmbx = "" Then
            value = "main"
        Else
            value = Cmbx
        End If

        Return value
    End Function
    Public Function vBrcompat(cmbx As String) As String
        'ComboBox21.text'
        Dim value As String
        If cmbx = "enable" Then
            value = "true"
        ElseIf cmbx = "disable" Then
            value = "false"
        Else
            value = "false"
        End If

        Return value
    End Function
End Module