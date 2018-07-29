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

        Public Shared Sub Main()
            Dim pdfFiles = Directory.GetFiles("D:\Users\Lemuel\Documents\GitHub\Multiple-Choice-OMR\Visual-Basic\outputFolder\", "*.pdf")

            For Each pdfFile In pdfFiles
                Dim fileName = Path.GetFileNameWithoutExtension(pdfFile)
                PdfToPng(pdfFile, fileName)
            Next

            'Console.ReadKey()
        End Sub

        Private Shared Sub PdfToPng(ByVal inputFile As String, ByVal outputFileName As String)
            Dim xDpi = 100
            Dim yDpi = 100
            Dim pageNumber = 1

            Using rasterizer = New GhostscriptRasterizer()
                rasterizer.Open(inputFile)
                Dim outputPNGPath = Path.Combine(outputFolder, String.Format("{0}.png", outputFileName))
                Dim pdf2PNG = rasterizer.GetPage(xDpi, yDpi, pageNumber)
                pdf2PNG.Save(outputPNGPath, ImageFormat.Png)
            End Using
        End Sub
    End Class
End Namespace