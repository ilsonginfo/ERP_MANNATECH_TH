namespace MLM_Program
{
    partial class frm_Excel_Import_Rec
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dGridView_Base = new System.Windows.Forms.DataGridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.butt_Search = new System.Windows.Forms.Button();
            this.combo_Sheet = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.butt_Excel = new System.Windows.Forms.Button();
            this.butt_Delete = new System.Windows.Forms.Button();
            this.butt_Clear = new System.Windows.Forms.Button();
            this.butt_Select = new System.Windows.Forms.Button();
            this.butt_Exit = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnExcelTemplateDownload = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel38 = new System.Windows.Forms.TableLayoutPanel();
            this.label40 = new System.Windows.Forms.Label();
            this.panel23 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dGridView_Base)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel38.SuspendLayout();
            this.panel23.SuspendLayout();
            this.SuspendLayout();
            // 
            // dGridView_Base
            // 
            this.dGridView_Base.BackgroundColor = System.Drawing.Color.White;
            this.dGridView_Base.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("돋움", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dGridView_Base.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dGridView_Base.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("돋움", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGridView_Base.DefaultCellStyle = dataGridViewCellStyle2;
            this.dGridView_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGridView_Base.GridColor = System.Drawing.SystemColors.ButtonFace;
            this.dGridView_Base.Location = new System.Drawing.Point(0, 107);
            this.dGridView_Base.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.dGridView_Base.Name = "dGridView_Base";
            this.dGridView_Base.RowTemplate.Height = 23;
            this.dGridView_Base.Size = new System.Drawing.Size(1292, 675);
            this.dGridView_Base.TabIndex = 8;
            this.dGridView_Base.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dGridView_Base_CellPainting);
            this.dGridView_Base.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dGridView_KeyDown);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtFilePath
            // 
            this.txtFilePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(241)))), ((int)(((byte)(220)))));
            this.txtFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilePath.Font = new System.Drawing.Font("돋움", 9.75F);
            this.txtFilePath.Location = new System.Drawing.Point(3, 3);
            this.txtFilePath.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtFilePath.MaxLength = 30;
            this.txtFilePath.Multiline = true;
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(652, 22);
            this.txtFilePath.TabIndex = 9;
            // 
            // butt_Search
            // 
            this.butt_Search.BackColor = System.Drawing.Color.White;
            this.butt_Search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butt_Search.Location = new System.Drawing.Point(3, 3);
            this.butt_Search.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.butt_Search.Name = "butt_Search";
            this.butt_Search.Size = new System.Drawing.Size(212, 35);
            this.butt_Search.TabIndex = 0;
            this.butt_Search.Text = "적용_파일_찾기";
            this.butt_Search.UseVisualStyleBackColor = false;
            this.butt_Search.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // combo_Sheet
            // 
            this.combo_Sheet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.combo_Sheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Sheet.FormattingEnabled = true;
            this.combo_Sheet.Location = new System.Drawing.Point(3, 4);
            this.combo_Sheet.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.combo_Sheet.Name = "combo_Sheet";
            this.combo_Sheet.Size = new System.Drawing.Size(245, 20);
            this.combo_Sheet.TabIndex = 194;
            this.combo_Sheet.SelectedIndexChanged += new System.EventHandler(this.combo_Pay_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.butt_Excel);
            this.panel1.Controls.Add(this.butt_Delete);
            this.panel1.Controls.Add(this.butt_Clear);
            this.panel1.Controls.Add(this.butt_Select);
            this.panel1.Controls.Add(this.butt_Exit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1292, 28);
            this.panel1.TabIndex = 197;
            // 
            // butt_Excel
            // 
            this.butt_Excel.BackColor = System.Drawing.Color.White;
            this.butt_Excel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butt_Excel.Location = new System.Drawing.Point(538, 1);
            this.butt_Excel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.butt_Excel.Name = "butt_Excel";
            this.butt_Excel.Size = new System.Drawing.Size(111, 26);
            this.butt_Excel.TabIndex = 9;
            this.butt_Excel.TabStop = false;
            this.butt_Excel.Text = "엑셀";
            this.butt_Excel.UseVisualStyleBackColor = false;
            this.butt_Excel.Visible = false;
            this.butt_Excel.Click += new System.EventHandler(this.butt_Excel_Click);
            // 
            // butt_Delete
            // 
            this.butt_Delete.BackColor = System.Drawing.Color.White;
            this.butt_Delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butt_Delete.Location = new System.Drawing.Point(135, 1);
            this.butt_Delete.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.butt_Delete.Name = "butt_Delete";
            this.butt_Delete.Size = new System.Drawing.Size(111, 26);
            this.butt_Delete.TabIndex = 8;
            this.butt_Delete.TabStop = false;
            this.butt_Delete.Text = "삭제";
            this.butt_Delete.UseVisualStyleBackColor = false;
            this.butt_Delete.Visible = false;
            // 
            // butt_Clear
            // 
            this.butt_Clear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(89)))), ((int)(((byte)(97)))));
            this.butt_Clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butt_Clear.ForeColor = System.Drawing.Color.White;
            this.butt_Clear.Location = new System.Drawing.Point(276, 1);
            this.butt_Clear.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.butt_Clear.Name = "butt_Clear";
            this.butt_Clear.Size = new System.Drawing.Size(111, 26);
            this.butt_Clear.TabIndex = 7;
            this.butt_Clear.TabStop = false;
            this.butt_Clear.Text = "새로입력";
            this.butt_Clear.UseVisualStyleBackColor = false;
            this.butt_Clear.Visible = false;
            // 
            // butt_Select
            // 
            this.butt_Select.BackColor = System.Drawing.Color.White;
            this.butt_Select.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butt_Select.Location = new System.Drawing.Point(421, 1);
            this.butt_Select.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.butt_Select.Name = "butt_Select";
            this.butt_Select.Size = new System.Drawing.Size(111, 26);
            this.butt_Select.TabIndex = 6;
            this.butt_Select.Text = "저장";
            this.butt_Select.UseVisualStyleBackColor = false;
            this.butt_Select.Click += new System.EventHandler(this.butt_Exit_Click);
            // 
            // butt_Exit
            // 
            this.butt_Exit.BackColor = System.Drawing.Color.White;
            this.butt_Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butt_Exit.Location = new System.Drawing.Point(668, 1);
            this.butt_Exit.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.butt_Exit.Name = "butt_Exit";
            this.butt_Exit.Size = new System.Drawing.Size(111, 26);
            this.butt_Exit.TabIndex = 5;
            this.butt_Exit.TabStop = false;
            this.butt_Exit.Text = "닫기";
            this.butt_Exit.UseVisualStyleBackColor = false;
            this.butt_Exit.Click += new System.EventHandler(this.butt_Exit_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(1036, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(64, 78);
            this.groupBox2.TabIndex = 198;
            this.groupBox2.TabStop = false;
            this.groupBox2.Visible = false;
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(599, 40);
            this.progress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(406, 16);
            this.progress.TabIndex = 181;
            this.progress.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnExcelTemplateDownload);
            this.panel2.Controls.Add(this.tableLayoutPanel2);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.progress);
            this.panel2.Controls.Add(this.tableLayoutPanel38);
            this.panel2.Controls.Add(this.butt_Search);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1292, 79);
            this.panel2.TabIndex = 199;
            // 
            // btnExcelTemplateDownload
            // 
            this.btnExcelTemplateDownload.BackColor = System.Drawing.Color.White;
            this.btnExcelTemplateDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcelTemplateDownload.Location = new System.Drawing.Point(1011, 6);
            this.btnExcelTemplateDownload.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnExcelTemplateDownload.Name = "btnExcelTemplateDownload";
            this.btnExcelTemplateDownload.Size = new System.Drawing.Size(212, 35);
            this.btnExcelTemplateDownload.TabIndex = 199;
            this.btnExcelTemplateDownload.Text = "템플릿 다운로드";
            this.btnExcelTemplateDownload.UseVisualStyleBackColor = false;
            this.btnExcelTemplateDownload.Click += new System.EventHandler(this.btnExcelTemplateDownload_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(144)))), ((int)(((byte)(176)))));
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(217, 40);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(381, 36);
            this.tableLayoutPanel2.TabIndex = 186;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.combo_Sheet);
            this.panel3.Location = new System.Drawing.Point(126, 4);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(251, 28);
            this.panel3.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(144)))), ((int)(((byte)(176)))));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(2, 2);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 32);
            this.label7.TabIndex = 0;
            this.label7.Text = "적용_Sheet";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel38
            // 
            this.tableLayoutPanel38.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(144)))), ((int)(((byte)(176)))));
            this.tableLayoutPanel38.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel38.ColumnCount = 2;
            this.tableLayoutPanel38.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel38.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel38.Controls.Add(this.label40, 0, 0);
            this.tableLayoutPanel38.Controls.Add(this.panel23, 1, 0);
            this.tableLayoutPanel38.Location = new System.Drawing.Point(217, 2);
            this.tableLayoutPanel38.Name = "tableLayoutPanel38";
            this.tableLayoutPanel38.RowCount = 1;
            this.tableLayoutPanel38.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel38.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel38.Size = new System.Drawing.Size(788, 37);
            this.tableLayoutPanel38.TabIndex = 185;
            // 
            // label40
            // 
            this.label40.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(144)))), ((int)(((byte)(176)))));
            this.label40.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label40.ForeColor = System.Drawing.Color.White;
            this.label40.Location = new System.Drawing.Point(2, 2);
            this.label40.Margin = new System.Windows.Forms.Padding(0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(120, 33);
            this.label40.TabIndex = 0;
            this.label40.Text = "적용_파일_경로";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel23
            // 
            this.panel23.BackColor = System.Drawing.Color.White;
            this.panel23.Controls.Add(this.txtFilePath);
            this.panel23.Location = new System.Drawing.Point(126, 4);
            this.panel23.Margin = new System.Windows.Forms.Padding(2);
            this.panel23.Name = "panel23";
            this.panel23.Size = new System.Drawing.Size(658, 28);
            this.panel23.TabIndex = 15;
            // 
            // frm_Excel_Import_Rec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1137, 782);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1309, 761);
            this.Controls.Add(this.dGridView_Base);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("돋움", 9F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frm_Excel_Import_Rec";
            this.Text = "택배사_관련_정보_가져오기";
            this.Activated += new System.EventHandler(this.frm_Base_Activated);
            this.Load += new System.EventHandler(this.frm_Excel_Import_Rec_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_From_KeyDown);
            this.Resize += new System.EventHandler(this.frmBase_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dGridView_Base)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tableLayoutPanel38.ResumeLayout(false);
            this.panel23.ResumeLayout(false);
            this.panel23.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dGridView_Base;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button butt_Search;
        private System.Windows.Forms.ComboBox combo_Sheet;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button butt_Excel;
        private System.Windows.Forms.Button butt_Delete;
        private System.Windows.Forms.Button butt_Clear;
        private System.Windows.Forms.Button butt_Select;
        private System.Windows.Forms.Button butt_Exit;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel38;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Panel panel23;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnExcelTemplateDownload;
    }
}