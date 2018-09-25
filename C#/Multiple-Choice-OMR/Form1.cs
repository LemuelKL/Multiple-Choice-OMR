using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Ghostscript.NET.Rasterizer;
using System.Windows.Forms;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using iTextSharp;
using iTextSharp.text.pdf;

namespace Multiple_Choice_OMR
{
    public partial class Form1 : Form
    {
        private static int nImages = 0;
        private static int ImageCounter = 0;
        private static List<Mat> ImagesReadCV = new List<Mat>();
        private List<string> ImagesPaths = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            if (ImageCounter < 1)
                return;
            else
                ImageCounter = ImageCounter - 1;
            ImageBox_Main.Image = ImagesReadCV[ImageCounter];
            Label_PageNumber.Text = System.Convert.ToString(ImageCounter + 1);
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (ImageCounter > ImagesPaths.Count - 2)
                return;
            else
                ImageCounter = ImageCounter + 1;
            ImageBox_Main.Image = ImagesReadCV[ImageCounter];
            Label_PageNumber.Text = System.Convert.ToString(ImageCounter + 1);
        }

        private static List<Mat> ImageDonePreProcess = new List<Mat>();

        private void PreProcessImage()
        {
            if (ImagesPaths.Count < 1)
            {
                MessageBox.Show("No Image To Process!");
                return;
            }
            for (int index = 0; index <= nImages - 1; index++)
            {
                Mat GrayImage = ImagesReadCV[index].Clone();
                Mat BlurredImage = GrayImage.Clone();
                Mat AdaptThresh = BlurredImage.Clone();

                if (GrayImage.NumberOfChannels != 3)
                {
                    MessageBox.Show(@"Invalid Number of Channels within the Image!\nExpect 3 but got" + GrayImage.NumberOfChannels);
                    return;
                }
                CvInvoke.CvtColor(ImagesReadCV[index], GrayImage, ColorConversion.Bgr2Gray);
                CvInvoke.GaussianBlur(GrayImage, BlurredImage, new Size(3, 3), 1);
                CvInvoke.AdaptiveThreshold(BlurredImage, AdaptThresh, 255, AdaptiveThresholdType.GaussianC, ThresholdType.BinaryInv, 11, 2);
                ImagesReadCV[index] = AdaptThresh;
            }
            MessageBox.Show("Done!");
            ReloadImageBox();
        }

        private void Button_PreProcess_Click_1(object sender, EventArgs e)
        {
            PreProcessImage();
            ImageBox_Main.Image = ImagesReadCV[ImageCounter];
        }

        private List<Emgu.CV.Util.VectorOfVectorOfPoint> ContoursInAllImages = new List<Emgu.CV.Util.VectorOfVectorOfPoint>();
        private List<Mat> HierarchyInAllImages = new List<Mat>(); 

        private void Button_ContourDetection_Click(object sender, EventArgs e)
        {
            FindCricleContours();
        }

        private void ReloadImageBox()
        {
            ImageBox_Main.Image = ImagesReadCV[ImageCounter];
        }

        private void Button_ChoosePDF_Click_1(object sender, EventArgs e)
        {
            TextBox_FilePath.Text = "";
            ImagesPaths.Clear();
            ImagesReadCV.Clear();
            nImages = 0;
            ImageCounter = 0;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                PDF2Images.Program.PdfToPng(openFileDialog1.FileName, ref ImagesPaths);

                foreach (string path in ImagesPaths)
                    TextBox_FilePath.AppendText(path + Environment.NewLine);
                nImages = ImagesPaths.Count();

                ImagesReadCV.Clear();
                for (int index = 0; index <= nImages - 1; index++)
                {
                    ImagesReadCV.Add(CvInvoke.Imread(ImagesPaths[index]));
                    if (ImagesReadCV[index].IsEmpty == true)
                    {
                        MessageBox.Show("One or more files does not exist!");
                        return;
                    }
                }

                ImageBox_Main.Image = ImagesReadCV[0];
                Label_PageNumber.Text = "1";
            }
            else
                MessageBox.Show("No File Selected.");
        }

        private void FindCricleContours()
        {
            for (int index = 0; index <= nImages - 1; index++)
            {
                Emgu.CV.Util.VectorOfVectorOfPoint ContourInThisImage = new Emgu.CV.Util.VectorOfVectorOfPoint();
                Mat HierarchyInThisImage = new Mat();

                CvInvoke.FindContours(ImagesReadCV[index], ContourInThisImage, HierarchyInThisImage, Emgu.CV.CvEnum.RetrType.Ccomp, ChainApproxMethod.ChainApproxNone);
                ContoursInAllImages.Add(ContourInThisImage);
                HierarchyInAllImages.Add(HierarchyInThisImage);
                if (ContourInThisImage == null)
                    MessageBox.Show("!!");
                CvInvoke.CvtColor(ImagesReadCV[index], ImagesReadCV[index], ColorConversion.Gray2Bgr, 3);
                CvInvoke.DrawContours(ImagesReadCV[index], ContourInThisImage, -1, new MCvScalar(0, 255, 0, 255), 1);
                
            }
            MessageBox.Show("Done!");
            ReloadImageBox();
        }

    }

    namespace PDF2Images
    {
        class Program
        {
            public static void PdfToPng(string inputFilePath, ref List<string> retFilePaths)
            {
                string outputDirectory = Path.GetDirectoryName(inputFilePath);
                string inputFileName = Path.GetFileNameWithoutExtension(inputFilePath);
                var xDpi = 300;
                var yDpi = 300;
                PdfReader pdf = new PdfReader(inputFilePath);
                int nPages = pdf.NumberOfPages;

                using (var rasterizer = new GhostscriptRasterizer())
                {
                    rasterizer.Open(inputFilePath);
                    for (int index = 1; index <= nPages; index++)
                    {
                        string currentPageFileName = inputFileName + "-" + System.Convert.ToString(index);
                        var outputPNGPath = Path.Combine(outputDirectory, string.Format("{0}.png", currentPageFileName));
                        var pdf2PNG = rasterizer.GetPage(xDpi, yDpi, index);
                        pdf2PNG.Save(outputPNGPath, ImageFormat.Png);
                        retFilePaths.Add(outputPNGPath);
                    }
                }
            }
        }
    }
}
