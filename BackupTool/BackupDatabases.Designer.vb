<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frm_Backup
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Backup))
        Me.chklstDatabases = New System.Windows.Forms.CheckedListBox()
        Me.cmbServers = New System.Windows.Forms.ComboBox()
        Me.BtnBak = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btn_SelectAll = New System.Windows.Forms.Button()
        Me.btn_Shrink = New System.Windows.Forms.Button()
        Me.btn_CreateAccIndexes = New System.Windows.Forms.Button()
        Me.btn_CreatePayIndexes = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.cmbDrivers = New System.Windows.Forms.ComboBox()
        Me.Btn_RebuildIndexesAcc = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btn_RebuildPayIndexes = New System.Windows.Forms.Button()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.btnDeleteAllDatabase = New System.Windows.Forms.Button()
        Me.btnLoadDatabases = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'chklstDatabases
        '
        Me.chklstDatabases.BackColor = System.Drawing.Color.Snow
        Me.chklstDatabases.CheckOnClick = True
        Me.chklstDatabases.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chklstDatabases.FormattingEnabled = True
        Me.chklstDatabases.Location = New System.Drawing.Point(12, 35)
        Me.chklstDatabases.Name = "chklstDatabases"
        Me.chklstDatabases.Size = New System.Drawing.Size(221, 395)
        Me.chklstDatabases.TabIndex = 0
        '
        'cmbServers
        '
        Me.cmbServers.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.cmbServers.FormattingEnabled = True
        Me.cmbServers.Location = New System.Drawing.Point(243, 6)
        Me.cmbServers.Name = "cmbServers"
        Me.cmbServers.Size = New System.Drawing.Size(186, 22)
        Me.cmbServers.TabIndex = 2
        Me.cmbServers.Text = "Select Server"
        '
        'BtnBak
        '
        Me.BtnBak.BackColor = System.Drawing.Color.SteelBlue
        Me.BtnBak.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnBak.FlatAppearance.BorderSize = 0
        Me.BtnBak.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon
        Me.BtnBak.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnBak.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.BtnBak.ForeColor = System.Drawing.Color.White
        Me.BtnBak.Image = CType(resources.GetObject("BtnBak.Image"), System.Drawing.Image)
        Me.BtnBak.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnBak.Location = New System.Drawing.Point(244, 73)
        Me.BtnBak.Name = "BtnBak"
        Me.BtnBak.Size = New System.Drawing.Size(185, 40)
        Me.BtnBak.TabIndex = 3
        Me.BtnBak.Text = "Backup DataBases"
        Me.BtnBak.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnBak.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.BtnBak.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.Enabled = False
        Me.Button1.Location = New System.Drawing.Point(49, 512)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(206, 10)
        Me.Button1.TabIndex = 3
        Me.Button1.TabStop = False
        Me.Button1.Tag = "Restore"
        Me.Button1.Text = "Restore"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btn_SelectAll
        '
        Me.btn_SelectAll.BackColor = System.Drawing.Color.SteelBlue
        Me.btn_SelectAll.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_SelectAll.FlatAppearance.BorderSize = 0
        Me.btn_SelectAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon
        Me.btn_SelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_SelectAll.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btn_SelectAll.ForeColor = System.Drawing.Color.White
        Me.btn_SelectAll.Location = New System.Drawing.Point(12, 5)
        Me.btn_SelectAll.Name = "btn_SelectAll"
        Me.btn_SelectAll.Size = New System.Drawing.Size(91, 23)
        Me.btn_SelectAll.TabIndex = 5
        Me.btn_SelectAll.Text = "SelectAll"
        Me.btn_SelectAll.UseVisualStyleBackColor = False
        '
        'btn_Shrink
        '
        Me.btn_Shrink.BackColor = System.Drawing.Color.SteelBlue
        Me.btn_Shrink.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_Shrink.FlatAppearance.BorderSize = 0
        Me.btn_Shrink.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon
        Me.btn_Shrink.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_Shrink.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btn_Shrink.ForeColor = System.Drawing.Color.White
        Me.btn_Shrink.Image = CType(resources.GetObject("btn_Shrink.Image"), System.Drawing.Image)
        Me.btn_Shrink.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_Shrink.Location = New System.Drawing.Point(244, 126)
        Me.btn_Shrink.Name = "btn_Shrink"
        Me.btn_Shrink.Size = New System.Drawing.Size(185, 40)
        Me.btn_Shrink.TabIndex = 7
        Me.btn_Shrink.Text = "Shrink DataBases"
        Me.btn_Shrink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_Shrink.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.btn_Shrink.UseVisualStyleBackColor = False
        '
        'btn_CreateAccIndexes
        '
        Me.btn_CreateAccIndexes.BackColor = System.Drawing.Color.SteelBlue
        Me.btn_CreateAccIndexes.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_CreateAccIndexes.FlatAppearance.BorderSize = 0
        Me.btn_CreateAccIndexes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon
        Me.btn_CreateAccIndexes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_CreateAccIndexes.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btn_CreateAccIndexes.ForeColor = System.Drawing.Color.White
        Me.btn_CreateAccIndexes.Image = CType(resources.GetObject("btn_CreateAccIndexes.Image"), System.Drawing.Image)
        Me.btn_CreateAccIndexes.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CreateAccIndexes.Location = New System.Drawing.Point(244, 179)
        Me.btn_CreateAccIndexes.Name = "btn_CreateAccIndexes"
        Me.btn_CreateAccIndexes.Size = New System.Drawing.Size(185, 40)
        Me.btn_CreateAccIndexes.TabIndex = 7
        Me.btn_CreateAccIndexes.Text = "Create Indexes Acc"
        Me.btn_CreateAccIndexes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CreateAccIndexes.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.btn_CreateAccIndexes.UseVisualStyleBackColor = False
        '
        'btn_CreatePayIndexes
        '
        Me.btn_CreatePayIndexes.BackColor = System.Drawing.Color.SteelBlue
        Me.btn_CreatePayIndexes.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_CreatePayIndexes.FlatAppearance.BorderSize = 0
        Me.btn_CreatePayIndexes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon
        Me.btn_CreatePayIndexes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_CreatePayIndexes.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btn_CreatePayIndexes.ForeColor = System.Drawing.Color.White
        Me.btn_CreatePayIndexes.Image = CType(resources.GetObject("btn_CreatePayIndexes.Image"), System.Drawing.Image)
        Me.btn_CreatePayIndexes.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_CreatePayIndexes.Location = New System.Drawing.Point(244, 285)
        Me.btn_CreatePayIndexes.Name = "btn_CreatePayIndexes"
        Me.btn_CreatePayIndexes.Size = New System.Drawing.Size(185, 40)
        Me.btn_CreatePayIndexes.TabIndex = 7
        Me.btn_CreatePayIndexes.Text = "Create Indexes Pay"
        Me.btn_CreatePayIndexes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_CreatePayIndexes.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.btn_CreatePayIndexes.UseVisualStyleBackColor = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.BackColor = System.Drawing.Color.Snow
        Me.ProgressBar1.ForeColor = System.Drawing.Color.LightCoral
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 443)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(417, 32)
        Me.ProgressBar1.Step = 5
        Me.ProgressBar1.TabIndex = 8
        '
        'cmbDrivers
        '
        Me.cmbDrivers.Font = New System.Drawing.Font("Tahoma", 11.0!)
        Me.cmbDrivers.Location = New System.Drawing.Point(355, 35)
        Me.cmbDrivers.Name = "cmbDrivers"
        Me.cmbDrivers.Size = New System.Drawing.Size(74, 26)
        Me.cmbDrivers.TabIndex = 9
        '
        'Btn_RebuildIndexesAcc
        '
        Me.Btn_RebuildIndexesAcc.BackColor = System.Drawing.Color.SteelBlue
        Me.Btn_RebuildIndexesAcc.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Btn_RebuildIndexesAcc.FlatAppearance.BorderSize = 0
        Me.Btn_RebuildIndexesAcc.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon
        Me.Btn_RebuildIndexesAcc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Btn_RebuildIndexesAcc.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Btn_RebuildIndexesAcc.ForeColor = System.Drawing.Color.White
        Me.Btn_RebuildIndexesAcc.Image = CType(resources.GetObject("Btn_RebuildIndexesAcc.Image"), System.Drawing.Image)
        Me.Btn_RebuildIndexesAcc.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Btn_RebuildIndexesAcc.Location = New System.Drawing.Point(244, 232)
        Me.Btn_RebuildIndexesAcc.Name = "Btn_RebuildIndexesAcc"
        Me.Btn_RebuildIndexesAcc.Size = New System.Drawing.Size(185, 40)
        Me.Btn_RebuildIndexesAcc.TabIndex = 10
        Me.Btn_RebuildIndexesAcc.Text = "Rebuild Indexes    Update Statistics Acc"
        Me.Btn_RebuildIndexesAcc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Btn_RebuildIndexesAcc.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.Btn_RebuildIndexesAcc.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.IndianRed
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(243, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(113, 25)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Backup Drive"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btn_RebuildPayIndexes
        '
        Me.btn_RebuildPayIndexes.BackColor = System.Drawing.Color.SteelBlue
        Me.btn_RebuildPayIndexes.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_RebuildPayIndexes.FlatAppearance.BorderSize = 0
        Me.btn_RebuildPayIndexes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon
        Me.btn_RebuildPayIndexes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_RebuildPayIndexes.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btn_RebuildPayIndexes.ForeColor = System.Drawing.Color.White
        Me.btn_RebuildPayIndexes.Image = CType(resources.GetObject("btn_RebuildPayIndexes.Image"), System.Drawing.Image)
        Me.btn_RebuildPayIndexes.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_RebuildPayIndexes.Location = New System.Drawing.Point(244, 338)
        Me.btn_RebuildPayIndexes.Name = "btn_RebuildPayIndexes"
        Me.btn_RebuildPayIndexes.Size = New System.Drawing.Size(185, 40)
        Me.btn_RebuildPayIndexes.TabIndex = 13
        Me.btn_RebuildPayIndexes.Text = "Rebuild Indexes     Update Statistics Pay"
        Me.btn_RebuildPayIndexes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_RebuildPayIndexes.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.btn_RebuildPayIndexes.UseVisualStyleBackColor = False
        '
        'btnDeleteAllDatabase
        '
        Me.btnDeleteAllDatabase.BackColor = System.Drawing.Color.SteelBlue
        Me.btnDeleteAllDatabase.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDeleteAllDatabase.Enabled = False
        Me.btnDeleteAllDatabase.FlatAppearance.BorderSize = 0
        Me.btnDeleteAllDatabase.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon
        Me.btnDeleteAllDatabase.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDeleteAllDatabase.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnDeleteAllDatabase.ForeColor = System.Drawing.Color.White
        Me.btnDeleteAllDatabase.Image = Global.MaintainceTool.My.Resources.Resources.delete__1_
        Me.btnDeleteAllDatabase.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDeleteAllDatabase.Location = New System.Drawing.Point(244, 390)
        Me.btnDeleteAllDatabase.Name = "btnDeleteAllDatabase"
        Me.btnDeleteAllDatabase.Size = New System.Drawing.Size(185, 40)
        Me.btnDeleteAllDatabase.TabIndex = 13
        Me.btnDeleteAllDatabase.Text = "Delete Databases"
        Me.btnDeleteAllDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDeleteAllDatabase.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.btnDeleteAllDatabase.UseVisualStyleBackColor = False
        '
        'btnLoadDatabases
        '
        Me.btnLoadDatabases.BackColor = System.Drawing.Color.SteelBlue
        Me.btnLoadDatabases.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoadDatabases.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnLoadDatabases.ForeColor = System.Drawing.SystemColors.Control
        Me.btnLoadDatabases.Location = New System.Drawing.Point(123, 4)
        Me.btnLoadDatabases.Name = "btnLoadDatabases"
        Me.btnLoadDatabases.Size = New System.Drawing.Size(110, 26)
        Me.btnLoadDatabases.TabIndex = 14
        Me.btnLoadDatabases.Text = "Load Databases"
        Me.btnLoadDatabases.UseVisualStyleBackColor = False
        '
        'frm_Backup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(436, 480)
        Me.Controls.Add(Me.btnLoadDatabases)
        Me.Controls.Add(Me.btnDeleteAllDatabase)
        Me.Controls.Add(Me.btn_RebuildPayIndexes)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Btn_RebuildIndexesAcc)
        Me.Controls.Add(Me.cmbDrivers)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.btn_CreatePayIndexes)
        Me.Controls.Add(Me.btn_CreateAccIndexes)
        Me.Controls.Add(Me.btn_Shrink)
        Me.Controls.Add(Me.btn_SelectAll)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.BtnBak)
        Me.Controls.Add(Me.cmbServers)
        Me.Controls.Add(Me.chklstDatabases)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frm_Backup"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Maintaince Tool"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents chklstDatabases As CheckedListBox
    Friend WithEvents cmbServers As ComboBox
    Friend WithEvents BtnBak As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents btn_SelectAll As Button
    Friend WithEvents btn_Shrink As Button
    Friend WithEvents btn_CreateAccIndexes As Button
    Friend WithEvents btn_CreatePayIndexes As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents cmbDrivers As ComboBox
    Friend WithEvents Btn_RebuildIndexesAcc As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents btn_RebuildPayIndexes As Button
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents btnDeleteAllDatabase As Button
    Friend WithEvents btnLoadDatabases As Button
End Class
