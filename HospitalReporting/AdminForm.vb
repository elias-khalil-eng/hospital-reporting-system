Imports System.Data.SqlClient
Imports System.Configuration
Imports Microsoft.Office.Interop.Excel
Imports System.IO
Imports System.Text

Public Class NUM_BORD
    Private currentTable As String = ""
    Private adapter As SqlDataAdapter
    Private dataTable As System.Data.DataTable

    Private Sub AdminForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        adminGrid.Visible = True
        btnSave.Visible = False
        btnclose.Visible = False
        'btnClearTables.Enabled = False
    End Sub

    Private Sub LoadTable(tableName As String)
        Try
            btnclose.Visible = True
            Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
            Dim conn As New SqlConnection(connStr)

            adapter = New SqlDataAdapter("SELECT * FROM " & tableName, conn)
            Dim builder As New SqlCommandBuilder(adapter)
            dataTable = New System.Data.DataTable()
            adapter.Fill(dataTable)

            Select Case tableName
                Case "[dbo].[forfait]", "[dbo].[ValidAssurance]", "[dbo].[ORDER_LETTER]", "[dbo].[SPECIALITEE]", "[dbo].[NUM_BORDEREAU]"
                    dataTable.PrimaryKey = New DataColumn() {dataTable.Columns("ID")}
                Case Else
                    Throw New Exception("Primary key for table '" & tableName & "' not defined.")
            End Select

            adminGrid.DataSource = dataTable
            btnSave.Visible = True
            currentTable = tableName

        Catch ex As Exception
            MessageBox.Show("Error loading table: " & ex.Message)
        End Try
    End Sub

    Private Sub DisableOtherButtons(exceptButton As System.Windows.Forms.Button)
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is System.Windows.Forms.Button AndAlso ctrl IsNot exceptButton AndAlso ctrl.Name <> "btnSave" AndAlso ctrl.Name <> "btnclose" Then
                ctrl.Enabled = False
            End If
        Next
    End Sub

    Private Sub EnableAllButtons()
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is Button Then
                ctrl.Enabled = True
            End If
        Next
    End Sub
    
   Private Sub NUM_BORD_Click(sender As Object, e As EventArgs) Handles btnNum_bord.Click
    LoadTable("[dbo].[NUM_BORDEREAU]")
    DisableOtherButtons(btnNum_bord) ' btnNum_bord is System.Windows.Forms.Button
End Sub
    Private Sub btnForfai_Click(sender As Object, e As EventArgs) Handles btnForfai.Click
        LoadTable("[dbo].[forfait]")
        DisableOtherButtons(btnForfai)
    End Sub

    Private Sub btnValidAssure_Click(sender As Object, e As EventArgs) Handles btnValidAssure.Click
        LoadTable("[dbo].[ValidAssurance]")
        DisableOtherButtons(btnValidAssure)
    End Sub

    Private Sub btnOrderLetter_Click(sender As Object, e As EventArgs) Handles btnOrderLetter.Click
        LoadTable("[dbo].[ORDER_LETTER]")
        DisableOtherButtons(btnOrderLetter)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            adapter.Update(dataTable)
            MessageBox.Show("Changes saved successfully.")
        Catch ex As Exception
            MessageBox.Show("Error saving changes: " & ex.Message)
        End Try
    End Sub

    Private Sub btnclose_Click(sender As Object, e As EventArgs) Handles btnclose.Click
        adminGrid.DataSource = Nothing
        currentTable = ""
        dataTable = Nothing
        adapter = Nothing

        EnableAllButtons()
        EnableMainButtons()
        btnSave.Visible = False
        btnclose.Visible = False
    End Sub

    Private Sub btnClearTables_Click(sender As Object, e As EventArgs) Handles btnClearTables.Click
        Dim confirm = MessageBox.Show(
            "Are you sure you want to delete all records from 'Detail_Bord' and 'NUM_BORDEREAU'?",
            "Confirm Deletion",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        )

        If confirm = DialogResult.Yes Then
            Try
                Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
                Using conn As New SqlConnection(connStr)
                    conn.Open()

                    Dim sql As String = "
                        DELETE FROM dbo.Detail_Bord;
                        DELETE FROM dbo.NUM_BORDEREAU;
                    "

                    Using cmd As New SqlCommand(sql, conn)
                        cmd.ExecuteNonQuery()
                    End Using

                    MessageBox.Show("All data has been deleted successfully from both tables.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Using
            Catch ex As Exception
                MessageBox.Show("Error while deleting data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            System.Windows.Forms.Application.Restart()
        End If
    End Sub

    Private Sub Close_Click(sender As Object, e As EventArgs) Handles CloseForm.Click
        Me.Close()
    End Sub


    Private Sub btnExportDetailBord_Click(sender As Object, e As EventArgs) Handles btnExportDetailBord.Click
        Dim dt As New System.Data.DataTable()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

        Try
            ' Load all data from Detail_Bord table
            Using conn As New SqlConnection(connStr)
                conn.Open()
                Dim query As String = "SELECT * FROM dbo.Detail_Bord"
                Using cmd As New SqlCommand(query, conn)
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            If dt.Rows.Count = 0 Then
                MessageBox.Show("لا يوجد بيانات في جدول Detail_Bord.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Save File Dialog for export
            Dim sfd As New SaveFileDialog() With {
                .Filter = "CSV files (*.csv)|*.csv",
                .Title = "Export Detail_Bord to CSV",
                .FileName = "Detail_Bord_Export.csv"
            }

            If sfd.ShowDialog() = DialogResult.OK Then
                Dim path = sfd.FileName

                Using writer As New StreamWriter(path, False, New UTF8Encoding(True)) ' UTF-8 with BOM
                    ' Write column headers
                    Dim headers = dt.Columns.Cast(Of DataColumn).Select(Function(c) CsvEscape(c.ColumnName))
                    writer.WriteLine(String.Join(";", headers))

                    ' Write data rows
                    For Each row As DataRow In dt.Rows
                        Dim fields = row.ItemArray.Select(Function(o) CsvEscape(If(o IsNot Nothing, o.ToString(), "")))
                        writer.WriteLine(String.Join(";", fields))
                    Next
                End Using

                MessageBox.Show("✅ تم تصدير البيانات بنجاح إلى ملف CSV.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Optional: Open folder containing the file
                ' Process.Start("explorer.exe", "/select,""" & path & """")
            End If

        Catch ex As Exception
            MessageBox.Show("❌ حدث خطأ أثناء التصدير: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Escapes a field for CSV output
    Private Function CsvEscape(field As String) As String
        If field Is Nothing Then Return """" ' Return empty quoted string for null

        Dim mustQuote = field.Contains(";") OrElse field.Contains("""") OrElse field.Contains(vbLf) OrElse field.Contains(vbCr)

        If mustQuote Then
            field = field.Replace("""", """""") ' Double internal quotes
            Return $"""{field}"""
        Else
            Return field
        End If
    End Function

    Private Sub EnableMainButtons()
        ' Enable the main buttons you want active when form is reset
        btnNum_bord.Enabled = True
        btnForfai.Enabled = True
        btnValidAssure.Enabled = True
        btnOrderLetter.Enabled = True
        btnClearTables.Enabled = True
        btnExportDetailBord.Enabled = True
        CloseForm.Enabled = True
        ' ...enable others as needed
        btnSave.Visible = False
        btnclose.Visible = False
    End Sub
End Class
