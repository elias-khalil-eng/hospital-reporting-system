Imports System.Data.SqlClient
Imports System.Configuration

Public Class TableEditorForm
    Private tableName As String
    Private adapter As SqlDataAdapter
    Private table As DataTable

    Public Sub New(tableName As String)
        InitializeComponent()
        Me.tableName = tableName
    End Sub

    Private Sub TableEditorForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
            Dim conn As New SqlConnection(connStr)
            adapter = New SqlDataAdapter($"SELECT * FROM dbo.{tableName}", conn)
            Dim builder As New SqlCommandBuilder(adapter)
            table = New DataTable()
            adapter.Fill(table)
            dgvEditor.DataSource = table
            dgvEditor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            adapter.Update(table)
            MessageBox.Show("Changes saved successfully.")
        Catch ex As Exception
            MessageBox.Show("Error saving changes: " & ex.Message)
        End Try
    End Sub
End Class
