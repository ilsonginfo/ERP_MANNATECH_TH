﻿namespace MLM_Program
{
    partial class frm_Login
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Login));
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.btn_Login = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.lbl_Login = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_Exit = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.tbDD = new System.Windows.Forms.TextBox();
            this.lab_Up = new System.Windows.Forms.Label();
            this.lbl_ver = new System.Windows.Forms.Label();
            this.pan_Language = new System.Windows.Forms.Panel();
            this.cbo_Language = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pan_Language.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUserID
            // 
            resources.ApplyResources(this.txtUserID, "txtUserID");
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Enter += new System.EventHandler(this.txtUserID_Enter);
            this.txtUserID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUserID_KeyPress);
            // 
            // txtPass
            // 
            resources.ApplyResources(this.txtPass, "txtPass");
            this.txtPass.Name = "txtPass";
            this.txtPass.Enter += new System.EventHandler(this.txtPass_Enter);
            this.txtPass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPass_KeyPress_1);
            // 
            // btn_Login
            // 
            resources.ApplyResources(this.btn_Login, "btn_Login");
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // btn_Exit
            // 
            resources.ApplyResources(this.btn_Exit, "btn_Exit");
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // lbl_Login
            // 
            this.lbl_Login.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lbl_Login, "lbl_Login");
            this.lbl_Login.Name = "lbl_Login";
            this.lbl_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // lbl_Exit
            // 
            this.lbl_Exit.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lbl_Exit, "lbl_Exit");
            this.lbl_Exit.Name = "lbl_Exit";
            this.lbl_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // progress
            // 
            resources.ApplyResources(this.progress, "progress");
            this.progress.Name = "progress";
            // 
            // tbDD
            // 
            resources.ApplyResources(this.tbDD, "tbDD");
            this.tbDD.Name = "tbDD";
            // 
            // lab_Up
            // 
            this.lab_Up.BackColor = System.Drawing.Color.White;
            this.lab_Up.ForeColor = System.Drawing.Color.LightSkyBlue;
            resources.ApplyResources(this.lab_Up, "lab_Up");
            this.lab_Up.Name = "lab_Up";
            // 
            // lbl_ver
            // 
            this.lbl_ver.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lbl_ver, "lbl_ver");
            this.lbl_ver.Name = "lbl_ver";
            // 
            // pan_Language
            // 
            this.pan_Language.BackColor = System.Drawing.Color.White;
            this.pan_Language.Controls.Add(this.cbo_Language);
            this.pan_Language.Controls.Add(this.label2);
            resources.ApplyResources(this.pan_Language, "pan_Language");
            this.pan_Language.Name = "pan_Language";
            // 
            // cbo_Language
            // 
            this.cbo_Language.FormattingEnabled = true;
            resources.ApplyResources(this.cbo_Language, "cbo_Language");
            this.cbo_Language.Name = "cbo_Language";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // frm_Login
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ControlBox = false;
            this.Controls.Add(this.pan_Language);
            this.Controls.Add(this.lbl_ver);
            this.Controls.Add(this.lab_Up);
            this.Controls.Add(this.tbDD);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.lbl_Exit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lbl_Login);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.btn_Login);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.txtUserID);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frm_Login";
            this.Activated += new System.EventHandler(this.frm_Login_Activated);
            this.Load += new System.EventHandler(this.frm_Login_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_Login_KeyDown);
            this.pan_Language.ResumeLayout(false);
            this.pan_Language.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.Label lbl_Login;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_Exit;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.TextBox tbDD;
        private System.Windows.Forms.Label lab_Up;
        private System.Windows.Forms.Label lbl_ver;
        private System.Windows.Forms.Panel pan_Language;
        private System.Windows.Forms.ComboBox cbo_Language;
        private System.Windows.Forms.Label label2;
    }
}

