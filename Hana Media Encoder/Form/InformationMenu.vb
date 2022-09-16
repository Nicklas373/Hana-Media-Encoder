Imports Syncfusion.Windows.Forms
Imports Syncfusion.WinForms.Controls
Public Class InformationMenu
    Inherits SfForm
    Dim vid_btn_state As Boolean = False
    Dim aud_btn_state As Boolean = False
    Private Sub InformationMenu_load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        MessageBoxAdv.MessageBoxStyle = MessageBoxAdv.Style.Metro
        InitMedia_Info("video")
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        If keyData = Keys.Left Then
            PrevBtn()
            Return True
        ElseIf keyData = Keys.Right Then
            NextBtn()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
    Private Sub Next_Btn_VideoConf(sender As Object, e As EventArgs) Handles Button7.Click
        NextBtn()
    End Sub
    Private Sub Prev_Btn_VideoConf(sender As Object, e As EventArgs) Handles Button8.Click
        PrevBtn()
    End Sub
    Private Sub Video_CDC_Btn(sender As Object, e As EventArgs) Handles Button1.Click
        InitMedia_Info("video")
    End Sub
    Private Sub Audio_CDC_Btn(sender As Object, e As EventArgs) Handles Button2.Click
        InitMedia_Info("audio")
    End Sub
    Private Sub InitMedia_Info(mediaType As String)
        If mediaType = "video" Then
            Audio_pnl_2.Visible = False
            Audio_pnl_1.Visible = False
            Audio_pnl_2.AutoScroll = False
            Audio_pnl_1.AutoScroll = False
            Video_pnl_7.Visible = False
            Video_pnl_6.Visible = False
            Video_pnl_5.Visible = False
            Video_pnl_4.Visible = False
            Video_pnl_3.Visible = False
            Video_pnl_2.Visible = False
            Vid_pnl_1.Visible = True
            vid_btn_state = True
            aud_btn_state = False
            Label172.Text = "1"
            Label170.Text = "7"
        ElseIf mediaType = "audio" Then
            Audio_pnl_2.Visible = False
            Video_pnl_7.AutoScroll = False
            Video_pnl_6.AutoScroll = False
            Video_pnl_5.AutoScroll = False
            Video_pnl_4.AutoScroll = False
            Audio_pnl_1.Visible = True
            Audio_pnl_1.AutoScroll = True
            Video_pnl_7.Visible = True
            Video_pnl_6.Visible = True
            Video_pnl_5.Visible = True
            Video_pnl_4.Visible = True
            Video_pnl_3.Visible = True
            Video_pnl_2.Visible = True
            Vid_pnl_1.Visible = True
            vid_btn_state = False
            aud_btn_state = True
            Label172.Text = "1"
            Label170.Text = "2"
        End If
    End Sub
    Private Sub NextBtn()
        If vid_btn_state = True And aud_btn_state = False Then
            If Vid_pnl_1.Visible = True And Video_pnl_2.Visible = False Then
                Video_pnl_2.Visible = True
                Label172.Text = CInt(Label172.Text) + 1
            ElseIf Video_pnl_2.Visible = True And Video_pnl_3.Visible = False Then
                Video_pnl_3.Visible = True
                Label172.Text = CInt(Label172.Text) + 1
            ElseIf Video_pnl_3.Visible = True And Video_pnl_4.Visible = False Then
                Video_pnl_4.Visible = True
                Video_pnl_4.AutoScroll = True
                Label172.Text = CInt(Label172.Text) + 1
            ElseIf Video_pnl_4.Visible = True And Video_pnl_5.Visible = False Then
                Video_pnl_4.AutoScroll = False
                Video_pnl_5.Visible = True
                Video_pnl_5.AutoScroll = True
                Label172.Text = CInt(Label172.Text) + 1
            ElseIf Video_pnl_5.Visible = True And Video_pnl_6.Visible = False Then
                Video_pnl_5.AutoScroll = False
                Video_pnl_6.AutoScroll = True
                Video_pnl_6.Visible = True
                Label172.Text = CInt(Label172.Text) + 1
            ElseIf Video_pnl_6.Visible = True And Video_pnl_7.Visible = False Then
                Video_pnl_6.AutoScroll = False
                Video_pnl_7.AutoScroll = True
                Video_pnl_7.Visible = True
                Label172.Text = CInt(Label172.Text) + 1
            End If
        ElseIf vid_btn_state = False And aud_btn_state = True Then
            If Audio_pnl_2.Visible = False Then
                Audio_pnl_1.AutoScroll = False
                Audio_pnl_2.Visible = True
                Label172.Text = CInt(Label172.Text) + 1
            End If
        End If
    End Sub
    Private Sub PrevBtn()
        If vid_btn_state = True And aud_btn_state = False Then
            If Video_pnl_6.Visible = True And Video_pnl_7.Visible = True Then
                Video_pnl_7.Visible = False
                Video_pnl_7.AutoScroll = False
                Video_pnl_6.AutoScroll = True
                Label172.Text = CInt(Label172.Text) - 1
            ElseIf Video_pnl_5.Visible = True And Video_pnl_6.Visible = True Then
                Video_pnl_6.Visible = False
                Video_pnl_6.AutoScroll = False
                Video_pnl_5.AutoScroll = True
                Label172.Text = CInt(Label172.Text) - 1
            ElseIf Video_pnl_5.Visible = True And Video_pnl_6.Visible = False Then
                Video_pnl_5.AutoScroll = False
                Video_pnl_5.Visible = False
                Video_pnl_4.AutoScroll = True
                Label172.Text = CInt(Label172.Text) - 1
            ElseIf Video_pnl_4.Visible = True And Video_pnl_5.Visible = False Then
                Video_pnl_4.AutoScroll = False
                Video_pnl_4.Visible = False
                Label172.Text = CInt(Label172.Text) - 1
            ElseIf Video_pnl_3.Visible = True And Video_pnl_4.Visible = False Then
                Video_pnl_3.Visible = False
                Label172.Text = CInt(Label172.Text) - 1
            ElseIf Video_pnl_2.Visible = True And Video_pnl_3.Visible = False Then
                Video_pnl_2.Visible = False
                Label172.Text = CInt(Label172.Text) - 1
            End If
        ElseIf vid_btn_state = False And aud_btn_state = True Then
            If Audio_pnl_2.Visible = True Then
                Audio_pnl_2.Visible = False
                Audio_pnl_2.AutoScroll = False
                Audio_pnl_1.AutoScroll = True
                Label172.Text = CInt(Label172.Text) - 1
            End If
        End If
    End Sub
End Class