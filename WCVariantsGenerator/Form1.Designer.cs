namespace WCVariantsGenerator
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
            this.dlgOpenCSV = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.brnOpenCSV = new System.Windows.Forms.Button();
            this.btnParentProds = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnVariationProds = new System.Windows.Forms.Button();
            this.btnSimpleProds = new System.Windows.Forms.Button();
            this.btnMissingSKUs = new System.Windows.Forms.Button();
            this.btnImageCSVOpen = new System.Windows.Forms.Button();
            this.txtImageSKUFileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGenerateImgCSV = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSimpleFileOpen = new System.Windows.Forms.Button();
            this.txtSimpleProdFile = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnVariantsFileOpen = new System.Windows.Forms.Button();
            this.txtVariantsFile = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dlgOpenCSV
            // 
            this.dlgOpenCSV.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "CSV File";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(80, 13);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(381, 20);
            this.txtFileName.TabIndex = 1;
            // 
            // brnOpenCSV
            // 
            this.brnOpenCSV.Location = new System.Drawing.Point(465, 12);
            this.brnOpenCSV.Name = "brnOpenCSV";
            this.brnOpenCSV.Size = new System.Drawing.Size(75, 23);
            this.brnOpenCSV.TabIndex = 2;
            this.brnOpenCSV.Text = "Open";
            this.brnOpenCSV.UseVisualStyleBackColor = true;
            this.brnOpenCSV.Click += new System.EventHandler(this.brnOpenCSV_Click);
            // 
            // btnParentProds
            // 
            this.btnParentProds.Location = new System.Drawing.Point(80, 39);
            this.btnParentProds.Name = "btnParentProds";
            this.btnParentProds.Size = new System.Drawing.Size(75, 47);
            this.btnParentProds.TabIndex = 3;
            this.btnParentProds.Text = "Generate Parent Prods";
            this.btnParentProds.UseVisualStyleBackColor = true;
            this.btnParentProds.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(386, 39);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 47);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnVariationProds
            // 
            this.btnVariationProds.Location = new System.Drawing.Point(156, 39);
            this.btnVariationProds.Name = "btnVariationProds";
            this.btnVariationProds.Size = new System.Drawing.Size(75, 47);
            this.btnVariationProds.TabIndex = 5;
            this.btnVariationProds.Text = "Generate Variation Prods";
            this.btnVariationProds.UseVisualStyleBackColor = true;
            this.btnVariationProds.Click += new System.EventHandler(this.btnVariationProds_Click);
            // 
            // btnSimpleProds
            // 
            this.btnSimpleProds.Location = new System.Drawing.Point(233, 39);
            this.btnSimpleProds.Name = "btnSimpleProds";
            this.btnSimpleProds.Size = new System.Drawing.Size(75, 47);
            this.btnSimpleProds.TabIndex = 6;
            this.btnSimpleProds.Text = "Generate Simple Prods";
            this.btnSimpleProds.UseVisualStyleBackColor = true;
            this.btnSimpleProds.Click += new System.EventHandler(this.btnSimpleProds_Click);
            // 
            // btnMissingSKUs
            // 
            this.btnMissingSKUs.Location = new System.Drawing.Point(310, 39);
            this.btnMissingSKUs.Name = "btnMissingSKUs";
            this.btnMissingSKUs.Size = new System.Drawing.Size(75, 47);
            this.btnMissingSKUs.TabIndex = 7;
            this.btnMissingSKUs.Text = "Generate Missing SKUS";
            this.btnMissingSKUs.UseVisualStyleBackColor = true;
            this.btnMissingSKUs.Click += new System.EventHandler(this.btnMissingSKUs_Click);
            // 
            // btnImageCSVOpen
            // 
            this.btnImageCSVOpen.Location = new System.Drawing.Point(465, 137);
            this.btnImageCSVOpen.Name = "btnImageCSVOpen";
            this.btnImageCSVOpen.Size = new System.Drawing.Size(75, 23);
            this.btnImageCSVOpen.TabIndex = 10;
            this.btnImageCSVOpen.Text = "Open";
            this.btnImageCSVOpen.UseVisualStyleBackColor = true;
            this.btnImageCSVOpen.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtImageSKUFileName
            // 
            this.txtImageSKUFileName.Location = new System.Drawing.Point(80, 138);
            this.txtImageSKUFileName.Name = "txtImageSKUFileName";
            this.txtImageSKUFileName.Size = new System.Drawing.Size(381, 20);
            this.txtImageSKUFileName.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Image SKU File";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // btnGenerateImgCSV
            // 
            this.btnGenerateImgCSV.Location = new System.Drawing.Point(80, 233);
            this.btnGenerateImgCSV.Name = "btnGenerateImgCSV";
            this.btnGenerateImgCSV.Size = new System.Drawing.Size(75, 47);
            this.btnGenerateImgCSV.TabIndex = 11;
            this.btnGenerateImgCSV.Text = "Generate Image CSV";
            this.btnGenerateImgCSV.UseVisualStyleBackColor = true;
            this.btnGenerateImgCSV.Click += new System.EventHandler(this.btnGenerateImgCSV_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(161, 233);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Source File";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(161, 267);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Destination File";
            // 
            // btnSimpleFileOpen
            // 
            this.btnSimpleFileOpen.Location = new System.Drawing.Point(465, 166);
            this.btnSimpleFileOpen.Name = "btnSimpleFileOpen";
            this.btnSimpleFileOpen.Size = new System.Drawing.Size(75, 23);
            this.btnSimpleFileOpen.TabIndex = 16;
            this.btnSimpleFileOpen.Text = "Open";
            this.btnSimpleFileOpen.UseVisualStyleBackColor = true;
            this.btnSimpleFileOpen.Click += new System.EventHandler(this.btnSimpleFileOpen_Click);
            // 
            // txtSimpleProdFile
            // 
            this.txtSimpleProdFile.Location = new System.Drawing.Point(80, 167);
            this.txtSimpleProdFile.Name = "txtSimpleProdFile";
            this.txtSimpleProdFile.Size = new System.Drawing.Size(381, 20);
            this.txtSimpleProdFile.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Simple Prods";
            // 
            // btnVariantsFileOpen
            // 
            this.btnVariantsFileOpen.Location = new System.Drawing.Point(465, 195);
            this.btnVariantsFileOpen.Name = "btnVariantsFileOpen";
            this.btnVariantsFileOpen.Size = new System.Drawing.Size(75, 23);
            this.btnVariantsFileOpen.TabIndex = 19;
            this.btnVariantsFileOpen.Text = "Open";
            this.btnVariantsFileOpen.UseVisualStyleBackColor = true;
            this.btnVariantsFileOpen.Click += new System.EventHandler(this.btnVariantsFileOpen_Click);
            // 
            // txtVariantsFile
            // 
            this.txtVariantsFile.Location = new System.Drawing.Point(80, 196);
            this.txtVariantsFile.Name = "txtVariantsFile";
            this.txtVariantsFile.Size = new System.Drawing.Size(381, 20);
            this.txtVariantsFile.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Variants File";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 294);
            this.Controls.Add(this.btnVariantsFileOpen);
            this.Controls.Add(this.txtVariantsFile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSimpleFileOpen);
            this.Controls.Add(this.txtSimpleProdFile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnGenerateImgCSV);
            this.Controls.Add(this.btnImageCSVOpen);
            this.Controls.Add(this.txtImageSKUFileName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnMissingSKUs);
            this.Controls.Add(this.btnSimpleProds);
            this.Controls.Add(this.btnVariationProds);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnParentProds);
            this.Controls.Add(this.brnOpenCSV);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "CricMax Import CSV Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog dlgOpenCSV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button brnOpenCSV;
        private System.Windows.Forms.Button btnParentProds;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnVariationProds;
        private System.Windows.Forms.Button btnSimpleProds;
        private System.Windows.Forms.Button btnMissingSKUs;
        private System.Windows.Forms.Button btnImageCSVOpen;
        private System.Windows.Forms.TextBox txtImageSKUFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGenerateImgCSV;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSimpleFileOpen;
        private System.Windows.Forms.TextBox txtSimpleProdFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnVariantsFileOpen;
        private System.Windows.Forms.TextBox txtVariantsFile;
        private System.Windows.Forms.Label label6;
    }
}

