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
        PDF2Images.Program.Main()

        OpenFileDialog1.ShowDialog()
        imgPath_TextBox.Text = OpenFileDialog1.FileName
        PictureBox1.ImageLocation = imgPath_TextBox.Text

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles imgPath_TextBox.TextChanged

    End Sub
End Class

Namespace PDF2Images
    Class Program
        Shared outputFolder As String = "D:\Users\Lemuel\Documents\GitHub\Multiple-Choice-OMR\Visual-Basic\outputFolder\"

        Public Shared Sub GetPdfNumberOfPages(ByVal PdfPath As String, ByRef nPages As Integer)
            Dim pdf As PdfReader = New PdfReader(PdfPath)
            nPages = pdf.NumberOfPages
        End Sub

        Public Shared Sub Main()
            Dim pdfFiles = Directory.GetFiles("D:\Users\Lemuel\Documents\GitHub\Multiple-Choice-OMR\Visual-Basic\outputFolder\", "*.pdf")
            For Each pdfFile In pdfFiles
                Dim fileName = Path.GetFileNameWithoutExtension(pdfFile)
                PdfToPng(pdfFile, fileName)
            Next
        End Sub

        Private Shared Sub PdfToPng(ByVal inputFile As String, ByVal outputFileName As String)
            Dim xDpi = 100
            Dim yDpi = 100
            Dim nPages As Integer
            GetPdfNumberOfPages(inputFile, nPages)
            MessageBox.Show(nPages)

            Using rasterizer = New GhostscriptRasterizer()
                rasterizer.Open(inputFile)
                For index As Integer = 1 To nPages
                    Dim currentPageFileName As String = outputFileName + "-" + CStr(index)
                    Dim outputPNGPath = Path.Combine(outputFolder, String.Format("{0}.png", currentPageFileName))
                    Dim pdf2PNG = rasterizer.GetPage(xDpi, yDpi, nPages)
                    pdf2PNG.Save(outputPNGPath, ImageFormat.Png)
                Next

            End Using
        End Sub
    End Class
End Namespace

