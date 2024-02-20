using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MLM_Program
{
    public partial class frmMember_Address_Change : Form
    {
        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);


        private const string base_db_name = "tbl_Memberinfo";
        private int Data_Set_Form_TF;

        public delegate void Take_NumberDele(ref string Send_Number, ref string Send_Name);
        public event Take_NumberDele Take_Mem_Number;

        public frmMember_Address_Change()
        {
            InitializeComponent();
        }
        

        
        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;
           
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
            

            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;            
        }



        private void frmBase_Resize(object sender, EventArgs e)
        {

            int base_w = this.Width / 3;
            butt_Clear.Width = base_w;
            butt_Save.Width = base_w;

            //butt_Delete.Width = base_w;
            butt_Exit.Width = base_w;

            butt_Clear.Left = 0;
            butt_Save.Left = butt_Clear.Left + butt_Clear.Width;

            //butt_Delete.Left = butt_Save.Left + butt_Save.Width;
            butt_Exit.Left = butt_Save.Left + butt_Save.Width;    
        }


        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            string Send_Number = ""; string Send_Name = "";
            Take_Mem_Number(ref Send_Number, ref Send_Name);

            if (Send_Number != "")
            {
                mtxtMbid.Text = Send_Number;
                Set_Form_Date(mtxtMbid.Text);
            }
        }

        private void frmBase_From_KeyDown(object sender, KeyEventArgs e)
        {
            //폼일 경우에는 ESC버튼에 폼이 종료 되도록 한다
            if (sender is Form)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (!this.Controls.ContainsKey("Popup_gr"))
                        this.Close();
                    else
                    {
                        DataGridView T_Gd = (DataGridView)this.Controls["Popup_gr"];

                        if (T_Gd.Name == "Popup_gr")
                        {
                            if (T_Gd.Tag != null)
                            {
                                if (!this.Controls.ContainsKey(T_Gd.Tag.ToString()))
                                {
                                    cls_form_Meth cfm = new cls_form_Meth();
                                    Control T_cl = cfm.from_Search_Control(this, T_Gd.Tag.ToString());
                                    if (T_cl != null)
                                        T_cl.Focus();

                                }
                            }

                            T_Gd.Visible = false;
                            T_Gd.Dispose();

                            //cls_form_Meth cfm = new cls_form_Meth();
                            //cfm.form_Group_Panel_Enable_True(this);
                        }
                    }
                }// end if
            }


            
            Button T_bt = butt_Exit;
            if (e.KeyValue == 123)
                T_bt = butt_Exit;    //닫기  F12
            if (e.KeyValue == 113)
                T_bt = butt_Save;     //저장  F1
            if (e.KeyValue == 115)
                T_bt = butt_Delete;   // 삭제  F4
            if (e.KeyValue == 119)
                T_bt = butt_Excel;    //엑셀  F8    
            if (e.KeyValue == 112)
                T_bt = butt_Clear;    //엑셀  F5    

            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123 || e.KeyValue == 113 || e.KeyValue == 115 || e.KeyValue == 119 || e.KeyValue == 112)
                    Base_Button_Click(T_bt, ee1);
            }
        }



        private void MtxtData_KeyPress(object sender, KeyPressEventArgs e)
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

                MaskedTextBox mtb = (MaskedTextBox)sender;

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
                            if (Input_Error_Check(mtb,0) == true)
                                Set_Form_Date(mtb.Text);
                            //SendKeys.Send("{TAB}");

                        }
       
                    }

                    else if (reCnt > 1)  //회원번호 비슷한 사람들이 많은 경우
                    {
                        string Mbid = "";
                        int Mbid2 = 0;
                        cds.Member_Nmumber_Split(mtb.Text, ref Mbid, ref Mbid2);

                        //cls_app_static_var.Search_Member_Number_Mbid = Mbid;
                        //cls_app_static_var.Search_Member_Number_Mbid2 = Mbid2;
                        frmBase_Member_Search e_f = new frmBase_Member_Search();

                        if (mtb.Name == "mtxtMbid")
                        {
                            e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                            e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info);
                        }

                        e_f.ShowDialog();

                        SendKeys.Send("{TAB}");
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
             
                //변경할려는 대상자에 대한 회원번호에서 회원 검색창을 뛰엇을 경우에
        void e_f_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            mtxtMbid.Text = Send_Number;            txtName.Text = Send_Name;
            if (Input_Error_Check(mtxtMbid, 0) == true)
                Set_Form_Date(mtxtMbid.Text);
        }

  


        //회원번호 입력 박스의 내역이 모두 지워지면 하부 관련 회원데이타 내역을 다 리셋 시킨다. 
        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {
            MaskedTextBox mtb = (MaskedTextBox)sender;

            if (mtb.Text.Replace("_", "").Replace("-", "").Replace(" ", "") == "")
            {
                cls_form_Meth ct = new cls_form_Meth();
                if (mtb.Name == "mtxtMbid")
                {
                    ct.from_control_clear(this, mtb);
                    
                }
                //    ct.from_control_clear(groupBox2, mtb);

                //ct.from_control_clear((GroupBox)mtb.Parent, mtb);
            }
        }


        //텍스트 박스들 포커스가 갓을때 전제 선택이 되도록함.
        private void txtData_Enter(object sender, EventArgs e)
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

        private void txtData_Base_Leave(object sender, EventArgs e)
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


        //텍스트 박스 키 프레스 이벤트
        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
            T_R.Key_Enter_13_Name += new Key_13_Name_Event_Handler(T_R_Key_Enter_13_Name);
          
            TextBox tb = (TextBox)sender;

            if ((tb.Tag == null) || (tb.Tag.ToString() == ""))
            {                
                if (T_R.Text_KeyChar_Check(e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if   
            }
            else if (tb.Tag.ToString() == "1") //숫자만 입력 가능하다.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if (tb.Tag.ToString() == "name")  //회원 정보 관련해서 이름 검색을 필요로 하는 텍스트 박스이다.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(tb, e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }
        }


        //텍스트 박스의 기준 길이만큰 다 차면 다음 지정된 탭으로 이동을 해라 전화번호 관려련 사항때문에 첨가됨.
        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void T_R_Key_Enter_13_Name(string txt_tag, TextBox tb)
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
                        if (Input_Error_Check(mtxtMbid, 0) == true)
                            Set_Form_Date(mtxtMbid.Text);

                        //SendKeys.Send("{TAB}");
                    }

                    
                }
                else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
                {
                    
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

        void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
           seachName = txtName.Text.Trim() ;
        }
       
        
        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


        private void Sub_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            Data_Set_Form_TF = 1; 
            if (bt.Name == "butt_AddCode")
            {
                frmBase_AddCode e_f = new frmBase_AddCode();
                e_f.Send_Address_Info += new frmBase_AddCode.SendAddressDele(e_f_Send_Address_Info);
                e_f.ShowDialog();
            }

            else if (bt.Name == "butt_AddCode_C")
            {
                frmBase_AddCode e_f = new frmBase_AddCode();
                e_f.Send_Address_Info += new frmBase_AddCode.SendAddressDele(e_f_Send_Address_Info_C);
                e_f.ShowDialog();
            }

            else if (bt.Name == "butt_AddCode_R")
            {
                frmBase_AddCode e_f = new frmBase_AddCode();
                e_f.Send_Address_Info += new frmBase_AddCode.SendAddressDele(e_f_Send_Address_Info_R);
                e_f.ShowDialog();
            }

            else if (bt.Name == "butt_Top1")
            {
                txtAddCode_C_1.Text = txtAddCode1.Text ;
                txtAddCode_C_2.Text = txtAddCode2.Text ;
                txtAddress_C_1.Text = txtAddress1.Text;
                txtAddress_C_2.Text = txtAddress2.Text;

                txtTel_C_1.Text = txtTel_1.Text ;
                txtTel_C_2.Text = txtTel_2.Text ;
                txtTel_C_3.Text = txtTel_3.Text;

                txtTel2_C_1.Text = txtTel2_1.Text;
                txtTel2_C_2.Text = txtTel2_2.Text;
                txtTel2_C_3.Text = txtTel2_3.Text;

                txtAddress_R_1.Focus();
            }

            else if (bt.Name == "butt_Top2")
            {
                txtAddCode_R_1.Text = txtAddCode1.Text;
                txtAddCode_R_2.Text = txtAddCode2.Text;
                txtAddress_R_1.Text = txtAddress1.Text;
                txtAddress_R_2.Text = txtAddress2.Text;

                txtTel_R_1.Text = txtTel_1.Text;
                txtTel_R_2.Text = txtTel_2.Text;
                txtTel_R_3.Text = txtTel_3.Text;

                txtTel2_R_1.Text = txtTel2_1.Text;
                txtTel2_R_2.Text = txtTel2_2.Text;
                txtTel2_R_3.Text = txtTel2_3.Text;

                txtName_R.Focus();
            }

            else if (bt.Name == "butt_Top3")
            {
                txtAddCode_R_1.Text = txtAddCode_C_1.Text;
                txtAddCode_R_2.Text = txtAddCode_C_2.Text;
                txtAddress_R_1.Text = txtAddress_C_1.Text;
                txtAddress_R_2.Text = txtAddress_C_2.Text;

                txtTel_R_1.Text = txtTel_C_1.Text;
                txtTel_R_2.Text = txtTel_C_2.Text;
                txtTel_R_3.Text = txtTel_C_3.Text;

                txtTel2_R_1.Text = txtTel2_C_1.Text;
                txtTel2_R_2.Text = txtTel2_C_2.Text;
                txtTel2_R_3.Text = txtTel2_C_3.Text;

                txtName_R.Focus();
            }

            Data_Set_Form_TF = 0; 
        }


        private void e_f_Send_Address_Info(string AddCode1, string AddCode2, string Address1, string Address2, string Address3)
        {
            txtAddCode1.Text = AddCode1; txtAddCode2.Text = AddCode2;
            txtAddress1.Text = Address1; txtAddress2.Text = Address2;

            txtAddress2.Focus();
        }

        private void e_f_Send_Address_Info_C(string AddCode1, string AddCode2, string Address1, string Address2, string Address3)
        {
            txtAddCode_C_1.Text = AddCode1; txtAddCode_C_2.Text = AddCode2;
            txtAddress_C_1.Text = Address1; txtAddress_C_2.Text = Address2;

            txtAddress_C_2.Focus();
        }

        private void e_f_Send_Address_Info_R(string AddCode1, string AddCode2, string Address1, string Address2, string Address3)
        {
            txtAddCode_R_1.Text = AddCode1; txtAddCode_R_2.Text = AddCode2;
            txtAddress_R_1.Text = Address1; txtAddress_R_2.Text = Address2;

            txtAddress_R_2.Focus();
        }


      

        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            
            if (bt.Name == "butt_Clear")
            {                
                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtMbid);                 
            }
                
            else if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Save_Base_Data(ref Save_Error_Check);

                if (Save_Error_Check > 0)
                {                    
                    cls_form_Meth ct = new cls_form_Meth();
                    ct.from_control_clear(this, mtxtMbid);                     
                }

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            
            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }                         
        }





        private Boolean  Input_Error_Check(MaskedTextBox  m_tb, int s_Kind )
        {
            string T_Mbid = m_tb.Text;
            string Mbid = ""; int Mbid2 = 0;

            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == -1) //올바르게 회원번호 양식에 맞춰서 입력햇는가.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus();                return false;
            }
            
            string Tsql = "";
            Tsql = "Select Mbid , Mbid2, M_Name ";
            Tsql = Tsql + " , LineCnt , N_LineCnt  ";
            Tsql = Tsql + " , LeaveDate , LineUserDate  " ;
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
            Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

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
                m_tb.Focus();                return false;
            }
            //++++++++++++++++++++++++++++++++   


            return true ;
        }

        





        private void Set_Form_Date(string  T_Mbid)
        {
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

                Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

                Tsql = Tsql + ",  tbl_Memberinfo.Cpno ";

                Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";

                Tsql = Tsql + " , LEFT(tbl_Memberinfo.RegTime,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.RegTime,4),2) + '-' + RIGHT(tbl_Memberinfo.RegTime,2)  AS RegTime  ";                
        
                Tsql = Tsql + "  , Add_TF ";
                Tsql = Tsql + " , tbl_Memberinfo.hptel ";
                Tsql = Tsql + " , tbl_Memberinfo.hometel ";
                Tsql = Tsql + " , tbl_Memberinfo.address1 " ;
                Tsql = Tsql + " , tbl_Memberinfo.address2 ";
                Tsql = Tsql + " , tbl_Memberinfo.Addcode1 ";
                                
                Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
                
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                }
                //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";

                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;
                //++++++++++++++++++++++++++++++++

                mtxtMbid.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();
                txtName.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();

                
                txtCpno.Text = encrypter.Decrypt(  ds.Tables[base_db_name].Rows[0]["Cpno"].ToString(),"Cpno");             
               

                txtAddress1.Text = encrypter.Decrypt( ds.Tables[base_db_name].Rows[0]["address1"].ToString());
                txtAddress2.Text = encrypter.Decrypt( ds.Tables[base_db_name].Rows[0]["address2"].ToString());



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

                

                if (ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString().Length >= 6)
                {
                    txtAddCode1.Text = ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString().Substring(0, 3);
                    txtAddCode2.Text = ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString().Substring(3, 3);
                }

                string T_Num_1  = "";  string T_Num_2  = ""; string T_Num_3  = "";
                cls_form_Meth cfm = new cls_form_Meth();
                cfm.Phone_Number_Split (encrypter.Decrypt( ds.Tables[base_db_name].Rows[0]["hptel"].ToString()), ref T_Num_1, ref T_Num_2,  ref T_Num_3);
                txtTel2_1.Text =T_Num_1;  txtTel2_2.Text =T_Num_2; txtTel2_3.Text =T_Num_3;

                cfm.Phone_Number_Split(encrypter.Decrypt( ds.Tables[base_db_name].Rows[0]["hometel"].ToString()), ref T_Num_1, ref T_Num_2, ref T_Num_3);
                txtTel_1.Text =T_Num_1;  txtTel_2.Text =T_Num_2; txtTel_3.Text =T_Num_3;
                

                Set_Mem_Address_Info_C(Mbid,Mbid2, "C" ); //직장주소

                Set_Mem_Address_Info_R(Mbid, Mbid2, "R"); //기본배송지 주소
            }

            Data_Set_Form_TF = 0;
        }



        private void Set_Mem_Address_Info_C(string Mbid, int Mbid2, string   Sort_Add )
        {
            string Tsql = "";
            Tsql = "Select  ";
            Tsql = Tsql + " ETC_Tel_1 ";
            Tsql = Tsql + " , ETC_Tel_2 ";
            Tsql = Tsql + " , Etc_Name ";
            Tsql = Tsql + " , ETC_Address1 ";
            Tsql = Tsql + " , ETC_Address2 ";
            Tsql = Tsql + " , ETC_Address3 ";
            Tsql = Tsql + " , ETC_Addcode1 ";

            Tsql = Tsql + " From tbl_Memberinfo_Address (nolock) ";

            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " And  Sort_Add = '" + Sort_Add  +  "'";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            txtAddress_C_1.Text = encrypter.Decrypt ( ds.Tables[base_db_name].Rows[0]["ETC_address1"].ToString());
            txtAddress_C_2.Text = encrypter.Decrypt (ds.Tables[base_db_name].Rows[0]["ETC_address2"].ToString());

            if (ds.Tables[base_db_name].Rows[0]["ETC_Addcode1"].ToString().Length >= 6)
            {
                txtAddCode_C_1.Text = ds.Tables[base_db_name].Rows[0]["ETC_Addcode1"].ToString().Substring(0, 3);
                txtAddCode_C_2.Text = ds.Tables[base_db_name].Rows[0]["ETC_Addcode1"].ToString().Substring(3, 3);
            }

            string T_Num_1 = ""; string T_Num_2 = ""; string T_Num_3 = "";
            cls_form_Meth cfm = new cls_form_Meth();
            cfm.Phone_Number_Split(encrypter.Decrypt (ds.Tables[base_db_name].Rows[0]["ETC_Tel_1"].ToString()), ref T_Num_1, ref T_Num_2, ref T_Num_3);
            txtTel_C_1.Text = T_Num_1; txtTel_C_2.Text = T_Num_2; txtTel_C_3.Text = T_Num_3;

            cfm.Phone_Number_Split(encrypter.Decrypt (ds.Tables[base_db_name].Rows[0]["ETC_Tel_2"].ToString()), ref T_Num_1, ref T_Num_2, ref T_Num_3);
            txtTel2_C_1.Text = T_Num_1; txtTel2_C_2.Text = T_Num_2; txtTel2_C_3.Text = T_Num_3;           
        }



        private void Set_Mem_Address_Info_R(string Mbid, int Mbid2, string Sort_Add)
        {
            string Tsql = "";
            Tsql = "Select  ";
            Tsql = Tsql + " ETC_Tel_1 ";
            Tsql = Tsql + " , ETC_Tel_2 ";
            Tsql = Tsql + " , Etc_Name ";
            Tsql = Tsql + " , ETC_Address1 ";
            Tsql = Tsql + " , ETC_Address2 ";
            Tsql = Tsql + " , ETC_Address3 ";
            Tsql = Tsql + " , ETC_Addcode1 ";

            Tsql = Tsql + " From tbl_Memberinfo_Address (nolock) ";

            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " And  Sort_Add = '" + Sort_Add + "'";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            txtAddress_R_1.Text = encrypter.Decrypt (ds.Tables[base_db_name].Rows[0]["ETC_address1"].ToString());
            txtAddress_R_2.Text = encrypter.Decrypt (ds.Tables[base_db_name].Rows[0]["ETC_address2"].ToString());

            if (ds.Tables[base_db_name].Rows[0]["ETC_Addcode1"].ToString().Length >= 6)
            {
                txtAddCode_R_1.Text = ds.Tables[base_db_name].Rows[0]["ETC_Addcode1"].ToString().Substring(0, 3);
                txtAddCode_R_2.Text = ds.Tables[base_db_name].Rows[0]["ETC_Addcode1"].ToString().Substring(3, 3);
            }

            string T_Num_1 = ""; string T_Num_2 = ""; string T_Num_3 = "";
            cls_form_Meth cfm = new cls_form_Meth();
            cfm.Phone_Number_Split(encrypter.Decrypt (ds.Tables[base_db_name].Rows[0]["ETC_Tel_1"].ToString()), ref T_Num_1, ref T_Num_2, ref T_Num_3);
            txtTel_R_1.Text = T_Num_1; txtTel_R_2.Text = T_Num_2; txtTel_R_3.Text = T_Num_3;

            cfm.Phone_Number_Split(encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["ETC_Tel_2"].ToString()), ref T_Num_1, ref T_Num_2, ref T_Num_3);
            txtTel2_R_1.Text = T_Num_1; txtTel2_R_2.Text = T_Num_2; txtTel2_R_3.Text = T_Num_3;

            
             txtName_R.Text = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["ETC_Name"].ToString());
        }


        private Boolean Check_TextBox_Error()
        {
            cls_Check_Input_Error c_er = new cls_Check_Input_Error();

            if (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                int Ret = 0;
                Ret = c_er._Member_Nmumber_Split(mtxtMbid);

                if (Ret == -1)
                {
                    mtxtMbid.Focus(); return false;
                }
            }
            else
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtMbid.Focus(); return false;
            }


            if ((txtAddCode1.Text.Trim() != "") || (txtAddCode2.Text.Trim() != ""))
            {
                if ((txtAddCode1.Text.Trim() == "") || (txtAddCode2.Text.Trim() == ""))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_AddCode")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtAddCode1.Focus();
                    return false;
                }
            }//우편번호가 다 입력이 되엇는지 체크를 한다.


            if ((txtTel_1.Text.Trim() != "") || (txtTel_2.Text.Trim() != "") || (txtTel_3.Text.Trim() != ""))
            {
                if ((txtTel_1.Text.Trim() == "") || (txtTel_2.Text.Trim() == "") || (txtTel_3.Text.Trim() == ""))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Tel")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtTel_1.Focus();
                    return false;
                }

            } //전화 번호가 3칸다 제대로 들어 왓는지 체크를 한다.  


            if ((txtTel2_1.Text.Trim() != "") || (txtTel2_2.Text.Trim() != "") || (txtTel2_3.Text.Trim() != ""))
            {
                if ((txtTel2_1.Text.Trim() == "") || (txtTel2_2.Text.Trim() == "") || (txtTel2_3.Text.Trim() == ""))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Fax")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtTel2_1.Focus();
                    return false;
                }
            } //팩스 번호가 제대로 들어 왓는지 체크한다.



            return true;
        }



        private Boolean Check_TextBox_Error_C()
        {
        

            if ((txtAddCode_C_1.Text.Trim() != "") || (txtAddCode_C_2.Text.Trim() != ""))
            {
                if ((txtAddCode_C_1.Text.Trim() == "") || (txtAddCode_C_2.Text.Trim() == ""))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_AddCode")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtAddCode_C_1.Focus();
                    return false;
                }
            }//우편번호가 다 입력이 되엇는지 체크를 한다.


            if ((txtTel_C_1.Text.Trim() != "") || (txtTel_C_2.Text.Trim() != "") || (txtTel_C_3.Text.Trim() != ""))
            {
                if ((txtTel_C_1.Text.Trim() == "") || (txtTel_C_2.Text.Trim() == "") || (txtTel_C_3.Text.Trim() == ""))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Tel")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtTel_C_1.Focus();
                    return false;
                }

            } //전화 번호가 3칸다 제대로 들어 왓는지 체크를 한다.  


            if ((txtTel2_C_1.Text.Trim() != "") || (txtTel2_C_2.Text.Trim() != "") || (txtTel2_C_3.Text.Trim() != ""))
            {
                if ((txtTel2_C_1.Text.Trim() == "") || (txtTel2_C_2.Text.Trim() == "") || (txtTel2_C_3.Text.Trim() == ""))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Fax")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtTel2_C_1.Focus();
                    return false;
                }
            } //팩스 번호가 제대로 들어 왓는지 체크한다.



            return true;
        }


        private Boolean Check_TextBox_Error_R()
        {


            if ((txtAddCode_R_1.Text.Trim() != "") || (txtAddCode_R_2.Text.Trim() != ""))
            {
                if ((txtAddCode_R_1.Text.Trim() == "") || (txtAddCode_R_2.Text.Trim() == ""))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_AddCode")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtAddCode_R_1.Focus();
                    return false;
                }
            }//우편번호가 다 입력이 되엇는지 체크를 한다.


            if ((txtTel_R_1.Text.Trim() != "") || (txtTel_R_2.Text.Trim() != "") || (txtTel_R_3.Text.Trim() != ""))
            {
                if ((txtTel_R_1.Text.Trim() == "") || (txtTel_R_2.Text.Trim() == "") || (txtTel_R_3.Text.Trim() == ""))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Tel")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtTel_R_1.Focus();
                    return false;
                }

            } //전화 번호가 3칸다 제대로 들어 왓는지 체크를 한다.  


            if ((txtTel2_R_1.Text.Trim() != "") || (txtTel2_R_2.Text.Trim() != "") || (txtTel2_R_3.Text.Trim() != ""))
            {
                if ((txtTel2_R_1.Text.Trim() == "") || (txtTel2_R_2.Text.Trim() == "") || (txtTel2_R_3.Text.Trim() == ""))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Fax")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtTel2_R_1.Focus();
                    return false;
                }
            } //팩스 번호가 제대로 들어 왓는지 체크한다.



            return true;
        }
       


       
        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
                        
            if (Check_TextBox_Error() == false) return;
            if (Check_TextBox_Error_C() == false) return;
            if (Check_TextBox_Error_R() == false) return;
            if (Input_Error_Check(mtxtMbid, 0) == false) return ;
            





            cls_Search_DB csd = new cls_Search_DB();
            csd.Member_Mod_BackUp(mtxtMbid.Text.Trim(),"tbl_Memberinfo");

            cls_Search_DB csd_C = new cls_Search_DB();
            csd_C.Member_Mod_BackUp(mtxtMbid.Text.Trim(), "tbl_Memberinfo_Address", " And Sort_Add = 'C' " );

            cls_Search_DB csd_R = new cls_Search_DB();
            csd_R.Member_Mod_BackUp(mtxtMbid.Text.Trim(), "tbl_Memberinfo_Address", " And Sort_Add = 'R' ");

            
            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                string hometel = ""; string hptel = ""; int Add_TF = 0;

                
                if (txtTel_1.Text != "") hometel = txtTel_1.Text.Trim() + '-' + txtTel_2.Text.Trim() + '-' + txtTel_3.Text.Trim();
                if (txtTel2_1.Text != "") hptel = txtTel2_1.Text.Trim() + '-' + txtTel2_2.Text.Trim() + '-' + txtTel2_3.Text.Trim();
                if (opt_B_1.Checked == true) Add_TF = 1;  //기본주소가 
                if (opt_B_2.Checked == true)  Add_TF = 2; //회사 주소가
                if (opt_B_3.Checked == true) Add_TF = 3; //기본배송지 주소가

                string StrSql = "";
                StrSql = "Update tbl_Memberinfo Set ";
                StrSql = StrSql + "  Addcode1 = '" + txtAddCode1.Text.Trim() + txtAddCode2.Text.Trim() + "'";
                StrSql = StrSql + " ,Address1 = '" + encrypter.Encrypt (txtAddress1.Text.Trim() )+ "'";
                StrSql = StrSql + " ,Address2 = '" + encrypter.Encrypt (txtAddress2.Text.Trim()) + "'";
                StrSql = StrSql + " ,hometel = '" + encrypter.Encrypt (hometel) + "'";
                StrSql = StrSql + " ,hptel = '" + encrypter.Encrypt (hptel )+ "'";
                StrSql = StrSql + " ,Add_TF = " + Add_TF.ToString () ;                
                StrSql = StrSql + " Where mbid = '" + Mbid + "'";
                StrSql = StrSql + " And mbid2 = " + Mbid2.ToString();            

                
                Temp_Connect.Update_Data(StrSql,  Conn, tran);

                Chang_Mem_Address_C(Mbid,Mbid2,Temp_Connect,Conn, tran);
                Chang_Mem_Address_R(Mbid, Mbid2, Temp_Connect, Conn, tran);
                
                tran.Commit();
                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));

                csd.tbl_Memberinfo_Mod(mtxtMbid.Text.Trim());
                csd_C.tbl_Memberinfo_Mod(mtxtMbid.Text.Trim(), "C", "tbl_Memberinfo_Address", " And Sort_Add = 'C' ");
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

        private void Chang_Mem_Address_C(string Mbid, int Mbid2, cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string ETC_Tel_1 = ""; string ETC_Tel_2 = "";
            string StrSql = "";
 
            StrSql = "Select Sort_Add , Mbid, Mbid2 ";
            StrSql = StrSql + " From tbl_Memberinfo_Address  (nolock)  ";
            StrSql = StrSql + " Where mbid = '" + Mbid + "'";
            StrSql = StrSql + " And mbid2 = " + Mbid2.ToString();
            StrSql = StrSql + " And Sort_Add = 'C' ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Memberinfo_Address", ds) == true)
            {
                if (txtTel_C_1.Text != "") ETC_Tel_1 = txtTel_C_1.Text.Trim() + '-' + txtTel_C_2.Text.Trim() + '-' + txtTel_C_3.Text.Trim();
                if (txtTel2_C_1.Text != "") ETC_Tel_2 = txtTel2_C_1.Text.Trim() + '-' + txtTel2_C_2.Text.Trim() + '-' + txtTel2_C_3.Text.Trim();

                if (Temp_Connect.DataSet_ReCount == 0)//동일한 이름으로 코드가 있다 그럼.이거 저장하면 안되요
                {
                    

                    StrSql = "Insert into tbl_Memberinfo_Address (";
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

                    StrSql = StrSql + " 'C' ";
                    StrSql = StrSql + ",'" + Mbid + "'";
                    StrSql = StrSql + "," + Mbid2.ToString();
                    StrSql = StrSql + ", '" + txtAddCode_C_1.Text.Trim() + txtAddCode_C_2.Text.Trim() + "'";
                    StrSql = StrSql + ", '" + encrypter.Encrypt (txtAddress_C_1.Text.Trim()) + "'";
                    StrSql = StrSql + ", '" + encrypter.Encrypt (txtAddress_C_2.Text.Trim()) + "'";
                    StrSql = StrSql + ", '' ";
                    StrSql = StrSql + ", '" + encrypter.Encrypt (ETC_Tel_1) + "'";
                    StrSql = StrSql + ", '" + encrypter.Encrypt (ETC_Tel_2) + "'";
                    StrSql = StrSql + ", '' ";
                    StrSql = StrSql + ",'" + cls_User.gid + "'";
                    StrSql = StrSql + ", Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " ) ";

                    Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_Address", Conn, tran);
                }
                else
                {
                    StrSql = "Update tbl_Memberinfo_Address Set ";
                    StrSql = StrSql + "  ETC_Addcode1 = '" + txtAddCode_C_1.Text.Trim() + txtAddCode_C_2.Text.Trim() + "'";
                    StrSql = StrSql + " ,ETC_Address1 = '" + encrypter.Encrypt (txtAddress_C_1.Text.Trim()) + "'";
                    StrSql = StrSql + " ,ETC_Address2 = '" + encrypter.Encrypt (txtAddress_C_2.Text.Trim()) + "'";
                    StrSql = StrSql + " ,ETC_Address3 = ''";
                    StrSql = StrSql + " ,ETC_Tel_1 = '" + encrypter.Encrypt ( ETC_Tel_1) + "'";
                    StrSql = StrSql + " ,ETC_Tel_2 = '" + encrypter.Encrypt ( ETC_Tel_2) + "'";
                    StrSql = StrSql + " Where mbid = '" + Mbid + "'";
                    StrSql = StrSql + " And mbid2 = " + Mbid2.ToString();
                    StrSql = StrSql + " And Sort_Add = 'C' ";

                    Temp_Connect.Update_Data (StrSql,  Conn, tran);

                }
            }
        }


        private void Chang_Mem_Address_R(string Mbid, int Mbid2, cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string ETC_Tel_1 = ""; string ETC_Tel_2 = "";
            string StrSql = "";

            StrSql = "Select Sort_Add , Mbid, Mbid2 ";
            StrSql = StrSql + " From tbl_Memberinfo_Address  (nolock)  ";
            StrSql = StrSql + " Where mbid = '" + Mbid + "'";
            StrSql = StrSql + " And mbid2 = " + Mbid2.ToString();
            StrSql = StrSql + " And Sort_Add = 'R' ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Memberinfo_Address", ds) == true)
            {
                if (txtTel_R_1.Text != "") ETC_Tel_1 = txtTel_R_1.Text.Trim() + '-' + txtTel_R_2.Text.Trim() + '-' + txtTel_R_3.Text.Trim();
                if (txtTel2_R_1.Text != "") ETC_Tel_2 = txtTel2_R_1.Text.Trim() + '-' + txtTel2_R_2.Text.Trim() + '-' + txtTel2_R_3.Text.Trim();

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
                    StrSql = StrSql + ", '" + txtAddCode_R_1.Text.Trim() + txtAddCode_R_2.Text.Trim() + "'";
                    StrSql = StrSql + ", '" + encrypter.Encrypt (txtAddress_R_1.Text.Trim()) + "'";
                    StrSql = StrSql + ", '" + encrypter.Encrypt (txtAddress_R_2.Text.Trim()) + "'";
                    StrSql = StrSql + ", '' ";
                    StrSql = StrSql + ", '" + encrypter.Encrypt (ETC_Tel_1) + "'";
                    StrSql = StrSql + ", '" + encrypter.Encrypt (ETC_Tel_2) + "'";
                    StrSql = StrSql + ", '" + encrypter.Encrypt (txtName_R.Text.Trim ()) + "'";
                    StrSql = StrSql + ",'" + cls_User.gid + "'";
                    StrSql = StrSql + ", Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " ) ";

                    Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_Address", Conn, tran);
                }
                else
                {
                    StrSql = "Update tbl_Memberinfo_Address Set ";
                    StrSql = StrSql + "  ETC_Addcode1 = '" + txtAddCode_R_1.Text.Trim() + txtAddCode_R_2.Text.Trim() + "'";
                    StrSql = StrSql + " ,ETC_Address1 = '" + encrypter.Encrypt (txtAddress_R_1.Text.Trim()) + "'";
                    StrSql = StrSql + " ,ETC_Address2 = '" + encrypter.Encrypt (txtAddress_R_2.Text.Trim()) + "'";
                    StrSql = StrSql + " ,ETC_Address3 = ''";
                    StrSql = StrSql + " ,ETC_Tel_1 = '" + encrypter.Encrypt (ETC_Tel_1) + "'";
                    StrSql = StrSql + " ,ETC_Tel_2 = '" + encrypter.Encrypt (ETC_Tel_2) + "'";
                    StrSql = StrSql + " ,ETC_Name = '" + encrypter.Encrypt (txtName_R.Text.Trim() ) + "'";
                    StrSql = StrSql + " Where mbid = '" + Mbid + "'";
                    StrSql = StrSql + " And mbid2 = " + Mbid2.ToString();
                    StrSql = StrSql + " And Sort_Add = 'R' ";

                    Temp_Connect.Update_Data(StrSql, Conn, tran);

                }
            }
        }

        

        //회원번호 클릿햇을때. 관련 정보들 다 리셋 시킨다.
        //추후 번호만 변경하고 엔터 안누눌러서.. 데이타가 엉키는 것을 방지하기 위함.
        private void mtxtMbid_Click(object sender, EventArgs e)
        {
            MaskedTextBox mtb = (MaskedTextBox)sender;

            if (mtb.Name == "mtxtMbid")
            {
                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtMbid);        
            }


            //마스크텍스트 박스에 입력한 내용이 있으면 그곳 다음으로 커서가 가게 한다.
            if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                mtb.SelectionStart = mtb.Text.Replace("-", "").Replace("_", "").Trim().Length + 1;
        }

        private void chk_Base_1_MouseClick(object sender, MouseEventArgs e)
        {
            CheckBox tck = (CheckBox)sender; 
            EventArgs e1 =null ;

            Data_Set_Form_TF = 1; 
            if (tck.Name == "chk_Base_1")
                Sub_Button_Click (butt_Top1,e1 );

            if (tck.Name == "chk_Base_2")
                Sub_Button_Click (butt_Top2,e1 );

            if (tck.Name == "chk_Com_1")
                Sub_Button_Click(butt_Top3, e1);

            Data_Set_Form_TF = 0;
        }








    }
}
