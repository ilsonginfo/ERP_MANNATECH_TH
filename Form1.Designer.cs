namespace MLM_Program
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button12 = new System.Windows.Forms.Button();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.edt_emv = new System.Windows.Forms.TextBox();
            this.button14 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.EDT_EVentHext = new System.Windows.Forms.TextBox();
            this.EDT_EVENT = new System.Windows.Forms.TextBox();
            this.EDT_RCD = new System.Windows.Forms.TextBox();
            this.EDT_JCD = new System.Windows.Forms.TextBox();
            this.EDT_GCD = new System.Windows.Forms.TextBox();
            this.EDT_CMDF = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "포트";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(57, 21);
            this.textBox1.MaxLength = 2;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(35, 21);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "1";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(133, 21);
            this.textBox2.MaxLength = 6;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(57, 21);
            this.textBox2.TabIndex = 3;
            this.textBox2.Text = "57600";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "속도";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(196, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 20);
            this.button1.TabIndex = 4;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(291, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 20);
            this.button2.TabIndex = 5;
            this.button2.Text = "DisConnect";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(407, 21);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(89, 20);
            this.button3.TabIndex = 6;
            this.button3.Text = "초기화";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(502, 20);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(89, 20);
            this.button4.TabIndex = 7;
            this.button4.Text = "Sign";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(597, 20);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(89, 20);
            this.button5.TabIndex = 8;
            this.button5.Text = "Cash";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(794, 20);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(89, 20);
            this.button6.TabIndex = 9;
            this.button6.Text = "ShopInfo";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(205, 47);
            this.textBox3.MaxLength = 5;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(57, 21);
            this.textBox3.TabIndex = 13;
            this.textBox3.Text = "15200";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(161, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "PORT";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(57, 48);
            this.textBox4.MaxLength = 20;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(98, 21);
            this.textBox4.TabIndex = 11;
            this.textBox4.Text = "203.233.72.55";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "IP";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(65, 184);
            this.textBox5.MaxLength = 1024;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(629, 21);
            this.textBox5.TabIndex = 15;
            this.textBox5.Text = "S01=EX;S02=D1;S03= ;S04=40;S05=0700081;S08=A;S09=9410457864546012=070510123410724" +
                "67509;S12=1004;S11=00;S19=N;";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 187);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "요청전문";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(65, 211);
            this.textBox6.MaxLength = 2048;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(629, 21);
            this.textBox6.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 214);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "응답전문";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(700, 184);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(89, 20);
            this.button7.TabIndex = 18;
            this.button7.Text = "Send";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(700, 211);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(89, 20);
            this.button8.TabIndex = 19;
            this.button8.Text = "getTRNO";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(802, 211);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(89, 20);
            this.button9.TabIndex = 20;
            this.button9.Text = "통신취소";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.button12);
            this.groupBox1.Controls.Add(this.textBox7);
            this.groupBox1.Controls.Add(this.button11);
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Location = new System.Drawing.Point(43, 257);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(837, 104);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TS 단말기 연동";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "요청";
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(700, 65);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(75, 23);
            this.button12.TabIndex = 17;
            this.button12.Text = "단말기승인";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(65, 67);
            this.textBox7.MaxLength = 1024;
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(629, 21);
            this.textBox7.TabIndex = 16;
            this.textBox7.Text = "D1                                                             1000            91" +
                "      12345678901234567890";
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(322, 20);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(169, 20);
            this.button11.TabIndex = 9;
            this.button11.Text = "MSR 리딩 + 서명";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(65, 20);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(169, 20);
            this.button10.TabIndex = 8;
            this.button10.Text = "MSR 리딩";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.edt_emv);
            this.groupBox2.Controls.Add(this.button14);
            this.groupBox2.Controls.Add(this.button13);
            this.groupBox2.Location = new System.Drawing.Point(43, 377);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(837, 80);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "서명패드";
            // 
            // edt_emv
            // 
            this.edt_emv.Location = new System.Drawing.Point(270, 36);
            this.edt_emv.Name = "edt_emv";
            this.edt_emv.Size = new System.Drawing.Size(435, 21);
            this.edt_emv.TabIndex = 25;
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(162, 36);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(89, 20);
            this.button14.TabIndex = 24;
            this.button14.Text = "EMV";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(32, 36);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(89, 20);
            this.button13.TabIndex = 23;
            this.button13.Text = "카드리딩";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.EDT_EVentHext);
            this.groupBox3.Controls.Add(this.EDT_EVENT);
            this.groupBox3.Controls.Add(this.EDT_RCD);
            this.groupBox3.Controls.Add(this.EDT_JCD);
            this.groupBox3.Controls.Add(this.EDT_GCD);
            this.groupBox3.Controls.Add(this.EDT_CMDF);
            this.groupBox3.Location = new System.Drawing.Point(14, 75);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(869, 79);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Event";
            // 
            // EDT_EVentHext
            // 
            this.EDT_EVentHext.Location = new System.Drawing.Point(154, 47);
            this.EDT_EVentHext.Name = "EDT_EVentHext";
            this.EDT_EVentHext.Size = new System.Drawing.Size(699, 21);
            this.EDT_EVentHext.TabIndex = 5;
            // 
            // EDT_EVENT
            // 
            this.EDT_EVENT.Location = new System.Drawing.Point(154, 20);
            this.EDT_EVENT.Name = "EDT_EVENT";
            this.EDT_EVENT.Size = new System.Drawing.Size(699, 21);
            this.EDT_EVENT.TabIndex = 4;
            // 
            // EDT_RCD
            // 
            this.EDT_RCD.Location = new System.Drawing.Point(120, 20);
            this.EDT_RCD.Name = "EDT_RCD";
            this.EDT_RCD.Size = new System.Drawing.Size(28, 21);
            this.EDT_RCD.TabIndex = 3;
            // 
            // EDT_JCD
            // 
            this.EDT_JCD.Location = new System.Drawing.Point(86, 20);
            this.EDT_JCD.Name = "EDT_JCD";
            this.EDT_JCD.Size = new System.Drawing.Size(28, 21);
            this.EDT_JCD.TabIndex = 2;
            // 
            // EDT_GCD
            // 
            this.EDT_GCD.Location = new System.Drawing.Point(52, 20);
            this.EDT_GCD.Name = "EDT_GCD";
            this.EDT_GCD.Size = new System.Drawing.Size(28, 21);
            this.EDT_GCD.TabIndex = 1;
            // 
            // EDT_CMDF
            // 
            this.EDT_CMDF.Location = new System.Drawing.Point(18, 20);
            this.EDT_CMDF.Name = "EDT_CMDF";
            this.EDT_CMDF.Size = new System.Drawing.Size(28, 21);
            this.EDT_CMDF.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 511);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.TextBox edt_emv;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox EDT_EVentHext;
        private System.Windows.Forms.TextBox EDT_EVENT;
        private System.Windows.Forms.TextBox EDT_RCD;
        private System.Windows.Forms.TextBox EDT_JCD;
        private System.Windows.Forms.TextBox EDT_GCD;
        private System.Windows.Forms.TextBox EDT_CMDF;
    }
}

