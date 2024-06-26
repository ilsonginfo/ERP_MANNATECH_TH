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
    public partial class frmClose_2 : Form
    {
           cls_Grid_Base cgb = new cls_Grid_Base();
        private int Data_Set_Form_TF = 0;
        private string base_db_name = "tbl_CloseTotal_02";
        private string FromEndDate = ""; private string ToEndDate = ""; private string PayDate = "", PayDate2  = "" ;
        private int From_Load_TF = 0;
        private int Cl_F_TF = 0, ReCnt = 0, MaxLevel = 0, N_MaxLevel = 0, Retry_MaxLevel = 0, Retry_N_MaxLevel = 0;

        Dictionary<string, cls_Close_Mem> Clo_Mem = new Dictionary<string, cls_Close_Mem>();
        Dictionary<string, cls_Close_Sell> Clo_Sell = new Dictionary<string, cls_Close_Sell>();

        cls_Close_Sell[] C_Sell;


        private int Chang_Date_Close_Ver02 = 20200101;
        

        cls_Connect_DB Search_Connect = new cls_Connect_DB();
        SqlConnection Search_Conn = null;

        int Chang_Base_CloDAte = 20150118;

        double Sum_T_PV_001 = 0, Sum_T_PV_01 = 0;

        double Kor_Pay = 0; 

        public frmClose_2()
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

            radioB_Year.Checked = true; 
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

               // DateTime dt = DateTime.Parse(FromEndDate);


                string TPDate = "";
                TPDate = FromEndDate.Substring(0, 4) + '-' + FromEndDate.Substring(4, 2) + '-' + FromEndDate.Substring(6, 2);

                DateTime dt = DateTime.Parse(TPDate);

                DateTime TodayDate = new DateTime();
                TodayDate = DateTime.Parse(TPDate);
                              


                string week = string.Empty;

                switch (dt.DayOfWeek)
                {

                    case DayOfWeek.Monday:
                        TPDate = TodayDate.AddDays(1).ToString("yyyy-MM-dd").Replace("-", "");
                        break;

                    case DayOfWeek.Tuesday:
                        TPDate = TodayDate.AddDays(0).ToString("yyyy-MM-dd").Replace("-", "");
                        break;

                    case DayOfWeek.Wednesday:
                        TPDate = TodayDate.AddDays(6).ToString("yyyy-MM-dd").Replace("-", "");
                        break;

                    case DayOfWeek.Thursday:
                        TPDate = TodayDate.AddDays(5).ToString("yyyy-MM-dd").Replace("-", "");
                        break;

                    case DayOfWeek.Friday:
                        TPDate = TodayDate.AddDays(4).ToString("yyyy-MM-dd").Replace("-", "");
                        break;

                    case DayOfWeek.Saturday:
                        TPDate = TodayDate.AddDays(3).ToString("yyyy-MM-dd").Replace("-", "");
                        break;

                    case DayOfWeek.Sunday:
                        TPDate = TodayDate.AddDays(2).ToString("yyyy-MM-dd").Replace("-", "");
                        break;

                    default:
                        break;

                }


                //TodayDate = DateTime.Parse(TPDate);
                //TPDate = TodayDate.AddDays(-1).ToString("yyyy-MM-dd").Replace("-", "");
                ToEndDate = TPDate;


                //if (int.Parse(FromEndDate.Substring(6, 2)) >= 1 && int.Parse(FromEndDate.Substring(6, 2)) <= 8)                
                //    ToEndDate = FromEndDate.Substring(0, 6) + "08";

                //if (int.Parse(FromEndDate.Substring(6, 2)) >= 9 && int.Parse(FromEndDate.Substring(6, 2)) <= 15)
                //    ToEndDate = FromEndDate.Substring(0, 6) + "15";

                //if (int.Parse(FromEndDate.Substring(6, 2)) >= 16 && int.Parse(FromEndDate.Substring(6, 2)) <= 23)
                //    ToEndDate = FromEndDate.Substring(0, 6) + "23";

                //if (int.Parse(FromEndDate.Substring(6, 2)) >= 23)
                //{
                //    string TPDate = "";
                //    TPDate = FromEndDate.Substring(0, 4) + '-' + FromEndDate.Substring(4, 2) + '-' + "01";
                //    DateTime TodayDate = new DateTime();
                //    TodayDate = DateTime.Parse(TPDate);
                //    TPDate = TodayDate.AddMonths(1).ToString("yyyy-MM-dd");

                //    TodayDate = DateTime.Parse(TPDate);
                //    TPDate = TodayDate.AddDays(-1).ToString("yyyy-MM-dd").Replace("-", "");
                //    ToEndDate = TPDate; 
                //}



                //if (FromEndDate == "20180912" )
                //    ToEndDate = "20180916"; 
                //else
                //    ToEndDate = TPDate; 

                txt_To.Text = ToEndDate;
                Close_Base_Work();

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
            Tsql = "Select Isnull (Max(ToEndDate),'') From  tbl_CloseTotal_02 (nolock) ";

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
                Tsql = "Select Isnull(Min(SellDate_2),'')  From   tbl_SalesDetail (nolock) Where Ga_Order = 0  And ReturnTF = 1 And SellDate_2 >= '20190416' ";

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
        
            StrSql = "select Isnull(Count(Mbid),0) from tbl_SalesDetail (nolock) ";
            StrSql = StrSql + " Where SellDate_2 >= '" + FromEndDate + "'";
            StrSql = StrSql + " And SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " And TotalPV + TotalCV > 0 ";
            StrSql = StrSql + " And  Ga_Order = 0 ";
                                         
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            txt_SellCnt.Text = "0";
            if (ReCnt != 0)            
                txt_SellCnt.Text =  ds.Tables[base_db_name].Rows[0][0].ToString()                                                                                                                                                                                                                                                       ;


            StrSql = "select Isnull(Count(Mbid),0) from tbl_SalesDetail  (nolock)  ";
            StrSql = StrSql + " Where SellDate_2 >= '" + FromEndDate + "'";
            StrSql = StrSql + " And SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " And TotalPV + TotalCV < 0 ";
            StrSql = StrSql + " And  Ga_Order = 0 ";
            
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
            PayDate = TodayDate.AddDays (14).ToString("yyyy-MM-dd").Replace("-", "");
            
            //PayDate = PayDate.Substring(0, 4) + '-' + PayDate.Substring(4, 2) + "15";

            //if (ToEndDate.Substring(6, 2) == "08")
            //    PayDate = ToEndDate.Substring(0, 4) + '-' + ToEndDate.Substring(4, 2) + "23";

            //if (ToEndDate.Substring(6, 2) == "15")
            //{
            //    PayDate = ToEndDate.Substring(0, 4) + '-' + ToEndDate.Substring(4, 2) + '-' + "01";
            //    DateTime TodayDate = new DateTime();
            //    TodayDate = DateTime.Parse(PayDate);
            //    PayDate = TodayDate.AddMonths(1).ToString("yyyy-MM-dd");

            //    TodayDate = DateTime.Parse(PayDate);
            //    PayDate = TodayDate.AddDays(-1).ToString("yyyy-MM-dd").Replace("-", "");                
            //}


            //if (ToEndDate.Substring(6, 2) == "23")
            //{
            //    PayDate = ToEndDate.Substring(0, 4) + '-' + ToEndDate.Substring(4, 2) + '-' + ToEndDate.Substring(6, 2);
            //    DateTime TodayDate = new DateTime();
            //    TodayDate = DateTime.Parse(PayDate);
            //    PayDate = TodayDate.AddMonths(1).ToString("yyyy-MM-dd").Replace("-", "");
            //    PayDate = PayDate.Substring(0, 4) + '-' + PayDate.Substring(4, 2) + "08";
            //}

            //if (FromEndDate.Substring(6, 2) == "24" || PayDate == "" )
            //{
            //    PayDate = ToEndDate.Substring(0, 4) + '-' + ToEndDate.Substring(4, 2) + '-' + ToEndDate.Substring(6, 2);
            //    DateTime TodayDate = new DateTime();
            //    TodayDate = DateTime.Parse(PayDate);
            //    PayDate = TodayDate.AddMonths(1).ToString("yyyy-MM-dd").Replace("-", "");
            //    PayDate = PayDate.Substring(0, 4) + '-' + PayDate.Substring(4, 2) + "15";
            //}



            

            mtxtPayDate.Text = PayDate;

            txt_From.Refresh();
            txt_To.Refresh();
            mtxtPayDate.Refresh();
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






            string StrSql = "";

            StrSql = "Select Isnull( Count(ToEndDate),0 )  From CKDPHARM_Ga_Close.dbo.tbl_Close_Log_Ga (nolock) ";            

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            ReCnt = 0;
            Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text);
            ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {
                if (int.Parse(ds.Tables[base_db_name].Rows[0][0].ToString()) > 0)
                {
                    MessageBox.Show("현재 가마감이 정산 중입니다. 가마감 완료후 다시 시도해 주십시요." + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    return false;
                }                
            }


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

            radioB_Month.Checked = true; 

            if (Search_Check_TextBox_Error() == false) return;

            //if ("ilsong_7" != cls_User.gid)
            //{
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_Start"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            //}
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

                //if ("ilsong_7" != cls_User.gid)
                //{
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_End"));
                //}
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
                    //if ("ilsong_7" != cls_User.gid)
                    //{
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_End"));
                    //}
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


                    System.Threading.Thread.Sleep(1000);

                    cls_Connect_DB Temp_Re_Connect = new cls_Connect_DB();
                    Temp_Re_Connect.Connect_Ga_Close_DB();
                    SqlConnection Re_Conn = Temp_Re_Connect.Conn_Ga_Close();
                    SqlTransaction Re_tran = Re_Conn.BeginTransaction();


                    string StrSql = " EXEC Usp_Close_Pro_00_Ga_Close_Ready ";
                    Temp_Re_Connect.Insert_Data(StrSql, Re_Conn, Re_tran);


                    Re_tran.Commit();
                    Re_tran.Dispose(); Temp_Re_Connect.Close_Ga_DB();

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
            
            if (Clo_Mem != null)
            {
                Clo_Mem.Clear(); Clo_Mem = null;
            }

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


            //if (int.Parse(FromEndDate.Substring(6, 2)) >= 1 && int.Parse(FromEndDate.Substring(6, 2)) <= 8)
            //    ToEndDate = FromEndDate.Substring(0, 6) + "08";

            //if (int.Parse(FromEndDate.Substring(6, 2)) >= 9 && int.Parse(FromEndDate.Substring(6, 2)) <= 15)
            //    ToEndDate = FromEndDate.Substring(0, 6) + "15";

            //if (int.Parse(FromEndDate.Substring(6, 2)) >= 16 && int.Parse(FromEndDate.Substring(6, 2)) <= 23)
            //    ToEndDate = FromEndDate.Substring(0, 6) + "23";

            //if (int.Parse(FromEndDate.Substring(6, 2)) >= 23)
            //{
            //    string TPDate = "";
            //    TPDate = FromEndDate.Substring(0, 4) + '-' + FromEndDate.Substring(4, 2) + '-' + "01";
            //    DateTime TodayDate = new DateTime();
            //    TodayDate = DateTime.Parse(TPDate);
            //    TPDate = TodayDate.AddMonths(1).ToString("yyyy-MM-dd");

            //    TodayDate = DateTime.Parse(TPDate);
            //    TPDate = TodayDate.AddDays(-1).ToString("yyyy-MM-dd").Replace("-", "");
            //    ToEndDate = TPDate;
            //}

            string TPDate = "";
            TPDate = FromEndDate.Substring(0, 4) + '-' + FromEndDate.Substring(4, 2) + '-' + FromEndDate.Substring(6, 2) ;
            DateTime TodayDate = new DateTime();
            TodayDate = DateTime.Parse(TPDate);
            TPDate = TodayDate.AddDays (6).ToString("yyyy-MM-dd").Replace("-", ""); ;
            ToEndDate = TPDate;

            txt_To.Text = ToEndDate;
            Close_Base_Work();

            txt_From.Refresh();
            txt_To.Refresh();
            mtxtPayDate.Refresh();


            //if ("ilsong_7" == cls_User.gid)
            //{
            //    EventArgs e = null;
            //    butt_Pay_Click(butt_Pay, e);
            //}
        }



        private void Close_Work_Real(cls_Connect_DB Temp_Connect , SqlConnection Conn, SqlTransaction tran)
        {
            pg2.Minimum = 0;            pg2.Maximum = 61;
            pg2.Step = 1;               pg2.Value = 0;
            pg1.Step = 1;
            
            //Kor_Pay = int.Parse(txtB1.Text);
            Kor_Pay = double.Parse(txtB1.Text);


            Cl_F_TF = 1;
            PayDate = mtxtPayDate.Text.Replace ("-","").Trim ();
             
            //pg2.PerformStep() ; pg2.Refresh();

            
            //pg2.PerformStep(); pg2.Refresh();s

            
            ////Put_Member_Base_Info_2014_1001(Temp_Connect, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();


           
            //pg2.PerformStep(); pg2.Refresh();

           
            //pg2.PerformStep(); pg2.Refresh();

            
            //pg2.PerformStep(); pg2.Refresh();

            
            //pg2.PerformStep(); pg2.Refresh();

            string Strsql = "";


           //가마감들 때문에 가마감상에서 작업했던 내역들을 삭제 처리하고 들어간다.
            Strsql = "EXEC Usp_Close_Pro_10_Real_Close_Ready  '" + FromEndDate + "','" + ToEndDate + "'";             
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();


            string StrSql = "";
            string Search_ToEndDate = "", Return_ToEndDate = ""; ;

            //StrSql = " Select Top 1 ToEndDate  From  tbl_CloseTotal_02 (nolock) order by ToEndDate desc   ";
            //ReCnt = 0;
            //DataSet Dset4 = new DataSet();
            //Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
            //ReCnt = Search_Connect.DataSet_ReCount;

            //if (ReCnt > 0)
            //{
            //    Search_ToEndDate = Dset4.Tables[base_db_name].Rows[0]["ToEndDate"].ToString();
            //}

            //StrSql = " Select Top 1 ToEndDate  From  SOLRX_Return.dbo.tbl_CloseTotal_02 (nolock) order by ToEndDate desc   ";
            //ReCnt = 0;
            //DataSet Dset55 = new DataSet();
            //Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset55);
            //ReCnt = Search_Connect.DataSet_ReCount;

            //if (ReCnt > 0)
            //{
            //    Return_ToEndDate = Dset55.Tables[base_db_name].Rows[0]["ToEndDate"].ToString();
            //}



            //if (Search_ToEndDate == Return_ToEndDate)
            //{
            //    StrSql = "Delete From tbl_ClosePay_02  ";

            //    Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //    StrSql = "Insert into tbl_ClosePay_02 Select * From SOLRX_Return.dbo.tbl_ClosePay_02 (nolock)  ";

            //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //}
             






            //마감돌리는 동안 매출 등록을 못하도록 하기 위해서 제일 먼저 체크 테이블인 집계 테이블을 만든다.
            Strsql = " EXEC Usp_Close_Pro_500_C_Put_tbl_CloseTotal_Put1 '" + FromEndDate + "','" + ToEndDate + "','" + PayDate + "','" + cls_User.gid + "'";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();



            StrSql = "Update tbl_CloseTotal_02 SET ";
            StrSql = StrSql + "  Temp01 = " + double.Parse(txtB1.Text) ;
            StrSql = StrSql + " , Temp02 = " + double.Parse(txtB2.Text);
            if (radioB_Year.Checked == true )
                StrSql = StrSql + " ,Temp11 = 1 ";
            else
                StrSql = StrSql + " ,Temp11 = 2 ";
            StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();



            Strsql = " EXEC Usp_Close_Pro_100_A_001 '" + FromEndDate  +"','" + ToEndDate  +"'";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            //Make_Close_Table(Temp_Connect, Conn, tran);    
            //Put_Leave_StopDate(Temp_Connect, Conn, tran);
            //Put_Member_Base_Info(Temp_Connect, Conn, tran);

            
            Strsql = " EXEC Usp_Close_Pro_100_A_Sell_002 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            //Put_Sell_Date(Temp_Connect, Conn, tran);


            Strsql = " EXEC Usp_Close_Pro_100_A_Sell_003 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            //Put_SellPV(Temp_Connect, Conn, tran);dk
            //Put_DayPV(Temp_Connect, Conn, tran);


            //이부분은 가마감쪽에는 없음.. 시간이 오래걸리는 프로세스 이기때문에.. 소스상에서만 처리하기로함.
            //반픔관련 프로세스가 들어간다. //올스타팩보너스 환수를 위해서 직급을 다시 계산하기 위함.
            //반픔관련 프로세스가 들어간다. //올스타팩보너스 환수를 위해서 직급을 다시 계산하기 위함.
            //Retry_ToEndDate(Temp_Connect, Conn, tran);
            //반픔관련 프로세스가 들어간다. //올스타팩보너스 환수를 위해서 직급을 다시 계산하기 위함.
            //반픔관련 프로세스가 들어간다. //올스타팩보너스 환수를 위해서 직급을 다시 계산하기 위함.


            
            Strsql = " EXEC Usp_Close_Pro_100_B_LevelCnt '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            //Put_LevelCnt_Update(Temp_Connect, Conn, tran);
            //Put_LevelCnt_Update_Nom(Temp_Connect, Conn, tran);

            if (int.Parse(FromEndDate) >= Chang_Date_Close_Ver02)
                Strsql = " EXEC Usp_Close_Pro_100_B_ReqTF1_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";   //4주매출 합산 유지 부분 미팅후 보완해야함
            else
                Strsql = " EXEC Usp_Close_Pro_100_B_ReqTF1 '" + FromEndDate + "','" + ToEndDate + "'";   //4주매출 합산 유지 부분 미팅후 보완해야함
            Temp_Connect.Insert_Data(Strsql, Conn, tran);


            
           // ReqTF1(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            Strsql = " EXEC Usp_Close_Pro_100_B_ReqTF2_Mon_Grade '" + FromEndDate + "','" + ToEndDate + "'";   //
            Temp_Connect.Insert_Data(Strsql, Conn, tran);            
            pg2.PerformStep(); pg2.Refresh();


            
            


            Strsql = " EXEC Usp_Close_Pro_100_C_Grade_Grade_ReqTF2 '" + FromEndDate + "','" + ToEndDate + "'";   //당월의 매출 실적으로 해서 다음달의 유지 여부를 체크한다. 직급 관련 사항임.
            Temp_Connect.Insert_Data(Strsql, Conn, tran);

            if (FromEndDate.Substring(0, 6) != ToEndDate.Substring(0, 6))
            {
                Strsql = " EXEC Usp_Close_Pro_100_C_Grade_Grade_ReqTF2_M2 '" + FromEndDate + "','" + ToEndDate + "'";   //당월의 매출 실적으로 해서 다음달의 유지 여부를 체크한다. 직급 관련 사항임.
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
            }
            pg2.PerformStep(); pg2.Refresh();



            if (int.Parse(FromEndDate) >= Chang_Date_Close_Ver02)
                Strsql = " EXEC Usp_Close_Pro_100_B2_ReqTF2_OneGrade_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";   //
            else
                Strsql = " EXEC Usp_Close_Pro_100_B2_ReqTF2_OneGrade '" + FromEndDate + "','" + ToEndDate + "'";   //
            Temp_Connect.Insert_Data(Strsql, Conn, tran);




            Strsql = " EXEC Usp_Close_Pro_100_C_ReqTF1_Dir_Nom_SaveG '" + FromEndDate + "','" + ToEndDate + "'";   //직추천 좌우를 구한다
            Temp_Connect.Insert_Data(Strsql, Conn, tran);            
            //--------------------------------------------------------------



            //반픔관련 프로세스가 들어간다. //올스타팩보너스 환수를 위해서 직급을 다시 계산하기 위함.
            //Retry_ToEndDate(Temp_Connect, Conn, tran);




            //Strsql = " EXEC Usp_Close_Pro_70_CurGrade_OrgGrade_Put '" + FromEndDate + "','" + ToEndDate + "'";
            //Temp_Connect.Insert_Data(Strsql, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();
            //CurGrade_OrgGrade_Put(Temp_Connect, Conn, tran);           
            //GiveShamGrade(Temp_Connect, Conn, tran);
            //--------------------------------------------------------------


            //Strsql = " EXEC Usp_Close_Pro_Put_Down_SumPV '" + FromEndDate + "','" + ToEndDate + "'";
            //Temp_Connect.Insert_Data(Strsql, Conn, tran);
            // Put_Down_SumPV(Temp_Connect, Conn, tran);  //추천 라인으로 해서 하선 매출을 잡아준다. 대소실적 따집 4주간의 매출 실적등등
            // pg2.PerformStep(); pg2.Refresh();




            //Put_Down_SumPV_Save(Temp_Connect, Conn, tran);  //추천 라인으로 해서 하선 매출을 잡아준다. 대소실적 따집 4주간의 매출 실적등등
            //pg2.PerformStep(); pg2.Refresh();



            //Strsql = " EXEC Usp_Close_Pro_80_GradeUpLine_ReqTF1 '" + FromEndDate + "','" + ToEndDate + "'";
            //Temp_Connect.Insert_Data(Strsql, Conn, tran);

            ////Strsql = " EXEC Usp_Close_Pro_GradeUpLine_ReqTF1__2 '" + FromEndDate + "','" + ToEndDate + "'";            
            ////Temp_Connect.Insert_Data(Strsql, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();
            //GradeUpLine_ReqTF1(Temp_Connect, Conn, tran);  //하선 활동회원수를 넣어둔다 후원그룹사응로 해서            
            //--------------------------------------------------------------


            //int S_LevelCnt = N_MaxLevel;

            //while (0 <= S_LevelCnt)
            //{

            //    if (S_LevelCnt < MaxLevel)
            //    {
            //        pg2.Maximum = pg2.Maximum + 13;
            //    }


            int S_LevelCnt = -1;


        //하선 좌우 유지자수를 넣는다 직추천인 대상으로 해서
        //--N_TF_GradeCnt6_1 ,N_TF_GradeCnt6_2 유지직급 관련해서 하선 유지자숫자를 체크하는 부분이 없어짐 41주차 부터 이전부터 아니엇다고함.
        //2018-02-27
        //Strsql = " EXEC Usp_Close_Pro_Sol_Cnt__N_ReqTF1 '" + FromEndDate + "','" + ToEndDate + "'";
        //Temp_Connect.Insert_Data(Strsql, Conn, tran); 


        //2018-03-19 현조직상태로 해서 구하는 부분이기때문에  50분 단위로 프로시져상에서 구해서 회원테이블에 넣고.. 그거를 가져오는 방식으로 처리하기로함.
        //Strsql = " EXEC Usp_Close_Pro_Sol_Cnt__N '" + FromEndDate + "','" + ToEndDate + "'";
        //Temp_Connect.Insert_Data(Strsql, Conn, tran);



        //Strsql = " EXEC Usp_Close_Pro_Put_Down_4Week_PV_01 '" + FromEndDate + "','" + ToEndDate + "',0 "; //일반 매출만 잡아준다.
        //Strsql = " EXEC Usp_Close_Pro_Put_Down_4Week_PV_01_Mem '" + FromEndDate + "','" + ToEndDate + "',0 "; //일반 매출만 잡아준다.
        //Strsql = " EXEC Usp_Close_Pro_Put_Down_4Week_PV_02 '" + FromEndDate + "','" + ToEndDate + "',0 "; //일반 매출만 잡아준다.
        // Temp_Connect.Insert_Data(Strsql, Conn, tran);





        Re_Grade_10:
            Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_10 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            if (Check_UP_Grade_TF(10, Temp_Connect, Conn, tran) == true) goto Re_Grade_10;
            pg2.PerformStep(); pg2.Refresh();


        Re_Grade_20:
            Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_20 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            if (Check_UP_Grade_TF(10, Temp_Connect, Conn, tran) == true) goto Re_Grade_20;
            pg2.PerformStep(); pg2.Refresh();


        Re_Grade_30:
            Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_30 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            if (Check_UP_Grade_TF(30, Temp_Connect, Conn, tran) == true) goto Re_Grade_30;
            pg2.PerformStep(); pg2.Refresh();




        Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_40 '" + FromEndDate + "','" + ToEndDate + "'";
        Temp_Connect.Insert_Data(Strsql, Conn, tran);
        pg2.PerformStep(); pg2.Refresh();

        Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_50 '" + FromEndDate + "','" + ToEndDate + "'";
        Temp_Connect.Insert_Data(Strsql, Conn, tran);
        pg2.PerformStep(); pg2.Refresh();                       
        
        Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_60 '" + FromEndDate + "','" + ToEndDate + "'";
        Temp_Connect.Insert_Data(Strsql, Conn, tran);                
        pg2.PerformStep(); pg2.Refresh();            
                  
        Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_70 '" + FromEndDate + "','" + ToEndDate + "'";
        Temp_Connect.Insert_Data(Strsql, Conn, tran);               
        pg2.PerformStep(); pg2.Refresh();

            if (int.Parse(FromEndDate) >= Chang_Date_Close_Ver02)
            {
                Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_80_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_90_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();


                Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_100_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_110_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();


                Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_120_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_130_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_140_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_150_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();
            }
            else
            {
                Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_80 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_90 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();


                Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_100 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_110 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();


                Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_120 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_130 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_140 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_150 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();
            }

        if (FromEndDate.Substring(0, 6) != ToEndDate.Substring(0, 6))
        {
            Re_Grade_10_M2:
                Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_10_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                if (Check_UP_Grade_TF(10, Temp_Connect, Conn, tran) == true) goto Re_Grade_10_M2;
                pg2.PerformStep(); pg2.Refresh();

            Re_Grade_20_M2:
                Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_20_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                if (Check_UP_Grade_TF(10, Temp_Connect, Conn, tran) == true) goto Re_Grade_20_M2;
                pg2.PerformStep(); pg2.Refresh();


            Re_Grade_30_M2:
                Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_30_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                if (Check_UP_Grade_TF(30, Temp_Connect, Conn, tran) == true) goto Re_Grade_30_M2;
                pg2.PerformStep(); pg2.Refresh();


                Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_40_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_50_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();


                Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_60_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_70_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                if (int.Parse(FromEndDate) >= Chang_Date_Close_Ver02)
                {
                    Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_80_M2_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();

                    Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_90_M2_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();

                    Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_100_M2_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();

                    Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_110_M2_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();

                    Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_120_M2_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();

                    Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_130_M2_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();


                    Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_140_M2_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();

                    Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_150_M2_Ver02 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();
                }
                else
                {
                    Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_80_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();

                    Strsql = " EXEC Usp_Close_Pro_200_GiveGrade_90_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();

                    Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_100_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();

                    Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_110_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();

                    Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_120_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();

                    Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_130_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();


                    Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_140_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();

                    Strsql = " EXEC Usp_Close_Pro_210_GiveGrade_150_M2 '" + FromEndDate + "','" + ToEndDate + "'";
                    Temp_Connect.Insert_Data(Strsql, Conn, tran);
                    pg2.PerformStep(); pg2.Refresh();
                }
            }



                ////       //Strsql = " EXEC Usp_Close_Pro_90_GradeUpLine__N_TF_100 '" + FromEndDate + "','" + ToEndDate + "',100";
                ////       //Temp_Connect.Insert_Data(Strsql, Conn, tran); 
                ////  // Re_Grade_120:
                ////   //    Strsql = " EXEC Usp_Close_Pro_GradeUpLine__3_100 '" + FromEndDate + "','" + ToEndDate + "',100";
                ////   //Temp_Connect.Insert_Data(Strsql, Conn, tran);
                ////       Strsql = " EXEC Usp_Close_Pro_90_GradeUpLine__N_100 '" + FromEndDate + "','" + ToEndDate + "',100";
                ////       Temp_Connect.Insert_Data(Strsql, Conn, tran);


                ////       Strsql = " EXEC Usp_Close_Pro_90_GiveGrade12 '" + FromEndDate + "','" + ToEndDate + "'";
                ////       Temp_Connect.Insert_Data(Strsql, Conn, tran);
                ////       //GradeUpLine__3(100, Temp_Connect, Conn, tran, S_LevelCnt);
                ////       //GiveGrade12(Temp_Connect, Conn, tran, S_LevelCnt);                
                ////     //  if (Check_UP_Grade_TF(120, Temp_Connect, Conn, tran) == true) goto Re_Grade_120;
                ////       pg2.PerformStep(); pg2.Refresh();


                ////       //Strsql = " EXEC Usp_Close_Pro_90_GradeUpLine__N_TF_110 '" + FromEndDate + "','" + ToEndDate + "',110";
                ////       //Temp_Connect.Insert_Data(Strsql, Conn, tran);
                //////   Re_Grade_130:
                ////       Strsql = " EXEC Usp_Close_Pro_90_GradeUpLine__N_110 '" + FromEndDate + "','" + ToEndDate + "',110";
                ////       Temp_Connect.Insert_Data(Strsql, Conn, tran);

                ////       Strsql = " EXEC Usp_Close_Pro_90_GiveGrade13 '" + FromEndDate + "','" + ToEndDate + "'";
                ////       Temp_Connect.Insert_Data(Strsql, Conn, tran);

                ////       //if (Check_UP_Grade_TF(130, Temp_Connect, Conn, tran) == true) goto Re_Grade_130;
                ////       pg2.PerformStep(); pg2.Refresh();


                ////       //Strsql = " EXEC Usp_Close_Pro_90_GradeUpLine__N_TF_120 '" + FromEndDate + "','" + ToEndDate + "',120";
                ////       //Temp_Connect.Insert_Data(Strsql, Conn, tran);
                //// //  Re_Grade_140:
                ////       Strsql = " EXEC Usp_Close_Pro_90_GradeUpLine__N_120 '" + FromEndDate + "','" + ToEndDate + "',120";
                ////       Temp_Connect.Insert_Data(Strsql, Conn, tran);

                ////       Strsql = " EXEC Usp_Close_Pro_90_GiveGrade14 '" + FromEndDate + "','" + ToEndDate + "'";
                ////       Temp_Connect.Insert_Data(Strsql, Conn, tran);

                ////      // if (Check_UP_Grade_TF(140, Temp_Connect, Conn, tran) == true) goto Re_Grade_140;
                ////       pg2.PerformStep(); pg2.Refresh();


                //Strsql = " EXEC Usp_Close_Pro_90_GradeUpLine__N_30_ETC '" + FromEndDate + "','" + ToEndDate + "',30";
                //Temp_Connect.Insert_Data(Strsql, Conn, tran);

                //Strsql = " EXEC Usp_Close_Pro_90_GradeUpLine__N_40_ETC '" + FromEndDate + "','" + ToEndDate + "',40";
                //Temp_Connect.Insert_Data(Strsql, Conn, tran);

                //Strsql = " EXEC Usp_Close_Pro_90_GradeUpLine__N_45_ETC '" + FromEndDate + "','" + ToEndDate + "',45";
                //Temp_Connect.Insert_Data(Strsql, Conn, tran);

                //Strsql = " EXEC Usp_Close_Pro_90_GradeUpLine__N_50_ETC '" + FromEndDate + "','" + ToEndDate + "',50";
                //Temp_Connect.Insert_Data(Strsql, Conn, tran);

                //    GradeUpLine__3(10, Temp_Connect, Conn, tran, S_LevelCnt);
                //    GradeUpLine__3(20, Temp_Connect, Conn, tran, S_LevelCnt);
                //    GradeUpLine__3(30, Temp_Connect, Conn, tran, S_LevelCnt);
                //    GradeUpLine__3(40, Temp_Connect, Conn, tran, S_LevelCnt);
                //    GradeUpLine__3(50, Temp_Connect, Conn, tran, S_LevelCnt);
                //    GradeUpLine__3(60, Temp_Connect, Conn, tran, S_LevelCnt);
                //    GradeUpLine__3(70, Temp_Connect, Conn, tran, S_LevelCnt);
                //    GradeUpLine__3(80, Temp_Connect, Conn, tran, S_LevelCnt);
                //    GradeUpLine__3(90, Temp_Connect, Conn, tran, S_LevelCnt);
                //    GradeUpLine__3(100, Temp_Connect, Conn, tran, S_LevelCnt);
                //    GradeUpLine__3(110, Temp_Connect, Conn, tran, S_LevelCnt);
                //    GradeUpLine__3(120, Temp_Connect, Conn, tran, S_LevelCnt);
                //    pg2.PerformStep(); pg2.Refresh();


                //    S_LevelCnt--;
                //}




                //Put_cls_Close_Mem(Temp_Connect, Conn, tran);
                //pg2.PerformStep(); pg2.Refresh();

                //현재 PV가  CV   이고 (수당지급에 적용)                현재 CV가 QV로 적용했음(승급이나 자격조건따지는데 적용) .           
                ////--------------------------------------------------------------




                //Strsql = " EXEC Usp_Close_Pro_Put_Down_PV_01_Be '" + FromEndDate + "','" + ToEndDate + "',0 "; //일반 매출만 잡아준다.
                //Strsql = " EXEC Usp_Close_Pro_Put_Down_PV_01_Be_Mem '" + FromEndDate + "','" + ToEndDate + "',0 "; //일반 매출만 잡아준다.
                //Strsql = " EXEC Usp_Close_Pro_Put_Down_PV_01_Biz '" + FromEndDate + "','" + ToEndDate + "',0 "; //랜탈 매출만 잡아준다.
                //Strsql = " EXEC Usp_Close_Pro_Put_Down_PV_01_Biz_Mem '" + FromEndDate + "','" + ToEndDate + "',0 "; //랜탈 매출만 잡아준다.
                //Strsql = " EXEC Usp_Close_Pro_Put_Down_PV_01_Mem '" + FromEndDate + "','" + ToEndDate + "',0 "; //일반 매출만 잡아준다.


                Strsql = " EXEC Usp_Close_Pro_300_A_Put_Down_PV_01 '" + FromEndDate + "','" + ToEndDate + "',0 "; //일반 매출만 잡아준다.
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();
               

                //반품 관련해서는 위로 올려주지 않는다 어차피 반품 관련 일일 재정산이 돌기 때문에
                // Strsql = " EXEC Usp_Close_Pro_Put_Down_PV_Re '" + FromEndDate + "','" + ToEndDate + "'"; //반품 내역을 위로 올려주면서 빼준다.
                // Temp_Connect.Insert_Data(Strsql, Conn, tran);
                //Put_Down_PV_Re(Temp_Connect, Conn, tran);  //반품 내역을 위로 올려주면서 빼준다.
                //pg2.PerformStep(); pg2.Refresh();


                Strsql = " EXEC Usp_Close_Pro_300_B_Put_Down_PV_02 '" + FromEndDate + "','" + ToEndDate + "'"; //하선 누적 관련 사항을 합산 한다.반품처리도 여기서함.
                Temp_Connect.Insert_Data(Strsql, Conn, tran);
                //Put_Down_PV_02(Temp_Connect, Conn, tran);  //하선 누적 관련 사항을 합산 한다.
                pg2.PerformStep(); pg2.Refresh();

      


            ////////GradeUpLine_ReqTF1(Temp_Connect, Conn, tran);  //하선 활동회원수를 넣어둔다 후원그룹사응로 해서
            ////////Give_Allowance1(Temp_Connect, Conn, tran);  //후원보너스 n분해
            //--------------------------------------------------------------
                        
            
            Strsql = " EXEC Usp_Close_Pro_400_A_Give_Allowance1_Real '" + FromEndDate + "','" + ToEndDate + "'"; //후원보너스 그대로  1
            Temp_Connect.Insert_Data(Strsql, Conn, tran);            
            pg2.PerformStep(); pg2.Refresh();

            Strsql = " EXEC Usp_Close_Pro_400_A_Give_Allowance2 '" + FromEndDate + "','" + ToEndDate + "'"; //후원보너스에 대한 추천매칭보너스  2
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();


            Strsql = " EXEC Usp_Close_Pro_400_A_Give_Allowance3 '" + FromEndDate + "','" + ToEndDate + "'"; //추천보너스 1스타 이상 
            Temp_Connect.Insert_Data(Strsql, Conn, tran);            
            pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------



            //Strsql = " EXEC Usp_Close_Pro_920_Give_Allowance6 '" + FromEndDate + "','" + ToEndDate + "'"; //직급달성 보너스 6
            //Temp_Connect.Insert_Data(Strsql, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();


            //if (int.Parse(FromEndDate) >= 20181201)
            //{
            //    Strsql = " EXEC Usp_Close_Pro_920_Give_Allowance7_New '" + FromEndDate + "','" + ToEndDate + "'"; //후원보너스에 대한 추천매칭보너스  2
            //    Temp_Connect.Insert_Data(Strsql, Conn, tran);

            //    Strsql = " EXEC Usp_Close_Pro_920_Give_Allowance8_New '" + FromEndDate + "','" + ToEndDate + "'"; //후원보너스에 대한 추천매칭보너스  2
            //    Temp_Connect.Insert_Data(Strsql, Conn, tran);
            //}
            ////--------------------------------------------------------------



            //Strsql = " EXEC Usp_Close_Pro_Give_Allowance3 '" + FromEndDate + "','" + ToEndDate + "'"; //맴버보너스 소비자매출 추천인한태
            //Strsql = " EXEC Usp_Close_Pro_Give_Allowance3_Biz '" + FromEndDate + "','" + ToEndDate + "'"; //맴버보너스 소비자매출 추천인한태            
            //Strsql = " EXEC Usp_Close_Pro_Give_Allowance6 '" + FromEndDate + "','" + ToEndDate + "'"; //직급달성보너스 본인매출 패키지 빼고 가져가기            
            //Strsql = " EXEC Usp_Close_Pro_Give_Allowance7 '" + FromEndDate + "','" + ToEndDate + "'"; //추천보너스 추천한사람 패키지 구매에 대해선 본인이 가져감.
            //Strsql = " EXEC Usp_Close_Pro_Give_Allowance4 '" + FromEndDate + "','" + ToEndDate + "'"; //올스타팩보너스
            //Strsql = " EXEC Usp_Close_Pro_Give_Allowance4_2 '" + FromEndDate + "','" + ToEndDate + "'"; //올스타팩보너스   13주후에 지급 되는 2차분  5번임.            
            //Strsql = " EXEC Usp_Close_Pro_Give_Allowance8 '" + FromEndDate + "','" + ToEndDate + "'"; //리더십보너스
            //Strsql = " EXEC Usp_Close_Pro_Give_Allowance9 '" + FromEndDate + "','" + ToEndDate + "'"; //PB보너스
            //Strsql = " EXEC Usp_Close_Pro_Give_Allowance14 '" + FromEndDate + "','" + ToEndDate + "'"; //패키지 보너스

            //if (PayDate.Substring(6, 2) == "15")
            //{
            //    Give_Allowance3_20160329(Temp_Connect, Conn, tran);  //올스타팩보너스
            //}


            //--------------------------------------------------------------




            //string SDate3 = "";

            //SDate3 = FromEndDate;
            //while (int.Parse(SDate3) <= int.Parse(ToEndDate))
            //{
            //    Give_Allowance1_Day(Temp_Connect, Conn, tran, SDate3);

            //    DateTime dt = DateTime.Parse(SDate3.Substring(0, 4) + "-" + SDate3.Substring(4, 2) + "-" + SDate3.Substring(6, 2));
            //    SDate3 = dt.AddDays(1).ToShortDateString().Replace("-", "");
            //}

            //Give_Allowance1(Temp_Connect, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------



            //Nom_Point_2(Temp_Connect, Conn, tran);

            //Put_Down_PV_01(Temp_Connect, Conn, tran);                                               
            //Put_Down_PV_Re(Temp_Connect, Conn, tran);                        
            //Put_Down_PV_02(Temp_Connect, Conn, tran);  //본인 누적 소실적 적용 부분은 신마케팅 적용되면서 없어짐.            

            ////Give_Allowance1_20150201(Temp_Connect, Conn, tran);  //후원수당 소 1   , 대 4 로 한다. 대실적만 지급 상한선이 잇다.
            //Give_Allowance1_20150216(Temp_Connect, Conn, tran);  //후원수당 소 1   , 대 4 로 한다. 대실적만 지급 상한선이 잇다.
            //Give_Allowance1_Cut_20150201(Temp_Connect, Conn, tran); 




            //Put_cls_Close_Mem(Temp_Connect, Conn, tran);  //후원수당 받은 사람만 받을수 있다고 해서 다시.. 가져옴


            //    Put_cls_Close_Mem(Temp_Connect, Conn, tran);  //후원수당 받은 사람이 누구인지를 알아오기 위함.

            //    Give_Allowance1_20150310(Temp_Connect, Conn, tran);  //매칭수당 추천 조직으로 지급한다.
            //    Give_Allowance5_20150310(Temp_Connect, Conn, tran);  //매칭수당 후원 조직으로 지급한다.

            //    Give_Allowance1_20150201(Temp_Connect, Conn, tran);  //매칭수당 추천 조직으로 지급한다.
            //    Give_Allowance5_20150201(Temp_Connect, Conn, tran);  //매칭수당 후원 조직으로 지급한다.

            //Give_Allowance3(Temp_Connect, Conn, tran);  //추천보너스
            //Give_Allowance3_20150201(Temp_Connect, Conn, tran);  //추천보너스



            // Strsql = " EXEC Usp_Close_Pro_Put_Return_Pay '" + FromEndDate + "','" + ToEndDate + "'"; // 반품으로 인한 공제 금액 만들기 우대고객 커미션 관련
            //Temp_Connect.Insert_Data(Strsql, Conn, tran);
            //Put_Return_Pay(Temp_Connect, Conn, tran);
            //  pg2.PerformStep(); pg2.Refresh();


            //Strsql = " EXEC Usp_Close_Pro_Put_Return_Pay_1 '" + FromEndDate + "','" + ToEndDate + "'"; // 반품으로 인한 공제 금액 만들기 후원보너스 관련
            //Temp_Connect.Insert_Data(Strsql, Conn, tran);
            // Put_Return_Pay_All_1(Temp_Connect, Conn, tran);   //팀매칭에 대한 반품 공제 금액을 계산처리한다.//
            // pg2.PerformStep(); pg2.Refresh();



            ////이부분은 가마감쪽에는 없음.. 시간이 오래걸리는 프로세스 이기때문에.. 소스상에서만 처리하기로함.
            ////반픔관련 프로세스가 들어간다. //올스타팩보너스 환수를 위해서 직급을 다시 계산하기 위함.
            ////반픔관련 프로세스가 들어간다. //올스타팩보너스 환수를 위해서 직급을 다시 계산하기 위함.
            //Retry_ToEndDate(Temp_Connect, Conn, tran);
            ////반픔관련 프로세스가 들어간다. //올스타팩보너스 환수를 위해서 직급을 다시 계산하기 위함.
            ////반픔관련 프로세스가 들어간다. //올스타팩보너스 환수를 위해서 직급을 다시 계산하기 위함.



            //Strsql = " EXEC Usp_Close_Pro_930_Chang_RetunPay_Table_Pre '" + FromEndDate + "','" + ToEndDate + "'";
            //Temp_Connect.Insert_Data(Strsql, Conn, tran);

            //Put_Start_35_End_35_002(Temp_Connect, Conn, tran);
            //--------------------------------------------------------------
            //Strsql = " EXEC Usp_Close_Pro_Put_Return_Pay_Ga '" + FromEndDate + "','" + ToEndDate + "' ";
            //Temp_Connect.Insert_Data(Strsql, Conn, tran);            
            //Chang_RetunPay_Table(Temp_Connect, Conn, tran);                       
            //Put_Sum_Return_Remain_Pay(Temp_Connect, Conn, tran);


            

            Strsql = " EXEC Usp_Close_Pro_400_A1_Put_Sum_Return_Remain_Pay_Pre '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);


            Strsql = " EXEC Usp_Close_Pro_400_B_Put_Sum_Return_Remain_Pay '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);           
            pg2.PerformStep(); pg2.Refresh();
            ////--------------------------------------------------------------
            
            //--------------------------------------------------------------
            Strsql = " EXEC Usp_Close_Pro_400_C_CalculateTruePayment '" + FromEndDate + "','" + ToEndDate + "', 2 ";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);            
            pg2.PerformStep(); pg2.Refresh();

           
            Strsql = " EXEC Usp_Close_Pro_500_A_Chang_RetunPay_Table '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);



            Strsql = " EXEC Usp_Close_Pro_500_B1_Chang_RetunPay_Table_DED '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------



            Strsql = " EXEC Usp_Close_Pro_500_C_Put_tbl_CloseTotal_Put1 '" + FromEndDate + "','" + ToEndDate + "','" + PayDate  + "','" + cls_User.gid  + "'";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);         
           // tbl_CloseTotal_Put1(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = "Update tbl_CloseTotal_02 SET ";
            StrSql = StrSql + "  Temp01 = " + double.Parse(txtB1.Text);
            if (radioB_Year.Checked == true)
                StrSql = StrSql + " ,Temp11 = 1 ";
            else
                StrSql = StrSql + " ,Temp11 = 2 ";
            StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            Strsql = " EXEC Usp_Close_Pro_500_C_Put_tbl_CloseTotal_Put2 '" + FromEndDate + "','" + ToEndDate + "' ";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            //tbl_CloseTotal_Put2(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();


            Strsql = " EXEC Usp_Close_Pro_500_C_Put_tbl_CloseTotal_Put3 '" + FromEndDate + "','" + ToEndDate + "' ";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            //tbl_CloseTotal_Put3(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------



            


            //--------------------------------------------------------------
            Strsql = " EXEC Usp_Close_Pro_500_D_MakeModForCheckRequirement1 '" + FromEndDate + "','" + ToEndDate + "','" + PayDate + "' ";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            //MakeModForCheckRequirement1(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            if (int.Parse(FromEndDate) >= Chang_Date_Close_Ver02)
                Strsql = " EXEC Usp_Close_Pro_500_D_ReadyNewForCheckRequirement1_Ver02 '" + FromEndDate + "','" + ToEndDate + "','" + PayDate + "' ";
            else
                Strsql = " EXEC Usp_Close_Pro_500_D_ReadyNewForCheckRequirement1 '" + FromEndDate + "','" + ToEndDate + "','" + PayDate + "' ";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            //ReadyNewForCheckRequirement1(Temp_Connect, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            Strsql = " EXEC Usp_Close_Pro_500_E_Check_Close_Gid '" + FromEndDate + "','" + ToEndDate + "','" + cls_User.gid + "' ";
            Temp_Connect.Insert_Data(Strsql, Conn, tran);
            //Check_Close_Gid(Temp_Connect, Conn, tran,1,0);
            pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------


            //진마감이 다 돌았음을 알린다... 가마감 돌아도 되도록 체크를 한다.
            StrSql = " UpDate tbl_CloseTotal_02 SET  Real_FLAG  = 0 Where ToEndDate = '" + ToEndDate  + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
           


            
            //-----가마감 디비 상에서 동일 날짜의 가마감을 삭제 처리 한다.---
            StrSql = "Delete From CKDPHARM_Ga_Close.dbo.tbl_ClosePay_02 Where RecordMakeDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From CKDPHARM_Ga_Close.dbo.tbl_ClosePay_02_Sell Where RecordMakeDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
          
            StrSql = "Delete From CKDPHARM_Ga_Close.dbo.tbl_ClosePay_02_Mod Where ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From CKDPHARM_Ga_Close.dbo.tbl_ClosePay_02_Sell_Mod Where ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            
            StrSql = "Delete From CKDPHARM_Ga_Close.dbo.tbl_Close_DownPV_ALL_02 Where EndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From CKDPHARM_Ga_Close.dbo.tbl_Close_DownPV_PV_02 Where EndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From CKDPHARM_Ga_Close.dbo.tbl_Sales_Put_Return_Pay Where ToEndDate >= '" + ToEndDate + "' And  Cl_TF = 2";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From CKDPHARM_Ga_Close.dbo.tbl_Sales_Put_Return_Pay_DED Where ToEndDate >= '" + ToEndDate + "' And  Cl_TF = 2";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From CKDPHARM_Ga_Close.dbo.tbl_CloseTotal_02 Where ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            
            StrSql = "Delete From CKDPHARM_Ga_Close.dbo.tbl_ClosePay_02_Ded_P_Detail_Mod Where Cur_ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From CKDPHARM_Ga_Close.dbo.tbl_ClosePay_02_Ded_P_Mod Where ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //-----가마감 디비 상에서 동일 날짜의 가마감을 삭제 처리 한다.---
           



            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


        }


        private void Make_Close_Table(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Minimum = 0; pg1.Maximum = 40; pg1.Refresh(); 
            
            pg1.Value = 10; ; pg1.Refresh(); 
            //pg1.PerformStep(); pg1.Refresh(); 

            string StrSql = "";

            StrSql = "INSERT INTO tbl_ClosePay_02 (Mbid,Mbid2,RecordMakeDate)  ";
            StrSql = StrSql + " Select   A.Mbid,A.Mbid2,  '" + ToEndDate + "' From tbl_Memberinfo AS A  (nolock)  ";
            StrSql = StrSql + " LEFT Join tbl_ClosePay_02 AS B ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 ";
            StrSql = StrSql + " Where b.Mbid Is Null " ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
            pg1.Value = 20; pg1.Refresh(); 


            StrSql = "INSERT INTO tbl_ClosePay_02_Sell (Mbid,Mbid2,SellCode , RecordMakeDate)  ";
            StrSql = StrSql + " Select  distinct A.Mbid,A.Mbid2, A.SellCode, '" + ToEndDate + "' From tbl_SalesDetail AS A   (nolock) ";
            StrSql = StrSql + " LEFT Join tbl_ClosePay_02_Sell AS B ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 And A.SellCode = B.SellCode ";
            StrSql = StrSql + " Where  A.SellDate_2 >= '" + FromEndDate + "'";
            StrSql = StrSql + " And    A.SellDate_2 <= '" + ToEndDate + "'" ;
            StrSql = StrSql + " And b.Mbid Is Null ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
            pg1.Value = 30;  pg1.Refresh(); 


             StrSql = "INSERT INTO tbl_ClosePay_02_Sell (Mbid,Mbid2,SellCode, RecordMakeDate) ";
            StrSql = StrSql + " Select  distinct A.Mbid,A.Mbid2, A.SellCode,   '" + ToEndDate + "'  From tbl_Sham_Sell AS A   (nolock) ";
            StrSql = StrSql + " LEFT Join tbl_ClosePay_02_Sell AS B ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 And A.SellCode = B.SellCode";            
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
    
            StrSql = "Update tbl_ClosePay_02 SET StopDate = ISNULL(B.PayStop_Date,'')" ;
           StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
           StrSql = StrSql + " (";
           StrSql = StrSql + " Select    PayStop_Date,Mbid,Mbid2   From tbl_Memberinfo   (nolock) ";
           StrSql = StrSql + " Where PayStop_Date <= '" + ToEndDate + "'";
           StrSql = StrSql + " And   PayStop_Date <>'' ";
           StrSql = StrSql + " ) B";
           StrSql = StrSql + "  Where a.Mbid = b.Mbid And a.Mbid2 = b.Mbid2";
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 

    
            StrSql = "Update tbl_ClosePay_02 SET LeaveDate=ISNULL(B.LeaveDate,'')";
           StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
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
    
            StrSql = "Update tbl_ClosePay_02 SET" ;
            StrSql = StrSql +" BankCode=ISNULL(B.BankCode,'')";
            StrSql = StrSql +" ,Cpno=ISNULL(B.Cpno,'')";
            StrSql = StrSql +" ,BankAcc=ISNULL(B.bankaccnt,'')";
            StrSql = StrSql +" ,BankOwner=ISNULL(B.BankOwner,'')";
            StrSql = StrSql +" ,M_Name=ISNULL(B.M_Name,'')";
            StrSql = StrSql +" ,BusCode=ISNULL(B.businesscode,'')";
    
            //StrSql = StrSql +" ,ED_Date=ISNULL(B.ED_Date,'')"
    
            StrSql = StrSql +" ,Saveid=ISNULL(B.Saveid,'')";
            StrSql = StrSql +" ,Saveid2=ISNULL(B.Saveid2,0)";
            StrSql = StrSql +" ,LineCnt=ISNULL(B.LineCnt,0)";
            
            StrSql = StrSql +" ,Nominid=ISNULL(B.Nominid,'')";
            StrSql = StrSql +" ,Nominid2=ISNULL(B.Nominid2,0)";
            StrSql = StrSql +" ,N_LineCnt=ISNULL(B.N_LineCnt,0)";
    
        //    StrSql = StrSql +" ,BaseMbid=ISNULL(B.BaseMbid,'')"
        //    StrSql = StrSql +" ,BaseMbid2=ISNULL(B.BaseMbid2,0)"             
    
           StrSql = StrSql +" ,Sell_Mem_TF = ISNULL(B.Sell_Mem_TF,0)" ;

           //StrSql = StrSql + " ,GiBu_=ISNULL(B.GiBu_,0)";        

            StrSql = StrSql +" ,RegTime=  replace(ISNULL(B.regtime,''),'-','')";
            StrSql = StrSql +"  FROM  tbl_ClosePay_02  A,";
    
            StrSql = StrSql +" (";
            StrSql = StrSql +" Select   BankCode,Cpno,bankaccnt,BankOwner,M_Name,businesscode,ED_Date,";
            StrSql = StrSql +" Saveid,Saveid2,LineCnt,Nominid,Nominid2,N_LineCnt,";
            StrSql = StrSql +" Mbid,Mbid2,regtime , Sell_Mem_TF ";
            StrSql = StrSql +"  From tbl_Memberinfo   (nolock)   ";
            StrSql = StrSql +" ) B";
            StrSql = StrSql +" Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And a.Mbid2 = b.Mbid2";
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
        }


        private void Put_Member_Base_Info_2014_1001(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();

            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 SET";
            StrSql = StrSql + " BankCode=ISNULL(B.BankCode,'')";
            StrSql = StrSql + " ,Cpno=ISNULL(B.Cpno,'')";
            StrSql = StrSql + " ,BankAcc=ISNULL(B.BankAcc,'')";
            StrSql = StrSql + " ,BankOwner=ISNULL(B.BankOwner,'')";
            StrSql = StrSql + " ,M_Name=ISNULL(B.M_Name,'')";
            StrSql = StrSql + " ,BusCode=ISNULL(B.BusCode,'')";

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
            StrSql = StrSql + " ,GiBu_=ISNULL(B.GiBu_,0)";
            StrSql = StrSql + " ,LEaveDate=ISNULL(B.LEaveDate,'')";
            StrSql = StrSql + " ,StopDate=ISNULL(B.StopDate,'')";

            StrSql = StrSql + " ,RegTime=  replace(ISNULL(B.regtime,''),'-','')";
            StrSql = StrSql + "  FROM  tbl_ClosePay_02  A,";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   BankCode,Cpno,BankAcc,BankOwner,M_Name,BusCode,";
            StrSql = StrSql + " Saveid,Saveid2,LineCnt,Nominid,Nominid2,N_LineCnt,";
            StrSql = StrSql + " Mbid,Mbid2,regtime , Sell_Mem_TF,GiBu_ , LEaveDate , StopDate ";
            StrSql = StrSql + "  From tbl_ClosePay_02_Mod_161005 (nolock)   ";
            StrSql = StrSql + " Where ToEndDate ='" + ToEndDate  +"'";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
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
                StrSql = " Update tbl_ClosePay_02_Sell SET" ;
                StrSql = StrSql + " BeAmount = IsNull(b.A1, 0)";
                StrSql = StrSql + " ,BeCash=ISNULL(B.A2,0)";
                StrSql = StrSql + " ,BeCard=ISNULL(B.A3,0)";
                StrSql = StrSql + " ,BeBank=ISNULL(B.A4,0)";
                //StrSql = StrSql + " ,BeTotalPV=ISNULL(B.A5,0)";
                //StrSql = StrSql + " ,BeTotalCV=ISNULL(B.A6,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Sell  A,";

                StrSql = StrSql + " (";
                StrSql = StrSql + " Select  Sum(TotalPrice) AS A1,Sum(InputCash) AS A2, ";
                StrSql = StrSql + " Sum(InputCard) AS A3 ,        Sum(InputPassbook) AS A4 , ";
                StrSql = StrSql + " Sum(TotalPV) AS A5,           Sum(TotalCV) AS A6, ";
                StrSql = StrSql + " Mbid,Mbid2 , SellCode";
                StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
                StrSql = StrSql + " Where   SellDate_2 < '" + FromEndDate + "'";
                //StrSql = StrSql + " And   TotalPV  + TotalCV + TotalPrice > 0 ";
                StrSql = StrSql + " And   TotalPrice > 0 ";
                StrSql = StrSql + " And   Ga_Order = 0 ";
                StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where a.Mbid = b.Mbid ";
                StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
                StrSql = StrSql + " And   a.SellCode = b.SellCode";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                StrSql = " Update tbl_ClosePay_02_Sell SET";
                StrSql = StrSql + " BeTotalPV=ISNULL(B.A5,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Sell  A,";
                StrSql = StrSql + " (";
                StrSql = StrSql + " Select  Sum(TotalPV) AS A5, ";
                StrSql = StrSql + " Mbid,Mbid2 , SellCode";
                StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
                StrSql = StrSql + " Where   SellDate_2 < '" + FromEndDate + "'";
                StrSql = StrSql + " And   TotalPV  > 0 ";
                StrSql = StrSql + " And   Ga_Order = 0 ";
                StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where a.Mbid = b.Mbid ";
                StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
                StrSql = StrSql + " And   a.SellCode = b.SellCode";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                StrSql = " Update tbl_ClosePay_02_Sell SET";
                StrSql = StrSql + " BeTotalCV=ISNULL(B.A6,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Sell  A,";
                StrSql = StrSql + " (";
                StrSql = StrSql + " Select  Sum(TotalCV) AS A6, ";
                StrSql = StrSql + " Mbid,Mbid2 , SellCode";
                StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
                StrSql = StrSql + " Where   SellDate_2 < '" + FromEndDate + "'";
                StrSql = StrSql + " And   TotalCV > 0 ";
                StrSql = StrSql + " And   Ga_Order = 0 ";
                StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where a.Mbid = b.Mbid ";
                StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
                StrSql = StrSql + " And   a.SellCode = b.SellCode";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);








                StrSql = " Update tbl_ClosePay_02_Sell SET";
                StrSql = StrSql + "  BeReAmount = -IsNull(b.A1, 0)";
                StrSql = StrSql + " ,BeReCash=-ISNULL(B.A2,0)";
                StrSql = StrSql + " ,BeReCard=-ISNULL(B.A3,0)";
                StrSql = StrSql + " ,BeReBank=-ISNULL(B.A4,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Sell  A,";

                StrSql = StrSql + " (";
                StrSql = StrSql + " Select  Sum(TotalPrice) AS A1,Sum(InputCash) AS A2, ";
                StrSql = StrSql + " Sum(InputCard) AS A3 ,        Sum(InputPassbook) AS A4 , ";
                StrSql = StrSql + " Sum(TotalPV) AS A5,           Sum(TotalCV) AS A6, ";
                StrSql = StrSql + " Mbid,Mbid2 , SellCode";
                StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
                StrSql = StrSql + " Where   SellDate_2 < '" + FromEndDate + "'";
                StrSql = StrSql + " And   TotalPrice < 0 ";
                StrSql = StrSql + " And   Ga_Order = 0 ";
                StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where a.Mbid = b.Mbid ";
                StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
                StrSql = StrSql + " And   a.SellCode = b.SellCode";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);





                StrSql = " Update tbl_ClosePay_02_Sell SET";
                StrSql = StrSql + " BeReTotalPV=-ISNULL(B.A5,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Sell  A,";

                StrSql = StrSql + " (";
                StrSql = StrSql + " Select  Sum(TotalPV) AS A5, ";
                StrSql = StrSql + " Mbid,Mbid2 , SellCode";
                StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
                StrSql = StrSql + " Where   SellDate_2 < '" + FromEndDate + "'";
                StrSql = StrSql + " And   TotalPV < 0 ";
                StrSql = StrSql + " And   Ga_Order = 0 ";
                StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where a.Mbid = b.Mbid ";
                StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
                StrSql = StrSql + " And   a.SellCode = b.SellCode";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);



                StrSql = " Update tbl_ClosePay_02_Sell SET";
                StrSql = StrSql + " BeReTotalCV=-ISNULL(B.A6,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Sell  A,";

                StrSql = StrSql + " (";
                StrSql = StrSql + " Select  Sum(TotalCV) AS A6, ";
                StrSql = StrSql + " Mbid,Mbid2 , SellCode";
                StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
                StrSql = StrSql + " Where   SellDate_2 < '" + FromEndDate + "'";
                StrSql = StrSql + " And   TotalCV < 0 ";
                StrSql = StrSql + " And   Ga_Order = 0 ";
                StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where a.Mbid = b.Mbid ";
                StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
                StrSql = StrSql + " And   a.SellCode = b.SellCode";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                StrSql = " Update tbl_ClosePay_02_Sell SET";
                StrSql = StrSql + " BeShamSell = IsNull(b.A1, 0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Sell  A,";
                StrSql = StrSql + " (";
                StrSql = StrSql + " Select    Sum(Apply_PV) AS A1, Mbid,Mbid2 , SellCode";
                StrSql = StrSql + " From tbl_Sham_Sell  (nolock) ";
                StrSql = StrSql + " Where   Apply_Date < '" + FromEndDate + "'";
                StrSql = StrSql + " And     Apply_PV <> 0";
                StrSql = StrSql + " Group By Mbid,Mbid2, SellCode";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where a.Mbid = b.Mbid ";
                StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
                StrSql = StrSql + " And   a.SellCode = b.SellCode";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }



            StrSql = "Select Mbid,Mbid2 ,OrderNumber, SellCode , TotalPrice , InputCash , InputCard , InputPassbook , TotalPV , TotalCV , Re_BaseOrderNumber , SellDate_2 SellDate_2  ";
            StrSql = StrSql + " From    tbl_SalesDetail  (nolock)  ";
            StrSql = StrSql + " Where   SellDate_2 >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     SellDate_2 <= '" + ToEndDate + "'";
            //StrSql = StrSql + " And     TotalPV  + TotalCV < 0 ";
            StrSql = StrSql + " And     TotalPrice < 0 ";
            StrSql = StrSql + " And     SellCode <> '' " ;


            DataSet Dset = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn,  Dset);
            ReCnt = 0;            
            ReCnt = Search_Connect.DataSet_ReCount;
            
            
            pg1.Value = 0; pg1.Maximum = ReCnt;
            string Re_BaseOrderNumber = "", T_SellDate_2 = "", RePayDate = "", Rs_SellDate_2 = "";

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Re_BaseOrderNumber = Dset.Tables[base_db_name].Rows[fi_cnt]["Re_BaseOrderNumber"].ToString();
                Rs_SellDate_2 = Dset.Tables[base_db_name].Rows[fi_cnt]["SellDate_2"].ToString();

                T_SellDate_2 = ""; RePayDate = "";

                StrSql = "Select Mbid,Mbid2, OrderNumber, TotalPV , SellDate_2 SellDate_2   From tbl_SalesDetail   (nolock) ";
                StrSql = StrSql + " Where OrderNumber ='" + Re_BaseOrderNumber + "'";

                DataSet Dset2 = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset2);
                int ReCnt2 = Search_Connect.DataSet_ReCount;
                if (ReCnt2 >0 )
                {
                    T_SellDate_2 = Dset2.Tables[base_db_name].Rows[0]["SellDate_2"].ToString();
                }
           

                //if (T_SellDate_2 != "")
                //{
                //    StrSql = "Select ToEndDate,  PayDate   From  tbl_CloseTotal_02 (nolock) ";
                //    StrSql = StrSql + " Where FromEndDate <='" + T_SellDate_2 + "'";
                //    StrSql = StrSql + " And   ToEndDate >='" + T_SellDate_2 + "'";

                //    DataSet Dset3 = new DataSet();
                //    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset3);
                //    int ReCnt3 = Search_Connect.DataSet_ReCount;
                //    if (ReCnt3 > 0 )
                //    {
                //        RePayDate = Dset3.Tables[base_db_name].Rows[0]["PayDate"].ToString();
                //    }
                //}

                //if (RePayDate != "")
                //{
                //    if (int.Parse(Rs_SellDate_2) > int.Parse(RePayDate))
                //    {
                        StrSql = "Update tbl_ClosePay_02_Sell SET ";
                        StrSql = StrSql + "  DayReAmount = DayReAmount + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
                        StrSql = StrSql + " ,DayReCash = DayReCash + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["InputCash"].ToString());
                        StrSql = StrSql + " ,DayReCard = DayReCard + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["InputCard"].ToString());
                        StrSql = StrSql + " ,DayReBank = DayReBank + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["InputPassbook"].ToString());
                        StrSql = StrSql + "  Where Mbid  = '" + Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "'";
                        StrSql = StrSql + "  And   Mbid2 =  " + Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                        StrSql = StrSql + "  And   SellCode =  '" + Dset.Tables[base_db_name].Rows[fi_cnt]["SellCode"].ToString() + "'";

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);
                //    }
                //}

                pg1.PerformStep(); pg1.Refresh();
            }



            StrSql = "Select Mbid,Mbid2 ,OrderNumber, SellCode , TotalPrice , InputCash , InputCard , InputPassbook , TotalPV , TotalCV , Re_BaseOrderNumber ,SellDate_2 SellDate_2  ";
            StrSql = StrSql + " From    tbl_SalesDetail  (nolock)  ";
            StrSql = StrSql + " Where   SellDate_2 >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     TotalPV < 0 ";
            StrSql = StrSql + " And     SellCode <> '' ";


            Dset.Clear();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset);
            ReCnt = 0;
            ReCnt = Search_Connect.DataSet_ReCount;


            pg1.Value = 0; pg1.Maximum = ReCnt;
            Re_BaseOrderNumber = ""; T_SellDate_2 = ""; RePayDate = ""; Rs_SellDate_2 = "";

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Re_BaseOrderNumber = Dset.Tables[base_db_name].Rows[fi_cnt]["Re_BaseOrderNumber"].ToString();
                Rs_SellDate_2 = Dset.Tables[base_db_name].Rows[fi_cnt]["SellDate_2"].ToString();

                T_SellDate_2 = ""; RePayDate = "";

                StrSql = "Select Mbid,Mbid2, OrderNumber, TotalPV , SellDate_2 SellDate_2   From tbl_SalesDetail   (nolock) ";
                StrSql = StrSql + " Where OrderNumber ='" + Re_BaseOrderNumber + "'";

                DataSet Dset2 = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset2);
                int ReCnt2 = Search_Connect.DataSet_ReCount;
                if (ReCnt2 > 0)
                {
                    T_SellDate_2 = Dset2.Tables[base_db_name].Rows[0]["SellDate_2"].ToString();
                }


                //if (T_SellDate_2 != "")
                //{
                //    StrSql = "Select ToEndDate,  PayDate   From  tbl_CloseTotal_02 (nolock) ";
                //    StrSql = StrSql + " Where FromEndDate <='" + T_SellDate_2 + "'";
                //    StrSql = StrSql + " And   ToEndDate >='" + T_SellDate_2 + "'";

                //    DataSet Dset3 = new DataSet();
                //    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset3);
                //    int ReCnt3 = Search_Connect.DataSet_ReCount;
                //    if (ReCnt3 > 0)
                //    {
                //        RePayDate = Dset3.Tables[base_db_name].Rows[0]["PayDate"].ToString();
                //    }
                //}

                //if (RePayDate != "")
                //{
                //    if (int.Parse(Rs_SellDate_2) > int.Parse(RePayDate))
                //    {
                        StrSql = "Update tbl_ClosePay_02_Sell SET ";
                        StrSql = StrSql + " DayReTotalPV = DayReTotalPV + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
                        StrSql = StrSql + "  Where Mbid  = '" + Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "'";
                        StrSql = StrSql + "  And   Mbid2 =  " + Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                        StrSql = StrSql + "  And   SellCode =  '" + Dset.Tables[base_db_name].Rows[fi_cnt]["SellCode"].ToString() + "'";

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);
                //    }
                //}

                pg1.PerformStep(); pg1.Refresh();
            }



            StrSql = "Select Mbid,Mbid2 ,OrderNumber, SellCode , TotalPrice , InputCash , InputCard , InputPassbook , TotalPV , TotalCV , Re_BaseOrderNumber , SellDate_2 SellDate_2  ";
            StrSql = StrSql + " From    tbl_SalesDetail  (nolock)  ";
            StrSql = StrSql + " Where   SellDate_2 >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     TotalCV < 0 ";
            StrSql = StrSql + " And     SellCode <> '' ";


            Dset.Clear();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset);
            ReCnt = 0;
            ReCnt = Search_Connect.DataSet_ReCount;


            pg1.Value = 0; pg1.Maximum = ReCnt;
            Re_BaseOrderNumber = ""; T_SellDate_2 = ""; RePayDate = ""; Rs_SellDate_2 = "";

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Re_BaseOrderNumber = Dset.Tables[base_db_name].Rows[fi_cnt]["Re_BaseOrderNumber"].ToString();
                Rs_SellDate_2 = Dset.Tables[base_db_name].Rows[fi_cnt]["SellDate_2"].ToString();

                T_SellDate_2 = ""; RePayDate = "";

                StrSql = "Select Mbid,Mbid2, OrderNumber, TotalPV ,SellDate_2 SellDate_2   From tbl_SalesDetail   (nolock) ";
                StrSql = StrSql + " Where OrderNumber ='" + Re_BaseOrderNumber + "'";

                DataSet Dset2 = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset2);
                int ReCnt2 = Search_Connect.DataSet_ReCount;
                if (ReCnt2 > 0)
                {
                    T_SellDate_2 = Dset2.Tables[base_db_name].Rows[0]["SellDate_2"].ToString();
                }


                //if (T_SellDate_2 != "")
                //{
                //    StrSql = "Select ToEndDate,  PayDate   From  tbl_CloseTotal_02 (nolock) ";
                //    StrSql = StrSql + " Where FromEndDate <='" + T_SellDate_2 + "'";
                //    StrSql = StrSql + " And   ToEndDate >='" + T_SellDate_2 + "'";

                //    DataSet Dset3 = new DataSet();
                //    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset3);
                //    int ReCnt3 = Search_Connect.DataSet_ReCount;
                //    if (ReCnt3 > 0)
                //    {
                //        RePayDate = Dset3.Tables[base_db_name].Rows[0]["PayDate"].ToString();
                //    }
                //}

                //if (RePayDate != "")
                //{
                //    if (int.Parse(Rs_SellDate_2) > int.Parse(RePayDate))
                //    {
                        StrSql = "Update tbl_ClosePay_02_Sell SET ";
                        StrSql = StrSql + " DayReTotalCV = DayReTotalCV + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["TotalCV"].ToString());
                        StrSql = StrSql + "  Where Mbid  = '" + Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "'";
                        StrSql = StrSql + "  And   Mbid2 =  " + Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                        StrSql = StrSql + "  And   SellCode =  '" + Dset.Tables[base_db_name].Rows[fi_cnt]["SellCode"].ToString() + "'";

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);
                //    }
                //}

                pg1.PerformStep(); pg1.Refresh();
            }
                    

            pg1.Value = 0; pg1.Maximum = 4  ;
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_02_Sell SET";
            StrSql = StrSql + " DayAmount = IsNull(b.A1, 0)";
            StrSql = StrSql + " ,DayCash=ISNULL(B.A2,0)";
            StrSql = StrSql + " ,DayCard=ISNULL(B.A3,0)";
            StrSql = StrSql + " ,DayBank=ISNULL(B.A4,0)";
            //StrSql = StrSql + " ,DayTotalPV=ISNULL(B.A5,0)";
            //StrSql = StrSql + " ,DayTotalCV=ISNULL(B.A6,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Sell  A,";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select  ";
            StrSql = StrSql + " Sum(BS1.TotalPrice)   AS A1,         Sum(BS1.InputCash)       AS A2, ";
            StrSql = StrSql + " Sum(BS1.InputCard)     AS A3 ,        Sum(BS1.InputPassbook)   AS A4 , ";
            StrSql = StrSql + " Sum(BS1.TotalPV)         AS A5,         Sum(BS1.TotalCV)        AS A6, ";
            StrSql = StrSql + " BS1.Mbid,BS1.Mbid2 , BS1.SellCode";
            StrSql = StrSql + " From tbl_SalesDetail AS BS1 (nolock) ";
            //StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            //StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber And   Bs_R.Re_BaseOrderNumber  <> ''   And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " Where   BS1.SellDate_2 >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     BS1.SellDate_2 <= '" + ToEndDate + "'";
            //StrSql = StrSql + " And     BS1.TotalPV  + BS1.TotalCV >= 0 ";
            StrSql = StrSql + " And     BS1.TotalPrice >= 0 ";
            StrSql = StrSql + " And     BS1.SellCode <> '' ";
            StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
            StrSql = StrSql + " Group By BS1.Mbid,BS1.Mbid2, BS1.SellCode";
            StrSql = StrSql + " Having Sum(BS1.TotalPrice)  >= 0";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
            StrSql = StrSql + " And   a.SellCode = b.SellCode";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();  //구매 종류 별로 넣는다. 합계를 +판매에 대해서만



            StrSql = " Update tbl_ClosePay_02_Sell SET";
            StrSql = StrSql + " DayTotalPV=ISNULL(B.A5,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Sell  A,";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select  ";
            StrSql = StrSql + " Sum(BS1.TotalPV)     AS A5, ";
            StrSql = StrSql + " BS1.Mbid,BS1.Mbid2 , BS1.SellCode";
            StrSql = StrSql + " From tbl_SalesDetail AS BS1 (nolock) ";
            //StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            //StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber   And   Bs_R.Re_BaseOrderNumber  <> ''   And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " Where   BS1.SellDate_2 >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     BS1.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     BS1.TotalPV  >= 0 ";
            StrSql = StrSql + " And     BS1.SellCode <> '' ";
            StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
            StrSql = StrSql + " Group By BS1.Mbid,BS1.Mbid2, BS1.SellCode";
            StrSql = StrSql + " Having Sum(BS1.TotalPV)  >= 0";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
            StrSql = StrSql + " And   a.SellCode = b.SellCode";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();  //구매 종류 별로 넣는다. 합계를 +판매에 대해서만



            StrSql = " Update tbl_ClosePay_02_Sell SET";
            StrSql = StrSql + " DayTotalCV=ISNULL(B.A6,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Sell  A,";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select  ";
            StrSql = StrSql + " Sum(BS1.TotalCV)        AS A6, ";
            StrSql = StrSql + " BS1.Mbid,BS1.Mbid2 , BS1.SellCode";
            StrSql = StrSql + " From tbl_SalesDetail AS BS1 (nolock) ";
            //StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            //StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber   And   Bs_R.Re_BaseOrderNumber  <> ''   And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " Where   BS1.SellDate_2 >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     BS1.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     BS1.TotalCV >= 0 ";
            StrSql = StrSql + " And     BS1.SellCode <> '' ";
            StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
            StrSql = StrSql + " Group By BS1.Mbid,BS1.Mbid2, BS1.SellCode";
            StrSql = StrSql + " Having Sum(BS1.TotalCV)  >= 0";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
            StrSql = StrSql + " And   a.SellCode = b.SellCode";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();  //구매 종류 별로 넣는다. 합계를 +판매에 대해서만


        
            StrSql = " Update tbl_ClosePay_02_Sell SET";
            StrSql = StrSql + " DayShamSell = IsNull(b.A1, 0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Sell  A,";
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
    
    
    
            StrSql = " Update tbl_ClosePay_02_Sell Set";
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
       

            StrSql = "Update tbl_ClosePay_02 SET " ;
            StrSql = StrSql + " SellPrice01=ISNULL(B.A1,0) " ;
            StrSql = StrSql + ",SellPv01=ISNULL(B.A2,0) " ;
            StrSql = StrSql + ",SellCv01=ISNULL(B.A3,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, " ;
    
            StrSql = StrSql + " (Select Sum(SumAmount-SumReAmount) AS A1, Sum(SumTotalPV-SumReTotalPV) AS A2 , Sum(SumTotalCV-SumReTotalCV) AS A3 , Sum(SumShamSell) AS A4 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_02_Sell " ;
            StrSql = StrSql + " Where (SellCode ='01' OR SellCode ='Auto')";
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_02 SET " ;
            StrSql = StrSql + " SellSham01 = ISNULL(B.A4,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, " ;
    
            StrSql = StrSql + " (Select  Sum(SumShamSell) AS A4 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_02_Sell " ;
            StrSql = StrSql + " Where SellCode ='01'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_02 SET " ;
            StrSql = StrSql + " SellPrice02 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + ",SellPv02 = ISNULL(B.A2,0) " ;
            StrSql = StrSql + ",SellCv02 = ISNULL(B.A3,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, " ;
    
            StrSql = StrSql + " (Select Sum(SumAmount-SumReAmount) AS A1, Sum(SumTotalPV-SumReTotalPV) AS A2 , Sum(SumTotalCV-SumReTotalCV) AS A3 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_02_Sell " ;
            StrSql = StrSql + " Where SellCode ='02'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_02 SET " ;
            StrSql = StrSql + " SellPrice03 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + ",SellPv03 = ISNULL(B.A2,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, " ;

            StrSql = StrSql + " (Select Sum(SumAmount-SumReAmount) AS A1, Sum(SumTotalPV-SumReTotalPV) AS A2 , Sum(SumTotalCV-SumReTotalCV) AS A3 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_02_Sell " ;
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
  
            StrSql = "Update tbl_ClosePay_02 SET " ;
            StrSql = StrSql + "  DayPrice01 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,DayPv01 = ISNULL(B.A2,0) " ;
            StrSql = StrSql + " ,DayCV01 = ISNULL(B.A3,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, " ;
    
            StrSql = StrSql + " (Select Sum(DayAmount-DayReAmount) AS A1 , Sum(DayTotalPV-DayReTotalPV) AS A2 ,Sum(DayTotalCV-DayReTotalCV) AS A3 ,Sum(DayShamSell) AS A4  ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_02_Sell " ;
            StrSql = StrSql + " Where  (SellCode ='01' OR SellCode ='Auto')";
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
            StrSql = "Update tbl_ClosePay_02 SET " ;
            StrSql = StrSql + " DaySham01 = ISNULL(B.A4,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, " ;
    
            StrSql = StrSql + " (Select  Sum(DayShamSell) AS A4 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_02_Sell " ;
            StrSql = StrSql + " Where SellCode ='01'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_02 SET " ;
            StrSql = StrSql + "  DayPrice02 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,DayPv02 = ISNULL(B.A2,0) " ;
            StrSql = StrSql + " ,DayCV02 = ISNULL(B.A3,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, " ;
            StrSql = StrSql + " (Select Sum(DayAmount-DayReAmount) AS A1 ,Sum(DayTotalPV-DayReTotalPV) AS A2  ,Sum(DayTotalCV-DayReTotalCV) AS A3,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_02_Sell " ;
            StrSql = StrSql + " Where SellCode ='02'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_02 SET " ;
            StrSql = StrSql + "  DayPrice03 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,DayPv03 = ISNULL(B.A2,0) " ;
            StrSql = StrSql + " ,DayCV03 = ISNULL(B.A3,0) " ;
    
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, " ;

            StrSql = StrSql + " (Select Sum(DayAmount-DayReAmount) AS A1 , Sum(DayTotalPV-DayReTotalPV) AS A2  ,Sum(DayTotalCV-DayReTotalCV) AS A3,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_02_Sell " ;
            StrSql = StrSql + " Where SellCode ='03'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;

            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
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

                 StrSql = "Update tbl_ClosePay_02 SET ";
                 StrSql = StrSql + " LevelCnt=ISNULL(B.lvl,0) ";
                 StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                 StrSql = StrSql + " (Select    empid0,empid,lvl ";
                 StrSql = StrSql + " From ufn_GetSubTree_Pay02_Mem('" + Mbid + "'," + Mbid2;
                 StrSql = StrSql + ") Where pos <>0 ";
                 StrSql = StrSql + " ) B";

                 StrSql = StrSql + " Where A.Mbid=B.empid0 ";
                 StrSql = StrSql + " And   A.Mbid2=B.empid ";

                 Temp_Connect.Insert_Data(StrSql, Conn, tran);

                 pg1.PerformStep(); pg1.Refresh();
             } // for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)

         } // (ReCnt != 0)




         StrSql = "Select Max(LevelCnt) From tbl_ClosePay_02  ";

         SqlDataReader sr = null;
         Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
         ReCnt = Temp_Connect.DataSet_ReCount;

         while (sr.Read())
         {
             MaxLevel = int.Parse(sr.GetValue(0).ToString());
         }

         sr.Close(); sr.Dispose();

     }





     private void Put_LevelCnt_Update_Nom(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
     {
         string StrSql = " Select Mbid,Mbid2 From Tbl_Memberinfo  (nolock) Where nominid='**'   ";
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

                 StrSql = "Update tbl_ClosePay_02 SET ";
                 StrSql = StrSql + " N_LevelCnt=ISNULL(B.lvl,0) ";
                 StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                 StrSql = StrSql + " (Select    empid0,empid,lvl ";
                 StrSql = StrSql + " From ufn_GetSubTree_Pay02_Nom('" + Mbid + "'," + Mbid2;
                 StrSql = StrSql + ") Where pos <>0 ";
                 StrSql = StrSql + " ) B";

                 StrSql = StrSql + " Where A.Mbid=B.empid0 ";
                 StrSql = StrSql + " And   A.Mbid2=B.empid ";

                 Temp_Connect.Insert_Data(StrSql, Conn, tran);

                 pg1.PerformStep(); pg1.Refresh();
             } // for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)

         } // (ReCnt != 0)



         pg1.Value = 0; pg1.Maximum = 5;
         pg1.PerformStep(); pg1.Refresh();


         StrSql = "Select Max(N_LevelCnt) From tbl_ClosePay_02  ";

         SqlDataReader sr = null;
         Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
         ReCnt = Temp_Connect.DataSet_ReCount;

         while (sr.Read())
         {
             N_MaxLevel = int.Parse(sr.GetValue(0).ToString());
         }

         sr.Close(); sr.Dispose();
         pg1.PerformStep(); pg1.Refresh();


         //StrSql = "INSERT INTO tbl_ClosePay_02_DownPV";
         //StrSql = StrSql + " (Mbid,Mbid2,N_LineCnt,RecordMakeDate,LevelCnt) ";
         //StrSql = StrSql + " Select    a.Mbid,A.mbid2,0,'" + ToEndDate + "', 0 ";
         //StrSql = StrSql + " From (tbl_memberinfo as a (nolock)  ";
         //StrSql = StrSql + " Left join tbl_memberinfo as b  (nolock) on ";
         //StrSql = StrSql + " a.Mbid=b.nominid and a.Mbid2=b.nominid2) ";
         //StrSql = StrSql + " Where b.nominid is null ";
         //StrSql = StrSql + " And   a.N_LineCnt >0";

         //Temp_Connect.Insert_Data(StrSql, Conn, tran);
         //pg1.PerformStep(); pg1.Refresh();



         //StrSql = "INSERT INTO tbl_ClosePay_02_DownPV";
         //StrSql = StrSql + " (Mbid,Mbid2,N_LineCnt,RecordMakeDate,LevelCnt) ";
         //StrSql = StrSql + " Select    nominid,nominid2,N_LineCnt,'" + ToEndDate + "',0 ";
         //StrSql = StrSql + " From tbl_memberinfo (nolock) ";
         //StrSql = StrSql + " Where N_LineCnt >0 ";
         //StrSql = StrSql + " And   nominid <>'**'";
         //Temp_Connect.Insert_Data(StrSql, Conn, tran);
         //pg1.PerformStep(); pg1.Refresh();



         //StrSql = "Update tbl_ClosePay_02_DownPV SET ";
         //StrSql = StrSql + " nominid=ISNULL(B.nominid,'') ";
         //StrSql = StrSql + " ,nominid2=ISNULL(B.nominid2,0) ";
         //StrSql = StrSql + " ,Curposition=ISNULL(B.N_LineCnt,0) ";
         //StrSql = StrSql + " ,LevelCnt=ISNULL(B.LevelCnt,0) ";

         //StrSql = StrSql + " FROM  tbl_ClosePay_02_DownPV  A, ";
         //StrSql = StrSql + " (Select    nominid,nominid2,N_LineCnt,Mbid,Mbid2,LevelCnt ";
         //StrSql = StrSql + " From tbl_ClosePay_02 ) B";
         //StrSql = StrSql + " Where A.Mbid=B.Mbid ";
         //StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
         //Temp_Connect.Insert_Data(StrSql, Conn, tran);
         //pg1.PerformStep(); pg1.Refresh();



     }





        private void ReqTF1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 8;
            pg1.PerformStep(); pg1.Refresh();


            string StrSql = "";



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " SellPV01 =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " ,SellCV01 =  ISNULL(B.a2,0) ";
            StrSql = StrSql + " ,SellPrice01 =  ISNULL(B.a3,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 ";
            StrSql = StrSql + " , Sum(Se.TotalCV )  a2 ";
            StrSql = StrSql + " , Sum(Se.TotalPrice )  a3 ";
            StrSql = StrSql + " , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " WHERE   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";                        
            StrSql = StrSql + " Where   A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);




           StrSql = "select top 3 fromenddate , ToEndDate from dbo.tbl_CloseTotal_02 Where ToEndDate <> '" + ToEndDate  + "' Order by ToEndDate DESC  ";
           string SDate3 = "", To_SDate3 = "", From_SDate2 = "", From_SDate1 = "", To_SDate2 = "", To_SDate1 = "";
           int ReCnt = 0;
           DataSet Dset4 = new DataSet();
           Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
           ReCnt = Search_Connect.DataSet_ReCount;

           if (ReCnt > 0)
           {  //이번주 포함 4주간의 매출 합산을 불러온다.  150
               SDate3 = Dset4.Tables[base_db_name].Rows[0][0].ToString();
               To_SDate3 = Dset4.Tables[base_db_name].Rows[0][1].ToString();

               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " W_3_QV_Real =  ISNULL(B.a1,0) ";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + " Select   Sum(Se.TotalPV)  a1 , Se.Mbid , Se.Mbid2 ";
               StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
               //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
               StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + SDate3 + "'";
               StrSql = StrSql + " And   Se.SellDate_2  <='" + To_SDate3 + "'";
               StrSql = StrSql + " And   Se.Ga_Order = 0 ";
               StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
               StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
               StrSql = StrSql + " ) B";
               StrSql = StrSql + " Where A.Mbid=B.Mbid ";
               StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

               Temp_Connect.Insert_Data(StrSql, Conn, tran);


               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " Active_3_FLAG =  ISNULL(B.a1,0) ";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + " Select   Mbid,Mbid2 , Case When  ReqTF1 = 1 then 'Y' ELSE '' End a1  ";
               StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
               StrSql = StrSql + " WHERE   ToEndDate ='" + To_SDate3 + "'";
               StrSql = StrSql + " ) B";
               StrSql = StrSql + " Where A.Mbid=B.Mbid ";
               StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

               Temp_Connect.Insert_Data(StrSql, Conn, tran);



               if (ReCnt >= 2)
               {
                   From_SDate2 = Dset4.Tables[base_db_name].Rows[1][0].ToString();
                   To_SDate2 = Dset4.Tables[base_db_name].Rows[1][1].ToString();

                   StrSql = "Update tbl_ClosePay_02 SET ";
                   StrSql = StrSql + " W_2_QV_Real =  ISNULL(B.a1,0) ";
                   StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                   StrSql = StrSql + " (";
                   StrSql = StrSql + " Select   Sum(Se.TotalPV)  a1 , Se.Mbid , Se.Mbid2 ";
                   StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
                   //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
                   StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + From_SDate2 + "'";
                   StrSql = StrSql + " And   Se.SellDate_2  <='" + To_SDate2 + "'";
                   StrSql = StrSql + " And   Se.Ga_Order = 0 ";
                   StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
                   StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
                   StrSql = StrSql + " ) B";
                   StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                   StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

                   Temp_Connect.Insert_Data(StrSql, Conn, tran);

                   StrSql = "Update tbl_ClosePay_02 SET ";
                   StrSql = StrSql + " Active_2_FLAG =  ISNULL(B.a1,0) ";
                   StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                   StrSql = StrSql + " (";
                   StrSql = StrSql + " Select   Mbid,Mbid2 , Case When  ReqTF1 = 1 then 'Y' ELSE '' End a1  ";
                   StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
                   StrSql = StrSql + " WHERE   ToEndDate ='" + To_SDate2 + "'";
                   StrSql = StrSql + " ) B";
                   StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                   StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

                   Temp_Connect.Insert_Data(StrSql, Conn, tran);

               }

               if (ReCnt >= 3)
               {
                   From_SDate1 = Dset4.Tables[base_db_name].Rows[2][0].ToString();
                   To_SDate1 = Dset4.Tables[base_db_name].Rows[2][1].ToString();

                   StrSql = "Update tbl_ClosePay_02 SET ";
                   StrSql = StrSql + " W_1_QV_Real =  ISNULL(B.a1,0) ";
                   StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                   StrSql = StrSql + " (";
                   StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 , Se.Mbid , Se.Mbid2 ";
                   StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
                  // StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
                   StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + From_SDate1 + "'";
                   StrSql = StrSql + " And   Se.SellDate_2  <='" + To_SDate1 + "'";
                   StrSql = StrSql + " And   Se.Ga_Order = 0 ";
                   StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
                   StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
                   StrSql = StrSql + " ) B";
                   StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                   StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

                   Temp_Connect.Insert_Data(StrSql, Conn, tran);



                   StrSql = "Update tbl_ClosePay_02 SET ";
                   StrSql = StrSql + " Active_1_FLAG =  ISNULL(B.a1,0) ";
                   StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                   StrSql = StrSql + " (";
                   StrSql = StrSql + " Select   Mbid,Mbid2 , Case When  ReqTF1 = 1 then 'Y' ELSE '' End a1  ";
                   StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
                   StrSql = StrSql + " WHERE   ToEndDate ='" + To_SDate1 + "'";
                   StrSql = StrSql + " ) B";
                   StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                   StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

                   Temp_Connect.Insert_Data(StrSql, Conn, tran);
               }
           }

         
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " W_4_QV_Real =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + FromEndDate  + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);




            if (SDate3 == "") SDate3 = FromEndDate; 

            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " W4_QV_Real =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + SDate3 + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";               
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " W4_QV =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";               
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + SDate3 + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate  + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";               
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " And   Se.SellCode <> 'Auto' " ;
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            //4주간의 오토쉽 합산을 불러온다.  100
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " W4_QV_Auto =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + SDate3 + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " And   Se.SellCode = 'Auto' ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //4주간의 직추천 우대고객의 매출을 불러온다.
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " W4_QV_Down =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 , tbl_Memberinfo.Nominid , tbl_Memberinfo.Nominid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid  = tbl_Memberinfo.Mbid And tbl_Memberinfo.Mbid2  = tbl_Memberinfo.Mbid2 "; 
            StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + SDate3 + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " And   tbl_Memberinfo.Sell_Mem_TF = 1 ";
            StrSql = StrSql + " Group by tbl_Memberinfo.Nominid , tbl_Memberinfo.Nominid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //W4_QV  W4_QV_Auto  W4_QV_Down

            //ReqTF10 = 1 개별구매, ReqTF10 =2  오토쉽구매 ,  ReqTF10 = 3 직추천소비자 구매    
            StrSql = " Update tbl_ClosePay_02 SET";
            StrSql = StrSql + " ReqTF1 = 1 ";
            StrSql = StrSql + " , ReqTF10 = 1 ";
            StrSql = StrSql + " Where  W4_QV >= 150  ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = " Update tbl_ClosePay_02 SET";
            StrSql = StrSql + " ReqTF1 = 1 ";
            StrSql = StrSql + " , ReqTF10 = 2 ";            
            StrSql = StrSql + " Where  W4_QV_Auto >= 100  ";
            StrSql = StrSql + " And    ReqTF1 = 0   ";            

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = " Update tbl_ClosePay_02 SET";
            StrSql = StrSql + " ReqTF1 = 1 ";
            StrSql = StrSql + " , ReqTF10 = 3 ";            
            StrSql = StrSql + " Where  (W4_QV_Down >= 300 And (W4_QV + W4_QV_Auto ) >= 1 )   ";
            StrSql = StrSql + " And    ReqTF1 = 0   ";            

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " ReqDate1='" + ToEndDate + "'";
            StrSql = StrSql + " Where ReqDate1=''";
            StrSql = StrSql + " And ReqTF1 >= 1 ";
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        
        }




        private void Put_Frist_OrderNumber_01(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran )
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";     

            StrSql = " ;Update tbl_ClosePay_02 SET " ;
            StrSql = StrSql + "  First_OrderNumber =ISNULL(B.A1,'') ";            
            StrSql = StrSql +  "  FROM  tbl_ClosePay_02  A, " ;    
            StrSql = StrSql +  "  (Select Isnull( Min(Se.RecordTime) ,'') As A1 , Se.Mbid,Se.Mbid2 " ;        
            StrSql = StrSql +  "  From tbl_SalesDetail Se (nolock) " ;
            StrSql = StrSql +  "  Where Se.TotalPV > 0   " ;
            StrSql = StrSql + "  And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql +  "  And   SE.Ga_Order = 0 " ;
            StrSql = StrSql +  "  Group By Se.Mbid,Se.Mbid2 " ;
            StrSql = StrSql +  "  ) B" ;
    
            StrSql = StrSql +  "  Where A.Mbid=B.Mbid " ;
            StrSql = StrSql +  "  And   A.Mbid2=B.Mbid2 " ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
        }


        private void CurPoint_Put_2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string S_ToEndDate)
        {

            pg1.Value = 0; pg1.Maximum = 7    ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";   
         
            StrSql = " Update tbl_ClosePay_02 SET"    ;
            StrSql = StrSql + " CurPoint_Date_2_Gap = 0 "   ;
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    

            StrSql = "Update tbl_ClosePay_02 SET "   ;
            StrSql = StrSql + " CurPoint_SellPV = ISNULL(B.A1, 0 )   "   ;
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, "   ;
    
            StrSql = StrSql + " (Select Sum(TotalPV) A1,  Mbid ,Mbid2   "   ;
            StrSql = StrSql + " From tbl_SalesDetail (nolock)"   ;
            StrSql = StrSql + " Where   SellDate_2 <='" + S_ToEndDate + "'";
            StrSql = StrSql + " And      (SellCode ='01' OR SellCode ='Auto') ";
            StrSql = StrSql + " And  Ga_Order = 0 ";
            StrSql = StrSql + " Group By Mbid,Mbid2"   ;
            StrSql = StrSql + " ) B"   ;
    
            StrSql = StrSql + " Where A.Mbid  = B.Mbid "   ;
            StrSql = StrSql + " And   A.Mbid2  = B.Mbid2 "   ;
      
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();
    
    
            StrSql = " Update tbl_ClosePay_02 SET"   ;
            StrSql = StrSql + " CurPoint = 2 "   ;
            StrSql = StrSql + " Where CurPoint_SellPV >= 250000 "   ;
            StrSql = StrSql + " And Sell_Mem_TF = 0 "   ;
            
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();
        
       
            StrSql = "Update tbl_ClosePay_02 Set "   ;
            StrSql = StrSql + " CurPoint_Date_2='" + S_ToEndDate + "'"   ;
            StrSql = StrSql + " Where CurPoint_Date_2=''"   ;
            StrSql = StrSql + " And CurPoint = 2 "   ;
        
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();
    
            StrSql = "Update tbl_ClosePay_02 Set "   ;
            StrSql = StrSql + " CurPoint_Date_2_Gap =  DateDiff(D, Regtime, CurPoint_Date_2) "   ;
            StrSql = StrSql + " Where CurPoint_Date_2 ='" + S_ToEndDate + "'"   ;
        
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();
    
    
            StrSql = "Update tbl_ClosePay_02 Set "   ;
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


           StrSql = " Update tbl_ClosePay_02 SET"  ;
            StrSql = StrSql + " CurPoint_Date_3_Gap = 0 "  ;
            
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
                   pg1.PerformStep(); pg1.Refresh();
 
            StrSql = " Update tbl_ClosePay_02 SET"  ;
            StrSql = StrSql + " CurPoint = 3 "  ;
            StrSql = StrSql + " Where CurPoint_SellPV >= 750000 "  ;
            StrSql = StrSql + " And CurPoint_Date_2 <> '' "  ;
            StrSql = StrSql + " And Sell_Mem_TF = 0 "  ;
            
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
                   pg1.PerformStep(); pg1.Refresh();     
       
            StrSql = "Update tbl_ClosePay_02 Set "  ;
            StrSql = StrSql + " CurPoint_Date_3='" + S_ToEndDate + "'"  ;
            StrSql = StrSql + " Where CurPoint_Date_3=''"  ;
            StrSql = StrSql + " And CurPoint = 3 "  ;
        
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
                   pg1.PerformStep(); pg1.Refresh(); 
    
             StrSql = "Update tbl_ClosePay_02 Set "  ;
             StrSql = StrSql + " CurPoint_Date_3_Gap =  DateDiff(D, CurPoint_Date_2, CurPoint_Date_3) ";
            StrSql = StrSql + " Where CurPoint_Date_3 ='" + S_ToEndDate + "'"  ;
        
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
                   pg1.PerformStep(); pg1.Refresh();
    
            StrSql = "Update tbl_ClosePay_02 Set "  ;
            StrSql = StrSql + " CurPoint =  0 "  ;
            StrSql = StrSql + " ,CurPoint_Date_3 = '' "  ;
            StrSql = StrSql + " Where CurPoint_Date_3 ='" + S_ToEndDate + "'"  ;
            StrSql = StrSql + " And   CurPoint_Date_3_Gap > 45 "  ;
        
   
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
           pg1.PerformStep(); pg1.Refresh();

       }





       private void Put_Down_SumPV(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
       {
           string StrSql = "";



           pg1.Value = 0; pg1.Maximum = N_MaxLevel + 9;
           pg1.PerformStep(); pg1.Refresh();

           int Cnt = N_MaxLevel;

           while (Cnt >= 0)
           {

               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " Down_W4_QV_Real =  ISNULL(B.A2,0)";
               StrSql = StrSql + " ,Down_G_Down =    ISNULL(B.A1,0)";
               StrSql = StrSql + " ,Down_W_1_QV_Real =    ISNULL(B.W_1,0)";
               StrSql = StrSql + " ,Down_W_2_QV_Real =    ISNULL(B.W_2,0)";
               StrSql = StrSql + " ,Down_W_3_QV_Real =    ISNULL(B.W_3,0)";
               StrSql = StrSql + " ,Down_W_4_QV_Real =    ISNULL(B.W_4,0)";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + "Select Sum(Down_G_Down + SellPV01 ) A1  , Sum(Down_W4_QV_Real +  W4_QV_Real  ) A2 ";
               StrSql = StrSql + " ,Sum(Down_W_1_QV_Real + W_1_QV_Real ) W_1 ";
               StrSql = StrSql + " ,Sum(Down_W_2_QV_Real + W_2_QV_Real ) W_2 ";
               StrSql = StrSql + " ,Sum(Down_W_3_QV_Real + W_3_QV_Real ) W_3 ";
               StrSql = StrSql + " ,Sum(Down_W_4_QV_Real + W_4_QV_Real ) W_4 ";

               StrSql = StrSql + " ,Nominid,Nominid2 ";
               StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
               StrSql = StrSql + " Where ((  Down_W4_QV_Real +  W4_QV_Real ) <>0  OR (Down_G_Down + SellPV01) <> 0  )  ";
               StrSql = StrSql + " And   N_LevelCnt =" + Cnt;
               StrSql = StrSql + " Group By Nominid,Nominid2   ";
               StrSql = StrSql + " ) B";

               StrSql = StrSql + " Where A.Mbid=B.Nominid ";
               StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

               Temp_Connect.Insert_Data(StrSql, Conn, tran);

               //StrSql = "Update tbl_ClosePay_02 SET ";
               //StrSql = StrSql + " G_Cur_PV_1 =  ISNULL(B.A2,0)";
               //StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               //StrSql = StrSql + " (";
               //StrSql = StrSql + "Select    Sum(G_Cur_PV_1 + G_Cur_PV_2 + SellPV01 + SellPV02 +SellPV03  ) A2 ";
               //StrSql = StrSql + " ,Saveid,Saveid2 ";
               //StrSql = StrSql + " From tbl_ClosePay_02 ";
               //StrSql = StrSql + " Where (  G_Cur_PV_1 + G_Cur_PV_2 + SellPV01 + SellPV02 +SellPV03 ) <>0   ";
               //StrSql = StrSql + " And   LevelCnt =" + Cnt;
               //StrSql = StrSql + " And   LineCnt =  1 ";
               //StrSql = StrSql + " Group By Saveid,Saveid2   ";
               //StrSql = StrSql + " ) B";

               //StrSql = StrSql + " Where A.Mbid=B.Saveid ";
               //StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

               //Temp_Connect.Insert_Data(StrSql, Conn, tran);


               //StrSql = "Update tbl_ClosePay_02 SET ";
               //StrSql = StrSql + " G_Cur_PV_2 =  ISNULL(B.A2,0)";
               //StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               //StrSql = StrSql + " (";
               //StrSql = StrSql + "Select    Sum(G_Cur_PV_1 + G_Cur_PV_2 + SellPV01 + SellPV02 +SellPV03   ) A2 ";
               //StrSql = StrSql + " ,Saveid,Saveid2 ";
               //StrSql = StrSql + " From tbl_ClosePay_02 ";
               //StrSql = StrSql + " Where (  G_Cur_PV_1 + G_Cur_PV_2 + SellPV01 + SellPV02 +SellPV03  ) <>0   ";
               //StrSql = StrSql + " And   LevelCnt =" + Cnt;
               //StrSql = StrSql + " And   LineCnt =  2 ";
               //StrSql = StrSql + " Group By Saveid,Saveid2   ";
               //StrSql = StrSql + " ) B";

               //StrSql = StrSql + " Where A.Mbid=B.Saveid ";
               //StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

               //Temp_Connect.Insert_Data(StrSql, Conn, tran);
               pg1.PerformStep(); pg1.Refresh();

               Cnt = Cnt - 1;

           }


           StrSql = "Update tbl_ClosePay_02 SET ";
           StrSql = StrSql + " Max_Down_W4_QV_Real =  ISNULL(B.A1,0)";           
           StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

           StrSql = StrSql + " (";
           StrSql = StrSql + " Select Max(Down_W4_QV_Real + W4_QV_Real ) A1 ";
           StrSql = StrSql + " ,Nominid,Nominid2 ";
           StrSql = StrSql + " From tbl_ClosePay_02  (nolock) ";
           StrSql = StrSql + " Where (Down_W4_QV_Real + W4_QV_Real )  > 0 ";
           StrSql = StrSql + " Group By Nominid,Nominid2   ";
           StrSql = StrSql + " ) B";

           StrSql = StrSql + " Where A.Mbid=B.Nominid ";
           StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

           Temp_Connect.Insert_Data(StrSql, Conn, tran);
           pg1.PerformStep(); pg1.Refresh();


           StrSql = "Update tbl_ClosePay_02 SET ";
           StrSql = StrSql + " Max_Down_G_Down =  ISNULL(B.A1,0)";
           StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

           StrSql = StrSql + " (";
           StrSql = StrSql + "Select Max(Down_G_Down + SellCV01 ) A1 ";
           StrSql = StrSql + " ,Nominid,Nominid2 ";
           StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
           StrSql = StrSql + " Where (Down_G_Down + SellCV01 )  > 0 ";
           StrSql = StrSql + " Group By Nominid,Nominid2   ";
           StrSql = StrSql + " ) B";

           StrSql = StrSql + " Where A.Mbid=B.Nominid ";
           StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

           Temp_Connect.Insert_Data(StrSql, Conn, tran);
           pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Max_N_LineCnt =  ";
            StrSql += " (Select Top 1 N_LineCnt From tbl_ClosePay_02 (nolock) AS CC_02  " ;
            StrSql += "                      Where CC_02.Nominid = tbl_ClosePay_02.Mbid  ";
            StrSql += "                      And   CC_02.Nominid2 = tbl_ClosePay_02.Mbid2   ";
            StrSql += "                      And   CC_02.Down_W4_QV_Real + CC_02.W4_QV_Real  =  tbl_ClosePay_02.Max_Down_W4_QV_Real   ";
            StrSql += "  ) ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();    
           
           
     
           StrSql = "Update tbl_ClosePay_02 SET ";
           StrSql = StrSql + " Max_Down_W_1_QV_Real =  ISNULL(B.A1,0)";           
           StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

           StrSql = StrSql + " (";
           StrSql = StrSql + " Select (Down_W_1_QV_Real + W_1_QV_Real ) A1 ";
           StrSql = StrSql + " ,Nominid,Nominid2, N_LineCnt  ";
           StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
           StrSql = StrSql + " Where (Down_W_1_QV_Real + W_1_QV_Real )  > 0 ";           
           StrSql = StrSql + " ) B";

           StrSql = StrSql + " Where A.Mbid=B.Nominid ";
           StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
           StrSql = StrSql + " And   A.Max_N_LineCnt = B.N_LineCnt  "; 

           Temp_Connect.Insert_Data(StrSql, Conn, tran);
           pg1.PerformStep(); pg1.Refresh();


           StrSql = "Update tbl_ClosePay_02 SET ";
           StrSql = StrSql + " Max_Down_W_2_QV_Real =  ISNULL(B.A1,0)";
           StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

           StrSql = StrSql + " (";
           StrSql = StrSql + " Select (Down_W_2_QV_Real + W_2_QV_Real ) A1 ";
           StrSql = StrSql + " ,Nominid,Nominid2, N_LineCnt  ";
           StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
           StrSql = StrSql + " Where (Down_W_2_QV_Real + W_2_QV_Real )  > 0 ";           
           StrSql = StrSql + " ) B";

           StrSql = StrSql + " Where A.Mbid=B.Nominid ";
           StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
           StrSql = StrSql + " And   A.Max_N_LineCnt = B.N_LineCnt  ";

           Temp_Connect.Insert_Data(StrSql, Conn, tran);
           pg1.PerformStep(); pg1.Refresh();


           StrSql = "Update tbl_ClosePay_02 SET ";
           StrSql = StrSql + " Max_Down_W_3_QV_Real =  ISNULL(B.A1,0)";
           StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

           StrSql = StrSql + " (";
           StrSql = StrSql + " Select (Down_W_3_QV_Real + W_3_QV_Real ) A1 ";
           StrSql = StrSql + " ,Nominid,Nominid2, N_LineCnt  ";
           StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
           StrSql = StrSql + " Where (Down_W_3_QV_Real + W_3_QV_Real )  > 0 ";           
           StrSql = StrSql + " ) B";

           StrSql = StrSql + " Where A.Mbid=B.Nominid ";
           StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
           StrSql = StrSql + " And   A.Max_N_LineCnt = B.N_LineCnt  ";

           Temp_Connect.Insert_Data(StrSql, Conn, tran);
           pg1.PerformStep(); pg1.Refresh();


           StrSql = "Update tbl_ClosePay_02 SET ";
           StrSql = StrSql + " Max_Down_W_4_QV_Real =  ISNULL(B.A1,0)";
           StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

           StrSql = StrSql + " (";
           StrSql = StrSql + " Select (Down_W_4_QV_Real + W_4_QV_Real ) A1 ";
           StrSql = StrSql + " ,Nominid,Nominid2, N_LineCnt  ";
           StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
           StrSql = StrSql + " Where (Down_W_4_QV_Real + W_4_QV_Real )  > 0 ";           
           StrSql = StrSql + " ) B";

           StrSql = StrSql + " Where A.Mbid=B.Nominid ";
           StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
           StrSql = StrSql + " And   A.Max_N_LineCnt = B.N_LineCnt  ";

           Temp_Connect.Insert_Data(StrSql, Conn, tran);
           pg1.PerformStep(); pg1.Refresh(); 
           
       }


       private void Put_Down_SumPV_Save(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
       {
           string StrSql = "";



           pg1.Value = 0; pg1.Maximum = MaxLevel + 9;
           pg1.PerformStep(); pg1.Refresh();

           int Cnt = MaxLevel;

           while (Cnt >= 0)
           {

               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " Down_W4_QV_Real_1 =  ISNULL(B.A2,0)";
               StrSql = StrSql + " ,Down_W_1_QV_Real_1 =    ISNULL(B.W_1,0)";
               StrSql = StrSql + " ,Down_W_2_QV_Real_1 =    ISNULL(B.W_2,0)";
               StrSql = StrSql + " ,Down_W_3_QV_Real_1 =    ISNULL(B.W_3,0)";
               StrSql = StrSql + " ,Down_W_4_QV_Real_1 =    ISNULL(B.W_4,0)";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + "Select  Sum(Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real  ) A2 ";
               StrSql = StrSql + " ,Sum(Down_W_1_QV_Real_1 + Down_W_1_QV_Real_2 + W_1_QV_Real ) W_1 ";
               StrSql = StrSql + " ,Sum(Down_W_2_QV_Real_1 + Down_W_2_QV_Real_2 + W_2_QV_Real ) W_2 ";
               StrSql = StrSql + " ,Sum(Down_W_3_QV_Real_1 + Down_W_3_QV_Real_2 + W_3_QV_Real ) W_3 ";
               StrSql = StrSql + " ,Sum(Down_W_4_QV_Real_1 + Down_W_4_QV_Real_2 +  W_4_QV_Real ) W_4 ";

               StrSql = StrSql + " ,Saveid,Saveid2 ";
               StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
               StrSql = StrSql + " Where ((  Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real ) <>0    )  ";
               StrSql = StrSql + " And   LevelCnt =" + Cnt;
               StrSql = StrSql + " And   LineCnt =  1 ";
               StrSql = StrSql + " Group By Saveid,Saveid2   ";
               StrSql = StrSql + " ) B";

               StrSql = StrSql + " Where A.Mbid=B.Saveid ";
               StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

               Temp_Connect.Insert_Data(StrSql, Conn, tran);


               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " Down_W4_QV_Real_2 =  ISNULL(B.A2,0)";
               StrSql = StrSql + " ,Down_W_1_QV_Real_2 =    ISNULL(B.W_1,0)";
               StrSql = StrSql + " ,Down_W_2_QV_Real_2 =    ISNULL(B.W_2,0)";
               StrSql = StrSql + " ,Down_W_3_QV_Real_2 =    ISNULL(B.W_3,0)";
               StrSql = StrSql + " ,Down_W_4_QV_Real_2 =    ISNULL(B.W_4,0)";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + "Select  Sum(Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real  ) A2 ";
               StrSql = StrSql + " ,Sum(Down_W_1_QV_Real_1 + Down_W_1_QV_Real_2 + W_1_QV_Real ) W_1 ";
               StrSql = StrSql + " ,Sum(Down_W_2_QV_Real_1 + Down_W_2_QV_Real_2 + W_2_QV_Real ) W_2 ";
               StrSql = StrSql + " ,Sum(Down_W_3_QV_Real_1 + Down_W_3_QV_Real_2 + W_3_QV_Real ) W_3 ";
               StrSql = StrSql + " ,Sum(Down_W_4_QV_Real_1 + Down_W_4_QV_Real_2 +  W_4_QV_Real ) W_4 ";

               StrSql = StrSql + " ,Saveid,Saveid2 ";
               StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
               StrSql = StrSql + " Where ((  Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real ) <>0    )  ";
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



       private void Put_Down_SumPV_002(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
       {

           



           pg1.Value = 0; pg1.Maximum = 4;
           pg1.PerformStep(); pg1.Refresh();
           string StrSql = "";


           StrSql = "Update tbl_ClosePay_02 SET G2_Cur_PV_1 = 0, G2_Cur_PV_2 = 0 , G3_Cur_PV_1= 0 ,G3_Cur_PV_2 = 0 ";

           Temp_Connect.Insert_Data(StrSql, Conn, tran);


           int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
           string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
           double Allowance1 = 0, TotalPV = 0, TH_TotalPV = 0, KR_TotalPV = 0;
           int TSW = 0 ;
           int TSW2 = 0; 

           int t_qu_Cnt = 0;
           Dictionary<int, string> t_qu = new Dictionary<int, string>();

           StrSql = " Select SellPV01 + SellPV02 +SellPV03   TotalPV , Se.M_Name,  Se.Mbid,Se.Mbid2 , Se.CurGrade ";
           StrSql = StrSql + " From tbl_ClosePay_02 Se (nolock) ";
           StrSql = StrSql + " WHERE SellPV01 + SellPV02 +SellPV03  <> 0  ";
           StrSql = StrSql + " And OneGrade < 20 ";
           StrSql = StrSql + " And  SellPV01 + SellPV02 +SellPV03 < 100000 ";
           StrSql = StrSql + " And  LevelCnt >= " + S_LevelCnt;
             
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

          
               S_Mbid = Mbid + "-" + Mbid2.ToString();
               if (Clo_Mem.ContainsKey(S_Mbid) == true)
               {
                   TSaveid = Clo_Mem[S_Mbid].Saveid;
                   TSaveid2 = Clo_Mem[S_Mbid].Saveid2;
                   TLine = Clo_Mem[S_Mbid].LineCnt;
               }

               OrderNumber = "";
               TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) ;

               TSW = 0;
               TSW2 = 0; 
               S_Mbid = TSaveid + "-" + TSaveid2.ToString();

               while (TSaveid != "**" && TSW == 0  )
               //while (TSaveid != "**" )
               {
                   LevelCnt++;

                   if (Clo_Mem.ContainsKey(S_Mbid) == true)
                   {
                       if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "")
                       {
                           if (TSW == 0)
                           {
                               StrSql = "Update tbl_ClosePay_02 SET ";
                               if (TLine == 1)
                                   StrSql = StrSql + " G2_Cur_PV_1 = G2_Cur_PV_1 +  " + TotalPV;
                               else
                                   StrSql = StrSql + " G2_Cur_PV_2 = G2_Cur_PV_2 +  " + TotalPV;

                               StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                               StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                               Temp_Connect.Insert_Data(StrSql, Conn, tran);
                               //t_qu[t_qu_Cnt] = StrSql;
                               //t_qu_Cnt++;
                           }

                           if (Clo_Mem[S_Mbid].CurGrade >= 20 
                               || Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03 >= 100000
                             //  || Clo_Mem[S_Mbid].Lvl == S_LevelCnt - 1
                               )
                               TSW = 1;

                           //if (TSW2 == 0)
                           //{
                           //    StrSql = "Update tbl_ClosePay_02 SET ";
                           //    if (TLine == 1)
                           //        StrSql = StrSql + " G3_Cur_PV_1 = G3_Cur_PV_1 +  " + TotalPV;
                           //    else
                           //        StrSql = StrSql + " G3_Cur_PV_2 = G3_Cur_PV_2 +  " + TotalPV;

                           //    StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                           //    StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                           //    t_qu[t_qu_Cnt] = StrSql;
                           //    t_qu_Cnt++;
                           //}

                           
                           //if (Clo_Mem[S_Mbid].CurGrade >= 30) TSW2 = 1; 

                           


                       }

                       TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;

                       S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                   }
                   else
                   {
                       TSaveid = "**";
                   }

                   //if (LevelCnt == S_LevelCnt - 1) TSaveid = "**";
                   
               } //While

           }



           //pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
           //foreach (int tkey in t_qu.Keys)
           //{
           //    StrSql = t_qu[tkey];
           //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
           //    pg1.PerformStep(); pg1.Refresh();
           //}





       }



       private void Put_Down_SumPV_003(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
       {
           pg1.Value = 0; pg1.Maximum = 4;
           pg1.PerformStep(); pg1.Refresh();
           string StrSql = "";

           int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
           string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
           double Allowance1 = 0, TotalPV = 0, TH_TotalPV = 0, KR_TotalPV = 0;
           int TSW = 0;
           int TSW2 = 0;

           int t_qu_Cnt = 0;
           Dictionary<int, string> t_qu = new Dictionary<int, string>();

           StrSql = " Select SellPV01 + SellPV02 +SellPV03   TotalPV , Se.M_Name,  Se.Mbid,Se.Mbid2 , Se.CurGrade ";
           StrSql = StrSql + " From tbl_ClosePay_02 Se (nolock) ";
           StrSql = StrSql + " WHERE SellPV01 + SellPV02 +SellPV03  <> 0  ";
           StrSql = StrSql + " And OneGrade < 30 ";
           StrSql = StrSql + " And  SellPV01 + SellPV02 +SellPV03 < 600000 ";
           StrSql = StrSql + " And  LevelCnt >= " + S_LevelCnt;
           
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


               S_Mbid = Mbid + "-" + Mbid2.ToString();
               if (Clo_Mem.ContainsKey(S_Mbid) == true)
               {
                   TSaveid = Clo_Mem[S_Mbid].Saveid;
                   TSaveid2 = Clo_Mem[S_Mbid].Saveid2;
                   TLine = Clo_Mem[S_Mbid].LineCnt;
               }

               OrderNumber = "";
               TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());

               TSW = 0;
               TSW2 = 0;
               S_Mbid = TSaveid + "-" + TSaveid2.ToString();

               while (TSaveid != "**" && TSW == 0)
               {
                   LevelCnt++;

                   if (Clo_Mem.ContainsKey(S_Mbid) == true)
                   {
                       if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "")
                       {
                           
                           if (TSW == 0)
                           {
                                StrSql = "Update tbl_ClosePay_02 SET ";
                                if (TLine == 1)
                                    StrSql = StrSql + " G3_Cur_PV_1 = G3_Cur_PV_1 +  " + TotalPV;
                                else
                                    StrSql = StrSql + " G3_Cur_PV_2 = G3_Cur_PV_2 +  " + TotalPV;

                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                //t_qu[t_qu_Cnt] = StrSql;
                                //t_qu_Cnt++;

                           }

                           if (Clo_Mem[S_Mbid].CurGrade >= 30 
                               || Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03 >= 600000
                               //|| Clo_Mem[S_Mbid].Lvl == S_LevelCnt - 1
                               ) 
                               TSW = 1;
                       }

                       TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;

                       S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                   }
                   else
                   {
                       TSaveid = "**";
                   }

                  // if (LevelCnt == S_LevelCnt - 1) TSaveid = "**";

               } //While

           }



           //pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
           //foreach (int tkey in t_qu.Keys)
           //{
           //    StrSql = t_qu[tkey];
           //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
           //    pg1.PerformStep(); pg1.Refresh();
           //}

       }





       private void Put_Down_SumPV_002_2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
       {





           //pg1.Value = 0; pg1.Maximum = 4;
           //pg1.PerformStep(); pg1.Refresh();
           //string StrSql = "";



           //int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
           //string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
           //double Allowance1 = 0, TotalPV = 0, TH_TotalPV = 0, KR_TotalPV = 0;
           //int TSW = 0;
           //int TSW2 = 0;

           //int t_qu_Cnt = 0;
           //Dictionary<int, string> t_qu = new Dictionary<int, string>();

           //StrSql = " Select SellPV01 + SellPV02 +SellPV03   TotalPV , Se.M_Name,  Se.Mbid,Se.Mbid2 , Se.CurGrade ";
           //StrSql = StrSql + " From tbl_ClosePay_02 Se (nolock) ";
           //StrSql = StrSql + " WHERE SellPV01 + SellPV02 +SellPV03  <> 0  ";
           //StrSql = StrSql + " And OneGrade < 20 ";
           //StrSql = StrSql + " And  SellPV01 + SellPV02 +SellPV03 < 100000 ";
           //StrSql = StrSql + " And  LevelCnt >= " + S_LevelCnt;

           //DataSet ds = new DataSet();
           //ReCnt = 0;
           //Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
           //ReCnt = Search_Connect.DataSet_ReCount;

           //pg1.Value = 0; pg1.Maximum = ReCnt + 1;

           //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
           //{
           //    LevelCnt = 0; TSaveid = "**";
           //    Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
           //    Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
           //    M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();


           //    S_Mbid = Mbid + "-" + Mbid2.ToString();
           //    if (Clo_Mem.ContainsKey(S_Mbid) == true)
           //    {
           //        TSaveid = Clo_Mem[S_Mbid].Saveid;
           //        TSaveid2 = Clo_Mem[S_Mbid].Saveid2;
           //        TLine = Clo_Mem[S_Mbid].LineCnt;
           //    }

           //    OrderNumber = "";
           //    TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());

           //    TSW = 0;
           //    TSW2 = 0;
           //    S_Mbid = TSaveid + "-" + TSaveid2.ToString();

           //    while (TSaveid != "**" && TSW == 0)
           //    //while (TSaveid != "**" )
           //    {
           //        LevelCnt++;

           //        if (Clo_Mem.ContainsKey(S_Mbid) == true)
           //        {
           //            if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "")
           //            {
           //                if (TSW == 0)
           //                {
           //                    StrSql = "Update tbl_ClosePay_02 SET ";
           //                    if (TLine == 1)
           //                        StrSql = StrSql + " G2_Cur_PV_1 = G2_Cur_PV_1 +  " + TotalPV;
           //                    else
           //                        StrSql = StrSql + " G2_Cur_PV_2 = G2_Cur_PV_2 +  " + TotalPV;

           //                    StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
           //                    StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

           //                    t_qu[t_qu_Cnt] = StrSql;
           //                    t_qu_Cnt++;
           //                }

           //                if (Clo_Mem[S_Mbid].CurGrade >= 20 || Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03 >= 100000) TSW = 1;

           //                //if (TSW2 == 0)
           //                //{
           //                //    StrSql = "Update tbl_ClosePay_02 SET ";
           //                //    if (TLine == 1)
           //                //        StrSql = StrSql + " G3_Cur_PV_1 = G3_Cur_PV_1 +  " + TotalPV;
           //                //    else
           //                //        StrSql = StrSql + " G3_Cur_PV_2 = G3_Cur_PV_2 +  " + TotalPV;

           //                //    StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
           //                //    StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

           //                //    t_qu[t_qu_Cnt] = StrSql;
           //                //    t_qu_Cnt++;
           //                //}


           //                //if (Clo_Mem[S_Mbid].CurGrade >= 30) TSW2 = 1; 




           //            }

           //            TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;

           //            S_Mbid = TSaveid + "-" + TSaveid2.ToString();
           //        }
           //        else
           //        {
           //            TSaveid = "**";
           //        }

           //        if (LevelCnt == S_LevelCnt - 1) TSaveid = "**";

           //    } //While

           //}



           //pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
           //foreach (int tkey in t_qu.Keys)
           //{
           //    StrSql = t_qu[tkey];
           //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
           //    pg1.PerformStep(); pg1.Refresh();
           //}

           string StrSql = "";
           pg1.Value = 0; pg1.Maximum = (MaxLevel * 7 ) + 7 ;
           pg1.PerformStep(); pg1.Refresh();

           int Cnt = MaxLevel;

           while (Cnt >= 0)
           {
               label18.Text = Cnt.ToString(); label18.Refresh(); 

               GiveGrade1(Temp_Connect, Conn, tran, Cnt );
               pg2.PerformStep(); pg2.Refresh();

               GiveGrade2(Temp_Connect, Conn, tran, Cnt );   // NP    G2_Cur_PV_1
               pg2.PerformStep(); pg2.Refresh();

               GiveGrade3(Temp_Connect, Conn, tran, Cnt );  // BP     G3_Cur_PV_1                
               pg2.PerformStep(); pg2.Refresh();

               GiveGrade4(Temp_Connect, Conn, tran, Cnt );  //SP       G3_Cur_PV_1                
               pg2.PerformStep(); pg2.Refresh();

               GiveGrade5(Temp_Connect, Conn, tran, Cnt );  //AP     G3_Cur_PV_1                
               pg2.PerformStep(); pg2.Refresh();

               //2016-11-07 박해진 대리 요청에 의해서 소매매출은 상위로 잡을때 PV는  50%만 잡아줘라.
               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " G2_Cur_PV_1 =  ISNULL(B.A2,0)";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + "Select    Sum(G2_Cur_PV_1 + G2_Cur_PV_2 + SellPV01 + (SellPV02 * 0.5) +SellPV03) A2 ";
               StrSql = StrSql + " ,Saveid,Saveid2 ";
               StrSql = StrSql + " From tbl_ClosePay_02 ";               
               StrSql = StrSql + " Where   LevelCnt =" + Cnt;
               StrSql = StrSql + " And   LineCnt =  1 ";               
               StrSql = StrSql + " And   OneGrade < 20 ";
               StrSql = StrSql + " And   SellPV01 + SellPV02 +SellPV03 < 100000 ";               
               StrSql = StrSql + " Group By Saveid,Saveid2   ";
               StrSql = StrSql + " ) B";

               StrSql = StrSql + " Where A.Mbid=B.Saveid ";
               StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

               Temp_Connect.Insert_Data(StrSql, Conn, tran);


               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " G2_Cur_PV_1 = G2_Cur_PV_1 +   ISNULL(B.A2,0)";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + "Select    Sum(G2_Cur_PV_1 + G2_Cur_PV_2 ) A2 ";
               StrSql = StrSql + " ,Saveid,Saveid2 ";
               StrSql = StrSql + " From tbl_ClosePay_02 ";
               StrSql = StrSql + " Where   LevelCnt =" + Cnt;
               StrSql = StrSql + " And   LineCnt =  1 ";
               StrSql = StrSql + " And   LeaveDate <> '' ";
               StrSql = StrSql + " And   SellPV01 + SellPV02 +SellPV03 >= 100000 ";               
               StrSql = StrSql + " Group By Saveid,Saveid2   ";
               StrSql = StrSql + " ) B";

               StrSql = StrSql + " Where A.Mbid=B.Saveid ";
               StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

               Temp_Connect.Insert_Data(StrSql, Conn, tran);
               pg1.PerformStep(); pg1.Refresh();



               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " G2_Cur_PV_2 =  ISNULL(B.A2,0)";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + "Select    Sum(G2_Cur_PV_1 + G2_Cur_PV_2 + SellPV01 + (SellPV02 * 0.5) +SellPV03) A2 ";
               StrSql = StrSql + " ,Saveid,Saveid2 ";
               StrSql = StrSql + " From tbl_ClosePay_02 ";               
               StrSql = StrSql + " Where   LevelCnt =" + Cnt;
               StrSql = StrSql + " And   LineCnt =  2 ";
               StrSql = StrSql + " And   OneGrade < 20 ";
               StrSql = StrSql + " And   SellPV01 + SellPV02 +SellPV03 < 100000 ";               
               StrSql = StrSql + " Group By Saveid,Saveid2   ";
               StrSql = StrSql + " ) B";

               StrSql = StrSql + " Where A.Mbid=B.Saveid ";
               StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

               Temp_Connect.Insert_Data(StrSql, Conn, tran);


               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " G2_Cur_PV_2 = G2_Cur_PV_2 +   ISNULL(B.A2,0)";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + "Select    Sum(G2_Cur_PV_1 + G2_Cur_PV_2 ) A2 ";
               StrSql = StrSql + " ,Saveid,Saveid2 ";
               StrSql = StrSql + " From tbl_ClosePay_02 ";
               StrSql = StrSql + " Where   LevelCnt =" + Cnt;
               StrSql = StrSql + " And   LineCnt =  2 ";
               StrSql = StrSql + " And   LeaveDate <> '' ";
               StrSql = StrSql + " And   SellPV01 + SellPV02 +SellPV03 >= 100000 ";               
               StrSql = StrSql + " Group By Saveid,Saveid2   ";
               StrSql = StrSql + " ) B";

               StrSql = StrSql + " Where A.Mbid=B.Saveid ";
               StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

               Temp_Connect.Insert_Data(StrSql, Conn, tran);
               pg1.PerformStep(); pg1.Refresh();



               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " G3_Cur_PV_1 =  ISNULL(B.A2,0)";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + "Select    Sum(G3_Cur_PV_1 + G3_Cur_PV_2 + SellPV01 + (SellPV02 * 0.5) +SellPV03) A2 ";
               StrSql = StrSql + " ,Saveid,Saveid2 ";
               StrSql = StrSql + " From tbl_ClosePay_02 ";               
               StrSql = StrSql + " Where   LevelCnt =" + Cnt;
               StrSql = StrSql + " And   LineCnt =  1 ";
               StrSql = StrSql + " And   OneGrade < 30 ";
               StrSql = StrSql + " And   SellPV01 + SellPV02 +SellPV03 < 600000 ";               
               StrSql = StrSql + " Group By Saveid,Saveid2   ";
               StrSql = StrSql + " ) B";

               StrSql = StrSql + " Where A.Mbid=B.Saveid ";
               StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

               Temp_Connect.Insert_Data(StrSql, Conn, tran);

               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " G3_Cur_PV_1 = G3_Cur_PV_1 +   ISNULL(B.A2,0)";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + "Select    Sum(G3_Cur_PV_1 + G3_Cur_PV_2 ) A2 ";
               StrSql = StrSql + " ,Saveid,Saveid2 ";
               StrSql = StrSql + " From tbl_ClosePay_02 ";
               StrSql = StrSql + " Where   LevelCnt =" + Cnt;
               StrSql = StrSql + " And   LineCnt =  1 ";
               StrSql = StrSql + " And   LeaveDate <> '' ";
               StrSql = StrSql + " And   SellPV01 + SellPV02 +SellPV03 >= 600000 ";               
               StrSql = StrSql + " Group By Saveid,Saveid2   ";
               StrSql = StrSql + " ) B";

               StrSql = StrSql + " Where A.Mbid=B.Saveid ";
               StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

               Temp_Connect.Insert_Data(StrSql, Conn, tran);
               pg1.PerformStep(); pg1.Refresh();

               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " G3_Cur_PV_2 =  ISNULL(B.A2,0)";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + "Select    Sum(G3_Cur_PV_1 + G3_Cur_PV_2 + SellPV01 + (SellPV02 * 0.5) +SellPV03) A2 ";
               StrSql = StrSql + " ,Saveid,Saveid2 ";
               StrSql = StrSql + " From tbl_ClosePay_02 ";
               //StrSql = StrSql + " Where (  G3_Cur_PV_1 + G3_Cur_PV_2 + SellPV01 + SellPV02 +SellPV03 ) <>0   ";
               StrSql = StrSql + " Where   LevelCnt =" + Cnt;
               StrSql = StrSql + " And   LineCnt =  2 ";
               StrSql = StrSql + " And   OneGrade < 30 ";
               StrSql = StrSql + " And   SellPV01 + SellPV02 +SellPV03 < 600000 ";               
               StrSql = StrSql + " Group By Saveid,Saveid2   ";
               StrSql = StrSql + " ) B";

               StrSql = StrSql + " Where A.Mbid=B.Saveid ";
               StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

               Temp_Connect.Insert_Data(StrSql, Conn, tran);


               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " G3_Cur_PV_2 =  G3_Cur_PV_2 + ISNULL(B.A2,0)";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + "Select    Sum(G3_Cur_PV_1 + G3_Cur_PV_2 ) A2 ";
               StrSql = StrSql + " ,Saveid,Saveid2 ";
               StrSql = StrSql + " From tbl_ClosePay_02 ";
               //StrSql = StrSql + " Where (  G3_Cur_PV_1 + G3_Cur_PV_2 + SellPV01 + SellPV02 +SellPV03 ) <>0   ";
               StrSql = StrSql + " Where   LevelCnt =" + Cnt;
               StrSql = StrSql + " And   LineCnt =  2 ";
               StrSql = StrSql + " And   LeaveDate <> '' ";
               StrSql = StrSql + " And   SellPV01 + SellPV02 +SellPV03 >= 600000 ";  
               StrSql = StrSql + " Group By Saveid,Saveid2   ";
               StrSql = StrSql + " ) B";

               StrSql = StrSql + " Where A.Mbid=B.Saveid ";
               StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

               Temp_Connect.Insert_Data(StrSql, Conn, tran);
               pg1.PerformStep(); pg1.Refresh();



               //StrSql = "Update tbl_ClosePay_02 SET ";
               //StrSql = StrSql + " G2_Cur_PV_1 =  0 ";
               //StrSql = StrSql + " ,G2_Cur_PV_2 =  0 ";
               //StrSql = StrSql + " ,G3_Cur_PV_1 =  0 ";
               //StrSql = StrSql + " ,G3_Cur_PV_2 =  0 ";
               //StrSql = StrSql + " Where   LevelCnt =" + Cnt;
               //StrSql = StrSql + " And   LeaveDate <> '' ";

               //Temp_Connect.Insert_Data(StrSql, Conn, tran);

              
               Cnt = Cnt - 1;

           }
       }



       private void Put_Down_SumPV_003_2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
       {
           pg1.Value = 0; pg1.Maximum = 4;
           pg1.PerformStep(); pg1.Refresh();
           string StrSql = "";

           int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
           string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
           double Allowance1 = 0, TotalPV = 0, TH_TotalPV = 0, KR_TotalPV = 0;
           int TSW = 0;
           int TSW2 = 0;

           int t_qu_Cnt = 0;
           Dictionary<int, string> t_qu = new Dictionary<int, string>();

           StrSql = " Select SellPV01 + SellPV02 +SellPV03   TotalPV , Se.M_Name,  Se.Mbid,Se.Mbid2 , Se.CurGrade ";
           StrSql = StrSql + " From tbl_ClosePay_02 Se (nolock) ";
           StrSql = StrSql + " WHERE SellPV01 + SellPV02 +SellPV03  <> 0  ";
           StrSql = StrSql + " And OneGrade < 30 ";
           StrSql = StrSql + " And  SellPV01 + SellPV02 +SellPV03 < 600000 ";
           StrSql = StrSql + " And  LevelCnt >= " + S_LevelCnt;

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


               S_Mbid = Mbid + "-" + Mbid2.ToString();
               if (Clo_Mem.ContainsKey(S_Mbid) == true)
               {
                   TSaveid = Clo_Mem[S_Mbid].Saveid;
                   TSaveid2 = Clo_Mem[S_Mbid].Saveid2;
                   TLine = Clo_Mem[S_Mbid].LineCnt;
               }

               OrderNumber = "";
               TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());

               TSW = 0;
               TSW2 = 0;
               S_Mbid = TSaveid + "-" + TSaveid2.ToString();

               while (TSaveid != "**" && TSW == 0)
               {
                   LevelCnt++;

                   if (Clo_Mem.ContainsKey(S_Mbid) == true)
                   {
                       if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "")
                       {

                           if (TSW == 0)
                           {
                               StrSql = "Update tbl_ClosePay_02 SET ";
                               if (TLine == 1)
                                   StrSql = StrSql + " G3_Cur_PV_1 = G3_Cur_PV_1 +  " + TotalPV;
                               else
                                   StrSql = StrSql + " G3_Cur_PV_2 = G3_Cur_PV_2 +  " + TotalPV;

                               StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                               StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                               t_qu[t_qu_Cnt] = StrSql;
                               t_qu_Cnt++;

                           }

                           if (Clo_Mem[S_Mbid].CurGrade >= 30 || Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03 >= 600000) TSW = 1;
                       }

                       TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;

                       S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                   }
                   else
                   {
                       TSaveid = "**";
                   }

                   if (LevelCnt == S_LevelCnt - 1) TSaveid = "**";

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


       //private void Put_Down_SumPV_4(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
       //{
       //    string StrSql = "";
                      
       //    string  str_GradeCnt = "", str_GradeCnt1 = "", str_GradeCnt2 = "", str_GradeCnt3 = "";

       //    Dictionary<int, string> dic_ToEndDate = new Dictionary<int, string>();


       //    dic_ToEndDate[0] = ToEndDate;   //4,3,2,1,0 순으로 보면됨
       //    //=====================================================================================
       //    //=====================================================================================
       //    //StrSql = "select top 3 fromenddate , ToEndDate from dbo.tbl_CloseTotal_02 Order by ToEndDate DESC";
       //    StrSql = "select top 4 fromenddate , ToEndDate from dbo.tbl_CloseTotal_02  Where ToEndDate <> '" + ToEndDate + "' Order by ToEndDate DESC";
       //    string SDate3 = "", SDate4 = "";
       //    int ReCnt = 0;
       //    DataSet Dset4 = new DataSet();
       //    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
       //    ReCnt = Search_Connect.DataSet_ReCount;

       //    if (ReCnt > 0)
       //    {
       //        for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
       //        {
       //            SDate3 = Dset4.Tables[base_db_name].Rows[fi_cnt]["fromenddate"].ToString();
       //            SDate4 = Dset4.Tables[base_db_name].Rows[fi_cnt]["ToEndDate"].ToString();

       //            dic_ToEndDate[fi_cnt+1] = SDate4;  //3,2,1,0 순으로 보면됨

       //            //이번달의 반품과 실매출을 합산하거를 뽑아온다.....
       //            StrSql = " Update tbl_ClosePay_02 SET";

       //            if (fi_cnt == 0) StrSql = StrSql + " Day4_Sum_PV_3 = IsNull(b.A1, 0) , G4_Date_3 ='" + SDate4 + "'";
       //            if (fi_cnt == 1) StrSql = StrSql + " Day4_Sum_PV_2 = IsNull(b.A1, 0) , G4_Date_2 ='" + SDate4 + "'";
       //            if (fi_cnt == 2) StrSql = StrSql + " Day4_Sum_PV_1 = IsNull(b.A1, 0) , G4_Date_1 ='" + SDate4 + "'";
       //            if (fi_cnt == 3) StrSql = StrSql + " Day4_Sum_PV_0 = IsNull(b.A1, 0) , G4_Date_0 ='" + SDate4 + "'";

       //            StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
       //            StrSql = StrSql + " (";
       //            StrSql = StrSql + " Select  Sum(TotalPV) AS A1 ";
       //            StrSql = StrSql + " ,Mbid,Mbid2 ";
       //            StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
       //            StrSql = StrSql + " Where   SellDate_2 >= '" + SDate3 + "'";
       //            StrSql = StrSql + " And     SellDate_2 <= '" + SDate4 + "'";
       //            StrSql = StrSql + " And   Ga_Order = 0 ";
       //            StrSql = StrSql + " And  SellCode = '01' ";
       //            StrSql = StrSql + " Group By Mbid,Mbid2";
       //            StrSql = StrSql + " ) B";
       //            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
       //            StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

       //            Temp_Connect.Insert_Data(StrSql, Conn, tran);


       //            //2016-11-07 박해진 대리 요청에 의해서 소매매출은 상위로 잡을때 PV는  50%만 잡아줘라.
       //            StrSql = " Update tbl_ClosePay_02 SET";
       //            if (fi_cnt == 0)  StrSql = StrSql + " Day4_Sum_PV_3 =Day4_Sum_PV_3 +  (IsNull(b.A1, 0) * 0.5 ) ";
       //            if (fi_cnt == 1) StrSql = StrSql + " Day4_Sum_PV_2 =Day4_Sum_PV_2 +  (IsNull(b.A1, 0) * 0.5 ) ";
       //            if (fi_cnt == 2) StrSql = StrSql + " Day4_Sum_PV_1 =Day4_Sum_PV_1 +  (IsNull(b.A1, 0) * 0.5 ) ";
       //            if (fi_cnt == 3) StrSql = StrSql + " Day4_Sum_PV_0 =Day4_Sum_PV_0 +  (IsNull(b.A1, 0) * 0.5 ) ";

       //            StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
       //            StrSql = StrSql + " (";
       //            StrSql = StrSql + " Select  Sum(TotalPV) AS A1 ";
       //            StrSql = StrSql + " ,Mbid,Mbid2 ";
       //            StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
       //            StrSql = StrSql + " Where   SellDate_2 >= '" + SDate3 + "'";
       //            StrSql = StrSql + " And     SellDate_2 <= '" + SDate4 + "'";
       //            StrSql = StrSql + " And   Ga_Order = 0 ";
       //            StrSql = StrSql + " And  SellCode = '02' ";
       //            StrSql = StrSql + " Group By Mbid,Mbid2";
       //            StrSql = StrSql + " ) B";
       //            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
       //            StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

       //            Temp_Connect.Insert_Data(StrSql, Conn, tran);



       //            //날짜과련 셋팅을 넣는다.
       //            StrSql = " Update tbl_ClosePay_02 SET";
       //            if (fi_cnt == 0) StrSql = StrSql + "  G4_Date_3 ='" + SDate4 + "'";
       //            if (fi_cnt == 1) StrSql = StrSql + "  G4_Date_2 ='" + SDate4 + "'";
       //            if (fi_cnt == 2) StrSql = StrSql + "  G4_Date_1 ='" + SDate4 + "'";
       //            if (fi_cnt == 3) StrSql = StrSql + "  G4_Date_0 ='" + SDate4 + "'";

                   
       //            Temp_Connect.Insert_Data(StrSql, Conn, tran);

       //        }


       //       // SDate3 = Dset4.Tables[base_db_name].Rows[ReCnt - 1]["fromenddate"].ToString();
       //    }

           


       //    //이번마감동안의 본인 매출 합산을 뽑아온다.
       //    //이번달의 반품과 실매출을 합산하거를 뽑아온다.....
       //    StrSql = " Update tbl_ClosePay_02 SET";
       //    StrSql = StrSql + " Day4_Sum_PV_4 = IsNull(b.A1, 0)";
       //    StrSql = StrSql + ",G4_Date_4 = '" + ToEndDate + "'";
       //    StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
       //    StrSql = StrSql + " (";
       //    StrSql = StrSql + " Select  Sum(TotalPV) AS A1 ";
       //    StrSql = StrSql + " ,Mbid,Mbid2 ";
       //    StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
       //    StrSql = StrSql + " Where   SellDate_2 >= '" + FromEndDate + "'";
       //    StrSql = StrSql + " And     SellDate_2 <= '" + ToEndDate + "'";
       //    StrSql = StrSql + " And   Ga_Order = 0 ";
       //    StrSql = StrSql + " And  SellCode = '01' ";
       //    StrSql = StrSql + " Group By Mbid,Mbid2";
       //    StrSql = StrSql + " ) B";
       //    StrSql = StrSql + " Where a.Mbid = b.Mbid ";
       //    StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

       //    Temp_Connect.Insert_Data(StrSql, Conn, tran);


       //    //2016-11-07 박해진 대리 요청에 의해서 소매매출은 상위로 잡을때 PV는  50%만 잡아줘라.
       //    StrSql = " Update tbl_ClosePay_02 SET";
       //    StrSql = StrSql + " Day4_Sum_PV_4 =Day4_Sum_PV_4 +  (IsNull(b.A1, 0) * 0.5 ) ";
       //    StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
       //    StrSql = StrSql + " (";
       //    StrSql = StrSql + " Select  Sum(TotalPV) AS A1 ";
       //    StrSql = StrSql + " ,Mbid,Mbid2 ";
       //    StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
       //    StrSql = StrSql + " Where   SellDate_2 >= '" + FromEndDate + "'";
       //    StrSql = StrSql + " And     SellDate_2 <= '" + ToEndDate + "'";
       //    StrSql = StrSql + " And   Ga_Order = 0 ";
       //    StrSql = StrSql + " And  SellCode = '02' ";
       //    StrSql = StrSql + " Group By Mbid,Mbid2";
       //    StrSql = StrSql + " ) B";
       //    StrSql = StrSql + " Where a.Mbid = b.Mbid ";
       //    StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

       //    Temp_Connect.Insert_Data(StrSql, Conn, tran);


       //    StrSql = " Update tbl_ClosePay_02 SET";
       //    StrSql = StrSql + "  G4_Date_4 ='" + ToEndDate + "'";          

       //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
       //    //=====================================================================================
       //    //=====================================================================================





       //    ////++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
       //    ////++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
       //    ////이번달의 반품과 실매출을 합산하거를 뽑아온다.....4주 기준으로 해서 뽑아온다.
       //    //StrSql = " Update tbl_ClosePay_02 SET";
       //    //StrSql = StrSql + " Day_Sum_PV_4 = IsNull(b.A1, 0)";
       //    //StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
       //    //StrSql = StrSql + " (";
       //    //StrSql = StrSql + " Select  Sum(TotalPV) AS A1 ";
       //    //StrSql = StrSql + " ,Mbid,Mbid2 ";
       //    //StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
       //    //StrSql = StrSql + " Where   SellDate_2 >= '" + SDate3 + "'";
       //    //StrSql = StrSql + " And     SellDate_2 <= '" + ToEndDate + "'";
       //    //StrSql = StrSql + " And   Ga_Order = 0 ";
       //    //StrSql = StrSql + " And  SellCode = '01' "; 
       //    //StrSql = StrSql + " Group By Mbid,Mbid2";
       //    //StrSql = StrSql + " ) B";
       //    //StrSql = StrSql + " Where a.Mbid = b.Mbid ";
       //    //StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

       //    //Temp_Connect.Insert_Data(StrSql, Conn, tran);



       //    ////2016-11-07 박해진 대리 요청에 의해서 소매매출은 상위로 잡을때 PV는  50%만 잡아줘라.
       //    //StrSql = " Update tbl_ClosePay_02 SET";
       //    //StrSql = StrSql + " Day_Sum_PV_4 =Day_Sum_PV_4 +  (IsNull(b.A1, 0) * 0.5 ) ";
       //    //StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
       //    //StrSql = StrSql + " (";
       //    //StrSql = StrSql + " Select  Sum(TotalPV) AS A1 ";
       //    //StrSql = StrSql + " ,Mbid,Mbid2 ";
       //    //StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
       //    //StrSql = StrSql + " Where   SellDate_2 >= '" + SDate3 + "'";
       //    //StrSql = StrSql + " And     SellDate_2 <= '" + ToEndDate + "'";
       //    //StrSql = StrSql + " And   Ga_Order = 0 ";
       //    //StrSql = StrSql + " And  SellCode = '02' "; 
       //    //StrSql = StrSql + " Group By Mbid,Mbid2";
       //    //StrSql = StrSql + " ) B";
       //    //StrSql = StrSql + " Where a.Mbid = b.Mbid ";
       //    //StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

       //    //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                      

       //    //StrSql = " Update tbl_ClosePay_02 SET";
       //    //StrSql = StrSql + " D_PV_4 = IsNull(b.A1, 0)";
       //    //StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
       //    //StrSql = StrSql + " (";
       //    //StrSql = StrSql + " Select  Sum(Day_Sum_PV_4) A1 ";
       //    //StrSql = StrSql + " ,Nominid,Nominid2 ";
       //    //StrSql = StrSql + " From tbl_ClosePay_02 (nolock)  ";
       //    //StrSql = StrSql + " Where   Sell_Mem_TF = 1 ";
       //    //StrSql = StrSql + " Group by Nominid , Nominid2 ";
       //    //StrSql = StrSql + " ) B";
       //    //StrSql = StrSql + " Where a.Mbid = b.Nominid ";
       //    //StrSql = StrSql + " And   a.Mbid2 = b.Nominid2 ";

       //    //Temp_Connect.Insert_Data(StrSql, Conn, tran);
       //    ////++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
       //    ////++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++




       //    ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
       //    ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
       //    ////이번달의 반품과 실매출을 합산하거를 뽑아온다.....
       //    //StrSql = " Update tbl_ClosePay_02 SET";
       //    //StrSql = StrSql + " Day_Sum_PV_5 = IsNull(b.A1, 0)";
       //    //StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
       //    //StrSql = StrSql + " (";
       //    //StrSql = StrSql + " Select  Sum(TotalPV) AS A1 ";
       //    //StrSql = StrSql + " ,Mbid,Mbid2 ";
       //    //StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
       //    //StrSql = StrSql + " Where   LEFT(SellDate_2,6) = '" + ToEndDate.Substring(0, 6) + "'";
       //    //StrSql = StrSql + " And   Ga_Order = 0 ";
       //    //StrSql = StrSql + " And   SellCode = '01' ";
       //    //StrSql = StrSql + " Group By Mbid,Mbid2";
       //    //StrSql = StrSql + " ) B";
       //    //StrSql = StrSql + " Where a.Mbid = b.Mbid ";
       //    //StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

       //    //Temp_Connect.Insert_Data(StrSql, Conn, tran);

       //    ////2016-11-07 박해진 대리 요청에 의해서 소매매출은 상위로 잡을때 PV는  50%만 잡아줘라.
       //    //StrSql = " Update tbl_ClosePay_02 SET";
       //    //StrSql = StrSql + " Day_Sum_PV_5 = Day_Sum_PV_5 + (IsNull(b.A1, 0) * 0.5)";
       //    //StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
       //    //StrSql = StrSql + " (";
       //    //StrSql = StrSql + " Select  Sum(TotalPV) AS A1 ";
       //    //StrSql = StrSql + " ,Mbid,Mbid2 ";
       //    //StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
       //    //StrSql = StrSql + " Where   LEFT(SellDate_2,6) = '" + ToEndDate.Substring(0, 6) + "'";
       //    //StrSql = StrSql + " And   Ga_Order = 0 ";
       //    //StrSql = StrSql + " And   SellCode = '02' ";
       //    //StrSql = StrSql + " Group By Mbid,Mbid2";
       //    //StrSql = StrSql + " ) B";
       //    //StrSql = StrSql + " Where a.Mbid = b.Mbid ";
       //    //StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

       //    //Temp_Connect.Insert_Data(StrSql, Conn, tran);


       //    //StrSql = " Update tbl_ClosePay_02 SET";
       //    //StrSql = StrSql + " D_PV_5 = IsNull(b.A1, 0)";
       //    //StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
       //    //StrSql = StrSql + " (";
       //    //StrSql = StrSql + " Select  Sum(Day_Sum_PV_5) A1 ";
       //    //StrSql = StrSql + " ,Nominid,Nominid2 ";
       //    //StrSql = StrSql + " From tbl_ClosePay_02 (nolock)  ";
       //    //StrSql = StrSql + " Where   Sell_Mem_TF = 1 ";
       //    //StrSql = StrSql + " Group by Nominid , Nominid2 ";
       //    //StrSql = StrSql + " ) B";
       //    //StrSql = StrSql + " Where a.Mbid = b.Nominid ";
       //    //StrSql = StrSql + " And   a.Mbid2 = b.Nominid2 ";

       //    //Temp_Connect.Insert_Data(StrSql, Conn, tran);
       //    ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
       //    ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

       //    pg1.Value = 0; pg1.Maximum = MaxLevel + 2;
       //    pg1.PerformStep(); pg1.Refresh();

       //    int Cnt = MaxLevel;

       //    while (Cnt >= 0)
       //    {

       //        StrSql = "Update tbl_ClosePay_02 SET ";
       //        //StrSql = StrSql + " G_Cur_PV_4_1 =  ISNULL(B.A2,0)";
       //        StrSql = StrSql + " G4_Cur_PV_4_1 =  ISNULL(B.G4,0)";
       //        StrSql = StrSql + " ,G4_Cur_PV_3_1 =  ISNULL(B.G3,0)";
       //        StrSql = StrSql + " ,G4_Cur_PV_2_1 =  ISNULL(B.G2,0)";
       //        StrSql = StrSql + " ,G4_Cur_PV_1_1 =  ISNULL(B.G1,0)";
       //        StrSql = StrSql + " ,G4_Cur_PV_0_1 =  ISNULL(B.G0,0)";
       //        StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

       //        StrSql = StrSql + " (";
       //        StrSql = StrSql + "Select  " ;//   Sum(G_Cur_PV_4_1 + G_Cur_PV_4_2 + Day_Sum_PV_4) A2 ";
       //        StrSql = StrSql + "  Sum(G4_Cur_PV_4_1 + G4_Cur_PV_4_2 + Day4_Sum_PV_4) G4 ";
       //        StrSql = StrSql + " , Sum(G4_Cur_PV_3_1 + G4_Cur_PV_3_2 + Day4_Sum_PV_3) G3 ";
       //        StrSql = StrSql + " , Sum(G4_Cur_PV_2_1 + G4_Cur_PV_2_2 + Day4_Sum_PV_2) G2 ";
       //        StrSql = StrSql + " , Sum(G4_Cur_PV_1_1 + G4_Cur_PV_1_2 + Day4_Sum_PV_1) G1 ";
       //        StrSql = StrSql + " , Sum(G4_Cur_PV_0_1 + G4_Cur_PV_0_2 + Day4_Sum_PV_0) G0 ";
       //        StrSql = StrSql + " ,Saveid,Saveid2 ";
       //        StrSql = StrSql + " From tbl_ClosePay_02 ";               
       //        StrSql = StrSql + " Where   LevelCnt =" + Cnt;
       //        StrSql = StrSql + " And   LineCnt =  1 ";
       //        StrSql = StrSql + " Group By Saveid,Saveid2   ";
       //        StrSql = StrSql + " ) B";

       //        StrSql = StrSql + " Where A.Mbid=B.Saveid ";
       //        StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

       //        Temp_Connect.Insert_Data(StrSql, Conn, tran);


       //        StrSql = "Update tbl_ClosePay_02 SET ";
       //        //StrSql = StrSql + " G_Cur_PV_4_2 =  ISNULL(B.A2,0)";
       //        StrSql = StrSql + " G4_Cur_PV_4_2 =  ISNULL(B.G4,0)";
       //        StrSql = StrSql + " ,G4_Cur_PV_3_2 =  ISNULL(B.G3,0)";
       //        StrSql = StrSql + " ,G4_Cur_PV_2_2 =  ISNULL(B.G2,0)";
       //        StrSql = StrSql + " ,G4_Cur_PV_1_2 =  ISNULL(B.G1,0)";
       //        StrSql = StrSql + " ,G4_Cur_PV_0_2 =  ISNULL(B.G0,0)";
       //        StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

       //        StrSql = StrSql + " (";
       //        StrSql = StrSql + "Select  " ;//   Sum(G_Cur_PV_4_1 + G_Cur_PV_4_2 + Day_Sum_PV_4  ) A2 ";
       //        StrSql = StrSql + "  Sum(G4_Cur_PV_4_1 + G4_Cur_PV_4_2 + Day4_Sum_PV_4) G4 ";
       //        StrSql = StrSql + " , Sum(G4_Cur_PV_3_1 + G4_Cur_PV_3_2 + Day4_Sum_PV_3) G3 ";
       //        StrSql = StrSql + " , Sum(G4_Cur_PV_2_1 + G4_Cur_PV_2_2 + Day4_Sum_PV_2) G2 ";
       //        StrSql = StrSql + " , Sum(G4_Cur_PV_1_1 + G4_Cur_PV_1_2 + Day4_Sum_PV_1) G1 ";
       //        StrSql = StrSql + " , Sum(G4_Cur_PV_0_1 + G4_Cur_PV_0_2 + Day4_Sum_PV_0) G0 ";
       //        StrSql = StrSql + " ,Saveid,Saveid2 ";
       //        StrSql = StrSql + " From tbl_ClosePay_02 ";               
       //        StrSql = StrSql + " Where   LevelCnt =" + Cnt;
       //        StrSql = StrSql + " And   LineCnt =  2 ";
       //        StrSql = StrSql + " Group By Saveid,Saveid2   ";
       //        StrSql = StrSql + " ) B";

       //        StrSql = StrSql + " Where A.Mbid=B.Saveid ";
       //        StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

       //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
       //        pg1.PerformStep(); pg1.Refresh();

       //        Cnt = Cnt - 1;
       //    }



       //    //G_Cur_PV_4_1 + G_Cur_PV_4_2 가 4주차로 승급하는 것이기때문에 여기에 값을 넣어주어야함  4주차
       //    //G_Cur_PV_5_1 + G_Cur_PV_5_2 가 이번달 하선 매출로 승급하는 것이기때문에 여기에 값을 넣어주어야함  이번달값


       //    //G4_Cur_PV_4_1 + G4_Cur_PV_4_2 = 이번 마감 기간 동안의 하선 누적
       //    //G4_Cur_PV_3_1 + G4_Cur_PV_3_2 = 전 마감 기간 동안의 하선 누적
       //    //G4_Cur_PV_2_1 + G4_Cur_PV_2_2 = 전전 마감 기간 동안의 하선 누적
       //    //G4_Cur_PV_1_1 + G4_Cur_PV_1_2 = 전전전 마감 기간 동안의 하선 누적
       //    //G4_Cur_PV_0_1 + G4_Cur_PV_0_2 = 전전전전 마감 기간 동안의 하선 누적


       //    //GradeDate3 BP달성 마감..

       //    ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
       //    ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
       //    //4주차 전보다 더 전에 BP가 되엇으면 다 누적 받는다.
       //    StrSql = "Update tbl_ClosePay_02 Set ";
       //    StrSql = StrSql + " G_Cur_PV_4_1 =  G4_Cur_PV_1_1 ";
       //    StrSql = StrSql + " ,G_Cur_PV_4_2 =  G4_Cur_PV_1_2 ";
       //    StrSql = StrSql + " Where GradeDate3 <> ''  ";
       //    StrSql = StrSql + " And  GradeDate3 <= G4_Date_1 ";


       //    //누적받는 당시에 내가 판매원이어야지만 받을수 있다. 201612-21 박해진 대리 요청에 의해선
       //    StrSql += " And  Mbid +'-' + Convert(Varchar,Mbid2 )  " ;
       //    StrSql += " in (Select Mbid +'-' + Convert(Varchar,Mbid2 ) From tbl_ClosePay_02_Mod (nolock)  Where ToEndDate ='" + dic_ToEndDate[3] + "' And Sell_Mem_TF = 0 ) ";
       //    Temp_Connect.Insert_Data(StrSql, Conn, tran);


       //    //4주차 전보다 더 전에 BP가 되엇으면 다 누적 받는다.
       //    StrSql = "Update tbl_ClosePay_02 Set ";
       //    StrSql = StrSql + "  G_Cur_PV_4_1 = G_Cur_PV_4_1 +  G4_Cur_PV_2_1 ";
       //    StrSql = StrSql + " ,G_Cur_PV_4_2 = G_Cur_PV_4_2 +  G4_Cur_PV_2_2 ";
       //    StrSql = StrSql + " Where GradeDate3 <> ''  ";
       //    StrSql = StrSql + " And  GradeDate3 <= G4_Date_2 ";

       //    //누적받는 당시에 내가 판매원이어야지만 받을수 있다. 201612-21 박해진 대리 요청에 의해선
       //    StrSql += " And  Mbid +'-' + Convert(Varchar,Mbid2 )  ";
       //    StrSql += " in (Select Mbid +'-' + Convert(Varchar,Mbid2 ) From tbl_ClosePay_02_Mod (nolock)  Where ToEndDate ='" + dic_ToEndDate[2] + "' And Sell_Mem_TF = 0 ) ";
         
       //    Temp_Connect.Insert_Data(StrSql, Conn, tran);


       //    //4주차 전보다 더 전에 BP가 되엇으면 다 누적 받는다.
       //    StrSql = "Update tbl_ClosePay_02 Set ";
       //    StrSql = StrSql + "  G_Cur_PV_4_1 = G_Cur_PV_4_1 +  G4_Cur_PV_3_1 ";
       //    StrSql = StrSql + " ,G_Cur_PV_4_2 = G_Cur_PV_4_2 +  G4_Cur_PV_3_2 ";
       //    StrSql = StrSql + " Where GradeDate3 <> ''  ";
       //    StrSql = StrSql + " And  GradeDate3 <= G4_Date_3 ";

       //    //누적받는 당시에 내가 판매원이어야지만 받을수 있다. 201612-21 박해진 대리 요청에 의해선
       //    StrSql += " And  Mbid +'-' + Convert(Varchar,Mbid2 )  ";
       //    StrSql += " in (Select Mbid +'-' + Convert(Varchar,Mbid2 ) From tbl_ClosePay_02_Mod (nolock)  Where ToEndDate ='" + dic_ToEndDate[1] + "' And Sell_Mem_TF = 0 ) ";
           
       //    Temp_Connect.Insert_Data(StrSql, Conn, tran);


       //    //4주차 전보다 더 전에 BP가 되엇으면 다 누적 받는다.
       //    StrSql = "Update tbl_ClosePay_02 Set ";
       //    StrSql = StrSql + "  G_Cur_PV_4_1 = G_Cur_PV_4_1 +  G4_Cur_PV_4_1 ";
       //    StrSql = StrSql + " ,G_Cur_PV_4_2 = G_Cur_PV_4_2 +  G4_Cur_PV_4_2 ";
       //    StrSql = StrSql + " Where GradeDate3 <> ''  ";
       //    StrSql = StrSql + " And  GradeDate3 <= G4_Date_4 ";
       //    //누적받는 당시에 내가 판매원이어야지만 받을수 있다. 201612-21 박해진 대리 요청에 의해선
       //    StrSql += " And Sell_Mem_TF = 0  ";
          
       //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
       //    ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
       //    ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



       //    ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
       //    ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

       //    //4주차 전보다 더 전에 BP가 되엇으면 다 누적 받는다.
       //    StrSql = "Update tbl_ClosePay_02 Set ";
       //    StrSql = StrSql + " G_Cur_PV_5_1 =  G4_Cur_PV_0_1 ";
       //    StrSql = StrSql + " ,G_Cur_PV_5_2 =  G4_Cur_PV_0_2 ";
       //    StrSql = StrSql + " Where GradeDate3 <> ''  ";
       //    StrSql = StrSql + " And  GradeDate3 <= G4_Date_0 ";
       //    StrSql = StrSql + " And  LEFT(G4_Date_0,6) = '" + ToEndDate.Substring (0,6) + "'";

       //    //누적받는 당시에 내가 판매원이어야지만 받을수 있다. 201612-21 박해진 대리 요청에 의해선
       //    StrSql += " And  Mbid +'-' + Convert(Varchar,Mbid2 )  ";
       //    StrSql += " in (Select Mbid +'-' + Convert(Varchar,Mbid2 ) From tbl_ClosePay_02_Mod (nolock)  Where ToEndDate ='" + dic_ToEndDate[4] + "' And Sell_Mem_TF = 0 ) ";
           
       //    Temp_Connect.Insert_Data(StrSql, Conn, tran);


       //    StrSql = "Update tbl_ClosePay_02 Set ";
       //    StrSql = StrSql + " G_Cur_PV_5_1 = G_Cur_PV_5_1 +   G4_Cur_PV_1_1 ";
       //    StrSql = StrSql + " ,G_Cur_PV_5_2 = G_Cur_PV_5_2 +  G4_Cur_PV_1_2 ";
       //    StrSql = StrSql + " Where GradeDate3 <> ''  ";
       //    StrSql = StrSql + " And  GradeDate3 <= G4_Date_1 ";
       //    StrSql = StrSql + " And  LEFT(G4_Date_1,6) = '" + ToEndDate.Substring(0, 6) + "'";

       //    //누적받는 당시에 내가 판매원이어야지만 받을수 있다. 201612-21 박해진 대리 요청에 의해선
       //    StrSql += " And  Mbid +'-' + Convert(Varchar,Mbid2 )  ";
       //    StrSql += " in (Select Mbid +'-' + Convert(Varchar,Mbid2 ) From tbl_ClosePay_02_Mod (nolock)  Where ToEndDate ='" + dic_ToEndDate[3] + "' And Sell_Mem_TF = 0 ) ";
         
       //    Temp_Connect.Insert_Data(StrSql, Conn, tran);


       //    //4주차 전보다 더 전에 BP가 되엇으면 다 누적 받는다.
       //    StrSql = "Update tbl_ClosePay_02 Set ";
       //    StrSql = StrSql + "  G_Cur_PV_5_1 = G_Cur_PV_5_1 +  G4_Cur_PV_2_1 ";
       //    StrSql = StrSql + " ,G_Cur_PV_5_2 = G_Cur_PV_5_2 +  G4_Cur_PV_2_2 ";
       //    StrSql = StrSql + " Where GradeDate3 <> ''  ";
       //    StrSql = StrSql + " And  GradeDate3 <= G4_Date_2 ";
       //    StrSql = StrSql + " And  LEFT(G4_Date_2,6) = '" + ToEndDate.Substring(0, 6) + "'";

       //    //누적받는 당시에 내가 판매원이어야지만 받을수 있다. 201612-21 박해진 대리 요청에 의해선
       //    StrSql += " And  Mbid +'-' + Convert(Varchar,Mbid2 )  ";
       //    StrSql += " in (Select Mbid +'-' + Convert(Varchar,Mbid2 ) From tbl_ClosePay_02_Mod (nolock)  Where ToEndDate ='" + dic_ToEndDate[2] + "' And Sell_Mem_TF = 0 ) ";
           
       //    Temp_Connect.Insert_Data(StrSql, Conn, tran);


       //    //4주차 전보다 더 전에 BP가 되엇으면 다 누적 받는다.
       //    StrSql = "Update tbl_ClosePay_02 Set ";
       //    StrSql = StrSql + "  G_Cur_PV_5_1 = G_Cur_PV_5_1 +  G4_Cur_PV_3_1 ";
       //    StrSql = StrSql + " ,G_Cur_PV_5_2 = G_Cur_PV_5_2 +  G4_Cur_PV_3_2 ";
       //    StrSql = StrSql + " Where GradeDate3 <> ''  ";
       //    StrSql = StrSql + " And  GradeDate3 <= G4_Date_3 ";
       //    StrSql = StrSql + " And  LEFT(G4_Date_3,6) = '" + ToEndDate.Substring(0, 6) + "'";

       //    //누적받는 당시에 내가 판매원이어야지만 받을수 있다. 201612-21 박해진 대리 요청에 의해선
       //    StrSql += " And  Mbid +'-' + Convert(Varchar,Mbid2 )  ";
       //    StrSql += " in (Select Mbid +'-' + Convert(Varchar,Mbid2 ) From tbl_ClosePay_02_Mod (nolock)  Where ToEndDate ='" + dic_ToEndDate[1] + "' And Sell_Mem_TF = 0 ) ";
           
       //    Temp_Connect.Insert_Data(StrSql, Conn, tran);


       //    //4주차 전보다 더 전에 BP가 되엇으면 다 누적 받는다.
       //    StrSql = "Update tbl_ClosePay_02 Set ";
       //    StrSql = StrSql + "  G_Cur_PV_5_1 = G_Cur_PV_5_1 +  G4_Cur_PV_4_1 ";
       //    StrSql = StrSql + " ,G_Cur_PV_5_2 = G_Cur_PV_5_2 +  G4_Cur_PV_4_2 ";
       //    StrSql = StrSql + " Where GradeDate3 <> ''  ";
       //    StrSql = StrSql + " And  GradeDate3 <= G4_Date_4 ";
       //    StrSql = StrSql + " And  LEFT(G4_Date_4,6) = '" + ToEndDate.Substring(0, 6) + "'";

       //    //누적받는 당시에 내가 판매원이어야지만 받을수 있다. 201612-21 박해진 대리 요청에 의해선
       //    StrSql += " And  Sell_Mem_TF = 0  ";
           
       //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
       //    ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
       //    ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


       //}



       private void Put_Down_SumPV_5(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
       {
           string StrSql = "";

           string str_GradeCnt = "", str_GradeCnt1 = "", str_GradeCnt2 = "", str_GradeCnt3 = "";

           
           //이번달의 반품과 실매출을 합산하거를 뽑아온다.....
           StrSql = " Update tbl_ClosePay_02 SET";
           StrSql = StrSql + " Day_Sum_PV_5 = IsNull(b.A1, 0)";
           StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
           StrSql = StrSql + " (";
           StrSql = StrSql + " Select  Sum(TotalPV) AS A1 ";
           StrSql = StrSql + " ,Mbid,Mbid2 ";
           StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
           StrSql = StrSql + " Where   LEFT(SellDate_2,6) = '" + ToEndDate.Substring (0,6) + "'";           
           StrSql = StrSql + " And   Ga_Order = 0 ";
           StrSql = StrSql + " And   SellCode = '01' ";
           StrSql = StrSql + " Group By Mbid,Mbid2";
           StrSql = StrSql + " ) B";
           StrSql = StrSql + " Where a.Mbid = b.Mbid ";
           StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

           Temp_Connect.Insert_Data(StrSql, Conn, tran);

           //2016-11-07 박해진 대리 요청에 의해서 소매매출은 상위로 잡을때 PV는  50%만 잡아줘라.
           StrSql = " Update tbl_ClosePay_02 SET";
           StrSql = StrSql + " Day_Sum_PV_5 = Day_Sum_PV_5 + (IsNull(b.A1, 0) * 0.5)";
           StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
           StrSql = StrSql + " (";
           StrSql = StrSql + " Select  Sum(TotalPV) AS A1 ";
           StrSql = StrSql + " ,Mbid,Mbid2 ";
           StrSql = StrSql + " From tbl_SalesDetail (nolock)  ";
           StrSql = StrSql + " Where   LEFT(SellDate_2,6) = '" + ToEndDate.Substring(0, 6) + "'";
           StrSql = StrSql + " And   Ga_Order = 0 ";
           StrSql = StrSql + " And   SellCode = '02' ";
           StrSql = StrSql + " Group By Mbid,Mbid2";
           StrSql = StrSql + " ) B";
           StrSql = StrSql + " Where a.Mbid = b.Mbid ";
           StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

           Temp_Connect.Insert_Data(StrSql, Conn, tran);


           StrSql = " Update tbl_ClosePay_02 SET";
           StrSql = StrSql + " D_PV_5 = IsNull(b.A1, 0)";
           StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";
           StrSql = StrSql + " (";
           StrSql = StrSql + " Select  Sum(Day_Sum_PV_5) A1 ";
           StrSql = StrSql + " ,Nominid,Nominid2 ";
           StrSql = StrSql + " From tbl_ClosePay_02 (nolock)  ";
           StrSql = StrSql + " Where   Sell_Mem_TF = 1 ";
           StrSql = StrSql + " Group by Nominid , Nominid2 ";
           StrSql = StrSql + " ) B";
           StrSql = StrSql + " Where a.Mbid = b.Nominid ";
           StrSql = StrSql + " And   a.Mbid2 = b.Nominid2 ";

           Temp_Connect.Insert_Data(StrSql, Conn, tran);


           pg1.Value = 0; pg1.Maximum = MaxLevel + 2;
           pg1.PerformStep(); pg1.Refresh();

           int Cnt = MaxLevel;

           while (Cnt >= 0)
           {

               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " G_Cur_PV_5_1 =  ISNULL(B.A2,0)";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + "Select    Sum(G_Cur_PV_5_1 + G_Cur_PV_5_2 + Day_Sum_PV_5) A2 ";
               StrSql = StrSql + " ,Saveid,Saveid2 ";
               StrSql = StrSql + " From tbl_ClosePay_02 ";
               StrSql = StrSql + " Where (  G_Cur_PV_5_1 + G_Cur_PV_5_2 + Day_Sum_PV_5 ) <>0   ";
               StrSql = StrSql + " And   LevelCnt =" + Cnt;
               StrSql = StrSql + " And   LineCnt =  1 ";
               StrSql = StrSql + " Group By Saveid,Saveid2   ";
               StrSql = StrSql + " ) B";

               StrSql = StrSql + " Where A.Mbid=B.Saveid ";
               StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

               Temp_Connect.Insert_Data(StrSql, Conn, tran);


               StrSql = "Update tbl_ClosePay_02 SET ";
               StrSql = StrSql + " G_Cur_PV_5_2 =  ISNULL(B.A2,0)";
               StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

               StrSql = StrSql + " (";
               StrSql = StrSql + "Select    Sum(G_Cur_PV_5_1 + G_Cur_PV_5_2 + Day_Sum_PV_5  ) A2 ";
               StrSql = StrSql + " ,Saveid,Saveid2 ";
               StrSql = StrSql + " From tbl_ClosePay_02 ";
               StrSql = StrSql + " Where (  G_Cur_PV_5_1 + G_Cur_PV_5_2 + Day_Sum_PV_5  ) <>0   ";
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
      

        private void Put_OrgGrade( cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran )
       {
            pg1.Value = 0; pg1.Maximum = 14  ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
            string SDate = "";

            StrSql = "Select Isnull(Max(ToEndDate), '')  From tbl_CloseTotal_04 (nolock) ";   //'''--직급마감에서 전달 마감일자를 알아온다.
            StrSql = StrSql  + " Where LEFT(ToEndDate,6) < '" + ToEndDate.Substring(0,6) + "'"  ;    // '''--전달마감을 알아온다.

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
            
    
            //if (SDate == "") return ;

            if (FromEndDate.Substring(0, 6) == "201409" )
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "  CurGrade = ISNULL(B.A1,0) ";
                StrSql = StrSql + " , ReqTF2 = 1  ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                StrSql = StrSql + " (Select  Apply_Grade As A1 ,  Mbid,Mbid2 ";
                StrSql = StrSql + " From tbl_Sham_Grade  (nolock) ";
                StrSql = StrSql + " Where LEFT(Apply_Date,6) = '201408'";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }
            else
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "  CurGrade =ISNULL(B.A1,0) ";
                StrSql = StrSql + " , ReqTF2 =ISNULL(B.ReqTF2,0) ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                StrSql = StrSql + " (Select  CurGrade As A1 , ReqTF2 , Mbid,Mbid2 ";
                StrSql = StrSql + " From tbl_ClosePay_04_Mod  (nolock) ";
                StrSql = StrSql + " Where ToEndDate = '" + SDate + "'";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  ReqTF2 = 1 ";
            StrSql = StrSql + " Where  ReqTF2 = 0 ";
            StrSql = StrSql + " And  (LEFT (RegTime,6) ='" + ToEndDate.Substring (0, 6) + "'" ;
            StrSql = StrSql + " OR   LEFT (RegTime,6) ='" + FromEndDate.Substring(0, 6) + "')";
    

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  D_CurGrade = 10 ";
            StrSql = StrSql + " Where  SellPV01 + SellPV02 + SellPV03 >= 180 ";
                        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  D_CurGrade = 20 ";
            StrSql = StrSql + " Where  SellPV01 + SellPV02 + SellPV03 >= 600 ";
            StrSql = StrSql + " And   Datediff(month,RegTime,'" + ToEndDate + "') <= 1 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  D_CurGrade = 30 ";
            StrSql = StrSql + " Where  SellPV01 + SellPV02 + SellPV03 >= 1200 ";
            StrSql = StrSql + " And   Datediff(month,RegTime,'" + ToEndDate + "') <= 2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  D_CurGrade = 40 ";
            StrSql = StrSql + " Where  SellPV01 + SellPV02 + SellPV03 >= 3000 ";
            StrSql = StrSql + " And   Datediff(month,RegTime,'" + ToEndDate + "') <= 3 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Pay_CurGrade = D_CurGrade ";
            StrSql = StrSql + " Where  D_CurGrade >= CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Pay_CurGrade = CurGrade ";
            StrSql = StrSql + " Where  D_CurGrade < CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();





            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  DS_CurGrade =ISNULL(B.A1,0) ";            
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (Select  Apply_Grade As A1  , Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_Sham_Grade  (nolock) ";
            StrSql = StrSql + " Where seq IN (Select Max( seq) From tbl_Sham_Grade  (nolock) Where LEFT( Apply_Date,6) ='" + ToEndDate.Substring (0,6)  + "' Group By Mbid,Mbid2 )" ;
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //인정직급이 더 큰 경우. 이번달에 인정직급을 준거기 때문에.. 그 직급으로 해서. 유지가 된거로 본다.
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Pay_CurGrade = DS_CurGrade ";
            StrSql = StrSql + "  ,ReqTF2 = 1 ";
            //StrSql = StrSql + " Where  Pay                                                                                                                                                                                                                  
            StrSql = StrSql + " Where   DS_CurGrade > 0 "; 

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            if (FromEndDate.Substring(0, 6) == "201412")
            {
                StrSql = "Update tbl_ClosePay_02 SET ";                
                StrSql = StrSql + "  ReqTF2 = 1 ";                
                StrSql = StrSql + " Where   Mbid = 'KR' And Mbid2 = 457 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "  ReqTF2 = 1 ";
                StrSql = StrSql + " Where   Mbid = 'KR' And Mbid2 = 555 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (FromEndDate.Substring(0, 6) == "201501" || FromEndDate.Substring(0, 6) == "201502")
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "  ReqTF2 = 1 ";
                StrSql = StrSql + " Where   Mbid = 'KR' And Mbid2 IN ( 1403,1346 , 1931 , 1881 , 1912 ) ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }


            StrSql = "Update tbl_ClosePay_02 SET ";            
            StrSql = StrSql + "  ReqTF2 = 1  ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (Select   Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_Sham_Sell_TF  (nolock) ";
            StrSql = StrSql + " Where LEFT(Apply_Date,6) = '" + FromEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);



       }



        private void CurGrade_OrgGrade_Put(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();

            string StrSql = "Update tbl_ClosePay_02 set";
            StrSql = StrSql + " OrgGrade  = BeforeGrade";
            StrSql = StrSql + " ,CurGrade = BeforeGrade";
            StrSql = StrSql + " Where  BeforeGrade > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
              

            //소비자일경우에 역대 최고직급은 다사라지록 처음으로 돌아간다. 2016-12-02
            StrSql = "Update tbl_ClosePay_02 set";
            StrSql = StrSql + " CurGrade  = 0 ";
            StrSql = StrSql + " Where Sell_Mem_TF > 0  ";
            StrSql = StrSql + " And CurGrade >= 10 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
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

                    //if (S_Grade == 0)
                    //{
                    //    StrSql = "Update tbl_ClosePay_02 SET ";
                    //    StrSql = StrSql + " CurGrade =  0  ";
                    //    StrSql = StrSql + " ,OrgGrade =    " + S_Grade;
                    //    StrSql = StrSql + " ,ShamGrade =    " + S_Grade;
                    //    StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
                    //    StrSql = StrSql + " And   Mbid2 = " + Mbid2;
                    //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    //}
                    //else
                    //{
                    StrSql = "Update tbl_ClosePay_02 SET ";
                    StrSql = StrSql + " CurGrade =    " + S_Grade;
                    //StrSql = StrSql + " ,OrgGrade =    " + S_Grade;
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

                        StrSql = "Update tbl_ClosePay_02 Set ";
                        StrSql = StrSql + TFild + " = '" + TMaxDate + "'";
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





        private void GiveGrade1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int  S_LevelCnt)
        {
            pg1.Value = 0; pg1.Maximum = 3;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade = 10 ";
            StrSql = StrSql + " ,UP_Grade_TF = 10 ";
            StrSql = StrSql + " Where   OneGrade < 10 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 500 ";
            //StrSql = StrSql + " And   Down_G_Down - Max_Down_G_Down >= 1000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 500 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 500 ";
            StrSql = StrSql + " And   GradeDate1 = '' ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt; 
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade = 10 ";
            StrSql = StrSql + " ,UP_Grade_TF = 10 ";
            StrSql = StrSql + " Where   OneGrade < 10 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 500 ";
            //StrSql = StrSql + " And   Down_W4_QV_Real - Max_Down_W4_QV_Real >= 1000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 1000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 1000 ";
            StrSql = StrSql + " And   GradeDate1 <> '' ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt; 
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade =10 ";
            StrSql = StrSql + " And GradeDate1 =''";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }


        private void GiveGrade2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
        {
            pg1.Value = 0; pg1.Maximum = 6;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
           
            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade = 20 ";
            StrSql = StrSql + " ,UP_Grade_TF = 20 ";
            StrSql = StrSql + " Where   OneGrade < 20 ";

            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 500 ";
            //StrSql = StrSql + " And   Down_W4_QV_Real - Max_Down_W4_QV_Real >= 2000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 2000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 2000 ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt; 

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 20 ";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }


        private void GiveGrade3(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
        {
            pg1.Value = 0; pg1.Maximum = 9;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 Set ";            
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            //per
            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade= 30 ";
            StrSql = StrSql + " ,UP_Grade_TF = 30 ";
            StrSql = StrSql + " Where   OneGrade < 30 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            //StrSql = StrSql + " And   Down_W4_QV_Real - Max_Down_W4_QV_Real >= 4000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 4000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 4000 ";
            StrSql = StrSql + " And   Max_GradeCnt1 >= 1  ";
            StrSql = StrSql + " And   GradeCnt1 >= 2  "; 
            StrSql = StrSql + " And   GradeCnt1 - Max_GradeCnt1 >= 1  ";
            if (S_LevelCnt >= 0 ) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt; 
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

       
            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 30 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 30";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 30 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }


        private void GiveGrade4(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
        {
            pg1.Value = 0; pg1.Maximum = 10;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade= 40 ";
            StrSql = StrSql + " ,UP_Grade_TF = 40 ";
            StrSql = StrSql + " Where OneGrade < 40 ";            
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            //StrSql = StrSql + " And   Down_W4_QV_Real - Max_Down_W4_QV_Real >= 8000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 8000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 8000 ";
            StrSql = StrSql + " And   Max_GradeCnt2 >= 1  ";
            StrSql = StrSql + " And   GradeCnt2 >= 2  "; 
            StrSql = StrSql + " And   GradeCnt2 - Max_GradeCnt2 >= 1  ";
            if (S_LevelCnt >= 0)  StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt; 
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 40 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 40 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 40";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 40 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }





        private void GiveGrade5(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
        {
            pg1.Value = 0; pg1.Maximum = 12;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade= 50 ";
            StrSql = StrSql + " ,UP_Grade_TF = 50 ";
            StrSql = StrSql + " Where OneGrade < 50 ";                        
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            //StrSql = StrSql + " And   Down_W4_QV_Real - Max_Down_W4_QV_Real >= 15000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 15000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 15000 ";
            StrSql = StrSql + " And   Max_GradeCnt3 >= 1  ";
            StrSql = StrSql + " And   GradeCnt3 >= 2  "; 
            StrSql = StrSql + " And   GradeCnt3 - Max_GradeCnt3 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt; 

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void GiveGrade6(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
        {
            pg1.Value = 0; pg1.Maximum = 11;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade= 60 ";
            StrSql = StrSql + " ,UP_Grade_TF = 60 ";
            StrSql = StrSql + " Where OneGrade < 60 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            //StrSql = StrSql + " And   Down_W4_QV_Real - Max_Down_W4_QV_Real >= 30000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 30000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 30000 ";
            StrSql = StrSql + " And   Max_GradeCnt4 >= 1  ";
            StrSql = StrSql + " And   GradeCnt4 >= 2  ";
            StrSql = StrSql + " And   GradeCnt4 - Max_GradeCnt4 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt; 

            Temp_Connect.Insert_Data(StrSql, Conn, tran);                 
            pg1.PerformStep(); pg1.Refresh();

            
            





            //// 반품 관련 4주 부분 때문에.. 우선은 이렇게 처리해 놓은 이부분은 보완을 해야 함 ㅠㅠ
            //StrSql = "Update tbl_ClosePay_02 Set ";
            //StrSql = StrSql + " CurGrade= 60 ";
            //StrSql = StrSql + " Where CurGrade < 60 ";
            //StrSql = StrSql + " And   OrgGrade = 60  ";
            //StrSql = StrSql + " And   LeaveDate = ''";
            //StrSql = StrSql + " And   Sell_MEM_TF = 0 ";


            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);




            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void GiveGrade7(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
        {
            pg1.Value = 0; pg1.Maximum = 11;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade= 70 ";
            StrSql = StrSql + " ,UP_Grade_TF = 70 ";
            StrSql = StrSql + " Where OneGrade < 70 "; ;            
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            //StrSql = StrSql + " And   Down_W4_QV_Real - Max_Down_W4_QV_Real >= 60000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 60000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 60000 ";
            StrSql = StrSql + " And   Max_GradeCnt5 >= 1  ";
            StrSql = StrSql + " And   GradeCnt5 >= 2  ";
            StrSql = StrSql + " And   GradeCnt5 - Max_GradeCnt5 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt; 
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //    StrSql = "Update tbl_ClosePay_02 Set " ;
            //StrSql = StrSql + " CurGrade= 70 " ;
            //StrSql = StrSql + " Where CurGrade < 70 " ;

            //StrSql = StrSql + " And   LeaveDate = ''" ;
            //StrSql = StrSql + " And   Sell_MEM_TF = 0 " ;


            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            ////    StrSql = "Update tbl_ClosePay_02 Set " ;
            ////StrSql = StrSql + " CurGrade = 70 " ;
            ////StrSql = StrSql + " Where BeforeGrade = 70 " ;
            ////StrSql = StrSql + " And   CurGrade < 70 " ;
            ////StrSql = StrSql + " And   G_Sum_PV_1 >= 30000 " ;
            ////StrSql = StrSql + " And   G_Sum_PV_2 >= 30000 " ;
            ////StrSql = StrSql + " And   LeaveDate = ''" ;
            ////StrSql = StrSql + " And   Sell_MEM_TF = 0 ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }

        private void GiveGrade8(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
        {
            pg1.Value = 0; pg1.Maximum = 12;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade = 80 ";
            StrSql = StrSql + " ,UP_Grade_TF = 80 ";
            StrSql = StrSql + " Where OneGrade < 80 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            //StrSql = StrSql + " And   Down_W4_QV_Real - Max_Down_W4_QV_Real >= 120000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 120000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 120000 ";
            StrSql = StrSql + " And   Max_GradeCnt6 >= 1  ";
            StrSql = StrSql + " And   GradeCnt6 >= 2  ";
            StrSql = StrSql + " And   GradeCnt6 - Max_GradeCnt6 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt; 
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);



            //StrSql = "Update tbl_ClosePay_02 Set ";
            //StrSql = StrSql + " CurGrade= 80 ";
            //StrSql = StrSql + " Where CurGrade < 80 ";

            //StrSql = StrSql + " And   LeaveDate = '' ";
            //StrSql = StrSql + " And   Sell_MEM_TF = 0 ";


            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            ////StrSql = "Update tbl_ClosePay_02 Set ";
            ////StrSql = StrSql + " CurGrade = 80 ";
            ////StrSql = StrSql + " Where BeforeGrade = 80 ";
            ////StrSql = StrSql + " And   CurGrade < 80 ";
            ////StrSql = StrSql + " And   G_Sum_PV_1 >= 90000 ";
            ////StrSql = StrSql + " And   G_Sum_PV_2 >= 90000 ";
            ////StrSql = StrSql + " And   LeaveDate = ''";
            ////StrSql = StrSql + " And   Sell_MEM_TF = 0 ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate8 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }

        private void GiveGrade9(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
        {
            pg1.Value = 0; pg1.Maximum = 13;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade= 90 ";
            StrSql = StrSql + " ,UP_Grade_TF = 90 ";
            StrSql = StrSql + " Where OneGrade < 90 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            //StrSql = StrSql + " And   Down_W4_QV_Real - Max_Down_W4_QV_Real >= 250000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 250000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 250000 ";
            StrSql = StrSql + " And   Max_GradeCnt7 >= 1  ";
            StrSql = StrSql + " And   GradeCnt7 >= 2  ";
            StrSql = StrSql + " And   GradeCnt7 - Max_GradeCnt7 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt; 

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate9 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate8 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }


        private void GiveGrade10(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
        {
            pg1.Value = 0; pg1.Maximum = 14;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade= 100 ";
            StrSql = StrSql + " ,UP_Grade_TF = 100 ";
            StrSql = StrSql + " Where OneGrade < 100 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            //StrSql = StrSql + " And   Down_W4_QV_Real - Max_Down_W4_QV_Real >= 500000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 500000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 500000 ";
            StrSql = StrSql + " And   Max_GradeCnt8 >= 1  ";
            StrSql = StrSql + " And   GradeCnt8 >= 2  ";
            StrSql = StrSql + " And   GradeCnt8 - Max_GradeCnt8 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt; 
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate10 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate10 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate9 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate8 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void GiveGrade11(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
        {
            pg1.Value = 0; pg1.Maximum = 14;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade= 110 ";
            StrSql = StrSql + " ,UP_Grade_TF = 110 ";
            StrSql = StrSql + " Where OneGrade < 110 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            //StrSql = StrSql + " And   Down_W4_QV_Real - Max_Down_W4_QV_Real >= 1000000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 1000000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 1000000 ";
            StrSql = StrSql + " And   Max_GradeCnt9 >= 1  ";
            StrSql = StrSql + " And   GradeCnt9 >= 2  ";
            StrSql = StrSql + " And   GradeCnt9 - Max_GradeCnt9 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt; 

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate11 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate11 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate10 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate10 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate9 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate8 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }




        private void GiveGrade12(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt)
        {
            pg1.Value = 0; pg1.Maximum = 14;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade= 120 ";
            StrSql = StrSql + " ,UP_Grade_TF = 120 ";
            StrSql = StrSql + " Where OneGrade < 120 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            //StrSql = StrSql + " And   Down_W4_QV_Real - Max_Down_W4_QV_Real >= 2000000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 2000000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 2000000 ";
            StrSql = StrSql + " And   Max_GradeCnt10 >= 1  ";
            StrSql = StrSql + " And   GradeCnt10 >= 2  ";
            StrSql = StrSql + " And   GradeCnt10 - Max_GradeCnt10 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt; 

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate12 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate12 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate11 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate11 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate10 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate10 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate9 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate8 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private Boolean Check_UP_Grade_TF(int CurrentGrade, cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string StrSql = ""; 

            StrSql = " Select Isnull(Count(Mbid),0)    ";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where   UP_Grade_TF = " + CurrentGrade; 
            
            DataSet ds_T = new DataSet();

            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds_T);
            ReCnt = Search_Connect.DataSet_ReCount;

            int up_Cnt = 0; 
            if (ReCnt <= 0)
                return false;
            else
            {
                up_Cnt = int.Parse (ds_T.Tables[base_db_name].Rows[0][0].ToString());
            }

            if (up_Cnt >0)
                return true; 
            else
                return false ; 
        }


        private void GradeUpLine__3(int CurrentGrade, cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt )
        {
            int Cnt = 0;
            string StrSql = "", str_GradeCnt = "", str_GradeCnt1 = "", str_GradeCnt2 = "", str_GradeCnt3 = "";

            if (CurrentGrade == 10) str_GradeCnt = " GradeCnt1 ";
            if (CurrentGrade == 20) str_GradeCnt = " GradeCnt2 ";
            if (CurrentGrade == 30) str_GradeCnt = " GradeCnt3 ";
            if (CurrentGrade == 40) str_GradeCnt = " GradeCnt4 ";
            if (CurrentGrade == 50) str_GradeCnt = " GradeCnt5 ";
            if (CurrentGrade == 60) str_GradeCnt = " GradeCnt6 ";
            if (CurrentGrade == 70) str_GradeCnt = " GradeCnt7 ";
            if (CurrentGrade == 80) str_GradeCnt = " GradeCnt8 ";
            if (CurrentGrade == 90) str_GradeCnt = " GradeCnt9 ";
            if (CurrentGrade == 100) str_GradeCnt = " GradeCnt10 ";
            if (CurrentGrade == 110) str_GradeCnt = " GradeCnt11 ";
            if (CurrentGrade == 120) str_GradeCnt = " GradeCnt12 ";


            string Max_str_GradeCnt = "";
            if (CurrentGrade == 10) Max_str_GradeCnt = " Max_GradeCnt1 ";
            if (CurrentGrade == 20) Max_str_GradeCnt = " Max_GradeCnt2 ";
            if (CurrentGrade == 30) Max_str_GradeCnt = " Max_GradeCnt3 ";
            if (CurrentGrade == 40) Max_str_GradeCnt = " Max_GradeCnt4 ";
            if (CurrentGrade == 50) Max_str_GradeCnt = " Max_GradeCnt5 ";
            if (CurrentGrade == 60) Max_str_GradeCnt = " Max_GradeCnt6 ";
            if (CurrentGrade == 70) Max_str_GradeCnt = " Max_GradeCnt7 ";
            if (CurrentGrade == 80) Max_str_GradeCnt = " Max_GradeCnt8 ";
            if (CurrentGrade == 90) Max_str_GradeCnt = " Max_GradeCnt9 ";
            if (CurrentGrade == 100) Max_str_GradeCnt = " Max_GradeCnt10 ";
            if (CurrentGrade == 110) Max_str_GradeCnt = " Max_GradeCnt11 ";
            if (CurrentGrade == 120) Max_str_GradeCnt = " Max_GradeCnt12 ";




          



            if (S_LevelCnt >= 0)
            {
                pg1.Value = 0; pg1.Maximum = Cnt + 4;
                pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + str_GradeCnt + "=ISNULL(B.A1,0) ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                StrSql = StrSql + " (Select    Sum(" + str_GradeCnt + ") A1,Nominid,Nominid2 ";
                StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
                StrSql = StrSql + " Where " + str_GradeCnt + " > 0  ";
                StrSql = StrSql + " And N_LevelCnt =" + S_LevelCnt;
                StrSql = StrSql + " Group By Nominid,Nominid2 ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Nominid ";
                StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + str_GradeCnt + " =" + str_GradeCnt + " + ISNULL(B.A1,0)  ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                StrSql = StrSql + " (Select Count(Mbid) A1,   Nominid,Nominid2 ";
                StrSql = StrSql + " From tbl_ClosePay_02 ";
                StrSql = StrSql + " Where OneGrade >= " + CurrentGrade;
                //else
                //    StrSql = StrSql + " Where OneGrade = " + CurrentGrade;

                StrSql = StrSql + " And N_LevelCnt =" + S_LevelCnt;
                StrSql = StrSql + " Group By Nominid,Nominid2  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Nominid ";
                StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
                //'''---------------------------------------------------------------
            }
            else
            {

                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + " " + str_GradeCnt + " = 0";
                StrSql = StrSql + ", " + Max_str_GradeCnt + " = 0";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);




                int T_N_MaxLevel = 0;
                StrSql = " Select Isnull(Max(N_LevelCnt),0)    ";
                StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
                StrSql = StrSql + " Where   OneGrade = " + CurrentGrade;

                DataSet ds_T = new DataSet();

                int ReCnt = 0;
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds_T);
                ReCnt = Search_Connect.DataSet_ReCount;
                                
                if (ReCnt > 0)
                {                    
                    T_N_MaxLevel = int.Parse(ds_T.Tables[base_db_name].Rows[0][0].ToString()) ;
                }


                if (T_N_MaxLevel > 0)
                {
                    Cnt = T_N_MaxLevel + 1;

                    pg1.Value = 0; pg1.Maximum = Cnt + 2;
                    pg1.PerformStep(); pg1.Refresh();

                    while (Cnt >= 1)
                    {
                        StrSql = "Update tbl_ClosePay_02 SET ";
                        StrSql = StrSql + str_GradeCnt + "=ISNULL(B.A1,0) ";
                        StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                        StrSql = StrSql + " (Select    Sum(" + str_GradeCnt + ") A1,Nominid,Nominid2 ";
                        StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
                        StrSql = StrSql + " Where " + str_GradeCnt + " > 0  ";
                        StrSql = StrSql + " And N_LevelCnt =" + Cnt;
                        StrSql = StrSql + " Group By Nominid,Nominid2 ";
                        StrSql = StrSql + " ) B";

                        StrSql = StrSql + " Where A.Mbid=B.Nominid ";
                        StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);


                        StrSql = "Update tbl_ClosePay_02 SET ";
                        StrSql = StrSql + str_GradeCnt + " =" + str_GradeCnt + " + ISNULL(B.A1,0)  ";
                        StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                        StrSql = StrSql + " (Select Count(Mbid) A1,   Nominid,Nominid2 ";
                        StrSql = StrSql + " From tbl_ClosePay_02  (nolock) ";
                        StrSql = StrSql + " Where OneGrade >= " + CurrentGrade;
                        StrSql = StrSql + " And N_LevelCnt =" + Cnt;
                        StrSql = StrSql + " Group By Nominid,Nominid2  ";
                        StrSql = StrSql + " ) B";

                        StrSql = StrSql + " Where A.Mbid=B.Nominid ";
                        StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);
                        pg1.PerformStep(); pg1.Refresh();
                        //'''---------------------------------------------------------------

                        Cnt = Cnt - 1;
                    }
                }
            }
            


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " " + Max_str_GradeCnt + " =  ISNULL(B.A1,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select Case When OneGrade >= " + CurrentGrade + " then " + str_GradeCnt + " + 1  ELSE  " + str_GradeCnt + "  END A1 ";
            StrSql = StrSql + " ,Nominid,Nominid2 , N_LineCnt  ";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where (" + str_GradeCnt + "  > 0 ";
            StrSql = StrSql + " OR  OneGrade >= " + CurrentGrade  + ")";
            if (S_LevelCnt >= 0)  StrSql = StrSql + " And N_LevelCnt =" + S_LevelCnt;  ////이게 변경 첨가된 부분임.
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
            StrSql = StrSql + " And   A.Max_N_LineCnt = B.N_LineCnt ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

        }


        private void GradeUpLine2_N_30(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate_2 = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();


            StrSql = " Select Nominid , Nominid2, Saveid , Saveid2 , Mbid , Mbid2 ,  LineCnt , M_Name   ";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            //StrSql = StrSql + " Where   OneGrade = 30  ";            
            StrSql = StrSql + " Where   G3_Cur_PV_1 + G3_Cur_PV_2 + SellPV01 + SellPV02 +SellPV03 + D_PV >= 600000 "  ;
            StrSql = StrSql + " And     G3_Cur_PV_1 + G3_Cur_PV_2 + SellPV01 + SellPV02 +SellPV03 + D_PV < 1200000 ";
            StrSql = StrSql + " Order by Mbid , Mbid2  ASC ";

            DataSet ds_T = new DataSet();

            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds_T);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";

                Mbid = ds_T.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds_T.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds_T.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                //R_TotalPV = double.Parse(ds_T.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) + double.Parse(ds_T.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                //OrderNumber = ds_T.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                //if (Mbid2.ToString() == "8290420")
                //    S_Mbid = Mbid + "-" + Mbid2.ToString();

                S_Mbid = Mbid + "-" + Mbid2.ToString();

                TSaveid = Clo_Mem[S_Mbid].Saveid;
                TSaveid2 = Clo_Mem[S_Mbid].Saveid2;
                TLine = Clo_Mem[S_Mbid].LineCnt;

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[Mbid + "-" + Mbid2.ToString()].Nominid == TSaveid && Clo_Mem[Mbid + "-" + Mbid2.ToString()].Nominid2 == TSaveid2)
                        {
                            Allowance1 = 1;
                            R_LevelCnt++;
                           
                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_02 SET ";

                                if (TLine == 1)
                                    StrSql = StrSql + " GradeCnt3_N_1 = GradeCnt3_N_1 + 1";
                                else
                                    StrSql = StrSql + " GradeCnt3_N_2 = GradeCnt3_N_2 + 1 "; 
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                //StrSql = "INSERT INTO tbl_Close_DownPV_ALL_02";
                                //StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                //StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                //StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                                //StrSql = StrSql + "Values(";
                                //StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                //StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                //StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                //StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                //StrSql = StrSql + ",'1' ,'" + OrderNumber + "')";

                                ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                //t_qu[t_qu_Cnt] = StrSql;
                                //t_qu_Cnt++;
                            }


                        }

                        TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;

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



        private void GradeUpLine2_N_40(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate_2 = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();


            StrSql = " Select Nominid , Nominid2, Saveid , Saveid2 , Mbid , Mbid2 ,  LineCnt , M_Name ";
            StrSql = StrSql + " From tbl_ClosePay_02  (nolock) ";
            //StrSql = StrSql + " Where   OneGrade = 40  ";
            StrSql = StrSql + " Where   G3_Cur_PV_1 + G3_Cur_PV_2 + SellPV01 + SellPV02 +SellPV03 + D_PV >= 1200000 ";
            StrSql = StrSql + " And     G3_Cur_PV_1 + G3_Cur_PV_2 + SellPV01 + SellPV02 +SellPV03 + D_PV < 2400000 ";
            StrSql = StrSql + " Order by Mbid , Mbid2  ASC ";

            DataSet ds_T = new DataSet();

            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds_T);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";

                Mbid = ds_T.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds_T.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds_T.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                //R_TotalPV = double.Parse(ds_T.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) + double.Parse(ds_T.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                //OrderNumber = ds_T.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                //if (Mbid2.ToString() == "8290420")
                //    S_Mbid = Mbid + "-" + Mbid2.ToString();

                S_Mbid = Mbid + "-" + Mbid2.ToString();

                TSaveid = Clo_Mem[S_Mbid].Saveid;
                TSaveid2 = Clo_Mem[S_Mbid].Saveid2;
                TLine = Clo_Mem[S_Mbid].LineCnt;

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[Mbid + "-" + Mbid2.ToString()].Nominid == TSaveid && Clo_Mem[Mbid + "-" + Mbid2.ToString()].Nominid2 == TSaveid2)
                        {
                            Allowance1 = 1;
                            R_LevelCnt++;

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_02 SET ";

                                if (TLine == 1)
                                    StrSql = StrSql + " GradeCnt4_N_1 = GradeCnt4_N_1 + 1";
                                else
                                    StrSql = StrSql + " GradeCnt4_N_2 = GradeCnt4_N_2 + 1 ";
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                //StrSql = "INSERT INTO tbl_Close_DownPV_ALL_02";
                                //StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                //StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                //StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                                //StrSql = StrSql + "Values(";
                                //StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                //StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                //StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                //StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                //StrSql = StrSql + ",'1' ,'" + OrderNumber + "')";

                                ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                //t_qu[t_qu_Cnt] = StrSql;
                                //t_qu_Cnt++;
                            }


                        }

                        TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;

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




        private void GradeUpLine2_N_50(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate_2 = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();


            StrSql = "Update tbl_ClosePay_02 SET GradeCnt5_N_1 = 0 , GradeCnt5_N_2 = 0  ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = " Select Nominid , Nominid2, Saveid , Saveid2 , Mbid , Mbid2 ,  LineCnt , M_Name  ";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            //StrSql = StrSql + " Where   OneGrade >= 50  ";
            StrSql = StrSql + " Where   G3_Cur_PV_1 + G3_Cur_PV_2 + SellPV01 + SellPV02 +SellPV03 + D_PV >= 2400000 ";            
            StrSql = StrSql + " Order by Mbid , Mbid2  ASC ";

            DataSet ds_T = new DataSet();

            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds_T);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                TotalPV = 0;
                LevelCnt = 0; TSaveid = "**";

                Mbid = ds_T.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds_T.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds_T.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                //R_TotalPV = double.Parse(ds_T.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) + double.Parse(ds_T.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                //OrderNumber = ds_T.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                //if (Mbid2.ToString() == "8290420")
                //    S_Mbid = Mbid + "-" + Mbid2.ToString();

                S_Mbid = Mbid + "-" + Mbid2.ToString();

                TSaveid = Clo_Mem[S_Mbid].Saveid;
                TSaveid2 = Clo_Mem[S_Mbid].Saveid2;
                TLine = Clo_Mem[S_Mbid].LineCnt;

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[Mbid + "-" + Mbid2.ToString()].Nominid == TSaveid && Clo_Mem[Mbid + "-" + Mbid2.ToString()].Nominid2 == TSaveid2)
                        {
                            Allowance1 = 1;
                            R_LevelCnt++;

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_02 SET ";

                                if (TLine == 1)
                                    StrSql = StrSql + " GradeCnt5_N_1 = GradeCnt5_N_1 + 1";
                                else
                                    StrSql = StrSql + " GradeCnt5_N_2 = GradeCnt5_N_2 + 1 ";
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                //StrSql = "INSERT INTO tbl_Close_DownPV_ALL_02";
                                //StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                //StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                //StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                                //StrSql = StrSql + "Values(";
                                //StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                //StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                //StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                //StrSql = StrSql + Allowance1 + " ," + R_TotalPV + "," + LevelCnt + " ," + TLine;
                                //StrSql = StrSql + ",'1' ,'" + OrderNumber + "')";

                                ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                //t_qu[t_qu_Cnt] = StrSql;
                                //t_qu_Cnt++;
                            }


                        }

                        TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;

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





        private void  Put_ReqTF2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 9;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            string SDate3 = "";

            DateTime dt = DateTime.Parse(ToEndDate.Substring(0, 4) + "-" + ToEndDate.Substring(4, 2) + "-" + ToEndDate.Substring(6, 2));
            SDate3 = dt.AddMonths(-1).ToShortDateString().Replace("-", "");
                        
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  ReqTF2 = 1 ";
            StrSql = StrSql + " Where  ReqTF2 = 0 ";
            StrSql = StrSql + " And ( LEFT (RegTime,6) ='" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " OR   LEFT (RegTime,6) ='" + SDate3.Substring(0, 6) + "')";

            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  ReqTF2 = 1 ";
            StrSql = StrSql + " Where  OrgGrade <= 20  ";            
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  ReqTF2 = 1 ";
            StrSql = StrSql + " Where  OrgGrade >= 30 And OrgGrade <= 50   ";    
            StrSql = StrSql + " And   DayPV01 + DayPV02 + DayPV03 + D_PV >= 10000 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  ReqTF2 = 1 ";
            StrSql = StrSql + " Where  OrgGrade >= 60 And OrgGrade <= 70   ";
            StrSql = StrSql + " And   DayPV01 + DayPV02 + DayPV03 + D_PV >= 20000 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  ReqTF2 = 1 ";
            StrSql = StrSql + " Where  OrgGrade >= 80 And OrgGrade <= 90   ";
            StrSql = StrSql + " And   DayPV01 + DayPV02 + DayPV03 + D_PV >= 30000 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  ReqTF2 = 1 ";
            StrSql = StrSql + " Where  OrgGrade = 100  "; 
            StrSql = StrSql + " And   DayPV01 + DayPV02 + DayPV03 + D_PV >= 40000 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  ReqTF2 = 1 ";
            StrSql = StrSql + " Where  OrgGrade >= 110  ";
            StrSql = StrSql + " And   DayPV01 + DayPV02 + DayPV03 + D_PV >= 50000 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //StrSql = StrSql + " Be_M_Grade= 0  ,Cur_M_PV = 0  , ";
        }



        private void Put_MonthGrade(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 16;
            pg1.PerformStep(); pg1.Refresh();
           

            string StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " MonthGrade = Be_MonthGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " MonthGrade = 60 ";
            StrSql = StrSql + " Where MonthGrade <  60 ";
            StrSql = StrSql + " And  LEFT (GradeDate6,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " MonthGrade = 70 ";
            StrSql = StrSql + " Where MonthGrade <  70";
            StrSql = StrSql + " And  LEFT (GradeDate7,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " MonthGrade = 80";
            StrSql = StrSql + " Where MonthGrade <  80 ";
            StrSql = StrSql + " And  LEFT (GradeDate8,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " MonthGrade = 90 ";
            StrSql = StrSql + " Where MonthGrade <  90";
            StrSql = StrSql + " And  LEFT (GradeDate9,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " MonthGrade = 100 ";
            StrSql = StrSql + " Where MonthGrade <  100";
            StrSql = StrSql + " And  LEFT (GradeDate10,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " MonthGrade = 110 ";
            StrSql = StrSql + " Where MonthGrade <  110";
            StrSql = StrSql + " And  LEFT (GradeDate11,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " MonthGrade = 120 ";
            StrSql = StrSql + " Where MonthGrade <  120";
            StrSql = StrSql + " And  LEFT (GradeDate12,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //======================================================================



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade = 60 ";
            StrSql = StrSql + " Where OneGrade <  60 ";
            StrSql = StrSql + " And  LEFT (GradeDate6,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade = 70 ";
            StrSql = StrSql + " Where OneGrade <  70";
            StrSql = StrSql + " And  LEFT (GradeDate7,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade = 80";
            StrSql = StrSql + " Where OneGrade <  80 ";
            StrSql = StrSql + " And  LEFT (GradeDate8,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade = 90 ";
            StrSql = StrSql + " Where OneGrade <  90";
            StrSql = StrSql + " And  LEFT (GradeDate9,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade = 100 ";
            StrSql = StrSql + " Where OneGrade <  100";
            StrSql = StrSql + " And  LEFT (GradeDate10,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade = 110 ";
            StrSql = StrSql + " Where OneGrade <  110";
            StrSql = StrSql + " And  LEFT (GradeDate11,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " OneGrade = 120 ";
            StrSql = StrSql + " Where OneGrade <  120";
            StrSql = StrSql + " And  LEFT (GradeDate12,6) ='" + ToEndDate.Substring(0, 6) + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //======================================================================



            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + " MonthGrade = OneGrade ";
            StrSql = StrSql + " Where  LEFT (GradeDate6,6) <>'" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And   LEFT (GradeDate7,6) <>'" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And   LEFT (GradeDate8,6) <>'" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And   LEFT (GradeDate9,6) <>'" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And   LEFT (GradeDate10,6) <>'" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And   LEFT (GradeDate11,6) <>'" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And   LEFT (GradeDate12,6) <>'" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + "";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }
    

        //private void Put_ReqTF3(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        //{
        //    pg1.Value = 0; pg1.Maximum = 4;
        //    pg1.PerformStep(); pg1.Refresh();
        //    string StrSql = "";
    

        //    //전달 마지막 주의 직급을 가져온다. 전달 직급을 가져온다.
        //    StrSql = " Update tbl_ClosePay_02 SET";
        //    StrSql = StrSql + " Be_M_Grade = IsNull(b.A1, 0)";
        //    StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";

        //    StrSql = StrSql + " (";
        //    StrSql = StrSql + " Select  ";
        //    StrSql = StrSql + " OneGrade A1,Mbid,Mbid2 ";
        //    StrSql = StrSql + " From tbl_ClosePay_02_Mod (nolock) ";
        //    StrSql = StrSql + " Where   ToEndDate in (Select Max(ToEndDate) From tbl_CloseTotal_02 (nolock) Where LEFT(ToEndDate,6) < '" + ToEndDate.Substring (0,6)  + "' )";            
        //    StrSql = StrSql + " ) B";
        //    StrSql = StrSql + " Where a.Mbid = b.Mbid ";
        //    StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
            
        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();

        //    //이번달의 개인별 매출을 가져온다.
        //    StrSql = " Update tbl_ClosePay_02 SET";
        //    StrSql = StrSql + " Cur_M_PV = IsNull(b.A1, 0)";
        //    StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";

        //    StrSql = StrSql + " (";
        //    StrSql = StrSql + " Select  ";
        //    StrSql = StrSql + " Sum(BS1.TotalPV) A1 ";
        //    StrSql = StrSql + " ,BS1.Mbid,BS1.Mbid2 ";
        //    StrSql = StrSql + " From tbl_SalesDetail AS BS1 (nolock) ";
        //    //StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate_2 <= '" + PayDate + "'";
        //    StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
        //    StrSql = StrSql + " Where   LEFT(BS1.SellDate_2,6) = '" + FromEndDate.Substring(0, 6) + "'";
        //    StrSql = StrSql + " And     BS1.SellDate_2 <= '" + ToEndDate + "'";
        //    StrSql = StrSql + " And     BS1.TotalPV  + BS1.TotalCV >= 0 ";
        //    StrSql = StrSql + " And     BS1.SellCode <> '' ";
        //    StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
        //    StrSql = StrSql + " Group By BS1.Mbid,BS1.Mbid2";            
        //    StrSql = StrSql + " ) B";
        //    StrSql = StrSql + " Where a.Mbid = b.Mbid ";
        //    StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
            
        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();


        //}

       // private void Put_ReqTF3_2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
       //{
       //    pg1.Value = 0; pg1.Maximum = 4;
       //    pg1.PerformStep(); pg1.Refresh();
       //    string StrSql = "";

       //    int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
       //    string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
       //    double Allowance1 = 0, TotalPV = 0, TH_TotalPV = 0, KR_TotalPV = 0;
       //    int TSW = 0 ;
       //    int TSW2 = 0; 

       //    int t_qu_Cnt = 0;
       //    Dictionary<int, string> t_qu = new Dictionary<int, string>();

       //    StrSql = " Select Cur_M_PV   TotalPV , Se.M_Name,  Se.Mbid,Se.Mbid2 ";
       //    StrSql = StrSql + " From tbl_ClosePay_02 Se (nolock) ";
       //    StrSql = StrSql + " WHERE Cur_M_PV  <> 0  ";
       //    StrSql = StrSql + " And  Be_M_Grade < 20 "; 

       //    DataSet ds = new DataSet();
       //    ReCnt = 0;
       //    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
       //    ReCnt = Search_Connect.DataSet_ReCount;

       //    pg1.Value = 0; pg1.Maximum = ReCnt + 1;

       //    for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
       //    {
       //        LevelCnt = 0; TSaveid = "**";
       //        Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
       //        Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
       //        M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

          
       //        S_Mbid = Mbid + "-" + Mbid2.ToString();
       //        if (Clo_Mem.ContainsKey(S_Mbid) == true)
       //        {
       //            TSaveid = Clo_Mem[S_Mbid].Saveid;
       //            TSaveid2 = Clo_Mem[S_Mbid].Saveid2;
       //            TLine = Clo_Mem[S_Mbid].LineCnt;
       //        }

       //        OrderNumber = "";
       //        TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) ;

       //        TSW = 0;
       //        TSW2 = 0; 
       //        S_Mbid = TSaveid + "-" + TSaveid2.ToString();

       //        while (TSaveid != "**" && TSW == 0  )
       //        {
       //            LevelCnt++;

       //            if (Clo_Mem.ContainsKey(S_Mbid) == true)
       //            {
       //                if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "")
       //                {
       //                    if (TSW == 0)
       //                    {
       //                        StrSql = "Update tbl_ClosePay_02 SET ";
       //                        if (TLine == 1)
       //                            StrSql = StrSql + " GM_Cur_PV_1 = GM_Cur_PV_1 +  " + TotalPV;
       //                        else
       //                            StrSql = StrSql + " GM_Cur_PV_2 = GM_Cur_PV_2 +  " + TotalPV;

       //                        StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
       //                        StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

       //                        t_qu[t_qu_Cnt] = StrSql;
       //                        t_qu_Cnt++;
       //                    }
                                                      
       //                    if (Clo_Mem[S_Mbid].Be_M_Grade >= 20) TSW = 1;
                                   

       //                }

       //                TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;

       //                S_Mbid = TSaveid + "-" + TSaveid2.ToString();
       //            }
       //            else
       //            {
       //                TSaveid = "**";
       //            }
                   
       //        } //While

       //    }



       //    pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
       //    foreach (int tkey in t_qu.Keys)
       //    {
       //        StrSql = t_qu[tkey];
       //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
       //        pg1.PerformStep(); pg1.Refresh();
       //    }

       //}



        //private void Put_ReqTF3_3(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        //{

        //    pg1.Value = 0; pg1.Maximum = 4;
        //    pg1.PerformStep(); pg1.Refresh();
        //    string StrSql = "";

        //    string SDate3 = "";

        //    DateTime dt = DateTime.Parse(ToEndDate.Substring(0, 4) + "-" + ToEndDate.Substring(4, 2) + "-" + ToEndDate.Substring(6, 2));
        //    SDate3 = dt.AddMonths(-1).ToShortDateString().Replace("-", "");

        //    //---------------------------------------------------------------------------------------
        //    //---------------------------------------------------------------------------------------
        //    pg1.Value = 0; pg1.Maximum = 16;
        //    pg1.PerformStep(); pg1.Refresh();

        //    //2달간의 유예 기간을 주기로함.  2015-11-16일 이홍민 부사장님 요청에 의해서
        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  ReqTF3 = 0 ";
        //    StrSql = StrSql + " And  LEFT (RegTime,6) ='" + ToEndDate.Substring(0, 6) + "'";            

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();


        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  ReqTF3 = 0 ";
        //    StrSql = StrSql + " And ( LEFT (GradeDate2,6) ='" + ToEndDate.Substring(0, 6) + "'";
        //    StrSql = StrSql + " OR   LEFT (GradeDate3,6) ='" + ToEndDate.Substring(0, 6) + "'";
        //    StrSql = StrSql + " OR   LEFT (GradeDate4,6) ='" + ToEndDate.Substring(0, 6) + "'";
        //    StrSql = StrSql + " OR   LEFT (GradeDate5,6) ='" + ToEndDate.Substring(0, 6) + "'";
        //    StrSql = StrSql + " OR   LEFT (GradeDate6,6) ='" + ToEndDate.Substring(0, 6) + "'";
        //    StrSql = StrSql + " OR   LEFT (GradeDate7,6) ='" + ToEndDate.Substring(0, 6) + "'";
        //    StrSql = StrSql + " OR   LEFT (GradeDate8,6) ='" + ToEndDate.Substring(0, 6) + "'";
        //    StrSql = StrSql + " OR   LEFT (GradeDate9,6) ='" + ToEndDate.Substring(0, 6) + "'";
        //    StrSql = StrSql + " OR   LEFT (GradeDate10,6) ='" + ToEndDate.Substring(0, 6) + "'";
        //    StrSql = StrSql + " OR   LEFT (GradeDate11,6) ='" + ToEndDate.Substring(0, 6) + "'";
        //    StrSql = StrSql + " OR   LEFT (GradeDate12,6) ='" + ToEndDate.Substring(0, 6) + "'";
        //    StrSql = StrSql + ")";

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();


           



        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  ReqTF3 = 0 ";
        //    StrSql = StrSql + " And  LEFT (RegTime,6) ='" + ToEndDate.Substring(0, 6) + "'";            

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();




        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  Be_M_Grade <= 20  ";

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();



        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  Be_M_Grade >= 30 And Be_M_Grade <= 50   ";
        //    StrSql = StrSql + " And   (Cur_M_PV >= 40000 Or GM_Cur_PV_1 + GM_Cur_PV_2 >= 40000) ";

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();



        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  Be_M_Grade >= 60 And Be_M_Grade <= 70   ";
        //    StrSql = StrSql + " And   (Cur_M_PV >= 80000 Or GM_Cur_PV_1 + GM_Cur_PV_2 >= 80000)";

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();


        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  Be_M_Grade >= 80 And Be_M_Grade <= 90   ";
        //    StrSql = StrSql + " And   (Cur_M_PV >= 120000 Or GM_Cur_PV_1 + GM_Cur_PV_2 >= 120000)";

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();


        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  Be_M_Grade = 100  ";
        //    StrSql = StrSql + " And   (Cur_M_PV >= 160000 Or GM_Cur_PV_1 + GM_Cur_PV_2 >= 160000)";

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();

        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  Be_M_Grade >= 110  ";
        //    StrSql = StrSql + " And   (Cur_M_PV >= 200000 Or GM_Cur_PV_1 + GM_Cur_PV_2 >= 200000)";

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();
        //    //---------------------------------------------------------------------------------------
        //    //---------------------------------------------------------------------------------------





        //    //2016-01-27일 현 직급으로 해서 유지 여부를 체크할수 잇게 해달라 떨어진 직급으로 되게 해달라는 말임.
        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  OneGrade <= 20  ";
        //    StrSql = StrSql + " And ReqTF3 = 0 ";
        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();



        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  OneGrade >= 30 And OneGrade <= 50   ";
        //    StrSql = StrSql + " And   (Cur_M_PV >= 40000 Or GM_Cur_PV_1 + GM_Cur_PV_2 >= 40000) ";
        //    StrSql = StrSql + " And ReqTF3 = 0 ";

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();



        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  OneGrade >= 60 And OneGrade <= 70   ";
        //    StrSql = StrSql + " And   (Cur_M_PV >= 80000 Or GM_Cur_PV_1 + GM_Cur_PV_2 >= 80000)";
        //    StrSql = StrSql + " And ReqTF3 = 0 ";

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();


        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  OneGrade >= 80 And OneGrade <= 90   ";
        //    StrSql = StrSql + " And   (Cur_M_PV >= 120000 Or GM_Cur_PV_1 + GM_Cur_PV_2 >= 120000)";
        //    StrSql = StrSql + " And ReqTF3 = 0 ";

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();


        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  OneGrade = 100  ";
        //    StrSql = StrSql + " And   (Cur_M_PV >= 160000 Or GM_Cur_PV_1 + GM_Cur_PV_2 >= 160000)";
        //    StrSql = StrSql + " And ReqTF3 = 0 ";

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();



        //    StrSql = "Update tbl_ClosePay_02 SET ";
        //    StrSql = StrSql + "  ReqTF3 = 1 ";
        //    StrSql = StrSql + " Where  OneGrade >= 110  ";
        //    StrSql = StrSql + " And   (Cur_M_PV >= 200000 Or GM_Cur_PV_1 + GM_Cur_PV_2 >= 200000)";
        //    StrSql = StrSql + " And   ReqTF3 = 0 ";

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();
        //    //---------------------------------------------------------------------------------------
        //    //---------------------------------------------------------------------------------------
        //}




        //private void GradeUpLine2(int CurrentGrade, cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        //{
        //    int Cnt = 0;
        //    string StrSql = "", str_GradeCnt = "", str_GradeCnt1 = "", str_GradeCnt2 = "", str_GradeCnt3 = "";

        //    int Base_M_Lvl = MaxLevel;

        //    if (CurrentGrade == 10)
        //    {
        //        str_GradeCnt = " GradeCnt1_1 + GradeCnt1_2 ";
        //        str_GradeCnt1 = " GradeCnt1_1 "; str_GradeCnt2 = " GradeCnt1_2 ";

        //        StrSql = "Update tbl_ClosePay_02 SET GradeCnt1_1 =0,GradeCnt1_2 =0 ";
        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    }

        //    if (CurrentGrade == 20)
        //    {
        //        str_GradeCnt = " GradeCnt2_1 + GradeCnt2_2 ";
        //        str_GradeCnt1 = " GradeCnt2_1 "; str_GradeCnt2 = " GradeCnt2_2 ";

        //        StrSql = "Update tbl_ClosePay_02 SET GradeCnt2_1 =0 ,  GradeCnt2_2 =0 ";
        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    }

        //    if (CurrentGrade == 30)
        //    {
        //        str_GradeCnt = " GradeCnt3_1 + GradeCnt3_2 ";
        //        str_GradeCnt1 = " GradeCnt3_1 "; str_GradeCnt2 = " GradeCnt3_2 ";

        //        StrSql = "Update tbl_ClosePay_02 SET GradeCnt3_1 =0 ,  GradeCnt3_2 =0 ";
        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    }

        //    if (CurrentGrade == 40)
        //    {
        //        str_GradeCnt = " GradeCnt4_1 + GradeCnt4_2 ";
        //        str_GradeCnt1 = " GradeCnt4_1 "; str_GradeCnt2 = " GradeCnt4_2 ";

        //        StrSql = "Update tbl_ClosePay_02 SET GradeCnt4_1 =0 ,  GradeCnt4_2 =0 ";
        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    }

        //    if (CurrentGrade == 50)
        //    {
        //        str_GradeCnt = " GradeCnt5_1 + GradeCnt5_2 ";
        //        str_GradeCnt1 = " GradeCnt5_1 "; str_GradeCnt2 = " GradeCnt5_2 ";

        //        StrSql = "Update tbl_ClosePay_02 SET GradeCnt5_1 =0 ,  GradeCnt5_2 =0 ";
        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    }

        //    if (CurrentGrade == 60)
        //    {                
        //        str_GradeCnt = " GradeCnt6_1 + GradeCnt6_2 ";
        //        str_GradeCnt1 = " GradeCnt6_1 "; str_GradeCnt2 = " GradeCnt6_2 ";

        //        StrSql = "Update tbl_ClosePay_02 SET GradeCnt6_1 =0 ,  GradeCnt6_2 =0 ";
        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    }

        //    if (CurrentGrade == 70)
        //    {
        //        str_GradeCnt = " GradeCnt7_1 + GradeCnt7_2 ";
        //        str_GradeCnt1 = " GradeCnt7_1 "; str_GradeCnt2 = " GradeCnt7_2 ";

        //        StrSql = "Update tbl_ClosePay_02 SET GradeCnt7_1 =0 ,  GradeCnt7_2 =0 ";
        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    }

        //    if (CurrentGrade == 80)
        //    {
        //        str_GradeCnt = " GradeCnt8_1 + GradeCnt8_2 ";
        //        str_GradeCnt1 = " GradeCnt8_1 "; str_GradeCnt2 = " GradeCnt8_2 ";

        //        StrSql = "Update tbl_ClosePay_02 SET GradeCnt8_1 =0 ,  GradeCnt8_2 =0 ";
        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    }

        //    if (CurrentGrade == 90)
        //    {
        //        str_GradeCnt = " GradeCnt9_1 + GradeCnt9_2 ";
        //        str_GradeCnt1 = " GradeCnt9_1 "; str_GradeCnt2 = " GradeCnt9_2 ";

        //        StrSql = "Update tbl_ClosePay_02 SET GradeCnt9_1 =0 ,  GradeCnt9_2 =0 ";
        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    }

        //    if (CurrentGrade == 100)
        //    {
        //        str_GradeCnt = " GradeCnt10_1 + GradeCnt10_2 ";
        //        str_GradeCnt1 = " GradeCnt10_1 "; str_GradeCnt2 = " GradeCnt10_2 ";

        //        StrSql = "Update tbl_ClosePay_02 SET GradeCnt10_1 =0 ,  GradeCnt10_2 =0 ";
        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    }

        //    if (CurrentGrade == 110)
        //    {
        //        str_GradeCnt = " GradeCnt11_1 + GradeCnt11_2 ";
        //        str_GradeCnt1 = " GradeCnt11_1 "; str_GradeCnt2 = " GradeCnt11_2 ";

        //        StrSql = "Update tbl_ClosePay_02 SET GradeCnt11_1 =0 ,  GradeCnt11_2 =0 ";
        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    }


        //    if (CurrentGrade == 120)
        //    {
        //        str_GradeCnt = " GradeCnt12_1 + GradeCnt12_2 ";
        //        str_GradeCnt1 = " GradeCnt12_1 "; str_GradeCnt2 = " GradeCnt12_2 ";

        //        StrSql = "Update tbl_ClosePay_02 SET GradeCnt12_1 =0 ,  GradeCnt12_2 =0 ";
        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    }

        //    if (CurrentGrade >= 60 ) 
        //    {
        //        StrSql = " Select Isnull( Max(LevelCnt ),0)   ";
        //        StrSql = StrSql + " From tbl_ClosePay_02 Se (nolock) ";                
        //        StrSql = StrSql + " WHERE OneGrade =" + CurrentGrade ;

        //        DataSet ds = new DataSet();
        //        int ReCnt = 0;
        //        Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
        //        ReCnt = Search_Connect.DataSet_ReCount;

        //        if (ReCnt > 0)
        //        {
        //            Base_M_Lvl = int.Parse(ds.Tables[base_db_name].Rows[0][0].ToString());
        //        }
        //    }

        //    pg1.Value = 0; pg1.Maximum = Cnt + 4;
        //    pg1.PerformStep(); pg1.Refresh();

        //    Cnt = Base_M_Lvl;

        //    while (Cnt >= 1)
        //    {
        //        StrSql = "Update tbl_ClosePay_02 SET ";
        //        StrSql = StrSql + str_GradeCnt1 + "=ISNULL(B.A1,0) ";
        //        StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

        //        StrSql = StrSql + " (Select    Sum(" + str_GradeCnt + ") A1,Saveid,Saveid2 ";
        //        StrSql = StrSql + " From tbl_ClosePay_02 ";
        //        StrSql = StrSql + " Where " + str_GradeCnt + " > 0  ";
        //        StrSql = StrSql + " And LineCnt = 1 ";
        //        StrSql = StrSql + " And LevelCnt =" + Cnt;
        //        StrSql = StrSql + " Group By Saveid,Saveid2  ";
        //        StrSql = StrSql + " ) B";

        //        StrSql = StrSql + " Where A.Mbid=B.Saveid ";
        //        StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //        pg1.PerformStep(); pg1.Refresh();

        //        StrSql = "Update tbl_ClosePay_02 SET ";
        //        StrSql = StrSql + str_GradeCnt1 + " =" + str_GradeCnt1 + " + ISNULL(B.A1,0)  ";
        //        StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

        //        StrSql = StrSql + " (Select Count(Mbid) A1,   Saveid,Saveid2 ";
        //        StrSql = StrSql + " From tbl_ClosePay_02 ";
        //        if (CurrentGrade == 110)
        //            StrSql = StrSql + " Where OneGrade >= " + CurrentGrade;
        //        else
        //            StrSql = StrSql + " Where OneGrade = " + CurrentGrade;

        //        StrSql = StrSql + " And LineCnt = 1 ";
        //        StrSql = StrSql + " And LevelCnt =" + Cnt;
        //        StrSql = StrSql + " Group By Saveid,Saveid2  ";
        //        StrSql = StrSql + " ) B";

        //        StrSql = StrSql + " Where A.Mbid=B.Saveid ";
        //        StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //        pg1.PerformStep(); pg1.Refresh();
        //        //'''---------------------------------------------------------------



        //        StrSql = "Update tbl_ClosePay_02 SET ";
        //        StrSql = StrSql + str_GradeCnt2 + "=ISNULL(B.A1,0) ";
        //        StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

        //        StrSql = StrSql + " (Select    Sum(" + str_GradeCnt + ") A1,Saveid,Saveid2 ";
        //        StrSql = StrSql + " From tbl_ClosePay_02 ";
        //        StrSql = StrSql + " Where " + str_GradeCnt + "> 0  ";
        //        StrSql = StrSql + " And LineCnt >= 2 ";
        //        StrSql = StrSql + " And LevelCnt =" + Cnt;
        //        StrSql = StrSql + " Group By Saveid,Saveid2  ";
        //        StrSql = StrSql + " ) B";

        //        StrSql = StrSql + " Where A.Mbid=B.Saveid ";
        //        StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //        pg1.PerformStep(); pg1.Refresh();


        //        StrSql = "Update tbl_ClosePay_02 SET ";
        //        StrSql = StrSql + str_GradeCnt2 + " =" + str_GradeCnt2 + " + + ISNULL(B.A1,0)  ";
        //        StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

        //        StrSql = StrSql + " (Select Count(Mbid) A1,   Saveid,Saveid2 ";
        //        StrSql = StrSql + " From tbl_ClosePay_02 ";

        //        if (CurrentGrade == 110)
        //            StrSql = StrSql + " Where OneGrade >= " + CurrentGrade;
        //        else
        //            StrSql = StrSql + " Where OneGrade = " + CurrentGrade;


        //        StrSql = StrSql + " And LineCnt >= 2 ";
        //        StrSql = StrSql + " And LevelCnt =" + Cnt;
        //        StrSql = StrSql + " Group By Saveid,Saveid2  ";
        //        StrSql = StrSql + " ) B";

        //        StrSql = StrSql + " Where A.Mbid=B.Saveid ";
        //        StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

        //        Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //        pg1.PerformStep(); pg1.Refresh();
        //        // '''---------------------------------------------------------------

        //        Cnt = Cnt - 1;
        //    }


        //}




        private void GradeUpLine_ReqTF1( cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            int Cnt = 0;
            string StrSql = "", str_GradeCnt = "", str_GradeCnt1 = "", str_GradeCnt2 = "", str_GradeCnt3 = "";

            int Base_M_Lvl = MaxLevel;


            pg1.Value = 0; pg1.Maximum = Cnt + 4;
            pg1.PerformStep(); pg1.Refresh();

            Cnt = Base_M_Lvl;

            while (Cnt >= 1)
            {
                //StrSql = "Update tbl_ClosePay_02 SET ";
                //StrSql = StrSql + " Down_W4_QV_Real_1 =  ISNULL(B.A2,0)";
                //StrSql = StrSql + " ,Down_W_1_QV_Real_1 =    ISNULL(B.W_1,0)";
                //StrSql = StrSql + " ,Down_W_2_QV_Real_1 =    ISNULL(B.W_2,0)";
                //StrSql = StrSql + " ,Down_W_3_QV_Real_1 =    ISNULL(B.W_3,0)";
                //StrSql = StrSql + " ,Down_W_4_QV_Real_1 =    ISNULL(B.W_4,0)";
                //StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                //StrSql = StrSql + " (";
                //StrSql = StrSql + "Select  Sum(Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real  ) A2 ";
                //StrSql = StrSql + " ,Sum(Down_W_1_QV_Real_1 + Down_W_1_QV_Real_2 + W_1_QV_Real ) W_1 ";
                //StrSql = StrSql + " ,Sum(Down_W_2_QV_Real_1 + Down_W_2_QV_Real_2 + W_2_QV_Real ) W_2 ";
                //StrSql = StrSql + " ,Sum(Down_W_3_QV_Real_1 + Down_W_3_QV_Real_2 + W_3_QV_Real ) W_3 ";
                //StrSql = StrSql + " ,Sum(Down_W_4_QV_Real_1 + Down_W_4_QV_Real_2 +  W_4_QV_Real ) W_4 ";

                //StrSql = StrSql + " ,Saveid,Saveid2 ";
                //StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
                //StrSql = StrSql + " Where ((  Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real ) <>0    )  ";
                //StrSql = StrSql + " And   LevelCnt =" + Cnt;
                //StrSql = StrSql + " And   LineCnt =  1 ";
                //StrSql = StrSql + " Group By Saveid,Saveid2   ";
                //StrSql = StrSql + " ) B";

                //StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                //StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                //Temp_Connect.Insert_Data(StrSql, Conn, tran);


                //StrSql = "Update tbl_ClosePay_02 SET ";
                //StrSql = StrSql + " Down_W4_QV_Real_2 =  ISNULL(B.A2,0)";
                //StrSql = StrSql + " ,Down_W_1_QV_Real_2 =    ISNULL(B.W_1,0)";
                //StrSql = StrSql + " ,Down_W_2_QV_Real_2 =    ISNULL(B.W_2,0)";
                //StrSql = StrSql + " ,Down_W_3_QV_Real_2 =    ISNULL(B.W_3,0)";
                //StrSql = StrSql + " ,Down_W_4_QV_Real_2 =    ISNULL(B.W_4,0)";
                //StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                //StrSql = StrSql + " (";
                //StrSql = StrSql + "Select  Sum(Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real  ) A2 ";
                //StrSql = StrSql + " ,Sum(Down_W_1_QV_Real_1 + Down_W_1_QV_Real_2 + W_1_QV_Real ) W_1 ";
                //StrSql = StrSql + " ,Sum(Down_W_2_QV_Real_1 + Down_W_2_QV_Real_2 + W_2_QV_Real ) W_2 ";
                //StrSql = StrSql + " ,Sum(Down_W_3_QV_Real_1 + Down_W_3_QV_Real_2 + W_3_QV_Real ) W_3 ";
                //StrSql = StrSql + " ,Sum(Down_W_4_QV_Real_1 + Down_W_4_QV_Real_2 +  W_4_QV_Real ) W_4 ";

                //StrSql = StrSql + " ,Saveid,Saveid2 ";
                //StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
                //StrSql = StrSql + " Where ((  Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real ) <>0    )  ";
                //StrSql = StrSql + " And   LevelCnt =" + Cnt;
                //StrSql = StrSql + " And   LineCnt =  2 ";
                //StrSql = StrSql + " Group By Saveid,Saveid2   ";
                //StrSql = StrSql + " ) B";

                //StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                //StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                //Temp_Connect.Insert_Data(StrSql, Conn, tran);



                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + " ReqTF1_L_1 = ISNULL(B.A1,0) ";
               // StrSql = StrSql + " ,Down_W4_QV_Real_1 =  ISNULL(B.A2,0)";
                StrSql = StrSql + " ,Down_W_1_QV_Real_1 =    ISNULL(B.W_1,0)";
                StrSql = StrSql + " ,Down_W_2_QV_Real_1 =    ISNULL(B.W_2,0)";
                StrSql = StrSql + " ,Down_W_3_QV_Real_1 =    ISNULL(B.W_3,0)";
                StrSql = StrSql + " ,Down_W_4_QV_Real_1 =    ISNULL(B.W_4,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                StrSql = StrSql + " (Select    Sum( ReqTF1_L_1 + ReqTF1_L_2 ) A1,Saveid,Saveid2 ";
                StrSql = StrSql + " , Sum(Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real  ) A2 ";
                StrSql = StrSql + " ,Sum(Down_W_1_QV_Real_1 + Down_W_1_QV_Real_2 + W_1_QV_Real ) W_1 ";
                StrSql = StrSql + " ,Sum(Down_W_2_QV_Real_1 + Down_W_2_QV_Real_2 + W_2_QV_Real ) W_2 ";
                StrSql = StrSql + " ,Sum(Down_W_3_QV_Real_1 + Down_W_3_QV_Real_2 + W_3_QV_Real ) W_3 ";
                StrSql = StrSql + " ,Sum(Down_W_4_QV_Real_1 + Down_W_4_QV_Real_2 +  W_4_QV_Real ) W_4 ";
                StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
                StrSql = StrSql + " Where (ReqTF1_L_1 + ReqTF1_L_2  > 0  OR (  Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real ) <>0 ) ";
                StrSql = StrSql + " And LineCnt = 1 ";
                StrSql = StrSql + " And LevelCnt =" + Cnt;
                StrSql = StrSql + " Group By Saveid,Saveid2  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + " ReqTF1_L_1 = ReqTF1_L_1  + ISNULL(B.A1,0)  ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                StrSql = StrSql + " (Select Count(Mbid) A1,   Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
                StrSql = StrSql + " Where ReqTF1 =  1 ";
                StrSql = StrSql + " And  LineCnt = 1 ";
                StrSql = StrSql + " And LevelCnt =" + Cnt;
                StrSql = StrSql + " Group By Saveid,Saveid2  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
                //'''---------------------------------------------------------------



                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + " ReqTF1_L_2 = ISNULL(B.A1,0) ";
               // StrSql = StrSql + " ,Down_W4_QV_Real_2 =  ISNULL(B.A2,0)";
                StrSql = StrSql + " ,Down_W_1_QV_Real_2 =    ISNULL(B.W_1,0)";
                StrSql = StrSql + " ,Down_W_2_QV_Real_2 =    ISNULL(B.W_2,0)";
                StrSql = StrSql + " ,Down_W_3_QV_Real_2 =    ISNULL(B.W_3,0)";
                StrSql = StrSql + " ,Down_W_4_QV_Real_2 =    ISNULL(B.W_4,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                StrSql = StrSql + " (Select    Sum(ReqTF1_L_1 + ReqTF1_L_2) A1,Saveid,Saveid2 ";
                StrSql = StrSql + " , Sum(Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real  ) A2 ";
                StrSql = StrSql + " ,Sum(Down_W_1_QV_Real_1 + Down_W_1_QV_Real_2 + W_1_QV_Real ) W_1 ";
                StrSql = StrSql + " ,Sum(Down_W_2_QV_Real_1 + Down_W_2_QV_Real_2 + W_2_QV_Real ) W_2 ";
                StrSql = StrSql + " ,Sum(Down_W_3_QV_Real_1 + Down_W_3_QV_Real_2 + W_3_QV_Real ) W_3 ";
                StrSql = StrSql + " ,Sum(Down_W_4_QV_Real_1 + Down_W_4_QV_Real_2 +  W_4_QV_Real ) W_4 ";
                
                StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
                StrSql = StrSql + " Where (ReqTF1_L_1 + ReqTF1_L_2  > 0  OR (  Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real ) <>0 ) ";
                StrSql = StrSql + " And LineCnt >= 2 ";
                StrSql = StrSql + " And LevelCnt =" + Cnt;
                StrSql = StrSql + " Group By Saveid,Saveid2  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();


                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + " ReqTF1_L_2 = ReqTF1_L_2 + ISNULL(B.A1,0)  ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                StrSql = StrSql + " (Select Count(Mbid) A1,   Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
                StrSql = StrSql + " Where ReqTF1 =  1 ";
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



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Down_W4_QV_Real_1 = Down_W_1_QV_Real_1  ";
            StrSql = StrSql + ",Down_W4_QV_Real_2 = Down_W_1_QV_Real_2  ";
            StrSql = StrSql + " Where  Active_1_FLAG = 'Y'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Down_W4_QV_Real_1 = Down_W4_QV_Real_1 + Down_W_2_QV_Real_1  ";
            StrSql = StrSql + " D,own_W4_QV_Real_2 = Down_W4_QV_Real_2 + Down_W_2_QV_Real_2  ";
            StrSql = StrSql + " Where  Active_2_FLAG = 'Y'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Down_W4_QV_Real_1 = Down_W4_QV_Real_1 + Down_W_3_QV_Real_1  ";
            StrSql = StrSql + " ,Down_W4_QV_Real_2 = Down_W4_QV_Real_2 + Down_W_3_QV_Real_2  ";
            StrSql = StrSql + " Where  Active_3_FLAG = 'Y'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Down_W4_QV_Real_1 = Down_W4_QV_Real_1 + Down_W_4_QV_Real_1  ";
            StrSql = StrSql + " ,Down_W4_QV_Real_2 = Down_W4_QV_Real_2 + Down_W_4_QV_Real_2  ";
            StrSql = StrSql + " Where  ReqTF1 = 1";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


        }




        private void Put_Down_PV_01(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double TotalCV = 0, TotalPrice = 0 ;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select Se.TotalCV , 0 AS RePV, Se.TotalPrice  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate_2 SellDate_2 , Se.Mbid,Se.Mbid2, Se.Na_Code  ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalCV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalCV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";

            StrSql = StrSql + " WHERE Se.TotalCV  > 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            //StrSql = StrSql + " And   Se.SellCode = '01' ";

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
                //TotalCV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalCV"].ToString()) + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                TotalCV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalCV"].ToString());
                TotalPrice = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
               
                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        //판매원한태 누적잡히도록 수정함 2016-12-02일자에
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == ""  )
                        {

                            R_LevelCnt++;

                            StrSql = "Update tbl_ClosePay_02 SET ";

                            if (TLine == 1)
                            {
                                StrSql = StrSql + " Cur_PV_1 = Cur_PV_1 +  " + TotalCV;
                                StrSql = StrSql + " ,Cur_Price_1 = Cur_Price_1 +  " + TotalPrice;
                            }
                            else
                            {
                                StrSql = StrSql + " Cur_PV_2 = Cur_PV_2 +  " + TotalCV;
                                StrSql = StrSql + " ,Cur_Price_2 = Cur_Price_2 +  " + TotalPrice;
                            }

                            StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                            StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                            t_qu[t_qu_Cnt] = StrSql;
                            t_qu_Cnt++;



                            StrSql = "INSERT INTO tbl_Close_DownPV_PV_02";
                            StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                            StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV , ";
                            StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber  ) ";

                            StrSql = StrSql + "Values(";
                            StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                            StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                            StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                                        
                            StrSql = StrSql + TotalCV + " , " + LevelCnt + " ," + TLine;                            

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




        private void Put_Down_PV_0222(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double Allowance1 = 0, TotalPV = 0, TH_TotalPV = 0, KR_TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select Se.TotalCV TotalPV , 0 AS RePV  , Se.M_Name, AA1.OrderNumber, Se.SellCode, Se.SellDate_2 SellDate_2 , Se.Mbid,Se.Mbid2, Se.Na_Code, AA1.GivePay  ";
            StrSql = StrSql + " From tbl_Close_DownPV_ALL_02 AA1  (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail Se  (nolock) ON AA1.OrderNumber = Se.OrderNumber And AA1.SortOrder = '2' ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalCV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalCV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " WHERE Se.TotalCV  > 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Se.SellCode = '02' ";
            StrSql = StrSql + " And   AA1.OrderNumber is not null "; 


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
                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["GivePay"].ToString() );


                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        //판매원한태만 누적되도록 수저앟ㅁ 2016-12-02
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].Sell_Mem_TF == 0)
                        {

                            R_LevelCnt++;

                            StrSql = "Update tbl_ClosePay_02 SET ";


                            if (TLine == 1)
                                StrSql = StrSql + " Cur_PV_1 = Cur_PV_1 +  " + TotalPV;
                            else
                                StrSql = StrSql + " Cur_PV_2 = Cur_PV_2 +  " + TotalPV;


                            StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                            StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                            t_qu[t_qu_Cnt] = StrSql;
                            t_qu_Cnt++;



                            StrSql = "INSERT INTO tbl_Close_DownPV_PV_02";
                            StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                            StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV , ";
                            StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber  ) ";

                            StrSql = StrSql + "Values(";
                            StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                            StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                            StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";

                            StrSql = StrSql + TotalPV + " , " + LevelCnt + " ," + TLine;

                            StrSql = StrSql + ",'2' ,'" + OrderNumber + "')";

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
            double TotalCV = 0, Sell_DownPV = 0, Cut_PV = 0;
            string SaveMbid = "", SaveName = ""; 
            int SaveMbid2 = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select Se.TotalCV TotalCV, Se.TotalCV  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate_2 SellDate_2 , Se.Mbid,Se.Mbid2, Se.Re_BaseOrderNumber , Se.Na_Code ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " WHERE Se.TotalCV  <  0 ";
            StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
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
                TotalCV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalCV"].ToString());
               // double TotalCV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalCV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                //if (ds.Tables[base_db_name].Rows[fi_cnt]["Na_Code"].ToString() == "TH" )
                //{
                //    TotalCV = Chang_Search_Th_Re_Ord(OrderNumber, Re_BaseOrderNumber);
                //}

                


                StrSql = "SELECT  Sell_DownPV , SaveMbid, SaveMbid2, SaveName ,SortOrder , LineCnt ,LevelCnt     ";
                StrSql = StrSql + " From tbl_Close_DownPV_PV_02 (nolock) ";
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
                    
                   
                    if (Sell_DownPV == -TotalCV)
                        Cut_PV = TotalCV;
                    else
                        Cut_PV = TotalCV;
                        //Cut_PV = -Sell_DownPV;
                   


                    StrSql = "Update tbl_ClosePay_02 SET ";

                    if (LineCnt == 1)
                    {
//                        StrSql = StrSql + " ,Cur_PV_1 = Cur_PV_1 +  " + Cut_PV;
                        StrSql = StrSql + " Re_Cur_PV_1 = Re_Cur_PV_1 +  " + Cut_PV;
                        StrSql = StrSql + " ,Re_Cur_Qv_1 = Re_Cur_Qv_1 +  " + TotalCV;
                        
                    }
                    else
                    {
  //                      StrSql = StrSql + " ,Cur_PV_2 = Cur_PV_2 + " + Cut_PV;
                        StrSql = StrSql + " Re_Cur_PV_2 = Re_Cur_PV_2 +  " + Cut_PV;
                        StrSql = StrSql + " ,Re_Cur_Qv_2 = Re_Cur_Qv_2 +  " + TotalCV;
                    }
                     
                    StrSql = StrSql + " Where   Mbid = '" + SaveMbid + "'";
                    StrSql = StrSql + " And     Mbid2 = " + SaveMbid2;

                    t_qu[t_qu_Cnt] = StrSql;
                    t_qu_Cnt++;
                    



                    StrSql = "INSERT INTO tbl_Close_DownPV_PV_02";
                    StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                    StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV ,  ";
                    StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_DownPV, R_DownQV ) ";
                    StrSql = StrSql + "Values(";

                    StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'," + Mbid2 + ",'" + M_Name + "',";
                    StrSql = StrSql + "'" + SaveMbid + "'," + SaveMbid2 + ",'" + SaveName + "',";
                    StrSql = StrSql + Cut_PV + ", " + LevelCnt + " ," + LineCnt;
                    StrSql = StrSql + ",'-1' ,'" + OrderNumber + "'," + TotalCV + "," + TotalCV + " )";


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


            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + "  Sham_PV_1 = Isnull(B.A1,0 )  ";
            //StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            //StrSql = StrSql + " (Select Sum(Apply_Pv) A1 ,   Mbid,Mbid2 ";
            //StrSql = StrSql + " From tbl_Sham_Sell_Down_2  (nolock) ";
            //StrSql = StrSql + " Where Apply_Date >= '" + FromEndDate + "'";
            //StrSql = StrSql + " And   Apply_Date <= '" + ToEndDate + "'";
            //StrSql = StrSql + " And   SellCode = '1' ";
            //StrSql = StrSql + " Group By Mbid,MBid2 ";
            //StrSql = StrSql + " ) B";

            //StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            //StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);

            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + "  Sham_PV_2 = Isnull(B.A1,0 )  ";
            //StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            //StrSql = StrSql + " (Select Sum(Apply_Pv) A1 ,   Mbid,Mbid2 ";
            //StrSql = StrSql + " From tbl_Sham_Sell_Down_2  (nolock) ";
            //StrSql = StrSql + " Where Apply_Date >= '" + FromEndDate + "'";
            //StrSql = StrSql + " And   Apply_Date <= '" + ToEndDate + "'";
            //StrSql = StrSql + " And   SellCode = '2' ";
            //StrSql = StrSql + " Group By Mbid,MBid2 ";
            //StrSql = StrSql + " ) B";

            //StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            //StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //151905808
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "   Sum_PV_1 = Be_PV_1 + Cur_PV_1 + Sham_PV_1 + Re_Cur_PV_1   ";
            StrSql = StrSql + "  ,Sum_PV_2 = Be_PV_2 + Cur_PV_2 + Sham_PV_2 + Re_Cur_PV_2 ";
            StrSql = StrSql + " Where Sell_Mem_TF = 0  ";
            StrSql = StrSql + " And ReqTF1 = 1 "; 
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "   Sum_PV_1 = Be_PV_1  ";
            StrSql = StrSql + " Where Sell_Mem_TF = 0  ";
            StrSql = StrSql + " And ReqTF1 = 0 ";
            StrSql = StrSql + " And Be_PV_1 < 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "   Sum_PV_1 = Sum_PV_1 + Re_Cur_PV_1  ";
            StrSql = StrSql + " Where Sell_Mem_TF = 0  ";
            StrSql = StrSql + " And ReqTF1 = 0 ";
            StrSql = StrSql + " And Re_Cur_PV_1 < 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "   Sum_PV_2 = Be_PV_2  ";
            StrSql = StrSql + " Where Sell_Mem_TF = 0  ";
            StrSql = StrSql + " And ReqTF1 = 0 ";
            StrSql = StrSql + " And Be_PV_2 < 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "   Sum_PV_2 = Sum_PV_2 + Re_Cur_PV_2   ";
            StrSql = StrSql + " Where Sell_Mem_TF = 0  ";
            StrSql = StrSql + " And ReqTF1 = 0 ";
            StrSql = StrSql + " And Re_Cur_PV_2 < 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Real_Sum_PV_1 =  Sum_PV_1 ";
            StrSql = StrSql + ",  Real_Sum_PV_2 =  Sum_PV_2 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            


            //if (FromEndDate == "20170309")
            //{
            //    StrSql = "Update tbl_ClosePay_02 SET ";
            //    StrSql = StrSql + " Sum_PV_2 = Sum_PV_2 + 151905808 ";                
            //    StrSql = StrSql + " Where Mbid2 = 58 ";  //2017-03-15 박해진 대리 요청에 의해서 
            //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //}



            ////소비자일경우에 하선누적 이월은 없다.   2016-12-21 박해진대리 요청에 의해선
            //StrSql = "Update tbl_ClosePay_02 set";
            //StrSql = StrSql + " Sum_PV_1  = 0 , Sum_PV_2 = 0 ";
            //StrSql = StrSql + " Where Sell_Mem_TF > 0  ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
        }





        private void Put_Self_PV( cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran )
       {
            pg1.Value = 0; pg1.Maximum = 4  ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
            string SDate3 = "";

            //DateTime dt = DateTime.Parse(ToEndDate.Substring(0, 4) + "-" + ToEndDate.Substring(4, 2) + "-" + ToEndDate.Substring(6, 2) );
            //SDate3 = dt.AddMonths(-3).ToShortDateString().Replace("-", "");


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Sell_PV_20150328 =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 ))  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";

            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  <='20150328'";            
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";

            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Sell_PV_20150328 = 1500 ";
            StrSql = StrSql + "  Where Mbid2  = 55666 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Sell_PV_30 =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 ))  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = Se.Mbid And tbl_Memberinfo.Mbid2 = Se.Mbid2 "; 
            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Datediff(Day,tbl_Memberinfo.RegTime,Se.SellDate_2 ) <= 30 ";
            StrSql = StrSql + " And   Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) >= 375 ";
            StrSql = StrSql + " And   Se.SellDate_2 > '20150328' ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            if (int.Parse(FromEndDate) >= 20150701)
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + " Sell_PV_30 =  ISNULL(B.a1,0) ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
                StrSql = StrSql + " (";
                StrSql = StrSql + " Select   Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 ))  a1 , Se.Mbid , Se.Mbid2 ";
                StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
                //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
                StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
                StrSql = StrSql + " LEFT JOIN  tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = Se.Mbid And tbl_Memberinfo.Mbid2 = Se.Mbid2 ";
                StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
                StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
                StrSql = StrSql + " And   Se.Ga_Order = 0 ";
                StrSql = StrSql + " And   Datediff(Day,tbl_Memberinfo.RegTime,Se.SellDate_2 ) <= 30 ";
                StrSql = StrSql + " And   Se.SellDate_2 > '20150328' ";
                StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
                StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
                StrSql = StrSql + " Having  Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 )) >= 375 ";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
                //StrSql = StrSql + " And   A.Sell_PV_30 < 375 ";
            
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            pg1.PerformStep(); pg1.Refresh();

            //201504150100001
            //201504130100004
            //201504300100071
            //201505060100016
            //201505200100005
            //201505210100021

            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Sell_PV_30 = Sell_PV_30 +  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 ))  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = Se.Mbid And tbl_Memberinfo.Mbid2 = Se.Mbid2 ";
            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Datediff(Day,tbl_Memberinfo.RegTime,Se.SellDate_2 ) <= 30 ";
            StrSql = StrSql + " And   Se.Ordernumber IN ('201504150100001', '201504130100004', '201504300100071', '201505060100016','201505200100005','201505210100021')  ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";           
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            




            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Sell_PV_60 =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 ))  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = Se.Mbid And tbl_Memberinfo.Mbid2 = Se.Mbid2 ";
            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Datediff(Day,tbl_Memberinfo.RegTime,Se.SellDate_2 ) <= 60 ";
            StrSql = StrSql + " And   Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) >= 375 ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " And   Se.SellDate_2 > '20150328' ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            StrSql = StrSql + " And   (A.Sell_PV_30 >= 375  OR A.Sell_PV_20150328 >= 375 ) ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            if (int.Parse(FromEndDate) >= 20150701)
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + " Sell_PV_60 =  ISNULL(B.a1,0) ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
                StrSql = StrSql + " (";
                StrSql = StrSql + " Select   Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 ))  a1 , Se.Mbid , Se.Mbid2 ";
                StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
                //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
                StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
                StrSql = StrSql + " LEFT JOIN  tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = Se.Mbid And tbl_Memberinfo.Mbid2 = Se.Mbid2 ";
                StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
                StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
                StrSql = StrSql + " And   Se.Ga_Order = 0 ";
                StrSql = StrSql + " And   Datediff(Day,tbl_Memberinfo.RegTime,Se.SellDate_2 ) <= 60 ";                
                StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
                StrSql = StrSql + " And   Se.SellDate_2 > '20150328' ";
                StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
                StrSql = StrSql + " Having  Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 )) >= 375 ";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
                StrSql = StrSql + " And   (A.Sell_PV_30 >= 375  OR A.Sell_PV_20150328 >= 375 ) ";
                //StrSql = StrSql + " And   A.Sell_PV_60 < 375 ";
            }
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Sell_PV_60 = Sell_PV_60 +  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
            StrSql = StrSql + " (";

            StrSql = StrSql + " Select   Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 ))  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = Se.Mbid And tbl_Memberinfo.Mbid2 = Se.Mbid2 ";
            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Datediff(Day,tbl_Memberinfo.RegTime,Se.SellDate_2 ) <= 60 ";
            StrSql = StrSql + " And   Se.Ordernumber IN ('201504150100001', '201504130100004', '201504300100071', '201505060100016','201505200100005','201505210100021')  ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            StrSql = StrSql + " And   (A.Sell_PV_30 > 0  OR A.Sell_PV_20150328 >= 375 ) ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);






            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Sell_PV_1125 =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 ))  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = Se.Mbid And tbl_Memberinfo.Mbid2 = Se.Mbid2 ";
            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Datediff(Day,tbl_Memberinfo.RegTime,Se.SellDate_2 ) > 60 ";
            StrSql = StrSql + " And   Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) >= 1125 ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";            
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            StrSql = StrSql + " And   (A.Sell_PV_30 >= 375  OR A.Sell_PV_20150328 >= 375 ) ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Sell_PV_750 =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 ))  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = Se.Mbid And tbl_Memberinfo.Mbid2 = Se.Mbid2 ";
            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Datediff(Day,tbl_Memberinfo.RegTime,Se.SellDate_2 ) > 60 ";
            StrSql = StrSql + " And   Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) >= 750 ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            StrSql = StrSql + " And   (A.Sell_PV_30 >= 375  OR A.Sell_PV_20150328 >= 375 ) ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
        
        }



        private void Put_Self_Point(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 16;
            pg1.PerformStep(); pg1.Refresh();

            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 2 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 >= 375 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();    


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 3 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 >= 750 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();    


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 5 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 >= 1500 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();    
            //-------------------------------------------------------------------- 3


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 2 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 = 0 ";
            StrSql = StrSql + " And    Sell_PV_30 >= 375 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();    


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 3 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 = 0 ";
            StrSql = StrSql + " And    Sell_PV_30 >= 750 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();    


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 5 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 = 0 ";
            StrSql = StrSql + " And    Sell_PV_30 >= 1500 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //-------------------------------------------------------------------- 6




            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 2 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 = 0 ";
            StrSql = StrSql + " And    Sell_PV_60 >= 375 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();    


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 3 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 = 0 ";
            StrSql = StrSql + " And    Sell_PV_60 >= 750 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();    


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 5 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 = 0 ";
            StrSql = StrSql + " And    Sell_PV_60 >= 1500 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //-------------------------------------------------------------------- 9


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 2 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 > 0 ";
            StrSql = StrSql + " And    Sell_PV_30 > 0 ";
            StrSql = StrSql + " And    Sell_PV_30 + Sell_PV_20150328 >= 375 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 3 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 > 0 ";
            StrSql = StrSql + " And    Sell_PV_30 > 0 ";
            StrSql = StrSql + " And    Sell_PV_30 + Sell_PV_20150328 >= 750 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 5 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 > 0 ";
            StrSql = StrSql + " And    Sell_PV_30 > 0 ";
            StrSql = StrSql + " And    Sell_PV_30 + Sell_PV_20150328 >= 1500 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //-------------------------------------------------------------------- 12


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 2 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 > 0 ";
            StrSql = StrSql + " And    Sell_PV_60 > 0 ";
            StrSql = StrSql + " And    Sell_PV_60 + Sell_PV_20150328 >= 375 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 3 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 > 0 ";
            StrSql = StrSql + " And    Sell_PV_60 > 0 ";
            StrSql = StrSql + " And    Sell_PV_60 + Sell_PV_20150328 >= 750 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 5 ";
            StrSql = StrSql + " Where   Sell_PV_20150328 > 0 ";
            StrSql = StrSql + " And    Sell_PV_60 > 0 ";
            StrSql = StrSql + " And    Sell_PV_60 + Sell_PV_20150328 >= 1500 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 5 ";
            StrSql = StrSql + " Where   Self_Point = 2 ";
            StrSql = StrSql + " And    Sell_PV_1125 > 0 ";            

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Self_Point = 5 ";
            StrSql = StrSql + " Where   Self_Point = 3 ";
            StrSql = StrSql + " And    Sell_PV_750 + Sell_PV_1125 > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //-------------------------------------------------------------------- 15

       }

        private void Nom_Point(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            pg1.Value = 0; pg1.Maximum = 4 ;
            pg1.PerformStep(); pg1.Refresh();

            string StrSql = "";
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Nom_Point = Isnull(B.A1,0)  ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (Select Sum(Self_Point - Be_Self_Point) A1,  Nominid,Nominid2 ";
            StrSql = StrSql + " From tbl_ClosePay_02  (nolock) ";
            StrSql = StrSql + " Where Self_Point - Be_Self_Point > 0";
            StrSql = StrSql + " Group By   Nominid,Nominid2 "; 
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid  = B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2 = B.Nominid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            

            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Nom_Sell_Cnt = Isnull(B.A1,0)  ";
            StrSql = StrSql + "  ,Nom_Sell_PV = Isnull(B.A2, 0)  ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (Select Count(Mbid) A1,Sum(DayPV01 + DayPV02) A2 , Nominid,Nominid2 ";
            StrSql = StrSql + " From tbl_ClosePay_02  (nolock) ";            
            StrSql = StrSql + " Group By   Nominid,Nominid2 ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid  = B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2 = B.Nominid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + "  Cur_Point = Self_Point ";
            //StrSql = StrSql + "  Where Self_Point > 0 ";
                        
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
        }




        private void Nom_Point_2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            StrSql = " Select Self_Point - Be_Self_Point Nom_Point , Cl2.M_Name, Cl2.Mbid,Cl2.Mbid2 ";
            StrSql = StrSql + " From tbl_ClosePay_02 Cl2 (nolock) ";
            StrSql = StrSql + " WHERE Self_Point - Be_Self_Point > 0";

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
                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Nom_Point"].ToString()); 
                



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
                                && Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].ShamPV >= 100
                                && int.Parse(Clo_Mem[S_Mbid].Cur_End_35) >= int.Parse(FromEndDate )
                                )
                        {
                            
                            R_LevelCnt++;


                            StrSql = "Update tbl_ClosePay_02 SET ";
                            StrSql = StrSql + " Nom_Point = Nom_Point +  " + R_TotalPV;                            
                            StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                            StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                            t_qu[t_qu_Cnt] = StrSql;
                            t_qu_Cnt++;


                            StrSql = "INSERT INTO tbl_Close_DownPV_PV_02";
                            StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                            StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV ,  ";
                            StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_DownPV) ";

                            StrSql = StrSql + "Values(";
                            StrSql = StrSql + "'" + ToEndDate + "','" + Mbid.ToString() + "'";
                            StrSql = StrSql + "," + Mbid2.ToString() + ",'" + M_Name.ToString() + "',";
                            StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                            StrSql = StrSql + R_TotalPV + ", " + LevelCnt + " ," + TLine;
                            StrSql = StrSql + ",'SP' ,'1',0)";

                            t_qu[t_qu_Cnt] = StrSql;
                            t_qu_Cnt++;
                   

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


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Nom_Sell_Cnt = Isnull(B.A1,0)  ";
            StrSql = StrSql + "  ,Nom_Sell_PV = Isnull(B.A2, 0)  ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (Select Count(Mbid) A1,Sum(DayPV01 + DayPV02) A2 , Nominid,Nominid2 ";
            StrSql = StrSql + " From tbl_ClosePay_02  (nolock) ";
            StrSql = StrSql + " Group By   Nominid,Nominid2 ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid  = B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2 = B.Nominid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
        }



        private void Cur_Point(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();

            string StrSql = "";

            if (int.Parse (FromEndDate ) >= 20150329)
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "  Self_Point = 5 ";
                StrSql = StrSql + "  Where Mbid2  IN (  8205069  ,8233270,8252480 ,8287307 ,8265446 ,8258953 ,8293440 ,8263620,8244268,8241910 ,8265429 ) ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            





            if (int.Parse(FromEndDate) >= 20150405)
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "  Self_Point = 5 ";
                StrSql = StrSql + "  Where Mbid2  IN (  8272523  ,8295613,8251708 ,8259231 ,8254672,8253384,8202693,8260278,8213450,8264849,8204771 ) ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (int.Parse(FromEndDate) >= 20150412)
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "  Self_Point = 5 ";
                StrSql = StrSql + "  Where Mbid2  IN ( 8260278,8213450,8264849,8204771 ) ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (int.Parse(FromEndDate) >= 20150426)
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "  Self_Point = 5 ";
                StrSql = StrSql + "  Where Mbid2  IN ( 8259587,8263006,8269554,8239414 ) ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }


            if (int.Parse(FromEndDate) >= 20150524)
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "  Self_Point = 5 ";
                StrSql = StrSql + "  Where Mbid2  IN ( 8293225 ) ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }


            if (int.Parse(FromEndDate) >= 20150531)
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "  Self_Point = 5 ";
                StrSql = StrSql + "  Where Mbid2  IN ( 8233734,8260637,8251018,8241709,8244160,8270826,8259153,8234993,8241061,8254725,8278045,8263742,8276260,8299399,8264312,8215403,8214349,8226328,8206372,8220461,8229003,8214222 ) ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (int.Parse(FromEndDate) >= 20150607)
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "  Self_Point = 5 ";
                StrSql = StrSql + "  Where Mbid2  IN ( 8278028,8283392,8241709, 8200271, 8262310 , 8224378 , 8272010 , 8235566 , 8209285  ) ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }


            if (int.Parse(FromEndDate) >= 20150614)
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "  Self_Point = 5 ";
                StrSql = StrSql + "  Where Mbid2  IN ( 8253339,8252926 ,8214992) ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }




            //tbl_Sham_Sell_Down_2
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Self_Point = ISNULL(B.A1, 0 )   ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (Select    Apply_Pv  A1,  mbid ,mbid2   ";
            StrSql = StrSql + " From tbl_Sham_Sell_Down_2 (nolock) ";            
            StrSql = StrSql + " Where     Apply_Date <= '" + ToEndDate + "' ";
            StrSql = StrSql + " And  seq in (Select Max(seq) From tbl_Sham_Sell_Down_2  (nolock)  Where     Apply_Date <= '" + ToEndDate + "' Group by Mbid,Mbid2  ) ";
            
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid  = B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2  = B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);



 


            //본인점수만으로 수당에 참여 할수 잇게 했느나.. 유과장님 요청에 의해서 둘다 있어야지만되게 변경함.
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Cur_Point = Self_Point * Nom_Point ";
            StrSql = StrSql + "  Where Self_Point > 0  " ;
            StrSql = StrSql + "  And Nom_Point > 0 ";

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


            StrSql = "Select Isnull(Max(ToEndDate),'') , Isnull(Max(FromEndDate),'') From tbl_ClosePay_02 (nolock) ";

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
                StrSql = "Select Isnull(Max(ToEndDate),'') , Isnull(Max(FromEndDate),'') From tbl_ClosePay_02  (nolock) ";
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
                StrSql = "Select Isnull(Max(ToEndDate),'') , Isnull(Max(FromEndDate),'') From tbl_ClosePay_02  (nolock)  ";
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
                StrSql = StrSql + " From tbl_ClosePay_02_Mod (nolock) "  ; 
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
                        StrSql = "Select Mbid,Mbid2 From tbl_ClosePay_02_Mod  (nolock)  ";
                        StrSql = StrSql + "  Where Mbid  = '" + Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "'";
                        StrSql = StrSql + "  And   Mbid2 =  " + Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                        StrSql = StrSql + "  And   ToEndDate  = '" + Dset4.Tables[base_db_name].Rows[fi_cnt]["ToEndDate"].ToString() + "'";
                        StrSql = StrSql +  " And   Allowance1  > 0 " ;
                                                
                        DataSet Dset5 = new DataSet();
                        Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset5);                        
                        int ReCnt5 = Search_Connect.DataSet_ReCount;

                        if (ReCnt5 <= 0)
                        {
                            StrSql = "Update tbl_ClosePay_02 SET ";
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

        private void Put_Start_35_End_35(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 6;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Cur_Start_35_100 =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Max(Se.SellDate_2)  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Totalpv >= 100 ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            


            StrSql = " Update tbl_ClosePay_02 SET";
            StrSql = StrSql + " Cur_End_35_100 = Convert(varchar,dateadd(day, 34, Cur_Start_35_100),112)  ";
            StrSql = StrSql + " Where Cur_Start_35_100  <> '' ";
            StrSql = StrSql + " And  Cur_Start_35_100  >= Convert(varchar,dateadd(day, -34, '" + FromEndDate + "'),112) ";
            StrSql = StrSql + " And   Cur_End_35_100 = '' ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //--------------------------------------------------------------------------------------------------------


            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Cur_Start_35 =  ISNULL(B.a1,0) ";
            //StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
            //StrSql = StrSql + " (";
            //StrSql = StrSql + " Select   Max(Se.SellDate_2)  a1 , Se.Mbid , Se.Mbid2 ";
            //StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            //StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_02 (nolock) ON tbl_ClosePay_02.Mbid = Se.Mbid And tbl_ClosePay_02.Mbid2 = Se.Mbid2 ";
            //StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            //StrSql = StrSql + " And  Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) < 100 ";
            //StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";

            ////StrSql = StrSql + " And   Se.SellDate_2  >= Cur_Start_35_100 ";
            ////StrSql = StrSql + " And   Se.SellDate_2  <=  Convert(varchar,dateadd(day, 34, Cur_Start_35_100),112) ";

            //StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";            
            //StrSql = StrSql + " ) B";
            //StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            //StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = " Update tbl_ClosePay_02 SET";
            //StrSql = StrSql + " TF_35 = 1 ";            
            //StrSql = StrSql + " Where Cur_Start_35  <> '' ";
            //StrSql = StrSql + " And  Cur_Start_35  >= Convert(varchar,dateadd(day, -34, '" +  FromEndDate + "'),112) ";
            
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " PV_35 =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 ))  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_02 (nolock) ON tbl_ClosePay_02.Mbid = Se.Mbid And tbl_ClosePay_02.Mbid2 = Se.Mbid2 ";

            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql + " And  Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) < 100 ";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  >= Cur_Start_35_100 ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            //StrSql = StrSql + " Having Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 )) >= 100 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            //StrSql = " Update tbl_ClosePay_02 SET";
            //StrSql = StrSql + " Cur_End_35  = Convert(varchar,dateadd(day, 34, Cur_Start_35),112) "; 
            //StrSql = StrSql + " Where Cur_Start_35  <> '' ";                        
            //StrSql = StrSql + " And  TF_35 = 1 ";
            //StrSql = StrSql + " And  PV_35 >= 100 ";            

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();



            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();

            int  TSaveid2 = 0, Mbid2 = 0;
            string Cur_Start_35_100 = "", Cur_Start_35 = "", Cur_EE_100 = "";
            double Sum_PV_1 = 0,TotalPV_2 = 0, PV_35= 0;
            //int L_1 = 0, L_2 = 0;

            //int t_qu_Cnt = 0;
            //Dictionary<int, string> t_qu = new Dictionary<int, string>();
            //Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

           // StrSql = StrSql + " And   Se.SellDate_2  >= Cur_Start_35_100 ";
           // StrSql = StrSql + " And   Se.SellDate_2  <=  Convert(varchar,dateadd(day, 34, Cur_Start_35_100),112) ";

            StrSql = " Select Mbid,Mbid2 , Cur_Start_35 , Cur_Start_35_100 , PV_35  ,  Convert(varchar,dateadd(day, 34, Cur_Start_35_100),112) Cur_EE_100  ";            
            StrSql = StrSql + " From tbl_ClosePay_02  (nolock) ";
            StrSql = StrSql + " WHERE Cur_End_35_100  = '' ";
            StrSql = StrSql + " And   PV_35 >= 100 ";            
            StrSql = StrSql + " Order by Mbid , Mbid2  ASC ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {                                
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());               
                Cur_Start_35 = "" ;ds.Tables[base_db_name].Rows[fi_cnt]["Cur_Start_35"].ToString();
                PV_35 = 0; //double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["PV_35"].ToString());
                Cur_Start_35_100 = ds.Tables[base_db_name].Rows[fi_cnt]["Cur_Start_35_100"].ToString();
                Cur_EE_100 = ds.Tables[base_db_name].Rows[fi_cnt]["Cur_EE_100"].ToString();

                if (Mbid2 == 8216859)
                    Mbid2 = Mbid2;

                StrSql = " Select   Se.TotalPV - Isnull( Bs_R.TotalPV, 0 ) a1 , Se.Mbid , Se.Mbid2, Se.SellDate_2 SellDate_2 ";
                StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
                //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
                StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";                
                StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) < 100 ";            
                StrSql = StrSql + " And   Se.Mbid2 =" + Mbid2 ;              
                StrSql = StrSql + " And   Se.Ga_Order = 0 ";
                StrSql = StrSql + " And   Se.SellDate_2 <= '" + ToEndDate  + "'";     
                
           
                if (Cur_Start_35_100 != "")
                    StrSql = StrSql + " And   Se.SellDate_2 > '" + Cur_Start_35_100 + "'";

                StrSql = StrSql + " Order by   Se.SellDate_2 ASC, Se.RecordTime ASC "; 

                DataSet ds_2 = new DataSet();            
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds_2);
                int ReCnt_2 = Search_Connect.DataSet_ReCount;
            
                for (int fi_cnt_2 = 0; fi_cnt_2 <= ReCnt_2 - 1; fi_cnt_2++)
                {
                    Sum_PV_1 = double.Parse(ds_2.Tables[base_db_name].Rows[fi_cnt_2]["a1"].ToString());
                    Cur_Start_35 = ds_2.Tables[base_db_name].Rows[fi_cnt_2]["SellDate_2"].ToString(); 
                    
                    PV_35 = PV_35 +  Sum_PV_1 ;

                    if (PV_35 >= 100)
                    {
                        StrSql = "Update tbl_ClosePay_02 SET ";
                        StrSql = StrSql + " Cur_Start_35 =   '" + Cur_Start_35 + "'";
                        StrSql = StrSql + " ,Cur_End_35  = Convert(varchar,dateadd(day, 34, '" + Cur_Start_35 + "'),112) ";
                        StrSql = StrSql + " Where     Mbid2 = " + Mbid2;

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);

                        PV_35 = 0;
                        Cur_Start_35 = "";
                    }
                }

                
            }




            StrSql = " Update tbl_ClosePay_02 SET";
            StrSql = StrSql + " Cur_End_35  = Cur_End_35_100 ";
            StrSql = StrSql + " Where Cur_End_35  <> '' ";
            StrSql = StrSql + " And  Cur_End_35_100 <> '' ";
            StrSql = StrSql + " And  Cur_End_35 < Cur_End_35_100 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = " Update tbl_ClosePay_02 SET";
            StrSql = StrSql + " Cur_End_35  = Cur_End_35_100 ";
            StrSql = StrSql + " Where Cur_End_35  = '' ";
            StrSql = StrSql + " And  Cur_End_35_100 <> '' ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            if ( FromEndDate == "20150712")
            {
                StrSql = " Update tbl_ClosePay_02 SET";
                StrSql = StrSql + " Cur_End_35  = '' ";
                StrSql = StrSql + " Where Mbid2  = 8210502 ";     

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }



            //pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            //foreach (int tkey in t_qu.Keys)
            //{
            //    StrSql = t_qu[tkey];
            //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //    pg1.PerformStep(); pg1.Refresh();
            //}
              
        }


        private void Put_Start_35_End_35_002(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 3;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
           
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " PV_35 =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 ))  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_02 (nolock) ON tbl_Memberinfo.Mbid = Se.Mbid And tbl_Memberinfo.Mbid2 = Se.Mbid2 ";
            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  >= Cur_Start_35 ";
            StrSql = StrSql + " And   Se.SellDate_2  <= Cur_End_35 ";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Cur_End_35 >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Cur_End_35 <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Cur_Start_35 <> ''";
            StrSql = StrSql + " And   Cur_End_35 <> '' ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " Having Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 )) >= 100 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            //이번주로 해서 종료가 되는 유지 조건이 있다..근데 35일동안 100을 햇네  그럼 35일더 연장이 된다.
            StrSql = " Update tbl_ClosePay_02 SET";
            StrSql = StrSql + " Cur_Start_35 = convert(varchar,dateadd(day, 1, Be_End_35),112)  Be_Start_35 ";
            StrSql = StrSql + " ,Cur_End_35 =  convert(varchar,dateadd(day, 36, Be_End_35),112)  Be_Start_35 ";
            StrSql = StrSql + " Where Cur_End_35 >='" + FromEndDate + "'";
            StrSql = StrSql + " And  Cur_End_35 <='" + ToEndDate + "'";
            StrSql = StrSql + " And  PV_35 >= 100 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //100 이 안되네 저번 기간동안 매출이 그럼 유지 연장이 안된다.
            StrSql = " Update tbl_ClosePay_02 SET";
            StrSql = StrSql + " Cur_Start_35 = '' ";
            StrSql = StrSql + " ,Cur_End_35 =  '' ";
            StrSql = StrSql + " Where Cur_End_35 >='" + FromEndDate + "'";
            StrSql = StrSql + " And  Cur_End_35 <='" + ToEndDate + "'";
            StrSql = StrSql + " And  PV_35 < 100 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }
        


        private void Put_cls_Close_Mem(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            if (Clo_Mem != null)
                Clo_Mem.Clear() ;
            //SellPV01 SellSham01
            StrSql = "Select Mbid,Mbid2, M_Name, Saveid, Saveid2, Nominid, Nominid2, LineCnt , N_LineCnt, LeaveDate, StopDate  ";
            StrSql = StrSql + " , DayPV01, DayPV02 , DayPV03, SellPV01 , SellPV02 ,SellPV03 ";
            StrSql = StrSql + " , ReqTF1, ReqTF2 " ;
            StrSql = StrSql + " , OneGrade";
            StrSql = StrSql + " , SellSham01  ";
            StrSql = StrSql + " , OneGrade  ";
            StrSql = StrSql + " , LevelCnt  ";
            StrSql = StrSql + " , Sell_Mem_TF  ";
            
            StrSql = StrSql + "  From tbl_ClosePay_02 (nolock) ";

            
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

                t_c_mem.DayPV01 = double.Parse(sr.GetValue(11).ToString());
                t_c_mem.DayPV02 = double.Parse(sr.GetValue(12).ToString());
                t_c_mem.DayPV03 = double.Parse(sr.GetValue(13).ToString());

                t_c_mem.SellPV01 = double.Parse(sr.GetValue(14).ToString());
                t_c_mem.SellPV02 = double.Parse(sr.GetValue(15).ToString());
                t_c_mem.SellPV03 = double.Parse(sr.GetValue(16).ToString());

                t_c_mem.ReqTF1 = int.Parse(sr.GetValue(17).ToString());
                t_c_mem.ReqTF2 = int.Parse(sr.GetValue(18).ToString());

                t_c_mem.CurGrade = int.Parse(sr.GetValue(19).ToString());
                t_c_mem.ShamPV = double.Parse(sr.GetValue(20).ToString());
                t_c_mem.OneGrade  = int.Parse(sr.GetValue(21).ToString());
               // t_c_mem.Lvl = int.Parse(sr.GetValue(22).ToString());
                t_c_mem.Sell_Mem_TF = int.Parse(sr.GetValue(23).ToString());

              
                T_Clo_Mem[T_Mbid] = t_c_mem;

                pg1.PerformStep(); pg1.Refresh();
            }


            Clo_Mem = T_Clo_Mem;
            sr.Close(); sr.Dispose();            
        }



        private void Put_cls_Close_Mem_Grade_Chang(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            //if (Clo_Mem != null)
            //    Clo_Mem.Clear();
            //SellPV01 SellSham01
            StrSql = "Select Mbid,Mbid2, M_Name, Saveid, Saveid2, Nominid, Nominid2, LineCnt , N_LineCnt, LeaveDate, StopDate  ";
            StrSql = StrSql + " , DayPV01, DayPV02 , DayPV03, SellPV01 , SellPV02 ,SellPV03 ";
            StrSql = StrSql + " , ReqTF1, ReqTF2 ";
            StrSql = StrSql + " , OneGrade";
            StrSql = StrSql + " , SellSham01  ";
            StrSql = StrSql + " , Be_M_Grade  ";
            StrSql = StrSql + " , LevelCnt  ";
            StrSql = StrSql + "   From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + "  Where OneGrade >= 10 ";


            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;

            string T_Mbid = "";

            //Dictionary<string, cls_Close_Mem> T_Clo_Mem = new Dictionary<string, cls_Close_Mem>();

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;


            while (sr.Read())
            {
                //t_c_mem.CurGrade = int.Parse(sr.GetValue(19).ToString());


                T_Mbid = sr.GetValue(0).ToString() + "-" + sr.GetValue(1).ToString();
                Clo_Mem[T_Mbid].CurGrade = int.Parse(sr.GetValue(19).ToString());

                pg1.PerformStep(); pg1.Refresh();
            }


            //Clo_Mem = T_Clo_Mem;
            sr.Close(); sr.Dispose();
        }


        private void Put_cls_Close_Mem_Allowance_Chang(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            //if (Clo_Mem != null)
            //    Clo_Mem.Clear();
            //SellPV01 SellSham01
            StrSql = "Select Mbid,Mbid2, M_Name, Saveid, Saveid2, Nominid, Nominid2, LineCnt , N_LineCnt, LeaveDate, StopDate  ";
            StrSql = StrSql + " , DayPV01, DayPV02 , DayPV03, SellPV01 , SellPV02 ,SellPV03 ";
            StrSql = StrSql + " , ReqTF1, ReqTF2 ";
            StrSql = StrSql + " , Allowance1";
            StrSql = StrSql + " , SellSham01  ";
            StrSql = StrSql + " , OneGrade  ";
            StrSql = StrSql + " , LevelCnt  ";
            StrSql = StrSql + "   From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + "  Where OneGrade >= 10 ";


            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;

            string T_Mbid = "";

            //Dictionary<string, cls_Close_Mem> T_Clo_Mem = new Dictionary<string, cls_Close_Mem>();

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;


            while (sr.Read())
            {
                //t_c_mem.CurGrade = int.Parse(sr.GetValue(19).ToString());


                T_Mbid = sr.GetValue(0).ToString() + "-" + sr.GetValue(1).ToString();
                //Clo_Mem[T_Mbid].Allowance1 = double.Parse(sr.GetValue(19).ToString());

                pg1.PerformStep(); pg1.Refresh();
            }


            //Clo_Mem = T_Clo_Mem;
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
            StrSql = StrSql + " , Se.SellDate_2,  Ce1.SellPv01, Ce1.DayPV01   ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";    
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R   (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R   (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";                    
            StrSql = StrSql + " LEFT JOIN tbl_ClosePay_02 Ce1 ON Ce1.Mbid = SE.Mbid And Ce1.Mbid2 = SE.Mbid2";

            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) >= 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
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


        private void Put_Down_PV_01_TT(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran , int i)
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
                                SellDate_2 = sellinfo.SellDate,
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
                            StrSql = "Update tbl_ClosePay_02 SET ";
                            if (TLine == 1) 
                                 StrSql = StrSql + " Cur_PV_1 = Cur_PV_1 +  " + (Rs_TotalPV + Rs_RePV) ;
                                             
                             if (TLine >= 2)
                                 StrSql = StrSql + " Cur_PV_2 = Cur_PV_2 + " + (Rs_TotalPV + Rs_RePV) ; 

                            StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                            StrSql = StrSql + " And     Mbid2 = " + TSaveid2 ;

                            Temp_Connect.Insert_Data(StrSql, Conn, tran); 

                   
                            StrSql = "INSERT INTO tbl_Close_DownPV_PV_02" ;
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


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "   Sum_PV_1 = Be_PV_1 + Cur_PV_1 - Cut_PV_4_1 ";
            StrSql = StrSql + "  ,Sum_PV_2 = Be_PV_2 + Cur_PV_2 - Cut_PV_4_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
    
        }


        private void Put_Down_PV_01_TTTT(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            //    pg1.Value = 0; pg1.Maximum = 4;
            //pg1.PerformStep(); pg1.Refresh();
            //string StrSql = "";


            //int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            //string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "" ,  OrderNumber = "";
            //double Allowance1 = 0, Allowance1 = 0;

            //int t_qu_Cnt = 0;
            //Dictionary<int, string> t_qu = new Dictionary<int, string>();

            //StrSql = " Select Se.TotalPV , Isnull( Bs_R.TotalPV, 0 ) AS RePV  , Nominid , Nominid2, Saveid , Saveid2 , Ce1.Mbid , Ce1.Mbid2 , Ce1.N_LineCnt , Ce1.LineCnt,   " ;
            //StrSql = StrSql + " Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate_2, SellPv01, Daypv01 ";
    
            //StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            //StrSql = StrSql + " LEFT JOIN tbl_ClosePay_02 Ce1 ON Ce1.Mbid = SE.Mbid And Ce1.Mbid2 = SE.Mbid2";

            //StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            //StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
            //StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            //StrSql = StrSql + " And   Se.Ga_Order = 0 "; 
            
            //ReCnt = 0;
            //SqlDataReader sr = null;
            //Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            //ReCnt = Temp_Connect.DataSet_ReCount;

            
            //pg1.Value = 0; pg1.Maximum = ReCnt + 1;                         
            ////for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)            
            //while(sr.Read ())
            //{
            //    LevelCnt = 0;
            //    //TSaveid = ds.Tables[base_db_name].Rows[fi_cnt]["Nominid"].ToString();
            //    //TSaveid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Nominid2"].ToString());
            //    //TLine = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["N_LineCnt"].ToString());
            //    //Allowance1 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance1"].ToString());

            //    //Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
            //    //Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
            //    //M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

            //    TSaveid = sr.GetValue(4).ToString();
            //    TSaveid2 = int.Parse(sr.GetValue(5).ToString());
            //    TLine = int.Parse(sr.GetValue(9).ToString());

            //    Allowance1 = double.Parse(sr.GetValue(0).ToString());

            //    Mbid = sr.GetValue(6).ToString().ToString();
            //    Mbid2 = int.Parse(sr.GetValue(7).ToString().ToString());
            //    M_Name = sr.GetValue(10).ToString();

            //    OrderNumber = sr.GetValue(11).ToString();

            //    S_Mbid = TSaveid + "-" + TSaveid2.ToString();

            //    while (TSaveid != "**")
            //    {
            //        LevelCnt++;

            //        if (Clo_Mem.ContainsKey(S_Mbid) == true)
            //        {
            //            if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" )                        
            //            {
                          
            //                R_LevelCnt++;

            //                if (Allowance1 > 0)
            //                {
            //                    StrSql = "Update tbl_ClosePay_02 SET ";
            //                    if (TLine == 1)
            //                        StrSql = StrSql + " Cur_PV_1 = Cur_PV_1 +  " + Allowance1;
            //                    else
            //                        StrSql = StrSql + " Cur_PV_2 = Cur_PV_2 +  " + Allowance1;
            //                    StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
            //                    StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

            //                    //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //                    t_qu[t_qu_Cnt] = StrSql;
            //                    t_qu_Cnt++;



            //                    StrSql = "INSERT INTO tbl_Close_DownPV_PV_02";
            //                    StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
            //                    StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV , ";
            //                    StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

            //                    StrSql = StrSql + "Values(";
            //                    StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
            //                    StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
            //                    StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
            //                    StrSql = StrSql + Allowance1 + " , " + LevelCnt + " ," + TLine;
            //                    StrSql = StrSql + ",'1' ,'" + OrderNumber + "')";

            //                    //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //                    t_qu[t_qu_Cnt] = StrSql;
            //                    t_qu_Cnt++;
            //                }

            //            }

            //            TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;
            //        }
            //        else
            //        {
            //            TSaveid = "**";
            //        }

            //        //if (LevelCnt == 2) TSaveid = "**";

            //    } //While


            //    pg1.PerformStep(); pg1.Refresh();
            //}


            //sr.Close(); sr.Dispose();

            //pg1.Value = 0; pg1.Maximum = t_qu.Count + 1 ; pg1.Refresh();            
            //foreach (int tkey in t_qu.Keys )
            //{
            //    StrSql = t_qu[tkey];
            //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //    pg1.PerformStep(); pg1.Refresh();
            //}


            //pg1.Value = 0; pg1.Maximum = 4;
            //pg1.PerformStep(); pg1.Refresh();
            //string StrSql = "";

            //int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            //string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            //double Allowance1 = 0, Allowance1 = 0, TotalPV = 0;


            //int t_qu_Cnt = 0;
            //Dictionary<int, string> t_qu = new Dictionary<int, string>();

            //StrSql = " Select Se.TotalPV , Isnull( Bs_R.TotalPV, 0 ) AS RePV  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate_2 , Se.Mbid,Se.Mbid2 ";
            //StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";            

            //StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            //StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
            //StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            //StrSql = StrSql + " And   Se.Ga_Order = 0 ";

            //DataSet ds = new DataSet();
            //ReCnt = 0;
            //Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            //ReCnt = Search_Connect.DataSet_ReCount;

            //pg1.Value = 0; pg1.Maximum = ReCnt + 1;
                     
            //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            //{



            //    LevelCnt = 0; TSaveid = "**";
            //    //TSaveid = ds.Tables[base_db_name].Rows[fi_cnt]["Nominid"].ToString();
            //    //TSaveid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Nominid2"].ToString());
            //    //TLine = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["N_LineCnt"].ToString());
            //    //Allowance1 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance1"].ToString());

            //    Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
            //    Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
            //    M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

            //    S_Mbid = Mbid + "-" + Mbid2.ToString();
            //    if (Clo_Mem.ContainsKey(S_Mbid) == true)
            //    {
            //        TSaveid = Clo_Mem[S_Mbid].Saveid;
            //        TSaveid2 = Clo_Mem[S_Mbid].Saveid2;
            //        TLine = Clo_Mem[S_Mbid].LineCnt;
            //    }

            //    TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
            //    OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

            //    S_Mbid = TSaveid + "-" + TSaveid2.ToString();

            //    while (TSaveid != "**")
            //    {
            //        LevelCnt++;

            //        if (Clo_Mem.ContainsKey(S_Mbid) == true)
            //        {
            //            if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && (Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03 >= 180 || Clo_Mem[S_Mbid].CurGrade >= 10))
            //            {

            //                R_LevelCnt++;

            //                StrSql = "Update tbl_ClosePay_02 SET ";
            //                if (TLine == 1  )
            //                    StrSql = StrSql + " Cur_PV_1 = Cur_PV_1 +  " + TotalPV;
            //                else
            //                    StrSql = StrSql + " Cur_PV_2 = Cur_PV_2 +  " + TotalPV;
            //                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
            //                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

            //                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //                t_qu[t_qu_Cnt] = StrSql;
            //                t_qu_Cnt++;



            //                StrSql = "INSERT INTO tbl_Close_DownPV_PV_02";
            //                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
            //                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV , ";
            //                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

            //                StrSql = StrSql + "Values(";
            //                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
            //                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
            //                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
            //                StrSql = StrSql + TotalPV + " , " + LevelCnt + " ," + TLine;
            //                StrSql = StrSql + ",'1' ,'" + OrderNumber + "')";

            //                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //                t_qu[t_qu_Cnt] = StrSql;
            //                t_qu_Cnt++;
                           

            //            }

            //            TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;

            //            S_Mbid = TSaveid + "-" + TSaveid2.ToString();
            //        }
            //        else
            //        {
            //            TSaveid = "**";
            //        }

            //        //if (LevelCnt == 2) TSaveid = "**";

            //    } //While

            //}



            //pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            //foreach (int tkey in t_qu.Keys)
            //{
            //    StrSql = t_qu[tkey];
            //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //    pg1.PerformStep(); pg1.Refresh();
            //}







            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double Sum_PV_1 = 0, Sum_PV_2 = 0, R_TotalPV = 0, TotalPV = 0, TotalPV_2 = 0;
            

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            StrSql = " Select Se.TotalPV , Isnull( Bs_R.TotalPV, 0 ) AS RePV  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate_2 SellDate_2 , Se.Mbid,Se.Mbid2 ";
            StrSql = StrSql + " , C2.Sum_PV_1 , C2.Sum_PV_2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";

            StrSql = StrSql + " LEFT JOIN tbl_ClosePay_02 (nolock)  C2 ON C2.Mbid = Se.Mbid And C2.Mbid2 = Se.Mbid2 ";

            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            //StrSql = StrSql + " And   Se.SellCode = '01' "; //신규만 계산을 한다 재구매는 이제 포함안함.
            StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2 ASC , Se.OrderNumber  ASC ";

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

                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());

                Sum_PV_1 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sum_PV_1"].ToString());
                Sum_PV_2 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sum_PV_2"].ToString());

                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                


                S_Mbid = Mbid + "-" + Mbid2.ToString();
                if (Clo_Mem.ContainsKey(S_Mbid) == true)
                {
                    if (Sell_PV.ContainsKey(S_Mbid) == false)
                        Sell_PV[S_Mbid] = (Clo_Mem[S_Mbid].SellPV01 - (Clo_Mem[S_Mbid].DayPV01));

                    if (R_TotalPV >= 180) //18만 이상은 무조건 신규로 본다.
                        TotalPV_2 = R_TotalPV; 
                    else
                    {
                        if (Sell_PV[S_Mbid] >= 180)
                        {
                            TotalPV_2 = R_TotalPV;                        
                        }
                        else
                        {
                            if (Sell_PV[S_Mbid] + R_TotalPV < 180)
                            {
                                TotalPV_2 = 0; //180이하는 무조건 재구매로 본다/.                        
                            }
                            else
                            {
                                if (Sell_PV[S_Mbid] == 0)
                                {
                                    TotalPV_2 = R_TotalPV;
                                }
                                else
                                {
                                    TotalPV = 180 - Sell_PV[S_Mbid];
                                    TotalPV_2 = R_TotalPV - TotalPV;
                                }
                            }
                        }
                    }
                    Sell_PV[S_Mbid] = Sell_PV[S_Mbid] + R_TotalPV;
                }



                S_Mbid = Mbid + "-" + Mbid2.ToString();
                if (Clo_Mem.ContainsKey(S_Mbid) == true)
                {
                    TSaveid = Clo_Mem[S_Mbid].Saveid;
                    TSaveid2 = Clo_Mem[S_Mbid].Saveid2;
                    TLine = Clo_Mem[S_Mbid].LineCnt;
                }

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**" && TotalPV_2 > 0)
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && (Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03  >= 180 || Clo_Mem[S_Mbid].CurGrade >= 10))
                        {

                            R_LevelCnt++;

                            StrSql = "Update tbl_ClosePay_02 SET ";
                            if (TLine == 1  )
                                StrSql = StrSql + " Cur_PV_1 = Cur_PV_1 +  " + TotalPV_2;
                            else
                                StrSql = StrSql + " Cur_PV_2 = Cur_PV_2 +  " + TotalPV_2;
                            StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                            StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                            t_qu[t_qu_Cnt] = StrSql;
                            t_qu_Cnt++;



                            StrSql = "INSERT INTO tbl_Close_DownPV_PV_02";
                            StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                            StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV , ";
                            StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                            StrSql = StrSql + "Values(";
                            StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                            StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                            StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                            StrSql = StrSql + TotalPV_2 + " , " + LevelCnt + " ," + TLine;
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








        private void Put_Down_PV_02_TTT(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 9;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            string SDate3 = "";

            DateTime dt = DateTime.Parse(ToEndDate.Substring(0, 4) + "-" + ToEndDate.Substring(4, 2) + "-" + ToEndDate.Substring(6, 2));
            SDate3 = dt.AddMonths(-3).ToShortDateString().Replace("-", "");


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Sham_PV_1 = Isnull(B.A1,0 )  ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

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

            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Sham_PV_2 = Isnull(B.A1,0 )  ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

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


            if (int.Parse(FromEndDate) == 20150310)
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "   Sum_PV_1 = Be_PV_1 + Cur_PV_1 + Sham_PV_1  ";
                StrSql = StrSql + "  ,Sum_PV_2 = Be_PV_2 + Cur_PV_2 + Sham_PV_2 ";
                StrSql = StrSql + "  Where T_Be_PV_TF = 0 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "   Sum_PV_1 = T_Be_PV_1 + Cur_PV_1 + Sham_PV_1  ";
                StrSql = StrSql + "  ,Sum_PV_2 = T_Be_PV_2 + Cur_PV_2 + Sham_PV_2 ";
                StrSql = StrSql + "  Where T_Be_PV_TF = 1 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            else
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "   Sum_PV_1 = Be_PV_1 + Cur_PV_1 + Sham_PV_1  ";
                StrSql = StrSql + "  ,Sum_PV_2 = Be_PV_2 + Cur_PV_2 + Sham_PV_2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }
            if (FromEndDate == "20141102")
            {
                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + "   Sum_PV_1 = Sum_PV_1 + 60285   ";                
                StrSql = StrSql + "   Where Mbid = 'KR' ";
                StrSql = StrSql + "   And   Mbid2 = 881 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }



            if (FromEndDate == "20141207")
            {
                StrSql = " Update tbl_ClosePay_02 SET";
                StrSql = StrSql + "   Sum_PV_1 = 0 +  Cur_PV_1 ";
                StrSql = StrSql + "  ,Sum_PV_2 = 70497 +  Cur_PV_2 ";
                StrSql = StrSql + "   Where Mbid = 'KR' ";
                StrSql = StrSql + "   And   Mbid2 = 457 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }




            ////3천인 넘은 경우에는 그다음 부터 본인 매출을 소실적으로 잡히게 된다.
            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Self_PV_1 =  DayPV01 + DayPV02  + DayPV03 ";
            //StrSql = StrSql + ",Sum_PV_1 =  Sum_PV_1  + (DayPV01 + DayPV02  + DayPV03) ";
            //StrSql = StrSql + " Where Sum_PV_1 <= Sum_PV_2 ";
            //StrSql = StrSql + " And   ( SellPV01 + SellPV02 + SellPV03 ) - (DayPV01 + DayPV02  + DayPV03) >= 3000 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Self_PV_2 =  DayPV01 + DayPV02  + DayPV03 ";
            //StrSql = StrSql + ",Sum_PV_2 =  Sum_PV_2  + (DayPV01 + DayPV02  + DayPV03) ";
            //StrSql = StrSql + " Where Sum_PV_1 > Sum_PV_2 ";
            //StrSql = StrSql + " And   ( SellPV01 + SellPV02 + SellPV03 ) - (DayPV01 + DayPV02  + DayPV03) >= 3000 ";
            //StrSql = StrSql + " And   Self_PV_1 = 0 "; // 이미 위에서 적용된게 아닌 경우에만... 2라인으로 소실적으로 잡힌다.

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();








            //본인매출 소실적 누적 되는 부분은 없어지게됨 신마케팅 적용 되면서
            if (int.Parse(FromEndDate) < Chang_Base_CloDAte)
            {

                pg1.Value = 0; pg1.Maximum = 4;
                pg1.PerformStep(); pg1.Refresh();

                int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
                string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
                double Sum_PV_1 = 0, Sum_PV_2 = 0, R_TotalPV = 0, TotalPV = 0, TotalPV_2 = 0;
                int L_1 = 0, L_2 = 0;

                int t_qu_Cnt = 0;
                Dictionary<int, string> t_qu = new Dictionary<int, string>();
                Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

                StrSql = " Select Se.TotalPV , Isnull( Bs_R.TotalPV, 0 ) AS RePV  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate_2 SellDate_2 , Se.Mbid,Se.Mbid2 ";
                StrSql = StrSql + " , C2.Sum_PV_1 , C2.Sum_PV_2 ";
                StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
                //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
                StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";

                StrSql = StrSql + " LEFT JOIN tbl_ClosePay_02 (nolock)  C2 ON C2.Mbid = Se.Mbid And C2.Mbid2 = Se.Mbid2 ";

                StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
                StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
                StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
                StrSql = StrSql + " And   Se.Ga_Order = 0 ";
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

                    R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());

                    Sum_PV_1 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sum_PV_1"].ToString());
                    Sum_PV_2 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sum_PV_2"].ToString());

                    OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                    L_1 = 0; L_2 = 0;


                    S_Mbid = Mbid + "-" + Mbid2.ToString();
                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (Sell_PV.ContainsKey(S_Mbid) == false)
                            Sell_PV[S_Mbid] = (Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03 - (Clo_Mem[S_Mbid].DayPV01 + Clo_Mem[S_Mbid].DayPV02 + Clo_Mem[S_Mbid].DayPV03));


                        if (Sell_PV[S_Mbid] >= 3000)
                        {
                            TotalPV_2 = R_TotalPV;
                            L_2 = 1;
                        }
                        else
                        {
                            if (Sell_PV[S_Mbid] + R_TotalPV <= 3000)
                            {
                                TotalPV = R_TotalPV;
                                L_1 = 1;
                            }
                            else
                            {
                                TotalPV = 3000 - Sell_PV[S_Mbid];
                                TotalPV_2 = R_TotalPV - TotalPV;
                                L_2 = 1;
                                L_1 = 1;
                            }
                        }

                        Sell_PV[S_Mbid] = Sell_PV[S_Mbid] + R_TotalPV;
                    }



                    S_Mbid = Mbid + "-" + Mbid2.ToString();
                    if (L_2 == 1)
                    {
                        TSaveid = Clo_Mem[S_Mbid].Mbid;
                        TSaveid2 = Clo_Mem[S_Mbid].Mbid2;
                        TLine = Clo_Mem[S_Mbid].N_LineCnt;

                        if (Clo_Mem.ContainsKey(S_Mbid) == true)
                        {

                            if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "")
                            {

                                StrSql = "Update tbl_ClosePay_02 SET ";
                                if (Sum_PV_1 <= Sum_PV_2)
                                {
                                    StrSql = StrSql + " Self_PV_1 = Self_PV_1 +  " + TotalPV_2;
                                    StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 +  " + TotalPV_2;
                                }
                                else
                                {
                                    StrSql = StrSql + " Self_PV_2 = Self_PV_2 +  " + TotalPV_2;
                                    StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 +  " + TotalPV_2;
                                }
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;

                                StrSql = "INSERT INTO tbl_Close_DownPV_PV_02";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + TotalPV_2 + " , " + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'1' ,'" + OrderNumber + "')";

                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;

                            }

                        }
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
























            ////////'''--재구매는 본인 소실적으로 잡아준다.
            //////StrSql = "Update tbl_ClosePay_02 SET ";
            //////StrSql = StrSql + "   Sum_PV_1 =  Sum_PV_1 + DayPV02  ";
            //////StrSql = StrSql + " Where Sum_PV_1 <  Sum_PV_2"; 

            //////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //////pg1.PerformStep(); pg1.Refresh();


            //////StrSql = "Update tbl_ClosePay_02 SET " ;
            //////StrSql = StrSql + " Sum_PV_2 =  Sum_PV_2 + DayPV02  ";
            //////StrSql = StrSql + " Where Sum_PV_1 >=  Sum_PV_2";

            //////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //////pg1.PerformStep(); pg1.Refresh();


            //'''3개월간 본인 매출 실적이 없다. 뭐고 없다. 그럼 - 시킨다
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Fresh_1 =  Sum_PV_1 ";
            StrSql = StrSql + ",Sum_PV_1 =  0 ";
            StrSql = StrSql + " Where Self_M3_PV  <= 0";
            StrSql = StrSql + " And Sum_PV_1 > 0 ";
            StrSql = StrSql + " And Regtime  <= '" + SDate3 + "'"; 

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Fresh_1 =  Sum_PV_1 ";
            //StrSql = StrSql + ",Sum_PV_1 =  0 ";
            //StrSql = StrSql + " Where Self_M3_PV + Cur_PV_M3_1 +Cur_PV_M3_2 <= 0";
            //StrSql = StrSql + " And Sum_PV_1 > 0 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            //'''가입한지 3개월이 넘엇는데 매출이 없네 그럼 
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Fresh_2 =  Sum_PV_2 ";
            StrSql = StrSql + ",Sum_PV_2 =  0 ";
            StrSql = StrSql + " Where Self_M3_PV  <= 0";
            StrSql = StrSql + " And Sum_PV_2 > 0 ";
            StrSql = StrSql + " And Regtime  <= '" + SDate3 + "'"; 

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


        }




        private void Put_Down_PV_Re_TT(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {           

            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0, LineCnt = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", Re_BaseOrderNumber  = "";
            double TotalPV = 0, Sell_DownPV = 0 , Cut_PV= 0 ;
            string SaveMbid = "", SaveName= ""  ;
            int SaveMbid2 = 0 ;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select Se.TotalPV  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate_2 SellDate_2 , Se.Mbid,Se.Mbid2, Se.Re_BaseOrderNumber  ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";            
            StrSql = StrSql + " WHERE Se.TotalPV  <  0 ";
            StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            
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
                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) ;
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                StrSql = "SELECT  Sell_DownPV , SaveMbid, SaveMbid2, SaveName ,SortOrder , LineCnt ,LevelCnt ";
                StrSql = StrSql + " From tbl_Close_DownPV_PV_02 (nolock) ";
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
                        Cut_PV = Sell_DownPV; 

                    StrSql = "Update tbl_ClosePay_02 SET ";

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


                    StrSql = "INSERT INTO tbl_Close_DownPV_PV_02";
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


        private void Give_Allowance3(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate_2 = "";
            double Allowance2 = 0, Allowance1 = 0, R_TotalPV = 0, TotalPV = 0, TotalPV_2 = 0, GivePirce = 0 ;
            int L_1 = 0, L_2 = 0;

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            StrSql = " Select Se.TotalCv TotalPv , 0 AS RePV , Se.TotalPrice  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate_2 , Se.Mbid,Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " LEFT JOIN tbl_ClosePay_02 (nolock)  C2 ON C2.Mbid = Se.Mbid And C2.Mbid2 = Se.Mbid2 ";
            StrSql = StrSql + " WHERE Se.TotalCv > 0 ";
            StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   C2.Sell_Mem_TF = 1 ";
            
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
                if (Clo_Mem.ContainsKey(S_Mbid) == true)
                {
                    TSaveid = Clo_Mem[S_Mbid].Nominid;
                    TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;
                }

                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()); // +double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                SellDate_2 = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate_2"].ToString();
                GivePirce = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();


                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].Sell_Mem_TF  == 0 && Clo_Mem[S_Mbid].ReqTF1 >= 1 )
                        {
                            Allowance1 = 0;
                            Allowance2 = 0;
                            R_LevelCnt++;

                            //if (Clo_Mem[S_Mbid].CurGrade >= 10 ) Allowance1 = TotalPV * 0.5;
                            //if (Clo_Mem[S_Mbid].CurGrade >= 20) Allowance1 = TotalPV * 0.1;
                            //if (Clo_Mem[S_Mbid].CurGrade >= 30) Allowance1 = TotalPV * 0.05;


                            Allowance1 = (TotalPV * 0.25) * 1000;
                            string TPer = "25"; 
                            

                            if (Allowance1 > 0 )
                            {
                                Allowance1 = Allowance1  - (Allowance1 % 10);

                                StrSql = "Update tbl_ClosePay_02 SET ";
                                StrSql = StrSql + " Allowance3 = Allowance3 +  " + Allowance1;                                
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;


                                if (Allowance1 > 0)
                                {
                                    StrSql = "INSERT INTO tbl_Close_DownPV_ALL_02";
                                    StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                    StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , GivePirce , ";
                                    StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, TPer, SellDate) ";

                                    StrSql = StrSql + "Values(";
                                    StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                    StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                    StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                    StrSql = StrSql + Allowance1 + " ," + TotalPV + "," + GivePirce ; 
                                    StrSql = StrSql +  "," + LevelCnt + " ," + TLine;
                                    StrSql = StrSql + ",'3' ,'" + OrderNumber + "','" + TPer + "','" + SellDate_2 + "')";

                                    //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                    t_qu[t_qu_Cnt] = StrSql;
                                    t_qu_Cnt++;
                                }
                               
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



            //우대고객 커미션은 주당 50만 가능하다.
            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance3_Cut = Allowance3 - 500000 ";
            StrSql = StrSql + " Where Allowance3 > 0 ";
            StrSql = StrSql + " And   Allowance3 > 500000 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);            

        }




        private void Give_Allowance4(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 24;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            //tbl_ClosePay_02_G_FLAG

            StrSql = "Insert into tbl_ClosePay_02_G_FLAG (Mbid,Mbid2,Grade_FLAG , Give_ToEndDate ,Grade_FLAG_2) ";
            StrSql = StrSql + " Select Mbid,Mbid2, '20',  GradeDate2,1";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where GradeDate2 ='" + ToEndDate + "'";            

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into tbl_ClosePay_02_G_FLAG (Mbid,Mbid2,Grade_FLAG , Give_ToEndDate,Grade_FLAG_2 ) ";
            StrSql = StrSql + " Select Mbid,Mbid2, '30', GradeDate3,1 ";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where GradeDate3 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into tbl_ClosePay_02_G_FLAG (Mbid,Mbid2,Grade_FLAG , Give_ToEndDate ,Grade_FLAG_2) ";
            StrSql = StrSql + " Select Mbid,Mbid2, '40', GradeDate4,1 ";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where GradeDate4 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into tbl_ClosePay_02_G_FLAG (Mbid,Mbid2,Grade_FLAG , Give_ToEndDate ,Grade_FLAG_2) ";
            StrSql = StrSql + " Select Mbid,Mbid2, '50', GradeDate5,1 ";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where GradeDate5 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Insert into tbl_ClosePay_02_G_FLAG (Mbid,Mbid2,Grade_FLAG , Give_ToEndDate,Grade_FLAG_2 ) ";
            StrSql = StrSql + " Select Mbid,Mbid2, '60',  GradeDate6 ,1";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where GradeDate6 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into tbl_ClosePay_02_G_FLAG (Mbid,Mbid2,Grade_FLAG , Give_ToEndDate,Grade_FLAG_2 ) ";
            StrSql = StrSql + " Select Mbid,Mbid2, '70', GradeDate7 ,1";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where GradeDate7 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Insert into tbl_ClosePay_02_G_FLAG (Mbid,Mbid2,Grade_FLAG , Give_ToEndDate,Grade_FLAG_2 ) ";
            StrSql = StrSql + " Select Mbid,Mbid2, '80',  GradeDate8 ,1";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where GradeDate8 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into tbl_ClosePay_02_G_FLAG (Mbid,Mbid2,Grade_FLAG , Give_ToEndDate,Grade_FLAG_2 ) ";
            StrSql = StrSql + " Select Mbid,Mbid2, '90',  GradeDate9 ,1";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where GradeDate9 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into tbl_ClosePay_02_G_FLAG (Mbid,Mbid2,Grade_FLAG , Give_ToEndDate,Grade_FLAG_2 ) ";
            StrSql = StrSql + " Select Mbid,Mbid2, '100', GradeDate10 ,1";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where GradeDate10 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Insert into tbl_ClosePay_02_G_FLAG (Mbid,Mbid2,Grade_FLAG , Give_ToEndDate,Grade_FLAG_2 ) ";
            StrSql = StrSql + " Select Mbid,Mbid2, '110', GradeDate11,1 ";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where GradeDate11 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Insert into tbl_ClosePay_02_G_FLAG (Mbid,Mbid2,Grade_FLAG , Give_ToEndDate ,Grade_FLAG_2) ";
            StrSql = StrSql + " Select Mbid,Mbid2, '120', GradeDate12,1 ";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where GradeDate12 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //--------------------------------------------------------------------------------------------------------------------

            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance4 = 100000 ";
            StrSql = StrSql + " Where GradeDate2 ='" + ToEndDate  + "'";            

            Temp_Connect.Insert_Data(StrSql, Conn, tran); //브론즈
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance4 = 250000 ";
            StrSql = StrSql + " Where GradeDate3 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);  //실버
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance4 = 500000 ";
            StrSql = StrSql + " Where GradeDate4 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran); //골드
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance4 = 1000000 ";
            StrSql = StrSql + " Where GradeDate5 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran); //플래티넘
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance4 = 2500000 ";
            StrSql = StrSql + " Where GradeDate6 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran); //루비
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance4 = 500000 ";
            StrSql = StrSql + " Where GradeDate7 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran); //에메랄드
            pg1.PerformStep(); pg1.Refresh();



            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance4 = 10000000 ";
            StrSql = StrSql + " Where GradeDate8 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);  //다이아
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance4 = 15000000 ";
            StrSql = StrSql + " Where GradeDate9 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran); //더블다이아
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance4 = 30000000 ";
            StrSql = StrSql + " Where GradeDate10 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);  //트리풀다이아
            pg1.PerformStep(); pg1.Refresh();


            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance4 = 60000000 ";
            StrSql = StrSql + " Where GradeDate11 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran); //크라운
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance4 = 200000000 ";
            StrSql = StrSql + " Where GradeDate12 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran); //로얄크라운
            pg1.PerformStep(); pg1.Refresh();

        }

        private void Give_Allowance4_2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 6;
            pg1.PerformStep(); pg1.Refresh();

            string  StrSql= "" ;
            string SDate8 = "", SDate4 = "", SDate12 = "";

            StrSql = "select top 4 fromenddate , ToEndDate from dbo.tbl_CloseTotal_02  Where ToEndDate <> '" + ToEndDate + "' Order by ToEndDate DESC ";           
           int ReCnt = 0;
           DataSet Dset4 = new DataSet();
           Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
           ReCnt = Search_Connect.DataSet_ReCount;

           if (ReCnt >= 4)
           {
               SDate4 = Dset4.Tables[base_db_name].Rows[3][1].ToString();
           }
           pg1.PerformStep(); pg1.Refresh();



           StrSql = "select top 8 fromenddate , ToEndDate from dbo.tbl_CloseTotal_02  Where ToEndDate <> '" + ToEndDate + "' Order by ToEndDate DESC ";
           ReCnt = 0;
           DataSet Dset8 = new DataSet();
           Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset8);
           ReCnt = Search_Connect.DataSet_ReCount;

           if (ReCnt >= 8)
           {
               SDate8 = Dset8.Tables[base_db_name].Rows[7][1].ToString();
           }
           pg1.PerformStep(); pg1.Refresh();


           StrSql = "select top 12 fromenddate , ToEndDate from dbo.tbl_CloseTotal_02  Where ToEndDate <> '" + ToEndDate + "' Order by ToEndDate DESC  ";
           ReCnt = 0;
           DataSet Dset12 = new DataSet();
           Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset12);
           ReCnt = Search_Connect.DataSet_ReCount;

           if (ReCnt >= 12)
           {
               SDate12  = Dset12.Tables[base_db_name].Rows[11][1].ToString();
           }
           pg1.PerformStep(); pg1.Refresh();



           //다이아는 없어짐 수당 시작도 하기전에 2017-06-07 
           StrSql = "Update tbl_ClosePay_02 SET ";
           StrSql = StrSql + "  OneGrade_4 = Isnull(B.OneGrade, 0)  ";
           StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

           StrSql = StrSql + " (Select OneGrade,  Mbid,Mbid2 ";
           StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
           StrSql = StrSql + " Where ToEndDate  = '" + SDate8 + "'";
            StrSql = StrSql + " ) B";
           StrSql = StrSql + " Where A.Mbid=B.Mbid ";
           StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

           Temp_Connect.Insert_Data(StrSql, Conn, tran);
           pg1.PerformStep(); pg1.Refresh();

           StrSql = "Update tbl_ClosePay_02 SET ";
           StrSql = StrSql + "  OneGrade_8 = Isnull(B.OneGrade, 0)   ";
           StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

           StrSql = StrSql + " (Select OneGrade,  Mbid,Mbid2 ";
           StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
           StrSql = StrSql + " Where ToEndDate  = '" + SDate4 + "'";
           StrSql = StrSql + " ) B";
           StrSql = StrSql + " Where A.Mbid=B.Mbid ";
           StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

           Temp_Connect.Insert_Data(StrSql, Conn, tran);
           pg1.PerformStep(); pg1.Refresh();




           int  Mbid2 = 0;
           string Mbid = "", M_Name = "", GradeDate9 = "", GradeDate10 = "", GradeDate11 = "", GradeDate12 = "", Grade_FLAG = "", Grade_FLAG_2 = "";
           double Allowance5 = 0; 
           int L_1 = 0,  OneGrade_4 = 0, OneGrade_8 = 0, OneGrade_12 = 0 ;

           
           StrSql = " Select  Se.M_Name,  Se.Mbid,Se.Mbid2 , Grade_FLAG , Give_ToEndDate ";
           StrSql = StrSql + " , GradeDate9, GradeDate10, GradeDate11, GradeDate12 ";
           StrSql = StrSql + " , OneGrade_4,OneGrade_8, Se.OneGrade OneGrade_12 "; 
           StrSql = StrSql + " From tbl_ClosePay_02_G_FLAG   (nolock) ";
           StrSql = StrSql + " LEFT JOIN tbl_ClosePay_02 (nolock)  Se ON tbl_ClosePay_02_G_FLAG.Mbid = Se.Mbid And tbl_ClosePay_02_G_FLAG.Mbid2 = Se.Mbid2 ";
           StrSql = StrSql + " WHERE   Give_ToEndDate  ='" + SDate12 + "'";
           StrSql = StrSql + " And     Grade_FLAG  >=  90  ";
           StrSql = StrSql + " And     Grade_FLAG_2 = 1   "; //2차지급분에 대해서 조회가 안되게 하기 위함.
           StrSql = StrSql + " And     Cut_FLAG = '' ";
           StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2 , Grade_FLAG  ASC ";

           DataSet ds = new DataSet();
           ReCnt = 0;
           Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
           ReCnt = Search_Connect.DataSet_ReCount;

           pg1.Value = 0; pg1.Maximum = ReCnt + 1;

           for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
           {

               Allowance5 = 0; 
               

               Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
               Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
               M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

               OneGrade_4 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["OneGrade_4"].ToString());
               OneGrade_8 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["OneGrade_8"].ToString());
               OneGrade_12 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["OneGrade_12"].ToString());

               GradeDate9 = ds.Tables[base_db_name].Rows[fi_cnt]["GradeDate9"].ToString();
               GradeDate10 = ds.Tables[base_db_name].Rows[fi_cnt]["GradeDate10"].ToString();
               GradeDate11 = ds.Tables[base_db_name].Rows[fi_cnt]["GradeDate11"].ToString();
               GradeDate12 = ds.Tables[base_db_name].Rows[fi_cnt]["GradeDate12"].ToString();

               Grade_FLAG = ds.Tables[base_db_name].Rows[fi_cnt]["Grade_FLAG"].ToString();
               L_1 = 0 ;

               if (Grade_FLAG == "90")
               {
                   if (GradeDate9 == GradeDate10) L_1 = 1;

                   if (OneGrade_4 < 80) L_1 = 1;
                   if (OneGrade_8 < 80) L_1 = 1;
                   if (OneGrade_12 < 80) L_1 = 1;

                   Allowance5 = 10000000;
                   Grade_FLAG_2 = "90";
               }

               if (Grade_FLAG == "100")
               {
                   if (GradeDate10 == GradeDate11) L_1 = 1;

                   if (OneGrade_4 < 90) L_1 = 1;
                   if (OneGrade_8 < 90) L_1 = 1;
                   if (OneGrade_12 < 90) L_1 = 1;


                   Allowance5 = 20000000; 
                   Grade_FLAG_2 = "100" ;
               }

               if (Grade_FLAG == "110")
               {
                   if (GradeDate11 == GradeDate12) L_1 = 1;

                   if (OneGrade_4 < 100) L_1 = 1;
                   if (OneGrade_8 < 100) L_1 = 1;
                   if (OneGrade_12 < 100) L_1 = 1;

                   Allowance5 = 40000000;
                   Grade_FLAG_2 = "110";
               }

               if (Grade_FLAG == "120")
               {
                   if (OneGrade_4 < 110) L_1 = 1;
                   if (OneGrade_8 < 110) L_1 = 1;
                   if (OneGrade_12 < 110) L_1 = 1;

                   Allowance5 = 100000000;
                   Grade_FLAG_2 = "120";
               }


               if (L_1 == 0)  //상위로 같이 승급한 내역이 없다.
               {

                   StrSql = "Update tbl_ClosePay_02 SET ";
                   StrSql = StrSql + " Allowance5 = " + Allowance5;
                   StrSql = StrSql + " Where   Mbid = '" + Mbid + "'";
                   StrSql = StrSql + " And     Mbid2 = " + Mbid2;

                   Temp_Connect.Insert_Data(StrSql, Conn, tran);

                   StrSql = "Insert into tbl_ClosePay_02_G_FLAG (Mbid,Mbid2,Grade_FLAG , Give_ToEndDate , Grade_FLAG_2  ) ";
                   StrSql = StrSql + " Values ('" + Mbid + "'," + Mbid2 + ", " + Grade_FLAG_2 + ",'" + ToEndDate + "',2 )  ";

                   Temp_Connect.Insert_Data(StrSql, Conn, tran);
                   
               }

               pg1.PerformStep(); pg1.Refresh();                   
           }

        }


        private void Give_Allowance3_20160329(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 19;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " G9_Cnt = Be_G9_Cnt ";
            StrSql = StrSql + ",G10_Cnt = Be_G10_Cnt ";
            StrSql = StrSql + ",G11_Cnt = Be_G11_Cnt ";
            StrSql = StrSql + ",G12_Cnt = Be_G12_Cnt ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            DateTime Month_Before = new DateTime();
            string BeforeFromEndDate = "";
            Month_Before = DateTime.Parse(FromEndDate.Substring(0, 4) + '-' + FromEndDate.Substring(4, 2) + '-' + "01");
            BeforeFromEndDate = Month_Before.AddMonths(-1).ToString("yyyy-MM-dd").Replace("-", "");

            /*
            //사파이어는 한방에 지급을 한다.
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance3 = 2000000 ";
            StrSql = StrSql + " Where LEFT(GradeDate8, 6) = '" + FromEndDate.Substring(0,6) + "' ";
            //StrSql = StrSql + "    Where GradeDate8 >= '" + FromEndDate + "'";
            //StrSql = StrSql + "    And   GradeDate8 <= '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            */
            //6개월 분할 지급
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance3 = Allowance3  +  833333 ";
            StrSql = StrSql + " ,Allowance3_9 = Allowance3_9  +  833333 ";
            StrSql = StrSql + " ,G9_Cnt = G9_Cnt + 1 ";
            StrSql = StrSql + " Where LEFT(GradeDate9, 6) = '" + FromEndDate.Substring(0, 6) + "' ";
            //StrSql = StrSql + "    Where GradeDate9 >= '" + FromEndDate + "'";
            //StrSql = StrSql + "    And   GradeDate9 <= '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //6개월 분할 지급
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance3 = Allowance3  +  3333333 ";
            StrSql = StrSql + " ,Allowance3_10 = Allowance3_10  +  3333333 ";
            StrSql = StrSql + " ,G10_Cnt = G10_Cnt + 1 ";
            StrSql = StrSql + " Where LEFT(GradeDate10, 6) = '" + FromEndDate.Substring(0, 6) + "' ";
            //StrSql = StrSql + "    Where GradeDate10 >= '" + FromEndDate + "'";
            //StrSql = StrSql + "    And   GradeDate10 <= '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //6개월 분할 지급
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance3 = Allowance3  +  8333333 ";
            StrSql = StrSql + " ,Allowance3_11 = Allowance3_11  +  8333333 ";
            StrSql = StrSql + " ,G11_Cnt = G11_Cnt + 1 ";
            StrSql = StrSql + " Where LEFT(GradeDate11, 6) = '" + FromEndDate.Substring(0, 6) + "' ";
            //StrSql = StrSql + "    Where GradeDate11 >= '" + FromEndDate + "'";
            //StrSql = StrSql + "    And   GradeDate11 <= '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //6개월 분할 지급
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance3 = Allowance3  +  33333333 ";
            StrSql = StrSql + " ,Allowance3_12 = Allowance3_12  +  33333333 ";
            StrSql = StrSql + " ,G12_Cnt = G12_Cnt + 1 ";
            StrSql = StrSql + " Where LEFT(GradeDate12, 6) = '" + FromEndDate.Substring(0, 6) + "' ";
            //StrSql = StrSql + "    Where GradeDate12 >= '" + FromEndDate + "'";
            //StrSql = StrSql + "    And   GradeDate12 <= '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            

            //동일월에 주간마감이 몇번 돌았는지 체크한다 총 4번이 기본이기 때문에... 6개월전의 몇번째 정산에서 가져와야 되는지를 체크하기 위함.
            /*
            StrSql = " Select Top 4  ToEndDate ";
            StrSql = StrSql + " From tbl_CloseTotal_02 Se (nolock) ";
            StrSql = StrSql + " WHERE   LEFT(Se.ToEndDate,6)   ='" + FromEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " Order by ToEndDate DESC ";
            DataSet ds = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);

            int Clo_Cnt = Search_Connect.DataSet_ReCount;
            Clo_Cnt = Clo_Cnt + 1;
            */

            string TPDate = "";
            int W_Cnt = 5;
            while (W_Cnt >= 1)
            {
                TPDate = FromEndDate.Substring(0, 4) + '-' + FromEndDate.Substring(4, 2) + '-' + "01";
                DateTime TodayDate = new DateTime();
                TodayDate = DateTime.Parse(TPDate);
                TPDate = TodayDate.AddMonths(-W_Cnt).ToString("yyyy-MM-dd").Replace("-", "");
                
                /*
                StrSql = " Select Top " + Clo_Cnt + " ToEndDate, FromEndDate  ";
                StrSql = StrSql + " From tbl_CloseTotal_02 Se (nolock) ";
                StrSql = StrSql + " WHERE   LEFT(Se.ToEndDate,6)   ='" + TPDate.Substring(0, 6) + "'";
                StrSql = StrSql + " Order by ToEndDate ASC ";
                DataSet ds_T = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds_T);

                int Clo_Cnt_2 = Search_Connect.DataSet_ReCount;
                
                if (Clo_Cnt_2 > 0 && Clo_Cnt_2 >= Clo_Cnt)
                {
                */
                    //string TT_ToEndDate = ds_T.Tables[base_db_name].Rows[Clo_Cnt - 1]["ToEndDate"].ToString();
                    //string TT_FromEndDate = ds_T.Tables[base_db_name].Rows[Clo_Cnt - 1]["FromEndDate"].ToString();


                    StrSql = "Update tbl_ClosePay_02 SET ";
                    if (W_Cnt == 5) StrSql = StrSql + " Allowance3 =  Allowance3 + 833335 ";
                    if (W_Cnt == 4) StrSql = StrSql + " Allowance3 =  Allowance3 + 833333 ";
                    if (W_Cnt == 3) StrSql = StrSql + " Allowance3 =  Allowance3 + 833333 ";
                    if (W_Cnt == 2) StrSql = StrSql + " Allowance3 =  Allowance3 + 833333 ";
                    if (W_Cnt == 1) StrSql = StrSql + " Allowance3 =  Allowance3 + 833333 ";

                    if (W_Cnt == 5) StrSql = StrSql + ", Allowance3_9 =  Allowance3_9 + 833335 ";
                    if (W_Cnt == 4) StrSql = StrSql + ", Allowance3_9 =  Allowance3_9 + 833333 ";
                    if (W_Cnt == 3) StrSql = StrSql + ", Allowance3_9 =  Allowance3_9 + 833333 ";
                    if (W_Cnt == 2) StrSql = StrSql + ", Allowance3_9 =  Allowance3_9 + 833333 ";
                    if (W_Cnt == 1) StrSql = StrSql + ", Allowance3_9 =  Allowance3_9 + 833333 ";
                    StrSql = StrSql + " ,G9_Cnt = G9_Cnt + 1 ";

                    StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
                    StrSql = StrSql + " (";
                    StrSql = StrSql + " Select  Mbid , Mbid2 ";
                    StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
                    StrSql = StrSql + " Where LEFT(GradeDate9, 6) = '" + TPDate.Substring(0,6).ToString() + "' ";

                    //StrSql = StrSql + " Where GradeDate9 >= '" + TT_FromEndDate + "'";
                    //StrSql = StrSql + " And   GradeDate9 <= '" + TT_ToEndDate + "'";
                    StrSql = StrSql + " ) B";
                    StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                    StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);

                    StrSql = "Update tbl_ClosePay_02 SET ";

                    if (W_Cnt == 5) StrSql = StrSql + " Allowance3 =  Allowance3 + 3333335 ";
                    if (W_Cnt == 4) StrSql = StrSql + " Allowance3 =  Allowance3 + 3333333 ";
                    if (W_Cnt == 3) StrSql = StrSql + " Allowance3 =  Allowance3 + 3333333 ";
                    if (W_Cnt == 2) StrSql = StrSql + " Allowance3 =  Allowance3 + 3333333 ";
                    if (W_Cnt == 1) StrSql = StrSql + " Allowance3 =  Allowance3 + 3333333 ";

                    if (W_Cnt == 5) StrSql = StrSql + ", Allowance3_10 =  Allowance3_10 + 3333335 ";
                    if (W_Cnt == 4) StrSql = StrSql + ", Allowance3_10 =  Allowance3_10 + 3333333 ";
                    if (W_Cnt == 3) StrSql = StrSql + ", Allowance3_10 =  Allowance3_10 + 3333333 ";
                    if (W_Cnt == 2) StrSql = StrSql + ", Allowance3_10 =  Allowance3_10 + 3333333 ";
                    if (W_Cnt == 1) StrSql = StrSql + ", Allowance3_10 =  Allowance3_10 + 3333333 ";
                    StrSql = StrSql + " ,G10_Cnt = G10_Cnt + 1 ";

                    StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
                    StrSql = StrSql + " (";
                    StrSql = StrSql + " Select  Mbid , Mbid2 ";
                    StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
                    StrSql = StrSql + " Where LEFT(GradeDate10, 6) = '" + TPDate.Substring(0,6).ToString() + "' ";

                    //StrSql = StrSql + " Where GradeDate10 >= '" + TT_FromEndDate + "'";
                    //StrSql = StrSql + " And   GradeDate10 <= '" + TT_ToEndDate + "'";
                    StrSql = StrSql + " ) B";
                    StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                    StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);

                    StrSql = "Update tbl_ClosePay_02 SET ";

                    if (W_Cnt == 5) StrSql = StrSql + " Allowance3 =  Allowance3 + 8333335 ";
                    if (W_Cnt == 4) StrSql = StrSql + " Allowance3 =  Allowance3 + 8333333 ";
                    if (W_Cnt == 3) StrSql = StrSql + " Allowance3 =  Allowance3 + 8333333 ";
                    if (W_Cnt == 2) StrSql = StrSql + " Allowance3 =  Allowance3 + 8333333 ";
                    if (W_Cnt == 1) StrSql = StrSql + " Allowance3 =  Allowance3 + 8333333 ";

                    if (W_Cnt == 5) StrSql = StrSql + ", Allowance3_11 =  Allowance3_11 + 8333335 ";
                    if (W_Cnt == 4) StrSql = StrSql + ", Allowance3_11 =  Allowance3_11 + 8333333 ";
                    if (W_Cnt == 3) StrSql = StrSql + ", Allowance3_11 =  Allowance3_11 + 8333333 ";
                    if (W_Cnt == 2) StrSql = StrSql + ", Allowance3_11 =  Allowance3_11 + 8333333 ";
                    if (W_Cnt == 1) StrSql = StrSql + ", Allowance3_11 =  Allowance3_11 + 8333333 ";
                    StrSql = StrSql + " ,G11_Cnt = G11_Cnt + 1 ";

                    StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
                    StrSql = StrSql + " (";
                    StrSql = StrSql + " Select  Mbid , Mbid2 ";
                    StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
                    StrSql = StrSql + " Where LEFT(GradeDate11, 6) = '" + TPDate.Substring(0,6).ToString() + "' ";

                    //StrSql = StrSql + " Where GradeDate11 >= '" + TT_FromEndDate + "'";
                    //StrSql = StrSql + " And   GradeDate11 <= '" + TT_ToEndDate + "'";
                    StrSql = StrSql + " ) B";
                    StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                    StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);

                    StrSql = "Update tbl_ClosePay_02 SET ";
                    if (W_Cnt == 5) StrSql = StrSql + " Allowance3 =  Allowance3 + 33333335 ";
                    if (W_Cnt == 4) StrSql = StrSql + " Allowance3 =  Allowance3 + 33333333 ";
                    if (W_Cnt == 3) StrSql = StrSql + " Allowance3 =  Allowance3 + 33333333 ";
                    if (W_Cnt == 2) StrSql = StrSql + " Allowance3 =  Allowance3 + 33333333 ";
                    if (W_Cnt == 1) StrSql = StrSql + " Allowance3 =  Allowance3 + 33333333 ";


                    if (W_Cnt == 5) StrSql = StrSql + ", Allowance3_12 =  Allowance3_12 + 33333335 ";
                    if (W_Cnt == 4) StrSql = StrSql + ", Allowance3_12 =  Allowance3_12 + 33333333 ";
                    if (W_Cnt == 3) StrSql = StrSql + ", Allowance3_12 =  Allowance3_12 + 33333333 ";
                    if (W_Cnt == 2) StrSql = StrSql + ", Allowance3_12 =  Allowance3_12 + 33333333 ";
                    if (W_Cnt == 1) StrSql = StrSql + ", Allowance3_12 =  Allowance3_12 + 33333333 ";
                    StrSql = StrSql + " ,G12_Cnt = G12_Cnt + 1 ";


                    StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";
                    StrSql = StrSql + " (";
                    StrSql = StrSql + " Select  Mbid , Mbid2 ";
                    StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
                    StrSql = StrSql + " Where LEFT(GradeDate12, 6) = '" + TPDate.Substring(0,6).ToString() + "' ";

                    //StrSql = StrSql + " Where GradeDate12 >= '" + TT_FromEndDate + "'";
                    //StrSql = StrSql + " And   GradeDate12 <= '" + TT_ToEndDate + "'";
                    StrSql = StrSql + " ) B";
                    StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                    StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);



                //}


                W_Cnt--;
            }


            //판매원이 아니라고 하면 줫던 수당을 빼앗는다.
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance3 = 0 ";
            StrSql = StrSql + " ,Allowance3_9 =  0 ";
            StrSql = StrSql + " ,Allowance3_10 =  0 ";
            StrSql = StrSql + " ,Allowance3_11 =  0 ";
            StrSql = StrSql + " ,Allowance3_12 =  0 ";            
            StrSql = StrSql + " Where Sell_Mem_TF > 0 ";
                        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);




            //2017-02-17 구하면 바로 원단위절사를 한다.
            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance3 = ROUND(Allowance3, -1,1) ";
            StrSql = StrSql + " Where Allowance3 > 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance3_9 = ROUND(Allowance3_9, -1,1) ";
            StrSql = StrSql + " Where Allowance3_9 > 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance3_10 = ROUND(Allowance3_10, -1,1) ";
            StrSql = StrSql + " Where Allowance3_10 > 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance3_11 = ROUND(Allowance3_11, -1,1) ";
            StrSql = StrSql + " Where Allowance3_11 > 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance3_12 = ROUND(Allowance3_12, -1,1) ";
            StrSql = StrSql + " Where Allowance3_12 > 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //2017-02-17 구하면 바로 원단위절사를 한다.


        }





        private void Give_Allowance1_20150201(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 10;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Cur_PV_2 * 0.1 ";
            StrSql = StrSql + " ,Allowance4 = Cur_PV_1 * 0.01 ";

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Cur_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 -  Cur_PV_2 ";

            StrSql = StrSql + " ,Ded_1 = Cur_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Cur_PV_2 ";

            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   SellPV01 + SellPV02 >= 180 ";
            StrSql = StrSql + "    And   SellPV01 + SellPV02 < 600 ";
            StrSql = StrSql + "    And   Cur_PV_1 > 0 ";
            StrSql = StrSql + "    And   Cur_PV_2 > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Cur_PV_2 * 0.1 ";
            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Cur_PV_2 ";
            StrSql = StrSql + " ,Ded_2 = Cur_PV_2 ";            

            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   SellPV01 + SellPV02 >= 180 ";
            StrSql = StrSql + "    And   SellPV01 + SellPV02 < 600 ";
            StrSql = StrSql + "    And   Cur_PV_1 = 0 ";
            StrSql = StrSql + "    And   Cur_PV_2 > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Cur_PV_1 * 0.1 ";
            StrSql = StrSql + " ,Allowance4 = Cur_PV_2 * 0.01 ";

            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Cur_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Cur_PV_2  ";

            StrSql = StrSql + " ,Ded_1 = Cur_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Cur_PV_2 ";

            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 < Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   SellPV01 + SellPV02 >= 180 ";
            StrSql = StrSql + "    And   SellPV01 + SellPV02 < 600 ";
            StrSql = StrSql + "    And   Cur_PV_2 > 0 ";
            StrSql = StrSql + "    And   Cur_PV_1 > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Cur_PV_1 * 0.1 ";

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Cur_PV_1 ";

            StrSql = StrSql + " ,Ded_1 = Cur_PV_1 ";            

            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 < Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   SellPV01 + SellPV02 >= 180 ";
            StrSql = StrSql + "    And   SellPV01 + SellPV02 < 600 ";
            StrSql = StrSql + "    And   Cur_PV_2 = 0 ";
            StrSql = StrSql + "    And   Cur_PV_1 > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //---------------------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------------------


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Cur_PV_2 * 0.15 ";
            StrSql = StrSql + " ,Allowance4 = Cur_PV_1 * 0.03 ";

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Cur_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Cur_PV_2 ";
            
            StrSql = StrSql + " ,Ded_1 = Cur_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Cur_PV_2 ";

            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   SellPV01 + SellPV02 >= 600 ";
            StrSql = StrSql + "    And   Cur_PV_1 > 0 ";
            StrSql = StrSql + "    And   Cur_PV_2 > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Cur_PV_2 * 0.15 ";
            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Cur_PV_2 ";
            StrSql = StrSql + " ,Ded_2 = Cur_PV_2 ";

            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   SellPV01 + SellPV02 >= 600 ";
            StrSql = StrSql + "    And   Cur_PV_1 = 0 ";
            StrSql = StrSql + "    And   Cur_PV_2 > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Cur_PV_1 * 0.15 ";
            StrSql = StrSql + " ,Allowance4 = Cur_PV_2 * 0.03 ";

            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Cur_PV_2 ";
            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Cur_PV_1 ";

            StrSql = StrSql + " ,Ded_1 = Cur_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Cur_PV_2 ";

            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 < Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   SellPV01 + SellPV02 >= 600 ";
            StrSql = StrSql + "    And   Cur_PV_2 > 0 ";
            StrSql = StrSql + "    And   Cur_PV_1 > 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Cur_PV_1 * 0.15 ";

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Cur_PV_1 ";
            StrSql = StrSql + " ,Ded_1 = Cur_PV_1 ";            

            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 < Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   SellPV01 + SellPV02 >= 600 ";
            StrSql = StrSql + "    And   Cur_PV_2 = 0 ";
            StrSql = StrSql + "    And   Cur_PV_1 > 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //---------------------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------------------


        }


        private void Give_Allowance1_20150216(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            double Cur_PV_1 = 0, Cur_PV_2 = 0, Sum_PV_1 = 0, Sum_PV_2 = 0, Ded_1 = 0, Ded_2 = 0, SSPV = 0 ;
            string Mbid = "";
            double Allowance1 = 0, Allowance4 = 0;
            int Mbid2 = 0, ReqTF2= 0; 


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();


            StrSql = " Select Mbid,Mbid2, Cur_PV_1 , Cur_PV_2 , Sum_PV_1, Sum_PV_2 , SellPV01 + SellPV02 + SellSham01 AS SSPV , ReqTF2  ";
            StrSql = StrSql + " From tbl_ClosePay_02  (nolock) ";
            StrSql = StrSql + " WHERE (Cur_PV_1 > 0 Or Cur_PV_2 > 0 ) ";
            StrSql = StrSql + " And ReqTF2 > 0  ";
                        
            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                ReqTF2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ReqTF2"].ToString());

                if (Mbid2.ToString () == "2585")
                    SSPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SSPV"].ToString());

                Cur_PV_1 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Cur_PV_1"].ToString());
                Cur_PV_2 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Cur_PV_2"].ToString());

                Sum_PV_1 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sum_PV_1"].ToString());
                Sum_PV_2 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sum_PV_2"].ToString());

                SSPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SSPV"].ToString());

                if (Sum_PV_1 < Cur_PV_1)
                    Cur_PV_1 = Sum_PV_1;

                if (Sum_PV_2 < Cur_PV_2)
                    Cur_PV_2 = Sum_PV_2;

                Allowance1 = 0; Allowance4 = 0;
                Ded_1 = 0; Ded_2 = 0;

                if (Sum_PV_1 > 0 && Sum_PV_2 > 0)
                {
                    if (Sum_PV_1 >= Sum_PV_2)  //2라인 소실적인 경우
                    {
                        if (ReqTF2 == 0)
                        {
                            if (Sum_PV_2 > 0)
                            {
                                Allowance1 = Sum_PV_2 * 0.02;
                                Ded_2 = Sum_PV_2;
                                Ded_1 = Sum_PV_2;                                
                                Sum_PV_1 = Sum_PV_1 - Sum_PV_2;
                                Sum_PV_2 = 0;
                            }
                        }
                        else
                        {
                            if (SSPV >= 180 && SSPV < 600)
                            {
                                if (Sum_PV_2 > 0)
                                {
                                    Allowance1 = Sum_PV_2 * 0.1;
                                    Ded_2 = Sum_PV_2;
                                    Ded_1 = Sum_PV_2;                                    
                                    Sum_PV_1 = Sum_PV_1 - Sum_PV_2;
                                    Sum_PV_2 = 0;
                                }

                                if (Allowance1 > 0 && Cur_PV_1 > 0) //대실적 수당이 발생할려면 당마감 소실적 매출이 잇어야 한다.
                                {
                                    if (Cur_PV_1 <= Sum_PV_1)
                                    {
                                        Allowance4 = Cur_PV_1 * 0.01;
                                        Ded_1 = Ded_1 + Cur_PV_1;
                                        Sum_PV_1 = Sum_PV_1 - Cur_PV_1;
                                    }
                                    else
                                    {
                                        Allowance4 = Sum_PV_1 * 0.01;
                                        Ded_1 = Ded_1 + Sum_PV_1;
                                        Sum_PV_1 = 0; 
                                    }
                                }
                            }

                            if (SSPV >= 600)
                            {
                                if (Sum_PV_2 > 0)
                                {
                                    Allowance1 = Sum_PV_2 * 0.15;
                                    Ded_2 = Sum_PV_2;
                                    Ded_1 = Sum_PV_2;                                    
                                    Sum_PV_1 = Sum_PV_1 - Sum_PV_2;
                                    Sum_PV_2 = 0;
                                }
                                if (Allowance1 > 0 && Cur_PV_1 > 0) //대실적 수당이 발생할려면 당마감 소실적 매출이 잇어야 한다.
                                {
                                    if (Cur_PV_1 <= Sum_PV_1)
                                    {
                                        Allowance4 = Cur_PV_1 * 0.03;
                                        Ded_1 = Ded_1 + Cur_PV_1;
                                        Sum_PV_1 = Sum_PV_1 - Cur_PV_1;
                                    }
                                    else
                                    {
                                        Allowance4 = Sum_PV_1 * 0.03;
                                        Ded_1 = Ded_1 + Sum_PV_1;
                                        Sum_PV_1 = 0; 
                                    }
                                }
                            }
                        }
                    }
                    else  //1라인 소실적인 경우
                    {
                        if (ReqTF2 == 0)
                        {
                            if (Sum_PV_1 > 0)
                            {
                                Allowance1 = Sum_PV_1 * 0.02;
                                Ded_1 = Sum_PV_1;
                                Ded_2 = Sum_PV_1;                                
                                Sum_PV_2 = Sum_PV_2 - Sum_PV_1;
                                Sum_PV_1 = 0;
                            }
                        }
                        else
                        {
                            if (SSPV >= 180 && SSPV < 600)
                            {
                                if (Sum_PV_1 > 0)
                                {
                                    Allowance1 = Sum_PV_1 * 0.1;
                                    Ded_1 = Sum_PV_1;
                                    Ded_2 = Sum_PV_1;                                    
                                    Sum_PV_2 = Sum_PV_2 - Sum_PV_1;
                                    Sum_PV_1 = 0;
                                }
                                if (Allowance1 > 0 && Cur_PV_2 > 0) //대실적 수당이 발생할려면 당마감 소실적 매출이 잇어야 한다.
                                {
                                    if (Cur_PV_2 <= Sum_PV_2)
                                    {
                                        Allowance4 = Cur_PV_2 * 0.01;
                                        Ded_2 = Ded_2 + Cur_PV_2;
                                        Sum_PV_2 = Sum_PV_2 - Cur_PV_2;
                                    }
                                    else
                                    {
                                        Allowance4 = Sum_PV_2 * 0.01;
                                        Ded_2 = Ded_2 + Sum_PV_2;
                                        Sum_PV_2 = 0;
                                    }
                                }
                            }

                            if (SSPV >= 600)
                            {
                                if (Sum_PV_1 > 0)
                                {
                                    Allowance1 = Sum_PV_1 * 0.15;
                                    Ded_1 = Sum_PV_1;
                                    Ded_2 = Sum_PV_1;                                    
                                    Sum_PV_2 = Sum_PV_2 - Sum_PV_1;
                                    Sum_PV_1 = 0;
                                }
                                if (Allowance1 > 0 && Cur_PV_2 > 0)  //대실적 수당이 발생할려면 당마감 소실적 매출이 잇어야 한다.
                                {
                                    if (Cur_PV_2 <= Sum_PV_2)
                                    {
                                        Allowance4 = Cur_PV_2 * 0.03;
                                        Ded_2 = Ded_2 +  Cur_PV_2;
                                        Sum_PV_2 = Sum_PV_2 - Cur_PV_2;
                                    }
                                    else
                                    {
                                        Allowance4 = Sum_PV_2 * 0.03;
                                        Ded_2 = Ded_2 + Sum_PV_2;
                                        Sum_PV_2 = 0;
                                    }
                                }
                            }
                        }
                    }
                    
                }

                StrSql = "Update tbl_ClosePay_02 SET ";

                StrSql = StrSql + " Allowance1 =   " + Allowance1;
                StrSql = StrSql + " ,Allowance4 =   " + Allowance4;

                StrSql = StrSql + " ,Ded_1 =   " + Ded_1;
                StrSql = StrSql + " ,Ded_2 =   " + Ded_2;

                StrSql = StrSql + " ,Sum_PV_1 =   " + Sum_PV_1;
                StrSql = StrSql + " ,Sum_PV_2 =   " + Sum_PV_2;

                StrSql = StrSql + " Where   Mbid = '" + Mbid + "'";
                StrSql = StrSql + " And     Mbid2 = " + Mbid2;

                t_qu[t_qu_Cnt] = StrSql;
                t_qu_Cnt++;


                }      // end for          

          


            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }    

        }




        private void Give_Allowance1_Cut_20150201(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            pg1.Value = 0; pg1.Maximum = 10;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02  SET Allowance1 = Allowance1 *  " + Kor_Pay  + " Where Allowance1 > 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02  SET Allowance4 = Allowance4 *  " + Kor_Pay + " Where Allowance4 > 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            //대꺼만 상한 선이 잇으므로4에 대해서 상한선은 넣는다.

            if (int.Parse (FromEndDate) >= 20150303)
            {
                if (int.Parse(FromEndDate) >= 20150310)
                {
                    StrSql = "Update tbl_ClosePay_02 Set";
                    StrSql = StrSql + "  Allowance4_Cut = Allowance4 - 100000 ";
                    StrSql = StrSql + " Where ( Allowance4  ) > 100000 ";
                    StrSql = StrSql + "    And   SellPV01 + SellPV02 + SellSham01>= 180 ";
                    StrSql = StrSql + "    And   SellPV01 + SellPV02 + SellSham01< 600 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();

                    StrSql = "Update tbl_ClosePay_02 Set";
                    StrSql = StrSql + "  Allowance4_Cut = Allowance4 - 200000 ";
                    StrSql = StrSql + " Where ( Allowance4  ) > 200000 ";
                    StrSql = StrSql + "    And   SellPV01 + SellPV02+ SellSham01 >= 600 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();



                    StrSql = "Update tbl_ClosePay_02 Set";
                    StrSql = StrSql + "  Allowance1_1_Cut = Allowance1 - 20000000 ";
                    StrSql = StrSql + " Where ( Allowance1  ) > 20000000 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                }
                else
                {
                    StrSql = "Update tbl_ClosePay_02 Set";
                    StrSql = StrSql + "  Allowance4_Cut = Allowance4 - 200000 ";
                    StrSql = StrSql + " Where ( Allowance4  ) > 200000 ";
                    StrSql = StrSql + "    And   SellPV01 + SellPV02 + SellSham01>= 180 ";
                    StrSql = StrSql + "    And   SellPV01 + SellPV02 + SellSham01< 600 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();

                    StrSql = "Update tbl_ClosePay_02 Set";
                    StrSql = StrSql + "  Allowance4_Cut = Allowance4 - 500000 ";
                    StrSql = StrSql + " Where ( Allowance4  ) > 500000 ";
                    StrSql = StrSql + "    And   SellPV01 + SellPV02+ SellSham01 >= 600 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();



                    StrSql = "Update tbl_ClosePay_02 Set";
                    StrSql = StrSql + "  Allowance1_1_Cut = Allowance1 - 20000000 ";
                    StrSql = StrSql + " Where ( Allowance1  ) > 20000000 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                }
                
            }
            else
            {
                StrSql = "Update tbl_ClosePay_02 Set";
                StrSql = StrSql + "  Allowance4_Cut = Allowance4 - 500000 ";
                StrSql = StrSql + " Where ( Allowance4  ) > 500000 ";
                StrSql = StrSql + "    And   SellPV01 + SellPV02+ SellSham01 >= 180 ";
                StrSql = StrSql + "    And   SellPV01 + SellPV02 + SellSham01< 600 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_02 Set";
                StrSql = StrSql + "  Allowance4_Cut = Allowance4 - 1000000 ";
                StrSql = StrSql + " Where ( Allowance4  ) > 1000000 ";
                StrSql = StrSql + "    And   SellPV01 + SellPV02 + SellSham01 >= 600 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }


            StrSql = " Select Isnull(Sum(TotalPrice),0)  ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";

            StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";

            DataSet ds = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);

            Sum_T_PV_01 = double.Parse(ds.Tables[base_db_name].Rows[0][0].ToString());
            pg1.PerformStep(); pg1.Refresh();

            if (Sum_T_PV_01 > 0)
            {
                StrSql = " Select Isnull(Sum(Allowance1  + Allowance4 - Allowance4_Cut - Allowance1_1_Cut) , 0 )  AS DayPV From tbl_ClosePay_02 ";

                SqlDataReader sr = null;
                Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);

                while (sr.Read())
                {
                    Sum_T_PV_001 = double.Parse(sr.GetValue(0).ToString());
                }
                sr.Close(); sr.Dispose();

                pg1.PerformStep(); pg1.Refresh();

                if (Sum_T_PV_01 * 0.35 < Sum_T_PV_001)
                {
                    double Cut_Pay = Sum_T_PV_001 - (Sum_T_PV_01 * 0.35);

                    StrSql = "Update tbl_ClosePay_02 Set";
                    StrSql = StrSql + "  Allowance1_Cut =  " + Cut_Pay + " * ((Allowance1 + Allowance4 - Allowance4_Cut - Allowance1_1_Cut) /  " + Sum_T_PV_001 + ")";
                    StrSql = StrSql + " Where ( Allowance1  + Allowance4 - Allowance4_Cut - Allowance1_1_Cut ) > 0 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                }
            }
        }


        private void Give_Allowance1_TT(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
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
                                SellDate_2 = sellinfo.SellDate,
                                OrderNumber = sellinfo.OrderNumber,
                                M_Name = sellinfo.M_Name ,
                                Mbid = sellinfo.Mbid ,
                                Mbid2 = sellinfo.Mbid2 ,
                                Saveid = sellinfo.Saveid ,
                                Saveid2 = sellinfo.Saveid2,
                                LineCnt = sellinfo.LineCnt ,
                                Nominid = sellinfo.Nominid ,
                                Nominid2 = sellinfo.Nominid2,
                                N_LineCnt = sellinfo.N_LineCnt 
                            };

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt  = 0  ;
            string TSaveid = "", S_Mbid = "";
            double  Rs_TotalPV = 0, Rs_RePV = 0 ,  Allowance1 = 0  ;
            string StrSql = "";

            
            foreach (var sellinfo in sellinfos )
            {

                LevelCnt = 0;
                TSaveid = sellinfo.Nominid.ToString();
                TSaveid2 = int.Parse(sellinfo.Nominid2.ToString());
                TLine = int.Parse(sellinfo.N_LineCnt.ToString());
                Rs_TotalPV = double.Parse(sellinfo.TotalPV.ToString());
                Rs_RePV = double.Parse(sellinfo.RePV.ToString());

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" &&  Clo_Mem[S_Mbid].CurPoint >= 2 )
                        {
                            Allowance1 = 0; 
                            R_LevelCnt++;

                            if (LevelCnt == 1 && Clo_Mem[S_Mbid].CurPoint == 2 )
                            {
                                Allowance1 = (Rs_TotalPV + Rs_RePV) * 0.1 ;
                            }
                        
                            if (LevelCnt == 1 && Clo_Mem[S_Mbid].CurPoint == 3 )
                            {
                                Allowance1 = (Rs_TotalPV + Rs_RePV) * 0.2;
                            }
                        
                            if( LevelCnt == 2 && Clo_Mem[S_Mbid].CurPoint >= 3 )
                            {
                                Allowance1 = (Rs_TotalPV + Rs_RePV) * 0.1;
                            }

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_02 SET ";
                                StrSql = StrSql + " Allowance1 = Allowance1 +  " + Allowance1  ;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'"  ;
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2  ;

                                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_02";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV , GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + sellinfo.Mbid.ToString() + "'";
                                StrSql = StrSql + "," + int.Parse(sellinfo.Mbid2.ToString()) + ",'" + sellinfo.M_Name.ToString() + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + ", 0 , " + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'1' ,'" + sellinfo.OrderNumber.ToString() + "')";

                                Temp_Connect.Insert_Data(StrSql, Conn, tran);
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

        }



        private void Give_Allowance1BBB(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 7;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            //StrSql = "Select Sum(Se.TotalPv) AS DayPV From tbl_SalesDetail SE (nolock) " ;
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R   (nolock) ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPv  + Bs_R.TotalPv < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";

            //StrSql = StrSql + " WHERE Se.TotalPv + Isnull( Bs_R.TotalPv, 0 ) > 0 ";

            //if (FromEndDate == "20150309")
            //    StrSql = StrSql + " And   Se.SellDate_2  >='20150323'";
            //else
            //    StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
            //StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            //StrSql = StrSql + " And   Se.SellCode <> ''";    
            //StrSql = StrSql + " And   Se.Ga_Order = 0 ";


            StrSql = "Select Sum(Se.TotalPv) AS DayPV From tbl_SalesDetail SE (nolock) ";
            StrSql = StrSql + " Where   Se.SellCode <> ''";
            

            if (FromEndDate == "20150309")
                StrSql = StrSql + " And   Se.SellDate_2  >='20150323'";
            else
                StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";

            DataSet ds = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);

            Sum_T_PV_01 = double.Parse(ds.Tables[base_db_name].Rows[0][0].ToString());
            pg1.PerformStep(); pg1.Refresh();

            int GradeCnt = 0; 
            double Allowance1 = 0 ;


            GradeCnt = 0; Allowance1 = 0;
            StrSql = "Select Isnull(Sum(Cur_Point),0) AS DayPV From tbl_ClosePay_02 ";
            StrSql = StrSql + " Where Cur_Point > 0 ";
            StrSql = StrSql + " And   StopDate = '' ";
            StrSql = StrSql + " And   LeaveDate = '' ";
            StrSql = StrSql + " And   SellPV01 + SellSham01 >= 100 ";
            StrSql = StrSql + " And   Cur_End_35 >= '" + FromEndDate  + "'";
            StrSql = StrSql + " And   Cur_End_35 <> '' "; 
            
            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);

            while (sr.Read())
            {
                GradeCnt = int.Parse(sr.GetValue(0).ToString()) ;                
            }
            sr.Close(); sr.Dispose();

            if  (GradeCnt > 0 )
            {
                Allowance1 = ((Sum_T_PV_01) * 0.04) / GradeCnt;

                Allowance1 = Allowance1 * Kor_Pay ;

                if (double.Parse(txtB2.Text) > 0)
                {
                    Allowance1 = double.Parse(txtB2.Text); 
                }

                StrSql = "Update tbl_ClosePay_02 Set";
                StrSql = StrSql + "  Allowance1 = Cur_Point  * " + Allowance1;
                StrSql = StrSql + "  , Allowance1_P =  " + Allowance1;
                StrSql = StrSql + " Where Cur_Point > 0 ";
                StrSql = StrSql + " And   StopDate = '' ";
                StrSql = StrSql + " And   LeaveDate = '' ";          
                StrSql = StrSql + " And   SellPV01 + SellSham01 >= 100 " ;
                StrSql = StrSql + " And   Cur_End_35 >= '" + FromEndDate + "'";
                StrSql = StrSql + " And   Cur_End_35 <> '' "; 
              
                Temp_Connect.Insert_Data(StrSql, Conn, tran);

                

                
            }

            if (int.Parse(FromEndDate) == 20150607)
            {
                StrSql = "Update tbl_ClosePay_02 Set";
                StrSql = StrSql + "  Allowance1 = 182000  ";
                StrSql = StrSql + "  , Allowance1_P =  " + Allowance1;
                StrSql = StrSql + " Where Mbid2 = 8233270 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            
            pg1.PerformStep(); pg1.Refresh();
            //----------------------------------------------------------------------------------------------------------                                   
        }



        private void Give_Allowance1_Day(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran , string DayC_Date)
        {
            pg1.Value = 0; pg1.Maximum = 7;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            
            StrSql = "Select Isnull(Sum(Se.TotalPv),0)  AS DayPV From tbl_SalesDetail SE (nolock) ";
            StrSql = StrSql + " Where   Se.SellCode <> ''";


            //if (FromEndDate == "20150309")
            //    StrSql = StrSql + " And   Se.SellDate_2  >='20150323'";
            //else
            //    StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate_2  ='" + DayC_Date + "'";

            StrSql = StrSql + " And   Se.Ga_Order = 0 ";

            DataSet ds = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);

            Sum_T_PV_01 = double.Parse(ds.Tables[base_db_name].Rows[0][0].ToString());
            pg1.PerformStep(); pg1.Refresh();

            if (Sum_T_PV_01 == 0)
                return; 

            int GradeCnt = 0;
            double Allowance1 = 0;




            GradeCnt = 0; Allowance1 = 0;
            StrSql = "Select Isnull(Sum(Cur_Point),0) AS DayPV From tbl_ClosePay_02 ";
            StrSql = StrSql + " Where Cur_Point > 0 ";
            StrSql = StrSql + " And   StopDate = '' ";
            StrSql = StrSql + " And   LeaveDate = '' ";
            StrSql = StrSql + " And   SellPV01 + SellSham01 >= 100 ";
            StrSql = StrSql + " And   Cur_End_35 >= '" + DayC_Date + "'";
            StrSql = StrSql + " And   Cur_End_35 <> '' "; 

            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);

            while (sr.Read())
            {
                GradeCnt = int.Parse(sr.GetValue(0).ToString());
            }
            sr.Close(); sr.Dispose();

            if (GradeCnt > 0)
            {
                Allowance1 = ((Sum_T_PV_01) * 0.04) / GradeCnt;

                Allowance1 = Allowance1 * Kor_Pay;

                if (int.Parse(txtB2.Text) > 0)
                {
                    Allowance1 = int.Parse(txtB2.Text);
                }

                StrSql = "Update tbl_ClosePay_02 Set";
                StrSql = StrSql + "   Allowance1 = Allowance1 + ( Cur_Point  * " + Allowance1 + ")"; 
                StrSql = StrSql + "  ,Allowance1_P = Allowance1_P + " + Allowance1;
                StrSql = StrSql + " Where Cur_Point > 0 ";
                StrSql = StrSql + " And   StopDate = '' ";
                StrSql = StrSql + " And   LeaveDate = '' ";
                StrSql = StrSql + " And   SellPV01 + SellSham01 >= 100 ";
                StrSql = StrSql + " And   Cur_End_35 >= '" + DayC_Date + "'";
                StrSql = StrSql + " And   Cur_End_35 <> '' "; 

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            pg1.PerformStep(); pg1.Refresh();
            //----------------------------------------------------------------------------------------------------------                                   
        }









        private void Give_Allowance1_20150201dd(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "";
            double Allowance1 = 0;

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = "  Select  (Allowance1 + Allowance4  - Allowance1_Cut - Allowance4_Cut - Allowance1_1_Cut) AS Allowance1 ,  Nominid , Nominid2 , Mbid , Mbid2 ";
            StrSql = StrSql + " ,N_LineCnt , M_Name, N_LineCnt , LineCnt   ";
            StrSql = StrSql + "  From tbl_ClosePay_02    ";
            StrSql = StrSql + " Where  Allowance1 + Allowance4  - Allowance1_Cut - Allowance4_Cut - Allowance1_1_Cut  > 0 ";

            ReCnt = 0;
            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;


            pg1.Value = 0; pg1.Maximum = ReCnt + 1;
            
            while (sr.Read())
            {
                LevelCnt = 0;
                R_LevelCnt = 0;
               
                TSaveid = sr.GetValue(1).ToString();
                TSaveid2 = int.Parse(sr.GetValue(2).ToString());
                TLine = int.Parse(sr.GetValue(5).ToString());
                Allowance1 = double.Parse(sr.GetValue(0).ToString());

                Mbid = sr.GetValue(3).ToString().ToString();
                Mbid2 = int.Parse(sr.GetValue(4).ToString().ToString());
                M_Name = sr.GetValue(6).ToString();

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].ReqTF2 >= 1 && (Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03  >= 180 || Clo_Mem[S_Mbid].CurGrade >= 10))
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;

                            if (R_LevelCnt == 1)
                                Allowance1 = (Allowance1) * 0.1;

                            if (R_LevelCnt == 2)
                                Allowance1 = (Allowance1) * 0.05;

                            if (R_LevelCnt == 3)
                                Allowance1 = (Allowance1) * 0.05;

                            if (R_LevelCnt == 4)
                                Allowance1 = (Allowance1) * 0.05;

                            
                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_02 SET ";
                                StrSql = StrSql + " Allowance1 = Allowance1 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_02";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV , GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + ", 0 , " + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'2' ,'', " + R_LevelCnt + " )";

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

                    if (R_LevelCnt == 4) TSaveid = "**";

                } //While


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




        private void Give_Allowance1_20150310(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "";
            double Allowance1 = 0;

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = "  Select  (Allowance1 + Allowance4  - Allowance1_Cut - Allowance4_Cut - Allowance1_1_Cut) AS Allowance1 ,  Nominid , Nominid2 , Mbid , Mbid2 ";
            StrSql = StrSql + " ,N_LineCnt , M_Name, N_LineCnt , LineCnt   ";
            StrSql = StrSql + "  From tbl_ClosePay_02    ";
            StrSql = StrSql + " Where  Allowance1 + Allowance4  - Allowance1_Cut - Allowance4_Cut - Allowance1_1_Cut  > 0 ";

            ReCnt = 0;
            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;


            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            while (sr.Read())
            {
                LevelCnt = 0;
                R_LevelCnt = 0;

                TSaveid = sr.GetValue(1).ToString();
                TSaveid2 = int.Parse(sr.GetValue(2).ToString());
                TLine = int.Parse(sr.GetValue(5).ToString());
                Allowance1 = double.Parse(sr.GetValue(0).ToString());

                Mbid = sr.GetValue(3).ToString().ToString();
                Mbid2 = int.Parse(sr.GetValue(4).ToString().ToString());
                M_Name = sr.GetValue(6).ToString();

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].ReqTF2 >= 1
                            && (Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03  >= 180 || Clo_Mem[S_Mbid].CurGrade >= 10)                           
                            )
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;

                            if (R_LevelCnt == 1)
                                Allowance1 = (Allowance1) * 0.1;

                            if (R_LevelCnt == 2)
                                Allowance1 = (Allowance1) * 0.05;

                            if (R_LevelCnt == 3)
                                Allowance1 = (Allowance1) * 0.05;

                            if (R_LevelCnt == 4)
                                Allowance1 = (Allowance1) * 0.05;


                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_02 SET ";
                                StrSql = StrSql + " Allowance1 = Allowance1 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_02";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV , GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + ", 0 , " + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'2' ,'', " + R_LevelCnt + " )";

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

                    if (R_LevelCnt == 4) TSaveid = "**";

                } //While


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


        private void Give_Allowance5_20150201(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "";
            double Allowance1 = 0;

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = "  Select  (Allowance1 + Allowance4  - Allowance1_Cut - Allowance4_Cut - Allowance1_1_Cut) AS Allowance1 ,  Saveid , Saveid2 , Mbid , Mbid2 ";
            StrSql = StrSql + " ,LineCnt , M_Name, N_LineCnt , LineCnt   ";
            StrSql = StrSql + "  From tbl_ClosePay_02    ";
            StrSql = StrSql + " Where  Allowance1 + Allowance4  - Allowance1_Cut - Allowance4_Cut - Allowance1_1_Cut > 0 ";

            ReCnt = 0;
            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;


            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            while (sr.Read())
            {
                LevelCnt = 0;
                R_LevelCnt = 0;

                TSaveid = sr.GetValue(1).ToString();
                TSaveid2 = int.Parse(sr.GetValue(2).ToString());
                TLine = int.Parse(sr.GetValue(5).ToString());
                Allowance1 = double.Parse(sr.GetValue(0).ToString());

                Mbid = sr.GetValue(3).ToString().ToString();
                Mbid2 = int.Parse(sr.GetValue(4).ToString().ToString());
                M_Name = sr.GetValue(6).ToString();

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].ReqTF2 >= 1 && (Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03  >= 180 || Clo_Mem[S_Mbid].CurGrade >= 10))
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;

                            if (R_LevelCnt == 1)
                                Allowance1 = (Allowance1) * 0.1;

                            if (R_LevelCnt == 2)
                                Allowance1 = (Allowance1) * 0.05;

                            if (R_LevelCnt == 3)
                                Allowance1 = (Allowance1) * 0.05;

                            if (R_LevelCnt == 4)
                                Allowance1 = (Allowance1) * 0.05;


                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_02 SET ";
                                StrSql = StrSql + " Allowance5 = Allowance5 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_02";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV , GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + ", 0 , " + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'5' ,'', " + R_LevelCnt + " )";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }

                        }

                        TSaveid = Clo_Mem[S_Mbid].Saveid ; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (R_LevelCnt == 4) TSaveid = "**";

                } //While


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




        private void Give_Allowance5_20150310(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "";
            double Allowance1 = 0;

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = "  Select  (Allowance1 + Allowance4  - Allowance1_Cut - Allowance4_Cut - Allowance1_1_Cut) AS Allowance1 ,  Saveid , Saveid2 , Mbid , Mbid2 ";
            StrSql = StrSql + " ,LineCnt , M_Name, N_LineCnt , LineCnt   ";
            StrSql = StrSql + "  From tbl_ClosePay_02    ";
            StrSql = StrSql + " Where  Allowance1 + Allowance4  - Allowance1_Cut - Allowance4_Cut - Allowance1_1_Cut > 0 ";

            ReCnt = 0;
            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;


            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            while (sr.Read())
            {
                LevelCnt = 0;
                R_LevelCnt = 0;

                TSaveid = sr.GetValue(1).ToString();
                TSaveid2 = int.Parse(sr.GetValue(2).ToString());
                TLine = int.Parse(sr.GetValue(5).ToString());
                Allowance1 = double.Parse(sr.GetValue(0).ToString());

                Mbid = sr.GetValue(3).ToString().ToString();
                Mbid2 = int.Parse(sr.GetValue(4).ToString().ToString());
                M_Name = sr.GetValue(6).ToString();

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].ReqTF2 >= 1
                            && (Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03  >= 180 || Clo_Mem[S_Mbid].CurGrade >= 10)                            
                            )
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;

                            if (R_LevelCnt == 1)
                                Allowance1 = (Allowance1) * 0.1;

                            if (R_LevelCnt == 2)
                                Allowance1 = (Allowance1) * 0.05;

                            if (R_LevelCnt == 3)
                                Allowance1 = (Allowance1) * 0.05;

                            if (R_LevelCnt == 4)
                                Allowance1 = (Allowance1) * 0.05;


                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_02 SET ";
                                StrSql = StrSql + " Allowance5 = Allowance5 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_02";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV , GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, R_LevelCnt) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + ", 0 , " + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'5' ,'', " + R_LevelCnt + " )";

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;
                            }

                        }

                        TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt;

                        S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                    }
                    else
                    {
                        TSaveid = "**";
                    }

                    if (R_LevelCnt == 4) TSaveid = "**";

                } //While


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





        private void Give_Allowance1_TTdd(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 15    ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            //////테스트용임 다 지워야함. 아래 주석을 열어죽소
            ////StrSql = "Update tbl_ClosePay_02 SET ";
            ////StrSql = StrSql + " Allowance1 = Sum_PV_2 * 0.2 ";

            ////StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            ////StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            ////StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            ////StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";
                        
            ////StrSql = StrSql + " Where Sum_PV_1 >= Sum_PV_2 ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update tbl_ClosePay_02 SET ";
            ////StrSql = StrSql + " Allowance1 = Sum_PV_1 * 0.2 ";

            ////StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            ////StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            ////StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            ////StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";
            ////StrSql = StrSql + " Where Sum_PV_1 < Sum_PV_2 ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();





            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Sum_PV_2 * 0.2 ";

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            StrSql = StrSql + " Where (CurPoint = 2 OR CurGrade >= 2 ) ";
            StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Sum_PV_1 * 0.2 ";

            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            StrSql = StrSql + " Where (CurPoint = 2 OR CurGrade >= 2 ) ";
            StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Sum_PV_2 * 0.1";

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            StrSql = StrSql + " Where (CurPoint = 1 ) ";
            StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Sum_PV_1 * 0.1 ";

            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            StrSql = StrSql + " Where (CurPoint = 1 ) ";
            StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        
    
    
    
            StrSql = "Update tbl_ClosePay_02 SET "  ;
            StrSql = StrSql + " Allowance1_Cut = Allowance1 - 500000 "  ;
            StrSql = StrSql + " Where Allowance1 > 500000 "  ;
            StrSql = StrSql + " And CurGrade < 2 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
             StrSql = "Update tbl_ClosePay_02 SET "  ;
            StrSql = StrSql + " Allowance1_Cut = Allowance1 - 1000000 "  ;
            StrSql = StrSql + " Where Allowance1 > 1000000 "  ;
            StrSql = StrSql + " And CurGrade = 2 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
             StrSql = "Update tbl_ClosePay_02 SET "  ;
            StrSql = StrSql + " Allowance1_Cut = Allowance1 - 2500000 "  ;
            StrSql = StrSql + " Where Allowance1 > 2500000 "  ;
            StrSql = StrSql + " And CurGrade = 3 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
             StrSql = "Update tbl_ClosePay_02 SET "  ;
            StrSql = StrSql + " Allowance1_Cut = Allowance1 - 5000000 "  ;
            StrSql = StrSql + " Where Allowance1 > 5000000 "  ;  ;
            StrSql = StrSql + " And CurGrade = 4 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
      
             StrSql = "Update tbl_ClosePay_02 SET "  ;
            StrSql = StrSql + " Allowance1_Cut = Allowance1 - 10000000 "  ;
            StrSql = StrSql + " Where Allowance1 > 10000000 "  ;
            StrSql = StrSql + " And CurGrade = 5 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
    
              StrSql = "Update tbl_ClosePay_02 SET "  ;
            StrSql = StrSql + " Allowance1_Cut = Allowance1 - 15000000 "  ;
            StrSql = StrSql + " Where Allowance1 > 15000000 "  ;
            StrSql = StrSql + " And CurGrade = 6 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
      
              StrSql = "Update tbl_ClosePay_02 SET "  ;
            StrSql = StrSql + " Allowance1_Cut = Allowance1 - 25000000 "  ;
            StrSql = StrSql + " Where Allowance1 > 25000000 "  ;
            StrSql = StrSql + " And CurGrade = 7 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
      
              StrSql = "Update tbl_ClosePay_02 SET "  ;
            StrSql = StrSql + " Allowance1_Cut = Allowance1 - 30000000 "  ;
            StrSql = StrSql + " Where Allowance1 > 30000000 "  ;
            StrSql = StrSql + " And CurGrade = 8 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
              StrSql = "Update tbl_ClosePay_02 SET "  ;
            StrSql = StrSql + " Allowance1_Cut = Allowance1 - 50000000 "  ;
            StrSql = StrSql + " Where Allowance1 > 50000000 "  ;
            StrSql = StrSql + " And CurGrade = 9 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }



        private void Give_Allowance1_TEST(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0, Big_line = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "";
            double Allowance1 = 0,  Allowance3 = 0, Allowance4 = 0, Allowance5 = 0;
            double Allowance6 = 0, Allowance7 = 0, Allowance8 = 0, Allowance9 = 0, Allowance10 = 0;
            double Allowance11 = 0, Allowance12 = 0, Allowance13 = 0, Allowance14 = 0, Allowance15 = 0;
            double Sum_PV_1 = 0, Sum_PV_2 = 0, Ded_1 = 0, Ded_2 = 0 ;
            
            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = "  Select  Allowance1 ,  Nominid , Nominid2 , Mbid , Mbid2  ";
            StrSql = StrSql + " ,N_LineCnt , M_Name, N_LineCnt , LineCnt   , Sum_PV_1, Sum_PV_2   ";
            StrSql = StrSql + "  From tbl_ClosePay_02    ";
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
                Allowance1 = double.Parse(sr.GetValue(0).ToString());

                Mbid = sr.GetValue(3).ToString().ToString();
                Mbid2 = int.Parse(sr.GetValue(4).ToString().ToString());
                M_Name = sr.GetValue(6).ToString();

                Sum_PV_1 = double.Parse(sr.GetValue(9).ToString());
                Sum_PV_2 = double.Parse(sr.GetValue(10).ToString());
                Ded_1 = 0; Ded_2 = 0;

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                Allowance1 = 0; Allowance1 = 0; Allowance3 = 0; Allowance4 = 0; Allowance5 = 0;
                Allowance6 = 0; Allowance7 = 0; Allowance8 = 0; Allowance9 = 0; Allowance10 = 0;
                Allowance11 = 0; Allowance12 = 0; Allowance13 = 0; Allowance14 = 0; Allowance15 = 0;

                if (Sum_PV_1 > Sum_PV_2)
                {
                    Allowance1 = (Sum_PV_2) * 0.1;

                    if (Sum_PV_1 >= (Sum_PV_2 * 2))
                    {
                        Allowance1 = (Sum_PV_2 * 2) * 0.05;
                        Allowance6 = (Sum_PV_2 * 2) * 0.1;
                        Allowance10 = (Sum_PV_2 * 2) * 0.15;
                    }
                    else
                    {
                        Allowance1 = Sum_PV_1 * 0.05;
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
                        Allowance1 = (Sum_PV_1 * 2) * 0.05;
                        Allowance6 = (Sum_PV_1 * 2) * 0.1;
                        Allowance10 = (Sum_PV_1 * 2) * 0.15;
                    }
                    else
                    {
                        Allowance1 = Sum_PV_2 * 0.05;
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


                 


                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + " Allowance1 = Allowance1 +  " + Allowance1;
                StrSql = StrSql + " ,Allowance1 = Allowance1 +  " + Allowance1;
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











        private void Give_Allowance1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            //pg1.Value = 0; pg1.Maximum = 4;
            //pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            
            //double Allowance1 = 0, Allowance3 = 0, Allowance4 = 0, Allowance5 = 0;            
            //double Sum_PV_1 = 0, Sum_PV_2 = 0, Ded_1 = 0, Ded_2 = 0, Base_CV = 0 ;
            //int A1_Point = 0;

            //int t_qu_Cnt = 0;            
            //Dictionary<int, string> t_qu = new Dictionary<int, string>();

            //StrSql = "  Select  Allowance1 ,  Nominid , Nominid2 , Mbid , Mbid2  ";
            //StrSql = StrSql + " ,N_LineCnt , M_Name, N_LineCnt , LineCnt   , Sum_PV_1, Sum_PV_2   ";
            //StrSql = StrSql + "  From tbl_ClosePay_02 (nolock)    ";
            //StrSql = StrSql + " Where  Sum_PV_1  >= 500 ";
            //StrSql = StrSql + " And    Sum_PV_2  >= 500 ";
            //StrSql = StrSql + " And    Sell_Mem_TF   = 1 ";
            //StrSql = StrSql + " And    ReqTF1    = 1 ";

            //DataSet ds = new DataSet();
            //ReCnt = 0;
            //Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            //ReCnt = Search_Connect.DataSet_ReCount;

            //pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            //{
            //    string Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
            //    int Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());

            //    Ded_1 = 0; Ded_2 = 0;
            //    Sum_PV_1 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sum_PV_1"].ToString());
            //    Sum_PV_2 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sum_PV_2"].ToString());

            //    A1_Point = 0;

            //    if (Sum_PV_1 >= Sum_PV_2) Base_CV = Sum_PV_2;
            //    if (Sum_PV_1 < Sum_PV_2) Base_CV = Sum_PV_1;
                



            //    if (A1_Point > 0)
            //    {
            //        StrSql = "Update tbl_ClosePay_02 SET ";
            //        StrSql = StrSql + " Sum_PV_1 =   " + Sum_PV_1;
            //        StrSql = StrSql + " ,Sum_PV_2 =  " + Sum_PV_2;
            //        StrSql = StrSql + " ,Ded_1 =   " + Ded_1;
            //        StrSql = StrSql + " ,Ded_2 =   " + Ded_2;
            //        StrSql = StrSql + " ,A1_Point =   " + A1_Point;
            //        StrSql = StrSql + " Where   Mbid = '" + Mbid + "'";
            //        StrSql = StrSql + " And     Mbid2 = " + Mbid2;
                    
            //        t_qu[t_qu_Cnt] = StrSql;
            //        t_qu_Cnt++;
            //    }
            //}


            //pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            //foreach (int tkey in t_qu.Keys)
            //{
            //    StrSql = t_qu[tkey];
            //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //    pg1.PerformStep(); pg1.Refresh();
            //}



            pg1.Value = 0; pg1.Maximum = 20;
            pg1.PerformStep(); pg1.Refresh();

            
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  A1_Point = ROUND(Sum_PV_2 / 500,0,1)  ";
            StrSql = StrSql + " Where (Sell_Mem_TF = 0 ) ";
            StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql + " And Sum_PV_1 >= 500  ";
            StrSql = StrSql + " And Sum_PV_2 >= 500  ";            
            StrSql = StrSql + " And ReqTF1    = 1 ";
            StrSql = StrSql + " And ROUND(Sum_PV_2 / 500,0,1) <= 400 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  A1_Point = 400  ";
            StrSql = StrSql + " Where (Sell_Mem_TF = 0 ) ";
            StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql + " And Sum_PV_1 >= 500  ";
            StrSql = StrSql + " And Sum_PV_2 >= 500  ";
            StrSql = StrSql + " And ReqTF1    = 1 ";
            StrSql = StrSql + " And ROUND(Sum_PV_2 / 500,0,1) > 400 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //-------------------------------------------------------------------------------------------------------------------



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  A1_Point = ROUND(Sum_PV_1 / 500,0,1)  ";
            StrSql = StrSql + " Where (Sell_Mem_TF = 0 ) ";
            StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";
            StrSql = StrSql + " And Sum_PV_1 >= 500  ";
            StrSql = StrSql + " And Sum_PV_2 >= 500  ";
            StrSql = StrSql + " And ReqTF1    = 1 ";
            StrSql = StrSql + " And ROUND(Sum_PV_1 / 500,0,1) <= 400 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  A1_Point = 400  ";
            StrSql = StrSql + " Where (Sell_Mem_TF = 0 ) ";
            StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";
            StrSql = StrSql + " And Sum_PV_1 >= 500  ";
            StrSql = StrSql + " And Sum_PV_2 >= 500  ";
            StrSql = StrSql + " And ReqTF1    = 1 ";
            StrSql = StrSql + " And ROUND(Sum_PV_1 / 500,0,1) > 400 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //-------------------------------------------------------------------------------------------------------------------



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Ded_1 = 500 * A1_Point  ";
            StrSql = StrSql + " , Ded_2 = 500 * A1_Point  ";
            StrSql = StrSql + ",  Sum_PV_1 = Sum_PV_1 - (500 * A1_Point )  ";
            StrSql = StrSql + " , Sum_PV_2 = Sum_PV_2  - (500 * A1_Point) ";
            StrSql = StrSql + " Where A1_Point > 0  ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


          
            
            
            int GradeCnt = 0; 
            double Allowance1 = 0;
            double t_Per = 0; 
            int C_Grade = 0; 

            for(int i = 0 ; i <= 12  ; i++)
            {
                Allowance1 = 0;
                GradeCnt = 0;
                C_Grade = i * 10;
                if (i == 0) t_Per = 0.05;
                if (i >= 1 && i<= 4) t_Per = 0.15;
                if (i >= 5 && i <= 8) t_Per = 0.12;
                if (i >= 9 ) t_Per = 0.1;

                SqlDataReader sr = null;
                
                StrSql = "Select Isnull(Sum(A1_Point),0) AS DayPV From tbl_ClosePay_02 (nolock) ";
                StrSql = StrSql + " Where A1_Point > 0 ";
                if (i >= 1)
                    StrSql = StrSql + " And   OneGrade =  " + C_Grade;
                else
                {
                    StrSql = StrSql + " And   ReqTF1  =1 ";
                    StrSql = StrSql + " And   OneGrade  < 10  ";
                }
                StrSql = StrSql + " And   LeaveDate = '' ";
                StrSql = StrSql + " And   Sell_Mem_TF = 0 ";
                            
                Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);

                while (sr.Read())
                {
                    GradeCnt = int.Parse(sr.GetValue(0).ToString());
                }
                sr.Close(); sr.Dispose(); 

                if (GradeCnt > 0)
                {
                    Allowance1 = GradeCnt * 500 * t_Per ; 
                    Allowance1 = Allowance1 - (Allowance1 % 10); 

                    StrSql = "Update tbl_ClosePay_02 Set";
                    StrSql = StrSql + "  Allowance1 = A1_Point * " + Allowance1;
                    if (i >= 1)
                        StrSql = StrSql + " Where OneGrade =   " + C_Grade;
                    else
                    {
                        StrSql = StrSql + " Where ReqTF1 = 1  ";
                        StrSql = StrSql + " And   OneGrade  < 10  ";
                    }
                    StrSql = StrSql + " And   StopDate = '' ";
                    StrSql = StrSql + " And   LeaveDate = '' ";
                    StrSql = StrSql + " And   Sell_Mem_TF = 0 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                }
                pg1.PerformStep(); pg1.Refresh();
            }
            //----------------------------------------------------------------------------------------------------------







            

            ////2015-12-16일 이홍민 본부장님 요청에 의해서  최고 직급 PD 이상으로 변경한다. 유지직급이 아니고 PD가 
            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + "  Allowance1 = Sum_PV_2 * 0.1 ";

            //StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            //StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            //StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            //StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            //StrSql = StrSql + " Where (CurGrade >= 100 ) ";
            //StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";

            //StrSql = StrSql + " And Sum_PV_1 > 0  ";
            //StrSql = StrSql + " And Sum_PV_2 > 0  ";
            //StrSql = StrSql + " And Sell_Mem_TF = 0  ";
             


            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Allowance1 = Sum_PV_1 * 0.1 ";

            //StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            //StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            //StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            //StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            //StrSql = StrSql + " Where (CurGrade >= 100 ) ";
            //StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            //StrSql = StrSql + " And Sum_PV_1 > 0  ";
            //StrSql = StrSql + " And Sum_PV_2 > 0  ";
            //StrSql = StrSql + " And Sell_Mem_TF = 0  ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
            ////''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ////''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''




            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + "  Allowance1 = Sum_PV_2 * 0.12 ";

            //StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            //StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            //StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            //StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            //StrSql = StrSql + " Where (OneGrade >= 60 ) ";
            //StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";
            //StrSql = StrSql + " And Sum_PV_1 > 0  ";
            //StrSql = StrSql + " And Sum_PV_2 > 0  ";
            //StrSql = StrSql + " And Sell_Mem_TF = 0  ";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Allowance1 = Sum_PV_1 * 0.12 ";

            //StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            //StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            //StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            //StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            //StrSql = StrSql + " Where (OneGrade >= 60 ) ";
            //StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            //StrSql = StrSql + " And Sum_PV_1 > 0  ";
            //StrSql = StrSql + " And Sum_PV_2 > 0  ";
            //StrSql = StrSql + " And Sell_Mem_TF = 0  ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
            ////''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ////''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''




            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + "  Allowance1 = Sum_PV_2 * 0.15 ";            

            //StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            //StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            //StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            //StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            //StrSql = StrSql + " Where (OneGrade >= 20 ) ";
            //StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";

            //StrSql = StrSql + " And Sum_PV_1 > 0  ";
            //StrSql = StrSql + " And Sum_PV_2 > 0  ";
            //StrSql = StrSql + " And Sell_Mem_TF = 0  ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Allowance1 = Sum_PV_1 * 0.15 ";
            
            //StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            //StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            //StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            //StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            //StrSql = StrSql + " Where (OneGrade >= 20 ) ";
            //StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            //StrSql = StrSql + " And Sum_PV_1 > 0  ";
            //StrSql = StrSql + " And Sum_PV_2 > 0  ";
            //StrSql = StrSql + " And Sell_Mem_TF = 0  ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
            ////''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ////''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + "  Allowance1 = Sum_PV_2 * 0.05 ";
            
            //StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            //StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            //StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            //StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            //StrSql = StrSql + " Where (OneGrade = 10 ) ";
            //StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";

            //StrSql = StrSql + " And Sum_PV_1 > 0  ";
            //StrSql = StrSql + " And Sum_PV_2 > 0  ";
            //StrSql = StrSql + " And Sell_Mem_TF = 0  ";


            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Allowance1 = Sum_PV_1 * 0.05 ";
            
            //StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            //StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            //StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            //StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            //StrSql = StrSql + " Where (OneGrade = 10 ) ";
            //StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            //StrSql = StrSql + " And Sum_PV_1 > 0  ";
            //StrSql = StrSql + " And Sum_PV_2 > 0  ";   
            //StrSql = StrSql + " And Sell_Mem_TF = 0  ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
            ////''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ////''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            ////소수점 이사는 절삭을 해버린다.
            //StrSql = " Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Allowance1 = ROUND(Allowance1, 0,1) ";
            //StrSql = StrSql + " Where Allowance1 > 0 ";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);

            
            
            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Max_Pay =  3000000 ";            
            //StrSql = StrSql + " Where OneGrade  = 30";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Max_Pay =  5000000 ";
            //StrSql = StrSql + " Where OneGrade  = 40";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Max_Pay =  10000000 ";
            //StrSql = StrSql + " Where OneGrade  = 50";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            ////후원보너스 자체가 최고 직급으로 PD 이상 따지를 걸로 변경 처리 되엇기 때문에... 한도도 역시 그것에 맞춰서 변경 처리함.
            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Max_Pay =  20000000 ";
            ////StrSql = StrSql + " Where OneGrade  >= 60";
            //StrSql = StrSql + " Where CurGrade  >= 60";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            ////2016-10-07 요청에 의해서 아래로 내림 매출 대시 절사한 다음에... 회원별 주간 맥스로 변경 처리함.
            ////StrSql = "Update tbl_ClosePay_02 SET ";
            ////StrSql = StrSql + " Allowance1_Cut =  Allowance1 - Max_Pay ";
            ////StrSql = StrSql + " ,Allowance1 = Max_Pay ";
            ////StrSql = StrSql + " Where Allowance1 > Max_Pay";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update tbl_ClosePay_02 SET ";
            ////StrSql = StrSql + " Fresh_1 = Sum_PV_1 ";
            ////StrSql = StrSql + " ,Sum_PV_1 = 0 ";          
            ////StrSql = StrSql + " Where Sum_PV_1 > 0  ";
            ////StrSql = StrSql + " And ReqTF2 = 0  ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update tbl_ClosePay_02 SET ";
            ////StrSql = StrSql + " Fresh_2 = Sum_PV_2 ";
            ////StrSql = StrSql + " ,Sum_PV_2 = 0 ";
            ////StrSql = StrSql + " Where Sum_PV_2 > 0  ";
            ////StrSql = StrSql + " And ReqTF2 = 0  ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();
            
            ///*2016-06-08 입력한 %에 의해 후원보너스 초과유무 확인 및 금액 변경*/

            //if (int.Parse(FromEndDate) >= int.Parse("20160601"))
            //{
            //    double TotalPrice = 0, TotalAllowance1 = 0, Cut_Per2 = 0, S_CutPay2 = 0;

                
            //    StrSql = " Select SUM(TotalPrice) from tbl_SalesDetail (nolock) ";
            //    StrSql = StrSql + " Where Ga_Order = 0 And SellCode <> '' ";
            //    StrSql = StrSql + " And SellDate_2 between '" + FromEndDate + "' and '" + ToEndDate + "' ";

            //    DataSet ds = new DataSet();
            //    ReCnt = 0;
            //    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            //    ReCnt = Search_Connect.DataSet_ReCount;

            //    if (ReCnt > 0)
            //    {
            //        TotalPrice = double.Parse(ds.Tables[base_db_name].Rows[0][0].ToString());
            //    }

            //    StrSql = " Select ISNULL(SUM(Allowance1), 0) From tbl_ClosePay_02 (nolock) Where Allowance1 > 0 ";
            //    DataSet ds2 = new DataSet();
            //    int ReCnt2 = 0;
            //    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds2);
            //    ReCnt2 = Search_Connect.DataSet_ReCount;

            //    if (ReCnt2 > 0)
            //    {
            //        TotalAllowance1 = double.Parse(ds2.Tables[base_db_name].Rows[0][0].ToString());
            //    }

            //    if (TotalAllowance1 > 0)
            //    {
            //        if (TotalAllowance1 / TotalPrice * 100 > Kor_Pay)
            //        {
            //            Cut_Per2 = ((TotalAllowance1 / TotalPrice) * 100) - Kor_Pay;
            //            S_CutPay2 = TotalPrice * (Cut_Per2 / 100);

            //            StrSql = "Update tbl_ClosePay_02 Set ";
            //            StrSql = StrSql + "  Allowance1_Cut_2 = Convert( FLOAT," + S_CutPay2 + ") * (Allowance1 / Convert(FLOAT," + TotalAllowance1 + "))";
            //            StrSql = StrSql + " Where Allowance1  > 0 ";
            //            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            //            StrSql = "Update tbl_ClosePay_02 Set ";
            //            StrSql = StrSql + " Allowance1= Allowance1 - Allowance1_Cut_2 ";
            //            StrSql = StrSql + " Where Allowance1  > 0 And Allowance1_Cut_2 >0 ";
            //            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //        }
            //    }
            //}

            ////2016-10-07 요청에 의해서 아래로 내려옴... 매출 대비 공제 처리하고 본인 주간 공제로..
            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Allowance1_Cut =  Allowance1 - Max_Pay ";
            //StrSql = StrSql + " ,Allowance1 = Max_Pay ";
            //StrSql = StrSql + " Where Allowance1 > Max_Pay";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            ////2017-02-17 계산처리하고 원단위절사로 변경햇음.
            ////2016-04-21 소수점이하 절삭
            //StrSql = " Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Allowance1 = ROUND(Allowance1, -1,1) ";
            //StrSql = StrSql + " Where Allowance1 > 0 ";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);

            
            
        }




        private void Give_Allowance1_Real(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            //pg1.Value = 0; pg1.Maximum = 4;
            //pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";





            pg1.Value = 0; pg1.Maximum = 20;
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  A1_Point = ROUND(Sum_PV_2 / 500,0,1)  ";
            //StrSql = StrSql + " Where (Sell_Mem_TF = 0 ) ";
            StrSql = StrSql + " Where Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql + " And Sum_PV_1 >= 500  ";
            StrSql = StrSql + " And Sum_PV_2 >= 500  ";
            StrSql = StrSql + " And ReqTF1    = 1 ";
            StrSql = StrSql + " And ROUND(Sum_PV_2 / 500,0,1) <= 400 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  A1_Point = 400  ";
            //StrSql = StrSql + " Where (Sell_Mem_TF = 0 ) ";
            StrSql = StrSql + " Where Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql + " And Sum_PV_1 >= 500  ";
            StrSql = StrSql + " And Sum_PV_2 >= 500  ";
            StrSql = StrSql + " And ReqTF1    = 1 ";
            StrSql = StrSql + " And ROUND(Sum_PV_2 / 500,0,1) > 400 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //-------------------------------------------------------------------------------------------------------------------



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  A1_Point = ROUND(Sum_PV_1 / 500,0,1)  ";
            //StrSql = StrSql + " Where (Sell_Mem_TF = 0 ) ";
            StrSql = StrSql + " Where Sum_PV_1 < Sum_PV_2 ";
            StrSql = StrSql + " And Sum_PV_1 >= 500  ";
            StrSql = StrSql + " And Sum_PV_2 >= 500  ";
            StrSql = StrSql + " And ReqTF1    = 1 ";
            StrSql = StrSql + " And ROUND(Sum_PV_1 / 500,0,1) <= 400 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  A1_Point = 400  ";
            //StrSql = StrSql + " Where (Sell_Mem_TF = 0 ) ";
            StrSql = StrSql + " Where Sum_PV_1 < Sum_PV_2 ";
            StrSql = StrSql + " And Sum_PV_1 >= 500  ";
            StrSql = StrSql + " And Sum_PV_2 >= 500  ";
            StrSql = StrSql + " And ReqTF1    = 1 ";
            StrSql = StrSql + " And ROUND(Sum_PV_1 / 500,0,1) > 400 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //-------------------------------------------------------------------------------------------------------------------



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  Ded_1 = 500 * A1_Point  ";
            StrSql = StrSql + " , Ded_2 = 500 * A1_Point  ";
            StrSql = StrSql + ",  Sum_PV_1 = Sum_PV_1 - (500 * A1_Point )  ";
            StrSql = StrSql + " , Sum_PV_2 = Sum_PV_2  - (500 * A1_Point) ";
            StrSql = StrSql + " Where A1_Point > 0  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();





            int GradeCnt = 0;
            double Allowance1 = 0;
            double t_Per = 0;
            int C_Grade = 0;

            for (int i = 1; i <= 12; i++)
            {
                Allowance1 = 0;
                GradeCnt = 0;
                C_Grade = i * 10;

                //if (i == 0) t_Per = 0.05; //사라짐 BP는 없어짐 수당 시작도 하기전 변경사하에서 
                if (i >= 1 && i <= 4) t_Per = 0.2;
                if (i >= 5 && i <= 8) t_Per = 0.15;
                if (i >= 9) t_Per = 0.1;

              

                StrSql = "Update tbl_ClosePay_02 Set";
                StrSql = StrSql + "  Allowance1 = A1_Point * 500  * " + t_Per;
                StrSql = StrSql + "  ,Allowance2_T_Per = '" + (t_Per * 100).ToString() +"'";                
                StrSql = StrSql + " Where OneGrade =   " + C_Grade;                
                StrSql = StrSql + " And   StopDate = '' ";
                StrSql = StrSql + " And   LeaveDate = '' ";
                StrSql = StrSql + " And   Sell_Mem_TF = 0 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
           
                pg1.PerformStep(); pg1.Refresh();
            }
            //----------------------------------------------------------------------------------------------------------


            //소비자도 우선은 발생을 시키고 아래서 빼앗어 버린다.
            StrSql = "Update tbl_ClosePay_02 Set";
            StrSql = StrSql + "  Allowance1 = (A1_Point * 500)  * 0.2 "; 
            StrSql = StrSql + "  ,Allowance2_T_Per = '20'";
            StrSql = StrSql + " Where Sell_Mem_TF = 1  "; 
            StrSql = StrSql + " And   StopDate = '' ";
            StrSql = StrSql + " And   LeaveDate = '' ";           

            Temp_Connect.Insert_Data(StrSql, Conn, tran);



            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Allowance1 * 1000  ";
            StrSql = StrSql + " Where Allowance1 > 0 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);    


            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1 = Round((Allowance1 /10),0,1) * 10 ";
            StrSql = StrSql + " Where Allowance1 > 0 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran); 


            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1_Cut_S = Allowance1  ";
            StrSql = StrSql + " ,Allowance1 = 0  "; 
            StrSql = StrSql + " Where Allowance1 > 0 ";
            StrSql = StrSql + " And  Sell_Mem_TF = 1  "; 
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran); 
            
            

        }





        private void Give_Allowance2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double Allowance2 = 0, Allowance1 = 0, R_TotalPV = 0, TotalPV = 0, TotalPV_2 = 0, TH_TotalPV = 0, KR_TotalPV = 0, GivePay = 0 ;
            int L_1 = 0, L_2 = 0, GiveGrade = 0 ;
            string TPer = "";

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            StrSql = " Select Allowance1, Mbid,Mbid2 ,M_Name  ";
            StrSql = StrSql + " From tbl_ClosePay_02 Se (nolock) ";            
            StrSql = StrSql + " WHERE Se.Allowance1  > 0 ";
            StrSql = StrSql + " And OneGrade >= 30 "; 
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
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();
                                
                S_Mbid = Mbid + "-" + Mbid2.ToString();

                if (Clo_Mem.ContainsKey(S_Mbid) == true)
                {
                    TSaveid = Clo_Mem[S_Mbid].Nominid;
                    TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;

                    GiveGrade = Clo_Mem[S_Mbid].OneGrade ;
                }
                else
                    TSaveid = "**";

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance1"].ToString()); //+ double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                //OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].OneGrade >= 30 && Clo_Mem[S_Mbid].Sell_Mem_TF == 0 )
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;
                            if (R_LevelCnt == 1)
                            {
                                Allowance1 = TotalPV * 0.2;
                                TPer = "20";
                            }
                            if (R_LevelCnt == 2)
                            {
                                Allowance1 = TotalPV * 0.15;
                                TPer = "15";
                            }

                            if (R_LevelCnt >= 3)
                            {
                                Allowance1 = TotalPV * 0.1;
                                TPer = "10";
                            }

                            if (Clo_Mem[S_Mbid].OneGrade <= 40 && R_LevelCnt >= 2 ) Allowance1 = 0;
                            if (Clo_Mem[S_Mbid].OneGrade <= 60 && R_LevelCnt >= 3) Allowance1 = 0;
                            if (Clo_Mem[S_Mbid].OneGrade <= 80 && R_LevelCnt >= 4) Allowance1 = 0;
                            if (Clo_Mem[S_Mbid].OneGrade <= 100 && R_LevelCnt >= 5) Allowance1 = 0;                            

                            if (Allowance1 > 0) //후원보너스을 받은 사람만 매칭 보너스를 받을수 잇게 처리를한다.
                            {
                                Allowance1 = Allowance1 - (Allowance1 % 10); //원단위절사
                                
                                StrSql = "Update tbl_ClosePay_02 SET ";
                                StrSql = StrSql + " Allowance2 = Allowance2 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_02";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,R_LevelCnt , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, TPer, GivePay, GiveGrade ) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_LevelCnt + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'2' ,'" + OrderNumber + "','" + TPer + "'," + TotalPV + "," + GiveGrade + " )";

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

                    if (R_LevelCnt == 5) TSaveid = "**";

                } //While


            }


            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }


            //매칭은 후원보너스의 2배까지만 받을수 있다 주당.
            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - (Allowance1 * 2)  ";
            StrSql = StrSql + " Where Allowance2 > 0 ";
            StrSql = StrSql + " And   Allowance2 > (Allowance1 * 2) ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance2 = Allowance2 - Allowance2_Cut  ";
            StrSql = StrSql + " Where Allowance2 > 0 ";
            StrSql = StrSql + " And   Allowance2_Cut >  0 "; 

            Temp_Connect.Insert_Data(StrSql, Conn, tran);    
        }




        private void Give_Allowance3_20150201(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double Allowance1 = 0, R_TotalPV = 0, TotalPV = 0, TotalPV_2 = 0;
            int L_1 = 0, L_2 = 0, BasePay_Level = 0;

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            StrSql = " Select Se.TotalPV , Isnull( Bs_R.TotalPV, 0 ) AS RePV  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate_2 SellDate_2 , Se.Mbid,Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + PayDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";

            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) >= 180 ";
            StrSql = StrSql + " And   Se.SellDate_2  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            //StrSql = StrSql + " And   Se.SellCode = '01' ";
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
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                R_TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                L_1 = 0; L_2 = 0;
                BasePay_Level = 0;
                //if (Mbid2.ToString() == "1091")
                //   MessageBox.Show("EE");

                S_Mbid = Mbid + "-" + Mbid2.ToString();
                if (Clo_Mem.ContainsKey(S_Mbid) == true)
                {
                    if (Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03  >= 180)
                    {
                        BasePay_Level = 6;
                    }

                    if (Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03  >= 600)
                    {
                        BasePay_Level = 11;
                    }
                }





                StrSql = " Select Sell_DownPV  ";
                StrSql = StrSql + " From tbl_Close_DownPV_PV_02 Se (nolock) ";
                StrSql = StrSql + " WHERE OrderNumber  ='" + OrderNumber + "'";
                StrSql = StrSql + " And   SortOrder = '1' ";
                //StrSql = StrSql + " And   Se.SellDate_2  <='" + ToEndDate + "'";
                //StrSql = StrSql + " And   Se.Ga_Order = 0 ";
                //StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2  ASC ";

                DataSet ds2 = new DataSet();
                int ReCnt2 = 0;
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds2);
                ReCnt2 = Search_Connect.DataSet_ReCount;

                if (ReCnt2 >0 )
                    R_TotalPV = double.Parse(ds2.Tables[base_db_name].Rows[0]["Sell_DownPV"].ToString());  //신규로 사용된 내역만 즉 후원수당에 누적된 내역만.. 추천으로 풀리는 거임.


                TSaveid = Clo_Mem[S_Mbid].Nominid;
                TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                TLine = Clo_Mem[S_Mbid].N_LineCnt;

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**" && ReCnt2 > 0)
                {                    

                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].ReqTF2 >= 1 && (Clo_Mem[S_Mbid].SellPV01 + Clo_Mem[S_Mbid].SellPV02 + Clo_Mem[S_Mbid].SellPV03  >= 180 || Clo_Mem[S_Mbid].CurGrade >= 10))
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;

                            if (LevelCnt == 1) Allowance1 = R_TotalPV * 0.1;
                            if (LevelCnt == 2) Allowance1 = R_TotalPV * 0.01;
                            if (LevelCnt == 3) Allowance1 = R_TotalPV * 0.01;
                            if (LevelCnt == 4) Allowance1 = R_TotalPV * 0.01;
                            if (LevelCnt == 5) Allowance1 = R_TotalPV * 0.01;
                            if (LevelCnt == 6) Allowance1 = R_TotalPV * 0.01;
                            if (LevelCnt == 7) Allowance1 = R_TotalPV * 0.01;
                            if (LevelCnt == 8) Allowance1 = R_TotalPV * 0.01;
                            if (LevelCnt == 9) Allowance1 = R_TotalPV * 0.01;
                            if (LevelCnt == 10) Allowance1 = R_TotalPV * 0.01;
                            if (LevelCnt == 11) Allowance1 = R_TotalPV * 0.01;

                            

                            if (Allowance1 > 0)
                            {
                                Allowance1 = Allowance1 * Kor_Pay;

                                StrSql = "Update tbl_ClosePay_02 SET ";
                                StrSql = StrSql + " Allowance3 = Allowance3 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_02";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_LevelCnt + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'3' ,'" + OrderNumber + "')";

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

                    if (LevelCnt == BasePay_Level) TSaveid = "**";

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




        private void Give_Allowance3_Be(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "";
            double Allowance1 = 0;

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = "  Select  Allowance1 ,  Nominid , Nominid2 , Mbid , Mbid2 ";
            StrSql = StrSql + " ,N_LineCnt , M_Name, N_LineCnt , LineCnt   ";
            StrSql = StrSql + "  From tbl_ClosePay_02    ";
            StrSql = StrSql + " Where  Allowance1 > 0 ";
            
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
                //Allowance1 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance1"].ToString());

                //Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                //Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                //M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                TSaveid = sr.GetValue(1).ToString();
                TSaveid2 = int.Parse(sr.GetValue(2).ToString());
                TLine = int.Parse(sr.GetValue(5).ToString());
                Allowance1 = double.Parse(sr.GetValue(0).ToString());

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
                                Allowance1 = (Allowance1) * 0.1;
                            }

                            if (LevelCnt == 1 && Clo_Mem[S_Mbid].CurGrade >= 40)
                            {
                                Allowance1 = Allowance1 * 0.1;
                            }

                            if (LevelCnt == 2 && Clo_Mem[S_Mbid].CurGrade >= 50)
                            {
                                Allowance1 = Allowance1 * 0.1;
                            }

                            if (LevelCnt == 2 && Clo_Mem[S_Mbid].CurGrade >= 60)
                            {
                                Allowance1 = Allowance1 * 0.1;
                            }

                            Allowance1 = (Allowance1) * 0.1;

                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_02 SET ";
                                StrSql = StrSql + " Allowance3 = Allowance3 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_02";
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


            StrSql = " Select OrderNumber , Re_BaseOrderNumber ,  TotalCv TotalPV , tbl_SalesDetail.Mbid , tbl_SalesDetail.Mbid2 , tbl_SalesDetail.M_Name , SellDate_2 SellDate_2  ";            
            StrSql = StrSql + " From tbl_SalesDetail (nolock)   ";
            StrSql = StrSql + " WHERE TotalCv < 0   ";
            StrSql = StrSql + " And   SellDate_2  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   SellDate_2  <='" + ToEndDate + "'";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql,  base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            int  Mbid2 = 0 ;
            string Mbid = "", Re_BaseOrderNumber = "", OrderNumber = "", M_Name ="", SellDate_2 = "";
            double Base_PV = 0, Return_Pay = 0, TotalPV = 0 ;
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                
                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                Re_BaseOrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["Re_BaseOrderNumber"].ToString();
                TotalPV = -double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();
                SellDate_2 = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate_2"].ToString();

                StrSql = "SELECT  DownPV , SaveMbid, SaveMbid2, SaveName ,SortOrder ,EndDate " ;
                StrSql = StrSql + " From tbl_Close_DownPV_ALL_02  ";
                StrSql = StrSql + " WHERE RequestMbid = '" + Mbid + "'" ;
                StrSql = StrSql + " And   RequestMbid2 = " + Mbid2 ;
                StrSql = StrSql + " And   OrderNumber = '" + Re_BaseOrderNumber + "'" ;
                StrSql = StrSql + " And   SortOrder = '3' ";

               
                SqlDataReader sr2 = null;
                Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr2);                
                int ReCnt2 = Temp_Connect.DataSet_ReCount;

                if (ReCnt2 > 0)
                {
                    Base_PV = 0;



                    StrSql = "SELECT  TotalCV TotalPV  From tbl_SalesDetail (nolock)  ";
                    StrSql = StrSql + " WHERE   OrderNumber = '" + Re_BaseOrderNumber + "'";

                    DataSet ds3 = new DataSet();
                    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds3);
                    int ReCnt3 = Search_Connect.DataSet_ReCount;

                    if (ReCnt3 > 0)
                        Base_PV = double.Parse(ds3.Tables[base_db_name].Rows[0]["TotalPV"].ToString());


                    if (Base_PV > 0)
                    {




                        double SumAllAllowance_cut_per = 0; 
                        //for (int fi_cnt2 = 0; fi_cnt2 <= ReCnt2 - 1; fi_cnt2++)
                        while (sr2.Read() )
                        {

                            StrSql = "SELECT  SumAllAllowance_cut_per  From tbl_ClosePay_02_Mod (nolock)  ";
                            StrSql = StrSql + " WHERE   ToEndDate = '" + sr2.GetValue(0).ToString() + "'";
                            StrSql = StrSql + " And  Mbid ='' ";
                            StrSql = StrSql + " And  Mbid2 = " + sr2.GetValue(2).ToString();

                            DataSet ds44 = new DataSet();
                            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds44);
                            int ReCnt44 = Search_Connect.DataSet_ReCount;

                            if (ReCnt44 > 0)
                                SumAllAllowance_cut_per = double.Parse(ds44.Tables[base_db_name].Rows[0]["SumAllAllowance_cut_per"].ToString());

                            if (SumAllAllowance_cut_per == 0) SumAllAllowance_cut_per = 1; 


                            Return_Pay = 0 ;
                            Return_Pay = double.Parse(sr2.GetValue (0).ToString()) * (TotalPV / Base_PV);

                            if (Return_Pay > 0)
                            {
                                Return_Pay = Return_Pay * SumAllAllowance_cut_per;

                                 StrSql = " INSERT INTO tbl_Sales_Put_Return_Pay ";
                                 StrSql = StrSql + " (ToEndDate,OrderNumber,Re_BaseOrderNumber,C_Mbid,C_Mbid2, C_M_Name ,R_Mbid,R_Mbid2, R_M_Name , SellDate , Return_Pay, Return_Pay2, Cl_TF, SortOrder )";
                                StrSql = StrSql + " Values(" ;
                                StrSql = StrSql + "'" + ToEndDate + "','" + OrderNumber + "'";
                                StrSql = StrSql + ",'" + Re_BaseOrderNumber  + "',";
                                StrSql = StrSql + "'" + sr2.GetValue (1).ToString()  + "'";
                                StrSql = StrSql + "," + int.Parse(sr2.GetValue(2).ToString()) + ",";
                                StrSql = StrSql + "'" + sr2.GetValue(3).ToString() + "'";
                                StrSql = StrSql + ",'" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + SellDate_2 + "'," + Return_Pay + "," + Return_Pay + ",2 , 'W3'";
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

        private void Put_Return_Pay_All_1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            
            string StrSql = "";
            
            pg1.Value = 0; pg1.Maximum = 15;
            pg1.PerformStep(); pg1.Refresh();


            //Allowance1_18_Week


            //StrSql = "select top 12 fromenddate , ToEndDate from dbo.tbl_CloseTotal_02 Order by ToEndDate ASC ";
            string SDate3 = "", SDate4 = "";
            //int ReCnt = 0;
            //DataSet Dset4 = new DataSet();
            //Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
            //ReCnt = Search_Connect.DataSet_ReCount;

            StrSql = "select top 12 fromenddate , ToEndDate from dbo.tbl_CloseTotal_02  Where ToEndDate <> '" + ToEndDate + "' Order by ToEndDate DESC  ";
            int ReCnt = 0;
            DataSet Dset12 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset12);
            ReCnt = Search_Connect.DataSet_ReCount;

            if (ReCnt >= 12)
            {
                SDate3 = Dset12.Tables[base_db_name].Rows[11][1].ToString();
            }
            else
            {
                if (ReCnt >= 1)
                {
                    SDate3 = Dset12.Tables[base_db_name].Rows[ReCnt - 1][1].ToString();
                }
            }
            pg1.PerformStep(); pg1.Refresh();



            //if (ReCnt > 0)
            //{  //이번주 포함 4주간의 매출 합산을 불러온다.  150
            //    SDate3 = Dset4.Tables[base_db_name].Rows[0][0].ToString();
            //}

            if (SDate3 == "") SDate3 = FromEndDate;
            pg1.PerformStep(); pg1.Refresh();


            //18주 동안의 팀 매출 수당을 합산처리한다.
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1_18_Week = Allowance1 +  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Allowance1)  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_ClosePay_02_Mod Se (nolock) ";            
            StrSql = StrSql + " WHERE   Se.fromenddate  >='" + SDate3 + "'";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  A1_Point_ = ROUND(Sum_PV_1 / 500,0,1)  ";
            StrSql = StrSql + " ,Sum_PV_1 =  Sum_PV_1 + (ROUND(Sum_PV_1 / 500,0,1) *  (-500))  ";
            StrSql = StrSql + " Where (Sell_Mem_TF = 0 ) ";            
            StrSql = StrSql + " And Sum_PV_1 <= -500  ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  A1_Point_ = A1_Point_ +  ROUND(Sum_PV_2 / 500,0,1)  ";
            StrSql = StrSql + " ,Sum_PV_2 =  Sum_PV_2 + (ROUND(Sum_PV_2 / 500,0,1) *  (-500))  ";
            StrSql = StrSql + " Where (Sell_Mem_TF = 0 ) ";
            StrSql = StrSql + " And Sum_PV_2 <= -500  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 Set";
            StrSql = StrSql + "  Allowance1_ =  -((A1_Point_ * 500)  * 0.1 )"; 
            StrSql = StrSql + " Where CurGrade >=  10 ";
            StrSql = StrSql + " And   CurGrade <=  40 ";
            StrSql = StrSql + " And   Sell_Mem_TF = 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_02 Set";
            StrSql = StrSql + "  Allowance1_ =  -((A1_Point_ * 500)  * 0.075 )";
            StrSql = StrSql + " Where CurGrade >=  50 ";
            StrSql = StrSql + " And   CurGrade <=  80 ";
            StrSql = StrSql + " And   Sell_Mem_TF = 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set";
            StrSql = StrSql + "  Allowance1_ = -((A1_Point_ * 500)  * 0.05 )";
            StrSql = StrSql + " Where CurGrade >=  90 ";            
            StrSql = StrSql + " And   Sell_Mem_TF = 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            
            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1_ = Allowance1_ * 1000  ";
            StrSql = StrSql + " Where Allowance1_ > 0 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1_ = Round((Allowance1_ /10),0,1) * 10 ";
            StrSql = StrSql + " Where Allowance1_ > 0 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //18주동안 받은 팀 후원보다 공제금액이 더 클수는 없기 때문에 짜름
            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Allowance1_ = Allowance1_18_Week ";
            StrSql = StrSql + " ,Allowance1_Cut_ = Allowance1_ - Allowance1_18_Week   ";
            StrSql = StrSql + " Where Allowance1_ > 0 ";
            StrSql = StrSql + " And  Allowance1_18_Week  < Allowance1_ ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = " INSERT INTO tbl_Sales_Put_Return_Pay ";
            StrSql = StrSql + " (ToEndDate,OrderNumber,Re_BaseOrderNumber,C_Mbid,C_Mbid2, C_M_Name ,R_Mbid,R_Mbid2, R_M_Name , SellDate , Return_Pay, Return_Pay2, Cl_TF , SortOrder )";
            StrSql = StrSql + " Select ";
            StrSql = StrSql + "'" + ToEndDate + "','C1'";
            StrSql = StrSql + ",'C1'";
            StrSql = StrSql + ",Mbid";
            StrSql = StrSql + ",Mbid2 ";
            StrSql = StrSql + ",M_Name";
            StrSql = StrSql + ",Mbid";
            StrSql = StrSql + ",Mbid2 ";
            StrSql = StrSql + ",M_Name";
            StrSql = StrSql + ",'" + ToEndDate + "',Allowance1_ , Allowance1_ ,2,  'W2'";
            StrSql = StrSql + " From tbl_ClosePay_02  (nolock) Where Allowance1_ >  0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //-------------------------------------------------------------------------------------------------------------------
        }


        private void Put_Sum_Return_Remain_Pay(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4   ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_02 SET " ;
            StrSql = StrSql + " Sum_Return_Take_Pay = ISNULL(B.A1, 0 )   " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, " ;
    
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
            StrSql = "Update tbl_ClosePay_02 SET " ;
            StrSql = StrSql + " Sum_Return_DedCut_Pay = ISNULL(B.A1, 0 )   " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, " ;
    
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




            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Sum_Return_Remain_Pay = Sum_Return_Take_Pay - Sum_Return_DedCut_Pay " ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }

        private void CalculateTruePayment(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 15    ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            ////tbl_Sham_Sell_Down_2
            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Etc_Pay = ISNULL(B.A1, 0 )   ";
            //StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            //StrSql = StrSql + " (Select    Sum(Apply_Pv) A1,  mbid ,mbid2   ";
            //StrSql = StrSql + " From tbl_Sham_Pay (nolock) ";
            //StrSql = StrSql + " Where   Apply_Date >='" + FromEndDate  + "'";
            //StrSql = StrSql + " And     Apply_Date <= '" + ToEndDate  + "' ";
            //StrSql = StrSql + " And     (SortKind2 = '02' Or SortKind2 = '2' )  ";
            //StrSql = StrSql + " Group By mbid ,mbid2 ";
            //StrSql = StrSql + " ) B";

            //StrSql = StrSql + " Where A.Mbid  = B.Mbid ";
            //StrSql = StrSql + " And   A.Mbid2  = B.Mbid2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);

            

            StrSql = "Update tbl_ClosePay_02 Set "    ;
            StrSql = StrSql + " SumAllAllowance = Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 -  Allowance3_Cut  ";
            StrSql = StrSql + " Where Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 -  Allowance3_Cut   > 0";
    
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            


            double TotalPrice = 0, TotalAllowance1 = 0, Cut_Per2 = 0, S_CutPay2 = 0 , Per_SumAllowance = 0 ;
            //
            StrSql = " Select Isnull(SUM(TotalPrice), 0 )  from tbl_SalesDetail (nolock) ";
            StrSql = StrSql + " Where Ga_Order = 0 And SellCode <> '' ";
            StrSql = StrSql + " And left(SellDate_2,4)  = '" + FromEndDate.Substring(0, 4) + "'";
            StrSql = StrSql + " And SellDate_2 <= '" + ToEndDate  +"'"; 

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {
                TotalPrice = double.Parse(ds.Tables[base_db_name].Rows[0][0].ToString());  //동일년의 매출 실적을 가져온다.
            }

            StrSql = " Select ISNULL(SUM(SumAllAllowance), 0) From tbl_ClosePay_02 (nolock) Where SumAllAllowance > 0 ";
            DataSet ds2 = new DataSet();
            int ReCnt2 = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds2);
            ReCnt2 = Search_Connect.DataSet_ReCount;

            if (ReCnt2 > 0)
            {
                TotalAllowance1 = double.Parse(ds2.Tables[base_db_name].Rows[0][0].ToString());
                Per_SumAllowance = TotalAllowance1; 
            }
            pg1.PerformStep(); pg1.Refresh();


            //동일년에 발생한 수당금액을 다 불러온다.
            StrSql = " Select ISNULL(SUM(SumAllAllowance), 0) From tbl_ClosePay_02_Mod (nolock) ";
            StrSql = StrSql + "  Where SumAllAllowance > 0 ";
            StrSql = StrSql + " And left(FromEndDate,4)  = '" + FromEndDate.Substring(0, 4) + "'";
            DataSet ds3 = new DataSet();
            int ReCnt3 = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds3);
            ReCnt3 = Search_Connect.DataSet_ReCount;

            if (ReCnt3 > 0)
            {
                TotalAllowance1 = TotalAllowance1 +  double.Parse(ds3.Tables[base_db_name].Rows[0][0].ToString());
            }

            double Kor_Pay = 32; 

            if (TotalAllowance1 > 0)
            {
                if (TotalAllowance1 / TotalPrice * 100 > Kor_Pay)
                {
                    Cut_Per2 = ((TotalAllowance1 / TotalPrice) * 100) - Kor_Pay;
                    S_CutPay2 = TotalPrice * (Cut_Per2 / 100);

                    StrSql = "Update tbl_ClosePay_02 Set ";
                    StrSql = StrSql + "  SumAllAllowance_Cut = Convert( FLOAT," + S_CutPay2 + ") * (SumAllAllowance / Convert(FLOAT," + Per_SumAllowance + "))";
                    StrSql = StrSql + " Where SumAllAllowance  > 0 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);

                    StrSql = "Update tbl_ClosePay_02 Set ";
                    //StrSql = StrSql + "  SumAllAllowance_Cut = ROUND(SumAllAllowance_Cut,0,1)";  //소수점 아래를 다 절사를 해버린다.
                    StrSql = StrSql + "  SumAllAllowance_Cut = ROUND(SumAllAllowance_Cut,-1,0)";  // 원단위 아래를 절사를 해버린다.                   
                    StrSql = StrSql + " Where SumAllAllowance_Cut  > 0 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);

                    StrSql = "Update tbl_ClosePay_02 Set ";
                    StrSql = StrSql + " SumAllAllowance = SumAllAllowance - SumAllAllowance_Cut ";
                    StrSql = StrSql + " Where SumAllAllowance  > 0 And SumAllAllowance_Cut >0 ";
                    Temp_Connect.Insert_Data(StrSql, Conn, tran);


                    StrSql = "Update tbl_ClosePay_02 set SumAllAllowance_cut_per = Convert( FLOAT,SumAllAllowance / (SumAllAllowance+ SumAllAllowance_Cut )) ";
                    StrSql = StrSql + " Where SumAllAllowance  > 0 And SumAllAllowance_Cut >0 ";
                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                }
            }
            pg1.PerformStep(); pg1.Refresh();




    

            //'''---반품으로 해서 차감시킬 금액이 아직 남아잇다.
            StrSql = "Update tbl_ClosePay_02 Set "    ;
            StrSql = StrSql + " Cur_DedCut_Pay = SumAllAllowance "    ;
            StrSql = StrSql + ",SumAllAllowance = 0 "    ;
            StrSql = StrSql + " Where SumAllAllowance  > 0"    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay > 0 "    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay >= SumAllAllowance "    ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 Set "    ;
            StrSql = StrSql + " Cur_DedCut_Pay = Sum_Return_Remain_Pay "    ;
            StrSql = StrSql + ",SumAllAllowance = SumAllAllowance - Sum_Return_Remain_Pay "    ;
            StrSql = StrSql + " Where SumAllAllowance  > 0"    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay > 0 "    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay < SumAllAllowance "    ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " SumAllAllowance_10000 = ISNULL(B.A1, 0 )   ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (Select    Sum(SumAllAllowance) A1,  Mbid ,Mbid2   ";
            StrSql = StrSql + " From tbl_ClosePay_10000  (nolock)  ";
            StrSql = StrSql + " Where   AP_ToEndDate  ='' ";
            StrSql = StrSql + " And     SumAllAllowance > 0 ";
            StrSql = StrSql + " Group By Mbid ,Mbid2 ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid  = B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2  = B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " SumAllAllowance_Be_Not = ISNULL(B.A1, 0 )   ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (Select    Sum(SumAllAllowance) A1,  Mbid ,Mbid2   ";
            StrSql = StrSql + " From tbl_CloseNot_Pay (nolock) ";
            StrSql = StrSql + " Where   AP_ToEndDate  ='' ";
            StrSql = StrSql + " And     SumAllAllowance > 0 ";
            StrSql = StrSql + " Group By Mbid ,Mbid2 ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid  = B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2  = B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            
            
            
            StrSql = "Update tbl_ClosePay_02 Set "    ;
            StrSql = StrSql + " SumAllAllowance = Round((SumAllAllowance +  SumAllAllowance_10000 + SumAllAllowance_Be_Not) /10,0,1) * 10 ";            
            StrSql = StrSql + " Where SumAllAllowance > 0 ";

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            
           
            
            StrSql = "Update tbl_ClosePay_02 Set "    ;
            StrSql = StrSql + " InComeTax = Round(((SumAllAllowance * 0.03) /10),0,1) * 10  ";
            //StrSql = StrSql + " InComeTax = Convert(int,((SumAllAllowance ) * 0.03) /10) * 10  ";
            StrSql = StrSql + " Where SumAllAllowance  >= 30000 ";
            //StrSql = StrSql + " Where SumAllAllowance >0 ";
            
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            
            StrSql = "Update tbl_ClosePay_02 Set "    ;
            StrSql = StrSql + " ResidentTax = Round(((InComeTax * 0.1) /10),0,1) * 10  ";
            //StrSql = StrSql + " ResidentTax = Convert(int,(InComeTax * 0.1) /10) * 10  "    ;
            StrSql = StrSql + " Where SumAllAllowance   >= 30000 ";
           // StrSql = StrSql + " Where SumAllAllowance > 0 ";
            

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();






            StrSql = "Update tbl_ClosePay_02 Set "    ;
            StrSql = StrSql + " TruePayment =  ((SumAllAllowance - InComeTax - ResidentTax) / 10 ) * 10 ";
            //StrSql = StrSql + " TruePayment = (((SumAllAllowance ) - InComeTax - ResidentTax) / 10 ) * 10 ";
            StrSql = StrSql + " Where SumAllAllowance   >= 30000 ";
            //StrSql = StrSql + " Where SumAllAllowance > 0 ";

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update tbl_ClosePay_02 Set ";
            ////StrSql = StrSql + " Sum_Gibu = Convert(int,(TruePayment * 0.01) / 10 ) * 10 ";
            ////StrSql = StrSql + " Where TruePayment  > 0 ";
            ////StrSql = StrSql + " And   GiBu_  > 0 ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);


            ////StrSql = "Update tbl_ClosePay_02 Set ";
            ////StrSql = StrSql + " TruePayment = TruePayment -  Sum_Gibu ";
            ////StrSql = StrSql + " Where TruePayment  > 0 ";
            ////StrSql = StrSql + " And   Sum_Gibu  > 0 ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_Close_Not_Pay SET ";
            StrSql = StrSql + "  AP_ToEndDate = '" + ToEndDate + "'";            
            StrSql = StrSql + " FROM  tbl_Close_Not_Pay  A, ";

            StrSql = StrSql + " (Select  Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where SumAllAllowance   >= 30000 ";
            StrSql = StrSql + " And SumAllAllowance_Be_Not  > 0 ";
            StrSql = StrSql + " And SumAllAllowance > 0 ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.AP_ToEndDate =  '' ";
            StrSql = StrSql + " And   A.Mbid = B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2 = B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_10000 SET ";
            StrSql = StrSql + "  AP_ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " ,AP_TF = 2 ";
            StrSql = StrSql + " FROM  tbl_ClosePay_10000  A, ";

            StrSql = StrSql + " (Select  Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " Where SumAllAllowance   >= 30000 ";
            StrSql = StrSql + " And SumAllAllowance_10000 > 0 ";
            StrSql = StrSql + " And SumAllAllowance > 0 ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.AP_ToEndDate =  '' ";
            StrSql = StrSql + " And   A.Mbid = B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2 = B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "INSERT INTO tbl_ClosePay_10000 ";
            StrSql = StrSql + "(ToEndDAte,ToEndDAte_TF, mbid,mbid2,SumAllAllowance ) ";
            StrSql = StrSql + " Select  ";
            StrSql = StrSql + " '" + ToEndDate + "',2 ,Mbid,Mbid2,SumAllAllowance - SumAllAllowance_10000 - SumAllAllowance_Be_Not ";
            StrSql = StrSql + " From tbl_ClosePay_02  (nolock) ";
            StrSql = StrSql + " Where SumAllAllowance   < 30000 ";
            StrSql = StrSql + " And SumAllAllowance > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            // 30000원이하 보관금으로 들어간 건들 수당관련 금액 0원 처리
            StrSql = " Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " InComeTax = 0, ResidentTax = 0, TruePayment = 0 ";
            StrSql = StrSql + " Where SumAllAllowance   < 30000  ";
            StrSql = StrSql + " And SumAllAllowance > 0  ";

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
            StrSql = StrSql + " From tbl_ClosePay_02  (nolock)  ";
            StrSql = StrSql + " WHERE Cur_DedCut_Pay > 0 ";

            ReCnt = 0;
            //SqlDataReader sr = null;
            //Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            //ReCnt = Temp_Connect.DataSet_ReCount;
            DataSet ds = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;
                
            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            int Mbid2 = 0, Top_SW = 0,  TSw = 0,  T_index = 0 ;
            double T_Pay = 0;
            double Cut_Pay = 0, RR_Cut_Pay = 0;
            string Mbid = "", Re_BaseOrderNumber = "", M_Name = "";

           // t_qu.Clear();
            
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            //while (sr.Read ())
            {
                //Cut_Pay = int.Parse(sr.GetValue (0).ToString ());
                //Mbid = sr.GetValue(1).ToString();
                //Mbid2 = int.Parse(sr.GetValue(2).ToString());
                //M_Name = sr.GetValue(3).ToString();

                Cut_Pay = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Cur_DedCut_Pay"].ToString());
                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

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
                        T_Pay = double.Parse(ds2.Tables[base_db_name].Rows[fi_cnt2]["Return_Pay2"].ToString());
                        Re_BaseOrderNumber = ds2.Tables[base_db_name].Rows[fi_cnt2]["Re_BaseOrderNumber"].ToString();
                        T_index = int.Parse(ds2.Tables[base_db_name].Rows[fi_cnt2]["T_index"].ToString());

                        TSw = 0;

                        while (Cut_Pay != 0 && TSw == 0)
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

                            if (RR_Cut_Pay != 0)
                            {
                                StrSql = "Insert into tbl_Sales_Put_Return_Pay (ToEndDate, C_mbid,C_mbid2 , C_M_Name , Return_Pay , Base_OrderNumber , Base_T_index , Cl_TF ) " ;
                                StrSql = StrSql + " Values (";
                                StrSql = StrSql + " '" + ToEndDate + "','" + Mbid + "', " + Mbid2 ;
                                StrSql = StrSql + " , '" + M_Name + "', " + RR_Cut_Pay + ",";
                                StrSql = StrSql + "'" + Re_BaseOrderNumber + "'";
                                StrSql = StrSql + "," + T_index + ",2";
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

           // sr.Close(); sr.Dispose();

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
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Delete From tbl_CloseTotal_02 ";
            StrSql = StrSql + "  Where ToEndDate ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();


            StrSql = "INSERT INTO tbl_CloseTotal_02 ";
            StrSql = StrSql +  " (ToEndDate,      FromEndDate,   PayDate ,   PayDate2 " ;
            StrSql = StrSql +  " ,TotalSellAmount,TotalInputCash,TotalInputCard,TotalInputBank"  ;
            StrSql = StrSql +  " ,TotalSellPV,    TotalShamPV,   TotalReturnSellAmount"  ;
            StrSql = StrSql +  " ,TotalReturnInputCash, TotalReturnInputCard,TotalReturnInputBank, TotalReturnSellPV "  ;
            StrSql = StrSql +  " ,TotalSellCV,TotalReturnSellCV " ;
            StrSql = StrSql + " ,PC_TotalSellCV,PC_TotalReturnSellCV "; 
            StrSql = StrSql +  " ,Temp01,Temp02, Temp03, Temp04, Temp05, Temp06 , Temp07, Temp08, Temp09, Temp10, Temp11, Temp12 "  ; 
            StrSql = StrSql +  " ,RecordID,RecordTime "  ;
            StrSql = StrSql +  " ) "  ;
    
            StrSql = StrSql +  " Select "  ;
            StrSql = StrSql +  "'" + ToEndDate +  "','" +  FromEndDate +  "','" +  PayDate +  "','" +  PayDate2 +  "'" ;
            StrSql = StrSql +  ",Sum(DayAmount),Sum(DayCash),Sum(DayCard),Sum(DayBank)" ;
            StrSql = StrSql +  ",Sum(DayTotalPV),Sum(DayShamSell),Sum(DayReAmount)";
            StrSql = StrSql +  ",Sum(DayReCash),Sum(DayReCard),Sum(DayReBank),Sum(DayReTotalPV)";
            StrSql = StrSql +  ",Sum(DayTotalCV),Sum(DayReTotalCV) " ;
            StrSql = StrSql + " ,0,0 ";
            StrSql = StrSql +  "," + double.Parse(txtB1.Text) + "," + double.Parse(txtB2.Text) + "," + double.Parse(txtB3.Text) + "," + double.Parse(txtB4.Text) + "," + double.Parse(txtB5.Text) ;
            StrSql = StrSql +  "," + double.Parse(txtB6.Text) + "," + double.Parse(txtB7.Text) + "," + double.Parse(txtB8.Text) + "," + double.Parse(txtB9.Text) + "," + double.Parse(txtB10.Text) ;
            StrSql = StrSql +  ", 0 , 0 " ; 

            StrSql = StrSql +  ",'" + cls_User.gid  + "',Convert(Varchar(25),GetDate(),21)" ;
            StrSql = StrSql + " From  tbl_ClosePay_02_Sell (nolock) ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            //PC 들의  CV 만 별도로 가져온다.
            StrSql = "Update tbl_CloseTotal_02 SET ";
            StrSql = StrSql + "  PC_TotalSellCV =ISNULL(B.A1,0) ";
            StrSql = StrSql + " ,PC_TotalReturnSellCV =ISNULL(B.A2,0) ";
            StrSql = StrSql + " FROM  tbl_CloseTotal_02  A, ";

            StrSql = StrSql + " (Select ";
            StrSql = StrSql + " Sum(DayTotalCV) A1 ,Sum(DayReTotalCV) A2  ";
            StrSql = StrSql + " From tbl_ClosePay_02_Sell (nolock) ";
            StrSql = StrSql + " LEFT JOIN tbl_ClosePay_02 (nolock) ON tbl_ClosePay_02_Sell.Mbid = tbl_ClosePay_02.Mbid And tbl_ClosePay_02_Sell.Mbid2 = tbl_ClosePay_02.Mbid2 ";
            StrSql = StrSql + " Where tbl_ClosePay_02.Sell_Mem_TF = 1 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.ToEndDate ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

        }




        private void tbl_CloseTotal_Put2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_CloseTotal_02 SET " ;
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

        //    StrSql = StrSql + " ,Allowance11 =ISNULL(B.A11,0) ";
        //    StrSql = StrSql + " ,Allowance12 =ISNULL(B.A12,0) ";
        //    StrSql = StrSql + " ,Allowance13 =ISNULL(B.A13,0) ";
        ////    StrSql = StrSql + " ,Allowance14 =ISNULL(B.A14,0) " ;
        ////    StrSql = StrSql + " ,Allowance15 =ISNULL(B.A15,0) " ;
        ////'    StrSql = StrSql + " ,Allowance16 =ISNULL(B.A16,0) " ;
        ////'    StrSql = StrSql + " ,Allowance17 =ISNULL(B.A17,0) " ;
        ////'    StrSql = StrSql + " ,Allowance18 =ISNULL(B.A18,0) " ;
        ////'    StrSql = StrSql + " ,Allowance19 =ISNULL(B.A19,0) " ;
        ////'    StrSql = StrSql + " ,Allowance10 =ISNULL(B.A20,0) " ;
        ////'
        ////    StrSql = StrSql + " ,Allowance11 =ISNULL(B.A21,0) " ;
        ////    StrSql = StrSql + " ,Allowance12 =ISNULL(B.A22,0) " ;
        ////    StrSql = StrSql + " ,Allowance13 =ISNULL(B.A23,0) " ;
        ////    StrSql = StrSql + " ,Allowance14 =ISNULL(B.A24,0) " ;
        ////    StrSql = StrSql + " ,Allowance15 =ISNULL(B.A25,0) " ;
        ////    StrSql = StrSql + " ,Allowance16 =ISNULL(B.A26,0) " ;
            //StrSql = StrSql + " ,Allowance17 =ISNULL(B.A27,0) " ;
            //StrSql = StrSql + " ,Allowance18 =ISNULL(B.A28,0) ";
            StrSql = StrSql + " ,Allowance19 =ISNULL(B.A29,0) ";  //반품공제
            StrSql = StrSql + " ,Allowance30 =ISNULL(B.A30,0) ";  //기타보너스

            StrSql = StrSql + " ,SumAllowance=ISNULL(B.AS1,0) " ;
            StrSql = StrSql + " ,SumInComeTax=ISNULL(B.AS2,0) " ;
            StrSql = StrSql + " ,SumResidentTax=ISNULL(B.AS3,0) " ;
            StrSql = StrSql + " ,SumTruePayment=ISNULL(B.AS4,0) " ;
            StrSql = StrSql + " ,SumAllowance_2=ISNULL(B.AS5,0) ";

            StrSql = StrSql + " FROM  tbl_CloseTotal_02  A, " ;

            StrSql = StrSql + " (Select " ;
            StrSql = StrSql + " Sum(convert(float,Allowance1)) AS A1 ,Sum(convert(float,Allowance2)) AS A2 ";
            StrSql = StrSql + ",Sum(convert(float,Allowance3)) AS A3 ,Sum(convert(float,Allowance4)) AS A4 ";
            StrSql = StrSql + ",Sum(convert(float,Allowance5)) AS A5 ,Sum(convert(float,Allowance6)) AS A6";
            StrSql = StrSql + ",Sum(convert(float,Allowance7)) AS A7 ,Sum(convert(float,Allowance8)) AS A8 ";
            StrSql = StrSql + ",Sum(convert(float,Allowance9)) AS A9 ,Sum(convert(float,Allowance10)) AS A10 ";
        //'
            //StrSql = StrSql + ",Sum(convert(float,Allowance11)) AS A11,Sum(convert(float,Allowance12)) AS A12 ";
            //StrSql = StrSql + ",Sum(convert(float,Allowance13)) AS A13,Sum(convert(float,Allowance14)) AS A14 ";
        //    StrSql = StrSql + ",Sum(Allowance15) AS A15 " //,Sum(Allowance16) AS A16" ;
        //    StrSql = StrSql + ",Sum(Allowance17) AS A17,Sum(Allowance18) AS A18 " ;
        //    StrSql = StrSql + ",Sum(Allowance19) AS A19,Sum(Allowance10) AS A20 " ;
        
        //    StrSql = StrSql + ",Sum(Allowance11) AS A21,Sum(Allowance12) AS A22 " ;
        //    StrSql = StrSql + ",Sum(Allowance13) AS A23,Sum(Allowance14) AS A24 " ;
        //    StrSql = StrSql + ",Sum(Allowance15) AS A25 " //,Sum(Allowance16) AS A26" ;
        //    StrSql = StrSql + ",Sum(Allowance17) AS A27,Sum(Allowance18) AS A28 " ;

            //StrSql = StrSql + ",Sum(convert(float,Allowance4_cut + Allowance1_1_Cut)) AS A27";
            //StrSql = StrSql + ",Sum(convert(float,Sum_Gibu)) AS A28";
            StrSql = StrSql + ",Sum(convert(float,Cur_DedCut_Pay)) AS A29";
            StrSql = StrSql + ",Sum(convert(float,Etc_Pay)) AS A30 ";


            StrSql = StrSql + ",Sum(convert(float,SumAllAllowance)) AS AS1,Sum(convert(float,InComeTax)) AS AS2 ";
            StrSql = StrSql + ",Sum(convert(float,ResidentTax)) AS AS3,Sum(convert(float,TruePayment)) AS AS4 ";
            StrSql = StrSql + ",Sum(convert(float,SumAllAllowance + SumAllAllowance_Cut)) AS AS5 ";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
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


            StrSql = "Update tbl_CloseTotal_02 Set "  ;
            StrSql = StrSql + "  Allowance1Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance1 > 0),0) ";
            StrSql = StrSql + " ,Allowance2Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance2 > 0),0) ";
            StrSql = StrSql + " ,Allowance3Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance3 > 0),0) ";
            StrSql = StrSql + " ,Allowance4Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance4 > 0),0) ";
            StrSql = StrSql + " ,Allowance5Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance5 > 0),0) ";
            StrSql = StrSql + " ,Allowance6Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance6 > 0),0) ";
            StrSql = StrSql + " ,Allowance7Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance7> 0),0) ";
            StrSql = StrSql + " ,Allowance8Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance8 > 0),0) ";
            StrSql = StrSql + " ,Allowance9Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance9 > 0),0) ";
            StrSql = StrSql + " ,Allowance10Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance10 > 0),0) ";

            //StrSql = StrSql + " ,Allowance11Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance11 > 0),0) "  ;
            //StrSql = StrSql + " ,Allowance12Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance12 > 0),0) "  ;
            //StrSql = StrSql + " ,Allowance13Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance13 > 0),0) "  ;
            ////    StrSql = StrSql + " ,Allowance14Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance14 > 0),0) "  ;
            ////    StrSql = StrSql + " ,Allowance15Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance15 > 0),0) "  ;
            ////'    StrSql = StrSql + " ,Allowance16Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance16 > 0),0) "  ;
            ////'    StrSql = StrSql + " ,Allowance17Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance17> 0),0) "  ;
            ////'    StrSql = StrSql + " ,Allowance18Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance18 > 0),0) "  ;
            //////'    StrSql = StrSql + " ,Allowance19Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance19 > 0),0) "  ;
            ////'    StrSql = StrSql + " ,Allowance10Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance10 > 0),0) "  ;
        ////'
            ////    StrSql = StrSql + " ,Allowance11Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance11 > 0),0) "  ;
            ////    StrSql = StrSql + " ,Allowance12Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance12 > 0),0) "  ;
            ////    StrSql = StrSql + " ,Allowance13Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance13 > 0),0) "  ;
            ////    StrSql = StrSql + " ,Allowance14Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance14 > 0),0) "  ;
            ////    StrSql = StrSql + " ,Allowance15Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance15 > 0),0) "  ;
            //    StrSql = StrSql + " ,Allowance16Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance16 > 0),0) "  ;
            //    StrSql = StrSql + " ,Allowance17Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance17> 0),0) "  ;
            //StrSql = StrSql + " ,Allowance17Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Allowance4_cut + Allowance1_1_Cut > 0),0) ";
           // StrSql = StrSql + " ,Allowance18Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Sum_Gibu > 0),0) ";
            StrSql = StrSql + " ,Allowance19Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Cur_DedCut_Pay > 0),0) ";
            StrSql = StrSql + " ,Allowance30Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where Etc_Pay > 0),0) ";
            StrSql = StrSql + " ,SumAllowanceCount = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where SumAllAllowance > 0),0) ";
            StrSql = StrSql + " ,SumAllowanceCount_2 = ISNULL((Select Count(Mbid) From tbl_ClosePay_02 Where SumAllAllowance + SumAllAllowance_Cut > 0),0) "; 
            
            StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "'" ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_CloseTotal_02 Set " ; 
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
        
            //StrSql = StrSql + " ,Allowance11Rate = (Allowance11 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            //StrSql = StrSql + " ,Allowance12Rate = (Allowance12 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            //StrSql = StrSql + " ,Allowance13Rate = (Allowance13 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance14Rate = (Allowance14 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance15Rate = (Allowance15 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        ////'    StrSql = StrSql + " ,Allowance16Rate = (Allowance16 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        ////'    StrSql = StrSql + " ,Allowance17Rate = (Allowance17 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        ////'    StrSql = StrSql + " ,Allowance18Rate = (Allowance18 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //////    StrSql = StrSql + " ,Allowance19Rate = (Allowance19 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        ////'    StrSql = StrSql + " ,Allowance10Rate = (Allowance10 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance11Rate = (Allowance11 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance12Rate = (Allowance12 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance13Rate = (Allowance13 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance14Rate = (Allowance14 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //'    StrSql = StrSql + " ,Allowance15Rate = (Allowance15 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //    StrSql = StrSql + " ,Allowance16Rate = (Allowance16 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
        //    StrSql = StrSql + " ,Allowance17Rate = (Allowance17 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;

            //StrSql = StrSql + " ,Allowance17Rate = (Allowance17 /(TotalSellAmount-TotalReturnSellAmount)) * 100  ";
            //StrSql = StrSql + " ,Allowance18Rate = (Allowance18 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            StrSql = StrSql + " ,Allowance19Rate = (Allowance19 /(TotalSellAmount-TotalReturnSellAmount)) * 100  ";
            StrSql = StrSql + " ,Allowance30Rate = (Allowance30 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;

            StrSql = StrSql + " ,SumAllowanceRate = (SumAllowance /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
            StrSql = StrSql + " ,SumAllowanceRate_2 = (SumAllowance_2 /(TotalSellAmount-TotalReturnSellAmount)) * 100  ";
    
            StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " And (TotalSellAmount-TotalReturnSellAmount) > 0";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 

        }




        private void MakeModForCheckRequirement1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 5;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            //if (int.Parse(FromEndDate.Substring(6, 2)) >= 23)
            //{

            //유지조건이 안되게 되면 누적된거 다 후레취 처리를 한다.
            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Fresh_1 = Sum_PV_1  ";
            StrSql = StrSql + " ,Sum_PV_1 = 0 ";
            StrSql = StrSql + " Where Sum_PV_1 > 0 ";
            StrSql = StrSql + " And   ReqTF1  = 0  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Fresh_2 = Sum_PV_2  ";
            StrSql = StrSql + " ,Sum_PV_2 = 0 ";
            StrSql = StrSql + " Where Sum_PV_2 > 0 ";
            StrSql = StrSql + " And   ReqTF1  = 0  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //    StrSql = "Update tbl_ClosePay_02 SET ";
            //    StrSql = StrSql + " Fresh_2 = Sum_PV_2  ";
            //    StrSql = StrSql + " ,Sum_PV_2 = 0 ";
            //    StrSql = StrSql + " Where Sum_PV_2 > 0 ";
            //    StrSql = StrSql + " And   ReqTF3  = 0  ";

            //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //    pg1.PerformStep(); pg1.Refresh();

            //}
            ////2016-07-14 작업 NP이하인 사람들의 AV는 후레쉬로 월말에만 이루어지던걸 매주 마감 돌릴때마다 실행되게
            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Fresh_1 = Sum_PV_1  ";
            //StrSql = StrSql + " ,Sum_PV_1 = 0 ";
            //StrSql = StrSql + " Where Sum_PV_1 > 0 ";
            //StrSql = StrSql + " And   OneGrade  <= 20  ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " Fresh_2 = Sum_PV_2  ";
            //StrSql = StrSql + " ,Sum_PV_2 = 0 ";
            //StrSql = StrSql + " Where Sum_PV_2 > 0 ";
            //StrSql = StrSql + " And   OneGrade  <= 20  ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////}



             StrSql = "Insert into tbl_ClosePay_02_Mod select "  ;
            StrSql = StrSql + " '" + ToEndDate + "','" + FromEndDate + "','" + PayDate + "','" + PayDate2 + "',*,'',''"  ;
            StrSql = StrSql + " From tbl_ClosePay_02 "  ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into tbl_ClosePay_02_Sell_Mod select "  ;
            StrSql = StrSql + " '" + ToEndDate + "',* From tbl_ClosePay_02_Sell";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }



        private void ReadyNewForCheckRequirement1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 3;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_Sham_Grade SET ";
            StrSql = StrSql + " Ap_Date= '" + ToEndDate + "'";
            StrSql = StrSql + " FROM  tbl_Sham_Grade  A, ";

            StrSql = StrSql + " (Select Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_ClosePay_02 ";
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
            StrSql = StrSql + " CurGrade = 0 , Max_CurGrade = 0  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update Tbl_Memberinfo SET ";
            StrSql = StrSql + " CurGrade=ISNULL(B.OneGrade,0) ";
            StrSql = StrSql + " ,Max_CurGrade = ISNULL(B.CurGrade,0) ";            
            StrSql = StrSql + " FROM  Tbl_Memberinfo  A, ";

            StrSql = StrSql + " (Select Mbid,Mbid2,OneGrade , CurGrade ";
            StrSql = StrSql + " From tbl_ClosePay_02 (nolock) ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid  = B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2 = B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 set " ;
            StrSql = StrSql + " DayPrice01 =0, DayPrice02 =0 , DayPrice03 = 0, " ;
            StrSql = StrSql + " DayPv01 =0, DayPv02 =0 , DayPv03 = 0, " ;
            StrSql = StrSql + " DayCv01 =0, DayCv02 =0 , DayCv03 = 0, " ;
    
            StrSql = StrSql + " SellPrice01 =0, SellPrice02 =0 , SellPrice03 = 0, " ;
            StrSql = StrSql + " SellPv01 =0, SellPv02 =0 , SellPv03 = 0, " ;
            StrSql = StrSql + " SellCv01 =0, SellCv02 =0 , SellCv03 = 0,  " ;
    
            StrSql = StrSql + " DaySham01 =0, SellSham01 =0 , " ;
    
            StrSql = StrSql + " LeaveDate = '',BankCode='',BankAcc='',Cpno='',BankOwner='',RegTime='',  BusCode = '' , StopDate = '', Sell_Mem_TF = 0 , " ;
            StrSql = StrSql + " ReqTF1 = 0, ReqTF2 = 0,ReqTF3 = 0,  ReqTF10 = 0 , ";
            
            StrSql = StrSql + " Saveid='',Saveid2=0,LineCnt=0,LevelCnt=0," ;
            StrSql = StrSql + " Nominid='',Nominid2=0,N_LineCnt=0 , ";

            ////StrSql = StrSql + " Be_M_Grade= 0  ,Cur_M_PV = 0  , GM_Cur_PV_1 =0 ,GM_Cur_PV_2 = 0 ,";

            StrSql = StrSql + " BeforeGrade =  CurGrade , CurGrade = 0 ,ShamGrade = 0 , OrgGrade = 0  , OneGrade = 0 , OneGrade_4 = 0, OneGrade_8 = 0,";

            ////StrSql = StrSql + " Cur_G_Pay_Grade = Be_G_Pay_Grade ";
            ////StrSql = StrSql + ",Cur_G_Pay_Check_Cnt = Be_G_Pay_Check_Cnt ";
            ////StrSql = StrSql + ",Cur_G_Pay_Check_Date = Be_G_Pay_Check_Date ";


            StrSql = StrSql + " Active_1_FLAG =  '' , Active_2_FLAG = '' , Active_3_FLAG =  ''  , ";
            StrSql = StrSql + " W_1_QV_Real =  0 , W_2_QV_Real = 0 , W_3_QV_Real = 0  , W_4_QV_Real = 0 , ";
            StrSql = StrSql + " Down_W_1_QV_Real =  0 , Down_W_2_QV_Real = 0 , Down_W_3_QV_Real = 0  , Down_W_4_QV_Real = 0 , ";

            StrSql = StrSql + " Max_Down_W_1_QV_Real =  0 , Max_Down_W_2_QV_Real = 0 , Max_Down_W_3_QV_Real = 0  , Max_Down_W_4_QV_Real = 0 , ";


            StrSql = StrSql + " Down_W_1_QV_Real_1 =  0 , Down_W_2_QV_Real_1 = 0 , Down_W_3_QV_Real_1 = 0  , Down_W_4_QV_Real_1 = 0 ,Down_W4_QV_Real_1 = 0 , ";
            StrSql = StrSql + " Down_W_1_QV_Real_2 =  0 , Down_W_2_QV_Real_2 = 0 , Down_W_3_QV_Real_2 = 0  , Down_W_4_QV_Real_2 = 0 ,Down_W4_QV_Real_2 = 0 , ";

            

            ////StrSql = StrSql + " Allowance3_9 =  0 , Allowance3_10 = 0 , Allowance3_11 = 0  , Allowance3_12 = 0 , ";
            ////StrSql = StrSql + " Be_G9_Cnt =  G9_Cnt , Be_G10_Cnt =  G10_Cnt  , Be_G11_Cnt =  G11_Cnt   , Be_G12_Cnt =  G12_Cnt  , ";
            ////StrSql = StrSql + " G9_Cnt =  0 , G10_Cnt =  0  , G11_Cnt =  0   , G12_Cnt =  0  , ";


            StrSql = StrSql + " GradeCnt1 = 0 , GradeCnt2 = 0 , GradeCnt3 = 0 , GradeCnt4 = 0 , GradeCnt5 = 0 ,GradeCnt6 = 0 ,GradeCnt7 = 0 ,GradeCnt8 = 0 , GradeCnt9 = 0 , GradeCnt10 = 0 , GradeCnt11 = 0, GradeCnt12 = 0, ";
            StrSql = StrSql + " Max_GradeCnt1 = 0 ,Max_GradeCnt2 = 0 , Max_GradeCnt3 = 0 , Max_GradeCnt4 = 0 , Max_GradeCnt5 = 0 ,Max_GradeCnt6 = 0 ,Max_GradeCnt7 = 0 , ";
            StrSql = StrSql + " Max_GradeCnt8 = 0 , Max_GradeCnt9 = 0 , Max_GradeCnt10 = 0 , Max_GradeCnt11 = 0, Max_GradeCnt12 = 0, ";


            StrSql = StrSql + " Be_PV_1 =  Sum_PV_1 , Sum_PV_1= 0, Ded_1= 0 , Fresh_1 = 0 , Cur_PV_1 = 0 , Re_Cur_PV_1 = 0 , Sham_PV_1 = 0 ,Re_Cur_QV_1 = 0 , Real_Sum_PV_1 = 0 , ReqTF1_L_1 = 0 ,Cur_Price_1 = 0 , ";
            StrSql = StrSql + " Be_PV_2 =  Sum_PV_2 , Sum_PV_2= 0, Ded_2= 0 , Fresh_2 = 0 , Cur_PV_2 = 0 , Re_Cur_PV_2 = 0 , Sham_PV_2 = 0 ,Re_Cur_QV_2 = 0 , Real_Sum_PV_2 = 0 , ReqTF1_L_2 = 0 ,Cur_Price_2 = 0 ,  ";

            StrSql = StrSql + " W4_QV_Real = 0 , W4_QV = 0 , W4_QV_Auto = 0 , W4_QV_Down = 0 , A1_Point = 0 , A1_Point_ = 0 , Allowance2_T_Per = 0 , Allowance1_18_Week = 0 ,  ";



            StrSql = StrSql + " Down_W4_QV_Real = 0 , Down_G_Down = 0 ,  Max_N_LineCnt = 0 , Max_Down_W4_QV_Real = 0 , ";

            //StrSql = StrSql + " Be_G_Pay_Grade = Cur_G_Pay_Grade , Cur_G_Pay_Grade = 0 ,  ";
            //StrSql = StrSql + " Be_G_Pay_Check_Cnt = 0 , Cur_G_Pay_Check_Cnt = 0 , ";
            //StrSql = StrSql + " Be_G_Pay_Check_Date = Cur_G_Pay_Check_Date , Cur_G_Pay_Check_Date = '' ,";


            StrSql = StrSql + " Sum_Return_Take_Pay = 0 , Sum_Return_DedCut_Pay = 0 , Sum_Return_Remain_Pay = 0 , Cur_DedCut_Pay = 0 , " ;
            StrSql = StrSql + " Allowance1_Cut_S = 0 , SumAllAllowance_Be_Not = 0 , SumAllAllowance_Cut_Per = 0 ,";

            StrSql = StrSql + "  Etc_Pay = 0 ,SumAllAllowance_10000 = 0 , Allowance2_Cut = 0 , Allowance3_Cut = 0 ,SumAllAllowance_Cut = 0 , Allowance1_ = 0 , Allowance1_Cut_ = 0 ,"; 
            StrSql = StrSql + " Allowance1=0,Allowance2=0 , Allowance3=0 , Allowance4=0, Allowance5=0," ;
            StrSql = StrSql + " Allowance6=0,Allowance7=0,  Allowance8=0 , Allowance9=0, Allowance10=0," ;
    
            StrSql = StrSql + " SumAllAllowance=0," ;
            StrSql = StrSql + " InComeTax=0, ResidentTax=0,TruePayment=0 " ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02_Sell set " ;
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


        //private void MonthPrice_20160404(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        //{
        //    string StrSql = "";


        //    StrSql = "Select Mbid,Mbid2 ,OrderNumber, SellCode , TotalPrice , InputCash , InputCard , InputPassbook , TotalPV , TotalCV , Re_BaseOrderNumber ,SellDate_2 SellDate_2  ";
        //    StrSql = StrSql + " From    tbl_SalesDetail  (nolock)  ";
        //    StrSql = StrSql + " Where   SellDate_2 >= '" + FromEndDate.Substring(0, 6).ToString() + "' + '01' ";
        //    StrSql = StrSql + " And     SellDate_2 <= '" + ToEndDate + "'";
        //    StrSql = StrSql + " And     TotalPV  + TotalCV < 0 ";
        //    StrSql = StrSql + " And     SellCode <> '' ";


        //    DataSet Dset = new DataSet();
        //    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset);
        //    ReCnt = 0;
        //    ReCnt = Search_Connect.DataSet_ReCount;


        //    pg1.Value = 0; pg1.Maximum = ReCnt;
        //    string Re_BaseOrderNumber = "", T_SellDate_2 = "", RePayDate = "", Rs_SellDate_2 = "";

        //    for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
        //    {
        //        Re_BaseOrderNumber = Dset.Tables[base_db_name].Rows[fi_cnt]["Re_BaseOrderNumber"].ToString();
        //        Rs_SellDate_2 = Dset.Tables[base_db_name].Rows[fi_cnt]["SellDate_2"].ToString();

        //        T_SellDate_2 = ""; RePayDate = "";

        //        StrSql = "Select Mbid,Mbid2, OrderNumber, TotalPV , SellDate_2 SellDate_2   From tbl_SalesDetail   (nolock) ";
        //        StrSql = StrSql + " Where OrderNumber ='" + Re_BaseOrderNumber + "'";

        //        DataSet Dset2 = new DataSet();
        //        Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset2);
        //        int ReCnt2 = Search_Connect.DataSet_ReCount;
        //        if (ReCnt2 > 0)
        //        {
        //            T_SellDate_2 = Dset2.Tables[base_db_name].Rows[0]["SellDate_2"].ToString();
        //        }


        //        if (T_SellDate_2 != "")
        //        {
        //            StrSql = "Select ToEndDate,  PayDate   From  tbl_CloseTotal_02 (nolock) ";
        //            StrSql = StrSql + " Where FromEndDate <='" + T_SellDate_2 + "'";
        //            StrSql = StrSql + " And   ToEndDate >='" + T_SellDate_2 + "'";

        //            DataSet Dset3 = new DataSet();
        //            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset3);
        //            int ReCnt3 = Search_Connect.DataSet_ReCount;
        //            if (ReCnt3 > 0)
        //            {
        //                RePayDate = Dset3.Tables[base_db_name].Rows[0]["PayDate"].ToString();
        //            }
        //        }

        //        if (RePayDate != "")
        //        {
        //            if (int.Parse(Rs_SellDate_2) > int.Parse(RePayDate))
        //            {
        //                StrSql = "Update tbl_ClosePay_02 SET ";
        //                StrSql = StrSql + "  MonthPrice = ISNULL(MonthPrice, 0) + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
        //                StrSql = StrSql + "  Where Mbid  = '" + Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "'";
        //                StrSql = StrSql + "  And   Mbid2 =  " + Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();

        //                Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //            }
        //        }

        //        pg1.PerformStep(); pg1.Refresh();
        //    }


        //     //2016-12-20일 금액에서 pv 로 변경 처리함  표홍선 부장 통화를 한후에
        //    pg1.Value = 0; pg1.Maximum = 2;
        //    pg1.PerformStep(); pg1.Refresh();

        //    StrSql = " Update tbl_ClosePay_02 SET";
        //    StrSql = StrSql + " MonthPrice = ISNULL(MonthPrice, 0) + IsNull(B.A1, 0)";
        //    StrSql = StrSql + " FROM  tbl_ClosePay_02  A,";

        //    StrSql = StrSql + " (";
        //    StrSql = StrSql + " Select  ";
        //    StrSql = StrSql + " Sum(BS1.TotalPV) + Isnull(Sum(Bs_R.TotalPV),0)  AS A1, ";
        //    StrSql = StrSql + " BS1.Mbid,BS1.Mbid2 ";
        //    StrSql = StrSql + " From tbl_SalesDetail AS BS1 (nolock) ";
        //    //StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate_2 <= '" + PayDate + "'";
        //    StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
        //    StrSql = StrSql + " Where   BS1.SellDate_2 >= '" + FromEndDate.Substring(0, 6).ToString() + "' + '01' ";
        //    StrSql = StrSql + " And     BS1.SellDate_2 <= '" + ToEndDate + "'";
        //    StrSql = StrSql + " And     BS1.TotalPV  + BS1.TotalCV >= 0 ";
        //    StrSql = StrSql + " And     BS1.SellCode <> '' ";
        //    StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
        //    StrSql = StrSql + " Group By BS1.Mbid,BS1.Mbid2";
        //    StrSql = StrSql + " Having Sum(BS1.TotalPV) + IsNull(Sum(Bs_R.TotalPV), 0) >= 0";
        //    StrSql = StrSql + " ) B";
        //    StrSql = StrSql + " Where a.Mbid = b.Mbid ";
        //    StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

        //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
        //    pg1.PerformStep(); pg1.Refresh();

        //}



        private void OneGrade60_20160404(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string StrSql = "";
            int Cnt = MaxLevel;

            pg1.Value = 0; pg1.Maximum  = (MaxLevel * 4) + 5 ;
            pg1.PerformStep(); pg1.Refresh();

            
            while (Cnt >= 0)
            {

                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + " Cur_10_Cnt_1 =  ISNULL(B.A2,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + "Select    Sum(Cur_10_Cnt_1 + Cur_10_Cnt_2  ) A2 ";
                StrSql = StrSql + " ,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_02 ";
                StrSql = StrSql + " Where (  Cur_10_Cnt_1 + Cur_10_Cnt_2  ) <>0   ";
                StrSql = StrSql + " And   LevelCnt =" + Cnt;
                StrSql = StrSql + " And   LineCnt =  1 ";
                StrSql = StrSql + " Group By Saveid,Saveid2   ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + " Cur_10_Cnt_1 =  Cur_10_Cnt_1 + ISNULL(B.A2,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + "Select    Count(Mbid ) A2 ";
                StrSql = StrSql + " ,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_02 ";
                StrSql = StrSql + " Where ISNULL(MonthPrice, 0) >= 100000   ";
                StrSql = StrSql + " And   LevelCnt =" + Cnt;
                StrSql = StrSql + " And   LineCnt =  1 ";
                StrSql = StrSql + " Group By Saveid,Saveid2   ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();



                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + " Cur_10_Cnt_2 =  ISNULL(B.A2,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + "Select    Sum(Cur_10_Cnt_1 + Cur_10_Cnt_2  ) A2 ";
                StrSql = StrSql + " ,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_02 ";
                StrSql = StrSql + " Where (  Cur_10_Cnt_1 + Cur_10_Cnt_2   ) <>0   ";
                StrSql = StrSql + " And   LevelCnt =" + Cnt;
                StrSql = StrSql + " And   LineCnt =  2 ";
                StrSql = StrSql + " Group By Saveid,Saveid2   ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();


                StrSql = "Update tbl_ClosePay_02 SET ";
                StrSql = StrSql + " Cur_10_Cnt_2 =  Cur_10_Cnt_2 + ISNULL(B.A2,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + "Select    Count(Mbid ) A2 ";
                StrSql = StrSql + " ,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_02 ";
                StrSql = StrSql + " Where ISNULL(MonthPrice, 0) >= 100000   ";
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


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + "  OneGrade = 60 ";
            StrSql = StrSql + " Where Cur_10_Cnt_1 >= 25 ";
            StrSql = StrSql + " And  Cur_10_Cnt_2 >= 25 ";
            StrSql = StrSql + " And  CurGrade >= 60";
            StrSql = StrSql + " And  OneGrade < 60";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



        }





        private void Retry_ToEndDate(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();
            string Start_Date = "" ,End_Date = "" ;
            string StrSql = "";

            Retry_ToEndDate_Search(ref Start_Date, ref End_Date);

            if (Start_Date == "" && End_Date == "") return; 


            //Retry_ToEndDate_Make_Table(Temp_Connect, Conn, tran, Start_Date, End_Date );


            string Retry_ToEndDate = Start_Date;
            int G_Cnt = 0;
            while (Retry_ToEndDate != "" && int.Parse(Retry_ToEndDate) <= int.Parse(End_Date))
            {
                Retry_MaxLevel = 0;
                Retry_N_MaxLevel = 0;

                Retry_ToEndDate_Make_Table(Temp_Connect, Conn, tran, Retry_ToEndDate); 

                Retry_Put_LevelCnt_Update(Temp_Connect, Conn, tran, Retry_ToEndDate);
                Retry_Put_LevelCnt_Update_Nom(Temp_Connect, Conn, tran, Retry_ToEndDate);
                
                Retry_ReqTF1(Temp_Connect, Conn, tran, Retry_ToEndDate);
                Retry_Put_Down_SumPV(Temp_Connect, Conn, tran, Retry_ToEndDate);
                Retry_GradeUpLine_ReqTF1(Temp_Connect, Conn, tran, Retry_ToEndDate);

                int S_LevelCnt = -1; // Retry_N_MaxLevel;

                //while (0 <= S_LevelCnt)
                //{
                //    if (S_LevelCnt < MaxLevel)
                //    {
                //        pg2.Maximum = pg2.Maximum + 13;
                //    }
                    //문제 : SellCV01   ㅠㅠ

                
                    Retry_GiveGrade1(Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);                    
                    pg2.PerformStep(); pg2.Refresh();

                
                    Retry_GiveGrade2(Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);                
                    pg2.PerformStep(); pg2.Refresh();

                Re_Grade_30:
                    Retry_GradeUpLine__3(10, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                    Retry_GiveGrade3(Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                    
                    if (Retry_Check_UP_Grade_TF(30, Temp_Connect, Conn, tran, Retry_ToEndDate) == true) goto Re_Grade_30;
                    pg2.PerformStep(); pg2.Refresh();

                Re_Grade_40:
                    Retry_GradeUpLine__3(20, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                    Retry_GiveGrade4(Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                    
                    if (Retry_Check_UP_Grade_TF(40, Temp_Connect, Conn, tran, Retry_ToEndDate) == true) goto Re_Grade_40;
                    pg2.PerformStep(); pg2.Refresh();

                Re_Grade_50:
                    Retry_GradeUpLine__3(30, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                    Retry_GiveGrade5(Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                
                    if (Retry_Check_UP_Grade_TF(50, Temp_Connect, Conn, tran, Retry_ToEndDate) == true) goto Re_Grade_50;
                    pg2.PerformStep(); pg2.Refresh();

                Re_Grade_60:
                    Retry_GradeUpLine__3(40, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                    Retry_GiveGrade6(Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                
                    if (Retry_Check_UP_Grade_TF(60, Temp_Connect, Conn, tran, Retry_ToEndDate) == true) goto Re_Grade_60;
                    pg2.PerformStep(); pg2.Refresh();

                Re_Grade_70:
                    Retry_GradeUpLine__3(50, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                    Retry_GiveGrade7(Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                
                    if (Retry_Check_UP_Grade_TF(70, Temp_Connect, Conn, tran, Retry_ToEndDate) == true) goto Re_Grade_70;
                    pg2.PerformStep(); pg2.Refresh();


                Re_Grade_80:
                    Retry_GradeUpLine__3(60, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                    Retry_GiveGrade8(Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                
                    if (Retry_Check_UP_Grade_TF(80, Temp_Connect, Conn, tran, Retry_ToEndDate) == true) goto Re_Grade_80;
                    pg2.PerformStep(); pg2.Refresh();


                Re_Grade_90:
                    Retry_GradeUpLine__3(70, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                    Retry_GiveGrade9(Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                
                    if (Retry_Check_UP_Grade_TF(90, Temp_Connect, Conn, tran, Retry_ToEndDate) == true) goto Re_Grade_90;
                    pg2.PerformStep(); pg2.Refresh();

                Re_Grade_100:
                    Retry_GradeUpLine__3(80, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                    Retry_GiveGrade10(Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                
                    if (Retry_Check_UP_Grade_TF(100, Temp_Connect, Conn, tran, Retry_ToEndDate) == true) goto Re_Grade_100;
                    pg2.PerformStep(); pg2.Refresh();

                Re_Grade_110:
                    Retry_GradeUpLine__3(90, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                    Retry_GiveGrade11(Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                
                    if (Retry_Check_UP_Grade_TF(110, Temp_Connect, Conn, tran, Retry_ToEndDate) == true) goto Re_Grade_110;
                    pg2.PerformStep(); pg2.Refresh();

                Re_Grade_120:
                    Retry_GradeUpLine__3(100, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                    Retry_GiveGrade12(Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);                
                    if (Retry_Check_UP_Grade_TF(120, Temp_Connect, Conn, tran, Retry_ToEndDate) == true) goto Re_Grade_120;
                    pg2.PerformStep(); pg2.Refresh();


                //    Retry_GradeUpLine__3(10, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                //    Retry_GradeUpLine__3(20, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                //    Retry_GradeUpLine__3(30, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                //    Retry_GradeUpLine__3(40, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                //    Retry_GradeUpLine__3(50, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                //    Retry_GradeUpLine__3(60, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                //    Retry_GradeUpLine__3(70, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                //    Retry_GradeUpLine__3(80, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                //    Retry_GradeUpLine__3(90, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                //    Retry_GradeUpLine__3(100, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                //    Retry_GradeUpLine__3(110, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);
                //    Retry_GradeUpLine__3(120, Temp_Connect, Conn, tran, S_LevelCnt, Retry_ToEndDate);

                //    S_LevelCnt--;
                //}


                
                

                StrSql = "";                                
                StrSql = "Select Isnull(Min(ToEndDate),'')   From  tbl_CloseTotal_02 (nolock) ";
                StrSql = StrSql + "  Where ToEndDate <> '" + ToEndDate + "' And  ToEndDate > '" + Retry_ToEndDate + "'";                

                DataSet Dset3 = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset3);
                int ReCnt3 = Search_Connect.DataSet_ReCount;
                if (ReCnt3 > 0)
                {
                    Retry_ToEndDate = Dset3.Tables[base_db_name].Rows[0][0].ToString();
                }

            }


            //여기서 다시 한번 도는 이유는 재계산된 직급을 가지고도.. 승급 여부를 따지기 위함.
            //이후 승급이 다시 되엇으면 날자를 조정하고... 승급 보너스를 환수 하지 않기 위해서.
            Retry_ToEndDate = Start_Date;
            G_Cnt = 0;
            
            while (Retry_ToEndDate != "" && int.Parse(Retry_ToEndDate) <= int.Parse(End_Date))
            {
                Put_Return_Pay_All_4(Temp_Connect, Conn, tran, Retry_ToEndDate, End_Date);   //올스타팩보너스 1차분에 대한 환급 처리를 진행한다.
                               
                 StrSql = "Select Isnull(Min(ToEndDate),'')   From  tbl_CloseTotal_02 (nolock) ";
                StrSql = StrSql + "  Where ToEndDate <> '" + ToEndDate + "'" ;
                StrSql = StrSql + "  And  ToEndDate > '" + Retry_ToEndDate + "'";         

                DataSet Dset3 = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset3);
                int ReCnt3 = Search_Connect.DataSet_ReCount;
                if (ReCnt3 > 0)
                {
                    Retry_ToEndDate = Dset3.Tables[base_db_name].Rows[0][0].ToString();
                }
            }


            StrSql = "insert into tbl_ClosePay_02_Mod_Retry_Back Select * From  tbl_ClosePay_02_Mod_Retry (nolock) ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Delete From  tbl_ClosePay_02_Mod_Retry ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

        }

        private void Put_Return_Pay_All_4(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string Retry_ToEndDate, string End_Date) 
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();

            string StrSql = "";

            //직급이 어긋난 사람중에서 올스타팩보너스가 발생을 했다.
            StrSql = "Select C2_R.Mbid,C2_R.Mbid2 ,C2_R.OneGrade Re_OneGrade, C2.OneGrade Real_OneGrade,   C2.Allowance4, C2.M_Name  ";
            StrSql = StrSql + " From    tbl_ClosePay_02_Mod_Retry C2_R  (nolock)  ";
            StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_02_Mod C2  (nolock) ON C2.Mbid = C2_R.Mbid And C2.Mbid2 = C2_R.Mbid2 And C2.ToEndDate = C2_R.Retry_ToEndDate  ";
            StrSql = StrSql + " Where   C2_R.ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " And     C2_R.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And     C2_R.OneGrade < C2.OneGrade ";
            StrSql = StrSql + " And     C2.Allowance4 > 0 ";
            

            DataSet Dset = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset);
            ReCnt = 0;
            ReCnt = Search_Connect.DataSet_ReCount;


            pg1.Value = 0; pg1.Maximum = ReCnt;           

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                string Mbid = Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                int Mbid2 = int.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                string M_Name = Dset.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();


                int Real_OneGrade = int.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["Real_OneGrade"].ToString());
                double Allowance4 = double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["Allowance4"].ToString());  //32 공제률이 적용된 내역임...Allowance4_R  이 실제 발생된 수당이구


                StrSql = "Select Top 1 Grade_FLAG_Cnt , Give_Pay , Cut_FLAG    ";
                StrSql = StrSql + " From    tbl_ClosePay_02_G_FLAG C2  (nolock)  ";
                StrSql = StrSql + " Where   C2.Mbid = '" + Mbid + "'";
                StrSql = StrSql + " And     C2.Mbid2 = " + Mbid2;
                StrSql = StrSql + " And     C2.Grade_FLAG  = " + Real_OneGrade;
                StrSql = StrSql + " And     C2.Grade_FLAG_2 = 1 ";
                StrSql = StrSql + " Order by Grade_FLAG_Cnt DESC  ";

                DataSet Ds_FL = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Ds_FL);
                int ReCnt_2 = 0;
                ReCnt_2 = Search_Connect.DataSet_ReCount;

                int Grade_FLAG_Cnt = int.Parse(Ds_FL.Tables[base_db_name].Rows[0]["Grade_FLAG_Cnt"].ToString());
                double Allowance4_Give_Pay = double.Parse(Ds_FL.Tables[base_db_name].Rows[0]["Give_Pay"].ToString());
                string Cut_FLAG = Ds_FL.Tables[base_db_name].Rows[0]["Cut_FLAG"].ToString();
                     

                ////재계산된 반품 마감상에서 현 반품처리로 떨어진 직급과 동일한 직급이상이 된적이 있는가?
                StrSql = "Select Top 1  Retry_ToEndDate , OneGrade    ";
                StrSql = StrSql + " From    tbl_ClosePay_02_Mod_Retry C2  (nolock)  ";
                StrSql = StrSql + " Where   C2.Mbid = '" + Mbid + "'";
                StrSql = StrSql + " And     C2.Mbid2 = " + Mbid2;
                StrSql = StrSql + " And     C2.ToEndDate = '" + ToEndDate + "'";
                StrSql = StrSql + " And     C2.Retry_ToEndDate > '" + Retry_ToEndDate + "'";
                StrSql = StrSql + " And     C2.OneGrade  >= " + Real_OneGrade;
                StrSql = StrSql + " Order by Retry_ToEndDate ASC "; 

                DataSet Ds_2 = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Ds_2);
                ReCnt_2 = 0;
                ReCnt_2 = Search_Connect.DataSet_ReCount;

                if (ReCnt_2 > 0)
                {
                    string ToEndDate_RR = Ds_2.Tables[base_db_name].Rows[0]["Retry_ToEndDate"].ToString();

                    Grade_FLAG_Cnt++; 

                    StrSql = " INSERT INTO tbl_ClosePay_02_G_FLAG ";
                    StrSql = StrSql + " (Mbid , Mbid2 , Grade_FLAG , Give_ToEndDate,Grade_FLAG_2 , Give_Pay, Grade_FLAG_Cnt, Grade_FLAG_ToEndDate) ";
                    StrSql = StrSql + " Values  (  ";
                    StrSql = StrSql + "'" + Mbid + "'";
                    StrSql = StrSql + "," + Mbid2;
                    StrSql = StrSql + "," + Real_OneGrade;
                    StrSql = StrSql + ",'" + ToEndDate_RR + "'";                    
                    StrSql = StrSql + ", 1 " ;
                    StrSql = StrSql + ", 0 ";
                    StrSql = StrSql + "," + Grade_FLAG_Cnt;
                    StrSql = StrSql + ",'" + ToEndDate + "'";                                        
                    StrSql = StrSql + " )";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);


                    StrSql = " Update tbl_ClosePay_02 SET  ";
                    if (Real_OneGrade == 20) StrSql = StrSql + " GradeDate2 = '" + ToEndDate_RR + "'";
                    if (Real_OneGrade == 30) StrSql = StrSql + " GradeDate3 = '" + ToEndDate_RR + "'";
                    if (Real_OneGrade == 40) StrSql = StrSql + " GradeDate4 = '" + ToEndDate_RR + "'";
                    if (Real_OneGrade == 50) StrSql = StrSql + " GradeDate5 = '" + ToEndDate_RR + "'"; 
                    if (Real_OneGrade == 60) StrSql = StrSql + " GradeDate6 = '" + ToEndDate_RR + "'";
                    if (Real_OneGrade == 70) StrSql = StrSql + " GradeDate7 = '" + ToEndDate_RR + "'";
                    if (Real_OneGrade == 80) StrSql = StrSql + " GradeDate8 = '" + ToEndDate_RR + "'";
                    if (Real_OneGrade == 90) StrSql = StrSql + " GradeDate9 = '" + ToEndDate_RR + "'";
                    if (Real_OneGrade == 100) StrSql = StrSql + " GradeDate10 = '" + ToEndDate_RR + "'";
                    if (Real_OneGrade == 110) StrSql = StrSql + " GradeDate11 = '" + ToEndDate_RR + "'";
                    if (Real_OneGrade == 120) StrSql = StrSql + " GradeDate12 = '" + ToEndDate_RR + "'";                    
                    StrSql = StrSql + " Where   Mbid = '" + Mbid + "'";
                    StrSql = StrSql + " And     Mbid2 = " + Mbid2;

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                }
                else 
                {

                    StrSql = "Select Top 1  ToEndDate , OneGrade    ";
                    StrSql = StrSql + " From    tbl_ClosePay_02_Mod C2  (nolock)  ";
                    StrSql = StrSql + " Where   C2.Mbid = '" + Mbid + "'";
                    StrSql = StrSql + " And     C2.Mbid2 = " + Mbid2;
                    StrSql = StrSql + " And     C2.ToEndDate > '" + End_Date + "'";  //재정산 이후의 마감중에서... 승급이 된 내역이 잇는가. 체크를 한다.
                    StrSql = StrSql + " And     C2.OneGrade  >= " + Real_OneGrade;
                    StrSql = StrSql + " And     C2.ToEndDate in (sELECT ToEndDAte From tbl_CloseTotal_02 (nolock) Where Real_FLAG = 0) ";
                    StrSql = StrSql + " Order by ToEndDate ASC ";

                    DataSet Ds_33 = new DataSet();
                    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Ds_33);
                    ReCnt_2 = 0;
                    ReCnt_2 = Search_Connect.DataSet_ReCount;

                    if (ReCnt_2 > 0)
                    {
                        string ToEndDate_RR = Ds_33.Tables[base_db_name].Rows[0]["ToEndDate"].ToString();

                        Grade_FLAG_Cnt++; 
                        StrSql = " INSERT INTO tbl_ClosePay_02_G_FLAG ";
                        StrSql = StrSql + " (Mbid , Mbid2 , Grade_FLAG , Give_ToEndDate,Grade_FLAG_2 , Give_Pay, Grade_FLAG_Cnt, Grade_FLAG_ToEndDate) ";
                        StrSql = StrSql + " Values  (  ";
                        StrSql = StrSql + "'" + Mbid + "'";
                        StrSql = StrSql + "," + Mbid2;
                        StrSql = StrSql + "," + Real_OneGrade;
                        StrSql = StrSql + ",'" + ToEndDate_RR + "'";
                        StrSql = StrSql + ", 1 ";
                        StrSql = StrSql + ", 0 ";
                        StrSql = StrSql + "," + Grade_FLAG_Cnt;
                        StrSql = StrSql + ",'" + ToEndDate + "'";
                        StrSql = StrSql + " )";

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);


                        StrSql = " Update tbl_ClosePay_02 SET  ";
                        if (Real_OneGrade == 20) StrSql = StrSql + " GradeDate2 = '" + ToEndDate_RR + "'";
                        if (Real_OneGrade == 30) StrSql = StrSql + " GradeDate3 = '" + ToEndDate_RR + "'";
                        if (Real_OneGrade == 40) StrSql = StrSql + " GradeDate4 = '" + ToEndDate_RR + "'";
                        if (Real_OneGrade == 50) StrSql = StrSql + " GradeDate5 = '" + ToEndDate_RR + "'";
                        if (Real_OneGrade == 60) StrSql = StrSql + " GradeDate6 = '" + ToEndDate_RR + "'";
                        if (Real_OneGrade == 70) StrSql = StrSql + " GradeDate7 = '" + ToEndDate_RR + "'";
                        if (Real_OneGrade == 80) StrSql = StrSql + " GradeDate8 = '" + ToEndDate_RR + "'";
                        if (Real_OneGrade == 90) StrSql = StrSql + " GradeDate9 = '" + ToEndDate_RR + "'";
                        if (Real_OneGrade == 100) StrSql = StrSql + " GradeDate10 = '" + ToEndDate_RR + "'";
                        if (Real_OneGrade == 110) StrSql = StrSql + " GradeDate11 = '" + ToEndDate_RR + "'";
                        if (Real_OneGrade == 120) StrSql = StrSql + " GradeDate12 = '" + ToEndDate_RR + "'";
                        StrSql = StrSql + " Where   Mbid = '" + Mbid + "'";
                        StrSql = StrSql + " And     Mbid2 = " + Mbid2;

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    }
                    else
                    {
                        StrSql = "Select  OneGrade    ";
                        StrSql = StrSql + " From    tbl_ClosePay_02 C2  (nolock)  ";
                        StrSql = StrSql + " Where   C2.Mbid = '" + Mbid + "'";
                        StrSql = StrSql + " And     C2.Mbid2 = " + Mbid2;                        
                        StrSql = StrSql + " And     C2.OneGrade  >= " + Real_OneGrade;
                        
                        DataSet Ds_333 = new DataSet();
                        Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Ds_333);
                        ReCnt_2 = 0;
                        ReCnt_2 = Search_Connect.DataSet_ReCount;

                        if (ReCnt_2 > 0)
                        {
                            //string ToEndDate_RR = Ds_33.Tables[base_db_name].Rows[0]["ToEndDate"].ToString();

                            Grade_FLAG_Cnt++ ;

                            StrSql = " INSERT INTO tbl_ClosePay_02_G_FLAG ";
                            StrSql = StrSql + " (Mbid , Mbid2 , Grade_FLAG , Give_ToEndDate,Grade_FLAG_2 , Give_Pay, Grade_FLAG_Cnt, Grade_FLAG_ToEndDate) ";
                            StrSql = StrSql + " Values  (  ";
                            StrSql = StrSql + "'" + Mbid + "'";
                            StrSql = StrSql + "," + Mbid2;
                            StrSql = StrSql + "," + Real_OneGrade;
                            StrSql = StrSql + ",'" + ToEndDate + "'";
                            StrSql = StrSql + ", 1 ";
                            StrSql = StrSql + ", 0 ";
                            StrSql = StrSql + "," + Grade_FLAG_Cnt ;
                            StrSql = StrSql + ",'" + ToEndDate + "'";
                            StrSql = StrSql + " )";

                            Temp_Connect.Insert_Data(StrSql, Conn, tran);


                            StrSql = " Update tbl_ClosePay_02 SET  ";
                            if (Real_OneGrade == 20) StrSql = StrSql + " GradeDate2 = '" + ToEndDate + "'";
                            if (Real_OneGrade == 30) StrSql = StrSql + " GradeDate3 = '" + ToEndDate + "'";
                            if (Real_OneGrade == 40) StrSql = StrSql + " GradeDate4 = '" + ToEndDate + "'";
                            if (Real_OneGrade == 50) StrSql = StrSql + " GradeDate5 = '" + ToEndDate + "'";
                            if (Real_OneGrade == 60) StrSql = StrSql + " GradeDate6 = '" + ToEndDate + "'";
                            if (Real_OneGrade == 70) StrSql = StrSql + " GradeDate7 = '" + ToEndDate + "'";
                            if (Real_OneGrade == 80) StrSql = StrSql + " GradeDate8 = '" + ToEndDate + "'";
                            if (Real_OneGrade == 90) StrSql = StrSql + " GradeDate9 = '" + ToEndDate + "'";
                            if (Real_OneGrade == 100) StrSql = StrSql + " GradeDate10 = '" + ToEndDate + "'";
                            if (Real_OneGrade == 110) StrSql = StrSql + " GradeDate11 = '" + ToEndDate + "'";
                            if (Real_OneGrade == 120) StrSql = StrSql + " GradeDate12 = '" + ToEndDate + "'";
                            StrSql = StrSql + " Where   Mbid = '" + Mbid + "'";
                            StrSql = StrSql + " And     Mbid2 = " + Mbid2;

                            Temp_Connect.Insert_Data(StrSql, Conn, tran);

                        }
                        else
                        {
                            //더높은 직급이 없는 경우에는 기존 승급 일자를 없애버린다.
                            //그럼 추후에 다시 승급하게되면 일자가 밖히면서 다시 승급 수당이 발생을 하게 됨.
                            StrSql = " Update tbl_ClosePay_02 SET  ";
                            if (Real_OneGrade == 20) StrSql = StrSql + " GradeDate2 = ''";
                            if (Real_OneGrade == 30) StrSql = StrSql + " GradeDate3 = ''";
                            if (Real_OneGrade == 40) StrSql = StrSql + " GradeDate4 = ''";
                            if (Real_OneGrade == 50) StrSql = StrSql + " GradeDate5 = ''";
                            if (Real_OneGrade == 60) StrSql = StrSql + " GradeDate6 = ''";
                            if (Real_OneGrade == 70) StrSql = StrSql + " GradeDate7 = ''";
                            if (Real_OneGrade == 80) StrSql = StrSql + " GradeDate8 = ''";
                            if (Real_OneGrade == 90) StrSql = StrSql + " GradeDate9 = ''";
                            if (Real_OneGrade == 100) StrSql = StrSql + " GradeDate10 = ''";
                            if (Real_OneGrade == 110) StrSql = StrSql + " GradeDate11 = ''";
                            if (Real_OneGrade == 120) StrSql = StrSql + " GradeDate12 = ''";
                            StrSql = StrSql + " Where   Mbid = '" + Mbid + "'";
                            StrSql = StrSql + " And     Mbid2 = " + Mbid2;

                            Temp_Connect.Insert_Data(StrSql, Conn, tran);


                            if (Allowance4_Give_Pay > 0 && Cut_FLAG == "")  //올스타팩보너스를 받은 거에 대해서만 반품 환수 처리를 한다. 그리고 반품으로 체크가 풀린 내역은 넣지 않는다.
                            {
                                StrSql = " INSERT INTO tbl_Sales_Put_Return_Pay ";
                                StrSql = StrSql + " (ToEndDate,OrderNumber,Re_BaseOrderNumber,C_Mbid,C_Mbid2, C_M_Name ,R_Mbid,R_Mbid2, R_M_Name , SellDate , Return_Pay, Return_Pay2, Cl_TF , SortOrder )";
                                StrSql = StrSql + " Values  (  ";
                                StrSql = StrSql + "'" + ToEndDate + "','C1'";
                                StrSql = StrSql + ",'C1'";
                                StrSql = StrSql + ",'" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2;
                                StrSql = StrSql + ",'" + M_Name + "'";
                                StrSql = StrSql + ",'" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2;
                                StrSql = StrSql + ",'" + M_Name + "'";
                                StrSql = StrSql + ",'" + ToEndDate + "'," + Allowance4 + " , " + Allowance4 + " ,2,  'W4' )";


                                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                            }
                        }
                    }
                    
                }


                StrSql = " Update tbl_ClosePay_02_G_FLAG SET";
                StrSql = StrSql + " Cut_FLAG = '" + ToEndDate + "' ";
                StrSql = StrSql + " Where Give_ToEndDate = '" + Retry_ToEndDate + "'";
                StrSql = StrSql + " And   Mbid = '" + Mbid + "'";
                StrSql = StrSql + " And   Mbid2 = " + Mbid2;
                StrSql = StrSql + " And   Grade_FLAG ='" + Real_OneGrade + "'";
                StrSql = StrSql + " And   Grade_FLAG_Cnt = " + Grade_FLAG_Cnt;
                StrSql = StrSql + " And   Cut_FLAG = '' ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);

                pg1.PerformStep(); pg1.Refresh();

            }

        }


        private void Retry_ToEndDate_Search( ref string Start_Date , ref string End_Date )
        {
            Start_Date = "";
            End_Date = "";
            int T_SW = 0; 

            string StrSql = "";

            string Min_Date = "30001231", Max_Date = "0";  


            StrSql = "Select Mbid,Mbid2 ,OrderNumber, SellCode , TotalPrice , InputCash , InputCard , InputPassbook , TotalPV , TotalCV , Re_BaseOrderNumber ,SellDate_2 SellDate_2  ";
            StrSql = StrSql + " From    tbl_SalesDetail  (nolock)  ";
            StrSql = StrSql + " Where   SellDate_2 >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     TotalPV  + TotalCV < 0 ";
            StrSql = StrSql + " And     SellCode <> '' ";


            DataSet Dset = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset);
            ReCnt = 0;
            ReCnt = Search_Connect.DataSet_ReCount;


            pg1.Value = 0; pg1.Maximum = ReCnt;
            string Re_BaseOrderNumber = "", T_SellDate_2 = "", RePayDate = "", Rs_SellDate_2 = "";

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Re_BaseOrderNumber = Dset.Tables[base_db_name].Rows[fi_cnt]["Re_BaseOrderNumber"].ToString();
                Rs_SellDate_2 = Dset.Tables[base_db_name].Rows[fi_cnt]["SellDate_2"].ToString();

                T_SellDate_2 = ""; RePayDate = "";

                StrSql = "Select Mbid,Mbid2, OrderNumber, TotalPV , SellDate_2   From tbl_SalesDetail   (nolock) ";
                StrSql = StrSql + " Where OrderNumber ='" + Re_BaseOrderNumber + "'";

                DataSet Dset2 = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset2);
                int ReCnt2 = Search_Connect.DataSet_ReCount;
                if (ReCnt2 > 0)
                {
                    T_SellDate_2 = Dset2.Tables[base_db_name].Rows[0]["SellDate_2"].ToString();
                }


                if (T_SellDate_2 != "")
                {
                    StrSql = "Select ToEndDate,  PayDate   From  tbl_CloseTotal_02 (nolock) ";
                    StrSql = StrSql + " Where FromEndDate <='" + T_SellDate_2 + "'";
                    StrSql = StrSql + " And   ToEndDate >='" + T_SellDate_2 + "'";

                    DataSet Dset3 = new DataSet();
                    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset3);
                    int ReCnt3 = Search_Connect.DataSet_ReCount;
                    if (ReCnt3 > 0)
                    {
                        RePayDate = Dset3.Tables[base_db_name].Rows[0]["ToEndDate"].ToString();
                    }


                    if (RePayDate != "")
                    {
                        if (int.Parse(Rs_SellDate_2) > int.Parse(RePayDate))  //반품일자가.. 원주문이 돈 마감일자의 종료일자 이후에 들어온 매출이다.
                        {
                            if (int.Parse(Min_Date) > int.Parse(RePayDate)) Min_Date = RePayDate;
                            if (int.Parse(Max_Date) < int.Parse(RePayDate)) Max_Date = RePayDate;

                            T_SW = 1; 
                        }
                    }

                }
            }

            if (T_SW == 1)
            {
                StrSql = "Select Top 3 ToEndDate,  PayDate   From  tbl_CloseTotal_02 (nolock) ";
                StrSql = StrSql + "  Where ToEndDate <> '" + ToEndDate + "' And  FromEndDate >'" + Max_Date + "'";
                StrSql = StrSql + " Order by ToEndDate ASC  "; 

                DataSet Dset3 = new DataSet();
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset3);
                int ReCnt3 = Search_Connect.DataSet_ReCount;
                if (ReCnt3 > 0)
                {
                    Max_Date = Dset3.Tables[base_db_name].Rows[0]["ToEndDate"].ToString();

                    if (ReCnt3 >= 2 )
                        Max_Date = Dset3.Tables[base_db_name].Rows[1]["ToEndDate"].ToString();

                    if (ReCnt3 >= 3)
                        Max_Date = Dset3.Tables[base_db_name].Rows[2]["ToEndDate"].ToString();
                }

                Start_Date = Min_Date;
                End_Date = Max_Date;
            }



        }










        private void Retry_ToEndDate_Make_Table(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string Start_Date , string  End_Date )
        {
            pg1.Value = 0; pg1.Maximum = 2; 
            pg1.PerformStep(); pg1.Refresh();

            string StrSql = "";

            StrSql = "INSERT INTO tbl_ClosePay_02_Mod_Retry  ";
            StrSql = StrSql + " ( ";
            StrSql = StrSql + "  ToEndDate,Retry_ToEndDate,LeaveDate,Sell_Mem_TF, Mbid,Mbid2 ";
            StrSql = StrSql + " ,Saveid,Saveid2,LineCnt,LevelCnt ";
            StrSql = StrSql + " ,Nominid,Nominid2,N_LineCnt,N_LevelCnt ";
            StrSql = StrSql + " ,GradeDate1,GradeDate2,GradeDate3,GradeDate4,GradeDate5 ";
            StrSql = StrSql + " ,GradeDate6,GradeDate7,GradeDate8,GradeDate9,GradeDate10 ";
            StrSql = StrSql + " ,GradeDate11,GradeDate12 "; 
            StrSql = StrSql + "  )  ";
            StrSql = StrSql + " Select ";
            StrSql = StrSql + " '" + ToEndDate + "',ToEndDate,LeaveDate,Sell_Mem_TF, Mbid,Mbid2 ";
            StrSql = StrSql + " ,Saveid,Saveid2,LineCnt,LevelCnt ";
            StrSql = StrSql + " ,Nominid,Nominid2,N_LineCnt,N_LevelCnt ";

            StrSql = StrSql + " ,GradeDate1,GradeDate2,GradeDate3,GradeDate4,GradeDate5 ";
            StrSql = StrSql + " ,GradeDate6,GradeDate7,GradeDate8,GradeDate9,GradeDate10 ";
            StrSql = StrSql + " ,GradeDate11,GradeDate12 "; 

            StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock)  ";
            StrSql = StrSql + " Where ToEndDate >='" + Start_Date  + "'";
            StrSql = StrSql + " And   ToEndDate <='" + End_Date + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void Retry_ToEndDate_Make_Table(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();

            string StrSql = "";

            StrSql = "INSERT INTO tbl_ClosePay_02_Mod_Retry  ";
            StrSql = StrSql + " ( ";
            StrSql = StrSql + "  ToEndDate,Retry_ToEndDate,LeaveDate,Sell_Mem_TF, Mbid,Mbid2 ";
            StrSql = StrSql + " ,Saveid,Saveid2,LineCnt,LevelCnt ";
            StrSql = StrSql + " ,Nominid,Nominid2,N_LineCnt,N_LevelCnt ";
            StrSql = StrSql + " ,GradeDate1,GradeDate2,GradeDate3,GradeDate4,GradeDate5 ";
            StrSql = StrSql + " ,GradeDate6,GradeDate7,GradeDate8,GradeDate9,GradeDate10 ";
            StrSql = StrSql + " ,GradeDate11,GradeDate12 ";
            StrSql = StrSql + "  )  ";
            StrSql = StrSql + " Select ";
            StrSql = StrSql + " '" + ToEndDate + "',ToEndDate,LeaveDate,Sell_Mem_TF, Mbid,Mbid2 ";
            StrSql = StrSql + " ,Saveid,Saveid2,LineCnt,LevelCnt ";
            StrSql = StrSql + " ,Nominid,Nominid2,N_LineCnt,N_LevelCnt ";

            StrSql = StrSql + " ,GradeDate1,GradeDate2,GradeDate3,GradeDate4,GradeDate5 ";
            StrSql = StrSql + " ,GradeDate6,GradeDate7,GradeDate8,GradeDate9,GradeDate10 ";
            StrSql = StrSql + " ,GradeDate11,GradeDate12 ";

            StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock)  ";
            StrSql = StrSql + " Where ToEndDate ='" + Retry_ToEndDate + "'";            

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }


        private void Retry_ReqTF1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 8;
            pg1.PerformStep(); pg1.Refresh();

            string StrSql = "";


            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " SellPV01 =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " ,SellCV01 =  ISNULL(B.a2,0) ";
            StrSql = StrSql + " ,SellPrice01 =  ISNULL(B.a3,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 ";
            StrSql = StrSql + " , Sum(Se.TotalCV )  a2 ";
            StrSql = StrSql + " , Sum(Se.TotalPrice )  a3 ";
            StrSql = StrSql + " , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " WHERE   Se.SellDate_2  <='" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   (Se.TotalPV > 0 OR Se.ReturnTF = 3 ) ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where   A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            StrSql = StrSql + " And   A.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   A.ToEndDate = '" + ToEndDate + "'";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);




            StrSql = "select top 3 fromenddate , ToEndDate from dbo.tbl_CloseTotal_02 (nolock) " ;
            StrSql = StrSql + " Where ToEndDate < '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And  ToEndDate <> '" + ToEndDate + "'";
            StrSql = StrSql + " Order by ToEndDate DESC  ";

            string SDate3 = "", To_SDate3 = "", From_SDate2 = "", From_SDate1 = "", To_SDate2 = "", To_SDate1 = "";
            int ReCnt = 0;
            DataSet Dset4 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
            ReCnt = Search_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {  //이번주 포함 4주간의 매출 합산을 불러온다.  150
                SDate3 = Dset4.Tables[base_db_name].Rows[0][0].ToString();
                To_SDate3 = Dset4.Tables[base_db_name].Rows[0][1].ToString();

                StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                StrSql = StrSql + " W_3_QV_Real =  ISNULL(B.a1,0) ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + " Select   Sum(Se.TotalPV)  a1 , Se.Mbid , Se.Mbid2 ";
                StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
                //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
                StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + SDate3 + "'";
                StrSql = StrSql + " And   Se.SellDate_2  <='" + To_SDate3 + "'";
                StrSql = StrSql + " And   Se.Ga_Order = 0 ";
                StrSql = StrSql + " And   (Se.TotalPV > 0 OR Se.ReturnTF = 3 ) ";
                StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
                StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
                StrSql = StrSql + " And   A.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                StrSql = StrSql + " And   A.ToEndDate = '" + ToEndDate + "'";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                StrSql = StrSql + " Active_3_FLAG =  ISNULL(B.a1,0) ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + " Select   Mbid,Mbid2 , Case When  ReqTF1 = 1 then 'Y' ELSE '' End a1  ";
                StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
                StrSql = StrSql + " WHERE   ToEndDate ='" + To_SDate3 + "'";
                StrSql = StrSql + " ) B";
                StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
                StrSql = StrSql + " And   A.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                StrSql = StrSql + " And   A.ToEndDate = '" + ToEndDate + "'";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);



                if (ReCnt >= 2)
                {
                    From_SDate2 = Dset4.Tables[base_db_name].Rows[1][0].ToString();
                    To_SDate2 = Dset4.Tables[base_db_name].Rows[1][1].ToString();

                    StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                    StrSql = StrSql + " W_2_QV_Real =  ISNULL(B.a1,0) ";
                    StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                    StrSql = StrSql + " (";
                    StrSql = StrSql + " Select   Sum(Se.TotalPV)  a1 , Se.Mbid , Se.Mbid2 ";
                    StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
                    //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
                    StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + From_SDate2 + "'";
                    StrSql = StrSql + " And   Se.SellDate_2  <='" + To_SDate2 + "'";
                    StrSql = StrSql + " And   Se.Ga_Order = 0 ";
                    StrSql = StrSql + " And   (Se.TotalPV > 0 OR Se.ReturnTF = 3 ) ";
                    StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
                    StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
                    StrSql = StrSql + " ) B";
                    StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                    StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
                    StrSql = StrSql + " And   A.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                    StrSql = StrSql + " And   A.ToEndDate = '" + ToEndDate + "'";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);

                    StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                    StrSql = StrSql + " Active_2_FLAG =  ISNULL(B.a1,0) ";
                    StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                    StrSql = StrSql + " (";
                    StrSql = StrSql + " Select   Mbid,Mbid2 , Case When  ReqTF1 = 1 then 'Y' ELSE '' End a1  ";
                    StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
                    StrSql = StrSql + " WHERE   ToEndDate ='" + To_SDate2 + "'";
                    StrSql = StrSql + " ) B";
                    StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                    StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
                    StrSql = StrSql + " And   A.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                    StrSql = StrSql + " And   A.ToEndDate = '" + ToEndDate + "'";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);

                }

                if (ReCnt >= 3)
                {
                    From_SDate1 = Dset4.Tables[base_db_name].Rows[2][0].ToString();
                    To_SDate1 = Dset4.Tables[base_db_name].Rows[2][1].ToString();

                    StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                    StrSql = StrSql + " W_1_QV_Real =  ISNULL(B.a1,0) ";
                    StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                    StrSql = StrSql + " (";
                    StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 , Se.Mbid , Se.Mbid2 ";
                    StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
                    // StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
                    StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + From_SDate1 + "'";
                    StrSql = StrSql + " And   Se.SellDate_2  <='" + To_SDate1 + "'";
                    StrSql = StrSql + " And   Se.Ga_Order = 0 ";
                    StrSql = StrSql + " And   (Se.TotalPV > 0 OR Se.ReturnTF = 3 ) ";
                    StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
                    StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
                    StrSql = StrSql + " ) B";
                    StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                    StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);



                    StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                    StrSql = StrSql + " Active_1_FLAG =  ISNULL(B.a1,0) ";
                    StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                    StrSql = StrSql + " (";
                    StrSql = StrSql + " Select   Mbid,Mbid2 , Case When  ReqTF1 = 1 then 'Y' ELSE '' End a1  ";
                    StrSql = StrSql + " From tbl_ClosePay_02_Mod (nolock) ";
                    StrSql = StrSql + " WHERE   ToEndDate ='" + To_SDate1 + "'";

                    StrSql = StrSql + " ) B";
                    StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                    StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
                    StrSql = StrSql + " And   A.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                    StrSql = StrSql + " And   A.ToEndDate = '" + ToEndDate + "'";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                }
            }

            string  Retry_FromEndDate = "" ;

            StrSql = "select  fromenddate , ToEndDate from dbo.tbl_CloseTotal_02 Where ToEndDate = '" + Retry_ToEndDate  + "'";
            
            ReCnt = 0;
            DataSet DsetF = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, DsetF);
            ReCnt = Search_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {
                Retry_FromEndDate = DsetF.Tables[base_db_name].Rows[0][0].ToString();
            }



            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " W_4_QV_Real =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + Retry_FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   (Se.TotalPV > 0 OR Se.ReturnTF = 3 ) ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            StrSql = StrSql + " And   A.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   A.ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            if (From_SDate1 == "") From_SDate1 = From_SDate2;
            if (From_SDate1 == "") From_SDate1 = SDate3;
            if (From_SDate1 == "") From_SDate1 = Retry_FromEndDate;

            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " W4_QV_Real =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + From_SDate1 + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   (Se.TotalPV > 0 OR Se.ReturnTF = 3 ) ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            StrSql = StrSql + " And   A.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   A.ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " W4_QV =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + From_SDate1 + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   (Se.TotalPV > 0 OR Se.ReturnTF = 3 ) ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " And   Se.SellCode <> 'Auto' ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            StrSql = StrSql + " And   A.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   A.ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            //4주간의 오토쉽 합산을 불러온다.  100
            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " W4_QV_Auto =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + From_SDate1 + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   (Se.TotalPV > 0 OR Se.ReturnTF = 3 ) ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " And   Se.SellCode = 'Auto' ";
            StrSql = StrSql + " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            StrSql = StrSql + " And   A.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   A.ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //4주간의 직추천 우대고객의 매출을 불러온다.
            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " W4_QV_Down =  ISNULL(B.a1,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   Sum(Se.TotalPV )  a1 , tbl_Memberinfo.Nominid , tbl_Memberinfo.Nominid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            //StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalPV < 0  And  Bs_R.SellDate_2 <= '" + ToEndDate + "'";
            StrSql = StrSql + " LEFT JOIN  tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid  = tbl_Memberinfo.Mbid And tbl_Memberinfo.Mbid2  = tbl_Memberinfo.Mbid2 ";
            StrSql = StrSql + " WHERE   Se.SellDate_2  >='" + From_SDate1 + "'";
            StrSql = StrSql + " And   Se.SellDate_2  <='" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   (Se.TotalPV > 0 OR Se.ReturnTF = 3 ) ";
            StrSql = StrSql + " And   Se.OrderNumber not in (Select Re_BaseOrderNumber From tbl_SalesDetail (nolock) Where SellDate_2 <='" + ToEndDate + "' And ReturnTF = 2) ";
            StrSql = StrSql + " And   tbl_Memberinfo.Sell_Mem_TF = 1 ";
            StrSql = StrSql + " Group by tbl_Memberinfo.Nominid , tbl_Memberinfo.Nominid2 ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
            StrSql = StrSql + " And   A.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   A.ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //W4_QV  W4_QV_Auto  W4_QV_Down

            //ReqTF10 = 1 개별구매, ReqTF10 =2  오토쉽구매 ,  ReqTF10 = 3 직추천소비자 구매    
            //--오토쉽도 합산 처리를 한다 2017-12-01  ㅜㅜㅜ 
            StrSql = " Update tbl_ClosePay_02_Mod_Retry SET";
            StrSql = StrSql + " ReqTF1 = 1 ";
            StrSql = StrSql + " , ReqTF10 = 1 ";
            StrSql = StrSql + " Where  W4_QV + W4_QV_Auto  >= 150  ";
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = " Update tbl_ClosePay_02_Mod_Retry SET";
            StrSql = StrSql + " ReqTF1 = 1 ";
            StrSql = StrSql + " , ReqTF10 = 2 ";
            StrSql = StrSql + " Where  W4_QV_Auto >= 100  ";
            StrSql = StrSql + " And    ReqTF1 = 0   ";
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = " Update tbl_ClosePay_02_Mod_Retry SET";
            StrSql = StrSql + " ReqTF1 = 1 ";
            StrSql = StrSql + " , ReqTF10 = 3 ";
            StrSql = StrSql + " Where  (W4_QV_Down >= 300 And (W4_QV + W4_QV_Auto ) >= 1 )   ";
            StrSql = StrSql + " And    ReqTF1 = 0   ";
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " ReqDate1='" + ToEndDate + "'";
            StrSql = StrSql + " Where ReqDate1=''";
            StrSql = StrSql + " And ReqTF1 >= 1 ";
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //월요일날 적용해 주어야함.
//             IF @v_ToEndDate =  '20171016'  OR @v_ToEndDate = '20171023' OR @v_ToEndDate ='20171030'
//    BEGIN
//        Update tbl_ClosePay_02 SET  
//        ReqTF1 = 1   
//        , ReqTF10 = 1   
//        Where mbid2 in (1001306,1001714 )
//    END
	
//    --2017-01-16 천차장님 요청에 의해서 1주간 동안 1분에 대해서 자격조건 Active 를 강제로 준다.
//IF @v_ToEndDate =  '20171016' 
//    BEGIN
//        Update tbl_ClosePay_02 SET  
//        ReqTF1 = 1   
//        , ReqTF10 = 1   
//        Where mbid2 in (1002711)
//    END
	
//IF @v_ToEndDate =  '20171030'  OR @v_ToEndDate =  '20171106'  OR @v_ToEndDate =  '20171113' OR @v_ToEndDate =  '20171120' 
//    BEGIN
//        Update tbl_ClosePay_02 SET  
//        ReqTF1 = 1   
//        , ReqTF10 = 1   
//        Where mbid2 in (1006744)
//    END
	

//IF @v_ToEndDate =  '20171106' 
//    BEGIN
//        Update tbl_ClosePay_02 SET  
//        ReqTF1 = 1   
//        , ReqTF10 = 1   
//        Where mbid2 in (1001714)
//    END
        
        }






        private void Retry_Put_Down_SumPV(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string Retry_ToEndDate)
        {
            string StrSql = "";



            pg1.Value = 0; pg1.Maximum = Retry_N_MaxLevel + 4;
            pg1.PerformStep(); pg1.Refresh();

            int Cnt = Retry_N_MaxLevel;

            while (Cnt >= 0)
            {


                StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                StrSql = StrSql + " Down_W4_QV_Real =  ISNULL(B.A2,0)";
                StrSql = StrSql + " ,Down_G_Down =    ISNULL(B.A1,0)";
                StrSql = StrSql + " ,Down_W_1_QV_Real =    ISNULL(B.W_1,0)";
                StrSql = StrSql + " ,Down_W_2_QV_Real =    ISNULL(B.W_2,0)";
                StrSql = StrSql + " ,Down_W_3_QV_Real =    ISNULL(B.W_3,0)";
                StrSql = StrSql + " ,Down_W_4_QV_Real =    ISNULL(B.W_4,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + "Select Sum(Down_G_Down + SellPV01 ) A1  , Sum(Down_W4_QV_Real +  W4_QV_Real  ) A2 ";
                StrSql = StrSql + " ,Sum(Down_W_1_QV_Real + W_1_QV_Real ) W_1 ";
                StrSql = StrSql + " ,Sum(Down_W_2_QV_Real + W_2_QV_Real ) W_2 ";
                StrSql = StrSql + " ,Sum(Down_W_3_QV_Real + W_3_QV_Real ) W_3 ";
                StrSql = StrSql + " ,Sum(Down_W_4_QV_Real + W_4_QV_Real ) W_4 ";

                StrSql = StrSql + " ,Nominid,Nominid2 , Retry_ToEndDate , ToEndDate ";
                StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
                StrSql = StrSql + " Where Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

                StrSql = StrSql + " And  ((  Down_W4_QV_Real +  W4_QV_Real ) <>0  OR (Down_G_Down + SellPV01) <> 0  )  ";
                StrSql = StrSql + " And   N_LevelCnt =" + Cnt;
                StrSql = StrSql + " Group By Nominid,Nominid2  , Retry_ToEndDate, ToEndDate  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Nominid ";
                StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
                StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
                StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);                      
              
                pg1.PerformStep(); pg1.Refresh();

                Cnt = Cnt - 1;

            }








            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " Max_Down_W4_QV_Real =  ISNULL(B.A1,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select Max(Down_W4_QV_Real + W4_QV_Real ) A1 ";
            StrSql = StrSql + " ,Nominid,Nominid2, Retry_ToEndDate, ToEndDate  ";
            StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry  (nolock) ";
            StrSql = StrSql + " Where Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " And (Down_W4_QV_Real + W4_QV_Real )  > 0 ";
            StrSql = StrSql + " Group By Nominid,Nominid2 , Retry_ToEndDate , ToEndDate ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
            StrSql = StrSql + " And A.Retry_ToEndDate=B.Retry_ToEndDate ";
            StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " Max_Down_G_Down =  ISNULL(B.A1,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + "Select Max(Down_G_Down + SellCV01 ) A1 ";
            StrSql = StrSql + " ,Nominid,Nominid2 , Retry_ToEndDate, ToEndDate  ";
            StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
            StrSql = StrSql + " Where Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " And (Down_G_Down + SellCV01 )  > 0 ";
            StrSql = StrSql + " Group By Nominid,Nominid2 , Retry_ToEndDate , ToEndDate ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
            StrSql = StrSql + " And A.Retry_ToEndDate=B.Retry_ToEndDate ";
            StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " Max_N_LineCnt =  ";
            StrSql += " (Select Top 1 N_LineCnt From tbl_ClosePay_02_Mod_Retry (nolock) AS CC_02  ";
            StrSql += "                      Where CC_02.Nominid = tbl_ClosePay_02_Mod_Retry.Mbid  ";
            StrSql += "                      And   CC_02.Nominid2 = tbl_ClosePay_02_Mod_Retry.Mbid2   ";
            StrSql += "                      And   CC_02.Down_W4_QV_Real + CC_02.W4_QV_Real  =  tbl_ClosePay_02_Mod_Retry.Max_Down_W4_QV_Real   ";
            StrSql += "                      And   CC_02.Retry_ToEndDate = tbl_ClosePay_02_Mod_Retry.Retry_ToEndDate   ";
            StrSql += "                      And   CC_02.ToEndDate = tbl_ClosePay_02_Mod_Retry.ToEndDate   ";
            StrSql += "  ) ";
            StrSql = StrSql + " Where Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();









            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " Max_Down_W_1_QV_Real =  ISNULL(B.A1,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select (Down_W_1_QV_Real + W_1_QV_Real ) A1 ";
            StrSql = StrSql + " ,Nominid,Nominid2, N_LineCnt , Retry_ToEndDate, ToEndDate  ";
            StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
            StrSql = StrSql + " Where (Down_W_1_QV_Real + W_1_QV_Real )  > 0 ";
            StrSql = StrSql + " And  Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
            StrSql = StrSql + " And   A.Max_N_LineCnt = B.N_LineCnt  ";
            StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
            StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " Max_Down_W_2_QV_Real =  ISNULL(B.A1,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select (Down_W_2_QV_Real + W_2_QV_Real ) A1 ";
            StrSql = StrSql + " ,Nominid,Nominid2, N_LineCnt , Retry_ToEndDate, ToEndDate  ";
            StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
            StrSql = StrSql + " Where (Down_W_2_QV_Real + W_2_QV_Real )  > 0 ";
            StrSql = StrSql + " And  Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
            StrSql = StrSql + " And   A.Max_N_LineCnt = B.N_LineCnt  ";
            StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
            StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " Max_Down_W_3_QV_Real =  ISNULL(B.A1,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select (Down_W_3_QV_Real + W_3_QV_Real ) A1 ";
            StrSql = StrSql + " ,Nominid,Nominid2, N_LineCnt, Retry_ToEndDate, ToEndDate  ";
            StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
            StrSql = StrSql + " Where (Down_W_3_QV_Real + W_3_QV_Real )  > 0 ";
            StrSql = StrSql + " And  Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
            StrSql = StrSql + " And   A.Max_N_LineCnt = B.N_LineCnt  ";
            StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
            StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " Max_Down_W_4_QV_Real =  ISNULL(B.A1,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select (Down_W_4_QV_Real + W_4_QV_Real ) A1 ";
            StrSql = StrSql + " ,Nominid,Nominid2, N_LineCnt, Retry_ToEndDate, ToEndDate  ";
            StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
            StrSql = StrSql + " Where (Down_W_4_QV_Real + W_4_QV_Real )  > 0 ";
            StrSql = StrSql + " And  Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
            StrSql = StrSql + " And   A.Max_N_LineCnt = B.N_LineCnt  ";
            StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
            StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 






            //StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            //StrSql = StrSql + " Max_Down_W4_QV_Real =  ISNULL(B.A1,0)";
            //StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            //StrSql = StrSql + " (";
            //StrSql = StrSql + " Select Max(Down_W4_QV_Real + W4_QV_Real ) A1 ";
            //StrSql = StrSql + " ,Nominid,Nominid2, Retry_ToEndDate, ToEndDate ";
            //StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry ";
            //StrSql = StrSql + " Where Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            //StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
            //StrSql = StrSql + " And  (Down_W4_QV_Real + W4_QV_Real )  > 0 ";
            //StrSql = StrSql + " Group By Nominid,Nominid2 , Retry_ToEndDate , ToEndDate ";
            //StrSql = StrSql + " ) B";

            //StrSql = StrSql + " Where A.Retry_ToEndDate=B.Retry_ToEndDate ";
            //StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";
            //StrSql = StrSql + " And   A.Mbid=B.Nominid ";
            //StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            //StrSql = StrSql + " Max_Down_G_Down =  ISNULL(B.A1,0)";
            //StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            //StrSql = StrSql + " (";
            //StrSql = StrSql + "Select Max(Down_G_Down + SellPV01 ) A1 ";
            //StrSql = StrSql + " ,Nominid,Nominid2, Retry_ToEndDate, ToEndDate ";
            //StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
            //StrSql = StrSql + " Where Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            //StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
            //StrSql = StrSql + " And  (Down_G_Down + SellPV01 )  > 0 ";
            //StrSql = StrSql + " Group By Nominid,Nominid2, Retry_ToEndDate , ToEndDate  ";
            //StrSql = StrSql + " ) B";

            //StrSql = StrSql + " Where A.Retry_ToEndDate=B.Retry_ToEndDate ";
            //StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";
            //StrSql = StrSql + " And A.Mbid=B.Nominid ";
            //StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            //StrSql = StrSql + " Max_N_LineCnt =  ";
            //StrSql += " (Select Top 1 N_LineCnt From tbl_ClosePay_02_Mod_Retry (nolock) AS CC_02  ";
            //StrSql += "                     Where  CC_02.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            //StrSql += "                      And   CC_02.ToEndDate = '" + ToEndDate + "'";
            //StrSql += "                      And   CC_02.Nominid = tbl_ClosePay_02_Mod_Retry.Mbid  ";
            //StrSql += "                      And   CC_02.Nominid2 = tbl_ClosePay_02_Mod_Retry.Mbid2   ";
            //StrSql += "                      And   CC_02.Retry_ToEndDate = tbl_ClosePay_02_Mod_Retry.Retry_ToEndDate   ";
            //StrSql += "                      And   CC_02.ToEndDate = tbl_ClosePay_02_Mod_Retry.ToEndDate   ";
            //StrSql += "                      And   CC_02.Down_W4_QV_Real + CC_02.W4_QV_Real  =  tbl_ClosePay_02_Mod_Retry.Max_Down_W4_QV_Real   ";
            //StrSql += "  ) ";
            //StrSql = StrSql + " Where Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            //StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
        }






        private void Retry_GradeUpLine_ReqTF1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string Retry_ToEndDate)
        {
            int Cnt = 0;
            string StrSql = "", str_GradeCnt = "", str_GradeCnt1 = "", str_GradeCnt2 = "", str_GradeCnt3 = "";

            
            int Base_M_Lvl = Retry_MaxLevel;


            pg1.Value = 0; pg1.Maximum = Cnt + 4;
            pg1.PerformStep(); pg1.Refresh();

            Cnt = Base_M_Lvl;

            while (Cnt >= 1)
            {
              


                StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                StrSql = StrSql + " ReqTF1_L_1 = ISNULL(B.A1,0) ";
                // StrSql = StrSql + " ,Down_W4_QV_Real_1 =  ISNULL(B.A2,0)";
                StrSql = StrSql + " ,Down_W_1_QV_Real_1 =    ISNULL(B.W_1,0)";
                StrSql = StrSql + " ,Down_W_2_QV_Real_1 =    ISNULL(B.W_2,0)";
                StrSql = StrSql + " ,Down_W_3_QV_Real_1 =    ISNULL(B.W_3,0)";
                StrSql = StrSql + " ,Down_W_4_QV_Real_1 =    ISNULL(B.W_4,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                StrSql = StrSql + " (Select    Sum( ReqTF1_L_1 + ReqTF1_L_2 ) A1,Saveid,Saveid2, Retry_ToEndDate , ToEndDate  ";
                StrSql = StrSql + " , Sum(Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real  ) A2 ";
                StrSql = StrSql + " ,Sum(Down_W_1_QV_Real_1 + Down_W_1_QV_Real_2 + W_1_QV_Real ) W_1 ";
                StrSql = StrSql + " ,Sum(Down_W_2_QV_Real_1 + Down_W_2_QV_Real_2 + W_2_QV_Real ) W_2 ";
                StrSql = StrSql + " ,Sum(Down_W_3_QV_Real_1 + Down_W_3_QV_Real_2 + W_3_QV_Real ) W_3 ";
                StrSql = StrSql + " ,Sum(Down_W_4_QV_Real_1 + Down_W_4_QV_Real_2 +  W_4_QV_Real ) W_4 ";
                StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
                //StrSql = StrSql + " Where (ReqTF1_L_1 + ReqTF1_L_2  > 0  OR (  Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real ) <>0 ) ";
                StrSql = StrSql + " Where LineCnt = 1 ";
                StrSql = StrSql + " And LevelCnt =" + Cnt;
                StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
                StrSql = StrSql + " Group By Saveid,Saveid2, Retry_ToEndDate , ToEndDate  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";
                StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
                StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                StrSql = StrSql + " ReqTF1_L_1 = ReqTF1_L_1  + ISNULL(B.A1,0)  ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                StrSql = StrSql + " (Select Count(Mbid) A1,   Saveid,Saveid2, Retry_ToEndDate , ToEndDate  ";
                StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
                StrSql = StrSql + " Where ReqTF1 =  1 ";
                StrSql = StrSql + " And  LineCnt = 1 ";
                StrSql = StrSql + " And LevelCnt =" + Cnt;
                StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
                StrSql = StrSql + " Group By Saveid,Saveid2 , Retry_ToEndDate , ToEndDate  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";
                StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
                StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
                //'''---------------------------------------------------------------



                StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                StrSql = StrSql + " ReqTF1_L_2 = ISNULL(B.A1,0) ";
                // StrSql = StrSql + " ,Down_W4_QV_Real_2 =  ISNULL(B.A2,0)";
                StrSql = StrSql + " ,Down_W_1_QV_Real_2 =    ISNULL(B.W_1,0)";
                StrSql = StrSql + " ,Down_W_2_QV_Real_2 =    ISNULL(B.W_2,0)";
                StrSql = StrSql + " ,Down_W_3_QV_Real_2 =    ISNULL(B.W_3,0)";
                StrSql = StrSql + " ,Down_W_4_QV_Real_2 =    ISNULL(B.W_4,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                StrSql = StrSql + " (Select    Sum(ReqTF1_L_1 + ReqTF1_L_2) A1,Saveid,Saveid2, Retry_ToEndDate , ToEndDate  ";
                StrSql = StrSql + " , Sum(Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real  ) A2 ";
                StrSql = StrSql + " ,Sum(Down_W_1_QV_Real_1 + Down_W_1_QV_Real_2 + W_1_QV_Real ) W_1 ";
                StrSql = StrSql + " ,Sum(Down_W_2_QV_Real_1 + Down_W_2_QV_Real_2 + W_2_QV_Real ) W_2 ";
                StrSql = StrSql + " ,Sum(Down_W_3_QV_Real_1 + Down_W_3_QV_Real_2 + W_3_QV_Real ) W_3 ";
                StrSql = StrSql + " ,Sum(Down_W_4_QV_Real_1 + Down_W_4_QV_Real_2 +  W_4_QV_Real ) W_4 ";

                StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
               // StrSql = StrSql + " Where (ReqTF1_L_1 + ReqTF1_L_2  > 0  OR (  Down_W4_QV_Real_1 + Down_W4_QV_Real_2 +  W4_QV_Real ) <>0 ) ";
                StrSql = StrSql + " Where LineCnt >= 2 ";
                StrSql = StrSql + " And LevelCnt =" + Cnt;
                StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
                StrSql = StrSql + " Group By Saveid,Saveid2 , Retry_ToEndDate , ToEndDate  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";
                StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
                StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();


                StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                StrSql = StrSql + " ReqTF1_L_2 = ReqTF1_L_2 + ISNULL(B.A1,0)  ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                StrSql = StrSql + " (Select Count(Mbid) A1,   Saveid,Saveid2 , Retry_ToEndDate , ToEndDate  ";
                StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
                StrSql = StrSql + " Where ReqTF1 =  1 ";
                StrSql = StrSql + " And LineCnt >= 2 ";
                StrSql = StrSql + " And LevelCnt =" + Cnt;
                StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
                StrSql = StrSql + " Group By Saveid,Saveid2 , Retry_ToEndDate , ToEndDate  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";
                StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
                StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
                // '''---------------------------------------------------------------

                Cnt = Cnt - 1;
            }



            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " Down_W4_QV_Real_1 = Down_W_1_QV_Real_1  ";
            StrSql = StrSql + ",Down_W4_QV_Real_2 = Down_W_1_QV_Real_2  ";
            StrSql = StrSql + " Where  Active_1_FLAG = 'Y'";
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " Down_W4_QV_Real_1 = Down_W4_QV_Real_1 + Down_W_2_QV_Real_1  ";
            StrSql = StrSql + ", Down_W4_QV_Real_2 = Down_W4_QV_Real_2 + Down_W_2_QV_Real_2  ";
            StrSql = StrSql + " Where  Active_2_FLAG = 'Y'";
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " Down_W4_QV_Real_1 = Down_W4_QV_Real_1 + Down_W_3_QV_Real_1  ";
            StrSql = StrSql + " ,Down_W4_QV_Real_2 = Down_W4_QV_Real_2 + Down_W_3_QV_Real_2  ";
            StrSql = StrSql + " Where  Active_3_FLAG = 'Y'";
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " Down_W4_QV_Real_1 = Down_W4_QV_Real_1 + Down_W_4_QV_Real_1  ";
            StrSql = StrSql + " ,Down_W4_QV_Real_2 = Down_W4_QV_Real_2 + Down_W_4_QV_Real_2  ";
            StrSql = StrSql + " Where  ReqTF1 = 1";
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


        }



        private void Retry_Put_LevelCnt_Update(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string Retry_ToEndDate) 
        {
            string StrSql = " Select Mbid,Mbid2 From tbl_ClosePay_02_Mod_Retry  (nolock) ";
            StrSql = StrSql + " Where Retry_ToEndDate ='" + Retry_ToEndDate + "'" ;
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " And Saveid='**'   ";

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

                    StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                    StrSql = StrSql + " LevelCnt=ISNULL(B.lvl,0) ";
                    StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                    StrSql = StrSql + " (Select    empid0,empid,lvl ";
                    StrSql = StrSql + " From ufn_GetSubTree_Pay02_Mem_Retry('" + Mbid + "'," + Mbid2 + ",'" + Retry_ToEndDate + "','" + ToEndDate + "'";
                    StrSql = StrSql + ") Where pos <>0 ";
                    StrSql = StrSql + " ) B";

                    StrSql = StrSql + " Where   A.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                    StrSql = StrSql + " And    A.Mbid=B.empid0 ";
                    StrSql = StrSql + " And   A.Mbid2=B.empid ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);

                    pg1.PerformStep(); pg1.Refresh();
                } // for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)

            } // (ReCnt != 0)




            StrSql = "Select Max(LevelCnt) From tbl_ClosePay_02_Mod_Retry Where Retry_ToEndDate ='" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;

            while (sr.Read())
            {
                Retry_MaxLevel = int.Parse(sr.GetValue(0).ToString());
            }

            sr.Close(); sr.Dispose();

        }



        private void Retry_Put_LevelCnt_Update_Nom(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string Retry_ToEndDate) 
        {
            string StrSql = " Select Mbid,Mbid2 From tbl_ClosePay_02_Mod_Retry  (nolock) ";
            StrSql = StrSql + " Where Retry_ToEndDate ='" + Retry_ToEndDate + "'" ;
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " And   Nominid='**'   ";

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

                    StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                    StrSql = StrSql + " N_LevelCnt = ISNULL(B.lvl,0) ";
                    StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                    StrSql = StrSql + " (Select    empid0,empid,lvl ";
                    StrSql = StrSql + " From ufn_GetSubTree_Pay02_Nom_Retry('" + Mbid + "'," + Mbid2 + ",'" + Retry_ToEndDate + "','" + ToEndDate + "'";
                    StrSql = StrSql + ") Where pos <>0 ";
                    StrSql = StrSql + " ) B";

                    StrSql = StrSql + " Where   A.Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                    StrSql = StrSql + " And    A.Mbid=B.empid0 ";
                    StrSql = StrSql + " And   A.Mbid2=B.empid ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);

                    pg1.PerformStep(); pg1.Refresh();
                } // for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)

            } // (ReCnt != 0)




            StrSql = "Select Max(N_LevelCnt) From tbl_ClosePay_02_Mod_Retry Where Retry_ToEndDate ='" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;

            while (sr.Read())
            {
                Retry_N_MaxLevel = int.Parse(sr.GetValue(0).ToString());
            }

            sr.Close(); sr.Dispose();

        }






        private Boolean Retry_Check_UP_Grade_TF(int CurrentGrade, cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string Retry_ToEndDate)
        {
            string StrSql = "";

            StrSql = " Select Isnull(Count(Mbid),0)    ";
            StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
            StrSql = StrSql + " Where   UP_Grade_TF = " + CurrentGrade;
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            DataSet ds_T = new DataSet();

            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds_T);
            ReCnt = Search_Connect.DataSet_ReCount;

            int up_Cnt = 0;
            if (ReCnt <= 0)
                return false;
            else
            {
                up_Cnt = int.Parse(ds_T.Tables[base_db_name].Rows[0][0].ToString());
            }

            if (up_Cnt > 0)
                return true;
            else
                return false;
        }




        private void Retry_GiveGrade1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt , string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 3;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " OneGrade = 10 ";
            StrSql = StrSql + " ,UP_Grade_TF = 10 ";
            StrSql = StrSql + " Where   OneGrade < 10 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 500 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 500 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 500 ";
            StrSql = StrSql + " And   GradeDate1 = '' ";            
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " OneGrade = 10 ";
            StrSql = StrSql + " ,UP_Grade_TF = 10 ";
            StrSql = StrSql + " Where   OneGrade < 10 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 500 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_1 >= 1000 ";
            StrSql = StrSql + " And   Down_W4_QV_Real_2 >= 1000 ";
            StrSql = StrSql + " And   GradeDate1 <> '' ";            
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();





            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " CurGrade = OneGrade ";
            //StrSql = StrSql + " Where   OneGrade > CurGrade ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade =10 ";
            //StrSql = StrSql + " And GradeDate1 =''";


            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
        }


        private void Retry_GiveGrade2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt, string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 6;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " OneGrade = 20 ";
            StrSql = StrSql + " ,UP_Grade_TF = 20 ";
            StrSql = StrSql + " Where   OneGrade < 20 ";

            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 500 ";
            StrSql = StrSql + " And     Down_W4_QV_Real_1 >= 2000  ";
            StrSql = StrSql + " And     Down_W4_QV_Real_2 >= 2000  ";
            //if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt;
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " CurGrade = OneGrade ";
            //StrSql = StrSql + " Where   OneGrade > CurGrade ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 20 ";
            //StrSql = StrSql + " And GradeDate2 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

        }


        private void Retry_GiveGrade3(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt, string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 9;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            //per
            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " OneGrade= 30 ";
            StrSql = StrSql + " ,UP_Grade_TF = 30 ";
            StrSql = StrSql + " Where   OneGrade < 30 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            StrSql = StrSql + " And     Down_W4_QV_Real_1 >= 4000  ";
            StrSql = StrSql + " And     Down_W4_QV_Real_2 >= 4000  ";
            StrSql = StrSql + " And   Max_GradeCnt1 >= 1  ";
            StrSql = StrSql + " And   GradeCnt1 >= 2  ";
            StrSql = StrSql + " And   GradeCnt1 - Max_GradeCnt1 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt;
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " CurGrade = OneGrade ";
            //StrSql = StrSql + " Where   OneGrade > CurGrade ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 30 ";
            //StrSql = StrSql + " And GradeDate3 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 30";
            //StrSql = StrSql + " And GradeDate2 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 30 ";
            //StrSql = StrSql + " And GradeDate1 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
        }


        private void Retry_GiveGrade4(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt, string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 10;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " OneGrade= 40 ";
            StrSql = StrSql + " ,UP_Grade_TF = 40 ";
            StrSql = StrSql + " Where OneGrade < 40 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            StrSql = StrSql + " And     Down_W4_QV_Real_1 >= 8000  ";
            StrSql = StrSql + " And     Down_W4_QV_Real_2 >= 8000  ";
            StrSql = StrSql + " And   Max_GradeCnt2 >= 1  ";
            StrSql = StrSql + " And   GradeCnt2 >= 2  ";
            StrSql = StrSql + " And   GradeCnt2 - Max_GradeCnt2 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt;
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " CurGrade = OneGrade ";
            //StrSql = StrSql + " Where   OneGrade > CurGrade ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);

            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 40 ";
            //StrSql = StrSql + " And GradeDate4 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();




            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 40 ";
            //StrSql = StrSql + " And GradeDate3 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 40";
            //StrSql = StrSql + " And GradeDate2 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 40 ";
            //StrSql = StrSql + " And GradeDate1 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
        }





        private void Retry_GiveGrade5(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt, string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 12;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " OneGrade= 50 ";
            StrSql = StrSql + " ,UP_Grade_TF = 50 ";
            StrSql = StrSql + " Where OneGrade < 50 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            StrSql = StrSql + " And     Down_W4_QV_Real_1 >= 15000  ";
            StrSql = StrSql + " And     Down_W4_QV_Real_2 >= 15000  ";
            StrSql = StrSql + " And   Max_GradeCnt3 >= 1  ";
            StrSql = StrSql + " And   GradeCnt3 >= 2  ";
            StrSql = StrSql + " And   GradeCnt3 - Max_GradeCnt3 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt;
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " CurGrade = OneGrade ";
            //StrSql = StrSql + " Where   OneGrade > CurGrade ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);




            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 50 ";
            //StrSql = StrSql + " And GradeDate5 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 50 ";
            //StrSql = StrSql + " And GradeDate4 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();




            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 50 ";
            //StrSql = StrSql + " And GradeDate3 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 50";
            //StrSql = StrSql + " And GradeDate2 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 50 ";
            //StrSql = StrSql + " And GradeDate1 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
        }



        private void Retry_GiveGrade6(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt, string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 11;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " OneGrade= 60 ";
            StrSql = StrSql + " ,UP_Grade_TF = 60 ";
            StrSql = StrSql + " Where OneGrade < 60 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            StrSql = StrSql + " And     Down_W4_QV_Real_1 >= 30000  ";
            StrSql = StrSql + " And     Down_W4_QV_Real_2 >= 30000  ";
            StrSql = StrSql + " And   Max_GradeCnt4 >= 1  ";
            StrSql = StrSql + " And   GradeCnt4 >= 2  ";
            StrSql = StrSql + " And   GradeCnt4 - Max_GradeCnt4 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt;
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();








            ////// 반품 관련 4주 부분 때문에.. 우선은 이렇게 처리해 놓은 이부분은 보완을 해야 함 ㅠㅠ
            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " CurGrade= 60 ";
            ////StrSql = StrSql + " Where CurGrade < 60 ";
            ////StrSql = StrSql + " And   OrgGrade = 60  ";
            ////StrSql = StrSql + " And   LeaveDate = ''";
            ////StrSql = StrSql + " And   Sell_MEM_TF = 0 ";


            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();



            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " CurGrade = OneGrade ";
            //StrSql = StrSql + " Where   OneGrade > CurGrade ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);




            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 60 ";
            //StrSql = StrSql + " And GradeDate6 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 60 ";
            //StrSql = StrSql + " And GradeDate5 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 60 ";
            //StrSql = StrSql + " And GradeDate4 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();




            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 60 ";
            //StrSql = StrSql + " And GradeDate3 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 60";
            //StrSql = StrSql + " And GradeDate2 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 60 ";
            //StrSql = StrSql + " And GradeDate1 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
        }



        private void Retry_GiveGrade7(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt, string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 11;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " OneGrade= 70 ";
            StrSql = StrSql + " ,UP_Grade_TF = 70 ";
            StrSql = StrSql + " Where OneGrade < 70 "; ;
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            StrSql = StrSql + " And     Down_W4_QV_Real_1 >= 60000  ";
            StrSql = StrSql + " And     Down_W4_QV_Real_2 >= 60000  ";
            
            StrSql = StrSql + " And   Max_GradeCnt5 >= 1  ";
            StrSql = StrSql + " And   GradeCnt5 >= 2  ";
            StrSql = StrSql + " And   GradeCnt5 - Max_GradeCnt5 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt;
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " CurGrade = OneGrade ";
            //StrSql = StrSql + " Where   OneGrade > CurGrade ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            ////    StrSql = "Update tbl_ClosePay_02_Mod_Retry Set " ;
            ////StrSql = StrSql + " CurGrade= 70 " ;
            ////StrSql = StrSql + " Where CurGrade < 70 " ;

            ////StrSql = StrSql + " And   LeaveDate = ''" ;
            ////StrSql = StrSql + " And   Sell_MEM_TF = 0 " ;


            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();

            //////    StrSql = "Update tbl_ClosePay_02_Mod_Retry Set " ;
            //////StrSql = StrSql + " CurGrade = 70 " ;
            //////StrSql = StrSql + " Where BeforeGrade = 70 " ;
            //////StrSql = StrSql + " And   CurGrade < 70 " ;
            //////StrSql = StrSql + " And   G_Sum_PV_1 >= 30000 " ;
            //////StrSql = StrSql + " And   G_Sum_PV_2 >= 30000 " ;
            //////StrSql = StrSql + " And   LeaveDate = ''" ;
            //////StrSql = StrSql + " And   Sell_MEM_TF = 0 ";

            //////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //////pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 70 ";
            //StrSql = StrSql + " And GradeDate7 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();



            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 70 ";
            //StrSql = StrSql + " And GradeDate6 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 70 ";
            //StrSql = StrSql + " And GradeDate5 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 70 ";
            //StrSql = StrSql + " And GradeDate4 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();




            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 70 ";
            //StrSql = StrSql + " And GradeDate3 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 70";
            //StrSql = StrSql + " And GradeDate2 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 70 ";
            //StrSql = StrSql + " And GradeDate1 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
        }

        private void Retry_GiveGrade8(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt, string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 12;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " OneGrade = 80 ";
            StrSql = StrSql + " ,UP_Grade_TF = 80 ";
            StrSql = StrSql + " Where OneGrade < 80 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            StrSql = StrSql + " And     Down_W4_QV_Real_1 >= 120000  ";
            StrSql = StrSql + " And     Down_W4_QV_Real_2 >= 120000  ";
            StrSql = StrSql + " And   Max_GradeCnt6 >= 1  ";
            StrSql = StrSql + " And   GradeCnt6 >= 2  ";
            StrSql = StrSql + " And   GradeCnt6 - Max_GradeCnt6 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt;
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " CurGrade = OneGrade ";
            //StrSql = StrSql + " Where   OneGrade > CurGrade ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);



            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " CurGrade= 80 ";
            ////StrSql = StrSql + " Where CurGrade < 80 ";

            ////StrSql = StrSql + " And   LeaveDate = '' ";
            ////StrSql = StrSql + " And   Sell_MEM_TF = 0 ";


            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();

            //////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //////StrSql = StrSql + " CurGrade = 80 ";
            //////StrSql = StrSql + " Where BeforeGrade = 80 ";
            //////StrSql = StrSql + " And   CurGrade < 80 ";
            //////StrSql = StrSql + " And   G_Sum_PV_1 >= 90000 ";
            //////StrSql = StrSql + " And   G_Sum_PV_2 >= 90000 ";
            //////StrSql = StrSql + " And   LeaveDate = ''";
            //////StrSql = StrSql + " And   Sell_MEM_TF = 0 ";

            //////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //////pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 80 ";
            //StrSql = StrSql + " And GradeDate8 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 80 ";
            //StrSql = StrSql + " And GradeDate7 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();



            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 80 ";
            //StrSql = StrSql + " And GradeDate6 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 80 ";
            //StrSql = StrSql + " And GradeDate5 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 80 ";
            //StrSql = StrSql + " And GradeDate4 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();




            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 80 ";
            //StrSql = StrSql + " And GradeDate3 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 80";
            //StrSql = StrSql + " And GradeDate2 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 80 ";
            //StrSql = StrSql + " And GradeDate1 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
        }

        private void Retry_GiveGrade9(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt, string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 13;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " OneGrade= 90 ";
            StrSql = StrSql + " ,UP_Grade_TF = 90 ";
            StrSql = StrSql + " Where OneGrade < 90 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            StrSql = StrSql + " And     Down_W4_QV_Real_1 >= 250000  ";
            StrSql = StrSql + " And     Down_W4_QV_Real_2 >= 250000  ";
            StrSql = StrSql + " And   Max_GradeCnt7 >= 1  ";
            StrSql = StrSql + " And   GradeCnt7 >= 2  ";
            StrSql = StrSql + " And   GradeCnt7 - Max_GradeCnt7 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt;
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " CurGrade = OneGrade ";
            //StrSql = StrSql + " Where   OneGrade > CurGrade ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 90 ";
            //StrSql = StrSql + " And GradeDate9 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();



            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 90 ";
            //StrSql = StrSql + " And GradeDate8 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 90 ";
            //StrSql = StrSql + " And GradeDate7 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();



            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 90 ";
            //StrSql = StrSql + " And GradeDate6 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 90 ";
            //StrSql = StrSql + " And GradeDate5 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 90 ";
            //StrSql = StrSql + " And GradeDate4 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();




            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 90 ";
            //StrSql = StrSql + " And GradeDate3 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 90";
            //StrSql = StrSql + " And GradeDate2 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 90 ";
            //StrSql = StrSql + " And GradeDate1 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
        }


        private void Retry_GiveGrade10(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt, string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 14;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " OneGrade= 100 ";
            StrSql = StrSql + " ,UP_Grade_TF = 100 ";
            StrSql = StrSql + " Where OneGrade < 100 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            StrSql = StrSql + " And     Down_W4_QV_Real_1 >= 500000  ";
            StrSql = StrSql + " And     Down_W4_QV_Real_2 >= 500000  ";
            StrSql = StrSql + " And   Max_GradeCnt8 >= 1  ";
            StrSql = StrSql + " And   GradeCnt8 >= 2  ";
            StrSql = StrSql + " And   GradeCnt8 - Max_GradeCnt8 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt;
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " CurGrade = OneGrade ";
            //StrSql = StrSql + " Where   OneGrade > CurGrade ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);



            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate10 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 100 ";
            //StrSql = StrSql + " And GradeDate10 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 100 ";
            //StrSql = StrSql + " And GradeDate9 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();



            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 100 ";
            //StrSql = StrSql + " And GradeDate8 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 100 ";
            //StrSql = StrSql + " And GradeDate7 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();



            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 100 ";
            //StrSql = StrSql + " And GradeDate6 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 100 ";
            //StrSql = StrSql + " And GradeDate5 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 100 ";
            //StrSql = StrSql + " And GradeDate4 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();




            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 100 ";
            //StrSql = StrSql + " And GradeDate3 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 100";
            //StrSql = StrSql + " And GradeDate2 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 100 ";
            //StrSql = StrSql + " And GradeDate1 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
        }



        private void Retry_GiveGrade11(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt, string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 14;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " OneGrade= 110 ";
            StrSql = StrSql + " ,UP_Grade_TF = 110 ";
            StrSql = StrSql + " Where OneGrade < 110 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            StrSql = StrSql + " And     Down_W4_QV_Real_1 >= 1000000  ";
            StrSql = StrSql + " And     Down_W4_QV_Real_2 >= 1000000  ";
            StrSql = StrSql + " And   Max_GradeCnt9 >= 1  ";
            StrSql = StrSql + " And   GradeCnt9 >= 2  ";
            StrSql = StrSql + " And   GradeCnt9 - Max_GradeCnt9 >= 1  ";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt;
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " CurGrade = OneGrade ";
            ////StrSql = StrSql + " Where   OneGrade > CurGrade ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);



            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " GradeDate11 ='" + ToEndDate + "'";
            ////StrSql = StrSql + " Where CurGrade = 110 ";
            ////StrSql = StrSql + " And GradeDate11 =''";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " GradeDate10 ='" + ToEndDate + "'";
            ////StrSql = StrSql + " Where CurGrade = 110 ";
            ////StrSql = StrSql + " And GradeDate10 =''";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            ////StrSql = StrSql + " Where CurGrade = 110 ";
            ////StrSql = StrSql + " And GradeDate9 =''";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();



            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            ////StrSql = StrSql + " Where CurGrade = 110 ";
            ////StrSql = StrSql + " And GradeDate8 =''";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            ////StrSql = StrSql + " Where CurGrade = 110 ";
            ////StrSql = StrSql + " And GradeDate7 =''";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();



            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            ////StrSql = StrSql + " Where CurGrade = 110 ";
            ////StrSql = StrSql + " And GradeDate6 =''";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();

            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            ////StrSql = StrSql + " Where CurGrade = 110 ";
            ////StrSql = StrSql + " And GradeDate5 =''";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            ////StrSql = StrSql + " Where CurGrade = 110 ";
            ////StrSql = StrSql + " And GradeDate4 =''";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();




            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            ////StrSql = StrSql + " Where CurGrade = 110 ";
            ////StrSql = StrSql + " And GradeDate3 =''";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            ////StrSql = StrSql + " Where CurGrade = 110";
            ////StrSql = StrSql + " And GradeDate2 =''";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            ////StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            ////StrSql = StrSql + " Where CurGrade = 110 ";
            ////StrSql = StrSql + " And GradeDate1 =''";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();
        }




        private void Retry_GiveGrade12(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt, string Retry_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 14;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " UP_Grade_TF = 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            StrSql = StrSql + " OneGrade= 120 ";
            StrSql = StrSql + " ,UP_Grade_TF = 120 ";
            StrSql = StrSql + " Where OneGrade < 120 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ReqTF1 = 1 ";
            StrSql = StrSql + " And   SellPV01 >= 1000 ";
            StrSql = StrSql + " And     Down_W4_QV_Real_1 >= 2000000  ";
            StrSql = StrSql + " And     Down_W4_QV_Real_2 >= 2000000  ";
            StrSql = StrSql + " And   Max_GradeCnt10 >= 1  ";
            StrSql = StrSql + " And   GradeCnt10 >= 2  ";
            StrSql = StrSql + " And   GradeCnt10 - Max_GradeCnt10 >= 1  ";
            StrSql = StrSql + " And   N_LevelCnt = " + S_LevelCnt;
            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " CurGrade = OneGrade ";
            //StrSql = StrSql + " Where   OneGrade > CurGrade ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);

            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate12 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 120 ";
            //StrSql = StrSql + " And GradeDate12 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate11 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 120 ";
            //StrSql = StrSql + " And GradeDate11 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate10 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 120 ";
            //StrSql = StrSql + " And GradeDate10 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 120 ";
            //StrSql = StrSql + " And GradeDate9 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();



            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 120 ";
            //StrSql = StrSql + " And GradeDate8 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 120 ";
            //StrSql = StrSql + " And GradeDate7 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();



            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 120 ";
            //StrSql = StrSql + " And GradeDate6 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 120 ";
            //StrSql = StrSql + " And GradeDate5 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 120 ";
            //StrSql = StrSql + " And GradeDate4 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();




            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 120 ";
            //StrSql = StrSql + " And GradeDate3 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 120";
            //StrSql = StrSql + " And GradeDate2 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02_Mod_Retry Set ";
            //StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            //StrSql = StrSql + " Where CurGrade = 120 ";
            //StrSql = StrSql + " And GradeDate1 =''";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
        }





        private void Retry_GradeUpLine__3(int CurrentGrade, cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int S_LevelCnt, string Retry_ToEndDate)
        {
            int Cnt = 0;
            string StrSql = "", str_GradeCnt = "", str_GradeCnt1 = "", str_GradeCnt2 = "", str_GradeCnt3 = "";

            if (CurrentGrade == 10) str_GradeCnt = " GradeCnt1 ";
            if (CurrentGrade == 20) str_GradeCnt = " GradeCnt2 ";
            if (CurrentGrade == 30) str_GradeCnt = " GradeCnt3 ";
            if (CurrentGrade == 40) str_GradeCnt = " GradeCnt4 ";
            if (CurrentGrade == 50) str_GradeCnt = " GradeCnt5 ";
            if (CurrentGrade == 60) str_GradeCnt = " GradeCnt6 ";
            if (CurrentGrade == 70) str_GradeCnt = " GradeCnt7 ";
            if (CurrentGrade == 80) str_GradeCnt = " GradeCnt8 ";
            if (CurrentGrade == 90) str_GradeCnt = " GradeCnt9 ";
            if (CurrentGrade == 100) str_GradeCnt = " GradeCnt10 ";
            if (CurrentGrade == 110) str_GradeCnt = " GradeCnt11 ";
            if (CurrentGrade == 120) str_GradeCnt = " GradeCnt12 ";


            string Max_str_GradeCnt = "";
            if (CurrentGrade == 10) Max_str_GradeCnt = " Max_GradeCnt1 ";
            if (CurrentGrade == 20) Max_str_GradeCnt = " Max_GradeCnt2 ";
            if (CurrentGrade == 30) Max_str_GradeCnt = " Max_GradeCnt3 ";
            if (CurrentGrade == 40) Max_str_GradeCnt = " Max_GradeCnt4 ";
            if (CurrentGrade == 50) Max_str_GradeCnt = " Max_GradeCnt5 ";
            if (CurrentGrade == 60) Max_str_GradeCnt = " Max_GradeCnt6 ";
            if (CurrentGrade == 70) Max_str_GradeCnt = " Max_GradeCnt7 ";
            if (CurrentGrade == 80) Max_str_GradeCnt = " Max_GradeCnt8 ";
            if (CurrentGrade == 90) Max_str_GradeCnt = " Max_GradeCnt9 ";
            if (CurrentGrade == 100) Max_str_GradeCnt = " Max_GradeCnt10 ";
            if (CurrentGrade == 110) Max_str_GradeCnt = " Max_GradeCnt11 ";
            if (CurrentGrade == 120) Max_str_GradeCnt = " Max_GradeCnt12 ";

            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " " + str_GradeCnt + " = 0";
            StrSql = StrSql + ", " + Max_str_GradeCnt  + " = 0";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            if (S_LevelCnt >= 0)
            {
                pg1.Value = 0; pg1.Maximum = Cnt + 4;
                pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                StrSql = StrSql + str_GradeCnt + "=ISNULL(B.A1,0) ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                StrSql = StrSql + " (Select    Sum(" + str_GradeCnt + ") A1,Nominid,Nominid2, Retry_ToEndDate, ToEndDate   ";
                StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
                StrSql = StrSql + " Where " + str_GradeCnt + " > 0  ";
                if (S_LevelCnt >= 0) StrSql = StrSql + " And N_LevelCnt =" + S_LevelCnt;
                StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
                StrSql = StrSql + " Group By Nominid,Nominid2, Retry_ToEndDate, ToEndDate ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Nominid ";
                StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

                StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
                StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                StrSql = StrSql + str_GradeCnt + " =" + str_GradeCnt + " + ISNULL(B.A1,0)  ";
                StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                StrSql = StrSql + " (Select Count(Mbid) A1,   Nominid,Nominid2 , Retry_ToEndDate, ToEndDate ";
                StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry ";
                StrSql = StrSql + " Where OneGrade >= " + CurrentGrade;
                //else
                //    StrSql = StrSql + " Where OneGrade = " + CurrentGrade;

                if (S_LevelCnt >= 0) StrSql = StrSql + " And N_LevelCnt =" + S_LevelCnt;

                StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

                StrSql = StrSql + " Group By Nominid,Nominid2  , Retry_ToEndDate, ToEndDate ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Nominid ";
                StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

                StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
                StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
                //'''---------------------------------------------------------------
            }
            else
            {

                int T_N_MaxLevel = 0;
                StrSql = " Select Isnull(Max(N_LevelCnt),0)    ";
                StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
                StrSql = StrSql + " Where   OneGrade >= " + CurrentGrade;
                StrSql = StrSql + " And     Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                StrSql = StrSql + " And     ToEndDate = '" + ToEndDate + "'";

                DataSet ds_T = new DataSet();

                int ReCnt = 0;
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds_T);
                ReCnt = Search_Connect.DataSet_ReCount;
                                
                if (ReCnt > 0)
                {                    
                    T_N_MaxLevel = int.Parse(ds_T.Tables[base_db_name].Rows[0][0].ToString()) ;
                }


                if (T_N_MaxLevel > 0)
                {
                    Cnt = T_N_MaxLevel + 1;


                    pg1.Value = 0; pg1.Maximum = Cnt + 2;
                    pg1.PerformStep(); pg1.Refresh();

                    while (Cnt >= 1)
                    {
                        StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                        StrSql = StrSql + str_GradeCnt + "=ISNULL(B.A1,0) ";
                        StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                        StrSql = StrSql + " (Select    Sum(" + str_GradeCnt + ") A1,Nominid,Nominid2 , Retry_ToEndDate, ToEndDate ";
                        StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
                        StrSql = StrSql + " Where " + str_GradeCnt + " > 0  ";
                        StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                        StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
                        StrSql = StrSql + " And N_LevelCnt =" + Cnt;
                        StrSql = StrSql + " Group By Nominid,Nominid2 , Retry_ToEndDate, ToEndDate ";
                        StrSql = StrSql + " ) B";

                        StrSql = StrSql + " Where A.Mbid=B.Nominid ";
                        StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
                        StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
                        StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);


                        StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
                        StrSql = StrSql + str_GradeCnt + " =" + str_GradeCnt + " + ISNULL(B.A1,0)  ";
                        StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

                        StrSql = StrSql + " (Select Count(Mbid) A1,   Nominid,Nominid2 , Retry_ToEndDate, ToEndDate ";
                        StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry  (nolock) ";
                        StrSql = StrSql + " Where OneGrade >= " + CurrentGrade;
                        StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
                        StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";
                        StrSql = StrSql + " And N_LevelCnt =" + Cnt;
                        StrSql = StrSql + " Group By Nominid,Nominid2 , Retry_ToEndDate, ToEndDate  ";
                        StrSql = StrSql + " ) B";

                        StrSql = StrSql + " Where A.Mbid=B.Nominid ";
                        StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
                        StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
                        StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);
                        pg1.PerformStep(); pg1.Refresh();
                        //'''---------------------------------------------------------------

                        Cnt = Cnt - 1;
                    }

                }
            }

            StrSql = "Update tbl_ClosePay_02_Mod_Retry SET ";
            StrSql = StrSql + " " + Max_str_GradeCnt + " =  ISNULL(B.A1,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod_Retry  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select Case When OneGrade >= " + CurrentGrade + " then " + str_GradeCnt + " + 1  ELSE  " + str_GradeCnt + "  END A1 ";
            StrSql = StrSql + " ,Nominid,Nominid2 , N_LineCnt , Retry_ToEndDate, ToEndDate ";
            StrSql = StrSql + " From tbl_ClosePay_02_Mod_Retry (nolock) ";
            StrSql = StrSql + " Where (" + str_GradeCnt + "  > 0 ";
            StrSql = StrSql + " OR  OneGrade >= " + CurrentGrade + ")";
            if (S_LevelCnt >= 0) StrSql = StrSql + " And N_LevelCnt =" + S_LevelCnt;  ////이게 변경 첨가된 부분임..

            StrSql = StrSql + " And   Retry_ToEndDate = '" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   ToEndDate = '" + ToEndDate + "'";

            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";
            StrSql = StrSql + " And   A.Max_N_LineCnt = B.N_LineCnt ";

            StrSql = StrSql + " And   A.Retry_ToEndDate=B.Retry_ToEndDate ";
            StrSql = StrSql + " And   A.ToEndDate=B.ToEndDate ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

        }





































































    }
}
