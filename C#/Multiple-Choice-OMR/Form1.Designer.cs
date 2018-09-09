namespace Multiple_Choice_OMR
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ImageBox_Main = new Emgu.CV.UI.ImageBox();
            this.Button_ChoosePDF = new System.Windows.Forms.Button();
            this.TextBox_FilePath = new System.Windows.Forms.TextBox();
            this.PrevButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.Button_PreProcess = new System.Windows.Forms.Button();
            this.Button_ContourDetection = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Label_PageNumber = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // ImageBox_Main
            // 
            this.ImageBox_Main.Location = new System.Drawing.Point(13, 13);
            this.ImageBox_Main.Name = "ImageBox_Main";
            this.ImageBox_Main.Size = new System.Drawing.Size(351, 471);
            this.ImageBox_Main.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ImageBox_Main.TabIndex = 2;
            this.ImageBox_Main.TabStop = false;
            // 
            // Button_ChoosePDF
            // 
            this.Button_ChoosePDF.Location = new System.Drawing.Point(370, 193);
            this.Button_ChoosePDF.Name = "Button_ChoosePDF";
            this.Button_ChoosePDF.Size = new System.Drawing.Size(124, 49);
            this.Button_ChoosePDF.TabIndex = 3;
            this.Button_ChoosePDF.Text = "Choose PDF";
            this.Button_ChoosePDF.UseVisualStyleBackColor = true;
            this.Button_ChoosePDF.Click += new System.EventHandler(this.Button_ChoosePDF_Click_1);
            // 
            // TextBox_FilePath
            // 
            this.TextBox_FilePath.Location = new System.Drawing.Point(370, 12);
            this.TextBox_FilePath.Multiline = true;
            this.TextBox_FilePath.Name = "TextBox_FilePath";
            this.TextBox_FilePath.Size = new System.Drawing.Size(561, 175);
            this.TextBox_FilePath.TabIndex = 4;
            // 
            // PrevButton
            // 
            this.PrevButton.Location = new System.Drawing.Point(500, 193);
            this.PrevButton.Name = "PrevButton";
            this.PrevButton.Size = new System.Drawing.Size(64, 49);
            this.PrevButton.TabIndex = 5;
            this.PrevButton.Text = "Previous";
            this.PrevButton.UseVisualStyleBackColor = true;
            this.PrevButton.Click += new System.EventHandler(this.PrevButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(570, 193);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(64, 49);
            this.NextButton.TabIndex = 6;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // Button_PreProcess
            // 
            this.Button_PreProcess.Location = new System.Drawing.Point(370, 336);
            this.Button_PreProcess.Name = "Button_PreProcess";
            this.Button_PreProcess.Size = new System.Drawing.Size(194, 45);
            this.Button_PreProcess.TabIndex = 7;
            this.Button_PreProcess.Text = "Preprocess";
            this.Button_PreProcess.UseVisualStyleBackColor = true;
            this.Button_PreProcess.Click += new System.EventHandler(this.Button_PreProcess_Click_1);
            // 
            // Button_ContourDetection
            // 
            this.Button_ContourDetection.Location = new System.Drawing.Point(370, 387);
            this.Button_ContourDetection.Name = "Button_ContourDetection";
            this.Button_ContourDetection.Size = new System.Drawing.Size(194, 45);
            this.Button_ContourDetection.TabIndex = 8;
            this.Button_ContourDetection.Text = "Find Contours";
            this.Button_ContourDetection.UseVisualStyleBackColor = true;
            this.Button_ContourDetection.Click += new System.EventHandler(this.Button_ContourDetection_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Label_PageNumber
            // 
            this.Label_PageNumber.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Label_PageNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_PageNumber.Location = new System.Drawing.Point(640, 193);
            this.Label_PageNumber.Name = "Label_PageNumber";
            this.Label_PageNumber.Size = new System.Drawing.Size(52, 49);
            this.Label_PageNumber.TabIndex = 9;
            this.Label_PageNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_PageNumber.UseMnemonic = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 496);
            this.Controls.Add(this.Label_PageNumber);
            this.Controls.Add(this.Button_ContourDetection);
            this.Controls.Add(this.Button_PreProcess);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.PrevButton);
            this.Controls.Add(this.TextBox_FilePath);
            this.Controls.Add(this.Button_ChoosePDF);
            this.Controls.Add(this.ImageBox_Main);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox_Main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox ImageBox_Main;
        private System.Windows.Forms.Button Button_ChoosePDF;
        private System.Windows.Forms.TextBox TextBox_FilePath;
        private System.Windows.Forms.Button PrevButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button Button_PreProcess;
        private System.Windows.Forms.Button Button_ContourDetection;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label Label_PageNumber;
    }
}

