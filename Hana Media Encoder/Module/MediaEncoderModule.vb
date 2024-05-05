Imports System.IO
Imports Newtonsoft.Json

Module MediaEncoderModule
    Public Sub CleanEnv(cleanStats As String)
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete(My.Application.Info.DirectoryPath & "\FFMETADATAFILE")
        MassDelete(My.Application.Info.DirectoryPath & "\", "bat")
        MassDelete(My.Application.Info.DirectoryPath & "\", "msi")
        MassDelete(My.Application.Info.DirectoryPath & "\", "txt")
        If cleanStats = "all" Then
            File.Delete(My.Application.Info.DirectoryPath & "\spectrum-temp.jpg")
            MassDelete(My.Application.Info.DirectoryPath & "\", "bat")
            MassDelete(My.Application.Info.DirectoryPath & "\", "msi")
            MassDelete(My.Application.Info.DirectoryPath & "\", "txt")
            MassDelete(My.Application.Info.DirectoryPath & "\audioStream", "txt")
            MassDelete(My.Application.Info.DirectoryPath & "\audioConfig", "txt")
            MassDelete(My.Application.Info.DirectoryPath & "\chapterConfig", "txt")
            MassDelete(My.Application.Info.DirectoryPath & "\chapterConfig", "FFMETADATAFILE")
            MassDelete(My.Application.Info.DirectoryPath & "\queue\audio\audioStream", "txt")
            MassDelete(My.Application.Info.DirectoryPath & "\queue\audio\audioConfig", "txt")
            MassDelete(My.Application.Info.DirectoryPath & "\HME-Engine", "bat")
            MassDelete(My.Application.Info.DirectoryPath & "\muxConfig", "txt")
            MassDelete(My.Application.Info.DirectoryPath & "\trimConfig", "txt")
            MassDelete(My.Application.Info.DirectoryPath & "\videoStream", "txt")
            MassDelete(My.Application.Info.DirectoryPath & "\videoConfig", "txt")
            MassDelete(My.Application.Info.DirectoryPath & "\queue\video\videoStream", "txt")
            MassDelete(My.Application.Info.DirectoryPath & "\queue\video\videoConfig", "txt")
            MassDelete(My.Application.Info.DirectoryPath & "\thumbnail", "jpg")
        End If
    End Sub
    Public Function GetGraphicsHWEngine(HWDec As String) As String
        Dim HWDecName As String
        If HWDec = "" Then
            HWDecName = "null"
        ElseIf HWDec = "Intel (QuickSync)" Then
            HWDecName = "qsv"
        ElseIf HWDec = "AMD (OpenCL)" Then
            HWDecName = "opencl"
        ElseIf HWDec = "NVIDIA (NVENC / NVDEC)" Then
            HWDecName = "cuda"
        Else
            HWDecName = "null"
        End If

        Return HWDecName
    End Function
    Public Sub HMEAudioStreamConfigGenerate(HMEName As String, acodec As String, abitdepth As String, aratecontrol As String,
                                             arate As String, achannel As String, acomplvl As String, afreq As String, achannellayout As String)
        If File.Exists(HMEName) Then
            GC.Collect()
            GC.WaitForPendingFinalizers()
            File.Delete(HMEName)
        End If

        Dim mediaAudioProfile As New MediaAudioClass With {
            .Codec = acodec,
            .BitDepth = abitdepth,
            .RateControl = aratecontrol,
            .Rate = arate,
            .Channel = achannel,
            .ChannelLayout = achannellayout,
            .Compression = acomplvl,
            .Frequency = afreq
        }

        Dim AUProfile As String = JsonConvert.SerializeObject(mediaAudioProfile)
        File.WriteAllText(HMEName, AUProfile)
    End Sub
    Public Sub HMEGenerate(HMEName As String, ffmpegletter As String, ffmpegbin As String, ffargs As String, ffargs2 As String)
        If File.Exists(HMEName) Then
            GC.Collect()
            GC.WaitForPendingFinalizers()
            File.Delete(HMEName)
            File.Create(HMEName).Dispose()
        Else
            File.Create(HMEName).Dispose()
        End If
        Dim writer As New StreamWriter(HMEName, True)
        writer.WriteLine("chcp 65001")
        writer.WriteLine("@echo off")
        writer.WriteLine(ffmpegletter)
        writer.WriteLine("cd " & ffmpegbin)
        writer.WriteLine(ffargs)
        writer.WriteLine(ffargs2)
        writer.Close()
    End Sub
    Public Sub HMEGenerateAlt(HMEName As String, ffmpegletter As String, ffmpegbin As String, ffargs As String, ffargs2 As String)
        If File.Exists(HMEName) Then
            GC.Collect()
            GC.WaitForPendingFinalizers()
            File.Delete(HMEName)
            File.Create(HMEName).Dispose()
        Else
            File.Create(HMEName).Dispose()
        End If
        Dim writer As New StreamWriter(HMEName, True)
        writer.WriteLine("@echo off")
        writer.WriteLine(ffmpegletter)
        writer.WriteLine("cd " & ffmpegbin)
        writer.WriteLine(ffargs)
        writer.WriteLine(ffargs2)
        writer.WriteLine("exit")
        writer.Close()
    End Sub
    Public Sub HMEStreamProfileGenerate(HMEName As String, ffargs As String)
        If File.Exists(HMEName) Then
            GC.Collect()
            GC.WaitForPendingFinalizers()
            File.Delete(HMEName)
            File.Create(HMEName).Dispose()
            Dim writer As New StreamWriter(HMEName, True)
            writer.WriteLine(ffargs)
            writer.Close()
        Else
            File.Create(HMEName).Dispose()
            Dim writer As New StreamWriter(HMEName, True)
            writer.WriteLine(ffargs)
            writer.Close()
        End If
    End Sub
    Public Sub HMEVideoStreamConfigGenerate(HMEName As String, brCompat As String, ovrbitrate As String, bref As String, codec As String, framerate As String,
                                            level As String, maxbitrate As String, multipass As String, preset As String, pixfmt As String, profile As String,
                                            force10bit As String, ratectr As String, lookahead As String, spatialaq As String, aqstrength As String, temporalaq As String, targetql As String,
                                            tier As String, tune As String, tilecol As String, tilerow As String, ar As String, res As String, algo As String, colorrange As String,
                                            colorprimary As String, colorspace As String, deinterlace As String, deinterlaceMode As String, deinterlaceParity As String, deinterlaceFrame As String,
                                            scaleType As String, metaData As String)
        If File.Exists(HMEName) Then
            GC.Collect()
            GC.WaitForPendingFinalizers()
            File.Delete(HMEName)
        End If

        Dim mediaVideoProfile As New MediaVideoClass With {
            .BRCompat = brCompat,
            .OvrBitrate = ovrbitrate,
            .Bref = bref,
            .Codec = codec,
            .Fps = framerate,
            .Level = level,
            .MaxBitrate = maxbitrate,
            .Multipass = multipass,
            .Preset = preset,
            .PixelFormat = pixfmt,
            .Profile = profile,
            .Force10Bit = force10bit,
            .RateControl = ratectr,
            .LookAhead = lookahead,
            .SpatialAQ = spatialaq,
            .AQStrength = aqstrength,
            .TemporalAQ = temporalaq,
            .TargetQL = targetql,
            .Tier = tier,
            .Tune = tune,
            .TileCol = tilecol,
            .TileRow = tilerow,
            .AspectRatio = ar,
            .Resolution = res,
            .ScaleAlgo = algo,
            .ColorRange = colorrange,
            .ColorPrimary = colorprimary,
            .ColorSpace = colorspace,
            .Deinterlace = deinterlace,
            .DeMode = deinterlaceMode,
            .DeParity = deinterlaceParity,
            .DeFrame = deinterlaceFrame,
            .ScaleType = scaleType,
            .Metadata = metaData
        }

        Dim MVProfile As String = JsonConvert.SerializeObject(mediaVideoProfile)
        File.WriteAllText(HMEName, MVProfile)
    End Sub
    Public Sub InitExit(state As String)
        Dim progList As String() = {"ffplay", "ffmpeg", "ffprobe", "nvencc64"}
        For Each prog As Process In Process.GetProcesses
            For Each progQueue As String In progList
                If prog.ProcessName = progQueue Then
                    prog.Kill()
                End If
            Next
        Next
        If state IsNot "reset" Then
            Environment.Exit(Environment.ExitCode)
        End If

    End Sub
    Public Sub previewMediaModule(mediaFile As String, ffplay As String, hwengine As String, mediaID As String)
        Dim newffargs As String
        If mediaID = "Not Detected" Then
            newffargs = " -x 960 -y 540 -showmode 1"
        Else
            newffargs = " -hwaccel " & hwengine & " -x 960 -y 540 -showmode 0"
        End If
        Dim psi As New ProcessStartInfo(ffplay) With {
               .RedirectStandardError = False,
               .RedirectStandardOutput = True,
               .CreateNoWindow = True,
               .Arguments = Chr(34) & mediaFile & Chr(34) & newffargs,
               .WindowStyle = ProcessWindowStyle.Hidden,
               .UseShellExecute = False
           }
        Dim process As Process = Process.Start(psi)
    End Sub
    Public Sub RunProc(bat As String)
        Dim psi As New ProcessStartInfo(bat) With {
            .RedirectStandardError = False,
            .RedirectStandardOutput = False,
            .CreateNoWindow = True,
            .WindowStyle = ProcessWindowStyle.Hidden,
            .UseShellExecute = False
        }
        Dim process As Process = Process.Start(psi)
        process.WaitForExit()
    End Sub
    Public Sub RunProcAlt(bat As String)
        Dim psi As New ProcessStartInfo(bat) With {
            .RedirectStandardError = False,
            .RedirectStandardOutput = False,
            .CreateNoWindow = True,
            .WindowStyle = ProcessWindowStyle.Hidden,
            .UseShellExecute = False
        }
        Dim process As Process = Process.Start(psi)
    End Sub
    Public Async Sub RunProcAsync(bat As String)
        Dim psi As New ProcessStartInfo(bat) With {
            .RedirectStandardError = False,
            .RedirectStandardOutput = False,
            .CreateNoWindow = True,
            .WindowStyle = ProcessWindowStyle.Hidden,
            .UseShellExecute = False
        }
        Dim process As Process = Process.Start(psi)
        Await Task.Delay(1500)
        Await Task.Run(Sub() process.WaitForExit())
    End Sub
    Public Function TimeConversion(Hours As Integer, Minute As Integer, Seconds As Integer) As Integer
        Dim hoursToMinute As Integer = Hours * 60
        Dim minuteToSeconds As Integer = (hoursToMinute + Minute) * 60
        Dim newSeconds As Integer = minuteToSeconds + Seconds
        Return newSeconds
    End Function
    Public Function TimeConversionReverse(CnvDur As Integer) As String
        Dim hoursValue As Integer = 3600 'Hours Value
        Dim hoursCounter As Integer = 0 'Hours Counter Value / Final hours value
        Dim minuteDeviation As Integer 'Deviation from CnvDur - (hoursValue * hoursCounter)
        Dim minuteValue As Integer = 60 'Minute Value
        Dim minuteCounter As Integer = 0 'Minute Counter Value / Final minute value
        Dim secondsDeviation As Integer 'Deviation from minuteDeviation - (minuteValue * minuteCounter)
        Dim conversionResult As String 'Final time result
        If hoursValue > CnvDur Then
            hoursValue = 3600
            hoursCounter = 0
        Else
            If hoursValue < CnvDur Then
                For b = 3600 To CnvDur Step 3600
                    hoursCounter += 1
                Next b
            End If
        End If
        minuteDeviation = CnvDur - (hoursValue * hoursCounter)
        If minuteValue > minuteDeviation Then
            minuteValue = 60
            minuteCounter = 0
        Else
            If minuteValue < minuteDeviation Then
                For z = 60 To minuteDeviation Step 60
                    minuteCounter += 1
                Next z
            End If
        End If
        If minuteCounter > 59 Then
            hoursCounter += 1
            minuteCounter = 0
            secondsDeviation = 0
        Else
            minuteDeviation = minuteDeviation
            hoursCounter = hoursCounter
            secondsDeviation = minuteDeviation - (minuteValue * minuteCounter)
        End If
        If secondsDeviation > 59 Then
            secondsDeviation = 0
            minuteCounter += 1
        Else
            secondsDeviation = secondsDeviation
            minuteCounter = minuteCounter
        End If
        If hoursCounter < 10 Then
            If minuteCounter < 10 Then
                If secondsDeviation < 10 Then
                    conversionResult = "0" & hoursCounter & ":0" & minuteCounter & ":0" & secondsDeviation
                Else
                    conversionResult = "0" & hoursCounter & ":0" & minuteCounter & ":" & secondsDeviation
                End If
            Else
                If secondsDeviation < 10 Then
                    conversionResult = "0" & hoursCounter & ":" & minuteCounter & ":0" & secondsDeviation
                Else
                    conversionResult = "0" & hoursCounter & ":" & minuteCounter & ":" & secondsDeviation
                End If
            End If
        Else
            If minuteCounter < 10 Then
                If secondsDeviation < 10 Then
                    conversionResult = hoursCounter & ":0" & minuteCounter & ":0" & secondsDeviation
                Else
                    conversionResult = hoursCounter & ":0" & minuteCounter & ":" & secondsDeviation
                End If
            Else
                If secondsDeviation < 10 Then
                    conversionResult = hoursCounter & ":" & minuteCounter & ":0" & secondsDeviation
                Else
                    conversionResult = hoursCounter & ":" & minuteCounter & ":" & secondsDeviation
                End If
            End If
        End If
        Return conversionResult
    End Function
End Module