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
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
        imgPath_TextBox.Text = OpenFileDialog1.FileName

        PDF2Images.Program.PdfToPng(imgPath_TextBox.Text)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles imgPath_TextBox.TextChanged

    End Sub
End Class

Namespace PDF2Images
    Class Program
        Public Shared Sub GetPdfNumberOfPages(ByVal PdfPath As String, ByRef nPages As Integer)
            Dim pdf As PdfReader = New PdfReader(PdfPath)
            nPages = pdf.NumberOfPages
        End Sub

        Public Shared Sub PdfToPng(ByVal inputFilePath As String)
            Dim outputDirectory As String
            outputDirectory = Path.GetDirectoryName(inputFilePath)
            Dim inputFileName As String
            inputFileName = Path.GetFileName(inputFilePath)
            Dim xDpi = 100
            Dim yDpi = 100
            Dim nPages As Integer
            GetPdfNumberOfPages(inputFilePath, nPages)

            Using rasterizer = New GhostscriptRasterizer()
                rasterizer.Open(inputFilePath)
                For index As Integer = 1 To nPages
                    Dim currentPageFileName As String = inputFileName + "-" + CStr(index)
                    Dim outputPNGPath = Path.Combine(outputDirectory, String.Format("{0}.png", currentPageFileName))
                    Dim pdf2PNG = rasterizer.GetPage(xDpi, yDpi, index)
                    pdf2PNG.Save(outputPNGPath, ImageFormat.Png)
                Next
            End Using
        End Sub
    End Class
End Namespace

