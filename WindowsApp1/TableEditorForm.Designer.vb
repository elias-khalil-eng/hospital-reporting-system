
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TableEditorForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        Me.dgvEditor = New System.Windows.Forms.DataGridView()
        Me.btnSave = New System.Windows.Forms.Button()
        CType(Me.dgvEditor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvEditor
        '
        Me.dgvEditor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEditor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvEditor.Location = New System.Drawing.Point(0, 0)
        Me.dgvEditor.Name = "dgvEditor"
        Me.dgvEditor.Size = New System.Drawing.Size(800, 420)
        Me.dgvEditor.TabIndex = 0
        '
        'btnSave
        '
        Me.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnSave.Location = New System.Drawing.Point(0, 420)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(800, 30)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "Save Changes"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'TableEditorForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.dgvEditor)
        Me.Controls.Add(Me.btnSave)
        Me.Name = "TableEditorForm"
        Me.Text = "Table Editor"
        CType(Me.dgvEditor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvEditor As System.Windows.Forms.DataGridView
    Friend WithEvents btnSave As System.Windows.Forms.Button
End Class
