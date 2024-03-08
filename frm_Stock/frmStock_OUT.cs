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
    public partial class frmStock_OUT : Form
    {
       
        
        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cg_Sub = new cls_Grid_Base();

        private const string base_db_name = "tbl_StockOutput";
        private int Data_Set_Form_TF;

        public frmStock_OUT()
        {
            InitializeComponent();
            

        }


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;

            Base_Grid_Set();
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
            
            Data_Set_Form_TF = 1;
            mtxtInDate.Text = DateTime.Now.ToString("yyyyMMdd");
            Data_Set_Form_TF = 0;  
        
            combo_Se.Items.Add ("");            
            combo_Se.Items.Add(cm._chang_base_caption_search("회원"));
            combo_Se.Items.Add(cm._chang_base_caption_search("직원"));
            combo_Se.Items.Add(cm._chang_base_caption_search("센타"));
            combo_Se.Items.Add(cm._chang_base_caption_search("기타"));

            //combo_Se.Items.Add(cm._chang_base_caption_search("본사"));
            //combo_Se.Items.Add(cm._chang_base_caption_search("대전"));
            //combo_Se.Items.Add(cm._chang_base_caption_search("지점"));
            //combo_Se.Items.Add(cm._chang_base_caption_search("업체"));
            //combo_Se.Items.Add(cm._chang_base_caption_search("기타"));


            combo_Se2.Items.Add("");            
            combo_Se2.Items.Add(cm._chang_base_caption_search("회원"));
            combo_Se2.Items.Add(cm._chang_base_caption_search("직원"));
            combo_Se2.Items.Add(cm._chang_base_caption_search("센타"));
            combo_Se2.Items.Add(cm._chang_base_caption_search("기타"));

            //combo_Se2.Items.Add(cm._chang_base_caption_search("본사"));
            //combo_Se2.Items.Add(cm._chang_base_caption_search("대전"));
            //combo_Se2.Items.Add(cm._chang_base_caption_search("지점"));
            //combo_Se2.Items.Add(cm._chang_base_caption_search("업체"));

            table_center.Visible = false;
            table_mem.Visible = false;
            table_staff.Visible = false;

            table_mem.Top = table_center.Top;
            table_staff.Top = table_center.Top;

            mtxtInDate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtInDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtInDate3.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat; 
        }



        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();
        }

        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Save.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_Delete.Left = butt_Save.Left + butt_Save.Width + 2;
            //butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Save);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);

            cfm.button_flat_change(butt_Search);
            cfm.button_flat_change(butt_Excel);
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

                            //cls_form_Meth cfm = new cls_form_Meth();
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
            T_R.Key_Enter_13_Name += new Key_13_Name_Event_Handler(T_R_Key_Enter_13_Name);

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

            if (tb.Name == "txtR_Id3")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtR_Id_Code3.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_ItemName2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txt_ItemName_Code2.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_BaseOut")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txt_BaseOut_Code.Text = "";
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

            if (tb.Name == "txtR_Id3")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtR_Id_Code3);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtR_Id_Code3, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtR_Id_Code3);

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

            if (tb.Name == "txt_BaseOut")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txt_BaseOut_Code);
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

            if (tb.Name == "txtR_Id2")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txtR_Id2")
                cgb_Pop.Next_Focus_Control = butt_Save;

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

            if (tb.Name == "txt_BaseOut")
                cgb_Pop.Next_Focus_Control = combo_Se;

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
                if (tb.Name == "txtCenter" || tb.Name == "txtCenter2" || tb.Name == "txtCenter3")
                {
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);
                    if (tb.Name == "txtCenter")
                        cgb_Pop.Next_Focus_Control = txtR_Id;
                }

                if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2")
                {
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);
                    if (tb.Name == "txtR_Id")
                        cgb_Pop.Next_Focus_Control = txtRemark ;
                }

                if (tb.Name == "txtR_Id3" )
                    cgb_Pop.db_grid_Popup_Base(2, "직원번호", "성명", "User_Ncode", "U_Name", strSql);

                if (tb.Name == "txt_ItemName2")
                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", strSql);
            }
            else
            {
                if (tb.Name == "txtCenter" || tb.Name == "txtCenter2" || tb.Name == "txtCenter3")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " And  ShowOrderCenter = 'Y' ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                    if (tb.Name == "txtCenter")
                        cgb_Pop.Next_Focus_Control = txtR_Id;
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

                if (tb.Name == "txtR_Id3")
                {
                    string Tsql;
                    Tsql = "Select User_Ncode ,U_Name   ";
                    Tsql = Tsql + " From tbl_User (nolock) ";
                    Tsql = Tsql + " Order by user_id ";

                    cgb_Pop.db_grid_Popup_Base(2, "직원번호", "성명", "User_Ncode", "U_Name", Tsql);
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

            if (tb.Name == "txtR_Id3")
            {
                Tsql = "Select User_Ncode ,U_Name   ";
                Tsql = Tsql + " From tbl_User (nolock) ";
                Tsql = Tsql + " Where U_Name like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    User_Ncode like '%" + tb.Text.Trim() + "%'";
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
            
            Base_Grid_Set();   //상품 정보를 불러온다.
            

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub.d_Grid_view_Header_Reset();            
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtInDate);

            combo_Se.SelectedIndex = -1;
            table_center.Visible = false;
            table_mem.Visible = false;
            table_staff.Visible = false;

            
            mtxtInDate.Text = DateTime.Now.ToString("yyyyMMdd");
            Data_Set_Form_TF = 0;
            mtxtInDate.Focus();
            
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
            Excel_Export_File_Name = this.Text; // "OUT_Select";
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


            if (mtxtInDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtInDate2.Text, mtxtInDate2, "Date") == false)
                {
                    mtxtInDate2.Focus();
                    return ;
                }
            }

            if (mtxtInDate3.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtInDate3.Text, mtxtInDate3, "Date") == false)
                {
                    mtxtInDate3.Focus();
                    return;
                }
            }


            
            int SG_TF = 0;
            cls_form_Meth cm = new cls_form_Meth();

            if (combo_Se2.Text == cm._chang_base_caption_search("회원"))
            {
                SG_TF = 1;                
            }

            if (combo_Se2.Text == cm._chang_base_caption_search("직원"))
            {
                SG_TF = 2;                
            }

            if (combo_Se2.Text == cm._chang_base_caption_search("센타"))
            {
                SG_TF = 3;            
            }


            //if (combo_Se2.Text == cm._chang_base_caption_search("본사"))
            //{
            //    SG_TF = 4;
            //}

            //if (combo_Se2.Text == cm._chang_base_caption_search("대전"))
            //{
            //    SG_TF = 5;
            //}

            //if (combo_Se2.Text == cm._chang_base_caption_search("지점"))
            //{
            //    SG_TF = 6;
            //}

            //if (combo_Se2.Text == cm._chang_base_caption_search("업체"))
            //{
            //    SG_TF = 7;
            //}

            if (combo_Se2.Text == cm._chang_base_caption_search("기타"))
            {
                SG_TF = 4;
            }

            string Tsql = "";
            //string[] g_HeaderText = {"",  "출고번호"  , "출고일자"   , "상품코드"  , "상품명"          
            //                    , "출고지"  , "출고수량"   , "출고자"    , "비고"   , "-출고지코드"       
            //                    , "-Out_FL"    , "-출고자명"   , "출고_대상"    , "대상_회원번호"   , "대상_회원명"   
            //                    , "대상_센타"    , "-대상센타코드"   , "대상_직원"    , "-대상_직원번호"   , ""   
            //                    };

            Tsql = "Select '소진출고'";
            Tsql = Tsql + " ,Out_Index ";
            Tsql = Tsql + " ,LEFT(Out_Date,4) +'-' + LEFT(RIGHT(Out_Date,4),2) + '-' + RIGHT(Out_Date,2) ";
            Tsql = Tsql + " ,ItemCode ";
            Tsql = Tsql + " ,Isnull(tbl_Goods.name,'') ";
            Tsql = Tsql + " ,Isnull(tbl_Business.name,'') ";            
            Tsql = Tsql + " ,ItemCount ";
            Tsql = Tsql + " ,Out_Name " ;
            Tsql = Tsql + " ,Remarks1 ";

            Tsql = Tsql + " ,Out_C_Code ";
            Tsql = Tsql + " ,Out_FL";
            Tsql = Tsql + " ,Isnull(tbl_User.U_Name ,'' )  ";

            //if (combo_Se.Text == cm._chang_base_caption_search("본사"))
            //{
            //    SG_TF = 5;
            //}

            //if (combo_Se.Text == cm._chang_base_caption_search("대전"))
            //{
            //    SG_TF = 6;
            //}

            //if (combo_Se.Text == cm._chang_base_caption_search("지점"))
            //{
            //    SG_TF = 7;
            //}

            //if (combo_Se.Text == cm._chang_base_caption_search("업체"))
            //{
            //    SG_TF = 8;
            //}

            //if (combo_Se.Text == cm._chang_base_caption_search("기타"))
            //{
            //    SG_TF = 9;
            //}


            Tsql = Tsql + " ,Case ";
            Tsql = Tsql + " When SG_TF = 1 Then '" + cm._chang_base_caption_search("회원") + "'";
            Tsql = Tsql + " When SG_TF = 2 Then '" + cm._chang_base_caption_search("직원") + "'";
            Tsql = Tsql + " When SG_TF = 3 Then '" + cm._chang_base_caption_search("센타") + "'";
            Tsql = Tsql + " When SG_TF = 4 Then '" + cm._chang_base_caption_search("기타") + "'";            
            //Tsql = Tsql + " When SG_TF = 4 Then '" + cm._chang_base_caption_search("본사") + "'";
            //Tsql = Tsql + " When SG_TF = 5 Then '" + cm._chang_base_caption_search("대전") + "'";
            //Tsql = Tsql + " When SG_TF = 6 Then '" + cm._chang_base_caption_search("지점") + "'";
            //Tsql = Tsql + " When SG_TF = 7 Then '" + cm._chang_base_caption_search("업체") + "'";
            Tsql = Tsql + " END  ";
            Tsql = Tsql + ", SG_TF ";
            
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", SG_Mbid + '-' + Convert(Varchar,SG_Mbid2) ";
            else
                Tsql = Tsql + ", SG_Mbid ";

            Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

            Tsql = Tsql + " ,Isnull(Sug_B.Name ,'' ) , SG_BusCode  ";
            Tsql = Tsql + " ,Isnull(SUg_U.U_Name ,'' ) ,  SG_Staff_Code";

            Tsql = Tsql + " , tbl_StockOutput.OrderNumber ";

            Tsql = Tsql + " , tbl_StockOutput.Out_FL_Code_2 ";
            Tsql = Tsql + " , Isnull(tbl_Base_Out_Code.T_Name,'')  ";

            Tsql = Tsql + " From tbl_StockOutput (nolock) " ;
            Tsql = Tsql + " LEFT Join tbl_Goods (nolock) ON ItemCode = tbl_Goods.Ncode ";
            Tsql = Tsql + " LEFT Join tbl_Business (nolock) ON tbl_Business.Ncode = tbl_StockOutput.Out_C_Code  ";
            Tsql = Tsql + " LEFT Join tbl_User (nolock) ON tbl_User.User_id = tbl_StockOutput.Out_Name  ";
            Tsql = Tsql + " LEFT Join tbl_User SUg_U (nolock) ON SUg_U.User_Ncode = tbl_StockOutput.SG_Staff_Code  ";
            Tsql = Tsql + " LEFT Join tbl_Business Sug_B  (nolock) ON Sug_B.Ncode = tbl_StockOutput.SG_BusCode  ";

            Tsql = Tsql + " LEFT Join tbl_Base_Out_Code (nolock) ON tbl_Base_Out_Code.Ncode = tbl_StockOutput.Out_FL_Code_2  ";
            

            Tsql = Tsql + " LEFT Join tbl_Memberinfo   (nolock) ON tbl_Memberinfo.Mbid = tbl_StockOutput.SG_Mbid And tbl_Memberinfo.Mbid2 = tbl_StockOutput.SG_Mbid2  ";
            
            Tsql = Tsql + " Where (Out_FL = '002' )";

            if (mtxtInDate2.Text.Replace("-", "").Trim() != "" && mtxtInDate3.Text.Replace("-", "").Trim() == "")
                Tsql = Tsql + " And Out_Date = '" + mtxtInDate2.Text.Replace("-", "") + "'";

            if (mtxtInDate2.Text.Replace("-", "").Trim() != "" && mtxtInDate3.Text.Replace("-", "").Trim() != "")
            {
                Tsql = Tsql + " And Out_Date >= '" + mtxtInDate2.Text.Replace("-", "") + "'";
                Tsql = Tsql + " And Out_Date <= '" + mtxtInDate3.Text.Replace("-", "") + "'";
            }

            if (SG_TF > 0)
            {
                Tsql = Tsql + " And SG_TF = " + SG_TF ;
            }

            if (txtCenter_Code2.Text != "" )
                Tsql = Tsql + " And Out_C_Code = '" + txtCenter_Code2.Text + "'";

            if (txt_ItemName_Code2.Text != "")
                Tsql = Tsql + " And ItemCode = '" + txt_ItemName_Code2.Text + "'";

            if (txtR_Id_Code2.Text != "")
                Tsql = Tsql + " And Out_Name = '" + txtR_Id_Code2.Text + "'";

            Tsql = Tsql + " And tbl_StockOutput.Out_C_Code in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";


            Tsql = Tsql + " Order by Out_Index ";

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
            cg_Sub.grid_col_Count = 23;
            cg_Sub.basegrid = dGridView_Base_Sub;
            cg_Sub.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_Sub.grid_Frozen_End_Count = 3;
            cg_Sub.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"",  "출고번호"  , "출고일자"   , "상품코드"  , "상품명"          
                                , "출고지"  , "출고수량"   , "출고자"    , "비고"   , "_출고지코드"       
                                , "_Out_FL"    , "_출고자명"   , "출고_대상"  , "_출고대상"  , "대상_회원번호"   
                                , "대상_회원명"   , "대상_센타"    , "_대상센타코드"   , "대상_직원"    , "_대상_직원번호"   
                                ,"_OrderNumber" , "소진코드", "소진사유"
                                };
            cg_Sub.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 0,  110, 90, 80, 110                             
                             , 110 ,70 , 100 ,  200 , 0  
                             ,0 , 0 ,  100 , 0 ,  100   
                             ,100  ,100 , 0 ,  100 , 0 
                             , 0 ,10,10
                            };
            cg_Sub.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                         
                                    ,true , true,  true,  true ,true    
                                    ,true , true,  true,  true ,true  
                                     ,true ,true ,true
                                   };
            cg_Sub.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleCenter //10    
                               
                                ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //15  

                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft //20 
 
                               ,DataGridViewContentAlignment.MiddleLeft //20  
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                              };
            cg_Sub.grid_col_alignment = g_Alignment;

            //Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            //gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //cg_Sub.grid_cell_format = gr_dic_cell_format;           
        }














        private void Base_Grid_Set()
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            ((DataGridViewTextBoxColumn)dGridView_Base.Columns[0]).MaxInputLength = 6;
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            string Tsql = "";

            Tsql = "Select 0, Name , NCode ,price2 ,''    ";
            Tsql = Tsql + " , '', '' ,'' ,'' ,'' ";
            if (mtxtInDate.Text.Replace("-", "").Length == 8)
                //Tsql = Tsql + " From ufn_Good_Search_01 ('" + mtxtInDate.Text.Replace("-", "").Trim() + "') ";
                Tsql = Tsql + " From ufn_Good_Search_02 ('" + mtxtInDate.Text.Replace("-", "").Trim() + "', '" + cls_User.gid_CountryCode + "') ";
            else
                //Tsql = Tsql + " From ufn_Good_Search_01 ('" + cls_User.gid_date_time + "') ";
                Tsql = Tsql + " From ufn_Good_Search_02 ('" + cls_User.gid_date_time + "', '" + cls_User.gid_CountryCode + "') ";
            Tsql = Tsql + " Order by Ncode ";

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
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;
            object[] row0 = new object[cgb.grid_col_Count];

            while (Col_Cnt < cgb.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;
        }



        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 10;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.CellSelect;
            cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"출고수량"  , "상품명"   , "상품코드"  , "소비자가"   , ""        
                                , ""   , ""    , ""   , ""    , ""                                
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 60, 130, 100, 70, 0                             
                             ,0 , 0 ,  0 , 0 ,  0                             
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { false , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                         
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //10                                                           
                              };
            cgb.grid_col_alignment = g_Alignment;


            DataGridViewColumnSortMode[] g_SortM =
                              {DataGridViewColumnSortMode.NotSortable  
                               ,DataGridViewColumnSortMode.Automatic  
                               ,DataGridViewColumnSortMode.Automatic  
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic  //5
                               
                               ,DataGridViewColumnSortMode.Automatic                              
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic 
                               ,DataGridViewColumnSortMode.Automatic //10                                                           
                              };
            cgb.grid_col_SortMode = g_SortM;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb.grid_cell_format = gr_dic_cell_format;

            cgb.basegrid.RowHeadersVisible = false;
        }


        


        private bool  Check_TextBox_Error_Date()
        {

            if (mtxtInDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtInDate.Text, mtxtInDate, "Date") == false)
                {
                    mtxtInDate.Focus();
                    return false;
                }
            }
            return true;
        }


        
        private Boolean Check_TextBox_Error()
        {
            cls_Check_Text T_R = new cls_Check_Text();
            string me = "";


            me = T_R.Text_Null_Check(mtxtInDate, "Msg_Sort_Stock_Out_Date"); //입고일자를
            if (me != "")
            {
                MessageBox.Show(me);
                return false;
            }

            me = T_R.Text_Null_Check(txtCenter_Code, "Msg_Sort_Stock_Out_Center"); //입고지를
            if (me != "")
            {
                MessageBox.Show(me);
                return false;
            }



            if (txt_BaseOut_Code.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Select_Not_OUT_Sort_2") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_BaseOut.Focus(); return false;                
            }


            if (combo_Se.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Select_Not_OUT_Sort") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                combo_Se.Focus(); return false;                
            }



            cls_form_Meth cm = new cls_form_Meth();            
            string T_Mbid = mtxtMbid.Text.Trim();

            if (combo_Se.Text == cm._chang_base_caption_search("회원"))
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
            }

            if (combo_Se.Text == cm._chang_base_caption_search("직원"))
            {
                if (txtR_Id_Code3.Text.Trim() == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Staff")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtR_Id3.Focus(); return false;
                }
            }

            if (combo_Se.Text == cm._chang_base_caption_search("센타"))
            {              
                if (txtCenter_Code3.Text.Trim() == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Center")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtCenter3.Focus(); return false;
                }                                
            }


            int chk_cnt = 0;

            for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "")
                    dGridView_Base.Rows[i].Cells[0].Value = "0";
                
                //0보다 큰 내역이 있는지를 체크한다. 없으면 저장할 내역이 없다는 걸 알리기 위함.
                if (int.Parse( dGridView_Base.Rows[i].Cells[0].Value.ToString()) > 0 )
                    chk_cnt++;                  
            }

            if (chk_cnt == 0) //저장할 내역이 없을을 알린다.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridView_Base.Focus(); return false;
            }


            //수정인데 선택된 내역이 없거나 2건이상이 선택이 되었다.
            if (txtKey.Text.Trim() != "")
            {
                if (chk_cnt >= 2)
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Edit_Two_Not") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    dGridView_Base.Focus(); return false;
                }
            }


            //날짜 관련 텍스트 파일들에 대해서 날짜 오류를 체크한다
            if (Check_TextBox_Error_Date() == false) return false;


            cls_Search_DB csd = new cls_Search_DB();
            if (csd.Check_Stock_Close(txtCenter_Code.Text, mtxtInDate.Text.Replace("-", "").Trim()) == false)
            {
                txtCenter.Focus();
                return false;
            }
            
            return true;
        }



        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
            string str_Q = ""; int SG_TF = 0; string SG_Staff_Code = ""; string SG_BusCode = "";
            string SG_Mbid = ""; int SG_Mbid2 = 0;

            if (txtKey.Text == "")
                str_Q = "Msg_Base_Save_Q";
            else
                str_Q = "Msg_Base_Edit_Q";

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString(str_Q), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            
            if (Check_TextBox_Error() == false) return;

            string Out_FL = "002";   //'''---소진출고는 002입  

             cls_form_Meth cm = new cls_form_Meth ();
             cls_Search_DB csd = new cls_Search_DB();
             string T_Mbid = mtxtMbid.Text.Trim();                        

             if (combo_Se.Text == cm._chang_base_caption_search("회원"))
             {
                 SG_TF = 1;
                 csd.Member_Nmumber_Split(T_Mbid, ref SG_Mbid, ref SG_Mbid2);
             }

             if (combo_Se.Text == cm._chang_base_caption_search("직원"))
             {
                 SG_TF = 2;
                 SG_Staff_Code = txtR_Id_Code3.Text.Trim();
             }

             if (combo_Se.Text == cm._chang_base_caption_search("센타"))
             {
                 SG_TF = 3;
                 SG_BusCode = txtCenter_Code3.Text.Trim();
             }


             //if (combo_Se.Text == cm._chang_base_caption_search("본사"))
             //{
             //    SG_TF = 4;                
             //}

             //if (combo_Se.Text == cm._chang_base_caption_search("대전"))
             //{
             //    SG_TF = 5;              
             //}

             //if (combo_Se.Text == cm._chang_base_caption_search("지점"))
             //{
             //    SG_TF = 6;               
             //}

             //if (combo_Se.Text == cm._chang_base_caption_search("업체"))
             //{
             //    SG_TF = 7;             
             //}

             if (combo_Se.Text == cm._chang_base_caption_search("기타"))
             {
                 SG_TF = 4;               
             }


               
            if (txtKey.Text != "") //수정일 경우에는 수정 프로시져로 가고 이 프로시져를 빠져나가라
            {
                Save_Base_Data_UpDate(ref Save_Error_Check, SG_TF, SG_Mbid, SG_Mbid2, SG_Staff_Code, SG_BusCode);
                if (Save_Error_Check > 0)
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
                return;
            }
                  
            string Tsql = "";
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();
            
           
            try
            {               

                string StrSql = ""; string T_Or = ""; string Out_Index = "";
                int ItemCnt = 0; string ItemCode = ""; int Out_Price = 0;
                for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                {
                    if (int.Parse (dGridView_Base.Rows[i].Cells[0].Value.ToString()) > 0)
                    {                       
                        ItemCnt = int.Parse(dGridView_Base.Rows[i].Cells[0].Value.ToString());
                        ItemCode = dGridView_Base.Rows[i].Cells[2].Value.ToString();
                        Out_Price = int.Parse( dGridView_Base.Rows[i].Cells[3].Value.ToString()) ;

                        T_Or = cls_User.gid + ' ' + DateTime.UtcNow.ToString();

                        StrSql = "INSERT INTO tbl_Sales_PassNumber ";
                        StrSql = StrSql + " (Pass_Number2,OrderNumber,SalesItemIndex,User_TF,T_Date) " ;
                        StrSql = StrSql + " Select " ;
                        StrSql = StrSql + "'" + mtxtInDate.Text.Replace("-", "").Substring(2, 6);
                        StrSql = StrSql + "'+ Right('00000' + convert(varchar(8),convert(float,Right(Isnull(Max(Pass_Number2),0),5)) + 1),5)  ";
        
                        StrSql = StrSql + ",'" + T_Or + "',0,1,Convert(Varchar(25),GetDate(),21)" ;
                        StrSql = StrSql + " From tbl_Sales_PassNumber (nolock) " ;
                        StrSql = StrSql + " Where LEFT(Pass_Number2,6) = '" + mtxtInDate.Text.Replace("-", "").Substring(2, 6) + "'";
        
                        Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);



                        Tsql = "Select Top 1  Pass_Number2   ";
                        Tsql = Tsql + " From tbl_Sales_PassNumber (nolock) ";
                        Tsql = Tsql + " Where  OrderNumber ='"+ T_Or + "'" ;
                        Tsql = Tsql + " Order by Pass_Number2 DESC ";
                        
                        DataSet ds = new DataSet();
                        //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                        if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;                                                
                        Out_Index = ds.Tables["t_P_table"].Rows[0][0].ToString();


                        StrSql = "Insert into tbl_StockOutput (";
                        StrSql = StrSql + " Out_Index,Out_FL,Out_Date  ";
                        StrSql = StrSql + " , ItemCode ";
                        StrSql = StrSql + " ,ItemCount" ;
                        StrSql = StrSql + " ,Out_Price,Out_PV1, Out_SumPrice,Out_SumPV1 ";
                        StrSql = StrSql + " , Out_Name ";
                        StrSql = StrSql + " , Remarks1, Remarks2 ";
                        StrSql = StrSql + " ,C_Code_FL , Out_C_Code ";

                        StrSql = StrSql + " ,SG_TF ";
                        StrSql = StrSql + " ,SG_Staff_Code , SG_BusCode ";
                        StrSql = StrSql + " ,SG_Mbid , SG_Mbid2 ";
                        StrSql = StrSql + " ,Out_FL_Code_2 ";

                        StrSql = StrSql + " ,RecordId, RecordTime ";
                        StrSql = StrSql + " )";
                        StrSql = StrSql + " Values " ;
                        StrSql = StrSql + " (";
                        StrSql = StrSql + "'" + Out_Index + "'" ;   //입고번호
                        StrSql = StrSql + ",'" + Out_FL + "'";   //기타입고 코드 번호
                        StrSql = StrSql + ",'" + mtxtInDate.Text.Replace("-", "").Trim() + "'";   //입고일자                                                
                        StrSql = StrSql + ",'" + ItemCode  + "'";       //상품코드
                        StrSql = StrSql + "," + ItemCnt ;      //입고수량
                        StrSql = StrSql + "," + Out_Price   ;       //단위소매가
                        StrSql = StrSql + ", 0 "        ;  //단위PV
        
        
                        StrSql = StrSql + "," + Out_Price *  ItemCnt  ;      //총입고금액
                        StrSql = StrSql + ", 0 "  ;        //총입고PV

                        StrSql = StrSql + ",'" + txtR_Id_Code.Text.Trim ()  + "'";      //입고자
                        StrSql = StrSql + ",'" + txtRemark.Text.Trim()  + "'";       //비고1
                        StrSql = StrSql + ",''"   ;        //비고2
        
                        StrSql = StrSql + ",'C'" ;   //센타/창고 구분자 c:센타  w:창고
                        StrSql = StrSql + ",'" + txtCenter_Code.Text.Trim() + "'";  //센타/창고 코드 번호

                        StrSql = StrSql + "," + SG_TF;      //출고대상 구분자 1=회원   2=직원    3=센타
                        StrSql = StrSql + ",'" + SG_Staff_Code.Trim() + "'";  //직원번호
                        StrSql = StrSql + ",'" + SG_BusCode.Trim() + "'";  //센타코드
                        StrSql = StrSql + ",'" + SG_Mbid + "'";  //회원번호1
                        StrSql = StrSql + "," + SG_Mbid2 ;  //회원번호2


                        StrSql = StrSql + ",'" + txt_BaseOut_Code.Text.Trim() + "'";
                        StrSql = StrSql + ",'" + cls_User.gid  + "'";
                        StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                
                        StrSql = StrSql + ")"  ;

                        Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);
                    }
                }

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




        private void Save_Base_Data_UpDate(ref int Save_Error_Check , int SG_TF, string SG_Mbid, int SG_Mbid2, string  SG_Staff_Code , string  SG_BusCode)
        {   
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            string Out_FL = "002";
            try
            {               

            string StrSql = ""; 
            int ItemCnt = 0; string ItemCode = ""; int Out_Price = 0;
            for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            {
                if (int.Parse(dGridView_Base.Rows[i].Cells[0].Value.ToString()) > 0)
                {
                    ItemCnt = int.Parse(dGridView_Base.Rows[i].Cells[0].Value.ToString());
                    ItemCode = dGridView_Base.Rows[i].Cells[2].Value.ToString();
                    Out_Price = int.Parse(dGridView_Base.Rows[i].Cells[3].Value.ToString());

                    StrSql = "INSERT INTO tbl_StockOutput_DelBackup ";                    
                    StrSql = StrSql + " Select  * ";                    
                    StrSql = StrSql + ",'" + cls_User.gid  + "'" ;
                    StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " From tbl_StockOutput ";
                    StrSql = StrSql + " Where Out_Index = '" + txtKey.Text.Trim() + "'";

                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                    //StrSql = "Delete From   tbl_StockOutput ";
                    //StrSql = StrSql + " Where Out_Index = '" + txtKey.Text.Trim()  + "'";

                    //Temp_Connect.Delete_Data (StrSql, base_db_name, Conn, tran);


                    StrSql = "UpDate  tbl_StockOutput Set ";
                    StrSql = StrSql + "  Out_FL = '" + Out_FL + "'";
                    StrSql = StrSql + ", Out_Date = '" + mtxtInDate.Text.Replace("-", "").Trim () + "'";
                    StrSql = StrSql + ", Out_C_Code  = '" + txtCenter_Code.Text.Trim() + "'";
                    StrSql = StrSql + ", ItemCode = '" + ItemCode + "'";
                    StrSql = StrSql + ", ItemCount = " + ItemCnt;
                    StrSql = StrSql + ", Out_Price = " + Out_Price ;
                    StrSql = StrSql + ", Out_SumPrice = " + Out_Price * ItemCnt;

                    StrSql = StrSql + ", SG_TF = " + SG_TF;
                    StrSql = StrSql + ", SG_Staff_Code = '" + SG_Staff_Code + "'";
                    StrSql = StrSql + ", SG_BusCode = '" + SG_BusCode + "'";
                    StrSql = StrSql + ", SG_Mbid = '" + SG_Mbid + "'";
                    StrSql = StrSql + ", SG_Mbid2 = " + SG_Mbid2;

                    StrSql = StrSql + ", Out_FL_Code_2 = '" + txt_BaseOut_Code.Text.Trim() + "'";
                                        
                    StrSql = StrSql + ", Remarks1 = '" + txtRemark.Text.Trim() + "'";                    
                    StrSql = StrSql + ", Out_Name  = '" + txtR_Id_Code.Text.Trim() + "'";

                    StrSql = StrSql + " Where Out_Index ='" + txtKey.Text  +"'";


                    Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);
                }
            }

            tran.Commit();

            Save_Error_Check = 1;
            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));


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

                StrSql = "Insert into  tbl_StockOutput_DelBackup ";
                StrSql = StrSql + " Select *  ";
                StrSql = StrSql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_StockOutput ";
                StrSql = StrSql + " Where Out_Index = " + txtKey.Text.Trim();
                
                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                StrSql = "Delete From tbl_StockOutput  ";
                StrSql = StrSql + " Where Out_Index = " + txtKey.Text.Trim();

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




        private void dGridView_Base_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dGridView_Base.EditingControl.KeyPress += new KeyPressEventHandler(textBoxPart_TextChanged);
        }

        private void textBoxPart_TextChanged(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }




        ////string[] g_HeaderText = {"",  "출고번호"  , "출고일자"   , "상품코드"  , "상품명"          
        ////                        , "출고지"  , "출고수량"   , "출고자"    , "비고"   , "-출고지코드"       
        ////                        , "-Out_FL"    , "-출고자명"   , "출고_대상"  , "-출고대상"  , "대상_회원번호"   
        ////                        , "대상_회원명"   , "대상_센타"    , "-대상센타코드"   , "대상_직원"    , "-대상_직원번호"   
        ////                        };
        private void dGridView_Base_Sub_DoubleClick(object sender, EventArgs e)
        {
            //StrSql = StrSql + "  Out_FL = '" + Out_FL + "'";
            //StrSql = StrSql + ", Out_Date = '" + txtInDate.Text + "'";
            //StrSql = StrSql + ", Out_C_Code  = '" + txtCenter_Code.Text.Trim() + "'";
            //StrSql = StrSql + ", ItemCode = '" + ItemCode + "'";
            //StrSql = StrSql + ", ItemCnt = " + ItemCnt;
            //StrSql = StrSql + ", Out_Price = " + Out_Price;
            //StrSql = StrSql + ", Out_SumPrice = " + Out_Price * ItemCnt;

            //StrSql = StrSql + ", Remarks1 = '" + txtRemark.Text.Trim() + "'";
            

            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string Out_Index = ""; string Out_Date = ""; string Out_FL = "";
                string Out_C_Code = ""; string ItemCode = ""; int ItemCnt = 0;
                string Remarks1 = "";  string Out_Name ="" ; string Center_Name = "";
                string U_Name = ""; int SG_TF = 0;
                string SG_Staff_Code = ""; string SG_Staff_Code_Name = "";
                string SG_BusCode = ""; string SG_BusCode_Name = "";
                //string SG_Mbid = ""; int SG_Mbid2 = 0; 
                string SG_Name = "";


                string OrderNumber = (sender as DataGridView).CurrentRow.Cells[20].Value.ToString();

                if (OrderNumber != "")
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Selective exhaustion release details are due to sales. It cannot be modified/deleted on the current screen.");
                    }
                    else
                    {
                        MessageBox.Show("선택 소진출고 내역은 매출로 인한 내역입니다. 현화면상에서 수정/삭제 할 수 없습니다.");
                    }
                    return;
                }
                Out_Index = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
                Out_Date = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString();
                ItemCode = (sender as DataGridView).CurrentRow.Cells[3].Value.ToString();
                Center_Name = (sender as DataGridView).CurrentRow.Cells[5].Value.ToString();
                Out_Name= (sender as DataGridView).CurrentRow.Cells[7].Value.ToString();

                ItemCnt = int.Parse ((sender as DataGridView).CurrentRow.Cells[6].Value.ToString());
                Remarks1 = (sender as DataGridView).CurrentRow.Cells[8].Value.ToString();

                Out_C_Code = (sender as DataGridView).CurrentRow.Cells[9].Value.ToString();
                Out_FL= (sender as DataGridView).CurrentRow.Cells[10].Value.ToString();
                U_Name = (sender as DataGridView).CurrentRow.Cells[11].Value.ToString();

                SG_TF = int.Parse ((sender as DataGridView).CurrentRow.Cells[13].Value.ToString()) ;
                SG_Staff_Code_Name = (sender as DataGridView).CurrentRow.Cells[18].Value.ToString();
                SG_Staff_Code = (sender as DataGridView).CurrentRow.Cells[19].Value.ToString();

                SG_BusCode_Name = (sender as DataGridView).CurrentRow.Cells[16].Value.ToString();
                SG_BusCode = (sender as DataGridView).CurrentRow.Cells[17].Value.ToString();

                



                combo_Se.SelectedIndex = SG_TF; 

                if (SG_TF == 1)
                {
                    //cls_Search_DB csd = new cls_Search_DB();
                    string T_Mbid = (sender as DataGridView).CurrentRow.Cells[14].Value.ToString();
                    //csd.Member_Nmumber_Split(T_Mbid, ref SG_Mbid, ref SG_Mbid2);
                    
                    mtxtMbid.Text = T_Mbid;
                    SG_Name = (sender as DataGridView).CurrentRow.Cells[15].Value.ToString();
                    txtName.Text = SG_Name;
                }

                txtKey.Text = Out_Index;
                mtxtInDate.Text = Out_Date.Replace ("-","") ;
                txtCenter_Code.Text = Out_C_Code;
                txtCenter.Text = Center_Name;
                txtR_Id_Code.Text = Out_Name;
                txtR_Id.Text = U_Name;
                txtRemark.Text = Remarks1;

                txtR_Id_Code3.Text = SG_Staff_Code;
                txtR_Id3.Text = SG_Staff_Code_Name;

                txtCenter_Code3.Text = SG_BusCode;
                txtCenter3.Text = SG_BusCode_Name;
                               

                
                
                
                //ItemCnt    ItemCode
                for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
                {
                    //빈칸으로 들어간 내역을 0으로 바꾼다
                    if (dGridView_Base.Rows[i].Cells[2].Value.ToString() == ItemCode)
                        dGridView_Base.Rows[i].Cells[0].Value = ItemCnt;
                    else
                        dGridView_Base.Rows[i].Cells[0].Value = "0";
                }

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
                            if (Input_Error_Check(mtb, 0) == true)
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
            mtxtMbid.Text = Send_Number; txtName.Text = Send_Name;
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
                    ct.from_control_clear(table_mem, mtb);
                }                
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
            seachName = txtName.Text.Trim();
        }




        private Boolean Input_Error_Check(MaskedTextBox m_tb, int s_Kind)
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
            Tsql = "Select Mbid , Mbid2, M_Name ";
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


        private void Set_Form_Date(string T_Mbid)
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

                Tsql = Tsql + ", tbl_Memberinfo.Cpno ";

                Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";

                Tsql = Tsql + " , LEFT(tbl_Memberinfo.RegTime,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.RegTime,4),2) + '-' + RIGHT(tbl_Memberinfo.RegTime,2)  AS RegTime  ";

                Tsql = Tsql + "  , Add_TF ";
                Tsql = Tsql + " , tbl_Memberinfo.hptel ";
                Tsql = Tsql + " , tbl_Memberinfo.hometel ";
                Tsql = Tsql + " , tbl_Memberinfo.address1 ";
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
            }

            Data_Set_Form_TF = 0;
        }


        private void combo_Se_SelectedIndexChanged(object sender, EventArgs e)
        {
            cls_form_Meth cm = new cls_form_Meth () ;

            if (combo_Se.Text  == cm._chang_base_caption_search("회원"))
            {
                table_center.Visible = false;
                table_mem.Visible = true;
                table_staff.Visible = false;
                cm.from_control_clear(table_mem, mtxtMbid);
            }

            if (combo_Se.Text == cm._chang_base_caption_search("센타"))
            {
                table_center.Visible = true;
                table_mem.Visible = false;
                table_staff.Visible = false;
                cm.from_control_clear(table_center, txtCenter3);
            }

            if (combo_Se.Text == cm._chang_base_caption_search("직원"))
            {
                table_center.Visible = false;
                table_mem.Visible = false;
                table_staff.Visible = true;
                cm.from_control_clear(table_staff, txtR_Id3);
            }
        }


        private void radioB_R_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtInDate2, mtxtInDate3, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }













    }
}
