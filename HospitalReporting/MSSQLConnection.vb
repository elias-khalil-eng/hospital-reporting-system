Imports System.Data.SqlClient
Imports System.Configuration
Public Class MSSQLConnection
    Private conn As SqlConnection
    Private connectionString As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

    Public Sub New(Optional customConnectionString As String = "")
        If Not String.IsNullOrEmpty(customConnectionString) Then
            connectionString = customConnectionString
        End If
        conn = New SqlConnection(connectionString)
    End Sub

    Public Function GetDataTable(query As String) As DataTable
        Dim dt As New DataTable()
        Try
            Using cmd As New SqlCommand(query, conn)
                Using adapter As New SqlDataAdapter(cmd)
                    conn.Open()
                    adapter.Fill(dt)
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        Finally
            conn.Close()
        End Try
        Return dt
    End Function

    Public Function UpdateTable(query As String, ByRef dt As DataTable) As Boolean
        Try
            Using adapter As New SqlDataAdapter(query, conn)
                Dim builder As New SqlCommandBuilder(adapter)
                conn.Open()
                adapter.Update(dt)
                Return True
            End Using
        Catch ex As Exception
            MessageBox.Show("Error saving changes: " & ex.Message)
            Return False
        Finally
            conn.Close()
        End Try
    End Function
End Class
