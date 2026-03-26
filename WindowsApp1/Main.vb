Imports System.Data.SqlClient
Imports System.Configuration

Public Class Main

    Private moulhakAdapter As SqlDataAdapter
    Private moulhakTable As DataTable

    Private selectedNumBord As String
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

        Using conn As New SqlConnection(connStr)
            conn.Open()

            Dim checkQuery As String = "SELECT COUNT(*) FROM dbo.NUM_BORDEREAU WHERE NUM_BORD IS NOT NULL AND status IS NOT NULL"
            Dim checkCmd As New SqlCommand(checkQuery, conn)
            Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If count = 0 Then
                btnLoadBordereaux.Enabled = False
                ButtonExportExcel.Enabled = False
                CLOSE_BORD.Enabled = False
            End If
        End Using

        CheckLatestBordereauStatus()

        Button1.Visible = False
        dgvBordereaux.Visible = False
        btnClose.Visible = False
        dgvBordereaux.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvBordereaux.ReadOnly = True
        dgvBordereaux.RowHeadersVisible = False


        btnOpenEditor.Visible = False
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If String.IsNullOrEmpty(selectedNumBord) Then
            MessageBox.Show("يرجى اختيار الجدول ", "لا يوجد اختيار", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim form1Instance As New Form1()
        form1Instance.SelectedNumBord = selectedNumBord
        form1Instance.Show()

    End Sub


    Private Sub btnLoadBordereaux_Click(sender As Object, e As EventArgs) Handles btnLoadBordereaux.Click
        CLOSE_ALL.Enabled = False
        CheckLatestBordereauStatus()
        dgvBordereaux.Visible = True
        CLOSE_BORD.Enabled = False
        ButtonExportExcel.Enabled = False
        ADD_BORD.Enabled = False
        btnLoadBordereaux.Enabled = False
        btnClose.Visible = True
        btnOpenMoulhak.Enabled = False
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
        Dim query As String = "SELECT NUM_BORD, OPEN_DATE, CLOSE_DATE, status FROM dbo.num_bordereau ORDER BY NUM_BORD"

        Using connection As New SqlConnection(connStr)
            Dim adapter As New SqlDataAdapter(query, connection)
            Dim table As New DataTable()

            Try
                adapter.Fill(table)
                dgvBordereaux.DataSource = table
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub dgvBordereaux_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBordereaux.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim clickedColumn As String = dgvBordereaux.Columns(e.ColumnIndex).Name

            If clickedColumn = "NUM_BORD" Then
                Dim selectedRow As DataGridViewRow = dgvBordereaux.Rows(e.RowIndex)
                Dim clickedNumBord As String = selectedRow.Cells("NUM_BORD").Value.ToString()

                selectedNumBord = clickedNumBord

                Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

                Using connection As New SqlConnection(connStr)
                    connection.Open()

                    Dim statusQuery As String = "SELECT status FROM dbo.NUM_BORDEREAU WHERE NUM_BORD = @NUM_BORD"
                    Using statusCmd As New SqlCommand(statusQuery, connection)
                        statusCmd.Parameters.AddWithValue("@NUM_BORD", clickedNumBord)

                        Dim status As String = Convert.ToString(statusCmd.ExecuteScalar()).ToLower()

                        If status = "open" Then
                            Button1.Visible = True
                            Button1.Enabled = True
                        Else
                            Button1.Visible = False
                            Button1.Enabled = False
                        End If
                    End Using
                End Using

                btnOpenEditor.Visible = True
                btnClose.Visible = True

                LoadBordetails(clickedNumBord)
            End If
        End If
    End Sub



    Private Sub LoadBordetails(NUM_BORD As String)
        Button1.Visible = True
        btnOpenEditor.Visible = True
        btnClose.Visible = True
    End Sub
    Private Sub btnOpenEditor_Click(sender As Object, e As EventArgs) Handles btnOpenEditor.Click
        Dim editor As New FormEditNDA()
        editor.SelectedNumBord = selectedNumBord
        editor.ShowDialog()
    End Sub



    Private Sub ADD_BORD_Click(sender As Object, e As EventArgs) Handles ADD_BORD.Click
        CheckLatestBordereauStatus()
        btnLoadBordereaux.Enabled = True


        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

        Using connection As New SqlConnection(connStr)
            connection.Open()

            Dim checkQuery As String = "SELECT COUNT(*) FROM dbo.NUM_BORDEREAU WHERE NUM_BORD IS NOT NULL AND status IS NOT NULL"
            Dim checkCmd As New SqlCommand(checkQuery, connection)
            Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If count = 0 Then
                Dim insertQuery As String = "INSERT INTO dbo.NUM_BORDEREAU (NUM_BORD, status, OPEN_DATE) VALUES (1, 'Open', @date)"
                Using insertCmd As New SqlCommand(insertQuery, connection)
                    insertCmd.Parameters.AddWithValue("@date", DateTime.Now)
                    insertCmd.ExecuteNonQuery()
                End Using
                MessageBox.Show("تم إدخال الجدول الأولي بنجاح")
                CheckLatestBordereauStatus()
                Exit Sub
            End If
            Dim statusQuery As String = "Select TOP 1 status FROM dbo.NUM_BORDEREAU ORDER BY NUM_BORD DESC;"
            Dim statusCmd As New SqlCommand(statusQuery, connection)
            Dim status As String = Convert.ToString(statusCmd.ExecuteScalar())

            If status = "Open" Then
                CLOSE_BORD.Enabled = True
                MessageBox.Show("يوجد جدول مفتوح بالفعل. يرجى إغلاقه قبل إضافة جدول جديد.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CheckLatestBordereauStatus()
                Exit Sub
            ElseIf status = "Closed" Then

                Dim confirm As DialogResult = MessageBox.Show("أنت على وشك إنشاء جدول جديد. هل أنت متأكد؟", "تأكيد الإجراء", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                If confirm = DialogResult.No Then
                    MessageBox.Show("تم إلغاء العملية.", "تم الإلغاء", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    CheckLatestBordereauStatus()
                    Exit Sub
                End If


                Dim numQuery As String = "Select TOP 1 NUM_BORD FROM dbo.NUM_BORDEREAU ORDER BY NUM_BORD DESC;"
                Dim numCmd As New SqlCommand(numQuery, connection)
                Dim lastNum As Integer = Convert.ToInt32(numCmd.ExecuteScalar())
                Dim newNumBord As Integer = lastNum + 1
                Dim insertQuery As String = "INSERT INTO num_bordereau (NUM_BORD, OPEN_DATE, status) VALUES (@num, @Date, 'Open')"
                Dim insertCmd As New SqlCommand(insertQuery, connection)
                insertCmd.Parameters.AddWithValue("@num", newNumBord)
                insertCmd.Parameters.AddWithValue("@date", DateTime.Now)
                insertCmd.ExecuteNonQuery()
                CheckLatestBordereauStatus()
                MessageBox.Show($"تم إنشاء الجدول رقم {newNumBord} بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)


            End If
        End Using

    End Sub



    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CLOSE_ALL.Enabled = True
        CheckLatestBordereauStatus()
        ButtonExportExcel.Enabled = True
        dgvBordereaux.Visible = False
        Button1.Visible = False
        btnClose.Visible = False
        btnOpenEditor.Visible = False
        btnLoadBordereaux.Enabled = True
        btnOpenMoulhak.Enabled = True
    End Sub
    Private Sub CheckLatestBordereauStatus()
        Try
            Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
            Using conn As New SqlConnection(connStr)
                conn.Open()

                Dim query As String = "SELECT TOP 1 Status FROM dbo.num_bordereau ORDER BY NUM_BORD DESC"
                Using cmd As New SqlCommand(query, conn)
                    Dim statusObj = cmd.ExecuteScalar()

                    If statusObj IsNot Nothing Then
                        Dim status As String = statusObj.ToString().ToLower()

                        If status = "open" Then
                            CLOSE_BORD.Enabled = True
                            ADD_BORD.Enabled = False
                        ElseIf status = "closed" Then
                            CLOSE_BORD.Enabled = False
                            ADD_BORD.Enabled = True
                        End If
                    Else
                        ADD_BORD.Enabled = True
                        CLOSE_BORD.Enabled = False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking latest bordereau status: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCloseBordereau_Click(sender As Object, e As EventArgs) Handles CLOSE_BORD.Click
        CheckLatestBordereauStatus()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

        Using conn As New SqlConnection(connStr)
            conn.Open()

            Dim query As String = "SELECT TOP 1 NUM_BORD, status FROM dbo.NUM_BORDEREAU ORDER BY NUM_BORD DESC"
            Using cmd As New SqlCommand(query, conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Dim lastNumBord As Integer = Convert.ToInt32(reader("NUM_BORD"))
                        Dim status As String = reader("status").ToString()

                        reader.Close()
                        Dim ndaQuery As String = "SELECT COUNT(*) FROM dbo.detail_bord WHERE NUM_BORD = @num"
                        Using ndaCmd As New SqlCommand(ndaQuery, conn)
                            ndaCmd.Parameters.AddWithValue("@num", lastNumBord)
                            Dim ndaCount As Integer = Convert.ToInt32(ndaCmd.ExecuteScalar())

                            If ndaCount = 0 Then
                                MessageBox.Show($"⚠️ لا يمكن إغلاق البيان رقم {lastNumBord} لأنه لا يحتوي على أي ملف.", "لا يوجد ملف", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                CLOSE_BORD.Enabled = False
                                Exit Sub
                            Else
                                CLOSE_BORD.Enabled = True
                            End If
                        End Using

                        If status = "Closed" Then
                            MessageBox.Show($"ℹ️ الجدول الأخير رقم {lastNumBord} مغلق بالفعل. يمكنك الآن إنشاء جدول جديد.", "مغلق مسبقًا", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            CheckLatestBordereauStatus()
                            Exit Sub
                        End If

                        Dim confirm As DialogResult = MessageBox.Show(
                            $"⚠️ أنت على وشك إغلاق البيان رقم #{lastNumBord}. لا يمكن التراجع عن هذا الإجراء. هل أنت متأكد تمامًا؟",
                            "تأكيد الإغلاق",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        )

                        If confirm = DialogResult.No Then
                            MessageBox.Show("❌ تم إلغاء العملية.", "تم الإلغاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            CheckLatestBordereauStatus()
                            Exit Sub
                        End If

                        Dim updateQuery As String = "UPDATE dbo.NUM_BORDEREAU SET status = 'Closed', CLOSE_DATE = @closeDate WHERE NUM_BORD = @num"
                        Using updateCmd As New SqlCommand(updateQuery, conn)
                            updateCmd.Parameters.AddWithValue("@closeDate", DateTime.Now)
                            updateCmd.Parameters.AddWithValue("@num", lastNumBord)
                            updateCmd.ExecuteNonQuery()
                        End Using
                        CheckLatestBordereauStatus()
                        MessageBox.Show($"✅ تم إغلاق البيان رقم #{lastNumBord} بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("⚠️ لم يتم العثور على سجلات بيان في قاعدة البيانات.", "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End Using
            End Using
        End Using
    End Sub


    Private Function GetLastBordereauStatus() As String
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

        Using conn As New SqlConnection(connStr)
            conn.Open()

            Dim query As String = "SELECT TOP 1 status FROM dbo.NUM_BORDEREAU ORDER BY NUM_BORD DESC"
            Using cmd As New SqlCommand(query, conn)
                Dim result As Object = cmd.ExecuteScalar()

                If result IsNot Nothing Then
                    Return result.ToString()
                Else
                    Return "NoData"
                End If
            End Using
        End Using
    End Function

    Private Sub CLOSE_ALL_Click(sender As Object, e As EventArgs) Handles CLOSE_ALL.Click
        Me.Close()
    End Sub
    Private Sub ButtonExportToExcel_Click(sender As Object, e As EventArgs) Handles ButtonExportExcel.Click
        Dim numBord As String = InputBox("أدخل رقم البورد:", "تصدير إلى Excel")

        If String.IsNullOrWhiteSpace(numBord) Then
            MessageBox.Show("الرجاء إدخال رقم البورد.")
            Return
        End If

        Dim dt As New DataTable()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

        Try
            Using conn As New SqlConnection(connStr)
                conn.Open()
                Dim query As String = "
WITH grouped AS (
    SELECT 
        MIN(ICD5) AS ICD5,
        MIN(ICD4) AS ICD4,
        MIN(ICD3) AS ICD3,
        MIN(ICD2) AS ICD2,
        MIN(ICD1) AS ICD1,
        MIN(Couverture) AS Couverture,
        SUM(ISNULL(PartHopital, 0)) AS TotalPartHopital,
        SUM(ISNULL(PartMedecin, 0)) AS TotalPartMedecin,
        MIN(Specialite) AS Specialite,
        MIN(NADELI) AS NADELI,
        MIN(CodeSequence) AS CodeSequence,
        CodeAct,
        SUM(CASE WHEN LOWER(RTRIM(TYPE_REPARTITION)) = 'etab' THEN ISNULL(Quantite, 0) ELSE 0 END) AS Quantite_etab,
        SUM(CASE WHEN LOWER(RTRIM(TYPE_REPARTITION)) = 'med' THEN ISNULL(Quantite, 0) ELSE 0 END) AS Quantite_med,
        CAST(DateAct AS DATE) AS DateAct,
        MIN(DateSortie) AS DateSortie,
        MIN(DateEntree) AS DateEntree,
        MIN(PEC) AS PEC,
        MIN(Contrat) AS Contrat
    FROM dbo.Detail_Bord
    WHERE NUM_BORD = @num_bord
      AND CODE_HDF NOT IN (SELECT CODE_HDF FROM dbo.NOT_MERGED)
    GROUP BY 
        CAST(DateAct AS DATE),
        CodeAct,
        PEC
),

notGrouped AS (
    SELECT 
        ICD5,
        ICD4,
        ICD3,
        ICD2,
        ICD1,
        Couverture,
        ISNULL(PartHopital, 0) AS TotalPartHopital,
        ISNULL(PartMedecin, 0) AS TotalPartMedecin,
        Specialite,
        NADELI,
        CodeSequence,
        CodeAct,
        CASE 
            WHEN LOWER(RTRIM(TYPE_REPARTITION)) = 'etab' THEN ISNULL(Quantite, 0)
            WHEN LOWER(RTRIM(TYPE_REPARTITION)) = 'med' THEN ISNULL(Quantite, 0)
            ELSE ISNULL(Quantite, 0)
        END AS Quantite,
        CAST(DateAct AS DATE) AS DateAct,
        DateSortie,
        DateEntree,
        PEC,
        Contrat
    FROM dbo.Detail_Bord
    WHERE NUM_BORD = @num_bord
      AND CODE_HDF IN (SELECT CODE_HDF FROM dbo.NOT_MERGED)
)

SELECT 
    ICD5, ICD4, ICD3, ICD2, ICD1,
    Couverture,
    TotalPartHopital,
    TotalPartMedecin,
    Specialite,
    NADELI,
    CodeSequence,
    CodeAct,
    CASE 
        WHEN TotalPartHopital > 0 AND TotalPartMedecin > 0 THEN Quantite_etab
        WHEN TotalPartHopital > 0 THEN Quantite_etab
        WHEN TotalPartMedecin > 0 THEN Quantite_med
        ELSE Quantite_etab + Quantite_med
    END AS Quantite,
    DateAct,
    DateSortie,
    DateEntree,
    PEC,
    Contrat
FROM grouped

UNION ALL

SELECT 
    ICD5, ICD4, ICD3, ICD2, ICD1,
    Couverture,
    TotalPartHopital,
    TotalPartMedecin,
    Specialite,
    NADELI,
    CodeSequence,
    CodeAct,
    Quantite,
    DateAct,
    DateSortie,
    DateEntree,
    PEC,
    Contrat
FROM notGrouped

ORDER BY PEC, CodeAct, DateAct;
"


                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@num_bord", numBord)
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            If dt.Rows.Count = 0 Then
                MessageBox.Show("لم يتم العثور على ملفات لهذا الرقم.")
                Return
            End If

            ' Column name translation dictionary
            Dim columnHeaders As New Dictionary(Of String, String) From {
            {"Contrat", "رقم العقد"},
            {"PEC", "رقم بطاقة الاستشفاء"},
            {"DateEntree", "تاريخ الدخول"},
            {"DateSortie", "تاريخ الخروج"},
            {"DateAct", "تاريخ الفحص"},
            {"Quantite", "عدد الفحوصات"},
            {"CodeAct", "رقم العمل الجراحي"},
            {"CodeSequence", "رقم تسلسل العمل الجراحي"},
            {"NADELI", "رقم الطبيب في النقابة"},
            {"Specialite", "نوع الطبيب"},
            {"TotalPartMedecin", "حصة الطبيب المطلوبة"},
            {"TotalPartHopital", "حصة المستشفى المطلوبة"},
            {"Couverture", "تغطية الوزارة"},
            {"ICD1", "تشخيص أساسي"},
            {"ICD2", "تشخيص ثانوي أول"},
            {"ICD3", "تشخيص ثانوي ثاني"},
            {"ICD4", "تشخيص ثانوي ثالث"},
            {"ICD5", "تشخيص ثانوي رابع"}
        }

            ' Only export columns that exist in the dictionary and in the DataTable
            Dim exportColumns = dt.Columns.Cast(Of DataColumn)().
                            Where(Function(c) columnHeaders.ContainsKey(c.ColumnName)).
                            ToList()

            ' Start Excel
            Dim excelApp As New Microsoft.Office.Interop.Excel.Application
            excelApp.ScreenUpdating = False
            excelApp.DisplayAlerts = False

            Dim workbook = excelApp.Workbooks.Add()
            Dim worksheet = workbook.Sheets(1)

            ' Write headers
            For j As Integer = 0 To exportColumns.Count - 1
                Dim colName = exportColumns(j).ColumnName
                worksheet.Cells(1, j + 1).Value = columnHeaders(colName)
            Next

            ' Write data
            Dim data(dt.Rows.Count - 1, exportColumns.Count - 1) As Object
            For i As Integer = 0 To dt.Rows.Count - 1
                For j As Integer = 0 To exportColumns.Count - 1
                    data(i, j) = dt.Rows(i)(exportColumns(j).ColumnName)
                Next
            Next

            Dim startCell As Microsoft.Office.Interop.Excel.Range = worksheet.Cells(2, 1)
            Dim endCell As Microsoft.Office.Interop.Excel.Range = worksheet.Cells(dt.Rows.Count + 1, exportColumns.Count)
            worksheet.Range(startCell, endCell).Value = data

            worksheet.Columns.AutoFit()
            excelApp.Visible = True
            excelApp.ScreenUpdating = True
            excelApp.DisplayAlerts = True

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء التصدير: " & ex.Message)
        End Try
    End Sub



    Private Sub LoadMoulhakData()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
        Try
            Dim conn As New SqlConnection(connStr)
            moulhakAdapter = New SqlDataAdapter("SELECT * FROM dbo.moulhak", conn)
            Dim builder As New SqlCommandBuilder(moulhakAdapter)

            moulhakTable = New DataTable()
            moulhakAdapter.Fill(moulhakTable)
            dgvMoulhak.DataSource = moulhakTable

            AddHandler moulhakTable.RowChanged, AddressOf EnableSaveButton
            AddHandler moulhakTable.RowDeleted, AddressOf EnableSaveButton
            AddHandler moulhakTable.ColumnChanged, AddressOf EnableSaveButton

            dgvMoulhak.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            btnSaveMoulhak.Enabled = False
        Catch ex As Exception
            MessageBox.Show("خطأ في تحميل بيانات الملحق: " & ex.Message)
        End Try
    End Sub

    Private Sub EnableSaveButton(sender As Object, e As EventArgs)
        btnSaveMoulhak.Enabled = True
    End Sub
    Private Sub SaveMoulhakData()
        Try
            If moulhakAdapter Is Nothing Or moulhakTable Is Nothing Then
                MessageBox.Show("Adapter or data not initialized. Please load the data first.")
                Return
            End If

            moulhakAdapter.Update(moulhakTable)
            MessageBox.Show("تم حفظ تغييرات الملحق بنجاح.")
            btnSaveMoulhak.Enabled = False
        Catch ex As Exception
            MessageBox.Show("خطأ في حفظ بيانات الملحق: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSaveMoulhak_Click(sender As Object, e As EventArgs) Handles btnSaveMoulhak.Click
        SaveMoulhakData()
    End Sub


    Private Sub btnOpenMoulhak_Click(sender As Object, e As EventArgs) Handles btnOpenMoulhak.Click
        LoadMoulhakData()
        Button_close_moulhak.Visible = True
        btnImportMoulhakExcel.Visible = True
        dgvMoulhak.Visible = True
        btnSaveMoulhak.Visible = True
        Label1.Visible = True
        btnLoadBordereaux.Enabled = False
        ButtonExportExcel.Enabled = False
        ADD_BORD.Enabled = False
        CLOSE_BORD.Enabled = False
        CLOSE_ALL.Enabled = False
        btnOpenMoulhak.Enabled = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button_close_moulhak.Click
        dgvMoulhak.Visible = False
        btnSaveMoulhak.Visible = False
        Button_close_moulhak.Visible = False
        btnImportMoulhakExcel.Visible = False
        Label1.Visible = False
        CLOSE_ALL.Enabled = True
        CheckLatestBordereauStatus()
        ButtonExportExcel.Enabled = True
        btnOpenMoulhak.Enabled = True
        btnLoadBordereaux.Enabled = True
    End Sub

    Private Sub btnImportMoulhakExcel_Click(sender As Object, e As EventArgs) Handles btnImportMoulhakExcel.Click

        MessageBox.Show("يرجى التأكد من أن الملف يحتوي على عمودين فقط بالترتيب التالي:" & vbCrLf & vbCrLf &
                "1. Code" & vbCrLf &
                "2. Description" & vbCrLf & vbCrLf &
                "(Code)  يُرجى التأكد من عدم وجود رموز مكررة داخل الملف",
                "تنبيه حول هيكل الملف", MessageBoxButtons.OK, MessageBoxIcon.Information)



        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "Excel Files|*.xlsx"

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Dim importedTable As New DataTable()

            Using workbook = New ClosedXML.Excel.XLWorkbook(openFileDialog.FileName)
                Dim worksheet = workbook.Worksheets.First()
                Dim rows = worksheet.RangeUsed().RowsUsed().ToList()

                If rows.Count > 0 Then
                    For Each cell In rows(0).Cells()
                        importedTable.Columns.Add(cell.Value.ToString())
                    Next

                    If importedTable.Columns.Count <> 2 OrElse
                       importedTable.Columns(0).ColumnName.ToLower() <> "code" OrElse
                       importedTable.Columns(1).ColumnName.ToLower() <> "description" Then

                        MessageBox.Show("الملف لا يحتوي على الأعمدة المطلوبة: 'Code' و 'Description'.", "خطأ في الهيكل", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    For i = 1 To rows.Count - 1
                        importedTable.Rows.Add(rows(i).Cells().Select(Function(c) c.Value.ToString()).ToArray())
                    Next
                End If
            End Using

            Dim duplicateCodes = importedTable.AsEnumerable().
                GroupBy(Function(row) row.Field(Of String)("Code")).
                Where(Function(g) g.Count() > 1).
                Select(Function(g) g.Key).ToList()

            If duplicateCodes.Any() Then
                MessageBox.Show("الملف يحتوي على رموز مكررة ولا يمكن استيراده." & vbCrLf &
                                "الرموز المكررة: " & String.Join(", ", duplicateCodes), "خطأ في البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If moulhakAdapter Is Nothing Then
                Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
                Dim conn As New SqlConnection(connStr)
                moulhakAdapter = New SqlDataAdapter("SELECT * FROM dbo.moulhak", conn)
                Dim builder As New SqlCommandBuilder(moulhakAdapter)
            End If

            If moulhakTable Is Nothing Then
                moulhakTable = importedTable.Copy()
            Else
                For Each row As DataRow In importedTable.Rows
                    moulhakTable.ImportRow(row)
                Next
            End If

            dgvMoulhak.DataSource = moulhakTable
            dgvMoulhak.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            AddHandler moulhakTable.RowChanged, AddressOf EnableSaveButton
            AddHandler moulhakTable.RowDeleted, AddressOf EnableSaveButton
            AddHandler moulhakTable.ColumnChanged, AddressOf EnableSaveButton

            btnSaveMoulhak.Enabled = True
            MessageBox.Show("تم استيراد الملحقات وإضافتها إلى الملحق بنجاح.")
        End If
    End Sub


    Private Sub btnAdminAccess_Click(sender As Object, e As EventArgs) Handles btnAdminAccess.Click
        Dim password As String = InputBox("Enter admin password:", "Admin Access")

        If password = "elias" Then
            Dim adminForm As New NUM_BORD()
            adminForm.Show()
        Else
            MessageBox.Show("Invalid password!", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub




End Class