Imports Syncfusion.Windows.Forms
Imports Syncfusion.WinForms.Controls
Public Class InformationMenu
    Inherits SfForm
    Private Sub InformationMenu_load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        Style.TitleBar.TextHorizontalAlignment = HorizontalAlignment.Center
        Style.TitleBar.TextVerticalAlignment = VisualStyles.VerticalAlignment.Center
        MessageBoxAdv.MessageBoxStyle = MessageBoxAdv.Style.Metro
        Video_pnl_2.Visible = False
        Video_pnl_3.Visible = False
        Video_pnl_4.Visible = False
        Video_pnl_5.Visible = False
        Video_pnl_6.Visible = False
        Audio_pnl_2.Visible = False
    End Sub
    Private Sub audioTab(sender As Object, e As EventArgs) Handles TabControl1.Click
        TabControl1.SelectedTab = TabPage1
    End Sub
    Private Sub Next_Btn_VideoConf(sender As Object, e As EventArgs) Handles Button7.Click
        If Vid_pnl_1.Visible = True And Video_pnl_2.Visible = False Then
            Video_pnl_2.Visible = True
        ElseIf Video_pnl_2.Visible = True And Video_pnl_3.Visible = False Then
            Video_pnl_3.Visible = True
        ElseIf Video_pnl_3.Visible = True And Video_pnl_4.Visible = False Then
            Video_pnl_4.Visible = True
        ElseIf Video_pnl_4.Visible = True And Video_pnl_5.Visible = False Then
            Video_pnl_5.Visible = True
        ElseIf Video_pnl_5.Visible = True And Video_pnl_6.Visible = False Then
            Video_pnl_6.Visible = True
        End If
    End Sub
    Private Sub Prev_Btn_VideoConf(sender As Object, e As EventArgs) Handles Button8.Click
        If Video_pnl_5.Visible = True And Video_pnl_6.Visible = True Then
            Video_pnl_6.Visible = False
        ElseIf Video_pnl_5.Visible = True And Video_pnl_6.Visible = False Then
            Video_pnl_5.Visible = False
        ElseIf Video_pnl_4.Visible = True And Video_pnl_5.Visible = False Then
            Video_pnl_4.Visible = False
        ElseIf Video_pnl_3.Visible = True And Video_pnl_4.Visible = False Then
            Video_pnl_3.Visible = False
        ElseIf Video_pnl_2.Visible = True And Video_pnl_3.Visible = False Then
            Video_pnl_2.Visible = False
        End If
    End Sub
    Private Sub Video_CDC_Btn(sender As Object, e As EventArgs) Handles Button1.Click
        TabControl1.SelectedTab = TabPage1
    End Sub
    Private Sub Audio_CDC_Btn(sender As Object, e As EventArgs) Handles Button2.Click
        TabControl1.SelectedTab = TabPage2
    End Sub
    Private Sub Prev_Btn_AudioConf(sender As Object, e As EventArgs) Handles Button3.Click
        If Audio_pnl_2.Visible = True Then
            Audio_pnl_2.Visible = False
        End If
    End Sub
    Private Sub Next_Btn_AudioConf(sender As Object, e As EventArgs) Handles Button4.Click
        If Audio_pnl_2.Visible = False Then
            Audio_pnl_2.Visible = True
        End If
    End Sub
End Class