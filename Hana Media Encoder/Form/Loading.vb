Imports System.IO
Imports Syncfusion.WinForms.Controls
Public Class Loading
    Inherits SfForm
    Public Sub Loading_load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        AllowRoundedCorners = True
        CenterForm(MainMenu)
        ProgressBar1.Style = ProgressBarStyle.Marquee
        ProgressBar1.MarqueeAnimationSpeed = 40
        ProgressBar1.Refresh()
    End Sub
    Public Sub New(mediaType As String, mediaTitle As String)
        InitializeComponent()
        Me.Refresh()
        If mediaType = "Frame" Then
            Label1.Text = "Calculate frame size"
        ElseIf mediaType = "Reset" Then
            Label1.Text = "Re-setting to initial state"
        Else
            Label1.Text = "Loading " & mediaType & " : " & Path.GetFileName(mediaTitle)
        End If
        Me.Refresh()
    End Sub
    Public Shared Sub CenterForm(ByVal frm As Form, Optional ByVal parent As Form = Nothing)
        '' Note: call this from frm's Load event!
        Dim r As Rectangle
        If parent IsNot Nothing Then
            r = parent.RectangleToScreen(parent.ClientRectangle)
        Else
            r = Screen.FromPoint(frm.Location).WorkingArea
        End If

        Dim x = r.Left + (r.Width - frm.Width) \ 2
        Dim y = r.Top + (r.Height - frm.Height) \ 2
        frm.Location = New Point(x, y)
    End Sub
End Class