﻿Imports Syncfusion.Windows.Forms
Imports Syncfusion.WinForms.Controls
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports System.IO
Public Class OTAMenu
    Inherits SfForm
    Dim curParsedVer As String()
    Dim downloadURL As String
    Dim installStats As Boolean = False
    Dim newParsedVer As String()
    Dim mergedNewVer As Integer
    Dim mergedCurVer As Integer
    Dim OTA As String
    Dim parsejson As JObject
    Dim parsedchangelog As String = ""
    Dim progPercentage As Integer
    Dim WithEvents WC As New WebClient
    Public Sub OTA_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        Style.TitleBar.TextHorizontalAlignment = HorizontalAlignment.Center
        Style.TitleBar.TextVerticalAlignment = VisualStyles.VerticalAlignment.Center
        MessageBoxAdv.MessageBoxStyle = MessageBoxAdv.Style.Metro
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        OTA = WC.DownloadString("https://raw.githubusercontent.com/Nicklas373/Hana-Media-Encoder/master/OTA")
        parsejson = JObject.Parse(OTA)
        Dim apprel = parsejson.SelectToken("release_date").ToString()
        Dim appver = parsejson.SelectToken("version").ToString()
        downloadURL = parsejson.SelectToken("update_url").ToString
        Dim appchangelog = Newtonsoft.Json.JsonConvert.DeserializeObject(parsejson.SelectToken("changelog").ToString)
        newParsedVer = appver.Split(".")
        curParsedVer = My.Application.Info.Version.ToString.Split(".")
        mergedNewVer = String.Join(newParsedVer(0), newParsedVer(1), newParsedVer(2))
        mergedCurVer = String.Join(curParsedVer(0), curParsedVer(1), curParsedVer(2))
        For Each item As String In appchangelog
            parsedchangelog &= item & vbCrLf
        Next
        Label7.Text = appver.ToString
        Label5.Text = apprel.ToString
        RichTextBox1.Text = "Changelog:" & vbCrLf & vbCrLf & parsedchangelog
    End Sub
    Private Sub UpdateNow(sender As Object, e As EventArgs) Handles Button1.Click
        If installStats = True Then
            If File.Exists("OTA.bat") Then
                GC.Collect()
                GC.WaitForPendingFinalizers()
                File.Delete("OTA.bat")
            End If
            HMEGenerate("OTA.bat", "C:", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HME.msi", "")
            RunProc("OTA.bat")
        Else
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HME.msi") Then
                GC.Collect()
                GC.WaitForPendingFinalizers()
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HME.msi")
            End If
            WC.DownloadFileAsync(New Uri(downloadURL), Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\HME.msi")
        End If
    End Sub
    Private Sub WC_DownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs) Handles WC.DownloadProgressChanged
        Button1.Text = "Downloading... " & e.ProgressPercentage & "%"
        If e.ProgressPercentage = 100 Then
            progPercentage = 100
            MsgBox("Download Complete")
            Button1.Text = "Install Update"
            installStats = True
        End If
    End Sub
End Class