<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class NUM_BORD
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btnForfai = New System.Windows.Forms.Button()
        Me.btnValidAssure = New System.Windows.Forms.Button()
        Me.btnOrderLetter = New System.Windows.Forms.Button()
        Me.btnClearTables = New System.Windows.Forms.Button()
        Me.adminGrid = New System.Windows.Forms.DataGridView()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnclose = New System.Windows.Forms.Button()
        Me.CloseForm = New System.Windows.Forms.Button()
        Me.btnExportDetailBord = New System.Windows.Forms.Button()
        Me.btnNum_bord = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.adminGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnForfai
        '
        Me.btnForfai.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnForfai.Location = New System.Drawing.Point(175, 74)
        Me.btnForfai.Name = "btnForfai"
        Me.btnForfai.Size = New System.Drawing.Size(213, 92)
        Me.btnForfai.TabIndex = 0
        Me.btnForfai.Text = "Forfai"
        Me.btnForfai.UseVisualStyleBackColor = True
        '
        'btnValidAssure
        '
        Me.btnValidAssure.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnValidAssure.Location = New System.Drawing.Point(175, 193)
        Me.btnValidAssure.Name = "btnValidAssure"
        Me.btnValidAssure.Size = New System.Drawing.Size(213, 92)
        Me.btnValidAssure.TabIndex = 1
        Me.btnValidAssure.Text = "Valid Assurance"
        Me.btnValidAssure.UseVisualStyleBackColor = True
        '
        'btnOrderLetter
        '
        Me.btnOrderLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOrderLetter.Location = New System.Drawing.Point(175, 312)
        Me.btnOrderLetter.Name = "btnOrderLetter"
        Me.btnOrderLetter.Size = New System.Drawing.Size(213, 92)
        Me.btnOrderLetter.TabIndex = 2
        Me.btnOrderLetter.Text = "Order Letter"
        Me.btnOrderLetter.UseVisualStyleBackColor = True
        '
        'btnClearTables
        '
        Me.btnClearTables.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearTables.Location = New System.Drawing.Point(175, 550)
        Me.btnClearTables.Name = "btnClearTables"
        Me.btnClearTables.Size = New System.Drawing.Size(213, 92)
        Me.btnClearTables.TabIndex = 3
        Me.btnClearTables.Text = "RESET TABLE"
        Me.btnClearTables.UseVisualStyleBackColor = True
        '
        'adminGrid
        '
        Me.adminGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.adminGrid.Location = New System.Drawing.Point(804, 50)
        Me.adminGrid.Name = "adminGrid"
        Me.adminGrid.Size = New System.Drawing.Size(546, 660)
        Me.adminGrid.TabIndex = 4
        Me.adminGrid.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(529, 282)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(213, 92)
        Me.btnSave.TabIndex = 5
        Me.btnSave.Text = "Save Changes"
        Me.btnSave.UseVisualStyleBackColor = True
        Me.btnSave.Visible = False
        '
        'btnclose
        '
        Me.btnclose.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnclose.Location = New System.Drawing.Point(529, 441)
        Me.btnclose.Name = "btnclose"
        Me.btnclose.Size = New System.Drawing.Size(213, 92)
        Me.btnclose.TabIndex = 6
        Me.btnclose.Text = "Close"
        Me.btnclose.UseVisualStyleBackColor = True
        Me.btnclose.Visible = False
        '
        'CloseForm
        '
        Me.CloseForm.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CloseForm.Location = New System.Drawing.Point(729, 827)
        Me.CloseForm.Name = "CloseForm"
        Me.CloseForm.Size = New System.Drawing.Size(279, 92)
        Me.CloseForm.TabIndex = 7
        Me.CloseForm.Text = "Close Admin Panel"
        Me.CloseForm.UseVisualStyleBackColor = True
        '
        'btnExportDetailBord
        '
        Me.btnExportDetailBord.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportDetailBord.Location = New System.Drawing.Point(175, 669)
        Me.btnExportDetailBord.Name = "btnExportDetailBord"
        Me.btnExportDetailBord.Size = New System.Drawing.Size(213, 92)
        Me.btnExportDetailBord.TabIndex = 8
        Me.btnExportDetailBord.Text = "Export Detail_Bord to Excel"
        Me.btnExportDetailBord.UseVisualStyleBackColor = True
        '
        'btnNum_bord
        '
        Me.btnNum_bord.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNum_bord.Location = New System.Drawing.Point(175, 431)
        Me.btnNum_bord.Name = "btnNum_bord"
        Me.btnNum_bord.Size = New System.Drawing.Size(213, 92)
        Me.btnNum_bord.TabIndex = 9
        Me.btnNum_bord.Text = "Num Bord"
        Me.btnNum_bord.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(865, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(102, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Made By Elias Khalil"
        '
        'NUM_BORD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1580, 931)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnNum_bord)
        Me.Controls.Add(Me.btnExportDetailBord)
        Me.Controls.Add(Me.CloseForm)
        Me.Controls.Add(Me.btnclose)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.adminGrid)
        Me.Controls.Add(Me.btnClearTables)
        Me.Controls.Add(Me.btnOrderLetter)
        Me.Controls.Add(Me.btnValidAssure)
        Me.Controls.Add(Me.btnForfai)
        Me.Name = "NUM_BORD"
        Me.Text = "Admin Panel"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.adminGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnForfai As Button
    Friend WithEvents btnValidAssure As Button
    Friend WithEvents btnOrderLetter As Button
    Friend WithEvents btnClearTables As Button
    Friend WithEvents adminGrid As DataGridView
    Friend WithEvents btnSave As Button
    Friend WithEvents btnclose As Button
    Friend WithEvents CloseForm As Button
    Friend WithEvents btnExportDetailBord As Button
    Friend WithEvents btnNum_bord As Button
    Friend WithEvents Label1 As Label
End Class
