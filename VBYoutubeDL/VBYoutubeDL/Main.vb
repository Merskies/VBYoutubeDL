Public Class frmMain
    Private psi As ProcessStartInfo
    Private cmd As Process
    Private Delegate Sub InvokeWithString(ByVal text As String)
    Private Sub DaLoad() Handles Me.Load
        txtFolder.Text = "Request"
    End Sub
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRun.Click
        Dim Command, Folder, URL As String
        Folder = txtFolder.Text
        URL = txtURL.Text
        Command = "youtube-dl.exe"
        Dim Arguments As String = "-i -o \\MERSKIES\rootshare\Media\Music\Downloaded\" & Folder & "\%(title)s.%(ext)s " & URL & " -f bestaudio"
        Try
            cmd.Kill()
        Catch ex As Exception
        End Try
        txtOutput.Clear()
        If Arguments <> "" Then
            psi = New ProcessStartInfo(Command, Arguments)
        Else
            psi = New ProcessStartInfo(Command)
        End If
        Dim systemencoding As Text.Encoding
        System.Text.Encoding.GetEncoding(Globalization.CultureInfo.CurrentUICulture.TextInfo.OEMCodePage)
        With psi
            .UseShellExecute = False
            .RedirectStandardError = True
            .RedirectStandardOutput = True
            .RedirectStandardInput = True
            .CreateNoWindow = True
            .StandardOutputEncoding = systemencoding
            .StandardErrorEncoding = systemencoding
        End With
        cmd = New Process With {.StartInfo = psi, .EnableRaisingEvents = True}
        AddHandler cmd.ErrorDataReceived, AddressOf Async_Data_Received
        AddHandler cmd.OutputDataReceived, AddressOf Async_Data_Received
        cmd.Start()
        cmd.BeginOutputReadLine()
        cmd.BeginErrorReadLine()
        txtURL.Text = ""
    End Sub
    Private Sub Async_Data_Received(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        Invoke(New InvokeWithString(AddressOf Sync_Output), e.Data)
    End Sub
    Private Sub Sync_Output(ByVal text As String)
        txtOutput.AppendText(text & Environment.NewLine)
        txtOutput.ScrollToCaret()
    End Sub
End Class