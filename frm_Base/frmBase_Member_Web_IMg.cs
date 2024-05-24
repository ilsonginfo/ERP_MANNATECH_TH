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
    public partial class frmBase_Member_Web_IMg : clsForm_Extends
    {
       

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        cls_Grid_Base cgb = new cls_Grid_Base();
        private const string base_db_name = "tbl_Memberinfo";

        //더블 클릭한 내역을 호출한 폼으로 다시 돌려 보내기 위한 델리케이트
        //public delegate void SendNumberDele(string Send_Number, string Send_Name);
        //public event SendNumberDele Send_Mem_Number;


        //public delegate void Send_Search_Mem_Number_Info_Dele(ref string searchMbid, ref int searchMbid2);
        //public event Send_Search_Mem_Number_Info_Dele Send_MemNumber_Info;

        public delegate void Call_searchNumber_Info_Dele(ref string searchMbid, ref int searchMbid2, ref string searchName);
        public event Call_searchNumber_Info_Dele Call_searchNumber_Info;

                

        private string Search_Member_Number_Mbid;
        private int Search_Member_Number_Mbid2;
        private string Search_Member_Name;
        private string Search_Member_Name_KR;


        public frmBase_Member_Web_IMg()
        {
            InitializeComponent();
        }



        private void frmBase_From_Load(object sender, EventArgs e)
        {

            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Search);


            Search_Member_Number_Mbid = ""; Search_Member_Number_Mbid2 = 0;
            Search_Member_Name = ""; Search_Member_Name_KR = "";

            Call_searchNumber_Info(ref Search_Member_Number_Mbid, ref Search_Member_Number_Mbid2, ref Search_Member_Name);


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string StrSql = ""; 
            //if (Search_Member_Name == "")
            //    StrSql = "select UPLOAD_PATH + UPLOAD_FILE_NM from TLS_FILE with (nolock)";
            //else
                StrSql = "select UPLOAD_PATH + UPLOAD_FILE_NM  as T_FileDir from TLS_FILE with (nolock) ";
            StrSql = StrSql + " where  GUBUN_1 = 'MEMBER' ";
            StrSql = StrSql + " and GUBUN_2 = 'IDCARD' ";     // BANKBOOK

            //StrSql = StrSql + "    AND REG_ID = (SELECT WEBID FROM TBL_MEMBERINFO WHERE MBID2 = " + Search_Member_Number_Mbid2 + ") ORDER BY REG_DATE DESC";
            StrSql = StrSql + "    AND ORG_SEQ = " + Search_Member_Number_Mbid2 + " ORDER BY REG_DATE DESC";
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Memberinfo", ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)
            {
                this.Close(); 
                return;
            }
            //++++++++++++++++++++++++++++++++                        

            //string Cpno = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Cpno"].ToString());
            
                        
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
            string T_FileDir = ds.Tables[base_db_name].Rows[0]["T_FileDir"].ToString();

            //string t_url = "https://www.applicant.im/uImage" + T_FileDir ;
            string t_url = "https://uat.mannatech.co.th/uImage" + T_FileDir;    // uat 버전. 
            //string t_url = "https://www.mannatech.co.th/uImage" + T_FileDir;    // live 버전. 


            webBrowser1.Navigate(t_url);

            
            
                                   
        }











    }
}
