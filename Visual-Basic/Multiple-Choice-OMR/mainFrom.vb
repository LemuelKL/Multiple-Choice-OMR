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
'Imports OpenCV.Net

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
        ImagesReadCV.Clear()
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
                ImagesReadCV.Add(CvInvoke.Imread(ImagesPaths(index), 1))
                If ImagesReadCV(index).IsEmpty = True Then
                    MessageBox.Show("One or more files does not exist!")
                    Return
                End If
            Next

            ImageBox_Main.Image = ImagesReadCV(0)
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
            MessageBox.Show("No Image To Process!")
            Return
        End If
        For index As Integer = 0 To nImages - 1
            Dim GrayImage As Mat = ImagesReadCV(index).Clone()
            Dim BlurredImage As Mat = GrayImage.Clone()
            Dim AdaptThresh As Mat = BlurredImage.Clone()

            If GrayImage.NumberOfChannels <> 3 Then
                MessageBox.Show("Invalid Number of Channels within the Image!\nExpect 3 but got" & GrayImage.NumberOfChannels)
                Return
            End If
            CvInvoke.CvtColor(ImagesReadCV(index), GrayImage, ColorConversion.Bgr2Gray)
            CvInvoke.GaussianBlur(GrayImage, BlurredImage, New Drawing.Size(3, 3), 1)
            CvInvoke.AdaptiveThreshold(BlurredImage, AdaptThresh, 255, AdaptiveThresholdType.GaussianC, ThresholdType.BinaryInv, 11, 2)
            ImagesReadCV(index) = AdaptThresh
        Next
        MessageBox.Show("Done!")
        ReloadImageBox()
    End Sub

    Private Sub Button_PreProcess_Click(sender As Object, e As EventArgs) Handles Button_PreProcess.Click
        PreProcessImage()
        ImageBox_Main.Image = ImagesReadCV(ImageCounter)
    End Sub

    Dim ContoursInAllImages As New List(Of Emgu.CV.Util.VectorOfVectorOfPoint)
    Dim HierarchyInAllImages As New List(Of Mat)

    Private Sub FindCricleContours()
        For index As Integer = 0 To nImages - 1
            Dim ContourInThisImage As Emgu.CV.Util.VectorOfVectorOfPoint = New Emgu.CV.Util.VectorOfVectorOfPoint()
            Dim HierarchyInThisImage As New Mat()

            CvInvoke.FindContours(ImagesReadCV(index).Clone, ContourInThisImage, HierarchyInThisImage, 2, ChainApproxMethod.ChainApproxNone)
            ContoursInAllImages.Add(ContourInThisImage)
            HierarchyInAllImages.Add(HierarchyInThisImage)
            If ContourInThisImage Is Nothing Then
                MessageBox.Show("!!")
            End If
            CvInvoke.CvtColor(ImagesReadCV(index), ImagesReadCV(index), ColorConversion.Gray2Bgr, 3)
            CvInvoke.DrawContours(ImagesReadCV(index), ContourInThisImage, -1, New MCvScalar(0, 255, 0, 255), 1)

            Dim ret() As Integer = New Integer(-1) {}
            ret = GetHierarchy(HierarchyInThisImage, 0)
            Console.WriteLine(ret)

        Next
        MessageBox.Show("Done!")
        ReloadImageBox()
    End Sub

    Private Sub Button_ContourDetection_Click(sender As Object, e As EventArgs) Handles Button_ContourDetection.Click
        FindCricleContours()
    End Sub

    Private Sub ReloadImageBox()
        ImageBox_Main.Image = ImagesReadCV(ImageCounter)
    End Sub

    Public Function GetHierarchy(ByVal Hierarchy As Mat, ByVal contourIdx As Integer) As Integer()
        Dim ret() As Integer = New Integer(-1) {}
        If (Hierarchy.Depth <> Emgu.CV.CvEnum.DepthType.Cv32S) Then
            Throw New ArgumentOutOfRangeException("ContourData must have Cv32S hierarchy element type.")
        End If

        If (Hierarchy.Rows <> 1) Then
            Throw New ArgumentOutOfRangeException("ContourData must have one hierarchy hierarchy row.")
        End If

        If (Hierarchy.NumberOfChannels <> 4) Then
            Throw New ArgumentOutOfRangeException("ContourData must have four hierarchy channels.")
        End If

        If (Hierarchy.Dims <> 2) Then
            Throw New ArgumentOutOfRangeException("ContourData must have two dimensional hierarchy.")
        End If

        Dim elementStride As Long
        Dim offset0 = (CType(0, Long) _
                    + (contourIdx * elementStride))
        If ((0 <= offset0) _
                    AndAlso (offset0 _
                    < (Hierarchy.Total.ToInt64 * elementStride))) Then
            Dim offset1 = (CType(1, Long) _
                        + (contourIdx * elementStride))
            Dim offset2 = (CType(2, Long) _
                        + (contourIdx * elementStride))
            Dim offset3 = (CType(3, Long) _
                        + (contourIdx * elementStride))
            ret = New Integer((4) - 1) {}
            'return *((Int32*)Hierarchy.DataPointer.ToPointer() + offset);
            ret(0) = (CType(Hierarchy.DataPointer, Int32) + offset0)
            ret(1) = (CType(Hierarchy.DataPointer, Int32) + offset1)
            ret(2) = (CType(Hierarchy.DataPointer, Int32) + offset2)
            ret(3) = (CType(Hierarchy.DataPointer, Int32) + offset3)
            CType
        End If

        'else
        '{
        '    return new int[] { };
        '}
        Return ret
    End Function

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