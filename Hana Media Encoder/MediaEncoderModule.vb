Imports System.IO
Imports System.Management
Module MediaEncoderModule
    Public Function GetGraphicsCardName(gpuProperty As String) As String
        Dim searcher As ManagementObjectSearcher = New ManagementObjectSearcher("SELECT * FROM Win32_VideoController")
        Dim graphicsCard As String = String.Empty
        For Each mo As ManagementObject In searcher.[Get]()
            For Each [property] As PropertyData In mo.Properties
                If [property].Name = gpuProperty Then
                    graphicsCard = [property].Value.ToString()
                End If
            Next
        Next
        Return graphicsCard
    End Function
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
    Public Function TimeConversion(Hours As Integer, Minute As Integer, Seconds As Integer) As Integer
        Dim hoursToMinute As Integer = Hours * 60
        Dim minuteToSeconds As Integer = (hoursToMinute + Minute) * 60
        Dim newSeconds As Integer = minuteToSeconds + Seconds
        Return newSeconds
    End Function
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
                                            ratectr As String, spatialaq As String, aqstrength As String, temporalaq As String, targetql As String,
                                            tier As String, tune As String)
        If File.Exists(HMEName) Then
            GC.Collect()
            GC.WaitForPendingFinalizers()
            File.Delete(HMEName)
            File.Create(HMEName).Dispose()
        Else
            File.Create(HMEName).Dispose()
        End If
        Dim writer As New StreamWriter(HMEName, True)
        writer.WriteLine("BRCompat=" & brCompat)
        writer.WriteLine("OvrBitrate=" & ovrbitrate)
        writer.WriteLine("Bref=" & bref)
        writer.WriteLine("Codec=" & codec)
        writer.WriteLine("Fps=" & framerate)
        writer.WriteLine("Level=" & level)
        writer.WriteLine("MaxBitrate=" & maxbitrate)
        writer.WriteLine("Multipass=" & multipass)
        writer.WriteLine("Preset=" & preset)
        writer.WriteLine("PixelFormat=" & pixfmt)
        writer.WriteLine("Profile=" & profile)
        writer.WriteLine("RateControl=" & ratectr)
        writer.WriteLine("SpatialAQ=" & spatialaq)
        writer.WriteLine("AQStrength=" & aqstrength)
        writer.WriteLine("TemporalAQ=" & temporalaq)
        writer.WriteLine("TargetQL=" & targetql)
        writer.WriteLine("Tier=" & tier)
        writer.WriteLine("Tune=" & tune)
        writer.Close()
    End Sub
    Public Sub HMEAudioStreamConfigGenerate(HMEName As String, acodec As String, abitdepth As String, aratecontrol As String,
                                             arate As String, achannel As String, acomplvl As String, afreq As String)
        If File.Exists(HMEName) Then
            GC.Collect()
            GC.WaitForPendingFinalizers()
            File.Delete(HMEName)
            File.Create(HMEName).Dispose()
        Else
            File.Create(HMEName).Dispose()
        End If
        Dim writer As New StreamWriter(HMEName, True)
        writer.WriteLine("Codec=" & acodec)
        writer.WriteLine("BitDepth=" & abitdepth)
        writer.WriteLine("RateControl=" & aratecontrol)
        writer.WriteLine("Rate=" & arate)
        writer.WriteLine("Channel=" & achannel)
        writer.WriteLine("Compression=" & acomplvl)
        writer.WriteLine("Frequency=" & afreq)
        writer.Close()
    End Sub
    Public Sub previewMediaModule(mediaFile As String, ffplay As String)
        Dim psi As New ProcessStartInfo(ffplay) With {
               .RedirectStandardError = False,
               .RedirectStandardOutput = True,
               .CreateNoWindow = True,
               .Arguments = "-x 960 -y 540 " & Chr(34) & mediaFile & Chr(34),
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
    Public Sub CleanEnv(cleanStats As String)
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete("HME.bat")
        File.Delete("HME_VF.bat")
        File.Delete("HME_Audio_Flags.txt")
        If cleanStats = "all" Then
            File.Delete("chapter.txt")
            File.Delete("FFMETADATAFILE")
            MassDelete("audioStream", "txt")
            MassDelete("audioConfig", "txt")
            MassDelete("videoStream", "txt")
            MassDelete("videoConfig", "txt")
        End If
        If File.Exists("thumbnail\1.png") Then
            File.Delete("thumbnail\1.png")
            File.Delete("thumbnail\2.png")
        End If
    End Sub
End Module