Imports System.IO
Imports System.Threading
Imports Syncfusion.WinForms.Controls
Public Class InitScreen
    Inherits SfForm
    Public Sub SplashLoad(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        AllowRoundedCorners = True
        Height = 600
        Width = 960
        Dim EndResult As String
        Dim initFFMPEGRes As String
        Label2.Text = "Copyright (C) " & My.Application.Info.Copyright
        Label3.Text = "Created by Dicky Herlambang"
        Label4.Text = "v" & Strings.Left(My.Application.Info.Version.ToString, 5)
        Refresh()
        InitDir()
        Refresh()
        initFFMPEGRes = Strings.Mid(InitFFMPEG(), 6)
        If initFFMPEGRes.Equals("") = True Then
            EndResult = ""
        Else
            EndResult = getBetween(initFFMPEGRes, "START1:", ":END1") & vbCrLf & vbCrLf & getBetween(initFFMPEGRes, "START2:", ":END2")
        End If
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete(My.Application.Info.DirectoryPath & "\Init_Res.txt")
        File.Create(My.Application.Info.DirectoryPath & "\Init_Res.txt").Dispose()
        Dim writer As New StreamWriter(My.Application.Info.DirectoryPath & "\Init_Res.txt", True)
        writer.WriteLine(EndResult)
        writer.Close()
        Hide()
    End Sub
    Private Sub InitDir()
        Dim retReqDir As String() = {"audioConfig", "audioStream", "videoConfig", "videoStream", "thumbnail", "queue", "queue\audio",
                                     "queue\audio\audioStream", "queue\audio\audioConfig", "queue\video", "queue\video\videoStream",
                                     "queue\video\videoConfig", "chapterConfig", "muxConfig", "trimConfig", "HME-Engine"}
        For Each req_dir As String In retReqDir
            Label1.Text = "Loading: " & req_dir
            Thread.Sleep(100)
            Refresh()
            If Directory.Exists(My.Application.Info.DirectoryPath & "\" & req_dir) = False Then
                Thread.Sleep(50)
                Refresh()
                Directory.CreateDirectory(My.Application.Info.DirectoryPath & "\" & req_dir)
            End If
        Next
    End Sub
    Private Function InitFFMPEG() As String
        Dim retFFMPEGConf As String
        Dim retFFMPEGLetter As String
        Dim retNVENCCConf As String
        Dim retNVENCCLetter As String
        Dim retStats As String
        Dim retMessage As String
        Dim ffBinaryLoad As String() = {"ffmpeg.exe", "ffplay.exe", "ffprobe.exe"}
        Dim ffBinaryValidityLoad As String() = {"ffmpeg", "ffplay", "ffprobe"}
        Dim ffBinaryMissLoad As New List(Of String)()
        Dim ffBinaryValidityMissLoad As New List(Of String)()
        Dim ffBinaryCount As Integer = 3
        Dim ffBinaryValidityCount As Integer = 3
        Dim nvBinaryLoad As String() = {"NVEncC64.exe", "hdr10plus_gen.exe", "avcodec-60.dll", "avdevice-60.dll", "avfilter-9.dll", "avformat-60.dll", "avutil-58.dll",
                                        "cudart64_110.dll", "libass-9.dll", "libvmaf.dll", "msvcp140.dll", "NVEncNVOFFRUC.dll", "NvOFFRUC.dll", "nvrtc64_101_0.dll", "nvrtc-builtins64_101.dll",
                                        "swresample-4.dll", "vcruntime140.dll", "vcruntime140_1.dll"}
        Dim nvBinaryValidityLoad As String() = {"nvencc64"}
        Dim nvBinaryMissLoad As New List(Of String)()
        Dim nvBinaryValidityMissLoad As New List(Of String)()
        Dim nvBinaryCount As Integer = 18
        Dim nvBinaryValidityCount As Integer = 1
        Dim nvBinaryPath As String
        Dim mediaenginestate As String
        If File.Exists(My.Application.Info.DirectoryPath & "\config.ini") Then
            FfmpegConfig = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "FFMPEG Binary:")
            Hwdefconfig = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "GPU Engine:")
            NVENCCBinary = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "NVENCC Binary:")
            MediaEngine = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "Media Engine:")
            If FfmpegConfig = "null" Then
                retStats = "False"
                retMessage = "START1: FFMPEG configuration was not found ! :END1" & vbCrLf & vbCrLf & "START2: Please configure it on options menu ! :END2"
            ElseIf NVENCCBinary = "null" Then
                retStats = "False"
                retMessage = "START1: NVENCC configuration was not found ! :END1" & vbCrLf & vbCrLf & "START2: Please configure it on options menu ! :END2"
            Else
                If NVENCCBinary = "null" Then
                    nvBinaryPath = ""
                Else
                    nvBinaryPath = NVENCCBinary.Remove(0, 14)
                End If
                If MediaEngine IsNot "null" Then
                    mediaenginestate = MediaEngine.Remove(0, 13)
                Else
                    mediaenginestate = "FFMPEG"
                End If
                If mediaenginestate = "NVENCC" Then
                    retNVENCCConf = NVENCCBinary.Remove(0, 14) & "\"
                    For Each nvBin As String In nvBinaryLoad
                        Label1.Text = "Loading: " & nvBin
                        Thread.Sleep(100)
                        Refresh()
                        If File.Exists(retNVENCCConf & nvBin) = False Then
                            nvBinaryCount += 1
                            nvBinaryMissLoad.Add(nvBin)
                        End If
                    Next
                    retNVENCCConf = NVENCCBinary.Remove(0, 14) & "\"
                    retNVENCCLetter = retNVENCCConf.Substring(0, 1) & ":"
                    If nvBinaryCount > 18 Then
                        retStats = "False"
                        retMessage = "START1: Failed to find NVENCC Binary ! :END1" & vbCrLf & vbCrLf & "START2: Missing NVENCC Binary: " & String.Join(" , ", nvBinaryMissLoad) & " :END2"
                    Else
                        For Each nvres As String In nvBinaryValidityLoad
                            Newffargs = nvres & " --version 2>&1"
                            HMEGenerateAlt(My.Application.Info.DirectoryPath & "\HME_NVENCC_Init.bat", retNVENCCLetter, Chr(34) & retNVENCCConf & Chr(34), Newffargs, "")
                            Dim psi As New ProcessStartInfo(My.Application.Info.DirectoryPath & "\HME_NVENCC_Init.bat") With {
                                .RedirectStandardError = False,
                                .RedirectStandardOutput = True,
                                .CreateNoWindow = True,
                                .WindowStyle = ProcessWindowStyle.Hidden,
                                .UseShellExecute = False
                            }
                            Dim process As Process = Process.Start(psi)
                            While Not process.StandardOutput.EndOfStream
                                Newffres = process.StandardOutput.ReadToEnd
                                If Strings.Left(Newffres, 2).Equals("nv") = False Then
                                    nvBinaryValidityCount += 1
                                    nvBinaryValidityMissLoad.Add(nvres)
                                End If
                            End While
                        Next
                        If nvBinaryValidityCount > 18 Then
                            retStats = "False"
                            retMessage = "START1: Failed to initialize NVENCC Binary ! :END1" & vbCrLf & vbCrLf & "START2: Invalid NVENCC Binary: " & String.Join(" , ", nvBinaryValidityMissLoad) & " :END2"
                        Else
                            If Hwdefconfig = "GPU Engine:" Then
                                retStats = "False"
                                retMessage = "START1: GPU HW Encoder was not configured ! :END1" & vbCrLf & vbCrLf & "START2: Please configure it on options menu, native encoding are not supported yet !" & " :END2"
                            Else
                                retStats = "True"
                                retMessage = ""
                            End If
                        End If
                    End If
                Else
                    retFFMPEGConf = FfmpegConfig.Remove(0, 14) & "\"
                    retFFMPEGLetter = String.Concat(retFFMPEGConf.AsSpan(0, 1), ":")
                    For Each ffBin As String In ffBinaryLoad
                        Label1.Text = "Loading: " & ffBin
                        Thread.Sleep(100)
                        Refresh()
                        If File.Exists(retFFMPEGConf & ffBin) = False Then
                            ffBinaryCount += 1
                            ffBinaryMissLoad.Add(ffBin)
                        End If
                    Next
                    If ffBinaryCount > 3 Then
                        retStats = "False"
                        retMessage = "START1: Failed to find FFMPEG Binary ! :END1" & vbCrLf & vbCrLf & "START2: Missing FFMPEG Binary: " & String.Join(" , ", ffBinaryMissLoad) & " :END2"
                    Else
                        For Each ffres As String In ffBinaryValidityLoad
                            Newffargs = ffres & " -hide_banner -version 2>&1"
                            HMEGenerateAlt(My.Application.Info.DirectoryPath & "\HME_FFMPEG_Init.bat", retFFMPEGLetter, Chr(34) & retFFMPEGConf & Chr(34), Newffargs, "")
                            Dim psi As New ProcessStartInfo(My.Application.Info.DirectoryPath & "\HME_FFMPEG_Init.bat") With {
                                .RedirectStandardError = False,
                                .RedirectStandardOutput = True,
                                .CreateNoWindow = True,
                                .WindowStyle = ProcessWindowStyle.Hidden,
                                .UseShellExecute = False
                            }
                            Dim process As Process = Process.Start(psi)
                            While Not process.StandardOutput.EndOfStream
                                Newffres = process.StandardOutput.ReadToEnd
                                If Strings.Left(Newffres, 2).Equals("ff") = False Then
                                    ffBinaryValidityCount += 1
                                    ffBinaryValidityMissLoad.Add(ffres)
                                End If
                            End While
                        Next
                        If ffBinaryValidityCount > 3 Then
                            retStats = "False"
                            retMessage = "START1: Failed to initialize FFMPEG Binary ! :END1" & vbCrLf & vbCrLf & "START2: Invalid FFMPEG Binary: " & String.Join(" , ", ffBinaryValidityMissLoad) & " :END2"
                        Else
                            If Hwdefconfig = "GPU Engine:" Then
                                retStats = "False"
                                retMessage = "START1: GPU HW Encoder was not configured ! :END1" & vbCrLf & vbCrLf & "START2: Please configure it on options menu, native encoding are not supported yet !" & " :END2"
                            Else
                                retStats = "True"
                                retMessage = ""
                            End If
                        End If
                    End If
                End If
            End If
        Else
            retStats = "False"
            retMessage = "START1: Config file was not found ! :END1" & vbCrLf & vbCrLf & "START2: Please configure it on options menu ! :END2"
        End If
        Return retStats & retMessage
    End Function
End Class