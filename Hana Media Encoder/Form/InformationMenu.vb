Imports Syncfusion.Windows.Forms
Imports Syncfusion.WinForms.Controls
Public Class InformationMenu
    Inherits SfForm
    Dim vid_btn_state As Boolean = False
    Dim aud_btn_state As Boolean = False
    Private Sub InformationMenu_load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        MessageBoxAdv.MessageBoxStyle = MessageBoxAdv.Style.Metro
        Video_pnl_2.Visible = False
        Video_pnl_3.Visible = False
        Video_pnl_4.Visible = False
        Video_pnl_5.Visible = False
        Video_pnl_6.Visible = False
        Audio_pnl_2.Visible = False
    End Sub
    Private Sub Next_Btn_VideoConf(sender As Object, e As EventArgs) Handles Button7.Click
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
            ElseIf Video_pnl_6.Visible = True Then
                MessageBoxAdv.Show("Already on the last page !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        ElseIf vid_btn_state = False And aud_btn_state = True Then
            If Audio_pnl_2.Visible = False Then
                Audio_pnl_1.AutoScroll = False
                Audio_pnl_2.Visible = True
                Label172.Text = CInt(Label172.Text) + 1
            ElseIf Audio_pnl_2.Visible = True Then
                MessageBoxAdv.Show("Already on the last page !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        ElseIf vid_btn_state = False And aud_btn_state = False Then
            MessageBoxAdv.Show("Please select video or audio codec information", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub Prev_Btn_VideoConf(sender As Object, e As EventArgs) Handles Button8.Click
        If vid_btn_state = True And aud_btn_state = False Then
            If Video_pnl_5.Visible = True And Video_pnl_6.Visible = True Then
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
            ElseIf Video_pnl_2.Visible = False Then
                MessageBoxAdv.Show("Already on the first page !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        ElseIf vid_btn_state = False And aud_btn_state = True Then
            If Audio_pnl_2.Visible = True Then
                Audio_pnl_2.Visible = False
                Audio_pnl_2.AutoScroll = False
                Audio_pnl_1.AutoScroll = True
                Label172.Text = CInt(Label172.Text) - 1
            ElseIf Audio_pnl_2.Visible = False Then
                MessageBoxAdv.Show("Already on the first page !", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        ElseIf vid_btn_state = False And aud_btn_state = False Then
            MessageBoxAdv.Show("Please select video or audio codec information", "Hana Media Encoder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub Video_CDC_Btn(sender As Object, e As EventArgs) Handles Button1.Click
        Audio_pnl_2.Visible = False
        Audio_pnl_1.Visible = False
        Audio_pnl_2.AutoScroll = False
        Audio_pnl_1.AutoScroll = False
        Video_pnl_6.Visible = False
        Video_pnl_5.Visible = False
        Video_pnl_4.Visible = False
        Video_pnl_3.Visible = False
        Video_pnl_2.Visible = False
        Vid_pnl_1.Visible = True
        vid_btn_state = True
        aud_btn_state = False
        Label172.Text = "1"
        Label170.Text = "6"
    End Sub
    Private Sub Audio_CDC_Btn(sender As Object, e As EventArgs) Handles Button2.Click
        Audio_pnl_2.Visible = False
        Video_pnl_6.AutoScroll = False
        Video_pnl_5.AutoScroll = False
        Video_pnl_4.AutoScroll = False
        Audio_pnl_1.Visible = True
        Audio_pnl_1.AutoScroll = True
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
    End Sub
End Class