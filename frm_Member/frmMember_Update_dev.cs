﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Security.Cryptography;
using DevExpress.XtraEditors.Repository;
//20190225구현호 - 클래스종속을 위해 MLM_Program뒤에 폼이름을 삭제




namespace MLM_Program
{
    public partial class frmMember_Update_dev : DevExpress.XtraEditors.XtraForm
    {
        Class.DevGridControlService cgb = new Class.DevGridControlService();
        Class.DevGridControlService cgb_Item = new Class.DevGridControlService();
        Class.DevGridControlService cgb_Cacu = new Class.DevGridControlService();
        Class.DevGridControlService cgb_Rece = new Class.DevGridControlService();
        Class.DevGridControlService cgb_save = new Class.DevGridControlService();
        Class.DevGridControlService cgb_nomin = new Class.DevGridControlService();
        Class.DevGridControlService cgb_Talk = new Class.DevGridControlService();
        Class.DevGridControlService cgb_add = new Class.DevGridControlService();
        Class.DevGridControlService cgb_pay = new Class.DevGridControlService();
        Class.DevGridControlService cgb_memupc = new Class.DevGridControlService();
        Class.DevGridControlService cgb_Change = new Class.DevGridControlService();
        Class.DevGridControlService cgb_Re_Pay = new Class.DevGridControlService();
        cls_Grid_Base cg_Up_S = new cls_Grid_Base();



        private Dictionary<int, cls_Sell_Item> SalesItemDetail = new Dictionary<int, cls_Sell_Item>();
        private Dictionary<int, cls_Sell_Rece> Sales_Rece = new Dictionary<int, cls_Sell_Rece>();
        private Dictionary<int, cls_Sell_Cacu> Sales_Cacu = new Dictionary<int, cls_Sell_Cacu>();
        

        public delegate void Take_NumberDele(ref string Send_Number, ref string Send_Name);
        public event Take_NumberDele Take_Mem_Number;

        private const string base_db_name = "tbl_Memberinfo";
        private int Data_Set_Form_TF;
        private string idx_Mbid = "", idx_Password = "";
        private int idx_Mbid2 = 0;

        public virtual string NullText { get; set; }

        //Series series_Item = new Series();



        public frmMember_Update_dev()
        {
            InitializeComponent();
        }




        private void frmMember_Update_dev_Load(object sender, EventArgs e)
        {
           
            InitCombo();
            Data_Set_Form_TF = 0;




            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb.d_Grid_view_Header_Reset();
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //dGridView_Line_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cg_Li.d_Grid_view_Header_Reset();
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            //구현호 20190225 데브에서는 mask 기능이 없다. mask는 MaskedTextBox 윈폼클래스 기능이다
            //mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            //mtxtSn.Mask = "999999-9999999"; //기본 셋팅은 주민번호이다. 
            //mtxtSn_C.Mask = "999999-9999999"; //기본 셋팅은 주민번호이다. 

            //mtxtTel1.Mask = cls_app_static_var.Tel_Number_Fromat;
            //mtxtTel2.Mask = cls_app_static_var.Tel_Number_Fromat;
            //mtxtZip1.Mask = cls_app_static_var.ZipCode_Number_Fromat;
            //mtxtZip2.Mask = cls_app_static_var.ZipCode_Number_Fromat;

            //mtxtBrithDay.Mask = cls_app_static_var.Date_Number_Fromat;
            //mtxtRegDate.Mask = cls_app_static_var.Date_Number_Fromat;
            //mtxtEdDate.Mask = cls_app_static_var.Date_Number_Fromat;

            //mtxtRBODate.Mask = cls_app_static_var.Date_Number_Fromat;
            //mtxtVisaDay.Mask = cls_app_static_var.Date_Number_Fromat;

            //mtxtBrithDayC.Mask = cls_app_static_var.Date_Number_Fromat;
            //mtxtTel2_C.Mask = cls_app_static_var.Tel_Number_Fromat;


            if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
            {
                //일단 오른쪽 텝컨트롤들이임 
                //tabC_Up.TabPages.Remove(tabP_S);
                //tabC_Up.TabPages.Remove(tabP_S_D);
                //tabC_Mem.TabPages.Remove(tab_Down_Save);
                //tbl_save.Visible = false;
            }

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                //일단 오른쪽 텝컨트롤들이임 
                //tabC_Up.TabPages.Remove(tabP_N);
                //tabC_Up.TabPages.Remove(tabP_N_D);
                //tabC_Mem.TabPages.Remove(tab_Down_Nom);

                //tbl_nom.Visible = false;
            }

            //txtMbid_n.BackColor = cls_app_static_var.txt_Enable_Color;
            //txtName_n.BackColor = cls_app_static_var.txt_Enable_Color;
            //txtSN_n.BackColor = cls_app_static_var.txt_Enable_Color;

            //txtMbid_s.BackColor = cls_app_static_var.txt_Enable_Color;
            //txtName_s.BackColor = cls_app_static_var.txt_Enable_Color;
            //txtSN_s.BackColor = cls_app_static_var.txt_Enable_Color;

            //txtLineCnt.BackColor = cls_app_static_var.txt_Enable_Color;
            mtxtSn.BackColor = cls_app_static_var.txt_Enable_Color;
            txtLeaveDate.BackColor = cls_app_static_var.txt_Enable_Color;
            //txtLineDate.BackColor = cls_app_static_var.txt_Enable_Color;
            //txtGrade.BackColor = cls_app_static_var.txt_Enable_Color;
            //txt_Us.BackColor = cls_app_static_var.txt_Enable_Color;
            //txtGradeP.BackColor = cls_app_static_var.txt_Enable_Color;


            // 일단 다 오른쪽탭들임

            //if (tab_Nation.Visible == true)
            //{
            //    cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            //    cpbf.Put_NaCode_ComboBox(combo_Se, combo_Se_Code);
            //}

            //tabC_Mem.SelectedIndex = 0;

            //cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
            //cgbp11.dGridView_Put_baseinfo(dGridView_Talk, "talk");


            ////tabC_Mem.TabPages.Remove(tab_Auto);
            ////tabC_Mem.TabPages.Remove(tab_Auto_Select);
            ////tabC_Mem.TabPages.Remove(tab_CC);
            //tabC_Mem.TabPages.Remove(tab_Hide);

            //if (cls_User.gid_CC_Save_TF == 0)  //공동신청인 권한이 없는 사람은 보이지 않게 한다.
            //    panel_CC.Enabled = false;
            //else
            //    panel_CC.Enabled = true;

            //radioB_RBO.Checked = true;
            //radioB_G8.Checked = true;

            mtxtMbid.Focus();
        }
      
        private bool Sn_Number_1(string Sn, TextBoxBase mtb, string sort_TF, int t_Sort2 = 0)
        {

            bool check_b = false;
            cls_Sn_Check csn_C = new cls_Sn_Check();

            //sort_TF = "biz";  //사업자번호체크
            //sort_TF = "Tel";  //전화번호체크
            //sort_TF = "Zip";  //우편번호체크

            if (sort_TF == "Date")
            {
                string[] date_a = mtb.Text.Split('-');

                if (date_a.Length >= 3 && date_a[0].Trim() != "" && date_a[1].Trim() != "" && date_a[2].Trim() != "")
                {
                    string Date_YYYY = "0000" + int.Parse(date_a[0]).ToString();

                    date_a[0] = Date_YYYY.Substring(Date_YYYY.Length - 4, 4);

                    if (int.Parse(date_a[1]) < 10)
                        date_a[1] = "0" + int.Parse(date_a[1]).ToString();

                    if (int.Parse(date_a[2]) < 10)
                        date_a[2] = "0" + int.Parse(date_a[2]).ToString();

                    mtb.Text = date_a[0] + '-' + date_a[1] + '-' + date_a[2];

                    cls_Check_Input_Error c_er = new cls_Check_Input_Error();
                    if (mtb.Text.Replace("-", "").Trim() != "")
                    {
                        int Ret = 0;
                        Ret = c_er.Input_Date_Err_Check((MaskedTextBox)mtb);

                        if (Ret == -1)
                        {
                            mtb.Focus(); return false;
                        }
                    }

                }
                else
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Date")
                       + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtb.Focus(); return false;
                }
            }


            check_b = csn_C.Number_NotInput_Check(mtb.Text, sort_TF);

            if (check_b == false)
            {
                if (sort_TF == "biz")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_BuNum")
                       + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }

                if (sort_TF == "Tel")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Tel")
                       + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                if (sort_TF == "HpTel")
                {
                    MessageBox.Show("휴대폰"
                       + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }

                /* 2018-08-22 지성경 막음 
                if (sort_TF == "Zip")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_AddCode")
                       + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }

                if (sort_TF == "Date")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Date")
                       + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }

                if (sort_TF == "Email")
                {
                    MessageBox.Show("메일주소가 입력되지않았습니다."
                       + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                */
                mtb.Focus(); return false;
            }

