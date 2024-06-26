﻿namespace MLM_Program
{
    partial class frmClose_1_Cancel
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
            this.pg1 = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.txtPayDate = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tableLayoutPanel31 = new System.Windows.Forms.TableLayoutPanel();
            this.panel24 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_To = new System.Windows.Forms.TextBox();
            this.txt_From = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.butt_Exit = new System.Windows.Forms.Button();
            this.butt_Pay = new System.Windows.Forms.Button();
            this.tableLayoutPanel16.SuspendLayout();
            this.panel14.SuspendLayout();
            this.tableLayoutPanel31.SuspendLayout();
            this.panel24.SuspendLayout();
            this.SuspendLayout();
            // 
            // pg1
            // 
            this.pg1.Location = new System.Drawing.Point(7, 258);
            this.pg1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pg1.MarqueeAnimationSpeed = 1;
            this.pg1.Maximum = 0;
            this.pg1.Name = "pg1";
            this.pg1.Size = new System.Drawing.Size(347, 19);
            this.pg1.Step = 1;
            this.pg1.TabIndex = 233;
            this.pg1.Visible = false;
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(144)))), ((int)(((byte)(176)))));
            this.tableLayoutPanel16.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel16.ColumnCount = 2;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.Controls.Add(this.panel14, 1, 0);
            this.tableLayoutPanel16.Controls.Add(this.label16, 0, 0);
            this.tableLayoutPanel16.Location = new System.Drawing.Point(2, 38);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 1;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(354, 36);
            this.tableLayoutPanel16.TabIndex = 258;
            // 
            // panel14
            // 
            this.panel14.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel14.BackColor = System.Drawing.Color.White;
            this.panel14.Controls.Add(this.txtPayDate);
            this.panel14.Location = new System.Drawing.Point(126, 4);
            this.panel14.Margin = new System.Windows.Forms.Padding(2);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(224, 28);
            this.panel14.TabIndex = 15;
            // 
            // txtPayDate
            // 
            this.txtPayDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtPayDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPayDate.Font = new System.Drawing.Font("돋움", 9.75F);
            this.txtPayDate.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txtPayDate.Location = new System.Drawing.Point(3, 3);
            this.txtPayDate.MaxLength = 30;
            this.txtPayDate.Name = "txtPayDate";
            this.txtPayDate.ReadOnly = true;
            this.txtPayDate.Size = new System.Drawing.Size(94, 22);
            this.txtPayDate.TabIndex = 190;
            this.txtPayDate.TabStop = false;
            this.txtPayDate.Tag = global::MLM_Program.Resources.Japan_Caption_Resource.주민번호_없이;
            this.txtPayDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(144)))), ((int)(((byte)(176)))));
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(2, 2);
            this.label16.Margin = new System.Windows.Forms.Padding(0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(120, 32);
            this.label16.TabIndex = 0;
            this.label16.Text = "지급일자";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel31
            // 
            this.tableLayoutPanel31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(144)))), ((int)(((byte)(176)))));
            this.tableLayoutPanel31.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel31.ColumnCount = 2;
            this.tableLayoutPanel31.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel31.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel31.Controls.Add(this.panel24, 1, 0);
            this.tableLayoutPanel31.Controls.Add(this.label36, 0, 0);
            this.tableLayoutPanel31.Location = new System.Drawing.Point(2, 1);
            this.tableLayoutPanel31.Name = "tableLayoutPanel31";
            this.tableLayoutPanel31.RowCount = 1;
            this.tableLayoutPanel31.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel31.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel31.Size = new System.Drawing.Size(354, 36);
            this.tableLayoutPanel31.TabIndex = 257;
            // 
            // panel24
            // 
            this.panel24.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel24.BackColor = System.Drawing.Color.White;
            this.panel24.Controls.Add(this.label1);
            this.panel24.Controls.Add(this.txt_To);
            this.panel24.Controls.Add(this.txt_From);
            this.panel24.Location = new System.Drawing.Point(126, 4);
            this.panel24.Margin = new System.Windows.Forms.Padding(2);
            this.panel24.Name = "panel24";
            this.panel24.Size = new System.Drawing.Size(224, 28);
            this.panel24.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(102, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 19);
            this.label1.TabIndex = 218;
            this.label1.Text = "~";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txt_To
            // 
            this.txt_To.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txt_To.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_To.Font = new System.Drawing.Font("돋움", 9.75F);
            this.txt_To.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txt_To.Location = new System.Drawing.Point(126, 3);
            this.txt_To.MaxLength = 30;
            this.txt_To.Name = "txt_To";
            this.txt_To.ReadOnly = true;
            this.txt_To.Size = new System.Drawing.Size(94, 22);
            this.txt_To.TabIndex = 217;
            this.txt_To.TabStop = false;
            this.txt_To.Tag = global::MLM_Program.Resources.Japan_Caption_Resource.주민번호_없이;
            this.txt_To.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_From
            // 
            this.txt_From.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txt_From.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_From.Font = new System.Drawing.Font("돋움", 9.75F);
            this.txt_From.ForeColor = System.Drawing.Color.DodgerBlue;
            this.txt_From.Location = new System.Drawing.Point(3, 3);
            this.txt_From.MaxLength = 30;
            this.txt_From.Name = "txt_From";
            this.txt_From.ReadOnly = true;
            this.txt_From.Size = new System.Drawing.Size(94, 22);
            this.txt_From.TabIndex = 190;
            this.txt_From.TabStop = false;
            this.txt_From.Tag = global::MLM_Program.Resources.Japan_Caption_Resource.주민번호_없이;
            this.txt_From.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label36
            // 
            this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(144)))), ((int)(((byte)(176)))));
            this.label36.ForeColor = System.Drawing.Color.White;
            this.label36.Location = new System.Drawing.Point(2, 2);
            this.label36.Margin = new System.Windows.Forms.Padding(0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(120, 32);
            this.label36.TabIndex = 0;
            this.label36.Text = "마감기간";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // butt_Exit
            // 
            this.butt_Exit.BackColor = System.Drawing.Color.White;
            this.butt_Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butt_Exit.Location = new System.Drawing.Point(2, 312);
            this.butt_Exit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butt_Exit.Name = "butt_Exit";
            this.butt_Exit.Size = new System.Drawing.Size(354, 32);
            this.butt_Exit.TabIndex = 260;
            this.butt_Exit.TabStop = false;
            this.butt_Exit.Text = "닫기";
            this.butt_Exit.UseVisualStyleBackColor = false;
            this.butt_Exit.Click += new System.EventHandler(this.butt_Exit_Click);
            // 
            // butt_Pay
            // 
            this.butt_Pay.BackColor = System.Drawing.Color.White;
            this.butt_Pay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butt_Pay.Location = new System.Drawing.Point(2, 279);
            this.butt_Pay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butt_Pay.Name = "butt_Pay";
            this.butt_Pay.Size = new System.Drawing.Size(354, 32);
            this.butt_Pay.TabIndex = 259;
            this.butt_Pay.TabStop = false;
            this.butt_Pay.Text = "마감_취소";
            this.butt_Pay.UseVisualStyleBackColor = false;
            this.butt_Pay.Click += new System.EventHandler(this.butt_Pay_Click);
            // 
            // frmClose_1_Cancel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(358, 347);
            this.Controls.Add(this.butt_Exit);
            this.Controls.Add(this.butt_Pay);
            this.Controls.Add(this.tableLayoutPanel16);
            this.Controls.Add(this.tableLayoutPanel31);
            this.Controls.Add(this.pg1);
            this.Font = new System.Drawing.Font("돋움", 9F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmClose_1_Cancel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "주간(구기간)_마감_취소";
            this.Activated += new System.EventHandler(this.frm_Base_Activated);
            this.Load += new System.EventHandler(this.frmBase_From_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_From_KeyDown);
            this.tableLayoutPanel16.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            this.tableLayoutPanel31.ResumeLayout(false);
            this.panel24.ResumeLayout(false);
            this.panel24.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar pg1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.TextBox txtPayDate;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel31;
        private System.Windows.Forms.Panel panel24;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_To;
        private System.Windows.Forms.TextBox txt_From;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Button butt_Exit;
        private System.Windows.Forms.Button butt_Pay;
    }
}