Imports System.Data.SqlClient
Imports System.Configuration

Public Class FormEditNDA
    Inherits Form
    Public Property SelectedNumBord As String

    Public Sub New()
        InitializeComponent()
        AddHandler ListBoxNDAs.SelectedIndexChanged, AddressOf ListBoxNDAs_SelectedIndexChanged
        AddHandler btnDelete.Click, AddressOf btnDelete_Click
        AddHandler btnEdit.Click, AddressOf btnEdit_Click
    End Sub

    Private Sub ConfigureDataGridView()
        With DataGridView1
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            .AllowUserToResizeColumns = True
            .AllowUserToOrderColumns = True
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            .Dock = DockStyle.None
            .Width = Me.ClientSize.Width - 40
            .Height = 300
            .Location = New Point(20, ListBoxNDAs.Bottom + 20)
            .ScrollBars = ScrollBars.Both
            .ReadOnly = True
        End With
    End Sub


    Private Sub ListBoxNDAs_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ListBoxNDAs.SelectedIndex = -1 Then
            DataGridView1.DataSource = Nothing
            btnEdit.Visible = False
            btnDelete.Visible = False
            Return
        End If

        Dim selectedNDA As String = ListBoxNDAs.SelectedItem.ToString()
        LoadNDADetails(selectedNDA)

        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
        Dim numBord As String = SelectedNumBord

        If String.IsNullOrWhiteSpace(numBord) Then
            MessageBox.Show("رقم الجدول مفقود. لا يمكن التحقق من الحالة.", "معلومات مفقودة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As New SqlConnection(connStr)
                conn.Open()

                Dim query As String = "SELECT status FROM dbo.NUM_BORDEREAU WHERE NUM_BORD = @NUM_BORD"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@NUM_BORD", numBord)

                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        Dim status As String = result.ToString().ToLower()

                        If status = "closed" Then
                            btnEdit.Visible = False
                            btnDelete.Visible = False
                        ElseIf status = "open" Then
                            btnEdit.Visible = True
                            btnDelete.Visible = True
                        Else
                            MessageBox.Show("❓ قيمة حالة غير معروفة.", "الحالة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                    Else
                        MessageBox.Show("⚠️ لم يتم العثور على حالة للرقم الجدول المعطى.", "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error checking status: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub FormEditNDA_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not String.IsNullOrEmpty(SelectedNumBord) Then
            Me.Text = $"بيانات الجدول رقم: {SelectedNumBord}"
            lblBordereauInfo.Text = $"رقم الجدول المطلوب: {SelectedNumBord}"
        Else
            Me.Text = "تحرير بيانات الجدول"
            lblBordereauInfo.Text = "لم يتم تحديد رقم الجدول"
            MessageBox.Show("رقم الجدول غير محدد.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        LoadNDAsForSelectedBord(SelectedNumBord)
    End Sub

    Private Sub LoadNDAsForSelectedBord(numBordText As String)
        ListBoxNDAs.Items.Clear()
        DataGridView1.DataSource = Nothing

        Dim numBord As Integer
        If Not Integer.TryParse(numBordText, numBord) Then
            MessageBox.Show("رقم الجدول غير صالح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection")?.ConnectionString
        If String.IsNullOrEmpty(connStr) Then
            MessageBox.Show("سلسلة اتصال مفقودة في ملف الإعدادات.", "خطأ في الإعدادات", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            Dim isOpen As Boolean = False


            Using conn As New SqlConnection(connStr)
                conn.Open()

                Dim queryStatus As String = "SELECT status FROM dbo.NUM_BORDEREAU WHERE NUM_BORD = @numBord"
                Using cmdStatus As New SqlCommand(queryStatus, conn)
                    cmdStatus.Parameters.AddWithValue("@numBord", numBord)

                    Dim result As Object = cmdStatus.ExecuteScalar()
                    If result Is Nothing Then
                        MessageBox.Show($"رقم الجدول {numBord} غير موجود.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If

                    isOpen = result.ToString().ToLower() = "open"
                End Using
            End Using

            Using conn As New SqlConnection(connStr)
                conn.Open()

                Dim queryNDAs As String = "SELECT DISTINCT NDA FROM dbo.Detail_Bord WHERE NUM_BORD = @NUM_BORD"
                Using cmdNDAs As New SqlCommand(queryNDAs, conn)
                    cmdNDAs.Parameters.AddWithValue("@NUM_BORD", numBord)

                    Using reader As SqlDataReader = cmdNDAs.ExecuteReader()
                        While reader.Read()
                            ListBoxNDAs.Items.Add(reader("NDA").ToString())
                        End While
                    End Using
                End Using
            End Using

            If ListBoxNDAs.Items.Count = 0 Then
                isOpen = 0
                MessageBox.Show($"لا يوجد أي ملف للعرض {numBord}", "نتيجة فارغة", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            btnEdit.Visible = isOpen
            btnDelete.Visible = isOpen
        Catch ex As Exception
            MessageBox.Show($"[Error]: {ex.Message}{Environment.NewLine}{ex.StackTrace}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub




    Private Sub btnCloseForm_Click(sender As Object, e As EventArgs) Handles btnCloseForm.Click
        Me.Close()
    End Sub

    Private Sub LoadNDADetails(nda As String)
        Try
            Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
            Dim query As String = "
SELECT 
 

    ICD5,
    ICD4,
    ICD3,
    ICD2,
    ICD1,
    Couverture,
    SUM(PartHopital) AS PartHopital,
    SUM(PartMedecin) AS PartMedecin,
    Specialite,
    NADELI,
    CodeSequence,
    CodeAct,
    SUM(Quantite) AS Quantite,
    CAST(DateAct AS DATE) AS DateAct,
    DateSortie,
    DateEntree,
    PEC,
Contrat

FROM dbo.Detail_Bord
WHERE nda = @nda
GROUP BY 
    PEC, DateEntree, DateSortie,
    CAST(DateAct AS DATE),
    CodeAct, CodeSequence, NADELI, Specialite,
    Couverture, ICD1, ICD2, ICD3, ICD4, ICD5, Contrat
ORDER BY CodeAct, DateAct"

            Dim dt As New DataTable()

            Using conn As New SqlConnection(connStr),
              cmd As New SqlCommand(query, conn),
              adapter As New SqlDataAdapter(cmd)

                cmd.Parameters.AddWithValue("@NDA", nda)
                conn.Open()
                adapter.Fill(dt)
            End Using

            DataGridView1.DataSource = dt

            DataGridView1.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True

            DataGridView1.AutoResizeColumnHeadersHeight()
            Dim arabicHeaders As New Dictionary(Of String, String) From {
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
            {"PartMedecin", "حصة الطبيب المطلوبة"},
            {"PartHopital", "حصة المستشفى المطلوبة"},
            {"Couverture", "تغطية الوزارة"},
            {"ICD1", "تشخيص أساسي"},
            {"ICD2", "تشخيص ثانوي أول"},
            {"ICD3", "تشخيص ثانوي ثاني"},
            {"ICD4", "تشخيص ثانوي ثالث"},
            {"ICD5", "تشخيص ثانوي رابع"}
        }

            For Each column As DataGridViewColumn In DataGridView1.Columns
                If arabicHeaders.ContainsKey(column.Name) Then
                    column.HeaderText = arabicHeaders(column.Name)
                End If

                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
                column.MinimumWidth = 50
                column.Width = Math.Min(column.Width, 200)
            Next


        Catch ex As Exception
            MessageBox.Show($"خطأ في تحميل تفاصيل الملف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs)
        If ListBoxNDAs.SelectedIndex = -1 Then
            MessageBox.Show("الرجاء اختيار ملف أولاً.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim ndaToDelete As String = ListBoxNDAs.SelectedItem.ToString()
        Dim confirm As DialogResult = MessageBox.Show(
    $"هل أنت متأكد أنك تريد حذف جميع السجلات للملف: {ndaToDelete}؟",
    "تأكيد الحذف",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Warning
)

        If confirm = DialogResult.Yes Then
            Dim input = InputBox($"لتأكيد الحذف، اكتب اسم الملف بالضبط: {ndaToDelete}", "اكتب للتأكيد")
            If input <> ndaToDelete Then
                MessageBox.Show("تم إلغاء الحذف. لم يتطابق الملف.", "تم الإلغاء", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Try
                Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
                Dim query As String = "DELETE FROM dbo.Detail_Bord WHERE NDA = @NDA"

                Using conn As New SqlConnection(connStr),
                  cmd As New SqlCommand(query, conn)

                    cmd.Parameters.AddWithValue("@NDA", ndaToDelete)
                    conn.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    MessageBox.Show($"تم حذف {rowsAffected} سجل(سجلات) بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Close()
                End Using
            Catch ex As Exception
                MessageBox.Show($"خطأ في حذف السجلات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs)
        If ListBoxNDAs.SelectedIndex = -1 OrElse DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("الرجاء اختيار ملف والتأكد من تحميل الملفات.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim row As DataGridViewRow = DataGridView1.Rows(0)
        Dim currentValues As New Dictionary(Of String, String)

        For Each column As DataGridViewColumn In DataGridView1.Columns
            currentValues.Add(column.Name, If(row.Cells(column.Index).Value?.ToString(), ""))
        Next
        Dim editForm As New FormEditFields()
        editForm.TextBoxContratEdit.Text = currentValues("Contrat")
        editForm.TextBoxPECEdit.Text = currentValues("PEC")
        editForm.TextBoxICD1Edit.Text = currentValues("ICD1")
        editForm.TextBoxICD2Edit.Text = currentValues("ICD2")
        editForm.TextBoxICD3Edit.Text = currentValues("ICD3")
        editForm.TextBoxICD4Edit.Text = currentValues("ICD4")
        editForm.TextBoxICD5Edit.Text = currentValues("ICD5")

        If editForm.ShowDialog() = DialogResult.OK Then

            Try
                Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

                Dim query As String = "UPDATE dbo.Detail_Bord SET 
                                        Contrat = @Contrat,
                                        PEC = @PEC,
                                        ICD1 = @ICD1,
                                        ICD2 = @ICD2,
                                        ICD3 = @ICD3,
                                        ICD4 = @ICD4,
                                        ICD5 = @ICD5
                                        WHERE NDA = @NDA"


                Using conn As New SqlConnection(connStr),
                      cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Contrat", editForm.TextBoxContratEdit.Text)
                    cmd.Parameters.AddWithValue("@PEC", editForm.TextBoxPECEdit.Text)
                    cmd.Parameters.AddWithValue("@ICD1", editForm.TextBoxICD1Edit.Text)
                    cmd.Parameters.AddWithValue("@ICD2", editForm.TextBoxICD2Edit.Text)
                    cmd.Parameters.AddWithValue("@ICD3", editForm.TextBoxICD3Edit.Text)
                    cmd.Parameters.AddWithValue("@ICD4", editForm.TextBoxICD4Edit.Text)
                    cmd.Parameters.AddWithValue("@ICD5", editForm.TextBoxICD5Edit.Text)
                    cmd.Parameters.AddWithValue("@NDA", ListBoxNDAs.SelectedItem.ToString())

                    conn.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    MessageBox.Show($"تم تحديث {rowsAffected} ملف  بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadNDADetails(ListBoxNDAs.SelectedItem.ToString())
                End Using
            Catch ex As Exception
                MessageBox.Show($"خطأ في تحديث ملف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub


End Class
