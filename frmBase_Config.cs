﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLM_Program
{
    public partial class frmBase_Config : clsForm_Extends
    {

        cls_Grid_Base cgb = new cls_Grid_Base();
        private const string base_db_name = "tbl_Base";

        public frmBase_Config()
        {
            InitializeComponent();
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


            else if ((tb.Tag != null) && (tb.Tag.ToString() == "-"))
            {
                //숫자와  - 만
                if (T_R.Text_KeyChar_Check(e, "1") == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "."))
            {
                //숫자와  - 만
                if (T_R.Text_KeyChar_Check(e, "1",1) == false)
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

        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {

            if (tb.Name == "txt_Search")
            {
                if (tb.Text.Trim() != "")
                {
                    Lan_Grid_Set(tb.Text);
                }
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


        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }



        private void butt_Save_SellCode_Click(object sender, EventArgs e)
        {
            if (txtCode.Text.Trim() == "" || txtName.Text.Trim() == "")
                return; 
            
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            
            string StrSql = "insert into tbl_SellType (SellCode, SellTypeName, recordid, recordtime) " ;
            StrSql = StrSql + " values('" + txtCode.Text.Trim() + "', '" + txtName.Text.Trim() + "', '" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21)  )";                           
            
            if (Temp_Connect.Insert_Data(StrSql, base_db_name, this.Name.ToString(), this.Text) == false) return;
            
            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));

            SellCode_Grid_Set(); //주문종류 그리드를 다시 셋팅한다..
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(tab_SellCode, txtCode);


            this.Cursor = System.Windows.Forms.Cursors.Default ;
        }


        private void butt_Save_Class_Click(object sender, EventArgs e)
        {
            if (txtCode_C.Text.Trim() == "" || txtName_C.Text.Trim() == "" || txtCode_C2.Text.Trim() == "")
                return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string StrSql = "insert into tbl_Class (Grade_Code, Grade_Name, Grade_Cnt) " ;
            StrSql = StrSql + " values('" + txtCode_C.Text.Trim() + "', '" + txtName_C.Text.Trim() + "', " + txtCode_C2.Text + ")"; 

            if (Temp_Connect.Insert_Data(StrSql, base_db_name, this.Name.ToString(), this.Text) == false) return;


            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));

            Class_Grid_Set(); //직급 그리드를 다시 셋팅한다..
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(tab_Class, txtCode_C);

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        private void butt_Save_Class_Close(object sender, EventArgs e)
        {
            if (txtCode_P.Text.Trim() == "" || txtName_P.Text.Trim() == "" )
                return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string StrSql = "insert into tbl_SellType_Close (CloseCode, CloseTypeName ) ";
            StrSql = StrSql + " values('" + txtCode_P.Text.Trim() + "', '" + txtName_P.Text.Trim() + "')";                           
            

            if (Temp_Connect.Insert_Data(StrSql, base_db_name, this.Name.ToString(), this.Text) == false) return;


            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));

            Close_Grid_Set(); //직급 그리드를 다시 셋팅한다..
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(tab_Close, txtCode_P);

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void frmBase_Config_Load(object sender, EventArgs e)
        {
           
            tab_Base.SelectedIndex = 0;
            SellCode_Grid_Set();
            txtCode.Focus();

            string[] data_M = { "0","1", "2", "3", "4", "5" 
                               , "6", "7", "8", "9" , "10"                              
                              };

         

            // 각 콤보박스에 데이타를 초기화
            comboBox_Num_1.Items.AddRange(data_M);
            comboBox_Num_2.Items.AddRange(data_M);
            comboBox_Down_Cnt.Items.AddRange(data_M);
            comboBox_Center_Cnt.Items.AddRange(data_M);
            comboBox_GoodCode.Items.AddRange(data_M);
            comboBox_Sub_1.Items.AddRange(data_M);
            comboBox_Sub_2.Items.AddRange(data_M);
            comboBox_Sub_3.Items.AddRange(data_M);

            comboBox_Num_1.SelectedIndex = 0;
            comboBox_Num_2.SelectedIndex = 0;
            comboBox_Down_Cnt.SelectedIndex = 0;
            comboBox_Center_Cnt.SelectedIndex = 0;

            comboBox_GoodCode.SelectedIndex = 0;
            comboBox_Sub_1.SelectedIndex = 0;
            comboBox_Sub_2.SelectedIndex = 0;
            comboBox_Sub_3.SelectedIndex = 0;

        }



        private void SellCode_Grid_Set()
        {
            dGridView_Base_Header_Reset(dGridView_Base); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            string StrSql = "Select SellCode, SellTypeName , ''  " ;
            StrSql = StrSql + " From tbl_SellType ";
            StrSql = StrSql + " Order by SellCode";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
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

        


        private void Class_Grid_Set()
        {
            dGridView_Base_Header_Reset(dGridView_Class); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            string StrSql = "Select Grade_Code, Grade_Cnt,  Grade_Name  ";
            StrSql = StrSql + " From tbl_Class ";
            StrSql = StrSql + " Order by Grade_Code";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
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

        private void Close_Grid_Set()
        {
            dGridView_Base_Header_Reset(dGridView_Close); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            string StrSql = "Select CloseCode, CloseTypeName , ''  ";
            StrSql = StrSql + " From tbl_SellType_Close ";
            StrSql = StrSql + " Order by CloseCode";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
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


        private void Lan_Grid_Set(string Ncode = "")
        {
            dGridView_Base_Header_Reset(dGridView_Lan); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            string StrSql = "Select Base_L, Kor_L,  Eng_L, Jap_L  ";
            StrSql = StrSql + " From tbl_Base_Label (nolock) ";

            if (Ncode != "")            
                StrSql = StrSql + " Where Base_L Like '%" + Ncode.Trim() + "%'";                            

            StrSql = StrSql + " Order by Base_L";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_Lan_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }





        private void dGridView_Base_Header_Reset(DataGridView  Dgv)
        {
            cgb.grid_col_Count = 10;
            cgb.basegrid = Dgv;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;            
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            if (Dgv.Name == "dGridView_Base" || Dgv.Name ==  "dGridView_Close")
            {
                string[] g_HeaderText = {"코드" ,"명칭"  , ""   , ""  , ""    
                                    , ""     , ""   , ""    , ""   , ""                                  
                                        };
                cgb.grid_col_header_text = g_HeaderText;

                int[] g_Width = { 55 , 85, 0, 0, 0
                                 , 0 ,0, 0, 0, 0                            
                                };
                cgb.grid_col_w = g_Width;
            }

            if (Dgv.Name == "dGridView_Class")
            {
                string[] g_HeaderText = {"직급코드" ,"직급구분자"  , "직급명칭"   , ""  , ""    
                                    , ""     , ""   , ""    , ""   , ""                                  
                                        };
                cgb.grid_col_header_text = g_HeaderText;

                int[] g_Width = { 55 , 85, 100, 0, 0
                                 , 0 ,0, 0, 0, 0                            
                                };
                cgb.grid_col_w = g_Width;
            }

            if (Dgv.Name == "dGridView_Lan")
            {
                string[] g_HeaderText = {"기준글자" ,"한글"  , "영어"   , "일본어"  , ""    
                                    , ""     , ""   , ""    , ""   , ""                                  
                                        };
                cgb.grid_col_header_text = g_HeaderText;

                int[] g_Width = { 55 , 85, 100, 100, 0
                                 , 0 ,0, 0, 0, 0                            
                                };
                cgb.grid_col_w = g_Width;
            }


            Boolean[] g_ReadOnly = {true , true , true,  true,  true                                    
                                    ,true , true,  true,  true ,true                                                                         
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {
                               DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter //5

                               ,DataGridViewContentAlignment.MiddleCenter                                 
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10
                              };
            cgb.grid_col_alignment = g_Alignment;
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,"" //ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,"" //ds.Tables[base_db_name].Rows[fi_cnt][4] 
                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }

        private void Set_Lan_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,"" //ds.Tables[base_db_name].Rows[fi_cnt][4] 
                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void tab_Base_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab_Base.SelectedTab.Name == "tab_SellCode")  //
            {               
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(tab_SellCode, txtCode);

                SellCode_Grid_Set();
                txtCode.Focus();
                this.Cursor = System.Windows.Forms.Cursors.Default;               
            }

            if (tab_Base.SelectedTab.Name == "tab_Class")  //
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(tab_Class, txtCode_C);

                Class_Grid_Set();
                txtCode_C.Focus();
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            if (tab_Base.SelectedTab.Name == "tab_Close")  //
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(tab_Close, txtCode_P);

                Close_Grid_Set();
                txtCode_P.Focus();
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            if (tab_Base.SelectedTab.Name == "tab_Config")  //
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(tab_Config);

                Program_Base_Setting();                
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            if (tab_Base.SelectedTab.Name == "tab_Lan")  //
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(tab_Lan ,txt_Base);

                Lan_Grid_Set();
                txt_Base.Focus();
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            

        }




        private void frmBase_Config_KeyDown(object sender, KeyEventArgs e)
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
                        }
                    }
                }// end if
            }
        }

        private void butt_Save_Config_Click(object sender, EventArgs e)
        {
            int Member_Code1 = 0, Member_Code2 = 0, buss_Code_Cnt = 0;
            int Down_Reg_Cnt = 0, ItemCodeSort = 0, goods_Code_Cnt = 0;
            int ItemCodeCnt1 = 0, ItemCodeCnt2 = 0, ItemCodeCnt3 = 0;
            int Resident_Number_Check = 0, Resident_Number_Check2 = 0, Many_Account_Check = 0;
            int CenterProgram = 0, Using_GoodsSet_TF = 0, Using_Mileage_TF = 0;
            int Using_ReturnCost_TF = 0 ;
            string Mem_Number_Auto_Flag = "", Sell_Union_Flag = "", Base_Mbid_Char = "";
            int save_uging_Pr_Flag = 0, nom_uging_Pr_Flag = 0, LineChangeCheck = 0;
            string SMS_ID = "";
            int Sell_Address_Multi_TF = 0, Order_OutPut_Num_TF = 0;
            

            
            


            Member_Code1 = int.Parse(comboBox_Num_1.Text.ToString());  //회원번호 앞자리
            Member_Code2 = int.Parse(comboBox_Num_2.Text.ToString());  //회원번호 뒷자리
            buss_Code_Cnt = int.Parse(comboBox_Center_Cnt.Text.ToString());  //센타 코드 자리수

             

            if (Member_Code1 == 0)
                Base_Mbid_Char = "";
            else
                Base_Mbid_Char = txt_BaseChar.Text.Trim();

            if (Member_Code1 != txt_BaseChar.Text.Length)
            {
                MessageBox.Show("회원번호 앞자리수와 기준 회원번호 글자 자리수가 상입합니다. 확인후 다시 시도해 주십시요.");
                return;
            }


            Down_Reg_Cnt = int.Parse(comboBox_Down_Cnt.Text.ToString()) ; //다운레그수
            if (check_Down.Checked == true)
                Down_Reg_Cnt = 999;  //다운 레그 숫

            if (int.Parse(comboBox_Sub_1.Text.ToString()) > 0)
            {
                ItemCodeSort = 1;  //대중소 분류 사용함.
                goods_Code_Cnt = 0;
                ItemCodeCnt1 = int.Parse(comboBox_Sub_1.Text.ToString());  //대분류 자리수
                ItemCodeCnt2 = int.Parse(comboBox_Sub_2.Text.ToString());  //중분류 자리수
                ItemCodeCnt3 = int.Parse(comboBox_Sub_3.Text.ToString());
            }
            else
            {
                ItemCodeSort = 0;  //대중소분류 사용여부
                goods_Code_Cnt = int.Parse(comboBox_GoodCode.Text.ToString());  //상품코드 자리수
            }


            
            if (check_Cpno_Err.Checked == true)  //주민번호 오류체크
                Resident_Number_Check = 1;

            if (check_Cpno.Checked == true)   //주민번호 필수 입력
                Resident_Number_Check2 = 1;


            if (check_Cpno_Multi.Checked == true)  //다구좌 여부
                Many_Account_Check = 1;

            if (check_Center.Checked == true)   //센타 프로그램 사용 여부
                CenterProgram = 1;

            if (check_Good_Set.Checked == true)   //셋트 상품 사용 여부
                Using_GoodsSet_TF = 1;

            if (check_Point.Checked == true)    //마일리지 관련 사용 여부
                Using_Mileage_TF = 1;

            if (check_Return.Checked == true)  //교환 사용여부
                Using_ReturnCost_TF = 1;


            if (radio_Auto_Num.Checked == true)  //회원번호 자동 증가
                Mem_Number_Auto_Flag = "A";

            if (radio_Hand_Num.Checked == true)  //회원번호 수당 입력
                Mem_Number_Auto_Flag = "H";

            if (radio_Rand_Num.Checked == true)  //회원번호 랜덤 생성
                Mem_Number_Auto_Flag = "R";


            if (radio_Sell_Etc.Checked == true)  //직판도 특판도 아닌 일반 사용
                Sell_Union_Flag = "";

            if (radio_Sell_U.Checked == true)  //특판용 프로그램
                Sell_Union_Flag = "U";

            if (radio_Sell_D.Checked == true)  //직판용 프로그램
                Sell_Union_Flag = "D";


                        
            if (check_Save.Checked == true)  //후원인 사용 여부
                save_uging_Pr_Flag = 10;

            if (check_Nom.Checked == true)  //후원인 사용 여부
                nom_uging_Pr_Flag = 10;

            if (check_Line.Checked == true)  //후원인 사용 여부
                LineChangeCheck = 1;

            if (check_Sell_Address.Checked == true)  //매출 등록시 주소 관련 한줄로 한건지
                Sell_Address_Multi_TF = 1;

            if (check_order_OutPut.Checked == true)  //출고시 주문번호당 출고번호 하나로 할건지.
                Order_OutPut_Num_TF = 1;


            string Union_Com_Code = "", Com_Name = "", Com_Number = "", Com_Address = "", Com_Bos_Name = "", Com_P_Number = "", Com_Ac_IP = "";
            string Com_Ac_Port = "0", Com_Cancel_Port = "0";

            if (txt_Com_APP.Text.Trim() == "") txt_Com_APP.Text = "0";
            if (txt_Com_CanP.Text.Trim() == "") txt_Com_CanP.Text = "0";

            Union_Com_Code = txt_ComCode.Text.Trim ();   //회사 특판이나 직판시 회사 코드
            Com_Name = txt_ComName.Text.Trim();   //회사명
            Com_Number = txt_ComNumber.Text.Trim();   //회사 사업자 번호
            Com_Address = txt_ComAdd.Text.Trim();   //거래처 회사 주소
            Com_Bos_Name = txt_ComBos.Text.Trim();   //거래처 회사 대표자명
            Com_P_Number = txt_ComPNumber.Text.Trim();   //거래처 회사 전화번호
            Com_Ac_IP = txt_ComIP.Text.Trim();   //직판 신고시 직판 데몬 떠있는 아이피
            Com_Ac_Port = txt_Com_APP.Text.Trim();   //직판 신고시 승인 신고하는 포트 
            Com_Cancel_Port = txt_Com_CanP.Text.Trim();   //직판 신고시 취소 신고하는 포트

            SMS_ID = txtSMS_ID.Text.Trim() ;   //SMS 아이디를 입력 받는다.

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();

            string Tsql = "Select *  From tbl_Config  (nolock)  ";

            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Config", ds) == false) return;

            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt <= 0)
            {
                string StrSql = "insert into tbl_Config ( ";
                StrSql = StrSql + "Member_Code1, Member_Code2 ,buss_Code_Cnt ";
                StrSql = StrSql + ",Down_Reg_Cnt, ItemCodeSort ,goods_Code_Cnt ";
                StrSql = StrSql + ",ItemCodeCnt1, ItemCodeCnt2 ,ItemCodeCnt3 ";

                StrSql = StrSql + ",Resident_Number_Check, Resident_Number_Check2 ,Many_Account_Check ";
                StrSql = StrSql + ",CenterProgram, Using_GoodsSet_TF ,Using_Mileage_TF ";
                StrSql = StrSql + ",Using_ReturnCost_TF, Mem_Number_Auto_Flag ,Sell_Union_Flag ";
                StrSql = StrSql + ",save_uging_Pr_Flag, nom_uging_Pr_Flag ,LineChangeCheck ";
                StrSql = StrSql + ",Base_Mbid_Char ";
                StrSql = StrSql + ",Union_Com_Code, Com_Name ,Com_Number ";
                StrSql = StrSql + ",Com_Address, Com_Bos_Name ,Com_P_Number ";
                StrSql = StrSql + ",Com_Ac_IP, Com_Ac_Port ,Com_Cancel_Port , SMS_ID  ";
                StrSql = StrSql + ",Sell_Address_Multi_TF, Order_OutPut_Num_TF "; 

                StrSql = StrSql + " ) ";
                StrSql = StrSql + " values ( ";
                StrSql = StrSql + Member_Code1 + "," + Member_Code2 + "," + buss_Code_Cnt + ",";
                StrSql = StrSql + Down_Reg_Cnt + "," + ItemCodeSort + "," + goods_Code_Cnt + ",";
                StrSql = StrSql + ItemCodeCnt1 + "," + ItemCodeCnt2 + "," + ItemCodeCnt3 + ",";

                StrSql = StrSql + Resident_Number_Check + "," + Resident_Number_Check2 + "," + Many_Account_Check + ",";
                StrSql = StrSql + CenterProgram + "," + Using_GoodsSet_TF + "," + Using_Mileage_TF + ",";
                StrSql = StrSql + Using_ReturnCost_TF + ",'" + Mem_Number_Auto_Flag + "','" + Sell_Union_Flag + "',";
                StrSql = StrSql + save_uging_Pr_Flag + "," + nom_uging_Pr_Flag + "," + LineChangeCheck + "";
                StrSql = StrSql + ",'" + Base_Mbid_Char + "'";

                StrSql = StrSql + ",'" + Union_Com_Code + "','" + Com_Name + "','" + Com_Number + "'";
                StrSql = StrSql + ",'" + Com_Address + "','" + Com_Bos_Name + "','" + Com_P_Number + "'";
                StrSql = StrSql + ",'" + Com_Ac_IP + "'," + Com_Ac_Port + "," + Com_Cancel_Port + ",'" + SMS_ID + "'";

                StrSql = StrSql + ", " + Sell_Address_Multi_TF + "," + Order_OutPut_Num_TF;

                StrSql = StrSql + " ) ";

                if (Temp_Connect.Insert_Data(StrSql, base_db_name, this.Name.ToString(), this.Text) == false) return;
            }
            else
            {
                string StrSql = "UpDate  tbl_Config Set ";
                StrSql = StrSql + "  Member_Code1 = " + Member_Code1 ;
                StrSql = StrSql + ", Member_Code2 = " + Member_Code2 ;
                StrSql = StrSql + ", buss_Code_Cnt = " + buss_Code_Cnt;
                StrSql = StrSql + ", Down_Reg_Cnt = " + Down_Reg_Cnt;
                StrSql = StrSql + ", ItemCodeSort = " + ItemCodeSort;

                StrSql = StrSql + ", goods_Code_Cnt = " + goods_Code_Cnt;
                StrSql = StrSql + ", ItemCodeCnt1 = " + ItemCodeCnt1;
                StrSql = StrSql + ", ItemCodeCnt2 = " + ItemCodeCnt2;
                StrSql = StrSql + ", ItemCodeCnt3 = " + ItemCodeCnt3;
                StrSql = StrSql + ", Resident_Number_Check = " + Resident_Number_Check;

                StrSql = StrSql + ", Resident_Number_Check2 = " + Resident_Number_Check2;
                StrSql = StrSql + ", Many_Account_Check = " + Many_Account_Check;
                StrSql = StrSql + ", CenterProgram = " + CenterProgram;
                StrSql = StrSql + ", Using_GoodsSet_TF = " + Using_GoodsSet_TF;
                StrSql = StrSql + ", Using_Mileage_TF = " + Using_Mileage_TF;

                StrSql = StrSql + ", Using_ReturnCost_TF = " + Using_ReturnCost_TF;
                StrSql = StrSql + ", Mem_Number_Auto_Flag = '" + Mem_Number_Auto_Flag + "'";
                StrSql = StrSql + ", Sell_Union_Flag = '" + Sell_Union_Flag + "'";
                StrSql = StrSql + ", Base_Mbid_Char = '" + Base_Mbid_Char + "'";
                StrSql = StrSql + ", save_uging_Pr_Flag = " + save_uging_Pr_Flag;
                StrSql = StrSql + ", nom_uging_Pr_Flag = " + nom_uging_Pr_Flag;
                StrSql = StrSql + ", LineChangeCheck = " + LineChangeCheck;

                StrSql = StrSql + ", Sell_Address_Multi_TF = " + Sell_Address_Multi_TF;
                StrSql = StrSql + ", Order_OutPut_Num_TF = " + Order_OutPut_Num_TF;

                StrSql = StrSql + ", Union_Com_Code = '" + Union_Com_Code + "'";
                StrSql = StrSql + ", Com_Name = '" + Com_Name + "'";
                StrSql = StrSql + ", Com_Number = '" + Com_Number + "'";
                StrSql = StrSql + ", Com_Address = '" + Com_Address + "'";
                StrSql = StrSql + ", Com_Bos_Name = '" + Com_Bos_Name + "'";
                StrSql = StrSql + ", Com_P_Number = '" + Com_P_Number + "'";
                StrSql = StrSql + ", Com_Ac_IP = '" + Com_Ac_IP + "'";
                StrSql = StrSql + ", SMS_ID = '" + SMS_ID + "'";

                

                StrSql = StrSql + ", Com_Ac_Port = " + Com_Ac_Port;
                StrSql = StrSql + ", Com_Cancel_Port = " + Com_Cancel_Port;
                
                Temp_Connect.Update_Data (StrSql);
            }

            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));

            Program_Base_Setting(); //기본설정을 다시 불러온다.
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        private void Program_Base_Setting()
        {
            comboBox_Num_1.SelectedIndex = 0;
            comboBox_Num_2.SelectedIndex = 0;
            comboBox_Down_Cnt.SelectedIndex = 0;
            comboBox_Center_Cnt.SelectedIndex = 0;

            comboBox_GoodCode.SelectedIndex = 0;
            comboBox_Sub_1.SelectedIndex = 0;
            comboBox_Sub_2.SelectedIndex = 0;
            comboBox_Sub_3.SelectedIndex = 0;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();

            string Tsql = "Select *  From tbl_Config  (nolock)  ";

            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Config", ds) == false) return;
            
            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt <= 0) return;

            int T_Check = 0;                        
          
            comboBox_Num_1.Text = ds.Tables["tbl_Config"].Rows[0]["Member_Code1"].ToString();  //회원번호 앞자리
            comboBox_Num_2.Text = ds.Tables["tbl_Config"].Rows[0]["Member_Code2"].ToString();  //회원번호 뒷자리             
            comboBox_Center_Cnt.Text = ds.Tables["tbl_Config"].Rows[0]["buss_Code_Cnt"].ToString();  //회원번호 뒷자리 

            txt_BaseChar.Text = ds.Tables["tbl_Config"].Rows[0]["Base_Mbid_Char"].ToString();   //회원번호 앞자리 기준 글자.

            txt_ComCode.Text = ds.Tables["tbl_Config"].Rows[0]["Union_Com_Code"].ToString();   //회사 특판이나 직판시 회사 코드
            txt_ComName.Text = ds.Tables["tbl_Config"].Rows[0]["Com_Name"].ToString();   //회사명
            txt_ComNumber.Text = ds.Tables["tbl_Config"].Rows[0]["Com_Number"].ToString();   //회사 사업자 번호
            txt_ComAdd.Text = ds.Tables["tbl_Config"].Rows[0]["Com_Address"].ToString();   //거래처 회사 주소
            txt_ComBos.Text = ds.Tables["tbl_Config"].Rows[0]["Com_Bos_Name"].ToString();   //거래처 회사 대표자명
            txt_ComPNumber.Text = ds.Tables["tbl_Config"].Rows[0]["Com_P_Number"].ToString();   //거래처 회사 전화번호
            txt_ComIP.Text = ds.Tables["tbl_Config"].Rows[0]["Com_Ac_IP"].ToString();   //직판 신고시 직판 데몬 떠있는 아이피
            txt_Com_APP.Text = ds.Tables["tbl_Config"].Rows[0]["Com_Ac_Port"].ToString();   //직판 신고시 승인 신고하는 포트 
            txt_Com_CanP.Text = ds.Tables["tbl_Config"].Rows[0]["Com_Cancel_Port"].ToString();   //직판 신고시 취소 신고하는 포트


            txtSMS_ID.Text = ds.Tables["tbl_Config"].Rows[0]["SMS_ID"].ToString();   //SMS 아이디를 입력 받는다.


            check_Down.Checked = false;
            if (int.Parse(ds.Tables["tbl_Config"].Rows[0]["Down_Reg_Cnt"].ToString()) > 10)
            {
                check_Down.Checked = true;
                comboBox_Down_Cnt.Text = "0";
            }
            else
            {
                comboBox_Down_Cnt.Text = ds.Tables["tbl_Config"].Rows[0]["Down_Reg_Cnt"].ToString();
            }


            if (int.Parse(ds.Tables["tbl_Config"].Rows[0]["ItemCodeSort"].ToString()) == 0)
            {
                comboBox_GoodCode.Text = ds.Tables["tbl_Config"].Rows[0]["goods_Code_Cnt"].ToString();
                comboBox_Sub_1.Text = "0";
                comboBox_Sub_2.Text = "0";
                comboBox_Sub_3.Text = "0";
            }
            else
            {
                comboBox_Sub_1.Text = ds.Tables["tbl_Config"].Rows[0]["ItemCodeCnt1"].ToString();
                comboBox_Sub_2.Text = ds.Tables["tbl_Config"].Rows[0]["ItemCodeCnt2"].ToString();
                comboBox_Sub_3.Text = ds.Tables["tbl_Config"].Rows[0]["ItemCodeCnt3"].ToString();
                comboBox_GoodCode.Text = "0";
            }


            T_Check = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Resident_Number_Check"].ToString()); //주민번호 오류 체크해라
            check_Cpno_Err.Checked = false;
            if (T_Check == 1)
                check_Cpno_Err.Checked = true;

            T_Check = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Resident_Number_Check2"].ToString());  //주민번호 필수 입력이다.1   0 필수입력 아니다.
            check_Cpno.Checked = false;
            if (T_Check == 1)
                check_Cpno.Checked = true;

            T_Check = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Many_Account_Check"].ToString());  //동일 주민번호로 해서 중복 가입이 안된다.  
             check_Cpno_Multi.Checked = false;
            if (T_Check == 1)
                check_Cpno_Multi.Checked = true;

            T_Check = int.Parse(ds.Tables["tbl_Config"].Rows[0]["CenterProgram"].ToString());  //센타 프로그램 우선은 사용함. 1 사용 0 안사용
             check_Center.Checked = false;
            if (T_Check == 1)
                check_Center.Checked = true;

            T_Check = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Using_GoodsSet_TF"].ToString()); //셋트 구성 메뉴를 사용할지 안할지   1사용   0 사용안함.
             check_Good_Set.Checked = false;
            if (T_Check == 1)
                check_Good_Set.Checked = true;

            T_Check = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Using_Mileage_TF"].ToString()); //마일리지관련 프로그램 사용할지 말지  0이면 사용하지 말고 1이면 열어줌
            check_Point.Checked = false;
            if (T_Check == 1)
                check_Point.Checked = true;


            T_Check = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Using_ReturnCost_TF"].ToString());  //교환 관련 메뉴를 열어줄지 여부  0이면 사용 안하고 1이면 열어줌
            check_Return.Checked = false;
            if (T_Check == 1)
                check_Return.Checked = true;


            T_Check = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Sell_Address_Multi_TF"].ToString());  //매출 등록시 주소 관련 한줄로 한건지 0이면 여러개 1이면 한개
            check_Sell_Address.Checked = false;
            if (T_Check == 1)
                check_Sell_Address.Checked = true;

            T_Check = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Order_OutPut_Num_TF"].ToString());  //출고시 주문번호당 출고번호 하나로 할건지. 0이면 사용 안함 1이면 상품 한개도 출고번호하나
            check_order_OutPut.Checked = false;
            if (T_Check == 1)
                check_order_OutPut.Checked = true;

            

            // A면 자동으로 증가    H면 손수 입력함.    R 이면 랜덤.
            if (ds.Tables["tbl_Config"].Rows[0]["Mem_Number_Auto_Flag"].ToString() == "A")
                radio_Auto_Num.Checked = true;

            if (ds.Tables["tbl_Config"].Rows[0]["Mem_Number_Auto_Flag"].ToString() == "H")
                radio_Hand_Num.Checked = true;

            if (ds.Tables["tbl_Config"].Rows[0]["Mem_Number_Auto_Flag"].ToString() == "R")
                radio_Rand_Num.Checked = true;


            //빈칸인 경우 특판도 직판도 아니고.... D 직판  U가 특판이다.
            if (ds.Tables["tbl_Config"].Rows[0]["Sell_Union_Flag"].ToString() == "")
                radio_Sell_Etc.Checked = true;

            if (ds.Tables["tbl_Config"].Rows[0]["Sell_Union_Flag"].ToString() == "U") //특판
                radio_Sell_U.Checked = true;

            if (ds.Tables["tbl_Config"].Rows[0]["Sell_Union_Flag"].ToString() == "D") //직판
                radio_Sell_D.Checked = true;


            T_Check = int.Parse(ds.Tables["tbl_Config"].Rows[0]["save_uging_Pr_Flag"].ToString());  //프로그램 상에서 후원인 관련 기능을 빼고 싶으면 0   후원인 기능을 넣고 싶으면 10 을 넣는다.
            check_Save.Checked = false;
            if (T_Check == 10)
                check_Save.Checked = true;

            T_Check = int.Parse(ds.Tables["tbl_Config"].Rows[0]["nom_uging_Pr_Flag"].ToString());  //프로그램 상에서 추천인 관련 기능을 빼고 싶으면 0   추천인 기능을 넣고 싶으면 10 을 넣는다.            
            check_Nom.Checked = false;
            if (T_Check == 10)
                check_Nom.Checked = true;


            T_Check = int.Parse(ds.Tables["tbl_Config"].Rows[0]["LineChangeCheck"].ToString());// 위치를 선택해라. 1이 선택하고    0은 자동임   회원등록시 위치 지정
            check_Line.Checked = false;
            if (T_Check == 1)
                check_Line.Checked = true;                        
   
        }


      


        private void txt_Search_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Name == "txt_Search")
            {
                if (tb.Text.Trim() == "")
                    Lan_Grid_Set();
            }
        }



        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            //cls_form_Meth ct = new cls_form_Meth();
            //ct.from_control_clear(tab_Lan, txt_Base);
            
            if ((sender as DataGridView).CurrentRow.Cells[0].Value != null)
            {                
                DataGridView T_Gd = (DataGridView)sender;
                txt_Base.Text   = T_Gd.CurrentRow.Cells[0].Value.ToString();
                txt_Kor.Text = T_Gd.CurrentRow.Cells[1].Value.ToString();
                txt_Eng.Text = T_Gd.CurrentRow.Cells[2].Value.ToString();
                txt_Jp.Text = T_Gd.CurrentRow.Cells[3].Value.ToString();                
            }
        }

        private void butt_Save_Lan_Click(object sender, EventArgs e)
        {
            if (txt_Base.Text.Trim() == "" )
                return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            
            DataSet ds = new DataSet();

            string Tsql = "Select *  From tbl_Base_Label  (nolock) Where  Base_L ='" + txt_Base.Text  + "'";

            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Config", ds) == false) return;

            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt <= 0)
            {
                string StrSql = "insert into tbl_Base_Label (Base_L, Kor_L, Eng_L, Jap_L) ";
                StrSql = StrSql + " values('" + txt_Base.Text.Trim() + "', '" + txt_Kor.Text.Trim() + "','" + txt_Eng.Text.Trim() + "','" + txt_Jp.Text.Trim() + "' )";

                if (Temp_Connect.Insert_Data(StrSql, base_db_name) == false) return;
            }
            else
            {
                string StrSql = "UpDate  tbl_Base_Label Set ";
                StrSql = StrSql + "  Kor_L = '" + txt_Kor.Text + "'";
                StrSql = StrSql + ", Eng_L = '" + txt_Eng.Text + "'";
                StrSql = StrSql + ", Jap_L = '" + txt_Jp.Text + "'";
                StrSql = StrSql + "  Where  Base_L ='" + txt_Base.Text  + "'";

                Temp_Connect.Update_Data(StrSql);
            }

            


            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));

            //Lan_Grid_Set();//언어그리드를 다시 리셋한다
            //cls_form_Meth ct = new cls_form_Meth();
            //ct.from_control_clear(tab_Lan, txt_Base);
            
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }



    }
}
