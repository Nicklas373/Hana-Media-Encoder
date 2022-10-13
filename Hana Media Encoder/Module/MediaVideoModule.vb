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
        If cmbx = "4:3" Then
            value = "4/3"
        ElseIf cmbx = "16:9" Then
            value = "16/9"
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
    Public Function vResTranslate(cmbx As String) As String
        'Combobox40.text'
        Dim value As String
        If cmbx = "" Then
            value = ""
        Else
            value = getBetween(cmbx, "(", ")") & "y"
        End If

        Return value
    End Function
    Public Function vResTranslateReverse(asp As String, res As String) As String
        Dim value As String
        If asp = "" Then
            value = ""
        ElseIf asp = "4:3" Then
            If getBetween(res, "(", "x") = "960" Then
                value = "720p (960x720)"
            ElseIf getBetween(res, "(", "x") = "1920" Then
                value = "1080p (1920x1080)"
            ElseIf getBetween(res, "(", "x") = "1536" Then
                value = "2K (1536x1152)"
            ElseIf getBetween(res, "(", "x") = "2160" Then
                value = "3K UHD (2160x1620)"
            ElseIf getBetween(res, "(", "x") = "2880" Then
                value = "4K UHD (2880x2160)"
            Else
                value = ""
            End If
        ElseIf asp = "16:9" Then
            If getBetween(res, "(", "x") = "720" Then
                value = "720p (1280x720)"
            ElseIf getBetween(res, "(", "x") = "1920" Then
                value = "1080p (1920x1080)"
            ElseIf getBetween(res, "(", "x") = "2048" Then
                value = "2K (2048x1152)"
            ElseIf getBetween(res, "(", "x") = "2880" Then
                value = "3K UHD (2880x1620)"
            ElseIf getBetween(res, "(", "x") = "3840" Then
                value = "4K UHD (3840x2160)"
            Else
                value = ""
            End If
        ElseIf asp = "21:9" Then
            If getBetween(res, "(", "x") = "2560" Then
                value = "WFHD (2560x1080)"
            ElseIf getBetween(res, "(", "x") = "2880" Then
                value = "WFHD+ (2880x1200)"
            ElseIf getBetween(res, "(", "x") = "3440" Then
                value = "WQHD (3440x1440)"
            ElseIf getBetween(res, "(", "x") = "3840" Then
                value = "WQHD+ (3840x1600)"
            ElseIf getBetween(res, "(", "x") = "4320" Then
                value = "UW4K (4320x1800)"
            Else
                value = ""
            End If
        Else
            value = ""
        End If

        Return value
    End Function
End Module