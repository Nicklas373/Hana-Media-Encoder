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
        If Cmbx = "Copy" Or Cmbx = "Passthrough" Then
            value = "copy"
        ElseIf Cmbx = "H264" Then
            If Encoder = "opencl" Then
                value = "h264_amf"
            ElseIf Encoder = "qsv" Then
                value = "h264_qsv"
            ElseIf Encoder = "cuda" Then
                value = "h264_nvenc"
            Else
                value = " "
            End If
        ElseIf Cmbx = "HEVC" Then
            If Encoder = "opencl" Then
                value = "hevc_amf"
            ElseIf Encoder = "qsv" Then
                value = "hevc_qsv"
            ElseIf Encoder = "cuda" Then
                value = "hevc_nvenc"
            Else
                value = " "
            End If
        ElseIf Cmbx = "AV1" Then
            If Encoder = "opencl" Then
                value = "av1_amf"
            ElseIf Encoder = "qsv" Then
                value = "av1_qsv"
            ElseIf Encoder = "cuda" Then
                value = "av1_nvenc"
            Else
                value = " "
            End If
        Else
            value = " "
        End If

        Return value
    End Function
    Public Function vLevel(Cmbx As String) As String
        'Combobox8.text'
        Dim value As String
        If String.IsNullOrEmpty(Cmbx) = True Then
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
    Public Function metadata(cmbx As String) As String
        'Metrosetcheckbox4.checked'
        Dim value As String
        If cmbx = "True" Then
            value = " -bitexact -map_metadata -1 -metadata:s:v:0 encoder=" + Chr(34) + My.Application.Info.Title.ToString + " v" + My.Application.Info.Version.ToString.Replace(".0", "") + Chr(34) + ""
        ElseIf cmbx = "False" Then
            value = " -metadata:s:v:0 encoder=" + Chr(34) + My.Application.Info.Title.ToString + " v" + My.Application.Info.Version.ToString.Replace(".0", "") + Chr(34) + ""
        Else
            value = " -metadata:s:v:0 encoder=" + Chr(34) + My.Application.Info.Title.ToString + " v" + My.Application.Info.Version.ToString.Replace(".0", "") + Chr(34) + ""
        End If

        Return value
    End Function
    Public Function vPreset(Cmbx As String, GPUHW As String, Codec As String) As String
        'Combobox5.text'
        Dim value As String
        If String.IsNullOrEmpty(Cmbx) = True Then
            value = " "
        Else
            If GPUHW = "opencl" Then
                If Cmbx = "default" Then
                    value = " -quality balanced "
                ElseIf Cmbx = "slow" Or Cmbx = "slowest" Then
                    If Codec = "AV1" Then
                        value = " -quality high_quality "
                    Else
                        value = " -quality quality "
                    End If
                ElseIf Cmbx = "medium" Then
                    value = " -quality balanced "
                ElseIf Cmbx = "fast" Then
                    value = " -quality speed "
                Else
                    value = ""
                End If
            ElseIf GPUHW = "qsv" Then
                If Cmbx = "slowest" Then
                    value = " -preset slower"
                Else
                    value = " -preset " & Cmbx & " "
                End If
            ElseIf GPUHW = "cuda" Then
                If Cmbx = "default" Then
                    value = " -preset p4 "
                ElseIf Cmbx = "slowest" Then
                    value = " -preset p7 "
                ElseIf Cmbx = "slow" Then
                    value = " -preset p5 "
                ElseIf Cmbx = "medium" Then
                    value = " -preset p4 "
                ElseIf Cmbx = "fast" Then
                    value = " -preset p3 "
                Else
                    value = ""
                End If
            Else
                value = " "
            End If
        End If

        Return value
    End Function
    Public Function vPixFmt(Cmbx As String, GPUHW As String, Codec As String, Force10Bit As String) As String
        'Combobox3.text'
        Dim newValue As String
        Dim value As String
        If String.IsNullOrEmpty(Cmbx) = True Then
            value = " "
        Else
            If Codec = "AV1" And GPUHW IsNot "opencl" Then
                If Cmbx = "yuv420p" Then
                    newValue = " -pix_fmt " & Cmbx & " -rgb_mode 1 "
                ElseIf Cmbx = "yuv444p" Then
                    newValue = " -pix_fmt " & Cmbx & " -rgb_mode 2 "
                ElseIf Cmbx = "p010le" Then
                    newValue = " -pix_fmt " & Cmbx & " -rgb_mode 1 "
                Else
                    newValue = " -pix_fmt " & Cmbx & " -rgb_mode 1 "
                End If
                If Force10Bit Then
                    value = newValue & " -highbitdepth true "
                Else
                    value = newValue
                End If
            Else
                value = " "
            End If
        End If

        Return value
    End Function
    Public Function vProfile(Cmbx As String) As String
        'Combobox7.text'
        Dim value As String
        If String.IsNullOrEmpty(Cmbx) = True Then
            value = " "
        Else
            value = " -profile:v " & Cmbx & " "
        End If

        Return value
    End Function
    Public Function vRateControl(Cmbx As String, Codec As String, HWAccel As String) As String
        'Combobox4.text'
        Dim value As String
        If String.IsNullOrEmpty(Cmbx) = True Then
            value = " "
        ElseIf Cmbx = "Variable Bit Rate" Then
            If Codec = "AV1" And HWAccel = "opencl" Then
                value = " -rc:v:0 hqvbr "
            Else
                value = " -rc:v:0 vbr "
            End If
        ElseIf Cmbx = "Constant Bit Rate" Then
            If Codec = "AV1" And HWAccel = "opencl" Then
                value = " -rc:v:0 hqcbr "
            Else
                value = " -rc:v:0 cbr "
            End If
        Else
            value = " "
        End If

        Return value
    End Function
    Public Function vLookAheadLvl(Cmbx As String, GPUHW As String, codec As String) As String
        'LookaheadUpDown1'
        Dim value As String
        If String.IsNullOrEmpty(Cmbx) = True Then
            value = " "
        Else
            If GPUHW = "opencl" Then
                If codec = "H264" Then
                    value = " -lookahead true -lookahead_depth " & Cmbx
                Else
                    value = " -lookahead_depth " & Cmbx
                End If
            Else
                value = " -lookahead_level " & Cmbx
            End If
        End If

        Return value
    End Function
    Public Function vSpaTempAQ(Cmbx As String, Codec As String) As String
        'Combobox11.text'
        Dim value As String
        If Cmbx = "disable" Then
            value = ""
        ElseIf Cmbx = "enable" And Codec IsNot "AV1" Then
            value = " -spatial_aq 1 "
        ElseIf Cmbx = "enable" And Codec Is "AV1" Then
            value = " -spatial-aq 1 "
        Else
            value = " "
        End If

        Return value
    End Function
    Public Function vAQStrength(Cmbx As String) As String
        'Combobox12.text'
        Dim value As String
        If String.IsNullOrEmpty(Cmbx) = True Then
            value = " "
        Else
            value = " -aq-strength " & Cmbx
        End If

        Return value
    End Function
    Public Function vTempAQ(Cmbx As String, codec As String) As String
        'Combobox13.text'
        Dim value As String
        If Cmbx = "disable" Then
            value = " "
        ElseIf Cmbx = "enable" And Codec IsNot "AV1" Then
            value = " -temporal_aq 1 "
        ElseIf Cmbx = "enable" And Codec Is "AV1" Then
            value = " -temporal-aq 1 "
        Else
            value = " "
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
    Public Function vTier(Cmbx As String, GPUHW As String, Codec As String) As String
        'Combobox9.text'
        Dim value As String
        If String.IsNullOrEmpty(Cmbx) = True Then
            value = " "
        Else
            If GPUHW = "opencl" Then
                value = " -profile_tier " & Cmbx
            Else
                If GPUHW = "cuda" And Codec = "AV1" Then
                    If Cmbx = "main" Then
                        value = " -tier 0"
                    ElseIf Cmbx = "high" Then
                        value = " -tier 1"
                    Else
                        value = " -tier 0"
                    End If
                Else
                    value = " -tier " & Cmbx
                End If
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
        If cmbx = "1.33 (4:3)" Then
            value = "1.33"
        ElseIf cmbx = "1.78 (16:9)" Then
            value = "1.78"
        ElseIf cmbx = "2.33 (21:9)" Then
            value = "2.33"
        Else
            value = " "
        End If

        Return value
    End Function
    Public Function vAspectRatioCmp(intAsp As Integer, tarAsp As Integer)
        Dim value As String
        If vAspectRatioTrX(intAsp) > tarAsp Then
            value = "down"
        ElseIf vAspectRatioTrX(intAsp) < tarAsp Then
            value = "up"
        Else
            value = "err"
        End If
        Return value
    End Function
    Public Function vAspectRatioTrX(intAspX As Integer) As Integer
        Dim value As Integer
        Dim intAspNew As Double
        If intAspX.ToString.Length = 3 Then
            intAspNew = (intAspX / 100)
            If intAspNew = 1.33 Then
                value = 4
            ElseIf intAspNew = 1.78 Then
                value = 16
            ElseIf intAspNew = 2.33 Then
                value = 21
            ElseIf intAspNew = 1 Then
                value = intAspNew
            End If
        Else
            value = intAspX
        End If

        Return value
    End Function
    Public Function vAspectRatioTrY(intAspX As Integer, intAspY As Integer) As Integer
        Dim value As Integer
        Dim intAspNew As Double
        If intAspY.ToString.Length = 3 Then
            intAspNew = (intAspY / 100)
            If intAspNew = 1 Then
                If intAspX = 4 Then
                    value = 3
                ElseIf intAspX = 16 Then
                    value = 9
                ElseIf intAspX = 21 Then
                    value = 9
                Else
                    value = 9
                End If
            ElseIf intAspNew = 2 Then
                If intAspX = 21 Then
                    value = 9
                Else
                    value = 9
                End If
            End If
        Else
            value = intAspY
        End If

        Return value
    End Function
    Public Function vColorRange(cmbx As String) As String
        'Combobox37.text'
        Dim value As String
        If cmbx = "Full" Then
            value = " -color_range 2"
        ElseIf cmbx = "Limited" Then
            value = " -color_range 1"
        Else
            value = " "
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
            value = " "
        End If

        Return value
    End Function
    Public Function vColorSpace(cmbx As String, hwaccel As String) As String
        'Combobox39.text'
        Dim value As String
        If cmbx = "BT.2020 Constant" Then
            value = " -colorspace bt2020c -color_trc bt2020-10"
        ElseIf cmbx = "BT.2020 Non Constant" Then
            value = " -colorspace bt2020nc -color_trc bt2020-10"
        ElseIf cmbx = "BT.709" Then
            value = " -colorspace bt709 -color_trc bt709"
        Else
            value = " "
        End If

        Return value
    End Function
    Public Function vDeInterlace(cmbx As String, hwaccel As String, mode As String, parity As String, deint As String)
        Dim value As String
        Dim newMode As String
        Dim newParity As String
        Dim newDeint As String

        If String.IsNullOrEmpty(mode) = True Then
            newMode = ""
        Else
            If mode = "send_frame" Then
                newMode = "0"
            ElseIf mode = "send_field" Then
                newMode = "1"
            ElseIf mode = "send_frame_nospatial" And cmbx = "yadif" And hwaccel.Contains("cuda") Then
                newMode = "2"
            ElseIf mode = "send_field_nospatial" And cmbx = "yadif" And hwaccel.Contains("cuda") Then
                newMode = "3"
            Else
                newMode = "0"
            End If
        End If

        If String.IsNullOrEmpty(parity) = True Then
            newParity = ""
        Else
            If parity = "top field" Then
                newParity = "0"
            ElseIf parity = "bottom field" Then
                newParity = "1"
            Else
                newParity = "-1"
            End If
        End If

        If String.IsNullOrEmpty(deint) = True Then
            newDeint = ""
        Else
            If deint = "all" Then
                newDeint = "0"
            ElseIf deint = "interlaced" Then
                newDeint = "1"
            Else
                newDeint = "0"
            End If
        End If

        If String.IsNullOrEmpty(cmbx) = True Then
            value = " "
        ElseIf newDeint Is "" Or newMode Is "" Or newParity Is "" Then
            value = " "
        Else
            If hwaccel.Contains("cuda") Then
                value = " -vf " & cmbx & "_cuda=" & newMode & ":" & newParity & ":" & newDeint
            Else
                value = " -vf " & cmbx & "=" & newMode & ":" & newParity & ":" & newDeint
            End If
        End If

        Return value
    End Function
    Public Function vCropAspDown(tarHeigth As Integer, tarWidth As Integer, inpAspX As Integer, inpAspY As Integer, scaleAlgo As String) As String
        Dim value As String
        Dim xScale As Integer = (tarHeigth / vAspectRatioTrY(vAspectRatioTrX(inpAspX), inpAspY)) * vAspectRatioTrX(inpAspX)
        value = "scale=" & xScale & ":" & tarHeigth & scaleAlgo & ",crop=" & tarWidth & ":" & tarHeigth & ":(" & xScale & "-" & tarHeigth & ")/2:0"

        Return value
    End Function
    Public Function vCropAspUp(tarHeigth As Integer, tarWidth As Integer, inpAspX As Integer, inpAspY As Integer, scaleAlgo As String) As String
        Dim value As String
        Dim yScale As Integer = (tarWidth / vAspectRatioTrX(inpAspX)) * vAspectRatioTrY(vAspectRatioTrX(inpAspX), inpAspY)
        value = "scale=" & tarWidth & ":" & yScale & scaleAlgo & ",crop=" & tarWidth & ":" & tarHeigth & ":0:(" & yScale & "-" & tarHeigth & ")/2"

        Return value
    End Function
    Public Function vPadAspDown(tarHeigth As Integer, tarWidth As Integer, inpAspX As Integer, inpAspY As Integer, scaleAlgo As String) As String
        Dim value As String
        Dim yScale As Integer = (tarWidth / vAspectRatioTrX(inpAspX)) * vAspectRatioTrY(vAspectRatioTrX(inpAspX), inpAspY)
        value = "scale=" & tarWidth & ":" & yScale & scaleAlgo & ",pad=" & tarWidth & ":" & tarHeigth & ":0:(" & tarHeigth & "-" & yScale & ")/2"

        Return value
    End Function
    Public Function vPadAspUp(tarHeigth As Integer, tarWidth As Integer, inpAspX As Integer, inpAspY As Integer, scaleAlgo As String) As String
        Dim value As String
        Dim xScale As Integer = (tarHeigth / vAspectRatioTrY(vAspectRatioTrX(inpAspX), inpAspY)) * vAspectRatioTrX(inpAspX)
        value = "scale=" & xScale & ":" & tarHeigth & scaleAlgo & ",pad=" & tarWidth & ":" & tarHeigth & ":(" & tarWidth & "-" & xScale & ")/2:0"

        Return value
    End Function
    Public Function vResTranslate(cmbx As String) As String
        'Combobox40.text'
        Dim value As String
        If String.IsNullOrEmpty(cmbx) = True Then
            value = " "
        Else
            value = getBetween(cmbx, "(", ")") & "y"
        End If

        Return value
    End Function
    Public Function vResTranslateReverse(asp As String, res As String) As String
        Dim value As String
        If String.IsNullOrEmpty(asp) = True Then
            value = " "
        ElseIf asp = "1.33 (4:3)" Then
            If getBetween(res, "=", "x") = "960" Then
                value = "720p (960x720)"
            ElseIf getBetween(res, "=", "x") = "1440" Then
                value = "1080p (1440x1080)"
            ElseIf getBetween(res, "=", "x") = "1536" Then
                value = "2K (1536x1152)"
            ElseIf getBetween(res, "=", "x") = "2160" Then
                value = "3K UHD (2160x1620)"
            ElseIf getBetween(res, "=", "x") = "2880" Then
                value = "4K UHD (2880x2160)"
            Else
                value = " "
            End If
        ElseIf asp = "1.78 (16:9)" Then
            If getBetween(res, "=", "x") = "720" Then
                value = "720p (1280x720)"
            ElseIf getBetween(res, "=", "x") = "1920" Then
                value = "1080p (1920x1080)"
            ElseIf getBetween(res, "=", "x") = "2048" Then
                value = "2K (2048x1152)"
            ElseIf getBetween(res, "=", "x") = "2880" Then
                value = "3K UHD (2880x1620)"
            ElseIf getBetween(res, "=", "x") = "3840" Then
                value = "4K UHD (3840x2160)"
            Else
                value = " "
            End If
        ElseIf asp = "2.33 (21:9)" Then
            If getBetween(res, "=", "x") = "2560" Then
                value = "WFHD (2560x1080)"
            ElseIf getBetween(res, "=", "x") = "2880" Then
                value = "WFHD+ (2880x1200)"
            ElseIf getBetween(res, "=", "x") = "3440" Then
                value = "WQHD (3440x1440)"
            ElseIf getBetween(res, "=", "x") = "3840" Then
                value = "WQHD+ (3840x1600)"
            ElseIf getBetween(res, "=", "x") = "4320" Then
                value = "UW4K (4320x1800)"
            Else
                value = " "
            End If
        Else
            value = " "
        End If

        Return value
    End Function
End Module