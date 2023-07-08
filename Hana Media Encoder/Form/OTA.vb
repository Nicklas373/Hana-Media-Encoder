Imports System.IO
Imports System.Net
Imports Newtonsoft.Json.JsonConvert
Imports Newtonsoft.Json.Linq
Imports Syncfusion.Windows.Forms
Imports Syncfusion.WinForms.Controls
Public Class OTAMenu
    Inherits SfForm
    Dim downloadStats As Boolean = True
    Dim installStats As Boolean = False
    Dim WithEvents WC_Events As New WebClient
    Public Sub OTA_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        AllowRoundedCorners = True
        MessageBoxAdv.MessageBoxStyle = MessageBoxAdv.Style.Metro
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        OTA = WC_Events.DownloadString("https://raw.githubusercontent.com/Nicklas373/Hana-Media-Encoder/master/OTA")
        Parsejson = JObject.Parse(OTA)
        AppRel = Parsejson.SelectToken("release_date").ToString()
        AppVer = Parsejson.SelectToken("version").ToString()
        AppChangelog = DeserializeObject(Parsejson.SelectToken("changelog").ToString)
        DownloadURL = Parsejson.SelectToken("update_url").ToString
        NewParsedVer = AppVer.Split(".")
        CurParsedVer = My.Application.Info.Version.ToString.Split(".")
        MergedNewVer = String.Join(NewParsedVer(0), NewParsedVer(1), NewParsedVer(2))
        MergedCurVer = String.Join(CurParsedVer(0), CurParsedVer(1), CurParsedVer(2))
        For Each item As String In AppChangelog
            Parsedchangelog &= item & vbCrLf
        Next
        Label7.Text = AppVer.ToString
        Label5.Text = AppRel.ToString
        RichTextBox1.Text = "What's New:" & vbCrLf & vbCrLf & Parsedchangelog
    End Sub
    Private Sub UpdateNow(sender As Object, e As EventArgs) Handles Button1.Click
        If installStats = True Then
            If File.Exists(My.Application.Info.DirectoryPath & "\OTA.bat") Then
                GC.Collect()
                GC.WaitForPendingFinalizers()
                File.Delete(My.Application.Info.DirectoryPath & "\OTA.bat")
            End If
            HMEGenerate(My.Application.Info.DirectoryPath & "\OTA.bat", "C:", My.Application.Info.DirectoryPath, "HME.msi", "")
            RunProcAlt(My.Application.Info.DirectoryPath & "\OTA.bat")
        Else
            If downloadStats = False Then
                MessageBoxAdv.Show("Download still on progress, please wait !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If File.Exists(My.Application.Info.DirectoryPath & "\HME.msi") Then
                    GC.Collect()
                    GC.WaitForPendingFinalizers()
                    File.Delete(My.Application.Info.DirectoryPath & "\HME.msi")
                End If
                WC_Events.DownloadFileAsync(New Uri(DownloadURL), My.Application.Info.DirectoryPath & "\HME.msi")
            End If
        End If
    End Sub
    Private Sub WC_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs) Handles WC_Events.DownloadProgressChanged
        downloadStats = False
        Button1.Text = "Downloading... " & e.ProgressPercentage & "%"
        If e.ProgressPercentage = 100 Then
            ProgPercentage = 100
            MessageBoxAdv.Show("Download Complete !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Button1.Text = "Install Update"
            installStats = True
            downloadStats = True
        End If
    End Sub
End Class