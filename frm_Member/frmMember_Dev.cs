﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;

namespace MLM_Program
{
    public partial class frmMember_Dev : DevExpress.XtraEditors.XtraForm
    {
        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
        private const string base_db_name = "tbl_Memberinfo";
        private int Mbid_Number_Hand_Check_TF = 0;

        private int Data_Set_Form_TF;

        Class.DevGridControlService cgb = new Class.DevGridControlService();
        cls_Grid_Base cgb_Sub = new cls_Grid_Base();
        cls_Grid_Base cg_Li = new cls_Grid_Base();
        cls_Grid_Base cgb_Sub_Add = new cls_Grid_Base();

        // public event SendNumberDele Send_Mem_Number;

        public frmMember_Dev()
        {
            InitializeComponent();
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
        private void frmMember_Dev_Load(object sender, EventArgs e)
        {
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            txtB1.Text = "0";
            Data_Set_Form_TF = 0;
            Mbid_Number_Hand_Check_TF = 0;


            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            cls_form_Meth cm = new cls_form_Meth();


            cm.from_control_text_base_chang(this);

            Data_Set_Form_TF = 1;
            mtxtRegDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            Data_Set_Form_TF = 0;

            txtB1.Text = "0";
            

            //위치찾는게 자동이다 그럼 수동 관련된 요소를 닫는다.
            if (cls_app_static_var.Member_Reg_Line_Select_TF == 0)
            {

                txtLineCnt.BackColor = Color.AliceBlue;
                txtLineCnt.ReadOnly = true;
                txtLineCnt.Tag = "";
                //grB_Line.Visible = false;
                //groupBox1.Width = 772;
            }

            //if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
            //{false;
            //    chk_Top_s.Checked = true;
            //}

            //if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            //{
            //    tbl_nom.Visible = false;
            //    chk_Top_n.Checked = true;
            //}
            //    tbl_save.Visible = 

            txtSN_n.BackColor = cls_app_static_var.txt_Enable_Color;
            txtSN_s.BackColor = cls_app_static_var.txt_Enable_Color;

            if (cls_app_static_var.Member_Cpno_Error_Check_TF == 1)
                check_Cpno_Err.Checked = true;

            if (cls_app_static_var.Member_Cpno_Put_TF == 1)
                check_Cpno.Checked = true;

            if (cls_app_static_var.Member_Reg_Multi_TF == 1)
                check_Cpno_Multi.Checked = true;

            if (cls_app_static_var.Mem_Number_Auto_Flag == "R")
            {
                opt_MCode_R.Checked = true;
                mtxtMbid.ReadOnly = true;
                mtxtMbid.BackColor = cls_app_static_var.txt_Enable_Color;
                txtName.Focus();
            }

            if (cls_app_static_var.Mem_Number_Auto_Flag == "A")
            {
                mtxtMbid.ReadOnly = true;
                mtxtMbid.BackColor = cls_app_static_var.txt_Enable_Color;
                opt_MCode_A.Checked = true;
                txtName.Focus();
            }

            if (cls_app_static_var.Mem_Number_Auto_Flag == "H")
            {
                mtxtMbid.ReadOnly = false;
            }

            if (tab_Nation.Visible == true)
            {
                cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
                cpbf.Put_NaCode_ComboBox(combo_Se, combo_Se_Code);
            }

            //button1.Visible = false;
            //if (cls_User.gid == cls_User.SuperUserID)
            //{
            //    button1.Visible = true;
            //}



            //InitCombo();

            ////tab_Sub.TabPages.Remove(tab_C); 부부사업자 사용
            ////tab_Sub.TabPages.Remove(tab_Auto);
            //tab_Sub.TabPages.Remove(tab_Hide);
            //radioB_RBO.Checked = true;
            //radioB_G8.Checked = true;

            //opt_sell_2.Checked = true;
            //if (cls_User.gid == cls_User.SuperUserID)
            //{
            //    button5.Visible = true;
            //}
            //if (cls_User.gid_CC_Save_TF == 0 )  //공동신청인 권한이 없는 사람은 보이지 않게 한다.

            //combo_C_Card_Per.Items.AddRange(data_P);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            butt_Certify.Visible = false;
            button_Acc_Reg.Visible = false;

            InitCombo();

        }


        private void ClearPannel(Control control)
        {
            foreach (Control Edit in control.Controls)
            {
                if (Edit is TextEdit) (Edit as TextEdit).ResetText();
                if (Edit is DateEdit) (Edit as DateEdit).ResetText();
                if (Edit is ComboBoxEdit) (Edit as ComboBoxEdit).ResetText();



                else
                    ClearPannel((Edit));
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_Acc_Reg_Click(object sender, EventArgs e)
        {
            Reg_Bank_Account();
        }

        private void Reg_Bank_Account()
        {
              txtAccount_Reg.Text = "";

          //  lbl_ACC.Text = "미인증";

            string Sn = mtxtSn.Text.Replace("-", "").Replace("_", "").Trim();

            cls_Sn_Check csn_C = new cls_Sn_Check();
            string sort_TF = "";
            bool check_b = false;
            if (raButt_IN_1.Checked == true) //내국인인 구분자
                sort_TF = "in";

            if (raButt_IN_2.Checked == true) //외국인 구분자
                sort_TF = "fo";

            //if (raButt_IN_3.Checked == true) //사업자 구분자.
            //    sort_TF = "biz";

            check_b = csn_C.Sn_Number_Check(Sn, sort_TF);

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

            if (txtBank.EditValue == "")
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

        private void InitCombo()
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            // ** txtCenter 룩업에디트 세팅
            {
                Tsql = "Select  ncode as 'code' , ncode + ' ' + name as '센터이름'";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                //Tsql = Tsql + " Where Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + combo_Se_Code.Text.Trim() + "') )";
                //if (combo_Se_Code.Text.Trim() != "") Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";
                //Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                Tsql = Tsql + " WHERE ShowMemberCenter = 'Y' "; // 2019-04-15 구현호 센터관리 회원관련 센터보여주기에 맞춰서 나옴
            }
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            txtCenter.Properties.DataSource = ds.Tables["t_P_table"].Copy();
            txtCenter.Properties.ValueMember = "code";
            txtCenter.Properties.DisplayMember = "센터이름";


            // ** txtBank **
            cls_Connect_DB Temp_g = new cls_Connect_DB();
            string sql = "";

                sql = "Select ncode as 'code' , Ncode + ' ' + BankName as 은행이름  From tbl_Bank (nolock) ";

            DataSet d = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_g.Open_Data_Set(sql, "t_table", d) == false) return;
            ReCnt = Temp_g.DataSet_ReCount;
            

            txtBank.Properties.DataSource = d.Tables["t_table"].Copy();
            txtBank.Properties.ValueMember = "code";
            txtBank.Properties.DisplayMember = "은행이름";

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m_tb"></param>
        /// <param name="s_Kind"></param>
        /// <param name="SaveIn">세이브진행건인가</param>
        /// <returns></returns>
        private Boolean Input_Error_Check(Control m_tb, string s_Kind, bool SaveIn = false)
        {
            string T_Mbid = m_tb.Text;
            string Mbid = ""; int Mbid2 = 0;
            if (s_Kind == "s")
            {
                txtName_s.Text = ""; txtSN_s.Text = "";
            }
            if (s_Kind == "n")
            {
                txtName_n.Text = ""; txtSN_n.Text = "";
            }

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

            ///
            if (s_Kind == "n" || s_Kind == "s") //3인 경우는 새로운 지정 후원인인데.. 탈퇴나 라인중자가 아닌지를 체크한다.
            {
                if (s_Kind == "n")
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


                    if (chk_Top_s.Checked == false && chk_Top_n.Checked == false)
                    {
                        //입력 추천인 하부의 후원조직상에  입력 후원인이 존재해야 한다.
                        if (csb.Member_Down_Save_TF(m_tb.Text.Trim(), mtxtMbid_n.Text.Trim()) == false)
                        {
                            string Msg = "";

                            if (cls_User.gid_CountryCode == "TH")
                            {
                                Msg = "The sponsor you entered must exist on the sub-sponsorship organization you entered." + "\n" + "Do you want to proceed ?";
                            }
                            else
                            {
                                Msg = "입력하신 추천인 하부 후원조직상에 " + "\n" + "입력하신 후원인이 존재 해야 합니다." + "\n" + " 계속 진행하시겠습니까?";
                            }
                            
                            if (MessageBox.Show(Msg, "", MessageBoxButtons.YesNo) == DialogResult.No)
                            {


                                txtName_s.Text = string.Empty;
                                mtxtMbid_s.Text = string.Empty;

                                m_tb.Focus();

                                return false;
                            }
                            ////Msg_Mem_Down_Nom_Save
                            //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Down_Nom_Save")
                            //      + "\n" +                                 
                            //      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            //m_tb.Focus(); return false;

                        }
                    }
                    if (!SaveIn)
                    {
                        int LineCnt = csb.LineCnt_Search_Save(Mbid, Mbid2);
                        txtLineCnt.Text = LineCnt.ToString();
                        rdoLineLeft.Checked = true;

                        if (!LineCnt.Equals(1))
                        {
                            rdoLineRight.Checked = true;
                        }

                        if (!Check_SaveID_Down())
                        {
                            txtName_s.Text = string.Empty;
                            mtxtMbid_s.Text = string.Empty;
                            return false;
                        }
                    }
                   
                }
            }

            return true;
        }

        private void butt_Save_Click(object sender, EventArgs e)
        {
            int Save_Error_Check = 0; Data_Set_Form_TF = 1;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex;

            //저장이 이루어진다.
            Save_Base_Data(ref Save_Error_Check);  //저장이 이루어진다

            Data_Set_Form_TF = 0;
            if (Save_Error_Check > 0)
            {

                //---------------------------------------------------------
                cls_Search_DB csd = new cls_Search_DB();
                string T_Mbid = mtxtMbid.Text.Trim();
                string Mbid = ""; int Mbid2 = 0;
                csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);





                //Send_SMS_Message_Congratulations_membership(Mbid.ToString(), Mbid2.ToString());




                //cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                //string Sql = "EXEC Usp_Insert_SMS '10', '" + Mbid + "'," + Mbid2 + "','',''";
                //Temp_Connect.Insert_Data(Sql, "tbl_Memberinfo", this.Name, this.Text);

                //new cls_sms().Congratulations_Membership(Mbid2); //: 20190308구현호 - 뭔진 모르지만 구버전이든 여기든 에러남 물어보자
                 



                //EXEC Usp_Insert_SMS '10', '회원번호1', 회원번호2, '', ''
                if (cls_User.gid_SellInput == 1 && cls_app_static_var.Mid_Main_Menu.ContainsKey("m_SellBase"))  //매출창 자동으로 뜨기를 선택한 경우에
                {
                    //cls_Search_DB csd = new cls_Search_DB();

                    //string T_Mbid = mtxtMbid.Text.Trim();

                    string Send_Nubmer = ""; string Send_Name = ""; ; string Send_OrderNumber = "";
                    Send_OrderNumber = "";

                    Send_Nubmer = T_Mbid.ToString();
                    Send_Name = txtName.Text.ToString();
                    //Send_Mem_Number(Send_Nubmer, Send_Name, Send_OrderNumber);   //부모한테 이벤트 발생 신호한다.                        
                }

                Form_Clear_();

                if (cls_app_static_var.Mem_Number_Auto_Flag == "A" || cls_app_static_var.Mem_Number_Auto_Flag == "R")
                    txtName.Focus();
                else
                    mtxtMbid.Focus();
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        //private void Send_SMS_Message_Congratulations_membership(string mbid, string mbid2)
        //{
        //    cls_Connect_DB Temp_Connect = new cls_Connect_DB();
        //    string Tsql;
        //    Tsql = string.Format("EXEC Usp_Insert_SMS '10', '{0}', {1}, '', '' ", mbid, mbid2);

        //    DataSet ds = new DataSet();

        //    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
        //    if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Memberinfo", ds) == false)
        //        return;

        //}
        private void Form_Clear_()
        {

            Data_Set_Form_TF = 1;
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>                
            Base_Grid_Set(); //당일등록 회원을 불러온다.

            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this);

            //opt_sell_2.Checked = true; opt_Bir_TF_1.Checked = true;
            raButt_IN_1.Checked = true;

            // mtxtSn.Mask = "999999-9999999";
            mtxtRegDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            chk_Top_n.Checked = false; chk_Top_s.Checked = false;

            txtB1.Text = "0";
            check_BankDocument.Checked = true;
            check_CpnoDocument.Checked = true;

            //후원추천 기능 사용하지 말라고 하면 최상위로 체크를 해버린다.
            if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
            {
                chk_Top_s.Checked = true;
            }

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                chk_Top_n.Checked = true;
            }


            if (cls_app_static_var.Mem_Number_Auto_Flag == "H")
            {
                mtxtMbid.ReadOnly = false;
            }

            if (cls_app_static_var.Mem_Number_Auto_Flag == "A")
            {
                mtxtMbid.ReadOnly = true;
                mtxtMbid.BackColor = cls_app_static_var.txt_Enable_Color;

            }

            if (cls_app_static_var.Mem_Number_Auto_Flag == "R")
            {
                mtxtMbid.ReadOnly = true;
                mtxtMbid.BackColor = cls_app_static_var.txt_Enable_Color;
            }

            if (cls_app_static_var.Member_Cpno_Error_Check_TF == 1)
                check_Cpno_Err.Checked = true;

            if (cls_app_static_var.Member_Cpno_Put_TF == 1)
                check_Cpno.Checked = true;

            if (cls_app_static_var.Member_Reg_Multi_TF == 1)
                check_Cpno_Multi.Checked = true;
            //tab_Sub.SelectedIndex = 0;

            check_LR.Checked = true;
            radioB_RBO.Checked = true;
            radioB_G8.Checked = true;
            opt_sell_2.Checked = true;

            checkB_SMS_FLAG.Checked = true;
            checkB_EMail_FLAG.Checked = false;

            radioB_Sex_Y.Checked = false;
            radioB_Sex_X.Checked = false;
            Data_Set_Form_TF = 0;




            ClearPannel(this);

            txtCenter.Properties.ValueMember = "";
            //txtBank.Properties.ValueMember = "";

            txtBank.Text = "";

            txtB1.Text = "0";
            txtCenter.EditValue = null;
            txtBank.EditValue = null;
            lbl_ACC.Text = "미인증";
        }
        /// <summary> 동일인물이있는가? </summary>
        private Boolean Check_Duplication_Error()
        {
            //생년월일 중복체크
            //20180807 현재는 CI_DI를 필수적으로 받지않으닌까! 
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT isnull(count(*), 0) cnt");
            sb.AppendLine("FROM tbl_memberinfo ");
            sb.AppendLine("WHERE LeaveCheck = 1 ");
            // sb.AppendLine(string.Format("and Email = '{0}'", txtEmail.Text));
            sb.AppendLine(string.Format("and M_Name = '{0}'", txtName.Text));
            sb.AppendLine(string.Format("and (BirthDay+BirthDay_M+BirthDay_D) = '{0}'", mtxtBrithDay.Text.Replace("-", "")));

            DataSet ds = new DataSet();
            if (Temp_Connect.Open_Data_Set(sb.ToString(), "Check_Join", ds, this.Name, this.Text) == false) return false;
            if (Temp_Connect.DataSet_ReCount == 0) return true;

            int RowValue = 0;
            if (int.TryParse(ds.Tables["Check_Join"].Rows[0][0].ToString(), out RowValue))
            {
                if (RowValue > 0)
                {
                    DialogResult ret = MessageBox.Show(string.Format("{0}님 이름과 생년월일로 중복 체크 결과 {1}명이있는것을 확인했습니다."
                        + Environment.NewLine + "기존 가입 회원입니다." 
                        + Environment.NewLine + "가입 등록하시겠습니까?"
                        , txtName.Text
                        , RowValue), "확인", MessageBoxButtons.OKCancel);
                    return ret == DialogResult.OK;
                    return false;
                }
            }

            return true;
        }
        private Boolean Check_Duplication_Error1()
        {
            //핸드폰중복체크
            //20180807 현재는 CI_DI를 필수적으로 받지않으닌까! 
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT isnull(count(*), 0) cnt");
            sb.AppendLine("FROM tbl_memberinfo ");
            sb.AppendLine("WHERE LeaveCheck = 1 ");
            // sb.AppendLine(string.Format("and Email = '{0}'", txtEmail.Text));
            sb.AppendLine(string.Format("and M_Name = '{0}'", txtName.Text));
            sb.AppendLine(string.Format("and hptel = '{0}'", mtxtTel2.Text.Trim().Replace("_", "")));

            DataSet ds = new DataSet();
            if (Temp_Connect.Open_Data_Set(sb.ToString(), "Check_Join", ds, this.Name, this.Text) == false) return false;
            if (Temp_Connect.DataSet_ReCount == 0) return true;

            int RowValue = 0;
            if (int.TryParse(ds.Tables["Check_Join"].Rows[0][0].ToString(), out RowValue))
            {
                if (RowValue > 0)
                {
                    DialogResult ret = MessageBox.Show(string.Format("{0}님 이름과 핸드폰번호로 중복 체크 결과 {1}명이있는것을 확인했습니다."
                        + Environment.NewLine + "기존 가입 회원입니다."
                        + Environment.NewLine + "가입 등록하시겠습니까?"
                        , txtName.Text
                        , RowValue), "확인", MessageBoxButtons.OKCancel);

                    return ret == DialogResult.OK;
                }
            }

            return true;
        }
        private Boolean Check_Duplication_Error2()
        {
            //집전화중복체크
            //20180807 현재는 CI_DI를 필수적으로 받지않으닌까! 
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT isnull(count(*), 0) cnt");
            sb.AppendLine("FROM tbl_memberinfo ");
            sb.AppendLine("WHERE LeaveCheck = 1 ");
            // sb.AppendLine(string.Format("and Email = '{0}'", txtEmail.Text));
            sb.AppendLine(string.Format("and M_Name = '{0}'", txtName.Text));

            //20190528 구현호 : 만약 집전화번호 없이 저장했을시에 이름이 같은 비교대상이 똑같이 집전화번호가 없다면? 
            //집전화번호 없이 저장한다 해도 임의로 전화번호를 생성하여 이름이 중첩되고 집전화번호가 없는 대상과 비교되도록 해야한다.
            if (mtxtTel1.Text.Replace("_", "") == "")
            {
                return true;//집전화번호를 000-0000-0000으로 저장할 리는 없으니 리턴한다.
            }
            else
            {
                sb.AppendLine(string.Format("and hometel = '{0}'", mtxtTel1.Text.Replace("_", "")));
            }
            
            DataSet ds = new DataSet();
            if (Temp_Connect.Open_Data_Set(sb.ToString(), "Check_Join", ds, this.Name, this.Text) == false) return false;
            if (Temp_Connect.DataSet_ReCount == 0) return true;

            int RowValue = 0;
            if (int.TryParse(ds.Tables["Check_Join"].Rows[0][0].ToString(), out RowValue))
            {
                if (RowValue > 0)
                {
                    DialogResult ret = MessageBox.Show(string.Format("{0}님 이름과 집전화번호로 중복 체크 결과 {1}명이있는것을 확인했습니다." 
                        + Environment.NewLine + "기존 가입 회원입니다."
                        + Environment.NewLine + "가입 등록하시겠습니까?"
                        , txtName.Text
                        , RowValue), "확인", MessageBoxButtons.OKCancel);

                    return ret == DialogResult.OK;
                }
            }

            return true;
        }
        private Boolean Check_Duplication_Error_OutMember() //탈퇴회원 
        {
            //생년월일 중복체크
            //20180807 현재는 CI_DI를 필수적으로 받지않으닌까! 
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT isnull(count(*), 0) cnt");
            sb.AppendLine("FROM tbl_memberinfo ");
            sb.AppendLine("WHERE LeaveCheck = 0 ");
            // sb.AppendLine(string.Format("and Email = '{0}'", txtEmail.Text));
            sb.AppendLine(string.Format("and M_Name = '{0}'", txtName.Text));
            sb.AppendLine(string.Format("and (BirthDay+BirthDay_M+BirthDay_D) = '{0}'", mtxtBrithDay.Text.Replace("-", "")));

            DataSet ds = new DataSet();
            if (Temp_Connect.Open_Data_Set(sb.ToString(), "Check_Join", ds, this.Name, this.Text) == false) return false;
            if (Temp_Connect.DataSet_ReCount == 0) return true;

            int RowValue = 0;
            if (int.TryParse(ds.Tables["Check_Join"].Rows[0][0].ToString(), out RowValue))
            {
                if (RowValue > 0)
                {
                    DialogResult ret = MessageBox.Show(string.Format("{0}님 이름과 생년월일로 중복 체크 결과 {1}명이있는것을 확인했습니다."
                        + Environment.NewLine + "탈퇴 회원입니다."
                        + Environment.NewLine + "가입 등록하시겠습니까?"
                        , txtName.Text
                        , RowValue), "확인", MessageBoxButtons.OKCancel);
                    return ret == DialogResult.OK;
                }
            }

            return true;
        }
        private Boolean Check_Duplication_Error1_OutMember() //탈퇴회원 
        {
            //핸드폰중복체크
            //20180807 현재는 CI_DI를 필수적으로 받지않으닌까! 
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT isnull(count(*), 0) cnt");
            sb.AppendLine("FROM tbl_memberinfo ");
            sb.AppendLine("WHERE LeaveCheck = 0 ");
            // sb.AppendLine(string.Format("and Email = '{0}'", txtEmail.Text));
            sb.AppendLine(string.Format("and M_Name = '{0}'", txtName.Text));
            sb.AppendLine(string.Format("and hptel = '{0}'", mtxtTel2.Text.Trim().Replace("_", "")));

            DataSet ds = new DataSet();
            if (Temp_Connect.Open_Data_Set(sb.ToString(), "Check_Join", ds, this.Name, this.Text) == false) return false;
            if (Temp_Connect.DataSet_ReCount == 0) return true;

            int RowValue = 0;
            if (int.TryParse(ds.Tables["Check_Join"].Rows[0][0].ToString(), out RowValue))
            {
                if (RowValue > 0)
                {
                    DialogResult ret = MessageBox.Show(string.Format("{0}님 이름과 핸드폰번호로 중복 체크 결과 {1}명이있는것을 확인했습니다."
                        + Environment.NewLine + "탈퇴 회원입니다."
                        + Environment.NewLine + "가입 등록하시겠습니까?"
                        , txtName.Text
                        , RowValue), "확인", MessageBoxButtons.OKCancel);

                    return ret == DialogResult.OK;
                }
            }

            return true;
        }
        private Boolean Check_Duplication_Error2_OutMember() //탈퇴회원 
        {
            //집전화중복체크
            //20180807 현재는 CI_DI를 필수적으로 받지않으닌까! 
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT isnull(count(*), 0) cnt");
            sb.AppendLine("FROM tbl_memberinfo ");
            sb.AppendLine("WHERE LeaveCheck = 0 ");
            // sb.AppendLine(string.Format("and Email = '{0}'", txtEmail.Text));
            sb.AppendLine(string.Format("and M_Name = '{0}'", txtName.Text));

            //20190528 구현호 : 만약 집전화번호 없이 저장했을시에 이름이 같은 비교대상이 똑같이 집전화번호가 없다면? 
            //집전화번호 없이 저장한다 해도 임의로 전화번호를 생성하여 이름이 중첩되고 집전화번호가 없는 대상과 비교되도록 해야한다.
            if (mtxtTel1.Text.Replace("_", "") == "")
            {
                return true;//집전화번호를 000-0000-0000으로 저장할 리는 없으니 리턴한다.
            }
            else
            {
                sb.AppendLine(string.Format("and hometel = '{0}'", mtxtTel1.Text.Replace("_", "")));
            }

            DataSet ds = new DataSet();
            if (Temp_Connect.Open_Data_Set(sb.ToString(), "Check_Join", ds, this.Name, this.Text) == false) return false;
            if (Temp_Connect.DataSet_ReCount == 0) return true;

            int RowValue = 0;
            if (int.TryParse(ds.Tables["Check_Join"].Rows[0][0].ToString(), out RowValue))
            {
                if (RowValue > 0)
                {
                    DialogResult ret = MessageBox.Show(string.Format("{0}님 이름과 집전화번호로 중복 체크 결과 {1}명이있는것을 확인했습니다."
                        + Environment.NewLine + "탈퇴 회원입니다."
                        + Environment.NewLine + "가입 등록하시겠습니까?"
                        , txtName.Text
                        , RowValue), "확인", MessageBoxButtons.OKCancel);

                    return ret == DialogResult.OK;
                }
            }

            return true;
        }


        private bool Sn_Number_(string Sn, Control ctrl)
        {
            if (ctrl.Name == "mtxtSn")
            {
                if (raButt_IN_1.Checked == true) //내국인인 경우에는 주민번호 체크한다.
                {
                    string BirthDay2 = "";

                    string Sn_Recovery = Sn;
                    if (mtxtBrithDay.Text.Replace("-", "").Trim().Equals(string.Empty) == false)
                    {
                        string JuminLast = string.Empty;
                        string year = mtxtBrithDay.Text.Substring(0, 4);
                        int nYear = 0;
                        if (int.TryParse(year, out nYear))
                        {
                            JuminLast = nYear >= 2000 ? "3234567" : "1234567";

                        }

                        Sn = mtxtBrithDay.Text.Replace("-", "").Substring(2, 6) + JuminLast;
                    }


                    cls_Sn_Check csn_C = new cls_Sn_Check();
                    if (csn_C.check_19_nai(Sn, ref BirthDay2) == false) //한국같은 경우에는 미성년자 필히 체크한다.
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_19")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        ctrl.Focus(); return false;
                    }


                    if ((mtxtBrithDay.Text.Replace("-", "").Trim() == "" || mtxtBrithDay.Text.Replace("-", "").Trim().Length != 8) && BirthDay2 != "")
                        mtxtBrithDay.Text = BirthDay2;

                    if (Sn.Length >= 7)
                    {
                        int Sex = int.Parse(Sn.Substring(6, 1));

                        //if ((Sex % 2) == 0) radioB_Sex_X.Checked = true;
                        //if ((Sex % 2) == 1) radioB_Sex_Y.Checked = true;
                    }


                    Sn = Sn_Recovery;

                }
            }
            else if (Sn != "")
            {
                string sort_TF = "";
                bool check_b = false;
                cls_Sn_Check csn_C = new cls_Sn_Check();

                if (ctrl.Name == "mtxtSn")
                {
                    if (raButt_IN_1.Checked == true) //내국인인 구분자
                        sort_TF = "in";

                    if (raButt_IN_2.Checked == true) //외국인 구분자
                        sort_TF = "fo";

                    //if (raButt_IN_3.Checked == true) //사업자 구분자.
                    //    sort_TF = "biz";
                }
                else
                {
                    //if (raButt_IN_1_C.Checked == true) //내국인인 구분자
                    //    sort_TF = "in";

                    //if (raButt_IN_2_C.Checked == true) //외국인 구분자
                    //    sort_TF = "fo";

                }

                check_b = csn_C.Sn_Number_Check(Sn, sort_TF);

                if (check_b == false && sort_TF != "fo")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Error")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    ctrl.Focus(); return false;
                }
                else
                {
                    if (cls_app_static_var.Member_Reg_Multi_TF == 0) //다구좌 불가능으로 해서 체크되어 잇는 경우
                    {//동일 주민번호로 해서 가입한 사람이 있는지를 체크한다.
                        cls_Search_DB csb = new cls_Search_DB();
                        if (csb.Member_Multi_Sn_Search(Sn) == false) //주민번호 오류는 위에서 체크를 함.
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Same")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            ctrl.Focus(); return false;
                        }
                    }
                }


