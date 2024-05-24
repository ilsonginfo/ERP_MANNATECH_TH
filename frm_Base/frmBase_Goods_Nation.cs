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
    public partial class frmBase_Goods_Nation : clsForm_Extends
    {
        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_2 = new cls_Grid_Base();
        private const string base_db_name = "tbl_Goods_Na_item";
        private int Data_Set_Form_TF;

        Dictionary<string, TreeNode> dic_Tree_Sort_1 = new Dictionary<string, TreeNode>();  //상품 코드 분류상 대분류 관련 트리노드를 답는곳
        Dictionary<string, TreeNode> dic_Tree_Sort_2 = new Dictionary<string, TreeNode>();  //상품 코드 분류상 중분류 관려련 트리 노드를 답는곳


        public frmBase_Goods_Nation()
        {
            InitializeComponent();
        }

        private void frmBase_Goods_Nation_Load(object sender, EventArgs e)
        {

           
            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_NaCode_ComboBox (combo_Se, combo_Se_Code);


            Base_Grid_Set();


            //Base_Goods_Grid_Set();
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);


            Data_Set_Form_TF = 0;

            mtxtApplyDate.Mask = cls_app_static_var.Date_Number_Fromat;

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



        private void Base_Grid_Set(string Ncode = "")
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;

            Tsql = "Select ItemCode , Name , Na_Code ";
            Tsql = Tsql + " , Isnull (( Select Top 1 tbl_Goods_Change.price2 From tbl_Goods_Change (nolock) Where tbl_Goods_Change.Ncode = tbl_Goods_Na_item.ItemCode And tbl_Goods_Change.Na_Code  = tbl_Goods_Na_item.Na_code Order by ApplyDate DESC   ),tbl_Goods_Na_item.price2) Last_price2 ";
            Tsql = Tsql + " , Isnull (( Select Top 1 tbl_Goods_Change.price4 From tbl_Goods_Change (nolock) Where tbl_Goods_Change.Ncode = tbl_Goods_Na_item.ItemCode And tbl_Goods_Change.Na_Code  = tbl_Goods_Na_item.Na_code Order by ApplyDate DESC   ),tbl_Goods_Na_item.price4) Last_price4 ";
            Tsql = Tsql + " , Isnull (( Select Top 1 tbl_Goods_Change.price5 From tbl_Goods_Change (nolock) Where tbl_Goods_Change.Ncode = tbl_Goods_Na_item.ItemCode And tbl_Goods_Change.Na_Code  = tbl_Goods_Na_item.Na_code Order by ApplyDate DESC   ),tbl_Goods_Na_item.price5) Last_price5 ";
            Tsql = Tsql + " , tbl_Goods_Na_item.Recordid , tbl_Goods_Na_item.Recordtime , tbl_Goods_Na_item.Seq";
            Tsql = Tsql + " From tbl_Goods_Na_item (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Goods (nolock) ON tbl_Goods.ncode = tbl_Goods_Na_item.ItemCode"; 
            Tsql = Tsql + " Where Seq > 0  "; //셋트 상품만 불러온다.
            

            if (Ncode != "")
            {
                Tsql = Tsql + " And ( ItemCode Like '%" + Ncode.Trim() + "%'";
                Tsql = Tsql + " OR  ItemCode Like '%" + Ncode.Trim() + "%') ";
            }

            Tsql = Tsql + " Order by Na_Code ASC , ItemCode ";


            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }



        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 9;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            //cgb.grid_Merge = true;
            //cgb.grid_Merge_Col_Start_index = 0;
            //cgb.grid_Merge_Col_End_index = 1;

            string[] g_HeaderText = { "상품코드" , "상품명" ,"국가코드" , "회원가"   , "PV"  
                                     , "CV"   , "기록자" , "기록일" 
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb.grid_cell_format = gr_dic_cell_format;


            int[] g_Width = { 100, 150, 70, 70, 100  
                             ,10, 10 , 10 , 0
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true  , true  , true 
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft                        
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  //5
                               
                               ,DataGridViewContentAlignment.MiddleRight                                
                               ,DataGridViewContentAlignment.MiddleLeft                               
                               ,DataGridViewContentAlignment.MiddleLeft   
                               ,DataGridViewContentAlignment.MiddleLeft   
                              };
            cgb.grid_col_alignment = g_Alignment;
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
                                 };
            gr_dic_text[fi_cnt + 1] = row0;
        }




        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, txt_ItemCode);


            if ((sender as DataGridView).CurrentRow.Cells[0].Value != null)
            {
                Data_Set_Form_TF = 1;
                DataGridView T_Gd = (DataGridView)sender;
                string t_ncode = T_Gd.CurrentRow.Cells[8].Value.ToString();

                if (T_Gd.CurrentRow.Cells[8].Value != null)
                {
                    txt_ItemName.Text = T_Gd.CurrentRow.Cells[1].Value.ToString();
                    txt_ItemCode.Text = T_Gd.CurrentRow.Cells[0].Value.ToString();

                    combo_Se_Code.Text = T_Gd.CurrentRow.Cells[2].Value.ToString();
                    combo_Se.SelectedIndex = combo_Se_Code.SelectedIndex;

                    txtPrice2.Text = T_Gd.CurrentRow.Cells[3].Value.ToString();
                    txtPrice4.Text = T_Gd.CurrentRow.Cells[4].Value.ToString();
                    txtPrice5.Text = T_Gd.CurrentRow.Cells[5].Value.ToString();

                    txtKey.Text = T_Gd.CurrentRow.Cells[8].Value.ToString();

                    combo_Se.Enabled = false;
                    txt_ItemCode.BackColor = cls_app_static_var.txt_Enable_Color;
                    txt_ItemCode.ReadOnly = true;
                    txt_ItemCode.BorderStyle = BorderStyle.FixedSingle;

                }

                Data_Set_Form_TF = 0;
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

                if (tb.Tag != null)
                {
                    if (tb.Tag.ToString() == "2" && tb.Text != "")
                    {
                        Data_Set_Form_TF = 1;
                        double T_p = double.Parse(tb.Text.Replace(",", "").ToString());
                        tb.Text = string.Format(cls_app_static_var.str_Currency_Type, T_p);
                        Data_Set_Form_TF = 0;
                    }
                }
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
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);

            TextBox tb = (TextBox)sender;

            if ((tb.Tag == null) || (tb.Tag.ToString() == ""))
            {
                //숫자만 입력 가능하다.
                if (T_R.Text_KeyChar_Check(e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if   
            }
            else if (tb.Tag.ToString() == "1") //숫자관련된 사항만 받아들이도록 셋팅을 함.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if (tb.Tag.ToString() == "2") //숫자관련된 사항만 받아들이도록 셋팅을 함.  순수 계산식의 숫자 관련해서
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1,1) == false)
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

        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            // SendKeys.Send("{TAB}");
        }


        private void txtData_TextChanged(object sender, EventArgs e)
        {
            int Sw_Tab = 0;
            if (Data_Set_Form_TF == 1) return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
            }

            if (tb.Name == "txt_Search")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    Base_Grid_Set();
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_ItemCode")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txt_ItemName.Text = "";
                Data_Set_Form_TF = 0;
            }
        }

        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
            if (tb.Name == "txt_Search")
            {
                if (tb.Text.Trim() != "")
                {
                    Data_Set_Form_TF = 1;
                    Base_Grid_Set(tb.Text);
                    Data_Set_Form_TF = 0;
                }
            }

            if (tb.Name == "txt_ItemCode")
            {
                Data_Set_Form_TF = 1;

                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, txt_ItemName, "");
                else
                    Ncod_Text_Set_Data(tb, txt_ItemName);

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
                if (tb.Name == "txt_ItemCode")
                {
                    cgb_Pop.db_grid_Popup_Base(4, "상품명", "상품코드", "개별단가", "개별PV", "Name", "Ncode", "price2", "price4", strSql);
                    cgb_Pop.Next_Focus_Control = combo_Se;
                }

            }
            else
            {


                if (tb.Name == "txt_ItemCode")
                {
                    string Tsql;
                    Tsql = "Select Name , NCode  ,price2 , price4  ";
                    Tsql = Tsql + " From ufn_Good_Search_01 ('" + cls_User.gid_date_time + "') ";
                    Tsql = Tsql + " Where NCode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%'";

                    cgb_Pop.db_grid_Popup_Base(4, "상품명", "상품코드", "개별단가", "개별PV", "Name", "Ncode", "price2", "price4", Tsql);

                    cgb_Pop.Next_Focus_Control = combo_Se;
                }


            }
        }





        private void Ncod_Text_Set_Data(TextBox tb, TextBox tb1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            if (tb.Name == "txt_ItemCode")
            {
                Tsql = "Select Name , NCode ,price2 ,price4    ";
                Tsql = Tsql + " From ufn_Good_Search_01 ('" + cls_User.gid_date_time + "') ";
                Tsql = Tsql + " Where NCode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%'";
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



        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }

        private void from_Date_Clear_()
        {
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            Base_Grid_Set();            
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, txt_ItemCode);
            Data_Set_Form_TF = 0;

            
            combo_Se.Enabled = true; 
            txt_ItemCode.BackColor = SystemColors.Window;
            txt_ItemCode.ReadOnly = false;
            txt_ItemCode.BorderStyle = BorderStyle.Fixed3D;

   
        }


        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            if (bt.Name == "butt_Clear")
            {
                from_Date_Clear_();
            }


            //저장 버튼 클릭시에
            else if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex; 

                Save_Base_Data(ref Save_Error_Check);  //저장이 일어나는 함수

                if (Save_Error_Check > 0)
                {
                    from_Date_Clear_();
                }

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            //삭제버튼 클릭시에
            else if (bt.Name == "butt_Delete")
            {
                int Del_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex; 

                Delete_Base_Data(ref Del_Error_Check); //삭제가 일어남.

                if (Del_Error_Check > 0)
                {
                    from_Date_Clear_();
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }


            //엑셀 전환 버튼 클릭시에
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
        }


        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Goods_Set";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }


        private Boolean Check_TextBox_Error(int i)
        {
            if (i != 2)  //삭제일 경우에만 체크를 한다.
            {
                if (txtKey.Text.Trim() == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select"));
                    dGridView_Base.Focus();
                    return false;
                }
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string Tsql;
            DataSet ds = new DataSet();

            Tsql = "Select ItemCode From tbl_SalesItemDetail (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesItemDetail.OrderNumber = tbl_SalesDetail.OrderNumber "; 
            Tsql = Tsql + " Where ItemCode ='" + txt_ItemCode.Text.Trim() + "'";
            Tsql = Tsql + " And  Na_Code ='" + combo_Se_Code.Text.Trim() + "'";

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_SalesItemDetail", ds) == false) return false;
            if (Temp_Connect.DataSet_ReCount != 0)//이미 매출 내역에 등록 된 상품이다. 그럼안됨.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Data")
                    + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Sell")
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_ItemCode.Select();
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
            System.Data.SqlClient.SqlConnection Conn = Temp_Connect.Conn_Conn();
            System.Data.SqlClient.SqlTransaction tran = Conn.BeginTransaction();

            string Tsql;

            try
            {
                Tsql = "Insert into  tbl_Goods_Change_Mod ";
                Tsql = Tsql + " Select * , 'D' ";
                Tsql = Tsql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Goods_Change ";
                Tsql = Tsql + " Where Ncode = '" + txt_ItemCode.Text.Trim() + "'";
                Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";


                Temp_Connect.Insert_Data(Tsql, base_db_name, Conn, tran);

                Tsql = "Insert into  tbl_Goods_Na_item_Mod ";
                Tsql = Tsql + " Select * , 'D' ";
                Tsql = Tsql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Goods_Na_item ";
                Tsql = Tsql + " Where ItemCode = '" + txt_ItemCode.Text.Trim() + "'";
                Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";

                Temp_Connect.Insert_Data(Tsql, base_db_name, Conn, tran);


                Tsql = "Delete From tbl_Goods_Change ";
                Tsql = Tsql + " Where Ncode = '" + txt_ItemCode.Text.Trim() + "'";
                Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";


                Temp_Connect.Delete_Data(Tsql, base_db_name, Conn, tran, this.Name.ToString(), this.Text);

                Tsql = "Delete From tbl_Goods_Na_item ";
                Tsql = Tsql + " Where ItemCode = '" + txt_ItemCode.Text.Trim() + "'";
                Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";


                Temp_Connect.Delete_Data(Tsql, base_db_name, Conn, tran, this.Name.ToString(), this.Text);


                tran.Commit();

                Del_Error_Check = 1;
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

            string me = T_R.Text_Null_Check(txt_ItemCode); //코드
            if (me != "")
            {
                MessageBox.Show(me); return false;
            }

            me = T_R.Text_Null_Check(txt_ItemName);    //제품명
            if (me != "")
            {
                MessageBox.Show(me); return false;
            }
                        
            if (txtPrice2.Text.Trim() == "") txtPrice2.Text = "0";
            if (txtPrice5.Text.Trim() == "") txtPrice5.Text = "0";            
            if (txtPrice4.Text.Trim() == "") txtPrice4.Text = "0";
            
            if (mtxtApplyDate.Text.Replace("-", "").Trim() != "")            
            {                
                if (Sn_Number_(mtxtApplyDate.Text, mtxtApplyDate, "Date") == false)
                {
                    mtxtApplyDate.Focus();
                    return false;
                }
            }

            return true;
        }


        private bool Check_TextBox_Error(string SaveCheck_2, ref int Chang_Price_TF)
        {
            SaveCheck_2 = "";
            Chang_Price_TF = 0;  //상품의 금액 관련 사항들이 변경을 했는지를 체크한다 변경하면 1
            string Tsql;

            if (txtKey.Text.Trim() == "")  //처음 인설트 할때는 동일한 이름과 동일한 코드로 이미 저장된 내역이 잇는지를 체크한다.
            {
                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                Tsql = "Select Na_Code, ItemCode ";
                Tsql = Tsql + " From tbl_Goods_Na_item  (nolock)  ";
                Tsql = Tsql + " Where ItemCode = '" + txt_ItemCode.Text.Trim() + "'";
                Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";
                Tsql = Tsql + " Order by ItemCode ASC ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 코드가 있다 그럼.이거 저장하면 안되요
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_Code") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txt_ItemCode.Select();
                    return false;
                }
                //++++++++++++++++++++++++++++++++
            }
            else
            {


                //변경 저장일 경우에는 동일한 코드는 다른데 동일한 이름으로 저장된 내역이 있는지 체크한다.
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();


                Tsql = "Select Ncode , Name  ";
                Tsql = Tsql + " , Isnull (( Select Top 1 price1 From tbl_Goods_Change (nolock) Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_Code ='" + combo_Se_Code.Text.Trim() + "' Order by ApplyDate DESC   ),price) Last_price ";
                Tsql = Tsql + " , Isnull (( Select Top 1 price2 From tbl_Goods_Change (nolock) Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_Code ='" + combo_Se_Code.Text.Trim() + "' Order by ApplyDate DESC   ),price2) Last_price2 ";
                Tsql = Tsql + " , Isnull (( Select Top 1 price3 From tbl_Goods_Change (nolock) Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_Code ='" + combo_Se_Code.Text.Trim() + "' Order by ApplyDate DESC   ),price4) Last_price3 ";
                Tsql = Tsql + " , Isnull (( Select Top 1 price4 From tbl_Goods_Change (nolock) Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_Code ='" + combo_Se_Code.Text.Trim() + "' Order by ApplyDate DESC   ),price4) Last_price4 ";
                Tsql = Tsql + " , Isnull (( Select Top 1 price5 From tbl_Goods_Change (nolock) Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_Code ='" + combo_Se_Code.Text.Trim() + "' Order by ApplyDate DESC   ),price5) Last_price5 ";
                Tsql = Tsql + " , Isnull (( Select Top 1 price6 From tbl_Goods_Change (nolock) Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_Code ='" + combo_Se_Code.Text.Trim() + "' Order by ApplyDate DESC   ),price6) Last_price6 ";
                Tsql = Tsql + " , Isnull (( Select Top 1 Except_Sell_VAT_Price From tbl_Goods_Change (nolock) Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_Code ='" + combo_Se_Code.Text.Trim() + "' Order by ApplyDate DESC   ),Except_Sell_VAT_Price) Last_Except_Sell_VAT_Price ";
                Tsql = Tsql + " , Isnull (( Select Top 1 Sell_VAT_Price From tbl_Goods_Change (nolock) Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_Code ='" + combo_Se_Code.Text.Trim() + "' Order by ApplyDate DESC   ),Sell_VAT_Price) Last_Sell_VAT_Price ";
                Tsql = Tsql + " From tbl_Goods  (nolock)  ";
                Tsql = Tsql + " Where Ncode = '" + txt_ItemCode.Text.Trim() + "'";
                Tsql = Tsql + " Order by Ncode ASC ";

                ds.Clear();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 이름이 있다 그럼.이거 저장하면 안되요
                {
                    if (double.Parse(ds.Tables[base_db_name].Rows[0]["Last_price5"].ToString()) != double.Parse(txtPrice5.Text.Replace(",", "")))
                        Chang_Price_TF = 1;
                    if (double.Parse(ds.Tables[base_db_name].Rows[0]["Last_price2"].ToString()) != double.Parse(txtPrice2.Text.Replace(",", "")))
                        Chang_Price_TF = 1;
                    if (double.Parse(ds.Tables[base_db_name].Rows[0]["Last_price4"].ToString()) != double.Parse(txtPrice4.Text.Replace(",", "")))
                        Chang_Price_TF = 1;                }

                //상품의 금액적인 내역이 변경이 일어낫다. 그럼 변경 적용일을 입력하게 한다.
                if ((Chang_Price_TF == 1) && (mtxtApplyDate.Text.Replace("_", "").Replace("-", "").Trim() == ""))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_ChangDate")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtApplyDate.Focus(); return false;
                }
            }

            return true;
        }





        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
            int Chang_Price_TF = 0;

            if (Check_TextBox_Error() == false) return;
            //if (Check_TextBox_Error(2) == false) return;  //상품관련 코드가 한군데에서라도 사용되었는지를 확인한다.          
            if (Check_TextBox_Error("Save_Err_Check_2", ref Chang_Price_TF) == false) return;


            if (txtKey.Text.Trim() == "")
            {
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                Temp_Connect.Connect_DB();
                System.Data.SqlClient.SqlConnection Conn = Temp_Connect.Conn_Conn();
                System.Data.SqlClient.SqlTransaction tran = Conn.BeginTransaction();

                string Tsql;

                try
                {
                    Tsql = "insert into tbl_Goods_Na_item ( ";
                    Tsql = Tsql + " Na_Code ,  ItemCode  , price2 ,price4 , price5 , Recordid , Recordtime   ";
                    Tsql = Tsql + ") Values ( ";

                    Tsql = Tsql + "'" + combo_Se_Code.Text.Trim() + "'";
                    Tsql = Tsql + ",'" + txt_ItemCode.Text.Trim() + "'";
                    Tsql = Tsql + "," + double.Parse(txtPrice2.Text.Replace(",", ""));  ;
                    Tsql = Tsql + "," + double.Parse(txtPrice4.Text.Replace(",", "")); ;
                    Tsql = Tsql + "," + double.Parse(txtPrice5.Text.Replace(",", "")); ;
                    Tsql = Tsql + ",'" + cls_User.gid + "'";
                    Tsql = Tsql + " , Convert(Varchar(25),GetDate(),21) ";
                    Tsql = Tsql + " ) ";

                    Temp_Connect.Insert_Data(Tsql, base_db_name, Conn, tran);                

                    tran.Commit();

                    Save_Error_Check = 1;
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
                }
                catch (Exception)
                {
                    tran.Rollback();
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));

                }

                finally
                {
                    tran.Dispose();
                    Temp_Connect.Close_DB();
                }

            }
            else //동일한 코드가 있구나 그럼 업데이트
            {
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

                if (Save_Base_Data_Up(Chang_Price_TF) == false) return;

                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
            }

        }

        private Boolean Save_Base_Data_Up(int Chang_Price_TF)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            string Tsql;

            try
            {


                Tsql = "Insert into  tbl_Goods_Na_item_Mod ";
                Tsql = Tsql + " Select * , 'U' ";
                Tsql = Tsql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Goods_Na_item ";
                Tsql = Tsql + " Where ItemCode = '" + txt_ItemCode.Text.Trim() + "'";
                Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";

                Temp_Connect.Insert_Data(Tsql, base_db_name, Conn, tran);



                //금액변동일 일어나고 날짜가 있다. 변경 할려고 하는
                if ((Chang_Price_TF == 1) && (mtxtApplyDate.Text.Replace("_", "").Replace("-", "").Trim() != ""))
                {
                    Tsql = "Select Ncode, ApplyDate ";
                    Tsql = Tsql + " From tbl_Goods_Change  (nolock)  ";
                    Tsql = Tsql + " Where upper(Ncode) = '" + ((txt_ItemCode.Text).Trim()).ToUpper() + "'";
                    Tsql = Tsql + " And  ApplyDate = '" + (mtxtApplyDate.Text).Trim().Replace("_", "").Replace("-", "") + "'";
                    Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";
                    Tsql = Tsql + " Order by ApplyDate DESC ";

                    DataSet ds = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Goods_Change", ds) == true)
                    {
                        if (Temp_Connect.DataSet_ReCount != 0)//동일한 이름으로 코드가 있다 그럼.이거 저장하면 안되요
                        {
                            Tsql = "Insert into tbl_Goods_Change_Mod ";
                            Tsql = Tsql + " Select * , 'U' ";
                            Tsql = Tsql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Goods_Change ";
                            Tsql = Tsql + " Where Ncode = '" + txt_ItemCode.Text.Trim() + "'";
                            Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";

                            Temp_Connect.Insert_Data(Tsql, "tbl_Goods_Change", Conn, tran);

                            Tsql = "Update tbl_Goods_Change Set ";                           
                            Tsql = Tsql + "  price2 = " + double.Parse(txtPrice2.Text.Replace(",", ""));
                            Tsql = Tsql + " ,price3 = 0 ";
                            Tsql = Tsql + " ,price4 = " + double.Parse(txtPrice4.Text.Replace(",", ""));
                            Tsql = Tsql + " ,price5 = " + double.Parse(txtPrice5.Text.Replace(",", ""));
                            Tsql = Tsql + " ,price6 = 0";

                            Tsql = Tsql + " WHERE Ncode = '" + txt_ItemCode.Text.Trim() + "'";
                            Tsql = Tsql + " And  ApplyDate = '" + (mtxtApplyDate.Text).Trim().Replace("_", "").Replace("-", "") + "'";
                            Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";

                            Temp_Connect.Update_Data(Tsql, Conn, tran, this.Name.ToString(), this.Text);
                        }
                        else
                        {
                            Tsql = "insert into tbl_Goods_Change ( ";
                            Tsql = Tsql + " ncode, name , Na_Code , ApplyDate , ";
                            Tsql = Tsql + " price1 , price2 , price3 , ";
                            Tsql = Tsql + " price4 , price5 , price6 , ";
                            Tsql = Tsql + " price7 , price8 , price9 , ";
                            Tsql = Tsql + " Sell_VAT_Price , Except_Sell_VAT_Price , ";
                            Tsql = Tsql + " recordid , recordtime ";
                            Tsql = Tsql + " ) values ( ";
                            Tsql = Tsql + " '" + txt_ItemCode.Text.Trim() + "'";
                            Tsql = Tsql + ",'" + txt_ItemName.Text.Trim() + "'";
                            Tsql = Tsql + ",'" + combo_Se_Code.Text.Trim() + "'";
                            Tsql = Tsql + ",'" + mtxtApplyDate.Text.Replace("_", "").Replace("-", "").Trim() + "'";

                            Tsql = Tsql + ",0" ;
                            Tsql = Tsql + "," + double.Parse(txtPrice2.Text.Replace(",", ""));
                            Tsql = Tsql + ",0 ";
                            Tsql = Tsql + "," + double.Parse(txtPrice4.Text.Replace(",", ""));
                            Tsql = Tsql + "," + double.Parse(txtPrice5.Text.Replace(",", ""));
                            Tsql = Tsql + ",0 ";

                            Tsql = Tsql + ",0 ";
                            Tsql = Tsql + ",0 ";
                            Tsql = Tsql + ",0 ";

                            Tsql = Tsql + ",0" ;
                            Tsql = Tsql + ",0";

                            Tsql = Tsql + ",'" + cls_User.gid + "'";
                            Tsql = Tsql + " , Convert(Varchar(25),GetDate(),21) ";
                            Tsql = Tsql + ")";

                            Temp_Connect.Insert_Data(Tsql, base_db_name, Conn, tran, this.Name.ToString(), this.Text);
                        }
                    }
                }


                tran.Commit();
                return true;

            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Err"));
                return false;
            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();

            }
        }


      


        


    }
}
