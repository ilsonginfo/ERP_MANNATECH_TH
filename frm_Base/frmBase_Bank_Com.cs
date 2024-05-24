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
    public partial class frmBase_Bank_Com : clsForm_Extends
    {
        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        cls_Grid_Base cgb = new cls_Grid_Base();
        private const string base_db_name = "tbl_BankForCompany";
        private long Data_Set_Form_TF = 0;
        public frmBase_Bank_Com()
        {
            InitializeComponent();
        }

          
  


        private void frmBase_Resize(object sender, EventArgs e)
        {
            //int base_w = this.Width / 5;
            //butt_Clear.Width = base_w;
            //butt_Save.Width = base_w;
            //butt_Excel.Width = base_w;
            //butt_Delete.Width = base_w;
            //butt_Exit.Width = base_w;

            //butt_Clear.Left = 0;
            //butt_Save.Left = butt_Clear.Left + butt_Clear.Width;
            //butt_Excel.Left = butt_Save.Left + butt_Save.Width;
            //butt_Delete.Left = butt_Excel.Left + butt_Excel.Width;
            //butt_Exit.Left = butt_Delete.Left + butt_Delete.Width;

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



        private void Base_Grid_Set()
        {
                           
            
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select BankCode,BankName,BankPenName, BankAccountNumber ,BankOwnerName , Isnull(nationNameEng,'') nationNameEng , Na_code ";
            Tsql = Tsql + " From tbl_BankForCompany  (nolock)  ";
            Tsql = Tsql + " LEFT JOIN  tbl_Nation  (nolock) ON tbl_Nation.nationCode = tbl_BankForCompany.Na_Code  ";

            if (tab_Nation.Visible == true)
            {
                if (combo_Se_Code.Text != "")
                {
                    Tsql = Tsql + " Where tbl_BankForCompany.Na_Code = '" + combo_Se_Code.Text + "'";
                }
            }

            Tsql = Tsql + " Order by BankCode ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            
            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, string[]> gr_dic_text = new Dictionary<int, string[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt-1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

            }

            cgb.grid_name = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Data_Put();
            //dGridView_Base.RowCount = dGridView_Base.RowCount + 1;
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
      
                                   
            
            //dGridView_Base.columnh = System.Windows.Forms.dGridView_BaseColumnSortMode.NotSortable;                                       
            //dGridView_Base.DataSource = ds.Tables[base_db_name];            
                        
            //for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            //{
            //    dGridView_BaseRowHeaderCell headerCell = dGridView_Base.Rows[i].HeaderCell;

            //    headerCell.Value = (i + 1).ToString();
            //    headerCell.Style.Alignment = dGridView_BaseContentAlignment.MiddleCenter;
            //    headerCell.Style.Font = new Font(dGridView_Base.DefaultCellStyle.Font, FontStyle.Bold);
            //}
            
        }

        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 7;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = { "은행_코드" , "은행명"   , "계좌가명"  ,"계좌번호"   , "_예금주"                                       
                                   ,"소속국가" ,""
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 150, 200, 200, 0                               
                            ,cls_app_static_var.Using_Multi_language , 0
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                   ,true  ,true  
                                   };
            cgb.grid_col_Lock = g_ReadOnly;
     
            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter , 
                               DataGridViewContentAlignment.MiddleCenter, 
                               DataGridViewContentAlignment.MiddleCenter,  
                               DataGridViewContentAlignment.MiddleCenter ,
                               DataGridViewContentAlignment.MiddleCenter,  //5
                               
                                DataGridViewContentAlignment.MiddleCenter  ,                              
                                DataGridViewContentAlignment.MiddleCenter
                               //DataGridViewContentAlignment.MiddleCenter,
                               //DataGridViewContentAlignment.MiddleRight 
                              };
            cgb.grid_col_alignment = g_Alignment;            
        }

       


                

        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, string[]> gr_dic_text, int fi_cnt)
        {
            string[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0].ToString() , 
                                ds.Tables[base_db_name].Rows[fi_cnt][1].ToString() , 
                                ds.Tables[base_db_name].Rows[fi_cnt][2].ToString() , 
                                encrypter.Decrypt ( ds.Tables[base_db_name].Rows[fi_cnt][3].ToString())  , 
                                ds.Tables[base_db_name].Rows[fi_cnt][4].ToString() ,
                                
                                ds.Tables[base_db_name].Rows[fi_cnt][5].ToString()  ,
                                ds.Tables[base_db_name].Rows[fi_cnt][6].ToString() 
                                //ds.Tables[base_db_name].Rows[fi_cnt][7].ToString() ,
                                //ds.Tables[base_db_name].Rows[fi_cnt][8].ToString() ,
                                 };

            gr_dic_text[fi_cnt+1] = row0;
        }




        private void frmBase_Bank_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape)
            //{
            //    this.Close();
            //}// end if


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

                          //  cls_form_Meth cfm = new cls_form_Meth();
                          //  cfm.form_Group_Panel_Enable_True(this);
                        }
                    }
                }// end if
            }



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



            Button T_bt = butt_Exit;
            if (e.KeyValue == 123)
                T_bt = butt_Exit;    //닫기  F12
            if (e.KeyValue == 113)
                T_bt = butt_Save;     //저장  F2
            if (e.KeyValue == 115)
                T_bt = butt_Delete;   // 삭제  F4
            if (e.KeyValue == 119)
                T_bt = butt_Excel;    //엑셀  F8    
            if (e.KeyValue == 112)
                T_bt = butt_Clear;    //엑셀  F1   

            EventArgs ee1 = null;
            if (e.KeyValue == 123 || e.KeyValue == 113 || e.KeyValue == 115 || e.KeyValue == 119 || e.KeyValue == 112)                
                cmdSave_Click(T_bt, ee1);     
        }




        private void txtBank_Enter(object sender, EventArgs e)
        {         
            cls_Check_Text T_R = new cls_Check_Text();
            T_R.Text_Focus_All_Sel((TextBox)sender);
            TextBox tb = (TextBox)sender;
            if (tb.ReadOnly == false)
                tb.BackColor = cls_app_static_var.txt_Focus_Color;  //Color.FromArgb(239, 227, 240);   
        }

        private void txtData_Base_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.ReadOnly == false)
                tb.BackColor = Color.White;
        }

        private void txtBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);            
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

        private void txtBank_TextChanged(object sender, EventArgs e)
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

         

            if (tb.Name == "txtBank")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtBank_Code.Text = "";
                Data_Set_Form_TF = 0 ;
            }

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
                {
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);
                    cgb_Pop.Next_Focus_Control = txtAccount;
                }
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
                    if (combo_Se_Code.Text.Trim () != "" )    Tsql = Tsql + " Where   Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);
                    cgb_Pop.Next_Focus_Control = txtAccount;
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
                Tsql = Tsql + " Where (Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%')";
                if (combo_Se_Code.Enabled == false)
                    Tsql = Tsql + " And   Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 
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
  




        private Boolean Check_TextBox_Error()
        {      
            cls_Check_Text T_R = new cls_Check_Text();

            string me = T_R.Text_Null_Check(txtBank);
            if (me != "")
            {
                MessageBox.Show(me);         
                return false;
            }

            me = T_R.Text_Null_Check(txtBank_Code);
            if (me != "")
            {
                MessageBox.Show(me);                
                return false;
            }

            if (txtKey.Text.Trim() == "")
            {
                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                string Tsql;
                Tsql = "Select BankCode ";
                Tsql = Tsql + " From tbl_BankForCompany  (nolock)  ";
                Tsql = Tsql + " Where BankCode = '" + txtBank.Text.Trim() + "'";
                Tsql = Tsql + " And  BankAccountNumber  = '" + txtAccount.Text.Trim() + "'";
                Tsql = Tsql + " And   Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 
                Tsql = Tsql + " Order by BankCode ASC ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_Code") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txtBank.Select();
                    return false;
                }


                Tsql = "Select BankCode ";
                Tsql = Tsql + " From tbl_BankForCompany  (nolock)  ";
                Tsql = Tsql + " Where BankPenName = '" + txtAccount_Name.Text.Trim() + "'";
                Tsql = Tsql + " And   Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 
                Tsql = Tsql + " Order by BankCode ASC ";

                ds.Clear();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_Name") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txtBank_Code.Select();
                    return false;
                }

                //++++++++++++++++++++++++++++++++
            }
            else
            {
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                string Tsql;
                Tsql = "Select BankCode ";
                Tsql = Tsql + " From tbl_BankForCompany  (nolock)  ";
                Tsql = Tsql + " Where BankCode <> '" + txtBank.Text.Trim() + "'";
                Tsql = Tsql + " And   BankAccountNumber  <> '" + txtAccount.Text.Trim() + "'";
                Tsql = Tsql + " And   BankPenName = '" + txtAccount_Name.Text.Trim() + "'";
                Tsql = Tsql + " And   Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 
                Tsql = Tsql + " Order by BankCode ASC ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
                {
                    
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_Name") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action") );
                    txtBank_Code.Select();
                    return false;
                }
            }
                        
            return true;
        }


        private void cmdSave_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            if (bt.Name == "butt_Clear")
            {
                txtBank.ReadOnly =false ;
                txtAccount.ReadOnly = false;
                txtBank.BackColor =SystemColors.Window;
                txtAccount.BackColor = SystemColors.Window;
                cls_form_Meth ct = new cls_form_Meth();                                
                ct.from_control_clear(this, txtBank);
            }
            else if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex;
                Save_Base_Data(ref Save_Error_Check);

                if (Save_Error_Check > 0)
                {
                    txtBank.ReadOnly = false; txtAccount.ReadOnly = false;
                    txtBank.BackColor = SystemColors.Window; txtAccount.BackColor = SystemColors.Window;

                    cls_form_Meth ct = new cls_form_Meth();
                    ct.from_control_clear(this, txtBank);

                    Base_Grid_Set();                    
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
                Delete_Base_Data(ref Del_Error_Check);
                if (Del_Error_Check > 0)
                {
                   
                    txtBank.ReadOnly = false; txtAccount.ReadOnly = false;
                    txtBank.BackColor = SystemColors.Window; txtAccount.BackColor = SystemColors.Window;
                    cls_form_Meth ct = new cls_form_Meth();
                    ct.from_control_clear(this, txtBank);

                    Base_Grid_Set();
                }
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
            Excel_Export_File_Name = this.Text; // "Company_Bank";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }



        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
             if (Check_TextBox_Error() == false) return;                        

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string Tsql;
            Tsql = "Select BankCode ";
            Tsql = Tsql + " From tbl_BankForCompany   (nolock) ";
            Tsql = Tsql + " Where BankCode = '" + txtBank_Code.Text.Trim() + "'";
            Tsql = Tsql + " And  BankAccountNumber  = '" + encrypter.Encrypt ( txtAccount.Text.Trim()) + "'";
            Tsql = Tsql + " And   Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 
            Tsql = Tsql + " Order by BankCode ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)//동일한 은행 코드가없네 그럼 인설트
            {
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

                Tsql = "Insert Into tbl_BankForCompany (BankCode , BankName ,BankPenName , BankOwnerName ";
                Tsql = Tsql + ",BankAccountNumber , Na_Code "; 
                Tsql = Tsql + ",RecordID , recordtime  ) ";
                Tsql = Tsql + "  Values (";
                Tsql = Tsql + "'" + txtBank_Code.Text.Trim() + "','" + txtBank.Text.Trim() + "'";
                Tsql = Tsql + ",'" + txtAccount_Name.Text.Trim() + "','" + txtName_Accnt.Text.Trim() + "'";
                Tsql = Tsql + ",'" + encrypter.Encrypt(txtAccount.Text.Trim()) + "','" + combo_Se_Code.Text.Trim() + "'";
                Tsql = Tsql + ",'" + cls_User.gid + "'";
                Tsql = Tsql + ", Convert(Varchar(25),GetDate(),21) ";
                Tsql = Tsql + " ) ";

                if (Temp_Connect.Insert_Data( Tsql, base_db_name,this.Name.ToString (), this.Text ) == false) return;

                Save_Error_Check =1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
            }
            else //동일한 은행 코드가 있구나 그럼 업데이트
            {
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

                Tsql = "Update tbl_BankForCompany Set ";
                Tsql = Tsql + "  BankPenName = '" + txtAccount_Name.Text.Trim() + "'";
                Tsql = Tsql + " ,BankOwnerName = '" + txtName_Accnt.Text.Trim() + "'";
                Tsql = Tsql + " ,Na_Code = '" + combo_Se_Code.Text.Trim() + "'";
                Tsql = Tsql + " Where BankCode = '" + txtBank_Code.Text.Trim() + "'";
                Tsql = Tsql + " And  BankAccountNumber  = '" + encrypter.Encrypt (txtAccount.Text.Trim()) + "'";
                Tsql = Tsql + " And   Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 

                if (Temp_Connect.Update_Data(Tsql, this.Name.ToString(), this.Text) == false) return;

                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));               
            }                       
            
        }




        private void Delete_Base_Data(ref int Del_Error_Check)
        {
            Del_Error_Check = 0;
            if (Check_TextBox_Error(1) == false) return;

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string Tsql;
            Tsql = "Delete From tbl_BankForCompany ";
            Tsql = Tsql + " Where BankCode = '" + txtBank_Code.Text.Trim() + "'";
            Tsql = Tsql + " And   Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 
            Tsql = Tsql + " And  BankAccountNumber  = '" + encrypter.Encrypt (txtAccount.Text.Trim()) + "'";

            if (Temp_Connect.Delete_Data (Tsql, base_db_name, this.Name.ToString(), this.Text) == false) return;

            Del_Error_Check = 1;
            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del"));
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
            Tsql = "Select C_Code ";
            Tsql = Tsql + " From tbl_Sales_Cacu  (nolock) ";
            Tsql = Tsql + " Where C_Code = '" + txtBank_Code.Text.Trim() + "'";
            Tsql = Tsql + " And   C_Number1  = '" + encrypter.Encrypt (txtAccount.Text.Trim()) + "'";
            Tsql = Tsql + " And   C_TF = 2 "; // 무통장 결제 내역이다.
            //if (tab_Nation.Visible == true) Tsql = Tsql + " And   Na_Code = '" + combo_Se_Code.Text.Trim() + "'"; 
            Tsql = Tsql + " Order  by C_Code ASC ";

            ds.Clear();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Data") 
                + "\n" +
                cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                txtBank.Select();
                return false;
            }

            return true ;
        }






        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            //int rowcnt = (sender as DataGridView).CurrentCell.RowIndex;  
            if ((sender as DataGridView).CurrentRow.Cells[0].Value != null)
            {
                txtBank_Code.Text = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                txtBank.Text = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
                txtAccount_Name.Text = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString();
                txtAccount.Text = (sender as DataGridView).CurrentRow.Cells[3].Value.ToString();
                
                txtName_Accnt.Text = (sender as DataGridView).CurrentRow.Cells[4].Value.ToString();


                if (combo_Se.Enabled == true)
                {
                    combo_Se.Text = (sender as DataGridView).CurrentRow.Cells[5].Value.ToString();
                    combo_Se_Code.Text = (sender as DataGridView).CurrentRow.Cells[6].Value.ToString();
                }

                txtBank.ReadOnly = true;
                txtAccount.ReadOnly = true;
                txtBank.BackColor = cls_app_static_var.txt_Enable_Color;  //Color.AliceBlue;
                txtAccount.BackColor = cls_app_static_var.txt_Enable_Color;  //Color.AliceBlue;
                txtKey.Text = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
            }

        }

        private void frmBase_Bank_Com_Load(object sender, EventArgs e)
        {
           
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);


            if (tab_Nation.Visible == true)
            {
                cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
                cpbf.Put_NaCode_ComboBox(combo_Se, combo_Se_Code);
            }
            Base_Grid_Set();

            Data_Set_Form_TF = 0;

            ////txtBank_Code.BackColor = cls_app_static_var.txt_Enable_Color; 
        }
















    }
}
