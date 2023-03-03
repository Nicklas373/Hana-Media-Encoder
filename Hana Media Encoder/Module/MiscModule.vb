Imports System.IO
Module MiscModule
    Public Function FindConfig(confpath As String, contains As String) As String
        Dim value As String
        Using sReader As New StreamReader(confpath)
            While Not sReader.EndOfStream
                Dim line As String = sReader.ReadLine()
                If line.Contains(contains) Then
                    value = line
                    Return value
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
    Public Function getBetween(ByVal strSource As String, ByVal strStart As String, ByVal strEnd As String) As String
        If strSource = "" Then
            Return "FAIL"
        Else
            If strSource.Contains(strStart) AndAlso strSource.Contains(strEnd) Then
                Dim Start, [End] As Integer
                Start = strSource.IndexOf(strStart, 0) + strStart.Length
                If (strSource.IndexOf(strEnd, Start)) < 0 Then
                    MsgBox("End Error")
                    Return "End Error"
                Else
                    [End] = strSource.IndexOf(strEnd, Start)
                    Return strSource.Substring(Start, [End] - Start)
                End If
            End If
        End If
        Return ""
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
            InitExit()
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
End Module