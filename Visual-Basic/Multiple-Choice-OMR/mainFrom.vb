Imports Ghostscript.NET.Rasterizer
Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports Emgu.CV
Imports Emgu.CV.UI
Imports Emgu.Util
Imports Emgu.CV.Structure
Imports Emgu.CV.CvEnum
Imports iTextSharp
Imports iTextSharp.text.pdf

Public Class MainForm
    Shared ImageCounter As Integer = 0
    Dim ImagesPaths As New List(Of String)()
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
        imgPath_TextBox.Text = OpenFileDialog1.FileName
        Dim retFilePaths As New List(Of String)()
        PDF2Images.Program.PdfToPng(imgPath_TextBox.Text, retFilePaths)
        ImagesPaths = retFilePaths
        PictureBox1.ImageLocation = ImagesPaths(0)
        Label_PageNumber.Text = "1"
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles imgPath_TextBox.TextChanged

    End Sub

    Private Sub PrevButton_Click(sender As Object, e As EventArgs) Handles PrevButton.Click
        If ImageCounter < 1 Then
        Else
            ImageCounter = ImageCounter - 1
        End If
        PictureBox1.ImageLocation = ImagesPaths(ImageCounter)
        Label_PageNumber.Text = CStr(ImageCounter + 1)
    End Sub

    Private Sub NextButton_Click(sender As Object, e As EventArgs) Handles NextButton.Click
        If ImageCounter > ImagesPaths.Count - 2 Then
        Else
            ImageCounter = ImageCounter + 1
        End If
        PictureBox1.ImageLocation = ImagesPaths(ImageCounter)
        Label_PageNumber.Text = CStr(ImageCounter + 1)
    End Sub
End Class

Namespace PDF2Images
    Class Program
        Public Shared Sub PdfToPng(ByVal inputFilePath As String, ByRef retFilePaths As List(Of String))
            Dim outputDirectory As String = Path.GetDirectoryName(inputFilePath)
            Dim inputFileName As String = Path.GetFileNameWithoutExtension(inputFilePath)
            Dim xDpi = 300
            Dim yDpi = 300
            Dim pdf As PdfReader = New PdfReader(inputFilePath)
            Dim nPages As Integer = pdf.NumberOfPages

            Using rasterizer = New GhostscriptRasterizer()
                rasterizer.Open(inputFilePath)
                For index As Integer = 1 To nPages
                    Dim currentPageFileName As String = inputFileName + "-" + CStr(index)
                    Dim outputPNGPath = Path.Combine(outputDirectory, String.Format("{0}.png", currentPageFileName))
                    Dim pdf2PNG = rasterizer.GetPage(xDpi, yDpi, index)
                    pdf2PNG.Save(outputPNGPath, ImageFormat.Png)
                    retFilePaths.Add(outputPNGPath)
                Next
            End Using
        End Sub
    End Class
End Namespace