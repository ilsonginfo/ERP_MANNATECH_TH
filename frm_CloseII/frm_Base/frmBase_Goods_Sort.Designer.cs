﻿namespace MLM_Program
{
    partial class frmBase_Goods_Sort
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pn_Button = new System.Windows.Forms.Panel();
            this.butt_Excel = new System.Windows.Forms.Button();
            this.butt_Delete = new System.Windows.Forms.Button();
            this.butt_Clear = new System.Windows.Forms.Button();
            this.butt_Save = new System.Windows.Forms.Button();
            this.butt_Exit = new System.Windows.Forms.Button();
            this.dGridView_Base = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtData = new System.Windows.Forms.TextBox();
            this.txtData2 = new System.Windows.Forms.TextBox();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.pn_Button.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGridView_Base)).BeginInit();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pn_Button
            // 
            this.pn_Button.Controls.Add(this.butt_Excel);
            this.pn_Button.Controls.Add(this.butt_Delete);
            this.pn_Button.Controls.Add(this.butt_Clear);
            this.pn_Button.Controls.Add(this.butt_Save);
            this.pn_Button.Controls.Add(this.butt_Exit);
            this.pn_Button.Dock = System.Windows.Forms.DockStyle.Top;
            this.pn_Button.Location = new System.Drawing.Point(0, 0);
            this.pn_Button.Name = "pn_Button";
            this.pn_Button.Size = new System.Drawing.Size(1086, 28);
            this.pn_Button.TabIndex = 1;
            // 
            // butt_Excel
            // 
            this.butt_Excel.BackColor = System.Drawing.Color.White;
            this.butt_Excel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butt_Excel.Location = new System.Drawing.Point(528, 1);
            this.butt_Excel.Name = "butt_Excel";
            this.butt_Excel.Size = new System.Drawing.Size(111, 26);
            this.butt_Excel.TabIndex = 4;
            this.butt_Excel.TabStop = false;
            this.butt_Excel.Text = "엑셀";
            this.butt_Excel.UseVisualStyleBackColor = false;
            this.butt_Excel.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // butt_Delete
            // 
            this.butt_Delete.BackColor = System.Drawing.Color.White;
            this.butt_Delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butt_Delete.Location = new System.Drawing.Point(125, 1);
            this.butt_Delete.Name = "butt_Delete";
            this.butt_Delete.Size = new System.Drawing.Size(111, 26);
            this.butt_Delete.TabIndex = 3;
            this.butt_Delete.TabStop = false;
            this.butt_Delete.Text = "삭제";
            this.butt_Delete.UseVisualStyleBackColor = false;
            this.butt_Delete.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // butt_Clear
            // 
            this.butt_Clear.BackColor = System.Drawing.Color.White;
            this.butt_Clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butt_Clear.Location = new System.Drawing.Point(266, 1);
            this.butt_Clear.Name = "butt_Clear";
            this.butt_Clear.Size = new System.Drawing.Size(111, 26);
            this.butt_Clear.TabIndex = 2;
            this.butt_Clear.TabStop = false;
            this.butt_Clear.Text = "새로입력";
            this.butt_Clear.UseVisualStyleBackColor = false;
            this.butt_Clear.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // butt_Save
            // 
            this.butt_Save.BackColor = System.Drawing.Color.White;
            this.butt_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butt_Save.Location = new System.Drawing.Point(411, 1);
            this.butt_Save.Name = "butt_Save";
            this.butt_Save.Size = new System.Drawing.Size(111, 26);
            this.butt_Save.TabIndex = 2;
            this.butt_Save.Text = "저장";
            this.butt_Save.UseVisualStyleBackColor = false;
            this.butt_Save.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // butt_Exit
            // 
            this.butt_Exit.BackColor = System.Drawing.Color.White;
            this.butt_Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butt_Exit.Location = new System.Drawing.Point(658, 1);
            this.butt_Exit.Name = "butt_Exit";
            this.butt_Exit.Size = new System.Drawing.Size(111, 26);
            this.butt_Exit.TabIndex = 0;
            this.butt_Exit.TabStop = false;
            this.butt_Exit.Text = "닫기";
            this.butt_Exit.UseVisualStyleBackColor = false;
            this.butt_Exit.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // dGridView_Base
            // 
            this.dGridView_Base.BackgroundColor = System.Drawing.Color.White;
            this.dGridView_Base.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGridView_Base.DefaultCellStyle = dataGridViewCellStyle1;
            this.dGridView_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGridView_Base.GridColor = System.Drawing.SystemColors.ButtonFace;
            this.dGridView_Base.Location = new System.Drawing.Point(0, 67);
            this.dGridView_Base.Name = "dGridView_Base";
            this.dGridView_Base.RowTemplate.Height = 23;
            this.dGridView_Base.Size = new System.Drawing.Size(1086, 633);
            this.dGridView_Base.TabIndex = 7;
            this.dGridView_Base.DoubleClick += new System.EventHandler(this.dGridView_Base_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(758, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(47, 31);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // txtData
            // 
            this.txtData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtData.Font = new System.Drawing.Font("굴림", 9.75F);
            this.txtData.Location = new System.Drawing.Point(3, 3);
            this.txtData.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.txtData.MaxLength = 5;
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(164, 22);
            this.txtData.TabIndex = 0;
            this.txtData.TextChanged += new System.EventHandler(this.txtData_TextChanged);
            this.txtData.Enter += new System.EventHandler(this.txtData_Enter);
            this.txtData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtData_KeyPress);
            this.txtData.Leave += new System.EventHandler(this.txtData_Base_Leave);
            // 
            // txtData2
            // 
            this.txtData2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtData2.Font = new System.Drawing.Font("굴림", 9.75F);
            this.txtData2.Location = new System.Drawing.Point(3, 3);
            this.txtData2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.txtData2.MaxLength = 30;
            this.txtData2.Name = "txtData2";
            this.txtData2.Size = new System.Drawing.Size(164, 22);
            this.txtData2.TabIndex = 1;
            this.txtData2.TextChanged += new System.EventHandler(this.txtData_TextChanged);
            this.txtData2.Enter += new System.EventHandler(this.txtData_Enter);
            this.txtData2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtData_KeyPress);
            this.txtData2.Leave += new System.EventHandler(this.txtData_Base_Leave);
            // 
            // txtKey
            // 
            this.txtKey.Font = new System.Drawing.Font("굴림", 9.75F);
            this.txtKey.Location = new System.Drawing.Point(605, 11);
            this.txtKey.MaxLength = 30;
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(134, 22);
            this.txtKey.TabIndex = 14;
            this.txtKey.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.tableLayoutPanel2);
            this.panel2.Controls.Add(this.txtKey);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.tableLayoutPanel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1086, 39);
            this.panel2.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(117)))), ((int)(((byte)(159)))));
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(-1, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(300, 36);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.txtData);
            this.panel4.Location = new System.Drawing.Point(126, 4);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(170, 28);
            this.panel4.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(117)))), ((int)(((byte)(159)))));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(2, 2);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 32);
            this.label3.TabIndex = 0;
            this.label3.Text = "대분류_코드";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(117)))), ((int)(((byte)(159)))));
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(299, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(300, 36);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.txtData2);
            this.panel3.Location = new System.Drawing.Point(126, 4);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(170, 28);
            this.panel3.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(117)))), ((int)(((byte)(159)))));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(2, 2);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 32);
            this.label4.TabIndex = 0;
            this.label4.Text = "대분류명";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmBase_Goods_Sort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1086, 700);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1087, 679);
            this.Controls.Add(this.dGridView_Base);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pn_Button);
            this.KeyPreview = true;
            this.Name = "frmBase_Goods_Sort";
            this.Text = "상품_대분류_등록";
            this.Load += new System.EventHandler(this.frmBase_From_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_From_KeyDown);
            this.Resize += new System.EventHandler(this.frmBase_Resize);
            this.pn_Button.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGridView_Base)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pn_Button;
        private System.Windows.Forms.Button butt_Excel;
        private System.Windows.Forms.Button butt_Delete;
        private System.Windows.Forms.Button butt_Clear;
        private System.Windows.Forms.Button butt_Save;
        private System.Windows.Forms.Button butt_Exit;
        private System.Windows.Forms.DataGridView dGridView_Base;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.TextBox txtData2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
    }
}