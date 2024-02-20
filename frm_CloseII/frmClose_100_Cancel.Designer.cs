namespace MLM_Program
{
    partial class frmClose_100_Cancel
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPayDate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pg1 = new System.Windows.Forms.ProgressBar();
            this.butt_Exit = new System.Windows.Forms.Button();
            this.butt_Pay = new System.Windows.Forms.Button();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_To = new System.Windows.Forms.TextBox();
            this.txt_From = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(7, 225);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(347, 36);
            this.tableLayoutPanel1.TabIndex = 240;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.txtPayDate);
            this.panel1.Location = new System.Drawing.Point(117, 6);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(225, 24);
            this.panel1.TabIndex = 91;
            // 
            // txtPayDate
            // 
            this.txtPayDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(241)))), ((int)(((byte)(220)))));
            this.txtPayDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPayDate.Font = new System.Drawing.Font("돋움", 9.75F);
            this.txtPayDate.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtPayDate.Location = new System.Drawing.Point(1, 1);
            this.txtPayDate.MaxLength = 30;
            this.txtPayDate.Name = "txtPayDate";
            this.txtPayDate.ReadOnly = true;
            this.txtPayDate.Size = new System.Drawing.Size(101, 22);
            this.txtPayDate.TabIndex = 190;
            this.txtPayDate.TabStop = false;
            this.txtPayDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(222)))), ((int)(((byte)(176)))));
            this.label2.Location = new System.Drawing.Point(5, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 32);
            this.label2.TabIndex = 13;
            this.label2.Text = "지급_일자";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pg1
            // 
            this.pg1.Location = new System.Drawing.Point(7, 261);
            this.pg1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pg1.MarqueeAnimationSpeed = 1;
            this.pg1.Maximum = 0;
            this.pg1.Name = "pg1";
            this.pg1.Size = new System.Drawing.Size(347, 19);
            this.pg1.Step = 1;
            this.pg1.TabIndex = 239;
            this.pg1.Visible = false;
            // 
            // butt_Exit
            // 
            this.butt_Exit.Location = new System.Drawing.Point(7, 313);
            this.butt_Exit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butt_Exit.Name = "butt_Exit";
            this.butt_Exit.Size = new System.Drawing.Size(347, 32);
            this.butt_Exit.TabIndex = 238;
            this.butt_Exit.TabStop = false;
            this.butt_Exit.Text = "닫기";
            this.butt_Exit.UseVisualStyleBackColor = true;
            this.butt_Exit.Click += new System.EventHandler(this.butt_Exit_Click);
            // 
            // butt_Pay
            // 
            this.butt_Pay.Location = new System.Drawing.Point(7, 280);
            this.butt_Pay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butt_Pay.Name = "butt_Pay";
            this.butt_Pay.Size = new System.Drawing.Size(347, 32);
            this.butt_Pay.TabIndex = 0;
            this.butt_Pay.TabStop = false;
            this.butt_Pay.Text = "마감_취소";
            this.butt_Pay.UseVisualStyleBackColor = true;
            this.butt_Pay.Click += new System.EventHandler(this.butt_Pay_Click);
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Controls.Add(this.panel7, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel11.Location = new System.Drawing.Point(7, 42);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(347, 36);
            this.tableLayoutPanel11.TabIndex = 237;
            // 
            // panel7
            // 
            this.panel7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel7.Controls.Add(this.label4);
            this.panel7.Controls.Add(this.txt_To);
            this.panel7.Controls.Add(this.txt_From);
            this.panel7.Location = new System.Drawing.Point(117, 6);
            this.panel7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(225, 24);
            this.panel7.TabIndex = 91;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(102, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 19);
            this.label4.TabIndex = 218;
            this.label4.Text = "~";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txt_To
            // 
            this.txt_To.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(241)))), ((int)(((byte)(220)))));
            this.txt_To.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_To.Font = new System.Drawing.Font("돋움", 9.75F);
            this.txt_To.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txt_To.Location = new System.Drawing.Point(123, 1);
            this.txt_To.MaxLength = 30;
            this.txt_To.Name = "txt_To";
            this.txt_To.ReadOnly = true;
            this.txt_To.Size = new System.Drawing.Size(101, 22);
            this.txt_To.TabIndex = 217;
            this.txt_To.TabStop = false;
            this.txt_To.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_From
            // 
            this.txt_From.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(241)))), ((int)(((byte)(220)))));
            this.txt_From.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_From.Font = new System.Drawing.Font("돋움", 9.75F);
            this.txt_From.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txt_From.Location = new System.Drawing.Point(1, 1);
            this.txt_From.MaxLength = 30;
            this.txt_From.Name = "txt_From";
            this.txt_From.ReadOnly = true;
            this.txt_From.Size = new System.Drawing.Size(101, 22);
            this.txt_From.TabIndex = 190;
            this.txt_From.TabStop = false;
            this.txt_From.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(222)))), ((int)(((byte)(176)))));
            this.label3.Location = new System.Drawing.Point(5, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 32);
            this.label3.TabIndex = 13;
            this.label3.Text = "마감_기간";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(222)))), ((int)(((byte)(176)))));
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(5, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(337, 23);
            this.label7.TabIndex = 13;
            this.label7.Text = "수당_관리_센타_마감_취소";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(7, 8);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(347, 27);
            this.tableLayoutPanel5.TabIndex = 236;
            // 
            // frmClose_100_Cancel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(241)))), ((int)(((byte)(220)))));
            this.ClientSize = new System.Drawing.Size(357, 350);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pg1);
            this.Controls.Add(this.butt_Exit);
            this.Controls.Add(this.butt_Pay);
            this.Controls.Add(this.tableLayoutPanel11);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Font = new System.Drawing.Font("돋움", 9F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmClose_100_Cancel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "센타_마감_취소";
            this.Activated += new System.EventHandler(this.frm_Base_Activated);
            this.Load += new System.EventHandler(this.frmBase_From_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_From_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel11.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtPayDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar pg1;
        private System.Windows.Forms.Button butt_Exit;
        private System.Windows.Forms.Button butt_Pay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_To;
        private System.Windows.Forms.TextBox txt_From;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
    }
}