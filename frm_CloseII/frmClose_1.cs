﻿using System;
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
    public partial class frmClose_1 : Form
    {

        cls_Grid_Base cgb = new cls_Grid_Base();
        private int Data_Set_Form_TF = 0;
        private string base_db_name = "tbl_CloseTotal_01";
        private string FromEndDate = ""; private string ToEndDate = ""; private string PayDate = "", PayDate2  = "" ;
        private int From_Load_TF = 0;
        private int Cl_F_TF = 0, ReCnt = 0 ;

        private string Base_Chang_Date___1 = "";

        private int MaxLevel = 0;

        Dictionary<string, cls_Close_Mem> Clo_Mem = new Dictionary<string, cls_Close_Mem>();
        Dictionary<string, cls_Close_Sell> Clo_Sell = new Dictionary<string, cls_Close_Sell>();

        cls_Close_Sell[] C_Sell;

        cls_Connect_DB Search_Connect = new cls_Connect_DB();
        SqlConnection Search_Conn = null; 

        public frmClose_1()
        {
            InitializeComponent();
        }



        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;
            From_Load_TF = 0;

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            FromEndDate = ""; ToEndDate = ""; PayDate = "";

            Data_Set_Form_TF = 1;
            Data_Set_Form_TF = 0;


            Search_Connect.Connect_DB();
            Search_Conn = Search_Connect.Conn_Conn();

            mtxtPayDate.Mask = cls_app_static_var.Date_Number_Fromat;


            txt_From.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_To.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_SellCnt.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ReCnt.BackColor = cls_app_static_var.txt_Enable_Color;

            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Pay);
            cfm.button_flat_change(butt_Exit);
        }
        
        private void frmClose_1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Search_Conn.Close();
            Search_Conn.Dispose();
        }

        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (From_Load_TF == 0)
            {
                From_Load_TF = 1;
                FromEndDate = Check_Close_Date();

                if (FromEndDate == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not2_Close_Date"));
                    this.Close();
                    return;
                }

                txt_From.Text = FromEndDate; 
                Base_Sub_Grid_Set(FromEndDate);
            }

            

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
                if (e.KeyValue == 123 )
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


        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
         
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

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "."))
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1,".") == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

        
        }
        

        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            //int Sw_Tab = 0;

            if ((sender is TextBox) == false) return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
                //Sw_Tab = 1;
            }           
        }
        
        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }

        

        private void butt_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private string Check_Close_Date()
        {
            string Tsql = "";
            string Max_Toenddate = "";
            Tsql = "Select Isnull (Max(ToEndDate),'') From  tbl_CloseTotal_01 (nolock) ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt != 0)
            {
                Max_Toenddate = ds.Tables[base_db_name].Rows[0][0].ToString();
            }
            
            if (Max_Toenddate != "")
            {
                Max_Toenddate = Max_Toenddate.Substring(0, 4) + '-' + Max_Toenddate.Substring(4, 2) + '-' + Max_Toenddate.Substring(6, 2);
                DateTime TodayDate = new DateTime();
                TodayDate = DateTime.Parse(Max_Toenddate);
                Max_Toenddate = TodayDate.AddDays(1).ToString("yyyy-MM-dd").Replace ("-","") ;                
            }
            else
            {
                ReCnt = 0;
                Tsql = "Select Isnull(Min(SellDate),'')  From   tbl_SalesDetail (nolock) ";

                DataSet ds2 = new DataSet();
                Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds2, this.Name, this.Text);
                ReCnt = Temp_Connect.DataSet_ReCount;
                if (ReCnt != 0)
                    Max_Toenddate = ds2.Tables[base_db_name].Rows[0][0].ToString();
            }


            return Max_Toenddate ;
        }



        private void  Base_Sub_Grid_Set(string Base_C_Date)
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 
                        

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            string Max_Toenddate = Base_C_Date.Substring(0, 4) + '-' + Base_C_Date.Substring(4, 2) + '-' + Base_C_Date.Substring(6, 2);  

            for (int fi_cnt = 0; fi_cnt <= 100; fi_cnt++)
            {
                object[] row0 = { Max_Toenddate };
                gr_dic_text[fi_cnt] = row0;             
                                
                DateTime TodayDate = new DateTime();
                TodayDate = DateTime.Parse(Max_Toenddate);
                Max_Toenddate = TodayDate.AddDays(1).ToString("yyyy-MM-dd") ;  
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }

       


        private void dGridView_Base_Sub_Header_Reset()
        {
            cgb.grid_col_Count = 1;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;            
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            cgb.Sort_Mod_Auto_TF = 1;

            string[] g_HeaderText = {"미마감일자" 
                                };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 120 
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true 
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter                               
                              };
            cgb.grid_col_alignment = g_Alignment;
        }

        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            // SendKeys.Send("{TAB}");
        }


        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            ToEndDate = ""; txt_To.Text = "";

            if ((sender as DataGridView).CurrentRow.Cells[0].Value != null)
            {
                if ((sender as DataGridView).CurrentRow.Cells[0].Value.ToString() != "")
                {
                    ToEndDate = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString().Replace ("-","");                    
                    txt_To.Text = ToEndDate;
                    Close_Base_Work();

                    mtxtPayDate.Focus();
                }
            }
        }

        private void Close_Base_Work()
        {
            string StrSql = "";
        
            StrSql = "select Isnull(Count(Mbid),0) from tbl_SalesDetail ";
            //StrSql = StrSql + " LEFT JOIN tbl_SalesDetail_TF ON tbl_SalesDetail.OrderNumber = tbl_SalesDetail_TF.OrderNumber";
            StrSql = StrSql + " Where SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " And TotalPrice > 0 ";
            StrSql = StrSql + " And     Ga_Order = 0 ";            
                             
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            txt_SellCnt.Text = "0";
            if (ReCnt != 0)            
                txt_SellCnt.Text =  ds.Tables[base_db_name].Rows[0][0].ToString();


            StrSql = "select Isnull(Count(Mbid),0) from tbl_SalesDetail ";
           // StrSql = StrSql + " LEFT JOIN tbl_SalesDetail_TF ON tbl_SalesDetail.OrderNumber = tbl_SalesDetail_TF.OrderNumber";
            StrSql = StrSql + " Where SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " And TotalPrice < 0 ";
            
            DataSet ds2 = new DataSet();
            ReCnt = 0;
            Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds2, this.Name, this.Text);
            ReCnt = Temp_Connect.DataSet_ReCount;

            txt_ReCnt.Text = "0";
            if (ReCnt != 0)
                txt_ReCnt.Text = ds2.Tables[base_db_name].Rows[0][0].ToString();


            string PayDate = "";

            PayDate = ToEndDate.Substring(0, 4) + '-' + ToEndDate.Substring(4, 2) + '-' + ToEndDate.Substring(6, 2);
            DateTime TodayDate = new DateTime();
            TodayDate = DateTime.Parse(PayDate);
            PayDate = TodayDate.AddDays(5).ToString("yyyy-MM-dd").Replace("-", "");
            mtxtPayDate.Text = PayDate;
        }



        private Boolean Search_Check_TextBox_Error()
        {


            if (txt_To.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                      + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_CloseDate2")
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridView_Base.Focus(); return false;
            }

            if (mtxtPayDate.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                      + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_PayDate")
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtPayDate.Focus(); return false;
            }

            if (mtxtPayDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtPayDate.Text, mtxtPayDate, "Date") == false)
                {
                    mtxtPayDate.Focus();
                    return false;
                }
            }

            




            //string StrSql = "" ;

            //StrSql = "Select ToEndDate From tbl_CloseTotal_02 (nolock) " ;
            //StrSql = StrSql + " Where ToEndDate >= '" + ToEndDate + "'";

            ////++++++++++++++++++++++++++++++++
            //cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            //DataSet ds = new DataSet();
            //ReCnt = 0;
            //Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text);
            //ReCnt = Temp_Connect.DataSet_ReCount;

            //if (ReCnt <= 0)
            //{
            //    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Week2"));
            //    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Day2"));
            //    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Month2"));
            //    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Per2"));
            //    return false;
            //}


            if (txtB1.Text == "") txtB1.Text = "0";
            if (txtB2.Text == "") txtB2.Text = "0";
            if (txtB3.Text == "") txtB3.Text = "0";
            if (txtB4.Text == "") txtB4.Text = "0";
            if (txtB5.Text == "") txtB5.Text = "0";
            if (txtB6.Text == "") txtB6.Text = "0";
            if (txtB7.Text == "") txtB7.Text = "0";
            if (txtB8.Text == "") txtB8.Text = "0";
            if (txtB8.Text == "") txtB9.Text = "0";
            if (txtB9.Text == "") txtB9.Text = "0";
            if (txtB10.Text == "") txtB10.Text = "0";
            if (txtB11.Text == "") txtB11.Text = "0";
            if (txtB12.Text == "") txtB12.Text = "0";
            if (txtB13.Text == "") txtB13.Text = "0";
            if (txtB14.Text == "") txtB14.Text = "0";

            if (txtB15.Text == "") txtB15.Text = "0";
            if (txtB16.Text == "") txtB16.Text = "0";
            if (txtB17.Text == "") txtB17.Text = "0";
            if (txtB18.Text == "") txtB18.Text = "0";


            if (txtB20.Text == "") txtB20.Text = "0";
            if (txtB21.Text == "") txtB21.Text = "0";
            if (txtB22.Text == "") txtB22.Text = "0";
            if (txtB23.Text == "") txtB23.Text = "0";
            if (txtB24.Text == "") txtB24.Text = "0";
            if (txtB25.Text == "") txtB25.Text = "0";
            if (txtB26.Text == "") txtB26.Text = "0";
            if (txtB27.Text == "") txtB27.Text = "0";
            if (txtB28.Text == "") txtB28.Text = "0";
            if (txtB29.Text == "") txtB29.Text = "0";
            if (txtB30.Text == "") txtB30.Text = "0";

            if (txtB31.Text == "") txtB31.Text = "0";

            return true;
        }


        private void butt_Pay_Click(object sender, EventArgs e)
        {
            if (Search_Check_TextBox_Error() == false) return;

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_Start"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            pg1.Visible = true; pg1.Maximum = 0;
            pg2.Visible = true; pg2.Maximum = 0;
            butt_Pay.Enabled = false; butt_Exit.Enabled = false;
            tableLayoutPanel1.Enabled = false;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            int Close_Sucess_TF = 0;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            if (cls_User.SuperUserID == cls_User.gid)
            {
                Close_Work_Real(Temp_Connect, Conn, tran);
                tran.Commit();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_End"));
                tran.Dispose(); Temp_Connect.Close_DB();
                Close_Sucess_TF = 0; 
                Close_End(Close_Sucess_TF);
            }
            else
            {
                try
                {
                    Close_Work_Real(Temp_Connect, Conn, tran);

                    tran.Commit();
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_End"));
                    Close_Sucess_TF = 0; 
                }
                catch (Exception)
                {
                    tran.Rollback();
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_Err"));
                    Close_Sucess_TF = 1 ;
                }

                finally
                {
                    tran.Dispose();    Temp_Connect.Close_DB();
                    Close_End(Close_Sucess_TF);                    
                }       
            }     

        }

        private void Close_End(int Close_Sucess_TF )
        {
            if (Close_Sucess_TF == 1)
            {
                this.Close();
                return;
            }

            pg1.Visible = false; pg1.Maximum = 10;
            pg2.Visible = false; pg2.Maximum = 10;
            
            this.Cursor = System.Windows.Forms.Cursors.Default;
            butt_Pay.Enabled = true; butt_Exit.Enabled = true;
            tableLayoutPanel1.Enabled = true;

            FromEndDate = ""; ToEndDate = ""; PayDate = "";
            mtxtPayDate.Text = ""; txt_To.Text = ""; txt_From.Text = "";
            txt_SellCnt.Text = ""; txt_ReCnt.Text = "";
            Clo_Mem.Clear(); Clo_Mem = null;

            if (Clo_Sell != null)
            {
                Clo_Sell.Clear(); Clo_Sell = null;
            }
            FromEndDate = Check_Close_Date();

            if (FromEndDate == "")
            {                
                this.Close();
                return;
            }

            txt_From.Text = FromEndDate;
            Base_Sub_Grid_Set(FromEndDate);
        }



        private void Close_Work_Real(cls_Connect_DB Temp_Connect , SqlConnection Conn, SqlTransaction tran)
        {
            pg2.Minimum = 0;            pg2.Maximum = 26;
            pg2.Step = 1;               pg2.Value = 0;
            pg1.Step = 1;

            Base_Chang_Date___1 = "20170227";

            Cl_F_TF = 1;
            PayDate = mtxtPayDate.Text.Replace ("-","").Trim ();
            Make_Close_Table(Temp_Connect, Conn, tran);            
            pg2.PerformStep() ; pg2.Refresh();

            Put_Leave_StopDate(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            Put_Member_Base_Info(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            Put_LevelCnt_Update(Temp_Connect, Conn, tran);

            Put_Sell_Date(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            Put_SellPV(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            Put_DayPV(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

           
            ReqTF1(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            
            //////////string StrSql = " Update tbl_ClosePay_01 SET CurPoint = BePoint" ;
            //////////Temp_Connect.Insert_Data(StrSql, Conn, tran);

            //////////int G_Cnt = 0; string S_ToEndDate  ="";

            //////////S_ToEndDate = FromEndDate;
            //////////G_Cnt = 0;
            //////////while (int.Parse (S_ToEndDate) <= int.Parse (ToEndDate))
            //////////{
            //////////    CurPoint_Put_2(Temp_Connect, Conn, tran, S_ToEndDate);
            //////////    CurPoint_Put_3( Temp_Connect, Conn, tran, S_ToEndDate);
            //////////    G_Cnt ++;

            //////////    S_ToEndDate = FromEndDate.Substring(0, 4) + '-' + FromEndDate.Substring(4, 2) + '-' + FromEndDate.Substring(6, 2);
            //////////    DateTime TodayDate = new DateTime();
            //////////    TodayDate = DateTime.Parse(S_ToEndDate);
            //////////    S_ToEndDate = TodayDate.AddDays(G_Cnt).ToString("yyyy-MM-dd").Replace("-", "");

            //////////}
            //////////pg2.PerformStep(); pg2.Refresh();


            ////////Put_OrgGrade(Temp_Connect, Conn, tran);     
            ////////pg2.PerformStep(); pg2.Refresh();

            ////////Put_Self_PV(Temp_Connect, Conn, tran);
            ////////pg2.PerformStep(); pg2.Refresh();

            ////////Put_Cut_PV_4_1(Temp_Connect, Conn, tran);
            ////////pg2.PerformStep(); pg2.Refresh();
                       

            ////////Put_Mem_Sell_Info(Temp_Connect, Conn, tran);
            ////////pg2.PerformStep(); pg2.Refresh();


            ////////Put_Down_PV_01(Temp_Connect, Conn, tran);
            ////////pg2.PerformStep(); pg2.Refresh();

            if (int.Parse(FromEndDate) >= int.Parse(Base_Chang_Date___1))
            {

                if (int.Parse(FromEndDate) == int.Parse(Base_Chang_Date___1))
                {
                    string StrSql = "Update tbl_ClosePay_01 SET ";
                    StrSql = StrSql + " CurPoint=ISNULL(B.Cur_Point,0) ";
                    StrSql = StrSql + " ,BePoint=ISNULL(B.Cur_Point,0) ";
                    
                    StrSql = StrSql + " FROM  tbl_ClosePay_01  A, ";

                    StrSql = StrSql + " (Select Mbid2,Cur_Point ";
                    StrSql = StrSql + " From Sheet_Cur_point (nolock) ";
                    StrSql = StrSql + " ) B";

                    StrSql = StrSql + " Where A.Mbid2 = B.Mbid2 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                }

                if (int.Parse(FromEndDate) >= 20170724)  //0731 인데 수당만 그때고 등급은 7월 24일 마감부터
                    CurPoint_Put_2017_0731(Temp_Connect, Conn, tran);  //새로인 생긴 등급을 넣어준다.
                else
                    CurPoint_Put(Temp_Connect, Conn, tran);  //새로인 생긴 등급을 넣어준다.

                GiveShamGrade_P(Temp_Connect, Conn, tran);      //등급 인정을 넣는다.            
                pg2.PerformStep(); pg2.Refresh();

                Put_Down_SumPV(Temp_Connect, Conn, tran);  //하선의 매출을 총 잡아준다 pv

                CurGrade_OrgGrade_Put(Temp_Connect, Conn, tran);  //마케팅 변경 전의 직급을 주마감에서 가져온다.
                
                GiveShamGrade(Temp_Connect, Conn, tran);                
                //--------------------------------------------------------------

                if (int.Parse(FromEndDate) >= 20170731)
                {
                    
                    string StrSql = "Update tbl_ClosePay_01 Set ";
                    StrSql = StrSql + " GradeDate05 = GradeDate1";                        
                    StrSql = StrSql + " Where GradeDate1 <>''";
                    StrSql = StrSql + " And  GradeDate05 = ''";
                        
                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    
                    GiveGrade05(Temp_Connect, Conn, tran);  //매니져
                }
                

                GiveGrade1(Temp_Connect, Conn, tran);  //매니져
                GiveGrade2(Temp_Connect, Conn, tran);  //팀매니져
                GiveGrade3(Temp_Connect, Conn, tran);  //그룹매니져
                GradeUpLine2(30, Temp_Connect, Conn, tran);

                GiveGrade4(Temp_Connect, Conn, tran);   //마스터
                GradeUpLine2(40, Temp_Connect, Conn, tran);  

                GiveGrade5(Temp_Connect, Conn, tran); //스타마스타
                GradeUpLine2(50, Temp_Connect, Conn, tran);  

                GiveGrade6(Temp_Connect, Conn, tran); //임페리얼
                GradeUpLine2(30, Temp_Connect, Conn, tran);  
                GradeUpLine2(40, Temp_Connect, Conn, tran);  
                GradeUpLine2(50, Temp_Connect, Conn, tran);

                Put_ReqTF2_OneGrade(Temp_Connect, Conn, tran);
                //Put_ReqTF2_OneGrade_R(Temp_Connect, Conn, tran);
                
               
            }
           
            Put_cls_Close_Mem(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
                        
            //--------------------------------------------------------------

            if (int.Parse(FromEndDate) >= int.Parse (Base_Chang_Date___1) )
            {
                
                Put_Down_PV_01(Temp_Connect, Conn, tran);  //일반 매출만 잡아준다.       
                Put_Down_PV_01_After(Temp_Connect, Conn, tran);  //일반 매출만 잡아준다.         //후원인 없어서 누적 안잡히다 후원인 생기면 한번에 다 누적 잡아주게함.
                pg2.PerformStep(); pg2.Refresh();

                Put_Down_PV_Re(Temp_Connect, Conn, tran);  //반품 내역을 위로 올려주면서 빼준다.
                pg2.PerformStep(); pg2.Refresh();

                Put_Down_PV_02(Temp_Connect, Conn, tran);  //하선 누적 관련 사항을 합산 한다.
                pg2.PerformStep(); pg2.Refresh();
                //--------------------------------------------------------------

                Give_Allowance6(Temp_Connect, Conn, tran); //후원수당
                pg2.PerformStep(); pg2.Refresh();

                
                
                Give_Allowance8(Temp_Connect, Conn, tran); //추천관리수당    동급 적용도 같이 준다.

               
                if (int.Parse(FromEndDate) >= 20170731)
                    Give_Allowance7_2017_0731(Temp_Connect, Conn, tran); //후원수당에 대한        추천 매칭수당
                else
                    Give_Allowance7(Temp_Connect, Conn, tran); //후원수당에 대한        추천 매칭수당
                pg2.PerformStep(); pg2.Refresh();



                if (int.Parse(FromEndDate) >= 20170731 && int.Parse(FromEndDate) < 20180702)  //2018-07-02일 마감부터는 이게 적용안된다.
                     Give_Allowance10_2017_0731(Temp_Connect, Conn, tran); // 직급수당  10 , 11 , 12
                
            }
            else
            {

                if (int.Parse(FromEndDate) >= 20160725)
                {
                    Give_Allowance1_20160725(Temp_Connect, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();


                    if (int.Parse(FromEndDate) >= 20160816)
                        Give_Allowance1_Begin_20160816(Temp_Connect, Conn, tran);  //5번 비긴즈수당
                    else
                        Give_Allowance1_Begin_20160725(Temp_Connect, Conn, tran);  //5번 비긴즈수당
                    pg2.PerformStep(); pg2.Refresh();
                }
                else
                {
                    Give_Allowance1(Temp_Connect, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();

                    Give_Allowance1_Begin(Temp_Connect, Conn, tran);  //5번 비긴즈수당
                    pg2.PerformStep(); pg2.Refresh();
                }
                //--------------------------------------------------------------

                Give_Allowance2(Temp_Connect, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                Give_Allowance2_Begin(Temp_Connect, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();
                //--------------------------------------------------------------


                Give_Allowance3(Temp_Connect, Conn, tran);  //직적판매수당 B플랜
                pg2.PerformStep(); pg2.Refresh();


                Give_Allowance4(Temp_Connect, Conn, tran); //추천수당 B플랜
                pg2.PerformStep(); pg2.Refresh();
            }

            //Give_Allowance2_TEST(Temp_Connect, Conn, tran);
            //Give_Allowance3(Temp_Connect, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();
            




            //--------------------------------------------------------------
            Put_Return_Pay(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            Put_Sum_Return_Remain_Pay(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------


            //--------------------------------------------------------------
            CalculateTruePayment(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            
            Chang_RetunPay_Table(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------



            //--------------------------------------------------------------
            tbl_CloseTotal_Put1(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            tbl_CloseTotal_Put2(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            tbl_CloseTotal_Put3(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------




            //--------------------------------------------------------------
            MakeModForCheckRequirement1(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            ReadyNewForCheckRequirement1(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            Check_Close_Gid(Temp_Connect, Conn, tran,1,0);
            pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------


        }


        private void Make_Close_Table(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Minimum = 0; pg1.Maximum = 40; pg1.Refresh(); 
            
            pg1.Value = 10; ; pg1.Refresh(); 
            //pg1.PerformStep(); pg1.Refresh(); 

            string StrSql = "";

            StrSql = "INSERT INTO tbl_ClosePay_01 (Mbid,Mbid2,RecordMakeDate)  ";
            StrSql = StrSql + " Select   A.Mbid,A.Mbid2,  '" + ToEndDate + "' From tbl_Memberinfo AS A  (nolock)  ";
            StrSql = StrSql + " LEFT Join tbl_ClosePay_01 AS B ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 ";
            StrSql = StrSql + " Where b.Mbid Is Null " ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
            pg1.Value = 20; pg1.Refresh(); 


            StrSql = "INSERT INTO tbl_ClosePay_01_Sell (Mbid,Mbid2,SellCode , RecordMakeDate)  ";
            StrSql = StrSql + " Select  distinct A.Mbid,A.Mbid2, A.SellCode, '" + ToEndDate + "' From tbl_SalesDetail AS A   (nolock) ";
            StrSql = StrSql + " LEFT Join tbl_ClosePay_01_Sell AS B ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 And A.SellCode = B.SellCode ";
            StrSql = StrSql + " Where  A.SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And    A.SellDate <= '" + ToEndDate + "'" ;
            StrSql = StrSql + " And b.Mbid Is Null ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
            pg1.Value = 30;  pg1.Refresh(); 


             StrSql = "INSERT INTO tbl_ClosePay_01_Sell (Mbid,Mbid2,SellCode, RecordMakeDate) ";
            StrSql = StrSql + " Select  distinct A.Mbid,A.Mbid2, A.SellCode,   '" + ToEndDate + "'  From tbl_Sham_Sell AS A   (nolock) ";
            StrSql = StrSql + " LEFT Join tbl_ClosePay_01_Sell AS B ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 And A.SellCode = B.SellCode";            
            StrSql = StrSql + " Where  A.Apply_Date >= '" + FromEndDate + "'" ;
            StrSql = StrSql + " And    A.Apply_Date <= '" + ToEndDate + "'";
            StrSql = StrSql + " And    B.Mbid IS NULL";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh(); 
            pg1.Value = 40;  pg1.Refresh(); 
            

        }


        private void  Put_Leave_StopDate(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 3    ;
            pg1.PerformStep(); pg1.Refresh(); 

            string StrSql = "";
    
            StrSql = "Update tbl_ClosePay_01 SET StopDate = ISNULL(B.PayStop_Date,'')" ;
           StrSql = StrSql + " FROM  tbl_ClosePay_01  A,";
           StrSql = StrSql + " (";
           StrSql = StrSql + " Select    PayStop_Date,Mbid,Mbid2   From tbl_Memberinfo   (nolock) ";
           StrSql = StrSql + " Where PayStop_Date <= '" + ToEndDate + "'";
           StrSql = StrSql + " And   PayStop_Date <>'' ";
           StrSql = StrSql + " ) B";
           StrSql = StrSql + "  Where a.Mbid = b.Mbid And a.Mbid2 = b.Mbid2";
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 

    
            StrSql = "Update tbl_ClosePay_01 SET LeaveDate=ISNULL(B.LeaveDate,'')";
           StrSql = StrSql + " FROM  tbl_ClosePay_01  A,";
           StrSql = StrSql + " (";
           StrSql = StrSql + " Select    LeaveDate,Mbid,Mbid2   From tbl_Memberinfo   (nolock) ";
           StrSql = StrSql + " Where LeaveDate <= '" + ToEndDate + "'";
           StrSql = StrSql + " And   LeaveDate <>'' ";
           StrSql = StrSql + " ) B";
           StrSql = StrSql + " Where a.Mbid = b.Mbid And a.Mbid2 = b.Mbid2";
    
             
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
        }




        private void Put_Member_Base_Info(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2    ;
            pg1.PerformStep(); pg1.Refresh(); 

            string StrSql = "";

            StrSql = "Update tbl_ClosePay_01 SET";
            StrSql = StrSql + " BankCode=ISNULL(B.BankCode,'')";
            StrSql = StrSql + " ,Cpno=ISNULL(B.Cpno,'')";
            StrSql = StrSql + " ,BankAcc=ISNULL(B.bankaccnt,'')";
            StrSql = StrSql + " ,BankOwner=ISNULL(B.BankOwner,'')";
            StrSql = StrSql + " ,M_Name=ISNULL(B.M_Name,'')";
            StrSql = StrSql + " ,BusCode=ISNULL(B.businesscode,'')";

            //StrSql = StrSql +" ,ED_Date=ISNULL(B.ED_Date,'')"

            StrSql = StrSql + " ,Saveid=ISNULL(B.Saveid,'')";
            StrSql = StrSql + " ,Saveid2=ISNULL(B.Saveid2,0)";
            StrSql = StrSql + " ,LineCnt=ISNULL(B.LineCnt,0)";

            StrSql = StrSql + " ,Nominid=ISNULL(B.Nominid,'')";
            StrSql = StrSql + " ,Nominid2=ISNULL(B.Nominid2,0)";
            StrSql = StrSql + " ,N_LineCnt=ISNULL(B.N_LineCnt,0)";

            //    StrSql = StrSql +" ,BaseMbid=ISNULL(B.BaseMbid,'')"
            //    StrSql = StrSql +" ,BaseMbid2=ISNULL(B.BaseMbid2,0)"             

            StrSql = StrSql + " ,Sell_Mem_TF = ISNULL(B.Sell_Mem_TF,0)";
            StrSql = StrSql + " ,RBO_Mem_TF = ISNULL(B.RBO_Mem_TF,0)";


            StrSql = StrSql + " ,RegTime=  replace(ISNULL(B.regtime,''),'-','')";
            StrSql = StrSql + "  FROM  tbl_ClosePay_01  A,";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   BankCode,Cpno,bankaccnt,BankOwner,M_Name,businesscode,ED_Date,";
            StrSql = StrSql + " Saveid,Saveid2,LineCnt,Nominid,Nominid2,N_LineCnt,";
            StrSql = StrSql + " Mbid,Mbid2,regtime , Sell_Mem_TF , RBO_Mem_TF   ";
            StrSql = StrSql + "  From tbl_Memberinfo   (nolock)   ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And a.Mbid2 = b.Mbid2";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_01 SET";
            //StrSql = StrSql + " BankCode=ISNULL(B.BankCode,'')";
            //StrSql = StrSql + " ,Cpno=ISNULL(B.Cpno,'')";
            //StrSql = StrSql + " ,BankAcc=ISNULL(B.BankAcc,'')";
            //StrSql = StrSql + " ,BankOwner=ISNULL(B.BankOwner,'')";
            //StrSql = StrSql + " ,M_Name=ISNULL(B.M_Name,'')";
            //StrSql = StrSql + " ,BusCode=ISNULL(B.BusCode,'')";

            ////StrSql = StrSql +" ,ED_Date=ISNULL(B.ED_Date,'')"

            //StrSql = StrSql + " ,Saveid=ISNULL(B.Saveid,'')";
            //StrSql = StrSql + " ,Saveid2=ISNULL(B.Saveid2,0)";
            //StrSql = StrSql + " ,LineCnt=ISNULL(B.LineCnt,0)";

            //StrSql = StrSql + " ,Nominid=ISNULL(B.Nominid,'')";
            //StrSql = StrSql + " ,Nominid2=ISNULL(B.Nominid2,0)";
            //StrSql = StrSql + " ,N_LineCnt=ISNULL(B.N_LineCnt,0)";

            //StrSql = StrSql + " ,LeaveDate=ISNULL(B.LeaveDate,'')";
            //StrSql = StrSql + " ,StopDate=ISNULL(B.StopDate,'')";

            //StrSql = StrSql + " ,LevelCnt=ISNULL(B.LevelCnt,0)";
            //StrSql = StrSql + " ,N_LevelCnt=ISNULL(B.N_LevelCnt,0)";          

            //StrSql = StrSql + " ,Sell_Mem_TF = ISNULL(B.Sell_Mem_TF,0)";
            //StrSql = StrSql + " ,RBO_Mem_TF = ISNULL(B.RBO_Mem_TF,0)";


            //StrSql = StrSql + " ,RegTime=  replace(ISNULL(B.RegTime,''),'-','')";
            //StrSql = StrSql + "  FROM  tbl_ClosePay_01  A,";

            //StrSql = StrSql + " (";
            //StrSql = StrSql + " Select   BankCode,Cpno,BankAcc,BankOwner,M_Name,BusCode,";
            //StrSql = StrSql + " Saveid,Saveid2,LineCnt,Nominid,Nominid2,N_LineCnt,";
            //StrSql = StrSql + " Mbid,Mbid2,RegTime , Sell_Mem_TF , RBO_Mem_TF ,LeaveDate,  StopDate, LevelCnt , N_LevelCnt  ";
            //StrSql = StrSql + "  From tbl_ClosePay_01_Mod_0703   (nolock)   ";
            //StrSql = StrSql + "  Where ToEndDate ='" + ToEndDate  +"'"; 
            //StrSql = StrSql + " ) B";
            //StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            //StrSql = StrSql + " And a.Mbid2 = b.Mbid2";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh(); 
        }



        private void Put_LevelCnt_Update(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string StrSql = " Select Mbid,Mbid2 From Tbl_Memberinfo  (nolock) Where Saveid='**'   ";
            string Mbid = ""; int Mbid2 = 0;
            ReCnt = 0;
            DataSet Dset4 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
            ReCnt = Search_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {
                pg1.Value = 0; pg1.Maximum = ReCnt + 1;
                pg1.PerformStep(); pg1.Refresh();

                pg1.Value = 0; pg1.Maximum = ReCnt;

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Mbid = Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                    Mbid2 = int.Parse(Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());

                    StrSql = "Update tbl_ClosePay_01 SET ";
                    StrSql = StrSql + " LevelCnt=ISNULL(B.lvl,0) ";
                    StrSql = StrSql + " FROM  tbl_ClosePay_01  A, ";

                    StrSql = StrSql + " (Select    empid0,empid,lvl ";
                    StrSql = StrSql + " From ufn_GetSubTree_Pay_01_Mem('" + Mbid + "'," + Mbid2;
                    StrSql = StrSql + ") Where pos <>0 ";
                    StrSql = StrSql + " ) B";

                    StrSql = StrSql + " Where A.Mbid=B.empid0 ";
                    StrSql = StrSql + " And   A.Mbid2=B.empid ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);

                    pg1.PerformStep(); pg1.Refresh();
                } // for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)

            } // (ReCnt != 0)




            StrSql = "Select Max(LevelCnt) From tbl_ClosePay_01  ";

            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;

            while (sr.Read())
            {
                MaxLevel = int.Parse(sr.GetValue(0).ToString());
            }

            sr.Close(); sr.Dispose();

            //pg1.Value = 0; pg1.Maximum = 4;
            //pg1.PerformStep(); pg1.Refresh();

            //StrSql = "INSERT INTO tbl_ClosePay_01_DownPV ";
            //StrSql = StrSql + " (Mbid,Mbid2,LineCnt,RecordMakeDate,LevelCnt) ";
            //StrSql = StrSql + " Select    a.Mbid,A.mbid2,0,'" + ToEndDate + "', 0 ";
            //StrSql = StrSql + " From (tbl_memberinfo as a ";
            //StrSql = StrSql + " Left join tbl_memberinfo as b on ";
            //StrSql = StrSql + " a.Mbid=b.saveid and a.Mbid2=b.saveid2) ";
            //StrSql = StrSql + " Where b.saveid is null ";
            //StrSql = StrSql + " And   a.LineCnt >0 ";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            //StrSql = "INSERT INTO tbl_ClosePay_01_DownPV ";
            //StrSql = StrSql + " (Mbid,Mbid2,LineCnt,RecordMakeDate,LevelCnt) ";
            //StrSql = StrSql + " Select    Saveid,Saveid2,LineCnt,'" + ToEndDate + "',0 ";
            //StrSql = StrSql + " From tbl_memberinfo ";
            //StrSql = StrSql + " Where LineCnt >0 ";
            //StrSql = StrSql + " And   Saveid <>'**'";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            //StrSql = "Update tbl_ClosePay_01_DownPV SET ";
            //StrSql = StrSql + " Saveid=ISNULL(B.Saveid,'') ";
            //StrSql = StrSql + " ,Saveid2=ISNULL(B.Saveid2,0) ";
            //StrSql = StrSql + " ,Curposition=ISNULL(B.LineCnt,0) ";
            //StrSql = StrSql + " ,LevelCnt=ISNULL(B.LevelCnt,0) ";

            //StrSql = StrSql + " FROM  tbl_ClosePay_01_DownPV  A, ";
            //StrSql = StrSql + " (Select    Saveid,Saveid2,LineCnt,Mbid,Mbid2,LevelCnt ";
            //StrSql = StrSql + " From tbl_ClosePay_01 ) B";
            //StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            //StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
        }


        private void Put_Sell_Date(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
         
            pg1.Value = 0; pg1.Maximum = 2    ;
            pg1.PerformStep(); pg1.Refresh(); 

            string StrSql = "";


            if (int.Parse(FromEndDate) == int.Parse(Base_Chang_Date___1))
            {
                StrSql = " Update tbl_ClosePay_01_Sell SET";
                StrSql = StrSql + " BeTotalPV=ISNULL(B.A5,0)";                
                StrSql = StrSql + " FROM  tbl_ClosePay_01_Sell  A,";

                StrSql = StrSql + " (";
                StrSql = StrSql + " Select  Sum(TotalPV) AS A5,      "; 
                StrSql = StrSql + " Mbid,Mbid2 , SellCode";
                StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
                StrSql = StrSql + " Where   SellDate < '" + FromEndDate + "'";
                StrSql = StrSql + " And   TotalPV  + TotalCV + TotalPrice > 0 ";
                StrSql = StrSql + " And   Ga_Order = 0 ";
                StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where a.Mbid = b.Mbid ";
                StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
                StrSql = StrSql + " And   a.SellCode = b.SellCode";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                StrSql = " Update tbl_ClosePay_01_Sell SET";                
                StrSql = StrSql + " BeReTotalPV=-ISNULL(B.A5,0)";                
                StrSql = StrSql + " FROM  tbl_ClosePay_01_Sell  A,";

                StrSql = StrSql + " (";
                StrSql = StrSql + " Select  Sum(TotalPV) AS A5 "; 
                StrSql = StrSql + " ,Mbid,Mbid2 , SellCode";
                StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
                StrSql = StrSql + " Where   SellDate < '" + FromEndDate + "'";
                StrSql = StrSql + " And   TotalPV  + TotalCV < 0 ";
                StrSql = StrSql + " And   Ga_Order = 0 ";
                StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where a.Mbid = b.Mbid ";
                StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
                StrSql = StrSql + " And   a.SellCode = b.SellCode";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                //StrSql = " Update tbl_ClosePay_01_Sell SET";
                //StrSql = StrSql + " BeShamSell = IsNull(b.A1, 0)";
                //StrSql = StrSql + " FROM  tbl_ClosePay_01_Sell  A,";
                //StrSql = StrSql + " (";
                //StrSql = StrSql + " Select    Sum(Apply_PV) AS A1, Mbid,Mbid2 , SellCode";
                //StrSql = StrSql + " From tbl_Sham_Sell  (nolock) ";
                //StrSql = StrSql + " Where   Apply_Date < '" + FromEndDate + "'";
                //StrSql = StrSql + " And     Apply_PV <> 0";
                //StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
                //StrSql = StrSql + " ) B";
                //StrSql = StrSql + " Where a.Mbid = b.Mbid ";
                //StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
                //StrSql = StrSql + " And   a.SellCode = b.SellCode";

                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }


    
            StrSql = "Select Mbid,Mbid2 ,OrderNumber, SellCode , TotalPrice , InputCash , InputCard , InputPassbook , TotalPV , TotalCV , Re_BaseOrderNumber , SellDate  ";
            StrSql = StrSql + " From    tbl_SalesDetail  (nolock)  ";
            StrSql = StrSql + " Where   SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     TotalPrice < 0 ";
            StrSql = StrSql + " And     SellCode <> '' " ;


            DataSet Dset = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn,  Dset);
            ReCnt = 0;            
            ReCnt = Search_Connect.DataSet_ReCount;
            
            
            pg1.Value = 0; pg1.Maximum = ReCnt;
            string Re_BaseOrderNumber = "", T_SellDate = "", RePayDate = "", Rs_SellDate = "";

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Re_BaseOrderNumber = Dset.Tables[base_db_name].Rows[fi_cnt]["Re_BaseOrderNumber"].ToString();
                Rs_SellDate = Dset.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                T_SellDate = ""; RePayDate = "";

                StrSql = "Select Mbid,Mbid2, OrderNumber, TotalPV , SellDate   From tbl_SalesDetail   (nolock) ";
                StrSql = StrSql + " Where OrderNumber ='" + Re_BaseOrderNumber + "'";

                DataSet Dset2 = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset2);
                int ReCnt2 = Search_Connect.DataSet_ReCount;
                if (ReCnt2 >0 )
                {
                    T_SellDate = Dset2.Tables[base_db_name].Rows[0]["SellDate"].ToString();
                }
           

                if (T_SellDate != "")
                {
                    StrSql = "Select ToEndDate,  PayDate   From  tbl_CloseTotal_01 (nolock) ";
                    StrSql = StrSql + " Where FromEndDate <='" + T_SellDate + "'";
                    StrSql = StrSql + " And   ToEndDate >='" + T_SellDate + "'";

                    DataSet Dset3 = new DataSet();
                    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset3);
                    int ReCnt3 = Search_Connect.DataSet_ReCount;
                    if (ReCnt3 > 0 )
                    {
                        RePayDate = Dset3.Tables[base_db_name].Rows[0]["ToEndDate"].ToString();
                    }
                }

                if (RePayDate != "")
                {
                    if (int.Parse(Rs_SellDate) > int.Parse(RePayDate))
                    {
                        StrSql = "Update tbl_ClosePay_01_Sell SET ";
                        StrSql = StrSql + "  DayReAmount = DayReAmount + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
                        StrSql = StrSql + " ,DayReCash = DayReCash + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["InputCash"].ToString());
                        StrSql = StrSql + " ,DayReCard = DayReCard + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["InputCard"].ToString());
                        StrSql = StrSql + " ,DayReBank = DayReBank + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["InputPassbook"].ToString());
                        StrSql = StrSql + " ,DayReTotalPV = DayReTotalPV + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
                        StrSql = StrSql + " ,DayReTotalCV = DayReTotalCV + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["TotalCV"].ToString());
                        StrSql = StrSql + "  Where Mbid  = '" + Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "'";
                        StrSql = StrSql + "  And   Mbid2 =  " + Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                        StrSql = StrSql + "  And   SellCode =  '" + Dset.Tables[base_db_name].Rows[fi_cnt]["SellCode"].ToString() + "'";

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    }
                }

                pg1.PerformStep(); pg1.Refresh();
            }

                    

            pg1.Value = 0; pg1.Maximum = 4  ;
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_01_Sell SET";
            StrSql = StrSql + " DayAmount = IsNull(b.A1, 0)";
            StrSql = StrSql + " ,DayCash=ISNULL(B.A2,0)";
            StrSql = StrSql + " ,DayCard=ISNULL(B.A3,0)";
            StrSql = StrSql + " ,DayBank=ISNULL(B.A4,0)";
            StrSql = StrSql + " ,DayTotalPV=ISNULL(B.A5,0)";
            StrSql = StrSql + " ,DayTotalCV=ISNULL(B.A6,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_01_Sell  A,";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select  ";
            StrSql = StrSql + " Sum(BS1.TotalPrice) + Isnull(Sum(Bs_R.TotalPrice),0)  AS A1,         Sum(BS1.InputCash)  + Isnull(Sum(Bs_R.InputCash),0)      AS A2, ";
            StrSql = StrSql + " Sum(BS1.InputCard)  + Isnull(Sum(Bs_R.InputCard),0)   AS A3 ,        Sum(BS1.InputPassbook)  + Isnull(Sum(Bs_R.InputPassbook),0)  AS A4 , ";
            StrSql = StrSql + " Sum(BS1.TotalPV)    + Isnull(Sum(Bs_R.TotalPV),0)     AS A5,         Sum(BS1.TotalCV)  + Isnull(Sum(Bs_R.TotalCV),0)        AS A6, ";
            StrSql = StrSql + " BS1.Mbid,BS1.Mbid2 , BS1.SellCode";
            StrSql = StrSql + " From tbl_SalesDetail AS BS1 (nolock) ";
            StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate <= '" + ToEndDate + "'";            
            StrSql = StrSql + " Where   BS1.SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     BS1.SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     BS1.TotalPV  + BS1.TotalCV >= 0 ";
            StrSql = StrSql + " And     BS1.SellCode <> '' ";
            StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
            StrSql = StrSql + " Group By BS1.Mbid,BS1.Mbid2, BS1.SellCode";
            StrSql = StrSql + " Having Sum(BS1.TotalPV) + IsNull(Sum(Bs_R.TotalPV), 0) >= 0";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
            StrSql = StrSql + " And   a.SellCode = b.SellCode";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();  //주문 종류 별로 넣는다. 합계를 +판매에 대해서만


        
            StrSql = " Update tbl_ClosePay_01_Sell SET";
            StrSql = StrSql + " DayShamSell = IsNull(b.A1, 0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_01_Sell  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select    Sum(Apply_PV) AS A1, Mbid,Mbid2 , SellCode";
            StrSql = StrSql + " From tbl_Sham_Sell (nolock)  ";
            StrSql = StrSql + " Where   Apply_Date >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     Apply_Date <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     Apply_PV <> 0";
            StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
            StrSql = StrSql + " And   a.SellCode = b.SellCode";
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();  //----매출인정을 넣는다.
    
    
    
            StrSql = " Update tbl_ClosePay_01_Sell Set";
            StrSql = StrSql + " SumAmount = BeAmount + DayAmount";
            StrSql = StrSql + " ,SumCash = BeCash + DayCash";
            StrSql = StrSql + " ,SumCard = BeCard + DayCard";
            StrSql = StrSql + " ,SumBank = BeBank + DayBank";
            StrSql = StrSql + " ,SumTotalPV = BeTotalPV + DayTotalPV";
            StrSql = StrSql + " ,SumShamSell = BeShamSell + DayShamSell";
    
            StrSql = StrSql + " ,SumReAmount = BeReAmount + DayReAmount";
            StrSql = StrSql + " ,SumReCash = BeReCash + DayReCash";
            StrSql = StrSql + " ,SumReCard = BeReCard + DayReCard";
            StrSql = StrSql + " ,SumReBank = BeReBank + DayReBank";
            StrSql = StrSql + " ,SumReTotalPV = BeReTotalPV + DayReTotalPV";
    
            StrSql = StrSql + " ,SumTotalCV = BeTotalCV + DayTotalCV";
            StrSql = StrSql + " ,SumReTotalCV = BeReTotalCV + DayReTotalCV";
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();  //----합계를 넣는다.
        }





        private void Put_SellPV(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            pg1.Value = 0; pg1.Maximum = 5    ;
            pg1.PerformStep(); pg1.Refresh(); 

            string StrSql = "";   
       

            StrSql = "Update tbl_ClosePay_01 SET " ;
            StrSql = StrSql + " SellPrice01=ISNULL(B.A1,0) " ;
            StrSql = StrSql + ",SellPv01=ISNULL(B.A2,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, " ;
    
            StrSql = StrSql + " (Select Sum(SumAmount-SumReAmount) AS A1, Sum(SumTotalPV-SumReTotalPV) AS A2 , Sum(SumTotalCV-SumReTotalCV) AS A3 , Sum(SumShamSell) AS A4 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_01_Sell " ;
            StrSql = StrSql + " Where SellCode ='01'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_01 SET " ;
            StrSql = StrSql + " SellSham01 = ISNULL(B.A4,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, " ;
    
            StrSql = StrSql + " (Select  Sum(SumShamSell) AS A4 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_01_Sell " ;
            StrSql = StrSql + " Where SellCode ='01'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_01 SET " ;
            StrSql = StrSql + " SellPrice02 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + ",SellPv02 = ISNULL(B.A2,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, " ;
    
            StrSql = StrSql + " (Select Sum(SumAmount-SumReAmount) AS A1, Sum(SumTotalPV-SumReTotalPV) AS A2 , Sum(SumTotalCV-SumReTotalCV) AS A3 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_01_Sell " ;
            StrSql = StrSql + " Where SellCode ='02'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_01 SET " ;
            StrSql = StrSql + " SellPrice03 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + ",SellPv03 = ISNULL(B.A2,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, " ;

            StrSql = StrSql + " (Select Sum(SumAmount-SumReAmount) AS A1, Sum(SumTotalPV-SumReTotalPV) AS A2 , Sum(SumTotalCV-SumReTotalCV) AS A3 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_01_Sell " ;
            StrSql = StrSql + " Where SellCode ='03'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;

            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

           Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
        }


     private void   Put_DayPV(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            pg1.Value = 0; pg1.Maximum = 5    ;
            pg1.PerformStep(); pg1.Refresh(); 

            string StrSql = "";   
  
            StrSql = "Update tbl_ClosePay_01 SET " ;
            StrSql = StrSql + "  DayPrice01 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,DayPv01 = ISNULL(B.A2,0) " ;
            //StrSql = StrSql + " ,DayCV01 = ISNULL(B.A3,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, " ;
    
            StrSql = StrSql + " (Select Sum(DayAmount-DayReAmount) AS A1 , Sum(DayTotalPV-DayReTotalPV) AS A2 ,Sum(DayTotalCV-DayReTotalCV) AS A3 ,Sum(DayShamSell) AS A4  ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_01_Sell " ;
            StrSql = StrSql + " Where SellCode ='01'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
            StrSql = "Update tbl_ClosePay_01 SET " ;
            StrSql = StrSql + " DaySham01 = ISNULL(B.A4,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, " ;
    
            StrSql = StrSql + " (Select  Sum(DayShamSell) AS A4 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_01_Sell " ;
            StrSql = StrSql + " Where SellCode ='01'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_01 SET " ;
            StrSql = StrSql + "  DayPrice02 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,DayPv02 = ISNULL(B.A2,0) " ;
            //trSql = StrSql + " ,DayCV02 = ISNULL(B.A3,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, " ;
            StrSql = StrSql + " (Select Sum(DayAmount-DayReAmount) AS A1 ,Sum(DayTotalPV-DayReTotalPV) AS A2  ,Sum(DayTotalCV-DayReTotalCV) AS A3,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_01_Sell " ;
            StrSql = StrSql + " Where SellCode ='02'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_01 SET " ;
            StrSql = StrSql + "  DayPrice03 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,DayPv03 = ISNULL(B.A2,0) " ;
            //StrSql = StrSql + " ,DayCV03 = ISNULL(B.A3,0) " ;
    
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, " ;

            StrSql = StrSql + " (Select Sum(DayAmount-DayReAmount) AS A1 , Sum(DayTotalPV-DayReTotalPV) AS A2  ,Sum(DayTotalCV-DayReTotalCV) AS A3,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_01_Sell " ;
            StrSql = StrSql + " Where SellCode ='03'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;

            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 



      //   StrSql = " Delete From tbl_ClosePay_01 " ;

      //   Temp_Connect.Insert_Data(StrSql, Conn, tran);



      //   StrSql = " Insert into tbl_ClosePay_01 (" ;
      //   StrSql = StrSql + " [mbid]      ,[mbid2]      ,[M_Name]      ,[DayPrice01]      ,[DayPrice02]      ,[DayPrice03]      ,[DayPrice04]      ,[DayPrice05] ";
      //StrSql = StrSql + " ,[DayPV01]      ,[DayPV02]      ,[DayPV03]      ,[DayPV04]      ,[DayPV05]      ,[DayCV01]      ,[DayCV02]      ,[DayCV03]      ,[DaySham01] ";
      //StrSql = StrSql + " ,[SellPrice01]      ,[SellPrice02]      ,[SellPrice03]      ,[SellPrice04]      ,[SellPrice05]      ,[SellPV01]      ,[SellPV02]      ,[SellPV03] ";
      //StrSql = StrSql + " ,[SellPV04]      ,[SellPV05]      ,[SellCV01]      ,[SellCV02]      ,[SellCV03]      ,[SellSham01]      ,[RecordMakeDate]      ,[BankCode]      ,[BankAcc] ";
      //StrSql = StrSql + " ,[Cpno]      ,[BankOwner]      ,[LeaveDate]      ,[StopDate]      ,[RegTime]      ,[BusCode]      ,[Sell_Mem_TF]      ,[RBO_Mem_TF] ";
      //StrSql = StrSql + " ,[ReqDate1]      ,[ReqDate2]      ,[ReqTF1]      ,[ReqTF2]      ,[ReqTF3]      ,[Saveid]      ,[Saveid2]      ,[Nominid]      ,[Nominid2]      ,[LineCnt] ";
      //StrSql = StrSql + " ,[N_LineCnt]      ,[LevelCnt]      ,[N_LevelCnt]      ,[CurPoint]      ,[BePoint]      ,[ShamPoint]      ,[Be_PV_1]      ,[Be_PV_2]      ,[Cur_PV_1]      ,[Cur_PV_2] ";
      //StrSql = StrSql + " ,[Sum_PV_1]      ,[Sum_PV_2]      ,[Ded_1]      ,[Ded_2]      ,[Fresh_1]      ,[Fresh_2]      ,[Sham_PV_1]      ,[Sham_PV_2]      ,[Re_Cur_PV_1]      ,[Re_Cur_PV_2] ";
      //StrSql = StrSql + " ,[P_Date_10]      ,[P_Date_20]      ,[P_Date_30]      ,[P_Date_40]      ,[CurGrade]      ,[OrgGrade]      ,[BeforeGrade]      ,[ShamGrade]      ,[OneGrade] ";
      //StrSql = StrSql + " ,[G_Sum_PV_1]      ,[G_Sum_PV_2]      ,[GradeCnt1_1]      ,[GradeCnt1_2]      ,[GradeCnt2_1]      ,[GradeCnt2_2]      ,[GradeCnt3_1]      ,[GradeCnt3_2] ";
      //StrSql = StrSql + " ,[GradeCnt4_1]      ,[GradeCnt4_2]      ,[GradeCnt5_1]      ,[GradeCnt5_2]      ,[GradeCnt6_1]      ,[GradeCnt6_2]      ,[GradeDate05]      ,[GradeDate1]      ,[GradeDate2]      ,[GradeDate3]      ,[GradeDate4]      ,[GradeDate5]      ,[GradeDate6]      ,[GradeDate7]      ,[Be_Month_PV]      ,[Sham_GradeDate20]      ,[Sham_GradeDate30] ";
      //StrSql = StrSql + " ,[Sham_GradeDate40]      ,[Sham_GradeDate50]      ,[Frist_OrderNumber_01]      ,[Sum_Return_Take_Pay]      ,[Sum_Return_DedCut_Pay]      ,[Sum_Return_Remain_Pay]      ,[Cur_DedCut_Pay]      ,[Etc_Pay]      ,[Max_Pay]      ,[Allowance6_Cut]      ,[Allowance1_M]      ,[Allowance1]      ,[Allowance2] ";
      //StrSql = StrSql + " ,[Allowance3]      ,[Allowance4]      ,[Allowance5]      ,[Allowance6]      ,[Allowance7]      ,[Allowance8]      ,[Allowance9]      ,[Allowance10]      ,[Allowance11]      ,[Allowance12]      ,[SumAllAllowance]      ,[InComeTax]      ,[ResidentTax]      ,[TruePayment] ";

      //StrSql = StrSql + ") Select   ";

      //StrSql = StrSql + " [mbid]      ,[mbid2]      ,[M_Name]      ,[DayPrice01]      ,[DayPrice02]      ,[DayPrice03]      ,[DayPrice04]      ,[DayPrice05] ";
      //StrSql = StrSql + " ,[DayPV01]      ,[DayPV02]      ,[DayPV03]      ,[DayPV04]      ,[DayPV05]      ,[DayCV01]      ,[DayCV02]      ,[DayCV03]      ,[DaySham01] ";
      //StrSql = StrSql + " ,[SellPrice01]      ,[SellPrice02]      ,[SellPrice03]      ,[SellPrice04]      ,[SellPrice05]      ,[SellPV01]      ,[SellPV02]      ,[SellPV03] ";
      //StrSql = StrSql + " ,[SellPV04]      ,[SellPV05]      ,[SellCV01]      ,[SellCV02]      ,[SellCV03]      ,[SellSham01]      ,[RecordMakeDate]      ,[BankCode]      ,[BankAcc] ";
      //StrSql = StrSql + " ,[Cpno]      ,[BankOwner]      ,[LeaveDate]      ,[StopDate]      ,[RegTime]      ,[BusCode]      ,[Sell_Mem_TF]      ,[RBO_Mem_TF] ";
      //StrSql = StrSql + " ,[ReqDate1]      ,[ReqDate2]      ,[ReqTF1]      ,[ReqTF2]      ,[ReqTF3]      ,[Saveid]      ,[Saveid2]      ,[Nominid]      ,[Nominid2]      ,[LineCnt] ";
      //StrSql = StrSql + " ,[N_LineCnt]      ,[LevelCnt]      ,[N_LevelCnt]      ,[CurPoint]      ,[BePoint]      ,[ShamPoint]      ,[Be_PV_1]      ,[Be_PV_2]      ,[Cur_PV_1]      ,[Cur_PV_2] ";
      //StrSql = StrSql + " ,[Sum_PV_1]      ,[Sum_PV_2]      ,[Ded_1]      ,[Ded_2]      ,[Fresh_1]      ,[Fresh_2]      ,[Sham_PV_1]      ,[Sham_PV_2]      ,[Re_Cur_PV_1]      ,[Re_Cur_PV_2] ";
      //StrSql = StrSql + " ,[P_Date_10]      ,[P_Date_20]      ,[P_Date_30]      ,[P_Date_40]      ,[CurGrade]      ,[OrgGrade]      ,[BeforeGrade]      ,[ShamGrade]      ,[OneGrade] ";
      //StrSql = StrSql + " ,[G_Sum_PV_1]      ,[G_Sum_PV_2]      ,[GradeCnt1_1]      ,[GradeCnt1_2]      ,[GradeCnt2_1]      ,[GradeCnt2_2]      ,[GradeCnt3_1]      ,[GradeCnt3_2] ";
      //StrSql = StrSql + " ,[GradeCnt4_1]      ,[GradeCnt4_2]      ,[GradeCnt5_1]      ,[GradeCnt5_2]      ,[GradeCnt6_1]      ,[GradeCnt6_2]      ,[GradeDate05]      ,[GradeDate1]      ,[GradeDate2]      ,[GradeDate3]      ,[GradeDate4]      ,[GradeDate5]      ,[GradeDate6]      ,[GradeDate7]      ,[Be_Month_PV]      ,[Sham_GradeDate20]      ,[Sham_GradeDate30] ";
      //StrSql = StrSql + " ,[Sham_GradeDate40]      ,[Sham_GradeDate50]      ,[Frist_OrderNumber_01]      ,[Sum_Return_Take_Pay]      ,[Sum_Return_DedCut_Pay]      ,[Sum_Return_Remain_Pay]      ,[Cur_DedCut_Pay]      ,[Etc_Pay]      ,[Max_Pay]      ,[Allowance6_Cut]      ,[Allowance1_M]      ,[Allowance1]      ,[Allowance2] ";
      //StrSql = StrSql + " ,[Allowance3]      ,[Allowance4]      ,[Allowance5]      ,[Allowance6]      ,0      ,[Allowance8]      ,[Allowance9]      ,[Allowance10]      ,[Allowance11]      ,[Allowance12]      ,[SumAllAllowance]      ,[InComeTax]      ,[ResidentTax]      ,[TruePayment] ";

      //StrSql = StrSql + " From tbl_ClosePay_01_Mod_0703 (nolock) Where ToEndDate = '" + ToEndDate + "' ";

      //Temp_Connect.Insert_Data(StrSql, Conn, tran);
     }




        private void ReqTF1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            pg1.Value = 0; pg1.Maximum = 3    ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";     
    
            StrSql = " Update tbl_ClosePay_01 SET";
            StrSql = StrSql + " ReqTF1 = 1 ";
            StrSql = StrSql + " Where RBO_Mem_TF = 0 "; 
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " ReqDate1='" + ToEndDate + "'";
            StrSql = StrSql + " Where ReqDate1=''";
            StrSql = StrSql + " And ReqTF1 >= 1 ";
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        //'''''-------------------------------------------------//////////////////////////////
        }


        private void CurPoint_Put(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            pg1.Value = 0; pg1.Maximum = 10;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_01 set";
            StrSql = StrSql + " CurPoint  = BePoint";            
            StrSql = StrSql + " Where  BePoint > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //P_Date_10
            StrSql = " Update tbl_ClosePay_01 SET";
            StrSql = StrSql + " CurPoint = 10 ";
            StrSql = StrSql + " Where SellPV01 + SellPV02 + SellPv03 >= 300000 ";
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";
            StrSql = StrSql + " And CurPoint <10 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " P_Date_10  ='" + ToEndDate + "'";
            StrSql = StrSql + " Where P_Date_10 =''";
            StrSql = StrSql + " And   CurPoint = 10  ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = " Update tbl_ClosePay_01 SET";
            StrSql = StrSql + " CurPoint = 20 ";
            StrSql = StrSql + " Where SellPV01 + SellPV02 + SellPv03 >= 600000 ";
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";
            StrSql = StrSql + " And CurPoint < 20 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " P_Date_20  ='" + ToEndDate + "'";
            StrSql = StrSql + " Where P_Date_20 =''";
            StrSql = StrSql + " And   CurPoint = 20  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = " Update tbl_ClosePay_01 SET";
            StrSql = StrSql + " CurPoint = 30 ";
            StrSql = StrSql + " Where SellPV01 + SellPV02 + SellPv03 >= 1200000 ";
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";
            StrSql = StrSql + " And CurPoint < 30 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " P_Date_30  ='" + ToEndDate + "'";
            StrSql = StrSql + " Where P_Date_30 =''";
            StrSql = StrSql + " And   CurPoint = 30  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = " Update tbl_ClosePay_01 SET";
            StrSql = StrSql + " CurPoint = 40 ";
            StrSql = StrSql + " Where SellPV01 + SellPV02 + SellPv03 >= 2400000 ";
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";
            StrSql = StrSql + " And CurPoint < 40 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " P_Date_40 = '" + ToEndDate + "'";
            StrSql = StrSql + " Where P_Date_40 =''";
            StrSql = StrSql + " And   CurPoint = 40  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

          

        }





        private void CurPoint_Put_2017_0731(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            pg1.Value = 0; pg1.Maximum = 10;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_01 set";
            StrSql = StrSql + " CurPoint  = BePoint";
            StrSql = StrSql + " Where  BePoint > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //P_Date_10
            StrSql = " Update tbl_ClosePay_01 SET";
            StrSql = StrSql + " CurPoint = 10 ";
            StrSql = StrSql + " Where SellPV01 + SellPV02 + SellPv03 >= " +  double .Parse (txtB20.Text .Trim ());
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";
            StrSql = StrSql + " And CurPoint <10 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " P_Date_10  ='" + ToEndDate + "'";
            StrSql = StrSql + " Where P_Date_10 =''";
            StrSql = StrSql + " And   CurPoint = 10  ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = " Update tbl_ClosePay_01 SET";
            StrSql = StrSql + " CurPoint = 20 ";
            StrSql = StrSql + " Where SellPV01 + SellPV02 + SellPv03 >= " + double.Parse(txtB21.Text.Trim());
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";
            StrSql = StrSql + " And CurPoint < 20 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " P_Date_20  ='" + ToEndDate + "'";
            StrSql = StrSql + " Where P_Date_20 =''";
            StrSql = StrSql + " And   CurPoint = 20  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = " Update tbl_ClosePay_01 SET";
            StrSql = StrSql + " CurPoint = 30 ";
            StrSql = StrSql + " Where SellPV01 + SellPV02 + SellPv03 >= " + double.Parse(txtB22.Text.Trim());
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";
            StrSql = StrSql + " And CurPoint < 30 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " P_Date_30  ='" + ToEndDate + "'";
            StrSql = StrSql + " Where P_Date_30 =''";
            StrSql = StrSql + " And   CurPoint = 30  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = " Update tbl_ClosePay_01 SET";
            StrSql = StrSql + " CurPoint = 40 ";
            StrSql = StrSql + " Where SellPV01 + SellPV02 + SellPv03 >= " + double.Parse(txtB23.Text.Trim());
            //StrSql = StrSql + " And ReqTF1 = 1 "; // 비긴즈는 총판이 될수 있다고함 총판만 ㅠㅠ
            StrSql = StrSql + " And LeaveDate = '' ";
            StrSql = StrSql + " And CurPoint < 40 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " P_Date_40 = '" + ToEndDate + "'";
            StrSql = StrSql + " Where P_Date_40 =''";
            StrSql = StrSql + " And   CurPoint = 40  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



        }






        private void Put_Down_SumPV(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string StrSql = "";




            pg1.Value = 0; pg1.Maximum = MaxLevel + 2;
            pg1.PerformStep(); pg1.Refresh();

            int Cnt = MaxLevel;

            while (Cnt >= 0)
            {

                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " G_Sum_PV_1 =  ISNULL(B.A2,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_01  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + "Select    Sum(G_Sum_PV_1 + G_Sum_PV_2 +  SellPV01 + SellPV02 + SellPV03  ) A2 ";
                StrSql = StrSql + " ,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_01 ";
                StrSql = StrSql + " Where (  G_Sum_PV_1 + G_Sum_PV_2 +  SellPV01 + SellPV02 + SellPV03 ) <>0   ";
                StrSql = StrSql + " And   LevelCnt =" + Cnt;
                StrSql = StrSql + " And   LineCnt =  1 ";
                StrSql = StrSql + " Group By Saveid,Saveid2   ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " G_Sum_PV_2 =  ISNULL(B.A2,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_01  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + "Select    Sum(G_Sum_PV_1 + G_Sum_PV_2 +  SellPV01 + SellPV02 + SellPV03   ) A2 ";
                StrSql = StrSql + " ,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_01 ";
                StrSql = StrSql + " Where (  G_Sum_PV_1 + G_Sum_PV_2 +  SellPV01 + SellPV02 + SellPV03 ) <>0   ";
                StrSql = StrSql + " And   LevelCnt =" + Cnt;
                StrSql = StrSql + " And   LineCnt =  2 ";
                StrSql = StrSql + " Group By Saveid,Saveid2   ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();

                Cnt = Cnt - 1;

            }










        }


        private void CurGrade_OrgGrade_Put(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            if (FromEndDate == Base_Chang_Date___1)
            {
                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + "  BeforeGrade = ISNULL(B.BeforeGrade,0) ";
                StrSql = StrSql + " ,GradeDate1 = ISNULL(B.GradeDate1,'') ";
                StrSql = StrSql + " ,GradeDate2 = ISNULL(B.GradeDate2,'') ";
                StrSql = StrSql + " ,GradeDate3 = ISNULL(B.GradeDate3,'') ";
                StrSql = StrSql + " ,GradeDate4 = ISNULL(B.GradeDate4,'') ";
                StrSql = StrSql + " ,GradeDate5 = ISNULL(B.GradeDate5,'') ";
                StrSql = StrSql + " ,GradeDate6 = ISNULL(B.GradeDate6,'') ";
                StrSql = StrSql + " ,GradeDate7 = ISNULL(B.GradeDate7,'') ";
                StrSql = StrSql + " ,Sham_GradeDate20 = ISNULL(B.Sham_GradeDate20,'') ";
                StrSql = StrSql + " ,Sham_GradeDate30 = ISNULL(B.Sham_GradeDate30,'') ";
                StrSql = StrSql + " ,Sham_GradeDate40 = ISNULL(B.Sham_GradeDate40,'') ";
                StrSql = StrSql + " ,Sham_GradeDate50 = ISNULL(B.Sham_GradeDate50,'') ";

                StrSql = StrSql + " FROM  tbl_ClosePay_01  A, ";

                StrSql = StrSql + " (Select BeforeGrade, GradeDate1, GradeDate2, GradeDate3, GradeDate4, GradeDate5, GradeDate6, GradeDate7 ";
                StrSql = StrSql + " ,Sham_GradeDate20,Sham_GradeDate30 , Sham_GradeDate40, Sham_GradeDate50";
                StrSql = StrSql + " ,Mbid,Mbid2 ";
                StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            StrSql = "Update tbl_ClosePay_01 set";
            StrSql = StrSql + " OrgGrade  = BeforeGrade";
            StrSql = StrSql + " ,CurGrade = BeforeGrade";
            StrSql = StrSql + " Where  BeforeGrade > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            
        }


        private void GiveShamGrade(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string StrSql = "Exec Usp_Sham_Grade '" + ToEndDate + "'   ";
            string Mbid = "", TMaxDate = "", TFild = ""; int Mbid2 = 0, S_Grade = 0;
            int Cnt = 10;
            ReCnt = 0;
            DataSet Dset4 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
            ReCnt = Search_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {
                pg1.Value = 0; pg1.Maximum = ReCnt + 1;
                pg1.PerformStep(); pg1.Refresh();

                pg1.Value = 0; pg1.Maximum = ReCnt;

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Mbid = Dset4.Tables[base_db_name].Rows[fi_cnt]["TMbid"].ToString();
                    Mbid2 = int.Parse(Dset4.Tables[base_db_name].Rows[fi_cnt]["tMbid2"].ToString());
                    S_Grade = int.Parse(Dset4.Tables[base_db_name].Rows[fi_cnt]["S_Grade"].ToString());
                    TMaxDate = Dset4.Tables[base_db_name].Rows[fi_cnt]["TMaxDate"].ToString();

                    if (Mbid2 == 701)
                        Mbid2 = Mbid2; 

                    //if (S_Grade == 0)
                    //{
                    //    StrSql = "Update tbl_ClosePay_01 SET ";
                    //    StrSql = StrSql + " CurGrade =  0  ";
                    //    StrSql = StrSql + " ,OrgGrade =    " + S_Grade;
                    //    StrSql = StrSql + " ,ShamGrade =    " + S_Grade;
                    //    StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
                    //    StrSql = StrSql + " And   Mbid2 = " + Mbid2;
                    //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    //}
                    //else
                    //{
                    StrSql = "Update tbl_ClosePay_01 SET ";
                    StrSql = StrSql + " CurGrade =    " + S_Grade;
                    StrSql = StrSql + " ,OrgGrade =    " + S_Grade;
                    StrSql = StrSql + " ,ShamGrade =    " + S_Grade;
                    StrSql = StrSql + " ,ReqTF1 = 1   ";
                    StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
                    StrSql = StrSql + " And   Mbid2 = " + Mbid2;
                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    //}
                    Cnt = 10;

                    while (Cnt <= S_Grade)
                    {
                        TFild = "GradeDate" + (Cnt / 10).ToString();

                        StrSql = "Update tbl_ClosePay_01 Set ";
                        StrSql = StrSql + TFild + " = '" + ToEndDate + "'";
                        StrSql = StrSql + " Where Mbid='" + Mbid + "'";
                        StrSql = StrSql + " And Mbid2=" + Mbid2;
                        StrSql = StrSql + " And " + TFild + " = ''";

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);

                        Cnt = Cnt + 10;
                    }

                    pg1.PerformStep(); pg1.Refresh();
                } // for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)

            } // (ReCnt != 0)
        }




        private void GiveShamGrade_P(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string StrSql = "Exec Usp_Sham_Grade_P '" + ToEndDate + "'   ";
            string Mbid = "", TMaxDate = "", TFild = ""; int Mbid2 = 0, S_Grade = 0;
            int Cnt = 10;
            ReCnt = 0;
            DataSet Dset4 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
            ReCnt = Search_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {
                pg1.Value = 0; pg1.Maximum = ReCnt + 1;
                pg1.PerformStep(); pg1.Refresh();

                pg1.Value = 0; pg1.Maximum = ReCnt;

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Mbid = Dset4.Tables[base_db_name].Rows[fi_cnt]["TMbid"].ToString();
                    Mbid2 = int.Parse(Dset4.Tables[base_db_name].Rows[fi_cnt]["tMbid2"].ToString());
                    S_Grade = int.Parse(Dset4.Tables[base_db_name].Rows[fi_cnt]["S_Grade"].ToString());
                    TMaxDate = Dset4.Tables[base_db_name].Rows[fi_cnt]["TMaxDate"].ToString();

                    //if (S_Grade == 0)
                    //{
                    //    StrSql = "Update tbl_ClosePay_01 SET ";
                    //    StrSql = StrSql + " CurGrade =  0  ";
                    //    StrSql = StrSql + " ,OrgGrade =    " + S_Grade;
                    //    StrSql = StrSql + " ,ShamGrade =    " + S_Grade;
                    //    StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
                    //    StrSql = StrSql + " And   Mbid2 = " + Mbid2;
                    //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    //}
                    //else
                    //{
                    StrSql = "Update tbl_ClosePay_01 SET ";
                    StrSql = StrSql + " CurPoint =    " + S_Grade;
                    StrSql = StrSql + " ,ShamPoint =    " + S_Grade;                    
                    StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
                    StrSql = StrSql + " And   Mbid2 = " + Mbid2;
                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    //}
                    Cnt = 10;

                    while (Cnt <= S_Grade)
                    {
                        TFild = "P_Date_" + Cnt.ToString();

                        StrSql = "Update tbl_ClosePay_01 Set ";
                        StrSql = StrSql + TFild + " = '" + ToEndDate + "'";
                        StrSql = StrSql + " Where Mbid='" + Mbid + "'";
                        StrSql = StrSql + " And Mbid2=" + Mbid2;
                        StrSql = StrSql + " And " + TFild + " = ''";

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);

                        Cnt = Cnt + 10;
                    }

                    pg1.PerformStep(); pg1.Refresh();
                } // for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)

            } // (ReCnt != 0)
        }



        private void GiveGrade05(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 3;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " CurGrade = 5 ";
            StrSql = StrSql + " Where   CurGrade < 5 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (SellPv01 + SellPv02 >= 150000 ) ";
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate05 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 5 ";
            StrSql = StrSql + " And GradeDate05 =''";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void GiveGrade1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 3;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " CurGrade = 10 ";
            StrSql = StrSql + " Where   CurGrade < 10 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   SellPv01 + SellPv02 >=  " + double.Parse(txtB24.Text.Trim());
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";  
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
                        

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade =10 ";
            StrSql = StrSql + " And GradeDate1 =''";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }


        private void GiveGrade2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 6;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            //골드
            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " CurGrade = 20 ";
            StrSql = StrSql + " Where   CurGrade < 20 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            if (int.Parse(FromEndDate) >= 20170731)
                StrSql = StrSql + " And   SellPv01 + SellPv02 >= " + double.Parse(txtB25.Text.Trim());
            else
                StrSql = StrSql + " And   (SellPv01 + SellPv02 >= 10000000 ) ";
            StrSql = StrSql + " And   LeaveDate = '' ";
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";  

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " CurGrade = 20 ";
            StrSql = StrSql + " Where   CurGrade < 20 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            if (int.Parse(FromEndDate) >= 20170731)
            {
                StrSql = StrSql + " And   G_Sum_PV_1 >= " + double.Parse(txtB26.Text.Trim());
                StrSql = StrSql + " And   G_Sum_PV_2 >= " + double.Parse(txtB26.Text.Trim());
            }
            else
            {
                StrSql = StrSql + " And   (G_Sum_PV_1 >= 15000000 ) ";
                StrSql = StrSql + " And   (G_Sum_PV_2 >= 15000000 ) ";
            }
            StrSql = StrSql + " And   LeaveDate = '' ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 20 ";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }



        private void GiveGrade3(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 6;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            //골드
            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " CurGrade = 30 ";
            StrSql = StrSql + " Where   CurGrade < 30 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            if (int.Parse(FromEndDate) >= 20170731)
                StrSql = StrSql + " And   SellPv01 + SellPv02 >= " + double.Parse(txtB27.Text.Trim());
            else
                StrSql = StrSql + " And   (SellPv01 + SellPv02 >= 20000000 ) ";
            StrSql = StrSql + " And   LeaveDate = '' ";
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";  

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " CurGrade = 30 ";
            StrSql = StrSql + " Where   CurGrade < 30 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            if (int.Parse(FromEndDate) >= 20170731)
            {
                StrSql = StrSql + " And   G_Sum_PV_1 >= " + double.Parse(txtB28.Text.Trim());
                StrSql = StrSql + " And   G_Sum_PV_2 >= " + double.Parse(txtB28.Text.Trim());
            }
            else
            {
                StrSql = StrSql + " And   (G_Sum_PV_1 >= 30000000 ) ";
                StrSql = StrSql + " And   (G_Sum_PV_2 >= 30000000 ) ";
            }
            StrSql = StrSql + " And   LeaveDate = '' ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 30 ";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 30 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void GiveGrade4(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 6;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";



            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " CurGrade = 40 ";
            StrSql = StrSql + " Where   CurGrade < 40 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   G_Sum_PV_1 >=  " + double.Parse(txtB29.Text.Trim());
            StrSql = StrSql + " And   G_Sum_PV_2 >=  " + double.Parse(txtB29.Text.Trim());

            StrSql = StrSql + " And   (GradeCnt3_1 >= 1 ) ";
            StrSql = StrSql + " And   (GradeCnt3_1 >= 1 ) ";
            StrSql = StrSql + " And   LeaveDate = '' ";
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";  

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 40 ";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 40 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 40 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }



        private void GiveGrade5(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 6;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";



            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " CurGrade = 50 ";
            StrSql = StrSql + " Where   CurGrade < 50 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   G_Sum_PV_1 >= " +  double.Parse(txtB30.Text.Trim());
            StrSql = StrSql + " And   G_Sum_PV_2 >= " +  double.Parse(txtB30.Text.Trim());

            StrSql = StrSql + " And   (GradeCnt4_1 >= 1 ) ";
            StrSql = StrSql + " And   (GradeCnt4_1 >= 1 ) ";
            StrSql = StrSql + " And   LeaveDate = '' ";
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";  

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }



        private void GiveGrade6(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 7;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";



            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " CurGrade = 60 ";
            StrSql = StrSql + " Where   CurGrade < 60 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   G_Sum_PV_1 >= " + double.Parse(txtB31.Text.Trim());
            StrSql = StrSql + " And   G_Sum_PV_2 >= " + double.Parse(txtB31.Text.Trim());

            StrSql = StrSql + " And   (GradeCnt5_1 >= 1 ) ";
            StrSql = StrSql + " And   (GradeCnt5_1 >= 1 ) ";
            StrSql = StrSql + " And   LeaveDate = '' ";
            StrSql = StrSql + " And ReqTF1 = 1 ";
            StrSql = StrSql + " And LeaveDate = '' ";  

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }




        private void Put_ReqTF2_OneGrade(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 11;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            //Be_Month_PV


            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " ReqTF2 = 1 ";
            StrSql = StrSql + " Where  left(RegTime,6)  ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            string SDate = "";

            DateTime dt = DateTime.Parse(FromEndDate.Substring(0, 4) + "-" + FromEndDate.Substring(4, 2) + "-" + FromEndDate.Substring(6, 2));
            SDate = dt.AddMonths(-1).ToShortDateString().Replace("-", "");

            StrSql = " Update tbl_ClosePay_01 SET";
            StrSql = StrSql + " Be_Month_PV = IsNull(b.A1, 0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select  ";
            StrSql = StrSql + "  Sum(BS1.TotalPrice)    + Isnull(Sum(Bs_R.TotalPrice),0) AS A1 ";
            StrSql = StrSql + ",BS1.Mbid,BS1.Mbid2";
            StrSql = StrSql + " From tbl_SalesDetail AS BS1 (nolock) ";
            StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPrice  + Bs_R.TotalPrice < 0    And  Bs_R.SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " Where   LEFT(BS1.SellDate,6) = '" + SDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And     BS1.TotalPrice  + BS1.TotalPrice >= 0 ";
            StrSql = StrSql + " And     BS1.SellCode = '03' ";
            StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
            StrSql = StrSql + " Group By BS1.Mbid,BS1.Mbid2";
            StrSql = StrSql + " Having Sum(BS1.TotalPV) + IsNull(Sum(Bs_R.TotalPV), 0) >= 0";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " ReqTF2 = 1 ";
            StrSql = StrSql + " Where  left(RegTime,6)  ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " ReqTF2 = 1 ";
            StrSql = StrSql + " Where  GradeDate2 = '" + ToEndDate + "'";
            StrSql = StrSql + " OR     GradeDate3 = '" + ToEndDate + "'";
            StrSql = StrSql + " OR     GradeDate4 = '" + ToEndDate + "'";
            StrSql = StrSql + " OR     GradeDate5 = '" + ToEndDate + "'";
            StrSql = StrSql + " OR     GradeDate6 = '" + ToEndDate + "'";
            //StrSql = StrSql + " OR     GradeDate7 = '" + ToEndDate + "'";
            //StrSql = StrSql + " OR     GradeDate8 = '" + ToEndDate + "'";
            //StrSql = StrSql + " OR     GradeDate9 = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " ReqTF2 = 1 ";
            StrSql = StrSql + " Where  LEFT(GradeDate2,6) = '" + SDate.Substring(0, 6) + "'";
            StrSql = StrSql + " OR     LEFT(GradeDate3,6) = '" + SDate.Substring(0, 6) + "'";
            StrSql = StrSql + " OR     LEFT(GradeDate4,6) = '" + SDate.Substring(0, 6) + "'";
            StrSql = StrSql + " OR     LEFT(GradeDate5,6) = '" + SDate.Substring(0, 6) + "'";
            StrSql = StrSql + " OR     LEFT(GradeDate6,6) = '" + SDate.Substring(0, 6) + "'";
            //StrSql = StrSql + " OR     LEFT(GradeDate7,6) = '" + SDate.Substring(0, 6) + "'";
            //StrSql = StrSql + " OR     LEFT(GradeDate8,6) = '" + SDate.Substring(0, 6) + "'";
            //StrSql = StrSql + " OR     LEFT(GradeDate9,6) = '" + SDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " ReqTF2 = 1";
            StrSql = StrSql + " Where  Be_Month_PV >= 30000 ";
            StrSql = StrSql + " And   ReqTF2 = 0 ";
            StrSql = StrSql + " And   OrgGrade = 10 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " ReqTF2 = 1";
            StrSql = StrSql + " Where  Be_Month_PV >= 50000 ";
            StrSql = StrSql + " And   ReqTF2 = 0 ";
            StrSql = StrSql + " And   OrgGrade = 20 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " ReqTF2 = 1";
            StrSql = StrSql + " Where  Be_Month_PV >= 90000 ";
            StrSql = StrSql + " And   ReqTF2 = 0 ";
            StrSql = StrSql + " And   OrgGrade = 30 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " ReqTF2 = 1";
            StrSql = StrSql + " Where  Be_Month_PV >= 150000 ";
            StrSql = StrSql + " And   ReqTF2 = 0 ";
            StrSql = StrSql + " And   OrgGrade = 40 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " ReqTF2 = 1";
            StrSql = StrSql + " Where  Be_Month_PV >= 200000 ";
            StrSql = StrSql + " And   ReqTF2 = 0 ";
            StrSql = StrSql + " And   OrgGrade = 50 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " ReqTF2 = 1";
            StrSql = StrSql + " Where  Be_Month_PV >= 500000 ";
            StrSql = StrSql + " And   ReqTF2 = 0 ";
            StrSql = StrSql + " And   OrgGrade = 60 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //2017-06월까지는 유지가 없어도 수당이 나가는 걸로 셋팅을함.
            //2017-07-10 이사님 요청에 의해서 말씀이 있을때까지는 우선 풀어 두기로함. 
            //if (int.Parse(FromEndDate.Substring (0, 6)) <= 201706)
            //{
                StrSql = "Update tbl_ClosePay_01 SET  ";
                StrSql = StrSql + " ReqTF2 = 1";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //}
            //2017-07-10 이사님 요청에 의해서 말씀이 있을때까지는 우선 풀어 두기로함. 

        }








        private void Put_ReqTF2_OneGrade_R(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 19;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            string SDate = "";

            DateTime dt = DateTime.Parse(FromEndDate.Substring(0, 4) + "-" + FromEndDate.Substring(4, 2) + "-" + FromEndDate.Substring(6, 2));
            SDate = dt.AddMonths(-1).ToShortDateString().Replace("-", "");                       
            
            StrSql = " Update tbl_ClosePay_01 SET";
            StrSql = StrSql + " Be_Month_PV = IsNull(b.A1, 0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select  ";
            StrSql = StrSql + "  Sum(BS1.TotalPV)    + Isnull(Sum(Bs_R.TotalPV),0) AS A1 "; 
            StrSql = StrSql + ",BS1.Mbid,BS1.Mbid2";
            StrSql = StrSql + " From tbl_SalesDetail AS BS1 (nolock) ";
            StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " Where   LEFT(BS1.SellDate,6) = '" + SDate.Substring (0,6) + "'";            
            StrSql = StrSql + " And     BS1.TotalPV  + BS1.TotalCV >= 0 ";
            StrSql = StrSql + " And     BS1.SellCode <> '' ";
            StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
            StrSql = StrSql + " Group By BS1.Mbid,BS1.Mbid2";
            StrSql = StrSql + " Having Sum(BS1.TotalPV) + IsNull(Sum(Bs_R.TotalPV), 0) >= 0";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " OneGrade = 10 ";
            StrSql = StrSql + " Where OrgGrade >= 10 ";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   Be_Month_PV >= 30000 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " OneGrade = 20 ";
            StrSql = StrSql + " Where OrgGrade >= 20 ";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   Be_Month_PV >= 50000 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " OneGrade = 30 ";
            StrSql = StrSql + " Where OrgGrade >= 30 ";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   Be_Month_PV >= 90000 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " OneGrade = 40 ";
            StrSql = StrSql + " Where OrgGrade >= 40 ";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   Be_Month_PV >= 150000 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " OneGrade = 50 ";
            StrSql = StrSql + " Where OrgGrade >= 50 ";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   Be_Month_PV >= 200000 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " OneGrade = 60 ";
            StrSql = StrSql + " Where OrgGrade >= 60 ";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   Be_Month_PV >= 500000 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            
            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " OneGrade = 10 ";
            StrSql = StrSql + " Where  LEFT(GradeDate1,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 10 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " OneGrade = 20 ";
            StrSql = StrSql + " Where  LEFT(GradeDate2,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 20 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " OneGrade = 30 ";
            StrSql = StrSql + " Where  LEFT(GradeDate3,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 30 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " OneGrade = 40 ";
            StrSql = StrSql + " Where  LEFT(GradeDate4,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 40 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " OneGrade = 50 ";
            StrSql = StrSql + " Where  LEFT(GradeDate5,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 50 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 SET  ";
            StrSql = StrSql + " OneGrade = 60 ";
            StrSql = StrSql + " Where  LEFT(GradeDate6,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 60 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_01 SET  ";
            //StrSql = StrSql + " OneGrade = 70 ";
            //StrSql = StrSql + " Where  LEFT(GradeDate7,6) = '" + ToEndDate.Substring(0, 6) + "'";
            //StrSql = StrSql + " And OneGrade < 70 ";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_01 SET  ";
            //StrSql = StrSql + " OneGrade = 80 ";
            //StrSql = StrSql + " Where  LEFT(GradeDate8,6) = '" + ToEndDate.Substring(0, 6) + "'";
            //StrSql = StrSql + " And OneGrade < 80 ";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            //StrSql = "Update tbl_ClosePay_01 SET  ";
            //StrSql = StrSql + " OneGrade = 90 ";
            //StrSql = StrSql + " Where  LEFT(GradeDate9,6) = '" + ToEndDate.Substring(0, 6) + "'";
            //StrSql = StrSql + " And OneGrade < 90 ";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


        }


        private void GradeUpLine2(int CurrentGrade, cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            int Cnt = 0;
            string StrSql = "", str_GradeCnt = "", str_GradeCnt1 = "", str_GradeCnt2 = "", str_GradeCnt3 = "";

            if (CurrentGrade == 10)
            {
                str_GradeCnt = " GradeCnt1_1 + GradeCnt1_2 ";
                str_GradeCnt1 = " GradeCnt1_1 "; str_GradeCnt2 = " GradeCnt1_2 ";

                StrSql = "Update tbl_ClosePay_01 SET GradeCnt1_1 =0,GradeCnt1_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 20)
            {
                str_GradeCnt = " GradeCnt2_1 + GradeCnt2_2 ";
                str_GradeCnt1 = " GradeCnt2_1 "; str_GradeCnt2 = " GradeCnt2_2 ";

                StrSql = "Update tbl_ClosePay_01 SET GradeCnt2_1 =0 ,  GradeCnt2_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 30)
            {
                str_GradeCnt = " GradeCnt3_1 + GradeCnt3_2 ";
                str_GradeCnt1 = " GradeCnt3_1 "; str_GradeCnt2 = " GradeCnt3_2 ";

                StrSql = "Update tbl_ClosePay_01 SET GradeCnt3_1 =0 ,  GradeCnt3_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 40)
            {
                str_GradeCnt = " GradeCnt4_1 + GradeCnt4_2 ";
                str_GradeCnt1 = " GradeCnt4_1 "; str_GradeCnt2 = " GradeCnt4_2 ";

                StrSql = "Update tbl_ClosePay_01 SET GradeCnt4_1 =0 ,  GradeCnt4_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 50)
            {
                str_GradeCnt = " GradeCnt5_1 + GradeCnt5_2 ";
                str_GradeCnt1 = " GradeCnt5_1 "; str_GradeCnt2 = " GradeCnt5_2 ";

                StrSql = "Update tbl_ClosePay_01 SET GradeCnt5_1 =0 ,  GradeCnt5_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 60)
            {
                str_GradeCnt = " GradeCnt6_1 + GradeCnt6_2 ";
                str_GradeCnt1 = " GradeCnt6_1 "; str_GradeCnt2 = " GradeCnt6_2 ";

                StrSql = "Update tbl_ClosePay_01 SET GradeCnt6_1 =0 ,  GradeCnt6_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 70)
            {
                str_GradeCnt = " GradeCnt7_1 + GradeCnt7_2 ";
                str_GradeCnt1 = " GradeCnt7_1 "; str_GradeCnt2 = " GradeCnt7_2 ";

                StrSql = "Update tbl_ClosePay_01 SET GradeCnt7_1 =0 ,  GradeCnt7_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 80)
            {
                str_GradeCnt = " GradeCnt8_1 + GradeCnt8_2 ";
                str_GradeCnt1 = " GradeCnt8_1 "; str_GradeCnt2 = " GradeCnt8_2 ";

                StrSql = "Update tbl_ClosePay_01 SET GradeCnt8_1 =0 ,  GradeCnt8_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 90)
            {
                str_GradeCnt = " GradeCnt9_1 + GradeCnt9_2 ";
                str_GradeCnt1 = " GradeCnt9_1 "; str_GradeCnt2 = " GradeCnt9_2 ";

                StrSql = "Update tbl_ClosePay_01 SET GradeCnt9_1 =0 ,  GradeCnt9_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            pg1.Value = 0; pg1.Maximum = Cnt + 4;
            pg1.PerformStep(); pg1.Refresh();

            Cnt = MaxLevel;

            while (Cnt >= 1)
            {
                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + str_GradeCnt1 + "=ISNULL(B.A1,0) ";
                StrSql = StrSql + " FROM  tbl_ClosePay_01  A, ";

                StrSql = StrSql + " (Select    Sum(" + str_GradeCnt + ") A1,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_01 ";
                StrSql = StrSql + " Where " + str_GradeCnt + " > 0  ";
                StrSql = StrSql + " And LineCnt = 1 ";
                StrSql = StrSql + " And LevelCnt =" + Cnt;
                StrSql = StrSql + " Group By Saveid,Saveid2  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + str_GradeCnt1 + " =" + str_GradeCnt1 + " + ISNULL(B.A1,0)  ";
                StrSql = StrSql + " FROM  tbl_ClosePay_01  A, ";

                StrSql = StrSql + " (Select Count(Mbid) A1,   Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_01 ";
                if (CurrentGrade == 50)
                    StrSql = StrSql + " Where CurGrade >= " + CurrentGrade;
                else
                    StrSql = StrSql + " Where CurGrade = " + CurrentGrade;

                StrSql = StrSql + " And LineCnt = 1 ";
                StrSql = StrSql + " And LevelCnt =" + Cnt;
                StrSql = StrSql + " Group By Saveid,Saveid2  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
                //'''---------------------------------------------------------------



                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + str_GradeCnt2 + "=ISNULL(B.A1,0) ";
                StrSql = StrSql + " FROM  tbl_ClosePay_01  A, ";

                StrSql = StrSql + " (Select    Sum(" + str_GradeCnt + ") A1,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_01 ";
                StrSql = StrSql + " Where " + str_GradeCnt + "> 0  ";
                StrSql = StrSql + " And LineCnt >= 2 ";
                StrSql = StrSql + " And LevelCnt =" + Cnt;
                StrSql = StrSql + " Group By Saveid,Saveid2  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();


                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + str_GradeCnt2 + " =" + str_GradeCnt2 + " + + ISNULL(B.A1,0)  ";
                StrSql = StrSql + " FROM  tbl_ClosePay_01  A, ";

                StrSql = StrSql + " (Select Count(Mbid) A1,   Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_01 ";

                if (CurrentGrade == 50)
                    StrSql = StrSql + " Where CurGrade >= " + CurrentGrade;
                else
                    StrSql = StrSql + " Where CurGrade = " + CurrentGrade;


                StrSql = StrSql + " And LineCnt >= 2 ";
                StrSql = StrSql + " And LevelCnt =" + Cnt;
                StrSql = StrSql + " Group By Saveid,Saveid2  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
                // '''---------------------------------------------------------------

                Cnt = Cnt - 1;
            }


        }


        private void CurPoint_Put_2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string S_ToEndDate)
        {

            pg1.Value = 0; pg1.Maximum = 7    ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";   
         
            StrSql = " Update tbl_ClosePay_01 SET"    ;
            StrSql = StrSql + " CurPoint_Date_2_Gap = 0 "   ;
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    

            StrSql = "Update tbl_ClosePay_01 SET "   ;
            StrSql = StrSql + " CurPoint_SellPV = ISNULL(B.A1, 0 )   "   ;
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, "   ;
    
            StrSql = StrSql + " (Select Sum(TotalPV) A1,  Mbid ,Mbid2   "   ;
            StrSql = StrSql + " From tbl_SalesDetail (nolock)"   ;
            StrSql = StrSql + " Where   SellDate <='" + S_ToEndDate + "'"   ;
            StrSql = StrSql + " And     SellCode = '01' "   ;
            StrSql = StrSql + " Group By Mbid,Mbid2"   ;
            StrSql = StrSql + " ) B"   ;
    
            StrSql = StrSql + " Where A.Mbid  = B.Mbid "   ;
            StrSql = StrSql + " And   A.Mbid2  = B.Mbid2 "   ;
      
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();
    
    
            StrSql = " Update tbl_ClosePay_01 SET"   ;
            StrSql = StrSql + " CurPoint = 2 "   ;
            StrSql = StrSql + " Where CurPoint_SellPV >= 250000 "   ;
            StrSql = StrSql + " And Sell_Mem_TF = 0 "   ;
            
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();
        
       
            StrSql = "Update tbl_ClosePay_01 Set "   ;
            StrSql = StrSql + " CurPoint_Date_2='" + S_ToEndDate + "'"   ;
            StrSql = StrSql + " Where CurPoint_Date_2=''"   ;
            StrSql = StrSql + " And CurPoint = 2 "   ;
        
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();
    
            StrSql = "Update tbl_ClosePay_01 Set "   ;
            StrSql = StrSql + " CurPoint_Date_2_Gap =  DateDiff(D, Regtime, CurPoint_Date_2) "   ;
            StrSql = StrSql + " Where CurPoint_Date_2 ='" + S_ToEndDate + "'"   ;
        
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();
    
    
            StrSql = "Update tbl_ClosePay_01 Set "   ;
            StrSql = StrSql + " CurPoint =  0 "   ;
            StrSql = StrSql + " ,CurPoint_Date_2 = '' "   ;
            StrSql = StrSql + " Where CurPoint_Date_2 ='" + S_ToEndDate + "'"   ;
            StrSql = StrSql + " And   CurPoint_Date_2_Gap > 30 ";
        
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();
    
    }



       private void CurPoint_Put_3( cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran ,string S_ToEndDate)
       {

           pg1.Value = 0; pg1.Maximum = 6   ;
           pg1.PerformStep(); pg1.Refresh();
           string StrSql = "";


           StrSql = " Update tbl_ClosePay_01 SET"  ;
            StrSql = StrSql + " CurPoint_Date_3_Gap = 0 "  ;
            
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
                   pg1.PerformStep(); pg1.Refresh();
 
            StrSql = " Update tbl_ClosePay_01 SET"  ;
            StrSql = StrSql + " CurPoint = 3 "  ;
            StrSql = StrSql + " Where CurPoint_SellPV >= 750000 "  ;
            StrSql = StrSql + " And CurPoint_Date_2 <> '' "  ;
            StrSql = StrSql + " And Sell_Mem_TF = 0 "  ;
            
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
                   pg1.PerformStep(); pg1.Refresh();     
       
            StrSql = "Update tbl_ClosePay_01 Set "  ;
            StrSql = StrSql + " CurPoint_Date_3='" + S_ToEndDate + "'"  ;
            StrSql = StrSql + " Where CurPoint_Date_3=''"  ;
            StrSql = StrSql + " And CurPoint = 3 "  ;
        
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
                   pg1.PerformStep(); pg1.Refresh(); 
    
             StrSql = "Update tbl_ClosePay_01 Set "  ;
             StrSql = StrSql + " CurPoint_Date_3_Gap =  DateDiff(D, CurPoint_Date_2, CurPoint_Date_3) ";
            StrSql = StrSql + " Where CurPoint_Date_3 ='" + S_ToEndDate + "'"  ;
        
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
                   pg1.PerformStep(); pg1.Refresh();
    
            StrSql = "Update tbl_ClosePay_01 Set "  ;
            StrSql = StrSql + " CurPoint =  0 "  ;
            StrSql = StrSql + " ,CurPoint_Date_3 = '' "  ;
            StrSql = StrSql + " Where CurPoint_Date_3 ='" + S_ToEndDate + "'"  ;
            StrSql = StrSql + " And   CurPoint_Date_3_Gap > 45 "  ;
        
   
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
           pg1.PerformStep(); pg1.Refresh();

       }



        private void Put_OrgGrade( cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran )
       {
            pg1.Value = 0; pg1.Maximum = 7  ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
            string SDate = "";

            StrSql = "Select Isnull(Max(ToEndDate), '')  From tbl_CloseTotal_04 (nolock) ";   //'''--직급마감에서 전달 마감일자를 알아온다.
            StrSql = StrSql  + " Where LEFT(ToEndDate,6) < '" + FromEndDate.Substring(0,6) + "'"  ;    // '''--전달마감을 알아온다.

            ReCnt = 0;
            DataSet Dset = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset);
            ReCnt = Search_Connect.DataSet_ReCount;
            //Dset.Tables[base_db_name].Rows[0][0].ToString();

            if (ReCnt >0)
            {
                SDate = Dset.Tables[base_db_name].Rows[0][0].ToString();
            }
            pg1.PerformStep(); pg1.Refresh();
            
    
            if (SDate == "") return ; 

    
            StrSql = "Update tbl_ClosePay_01 SET "  ;
            StrSql = StrSql  + "  CurGrade =ISNULL(B.A1,0) "   ;
            StrSql = StrSql + " , ReqTF2 =ISNULL(B.ReqTF2,0) ";
            StrSql = StrSql  + " FROM  tbl_ClosePay_01  A, "   ;

            StrSql = StrSql + " (Select  CurGrade As A1 , ReqTF2 , Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_ClosePay_04_Mod  (nolock) ";
            StrSql = StrSql  + " Where ToEndDate = '" + SDate +  "'"   ;
            StrSql = StrSql  + " ) B"   ;
    
            StrSql = StrSql  + " Where A.Mbid=B.Mbid "   ;
            StrSql = StrSql  + " And   A.Mbid2=B.Mbid2 "   ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + "  ReqTF2 = 1 ";
            StrSql = StrSql + " Where Where ReqTF2 = 0 ";
            StrSql = StrSql + " And  (LEFT (RegTime,6) ='" + ToEndDate.Substring (0, 6) + "'" ;
            StrSql = StrSql + " OR   LEFT (RegTime,6) ='" + FromEndDate.Substring(0, 6) + "')";
    

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

       }

        private void Put_Self_PV( cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran )
       {
            pg1.Value = 0; pg1.Maximum = 7  ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
            string SDate = "";

            DateTime dt = DateTime.Parse(ToEndDate.Substring(0, 4) + "-" + ToEndDate.Substring(4, 2) + "-" + ToEndDate.Substring(6, 2) );
            SDate = dt.AddMonths(-3).ToShortDateString().Replace ("-","");
                       

    
            StrSql = "Update tbl_ClosePay_01 SET "  ;
            StrSql = StrSql  + "  CurGrade =ISNULL(B.A1,0) "   ;
            StrSql = StrSql + " , ReqTF2 =ISNULL(B.ReqTF2,0) ";
            StrSql = StrSql  + " FROM  tbl_ClosePay_01  A, "   ;

            StrSql = StrSql + " (Select  CurGrade As A1 , ReqTF2 , Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_ClosePay_04_Mod  (nolock) ";
            StrSql = StrSql  + " Where ToEndDate = '" + SDate +  "'"   ;
            StrSql = StrSql  + " ) B"   ;
    
            StrSql = StrSql  + " Where A.Mbid=B.Mbid "   ;
            StrSql = StrSql  + " And   A.Mbid2=B.Mbid2 "   ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + "  ReqTF2 = 1 ";
            StrSql = StrSql + " Where Where ReqTF2 = 0 ";
            StrSql = StrSql + " And  (LEFT (RegTime,6) ='" + ToEndDate.Substring (0, 6) + "'" ;
            StrSql = StrSql + " OR   LEFT (RegTime,6) ='" + FromEndDate.Substring(0, 6) + "')";
    

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

       } 

        private void Put_Cut_PV_4_1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4  ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            string M_Date = "", M_Date2 = "", M_Date3 = "";
            string F_Date = "", F_Date2 = "", F_Date3 = "";
            int M_Cnt = 0 ;


            StrSql = "Select Isnull(Max(ToEndDate),'') , Isnull(Max(FromEndDate),'') From tbl_CloseTotal_01 (nolock) ";

            ReCnt = 0;
            DataSet Dset = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset);
            ReCnt = Search_Connect.DataSet_ReCount;
            //Dset.Tables[base_db_name].Rows[0][0].ToString();

            if (ReCnt > 0 )
            {
                M_Date = Dset.Tables[base_db_name].Rows[0][0].ToString();
                F_Date = Dset.Tables[base_db_name].Rows[0][1].ToString();
                M_Cnt++;
            }
            pg1.PerformStep(); pg1.Refresh();
            

            if (M_Date != "")
            {
                StrSql = "Select Isnull(Max(ToEndDate),'') , Isnull(Max(FromEndDate),'') From tbl_CloseTotal_01  (nolock) ";
                StrSql = StrSql + " Where ToEndDate < '" + M_Date + "'";

                ReCnt = 0;
                DataSet Dset2 = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset2);
                ReCnt = Search_Connect.DataSet_ReCount;
                //Dset.Tables[base_db_name].Rows[0][0].ToString();

                if (ReCnt > 0)
                {
                    M_Date2 = Dset2.Tables[base_db_name].Rows[0][0].ToString();
                    F_Date2 = Dset2.Tables[base_db_name].Rows[0][1].ToString();
                    M_Cnt++;
                }

            }
            pg1.PerformStep(); pg1.Refresh();


            if (M_Date2 != "")
            {
                StrSql = "Select Isnull(Max(ToEndDate),'') , Isnull(Max(FromEndDate),'') From tbl_CloseTotal_01  (nolock)  ";
                StrSql = StrSql + " Where ToEndDate < '" + M_Date2 + "'";

                ReCnt = 0;
                DataSet Dset3 = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset3);
                ReCnt = Search_Connect.DataSet_ReCount;
                //Dset.Tables[base_db_name].Rows[0][0].ToString();

                if (ReCnt > 0)
                {
                    M_Date3 = Dset3.Tables[base_db_name].Rows[0][0].ToString();
                    F_Date3 = Dset3.Tables[base_db_name].Rows[0][1].ToString();
                    M_Cnt++;
                }
            }
            pg1.PerformStep(); pg1.Refresh();

            if (M_Cnt >= 3)
            {
                StrSql = " Select Mbid,Mbid2, ToEndDate,Cur_PV_1, Cur_PV_2   "; 
                StrSql = StrSql + " From tbl_ClosePay_01_Mod (nolock) "  ; 
                StrSql = StrSql + " Where Cur_PV_1 +  Cur_PV_2   >0    "  ; 
                StrSql = StrSql + " And ToEndDate = '" + M_Date3 + "'"  ;

                ReCnt = 0;
                DataSet Dset4 = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
                ReCnt = Search_Connect.DataSet_ReCount;
                //Dset.Tables[base_db_name].Rows[0][0].ToString();

                if (ReCnt > 0)
                {
                    pg1.Value = 0; pg1.Maximum = ReCnt;

                    for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                    {
                        StrSql = "Select Mbid,Mbid2 From tbl_ClosePay_01_Mod  (nolock)  ";
                        StrSql = StrSql + "  Where Mbid  = '" + Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "'";
                        StrSql = StrSql + "  And   Mbid2 =  " + Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                        StrSql = StrSql + "  And   ToEndDate  = '" + Dset4.Tables[base_db_name].Rows[fi_cnt]["ToEndDate"].ToString() + "'";
                        StrSql = StrSql +  " And   Allowance2  > 0 " ;
                                                
                        DataSet Dset5 = new DataSet();
                        Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset5);                        
                        int ReCnt5 = Search_Connect.DataSet_ReCount;

                        if (ReCnt5 <= 0)
                        {
                            StrSql = "Update tbl_ClosePay_01 SET ";
                            StrSql = StrSql + " Cut_PV_4_1 =  " + Dset4.Tables[base_db_name].Rows[fi_cnt]["Cur_PV_1"].ToString();
                            StrSql = StrSql + ",Cut_PV_4_2 =  " + Dset4.Tables[base_db_name].Rows[fi_cnt]["Cur_PV_2"].ToString();
                            StrSql = StrSql + "  Where Mbid  = '" + Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "'";
                            StrSql = StrSql + "  And   Mbid2 =  " + Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();

                             Temp_Connect.Insert_Data(StrSql, Conn, tran);
                        }

                        pg1.PerformStep(); pg1.Refresh();
                    } // for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)

                } // (ReCnt != 0)
                
            }//  if (M_Cnt >= 3)
            
        }




        private void Put_cls_Close_Mem(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            
            StrSql = "Select Mbid,Mbid2, M_Name, Saveid, Saveid2, Nominid, Nominid2, LineCnt , N_LineCnt, LeaveDate, StopDate  ";
            StrSql = StrSql + " ,DayPV01, DayPV02 , DayPV03, SellPV01 , SellPV02 ,SellPV03 ";
            StrSql = StrSql + " ,ReqTF1, ReqTF2 , RBO_Mem_TF,  Sell_Mem_TF ";
            StrSql = StrSql + " ,CurPoint , CurGrade, OneGrade, OrgGrade   ";
            StrSql = StrSql + "  From tbl_ClosePay_01 (nolock) ";

            
            SqlDataReader sr = null;            
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;
            
            string T_Mbid = "";

            Dictionary<string, cls_Close_Mem> T_Clo_Mem = new Dictionary<string, cls_Close_Mem>();

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            
            while (sr.Read())
            {
                cls_Close_Mem t_c_mem = new cls_Close_Mem();
                T_Mbid = sr.GetValue(0).ToString() + "-" + sr.GetValue(1).ToString();


                t_c_mem.Mbid = sr.GetValue(0).ToString();
                t_c_mem.Mbid2 = int.Parse(sr.GetValue(1).ToString());
                t_c_mem.M_Name = sr.GetValue(2).ToString();

                t_c_mem.Saveid = sr.GetValue(3).ToString();
                t_c_mem.Saveid2 = int.Parse(sr.GetValue(4).ToString());

                t_c_mem.Nominid = sr.GetValue(5).ToString()  ;
                t_c_mem.Nominid2 = int.Parse(sr.GetValue(6).ToString());

                t_c_mem.LineCnt = int.Parse(sr.GetValue(7).ToString());
                t_c_mem.N_LineCnt = int.Parse(sr.GetValue(8).ToString());

                t_c_mem.LeaveDate = sr.GetValue(9).ToString();
                t_c_mem.StopDate = sr.GetValue(10).ToString();

                t_c_mem.DayPV01 = int.Parse(sr.GetValue(11).ToString());
                t_c_mem.DayPV02 = int.Parse(sr.GetValue(12).ToString());
                t_c_mem.DayPV03 = int.Parse(sr.GetValue(13).ToString());

                t_c_mem.SellPV01 = int.Parse(sr.GetValue(14).ToString());
                t_c_mem.SellPV02 = int.Parse(sr.GetValue(15).ToString());
                t_c_mem.SellPV03 = int.Parse(sr.GetValue(16).ToString());

                t_c_mem.ReqTF1 = int.Parse(sr.GetValue(17).ToString());
                t_c_mem.ReqTF2 = int.Parse(sr.GetValue(18).ToString());

                t_c_mem.RBO_Mem_TF = int.Parse(sr.GetValue(19).ToString());
                t_c_mem.Sell_Mem_TF = int.Parse(sr.GetValue(20).ToString());

                t_c_mem.CurPoint = int.Parse(sr.GetValue(21).ToString());

                t_c_mem.CurGrade = int.Parse(sr.GetValue(22).ToString());
                t_c_mem.OneGrade  = int.Parse(sr.GetValue(23).ToString());
                t_c_mem.OrgGrade  = int.Parse(sr.GetValue(24).ToString());
                
                
                T_Clo_Mem[T_Mbid] = t_c_mem;

                pg1.PerformStep(); pg1.Refresh();
            }


            Clo_Mem = T_Clo_Mem;
            sr.Close(); sr.Dispose();            
        }





        private void Put_Mem_Sell_Info(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
            int WCnt = 0 ;

            StrSql = " Select     Se.TotalPV ,Se.TotalPrice , Isnull( Bs_R.TotalPV, 0 ) AS RePV , Isnull( Bs_R.TotalPrice, 0 ) AS RePrice , Nominid " ;
            StrSql = StrSql + " , Nominid2, Saveid , Saveid2 , Ce1.Mbid , Ce1.Mbid2  " ;
            StrSql = StrSql + " , Ce1.N_LineCnt , Ce1.LineCnt  , Se.M_Name, Se.OrderNumber, Se.SellCode  ";
            StrSql = StrSql + " , Se.SellDate,  Ce1.SellPv01, Ce1.DayPV01   ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";    
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R   (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate <= '" + ToEndDate + "'";        
            //StrSql = StrSql + " LEFT JOIN tbl_SalesDetail_TF  (nolock)  ON Se.OrderNumber = tbl_SalesDetail_TF.OrderNumber";
            StrSql = StrSql + " LEFT JOIN tbl_ClosePay_01 Ce1 ON Ce1.Mbid = SE.Mbid And Ce1.Mbid2 = SE.Mbid2";

            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) >= 0 ";    
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Ga_Order = 0 ";
            StrSql = StrSql + " And   Ce1.Mbid2 Is not null ";

            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1; 

            C_Sell = new cls_Close_Sell [ReCnt] ;

            //Dictionary<string, cls_Close_Sell> T_Clo_Sell = new Dictionary<string, cls_Close_Sell>();
                        
            while(sr.Read ())
            {
                cls_Close_Sell t_c_mem = new cls_Close_Sell();
                
                t_c_mem.Mbid = sr.GetValue(8).ToString();
                t_c_mem.Mbid2 = int.Parse(sr.GetValue(9).ToString());
                t_c_mem.M_Name = sr.GetValue(12).ToString();

                t_c_mem.Saveid = sr.GetValue(6).ToString();
                t_c_mem.Saveid2 = int.Parse(sr.GetValue(7).ToString());

                t_c_mem.Nominid = sr.GetValue(4).ToString()  ;
                t_c_mem.Nominid2 = int.Parse(sr.GetValue(5).ToString());

                t_c_mem.LineCnt = int.Parse(sr.GetValue(11).ToString());
                t_c_mem.N_LineCnt = int.Parse(sr.GetValue(10).ToString());

                t_c_mem.DayPV01 = double.Parse(sr.GetValue(17).ToString());
                t_c_mem.DayPV02 = 0 ;//double.Parse(sr.GetValue(12).ToString());
                t_c_mem.DayPV03 = 0; //double.Parse(sr.GetValue(13).ToString());

                t_c_mem.SellPV01 = double.Parse(sr.GetValue(16).ToString());
                t_c_mem.SellPV02 = 0; //double.Parse(sr.GetValue(15).ToString());
                t_c_mem.SellPV03 = 0; //double.Parse(sr.GetValue(16).ToString());

                t_c_mem.SellCode  = sr.GetValue(14).ToString();
                t_c_mem.SellDate = sr.GetValue(15).ToString();
                t_c_mem.OrderNumber  = sr.GetValue(13).ToString();

                t_c_mem.TotalPV  = double.Parse(sr.GetValue(0).ToString());
                t_c_mem.TotalPrice = double.Parse(sr.GetValue(1).ToString());

                t_c_mem.RePV  = double.Parse(sr.GetValue(2).ToString());
                t_c_mem.RePrice = double.Parse(sr.GetValue(3).ToString());

                t_c_mem.CurGrade =  0; //int.Parse(sr.GetValue(19).ToString());
                t_c_mem.CurPoint =  0; //int.Parse(sr.GetValue(20).ToString());

                //T_Clo_Sell[t_c_mem.OrderNumber] = t_c_mem;

                C_Sell[WCnt] = t_c_mem;
                WCnt++;
                pg1.PerformStep(); pg1.Refresh();
            }

            //Clo_Sell = T_Clo_Sell;
            sr.Close(); sr.Dispose();  
        }


        private void Put_Down_PV_01(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double Allowance1 = 0, TotalPV = 0, TH_TotalPV = 0, KR_TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select Se.TotalPV TotalPV , 0 AS RePV  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2, Se.Na_Code  ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalCV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate <= '" + PayDate + "'";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalCV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate <= '" + ToEndDate + "'";

            StrSql = StrSql + " WHERE Se.TotalPV  > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Se.SellCode IN ('01','02') ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                LevelCnt = 0; TSaveid = "**";
                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                if (Mbid2 == 1343)
                    S_Mbid = Mbid + "-" + Mbid2.ToString();

                S_Mbid = Mbid + "-" + Mbid2.ToString();
                if (Clo_Mem.ContainsKey(S_Mbid) == true)
                {
                    TSaveid = Clo_Mem[S_Mbid].Saveid;
                    TSaveid2 = Clo_Mem[S_Mbid].Saveid2;
                    TLine = Clo_Mem[S_Mbid].LineCnt;
                }

                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                //TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());


                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        //판매원한태 누적잡히도록 초기 설정을함. 문건을 보고 판단함.
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" 
                            && Clo_Mem[S_Mbid].Sell_Mem_TF == 0 
                            && Clo_Mem[S_Mbid].CurPoint >= 10
                            && Clo_Mem[S_Mbid].RBO_Mem_TF == 0                            
                            )
                        {

                            R_LevelCnt++;

                            StrSql = "Update tbl_ClosePay_01 SET ";


                            if (TLine == 1)
                                StrSql = StrSql + " Cur_PV_1 = Cur_PV_1 +  " + TotalPV;
                            else
                                StrSql = StrSql + " Cur_PV_2 = Cur_PV_2 +  " + TotalPV;


                            StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                            StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                            t_qu[t_qu_Cnt] = StrSql;
                            t_qu_Cnt++;



                            StrSql = "INSERT INTO tbl_Close_DownPV_PV_01";
                            StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                            StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV , ";
                            StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber  ) ";

                            StrSql = StrSql + "Values(";
                            StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                            StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                            StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";

                            StrSql = StrSql + TotalPV + " , " + LevelCnt + " ," + TLine;

                            StrSql = StrSql + ",'1' ,'" + OrderNumber + "')";

                            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                            t_qu[t_qu_Cnt] = StrSql;
                            t_qu_Cnt++;


                        }

                        TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    //if (LevelCnt == 2) TSaveid = "**";

                } //While

            }



            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

            //pg1.Value = 0;             
            //var sellCnt = from sellinfo in C_Sell                            
            //                group sellinfo by  sellinfo.SellCode == "01" into Gr
            //                select new
            //                {
            //                    T_Count =  Gr.Count ()
            //                };

            //foreach (var stateCnt in sellCnt )
            //{
            //    pg1.Maximum = stateCnt.T_Count ;
            //}
            //pg1.PerformStep(); pg1.Refresh();


            //var sellinfos = from sellinfo in C_Sell
            //                where sellinfo.SellCode == "01"
            //                orderby sellinfo.OrderNumber  
            //                select new
            //                {
            //                    TotalPV = sellinfo.TotalPV,
            //                    RePV = sellinfo.RePV,
            //                    SellDate = sellinfo.SellDate,
            //                    OrderNumber = sellinfo.OrderNumber,
            //                    M_Name = sellinfo.M_Name ,
            //                    Mbid = sellinfo.Mbid ,
            //                    Mbid2 = sellinfo.Mbid2 ,
            //                    Saveid = sellinfo.Saveid ,
            //                    Saveid2 = sellinfo.Saveid2,
            //                    LineCnt = sellinfo.LineCnt 
            //                };

            //int LevelCnt = 0 ,TSaveid2 = 0 , TLine = 0 ;
            //string TSaveid = "",S_Mbid = ""; 
            //double Rs_TotalPV = 0 ,Rs_RePV = 0 ;
            //string StrSql = "";



            //foreach (var sellinfo in sellinfos )
            //{
            //    LevelCnt = 0;
            //    TSaveid = sellinfo.Saveid.ToString () ;
            //    TSaveid2 = int.Parse(sellinfo.Saveid2.ToString());
            //    TLine = int.Parse(sellinfo.LineCnt.ToString());
            //    Rs_TotalPV = double.Parse(sellinfo.TotalPV .ToString());
            //    Rs_RePV = double.Parse(sellinfo.RePV.ToString());

            //    S_Mbid = TSaveid + "-" + TSaveid2.ToString ();
            //    while (TSaveid != "**")
            //    {
            //        LevelCnt  ++ ; 
                    
            //        if (Clo_Mem.ContainsKey(S_Mbid) == true)
            //        {
            //            //if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && (Clo_Mem[S_Mbid].CurGrade >=20 || Clo_Mem[S_Mbid].CurPoint >=1 )  )
            //            if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" )
            //            {
            //                StrSql = "Update tbl_ClosePay_01 SET ";
            //                if (TLine == 1) 
            //                     StrSql = StrSql + " Cur_PV_1 = Cur_PV_1 +  " + (Rs_TotalPV + Rs_RePV) ;
                                             
            //                 if (TLine >= 2)
            //                     StrSql = StrSql + " Cur_PV_2 = Cur_PV_2 + " + (Rs_TotalPV + Rs_RePV) ; 

            //                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
            //                StrSql = StrSql + " And     Mbid2 = " + TSaveid2 ;

            //                Temp_Connect.Insert_Data(StrSql, Conn, tran); 

                   
            //                StrSql = "INSERT INTO tbl_Close_DownPV_PV_01" ;
            //                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName," ;
            //                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV ,  ";
            //                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_DownPV) ";
                         
            //                StrSql = StrSql + "Values(" ;
            //                StrSql = StrSql + "'" + ToEndDate + "','" + sellinfo.Mbid.ToString() + "'";
            //                StrSql = StrSql + "," + sellinfo.Mbid2.ToString() + ",'" + sellinfo.M_Name.ToString() + "',";
            //                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
            //                StrSql = StrSql + (Rs_TotalPV + Rs_RePV) + ", " + LevelCnt + " ," + TLine;
            //                StrSql = StrSql + ",'01' ,'" + sellinfo.OrderNumber.ToString () + "',0)";

            //                Temp_Connect.Insert_Data(StrSql, Conn, tran) ;


            //            }

            //            TSaveid = Clo_Mem[S_Mbid].Saveid ;  TSaveid2 = Clo_Mem[S_Mbid].Saveid2 ;    TLine = Clo_Mem[S_Mbid].LineCnt ;
            //            S_Mbid = TSaveid + "-" + TSaveid2.ToString();
            //        }
            //        else
            //        {
            //            TSaveid = "**";
            //        }


            //    } //foreach


            //    pg1.PerformStep(); pg1.Refresh();
            //}


            //StrSql = "Update tbl_ClosePay_01 SET ";
            //StrSql = StrSql + "   Sum_PV_1 = Be_PV_1 + Cur_PV_1 - Cut_PV_4_1 ";
            //StrSql = StrSql + "  ,Sum_PV_2 = Be_PV_2 + Cur_PV_2 - Cut_PV_4_2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
    
        }


        private void Put_Down_PV_01_After(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double Allowance1 = 0, TotalPV = 0, TH_TotalPV = 0, KR_TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select Se.TotalPV TotalPV , 0 AS RePV  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2, Se.Na_Code  ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
        
            StrSql = StrSql + " WHERE Se.TotalPV  > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='20170925'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ordernumber not in (Select Distinct Ordernumber From tbl_Close_DownPV_PV_01 (nolock ) Where SortOrder = '1' )  ";

            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Se.SellCode IN ('01','02') ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                LevelCnt = 0; TSaveid = "**";
                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                if (Mbid2 == 1343)
                    S_Mbid = Mbid + "-" + Mbid2.ToString();

                S_Mbid = Mbid + "-" + Mbid2.ToString();
                if (Clo_Mem.ContainsKey(S_Mbid) == true)
                {
                    TSaveid = Clo_Mem[S_Mbid].Saveid;
                    TSaveid2 = Clo_Mem[S_Mbid].Saveid2;
                    TLine = Clo_Mem[S_Mbid].LineCnt;
                }

                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                //TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());


                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        //판매원한태 누적잡히도록 초기 설정을함. 문건을 보고 판단함.
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == ""
                            && Clo_Mem[S_Mbid].Sell_Mem_TF == 0
                            && Clo_Mem[S_Mbid].CurPoint >= 10
                            && Clo_Mem[S_Mbid].RBO_Mem_TF == 0
                            )
                        {

                            R_LevelCnt++;

                            StrSql = "Update tbl_ClosePay_01 SET ";


                            if (TLine == 1)
                                StrSql = StrSql + " Cur_PV_1 = Cur_PV_1 +  " + TotalPV;
                            else
                                StrSql = StrSql + " Cur_PV_2 = Cur_PV_2 +  " + TotalPV;


                            StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                            StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                            t_qu[t_qu_Cnt] = StrSql;
                            t_qu_Cnt++;



                            StrSql = "INSERT INTO tbl_Close_DownPV_PV_01";
                            StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                            StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV , ";
                            StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber  ) ";

                            StrSql = StrSql + "Values(";
                            StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                            StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                            StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";

                            StrSql = StrSql + TotalPV + " , " + LevelCnt + " ," + TLine;

                            StrSql = StrSql + ",'1' ,'" + OrderNumber + "')";

                            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                            t_qu[t_qu_Cnt] = StrSql;
                            t_qu_Cnt++;


                        }

                        TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    //if (LevelCnt == 2) TSaveid = "**";

                } //While

            }



            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

          

        }



        private void Put_Down_PV_Re(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0, LineCnt = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", Re_BaseOrderNumber = "";
            double TotalPV = 0, Sell_DownPV = 0, Cut_PV = 0, TH_TotalPV = 0, KR_TotalPV = 0;
            string SaveMbid = "", SaveName = "", Take_Na_Code = "";
            int SaveMbid2 = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select Se.TotalPV TotalPV  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2, Se.Re_BaseOrderNumber , Se.Na_Code ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " WHERE Se.TotalPV  <  0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            //StrSql = StrSql + " And   Se.ReturnTF = 2 ";
            //StrSql = StrSql + " And   Se.SellCode = '01' ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();
                Re_BaseOrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["Re_BaseOrderNumber"].ToString();
                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                //if (ds.Tables[base_db_name].Rows[fi_cnt]["Na_Code"].ToString() == "TH" )
                //{
                //    TotalPV = Chang_Search_Th_Re_Ord(OrderNumber, Re_BaseOrderNumber);
                //}




                StrSql = "SELECT  Sell_DownPV , SaveMbid, SaveMbid2, SaveName ,SortOrder , LineCnt ,LevelCnt     ";
                StrSql = StrSql + " From tbl_Close_DownPV_PV_01 (nolock) ";
                StrSql = StrSql + " WHERE RequestMbid = '" + Mbid + "'";
                StrSql = StrSql + " And   RequestMbid2 = " + Mbid2;
                StrSql = StrSql + " And   OrderNumber = '" + Re_BaseOrderNumber + "'";
                StrSql = StrSql + " And   SortOrder <> '-1'  ";

                DataSet ds_2 = new DataSet();
                int ReCnt_2 = 0;
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds_2);
                ReCnt_2 = Search_Connect.DataSet_ReCount;

                for (int fi_cnt_2 = 0; fi_cnt_2 <= ReCnt_2 - 1; fi_cnt_2++)
                {
                    LineCnt = int.Parse(ds_2.Tables[base_db_name].Rows[fi_cnt_2]["LineCnt"].ToString());
                    SaveMbid = ds_2.Tables[base_db_name].Rows[fi_cnt_2]["SaveMbid"].ToString();
                    SaveMbid2 = int.Parse(ds_2.Tables[base_db_name].Rows[fi_cnt_2]["SaveMbid2"].ToString());
                    SaveName = ds_2.Tables[base_db_name].Rows[fi_cnt_2]["SaveName"].ToString();
                    LevelCnt = int.Parse(ds_2.Tables[base_db_name].Rows[fi_cnt_2]["LevelCnt"].ToString());
                    Sell_DownPV = double.Parse(ds_2.Tables[base_db_name].Rows[fi_cnt_2]["Sell_DownPV"].ToString());


                    if (Sell_DownPV == -TotalPV)
                        Cut_PV = TotalPV;
                    else
                        Cut_PV = TotalPV;
                    //Cut_PV = -Sell_DownPV;



                    StrSql = "Update tbl_ClosePay_01 SET ";

                    if (LineCnt == 1)
                    {
                        StrSql = StrSql + " Cur_PV_1 = Cur_PV_1 +  " + Cut_PV;
                        StrSql = StrSql + " ,Re_Cur_PV_1 = Re_Cur_PV_1 +  " + Cut_PV;
                    }
                    else
                    {
                        StrSql = StrSql + " Cur_PV_2 = Cur_PV_2 + " + Cut_PV;
                        StrSql = StrSql + " ,Re_Cur_PV_2 = Re_Cur_PV_2 +  " + Cut_PV;
                    }

                    StrSql = StrSql + " Where   Mbid = '" + SaveMbid + "'";
                    StrSql = StrSql + " And     Mbid2 = " + SaveMbid2;

                    t_qu[t_qu_Cnt] = StrSql;
                    t_qu_Cnt++;


                    StrSql = "INSERT INTO tbl_Close_DownPV_PV_01";
                    StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                    StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV ,  ";
                    StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_DownPV) ";
                    StrSql = StrSql + "Values(";

                    StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'," + Mbid2 + ",'" + M_Name + "',";
                    StrSql = StrSql + "'" + SaveMbid + "'," + SaveMbid2 + ",'" + SaveName + "',";
                    StrSql = StrSql + Cut_PV + ", " + LevelCnt + " ," + LineCnt;
                    StrSql = StrSql + ",'-1' ,'" + OrderNumber + "'," + TotalPV + " )";


                    t_qu[t_qu_Cnt] = StrSql;
                    t_qu_Cnt++;

                }      // end for          

            } //end for


            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }





        private void Put_Down_PV_02(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 9;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            string SDate3 = "";

            DateTime dt = DateTime.Parse(ToEndDate.Substring(0, 4) + "-" + ToEndDate.Substring(4, 2) + "-" + ToEndDate.Substring(6, 2));
            SDate3 = dt.AddMonths(-3).ToShortDateString().Replace("-", "");


            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + "  Sham_PV_1 = Isnull(B.A1,0 )  ";
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, ";

            StrSql = StrSql + " (Select Sum(Apply_Pv) A1 ,   Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_Sham_Sell_Down_2  (nolock) ";
            StrSql = StrSql + " Where Apply_Date >= '" + FromEndDate + "'";
            StrSql = StrSql + " And   Apply_Date <= '" + ToEndDate + "'";
            StrSql = StrSql + " And   SellCode = '1' ";
            StrSql = StrSql + " Group By Mbid,MBid2 ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + "  Sham_PV_2 = Isnull(B.A1,0 )  ";
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, ";

            StrSql = StrSql + " (Select Sum(Apply_Pv) A1 ,   Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_Sham_Sell_Down_2  (nolock) ";
            StrSql = StrSql + " Where Apply_Date >= '" + FromEndDate + "'";
            StrSql = StrSql + " And   Apply_Date <= '" + ToEndDate + "'";
            StrSql = StrSql + " And   SellCode = '2' ";
            StrSql = StrSql + " Group By Mbid,MBid2 ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);



            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + "   Sum_PV_1 = Be_PV_1 + Cur_PV_1 + Sham_PV_1  ";
            StrSql = StrSql + "  ,Sum_PV_2 = Be_PV_2 + Cur_PV_2 + Sham_PV_2 ";
            StrSql = StrSql + " Where Sell_Mem_TF = 0  ";  //소비자일경우에 하선누적 이월은 없다. 
            Temp_Connect.Insert_Data(StrSql, Conn, tran);



            // 후원인 변경이 일어난 건에 대해서 마감 일자 기준 변경 일자 기준으로 해서... 이전 내역 안가져 간다.
            //2018-04-10 김종국 이사님 요청에 의해섬
            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + "   Sum_PV_1 =  Cur_PV_1 + Sham_PV_1  ";
            StrSql = StrSql + "  ,Sum_PV_2 =  Cur_PV_2 + Sham_PV_2 ";
            StrSql = StrSql + " Where Sell_Mem_TF = 0  ";  //소비자일경우에 하선누적 이월은 없다. 
            StrSql = StrSql + " And Mbid2 in (Select Mbid2  From tbl_Memberinfo_Save_Nomin_Change (nolock) Where Save_Nomin_SW = 'SAV' ";
            StrSql = StrSql + "              And Replace(LEFT(Recordtime,10) ,'-','')  >='" + FromEndDate  + "'" ;
            StrSql = StrSql + "              And Replace(LEFT(Recordtime,10) ,'-','')  <='" + ToEndDate + "'";
            StrSql = StrSql + "              ) "; 
            Temp_Connect.Insert_Data(StrSql, Conn, tran);



            if (FromEndDate == "20180409")  //4월 9일자 마감에 이전 0으로 리셋처리를 한다.
            {
                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + "   Sum_PV_1 =  Cur_PV_1 + Sham_PV_1  ";
                StrSql = StrSql + "  ,Sum_PV_2 = Be_PV_2 + Cur_PV_2 + Sham_PV_2 ";
                StrSql = StrSql + " Where Mbid2 = 466 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }


            //소비자일경우에 하선누적 이월은 없다.   2016-12-21 박해진대리 요청에 의해선
            StrSql = "Update tbl_ClosePay_01 set";
            StrSql = StrSql + " Sum_PV_1  = 0 , Sum_PV_2 = 0 ";
            StrSql = StrSql + " Where Sell_Mem_TF > 0  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
        }



        private void Give_Allowance6(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 20;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            //2015-12-16일 이홍민 본부장님 요청에 의해서  최고 직급 PD 이상으로 변경한다. 유지직급이 아니고 PD가 
            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + "  Allowance6 = Sum_PV_2 * " +( double.Parse (txtB1.Text ) / 100);

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            StrSql = StrSql + " Where (CurPoint = 10 ) ";
            StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";

            StrSql = StrSql + " And Sum_PV_1 > 0  ";
            StrSql = StrSql + " And Sum_PV_2 > 0  ";
            StrSql = StrSql + " And Sell_Mem_TF = 0  ";
            StrSql = StrSql + " And ReqTF1 = 1 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + " Allowance6 = Sum_PV_1 * " + (double.Parse(txtB1.Text) / 100);

            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            StrSql = StrSql + " Where (CurPoint = 10 ) ";
            StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            StrSql = StrSql + " And Sum_PV_1 > 0  ";
            StrSql = StrSql + " And Sum_PV_2 > 0  ";
            StrSql = StrSql + " And Sell_Mem_TF = 0  ";
            StrSql = StrSql + " And ReqTF1 = 1 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''




            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + "  Allowance6 = Sum_PV_2 *" + (double.Parse(txtB2.Text) / 100);

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            StrSql = StrSql + " Where (CurPoint = 20 ) ";
            StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql + " And Sum_PV_1 > 0  ";
            StrSql = StrSql + " And Sum_PV_2 > 0  ";
            StrSql = StrSql + " And Sell_Mem_TF = 0  ";
            StrSql = StrSql + " And ReqTF1 = 1 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + " Allowance6 = Sum_PV_1 * " + (double.Parse(txtB2.Text) / 100);

            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            StrSql = StrSql + " Where (CurPoint = 20 ) ";
            StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            StrSql = StrSql + " And Sum_PV_1 > 0  ";
            StrSql = StrSql + " And Sum_PV_2 > 0  ";
            StrSql = StrSql + " And Sell_Mem_TF = 0  ";
            StrSql = StrSql + " And ReqTF1 = 1 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''




            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + "  Allowance6 = Sum_PV_2 * " + (double.Parse(txtB3.Text) / 100);

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            StrSql = StrSql + " Where (CurPoint = 30 ) ";
            StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";

            StrSql = StrSql + " And Sum_PV_1 > 0  ";
            StrSql = StrSql + " And Sum_PV_2 > 0  ";
            StrSql = StrSql + " And Sell_Mem_TF = 0  ";
            StrSql = StrSql + " And ReqTF1 = 1 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + " Allowance6 = Sum_PV_1 * " + (double.Parse(txtB3.Text) / 100);

            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            StrSql = StrSql + " Where (CurPoint = 30 ) ";
            StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            StrSql = StrSql + " And Sum_PV_1 > 0  ";
            StrSql = StrSql + " And Sum_PV_2 > 0  ";
            StrSql = StrSql + " And Sell_Mem_TF = 0  ";
            StrSql = StrSql + " And ReqTF1 = 1 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + "  Allowance6 = Sum_PV_2 * " + (double.Parse(txtB4.Text) / 100);

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            StrSql = StrSql + " Where (CurPoint = 40 ) ";
            StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";

            StrSql = StrSql + " And Sum_PV_1 > 0  ";
            StrSql = StrSql + " And Sum_PV_2 > 0  ";
            StrSql = StrSql + " And Sell_Mem_TF = 0  ";
            StrSql = StrSql + " And ReqTF1 = 1 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + " Allowance6 = Sum_PV_1 * " + (double.Parse(txtB4.Text) / 100);

            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            StrSql = StrSql + " Where (CurPoint = 40 ) ";
            StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            StrSql = StrSql + " And Sum_PV_1 > 0  ";
            StrSql = StrSql + " And Sum_PV_2 > 0  ";
            StrSql = StrSql + " And Sell_Mem_TF = 0  ";
            StrSql = StrSql + " And ReqTF1 = 1 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''




            if (int.Parse(FromEndDate) >= 20170731)
            {
                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " Max_Pay =  250000 ";
                StrSql = StrSql + " Where CurPoint  = 10";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " Max_Pay =  500000 ";
                StrSql = StrSql + " Where CurPoint  = 20";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " Max_Pay =  3000000 ";
                StrSql = StrSql + " Where CurPoint  = 30";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();


                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " Max_Pay =  6000000 ";
                StrSql = StrSql + " Where CurPoint  >= 40";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " Allowance6_Cut =  Allowance6 - Max_Pay ";
                StrSql = StrSql + " ,Allowance6 = Max_Pay ";
                StrSql = StrSql + " Where Allowance6 > Max_Pay";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }
            else
            {
                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " Max_Pay =  5000000 ";
                StrSql = StrSql + " Where CurPoint  = 10";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " Max_Pay =  10000000 ";
                StrSql = StrSql + " Where CurPoint  = 20";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " Max_Pay =  20000000 ";
                StrSql = StrSql + " Where CurPoint  = 30";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();


                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " Max_Pay =  40000000 ";
                StrSql = StrSql + " Where CurPoint  >= 40";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " Allowance6_Cut =  Allowance6 - Max_Pay ";
                StrSql = StrSql + " ,Allowance6 = Max_Pay ";
                StrSql = StrSql + " Where Allowance6 > Max_Pay";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }



            //StrSql = "Update tbl_ClosePay_01 SET ";
            //StrSql = StrSql + " Fresh_1 = Sum_PV_1 ";
            //StrSql = StrSql + " ,Sum_PV_1 = 0 ";          
            //StrSql = StrSql + " Where Sum_PV_1 > 0  ";
            //StrSql = StrSql + " And ReqTF2 = 0  ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_01 SET ";
            //StrSql = StrSql + " Fresh_2 = Sum_PV_2 ";
            //StrSql = StrSql + " ,Sum_PV_2 = 0 ";
            //StrSql = StrSql + " Where Sum_PV_2 > 0  ";
            //StrSql = StrSql + " And ReqTF2 = 0  ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            /*2016-06-08 입력한 %에 의해 팀커미션 초과유무 확인 및 금액 변경*/

            //if (int.Parse(FromEndDate) >= int.Parse("20160601"))
            //{
            //    double TotalPrice = 0, TotalAllowance6 = 0, Cut_Per2 = 0, S_CutPay2 = 0;


            //    StrSql = " Select SUM(TotalPrice) from tbl_SalesDetail (nolock) ";
            //    StrSql = StrSql + " Where Ga_Order = 0 And SellCode <> '' ";
            //    StrSql = StrSql + " And SellDate between '" + FromEndDate + "' and '" + ToEndDate + "' ";

            //    DataSet ds = new DataSet();
            //    ReCnt = 0;
            //    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            //    ReCnt = Search_Connect.DataSet_ReCount;

            //    if (ReCnt > 0)
            //    {
            //        TotalPrice = double.Parse(ds.Tables[base_db_name].Rows[0][0].ToString());
            //    }

            //    StrSql = " Select ISNULL(SUM(Allowance6), 0) From tbl_ClosePay_01 (nolock) Where Allowance6 > 0 ";
            //    DataSet ds2 = new DataSet();
            //    int ReCnt2 = 0;
            //    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds2);
            //    ReCnt2 = Search_Connect.DataSet_ReCount;

            //    if (ReCnt2 > 0)
            //    {
            //        TotalAllowance6 = double.Parse(ds2.Tables[base_db_name].Rows[0][0].ToString());
            //    }

            //    if (TotalAllowance6 > 0)
            //    {
            //        if (TotalAllowance6 / TotalPrice * 100 > Kor_Pay)
            //        {
            //            Cut_Per2 = ((TotalAllowance6 / TotalPrice) * 100) - Kor_Pay;
            //            S_CutPay2 = TotalPrice * (Cut_Per2 / 100);

            //            StrSql = "Update tbl_ClosePay_01 Set ";
            //            StrSql = StrSql + "  Allowance6_Cut_2 = " + S_CutPay2 + " * (Allowance6 /" + TotalAllowance6 + ")";
            //            StrSql = StrSql + " Where Allowance6  > 0 ";
            //            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            //            StrSql = "Update tbl_ClosePay_01 Set ";
            //            StrSql = StrSql + " Allowance6= Allowance6 - Allowance6_Cut_2 ";
            //            StrSql = StrSql + " Where Allowance6  > 0 And Allowance6_Cut_2 >0 ";
            //            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //        }
            //    }
            //}

            ////2016-10-07 요청에 의해서 아래로 내려옴... 매출 대비 공제 처리하고 본인 주간 공제로..
            //StrSql = "Update tbl_ClosePay_01 SET ";
            //StrSql = StrSql + " Allowance6_Cut =  Allowance6 - Max_Pay ";
            //StrSql = StrSql + " ,Allowance6 = Max_Pay ";
            //StrSql = StrSql + " Where Allowance6 > Max_Pay";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();



            ////2016-04-21 소수점이하 절삭
            //StrSql = " Update tbl_ClosePay_01 SET ";
            //StrSql = StrSql + " Allowance6 = ROUND(Allowance6, 0 ,1) ";
            //StrSql = StrSql + " Where Allowance6 > 0 ";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


        }



        private void Give_Allowance7(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            //우대회원의 매출에 대해서는 상위 추천인 한태 준다. 우대회원의 매출을 상위 ibo로 잡지는 않는다.
            StrSql = " Select Allowance6  , M_Name,  Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_ClosePay_01  (nolock) ";
            StrSql = StrSql + " WHERE Allowance6  > 0 ";
            StrSql = StrSql + " Order by Mbid , Mbid2  ASC ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance6"].ToString());  //+ double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ""; //ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                SellDate = ""; //ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                //if (Mbid2 == 112924)
                //    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                TSaveid = Clo_Mem[S_Mbid].Nominid;
                TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                TLine = Clo_Mem[S_Mbid].N_LineCnt;

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {

                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == ""
                            && Clo_Mem[S_Mbid].CurPoint >= 20 
                            && Clo_Mem[S_Mbid].Sell_Mem_TF == 0
                            && Clo_Mem[S_Mbid].RBO_Mem_TF == 0                            
                            && Clo_Mem[S_Mbid].ReqTF2  >  0 //유지를 한 사람에 대해서만 지급을 한다 추천매칭을
                            )
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;
                            if (LevelCnt == 1 ) Allowance1 = R_TotalPV *  ( double.Parse (txtB5.Text ) / 100);
                            if (LevelCnt == 2) Allowance1 = R_TotalPV * (double.Parse(txtB6.Text) / 100);
                            if (LevelCnt == 3) Allowance1 = R_TotalPV * (double.Parse(txtB7.Text) / 100);



                            if (LevelCnt >= 2 && Clo_Mem[S_Mbid].CurPoint <= 20) Allowance1 = 0;
                            if (LevelCnt >= 3 && Clo_Mem[S_Mbid].CurPoint <= 30) Allowance1 = 0;                            

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_01 SET ";
                                StrSql = StrSql + " Allowance7 = Allowance7 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'7' ,'" + OrderNumber + "'," + R_LevelCnt + ")";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }


                        }

                        TSaveid = Clo_Mem[S_Mbid].Nominid; TSaveid2 = Clo_Mem[S_Mbid].Nominid2; TLine = Clo_Mem[S_Mbid].N_LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (LevelCnt == 3) TSaveid = "**";

                } //While

            }



            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }




        private void Give_Allowance7_2017_0731(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            //우대회원의 매출에 대해서는 상위 추천인 한태 준다. 우대회원의 매출을 상위 ibo로 잡지는 않는다.
            StrSql = " Select Allowance6 + Allowance8 + Allowance9  Allowance6  , M_Name,  Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_ClosePay_01  (nolock) ";
            StrSql = StrSql + " WHERE Allowance6 + Allowance8 + Allowance9  > 0 ";
            StrSql = StrSql + " Order by Mbid , Mbid2  ASC ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance6"].ToString());  //+ double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ""; //ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                SellDate = ""; //ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                //if (Mbid2 == 112924)
                //    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                TSaveid = Clo_Mem[S_Mbid].Nominid;
                TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                TLine = Clo_Mem[S_Mbid].N_LineCnt;

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {

                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == ""
                            && Clo_Mem[S_Mbid].CurPoint >= 20
                            && Clo_Mem[S_Mbid].Sell_Mem_TF == 0
                            && Clo_Mem[S_Mbid].RBO_Mem_TF == 0
                            && Clo_Mem[S_Mbid].ReqTF2 > 0 //유지를 한 사람에 대해서만 지급을 한다 추천매칭을
                            )
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;
                            Allowance1 = R_TotalPV * 0.02;
                            


                            if (LevelCnt >= 3 && Clo_Mem[S_Mbid].CurPoint <= 20) Allowance1 = 0;
                            if (LevelCnt >= 5 && Clo_Mem[S_Mbid].CurPoint <= 30) Allowance1 = 0;

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_01 SET ";
                                StrSql = StrSql + " Allowance7 = Allowance7 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'7' ,'" + OrderNumber + "'," + R_LevelCnt + ")";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }


                        }

                        TSaveid = Clo_Mem[S_Mbid].Nominid; TSaveid2 = Clo_Mem[S_Mbid].Nominid2; TLine = Clo_Mem[S_Mbid].N_LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (LevelCnt == 7) TSaveid = "**";

                } //While

            }



            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }


        private void Give_Allowance8(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double Allowance2 = 0, Allowance1 = 0, R_TotalPV = 0, TotalPV = 0, TotalPV_2 = 0, TH_TotalPV = 0, KR_TotalPV = 0, GivePay = 0;
            double L_1 = 0, L_2 = 0, L_3 = 0, L_4 = 0, L_5 = 0, L_6 = 0, L_05 = 0;

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            StrSql = " Select Se.TotalPV , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2 , Se.Na_Code ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";

            StrSql = StrSql + " WHERE Se.TotalPV  > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Se.SellCode IN ('01','02') ";

            StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2  ASC ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                TotalPV = 0; TotalPV_2 = 0;
                LevelCnt = 0; TSaveid = "**";

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                S_Mbid = Mbid + "-" + Mbid2.ToString();

                TSaveid = Clo_Mem[S_Mbid].Nominid;
                TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                TLine = Clo_Mem[S_Mbid].N_LineCnt;

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()); // +double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                if (OrderNumber == "2017033100200003")
                    OrderNumber = OrderNumber;

                if (int.Parse(FromEndDate) >= 20170731)
                {
                    L_05 = TotalPV * 0.2;
                    L_1 = TotalPV * 0.1; 
                }
                else
                {
                    L_05 = 0; 
                    L_1 = TotalPV * (double.Parse(txtB8.Text) / 100);
                }
                L_2 = TotalPV * (double.Parse(txtB9.Text) / 100);
                L_3 = TotalPV * (double.Parse(txtB10.Text) / 100);
                L_4 = TotalPV * (double.Parse(txtB11.Text) / 100);
                L_5 = TotalPV * (double.Parse(txtB12.Text) / 100);
                L_6 = TotalPV * (double.Parse(txtB13.Text) / 100);

               // TSaveid = Mbid;
                //TSaveid2 = Mbid2;
                //TLine = 1;

                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" 
                            && Clo_Mem[S_Mbid].CurGrade  >= 5
                            && Clo_Mem[S_Mbid].RBO_Mem_TF == 0
                            && Clo_Mem[S_Mbid].Sell_Mem_TF == 0 
                            
                            && Clo_Mem[S_Mbid].ReqTF2 >= 1)
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;
                            
                            if (Clo_Mem[S_Mbid].CurGrade == 5)
                            {
                                Allowance1 = L_05;
                                L_05 = 0;
                            }

                            //2017-05-29 Orggrade 에서 curgrade 로 변경 처리함.
                            if (Clo_Mem[S_Mbid].CurGrade == 10)
                            {
                                Allowance1 = L_05 +  L_1 ;
                                L_05 = 0; L_1 = 0;
                            }

                            if (Clo_Mem[S_Mbid].CurGrade == 20)
                            {
                                Allowance1 = L_05 + L_1 + L_2;
                                L_05 = 0;  L_1 = 0; L_2 = 0; 
                            }

                            if (Clo_Mem[S_Mbid].CurGrade == 30)
                            {
                                Allowance1 = L_05 + L_1 + L_2 + L_3;
                                L_05 = 0;  L_1 = 0; L_2 = 0; L_3 = 0; 
                            }

                            if (Clo_Mem[S_Mbid].CurGrade == 40)
                            {
                                Allowance1 = L_05 + L_1 + L_2 + L_3 + L_4;
                                L_05 = 0;  L_1 = 0; L_2 = 0; L_3 = 0; L_4 = 0;
                            }

                            if (Clo_Mem[S_Mbid].CurGrade == 50)
                            {
                                Allowance1 = L_05 + L_1 + L_2 + L_3 + L_4 + L_5;
                                L_05 = 0;  L_1 = 0; L_2 = 0; L_3 = 0; L_4 = 0; L_5 = 0;
                            }

                            if (Clo_Mem[S_Mbid].CurGrade == 60)
                            {
                                Allowance1 = L_05 + L_1 + L_2 + L_3 + L_4 + L_5 + L_6;
                                L_05 = 0;  L_1 = 0; L_2 = 0; L_3 = 0; L_4 = 0; L_5 = 0; L_6 = 0;
                            }

                            if (Allowance1 > 0)
                            {
                               

                                StrSql = "Update tbl_ClosePay_01 SET ";
                                StrSql = StrSql + " Allowance8 = Allowance8 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + TotalPV + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'8' ,'" + OrderNumber + "')";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                //동급 적용 수당을 지급한다. ########################
                                Give_Allowance9(Temp_Connect, Conn, tran, TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, TotalPV, OrderNumber, Clo_Mem[S_Mbid].CurGrade);
                                //동급 적용 수당을 지급한다. ########################
                            }


                        }

                        TSaveid = Clo_Mem[S_Mbid].Nominid; TSaveid2 = Clo_Mem[S_Mbid].Nominid2; TLine = Clo_Mem[S_Mbid].N_LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (L_1 + L_2 + L_3 + L_4 + L_5 + L_6 == 0) TSaveid = "**";
                    //if (LevelCnt == 1) TSaveid = "**";

                } //While


            }


            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }


        private void Give_Allowance10_2017_0731(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            pg1.Value = 0; pg1.Maximum = 8;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
            double F_TotalPV = 0, Sum_T_PV_01 = 0 ;


            StrSql = "Select Isnull(Sum(Se.TotalPV),0) AS DayPV From tbl_SalesDetail SE (nolock) ";
            StrSql = StrSql + " WHERE   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.SellCode <> ''";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";

            DataSet ds = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);

            Sum_T_PV_01 = double.Parse(ds.Tables[base_db_name].Rows[0][0].ToString());
            pg1.PerformStep(); pg1.Refresh();

            if (Sum_T_PV_01 == 0) return;

            int GradeCnt = 0;
            double Allowance1 = 0;


            GradeCnt = 0; Allowance1 = 0;
            StrSql = "Select Count(Mbid) AS DayPV From tbl_ClosePay_01 ";
            StrSql = StrSql + " Where OrgGrade =  40 ";
            StrSql = StrSql + " And   StopDate = '' ";
            StrSql = StrSql + " And   LeaveDate = '' ";
            StrSql = StrSql + " And   Sell_Mem_TF = 0 ";
            StrSql = StrSql + " And   ReqTF2 >= 1 "; 

            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);

            while (sr.Read())
            {
                GradeCnt = int.Parse(sr.GetValue(0).ToString());
            }
            sr.Close(); sr.Dispose();

            if (GradeCnt > 0)
            {
                Allowance1 = (Sum_T_PV_01 * 0.05) / GradeCnt;
                Allowance1 = Allowance1 - (Allowance1 % 10); //2017-02*-17 원단위 절사를 한후에 지급한다.

                StrSql = "Update tbl_ClosePay_01 Set";
                StrSql = StrSql + "  Allowance10 = " + Allowance1;
                StrSql = StrSql + " Where OrgGrade =  40 ";
                StrSql = StrSql + " And   StopDate = '' ";
                StrSql = StrSql + " And   LeaveDate = '' ";
                StrSql = StrSql + " And   Sell_Mem_TF = 0 ";
                StrSql = StrSql + " And   ReqTF2 >= 1 "; 

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            pg1.PerformStep(); pg1.Refresh();
            //----------------------------------------------------------------------------------------------------------




            GradeCnt = 0; Allowance1 = 0;
            StrSql = "Select Count(Mbid) AS DayPV From tbl_ClosePay_01 ";
            StrSql = StrSql + " Where OrgGrade = 50 ";
            StrSql = StrSql + " And   StopDate = '' ";
            StrSql = StrSql + " And   LeaveDate = '' ";
            StrSql = StrSql + " And   Sell_Mem_TF = 0 ";

            SqlDataReader sr2 = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr2);

            while (sr2.Read())
            {
                GradeCnt = int.Parse(sr2.GetValue(0).ToString());
            }
            sr2.Close(); sr2.Dispose();

            if (GradeCnt > 0)
            {
                Allowance1 = (Sum_T_PV_01 * 0.03) / GradeCnt;
                Allowance1 = Allowance1 - (Allowance1 % 10); //2017-02*-17 원단위 절사를 한후에 지급한다.

                StrSql = "Update tbl_ClosePay_01 Set";
                StrSql = StrSql + "  Allowance11 = " + Allowance1;
                StrSql = StrSql + " Where OrgGrade = 50 ";
                StrSql = StrSql + " And   StopDate = '' ";
                StrSql = StrSql + " And   LeaveDate = '' ";
                StrSql = StrSql + " And   Sell_Mem_TF = 0 ";
                StrSql = StrSql + " And   ReqTF2 >= 1 "; 

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            pg1.PerformStep(); pg1.Refresh();
            //----------------------------------------------------------------------------------------------------------


            GradeCnt = 0; Allowance1 = 0;
            StrSql = "Select Count(Mbid) AS DayPV From tbl_ClosePay_01 ";
            StrSql = StrSql + " Where OrgGrade = 60 ";
            StrSql = StrSql + " And   StopDate = '' ";
            StrSql = StrSql + " And   LeaveDate = '' ";
            StrSql = StrSql + " And   Sell_Mem_TF = 0 ";


            SqlDataReader sr3 = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr3);

            while (sr3.Read())
            {
                GradeCnt = int.Parse(sr3.GetValue(0).ToString());
            }
            sr3.Close(); sr3.Dispose();

            if (GradeCnt > 0)
            {
                Allowance1 = (Sum_T_PV_01 * 0.02) / GradeCnt;
                Allowance1 = Allowance1 - (Allowance1 % 10); //2017-02*-17 원단위 절사를 한후에 지급한다.

                StrSql = "Update tbl_ClosePay_01 Set";
                StrSql = StrSql + "  Allowance12 = " + Allowance1;
                StrSql = StrSql + " Where OrgGrade = 60 ";
                StrSql = StrSql + " And   StopDate = '' ";
                StrSql = StrSql + " And   LeaveDate = '' ";
                StrSql = StrSql + " And   Sell_Mem_TF = 0 ";
                StrSql = StrSql + " And   ReqTF2 >= 1 "; 

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            pg1.PerformStep(); pg1.Refresh();
            //----------------------------------------------------------------------------------------------------------





        }


        private void Give_Allowance9(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string Base_Mbid, int Base_Mbid2, string  Base_Name
               , double Base_TotalPV, string Base_Ordernumbe, int Base_Grade  )
        {
            
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0, Base_Level = 0 ;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double Allowance2 = 0, Allowance1 = 0, R_TotalPV = 0, TotalPV = 0, GivePay = 0;
            double L_1 = 0, L_2 = 0, L_3 = 0, L_4 = 0, L_5 = 0, L_6 = 0;
                        

            R_LevelCnt = 0;
            TotalPV = 0; 
            LevelCnt = 0; TSaveid = "**";

            Mbid = Base_Mbid;
            Mbid2 = Base_Mbid2; 
            M_Name = Base_Name ;

            S_Mbid = Mbid + "-" + Mbid2.ToString();

            TotalPV = Base_TotalPV;
            OrderNumber = Base_Ordernumbe ;

            S_Mbid = Mbid + "-" + Mbid2.ToString();

            TSaveid = Clo_Mem[S_Mbid].Nominid;
            TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
            TLine = Clo_Mem[S_Mbid].N_LineCnt;

            S_Mbid = TSaveid + "-" + TSaveid2.ToString();

            if (Base_Grade == 20)  Base_Level = 4 ; 
            if (Base_Grade == 30)  Base_Level = 3 ; 
            if (Base_Grade == 40)  Base_Level = 3 ; 
            if (Base_Grade == 50)  Base_Level = 3 ; 
            if (Base_Grade == 60)  Base_Level = 3 ; 


            while (TSaveid != "**" && Base_Level > 0 )
            {
                LevelCnt++;

                if (Clo_Mem.ContainsKey(S_Mbid) == true)
                {
                    //2017-05-29일 이사님 요청에 의해서 OrgGrade 에서 curgrade,  로 변경 처리함.
                    if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].CurGrade == Base_Grade  && Clo_Mem[S_Mbid].ReqTF2 >= 1)
                    {
                        Allowance1 = 0;
                        R_LevelCnt++;

                        if (Clo_Mem[S_Mbid].CurGrade == 20) Allowance1 = Base_TotalPV * (double.Parse(txtB14.Text) / 100);
                        if (Clo_Mem[S_Mbid].CurGrade == 30) Allowance1 = Base_TotalPV * (double.Parse(txtB15.Text) / 100);
                        if (Clo_Mem[S_Mbid].CurGrade == 40) Allowance1 = Base_TotalPV * (double.Parse(txtB16.Text) / 100);
                        if (Clo_Mem[S_Mbid].CurGrade == 50) Allowance1 = Base_TotalPV * (double.Parse(txtB17.Text) / 100);
                        if (Clo_Mem[S_Mbid].CurGrade == 60) Allowance1 = Base_TotalPV * (double.Parse(txtB18.Text) / 100);
                        
                        if (Allowance1 > 0)
                        {
                        

                            StrSql = "Update tbl_ClosePay_01 SET ";
                            StrSql = StrSql + " Allowance9 = Allowance9 +  " + Allowance1;
                            StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                            StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                            Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                


                            StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                            StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                            StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                            StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                            StrSql = StrSql + "Values(";
                            StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                            StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                            StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                            StrSql = StrSql + Allowance1 + " ," + Base_TotalPV + "," + LevelCnt + " ," + TLine;
                            StrSql = StrSql + ",'9' ,'" + OrderNumber + "')";

                            Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                
                        }


                    }

                    TSaveid = Clo_Mem[S_Mbid].Nominid; TSaveid2 = Clo_Mem[S_Mbid].Nominid2; TLine = Clo_Mem[S_Mbid].N_LineCnt;

                    S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                }
                else
                {
                    TSaveid = "**";
                }

                if (R_LevelCnt >= Base_Level ) TSaveid = "**";
                //if (LevelCnt == 1) TSaveid = "**";

            } //While

            

        }



        private void Give_Allowance1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0;
            string F_OrderN = ""; 

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            //우대회원의 매출에 대해서는 상위 추천인 한태 준다. 우대회원의 매출을 상위 ibo로 잡지는 않는다.
            StrSql = " Select Se.TotalPrice  TotalPV   , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2 ";

            if (int.Parse (FromEndDate ) >= 20160725 )
                StrSql = StrSql + " ,(Select Top 1 SellDate From tbl_SalesDetail (nolock) Where  tbl_SalesDetail.Mbid = Se.Mbid And tbl_SalesDetail.Mbid2 = Se.Mbid2  And Ga_Order = 0 Order by SellDate ASC ) AS F_OrderN";
            else
                StrSql = StrSql + " ,(Select Top 1 OrderNumber From tbl_SalesDetail (nolock) Where  tbl_SalesDetail.Mbid = Se.Mbid And tbl_SalesDetail.Mbid2 = Se.Mbid2  And Ga_Order = 0 Order by OrderNumber ASC ) AS F_OrderN";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";            
            StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_01 Cl2 (nolock) ON Cl2.Mbid = Se.Mbid And Cl2.Mbid2 = Se.Mbid2 ";

            StrSql = StrSql + " WHERE Se.TotalPrice  > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";            
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Cl2.RBO_Mem_TF = 0 "; //RBO에 대해서 지급한다 본인한태.            
            StrSql = StrSql + " And   Se.SellCode = '01' "; 
            StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2  ASC ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());  //+ double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                F_OrderN = ds.Tables[base_db_name].Rows[fi_cnt]["F_OrderN"].ToString(); //첫주문는 추천인 한태 직판 수당을 준다 본인이 가져가는게 없다.

                //if (Mbid2 == 112924)
                //    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                if (F_OrderN == OrderNumber)  //첫주문는 추천인이 직판수당을 가져간다.
                {
                    TSaveid = Clo_Mem[S_Mbid].Nominid ;
                    TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;
                }
                else
                {
                    TSaveid = Mbid;
                    TSaveid2 = Mbid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;
                }
                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {

                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == ""                            
                            && Clo_Mem[S_Mbid].RBO_Mem_TF == 0
                            )
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;
                            Allowance1 = R_TotalPV * 0.175;
                            
                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_01 SET ";
                                StrSql = StrSql + " Allowance1 = Allowance1 +  " + Allowance1;                                
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'1' ,'" + OrderNumber + "'," + R_LevelCnt + ")";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }


                        }

                        TSaveid = Clo_Mem[S_Mbid].Nominid; TSaveid2 = Clo_Mem[S_Mbid].Nominid2; TLine = Clo_Mem[S_Mbid].N_LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (LevelCnt == 1) TSaveid = "**";

                } //While

            }



            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }



        private void Give_Allowance1_20160725(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0;
            string F_OrderN = "";

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            //우대회원의 매출에 대해서는 상위 추천인 한태 준다. 우대회원의 매출을 상위 ibo로 잡지는 않는다.
            StrSql = " Select Se.TotalPrice  TotalPV   , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2 ";

            StrSql = StrSql + " ,(Select Top 1 SellDate From tbl_SalesDetail (nolock) Where  tbl_SalesDetail.Mbid = Se.Mbid And tbl_SalesDetail.Mbid2 = Se.Mbid2  And Ga_Order = 0 Order by SellDate ASC ) AS F_OrderN";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_01 Cl2 (nolock) ON Cl2.Mbid = Se.Mbid And Cl2.Mbid2 = Se.Mbid2 ";

            StrSql = StrSql + " WHERE Se.TotalPrice  > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Cl2.RBO_Mem_TF = 0 "; //RBO에 대해서 지급한다 본인한태.            
            StrSql = StrSql + " And   Se.SellCode = '01' ";
            StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2  ASC ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());  //+ double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                F_OrderN = ds.Tables[base_db_name].Rows[fi_cnt]["F_OrderN"].ToString(); //첫주문는 추천인 한태 직판 수당을 준다 본인이 가져가는게 없다.

                //if (Mbid2 == 112924)
                //    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                if (F_OrderN == SellDate)  //첫주문관련 일자는 추천인이 직판수당을 가져간다.
                {
                    TSaveid = Clo_Mem[S_Mbid].Nominid;
                    TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;
                }
                else
                {
                    TSaveid = Mbid;
                    TSaveid2 = Mbid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;
                }
                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {

                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == ""
                            && Clo_Mem[S_Mbid].RBO_Mem_TF == 0 && Clo_Mem[S_Mbid].Sell_Mem_TF  == 0
                            )
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;
                            Allowance1 = R_TotalPV * 0.175;

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_01 SET ";
                                StrSql = StrSql + " Allowance1 = Allowance1 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'1' ,'" + OrderNumber + "'," + R_LevelCnt + ")";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }


                        }

                        TSaveid = Clo_Mem[S_Mbid].Nominid; TSaveid2 = Clo_Mem[S_Mbid].Nominid2; TLine = Clo_Mem[S_Mbid].N_LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (LevelCnt == 1) TSaveid = "**";

                } //While

            }



            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }


        private void Give_Allowance3(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            //우대회원의 매출에 대해서는 상위 추천인 한태 준다. 우대회원의 매출을 상위 ibo로 잡지는 않는다.
            StrSql = " Select Se.TotalPrice  TotalPV   , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2 ";

            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_01 Cl2 (nolock) ON Cl2.Mbid = Se.Mbid And Cl2.Mbid2 = Se.Mbid2 ";

            StrSql = StrSql + " WHERE Se.TotalPrice  > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Cl2.RBO_Mem_TF = 0 "; //RBO에 대해서 지급한다 본인한태.            
            StrSql = StrSql + " And   Se.SellCode = '02' ";
            StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2  ASC ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());  //+ double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                if (Mbid2 == 112924)
                    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                TSaveid = Mbid;
                TSaveid2 = Mbid2;
                TLine = Clo_Mem[S_Mbid].N_LineCnt;

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {

                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == ""
                            && Clo_Mem[S_Mbid].RBO_Mem_TF == 0 && Clo_Mem[S_Mbid].Sell_Mem_TF == 0
                            )
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;
                            Allowance1 = R_TotalPV * 0.2;

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_01 SET ";
                                StrSql = StrSql + " Allowance3 = Allowance3 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'3' ,'" + OrderNumber + "'," + R_LevelCnt + ")";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }


                        }

                        TSaveid = Clo_Mem[S_Mbid].Nominid; TSaveid2 = Clo_Mem[S_Mbid].Nominid2; TLine = Clo_Mem[S_Mbid].N_LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (LevelCnt == 1) TSaveid = "**";

                } //While

            }



            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }




        private void Give_Allowance1_Begin(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0, Allowance1_M= 0 ;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            //우대회원의 매출에 대해서는 상위 추천인 한태 준다. 우대회원의 매출을 상위 ibo로 잡지는 않는다.
            StrSql = " Select (Se.TotalPrice  - Se.InputMile) TotalPV   , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2 ";
            StrSql = StrSql + " ,(Select Top 1 OrderNumber From tbl_SalesDetail (nolock) Where  tbl_SalesDetail.Mbid = Se.Mbid And tbl_SalesDetail.Mbid2 = Se.Mbid2  And Ga_Order = 0 Order by OrderNumber ASC ) AS F_OrderN";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_01 Cl2 (nolock) ON Cl2.Mbid = Se.Mbid And Cl2.Mbid2 = Se.Mbid2 ";

            StrSql = StrSql + " WHERE Se.TotalPrice   - Se.InputMile  > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Cl2.RBO_Mem_TF = 1 "; //비긴즈의 매출에 대해서 지급한다 본인한태.            

            //StrSql = StrSql + " And   OrderNumber not in  " ;  //최초주문은 빼버린다. 최초주문중에 반품 처리 된거는 제외한다.
            //StrSql = StrSql + "     (" ;
            //StrSql = StrSql + "      Select Top 1 OrderNumber From tbl_SalesDetail (nolock)  Where Ga_Order= 0 And ReturnTF =1 And tbl_SalesDetail.Mbid = Se.Mbid And tbl_SalesDetail.Mbid2 = Se.Mbid2 "; 
            //StrSql = StrSql + "      And OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock)  Where Re_BaseOrderNumber <> '' ) " ; 
            //StrSql = StrSql + "      Order by RecordTime ASC"; 
            //StrSql = StrSql + "      ) "; 

            StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2  ASC ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());  //+ double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                if (Mbid2 == 112924)
                    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                string F_OrderN = ds.Tables[base_db_name].Rows[fi_cnt]["F_OrderN"].ToString(); //첫주문는 추천인 한태 직판 수당을 준다 본인이 가져가는게 없다.

                //if (Mbid2 == 112924)
                //    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                if (F_OrderN == OrderNumber)  //첫주문는 추천인이 직판수당을 가져간다.
                {
                    TSaveid = Clo_Mem[S_Mbid].Nominid;
                    TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;
                }
                else
                {
                    TSaveid = Mbid;
                    TSaveid2 = Mbid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;
                }

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {

                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" )                            
                        {
                            Allowance1 = 0;

                            if (F_OrderN == OrderNumber)  //첫주문는 추천인이 직판수당을 가져간다.
                            {
                                Allowance1 = R_TotalPV * 0.175;  //추천인한태 주는 수당에 대해선 17.5  본인일 경우에는 5%
                            }
                            else
                            {
                                Allowance1 = R_TotalPV * 0.05;
                            }

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_01 SET ";
                                StrSql = StrSql + " Allowance5 = Allowance5 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'5' ,'" + OrderNumber + "'," + R_LevelCnt + ")";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }

                            


                        }

                        TSaveid = Clo_Mem[S_Mbid].Nominid; TSaveid2 = Clo_Mem[S_Mbid].Nominid2; TLine = Clo_Mem[S_Mbid].N_LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (LevelCnt == 1) TSaveid = "**";

                } //While

            }



            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }



        private void Give_Allowance1_Begin_20160725(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0, Allowance1_M = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            //우대회원의 매출에 대해서는 상위 추천인 한태 준다. 우대회원의 매출을 상위 ibo로 잡지는 않는다.
            StrSql = " Select (Se.TotalPrice  - Se.InputMile) TotalPV   , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2, Cl2.Sell_Mem_TF  ";
            //StrSql = StrSql + " ,(Select Top 1 OrderNumber From tbl_SalesDetail (nolock) Where  tbl_SalesDetail.Mbid = Se.Mbid And tbl_SalesDetail.Mbid2 = Se.Mbid2  And Ga_Order = 0 Order by OrderNumber ASC ) AS F_OrderN";
            StrSql = StrSql + " ,(Select Top 1 SellDAte From tbl_SalesDetail (nolock) Where  tbl_SalesDetail.Mbid = Se.Mbid And tbl_SalesDetail.Mbid2 = Se.Mbid2  And Ga_Order = 0 Order by SellDAte ASC ) AS F_OrderN";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_01 Cl2 (nolock) ON Cl2.Mbid = Se.Mbid And Cl2.Mbid2 = Se.Mbid2 ";

            StrSql = StrSql + " WHERE Se.TotalPrice   - Se.InputMile  > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Cl2.RBO_Mem_TF = 1 "; //비긴즈의 매출에 대해서 지급한다 본인한태.            

            //StrSql = StrSql + " And   OrderNumber not in  " ;  //최초주문은 빼버린다. 최초주문중에 반품 처리 된거는 제외한다.
            //StrSql = StrSql + "     (" ;
            //StrSql = StrSql + "      Select Top 1 OrderNumber From tbl_SalesDetail (nolock)  Where Ga_Order= 0 And ReturnTF =1 And tbl_SalesDetail.Mbid = Se.Mbid And tbl_SalesDetail.Mbid2 = Se.Mbid2 "; 
            //StrSql = StrSql + "      And OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock)  Where Re_BaseOrderNumber <> '' ) " ; 
            //StrSql = StrSql + "      Order by RecordTime ASC"; 
            //StrSql = StrSql + "      ) "; 

            StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2  ASC ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();
                int Sell_Mem_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sell_Mem_TF"].ToString());

                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());  //+ double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                if (Mbid2 == 112924)
                    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                string F_OrderN = ds.Tables[base_db_name].Rows[fi_cnt]["F_OrderN"].ToString(); //첫주문는 추천인 한태 직판 수당을 준다 본인이 가져가는게 없다.

                //if (Mbid2 == 112924)
                //    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                if (F_OrderN == SellDate  )  //첫주문는 추천인이 직판수당을 가져간다.   또는 소비자가 친 매출인 경우에는
                {
                    TSaveid = Clo_Mem[S_Mbid].Nominid;
                    TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;
                }
                else
                {
                    TSaveid = Mbid;
                    TSaveid2 = Mbid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;
                }

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {

                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "")
                        {
                            Allowance1 = 0;

                            if (F_OrderN == SellDate)  //첫주문는 추천인이 직판수당을 가져간다.
                            {                                
                                Allowance1 = R_TotalPV * 0.175;  //추천인한태 주는 수당에 대해선 17.5  본인일 경우에는 5%
                            }                           
                            else
                            {
                                Allowance1 = R_TotalPV * 0.05;
                            }

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_01 SET ";
                                StrSql = StrSql + " Allowance5 = Allowance5 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'5' ,'" + OrderNumber + "'," + R_LevelCnt + ")";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }




                        }

                        TSaveid = Clo_Mem[S_Mbid].Nominid; TSaveid2 = Clo_Mem[S_Mbid].Nominid2; TLine = Clo_Mem[S_Mbid].N_LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (LevelCnt == 1) TSaveid = "**";

                } //While

            }



            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }




        private void Give_Allowance1_Begin_20160816(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0, Allowance1_M = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            //우대회원의 매출에 대해서는 상위 추천인 한태 준다. 우대회원의 매출을 상위 ibo로 잡지는 않는다.
            StrSql = " Select (Se.TotalPrice  - Se.InputMile) TotalPV   , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2, Cl2.Sell_Mem_TF  ";
            //StrSql = StrSql + " ,(Select Top 1 OrderNumber From tbl_SalesDetail (nolock) Where  tbl_SalesDetail.Mbid = Se.Mbid And tbl_SalesDetail.Mbid2 = Se.Mbid2  And Ga_Order = 0 Order by OrderNumber ASC ) AS F_OrderN";
            StrSql = StrSql + " ,(Select Top 1 SellDAte From tbl_SalesDetail (nolock) Where  tbl_SalesDetail.Mbid = Se.Mbid And tbl_SalesDetail.Mbid2 = Se.Mbid2  And Ga_Order = 0 Order by SellDAte ASC ) AS F_OrderN";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_01 Cl2 (nolock) ON Cl2.Mbid = Se.Mbid And Cl2.Mbid2 = Se.Mbid2 ";

            StrSql = StrSql + " WHERE Se.TotalPrice   - Se.InputMile  > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Cl2.RBO_Mem_TF = 1 "; //비긴즈의 매출에 대해서 지급한다 본인한태.            

            //StrSql = StrSql + " And   OrderNumber not in  " ;  //최초주문은 빼버린다. 최초주문중에 반품 처리 된거는 제외한다.
            //StrSql = StrSql + "     (" ;
            //StrSql = StrSql + "      Select Top 1 OrderNumber From tbl_SalesDetail (nolock)  Where Ga_Order= 0 And ReturnTF =1 And tbl_SalesDetail.Mbid = Se.Mbid And tbl_SalesDetail.Mbid2 = Se.Mbid2 "; 
            //StrSql = StrSql + "      And OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock)  Where Re_BaseOrderNumber <> '' ) " ; 
            //StrSql = StrSql + "      Order by RecordTime ASC"; 
            //StrSql = StrSql + "      ) "; 

            StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2  ASC ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();
                int Sell_Mem_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sell_Mem_TF"].ToString());

                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());  //+ double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                if (Mbid2 == 112924)
                    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                string F_OrderN = ds.Tables[base_db_name].Rows[fi_cnt]["F_OrderN"].ToString(); //첫주문는 추천인 한태 직판 수당을 준다 본인이 가져가는게 없다.

                //if (Mbid2 == 112924)
                //    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                if (F_OrderN == SellDate || Sell_Mem_TF == 1)  //첫주문는 추천인이 직판수당을 가져간다.   또는 소비자가 친 매출인 경우에는
                {
                    TSaveid = Clo_Mem[S_Mbid].Nominid;
                    TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;
                }
                else
                {
                    TSaveid = Mbid;
                    TSaveid2 = Mbid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;
                }

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {

                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].Sell_Mem_TF == 0)
                        {
                            Allowance1 = 0;

                            if (F_OrderN == SellDate )  //첫주문는 추천인이 직판수당을 가져간다.
                            {
                                if ( Clo_Mem[S_Mbid].RBO_Mem_TF == 0 )
                                    Allowance1 = R_TotalPV * 0.175;  //추천인한태 주는 수당에 대해선 17.5  본인일 경우에는 5%                                

                            }
                            //2016-8-22 이사님 요청에 의해서
                            else if (Sell_Mem_TF == 1 && Clo_Mem[S_Mbid].RBO_Mem_TF == 0)  //주는 사람은 소비자이구 추천인은 RBO 이다 그럼  5프로 추천인 한태 준다. 본인이 가져가는게 없음.
                            {
                                Allowance1 = R_TotalPV * 0.05;
                            }
                            else
                            {
                                Allowance1 = R_TotalPV * 0.05;
                            }

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_01 SET ";
                                StrSql = StrSql + " Allowance5 = Allowance5 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'5' ,'" + OrderNumber + "'," + R_LevelCnt + ")";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }




                        }

                        TSaveid = Clo_Mem[S_Mbid].Nominid; TSaveid2 = Clo_Mem[S_Mbid].Nominid2; TLine = Clo_Mem[S_Mbid].N_LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (LevelCnt == 1) TSaveid = "**";

                } //While

            }



            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }

        private void Give_Allowance2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            //우대회원의 매출에 대해서는 상위 추천인 한태 준다. 우대회원의 매출을 상위 ibo로 잡지는 않는다.
            StrSql = " Select Se.TotalPrice  TotalPV   , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_01 Cl2 (nolock) ON Cl2.Mbid = Se.Mbid And Cl2.Mbid2 = Se.Mbid2 ";

            StrSql = StrSql + " WHERE Se.TotalPrice  > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Cl2.RBO_Mem_TF = 0 "; //RBO에 대해서 지급한다 본인한태.  
            StrSql = StrSql + " And   Se.SellCode = '01' "; 
            StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2  ASC ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());  //+ double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                if (Mbid2 == 112924)
                    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                TSaveid = Clo_Mem[S_Mbid].Nominid;
                TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                TLine = Clo_Mem[S_Mbid].N_LineCnt;

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {

                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == ""
                            && Clo_Mem[S_Mbid].RBO_Mem_TF == 0 && Clo_Mem[S_Mbid].Sell_Mem_TF == 0
                            )
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;
                            Allowance1 = R_TotalPV * 0.035;

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_01 SET ";
                                StrSql = StrSql + " Allowance2 = Allowance2 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'2' ,'" + OrderNumber + "'," + R_LevelCnt + ")";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }


                        }

                        TSaveid = Clo_Mem[S_Mbid].Nominid; TSaveid2 = Clo_Mem[S_Mbid].Nominid2; TLine = Clo_Mem[S_Mbid].N_LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (LevelCnt == 1) TSaveid = "**";

                } //While

            }



            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }

        private void Give_Allowance4(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            //우대회원의 매출에 대해서는 상위 추천인 한태 준다. 우대회원의 매출을 상위 ibo로 잡지는 않는다.
            StrSql = " Select Se.TotalPrice  TotalPV   , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_01 Cl2 (nolock) ON Cl2.Mbid = Se.Mbid And Cl2.Mbid2 = Se.Mbid2 ";

            StrSql = StrSql + " WHERE Se.TotalPrice  > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Cl2.RBO_Mem_TF = 0 "; //RBO에 대해서 지급한다 본인한태.  
            StrSql = StrSql + " And   Se.SellCode = '02' ";
            StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2  ASC ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());  //+ double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                if (Mbid2 == 112924)
                    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                TSaveid = Clo_Mem[S_Mbid].Nominid;
                TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                TLine = Clo_Mem[S_Mbid].N_LineCnt;

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {

                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == ""
                            && Clo_Mem[S_Mbid].RBO_Mem_TF == 0 && Clo_Mem[S_Mbid].Sell_Mem_TF == 0
                            )
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;
                            Allowance1 = R_TotalPV * 0.05;

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_01 SET ";
                                StrSql = StrSql + " Allowance4 = Allowance4 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'4' ,'" + OrderNumber + "'," + R_LevelCnt + ")";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }


                        }

                        TSaveid = Clo_Mem[S_Mbid].Nominid; TSaveid2 = Clo_Mem[S_Mbid].Nominid2; TLine = Clo_Mem[S_Mbid].N_LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (LevelCnt == 1) TSaveid = "**";

                } //While

            }



            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }


        private void Give_Allowance2_Begin(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            //우대회원의 매출에 대해서는 상위 추천인 한태 준다. 우대회원의 매출을 상위 ibo로 잡지는 않는다.
            StrSql = " Select (Se.TotalPrice  - Se.InputMile) TotalPV   , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_01 Cl2 (nolock) ON Cl2.Mbid = Se.Mbid And Cl2.Mbid2 = Se.Mbid2 ";

            StrSql = StrSql + " WHERE Se.TotalPrice   - Se.InputMile  > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Cl2.RBO_Mem_TF = 1 "; //비긴즈의 매출에 대해서 지급한다 본인한태.            

            //StrSql = StrSql + " And   OrderNumber not in  ";  //최초주문은 빼버린다. 최초주문중에 반품 처리 된거는 제외한다.
            //StrSql = StrSql + "     (";
            //StrSql = StrSql + "      Select Top 1 OrderNumber From tbl_SalesDetail (nolock)  Where Ga_Order= 0 And ReturnTF =1 And tbl_SalesDetail.Mbid = Se.Mbid And tbl_SalesDetail.Mbid2 = Se.Mbid2 ";
            //StrSql = StrSql + "      And OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock)  Where Re_BaseOrderNumber <> '' ) ";
            //StrSql = StrSql + "      Order by RecordTime ASC";
            //StrSql = StrSql + "      ) ";

            StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2  ASC ";


            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());  //+ double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                if (Mbid2 == 112924)
                    Mbid = Mbid;


                S_Mbid = Mbid + "-" + Mbid2.ToString();

                TSaveid = Clo_Mem[S_Mbid].Nominid;
                TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                TLine = Clo_Mem[S_Mbid].N_LineCnt;

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {

                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == ""
                            && Clo_Mem[S_Mbid].RBO_Mem_TF == 0 && Clo_Mem[S_Mbid].Sell_Mem_TF == 0
                            )
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;
                            Allowance1 = R_TotalPV * 0.1;  //비긴즈 관련 매출은 추천인 한태 무조건 10% 간다.

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_01 SET ";
                                StrSql = StrSql + " Allowance2 = Allowance2 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'2' ,'" + OrderNumber + "'," + R_LevelCnt + ")";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }


                        }

                        TSaveid = Clo_Mem[S_Mbid].Nominid; TSaveid2 = Clo_Mem[S_Mbid].Nominid2; TLine = Clo_Mem[S_Mbid].N_LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (R_LevelCnt == 1) TSaveid = "**";

                } //While

            }



            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }





        private void Give_Allowance2_TEST(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0, Big_line = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "";
            double Allowance2 = 0, Allowance1 = 0, Allowance3 = 0, Allowance4 = 0, Allowance5 = 0;
            double Allowance6 = 0, Allowance7 = 0, Allowance8 = 0, Allowance9 = 0, Allowance10 = 0;
            double Allowance11 = 0, Allowance12 = 0, Allowance13 = 0, Allowance14 = 0, Allowance15 = 0;
            double Sum_PV_1 = 0, Sum_PV_2 = 0, Ded_1 = 0, Ded_2 = 0 ;
            
            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = "  Select  Allowance2 ,  Nominid , Nominid2 , Mbid , Mbid2  ";
            StrSql = StrSql + " ,N_LineCnt , M_Name, N_LineCnt , LineCnt   , Sum_PV_1, Sum_PV_2   ";
            StrSql = StrSql + "  From tbl_ClosePay_01    ";
            StrSql = StrSql + " Where  Sum_PV_1  > 0 ";
            StrSql = StrSql + " And    Sum_PV_2  > 0 ";

            ReCnt = 0;
            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;
            
            pg1.Value = 0; pg1.Maximum = ReCnt + 1;            
            while (sr.Read())
            {
                LevelCnt = 0; Big_line = 0;
              
                TSaveid = sr.GetValue(1).ToString();
                TSaveid2 = int.Parse(sr.GetValue(2).ToString());
                TLine = int.Parse(sr.GetValue(5).ToString());
                Allowance2 = double.Parse(sr.GetValue(0).ToString());

                Mbid = sr.GetValue(3).ToString().ToString();
                Mbid2 = int.Parse(sr.GetValue(4).ToString().ToString());
                M_Name = sr.GetValue(6).ToString();

                Sum_PV_1 = double.Parse(sr.GetValue(9).ToString());
                Sum_PV_2 = double.Parse(sr.GetValue(10).ToString());
                Ded_1 = 0; Ded_2 = 0;

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                Allowance1 = 0; Allowance2 = 0; Allowance3 = 0; Allowance4 = 0; Allowance5 = 0;
                Allowance6 = 0; Allowance7 = 0; Allowance8 = 0; Allowance9 = 0; Allowance10 = 0;
                Allowance11 = 0; Allowance12 = 0; Allowance13 = 0; Allowance14 = 0; Allowance15 = 0;

                if (Sum_PV_1 > Sum_PV_2)
                {
                    Allowance1 = (Sum_PV_2) * 0.1;

                    if (Sum_PV_1 >= (Sum_PV_2 * 2))
                    {
                        Allowance2 = (Sum_PV_2 * 2) * 0.05;
                        Allowance6 = (Sum_PV_2 * 2) * 0.1;
                        Allowance10 = (Sum_PV_2 * 2) * 0.15;
                    }
                    else
                    {
                        Allowance2 = Sum_PV_1 * 0.05;
                        Allowance6 = Sum_PV_1 * 0.1;
                        Allowance10 = Sum_PV_1 * 0.15;
                    }


                    if (Sum_PV_1 >= (Sum_PV_2 * 3))
                    {
                        Allowance3 = (Sum_PV_2 * 3) * 0.05;
                        Allowance7 = (Sum_PV_2 * 3) * 0.1;
                        Allowance11 = (Sum_PV_2 * 3) * 0.15;
                    }
                    else
                    {
                        Allowance3 = Sum_PV_1 * 0.05;
                        Allowance7 = Sum_PV_1 * 0.1;
                        Allowance11 = Sum_PV_1 * 0.15;
                    }

                    if (Sum_PV_1 >= (Sum_PV_2 * 4))
                    {
                        Allowance4 = (Sum_PV_2 * 4) * 0.05;
                        Allowance8 = (Sum_PV_2 * 4) * 0.1;
                        Allowance12 = (Sum_PV_2 * 4) * 0.15;
                    }
                    else
                    {
                        Allowance4 = Sum_PV_1 * 0.05;
                        Allowance8 = Sum_PV_1 * 0.1;
                        Allowance12 = Sum_PV_1 * 0.15;
                    }

                    if (Sum_PV_1 >= (Sum_PV_2 * 5))
                    {
                        Allowance5 = (Sum_PV_2 * 5) * 0.05;
                        Allowance9 = (Sum_PV_2 * 5) * 0.1;
                        Allowance13 = (Sum_PV_2 * 5) * 0.15;
                    }
                    else
                    {
                        Allowance5 = Sum_PV_1 * 0.05;
                        Allowance9 = Sum_PV_1 * 0.1;
                        Allowance13 = Sum_PV_1 * 0.15;
                    }                    
                }
                else
                {
                    Allowance1 = (Sum_PV_1) * 0.1;


                    if (Sum_PV_2 >= (Sum_PV_1 * 2))
                    {
                        Allowance2 = (Sum_PV_1 * 2) * 0.05;
                        Allowance6 = (Sum_PV_1 * 2) * 0.1;
                        Allowance10 = (Sum_PV_1 * 2) * 0.15;
                    }
                    else
                    {
                        Allowance2 = Sum_PV_2 * 0.05;
                        Allowance6 = Sum_PV_2 * 0.1;
                        Allowance10 = Sum_PV_2 * 0.15;
                    }


                    if (Sum_PV_2 >= (Sum_PV_1 * 3))
                    {
                        Allowance3 = (Sum_PV_1 * 3) * 0.05;
                        Allowance7 = (Sum_PV_1 * 3) * 0.1;
                        Allowance11 = (Sum_PV_1 * 3) * 0.15;
                    }
                    else
                    {
                        Allowance3 = Sum_PV_2 * 0.05;
                        Allowance7 = Sum_PV_2 * 0.1;
                        Allowance11 = Sum_PV_2 * 0.15;
                    }

                    if (Sum_PV_2 >= (Sum_PV_1 * 4))
                    {
                        Allowance4 = (Sum_PV_1 * 4) * 0.05;
                        Allowance8 = (Sum_PV_1 * 4) * 0.1;
                        Allowance12 = (Sum_PV_1 * 4) * 0.15;
                    }
                    else
                    {
                        Allowance4 = Sum_PV_2 * 0.05;
                        Allowance8 = Sum_PV_2 * 0.1;
                        Allowance12 = Sum_PV_2 * 0.15;
                    }

                    if (Sum_PV_2 >= (Sum_PV_1 * 5))
                    {
                        Allowance5 = (Sum_PV_1 * 5) * 0.05;
                        Allowance9 = (Sum_PV_1 * 5) * 0.1;
                        Allowance13 = (Sum_PV_1 * 5) * 0.15;
                    }
                    else
                    {
                        Allowance5 = Sum_PV_2 * 0.05;
                        Allowance9 = Sum_PV_2 * 0.1;
                        Allowance13 = Sum_PV_2 * 0.15;
                    }           


                }


                 


                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " Allowance1 = Allowance1 +  " + Allowance1;
                StrSql = StrSql + " ,Allowance2 = Allowance2 +  " + Allowance2;
                StrSql = StrSql + " ,Allowance3 = Allowance3 +  " + Allowance3;
                StrSql = StrSql + " ,Allowance4 = Allowance4 +  " + Allowance4;
                StrSql = StrSql + " ,Allowance5 = Allowance5 +  " + Allowance5;
                StrSql = StrSql + " ,Allowance6 = Allowance6 +  " + Allowance6;
                StrSql = StrSql + " ,Allowance7 = Allowance7 +  " + Allowance7;
                StrSql = StrSql + " ,Allowance8 = Allowance8 +  " + Allowance8;
                StrSql = StrSql + " ,Allowance9 = Allowance9 +  " + Allowance9;
                StrSql = StrSql + " ,Allowance10 = Allowance10 +  " + Allowance10;
                StrSql = StrSql + " ,Allowance11 = Allowance11 +  " + Allowance11;
                StrSql = StrSql + " ,Allowance12 = Allowance12 +  " + Allowance12;
                StrSql = StrSql + " ,Allowance13 = Allowance13 +  " + Allowance13;
                
                StrSql = StrSql + " ,Sum_PV_1 = 0 ";
                StrSql = StrSql + " ,Sum_PV_2 = 0 " ;
                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                
                t_qu[t_qu_Cnt] = StrSql;
                t_qu_Cnt++;
                                        

                pg1.PerformStep(); pg1.Refresh();
            }


            sr.Close(); sr.Dispose();

            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }



        }












        private void Give_Allowance3_BB(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "";
            double Allowance2 = 0, Allowance1 = 0;

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = "  Select  Allowance2 ,  Nominid , Nominid2 , Mbid , Mbid2 ";
            StrSql = StrSql + " ,N_LineCnt , M_Name, N_LineCnt , LineCnt   ";
            StrSql = StrSql + "  From tbl_ClosePay_01    ";
            StrSql = StrSql + " Where  Allowance2 > 0 ";
            
            ReCnt = 0;
            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;

            
            pg1.Value = 0; pg1.Maximum = ReCnt + 1;                         
            //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)            
            while(sr.Read ())
            {
                LevelCnt = 0;
                //TSaveid = ds.Tables[base_db_name].Rows[fi_cnt]["Nominid"].ToString();
                //TSaveid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Nominid2"].ToString());
                //TLine = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["N_LineCnt"].ToString());
                //Allowance2 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance2"].ToString());

                //Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                //Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                //M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                TSaveid = sr.GetValue(1).ToString();
                TSaveid2 = int.Parse(sr.GetValue(2).ToString());
                TLine = int.Parse(sr.GetValue(5).ToString());
                Allowance2 = double.Parse(sr.GetValue(0).ToString());

                Mbid = sr.GetValue(3).ToString().ToString();
                Mbid2 = int.Parse(sr.GetValue(4).ToString().ToString());
                M_Name = sr.GetValue(6).ToString();

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].CurGrade >= 30)                        
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;

                            if (LevelCnt == 1 && Clo_Mem[S_Mbid].CurGrade >= 30)
                            {
                                Allowance1 = (Allowance2) * 0.1;
                            }

                            if (LevelCnt == 1 && Clo_Mem[S_Mbid].CurGrade >= 40)
                            {
                                Allowance1 = Allowance2 * 0.1;
                            }

                            if (LevelCnt == 2 && Clo_Mem[S_Mbid].CurGrade >= 50)
                            {
                                Allowance1 = Allowance2 * 0.1;
                            }

                            if (LevelCnt == 2 && Clo_Mem[S_Mbid].CurGrade >= 60)
                            {
                                Allowance1 = Allowance2 * 0.1;
                            }

                            Allowance1 = (Allowance2) * 0.1;

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_01 SET ";
                                StrSql = StrSql + " Allowance3 = Allowance3 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_01";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV , GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + ", 0 , " + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'3' ,'')";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }

                        }

                        TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (LevelCnt == 2) TSaveid = "**";

                } //While


                pg1.PerformStep(); pg1.Refresh();
            }


            sr.Close(); sr.Dispose();

            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1 ; pg1.Refresh();            
            foreach (int tkey in t_qu.Keys )
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }


        }


        private void Put_Return_Pay(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();


            StrSql = " Select OrderNumber , Re_BaseOrderNumber ,  TotalPV, TotalPrice  , tbl_SalesDetail.Mbid , tbl_SalesDetail.Mbid2 , tbl_SalesDetail.M_Name , SellDate  ";            
            StrSql = StrSql + " From tbl_SalesDetail (nolock)   ";            
            StrSql = StrSql + " WHERE TotalPrice < 0   ";
            StrSql = StrSql + " And   SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   SellDate  <='" + ToEndDate + "'";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql,  base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            int  Mbid2 = 0 ;
            string Mbid = "", Re_BaseOrderNumber = "", OrderNumber = "", M_Name ="", SellDate = "";
            double Base_PV = 0, Return_Pay = 0, TotalPV = 0 ;
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                
                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                Re_BaseOrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["Re_BaseOrderNumber"].ToString();
                TotalPV = -double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();
                SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                StrSql = "SELECT  DownPV , SaveMbid, SaveMbid2, SaveName ,SortOrder " ;
                StrSql = StrSql + " From tbl_Close_DownPV_ALL_01  ";
                StrSql = StrSql + " WHERE RequestMbid = '" + Mbid + "'" ;
                StrSql = StrSql + " And   RequestMbid2 = " + Mbid2 ;
                StrSql = StrSql + " And   OrderNumber = '" + Re_BaseOrderNumber + "'" ;

                ReCnt = 0;
                SqlDataReader sr2 = null;
                Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr2);                
                int ReCnt2 = Temp_Connect.DataSet_ReCount;

                if (ReCnt2 > 0)
                {
                    Base_PV = 0;

                    StrSql = "SELECT  TotalPrice  From tbl_SalesDetail (nolock)  ";
                    StrSql = StrSql + " WHERE   OrderNumber = '" + Re_BaseOrderNumber + "'";

                    DataSet ds3 = new DataSet();
                    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds3);
                    int ReCnt3 = Search_Connect.DataSet_ReCount;

                    if (ReCnt3 > 0)
                        Base_PV = double.Parse(ds3.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());


                    if (Base_PV > 0)
                    {
                        //for (int fi_cnt2 = 0; fi_cnt2 <= ReCnt2 - 1; fi_cnt2++)
                        while (sr2.Read() )
                        {
                            Return_Pay = 0 ;
                            Return_Pay = double.Parse(sr2.GetValue (0).ToString()) * (TotalPV / Base_PV);

                            if (Return_Pay > 0)
                            {
                                 StrSql = " INSERT INTO tbl_Sales_Put_Return_Pay ";
                                StrSql = StrSql + " (ToEndDate,OrderNumber,Re_BaseOrderNumber,C_Mbid,C_Mbid2, C_M_Name ,R_Mbid,R_Mbid2, R_M_Name , SellDate , Return_Pay, Return_Pay2, Cl_TF )";
                                StrSql = StrSql + " Values(" ;
                                StrSql = StrSql + "'" + ToEndDate + "','" + OrderNumber + "'";
                                StrSql = StrSql + ",'" + Re_BaseOrderNumber  + "',";
                                StrSql = StrSql + "'" + sr2.GetValue (1).ToString()  + "'";
                                StrSql = StrSql + "," + int.Parse(sr2.GetValue(2).ToString()) + ",";
                                StrSql = StrSql + "'" + sr2.GetValue(3).ToString() + "'";
                                StrSql = StrSql + ",'" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + SellDate + "'," + Return_Pay + "," + Return_Pay + ",1";
                                StrSql = StrSql + ")" ;

                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);

                            }//if (Return_Pay > 0)

                        }//while (sr2.Read() )

                    }//if (Base_PV > 0)

                }//if (ReCnt2 > 0)
                sr2.Close(); sr2.Dispose();

                pg1.PerformStep(); pg1.Refresh();
            }


           

            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }


        private void Put_Sum_Return_Remain_Pay(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4   ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_01 SET " ;
            StrSql = StrSql + " Sum_Return_Take_Pay = ISNULL(B.A1, 0 )   " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, " ;
    
            StrSql = StrSql + " (Select    Sum(Return_Pay) A1,  C_Mbid ,C_Mbid2   " ;
            StrSql = StrSql + " From tbl_Sales_Put_Return_Pay " ;
            StrSql = StrSql + " Where   Return_Pay > 0 " ;
            StrSql = StrSql + " And     OrderNumber <> '' " ;
            StrSql = StrSql + " Group By C_Mbid ,C_Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid  = B.C_Mbid " ;
            StrSql = StrSql + " And   A.Mbid2  = B.C_Mbid2 " ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
    
            // '''--여태 까지 발생한 총 반품으로 인해서 발생된 차감액을 구한ㄷ4ㅏ.
            StrSql = "Update tbl_ClosePay_01 SET " ;
            StrSql = StrSql + " Sum_Return_DedCut_Pay = ISNULL(B.A1, 0 )   " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, " ;
    
            StrSql = StrSql + " (Select    Sum(Return_Pay) A1,  C_Mbid ,C_Mbid2   " ;
            StrSql = StrSql + " From tbl_Sales_Put_Return_Pay " ;
            StrSql = StrSql + " Where   Return_Pay > 0 " ;
            StrSql = StrSql + " And     OrderNumber = '' " ;
            StrSql = StrSql + " Group By C_Mbid ,C_Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid  = B.C_Mbid " ;
            StrSql = StrSql + " And   A.Mbid2  = B.C_Mbid2 " ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + " Sum_Return_Remain_Pay = Sum_Return_Take_Pay - Sum_Return_DedCut_Pay " ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }

        private void CalculateTruePayment(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 8    ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            //StrSql = "INSERT INTO tbl_Member_Mileage ";
            //StrSql = StrSql + "(T_Time,mbid,mbid2,M_Name, PlusValue ,PlusKind,Plus_OrderNumber,User_id, ETC1, ToEndDate, PayDate  )";
            //StrSql = StrSql + " Select  ";
            //StrSql = StrSql + " Convert(Varchar(25),GetDate(),120) , Mbid , Mbid2 , M_NAME ";
            //StrSql = StrSql + ",Allowance1_M,'51','A1','" + cls_User.gid + "','', '" + ToEndDate + "','" + ToEndDate + "'";
            //StrSql = StrSql + " From tbl_ClosePay_01 ";
            //StrSql = StrSql + " Where Allowance1_M > 0 ";


            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + " Etc_Pay = ISNULL(B.A1, 0 )   ";
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, ";

            StrSql = StrSql + " (Select    Sum(Apply_Pv) A1,  mbid ,mbid2   ";
            StrSql = StrSql + " From tbl_Sham_Pay (nolock) ";
            StrSql = StrSql + " Where   Apply_Date >='" + FromEndDate  + "'";
            StrSql = StrSql + " And     Apply_Date <= '" + ToEndDate  + "' ";
            StrSql = StrSql + " And     SortKind2 = '01' ";
            StrSql = StrSql + " Group By mbid ,mbid2 ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid  = B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2  = B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);




            StrSql = "Update tbl_ClosePay_01 Set "    ;
            StrSql = StrSql + " SumAllAllowance = Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6 + Allowance7 + Allowance8 + Allowance9 + Allowance10 + Allowance11 + Allowance12 +   Etc_Pay  ";
            StrSql = StrSql + " Where Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6 + Allowance7  + Allowance8 + Allowance9 + Allowance10 + Allowance11 + Allowance12  + Etc_Pay   > 0";
    
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    

            //'''---반품으로 해서 차감시킬 금액이 아직 남아잇다.
            StrSql = "Update tbl_ClosePay_01 Set "    ;
            StrSql = StrSql + " Cur_DedCut_Pay = SumAllAllowance "    ;
            StrSql = StrSql + ",SumAllAllowance = 0 "    ;
            StrSql = StrSql + " Where SumAllAllowance  > 0"    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay > 0 "    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay >= SumAllAllowance "    ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set "    ;
            StrSql = StrSql + " Cur_DedCut_Pay = Sum_Return_Remain_Pay "    ;
            StrSql = StrSql + ",SumAllAllowance = SumAllAllowance - Sum_Return_Remain_Pay "    ;
            StrSql = StrSql + " Where SumAllAllowance  > 0"    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay > 0 "    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay < SumAllAllowance "    ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " SumAllAllowance = Round((SumAllAllowance ) /10,0,1) * 10 ";            
            StrSql = StrSql + " Where SumAllAllowance > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " InComeTax = Round(((SumAllAllowance * 0.03) /10),0,1) * 10  ";
            StrSql = StrSql + " Where SumAllAllowance > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " ResidentTax = Round(((InComeTax * 0.1) /10),0,1) * 10  ";
            StrSql = StrSql + " Where SumAllAllowance > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + " TruePayment = ((SumAllAllowance - InComeTax - ResidentTax) / 10 ) * 10 ";
            StrSql = StrSql + " Where SumAllAllowance > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


        }


        private void Chang_RetunPay_Table(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select Cur_DedCut_Pay,Mbid,Mbid2 , M_Name";
            StrSql = StrSql + " From tbl_ClosePay_01    ";
            StrSql = StrSql + " WHERE Cur_DedCut_Pay > 0 ";

            ReCnt = 0;
            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;
                
            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            int Mbid2 = 0,  Top_SW = 0, T_Pay = 0, TSw = 0,  T_index = 0 ;
            string Mbid = "", Re_BaseOrderNumber = "", M_Name = "";
            double Cut_Pay = 0, RR_Cut_Pay = 0 ;
            
            //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            while (sr.Read ())
            {
                Cut_Pay = double.Parse(sr.GetValue(0).ToString());
                Mbid = sr.GetValue(1).ToString();
                Mbid2 = int.Parse(sr.GetValue(2).ToString());
                M_Name = sr.GetValue(3).ToString();


                StrSql = "Select Return_Pay2,C_Mbid,C_Mbid2, T_index , Re_BaseOrderNumber ";
                StrSql = StrSql + " From tbl_Sales_Put_Return_Pay (nolock) ";
                StrSql = StrSql + " WHERE C_Mbid = '" + Mbid + "'";
                StrSql = StrSql + " And   C_Mbid2 = " + Mbid2;
                StrSql = StrSql + " And   Return_Pay2 > 0 ";
                StrSql = StrSql + " And   Base_OrderNumber = '' " ;

                DataSet ds2 = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name,Search_Conn , ds2);
                int ReCnt2 = Search_Connect.DataSet_ReCount;

                if (ReCnt2 > 0)
                {
                    Top_SW = 0;
                    int fi_cnt2 = 0;
                    while ((fi_cnt2 <= ReCnt2 - 1) && Top_SW == 0 )
                    {
                        T_Pay = int.Parse(ds2.Tables[base_db_name].Rows[fi_cnt2]["Return_Pay2"].ToString());
                        Re_BaseOrderNumber = ds2.Tables[base_db_name].Rows[fi_cnt2]["Re_BaseOrderNumber"].ToString();
                        T_index = int.Parse(ds2.Tables[base_db_name].Rows[fi_cnt2]["T_index"].ToString());

                        TSw = 0;

                        while (Cut_Pay > 0 && TSw == 0)
                        {
                            RR_Cut_Pay = 0 ;
                        
                            if (Cut_Pay > T_Pay)
                            {
                                RR_Cut_Pay = T_Pay ;
                                Cut_Pay = Cut_Pay - T_Pay;
                                T_Pay = 0;
                                TSw = 1;
                            }
                            else
                            {
                                RR_Cut_Pay = Cut_Pay ;
                                Cut_Pay = 0;
                            }

                            if (RR_Cut_Pay > 0)
                            {
                                StrSql = "Insert into tbl_Sales_Put_Return_Pay (ToEndDate, C_mbid,C_mbid2 , C_M_Name , Return_Pay , Base_OrderNumber , Base_T_index , Cl_TF ) " ;
                                StrSql = StrSql + " Values (";
                                StrSql = StrSql + " '" + ToEndDate + "','" + Mbid + "', " + Mbid2 ;
                                StrSql = StrSql + " , '" + M_Name + "', " + RR_Cut_Pay + ",";
                                StrSql = StrSql + "'" + Re_BaseOrderNumber + "'";
                                StrSql = StrSql + "," + T_index + ",1";
                                StrSql = StrSql + ")";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;


                                StrSql = "Update tbl_Sales_Put_Return_Pay SET ";
                                StrSql = StrSql + " Return_Pay2 = Return_Pay2 -" + RR_Cut_Pay ;
                                StrSql = StrSql + " Where   T_index  = " + T_index ;
                                StrSql = StrSql + " And   C_Mbid = '" + Mbid + "'";
                                StrSql = StrSql + " And   C_Mbid2 = " + Mbid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }

                        }

                        if (Cut_Pay == 0) Top_SW = 1;
                        fi_cnt2++;
                    }
                }
                 

                pg1.PerformStep(); pg1.Refresh();
            }

            sr.Close(); sr.Dispose();

            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }






        private void tbl_CloseTotal_Put1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "INSERT INTO tbl_CloseTotal_01 ";
            StrSql = StrSql +  " (ToEndDate,      FromEndDate,   PayDate ,   PayDate2 " ;
            StrSql = StrSql +  " ,TotalSellAmount,TotalInputCash,TotalInputCard,TotalInputBank"  ;
            StrSql = StrSql +  " ,TotalSellPV,    TotalShamPV,   TotalReturnSellAmount"  ;
            StrSql = StrSql +  " ,TotalReturnInputCash, TotalReturnInputCard,TotalReturnInputBank, TotalReturnSellPV "  ;
            StrSql = StrSql +  " ,TotalSellCV,TotalReturnSellCV " ;
            StrSql = StrSql + " ,Temp01,Temp02, Temp03, Temp04, Temp05, Temp06 , Temp07, Temp08, Temp09, Temp10 ";
            StrSql = StrSql + " , Temp11, Temp12, Temp13, Temp14 , Temp15, Temp16 , Temp17, Temp18";
            StrSql = StrSql + " ,Temp20, Temp21, Temp22, Temp23, Temp24 , Temp25 " ;
            StrSql = StrSql + " , Temp26 , Temp27, Temp28, Temp29, Temp30, Temp31"; 
            StrSql = StrSql +  " ,RecordID,RecordTime "  ;
            StrSql = StrSql +  " ) "  ;
    
            StrSql = StrSql +  " Select "  ;
            StrSql = StrSql +  "'" + ToEndDate +  "','" +  FromEndDate +  "','" +  PayDate +  "','" +  PayDate2 +  "'" ;
            StrSql = StrSql +  ",Sum(DayAmount),Sum(DayCash),Sum(DayCard),Sum(DayBank)" ;
            StrSql = StrSql +  ",Sum(DayTotalPV),Sum(DayShamSell),Sum(DayReAmount)";
            StrSql = StrSql +  ",Sum(DayReCash),Sum(DayReCard),Sum(DayReBank),Sum(DayReTotalPV)";
            StrSql = StrSql +  ",Sum(DayTotalCV),Sum(DayReTotalCV) " ;
            StrSql = StrSql +  "," + double.Parse(txtB1.Text) + "," + double.Parse(txtB2.Text) + "," + double.Parse(txtB3.Text) + "," + double.Parse(txtB4.Text) + "," + double.Parse(txtB5.Text) ;
            StrSql = StrSql +  "," + double.Parse(txtB6.Text) + "," + double.Parse(txtB7.Text) + "," + double.Parse(txtB8.Text) + "," + double.Parse(txtB9.Text) + "," + double.Parse(txtB10.Text) ;
            StrSql = StrSql + "," + double.Parse(txtB11.Text) + "," + double.Parse(txtB12.Text) + "," + double.Parse(txtB13.Text) + "," + double.Parse(txtB14.Text) ;
            StrSql = StrSql + "," + double.Parse(txtB15.Text) + "," + double.Parse(txtB16.Text) + "," + double.Parse(txtB17.Text) + "," + double.Parse(txtB18.Text);

            StrSql = StrSql + "," + double.Parse(txtB20.Text) + "," + double.Parse(txtB21.Text) + "," + double.Parse(txtB22.Text) + "," + double.Parse(txtB23.Text) + "," + double.Parse(txtB24.Text);
            StrSql = StrSql + "," + double.Parse(txtB25.Text) + "," + double.Parse(txtB26.Text) + "," + double.Parse(txtB27.Text) + "," + double.Parse(txtB28.Text) + "," + double.Parse(txtB29.Text);
            StrSql = StrSql + "," + double.Parse(txtB30.Text) + "," + double.Parse(txtB31.Text);  

            StrSql = StrSql +  ",'" + cls_User.gid  + "',Convert(Varchar(25),GetDate(),21)" ;    
            StrSql = StrSql +  " From  tbl_ClosePay_01_Sell " ;
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }




        private void tbl_CloseTotal_Put2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_CloseTotal_01 SET " ;
            StrSql = StrSql + "  Allowance1 =ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,Allowance2 =ISNULL(B.A2,0) " ;
            StrSql = StrSql + " ,Allowance3 =ISNULL(B.A3,0) " ;
            StrSql = StrSql + " ,Allowance4 =ISNULL(B.A4,0) " ;
            StrSql = StrSql + " ,Allowance5 =ISNULL(B.A5,0) " ;
            StrSql = StrSql + " ,Allowance6 =ISNULL(B.A6,0) " ;
            StrSql = StrSql + " ,Allowance7 =ISNULL(B.A7,0) " ;
            StrSql = StrSql + " ,Allowance8 =ISNULL(B.A8,0) ";
            StrSql = StrSql + " ,Allowance9 =ISNULL(B.A9,0) " ;
            StrSql = StrSql + " ,Allowance10 =ISNULL(B.A10,0) ";

            StrSql = StrSql + " ,Allowance11 =ISNULL(B.A11,0) ";
            StrSql = StrSql + " ,Allowance12 =ISNULL(B.A12,0) ";
            //StrSql = StrSql + " ,Allowance13 =ISNULL(B.A13,0) ";
        //    StrSql = StrSql + " ,Allowance14 =ISNULL(B.A14,0) " ;
        //    StrSql = StrSql + " ,Allowance15 =ISNULL(B.A15,0) " ;
        //'    StrSql = StrSql + " ,Allowance16 =ISNULL(B.A16,0) " ;
        //'    StrSql = StrSql + " ,Allowance17 =ISNULL(B.A17,0) " ;
        //'    StrSql = StrSql + " ,Allowance18 =ISNULL(B.A18,0) " ;
        //'    StrSql = StrSql + " ,Allowance19 =ISNULL(B.A19,0) " ;
        //'    StrSql = StrSql + " ,Allowance20 =ISNULL(B.A20,0) " ;
        //'
        //    StrSql = StrSql + " ,Allowance21 =ISNULL(B.A21,0) " ;
        //    StrSql = StrSql + " ,Allowance22 =ISNULL(B.A22,0) " ;
        //    StrSql = StrSql + " ,Allowance23 =ISNULL(B.A23,0) " ;
        //    StrSql = StrSql + " ,Allowance24 =ISNULL(B.A24,0) " ;
        //    StrSql = StrSql + " ,Allowance25 =ISNULL(B.A25,0) " ;
        //    StrSql = StrSql + " ,Allowance26 =ISNULL(B.A26,0) " ;
        //    StrSql = StrSql + " ,Allowance27 =ISNULL(B.A27,0) " ;
            //StrSql = StrSql + " ,Allowance28 =ISNULL(B.A28,0) " ;
            StrSql = StrSql + " ,Allowance29 =ISNULL(B.A29,0) " ;  //반품공제
            StrSql = StrSql + " ,Allowance30 =ISNULL(B.A30,0) " ;  //기타수당

            StrSql = StrSql + " ,SumAllowance=ISNULL(B.AS1,0) " ;
            StrSql = StrSql + " ,SumInComeTax=ISNULL(B.AS2,0) " ;
            StrSql = StrSql + " ,SumResidentTax=ISNULL(B.AS3,0) " ;
            StrSql = StrSql + " ,SumTruePayment=ISNULL(B.AS4,0) " ;

            StrSql = StrSql + " FROM  tbl_CloseTotal_01  A, " ;

            StrSql = StrSql + " (Select " ;
            StrSql = StrSql + " Sum(convert(float,Allowance1)) AS A1 ,Sum(convert(float,Allowance2)) AS A2 ";
            StrSql = StrSql + ",Sum(convert(float,Allowance3)) AS A3 ,Sum(convert(float,Allowance4)) AS A4 ";
            StrSql = StrSql + ",Sum(convert(float,Allowance5)) AS A5 ,Sum(convert(float,Allowance6)) AS A6";
            StrSql = StrSql + ",Sum(convert(float,Allowance7)) AS A7 ,Sum(convert(float,Allowance8)) AS A8 ";
            StrSql = StrSql + ",Sum(convert(float,Allowance9)) AS A9 ,Sum(convert(float,Allowance10)) AS A10 ";
        //'
            StrSql = StrSql + ",Sum(convert(float,Allowance11)) AS A11 ,Sum(convert(float,Allowance12)) AS A12 ";
            //StrSql = StrSql + ",Sum(convert(float,Allowance13)) AS A13,Sum(convert(float,Allowance14)) AS A14 ";
        //    StrSql = StrSql + ",Sum(Allowance15) AS A15 " //,Sum(Allowance16) AS A16" ;
        //    StrSql = StrSql + ",Sum(Allowance17) AS A17,Sum(Allowance18) AS A18 " ;
        //    StrSql = StrSql + ",Sum(Allowance19) AS A19,Sum(Allowance20) AS A20 " ;
        
        //    StrSql = StrSql + ",Sum(Allowance21) AS A21,Sum(Allowance22) AS A22 " ;
        //    StrSql = StrSql + ",Sum(Allowance23) AS A23,Sum(Allowance24) AS A24 " ;
        //    StrSql = StrSql + ",Sum(Allowance25) AS A25 " //,Sum(Allowance26) AS A26" ;
        //    StrSql = StrSql + ",Sum(Allowance27) AS A27,Sum(Allowance28) AS A28 " ;

            //StrSql = StrSql + ",Sum(convert(float,Allowance1_cut)) AS A28";
            StrSql = StrSql + ",Sum(convert(float,Cur_DedCut_Pay)) AS A29";
            StrSql = StrSql + ",Sum(convert(float,Etc_Pay)) AS A30 ";


            StrSql = StrSql + ",Sum(convert(float,SumAllAllowance)) AS AS1,Sum(convert(float,InComeTax)) AS AS2 ";
            StrSql = StrSql + ",Sum(convert(float,ResidentTax)) AS AS3,Sum(convert(float,TruePayment)) AS AS4 ";
            StrSql = StrSql + " From tbl_ClosePay_01 " ;
            StrSql = StrSql + " ) B" ;
            StrSql = StrSql + " Where A.ToEndDate ='" + ToEndDate + "'" ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }




        private void tbl_CloseTotal_Put3(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 3  ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_CloseTotal_01 Set "  ;
            StrSql = StrSql + "  Allowance1Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance1 > 0),0) "  ;
            StrSql = StrSql + " ,Allowance2Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance2 > 0),0) "  ;
            StrSql = StrSql + " ,Allowance3Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance3 > 0),0) "  ;
            StrSql = StrSql + " ,Allowance4Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance4 > 0),0) "  ;
            StrSql = StrSql + " ,Allowance5Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance5 > 0),0) "  ;
            StrSql = StrSql + " ,Allowance6Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance6 > 0),0) "  ;
            StrSql = StrSql + " ,Allowance7Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance7> 0),0) "  ;
            StrSql = StrSql + " ,Allowance8Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance8 > 0),0) "  ;
            StrSql = StrSql + " ,Allowance9Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance9 > 0),0) "  ;
            StrSql = StrSql + " ,Allowance10Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance10 > 0),0) ";

            StrSql = StrSql + " ,Allowance11Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance11 > 0),0) "  ;
            StrSql = StrSql + " ,Allowance12Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance12 > 0),0) "  ;
            //StrSql = StrSql + " ,Allowance13Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance13 > 0),0) "  ;
    ////    StrSql = StrSql + " ,Allowance14Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance14 > 0),0) "  ;
        ////    StrSql = StrSql + " ,Allowance15Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance15 > 0),0) "  ;
        ////'    StrSql = StrSql + " ,Allowance16Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance16 > 0),0) "  ;
        ////'    StrSql = StrSql + " ,Allowance17Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance17> 0),0) "  ;
        ////'    StrSql = StrSql + " ,Allowance18Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance18 > 0),0) "  ;
        //////'    StrSql = StrSql + " ,Allowance19Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance19 > 0),0) "  ;
        ////'    StrSql = StrSql + " ,Allowance20Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance20 > 0),0) "  ;
        ////'
        ////    StrSql = StrSql + " ,Allowance21Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance21 > 0),0) "  ;
        ////    StrSql = StrSql + " ,Allowance22Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance22 > 0),0) "  ;
        ////    StrSql = StrSql + " ,Allowance23Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance23 > 0),0) "  ;
        ////    StrSql = StrSql + " ,Allowance24Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance24 > 0),0) "  ;
        ////    StrSql = StrSql + " ,Allowance25Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance25 > 0),0) "  ;
        //    StrSql = StrSql + " ,Allowance26Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance26 > 0),0) "  ;
        //    StrSql = StrSql + " ,Allowance27Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance27> 0),0) "  ;
            //StrSql = StrSql + " ,Allowance28Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Allowance2_cut > 0),0) ";
            StrSql = StrSql + " ,Allowance29Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Cur_DedCut_Pay > 0),0) ";
            StrSql = StrSql + " ,Allowance30Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where Etc_Pay > 0),0) ";
            StrSql = StrSql + " ,SumAllowanceCount = ISNULL((Select Count(Mbid) From tbl_ClosePay_01 Where SumAllAllowance > 0),0) " ; 
            
            StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "'" ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_CloseTotal_01 Set " ; 
            StrSql = StrSql + "  Allowance1Rate = (Allowance1 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            StrSql = StrSql + " ,Allowance2Rate = (Allowance2 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            StrSql = StrSql + " ,Allowance3Rate = (Allowance3 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            StrSql = StrSql + " ,Allowance4Rate = (Allowance4 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            StrSql = StrSql + " ,Allowance5Rate = (Allowance5 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            StrSql = StrSql + " ,Allowance6Rate = (Allowance6 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            StrSql = StrSql + " ,Allowance7Rate = (Allowance7 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            StrSql = StrSql + " ,Allowance8Rate = (Allowance8 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            StrSql = StrSql + " ,Allowance9Rate = (Allowance9 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            StrSql = StrSql + " ,Allowance10Rate = (Allowance10 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        
            StrSql = StrSql + " ,Allowance11Rate = (Allowance11 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            StrSql = StrSql + " ,Allowance12Rate = (Allowance12 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            //StrSql = StrSql + " ,Allowance13Rate = (Allowance13 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance14Rate = (Allowance14 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance15Rate = (Allowance15 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        ////'    StrSql = StrSql + " ,Allowance16Rate = (Allowance16 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        ////'    StrSql = StrSql + " ,Allowance17Rate = (Allowance17 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        ////'    StrSql = StrSql + " ,Allowance18Rate = (Allowance18 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //////    StrSql = StrSql + " ,Allowance19Rate = (Allowance19 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        ////'    StrSql = StrSql + " ,Allowance20Rate = (Allowance20 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance21Rate = (Allowance21 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance22Rate = (Allowance22 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance23Rate = (Allowance23 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance24Rate = (Allowance24 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance25Rate = (Allowance25 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //    StrSql = StrSql + " ,Allowance26Rate = (Allowance26 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //    StrSql = StrSql + " ,Allowance27Rate = (Allowance27 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;

            //StrSql = StrSql + " ,Allowance28Rate = (Allowance28 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            StrSql = StrSql + " ,Allowance29Rate = (Allowance29 /(TotalSellAmount-TotalReturnSellAmount)) * 100  ";
            StrSql = StrSql + " ,Allowance30Rate = (Allowance30 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;

            StrSql = StrSql + " ,SumAllowanceRate = (SumAllowance /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
    
            StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " And (TotalSellAmount-TotalReturnSellAmount) > 0";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 

        }




        private void MakeModForCheckRequirement1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 3;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


             StrSql = "Insert into tbl_ClosePay_01_Mod select "  ;
            StrSql = StrSql + " '" + ToEndDate + "','" + FromEndDate + "','" + PayDate + "','" + PayDate2 + "',*,'',''"  ;
            StrSql = StrSql + " From tbl_ClosePay_01 "  ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into tbl_ClosePay_01_Sell_Mod select "  ;
            StrSql = StrSql + " '" + ToEndDate + "',* From tbl_ClosePay_01_Sell";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }



        private void ReadyNewForCheckRequirement1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 15;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_Sham_Grade_P SET ";
            StrSql = StrSql + " Ap_Date= '" + ToEndDate + "'";
            StrSql = StrSql + " FROM  tbl_Sham_Grade_P  A, ";

            StrSql = StrSql + " (Select Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_ClosePay_01 ";
            StrSql = StrSql + " Where CurPoint > ShamPoint";
            StrSql = StrSql + " And   ShamPoint > 0 ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            StrSql = StrSql + " And   A.Ap_Date = ''";
            StrSql = StrSql + " And   A.Apply_Date <='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update Tbl_Memberinfo SET ";
            StrSql = StrSql + " CurPoint = 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update Tbl_Memberinfo SET ";
            StrSql = StrSql + " CurPoint=ISNULL(B.CurPoint,0) ";
            StrSql = StrSql + " FROM  Tbl_Memberinfo  A, ";

            StrSql = StrSql + " (Select Mbid,Mbid2,CurPoint ";
            StrSql = StrSql + " From tbl_ClosePay_01 ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid  = B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2 = B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            
            if (int.Parse(FromEndDate) >= int.Parse (Base_Chang_Date___1))
            {
                StrSql = "Update tbl_Sham_Grade SET ";
                StrSql = StrSql + " Ap_Date= '" + ToEndDate + "'";
                StrSql = StrSql + " FROM  tbl_Sham_Grade  A, ";

                StrSql = StrSql + " (Select Mbid,Mbid2 ";
                StrSql = StrSql + " From tbl_ClosePay_01 ";
                StrSql = StrSql + " Where CurGrade > ShamGrade";
                StrSql = StrSql + " And   ShamGrade > 0 ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
                StrSql = StrSql + " And   A.Ap_Date = ''";
                StrSql = StrSql + " And   A.Apply_Date <='" + ToEndDate + "'";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
                                

                StrSql = "Update Tbl_Memberinfo SET ";
                StrSql = StrSql + " CurGrade = 0 , Max_CurGrade = 0 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();


                StrSql = "Update Tbl_Memberinfo SET ";
                StrSql = StrSql + " CurGrade=ISNULL(B.CurGrade,0) ";                
                StrSql = StrSql + " FROM  Tbl_Memberinfo  A, ";

                StrSql = StrSql + " (Select Mbid,Mbid2, CurGrade ";
                StrSql = StrSql + " From tbl_ClosePay_01 ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid  = B.Mbid ";
                StrSql = StrSql + " And   A.Mbid2 = B.Mbid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }



            StrSql = "Update tbl_ClosePay_01 set " ;
            StrSql = StrSql + " DayPrice01 =0, DayPrice02 =0 , DayPrice03 = 0, " ;
            StrSql = StrSql + " DayPv01 =0, DayPv02 =0 , DayPv03 = 0, " ;
        //    StrSql = StrSql + " DayCv01 =0, DayCv02 =0 , DayCv03 = 0, " ;
    
            StrSql = StrSql + " SellPrice01 =0, SellPrice02 =0 , SellPrice03 = 0, " ;
            StrSql = StrSql + " SellPv01 =0, SellPv02 =0 , SellPv03 = 0, " ;
        //    StrSql = StrSql + " SellCv01 =0, SellCv02 =0 , SellCv03 = 0,  " ;
    
            StrSql = StrSql + " DaySham01 =0, SellSham01 =0 , " ;

            StrSql = StrSql + " LeaveDate = '',BankCode='',BankAcc='',Cpno='',BankOwner='',RegTime='',  BusCode = '' , StopDate = '', Sell_Mem_TF = 0 ,  RBO_Mem_TF = 0 ,";
            StrSql = StrSql + " ReqTF1 = 0, ReqTF2 = 0, "  ;
            
            StrSql = StrSql + " Saveid='',Saveid2=0,LineCnt=0,LevelCnt=0," ;
            StrSql = StrSql + " Nominid='',Nominid2=0,N_LineCnt=0 , " ;

            StrSql = StrSql + " BeforeGrade =  CurGrade, CurGrade= 0, OrgGrade= 0 , ShamGrade = 0 , OneGrade = 0 , Be_Month_PV = 0 ,";

            StrSql = StrSql + " BePoint = CurPoint,ShamPoint = 0 , CurPoint = 0 ,Allowance6_Cut = 0 ,Max_Pay = 0 ,  "; 
            StrSql = StrSql + " Be_PV_1 =  Sum_PV_1 , Sum_PV_1= 0, Ded_1= 0 , Fresh_1 = 0 , Cur_PV_1 = 0 , Re_Cur_PV_1 = 0 , Sham_PV_1 = 0 ,  ";
            StrSql = StrSql + " Be_PV_2 =  Sum_PV_2 , Sum_PV_2= 0, Ded_2= 0 , Fresh_2 = 0 , Cur_PV_2 = 0 , Re_Cur_PV_2 = 0 , Sham_PV_2 = 0 , ";

            StrSql = StrSql + " G_Sum_PV_1 =  0 , GradeCnt1_1= 0, GradeCnt2_1= 0 , GradeCnt3_1 = 0 , GradeCnt4_1 = 0 , GradeCnt5_1 = 0 , GradeCnt6_1 = 0 ,  ";
            StrSql = StrSql + " G_Sum_PV_2 =  0 , GradeCnt1_2= 0, GradeCnt2_2= 0 , GradeCnt3_2 = 0 , GradeCnt4_2 = 0 , GradeCnt5_2 = 0 , GradeCnt6_2 = 0 , ";


            StrSql = StrSql + " Sum_Return_Take_Pay = 0 , Sum_Return_DedCut_Pay = 0 , Sum_Return_Remain_Pay = 0 , Cur_DedCut_Pay = 0 ,   " ;

            StrSql = StrSql + " Allowance1_M = 0 , Etc_Pay = 0 ,";
            StrSql = StrSql + " Allowance1=0,Allowance2=0 , Allowance3=0 , Allowance4=0, Allowance5=0," ;
            StrSql = StrSql + " Allowance6=0,Allowance7=0,  Allowance8=0 , Allowance9=0, Allowance10=0," ;
            StrSql = StrSql + " Allowance11 = 0,Allowance12 = 0, ";

            StrSql = StrSql + " SumAllAllowance=0," ;
            StrSql = StrSql + " InComeTax=0, ResidentTax=0,TruePayment=0 " ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_01_Sell set " ;
            StrSql = StrSql + " BeAmount = SumAmount , BeCash = SumCash, BeCard = SumCard ,BeBank = SumBank , " ;
            StrSql = StrSql + " BeTotalPV = SumTotalPV,BeShamSell = SumShamSell," ;
    
            StrSql = StrSql + " BeReAmount = SumReAmount, BeReCash = SumReCash,BeReBank = SumReBank," ;
            StrSql = StrSql + " BeReCard = SumReCard , BeReTotalPV = SumReTotalPV , " ;
    
            StrSql = StrSql + " DayAmount = 0, DayCash = 0, DayCard = 0,DayBank=0, DayTotalPV= 0 , DayShamSell = 0,  " ;
            StrSql = StrSql + " DayReAmount=0, DayReCash = 0, DayReCard=0,DayReBank=0, DayReTotalPV = 0, " ;
    
            StrSql = StrSql + " SumAmount = 0, SumCash = 0, SumCard = 0,SumBank=0,SumTotalPV=0, SumShamSell=0, " ;
            StrSql = StrSql + " SumReAmount =0,SumReCash = 0, SumReCard=0,SumReBank=0, SumReTotalPV=0, " ;
    
            StrSql = StrSql + " BeReTotalCV = SumReTotalCV ,BeTotalCV = SumTotalCV ,SumTotalCV=0, "  ;
            StrSql = StrSql + " DayTotalCV= 0 , DayReTotalCV = 0, SumReTotalCV=0 " ;


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }


        private void Check_Close_Gid(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int Close_Sort, int Close_Cancel_TF)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Insert Into tbl_Close_Log Values (" + Close_Sort + ",'" + FromEndDate + "','" + ToEndDate + " ', " + Close_Cancel_TF + ",'" + cls_User.gid  + "', Convert(Varchar(25),GetDate(),21) )";
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }





        
























    }
}
