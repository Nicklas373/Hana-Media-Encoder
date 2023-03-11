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
    Private sub InitDir()
        Dim retReqDir As String() = {"audioConfig", "audioStream", "videoConfig", "videoStream", "thumbnail", "queue", "queue\audio",
                                     "queue\audio\audioStream", "queue\audio\audioConfig", "queue\video", "queue\video\videoStream",
                                     "queue\video\videoConfig", "chapterConfig", "muxConfig", "trimConfig", "HME-Engine"}
        For Each req_dir As String In retReqDir
            Label1.Text = "Loading: " & req_dir
            Thread.Sleep(100)
            Refresh()
            If Directory.Exists(My.Application.Info.DirectoryPath & "\" & req_dir) = False Then
                Thread.Sleep(100)
                Refresh()
                Directory.CreateDirectory(My.Application.Info.DirectoryPath & "\" & req_dir)
            End If
        Next
    End sub
    Private Function InitFFMPEG() As String
        Dim retFFMPEGConf As String
        Dim retFFMPEGLetter As String
        Dim retStats As String
        Dim retMessage As String
        Dim ffBinaryLoad As String() = {"ffmpeg.exe", "ffplay.exe", "ffprobe.exe"}
        Dim ffBinaryValidityLoad As String() = {"ffmpeg", "ffplay", "ffprobe"}
        Dim ffBinaryMissLoad As New List(Of String)()
        Dim ffBinaryValidityMissLoad As New List(Of String)()
        Dim ffBinaryCount As Integer = 3
        Dim ffBinaryValidityCount As Integer = 3
        If File.Exists(My.Application.Info.DirectoryPath & "\config.ini") Then
            FfmpegConfig = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "FFMPEG Binary:")
            Hwdefconfig = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "GPU Engine:")
            If FfmpegConfig = "null" Then
                retStats = "False"
                retMessage = "FFMPEG configuration was not found !" & vbCrLf & vbCrLf & "Please configure it on options menu !"
            Else
                retFFMPEGConf = FfmpegConfig.Remove(0, 14) & "\"
                retFFMPEGLetter = String.Concat(retFFMPEGConf.AsSpan(0, 1), ":")
                For Each ffBin As String In ffBinaryLoad
                    Label1.Text = "Loading: " & ffBin
                    Thread.Sleep(200)
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
                            retStats = True
                            retMessage = ""
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