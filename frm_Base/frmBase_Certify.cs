﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MLM_Program
{
    public partial class frmBase_Certify : clsForm_Extends
    {
        public delegate void SendCertifyDele(string SuccessYN, string Message, string Name, string DI, string CI, string BirthDay, string Gender, string NationalInfo, string Age, string VNumber, string AgeCode, string AuthInfo, string RegTime);
        public event SendCertifyDele Send_Certify_Info;

        public delegate void Call_Certify_Info_Dele(ref string mode);
        public event Call_Certify_Info_Dele Call_Certify_Info;

        cls_Grid_Base cgb = new cls_Grid_Base();
        private const string base_db_name = "tbl_certify_IF";
        private int FormLoad_TF = 0;
        private int Data_Set_Form_TF = 0;
        private int Data_Set_Form_TF2 = 0;

        private string Certify_Mode;

        private string t_SuccessYN;
        private string t_Message;
        private string t_AuthType;
        private string t_Name;
        private string t_DI;
        private string t_CI;
        private string t_BirthDate;
        private string t_Gender;
        private string t_NationalInfo;
        private string t_Age;
        private string t_Vnumber;
        private string t_AgeCode;
        private string t_AuthInfo;

        public frmBase_Certify()
        {
            InitializeComponent();
        }


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Certify_Mode = "";
            Call_Certify_Info(ref Certify_Mode);

            Data_Set_Form_TF = 0;
            Data_Set_Form_TF2 = 0;
            t_SuccessYN = ""; t_Message = ""; t_SuccessYN = ""; t_AuthType = "";
            t_Name = ""; t_DI = ""; t_CI = ""; t_BirthDate = ""; t_Gender = "";
            t_NationalInfo = ""; t_Age = ""; t_Vnumber = ""; t_AgeCode = ""; t_AuthInfo = "";

            FormLoad_TF = 1;
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
            FormLoad_TF = 0;

            string strUrl = cls_app_static_var.AuthURL;

            string strPostData = string.Format("?authType={0}", Certify_Mode);
            //byte[] postData = Encoding.Default.GetBytes(strPostData);
            strUrl += strPostData;
            webBrowser1.Navigate(strUrl);//, null, postData, "Content-Type: application/x-www-form-urlencoded");

            timer1.Enabled = true;
        }


        private void frmBase_From_KeyDown(object sender, KeyEventArgs e)
        {
            //폼일 경우에는 ESC버튼에 폼이 종료 되도록 한다
            if (sender is Form)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (webBrowser1.DocumentText.Contains("successYN") == true)
            {
                timer1.Enabled = false;
                string Getstring = "";

                Getstring = webBrowser1.DocumentText.ToString();
                JObject ReturnData = JObject.Parse(Getstring.Replace("\r", "").Replace("\n", ""));

                t_SuccessYN = ""; t_Message = ""; t_AuthType = ""; t_Name = ""; t_DI = ""; t_CI = "";
                t_BirthDate = ""; t_Gender = ""; t_NationalInfo = ""; t_Age = ""; t_Vnumber = "";
                t_AgeCode = ""; t_AuthInfo = "";

                t_SuccessYN = ReturnData["successYN"].ToString();

                if (t_SuccessYN == "Y")
                {
                    t_AuthType = ReturnData["authType"].ToString();
                    t_Name = ReturnData["name"].ToString();
                    t_DI = ReturnData["di"].ToString();
                    t_CI = ReturnData["ci"].ToString();
                    t_BirthDate = ReturnData["birthDate"].ToString();
                    t_Gender = ReturnData["gender"].ToString();
                    t_NationalInfo = ReturnData["nationalInfo"].ToString();
                    t_Age = ReturnData["age"].ToString();

                    //t_Vnumber = ReturnData["vnumber"].ToString();
                    //t_AgeCode = ReturnData["ageCode"].ToString();
                    //t_AuthInfo = ReturnData["authInfo"].ToString();                    
                }
                else
                {
                    t_Message = ReturnData["message"].ToString();
                }




                Send_Certify_Info(t_SuccessYN, t_Message, t_Name, t_DI, t_CI, t_BirthDate, t_Gender, t_NationalInfo, t_Age, t_Vnumber, t_AgeCode, t_AuthInfo, t_AuthType);
                this.Close();
            }

        }

    }
}
