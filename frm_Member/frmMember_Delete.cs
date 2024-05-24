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
    public partial class frmMember_Delete : clsForm_Extends
    {
                
        cls_Grid_Base cg_Up_S = new cls_Grid_Base();
        
        private const string base_db_name = "tbl_Memberinfo";
        private int Data_Set_Form_TF;
        private string idx_Mbid = "";
        private int idx_Mbid2 = 0;        

        public frmMember_Delete()
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

            //groupB_M.Width = this.Width - groupBox1.Left - groupBox1.Width - 20;
            //groupB_M.Height = groupBox1.Height - groupBox2.Height - 20;
        }


        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();
        }

        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Exit.Left = this.Width - butt_Exit.Width - 20;

            butt_Clear.Left = 3;
            butt_Delete.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_Excel.Left = butt_Save.Left + butt_Save.Width + 2;
            butt_Save.Left = butt_Excel.Left + butt_Excel.Width + 2;
            ////this.Refresh();
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

           
        }

        

        private void txtData_Enter(object sender, EventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            if (sender is TextBox) T_R.Text_Focus_All_Sel((TextBox)sender);

            if (sender is MaskedTextBox) T_R.Text_Focus_All_Sel((MaskedTextBox)sender);

            if (this.Controls.ContainsKey("Popup_gr"))
            {
                DataGridView T_Gd = (DataGridView)this.Controls["Popup_gr"];
                T_Gd.Visible = false;
                T_Gd.Dispose();
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
            mtxtMbid.Text = Send_Number; txtName_s.Text = Send_Name;
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










        private void Set_Form_Date(string T_Mbid, string T_sort )
        {
            _From_Data_Clear();   
            
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
                Tsql = Tsql + " ,tbl_Memberinfo.Email ";

                Tsql = Tsql + ",  tbl_Memberinfo.Cpno";

                Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";
                
                Tsql = Tsql + " , tbl_Memberinfo.RegTime ";
                Tsql = Tsql + " , tbl_Memberinfo.hptel ";
                Tsql = Tsql + " , tbl_Memberinfo.Addcode1 "; 
                Tsql = Tsql + " , tbl_Memberinfo.address1 "; 
                Tsql = Tsql + " , tbl_Memberinfo.address2 ";

                Tsql = Tsql + " , tbl_Memberinfo.hometel ";
                Tsql = Tsql + " , tbl_Memberinfo.hptel ";
                Tsql = Tsql + " , tbl_Memberinfo.businesscode ";
                Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";

                Tsql = Tsql + " , tbl_Memberinfo.BankCode ";
                Tsql = Tsql + " ,Isnull(tbl_Bank.bankName,'') as Bank_Name";
                Tsql = Tsql + " , tbl_Memberinfo.bankowner ";
                Tsql = Tsql + " , tbl_Memberinfo.bankaccnt ";

                Tsql = Tsql + " , tbl_Memberinfo.Remarks ";

                Tsql = Tsql + " , tbl_Memberinfo.BirthDay ";
                Tsql = Tsql + " , tbl_Memberinfo.BirthDay_M ";
                Tsql = Tsql + " , tbl_Memberinfo.BirthDay_D ";
                Tsql = Tsql + " , tbl_Memberinfo.BirthDayTF ";
                
                Tsql = Tsql + " , tbl_Memberinfo.LeaveDate ";
                Tsql = Tsql + " , tbl_Memberinfo.LineUserDate ";
                Tsql = Tsql + " , tbl_Memberinfo.WebID ";
                Tsql = Tsql + " , tbl_Memberinfo.WebPassWord ";
                Tsql = Tsql + " , tbl_Memberinfo.Ed_Date ";
                Tsql = Tsql + " , tbl_Memberinfo.PayStop_Date ";

                Tsql = Tsql + " , tbl_Memberinfo.For_Kind_TF ";
                Tsql = Tsql + " , tbl_Memberinfo.Sell_Mem_TF ";
                
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " ,tbl_Memberinfo.Saveid + '-' + Convert(Varchar,tbl_Memberinfo.Saveid2) AS T_Saveid ";
                else
                    Tsql = Tsql + " ,tbl_Memberinfo.Saveid2 AS T_Saveid ";

                Tsql = Tsql + " , Isnull(Sav.M_Name,'') AS Save_Name ";
                Tsql = Tsql + " , tbl_Memberinfo.Saveid ";

                if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
                    Tsql = Tsql + ", Case When  Sav.Cpno <> '' Then LEFT(Sav.Cpno,6) +'-' + RIGHT(Sav.Cpno,7)  ELSE '' End AS Save_Cpno";
                else
                    Tsql = Tsql + ", Case When  Sav.Cpno <> '' Then LEFT(Sav.Cpno,6) +'-' + '*******'  ELSE '' End  AS Save_Cpno";
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<  

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " ,tbl_Memberinfo.Nominid + '-' + Convert(Varchar,tbl_Memberinfo.Nominid2) AS T_Nominid ";
                else
                    Tsql = Tsql + " ,tbl_Memberinfo.Nominid2 AS T_Nominid ";

                Tsql = Tsql + " , Isnull(Nom.M_Name,'') AS Nomin_Name ";
                Tsql = Tsql + " , tbl_Memberinfo.Nominid ";

                if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
                    Tsql = Tsql + ", Case When  Nom.Cpno <> '' Then LEFT(Nom.Cpno,6) +'-' + RIGHT(Nom.Cpno,7)  ELSE '' End AS Nom_Cpno";
                else
                    Tsql = Tsql + ", Case When  Nom.Cpno <> '' Then LEFT(Nom.Cpno,6) +'-' + '*******'  ELSE '' End  AS Nom_Cpno";
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<  
                

                Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Sav (nolock) ON tbl_Memberinfo.Saveid = Sav.Mbid And tbl_Memberinfo.Saveid2 = Sav.Mbid2 ";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Nom (nolock) ON tbl_Memberinfo.Nominid = Nom.Mbid And tbl_Memberinfo.Nominid2 = Nom.Mbid2 ";
                Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";
                Tsql = Tsql + " Left Join tbl_Bank (nolock) On tbl_Memberinfo.bankcode=tbl_Bank.ncode ";
                cls_NationService.SQL_BankNationCode(ref Tsql);

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
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
                Set_Form_Date(ds);

                Set_Form_Date_Up(2);    //직추천한 사람들을 뿌려줌
                Set_Form_Date_Up("S2");  //직후원한 사람들을 뿌려줌.

                Set_Form_Date_Info(); //회원 매출 관련 뿌려줌
                                            
                mtxtMbid.Focus();                
            }
            
            Data_Set_Form_TF = 0;            
        }

        private void Set_Form_Date(DataSet ds)
        {
            idx_Mbid =  ds.Tables[base_db_name].Rows[0]["Mbid"].ToString();
            idx_Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[0]["Mbid2"].ToString());
            
            mtxtMbid.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();
            txtName.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();
            mtxtSn.Text = ds.Tables[base_db_name].Rows[0]["Cpno"].ToString();
            txtName_E_1.Text = ds.Tables[base_db_name].Rows[0]["E_name"].ToString();
            txtName_E_2.Text = ds.Tables[base_db_name].Rows[0]["E_name_Last"].ToString();
            txtLineCnt.Text = ds.Tables[base_db_name].Rows[0]["LineCnt"].ToString();

            txtMbid_s.Text = ds.Tables[base_db_name].Rows[0]["T_Saveid"].ToString();
            txtName_s.Text = ds.Tables[base_db_name].Rows[0]["Save_Name"].ToString();
            txtSN_s.Text = ds.Tables[base_db_name].Rows[0]["Save_Cpno"].ToString();

            txtMbid_n.Text = ds.Tables[base_db_name].Rows[0]["T_Nominid"].ToString();
            txtName_n.Text = ds.Tables[base_db_name].Rows[0]["Nomin_Name"].ToString();
            txtSN_n.Text = ds.Tables[base_db_name].Rows[0]["Nom_Cpno"].ToString();
            
            txtLeaveDate.Text = ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString();
            txtLineDate.Text = ds.Tables[base_db_name].Rows[0]["LineUserDate"].ToString();
            txtS.Text = ds.Tables[base_db_name].Rows[0]["PayStop_Date"].ToString();

            txtRegDate.Text = ds.Tables[base_db_name].Rows[0]["Regtime"].ToString();
            txtEdDate.Text = ds.Tables[base_db_name].Rows[0]["Ed_Date"].ToString();

            if (ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString() != "")
            {
                txtAddCode1.Text = ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString().Substring(0, 3);
                txtAddCode2.Text = ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString().Substring(3, 3);                
            }
            txtAddress1.Text = ds.Tables[base_db_name].Rows[0]["Address1"].ToString();
            txtAddress2.Text = ds.Tables[base_db_name].Rows[0]["Address2"].ToString();

            

            if (ds.Tables[base_db_name].Rows[0]["hometel"].ToString() != "")
            {
                string[] tel = ds.Tables[base_db_name].Rows[0]["hometel"].ToString().Split('-');
                txtTel_1.Text = tel[0].ToString ();
                txtTel_2.Text = tel[1].ToString();
                txtTel_3.Text = tel[2].ToString();
            }

            if (ds.Tables[base_db_name].Rows[0]["hptel"].ToString() != "")
            {
                string[] tel = ds.Tables[base_db_name].Rows[0]["hptel"].ToString().Split('-');
                txtTel2_1.Text = tel[0].ToString();
                txtTel2_2.Text = tel[1].ToString();
                txtTel2_3.Text = tel[2].ToString();
            }


            txtCenter.Text = ds.Tables[base_db_name].Rows[0]["B_Name"].ToString();
            txtCenter_Code.Text = ds.Tables[base_db_name].Rows[0]["businesscode"].ToString();

            txtBank.Text = ds.Tables[base_db_name].Rows[0]["bank_Name"].ToString();
            txtBank_Code.Text = ds.Tables[base_db_name].Rows[0]["bankcode"].ToString();
            txtAccount.Text = ds.Tables[base_db_name].Rows[0]["bankaccnt"].ToString();
            txtName_Accnt.Text = ds.Tables[base_db_name].Rows[0]["bankowner"].ToString();

            txtWebID.Text = ds.Tables[base_db_name].Rows[0]["webid"].ToString();
            txtPassword.Text = ds.Tables[base_db_name].Rows[0]["webpassword"].ToString();

            txtEmail.Text = ds.Tables[base_db_name].Rows[0]["Email"].ToString();
            txtRemark.Text = ds.Tables[base_db_name].Rows[0]["Remarks"].ToString();

            string BirthDay = ds.Tables[base_db_name].Rows[0]["BirthDay"].ToString();
            if (BirthDay != "")
            {
                BirthDay = BirthDay + ds.Tables[base_db_name].Rows[0]["BirthDay_M"].ToString();
                BirthDay = BirthDay + ds.Tables[base_db_name].Rows[0]["BirthDay_D"].ToString();

                txtBrithDay.Text = BirthDay;
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



            if (ds.Tables[base_db_name].Rows[0]["Saveid"].ToString() != "" && ds.Tables[base_db_name].Rows[0]["Saveid"].ToString().Substring(0, 1) == "*")
                chk_S.Checked = true;

            if (ds.Tables[base_db_name].Rows[0]["Nominid"].ToString() != "" && ds.Tables[base_db_name].Rows[0]["Nominid"].ToString().Substring(0, 1) == "*")
                chk_N.Checked = true;


            txtName.ReadOnly = true;
            txtName.BackColor = Color.AliceBlue;
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

        private void Set_Form_Date_Info()
        {
            dGridView_Info_Header_Reset(dGridView_Sell,1);
            cg_Up_S.d_Grid_view_Header_Reset();

            Base_Grid_info_Set(1);
        }



       

        private void Base_Grid_Set(string Ufn_Name  )
        {            
            string T_Mbid   = "" ;            
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

            Tsql = Tsql + " From " + Ufn_Name ;
            Tsql = Tsql + " ('" + Mbid + "'," + Mbid2.ToString () + ") AS T_up";
            
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
                Tsql = Tsql + " From tbl_Memberinfo " ;
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



        private void dGridView_Up_S_Header_Reset(DataGridView t_Dgv)
        {
            cg_Up_S.Grid_Base_Arr_Clear();

            cg_Up_S.grid_col_Count = 5;
            cg_Up_S.basegrid = t_Dgv; //dGridView_Up_S;
            cg_Up_S.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_Up_S.grid_Frozen_End_Count = 2;
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













        private void Base_Grid_info_Set(int intTemp)
        {
            string T_Mbid = "";
            T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) != 1) return;

            string Tsql = "";

            Tsql = "Select  ";
            if (intTemp == 1)
            {                
                Tsql = Tsql + " SellDate ";
                Tsql = Tsql + " ,SellTypeName ";
                Tsql = Tsql + " ,Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " Ch_Detail ";
                //Tsql = Tsql + " ,Case When ReturnTF = 1 Then '정상' When ReturnTF = 2 Then '반품' When ReturnTF = 3 Then '교환' When ReturnTF = 4 Then '부분반품' END ";
                Tsql = Tsql + " ,TotalPrice ";
                Tsql = Tsql + " ,TotalInputPrice ";
                Tsql = Tsql + " ,TotalPV ";
                Tsql = Tsql + " ,OrderNumber ";

                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";

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

                Tsql = Tsql + " FROM tbl_Memberinfo_Mod AS A (nolock) " ;
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo_Mod_Detail Ch_T  (nolock) ON Ch_T.M_Detail = A.ChangeDetail";
                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS B  (nolock) ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 ";
                Tsql = Tsql + " LEFT JOIN tbl_Business         (nolock) ON B.BusinessCode = tbl_Business.ncode  And b.Na_code = tbl_Business.Na_code";

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where B.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where b.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   B.Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " And Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " IS NOT NULL ";
                Tsql = Tsql + " Order By Modrecordtime DESC ";
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
                Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            cg_Up_S.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cg_Up_S.db_grid_Obj_Data_Put();
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
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }




        private void dGridView_Info_Header_Reset(DataGridView t_Dgv, int intTemp)
        {
            cg_Up_S.Grid_Base_Arr_Clear();
            cg_Up_S.basegrid = t_Dgv; 
            cg_Up_S.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;            
            cg_Up_S.grid_col_Count = 10;
            cg_Up_S.grid_Frozen_End_Count = 1;
            cg_Up_S.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            if (intTemp == 1)
            {
                string[] g_HeaderText = {"매출_일자"  , "주문_종류"   , "상태"  , "매출액"   , "입급액"        
                                        ,"매출PV" , ""  , "" , "" , ""
                                        };

                Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
                //gr_dic_cell_format[1 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                cg_Up_S.grid_cell_format = gr_dic_cell_format;

                int[] g_Width = { 100, 90, 70, 80, 80
                                 ,80 , 0 , 0 , 0 , 0
                                };

                DataGridViewContentAlignment[] g_Alignment =
                                  {DataGridViewContentAlignment.MiddleCenter  
                                   ,DataGridViewContentAlignment.MiddleCenter 
                                   ,DataGridViewContentAlignment.MiddleCenter  
                                   ,DataGridViewContentAlignment.MiddleRight
                                   ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                   ,DataGridViewContentAlignment.MiddleRight 
                                   ,DataGridViewContentAlignment.MiddleCenter  
                                   ,DataGridViewContentAlignment.MiddleCenter
                                   ,DataGridViewContentAlignment.MiddleCenter  //10
                                  };

                cg_Up_S.grid_col_header_text = g_HeaderText;
                cg_Up_S.grid_col_w = g_Width;
                cg_Up_S.grid_col_alignment = g_Alignment;
            }
            else if (intTemp == 2)
            {
                string[] g_HeaderText = {"변경일"  , "변경내역"   , "전_내역"  , "후_내역"   , "변경자"        
                                    , ""   , ""    , ""  , "" , ""
                                    };

                int[] g_Width = { 120, 100, 100, 100, 80
                                 ,0 , 0 , 0 , 0 , 0
                                };

                DataGridViewContentAlignment[] g_Alignment =
                                  {DataGridViewContentAlignment.MiddleLeft
                                   ,DataGridViewContentAlignment.MiddleLeft 
                                   ,DataGridViewContentAlignment.MiddleCenter  
                                   ,DataGridViewContentAlignment.MiddleCenter
                                   ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                   ,DataGridViewContentAlignment.MiddleCenter 
                                   ,DataGridViewContentAlignment.MiddleCenter  
                                   ,DataGridViewContentAlignment.MiddleCenter
                                   ,DataGridViewContentAlignment.MiddleCenter  //10
                                  };

                cg_Up_S.grid_col_header_text = g_HeaderText;
                cg_Up_S.grid_col_w = g_Width;
                cg_Up_S.grid_col_alignment = g_Alignment;
            }

            else if (intTemp == 3)
            {
                string[] g_HeaderText = {"변경일"  , "전_상위번호"   , "전_상위성명"  , "후_상위번호"   , "후_상위성명"        
                                    , "구분"   , "변경자"    , ""  , "" , ""
                                    };

                int[] g_Width = { 120, 100, 100, 100, 100
                                 ,80 , 80 , 0 , 0 , 0
                                };

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
                                  };

                cg_Up_S.grid_col_header_text = g_HeaderText;
                cg_Up_S.grid_col_w = g_Width;
                cg_Up_S.grid_col_alignment = g_Alignment;
            }
            




            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                            
                                   };            
            cg_Up_S.grid_col_Lock = g_ReadOnly;
            
            cg_Up_S.basegrid.RowHeadersVisible = false;
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

        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            int Sw_Tab = 0;

            if ((sender is TextBox) == false) return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
                Sw_Tab = 1;
            }

            if (tb.Name == "txtCenter")
            {
                if (tb.Text.Trim() == "")
                    txtCenter_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtCenter_Code);
            }

            if (tb.Name == "txtBank")
            {
                if (tb.Text.Trim() == "")
                    txtBank_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtBank_Code);
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


        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
            if (tb.Name == "txtCenter")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, txtCenter_Code, "");
                else
                    Ncod_Text_Set_Data(tb, txtCenter_Code);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtBank")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, txtBank_Code, "");
                else
                    Ncod_Text_Set_Data(tb, txtBank_Code);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }
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
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtBank")
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);
            }
            else
            {
                if (tb.Name == "txtCenter")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
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
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);
                }

            }
        }



        private void Ncod_Text_Set_Data(TextBox tb, TextBox tb1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            if (tb.Name == "txtCenter")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
            }

            if (tb.Name == "txtR_Id")
            {
                Tsql = "Select user_id ,U_Name   ";
                Tsql = Tsql + " From tbl_User (nolock) ";
                Tsql = Tsql + " Where U_Name like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    user_id like '%" + tb.Text.Trim() + "%'";
            }

            if (tb.Name == "txtBank")
            {
                Tsql = "Select Ncode , BankName   ";
                Tsql = Tsql + " From tbl_Bank (nolock) ";
                Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%'";
            }

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 1)
            {
                tb.Text = ds.Tables["t_P_table"].Rows[0][1].ToString();
                tb1_Code.Text = ds.Tables["t_P_table"].Rows[0][0].ToString();
            }

            if ((ReCnt > 1) || (ReCnt == 0)) Db_Grid_Popup(tb, tb1_Code, Tsql);
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
                m_tb.Focus(); return false;
            }
            //++++++++++++++++++++++++++++++++  

            return true;
        }



        private Boolean Input_Error_Check(MaskedTextBox m_tb, string Mbid, int Mbid2)
        {
                       
            string Tsql = "";
            //후원한 사람이 있는가.
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            Tsql = "Select Mbid , Mbid2, M_Name ";
            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Saveid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Saveid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Saveid2 = " + Mbid2.ToString();
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
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Nominid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Nominid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Nominid2 = " + Mbid2.ToString();
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
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }

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
            //Tsql = "Select Mbid , Mbid2, M_Name ";
            //Tsql = Tsql + " From tbl_ClosePay_01 (nolock) ";
            //if (Mbid.Length == 0)
            //    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            //else
            //{
            //    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
            //    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            //}

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




















        private void _From_Data_Clear()
        {
            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb.d_Grid_view_Header_Reset();
            //Base_Grid_Set(); //당일등록 회원을 불러온다.
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Up_S_Header_Reset(dGridView_Down_N); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset(1);

            dGridView_Up_S_Header_Reset(dGridView_Down_S); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset(1);

            dGridView_Info_Header_Reset(dGridView_Sell, 1);
            cg_Up_S.d_Grid_view_Header_Reset(1);      
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            txtName.ReadOnly =false ;
            txtName.BackColor = SystemColors.Window;

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtMbid);

            opt_sell_2.Checked = true; opt_Bir_TF_1.Checked = true;
            raButt_IN_1.Checked = true;
            chk_N.Checked = false; chk_S.Checked = false;
            mtxtSn.Mask = "999999-9999999";
            idx_Mbid = ""; idx_Mbid2 = 0;
            mtxtMbid.Focus();
        }


        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {                
                _From_Data_Clear();                                
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
            
            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }



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

                StrSql = "Insert into  tbl_Memberinfo_del_backup ";
                StrSql = StrSql + " Select *  ";
                StrSql = StrSql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Memberinfo ";
                if (Mbid.Length == 0)
                    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
                    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
                }

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                StrSql = "Delete From tbl_Memberinfo  ";
                if (Mbid.Length == 0)
                    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
                    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
                }
                Temp_Connect.Update_Data (StrSql, Conn, tran, this.Name, this.Text);

                
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

























    }
}
