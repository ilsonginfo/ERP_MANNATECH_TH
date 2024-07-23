namespace MLM_Program
{
    partial class frmFastReport
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
            this.preview1 = new FastReport.Preview.PreviewControl();
            this.SuspendLayout();
            // 
            // preview1
            // 
            this.preview1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.preview1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.preview1.Font = new System.Drawing.Font("Angsana New", 8F);
            this.preview1.Location = new System.Drawing.Point(0, 0);
            this.preview1.Name = "preview1";
            //this.preview1.PageOffset = new System.Drawing.Point(10, 10);
            this.preview1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            //this.preview1.SaveInitialDirectory = null;
            this.preview1.Size = new System.Drawing.Size(951, 696);
            this.preview1.StatusbarVisible = false;
            this.preview1.TabIndex = 2;
            this.preview1.UIStyle = FastReport.Utils.UIStyle.Office2007Black;
            // 
            // frmFastReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 696);
            this.Controls.Add(this.preview1);
            this.Name = "frmFastReport";
            this.Text = "리포트 출력";
            this.ResumeLayout(false);

        }

        private FastReport.Preview.PreviewControl preview1;

        #endregion
    }
}