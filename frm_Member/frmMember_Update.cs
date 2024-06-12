using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Printing;
using System.Reflection;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

//using System.Net.Http;
//using System.Threading.Tasks;

namespace MLM_Program
{
    public partial class frmMember_Update : clsForm_Extends
    {
        cls_Grid_Base cgb = new cls_Grid_Base();

        cls_Grid_Base cg_Up_S = new cls_Grid_Base();
        cls_Grid_Base cgb_Item = new cls_Grid_Base();
        cls_Grid_Base cgb_Cacu = new cls_Grid_Base();
        cls_Grid_Base cgb_Rece = new cls_Grid_Base();
        cls_Grid_Base cgb_Down= new cls_Grid_Base();
        private Dictionary<int, cls_Sell_Item> SalesItemDetail = new Dictionary<int, cls_Sell_Item>();
        private Dictionary<int, cls_Sell_Rece> Sales_Rece = new Dictionary<int, cls_Sell_Rece>();
        private Dictionary<int, cls_Sell_Cacu> Sales_Cacu = new Dictionary<int, cls_Sell_Cacu>();


        public delegate void Take_NumberDele(ref string Send_Number, ref string Send_Name);
        public event Take_NumberDele Take_Mem_Number;

        private const string base_db_name = "tbl_Memberinfo";
        private int Data_Set_Form_TF;
        private string idx_Mbid = "", idx_Password = "";
        private int idx_Mbid2 = 0;


        Series series_Item = new Series();


