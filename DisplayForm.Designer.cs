namespace BotProject
{
    partial class DisplayForm
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
            this.Start = new System.Windows.Forms.Button();
            this.RawImage = new System.Windows.Forms.PictureBox();
            this.CoOrdinatesLabel = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.DebugImage = new System.Windows.Forms.PictureBox();
            this.MoveApp = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.RawImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DebugImage)).BeginInit();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(236, 378);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(125, 41);
            this.Start.TabIndex = 0;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.CaptureButtonClick);
            // 
            // RawImage
            // 
            this.RawImage.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.RawImage.Location = new System.Drawing.Point(12, 12);
            this.RawImage.Name = "RawImage";
            this.RawImage.Size = new System.Drawing.Size(317, 258);
            this.RawImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.RawImage.TabIndex = 1;
            this.RawImage.TabStop = false;
            // 
            // CoOrdinatesLabel
            // 
            this.CoOrdinatesLabel.Location = new System.Drawing.Point(335, 12);
            this.CoOrdinatesLabel.Name = "CoOrdinatesLabel";
            this.CoOrdinatesLabel.Size = new System.Drawing.Size(130, 145);
            this.CoOrdinatesLabel.TabIndex = 2;
            this.CoOrdinatesLabel.Text = "label1";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(471, 276);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(64, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "DEBUG";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Location = new System.Drawing.Point(12, 273);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(218, 168);
            this.StatusLabel.TabIndex = 4;
            this.StatusLabel.Text = "label2";
            // 
            // DebugImage
            // 
            this.DebugImage.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.DebugImage.Location = new System.Drawing.Point(471, 12);
            this.DebugImage.Name = "DebugImage";
            this.DebugImage.Size = new System.Drawing.Size(317, 258);
            this.DebugImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.DebugImage.TabIndex = 5;
            this.DebugImage.TabStop = false;
            // 
            // MoveApp
            // 
            this.MoveApp.Location = new System.Drawing.Point(377, 378);
            this.MoveApp.Name = "MoveApp";
            this.MoveApp.Size = new System.Drawing.Size(125, 41);
            this.MoveApp.TabIndex = 6;
            this.MoveApp.Text = "MoveApp";
            this.MoveApp.UseVisualStyleBackColor = true;
            // 
            // DisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MoveApp);
            this.Controls.Add(this.DebugImage);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.CoOrdinatesLabel);
            this.Controls.Add(this.RawImage);
            this.Controls.Add(this.Start);
            this.Name = "DisplayForm";
            this.Text = "DisplayForm";
            this.Load += new System.EventHandler(this.DisplayFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.RawImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DebugImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.PictureBox RawImage;
        private System.Windows.Forms.Label CoOrdinatesLabel;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.PictureBox DebugImage;
        private System.Windows.Forms.Button MoveApp;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

