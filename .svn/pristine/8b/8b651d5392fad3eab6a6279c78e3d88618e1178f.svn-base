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
    public partial class frmBase_AddCode : Form
    {

        public delegate void SendAddressDele(string AddCode1, string AddCode2, string Address1, string Address2, string Address3);
        public event SendAddressDele Send_Address_Info;

        private string idx_output_User_ID = "" ;

        cls_Grid_Base cgb = new cls_Grid_Base();
        private const string base_db_name = "tbl_zipcode";
        private string t_AddCode1;
        private string t_AddCode2;
        private int FormLoad_TF = 0;
        private int Data_Set_Form_TF = 0;
        private int Data_Set_Form_TF2 = 0;

        public frmBase_AddCode()
        {
            InitializeComponent();
        }

        
        private void frmBase_From_Load(object sender, EventArgs e)
        {

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            byte[] contents1 = System.Text.Encoding.Unicode.GetBytes(cls_User.gid);
            idx_output_User_ID = BitConverter.ToString(contents1);


            string TSql = "Delete From tbl_Temp_ADD  ";
            TSql = TSql + " Where Com_Name ='LuLuE' ";
            TSql = TSql + " And  User_ID ='" + idx_output_User_ID + "' ";

            Temp_Connect.Query_Exec_God_Daum(TSql);


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Search);
            cfm.button_flat_change(butt_Search_2);
            cfm.button_flat_change(butt_Input);

            txtAdd1.BackColor = cls_app_static_var.txt_Enable_Color; 

            Data_Set_Form_TF = 0;
            Data_Set_Form_TF2 = 0; 
            t_AddCode1 = "";   t_AddCode2 = "";
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            FormLoad_TF = 1;
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
            txtDong.Focus();
            FormLoad_TF = 0;

            //byte[] contents = Encoding.UTF8.GetBytes(cls_User.gid);
            //idx_output_User_ID = Encoding.UTF8.GetString(contents);

        

            string t_url = "http://www.isgate.co.kr/standard/address/addressDaumApiPage.do?company=LuLuE&adminId=" + idx_output_User_ID;
            //string t_url = "http://test.isgate.co.kr/standard/address/addressDaumApiPage.do?company=QSciences&adminId=" + cls_User.gid;
            

            webBrowser1.Navigate(t_url);

            timer1.Enabled = true; 

            ////ComboBox  ctlcb;
            ////foreach (Control ctl in this.Controls)
            ////{
            ////    try
            ////    {
            ////        // Attempt to cast the control to type MdiClient.
            ////        ctlcb = (ComboBox)ctl;

            ////        // Set the BackColor of the MdiClient control.
            ////        ctlcb.BackColor = SystemColors.Window;
            ////    }
            ////    catch (InvalidCastException)
            ////    {
            ////        // Catch and ignore the error if casting failed.
            ////    }
            ////}


        }


        private void DataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            //그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
            if (sender is DataGridView)
            {

                if (e.KeyValue == 13)
                {
                    EventArgs ee = null;
                    dGridView_Base_DoubleClick(sender, ee);
                    e.Handled = true;
                } // end if
            }
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

            

            Button T_bt = butt_Input;
            if (e.KeyValue == 113)
                T_bt = butt_Input;     //저장  F1
            //if (e.KeyValue == 115)
            //    T_bt = butt_Delete;   // 삭제  F4
            
            EventArgs ee1 = null;
            if (e.KeyValue == 113 )
                Base_Button_Click(T_bt, ee1);  
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
            Tsql = "select zipcode,Total_Address, seq ,'','' ";
            Tsql = Tsql + " From tbl_zipcode  (nolock)   ";
            Tsql = Tsql + " Where charindex ('" + txtDong.Text.Trim() + "',dong) > 0 ";
            Tsql = Tsql + " order by zipcode";

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set_AddCode (Tsql, base_db_name, ds) == false) return;
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
            cgb.grid_col_Count = 5;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            //cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = { "우편_번호" , "주소" ,"" , ""   , ""                                      
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 600, 0, 0, 0                              
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                                                        
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5                               
                                                                                     
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
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }




        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            t_AddCode1 = ""; t_AddCode2 = "";
            string Seq = "";
            string TempAdd1 = "";

            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string t_AddCode =(sender as DataGridView).CurrentRow.Cells[0].Value.ToString().Replace ("-","");
                t_AddCode1 = t_AddCode.Substring(0, 3);
                t_AddCode2 = t_AddCode.Substring(3, 3);

                if ((sender as DataGridView).Name == "dGridView_Base")
                {
                    Seq = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString();

                    cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                    string Tsql;
                    Tsql = "select sido, gugun, dong, ri, dose, san, start_Bunji, start_Bunji2 , end_Bunji,  end_Bunji2 , ap_name, ap_start_dong, ap_end_dong , total_address";
                    Tsql = Tsql + " From tbl_zipcode  (nolock)   ";
                    Tsql = Tsql + " Where replace(zipcode,'-','') = '" + t_AddCode + "'";
                    Tsql = Tsql + " And  Seq = '" + Seq + "'";

                    DataSet ds = new DataSet();

                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set_AddCode(Tsql, base_db_name, ds) == false) return;
                    int ReCnt = Temp_Connect.DataSet_ReCount;

                    if (ReCnt == 0) return;
                    //++++++++++++++++++++++++++++++++

                    

                    if (ds.Tables[base_db_name].Rows[0]["ap_name"].ToString() != "")
                    {
                        TempAdd1 = ds.Tables[base_db_name].Rows[0]["sido"].ToString() + " " + ds.Tables[base_db_name].Rows[0]["gugun"].ToString();
                        if (ds.Tables[base_db_name].Rows[0]["dong"].ToString() != "") TempAdd1 = TempAdd1 + ds.Tables[base_db_name].Rows[0]["dong"].ToString();
                        if (ds.Tables[base_db_name].Rows[0]["ri"].ToString() != "") TempAdd1 = TempAdd1 + ds.Tables[base_db_name].Rows[0]["ri"].ToString();
                        if (ds.Tables[base_db_name].Rows[0]["ap_name"].ToString() != "") TempAdd1 = TempAdd1 + ds.Tables[base_db_name].Rows[0]["ap_name"].ToString();
                    }
                    else
                    {
                        TempAdd1 = ds.Tables[base_db_name].Rows[0]["sido"].ToString() + " " + ds.Tables[base_db_name].Rows[0]["gugun"].ToString();
                        if (ds.Tables[base_db_name].Rows[0]["dong"].ToString() != "") TempAdd1 = TempAdd1 + ds.Tables[base_db_name].Rows[0]["dong"].ToString();
                        if (ds.Tables[base_db_name].Rows[0]["ri"].ToString() != "") TempAdd1 = TempAdd1 + ds.Tables[base_db_name].Rows[0]["ri"].ToString();
                        if (ds.Tables[base_db_name].Rows[0]["dose"].ToString() != "") TempAdd1 = TempAdd1 + ds.Tables[base_db_name].Rows[0]["dose"].ToString();
                        if (ds.Tables[base_db_name].Rows[0]["San"].ToString() != "") TempAdd1 = TempAdd1 + ds.Tables[base_db_name].Rows[0]["San"].ToString();
                    }
                }
                else
                {
                    TempAdd1 = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString();
                }

                FormLoad_TF = 1;
                txtAdd1.Text = TempAdd1;  // (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
                txtAdd2.Focus();
                FormLoad_TF = 0;
            }

           
        }

        

        private void txtData_Enter(object sender, EventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();
            T_R.Text_Focus_All_Sel((TextBox)sender);

            TextBox tb = (TextBox)sender;
            if (tb.ReadOnly == false)
                tb.BackColor = cls_app_static_var.txt_Focus_Color;  //Color.FromArgb(239, 227, 240);   

            if (this.Controls.ContainsKey("Popup_gr"))
            {
                DataGridView T_Gd = (DataGridView)this.Controls["Popup_gr"];
                T_Gd.Visible = false;
                T_Gd.Dispose();
            }
        }


        private void txtData_Base_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.ReadOnly == false)
                tb.BackColor = Color.White;
        }




        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
            

            TextBox tb = (TextBox)sender;

            if ((tb.Tag == null) ||  (tb.Tag.ToString () == ""))
            {
                //숫자만 입력 가능하다.
                if (T_R.Text_KeyChar_Check(e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if   
            }
            else if (tb.Tag.ToString() == "1")
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            if (tb.TextLength + 1 >= tb.MaxLength)
                SendKeys.Send("{TAB}");

            //if (tb.Name == "txtAdd2" && e.KeyChar == 13)
                //butt_Input.Focus();


        }


        void T_R_Key_Enter_13()
        {            
            SendKeys.Send("{TAB}");            
        }
             



        private void Base_Button_Click(object sender, EventArgs e)
        {

            Button bt = (Button)sender;

            if (bt.Name == "butt_Search")
            {
                if (txtDong.Text.Trim() == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Select_Term")
                   + "\n" +
                   cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtDong.Focus();      return ;
                }
                
                Base_Grid_Set();
                dGridView_Base.Focus();
            }
            else if (bt.Name == "butt_Input")
            {
                Send_Address_Info(t_AddCode1, t_AddCode2, txtAdd1.Text.Trim(), txtAdd2.Text.Trim(), txtAdd3.Text.Trim());
                this.Close();
            }             
                         
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tbc = (TabControl)sender;

            if (tbc.SelectedIndex == 1)
            {

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset_2(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                txtDong2.Text = "";
                combo_Sido.Items.Clear();

                 cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                string Tsql;
                Tsql = "Select Temp_Code,SiDo_Name, SiDo_Name_2 From tbl_SiDo_Code (nolock) ";
                Tsql = Tsql + " Order by Temp_Code  ";
                
                DataSet ds = new DataSet();

                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set_AddCode(Tsql, base_db_name, ds) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;
                //++++++++++++++++++++++++++++++++
                                
                string[] data_P = new string[ReCnt];
                string[] data_P_C = new string[ReCnt];

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    data_P[fi_cnt] = ds.Tables[base_db_name].Rows[fi_cnt]["SiDo_Name"].ToString();
                    data_P_C[fi_cnt] = ds.Tables[base_db_name].Rows[fi_cnt]["SiDo_Name_2"].ToString();
                }

                Data_Set_Form_TF = 1;
                combo_Sido.Items.AddRange(data_P);
                combo_Sido.SelectedIndex = -1;

                combo_Sido_Code.Items.AddRange(data_P_C);
                combo_Sido_Code.SelectedIndex = combo_Sido.SelectedIndex;

                combo_Gu.Items.Clear();

                Data_Set_Form_TF = 0;
            }

        }

        private void combo_Sido_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return; 

            if (combo_Sido.Text == "")
                combo_Gu.Items.Clear();
            else
            {
                Data_Set_Form_TF2 = 1;
                combo_Sido_Code.SelectedIndex = combo_Sido.SelectedIndex;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                
                
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                string Tsql;
                Tsql = "Select  gugun From " + combo_Sido_Code.Text.Trim() + " (nolock)  Group By gugun ";
                Tsql = Tsql + " Order by gugun ASC  ";

                DataSet ds = new DataSet();

                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set_AddCode(Tsql, base_db_name, ds) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;
                //++++++++++++++++++++++++++++++++

                string[] data_P = new string[ReCnt];

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    data_P[fi_cnt] = ds.Tables[base_db_name].Rows[fi_cnt]["gugun"].ToString();
                }

                combo_Gu.Items.Clear();
                combo_Gu.Items.AddRange(data_P);
                combo_Gu.SelectedIndex = -1;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                Data_Set_Form_TF2 = 0;

            }
        }

        private void butt_Search_2_Click(object sender, EventArgs e)
        {
            if (combo_Sido.Text.Trim() == "")
            {
                MessageBox.Show("시/도를 선택해 주십시요!"
               + "\n" +
               cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                combo_Sido.Focus(); return;
            }

            if (combo_Gu.Text.Trim() == "")
            {
                MessageBox.Show("구/시를 선택해 주십시요!"
               + "\n" +
               cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                combo_Gu.Focus(); return;
            }


            if (txtDong2.Text.Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Select_Term")
               + "\n" +
               cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtDong2.Focus(); return;
            }

            Base_Grid_Set_2();
            dGridView_Base.Focus();
        }



        private void Base_Grid_Set_2()
        {


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset_2(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string StrSql;
            StrSql = "select Zip_Do.Zipcode , buildingCode, Zip_Do.sido, isnull(Zip_Do.gugun,'') gugun, isnull(eubmyun,'') eubmyun " ;
            StrSql = StrSql + " , isnull(doromyung,'') doromyung, isnull(buildingNum,'') buildingNum ,isnull( buildingNumSub ,'') buildingNumSub, isnull(buildingName ,'') buildingName";
            StrSql = StrSql + " , total_address , isnull(rawDong,'') rawDong  From " +  combo_Sido_Code.Text.Trim() + "  AS Zip_Do (nolock) " ;
            StrSql = StrSql + " LEFT JOIN tbl_zipcode  tbl_zipcode  (nolock)  on  tbl_zipcode.ZipCode = Zip_Do.ZipCode and  tbl_zipcode.seq = Zip_Do.ZipCode_seq ";
            StrSql = StrSql + " Where Zip_Do.gugun ='" + combo_Gu.Text.Trim() + "'";
            StrSql = StrSql + " And  (charindex('" + txtDong2.Text.Trim() + "',doromyung) > 0 ";
            StrSql = StrSql + " OR    charindex('" + txtDong2.Text.Trim() + "',buildingName) > 0  )"; 

            if (txt_B_Num.Text  !="")
                StrSql = StrSql + " And  charindex ('" + txt_B_Num.Text.Trim() + "',buildingnum ) > 0  ";
            StrSql = StrSql + " Order by buildingCode ASC" ;

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set_AddCode(StrSql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, string[]> gr_dic_text = new Dictionary<int, string[]>();
            string TAdd = "";
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                if (ds.Tables[base_db_name].Rows[fi_cnt]["eubmyun"].ToString()  != "")
                    TAdd = ds.Tables[base_db_name].Rows[fi_cnt]["sido"].ToString() + " " + ds.Tables[base_db_name].Rows[fi_cnt]["gugun"].ToString() ;
                else
                    TAdd = ds.Tables[base_db_name].Rows[fi_cnt]["sido"].ToString() + " " + ds.Tables[base_db_name].Rows[fi_cnt]["gugun"].ToString() + " " + ds.Tables[base_db_name].Rows[fi_cnt]["eubmyun"].ToString() ;                
                        
                TAdd = TAdd + " " + ds.Tables[base_db_name].Rows[fi_cnt]["doromyung"].ToString()  +  " " + ds.Tables[base_db_name].Rows[fi_cnt]["buildingNum"].ToString()  ;


                if (ds.Tables[base_db_name].Rows[fi_cnt]["buildingNumSub"].ToString() != "" && ds.Tables[base_db_name].Rows[fi_cnt]["buildingNumSub"].ToString() != "0")
                    TAdd = TAdd + "-" + ds.Tables[base_db_name].Rows[fi_cnt]["buildingNumSub"].ToString() ;
                        
                TAdd = TAdd + "(" ;
            
                if (ds.Tables[base_db_name].Rows[fi_cnt]["rawDong"].ToString() != "" && ds.Tables[base_db_name].Rows[fi_cnt]["buildingName"].ToString() != "" )
                    TAdd = TAdd + ds.Tables[base_db_name].Rows[fi_cnt]["rawDong"].ToString()  + "," + ds.Tables[base_db_name].Rows[fi_cnt]["buildingName"].ToString() ;

                if (ds.Tables[base_db_name].Rows[fi_cnt]["rawDong"].ToString() == "" && ds.Tables[base_db_name].Rows[fi_cnt]["buildingName"].ToString() != "" )
                    TAdd = TAdd +  ds.Tables[base_db_name].Rows[fi_cnt]["buildingName"].ToString() ;

                if (ds.Tables[base_db_name].Rows[fi_cnt]["rawDong"].ToString() != "" && ds.Tables[base_db_name].Rows[fi_cnt]["buildingName"].ToString() == "" )
                    TAdd = TAdd +  ds.Tables[base_db_name].Rows[fi_cnt]["rawDong"].ToString() ;
                            
                TAdd = TAdd + ")" ;

                string[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt]["Zipcode"].ToString()  
                                ,TAdd + "\n" + ds.Tables[base_db_name].Rows[fi_cnt]["total_address"].ToString()  
                                ,TAdd 
                                ,""  
                                ,""
                                 };

                gr_dic_text[fi_cnt + 1] = row0;
            }

            cgb.grid_name = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Data_Put();
        }



        private void dGridView_Base_Header_Reset_2()
        {
            cgb.grid_col_Count = 5;
            cgb.basegrid = dGridView_Base2;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            //cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            cgb.RowTemplate_Height = 35;

            string[] g_HeaderText = { "우편_번호" , "신주소" ,"" , ""   , ""                                      
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 600, 0, 0, 0                              
                            };
            cgb.grid_col_w = g_Width;
            

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                                                        
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5                               
                                                                                     
                              };
            cgb.grid_col_alignment = g_Alignment;
        }

        private void combo_Gu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF2 == 1) return;
            txtDong2.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select Com_Name,User_ID, ZipCode, Add_1 From tbl_Temp_ADD (nolock) ";
            Tsql = Tsql + " Where Com_Name ='LuLuE' ";
            Tsql = Tsql + " And  User_ID ='" + idx_output_User_ID + "' ";

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set_AddCode_Daum(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {
                timer1.Enabled = false;

                t_AddCode1 = ds.Tables[base_db_name].Rows[0]["ZipCode"].ToString();
                t_AddCode2 = "";
                string Add_1 = ds.Tables[base_db_name].Rows[0]["Add_1"].ToString();
                Send_Address_Info(t_AddCode1, t_AddCode2, Add_1, "", "");
                this.Close();
                return;
            }
            //++++++++++++++++++++++++++++++++
        }

        

      

     

    }
}
