using SwApp.Properties;

namespace SwApp.AssemblyCreation
{
    partial class BushAssembly
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing"></param>
        /// true if managed resources should be disposed;
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
            this.LValue = new System.Windows.Forms.TextBox();
            this.WValue = new System.Windows.Forms.TextBox();
            this.HValue = new System.Windows.Forms.TextBox();
            this.ModelParts = new System.Windows.Forms.Button();
            this.LDimension = new System.Windows.Forms.Label();
            this.WDimension = new System.Windows.Forms.Label();
            this.HDimension = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.LengthLabel = new System.Windows.Forms.Label();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.HeightLabel = new System.Windows.Forms.Label();
            this.BuildAsm = new System.Windows.Forms.Button();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.Part1 = new System.Windows.Forms.PictureBox();
            this.Part2 = new System.Windows.Forms.PictureBox();
            this.Part3 = new System.Windows.Forms.PictureBox();
            this.Drawing = new System.Windows.Forms.PictureBox();
            this.Part3Draw = new System.Windows.Forms.PictureBox();
            this.Part2Draw = new System.Windows.Forms.PictureBox();
            this.Part1Draw = new System.Windows.Forms.PictureBox();
            this.Assembly = new System.Windows.Forms.PictureBox();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Part1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Part2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Part3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Drawing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Part3Draw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Part2Draw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Part1Draw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Assembly)).BeginInit();
            this.SuspendLayout();
            // 
            // LValue
            // 
            this.LValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LValue.Location = new System.Drawing.Point(345, 56);
            this.LValue.Name = "LValue";
            this.LValue.Size = new System.Drawing.Size(56, 32);
            this.LValue.TabIndex = 1;
            this.LValue.Text = "76";
            this.LValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.LValue.TextChanged += new System.EventHandler(this.LValueTextChanged);
            // 
            // WValue
            // 
            this.WValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.WValue.Location = new System.Drawing.Point(345, 216);
            this.WValue.Name = "WValue";
            this.WValue.Size = new System.Drawing.Size(57, 32);
            this.WValue.TabIndex = 2;
            this.WValue.Text = "32";
            this.WValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.WValue.TextChanged += new System.EventHandler(this.WValueTextChanged);
            // 
            // HValue
            // 
            this.HValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.HValue.Location = new System.Drawing.Point(345, 370);
            this.HValue.Name = "HValue";
            this.HValue.Size = new System.Drawing.Size(57, 32);
            this.HValue.TabIndex = 3;
            this.HValue.Text = "35";
            this.HValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.HValue.TextChanged += new System.EventHandler(this.HValueTextChanged);
            // 
            // ModelParts
            // 
            this.ModelParts.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ModelParts.Location = new System.Drawing.Point(486, 408);
            this.ModelParts.Name = "ModelParts";
            this.ModelParts.Size = new System.Drawing.Size(225, 37);
            this.ModelParts.TabIndex = 0;
            this.ModelParts.Text = "Model parts";
            this.ModelParts.UseVisualStyleBackColor = true;
            this.ModelParts.Click += new System.EventHandler(this.ModelPartsClick);
            // 
            // LDimension
            // 
            this.LDimension.AutoSize = true;
            this.LDimension.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.LDimension.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LDimension.Location = new System.Drawing.Point(410, 69);
            this.LDimension.Name = "LDimension";
            this.LDimension.Size = new System.Drawing.Size(46, 20);
            this.LDimension.TabIndex = 5;
            this.LDimension.Text = "l, mm";
            // 
            // WDimension
            // 
            this.WDimension.AutoSize = true;
            this.WDimension.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.WDimension.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.WDimension.Location = new System.Drawing.Point(411, 229);
            this.WDimension.Name = "WDimension";
            this.WDimension.Size = new System.Drawing.Size(54, 20);
            this.WDimension.TabIndex = 6;
            this.WDimension.Text = "w, mm";
            // 
            // HDimension
            // 
            this.HDimension.AutoSize = true;
            this.HDimension.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.HDimension.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.HDimension.Location = new System.Drawing.Point(411, 383);
            this.HDimension.Name = "HDimension";
            this.HDimension.Size = new System.Drawing.Size(52, 20);
            this.HDimension.TabIndex = 7;
            this.HDimension.Text = "h, mm";
            // 
            // Label4
            // 
            this.Label4.Location = new System.Drawing.Point(0, 0);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(100, 23);
            this.Label4.TabIndex = 0;
            // 
            // LengthLabel
            // 
            this.LengthLabel.AutoSize = true;
            this.LengthLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.LengthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LengthLabel.Location = new System.Drawing.Point(270, 62);
            this.LengthLabel.Name = "LengthLabel";
            this.LengthLabel.Size = new System.Drawing.Size(58, 17);
            this.LengthLabel.TabIndex = 10;
            this.LengthLabel.Text = "Length";
            // 
            // WidthLabel
            // 
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.WidthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.WidthLabel.Location = new System.Drawing.Point(277, 222);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(49, 17);
            this.WidthLabel.TabIndex = 11;
            this.WidthLabel.Text = "Width";
            this.WidthLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // HeightLabel
            // 
            this.HeightLabel.AutoSize = true;
            this.HeightLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.HeightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.HeightLabel.Location = new System.Drawing.Point(278, 376);
            this.HeightLabel.Name = "HeightLabel";
            this.HeightLabel.Size = new System.Drawing.Size(55, 17);
            this.HeightLabel.TabIndex = 12;
            this.HeightLabel.Text = "Height";
            this.HeightLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // BuildAsm
            // 
            this.BuildAsm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BuildAsm.Location = new System.Drawing.Point(762, 408);
            this.BuildAsm.Name = "BuildAsm";
            this.BuildAsm.Size = new System.Drawing.Size(277, 37);
            this.BuildAsm.TabIndex = 23;
            this.BuildAsm.Text = "Build assembly";
            this.BuildAsm.UseVisualStyleBackColor = true;
            this.BuildAsm.Click += new System.EventHandler(this.BuildAsmClick);
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Panel1.Controls.Add(this.LDimension);
            this.Panel1.Controls.Add(this.WDimension);
            this.Panel1.Controls.Add(this.HDimension);
            this.Panel1.Location = new System.Drawing.Point(-3, -6);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(470, 477);
            this.Panel1.TabIndex = 29;
            // 
            // Part1
            // 
            this.Part1.Image = global::SwApp.Properties.Resources._1det;
            this.Part1.Location = new System.Drawing.Point(486, 12);
            this.Part1.Name = "Part1";
            this.Part1.Size = new System.Drawing.Size(225, 123);
            this.Part1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Part1.TabIndex = 28;
            this.Part1.TabStop = false;
            // 
            // Part2
            // 
            this.Part2.Image = global::SwApp.Properties.Resources._2det;
            this.Part2.Location = new System.Drawing.Point(486, 146);
            this.Part2.Name = "Part2";
            this.Part2.Size = new System.Drawing.Size(225, 123);
            this.Part2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Part2.TabIndex = 27;
            this.Part2.TabStop = false;
            // 
            // Part3
            // 
            this.Part3.Image = global::SwApp.Properties.Resources._3det;
            this.Part3.Location = new System.Drawing.Point(486, 279);
            this.Part3.Name = "Part3";
            this.Part3.Size = new System.Drawing.Size(225, 123);
            this.Part3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Part3.TabIndex = 26;
            this.Part3.TabStop = false;
            // 
            // Drawing
            // 
            this.Drawing.Image = global::SwApp.Properties.Resources._2d;
            this.Drawing.Location = new System.Drawing.Point(762, 12);
            this.Drawing.Name = "Drawing";
            this.Drawing.Size = new System.Drawing.Size(277, 203);
            this.Drawing.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Drawing.TabIndex = 22;
            this.Drawing.TabStop = false;
            // 
            // Part3Draw
            // 
            this.Part3Draw.Image = global::SwApp.Properties.Resources.height;
            this.Part3Draw.Location = new System.Drawing.Point(12, 334);
            this.Part3Draw.Name = "Part3Draw";
            this.Part3Draw.Size = new System.Drawing.Size(248, 111);
            this.Part3Draw.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Part3Draw.TabIndex = 20;
            this.Part3Draw.TabStop = false;
            // 
            // Part2Draw
            // 
            this.Part2Draw.Image = global::SwApp.Properties.Resources.width;
            this.Part2Draw.Location = new System.Drawing.Point(12, 182);
            this.Part2Draw.Name = "Part2Draw";
            this.Part2Draw.Size = new System.Drawing.Size(248, 108);
            this.Part2Draw.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Part2Draw.TabIndex = 19;
            this.Part2Draw.TabStop = false;
            // 
            // Part1Draw
            // 
            this.Part1Draw.Image = global::SwApp.Properties.Resources.length;
            this.Part1Draw.Location = new System.Drawing.Point(12, 12);
            this.Part1Draw.Name = "Part1Draw";
            this.Part1Draw.Size = new System.Drawing.Size(248, 123);
            this.Part1Draw.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Part1Draw.TabIndex = 18;
            this.Part1Draw.TabStop = false;
            // 
            // Assembly
            // 
            this.Assembly.Image = global::SwApp.Properties.Resources.cdet;
            this.Assembly.Location = new System.Drawing.Point(762, 228);
            this.Assembly.Name = "Assembly";
            this.Assembly.Size = new System.Drawing.Size(277, 174);
            this.Assembly.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Assembly.TabIndex = 9;
            this.Assembly.TabStop = false;
            // 
            // BushAssembly
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 457);
            this.Controls.Add(this.Part1);
            this.Controls.Add(this.Part2);
            this.Controls.Add(this.Part3);
            this.Controls.Add(this.BuildAsm);
            this.Controls.Add(this.Drawing);
            this.Controls.Add(this.Part3Draw);
            this.Controls.Add(this.Part2Draw);
            this.Controls.Add(this.Part1Draw);
            this.Controls.Add(this.HeightLabel);
            this.Controls.Add(this.WidthLabel);
            this.Controls.Add(this.LengthLabel);
            this.Controls.Add(this.Assembly);
            this.Controls.Add(this.HValue);
            this.Controls.Add(this.WValue);
            this.Controls.Add(this.LValue);
            this.Controls.Add(this.ModelParts);
            this.Controls.Add(this.Panel1);
            this.KeyPreview = true;
            this.Name = "BushAssembly";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Part1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Part2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Part3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Drawing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Part3Draw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Part2Draw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Part1Draw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Assembly)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox LValue;
        private System.Windows.Forms.TextBox WValue;
        private System.Windows.Forms.TextBox HValue;
        private System.Windows.Forms.Button ModelParts;
        private System.Windows.Forms.PictureBox Assembly;
        private System.Windows.Forms.PictureBox Part1Draw;
        private System.Windows.Forms.PictureBox Part2Draw;
        private System.Windows.Forms.PictureBox Part3Draw;
        private System.Windows.Forms.PictureBox Drawing;
        private System.Windows.Forms.Label LDimension;
        private System.Windows.Forms.Label WDimension;
        private System.Windows.Forms.Label HDimension;
        private System.Windows.Forms.Label Label4;
        private System.Windows.Forms.Label LengthLabel;
        private System.Windows.Forms.Label WidthLabel;
        private System.Windows.Forms.Label HeightLabel;
        private System.Windows.Forms.Button BuildAsm;
        private System.Windows.Forms.PictureBox Part3;
        private System.Windows.Forms.PictureBox Part2;
        private System.Windows.Forms.PictureBox Part1;
        private System.Windows.Forms.Panel Panel1;
    }
}
