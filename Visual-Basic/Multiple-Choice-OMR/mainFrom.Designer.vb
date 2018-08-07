<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
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
        Me.components = New System.ComponentModel.Container()
        Me.Button_ChoosePDF = New System.Windows.Forms.Button()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me.PrevButton = New System.Windows.Forms.Button()
        Me.NextButton = New System.Windows.Forms.Button()
        Me.Label_PageNumber = New System.Windows.Forms.Label()
        Me.TextBox_FilePath = New System.Windows.Forms.TextBox()
        Me.Button_PreProcess = New System.Windows.Forms.Button()
        Me.ImageBox_Main = New Emgu.CV.UI.ImageBox()
        CType(Me.ImageBox_Main, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_ChoosePDF
        '
        Me.Button_ChoosePDF.Location = New System.Drawing.Point(471, 12)
        Me.Button_ChoosePDF.Name = "Button_ChoosePDF"
        Me.Button_ChoosePDF.Size = New System.Drawing.Size(116, 38)
        Me.Button_ChoosePDF.TabIndex = 0
        Me.Button_ChoosePDF.Text = "Choose PDF"
        Me.Button_ChoosePDF.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'ImageList2
        '
        Me.ImageList2.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList2.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList2.TransparentColor = System.Drawing.Color.Transparent
        '
        'PrevButton
        '
        Me.PrevButton.Location = New System.Drawing.Point(471, 56)
        Me.PrevButton.Name = "PrevButton"
        Me.PrevButton.Size = New System.Drawing.Size(55, 49)
        Me.PrevButton.TabIndex = 3
        Me.PrevButton.Text = "Prev"
        Me.PrevButton.UseVisualStyleBackColor = True
        '
        'NextButton
        '
        Me.NextButton.Location = New System.Drawing.Point(532, 56)
        Me.NextButton.Name = "NextButton"
        Me.NextButton.Size = New System.Drawing.Size(55, 49)
        Me.NextButton.TabIndex = 4
        Me.NextButton.Text = "Next"
        Me.NextButton.UseVisualStyleBackColor = True
        '
        'Label_PageNumber
        '
        Me.Label_PageNumber.AutoSize = True
        Me.Label_PageNumber.Location = New System.Drawing.Point(593, 74)
        Me.Label_PageNumber.Name = "Label_PageNumber"
        Me.Label_PageNumber.Size = New System.Drawing.Size(27, 13)
        Me.Label_PageNumber.TabIndex = 5
        Me.Label_PageNumber.Text = "N/A"
        '
        'TextBox_FilePath
        '
        Me.TextBox_FilePath.Location = New System.Drawing.Point(471, 111)
        Me.TextBox_FilePath.Multiline = True
        Me.TextBox_FilePath.Name = "TextBox_FilePath"
        Me.TextBox_FilePath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox_FilePath.Size = New System.Drawing.Size(501, 162)
        Me.TextBox_FilePath.TabIndex = 6
        '
        'Button_PreProcess
        '
        Me.Button_PreProcess.Location = New System.Drawing.Point(471, 279)
        Me.Button_PreProcess.Name = "Button_PreProcess"
        Me.Button_PreProcess.Size = New System.Drawing.Size(116, 52)
        Me.Button_PreProcess.TabIndex = 7
        Me.Button_PreProcess.Text = "Start Pre-Processing"
        Me.Button_PreProcess.UseVisualStyleBackColor = True
        '
        'ImageBox_Main
        '
        Me.ImageBox_Main.Location = New System.Drawing.Point(12, 12)
        Me.ImageBox_Main.Name = "ImageBox_Main"
        Me.ImageBox_Main.Size = New System.Drawing.Size(453, 637)
        Me.ImageBox_Main.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.ImageBox_Main.TabIndex = 2
        Me.ImageBox_Main.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 661)
        Me.Controls.Add(Me.ImageBox_Main)
        Me.Controls.Add(Me.Button_PreProcess)
        Me.Controls.Add(Me.TextBox_FilePath)
        Me.Controls.Add(Me.Label_PageNumber)
        Me.Controls.Add(Me.NextButton)
        Me.Controls.Add(Me.PrevButton)
        Me.Controls.Add(Me.Button_ChoosePDF)
        Me.Name = "MainForm"
        Me.Text = "Multiple-Choice-OMR"
        CType(Me.ImageBox_Main, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button_ChoosePDF As Button
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents ImageList2 As ImageList
    Friend WithEvents PrevButton As Button
    Friend WithEvents NextButton As Button
    Friend WithEvents Label_PageNumber As Label
    Friend WithEvents TextBox_FilePath As TextBox
    Friend WithEvents Button_PreProcess As Button
    Friend WithEvents ImageBox_Main As Emgu.CV.UI.ImageBox
End Class
