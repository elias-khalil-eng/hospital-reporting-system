<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Title = New System.Windows.Forms.Label()
        Me.btnLoadBordereaux = New System.Windows.Forms.Button()
        Me.btnOpenEditor = New System.Windows.Forms.Button()
        Me.dgvBordereaux = New System.Windows.Forms.DataGridView()
        Me.ADD_BORD = New System.Windows.Forms.Button()
        Me.ButtonExportExcel = New System.Windows.Forms.Button()
        Me.CLOSE_BORD = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.CLOSE_ALL = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnOpenMoulhak = New System.Windows.Forms.Button()
        Me.dgvMoulhak = New System.Windows.Forms.DataGridView()
        Me.btnSaveMoulhak = New System.Windows.Forms.Button()
        Me.Button_close_moulhak = New System.Windows.Forms.Button()
        Me.btnImportMoulhakExcel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnAdminAccess = New System.Windows.Forms.Button()
        CType(Me.dgvBordereaux, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvMoulhak, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(49, 472)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(257, 127)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "إضافة ملف"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'Title
        '
        Me.Title.AutoSize = True
        Me.Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 40.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Title.Location = New System.Drawing.Point(729, 39)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(387, 63)
        Me.Title.TabIndex = 1
        Me.Title.Text = "جدول فواتير الوزارة"
        '
        'btnLoadBordereaux
        '
        Me.btnLoadBordereaux.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadBordereaux.Location = New System.Drawing.Point(928, 301)
        Me.btnLoadBordereaux.Name = "btnLoadBordereaux"
        Me.btnLoadBordereaux.Size = New System.Drawing.Size(257, 127)
        Me.btnLoadBordereaux.TabIndex = 5
        Me.btnLoadBordereaux.Text = "البحث عن جدول"
        '
        'btnOpenEditor
        '
        Me.btnOpenEditor.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOpenEditor.Location = New System.Drawing.Point(49, 618)
        Me.btnOpenEditor.Name = "btnOpenEditor"
        Me.btnOpenEditor.Size = New System.Drawing.Size(257, 127)
        Me.btnOpenEditor.TabIndex = 6
        Me.btnOpenEditor.Text = "بحث عن ملف"
        Me.btnOpenEditor.UseVisualStyleBackColor = True
        Me.btnOpenEditor.Visible = False
        '
        'dgvBordereaux
        '
        Me.dgvBordereaux.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBordereaux.Location = New System.Drawing.Point(42, 247)
        Me.dgvBordereaux.Name = "dgvBordereaux"
        Me.dgvBordereaux.Size = New System.Drawing.Size(503, 155)
        Me.dgvBordereaux.TabIndex = 7
        Me.dgvBordereaux.Visible = False
        '
        'ADD_BORD
        '
        Me.ADD_BORD.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ADD_BORD.Location = New System.Drawing.Point(627, 472)
        Me.ADD_BORD.Name = "ADD_BORD"
        Me.ADD_BORD.Size = New System.Drawing.Size(257, 127)
        Me.ADD_BORD.TabIndex = 8
        Me.ADD_BORD.Text = "إضافة جدول"
        Me.ADD_BORD.UseVisualStyleBackColor = True
        '
        'ButtonExportExcel
        '
        Me.ButtonExportExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportExcel.Location = New System.Drawing.Point(627, 301)
        Me.ButtonExportExcel.Name = "ButtonExportExcel"
        Me.ButtonExportExcel.Size = New System.Drawing.Size(257, 127)
        Me.ButtonExportExcel.TabIndex = 9
        Me.ButtonExportExcel.Text = "تصدير جدول"
        Me.ButtonExportExcel.UseVisualStyleBackColor = True
        '
        'CLOSE_BORD
        '
        Me.CLOSE_BORD.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CLOSE_BORD.Location = New System.Drawing.Point(928, 472)
        Me.CLOSE_BORD.Name = "CLOSE_BORD"
        Me.CLOSE_BORD.Size = New System.Drawing.Size(257, 127)
        Me.CLOSE_BORD.TabIndex = 10
        Me.CLOSE_BORD.Text = "إقفال الجدول"
        Me.CLOSE_BORD.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(49, 763)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(257, 127)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "إغلاق"
        Me.btnClose.UseVisualStyleBackColor = True
        Me.btnClose.Visible = False
        '
        'CLOSE_ALL
        '
        Me.CLOSE_ALL.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CLOSE_ALL.Location = New System.Drawing.Point(765, 782)
        Me.CLOSE_ALL.Name = "CLOSE_ALL"
        Me.CLOSE_ALL.Size = New System.Drawing.Size(257, 127)
        Me.CLOSE_ALL.TabIndex = 13
        Me.CLOSE_ALL.Text = "إنهاء العمل"
        Me.CLOSE_ALL.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(2, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(227, 82)
        Me.PictureBox1.TabIndex = 12
        Me.PictureBox1.TabStop = False
        '
        'btnOpenMoulhak
        '
        Me.btnOpenMoulhak.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOpenMoulhak.Location = New System.Drawing.Point(627, 636)
        Me.btnOpenMoulhak.Name = "btnOpenMoulhak"
        Me.btnOpenMoulhak.Size = New System.Drawing.Size(257, 127)
        Me.btnOpenMoulhak.TabIndex = 14
        Me.btnOpenMoulhak.Text = "ملحق"
        '
        'dgvMoulhak
        '
        Me.dgvMoulhak.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMoulhak.Location = New System.Drawing.Point(1223, 149)
        Me.dgvMoulhak.Name = "dgvMoulhak"
        Me.dgvMoulhak.Size = New System.Drawing.Size(605, 344)
        Me.dgvMoulhak.TabIndex = 15
        Me.dgvMoulhak.Visible = False
        '
        'btnSaveMoulhak
        '
        Me.btnSaveMoulhak.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveMoulhak.Location = New System.Drawing.Point(1273, 545)
        Me.btnSaveMoulhak.Name = "btnSaveMoulhak"
        Me.btnSaveMoulhak.Size = New System.Drawing.Size(257, 127)
        Me.btnSaveMoulhak.TabIndex = 17
        Me.btnSaveMoulhak.Text = "حفظ التعديل"
        Me.btnSaveMoulhak.UseVisualStyleBackColor = True
        Me.btnSaveMoulhak.Visible = False
        '
        'Button_close_moulhak
        '
        Me.Button_close_moulhak.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_close_moulhak.Location = New System.Drawing.Point(1536, 545)
        Me.Button_close_moulhak.Name = "Button_close_moulhak"
        Me.Button_close_moulhak.Size = New System.Drawing.Size(257, 127)
        Me.Button_close_moulhak.TabIndex = 18
        Me.Button_close_moulhak.Text = "إغلاق"
        Me.Button_close_moulhak.UseVisualStyleBackColor = True
        Me.Button_close_moulhak.Visible = False
        '
        'btnImportMoulhakExcel
        '
        Me.btnImportMoulhakExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImportMoulhakExcel.Location = New System.Drawing.Point(1273, 678)
        Me.btnImportMoulhakExcel.Name = "btnImportMoulhakExcel"
        Me.btnImportMoulhakExcel.Size = New System.Drawing.Size(257, 127)
        Me.btnImportMoulhakExcel.TabIndex = 19
        Me.btnImportMoulhakExcel.Text = "استيراد"
        Me.btnImportMoulhakExcel.UseVisualStyleBackColor = True
        Me.btnImportMoulhakExcel.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 40.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label1.Location = New System.Drawing.Point(1481, 68)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 63)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "ملحق"
        Me.Label1.Visible = False
        '
        'btnAdminAccess
        '
        Me.btnAdminAccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdminAccess.Location = New System.Drawing.Point(928, 636)
        Me.btnAdminAccess.Name = "btnAdminAccess"
        Me.btnAdminAccess.Size = New System.Drawing.Size(257, 127)
        Me.btnAdminAccess.TabIndex = 22
        Me.btnAdminAccess.Text = "Admin"
        Me.btnAdminAccess.UseVisualStyleBackColor = True
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1840, 921)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnAdminAccess)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnImportMoulhakExcel)
        Me.Controls.Add(Me.Button_close_moulhak)
        Me.Controls.Add(Me.btnSaveMoulhak)
        Me.Controls.Add(Me.dgvMoulhak)
        Me.Controls.Add(Me.btnOpenMoulhak)
        Me.Controls.Add(Me.CLOSE_ALL)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.CLOSE_BORD)
        Me.Controls.Add(Me.ButtonExportExcel)
        Me.Controls.Add(Me.ADD_BORD)
        Me.Controls.Add(Me.dgvBordereaux)
        Me.Controls.Add(Me.btnOpenEditor)
        Me.Controls.Add(Me.btnLoadBordereaux)
        Me.Controls.Add(Me.Title)
        Me.Controls.Add(Me.Button1)
        Me.Location = New System.Drawing.Point(900, 200)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Main"
        Me.Text = "Main"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvBordereaux, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvMoulhak, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents Title As Label
    Private WithEvents btnLoadBordereaux As Button
    Friend WithEvents btnOpenEditor As Button
    Friend WithEvents dgvBordereaux As DataGridView
    Friend WithEvents ADD_BORD As Button
    Friend WithEvents ButtonExportExcel As Button
    Friend WithEvents CLOSE_BORD As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents CLOSE_ALL As Button
    Friend WithEvents PictureBox1 As PictureBox
    Private WithEvents btnOpenMoulhak As Button
    Friend WithEvents dgvMoulhak As DataGridView
    Friend WithEvents btnSaveMoulhak As Button
    Friend WithEvents Button_close_moulhak As Button
    Friend WithEvents btnImportMoulhakExcel As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents btnAdminAccess As Button
End Class
