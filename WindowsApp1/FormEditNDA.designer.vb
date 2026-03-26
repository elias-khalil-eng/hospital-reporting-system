<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormEditNDA
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer
    Private ListBoxNDAs As ListBox
    Private DataGridView1 As DataGridView
    Private btnEdit As Button
    Private btnDelete As Button

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ListBoxNDAs = New System.Windows.Forms.ListBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnCloseForm = New System.Windows.Forms.Button()
        Me.lblBordereauInfo = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ListBoxNDAs
        '
        Me.ListBoxNDAs.ItemHeight = 17
        Me.ListBoxNDAs.Location = New System.Drawing.Point(20, 70)
        Me.ListBoxNDAs.Name = "ListBoxNDAs"
        Me.ListBoxNDAs.Size = New System.Drawing.Size(110, 106)
        Me.ListBoxNDAs.TabIndex = 2
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(136, 56)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(1717, 799)
        Me.DataGridView1.TabIndex = 3
        '
        'btnEdit
        '
        Me.btnEdit.Location = New System.Drawing.Point(17, 212)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(113, 40)
        Me.btnEdit.TabIndex = 4
        Me.btnEdit.Text = "تعديل ملف"
        Me.btnEdit.UseVisualStyleBackColor = True
        Me.btnEdit.Visible = False
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(17, 268)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(113, 40)
        Me.btnDelete.TabIndex = 5
        Me.btnDelete.Text = "حذف ملف"
        Me.btnDelete.UseVisualStyleBackColor = True
        Me.btnDelete.Visible = False
        '
        'btnCloseForm
        '
        Me.btnCloseForm.Location = New System.Drawing.Point(17, 329)
        Me.btnCloseForm.Name = "btnCloseForm"
        Me.btnCloseForm.Size = New System.Drawing.Size(113, 40)
        Me.btnCloseForm.TabIndex = 6
        Me.btnCloseForm.Text = "إغلاق"
        Me.btnCloseForm.UseVisualStyleBackColor = True
        '
        'lblBordereauInfo
        '
        Me.lblBordereauInfo.AutoSize = True
        Me.lblBordereauInfo.Location = New System.Drawing.Point(859, 23)
        Me.lblBordereauInfo.Name = "lblBordereauInfo"
        Me.lblBordereauInfo.Size = New System.Drawing.Size(41, 19)
        Me.lblBordereauInfo.TabIndex = 7
        Me.lblBordereauInfo.Text = "TITLE"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 19)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "اختر رقم الملف"
        '
        'FormEditNDA
        '
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1876, 847)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblBordereauInfo)
        Me.Controls.Add(Me.btnCloseForm)
        Me.Controls.Add(Me.ListBoxNDAs)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnDelete)
        Me.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormEditNDA"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit NDA Records"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnCloseForm As Button
    Friend WithEvents lblBordereauInfo As Label
    Friend WithEvents Label1 As Label
End Class
