Public Class FormResults
    Public Sub New(ByVal table As DataTable)
        InitializeComponent()
        DataGridViewResults.DataSource = table
        DataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub


End Class