            return true;
        }
        private Boolean Check_TextBox_Error()
        {

            if (Input_Error_Check(mtxtMbid, "m") == false) return false; //회원번호 관련 관련 오류 체크




            cls_Check_Text T_R = new cls_Check_Text();
            string me = "";

            me = T_R.Text_Null_Check(txtName, "Msg_Sort_M_Name"); //성명을 필히 넣어야 합니다.
            if (me != "")
            {
                MessageBox.Show(me);
                return false;
            }

            if (mtxtRegDate.Text == "") //등록일자가 빈칸으로 되어 잇으면 당일을 셋팅한다.
                mtxtRegDate.Text = DateTime.Now.ToString("yyyy-MM-dd");




            string Sn = string.Empty;
            //Sn = mtxtTel1.Text.Replace("-", "").Replace("_", "").Trim();
            //if (Sn_Number_1(Sn, mtxtTel1, "Tel") == false)
            //{
            //    mtxtTel1.Focus();
            //    return false;
            //}
            mtxtTel3.Text = mtxtTel2.Text;// 테스트를 위해 winform 텍스트에 dev텍스트를 먼저 이동시킨다.
            Sn = mtxtTel2.Text.Replace("-", "").Replace("_", "").Trim();
            
            if (Sn_Number_1(Sn, mtxtTel3, "HpTel") == false)
            {
                mtxtTel2.Focus();
                return false;
            }

            mtxtZip3.Text = mtxtZip1.Text;// 테스트를 위해 winform 텍스트에 dev텍스트를 먼저 이동시킨다.
            Sn = mtxtZip1.Text.Replace("-", "").Replace("_", "").Trim();
            if (Sn_Number_1(Sn, mtxtZip3, "Zip") == false)
            {
                mtxtZip1.Focus();
                return false;
            }


            /* 2018-08-22 지성경 일단막자....
            if (txtAccount.Text == "")
            {
                me = "계좌번호를 필히 입력해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                txtAccount.Focus();
                return false;
            }

            if (txtName_Accnt.Text == "")
            {
                me = "예금주를 필히 입력해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                txtName_Accnt.Focus();
                return false;
            }


            if (txtBank_Code.Text == "")
            {
                me = "은행을 필히 선택해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                txtBank.Focus();
                return false;
            }
            */


            //날짜 관련 텍스트 파일들에 대해서 날짜 오류를 체크한다
           // if (Check_TextBox_Error_Date() == false) return false;

            return true;
        }
        private bool Check_TextBox_CC_Error()
        {
            cls_Check_Text T_R = new cls_Check_Text();
            string me = "";

            me = T_R.Text_Null_Check(txtName_C, "Msg_Sort_M_Name"); //성명을 필히 넣어야 합니다.
            if (me != "")
            {
                MessageBox.Show(me);
                txtName_C.Focus();
                return false;
            }

            /* 2018-08-05 지성경 현재 부부사업자는 주민번호 체크하지아니함 
            if (mtxtSn_C.Text.Replace("-", "") == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Error")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtSn_C.Focus(); return false;
            }
            
            string Sn = mtxtSn_C.Text.Replace("-", "").Replace("_", "").Trim();
            if (Sn_Number_(Sn, mtxtSn_C) == false) return false;   //주민번호 입력 사항에 대해서 체크를 한다.                     
            */

            cls_Check_Input_Error c_er = new cls_Check_Input_Error();

            //if (mtxtBrithDayC.Text.Replace("-", "").Trim() != "")
            //{
            //    if (Sn_Number_1(mtxtBrithDayC.Text, mtxtBrithDayC, "Date") == false)
            //    {
            //        mtxtBrithDayC.Focus();
            //        return false;
            //    }
            //}

            return true;
        }
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;


            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (txtB1.Text.Trim() == "") txtB1.Text = "0";
            if (Check_TextBox_Error() == false) return;

            if (check_CC.Checked == true)
                if (Check_TextBox_CC_Error() == false) return;  //오토쉽 등록 관련 오류를 체크한다.

            cls_Search_DB csd = new cls_Search_DB();

            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

            int S_RBO_Mem_TF = 0;
            string RBO_S_Date = "";
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string StrSql = "Select  Mbid, Mbid2 , RBO_Mem_TF , RBO_S_Date  ";
            StrSql = StrSql + " From tbl_Memberinfo  (nolock)  ";
            StrSql = StrSql + " Where mbid = '" + Mbid + "'";
            StrSql = StrSql + " And mbid2 = " + Mbid2.ToString();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(StrSql, "tbl_Memberinfo", ds);
            {
                if (Temp_Connect.DataSet_ReCount > 0)//오토쉽이 체크되어 잇는데 체크를 풀엇다. 그럼 삭제하라는 의미로 받아들인다.
                {
                    S_RBO_Mem_TF = int.Parse(ds.Tables["tbl_Memberinfo"].Rows[0]["RBO_Mem_TF"].ToString());
                    RBO_S_Date = ds.Tables["tbl_Memberinfo"].Rows[0]["RBO_S_Date"].ToString();

                    if (radioB_RBO.Checked == true && S_RBO_Mem_TF == 1 && mtxtRBODate.Text.Replace("-", "").Trim() == "")
                    {
                        MessageBox.Show("비긴즈에서 RBO 전환시에 날짜를 필히 입력 해야 합니다.."
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                        mtxtRBODate.Focus();
                        return;
                    }
                }
            }


            csd.Member_Mod_BackUp(mtxtMbid.Text.Trim(), "tbl_Memberinfo");

            cls_Search_DB csd_R = new cls_Search_DB();
            csd_R.Member_Mod_BackUp(mtxtMbid.Text.Trim(), "tbl_Memberinfo_Address", " And Sort_Add = 'R' ");




            //cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

            try
            {
                string hometel = ""; string hptel = "";
                string BirthDay = ""; string BirthDay_M = ""; string BirthDay_D = ""; int BirthDayTF = 0;
                string Sex_FLAG = "";
                string AgreeSMS = "N";
                string AgreeEmail = "N";
                int Sell_Mem_TF = 0; int Add_TF = 0, Myoffice_TF = 0, RBO_Mem_TF = 0, G8_TF = 0;
                int BankDocument = 0, CpnoDocument = 0;
                int For_Kind_TF = 0;

                if (check_BankDocument.Checked == true) BankDocument = 1;
                if (check_CpnoDocument.Checked == true) CpnoDocument = 1;

                if (mtxtTel1.Text.Replace("-", "").Trim() != "") hometel = mtxtTel1.Text;
                if (mtxtTel2.Text.Replace("-", "").Trim() != "") hptel = mtxtTel2.Text;

                if (opt_sell_3.Checked == true) Sell_Mem_TF = 1; //소비자는 1 판매원은 기본 0

                if (opt_Bir_TF_1.Checked == true) BirthDayTF = 1; //양력은 1  음력은 2
                if (opt_Bir_TF_2.Checked == true) BirthDayTF = 2;

                if (opt_B_1.Checked == true) Add_TF = 1;  //기본주소가 
                if (opt_B_2.Checked == true) Add_TF = 2; //회사 주소가
                if (opt_B_3.Checked == true) Add_TF = 3; //기본배송지 주소가


                if (radioB_RBO.Checked == true) RBO_Mem_TF = 0;// RBO 0 비긴즈 1
                if (radioB_Begin.Checked == true) RBO_Mem_TF = 1;

                if (radioB_G8.Checked == true) G8_TF = 8;// RBO 0 비긴즈 1
                if (radioB_G4.Checked == true) G8_TF = 4;

                if (check_MyOffice.Checked == true) Myoffice_TF = 1;

                if (mtxtBrithDay.Text.Replace("-", "").Trim() != "")
                {
                    string[] Sn_t = mtxtBrithDay.Text.Split('-');

                    BirthDay = Sn_t[0];  //생년월일을 년월일로 해서 쪼갠다
                    BirthDay_M = Sn_t[1]; //웹쪽 관련해서 이렇게 받아들이는데가 많아서
                    BirthDay_D = Sn_t[2]; //웹쪽 기준에 맞춘거임.
                }

                if (radioB_Sex_Y.Checked == true) Sex_FLAG = "Y";
                if (radioB_Sex_X.Checked == true) Sex_FLAG = "X";

                if (checkB_SMS_FLAG.Checked == true) AgreeSMS = "Y";
                if (checkB_EMail_FLAG.Checked == true) AgreeEmail = "Y";


                if (raButt_IN_2.Checked == true) For_Kind_TF = 1;// 내국인은 0 외국인은 1  사업자는 2
                if (raButt_IN_3.Checked == true) For_Kind_TF = 2;

                StrSql = "";
                StrSql = "Update tbl_Memberinfo Set ";

                StrSql = StrSql + " E_name = '" + txtName_E_1.Text.Trim() + "'";
                StrSql = StrSql + " ,E_name_Last = '" + txtName_E_2.Text.Trim() + "'";
                StrSql = StrSql + " ,Email = '" + txtEmail.Text.Trim() + "'";

                //StrSql = StrSql + " ,Email = '" + txtEmail.Text.Trim() + "'";
                StrSql = StrSql + " ,Ed_Date = '" + mtxtEdDate.Text.Replace("-", "").Trim() + "'";
                StrSql = StrSql + " ,Remarks = '" + txtRemark.Text.Trim() + "'";
                StrSql = StrSql + " ,Regtime = '" + mtxtRegDate.Text.Replace("-", "").Trim() + "'";

                StrSql = StrSql + " ,RBO_S_Date = '" + mtxtRBODate.Text.Replace("-", "").Trim() + "'";


                StrSql = StrSql + " ,VisaDate = '" + mtxtVisaDay.Text.Replace("-", "").Trim() + "'";

                StrSql = StrSql + " ,Addcode1 = '" + mtxtZip1.Text.Trim().Replace("-", "") + "'";
                StrSql = StrSql + " ,Address1 = '" + txtAddress1.Text.Trim() + "'";
                StrSql = StrSql + " ,Address2 = '" + txtAddress2.Text.Trim() + "'";
                StrSql = StrSql + " ,hometel = '" + hometel + "'";
                StrSql = StrSql + " ,hptel = '" + hptel + "'";

                StrSql = StrSql + " ,BirthDay = '" + BirthDay + "'";
                StrSql = StrSql + " ,BirthDay_M = '" + BirthDay_M + "'";
                StrSql = StrSql + " ,BirthDay_D = '" + BirthDay_D + "'";

                //20190306 구현호 선택된 룩업에디트의 텍스트를 잘라서 가져온다
                string banktext = txtBank.Properties.GetDisplayText(txtBank.EditValue);
                StrSql = StrSql + " ,BankCode = left( '" + banktext + "', 3 )";

                StrSql = StrSql + " ,bankowner = '" + txtName_Accnt.Text.Trim() + "'";
                StrSql = StrSql + " ,bankaccnt = dbo.ENCRYPT_AES256('" + txtAccount.Text.Trim() + "')";
                StrSql = StrSql + " ,Reg_bankaccnt = dbo.ENCRYPT_AES256('" + txtAccount_Reg.Text.Trim() + "')";

                string centertext = txtCenter.Properties.GetDisplayText(txtCenter.EditValue);
                StrSql = StrSql + ", BusinessCode  = left( '" + centertext + "', 3 )";

                StrSql = StrSql + " ,For_Kind_TF = " + For_Kind_TF;

                if (txtPassword.Text.Equals(idx_Password) == false)
                    StrSql = StrSql + " ,WebPassWord = '" + EncryptSHA256_EUCKR(txtPassword.Text.Trim()) + "'";

                if (check_CC.Checked == true)
                {
                    if (mtxtBrithDayC.Text.Replace("-", "").Trim() != "")
                    {
                        string[] Sn_t = mtxtBrithDayC.Text.Split('-');

                        BirthDay = Sn_t[0];  //생년월일을 년월일로 해서 쪼갠다
                        BirthDay_M = Sn_t[1]; //웹쪽 관련해서 이렇게 받아들이는데가 많아서
                        BirthDay_D = Sn_t[2]; //웹쪽 기준에 맞춘거임.
                    }

                    if (raButt_IN_2_C.Checked == true) For_Kind_TF = 1;// 내국인은 0 외국인은 1  사업자는 2

                    StrSql = StrSql + " ,C_M_Name = '" + txtName_C.Text.Trim() + "'";
                    StrSql = StrSql + " ,C_For_Kind_TF = " + For_Kind_TF;
                    //StrSql = StrSql + " ,C_cpno = '" + encrypter.Encrypt(mtxtSn_C.Text.Replace("-", "").Trim()) + "'";
                    //StrSql = StrSql + " ,C_E_name = '" + txtName_E_1_C.Text.Trim() + "'";
                    //StrSql = StrSql + " ,C_E_name_Last = '" + txtName_E_2_C.Text.Trim() + "'";
                    StrSql = StrSql + " ,C_BirthDay = '" + BirthDay + "'";
                    StrSql = StrSql + " ,C_BirthDay_M = '" + BirthDay_M + "'";
                    StrSql = StrSql + " ,C_BirthDay_D = '" + BirthDay_D + "'";
                    StrSql = StrSql + " ,C_hptel = '" + mtxtTel2_C.Text + "'";
                    StrSql = StrSql + " ,C_Email = '" + txtEmail_C.Text + "'";
                }
                else
                {
                    StrSql = StrSql + " ,C_M_Name = ''";
                    StrSql = StrSql + " , C_For_Kind_TF = 0 ";
                    //StrSql = StrSql + " ,C_cpno = ''";
                    //StrSql = StrSql + " ,C_E_name = ''";
                    //StrSql = StrSql + " ,C_E_name_Last = ''";
                    StrSql = StrSql + " ,C_BirthDay = '' ";
                    StrSql = StrSql + " ,C_BirthDay_M = '' ";
                    StrSql = StrSql + " ,C_BirthDay_D = '' ";
                    StrSql = StrSql + " ,C_hptel = '" + mtxtTel2_C.Text + "'";
                    StrSql = StrSql + " ,C_Email = '" + txtEmail_C.Text + "'";


                }

                StrSql = StrSql + " ,BirthDayTF = " + BirthDayTF.ToString();
                StrSql = StrSql + " ,Sell_Mem_TF = " + Sell_Mem_TF.ToString();

                StrSql = StrSql + " ,G8_TF = " + G8_TF.ToString();
                StrSql = StrSql + " ,RBO_Mem_TF = " + RBO_Mem_TF.ToString();

                StrSql = StrSql + " ,BankDocument = " + BankDocument.ToString();
                StrSql = StrSql + " ,CpnoDocument = " + CpnoDocument.ToString();

                StrSql = StrSql + " ,Add_TF = " + Add_TF.ToString();

                StrSql = StrSql + " ,Myoffice_TF = " + Myoffice_TF.ToString();
                StrSql = StrSql + " ,Sex_Flag = '" + Sex_FLAG + "'";
                StrSql = StrSql + " ,AgreeSMS = '" + AgreeSMS + "'";
                StrSql = StrSql + " ,AgreeEmail = '" + AgreeEmail + "'";


                //StrSql = StrSql + " ,GiBu_ = " + double.Parse (txtB1.Text.Trim ().ToString());

                if (Mbid.Length == 0)
                    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
                    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
                }

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);


                Chang_Mem_Address_R(Mbid, Mbid2, Temp_Connect, Conn, tran);



                tran.Commit();
                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));

                csd.tbl_Memberinfo_Mod(mtxtMbid.Text.Trim());
                csd_R.tbl_Memberinfo_Mod(mtxtMbid.Text.Trim(), "R", "tbl_Memberinfo_Address", " And Sort_Add = 'R' ");

            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Err"));

            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }

        }
        private void butt_AddCode_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void butt_AddCode_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            frmBase_AddCode e_f = new frmBase_AddCode();
            e_f.Send_Address_Info += new frmBase_AddCode.SendAddressDele(e_f_Send_Address_Info);
            e_f.ShowDialog();
        }
        private void e_f_Send_Address_Info(string AddCode1, string AddCode2, string Address1, string Address2, string Address3)
        {
            if (AddCode2 != string.Empty)
                mtxtZip1.Text = AddCode1 + "-" + AddCode2;
            else
                mtxtZip1.Text = AddCode1;
            txtAddress1.Text = Address1; txtAddress2.Text = Address2;
        }
        private void e_f_Send_Address_Info2(string AddCode1, string AddCode2, string Address1, string Address2, string Address3)
        {
            if (AddCode2 != string.Empty)
                mtxtZip2.Text = AddCode1 + "-" + AddCode2;
            else
                mtxtZip2.Text = AddCode1;
            txtAddress3.Text = Address1; txtAddress4.Text = Address2;

        }

        private void InitCombo()
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            // ** txtCenter 룩업에디트 세팅
            {
                Tsql = "Select  ncode + ' ' + name as 센터이름";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + combo_Se_Code.Text.Trim() + "') )";
                if (combo_Se_Code.Text.Trim() != "") Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                Tsql = Tsql + " And ncode <> '002'"; // 2018-11-23 지성경 에스제이로직스는 선택불가능하게끔한다.
            }
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            txtCenter.Properties.DataSource = ds.Tables["t_P_table"].Copy();
            txtCenter.Properties.ValueMember = "센터이름";
            txtCenter.Properties.DisplayMember = "센터이름";


            // ** txtBank **
            cls_Connect_DB Temp_g = new cls_Connect_DB();
            string sql = "";

            {
                sql = "Select  Ncode + ' ' + BankName as 은행이름  From tbl_Bank (nolock) ";
            }

            DataSet d = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_g.Open_Data_Set(sql, "t_table", d) == false) return;
            ReCnt = Temp_g.DataSet_ReCount;

            //RepositoryItemLookUpEdit riLookup2 = new RepositoryItemLookUpEdit();
            //riLookup2.DataSource = ds.Tables["t_P_table"];
            //riLookup2.ValueMember = "Ncode";
            //riLookup2.DisplayMember = "BankName";

            txtBank.Properties.DataSource = d.Tables["t_table"].Copy();
            txtBank.Properties.ValueMember = "은행이름";
            txtBank.Properties.DisplayMember = "은행이름";



            // ** txt_C_Card ** 우측 텝창이라 아직
            //cls_Connect_DB Temp_card = new cls_Connect_DB();
            //string cardsql = "";

            //{
            //    cardsql = "select Ncode , ncode + ' ' + cardName as cardname from tbl_Card (nolock)";
            //}

            //DataSet card = new DataSet();
            ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //if (Temp_card.Open_Data_Set(cardsql, "t_Card_table", card) == false) return;
            //ReCnt = Temp_card.DataSet_ReCount;

            //txt_C_Card.Properties.DataSource = card.Tables["t_Card_table"].Copy();
            //txt_C_Card.Properties.ValueMember = "Ncode";
            //txt_C_Card.Properties.DisplayMember = "cardname";

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (idx_Mbid2 <= 0 || idx_Mbid2 == null)
                return;
            if (mtxtBrithDay.Text.Replace("-", "").Length != 10)
            {
                MessageBox.Show("생년월일을 확인해주십시오.");
                mtxtBrithDay.Focus();
                return;
            }

            txtPassword.Text = "anew" + mtxtBrithDay.Text.Replace("-", "").Trim().Substring(2, 6);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Update Tbl_Memberinfo");
            sb.AppendLine("SET WebPassword = '" + EncryptSHA256_EUCKR(txtPassword.Text.Trim()) + "'");
            sb.AppendLine("WHERE mbid2 = '" + idx_Mbid2 + "'");

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();

            Temp_Connect.Update_Data(sb.ToString(), this.Name, this.Text);
            MessageBox.Show("비밀번호가 정상적으로 변경 되었습니다.");
        }
        private string EncryptSHA256_EUCKR(string phrase)
        {
            /*
            SHA256 sha = new SHA256Managed();

            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(phrase));

            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }

            return stringBuilder.ToString();
            */
            if (string.IsNullOrEmpty(phrase) == true)
            {
                return "";
            }
            else
            {
                Encoding encoding = Encoding.Unicode;

                SHA256 sha = new SHA256Managed();
                byte[] data = sha.ComputeHash(encoding.GetBytes(phrase));

                StringBuilder sb = new StringBuilder();
                foreach (byte b in data)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private Boolean Input_Error_Check(Control m_tb, string s_Kind)
        {
            string T_Mbid = m_tb.Text;
            string Mbid = ""; int Mbid2 = 0;
            //if (s_Kind == "s")
            //{
            //    txtName_s.Text = ""; txtSN_s.Text = "";
            //}
            //if (s_Kind == "n")
            //{
            //    txtName_n.Text = ""; txtSN_n.Text = "";
            //}

            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == -1) //올바르게 회원번호 양식에 맞춰서 입력햇는가.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }

            string Tsql = "";
            Tsql = "Select Mbid , Mbid2, M_Name , Sell_Mem_TF , RBO_Mem_TF ";
            Tsql = Tsql + " , LineCnt , N_LineCnt  ";
            Tsql = Tsql + " , LeaveDate , LineUserDate  ";
            Tsql = Tsql + " , Saveid  , Saveid2  ";
            Tsql = Tsql + " , Nominid , Nominid2  ";
            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }

            //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            if (s_Kind != "s" && s_Kind != "n")
            {
                Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
                Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";
            }
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)  //실제로 존재하는 회원 번호 인가.
            {

                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }
            //++++++++++++++++++++++++++++++++   


            if (s_Kind == "n" || s_Kind == "s") //3인 경우는 새로운 지정 후원인인데.. 탈퇴나 라인중자가 아닌지를 체크한다.
            {
                if (ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString() != "")
                {

                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Leave_")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    m_tb.Focus(); return false;
                }

                if (ds.Tables[base_db_name].Rows[0]["LineUserDate"].ToString() != "")
                {

                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Delete_")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    m_tb.Focus(); return false;
                }


                if (s_Kind == "s")
                {
                    //후원인은 소비자로 할수는 없다. 추천인은 가능할듯.
                    if (int.Parse(ds.Tables[base_db_name].Rows[0]["RBO_Mem_TF"].ToString()) == 1)
                    {

                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Sell_TF_0")
                                + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                               + "\n" +
                               cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        m_tb.Focus(); return false;
                    }

                    //하선인원수를 체크한다.
                    if (csb.Member_Down_Save_TF(m_tb.Text.Trim()) == false)
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Down_Full")
                               + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                              + "\n" +
                              cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        m_tb.Focus(); return false;
                    }


                    //if (chk_Top_s.Checked == false && chk_Top_n.Checked == false)
                    //{
                    //    //입력 추천인 하부의 후원조직상에  입력 후원인이 존재해야 한다.
                    //    if (csb.Member_Down_Save_TF(m_tb.Text.Trim(), mtxtMbid_n.Text.Trim()) == false)
                    //    {
                    //        string Msg = "";
                    //        Msg = "입력하신 추천인 하부 후원조직상에 " + "\n" + "입력하신 후원인이 존재 해야 합니다." + "\n" + " 계속 진행하시겠습니까?";

                    //        if (MessageBox.Show(Msg, "", MessageBoxButtons.YesNo) == DialogResult.No)
                    //        {
                    //            m_tb.Focus(); return false;
                    //        }
                    //        ////Msg_Mem_Down_Nom_Save
                    //        //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Down_Nom_Save")
                    //        //      + "\n" +                                 
                    //        //      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    //        //m_tb.Focus(); return false;

                    //    }
                    //}
                    //int LineCnt = csb.LineCnt_Search_Save(Mbid, Mbid2);
                    //txtLineCnt.Text = LineCnt.ToString();

                    //if (txtLineCnt.Text.Equals("2"))
                    //{
                    //    rdoLineRight.Checked = true;
                    //}
                    //else
                    //{
                    //    rdoLineLeft.Checked = true;
                    //}

                }
            }

            return true;
        }



        private void Chang_Mem_Address_R(string Mbid, int Mbid2, cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            // string ETC_Tel_1 = ""; string ETC_Tel_2 = "";
            string StrSql = "";

            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);


            StrSql = "Select Sort_Add , Mbid, Mbid2 ";
            StrSql = StrSql + " From tbl_Memberinfo_Address  (nolock)  ";
            StrSql = StrSql + " Where mbid = '" + Mbid + "'";
            StrSql = StrSql + " And mbid2 = " + Mbid2.ToString();
            StrSql = StrSql + " And Sort_Add = 'R' ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Memberinfo_Address", ds) == true)
            {
                //if (txtTel_R_1.Text != "") ETC_Tel_1 = txtTel_R_1.Text.Trim() + '-' + txtTel_R_2.Text.Trim() + '-' + txtTel_R_3.Text.Trim();
                //if (txtTel2_R_1.Text != "") ETC_Tel_2 = txtTel2_R_1.Text.Trim() + '-' + txtTel2_R_2.Text.Trim() + '-' + txtTel2_R_3.Text.Trim();

                if (Temp_Connect.DataSet_ReCount == 0)//동일한 이름으로 코드가 있다 그럼.이거 저장하면 안되요
                {


                    StrSql = "Insert into tbl_Memberinfo_Address ( ";
                    StrSql = StrSql + " Sort_Add ";
                    StrSql = StrSql + " ,Mbid ";
                    StrSql = StrSql + " ,Mbid2 ";
                    StrSql = StrSql + " ,ETC_Addcode1 ";
                    StrSql = StrSql + " ,ETC_Address1 ";
                    StrSql = StrSql + " ,ETC_Address2 ";
                    StrSql = StrSql + " ,ETC_Address3 ";
                    StrSql = StrSql + " ,ETC_Tel_1 ";
                    StrSql = StrSql + " ,ETC_Tel_2 ";
                    StrSql = StrSql + " ,ETC_Name ";
                    StrSql = StrSql + " ,Recordid ";
                    StrSql = StrSql + " ,Recordtime ";
                    StrSql = StrSql + " ) ";
                    StrSql = StrSql + " Values ( ";

                    StrSql = StrSql + " 'R' ";
                    StrSql = StrSql + ",'" + Mbid + "'";
                    StrSql = StrSql + "," + Mbid2.ToString();
                    StrSql = StrSql + ", '" + mtxtZip2.Text.Trim().Replace("-", "") + "'";
                    StrSql = StrSql + ", '" + encrypter.Encrypt(txtAddress3.Text.Trim()) + "'";
                    StrSql = StrSql + ", '" + encrypter.Encrypt(txtAddress4.Text.Trim()) + "'";
                    StrSql = StrSql + ", '' ";

                    StrSql = StrSql + ", '' ";
                    StrSql = StrSql + ", '' ";
                    StrSql = StrSql + ", '' ";
                    //StrSql = StrSql + ", '" + encrypter.Encrypt(ETC_Tel_1) + "'";
                    //StrSql = StrSql + ", '" + encrypter.Encrypt(ETC_Tel_2) + "'";
                    //StrSql = StrSql + ", '" + encrypter.Encrypt(txtName_R.Text.Trim()) + "'";
                    StrSql = StrSql + ",'" + cls_User.gid + "'";
                    StrSql = StrSql + ", Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " ) ";

                    Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_Address", Conn, tran);
                }
                else
                {
                    StrSql = "Update tbl_Memberinfo_Address Set ";
                    StrSql = StrSql + "  ETC_Addcode1 = '" + mtxtZip2.Text.Trim().Replace("-", "") + "'";
                    StrSql = StrSql + " ,ETC_Address1 = '" + encrypter.Encrypt(txtAddress3.Text.Trim()) + "'";
                    StrSql = StrSql + " ,ETC_Address2 = '" + encrypter.Encrypt(txtAddress4.Text.Trim()) + "'";
                    StrSql = StrSql + " ,ETC_Address3 = ''";
                    //StrSql = StrSql + " ,ETC_Tel_1 = '" + encrypter.Encrypt(ETC_Tel_1) + "'";
                    //StrSql = StrSql + " ,ETC_Tel_2 = '" + encrypter.Encrypt(ETC_Tel_2) + "'";
                    //StrSql = StrSql + " ,ETC_Name = '" + encrypter.Encrypt(txtName_R.Text.Trim()) + "'";
                    StrSql = StrSql + " Where mbid = '" + Mbid + "'";
                    StrSql = StrSql + " And mbid2 = " + Mbid2.ToString();
                    StrSql = StrSql + " And Sort_Add = 'R' ";

                    Temp_Connect.Update_Data(StrSql, Conn, tran);

                }
            }
        }

        private void dGridView_Up_S_Header_Reset(DataGridView t_Dgv)
        {
            cg_Up_S.Grid_Base_Arr_Clear();

            cg_Up_S.grid_col_Count = 5;
            cg_Up_S.basegrid = t_Dgv; //dGridView_Up_S;
            cg_Up_S.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_Up_S.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"회원_번호"  , "성명"   , "위치"  , ""   , ""
                                    };
            cg_Up_S.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 60, 70, 30, 0, 0
                            };
            cg_Up_S.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                   };
            cg_Up_S.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5      
                              };
            cg_Up_S.grid_col_alignment = g_Alignment;
            cg_Up_S.basegrid.RowHeadersWidth = 25;

            cg_Up_S.basegrid.ColumnHeadersDefaultCellStyle.Font =
            new Font(cg_Up_S.basegrid.Font.FontFamily, 8);
        }





        private void tabP_Sell_dGridView_Base_Header_Reset()
        {


            cgb.basegrid = dGridCtrl_Sell;
            cgb.baseview = dGridView_Sell;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 1;


            string[] g_HeaderText = {"승인여부" , "매출_일자" ,  "주문번호" ,  "주문_종류"   , "상태"
                                     , "매출액"  , "입급액"  ,"매출PV"  , "현금"  , "카드"
                                    , "무통장" , "비고"
                                    };


            cgb.grid_col_header_text = g_HeaderText;
            cgb.grid_col_Count = g_HeaderText.Length;
            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb.grid_cell_format = gr_dic_cell_format;


            int[] g_Width = { 80,100, 150, 80, 100
                                , 100,100 , 100 , 100 , 100
                                , 100,100
                            };

            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = {  true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                                {
                                DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter//5     

                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //10

                                ,DataGridViewContentAlignment.MiddleRight  //11
                                ,DataGridViewContentAlignment.MiddleCenter //12
                                };

            cgb.grid_col_alignment = g_Alignment;
            
        }

        private void tabP_info_dGridView_Base_Header_Reset()
        {
            cgb_Change.basegrid = dGridView_inf;
            cgb_Change.baseview = gridView7;
            cgb_Change.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Change.grid_Frozen_End_Count = 1;

            string[] g_HeaderText = {"변경일"  , "변경내역"   , "전_내역"  , "후_내역"   , "변경자"
                                    , ""   , ""    , ""  , "" , ""
                                    ,""
                                    };

            cgb_Change.grid_col_header_text = g_HeaderText;
            cgb_Change.grid_col_Count = g_HeaderText.Length;

            int[] g_Width = { 200, 120, 200, 200, 100
                                 ,0 , 0 , 0 , 0 , 0
                                 ,0
                                };
            cgb_Change.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = {  true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true
                                   };
            cgb_Change.grid_col_Lock = g_ReadOnly;


            DataGridViewContentAlignment[] g_Alignment =
                          {
                                    DataGridViewContentAlignment.MiddleLeft
                                   ,DataGridViewContentAlignment.MiddleLeft
                                   ,DataGridViewContentAlignment.MiddleCenter
                                   ,DataGridViewContentAlignment.MiddleCenter
                                   ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                   ,DataGridViewContentAlignment.MiddleCenter
                                   ,DataGridViewContentAlignment.MiddleCenter
                                   ,DataGridViewContentAlignment.MiddleCenter
                                   ,DataGridViewContentAlignment.MiddleCenter  //10
                                   ,DataGridViewContentAlignment.MiddleCenter  //10
                                   ,DataGridViewContentAlignment.MiddleCenter  //11
                                  };
            cgb_Change.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();

            cgb_Change.grid_cell_format = gr_dic_cell_format;
            //cgb_Change.grid_col_Count = g_HeaderText.Length;
            ////cgb_Change.grid_col_header_text = g_HeaderText;
            //cgb_Change.grid_col_w = g_Width;
            // cgb_Change.grid_col_alignment = g_Alignment;
        }

        private void tabP_memupc_dGridView_Base_Header_Reset()
        {

            cgb_memupc.basegrid = dGridView_Up;
            cgb_memupc.baseview = gridView8;
            cgb_memupc.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_memupc.grid_Frozen_End_Count = 1;

            string[] g_HeaderText = {"변경일"  , "전_상위번호"   , "전_상위성명"  , "후_상위번호"   , "후_상위성명"
                                , "구분"   , "변경자"    , ""  , "" , ""
                                };
            cgb_memupc.grid_col_header_text = g_HeaderText;
            cgb_memupc.grid_col_Count = g_HeaderText.Length;

            int[] g_Width = { 120, 100, 100, 100, 100
                                ,80 , 80 , 0 , 0 , 0
                            };

            cgb_memupc.grid_col_w = g_Width;
            Boolean[] g_ReadOnly = {  true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            cgb_memupc.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                ,DataGridViewContentAlignment.MiddleCenter  //11
                                };
            cgb_memupc.grid_col_alignment = g_Alignment;

            //Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();

            //cgb_memupc.grid_cell_format = gr_dic_cell_format;
        }
        private void tabP_pay_dGridView_Base_Header_Reset()
        {
            cgb_pay.basegrid = dGridView_Pay;
            cgb_pay.baseview = gridView9;
            cgb_pay.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_pay.grid_Frozen_End_Count = 1;

            string[] g_HeaderText = {"구분" ,  "마감일자" ,  "지급일자"   , "발생액"  , "소득세"
                                    , "주민세"  ,"실지급액"  , ""  , "" , ""
                                    , ""
                                    };

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb_pay.grid_col_header_text = g_HeaderText;
            cgb_pay.grid_col_Count = g_HeaderText.Length;

            int[] g_Width = { 100, 100, 100, 80, 80
                                ,80 , 80 , 0 , 0 , 0
                                ,0
                            };
            cgb_pay.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = {  true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true
                                   };
            cgb_pay.grid_col_Lock = g_ReadOnly;


            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //10
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };
            cgb_pay.grid_col_alignment = g_Alignment;

            cgb_pay.grid_cell_format = gr_dic_cell_format;
        }
        private void tabP_add_dGridView_Base_Header_Reset()
        {
            cgb_add.basegrid = dGridView_Add;
            cgb_add.baseview = gridView10;
            cgb_add.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_add.grid_Frozen_End_Count = 1;
            string[] g_HeaderText = {"구분"  , "우편_번호"   , "주소1"  , "주소2"   , "연락처1"
                                , "연락처2"   , "수취인명"    , ""  , "" , ""
                                ,""
                                };

            cgb_add.grid_col_header_text = g_HeaderText;
            cgb_add.grid_col_Count = g_HeaderText.Length;

            int[] g_Width = { 120, 100, 100, 100, 100
                                ,80 , 80 , 0 , 0 , 0
                                ,0
                            };
            cgb_add.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = {  true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true
                                   };
            cgb_add.grid_col_Lock = g_ReadOnly;



            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft  //5    
  
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };
            cgb_add.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();

            cgb_add.grid_cell_format = gr_dic_cell_format;


        }
        private void tabP_talk_dGridView_Base_Header_Reset()
        {
            cgb_Talk.basegrid = dGridView_Talk;
            cgb_Talk.baseview = gridView6;
            cgb_Talk.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Talk.grid_Frozen_End_Count = 1;
            string[] g_HeaderText = {"상담_내역" ,  "기록자" ,  "기록일"   , "_Seq"  , ""
                                    , ""  ,""  , ""  , "" , ""
                                    };

            cgb_Talk.grid_col_header_text = g_HeaderText;
            cgb_Talk.grid_col_Count = g_HeaderText.Length;


            int[] g_Width = { 500, 100, 200, 0, 0
                                ,0 , 0 , 0 , 0 , 0
                            };

            cgb_Talk.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = {  true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            cgb_Talk.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //
                                ,DataGridViewContentAlignment.MiddleRight  //10
                                };
            cgb_Talk.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();

            cgb_Talk.grid_cell_format = gr_dic_cell_format;

        }
        private void tabP_nomin_dGridView_Base_Header_Reset()
        {
            cgb_nomin.basegrid = dGridView_Down_N2;
            cgb_nomin.baseview = gridView1;
            cgb_nomin.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_nomin.grid_Frozen_End_Count = 1;
            string[] g_HeaderText = {"대수"  , "회원_번호"   , "직급"  , "성명"   , "가입일"
                                , "탈퇴일"   , "센타명"    , "후원인"   , "후원인명"    , "추천인"
                                , "추천인명"   , "집전화"  , "핸드폰"   , "ㅇㅇ"   ,"위치"
                                    };

            cgb_nomin.grid_col_header_text = g_HeaderText;
            cgb_nomin.grid_col_Count = g_HeaderText.Length;



            int[] g_Width = { 90, 90 , 130, 80, 100
                             ,100, 90, 130,130,130
                            ,130, 100, 100, 0, 100

                            };

            cgb_nomin.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };

            cgb_nomin.grid_col_Lock = g_ReadOnly;



            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //15   
                  
                              };
            cgb_nomin.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();

            cgb_nomin.grid_cell_format = gr_dic_cell_format;
        }
        private void tabP_save_dGridView_Base_Header_Reset()
        {
            cgb_save.basegrid = dGridView_Down_S2;
            cgb_save.baseview = gridView2;
            cgb_save.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_save.grid_Frozen_End_Count = 1;
            string[] g_HeaderText = {"대수"  , "회원_번호"   , "직급"  , "성명"   , "가입일"
                                , "탈퇴일"   , "센타명"    , "후원인"   , "후원인명"    , "추천인"
                                , "추천인명"   , "집전화"  , "핸드폰"   , "ㅇㅇ"   ,"위치"
                                    };

            cgb_save.grid_col_header_text = g_HeaderText;
            cgb_save.grid_col_Count = g_HeaderText.Length;



            int[] g_Width = { 90, 90 , 130, 80, 100
                             ,100, 90, 130,130,130
                            ,130, 100, 100, 0, 100

                            };

            cgb_save.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };

            cgb_save.grid_col_Lock = g_ReadOnly;
            //int[] g_Width = { 90, 90 , 130, 80, 60
            //                 ,100, 90, cls_app_static_var.save_uging_Pr_Flag, cls_app_static_var.save_uging_Pr_Flag, cls_app_static_var.nom_uging_Pr_Flag
            //                 ,cls_app_static_var.nom_uging_Pr_Flag , 90, 80, 0, 100
            //                };



            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //15   
                  
                              };
            cgb_save.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();

            cgb_save.grid_cell_format = gr_dic_cell_format;
        }
        private void tabP_item_dGridView_Base_Header_Reset()
        {
            cgb_Item.basegrid = dGridCtrl_Sell_Item;
            cgb_Item.baseview = dGridView_Sell_Item;
            cgb_Item.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Item.grid_Frozen_End_Count = 1;
            string[] g_HeaderText = {""  , "상품_코드"   , "상품명"  , "개별단가"   , "개별PV"
                                , "주문_수량"   , "총상품액"    , "총상품PV"  , "구분" , "비고"
                                ,"주문번호"
                                };

            int[] g_Width = { 0, 80, 200, 80, 80, 80
                                ,80 , 80 , 80 , 200 ,80
                            
                            };
            cgb_Item.grid_col_w = g_Width;

            cgb_Item.grid_col_header_text = g_HeaderText;
            cgb_Item.grid_col_Count = g_HeaderText.Length;

            Boolean[] g_ReadOnly = { false , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true
                                   };


            cgb_Item.grid_col_Lock = g_ReadOnly;
            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft  //10

                                ,DataGridViewContentAlignment.MiddleCenter
                                };


            cgb_Item.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb_Item.grid_cell_format = gr_dic_cell_format;
        }
        private void tabP_cacu_dGridView_Base_Header_Reset()
        {
            cgb_Cacu.basegrid = dGridView_Sell_Cacu;
            cgb_Cacu.baseview = gridView12;
            cgb_Cacu.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Cacu.grid_Frozen_End_Count = 1;
            string[] g_HeaderText = {""  , "결제방법"   , "결제액"  , "결제일자"   , "카드_은행명"
                                , "카드_은행번호"   , "카드소유자"    , "입금자"  , "비고" ,"주문번호"
                                };

            int[] g_Width = { 0, 90, 100, 90, 100
                                ,150 , 100 , 90 , 150 , 100
                            };

            cgb_Cacu.grid_col_w = g_Width;
            cgb_Cacu.grid_col_header_text = g_HeaderText;
            cgb_Cacu.grid_col_Count = g_HeaderText.Length;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true

                                   };

            cgb_Cacu.grid_col_Lock = g_ReadOnly;
            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };


            cgb_Cacu.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb_Cacu.grid_cell_format = gr_dic_cell_format;
        }
        private void tabP_rece_dGridView_Base_Header_Reset()
        {
            cgb_Rece.basegrid = dGridView_Sell_Rece;
            cgb_Rece.baseview = gridView13;
            cgb_Rece.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Rece.grid_Frozen_End_Count = 1;
            string[] g_HeaderText = {""  , "배송구분"   , "배송일"  , "수령인"   , "우편_번호"
                                , "주소1"   , "주소2"    , "연락처_1"  , "연락처_2" , "비고"
                                ,"주문번호"
                                };

            int[] g_Width = { 0, 90, 70, 90, 100
                                ,300 , 100 , 90 , 150 , 200
                                ,100
                            };

            cgb_Rece.grid_col_w = g_Width;
            cgb_Rece.grid_col_header_text = g_HeaderText;
            cgb_Rece.grid_col_Count = g_HeaderText.Length;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                     ,true
                                   };
            cgb_Rece.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter  //10

                                ,DataGridViewContentAlignment.MiddleCenter  //11
                                };

            cgb_Rece.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();

            cgb_Rece.grid_cell_format = gr_dic_cell_format;
        }


        private void tabP_Re_Pay_dGridView_Base_Header_Reset()
        {
            cgb_Re_Pay.basegrid = dGridView_Re_Pay;
            cgb_Re_Pay.baseview = gridView5;
            cgb_Re_Pay.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Re_Pay.grid_Frozen_End_Count = 1;

            string[] g_HeaderText = {"원마감일"  ,"확정마감일"  , "반품주문번호"   ,  "반품회원번호" , "반품성명"
                                     ,  "_멤버" ,"_소비전환"    , "_패키지"  , "후원"  ,"매칭"
                                     ,"공제예상액합산","반품PV"    , "차감한도","후원좌 차감" , "후원우 차감"
                                     ,"매칭회원상세", "매칭금액상세"
                                    };




            cgb_Re_Pay.grid_col_header_text = g_HeaderText;
            cgb_Re_Pay.grid_col_Count = g_HeaderText.Length;

            int[] g_Width = { 90,120, 90, 100, 80
                              , 0  ,0 , 0 ,80 , 80
                              , 80    , 80, 80    , 80  , 80
                              ,200                              ,200
                            };

            cgb_Re_Pay.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = {  true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true ,true
                                   };
            cgb_Re_Pay.grid_col_Lock = g_ReadOnly;


            DataGridViewContentAlignment[] g_Alignment =
                               {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft

                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5      
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight

                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //10
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight

                                ,DataGridViewContentAlignment.MiddleLeft  //10
                                ,DataGridViewContentAlignment.MiddleLeft  //10
                                };
            cgb_Re_Pay.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();

            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;


            cgb_Re_Pay.grid_cell_format = gr_dic_cell_format;           
        }



        private void Db_Grid_Popup(TextBox tb, TextBox tb1_Code)
        {
            cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            DataGridView Popup_gr = new DataGridView();
            Popup_gr.Name = "Popup_gr";
            this.Controls.Add(Popup_gr);
            cgb_Pop.basegrid = Popup_gr;
            cgb_Pop.Base_fr = this;
            cgb_Pop.Base_tb = tb1_Code;  //앞에게 코드
            cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            cgb_Pop.Base_Location_obj = tb;

            if (tb.Name == "txtCenter")
                cgb_Pop.Next_Focus_Control = mtxtZip1;

            if (tb.Name == "txtCenter2")
                cgb_Pop.Next_Focus_Control = mtxtZip1;

            if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = txtName_Accnt;

            if (tb.Name == "txtR_Id")
                cgb_Pop.Next_Focus_Control = txtName_Accnt;

            if (tb.Name == "txtChange")
                cgb_Pop.Next_Focus_Control = txtName_Accnt;

            if (tb.Name == "txtSellCode")
                cgb_Pop.Next_Focus_Control = txtName_Accnt;

            cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
        }
        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
            if (tab_Nation.Visible == true)
            {
                // combo_Se_Code.SelectedIndex = combo_Se.SelectedText;
                if (combo_Se_Code.Text == "")  //다국어 지원프로그램을 사용시 국가는 필히 선택을 해야 된다.
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Not_Na_Code")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    combo_Se.Focus(); return;
                }
            }

            //if (tb.Name == "txtCenter")
            //{
            //    Data_Set_Form_TF = 1;
            //    Db_Grid_Popup(tb, txtCenter_Code);
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtCenter_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtCenter_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            //if (tb.Name == "txtBank")
            //{
            //    Data_Set_Form_TF = 1;
            //    Db_Grid_Popup(tb, txtBank_Code);
            //    //if (tb.Text.ToString() == "")
            //    //    Db_Grid_Popup(tb, txtBank_Code, "");
            //    //else
            //    //    Ncod_Text_Set_Data(tb, txtBank_Code);

            //    //SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}


        }

        private void Base_Grid_Set(string Ufn_Name)
        {
            string T_Mbid = "";
            T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) != 1) return;

            string Tsql = "";

            Tsql = "Select  ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " T_up.mbid + '-' + Convert(Varchar,T_up.mbid2) ";
            else
                Tsql = Tsql + " T_up.mbid2 ";

            Tsql = Tsql + " ,T_up.M_Name ";
            Tsql = Tsql + " ,T_up.curP ";

            Tsql = Tsql + " From " + Ufn_Name;
            Tsql = Tsql + " ('" + Mbid + "'," + Mbid2.ToString() + ") AS T_up";

            Tsql = Tsql + " Where    lvl > 0 ";
            Tsql = Tsql + " Order BY lvl Desc ";

            //당일 등록된 회원을 불러온다.

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();


            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic_Line(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            cg_Up_S.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cg_Up_S.db_grid_Obj_Data_Put();
        }
        private void Set_Form_Date_Up(int intTemp) //추천 관련.
        {
            if (intTemp == 1) //추천상위
                dGridView_Up_S_Header_Reset(dGridView_Up_N); //디비그리드 헤더와 기본 셋팅을 한다.
            else
                dGridView_Up_S_Header_Reset(dGridView_Down_N); //디비그리드 헤더와 기본 셋팅을 한다.

            cg_Up_S.d_Grid_view_Header_Reset();

            if (intTemp == 1) //추천상위
            {
                if (chk_N.Checked == true) return; //최상위 이면 상선 내역을 보여줄 필요가 없다.            
                Base_Grid_Set(" ufn_Up_Search_Nomin ");
            }
            else
            {
                Base_Grid_Down_Set("N");
            }
        }
        private void Set_Form_Date_Up(string strTemp)
        {
            if (strTemp == "S")
                dGridView_Up_S_Header_Reset(dGridView_Up_S); //디비그리드 헤더와 기본 셋팅을 한다.
            else
                dGridView_Up_S_Header_Reset(dGridView_Down_S); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset();

            if (strTemp == "S")
            {
                if (chk_S.Checked == true) return;     //최상위 이면 상선 내역을 보여줄 필요가 없다.   
                Base_Grid_Set(" ufn_Up_Search_Save ");
            }
            else
            {
                Base_Grid_Down_Set("S");
            }
        }
        private void Base_Grid_Down_Set(string tSort)
        {
            string T_Mbid = "";
            T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) != 1) return;

            string Tsql = "";

            Tsql = "Select  ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) ";
            else
                Tsql = Tsql + " tbl_Memberinfo.mbid2 ";

            Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

            if (tSort == "S")
            {
                Tsql = Tsql + " ,tbl_Memberinfo.LineCnt ";
                Tsql = Tsql + " From tbl_Memberinfo ";
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where Saveid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where Saveid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   Saveid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By LineCnt ASC ";
            }
            else
            {
                Tsql = Tsql + " ,tbl_Memberinfo.N_LineCnt ";
                Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where Nominid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where Nominid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   Nominid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By N_LineCnt ASC ";
            }

            //당일 등록된 회원을 불러온다.

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic_Line(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            cg_Up_S.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cg_Up_S.db_grid_Obj_Data_Put();
        }
        private void lookUpEdit1_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            //e.DisplayText = _text + ": " + e.DisplayText;
            SearchLookUpEdit edit = sender as SearchLookUpEdit;
            if (edit != null && edit.Focused)
            {
                int theIndex = edit.Properties.GetIndexByKeyValue(edit.EditValue);
                if (edit.Properties.View.IsDataRow(theIndex))
                {
                    DataRow row = edit.Properties.View.GetDataRow(theIndex);
                    e.DisplayText = row["Lot"].ToString() + "; " + row["Description"].ToString();
                }
            }
        }

        private void Set_Form_Date(DataSet ds)
        {
            StringEncrypter decrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

            idx_Mbid = ds.Tables[base_db_name].Rows[0]["Mbid"].ToString();
            idx_Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[0]["Mbid2"].ToString());

            mtxtMbid.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();
            txtName.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();
            mtxtSn.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Cpno"].ToString(), "Cpno");
            txtName_E_1.Text = ds.Tables[base_db_name].Rows[0]["E_name"].ToString();
            txtName_E_2.Text = ds.Tables[base_db_name].Rows[0]["E_name_Last"].ToString();
            txtLineCnt.Text = ds.Tables[base_db_name].Rows[0]["LineCnt"].ToString();

            txtMbid_s.Text = ds.Tables[base_db_name].Rows[0]["T_Saveid"].ToString();
            txtName_s.Text = ds.Tables[base_db_name].Rows[0]["Save_Name"].ToString();
            txtSN_s.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Save_Cpno"].ToString(), "Cpno");

            txtMbid_n.Text = ds.Tables[base_db_name].Rows[0]["T_Nominid"].ToString();
            txtName_n.Text = ds.Tables[base_db_name].Rows[0]["Nomin_Name"].ToString();
            txtSN_n.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Nom_Cpno"].ToString(), "Cpno");

            txtGrade.Text = ds.Tables[base_db_name].Rows[0]["G_Name"].ToString();
            txtGradeP.Text = ds.Tables[base_db_name].Rows[0]["G_NameP"].ToString();

            if (ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString().Replace("-", "").Trim() != "")
                txtLeaveDate.Text = string.Format("{0:####-##-##}", int.Parse(ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString()));//ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString();

            if (ds.Tables[base_db_name].Rows[0]["LineUserDate"].ToString().Replace("-", "").Trim() != "")
                txtLineDate.Text = string.Format("{0:####-##-##}", int.Parse(ds.Tables[base_db_name].Rows[0]["LineUserDate"].ToString()));//ds.Tables[base_db_name].Rows[0]["LineUserDate"].ToString();

            if (ds.Tables[base_db_name].Rows[0]["PayStop_Date"].ToString().Replace("-", "").Trim() != "")
                txtS.Text = string.Format("{0:####-##-##}", int.Parse(ds.Tables[base_db_name].Rows[0]["PayStop_Date"].ToString()));  //ds.Tables[base_db_name].Rows[0]["PayStop_Date"].ToString();

            if (ds.Tables[base_db_name].Rows[0]["Regtime"].ToString().Replace("-", "").Trim() != "")
                mtxtRegDate.Text = string.Format("{0:####-##-##}", int.Parse(ds.Tables[base_db_name].Rows[0]["Regtime"].ToString())); //ds.Tables[base_db_name].Rows[0]["Regtime"].ToString();

            if (ds.Tables[base_db_name].Rows[0]["Ed_Date"].ToString().Replace("-", "").Trim() != "")
                mtxtEdDate.Text = string.Format("{0:####-##-##}", int.Parse(ds.Tables[base_db_name].Rows[0]["Ed_Date"].ToString()));  // ds.Tables[base_db_name].Rows[0]["Ed_Date"].ToString();


            if (ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString().Replace("-", "").Trim() != "")
            {
                //txtAddCode1.Text = ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString().Substring(0, 3);
                //txtAddCode2.Text = ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString().Substring(3, 3);                
                mtxtZip1.Text = ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString();
            }
            txtAddress1.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Address1"].ToString());
            txtAddress2.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Address2"].ToString());


            string T_tel = "";
            if (decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hometel"].ToString()).Replace("-", "").Trim() != "")
            {
                //string[] tel = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hometel"].ToString()).Split('-');
                //txtTel_1.Text = tel[0].ToString ();
                //txtTel_2.Text = tel[1].ToString();
                //txtTel_3.Text = tel[2].ToString();

                mtxtTel1.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hometel"].ToString());

                // T_tel = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hometel"].ToString());
            }

            if (decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hptel"].ToString()).Replace("-", "").Trim() != "")
            {
                //string[] tel = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hptel"].ToString()).Split('-');
                //txtTel2_1.Text = tel[0].ToString();
                //txtTel2_2.Text = tel[1].ToString();
                //txtTel2_3.Text = tel[2].ToString();

                mtxtTel2.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hptel"].ToString());
            }

            //20190306 구현호 룩업에디트 텍스트에 검색쿼리 테이블데이터에서 가져오기
            string bankcode = ds.Tables[base_db_name].Rows[0]["bankcode"].ToString();
            string businesscode = ds.Tables[base_db_name].Rows[0]["businesscode"].ToString();
            string bank = ds.Tables[base_db_name].Rows[0]["Bank_Name"].ToString();
            string business = ds.Tables[base_db_name].Rows[0]["B_name"].ToString();
            string bankcomplite = bankcode + " " + bank;
            string businesscomplite = businesscode + " " + business;

            txtCenter.EditValue = businesscomplite;
            txtBank.EditValue = bankcomplite;

        
            txtCenter_Code.Text = ds.Tables[base_db_name].Rows[0]["businesscode"].ToString();
            
            txtBank_Code.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["bankcode"].ToString());
            txtAccount.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["bankaccnt"].ToString());
            txtName_Accnt.Text = ds.Tables[base_db_name].Rows[0]["bankowner"].ToString();


            txtAccount_Reg.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Reg_bankaccnt"].ToString());


            txtWebID.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["webid"].ToString());
            txtPassword.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["webpassword"].ToString());

            idx_Password = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["webpassword"].ToString());

            txtEmail.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Email"].ToString());
            txtRemark.Text = ds.Tables[base_db_name].Rows[0]["Remarks"].ToString();

            txtB1.Text = ds.Tables[base_db_name].Rows[0]["GiBu_"].ToString();

            mtxtVisaDay.Text = ds.Tables[base_db_name].Rows[0]["VisaDate"].ToString();





            string BirthDay = ds.Tables[base_db_name].Rows[0]["BirthDay"].ToString();
            if (BirthDay != "")
            {
                BirthDay = BirthDay + "-" + ds.Tables[base_db_name].Rows[0]["BirthDay_M"].ToString();
                BirthDay = BirthDay + "-" + ds.Tables[base_db_name].Rows[0]["BirthDay_D"].ToString();

                mtxtBrithDay.Text = BirthDay;
            }

            //소비자는 1 판매원은 기본 0
            if (ds.Tables[base_db_name].Rows[0]["Sell_Mem_TF"].ToString() == "1")
                opt_sell_3.Checked = true;
            else
                opt_sell_2.Checked = true;

            // 내국인은 0 외국인은 1  사업자는 2
            if (ds.Tables[base_db_name].Rows[0]["For_Kind_TF"].ToString() == "0")
                raButt_IN_1.Checked = true;
            else if (ds.Tables[base_db_name].Rows[0]["For_Kind_TF"].ToString() == "1")
                raButt_IN_2.Checked = true;
            else
                raButt_IN_3.Checked = true;

            //양력은 1  음력은 2
            if (ds.Tables[base_db_name].Rows[0]["BirthDayTF"].ToString() == "1")
                opt_Bir_TF_1.Checked = true;
            else
                opt_Bir_TF_2.Checked = true;


            if (ds.Tables[base_db_name].Rows[0]["RBO_Mem_TF"].ToString() == "0")
                radioB_RBO.Checked = true;
            else
                radioB_Begin.Checked = true;
            mtxtRBODate.Text = ds.Tables[base_db_name].Rows[0]["RBO_S_Date"].ToString();


            if (ds.Tables[base_db_name].Rows[0]["G8_TF"].ToString() == "8")
                radioB_G8.Checked = true;
            else
                radioB_G4.Checked = true;



            check_MyOffice.Checked = false;
            if (ds.Tables[base_db_name].Rows[0]["Myoffice_TF"].ToString() == "1")
                check_MyOffice.Checked = true;


            if (ds.Tables[base_db_name].Rows[0]["Saveid"].ToString() != "")
            {
                if (ds.Tables[base_db_name].Rows[0]["Saveid"].ToString().Substring(0, 1) == "*")
                    chk_S.Checked = true;
            }

            if (ds.Tables[base_db_name].Rows[0]["Nominid"].ToString() != "")
            {
                if (ds.Tables[base_db_name].Rows[0]["Nominid"].ToString().Substring(0, 1) == "*")
                    chk_N.Checked = true;
            }

            if (int.Parse(ds.Tables[base_db_name].Rows[0]["Add_TF"].ToString()) == 1)
                opt_B_1.Checked = true;
            else if (int.Parse(ds.Tables[base_db_name].Rows[0]["Add_TF"].ToString()) == 2)
                opt_B_2.Checked = true;
            else if (int.Parse(ds.Tables[base_db_name].Rows[0]["Add_TF"].ToString()) == 3)
                opt_B_3.Checked = true;
            else
            {
                opt_B_1.Checked = false; opt_B_2.Checked = false; opt_B_3.Checked = false;
            }

            if (int.Parse(ds.Tables[base_db_name].Rows[0]["BankDocument"].ToString()) == 1)
                check_BankDocument.Checked = true;

            if (int.Parse(ds.Tables[base_db_name].Rows[0]["CpnoDocument"].ToString()) == 1)
                check_CpnoDocument.Checked = true;

            radioB_Sex_X.Checked = false;
            radioB_Sex_Y.Checked = false;
            if (ds.Tables[base_db_name].Rows[0]["Sex_FLAG"].ToString() == "X")
                radioB_Sex_X.Checked = true;

            if (ds.Tables[base_db_name].Rows[0]["Sex_FLAG"].ToString() == "Y")
                radioB_Sex_Y.Checked = true;


            if (ds.Tables[base_db_name].Rows[0]["AgreeSMS"].ToString() == "Y")
                checkB_SMS_FLAG.Checked = true;
            else
                checkB_SMS_FLAG.Checked = false;


            if (ds.Tables[base_db_name].Rows[0]["AgreeEmail"].ToString() == "Y")
                checkB_EMail_FLAG.Checked = true;
            else
                checkB_EMail_FLAG.Checked = false;


            if (ds.Tables[base_db_name].Rows[0]["ETC_Addcode1"].ToString().Replace("-", "").Trim() != "")
            {
                //txtAddCode1.Text = ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString().Substring(0, 3);
                //txtAddCode2.Text = ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString().Substring(3, 3);                
                mtxtZip2.Text = ds.Tables[base_db_name].Rows[0]["ETC_Addcode1"].ToString();
            }
            txtAddress3.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["ETC_Address1"].ToString());
            txtAddress4.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["ETC_Address2"].ToString());

            combo_Se.Text = ds.Tables[base_db_name].Rows[0]["nationNameEng"].ToString();
            combo_Se_Code.Text = ds.Tables[base_db_name].Rows[0]["Na_Code"].ToString();

            radioB_Sex_X.Checked = ds.Tables[base_db_name].Rows[0]["Sex_FLAG"].ToString() == "X";
            radioB_Sex_Y.Checked = ds.Tables[base_db_name].Rows[0]["Sex_FLAG"].ToString() == "Y";
            checkB_SMS_FLAG.Checked = ds.Tables[base_db_name].Rows[0]["AgreeSMS"].ToString() == "Y";
            checkB_EMail_FLAG.Checked = ds.Tables[base_db_name].Rows[0]["AgreeEmail"].ToString() == "Y";


            if (ds.Tables[base_db_name].Rows[0]["C_M_Name"].ToString() != "")
            {
                check_CC.Checked = true;
                txtName_C.Text = ds.Tables[base_db_name].Rows[0]["C_M_Name"].ToString();
                //mtxtSn_C.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["C_cpno"].ToString());

                //txtName_E_1_C.Text = ds.Tables[base_db_name].Rows[0]["C_E_name"].ToString();
                //txtName_E_2_C.Text = ds.Tables[base_db_name].Rows[0]["C_E_name_Last"].ToString();

                BirthDay = ds.Tables[base_db_name].Rows[0]["C_BirthDay"].ToString();
                if (BirthDay != "")
                {
                    BirthDay = BirthDay + "-" + ds.Tables[base_db_name].Rows[0]["C_BirthDay_M"].ToString();
                    BirthDay = BirthDay + "-" + ds.Tables[base_db_name].Rows[0]["C_BirthDay_D"].ToString();
                    mtxtBrithDayC.Text = BirthDay;
                }

                // 내국인은 0 외국인은 1  사업자는 2
                if (ds.Tables[base_db_name].Rows[0]["C_For_Kind_TF"].ToString() == "0")
                    raButt_IN_1_C.Checked = true;
                else if (ds.Tables[base_db_name].Rows[0]["C_For_Kind_TF"].ToString() == "1")
                    raButt_IN_2_C.Checked = true;

                mtxtTel2_C.Text = ds.Tables[0].Rows[0]["C_hptel"].ToString();
                txtEmail_C.Text = ds.Tables[0].Rows[0]["C_Email"].ToString();
            }


            //button_exigo.Visible = false;

            //if (int.Parse (ds.Tables[base_db_name].Rows[0]["US_Num"].ToString()) == 0 )
            //    button_exigo.Visible = true;


            txt_Us.Text = ds.Tables[base_db_name].Rows[0]["US_Num"].ToString();


            txtName.ReadOnly = true;
            txtName.BackColor = cls_app_static_var.txt_Enable_Color;
            //txtName.BorderStyle = BorderStyle.FixedSingle;
        }

        private void Set_Form_Date_Info()
        {

            tabP_Sell_dGridView_Base_Header_Reset();//주문내역 그리드형식
                                                    //20190227 구현호 그리드뽑기, 5번은 매출
            Base_Grid_info_Set(5);

        }
        private void Set_Form_Date_Change()
        {
            tabP_info_dGridView_Base_Header_Reset();//정보변경 그리드형식
                                                    //20190227 구현호 그리드뽑기, 2번은 변경내역
            Base_Grid_info_Set(2);
        }
        private void Set_Form_Date_memupc()
        {
            tabP_memupc_dGridView_Base_Header_Reset();//상선변경 그리드형식
                                                      //20190227 구현호 그리드뽑기, 2번은 변경내역
            Base_Grid_info_Set(6);
        }

        private void Set_Form_Date_pay()
        {
            tabP_pay_dGridView_Base_Header_Reset();//수당 그리드형식
                                                   //20190227 구현호 그리드뽑기, 2번은 변경내역
            Base_Grid_info_Set(7);
        }
        private void Set_Form_Date_add()
        {
            tabP_add_dGridView_Base_Header_Reset();//주소내역 그리드형식
                                                   //20190227 구현호 그리드뽑기, 2번은 변경내역
            Base_Grid_info_Set(8);
        }
        private void Set_Form_Date_Talk()
        {
            tabP_talk_dGridView_Base_Header_Reset();//상담내역 그리드형식
                                                    //20190227 구현호 그리드뽑기, 2번은 변경내역
            Base_Grid_info_Set(9);
        }
        private void Set_Form_Date_nomin()
        {
            tabP_nomin_dGridView_Base_Header_Reset();//추천인 및 하선인원 그리드형식
                                                     //20190227 구현호 그리드뽑기, 2번은 변경내역
            Base_Grid_info_Set(10);
        }
        private void Set_Form_Date_save()
        {
            tabP_save_dGridView_Base_Header_Reset();//후원인 및 하선인원 그리드형식
                                                    //20190227 구현호 그리드뽑기, 2번은 변경내역
            Base_Grid_info_Set(11);
        }
        private void Set_Form_Date_item()
        {
            tabP_item_dGridView_Base_Header_Reset();//주문내역 주문상품내역 리드형식
                                                    //20190227 구현호 그리드뽑기, 2번은 변경내역
            Base_Grid_info_Set(12);
        }
        private void Set_Form_Date_cacu()
        {
            tabP_cacu_dGridView_Base_Header_Reset();//주문내역 결제내역 그리드형식
                                                    //20190227 구현호 그리드뽑기, 2번은 변경내역
            Base_Grid_info_Set(13);
        }
        private void Set_Form_Date_rece()
        {
            tabP_rece_dGridView_Base_Header_Reset();//주문내역 배송내역 그리드형식
                                                    //20190227 구현호 그리드뽑기, 2번은 변경내역
            Base_Grid_info_Set(14);
        }

        private void Set_Form_Date_Re_Pay()
        {
            tabP_Re_Pay_dGridView_Base_Header_Reset();//주문내역 배송내역 그리드형식
                                                    //20190227 구현호 그리드뽑기, 2번은 변경내역
            Base_Grid_info_Set(15);
        }
        
        private void Set_Form_Date(string T_Mbid, string T_sort)
        {
            if (cgb.basegrid != null)
            {
                _From_Data_Clear();
            }
            idx_Mbid = ""; idx_Mbid2 = 0;
            string Mbid = ""; int Mbid2 = 0;
            Data_Set_Form_TF = 1;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == 1)
            {
                string Tsql = "";

                Tsql = "Select  ";
                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) AS M_Mbid ";
                else
                    Tsql = Tsql + " tbl_Memberinfo.mbid2 AS M_Mbid ";

                Tsql = Tsql + " ,tbl_Memberinfo.mbid ";
                Tsql = Tsql + " ,tbl_Memberinfo.mbid2 ";
                Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";
                Tsql = Tsql + " ,tbl_Memberinfo.E_name ";
                Tsql = Tsql + " ,tbl_Memberinfo.E_name_Last ";

                Tsql = Tsql + " , tbl_Memberinfo.Email  AS Email ";
                Tsql = Tsql + ", tbl_Memberinfo.Cpno AS Cpno ";

                Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";

                Tsql = Tsql + " , tbl_Memberinfo.RegTime ";
                Tsql = Tsql + " , tbl_Memberinfo.hptel   AS hptel";
                Tsql = Tsql + " , tbl_Memberinfo.Addcode1  AS Addcode1 ";
                Tsql = Tsql + " , tbl_Memberinfo.address1  AS address1 ";
                Tsql = Tsql + " , tbl_Memberinfo.address2   AS address2";

                Tsql = Tsql + " , tbl_Memberinfo.hometel   AS hometel";
                //Tsql = Tsql + " , tbl_Memberinfo.hptel )  AS hptel";
                Tsql = Tsql + " , tbl_Memberinfo.businesscode ";
                Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";

                Tsql = Tsql + " , tbl_Memberinfo.BankCode ";
                Tsql = Tsql + " ,Isnull(tbl_Bank.bankName,'') as Bank_Name";
                Tsql = Tsql + " , tbl_Memberinfo.bankowner ";
                Tsql = Tsql + " , tbl_Memberinfo.bankaccnt  AS bankaccnt ";
                Tsql = Tsql + " , tbl_Memberinfo.Reg_bankaccnt  AS Reg_bankaccnt ";


                Tsql = Tsql + " , tbl_Memberinfo.Remarks ";

                Tsql = Tsql + " , tbl_Memberinfo.BirthDay ";
                Tsql = Tsql + " , tbl_Memberinfo.BirthDay_M ";
                Tsql = Tsql + " , tbl_Memberinfo.BirthDay_D ";
                Tsql = Tsql + " , tbl_Memberinfo.BirthDayTF ";

                Tsql = Tsql + " , tbl_Memberinfo.CpnoDocument ";
                Tsql = Tsql + " , tbl_Memberinfo.BankDocument ";

                Tsql = Tsql + " , tbl_Memberinfo.LeaveDate ";
                Tsql = Tsql + " , tbl_Memberinfo.LineUserDate ";
                Tsql = Tsql + " , tbl_Memberinfo.WebID ";
                Tsql = Tsql + " , tbl_Memberinfo.WebPassWord ";
                Tsql = Tsql + " , tbl_Memberinfo.Ed_Date ";
                Tsql = Tsql + " , tbl_Memberinfo.PayStop_Date ";

                Tsql = Tsql + " , tbl_Memberinfo.For_Kind_TF ";
                Tsql = Tsql + " , tbl_Memberinfo.Sell_Mem_TF ";
                Tsql = Tsql + " , tbl_Memberinfo.Add_TF ";
                Tsql = Tsql + " , tbl_Memberinfo.GiBu_ ";
                Tsql = Tsql + " , tbl_Memberinfo.Myoffice_TF ";

                Tsql = Tsql + " , tbl_Memberinfo.VisaDate ";
                Tsql = Tsql + " , tbl_Memberinfo.RBO_S_Date ";


                Tsql = Tsql + " , tbl_Memberinfo.C_M_Name ";
                Tsql = Tsql + " , tbl_Memberinfo.C_For_Kind_TF ";
                Tsql = Tsql + " , tbl_Memberinfo.C_cpno ";
                Tsql = Tsql + " , tbl_Memberinfo.C_E_name ";
                Tsql = Tsql + " , tbl_Memberinfo.C_E_name_Last ";

                Tsql = Tsql + " , tbl_Memberinfo.C_BirthDay ";
                Tsql = Tsql + " , tbl_Memberinfo.C_BirthDay_M ";
                Tsql = Tsql + " , tbl_Memberinfo.C_BirthDay_D ";
                Tsql = Tsql + " , tbl_Memberinfo.C_hptel ";
                Tsql = Tsql + " , tbl_Memberinfo.C_email ";

                Tsql = Tsql + " , tbl_Memberinfo.RBO_Mem_TF ";
                Tsql = Tsql + " , tbl_Memberinfo.G8_TF ";

                Tsql = Tsql + " , tbl_Memberinfo.Sex_FLAG";
                Tsql = Tsql + " , tbl_Memberinfo.AgreeSMS";
                Tsql = Tsql + " , tbl_Memberinfo.AgreeEmail";
                //Tsql = Tsql + " , tbl_Memberinfo.ipin_ci"; //휴대폰인증은 명의변경쪽에서 진행해야함
                //Tsql = Tsql + " , tbl_Memberinfo.ipin_di";



                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " ,tbl_Memberinfo.Saveid + '-' + Convert(Varchar,tbl_Memberinfo.Saveid2) AS T_Saveid ";
                else
                    Tsql = Tsql + " ,tbl_Memberinfo.Saveid2 AS T_Saveid ";

                Tsql = Tsql + " , Isnull(Sav.M_Name,'') AS Save_Name ";
                Tsql = Tsql + " , tbl_Memberinfo.Saveid ";
                Tsql = Tsql + ",  Sav.Cpno  AS Save_Cpno ";
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<  

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " ,tbl_Memberinfo.Nominid + '-' + Convert(Varchar,tbl_Memberinfo.Nominid2) AS T_Nominid ";
                else
                    Tsql = Tsql + " ,tbl_Memberinfo.Nominid2 AS T_Nominid ";

                Tsql = Tsql + " , Isnull(Nom.M_Name,'') AS Nomin_Name ";
                Tsql = Tsql + " , tbl_Memberinfo.Nominid ";

                Tsql = Tsql + ",  Nom.Cpno AS Nom_Cpno ";

                //if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
                //    Tsql = Tsql + ", Case When  Nom.Cpno <> '' Then LEFT(Nom.Cpno,6) +'-' + RIGHT(Nom.Cpno,7)  ELSE '' End AS Nom_Cpno";
                //else
                //    Tsql = Tsql + ", Case When  Nom.Cpno <> '' Then LEFT(Nom.Cpno,6) +'-' + '*******'  ELSE '' End  AS Nom_Cpno";
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<  

                Tsql = Tsql + ", Isnull(ETC_Addcode1,'') ETC_Addcode1 ";
                Tsql = Tsql + ", Isnull(ETC_Address1,'') ETC_Address1  ";
                Tsql = Tsql + ", Isnull(ETC_Address2,'') ETC_Address2  ";

                Tsql = Tsql + ", Isnull(nationNameEng,'') nationNameEng , tbl_Memberinfo.Na_code ";

                Tsql = Tsql + ", CC_A.G_Name ";
                Tsql = Tsql + " , ISNULL(CP.Grade_Name,'')  G_NameP  ";

                Tsql = Tsql + ", isnull(MAuto.A_CardCode,'') A_CardCode ";
                Tsql = Tsql + ", isnull(MAuto.A_CardNumber,'') A_CardNumber ";
                Tsql = Tsql + ", isnull(MAuto.A_Period1,'') A_Period1 ";
                Tsql = Tsql + ", isnull(MAuto.A_Period2,'') A_Period2 ";
                Tsql = Tsql + ", isnull(MAuto.A_Card_Name_Number,'') A_Card_Name_Number ";
                Tsql = Tsql + ", isnull(MAuto.A_Start_Date,'') A_Start_Date ";
                Tsql = Tsql + ", isnull(MAuto.A_Month_Date,'') A_Month_Date ";
                Tsql = Tsql + ", isnull(MAuto.A_Stop_Date,'') A_Stop_Date ";

                Tsql = Tsql + ", isnull(MAuto.A_Rec_Name,'') A_Rec_Name ";
                Tsql = Tsql + ", isnull(MAuto.A_hptel,'') A_hptel ";
                Tsql = Tsql + ", isnull(MAuto.A_Addcode1,'') A_Addcode1 ";
                Tsql = Tsql + ", isnull(MAuto.A_Address1,'') A_Address1 ";
                Tsql = Tsql + ", isnull(MAuto.A_Address2,'') A_Address2 ";
                Tsql = Tsql + ", isnull(MAuto.A_ETC,'') A_ETC ";

                Tsql = Tsql + ", isnull(MAuto.A_ProcDay,'') A_ProcDay ";
                Tsql = Tsql + ", isnull(MAuto.A_ProcAmt,0) A_ProcAmt ";

                Tsql = Tsql + ", isnull(tbl_Card.cardname,'') Card_Name";

                Tsql = Tsql + ", isnull(MAuto.Mbid2,0) A_Mbid2 ";


                Tsql = Tsql + ", isnull(tbl_Memberinfo.US_Num,0) US_Num ";


                Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Sav (nolock) ON tbl_Memberinfo.Saveid = Sav.Mbid And tbl_Memberinfo.Saveid2 = Sav.Mbid2 ";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Nom (nolock) ON tbl_Memberinfo.Nominid = Nom.Mbid And tbl_Memberinfo.Nominid2 = Nom.Mbid2 ";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo_Address MAdd (nolock) ON MAdd.Mbid = tbl_Memberinfo.Mbid And MAdd.Mbid2 = tbl_Memberinfo.Mbid2 And Sort_Add = 'R' ";

                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo_A MAuto (nolock) ON MAuto.Mbid = tbl_Memberinfo.Mbid And MAuto.Mbid2 = tbl_Memberinfo.Mbid2 ";
                Tsql = Tsql + " LEFT JOIN tbl_Card (nolock) ON tbl_Card.Ncode = MAuto.A_CardCode ";

                Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";
                //Tsql = Tsql + " Left Join tbl_Bank (nolock) On tbl_Memberinfo.bankcode=tbl_Bank.ncode And tbl_Memberinfo.Na_code = tbl_Bank.Na_code ";
                Tsql = Tsql + " Left Join tbl_Bank (nolock) On tbl_Memberinfo.bankcode=tbl_Bank.ncode ";
                cls_NationService.SQL_BankNationCode(ref Tsql);
                Tsql = Tsql + " LEFT JOIN  tbl_Nation  (nolock) ON tbl_Nation.nationCode = tbl_Memberinfo.Na_Code  ";
                Tsql = Tsql + " Left Join tbl_Class_P CP On tbl_Memberinfo.CurPoint = CP.Grade_Cnt ";
                Tsql = Tsql + " Left Join ufn_Mem_CurGrade_Mbid_Search ('" + Mbid + "'," + Mbid2.ToString() + ") AS CC_A On CC_A.Mbid = tbl_Memberinfo.Mbid And  CC_A.Mbid2 = tbl_Memberinfo.Mbid2 ";

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                }

                //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
                Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
                Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


                if (tab_Nation.Visible == true)
                {
                    if (combo_Se_Code.Text != "")
                    {
                        Tsql = Tsql + " And tbl_Memberinfo.Na_Code = '" + combo_Se_Code.Text + "'";
                    }
                }


                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;
                //++++++++++++++++++++++++++++++++
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                progress.Visible = true;
                progress.Maximum = 90; progress.Value = 10; progress.Refresh();
                Set_Form_Date(ds);
                progress.Value = progress.Value + 10; progress.Refresh();

                Set_Form_Date_Up(1);    //추천인 상선을 뿌려줌
                progress.Value = progress.Value + 10; progress.Refresh();

                Set_Form_Date_Up("S");  //후원인 상선을 뿌려줌
                progress.Value = progress.Value + 10; progress.Refresh();

                Set_Form_Date_Up(2);    //직추천한 사람들을 뿌려줌
                progress.Value = progress.Value + 10; progress.Refresh();

                Set_Form_Date_Up("S2");  //직후원한 사람들을 뿌려줌.
                progress.Value = progress.Value + 10; progress.Refresh();






                Set_Form_Date_Info(); //회원 매출 관련 뿌려줌   , 변경 정보, 수당 발생 내역 , 후원인 추천인 변경 내역 뿌려줌  

                Set_Form_Date_Change();// 정보변경 관련 뿌려줌

                Set_Form_Date_memupc();//상선변경 관련 뿌려줌

                Set_Form_Date_pay(); // 수당내역 관련 뿌려줌

                Set_Form_Date_add(); // 주소내역 관련 뿌려줌

                Set_Form_Date_Talk(); //상담내역 관련 뿌려줌

                Set_Form_Date_nomin(); //추천인및하선인원 뿌려줌

                Set_Form_Date_save(); //후원인 및 하선인원 뿌려줌

                Set_Form_Date_item(); //주문내역 첫번째

                Set_Form_Date_cacu(); //주문내역 두번째

                Set_Form_Date_rece(); //주문내역 세번쩨

                Set_Form_Date_Re_Pay();

                progress.Value = progress.Value + 10; progress.Refresh();





                //chart_Item.Series.Clear();
                //Save_Nom_Line_Chart();
                //Set_SalesItemDetail(Mbid, Mbid2); //상품 관련 집계 도표를 뿌려준다.
                //Set_Form_Date_Talk(); //상담내역을 뿌려준다.
                progress.Value = progress.Value + 10; progress.Refresh();

                //Set_SalesDetail_Chart(Mbid, Mbid2); //pie 도표를 뿌려준다.
                progress.Value = progress.Value + 10; progress.Refresh();

                progress.Visible = false;
                this.Cursor = System.Windows.Forms.Cursors.Default;

                mtxtMbid.Focus();
            }

            Data_Set_Form_TF = 0;
        }
        void e_f_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            mtxtMbid.Text = Send_Number; txtName.Text = Send_Name;
            if (Input_Error_Check(mtxtMbid, "m") == true)
                Set_Form_Date(mtxtMbid.Text, "m");
        }
        void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtName.Text.Trim();
        }
        void T_R_Key_Enter_13_Name(string txt_tag, Control tb)
        {
            if (txt_tag != "")
            {
                int reCnt = 0;
                cls_Search_DB cds = new cls_Search_DB();
                string Search_Mbid = "";
                reCnt = cds.Member_Name_Search(ref Search_Mbid, txt_tag);

                if (reCnt == 1)
                {
                    if (tb.Name == "txtName")
                    {
                        mtxtMbid.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
                        if (Input_Error_Check(mtxtMbid, "m") == true)
                            Set_Form_Date(mtxtMbid.Text, "m");
                    }
                }
                else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
                {

                    //cls_app_static_var.Search_Member_Name = txt_tag;
                    frmBase_Member_Search e_f = new frmBase_Member_Search();
                    if (tb.Name == "txtName")
                    {
                        e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                        e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);
                    }

                    e_f.ShowDialog();

                    SendKeys.Send("{TAB}");
                }


            }
            else
                SendKeys.Send("{TAB}");

        }
        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string txt_tag = txtName.Text;
                if (txt_tag != "")
                {
                    int reCnt = 0;
                    cls_Search_DB cds = new cls_Search_DB();
                    string Search_Mbid = "";
                    reCnt = cds.Member_Name_Search(ref Search_Mbid, txt_tag);

                    if (reCnt == 1)
                    {
                        //if (tb.Name == "txtName_s")
                        //{
                        //    mtxtMbid_s.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
                        //    if (Input_Error_Check(mtxtMbid_s, "s") == true)
                        //        Set_Form_Date(mtxtMbid_s.Text, "s");

                        //    //SendKeys.Send("{TAB}");
                        //}

                        //if (tb.Name == "txtName_n")
                        //{
                        mtxtMbid.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
                        if (Input_Error_Check(mtxtMbid, "n") == true)
                            Set_Form_Date(mtxtMbid.Text, "n");
                        //SendKeys.Send("{TAB}");
                        //}
                    }
                    else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
                    {

                        //cls_app_static_var.Search_Member_Name = txt_tag;
                        frmBase_Member_Search e_f = new frmBase_Member_Search();
                        //if (tb.Name == "txtName_s")
                        //{
                        //    e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                        //    e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);
                        //}

                        //if (tb.Name == "txtName_n")
                        //{
                        e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_txtname);
                        e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                        //}

                        e_f.ShowDialog();

                        SendKeys.Send("{TAB}");
                    }


                }
                else
                    SendKeys.Send("{TAB}");
            }
        }
        void e_f_Send_MemName_txtname(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = txtName.Text.Trim(); ;
        }
        private void txtName_Enter(object sender, EventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            if (sender is TextBox)
            {
                T_R.Text_Focus_All_Sel((TextBox)sender);
                TextBox tb = null;
                tb = (TextBox)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = cls_app_static_var.txt_Focus_Color;
            }

            if (sender is MaskedTextBox)
            {
                T_R.Text_Focus_All_Sel((MaskedTextBox)sender);
                MaskedTextBox tb = (MaskedTextBox)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = cls_app_static_var.txt_Focus_Color;
            }

            if (this.Controls.ContainsKey("Popup_gr"))
            {
                DataGridView T_Gd = (DataGridView)this.Controls["Popup_gr"];
                T_Gd.Visible = false;
                T_Gd.Dispose();
            }
        }

        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][7]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][10]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][11]

                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }
        private void Set_gr_dic_Line(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][4]                                                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }
        private void Set_gr_dic_Info(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][7]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][10]
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }
        private void Set_gr_dic_talk(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][7]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9]
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }
        private void Set_gr_dic_nomin(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][7]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][10]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][11]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][12]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][13]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][14]
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }
    

        private void _From_Data_Clear()
        {
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 
            //dGridView_Up_S_Header_Reset(dGridView_Up_N); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            cgb_Cacu.d_Grid_view_Header_Reset();
            cgb_Item.d_Grid_view_Header_Reset();
            cgb_save.d_Grid_view_Header_Reset();
            cgb_nomin.d_Grid_view_Header_Reset();
            cgb_Talk.d_Grid_view_Header_Reset();
            cgb_pay.d_Grid_view_Header_Reset();
            cgb_memupc.d_Grid_view_Header_Reset();
            cgb_Change.d_Grid_view_Header_Reset();
            cgb_Rece.d_Grid_view_Header_Reset();
            cgb_add.d_Grid_view_Header_Reset();

            cgb_Re_Pay.d_Grid_view_Header_Reset();



            mtxtMbid.Text = "";
            txtPassword.Text = "";
            txtName.Text = "";
            mtxtBrithDay.Text = "";
            txtName_E_1.Text = "";
            txtName_E_2.Text = "";
            mtxtTel1.Text = "";
            mtxtSn.Text = "";
            mtxtTel2.Text = "";
            mtxtRegDate.Text = "";
            txtEmail.Text = "";
            mtxtZip1.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            mtxtZip2.Text = "";
            txtAddress3.Text = "";
            txtAddress4.Text = "";
            txtBank_Code.Text = "";
            txtName_Accnt.Text = "";
            txtAccount.Text = "";
            txtAccount_Reg.Text = "";
            txtRemark.Text = "";
            txtTalk.Text = "";
            txtLeaveDate.Text = "";
            txtName_C.Text = "";
            mtxtBrithDayC.Text = "";
            txtEmail_C.Text = "";
            mtxtTel2_C.Text = "";
            combo_Se.Text = "";
            txtB1.Text = "";
            
            txtCenter_Code.Text = "";
            txtBank_Code.Text = "";
            txtB1.Text = "";
            txtBank.Text = "";


            txtCenter.Properties.NullText = "";
            txtBank.Properties.NullText = "";
            txtBank.Text = "";

            txtMbid_n.Text = "";
            txtName_n.Text = "";
            txtSN_n.Text = "";
            txtMbid_s.Text = "";
            txtName_s.Text = "";
            txtSN_s.Text = "";
            txtLineCnt.Text = "";

            txtCenter.EditValue = null;
            txtBank.EditValue = null;

            txtName.ReadOnly = false;
            txtName.BackColor = SystemColors.Window;
          //  txtName.BorderStyle = BorderStyle.Fixed3D;

            //txtName.BackColor = Color.FromArgb(236, 241, 220); 
            //txtName.BorderStyle = BorderStyle.Fixed3D; 

            cls_form_Meth ct = new cls_form_Meth();
            mtxtMbid.Text = "";

            opt_sell_2.Checked = true; opt_Bir_TF_1.Checked = true;
            raButt_IN_1.Checked = true;
            check_BankDocument.Checked = false;
            check_CpnoDocument.Checked = false;

            opt_B_1.Checked = false; opt_B_2.Checked = false; opt_B_3.Checked = false;

            chk_N.Checked = false; chk_S.Checked = false;
            idx_Mbid = ""; idx_Mbid2 = 0;
            idx_Password = "";
            txtB1.Text = "0";
            //button_exigo.Enabled = true;
            //button_exigo.Visible = false;

            Reset_Chart_Total();

            combo_Se.Text = ""; combo_Se_Code.Text = "";
            radioB_RBO.Checked = true;
            radioB_G8.Checked = true;

            txtName.Focus();
        }
        private void Reset_Chart_Total()
        {
            //chart_Mem.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();

            if (cls_app_static_var.Using_Mileage_TF == 1)
            {
                double[] yValues = { 0, 0, 0, 0 };
                string[] xValues = { cm._chang_base_caption_search("현금"), cm._chang_base_caption_search("카드"), cm._chang_base_caption_search("무통장"), cm._chang_base_caption_search("마일리지") };
                chart_Mem.Series["Series1"].Points.DataBindXY(xValues, yValues);
            }
            else
            {
                double[] yValues = { 0, 0, 0 };
                string[] xValues = { cm._chang_base_caption_search("현금"), cm._chang_base_caption_search("카드"), cm._chang_base_caption_search("무통장") };
                chart_Mem.Series["Series1"].Points.DataBindXY(xValues, yValues);
            }

           // chart_Mem.Series["Series1"].ChartType = SeriesChartType.Pie;
            chart_Mem.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_Mem.Legends[0].Enabled = true;

            string Tsql = "Select SellCode , SellTypeName ";
            Tsql = Tsql + " From tbl_SellType ";
            Tsql = Tsql + " Order BY SellCode  ";
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_SellType", ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt != 0)
            {
                double[] yValues_2 = new double[ReCnt];
                string[] xValues_2 = new string[ReCnt]; // { cm._chang_base_caption_search(""), cm._chang_base_caption_search("탈퇴") }; 

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    yValues_2[fi_cnt] = 0;
                    xValues_2[fi_cnt] = ds.Tables["tbl_SellType"].Rows[fi_cnt]["SellTypeName"].ToString();
                }

                chart_Leave.Series["Series1"].Points.DataBindXY(xValues_2, yValues_2);

                //chart_Leave.Series["Series1"].ChartType = SeriesChartType.Pie;
                chart_Leave.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                chart_Leave.Legends[0].Enabled = true;
            }



            double[] yValues_3 = { 0, 0 };
            string[] xValues_3 = { cm._chang_base_caption_search("일반"), cm._chang_base_caption_search("WEB") };
            chart_edu.Series["Series1"].Points.DataBindXY(xValues_3, yValues_3);
            //chart_edu.Series["Series1"].ChartType = SeriesChartType.Pie;
            chart_edu.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_edu.Legends[0].Enabled = true;

            chart_Item.Series.Clear();
           // series_Item.Points.Clear();
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox tb = (TextBox)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = Color.White;
            }

            if (sender is MaskedTextBox)
            {
                MaskedTextBox tb = (MaskedTextBox)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = Color.White;
            }
        }

        private void butt_Talk_Click(object sender, EventArgs e)
        {

            if (txtSeq.Text == "")
            {
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            else
            {
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }

            if (Input_Error_Check(mtxtMbid, "m") == false) return; //회원번호 관련 관련 오류 체크

            cls_Check_Text T_R = new cls_Check_Text();
            string me = "";

            me = T_R.Text_Null_Check(txtName, "Msg_Sort_M_Name"); //성명을 필히 넣어야 합니다.
            if (me != "")
            {
                MessageBox.Show(me);
                return;
            }

            me = T_R.Text_Null_Check(txtTalk, "Msg_Sort_Talk"); //상담내역을 필히 넣어야 합니다.
            if (me != "")
            {
                MessageBox.Show(me);
                txtTalk.Focus();
                return;
            }



            cls_Search_DB csd = new cls_Search_DB();

            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

            try
            {
                string StrSql = "";

                if (txtSeq.Text == "")
                {

                    StrSql = "Insert into tbl_Memberinfo_Talk ";
                    StrSql = StrSql + " (";
                    StrSql = StrSql + " Mbid  ";
                    StrSql = StrSql + " , Mbid2 ";
                    StrSql = StrSql + " , TalkContent ";
                    StrSql = StrSql + " , Recordid ";
                    StrSql = StrSql + " , Recordtime ";

                    StrSql = StrSql + ") Values ( ";
                    StrSql = StrSql + "'" + Mbid + "'";
                    StrSql = StrSql + "," + Mbid2;
                    StrSql = StrSql + ",'" + txtTalk.Text.Trim() + "'";
                    StrSql = StrSql + ",'" + cls_User.gid + "'";
                    StrSql = StrSql + ", Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + ")";

                    Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo", Conn, tran, this.Name, this.Text);
                }
                else
                {
                    StrSql = "Insert into tbl_Memberinfo_Talk_Mod Select * ,'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21)  From tbl_Memberinfo_Talk  ";
                    StrSql = StrSql + " Where Seq = " + txtSeq.Text;

                    Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo", Conn, tran, this.Name, this.Text);


                    StrSql = "Update tbl_Memberinfo_Talk Set ";
                    StrSql = StrSql + " TalkContent = '" + txtTalk.Text.Trim() + "'";
                    StrSql = StrSql + " Where Seq = " + txtSeq.Text;


                    Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);
                }
                tran.Commit();
                if (txtSeq.Text == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
                }
                else
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
                }
            }
            catch (Exception)
            {
                tran.Rollback();

                if (txtSeq.Text == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
                }
                else
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Err"));
                }

            }

            finally
            {
                Set_Form_Date_Talk();
                //    tran.Dispose();
                //    Temp_Connect.Close_DB();

                // cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
                // cgbp5.dGridView_Put_baseinfo(this, dGridView_Talk, "talk", mtxtMbid.Text);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void mtxtMbid_KeyPress(object sender, KeyPressEventArgs e)
        {
            //회원번호 관련칸은 소문자를 다 대문자로 만들어 준다.
            if (e.KeyChar >= 97 && e.KeyChar <= 122)
            {
                string str = e.KeyChar.ToString().ToUpper();
                char[] ch = str.ToCharArray();
                e.KeyChar = ch[0];
            }

            if (e.KeyChar == 13)
            {
                TextEdit mtb = (TextEdit)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    int reCnt = 0;
                    cls_Search_DB cds = new cls_Search_DB();
                    string Search_Name = "";
                    reCnt = cds.Member_Name_Search(mtb.Text, ref Search_Name);

                    if (reCnt == 1)
                    {
                        if (mtb.Name == "mtxtMbid")
                        {
                            txtName.Text = Search_Name;
                            if (Input_Error_Check(mtb, "m") == true)
                                Set_Form_Date(mtb.Text, "m");
                            //SendKeys.Send("{TAB}");
                        }
                    }

                    else if (reCnt > 1)  //회원번호 비슷한 사람들이 많은 경우
                    {
                        string Mbid = "";
                        int Mbid2 = 0;
                        cds.Member_Nmumber_Split(mtb.Text, ref Mbid, ref Mbid2);

                        frmBase_Member_Search e_f = new frmBase_Member_Search();

                        if (mtb.Name == "mtxtMbid")
                        {
                            e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                            e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info);
                        }

                        e_f.ShowDialog();

                        SendKeys.Send("{TAB}");
                    }
                    else if (reCnt <= 0)  //동일 회원번호로 사람이 없는 경우에
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Not_Exist")
                         + "\n" +
                         cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    }

                }
                else
                    SendKeys.Send("{TAB}");
            }
        }
        void e_f_Send_MemNumber_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = "";
            cls_Search_DB csb = new cls_Search_DB();
            csb.Member_Nmumber_Split(mtxtMbid.Text.Trim(), ref searchMbid, ref searchMbid2);
        }

        private void butt_Clear_Click(object sender, EventArgs e)
        {
            _From_Data_Clear();
        }

        private void butt_Save_Click(object sender, EventArgs e)
        {
            int Save_Error_Check = 0;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Save_Base_Data(ref Save_Error_Check);

            if (Save_Error_Check > 0)
                _From_Data_Clear();


            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void closebutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonEdit1_EditValueChanged(object sender, EventArgs e)
        {
            frmBase_AddCode e_f = new frmBase_AddCode();
            e_f.Send_Address_Info += new frmBase_AddCode.SendAddressDele(e_f_Send_Address_Info2);
            e_f.ShowDialog();

            txtAddress4.Focus();
        }

        private void butt_AddCode2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            frmBase_AddCode e_f = new frmBase_AddCode();
            e_f.Send_Address_Info += new frmBase_AddCode.SendAddressDele(e_f_Send_Address_Info2);
            e_f.ShowDialog();

            txtAddress4.Focus();
        }

        private void button_Acc_Reg_Click(object sender, EventArgs e)
        {
                Reg_Bank_Account();
        }
        private void Reg_Bank_Account()
        {
            txtAccount_Reg.Text = "";

            lbl_ACC.Text = "미인증";

            string Sn = mtxtSn.Text.Replace("-", "").Replace("_", "").Trim();

            cls_Sn_Check csn_C = new cls_Sn_Check();
            string sort_TF = "";
            bool check_b = false;
            if (raButt_IN_1.Checked == true) //내국인인 구분자
                sort_TF = "in";

            if (raButt_IN_2.Checked == true) //외국인 구분자
                sort_TF = "fo";

            if (raButt_IN_3.Checked == true) //사업자 구분자.
                sort_TF = "biz";

            check_b = csn_C.Sn_Number_Check(Sn, sort_TF);

            Data_Set_Form_TF = 0;

            //if (check_b == false)
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Error")
            //           + "\n" +
            //           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    mtxtSn.Focus(); return;
            //}


            string me = "";

            if (txtAccount.Text == "")
            {
                me = "계좌번호를 필히 입력해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                txtAccount.Focus();
                return;
            }

            if (txtName_Accnt.Text == "")
            {
                me = "예금주를 필히 입력해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                txtName_Accnt.Focus();
                return;
            }


            if (txtBank_Code.Text == "")
            {
                me = "은행을 필히 선택해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                txtBank.Focus();
                return;
            }

            if (mtxtBrithDay.Text.Replace("-", "").Trim() == "")
            {
                me = "생년월일을 필히 선택해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                mtxtBrithDay.Focus();
                return;
            }


            cls_Sn_Check csc = new cls_Sn_Check();

            string successYN = "";

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                //successYN = csc.Bank_Acount_Check(txtName_Accnt.Text, mtxtSn.Text.Substring(0, 6), txtBank_Code.Text, txtAccount.Text);
                successYN = csc.Bank_Acount_Check(txtName_Accnt.Text, mtxtBrithDay.Text.Replace("-", "").Substring(2, 6), txtBank_Code.Text, txtAccount.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("계좌인증 오류");
                //MessageBox.Show(ee.ToString ());
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;

            if (successYN == "Y")
            {
                txtAccount_Reg.Text = txtAccount.Text;
                lbl_ACC.Text = "Success";
                me = "올바른 계좌 정보 입니다. 계좌인증 성공.";
                MessageBox.Show(me);
                txtName_E_1.Focus();
            }
            else
            {
                txtAccount_Reg.Text = "";
                lbl_ACC.Text = "Fail";
                me = "올바른 계좌 정보가 아닙니다. 확인후 다시 시도해 주십시요. 계좌인증 실패.";
                MessageBox.Show(me);
                txtAccount.Focus();
            }



        }

        private void txtCenter_EditValueChanged(object sender, EventArgs e)
        {
          //  txtCenter.EditValue.ToString();
            string displayText = txtCenter.Properties.GetDisplayText(txtCenter.EditValue);
        }

        private void txtBank_EditValueChanged(object sender, EventArgs e)
        {
                //  txtCenter.EditValue.ToString();
                string displayText = txtBank.Properties.GetDisplayText(txtBank.EditValue);
        }

        private void frmMember_Update_dev_Activated(object sender, EventArgs e)
        {
            string Send_Number = ""; string Send_Name = "";
            Take_Mem_Number(ref Send_Number, ref Send_Name);

            if (Send_Number != "")
            {
                mtxtMbid.Text = Send_Number;
                Set_Form_Date(mtxtMbid.Text, "m");
            }


        }




        //private void txtCenter_EditValueChanged(object sender, EventArgs e)
        //{
        //  //  txtCenter_Code.Text = txtCenter.EditValue.ToString();
        //}

        private void Base_Grid_info_Set(int intTemp)
        {
            //20190227 구현호 각종 그리드의 셀렉트문들 내용을 inttemp값대로 출력한다. 예 - 매출은 5
            string T_Mbid = "";
            T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            cls_Search_DB csb = new cls_Search_DB();
            cls_form_Meth cm = new cls_form_Meth();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) != 1) return;

            string Tsql = "";

            Tsql = "Select  ";
            if (intTemp == 1)
            {

                Tsql = Tsql + " SellDate ";
                Tsql = Tsql + " ,OrderNumber ";
                Tsql = Tsql + " ,SellTypeName ";
                //Tsql = Tsql + " ,Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " Ch_Detail ";
                Tsql = Tsql + " ,Case When ReturnTF = 1 Then '" + cm._chang_base_caption_search("정상") + "'";
                Tsql = Tsql + "  When ReturnTF = 2 Then '" + cm._chang_base_caption_search("반품") + "'";
                Tsql = Tsql + "  When ReturnTF = 4 Then '" + cm._chang_base_caption_search("교환") + "'";
                Tsql = Tsql + "  When ReturnTF = 3 Then '" + cm._chang_base_caption_search("부분반품") + "'";
                Tsql = Tsql + "  When ReturnTF = 5 Then '" + cm._chang_base_caption_search("취소") + "'";
                Tsql = Tsql + " END ";

                Tsql = Tsql + " ,TotalPrice ";
                Tsql = Tsql + " ,TotalInputPrice ";
                Tsql = Tsql + " ,TotalPV ";


                Tsql = Tsql + " ,InputCash ";
                Tsql = Tsql + " ,InputCard ";
                Tsql = Tsql + " ,InputPassbook ";
                Tsql = Tsql + " ,Etc1 ";

                Tsql = Tsql + " From tbl_SalesDetail (nolock) ";
                Tsql = Tsql + " Left Join tbl_SellType (nolock) On tbl_SellType.SellCode =tbl_SalesDetail.SellCode ";
                Tsql = Tsql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_SalesDetail' And  Ch_T.M_Detail = Convert(Varchar,tbl_SalesDetail.ReturnTF ) ";
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By OrderNumber ASC ";
            }
            else if (intTemp == 2)
            {

                Tsql = Tsql + " A.ModRecordtime ";
                Tsql = Tsql + " ,Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " Ch_Detail ";
                Tsql = Tsql + " ,BeforeDetail ";
                Tsql = Tsql + " ,AfterDetail ";
                Tsql = Tsql + " ,A.ModRecordid ";

                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";

                Tsql = Tsql + " FROM tbl_Memberinfo_Mod AS A (nolock) ";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo_Mod_Detail Ch_T  (nolock) ON Ch_T.M_Detail = A.ChangeDetail";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS B  (nolock) ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 ";
                Tsql = Tsql + " LEFT JOIN tbl_Business         (nolock) ON B.BusinessCode = tbl_Business.ncode  And B.Na_code = tbl_Business.Na_code ";

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where B.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where b.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   B.Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " And Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " IS NOT NULL ";
                Tsql = Tsql + " Order By Modrecordtime DESC ";



                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;

                cgb_Change.FillGrid(ds.Tables[0]);



                Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
                cgb_Change.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
                cgb_Change.db_grid_Obj_Data_Put();

                foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView7.Columns)
                {
                    //if (new List<string>()
                    //{
                    //    "OrderNumber"  ,"InsuranceNumber", "mbid2"   , "mname",  "cpno","nmbid2", "nmname"
                    //}.Contains(col.Name))
                    //{
                    //    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                    //}
                    //else
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;

                }
            }

            else if (intTemp == 3)
            {
                Tsql = Tsql + " tbl_Memberinfo_Save_Nomin_Change.recordtime ";

                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.Old_mbid + '-' + Convert(Varchar,tbl_Memberinfo_Save_Nomin_Change.Old_mbid2) ";
                else
                    Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.Old_mbid2 ";
                Tsql = Tsql + " ,A.M_name AS oldname ";

                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.New_mbid + '-' + Convert(Varchar,tbl_Memberinfo_Save_Nomin_Change.New_mbid2) ";
                else
                    Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.New_mbid2 ";
                Tsql = Tsql + " ,B.M_name AS Newname";

                Tsql = Tsql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + "  Ch_Detail ";
                Tsql = Tsql + " ,tbl_Memberinfo_Save_Nomin_Change.Recordid ";

                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";


                Tsql = Tsql + " FROM      tbl_Memberinfo_Save_Nomin_Change  (nolock) ";

                Tsql = Tsql + " Left JOIN tbl_Memberinfo A (nolock)  ON";
                Tsql = Tsql + " tbl_Memberinfo_Save_Nomin_Change.Old_mbid = A.mbid ";
                Tsql = Tsql + " And tbl_Memberinfo_Save_Nomin_Change.Old_mbid2 = A.mbid2 ";

                Tsql = Tsql + " Left Join tbl_Memberinfo B (nolock) ON ";
                Tsql = Tsql + " tbl_Memberinfo_Save_Nomin_Change.New_mbid = B.Mbid";
                Tsql = Tsql + " And tbl_Memberinfo_Save_Nomin_Change.New_mbid2 = B.Mbid2";

                Tsql = Tsql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_Memberinfo_Save_Nomin_Change' And  Ch_T.M_Detail = tbl_Memberinfo_Save_Nomin_Change.Save_Nomin_SW ";

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_Memberinfo_Save_Nomin_Change.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_Memberinfo_Save_Nomin_Change.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_Memberinfo_Save_Nomin_Change.Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By tbl_Memberinfo_Save_Nomin_Change.recordtime DESC  ";
            }

            else if (intTemp == 4)
            {

                Tsql = Tsql + " Case When Sort_Add = 'C' Then '" + cm._chang_base_caption_search("직장") + "'";
                Tsql = Tsql + "  When Sort_Add = 'R' Then '" + cm._chang_base_caption_search("기본배송지") + "'";
                Tsql = Tsql + " END ";

                Tsql = Tsql + " ,ETC_Addcode1   ";
                Tsql = Tsql + " ,ETC_Address1 ";
                Tsql = Tsql + " ,ETC_Address2 ";

                Tsql = Tsql + " ,ETC_Tel_1 ";
                Tsql = Tsql + " ,ETC_Tel_2 ";
                Tsql = Tsql + " ,ETC_Name ";


                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";

                Tsql = Tsql + " From tbl_Memberinfo_Address (nolock) ";

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By Sort_Add ASC ";
            }


            else if (intTemp == 5) //20190227 구현호 당사자매출
            {

                Tsql = Tsql + " Case When tbl_SalesDetail.Ga_Order = 0 Then '" + cm._chang_base_caption_search("승인") + "'";
                Tsql = Tsql + "  When tbl_SalesDetail.Ga_Order > 0 Then '" + cm._chang_base_caption_search("미승인") + "'";
                Tsql = Tsql + " END SellTFName ";

                Tsql = Tsql + " ,SellDate ";
                Tsql = Tsql + " ,tbl_SalesDetail.OrderNumber ";
                Tsql = Tsql + " ,SellTypeName ";
                //Tsql = Tsql + " ,Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " Ch_Detail ";
                Tsql = Tsql + " ,Case When ReturnTF = 1 Then '" + cm._chang_base_caption_search("정상") + "'";
                Tsql = Tsql + "  When ReturnTF = 2 Then '" + cm._chang_base_caption_search("반품") + "'";
                Tsql = Tsql + "  When ReturnTF = 4 Then '" + cm._chang_base_caption_search("교환") + "'";
                Tsql = Tsql + "  When ReturnTF = 3 Then '" + cm._chang_base_caption_search("부분반품") + "'";
                Tsql = Tsql + "  When ReturnTF = 5 Then '" + cm._chang_base_caption_search("취소") + "'";
                Tsql = Tsql + " END ";

                Tsql = Tsql + " ,TotalPrice ";
                Tsql = Tsql + " ,TotalInputPrice ";
                Tsql = Tsql + " ,TotalPV ";


                Tsql = Tsql + " ,InputCash ";
                Tsql = Tsql + " ,InputCard ";
                Tsql = Tsql + " ,InputPassbook ";
                Tsql = Tsql + " ,Etc1 ";

                Tsql = Tsql + " From tbl_SalesDetail (nolock) ";
                //Tsql = Tsql + " Left Join tbl_SalesDetail_TF (nolock) On tbl_SalesDetail_TF.OrderNumber =tbl_SalesDetail.OrderNumber ";
                Tsql = Tsql + " Left Join tbl_SellType (nolock) On tbl_SellType.SellCode =tbl_SalesDetail.SellCode ";
                Tsql = Tsql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_SalesDetail' And  Ch_T.M_Detail = Convert(Varchar,tbl_SalesDetail.ReturnTF ) ";
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By OrderNumber ASC ";

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;

                cgb.FillGrid(ds.Tables[0]);



                Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
                cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
                cgb.db_grid_Obj_Data_Put();

                foreach (DevExpress.XtraGrid.Columns.GridColumn col in dGridView_Sell.Columns)
                {
                    //if (new List<string>()
                    //{
                    //    "OrderNumber"  ,"InsuranceNumber", "mbid2"   , "mname",  "cpno","nmbid2", "nmname"
                    //}.Contains(col.Name))
                    //{
                    //    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                    //}
                    //else
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;

                }

            }
            else if (intTemp == 6)
            {

                //cls_form_Meth cm = new cls_form_Meth();
                //string save_C = cm._chang_base_caption_search("후원인_변경");
                //string nom_C = cm._chang_base_caption_search("추천인_변경");

                //Tsql = "Select  ";
                Tsql = Tsql + " tbl_Memberinfo_Save_Nomin_Change.recordtime ";

                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.Old_mbid + '-' + Convert(Varchar,tbl_Memberinfo_Save_Nomin_Change.Old_mbid2) ";
                else
                    Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.Old_mbid2 ";
                Tsql = Tsql + " ,A.M_name AS oldname ";

                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.New_mbid + '-' + Convert(Varchar,tbl_Memberinfo_Save_Nomin_Change.New_mbid2) ";
                else
                    Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.New_mbid2 ";
                Tsql = Tsql + " ,B.M_name AS Newname";

                //      Tsql = Tsql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + "  Ch_Detail ";
                Tsql = Tsql + " , Case When Save_Nomin_SW = 'Sav' Then '후원인 변경' ELSE '추천인 변경' END";
                Tsql = Tsql + " ,tbl_Memberinfo_Save_Nomin_Change.Recordid ";

                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";


                Tsql = Tsql + " FROM      tbl_Memberinfo_Save_Nomin_Change  (nolock) ";

                Tsql = Tsql + " Left JOIN tbl_Memberinfo A (nolock)  ON";
                Tsql = Tsql + " tbl_Memberinfo_Save_Nomin_Change.Old_mbid = A.mbid ";
                Tsql = Tsql + " And tbl_Memberinfo_Save_Nomin_Change.Old_mbid2 = A.mbid2 ";

                Tsql = Tsql + " Left Join tbl_Memberinfo B (nolock) ON ";
                Tsql = Tsql + " tbl_Memberinfo_Save_Nomin_Change.New_mbid = B.Mbid";
                Tsql = Tsql + " And tbl_Memberinfo_Save_Nomin_Change.New_mbid2 = B.Mbid2";

                //Tsql = Tsql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_Memberinfo_Save_Nomin_Change' And  Ch_T.M_Detail = tbl_Memberinfo_Save_Nomin_Change.Save_Nomin_SW ";

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_Memberinfo_Save_Nomin_Change.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_Memberinfo_Save_Nomin_Change.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_Memberinfo_Save_Nomin_Change.Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By tbl_Memberinfo_Save_Nomin_Change.recordtime DESC  ";

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;

                cgb_memupc.FillGrid(ds.Tables[0]);



                Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
                cgb_memupc.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
                cgb_memupc.db_grid_Obj_Data_Put();

                foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView8.Columns)
                {
                    //if (new List<string>()
                    //{
                    //    "OrderNumber"  ,"InsuranceNumber", "mbid2"   , "mname",  "cpno","nmbid2", "nmname"
                    //}.Contains(col.Name))
                    //{
                    //    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                    //}
                    //else
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;

                }
            }
            else if (intTemp == 7)
            {

                //cls_form_Meth cm = new cls_form_Meth();

                Tsql = " Select ST1 ";
                Tsql = Tsql + ", LEFT(ToEndDate,4) +'-' + LEFT(RIGHT(ToEndDate,4),2) + '-' + RIGHT(ToEndDate,2) ";
                Tsql = Tsql + ",LEFT(PayDate,4) +'-' + LEFT(RIGHT(PayDate,4),2) + '-' + RIGHT(PayDate,2) ";

                Tsql = Tsql + " ,SumAllAllowance ";
                Tsql = Tsql + " ,InComeTax ";
                Tsql = Tsql + " ,ResidentTax ";
                Tsql = Tsql + " ,TruePayment ";

                Tsql = Tsql + " ,'','','' ,'' ";
                Tsql = Tsql + " From ";

                Tsql = Tsql + "  ( ";


                Tsql = Tsql + " Select '주간_마감' ST1, PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
                Tsql = Tsql + " From tbl_ClosePay_04_Mod (nolock)  ";
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " And  SumAllAllowance >0 ";

                Tsql = Tsql + " UNION ALL";

                Tsql = Tsql + " Select '월_마감' ST1, PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
                Tsql = Tsql + " From tbl_ClosePay_04_Mod (nolock)  ";
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " And  SumAllAllowance >0 ";

                Tsql = Tsql + " )AS C  ";
                Tsql = Tsql + " Order By PayDate DESC ";


                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;

                cgb_pay.FillGrid(ds.Tables[0]);



                Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
                cgb_pay.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
                cgb_pay.db_grid_Obj_Data_Put();

                foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView9.Columns)
                {
                    //if (new List<string>()
                    //{
                    //    "OrderNumber"  ,"InsuranceNumber", "mbid2"   , "mname",  "cpno","nmbid2", "nmname"
                    //}.Contains(col.Name))
                    //{
                    //    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                    //}
                    //else
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;

                }
            }
            else if (intTemp == 8)
            {
                //cls_form_Meth cm = new cls_form_Meth();


                Tsql = Tsql + " Case When Sort_Add = 'C' Then '" + cm._chang_base_caption_search("직장") + "'";
                Tsql = Tsql + "  When Sort_Add = 'R' Then '" + cm._chang_base_caption_search("기본배송지") + "'";
                Tsql = Tsql + " END ";

                Tsql = Tsql + " ,ETC_Addcode1   ";
                Tsql = Tsql + " ,ETC_Address1 ";
                Tsql = Tsql + " ,ETC_Address2 ";

                Tsql = Tsql + " ,ETC_Tel_1 ";
                Tsql = Tsql + " ,ETC_Tel_2 ";
                Tsql = Tsql + " ,ETC_Name ";


                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";

                Tsql = Tsql + " From tbl_Memberinfo_Address (nolock) ";

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By Sort_Add ASC ";

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;

                cgb_add.FillGrid(ds.Tables[0]);



                Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
                cgb_add.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
                cgb_add.db_grid_Obj_Data_Put();

                foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView10.Columns)
                {
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            else if (intTemp == 9)
            {
                Tsql = Tsql + " TalkContent ";

                Tsql = Tsql + " ,Recordid ";

                Tsql = Tsql + ", Recordtime ";

                Tsql = Tsql + " , Seq ";
                Tsql = Tsql + " , ''   ,'','','','','' ";


                Tsql = Tsql + " From tbl_Memberinfo_Talk (nolock) ";

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order by Seq DESC  ";

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;

                cgb_Talk.FillGrid(ds.Tables[0]);



                Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_gr_dic_talk(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
                cgb_Talk.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
                cgb_Talk.db_grid_Obj_Data_Put();

                foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView6.Columns)
                {
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                }
            }

            else if (intTemp == 10)
            {
                Tsql = Tsql + " T_AA.Lvl ";
                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + ", T_AA.mbid + '-' + Convert(Varchar,T_AA.mbid2) ";
                else
                    Tsql = Tsql + ", T_AA.mbid2 ";

                Tsql = Tsql + " ,Isnull(CC_A.G_Name,'') ";
                Tsql = Tsql + " ,A.M_Name ";
                Tsql = Tsql + " , Case When A.Regtime <> '' Then  LEFT(A.Regtime,4) +'-' + LEFT(RIGHT(A.Regtime,4),2) + '-' + RIGHT(A.Regtime,2) ELSE '' End ";
                Tsql = Tsql + " , Case When A.LeaveDate <> '' Then  LEFT(A.LeaveDate,4) +'-' + LEFT(RIGHT(A.LeaveDate,4),2) + '-' + RIGHT(A.LeaveDate,2) ELSE '' End ";
                Tsql = Tsql + ", Isnull( tbl_Business.name,'') ";


                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " ,A.Saveid + '-' + Convert(Varchar,A.Saveid2) ";
                else
                    Tsql = Tsql + " ,A.Saveid2 ";

                Tsql = Tsql + " , Isnull(b.M_Name,'') ";

                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " ,A.Nominid + '-' + Convert(Varchar,A.Nominid2) ";
                else
                    Tsql = Tsql + " ,A.Nominid2 ";

                Tsql = Tsql + " , Isnull(C.M_Name,'') ";
                Tsql = Tsql + " , A.hometel ";
                Tsql = Tsql + " , A.hptel ";

                Tsql = Tsql + " , '' ";
                Tsql = Tsql + " , A.N_LineCnt ";
                Tsql = Tsql + " From ufn_GetSubTree_NomGroup('" + Mbid + "'," + Mbid2 + ") T_AA ";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS A  (nolock) ON A.Mbid = T_AA.mbid And A.Mbid2 = T_AA.Mbid2   ";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS B  (nolock) ON a.Saveid = b.mbid And a.Saveid2 = b.mbid2   ";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS C  (nolock) ON a.Nominid=c.mbid And a.Nominid2 = c.mbid2   ";
                Tsql = Tsql + " LEFT Join tbl_Business  (nolock) On a.businesscode=tbl_Business.ncode And a.Na_code = tbl_Business.Na_code";
                Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On A.CurGrade=C1.Grade_Cnt ";
                Tsql = Tsql + " Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On CC_A.Mbid = A.Mbid And  CC_A.Mbid2 = A.Mbid2 ";
                Tsql = Tsql + " Where T_AA.Lvl > 0 ";
                Tsql = Tsql + " ORder by Lvl ASC, LEFT(SaveCur,3) ASC   , SaveCur ASC ";

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;

                cgb_nomin.FillGrid(ds.Tables[0]);








                Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_gr_dic_nomin(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
                cgb_nomin.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
                cgb_nomin.db_grid_Obj_Data_Put();

                foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView6.Columns)
                {
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            else if (intTemp == 11)
            {
                Tsql = Tsql + " T_AA.Lvl ";
                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + ", T_AA.mbid + '-' + Convert(Varchar,T_AA.mbid2) ";
                else
                    Tsql = Tsql + ", T_AA.mbid2 ";

                Tsql = Tsql + " ,Isnull(CC_A.G_Name,'') ";
                Tsql = Tsql + " ,A.M_Name ";
                Tsql = Tsql + " , Case When A.Regtime <> '' Then  LEFT(A.Regtime,4) +'-' + LEFT(RIGHT(A.Regtime,4),2) + '-' + RIGHT(A.Regtime,2) ELSE '' End ";
                Tsql = Tsql + " , Case When A.LeaveDate <> '' Then  LEFT(A.LeaveDate,4) +'-' + LEFT(RIGHT(A.LeaveDate,4),2) + '-' + RIGHT(A.LeaveDate,2) ELSE '' End ";
                Tsql = Tsql + ", Isnull( tbl_Business.name,'') ";


                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " ,A.Saveid + '-' + Convert(Varchar,A.Saveid2) ";
                else
                    Tsql = Tsql + " ,A.Saveid2 ";

                Tsql = Tsql + " , Isnull(b.M_Name,'') ";

                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " ,A.Nominid + '-' + Convert(Varchar,A.Nominid2) ";
                else
                    Tsql = Tsql + " ,A.Nominid2 ";

                Tsql = Tsql + " , Isnull(C.M_Name,'') ";
                Tsql = Tsql + " , A.hometel ";
                Tsql = Tsql + " , A.hptel ";

                Tsql = Tsql + " , '' ";


                Tsql = Tsql + " , A.LineCnt ";
                Tsql = Tsql + " From ufn_GetSubTree_MemGroup('" + Mbid + "'," + Mbid2 + ") T_AA ";


                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS A  (nolock) ON A.Mbid = T_AA.mbid And A.Mbid2 = T_AA.Mbid2   ";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS B  (nolock) ON a.Saveid = b.mbid And a.Saveid2 = b.mbid2   ";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS C  (nolock) ON a.Nominid=c.mbid And a.Nominid2 = c.mbid2   ";
                Tsql = Tsql + " LEFT Join tbl_Business  (nolock) On a.businesscode=tbl_Business.ncode And a.Na_code = tbl_Business.Na_code";
                Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On A.CurGrade=C1.Grade_Cnt ";
                Tsql = Tsql + " Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On CC_A.Mbid = A.Mbid And  CC_A.Mbid2 = A.Mbid2 ";
                Tsql = Tsql + " Where T_AA.Lvl > 0 ";
                Tsql = Tsql + " ORder by Lvl ASC, LEFT(SaveCur,3) ASC   , SaveCur ASC ";

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;

                cgb_save.FillGrid(ds.Tables[0]);








                Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_gr_dic_nomin(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
                cgb_save.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
                cgb_save.db_grid_Obj_Data_Put();

                foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView2.Columns)
                {
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            else if (intTemp == 12)
            {

                Tsql = Tsql + " tbl_SalesitemDetail.SalesItemIndex ";
                Tsql = Tsql + " , tbl_SalesitemDetail.ItemCode ";
                Tsql = Tsql + " , tbl_Goods.Name Item_Name ";
                Tsql = Tsql + " , tbl_SalesitemDetail.ItemPrice  ";
                Tsql = Tsql + " , tbl_SalesitemDetail.ItemPV  ";
                Tsql = Tsql + " , tbl_SalesitemDetail.ItemCount  ";
                Tsql = Tsql + " , tbl_SalesitemDetail.ItemTotalPrice  ";
                Tsql = Tsql + " , tbl_SalesitemDetail.ItemTotalPV  ";

                Tsql = Tsql + " ,Case When SellState = 'N_1' Then '" + cm._chang_base_caption_search("정상") + "'";
                Tsql = Tsql + "  When SellState = 'N_3' Then '" + cm._chang_base_caption_search("교환_정상") + "'";
                Tsql = Tsql + "  When SellState = 'R_1' Then '" + cm._chang_base_caption_search("반품") + "'";
                Tsql = Tsql + "  When SellState = 'R_3' Then '" + cm._chang_base_caption_search("교환_반품") + "'";
                Tsql = Tsql + "  When SellState = 'C_1' Then '" + cm._chang_base_caption_search("취소") + "'";
                Tsql = Tsql + " END  SellStateName ";
                Tsql = Tsql + " , tbl_SalesitemDetail.Etc  ";
                Tsql = Tsql + " , tbl_SalesitemDetail.OrderNumber   ";

                Tsql = Tsql + " From tbl_SalesitemDetail (nolock) ";
                Tsql = Tsql + " LEFT JOIN tbl_Goods (nolock) ON tbl_Goods.Ncode = tbl_SalesitemDetail.ItemCode ";


                Tsql = Tsql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_SalesitemDetail.OrderNumber ";
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By tbl_SalesDetail.OrderNumber DESC,  SalesItemIndex ASC ";


                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;

                cgb_Item.FillGrid(ds.Tables[0]);

                Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
                cgb_Item.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
                cgb_Item.db_grid_Obj_Data_Put();

                foreach (DevExpress.XtraGrid.Columns.GridColumn col in dGridView_Sell_Item.Columns)
                {
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                }

            }
            else if (intTemp == 13)
            {
                Tsql = Tsql + " tbl_Sales_Cacu.C_index ";
                Tsql = Tsql + " ,Case When C_TF = 1 Then '" + cm._chang_base_caption_search("현금") + "'";
                Tsql = Tsql + "  When C_TF = 2 Then '" + cm._chang_base_caption_search("무통장") + "'";
                Tsql = Tsql + "  When C_TF = 3 Then '" + cm._chang_base_caption_search("카드") + "'";
                Tsql = Tsql + "  When C_TF = 4 Then '" + cm._chang_base_caption_search("마일리지") + "'";
                Tsql = Tsql + "  When C_TF = 5 Then '" + cm._chang_base_caption_search("가상계좌") + "'";
                Tsql = Tsql + " END  C_TF_Name ";
                Tsql = Tsql + " ,tbl_Sales_Cacu.C_Price1  ";
                Tsql = Tsql + " ,tbl_Sales_Cacu.C_AppDate1  ";
                Tsql = Tsql + " ,Case When Isnull(tbl_Bank.bankname , '') <> '' then Isnull(tbl_Bank.bankname , '') ELSE tbl_Sales_Cacu.C_CodeName END ";
                Tsql = Tsql + " ,tbl_Sales_Cacu.C_Number1  ";
                Tsql = Tsql + " ,tbl_Sales_Cacu.C_Name1  ";
                Tsql = Tsql + " ,tbl_Sales_Cacu.C_Name2  ";
                Tsql = Tsql + " ,tbl_Sales_Cacu.C_Etc  ";
                Tsql = Tsql + " , tbl_Sales_Cacu.OrderNumber   ";
                Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
                Tsql = Tsql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber ";
                Tsql = Tsql + " LEFT JOIN tbl_BankForCompany (nolock) ON tbl_Sales_Cacu.C_Code = tbl_BankForCompany.BankCode And  tbl_Sales_Cacu.C_Number1 = tbl_BankForCompany.BankAccountNumber And tbl_SalesDetail.Na_Code = tbl_BankForCompany.Na_Code  ";

                Tsql = Tsql + " LEFT JOIN tbl_Bank (nolock) ON Right(tbl_Sales_Cacu.C_Code,2)  = Right(tbl_Bank.Ncode,2)  And tbl_Sales_Cacu.C_TF = 5   ";
                cls_NationService.SQL_BankNationCode(ref Tsql);


                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By tbl_SalesDetail.OrderNumber DESC,  C_index ASC ";


                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;

                cgb_Cacu.FillGrid(ds.Tables[0]);

                Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_gr_dic_talk(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
                cgb_Cacu.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
                cgb_Cacu.db_grid_Obj_Data_Put();

                foreach (DevExpress.XtraGrid.Columns.GridColumn col in dGridView_Sell_Item.Columns)
                {
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                }

            }
            else if (intTemp == 14)
            {
                Tsql = "Select ";
                Tsql = Tsql + " tbl_Sales_Rece.SalesItemIndex  ";
                Tsql = Tsql + " ,Case When Receive_Method = 1 Then '" + cm._chang_base_caption_search("직접수령") + "'";
                Tsql = Tsql + "  When Receive_Method = 2 Then '" + cm._chang_base_caption_search("배송") + "'";
                Tsql = Tsql + "  When Receive_Method = 3 Then '" + cm._chang_base_caption_search("센타수령") + "'";
                Tsql = Tsql + "  When Receive_Method = 4 Then '" + cm._chang_base_caption_search("본사직접수령") + "'";
                Tsql = Tsql + " END  Receive_Method_Name ";
                Tsql = Tsql + " ,Get_Date1 ";
                Tsql = Tsql + " ,Get_Name1 ";
                Tsql = Tsql + " ,Get_ZipCode ";
                Tsql = Tsql + " ,Get_Address1 ";
                Tsql = Tsql + " ,Get_Address2 ";
                Tsql = Tsql + " ,Get_Tel1 ";
                Tsql = Tsql + " ,Get_Tel2 ";
                Tsql = Tsql + " ,Get_Etc1 ";
                Tsql = Tsql + " , tbl_Sales_Rece.OrderNumber   ";

                Tsql = Tsql + " From tbl_Sales_Rece (nolock) ";
                Tsql = Tsql + " LEFT JOIN tbl_Base_Rec (nolock) on tbl_Base_Rec.ncode = tbl_Sales_Rece.Base_Rec ";


                Tsql = Tsql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_Sales_Rece.OrderNumber ";
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By tbl_SalesDetail.OrderNumber DESC,  SalesItemIndex ASC ";

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;

                cgb_Rece.FillGrid(ds.Tables[0]);

                Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
                cgb_Rece.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
                cgb_Rece.db_grid_Obj_Data_Put();

                foreach (DevExpress.XtraGrid.Columns.GridColumn col in dGridView_Sell_Item.Columns)
                {
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                }
            }

            else if (intTemp == 15)
            {

                Tsql = "Select  Re_T.clo_ToEndDate ";
                Tsql = Tsql + " , Re_T.Cur_ToEndDate  ";
                Tsql = Tsql + " , Re_T.Ordernumber  ";

                Tsql = Tsql + " , tbl_SalesDetail.Mbid2  ";
                Tsql = Tsql + " , tbl_SalesDetail.M_Name ";

                Tsql = Tsql + " , Re_T.Ded_A_3  ";
                Tsql = Tsql + " , Re_T.Ded_A_6 ";
                Tsql = Tsql + " , Re_T.Ded_A_15 ";

                Tsql = Tsql + " , Re_T.Ded_A_1 ";
                Tsql = Tsql + " , Re_T.Ded_A_2 ";

                Tsql = Tsql + " , Ded_A_3 + Ded_A_6 +  Ded_A_15 + Ded_A_1 +Ded_A_2 ";

                Tsql = Tsql + " , Re_T.TotalPV";
                Tsql = Tsql + ", Case When Re_T.Ded_PV_1 > 0 then Re_T.Ded_PV_1 ELSE  Re_T.Ded_PV_2 END  ";

                Tsql = Tsql + " , Re_T.Re_Cur_PV_1 ";
                Tsql = Tsql + " , Re_T.Re_Cur_PV_2 ";

                Tsql = Tsql + " , Re_T.Req_Mbid_T ";
                Tsql = Tsql + " , Re_T.Req_Pay_T ";


                Tsql = Tsql + " FROM tbl_ClosePay_04_Ded_P_Detail_Mod (nolock)   Re_T  ";
                Tsql = Tsql + " LEFT JOIN tbl_SalesDetail  (nolock) ON Re_T.Ordernumber = tbl_SalesDetail.Ordernumber ";
                Tsql = Tsql + " Where Re_T.Mbid2 ='" + Mbid2 + "'";
                Tsql = Tsql + " And  Ded_A_3 + Ded_A_6 + Ded_A_7+ Ded_A_15 + Ded_A_1 +Ded_A_2  + Re_Cur_PV_1 + Re_Cur_PV_2 > 0 ";

                Tsql = Tsql + " order by Re_T.Cur_ToEndDate  , tbl_SalesDetail.Mbid2  ";


                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;

                cgb_Re_Pay.FillGrid(ds.Tables[0]);

                Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
                cgb_Re_Pay.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
                cgb_Re_Pay.db_grid_Obj_Data_Put();

                foreach (DevExpress.XtraGrid.Columns.GridColumn col in dGridView_Sell_Item.Columns)
                {
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (Keys.F1.Equals(keyData))
            {
                butt_Clear_Click(butt_Clear, null);
            }
            else if (Keys.F2.Equals(keyData))
            {
                butt_Save_Click(null, null);
            }
            else if (Keys.F12.Equals(keyData))
            {
                this.Close();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }





    }
}