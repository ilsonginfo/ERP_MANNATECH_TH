using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLM_Program
{
    public partial class frmBase_AV_Accnt : Form
    {
        



        
        public delegate void SendAddressDele(string AddCode1, string AddCode2, string Address1, string Address2, string Address3);
        public event SendAddressDele Send_Address_Info;

        public delegate void Call_searchNumber_Info_Dele(ref string searchOrd, ref int searchPay, ref string searchTemp);
        public event Call_searchNumber_Info_Dele Call_searchNumber_Info;

        cls_Grid_Base cgb = new cls_Grid_Base();
        private const string base_db_name = "tbl_zipcode";
        private string t_AddCode1;
        private string t_AddCode2;
        private int FormLoad_TF = 0;
        private int Data_Set_Form_TF = 0;
        private int Data_Set_Form_TF2 = 0;

        private string Search_Ordernumber;
        private int Search_Send_Pay;
        private string Search_Temp;

        public frmBase_AV_Accnt()
        {
            InitializeComponent();
        }

        
        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Search_Ordernumber = "";
            Search_Send_Pay = 0;
            Search_Temp = "";

            Call_searchNumber_Info(ref Search_Ordernumber, ref Search_Send_Pay, ref Search_Temp);


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string TSql = "Delete From TLS_V_ACCOUNTINFO  ";            
            TSql = TSql + " Where  ID ='" + cls_User.gid + "' ";
            //TSql = TSql + " And    ORDERNUMBER ='" + Search_Ordernumber + "' ";

            Temp_Connect.Insert_Data(TSql, base_db_name, this.Name.ToString(), this.Text); 
                        
            Data_Set_Form_TF = 0;
            Data_Set_Form_TF2 = 0; 
            t_AddCode1 = "";   t_AddCode2 = "";
            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb.d_Grid_view_Header_Reset();
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            FormLoad_TF = 1;
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);            
            FormLoad_TF = 0;

            string t_url = "http://www.apyld.com/vaccount.do?orderNumber=" + Search_Ordernumber + "&id=" + cls_User.gid + "&amount=" + Search_Send_Pay ;

            webBrowser1.Navigate(t_url);

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
                }// end if

            }            
        }
             
   


        private void timer1_Tick(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select TRANSACTION_ID,ACCOUNT_NUMBER, BANK_CD From TLS_V_ACCOUNTINFO (nolock) ";
            Tsql = Tsql + " Where  ID ='" + cls_User.gid + "' ";
            Tsql = Tsql + " And    ORDERNUMBER ='" + Search_Ordernumber + "' ";

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set (Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {
                timer1.Enabled = false;

                t_AddCode1 = ds.Tables[base_db_name].Rows[0]["TRANSACTION_ID"].ToString();
                t_AddCode2 = ds.Tables[base_db_name].Rows[0]["ACCOUNT_NUMBER"].ToString();
                string BANK_CD = ds.Tables[base_db_name].Rows[0]["BANK_CD"].ToString();

                Send_Address_Info(t_AddCode1, t_AddCode2, BANK_CD, "", "");
                this.Close();
                return;
            }
        }

        















    }
}