        public frmMember_Update()
        {
            InitializeComponent();


            DoubleBuffered = true;

            typeof(Form).InvokeMember("DoubleBuffered", BindingFlags.NonPublic
          | BindingFlags.Instance | BindingFlags.SetProperty, null, this, new object[] { true });


            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.SetProperty, null, dGridView_inf, new object[] { true });

            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.SetProperty, null, dGridView_Pay, new object[] { true });

            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.SetProperty, null, dGridView_Down_S2, new object[] { true });

            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.SetProperty, null, dGridView_Down_N2, new object[] { true });

            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.SetProperty, null, dGridView_SaveDefault, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.SetProperty, null, panel13, new object[] { true });

            typeof(TabControl ).InvokeMember("DoubleBuffered", BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.SetProperty, null, tabC_Mem, new object[] { true });            
        }
                

        private void frmBase_From_Load(object sender, EventArgs e)
        {
            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_NaCode_ComboBox(combo_Se_2, combo_Se_Code_2);


            Data_Set_Form_TF = 0;

            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb.d_Grid_view_Header_Reset();
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //dGridView_Line_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cg_Li.d_Grid_view_Header_Reset();
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            tabC_Mem.TabPages.Remove(tabP_Ded_New);
            tabC_Mem.TabPages.Remove(tabP_Ded_New_Month);
            tabC_Mem.TabPages.Remove(tab_SaveDefault);
            



            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
            
            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            if (cls_User.gid_CountryCode != "TH")
            {
                mtxtSn.Mask = "999999-9999999"; //기본 셋팅은 주민번호이다. 
                mtxtSn_C.Mask = "999999-9999999"; //기본 셋팅은 주민번호이다. 
            }

            mtxtTel1.Mask = cls_app_static_var.Tel_Number_Fromat;
            mtxtTel2.Mask = cls_app_static_var.Tel_Number_Fromat;
            mtxtZip1.Mask = cls_app_static_var.ZipCode_Number_Fromat;
            mtxtZip2.Mask = cls_app_static_var.ZipCode_Number_Fromat;

            mtxtBrithDay.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtRegDate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtEdDate.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtRBODate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtVisaDay.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtBrithDayC.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtTel2_C.Mask = cls_app_static_var.Tel_Number_Fromat;

            txtB1.Text = "0"; 
            //Reset_Chart_Total();

            if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
            {       
                tabC_Up.TabPages.Remove(tabP_S);
                tabC_Up.TabPages.Remove(tabP_S_D);
                tabC_Mem.TabPages.Remove(tab_Down_Save);
                tbl_save.Visible = false;                             
            }

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                tabC_Up.TabPages.Remove(tabP_N);
                tabC_Up.TabPages.Remove(tabP_N_D);
                tabC_Mem.TabPages.Remove(tab_Down_Nom);
                
                tbl_nom.Visible = false;                
            }

            txtMbid_n.BackColor = cls_app_static_var.txt_Enable_Color;
            txtMbid_n2.BackColor = cls_app_static_var.txt_Enable_Color;
            txtName_n.BackColor = cls_app_static_var.txt_Enable_Color;
            txtName_n2.BackColor = cls_app_static_var.txt_Enable_Color;
            txtSN_n.BackColor = cls_app_static_var.txt_Enable_Color;
            txtSN_n2.BackColor = cls_app_static_var.txt_Enable_Color;
            txtMbid_s.BackColor = cls_app_static_var.txt_Enable_Color;
            txtMbid_s2.BackColor = cls_app_static_var.txt_Enable_Color;
            txtName_s.BackColor = cls_app_static_var.txt_Enable_Color;
            txtName_s2.BackColor = cls_app_static_var.txt_Enable_Color;
            txtSN_s.BackColor = cls_app_static_var.txt_Enable_Color;
            txtSN_s2.BackColor = cls_app_static_var.txt_Enable_Color;
            txtLineCnt.BackColor = cls_app_static_var.txt_Enable_Color; 
            mtxtSn.BackColor = cls_app_static_var.txt_Enable_Color;
            txtLeaveDate.BackColor = cls_app_static_var.txt_Enable_Color;
            txtLineDate.BackColor = cls_app_static_var.txt_Enable_Color;
            txtGrade.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_Us.BackColor = cls_app_static_var.txt_Enable_Color;
            txtGradeP.BackColor = cls_app_static_var.txt_Enable_Color;

         
            tabC_Mem.SelectedIndex = 0;

            cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
            cgbp11.dGridView_Put_baseinfo(dGridView_Talk, "talk");

         
            //tabC_Mem.TabPages.Remove(tab_Auto);
            //tabC_Mem.TabPages.Remove(tab_Auto_Select);
            //tabC_Mem.TabPages.Remove(tab_CC);
            tabC_Mem.TabPages.Remove(tab_Hide);

            if (cls_User.gid_CC_Save_TF == 0)  //공동신청인 권한이 없는 사람은 보이지 않게 한다.
                panel_CC.Enabled = false; 
            else
                panel_CC.Enabled = true ;

            radioB_RBO.Checked = true;
            radioB_G8.Checked = true;

            InitComboZipCode_TH();
            // 태국버전 인 경우
            if (cls_User.gid_CountryCode == "TH")
            {
                pnlDistrict_TH.Visible = true;
                pnlProvince_TH.Visible = true;
                pnlSubDistrict_TH.Visible = true;
                pnlZipCode_TH.Visible = true;
                pnlZipCode_KR.Visible = false;
                cbSubDistrict_TH_SelectedIndexChanged(this, null);
                //combo_Se_Code.Text = "TH";
                tlpMSADate.Visible = false;
                tlpNACODE.Visible = false;
                //combo_Se_Code_2.Text = "TH";

                //배송지가리기
                tableLayoutPanel15.Visible = false;
                tableLayoutPanel16.Visible = false;
                tableLayoutPanel17.Visible = false;
                cbProvince_TH.Font = new Font("Tahoma", 11f);
                cbDistrict_TH.Font = new Font("Tahoma", 11f);
                cbSubDistrict_TH.Font = new Font("Tahoma", 11f);
                txtZipCode_TH.Font = new Font("Tahoma", 11f);
                txtAddress1.Font = new Font("Tahoma", 11f);
                txtAddress1.Font = new Font("Tahoma", 11f);
                txtAddress2.Font = new Font("Tahoma", 11f);
                txtName.Font = new Font("Tahoma", 11f);

            }
            // 태국 이외 버전 인 경우
            else
            {
                pnlDistrict_TH.Visible = false;
                pnlProvince_TH.Visible = false;
                pnlSubDistrict_TH.Visible = false;
                pnlZipCode_TH.Visible = false;
                pnlZipCode_KR.Visible = true;
                txtAddress2.ReadOnly = false;
                txtAddress2.Clear();
                tabC_Mem.TabPages.Remove(tab_Img);
            }

            combo_Se_Code.Text = cls_User.gid_CountryCode;
            combo_Se_Code_2.Text = cls_User.gid_CountryCode;

            if (cls_User.gid_CountryCode == "TH")
            {
                //태국은 입력받을수 있음 
                mtxtSn.Mask = string.Empty;
                mtxtSn.ReadOnly = false;
                mtxtSn.Enabled = true;
                mtxtSn.Visible = true;
                mtxtSn.BackColor = Color.White ;
            }
            mtxtMbid.Focus();
        }

        private void InitComboZipCode_TH()
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("SELECT ZIPCODE_NM FROM dbo.ufn_Get_ZipCode_State_TH() ORDER BY ZIPCODE_SORT ");
            sb.AppendLine("SELECT * FROM ufn_Get_ZipCode_Province_TH() ORDER BY MinSubDistrictID ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            cbProvince_TH.DataBindings.Clear();
            cbProvince_TH.DataSource = ds.Tables["ZipCode_NM"];
            cbProvince_TH.DisplayMember = "ZipCode_NM";
            cbProvince_TH.ValueMember = "ProvinceCode";
            cbProvince_TH.Font = new Font("Tahoma", 11f);
        }

        private void Delete_Base_Data(ref int Delete_Error_Check)
        {
            Delete_Error_Check = 0;
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (Input_Error_Check(mtxtMbid, "m") == false) return;  //실제 존재 여부와 회원번호 오류등 체크


            cls_Search_DB csd = new cls_Search_DB();
            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

            //if (Input_Error_Check(mtxtMbid, Mbid, Mbid2) == false) return;  //회원번호로 연관된 자료들이 있는지를 체크한다.

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                string StrSql = "";
                string Remarks = "회원정보 삭제";


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

        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Save.Left = butt_Clear.Left + butt_Clear.Width + 2;
            //button_exigo.Left = butt_Save.Left + butt_Save.Width + 2;
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
            cfm.button_flat_change(butt_AddCode);
            cfm.button_flat_change(butt_AddCode2);
            cfm.button_flat_change(butt_AddCodeT1);
            cfm.button_flat_change(butt_Talk);

            cfm.button_flat_change(button_exigo);
            cfm.button_flat_change(button_Acc_Reg);
            

            
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

                            // cls_form_Meth cfm = new cls_form_Meth();
                            // cfm.form_Group_Panel_Enable_True(this);
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


        private void frmMember_Activated(object sender, EventArgs e)
        {
            //this.Refresh ();
            string Send_Number = ""; string Send_Name = "";
            Take_Mem_Number(ref Send_Number, ref Send_Name);

            if (Send_Number != "")
            {
                mtxtMbid.Text = Send_Number;
                Set_Form_Date(mtxtMbid.Text, "m");
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
                            {
                                Set_Form_Date(mtb.Text, "m");
                                Tab_Img_Activate();
                            }
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


        //회원번호 클릿햇을때. 관련 정보들 다 리셋 시킨다.
        //추후 번호만 변경하고 엔터 안누눌러서.. 데이타가 엉키는 것을 방지하기 위함.
        private void mtxtMbid_Click(object sender, EventArgs e)
        {
            MaskedTextBox mtb = (MaskedTextBox)sender;
                        
            if (mtb.Name == "mtxtMbid")
            {
                _From_Data_Clear();

                combo_Se_Code.Text = cls_User.gid_CountryCode;
                combo_Se_Code_2.Text = cls_User.gid_CountryCode;
            }

            

            //마스크텍스트 박스에 입력한 내용이 있으면 그곳 다음으로 커서가 가게 한다.
            if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                mtb.SelectionStart = mtb.Text.Replace("-", "").Replace("_", "").Trim().Length + 1;

        }




        private void MtxtData_Sn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    string Sn = mtb.Text.Replace("-", "").Replace("_", "").Trim();
                    if (Sn_Number_(Sn, mtb) == true)
                    {                        
                            txtName_E_1_C.Focus();
                    }
                }
                else
                {                    
                        txtName_E_1_C.Focus();
                }

            }
        }



        private bool Sn_Number_(string Sn, MaskedTextBox mtb, int Check_Multi_TF = 0 )
        {
            if (Sn != "")
            {
                string sort_TF = "";
                bool check_b = false;
                cls_Sn_Check csn_C = new cls_Sn_Check();

                if (mtb.Name == "mtxtSn")
                {
                    if (raButt_IN_1.Checked == true) //내국인인 구분자
                        sort_TF = "in";

                    if (raButt_IN_2.Checked == true) //외국인 구분자
                        sort_TF = "fo";

                    if (raButt_IN_3.Checked == true) //사업자 구분자.
                        sort_TF = "biz";
                }
                else
                {
                    if (raButt_IN_1_C.Checked == true) //내국인인 구분자
                        sort_TF = "in";

                    if (raButt_IN_2_C.Checked == true) //외국인 구분자
                        sort_TF = "fo";

                }
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

                    if (cls_app_static_var.Member_Reg_Multi_TF == 0 && Check_Multi_TF == 0 ) //다구좌 불가능으로 해서 체크되어 잇는 경우
                    {//동일 주민번호로 해서 가입한 사람이 있는지를 체크한다.
                        cls_Search_DB csb = new cls_Search_DB();
                        if (csb.Member_Multi_Sn_Search(Sn) == false) //주민번호 오류는 위에서 체크를 함.
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Same")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            mtb.Focus(); return false;
                        }
                    }
                }


                if (mtb.Name == "mtxtSn")
                {
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

                if (mtb.Name == "mtxtSn_C")
                {
                    if (raButt_IN_1_C.Checked == true && check_b == true) //내국인인 경우에는 주민번호 체크한다.
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






        private void  Set_Form_Date(string T_Mbid, string T_sort )
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
                    Tsql = Tsql + " ,Sav.sponsoralkynumber AS T_Saveid ";
                Tsql = Tsql + " , Isnull(Sav2.alphaname,'') AS Save_Name ";
                Tsql = Tsql + " , tbl_Memberinfo.Saveid ";
                //Tsql = Tsql + ",  Sav.Cpno  AS Save_Cpno ";
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<  

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " ,tbl_Memberinfo.Nominid + '-' + Convert(Varchar,tbl_Memberinfo.Nominid2) AS T_Nominid ";
                else
                    Tsql = Tsql + ",Nom.enrolleralkynumber AS T_Nominid ";


                Tsql = Tsql + " , Isnull(Nom2.alphaname,'') AS Nomin_Name ";
                Tsql = Tsql + " , tbl_Memberinfo.Nominid ";

                //Tsql = Tsql + ",  Nom.Cpno AS Nom_Cpno ";

                //if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
                //    Tsql = Tsql + ", Case When  Nom.Cpno <> '' Then LEFT(Nom.Cpno,6) +'-' + RIGHT(Nom.Cpno,7)  ELSE '' End AS Nom_Cpno";
                //else
                //    Tsql = Tsql + ", Case When  Nom.Cpno <> '' Then LEFT(Nom.Cpno,6) +'-' + '*******'  ELSE '' End  AS Nom_Cpno";
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<  

                Tsql = Tsql + ", Isnull(ETC_Addcode1,'') ETC_Addcode1 ";
                Tsql = Tsql + ", Isnull(ETC_Address1,'') ETC_Address1  ";
                Tsql = Tsql + ", Isnull(ETC_Address2,'') ETC_Address2  ";

                Tsql = Tsql + ", Isnull(tbl_Nation.nationNameKo,'') nationNameKo , tbl_Memberinfo.Na_code ";

                Tsql = Tsql + ", ISNULL(CP.Grade_Name,'')   G_Name ";
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
                Tsql = Tsql + ", isnull(tbl_Memberinfo.Third_Person_Agree,0)AS  Third_Person_Agree ";
                Tsql = Tsql + ", isnull(tbl_Memberinfo.AgreeMarketing, 'N')AS AgreeMarketing ";
                Tsql = Tsql + ", isnull(tbl_Memberinfo.Nation_Code, '')AS Nation_Code ";
                Tsql = Tsql + ", isnull(a.nationNameKo, '')AS nationNameKo";

                Tsql = Tsql + ", isnull(tbl_Memberinfo.Account_Wait_FLAG, '')AS Account_Wait_FLAG ";
                Tsql = Tsql + ", isnull(tbl_Memberinfo.LeaveCheck_FLAG, '')AS LeaveCheck_FLAG ";
                Tsql = Tsql + ", isnull(tbl_Memberinfo.For_Kind_TF_FLAG, '')AS For_Kind_TF_FLAG ";
                Tsql = Tsql + ",ED_Date ";

                Tsql = Tsql + " , tbl_Memberinfo.city";
                Tsql = Tsql + " , tbl_Memberinfo.state";

                Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
                Tsql = Tsql + " LEFT JOIN [mannaSync].[dbo].customer Sav (nolock) ON  Sav.[accountnumber] =  Convert(varchar,tbl_Memberinfo.mbid2)  ";
                Tsql = Tsql + " LEFT JOIN [mannaSync].[dbo].customer Nom (nolock) ON  Nom.[accountnumber] =  Convert(varchar,tbl_Memberinfo.mbid2 )  ";
                Tsql = Tsql + " LEFT JOIN [mannaSync].[dbo].customer Sav2 (nolock) ON  Sav.[sponsoralkynumber] = Sav2.[accountnumber]  ";
                Tsql = Tsql + " LEFT JOIN [mannaSync].[dbo].customer Nom2 (nolock) ON  Nom.[enrolleralkynumber] =  Nom2.[accountnumber]  ";

                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo_Address MAdd (nolock) ON MAdd.Mbid = tbl_Memberinfo.Mbid And MAdd.Mbid2 = tbl_Memberinfo.Mbid2 And Sort_Add = 'R' ";

                Tsql = Tsql + " LEFT JOIN tbl_Memberinfo_A MAuto (nolock) ON MAuto.Mbid = tbl_Memberinfo.Mbid And MAuto.Mbid2 = tbl_Memberinfo.Mbid2 ";
                Tsql = Tsql + " LEFT JOIN tbl_Card (nolock) ON tbl_Card.Ncode = MAuto.A_CardCode "; 

                Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";
                //Tsql = Tsql + " Left Join tbl_Bank (nolock) On tbl_Memberinfo.bankcode=tbl_Bank.ncode And tbl_Memberinfo.Na_code = tbl_Bank.Na_code ";
                Tsql = Tsql + " Left Join tbl_Bank (nolock) On tbl_Memberinfo.bankcode=tbl_Bank.ncode ";
                cls_NationService.SQL_BankNationCode(ref Tsql);
                Tsql = Tsql + " LEFT JOIN  tbl_Nation  (nolock) ON tbl_Nation.nationCode = tbl_Memberinfo.Na_Code  ";
                Tsql = Tsql + " LEFT JOIN  tbl_Nation a (nolock) ON a.nationCode = tbl_Memberinfo.Nation_Code  ";
                Tsql = Tsql + " Left Join tbl_Class CP On tbl_Memberinfo.CurGrade  = CP.Grade_Cnt ";
               // Tsql = Tsql + " Left Join ufn_Mem_CurGrade_Mbid_Search ('" + Mbid + "'," + Mbid2.ToString() + ") AS CC_A On CC_A.Mbid = tbl_Memberinfo.Mbid And  CC_A.Mbid2 = tbl_Memberinfo.Mbid2 ";            

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
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text ) == false) return;
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
                progress.Value = progress.Value + 10; progress.Refresh();

                //chart_Item.Series.Clear();
                //Save_Nom_Line_Chart();
                //Set_SalesItemDetail(Mbid, Mbid2); //상품 관련 집계 도표를 뿌려준다.
                //Set_Form_Date_Talk(); //상담내역을 뿌려준다.
                progress.Value = progress.Value + 10; progress.Refresh();

                //Set_SalesDetail_Chart(Mbid, Mbid2); //pie 도표를 뿌려준다.
                progress.Value = progress.Value + 10; progress.Refresh();

                progress.Visible =false ;
                this.Cursor = System.Windows.Forms.Cursors.Default ;           
                            
                mtxtMbid.Focus();                
            }
            
            Data_Set_Form_TF = 0;            
        }

        private void Set_Form_Date(DataSet ds)
        {
            StringEncrypter decrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

            idx_Mbid =  ds.Tables[base_db_name].Rows[0]["Mbid"].ToString();
            idx_Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[0]["Mbid2"].ToString());
            txt_ED_Date.Text = ds.Tables[base_db_name].Rows[0]["ED_Date"].ToString();
            mtxtMbid.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();
            txtName.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();
            mtxtSn.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Cpno"].ToString(), "Cpno");
            txtName_E_1.Text = ds.Tables[base_db_name].Rows[0]["E_name"].ToString();
            txtName_E_2.Text = ds.Tables[base_db_name].Rows[0]["E_name_Last"].ToString();
            txtLineCnt.Text = ds.Tables[base_db_name].Rows[0]["LineCnt"].ToString();
        
            txtMbid_s.Text = ds.Tables[base_db_name].Rows[0]["T_Saveid"].ToString();
            txtMbid_s2.Text = ds.Tables[base_db_name].Rows[0]["T_Saveid"].ToString();
            txtName_s.Text = ds.Tables[base_db_name].Rows[0]["Save_Name"].ToString();
            txtName_s2.Text = ds.Tables[base_db_name].Rows[0]["Save_Name"].ToString();
            //txtSN_s.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Save_Cpno"].ToString(), "Cpno");
            //txtSN_s2.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Save_Cpno"].ToString(), "Cpno");

            txtMbid_n.Text = ds.Tables[base_db_name].Rows[0]["T_Nominid"].ToString();
            txtMbid_n2.Text = ds.Tables[base_db_name].Rows[0]["T_Nominid"].ToString();
            txtName_n.Text = ds.Tables[base_db_name].Rows[0]["Nomin_Name"].ToString();
            txtName_n2.Text = ds.Tables[base_db_name].Rows[0]["Nomin_Name"].ToString();
            //txtSN_n.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Nom_Cpno"].ToString(), "Cpno");
            //txtSN_n2.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Nom_Cpno"].ToString(), "Cpno");

            txtGrade.Text = ds.Tables[base_db_name].Rows[0]["G_Name"].ToString();
            txtGradeP.Text = ds.Tables[base_db_name].Rows[0]["G_NameP"].ToString();

            if (ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString().Replace("-", "").Trim() != "")            
                txtLeaveDate.Text = string.Format("{0:####-##-##}", int.Parse(ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString()));//ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString();

            if (ds.Tables[base_db_name].Rows[0]["LineUserDate"].ToString().Replace("-", "").Trim() != "")            
                txtLineDate.Text = string.Format("{0:####-##-##}", int.Parse(ds.Tables[base_db_name].Rows[0]["LineUserDate"].ToString()));//ds.Tables[base_db_name].Rows[0]["LineUserDate"].ToString();

            if (ds.Tables[base_db_name].Rows[0]["PayStop_Date"].ToString().Replace("-", "").Trim() != "")            
                txtS.Text = string.Format("{0:####-##-##}", int.Parse(ds.Tables[base_db_name].Rows[0]["PayStop_Date"].ToString()));  //ds.Tables[base_db_name].Rows[0]["PayStop_Date"].ToString();

            if (ds.Tables[base_db_name].Rows[0]["Regtime"].ToString().Replace("-", "").Trim() != "")     
                mtxtRegDate.Text =  string.Format("{0:####-##-##}", int.Parse(ds.Tables[base_db_name].Rows[0]["Regtime"].ToString())); //ds.Tables[base_db_name].Rows[0]["Regtime"].ToString();

            if (ds.Tables[base_db_name].Rows[0]["Ed_Date"].ToString().Replace("-", "").Trim() != "")         
                mtxtEdDate.Text = string.Format("{0:####-##-##}", int.Parse(ds.Tables[base_db_name].Rows[0]["Ed_Date"].ToString()));  // ds.Tables[base_db_name].Rows[0]["Ed_Date"].ToString();

            if (ds.Tables[base_db_name].Rows[0]["state"].ToString().Replace("-", "").Trim() != "")
            {
                cbProvince_TH.Text = ds.Tables[base_db_name].Rows[0]["state"].ToString();
            }

            if (ds.Tables[base_db_name].Rows[0]["city"].ToString().Replace("-", "").Trim() != "")
            {
                cbDistrict_TH.Text = ds.Tables[base_db_name].Rows[0]["city"].ToString();
            }

            if (ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString().Replace ("-","").Trim () != "")
            {
                // 태국 국가코드인 경우
                if (ds.Tables[base_db_name].Rows[0]["Nation_Code"].ToString().Replace("-", "").Trim() == "TH")
                {
                    try
                    {
                        cbProvince_TH.Text = ds.Tables[base_db_name].Rows[0]["Address2"].ToString().Split(' ')[2];
                        cbDistrict_TH.Text = ds.Tables[base_db_name].Rows[0]["Address2"].ToString().Split(' ')[1];
                        cbSubDistrict_TH.Text = ds.Tables[base_db_name].Rows[0]["Address2"].ToString().Split(' ')[0];
                    }
                    catch (Exception)
                    {
                        cbProvince_TH.Text = "";
                        cbDistrict_TH.Text = "";
                        cbSubDistrict_TH.Text = "";
                    }

                    txtZipCode_TH.Text = ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString();
                    Update();
                }
                // 그 외 국가코드인 경우
                else
                {
                    //txtAddCode1.Text = ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString().Substring(0, 3);
                    //txtAddCode2.Text = ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString().Substring(3, 3);                
                    mtxtZip1.Text = ds.Tables[base_db_name].Rows[0]["Addcode1"].ToString();
                }
            }
            txtAddress1.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Address1"].ToString());
            txtAddress2.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Address2"].ToString());


            string T_tel = "";
            cls_form_Meth cfm = new cls_form_Meth();

            if (decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hometel"].ToString()).Replace("-", "").Trim() != "")
            {
                new cls_form_Meth().Home_Number_Setting( ds.Tables[base_db_name].Rows[0]["hometel"].ToString(), mtxtTel1);

                //string[] tel = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hometel"].ToString()).Split('-');
                //txtTel_1.Text = tel[0].ToString ();
                //txtTel_2.Text = tel[1].ToString();
                //txtTel_3.Text = tel[2].ToString();
                cfm.Home_Number_Setting(ds.Tables[base_db_name].Rows[0]["hometel"].ToString(), mtxtTel1);

               // T_tel = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hometel"].ToString());
            }

            if (decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hptel"].ToString()).Replace("-", "").Trim() != "")
            {
                //string[] tel = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hptel"].ToString()).Split('-');
                //txtTel2_1.Text = tel[0].ToString();
                //txtTel2_2.Text = tel[1].ToString();
                //txtTel2_3.Text = tel[2].ToString();
                cfm.Home_Number_Setting(ds.Tables[base_db_name].Rows[0]["hptel"].ToString(), mtxtTel2);
            }


            txtCenter.Text = ds.Tables[base_db_name].Rows[0]["B_Name"].ToString();
            txtCenter_Code.Text = ds.Tables[base_db_name].Rows[0]["businesscode"].ToString();

            txtBank.Text = ds.Tables[base_db_name].Rows[0]["bank_Name"].ToString();
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
                BirthDay = BirthDay + "-" +  ds.Tables[base_db_name].Rows[0]["BirthDay_M"].ToString();
                BirthDay = BirthDay + "-" +  ds.Tables[base_db_name].Rows[0]["BirthDay_D"].ToString();

                mtxtBrithDay.Text = BirthDay;
            }

            //소비자(프리퍼드커스텀)는 1 판매원(어소시에이트)은 기본 0
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

            //제3자동의
            if (ds.Tables[base_db_name].Rows[0]["Third_Person_Agree"].ToString() == "1")
                checkB_Third_Person_Agree.Checked = true;
            else
                checkB_Third_Person_Agree.Checked = false;
            //전환가입불가설정
            if (ds.Tables[base_db_name].Rows[0]["LeaveCheck_FLAG"].ToString() == "1")
                chk_LeaveCheck_FLAG.Checked = true;
            else
                chk_LeaveCheck_FLAG.Checked = false;
            //판매불간으 비자발급대상자
            if (ds.Tables[base_db_name].Rows[0]["For_Kind_TF_FLAG"].ToString() == "1")
                chk_For_Kind_TF_FLAG.Checked = true;
            else
                chk_For_Kind_TF_FLAG.Checked = false;
            //마케팅수신동의
            if (ds.Tables[base_db_name].Rows[0]["AgreeMarketing"].ToString() == "Y")
                checkB_AgreeMarketing.Checked = true;
            else
                checkB_AgreeMarketing.Checked = false;
            //국가코드
            combo_Se_2.Text = ds.Tables[base_db_name].Rows[0]["nationNameKo"].ToString();
            combo_Se_Code_2.Text = ds.Tables[base_db_name].Rows[0]["Nation_Code"].ToString();



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

            combo_Se.Text = ds.Tables[base_db_name].Rows[0]["nationNameKo"].ToString();
            combo_Se_Code.Text = ds.Tables[base_db_name].Rows[0]["Na_Code"].ToString();

            radioB_Sex_X.Checked = ds.Tables[base_db_name].Rows[0]["Sex_FLAG"].ToString() == "X";
            radioB_Sex_Y.Checked = ds.Tables[base_db_name].Rows[0]["Sex_FLAG"].ToString() == "Y";
            checkB_SMS_FLAG.Checked = ds.Tables[base_db_name].Rows[0]["AgreeSMS"].ToString() == "Y";
            checkB_EMail_FLAG.Checked = ds.Tables[base_db_name].Rows[0]["AgreeEmail"].ToString() == "Y";


            if (ds.Tables[base_db_name].Rows[0]["C_M_Name"].ToString() != "")
            {
                check_CC.Checked = true;
                txtName_C.Text = ds.Tables[base_db_name].Rows[0]["C_M_Name"].ToString();
                mtxtSn_C.Text = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["C_cpno"].ToString());

                txtName_E_1_C.Text = ds.Tables[base_db_name].Rows[0]["C_E_name"].ToString();
                txtName_E_2_C.Text = ds.Tables[base_db_name].Rows[0]["C_E_name_Last"].ToString();
                                
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


            button_exigo.Visible = false;

            //if (int.Parse (ds.Tables[base_db_name].Rows[0]["US_Num"].ToString()) == 0 )
            //    button_exigo.Visible = true;


            txt_Us.Text = ds.Tables[base_db_name].Rows[0]["US_Num"].ToString(); 


            txtName.ReadOnly = true;
            txtName.BackColor = cls_app_static_var.txt_Enable_Color; 
            txtName.BorderStyle = BorderStyle.FixedSingle;
        }

        private void Set_Form_Date_Up(int intTemp) //추천 관련.
        {
            if (intTemp ==1 ) //추천상위
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




        private void dGridView_Sell_Down_Header_Reset()
        {

            cgb_Down.grid_col_Count = 3;
            cgb_Down.basegrid = dGridView_ED;
            cgb_Down.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Down.grid_Frozen_End_Count = 2;
            cgb_Down.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"교육이름","신청시간"  , "수료여부"
                                    };
            cgb_Down.grid_col_header_text = g_HeaderText;

            int[] g_Width = {90, 90, 90
                            };
            cgb_Down.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true
                                   };
            cgb_Down.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter

                              };
            cgb_Down.grid_col_alignment = g_Alignment;



        }


        private void Set_Down_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]

                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void Set_Form_Date_Info()
        {
            cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
            cgbp.dGridView_Put_baseinfo(this, dGridView_Sell, "sell", mtxtMbid.Text);

            cls_Grid_Base_info_Put cgbp2 = new cls_Grid_Base_info_Put();
            cgbp2.dGridView_Put_baseinfo(this, dGridView_inf, "memc", mtxtMbid.Text);

            cls_Grid_Base_info_Put cgbp3 = new cls_Grid_Base_info_Put();
            cgbp3.dGridView_Put_baseinfo(this, dGridView_Up, "memupc", mtxtMbid.Text);

            cls_Grid_Base_info_Put cgbp4 = new cls_Grid_Base_info_Put();
            cgbp4.dGridView_Put_baseinfo(this, dGridView_Add, "memadd", mtxtMbid.Text);

            cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
            cgbp5.dGridView_Put_baseinfo(this, dGridView_Talk, "talk", mtxtMbid.Text);


            //cls_Grid_Base_info_Put cgbp13 = new cls_Grid_Base_info_Put();
            //cgbp13.dGridView_Put_baseinfo(this, dGridView_Sell_RePay_D2, "RePay_D2", mtxtMbid.Text.Trim());

            //cls_Grid_Base_info_Put cgbp103 = new cls_Grid_Base_info_Put();
            //cgbp103.dGridView_Put_baseinfo(this, dGridView_Sell_RePay_D4, "RePay_D4", mtxtMbid.Text.Trim());


            cg_Up_S.d_Grid_view_Header_Reset();

            Base_Grid_info_Set(5);


            dGridView_Sell_Down_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Down.d_Grid_view_Header_Reset();

            Base_Grid_Set_2(mtxtMbid.Text);

            //dGridView_Info_Header_Reset(dGridView_inf, 2);
            //cg_Up_S.d_Grid_view_Header_Reset();

            //Base_Grid_info_Set(2);

            //dGridView_Info_Header_Reset(dGridView_Up, 3);
            //cg_Up_S.d_Grid_view_Header_Reset();

            //Base_Grid_info_Set(3);


            //dGridView_Info_Header_Reset(dGridView_Add, 4);
            //cg_Up_S.d_Grid_view_Header_Reset();

            //Base_Grid_info_Set(4);
        }





        private void Base_Grid_Set_2(string MBID2)
        {
            string Tsql = "";
            Tsql = "select A.TITLE,B.REG_TIME,REPLACE(B.GRADUATION_DATE,'-','') from TLS_BOARD_SCHEDULE A JOIN TLS_BOARD_SCHEDULE_APPLY B ON A.SCHEDULE_SEQ = B.SCHEDULE_SEQ WHERE B.MBID2 = " + MBID2 + "";


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
                Set_Down_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

            }
            cgb_Down.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Down.db_grid_Obj_Data_Put();


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

                Tsql = Tsql + " FROM tbl_Memberinfo_Mod AS A (nolock) " ;
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

            else if (intTemp == 5)
            {

                Tsql = Tsql + " OrderDate ";
                Tsql = Tsql + " ,Gid   ";
                Tsql = Tsql + " ,Case When Send_Result =1 then '성공' ELSE '실패' End Send_Result ";
                Tsql = Tsql + ", Send_Error ";
                Tsql = Tsql + " ,RecordTime ";


                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";
                Tsql = Tsql + " ,'' ";

                Tsql = Tsql + " ,'' ";

                Tsql = Tsql + " From tbl_Memberinfo_Ca_A_Monthly_Mod (nolock) ";

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By Base_Index dESC ";
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
            int T_cnt = 0;
            double S_cnt4 = 0;    double S_cnt5 = 0;    double S_cnt6 = 0;    double S_cnt7 = 0;   double S_cnt8 = 0;   double S_cnt9 = 0;
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                T_cnt = fi_cnt;
                if (intTemp == 1)
                {
                    S_cnt4 = S_cnt4 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][4].ToString() );
                    S_cnt5 = S_cnt5 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][5].ToString());
                    S_cnt6 = S_cnt6 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][6].ToString());
                    S_cnt7 = S_cnt7 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][7].ToString());
                    S_cnt8 = S_cnt8 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][8].ToString());
                    S_cnt9 = S_cnt9 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][9].ToString());
                }
            }


            if (intTemp == 1)
            {
                object[] row0 = { ""
                                    ,"<< " + cm._chang_base_caption_search("합계") + " >>"
                                    ,""
                                    ,""
                                    ,S_cnt4

                                    ,S_cnt5
                                    ,S_cnt6
                                    ,S_cnt7
                                    ,S_cnt8
                                    ,S_cnt9

                                    ,""
                                     };

                gr_dic_text[T_cnt + 2] = row0;
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

                                ,ds.Tables[base_db_name].Rows[fi_cnt][10]  
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }




        private void dGridView_Info_Header_Reset(DataGridView t_Dgv, int intTemp)
        {
            cg_Up_S.Grid_Base_Arr_Clear();
            cg_Up_S.basegrid = t_Dgv; 
            cg_Up_S.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;            
            cg_Up_S.grid_col_Count = 11;

            //cg_sub.grid_Frozen_End_Count = 2;
            cg_Up_S.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            

            if (intTemp == 1)
            {
                string[] g_HeaderText = {"매출_일자" ,  "주문번호" ,  "주문_종류"   , "상태"  , "매출액"  
                                        , "입급액"  ,"매출PV"  , "현금"  , "카드" , "무통장" 
                                        , "비고"
                                        };

                Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
                gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;

                cg_Up_S.grid_cell_format = gr_dic_cell_format;

                int[] g_Width = { 100, 90, 70, 80, 80
                                 ,80 , 80 , 80 , 80 , 80
                                 ,100
                                };

                DataGridViewContentAlignment[] g_Alignment =
                                  {DataGridViewContentAlignment.MiddleCenter  
                                   ,DataGridViewContentAlignment.MiddleCenter 
                                   ,DataGridViewContentAlignment.MiddleCenter  
                                   ,DataGridViewContentAlignment.MiddleCenter
                                   ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                   ,DataGridViewContentAlignment.MiddleRight 
                                   ,DataGridViewContentAlignment.MiddleRight  
                                   ,DataGridViewContentAlignment.MiddleRight
                                   ,DataGridViewContentAlignment.MiddleRight  //10

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
                                    ,""
                                    };

                int[] g_Width = { 120, 100, 100, 100, 80
                                 ,0 , 0 , 0 , 0 , 0
                                 ,0
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
                                    ,""
                                    };

                int[] g_Width = { 120, 100, 100, 100, 100
                                 ,80 , 80 , 0 , 0 , 0
                                 ,0
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

                                   ,DataGridViewContentAlignment.MiddleCenter  //10
                                  };

                cg_Up_S.grid_col_header_text = g_HeaderText;
                cg_Up_S.grid_col_w = g_Width;
                cg_Up_S.grid_col_alignment = g_Alignment;
            }

            else if (intTemp == 4)
            {
                string[] g_HeaderText = {"구분"  , "우편_번호"   , "주소1"  , "주소2"   , "연락처1"        
                                    , "연락처2"   , "수취인명"    , ""  , "" , ""
                                    ,""
                                    };

                int[] g_Width = { 120, 100, 100, 100, 100
                                 ,80 , 80 , 0 , 0 , 0
                                 ,0
                                };

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
                                  };

                cg_Up_S.grid_col_header_text = g_HeaderText;
                cg_Up_S.grid_col_w = g_Width;
                cg_Up_S.grid_col_alignment = g_Alignment;
            }



            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true                      
                                   };            
            cg_Up_S.grid_col_Lock = g_ReadOnly;
            
            cg_Up_S.basegrid.RowHeadersVisible = false;
        }



        private void dGridView_Info_Header_Reset(DataGridView t_Dgv, string intTemp)
        {
            cg_Up_S.Grid_Base_Arr_Clear();
            cg_Up_S.basegrid = t_Dgv;
            cg_Up_S.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_Up_S.grid_col_Count = 11;
            cg_Up_S.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
            if (intTemp == "1")
            {
                string[] g_HeaderText = {"구분" ,  "마감일자" ,  "지급일자"   , "발생액"  , "소득세"  
                                        , "주민세"  ,"실지급액"  , ""  , "" , "" 
                                        , ""
                                        };

                Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
                gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                
                cg_Up_S.grid_cell_format = gr_dic_cell_format;

                int[] g_Width = { 100, 90, 70, 80, 80
                                 ,80 , 80 , 0 , 0 , 0
                                 ,0
                                };

                DataGridViewContentAlignment[] g_Alignment =
                                  {DataGridViewContentAlignment.MiddleCenter  
                                   ,DataGridViewContentAlignment.MiddleCenter 
                                   ,DataGridViewContentAlignment.MiddleCenter  
                                   ,DataGridViewContentAlignment.MiddleCenter
                                   ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                   ,DataGridViewContentAlignment.MiddleRight 
                                   ,DataGridViewContentAlignment.MiddleRight  
                                   ,DataGridViewContentAlignment.MiddleRight
                                   ,DataGridViewContentAlignment.MiddleRight  //10

                                   ,DataGridViewContentAlignment.MiddleCenter  //10
                                  };

                cg_Up_S.grid_col_header_text = g_HeaderText;
                cg_Up_S.grid_col_w = g_Width;
                cg_Up_S.grid_col_alignment = g_Alignment;
            }           
            
            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true                      
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

            else if ((tb.Tag != null) &&  tb.Tag.ToString() == "name")  //회원 정보 관련해서 이름 검색을 필요로 하는 텍스트 박스이다.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(tb, e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "."))
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1, ".") == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

        }




        private void MtxtData_Temp_KeyPress(object sender, KeyPressEventArgs e)
        {

            mtxtTel1.Mask = cls_app_static_var.Tel_Number_Fromat;

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
                        if (Sn_Number_(Sn, mtb, "Zip") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtZip_Auto")
                    {
                        if (Sn_Number_(Sn, mtb, "Zip") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel_Auto")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }


                    if (mtb.Name == "mtxtBrithDay")
                    {
                        if (Sn_Number_(Sn, mtb, "Date") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtVisaDay")
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
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("hptel"
                      + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    }
                    else
                    {
                        MessageBox.Show("휴대폰"
                       + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    }
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
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter_Code.Text = "";
                Data_Set_Form_TF = 0;
               
            }

            if (tb.Name == "txtBank")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtBank_Code.Text = "";
                Data_Set_Form_TF = 0;
                
            }

            if (tb.Name == "txtWebID")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtBank_Code.Text = "";
                Data_Set_Form_TF = 0;

            }
            
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
            if (tab_Nation.Visible == true)
            {
                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex;
                if (combo_Se_Code.Text == "")  //다국어 지원프로그램을 사용시 국가는 필히 선택을 해야 된다.
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Not_Na_Code")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    combo_Se.Focus(); return;
                }
            }

            if (tb.Name == "txtCenter")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter_Code);
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
                Db_Grid_Popup(tb, txtBank_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtBank_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtBank_Code);

                //SendKeys.Send("{TAB}");
                if(txtBank_Code.Text == "999")
                {
                    txtAccount.Text = "555555";
                }
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
                    cgb_Pop.Next_Focus_Control = mtxtZip1;
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
                    if (combo_Se_Code.Text.Trim() != "" ) Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " And ShowMemberCenter = 'Y'"; // 2018-11-23 지성경 에스제이로직스는 선택불가능하게끔한다.
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                    cgb_Pop.Next_Focus_Control = mtxtZip1;
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
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + combo_Se_Code.Text.Trim() + "') )";
                if (combo_Se_Code.Text.Trim() != "") Tsql = Tsql + " And   Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
             //   Tsql = Tsql + " And ShowMemberCenter = 'Y' ";
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
                Tsql = Tsql + " Where (Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%')";
                if (combo_Se_Code.Text.Trim() != "") Tsql = Tsql + " And   Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 
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




















        private void _From_Data_Clear()
        {
            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb.d_Grid_view_Header_Reset();
            //Base_Grid_Set(); //당일등록 회원을 불러온다.
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Up_S_Header_Reset(dGridView_Up_N); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset(1);

            dGridView_Up_S_Header_Reset(dGridView_Up_S); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset(1);

            dGridView_Up_S_Header_Reset(dGridView_Down_N); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset(1);

            dGridView_Up_S_Header_Reset(dGridView_Down_S); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset(1);

            cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
            cgbp.dGridView_Put_baseinfo( dGridView_Sell, "sell");

            cls_Grid_Base_info_Put cgbp2 = new cls_Grid_Base_info_Put();
            cgbp2.dGridView_Put_baseinfo( dGridView_inf, "memc");

            cls_Grid_Base_info_Put cgbp3 = new cls_Grid_Base_info_Put();
            cgbp3.dGridView_Put_baseinfo( dGridView_Up, "memupc");

            cls_Grid_Base_info_Put cgbp4 = new cls_Grid_Base_info_Put();
            cgbp4.dGridView_Put_baseinfo( dGridView_Add, "memadd");



            //dGridView_Info_Header_Reset(dGridView_Pay, "1");
            //cg_Up_S.d_Grid_view_Header_Reset(1);



            cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
            cgbp5.dGridView_Put_baseinfo(dGridView_Sell_Item, "item");

            cls_Grid_Base_info_Put cgbp6 = new cls_Grid_Base_info_Put();
            cgbp6.dGridView_Put_baseinfo(dGridView_Sell_Cacu, "cacu");

            cls_Grid_Base_info_Put cgbp7 = new cls_Grid_Base_info_Put();
            cgbp7.dGridView_Put_baseinfo(dGridView_Sell_Rece, "rece");


            cls_Grid_Base_info_Put cgbp8 = new cls_Grid_Base_info_Put();
            cgbp8.dGridView_Put_baseinfo(dGridView_Pay, "pay");

            cls_Grid_Base_info_Put cgbp9 = new cls_Grid_Base_info_Put();
            cgbp9.dGridView_Put_baseinfo( dGridView_Down_N2, "nomindown");

            cls_Grid_Base_info_Put cgbp10 = new cls_Grid_Base_info_Put();
            cgbp10.dGridView_Put_baseinfo( dGridView_Down_S2, "savedown");

            cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
            cgbp11.dGridView_Put_baseinfo(dGridView_Talk, "talk");

            cls_Grid_Base_info_Put cgbp12 = new cls_Grid_Base_info_Put();
            cgbp12.dGridView_Put_baseinfo(dGridView_SaveDefault, "savedefault");
            //dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Item.d_Grid_view_Header_Reset(1);

            //dGridView_Sell_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Cacu.d_Grid_view_Header_Reset(1);

            //dGridView_Sell_Rece_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Rece.d_Grid_view_Header_Reset(1);


            tabC_Up.SelectedIndex = 0;            
            tabC_Mem.SelectedIndex = 0;
            tabC_1.SelectedIndex = 0;
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            txtName.ReadOnly =false ;
            txtName.BackColor = SystemColors.Window;
            txtName.BorderStyle = BorderStyle.Fixed3D; 

            //txtName.BackColor = Color.FromArgb(236, 241, 220); 
            //txtName.BorderStyle = BorderStyle.Fixed3D; 

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtMbid);

            opt_sell_2.Checked = true; opt_Bir_TF_1.Checked = true;
            raButt_IN_1.Checked = true;
            check_BankDocument.Checked = false;
            check_CpnoDocument.Checked = false;

            opt_B_1.Checked = false; opt_B_2.Checked = false; opt_B_3.Checked = false; 

            chk_N.Checked = false; chk_S.Checked = false;
            mtxtSn.Mask = "999999-9999999";
            if (cls_User.gid_CountryCode == "TH")
            {
                //태국은 입력받을수 있음 
                mtxtSn.Mask = string.Empty;
                mtxtSn.ReadOnly = false;
                mtxtSn.Enabled = true;
                mtxtSn.Visible = true;
                mtxtSn.BackColor = Color.White;
            }

            mtxtTel1.Mask = cls_app_static_var.Tel_Number_Fromat;


            idx_Mbid = ""; idx_Mbid2 = 0;
            idx_Password = "";
            txtB1.Text = "0";
            button_exigo.Enabled = true;
            button_exigo.Visible = false; 

            Reset_Chart_Total();

            combo_Se.Text = ""; combo_Se_Code.Text = "";
            radioB_RBO.Checked = true;
            radioB_G8.Checked = true; 
            
            mtxtMbid.Focus();
        }
        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "후원인엑셀";
            Excel_Export_From_Name = this.Name;
            return dGridView_Sell_RePay_D2;
        }

        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {                
                _From_Data_Clear();

                combo_Se_Code.Text = cls_User.gid_CountryCode;
                combo_Se_Code_2.Text = cls_User.gid_CountryCode;
            }

            else if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Save_Base_Data(ref Save_Error_Check);

                if (Save_Error_Check > 0)
                {
                    _From_Data_Clear();
                    combo_Se_Code.Text = cls_User.gid_CountryCode;
                    combo_Se_Code_2.Text = cls_User.gid_CountryCode;
                }
                
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            else if (bt.Name == "butt_Excel")
            {
                frmBase_Excel e_f = new frmBase_Excel();
                e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
                e_f.ShowDialog();
            }
            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }

            else if (bt.Name == "butt_AddCode")
            {
                frmBase_AddCode e_f = new frmBase_AddCode();
                e_f.Send_Address_Info += new frmBase_AddCode.SendAddressDele(e_f_Send_Address_Info);
                e_f.ShowDialog();

                txtAddress2.Focus();
            }

            else if (bt.Name == "butt_AddCode2")
            {
                frmBase_AddCode e_f = new frmBase_AddCode();
                e_f.Send_Address_Info += new frmBase_AddCode.SendAddressDele(e_f_Send_Address_Info2);
                e_f.ShowDialog();

                txtAddress4.Focus();
            }

            else if (bt.Name == "butt_AddCodeT1")
            {
                txtAddress3.Text = txtAddress1.Text;
                txtAddress4.Text = txtAddress2.Text;
                mtxtZip2.Text = mtxtZip1.Text;

                txtAddress4.Focus();
            }
            else if(bt.Name == "butt_Delete")
            {
                int Delete_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Delete_Base_Data(ref Delete_Error_Check);

                if (Delete_Error_Check > 0)
                    _From_Data_Clear();

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        
          

        }

        private void e_f_Send_Address_Info(string AddCode1, string AddCode2, string Address1, string Address2, string Address3)
        {
            mtxtZip1.Text = AddCode1 + "-" + AddCode2; 
            txtAddress1.Text = Address1; txtAddress2.Text = Address2;
                        
        }

        private void e_f_Send_Address_Info2(string AddCode1, string AddCode2, string Address1, string Address2, string Address3)
        {
            mtxtZip2.Text = AddCode1 + "-" + AddCode2;
            txtAddress3.Text = Address1; txtAddress4.Text = Address2;

        }


        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            //SendKeys.Send("{TAB}");
        }

   
  


        private bool  Check_TextBox_Error_Date()
        {
            cls_Check_Input_Error c_er = new cls_Check_Input_Error();
            if (mtxtRegDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_1(mtxtRegDate.Text, mtxtRegDate, "Date") == false)
                {
                    mtxtRegDate.Focus();
                    return false;
                }
            }

            if (mtxtBrithDay.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_1(mtxtBrithDay.Text, mtxtBrithDay, "Date") == false)
                {
                    mtxtBrithDay.Focus();
                    return false;
                }
            }

            if (mtxtVisaDay.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_1(mtxtVisaDay.Text, mtxtVisaDay, "Date") == false)
                {
                    mtxtVisaDay.Focus();
                    return false;
                }
            }

            if (mtxtEdDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_1(mtxtEdDate.Text, mtxtEdDate, "Date") == false)
                {
                    mtxtEdDate.Focus();
                    return false;
                }
            }

            if (mtxtRBODate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_1(mtxtRBODate.Text, mtxtRBODate, "Date") == false)
                {
                    mtxtRBODate.Focus();
                    return false;
                }
            }

            return true;
        }


        private Boolean Check_USA_Error()
        {
            //미국회원번호 부여안됐으면 수정못하게 막는다.
            string Tsql = "";
            
            Tsql = "Select isnull(US_Num,0) FROM TBL_MEMBERINFO WHERE MBID2= '"+ mtxtMbid.Text +"' ";

            

            //당일 등록된 회원을 불러온다.

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return false;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)
            {
                return false;
            }
                
            string us_num = ds.Tables[base_db_name].Rows[0][0].ToString();

            if (us_num == "0")
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Membership information cannot be modified yet because the US membership number has not been assigned.");
                }
                else
                {
                    MessageBox.Show("미국회원번호가 부여되지 않아 아직 회원정보를 수정 할 수 없습니다.");
                }
                return false;
            }
            else
            {
                return true;
            }
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

            if (mtxtRegDate.Text =="") //등록일자가 빈칸으로 되어 잇으면 당일을 셋팅한다.
                mtxtRegDate.Text = DateTime.Now.ToString("yyyy-MM-dd");




            string Sn = string.Empty;
            //Sn = mtxtTel1.Text.Replace("-", "").Replace("_", "").Trim();
            //if (Sn_Number_1(Sn, mtxtTel1, "Tel") == false)
            //{
            //    mtxtTel1.Focus();
            //    return false;
            //}

            Sn = mtxtTel2.Text.Replace("-", "").Replace("_", "").Trim();
            if (Sn_Number_1(Sn, mtxtTel2, "HpTel") == false)
            {
                mtxtTel2.Focus();
                return false;
            }



            // 태국인 경우
            // if (combo_Se_Code.Text == "NA")
            if (combo_Se_Code_2.Text == "TH")
            {
                // to do : syhuh 230824..
            }
            // 태국 이외
            else
            {
                Sn = mtxtZip1.Text.Replace("-", "").Replace("_", "").Trim();

                if (Sn_Number_1(Sn, mtxtZip1, "Zip") == false)
                {
                    mtxtZip1.Focus();
                    return false;
                }
            }


            //if (radioB_Sex_X.Checked == false && radioB_Sex_Y.Checked == false)
            //{
            //    MessageBox.Show("성별을 선택해주시기바랍니다.");
            //    radioB_Sex_X.Focus();
            //    return false;
            //}
            //if (mtxtBrithDay.Text.Replace("-", "") == "" || mtxtBrithDay.Text.Replace("-", "").Length != 8)
            //{
            //    me = cls_app_static_var.app_msg_rm.GetString("Msg_Sort_BirthDay") + "\n" +
            //     cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

            //    MessageBox.Show(me);
            //    mtxtBrithDay.Focus();
            //    return false;
            //}
            //////First 영문이름 
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
            //우편번호
            // 태국인 경우
            if (combo_Se_Code_2.Text == "TH")
            {
                if (cbProvince_TH.SelectedIndex < 0)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify province.");
                    }
                    else
                    {
                        MessageBox.Show("Province를 지정해주세요");
                    }
                    cbProvince_TH.Focus();
                    return false;
                }
                else if (cbDistrict_TH.SelectedIndex < 0)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify district.");
                    }
                    else
                    {
                        MessageBox.Show("district를 지정해주세요");
                    }
                    cbDistrict_TH.Focus();
                    return false;
                }
                else if (cbSubDistrict_TH.SelectedIndex < 0)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify subdistrict.");
                    }
                    else
                    {
                        MessageBox.Show("subdistrict를 지정해주세요");
                    }
                    cbDistrict_TH.Focus();
                    return false;
                }
                else if (txtZipCode_TH.Text == "")
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify postal code.");
                    }
                    else
                    {
                        MessageBox.Show("우편번호를 지정해주세요");
                    }
                    txtZipCode_TH.Focus();
                    return false;
                }

                //핸드폰번호 - 태국
                if (mtxtTel2.Text.Replace("-", "") == "" || mtxtTel2.Text.Replace(" ", "").Replace("-", "").Length <= 9)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify your cell phone number.");
                    }
                    else
                    {
                        MessageBox.Show("핸드폰번호를 지정해주세요");
                    }
                    mtxtTel2.Focus();
                    return false;
                }
            }
            // 그 외 국가인 경우
            else
            {
                if (mtxtZip1.Text == "")
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify postal code.");
                    }
                    else
                    {
                        MessageBox.Show("우편번호를 지정해주세요");
                    }
                    mtxtZip1.Focus();
                    return false;
                }

                //핸드폰번호 - 한국
                if (mtxtTel2.Text.Replace("-", "") == "" || mtxtTel2.Text.Replace(" ", "").Replace("-", "").Length != 11)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify your cell phone number.");
                    }
                    else
                    {
                        MessageBox.Show("핸드폰번호를 지정해주세요");
                    }
                    mtxtTel2.Focus();
                    return false;
                }

            }
            ////이메일
            //Sn = txtEmail.Text.Replace("-", "").Replace("_", "").Trim();
            //if (Sn_Number_1(Sn, txtEmail, "Email") == false)
            //{
            //    txtEmail.Focus();
            //    return false;
            //}

            //집주소
            if (cls_User.gid_CountryCode != "TH")
            {
                Sn = mtxtZip1.Text.Replace("-", "").Replace("_", "").Trim();
                if (Sn_Number_1(Sn, mtxtZip1, "Zip") == false)
                {
                    mtxtZip1.Focus();
                    return false;
                }
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
            if (Check_TextBox_Error_Date() == false) return false;

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

            if (mtxtBrithDayC.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_1(mtxtBrithDayC.Text, mtxtBrithDayC, "Date") == false)
                {
                    mtxtBrithDayC.Focus();
                    return false;
                }
            }

            return true;
        }

        /// <summary> 동일인물이있는가? </summary>
        private Boolean Check_Duplication_Error()
        {
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
                    MessageBox.Show(string.Format("{0}님 이름과 생년월일로 중복 체크 결과 {1}명이있는것을 확인했습니다."
                        , txtName.Text
                        , RowValue));
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
            sb.AppendLine(string.Format("and hptel = '{0}'", mtxtTel2.Text));

            DataSet ds = new DataSet();
            if (Temp_Connect.Open_Data_Set(sb.ToString(), "Check_Join", ds, this.Name, this.Text) == false) return false;
            if (Temp_Connect.DataSet_ReCount == 0) return true;

            int RowValue = 0;
            if (int.TryParse(ds.Tables["Check_Join"].Rows[0][0].ToString(), out RowValue))
            {
                if (RowValue > 0)
                {
                    MessageBox.Show(string.Format("{0}님 이름과 핸드폰번호로 중복 체크 결과 {1}명이있는것을 확인했습니다."
                        , txtName.Text
                        , RowValue));
                    return false;
                }
            }

            return true;
        }
        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;


            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (txtB1.Text.Trim() == "") txtB1.Text = "0";
            //20240521 구현호 태국은 US아이디 안받는다
            if (Check_USA_Error() == false) return;
            //// 한국인 경우에만
            //if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
            //{
            //    if (Check_USA_Error() == false) return;
            //}

            if (Check_TextBox_Error() == false) return;

            if (check_CC.Checked == true)
                if (Check_TextBox_CC_Error() == false) return;  //부부사업자 등록 관련 오류를 체크한다.

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
                        if (cls_User.gid_CountryCode == "TH")
                        {
                            MessageBox.Show("When converting from Begins to RBO, you must enter the date.."
                            + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        }
                        else
                        {

                            MessageBox.Show("비긴즈에서 RBO 전환시에 날짜를 필히 입력 해야 합니다.."
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        }
                        mtxtRBODate.Focus();
                        return ;
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
                string BirthDay = "";  string BirthDay_M ="";  string BirthDay_D=""  ; int BirthDayTF = 0 ;
                string Sex_FLAG = "";
                string AgreeSMS   = "N";
                string AgreeEmail = "N";
                int Sell_Mem_TF = 0; int Add_TF = 0, Myoffice_TF = 0, RBO_Mem_TF = 0, G8_TF = 0;
                int BankDocument = 0, CpnoDocument = 0;
                int For_Kind_TF = 0;
                string ssn = encrypter.Encrypt( mtxtSn.Text);

                if (check_BankDocument.Checked == true) BankDocument = 1;
                if (check_CpnoDocument.Checked == true) CpnoDocument = 1;

                if (mtxtTel1.Text.Replace("-", "").Trim() != "") hometel = mtxtTel1.Text.Replace(" ", ""); 
                if (mtxtTel2.Text.Replace("-", "").Trim() != "") hptel = mtxtTel2.Text.Replace(" ", "");

                if (opt_sell_3.Checked == true) Sell_Mem_TF = 1; //소비자는 1 판매원은 기본 0
                
                if (opt_Bir_TF_1.Checked ==true)  BirthDayTF =1 ; //양력은 1  음력은 2
                if (opt_Bir_TF_2.Checked ==true)  BirthDayTF =2 ;

                if (opt_B_1.Checked == true) Add_TF = 1;  //기본주소가 
                if (opt_B_2.Checked == true) Add_TF = 2; //회사 주소가
                if (opt_B_3.Checked == true) Add_TF = 3; //기본배송지 주소가


                if (radioB_RBO.Checked == true) RBO_Mem_TF = 0;// RBO 0 비긴즈 10
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

                if (checkB_SMS_FLAG.Checked == true)    AgreeSMS    = "Y";
                if (checkB_EMail_FLAG.Checked == true)  AgreeEmail  = "Y";


                if (raButt_IN_2.Checked == true) For_Kind_TF = 1;// 내국인은 0 외국인은 1  사업자는 2
                if (raButt_IN_3.Checked == true) For_Kind_TF = 2;

                StrSql = "";                
                StrSql = "Update tbl_Memberinfo Set ";

                StrSql = StrSql + " E_name = '" + txtName_E_1.Text.Trim() + "'";
                StrSql = StrSql + " ,E_name_Last = '" + txtName_E_2.Text.Trim() + "'";
                StrSql = StrSql + " ,Email = '" +  txtEmail.Text.Trim() + "'";

                //StrSql = StrSql + " ,Email = '" + txtEmail.Text.Trim() + "'";
                StrSql = StrSql + " ,Ed_Date = '" + mtxtEdDate.Text.Replace("-", "").Trim() + "'";
                StrSql = StrSql + " ,Remarks = '" + txtRemark.Text.Trim() + "'";
                StrSql = StrSql + " ,Regtime = '" + mtxtRegDate.Text.Replace("-", "").Trim() + "'";

                StrSql = StrSql + " ,RBO_S_Date = '" + mtxtRBODate.Text.Replace("-", "").Trim() + "'";
               

                StrSql = StrSql + " ,VisaDate = '" + mtxtVisaDay.Text.Replace("-", "").Trim() + "'";                
                // 국가코드 - 태국 선택시
                if (combo_Se_Code_2.Text == "TH")
                {
                    StrSql = StrSql + " ,Addcode1 = '" + txtZipCode_TH.Text.Trim().Replace("-", "") + "'";
                    //StrSql = StrSql + " ,city = '" + cbDistrict_TH.Text.Trim().Replace("-", "") + "'";
                    //StrSql = StrSql + " ,state = '" + cbProvince_TH.Text.Trim().Replace("-", "") + "'";
                    StrSql = StrSql + " ,city = '" + cbDistrict_TH.Text.Trim().Replace("-", "") + "'";
                    StrSql = StrSql + " ,state = '" + cbProvince_TH.SelectedValue.ToString().Trim().Replace("-", "") + "'";
                    StrSql = StrSql + $" , cpno = '{ssn}'";
                }
                // 그 외 국가 선택시
                else
                {
                    StrSql = StrSql + " ,Addcode1 = '" + mtxtZip1.Text.Trim().Replace("-", "") + "'";
                }
                StrSql = StrSql + " ,Address1 = '" + txtAddress1.Text.Replace("'", "''").Trim() + "'";
                StrSql = StrSql + " ,Address2 = '" + txtAddress2.Text.Replace("'", "''").Trim() + "'";
                StrSql = StrSql + " ,hometel = '" + hometel + "'";
                StrSql = StrSql + " ,hptel = '" + hptel + "'";

                StrSql = StrSql + " ,BirthDay = '" + BirthDay + "'";
                StrSql = StrSql + " ,BirthDay_M = '" + BirthDay_M + "'";
                StrSql = StrSql + " ,BirthDay_D = '" + BirthDay_D + "'";

                StrSql = StrSql + " ,BankCode = '" + txtBank_Code.Text.Trim() + "'";
                StrSql = StrSql + " ,bankowner = '" + txtName_Accnt.Text.Trim() + "'";
                StrSql = StrSql + " ,bankaccnt = dbo.ENCRYPT_AES256('" + txtAccount.Text.Trim() + "')";
                StrSql = StrSql + " ,Reg_bankaccnt = dbo.ENCRYPT_AES256('" + txtAccount_Reg.Text.Trim() + "')";
                
                StrSql = StrSql + " ,BusinessCode = '" + txtCenter_Code.Text.Trim () + "'";
                StrSql = StrSql + " ,For_Kind_TF = " + For_Kind_TF;
                if (txtBank_Code.Text == "999")
                {
                    StrSql = StrSql + " ,Account_Wait_FLAG =  1 ";
                }
                else
                {
                    StrSql = StrSql + " ,Account_Wait_FLAG = 0 ";
                }
                if (chk_LeaveCheck_FLAG.Checked == true)
                {
                    StrSql = StrSql + " ,LeaveCheck_FLAG =  1 ";
                }
                else
                {
                    StrSql = StrSql + " ,LeaveCheck_FLAG = 0 ";
                }
                if (chk_For_Kind_TF_FLAG.Checked == true)
                {
                    StrSql = StrSql + " ,For_Kind_TF_FLAG =  1 ";
                }
                else
                {
                    StrSql = StrSql + " ,For_Kind_TF_FLAG = 0 ";
                }
                ////if (txtPassword.Text.Equals(idx_Password) == false)
                if (txtPassword_input.Text == "")
                    {
                    
                    }
                    else
                    {
                        StrSql = StrSql + " ,WebPassWord = '" + EncryptSHA256_EUCKR(txtPassword_input.Text.Trim()) + "'";
                    }
                if (txtWebID.Text == "")
                {
                
                }
                else 
                {
                        StrSql = StrSql + " ,WebID = '" + txtWebID.Text.Trim() + "'";
                    }
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
                //20200609구현호 제3자동의 업데이트
                if (checkB_Third_Person_Agree.Checked == true)
                {
                    StrSql = StrSql + " ,Third_Person_Agree = 1 ";
                }
                else
                {
                    StrSql = StrSql + " ,Third_Person_Agree = 0 ";
                }
                //20200609구현호 제3자동의 업데이트
                if (checkB_AgreeMarketing.Checked == true)
                {
                    StrSql = StrSql + " ,AgreeMarketing = 'Y' ";
                }
                else
                {
                    StrSql = StrSql + " ,AgreeMarketing = 'N' ";
                }
                //StrSql = StrSql + " ,GiBu_ = " + double.Parse (txtB1.Text.Trim ().ToString());
                if (combo_Se_Code_2.Text == "")
                {
                    StrSql = StrSql + " ,Nation_Code = 'KR'";
                }
                else
                {
                    StrSql = StrSql + " ,Nation_Code = '" + combo_Se_Code_2.Text + "'";
                }
                if (Mbid.Length == 0)
                    StrSql = StrSql + " Where Mbid2 = " + Mbid2.ToString();
                else
                {
                    StrSql = StrSql + " Where Mbid = '" + Mbid + "' ";
                    StrSql = StrSql + " And   Mbid2 = " + Mbid2.ToString();
                }

                Temp_Connect.Update_Data (StrSql, Conn, tran, this.Name, this.Text);


                Chang_Mem_Address_R(Mbid, Mbid2, Temp_Connect, Conn, tran);

              
                
                tran.Commit();
                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));


             
                ////--회원수정시 tbl_Memberinfo_Mod_JDE로
                ////--기존 tbl_Memberinfo 와 똑같은 데이터테이블인 곳에
                ////--회원수정시 MBID2만 들어간 가라데이터열이 INSERT된다.
                ////--tbl_Memberinfo_Mod_JDE는 소스의 Cls_Connect_DB.CS의 tbl_Memberinfo_Mod함수에서 업데이트된 내용만 들어간 열로 작업이 된다.
                ////--@v_AU = 'U'시에 tbl_Memberinfo_Mod_JDE의 내용이 mk_customer로 INSERT 된다.
                ////--수정된 내용 하나하나가 INSERT 되는 tbl_Memberinfo_Mod의 방식이 한열로 압축되서 INSERT 되는거라 보면된다.
              


          
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
        




                csd.tbl_Memberinfo_Mod(mtxtMbid.Text.Trim());
                csd_R.tbl_Memberinfo_Mod(mtxtMbid.Text.Trim(), "R", "tbl_Memberinfo_Address", " And Sort_Add = 'R' ");

                cls_Connect_DB Temp_Connect2 = new cls_Connect_DB();
                Temp_Connect2.Connect_DB();
                
                if (combo_Se_Code_2.Text == "TH")   // 태국인 경우
                {
                    StrSql = " EXEC  Usp_JDE_Update_MK_Customer_TH '" + Mbid2 + "','U' ";
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
                    StrSql = StrSql + ", '" + mtxtZip2.Text.Trim().Replace ("-","") + "'";
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


        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            //int rowcnt = (sender as DataGridView).CurrentCell.RowIndex;  
            if ((sender as DataGridView).CurrentRow.Cells[0].Value != null)
            {
                mtxtMbid.Text = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();

                int reCnt = 0;
                cls_Search_DB cds = new cls_Search_DB();
                string Search_Name = "";
                reCnt = cds.Member_Name_Search(mtxtMbid.Text, ref Search_Name);

                if (reCnt == 1)
                {
                    txtName.Text = Search_Name;
                    if (Input_Error_Check(mtxtMbid, "m") == true)
                        Set_Form_Date(mtxtMbid.Text, "m");
                 
                }
            }

        }


        private void dGridView_Base_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            //dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Item.d_Grid_view_Header_Reset();

            //dGridView_Sell_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Cacu.d_Grid_view_Header_Reset();

            //dGridView_Sell_Rece_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Rece.d_Grid_view_Header_Reset();
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[2].Value != null))
            {

                tabC_1.SelectedIndex = 0;

                string T_OrderNumber = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString();
                //string M_Nubmer = (sender as DataGridView).CurrentRow.Cells[3].Value.ToString();

                //Put_OrderNumber_SellDate(T_OrderNumber);           

                cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
                cgbp5.dGridView_Put_baseinfo(this, dGridView_Sell_Item, "item", "", T_OrderNumber);

                cls_Grid_Base_info_Put cgbp6 = new cls_Grid_Base_info_Put();
                cgbp6.dGridView_Put_baseinfo(this, dGridView_Sell_Cacu, "cacu", "", T_OrderNumber);

                cls_Grid_Base_info_Put cgbp7 = new cls_Grid_Base_info_Put();
                cgbp7.dGridView_Put_baseinfo(this, dGridView_Sell_Rece, "rece", "", T_OrderNumber);
            }
           
        }























        private void Set_SalesItemDetail(string Mbid, int Mbid2)
        {
            cls_form_Meth cm = new cls_form_Meth();
            string strSql = "";

            strSql = "Select Isnull(Sum(tbl_SalesitemDetail.ItemCount), 0 )   ";
            strSql = strSql + " , tbl_Goods.Name Item_Name ";          
            strSql = strSql + " From tbl_SalesitemDetail (nolock) ";
            strSql = strSql + " LEFT JOIN tbl_Goods (nolock) ON tbl_Goods.Ncode = tbl_SalesitemDetail.ItemCode ";
            strSql = strSql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_SalesitemDetail.OrderNumber ";
            strSql = strSql + " Where tbl_SalesDetail.Mbid = '" + Mbid.ToString() + "'";
            strSql = strSql + " And   tbl_SalesDetail.Mbid2 = " + Mbid2;
            strSql = strSql + " And   ItemCount > 0 ";
            strSql = strSql + " Group By tbl_Goods.Name ";
            strSql = strSql + " Order By tbl_Goods.Name ASC ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            //Dictionary<string, int> T_SalesitemDetail = new Dictionary<string, int>();
            int ItemCnt = 0; string ItemCode = "";

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                ItemCode = ds.Tables[base_db_name].Rows[fi_cnt]["Item_Name"].ToString();
                ItemCnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt][0].ToString());
                Push_data(series_Item, ItemCode.Replace(" ", "").Substring(0, 5), ItemCnt);
            }


        }





        private void Push_data(Series series, string p, int p_3)
        {
            DataPoint dp = new DataPoint();
            dp.SetValueXY(p, p_3);
            dp.Label = string.Format(cls_app_static_var.str_Currency_Type, p_3); // p_3.ToString(); //p_3.ToString();
            series.Points.Add(dp);
        }

        //Push_data(series_Item, nodeKey.ToString() + "Line", Save_Cnt[nodeKey]);
        private void Save_Nom_Line_Chart()
        {
            cls_form_Meth cm = new cls_form_Meth();
            //series_Item.Name = cm._chang_base_caption_search("상품별");            
            chart_Item.Series.Clear();
            series_Item.Points.Clear();
            series_Item["DrawingStyle"] = "Emboss";
            series_Item["PointWidth"] = "0.5";
            series_Item.Name = cm._chang_base_caption_search("수량");
            series_Item.ChartType = SeriesChartType.Column ;
            series_Item.Legend = "Legend1";
            chart_Item.Series.Add(series_Item);

            chart_Item.ChartAreas[0].AxisX.Interval = 1;
            chart_Item.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_Item.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 7;
        }





        private void Set_SalesDetail_Chart(string Mbid, int Mbid2)
        {
            cls_form_Meth cm = new cls_form_Meth();
            string strSql = "";

            strSql = "Select SellTypeName AS SellCodeName , InputCash,  InputCard , InputPassbook , TotalPrice ";
            strSql = strSql + ", tbl_SalesDetail.recordid , tbl_SalesDetail.Sellcode , InputMile ";            
            strSql = strSql + " From tbl_SalesDetail (nolock) ";
            strSql = strSql + " LEFT JOIN tbl_SellType (nolock) ON tbl_SellType.SellCode = tbl_SalesDetail.SellCode ";            
            strSql = strSql + " Where tbl_SalesDetail.Mbid = '" + Mbid.ToString() + "'";
            strSql = strSql + " And   tbl_SalesDetail.Mbid2 = " + Mbid2;
            strSql = strSql + " And   TotalPV >= 0 ";            
            strSql = strSql + " Order By OrderNumber ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++            
            
            Dictionary<string, double> SelType_1 = new Dictionary<string, double>();

            double Sum_13 = 0; double Sum_14 = 0; double Sum_15 = 0; double Sum_16 = 0;
            double Sell_Cnt_1 = 0; double Sell_Cnt_2 = 0;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
            
                Sum_13 = Sum_13 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCash"].ToString());
                Sum_14 = Sum_14 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCard"].ToString());
                Sum_15 = Sum_15 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputPassbook"].ToString());
                Sum_16 = Sum_16 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputMile"].ToString());

                string T_ver = ds.Tables[base_db_name].Rows[fi_cnt]["SellCodeName"].ToString();
                if (SelType_1.ContainsKey(T_ver) == true)
                {
                    SelType_1[T_ver] = SelType_1[T_ver] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());                 
                }
                else
                {
                    SelType_1[T_ver] = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
                }

                T_ver =  ds.Tables[base_db_name].Rows[fi_cnt]["recordid"].ToString();
                if (T_ver.Contains("WEB") != true)
                {
                    Sell_Cnt_1 = Sell_Cnt_1 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
                }
                else
                {
                    Sell_Cnt_2 = Sell_Cnt_2 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
                }
            }

            Reset_Chart_Total(Sum_13, Sum_14, Sum_15, Sum_16);
            Reset_Chart_Total(ref SelType_1);
            Reset_Chart_Total(Sell_Cnt_1, Sell_Cnt_2);            

        }


        private void Reset_Chart_Total()
        {
            //chart_Mem.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();

            if (cls_app_static_var.Using_Mileage_TF == 1)
            {
                double[] yValues = { 0, 0, 0 , 0  };
                string[] xValues = { cm._chang_base_caption_search("현금"), cm._chang_base_caption_search("카드"), cm._chang_base_caption_search("무통장"), cm._chang_base_caption_search("마일리지") };
                chart_Mem.Series["Series1"].Points.DataBindXY(xValues, yValues);
            }
            else
            {
                double[] yValues = { 0, 0, 0 };
                string[] xValues = { cm._chang_base_caption_search("현금"), cm._chang_base_caption_search("카드"), cm._chang_base_caption_search("무통장") };
                chart_Mem.Series["Series1"].Points.DataBindXY(xValues, yValues);
            }

            chart_Mem.Series["Series1"].ChartType = SeriesChartType.Pie;
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

                chart_Leave.Series["Series1"].ChartType = SeriesChartType.Pie;
                chart_Leave.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                chart_Leave.Legends[0].Enabled = true;
            }



            double[] yValues_3 = { 0, 0 };
            string[] xValues_3 = { cm._chang_base_caption_search("일반"), cm._chang_base_caption_search("WEB") };
            chart_edu.Series["Series1"].Points.DataBindXY(xValues_3, yValues_3);
            chart_edu.Series["Series1"].ChartType = SeriesChartType.Pie;
            chart_edu.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_edu.Legends[0].Enabled = true;

            chart_Item.Series.Clear();
            series_Item.Points.Clear();
        }



        private void Reset_Chart_Total(double SellCnt_1, double SellCnt_2, double SellCnt_3, double SellCnt_4)
        {
            //chart_Mem.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();
            Series series_Save = new Series();

            chart_Mem.Series.Clear();
            chart_Mem.Series.Add(series_Save);

            DataPoint dp = new DataPoint();
            series_Save.ChartType = SeriesChartType.Pie;
            dp.SetValueXY(cm._chang_base_caption_search("현금"), SellCnt_1);
            dp.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_1);
            dp.LabelForeColor = Color.Black;
            dp.LegendText = cm._chang_base_caption_search("현금");
            series_Save.Points.Add(dp);

            DataPoint dp2 = new DataPoint();

            dp2.SetValueXY(cm._chang_base_caption_search("카드"), SellCnt_2);
            dp2.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_2);
            dp2.LabelForeColor = Color.Black;
            dp2.LegendText = cm._chang_base_caption_search("카드");
            series_Save.Points.Add(dp2);


            DataPoint dp3 = new DataPoint();

            dp3.SetValueXY(cm._chang_base_caption_search("무통장"), SellCnt_3);
            dp3.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_3);
            dp3.LabelForeColor = Color.Black;
            dp3.LegendText = cm._chang_base_caption_search("무통장");
            series_Save.Points.Add(dp3);

            if (cls_app_static_var.Using_Mileage_TF == 1)
            {
                DataPoint dp4 = new DataPoint();

                dp4.SetValueXY(cm._chang_base_caption_search("마일리지"), SellCnt_4);
                dp4.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_3);
                dp4.LabelForeColor = Color.Black;
                dp4.LegendText = cm._chang_base_caption_search("마일리지");
                series_Save.Points.Add(dp4);
            }



        }

        private void Reset_Chart_Total(ref Dictionary<string, double> SelType_1)
        {

            cls_form_Meth cm = new cls_form_Meth();
            Series series_Save = new Series();

            chart_Leave.Series.Clear();
            chart_Leave.Series.Add(series_Save);
            int forCnt = 0;
            foreach (string tkey in SelType_1.Keys)
            {
                DataPoint dp = new DataPoint();
                series_Save.ChartType = SeriesChartType.Pie;
                dp.SetValueXY(tkey, SelType_1[tkey]);
                dp.Label = string.Format(cls_app_static_var.str_Currency_Type, SelType_1[tkey]);
                dp.LabelForeColor = Color.Black;
                dp.LegendText = tkey;
                series_Save.Points.Add(dp);
                forCnt++;
            }

            chart_Leave.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_Leave.Legends[0].Enabled = true;
        }


        private void Reset_Chart_Total(double SellCnt_1, double SellCnt_2)
        {
            //chart_edu.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();
            Series series_Save = new Series();

            chart_edu.Series.Clear();
            chart_edu.Series.Add(series_Save);

            DataPoint dp = new DataPoint();
            series_Save.ChartType = SeriesChartType.Pie;
            dp.SetValueXY(cm._chang_base_caption_search("일반"), SellCnt_1);
            dp.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_1);
            dp.LabelForeColor = Color.Black;
            dp.LegendText = cm._chang_base_caption_search("일반");
            series_Save.Points.Add(dp);

            DataPoint dp2 = new DataPoint();

            dp2.SetValueXY(cm._chang_base_caption_search("WEB"), SellCnt_2);
            dp2.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_2);
            dp2.LabelForeColor = Color.Black;
            dp2.LegendText = cm._chang_base_caption_search("WEB");
            series_Save.Points.Add(dp2);


            chart_edu.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_edu.Legends[0].Enabled = true;
        }




        private void tabC_Mem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (idx_Mbid2 <= 0)
                return;

            if (tabC_Mem.SelectedTab.Name  == "tabP_Pay")  //
            {
                if (dGridView_Pay.RowCount == 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                    cgbp.dGridView_Put_baseinfo(this, dGridView_Pay, "pay", mtxtMbid.Text.Trim());
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }

            if (tabC_Mem.SelectedTab.Name == "tab_Down_Save") // tabP_Down_Save
            {
                if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
                    return;

                if (dGridView_Down_S2.RowCount == 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                    cgbp.dGridView_Put_baseinfo(this, dGridView_Down_S2, "savedown", mtxtMbid.Text.Trim());
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }

            if (tabC_Mem.SelectedTab.Name == "tab_SaveDefault") // tabP_Down_Save
            {
                if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
                    return;

                if (dGridView_Down_S2.RowCount == 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                    cgbp.dGridView_Put_baseinfo(this, dGridView_Down_S2, "savedefault", mtxtMbid.Text.Trim());

                    foreach (DataGridViewRow row in dGridView_Down_S2.Rows)
                    {
                        if (row.Cells["직급"].Value.ToString().Equals(""))
                        {

                        }
                    }

                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }

            if (tabC_Mem.SelectedTab.Name  == "tab_Down_Nom") //tabP_Down_Nom
            {
                if (cls_app_static_var.nom_uging_Pr_Flag == 0) //추천인 기능 사용하지 마라.
                    return;

                if (dGridView_Down_N2.RowCount == 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                    cgbp.dGridView_Put_baseinfo(this, dGridView_Down_N2, "nomindown", mtxtMbid.Text.Trim());
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }

            if (tabC_Mem.SelectedTab.Name == "tab_Img")
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Tab_Img_Activate();
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void Tab_Img_Activate()
        {
            cls_form_Meth cfm = new cls_form_Meth();

            if (Input_Error_Check(mtxtMbid, "m") == false) return;  //실제 존재 여부와 회원번호 오류등 체크

            cls_Search_DB csd = new cls_Search_DB();
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

                //StrSql = "select IDCARD_CONFIRM_FLAG from TLS_FILE with (nolock) ";
                //StrSql = StrSql + " where  GUBUN_1 = 'MEMBER' ";
                //StrSql = StrSql + " and GUBUN_2 = 'IDCARD' ";
                //StrSql = StrSql + "    AND REG_ID = (SELECT WEBID FROM TBL_MEMBERINFO WHERE MBID2 = '" + Mbid2.ToString() + "')";

                //StrSql = "SELECT IDCARD_CONFIRM_FLAG FROM tbl_Memberinfo WITH(NOLOCK) WHERE mbid2 = '" + Mbid2.ToString() + "'";

                StrSql = "SELECT A.IDCARD_CONFIRM_FLAG FROM tbl_Memberinfo A WITH(NOLOCK) ";
                StrSql += " LEFT JOIN TLS_FILE B WITH(NOLOCK) ON A.mbid2 = B.ORG_SEQ AND B.GUBUN_1 = 'MEMBER' AND B.GUBUN_2 = 'IDCARD' ";
                //StrSql += " WHERE A.mbid2 = '" + Mbid2.ToString() + "' AND B.REG_ID = (SELECT WEBID FROM tbl_Memberinfo WITH(NOLOCK) WHERE mbid2 = '" + Mbid2.ToString() + "') ";
                StrSql += " WHERE A.mbid2 = '" + Mbid2.ToString() + "' AND B.ORG_SEQ = '" + Mbid2.ToString() + "' ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Memberinfo", ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;
                //++++++++++++++++++++++++++++++++          
                if (ReCnt != 0)
                {
                    //ds.Tables["tbl_Memberinfo"].Rows[0][1].ToString()
                    string sResult = ds.Tables["tbl_Memberinfo"].Rows[0][0].ToString();
                    if (sResult == "0")
                    {
                        chk_Web_img_N.Checked = true;
                    }
                    else if (sResult == "1")
                    {
                        chk_Web_img_Y.Checked = true;
                    }
                }

                ds.Dispose();
                ds = new DataSet();

                //StrSql = "select BANKBOOK_CONFIRM_FLAG from TLS_FILE with (nolock) ";
                //StrSql = StrSql + " where  GUBUN_1 = 'MEMBER' ";
                //StrSql = StrSql + " and GUBUN_2 = 'BANKBOOK' ";
                //StrSql = StrSql + "    AND REG_ID = (SELECT WEBID FROM TBL_MEMBERINFO WHERE MBID2 = " + Mbid2.ToString() + ")";

                //StrSql = "SELECT BANKBOOK_CONFIRM_FLAG FROM tbl_Memberinfo WITH(NOLOCK) WHERE mbid2 = '" + Mbid2.ToString() + "'";

                StrSql = "SELECT A.BANKBOOK_CONFIRM_FLAG FROM tbl_Memberinfo A WITH(NOLOCK) ";
                StrSql += " LEFT JOIN TLS_FILE B WITH(NOLOCK) ON A.mbid2 = B.ORG_SEQ AND B.GUBUN_1 = 'MEMBER' AND B.GUBUN_2 = 'BANKBOOK' ";
                //StrSql += " WHERE A.mbid2 = '" + Mbid2.ToString() + "' AND B.REG_ID = (SELECT WEBID FROM tbl_Memberinfo WITH(NOLOCK) WHERE mbid2 = '" + Mbid2.ToString() + "') ";
                StrSql += " WHERE A.mbid2 = '" + Mbid2.ToString() + "' AND B.ORG_SEQ = '" + Mbid2.ToString() + "' ";

                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Memberinfo", ds, this.Name, this.Text) == false) return;
                ReCnt = Temp_Connect.DataSet_ReCount;
                //++++++++++++++++++++++++++++++++          
                if (ReCnt != 0)
                {
                    //ds.Tables["tbl_Memberinfo"].Rows[0][1].ToString()
                    string sResult = ds.Tables["tbl_Memberinfo"].Rows[0][0].ToString();
                    if (sResult == "0")
                    {
                        chk_Web_book_N.Checked = true;
                    }
                    else if (sResult == "1")
                    {
                        chk_Web_book_Y.Checked = true;
                    }
                }

                tran.Commit();
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

            if (Input_Error_Check(mtxtMbid, "m") == false) return ; //회원번호 관련 관련 오류 체크
            
            cls_Check_Text T_R = new cls_Check_Text();
            string me = "";

            me = T_R.Text_Null_Check(txtName, "Msg_Sort_M_Name"); //성명을 필히 넣어야 합니다.
            if (me != "")
            {
                MessageBox.Show(me);
                return ;
            }

            me = T_R.Text_Null_Check(txtTalk, "Msg_Sort_Talk"); //상담내역을 필히 넣어야 합니다.
            if (me != "")
            {
                MessageBox.Show(me);
                txtTalk.Focus();
                return ;
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
                    StrSql = StrSql + " Where Seq = " + txtSeq.Text ;

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
                tran.Dispose();
                Temp_Connect.Close_DB();

                cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
                cgbp5.dGridView_Put_baseinfo(this, dGridView_Talk, "talk", mtxtMbid.Text);
            }

        }

        private void dGridView_Talk_DoubleClick(object sender, EventArgs e)
        {
            txtTalk.Text = ""; txtSeq.Text = "";
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[3].Value != null))
            {
                string TalkContent = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                string Seq = (sender as DataGridView).CurrentRow.Cells[3].Value.ToString();

                txtTalk.Text = TalkContent; txtSeq.Text = Seq;                 
            }
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
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Account authentication error.");
                }
                else
                {

                    MessageBox.Show("계좌인증 오류");
                }
                //MessageBox.Show(ee.ToString ());
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;

            if (successYN == "Y")
            {
                txtAccount_Reg.Text = txtAccount.Text;
                lbl_ACC.Text = "Success";
                if (cls_User.gid_CountryCode == "TH")
                {
                    me = " This is the correct account information. Account verification successful.  ";
                }
                else
                {
                    me = "올바른 계좌 정보 입니다. 계좌인증 성공.";
                }
                MessageBox.Show(me);
                txtName_E_1.Focus();
            }
            else
            {
                txtAccount_Reg.Text = "";
                lbl_ACC.Text = "Fail";
                lbl_ACC.Text = "Success";
                if (cls_User.gid_CountryCode == "TH")
                {
                    me = " Invalid account information. Please check and try again. Account verification failed.  ";
                }
                else
                {
                    me = "올바른 계좌 정보가 아닙니다. 확인후 다시 시도해 주십시요. 계좌인증 실패.";
                }
       
                MessageBox.Show(me);
                txtAccount.Focus();
            }



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

        private void combo_Se_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            combo_Se_Code_2.SelectedIndex = combo_Se_2.SelectedIndex;

            // 태국버전 인 경우
            if (combo_Se_Code_2.Text == "TH")
            {
                pnlDistrict_TH.Visible = true;
                pnlProvince_TH.Visible = true;
                pnlSubDistrict_TH.Visible = true;
                pnlZipCode_TH.Visible = true;
                pnlZipCode_KR.Visible = false;
                cbSubDistrict_TH_SelectedIndexChanged(this, null);
            }
            // 태국 이외 버전 인 경우
            else
            {
                pnlDistrict_TH.Visible = false;
                pnlProvince_TH.Visible = false;
                pnlSubDistrict_TH.Visible = false;
                pnlZipCode_TH.Visible = false;
                pnlZipCode_KR.Visible = true;
                txtAddress2.ReadOnly = false;
                txtAddress2.Clear();
            }
        }

        private void combo_Se_Code_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            combo_Se_2.SelectedIndex = combo_Se_Code_2.SelectedIndex;

            // 태국버전 인 경우
            if (combo_Se_Code_2.Text == "TH")
            {
                pnlDistrict_TH.Visible = true;
                pnlProvince_TH.Visible = true;
                pnlSubDistrict_TH.Visible = true;
                pnlZipCode_TH.Visible = true;
                pnlZipCode_KR.Visible = false;
                cbSubDistrict_TH_SelectedIndexChanged(this, null);
            }
            // 태국 이외 버전 인 경우
            else
            {
                pnlDistrict_TH.Visible = false;
                pnlProvince_TH.Visible = false;
                pnlSubDistrict_TH.Visible = false;
                pnlZipCode_TH.Visible = false;
                pnlZipCode_KR.Visible = true;
                txtAddress2.ReadOnly = false;
                txtAddress2.Clear();
            }
        }

        private void button_Web_img_Click(object sender, EventArgs e)
        {
            frmBase_Member_Web_IMg e_f = new frmBase_Member_Web_IMg();
            e_f.Call_searchNumber_Info += new frmBase_Member_Web_IMg.Call_searchNumber_Info_Dele(e_f_Send_Mem_Mbid_Info);

            e_f.ShowDialog();

            SendKeys.Send("{TAB}");
        }
        void e_f_Send_Mem_Mbid_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = idx_Mbid; searchMbid2 = idx_Mbid2;
            seachName = ""; // txtName.Text.Trim();
        }

        private void button_Web_book_Click(object sender, EventArgs e)
        {
            frmBase_Member_Web_IMg_2 e_f2 = new frmBase_Member_Web_IMg_2();
            e_f2.Call_searchNumber_Info += new frmBase_Member_Web_IMg_2.Call_searchNumber_Info_Dele(e_f_Send_Mem_Mbid_Info_2);

            e_f2.ShowDialog();

            SendKeys.Send("{TAB}");
        }
        void e_f_Send_Mem_Mbid_Info_2(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = idx_Mbid; searchMbid2 = idx_Mbid2;
            seachName = "2"; // txtName.Text.Trim();
        }

        private void button_Web_img_Del_Click(object sender, EventArgs e)
        {
            cls_form_Meth cfm = new cls_form_Meth();

            if (Input_Error_Check(mtxtMbid, "m") == false) return;  //실제 존재 여부와 회원번호 오류등 체크

            cls_Search_DB csd = new cls_Search_DB();
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

                StrSql = "select UPLOAD_PATH + UPLOAD_FILE_NM  as T_FileDir from TLS_FILE with (nolock) ";
                StrSql = StrSql + " where  GUBUN_1 = 'MEMBER' ";
                StrSql = StrSql + " and GUBUN_2 = 'IDCARD' ";

                //StrSql = StrSql + "    AND REG_ID = (SELECT WEBID FROM TBL_MEMBERINFO WHERE MBID2 = " + Mbid2.ToString() + ")";
                StrSql = StrSql + "    AND ORG_SEQ = (SELECT mbid2 FROM TBL_MEMBERINFO WHERE MBID2 = " + Mbid2.ToString() + ")";
                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Memberinfo", ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("The uploaded image does not exist.");
                    }
                    else
                    {

                        MessageBox.Show("업로드 된 이미지가 존재하지 않습니다.");
                    }
                    return;
                }
                else
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        DialogResult msg = MessageBox.Show("Are you sure you want to delete the uploaded image?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (msg == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else
                    {

                        DialogResult msg = MessageBox.Show("업로드 된 이미지를 삭제하시겠습니까?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (msg == DialogResult.No)
                        {
                            return;
                        }
                    }
                  
                }
                //++++++++++++++++++++++++++++++++                        

                cls_form_Meth cm = new cls_form_Meth();
                cm.from_control_text_base_chang(this);

                StrSql = "EXEC USP_DELETE_TLS_FILE_DATA '" + Mbid2.ToString() + "', 'MEMBER', 'IDCARD', '" + cls_User.gid + "'";

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);

                tran.Commit();
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Deletion is complete.");
                }
                else
                {
                    MessageBox.Show("삭제가 완료되었습니다.");
                }
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

        private void button_Web_book_Del_Click(object sender, EventArgs e)
        {
            cls_form_Meth cfm = new cls_form_Meth();

            if (Input_Error_Check(mtxtMbid, "m") == false) return;  //실제 존재 여부와 회원번호 오류등 체크

            cls_Search_DB csd = new cls_Search_DB();
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

                StrSql = "select UPLOAD_PATH + UPLOAD_FILE_NM  as T_FileDir from TLS_FILE with (nolock) ";
                StrSql = StrSql + " where  GUBUN_1 = 'MEMBER' ";
                StrSql = StrSql + " and GUBUN_2 = 'BANKBOOK' ";

                //StrSql = StrSql + "    AND REG_ID = (SELECT WEBID FROM TBL_MEMBERINFO WHERE MBID2 = " + Mbid2.ToString() + ")";
                StrSql = StrSql + "    AND ORG_SEQ = " + Mbid2.ToString() + " ";
                // StrSql = StrSql + "    AND ORG_SEQ = (SELECT mbid2 FROM TBL_MEMBERINFO WHERE MBID2 = " + Mbid2.ToString() + ")";
                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Memberinfo", ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("The uploaded image does not exist.");
                    }
                    else
                    {
                        MessageBox.Show("업로드 된 이미지가 존재하지 않습니다.");
                    }
                    return;
                }
                else
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        DialogResult msg = MessageBox.Show("Are you sure you want to delete the uploaded image?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (msg == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else
                    {
                        DialogResult msg = MessageBox.Show("업로드 된 이미지를 삭제하시겠습니까?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (msg == DialogResult.No)
                        {
                            return;
                        }
                    }
                }
                //++++++++++++++++++++++++++++++++                        

                cls_form_Meth cm = new cls_form_Meth();
                cm.from_control_text_base_chang(this);

                StrSql = "EXEC USP_DELETE_TLS_FILE_DATA '" + Mbid2.ToString() + "', 'MEMBER', 'BANKBOOK', '" + cls_User.gid + "'";

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);
                tran.Commit();
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Deletion is complete.");
                }
                else
                {
                    MessageBox.Show("삭제가 완료되었습니다.");
                }
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

        private void button_Change_Web_img_Click(object sender, EventArgs e)
        {
            cls_form_Meth cfm = new cls_form_Meth();

            if (Input_Error_Check(mtxtMbid, "m") == false) return;  //실제 존재 여부와 회원번호 오류등 체크

            cls_Search_DB csd = new cls_Search_DB();
            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            string sBeforeFlag = "0";
            string sAfterFlag = (chk_Web_img_N.Checked ? 0 : 1).ToString();

            try
            {
                string StrSql = "";

                //StrSql = "select UPLOAD_PATH + UPLOAD_FILE_NM  as T_FileDir, IDCARD_CONFIRM_FLAG from TLS_FILE with (nolock) ";
                //StrSql = StrSql + " where  GUBUN_1 = 'MEMBER' ";
                //StrSql = StrSql + " and GUBUN_2 = 'IDCARD' ";
                //StrSql = StrSql + "    AND REG_ID = (SELECT WEBID FROM TBL_MEMBERINFO WHERE MBID2 = " + Mbid2.ToString() + ")";

                StrSql = " SELECT B.UPLOAD_PATH + B.UPLOAD_FILE_NM  as T_FileDir, A.IDCARD_CONFIRM_FLAG FROM tbl_Memberinfo A WITH(NOLOCK) ";
                StrSql += " LEFT JOIN TLS_FILE B WITH(NOLOCK) ON A.mbid2 = B.ORG_SEQ AND B.GUBUN_1 = 'MEMBER' AND B.GUBUN_2 = 'IDCARD' ";
                //StrSql += " WHERE A.mbid2 = '" + Mbid2.ToString() + "' AND B.REG_ID = (SELECT WEBID FROM tbl_Memberinfo WITH(NOLOCK) WHERE mbid2 = '" + Mbid2.ToString() + "') ";
                StrSql += " WHERE A.mbid2 = '" + Mbid2.ToString() + "' AND B.ORG_SEQ = '" + Mbid2.ToString() + "' ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Memberinfo", ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("The uploaded image does not exist.");
                    }
                    else
                    {
                        MessageBox.Show("업로드 된 이미지가 존재하지 않습니다.");
                    }
                    return;
                }
                else
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        DialogResult msg = MessageBox.Show("Change uploaded image verification status?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (msg == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else
                    {

                        DialogResult msg = MessageBox.Show("업로드 된 이미지 확인 상태를 변경 하시겠습니까?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (msg == DialogResult.No)
                        {
                            return;
                        }
                    }
                    sBeforeFlag = ds.Tables["tbl_Memberinfo"].Rows[0][1].ToString();
                }
                //++++++++++++++++++++++++++++++++                        

                cls_form_Meth cm = new cls_form_Meth();
                cm.from_control_text_base_chang(this);

                if (chk_Web_img_N.Checked == false && chk_Web_img_Y.Checked == false)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please select Unconfirmed / Confirmed status.");
                    }
                    else
                    {
                        MessageBox.Show("미확인 / 확인완료 중 상태를 선택하여 주십시오.");
                    }
                    chk_Web_img_N.Focus();
                    return;
                }

                //StrSql = "UPDATE TLS_FILE SET IDCARD_CONFIRM_FLAG = " + sAfterFlag + " WHERE GUBUN_1 = 'MEMBER' AND GUBUN_2 = 'IDCARD' AND REG_ID = (SELECT WEBID FROM TBL_MEMBERINFO WHERE MBID2 = '" + Mbid2.ToString() + "')";
                StrSql = " UPDATE tbl_Memberinfo SET IDCARD_CONFIRM_FLAG = " + sAfterFlag;
                StrSql += " FROM tbl_Memberinfo A LEFT JOIN TLS_FILE B ON A.mbid2 = B.ORG_SEQ AND B.GUBUN_1 = 'MEMBER' AND B.GUBUN_2 = 'IDCARD' ";
                //StrSql += " WHERE A.mbid2 = '" + Mbid2.ToString() + "' AND REG_ID = (SELECT WEBID FROM TBL_MEMBERINFO WHERE MBID2 = '" + Mbid2.ToString() + "') ";
                StrSql += " WHERE A.mbid2 = '" + Mbid2.ToString() + "' AND ORG_SEQ = '" + Mbid2.ToString() + "' ";
                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);
                
                // 변경 전 값과 변경 후 값이 틀린 경우 LOG 기록
                if (sBeforeFlag != (chk_Web_img_N.Checked ? 0 : 1).ToString())
                {
                    StrSql = string.Format("INSERT INTO tbl_Memberinfo_Mod(Mbid, Mbid2, ChangeDetail, BeforeDetail, AfterDetail, ModRecordid, ModRecordtime) VALUES " +
                        "('{0}', '{1}', 'Foreign_IDCARD_CONFIRM', '{2}', '{3}', '{4}', CONVERT(VARCHAR(25), GETDATE(), 21))", Mbid.ToString(), Mbid2.ToString(), sBeforeFlag, sAfterFlag, cls_User.gid);
                    Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_Mod", Conn, tran, this.Name, this.Text);
                }

                tran.Commit();
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Image verification status change is complete.");
                }
                else
                {
                    MessageBox.Show("이미지 확인 상태 변경이 완료되었습니다.");
                }
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

        private void button_Change_Web_book_Click(object sender, EventArgs e)
        {
            cls_form_Meth cfm = new cls_form_Meth();

            if (Input_Error_Check(mtxtMbid, "m") == false) return;  //실제 존재 여부와 회원번호 오류등 체크

            cls_Search_DB csd = new cls_Search_DB();
            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            string sBeforeFlag = "0";
            string sAfterFlag = (chk_Web_book_N.Checked ? 0 : 1).ToString();

            try
            {
                string StrSql = "";

                //StrSql = "select UPLOAD_PATH + UPLOAD_FILE_NM  as T_FileDir, BANKBOOK_CONFIRM_FLAG from TLS_FILE with (nolock) ";
                //StrSql = StrSql + " where  GUBUN_1 = 'MEMBER' ";
                //StrSql = StrSql + " and GUBUN_2 = 'BANKBOOK' ";
                //StrSql = StrSql + "    AND REG_ID = (SELECT WEBID FROM TBL_MEMBERINFO WHERE MBID2 = " + Mbid2.ToString() + ")";

                StrSql = " SELECT B.UPLOAD_PATH + B.UPLOAD_FILE_NM  as T_FileDir, A.BANKBOOK_CONFIRM_FLAG FROM tbl_Memberinfo A WITH(NOLOCK) ";
                StrSql += " LEFT JOIN TLS_FILE B WITH(NOLOCK) ON A.mbid2 = B.ORG_SEQ AND B.GUBUN_1 = 'MEMBER' AND B.GUBUN_2 = 'BANKBOOK' ";
                //StrSql += " WHERE A.mbid2 = '" + Mbid2.ToString() + "' AND B.REG_ID = (SELECT WEBID FROM tbl_Memberinfo WITH(NOLOCK) WHERE mbid2 = '" + Mbid2.ToString() + "') ";
                StrSql += " WHERE A.mbid2 = '" + Mbid2.ToString() + "' AND B.ORG_SEQ = '" + Mbid2.ToString() + "' ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Memberinfo", ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("The uploaded image does not exist.");
                    }
                    else
                    {
                        MessageBox.Show("업로드 된 이미지가 존재하지 않습니다.");
                    }
                    return;
                }
                else
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        DialogResult msg = MessageBox.Show("Do you want to change the uploaded image verification status?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (msg == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else
                    {
                        DialogResult msg = MessageBox.Show("업로드 된 이미지 확인 상태를 변경 하시겠습니까?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (msg == DialogResult.No)
                        {
                            return;
                        }
                    }
                    sBeforeFlag = ds.Tables["tbl_Memberinfo"].Rows[0][1].ToString();
                }
                //++++++++++++++++++++++++++++++++                        

                cls_form_Meth cm = new cls_form_Meth();
                cm.from_control_text_base_chang(this);

                if (chk_Web_book_N.Checked == false && chk_Web_book_Y.Checked == false)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please select a status of Unconfirmed / Confirmed.");
                    }
                    else
                    {
                        MessageBox.Show("미확인 / 확인완료 중 상태를 선택하여 주십시오.");
                    }
                    chk_Web_book_N.Focus();
                    return;
                }

                //StrSql = "UPDATE TLS_FILE SET BANKBOOK_CONFIRM_FLAG = " + sAfterFlag + " WHERE GUBUN_1 = 'MEMBER' AND GUBUN_2 = 'BANKBOOK' AND REG_ID = (SELECT WEBID FROM TBL_MEMBERINFO WHERE MBID2 = '" + Mbid2.ToString() + "')";
                StrSql = " UPDATE tbl_Memberinfo SET BANKBOOK_CONFIRM_FLAG = " + sAfterFlag;
                StrSql += " FROM tbl_Memberinfo A LEFT JOIN TLS_FILE B ON A.mbid2 = B.ORG_SEQ AND B.GUBUN_1 = 'MEMBER' AND B.GUBUN_2 = 'BANKBOOK' ";
                //StrSql += " WHERE A.mbid2 = '" + Mbid2.ToString() + "' AND REG_ID = (SELECT WEBID FROM TBL_MEMBERINFO WHERE MBID2 = '" + Mbid2.ToString() + "') ";
                StrSql += " WHERE A.mbid2 = '" + Mbid2.ToString() + "' AND ORG_SEQ = '" + Mbid2.ToString() + "' ";

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);

                if (sBeforeFlag != (chk_Web_book_N.Checked ? 0 : 1).ToString())
                {
                    StrSql = string.Format("INSERT INTO tbl_Memberinfo_Mod(Mbid, Mbid2, ChangeDetail, BeforeDetail, AfterDetail, ModRecordid, ModRecordtime) VALUES " +
                        "('{0}', '{1}', 'Foreign_BANKBOOK_CONFIRM', '{2}', '{3}', '{4}', CONVERT(VARCHAR(25), GETDATE(), 21))", Mbid.ToString(), Mbid2.ToString(), sBeforeFlag, sAfterFlag, cls_User.gid);
                    Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_Mod", Conn, tran, this.Name, this.Text);
                }

                tran.Commit();
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Image verification status change is complete.");
                }
                else
                {
                    MessageBox.Show("이미지 확인 상태 변경이 완료되었습니다.");
                }
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

        /// <summary> 비밀번호 초기화를 원할시 ever + 생년월일로 비밀번호를 설정해줍시다. </summary>
        private void btnWebPasswordDefault_Click(object sender, EventArgs e)
        {
            if (idx_Mbid2 <= 0 || idx_Mbid2 == null)
                return; 
            if (mtxtBrithDay.Text.Replace("-", "").Length != 10)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Please confirm your date of birth.");
                }
                else
                {
                    MessageBox.Show("생년월일을 확인해주십시오.");
                }
                mtxtBrithDay.Focus();
                return;
            }

            txtPassword.Text = "ever" + mtxtBrithDay.Text.Replace("-", "").Trim().Substring(2, 6);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Update Tbl_Memberinfo");
            sb.AppendLine("SET WebPassword = '"+ EncryptSHA256_EUCKR(txtPassword.Text.Trim()) + "'");
            sb.AppendLine("WHERE mbid2 = '" + idx_Mbid2 + "'");
            
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();
            
            Temp_Connect.Update_Data(sb.ToString(), this.Name, this.Text);
            if (cls_User.gid_CountryCode == "TH")
            {
                MessageBox.Show("Password has been successfully changed.");
            }
            else
            {
                MessageBox.Show("비밀번호가 정상적으로 변경 되었습니다.");
            }
        }

        private void cbProvince_TH_SelectedIndexChanged(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("SELECT ZIPCODE_NM FROM dbo.ufn_Get_ZipCode_City_TH('" + cbProvince_TH.Text + "') ORDER BY ZIPCODE_SORT ");
            sb.AppendLine("SELECT ZIPCODE_NM FROM ufn_Get_ZipCode_District_TH('" + cbProvince_TH.Text + "') ORDER BY MinSubDistrictID ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            cbDistrict_TH.DataBindings.Clear();
            cbDistrict_TH.DataSource = ds.Tables["ZipCode_NM"];
            cbDistrict_TH.DisplayMember = "ZipCode_NM";
            cbDistrict_TH.Font = new Font("Tahoma", 11f);
        }

        private void cbDistrict_TH_SelectedIndexChanged(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("SELECT * FROM dbo.ufn_Get_ZipCode_TH('" + cbDistrict_TH.Text + "') ");
            sb.AppendLine("SELECT ZIPCODE_NM FROM dbo.ufn_Get_ZipCode_SubDistrict_TH('" + cbDistrict_TH.Text + "') ORDER BY MinSubDistrictID ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            cbSubDistrict_TH.DataBindings.Clear();
            cbSubDistrict_TH.DataSource = ds.Tables["ZipCode_NM"];
            cbSubDistrict_TH.DisplayMember = "ZipCode_NM";
            cbSubDistrict_TH.Font = new Font("Tahoma", 11f);
        }

        private void cbSubDistrict_TH_SelectedIndexChanged(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT [ZIPCODE_NM] = PostCode FROM TLS_ZIPCODE_CS WITH(NOLOCK) WHERE SubDistrictThaiShort = '" + cbSubDistrict_TH.Text + "' ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            if (Temp_conn.DataSet_ReCount <= 0) return;

            txtZipCode_TH.Text = "";
            txtZipCode_TH.Text = ds.Tables["ZipCode_NM"].Rows[0][0].ToString();
            txtZipCode_TH.Font = new Font("Tahoma", 11f);

            txtAddress2.Text = cbSubDistrict_TH.Text + " " + cbDistrict_TH.Text + " " + cbProvince_TH.Text;
        }

        private void button_Web_img_Upload_Click(object sender, EventArgs e)
        {
            UploadFileData(EWebImageType.IDCARD);
        }

        private void button_Web_book_Upload_Click(object sender, EventArgs e)
        {
            UploadFileData(EWebImageType.BANKBOOK);
        }

        private void UploadFileData(EWebImageType inputImgType)
        {
            using (OpenFileDialog OFD = new OpenFileDialog())
            {
                OFD.Filter = "Image Files (*.png, *.jpg, *.jpeg, *.bmp)|*.png;*.jpg;*.jpeg;*.bmp";
                OFD.FilterIndex = 1;
                OFD.RestoreDirectory = true;

                if (OFD.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    string sFilePath = OFD.FileName;                            // 파일경로
                    string sFileName = Path.GetFileName(OFD.FileName);          // 파일명
                    string sFileExtension = Path.GetExtension(OFD.FileName);    // 파일 확장자
                    long fileSizeInBytes = new FileInfo(OFD.FileName).Length;   // 파일 byte size

#if DEBUG
                    string t_url = "https://uat.mannatech.co.th/common/cs/uploadFile.do";    // uat 버전. 
#else
                    string t_url = "https://www.mannatech.co.th/common/cs/uploadFile.do" + T_FileDir;    // live 버전. 
                    //string t_url = "https://www.mannatech.co.th/uImage" + T_FileDir;    // live 버전. 
#endif

                    // WEB단에서 parameter로 넘겨줄 예정.
                    //string t_url2 = string.Format("/member/{0}/", DateTime.Now.ToString("yyyyMMdd"));
                    //string t_url3 = string.Format("{0}_{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), Guid.NewGuid().ToString(), sFileExtension);
                    //string sCompleteURL = t_url + t_url2 + t_url3;

                    string result = string.Empty; // 전송 후 결과값
                    // WEB으로 파일 데이터 전송, 전송 후 저장 정보 return 받음.
                    result = RequestHelper.PostMultipart(t_url, new Dictionary<string, object>()
                        {
                            {
                                //"uploadFile0", new FormFile()
                                sFileName, new FormFile()
                                {
                                    Name = sFileName, // 보내지는 파일명
                                    ContentType = "application/pdf",  // 파일 타입
                                    FilePath = sFilePath  // 로컬파일경로
                                }
                            }
                        }
                    );

                    // Web Return value list
                    string SuccessYN = "";
                    string t_url2 = "";
                    string t_url3 = "";
                    int sortNo = 0;
                    string sReturnFileName = "";
                    int sReturnfileSize = 0;

                    JObject ReturnData = new JObject();
                    string sfileList = "";
                    JArray jArrayList = new JArray();

                    try
                    {
                        ReturnData = JObject.Parse(result);
                        sfileList = ReturnData["fileList"].ToString();
                        jArrayList = JArray.Parse(sfileList);

                        // 중첩된 JSON 데이터 추출
                        foreach (JObject objectItem in jArrayList)
                        {
                            t_url2 = objectItem["uploadPath"].ToString();
                            t_url3 = objectItem["uploadFileNm"].ToString();

                            sortNo = int.Parse(objectItem["sortNo"].ToString());
                            sReturnFileName = objectItem["orgFileNm"].ToString();
                            sReturnfileSize = int.Parse(objectItem["fileSize"].ToString());
                        }
                    }
                    catch (Exception jsonEx)
                    {
                        throw new Exception(jsonEx.Message, jsonEx);
                    }

                    SuccessYN = ReturnData["successYN"].ToString();

                    if (SuccessYN != "Y")
                    {
                        MessageBox.Show("file not uploaded. Please Contact the program production company.");
                        return;
                    }


                    // DB [TLS_FILE] table record INSERT.
                    cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                    Temp_Connect.Connect_DB();
                    //int Com_TF = 0;
                    SqlConnection Conn = Temp_Connect.Conn_Conn();
                    SqlTransaction tran = Conn.BeginTransaction();

                    try
                    {
                        string StrSql = "";

                        StrSql = "INSERT INTO TLS_FILE (";
                        StrSql = StrSql + " ORG_SEQ";
                        StrSql = StrSql + " , GUBUN_1";
                        StrSql = StrSql + " , GUBUN_2";
                        StrSql = StrSql + " , ORG_FILE_NM";
                        StrSql = StrSql + " , UPLOAD_PATH";
                        StrSql = StrSql + " , UPLOAD_FILE_NM";
                        StrSql = StrSql + " , THUM_LIST_FILE_NM";
                        StrSql = StrSql + " , THUM_VIEW_FILE_NM";
                        StrSql = StrSql + " , FILE_SIZE";
                        StrSql = StrSql + " , SORT_NO";
                        StrSql = StrSql + " , REG_ID";
                        StrSql = StrSql + " , REG_DATE";
                        StrSql = StrSql + " , MOD_ID";
                        StrSql = StrSql + " , MOD_DATE";

                        StrSql = StrSql + " ) values(";

                        StrSql = StrSql + mtxtMbid.Text;                            // ORG_SEQ
                        StrSql = StrSql + ",'MEMBER'";                              // GUBUN_1
                        //StrSql = StrSql + ",'IDCARD'";                              // GUBUN_2
                        StrSql = StrSql + ",'" + inputImgType.ToString() + "'";      // GUBUN_2
                        StrSql = StrSql + ",'" + sFileName + "'";                   // ORG_FILE_NM, 파일명
                        StrSql = StrSql + ",'" + t_url2 + "'";                      // UPLOAD_PATH
                        StrSql = StrSql + ",'" + t_url3 + "'";                      // UPLOAD_FILE_NM
                        StrSql = StrSql + ",''";                                    // THUM_LIST_FILE_NM
                        StrSql = StrSql + ",''";                                    // THUM_VIEW_FILE_NM
                        StrSql = StrSql + "," + sReturnfileSize;                    // FILE_SIZE
                        StrSql = StrSql + "," + sortNo;                             // SORT_NO
                        StrSql = StrSql + ",'" + cls_User.gid + "'";                // REG_ID
                        StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";    // REG_DATE
                        StrSql = StrSql + ", NULL";                                 // MOD_ID
                        StrSql = StrSql + ", NULL";                                 // MOD_DATE
                        StrSql = StrSql + " ) ";

                        if (Temp_Connect.Insert_Data(StrSql, "TLS_FILE", Conn, tran, this.Name.ToString(), this.Text) == false) return;

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception(ex.Message, ex);
                    }
                    finally
                    {
                        tran.Dispose();
                        Temp_Connect.Close_DB();
                    }


                    if (cls_User.gid_CountryCode == "KR")
                    {
                        MessageBox.Show("이미지 업로드를 완료하였습니다.");
                    }
                    else
                    {
                        MessageBox.Show("Image uploaded successfully!");
                    }

                }
                catch (WebException wex)
                {
                    Console.WriteLine(wex.Message);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("File upload failed.. [" + ex.Message + "]");
                    Console.WriteLine("File upload failed.. [" + ex.Message + "]");
                }
            }
        }

















    }
}
