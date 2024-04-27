Module MediaVideoAltModule
    Public Function vCodecAlt(Cmbx As String) As String
        'Combobox2.text'
        Dim value As String
        If Cmbx = "H264" Then
            value = " --codec h264 "
        ElseIf Cmbx = "HEVC" Then
            value = " --codec hevc "
        ElseIf Cmbx = "AV1" Then
            value = " --codec av1 "
        Else
            value = " "
        End If

        Return value
    End Function

    Public Function vFpsAlt(cmbx As String) As String
        Dim values As String
        If cmbx IsNot "" Then
            values = " --fps " & cmbx
        Else
            values = " "
        End If

        Return values
    End Function

    Public Function vProfileAlt(Cmbx As String) As String
        Dim value As String
        If Cmbx IsNot "" Then
            value = " --profile " & Cmbx
        Else
            value = " "
        End If

        Return value
    End Function

    Public Function vLevelAlt(Cmbx As String) As String
        'Combobox8.text'
        Dim value As String
        If Cmbx = "" Then
            value = " "
        Else
            value = " --level " & Cmbx
        End If

        Return value
    End Function

    Public Function vTierAlt(Cmbx As String, codec As String) As String
        Dim value As String
        If Cmbx IsNot "" Then
            If codec = "AV1" Then
                If Cmbx = "main" Then
                    value = " --tier 0"
                ElseIf Cmbx = "high" Then
                    value = " --tier 1"
                Else
                    value = " --tier 0"
                End If
            Else
                value = " --tier " & Cmbx
            End If
        Else
            value = " "
        End If

        Return value
    End Function

    Public Function vPixFmtAlt(Cmbx As String, Force10Bit As String) As String
        'Combobox3.text'
        Dim value As String
        If Cmbx = "" Then
            value = " "
        Else
            If Force10Bit Then
                value = " --output-csp " & Cmbx & " --output-depth 10 "
            Else
                value = " --output-csp " & Cmbx & " --output-depth 8 "
            End If
        End If

        Return value
    End Function

    Public Function vRateControlAlt(Cmbx As String, MaxBitrate As String, OvrBitrate As String, RateQuality As String) As String
        'Combobox4.text'
        Dim value As String
        Dim maxbitratenewvalue As String
        Dim ovrbitratenewvalue As String
        Dim ratequalitynewvalue As String

        If MaxBitrate = "" Or MaxBitrate = "0" Then
            maxbitratenewvalue = " "
        Else
            maxbitratenewvalue = " --max-bitrate " & MaxBitrate & "000K"
        End If

        If OvrBitrate = "" Or OvrBitrate = "0" Then
            ovrbitratenewvalue = " "
        Else
            If Cmbx = "" Then
                ovrbitratenewvalue = " "
            ElseIf Cmbx = "Variable Bit Rate" Then
                ovrbitratenewvalue = " --vbr " & OvrBitrate & "000K"
            ElseIf Cmbx = "Constant Bit Rate" Then
                ovrbitratenewvalue = " --cbr " & OvrBitrate & "000K"
            Else
                ovrbitratenewvalue = " "
            End If
        End If

        If RateQuality = "" Or RateQuality = "0" Then
            ratequalitynewvalue = " "
        Else
            If Cmbx = "" Then
                ratequalitynewvalue = " "
            ElseIf Cmbx = "Variable Bit Rate" Then
                ratequalitynewvalue = " --vbr-quality " & RateQuality
            ElseIf Cmbx = "Constant Bit Rate" Then
                ratequalitynewvalue = " "
            Else
                ratequalitynewvalue = " "
            End If
        End If

        value = maxbitratenewvalue & ovrbitratenewvalue & ratequalitynewvalue

        Return value
    End Function

    Public Function vPresetAlt(Cmbx As String) As String
        Dim value As String
        If Cmbx = "" Then
            value = " "
        Else
            If Cmbx = "default" Then
                value = " --preset default "
            ElseIf Cmbx = "slow" Then
                value = " --preset quality "
            ElseIf Cmbx = "medium" Then
                value = " --preset default "
            ElseIf Cmbx = "fast" Then
                value = " --preset performance "
            ElseIf Cmbx = "slowest" Then
                value = " --preset quality"
            Else
                value = " "
            End If
        End If

        Return value
    End Function

    Public Function vTuneAlt(Cmbx As String) As String
        'Combobox6.text'
        Dim value As String
        If Cmbx = "High quality" Then
            value = " --tune hq "
        ElseIf Cmbx = "Low latency" Then
            value = " --tune lowlatency "
        ElseIf Cmbx = "Ultra low latency" Then
            value = " --tune ultralowlatency "
        ElseIf Cmbx = "Lossless" Then
            value = " --tune lossless "
        Else
            value = " "
        End If

        Return value
    End Function

    Public Function vColorRangeAlt(cmbx As String) As String
        'Combobox37.text'
        Dim value As String
        If cmbx = "Full" Then
            value = " --colorrange full "
        ElseIf cmbx = "Limited" Then
            value = " --colorrange limited "
        Else
            value = " "
        End If

        Return value
    End Function
    Public Function vColorPrimaryAlt(cmbx As String) As String
        'Combobox38.text'
        Dim value As String
        If cmbx = "BT.709" Then
            value = " --colorprim bt709"
        ElseIf cmbx = "BT.2020" Then
            value = " --colorprim bt2020"
        Else
            value = " "
        End If

        Return value
    End Function
    Public Function vColorSpaceAlt(cmbx As String) As String
        'Combobox39.text'
        Dim value As String
        If cmbx = "BT.2020 Constant" Then
            value = " --colormatrix bt2020c --transfer bt2020-10"
        ElseIf cmbx = "BT.2020 Non Constant" Then
            value = " --colormatrix bt2020nc --transfer bt2020-10"
        ElseIf cmbx = "BT.709" Then
            value = " --colormatrix bt709 --transfer bt709"
        Else
            value = " "
        End If

        Return value
    End Function

    Public Function vSpaTempAQAlt(Cmbx As String) As String
        'Combobox11.text'
        Dim value As String
        If Cmbx = "disable" Or Cmbx = "" Then
            value = " "
        Else
            value = " --aq "
        End If

        Return value
    End Function
    Public Function vAQStrengthAlt(Cmbx As String) As String
        'Combobox12.text'
        Dim value As String
        If Cmbx = "" Then
            value = " "
        Else
            value = " --aq-strength " & Cmbx
        End If

        Return value
    End Function
    Public Function vTempAQAlt(Cmbx As String) As String
        'Combobox13.text'
        Dim value As String
        If Cmbx = "disable" Or Cmbx = "" Then
            value = " "
        Else
            value = " --aq-temporal "
        End If

        Return value
    End Function

    Public Function vBrcompatAlt(cmbx As String) As String
        'ComboBox21.text'
        Dim value As String
        If cmbx = "enable" Or cmbx = "" Then
            value = " --bluray "
        ElseIf cmbx = "disable" Then
            value = " "
        Else
            value = " "
        End If

        Return value
    End Function

    Public Function bRefModeAlt(Cmbx As String) As String
        'Combobox10.text'
        Dim value As String
        If Cmbx = "" Then
            value = " "
        Else
            value = " --bref-mode " & Cmbx
        End If

        Return value
    End Function

    Public Function multiPassAlt(Cmbx As String) As String
        'Combobox14.text'
        Dim value As String
        If Cmbx = "1 Pass" Then
            value = " --multipass none "
        ElseIf Cmbx = "2 Pass (1/4 Resolution)" Then
            value = " --multipass 2pass-quarter "
        ElseIf Cmbx = "2 Pass (Full Resolution)" Then
            value = " --multipass 2pass-full "
        Else
            value = " "
        End If

        Return value
    End Function
End Module
