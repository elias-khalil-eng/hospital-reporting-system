Public Class FormEditFields
    Private Sub TextBoxContratEdit_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxContratEdit.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub TextBoxPECEdit_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxPECEdit.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If ValidateFields() Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Public ReadOnly Property ContratValue As String
        Get
            Return TextBoxContratEdit.Text.Trim()
        End Get
    End Property

    Public ReadOnly Property PECValue As String
        Get
            Return TextBoxPECEdit.Text.Trim()
        End Get
    End Property

    Public ReadOnly Property ICD10_1_Value As String
        Get
            Return TextBoxICD1Edit.Text.Trim()
        End Get
    End Property

    Public ReadOnly Property ICD10_2_Value As String
        Get
            Return TextBoxICD2Edit.Text.Trim()
        End Get
    End Property

    Public ReadOnly Property ICD10_3_Value As String
        Get
            Return TextBoxICD3Edit.Text.Trim()
        End Get
    End Property

    Public ReadOnly Property ICD10_4_Value As String
        Get
            Return TextBoxICD4Edit.Text.Trim()
        End Get
    End Property

    Public ReadOnly Property ICD10_5_Value As String
        Get
            Return TextBoxICD5Edit.Text.Trim()
        End Get
    End Property
    Public Function ValidateFields() As Boolean
        If String.IsNullOrWhiteSpace(TextBoxContratEdit.Text) OrElse
       String.IsNullOrWhiteSpace(TextBoxPECEdit.Text) OrElse
       String.IsNullOrWhiteSpace(TextBoxICD1Edit.Text) Then

            MessageBox.Show("يجب ملء جميع الحقول. الرجاء إكمال جميع المعلومات المطلوبة.", "خطأ في التحقق من الصحة", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function

End Class