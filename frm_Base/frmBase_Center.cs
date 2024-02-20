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
    public partial class frmBase_Center : Form
    {
        cls_Grid_Base cgb = new cls_Grid_Base();
        private const string base_db_name = "tbl_Business";
        private int Data_Set_Form_TF;

        public frmBase_Center()
        {
            InitializeComponent();

        }

        private void frmBase_From_Load(object sender, EventArgs e)
        {

           



            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            if (tab_Nation.Visible == true)
            {
                cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
                cpbf.Put_NaCode_ComboBox(combo_Se, combo_Se_Code);
            }
            Base_Grid_Set();

            txtNcode.MaxLength = cls_app_static_var.Center_Code_Length;


            Data_Set_Form_TF = 0; 
            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;

            mtxtTel1.Mask = cls_app_static_var.Tel_Number_Fromat;
            mtxtTel2.Mask = cls_app_static_var.Tel_Number_Fromat;
            mtxtZip1.Mask = cls_app_static_var.ZipCode_Number_Fromat;
            mtxtBiz1.Mask = cls_app_static_var.Biz_Number_Fromat;
        }


        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();
        }
        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Save.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_Excel.Left = butt_Save.Left + butt_Save.Width + 2;
            butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Save);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);
            cfm.button_flat_change(butt_AddCode);
            
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



        private void Base_Grid_Set()
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;

    //string[] g_HeaderText = { "센타 코드" , "센타명" ,"회원 번호" , "회원명"   , "우편 번호"  
    //                , "주소1"   , "주소2"   ,"전화 번호" , "팩스 번호" , "사업자 번호"
    //                , "대표자" , "업태" ,"종목" , "비고"

        

            cls_form_Meth cm = new cls_form_Meth() ;

            Tsql = "Select Ncode, Name  ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) ";
            else
                Tsql = Tsql + ",tbl_Memberinfo.mbid2 ";
            
            Tsql = Tsql + " ,Isnull(M_Name,'') Mem_Name ";
            Tsql = Tsql + " ,ZipCode ";
            Tsql = Tsql + " ,add1 , add2 ";
            Tsql = Tsql + " ,phone ";
            Tsql = Tsql + " ,FaxTel ";
            Tsql = Tsql + " ,Biz_Num ";
            Tsql = Tsql + " ,bossname ";
            Tsql = Tsql + " ,conditions ";
            Tsql = Tsql + " ,item ";
            Tsql = Tsql + " ,tbl_Business.Remarks ";
            Tsql = Tsql + ", Case When U_TF = 0 Then '" + cm._chang_base_caption_search("사용") + "' ELSE  '" + cm._chang_base_caption_search("미사용") + "' END    ";
            Tsql = Tsql + ", Isnull(nationNameEng,'') nationNameEng , tbl_Business.Na_code ";
            //Tsql = Tsql + " From tbl_ClosePay_01_Mod (nolock) ";
            //Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_ClosePay_01_Mod.Mbid = tbl_Business.Mbid And tbl_ClosePay_01_Mod.Mbid2 = tbl_Business.Mbid2 ";
            Tsql = Tsql + " From tbl_Business (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_Business.Mbid And tbl_Memberinfo.Mbid2 = tbl_Business.Mbid2 " ;

            Tsql = Tsql + " LEFT JOIN  tbl_Nation  (nolock) ON tbl_Nation.nationCode = tbl_Business.Na_Code  ";
            Tsql = Tsql + " Where tbl_Business.Na_Code in (Select Na_Code From ufn_User_In_Na_Code('" + cls_User.gid_CountryCode + "') )";
            //if (tab_Nation.Visible == true)
            //{
            //    if (combo_Se_Code.Text != "")
            //    {
            //        Tsql = Tsql + " Where tbl_Business.Na_Code = '" + combo_Se_Code.Text + "'";
            //    }
            //}


            Tsql = Tsql + " Order by Ncode ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, string[]> gr_dic_text = new Dictionary<int, string[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb.grid_name = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Data_Put();
        }



        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 17;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            string[] g_HeaderText = { "센타_코드" , "센타명" ,"회원_번호" , "회원명"   , "우편_번호"  
                                     , "주소1"   , "주소2"   ,"전화_번호" , "팩스_번호" , "사업자_번호"
                                     , "대표자" , "업태" ,"종목" , "비고" ,  "사용_여부"
                                     ,"소속국가", ""
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 200, 100, 100, 80  
                             ,400, 500, 130, 130, 130  
                             ,100, 100, 100, 400, 80
                             ,cls_app_static_var.Using_Multi_language , 0  
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true,  true
                                    ,  true,  true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter                                
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //10

                               ,DataGridViewContentAlignment.MiddleCenter                                
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //15

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                              };
            cgb.grid_col_alignment = g_Alignment;
        }

        
        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, string[]> gr_dic_text, int fi_cnt)
        {
            string[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0].ToString()  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1].ToString()  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2].ToString()  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3].ToString()  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4].ToString() 
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][5].ToString()                                  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6].ToString() 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][7].ToString() 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][8].ToString() 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9].ToString() 

                                ,ds.Tables[base_db_name].Rows[fi_cnt][10].ToString() 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][11].ToString() 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][12].ToString() 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][13].ToString() 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][14].ToString() 

                                ,ds.Tables[base_db_name].Rows[fi_cnt][15].ToString() 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][16].ToString() 
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }
        


        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, txtNcode);

            if ((sender as DataGridView).CurrentRow.Cells[0].Value != null)
            {
                Data_Set_Form_TF = 1;
                DataGridView T_Gd = (DataGridView)sender;
                string t_ncode = T_Gd.CurrentRow.Cells[0].Value.ToString();
                string Na_Ccode = T_Gd.CurrentRow.Cells[16].Value.ToString();
                Form_Refresh_Data(t_ncode, Na_Ccode);
                Data_Set_Form_TF = 0;
            }
        }
        

        private void Form_Refresh_Data(string ncode, string na_code = "")
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;

            Tsql = "Select Ncode, Name  ";                        
            Tsql = Tsql + " ,tbl_Memberinfo.mbid ,tbl_Memberinfo.mbid2 ";
            Tsql = Tsql + " ,Isnull(M_Name,'') Mem_Name ";
            Tsql = Tsql + " ,ZipCode";
            Tsql = Tsql + " ,add1 , add2 ";
            Tsql = Tsql + " ,phone ";
            Tsql = Tsql + " ,FaxTel ";
            Tsql = Tsql + " ,Biz_Num ";
            Tsql = Tsql + " ,bossname ";
            Tsql = Tsql + " ,conditions ";
            Tsql = Tsql + " ,item ";
            Tsql = Tsql + " ,tbl_Business.Remarks ";
            Tsql = Tsql + " ,U_TF ";
            Tsql = Tsql + ", Isnull(nationNameEng,'') nationNameEng , tbl_Business.Na_code ";

            Tsql = Tsql + " ,Map ";
            Tsql = Tsql + " ,Web ";
            Tsql = Tsql + " ,CenterBonus ";
            Tsql = Tsql + " ,ShowMemberCenter ";
            Tsql = Tsql + " ,ShowOrderCenter ";

            Tsql = Tsql + " From tbl_Business (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_Business.Mbid And tbl_Memberinfo.Mbid2 = tbl_Business.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN  tbl_Nation  (nolock) ON tbl_Nation.nationCode = tbl_Business.Na_Code  ";

            Tsql = Tsql + " Where tbl_Business.ncode = '" + ncode + "'";            
            Tsql = Tsql + " And  tbl_Business.Na_Code = '" + cls_User.gid_CountryCode + "'";                
            

            Tsql = Tsql + " Order by Ncode ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            TxtMap.Text = ds.Tables[base_db_name].Rows[0]["Map"].ToString();
            //웹보여주기 체크
            if (ds.Tables[base_db_name].Rows[0]["Web"].ToString() == "Y")
                ChkWeb.Checked = true;
            else
                ChkWeb.Checked = false;
            //센터보너스지급체크
            if (ds.Tables[base_db_name].Rows[0]["CenterBonus"].ToString() == "Y")
                ChkCenterBonus.Checked = true;
            else
                ChkCenterBonus.Checked = false;
            //회원관련 센터보여주기 체크
            if (ds.Tables[base_db_name].Rows[0]["ShowMemberCenter"].ToString() == "Y")
                CkviewMemberCenter.Checked = true;
            else
                CkviewMemberCenter.Checked = false;
            //주문관련 센터보여주기체크 
            if (ds.Tables[base_db_name].Rows[0]["ShowOrderCenter"].ToString() == "Y")
                CkViewOrderCenter.Checked = true;
            else
                CkViewOrderCenter.Checked = false;



            txtKey.Text = ds.Tables[base_db_name].Rows[0]["Ncode"].ToString();
            txtNcode.Text = ds.Tables[base_db_name].Rows[0]["Ncode"].ToString();
            txtName.Text = ds.Tables[base_db_name].Rows[0]["name"].ToString();

            if (cls_app_static_var.Member_Number_1 > 0)
                mtxtMbid.Text = ds.Tables[base_db_name].Rows[0]["Mbid"].ToString() 
                    + '-' 
                    + ds.Tables[base_db_name].Rows[0]["Mbid2"].ToString();
            else
                mtxtMbid.Text = ds.Tables[base_db_name].Rows[0]["Mbid2"].ToString();

            txtMemberName.Text = ds.Tables[base_db_name].Rows[0]["Mem_Name"].ToString();

            mtxtZip1.Text = ds.Tables[base_db_name].Rows[0]["ZipCode"].ToString();
            
            txtAddress1.Text = ds.Tables[base_db_name].Rows[0]["add1"].ToString();
            txtAddress2.Text = ds.Tables[base_db_name].Rows[0]["add2"].ToString();


            if (combo_Se.Enabled == true)
            {
                combo_Se.Text = ds.Tables[base_db_name].Rows[0]["nationNameEng"].ToString();
                combo_Se_Code.Text = ds.Tables[base_db_name].Rows[0]["Na_code"].ToString();
            }

            string T_string;             string[] T_string_A;
            T_string = ds.Tables[base_db_name].Rows[0]["phone"].ToString();
            mtxtTel1.Text = T_string;
            //T_string_A = T_string.Split('-');
            //for (int i = 0; i <= T_string_A.Length - 1; i++)
            //{
            //    if (i == 0) txtTel_1.Text = T_string_A[i];
            //    if (i == 1) txtTel_2.Text = T_string_A[i];
            //    if (i == 2) txtTel_3.Text = T_string_A[i];
            //}


            T_string = ds.Tables[base_db_name].Rows[0]["FaxTel"].ToString();
            mtxtTel2.Text = T_string;

            //T_string_A = T_string.Split('-');
            //for (int i = 0; i <= T_string_A.Length - 1; i++)
            //{
            //    if (i == 0) txtFax_1.Text = T_string_A[i];
            //    if (i == 1) txtFax_2.Text = T_string_A[i];
            //    if (i == 2) txtFax_3.Text = T_string_A[i];
            //}

            mtxtBiz1.Text = ds.Tables[base_db_name].Rows[0]["Biz_Num"].ToString();
            

            txtBossName.Text = ds.Tables[base_db_name].Rows[0]["bossname"].ToString();
            txtConditions.Text = ds.Tables[base_db_name].Rows[0]["conditions"].ToString();
            txtItem.Text = ds.Tables[base_db_name].Rows[0]["item"].ToString();
            texETC.Text = ds.Tables[base_db_name].Rows[0]["Remarks"].ToString();


            txtNcode.Text = ds.Tables[base_db_name].Rows[0]["Ncode"].ToString();


            radio_U_1.Checked = true;
            if (int.Parse(ds.Tables[base_db_name].Rows[0]["U_TF"].ToString()) != 0)
                radio_U_2.Checked = true;


            //더블클릭이나 상품 정보를 불러온 상태에선느 상품 코드의 변경이 안일어 나게 하기 위해서 상품 코드 텍스트를 락시킨다
            //추후 위의 새로 입력 버튼으로 풀수 있음.
            txtNcode.BackColor = Color.AliceBlue;
            txtNcode.ReadOnly = true;

            txtName.Focus();
        }



        private void MtxtData_Temp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    string Sn = mtb.Text.Replace("-", "").Replace("_", "").Trim();
                    if (mtb.Name == "mtxtBiz1" )
                    {
                        if (Sn_Number_(Sn, mtb,"biz" ) == true)
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
                }
                else
                    SendKeys.Send("{TAB}");


            }
        }


        private bool Sn_Number_(string Sn, MaskedTextBox mtb, string sort_TF , int t_Sort2 = 0 )
        {
            if (Sn != "")
            {

                bool check_b = false;
                cls_Sn_Check csn_C = new cls_Sn_Check();

                //sort_TF = "biz";  //사업자번호체크
                //sort_TF = "Tel";  //전화번호체크
                //sort_TF = "Zip";  //우편번호체크

                check_b = csn_C.Number_NotInput_Check(mtb.Text , sort_TF);

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

                    mtb.Focus(); return false;
                }
            }

            return true;
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
                if (mtxtMbid.Text.Trim() != "")
                {
                    int reCnt = 0;
                    cls_Search_DB cds = new cls_Search_DB();
                    string Search_Name = "";
                    reCnt = cds.Member_Name_Search(mtxtMbid.Text, ref Search_Name);

                    if (reCnt == 1)
                    {
                        txtMemberName.Text = Search_Name;
                    }
                    else if (reCnt > 1)  //회원번호 비슷한 사람들이 많은 경우
                    {
                        string Mbid = "";
                        int Mbid2 = 0;
                        cds.Member_Nmumber_Split(mtxtMbid.Text,ref Mbid,ref Mbid2);

                        frmBase_Member_Search e_f = new frmBase_Member_Search();
                        e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                        e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info);
                        e_f.ShowDialog();
                    }
                }

                SendKeys.Send("{TAB}");
            }
        }

        void e_f_Send_MemNumber_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = "";
            cls_Search_DB csb = new cls_Search_DB();
            csb.Member_Nmumber_Split(mtxtMbid.Text.Trim(), ref searchMbid, ref searchMbid2);
        }

        void e_f_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            mtxtMbid.Text = Send_Number;
            txtMemberName.Text = Send_Name;
        }

        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {
            if (mtxtMbid.Text.Replace("_", "").Replace("-", "").Replace(" ", "") == "")
            {
                txtMemberName.Text = "";
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

       


        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);            
            T_R.Key_Enter_13_Name +=new Key_13_Name_Event_Handler(T_R_Key_Enter_13_Name);
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);

            TextBox tb  = (TextBox)sender;

            if ((tb.Tag == null) || (tb.Tag.ToString () == ""))
            {
                //숫자만 입력 가능하다.
                if (T_R.Text_KeyChar_Check(e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if   
            }
            else if (tb.Tag.ToString () == "1")
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e,1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if (tb.Tag.ToString () == "name")
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(tb,e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }
            else if (tb.Tag.ToString() == "ncode") //코드관련해서 코드를치면 관련 내역이 나오도록 하기 위함.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, tb) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }
                        
        }

        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void  T_R_Key_Enter_13_Name(string txt_tag, TextBox tb)
        {
 	        if (txt_tag != "")
            {
                int reCnt = 0;
                cls_Search_DB cds = new cls_Search_DB();
                string Search_Mbid = "";
                reCnt = cds.Member_Name_Search(ref Search_Mbid, txt_tag);

                if (reCnt == 1)
                {
                    if (tb.Name == "txtMemberName")
                        mtxtMbid.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
                }
                else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
                {
                   // cls_app_static_var.Search_Member_Name = txt_tag;
                    frmBase_Member_Search e_f = new frmBase_Member_Search();
                    e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                    e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);
                    e_f.ShowDialog();

                    //if (tb.Name == "txtMemberName")
                    //{
                    //    tb.Text = cls_app_static_var.Search_Member_Name_Return;
                    //    mtxtMbid.Text = cls_app_static_var.Search_Member_Number_Return;
                    //}                    
                }
                SendKeys.Send("{TAB}");
            }

        }

        void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtMemberName.Text.Trim();
        }



        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }

        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
            if (tb.Text.Trim() != "")
            {
                Data_Set_Form_TF = 1;                
                Form_Refresh_Data(tb.Text, cls_User.gid_CountryCode );
                Data_Set_Form_TF = 0;
            }
        }


        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

         
            if (bt.Name == "butt_Clear")
            {
                Base_Grid_Set();

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, txtNcode);

                txtNcode.BackColor = SystemColors.Window;
                txtNcode.ReadOnly = false;
                radio_U_1.Checked = true;

                ChkWeb.Checked = false;
                ChkCenterBonus.Checked = false;
                TxtMap.Text = "";
                CkviewMemberCenter.Checked = false;
                CkViewOrderCenter.Checked = false;
            }
            else if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex;
                Save_Base_Data(ref Save_Error_Check); //저장수정이 이루어닌다

                if (Save_Error_Check > 0)
                {
                    
                    cls_form_Meth ct = new cls_form_Meth();
                    ct.from_control_clear(this, txtNcode);
                    Base_Grid_Set();
                    txtNcode.BackColor = SystemColors.Window;
                    txtNcode.ReadOnly = false;
                    radio_U_1.Checked = true;

                    ChkWeb.Checked = false;
                    ChkCenterBonus.Checked = false;
                    TxtMap.Text = "";
                    CkviewMemberCenter.Checked = false;
                    CkViewOrderCenter.Checked = false;

                }

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }
            else if (bt.Name == "butt_Delete")
            {
                int Del_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex;
                Delete_Base_Data(ref Del_Error_Check);  //삭제 관련 작업이 이루어진다.

                if (Del_Error_Check > 0)
                {
                   
                    cls_form_Meth ct = new cls_form_Meth();
                    ct.from_control_clear(this, txtNcode);
                    Base_Grid_Set();
                    txtNcode.BackColor = SystemColors.Window;
                    txtNcode.ReadOnly = false;
                    radio_U_1.Checked = true;
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            else if (bt.Name == "butt_Excel")
            {
                frmBase_Excel e_f = new frmBase_Excel();
                e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
                e_f.ShowDialog();
            }

            else if (bt.Name == "butt_AddCode")
            {                
                frmBase_AddCode e_f = new frmBase_AddCode();
                e_f.Send_Address_Info += new frmBase_AddCode.SendAddressDele(e_f_Send_Address_Info); 
                e_f.ShowDialog();
                txtAddress2.Focus();            
            }
        }


        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Center";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }

        private void e_f_Send_Address_Info(string AddCode1, string AddCode2, string Address1, string Address2, string Address3)
        {
            mtxtZip1.Text = AddCode1 + "-" +  AddCode2 ; 

            txtAddress1.Text = Address1;            txtAddress2.Text = Address2; 

            
        }



        private Boolean Check_TextBox_Error(int i)
        {
            if (txtKey.Text.Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select"));
                dGridView_Base.Select();
                return false;
            }


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string Tsql;
            DataSet ds = new DataSet();

            Tsql = "select Out_Index from tbl_StockOutput  (nolock) ";
            Tsql = Tsql + " Where Out_C_Code ='" + txtNcode.Text.Trim() + "'";
            Tsql = Tsql + " And C_Code_FL ='C'";

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Data")
                    + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Output")
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtNcode.Select();
                return false;
            }


            Tsql = "select in_Index from tbl_Stockinput (nolock)  ";
            Tsql = Tsql + " Where in_C_Code ='" + txtNcode.Text.Trim() + "'";
            Tsql = Tsql + " And C_Code_FL ='C'";

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Data")
                    + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Input")
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtNcode.Select();
                return false;
            }


            Tsql = "select Move_From_Dep_Cd from tbl_Stock_Move_Sub  (nolock) ";
            Tsql = Tsql + " Where Move_From_Dep_Cd ='" + txtNcode.Text.Trim() + "'";            

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Data")
                    + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Move")
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtNcode.Select();
                return false;
            }


            Tsql = "select Move_To_Dep_Cd from tbl_Stock_Move_Sub  (nolock) ";
            Tsql = Tsql + " Where Move_To_Dep_Cd ='" + txtNcode.Text.Trim() + "'";            

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Data")
                    + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Move")
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtNcode.Select();
                return false;
            }


            Tsql = "Select Mbid ";
            Tsql = Tsql + " From tbl_Memberinfo  (nolock) ";
            Tsql = Tsql + " Where BusinessCode = '" + txtNcode.Text.Trim() + "'";
            Tsql = Tsql + " And   Na_Code = '" + cls_User.gid_CountryCode + "'"; 
            Tsql = Tsql + " Order by Mbid ASC ";

            
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Data")
                    + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Mem")
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtNcode.Select();
                return false;
            }


            Tsql = "Select Mbid ";
            Tsql = Tsql + " From tbl_SalesDetail  (nolock) ";
            Tsql = Tsql + " Where BusCode = '" + txtNcode.Text.Trim() + "'";
            Tsql = Tsql + " And   Na_Code = '" + cls_User.gid_CountryCode + "'"; 
            Tsql = Tsql + " Order by Mbid ASC ";

            ds.Clear();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Data")
                + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Sell")
                + "\n" +
                cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                txtNcode.Select();
                return false;
            }

            return true;
        }


        private void Delete_Base_Data(ref int Del_Error_Check)
        {
            Del_Error_Check = 0;
            if (Check_TextBox_Error(1) == false) return;

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();                       
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            string Tsql;           

            try
            {
                //Tsql = "Update tbl_Memberinfo Set ";
                //Tsql = Tsql + " businesscode = '' ";
                //Tsql = Tsql + " Where businesscode = '" + (txtNcode.Text).Trim() + "'";

                //Temp_Connect.Update_Data(Tsql, Conn,tran, this.Name.ToString(), this.Text);


                //Tsql = "Update tbl_SalesDetail Set ";
                //Tsql = Tsql + " BusCode = '' ";
                //Tsql = Tsql + " Where BusCode = '" + (txtNcode.Text).Trim() + "'";

                //Temp_Connect.Update_Data(Tsql, Conn, tran, this.Name.ToString(), this.Text);


                Tsql = "Delete From tbl_Business ";
                Tsql = Tsql + " Where Ncode = '" + txtNcode.Text.Trim() + "'";
                Tsql = Tsql + " And   Na_Code = '" + cls_User.gid_CountryCode + "'"; 

                if (Temp_Connect.Delete_Data(Tsql, base_db_name, Conn, tran, this.Name.ToString(), this.Text) == false) return;

                //Conn.  ;

                tran.Commit();                

                Del_Error_Check =1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del"));
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






        private Boolean Check_TextBox_Error()
        {
            cls_Check_Text T_R = new cls_Check_Text();

            string me = T_R.Text_Null_Check(txtNcode);
            if (me != "")
            {
                MessageBox.Show(me);
                return false;
            }

            me = T_R.Text_Null_Check(txtName);
            if (me != "")
            {
                MessageBox.Show(me);
                return false;
            }


            //수정을 위한 저장 버튼 클릭인데 센타 코드가 변해 있다 그럼 안되지요.. 막아야함.
            if ((txtKey.Text.Trim() != "") && (txtNcode.Text.Trim() != txtKey.Text.Trim()))
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Key_Not_Change") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtNcode.Focus();
                return false;
            }

            
            //if ((txtAddCode1.Text.Trim() != "") || (txtAddCode2.Text.Trim() != ""))
            //{
            //    if ((txtAddCode1.Text.Trim() == "") || (txtAddCode2.Text.Trim() == ""))
            //    {
            //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
            //            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_AddCode")
            //           + "\n" +
            //           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //        txtAddCode1.Focus();
            //        return false;
            //    }
            //}//우편번호가 다 입력이 되엇는지 체크를 한다.


            //if ((txtTel_1.Text.Trim() != "") || (txtTel_2.Text.Trim() != "") || (txtTel_3.Text.Trim() != ""))
            //{
            //    if ((txtTel_1.Text.Trim() == "") || (txtTel_2.Text.Trim() == "") || (txtTel_3.Text.Trim() == ""))
            //    {
            //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
            //            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Tel")
            //           + "\n" +
            //           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //        txtTel_1.Focus();
            //        return false;
            //    }

            //} //전화 번호가 3칸다 제대로 들어 왓는지 체크를 한다.  


            //if ((txtFax_1.Text.Trim() != "") || (txtFax_2.Text.Trim() != "") || (txtFax_3.Text.Trim() != ""))
            //{
            //    if ((txtFax_1.Text.Trim() == "") || (txtFax_2.Text.Trim() == "") || (txtFax_3.Text.Trim() == ""))
            //    {
            //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
            //            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Fax")
            //           + "\n" +
            //           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //        txtFax_1.Focus();
            //        return false;
            //    }
            //} //팩스 번호가 제대로 들어 왓는지 체크한다.



            //if ((txtBnum1.Text.Trim() != "") || (txtBnum2.Text.Trim() != "") || (txtBnum3.Text.Trim() != ""))
            //{
            //    if ((txtBnum1.Text.Trim() == "") || (txtBnum2.Text.Trim() == "") || (txtBnum3.Text.Trim() == ""))
            //    {
            //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
            //            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_BuNum")
            //           + "\n" +
            //           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //        txtBnum3.Focus();
            //        return false;
            //    }
            //} //사업자 번호가 제대로 들어 왓는지 체크한다.


            string Sn = mtxtTel1.Text.Replace("-", "").Replace("_", "").Trim();
            if (Sn_Number_(Sn, mtxtTel1, "Tel") == false)
            {
                mtxtTel1.Focus();
                return false;
            }

            Sn = mtxtTel2.Text.Replace("-", "").Replace("_", "").Trim();
            if (Sn_Number_(Sn, mtxtTel2, "Tel") == false)
            {
                mtxtTel2.Focus();
                return false;
            }

            Sn = mtxtZip1.Text.Replace("-", "").Replace("_", "").Trim();
            if (Sn_Number_(Sn, mtxtZip1, "Zip") == false)
            {
                mtxtZip1.Focus();
                return false;
            }


            Sn = mtxtBiz1.Text.Replace("-", "").Replace("_", "").Trim();
            if (Sn_Number_(Sn, mtxtBiz1, "biz") == false)
            {
                mtxtBiz1.Focus();
                return false;
            }


            
            

            return true;
        }


        private bool  Check_TextBox_Error(string SaveCheck_2)
        {
            SaveCheck_2 = "";


            if (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                cls_Search_DB csb = new cls_Search_DB();
                string Search_Name =csb.Member_Name_Search(mtxtMbid.Text);


                if (Search_Name == "-1")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtMbid.Focus();
                    return false;
                }


                else if (Search_Name != "")
                    txtMemberName.Text = Search_Name;

                else if (Search_Name == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtMbid.Focus();
                    return false;
                }
                else //이도 저도 아닌 -2 같은 에러가 나온다. 그럼 다 리셋 시켜 버린다.
                {
                    mtxtMbid.Text = ""; txtMemberName.Text = "";
                }

            }//센타장으로 해서 회원번호를 입력한 경우
            else
                txtMemberName.Text = "";   //회원번호 입력 안되어있는 데 회원명 입력 될수 있기 때문에 그런 경우를 대비해서  회원명을 빈칸으로 함.
                       




            if (txtKey.Text.Trim() == "")
            {
                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                string Tsql;
                Tsql = "Select Ncode, Name ";
                Tsql = Tsql + " From tbl_business  (nolock)  ";
                Tsql = Tsql + " Where upper(Ncode) = '" + ((txtNcode.Text).Trim()).ToUpper() + "'";
                Tsql = Tsql + " And   Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                Tsql = Tsql + " Order by Ncode ASC ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 코드가 있다 그럼.이거 저장하면 안되요
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_Code") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txtNcode.Select();
                    return false;
                }


                Tsql = "Select Ncode, Name ";
                Tsql = Tsql + " From tbl_business  (nolock)  ";
                Tsql = Tsql + " Where Name = '" + (txtName.Text).Trim() + "'";
                Tsql = Tsql + " And   Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                Tsql = Tsql + " Order by Ncode ASC ";

                ds.Clear();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 이름이 있다 그럼.이거 저장하면 안되요
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_Name") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txtName.Select();
                    return false;
                }

                //++++++++++++++++++++++++++++++++
            }
            else
            {
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                string Tsql;
                Tsql = "Select Ncode, Name ";
                Tsql = Tsql + " From tbl_business  (nolock)  ";
                Tsql = Tsql + " Where upper(Ncode) <> '" + ((txtNcode.Text).Trim()).ToUpper() + "'";
                Tsql = Tsql + " And  Name = '" + (txtName.Text).Trim() + "'";
                Tsql = Tsql + " And   Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                Tsql = Tsql + " Order by Ncode ASC ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 이름으로 코드가 있다 그럼.이거 저장하면 안되요
                {

                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_Name") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtName.Select();
                    return false;
                }
            }

            return true;
        }





        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
            if (Check_TextBox_Error() == false) return;
            if (Check_TextBox_Error("Save_Err_Check_2") == false) return;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Mbid = ""; int Mbid2 = 0; string phone = ""; string FaxTel = "";

            

            if (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                cls_Search_DB csb = new cls_Search_DB();
                csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2);                
            }

            //if (txtTel_1.Text != "") phone = txtTel_1.Text.Trim() + '-' + txtTel_2.Text.Trim() + '-' + txtTel_3.Text.Trim();
            //if (txtFax_1.Text != "") FaxTel = txtFax_1.Text.Trim() + '-' + txtFax_2.Text.Trim() + '-' + txtFax_3.Text.Trim();

            phone = mtxtTel1.Text  ;
            FaxTel = mtxtTel2.Text;

            int U_TF = 0;
            if (radio_U_2.Checked == true) U_TF = 1; 

            string Na_Code  = cls_User.gid_CountryCode;
            //if (Na_Code == "")
            //    Na_Code = "KR"; 

            string Tsql;
            Tsql = "SELECT Ncode, Name ";
            Tsql = Tsql + " From tbl_Business   (nolock) ";
            Tsql = Tsql + " Where upper(Ncode) = '" + ((txtNcode.Text).Trim()).ToUpper() + "'";
            Tsql = Tsql + " And   Na_Code = '" + Na_Code + "'"; 
            Tsql = Tsql + " Order by Ncode ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)//동일한 코드가없네 그럼 인설트
            {
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

                Tsql = "insert into tbl_Business ( ";
                Tsql = Tsql + " ncode, name, Biz_Num, ";
                Tsql = Tsql + " bossname, conditions, item, ZipCode, add1, ";
                Tsql = Tsql + " add2, phone, mbid, mbid2, mname,FaxTel,Remarks, WebRemarks,";
                Tsql = Tsql + " Temp01,U_TF , Na_Code ,";
                Tsql = Tsql + " Map,Web,CenterBonus, ShowMemberCenter,ShowOrderCenter, recordid, recordtime ";
                Tsql = Tsql + " ) values(" ;
                Tsql = Tsql + " '" + txtNcode.Text.Trim() + "'";
                Tsql = Tsql + ",'" + txtName.Text.Trim() + "'";

                Tsql = Tsql + ",'" + mtxtBiz1.Text.Trim() + "'";                
                Tsql = Tsql + ",'" + txtBossName.Text.Trim() + "'";
                Tsql = Tsql + ",'" + txtConditions.Text.Trim() + "'";
                Tsql = Tsql + ",'" + txtItem.Text.Trim() + "'";

                Tsql = Tsql + ",'" + mtxtZip1.Text.Trim() + "'";                
                Tsql = Tsql + ",'" + txtAddress1.Text.Trim() + "'";
                Tsql = Tsql + ",'" + txtAddress2.Text.Trim() + "'";
                
                Tsql = Tsql + ",'" + phone + "'";
                Tsql = Tsql + ",'" + Mbid + "'";
                Tsql = Tsql + ", " + Mbid2 ;
                Tsql = Tsql + ",'" + txtMemberName.Text.Trim() + "'" ;
                Tsql = Tsql + ",'" + FaxTel + "'" ;
                Tsql = Tsql + ",'" + texETC.Text  + "'";
                Tsql = Tsql + ",'" + texETC.Text.Replace(Environment.NewLine, "<br/>")  + "'";
                Tsql = Tsql + ", '1'" ;
                Tsql = Tsql + ", " + U_TF ;
                Tsql = Tsql + ",'" + Na_Code + "'";


                Tsql = Tsql + ",N'" + TxtMap.Text.Trim() + "'";
                if(ChkWeb.Checked == true)
                {
                    Tsql = Tsql + ", 'Y' ";
                }
                else
                {
                    Tsql = Tsql + ", 'N' ";
                }
                if (ChkCenterBonus.Checked == true)
                {
                    Tsql = Tsql + ", 'Y' ";
                }
                else
                {
                    Tsql = Tsql + ", 'N' ";
                }
                if (CkviewMemberCenter.Checked == true)
                {
                    Tsql = Tsql + ", 'Y' ";
                }
                else
                {
                    Tsql = Tsql + ", 'N' ";
                }
                if (CkViewOrderCenter.Checked == true)
                {
                    Tsql = Tsql + ", 'Y' ";
                }
                else
                {
                    Tsql = Tsql + ", 'N' ";
                }
                //Tsql = Tsql + ",Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 

                Tsql = Tsql + ",'" + cls_User.gid  + "'";
                Tsql = Tsql + " , Convert(Varchar(25),GetDate(),21) ";
                Tsql = Tsql + ")" ;
                
                if (Temp_Connect.Insert_Data(Tsql, base_db_name, this.Name.ToString(), this.Text) == false) return;

                Save_Error_Check =1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));


            }
            else //동일한 코드가 있구나 그럼 업데이트
            {
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

                Tsql = "Update tbl_Business Set ";
                Tsql = Tsql + " name = '" + txtName.Text.Trim() + "'";
                Tsql = Tsql + " ,Biz_Num = '" + mtxtBiz1.Text.Trim() + "'";
                Tsql = Tsql + " ,bossname = '" +  txtBossName.Text.Trim() + "'";
                Tsql = Tsql + " ,conditions = '" +  txtConditions.Text.Trim() + "'";
                Tsql = Tsql + " ,item = '" +  txtItem.Text.Trim() + "'";
                Tsql = Tsql + " ,ZipCode = '" + mtxtZip1.Text.Trim() + "'";                
                Tsql = Tsql + " ,add1 = '" + txtAddress1.Text.Trim() + "'";
                Tsql = Tsql + " ,add2 = '" + txtAddress2.Text.Trim() + "'";
                Tsql = Tsql + " ,phone = '" + phone + "'";
                Tsql = Tsql + " ,FaxTel='" + FaxTel + "'";
                Tsql = Tsql + " ,U_TF = " + U_TF ;

                Tsql = Tsql + " ,mbid = '" +  Mbid + "'";
                Tsql = Tsql + " ,mbid2 =  " + Mbid2 ;
                Tsql = Tsql + " ,mname = '" +  txtMemberName.Text.Trim() + "'";
                Tsql = Tsql + " ,recordid = '" + cls_User.gid + "'";
                Tsql = Tsql + " ,Remarks='" + texETC.Text + "'";
                Tsql = Tsql + " ,WebRemarks='" + texETC.Text.Replace(Environment.NewLine, "<br/>")  + "'";


                Tsql = Tsql + " ,Map= N'" + TxtMap.Text.Trim() + "'";


                if (ChkWeb.Checked == true)
                {
                    Tsql = Tsql + ", Web= 'Y' ";
                }
                else
                {
                    Tsql = Tsql + ", Web= 'N' ";
                }
                if (ChkCenterBonus.Checked == true)
                {
                    Tsql = Tsql + ",CenterBonus= 'Y' ";
                }
                else
                {
                    Tsql = Tsql + ",CenterBonus= 'N' ";
                }
                if (CkviewMemberCenter.Checked == true)
                {
                    Tsql = Tsql + ", ShowMemberCenter =  'Y' ";
                }
                else
                {
                    Tsql = Tsql + ", ShowMemberCenter = 'N' ";
                }
                if (CkViewOrderCenter.Checked == true)
                {
                    Tsql = Tsql + ", ShowOrderCenter = 'Y' ";
                }
                else
                {
                    Tsql = Tsql + ", ShowOrderCenter =  'N' ";
                }


                //Tsql = Tsql + ",Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 


                Tsql = Tsql + " WHERE Ncode = '" + txtNcode.Text.Trim() + "'";
                Tsql = Tsql + " And   Na_Code = '" + Na_Code + "'"; 

                if (Temp_Connect.Update_Data(Tsql, this.Name.ToString(), this.Text) == false) return;

                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));

            }

        }





        



        //public delegate void FormSendDataHandler(object obj);
        //public event FormSendDataHandler FormSendEvent;
        //private void frm_Base_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    this.Visible = false;
        //    this.FormSendEvent(this.Text);
        //}

        
  
    } //end form
} // end
