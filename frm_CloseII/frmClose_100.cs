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
    public partial class frmClose_100 : Form
    {
  


        cls_Grid_Base cgb = new cls_Grid_Base();
        private int Data_Set_Form_TF = 0;
        private string base_db_name = "tbl_CloseTotal_100";
        private string FromEndDate = ""; private string ToEndDate = ""; private string PayDate = "", PayDate2  = "" ;
        private int From_Load_TF = 0;
        private int Cl_F_TF = 0, ReCnt = 0 ;

        Dictionary<string, cls_Close_Mem> Clo_Mem = new Dictionary<string, cls_Close_Mem>();
        Dictionary<string, cls_Close_Sell> Clo_Sell = new Dictionary<string, cls_Close_Sell>();

        cls_Close_Sell[] C_Sell;

        cls_Connect_DB Search_Connect = new cls_Connect_DB();
        SqlConnection Search_Conn = null; 

        public frmClose_100()
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
            Tsql = "Select Isnull (Max(ToEndDate),'') From  tbl_CloseTotal_100 (nolock) ";

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

                    txtPayDate.Focus();
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
            StrSql = StrSql + " And TotalPV + TotalCV > 0 ";
            StrSql = StrSql + " And Ga_Order = 0 ";            
                             
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            txt_SellCnt.Text = "0";
            if (ReCnt != 0)            
                txt_SellCnt.Text =  ds.Tables[base_db_name].Rows[0][0].ToString();


            StrSql = "select Isnull(Count(Mbid),0) from tbl_SalesDetail ";
            StrSql = StrSql + " LEFT JOIN tbl_SalesDetail_TF ON tbl_SalesDetail.OrderNumber = tbl_SalesDetail_TF.OrderNumber";
            StrSql = StrSql + " Where SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " And TotalPV + TotalCV < 0 ";
            
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
            PayDate = TodayDate.AddDays(14).ToString("yyyy-MM-dd").Replace("-", "");
            txtPayDate.Text = PayDate;
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

            if (txtPayDate.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                      + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_PayDate")
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtPayDate.Focus(); return false;
            }

            cls_Check_Input_Error c_er = new cls_Check_Input_Error();     
            if (txtPayDate.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtPayDate);

                if (Ret == -1)
                {
                    txtPayDate.Focus(); return false;
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


            return true;
        }


        private void butt_Pay_Click(object sender, EventArgs e)
        {
            if (Search_Check_TextBox_Error() == false) return;

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_Start"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            pg1.Visible = true; pg1.Maximum = 0;
            pg2.Visible = true; pg2.Maximum = 0;
            butt_Pay.Enabled = false; butt_Exit.Enabled = false;
            tableLayoutPanel2.Enabled = false;
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
            tableLayoutPanel2.Enabled = true;

            FromEndDate = ""; ToEndDate = ""; PayDate = "";
            txtPayDate.Text = ""; txt_To.Text = ""; txt_From.Text = "";
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
            pg2.Minimum = 0;            pg2.Maximum = 15;
            pg2.Step = 1;               pg2.Value = 0;
            pg1.Step = 1; 

            Cl_F_TF = 1;
            PayDate = txtPayDate.Text;
            Make_Close_Table(Temp_Connect, Conn, tran);   //테이블 생성           
            pg2.PerformStep() ; pg2.Refresh();

            Put_Leave_StopDate(Temp_Connect, Conn, tran);  //탈퇴나 수당 중지일자 가져오기
            pg2.PerformStep(); pg2.Refresh();

            Put_Member_Base_Info(Temp_Connect, Conn, tran);  //회원정보 가져오기
            pg2.PerformStep(); pg2.Refresh();

            //Put_Sell_Date(Temp_Connect, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();

            //Put_SellPV(Temp_Connect, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();

            //Put_DayPV(Temp_Connect, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();


            //ReqTF1(Temp_Connect, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();                       
           
            //Put_OrgGrade(Temp_Connect, Conn, tran);     
            //pg2.PerformStep(); pg2.Refresh();

            //Put_Cut_PV_4_1(Temp_Connect, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();

            Put_cls_Close_Mem(Temp_Connect, Conn, tran);  //수당테이블에 만들어진 사람들에 대한 내역을 넣는다.
            pg2.PerformStep(); pg2.Refresh();

            //Put_Mem_Sell_Info(Temp_Connect, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();
            
            //Put_Down_PV_01(Temp_Connect, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();
            
            //--------------------------------------------------------------
            Give_Allowance1(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            //Give_Allowance2(Temp_Connect, Conn, tran);
            //Give_Allowance2_TEST(Temp_Connect, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();

            //Give_Allowance3(Temp_Connect, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------




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

            Check_Close_Gid(Temp_Connect, Conn, tran,100,0);
            pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------


        }


        private void Make_Close_Table(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Minimum = 0; pg1.Maximum = 40; pg1.Refresh(); 
            
            pg1.Value = 10; ; pg1.Refresh(); 
            //pg1.PerformStep(); pg1.Refresh(); 

            string StrSql = "";

            StrSql = "INSERT INTO tbl_ClosePay_100 (Mbid,Mbid2, BusCode, Bus_Name ,RecordMakeDate)  ";
            StrSql = StrSql + " Select   A.Mbid,A.Mbid2, A.NCode, A.Name , '" + ToEndDate + "' From tbl_Business AS A  (nolock)  ";
            StrSql = StrSql + " LEFT Join tbl_ClosePay_100 AS B ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 ";
            StrSql = StrSql + " Where b.Mbid Is Null " ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
            pg1.Value = 20; pg1.Refresh(); 

        }


        private void  Put_Leave_StopDate(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 3    ;
            pg1.PerformStep(); pg1.Refresh(); 

            string StrSql = "";
    
            StrSql = "Update tbl_ClosePay_100 SET StopDate = ISNULL(B.PayStop_Date,'')" ;
           StrSql = StrSql + " FROM  tbl_ClosePay_100  A,";
           StrSql = StrSql + " (";
           StrSql = StrSql + " Select    PayStop_Date,Mbid,Mbid2   From tbl_Memberinfo   (nolock) ";
           StrSql = StrSql + " Where PayStop_Date <= '" + ToEndDate + "'";
           StrSql = StrSql + " And   PayStop_Date <>'' ";
           StrSql = StrSql + " ) B";
           StrSql = StrSql + "  Where a.Mbid = b.Mbid And a.Mbid2 = b.Mbid2";
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 

    
            StrSql = "Update tbl_ClosePay_100 SET LeaveDate=ISNULL(B.LeaveDate,'')";
           StrSql = StrSql + " FROM  tbl_ClosePay_100  A,";
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
    
            StrSql = "Update tbl_ClosePay_100 SET" ;
            StrSql = StrSql +" BankCode=ISNULL(B.BankCode,'')";
            StrSql = StrSql +" ,Cpno=ISNULL(B.Cpno,'')";
            StrSql = StrSql +" ,BankAcc=ISNULL(B.bankaccnt,'')";
            StrSql = StrSql +" ,BankOwner=ISNULL(B.BankOwner,'')";
            StrSql = StrSql +" ,M_Name=ISNULL(B.M_Name,'')";
            
            StrSql = StrSql +" ,Nominid=ISNULL(B.Nominid,'')";
            StrSql = StrSql +" ,Nominid2=ISNULL(B.Nominid2,0)";
    
            StrSql = StrSql +" ,RegTime=  replace(ISNULL(B.regtime,''),'-','')";
            StrSql = StrSql +"  FROM  tbl_ClosePay_100  A,";
    
            StrSql = StrSql +" (";
            StrSql = StrSql +" Select   BankCode,Cpno,bankaccnt,BankOwner,M_Name,businesscode,ED_Date,";
            StrSql = StrSql +" Saveid,Saveid2,LineCnt,Nominid,Nominid2,N_LineCnt,";
            StrSql = StrSql +" Mbid,Mbid2,regtime , Sell_Mem_TF  ";
            StrSql = StrSql +"  From tbl_Memberinfo   (nolock)   ";
            StrSql = StrSql +" ) B";
            StrSql = StrSql +" Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And a.Mbid2 = b.Mbid2";
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
        }



        private void Put_Sell_Date(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
         
            pg1.Value = 0; pg1.Maximum = 2    ;
            pg1.PerformStep(); pg1.Refresh(); 

            string StrSql = "";
    
        
            if  (Cl_F_TF == 0) 
            {
                StrSql = " Update tbl_ClosePay_100_Sell SET" ;
                StrSql = StrSql + " BeAmount = IsNull(b.A1, 0)";
                StrSql = StrSql + " ,BeCash=ISNULL(B.A2,0)";
                StrSql = StrSql + " ,BeCard=ISNULL(B.A3,0)";
                StrSql = StrSql + " ,BeBank=ISNULL(B.A4,0)";
                StrSql = StrSql + " ,BeTotalPV=ISNULL(B.A5,0)";
                StrSql = StrSql + " ,BeTotalCV=ISNULL(B.A6,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_100_Sell  A,";

                StrSql = StrSql + " (";
                StrSql = StrSql + " Select  Sum(TotalPrice) AS A1,Sum(InputCash) AS A2, ";
                StrSql = StrSql + " Sum(InputCard) AS A3 ,        Sum(InputPassbook) AS A4 , ";
                StrSql = StrSql + " Sum(TotalPV) AS A5,           Sum(TotalCV) AS A6, ";
                StrSql = StrSql + " Mbid,Mbid2 , SellCode";
                StrSql = StrSql + " From tbl_SalesDetail  ";
                StrSql = StrSql + " Where   SellDate < '" + FromEndDate + "'";
                StrSql = StrSql + " And   TotalPV  + TotalCV + TotalPrice > 0 ";
                StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where a.Mbid = b.Mbid ";
                StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
                StrSql = StrSql + " And   a.SellCode = b.SellCode";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                StrSql = " Update tbl_ClosePay_100_Sell SET";
                StrSql = StrSql + "  BeReAmount = -IsNull(b.A1, 0)";
                StrSql = StrSql + " ,BeReCash=-ISNULL(B.A2,0)";
                StrSql = StrSql + " ,BeReCard=-ISNULL(B.A3,0)";
                StrSql = StrSql + " ,BeReBank=-ISNULL(B.A4,0)";
                StrSql = StrSql + " ,BeReTotalPV=-ISNULL(B.A5,0)";
                StrSql = StrSql + " ,BeReTotalCV=-ISNULL(B.A6,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_100_Sell  A,";

                StrSql = StrSql + " (";
                StrSql = StrSql + " Select  Sum(TotalPrice) AS A1,Sum(InputCash) AS A2, ";
                StrSql = StrSql + " Sum(InputCard) AS A3 ,        Sum(InputPassbook) AS A4 , ";
                StrSql = StrSql + " Sum(TotalPV) AS A5,           Sum(TotalCV) AS A6, ";
                StrSql = StrSql + " Mbid,Mbid2 , SellCode";
                StrSql = StrSql + " From tbl_SalesDetail  ";
                StrSql = StrSql + " Where   SellDate < '" + FromEndDate + "'";
                StrSql = StrSql + " And   TotalPV  + TotalCV < 0 ";
                StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where a.Mbid = b.Mbid ";
                StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
                StrSql = StrSql + " And   a.SellCode = b.SellCode";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                StrSql = " Update tbl_ClosePay_100_Sell SET";
                StrSql = StrSql + " BeShamSell = IsNull(b.A1, 0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_100_Sell  A,";
                StrSql = StrSql + " (";
                StrSql = StrSql + " Select    Sum(Apply_PV) AS A1, Mbid,Mbid2 , SellCode";
                StrSql = StrSql + " From tbl_Sham_Sell  ";
                StrSql = StrSql + " Where   Apply_Date < '" + FromEndDate + "'";
                StrSql = StrSql + " And     Apply_PV <> 0";
                StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where a.Mbid = b.Mbid ";
                StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
                StrSql = StrSql + " And   a.SellCode = b.SellCode";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }


    
            StrSql = "Select Mbid,Mbid2 ,OrderNumber, SellCode , TotalPrice , InputCash , InputCard , InputPassbook , TotalPV , TotalCV , Re_BaseOrderNumber , SellDate  ";
            StrSql = StrSql + " From    tbl_SalesDetail  (nolock)  ";
            StrSql = StrSql + " Where   SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     TotalPV  + TotalCV < 0 ";
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
                    StrSql = "Select ToEndDate,  PayDate   From  tbl_CloseTotal_100 (nolock) ";
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
                        StrSql = "Update tbl_ClosePay_100_Sell SET ";
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

            StrSql = " Update tbl_ClosePay_100_Sell SET";
            StrSql = StrSql + " DayAmount = IsNull(b.A1, 0)";
            StrSql = StrSql + " ,DayCash=ISNULL(B.A2,0)";
            StrSql = StrSql + " ,DayCard=ISNULL(B.A3,0)";
            StrSql = StrSql + " ,DayBank=ISNULL(B.A4,0)";
            StrSql = StrSql + " ,DayTotalPV=ISNULL(B.A5,0)";
            StrSql = StrSql + " ,DayTotalCV=ISNULL(B.A6,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_100_Sell  A,";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select  ";
            StrSql = StrSql + " Sum(BS1.TotalPrice) + Isnull(Sum(Bs_R.TotalPrice),0)  AS A1,         Sum(BS1.InputCash)  + Isnull(Sum(Bs_R.InputCash),0)      AS A2, ";
            StrSql = StrSql + " Sum(BS1.InputCard)  + Isnull(Sum(Bs_R.InputCard),0)   AS A3 ,        Sum(BS1.InputPassbook)  + Isnull(Sum(Bs_R.InputPassbook),0)  AS A4 , ";
            StrSql = StrSql + " Sum(BS1.TotalPV)    + Isnull(Sum(Bs_R.TotalPV),0)     AS A5,         Sum(BS1.TotalCV)  + Isnull(Sum(Bs_R.TotalCV),0)        AS A6, ";
            StrSql = StrSql + " BS1.Mbid,BS1.Mbid2 , BS1.SellCode";
            StrSql = StrSql + " From tbl_SalesDetail AS BS1 ";
            StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate <= '" + PayDate + "'";
            //StrSql = StrSql + " LEFT JOIN tbl_SalesDetail_TF ON BS1.OrderNumber = tbl_SalesDetail_TF.OrderNumber";
            StrSql = StrSql + " Where   BS1.SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     BS1.SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     BS1.TotalPV  + BS1.TotalCV >= 0 ";
            StrSql = StrSql + " And     BS1.SellCode <> '' ";
            StrSql = StrSql + " And     BS1.Ga_order = 0 ";
            StrSql = StrSql + " Group By BS1.Mbid,BS1.Mbid2, BS1.SellCode";
            StrSql = StrSql + " Having Sum(BS1.TotalPV) + IsNull(Sum(Bs_R.TotalPV), 0) >= 0";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
            StrSql = StrSql + " And   a.SellCode = b.SellCode";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();  //구매 종류 별로 넣는다. 합계를 +판매에 대해서만


        
            StrSql = " Update tbl_ClosePay_100_Sell SET";
            StrSql = StrSql + " DayShamSell = IsNull(b.A1, 0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_100_Sell  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select    Sum(Apply_PV) AS A1, Mbid,Mbid2 , SellCode";
            StrSql = StrSql + " From tbl_Sham_Sell  ";
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
    
    
    
            StrSql = " Update tbl_ClosePay_100_Sell Set";
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
       

            StrSql = "Update tbl_ClosePay_100 SET " ;
            StrSql = StrSql + " SellPrice01=ISNULL(B.A1,0) " ;
            StrSql = StrSql + ",SellPv01=ISNULL(B.A2,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_100  A, " ;
    
            StrSql = StrSql + " (Select Sum(SumAmount-SumReAmount) AS A1, Sum(SumTotalPV-SumReTotalPV) AS A2 , Sum(SumTotalCV-SumReTotalCV) AS A3 , Sum(SumShamSell) AS A4 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_100_Sell " ;
            StrSql = StrSql + " Where SellCode ='01'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_100 SET " ;
            StrSql = StrSql + " SellSham01 = ISNULL(B.A4,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_100  A, " ;
    
            StrSql = StrSql + " (Select  Sum(SumShamSell) AS A4 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_100_Sell " ;
            StrSql = StrSql + " Where SellCode ='01'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_100 SET " ;
            StrSql = StrSql + " SellPrice02 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + ",SellPv02 = ISNULL(B.A2,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_100  A, " ;
    
            StrSql = StrSql + " (Select Sum(SumAmount-SumReAmount) AS A1, Sum(SumTotalPV-SumReTotalPV) AS A2 , Sum(SumTotalCV-SumReTotalCV) AS A3 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_100_Sell " ;
            StrSql = StrSql + " Where SellCode ='02'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_100 SET " ;
            StrSql = StrSql + " SellPrice03 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + ",SellPv03 = ISNULL(B.A2,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_100  A, " ;

            StrSql = StrSql + " (Select Sum(SumAmount-SumReAmount) AS A1, Sum(SumTotalPV-SumReTotalPV) AS A2 , Sum(SumTotalCV-SumReTotalCV) AS A3 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_100_Sell " ;
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
  
            StrSql = "Update tbl_ClosePay_100 SET " ;
            StrSql = StrSql + "  DayPrice01 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,DayPv01 = ISNULL(B.A2,0) " ;
            //StrSql = StrSql + " ,DayCV01 = ISNULL(B.A3,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_100  A, " ;
    
            StrSql = StrSql + " (Select Sum(DayAmount-DayReAmount) AS A1 , Sum(DayTotalPV-DayReTotalPV) AS A2 ,Sum(DayTotalCV-DayReTotalCV) AS A3 ,Sum(DayShamSell) AS A4  ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_100_Sell " ;
            StrSql = StrSql + " Where SellCode ='01'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
            StrSql = "Update tbl_ClosePay_100 SET " ;
            StrSql = StrSql + " DaySham01 = ISNULL(B.A4,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_100  A, " ;
    
            StrSql = StrSql + " (Select  Sum(DayShamSell) AS A4 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_100_Sell " ;
            StrSql = StrSql + " Where SellCode ='01'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_100 SET " ;
            StrSql = StrSql + "  DayPrice02 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,DayPv02 = ISNULL(B.A2,0) " ;
            //trSql = StrSql + " ,DayCV02 = ISNULL(B.A3,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_100  A, " ;
            StrSql = StrSql + " (Select Sum(DayAmount-DayReAmount) AS A1 ,Sum(DayTotalPV-DayReTotalPV) AS A2  ,Sum(DayTotalCV-DayReTotalCV) AS A3,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_100_Sell " ;
            StrSql = StrSql + " Where SellCode ='02'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_100 SET " ;
            StrSql = StrSql + "  DayPrice03 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,DayPv03 = ISNULL(B.A2,0) " ;
            //StrSql = StrSql + " ,DayCV03 = ISNULL(B.A3,0) " ;
    
            StrSql = StrSql + " FROM  tbl_ClosePay_100  A, " ;

            StrSql = StrSql + " (Select Sum(DayAmount-DayReAmount) AS A1 , Sum(DayTotalPV-DayReTotalPV) AS A2  ,Sum(DayTotalCV-DayReTotalCV) AS A3,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_100_Sell " ;
            StrSql = StrSql + " Where SellCode ='03'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;

            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
     }




        private void ReqTF1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            pg1.Value = 0; pg1.Maximum = 3    ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";     
    
            StrSql = " Update tbl_ClosePay_100 SET";
            StrSql = StrSql + " ReqTF1 = 1 ";
            StrSql = StrSql + " ,CurPoint = 1 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
            StrSql = "Update tbl_ClosePay_100 Set ";
            StrSql = StrSql + " ReqDate1='" + ToEndDate + "'";
            StrSql = StrSql + " Where ReqDate1=''";
            StrSql = StrSql + " And ReqTF1 >= 1 ";
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        //'''''-------------------------------------------------//////////////////////////////
        }





        private void Put_OrgGrade( cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran )
       {
            pg1.Value = 0; pg1.Maximum = 6   ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
            string SDate = "";
    
            StrSql = "Select Isnull(Max(ToEndDate), '')  From tbl_CloseTotal_02 (nolock) "  ;   //'''--직급마감에서 전달 마감일자를 알아온다.
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

    
            StrSql = "Update tbl_ClosePay_100 SET "  ;
            StrSql = StrSql  + "  CurGrade =ISNULL(B.A1,0) "   ;
            StrSql = StrSql  + " FROM  tbl_ClosePay_100  A, "   ;
    
            StrSql = StrSql  + " (Select  CurGrade As A1 , Mbid,Mbid2 "   ;
            StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
            StrSql = StrSql  + " Where ToEndDate = '" + SDate +  "'"   ;
            StrSql = StrSql  + " ) B"   ;
    
            StrSql = StrSql  + " Where A.Mbid=B.Mbid "   ;
            StrSql = StrSql  + " And   A.Mbid2=B.Mbid2 "   ;
    
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


            StrSql = "Select Isnull(Max(ToEndDate),'') , Isnull(Max(FromEndDate),'') From tbl_CloseTotal_100 (nolock) ";

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
                StrSql = "Select Isnull(Max(ToEndDate),'') , Isnull(Max(FromEndDate),'') From tbl_CloseTotal_100  (nolock) ";
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
                StrSql = "Select Isnull(Max(ToEndDate),'') , Isnull(Max(FromEndDate),'') From tbl_CloseTotal_100  (nolock)  ";
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
                StrSql = StrSql + " From tbl_ClosePay_100_Mod (nolock) "  ; 
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
                        StrSql = "Select Mbid,Mbid2 From tbl_ClosePay_100_Mod  (nolock)  ";
                        StrSql = StrSql + "  Where Mbid  = '" + Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "'";
                        StrSql = StrSql + "  And   Mbid2 =  " + Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                        StrSql = StrSql + "  And   ToEndDate  = '" + Dset4.Tables[base_db_name].Rows[fi_cnt]["ToEndDate"].ToString() + "'";
                        StrSql = StrSql +  " And   Allowance2  > 0 " ;
                                                
                        DataSet Dset5 = new DataSet();
                        Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset5);                        
                        int ReCnt5 = Search_Connect.DataSet_ReCount;

                        if (ReCnt5 <= 0)
                        {
                            StrSql = "Update tbl_ClosePay_100 SET ";
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
                        
            StrSql = "Select Mbid,Mbid2, M_Name, Nominid, Nominid2,  LeaveDate, StopDate , BusCode  ";
            StrSql = StrSql + "  From tbl_ClosePay_100 ";

            
            SqlDataReader sr = null;            
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;
            
            string T_Mbid = "";

            Dictionary<string, cls_Close_Mem> T_Clo_Mem = new Dictionary<string, cls_Close_Mem>();

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            
            while (sr.Read())
            {
                cls_Close_Mem t_c_mem = new cls_Close_Mem();
                T_Mbid = sr.GetValue(7).ToString() ;  


                t_c_mem.Mbid = sr.GetValue(0).ToString();
                t_c_mem.Mbid2 = int.Parse(sr.GetValue(1).ToString());
                t_c_mem.M_Name = sr.GetValue(2).ToString();

                t_c_mem.Nominid = sr.GetValue(3).ToString()  ;
                t_c_mem.Nominid2 = int.Parse(sr.GetValue(4).ToString());

                t_c_mem.LeaveDate = sr.GetValue(5).ToString();
                t_c_mem.StopDate = sr.GetValue(6).ToString();

                t_c_mem.BusCode = sr.GetValue(7).ToString();
                                
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
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R   (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate <= '" + PayDate + "'";        
            //StrSql = StrSql + " LEFT JOIN tbl_SalesDetail_TF  (nolock)  ON Se.OrderNumber = tbl_SalesDetail_TF.OrderNumber";
            StrSql = StrSql + " LEFT JOIN tbl_ClosePay_100 Ce1 ON Ce1.Mbid = SE.Mbid And Ce1.Mbid2 = SE.Mbid2";

            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) >= 0 ";    
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
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
            pg1.Value = 0;             
            var sellCnt = from sellinfo in C_Sell                            
                            group sellinfo by  sellinfo.SellCode == "01" into Gr
                            select new
                            {
                                T_Count =  Gr.Count ()
                            };

            foreach (var stateCnt in sellCnt )
            {
                pg1.Maximum = stateCnt.T_Count ;
            }
            pg1.PerformStep(); pg1.Refresh();


            var sellinfos = from sellinfo in C_Sell
                            where sellinfo.SellCode == "01"
                            orderby sellinfo.OrderNumber  
                            select new
                            {
                                TotalPV = sellinfo.TotalPV,
                                RePV = sellinfo.RePV,
                                SellDate = sellinfo.SellDate,
                                OrderNumber = sellinfo.OrderNumber,
                                M_Name = sellinfo.M_Name ,
                                Mbid = sellinfo.Mbid ,
                                Mbid2 = sellinfo.Mbid2 ,
                                Saveid = sellinfo.Saveid ,
                                Saveid2 = sellinfo.Saveid2,
                                LineCnt = sellinfo.LineCnt 
                            };

            int LevelCnt = 0 ,TSaveid2 = 0 , TLine = 0 ;
            string TSaveid = "",S_Mbid = ""; 
            double Rs_TotalPV = 0 ,Rs_RePV = 0 ;
            string StrSql = "";



            foreach (var sellinfo in sellinfos )
            {
                LevelCnt = 0;
                TSaveid = sellinfo.Saveid.ToString () ;
                TSaveid2 = int.Parse(sellinfo.Saveid2.ToString());
                TLine = int.Parse(sellinfo.LineCnt.ToString());
                Rs_TotalPV = double.Parse(sellinfo.TotalPV .ToString());
                Rs_RePV = double.Parse(sellinfo.RePV.ToString());

                S_Mbid = TSaveid + "-" + TSaveid2.ToString ();
                while (TSaveid != "**")
                {
                    LevelCnt  ++ ; 
                    
                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        //if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && (Clo_Mem[S_Mbid].CurGrade >=20 || Clo_Mem[S_Mbid].CurPoint >=1 )  )
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" )
                        {
                            StrSql = "Update tbl_ClosePay_100 SET ";
                            if (TLine == 1) 
                                 StrSql = StrSql + " Cur_PV_1 = Cur_PV_1 +  " + (Rs_TotalPV + Rs_RePV) ;
                                             
                             if (TLine >= 2)
                                 StrSql = StrSql + " Cur_PV_2 = Cur_PV_2 + " + (Rs_TotalPV + Rs_RePV) ; 

                            StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                            StrSql = StrSql + " And     Mbid2 = " + TSaveid2 ;

                            Temp_Connect.Insert_Data(StrSql, Conn, tran); 

                   
                            StrSql = "INSERT INTO tbl_Close_DownPV_PV_01" ;
                            StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName," ;
                            StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV ,  ";
                            StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_DownPV) ";
                         
                            StrSql = StrSql + "Values(" ;
                            StrSql = StrSql + "'" + ToEndDate + "','" + sellinfo.Mbid.ToString() + "'";
                            StrSql = StrSql + "," + sellinfo.Mbid2.ToString() + ",'" + sellinfo.M_Name.ToString() + "',";
                            StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                            StrSql = StrSql + (Rs_TotalPV + Rs_RePV) + ", " + LevelCnt + " ," + TLine;
                            StrSql = StrSql + ",'01' ,'" + sellinfo.OrderNumber.ToString () + "',0)";

                            Temp_Connect.Insert_Data(StrSql, Conn, tran) ;


                        }

                        TSaveid = Clo_Mem[S_Mbid].Saveid ;  TSaveid2 = Clo_Mem[S_Mbid].Saveid2 ;    TLine = Clo_Mem[S_Mbid].LineCnt ;
                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }


                } //foreach


                pg1.PerformStep(); pg1.Refresh();
            }


            StrSql = "Update tbl_ClosePay_100 SET ";
            StrSql = StrSql + "   Sum_PV_1 = Be_PV_1 + Cur_PV_1 - Cut_PV_4_1 ";
            StrSql = StrSql + "  ,Sum_PV_2 = Be_PV_2 + Cur_PV_2 - Cut_PV_4_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
    
        }



        private void Give_Allowance1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
           pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            int LevelCnt = 0, SMbid2 = 0 ; 
            string BusCode = "" , SMbid = "" , OrderNumber = "", SMbid_Name = "", Mem_BusCode = "" ; 
            double Allowance2 = 0, Allowance1 = 0,  TotalPV = 0 ;

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select Se.TotalPV ,  Se.TotalPrice,  Se.Mbid , Se.Mbid2 , Ce1.N_LineCnt , Ce1.LineCnt,Se.BusCode S_BusCode,  Ce1.BusinessCode C_BusCode,   ";
            StrSql = StrSql + " Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate   ";
            StrSql = StrSql +" From tbl_SalesDetail Se  (nolock) ";
            StrSql = StrSql +" LEFT JOIN tbl_Memberinfo Ce1  (nolock) ON Ce1.Mbid = SE.Mbid And Ce1.Mbid2 = SE.Mbid2";
            StrSql = StrSql + " LEFT JOIN tbl_Business  (nolock) ON tbl_Business.Ncode = Ce1.BusinessCode And Ce1.Na_code = tbl_Business.Na_code ";
            StrSql = StrSql + " LEFT JOIN tbl_Business SBB  (nolock) ON SBB.Ncode = Se.BusCode  And Se.Na_code = SBB.Na_code ";
            StrSql = StrSql +" LEFT JOIN tbl_ClosePay_100 CCC_10 ON CCC_10.BusCode = Se.BusCode";    
            StrSql = StrSql +" Where   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql +" And   Se.SellDate  <='" + ToEndDate + "'";            
            StrSql = StrSql +" And   Se.BusCode <> '' " ; 
            StrSql = StrSql +" And   Se.SellCode <> '' " ;
            StrSql = StrSql + " And   Se.Mbid2 IS not null ";
            
            ReCnt = 0;
            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;

            
            pg1.Value = 0; pg1.Maximum = ReCnt + 1;                         
            //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)            
            while(sr.Read ())
            {
                LevelCnt = 0;
               
                SMbid = sr.GetValue(2).ToString();
                SMbid2 = int.Parse(sr.GetValue(3).ToString());
                SMbid_Name = sr.GetValue(8).ToString();
                OrderNumber = sr.GetValue(9).ToString();
                                
                TotalPV = double.Parse(sr.GetValue(0).ToString());
                
                BusCode = sr.GetValue(6).ToString();  //판매센타기준
                Mem_BusCode= sr.GetValue(7).ToString();  //회원등록센타기준
                               
                LevelCnt++;

                if (Clo_Mem.ContainsKey(BusCode) == true)
                {
                    if (Clo_Mem[BusCode].LeaveDate == "" && Clo_Mem[BusCode].StopDate == "" )                        
                    {
                        Allowance1 = 0;                        
                        Allowance1 = TotalPV * 0.03;
                        
                        StrSql = "Update tbl_ClosePay_100 SET ";
                        StrSql = StrSql + " Allowance1 = Allowance1 +  " + Allowance1;
                        StrSql = StrSql + " Where   BusCode = '" + BusCode + "'";
                            
                        //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                        t_qu[t_qu_Cnt] = StrSql;
                        t_qu_Cnt++;


                        StrSql = "INSERT INTO tbl_Close_DownPV_PV_100";
                        StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                        StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV , R_DownPV , ";
                        StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber , BusCode ) ";

                        StrSql = StrSql + "Values(";
                        StrSql = StrSql + "'" + ToEndDate + "','" + SMbid + "'";
                        StrSql = StrSql + "," + SMbid2 + ",'" + SMbid_Name + "',";
                        StrSql = StrSql + "'" + Clo_Mem[BusCode].Mbid + "'," + Clo_Mem[BusCode].Mbid2  + ",'" + Clo_Mem[BusCode].M_Name + "',";
                        StrSql = StrSql + Allowance1 + ", " + TotalPV + " , 1 , 0 " ;
                        StrSql = StrSql + ",'1' ,'" + OrderNumber + "','" + BusCode + "')";

                        //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                        t_qu[t_qu_Cnt] = StrSql;
                        t_qu_Cnt++;
                        
                    }
                }
                                    

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




        private void Give_Allowance2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 15    ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            //////테스트용임 다 지워야함. 아래 주석을 열어죽소
            ////StrSql = "Update tbl_ClosePay_100 SET ";
            ////StrSql = StrSql + " Allowance2 = Sum_PV_2 * 0.2 ";

            ////StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            ////StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            ////StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            ////StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";
                        
            ////StrSql = StrSql + " Where Sum_PV_1 >= Sum_PV_2 ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update tbl_ClosePay_100 SET ";
            ////StrSql = StrSql + " Allowance2 = Sum_PV_1 * 0.2 ";

            ////StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            ////StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            ////StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            ////StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";
            ////StrSql = StrSql + " Where Sum_PV_1 < Sum_PV_2 ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();





            StrSql = "Update tbl_ClosePay_100 SET ";
            StrSql = StrSql + " Allowance2 = Sum_PV_2 * 0.2 ";

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            StrSql = StrSql + " Where (CurPoint = 2 OR CurGrade >= 2 ) ";
            StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_100 SET ";
            StrSql = StrSql + " Allowance2 = Sum_PV_1 * 0.2 ";

            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            StrSql = StrSql + " Where (CurPoint = 2 OR CurGrade >= 2 ) ";
            StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_100 SET ";
            StrSql = StrSql + " Allowance2 = Sum_PV_2 * 0.1";

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            StrSql = StrSql + " Where (CurPoint = 1 ) ";
            StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_100 SET ";
            StrSql = StrSql + " Allowance2 = Sum_PV_1 * 0.1 ";

            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            StrSql = StrSql + " Where (CurPoint = 1 ) ";
            StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        
    
    
    
            StrSql = "Update tbl_ClosePay_100 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 500000 "  ;
            StrSql = StrSql + " Where Allowance2 > 500000 "  ;
            StrSql = StrSql + " And CurGrade < 2 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
             StrSql = "Update tbl_ClosePay_100 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 1000000 "  ;
            StrSql = StrSql + " Where Allowance2 > 1000000 "  ;
            StrSql = StrSql + " And CurGrade = 2 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
             StrSql = "Update tbl_ClosePay_100 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 2500000 "  ;
            StrSql = StrSql + " Where Allowance2 > 2500000 "  ;
            StrSql = StrSql + " And CurGrade = 3 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
             StrSql = "Update tbl_ClosePay_100 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 5000000 "  ;
            StrSql = StrSql + " Where Allowance2 > 5000000 "  ;  ;
            StrSql = StrSql + " And CurGrade = 4 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
      
             StrSql = "Update tbl_ClosePay_100 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 10000000 "  ;
            StrSql = StrSql + " Where Allowance2 > 10000000 "  ;
            StrSql = StrSql + " And CurGrade = 5 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
    
              StrSql = "Update tbl_ClosePay_100 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 15000000 "  ;
            StrSql = StrSql + " Where Allowance2 > 15000000 "  ;
            StrSql = StrSql + " And CurGrade = 6 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
      
              StrSql = "Update tbl_ClosePay_100 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 25000000 "  ;
            StrSql = StrSql + " Where Allowance2 > 25000000 "  ;
            StrSql = StrSql + " And CurGrade = 7 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
      
              StrSql = "Update tbl_ClosePay_100 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 30000000 "  ;
            StrSql = StrSql + " Where Allowance2 > 30000000 "  ;
            StrSql = StrSql + " And CurGrade = 8 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
              StrSql = "Update tbl_ClosePay_100 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 50000000 "  ;
            StrSql = StrSql + " Where Allowance2 > 50000000 "  ;
            StrSql = StrSql + " And CurGrade = 9 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

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
            StrSql = StrSql + "  From tbl_ClosePay_100    ";
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


                 


                StrSql = "Update tbl_ClosePay_100 SET ";
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












        private void Give_Allowance3(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
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
            StrSql = StrSql + "  From tbl_ClosePay_100    ";
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
                                StrSql = "Update tbl_ClosePay_100 SET ";
                                StrSql = StrSql + " Allowance3 = Allowance3 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_100";
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


            StrSql = " Select OrderNumber , Re_BaseOrderNumber ,  TotalPV , tbl_SalesDetail.Mbid , tbl_SalesDetail.Mbid2 , tbl_SalesDetail.M_Name , SellDate  ";            
            StrSql = StrSql + " From tbl_SalesDetail (nolock)   ";            
            StrSql = StrSql + " WHERE TotalPV < 0   ";
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
                TotalPV = -double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();
                SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();

                StrSql = "SELECT  Sell_DownPV , SaveMbid, SaveMbid2, SaveName ,SortOrder " ;
                StrSql = StrSql + " From tbl_Close_DownPV_PV_100  ";
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

                    StrSql = "SELECT  TotalPV  From tbl_SalesDetail (nolock)  ";
                    StrSql = StrSql + " WHERE   OrderNumber = '" + Re_BaseOrderNumber + "'";

                    DataSet ds3 = new DataSet();
                    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds3);
                    int ReCnt3 = Search_Connect.DataSet_ReCount;

                    if (ReCnt3 > 0)
                        Base_PV = double.Parse(ds3.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());


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
                                StrSql = StrSql + "'" + SellDate + "'," + Return_Pay + "," + Return_Pay + ",100";
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


            StrSql = "Update tbl_ClosePay_100 SET " ;
            StrSql = StrSql + " Sum_Return_Take_Pay = ISNULL(B.A1, 0 )   " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_100  A, " ;
    
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
            StrSql = "Update tbl_ClosePay_100 SET " ;
            StrSql = StrSql + " Sum_Return_DedCut_Pay = ISNULL(B.A1, 0 )   " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_100  A, " ;
    
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




            StrSql = "Update tbl_ClosePay_100 SET ";
            StrSql = StrSql + " Sum_Return_Remain_Pay = Sum_Return_Take_Pay - Sum_Return_DedCut_Pay " ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }

        private void CalculateTruePayment(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 8    ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";



            StrSql = "Update tbl_ClosePay_100 SET ";
            StrSql = StrSql + " Etc_Pay = ISNULL(B.A1, 0 )   ";
            StrSql = StrSql + " FROM  tbl_ClosePay_100  A, ";

            StrSql = StrSql + " (Select    Sum(Apply_Pv) A1,  mbid ,mbid2   ";
            StrSql = StrSql + " From tbl_Sham_Pay (nolock) ";
            StrSql = StrSql + " Where   Apply_Date >='" + FromEndDate  + "'";
            StrSql = StrSql + " And     Apply_Date <= '" + ToEndDate  + "' ";
            StrSql = StrSql + " And     SortKind2 = '100' ";
            StrSql = StrSql + " Group By mbid ,mbid2 ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid  = B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2  = B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);




            StrSql = "Update tbl_ClosePay_100 Set "    ;
            StrSql = StrSql + " SumAllAllowance = Allowance1 + Allowance2 + Etc_Pay  ";
            StrSql = StrSql + " Where Allowance1 + Allowance2 +  Etc_Pay   > 0";
    
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    

            //'''---반품으로 해서 차감시킬 금액이 아직 남아잇다.
            StrSql = "Update tbl_ClosePay_100 Set "    ;
            StrSql = StrSql + " Cur_DedCut_Pay = SumAllAllowance "    ;
            StrSql = StrSql + ",SumAllAllowance = 0 "    ;
            StrSql = StrSql + " Where SumAllAllowance  > 0"    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay > 0 "    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay >= SumAllAllowance "    ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_100 Set "    ;
            StrSql = StrSql + " Cur_DedCut_Pay = Sum_Return_Remain_Pay "    ;
            StrSql = StrSql + ",SumAllAllowance = SumAllAllowance - Sum_Return_Remain_Pay "    ;
            StrSql = StrSql + " Where SumAllAllowance  > 0"    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay > 0 "    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay < SumAllAllowance "    ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
        
            StrSql = "Update tbl_ClosePay_100 Set "    ;
            StrSql = StrSql + " SumAllAllowance = Convert(int,(SumAllAllowance) /10) * 10  "    ;
            StrSql = StrSql + " Where SumAllAllowance  > 0 "    ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            
           
            StrSql = "Update tbl_ClosePay_100 Set "    ;
            StrSql = StrSql + " InComeTax = Convert(int,(SumAllAllowance * 0.03) /10) * 10  "    ;
            StrSql = StrSql + " Where SumAllAllowance  > 0 "    ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_100 Set "    ;
            StrSql = StrSql + " ResidentTax = Convert(int,(InComeTax * 0.1) /10) * 10  "    ;
            StrSql = StrSql + " Where SumAllAllowance  > 0 "    ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            

            StrSql = "Update tbl_ClosePay_100 Set "    ;
            StrSql = StrSql + " TruePayment = ((SumAllAllowance - InComeTax - ResidentTax) / 10 ) * 10 "    ;
            StrSql = StrSql + " Where SumAllAllowance  > 0 ";

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
            StrSql = StrSql + " From tbl_ClosePay_100    ";
            StrSql = StrSql + " WHERE Cur_DedCut_Pay > 0 ";

            ReCnt = 0;
            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;
                
            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            int Mbid2 = 0, Cut_Pay = 0, Top_SW = 0, T_Pay = 0, TSw = 0, RR_Cut_Pay = 0, T_index = 0 ;
            string Mbid = "", Re_BaseOrderNumber = "", M_Name = "";  
          
            
            //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            while (sr.Read ())
            {
                Cut_Pay = int.Parse(sr.GetValue (0).ToString ());
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
            pg1.Value = 0; pg1.Maximum = 3;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "INSERT INTO tbl_CloseTotal_100 " ;
            StrSql = StrSql +" (ToEndDate,      FromEndDate,   PayDate  ,RecordID,RecordTime   ) Values (" ;
            StrSql = StrSql +"'" + ToEndDate + "','" + FromEndDate + "','" + PayDate + "'" ;
            StrSql = StrSql +",'" + cls_User.gid + "',Convert(Varchar(25),GetDate(),21)" ;
            StrSql = StrSql +" )" ; 
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_CloseTotal_100 SET ";
            StrSql = StrSql +"  TotalSellAmount =ISNULL(B.A1,0) ";
            StrSql = StrSql +" ,TotalSellPv =ISNULL(B.A2,0) ";
            StrSql = StrSql + " FROM  tbl_CloseTotal_100  A, ";    
            StrSql = StrSql +" (Select ";
            StrSql = StrSql +" Sum(TotalPrice) A1, Sum(TotalPV) A2 From  tbl_SalesDetail (nolock) ";
            StrSql = StrSql +" Where SellDate  >= '" + FromEndDate + "'";
            StrSql = StrSql +" And  SellDate  <= '" + ToEndDate + "'";    
            StrSql = StrSql +" ) B";
            StrSql = StrSql +" Where A.ToEndDate ='" + ToEndDate + "'" ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }






        private void tbl_CloseTotal_Put2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_CloseTotal_100 SET " ;
            StrSql = StrSql + "  Allowance1 =ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,Allowance2 =ISNULL(B.A2,0) " ;
            ////StrSql = StrSql + " ,Allowance3 =ISNULL(B.A3,0) " ;
            ////StrSql = StrSql + " ,Allowance4 =ISNULL(B.A4,0) " ;
            ////StrSql = StrSql + " ,Allowance5 =ISNULL(B.A5,0) " ;
            ////StrSql = StrSql + " ,Allowance6 =ISNULL(B.A6,0) " ;
            ////StrSql = StrSql + " ,Allowance7 =ISNULL(B.A7,0) " ;
            ////StrSql = StrSql + " ,Allowance8 =ISNULL(B.A8,0) ";
            ////StrSql = StrSql + " ,Allowance9 =ISNULL(B.A9,0) " ;
            ////StrSql = StrSql + " ,Allowance10 =ISNULL(B.A10,0) ";

            ////StrSql = StrSql + " ,Allowance11 =ISNULL(B.A11,0) ";
            ////StrSql = StrSql + " ,Allowance12 =ISNULL(B.A12,0) ";
            ////StrSql = StrSql + " ,Allowance13 =ISNULL(B.A13,0) ";
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
        //    StrSql = StrSql + " ,Allowance28 =ISNULL(B.A28,0) " ;
            StrSql = StrSql + " ,Allowance29 =ISNULL(B.A29,0) " ;  //반품공제
            StrSql = StrSql + " ,Allowance30 =ISNULL(B.A30,0) " ;  //기타보너스

            StrSql = StrSql + " ,SumAllowance=ISNULL(B.AS1,0) " ;
            StrSql = StrSql + " ,SumInComeTax=ISNULL(B.AS2,0) " ;
            StrSql = StrSql + " ,SumResidentTax=ISNULL(B.AS3,0) " ;
            StrSql = StrSql + " ,SumTruePayment=ISNULL(B.AS4,0) " ;

            StrSql = StrSql + " FROM  tbl_CloseTotal_100  A, " ;

            StrSql = StrSql + " (Select " ;
            StrSql = StrSql + " Sum(convert(float,Allowance1)) AS A1 ,Sum(convert(float,Allowance2)) AS A2 ";
          //  StrSql = StrSql + ",Sum(convert(float,Allowance3)) AS A3 ,Sum(convert(float,Allowance4)) AS A4 ";
          //  StrSql = StrSql + ",Sum(convert(float,Allowance5)) AS A5 ,Sum(convert(float,Allowance6)) AS A6";
         //   StrSql = StrSql + ",Sum(convert(float,Allowance7)) AS A7 ,Sum(convert(float,Allowance8)) AS A8 ";
      //      StrSql = StrSql + ",Sum(convert(float,Allowance9)) AS A9 ,Sum(convert(float,Allowance10)) AS A10 ";
        //'
         //   StrSql = StrSql + ",Sum(convert(float,Allowance11)) AS A11,Sum(convert(float,Allowance12)) AS A12 ";
        //    StrSql = StrSql + ",Sum(convert(float,Allowance13)) AS A13,Sum(convert(float,Allowance14)) AS A14 ";
        //    StrSql = StrSql + ",Sum(Allowance15) AS A15 " //,Sum(Allowance16) AS A16" ;
        //    StrSql = StrSql + ",Sum(Allowance17) AS A17,Sum(Allowance18) AS A18 " ;
        //    StrSql = StrSql + ",Sum(Allowance19) AS A19,Sum(Allowance20) AS A20 " ;
        
        //    StrSql = StrSql + ",Sum(Allowance21) AS A21,Sum(Allowance22) AS A22 " ;
        //    StrSql = StrSql + ",Sum(Allowance23) AS A23,Sum(Allowance24) AS A24 " ;
        //    StrSql = StrSql + ",Sum(Allowance25) AS A25 " //,Sum(Allowance26) AS A26" ;
        //    StrSql = StrSql + ",Sum(Allowance27) AS A27,Sum(Allowance28) AS A28 " ;

           // StrSql = StrSql + ",Sum(convert(float,Allowance2_cut)) AS A28";
            StrSql = StrSql + ",Sum(convert(float,Cur_DedCut_Pay)) AS A29";
            StrSql = StrSql + ",Sum(convert(float,Etc_Pay)) AS A30 ";


            StrSql = StrSql + ",Sum(convert(float,SumAllAllowance)) AS AS1,Sum(convert(float,InComeTax)) AS AS2 ";
            StrSql = StrSql + ",Sum(convert(float,ResidentTax)) AS AS3,Sum(convert(float,TruePayment)) AS AS4 ";
            StrSql = StrSql + " From tbl_ClosePay_100 " ;
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


            StrSql = "Update tbl_CloseTotal_100 Set "  ;
            StrSql = StrSql + "  Allowance1Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance1 > 0),0) "  ;
            StrSql = StrSql + " ,Allowance2Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance2 > 0),0) "  ;
            //StrSql = StrSql + " ,Allowance3Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance3 > 0),0) "  ;
            //StrSql = StrSql + " ,Allowance4Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance4 > 0),0) "  ;
            //StrSql = StrSql + " ,Allowance5Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance5 > 0),0) "  ;
            //StrSql = StrSql + " ,Allowance6Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance6 > 0),0) "  ;
            //StrSql = StrSql + " ,Allowance7Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance7> 0),0) "  ;
            //StrSql = StrSql + " ,Allowance8Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance8 > 0),0) "  ;
            //StrSql = StrSql + " ,Allowance9Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance9 > 0),0) "  ;
            //StrSql = StrSql + " ,Allowance10Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance10 > 0),0) ";

            //StrSql = StrSql + " ,Allowance11Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance11 > 0),0) "  ;
            //StrSql = StrSql + " ,Allowance12Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance12 > 0),0) "  ;
            //StrSql = StrSql + " ,Allowance13Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance13 > 0),0) "  ;
    ////    StrSql = StrSql + " ,Allowance14Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance14 > 0),0) "  ;
        ////    StrSql = StrSql + " ,Allowance15Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance15 > 0),0) "  ;
        ////'    StrSql = StrSql + " ,Allowance16Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance16 > 0),0) "  ;
        ////'    StrSql = StrSql + " ,Allowance17Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance17> 0),0) "  ;
        ////'    StrSql = StrSql + " ,Allowance18Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance18 > 0),0) "  ;
        //////'    StrSql = StrSql + " ,Allowance19Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance19 > 0),0) "  ;
        ////'    StrSql = StrSql + " ,Allowance20Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance20 > 0),0) "  ;
        ////'
        ////    StrSql = StrSql + " ,Allowance21Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance21 > 0),0) "  ;
        ////    StrSql = StrSql + " ,Allowance22Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance22 > 0),0) "  ;
        ////    StrSql = StrSql + " ,Allowance23Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance23 > 0),0) "  ;
        ////    StrSql = StrSql + " ,Allowance24Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance24 > 0),0) "  ;
        ////    StrSql = StrSql + " ,Allowance25Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance25 > 0),0) "  ;
        //    StrSql = StrSql + " ,Allowance26Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance26 > 0),0) "  ;
        //    StrSql = StrSql + " ,Allowance27Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance27> 0),0) "  ;
            //StrSql = StrSql + " ,Allowance28Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Allowance2_cut > 0),0) ";
            StrSql = StrSql + " ,Allowance29Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Cur_DedCut_Pay > 0),0) ";
            StrSql = StrSql + " ,Allowance30Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where Etc_Pay > 0),0) ";
            StrSql = StrSql + " ,SumAllowanceCount = ISNULL((Select Count(Mbid) From tbl_ClosePay_100 Where SumAllAllowance > 0),0) " ; 
            
            StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "'" ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_CloseTotal_100 Set " ; 
            StrSql = StrSql + "  Allowance1Rate = (Allowance1 /(TotalSellPV)) * 100  "  ;
            StrSql = StrSql + " ,Allowance2Rate = (Allowance2 /(TotalSellPV)) * 100  "  ;
            //StrSql = StrSql + " ,Allowance3Rate = (Allowance3 /(TotalSellPV)) * 100  "  ;
            //StrSql = StrSql + " ,Allowance4Rate = (Allowance4 /(TotalSellPV)) * 100  "  ;
            //StrSql = StrSql + " ,Allowance5Rate = (Allowance5 /(TotalSellPV)) * 100  "  ;
            //StrSql = StrSql + " ,Allowance6Rate = (Allowance6 /(TotalSellPV)) * 100  "  ;
            //StrSql = StrSql + " ,Allowance7Rate = (Allowance7 /(TotalSellPV)) * 100  "  ;
            //StrSql = StrSql + " ,Allowance8Rate = (Allowance8 /(TotalSellPV)) * 100  "  ;
            //StrSql = StrSql + " ,Allowance9Rate = (Allowance9 /(TotalSellPV)) * 100  "  ;
            //StrSql = StrSql + " ,Allowance10Rate = (Allowance10 /(TotalSellPV)) * 100  "  ;
        
            //StrSql = StrSql + " ,Allowance11Rate = (Allowance11 /(TotalSellPV)) * 100  "  ;
            //StrSql = StrSql + " ,Allowance12Rate = (Allowance12 /(TotalSellPV)) * 100  "  ;
            //StrSql = StrSql + " ,Allowance13Rate = (Allowance13 /(TotalSellPV)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance14Rate = (Allowance14 /(TotalSellPV)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance15Rate = (Allowance15 /(TotalSellPV)) * 100  "  ;
        ////'    StrSql = StrSql + " ,Allowance16Rate = (Allowance16 /(TotalSellPV)) * 100  "  ;
        ////'    StrSql = StrSql + " ,Allowance17Rate = (Allowance17 /(TotalSellPV)) * 100  "  ;
        ////'    StrSql = StrSql + " ,Allowance18Rate = (Allowance18 /(TotalSellPV)) * 100  "  ;
        //////    StrSql = StrSql + " ,Allowance19Rate = (Allowance19 /(TotalSellPV)) * 100  "  ;
        ////'    StrSql = StrSql + " ,Allowance20Rate = (Allowance20 /(TotalSellPV)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance21Rate = (Allowance21 /(TotalSellPV)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance22Rate = (Allowance22 /(TotalSellPV)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance23Rate = (Allowance23 /(TotalSellPV)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance24Rate = (Allowance24 /(TotalSellPV)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance25Rate = (Allowance25 /(TotalSellPV)) * 100  "  ;
        //    StrSql = StrSql + " ,Allowance26Rate = (Allowance26 /(TotalSellPV)) * 100  "  ;
        //    StrSql = StrSql + " ,Allowance27Rate = (Allowance27 /(TotalSellPV)) * 100  "  ;

            //StrSql = StrSql + " ,Allowance28Rate = (Allowance28 /(TotalSellPV)) * 100  "  ;
            StrSql = StrSql + " ,Allowance29Rate = (Allowance29 /(TotalSellPV)) * 100  ";
            StrSql = StrSql + " ,Allowance30Rate = (Allowance30 /(TotalSellPV)) * 100  "  ;

            StrSql = StrSql + " ,SumAllowanceRate = (SumAllowance /(TotalSellPV)) * 100  "  ;
    
            StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " And (TotalSellPV) > 0";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 

        }




        private void MakeModForCheckRequirement1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


             StrSql = "Insert into tbl_ClosePay_100_Mod select "  ;
            StrSql = StrSql + " '" + ToEndDate + "','" + FromEndDate + "','" + PayDate + "', * , 0,'',''"  ;
            StrSql = StrSql + " From tbl_ClosePay_100 "  ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Insert into tbl_ClosePay_100_Sell_Mod select "  ;
            //StrSql = StrSql + " '" + ToEndDate + "',* From tbl_ClosePay_100_Sell";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

        }



        private void ReadyNewForCheckRequirement1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Delete From tbl_ClosePay_100 " ;

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
