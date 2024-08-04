Imports System.IO
Imports Syncfusion.WinForms.Controls
Public Class EFlagsMenu
    Inherits SfForm
    Public Sub EFlagsLoad(sender As Object, e As EventArgs) Handles MyBase.Load
        AllowTransparency = False
        AllowRoundedCorners = True
        MetroSetComboBox1.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox1.SelectedItemBackColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox1.SelectedItemForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetComboBox1.DisabledBackColor = ColorTranslator.FromHtml("#161B21")
        MetroSetComboBox1.DisabledBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox1.DisabledForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox2.ForeColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox2.SelectedItemBackColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox2.SelectedItemForeColor = ColorTranslator.FromHtml("#161B21")
        MetroSetComboBox2.DisabledBackColor = ColorTranslator.FromHtml("#161B21")
        MetroSetComboBox2.DisabledBorderColor = ColorTranslator.FromHtml("#F4A950")
        MetroSetComboBox2.DisabledForeColor = ColorTranslator.FromHtml("#F4A950")
        MediaNameLoad()
        If MetroSetComboBox1.Items.Count <= 0 Then
            MetroSetComboBox1.Enabled = False
        End If
        If MetroSetComboBox2.Items.Count <= 0 Then
            MetroSetComboBox2.Enabled = False
        End If
    End Sub
    Public Sub MediaNameChanged(sender As Object, e As EventArgs) Handles MetroSetComboBox1.SelectedIndexChanged
        If MetroSetComboBox1.Text.ToString IsNot "" Then
            MediaFlagsLoad()
        End If
    End Sub
    Public Sub MediaFlagsChanged(sender As Object, e As EventArgs) Handles MetroSetComboBox2.SelectedIndexChanged
        If MetroSetComboBox1.Text.ToString IsNot "" Then
            MediaEncodeLoad()
        End If
    End Sub
    Public Sub MediaNameLoad()
        MetroSetComboBox1.Items.Clear()
        If MainMenu.Textbox77.Text.ToString IsNot "" Then
            MetroSetComboBox1.Items.Add(Path.GetFileName(MainMenu.Textbox77.Text.ToString))
        End If
        If MainMenu.MetroSetSwitch6.Switched = True And MainMenu.DataGridView1.Rows.Count > 0 Then
            For i As Integer = 0 To MainMenu.DataGridView1.Rows.Count - 1
                MetroSetComboBox1.Items.Add(MainMenu.DataGridView1.Rows(i).Cells(2).Value.ToString)
            Next
        End If
        If MetroSetComboBox1.Items.Count <= 0 Then
            MetroSetComboBox1.Items.Add("No Media Command Found")
        End If
    End Sub
    Public Sub MediaFlagsLoad()
        MetroSetComboBox2.Enabled = True
        MetroSetComboBox2.Items.Clear()
        If File.Exists(VideoStreamFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
            MetroSetComboBox2.Items.Add("Video Command")
        ElseIf File.Exists(VideoQueueFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
            MetroSetComboBox2.Items.Add("Video Command")
        End If
        If File.Exists(AudioStreamFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
            MetroSetComboBox2.Items.Add("Audio Command")
        ElseIf File.Exists(AudioQueueFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
            MetroSetComboBox2.Items.Add("Audio Command")
        End If
        If File.Exists(ChapterStreamConfigPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
            MetroSetComboBox2.Items.Add("Chapter Command")
        End If
        If File.Exists(MuxStreamConfigPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
            MetroSetComboBox2.Items.Add("Mux Command")
        End If
        If File.Exists(TrimStreamConfigPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
            MetroSetComboBox2.Items.Add("Trim Command")
        End If
        If MetroSetComboBox2.Items.Count <= 0 Then
            MetroSetComboBox2.Items.Add("No Command Found")
        End If
    End Sub
    Public Sub MediaEncodeLoad()
        If MetroSetComboBox2.Text.ToString = "Video Command" Then
            If File.Exists(VideoStreamFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
                TextBoxExt1.Text = File.ReadAllText(VideoStreamFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt")
            ElseIf File.Exists(VideoQueueFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
                TextBoxExt1.Text = File.ReadAllText(VideoQueueFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt")
            Else
                TextBoxExt1.Text = "Command not found :("
            End If
        ElseIf MetroSetComboBox2.Text.ToString = "Audio Command" Then
            If File.Exists(AudioStreamFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
                If MainMenu.MetroSetSwitch6.Switched = False And MainMenu.DataGridView1.Rows.Count <= 0 Then
                    FlagsCount = MainMenu.MetroSetComboBox3.Items.Count
                    For FlagsStart = 1 To FlagsCount
                        My.Computer.FileSystem.WriteAllText(AudioStreamFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_join.txt", String.Join(" ", File.ReadAllLines(AudioStreamFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_" & FlagsStart.ToString & ".txt")), True)
                    Next
                    If File.Exists(AudioStreamFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_join.txt") Then
                        TextBoxExt1.Text = File.ReadAllText(AudioStreamFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_join.txt")
                        File.Delete(AudioStreamFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_join.txt")
                    End If
                Else
                    TextBoxExt1.Text = File.ReadAllText(AudioStreamFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt")
                End If
            ElseIf File.Exists(AudioQueueFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
                TextBoxExt1.Text = File.ReadAllText(AudioQueueFlagsPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt")
            Else
                TextBoxExt1.Text = "Command not found :("
            End If
        ElseIf MetroSetComboBox2.Text.ToString = "Chapter Command" Then
            If File.Exists(ChapterStreamConfigPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
                TextBoxExt1.Text = File.ReadAllText(ChapterStreamConfigPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt")
            Else
                TextBoxExt1.Text = "Command not found :("
            End If
        ElseIf MetroSetComboBox2.Text.ToString = "Mux Command" Then
            If File.Exists(MuxStreamConfigPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
                TextBoxExt1.Text = File.ReadAllText(MuxStreamConfigPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt")
            Else
                TextBoxExt1.Text = "Command not found :("
            End If
        ElseIf MetroSetComboBox2.Text.ToString = "Trim Command" Then
            If File.Exists(TrimStreamConfigPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt") Then
                TextBoxExt1.Text = File.ReadAllText(TrimStreamConfigPath & MetroSetComboBox1.Text.ToString & "_flags_1.txt")
            Else
                TextBoxExt1.Text = "Command not found :("
            End If
        Else
            TextBoxExt1.Text = "Command not found :("
        End If
    End Sub
End Class
