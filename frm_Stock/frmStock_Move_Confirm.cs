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
    public partial class frmStock_Move_Confirm : clsForm_Extends
    {
        
        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cg_Sub = new cls_Grid_Base();

        private const string base_db_name = "tbl_Stock_Move_Sub";
        private int Data_Set_Form_TF;

       public frmStock_Move_Confirm()
        {
            InitializeComponent();
        }


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;
            tableLayoutPanel38.Enabled = false;
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            mtxtCDate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMDate3.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtMakDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMakDate2.Mask = cls_app_static_var.Date_Number_Fromat;


            txtCenter2.BackColor = cls_app_static_var.txt_Enable_Color;
            txtCenter.BackColor = cls_app_static_var.txt_Enable_Color;
            txtMDate.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_M_Cnt.BackColor = cls_app_static_var.txt_Enable_Color;


            Data_Set_Form_TF = 1;
            //txtMDate.Text = DateTime.Now.ToString("yyyyMMdd");
            Data_Set_Form_TF = 0;      
            
            if(cls_User.gid == cls_User.SuperUserID)
            {
                butt_Delete.Visible = true;
            }

        }

        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();
        }


        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Save.Left = butt_Clear.Left + butt_Clear.Width + 2;
            //butt_Excel.Left = butt_Save.Left + butt_Save.Width + 2;
            butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Save);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);

            cfm.button_flat_change(butt_Search);
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
                T_bt = butt_Clear;    //리셋  F5    

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

            if (tb.Name == "txtCenter2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter_Code2.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtCenter3")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter_Code3.Text = "";
                Data_Set_Form_TF = 0;
            }


            if (tb.Name == "txtCenter4")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter_Code4.Text = "";
                Data_Set_Form_TF = 0;
            }


            if (tb.Name == "txtR_Id")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtR_Id_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtR_Id2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtR_Id_Code2.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_ItemName2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txt_ItemName_Code2.Text = "";
                Data_Set_Form_TF = 0;
            }

            
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
                Db_Grid_Popup(tb, txtCenter_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtCenter_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtCenter2")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter_Code2);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtCenter_Code2, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter_Code2);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtCenter3")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter_Code3);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtCenter_Code3, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter_Code3);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }


            if (tb.Name == "txtCenter4")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter_Code4);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtCenter_Code4, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter_Code4);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }



            if (tb.Name == "txtR_Id")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtR_Id_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtR_Id_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtR_Id_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtR_Id2")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtR_Id_Code2);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtR_Id_Code2, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtR_Id_Code2);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_ItemName2")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txt_ItemName_Code2);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txt_ItemName_Code2, "");
                //else
                //    Ncod_Text_Set_Data(tb, txt_ItemName_Code2);

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

            if (tb.Name == "txtCenter")
                cgb_Pop.Next_Focus_Control = txtR_Id;

            if (tb.Name == "txtCenter2")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txtR_Id")
                cgb_Pop.Next_Focus_Control = txtRemark;

            if (tb.Name == "txtChange")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txtSellCode")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txt_Base_Rec")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txt_Receive_Method")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txt_ItemCode")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txt_ItemName2")
                cgb_Pop.Next_Focus_Control = butt_Search;

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
                if (tb.Name == "txtCenter" || tb.Name == "txtCenter2" || tb.Name == "txtCenter3" || tb.Name == "txtCenter4")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2")
                {
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);
                    if (tb.Name == "txtR_Id")
                        cgb_Pop.Next_Focus_Control = txtRemark;
                }
                if (tb.Name == "txt_ItemName2")
                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", strSql);
            }
            else
            {
                if (tb.Name == "txtCenter" || tb.Name == "txtCenter2" || tb.Name == "txtCenter3" || tb.Name == "txtCenter4")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " And  ShowOrderCenter = 'Y' ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                }

                if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2")
                {
                    string Tsql;
                    Tsql = "Select user_id ,U_Name   ";
                    Tsql = Tsql + " From tbl_User (nolock) ";
                    Tsql = Tsql + " Order by user_id ";

                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", Tsql);

                    if (tb.Name == "txtR_Id")
                        cgb_Pop.Next_Focus_Control = txtRemark;
                }

                if (tb.Name == "txt_ItemName2")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name    ";
                    Tsql = Tsql + " From tbl_Goods (nolock) ";
                    //Tsql = Tsql + " Where GoodUse = 0 ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", Tsql);
                }

            }
        }



        private void Ncod_Text_Set_Data(TextBox tb, TextBox tb1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            if (tb.Name == "txtCenter" || tb.Name == "txtCenter2" || tb.Name == "txtCenter3" || tb.Name == "txtCenter4" )
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
            }

            if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2")
            {
                Tsql = "Select user_id ,U_Name   ";
                Tsql = Tsql + " From tbl_User (nolock) ";
                Tsql = Tsql + " Where U_Name like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    user_id like '%" + tb.Text.Trim() + "%'";
            }

            if (tb.Name == "txt_ItemName2")
            {
                Tsql = "Select Ncode , Name    ";
                Tsql = Tsql + " From tbl_Goods (nolock) ";
                Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
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


















        private void Form_Clear_()
        {  
            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //dGridView_Base_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cg_Sub.d_Grid_view_Header_Reset();            
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(panel15, mtxtCDate);
            mtxtMDate2.Text = "";
            mtxtMDate3.Text = "";
            txtCenter3.Text = "";
            txt_ItemName2.Text = "";
            txtCenter4.Text = "";
            mtxtMakDate1.Text = "";
            mtxtMakDate2.Text = "";
            mtxtCDate.Focus();
        }






        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {
                Form_Clear_();    
            }

            else if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Save_Base_Data(ref Save_Error_Check);

                if (Save_Error_Check > 0)
                {
                    Form_Clear_();
                                        
                    Base_Sub_Grid_Set();  //뿌려주는 곳                    
                }
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
                    Form_Clear_();

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }                
            else if (bt.Name == "butt_Search")
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Base_Sub_Grid_Set();  //뿌려주는 곳
                this.Cursor = System.Windows.Forms.Cursors.Default;           
            }

            else if (bt.Name == "butt_Excel")
            {
                frmBase_Excel e_f = new frmBase_Excel();
                e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
                e_f.ShowDialog();
            }

        }


        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Stock_Move";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base_Sub;
        }




















        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);            
        }






        private void Base_Sub_Grid_Set()
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 


            cls_Check_Input_Error c_er = new cls_Check_Input_Error();


            if (mtxtMDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtMDate2.Text, mtxtMDate2, "Date") == false)
                {
                    mtxtMDate2.Focus();                    return ;
                }
            }

            if (mtxtMDate3.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtMDate3.Text, mtxtMDate3, "Date") == false)
                {
                    mtxtMDate3.Focus(); return;
                }
            }


            if (mtxtMakDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtMakDate1.Text, mtxtMakDate1, "Date") == false)
                {
                    mtxtMakDate1.Focus(); return;
                }
            }
          

            if (mtxtMakDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtMakDate2.Text, mtxtMakDate2, "Date") == false)
                {
                    mtxtMakDate2.Focus(); return;
                }
            }


          


            string Tsql = "";

            //string[] g_HeaderText = {"입고번호"  , "입고일자"   , "상품코드"  , "상품명"   , "입고지"        
            //                        , "입고수량"   , "입고자"    , "비고"   , ""    , ""                                
            //                            };

            Tsql = "Select Move_From_Dep_Cd ";
            Tsql = Tsql + " ,Isnull(F_B.Name,'') ";
            Tsql = Tsql + " ,LEFT(Move_Date,4) +'-' + LEFT(RIGHT(Move_Date,4),2) + '-' + RIGHT(Move_Date,2) Move_Date ";
            Tsql = Tsql + " ,Move_To_Dep_Cd ";
            Tsql = Tsql + " ,Isnull(T_B.Name,'') ";
            Tsql = Tsql + " ,M_itemCode ";
            Tsql = Tsql + " ,Isnull(tbl_Goods.name,'') ";
            Tsql = Tsql + " ,M_Cnt ";
            Tsql = Tsql + " ,M_ID ";
            Tsql = Tsql + " ,Remarks1 ";

            Tsql = Tsql + " ,M_index  ";
            Tsql = Tsql + " ,Isnull(tbl_User.U_Name ,'' )  ";

            Tsql = Tsql + " ,LEFT(D_Date,4) +'-' + LEFT(RIGHT(D_Date,4),2) + '-' + RIGHT(D_Date,2) D_Date   ";
            Tsql = Tsql + " ,D_ID  ";
            Tsql = Tsql + " ,D_Cnt  ";
            Tsql = Tsql + " ,D_Remarks1  ";
            
            Tsql = Tsql + " ,tbl_Stock_Move_Sub.Recordid  ";
            Tsql = Tsql + " ,tbl_Stock_Move_Sub.RecordTime, Isnull(T_U2.U_Name ,'' ) D_Name,'' ";

            Tsql = Tsql + " From tbl_Stock_Move_Sub (nolock) ";
            Tsql = Tsql + " LEFT Join tbl_Goods  (nolock)  ON tbl_Stock_Move_Sub.M_itemCode = tbl_Goods.Ncode ";
            Tsql = Tsql + " LEFT Join tbl_Business  F_B  (nolock) ON F_B.Ncode = tbl_Stock_Move_Sub.Move_From_Dep_Cd  ";
            Tsql = Tsql + " LEFT Join tbl_Business  T_B  (nolock) ON T_B.Ncode = tbl_Stock_Move_Sub.Move_To_Dep_Cd  ";
            Tsql = Tsql + " LEFT Join tbl_User  (nolock)  ON tbl_User.User_id = tbl_Stock_Move_Sub.M_ID  ";
            Tsql = Tsql + " LEFT Join tbl_User T_U2 (nolock)  ON T_U2.User_id = tbl_Stock_Move_Sub.D_ID  ";

            Tsql = Tsql + " Where Move_Date <> '' ";


            Tsql = Tsql + " And ( tbl_Stock_Move_Sub.Move_To_Dep_Cd in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";

            Tsql = Tsql + " OR   tbl_Stock_Move_Sub.Move_From_Dep_Cd in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
            Tsql = Tsql + ") ";

            if (mtxtMDate2.Text.Replace("-", "").Trim().Length == 8)
                Tsql = Tsql + " And Move_Date = '" + mtxtMDate2.Text.Replace("-", "").Trim() + "'";


            if (mtxtMDate2.Text.Replace("-", "").Trim() != "" && mtxtMDate3.Text.Replace("-", "").Trim() != "")
            {
                Tsql = Tsql + " And Move_Date >= '" + mtxtMDate2.Text.Replace("-", "").Trim() + "'";
                Tsql = Tsql + " And Move_Date <= '" + mtxtMDate3.Text.Replace("-", "").Trim() + "'";
            }


             //기록일자로 검색 -1
            if ((mtxtMakDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakDate2.Text.Replace("-", "").Trim() == ""))
                Tsql = Tsql + " And tbl_Stock_Move_Sub.D_Date = '" + mtxtMakDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtMakDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakDate2.Text.Replace("-", "").Trim() != ""))
            {
                Tsql = Tsql + " And tbl_Stock_Move_Sub.D_Date >= '" + mtxtMakDate1.Text.Replace("-", "").Trim() + "'";
                Tsql = Tsql + " And tbl_Stock_Move_Sub.D_Date <= '" + mtxtMakDate2.Text.Replace("-", "").Trim() + "'";
            }




            if (txtCenter_Code3.Text != "" )
                Tsql = Tsql + " And Move_From_Dep_Cd = '" + txtCenter_Code3.Text + "'";

            if (txtCenter_Code4.Text != "")
                Tsql = Tsql + " And Move_To_Dep_Cd = '" + txtCenter_Code4.Text + "'";

            if (txt_ItemName_Code2.Text != "")
                Tsql = Tsql + " And M_itemCode = '" + txt_ItemName_Code2.Text + "'";

            if (txtR_Id_Code2.Text != "")
                Tsql = Tsql + " And M_ID = '" + txtR_Id_Code2.Text + "'";

            if (opt_confirm_1.Checked == true)
                Tsql = Tsql + " And D_Date = ''";

            if (opt_confirm_2.Checked == true)
                Tsql = Tsql + " And D_Date <> ''";

            Tsql = Tsql + " Order by Move_Date , Move_From_Dep_Cd , Move_To_Dep_Cd , M_itemCode ";

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
                Set_Sub_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cg_Sub.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cg_Sub.db_grid_Obj_Data_Put();
        }


        private void Set_Sub_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;
            object[] row0 = new object[cg_Sub.grid_col_Count];

            while (Col_Cnt < cg_Sub.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;
        }



        private void dGridView_Base_Sub_Header_Reset()
        {
            

            cg_Sub.grid_col_Count = 20;
            cg_Sub.basegrid = dGridView_Base_Sub;
            cg_Sub.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_Sub.grid_Frozen_End_Count = 3;
            cg_Sub.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

              string[] g_HeaderText = {"출고지",  "출고지명"  , "이동일자"   , "입고지"  , "입고지명"          
                                , "상품코드"  , "상품명"   , "요청수량"    , "요청자"   , "비고"       
                                , ""    , ""   , "확정일자"    , "확정자"   , "확정수량"   
                                , "확정비고"    , "기록일"   , "기록자"    , ""   , ""   
                                };
            cg_Sub.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 70,  120, 90, 70, 120                             
                             , 100 ,120 , 90 ,  120 , 300  
                             ,0 , 0 ,  100 , 130 ,  100   
                             ,130 , 100 ,  100 , 0 ,  0 
                            };
            cg_Sub.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                         
                                    ,true , true,  true,  true ,true    
                                    ,true , true,  true,  true ,true    
                                   };
            cg_Sub.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft //10    
                               
                                ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleRight //15  

                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //20
                              };
            cg_Sub.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cg_Sub.grid_cell_format = gr_dic_cell_format;           
        }




















        private bool  Check_TextBox_Error_Date()
        {
            cls_Check_Input_Error c_er = new cls_Check_Input_Error();
            if (txtMDate.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtMDate);

                if (Ret == -1)
                {
                    txtMDate.Focus(); return false;
                }
            }


            if (mtxtCDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtCDate.Text, mtxtCDate, "Date") == false)
                {
                    mtxtCDate.Focus();
                    return false;
                }
            }
                      
            return true;
        }


        
        private Boolean Check_TextBox_Error()
        {
            cls_Check_Text T_R = new cls_Check_Text();
            string me = "";



            me = T_R.Text_Null_Check(txtCenter_Code, "Msg_Sort_Stock_Out_Center"); //출고지지를
            if (me != "")
            {
                MessageBox.Show(me);
                return false;
            }

            me = T_R.Text_Null_Check(txtCenter_Code2, "Msg_Sort_Stock_IN_Center"); //입고지를
            if (me != "")
            {
                MessageBox.Show(me);
                return false;
            }


            me = T_R.Text_Null_Check(txtMDate, "Msg_Sort_Stock_Move_Date"); //입고일자를
            if (me != "")
            {
                MessageBox.Show(me);
                return false;
            }

            if (txt_D_Cnt.Text.Trim() == "")    txt_D_Cnt.Text = "0"; 

          
            //날짜 관련 텍스트 파일들에 대해서 날짜 오류를 체크한다
            if (Check_TextBox_Error_Date() == false) return false;

            int SW  = 0 ; 
            
            //확정 관련해서 입력한 내역이 아무것도 없다 그럼 취소로 본다. 
            if (txtR_Id_Code.Text.Trim() == "" &&  mtxtCDate.Text.Replace ("-","").Trim() == ""  && int.Parse(txt_D_Cnt.Text) == 0)            
                SW = 1 ;
            


            //확정 관련해서 제대로 다 입력을 했는지 체크를한다.
            if (txtR_Id_Code.Text.Trim() != "" && mtxtCDate.Text.Replace("-", "").Trim() != "" && int.Parse(txt_D_Cnt.Text) > 0)
                SW = 1 ;


            if (SW == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Stock_M_Confirm_Check__01")
                  + "\n" +
                  cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtCDate.Focus(); return false;
            }


            cls_Search_DB csd = new cls_Search_DB();
            if (csd.Check_Stock_Close(txtCenter_Code.Text, mtxtCDate.Text.Replace("-", "").Trim()) == false)
            {
                txtCenter.Focus();
                return false;
            }

            if (csd.Check_Stock_Close(txtCenter_Code2.Text, mtxtCDate.Text.Replace("-", "").Trim()) == false)
            {
                txtCenter2.Focus();
                return false;
            }


            return true;
        }



        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
            string str_Q = "";

            if (txtKey.Text == "")
                str_Q = "Msg_Base_Save_Q";
            else
                str_Q = "Msg_Base_Edit_Q";

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString(str_Q), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            
            if (Check_TextBox_Error() == false) return;
                          
            if (txtKey.Text != "") //수정일 경우에는 수정 프로시져로 가고 이 프로시져를 빠져나가라
            {
                Save_Base_Data_UpDate(ref Save_Error_Check);
                if (Save_Error_Check > 0)
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
                return;
            }                  
            
            //cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            //Temp_Connect.Connect_DB();
            //SqlConnection Conn = Temp_Connect.Conn_Conn();
            //SqlTransaction tran = Conn.BeginTransaction();
            
           
            //try
            //{               

            //    //string StrSql = "";// string T_Or = ""; string Out_Index = "";
            //    //int ItemCnt = 0; string ItemCode = ""; int Out_Price = 0;
            //    //for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            //    //{
            //    //    if (int.Parse (dGridView_Base.Rows[i].Cells[0].Value.ToString()) > 0)
            //    //    {                       
            //    //        ItemCnt = int.Parse(dGridView_Base.Rows[i].Cells[0].Value.ToString());
            //    //        ItemCode = dGridView_Base.Rows[i].Cells[2].Value.ToString();
            //    //        Out_Price = int.Parse( dGridView_Base.Rows[i].Cells[3].Value.ToString()) ;

            //    //        StrSql = "Insert into tbl_Stock_Move_Sub (";
            //    //        StrSql = StrSql + " Move_From_Dep_Cd";
            //    //        StrSql = StrSql + " ,Move_To_Dep_Cd ";
            //    //        StrSql = StrSql + " ,Move_Date";
            //    //        StrSql = StrSql + " ,M_itemCode ";
            //    //        StrSql = StrSql + " ,M_Cnt ";
            //    //        StrSql = StrSql + " ,Remarks1 ";
            //    //        StrSql = StrSql + " ,M_ID ";
            //    //        StrSql = StrSql + " ,RecordId ";
            //    //        StrSql = StrSql + " ,RecordTime ";
            //    //        StrSql = StrSql + " )";
            //    //        StrSql = StrSql + " Values " ;
            //    //        StrSql = StrSql + " (";

            //    //        StrSql = StrSql + "'" + txtCenter_Code.Text.Trim() + "'";  //센타/창고 코드 번호
            //    //        StrSql = StrSql + ",'" + txtCenter_Code2.Text.Trim() + "'";  //센타/창고 코드 번호
            //    //        StrSql = StrSql + ",'" + txtMDate.Text.Trim() + "'";   //입고번호
            //    //        StrSql = StrSql + ",'" + ItemCode + "'";       //상품코드
            //    //        StrSql = StrSql + "," + ItemCnt;      //입고수량                        

            //    //        StrSql = StrSql + ",'" + txtRemark.Text.Trim() + "'";       //비고1
            //    //        StrSql = StrSql + ",'" + txtR_Id_Code.Text.Trim() + "'";      //입고자

            //    //        StrSql = StrSql + ",'" + cls_User.gid  + "'";
            //    //        StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                
            //    //        StrSql = StrSql + ")"  ;

            //    //        Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);
            //    //    }
            //    //}

            //    tran.Commit();

            //    Save_Error_Check = 1;                
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));


            //}
            //catch (Exception)
            //{
            //    tran.Rollback();
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
            //}

            //finally
            //{
            //    tran.Dispose();
            //    Temp_Connect.Close_DB();
            //}            

        }




        private void Save_Base_Data_UpDate(ref int Save_Error_Check)
        {                 
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();
                        
            try
            {               
                string StrSql = "";                 

                StrSql = "INSERT INTO tbl_Stock_Move_Sub_Del ";
                StrSql = StrSql + " Select  * ";
                StrSql = StrSql + ",'" + cls_User.gid + "'";
                StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                StrSql = StrSql + " From tbl_Stock_Move_Sub ";
                StrSql = StrSql + " Where M_index = '" + txtKey.Text.Trim() + "'";

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                StrSql = "UpDate  tbl_Stock_Move_Sub Set ";
                StrSql = StrSql + "  D_ID = '" + txtR_Id_Code.Text.Trim() + "'";
                StrSql = StrSql + ", D_Date = '" + mtxtCDate.Text.Replace("-", "").Trim() + "'";
                StrSql = StrSql + ", D_Cnt  = " + txt_D_Cnt.Text.Trim() ;
                StrSql = StrSql + ", D_Remarks1 = '" + txtRemark.Text.Trim () + "'";
                
                StrSql = StrSql + " Where M_index ='" + txtKey.Text + "'";


                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);
                         
                tran.Commit();

                Save_Error_Check = 1;
                //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
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







        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Delete_Base_Data(ref int Delete_Error_Check)
        {
            Delete_Error_Check = 0;
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (txtKey.Text.Trim() == "")
            {
                return;
            }
           
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                string StrSql = "";

                StrSql = "Insert into  tbl_Stock_Move_Sub_Del ";
                StrSql = StrSql + " Select *  ";
                StrSql = StrSql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Stock_Move_Sub (nolock) ";
                StrSql = StrSql + " Where M_index = " + txtKey.Text.Trim();
                
                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                StrSql = "Delete From tbl_Stock_Move_Sub  ";
                StrSql = StrSql + " Where M_index = " + txtKey.Text.Trim();

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);


                tran.Commit();
                Delete_Error_Check = 1;
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






        //string[] g_HeaderText = {"출고지",  "출고지명"  , "이동일자"   , "입고지"  , "입고지명"          
        //                        , "상품코드"  , "상품명"   , "이동수량"    , "이동자"   , "비고"       
        //                        , ""    , ""   , ""    , ""   , ""   
        private void dGridView_Base_Sub_DoubleClick(object sender, EventArgs e)
        {
            //string[] g_HeaderText = {"출고지",  "출고지명"  , "이동일자"   , "입고지"  , "입고지명"          
            //                    , "상품코드"  , "상품명"   , "요청수량"    , "요청자"   , "비고"       
            //                    , ""    , ""   , "확정일자"    , "확정자"   , "확정수량"   
            //                    , "확정비고"    , "기록일"   , "기록자"    , "D_Name"   , ""   
            //                    };

            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string D_ID = "";
               
              
                string M_index = ""; string MDate = ""; 
                string Out_C_Code = ""; string IN_C_Code = ""; string ItemCode = ""; int ItemCnt = 0;
                string Remarks1 = ""; string M_ID = ""; string Center_Name = ""; string Center_IN_Name = "";
                string U_Name = "";    
                
                int D_Cnt = 0 ;    string D_NAme ="";  string D_Date = "";

                M_index = (sender as DataGridView).CurrentRow.Cells[10].Value.ToString();                                
                
                Out_C_Code = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                Center_Name = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();

                MDate = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString().Replace  ("-","");


                IN_C_Code = (sender as DataGridView).CurrentRow.Cells[3].Value.ToString();
                Center_IN_Name = (sender as DataGridView).CurrentRow.Cells[4].Value.ToString();

                ItemCode = (sender as DataGridView).CurrentRow.Cells[5].Value.ToString();

                ItemCnt = int.Parse ((sender as DataGridView).CurrentRow.Cells[7].Value.ToString());
                M_ID = (sender as DataGridView).CurrentRow.Cells[8].Value.ToString();                              
                U_Name = (sender as DataGridView).CurrentRow.Cells[11].Value.ToString();


                D_Date = (sender as DataGridView).CurrentRow.Cells[12].Value.ToString().Replace  ("-","");
                D_ID = (sender as DataGridView).CurrentRow.Cells[13].Value.ToString();
                D_NAme = (sender as DataGridView).CurrentRow.Cells[18].Value.ToString();
                D_Cnt = int.Parse ((sender as DataGridView).CurrentRow.Cells[14].Value.ToString());
                Remarks1 = (sender as DataGridView).CurrentRow.Cells[15].Value.ToString();                                
                

                txtKey.Text = M_index;
                txtMDate.Text = MDate.Replace("-", "");
                
                txtCenter_Code2.Text = Out_C_Code;
                txtCenter2.Text = Center_Name;

                txtCenter_Code.Text = IN_C_Code;
                txtCenter.Text = Center_IN_Name;

                txt_M_Cnt.Text = ItemCnt.ToString();



                mtxtCDate.Text = D_Date ;
                txtR_Id_Code.Text = D_ID;
                txtR_Id.Text = D_NAme;
                txtRemark.Text = Remarks1;
                txt_D_Cnt.Text = D_Cnt.ToString();

                mtxtCDate.Focus();               

            }
        }




        private void radioB_S_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtMakDate1, mtxtMakDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }



        private void radioB_R_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtMDate2, mtxtMDate3, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }

        private void opt_confirm_1_Click(object sender, EventArgs e)
        {
            tableLayoutPanel38.Enabled = false;
            mtxtMakDate1.Text = "";
            mtxtMakDate2.Text = "";
        }

        private void opt_confirm_2_Click(object sender, EventArgs e)
        {
            tableLayoutPanel38.Enabled = true;
        }

        private void opt_confirm_3_Click(object sender, EventArgs e)
        {
            tableLayoutPanel38.Enabled = true;
        }
    }
}
