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
        Dim initDirRes As String
        Dim initFFMPEGRes As String
        Label2.Text = "Copyright (C) " & My.Application.Info.Copyright
        Label3.Text = "Created by Dicky Herlambang"
        Label4.Text = "v" & Strings.Left(My.Application.Info.Version.ToString, 5)
        Me.Refresh()
        initDirRes = Strings.Mid(InitDir(), 6)
        Me.Refresh()
        initFFMPEGRes = Strings.Mid(InitFFMPEG(), 6)
        Dim EndResult As String = "START: " & initDirRes & " | " & initFFMPEGRes & " :END"
        GC.Collect()
        GC.WaitForPendingFinalizers()
        File.Delete("Init_Res.txt")
        File.Create("Init_Res.txt").Dispose()
        Dim writer As New StreamWriter("Init_Res.txt", True)
        writer.WriteLine(EndResult)
        writer.Close()
        Me.Hide()
    End Sub
    Private Function InitDir() As String
        Dim retReqDir As String() = {"audioConfig", "audioStream", "videoConfig", "videoStream", "thumbnail"}
        For Each req_dir As String In retReqDir
            Label1.Text = "Loading: " & req_dir
            Thread.Sleep(200)
            Me.Refresh()
            If Directory.Exists(req_dir) = False Then
                Thread.Sleep(200)
                Me.Refresh()
                Directory.CreateDirectory(req_dir)
            End If
        Next
    End Function
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
        If File.Exists("config.ini") Then
            FfmpegConfig = FindConfig("config.ini", "FFMPEG Binary:")
            Hwdefconfig = FindConfig("config.ini", "GPU Engine:")
            If FfmpegConfig = "null" Then
                retStats = "False"
                retMessage = "FFMPEG configuration was not found !" & vbCrLf & vbCrLf & "Please configure it on options menu !"
            Else
                retFFMPEGConf = FfmpegConfig.Remove(0, 14) & "\"
                retFFMPEGLetter = String.Concat(retFFMPEGConf.AsSpan(0, 1), ":")
                For Each ffBin As String In ffBinaryLoad
                    Label1.Text = "Loading: " & ffBin
                    Thread.Sleep(200)
                    Me.Refresh()
                    If File.Exists(retFFMPEGConf & ffBin) = False Then
                        ffBinaryCount += 1
                        ffBinaryMissLoad.Add(ffBin)
                    End If
                Next
                If ffBinaryCount > 3 Then
                    retStats = "False"
                    retMessage = "Failed to find FFMPEG Binary !" & vbCrLf & vbCrLf & "Missing FFMPEG Binary: " & String.Join(" , ", ffBinaryMissLoad)
                Else
                    For Each ffres As String In ffBinaryValidityLoad
                        Newffargs = ffres & " -hide_banner -version 2>&1"
                        HMEGenerateAlt("HME_FFMPEG_Init.bat", retFFMPEGLetter, Chr(34) & retFFMPEGConf & Chr(34), newffargs, "")
                        Dim psi As New ProcessStartInfo("HME_FFMPEG_Init.bat") With {
                            .RedirectStandardError = False,
                            .RedirectStandardOutput = True,
                            .CreateNoWindow = True,
                            .WindowStyle = ProcessWindowStyle.Hidden,
                            .UseShellExecute = False
                        }
                        Dim process As Process = Process.Start(psi)
                        While Not process.StandardOutput.EndOfStream
                            Newffres = process.StandardOutput.ReadToEnd
                            If Strings.Left(newffres, 2).Equals("ff") = False Then
                                ffBinaryValidityCount += 1
                                ffBinaryValidityMissLoad.Add(ffres)
                            End If
                        End While
                    Next
                    If ffBinaryValidityCount > 3 Then
                        retStats = "False"
                        retMessage = "Failed to initialize FFMPEG Binary !" & vbCrLf & vbCrLf & "Invalid FFMPEG Binary: " & String.Join(" , ", ffBinaryValidityMissLoad)
                    Else
                        If hwdefConfig = "GPU Engine:" Then
                            retStats = "False"
                            retMessage = "GPU HW Encoder was not configured !" & vbCrLf & vbCrLf & "Please configure it on options menu, native encoding are not supported yet !"
                        Else
                            retStats = True
                            retMessage = ""
                        End If
                    End If
                End If
            End If
        Else
            retStats = "False"
            retMessage = "Config file was not found !" & vbCrLf & vbCrLf & "Please configure it on options menu !"
        End If
        Return retStats & retMessage
    End Function
End Class