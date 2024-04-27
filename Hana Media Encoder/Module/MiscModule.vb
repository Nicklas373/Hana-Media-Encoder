Imports System.IO
Imports System.Management
Module MiscModule
    Public Function FindConfig(confpath As String, contains As String) As String
        Dim value As String
        Using sReader As New StreamReader(confpath)
            While Not sReader.EndOfStream
                Dim line As String = sReader.ReadLine()
                If line.Contains(contains) Then
                    If line IsNot "" Then
                        value = line
                        Return value
                    Else
                        value = "null"
                        Return value
                    End If
                End If
            End While
            value = "null"
            Return value
        End Using
    End Function
    Public Function GetFileSize(file As String) As String
        Dim srcFile As String
        If file = "" Then
            srcFile = ""
            Return srcFile
        Else
            Dim newFile As New FileInfo(file)
            If newFile.Exists Then
                Dim fileSize As Double = newFile.Length / 1024 / 1024 / 1024
                If fileSize < 1.0 Then
                    Dim newFileSize As Double = newFile.Length / 1024 / 1024
                    If newFileSize < 1.0 Then
                        Dim smallFileSize As Double = newFile.Length / 1024
                        srcFile = Format(newFileSize, "###.##").ToString & " KB"
                    Else
                        srcFile = Format(newFileSize, "###.##").ToString & " MB"
                    End If
                Else
                    srcFile = Format(fileSize, "###.##").ToString & " GB"
                End If
                Return srcFile
            Else
                srcFile = "File not found"
                Return srcFile
            End If
        End If
    End Function
    Public Function GetFileSizeAlt(file As Integer) As String
        Dim fileSize As Double = file / 1000 / 1000
        Dim humanFileSize As String
        If fileSize < 1.0 Then
            Dim newFileSize As Double = file / 1000
            If newFileSize < 1.0 Then
                humanFileSize = Format(file, "###.##").ToString & " KB"
            Else
                humanFileSize = Format(newFileSize, "###.##").ToString & " MB"
            End If
        Else
            humanFileSize = Format(fileSize, "###.##").ToString & " GB"
        End If
        Return humanFileSize
    End Function
    Public Function getBetween(ByVal strSource As String, ByVal strStart As String, ByVal strEnd As String) As String
        If strSource = "" Then
            Return ""
        Else
            If strSource.Contains(strStart) AndAlso strSource.Contains(strEnd) Then
                Dim Start, [End] As Integer
                Start = strSource.IndexOf(strStart, 0) + strStart.Length
                If (strSource.IndexOf(strEnd, Start)) < 0 Then
                    Return ""
                Else
                    [End] = strSource.IndexOf(strEnd, Start)
                    Return strSource.Substring(Start, [End] - Start)
                End If
            End If
        End If
        Return ""
    End Function
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
    Public Function NVENCCCompatibility() As Boolean
        Dim graphicsName As String = GetGraphicsCardName("Name")

        If graphicsName.Contains("NVIDIA") Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub InitMedia(mediaFile As Control)
        Dim sCmdLine As String = Environment.CommandLine()
        If sCmdLine.Equals("") = False Then
            Dim iPos = sCmdLine.IndexOf("""", 2)
            Dim sCmdLineArgs = sCmdLine.Substring(iPos + 1).Trim()
            mediaFile.Text = sCmdLineArgs.Replace(Chr(34), "")
        End If
    End Sub
    Public Sub NotifyIcon(NotifyTitle As String, NotifyText As String, NotifyTimeout As Integer, NotifyStatus As Boolean)
        Dim NotifyIcon As New NotifyIcon With {
           .Icon = New Icon(NotifyIcoPath),
           .Visible = True
        }
        If NotifyStatus = True Then
            NotifyIcon.ShowBalloonTip(NotifyTimeout, NotifyTitle, NotifyText, ToolTipIcon.[Info])
        ElseIf NotifyStatus = False Then
            NotifyIcon.ShowBalloonTip(NotifyTimeout, NotifyTitle, NotifyText, ToolTipIcon.[Error])
        End If
    End Sub
    Public Sub MassDelete(dirname As String, ext As String)
        For Each deleteFile In Directory.GetFiles(dirname, "*." & ext, SearchOption.TopDirectoryOnly)
            If File.Exists(deleteFile) Then
                File.Delete(deleteFile)
            End If
        Next
    End Sub
    Public Sub OnCompleted(cmbx As String)
        If cmbx = "Do Nothing" Then

        ElseIf cmbx = "Exit Application" Then
            CleanEnv("all")
            InitExit("")
        ElseIf cmbx = "Log Out" Then
            Shell("Shutdown -l -t 5 -c " & Chr(34) & "Your computer will log out after 5 seconds" & Chr(34))
        ElseIf cmbx = "Restart" Then
            Shell("Shutdown -r -t 5 -c " & Chr(34) & "Your computer will restart out after 5 seconds" & Chr(34))
        ElseIf cmbx = "Shutdown" Then
            Shell("Shutdown -s -t 5 -c " & Chr(34) & "Your computer will shutdown out after 5 seconds" & Chr(34))
        End If
    End Sub
    Public Function RemoveWhitespace(fullString As String) As String
        Return New String(fullString.Where(Function(x) Not Char.IsWhiteSpace(x)).ToArray())
    End Function
    Public Sub Tooltip(control As Control, subtitle As String)
        Dim toolTip As New ToolTip With {
            .AutoPopDelay = 5000,
            .IsBalloon = True,
            .InitialDelay = 1000,
            .ReshowDelay = 500,
            .ShowAlways = True,
            .ToolTipTitle = "Information",
            .UseFading = True,
            .UseAnimation = True
        }
        toolTip.SetToolTip(control, subtitle)
    End Sub
    Public Function getMediaEngineVersion(mediaEngine As String) As String
        Dim version As String
        Dim cmdArgs As String
        Dim mediaResult As String
        Dim mediaArgs As String
        Dim mediaConf As String
        Dim mediaLetter As String

        FfmpegConfig = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "FFMPEG Binary:")
        NVENCCBinary = FindConfig(My.Application.Info.DirectoryPath & "\config.ini", "NVENCC Binary:")

        If mediaEngine = "NVENCC" Then
            mediaArgs = "nvencc64"
            cmdArgs = mediaArgs & " --version 2>&1"
            mediaConf = NVENCCBinary.Remove(0, 14) & "\"
            mediaLetter = mediaConf.Substring(0, 1) & ":"
        ElseIf mediaEngine = "FFMPEG" Then
            mediaArgs = "ffmpeg"
            cmdArgs = mediaArgs & " -version 2>&1"
            mediaConf = FfmpegConfig.Remove(0, 14) & "\"
            mediaLetter = String.Concat(mediaConf.AsSpan(0, 1), ":")
        Else
            mediaArgs = "ffmpeg"
            cmdArgs = mediaArgs & " -version 2>&1"
            mediaConf = FfmpegConfig.Remove(0, 14) & "\"
            mediaLetter = String.Concat(mediaConf.AsSpan(0, 1), ":")
        End If

        HMEGenerateAlt(My.Application.Info.DirectoryPath & "\HME_ME_Version.bat", mediaLetter, Chr(34) & mediaConf & Chr(34), cmdArgs, "")
        Dim psi As New ProcessStartInfo(My.Application.Info.DirectoryPath & "\HME_ME_Version.bat") With {
            .RedirectStandardError = False,
            .RedirectStandardOutput = True,
            .CreateNoWindow = True,
            .WindowStyle = ProcessWindowStyle.Hidden,
            .UseShellExecute = False
        }
        Dim process As Process = Process.Start(psi)
        While Not process.StandardOutput.EndOfStream
            mediaResult = process.StandardOutput.ReadToEnd
            If mediaEngine = "FFMPEG" Then
                If RemoveWhitespace(getBetween(mediaResult, "ffmpeg", "Copyright")) IsNot "" Then
                    version = getBetween(mediaResult, "ffmpeg", "Copyright")
                Else
                    version = ""
                End If
            ElseIf mediaEngine = "NVENCC" Then
                If RemoveWhitespace(getBetween(mediaResult, "NVEncC", "by rigaya")) IsNot "" Then
                    version = getBetween(mediaResult, "NVEncC", "by rigaya")
                Else
                    version = ""
                End If
            Else
                If RemoveWhitespace(getBetween(mediaResult, "ffmpeg", "Copyright")) IsNot "" Then
                    version = getBetween(mediaResult, "ffmpeg", "Copyright")
                Else
                    version = ""
                End If
            End If
        End While

        Return version
    End Function
End Module