using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MLM_Program
{
    public partial class Form1 : Form
    {
        [DllImport("KiccPos.dll", EntryPoint = "KLoad", CharSet = CharSet.Ansi)]
        private static extern int KLoad(int pPort, int pBaud, byte[] pErrMsg);

        [DllImport("KiccPos.dll", EntryPoint = "KUnLoad", CharSet = CharSet.Ansi)]
        private static extern void KUnLoad();

        [DllImport("KiccPos.dll", EntryPoint = "KReqReset", CharSet = CharSet.Ansi)]
        private static extern int KReqReset();

        [DllImport("KiccPos.dll", EntryPoint = "KReqSign", CharSet = CharSet.Ansi)]
        private static extern int KReqSign(String TID, int Amount, int pX, int pY,
            String TopMsg, String CurrCD, String DispMsg, byte[] ErrMsg);

        [DllImport("KiccPos.dll", EntryPoint = "KSaveToBmp", CharSet = CharSet.Ansi)]
        private static extern int KSaveToBmp(String FName, int BmpType, byte[] ErrMsg);

        [DllImport("KiccPos.dll", EntryPoint = "KReqCmd", CharSet = CharSet.Ansi)]
        private static extern int KReqCmd(int CMD, int GCD, int JCD, String SendData, byte[] ErrMsg);

        [DllImport("KiccPos.dll", EntryPoint = "KWaitCmd", CharSet = CharSet.Ansi)]
        private static extern int KWaitCmd(int CMD, byte[] RcvData, int WaitTime, int WaitType, String DispMsg, byte[] ErrMsg);

        [DllImport("KiccPos.dll", EntryPoint = "KDownShopInfo", CharSet = CharSet.Ansi)]
        private static extern int KDownShopInfo(String Busino, String Areano, String TID, String AgentCd,
            String Telno, int WaitType, byte[] ErrMsg, String KiccIP, int KiccPort);

        [DllImport("KiccPos.dll", EntryPoint = "KGetSign", CharSet = CharSet.Ansi)]
        private static extern int KGetSign(byte[] Sign);

        [DllImport("KiccPos.dll", EntryPoint = "KApproval", CharSet = CharSet.Ansi)]
        private static extern int KApproval(int ReqType, String ReqMsg, int ReqMsgLen,
            String Sign, String Emv, int ResType, byte[] ResMsg, byte[] ErrMsg,
            String KiccIP, int KiccPort, int Secure, String RID, String trno);

        [DllImport("KiccPos.dll", EntryPoint = "KGetTRNO", CharSet = CharSet.Ansi)]
        private static extern int KGetTRNO(String RID, byte[] trno);

        [DllImport("KiccPos.dll", EntryPoint = "KRollBack", CharSet = CharSet.Ansi)]
        private static extern int KRollBack(byte[] ErrMsg, String KiccIP, int KiccPort, int Secure, String RID);
        

        [DllImport("KiccPos.dll", EntryPoint = "KGetCardNo", CharSet = CharSet.Ansi)]
        private static extern int KGetCardNo(byte[] RCARD);

        [DllImport("KiccPos.dll", EntryPoint = "KGetCashNo", CharSet = CharSet.Ansi)]
        private static extern int KGetCashNo(byte[] RCASH);
        
        [DllImport("KiccPos.dll", EntryPoint = "KGetEvent", CharSet = CharSet.Ansi)]
        private static extern int KGetEvent(ref int CMD, ref int GCD, ref int JCD, ref int RCD, byte[] RData, byte[] RHexData);

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] err = new byte[4096];
            int ret = 0;

            ret = KLoad(int.Parse(textBox1.Text), int.Parse(textBox2.Text), err);

            if (ret >= 0)
            {
                MessageBox.Show("OPEN");
                timer1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            KUnLoad();
            timer1.Enabled = false;
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            int ret = 0;
            
            ret = KReqReset();
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] err = new byte[4096];
            int ret = 0;

            ret = KReqSign("0700081", 1000, 100, 100, "", "", "", err);

            if (ret >= 0)
            {
                ret = KSaveToBmp(".\\Sign.bmp", 0, err);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            byte[] err = new byte[4096];
            byte[] pCash = new byte[20];
            int ret = 0;

            ret = KReqCmd(Convert.ToInt32("C3", 16), 0, 0, "", err);

            ret = KWaitCmd(Convert.ToInt32("C5", 16), pCash, 0, 2, "현금 영수증 번호를 입력해 주세요", err);

            if (ret > 0)
                MessageBox.Show(System.Text.Encoding.Default.GetString(pCash));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            byte[] err = new byte[4096];
            int ret = 0;

            ret = KDownShopInfo("2208889188", "02", "5182379", "5326", "12345678", 0, err, "203.233.72.21", 4110);
           // ret = KDownShopInfo("2208889188", "02", "5182382", "5326", "12345678", 0, err, "203.233.72.21", 4110);
            //ret = KDownShopInfo("2208889188", "02", "5182384", "5326", "12345678", 0, err, "203.233.72.21", 4110);


            if (ret >= 0)
                MessageBox.Show("Success");
            else
                MessageBox.Show("Fail");

        }

        private void button7_Click(object sender, EventArgs e)
        {
            byte[] err = new byte[4096];
            byte[] pAnsData = new byte[4096];
            byte[] Sign = new byte[4096];
            int ret = 0;

            KGetSign(Sign);

            ret = KApproval(3, textBox5.Text, textBox5.Text.Length, System.Text.Encoding.Default.GetString(Sign), "", 4, pAnsData, err, textBox4.Text, int.Parse(textBox3.Text), 0, "KICC", "0000001");

            if (ret >= 0)
                textBox6.Text = System.Text.Encoding.Default.GetString(pAnsData);
            else
            {
                textBox6.Text = "";
                MessageBox.Show(System.Text.Encoding.Default.GetString(err));
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            byte[] trno = new byte[100];
            int ret = 0;

            ret = KGetTRNO("KICC", trno);

            if (ret >= 0)
                MessageBox.Show(System.Text.Encoding.Default.GetString(trno));
            else
                MessageBox.Show("저장된 값이 없습니다.");

        }

        private void button9_Click(object sender, EventArgs e)
        {
            byte[] err = new byte[4096];
            int ret = 0;

            ret = KRollBack(err, textBox4.Text, int.Parse(textBox3.Text), 0, "KICC");
            if(ret >= 0)
                MessageBox.Show(Convert.ToString(ret));
            else
                MessageBox.Show(System.Text.Encoding.Default.GetString(err));

        }

        private void button10_Click(object sender, EventArgs e)
        {

            byte[] err = new byte[4096];
            byte[] rData = new byte[40];
            byte[] rCard = new byte[40];
            int ret = 0;

            ret = KReqCmd(Convert.ToInt32("FD", 16), 0, 0, "INM", err);

            ret = KWaitCmd(Convert.ToInt32("FD", 16), rData, 0, 1, "카드를 읽어주세요.", err);

            if (ret > 0)
            {
                KGetCardNo(rCard);
                MessageBox.Show(System.Text.Encoding.Default.GetString(rCard));
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            byte[] err = new byte[4096];
            byte[] rData = new byte[40];
            byte[] rCard = new byte[40];
            int ret = 0;

            ret = KReqCmd(Convert.ToInt32("FD", 16), 0, 0, "INMS1", err);

            ret = KWaitCmd(Convert.ToInt32("FD", 16), rData, 0, 1, "카드를 읽어주세요.", err);

            if (ret > 0)
            {
                KGetCardNo(rCard);
                MessageBox.Show(System.Text.Encoding.Default.GetString(rCard));
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {

//            byte[] err = new byte[4096];
//            int ret = 0;
//
//            ret = KReqCmd(Convert.ToInt32("FD", 16), 0, 0, textBox7.Text, err);

        	  byte[] err = new byte[4096];
            byte[] rData = new byte[40];
            byte[] rCard = new byte[40];
            int ret = 0;

            ret = KReqCmd(Convert.ToInt32("CD", 16), 0, 0, "INMS1", err);

            ret = KWaitCmd(Convert.ToInt32("CD", 16), rData, 0, 1, "카드를 읽어주세요.", err);

            if (ret > 0)
            {
                KGetCardNo(rCard);
                MessageBox.Show(System.Text.Encoding.Default.GetString(rCard));
            }
        
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int CMD = 0;
            int GCD = 0;
            int JCD = 0;
            int RCD = 0;
            byte[] RData = new byte[4096];
            byte[] RHexData = new byte[4096];

            int Ret = 0;

            // KWaitCmd 명령(Blocking 방식) 을 사용하지 않고 단말기로 부터 수신되는 값을
            // 채크(Non Blocking 방식)하기 위해 아래 함수를 주기적으로 호출하여 확인을 할 수 있다
            // 마지막 수신된 Data 값을 리턴 받을 수 있다.

            Ret = KGetEvent(ref CMD, ref GCD, ref JCD, ref RCD, RData, RHexData);

            if (Ret >= 0)
            {
                EDT_CMDF.Text = CMD.ToString();
                EDT_GCD.Text = GCD.ToString();
                EDT_JCD.Text = JCD.ToString();
                EDT_RCD.Text = RCD.ToString();
                EDT_EVENT.Text = System.Text.Encoding.Default.GetString(RData);
                EDT_EVentHext.Text = System.Text.Encoding.Default.GetString(RHexData);
            }

        }

        private void button13_Click(object sender, EventArgs e)
        {
            byte[] err = new byte[4096];
            byte[] rData = new byte[40];
            byte[] rCard = new byte[40];
            int ret = 0;

            ret = KReqCmd(Convert.ToInt32("F0", 16), 0, 0, "202", err);

            ret = KWaitCmd(Convert.ToInt32("F0", 16), rData, 0, 1, "카드를 읽어주세요.", err);


            if (ret > 0)
                edt_emv.Text = System.Text.Encoding.Default.GetString(rData);

        }

        private void button14_Click(object sender, EventArgs e)
        {
            byte[] err = new byte[4096];
            byte[] rData = new byte[40];
            int ret = 0;

            ret = KReqCmd(Convert.ToInt32("EC", 16), 0, 0, "002013090919000000000100400700081", err);

            ret = KWaitCmd(Convert.ToInt32("EC", 16), rData, 0, 1, "카드를 읽어주세요.", err);


            if (ret > 0)
                //MessageBox.Show(System.Text.Encoding.Default.GetString(rData));
                edt_emv.Text = System.Text.Encoding.Default.GetString(rData);

        }

        private void button15_Click(object sender, EventArgs e)
        {
            byte[] rCard = new byte[40];

            KGetCardNo(rCard);
            MessageBox.Show(System.Text.Encoding.Default.GetString(rCard));
        }

    }
}
