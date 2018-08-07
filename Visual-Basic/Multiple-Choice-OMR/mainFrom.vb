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
    Shared nImages As Integer = 0
    Shared ImageCounter As Integer = 0
    Shared ImagesReadCV As New List(Of Mat)()
    Dim ImagesPaths As New List(Of String)()
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button_ChoosePDF_Click(sender As Object, e As EventArgs) Handles Button_ChoosePDF.Click
        TextBox_FilePath.Text = ""
        ImagesPaths.Clear()
        nImages = 0
        ImageCounter = 0
        Dim result As DialogResult = OpenFileDialog1.ShowDialog()
        If result = Windows.Forms.DialogResult.OK Then
            PDF2Images.Program.PdfToPng(OpenFileDialog1.FileName, ImagesPaths)

            For Each path As String In ImagesPaths
                TextBox_FilePath.AppendText(path & Environment.NewLine)
            Next
            nImages = ImagesPaths.Count()

            ImagesReadCV.Clear()
            For index As Integer = 0 To nImages - 1
                ImagesReadCV.Add(CvInvoke.Imread(ImagesPaths(index), 0))
                If ImagesReadCV(index).IsEmpty = True Then
                    MessageBox.Show("One or more files does not exist!")
                    Return
                End If
            Next

            ImageBox_Main.Image = ImagesReadCV(4)
            Label_PageNumber.Text = "1"
        Else
            MessageBox.Show("No File Selected.")
        End If
    End Sub

    Private Sub PrevButton_Click(sender As Object, e As EventArgs) Handles PrevButton.Click
        If ImageCounter < 1 Then
            Return
        Else
            ImageCounter = ImageCounter - 1
        End If
        ImageBox_Main.Image = ImagesReadCV(ImageCounter)
        Label_PageNumber.Text = CStr(ImageCounter + 1)
    End Sub

    Private Sub NextButton_Click(sender As Object, e As EventArgs) Handles NextButton.Click
        If ImageCounter > ImagesPaths.Count - 2 Then
            Return
        Else
            ImageCounter = ImageCounter + 1
        End If
        ImageBox_Main.Image = ImagesReadCV(ImageCounter)
        Label_PageNumber.Text = CStr(ImageCounter + 1)
    End Sub

    Shared ImageDonePreProcess As New List(Of Mat)()

    Private Sub PreProcessImage()
        If ImagesPaths.Count < 1 Then
            Return
        End If
        If ImagesReadCV.Count >= ImagesPaths.Count Then
            MessageBox.Show("Action Ignored!")
            Return
        End If


    End Sub

    Private Sub Button_PreProcess_Click(sender As Object, e As EventArgs) Handles Button_PreProcess.Click
        PreProcessImage()
        'PictureBox1.Image = ImageDonePreProcess(1).ToBitmap

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

