Public Class FormMoulhak
    Dim db As New MSSQLConnection()
    Dim dt As New DataTable()
    Dim query As String = "SELECT * FROM dbo.moulhak"

    Private Sub FormMoulhak_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    Private Sub LoadData()
        dt = db.GetDataTable(query)
        DataGridView1.DataSource = dt
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If db.UpdateTable(query, dt) Then
            MessageBox.Show("Changes saved successfully.")
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            For Each row As DataGridViewRow In DataGridView1.SelectedRows
                DataGridView1.Rows.Remove(row)
            Next
        End If
    End Sub
End Class
