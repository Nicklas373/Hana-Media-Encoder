Module MediaVideoModule
    Public Function bRefMode(Cmbx As String) As String
        'Combobox10.text'
        Dim value As String
        If Cmbx = "disabled" Then
            value = " -b_ref_mode 0 "
        ElseIf Cmbx = "each" Then
            value = " -b_ref_mode 1 "
        ElseIf Cmbx = "middle" Then
            value = " -b_ref_mode 2 "
        Else
            value = " "
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
            value = " "
        Else
            value = " -level " & Cmbx
        End If

        Return value
    End Function
    Public Function multiPass(Cmbx As String) As String
        'Combobox14.text'
        Dim value As String
        If Cmbx = "1 Pass" Then
            value = " -multipass 0 "
        ElseIf Cmbx = "2 Pass (1/4 Resolution)" Then
            value = " -multipass 1 "
        ElseIf Cmbx = "2 Pass (Full Resolution)" Then
            value = " -multipass 2 "
        Else
            value = " "
        End If

        Return value
    End Function
    Public Function vPreset(Cmbx As String, GPUHW As String) As String
        'Combobox5.text'
        Dim value As String
        If Cmbx = "" Then
            value = " "
        Else
            If GPUHW = "opencl" Then
                If Cmbx = "default" Then
                    value = " -quality balanced "
                ElseIf Cmbx = "slow" Then
                    value = " -quality quality "
                ElseIf Cmbx = "medium" Then
                    value = " -quality balanced "
                ElseIf Cmbx = "fast" Then
                    value = " -quality speed "
                Else
                    value = ""
                End If
            Else
                value = " -preset " & Cmbx & " "
            End If
        End If

        Return value
    End Function
    Public Function vPixFmt(Cmbx As String) As String
        'Combobox3.text'
        Dim value As String
        If Cmbx = "" Then
            value = " "
        Else
            value = " -pix_fmt " & Cmbx & " "
        End If

        Return value
    End Function
    Public Function vProfile(Cmbx As String) As String
        'Combobox7.text'
        Dim value As String
        If Cmbx = "" Then
            value = " "
        Else
            value = " -profile:v " & Cmbx & " "
        End If

        Return value
    End Function
    Public Function vRateControl(Cmbx As String) As String
        'Combobox4.text'
        Dim value As String
        If Cmbx = "" Then
            value = " "
        ElseIf Cmbx = "Variable Bit Rate" Then
            value = " -rc:v:0 vbr "
        ElseIf Cmbx = "Constant Bit Rate" Then
            value = " -rc:v:0 cbr "
        Else
            value = " "
        End If

        Return value
    End Function
    Public Function vSpaTempAQ(Cmbx As String) As String
        'Combobox11.text'
        Dim value As String
        If Cmbx = "disable" Then
            value = ""
        ElseIf Cmbx = "enable" Then
            value = " -spatial_aq 1 "
        Else
            value = ""
        End If

        Return value
    End Function
    Public Function vAQStrength(Cmbx As String) As String
        'Combobox12.text'
        Dim value As String
        If Cmbx = "" Then
            value = " "
        Else
            value = " -aq-strength " & Cmbx
        End If

        Return value
    End Function
    Public Function vTempAQ(Cmbx As String) As String
        'Combobox13.text'
        Dim value As String
        If Cmbx = "disable" Then
            value = ""
        ElseIf Cmbx = "enable" Then
            value = " -temporal_aq 1 "
        Else
            value = ""
        End If

        Return value
    End Function
    Public Function vTune(Cmbx As String) As String
        'Combobox6.text'
        Dim value As String
        If Cmbx = "High quality" Then
            value = " -tune hq "
        ElseIf Cmbx = "Low latency" Then
            value = " -tune ll"
        ElseIf Cmbx = "Ultra low latency" Then
            value = " -tune ull "
        ElseIf Cmbx = "Lossless" Then
            value = " -tune lossless"
        Else
            value = " "
        End If

        Return value
    End Function
    Public Function vTier(Cmbx As String, GPUHW As String) As String
        'Combobox9.text'
        Dim value As String
        If Cmbx = "" Then
            value = " "
        Else
            If GPUHW = "opencl" Then
                value = " -profile_tier " & Cmbx
            Else
                value = " -tier " & Cmbx
            End If
        End If

        Return value
    End Function
    Public Function vBrcompat(cmbx As String) As String
        'ComboBox21.text'
        Dim value As String
        If cmbx = "enable" Then
            value = " -bluray-compat true "
        ElseIf cmbx = "disable" Then
            value = " "
        Else
            value = " "
        End If

        Return value
    End Function
    Public Function vAspectRatio(cmbx As String) As String
        'ComboBox31.text'
        Dim value As String
        If cmbx = "3:2" Then
            value = "3/2"
        ElseIf cmbx = "4:3" Then
            value = "4/3"
        ElseIf cmbx = "16:9" Then
            value = "16/9"
        ElseIf cmbx = "16:10" Then
            value = "16/10"
        ElseIf cmbx = "21:9" Then
            value = "21/9"
        Else
            value = "null"
        End If

        Return value
    End Function
    Public Function vColorRange(cmbx As String) As String
        'Combobox37.text'
        Dim value As String
        If cmbx = "Full" Then
            value = "full"
        ElseIf cmbx = "Limited" Then
            value = "limited"
        Else
            value = ""
        End If

        Return value
    End Function
    Public Function vColorPrimary(cmbx As String) As String
        'Combobox38.text'
        Dim value As String
        If cmbx = "BT.709" Then
            value = " -color_primaries bt709"
        ElseIf cmbx = "BT.2020" Then
            value = " -color_primaries bt2020"
        Else
            value = ""
        End If

        Return value
    End Function
    Public Function vColorSpace(cmbx As String) As String
        'Combobox39.text'
        Dim value As String
        If cmbx = "BT.2020 Constant" Then
            value = " -colorspace bt2020c"
        ElseIf cmbx = "BT.2020 Non Constant" Then
            value = " -colorspace bt2020nc"
        Else
            value = ""
        End If

        Return value
    End Function
End Module