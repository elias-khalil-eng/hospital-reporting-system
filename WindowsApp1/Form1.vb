Imports System.Configuration
Imports Oracle.ManagedDataAccess.Client
Imports System.Runtime.InteropServices
Imports System.Data.SqlClient


Public Class Form1
    Private savedData As DataTable
    Private exportedData As DataTable
    Private TextBoxFullName As TextBox
    Public Property SelectedNumBord As String
    Private ndaValidated As Boolean = False


    Private Async Sub ButtonSubmit_Click(sender As Object, e As EventArgs) Handles ButtonSubmit.Click



        DisableInputFields()
        ButtonSubmit.Enabled = False
        LabelStatus.Text = "جاري تحميل البيانات..."


        Dim nda As String = TextBoxNDA.Text.Trim()
        Dim contrat As String = TextBoxContrat.Text.Trim()
        Dim pec As String = TextBoxPEC.Text.Trim()
        Dim icd10_1 As String = TextBoxICD1.Text.Trim()


        TextBoxPEC.BackColor = Color.White
        TextBoxContrat.BackColor = Color.White
        TextBoxICD1.BackColor = Color.White


        If String.IsNullOrWhiteSpace(pec) Then TextBoxPEC.BackColor = Color.Yellow
        If String.IsNullOrWhiteSpace(contrat) Then TextBoxContrat.BackColor = Color.Yellow
        If String.IsNullOrWhiteSpace(icd10_1) Then TextBoxICD1.BackColor = Color.Yellow

        If String.IsNullOrWhiteSpace(pec) OrElse String.IsNullOrWhiteSpace(contrat) OrElse String.IsNullOrWhiteSpace(icd10_1) Then
            MessageBox.Show("يجب ملء كل الخانات الإجبارية", "خطأ في التحقق من الصحة", MessageBoxButtons.OK, MessageBoxIcon.Warning)


            ButtonSubmit.Enabled = True
            LabelStatus.Text = ""
            EnableInputFields()
             Return
        End If

        If String.IsNullOrEmpty(nda) Then
            MessageBox.Show("الرجاء إدخال الملف.", "خطأ في التحقق من الصحة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ButtonSubmit.Enabled = False
            LabelStatus.Text = ""
            Return
        End If

        Try
            Dim dt As DataTable = Await Task.Run(Function() LoadDataFromDatabase(nda, contrat, pec, icd10_1, TextBoxICD2.Text.Trim(), TextBoxICD3.Text.Trim(), TextBoxICD4.Text.Trim(), TextBoxICD5.Text.Trim()))

            If dt.Rows.Count = 0 Then
                MessageBox.Show("لم يتم العثور على أي نتائج لهذا الملف.", "لم يتم العثور على نتائج", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If Not String.IsNullOrEmpty(TextBoxFullName.Text) Then
                    ndaValidated = True
                    EnableInputFields()
                    ButtonSubmit.Enabled = True
                Else
                    ndaValidated = False
                    DisableInputFields()
                    ButtonSubmit.Enabled = False
                End If
            Else
                savedData = dt
                Dim resultsForm As New FormResults(dt)
                resultsForm.Show()
            End If
        Catch ex As Exception
            MessageBox.Show("خطأ أثناء الاتصال بقاعدة البيانات: " & ex.Message)
        Finally
            ButtonSubmit.Enabled = False
            LabelStatus.Text = ""
        End Try

        ButtonSave.Enabled = True
    End Sub

    Private Function LoadDataFromDatabase(nda As String, contrat As String, pec As String, icd10_1 As String, icd10_2 As String, icd10_3 As String, icd10_4 As String, icd10_5 As String) As DataTable
        Dim dt As New DataTable()
        Dim validAssuranceCodes As HashSet(Of String) = LoadValidAssuranceCodes()
        Static mappingFirst As Dictionary(Of String, String) = LoadGefMappingsFromFirstQuery()
        Static mappingSecond As Dictionary(Of String, String) = LoadGefMappingsFromSecondQuery()
        Dim forfaitCodes As HashSet(Of String) = LoadValidCodesFromForfait()



        Dim connStr As String = ConfigurationManager.ConnectionStrings("OracleConnection").ConnectionString

        Try
            Using conn As New OracleConnection(connStr)
                conn.Open()

                Dim query As String = "SELECT
  q1.NIP,
  q1.NDA,
  q1.PRENOM,
  q1.PATRONYME,
  q1.NOM,
  q1.DATE_ENTREE_SEJOUR,
  q1.DATE_SORTIE_SEJOUR,
  q1.NUMERO_FACTURE,
  q1.NUMERO,
  q1.ASSURANCE,
  q1.TYPE_REPARTITION,
  q1.MONTANT,
  q1.NIFACTURE_LIGNE,
  q1.CODE,
  q1.CODE_REGROUPEMENT,
  q1.LIBELLE,
  q1.QUANTITE,
  q1.DATE_DEB,
  q1.PART,
  q2.DATE_ENTREE_MVT,
  q2.DATE_SORTIE_MVT,
  q2.SPECIALITE,
  q2.MED_RESP,
  q2.NADELI,
  q3.EXECUTANT,
  q3.SPEC_EXECUTANT,
  q3.NADELI_EXECUTANT
FROM
  (
                       SELECT
                         PATIENT_SOIGNE.NIP,
                         SEJOUR.NDA,
                         PATIENT_SOIGNE.PRENOM,
                         PATIENT_SOIGNE.PATRONYME,
                         PATIENT_SOIGNE.NOM,
                         TO_DATE(SEJOUR.DATE_ENT, 'YYYYMMDD') AS DATE_ENTREE_SEJOUR,
                         TO_DATE(SEJOUR.DATE_SOR, 'YYYYMMDD') AS DATE_SORTIE_SEJOUR,
                         FACTURE_LOGIQUE.NUMERO_FACTURE,
                         FACTURE.NUMERO,
                         C_CONTRAT.CODE AS ASSURANCE,
                         FACTURE.TYPE_REPARTITION,
                         FACTURE_LIGNE.PRIX AS MONTANT,
                         FACTURE_LIGNE.NIFACTURE_LIGNE,
                         FACTURE_LIGNE.CODE,
                         --FACTURE_LIGNE.CODE_REGROUPEMENT,
                         FACTURATION.NOMENCLATURE.CODE_REGROUPEMENT,
                         FACTURE_LIGNE.LIBELLE,
                         FACTURE_LIGNE.QUANTITE,
                         FACTURE_LIGNE.DATE_DEB,
                         FACTURE_LIGNE.PART
                         FROM
                           PENSOINS.PATIENT PATIENT_SOIGNE,
                           PENSOINS.SEJOUR,
                           FACTURATION.DOSSIER,
                           FACTURATION.C_CONTRAT,
                           FACTURATION.COUVDOSSIER,
                           FACTURATION.FACTURE,
                           FACTURATION.FACTURE_LIGNE,
                           FACTURATION.FACTURE_LOGIQUE,
                           FACTURATION.NOMENCLATURE
                         WHERE
                           PATIENT_SOIGNE.NIPATIENT(+) = SEJOUR.NIPATIENT
                           AND SEJOUR.NISEJOUR(+) = DOSSIER.NISEJOUR
                           AND C_CONTRAT.NICONTRAT(+) = COUVDOSSIER.NICONTRAT
                           AND FACTURE.NIFACTURE = FACTURE_LIGNE.NIFACTURE
                           AND COUVDOSSIER.NICOUVDOSSIER(+) = FACTURE.NICOUVDOSSIER
                           AND DOSSIER.NIDOSSIER = FACTURE_LOGIQUE.NIDOSSIER(+)
                           AND FACTURE.NIFACTURE_LOGIQUE(+) = FACTURE_LOGIQUE.NIFACTURE_LOGIQUE
                           AND ( FACTURE_LIGNE.CODE = FACTURATION.NOMENCLATURE.CODE (+) )
                           AND SEJOUR.NDA = :NDA
                           AND FACTURE.RETRAIT = 'F'
                           AND FACTURE_LIGNE.RETRAIT = 'F'
                           AND FACTURE_LOGIQUE.RETRAIT = 'F'
                           AND COUVDOSSIER.RETRAIT = 'F'
                           AND FACTURE_LOGIQUE.STATUT = 'FA'
                           --AND  C_CONTRAT.CODE= '330'
                           ) q1
LEFT JOIN (SELECT
                           SEJOUR.NDA,
                           decode(length(MOUVEMEN.DATE_ENT||MOUVEMEN.HEURE_ENT), 12, to_date(MOUVEMEN.DATE_ENT||MOUVEMEN.HEURE_ENT,'YYYYMMDDHH24MI')) as DATE_ENTREE_MVT,
                           decode(length(MOUVEMEN.DATE_SOR||MOUVEMEN.HEURE_SOR), 12, to_date(MOUVEMEN.DATE_SOR||MOUVEMEN.HEURE_SOR,'YYYYMMDDHH24MI')) as DATE_SORTIE_MVT,
                           MED_RESP_MVT.SPECIALITE,
                           MED_RESP_MVT.PRENOM  || ' ' || MED_RESP_MVT.NOM as MED_RESP,
                           MED_RESP_MVT.NADELI,
                           DENSE_RANK() OVER (PARTITION BY PENSOINS.sejour.nda ORDER BY PENSOINS.MOUVEMEN.NISEJMOUV) AS med_number
                           
                         FROM
                           PENSOINS.MOUVEMEN,
                           PENSOINS.TYPO,
                           PENSOINS.EJ_PERSO MED_RESP_MVT,
                           PENSOINS.PATIENT PATIENT_SOIGNE,
                           PENSOINS.SEJOUR,
                           FACTURATION.DOSSIER,
                           PENSOINS.EJ_PT PT_MOUVEMEN
                         WHERE
                           MOUVEMEN.NISEJMOUV = TYPO.NISEJMOUV
                           AND TYPO.NIMED = MED_RESP_MVT.NIUTILISAT(+)
                           AND PATIENT_SOIGNE.NIPATIENT(+) = SEJOUR.NIPATIENT
                           AND SEJOUR.NISEJOUR(+) = DOSSIER.NISEJOUR
                           AND SEJOUR.NISEJOUR = MOUVEMEN.NISEJOUR
                           AND PT_MOUVEMEN.NIPT(+) = MOUVEMEN.NIPT
                           AND SEJOUR.NDA = :NDA
                           ) q2
                           ON q1.NDA = q2.NDA
                           AND q1.DATE_DEB >= q2.DATE_ENTREE_MVT AND q1.DATE_DEB < q2.DATE_SORTIE_MVT
LEFT JOIN (SELECT
                         PATIENT_SOIGNE.NIP,
                         SEJOUR.NDA,
                         PATIENT_SOIGNE.PRENOM,
                         PATIENT_SOIGNE.PATRONYME,
                         PATIENT_SOIGNE.NOM,
                         TO_DATE(SEJOUR.DATE_ENT, 'YYYYMMDD') AS DATE_ENTREE_SEJOUR,
                         TO_DATE(SEJOUR.DATE_SOR, 'YYYYMMDD') AS DATE_SORTIE_SEJOUR,
                         FACTURE_LOGIQUE.NUMERO_FACTURE,
                         FACTURE.NUMERO,
                         C_CONTRAT.CODE AS ASSURANCE,
                         FACTURE.TYPE_REPARTITION,
                         FACTURE_LIGNE.PRIX AS MONTANT,
                         FACTURE_LIGNE.NIFACTURE_LIGNE,
                         FACTURE_LIGNE.CODE,
                         FACTURE_LIGNE.CODE_REGROUPEMENT,
                         FACTURE_LIGNE.LIBELLE,
                         FACTURE_LIGNE.QUANTITE,
                         FACTURE_LIGNE.DATE_DEB,
                         EP.PRENOM || ' ' || EP.NOM AS EXECUTANT,
                         EP.SPECIALITE AS SPEC_EXECUTANT,
                         EP.NADELI AS NADELI_EXECUTANT
                         FROM
                           PENSOINS.PATIENT PATIENT_SOIGNE,
                           PENSOINS.SEJOUR,
                           FACTURATION.DOSSIER,
                           FACTURATION.C_CONTRAT,
                           FACTURATION.COUVDOSSIER,
                           FACTURATION.FACTURE,
                           FACTURATION.FACTURE_LIGNE,
                           FACTURATION.FACTURE_LOGIQUE,
                           PENSOINS.EJ_PERSO EP,
                           FACTURATION.FACTURE_LOGIQUE_LIGNE FLL
                         WHERE
                           PATIENT_SOIGNE.NIPATIENT(+) = SEJOUR.NIPATIENT
                           AND SEJOUR.NISEJOUR(+) = DOSSIER.NISEJOUR
                           AND C_CONTRAT.NICONTRAT(+) = COUVDOSSIER.NICONTRAT
                           AND FACTURE.NIFACTURE = FACTURE_LIGNE.NIFACTURE
                           AND COUVDOSSIER.NICOUVDOSSIER(+) = FACTURE.NICOUVDOSSIER
                           AND DOSSIER.NIDOSSIER = FACTURE_LOGIQUE.NIDOSSIER(+)
                           AND FACTURE.NIFACTURE_LOGIQUE(+) = FACTURE_LOGIQUE.NIFACTURE_LOGIQUE
                           AND FLL.NIEXECUTANT = EP.NIUTILISAT
                           AND FLL.NIFACTURE_LOGIQUE_LIGNE = FACTURATION.FACTURE_LIGNE.NIFACTURE_LOGIQUE_LIGNE (+)
                           AND SEJOUR.NDA = :NDA
                           AND FACTURE.RETRAIT = 'F'
                           AND FACTURE_LIGNE.RETRAIT = 'F'
                           AND FACTURE_LOGIQUE.RETRAIT = 'F'
                           AND COUVDOSSIER.RETRAIT = 'F'
                           AND FACTURE_LOGIQUE.STATUT = 'FA'
                           ) q3
                           ON q1.NIFACTURE_LIGNE = q3.NIFACTURE_LIGNE"

                Using cmd As New OracleCommand(query, conn)
                    cmd.Parameters.Add("NDA", OracleDbType.Varchar2).Value = nda

                    Using reader As OracleDataReader = cmd.ExecuteReader()
                        Dim letterMap As Dictionary(Of String, String) = LoadLetterMapping()



                        dt.Columns.Add("تشخيص ثانوي رابع", GetType(String))
                        dt.Columns.Add("تشخيص ثانوي ثالث", GetType(String))
                        dt.Columns.Add("تشخيص ثانوي ثاني", GetType(String))
                        dt.Columns.Add("تشخيص ثانوي أول", GetType(String))
                        dt.Columns.Add("تشخيص أساسي", GetType(String))
                        dt.Columns.Add("تغطية الوزارة", GetType(String))
                        dt.Columns.Add("حصة المستشفى المطلوبة", GetType(Decimal))
                        dt.Columns.Add("حصة الطبيب المطلوبة", GetType(Decimal))
                        dt.Columns.Add("نوع الطبيب", GetType(String))
                        dt.Columns.Add("رقم الطبيب في النقابة", GetType(String))
                        dt.Columns.Add("رقم تسلسل العمل الجراحي", GetType(String))
                        dt.Columns.Add("رقم العمل الجراحي", GetType(String))
                        dt.Columns.Add("عدد الفحوصات", GetType(Integer))
                        dt.Columns.Add("تاريخ الفحص", GetType(Date))
                        dt.Columns.Add("تاريخ الخروج", GetType(Date))
                        dt.Columns.Add("تاريخ الدخول", GetType(Date))
                        dt.Columns.Add("رقم بطاقة الاستشفاء", GetType(String))
                        dt.Columns.Add("رقم العقد السنوي", GetType(String))
                        dt.Columns.Add("CODE_REG", GetType(String))
                        dt.Columns.Add("CODE_HDF", GetType(String))
                        dt.Columns.Add("LIBELLE", GetType(String))
                        dt.Columns.Add("NDA", GetType(String))
                        dt.Columns.Add("PRENOM", GetType(String))
                        dt.Columns.Add("PATRONYME", GetType(String))
                        dt.Columns.Add("NOM", GetType(String))
                        dt.Columns.Add("NUMERO_FACTURE", GetType(String))
                        dt.Columns.Add("NUMERO", GetType(String))
                        dt.Columns.Add("ASSURANCE", GetType(String))
                        dt.Columns.Add("TYPE_REPARTITION", GetType(String))
                        dt.Columns.Add("DATE_ENTREE_MVT", GetType(Date))
                        dt.Columns.Add("DATE_SORTIE_MVT", GetType(Date))
                        dt.Columns.Add("MED_RESP", GetType(String))
                        dt.Columns.Add("NUM_BORD", GetType(Integer))
                        dt.Columns.Add("SPECIALITE_HDF", GetType(String))
                        dt.Columns.Add("EXECUTANT", GetType(String))
                        dt.Columns.Add("SPEC_EXECUTANT", GetType(String))
                        dt.Columns.Add("PART", GetType(String))
                        Dim validCodes As HashSet(Of String) = LoadValidCodesFromMoulhak()

                        While reader.Read()
                            Dim row As DataRow = dt.NewRow()

                            row("رقم بطاقة الاستشفاء") = pec
                            row("تاريخ الدخول") = If(reader.IsDBNull(reader.GetOrdinal("DATE_ENTREE_SEJOUR")), DBNull.Value, reader.GetDateTime(reader.GetOrdinal("DATE_ENTREE_SEJOUR")))
                            row("تاريخ الخروج") = If(reader.IsDBNull(reader.GetOrdinal("DATE_SORTIE_SEJOUR")), DBNull.Value, reader.GetDateTime(reader.GetOrdinal("DATE_SORTIE_SEJOUR")))
                            row("تاريخ الفحص") = If(reader.IsDBNull(reader.GetOrdinal("DATE_DEB")), DBNull.Value, reader.GetDateTime(reader.GetOrdinal("DATE_DEB")))


                            row("عدد الفحوصات") = If(reader.IsDBNull(reader.GetOrdinal("QUANTITE")), 0, reader.GetInt32(reader.GetOrdinal("QUANTITE")))

                            Dim partIsNull As Boolean = reader.IsDBNull(reader.GetOrdinal("PART"))
                            Dim codeRegroupement As String = If(reader.IsDBNull(reader.GetOrdinal("CODE_REGROUPEMENT")), "", reader("CODE_REGROUPEMENT").ToString().Trim().ToUpper())

                            If partIsNull Then
                                Dim replacement As String = ""

                                If mappingFirst.ContainsKey(codeRegroupement) Then
                                    replacement = mappingFirst(codeRegroupement)
                                ElseIf mappingSecond.ContainsKey(codeRegroupement) Then
                                    replacement = mappingSecond(codeRegroupement)
                                End If

                                If String.IsNullOrWhiteSpace(replacement) Then
                                    row("رقم العمل الجراحي") = codeRegroupement
                                Else
                                    row("رقم العمل الجراحي") = replacement
                                End If
                            Else
                                row("رقم العمل الجراحي") = codeRegroupement
                            End If

                            row("CODE_REG") = If(reader.IsDBNull(reader.GetOrdinal("CODE_REGROUPEMENT")), "", reader("CODE_REGROUPEMENT").ToString().Trim().ToUpper())

                            Dim codeValue As String = If(reader.IsDBNull(reader.GetOrdinal("CODE")), "", reader("CODE").ToString().Trim().ToUpper())
                            row("رقم تسلسل العمل الجراحي") = If(validCodes.Contains(codeValue), 2D, 0D)

                            Dim nadeliRaw As String = If(reader.IsDBNull(reader.GetOrdinal("NADELI")), "", reader("NADELI").ToString())
                            row("رقم الطبيب في النقابة") = ConvertNadeliToArabic(nadeliRaw, letterMap)
                            row("SPECIALITE_HDF") = If(reader.IsDBNull(reader.GetOrdinal("SPECIALITE")), "", reader("SPECIALITE").ToString())

                            Dim typeRepartition As String = If(reader.IsDBNull(reader.GetOrdinal("TYPE_REPARTITION")), "", reader("TYPE_REPARTITION").ToString().ToLower())
                            Dim montant As Decimal = If(reader.IsDBNull(reader.GetOrdinal("MONTANT")), 0D, Convert.ToDecimal(reader("MONTANT")))
                            If typeRepartition = "etab" Then
                                row("حصة المستشفى المطلوبة") = montant
                                row("حصة الطبيب المطلوبة") = 0D
                            ElseIf typeRepartition = "med" Then
                                row("حصة الطبيب المطلوبة") = montant
                                row("حصة المستشفى المطلوبة") = 0D
                            Else
                                row("حصة الطبيب المطلوبة") = 0D
                                row("حصة المستشفى المطلوبة") = 0D
                            End If

                            row("تغطية الوزارة") = "0"
                            row("تشخيص أساسي") = icd10_1
                            row("تشخيص ثانوي أول") = icd10_2
                            row("تشخيص ثانوي ثاني") = icd10_3
                            row("تشخيص ثانوي ثالث") = icd10_4
                            row("تشخيص ثانوي رابع") = icd10_5
                            row("رقم العقد السنوي") = contrat
                            row("CODE_HDF") = If(reader.IsDBNull(reader.GetOrdinal("CODE")), "", reader("CODE").ToString())
                            row("LIBELLE") = If(reader.IsDBNull(reader.GetOrdinal("LIBELLE")), "", reader("LIBELLE").ToString())

                            Dim specialty As String = If(reader.IsDBNull(reader.GetOrdinal("SPECIALITE")), "", reader("SPECIALITE").ToString())
                            Dim nadeli As String = If(reader.IsDBNull(reader.GetOrdinal("NADELI")), "", reader("NADELI").ToString())
                            Dim nadeliExecutant As String = If(reader.IsDBNull(reader.GetOrdinal("NADELI_EXECUTANT")), "", reader("NADELI_EXECUTANT").ToString())
                            Dim result As String = GetArabicLetterForSpecialty(specialty, nadeli, nadeliExecutant)

                            If result = "س" Then
                                row("رقم الطبيب في النقابة") = ConvertNadeliToArabic(nadeliExecutant, letterMap)
                            End If

                            row("نوع الطبيب") = result
                            row("NDA") = If(reader.IsDBNull(reader.GetOrdinal("NDA")), "", reader("NDA").ToString())
                            row("PRENOM") = If(reader.IsDBNull(reader.GetOrdinal("PRENOM")), "", reader("PRENOM").ToString())
                            row("PATRONYME") = If(reader.IsDBNull(reader.GetOrdinal("PATRONYME")), "", reader("PATRONYME").ToString())
                            row("NOM") = If(reader.IsDBNull(reader.GetOrdinal("NOM")), "", reader("NOM").ToString())
                            row("NUMERO_FACTURE") = If(reader.IsDBNull(reader.GetOrdinal("NUMERO_FACTURE")), "", reader("NUMERO_FACTURE").ToString())
                            row("NUMERO") = If(reader.IsDBNull(reader.GetOrdinal("NUMERO")), "", reader("NUMERO").ToString())

                            Dim assuranceCode As String = If(reader.IsDBNull(reader.GetOrdinal("ASSURANCE")), "", reader("ASSURANCE").ToString())

                            If Not validAssuranceCodes.Contains(assuranceCode) Then
                                Continue While
                            End If
                            row("ASSURANCE") = assuranceCode
                            row("TYPE_REPARTITION") = typeRepartition
                            row("DATE_ENTREE_MVT") = If(reader.IsDBNull(reader.GetOrdinal("DATE_ENTREE_MVT")), DBNull.Value, reader.GetDateTime(reader.GetOrdinal("DATE_ENTREE_MVT")))
                            row("DATE_SORTIE_MVT") = If(reader.IsDBNull(reader.GetOrdinal("DATE_SORTIE_MVT")), DBNull.Value, reader.GetDateTime(reader.GetOrdinal("DATE_SORTIE_MVT")))
                            row("MED_RESP") = If(reader.IsDBNull(reader.GetOrdinal("MED_RESP")), "", reader("MED_RESP").ToString())
                            row("NUM_BORD") = SelectedNumBord
                            row("EXECUTANT") = If(reader.IsDBNull(reader.GetOrdinal("EXECUTANT")), "", reader("EXECUTANT").ToString())
                            row("SPEC_EXECUTANT") = If(reader.IsDBNull(reader.GetOrdinal("SPEC_EXECUTANT")), "", reader("SPEC_EXECUTANT").ToString())
                            row("PART") = If(reader.IsDBNull(reader.GetOrdinal("PART")), "", reader("PART").ToString())
                            Dim codeHDF As String = If(reader.IsDBNull(reader.GetOrdinal("CODE")), "", reader("CODE").ToString().Trim().ToUpper())

                            Dim codeHDFCheck As String = row("CODE_HDF").ToString().Trim().ToUpper()
                            Dim quantite As Integer = If(reader.IsDBNull(reader.GetOrdinal("QUANTITE")), 0, reader.GetInt32(reader.GetOrdinal("QUANTITE")))
                            Dim dateDeb As Date = If(reader.IsDBNull(reader.GetOrdinal("DATE_DEB")), Date.MinValue, reader.GetDateTime(reader.GetOrdinal("DATE_DEB")))

                            If forfaitCodes.Contains(codeHDFCheck) AndAlso quantite > 1 Then
                                Dim dailyAmount As Decimal = montant / quantite
                                For i As Integer = 0 To quantite - 1
                                    Dim newRow As DataRow = dt.NewRow()
                                    newRow.ItemArray = row.ItemArray.Clone()
                                    newRow("عدد الفحوصات") = 1
                                    newRow("تاريخ الفحص") = dateDeb.AddDays(i)

                                    If typeRepartition = "etab" Then
                                        newRow("حصة المستشفى المطلوبة") = dailyAmount
                                        newRow("حصة الطبيب المطلوبة") = 0D
                                    ElseIf typeRepartition = "med" Then
                                        newRow("حصة الطبيب المطلوبة") = dailyAmount
                                        newRow("حصة المستشفى المطلوبة") = 0D
                                    Else
                                        newRow("حصة الطبيب المطلوبة") = 0D
                                        newRow("حصة المستشفى المطلوبة") = 0D
                                    End If

                                    If newRow("حصة الطبيب المطلوبة") IsNot DBNull.Value AndAlso Convert.ToDecimal(newRow("حصة الطبيب المطلوبة")) = 0 Then
                                        newRow("رقم الطبيب في النقابة") = DBNull.Value
                                        newRow("نوع الطبيب") = DBNull.Value
                                    End If

                                    If row("حصة الطبيب المطلوبة") IsNot DBNull.Value AndAlso Convert.ToDecimal(row("حصة الطبيب المطلوبة")) = 0 Then
                                        row("رقم الطبيب في النقابة") = DBNull.Value
                                        row("نوع الطبيب") = DBNull.Value
                                        dt.Rows.Add(newRow)
                                    End If

                                Next
                            Else
                                dt.Rows.Add(row)
                            End If


                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ أثناء الاتصال بقاعدة البيانات: " & ex.Message)
        End Try
        Return dt
    End Function

    Private Sub btnOpenEditor_Click(sender As Object, e As EventArgs)
        Dim editor As New FormEditNDA()
        editor.ShowDialog()
    End Sub

    Private Async Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        If savedData Is Nothing OrElse savedData.Rows.Count = 0 Then
            MessageBox.Show("لا توجد بيانات لحفظها.", "لا توجد بيانات للحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim ndaToCheck As String = savedData.Rows(0)("NDA").ToString()

        ProgressBarSave.Value = 0
        ProgressBarSave.Visible = True
        ButtonSave.Enabled = False

        Try
            Dim progressValue As Integer = 0

            Await Task.Run(Sub()
                               If NDAExistsInMSSQL(ndaToCheck) Then

                                   progressValue = 100
                                   Invoke(Sub()
                                              ProgressBarSave.Value = progressValue
                                              MessageBox.Show("ملف مسجل مسبقًا في الجدول.", "البيانات موجودة بالفعل", MessageBoxButtons.OK, MessageBoxIcon.Warning)

                                          End Sub)
                                   Return
                               End If

                               For i = 1 To 5
                                   Threading.Thread.Sleep(200)
                                   progressValue = i * 20
                                   Invoke(Sub() ProgressBarSave.Value = progressValue)
                               Next

                               SaveToMSSQL_Bulk(savedData)

                               Invoke(Sub()
                                          ProgressBarSave.Value = 100
                                      End Sub)
                           End Sub)

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء الحفظ: " & vbCrLf & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            ProgressBarSave.Visible = False
            ProgressBarSave.Value = 0
            ButtonSave.Enabled = True
        End Try
        ClearDoctorInfoWhereShareIsZero()
        Reset()

    End Sub



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ButtonSubmit.Enabled = False
        ButtonSave.Enabled = False

        If Not String.IsNullOrEmpty(SelectedNumBord) Then
            lblBordereauInfo.Text = $"رقم الجدول: {SelectedNumBord}"
        End If

        TextBoxFullName = New TextBox()
        With TextBoxFullName
            .Name = "TextBoxFullName"
            .Visible = False
            .ReadOnly = True
            .Font = New Font("Segoe UI", 15, FontStyle.Bold)
            .Location = New Point(452, 128)
            .AutoSize = False
            .Size = New Size(200, 40)
            .Text = ""
        End With
        Me.Controls.Add(TextBoxFullName)

        ProgressBarSave = New ProgressBar()
        With ProgressBarSave
            .Name = "ProgressBarSave"
            .Style = ProgressBarStyle.Continuous
            .Visible = False
            .Width = 300
            .Height = 25
            .Top = ButtonSave.Bottom + 10
            .Left = ButtonSave.Left
            .Minimum = 0
            .Maximum = 100
            .Value = 0
        End With
        Me.Controls.Add(ProgressBarSave)

        TextBoxPEC.Enabled = False
        TextBoxContrat.Enabled = False
        TextBoxICD1.Enabled = False
        TextBoxICD2.Enabled = False
        TextBoxICD3.Enabled = False
        TextBoxICD4.Enabled = False
        TextBoxICD5.Enabled = False

    End Sub
    Private Sub TextBoxNDA_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBoxNDA.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True

            Dim nda As String = TextBoxNDA.Text.Trim()

            If Not NDAExistsInOracle(nda) Then
                MessageBox.Show("الملف غير موجود في قاعدة البيانات. الرجاء التحقق من الرقم.", "ملف غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                ndaValidated = False
                DisableInputFields()
                ButtonSubmit.Enabled = False
                Return
            End If

            Dim numBord As String = GetNumBordByNDA(nda)
            If NDAExistsInMSSQL(nda) Then
                MessageBox.Show(String.Format("بيانات هذا الملف موجودة في جدول رقم {0} الرجاء مراجعتها أو تعديلها بدلاً من إرسالها.", numBord),
                            "البيانات موجودة بالفعل", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ndaValidated = True
            GetPatientFullName()
            EnableInputFields()
            ButtonSubmit.Enabled = True
        End If
    End Sub
    Private Function NDAExistsInOracle(nda As String) As Boolean
        Dim exists As Boolean = False
        Dim connStr As String = ConfigurationManager.ConnectionStrings("OracleConnection").ConnectionString

        Using conn As New OracleConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT COUNT(*) FROM PENSOINS.SEJOUR WHERE NDA = :NDA"
            Using cmd As New OracleCommand(query, conn)
                cmd.Parameters.Add("NDA", OracleDbType.Varchar2).Value = nda
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                exists = (count > 0)
            End Using
        End Using

        Return exists
    End Function


    Private Sub ResizeTextBox()
        Dim tb = DirectCast(Me.Controls("TextBoxFullName"), TextBox)

        Using g As Graphics = tb.CreateGraphics()
            Dim textSize = g.MeasureString(tb.Text, tb.Font)
            tb.Width = CInt(textSize.Width) + 20
            tb.Height = CInt(textSize.Height) + 10
        End Using
    End Sub



    Private Sub GetPatientFullName()
        Dim nda As String = TextBoxNDA.Text.Trim()

        If String.IsNullOrEmpty(nda) Then
            MessageBox.Show("الرجاء إدخال الملف.", "خطأ في التحقق من الصحة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TextBoxFullName.Visible = False
            Return
        End If

        Dim connStr As String = ConfigurationManager.ConnectionStrings("OracleConnection").ConnectionString

        Try
            Using conn As New OracleConnection(connStr)
                conn.Open()

                Dim query As String = "
                     SELECT DISTINCT
                       PATIENT_SOIGNE.PRENOM,
                       PATIENT_SOIGNE.NOM,
                       PATIENT_SOIGNE.PATRONYME
                     FROM
                       PENSOINS.PATIENT PATIENT_SOIGNE,
                       PENSOINS.SEJOUR
                     WHERE
                       PATIENT_SOIGNE.NIPATIENT(+) = PENSOINS.SEJOUR.NIPATIENT
                       AND PENSOINS.SEJOUR.NDA = :NDA"

                Using cmd As New OracleCommand(query, conn)
                    cmd.Parameters.Add("NDA", OracleDbType.Varchar2).Value = nda

                    Using reader As OracleDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim firstName As String = If(IsDBNull(reader("PRENOM")), "", reader("PRENOM").ToString())
                            Dim PATRONYME As String = If(IsDBNull(reader("PATRONYME")), "", reader("PATRONYME").ToString())
                            Dim lastName As String = If(IsDBNull(reader("NOM")), "", reader("NOM").ToString())
                            TextBoxFullName.Text = $"{firstName} {lastName} {PATRONYME}".Trim()

                            ResizeTextBox()
                            TextBoxFullName.Refresh()

                            TextBoxFullName.Visible = True
                        Else
                            MessageBox.Show("لم يتم العثور على أي مريض بهذا الملف.", "لم يتم العثور على مريض", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            TextBoxFullName.Visible = False
                            DisableInputFields()
                            MessageBox.Show("hi", "test", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("خطأ أثناء استرداد اسم المريض: " & ex.Message)
            TextBoxFullName.Visible = False
        End Try

    End Sub
    Private Sub TextBoxContrat_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxContrat.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    Private Sub TextBoxPEC_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxPEC.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub SaveToMSSQL_Bulk(dt As DataTable)

        Try
            Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

            If Not dt.Columns.Contains("NUM_BORD") Then
                dt.Columns.Add("NUM_BORD", GetType(Integer))
            End If

            For Each row As DataRow In dt.Rows
                row("NUM_BORD") = SelectedNumBord
            Next

            Using conn As New SqlConnection(connStr)
                conn.Open()
                Using bulkCopy As New SqlBulkCopy(conn)
                    bulkCopy.DestinationTableName = "dbo.Detail_Bord"
                    bulkCopy.ColumnMappings.Add("رقم بطاقة الاستشفاء", "PEC")
                    bulkCopy.ColumnMappings.Add("تاريخ الدخول", "DateEntree")
                    bulkCopy.ColumnMappings.Add("تاريخ الخروج", "DateSortie")
                    bulkCopy.ColumnMappings.Add("تاريخ الفحص", "DateAct")
                    bulkCopy.ColumnMappings.Add("عدد الفحوصات", "Quantite")
                    bulkCopy.ColumnMappings.Add("رقم العمل الجراحي", "CodeAct")
                    bulkCopy.ColumnMappings.Add("رقم تسلسل العمل الجراحي", "CodeSequence")
                    bulkCopy.ColumnMappings.Add("رقم الطبيب في النقابة", "NADELI")
                    bulkCopy.ColumnMappings.Add("نوع الطبيب", "Specialite")
                    bulkCopy.ColumnMappings.Add("حصة الطبيب المطلوبة", "PartMedecin")
                    bulkCopy.ColumnMappings.Add("حصة المستشفى المطلوبة", "PartHopital")
                    bulkCopy.ColumnMappings.Add("تغطية الوزارة", "Couverture")
                    bulkCopy.ColumnMappings.Add("تشخيص أساسي", "ICD1")
                    bulkCopy.ColumnMappings.Add("تشخيص ثانوي أول", "ICD2")
                    bulkCopy.ColumnMappings.Add("تشخيص ثانوي ثاني", "ICD3")
                    bulkCopy.ColumnMappings.Add("تشخيص ثانوي ثالث", "ICD4")
                    bulkCopy.ColumnMappings.Add("تشخيص ثانوي رابع", "ICD5")
                    bulkCopy.ColumnMappings.Add("رقم العقد السنوي", "Contrat")
                    bulkCopy.ColumnMappings.Add("CODE_HDF", "CODE_HDF")
                    bulkCopy.ColumnMappings.Add("LIBELLE", "LIBELLE")
                    bulkCopy.ColumnMappings.Add("NDA", "NDA")
                    bulkCopy.ColumnMappings.Add("PRENOM", "PRENOM")
                    bulkCopy.ColumnMappings.Add("PATRONYME", "PATRONYME")
                    bulkCopy.ColumnMappings.Add("NOM", "NOM")
                    bulkCopy.ColumnMappings.Add("NUMERO_FACTURE", "NUMERO_FACTURE")
                    bulkCopy.ColumnMappings.Add("NUMERO", "NUMERO")
                    bulkCopy.ColumnMappings.Add("ASSURANCE", "ASSURANCE")
                    bulkCopy.ColumnMappings.Add("TYPE_REPARTITION", "TYPE_REPARTITION")
                    bulkCopy.ColumnMappings.Add("DATE_ENTREE_MVT", "DATE_ENTREE_MVT")
                    bulkCopy.ColumnMappings.Add("DATE_SORTIE_MVT", "DATE_SORTIE_MVT")
                    bulkCopy.ColumnMappings.Add("MED_RESP", "MED_RESP")
                    bulkCopy.ColumnMappings.Add("NUM_BORD", "NUM_BORD")
                    bulkCopy.ColumnMappings.Add("SPECIALITE_HDF", "SPECIALITE_HDF")
                    bulkCopy.ColumnMappings.Add("EXECUTANT", "EXECUTANT")
                    bulkCopy.ColumnMappings.Add("SPEC_EXECUTANT", "SPEC_EXECUTANT")
                    bulkCopy.ColumnMappings.Add("PART", "PART")
                    bulkCopy.ColumnMappings.Add("CODE_REG", "CODE_REG")
                    bulkCopy.WriteToServer(dt)
                End Using
            End Using

            MessageBox.Show("تم حفظ الملف بنجاح  .", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("خطأ أثناء الحفظ السريع: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ExportToExcel(dt As DataTable)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            MessageBox.Show("Aucune donnée à exporter.")
            Return
        End If

        Try
            Dim excelApp As New Microsoft.Office.Interop.Excel.Application
            Dim workbook As Microsoft.Office.Interop.Excel.Workbook = excelApp.Workbooks.Add()
            Dim worksheet As Microsoft.Office.Interop.Excel.Worksheet = workbook.Sheets(1)
            excelApp.Visible = False
            For col As Integer = 0 To dt.Columns.Count - 1
                worksheet.Cells(1, col + 1) = dt.Columns(col).ColumnName
            Next

            Dim data(dt.Rows.Count - 1, dt.Columns.Count - 1) As Object
            For row As Integer = 0 To dt.Rows.Count - 1
                For col As Integer = 0 To dt.Columns.Count - 1
                    data(row, col) = dt.Rows(row)(col)
                Next
            Next


            Dim startCell As Microsoft.Office.Interop.Excel.Range = worksheet.Cells(2, 1)
            Dim endCell As Microsoft.Office.Interop.Excel.Range = worksheet.Cells(dt.Rows.Count + 1, dt.Columns.Count)
            worksheet.Range(startCell, endCell).Value = data

            worksheet.Columns.AutoFit()

            Dim saveDialog As New SaveFileDialog With {
            .Filter = "Excel Workbook|*.xlsx",
            .Title = "Save as Excel File",
            .FileName = "ExportedData.xlsx"
        }

            If saveDialog.ShowDialog() = DialogResult.OK Then
                workbook.SaveAs(saveDialog.FileName)
                MessageBox.Show("Données exportées avec succès.")
            End If

            workbook.Close(False)
            excelApp.Quit()

            Marshal.ReleaseComObject(worksheet)
            Marshal.ReleaseComObject(workbook)
            Marshal.ReleaseComObject(excelApp)

        Catch ex As Exception
            MessageBox.Show("Erreur lors de l'exportation : " & ex.Message)
        End Try
    End Sub

    Private Sub ButtonExportExcel_Click(sender As Object, e As EventArgs)
        Dim exportThread As New Threading.Thread(AddressOf PerformExport)
        exportThread.SetApartmentState(Threading.ApartmentState.STA)
        exportThread.Start()
    End Sub

    Private Sub PerformExport()
        Dim nda As String = ""
        Me.Invoke(Sub() nda = TextBoxNDA.Text.Trim())

        If String.IsNullOrEmpty(nda) Then
            MessageBox.Show("Please enter an NDA.")
            Return
        End If

        Try
            Dim exportedData As DataTable
            Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

            Using conn As New SqlConnection(connStr)
                conn.Open()

                Dim query As String = "
                SELECT 
                    PEC, DateEntree, DateSortie, DateAct, Quantite, CodeAct, CodeSequence,
                    NADELI, Specialite, PartMedecin, PartHopital, Couverture,
                    ICD1, ICD2, ICD3, ICD4, ICD5, Contrat
                FROM dbo.Detail_Bord
                WHERE nda = @nda AND BORD_CLOSED = 0;"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@nda", nda)
                    Dim adapter As New SqlDataAdapter(cmd)
                    exportedData = New DataTable()
                    adapter.Fill(exportedData)
                End Using

                If exportedData.Rows.Count = 0 Then
                    MessageBox.Show("No data found or already exported.")
                    Return
                End If

                Dim lastBord As Integer = 0
                Using cmdLast As New SqlCommand("SELECT ISNULL(MAX(Num_bord), 0) FROM dbo.num_bordereau", conn)
                    lastBord = Convert.ToInt32(cmdLast.ExecuteScalar())
                End Using
                Dim newBord As Integer = lastBord + 1

                Using cmdInsertBord As New SqlCommand("INSERT INTO dbo.num_bordereau (Num_bord, DateExported) VALUES (@nb, @dt)", conn)
                    cmdInsertBord.Parameters.AddWithValue("@nb", newBord)
                    cmdInsertBord.Parameters.AddWithValue("@dt", DateTime.Now)
                    cmdInsertBord.ExecuteNonQuery()
                End Using

                Using cmdUpdate As New SqlCommand("UPDATE dbo.Detail_Bord SET Num_bord = @nb, BORD_CLOSED = 1 WHERE NDA = @nda AND BORD_CLOSED = 0", conn)
                    cmdUpdate.Parameters.AddWithValue("@nb", newBord)
                    cmdUpdate.Parameters.AddWithValue("@nda", nda)
                    cmdUpdate.ExecuteNonQuery()
                End Using

                ExportToExcel(exportedData)

            End Using
        Catch ex As Exception
            MessageBox.Show("Error during export: " & ex.Message)
        End Try
    End Sub

    Private Function NDAExistsInMSSQL(nda As String) As Boolean
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
        Dim query As String = "SELECT COUNT(*) FROM Detail_Bord WHERE NDA = @NDA"
        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@NDA", nda)
                conn.Open()
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Private Sub btnCloseForm_Click(sender As Object, e As EventArgs) Handles btnCloseForm.Click
        Me.Close()
    End Sub

    Private Sub EnableInputFields()
        TextBoxPEC.Enabled = True
        TextBoxContrat.Enabled = True
        TextBoxICD1.Enabled = True
        TextBoxICD2.Enabled = True
        TextBoxICD3.Enabled = True
        TextBoxICD4.Enabled = True
        TextBoxICD5.Enabled = True
    End Sub

    Private Sub DisableInputFields()
        TextBoxPEC.Enabled = False
        TextBoxContrat.Enabled = False
        TextBoxICD1.Enabled = False
        TextBoxICD2.Enabled = False
        TextBoxICD3.Enabled = False
        TextBoxICD4.Enabled = False
        TextBoxICD5.Enabled = False
    End Sub
    Private Sub Reset()

        ButtonSubmit.Enabled = False
        TextBoxFullName.Visible = False
        ButtonSave.Enabled = False
        TextBoxNDA.Text = ""
        TextBoxPEC.Text = ""
        TextBoxContrat.Text = ""
        TextBoxICD1.Text = ""
        TextBoxICD2.Text = ""
        TextBoxICD3.Text = ""
        TextBoxICD4.Text = ""
        TextBoxICD5.Text = ""
    End Sub

    Private Function GetNumBordByNDA(nda As String) As String
        Dim result As String = ""
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
        Dim query As String = "SELECT DISTINCT NUM_BORD FROM dbo.Detail_Bord WHERE NDA = @NDA"

        Try
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@NDA", nda)

                    conn.Open()

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            result = reader("NUM_BORD").ToString()
                        Else
                            result = "لا يوجد رقم بورد لهذا الـNDA"
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء جلب رقم البورد: " & vbCrLf & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            result = "خطأ تقني"
        End Try

        Return result
    End Function

    Private Function ConvertNadeliToArabic(nadeliRaw As String, letterMap As Dictionary(Of String, String)) As String
        If String.IsNullOrWhiteSpace(nadeliRaw) Then Return ""

        If nadeliRaw.Contains("-") Then
            Dim parts = nadeliRaw.Split("-"c)
            If parts.Length = 2 Then
                Dim prefix = parts(0)
                Dim suffix = parts(1).ToUpper()
                Dim arabicLetter As String = If(letterMap.ContainsKey(suffix), letterMap(suffix), suffix)
                Return prefix & "/" & arabicLetter
            End If
        End If

        Return nadeliRaw
    End Function


    Private Function LoadLetterMapping() As Dictionary(Of String, String)
        Dim map As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT OrDer_Letter_Fr, OrDer_Letter_Ar FROM dbo.ORDER_LETTER"
            Using cmd As New SqlCommand(query, conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim latin = reader.GetString(0).ToUpper()
                        Dim arabic = reader.GetString(1)
                        If Not map.ContainsKey(latin) Then
                            map.Add(latin, arabic)
                        End If
                    End While
                End Using
            End Using
        End Using

        Return map
    End Function
    Private Function LoadValidAssuranceCodes() As HashSet(Of String)
        Dim validCodes As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
        Dim query As String = "SELECT Code FROM ValidAssurance"

        Using conn As New SqlConnection(connStr)
            conn.Open()
            Using cmd As New SqlCommand(query, conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        validCodes.Add(reader.GetString(0))
                    End While
                End Using
            End Using
        End Using

        Return validCodes
    End Function

    Function GetArabicLetterForSpecialty(specialty As String, nadeli As String, nadeliExecutant As String) As String
        Dim spec As String = If(specialty, "").ToUpper().Trim()

        Dim chirurgienSpecialties As String() = {
        "CHIR. PLASTIQUE ET REPARATRICE",
        "CHIR.CARDIOVASCULAIRE & THORA",
        "CHIRURGIE DIGESTIVE ET ENDOCRINO",
        "CHIRURGIE GENERALE",
        "CHIRURGIE PEDIATRIQUE",
        "NEUROCHIRURGIE"
    }

        If spec.Contains("ANESTHESIE") Then Return "م"

        For Each chir In chirurgienSpecialties
            If spec.Contains(chir) Then Return "ج"
        Next

        Dim Normalize = Function(name As String) String.Join(" ", name.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)).Trim().ToUpper()
        Dim n1 = Normalize(nadeli)
        Dim n2 = Normalize(nadeliExecutant)
        If String.IsNullOrWhiteSpace(n2) Then
            Return "ع"
        End If

        If n1 = n2 Then
            Return "ع"
        Else
            Return "س"
        End If
    End Function


    Private Function GetCodeSequenceValue(code As String) As Decimal
        If String.IsNullOrWhiteSpace(code) Then Return 0D

        code = code.Trim().ToUpper()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT COUNT(*) FROM dbo.moulhak WHERE UPPER(LTRIM(RTRIM(code))) = @code"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@code", code)
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                Return If(count > 0, 2D, 0D)
            End Using
        End Using
    End Function


    Private Function LoadValidCodesFromMoulhak() As HashSet(Of String)
        Dim validCodes As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT code FROM dbo.moulhak"
            Using cmd As New SqlCommand(query, conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        If Not reader.IsDBNull(0) Then
                            validCodes.Add(reader.GetString(0).Trim().ToUpper())
                        End If
                    End While
                End Using
            End Using
        End Using

        Return validCodes
    End Function


    Private Sub TextBoxNDA_TextChanged(sender As Object, e As EventArgs) Handles TextBoxNDA.TextChanged
        DisableInputFields()
        ButtonSubmit.Enabled = False
        TextBoxFullName.Visible = False
        ButtonSave.Enabled = False
        TextBoxPEC.Text = ""
        TextBoxContrat.Text = ""
        TextBoxICD1.Text = ""
        TextBoxICD2.Text = ""
        TextBoxICD3.Text = ""
        TextBoxICD4.Text = ""
        TextBoxICD5.Text = ""

    End Sub


    Private Function GetPartValueFromGef(codeGef As String,
                                     gefToNssfMap As Dictionary(Of String, String),
                                     gefToRefComMap As Dictionary(Of String, String)) As String
        If String.IsNullOrWhiteSpace(codeGef) Then Return ""

        codeGef = codeGef.Trim().ToUpper()

        If gefToNssfMap.ContainsKey(codeGef) AndAlso Not String.IsNullOrWhiteSpace(gefToNssfMap(codeGef)) Then
            Return gefToNssfMap(codeGef)
        ElseIf gefToRefComMap.ContainsKey(codeGef) AndAlso Not String.IsNullOrWhiteSpace(gefToRefComMap(codeGef)) Then
            Return gefToRefComMap(codeGef)
        Else
            Return ""
        End If
    End Function

    Private Function LoadGefMappingsFromFirstQuery() As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        Dim connStr As String = ConfigurationManager.ConnectionStrings("OracleConnection").ConnectionString

        Using conn As New OracleConnection(connStr)
            conn.Open()

            Dim query As String = "
            SELECT DISTINCT
                PL.CODE_GEF AS Code_Gestion_Produit,
                C_DM.CODE_REFCOM_RECH AS CODE_NSSF
            FROM PHARM.PHR_PRODUIT_LIVRET PL
            JOIN PENSOINS.C_REF L ON L.NIC_REF = PL.NIC_REF
            JOIN PHARM.PHR_PRODUIT P ON P.NIPRODUIT = PL.NIPRODUIT
            JOIN PENSOINS.C_TYPE_REF T ON T.NIC_TYPE_REF = L.NIC_TYPE_REF AND T.CODE = 'C_SPECIALITE'
            JOIN PHARM.PHR_TYPECOM ON P.NITYPE = PHR_TYPECOM.NI
            JOIN PENSOINS.C_DM ON P.NIREF = C_DM.NIDM
            WHERE PL.RETRAIT = 'F'
              AND P.RETRAIT = 'F'
              AND PHR_TYPECOM.LIBELLE = 'Dispositif médical'
              AND PL.CODE_GEF IS NOT NULL
        "

            Using cmd As New OracleCommand(query, conn)
                Using reader As OracleDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim gef As String = reader("Code_Gestion_Produit").ToString().Trim().ToUpper()
                        Dim nssf As String = reader("CODE_NSSF").ToString().Trim()
                        If Not result.ContainsKey(gef) Then
                            result(gef) = nssf
                        End If
                    End While
                End Using
            End Using
        End Using

        Return result
    End Function

    Private Function LoadGefMappingsFromSecondQuery() As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        Dim connStr As String = ConfigurationManager.ConnectionStrings("OracleConnection").ConnectionString

        Using conn As New OracleConnection(connStr)
            conn.Open()

            Dim query As String = "
            SELECT DISTINCT PHARM.PHR_PRODUIT_LIVRET.CODE_GEF, PRESCR_MEDIC.REF_COM
            FROM PENSOINS.C_SPECIALITE PRESCR_MEDIC
            JOIN PHARM.PHR_PRODUIT PHARM_PHR_PRODUIT_PRINCIP ON PHARM_PHR_PRODUIT_PRINCIP.NIREF = PRESCR_MEDIC.NIMED
            JOIN PHARM.PHR_TYPECOM ON PHARM_PHR_PRODUIT_PRINCIP.NITYPE = PHR_TYPECOM.NI
            JOIN PHARM.PHR_PRODUIT_LIVRET ON PHARM.PHR_PRODUIT_LIVRET.NIPRODUIT = PHARM_PHR_PRODUIT_PRINCIP.NIPRODUIT
            WHERE PHARM_PHR_PRODUIT_PRINCIP.RETRAIT = 'F'
              AND PHR_TYPECOM.LIBELLE = 'Médicament'
              AND PHARM.PHR_PRODUIT_LIVRET.RETRAIT = 'F'
        "

            Using cmd As New OracleCommand(query, conn)
                Using reader As OracleDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim gef As String = reader("CODE_GEF").ToString().Trim().ToUpper()
                        Dim refCom As String = reader("REF_COM").ToString().Trim()
                        If Not result.ContainsKey(gef) Then
                            result(gef) = refCom
                        End If
                    End While
                End Using
            End Using
        End Using

        Return result
    End Function

    Private Function LoadValidCodesFromForfait() As HashSet(Of String)
        Dim validCodes As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString
        Dim query As String = "SELECT CODE FROM dbo.forfait"

        Using conn As New SqlConnection(connStr)
            conn.Open()
            Using cmd As New SqlCommand(query, conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        If Not reader.IsDBNull(0) Then
                            validCodes.Add(reader.GetString(0).Trim().ToUpper())
                        End If
                    End While
                End Using
            End Using
        End Using

        Return validCodes
    End Function

    Private Sub ClearDoctorInfoWhereShareIsZero()
        Try
            Dim connStr As String = ConfigurationManager.ConnectionStrings("MSSQLConnection").ConnectionString

            Using conn As New SqlConnection(connStr)
                conn.Open()

                Dim sql As String = "
                UPDATE dbo.Detail_Bord
                SET NADELI = NULL,
                    Specialite= NULL
                WHERE PartMedecin = 0;
            "


                Using cmd As New SqlCommand(sql, conn)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error while updating doctor info: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


End Class