                //배우자관련기능 - 아직미구현
                //if (ctrl.Name == "mtxtSn_C")
                //{
                //    if (raButt_IN_1_C.Checked == true && check_b == true) //내국인인 경우에는 주민번호 체크한다.
                //    {
                //        string BirthDay2 = "";
                //        if (csn_C.check_19_nai(Sn, ref BirthDay2) == false) //한국같은 경우에는 미성년자 필히 체크한다.
                //        {
                //            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_19")
                //           + "\n" +
                //           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                //            mtb.Focus(); return false;
                //        }


                //        if (mtxtBrithDayC.Text.Replace("-", "").Trim() == "" || mtxtBrithDayC.Text.Replace("-", "").Trim().Length != 8 && BirthDay2 != "")
                //            mtxtBrithDayC.Text = BirthDay2;

                //    }
                //}

            }
            else
            {
                if (cls_app_static_var.Member_Cpno_Put_TF == 1) //주민번호 관련 필수입력인데 입력 안햇다.
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Put")
                            + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    ctrl.Focus(); return false;
                }
            }

            return true;

        }
        private bool Sn_Number_(string Sn, Control ctrl, string sort_TF, int t_Sort2 = 0)
        {
            if (Sn != "")
            {

                bool check_b = false;
                cls_Sn_Check csn_C = new cls_Sn_Check();

                //sort_TF = "biz";  //사업자번호체크
                //sort_TF = "Tel";  //전화번호체크
                //sort_TF = "Zip";  //우편번호체크

                if (sort_TF == "Date")
                {
                    string[] date_a = ctrl.Text.Split('-');

                    if (date_a.Length >= 3 && date_a[0].Trim() != "" && date_a[1].Trim() != "" && date_a[2].Trim() != "")
                    {
                        string Date_YYYY = "0000" + int.Parse(date_a[0]).ToString();

                        date_a[0] = Date_YYYY.Substring(Date_YYYY.Length - 4, 4);

                        if (int.Parse(date_a[1]) < 10)
                            date_a[1] = "0" + int.Parse(date_a[1]).ToString();

                        if (int.Parse(date_a[2]) < 10)
                            date_a[2] = "0" + int.Parse(date_a[2]).ToString();

                        ctrl.Text = date_a[0] + '-' + date_a[1] + '-' + date_a[2];

                        cls_Check_Input_Error c_er = new cls_Check_Input_Error();
                        if (ctrl.Text.Replace("-", "").Trim() != "")
                        {
                            int Ret = 0;
                            Ret = c_er.Input_Date_Err_Check(ctrl);

                            if (Ret == -1)
                            {
                                ctrl.Focus(); return false;
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Date")
                           + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        ctrl.Focus(); return false;
                    }
                }


                check_b = csn_C.Number_NotInput_Check(ctrl.Text, sort_TF);

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

                    ctrl.Focus(); return false;
                }
            }

            return true;
        }

        private Boolean Check_TextBox_ETC_Error()
        {

            string Sn = mtxtSn.Text.Replace("-", "").Replace("_", "").Trim();
            if (Sn_Number_(Sn, mtxtSn) == false) return false;   //주민번호 입력 사항에 대해서 체크를 한다.


            //if (txtWebID.Text != "")  //웹아이디가 등록 되는 경우에는 유일한 값인지 체크한다.
            //{
            //    //++++++++++++++++++++++++++++++++
            //    cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            //    string Tsql;
            //    Tsql = "Select Mbid,Mbid2  ";
            //    Tsql = Tsql + " From tbl_Memberinfo  (nolock)  ";
            //    Tsql = Tsql + " Where Webid = '" + txtWebID.Text.Trim() + "' ";

            //    DataSet ds = new DataSet();
            //    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //    if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Memberinfo", ds) == true)
            //    {
            //        int ReCnt = Temp_Connect.DataSet_ReCount;
            //        if (ReCnt > 0)
            //        {
            //            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Webid_Not")
            //            + "\n" +
            //            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //            txtWebID.Focus();
            //            return false;
            //        }
            //    }
            //}

            if (cls_User.gid_For_Save_TF != 1 && raButt_IN_2.Checked == true)
            {
                MessageBox.Show("외국인 등록 권한 없습니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                raButt_IN_2.Focus();
                return false;
            }

            //후원최상위로 입력되는 경우네는 강제로 1을 넣는다 라인에
            if (chk_Top_s.Checked == true)
            {
                txtLineCnt.Text = "1";
                rdoLineLeft.Checked = true;
            }

            cls_Check_Text T_R = new cls_Check_Text();
            string me = "";


            me = T_R.Text_Null_Check(txtName, "Msg_Sort_M_Name"); //성명을 필히 넣어야 합니다.
            if (me != "")
            {
                MessageBox.Show(me);
                return false;
            }

            if (radioB_Sex_X.Checked == false && radioB_Sex_Y.Checked == false)
            {
                MessageBox.Show("성별을 선택해주시기바랍니다.");
                radioB_Sex_X.Focus();
                return false;
            }

            if (txtName_Accnt.Text != "" && txtName_Accnt.Text.Trim() != txtName.Text.Trim())
            {
                if (MessageBox.Show("입력하신 회원명과 예금주명이 동일하지 않습니다. 계속 진행하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    return false;

            }

            //생년월일을 주민벚호 기준으로 넣는다.
            if (mtxtBrithDay.Text.Replace("-", "") == "" || mtxtBrithDay.Text.Replace("-", "").Length != 8)
            {
                string BirthDay2 = "";
                Sn = mtxtSn.Text.Replace("-", "").Replace("_", "").Trim();

                if (Sn != "")
                {
                    cls_Sn_Check csn_C = new cls_Sn_Check();
                    if (csn_C.check_19_nai(Sn, ref BirthDay2) == false)

                        mtxtBrithDay.Text = BirthDay2;
                }
            }

            //생년월일을 필수 값으로 지정 했음
            if (mtxtBrithDay.Text.Replace("-", "") == "" || mtxtBrithDay.Text.Replace("-", "").Length != 8)
            {
                me = cls_app_static_var.app_msg_rm.GetString("Msg_Sort_BirthDay") + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                mtxtBrithDay.Focus();
                return false;
            }

            ////센타를 필수 값으로 지정하기로함.
            if (txtCenter.Text == "")
            {
                me = "센터를 필히 선택해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                txtCenter.Focus();
                return false;
            }

            ////First 영문이름 
            //if (txtName_E_1.Text == "")
            //{
            //    me = cls_app_static_var.app_msg_rm.GetString("Msg_Sort_E_Name_F") + "\n" +
            //     cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

            //    MessageBox.Show(me);
            //    txtName_E_1.Focus();
            //    return false;
            //}

            ////Last 영문이름
            //if (txtName_E_2.Text == "")
            //{
            //    me = cls_app_static_var.app_msg_rm.GetString("Msg_Sort_E_Name_L") + "\n" +
            //     cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

            //    MessageBox.Show(me);
            //    txtName_E_2.Focus();
            //    return false;
            //}




            if (mtxtRegDate.Text.Replace("-", "") == "") //등록일자가 빈칸으로 되어 잇으면 당일을 셋팅한다.
                mtxtRegDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ////집전화
            //Sn = mtxtTel1.Text.Replace("-", "").Replace("_", "").Trim();
            //if (Sn_Number_1(Sn, mtxtTel1, "Tel") == false)
            //{
            //    mtxtTel1.Focus();
            //    return false;
            //}

            ////핸드폰
            //Sn = mtxtTel2.Text.Replace("-", "").Replace("_", "").Trim();
            //if (Sn_Number_1(Sn, mtxtTel2, "HpTel") == false)
            //{
            //    mtxtTel2.Focus();
            //    return false;
            //}

            ////이메일
            //Sn = txtEmail.Text.Replace("-", "").Replace("_", "").Trim();
            //if (Sn_Number_1(Sn, txtEmail, "Email") == false)
            //{
            //    txtEmail.Focus();
            //    return false;
            //}

            ////집주소
            //Sn = mtxtZip1.Text.Replace("-", "").Replace("_", "").Trim();
            //if (Sn_Number_1(Sn, mtxtZip1, "Zip") == false)
            //{
            //    mtxtZip1.Focus();
            //    return false;
            //}



            if (chk_Top_n.Checked == true)
            {
                if (txtName_n.Text != "" || txtSN_n.Text != "" || mtxtMbid_n.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Up_Checked")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Nomin")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtMbid_n.Focus();
                    return false;
                }
            }

            if (chk_Top_s.Checked == true)
            {
                if (txtName_s.Text != "" || txtSN_s.Text != "" || mtxtMbid_s.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Up_Checked")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Save")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtMbid_s.Focus();
                    return false;
                }
            }

            ////계좌인증 
            //if (lbl_ACC.Text.Equals("Success") == false)
            //{
            //    if (MessageBox.Show("계좌 미인증 상태 입니다. 미인증 상태에서 회원정보를 저장하시겠습니까?", "계좌인증 확인"
            //        , MessageBoxButtons.YesNo) == DialogResult.No)
            //    {
            //        txtAccount.Focus();
            //        return false;
            //    }
            //}

            ////휴대폰인증
            //if (txt_IpinCI.Text.Equals(string.Empty) && txt_IpinDI.Text.Equals(string.Empty))
            //{
            //    if (MessageBox.Show("휴대폰 미인증 상태 입니다. 미인증 상태에서 회원정보를 저장하시겠습니까?", "휴대폰인증확인"
            //        , MessageBoxButtons.YesNo) == DialogResult.No)
            //    {
            //        txtAccount.Focus();
            //        return false;
            //    }
            //}


            //후원인 라인 선택 
            if (cls_app_static_var.Member_Reg_Line_Select_TF == 1) //위치를 선택하는 옵션인 경우에
            {

                if (rdoLineRight.Checked.Equals(false) && rdoLineLeft.Checked.Equals(false))
                {
                    MessageBox.Show("좌우 위치를 필히 선택 해 주십시요. 1 좌측   2 우측 입니다."
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtWebID.Focus();
                    return false;
                }

            }

            if (txt_Name_Check.Text == "")
            {
                if (MessageBox.Show("간편체크 미인증 상태 입니다. 성명 미인증 상태에서 회원정보를 저장하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.No) return false;
            }


            return true;
        }

        private bool Check_TextBox_Error_Date()
        {


            cls_Check_Input_Error c_er = new cls_Check_Input_Error();
            if (mtxtRegDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtRegDate.Text, mtxtRegDate, "Date") == false)
                {
                    mtxtRegDate.Focus();
                    return false;
                }
            }

            if (mtxtBrithDay.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtBrithDay.Text, mtxtBrithDay, "Date") == false)
                {
                    mtxtBrithDay.Focus();
                    return false;
                }
            }



            return true;
        }

        private Boolean Check_TextBox_Error(bool SaveIn = false)
        {

            ////if (mtxtMbid_n.Text.Replace("-", "").Replace("_", "").Replace(" ", "") == "" && txtName_n.Text == "" && txtSN_n.Text == "")
            ////    chk_Top_n.Checked = true ;

            ////if (mtxtMbid_s.Text.Replace("-", "").Replace("_", "").Replace(" ", "") == "" && txtName_s.Text == "" && txtSN_s.Text == "")
            ////    chk_Top_s.Checked = true ;


            if (tab_Nation.Visible == true)
            {
                if (combo_Se_Code.Text == "")  //다국어 지원프로그램을 사용시 국가는 필히 선택을 해야 된다.
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Not_Na_Code")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    combo_Se.Focus(); return false;
                }
            }

            if (chk_Top_n.Checked == false)
                if (Input_Error_Check(mtxtMbid_n, "n", SaveIn) == false) return false;  //추천인 관련 오류 체크  

            if (chk_Top_s.Checked == false)
                if (Input_Error_Check(mtxtMbid_s, "s", SaveIn) == false) return false; //후원인 관련 오류 체크                        


            //2017-05-02 김종국 이사 요청에 의해서 추천인 최상위 막음 메일로 요청옴
            //if (radioB_Begin.Checked == true)
            //{
            //    if (chk_Top_n.Checked == false)
            //    {
            //        MessageBox.Show("비긴즈는 추천인을 지정할수 없습니다.  추천인 최상위에 체크하신후에 다시 시도해 주십시요."
            //          + "\n" +
            //          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

            //        chk_Top_n.Focus(); return false;
            //    }
            //}


            cls_Search_DB csd = new cls_Search_DB();
            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

            Boolean Top_Check_TF = chk_Top_n.Checked; //기본적으로 추천인 기준으로 해서 번호를 새로 따지만 추천선택이 최상위로 했다.
            string Base_Up_Number = mtxtMbid_n.Text.Trim();

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                Top_Check_TF = chk_Top_s.Checked; //추천인 기능 사용안하므로 후원인쪽에서 연결을 찾는다.
                Base_Up_Number = mtxtMbid_s.Text.Trim();
            }


            if (Top_Check_TF == true)
            {
                if (Mbid2 == 0) //입력된 회원번호가 없다.
                {
                    if (cls_app_static_var.Mem_Number_Auto_Flag == "A")  //회원번호 자동 생성
                    {
                        mtxtMbid.Text = csd.Auto_Member_Number_Search(Base_Up_Number);
                    }

                    if (cls_app_static_var.Mem_Number_Auto_Flag == "R")  //회원번호 랜덤 생성
                    {
                        mtxtMbid.Text = csd.Auto_Member_Number_Search_Random(Base_Up_Number);
                    }

                    if (cls_app_static_var.Mem_Number_Auto_Flag == "H")  //회원번호가 수동 생성
                    {
                        //수동인데 입력된 회원번호가 없거나 올바르지 않은 경우 만들어 줄지를 물보고 승낙하면 만들어 준다.
                        if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Hand_Not_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            mtxtMbid.Focus();
                            return false;
                        }
                        mtxtMbid.Text = csd.Auto_Member_Number_Search(Base_Up_Number);
                    }
                }
            }

            else
            {
                if (Mbid2 == 0) //자동번호 부여인데 입력된 회원번호가 없다.
                {
                    if (cls_app_static_var.Mem_Number_Auto_Flag == "A")  //회원번호 자동 생성
                    {
                        string N_Mbid = ""; int N_Mbid2 = 0;

                        //회원번호 자동인 경우에는 추천인 번호 또는 후원인 번호 를 받아서 하는데 최상위도 아닌데 추천인 번호 또는 후원인 번호가 오류가 나면 메시지를 뛰운다.
                        if (csd.Member_Nmumber_Split(Base_Up_Number, ref N_Mbid, ref N_Mbid2) < 0)
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Auto_Nomin")
                                + "\n" +
                                cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            mtxtMbid.Focus();
                            return false;
                        }
                        mtxtMbid.Text = csd.Auto_Member_Number_Search(Base_Up_Number);
                    }

                    if (cls_app_static_var.Mem_Number_Auto_Flag == "R")  //회원번호 랜덤 생성인 경우 만들어 준다.
                    {
                        mtxtMbid.Text = csd.Auto_Member_Number_Search_Random(Base_Up_Number);
                    }

                    if (cls_app_static_var.Mem_Number_Auto_Flag == "H")  //회원번호가 수동 생성
                    {
                        //수동인데 입력된 회원번호가 없거나 올바르지 않은 경우 만들어 줄지를 물보고 승낙하면 만들어 준다.
                        if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Hand_Not_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            mtxtMbid.Focus();
                            return false;
                        }
                        mtxtMbid.Text = csd.Auto_Member_Number_Search(Base_Up_Number);
                    }
                }
                else //최상위도 선택을 하지 않았고 회원번호가 입려이 되어 있다.
                {
                    if (cls_app_static_var.Mem_Number_Auto_Flag == "H" && Mbid_Number_Hand_Check_TF == 0)  //회원번호가 수동 생성셋팅인데 회원번호를 입력하지 않았다.
                    {

                        string Hand_M_Number = csd.Auto_Member_Number_Search_Hand(mtxtMbid.Text.Trim());

                        if (Hand_M_Number == "")
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Not")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            mtxtMbid.Focus();
                            return false;
                        }
                        else
                            mtxtMbid.Text = Hand_M_Number;
                    }
                    //else
                    //{
                    //    //입력된 회원번호가 이미 등록된 번호인지를 체크한다.
                    //    cls_Search_DB cds = new cls_Search_DB();
                    //    string Search_Name = cds.Member_Search_Base(T_Mbid); //회원번호가 이미 존재하는 번호인지를 체크한다. 존재하는 번호이면 그 번호의 회원명을 돌려줌

                    //    if (Search_Name == "")
                    //    {
                    //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Not")
                    //       + "\n" +
                    //       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    //        mtxtMbid.Focus();
                    //        return false;
                    //    }
                    //}
                }
            }




            return true;

        }

        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
            //if (Check_Certify_Error() == false) return; //회원 핸드폰인증이 되지않으면 탈출

            if (Check_TextBox_ETC_Error() == false) return;  //전화번호 웹아이디 주민번호 같은 부가적인 입력 사항에 대한 오류를 체크한다.
            if (Check_TextBox_Error_Date() == false) return; //날짜 관련 텍스트 파일들에 대해서 날짜 오류를 체크한다
            #region * * * * * * * * * * * *  회원 후원인체크
            int CheckLineCnt = rdoLineLeft.Checked ? 1 : 2;
            string Qry = "SELECT mbid2, m_name FROM tbl_Memberinfo (nolock) WHERE SaveId2 = " + mtxtMbid_s.Text.Replace("_", "").Trim();
            Qry += " AND LineCnt = " + CheckLineCnt;
            cls_Connect_DB conn1 = new cls_Connect_DB();
            DataSet ds = new DataSet();

            conn1.Open_Data_Set(Qry, "tbl_Memberinfo", ds);
            if (conn1.DataSet_ReCount > 0)
            {
                string Msg = string.Format("{0}측에 기존회원 {1}_{2} 회원이 존재합니다." + Environment.NewLine
                    + "변경하시겠습니까?"
                    , (CheckLineCnt == 1 ? "좌" : "우")
                    , ds.Tables["tbl_Memberinfo"].Rows[0]["mbid2"].ToString()
                    , ds.Tables["tbl_Memberinfo"].Rows[0]["m_name"].ToString()
                    );

                DialogResult ret = MessageBox.Show(Msg, "확인", MessageBoxButtons.YesNo);
                if (ret == DialogResult.Yes)
                {
                    int NEW_idx2 = 0;
                    if (!int.TryParse(mtxtMbid_s.Text.Replace("_", "").Trim(), out NEW_idx2))
                    {
                        MessageBox.Show("후원인 번호를 확인하지못했습니다. 다시검색해주십시오.");
                        mtxtMbid_s.Focus();
                        mtxtMbid_s.Select();
                        return;
                    }
                    string QrySave = string.Empty;
                    if (rdoLineLeft.Checked)
                    {
                        QrySave = "select top 1 mbid2 from tbl_memberinfo where Saveid2 = '" + NEW_idx2 + "' AND LineCnt = 1 ";

                        ds = new DataSet();
                        conn1.Open_Data_Set(QrySave, base_db_name, ds);
                        if (conn1.DataSet_ReCount > 0)
                        {
                            cls_Search_DB csd2 = new cls_Search_DB();
                            string TempMbid2 = ds.Tables[base_db_name].Rows[0][0].ToString();
                            csd2.Member_Mod_BackUp(TempMbid2, "tbl_Memberinfo");

                            QrySave = "Update tbl_Memberinfo Set ";
                            QrySave = QrySave + " LineCnt = 2 ";
                            QrySave = QrySave + " Where saveid2 = '" + NEW_idx2 + "'";
                            if (conn1.Update_Data(QrySave, this.Name.ToString(), this.Text) == false) return;

                            csd2.tbl_Memberinfo_Mod(TempMbid2, "회원가입에서 변경됨 ");
                        }

                        CheckLineCnt = 1;
                    }
                    //이미 우가있으면 좌로 빼줍니다
                    else if (rdoLineRight.Checked)
                    {
                        QrySave = "select top 1 mbid2 from tbl_memberinfo where Saveid2 = '" + NEW_idx2 + "' AND LineCnt = 2 ";

                        ds = new DataSet();
                        conn1.Open_Data_Set(QrySave, base_db_name, ds);
                        if (conn1.DataSet_ReCount > 0)
                        {
                            cls_Search_DB csd2 = new cls_Search_DB();
                            string TempMbid2 = ds.Tables[base_db_name].Rows[0][0].ToString();
                            csd2.Member_Mod_BackUp(TempMbid2, "tbl_Memberinfo");

                            QrySave = "Update tbl_Memberinfo Set ";
                            QrySave = QrySave + " LineCnt = 1 ";
                            QrySave = QrySave + " Where saveid2 = '" + NEW_idx2 + "'";
                            if (conn1.Update_Data(QrySave, this.Name.ToString(), this.Text) == false) return;

                            csd2.tbl_Memberinfo_Mod(TempMbid2, "회원가입에서 변경됨");
                        }

                        CheckLineCnt = 2;
                    }
                }
                else
                    return;

            }
            #endregion

            if (Check_TextBox_Error(true) == false) return;  //추천인과 후원인 회원번호에 대한 오류를 체크한다   

            if (check_CC.Checked == true)
            {
                if (Check_TextBox_CC_Error() == false) return;  //부부사업자 등록 관련 오류를 체크한다.
            }

            if (Check_Duplication_Error() == false) return;
            if (Check_Duplication_Error1() == false) return;
            if (Check_Duplication_Error2() == false) return;

            if (Check_Duplication_Error_OutMember() == false) return;
            if (Check_Duplication_Error1_OutMember() == false) return;
            if (Check_Duplication_Error2_OutMember() == false) return;

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            




            cls_Search_DB csd = new cls_Search_DB();
            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

            if (Mbid2 == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Not")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                return;
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();



            try
            {
                string hometel = ""; string hptel = ""; string t_Sn = "";
                int LineCnt = 0; int N_LineCnt = 0;
                string Nominid = ""; int Nominid2 = 0;
                string Saveid = ""; int Saveid2 = 0;
                string BirthDay = ""; string BirthDay_M = ""; string BirthDay_D = ""; int BirthDayTF = 0;
                int For_Kind_TF = 0; int Sell_Mem_TF = 0, G8_TF = 0;
                int BankDocument = 0, CpnoDocument = 0, RBO_Mem_TF = 0;



                if (mtxtTel1.Text.Replace("-", "").Trim() != "") hometel = mtxtTel1.Text.Replace("_", "").Trim();
                if (mtxtTel2.Text.Replace("-", "").Trim() != "") hptel = mtxtTel2.Text.Replace("_", "").Trim();

                t_Sn = mtxtSn.Text.Replace("-", "").Replace("_", "").Trim();

                if (check_BankDocument.Checked == true) BankDocument = 1;
                if (check_CpnoDocument.Checked == true) CpnoDocument = 1;

                if (chk_Top_n.Checked == true)
                {
                    Nominid = "**"; Nominid2 = 0; N_LineCnt = 1;
                }
                else
                {
                    T_Mbid = mtxtMbid_n.Text;
                    csd.Member_Nmumber_Split(T_Mbid, ref Nominid, ref Nominid2);
                    N_LineCnt = csd.N_LineCnt_Search_Nom(Nominid, Nominid2);
                }

                if (chk_Top_s.Checked == true)
                {
                    Saveid = "**"; Saveid2 = 0; LineCnt = 1;
                }
                else
                {
                    T_Mbid = mtxtMbid_s.Text;
                    csd.Member_Nmumber_Split(T_Mbid, ref Saveid, ref Saveid2);
                    int LineCnt_Tmp = csd.LineCnt_Search_Save(Saveid, Saveid2);

                    LineCnt = LineCnt_Tmp;//rdoLineLeft.Checked ? 1 : (LineCnt_Tmp == 1 ? 2 : LineCnt_Tmp);//csd.LineCnt_Search_Save(Saveid, Saveid2);
                }


                if (N_LineCnt <= 0 || LineCnt <= 0) //주문번호 미발급시 오류로 해서 되돌린다.  
                {

                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));

                    tran.Dispose();
                    Temp_Connect.Close_DB();

                    return;
                }


                if (opt_sell_3.Checked == true) Sell_Mem_TF = 1; //소비자는 1 판매원은 기본 0

                if (raButt_IN_2.Checked == true) For_Kind_TF = 1;// 내국인은 0 외국인은 1  사업자는 2
                                                                 //if (raButt_IN_3.Checked == true) For_Kind_TF = 2;

                //if (opt_Bir_TF_1.Checked == true)
                BirthDayTF = 1; //양력은 1  음력은 2
                //if (opt_Bir_TF_2.Checked == true) BirthDayTF = 2;

                if (radioB_RBO.Checked == true) RBO_Mem_TF = 0;// RBO 0 비긴즈 1
                if (radioB_Begin.Checked == true) RBO_Mem_TF = 1;

                if (radioB_G8.Checked == true) G8_TF = 8;// RBO 0 비긴즈 1
                if (radioB_G4.Checked == true) G8_TF = 4;


                if (mtxtBrithDay.Text.Replace("-", "").Trim() != "")
                {
                    string[] Sn_t = mtxtBrithDay.Text.Split('-');

                    BirthDay = Sn_t[0];  //생년월일을 년월일로 해서 쪼갠다
                    BirthDay_M = Sn_t[1]; //웹쪽 관련해서 이렇게 받아들이는데가 많아서
                    BirthDay_D = Sn_t[2]; //웹쪽 기준에 맞춘거임.
                }

                string Na_Code = combo_Se_Code.Text.Trim();

                if (Na_Code == "" || tab_Nation.Visible == false) Na_Code = "KR";

                if (txtPassword.Text.Trim() == string.Empty)
                {
                    txtPassword.Text = "anew" + mtxtBrithDay.Text.Replace("-", "").Trim().Substring(2, 6);
                }
                //20200609구현호 제3자동의 업데이트
                int Third_Person_Agree = 0; 
                if (checkB_Third_Person_Agree.Checked == true) Third_Person_Agree  = 1;
              

                string Sex_FLAG = "";
                if (radioB_Sex_Y.Checked == true) Sex_FLAG = "Y";
                if (radioB_Sex_X.Checked == true) Sex_FLAG = "X";


                string AgreeSMS = "N";
                string AgreeEmail = "N";

                if (checkB_SMS_FLAG.Checked == true) AgreeSMS = "Y";
                if (checkB_EMail_FLAG.Checked == true) AgreeEmail = "Y";

                StringBuilder sb = new StringBuilder();
                string StrSql = "";
                sb.Append("Insert into tbl_Memberinfo ");
                sb.Append(" (");
                sb.Append(" Mbid  ");
                sb.Append(" , Mbid2 ");
                sb.Append(" , m_name ");
                sb.Append(" , e_name ");
                sb.Append(" , E_name_Last ");
                sb.Append(" , email ");
                sb.Append(" , Cpno ");
                sb.AppendLine(" , addcode1 ");
                sb.Append(" , address1 ");
                sb.Append(" , address2 ");
                sb.Append(" , hometel");
                sb.Append(" , hptel");
                sb.Append(" , LineCnt ");
                sb.Append(" , N_LineCnt ");
                sb.Append(" , recordid ");
                sb.Append(" , recordtime ");

                //코드값도 조회되도록...이름도 조회되도록...
                //이름에 나옴에 따라 코드값도 조회되어 저장되도록 해야한다. 
                //그래야 회원현황에서 검색시 tbl_memberinfor의businesscode (센터코드)가 tbl_business ncode와 조인해서 이름이 조회된다.

                sb.Append(" , businesscode ");
                sb.AppendLine(" , bankcode ");

                sb.Append(" , bankaccnt ");
                sb.Append(" , bankowner ");
                sb.Append(" , regtime ");
                sb.Append(" , saveid ");
                sb.Append(" , saveid2 ");
                sb.Append(" , nominid ");
                sb.Append(" , nominid2 ");
                sb.Append(" , regdocument,bankdocument , cpnodocument ");
                sb.Append(" , Remarks ");
                sb.AppendLine(" , LeaveCheck,LineUserCheck ");
                sb.Append(" , LeaveDate,LineUserDate ");
                sb.Append(" , LeaveReason,LineDelReason");
                sb.Append(" , WebID ");
                sb.Append(" , WebPassWord ");
                sb.Append(" , BirthDay ");
                sb.Append(" , BirthDay_M ");
                sb.Append(" , BirthDay_D ");
                sb.Append(" , BirthDayTF ");
                sb.Append(" , Ed_Date  ");
                sb.AppendLine(" , For_Kind_TF ");
                sb.Append(" , Sell_Mem_TF ");
                sb.Append(" , GiBu_ ");
                sb.Append(" , Na_Code ");
                sb.Append(" , Reg_bankaccnt ");
                sb.Append(" , VisaDate ");
                sb.Append(" , RBO_Mem_TF ");
                sb.Append(" , RBO_S_Date ");
                sb.AppendLine(" , G8_TF ");
                sb.Append(" , Sex_FLAG");
                sb.Append(" , AgreeSMS");
                sb.Append(" , AgreeEmail");
                sb.Append(" , ipin_ci");
                sb.Append(" , ipin_di");
                sb.Append(" , Reg_Name_Birth_Sender");


                if (check_CC.Checked == true)
                {
                    sb.Append(" , C_M_Name ");
                    sb.Append(" , C_For_Kind_TF ");
                    //sb.Append(" , C_cpno ");
                    //sb.Append(" , C_E_name ");
                    //sb.Append(" , C_E_name_Last ");
                    //sb.Append(" , C_Cop ");                    
                    sb.Append(" , C_BirthDay ");
                    sb.Append(" , C_BirthDay_M ");
                    sb.Append(" , C_BirthDay_D ");
                    sb.Append(" , C_hptel");
                    sb.AppendLine(", C_Email");
                }
                sb.Append(",Third_Person_Agree");
                sb.AppendLine(") Values ( ");
                sb.AppendLine("'" + Mbid + "'");
                sb.AppendLine("," + Mbid2);
                sb.AppendLine(",'" + txtName.Text.Trim() + "'");
                sb.AppendLine(",'" + txtName_E_1.Text.Trim() + "'");
                sb.AppendLine(",'" + txtName_E_2.Text.Trim() + "'");
                sb.AppendLine(",'" + txtEmail.Text.Trim() + "'");
                sb.AppendLine(", dbo.ENCRYPT_AES256('" + t_Sn.Trim() + "') ");
                sb.AppendLine(",'" + mtxtZip1.Text.Replace("-", "") + "'");
                sb.AppendLine(",'" + txtAddress1.Text.Replace("'","''").Trim() + "'");
                sb.AppendLine(",'" + txtAddress2.Text.Replace("'","''").Trim() + "'");
                sb.AppendLine(",'" + hometel + "'");
                sb.AppendLine(",'" + hptel + "'");
                sb.AppendLine("," + LineCnt);
                sb.AppendLine("," + N_LineCnt);
                sb.AppendLine(",'" + cls_User.gid + "'");
                sb.AppendLine(", Convert(Varchar(25),GetDate(),21) ");
                //코드값도 조회되도록...이름도 조회되도록...
                sb.AppendLine(",left( '" + txtCenter.Text.Trim() + "', 3 )");
                sb.AppendLine(",left( '" + txtBank.Text.Trim() + "', 3 )");

                sb.AppendLine(", dbo.ENCRYPT_AES256('" + txtAccount.Text.Trim() + "') ");
                sb.AppendLine(",'" + txtName_Accnt.Text.Trim() + "'");

                sb.AppendLine(",'" + mtxtRegDate.Text.Replace("-", "").Trim() + "'");

                sb.AppendLine(",'" + Saveid + "'");
                sb.AppendLine("," + Saveid2);
                sb.AppendLine(",'" + Nominid + "'");
                sb.AppendLine("," + Nominid2);

                sb.AppendLine(", 0   ");
                sb.AppendLine("," + BankDocument);
                sb.AppendLine("," + CpnoDocument);

                sb.AppendLine(",'" + txtRemark.Text.Trim() + "'");
                sb.AppendLine(", 1 ,  1 ");
                sb.AppendLine(", '' ,  '' ");
                sb.AppendLine(", '' ,  '' ");

                sb.AppendLine(",'" + txtWebID.Text.Trim() + "'");
                sb.AppendLine(",'" + EncryptSHA256_EUCKR(txtPassword.Text.Trim()) + "'");

                sb.AppendLine(",'" + BirthDay.Trim() + "'");
                sb.AppendLine(",'" + BirthDay_M.Trim() + "'");
                sb.AppendLine(",'" + BirthDay_D.Trim() + "'");
                sb.AppendLine("," + BirthDayTF);

                sb.AppendLine(",'" + mtxtEdDate.Text.Replace("-", "").Trim() + "'");
                sb.AppendLine("," + For_Kind_TF);
                sb.AppendLine("," + Sell_Mem_TF);


                sb.AppendLine("," + double.Parse(txtB1.Text.Trim().ToString()));


                sb.AppendLine(",'" + Na_Code + "'");
                sb.AppendLine(", dbo.ENCRYPT_AES256('" + txtAccount_Reg.Text.Trim() + "') ");

                sb.AppendLine(",'" + mtxtVisaDay.Text.Replace("-", "").Trim() + "'");

                sb.AppendLine("," + RBO_Mem_TF);

                //RBO로 ㅈ입력시에서 전환일자에 등록일자를 넣고.. 비긴즈로 넣으면 전환일자에 빈칸을 넣는다.
                if (RBO_Mem_TF == 1)
                    sb.AppendLine(",''");
                else
                    sb.AppendLine(",'" + mtxtRegDate.Text.Replace("-", "").Trim() + "'");

                sb.AppendLine("," + G8_TF);
                sb.AppendLine(", '" + Sex_FLAG + "'");
                sb.Append(" , '" + AgreeSMS + "'");
                sb.Append(" , '" + AgreeEmail + "'");
                sb.AppendLine(", '" + txt_IpinCI.Text + "'");
                sb.AppendLine(", '" + txt_IpinDI.Text + "'");
                sb.AppendLine(", '" + txt_Name_Check.Text.Trim() + "'");


                if (check_CC.Checked == true)
                {
                    #region * 동반자 ( 부부 사업자 추가 ) 
                    BirthDay = ""; BirthDay_M = ""; BirthDay_D = "";

                    if (mtxtBrithDayC.Text.Replace("-", "").Trim() != "")
                    {
                        string[] Sn_t = mtxtBrithDayC.Text.Split('-');

                        BirthDay = Sn_t[0];  //생년월일을 년월일로 해서 쪼갠다
                        BirthDay_M = Sn_t[1]; //웹쪽 관련해서 이렇게 받아들이는데가 많아서
                        BirthDay_D = Sn_t[2]; //웹쪽 기준에 맞춘거임.
                    }

                    For_Kind_TF = 0;
                    if (raButt_IN_2_C.Checked == true) For_Kind_TF = 1;// 내국인은 0 외국인은 1  사업자는 2


                    //t_Sn = mtxtSn_C.Text.Replace("-", "").Replace("_", "").Trim();

                    sb.AppendLine(",'" + txtName_C.Text.Trim() + "'");
                    sb.AppendLine("," + For_Kind_TF);
                    //sb.AppendLine( ",'" + encrypter.Encrypt(t_Sn.Trim()) + "'");
                    //sb.AppendLine( ",'" + txtName_E_1_C.Text.Trim() + "'");
                    //sb.AppendLine( ",'" + txtName_E_2_C.Text.Trim() + "'");
                    sb.AppendLine(",'" + BirthDay.Trim() + "'");
                    sb.AppendLine(",'" + BirthDay_M.Trim() + "'");
                    sb.AppendLine(",'" + BirthDay_D.Trim() + "'");
                    sb.AppendLine(" ,'" + mtxtTel2_C.Text + "'");
                    sb.AppendLine(" ,'" + txtEmail_C.Text + "'");
                    #endregion
                }
                sb.AppendLine(", " + Third_Person_Agree );
                sb.AppendLine(")");

                StrSql = sb.ToString();
                Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo", Conn, tran, this.Name, this.Text);


                tran.Commit();
                Save_Error_Check = 1;


                cls_form_Meth cm = new cls_form_Meth();
                MessageBox.Show(cm._chang_base_caption_search("회원_번호") + ":" + mtxtMbid.Text.Trim()
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
            }
            catch (Exception)
            {
                if (Save_Error_Check == 0)
                    tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }

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

            if (mtxtBrithDayC.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtBrithDayC.Text, mtxtBrithDayC, "Date") == false)
                {
                    mtxtBrithDayC.Focus();
                    return false;
                }
            }
            if (txtEmail_C.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(txtEmail_C.Text, txtEmail_C, "Email") == false)
                {
                    txtEmail_C.Focus();
                    return false;
                }
            }
            if (mtxtTel2_C.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtTel2_C.Text, mtxtTel2_C, "HpTel") == false)
                {
                    mtxtTel2_C.Focus();
                    return false;
                }
            }

            return true;
        }

        private string EncryptSHA256_EUCKR(string phrase)
        {

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

        private void mtxtMbid_n_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        void e_f_Send_MemNumber_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = "";
            cls_Search_DB csb = new cls_Search_DB();
            csb.Member_Nmumber_Split(mtxtMbid_s.Text.Trim(), ref searchMbid, ref searchMbid2);
        }

        void e_f_Send_MemNumber_Info_3(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = "";
            cls_Search_DB csb = new cls_Search_DB();
            csb.Member_Nmumber_Split(mtxtMbid_n.Text.Trim(), ref searchMbid, ref searchMbid2);
        }

        //변경할려는 대상자에 대한 회원번호에서 회원 검색창을 뛰엇을 경우에
        void e_f_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            mtxtMbid_s.Text = Send_Number; txtName_s.Text = Send_Name;
            if (Input_Error_Check(mtxtMbid_s, "s") == true)
                Set_Form_Date(mtxtMbid_s.Text, "s");
        }


        //새로운 후원인 관련 회원 검색창을 뛰엇을 경우에 검색창에서 이벤트 실행시..
        void e_f_Send_Mem_Number_3(string Send_Number, string Send_Name)
        {
            mtxtMbid_n.Text = Send_Number; txtName_n.Text = Send_Name;
            if (Input_Error_Check(mtxtMbid_n, "n") == true)
                Set_Form_Date(mtxtMbid_n.Text, "n");
        }


        void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtName_s.Text.Trim();
        }


        void e_f_Send_MemName_Info_3(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtName_n.Text.Trim(); ;
        }
        void e_f_Send_MemName_txtname(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = txtName.Text.Trim(); ;
        }
        private void Set_Form_Date(string T_Mbid, string T_sort)
        {
            string Mbid = ""; int Mbid2 = 0;
            Data_Set_Form_TF = 1;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) != 1)
            {
                Data_Set_Form_TF = 0;
                return;
            }

            string Tsql = "";
            Tsql = "Select  ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) AS M_Mbid ";
            else
                Tsql = Tsql + " tbl_Memberinfo.mbid2 AS M_Mbid ";

            Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

            Tsql = Tsql + ",  tbl_Memberinfo.Cpno ";

            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            }
            //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";

            if (T_sort != "s" && T_sort != "n")  //후원인하고 추천인 검색시에는 필요가 없다.
            {
                Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
                Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";
            }

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            if (T_sort == "s")
            {
                mtxtMbid_s.Text = ""; txtName_s.Text = ""; txtSN_s.Text = "";
                mtxtMbid_s.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();
                txtName_s.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();
                txtSN_s.Text = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Cpno"].ToString(), "Cpno");

                ////원래는 추천인 번호 앞자리가 기준인데 추천인 기능이 사용을 안한다고 체크를 하게 되면 후원인 앞자리로 해서 번호를 따옴. 자동 부여일 경우
                //if (cls_app_static_var.nom_uging_Pr_Flag == 0 && cls_app_static_var.Mem_Number_Auto_Flag == "A")  //후원인만 사용을 하고 번호 자동 부여이다.                
                //    mtxtMbid.Text = csb.Auto_Member_Number_Search(mtxtMbid_s.Text.Trim()); //후원인 앞자리 번호에 맞게 해서 번호를 자동으로 받아온다.

                select_Save_Dir_Down(Mbid, Mbid2);

                txtLineCnt.Focus();
            }

            if (T_sort == "n")
            {
                mtxtMbid_n.Text = ""; txtName_n.Text = ""; txtSN_n.Text = "";
                mtxtMbid_n.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();
                txtName_n.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();
                txtSN_n.Text = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Cpno"].ToString(), "Cpno");

                //if (cls_app_static_var.Mem_Number_Auto_Flag == "A")  //회원번호 자동 생성으로 체크되어 잇는 경우
                //    mtxtMbid.Text = csb.Auto_Member_Number_Search(mtxtMbid_n.Text.Trim()); //추천인 앞자리 번호에 맞게 해서 번호를 자동으로 받아온다.
                ////원래는 추천인 번호 앞자리가 기준인데 추천인 기능이 사용을 안한다고 체크를 하게 되면 후원인 앞자리로 해서 번호를 따옴. 자동 부여일 경우
                //if (cls_app_static_var.nom_uging_Pr_Flag == 0 && cls_app_static_var.Mem_Number_Auto_Flag == "A")  //후원인만 사용을 하고 번호 자동 부여이다.                
                //    mtxtMbid.Text = csb.Auto_Member_Number_Search(mtxtMbid_s.Text.Trim()); //후원인 앞자리 번호에 맞게 해서 번호를 자동으로 받아온다.

                mtxtMbid_s.Focus();


                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Nom_Same_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    mtxtMbid_s.Text = ""; txtName_s.Text = ""; txtSN_s.Text = "";
                    mtxtMbid_s.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();

                    if (Input_Error_Check(mtxtMbid_s, "s") == true)
                    {
                        txtName_s.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();
                        txtSN_s.Text = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Cpno"].ToString(), "Cpno");

                        if(Check_SaveID_Down())
                        {
                            select_Save_Dir_Down(Mbid, Mbid2);
                            txtLineCnt.Focus();
                        }
                        else
                        {
                            txtName_s.Text = string.Empty;
                            mtxtMbid_s.Text = string.Empty;
                        }

                    }
                }
            }

            Data_Set_Form_TF = 0;
        }
        private void select_Save_Dir_Down(string Mbid, int Mbid2)
        {
            dGridView_Line_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
        }

        private bool Check_SaveID_Down()
        {
            int LineCnt = rdoLineLeft.Checked ? 1 : 2;
            string Qry = "SELECT mbid2, m_name FROM tbl_Memberinfo (nolock) WHERE SaveId2 = " + mtxtMbid_s.Text.Replace("_", "").Trim();
            Qry += " AND LineCnt = " + LineCnt;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();

            Temp_Connect.Open_Data_Set(Qry, "tbl_Memberinfo", ds);
            if (Temp_Connect.DataSet_ReCount > 0)
            {
                string Msg = string.Format("{0}측에 기존회원 {1}_{2} 회원이 존재합니다." + Environment.NewLine
                    + "변경하시겠습니까?"
                    , (LineCnt == 1 ? "좌" : "우")
                    , ds.Tables["tbl_Memberinfo"].Rows[0]["mbid2"].ToString()
                    , ds.Tables["tbl_Memberinfo"].Rows[0]["m_name"].ToString()
                    );

                DialogResult ret = MessageBox.Show(Msg, "확인", MessageBoxButtons.YesNo);
                return ret == DialogResult.Yes;
            }
            return true;
        }

        private void dGridView_Line_Header_Reset()
        {


            cg_Li.grid_col_Count = 5;
            // cg_Li.basegrid = dGridView_Li;
            cg_Li.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            // cg_Li.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"위치"  , "회원_번호"   , "성명"  , ""   , ""
                                    };
            cg_Li.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 30, 60, 70, 0, 0
                            };
            cg_Li.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                   };
            cg_Li.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5      
                              };
            cg_Li.grid_col_alignment = g_Alignment;
            // cg_Li.basegrid.RowHeadersVisible = false;
            //cg_Li.basegrid.Font.Size = 7.5;

            //cg_Li.basegrid.ColumnHeadersDefaultCellStyle.Font =
            //new Font(cg_Li.basegrid.Font.FontFamily ,8);


        }

        private void Db_Grid_Popup(TextBox tb, TextBox tb1_Code, string strSql)
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


            if (strSql != "")
            {
                if (tb.Name == "txtCenter")
                {
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);
                    cgb_Pop.Next_Focus_Control = butt_AddCode;
                }

                if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtBank")
                {
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);
                    cgb_Pop.Next_Focus_Control = txtName_Accnt;

                }
            }
            else
            {
                if (tb.Name == "txtCenter")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + combo_Se_Code.Text.Trim() + "') )";
                    if (combo_Se_Code.Text.Trim() != "") Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " And ncode <> '002'"; // 2018-11-23 지성경 에스제이로직스는 선택불가능하게끔한다.
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                    cgb_Pop.Next_Focus_Control = butt_AddCode;
                }

                if (tb.Name == "txtR_Id")
                {
                    string Tsql;
                    Tsql = "Select user_id ,U_Name   ";
                    Tsql = Tsql + " From tbl_User (nolock) ";
                    Tsql = Tsql + " Order by user_id ";

                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", Tsql);
                }

                if (tb.Name == "txtBank")
                {
                    string Tsql;
                    Tsql = "Select Ncode ,BankName    ";
                    Tsql = Tsql + " From tbl_Bank (nolock) ";
                    if (combo_Se_Code.Text.Trim() != "") Tsql = Tsql + " Where  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);
                    cgb_Pop.Next_Focus_Control = txtName_Accnt;
                }

            }
        }


        private void dGridView_Base_Header_Reset()
        {
            cgb.basegrid = dGridCtrl_Base;
            cgb.baseview = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;

            //cgb.grid_col_Count = 38;
            //cgb.basegrid = dGridCtrl_Base;
            //cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            //cgb.grid_Frozen_End_Count = 2;
            //cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            string[] g_HeaderText = {"회원_번호"  , "성명"   , "주민번호"  , "위치"   , "센타명"
                                , "가입일"   , "집전화"    , "핸드폰"   , "후원인"    , "후원인명"
                                , "추천인"   , "추천인명"  , "우편_번호"   , "주소"   , "주소"
                                ,"구분" , "기록자" , "기록일"    , "" , ""
                                    };

            //string[] g_Col_name = {"mbid2"  , "M_Name"   , "Cpno"  , "LineCnt"   , "B_Name"
            //                    , "regtime"    , "homeTel"   , "HpTel"    , "saveid2" , "savename"
            //                    , "nominid2e", "nominName"  , "zipcode" ,"address","address2","sellmem"
            //                     , "recordid"   ,"recordtime" ,"" ,"" ,""

            //                        };

            cgb.grid_col_header_text = g_HeaderText;
            //cgb.grid_col_name = g_Col_name;
            cgb.grid_col_Count = g_HeaderText.Length;


            int[] g_Width = {  85, 90, 130, 60, 100
                             ,80, 130, 130, 90 , 90
                             ,90  , 90, 80,  200, 80
                             ,90 , 120, 0 , 0 ,  0
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = {  true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                          {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //15   
                          
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //20
                              

                              };
            cgb.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            //gr_dic_cell_format[14] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[15] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[16] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[17] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[18] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[19] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[20] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[23] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[24] = cls_app_static_var.str_Grid_Currency_Type;

            cgb.grid_cell_format = gr_dic_cell_format;
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
                                ,ds.Tables[base_db_name].Rows[fi_cnt][12]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][13]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][14]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][15]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][16]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][17]

                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }

        private void Base_Grid_Set()
        {
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            string Tsql = "";
            Tsql = "Select  ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) ";
            else
                Tsql = Tsql + " tbl_Memberinfo.mbid2 ";

            Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

            Tsql = Tsql + ", tbl_Memberinfo.Cpno ";

            Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";

            Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";
            Tsql = Tsql + " , LEFT(tbl_Memberinfo.RegTime,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.RegTime,4),2) + '-' + RIGHT(tbl_Memberinfo.RegTime,2)   ";
            Tsql = Tsql + " , tbl_Memberinfo.hometel ";
            Tsql = Tsql + " , tbl_Memberinfo.hptel ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,tbl_Memberinfo.Saveid + '-' + Convert(Varchar,tbl_Memberinfo.Saveid2) ";
            else
                Tsql = Tsql + " ,tbl_Memberinfo.Saveid2 ";

            Tsql = Tsql + " , Isnull(Sav.M_Name,'') ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,tbl_Memberinfo.Nominid + '-' + Convert(Varchar,tbl_Memberinfo.Nominid2) ";
            else
                Tsql = Tsql + " ,tbl_Memberinfo.Nominid2 ";

            Tsql = Tsql + " , Isnull(Nom.M_Name,'') ";
            Tsql = Tsql + " , Case When tbl_Memberinfo.addcode1 <> '' Then  LEFT(tbl_Memberinfo.addcode1,3) +'-' + RIGHT(tbl_Memberinfo.addcode1,3) ELSE '' End ";

            Tsql = Tsql + " , tbl_Memberinfo.address1 ";
            Tsql = Tsql + " , tbl_Memberinfo.address2 ";
            Tsql = Tsql + " , Case  When tbl_Memberinfo.Sell_Mem_TF = 0 then '판매원' ELSE  '소비자' End";
            Tsql = Tsql + " , tbl_Memberinfo.recordid ";
            Tsql = Tsql + " , tbl_Memberinfo.recordtime ";
            Tsql = Tsql + " , tbl_Memberinfo.recordtime ";
            Tsql = Tsql + " , tbl_Memberinfo.recordtime ";
            Tsql = Tsql + " , tbl_Memberinfo.recordtime ";

            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Sav (nolock) ON tbl_Memberinfo.Saveid = Sav.Mbid And tbl_Memberinfo.Saveid2 = Sav.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Nom (nolock) ON tbl_Memberinfo.Nominid = Nom.Mbid And tbl_Memberinfo.Nominid2 = Nom.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";
            Tsql = Tsql + " Where Replace(left(tbl_Memberinfo.Recordtime,10),'-','') = Replace (LEFT( Convert(Varchar(25),GetDate(),21) ,10 ) ,'-' , '') ";
            Tsql = Tsql + " And  tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
            Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

            //++++++++++++++++++++++++++++++++
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

            foreach (DevExpress.XtraGrid.Columns.GridColumn col in dGridView_Base.Columns)
            {
                if (new List<string>()
                {
                    "OrderNumber"  ,"InsuranceNumber", "mbid2"   , "mname",  "cpno","nmbid2", "nmname"
                }.Contains(col.Name))
                {
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                }
                else
                    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;

            }
        }

        //private void Base_Grid_Set()
        //{
        //    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        //    dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
        //    cgb.d_Grid_view_Header_Reset();
        //    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 
        //    string Tsql = "";

        //    Tsql = "Select  ";
        //    if (cls_app_static_var.Member_Number_1 > 0)
        //        Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) ";
        //    else
        //        Tsql = Tsql + " tbl_Memberinfo.mbid2 ";

        //    Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

        //    Tsql = Tsql + ", tbl_Memberinfo.Cpno ";

        //    Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";

        //    Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";
        //    Tsql = Tsql + " , LEFT(tbl_Memberinfo.RegTime,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.RegTime,4),2) + '-' + RIGHT(tbl_Memberinfo.RegTime,2)   ";
        //    Tsql = Tsql + " , tbl_Memberinfo.hometel ";
        //    Tsql = Tsql + " , tbl_Memberinfo.hptel ";

        //    if (cls_app_static_var.Member_Number_1 > 0)
        //        Tsql = Tsql + " ,tbl_Memberinfo.Saveid + '-' + Convert(Varchar,tbl_Memberinfo.Saveid2) ";
        //    else
        //        Tsql = Tsql + " ,tbl_Memberinfo.Saveid2 ";

        //    Tsql = Tsql + " , Isnull(Sav.M_Name,'') ";

        //    if (cls_app_static_var.Member_Number_1 > 0)
        //        Tsql = Tsql + " ,tbl_Memberinfo.Nominid + '-' + Convert(Varchar,tbl_Memberinfo.Nominid2) ";
        //    else
        //        Tsql = Tsql + " ,tbl_Memberinfo.Nominid2 ";

        //    Tsql = Tsql + " , Isnull(Nom.M_Name,'') ";
        //    Tsql = Tsql + " , Case When tbl_Memberinfo.addcode1 <> '' Then  LEFT(tbl_Memberinfo.addcode1,3) +'-' + RIGHT(tbl_Memberinfo.addcode1,3) ELSE '' End ";

        //    Tsql = Tsql + " , tbl_Memberinfo.address1 ";
        //    Tsql = Tsql + " , tbl_Memberinfo.address2 ";
        //    Tsql = Tsql + " , Case  When tbl_Memberinfo.Sell_Mem_TF = 0 then '판매원' ELSE  '소비자' End";
        //    Tsql = Tsql + " , tbl_Memberinfo.recordid ";
        //    Tsql = Tsql + " , tbl_Memberinfo.recordtime ";

        //    Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
        //    Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Sav (nolock) ON tbl_Memberinfo.Saveid = Sav.Mbid And tbl_Memberinfo.Saveid2 = Sav.Mbid2 ";
        //    Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Nom (nolock) ON tbl_Memberinfo.Nominid = Nom.Mbid And tbl_Memberinfo.Nominid2 = Nom.Mbid2 ";
        //    Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";
        //    Tsql = Tsql + " Where Replace(left(tbl_Memberinfo.Recordtime,10),'-','') = Replace (LEFT( Convert(Varchar(25),GetDate(),21) ,10 ) ,'-' , '') ";
        //    Tsql = Tsql + " And  tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
        //    Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";
        //    //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
        //    //당일 등록된 회원을 불러온다.

        //    //++++++++++++++++++++++++++++++++
        //    cls_Connect_DB Temp_Connect = new cls_Connect_DB();

        //    DataSet ds = new DataSet();
        //    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
        //    if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
        //    int ReCnt = Temp_Connect.DataSet_ReCount;

        //    if (ReCnt == 0) return;
        //    //++++++++++++++++++++++++++++++++


        //    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        //    Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

        //    for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
        //    {
        //        Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
        //    }
        //    cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
        //    //여기가 문제!!!!!

        //    cgb.db_grid_Obj_Data_Put();


        //    foreach (DevExpress.XtraGrid.Columns.GridColumn col in dGridView_Base.Columns)
        //    {
        //        if (new List<string>()
        //        {
        //            "OrderNumber"  ,"InsuranceNumber", "mbid2"   , "mname",  "cpno","nmbid2", "nmname"
        //        }.Contains(col.Name))
        //        {
        //            col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
        //        }
        //        else
        //            col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;

        //    }

        //}


        ////구현호20190221데브텝컨트롤
        //private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        //{
        //    // string index = xtraTabControl1.SelectedTabPageIndex;
        //    for (int i = 0; i < xtraTabControl1.TabPages.Count; i++)
        //    {
        //        if (xtraTabControl1.TabPages[i].Name == "xtraTabPage2")
        //        {
        //            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
        //            cgb.d_Grid_view_Header_Reset();
        //            xtraTabControl1.SelectedTabPage = xtraTabControl1.TabPages[i];

        //            break;
        //        }

        //    }
        //}
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            // string index = xtraTabControl1.SelectedTabPageIndex;
            for (int i = 0; i < xtraTabControl1.TabPages.Count; i++)
            {
                if (xtraTabControl1.TabPages[i].Name == "xtraTabPage2")
                {
                    dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                    cgb.d_Grid_view_Header_Reset();
                    //   xtraTabControl1.SelectedTabPage = xtraTabControl1.TabPages[i];
                    Base_Grid_Set(); //당일등록 회원을 불러온다.
                    break;
                }

            }



        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            if (sender is TextEdit)
            {
                TextEdit tb = (TextEdit)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = Color.White;

                if (tb.Name == "txtName" && tb.Text != "")
                    txtName_Accnt.Text = tb.Text;

            }

            if (sender is MaskedTextBox)
            {
                MaskedTextBox tb = (MaskedTextBox)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = Color.White;
            }
        }

        private void frmMember_Dev_Activated(object sender, EventArgs e)
        {
            Base_Grid_Set(); //당일등록 회원을 불러온다.
        }


        private void butt_Clear_Click_1(object sender, EventArgs e)
        {


            string text = txtAccount.Text;
            Data_Set_Form_TF = 1;
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>                
            Base_Grid_Set(); //당일등록 회원을 불러온다.
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this);

            //opt_sell_2.Checked = true; opt_Bir_TF_1.Checked = true;
            raButt_IN_1.Checked = true;

            // mtxtSn.Mask = "999999-9999999";
            mtxtRegDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            chk_Top_n.Checked = false; chk_Top_s.Checked = false;

            txtB1.Text = "0";
            check_BankDocument.Checked = true;
            check_CpnoDocument.Checked = true;

            //후원추천 기능 사용하지 말라고 하면 최상위로 체크를 해버린다.
            if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
            {
                chk_Top_s.Checked = true;
            }

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                chk_Top_n.Checked = true;
            }


            if (cls_app_static_var.Mem_Number_Auto_Flag == "H")
            {
                mtxtMbid.ReadOnly = false;
            }

            if (cls_app_static_var.Mem_Number_Auto_Flag == "A")
            {
                mtxtMbid.ReadOnly = true;
                mtxtMbid.BackColor = cls_app_static_var.txt_Enable_Color;

            }

            if (cls_app_static_var.Mem_Number_Auto_Flag == "R")
            {
                mtxtMbid.ReadOnly = true;
                mtxtMbid.BackColor = cls_app_static_var.txt_Enable_Color;
            }

            if (cls_app_static_var.Member_Cpno_Error_Check_TF == 1)
                check_Cpno_Err.Checked = true;

            if (cls_app_static_var.Member_Cpno_Put_TF == 1)
                check_Cpno.Checked = true;

            if (cls_app_static_var.Member_Reg_Multi_TF == 1)
                check_Cpno_Multi.Checked = true;
            //tab_Sub.SelectedIndex = 0;

            check_LR.Checked = true;
            radioB_RBO.Checked = true;
            radioB_G8.Checked = true;
            opt_sell_2.Checked = true;

            checkB_SMS_FLAG.Checked = true;
            checkB_EMail_FLAG.Checked = false;

            radioB_Sex_Y.Checked = false;
            radioB_Sex_X.Checked = false;
            Data_Set_Form_TF = 0;


            ClearPannel(this);

            txtCenter.Properties.ValueMember = "";
            //txtBank.Properties.ValueMember = "";
            txtBank.Text = "";

            txtB1.Text = "0";
            txtCenter.EditValue = null;
            txtBank.EditValue = null;
            lbl_ACC.Text = "미인증";



            rdoLineLeft.Checked = true;
            mtxtRegDate.DateTime = DateTime.Now;//.ToString("yyyy-MM-dd");
        }

        private void rdoLineLeft_CheckedChanged(object sender, EventArgs e)
        {
            txtLineCnt.Text = "1";
        }

        private void rdoLineRight_CheckedChanged(object sender, EventArgs e)
        {
            txtLineCnt.Text = "2";
        }
        private void radioButt_Sn_MouseUp(object sender, MouseEventArgs e) // 내국인 인증관련된 함수, 일단 비활성화
        {
            //RadioButton  trd = (RadioButton)sender ;

            //mtxtSn.Text = "";
            //if (trd.Name == "raButt_IN_1" || trd.Name == "raButt_IN_2")
            //    mtxtSn.Mask = "999999-9999999";
            //else            
            //    mtxtSn.Mask = "999-99-99999";

            //if (raButt_IN_1.Checked == true)    //내국인 선택시
            //{
            //    butt_Certify.Visible = true;
            //    Lbl_Certify.Visible = true;
            //}
            //else if (raButt_IN_2.Checked == true)   //외국인 선택시
            //{
            //    butt_Certify.Visible = false;
            //    Lbl_Certify.Visible = false;
            //}

            //mtxtSn.Focus();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (Keys.F1.Equals(keyData))
            {
                butt_Clear_Click_1(butt_Clear, null);
            }
            else if (Keys.F2.Equals(keyData))
            {
                butt_Save_Click(null, null);
            }
            else if (Keys.F12.Equals(keyData) || Keys.Escape.Equals(keyData))
            {
                this.Close();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void mtxtMbid_Click(object sender, EventArgs e)
        {
            /* 아직미개발
            var mtb = sender as TextEdit;
            if (mtb.Name == "mtxtMbid_s")
            {

                txtName_s.Text = ""; txtSN_s.Text = "";
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Line_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cg_Li.d_Grid_view_Header_Reset();
                txtLineCnt.Text = "";
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            }

            if (mtb.Name == "mtxtMbid_n")
            {
                txtName_n.Text = ""; txtSN_n.Text = "";
            }

            //마스크텍스트 박스에 입력한 내용이 있으면 그곳 다음으로 커서가 가게 한다.
            if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                mtb.SelectionStart = mtb.Text.Replace("-", "").Replace("_", "").Trim().Length + 1;
                */
        }

        void T_R_Key_Enter_13_Name(string txt_tag, TextEdit tb)
        {
            if (txt_tag != "")
            {
                int reCnt = 0;
                cls_Search_DB cds = new cls_Search_DB();
                string Search_Mbid = "";
                reCnt = cds.Member_Name_Search(ref Search_Mbid, txt_tag);

                if (reCnt == 1)
                {
                    if (tb.Name == "txtName_s")
                    {
                        mtxtMbid_s.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
                        if (Input_Error_Check(mtxtMbid_s, "s") == true)
                            Set_Form_Date(mtxtMbid_s.Text, "s");

                        //SendKeys.Send("{TAB}");
                    }

                    if (tb.Name == "txtName_n")
                    {
                        mtxtMbid_n.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
                        if (Input_Error_Check(mtxtMbid_n, "n") == true)
                            Set_Form_Date(mtxtMbid_n.Text, "n");
                        //SendKeys.Send("{TAB}");
                    }
                }
                else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
                {

                    //cls_app_static_var.Search_Member_Name = txt_tag;
                    frmBase_Member_Search e_f = new frmBase_Member_Search();
                    if (tb.Name == "txtName_s")
                    {
                        e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                        e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);
                    }

                    if (tb.Name == "txtName_n")
                    {
                        e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number_3);
                        e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info_3);
                    }

                    e_f.ShowDialog();

                    SendKeys.Send("{TAB}");
                }
            }
            else
                SendKeys.Send("{TAB}");

        }

        private void MtxtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            var mtb = sender as TextEdit;

            if ((mtb.Name == "mtxtMbid_s" || mtb.Name == "mtxtMbid_n" ) && e.KeyChar == Keys.Enter.GetHashCode())
            {
                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    int reCnt = 0;
                    cls_Search_DB cds = new cls_Search_DB();
                    string Search_Name = "";
                    reCnt = cds.Member_Name_Search_S_N(mtb.Text, ref Search_Name);

                    if (reCnt == 1)
                    {
                        if (mtb.Name == "mtxtMbid_s")
                        {
                            txtName_s.Text = Search_Name;
                            if (Input_Error_Check(mtb, "s") == true)
                                Set_Form_Date(mtb.Text, "s");
                        }

                        if (mtb.Name == "mtxtMbid_n")
                        {
                            txtName_n.Text = Search_Name;
                            if (Input_Error_Check(mtb, "n") == true)
                                Set_Form_Date(mtb.Text, "n");
                        }
                    }

                    else if (reCnt > 1)  //회원번호 비슷한 사람들이 많은 경우
                    {
                        string Mbid = "";
                        int Mbid2 = 0;
                        cds.Member_Nmumber_Split(mtb.Text, ref Mbid, ref Mbid2);

                        frmBase_Member_Search e_f = new frmBase_Member_Search();

                        if (mtb.Name == "mtxtMbid_s")
                        {
                            e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                            e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info);
                        }

                        if (mtb.Name == "mtxtMbid_n")
                        {
                            e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number_3);
                            e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info_3);
                        }

                        e_f.ShowDialog();

                        SendKeys.Send("{TAB}");
                    }
                }
                else
                    SendKeys.Send("{TAB}");
            }
        }

        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            var txtEdit = sender as TextEdit;
            if (txtEdit.Tag == "name" && e.KeyChar == Keys.Enter.GetHashCode())
            {
                T_R_Key_Enter_13_Name(txtEdit.Text, txtEdit);
            }

        }

        private void txtCenter_QueryPopUp(object sender, CancelEventArgs e)
        {
            LookUpEdit lookUpEdit = sender as LookUpEdit;
            lookUpEdit.Properties.PopulateColumns();
            lookUpEdit.Properties.Columns["code"].Visible = false;
        }

        private void txtBank_QueryPopUp(object sender, CancelEventArgs e)
        {
            LookUpEdit lookUpEdit = sender as LookUpEdit;
            lookUpEdit.Properties.PopulateColumns();
            lookUpEdit.Properties.Columns["code"].Visible = false;
        }

        private void Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar == 8) || (e.KeyChar == 45))
            {
            }
            else
                e.Handled = true;

        }

        private void txt_Enter(object sender, EventArgs e)
        {
            if(sender is BaseEdit)
            {
                (sender as BaseEdit).SelectAll();
            }
        }

        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if ((0xAC00 <= c && c <= 0xD7A3) || (0x3131 <= c && c <= 0x318E))
                e.Handled = true;
        }

        private void check_CC_CheckedChanged(object sender, EventArgs e)
        {
            lcCC.Enabled = check_CC.Checked;
        }

        private void txtBank_EditValueChanged(object sender, EventArgs e)
        {
            if(txtBank.Text == "")
                return;
            if (txtBank.EditValue == null)
                return;
            if (txtBank.EditValue == string.Empty)
                return;

            txtBank_Code.Text = txtBank.EditValue.ToString(); 
        }

        private void butt_Name_Check_Click(object sender, EventArgs e)
        {
            txt_Name_Check.Text = "";
            string Sn = "";
            string me = "";

            if (txtName.Text == "")
            {
                me = "회원명을 필히 입력해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                txtName.Focus();
                return;
            }

            if (mtxtBrithDay.Text.Replace("-", "").Replace("/", "") == "")
            {
                me = "생년월일을 필히 입력해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                mtxtBrithDay.Focus();
                return;
            }
            else
            {
                if (Sn_Number_(mtxtBrithDay.Text, mtxtBrithDay, "Date") == false)
                {
                    mtxtBrithDay.Focus();
                    return;
                }
            }

            //if (radioB_Sex_1.Checked == false && radioB_Sex_2.Checked == false)
            //{
            //    me = "성별을 필히 선택해 주십시요." + "\n" +
            //     cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

            //    MessageBox.Show(me);
            //    panel41.Focus();
            //    return;
            //}


            cls_Sn_Check csc = new cls_Sn_Check();

            string T_Line1 = "", T_Line2 = "", T_Line3 = "";

            int Send_FLAG = 0;
            //if (radioB_Sex_1.Checked == true) Send_FLAG = 1;
            //if (radioB_Sex_2.Checked == true) Send_FLAG = 2;


            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                csc.Nice_Name_Birth_Sender_Check(txtName.Text, mtxtBrithDay.Text.Replace("-", "").Replace("/", ""), Send_FLAG, ref T_Line1, ref T_Line2, ref T_Line3);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;

            if (T_Line1 == "Y")
            {


                txt_Name_Check.Text = txtName.Text + "/" + mtxtBrithDay.Text.Replace("-", "").Replace("/", "") + "/" + Send_FLAG.ToString();
                me = "올바른 성명 정보 입니다. 성명 간편인증 성공.";
                MessageBox.Show(me);
                txtRemark.Focus();
            }
            else
            {
                txt_Name_Check.Text = "";
                me = "올바른 성명 정보가 아닙니다. 확인후 다시 시도해 주십시요. 성명 간편인증 실패.";
                MessageBox.Show(me);
                mtxtBrithDay.Focus();
            }
        }
    }

}


