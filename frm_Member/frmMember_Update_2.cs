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
    public partial class frmMember_Update_2 : clsForm_Extends
    {

        cls_Grid_Base cg_Up_S = new cls_Grid_Base();
        cls_Grid_Base cg_Li = new cls_Grid_Base();

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        private const string base_db_name = "tbl_Memberinfo";
        private int Data_Set_Form_TF;
        private string idx_Mbid = "";
        private int idx_Mbid2 = 0; private int idx_LineCnt = -1;
        private string idx_Org_Mbid = ""; int idx_Org_Mbid2 = -1;
        private string idx_LineDate = "";

        public delegate void Take_NumberDele(ref string Send_Number, ref string Send_Name);
        public event Take_NumberDele Take_Mem_Number;

        public frmMember_Update_2()
        {
            InitializeComponent();
        }



        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;
          
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
            
            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;            
            mtxtSn.Mask = "999999-9999999"; //기본 셋팅은 주민번호이다. 

            if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
            {               
                tbl_save.Visible = false;
                //tbl_save2.Visible = false;
                 tab_inf.TabPages.Remove(tab_save);
                
            }

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                tbl_nom.Visible = false;                
                tab_inf.TabPages.Remove(tab_nom);                
            }

            mtxtMbid.Focus();

            //2018-08-20 지성경 막음 
            tab_Base.TabPages.Remove(tab_Line);

        }


        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (cls_User.uSearch_MemberNumber != "")
            {
                Data_Set_Form_TF = 1;
                mtxtMbid.Text = cls_User.uSearch_MemberNumber;
                cls_User.uSearch_MemberNumber = "";
                //EventArgs ee1 = null; Select_Button_Click(butt_Select, ee1);
                Set_Form_Date(mtxtMbid.Text, "m");

                Data_Set_Form_TF = 0;
            }

            ////string Send_Number = ""; string Send_Name = "";
            ////Take_Mem_Number(ref Send_Number, ref Send_Name);

            ////if (Send_Number != "")
            ////{
            ////    mtxtMbid.Text = Send_Number;
            ////    Set_Form_Date(mtxtMbid.Text, "m");
            ////}

        }


        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Save.Left = butt_Clear.Left + butt_Clear.Width + 2;
            //butt_Excel.Left = butt_Save.Left + butt_Save.Width + 2;
            if (Environment.MachineName.Equals("LANCE1") || cls_User.gid.Equals("admin"))
            {
                butt_Delete.Visible = true;
                butt_Delete.Left = butt_Save.Left + butt_Save.Width + 2;
            }
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Save);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);

            cfm.button_flat_change(butt_Save_Name);
            cfm.button_flat_change(butt_Save_Leave);
            cfm.button_flat_change(butt_Save_Line);
            
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

            ////그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
            //if (sender is DataGridView)
            //{
            //    if (e.KeyValue == 46)
            //    {
            //        e.Handled = true;
            //    } // end if                
            //}

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

                    else if (reCnt <= 0)  //회원번호 비슷한 사람들이 많은 경우
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

   

        //변경할려는 대상자에 대한 회원번호에서 회원 검색창을 뛰엇을 경우에
        void e_f_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            mtxtMbid.Text = Send_Number; txtName.Text = Send_Name;
            if (Input_Error_Check(mtxtMbid, "m") == true)
                Set_Form_Date(mtxtMbid.Text, "m");
        }


        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            MaskedTextBox mtb = (MaskedTextBox)sender;

            if (mtb.Text.Replace("_", "").Replace("-", "").Replace(" ", "") == "")
            {                       
                if (mtb.Name == "mtxtMbid")
                {
                    _From_Data_Clear();                
                }
                
            }
        }


        ////회원번호 클릿햇을때. 관련 정보들 다 리셋 시킨다.
        ////추후 번호만 변경하고 엔터 안누눌러서.. 데이타가 엉키는 것을 방지하기 위함.
        //private void mtxtMbid_Click(object sender, EventArgs e)
        //{
        //    MaskedTextBox mtb = (MaskedTextBox)sender;
                        
        //    if (mtb.Name == "mtxtMbid")
        //    {
        //        _From_Data_Clear();                
        //    }
           

        //    //마스크텍스트 박스에 입력한 내용이 있으면 그곳 다음으로 커서가 가게 한다.
        //    if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
        //        mtb.SelectionStart = mtb.Text.Replace("-", "").Replace("_", "").Trim().Length + 1;

        //}




        private void MtxtData_Sn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    string Sn = mtb.Text.Replace("-", "").Replace("_", "").Trim();
                    Sn_Number_(Sn, mtb);                   
                }
                else
                    SendKeys.Send("{TAB}");                                
            }
        }



        private bool Sn_Number_(string Sn, MaskedTextBox mtb)
        {
            if (Sn != "")
            {
                string sort_TF = "";
                bool check_b = false;
                cls_Sn_Check csn_C = new cls_Sn_Check();

                if (raButt_IN_1.Checked == true) //내국인인 구분자
                    sort_TF = "in";

                if (raButt_IN_2.Checked == true) //외국인 구분자
                    sort_TF = "fo";

                if (raButt_IN_3.Checked == true) //사업자 구분자.
                    sort_TF = "biz";

                check_b = csn_C.Sn_Number_Check(Sn, sort_TF);

                if (check_b == false && sort_TF != "fo")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Error")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtb.Focus(); return false;
                }
                else
                {
                    if (cls_app_static_var.Member_Reg_Multi_TF == 0) //다구좌 불가능으로 해서 체크되어 잇는 경우
                    {//동일 주민번호로 해서 가입한 사람이 있는지를 체크한다.
                       
                        cls_Search_DB csb = new cls_Search_DB();
                        if (csb.Member_Multi_Sn_Search(Sn, idx_Mbid, idx_Mbid2) == false) //주민번호 오류는 위에서 체크를 함.
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Same")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            mtb.Focus();  return false;
                        }
                    }
                }


                if (raButt_IN_1.Checked == true && check_b == true) //내국인인 경우에는 주민번호 체크한다.
                {
                    string BirthDay2 = "";
                    if (csn_C.check_19_nai(Sn, ref BirthDay2) == false) //한국같은 경우에는 미성년자 필히 체크한다.
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_19")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        mtb.Focus(); return false;
                    }

                }
            }
            else
            {
                if (cls_app_static_var.Member_Cpno_Put_TF == 1) //주민번호 관련 필수입력인데 입력 안햇다.
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Put")
                            + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtb.Focus(); return false;
                }
            }

            return true;
        }






        private void Set_Form_Date(string T_Mbid, string T_sort )
        {
            _From_Data_Clear();   
            //idx_Mbid = ""; idx_Mbid2 = 0;
            string Mbid = ""; int Mbid2 = 0;
            Data_Set_Form_TF = 1;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == 1)
            {
                string Tsql = "";
                
                Tsql = "Select  ";
                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                    Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) AS M_Mbid ";
                else if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                    Tsql = Tsql + " tbl_Memberinfo.mbid2 AS M_Mbid ";
                else if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                    Tsql = Tsql + " tbl_Memberinfo.mbid AS M_Mbid ";

                Tsql = Tsql + " ,tbl_Memberinfo.mbid ";
                Tsql = Tsql + " ,tbl_Memberinfo.mbid2 ";
                Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";
                
                Tsql = Tsql + ", tbl_Memberinfo.Cpno  AS Cpno";
                Tsql = Tsql + ", tbl_Memberinfo.WebPassWord";
                

                Tsql = Tsql + " , tbl_Memberinfo.For_Kind_TF ";
                Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";
                
                Tsql = Tsql + " , tbl_Memberinfo.LeaveDate ";
                Tsql = Tsql + " , tbl_Memberinfo.LeaveCheck ";
                Tsql = Tsql + " , tbl_Memberinfo.LineUserDate ";
                Tsql = Tsql + " , tbl_Memberinfo.PayStop_Date ";

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    Tsql = Tsql + " ,tbl_Memberinfo.Saveid + '-' + Convert(Varchar,tbl_Memberinfo.Saveid2) AS T_Saveid ";
                }

                if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    Tsql = Tsql + " ,tbl_Memberinfo.Saveid2 AS T_Saveid ";
                }

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                {
                    Tsql = Tsql + " ,tbl_Memberinfo.Saveid AS T_Saveid ";
                }

                Tsql = Tsql + " , Isnull(Sav.M_Name,'') AS Save_Name ";
                Tsql = Tsql + " , tbl_Memberinfo.Saveid ";
                Tsql = Tsql + ",   Sav.Cpno   AS Save_Cpno";
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<  

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    Tsql = Tsql + " ,tbl_Memberinfo.Nominid + '-' + Convert(Varchar,tbl_Memberinfo.Nominid2) AS T_Nominid ";
                }

                if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    Tsql = Tsql + " ,tbl_Memberinfo.Nominid2 AS T_Nominid ";
                }

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                {
                    Tsql = Tsql + " ,tbl_Memberinfo.Nominid AS T_Nominid ";
                }


                Tsql = Tsql + " , Isnull(Nom.M_Name,'') AS Nomin_Name ";
                Tsql = Tsql + " , tbl_Memberinfo.Nominid ";
                Tsql = Tsql + ",  Nom.Cpno  AS Nom_Cpno";                
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<  

                Tsql = Tsql + ",tbl_Memberinfo.LeaveReason ";
                Tsql = Tsql + ",tbl_Memberinfo.LineDelReason ";

                Tsql = Tsql + " , tbl_leavereason.LeaveReason_code ";
                Tsql = Tsql + " , tbl_leavereason.leavereason_name ";

                Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Sav (nolock) ON tbl_Memberinfo.Saveid = Sav.Mbid And tbl_Memberinfo.Saveid2 = Sav.Mbid2 ";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Nom (nolock) ON tbl_Memberinfo.Nominid = Nom.Mbid And tbl_Memberinfo.Nominid2 = Nom.Mbid2 ";
                Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";
                Tsql = Tsql + " LEFT JOIN tbl_leavereason (nolock) ON tbl_Memberinfo.LeaveReason_code = tbl_leavereason.LeaveReason_code ";

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid.ToString() + "'";
                }

                //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
                Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text ) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;
                //++++++++++++++++++++++++++++++++

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Set_Form_Date(ds);

                Set_Form_Date_Up(2);    //직추천한 사람들을 뿌려줌
                Set_Form_Date_Up("S2");  //직후원한 사람들을 뿌려줌.

                select_Save_Dir_Down(); //후원인 기준 하선을 뿌려준다. 라인쪽 관련해서

                Set_Form_Date_Info(); //회원 매출 관련 뿌려줌
                this.Cursor = System.Windows.Forms.Cursors.Default; 

                mtxtMbid.Focus();                
            }
            
            Data_Set_Form_TF = 0;            
        }

        private void Set_Form_Date(DataSet ds)
        {
            idx_Mbid =  ds.Tables[base_db_name].Rows[0]["Mbid"].ToString();
            idx_Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[0]["Mbid2"].ToString());

            txtSellCode.Text = ds.Tables[base_db_name].Rows[0]["leavereason_name"].ToString();
            txtSellCode_Code.Text = ds.Tables[base_db_name].Rows[0]["LeaveReason_code"].ToString();


            mtxtMbid.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();
            txtName.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();
            txtName_C.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();
            mtxtSn.Text = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Cpno"].ToString(), "Cpno");
            txtLineCnt.Text = ds.Tables[base_db_name].Rows[0]["LineCnt"].ToString();
            idx_LineCnt = int.Parse ( ds.Tables[base_db_name].Rows[0]["LineCnt"].ToString()) ;


            radioB_0.Checked = false;
            radioB_1.Checked = true;
            radioB__100.Checked = false;
            radioB__1.Checked = false;
            int LeaveDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int LineUserDate = LeaveDate;
            int PayStop_Date = LeaveDate;

            int.TryParse(ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString(), out LeaveDate);
            int.TryParse(ds.Tables[base_db_name].Rows[0]["LineUserDate"].ToString(), out LineUserDate);
            int.TryParse(ds.Tables[base_db_name].Rows[0]["PayStop_Date"].ToString(), out PayStop_Date);

            string LeaveCheck = ds.Tables[base_db_name].Rows[0]["LeaveCheck"].ToString().Replace("_", "").Trim();
            if(LeaveCheck.Equals("1") == false)
                mtxtLeaveDate.Text = string.Format("{0:####-##-##}", LeaveDate); 

            if (LeaveCheck.Equals("0"))
            {
                radioB_0.Checked = true;
            }
            if (LeaveCheck.Equals("-1"))
            {
                radioB__1.Checked = true;
            }

            if (LeaveCheck.Equals("-100"))
            {
                radioB__100.Checked = true;
            }

            if (ds.Tables[base_db_name].Rows[0]["LineUserDate"].ToString().Replace("-", "").Trim() != "")
                mtxtLineDate.Text = string.Format("{0:####-##-##}", LineUserDate);  // ds.Tables[base_db_name].Rows[0]["Ed_Date"].ToString();
            idx_LineDate = mtxtLineDate.Text.Replace("-", "").Trim();

            if (ds.Tables[base_db_name].Rows[0]["PayStop_Date"].ToString().Replace("-", "").Trim() != "")
                txtPayDate.Text = string.Format("{0:####-##-##}", PayStop_Date);  // ds.Tables[base_db_name].Rows[0]["Ed_Date"].ToString();


            txt_leave_R.Text = ds.Tables[base_db_name].Rows[0]["LeaveReason"].ToString();
            txt_Line_R.Text = ds.Tables[base_db_name].Rows[0]["LineDelReason"].ToString();            
            
                                    
            // 내국인은 0 외국인은 1  사업자는 2
            if (ds.Tables[base_db_name].Rows[0]["For_Kind_TF"].ToString() == "0")
                raButt_IN_1.Checked = true;
            else if (ds.Tables[base_db_name].Rows[0]["For_Kind_TF"].ToString() == "1")
                raButt_IN_2.Checked = true;
            else
                raButt_IN_3.Checked = true;


            txtMbid_s.Text = ds.Tables[base_db_name].Rows[0]["T_Saveid"].ToString();
            txtName_s.Text = ds.Tables[base_db_name].Rows[0]["Save_Name"].ToString();
            txtSN_s.Text = encrypter.Decrypt (ds.Tables[base_db_name].Rows[0]["Save_Cpno"].ToString(),"Cpno");

            txtMbid_n.Text = ds.Tables[base_db_name].Rows[0]["T_Nominid"].ToString();
            txtName_n.Text = ds.Tables[base_db_name].Rows[0]["Nomin_Name"].ToString();
            txtSN_n.Text = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Nom_Cpno"].ToString(), "Cpno");

           // txtPassword.Text = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["WebPassWord"].ToString());
                 
            

            if (ds.Tables[base_db_name].Rows[0]["Saveid"].ToString() != "" && ds.Tables[base_db_name].Rows[0]["Saveid"].ToString().Substring(0, 1) == "*")
                chk_S.Checked = true;

            if (ds.Tables[base_db_name].Rows[0]["Nominid"].ToString() != "" && ds.Tables[base_db_name].Rows[0]["Nominid"].ToString().Substring(0, 1) == "*")
                chk_N.Checked = true;
            
            txtName.ReadOnly = true;
            txtName.BackColor = cls_app_static_var.txt_Enable_Color; 
            txtName.BorderStyle = BorderStyle.FixedSingle;
        }


        private void Set_Form_Date_Up(int intTemp) //추천 관련.
        {
            dGridView_Up_S_Header_Reset(dGridView_Down_N); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset();            

            Base_Grid_Down_Set("N");            
        }


        private void Set_Form_Date_Up(string strTemp)
        {            
            dGridView_Up_S_Header_Reset(dGridView_Down_S); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset();                        
            Base_Grid_Down_Set("S");            
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

            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) ";
            //else
            //    Tsql = Tsql + " tbl_Memberinfo.mbid2 ";

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2)  ";
            else if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                Tsql = Tsql + " tbl_Memberinfo.mbid2  ";
            else if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                Tsql = Tsql + " tbl_Memberinfo.mbid  ";




            Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

            if (tSort == "S")
            {
                Tsql = Tsql + " ,tbl_Memberinfo.LineCnt ";
                Tsql = Tsql + " From tbl_Memberinfo " ;
      

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Saveid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_Memberinfo.Saveid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Saveid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Saveid = '" + Mbid.ToString() + "'";
                }

                Tsql = Tsql + " Order By LineCnt ASC ";
            }
            else
            {
                Tsql = Tsql + " ,tbl_Memberinfo.N_LineCnt ";
                Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
       

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Nominid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_Memberinfo.Nominid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Nominid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Nominid = '" + Mbid.ToString() + "'";
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
                Set_gr_dic_Down(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            cg_Up_S.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cg_Up_S.db_grid_Obj_Data_Put();
        }



        private void Set_gr_dic_Down(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][4]                                                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }



        private void dGridView_Up_S_Header_Reset(DataGridView t_Dgv)
        {
            cg_Up_S.Grid_Base_Arr_Clear();

            cg_Up_S.grid_col_Count = 5;
            cg_Up_S.basegrid = t_Dgv; //dGridView_Up_S;
            cg_Up_S.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            //cg_sub.grid_Frozen_End_Count = 2;
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

            //cg_Up_S.basegrid.ColumnHeadersDefaultCellStyle.Font =
            //new Font(cg_Up_S.basegrid.Font.FontFamily, 8);
        }






        private void select_Save_Dir_Down()
        {
            string T_Mbid = txtMbid_s.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            Data_Set_Form_TF = 1;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == 1)
            {
                dGridView_Line_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cg_Li.d_Grid_view_Header_Reset();

                Base_Grid_Set(Mbid, Mbid2);
            }
        }

        private void Base_Grid_Set(string Mbid, int Mbid2)
        {
            string Tsql = "";

            Tsql = "Select  ";
            Tsql = Tsql + " tbl_Memberinfo.LineCnt ";
            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + ", tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) ";
            //else
            //    Tsql = Tsql + " ,tbl_Memberinfo.mbid2 ";

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                Tsql = Tsql + " ,tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2)  ";
            else if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                Tsql = Tsql + " ,tbl_Memberinfo.mbid2  ";
            else if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                Tsql = Tsql + " ,tbl_Memberinfo.mbid  ";

            Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";

        

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Saveid = '" + Mbid + "' ";
                Tsql = Tsql + " And   tbl_Memberinfo.Saveid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Saveid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Saveid = '" + Mbid.ToString() + "'";
            }


            Tsql = Tsql + " And   tbl_Memberinfo.LineCnt > 0 ";
            Tsql = Tsql + " Order by  tbl_Memberinfo.LineCnt";

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
            cg_Li.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cg_Li.db_grid_Obj_Data_Put();

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



        private void dGridView_Line_Header_Reset()
        {
            cg_Li.grid_col_Count = 5;
            cg_Li.basegrid = dGridView_Li;
            cg_Li.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            //cg_sub.grid_Frozen_End_Count = 2;
            cg_Li.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

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
            cg_Li.basegrid.RowHeadersVisible = false;
            //cg_Li.basegrid.Font.Size = 7.5;

            //cg_Li.basegrid.ColumnHeadersDefaultCellStyle.Font =
            //new Font(cg_Li.basegrid.Font.FontFamily, 8);


        }




        private void Set_Form_Date_Info()
        {
            cls_Grid_Base_info_Put cgbp7 = new cls_Grid_Base_info_Put();
            cgbp7.dGridView_Put_baseinfo(this, dGridView_Sell, "sell", mtxtMbid.Text);


            //cls_Grid_Base_info_Put cgbp8 = new cls_Grid_Base_info_Put();
            //cgbp8.dGridView_Put_baseinfo(this, dGridView_Pay, "pay", mtxtMbid.Text);
        }
        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
            //if (Base_Error_Check__01(1) == false)
            //    return;


            if (tb.Name == "txtSellCode")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtSellCode_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtSellCode_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtSellCode_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
           
            }



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

            string And_Sql = "";
            if(txtSellCode.Text != "")
            {
                And_Sql = " and leavereason_name like '%"+ txtSellCode.Text + "%'";
            }
            //if (tb.Name == "txtSellCode")
            //    cgb_Pop.Next_Focus_Control = txtSellCode;

            //else
            //{
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, "", "", And_Sql);
            //}
        }


        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);
            T_R.Key_Enter_13_Name += new Key_13_Name_Event_Handler(T_R_Key_Enter_13_Name);
            TextBox tb = (TextBox)sender;

            if ((tb.Tag == null) || (tb.Tag.ToString() == ""))
            {
                //쿼리문상 오류를 잡기 위함.
                if (T_R.Text_KeyChar_Check(e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if   
            }
            else if ((tb.Tag != null) && (tb.Tag.ToString() == "1"))
            {
                //숫자만 입력 가능
                if (T_R.Text_KeyChar_Check(e, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "-"))
            {
                //숫자와  - 만
                if (T_R.Text_KeyChar_Check(e, "-") == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }


            else if ((tb.Tag != null) && (tb.Tag.ToString() == "ncode")) //코드관련해서 코드를치면 관련 내역이 나오도록 하기 위함.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, tb) == false)
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




        private void MtxtData_Temp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    string Sn = mtb.Text.Replace("-", "").Replace("_", "").Trim();
                    if (mtb.Name == "mtxtBiz1")
                    {
                        if (Sn_Number_(Sn, mtb, "biz") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel1")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel2")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtZip1")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtBrithDay")
                    {
                        if (Sn_Number_(Sn, mtb, "Date") == true)
                            SendKeys.Send("{TAB}");
                    }

                    string R4_name = mtb.Name.Substring(mtb.Name.Length - 4, 4);
                    if (R4_name == "Date" || R4_name == "ate3" || R4_name == "ate1" || R4_name == "ate2" || R4_name == "ate4")
                    {
                        if (Sn_Number_(Sn, mtb, "Date") == true)
                            SendKeys.Send("{TAB}");
                    }
                    

                }
                else
                    SendKeys.Send("{TAB}");


            }
        }




        private bool Sn_Number_(string Sn, MaskedTextBox mtb, string sort_TF, int t_Sort2 = 0)
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
                            Ret = c_er.Input_Date_Err_Check(mtb);

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

                    mtb.Focus(); return false;
                }
            }

            return true;
        }





        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
          
            if ((sender is TextBox) == false) return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");       
            }
            if (tb.Name == "txtSellCode")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                {
                    txtSellCode_Code.Text = "";
                }
                Data_Set_Form_TF = 0;
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

        void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtName.Text.Trim();
        }           



        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


      









        private Boolean Input_Error_Check(MaskedTextBox m_tb, string s_Kind)
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
                m_tb.Focus(); return false;
            }

            string Tsql = "";
            Tsql = "Select Mbid , Mbid2, M_Name , Sell_Mem_TF  ";
            Tsql = Tsql + " , LineCnt , N_LineCnt  ";
            Tsql = Tsql + " , LeaveDate , LineUserDate  ";
            Tsql = Tsql + " , Saveid  , Saveid2  ";
            Tsql = Tsql + " , Nominid , Nominid2  ";
            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
   

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid.ToString() + "'";
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
                m_tb.Focus(); return false;
            }
            //++++++++++++++++++++++++++++++++            

            return true;
        }




















        private void _From_Data_Clear()
        {

            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Up_S_Header_Reset(dGridView_Down_N); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset(1);

            dGridView_Up_S_Header_Reset(dGridView_Down_S); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset(1);

            dGridView_Line_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Li.d_Grid_view_Header_Reset(1);

            cls_Grid_Base_info_Put cgbp7 = new cls_Grid_Base_info_Put();
            cgbp7.dGridView_Put_baseinfo( dGridView_Sell, "sell");
            
            cls_Grid_Base_info_Put cgbp8 = new cls_Grid_Base_info_Put();
            cgbp8.dGridView_Put_baseinfo( dGridView_Pay, "pay");
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            txtName.ReadOnly =false ;
            txtName.BackColor = SystemColors.Window;
            txtName.BorderStyle = BorderStyle.Fixed3D  ;

            tab_inf.SelectedIndex = 0;
            tab_Base.SelectedIndex = 0; 

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtMbid);
                        
            raButt_IN_1.Checked = true;          
            mtxtSn.Mask = "999999-9999999";
            idx_Mbid = ""; idx_Mbid2 = 0; idx_LineCnt = -1;
            idx_Org_Mbid = ""; idx_Org_Mbid2 = -1;
            idx_LineDate = "";
            mtxtMbid.Focus();
        }


        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {                
                _From_Data_Clear();                                
            }

            else if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Save_Base_Data(ref Save_Error_Check);

                if (Save_Error_Check > 0)               
                    _From_Data_Clear();                                     
                
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            
            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }

            else if (bt.Name == "butt_Delete")
            {
                int Delete_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Delete_Base_Data(ref Delete_Error_Check);

                if (Delete_Error_Check > 0)
                    _From_Data_Clear();

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            

        }


        //회원번호 클릿햇을때. 관련 정보들 다 리셋 시킨다.
        //추후 번호만 변경하고 엔터 안누눌러서.. 데이타가 엉키는 것을 방지하기 위함.
        private void mtxtMbid_Click(object sender, EventArgs e)
        {
            MaskedTextBox mtb = (MaskedTextBox)sender;

            if (mtb.Name == "mtxtMbid")
            {
                _From_Data_Clear();
            }


            //마스크텍스트 박스에 입력한 내용이 있으면 그곳 다음으로 커서가 가게 한다.
            if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                mtb.SelectionStart = mtb.Text.Replace("-", "").Replace("_", "").Trim().Length + 1;

        }




        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
           // SendKeys.Send("{TAB}");
        }



        private void radioButt_Sn_MouseUp(object sender, MouseEventArgs e)
        {
            RadioButton trd = (RadioButton)sender;

            mtxtSn.Text = "";
            if (trd.Name == "raButt_IN_1" || trd.Name == "raButt_IN_2")
                mtxtSn.Mask = "999999-9999999";
            else
                mtxtSn.Mask = "999-99-99999";

            mtxtSn.Focus();
        }


        private bool  Check_TextBox_Error_Date()
        {
            ////cls_Check_Input_Error c_er = new cls_Check_Input_Error();
            ////if (txtLeaveDate.Text.Trim() != "")
            ////{
            ////    int Ret = 0;
            ////    Ret = c_er.Input_Date_Err_Check(txtLeaveDate);

            ////    if (Ret == -1)
            ////    {
            ////        txtLeaveDate.Focus(); return false;
            ////    }
            ////}

            ////if (txtPayDate.Text.Trim() != "")
            ////{
            ////    int Ret = 0;
            ////    Ret = c_er.Input_Date_Err_Check(txtPayDate);

            ////    if (Ret == -1)
            ////    {
            ////        txtPayDate.Focus(); return false;
            ////    }
            ////}

            ////if (txtLineDate.Text.Trim() != "")
            ////{
            ////    int Ret = 0;
            ////    Ret = c_er.Input_Date_Err_Check(txtLineDate);

            ////    if (Ret == -1)
            ////    {
            ////        txtLineDate.Focus(); return false;
            ////    }
            ////}

            cls_Check_Input_Error c_er = new cls_Check_Input_Error();
            if (mtxtLeaveDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtLeaveDate.Text, mtxtLeaveDate, "Date") == false)
                {
                    mtxtLeaveDate.Focus();
                    return false;
                }
            }

            if (mtxtLineDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtLineDate.Text, mtxtLineDate, "Date") == false)
                {
                    mtxtLineDate.Focus();
                    return false;
                }
            }

            return true;
        }


        
        private Boolean Check_TextBox_Error()
        {
            
            

            if (Input_Error_Check(mtxtMbid, "m") == false) return false; //회원번호 관련 관련 오류 체크
            
            //날짜 관련 텍스트 파일들에 대해서 날짜 오류를 체크한다
            if (Check_TextBox_Error_Date() == false) return false;


            idx_Org_Mbid = ""; idx_Org_Mbid2 = -1;
            if (tab_Base.SelectedIndex == 1)
            {
                if (txtLineCnt.Text.Trim() == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_LineCnt")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtLineCnt.Focus(); return false;
                }

                //라인이 변경을 한경우에.. 그 변경 위치가 비어 잇는지 그리고. 누가 사용하고 있는지를 체크한다.
                if (idx_LineCnt != int.Parse(txtLineCnt.Text.Trim()) && int.Parse(txtLineCnt.Text.Trim()) > 0)
                {
                    //라인이 기준을 오버 했는지.
                    if (int.Parse(txtLineCnt.Text.Trim()) > cls_app_static_var.Member_Down_Cnt)
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Line_Not")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        txtLineCnt.Focus(); return false;
                    }

                    //이미 그 라인을 누군가가 사용하고 있는지 있다면. 그라인 사용하는 사람이랑 체인지 할건지.
                    cls_Search_DB csd = new cls_Search_DB();
                    if (csd.Member_Down_Save_TF(txtMbid_s.Text.Trim(), mtxtMbid.Text.Trim(), int.Parse(txtLineCnt.Text.Trim()), ref idx_Org_Mbid, ref idx_Org_Mbid2) == false)
                    {
                        if (idx_LineDate == "") //라인중지자가 아닌경우에
                        {
                            if (idx_Org_Mbid2 != -1) //내가 선택한 라인을 누군가가 사용하고 있다.
                            {
                                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Line"), "", MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    txtLineCnt.Focus(); return false;
                                }
                            }
                        }
                        else //라인중지자 엿던 경우에는 선택한 위치에 누가 있으면 못들어 가게 막는다.
                        {
                            if (idx_Org_Mbid2 != -1) //내가 선택한 라인을 누군가가 사용하고 있다.
                            {
                                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Line_Not_2")
                                + "\n" +
                                cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Line_Not_3")
                                + "\n" +
                                cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                                txtLineCnt.Focus(); return false;
                            }
                        }

                    }
                }
            }



            if (tab_Base.SelectedIndex == 0)
            {
                //주민번호체크
                if (mtxtSn.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    string Sn = mtxtSn.Text.Replace("-", "").Replace("_", "").Trim();
                    if (Sn_Number_(Sn, mtxtSn) == false) return false;
                }
            }
            




            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;

            if (tab_Base.SelectedIndex == 2)
            {
                //라인중지를 시킬려고 한다.
                if (idx_LineDate == "" && mtxtLineDate.Text.Replace("-", "").Trim() != "")
                {
                    cls_Search_DB csb = new cls_Search_DB();
                    csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

                    if (Check_TextBox_Error(Mbid, Mbid2) == false) return false;

                    txtLineCnt.Text = "0"; // 중지되면 본인 위치값은 0이 된다.
                }


                //라인중지를 풀려고 한다. 그럼 새로운 라인을 선택해 달라고 한다. 위치값이 0인 경우엔는
                if (idx_LineDate != "" && mtxtLineDate.Text.Replace("-", "").Trim() == "")
                {
                    if (txtLineCnt.Text.Trim() == "0")
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Line_Re_Using")
                         + "\n" +
                         cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        txtLineCnt.Focus(); return false;
                    }
                }
            }


            return true;
        }

        private Boolean Check_TextBox_Error_01()
        {
            //// ** 명의변경 진행시 
            //if (tab_Base.SelectedIndex == 0)
            //{
            //    if (Check_Certify_Error() == false) return false;
            //}
            //else 
            if (tab_Base.SelectedIndex == 2)
            {
                if (radioB__1.Checked && txt_leave_R.Text.Trim().Length == 0)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please enter the reason for the cancellation.");
                    }
                    else
                    {
                        MessageBox.Show("직권해지사유를 기입해주시기바랍니다.");
                    }
                    txt_leave_R.Focus();
                    return false;
                }
            }

            return true;
        }

        private bool Check_TextBox_Error(string Mbid, int Mbid2)
        {
            
            string Tsql = "";
            Tsql = "Select LeaveDate ";
            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid.ToString() + "'";
            }
            //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            //탈퇴된 회원만 라인중지를 시킬수 잇다. 먼저 탈퇴를 하라고 한다.
            if (ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Line_Leave")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtLineDate.Focus(); return false;
            }
            //++++++++++++++++++++++++++++++++   


            //라인중지시 후원한 사람이 한명이라도 있으면 그사람들 부터 조절 하라고 한다.
            Tsql = "Select LeaveDate ";
            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Saveid = '" + Mbid + "' ";
                Tsql = Tsql + " And   tbl_Memberinfo.Saveid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Saveid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Saveid = '" + Mbid.ToString() + "'";
            }

            Tsql = Tsql + " And  LineCnt > 0 ";
            //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            //++++++++++++++++++++++++++++++++

            ds.Clear();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt >= 1)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Line_Down")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtLineDate.Focus(); return false;
            }
            //++++++++++++++++++++++++++++++++   



            //라인중지시 추천한 사람이 한명이라도 있으면 그사람들 부터 조절 하라고 한다.
            Tsql = "Select LeaveDate ";
            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
     
            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Nominid = '" + Mbid + "' ";
                Tsql = Tsql + " And   tbl_Memberinfo.Nominid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Nominid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Nominid = '" + Mbid.ToString() + "'";
            }


            Tsql = Tsql + " And  N_LineCnt > 0 ";
            //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            //++++++++++++++++++++++++++++++++

            ds.Clear();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt >= 1)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Line_Down")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtLineDate.Focus(); return false;
            }
            //++++++++++++++++++++++++++++++++   


            return true;
        }






        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
                        
            if (Check_TextBox_Error() == false) return;

            int For_Kind_TF = 0; string Sn = ""; int LeaveCheck = 1; int LineUserCheck = 1;
            
            if (raButt_IN_2.Checked == true) For_Kind_TF = 1;// 내국인은 0 외국인은 1  사업자는 2
            if (raButt_IN_3.Checked == true) For_Kind_TF = 2;

            if (mtxtLeaveDate.Text.Replace ("-","").Trim() != "")
                LeaveCheck = 0;

            if (txtPayDate.Text.Replace("-", "").Trim() != "")
                LineUserCheck = 0;

            if (mtxtSn.Text.Replace("-", "").Replace("_", "").Trim() != "")            
                Sn = mtxtSn.Text.Replace("-", "").Replace("_", "").Trim();                
            
                       
            cls_Search_DB csd = new cls_Search_DB();
            csd.Member_Mod_BackUp(mtxtMbid.Text.Trim(), "tbl_Memberinfo");



            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {

                //위치값을 변경 했는데 이전에 차지한 사람이 있엇다 그럼 이전사람에 대해서 체인지.
                if (idx_LineCnt != int.Parse(txtLineCnt.Text.Trim())  && idx_Org_Mbid2 != -1)
                {
                    change_Org_Member_Line(Temp_Connect,Conn, tran);
                }

                ////라인중지를 선택 했다. 본인의 라인중지와 라인 변경 관련 백업.
                //if (idx_LineDate == "" && txtLineDate.Text != "")
                //{
                //    //change_Org_Member_Line(Temp_Connect, Conn, tran, Mbid, Mbid2);
                //}

                //if (idx_LineDate != "" && txtLineDate.Text == "")
                //{ 

                //}

                string StrSql = "";                
                StrSql = "Update tbl_Memberinfo Set ";
                StrSql = StrSql + "  M_Name  = '" + txtName_C.Text.Trim() + "'";

                StrSql = StrSql + " ,For_Kind_TF = " + For_Kind_TF ;

                StrSql = StrSql + " ,LeaveCheck = " + LeaveCheck;
                StrSql = StrSql + " ,LineUserCheck = " + LineUserCheck;                

                //StrSql = StrSql + " ,Cpno = '" + encrypter.Encrypt(Sn.Trim()) + "'";
                StrSql = StrSql + " ,LeaveDate ='" + mtxtLeaveDate.Text.Replace("-", "").Trim() + "'";
                StrSql = StrSql + " ,PayStop_Date ='" + txtPayDate.Text.Replace("-", "").Trim() + "'";
                
                StrSql = StrSql + " ,LineUserDate ='" + mtxtLineDate.Text.Replace("-", "").Trim() + "'";

                StrSql = StrSql + " ,LeaveReason ='" + txt_leave_R.Text.Trim() + "'";
                StrSql = StrSql + " ,LineDelReason ='" + txt_Line_R.Text.Trim() + "'";

                //StrSql = StrSql + " ,WebPassWord = '" + encrypter.Encrypt(txtPassword.Text.Trim()) + "'";

                if(tab_Base.SelectedIndex == 0)
                {

                    StrSql = StrSql + " ,ipin_ci ='" + txt_IpinCI.Text + "'";
                    StrSql = StrSql + " ,ipin_ci ='" + txt_IpinDI.Text + "'";
                    string BirthDay = ""; string BirthDay_M = ""; string BirthDay_D = ""; int BirthDayTF = 0;
                    if (mtxtBrithDay.Text.Replace("-", "").Trim() != "")
                    {
                        string[] Sn_t = mtxtBrithDay.Text.Split('-');

                        BirthDay = Sn_t[0];  //생년월일을 년월일로 해서 쪼갠다
                        BirthDay_M = Sn_t[1]; //웹쪽 관련해서 이렇게 받아들이는데가 많아서
                        BirthDay_D = Sn_t[2]; //웹쪽 기준에 맞춘거임.
                    }
                    StrSql = StrSql + " ,BirthDay ='" + BirthDay + "'";
                    StrSql = StrSql + " ,BirthDay_M ='" + BirthDay_M + "'";
                    StrSql = StrSql + " ,BirthDay_D ='" + BirthDay_D + "'";

                    string Sex_FLAG = "";
                    if (radioB_Sex_Y.Checked == true) Sex_FLAG = "Y";
                    if (radioB_Sex_X.Checked == true) Sex_FLAG = "X";

                    StrSql = StrSql + " ,Sex_FLAG ='" + Sex_FLAG + "'";
                    
                }

                //라인중지가 되면 후원위치와 추천위치가 사라진다.
                if (mtxtLineDate.Text.Replace("-", "").Trim() == "")
                {
                    StrSql = StrSql + " ,LineCnt =" + int.Parse(txtLineCnt.Text.Trim());
                    if (idx_LineDate != "" && mtxtLineDate.Text.Replace("-", "") == "")
                    {
                        if (chk_N.Checked == true)
                        {
                            StrSql = StrSql + " ,N_LineCnt = 1 ";
                        }
                        else
                        {
                            string T_NomMbid = txtMbid_n.Text;
                            string Nominid = ""; int Nominid2 = 0;

                            csd.Member_Nmumber_Split(T_NomMbid, ref Nominid, ref Nominid2);
                            StrSql = StrSql + " ,N_LineCnt =" + csd.N_LineCnt_Search_Nom(Nominid, Nominid2);
                        }
                    }
                }
                else
                {
                    StrSql = StrSql + " ,LineCnt = 0 ";
                    StrSql = StrSql + " ,N_LineCnt = 0 ";
                }
                
                
       

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
                    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                {
                    StrSql = StrSql + " Where Mbid = '" + Mbid.ToString() + "'";
                }


                Temp_Connect.Update_Data (StrSql, Conn, tran, this.Name, this.Text);

                
                tran.Commit();
                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));

                csd.tbl_Memberinfo_Mod(mtxtMbid.Text.Trim(), txt_N_Remark.Text.Trim());
                
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

        private void change_Org_Member_Line(cls_Connect_DB Temp_Connect,SqlConnection Conn, SqlTransaction tran)
        {
           
            //회원 정보 변경 테이블에 넣어둔다.
            string StrSql = "";
            StrSql = "insert into tbl_Memberinfo_Mod ";
            StrSql = StrSql + " (";
            StrSql = StrSql + " mbid, mbid2 ";
            StrSql = StrSql + ", ChangeDetail ";
            StrSql = StrSql + ", BeforeDetail ";
            StrSql = StrSql + ", AfterDetail ";
            StrSql = StrSql + ", ModRecordid ";
            StrSql = StrSql + ", ModRecordtime ";
            StrSql = StrSql + " ) ";
            StrSql = StrSql + " Select "; 
            StrSql = StrSql + " mbid, mbid2 ";
            StrSql = StrSql + ",'LineCnt' ";
            StrSql = StrSql + ", LineCnt ";
            StrSql = StrSql + ", " + idx_LineCnt.ToString() ;            
            StrSql = StrSql + ",'" + cls_User.gid + "'";
            StrSql = StrSql + ", Convert(Varchar(25),GetDate(),21) ";
            StrSql = StrSql + " From tbl_Memberinfo ";



            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                StrSql = StrSql + " Where Mbid = '" + idx_Org_Mbid + "' ";
                StrSql = StrSql + " And   Mbid2 = " + idx_Org_Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                StrSql = StrSql + " Where Mbid2 = " + idx_Org_Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                StrSql = StrSql + " Where Mbid = '" + idx_Org_Mbid.ToString() + "'";
            }



            Temp_Connect.Insert_Data (StrSql,"tbl_Memberinfo", Conn, tran, this.Name, this.Text);
            
            //실질적인 회원 관련에서 업데이트
            StrSql = "Update tbl_Memberinfo Set ";
            StrSql = StrSql + "  LineCnt  = " + idx_LineCnt.ToString();



            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                StrSql = StrSql + " Where Mbid = '" + idx_Org_Mbid + "' ";
                StrSql = StrSql + " And   Mbid2 = " + idx_Org_Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                StrSql = StrSql + " Where Mbid2 = " + idx_Org_Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                StrSql = StrSql + " Where Mbid = '" + idx_Org_Mbid.ToString() + "'";
            }

            Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);

            //csd.tbl_Memberinfo_Mod(idx_Org_Mbid, idx_Org_Mbid2);
        }



        private void change_Org_Member_Line(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string Mbid, int Mbid2)
        {

            ////회원 정보 변경 테이블에 넣어둔다.
            //string StrSql = "";
            //StrSql = "insert into tbl_Memberinfo_Mod ";
            //StrSql = StrSql + " (";
            //StrSql = StrSql + " mbid, mbid2 ";
            //StrSql = StrSql + ", ChangeDetail ";
            //StrSql = StrSql + ", BeforeDetail ";
            //StrSql = StrSql + ", AfterDetail ";
            //StrSql = StrSql + ", ModRecordid ";
            //StrSql = StrSql + ", ModRecordtime ";
            //StrSql = StrSql + " ) ";
            //StrSql = StrSql + " Select ";
            //StrSql = StrSql + " mbid, mbid2 ";
            //StrSql = StrSql + ",'LienCnt' ";
            //StrSql = StrSql + ", LineCnt ";
            //StrSql = StrSql + ",  0 ";
            //StrSql = StrSql + ",'" + cls_User.gid + "'";
            //StrSql = StrSql + ", Convert(Varchar(25),GetDate(),21) ";
            //StrSql = StrSql + " From tbl_Memberinfo ";

            //if (Mbid.Length == 0)
            //    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
            //else
            //{
            //    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
            //    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
            //}

            //Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo", Conn, tran, this.Name, this.Text);




            //StrSql = "insert into tbl_Memberinfo_Mod ";
            //StrSql = StrSql + " (";
            //StrSql = StrSql + " mbid, mbid2 ";
            //StrSql = StrSql + ", ChangeDetail ";
            //StrSql = StrSql + ", BeforeDetail ";
            //StrSql = StrSql + ", AfterDetail ";
            //StrSql = StrSql + ", ModRecordid ";
            //StrSql = StrSql + ", ModRecordtime ";
            //StrSql = StrSql + " ) ";
            //StrSql = StrSql + " Select ";
            //StrSql = StrSql + " mbid, mbid2 ";
            //StrSql = StrSql + ",'N_LineCnt' ";
            //StrSql = StrSql + ", N_LineCnt ";
            //StrSql = StrSql + ",  0 ";
            //StrSql = StrSql + ",'" + cls_User.gid + "'";
            //StrSql = StrSql + ", Convert(Varchar(25),GetDate(),21) ";
            //StrSql = StrSql + " From tbl_Memberinfo ";

            //if (Mbid.Length == 0)
            //    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
            //else
            //{
            //    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
            //    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
            //}

            //Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo", Conn, tran, this.Name, this.Text);



            //StrSql = "insert into tbl_Memberinfo_Mod ";
            //StrSql = StrSql + " (";
            //StrSql = StrSql + " mbid, mbid2 ";
            //StrSql = StrSql + ", ChangeDetail ";
            //StrSql = StrSql + ", BeforeDetail ";
            //StrSql = StrSql + ", AfterDetail ";
            //StrSql = StrSql + ", ModRecordid ";
            //StrSql = StrSql + ", ModRecordtime ";
            //StrSql = StrSql + " ) ";
            //StrSql = StrSql + " Select ";
            //StrSql = StrSql + " mbid, mbid2 ";
            //StrSql = StrSql + ",'LineUserDate' ";
            //StrSql = StrSql + ", LineUserDate ";
            //StrSql = StrSql + ",  '" + txtLineDate.Text.Trim() + "'";
            //StrSql = StrSql + ",'" + cls_User.gid + "'";
            //StrSql = StrSql + ", Convert(Varchar(25),GetDate(),21) ";
            //StrSql = StrSql + " From tbl_Memberinfo ";

            //if (Mbid.Length == 0)
            //    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
            //else
            //{
            //    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
            //    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
            //}

            //Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo", Conn, tran, this.Name, this.Text);

            //csd.tbl_Memberinfo_Mod(Mbid, Mbid2);
        }






        private Boolean Input_Error_Check(MaskedTextBox m_tb, string Mbid, int Mbid2)
        {

            string Tsql = "";
            //후원한 사람이 있는가.
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            Tsql = "Select Mbid , Mbid2, M_Name ";
            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
  


            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where Saveid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Saveid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where Saveid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                Tsql = Tsql + " Where Saveid = '" + Mbid.ToString() + "'";
            }



            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)  //실제로 존재하는 회원 번호 인가.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Connect_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Save")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<  


            //추천한 사람이 있는가.
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            Tsql = "Select Mbid , Mbid2, M_Name ";
            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
    

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where Nominid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Nominid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where Nominid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                Tsql = Tsql + " Where Nominid = '" + Mbid.ToString() + "'";
            }




            ds.Clear();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)  //실제로 존재하는 회원 번호 인가.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Connect_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Nomin")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<



            //매출친 내역이 잇는가 있는가.
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            Tsql = "Select Mbid , Mbid2, M_Name ";
            Tsql = Tsql + " From tbl_SalesDetail (nolock) ";
      

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid.ToString() + "'";
            }

            Tsql = Tsql + " And  tbl_SalesDetail.SellCode <> '' ";

            ds.Clear();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)  //실제로 존재하는 회원 번호 인가.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Connect_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Sell")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


            //마감에 관여한 내역이 잇는가 있는가.
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            Tsql = "Select Mbid , Mbid2, M_Name ";
            Tsql = Tsql + " From tbl_ClosePay_04 (nolock) ";
            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid.ToString() + "'";
            }


            Tsql = Tsql + "  And TruePayment  > 0 "; 
            ds.Clear();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)  //실제로 존재하는 회원 번호 인가.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Connect_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Close")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<



            //마감에 관여한 내역이 잇는가 있는가.
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            Tsql = "Select Mbid , Mbid2, M_Name ";
            Tsql = Tsql + " From tbl_ClosePay_04 (nolock) ";
            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid.ToString() + "'";
            }


            Tsql = Tsql + "  And TruePayment  > 0 ";
            ds.Clear();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)  //실제로 존재하는 회원 번호 인가.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Connect_Data")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Close")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


            return true;
        }




        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Delete_Base_Data(ref int Delete_Error_Check)
        {
            Delete_Error_Check = 0;
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (Input_Error_Check(mtxtMbid, "m") == false) return;  //실제 존재 여부와 회원번호 오류등 체크


            cls_Search_DB csd = new cls_Search_DB();
            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

            if (Input_Error_Check(mtxtMbid, Mbid, Mbid2) == false) return;  //회원번호로 연관된 자료들이 있는지를 체크한다.

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                string StrSql = "";

                //StrSql = "Insert into  tbl_Tax_Mod ";
                //StrSql = StrSql + " Select *  ";
                //StrSql = StrSql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Tax ";
                //if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                //{
                //    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
                //    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
                //}

                //if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                //{
                //    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
                //}

                //if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                //{
                //    StrSql = StrSql + " Where Mbid = '" + Mbid.ToString() + "'";
                //}

                //Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                //StrSql = "Delete From tbl_Tax  ";
                //if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                //{
                //    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
                //    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
                //}

                //if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                //{
                //    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
                //}

                //if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                //{
                //    StrSql = StrSql + " Where Mbid = '" + Mbid.ToString() + "'";
                //}
                //Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);


                string Remarks = txt_D_Remarks.Text.Trim ();


                StrSql = "Insert into  tbl_Memberinfo_del_backup ";
                StrSql = StrSql + " Select *  ";
                StrSql = StrSql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Memberinfo ";
                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
                    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                {
                    StrSql = StrSql + " Where Mbid = '" + Mbid.ToString() + "'";
                }

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                StrSql = "Delete From tbl_Memberinfo  ";
                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
                    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                {
                    StrSql = StrSql + " Where Mbid = '" + Mbid.ToString() + "'";
                }
                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);


                tran.Commit();
                Delete_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del"));

                csd.tbl_Memberinfo_Mod(mtxtMbid.Text.Trim());

            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Err"));

            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }






        }

        private void tab_inf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab_inf.SelectedIndex == 3)
            {
                if (dGridView_Pay.RowCount == 0 && txtMbid_n.Text.Trim() != "")
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                    cgbp.dGridView_Put_baseinfo(this, dGridView_Pay, "pay", mtxtMbid.Text.Trim());
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
        }

        private void butt_Save_Leave_Click(object sender, EventArgs e)
        {
            int Save_Error_Check = 0;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Save_Base_Data(ref Save_Error_Check, "1", 1);

            if (Save_Error_Check > 0)
                _From_Data_Clear();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }



        private void Save_Base_Data(ref int Save_Error_Check, string TRT, int i)
        {
            Save_Error_Check = 0;
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (Check_TextBox_Error() == false) return;

            int For_Kind_TF = 0; string Sn = ""; int LeaveCheck = 1; int LineUserCheck = 1;

            if (radioB_1.Checked == true)
            {
                mtxtLeaveDate.Text = "";
                LeaveCheck = 1;  //탈퇴취소
            }
            if (mtxtLeaveDate.Text.Replace("-", "").Trim() != "")
            {
                if (radioB_0.Checked == true) LeaveCheck = 0;  //탈퇴
                if (radioB__1.Checked == true) LeaveCheck = -1;  // 정지
                if (radioB__100.Checked == true) LeaveCheck = -100;  //휴면
             
            }

            if (radioB_0.Checked)
            {
                if (mtxtLeaveDate.Text.Trim().Replace("-", "") == string.Empty)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please enter the withdrawal date.");
                    }
                    else
                    {
                        MessageBox.Show("탈퇴일자를 입력해주십시오");
                    }
                    mtxtLeaveDate.Focus();
                    return;
                }
                //else if (txt_leave_R.Text.Trim() == string.Empty)
                //{
                //    MessageBox.Show("탈퇴사유를 입력해주십시오");
                //    txt_leave_R.Focus();
                //    return;
                //}
            }

            if (radioB_1.Checked)
            {
                if (mtxtLeaveDate.Text.Trim().Replace("-", "") != string.Empty)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please leave the withdrawal date blank.");
                    }
                    else
                    {

                        MessageBox.Show("탈퇴일자를 비워주십시오.");
                    }
                    mtxtLeaveDate.Focus();
                    return;
                }
            }
           
            if (txtPayDate.Text.Replace("-", "").Trim() != "")
                LineUserCheck = 0;


            cls_Search_DB csd = new cls_Search_DB();
            csd.Member_Mod_BackUp(mtxtMbid.Text.Trim(), "tbl_Memberinfo");



            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {


                string StrSql = "";
                StrSql = "Update tbl_Memberinfo Set ";
                StrSql = StrSql + "  LeaveCheck = " + LeaveCheck;
                StrSql = StrSql + " ,LineUserCheck = " + LineUserCheck;

                StrSql = StrSql + " ,LeaveDate ='" + mtxtLeaveDate.Text.Replace("-", "").Trim() + "'";
                StrSql = StrSql + " ,PayStop_Date ='" + txtPayDate.Text.Replace("-", "").Trim() + "'";

                StrSql = StrSql + " ,LineUserDate ='" + mtxtLineDate.Text.Replace("-", "").Trim() + "'";

                StrSql = StrSql + " ,LeaveReason ='" + txt_leave_R.Text.Trim() + "'";
                StrSql = StrSql + " ,LeaveReason_code ='" + txtSellCode_Code.Text.Trim() + "'";
                StrSql = StrSql + " ,LineDelReason ='" + txt_Line_R.Text.Trim() + "'";

                //StrSql = StrSql + " ,WebPassWord = '" + encrypter.Encrypt(txtPassword.Text.Trim()) + "'";

                //라인중지가 되면 후원위치와 추천위치가 사라진다.
                if (mtxtLineDate.Text.Replace("-", "").Trim() == "")
                {
                    StrSql = StrSql + " ,LineCnt =" + int.Parse(txtLineCnt.Text.Trim());
                    if (idx_LineDate != "" && mtxtLineDate.Text.Replace("-", "") == "")
                    {
                        if (chk_N.Checked == true)
                        {
                            StrSql = StrSql + " ,N_LineCnt = 1 ";
                        }
                        else
                        {
                            string T_NomMbid = txtMbid_n.Text;
                            string Nominid = ""; int Nominid2 = 0;

                            csd.Member_Nmumber_Split(T_NomMbid, ref Nominid, ref Nominid2);
                            StrSql = StrSql + " ,N_LineCnt =" + csd.N_LineCnt_Search_Nom(Nominid, Nominid2);
                        }
                    }
                }
                else
                {
                    StrSql = StrSql + " ,LineCnt = 0 ";
                    StrSql = StrSql + " ,N_LineCnt = 0 ";
                }


                if (Mbid.Length == 0)
                    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
                    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
                }

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);
                tran.Commit();
                Save_Error_Check = 1;


                if(LeaveCheck == 0)
                {
                    //cls_Connect_DB Temp_Connect2 = new cls_Connect_DB();
                    //Temp_Connect2.Connect_DB();
                    //SqlConnection Conn2 = Temp_Connect2.Conn_Conn();
                    //SqlTransaction tran2 = Conn2.BeginTransaction();

                    //try
                    //{
                    //    string StrSql1 = "";
                    //    string StrSql2 = "";
                    //    string Auto_Seq = "";  // tbl_Memberinfo_AutoShip.Auto_Seq
                    //    string EndDate = "";  //today
                    //    string End_Reason = "회원탈퇴";
                    //    cls_Search_DB csd_2 = new cls_Search_DB();
                    //    EndDate = csd_2.Select_Today("yyyyMMdd");

                    //    for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                    //    {



                    //        StrSql1 = "select tbl_Memberinfo_AutoShip.Auto_Seq from tbl_Memberinfo_AutoShip where mbid2 = ";


                    //              Auto_Seq = dGridView_Base.Rows[i].Cells[1].Value.ToString();

                    //            StrSql2 = " EXEC Usp_End_Memberinfo_Autoship_CS '" + Auto_Seq + "', '" + EndDate + "', '" + cls_User.gid + "', '" + End_Reason + "' ";

                    //            Temp_Connect2.Insert_Data(StrSql2, base_db_name, Conn2, tran2);

                    //}


                    //    tran2.Commit();
                    //    Save_Error_Check = 1;
                    //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));

                    //}
                    //catch (Exception)
                    //{
                    //    tran2.Rollback();
                    //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
                    //}
                    //finally
                    //{
                    //    tran2.Dispose();
                    //    Temp_Connect.Close_DB();
                    //}
                }
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));

                csd.tbl_Memberinfo_Mod(mtxtMbid.Text.Trim(), txt_leave_R.Text.Trim());

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

        private void radioB_1_Click(object sender, EventArgs e)
        {
            mtxtLeaveDate.Text = "";
        }

        private void butt_Save_Name_Click(object sender, EventArgs e)
        {
            int Save_Error_Check = 0;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Save_Base_Data(ref Save_Error_Check, 1);

            if (Save_Error_Check > 0)
                _From_Data_Clear();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        private void Save_Base_Data(ref int Save_Error_Check, int CCCnt)
        {
            Save_Error_Check = 0;
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (Check_TextBox_Error() == false) return;

            int For_Kind_TF = 0; string Sn = ""; int LeaveCheck = 1; int LineUserCheck = 1;

            if (raButt_IN_2.Checked == true) For_Kind_TF = 1;// 내국인은 0 외국인은 1  사업자는 2
            if (raButt_IN_3.Checked == true) For_Kind_TF = 2;

            Sn = mtxtSn.Text.Replace("-", "").Replace("_", "").Trim();

            cls_Search_DB csd = new cls_Search_DB();
            csd.Member_Mod_BackUp(mtxtMbid.Text.Trim(), "tbl_Memberinfo");

            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {

                string StrSql = "";
                StrSql = "Update tbl_Memberinfo Set ";
                StrSql = StrSql + "  M_Name  = '" + txtName_C.Text.Trim() + "'";
                StrSql = StrSql + " ,bankowner  = '" + txtName_C.Text.Trim() + "'";
                StrSql = StrSql + " ,For_Kind_TF = " + For_Kind_TF;
                StrSql = StrSql + " ,Cpno = '" + encrypter.Encrypt(Sn.Trim()) + "'";

                if (Mbid.Length == 0)
                    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
                    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
                }

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);


                tran.Commit();
                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));

                csd.tbl_Memberinfo_Mod(mtxtMbid.Text.Trim(), txt_N_Remark.Text.Trim());

                cls_Connect_DB Temp_Connect2 = new cls_Connect_DB();
                Temp_Connect2.Connect_DB();

                if (cls_User.gid_CountryCode == "TH")   // 태국인 경우
                {
                    StrSql = " EXEC  Usp_JDE_Update_MK_Customer_TA '" + Mbid2 + "','U' ";
                }
                else    // 태국 이외 국가인 경우
                {
                    StrSql = " EXEC  Usp_JDE_Update_MK_Customer '" + Mbid2 + "','U' ";
                }

                DataSet ds1 = new DataSet();
                Temp_Connect2.Open_Data_Set(StrSql, "tbl_memberinfo", ds1);

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

        private void butt_Save_Line_Click(object sender, EventArgs e)
        {
            int Save_Error_Check = 0;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Save_Base_Data(ref Save_Error_Check, "1");

            if (Save_Error_Check > 0)
                _From_Data_Clear();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        private void Save_Base_Data(ref int Save_Error_Check, string TRT)
        {
            Save_Error_Check = 0;
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (Check_TextBox_Error() == false) return;

            int For_Kind_TF = 0; string Sn = ""; int LeaveCheck = 1; int LineUserCheck = 1;

            cls_Search_DB csd = new cls_Search_DB();

            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            
            csd.Member_Mod_BackUp(mtxtMbid.Text.Trim(), "tbl_Memberinfo");

            txtLineCnt.Text = rdoLineLeft.Checked ? "1" : "2";
            try
            {

                //위치값을 변경 했는데 이전에 차지한 사람이 있엇다 그럼 이전사람에 대해서 체인지.
                if (idx_LineCnt != int.Parse(txtLineCnt.Text.Trim()) && idx_Org_Mbid2 != -1)
                {
                    change_Org_Member_Line(Temp_Connect, Conn, tran);


                    string StrSql = "";
                    StrSql = "Update tbl_Memberinfo Set ";                  
                    StrSql = StrSql + " LineCnt =" + int.Parse(txtLineCnt.Text.Trim());                  
                    if (Mbid.Length == 0)
                        StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
                    else
                    {
                        StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
                        StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
                    }

                    Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);                    
                }
                else
                {
                    string StrSql = "";
                    StrSql = "Update tbl_Memberinfo Set ";
                    StrSql = StrSql + " LineCnt =" + int.Parse(txtLineCnt.Text.Trim());
                    if (Mbid.Length == 0)
                        StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
                    else
                    {
                        StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
                        StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
                    }

                    Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);
                }


                tran.Commit();
                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));


                csd.tbl_Memberinfo_Mod(mtxtMbid.Text.Trim(),"");


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

        private void butt_Certify_Click(object sender, EventArgs e)
        {
            frmBase_Certify e_f = new frmBase_Certify();
            e_f.Send_Certify_Info += new frmBase_Certify.SendCertifyDele(e_f_Send_Certify_Info);
            e_f.Call_Certify_Info += new frmBase_Certify.Call_Certify_Info_Dele(e_f_Call_Certify_Info);
            e_f.ShowDialog();
        }


        void e_f_Call_Certify_Info(ref string Callmode)
        {
            Callmode = "M";
        }

        private void e_f_Send_Certify_Info(string SuccessYN, string Message, string Name, string DI, string CI, string BirthDay, string Gender, string NationalInfo, string Age, string VNumber, string AgeCode, string AuthInfo, string AuthType)
        {
            /*
            SuccessYN : 성공여부
            Message : 메세지 
            Name : 실명
            DI : DI값
            CI : CI값
            BirthDay : 생년월일
            Gender : 성  (1:남성, 2:여성)
            NationalInfo : 내/외국인 구분 
            Age : 만 나이
            VNumber : IPIN 데이터 가상주민번호
            AgeCode : IPIN 데이터 연령대코드
            AuthInfo : IPIN 데이터 본인확인수단 (0:공인인증서, 1:카드, 2:핸드폰, 3:대면확인, 4:기타)
            authType : 인증수단	(M:휴대폰, X:공인인증서, I:아이핀)
            */
            if (SuccessYN == "Y")
            {
                if (NationalInfo == "1")
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("As a result of authentication, you have been confirmed as a foreigner.\nPlease register as a foreigner.");
                    }
                    else
                    {

                        MessageBox.Show("인증결과 외국인으로 확인되었습니다.\n외국인으로 등록하시기 바랍니다.");
                    }
                    return;
                }

                txt_IpinCI.Text = CI;
                txt_IpinDI.Text = DI;
                txtName_C.Text = Name;
                //txtName_Accnt.Text = Name;
                mtxtBrithDay.Text = BirthDay;
                if (Gender == "1")
                {
                    radioB_Sex_Y.Checked = true;
                    radioB_Sex_X.Checked = false;
                }
                else if (Gender == "2")
                {
                    radioB_Sex_Y.Checked = false;
                    radioB_Sex_X.Checked = true;
                }

                Lbl_Certify.Text = "인증확인";
                Lbl_Certify.ForeColor = Color.Blue;
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Authentication confirmed.");
                }
                else
                {

                    MessageBox.Show("인증 확인되었습니다.");
                }
            }
            else
            {
                Lbl_Certify.Text = "인증미확인";
                Lbl_Certify.ForeColor = Color.Red;
                MessageBox.Show(Message);
            }

        }



        private Boolean Check_Certify_Error()
        {
            if (raButt_IN_1.Checked == true)
            {
                if (txt_IpinDI.Text == "")
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please proceed after verifying your mobile phone");
                    }
                    else
                    {

                        MessageBox.Show("휴대폰 인증 후에 진행하시기 바랍니다.");
                    }
                    return false;
                }
            }

            return true;
        }













    }
}
