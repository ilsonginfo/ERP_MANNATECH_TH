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
    public partial class frmStock_Close_Cancel : clsForm_Extends
    {

        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_2 = new cls_Grid_Base();
        private int Data_Set_Form_TF = 0;
        private string base_db_name = "tbl_CloseTotal_01";
        private string FromEndDate = ""; private string ToEndDate = ""; private string PayDate = "", PayDate2 = "";
        private int From_Load_TF = 0;
   
        public frmStock_Close_Cancel()
        {
            InitializeComponent();
        }


        private void butt_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Close_Date_Search(); 

            Put_StocK_Close_Log();


            From_Load_TF = 0;
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Pay);
            cfm.button_flat_change(butt_Exit);

            txt_From.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_To.BackColor = cls_app_static_var.txt_Enable_Color;

            mtxtSDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            
            FromEndDate = ""; ToEndDate = ""; 
        }



        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            //if (From_Load_TF == 0)
            //{
            //    From_Load_TF = 1;

            //    //Check_Close_Date();

            //    //if (FromEndDate == "")
            //    //{
            //    //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not3_Close_Date"));
            //    //    this.Close();
            //    //    return;
            //    //}
                
            //}


        }



        private void frmBase_From_KeyDown(object sender, KeyEventArgs e)
        {
            //폼일 경우에는 ESC버튼에 폼이 종료 되도록 한다
            if (sender is Form)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (!this.Controls.ContainsKey("Popup_gr"))
                    {
                        this.Close();
                        return;
                    }
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
                            return;
                            // cls_form_Meth cfm = new cls_form_Meth();
                            // cfm.form_Group_Panel_Enable_True(this);
                        }
                    }
                }// end if

            }

            Button T_bt = butt_Exit;
            if (e.KeyValue == 123)
                T_bt = butt_Exit;    //닫기  F12          

            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123)
                    butt_Exit_Click(T_bt, ee1);
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






        private void MtxtData_Temp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    Data_Set_Form_TF = 1;
                    int SW = 0;
                    string Sn = mtb.Text.Replace("-", "").Replace("_", "").Trim();
                    string R4_name = mtb.Name.Substring(mtb.Name.Length - 4, 4);
                    if (R4_name == "Date" || R4_name == "ate3" || R4_name == "ate1" || R4_name == "ate2" || R4_name == "ate4")
                    {
                        SW = 1;
                        if (Sn_Number_(Sn, mtb, "Date") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel1")
                    {
                        SW = 1;
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel2")
                    {
                        SW = 1;
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtZip1")
                    {
                        SW = 1;
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    Data_Set_Form_TF = 0;
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
                    cls_Check_Input_Error c_er = new cls_Check_Input_Error();
                    if (c_er.Input_Date_Err_Check__01(mtb) == false)
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
            //SendKeys.Send("{TAB}");
        }

        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);
            //T_R.Key_Enter_13_Name += new Key_13_Name_Event_Handler(T_R_Key_Enter_13_Name);

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

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "1"))
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1) == false)
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

            //if (tb.Name == "txtCenter")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtCenter_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtCenter_Code);
            //}

            //if (tb.Name == "txtBank")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtSellCode_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);
            //}

            //if (tb.Name == "txtR_Id")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtR_Id_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code);
            //}

            if (tb.Name == "txtCenter")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter_Code.Text = "";
                Data_Set_Form_TF = 0;
            }


            //if (tb.Name == "txt_ItemName2")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.Trim() == "")
            //        txt_ItemName_Code2.Text = "";
            //    Data_Set_Form_TF = 0;
            //}

            //if (tb.Name == "txtSellCode")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtSellCode_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);
            //}
        }



        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
            //if (tb.Name == "txtCenter")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtCenter_Code,"");
            //    else
            //        Ncod_Text_Set_Data(tb, txtCenter_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            //if (tb.Name == "txtR_Id")
            //{
            //    Data_Set_Form_TF = 1;
            //    Db_Grid_Popup(tb, txtR_Id_Code);
            //    //if (tb.Text.ToString() == "")
            //    //    Db_Grid_Popup(tb, txtR_Id_Code, "");
            //    //else
            //    //    Ncod_Text_Set_Data(tb, txtR_Id_Code);

            //    //SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            //if (tb.Name == "txtBank")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtSellCode_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            if (tb.Name == "txtCenter")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtCenter3_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter3_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            //if (tb.Name == "txt_ItemName2")
            //{
            //    Data_Set_Form_TF = 1;
            //    Db_Grid_Popup(tb, txt_ItemName_Code2);
            //    //if (tb.Text.ToString() == "")
            //    //    Db_Grid_Popup(tb, txt_ItemName_Code2, "");
            //    //else
            //    //    Ncod_Text_Set_Data(tb, txt_ItemName_Code2);

            //    //SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            //if (tb.Name == "txtSellCode")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtSellCode_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}
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
                cgb_Pop.Next_Focus_Control = butt_Pay;

            if (tb.Name == "txtCenter2")
                cgb_Pop.Next_Focus_Control = butt_Pay;

            if (tb.Name == "txtCenter3")
                cgb_Pop.Next_Focus_Control = butt_Pay;


            if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = butt_Pay;

            if (tb.Name == "txtR_Id")
                cgb_Pop.Next_Focus_Control = butt_Pay;

            if (tb.Name == "txtChange")
                cgb_Pop.Next_Focus_Control = butt_Pay;

            if (tb.Name == "txtSellCode")
                cgb_Pop.Next_Focus_Control = butt_Pay;

            if (tb.Name == "txt_Base_Rec")
                cgb_Pop.Next_Focus_Control = butt_Pay;

            if (tb.Name == "txt_Receive_Method")
                cgb_Pop.Next_Focus_Control = butt_Pay;

            if (tb.Name == "txt_ItemCode")
                cgb_Pop.Next_Focus_Control = butt_Pay;

            if (tb.Name == "txt_ItemName2")
                cgb_Pop.Next_Focus_Control = butt_Pay;

            if (tb.Name == "txtCenter4")
                cgb_Pop.Next_Focus_Control = butt_Pay;

            if (tb.Name == "txtIO")
            {
                cgb_Pop.Next_Focus_Control = butt_Pay;
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode, "", " And  (Ncode ='004' OR Ncode = '005' OR Ncode = '006'  ) ");
            }
            else
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
            //Tsql = Tsql + " And  (Ncode ='004' OR Ncode = '005' ) ";


        }


        private void Close_Date_Search()
        {
            dGridView_Base_Header_Reset();
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string strSQL;

            strSQL = "Select Distinct LEFT(StockDay,4) +'-' + LEFT(RIGHT(StockDay,4),2) + '-' + RIGHT(StockDay,2) ";
            strSQL = strSQL + " From  DayStock  (nolock)   ";           

            if (txtCenter_Code.Text != "")
                strSQL = strSQL + " Where CenterCode ='" + txtCenter_Code.Text.Trim() + "'" ;

            strSQL = strSQL + " Order by LEFT(StockDay,4) +'-' + LEFT(RIGHT(StockDay,4),2) + '-' + RIGHT(StockDay,2) DESC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSQL, base_db_name, ds) == false) return;
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
            dGridView_Base.RowHeadersVisible = false;
            cgb.grid_col_Count = 10;            
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.CellSelect ;
            //cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            string[] g_HeaderText = { "재고마감된날짜", "", "" , "" ,"" , 
                                      ""   , "" , ""   ,"" , ""                                     
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            

            int[] g_Width = { 130, 0 , 0 , 0, 0,
                              0, 0, 0, 0, 0                             
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , false,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true   
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleLeft                        
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft  //5
                               
                               ,DataGridViewContentAlignment.MiddleRight                                
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight //10

                              };
            cgb.grid_col_alignment = g_Alignment;
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][3]                                
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][4]
 
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][7]
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][9]                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;

        }
        


        private void Put_StocK_Close_Log()
        {
             dGridView_Base_2_Header_Reset();
            cgb_2.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string strSQL;

            strSQL = "Select LogType ,";
            strSQL = strSQL + " LEFT(CloseDay,4) +'-' + LEFT(RIGHT(CloseDay,4),2) + '-' + RIGHT(CloseDay,2) ,  " ;
            strSQL = strSQL + " isnull(tbl_Business.name,'') ,  ";
            strSQL = strSQL + " RegiUser , RegiDay ";
            strSQL = strSQL + " From  CloseLog (nolock) ";
            strSQL = strSQL + " LEFT JOIN tbl_Business  (nolock) on tbl_Business.ncode = CloseLog.CenterCode ";

            if (txtCenter_Code.Text != "")
                strSQL = strSQL + " Where CenterCode ='" + txtCenter_Code.Text.Trim() + "'" ;

            strSQL = strSQL + " Order by RegiDay DESC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSQL, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_2_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb_2.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_2.db_grid_Obj_Data_Put();            
        }




        private void dGridView_Base_2_Header_Reset()
        {
            //dGridView_Base_2.RowHeadersVisible = false;
            cgb_2.grid_col_Count = 10;            
            cgb_2.basegrid = dGridView_Base_2;
            cgb_2.grid_select_mod = DataGridViewSelectionMode.CellSelect ;
            //cgb_2.grid_Frozen_End_Count = 2;
            cgb_2.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            string[] g_HeaderText = { "구분", "마감일", "센타" , "기록자" ,"기록시간" , 
                                      ""   , "" , ""   ,"" , ""                                     
                                    };
            cgb_2.grid_col_header_text = g_HeaderText;

            

            int[] g_Width = { 35, 100 , 60 , 110, 70,
                              0, 0, 0, 0, 0                             
                            };
            cgb_2.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , false,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true   
                                   };
            cgb_2.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleLeft                        
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft  //5
                               
                               ,DataGridViewContentAlignment.MiddleRight                                
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight //10

                              };
            cgb_2.grid_col_alignment = g_Alignment;
        }


        private void Set_gr_2_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]                                
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]
 
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][7]
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt][9]                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;

        }


        private Boolean Check_TextBox_Error()
        {

            cls_Check_Input_Error c_er = new cls_Check_Input_Error();

           

            if (mtxtSDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSDate2.Text, mtxtSDate2, "Date") == false)
                {
                    mtxtSDate2.Focus();
                    return false;
                }

            }
            
            if (mtxtSDate2.Text.Replace("-", "").Trim() == "" )
            {
                MessageBox.Show("취소할 마감 시작일을 입력해 주십시요."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                mtxtSDate2.Focus(); return false;
            }
            

            txt_From.Text = mtxtSDate2.Text.Replace("-", "").Trim();

            if (int.Parse(txt_From.Text) > int.Parse(txt_To.Text))
            {
                MessageBox.Show("취소할 마감 시작일이 취소할 마감 종료일 이후의 날짜일수 없습니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_From.Text = "";
                mtxtSDate2.Focus(); return false;
            }

            return true;
        }




        private void butt_Pay_Click(object sender, EventArgs e)
        {
            if (Check_TextBox_Error() == false) return;

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_Cancel_Start"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            
            butt_Pay.Enabled = false; butt_Exit.Enabled = false;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            
                try
                {
                    Close_Work_Real(Temp_Connect, Conn, tran);

                    tran.Commit();
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_End"));
                   
                }
                catch (Exception)
                {
                    tran.Rollback();
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_Err"));
                    
                }

                finally
                {
                    tran.Dispose(); Temp_Connect.Close_DB();
                    this.Cursor = System.Windows.Forms.Cursors.Default ;
                    butt_Pay.Enabled = true; butt_Exit.Enabled = true;

                    Close_Date_Search();

                    Put_StocK_Close_Log();
                }
             
        }


        private void Close_Work_Real(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string StrSql = "";
            if (txtCenter_Code.Text.Trim() != "")
            {
                StrSql = "Delete From  DayStock ";
                StrSql = StrSql + " Where StockDay >='" + txt_From.Text.Trim() + "'";
                StrSql = StrSql + " And   StockDay <='" + txt_To.Text.Trim() + "'";
                StrSql = StrSql + " And    CenterCode ='" + txtCenter_Code.Text.Trim() + "'";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);

                StrSql = "Insert into CloseLog (CloseDay,CloseDay2,LogType,CenterCode,  RegiUser , RegiDay ) Values (";
                StrSql = StrSql + "'" + txt_From.Text.Trim() + "','" + txt_To.Text.Trim() + "','재고취소','" + txtCenter_Code.Text.Trim() + "','" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21))";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);

            }
            else
            {

                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Search_Connect = new cls_Connect_DB();

                StrSql = " Select Distinct CenterCode From DayStock  (nolock) ";
                StrSql = StrSql + " Where StockDay >='" + txt_From.Text.Trim() + "'";
                StrSql = StrSql + " And   StockDay <='" + txt_To.Text.Trim() + "'";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Search_Connect.Open_Data_Set(StrSql, base_db_name, ds) == false) return;
                int ReCnt = Search_Connect.DataSet_ReCount;

                if (ReCnt > 0)
                {
                    for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                    {
                        StrSql = "Insert into CloseLog (CloseDay,CloseDay2,LogType,CenterCode,  RegiUser , RegiDay ) Values (";
                        StrSql = StrSql + "'" + txt_From.Text.Trim() + "','" + txt_To.Text.Trim() + "','재고취소','" + ds.Tables[base_db_name].Rows[fi_cnt][0].ToString() + "','" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21))";

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    }
                }


                StrSql = "Delete From  DayStock ";
                StrSql = StrSql + " Where StockDay >='" + txt_From.Text.Trim() + "'";
                StrSql = StrSql + " And   StockDay <='" + txt_To.Text.Trim() + "'";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            
            

        }

        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.CurrentRow != null && dgv.CurrentRow.Cells[0].Value != null)
            {
                if (dgv.CurrentRow.Cells[0].Value.ToString() != "")
                    txt_To.Text = dgv.CurrentRow.Cells[0].Value.ToString().Replace("-", ""); 
            }
        }










    }
}
