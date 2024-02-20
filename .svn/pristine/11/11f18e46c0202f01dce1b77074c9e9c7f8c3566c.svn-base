using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace MLM_Program
{
    public partial class frmMileage_Select : Form
    {
     

        
        cls_Grid_Base cg_Sub = new cls_Grid_Base();
        cls_Grid_Base cg_Sub2 = new cls_Grid_Base();

        private const string base_db_name = "tbl_Memberinfo";
        private int Data_Set_Form_TF;

        public frmMileage_Select()
        {
            InitializeComponent();

            DoubleBuffered = true;
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.SetProperty, null, dGridView_Base_Sub, new object[] { true });

        }


        private void frmBase_From_Load(object sender, EventArgs e)
        {
            Data_Set_Form_TF = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub.d_Grid_view_Header_Reset(1);

            dGridView_Base_Sub2_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub2.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            Data_Set_Form_TF = 1;            
            Data_Set_Form_TF = 0;

            combo_Se2.Items.Add("");
            combo_Se2.Items.Add(cm._chang_base_caption_search("발생자"));
            combo_Se2.Items.Add(cm._chang_base_caption_search("미발생자"));            
          
            mtxtSMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtSMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            txt_PP_1.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_PP_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_PP_3.BackColor = cls_app_static_var.txt_Enable_Color;

            txt_P_1.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_3.BackColor = cls_app_static_var.txt_Enable_Color;       
        }



        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (cls_User.uSearch_MemberNumber != "")
            {
                Data_Set_Form_TF = 1;
                //mtxtMbid.Text = cls_User.uSearch_MemberNumber;
                mtxtSMbid.Text = cls_User.uSearch_MemberNumber;
                cls_User.uSearch_MemberNumber = "";

                EventArgs ee1 = null; Base_Button_Click(butt_Select, ee1);  //butt_Search
                //EventArgs ee1 = null; Select_Button_Click(butt_Select, ee1);

                //Set_Form_Date(mtxtMbid.Text);
                Data_Set_Form_TF = 0;
            }


        }

        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Select.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_Excel.Left = butt_Select.Left + butt_Select.Width + 2;
            butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Select);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);

            //cfm.button_flat_change(butt_Check_01);
            //cfm.button_flat_change(butt_Check_02);
            //cfm.button_flat_change(butt_Save);

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
                T_bt = butt_Select;     //조회  F1
            if (e.KeyValue == 115)
                T_bt = butt_Delete;   // 삭제  F4
            if (e.KeyValue == 119)
                T_bt = butt_Excel;    //엑셀  F8    
            if (e.KeyValue == 112)
                T_bt = butt_Clear;    //엑셀  F5    

            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123 || e.KeyValue == 113 || e.KeyValue == 119 || e.KeyValue == 112)
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

                if (tb.Name == "txt_Price_3")
                {
                    if (tb.Text != "")
                        tb.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(tb.Text.Replace(",", "")));
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

            //else if (tb.Tag.ToString() == "name")  //회원 정보 관련해서 이름 검색을 필요로 하는 텍스트 박스이다.
            //{
            //    //쿼리문 오류관련 입력만 아니면 가능하다.
            //    if (T_R.Text_KeyChar_Check(tb, e) == false)
            //    {
            //        e.Handled = true;
            //        return;
            //    } // end if
            //}

        }

        //void T_R_Key_Enter_13_Name(string txt_tag, TextBox tb)
        //{
        //    if (txt_tag != "")
        //    {
        //        int reCnt = 0;
        //        cls_Search_DB cds = new cls_Search_DB();
        //        string Search_Mbid = "";
        //        reCnt = cds.Member_Name_Search(ref Search_Mbid, txt_tag);

        //        if (reCnt == 1)
        //        {
        //            if (tb.Name == "txtName")
        //            {
        //                mtxtMbid.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
        //                if (Input_Error_Check(mtxtMbid, 0) == true)
        //                    Set_Form_Date(mtxtMbid.Text);

        //                //SendKeys.Send("{TAB}");
        //            }


        //        }
        //        else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
        //        {

        //            frmBase_Member_Search e_f = new frmBase_Member_Search();
        //            if (tb.Name == "txtName")
        //            {
        //                e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
        //                e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);
        //            }

        //            e_f.ShowDialog();

        //            SendKeys.Send("{TAB}");
        //        }


        //    }
        //    else
        //        SendKeys.Send("{TAB}");

        //}

        //void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        //{
        //    searchMbid = ""; searchMbid2 = 0;
        //    seachName = txtName.Text.Trim();
        //}
       



        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
           //int Sw_Tab = 0;

            if ((sender is TextBox) == false) return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
               // Sw_Tab = 1;
            }

            //if (tb.Name == "txtCenter")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtCenter_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtCenter_Code);
            //}

            //if (tb.Name == "txtCenter2")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtCenter_Code2.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtCenter_Code2);
            //}

            ////if (tb.Name == "txtR_Id")
            ////{
            ////    if (tb.Text.Trim() == "")
            ////        txtR_Id_Code.Text = "";
            ////    else if (Sw_Tab == 1)
            ////        Ncod_Text_Set_Data(tb, txtR_Id_Code);
            ////}

            //if (tb.Name == "txtR_Id2")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtR_Id_Code2.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code2);
            //}

            //if (tb.Name == "txt_ItemName2")
            //{
            //    if (tb.Text.Trim() == "")
            //        txt_ItemName_Code2.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txt_ItemName_Code2);
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
            //        Db_Grid_Popup(tb, txtCenter_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtCenter_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            //if (tb.Name == "txtCenter2")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtCenter_Code2, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtCenter_Code2);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            ////if (tb.Name == "txtR_Id")
            ////{
            ////    Data_Set_Form_TF = 1;
            ////    if (tb.Text.ToString() == "")
            ////        Db_Grid_Popup(tb, txtR_Id_Code, "");
            ////    else
            ////        Ncod_Text_Set_Data(tb, txtR_Id_Code);

            ////    SendKeys.Send("{TAB}");
            ////    Data_Set_Form_TF = 0;
            ////}

            //if (tb.Name == "txtR_Id2")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtR_Id_Code2, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code2);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            //if (tb.Name == "txt_ItemName2")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txt_ItemName_Code2, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txt_ItemName_Code2);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}
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
                if (tb.Name == "txtCenter" || tb.Name == "txtCenter2")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txt_ItemName2")
                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", strSql);
            }
            else
            {
                if (tb.Name == "txtCenter" || tb.Name == "txtCenter2")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
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

            if (tb.Name == "txtCenter" || tb.Name == "txtCenter2")
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
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub.d_Grid_view_Header_Reset(1);

            dGridView_Base_Sub2_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub2.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            combo_Se2.SelectedIndex = -1; 

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtSMbid);
        }






        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {
                Form_Clear_();    
            }

            //else if (bt.Name == "butt_Save")
            //{
            //    int Save_Error_Check = 0;
            //    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //    Save_Base_Data(ref Save_Error_Check);

            //    if (Save_Error_Check > 0)
            //    {
            //        Form_Clear_();
            //    }
            //    this.Cursor = System.Windows.Forms.Cursors.Default;
            //}
            
            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }

            //else if (bt.Name == "butt_Delete")
            //{
            //    int Delete_Error_Check = 0;
            //    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //    Delete_Base_Data(ref Delete_Error_Check);

            //    if (Delete_Error_Check > 0)
            //        Form_Clear_();

            //    this.Cursor = System.Windows.Forms.Cursors.Default;
            //}                
            else if (bt.Name == "butt_Select")
            {
                dGridView_Base_Sub2_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cg_Sub2.d_Grid_view_Header_Reset(1);

                txt_P_1.Text = ""; txt_P_2.Text = ""; txt_P_3.Text = "";
                txt_PP_1.Text = ""; txt_PP_2.Text = ""; txt_PP_3.Text = "";


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
            Excel_Export_File_Name = this.Text; // "Mileage_IN_OUT_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base_Sub;
        }




















        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);            
        }





        private Boolean Search_Check_TextBox_Error()
        {

            cls_Check_Input_Error c_er = new cls_Check_Input_Error();

            if (mtxtSMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                int Ret = 0;
                Ret = c_er._Member_Nmumber_Split(mtxtSMbid);

                if (Ret == -1)
                {
                    mtxtSMbid.Focus(); return false;
                }
            }


            if (mtxtSMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                int Ret = 0;
                Ret = c_er._Member_Nmumber_Split(mtxtSMbid2);

                if (Ret == -1)
                {
                    mtxtSMbid2.Focus(); return false;
                }
            }


            //if (txtInDate2.Text.Trim() != "")
            //{
            //    int Ret = 0;
            //    Ret = c_er.Input_Date_Err_Check(txtInDate2);

            //    if (Ret == -1)
            //    {
            //        txtInDate2.Focus(); return false ;
            //    }
            //}
            //if (txtInDate3.Text.Trim() != "")
            //{
            //    int Ret = 0;
            //    Ret = c_er.Input_Date_Err_Check(txtInDate3);

            //    if (Ret == -1)
            //    {
            //        txtInDate3.Focus(); return false;
            //    }
            //}



            return true;
        }



        private void Base_Sub_Grid_Set()
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            if (Search_Check_TextBox_Error() == false) return;           
         

            string Tsql = "";

            //string[] g_HeaderText = {"입고번호"  , "입고일자"   , "상품코드"  , "상품명"   , "입고지"        
            //                        , "입고수량"   , "입고자"    , "비고"   , ""    , ""                                
            //                            };

            Tsql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ufm.mbid + '-' + Convert(Varchar,ufm.mbid2) ";
            else
                Tsql = Tsql + " ufm.mbid2 ";

            Tsql = Tsql + " ,ufm.M_Name ";
            Tsql = Tsql + " ,PlusValue ";

            Tsql = Tsql + " , MinusValue ";
            Tsql = Tsql + " , TotalValue ";
           
            Tsql = Tsql + " , '' ";
            Tsql = Tsql + " , '' ";
            Tsql = Tsql + " ,'' ,'',''";
            Tsql = Tsql + " From ufn_Mem_Mileage_Search () ufm ";
            Tsql = Tsql + " LEFT Join tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid  = ufm.Mbid  And tbl_Memberinfo.Mbid2  = ufm.Mbid2 ";
            
            //Tsql = Tsql + " LEFT Join tbl_User  (nolock) ON tbl_User.User_id = tbl_Member_Mileage.In_Name  ";


            string strSql = " Where   ufm.Mbid2 > 0  ";
            
            string Mbid = ""; int Mbid2 = 0;
            //회원번호1로 검색
            if (
                (mtxtSMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
                &&
                (mtxtSMbid2.Text.Replace("-", "").Replace("_", "").Trim() == "")
                )
            {
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(mtxtSMbid.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And ufm.Mbid ='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And ufm.Mbid2 = " + Mbid2;
                }


            }

            //회원번호2로 검색
            if (
                (mtxtSMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
                &&
                (mtxtSMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
                )
            {
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(mtxtSMbid.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And ufm.Mbid >='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And ufm.Mbid2 >= " + Mbid2;
                }

                if (csb.Member_Nmumber_Split(mtxtSMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And ufm.Mbid <='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And ufm.Mbid2 <= " + Mbid2;
                }
            }

            //회원명으로 검색
            if (txtName2.Text.Trim() != "")
                strSql = strSql + " And ufm.M_Name Like '%" + txtName2.Text.Trim() + "%'";

            if (combo_Se2.SelectedIndex  == 1 )
                strSql = strSql + " And PlusValue + MinusValue > 0 ";

            if (combo_Se2.SelectedIndex == 2)
                strSql = strSql + " And PlusValue + MinusValue = 0 ";


            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

            Tsql = Tsql + strSql;
            Tsql = Tsql + " Order by ufm.Mbid,ufm.Mbid2  ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            double Sum_01 = 0; double Sum_02 = 0; double Sum_03 = 0;
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_Sub_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                Sum_01 = Sum_01 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][2].ToString());
                Sum_02 = Sum_02 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][3].ToString());
                Sum_03 = Sum_03 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][4].ToString());                
            }

            if (gr_dic_text.Count > 0)
            {
                txt_P_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_01);
                txt_P_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_02);
                txt_P_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_03);
               
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
            cg_Sub.grid_col_Count = 10;
            cg_Sub.basegrid = dGridView_Base_Sub;
            cg_Sub.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_Sub.grid_Frozen_End_Count = 2;
            cg_Sub.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"회원번호",  "성명"  , "총적립"   , "총사용"  , "총잔여"          
                                , ""  , ""   , ""    , ""   , ""                                       
                                };
            cg_Sub.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 90,  110, 90, 80, 110                             
                             , 0 ,0 , 0 ,  0 , 0                               
                            };
            cg_Sub.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                                                             
                                   };
            cg_Sub.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft //10                                   
                          
                              };
            cg_Sub.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cg_Sub.grid_cell_format = gr_dic_cell_format;           
        }













        private void Item_Grid_Set(string Mbid, int Mbid2)
        {



            string Tsql = "";

            //string[] g_HeaderText = {"기록일",  "적립"  , "사용"   , "구분"  , "적립_주문번호"          
            //                    , "사용_주문번호"  , "기록자"   , "T_index"    , ""   , ""                                       
            //                    };


            Tsql = "Select tbl_Member_Mileage.T_Time ";
            Tsql = Tsql + " ,tbl_Member_Mileage.PlusValue";
            Tsql = Tsql + " ,tbl_Member_Mileage.MinusValue ";
            Tsql = Tsql + " ,Case  When PlusValue  > 0 then C1.T_Name When MinusValue  >0  then C2.T_Name End ";
            Tsql = Tsql + " ,tbl_Member_Mileage.Plus_OrderNumber ";            
            Tsql = Tsql + " ,tbl_Member_Mileage.Minus_OrderNumber ";
            Tsql = Tsql + " ,tbl_Member_Mileage.User_id";

            Tsql = Tsql + " ,tbl_Member_Mileage.ETC1 ";
            Tsql = Tsql + " ,tbl_Member_Mileage.T_index ";
            Tsql = Tsql + " ,'',''   ";

            Tsql = Tsql + " From tbl_Member_Mileage  (nolock) ";
            Tsql = Tsql + " LEFT Join tbl_Member_Mileage_Code C1 (nolock) ON tbl_Member_Mileage.PlusKind = C1.T_Code ";
            Tsql = Tsql + " LEFT Join tbl_Member_Mileage_Code C2 (nolock) ON tbl_Member_Mileage.MinusKind = C2.T_Code  ";
            Tsql = Tsql + " Where T_Time <> '' "; 

            if (Mbid != "")
                Tsql = Tsql + " And tbl_Member_Mileage.Mbid ='" + Mbid + "'";

            if (Mbid2 >= 0)
                Tsql = Tsql + " And tbl_Member_Mileage.Mbid2 = " + Mbid2;

            Tsql = Tsql + " Order by tbl_Member_Mileage.T_Time DESC";
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            double Sum_11 = 0; //double Sum_07 = 0; double Sum_08 = 0;
            double Sum_09 = 0; double Sum_10 = 0; //double Sum_15 = 0;
            //double Sum_16 = 0;
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_Item(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.


                Sum_09 = Sum_09 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][1].ToString());
                Sum_10 = Sum_10 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][2].ToString());
                //Sum_11 = Sum_11 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][10].ToString());
            }


            if (gr_dic_text.Count > 0)
            {
                txt_PP_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_09);
                txt_PP_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_10);
                txt_PP_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_09 - Sum_10);
            }


            cg_Sub2.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cg_Sub2.db_grid_Obj_Data_Put();
        }


        private void Set_gr_Item(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;

            object[] row0 = new object[cg_Sub2.grid_col_Count];

            while (Col_Cnt < cg_Sub2.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }


            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Base_Sub2_Header_Reset()
        {
            cg_Sub2.grid_col_Count = 10;
            cg_Sub2.basegrid = dGridView_Base_Sub2;
            cg_Sub2.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_Sub2.grid_Frozen_End_Count = 2;
            cg_Sub2.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"기록일",  "적립"  , "사용"   , "구분"  , "적립_주문번호"          
                                , "사용_주문번호"  , "기록자"   , "비고"    , "T_index"   , ""                                       
                                };
            cg_Sub2.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 90,  110, 90, 80, 110                             
                             ,100 ,100 , 100 ,  0 , 0                               
                            };
            cg_Sub2.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                                                             
                                   };
            cg_Sub2.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft //10                                   
                          
                              };
            cg_Sub2.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[2 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;            
            cg_Sub2.grid_cell_format = gr_dic_cell_format;
        }









        //private bool numericCheck(string ss)
        //{
        //     cls_Check_Text T_R = new cls_Check_Text();

        //    //쿼리문 오류관련 입력만 아니면 가능하다.
        //    if (T_R.Text_KeyChar_Check(e, 1) == false)
        //    {
        //        e.Handled = true;
        //        return;
        //    } // end if   

        //    //try
        //    //{
        //    //    int ll = Convert.ToInt32(ss);
        //    //    return true;
        //    //}
        //    //catch
        //    //{
        //    //    return false;
        //    //}
        //}





     

        //string[] g_HeaderText = {"입고번호"  , "입고일자"   , "상품코드"  , "상품명"   , "입고지"        
        //                        , "입고수량"   , "입고자"    , "비고"   , ""    , ""                                
        //                            };
        private void dGridView_Base_Sub_DoubleClick(object sender, EventArgs e)
        {
            dGridView_Base_Sub2_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub2.d_Grid_view_Header_Reset();
            txt_PP_1.Text = ""; txt_PP_2.Text = ""; txt_PP_3.Text = "";            

            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string T_Mbid = "";
                T_Mbid = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();


                cls_Search_DB csb = new cls_Search_DB();
                string Mbid = ""; int Mbid2 = 0;
                if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == 1)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    Item_Grid_Set(Mbid, Mbid2);
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }               
            }
        }






        private void radioB_R_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(txtInDate2, txtInDate3, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }




        private void S_MtxtData_KeyPress(object sender, KeyPressEventArgs e)
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
                SendKeys.Send("{TAB}");
            }
        }

        private void S_MtxtMbid_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            MaskedTextBox tb = (MaskedTextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
            }
        }



























    }
}
