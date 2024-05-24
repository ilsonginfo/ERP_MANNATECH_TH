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
    public partial class frmClose_4 : clsForm_Extends
    {
        

          cls_Grid_Base cgb = new cls_Grid_Base();
        private int Data_Set_Form_TF = 0;
        private string base_db_name = "tbl_CloseTotal_04";
        private string FromEndDate = ""; private string ToEndDate = ""; private string PayDate = "", PayDate2  = "" ;
        private int From_Load_TF = 0;
        private int Cl_F_TF = 0, ReCnt = 0 ;
        private int MaxLevel = 0, Kor_Pay = 0 ;

        private int Chang_Date_Close_Ver02 = 20200101;
        

        Dictionary<string, cls_Close_Mem> Clo_Mem = new Dictionary<string, cls_Close_Mem>();
        Dictionary<string, cls_Close_Sell> Clo_Sell = new Dictionary<string, cls_Close_Sell>();

        cls_Close_Sell[] C_Sell;

        cls_Connect_DB Search_Connect = new cls_Connect_DB();
        SqlConnection Search_Conn = null;

        double Sum_T_PV_001 = 0, Sum_T_PV_01 = 0;

        public frmClose_4()
        {
            InitializeComponent();
        }
        
        
     
        

        private void frmBase_From_Load(object sender, EventArgs e)
        {
            //if (this.DesignMode)
            //    return;

           

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

                DateTime dt = DateTime.Parse(FromEndDate.Substring(0, 4) + "-" + FromEndDate.Substring(4, 2) + "-" + FromEndDate.Substring(6, 2));
                string SDate3 = dt.AddMonths(1).ToShortDateString().Replace("-", "").Substring(0, 6) + "01";

                dt = DateTime.Parse(SDate3.Substring(0, 4) + "-" + SDate3.Substring(4, 2) + "-" + SDate3.Substring(6, 2));
                SDate3 = dt.AddDays(-1).ToShortDateString().Replace("-", "");

                ToEndDate  = SDate3;
                txt_To.Text = ToEndDate;

                Close_Base_Work();

                //string PayDate = "";

                //PayDate = ToEndDate.Substring(0, 4) + '-' + ToEndDate.Substring(4, 2) + '-' + ToEndDate.Substring(6, 2);
                //DateTime TodayDate = new DateTime();
                //TodayDate = DateTime.Parse(PayDate);
                //PayDate = TodayDate.AddDays(20).ToString("yyyy-MM-dd").Replace("-", "");

                ////DateTime dt = DateTime.Parse(FromEndDate.Substring(0, 4) + "-" + FromEndDate.Substring(4, 2) + "-" + FromEndDate.Substring(6, 2));
                ////string SDate3 = dt.AddMonths(1).ToShortDateString().Replace("-", "").Substring(0, 6) + "15";
                ////PayDate = SDate3;

                //mtxtPayDate.Text = PayDate;


                //Base_Sub_Grid_Set(FromEndDate);
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


        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {

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
            Tsql = "Select Isnull (Max(ToEndDate),'') From  tbl_CloseTotal_04 (nolock) ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt != 0)
            {
                Max_Toenddate = ds.Tables[base_db_name].Rows[0][0].ToString();

                //if (int.Parse(Max_Toenddate) < 20180101)
                //    Max_Toenddate = "20180701"; 
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

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            ////현재 월마감보다 크거나 같은 주간 마감이 잇어야 지 된다 그래야지 월마감 돌면서 그주간의 직급을 가져오기 때문에.
            //StrSql = "select ToEndDAte from tbl_CloseTotal_02 (nolock) ";
            //StrSql = StrSql + " Where left(ToEndDate,6) = '" + ToEndDate.Substring (0,6) + "'";


            //DataSet ds3 = new DataSet();
            //Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds3, this.Name, this.Text);
            //int ReCnt3 = Temp_Connect.DataSet_ReCount;

            //if (ReCnt3 <= 3 && ToEndDate.Substring(0, 6) != "201809")
            //{
            //    MessageBox.Show("마감 기간내에 미적용된 주간 마감이 있습니다. 주간 마감을 정산후에 다시 시도해 주십시요.");
            //    txt_To.Text = "";
            //    return;
            //}



        
            StrSql = "select Isnull(Count(Mbid),0) from tbl_SalesDetail  (nolock) ";            
            StrSql = StrSql + " Where SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " And TotalPrice  > 0 ";
            StrSql = StrSql + " And  Ga_Order = 0 ";
                                         
            

            DataSet ds = new DataSet();
            Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            txt_SellCnt.Text = "0";
            if (ReCnt != 0)            
                txt_SellCnt.Text =  ds.Tables[base_db_name].Rows[0][0].ToString();


            StrSql = "select Isnull(Count(Mbid),0) from tbl_SalesDetail  (nolock)  ";            
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
            PayDate = TodayDate.AddDays(15).ToString("yyyy-MM-dd").Replace("-", "");

            //DateTime dt = DateTime.Parse(FromEndDate.Substring(0, 4) + "-" + FromEndDate.Substring(4, 2) + "-" + FromEndDate.Substring(6, 2));
            //string SDate3 = dt.AddMonths(1).ToShortDateString().Replace("-", "").Substring(0, 6) + "15";
            //PayDate = SDate3;

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

            //StrSql = "Select ToEndDate From tbl_CloseTotal_04 (nolock) " ;
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
            tableLayoutPanel1.Enabled = false;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            int Close_Sucess_TF = 0;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            /*
              //김영수 팀장 월 반품 재정산 작업중 절대 건드리지 마세요.
            string StrSql = "Delete From tbl_ClosePay_04_Re_Ord Where ToEndDate ='" +  ToEndDate + "'"; 
            Temp_Connect.Insert_Data (StrSql, base_db_name);
            
            int Reutn_FLAG = 0, ReturnClose_FLAG = 0 ;


            StrSql = " Select ";
            StrSql = StrSql + " Ordernumber ";
            StrSql = StrSql + " From tbl_SalesDetail(nolock)";            
            StrSql = StrSql + " Where  (ReturnTF = 2 Or ReturnTF = 3 )  ";
            StrSql = StrSql + " And  Re_BaseOrdernumber in (Select Ordernumber From tbl_SalesDetail (nolock) Where ReturnTF = 1  And   Ga_Order = 0   )";
            StrSql = StrSql + " And  Ordernumber not in (Select Re_Ordernumber From tbl_ClosePay_04_Re_Ord (nolock))";   //''이미 마감 처리된 반품을 포함하지 마라.


            int tReCnt = 0;
            DataSet Dset4 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
            tReCnt = Search_Connect.DataSet_ReCount;
            
            if (tReCnt > 0) Reutn_FLAG = 1;



            //Reutn_FLAG = 0; 
            // '''우선은 막는다  2020년 신규 마케팅 관계로 우선 막는다. 테스트 버전이므로
            
            ReturnClose_FLAG = 0;
            if (Reutn_FLAG == 1)
                Return_Close_Retry_DayC(ref ReturnClose_FLAG);

            if (ReturnClose_FLAG == 0)
            {
                MessageBox.Show("반품 월 재정산 중에 문제가 발생했습니다. 업체에 문의해 주십시요.");
                Temp_Connect.Close_DB();
                return;
            }
        */



           // cls_Connect_DB Temp_Connect = new cls_Connect_DB();
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
            //Clo_Mem.Clear(); Clo_Mem = null;

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

            DateTime dt = DateTime.Parse(FromEndDate.Substring(0, 4) + "-" + FromEndDate.Substring(4, 2) + "-" + FromEndDate.Substring(6, 2));
            string SDate3 = dt.AddMonths(1).ToShortDateString().Replace("-", "").Substring(0, 6) + "01";

            dt = DateTime.Parse(SDate3.Substring(0, 4) + "-" + SDate3.Substring(4, 2) + "-" + SDate3.Substring(6, 2));
            SDate3 = dt.AddDays(-1).ToShortDateString().Replace("-", "");

            ToEndDate = SDate3;
            txt_To.Text = ToEndDate;

            Close_Base_Work();

            //string PayDate = "";

            //PayDate = ToEndDate.Substring(0, 4) + '-' + ToEndDate.Substring(4, 2) + '-' + ToEndDate.Substring(6, 2);
            //DateTime TodayDate = new DateTime();
            //TodayDate = DateTime.Parse(PayDate);
            //PayDate = TodayDate.AddDays(20).ToString("yyyy-MM-dd").Replace("-", "");

            ////DateTime dt = DateTime.Parse(FromEndDate.Substring(0, 4) + "-" + FromEndDate.Substring(4, 2) + "-" + FromEndDate.Substring(6, 2));
            ////string SDate3 = dt.AddMonths(1).ToShortDateString().Replace("-", "").Substring(0, 6) + "15";
            ////PayDate = SDate3;

            //mtxtPayDate.Text = PayDate;


            //Base_Sub_Grid_Set(FromEndDate);
        }



        private void Close_Work_Real(cls_Connect_DB Temp_Connect , SqlConnection Conn, SqlTransaction tran)
        {
            pg2.Minimum = 0;            pg2.Maximum = 38 ;
            pg2.Step = 1;               pg2.Value = 0;
            pg1.Step = 1;

            string StrSql = "";

            PayDate = mtxtPayDate.Text.Replace("-", "").Trim();

            ////마감돌리는 동안 매출 등록을 못하도록 하기 위해서 제일 먼저 체크 테이블인 집계 테이블을 만든다.
            StrSql = " EXEC Usp_Close4_Pro_E_400_Put_tbl_CloseTotal_Put1 '" + FromEndDate + "','" + ToEndDate + "','" + PayDate + "','" + cls_User.gid + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();



            //StrSql = "Update tbl_CloseTotal_04 SET ";
            //StrSql = StrSql + "  Temp01 = " + double.Parse(txtB1.Text);
            //StrSql = StrSql + " , Temp02 = " + double.Parse(txtB2.Text);           
            //StrSql = StrSql + " ,Temp11 = 2 ";
            //StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "'";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();



            StrSql = " EXEC Usp_Close4_Pro_A_200_001 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
         

            StrSql = " EXEC Usp_Close4_Pro_A_300_Sell_002 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
         

            StrSql = " EXEC Usp_Close4_Pro_A_400_Sell_003 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
         

            StrSql = " EXEC Usp_Close4_Pro_A_500_LevelCnt '" + FromEndDate + "','" + ToEndDate + "'"; 
            Temp_Connect.Insert_Data(StrSql, Conn, tran);            
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC Usp_Close4_Pro_A_600_CurGrade_OrgGrade_Put '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();


            StrSql = " EXEC Usp_Close4_Pro_B_100_Put_Down_PV_01 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();                       

            StrSql = " EXEC Usp_Close4_Pro_B_200_Put_Down_Active_01 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
                                 
            
            StrSql = " EXEC Usp_Close4_Pro_B_300_Put_Down_Reg_Mem_Cnt '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();



            
            StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "',-1";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            StrSql = " EXEC dbo.Usp_Close4_Pro_C_100_GiveGrade1 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "',-1";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            StrSql = " EXEC dbo.Usp_Close4_Pro_C_100_GiveGrade2 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "',-1";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            StrSql = " EXEC dbo.Usp_Close4_Pro_C_100_GiveGrade3 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "',-1";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            StrSql = " EXEC dbo.Usp_Close4_Pro_C_100_GiveGrade4 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "',-1";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            StrSql = " EXEC dbo.Usp_Close4_Pro_C_100_GiveGrade5 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "',-1";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            StrSql = " EXEC dbo.Usp_Close4_Pro_C_100_GiveGrade6 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();


            StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "',-1";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            StrSql = " EXEC dbo.Usp_Close4_Pro_C_100_GiveGrade7 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();



            StrSql = " Select Isnull(Max(LevelCnt),0) From tbl_ClosePay_04_Up (nolock) ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            MaxLevel = int.Parse (ds.Tables[base_db_name].Rows[0][0].ToString());

            int L_Cnt = MaxLevel;

            pg2.Maximum = pg2.Maximum + (MaxLevel * 12) + 13 ;

            while (L_Cnt >= 0)
            {

                if (L_Cnt == 3)
                    L_Cnt = 3;



                StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                StrSql = " EXEC dbo.Usp_Close4_Pro_C_100_GiveGrade8 '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt ; 
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();  //----

                StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                StrSql = " EXEC dbo.Usp_Close4_Pro_C_100_GiveGrade9 '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();   //----

                StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                StrSql = " EXEC dbo.Usp_Close4_Pro_C_105_GiveGrade10 '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();   //----

                StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                StrSql = " EXEC dbo.Usp_Close4_Pro_C_105_GiveGrade11 '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();   //----


                StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                StrSql = " EXEC dbo.Usp_Close4_Pro_C_105_GiveGrade12 '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();   //----


                StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                StrSql = " EXEC dbo.Usp_Close4_Pro_C_105_GiveGrade13 '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();   //----

                StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                StrSql = " EXEC dbo.Usp_Close4_Pro_C_105_GiveGrade14 '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();   //----


                StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                StrSql = " EXEC dbo.Usp_Close4_Pro_C_105_GiveGrade15 '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();   //----


                StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                StrSql = " EXEC dbo.Usp_Close4_Pro_C_105_GiveGrade16 '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();   //----


                StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                StrSql = " EXEC dbo.Usp_Close4_Pro_C_105_GiveGrade17 '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();   //----


                StrSql = " EXEC dbo.Usp_Close4_Pro_C_200_Down_PV_Limt_Reset '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                StrSql = " EXEC dbo.Usp_Close4_Pro_C_105_GiveGrade18 '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();   //-----



                StrSql = " EXEC dbo.Usp_Close4_Pro_C_300_GradeUpLine '" + FromEndDate + "','" + ToEndDate + "', " + L_Cnt;
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();   //----
                               
                L_Cnt--; 
            }


            

            StrSql = " EXEC dbo.Usp_Close4_Pro_D_110_Give_Allowance1 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_D_120_Give_Allowance2 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_D_130_Give_Allowance3 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_D_140_Give_Allowance4_5 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_D_150_Give_Allowance6 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_D_160_Leg_Sum_Pay '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_D_200_6_Month_OneGrade_Give_Allowance7 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

                       

            StrSql = " EXEC Usp_Close4_Pro_E_090_Put_Sum_Return_Remain_Pay_Pre '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();




            StrSql = " EXEC Usp_Close4_Pro_E_100_Put_Return_Pay '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC Usp_Close4_Pro_E_150_Put_Sum_Return_Remain_Pay '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();


            StrSql = " EXEC Usp_Close4_Pro_E_200_CalculateTruePayment '" + FromEndDate + "','" + ToEndDate + "',0";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC Usp_Close4_Pro_E_300_Chang_RetunPay_Table '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();



            StrSql = " EXEC Usp_Close4_Pro_E_400_Put_tbl_CloseTotal_Put1 '" + FromEndDate + "','" + ToEndDate + "','" + PayDate + "','" + cls_User.gid + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            StrSql = " EXEC Usp_Close4_Pro_E_400_Put_tbl_CloseTotal_Put2 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            StrSql = " EXEC Usp_Close4_Pro_E_400_Put_tbl_CloseTotal_Put3 '" + FromEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC Usp_Close4_Pro_E_500_MakeModForCheckRequirement1 '" + FromEndDate + "','" + ToEndDate + "','" + PayDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
                       
            StrSql = " EXEC Usp_Close4_Pro_E_600_ReadyNewForCheckRequirement1 '" + FromEndDate + "','" + ToEndDate + "','" + PayDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC Usp_Close4_Pro_F_100_Check_Close_Gid '" + FromEndDate + "','" + ToEndDate + "','" + cls_User.gid + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            
                                 
            //진마감이 다 돌았음을 알린다... 가마감 돌아도 되도록 체크를 한다.
            StrSql = " UpDate tbl_CloseTotal_04 SET  Real_FLAG  = 0 Where ToEndDate = '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            
        }


        private Boolean Check_UP_Grade_TF(int CurrentGrade, cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string StrSql = "";

            StrSql = " Select Isnull(Count(Mbid),0)    ";
            StrSql = StrSql + " From tbl_ClosePay_04 (nolock) ";
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
                up_Cnt = int.Parse(ds_T.Tables[base_db_name].Rows[0][0].ToString());
            }

            if (up_Cnt > 0)
                return true;
            else
                return false;
        }

        private void Make_Close_Table(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Minimum = 0; pg1.Maximum = 40; pg1.Refresh(); 
            
            pg1.Value = 10; ; pg1.Refresh(); 
            //pg1.PerformStep(); pg1.Refresh(); 

            string StrSql = "";

            StrSql = "INSERT INTO tbl_ClosePay_04 (Mbid,Mbid2,RecordMakeDate)  ";
            StrSql = StrSql + " Select   A.Mbid,A.Mbid2,  '" + ToEndDate + "' From tbl_Memberinfo AS A  (nolock)  ";
            StrSql = StrSql + " LEFT Join tbl_ClosePay_04 AS B  (nolock) ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 ";
            StrSql = StrSql + " Where b.Mbid Is Null " ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
            pg1.Value = 20; pg1.Refresh(); 


            StrSql = "INSERT INTO tbl_ClosePay_04_Sell (Mbid,Mbid2,SellCode , RecordMakeDate)  ";
            StrSql = StrSql + " Select  distinct A.Mbid,A.Mbid2, A.SellCode, '" + ToEndDate + "' From tbl_SalesDetail AS A   (nolock) ";
            StrSql = StrSql + " LEFT Join tbl_ClosePay_04_Sell AS B (nolock)  ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 And A.SellCode = B.SellCode ";
            StrSql = StrSql + " Where  A.SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And    A.SellDate <= '" + ToEndDate + "'" ;
            StrSql = StrSql + " And b.Mbid Is Null ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
            pg1.Value = 30;  pg1.Refresh(); 


             StrSql = "INSERT INTO tbl_ClosePay_04_Sell (Mbid,Mbid2,SellCode, RecordMakeDate) ";
            StrSql = StrSql + " Select  distinct A.Mbid,A.Mbid2, A.SellCode,   '" + ToEndDate + "'  From tbl_Sham_Sell AS A   (nolock) ";
            StrSql = StrSql + " LEFT Join tbl_ClosePay_04_Sell AS B  (nolock) ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 And A.SellCode = B.SellCode";            
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
    
            StrSql = "Update tbl_ClosePay_04 SET StopDate = ISNULL(B.PayStop_Date,'')" ;
           StrSql = StrSql + " FROM  tbl_ClosePay_04  A,";
           StrSql = StrSql + " (";
           StrSql = StrSql + " Select    PayStop_Date,Mbid,Mbid2   From tbl_Memberinfo   (nolock) ";
           StrSql = StrSql + " Where PayStop_Date <= '" + ToEndDate + "'";
           StrSql = StrSql + " And   PayStop_Date <>'' ";
           StrSql = StrSql + " ) B";
           StrSql = StrSql + "  Where a.Mbid = b.Mbid And a.Mbid2 = b.Mbid2";
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 

    
            StrSql = "Update tbl_ClosePay_04 SET LeaveDate=ISNULL(B.LeaveDate,'')";
           StrSql = StrSql + " FROM  tbl_ClosePay_04  A,";
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
    
            StrSql = "Update tbl_ClosePay_04 SET" ;
            StrSql = StrSql +" BankCode=ISNULL(B.BankCode,'')";
            StrSql = StrSql +" ,Cpno=ISNULL(B.Cpno,'')";
            StrSql = StrSql +" ,BankAcc=ISNULL(B.bankaccnt,'')";
            StrSql = StrSql +" ,BankOwner=ISNULL(B.BankOwner,'')";
            StrSql = StrSql +" ,M_Name=ISNULL(B.M_Name,'')";
            StrSql = StrSql +" ,BusCode=ISNULL(B.businesscode,'')";

            StrSql = StrSql + " ,Us_Num=ISNULL(B.Us_Num,0)";
    
            StrSql = StrSql +" ,Saveid=ISNULL(B.Saveid,'')";
            StrSql = StrSql +" ,Saveid2=ISNULL(B.Saveid2,0)";
            StrSql = StrSql +" ,LineCnt=ISNULL(B.LineCnt,0)";
            
            StrSql = StrSql +" ,Nominid=ISNULL(B.Nominid,'')";
            StrSql = StrSql +" ,Nominid2=ISNULL(B.Nominid2,0)";
            StrSql = StrSql +" ,N_LineCnt=ISNULL(B.N_LineCnt,0)";
    
        //    StrSql = StrSql +" ,BaseMbid=ISNULL(B.BaseMbid,'')"
        //    StrSql = StrSql +" ,BaseMbid2=ISNULL(B.BaseMbid2,0)"             
    
           StrSql = StrSql +" ,Sell_Mem_TF = ISNULL(B.Sell_Mem_TF,0)" ;
           StrSql = StrSql + " ,GiBu_=ISNULL(B.GiBu_,0)";        
            
            StrSql = StrSql +" ,RegTime=  replace(ISNULL(B.regtime,''),'-','')";
            StrSql = StrSql +"  FROM  tbl_ClosePay_04  A,";
    
            StrSql = StrSql +" (";
            StrSql = StrSql +" Select   BankCode,Cpno,bankaccnt,BankOwner,M_Name,businesscode,ED_Date,";
            StrSql = StrSql +" Saveid,Saveid2,LineCnt,Nominid,Nominid2,N_LineCnt,";
            StrSql = StrSql + " Mbid,Mbid2,regtime , Sell_Mem_TF , GiBu_  , Us_Num ";
            StrSql = StrSql +"  From tbl_Memberinfo   (nolock)   ";
            StrSql = StrSql +" ) B";
            StrSql = StrSql +" Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And a.Mbid2 = b.Mbid2";
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 



            //G8_TF
        }



        private void Put_Member_Base_Info_2014_1001(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();

            string StrSql = "";

            StrSql = "Update tbl_ClosePay_04 SET";
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
            StrSql = StrSql + "  FROM  tbl_ClosePay_04  A,";

            StrSql = StrSql + " (";
            StrSql = StrSql + " Select   BankCode,Cpno,BankAcc,BankOwner,M_Name,BusCode,";
            StrSql = StrSql + " Saveid,Saveid2,LineCnt,Nominid,Nominid2,N_LineCnt,";
            StrSql = StrSql + " Mbid,Mbid2,regtime , Sell_Mem_TF,GiBu_ , LEaveDate , StopDate ";
            StrSql = StrSql + "  From tbl_ClosePay_04_Mod_0630 (nolock)   ";
            StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And a.Mbid2 = b.Mbid2";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }
        




        private void Put_LevelCnt_Update(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string StrSql = " Select Mbid,Mbid2 From Tbl_Memberinfo  (nolock) Where Saveid='**'   "; 
            string Mbid =""; int Mbid2 = 0 ; 
            ReCnt = 0;
            DataSet Dset4 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
            ReCnt = Search_Connect.DataSet_ReCount;            

            if (ReCnt > 0)
            {
                pg1.Value = 0; pg1.Maximum = ReCnt + 1 ;
                pg1.PerformStep(); pg1.Refresh(); 

                pg1.Value = 0; pg1.Maximum = ReCnt;

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Mbid = Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() ;
                    Mbid2 = int.Parse (Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                    
                    StrSql = "Update tbl_ClosePay_04 SET " ;
                    StrSql = StrSql + " LevelCnt=ISNULL(B.lvl,0) " ;
                    StrSql = StrSql + " FROM  tbl_ClosePay_04  A, " ;
            
                    StrSql = StrSql + " (Select    empid0,empid,lvl " ;
                    StrSql = StrSql + " From ufn_GetSubTree_Pay_Mem_04('" + Mbid + "'," + Mbid2 ;
                    StrSql = StrSql + ") Where pos <>0 " ;
                    StrSql = StrSql + " ) B" ;
            
                    StrSql = StrSql + " Where A.Mbid=B.empid0 " ;
                    StrSql = StrSql + " And   A.Mbid2=B.empid ";

                    Temp_Connect.Insert_Data(StrSql, Conn, tran);                   

                    pg1.PerformStep(); pg1.Refresh();
                } // for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)

            } // (ReCnt != 0)



            
            StrSql = "Select Max(LevelCnt) From tbl_ClosePay_04  ";
                        
            SqlDataReader sr = null;            
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;

            while (sr.Read())
            {
                MaxLevel = int.Parse(sr.GetValue(0).ToString()); 
            }

            sr.Close(); sr.Dispose();

        }
        






        private void Put_Sell_Date(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
         
            pg1.Value = 0; pg1.Maximum = 2    ;
            pg1.PerformStep(); pg1.Refresh(); 

            string StrSql = "";
    
        
            if  (Cl_F_TF == 0) 
            {
                StrSql = " Update tbl_ClosePay_04_Sell SET" ;
                StrSql = StrSql + " BeAmount = IsNull(b.A1, 0)";
                StrSql = StrSql + " ,BeCash=ISNULL(B.A2,0)";
                StrSql = StrSql + " ,BeCard=ISNULL(B.A3,0)";
                StrSql = StrSql + " ,BeBank=ISNULL(B.A4,0)";
                StrSql = StrSql + " ,BeTotalPV=ISNULL(B.A5,0)";
                StrSql = StrSql + " ,BeTotalCV=ISNULL(B.A6,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_04_Sell  A,";

                StrSql = StrSql + " (";
                StrSql = StrSql + " Select  Sum(TotalPrice) AS A1,Sum(InputCash) AS A2, ";
                StrSql = StrSql + " Sum(InputCard) AS A3 ,        Sum(InputPassbook) AS A4 , ";
                StrSql = StrSql + " Sum(TotalPV) AS A5,           Sum(TotalCV) AS A6, ";
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


                StrSql = " Update tbl_ClosePay_04_Sell SET";
                StrSql = StrSql + "  BeReAmount = -IsNull(b.A1, 0)";
                StrSql = StrSql + " ,BeReCash=-ISNULL(B.A2,0)";
                StrSql = StrSql + " ,BeReCard=-ISNULL(B.A3,0)";
                StrSql = StrSql + " ,BeReBank=-ISNULL(B.A4,0)";
                StrSql = StrSql + " ,BeReTotalPV=-ISNULL(B.A5,0)";
                StrSql = StrSql + " ,BeReTotalCV=-ISNULL(B.A6,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_04_Sell  A,";

                StrSql = StrSql + " (";
                StrSql = StrSql + " Select  Sum(TotalPrice) AS A1,Sum(InputCash) AS A2, ";
                StrSql = StrSql + " Sum(InputCard) AS A3 ,        Sum(InputPassbook) AS A4 , ";
                StrSql = StrSql + " Sum(TotalPV) AS A5,           Sum(TotalCV) AS A6, ";
                StrSql = StrSql + " Mbid,Mbid2 , SellCode";
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


                StrSql = " Update tbl_ClosePay_04_Sell SET";
                StrSql = StrSql + " BeShamSell = IsNull(b.A1, 0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_04_Sell  A,";
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



            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = "Select Mbid,Mbid2 ,OrderNumber, SellCode , TotalPrice , InputCash , InputCard , InputPassbook , TotalPV , TotalCV , Re_BaseOrderNumber , SellDate  ";
            StrSql = StrSql + " From    tbl_SalesDetail  (nolock)  ";
            StrSql = StrSql + " Where   SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     TotalPV  + TotalCV < 0 ";
            StrSql = StrSql + " And     SellCode <> '' ";


            DataSet Dset = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset);
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
                if (ReCnt2 > 0)
                {
                    T_SellDate = Dset2.Tables[base_db_name].Rows[0]["SellDate"].ToString();
                }
                Dset2.Dispose () ;

                if (T_SellDate != "")
                {
                    StrSql = "Select ToEndDate,  PayDate   From  tbl_CloseTotal_04 (nolock) ";
                    StrSql = StrSql + " Where FromEndDate <='" + T_SellDate + "'";
                    StrSql = StrSql + " And   ToEndDate >='" + T_SellDate + "'";

                    DataSet Dset3 = new DataSet();
                    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset3);
                    int ReCnt3 = Search_Connect.DataSet_ReCount;
                    if (ReCnt3 > 0)
                    {
                        RePayDate = Dset3.Tables[base_db_name].Rows[0]["PayDate"].ToString();
                    }

                    Dset3.Dispose();
                }

                if (RePayDate != "")
                {
                    if (int.Parse(Rs_SellDate) > int.Parse(RePayDate))
                    {
                        StrSql = "Update tbl_ClosePay_04_Sell SET ";
                        StrSql = StrSql + "  DayReAmount = DayReAmount + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
                        StrSql = StrSql + " ,DayReCash = DayReCash + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["InputCash"].ToString());
                        StrSql = StrSql + " ,DayReCard = DayReCard + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["InputCard"].ToString());
                        StrSql = StrSql + " ,DayReBank = DayReBank + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["InputPassbook"].ToString());
                        StrSql = StrSql + " ,DayReTotalPV = DayReTotalPV + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
                        StrSql = StrSql + " ,DayReTotalCV = DayReTotalCV + " + -double.Parse(Dset.Tables[base_db_name].Rows[fi_cnt]["TotalCV"].ToString());
                        StrSql = StrSql + "  Where Mbid  = '" + Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "'";
                        StrSql = StrSql + "  And   Mbid2 =  " + Dset.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                        StrSql = StrSql + "  And   SellCode =  '" + Dset.Tables[base_db_name].Rows[fi_cnt]["SellCode"].ToString() + "'";

                        //Temp_Connect.Insert_Data(StrSql, Conn, tran);

                        t_qu[t_qu_Cnt] = StrSql;
                        t_qu_Cnt++;
                    }
                }

                pg1.PerformStep(); pg1.Refresh();
            }

            Dset.Dispose();

            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

           
            pg1.Value = 0; pg1.Maximum = 4  ;
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_04_Sell SET";
            StrSql = StrSql + " DayAmount = IsNull(b.A1, 0)";
            StrSql = StrSql + " ,DayCash=ISNULL(B.A2,0)";
            StrSql = StrSql + " ,DayCard=ISNULL(B.A3,0)";
            StrSql = StrSql + " ,DayBank=ISNULL(B.A4,0)";
            StrSql = StrSql + " ,DayTotalPV=ISNULL(B.A5,0)";
            StrSql = StrSql + " ,DayTotalCV=ISNULL(B.A6,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_04_Sell  A,";

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
            pg1.PerformStep(); pg1.Refresh();  //구매 종류 별로 넣는다. 합계를 +판매에 대해서만


        
            StrSql = " Update tbl_ClosePay_04_Sell SET";
            StrSql = StrSql + " DayShamSell = IsNull(b.A1, 0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_04_Sell  A,";
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







            



    
            StrSql = " Update tbl_ClosePay_04_Sell Set";
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
       

            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql + " SellPrice01=ISNULL(B.A1,0) " ;
            StrSql = StrSql + ",SellPv01=ISNULL(B.A2,0) " ;
            StrSql = StrSql + ",SellCv01=ISNULL(B.A2,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, " ;
    
            StrSql = StrSql + " (Select Sum(SumAmount-SumReAmount) AS A1, Sum(SumTotalPV-SumReTotalPV) AS A2 , Sum(SumTotalCV-SumReTotalCV) AS A3 , Sum(SumShamSell) AS A4 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_04_Sell " ;
            StrSql = StrSql + " Where  (SellCode ='01' OR SellCode ='Auto') ";
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql + " SellSham01 = ISNULL(B.A4,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, " ;
    
            StrSql = StrSql + " (Select  Sum(SumShamSell) AS A4 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_04_Sell " ;
            StrSql = StrSql + " Where SellCode ='01'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql + " SellPrice02 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + ",SellPv02 = ISNULL(B.A2,0) " ;
            StrSql = StrSql + ",SellCv02 = ISNULL(B.A3,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, " ;
    
            StrSql = StrSql + " (Select Sum(SumAmount-SumReAmount) AS A1, Sum(SumTotalPV-SumReTotalPV) AS A2 , Sum(SumTotalCV-SumReTotalCV) AS A3 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_04_Sell " ;
            StrSql = StrSql + " Where SellCode ='02'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql + " SellPrice03 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + ",SellPv03 = ISNULL(B.A2,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, " ;

            StrSql = StrSql + " (Select Sum(SumAmount-SumReAmount) AS A1, Sum(SumTotalPV-SumReTotalPV) AS A2 , Sum(SumTotalCV-SumReTotalCV) AS A3 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_04_Sell " ;
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
  
            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql + "  DayPrice01 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,DayPv01 = ISNULL(B.A2,0) " ;
            StrSql = StrSql + " ,DayCV01 = ISNULL(B.A3,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, " ;
    
            StrSql = StrSql + " (Select Sum(DayAmount-DayReAmount) AS A1 , Sum(DayTotalPV-DayReTotalPV) AS A2 ,Sum(DayTotalCV-DayReTotalCV) AS A3 ,Sum(DayShamSell) AS A4  ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_04_Sell " ;
            StrSql = StrSql + " Where  (SellCode ='01' OR SellCode ='Auto')";
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql + " DaySham01 = ISNULL(B.A4,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, " ;
    
            StrSql = StrSql + " (Select  Sum(DayShamSell) AS A4 ,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_04_Sell " ;
            StrSql = StrSql + " Where SellCode ='01'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql + "  DayPrice02 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,DayPv02 = ISNULL(B.A2,0) " ;
            StrSql = StrSql + " ,DayCV02 = ISNULL(B.A3,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, " ;
            StrSql = StrSql + " (Select Sum(DayAmount-DayReAmount) AS A1 ,Sum(DayTotalPV-DayReTotalPV) AS A2  ,Sum(DayTotalCV-DayReTotalCV) AS A3,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_04_Sell " ;
            StrSql = StrSql + " Where SellCode ='02'" ;
            StrSql = StrSql + " Group By Mbid,Mbid2 " ;
            StrSql = StrSql + " ) B" ;
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh(); 
    
    
            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql + "  DayPrice03 = ISNULL(B.A1,0) " ;
            StrSql = StrSql + " ,DayPv03 = ISNULL(B.A2,0) " ;
            //StrSql = StrSql + " ,DayCV03 = ISNULL(B.A3,0) " ;
    
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, " ;

            StrSql = StrSql + " (Select Sum(DayAmount-DayReAmount) AS A1 , Sum(DayTotalPV-DayReTotalPV) AS A2  ,Sum(DayTotalCV-DayReTotalCV) AS A3,Mbid,Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_04_Sell " ;
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
    
            StrSql = " Update tbl_ClosePay_04 SET";
            StrSql = StrSql + " ReqTF1 = 1 ";
            //StrSql = StrSql + " ,CurPoint = 1 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " ReqDate1='" + ToEndDate + "'";
            StrSql = StrSql + " Where ReqDate1=''";
            StrSql = StrSql + " And ReqTF1 >= 1 ";
        
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        //'''''-------------------------------------------------//////////////////////////////
        }


        private void Put_Down_SumPV_Mon_PR(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string StrSql = "";



            //StrSql = " Update tbl_ClosePay_04 SET";
            //StrSql = StrSql + " D_PV = IsNull(b.A1, 0)";
            //StrSql = StrSql + " FROM  tbl_ClosePay_04  A,";
            //StrSql = StrSql + " (";
            //StrSql = StrSql + " Select  Sum(SellPV01 + SellPV02 +SellPV03) A1 ";
            //StrSql = StrSql + " ,Nominid,Nominid2 ";
            //StrSql = StrSql + " From tbl_ClosePay_04 (nolock)  ";
            //StrSql = StrSql + " Where   Sell_Mem_TF = 1 ";
            //StrSql = StrSql + " Group by Nominid , Nominid2 ";
            //StrSql = StrSql + " ) B";
            //StrSql = StrSql + " Where a.Mbid = b.Nominid ";
            //StrSql = StrSql + " And   a.Mbid2 = b.Nominid2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            pg1.Value = 0; pg1.Maximum = MaxLevel + 2;
            pg1.PerformStep(); pg1.Refresh();

            int Cnt = MaxLevel;

            while (Cnt >= 0)
            {

                StrSql = "Update tbl_ClosePay_04 SET ";
                StrSql = StrSql + " Cur_10_Cnt_1 =  ISNULL(B.A2,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + "Select    Sum(Cur_10_Cnt_1 + Cur_10_Cnt_2  ) A2 ";
                StrSql = StrSql + " ,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_04 ";
                StrSql = StrSql + " Where (  Cur_10_Cnt_1 + Cur_10_Cnt_2  ) <>0   ";
                StrSql = StrSql + " And   LevelCnt =" + Cnt;
                StrSql = StrSql + " And   LineCnt =  1 ";
                StrSql = StrSql + " Group By Saveid,Saveid2   ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                StrSql = "Update tbl_ClosePay_04 SET ";
                StrSql = StrSql + " Cur_10_Cnt_1 =  Cur_10_Cnt_1 + ISNULL(B.A2,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + "Select    Count(Mbid ) A2 ";
                StrSql = StrSql + " ,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_04 ";
                StrSql = StrSql + " Where ( DayPrice01 + DayPrice02 +DayPrice03 ) >= 100000   ";
                StrSql = StrSql + " And   LevelCnt =" + Cnt;
                StrSql = StrSql + " And   LineCnt =  1 ";
                StrSql = StrSql + " Group By Saveid,Saveid2   ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);




                StrSql = "Update tbl_ClosePay_04 SET ";
                StrSql = StrSql + " Cur_10_Cnt_2 =  ISNULL(B.A2,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + "Select    Sum(Cur_10_Cnt_1 + Cur_10_Cnt_2  ) A2 ";
                StrSql = StrSql + " ,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_04 ";
                StrSql = StrSql + " Where (  Cur_10_Cnt_1 + Cur_10_Cnt_2   ) <>0   ";
                StrSql = StrSql + " And   LevelCnt =" + Cnt;
                StrSql = StrSql + " And   LineCnt =  2 ";
                StrSql = StrSql + " Group By Saveid,Saveid2   ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();


                StrSql = "Update tbl_ClosePay_04 SET ";
                StrSql = StrSql + " Cur_10_Cnt_2 =  Cur_10_Cnt_2 + ISNULL(B.A2,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + "Select    Count(Mbid ) A2 ";
                StrSql = StrSql + " ,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_04 ";
                StrSql = StrSql + " Where ( DayPrice01 + DayPrice02 +DayPrice03 ) >= 100000   ";
                StrSql = StrSql + " And   LevelCnt =" + Cnt;
                StrSql = StrSql + " And   LineCnt =  2 ";
                StrSql = StrSql + " Group By Saveid,Saveid2   ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);

                Cnt = Cnt - 1;

            }

        }


        private void CurPoint_Put_2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string S_ToEndDate)
        {

            pg1.Value = 0; pg1.Maximum = 7    ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";   
         
            StrSql = " Update tbl_ClosePay_04 SET"    ;
            StrSql = StrSql + " CurPoint_Date_2_Gap = 0 "   ;
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    

            StrSql = "Update tbl_ClosePay_04 SET "   ;
            StrSql = StrSql + " CurPoint_SellPV = ISNULL(B.A1, 0 )   "   ;
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, "   ;
    
            StrSql = StrSql + " (Select Sum(TotalPV) A1,  Mbid ,Mbid2   "   ;
            StrSql = StrSql + " From tbl_SalesDetail (nolock)"   ;
            StrSql = StrSql + " Where   SellDate <='" + S_ToEndDate + "'"   ;
            StrSql = StrSql + " And      (SellCode ='01' OR SellCode ='Auto') ";
            StrSql = StrSql + " And  Ga_Order = 0 ";
            StrSql = StrSql + " Group By Mbid,Mbid2"   ;
            StrSql = StrSql + " ) B"   ;
    
            StrSql = StrSql + " Where A.Mbid  = B.Mbid "   ;
            StrSql = StrSql + " And   A.Mbid2  = B.Mbid2 "   ;
      
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();
    
    
            StrSql = " Update tbl_ClosePay_04 SET"   ;
            StrSql = StrSql + " CurPoint = 2 "   ;
            StrSql = StrSql + " Where CurPoint_SellPV >= 250000 "   ;
            StrSql = StrSql + " And Sell_Mem_TF = 0 "   ;
            
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();
        
       
            StrSql = "Update tbl_ClosePay_04 Set "   ;
            StrSql = StrSql + " CurPoint_Date_2='" + S_ToEndDate + "'"   ;
            StrSql = StrSql + " Where CurPoint_Date_2=''"   ;
            StrSql = StrSql + " And CurPoint = 2 "   ;
        
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();
    
            StrSql = "Update tbl_ClosePay_04 Set "   ;
            StrSql = StrSql + " CurPoint_Date_2_Gap =  DateDiff(D, Regtime, CurPoint_Date_2) "   ;
            StrSql = StrSql + " Where CurPoint_Date_2 ='" + S_ToEndDate + "'"   ;
        
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    pg1.PerformStep(); pg1.Refresh();
    
    
            StrSql = "Update tbl_ClosePay_04 Set "   ;
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


           StrSql = " Update tbl_ClosePay_04 SET"  ;
            StrSql = StrSql + " CurPoint_Date_3_Gap = 0 "  ;
            
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
                   pg1.PerformStep(); pg1.Refresh();
 
            StrSql = " Update tbl_ClosePay_04 SET"  ;
            StrSql = StrSql + " CurPoint = 3 "  ;
            StrSql = StrSql + " Where CurPoint_SellPV >= 750000 "  ;
            StrSql = StrSql + " And CurPoint_Date_2 <> '' "  ;
            StrSql = StrSql + " And Sell_Mem_TF = 0 "  ;
            
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
                   pg1.PerformStep(); pg1.Refresh();     
       
            StrSql = "Update tbl_ClosePay_04 Set "  ;
            StrSql = StrSql + " CurPoint_Date_3='" + S_ToEndDate + "'"  ;
            StrSql = StrSql + " Where CurPoint_Date_3=''"  ;
            StrSql = StrSql + " And CurPoint = 3 "  ;
        
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
                   pg1.PerformStep(); pg1.Refresh(); 
    
             StrSql = "Update tbl_ClosePay_04 Set "  ;
             StrSql = StrSql + " CurPoint_Date_3_Gap =  DateDiff(D, CurPoint_Date_2, CurPoint_Date_3) ";
            StrSql = StrSql + " Where CurPoint_Date_3 ='" + S_ToEndDate + "'"  ;
        
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
                   pg1.PerformStep(); pg1.Refresh();
    
            StrSql = "Update tbl_ClosePay_04 Set "  ;
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

            //StrSql = "Select Isnull(Max(ToEndDate), '')  From tbl_CloseTotal_04 (nolock) ";   //'''--직급마감에서 전달 마감일자를 알아온다.
            //StrSql = StrSql  + " Where LEFT(ToEndDate,6) < '" + FromEndDate.Substring(0,6) + "'"  ;    // '''--전달마감을 알아온다.

            //ReCnt = 0;
            //DataSet Dset = new DataSet();
            //Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset);
            //ReCnt = Search_Connect.DataSet_ReCount;
            ////Dset.Tables[base_db_name].Rows[0][0].ToString();

            //if (ReCnt >0)
            //{
            //    SDate = Dset.Tables[base_db_name].Rows[0][0].ToString();
            //}
            //pg1.PerformStep(); pg1.Refresh();
            
    
            //if (SDate == "") return ; 

    
            StrSql = "Update tbl_ClosePay_04 SET "  ;
            StrSql = StrSql + "  OneGrade =ISNULL(B.OneGrade,0) ";
            StrSql = StrSql + "  ,CurGrade =ISNULL(B.CurGrade,0) ";            
            StrSql = StrSql  + " FROM  tbl_ClosePay_04  A, "   ;

            StrSql = StrSql + " (Select  CurGrade , OneGrade , Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
            StrSql = StrSql  + " Where ToEndDate = '" + ToEndDate  +  "'"   ;
            StrSql = StrSql  + " ) B"   ;
    
            StrSql = StrSql  + " Where A.Mbid=B.Mbid "   ;
            StrSql = StrSql  + " And   A.Mbid2=B.Mbid2 "   ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            

            //StrSql = "Update tbl_ClosePay_04 SET ";
            //StrSql = StrSql + "  OneGrade = 60 ";
            //StrSql = StrSql + " Where Cur_10_Cnt_1 >= 25 ";
            //StrSql = StrSql + " And  Cur_10_Cnt_2 >= 25 ";
            //StrSql = StrSql + " And  CurGrade >= 60";
            //StrSql = StrSql + " And  OneGrade < 60";
            
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();

       }

        private void Put_Self_PV( cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran )
       {
            pg1.Value = 0; pg1.Maximum = 4  ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
            string SDate3 = "";

            DateTime dt = DateTime.Parse(ToEndDate.Substring(0, 4) + "-" + ToEndDate.Substring(4, 2) + "-" + ToEndDate.Substring(6, 2) );
            SDate3 = dt.AddMonths(-3).ToShortDateString().Replace("-", "");
                       

    
            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql +  " Self_M3_PV =  ISNULL(B.a1,0) ";
            StrSql = StrSql +  " FROM  tbl_ClosePay_04  A, ";

            StrSql = StrSql +  " (";
            StrSql = StrSql +  " Select   Sum(Se.TotalPV - Isnull( Bs_R.TotalPV, 0 ))  a1 , Se.Mbid , Se.Mbid2 ";
            StrSql = StrSql +  " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate <= '" + ToEndDate + "'";

            StrSql = StrSql +  " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql +  " And   Se.SellDate  >='" + SDate3 + "'";
            StrSql = StrSql +  " And   Se.SellDate <='" + ToEndDate + "'";
            StrSql = StrSql + " And  Se.Ga_Order = 0 ";
            StrSql = StrSql +  " Group by Se.mbid, Se.mbid2 ";
            StrSql = StrSql +  " ) B";
            StrSql = StrSql +  " Where A.Mbid=B.Mbid ";
            StrSql = StrSql +  " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();





            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql + " Cur_PV_M3_1 =  ISNULL(B.a1,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, " ;

            StrSql = StrSql + " (" ;
            StrSql = StrSql + " Select   Sum(Cur_PV_1)  a1 , Mbid , Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_04_Mod  (nolock) " ;
            StrSql = StrSql + " WHERE ToEndDate  >='" + SDate3 + "'" ;
            StrSql = StrSql + " Group by mbid, mbid2 " ;
            StrSql = StrSql + " ) B" ;
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 " ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql + " Cur_PV_M3_2 =  ISNULL(B.a1,0) " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, " ;

            StrSql = StrSql + " (" ;
            StrSql = StrSql + " Select   Sum(Cur_PV_2)  a1 , Mbid , Mbid2 " ;
            StrSql = StrSql + " From tbl_ClosePay_04_Mod  (nolock) " ;
            StrSql = StrSql + " WHERE ToEndDate  >='" + SDate3 + "'" ;
            StrSql = StrSql + " Group by mbid, mbid2 " ;
            StrSql = StrSql + " ) B" ;
            StrSql = StrSql + " Where A.Mbid=B.Mbid " ;
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

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


            StrSql = "Select Isnull(Max(ToEndDate),'') , Isnull(Max(FromEndDate),'') From tbl_ClosePay_04 (nolock) ";

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
                StrSql = "Select Isnull(Max(ToEndDate),'') , Isnull(Max(FromEndDate),'') From tbl_ClosePay_04  (nolock) ";
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
                StrSql = "Select Isnull(Max(ToEndDate),'') , Isnull(Max(FromEndDate),'') From tbl_ClosePay_04  (nolock)  ";
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
                StrSql = StrSql + " From tbl_ClosePay_04_Mod (nolock) "  ; 
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
                        StrSql = "Select Mbid,Mbid2 From tbl_ClosePay_04_Mod  (nolock)  ";
                        StrSql = StrSql + "  Where Mbid  = '" + Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "'";
                        StrSql = StrSql + "  And   Mbid2 =  " + Dset4.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                        StrSql = StrSql + "  And   ToEndDate  = '" + Dset4.Tables[base_db_name].Rows[fi_cnt]["ToEndDate"].ToString() + "'";
                        StrSql = StrSql +  " And   Allowance2  > 0 " ;
                                                
                        DataSet Dset5 = new DataSet();
                        Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset5);                        
                        int ReCnt5 = Search_Connect.DataSet_ReCount;

                        if (ReCnt5 <= 0)
                        {
                            StrSql = "Update tbl_ClosePay_04 SET ";
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

            Clo_Mem.Clear();
            
            StrSql = "Select Mbid,Mbid2, M_Name, Saveid, Saveid2, Nominid, Nominid2, LineCnt , N_LineCnt, LeaveDate, StopDate  ";
            StrSql = StrSql + " ,DayPV01, DayPV02 , DayPV03, SellPV01 , SellPV02 ,SellPV03 ";
            StrSql = StrSql + " ,ReqTF1, ReqTF2 " ;
            StrSql = StrSql + " ,CurGrade" ;
            StrSql = StrSql + " , OneGrade  ";
            StrSql = StrSql + "  From tbl_ClosePay_04 ";

            
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

                t_c_mem.CurGrade = int.Parse(sr.GetValue(19).ToString());
                t_c_mem.OneGrade = int.Parse(sr.GetValue(20).ToString());
               // t_c_mem.CurPoint = int.Parse(sr.GetValue(20).ToString());
                
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
            StrSql = StrSql + " LEFT JOIN tbl_ClosePay_04 Ce1 ON Ce1.Mbid = SE.Mbid And Ce1.Mbid2 = SE.Mbid2";

            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) >= 0 ";    
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And  Se.Ga_Order = 0 ";
            
            StrSql = StrSql + " And   Ce1.Mbid2 Is not null ";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";

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


        private void Put_Down_PV_01(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran , int i)
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
                            StrSql = "Update tbl_ClosePay_04 SET ";
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


            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + "   Sum_PV_1 = Be_PV_1 + Cur_PV_1 - Cut_PV_4_1 ";
            StrSql = StrSql + "  ,Sum_PV_2 = Be_PV_2 + Cur_PV_2 - Cut_PV_4_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
    
        }




        private void Put_Down_SumPV(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string StrSql = "";

           


            //이번달의 반품과 실매출을 합산하거를 뽑아온다.....
            StrSql = " Update tbl_ClosePay_04 SET";
            StrSql = StrSql + " Day_Sum_PV = IsNull(b.A1, 0) + IsNull(b.A2, 0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select  Sum(BS1.TotalPV) AS A1 ,  IsNull(Sum(Bs_R.TotalPV), 0) A2 ";
            StrSql = StrSql + " ,BS1.Mbid,BS1.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail (nolock) BS1  ";
            StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " Where   BS1.SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     BS1.SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     BS1.TotalPV  + BS1.TotalCV >= 0 ";
            StrSql = StrSql + " And     BS1.TotalPV > 0 ";
            StrSql = StrSql + " And     BS1.SellCode <> '' ";
            StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
            StrSql = StrSql + " Group By BS1.Mbid,BS1.Mbid2 ";
            StrSql = StrSql + " Having Sum(BS1.TotalPV) + IsNull(Sum(Bs_R.TotalPV), 0) >= 0";

            //StrSql = StrSql + " Where   SellDate >= '" + FromEndDate + "'";
            //StrSql = StrSql + " And     SellDate <= '" + ToEndDate + "'";            
            //StrSql = StrSql + " And   Ga_Order = 0 ";
            //StrSql = StrSql + " Group By Mbid,Mbid2";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            if (int.Parse(ToEndDate) <= 20150630)
            {
                StrSql = " Update tbl_ClosePay_04 SET";
                StrSql = StrSql + " Day_Sum_PV = 100";
                StrSql = StrSql + " Where Mbid2 = 55666 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);

                pg1.Value = 0; pg1.Maximum = MaxLevel + 6;
                pg1.PerformStep(); pg1.Refresh();
            }
            else
            {
                StrSql = " Update tbl_ClosePay_04 SET";
                StrSql = StrSql + " Day_Sum_PV = 200";
                StrSql = StrSql + " Where Mbid2 = 55666 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            //StrSql = " Update tbl_ClosePay_04 SET";
            //StrSql = StrSql + " Day_Sum_PV = 200";
            //StrSql = StrSql + " Where Mbid2 = 55666 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);



            int Cnt = MaxLevel;


           

            while (Cnt >= 0)
            {

                StrSql = "Update tbl_ClosePay_04 SET ";
                StrSql = StrSql + " G_Cur_PV =  ISNULL(B.A2,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + "Select    Sum(G_Cur_PV +  Day_Sum_PV  ) A2 ";
                StrSql = StrSql + " ,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_04 ";
                StrSql = StrSql + " Where (  G_Cur_PV + Day_Sum_PV ) <>0   ";
                StrSql = StrSql + " And   LevelCnt =" + Cnt;                
                StrSql = StrSql + " Group By Saveid,Saveid2   ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);                              
                pg1.PerformStep(); pg1.Refresh();

                Cnt = Cnt - 1; 

            }

            //최대 그룹 실적을 구한다.
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " High_PV =  ISNULL(B.A2,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + "Select    Max(G_Cur_PV +  Day_Sum_PV  ) A2 ";
            StrSql = StrSql + " ,Saveid,Saveid2 ";
            StrSql = StrSql + " From tbl_ClosePay_04 ";
            StrSql = StrSql + " Where  G_Cur_PV + Day_Sum_PV > 0   ";            
            StrSql = StrSql + " Group By Saveid,Saveid2   ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Saveid ";
            StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //최대 실적을 뺀 Non_High_PV 그룹 실적을 구한다.
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Non_High_PV =  G_Cur_PV - High_PV ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Pa_Down_Cnt =  ISNULL(B.A2,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + "Select   Count(Mbid) A2 ";
            StrSql = StrSql + " ,Nominid,Nominid2 ";
            StrSql = StrSql + " From tbl_ClosePay_04 ";

            if (int.Parse (FromEndDate )  >= 20150601 )
                StrSql = StrSql + " Where  Day_Sum_PV >= 40   ";
            else
                StrSql = StrSql + " Where  Day_Sum_PV >= 100   ";

            StrSql = StrSql + " Group By Nominid,Nominid2   ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Day_Sum_PV_Nom =  ISNULL(B.A2,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + "Select    Sum(Day_Sum_PV) A2 ";
            StrSql = StrSql + " ,Nominid,Nominid2 ";
            StrSql = StrSql + " From tbl_ClosePay_04 ";
            StrSql = StrSql + " Where  Day_Sum_PV > 0   ";
            StrSql = StrSql + " Group By Nominid,Nominid2   ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            
        }



        private void Put_Down_SumPV_Day(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string B_Date )
        {
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Day_Sum_PV = 0 , High_PV =  0 , G_Cur_PV = 0 , Non_High_PV = 0 , Pa_Down_Cnt = 0 , Day_Sum_PV_30 = 0, Day_Sum_PV_Nom = 0  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //이번달의 반품과 실매출을 합산하거를 뽑아온다.....
            StrSql = " Update tbl_ClosePay_04 SET";
            StrSql = StrSql + " Day_Sum_PV_30 = IsNull(b.A1, 0) + IsNull(b.A2, 0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select  Sum(BS1.TotalPV) AS A1 ,  IsNull(Sum(Bs_R.TotalPV), 0) A2 ";
            StrSql = StrSql + " ,BS1.Mbid, BS1.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail (nolock) BS1 ";
            StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " Left join  tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid2 = BS1.Mbid2 ";
            StrSql = StrSql + " Where   BS1.SellDate >= tbl_Memberinfo.RegTime ";
            StrSql = StrSql + " And     BS1.SellDate <= '" + B_Date + "'";
            StrSql = StrSql + " And     BS1.TotalPV  + BS1.TotalCV >= 0 ";
            StrSql = StrSql + " And     BS1.SellCode <> '' ";
            StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
            StrSql = StrSql + " Group By BS1.Mbid,BS1.Mbid2 ";
            StrSql = StrSql + " Having Sum(BS1.TotalPV) + IsNull(Sum(Bs_R.TotalPV), 0) >= 0";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = " Update tbl_ClosePay_04 SET Day_Sum_PV_30 = 0 ";
            StrSql = StrSql + " Where DateDiff(Day,Regtime,'" + B_Date + "') > 30 ";
            StrSql = StrSql + " And   Day_Sum_PV_30 > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);




            //이번달의 반품과 실매출을 합산하거를 뽑아온다.....
            StrSql = " Update tbl_ClosePay_04 SET";
            StrSql = StrSql + " Day_Sum_PV = IsNull(b.A1, 0) + IsNull(b.A2, 0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select  Sum(BS1.TotalPV) AS A1 ,  IsNull(Sum(Bs_R.TotalPV), 0) A2 ";
            StrSql = StrSql + " ,BS1.Mbid, BS1.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail (nolock) BS1 ";
            StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " Where   BS1.SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     BS1.SellDate <= '" + B_Date + "'";
            StrSql = StrSql + " And     BS1.TotalPV  + BS1.TotalCV >= 0 ";
            StrSql = StrSql + " And     BS1.SellCode <> '' ";
            StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
            StrSql = StrSql + " Group By BS1.Mbid,BS1.Mbid2 ";
            StrSql = StrSql + " Having Sum(BS1.TotalPV) + IsNull(Sum(Bs_R.TotalPV), 0) >= 0";

            //StrSql = StrSql + " Where   SellDate >= '" + FromEndDate + "'";
            //StrSql = StrSql + " And     SellDate <= '" + ToEndDate + "'";            
            //StrSql = StrSql + " And   Ga_Order = 0 ";
            //StrSql = StrSql + " Group By Mbid,Mbid2";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Mbid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            int Cnt = MaxLevel;

            while (Cnt >= 0)
            {

                StrSql = "Update tbl_ClosePay_04 SET ";
                StrSql = StrSql + " G_Cur_PV =  ISNULL(B.A2,0)";
                StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

                StrSql = StrSql + " (";
                StrSql = StrSql + "Select    Sum(G_Cur_PV +  Day_Sum_PV  ) A2 ";
                StrSql = StrSql + " ,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_04 ";
                StrSql = StrSql + " Where (  G_Cur_PV + Day_Sum_PV ) <>0   ";
                StrSql = StrSql + " And   LevelCnt =" + Cnt;
                StrSql = StrSql + " Group By Saveid,Saveid2   ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();

                Cnt = Cnt - 1;

            }

            //최대 그룹 실적을 구한다.
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " High_PV =  ISNULL(B.A2,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + "Select    Max(G_Cur_PV +  Day_Sum_PV  ) A2 ";
            StrSql = StrSql + " ,Saveid,Saveid2 ";
            StrSql = StrSql + " From tbl_ClosePay_04 ";
            StrSql = StrSql + " Where  G_Cur_PV + Day_Sum_PV > 0   ";
            StrSql = StrSql + " Group By Saveid,Saveid2   ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Saveid ";
            StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //최대 실적을 뺀 Non_High_PV 그룹 실적을 구한다.
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Non_High_PV =  G_Cur_PV - High_PV ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            if (int.Parse(ToEndDate) <= 20150630)
            {
                StrSql = " Update tbl_ClosePay_04 SET";
                StrSql = StrSql + " Day_Sum_PV = 100";
                StrSql = StrSql + " Where Mbid2 = 55666 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            else
            {
                StrSql = " Update tbl_ClosePay_04 SET";
                StrSql = StrSql + " Day_Sum_PV = 200";
                StrSql = StrSql + " Where Mbid2 = 55666 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }


            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Pa_Down_Cnt =  ISNULL(B.A2,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + "Select   Count(Mbid) A2 ";
            StrSql = StrSql + " ,Nominid,Nominid2 ";
            StrSql = StrSql + " From tbl_ClosePay_04 ";
            
            if (int.Parse(FromEndDate) >= 20150601)
                StrSql = StrSql + " Where  Day_Sum_PV >= 40   ";
            else
                StrSql = StrSql + " Where  Day_Sum_PV >= 100   ";


            StrSql = StrSql + " Group By Nominid,Nominid2   ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            if (int.Parse(ToEndDate) > 20150630)
            {
                StrSql = " Update tbl_ClosePay_04 SET";
                StrSql = StrSql + " Pa_Down_Cnt = 2";
                StrSql = StrSql + " Where Mbid2 = 55666 ";
                StrSql = StrSql + " And Pa_Down_Cnt < 2";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }


            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Day_Sum_PV_Nom =  ISNULL(B.A2,0)";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

            StrSql = StrSql + " (";
            StrSql = StrSql + "Select    Sum(Day_Sum_PV) A2 ";
            StrSql = StrSql + " ,Nominid,Nominid2 ";
            StrSql = StrSql + " From tbl_ClosePay_04 ";
            StrSql = StrSql + " Where  Day_Sum_PV > 0   ";
            StrSql = StrSql + " Group By Nominid,Nominid2   ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);








            //StrSql = "Update tbl_ClosePay_04 SET ";
            //StrSql = StrSql + " Day_Sum_PV_Nom =  ISNULL(B.A2,0)";
            //StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

            //StrSql = StrSql + " (";
            //StrSql = StrSql + "Select    Sum(Day_Sum_PV) A2 ";
            //StrSql = StrSql + " ,Nominid,Nominid2 ";
            //StrSql = StrSql + " From tbl_ClosePay_04 ";
            //StrSql = StrSql + " Where  Day_Sum_PV > 0   ";
            //StrSql = StrSql + " Group By Nominid,Nominid2   ";
            //StrSql = StrSql + " ) B";

            //StrSql = StrSql + " Where A.Mbid=B.Nominid ";
            //StrSql = StrSql + " And   A.Mbid2=B.Nominid2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
        }




        private void Put_Down_PV_01(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            


            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double Allowance2 = 0, Allowance1 = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select Se.TotalPV , Isnull( Bs_R.TotalPV, 0 ) AS RePV  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate <= '" + ToEndDate + "'";            

            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 "; 

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;
                     
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {



                LevelCnt = 0; TSaveid = "**";
                //TSaveid = ds.Tables[base_db_name].Rows[fi_cnt]["Nominid"].ToString();
                //TSaveid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Nominid2"].ToString());
                //TLine = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["N_LineCnt"].ToString());
                //Allowance2 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance2"].ToString());

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

                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "")
                        {

                            R_LevelCnt++;

                            StrSql = "Update tbl_ClosePay_04 SET ";
                            if (TLine == 1 )
                                StrSql = StrSql + " Cur_Down_PV_1 = Cur_Down_PV_1 +  " + TotalPV;
                            else
                                StrSql = StrSql + " Cur_Down_PV_2 = Cur_Down_PV_2 +  " + TotalPV;
                            StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                            StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                            t_qu[t_qu_Cnt] = StrSql;
                            t_qu_Cnt++;



                            StrSql = "INSERT INTO tbl_Close_DownPV_PV_04";
                            StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                            StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV , ";
                            StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

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


        private void Put_Down_PV_Be_M_01(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {



            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double Allowance2 = 0, Allowance1 = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select Se.TotalPV , Isnull( Bs_R.TotalPV, 0 ) AS RePV  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2 ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate <= '20180701'";

            StrSql = StrSql + " WHERE Se.TotalPV + Isnull( Bs_R.TotalPV, 0 ) > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='20180604'";
            StrSql = StrSql + " And   Se.SellDate  <='20180701'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";

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

                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "")
                        {

                            R_LevelCnt++;

                            StrSql = "Update tbl_ClosePay_04 SET ";
                            if (TLine == 1)
                                StrSql = StrSql + " Be_Down_PV_1 = Be_Down_PV_1 +  " + TotalPV;
                            else
                                StrSql = StrSql + " Be_Down_PV_1 = Be_Down_PV_1 +  " + TotalPV;
                            StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                            StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                            t_qu[t_qu_Cnt] = StrSql;
                            t_qu_Cnt++;



                            StrSql = "INSERT INTO tbl_Close_DownPV_PV_04";
                            StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                            StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , Sell_DownPV , ";
                            StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                            StrSql = StrSql + "Values(";
                            StrSql = StrSql + "'20180701','" + Mbid + "'";
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

        private void Put_Down_PV_02(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 7;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + "   Sum_PV_1 = Be_PV_1 + Cur_PV_1  ";
            StrSql = StrSql + "  ,Sum_PV_2 = Be_PV_2 + Cur_PV_2  ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //'''--재구매는 본인 소실적으로 잡아준다.
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + "   Sum_PV_1 =  Sum_PV_1 + DayPV02  ";
            StrSql = StrSql + " Where Sum_PV_1 <  Sum_PV_2"; 

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql + " Sum_PV_2 =  Sum_PV_2 + DayPV02  ";
            StrSql = StrSql + " Where Sum_PV_1 >=  Sum_PV_2";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //'''3개월간 소실적이고 뭐고 없다. 그럼 - 시킨다
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Fresh_1 =  Sum_PV_1 ";
            StrSql = StrSql + ",Sum_PV_1 =  0 ";
            StrSql = StrSql + " Where Self_M3_PV + Cur_PV_M3_1 + Cur_PV_M3_2 +  Cur_PV_1+ Cur_PV_2 <= 0";
            StrSql = StrSql + " And Sum_PV_1 > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_04 SET ";
            //StrSql = StrSql + " Fresh_1 =  Sum_PV_1 ";
            //StrSql = StrSql + ",Sum_PV_1 =  0 ";
            //StrSql = StrSql + " Where Self_M3_PV + Cur_PV_M3_1 +Cur_PV_M3_2 <= 0";
            //StrSql = StrSql + " And Sum_PV_1 > 0 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Fresh_2 =  Sum_PV_2 ";
            StrSql = StrSql + ",Sum_PV_2 =  0 ";
            StrSql = StrSql + " Where Self_M3_PV + Cur_PV_M3_1 +Cur_PV_M3_2  +  Cur_PV_1+ Cur_PV_2 <= 0";
            StrSql = StrSql + " And Sum_PV_2 > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


        }


        private void CurGrade_OrgGrade_Put(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2 ;
            pg1.PerformStep(); pg1.Refresh();

            string StrSql = "Update tbl_ClosePay_04 set" ;
            StrSql = StrSql + " OrgGrade  = BeforeGrade"          ;
            StrSql = StrSql + " ,CurGrade = BeforeGrade";
            StrSql = StrSql + " Where  BeforeGrade > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }


        private void GiveShamGrade(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string StrSql = "Exec Usp_Sham_Grade '" + ToEndDate  + "'   ";
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
                    //    StrSql = "Update tbl_ClosePay_04 SET ";
                    //    StrSql = StrSql + " CurGrade =  0  ";
                    //    StrSql = StrSql + " ,OrgGrade =    " + S_Grade;
                    //    StrSql = StrSql + " ,ShamGrade =    " + S_Grade;
                    //    StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
                    //    StrSql = StrSql + " And   Mbid2 = " + Mbid2;
                    //    Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    //}
                    //else
                    //{
                        StrSql = "Update tbl_ClosePay_04 SET ";
                        StrSql = StrSql + " CurGrade =    " + S_Grade;
                        StrSql = StrSql + " ,OrgGrade =    " + S_Grade;
                        StrSql = StrSql + " ,ShamGrade =    " + S_Grade;
                        StrSql = StrSql + " ,ReqTF1 = 1   ";
                        StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
                        StrSql = StrSql + " And   Mbid2 = " + Mbid2;
                        Temp_Connect.Insert_Data(StrSql, Conn, tran);
                    //}
                    Cnt =10 ;

                    while (Cnt <= S_Grade)
                    {
                        TFild = "GradeDate" + (Cnt / 10).ToString();

                        StrSql = "Update tbl_ClosePay_04 Set ";
                        StrSql = StrSql + TFild + " = '" + TMaxDate + "'";
                        StrSql = StrSql + " Where Mbid='" + Mbid + "'";
                        StrSql = StrSql + " And Mbid2=" + Mbid2;
                        StrSql = StrSql + " And " + TFild + " = ''" ;

                        Temp_Connect.Insert_Data(StrSql, Conn, tran);

                        Cnt = Cnt + 10; 
                    }

                    pg1.PerformStep(); pg1.Refresh();
                } // for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)

            } // (ReCnt != 0)
        }


        private void GradeUpLine2(int CurrentGrade  , cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            int Cnt = 0;
            string StrSql ="", str_GradeCnt = "", str_GradeCnt1 = "", str_GradeCnt2 = "", str_GradeCnt3 = "";

            if ( CurrentGrade == 10 )
            {   
                str_GradeCnt = " GradeCnt1_1 + GradeCnt1_2 " ;
                str_GradeCnt1 = " GradeCnt1_1 ";  str_GradeCnt2 = " GradeCnt1_2 ";

                StrSql = "Update tbl_ClosePay_04 SET GradeCnt1_1 =0,GradeCnt1_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 20)
            {
                str_GradeCnt = " GradeCnt2_1 + GradeCnt2_2 ";
                str_GradeCnt1 = " GradeCnt2_1 "; str_GradeCnt2 = " GradeCnt2_2 ";

                StrSql = "Update tbl_ClosePay_04 SET GradeCnt2_1 =0 ,  GradeCnt2_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 30)
            {
                str_GradeCnt = " GradeCnt3_1 + GradeCnt3_2 ";
                str_GradeCnt1 = " GradeCnt3_1 "; str_GradeCnt2 = " GradeCnt3_2 ";

                StrSql = "Update tbl_ClosePay_04 SET GradeCnt3_1 =0 ,  GradeCnt3_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 40)
            {
                str_GradeCnt = " GradeCnt4_1 + GradeCnt4_2 ";
                str_GradeCnt1 = " GradeCnt4_1 "; str_GradeCnt2 = " GradeCnt4_2 ";

                StrSql = "Update tbl_ClosePay_04 SET GradeCnt4_1 =0 ,  GradeCnt4_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 50)
            {
                str_GradeCnt = " GradeCnt5_1 + GradeCnt5_2 ";
                str_GradeCnt1 = " GradeCnt5_1 "; str_GradeCnt2 = " GradeCnt5_2 ";

                StrSql = "Update tbl_ClosePay_04 SET GradeCnt5_1 =0 ,  GradeCnt5_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 60)
            {
                str_GradeCnt = " GradeCnt6_1 + GradeCnt6_2 ";
                str_GradeCnt1 = " GradeCnt6_1 "; str_GradeCnt2 = " GradeCnt6_2 ";

                StrSql = "Update tbl_ClosePay_04 SET GradeCnt6_1 =0 ,  GradeCnt6_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 70)
            {
                str_GradeCnt = " GradeCnt7_1 + GradeCnt7_2 ";
                str_GradeCnt1 = " GradeCnt7_1 "; str_GradeCnt2 = " GradeCnt7_2 ";

                StrSql = "Update tbl_ClosePay_04 SET GradeCnt7_1 =0 ,  GradeCnt7_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 80)
            {
                str_GradeCnt = " GradeCnt8_1 + GradeCnt8_2 ";
                str_GradeCnt1 = " GradeCnt8_1 "; str_GradeCnt2 = " GradeCnt8_2 ";

                StrSql = "Update tbl_ClosePay_04 SET GradeCnt8_1 =0 ,  GradeCnt8_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            if (CurrentGrade == 90)
            {
                str_GradeCnt = " GradeCnt9_1 + GradeCnt9_2 ";
                str_GradeCnt1 = " GradeCnt9_1 "; str_GradeCnt2 = " GradeCnt9_2 ";

                StrSql = "Update tbl_ClosePay_04 SET GradeCnt9_1 =0 ,  GradeCnt9_2 =0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            pg1.Value = 0; pg1.Maximum = Cnt + 4 ;
            pg1.PerformStep(); pg1.Refresh();

            Cnt = MaxLevel;

             while (Cnt >= 1)
             {
                StrSql = "Update tbl_ClosePay_04 SET " ;
                StrSql = StrSql + str_GradeCnt1 + "=ISNULL(B.A1,0) ";
                StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

                StrSql = StrSql + " (Select    Sum(" + str_GradeCnt + ") A1,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_04 ";
                StrSql = StrSql + " Where " + str_GradeCnt + " > 0  ";
                StrSql = StrSql + " And LineCnt = 1 ";
                StrSql = StrSql + " And LevelCnt =" + Cnt;
                StrSql = StrSql + " Group By Saveid,Saveid2  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                 pg1.PerformStep(); pg1.Refresh();

                StrSql = "Update tbl_ClosePay_04 SET " ;
                StrSql = StrSql + str_GradeCnt1 + " =" + str_GradeCnt1 + " + ISNULL(B.A1,0)  ";
                StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

                StrSql = StrSql + " (Select Count(Mbid) A1,   Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_04 ";
                if ( CurrentGrade == 40 )
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



                StrSql = "Update tbl_ClosePay_04 SET ";
                StrSql = StrSql + str_GradeCnt2 + "=ISNULL(B.A1,0) ";
                StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

                StrSql = StrSql + " (Select    Sum(" + str_GradeCnt + ") A1,Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_04 ";
                StrSql = StrSql + " Where " + str_GradeCnt + "> 0  ";
                StrSql = StrSql + " And LineCnt >= 2 ";
                StrSql = StrSql + " And LevelCnt =" + Cnt;
                StrSql = StrSql + " Group By Saveid,Saveid2  ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Saveid ";
                StrSql = StrSql + " And   A.Mbid2=B.Saveid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                 pg1.PerformStep(); pg1.Refresh();


                StrSql = "Update tbl_ClosePay_04 SET ";
                StrSql = StrSql + str_GradeCnt2 + " =" + str_GradeCnt2 + " + + ISNULL(B.A1,0)  ";
                StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

                StrSql = StrSql + " (Select Count(Mbid) A1,   Saveid,Saveid2 ";
                StrSql = StrSql + " From tbl_ClosePay_04 ";

                if ( CurrentGrade == 40 )
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



        private void GiveGrade1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 3;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            
            StrSql = "Update tbl_ClosePay_04 Set " ;
            StrSql = StrSql + " OneGrade = 10 ";
            StrSql = StrSql + " Where   OneGrade < 10 " ;
            StrSql = StrSql + " And   Sell_MEM_TF = 0 " ;
            StrSql = StrSql + " And   (Day_Sum_PV  >= 100  Or Day_Sum_PV_Nom >= 100 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 100 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 1 ";       

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
                       
            StrSql = "Update tbl_ClosePay_04 Set " ;
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'" ;
            StrSql = StrSql + " Where CurGrade =10 " ;
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
            StrSql = "Update tbl_ClosePay_04 Set " ;
            StrSql = StrSql + " OneGrade = 20 ";            
            StrSql = StrSql + " Where   OneGrade < 20 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 100  Or Day_Sum_PV_Nom >= 100 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 375 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 1 ";       


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
                       

            StrSql = "Update tbl_ClosePay_04 Set " ;
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'" ;
            StrSql = StrSql + " Where CurGrade = 20 " ;
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }


        private void GiveGrade3(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 9;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            //per
            StrSql = "Update tbl_ClosePay_04 Set " ;
            StrSql = StrSql + " OneGrade = 30 ";
            StrSql = StrSql + " Where   OneGrade < 30 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 100  Or Day_Sum_PV_Nom >= 100 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 750 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 1 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 30 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 30";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 30 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }


        private void GiveGrade4(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 10;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 40 ";
            StrSql = StrSql + " Where   OneGrade < 40 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 100  Or Day_Sum_PV_Nom >= 100 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 1500 ";
            StrSql = StrSql + " And   non_High_PV   >= 500 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 2 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 40 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 40 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 40";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 40 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void GiveGrade5(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 12 ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            //StrSql = "Update tbl_ClosePay_04 Set ";
            //StrSql = StrSql + " Cur_PV_3000 = (DayPv01 + DayPV02) ";
            //StrSql = StrSql + " Where   (SellPV01 + SellPV02) - (DayPv01 + DayPV02)  >=  3000 ";  //오늘누적치 빼도 3천이 넘는다 그럼 오늘꺼는 다 잡아준다.
            //StrSql = StrSql + " And     (DayPv01 + DayPV02) > 0 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //StrSql = "Update tbl_ClosePay_04 Set ";
            //StrSql = StrSql + " Cur_PV_3000 = (SellPV01 + SellPV02) - 3000 ";
            //StrSql = StrSql + " Where   (SellPV01 + SellPV02)  >  3000 ";  //전체가 3천이 넘네
            //StrSql = StrSql + " And     (SellPV01 + SellPV02) - (DayPv01 + DayPV02)  <  3000 ";  //오늘꺼 뺀 이전에는 3천이 안넘엇네... 그럼 3천 이상부터는 나한태.
            //StrSql = StrSql + " And     (DayPv01 + DayPV02) > 0 " ;

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //StrSql = "Update tbl_ClosePay_04 Set ";
            //StrSql = StrSql + " G_Sum_PV_1 = G_Sum_PV_1 + Cur_PV_3000 ";
            //StrSql = StrSql + " ,ReqTF4 = 1 ";
            //StrSql = StrSql + " Where   Cur_PV_3000 > 0 ";
            //StrSql = StrSql + " And     G_Sum_PV_1 < G_Sum_PV_2 ";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //StrSql = "Update tbl_ClosePay_04 Set ";
            //StrSql = StrSql + " G_Sum_PV_2 = G_Sum_PV_2 + Cur_PV_3000 ";
            //StrSql = StrSql + " ,ReqTF4 = 2 ";
            //StrSql = StrSql + " Where   Cur_PV_3000 > 0 ";
            //StrSql = StrSql + " And     G_Sum_PV_1 >= G_Sum_PV_2 ";
            //StrSql = StrSql + " And     ReqTF4 = 0 ";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////----------------------------------------------------------------------------------------------------------------------------------






            StrSql = "Update tbl_ClosePay_04 Set " ;
            StrSql = StrSql + " OneGrade = 50 ";
            StrSql = StrSql + " Where   OneGrade < 50 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 100  Or Day_Sum_PV_Nom >= 100 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 4500 ";
            StrSql = StrSql + " And   non_High_PV   >= 1500 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 2 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void GiveGrade5_Day(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string Base_ToEndDate)
        {
            pg1.Value = 0; pg1.Maximum = 12;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

        

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 50 ";
            StrSql = StrSql + " Where   OneGrade < 50 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 100 OR Day_Sum_PV_30  >= 100  )  ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV + Day_Sum_PV_30  >= 4500 ";
            //StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV   >= 4500 ";   ''2015-8-17 유차장 요청에 의해서 본인 하선에도 30 이내 매출을 포함함.
            StrSql = StrSql + " And   non_High_PV   >= 1500 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 2 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            if (Base_ToEndDate == "20150530")
            {
                StrSql = "Update tbl_ClosePay_04 Set ";
                StrSql = StrSql + " OneGrade = 50 ";
                StrSql = StrSql + " Where   Mbid2 = 8271137 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate5 ='" + Base_ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate4 ='" + Base_ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + Base_ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + Base_ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + Base_ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 50 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }


        private void GiveGrade6(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 10  ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 60 ";
            StrSql = StrSql + " Where   OneGrade < 60 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 100  Or Day_Sum_PV_Nom >= 100 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 10000 ";
            StrSql = StrSql + " And   non_High_PV   >= 3000 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 2 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 60 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void GiveGrade7(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 11 ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 70 ";
            StrSql = StrSql + " Where   OneGrade < 70 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 200  Or Day_Sum_PV_Nom >= 200 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 20000 ";
            StrSql = StrSql + " And   non_High_PV   >= 7000 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 2 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 70 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }

        private void GiveGrade8(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 12  ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 80 ";
            StrSql = StrSql + " Where   OneGrade < 80 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 200  Or Day_Sum_PV_Nom >= 200 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 40000 ";
            StrSql = StrSql + " And   non_High_PV   >= 15000 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 2 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate8 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 80 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void GiveGrade9_Day(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string BaseToEndDAte)
        {
            pg1.Value = 0; pg1.Maximum = 13;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 90 ";
            StrSql = StrSql + " Where   OneGrade < 90 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 200  Or Day_Sum_PV_Nom >= 200 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 80000 ";
            StrSql = StrSql + " And   non_High_PV   >= 30000 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 2 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate9 ='" + BaseToEndDAte + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate9 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate8 ='" + BaseToEndDAte + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate8 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate7 ='" + BaseToEndDAte + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate6 ='" + BaseToEndDAte + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate5 ='" + BaseToEndDAte + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate4 ='" + BaseToEndDAte + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + BaseToEndDAte + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + BaseToEndDAte + "'";
            StrSql = StrSql + " Where CurGrade = 90";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + BaseToEndDAte + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void GiveGrade9(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 13   ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 90 ";
            StrSql = StrSql + " Where   OneGrade < 90 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 200  Or Day_Sum_PV_Nom >= 200 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 80000 ";
            StrSql = StrSql + " And   non_High_PV   >= 30000 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 2 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate9 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate8 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 90 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void GiveGrade10(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 13;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 100 ";
            StrSql = StrSql + " Where   OneGrade < 100 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 200  Or Day_Sum_PV_Nom >= 200 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 150000 ";
            StrSql = StrSql + " And   non_High_PV   >= 65000 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 2 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate10 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate10 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate9 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate8 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 100 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void GiveGrade11(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 13;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 110 ";
            StrSql = StrSql + " Where   OneGrade < 110 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 200  Or Day_Sum_PV_Nom >= 200 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 300000 ";
            StrSql = StrSql + " And   non_High_PV   >= 150000 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 2 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate11 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate11 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate10 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate10 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate9 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate8 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 110 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }




        private void GiveGrade12(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 13;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 120 ";
            StrSql = StrSql + " Where   OneGrade < 120 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 200  Or Day_Sum_PV_Nom >= 200 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 600000 ";
            StrSql = StrSql + " And   non_High_PV   >= 350000 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 2 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate12 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate12 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate11 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate11 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate10 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate10 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate9 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate8 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 120 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }





        private void GiveGrade13(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 13;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 130 ";
            StrSql = StrSql + " Where   OneGrade < 130 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   (Day_Sum_PV  >= 200  Or Day_Sum_PV_Nom >= 200 ) ";
            StrSql = StrSql + " And   G_Cur_PV +  Day_Sum_PV  >= 1000000 ";
            StrSql = StrSql + " And   non_High_PV   >= 600000 ";
            StrSql = StrSql + " And   Pa_Down_Cnt >= 2 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate13 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 130 ";
            StrSql = StrSql + " And GradeDate13 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate12 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 130 ";
            StrSql = StrSql + " And GradeDate12 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate11 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 130 ";
            StrSql = StrSql + " And GradeDate11 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate10 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 130 ";
            StrSql = StrSql + " And GradeDate10 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 130 ";
            StrSql = StrSql + " And GradeDate9 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 130 ";
            StrSql = StrSql + " And GradeDate8 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 130 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 130 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 130 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 130 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 130 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 130";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 130 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }




        private void GiveGrade14(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 15;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_04 Set";
            StrSql = StrSql + " ing_Cnt_13 = Be_ing_Cnt_13 + 1 ";
            StrSql = StrSql + " Where OneGrade = 130 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 140 ";
            StrSql = StrSql + " Where   OneGrade < 140 ";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";
            StrSql = StrSql + " And   ing_Cnt_13  >= 3 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //Pa_Down_Cnt    Day_Sum_PV      High_PV    non_High_PV    Pa_Down_Cnt  G_Cur_PV
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " CurGrade = OneGrade ";
            StrSql = StrSql + " Where   OneGrade > CurGrade ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate13 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140 ";
            StrSql = StrSql + " And GradeDate13 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();





            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate13 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140 ";
            StrSql = StrSql + " And GradeDate13 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate12 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140 ";
            StrSql = StrSql + " And GradeDate12 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate11 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140 ";
            StrSql = StrSql + " And GradeDate11 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate10 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140 ";
            StrSql = StrSql + " And GradeDate10 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate9 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140 ";
            StrSql = StrSql + " And GradeDate9 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate8 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140 ";
            StrSql = StrSql + " And GradeDate8 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate7 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140 ";
            StrSql = StrSql + " And GradeDate7 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate6 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140 ";
            StrSql = StrSql + " And GradeDate6 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate5 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140 ";
            StrSql = StrSql + " And GradeDate5 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate4 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140 ";
            StrSql = StrSql + " And GradeDate4 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate3 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140 ";
            StrSql = StrSql + " And GradeDate3 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate2 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140";
            StrSql = StrSql + " And GradeDate2 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " GradeDate1 ='" + ToEndDate + "'";
            StrSql = StrSql + " Where CurGrade = 140 ";
            StrSql = StrSql + " And GradeDate1 =''";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }



        private void Put_Grade_P(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 16;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_04 Set";
            StrSql = StrSql + " Grade_P = 5 ";
            StrSql = StrSql + " Where OneGrade = 50 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set";
            StrSql = StrSql + " Grade_P = 10 ";
            StrSql = StrSql + " Where OneGrade = 60 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set";
            StrSql = StrSql + " Grade_P = 15 ";
            StrSql = StrSql + " Where OneGrade = 70 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set";
            StrSql = StrSql + " Grade_P = 20 ";
            StrSql = StrSql + " Where OneGrade = 80 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set";
            StrSql = StrSql + " Grade_P = 25 ";
            StrSql = StrSql + " Where OneGrade = 90 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set";
            StrSql = StrSql + " Grade_P = 30 ";
            StrSql = StrSql + " Where OneGrade = 100 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set";
            StrSql = StrSql + " Grade_P = 35 ";
            StrSql = StrSql + " Where OneGrade = 110 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set";
            StrSql = StrSql + " Grade_P = 40 ";
            StrSql = StrSql + " Where OneGrade = 120 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set";
            StrSql = StrSql + " Grade_P = 45 ";
            StrSql = StrSql + " Where OneGrade >= 130 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();





            //이번달의 반품과 실매출을 합산하거를 뽑아온다.....
            StrSql = " Update tbl_ClosePay_04 SET";
            StrSql = StrSql + " Grade_N_P = 1";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select  Sum(BS1.TotalPV) AS A1 ,  IsNull(Sum(Bs_R.TotalPV), 0) A2 ";
            StrSql = StrSql + " , Ce4.Nominid, Ce4.Nominid2 ";
            StrSql = StrSql + " From tbl_SalesDetail (nolock) BS1 ";
            StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " Left join  tbl_ClosePay_04  AS Ce4 (nolock) ON Ce4.Mbid2  = BS1.Mbid2 ";  
            StrSql = StrSql + " Where   BS1.SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     BS1.SellDate <= '" + ToEndDate  + "'";
            StrSql = StrSql + " And     BS1.TotalPV  + BS1.TotalCV >= 375 ";
            StrSql = StrSql + " And     BS1.SellCode <> '' ";
            StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
            StrSql = StrSql + " Group By Ce4.Nominid, Ce4.Nominid2";
            StrSql = StrSql + " Having Sum(BS1.TotalPV) + IsNull(Sum(Bs_R.TotalPV), 0) >= 0";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Nominid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Nominid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_04 SET";
            StrSql = StrSql + " Grade_N_P = Grade_N_P + 3";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select  Sum(BS1.TotalPV) AS A1 ,  IsNull(Sum(Bs_R.TotalPV), 0) A2 ";
            StrSql = StrSql + " , Ce4.Nominid, Ce4.Nominid2 ";
            StrSql = StrSql + " From tbl_SalesDetail (nolock) BS1 ";
            StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " Left join  tbl_ClosePay_04  AS Ce4 (nolock) ON Ce4.Mbid2  = BS1.Mbid2 ";
            StrSql = StrSql + " Where   BS1.SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     BS1.SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     BS1.TotalPV  + BS1.TotalCV >= 750 ";
            StrSql = StrSql + " And     BS1.SellCode <> '' ";
            StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
            StrSql = StrSql + " Group By Ce4.Nominid, Ce4.Nominid2";
            StrSql = StrSql + " Having Sum(BS1.TotalPV) + IsNull(Sum(Bs_R.TotalPV), 0) >= 0";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Nominid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Nominid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = " Update tbl_ClosePay_04 SET";
            StrSql = StrSql + " Grade_N_P = Grade_N_P + 6";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select  Sum(BS1.TotalPV) AS A1 ,  IsNull(Sum(Bs_R.TotalPV), 0) A2 ";
            StrSql = StrSql + " , Ce4.Nominid, Ce4.Nominid2 ";
            StrSql = StrSql + " From tbl_SalesDetail (nolock) BS1 ";
            StrSql = StrSql + " Left join  tbl_SalesDetail  AS Bs_R  (nolock) ON Bs_R.Re_BaseOrderNumber  = BS1.OrderNumber  And     Bs_R.TotalPV  + Bs_R.TotalCV < 0    And  Bs_R.SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " Left join  tbl_ClosePay_04  AS Ce4 (nolock) ON Ce4.Mbid2  = BS1.Mbid2 ";
            StrSql = StrSql + " Where   BS1.SellDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And     BS1.SellDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " And     BS1.TotalPV  + BS1.TotalCV >= 1500 ";
            StrSql = StrSql + " And     BS1.SellCode <> '' ";
            StrSql = StrSql + " And     BS1.Ga_Order = 0 ";
            StrSql = StrSql + " Group By Ce4.Nominid, Ce4.Nominid2";
            StrSql = StrSql + " Having Sum(BS1.TotalPV) + IsNull(Sum(Bs_R.TotalPV), 0) >= 0";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where a.Mbid = b.Nominid ";
            StrSql = StrSql + " And   a.Mbid2 = b.Nominid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


        }





        private void Put_tbl_ClosePay_04_G(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 10;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Insert into  tbl_ClosePay_04_G (M_ToEndDate, Mbid,Mbid2, A_Grade, Max_Cnt, Total_Pay)  ";
            StrSql = StrSql + " Select '" + ToEndDate + "', Mbid,Mbid2, OneGrade, 1, 500000  ";
            StrSql = StrSql + " From  tbl_ClosePay_04 ";
            StrSql = StrSql + " Where OneGrade = 70 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into  tbl_ClosePay_04_G (M_ToEndDate, Mbid,Mbid2, A_Grade, Max_Cnt, Total_Pay)  ";
            StrSql = StrSql + " Select '" + ToEndDate + "', Mbid,Mbid2, OneGrade, 1, 1000000  ";
            StrSql = StrSql + " From  tbl_ClosePay_04 ";
            StrSql = StrSql + " Where OneGrade = 80 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into  tbl_ClosePay_04_G (M_ToEndDate, Mbid,Mbid2, A_Grade, Max_Cnt, Total_Pay)  ";
            StrSql = StrSql + " Select '" + ToEndDate + "', Mbid,Mbid2, OneGrade, 2, 2000000  ";
            StrSql = StrSql + " From  tbl_ClosePay_04 ";
            StrSql = StrSql + " Where OneGrade = 90 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into  tbl_ClosePay_04_G (M_ToEndDate, Mbid,Mbid2, A_Grade, Max_Cnt, Total_Pay)  ";
            StrSql = StrSql + " Select '" + ToEndDate + "', Mbid,Mbid2, OneGrade, 3, 3000000  ";
            StrSql = StrSql + " From  tbl_ClosePay_04 ";
            StrSql = StrSql + " Where OneGrade = 100 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into  tbl_ClosePay_04_G (M_ToEndDate, Mbid,Mbid2, A_Grade, Max_Cnt, Total_Pay)  ";
            StrSql = StrSql + " Select '" + ToEndDate + "', Mbid,Mbid2, OneGrade, 5, 5000000  ";
            StrSql = StrSql + " From  tbl_ClosePay_04 ";
            StrSql = StrSql + " Where OneGrade = 110 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into  tbl_ClosePay_04_G (M_ToEndDate, Mbid,Mbid2, A_Grade, Max_Cnt, Total_Pay)  ";
            StrSql = StrSql + " Select '" + ToEndDate + "', Mbid,Mbid2, OneGrade, 10, 10000000  ";
            StrSql = StrSql + " From  tbl_ClosePay_04 ";
            StrSql = StrSql + " Where OneGrade = 120 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Insert into  tbl_ClosePay_04_G (M_ToEndDate, Mbid,Mbid2, A_Grade, Max_Cnt, Total_Pay)  ";
            StrSql = StrSql + " Select '" + ToEndDate + "', Mbid,Mbid2, OneGrade, 12, 50000000  ";
            StrSql = StrSql + " From  tbl_ClosePay_04 ";
            StrSql = StrSql + " Where OneGrade = 130 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into  tbl_ClosePay_04_G (M_ToEndDate, Mbid,Mbid2, A_Grade, Max_Cnt, Total_Pay)  ";
            StrSql = StrSql + " Select '" + ToEndDate + "', Mbid,Mbid2, OneGrade, 24, 100000000  ";
            StrSql = StrSql + " From  tbl_ClosePay_04 ";
            StrSql = StrSql + " Where OneGrade = 140 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



        }




        private void Put_ReqTF2_OneGrade(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
             pg1.Value = 0; pg1.Maximum = 5;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

          
            StrSql = "Update tbl_ClosePay_04 SET  " ;
            StrSql = StrSql + " ReqTF2 = 1 ";
            StrSql = StrSql + " Where  CurGrade = 40 ";
            StrSql = StrSql + " And Cur_Down_PV_1 >= " + double.Parse(txtB1.Text);
            StrSql = StrSql + " And Cur_Down_PV_2 >= " + double.Parse(txtB1.Text);
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET  ";
            StrSql = StrSql + " ReqTF2 = 1 ";
            StrSql = StrSql + " Where  CurGrade = 50 ";
            StrSql = StrSql + " And Cur_Down_PV_1 >= " + double.Parse(txtB2.Text);
            StrSql = StrSql + " And Cur_Down_PV_2 >= " + double.Parse(txtB2.Text);
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET  ";
            StrSql = StrSql + " ReqTF2 = 1 ";
            StrSql = StrSql + " Where  CurGrade = 60 ";
            StrSql = StrSql + " And Cur_Down_PV_1 >= " + double.Parse(txtB3.Text);
            StrSql = StrSql + " And Cur_Down_PV_2 >= " + double.Parse(txtB3.Text);
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }




         private void Put_ReqTF2_OneGrade_R(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
             pg1.Value = 0; pg1.Maximum = 19;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = CurGrade ";
            StrSql = StrSql + " Where CurGrade <= 10 ";
            ////StrSql = StrSql + " And   G_Cur_PV_1 >= 300000";
            ////StrSql = StrSql + " And   G_Cur_PV_2 >= 300000";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 20 ";
            StrSql = StrSql + " Where CurGrade >= 20 ";
            StrSql = StrSql + " And   G_Cur_PV_1 >= 300";
            StrSql = StrSql + " And   G_Cur_PV_2 >= 300";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 30 ";
            StrSql = StrSql + " Where CurGrade >= 30 ";
            StrSql = StrSql + " And   G_Cur_PV_1 >= 600";
            StrSql = StrSql + " And   G_Cur_PV_2 >= 600";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 40 ";
            StrSql = StrSql + " Where CurGrade >= 40 ";
            StrSql = StrSql + " And   G_Cur_PV_1 >= 1500";
            StrSql = StrSql + " And   G_Cur_PV_2 >= 1500";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 50 ";
            StrSql = StrSql + " Where CurGrade >= 50 ";
            StrSql = StrSql + " And   G_Cur_PV_1 >= 9000";
            StrSql = StrSql + " And   G_Cur_PV_2 >= 9000";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 60 ";
            StrSql = StrSql + " Where CurGrade >= 60 ";
            StrSql = StrSql + " And   G_Cur_PV_1 >= 15000";
            StrSql = StrSql + " And   G_Cur_PV_2 >= 15000";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 70 ";
            StrSql = StrSql + " Where CurGrade >= 70 ";
            StrSql = StrSql + " And   G_Cur_PV_1 >= 30000";
            StrSql = StrSql + " And   G_Cur_PV_2 >= 30000";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 80 ";
            StrSql = StrSql + " Where CurGrade >= 80 ";
            StrSql = StrSql + " And   G_Cur_PV_1 >= 90000";
            StrSql = StrSql + " And   G_Cur_PV_2 >= 90000";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " OneGrade = 90 ";
            StrSql = StrSql + " Where CurGrade >= 90 ";
            StrSql = StrSql + " And   G_Cur_PV_1 >= 150000";
            StrSql = StrSql + " And   G_Cur_PV_2 >= 150000";
            StrSql = StrSql + " And   LeaveDate = ''";
            StrSql = StrSql + " And   Sell_MEM_TF = 0 ";


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update tbl_ClosePay_04 SET  ";
            StrSql = StrSql + " OneGrade = 10 ";
            StrSql = StrSql + " Where  LEFT(GradeDate1,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 10 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 SET  ";
            StrSql = StrSql + " OneGrade = 20 ";
            StrSql = StrSql + " Where  LEFT(GradeDate2,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 20 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET  ";
            StrSql = StrSql + " OneGrade = 30 ";
            StrSql = StrSql + " Where  LEFT(GradeDate3,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 30 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 SET  ";
            StrSql = StrSql + " OneGrade = 40 ";
            StrSql = StrSql + " Where  LEFT(GradeDate4,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 40 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET  ";
            StrSql = StrSql + " OneGrade = 50 ";
            StrSql = StrSql + " Where  LEFT(GradeDate5,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 50 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET  ";
            StrSql = StrSql + " OneGrade = 60 ";
            StrSql = StrSql + " Where  LEFT(GradeDate6,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 60 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET  ";
            StrSql = StrSql + " OneGrade = 70 ";
            StrSql = StrSql + " Where  LEFT(GradeDate7,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 70 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET  ";
            StrSql = StrSql + " OneGrade = 80 ";
            StrSql = StrSql + " Where  LEFT(GradeDate8,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 80 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 SET  ";
            StrSql = StrSql + " OneGrade = 90 ";
            StrSql = StrSql + " Where  LEFT(GradeDate9,6) = '" + ToEndDate.Substring(0, 6) + "'";
            StrSql = StrSql + " And OneGrade < 90 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            
        }


        private void Give_Allowance2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

     
            


            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double Allowance2 = 0, Allowance1 = 0, TotalPV = 0;


            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + "  Allowance1_Sum_02 =ISNULL(B.A1,0) ";            
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

            StrSql = StrSql + " (Select  Sum(Allowance1) A1  , Mbid,Mbid2 ";
            StrSql = StrSql + " From tbl_ClosePay_02_Mod  (nolock) ";
            StrSql = StrSql + " Where FromEndDate >= '" + FromEndDate + "'";
            StrSql = StrSql + " And   FromEndDate <= '" + ToEndDate + "'";
            StrSql = StrSql + " Group by Mbid,Mbid2 "; 
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);



            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = "  Select   Allowance1_Sum_02  AS Allowance2  ,  Nominid , Nominid2 , Mbid , Mbid2 ";
            StrSql = StrSql + " ,N_LineCnt , M_Name, N_LineCnt , LineCnt   ";
            StrSql = StrSql + "  From tbl_ClosePay_04 (nolock)   ";
            StrSql = StrSql + " Where  Allowance1_Sum_02 > 0 ";
            StrSql = StrSql + " And OneGrade  >= 60 "; // 2015-11-02 일 이홍민 부사장님 요청에 의해서 첨가함.
            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;

                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                S_Mbid = Mbid + "-" + Mbid2.ToString();
                if (Clo_Mem.ContainsKey(S_Mbid) == true)
                {                    
                    TSaveid = Clo_Mem[S_Mbid].Nominid ;
                    TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;
                    
                }

                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance2"].ToString()) ;
                OrderNumber = "" ; //ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].OneGrade  >= 60 )
                        {
                            Allowance1 = 1;
                                                        
                            if (LevelCnt  >= 3 && Clo_Mem[S_Mbid].OneGrade <= 60) Allowance1 = 0;
                            if (LevelCnt  >= 5 && Clo_Mem[S_Mbid].OneGrade <= 70) Allowance1 = 0;                                                        
                            if (LevelCnt  >= 7 && Clo_Mem[S_Mbid].OneGrade <= 80) Allowance1 = 0;
                            if (LevelCnt  >= 8 && Clo_Mem[S_Mbid].OneGrade <= 90) Allowance1 = 0;
                            if (LevelCnt  >= 9 && Clo_Mem[S_Mbid].OneGrade <= 100) Allowance1 = 0;
                            if (LevelCnt  >= 10 && Clo_Mem[S_Mbid].OneGrade <= 110) Allowance1 = 0;

                            if (Allowance1 > 0)
                            {
                                R_LevelCnt++;

                                if (LevelCnt == 1) Allowance1 = TotalPV * 0.1;
                                if (LevelCnt == 2) Allowance1 = TotalPV * 0.1;
                                if (LevelCnt == 3) Allowance1 = TotalPV * 0.06;
                                if (LevelCnt == 4) Allowance1 = TotalPV * 0.06;
                                if (LevelCnt == 5) Allowance1 = TotalPV * 0.04;
                                if (LevelCnt == 6) Allowance1 = TotalPV * 0.04;
                                if (LevelCnt == 7) Allowance1 = TotalPV * 0.03;
                                if (LevelCnt == 8) Allowance1 = TotalPV * 0.03;
                                if (LevelCnt == 9) Allowance1 = TotalPV * 0.02;
                                if (LevelCnt == 10) Allowance1 = TotalPV * 0.02;
                                                                

                                if (Allowance1 > 0)
                                {
                                    //Allowance1 = Allowance1 * 1000;

                                    StrSql = "Update tbl_ClosePay_04 SET ";
                                    StrSql = StrSql + " Allowance8 = Allowance8 +  " + Allowance1;
                                    StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                    StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                    //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                    t_qu[t_qu_Cnt] = StrSql;
                                    t_qu_Cnt++;



                                    StrSql = "INSERT INTO tbl_Close_DownPV_ALL_04";
                                    StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                    StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                    StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                                    StrSql = StrSql + "Values(";
                                    StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                    StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                    StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                    StrSql = StrSql + Allowance1 + " ," + R_LevelCnt + "," + LevelCnt + " ," + TLine;
                                    StrSql = StrSql + ",'8' ,'" + OrderNumber + "')";

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

                    if (R_LevelCnt == 10) TSaveid = "**";

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


        private void Give_Allowance1_B1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 19;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
            
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance1 = Sum_PV_2 * 0.02 ";

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            StrSql = StrSql + "    Where Sum_PV_1 > 0 " ;
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance1 = Sum_PV_1 * 0.02 ";

            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            StrSql = StrSql + "    Where Sum_PV_1 > 0 " ;
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 < Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 =  0";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql +  "    Allowance1 =  Sum_PV_2 * 0.02 ";
            StrSql = StrSql +  "    ,Sum_PV_1 =  Sum_PV_1 -  Sum_PV_2";
            StrSql = StrSql +  "    ,Sum_PV_2 =  0 ";
            StrSql = StrSql +  "    ,Ded_1 =  Sum_PV_2 ";
            StrSql = StrSql +  "    ,Ded_2 =  Sum_PV_2 ";
            StrSql = StrSql +  "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql +  "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql +  "    And   Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql +  "    And   ReqTF2 = 1 ";
            StrSql = StrSql +  "    And   CurGrade = 10 ";
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql +  "    Allowance1 =  Sum_PV_1 * 0.02 ";
            StrSql = StrSql +  "    ,Sum_PV_1 =  0 ";
            StrSql = StrSql +  "    ,Sum_PV_2 =  Sum_PV_2 -  Sum_PV_1 ";
            StrSql = StrSql +  "    ,Ded_1 =  Sum_PV_1 ";
            StrSql = StrSql +  "    ,Ded_2 =  Sum_PV_1 ";
            StrSql = StrSql +  "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql +  "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql +  "    And   Sum_PV_1 < Sum_PV_2 ";
            StrSql = StrSql +  "    And   ReqTF2 = 1 ";
            StrSql = StrSql +  "    And   CurGrade = 10 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //''''---------------------------------------------------------------------------------------------------------------------
            //''''---------------------------------------------------------------------------------------------------------------------
            //''''---------------------------------------------------------------------------------------------------------------------



            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + "    Allowance1 =  Sum_PV_2 * 0.05 ";
            StrSql = StrSql + "    ,Sum_PV_1 =  Sum_PV_1 -  Sum_PV_2";
            StrSql = StrSql + "    ,Sum_PV_2 =  0 ";
            StrSql = StrSql + "    ,Ded_1 =  Sum_PV_2 ";
            StrSql = StrSql + "    ,Ded_2 =  Sum_PV_2 ";
            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   CurGrade = 20 ";
    
           Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + "    Allowance1 =  Sum_PV_1 * 0.05 ";
            StrSql = StrSql + "    ,Sum_PV_1 =  0 ";
            StrSql = StrSql + "    ,Sum_PV_2 =  Sum_PV_2 -  Sum_PV_1 ";
            StrSql = StrSql + "    ,Ded_1 =  Sum_PV_1 ";
            StrSql = StrSql + "    ,Ded_2 =  Sum_PV_1 ";
            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 < Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   CurGrade = 20 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //''''---------------------------------------------------------------------------------------------------------------------
            //''''---------------------------------------------------------------------------------------------------------------------
            //''''---------------------------------------------------------------------------------------------------------------------
    
    
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + "    Allowance1 =  Sum_PV_2 * 0.1 ";
            StrSql = StrSql + "    ,Sum_PV_1 =  Sum_PV_1 -  Sum_PV_2";
            StrSql = StrSql + "    ,Sum_PV_2 =  0 ";
            StrSql = StrSql + "    ,Ded_1 =  Sum_PV_2 ";
            StrSql = StrSql + "    ,Ded_2 =  Sum_PV_2 ";
            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   CurGrade >= 30 ";
            StrSql = StrSql + "    And   CurGrade <= 50 ";
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + "    Allowance1 =  Sum_PV_1 * 0.1 ";
            StrSql = StrSql + "    ,Sum_PV_1 =  0 ";
            StrSql = StrSql + "    ,Sum_PV_2 =  Sum_PV_2 -  Sum_PV_1 ";
            StrSql = StrSql + "    ,Ded_1 =  Sum_PV_1 ";
            StrSql = StrSql + "    ,Ded_2 =  Sum_PV_1 ";
            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 < Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   CurGrade >= 30 ";
            StrSql = StrSql + "    And   CurGrade <= 50 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //''''---------------------------------------------------------------------------------------------------------------------
            //''''---------------------------------------------------------------------------------------------------------------------
            //''''---------------------------------------------------------------------------------------------------------------------
    
    
    
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + "    Allowance1 =  Sum_PV_2 * 0.15 ";
            StrSql = StrSql + "    ,Sum_PV_1 =  Sum_PV_1 -  Sum_PV_2";
            StrSql = StrSql + "    ,Sum_PV_2 =  0 ";
            StrSql = StrSql + "    ,Ded_1 =  Sum_PV_2 ";
            StrSql = StrSql + "    ,Ded_2 =  Sum_PV_2 ";
            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 >= Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   CurGrade >= 60 ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + "    Allowance1 =  Sum_PV_1 * 0.15 ";
            StrSql = StrSql + "    ,Sum_PV_1 =  0 ";
            StrSql = StrSql + "    ,Sum_PV_2 =  Sum_PV_2 -  Sum_PV_1 ";
            StrSql = StrSql + "    ,Ded_1 =  Sum_PV_1 ";
            StrSql = StrSql + "    ,Ded_2 =  Sum_PV_1 ";
            StrSql = StrSql + "    Where Sum_PV_1 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_2 > 0 ";
            StrSql = StrSql + "    And   Sum_PV_1 < Sum_PV_2 ";
            StrSql = StrSql + "    And   ReqTF2 = 1 ";
            StrSql = StrSql + "    And   CurGrade >= 60 " ;
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            //''''---------------------------------------------------------------------------------------------------------------------
            //''''---------------------------------------------------------------------------------------------------------------------
            //''''---------------------------------------------------------------------------------------------------------------------


            StrSql = "Update tbl_ClosePay_04  SET Allowance1 = Allowance1 *  1000 Where Allowance1 > 0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = " Select Isnull(Sum(TotalPV),0)  ";
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";          
                        
            StrSql = StrSql + " WHERE   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";

            DataSet ds = new DataSet();           
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);

            Sum_T_PV_01 = double.Parse(ds.Tables[base_db_name].Rows[0][0].ToString());
            pg1.PerformStep(); pg1.Refresh();


            StrSql = " Select Isnull(Sum(Allowance1 ) , 0 )  AS DayPV From tbl_ClosePay_04 ";
            
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

                StrSql = "Update tbl_ClosePay_04 Set";
                StrSql = StrSql + "  Allowance1_Cut =  " + Cut_Pay + " * ((Allowance1) /  " + Sum_T_PV_001 + ")";
                StrSql = StrSql + " Where ( Allowance1  ) > 0 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);                
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
                                SellDate = sellinfo.SellDate,
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
                                StrSql = "Update tbl_ClosePay_04 SET ";
                                StrSql = StrSql + " Allowance1 = Allowance1 +  " + Allowance1  ;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'"  ;
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2  ;

                                Temp_Connect.Insert_Data(StrSql, Conn, tran);


                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_04";
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


        private void Make_Pay_Base(string W_ToEndDate, ref int W_OrgGrade, string Mbid, int Mbid2)
        {
            //주간 마감상에서. 날짜가 속한 마감을 알아온다. 전직급을 알아오기 위함.임.
            string StrSql = "Select OrgGrade   From  tbl_ClosePay_02_Mod (nolock) ";
            StrSql = StrSql + " Where ToEndDate ='" + W_ToEndDate + "'";
            StrSql = StrSql + " And  Mbid ='" + Mbid + "'";
            StrSql = StrSql + " And  Mbid2 =" + Mbid2;

            DataSet Dset3 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset3);
            int ReCnt3 = Search_Connect.DataSet_ReCount;
            if (ReCnt3 > 0)
            {
                W_OrgGrade = int.Parse(Dset3.Tables[base_db_name].Rows[0]["OrgGrade"].ToString());
            }

        }


        private void Make_Pay_Base(string SellDate, ref string W_ToEndDate)
        {
            //주간 마감상에서. 날짜가 속한 마감을 알아온다. 전직급을 알아오기 위함.임.
            string StrSql = "Select ToEndDate,  PayDate   From  tbl_CloseTotal_02 (nolock) ";
            StrSql = StrSql + " Where FromEndDate <='" + SellDate + "'";
            StrSql = StrSql + " And   ToEndDate >='" + SellDate + "'";

            DataSet Dset3 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset3);
            int ReCnt3 = Search_Connect.DataSet_ReCount;
            if (ReCnt3 > 0)
            {
                W_ToEndDate = Dset3.Tables[base_db_name].Rows[0]["ToEndDate"].ToString();
            }

        }

        private void Make_Pay_Base(double T_TF, double TotalPV, int UpGradeCnt, ref  double Pay1, ref double Pay2, ref double Pay3, ref  double Pay4, ref  double Pay5,
                                    ref string SortOrder1, ref string SortOrder2, ref string SortOrder3, ref  string SortOrder4, ref  string SortOrder5)
        {
            if (T_TF == 2)
            {
                if  (UpGradeCnt == 0 || UpGradeCnt == 1)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.05); SortOrder1 = "팀매니져 5%의 싱글";

                }

                if (UpGradeCnt == 2)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.035); SortOrder1 = "팀매니져 3.5%의 더블(하)" ;
                    Pay2 = Math.Truncate(TotalPV * 0.015); SortOrder2 = "팀매니져 1.5%의 더블(상)";
                }

                if (UpGradeCnt >= 3)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.03); SortOrder1 = "팀매니져 3%의 쓰리플(하)";
                    Pay2 = Math.Truncate(TotalPV * 0.013); SortOrder2 = "팀매니져 1.3%의 쓰리플(중)";
                    Pay3 = Math.Truncate(TotalPV * 0.007); SortOrder3 = "팀매니져 0.7%의 쓰리플(상)";
                }
            }


            if (T_TF == 3)
            {
                if (UpGradeCnt == 0 || UpGradeCnt == 1)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.04); SortOrder1 = "그룹매니져 4%의 싱글";

                }

                if (UpGradeCnt == 2)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.025); SortOrder1 = "그룹매니져 2.5%의 더블(하)";
                    Pay2 = Math.Truncate(TotalPV * 0.015); SortOrder2 = "그룹매니져 1.5%의 더블(상)";
                }

                if (UpGradeCnt == 3)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.02); SortOrder1 = "그룹매니져 2%의 쓰리플(하)";
                    Pay2 = Math.Truncate(TotalPV * 0.012); SortOrder2 = "그룹매니져 1.2%의 쓰리플(중)";
                    Pay3 = Math.Truncate(TotalPV * 0.008); SortOrder3 = "그룹매니져 0.8%의 쓰리플(상)";
                }

                if (UpGradeCnt >= 4)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.018); SortOrder1 = "그룹매니져 1.8%의 쓰리플(하)";
                    Pay2 = Math.Truncate(TotalPV * 0.012); SortOrder2 = "그룹매니져 1.2%의 쓰리플(중1)";
                    Pay3 = Math.Truncate(TotalPV * 0.005); SortOrder3 = "그룹매니져 0.6%의 쓰리플(중2)";
                    Pay4 = Math.Truncate(TotalPV * 0.004); SortOrder4 = "그룹매니져 0.4%의 쓰리플(상)";
                }
            }



            if (T_TF == 4)
            {
                if (UpGradeCnt == 0 || UpGradeCnt == 1)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.03); SortOrder1 = "마스타 3%의 싱글";

                }

                if (UpGradeCnt == 2)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.02); SortOrder1 = "마스타 2%의 더블(하)";
                    Pay2 = Math.Truncate(TotalPV * 0.01); SortOrder2 = "마스타 1%의 더블(상)";
                }

                if (UpGradeCnt == 3)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.015); SortOrder1 = "마스타 1.5%의 쓰리플(하)";
                    Pay2 = Math.Truncate(TotalPV * 0.009); SortOrder2 = "마스타 0.9%의 쓰리플(중)";
                    Pay3 = Math.Truncate(TotalPV * 0.006); SortOrder3 = "마스타 0.6%의 쓰리플(상)";
                }

                if (UpGradeCnt >= 4)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.012); SortOrder1 = "마스타 1.2%의 쓰리플(하)";
                    Pay2 = Math.Truncate(TotalPV * 0.009); SortOrder2 = "마스타 0.9%의 쓰리플(중1)";
                    Pay3 = Math.Truncate(TotalPV * 0.006); SortOrder3 = "마스타 0.6%의 쓰리플(중2)";
                    Pay4 = Math.Truncate(TotalPV * 0.003); SortOrder4 = "마스타 0.3%의 쓰리플(상)";
                }
            }

            if (T_TF == 5)
            {
                if (UpGradeCnt == 0 || UpGradeCnt == 1)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.02); SortOrder1 = "스타마스타 2%의 싱글";

                }

                if (UpGradeCnt == 2)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.014); SortOrder1 = "스타마스타 1.4%의 더블(하)";
                    Pay2 = Math.Truncate(TotalPV * 0.006); SortOrder2 = "스타마스타 0.6%의 더블(상)";
                }

                if (UpGradeCnt == 3)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.01); SortOrder1 = "스타마스타 1%의 쓰리플(하)";
                    Pay2 = Math.Truncate(TotalPV * 0.006); SortOrder2 = "스타마스타 0.6%의 쓰리플(중)";
                    Pay3 = Math.Truncate(TotalPV * 0.004); SortOrder3 = "스타마스타 0.4%의 쓰리플(상)";
                }

                if (UpGradeCnt >= 4)
                {
                    Pay1 = Math.Truncate(TotalPV * 0.008); SortOrder1 = "스타마스타 0.8%의 쓰리플(하)";
                    Pay2 = Math.Truncate(TotalPV * 0.006); SortOrder2 = "스타마스타 0.6%의 쓰리플(중1)";
                    Pay3 = Math.Truncate(TotalPV * 0.003); SortOrder3 = "스타마스타 0.3%의 쓰리플(중2)";
                    Pay4 = Math.Truncate(TotalPV * 0.003); SortOrder4 = "스타마스타 0.3%의 쓰리플(상)";
                }
            }


        }

        private void Put_tbl_Close_DownPV_ALL_04(string TSaveid ,int TSaveid2 ,string  T_Name , double TPay ,int TLevel ,int  TLine ,string  SortOrder
            , string OrderNumber, string SellDate, ref string StrSql, string Mbid, int Mbid2, string M_Name)
        {

            StrSql = " INSERT INTO tbl_Close_DownPV_ALL_04 " ;
            StrSql = StrSql +  " (EndDate,RequestMbid,RequestMbid2,RequestName,SaveMbid";
            StrSql = StrSql +  " ,SaveMbid2,SaveName,DownPV,LevelCnt,LineCnt,SortOrder,OrderNumber,SellDate)";
            StrSql = StrSql +  " Values(";
            StrSql = StrSql +  "'" + ToEndDate + "','" + Mbid + "'," + Mbid2 + ",'" + M_Name + "'";
            StrSql = StrSql +  ",'" + TSaveid + "'," + TSaveid2 + ",'" + T_Name + "'";
            StrSql = StrSql +  "," + TPay + "," + TLevel + "," + TLine;
            StrSql = StrSql +  ",'" + SortOrder + "','" + OrderNumber + "','" + SellDate + "')";

             
    
        }



        private void Give_Allowance_3_4_5_2018_0702(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {

            pg1.Value = 0; pg1.Maximum = 8;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
            double F_TotalPV = 0, Sum_T_PV_01 = 0;


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
            StrSql = "Select Count(Mbid) AS DayPV From tbl_ClosePay_04 ";
            StrSql = StrSql + " Where CurGrade =  40 ";
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

                StrSql = "Update tbl_ClosePay_04 Set";
                StrSql = StrSql + "  Allowance3 = " + Allowance1;
                StrSql = StrSql + " Where CurGrade =  40 ";
                StrSql = StrSql + " And   StopDate = '' ";
                StrSql = StrSql + " And   LeaveDate = '' ";
                StrSql = StrSql + " And   Sell_Mem_TF = 0 ";
                StrSql = StrSql + " And   ReqTF2 >= 1 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            pg1.PerformStep(); pg1.Refresh();
            //----------------------------------------------------------------------------------------------------------




            GradeCnt = 0; Allowance1 = 0;
            StrSql = "Select Count(Mbid) AS DayPV From tbl_ClosePay_04 ";
            StrSql = StrSql + " Where CurGrade = 50 ";
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

                StrSql = "Update tbl_ClosePay_04 Set";
                StrSql = StrSql + "  Allowance4 = " + Allowance1;
                StrSql = StrSql + " Where CurGrade = 50 ";
                StrSql = StrSql + " And   StopDate = '' ";
                StrSql = StrSql + " And   LeaveDate = '' ";
                StrSql = StrSql + " And   Sell_Mem_TF = 0 ";
                StrSql = StrSql + " And   ReqTF2 >= 1 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            pg1.PerformStep(); pg1.Refresh();
            //----------------------------------------------------------------------------------------------------------


            GradeCnt = 0; Allowance1 = 0;
            StrSql = "Select Count(Mbid) AS DayPV From tbl_ClosePay_04 ";
            StrSql = StrSql + " Where CurGrade = 60 ";
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

                StrSql = "Update tbl_ClosePay_04 Set";
                StrSql = StrSql + "  Allowance5 = " + Allowance1;
                StrSql = StrSql + " Where CurGrade = 60 ";
                StrSql = StrSql + " And   StopDate = '' ";
                StrSql = StrSql + " And   LeaveDate = '' ";
                StrSql = StrSql + " And   Sell_Mem_TF = 0 ";
                StrSql = StrSql + " And   ReqTF2 >= 1 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            pg1.PerformStep(); pg1.Refresh();
            //----------------------------------------------------------------------------------------------------------





        }




        private void Give_Allowance1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            int Cnt = 0;
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", SellDate = "";
            double Allowance2 = 0, Allowance1 = 0, Allowance3 = 0, Allowance4 = 0, TotalPV = 0, TH_TotalPV = 0, KR_TotalPV = 0, GivePay = 0;
            int L_1 = 0, L_2 = 0;

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();
            Dictionary<string, Double> Sell_PV = new Dictionary<string, Double>();

            pg1.Value = 0; pg1.Maximum = MaxLevel + 4;
            pg1.PerformStep(); pg1.Refresh();

            Cnt = MaxLevel;

            while (Cnt >= 1)
            {


                StrSql = " Select Se.TotalPrice , Isnull( Bs_R.TotalPrice, 0 ) AS RePV  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.Mbid,Se.Mbid2 , Se.Na_Code ";
                StrSql = StrSql + " , C3.UpGradeCnt20 ,  C3.UpGradeCnt30 ,  C3.UpGradeCnt40,  C3.UpGradeCnt50 "; 
                StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
                StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPrice  < 0  And  Bs_R.SellDate <= '" + ToEndDate + "'";
                StrSql = StrSql + " LEFT JOIN tbl_ClosePay_04  C22  (nolock)  ON C22.Mbid = Se.Mbid And C22.Mbid2 = Se.Mbid2 ";
                StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_02_OrderNumber_Mod C3  (nolock)  ON C3.OrderNumber = Se.OrderNumber "; 

                StrSql = StrSql + " WHERE Se.TotalPrice + Isnull( Bs_R.TotalPrice, 0 ) > 0 ";
                StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
                StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
                StrSql = StrSql + " And   Se.Ga_Order = 0 ";
                StrSql = StrSql + " And   Se.SellCode = '01' ";
                StrSql = StrSql + " And      C22.LevelCnt = " + Cnt;
                StrSql = StrSql + " Order by Se.Mbid , Se.Mbid2  ASC ";

                DataSet ds = new DataSet();
                ReCnt = 0;
                Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
                ReCnt = Search_Connect.DataSet_ReCount;

                pg1.Value = 0; pg1.Maximum = ReCnt + 1;

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    string W_ToEndDate = "";
                    TotalPV = 0;
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

                    TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString()) + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                    OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                    SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();
                    S_Mbid = TSaveid + "-" + TSaveid2.ToString();


                    int Cnt2 = 0, Cnt3 = 0, Cnt4 = 0, Cnt5 = 0 ;
                    double  Pay21 = 0, Pay22 = 0, Pay23 = 0, Pay24 = 0, Pay25 = 0;
                    double Pay31 = 0, Pay32 = 0, Pay33 = 0, Pay34 = 0, Pay35 = 0;
                    double Pay41 = 0, Pay42 = 0, Pay43 = 0, Pay44 = 0, Pay45 = 0;
                    double Pay51 = 0, Pay52 = 0, Pay53 = 0, Pay54 = 0, Pay55 = 0;
                
                    string SortOrder21 = "", SortOrder22 = "", SortOrder23 = "", SortOrder24 = "", SortOrder25 = "";
                    string SortOrder31 = "", SortOrder32 = "", SortOrder33 = "", SortOrder34 = "", SortOrder35 = "";
                    string SortOrder41 = "", SortOrder42 = "", SortOrder43 = "", SortOrder44 = "", SortOrder45 = "";
                    string SortOrder51 = "", SortOrder52 = "", SortOrder53 = "", SortOrder54 = "", SortOrder55 = "";

                    int UpGradeCnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["UpGradeCnt20"].ToString());
                    Make_Pay_Base(2, TotalPV, UpGradeCnt, ref Pay21, ref Pay22, ref Pay23, ref Pay24,ref  Pay25, ref SortOrder21, ref SortOrder22,ref  SortOrder23,ref  SortOrder24,ref  SortOrder25);

                    UpGradeCnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["UpGradeCnt30"].ToString());
                    Make_Pay_Base(3, TotalPV, UpGradeCnt, ref Pay31, ref Pay32, ref Pay33,ref  Pay34,ref  Pay35,ref  SortOrder31,ref  SortOrder32, ref SortOrder33,ref  SortOrder34,ref  SortOrder35);

                    UpGradeCnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["UpGradeCnt40"].ToString());
                    Make_Pay_Base(4, TotalPV, UpGradeCnt, ref Pay41,ref  Pay42, ref Pay43, ref Pay44, ref Pay45, ref SortOrder41, ref SortOrder42, ref SortOrder43,ref  SortOrder44,ref  SortOrder45);

                    UpGradeCnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["UpGradeCnt50"].ToString());

                    if (UpGradeCnt > 0)
                        UpGradeCnt = UpGradeCnt; 
                    Make_Pay_Base(5, TotalPV, UpGradeCnt, ref Pay51,ref  Pay52, ref Pay53, ref Pay54, ref Pay55, ref SortOrder51,ref  SortOrder52,ref  SortOrder53,ref  SortOrder54,ref  SortOrder55);

                    Make_Pay_Base(SellDate, ref W_ToEndDate );



                    while (TSaveid != "**" && W_ToEndDate != "")
                    {
                        LevelCnt++;

                        if (Clo_Mem.ContainsKey(S_Mbid) == true)
                        {
                            if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" )
                            {
                                Allowance1 = 0; Allowance2 = 0; Allowance3 = 0; Allowance4 = 0;
                                R_LevelCnt++;
                                int S_OrgGrade  = 0 ; 

                                Make_Pay_Base (W_ToEndDate , ref S_OrgGrade, TSaveid, TSaveid2 ) ; 

                                if (S_OrgGrade == 20 )
                                {
                                    Cnt2 = Cnt2 + 1 ;

                                    if (Cnt2 == 1)
                                    {
                                        Allowance1 = Pay21 ;
                                        if (Allowance1 > 0 )
                                        {
                                            StrSql = "" ;
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay21, LevelCnt, TLine, SortOrder21
                                                 ,OrderNumber,  SellDate ,ref StrSql , Mbid, Mbid2, M_Name) ;
                                            
                                            t_qu[t_qu_Cnt] = StrSql;
                                            t_qu_Cnt++;   

                                        }
                                        Pay21 = 0 ; 
                                    }

                                    if (Cnt2 == 2)
                                        {
                                            Allowance1 = Pay22 ;
                                            if( Allowance1 > 0 )
                                            {
                                                Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay22, LevelCnt, TLine, SortOrder22
                                                 , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);
                                            }

                                            Pay22 = 0 ; 
                                        }

                                    if (Cnt2 == 3)
                                    {
                                        Allowance1 = Pay23;
                                        if (Allowance1 > 0)
                                        {
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay23, LevelCnt, TLine, SortOrder23
                                             , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);
                                        }

                                        Pay23 = 0;
                                    }
                                
                                }



                                if (S_OrgGrade == 30)
                                {
                                    if (Cnt2 == 0)
                                    {
                                        Allowance1 = Pay21;
                                        if (Allowance1 > 0)
                                        {
                                            StrSql = "";
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay21, LevelCnt, TLine, SortOrder21
                                                 , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);

                                            t_qu[t_qu_Cnt] = StrSql;
                                            t_qu_Cnt++;

                                        }
                                        Pay21 = 0;
                                    }


                                    Cnt3 = Cnt3 + 1;

                                    if (Cnt3 == 1)
                                    {
                                        Allowance2 = Pay31;
                                        if (Allowance2 > 0)
                                        {
                                            StrSql = "";
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay31, LevelCnt, TLine, SortOrder31
                                                 , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);

                                            t_qu[t_qu_Cnt] = StrSql;
                                            t_qu_Cnt++;

                                        }
                                        Pay31 = 0;
                                    }

                                    if (Cnt3 == 2)
                                    {
                                        Allowance2 = Pay32;
                                        if (Allowance2 > 0)
                                        {
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay32, LevelCnt, TLine, SortOrder32
                                             , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);
                                        }

                                        Pay32 = 0;
                                    }

                                    if (Cnt3 == 3)
                                    {
                                        Allowance2 = Pay33;
                                        if (Allowance2 > 0)
                                        {
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay33, LevelCnt, TLine, SortOrder33
                                             , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);
                                        }

                                        Pay33 = 0;
                                    }

                                    if (Cnt3 == 4)
                                    {
                                        Allowance2 = Pay34;
                                        if (Allowance2 > 0)
                                        {
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay34, LevelCnt, TLine, SortOrder34
                                             , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);
                                        }

                                        Pay34 = 0;
                                    }

                                }




                                if (S_OrgGrade == 40)
                                {
                                    if (Cnt2 == 0)
                                    {
                                        Allowance1 = Pay21;
                                        if (Allowance1 > 0)
                                        {
                                            StrSql = "";
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay21, LevelCnt, TLine, SortOrder21
                                                 , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);

                                            t_qu[t_qu_Cnt] = StrSql;
                                            t_qu_Cnt++;

                                        }
                                        Pay21 = 0;
                                    }

                                    if (Cnt3 == 0)
                                    {
                                        Allowance2 = Pay31;
                                        if (Allowance2 > 0)
                                        {
                                            StrSql = "";
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay31, LevelCnt, TLine, SortOrder31
                                                 , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);

                                            t_qu[t_qu_Cnt] = StrSql;
                                            t_qu_Cnt++;

                                        }
                                        Pay31 = 0;
                                    }


                                    Cnt4 = Cnt4 + 1;

                                    if (Cnt4 == 1)
                                    {
                                        Allowance3 = Pay41;
                                        if (Allowance3 > 0)
                                        {
                                            StrSql = "";
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay41, LevelCnt, TLine, SortOrder41
                                                 , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);

                                            t_qu[t_qu_Cnt] = StrSql;
                                            t_qu_Cnt++;

                                        }
                                        Pay41 = 0;
                                    }

                                    if (Cnt4 == 2)
                                    {
                                        Allowance3 = Pay42;
                                        if (Allowance3 > 0)
                                        {
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay42, LevelCnt, TLine, SortOrder42
                                             , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);
                                        }

                                        Pay42 = 0;
                                    }

                                    if (Cnt4 == 3)
                                    {
                                        Allowance3 = Pay43;
                                        if (Allowance3 > 0)
                                        {
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay43, LevelCnt, TLine, SortOrder43
                                             , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);
                                        }

                                        Pay43 = 0;
                                    }

                                    if (Cnt4 == 4)
                                    {
                                        Allowance3 = Pay44;
                                        if (Allowance3 > 0)
                                        {
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay44, LevelCnt, TLine, SortOrder44
                                             , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);
                                        }

                                        Pay44 = 0;
                                    }

                                }



                                if (S_OrgGrade == 50)
                                {
                                    if (Cnt2 == 0)
                                    {
                                        Allowance1 = Pay21;
                                        if (Allowance1 > 0)
                                        {
                                            StrSql = "";
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay21, LevelCnt, TLine, SortOrder21
                                                 , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);

                                            t_qu[t_qu_Cnt] = StrSql;
                                            t_qu_Cnt++;

                                        }
                                        Pay21 = 0;
                                    }

                                    if (Cnt3 == 0)
                                    {
                                        Allowance2 = Pay31;
                                        if (Allowance2 > 0)
                                        {
                                            StrSql = "";
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay31, LevelCnt, TLine, SortOrder31
                                                 , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);

                                            t_qu[t_qu_Cnt] = StrSql;
                                            t_qu_Cnt++;

                                        }
                                        Pay31 = 0;
                                    }

                                    if (Cnt4 == 0)
                                    {
                                        Allowance3 = Pay41;
                                        if (Allowance3 > 0)
                                        {
                                            StrSql = "";
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay41, LevelCnt, TLine, SortOrder41
                                                 , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);

                                            t_qu[t_qu_Cnt] = StrSql;
                                            t_qu_Cnt++;

                                        }
                                        Pay41 = 0;
                                    }


                                    Cnt5 = Cnt5 + 1;

                                    if (Cnt5 == 1)
                                    {
                                        Allowance4 = Pay51;
                                        if (Allowance4 > 0)
                                        {
                                            StrSql = "";
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay51, LevelCnt, TLine, SortOrder51
                                                 , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);

                                            t_qu[t_qu_Cnt] = StrSql;
                                            t_qu_Cnt++;

                                        }
                                        Pay51 = 0;
                                    }

                                    if (Cnt5 == 2)
                                    {
                                        Allowance4 = Pay52;
                                        if (Allowance4 > 0)
                                        {
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay52, LevelCnt, TLine, SortOrder52
                                             , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);
                                        }

                                        Pay52 = 0;
                                    }

                                    if (Cnt5 == 3)
                                    {
                                        Allowance4 = Pay53;
                                        if (Allowance4 > 0)
                                        {
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay53, LevelCnt, TLine, SortOrder53
                                             , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);
                                        }

                                        Pay53 = 0;
                                    }

                                    if (Cnt5 == 4)
                                    {
                                        Allowance4 = Pay54;
                                        if (Allowance4 > 0)
                                        {
                                            Put_tbl_Close_DownPV_ALL_04(TSaveid, TSaveid2, Clo_Mem[S_Mbid].M_Name, Pay54, LevelCnt, TLine, SortOrder54
                                             , OrderNumber, SellDate, ref StrSql, Mbid, Mbid2, M_Name);
                                        }

                                        Pay54 = 0;
                                    }

                                }




                                if( Allowance1 + Allowance2 + Allowance3 + Allowance4 > 0 )
                                {
                                    StrSql = "Update tbl_ClosePay_04 SET ";
                                    StrSql = StrSql + " Allowance1=Allowance1+ " + Allowance1;
                                    StrSql = StrSql + ", Allowance2=Allowance2+ " + Allowance2;
                                    StrSql = StrSql + ", Allowance3=Allowance3+ " + Allowance3;
                                    StrSql = StrSql + ", Allowance4=Allowance4+ " + Allowance4;
                                    StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                    StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                    //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                    t_qu[t_qu_Cnt] = StrSql;
                                    t_qu_Cnt++;       
                                }


                            }

                            TSaveid = Clo_Mem[S_Mbid].Saveid; TSaveid2 = Clo_Mem[S_Mbid].Saveid2; TLine = Clo_Mem[S_Mbid].LineCnt ;

                            S_Mbid = TSaveid + "-" + TSaveid2.ToString();
                        }
                        else
                        {
                            TSaveid = "**";
                        }

                        //if (LevelCnt == 1) TSaveid = "**";

                    } //While


                }



                pg1.PerformStep(); pg1.Refresh();

                Cnt = Cnt - 1;
            }


            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
            {
                StrSql = t_qu[tkey];
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg1.PerformStep(); pg1.Refresh();
            }

        }






        private void Give_Allowance2_Be(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "";
            double Allowance1 = 0, Allowance2 = 0;

            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = "  Select  Allowance1 ,  Nominid , Nominid2 , Mbid , Mbid2 ";
            StrSql = StrSql + " ,N_LineCnt , M_Name, N_LineCnt , LineCnt   ";
            StrSql = StrSql + "  From tbl_ClosePay_04    ";
            StrSql = StrSql + " Where  Allowance1 > 0 ";

            ReCnt = 0;
            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;


            pg1.Value = 0; pg1.Maximum = ReCnt + 1;
            //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)            
            while (sr.Read())
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
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].CurGrade >= 30 && Clo_Mem[S_Mbid].ReqTF2  >= 1 )
                        {
                            Allowance2 = 0;
                            R_LevelCnt++;

                            if (LevelCnt == 1 )
                                Allowance2 = (Allowance1) * 0.2;

                            if (LevelCnt == 2)
                                Allowance2 = (Allowance1) * 0.15;

                            if (LevelCnt == 3)
                                Allowance2 = (Allowance1) * 0.1;

                            if (LevelCnt == 4)
                                Allowance2 = (Allowance1) * 0.05;


                            if (LevelCnt == 2 && Clo_Mem[S_Mbid].CurGrade <= 40)
                                Allowance2 = 0;

                            if (LevelCnt == 3 && Clo_Mem[S_Mbid].CurGrade <= 50)
                                Allowance2 = 0;

                            if (LevelCnt == 4 && Clo_Mem[S_Mbid].CurGrade <= 80)
                                Allowance2 = 0;

                            


                            if (Allowance1 > 0)
                            {
                                StrSql = "Update tbl_ClosePay_04 SET ";
                                StrSql = StrSql + " Allowance2 = Allowance2 +  " + Allowance2;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_04";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV , GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + ", 0 , " + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'2' ,'')";

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

                    if (LevelCnt == 4) TSaveid = "**";

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


        private void Give_Allowance2_TT(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 15    ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            //////테스트용임 다 지워야함. 아래 주석을 열어죽소
            ////StrSql = "Update tbl_ClosePay_04 SET ";
            ////StrSql = StrSql + " Allowance2 = Sum_PV_2 * 0.2 ";

            ////StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            ////StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            ////StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            ////StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";
                        
            ////StrSql = StrSql + " Where Sum_PV_1 >= Sum_PV_2 ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update tbl_ClosePay_04 SET ";
            ////StrSql = StrSql + " Allowance2 = Sum_PV_1 * 0.2 ";

            ////StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            ////StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            ////StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            ////StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";
            ////StrSql = StrSql + " Where Sum_PV_1 < Sum_PV_2 ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();





            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance2 = Sum_PV_2 * 0.2 ";

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            StrSql = StrSql + " Where (CurPoint = 2 OR CurGrade >= 2 ) ";
            StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance2 = Sum_PV_1 * 0.2 ";

            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            StrSql = StrSql + " Where (CurPoint = 2 OR CurGrade >= 2 ) ";
            StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();




            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance2 = Sum_PV_2 * 0.1";

            StrSql = StrSql + " ,Sum_PV_1 = Sum_PV_1 - Sum_PV_2 ";
            StrSql = StrSql + " ,Sum_PV_2 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_2 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_2 ";

            StrSql = StrSql + " Where (CurPoint = 1 ) ";
            StrSql = StrSql + " And Sum_PV_1 >= Sum_PV_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance2 = Sum_PV_1 * 0.1 ";

            StrSql = StrSql + " ,Sum_PV_2 = Sum_PV_2 - Sum_PV_1 ";
            StrSql = StrSql + " ,Sum_PV_1 = 0 ";

            StrSql = StrSql + " ,Ded_1 = Sum_PV_1 ";
            StrSql = StrSql + " ,Ded_2 = Sum_PV_1 ";

            StrSql = StrSql + " Where (CurPoint = 1 ) ";
            StrSql = StrSql + " And Sum_PV_1 < Sum_PV_2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        
    
    
    
            StrSql = "Update tbl_ClosePay_04 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 500000 "  ;
            StrSql = StrSql + " Where Allowance2 > 500000 "  ;
            StrSql = StrSql + " And CurGrade < 2 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
             StrSql = "Update tbl_ClosePay_04 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 1000000 "  ;
            StrSql = StrSql + " Where Allowance2 > 1000000 "  ;
            StrSql = StrSql + " And CurGrade = 2 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
             StrSql = "Update tbl_ClosePay_04 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 2500000 "  ;
            StrSql = StrSql + " Where Allowance2 > 2500000 "  ;
            StrSql = StrSql + " And CurGrade = 3 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
             StrSql = "Update tbl_ClosePay_04 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 5000000 "  ;
            StrSql = StrSql + " Where Allowance2 > 5000000 "  ;  ;
            StrSql = StrSql + " And CurGrade = 4 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
      
             StrSql = "Update tbl_ClosePay_04 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 10000000 "  ;
            StrSql = StrSql + " Where Allowance2 > 10000000 "  ;
            StrSql = StrSql + " And CurGrade = 5 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
    
              StrSql = "Update tbl_ClosePay_04 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 15000000 "  ;
            StrSql = StrSql + " Where Allowance2 > 15000000 "  ;
            StrSql = StrSql + " And CurGrade = 6 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
      
              StrSql = "Update tbl_ClosePay_04 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 25000000 "  ;
            StrSql = StrSql + " Where Allowance2 > 25000000 "  ;
            StrSql = StrSql + " And CurGrade = 7 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
      
              StrSql = "Update tbl_ClosePay_04 SET "  ;
            StrSql = StrSql + " Allowance2_Cut = Allowance2 - 30000000 "  ;
            StrSql = StrSql + " Where Allowance2 > 30000000 "  ;
            StrSql = StrSql + " And CurGrade = 8 "  ;
    
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
    
    
              StrSql = "Update tbl_ClosePay_04 SET "  ;
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
            StrSql = StrSql + "  From tbl_ClosePay_04    ";
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


                 


                StrSql = "Update tbl_ClosePay_04 SET ";
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












        private void Give_Allowance7(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "";
            double Allowance2 = 0, Allowance1 = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select  DownPV TotalPV , SaveName M_Name, OrderNumber, SaveMbid Mbid, SaveMbid2  Mbid2 ";
            StrSql = StrSql + " From tbl_Close_DownPV_ALL_04 Se (nolock) ";
            StrSql = StrSql + " WHERE DownPV  > 0 ";
            StrSql = StrSql + " And   EndDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   EndDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   SortOrder = '1' ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;
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

                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) ;
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].OneGrade >= 70)
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;

                            if (R_LevelCnt == 1) Allowance1 = TotalPV * 0.06;
                            if (R_LevelCnt == 2) Allowance1 = TotalPV * 0.05;
                            if (R_LevelCnt == 3) Allowance1 = TotalPV * 0.05;
                            if (R_LevelCnt == 4) Allowance1 = TotalPV * 0.04;
                            if (R_LevelCnt == 5) Allowance1 = TotalPV * 0.03;
                            if (R_LevelCnt == 6) Allowance1 = TotalPV * 0.02;
                            
                            if (R_LevelCnt >= 2 && Clo_Mem[S_Mbid].OneGrade <= 70) Allowance1 = 0;
                            if (R_LevelCnt >= 3 && Clo_Mem[S_Mbid].OneGrade <= 80) Allowance1 = 0;
                            if (R_LevelCnt >= 4 && Clo_Mem[S_Mbid].OneGrade <= 90) Allowance1 = 0;
                            if (R_LevelCnt >= 5 && Clo_Mem[S_Mbid].OneGrade <= 100) Allowance1 = 0;
                            if (R_LevelCnt >= 6 && Clo_Mem[S_Mbid].OneGrade <= 110) Allowance1 = 0;
                            

                            if (Allowance1 > 0)
                            {
                                //Allowance1 = Allowance1 * 1000;

                                StrSql = "Update tbl_ClosePay_04 SET ";
                                StrSql = StrSql + " Allowance7 = Allowance7 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_04";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_LevelCnt + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'7' ,'" + OrderNumber + "')";

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

                    if (R_LevelCnt == 6) TSaveid = "**";

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
            double Allowance2 = 0, Allowance1 = 0, TotalPV = 0;


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select  DownPV TotalPV , SaveName M_Name, OrderNumber, SaveMbid Mbid, SaveMbid2  Mbid2 ";
            StrSql = StrSql + " From tbl_Close_DownPV_ALL_04 Se (nolock) ";
            StrSql = StrSql + " WHERE DownPV  > 0 ";
            StrSql = StrSql + " And   EndDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   EndDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   SortOrder = '1' ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                LevelCnt = 0; TSaveid = "**";
                R_LevelCnt = 0;
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

                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                S_Mbid = TSaveid + "-" + TSaveid2.ToString();

                while (TSaveid != "**")
                {
                    LevelCnt++;

                    if (Clo_Mem.ContainsKey(S_Mbid) == true)
                    {
                        if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "" && Clo_Mem[S_Mbid].OneGrade >= 70)
                        {
                            Allowance1 = 0;
                            R_LevelCnt++;

                            if (R_LevelCnt == 1) Allowance1 = TotalPV * 0.06;
                            if (R_LevelCnt == 2) Allowance1 = TotalPV * 0.05;
                            if (R_LevelCnt == 3) Allowance1 = TotalPV * 0.05;
                            if (R_LevelCnt == 4) Allowance1 = TotalPV * 0.04;
                            if (R_LevelCnt == 5) Allowance1 = TotalPV * 0.03;
                            if (R_LevelCnt == 6) Allowance1 = TotalPV * 0.02;

                            if (R_LevelCnt >= 2 && Clo_Mem[S_Mbid].OneGrade <= 70) Allowance1 = 0;
                            if (R_LevelCnt >= 3 && Clo_Mem[S_Mbid].OneGrade <= 80) Allowance1 = 0;
                            if (R_LevelCnt >= 4 && Clo_Mem[S_Mbid].OneGrade <= 90) Allowance1 = 0;
                            if (R_LevelCnt >= 5 && Clo_Mem[S_Mbid].OneGrade <= 100) Allowance1 = 0;
                            if (R_LevelCnt >= 6 && Clo_Mem[S_Mbid].OneGrade <= 110) Allowance1 = 0;


                            if (Allowance1 > 0)
                            {
                                //Allowance1 = Allowance1 * 1000;

                                StrSql = "Update tbl_ClosePay_04 SET ";
                                StrSql = StrSql + " Allowance8 = Allowance8 +  " + Allowance1;
                                StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                t_qu[t_qu_Cnt] = StrSql;
                                t_qu_Cnt++;



                                StrSql = "INSERT INTO tbl_Close_DownPV_ALL_04";
                                StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                                StrSql = StrSql + "Values(";
                                StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                StrSql = StrSql + Allowance1 + " ," + R_LevelCnt + "," + LevelCnt + " ," + TLine;
                                StrSql = StrSql + ",'8' ,'" + OrderNumber + "')";

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

                    if (R_LevelCnt == 6) TSaveid = "**";

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


        private void Give_Allowance9_TTTT(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 6 ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            //4월 월마감 한달에만 운영한다고 함.. 걍 20만원 주라고함.
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance9 =  200000 ";
            StrSql = StrSql + " Where GradeDate5 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance10 =  100000 ";
            StrSql = StrSql + " Where GradeDate6 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance10 =  200000 ";
            StrSql = StrSql + " Where GradeDate7 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance10 =  300000 ";
            StrSql = StrSql + " Where GradeDate8 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance10 =  2400000 ";
            StrSql = StrSql + " Where GradeDate9 ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();            
        }


        private void Give_Allowance9_2015_0501(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 6;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance9 =  100000 ";
            StrSql = StrSql + " Where Us_Num in (65209,65272,65386,65377,66427,66436,66769,65368,66967) ";           
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance9 =  200000 ";
            StrSql = StrSql + " Where Us_Num in (65314,72187,65404,72178,73978,72856,72196,72226,73261,66931) ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

           
        }

        private void Give_Allowance11(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_04_G SET ";
            StrSql = StrSql + " Cut_TF =  Be_Cut_TF ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();


            StrSql = " Select  Ce04.Mbid, Ce04.Mbid2, Ce04.M_Name, Ce04.OneGrade, G04.A_Grade, G04.Max_Cnt , G04.Total_Pay , G04.Be_Cnt , G04.Cut_TF, G04.Seq, G04.M_ToEndDate ";
            StrSql = StrSql + " From tbl_ClosePay_04_G (nolock)  G04 ";
            StrSql = StrSql + " LEFT JOIN tbl_ClosePay_04 (nolock) Ce04 ON Ce04.Mbid2 = G04.Mbid2 ";
            StrSql = StrSql + " WHERE G04.End_Date  = ''   ";
            StrSql = StrSql + " And G04.Be_Cnt + 1 <= G04.Max_Cnt ";
            StrSql = StrSql + " And G04.Cut_TF <= 1  ";

            DataSet ds = new DataSet();
            ReCnt = 0;
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds);
            ReCnt = Search_Connect.DataSet_ReCount;

            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            int Mbid2 = 0, Cut_TF = 0, Be_Cnt = 0, Cur_Cnt = 0, Total_Pay = 0, Max_Cnt = 0, A_Grade = 0, OneGrade = 0, Seq = 0;
            string Mbid = "", M_Name = "", End_Date = "", M_ToEndDate = "";
            double Allowance11 = 0;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                End_Date = "";
                Cur_Cnt = 0;
                Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();

                OneGrade = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["OneGrade"].ToString());
                A_Grade = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["A_Grade"].ToString());
                Max_Cnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Max_Cnt"].ToString());
                Total_Pay = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Total_Pay"].ToString());
                Be_Cnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Be_Cnt"].ToString());
                Cut_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Cut_TF"].ToString());

                Seq = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Seq"].ToString());
                M_ToEndDate = ds.Tables[base_db_name].Rows[fi_cnt]["M_ToEndDate"].ToString();


                if (OneGrade + 20 < A_Grade)
                    Cut_TF = 2;

                if (OneGrade + 20 == A_Grade)
                    Cut_TF = 1;


                Allowance11 = Total_Pay / Max_Cnt;
                if (Cut_TF == 1)
                    Allowance11 = Allowance11 * 0.5;


                if (Cut_TF == 2)
                {
                    Allowance11 = 0;

                    StrSql = "Update tbl_ClosePay_04_G SET ";
                    StrSql = StrSql + " Cut_TF =   " + Cut_TF;
                    StrSql = StrSql + " Where   Seq = " + Seq;

                    t_qu[t_qu_Cnt] = StrSql;
                    t_qu_Cnt++;
                }



                if (Allowance11 > 0)
                {
                    Cur_Cnt = Be_Cnt + 1;

                    if (Cur_Cnt == Max_Cnt)
                        End_Date = ToEndDate;


                    StrSql = "Update tbl_ClosePay_04_G SET ";
                    StrSql = StrSql + " Cur_Cnt =   " + Cur_Cnt;
                    StrSql = StrSql + " , End_Date =   '" + End_Date + "'";
                    StrSql = StrSql + " , Cut_TF =   " + Cut_TF;
                    StrSql = StrSql + " Where   Seq = " + Seq;

                    t_qu[t_qu_Cnt] = StrSql;
                    t_qu_Cnt++;


                    StrSql = "Update tbl_ClosePay_04 SET ";
                    StrSql = StrSql + " Allowance11 = Allowance11 +  " + Allowance11;
                    StrSql = StrSql + " Where   Mbid = '" + Mbid + "'";
                    StrSql = StrSql + " And     Mbid2 = " + Mbid2;

                    t_qu[t_qu_Cnt] = StrSql;
                    t_qu_Cnt++;



                    StrSql = "INSERT INTO tbl_Close_DownPV_ALL_04";
                    StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                    StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                    StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber) ";

                    StrSql = StrSql + "Values(";
                    StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                    StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                    StrSql = StrSql + "'" + Mbid + "'," + Mbid2 + ",'" + M_Name + "',";
                    StrSql = StrSql + Allowance11 + " ,0,0 ,1";
                    StrSql = StrSql + ",'11' ,'" + Seq + "')";


                    t_qu[t_qu_Cnt] = StrSql;
                    t_qu_Cnt++;

                }



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





        private void Give_Allowance12(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 9;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance9 =  200000 ";
            StrSql = StrSql + " Where GradeDate5 >='" + FromEndDate + "'";
            StrSql = StrSql + " And  GradeDate5 <='" + ToEndDate + "'";
            StrSql = StrSql + " And   DateDiff(D, Regtime, GradeDate5) <= 30 "   ;            

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance9 =  100000 ";
            StrSql = StrSql + " Where GradeDate5 >='" + FromEndDate + "'";
            StrSql = StrSql + " And  GradeDate5 <='" + ToEndDate + "'";
            StrSql = StrSql + " And   DateDiff(D, Regtime, GradeDate5) > 30 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            //신규 엘리트 무조건 20
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance12 =  200000 ";
            StrSql = StrSql + " Where GradeDate5 >='" + FromEndDate + "'";
            StrSql = StrSql + " And  GradeDate5 <='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //신규 브론즈 30
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance12 = Allowance12 +  300000 ";
            StrSql = StrSql + " Where GradeDate6 >='" + FromEndDate + "'";
            StrSql = StrSql + " And  GradeDate6 <='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //엘리트 유지 20 
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance12 = Allowance12 +   200000 ";
            StrSql = StrSql + " Where OneGrade = 50 ";
            StrSql = StrSql + " And  GradeDate5 <>'" + ToEndDate + "'";  

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            //브론즈 유지 30
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance12 = Allowance12 +   300000 ";
            StrSql = StrSql + " Where OneGrade = 60 ";
            StrSql = StrSql + " And  GradeDate6 <>'" + ToEndDate + "'";  

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //신규 엘리트 관련해서 추천인 한태 무조건 명당 20
            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Allowance12 = Allowance12 +  ( 200000 * ISNULL(B.A1, 0 ))   ";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

            StrSql = StrSql + " (Select    Count(Mbid)  A1,  Nominid2    ";
            StrSql = StrSql + " From tbl_ClosePay_04 (nolock) ";
            StrSql = StrSql + " Where GradeDate5 >='" + FromEndDate + "'";
            StrSql = StrSql + " And  GradeDate5 <='" + ToEndDate + "'";
            StrSql = StrSql + " Group by Nominid2 "; 
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid2  = B.Nominid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }


        private void Give_Allowance9(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            int LevelCnt = 0, TSaveid2 = 0, TLine = 0, R_LevelCnt = 0, Mbid2 = 0, CurGrade = 0;
            string TSaveid = "", S_Mbid = "", Mbid = "", M_Name = "", OrderNumber = "", ABC_Price = "", BusCode = "";
            double Allowance2 = 0, Allowance1 = 0, TotalPV = 0;



            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select Se.TotalCV TotalPV , Isnull( Bs_R.TotalCV, 0 ) AS RePV  , Se.M_Name, Se.OrderNumber, Se.SellCode, Se.SellDate , Se.BusCode,  Se.Mbid,Se.Mbid2 ";
            
            StrSql = StrSql + " From tbl_SalesDetail Se (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_SalesDetail  AS Bs_R  (nolock)  ON Bs_R.Re_BaseOrderNumber  = Se.OrderNumber   And     Bs_R.TotalPV  + Bs_R.TotalCV < 0  And  Bs_R.SellDate <= '" + ToEndDate + "'";

            StrSql = StrSql + " WHERE Se.TotalCV + Isnull( Bs_R.TotalCV, 0 ) > 0 ";
            StrSql = StrSql + " And   Se.SellDate  >='" + FromEndDate + "'";
            StrSql = StrSql + " And   Se.SellDate  <='" + ToEndDate + "'";
            StrSql = StrSql + " And   Se.Ga_Order = 0 ";
            StrSql = StrSql + " And   Se.BusCode <> '' ";
            StrSql = StrSql + " And   Se.BusCode  in (Select ncode From tbl_business (nolock) Where Mbid2 > 0  ) ";
            
            
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
                BusCode = ds.Tables[base_db_name].Rows[fi_cnt]["BusCode"].ToString();

                S_Mbid = Mbid + "-" + Mbid2.ToString();
                if (Clo_Mem.ContainsKey(S_Mbid) == true)
                {
                    TSaveid = Clo_Mem[S_Mbid].Nominid;
                    TSaveid2 = Clo_Mem[S_Mbid].Nominid2;
                    TLine = Clo_Mem[S_Mbid].N_LineCnt;
                    CurGrade = Clo_Mem[S_Mbid].CurGrade;
                    
                }


                TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString()) + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RePV"].ToString());
                OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                if (BusCode != "" && BusCode != null)
                {

                    LevelCnt = 0;

                    //센타코드를 기준으로 해서 센타장이 누구인지를 알아온다.
                    StrSql = " Select Mbid,Mbid2  ";
                    StrSql = StrSql + " From tbl_Business (nolock) ";
                    StrSql = StrSql + " WHERE    NCode  ='" + BusCode + "'";

                    DataSet ds2 = new DataSet();

                    Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, ds2);
                    int ReCnt2 = Search_Connect.DataSet_ReCount;

                    TSaveid = ds2.Tables[base_db_name].Rows[0]["Mbid"].ToString();
                    TSaveid2 = int.Parse(ds2.Tables[base_db_name].Rows[0]["Mbid2"].ToString());

                    S_Mbid = TSaveid + "-" + TSaveid2.ToString(); //센타장 관련 정보를 넣는다.

                    while (TSaveid2 > 0 && TSaveid != "**" )
                    {
                        LevelCnt++;

                        if (Clo_Mem.ContainsKey(S_Mbid) == true)
                        {
                            if (TLine > 0 && Clo_Mem[S_Mbid].LeaveDate == "" && Clo_Mem[S_Mbid].StopDate == "")
                            {
                                Allowance1 = TotalPV * 0.05;
                                R_LevelCnt++;

                                if (Allowance1 > 0)
                                {
                                    StrSql = "Update tbl_ClosePay_04 SET ";
                                    StrSql = StrSql + " Allowance9 = Allowance9 +  " + Allowance1;
                                    StrSql = StrSql + " Where   Mbid = '" + TSaveid + "'";
                                    StrSql = StrSql + " And     Mbid2 = " + TSaveid2;

                                    //Temp_Connect.Insert_Data(StrSql, Conn, tran);
                                    t_qu[t_qu_Cnt] = StrSql;
                                    t_qu_Cnt++;



                                    StrSql = "INSERT INTO tbl_Close_DownPV_ALL_04";
                                    StrSql = StrSql + " (EndDate , RequestMbid , RequestMbid2 , RequestName,";
                                    StrSql = StrSql + " SaveMbid , SaveMbid2 , SaveName , DownPV ,GivePay , ";
                                    StrSql = StrSql + " LevelCnt , LineCnt , SortOrder , OrderNumber, SellCode) ";

                                    StrSql = StrSql + "Values(";
                                    StrSql = StrSql + "'" + ToEndDate + "','" + Mbid + "'";
                                    StrSql = StrSql + "," + Mbid2 + ",'" + M_Name + "',";
                                    StrSql = StrSql + "'" + TSaveid + "'," + TSaveid2 + ",'" + Clo_Mem[S_Mbid].M_Name + "',";
                                    StrSql = StrSql + Allowance1 + " ," + R_LevelCnt + "," + LevelCnt + " ," + TLine;
                                    StrSql = StrSql + ",'9' ,'" + OrderNumber + "','" + BusCode + "')";

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
            }


            pg1.Value = 0; pg1.Maximum = t_qu.Count + 1; pg1.Refresh();
            foreach (int tkey in t_qu.Keys)
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

                StrSql = "SELECT  DownPV , SaveMbid, SaveMbid2, SaveName ,SortOrder " ;
                StrSql = StrSql + " From tbl_Close_DownPV_ALL_04 (nolock) ";
                StrSql = StrSql + " WHERE RequestMbid = '" + Mbid + "'" ;
                StrSql = StrSql + " And   RequestMbid2 = " + Mbid2 ;
                StrSql = StrSql + " And   OrderNumber = '" + Re_BaseOrderNumber + "'" ;

                
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
                        Base_PV = double.Parse(ds3.Tables[base_db_name].Rows[0]["TotalPV"].ToString());


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
                                StrSql = StrSql + "'" + SellDate + "'," + Return_Pay + "," + Return_Pay + ",4";
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


            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql + " Sum_Return_Take_Pay = ISNULL(B.A1, 0 )   " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, " ;
    
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
            StrSql = "Update tbl_ClosePay_04 SET " ;
            StrSql = StrSql + " Sum_Return_DedCut_Pay = ISNULL(B.A1, 0 )   " ;
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, " ;
    
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




            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Sum_Return_Remain_Pay = Sum_Return_Take_Pay - Sum_Return_DedCut_Pay " ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }

        private void CalculateTruePayment(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 8    ;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";



            StrSql = "Update tbl_ClosePay_04 SET ";
            StrSql = StrSql + " Etc_Pay = ISNULL(B.A1, 0 )   ";
            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

            StrSql = StrSql + " (Select    Sum(Apply_Pv) A1,  mbid ,mbid2   ";
            StrSql = StrSql + " From tbl_Sham_Pay (nolock) ";
            StrSql = StrSql + " Where   Apply_Date >='" + FromEndDate  + "'";
            StrSql = StrSql + " And     Apply_Date <= '" + ToEndDate  + "' ";
            StrSql = StrSql + " And     (SortKind2 = '04' Or SortKind2 = '4') ";
            StrSql = StrSql + " Group By mbid ,mbid2 ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid  = B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2  = B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);





            StrSql = "Update tbl_ClosePay_04 Set "    ;
            StrSql = StrSql + " SumAllAllowance = Allowance1 + Allowance2 + Allowance3 + Allowance4 + Allowance5 + Allowance6 +Allowance7 + Allowance8 + Allowance9 + Allowance10 + Etc_Pay ";
            StrSql = StrSql + " Where Allowance1 + Allowance2 + Allowance3 + Allowance4 + Allowance5 + Allowance6 +Allowance7 + Allowance8 + Allowance9 + Allowance10 +  Etc_Pay  > 0";
    
             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
                

            //'''---반품으로 해서 차감시킬 금액이 아직 남아잇다.
            StrSql = "Update tbl_ClosePay_04 Set "    ;
            StrSql = StrSql + " Cur_DedCut_Pay = SumAllAllowance "    ;
            StrSql = StrSql + ",SumAllAllowance = 0 "    ;
            StrSql = StrSql + " Where SumAllAllowance  > 0"    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay > 0 "    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay >= SumAllAllowance "    ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set "    ;
            StrSql = StrSql + " Cur_DedCut_Pay = Sum_Return_Remain_Pay "    ;
            StrSql = StrSql + ",SumAllAllowance = SumAllAllowance - Sum_Return_Remain_Pay "    ;
            StrSql = StrSql + " Where SumAllAllowance  > 0"    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay > 0 "    ;
            StrSql = StrSql + " And   Sum_Return_Remain_Pay < SumAllAllowance "    ;

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_04 SET ";
            //StrSql = StrSql + " SumAllAllowance_10000 = ISNULL(B.A1, 0 )   ";
            //StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

            //StrSql = StrSql + " (Select    Sum(SumAllAllowance) A1,  Mbid ,Mbid2   ";
            //StrSql = StrSql + " From tbl_ClosePay_10000 ";
            //StrSql = StrSql + " Where   AP_ToEndDate  ='' ";
            //StrSql = StrSql + " And     SumAllAllowance > 0 ";
            //StrSql = StrSql + " Group By Mbid ,Mbid2 ";
            //StrSql = StrSql + " ) B";

            //StrSql = StrSql + " Where A.Mbid  = B.Mbid ";
            //StrSql = StrSql + " And   A.Mbid2  = B.Mbid2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);



        
            StrSql = "Update tbl_ClosePay_04 Set "    ;
            StrSql = StrSql + " SumAllAllowance = Convert(int,(SumAllAllowance ) /10) * 10  ";            
            StrSql = StrSql + " Where SumAllAllowance > 0 ";

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            
           
            StrSql = "Update tbl_ClosePay_04 Set "    ;
            StrSql = StrSql + " InComeTax = Convert(int,((SumAllAllowance ) * 0.03) /10) * 10  ";
            StrSql = StrSql + " Where SumAllAllowance > 0 ";

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set "    ;
            StrSql = StrSql + " ResidentTax = Convert(int,(InComeTax * 0.1) /10) * 10  "    ;            
            StrSql = StrSql + " Where SumAllAllowance > 0 ";

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
            

            StrSql = "Update tbl_ClosePay_04 Set "    ;
            StrSql = StrSql + " TruePayment = ((SumAllAllowance - InComeTax - ResidentTax) / 10 ) * 10 "    ;            
            StrSql = StrSql + " Where SumAllAllowance > 0 ";

             Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            //StrSql = "Update tbl_ClosePay_10000 SET ";
            //StrSql = StrSql + "  AP_ToEndDate = '" + ToEndDate + "'";
            //StrSql = StrSql + " ,AP_TF = 4 ";
            //StrSql = StrSql + " FROM  tbl_ClosePay_10000  A, ";

            //StrSql = StrSql + " (Select  Mbid,Mbid2 ";
            //StrSql = StrSql + " From tbl_ClosePay_04 ";
            //StrSql = StrSql + " Where SumAllAllowance + SumAllAllowance_10000  >= 1000 ";
            //StrSql = StrSql + " And SumAllAllowance_10000 > 0 ";
            //StrSql = StrSql + " And SumAllAllowance > 0 ";
            //StrSql = StrSql + " ) B";

            //StrSql = StrSql + " Where A.AP_ToEndDate =  '' ";
            //StrSql = StrSql + " And   A.Mbid = B.Mbid ";
            //StrSql = StrSql + " And   A.Mbid2 = B.Mbid2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);



            //StrSql = "INSERT INTO tbl_ClosePay_10000 ";
            //StrSql = StrSql + "(ToEndDAte,ToEndDAte_TF, mbid,mbid2,SumAllAllowance ) ";
            //StrSql = StrSql + " Select  ";
            //StrSql = StrSql + " '" + ToEndDate + "',4 ,Mbid,Mbid2,SumAllAllowance ";
            //StrSql = StrSql + " From tbl_ClosePay_04 ";
            //StrSql = StrSql + " Where SumAllAllowance + SumAllAllowance_10000  < 1000 ";
            //StrSql = StrSql + " And SumAllAllowance > 0 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);




            ////StrSql = "Update tbl_ClosePay_04 Set ";
            ////StrSql = StrSql + " Sum_Gibu = ((TruePayment * (GiBu_/100)) / 10 ) * 10 ";
            ////StrSql = StrSql + " Where TruePayment  > 0 ";
            ////StrSql = StrSql + " And   GiBu_  > 0 ";

            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //StrSql = "Update tbl_ClosePay_04 Set ";
            //StrSql = StrSql + " TruePayment = TruePayment -  Sum_Gibu ";
            //StrSql = StrSql + " Where TruePayment  > 0 ";
            //StrSql = StrSql + " And   Sum_Gibu  > 0 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);

        }




        private void CalculateTruePayment_US(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 8;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";



            //StrSql = "Update tbl_ClosePay_04 SET ";
            //StrSql = StrSql + " SumAllAllowance_US = ISNULL(B.A1, 0 )   ";
            //StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

            //StrSql = StrSql + " (Select    Sum(Apply_Pv) A1,  mbid ,mbid2   ";
            //StrSql = StrSql + " From tbl_Sham_Pay (nolock) ";
            //StrSql = StrSql + " Where   Apply_Date >='" + FromEndDate + "'";
            //StrSql = StrSql + " And     Apply_Date <= '" + ToEndDate + "' ";
            //StrSql = StrSql + " And     (SortKind2 = '04' Or SortKind2 = '4') ";
            //StrSql = StrSql + " Group By mbid ,mbid2 ";
            //StrSql = StrSql + " ) B";

            //StrSql = StrSql + " Where A.Mbid  = B.Mbid ";
            //StrSql = StrSql + " And   A.Mbid2  = B.Mbid2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);




            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " SumAllAllowance_US = Convert(int,(SumAllAllowance_US) /10) * 10  ";
            StrSql = StrSql + " Where SumAllAllowance_US  > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " InComeTax_US = Convert(int,(SumAllAllowance_US * 0.03) /10) * 10  ";
            StrSql = StrSql + " Where SumAllAllowance_US  > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " ResidentTax_US = Convert(int,(InComeTax_US * 0.1) /10) * 10  ";
            StrSql = StrSql + " Where SumAllAllowance_US  > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " TruePayment_US = ((SumAllAllowance_US - InComeTax_US - ResidentTax_US) / 10 ) * 10 ";
            StrSql = StrSql + " Where SumAllAllowance_US  > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " Sum_Gibu_US = ((TruePayment_US * (GiBu_/100)) / 10 ) * 10 ";
            StrSql = StrSql + " Where TruePayment_US  > 0 ";
            StrSql = StrSql + " And   GiBu_  > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + " TruePayment_US = TruePayment_US -  Sum_Gibu_US ";
            StrSql = StrSql + " Where TruePayment_US  > 0 ";
            StrSql = StrSql + " And   Sum_Gibu_US  > 0 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

        }



        private void Chang_RetunPay_Table(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            int t_qu_Cnt = 0;
            Dictionary<int, string> t_qu = new Dictionary<int, string>();

            StrSql = " Select Cur_DedCut_Pay,Mbid,Mbid2 , M_Name";
            StrSql = StrSql + " From tbl_ClosePay_04    ";
            StrSql = StrSql + " WHERE Cur_DedCut_Pay > 0 ";

            ReCnt = 0;
            SqlDataReader sr = null;
            Temp_Connect.Open_Data_Set(StrSql, Conn, tran, ref sr);
            ReCnt = Temp_Connect.DataSet_ReCount;
                
            pg1.Value = 0; pg1.Maximum = ReCnt + 1;

            int Mbid2 = 0,  Top_SW = 0, T_Pay = 0, TSw = 0,  T_index = 0 ;
            double Cut_Pay = 0, RR_Cut_Pay = 0;
            string Mbid = "", Re_BaseOrderNumber = "", M_Name = "";  
          
            
            //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            while (sr.Read ())
            {
                Cut_Pay = double.Parse(sr.GetValue (0).ToString ());
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
                    //t_qu.Clear();

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
                                StrSql = StrSql + "," + T_index + ",4";
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


            StrSql = "INSERT INTO tbl_CloseTotal_04 ";
            StrSql = StrSql +  " (ToEndDate,      FromEndDate,   PayDate ,   PayDate2 " ;
            StrSql = StrSql +  " ,TotalSellAmount,TotalInputCash,TotalInputCard,TotalInputBank"  ;
            StrSql = StrSql +  " ,TotalSellPV,    TotalShamPV,   TotalReturnSellAmount"  ;
            StrSql = StrSql +  " ,TotalReturnInputCash, TotalReturnInputCard,TotalReturnInputBank, TotalReturnSellPV "  ;
            StrSql = StrSql +  " ,TotalSellCV,TotalReturnSellCV " ; 
            StrSql = StrSql +  " ,Temp01,Temp02, Temp03, Temp04, Temp05, Temp06 , Temp07, Temp08, Temp09, Temp10, Temp11, Temp12 "  ; 
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
            StrSql = StrSql +  ", 0 , 0 " ; 

            StrSql = StrSql +  ",'" + cls_User.gid  + "',Convert(Varchar(25),GetDate(),21)" ;
            StrSql = StrSql + " From  tbl_ClosePay_04_Sell ";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

        }




        private void tbl_CloseTotal_Put2(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            StrSql = "Update tbl_CloseTotal_04 SET " ;
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

            //StrSql = StrSql + " ,Allowance11 =ISNULL(B.A11,0) ";
            //StrSql = StrSql + " ,Allowance12 =ISNULL(B.A12,0) ";
        //    StrSql = StrSql + " ,Allowance13 =ISNULL(B.A13,0) ";
        ////    StrSql = StrSql + " ,Allowance14 =ISNULL(B.A14,0) " ;
        ////    StrSql = StrSql + " ,Allowance15 =ISNULL(B.A15,0) " ;
        ////'    StrSql = StrSql + " ,Allowance16 =ISNULL(B.A16,0) " ;
        ////'    StrSql = StrSql + " ,Allowance17 =ISNULL(B.A17,0) " ;
        ////'    StrSql = StrSql + " ,Allowance18 =ISNULL(B.A18,0) " ;
        ////'    StrSql = StrSql + " ,Allowance19 =ISNULL(B.A19,0) " ;
        ////'    StrSql = StrSql + " ,Allowance20 =ISNULL(B.A20,0) " ;
        ////'
        ////    StrSql = StrSql + " ,Allowance21 =ISNULL(B.A21,0) " ;
        ////    StrSql = StrSql + " ,Allowance22 =ISNULL(B.A22,0) " ;
        ////    StrSql = StrSql + " ,Allowance23 =ISNULL(B.A23,0) " ;
        ////    StrSql = StrSql + " ,Allowance24 =ISNULL(B.A24,0) " ;
        ////    StrSql = StrSql + " ,Allowance25 =ISNULL(B.A25,0) " ;
        ////    StrSql = StrSql + " ,Allowance26 =ISNULL(B.A26,0) " ;
        ////    StrSql = StrSql + " ,Allowance27 =ISNULL(B.A27,0) " ;
            ////StrSql = StrSql + " ,Allowance28 =ISNULL(B.A28,0) ";
            StrSql = StrSql + " ,Allowance29 =ISNULL(B.A29,0) ";  //반품공제
            StrSql = StrSql + " ,Allowance30 =ISNULL(B.A30,0) ";  //기타보너스

            StrSql = StrSql + " ,SumAllowance=ISNULL(B.AS1,0) " ;
            StrSql = StrSql + " ,SumInComeTax=ISNULL(B.AS2,0) " ;
            StrSql = StrSql + " ,SumResidentTax=ISNULL(B.AS3,0) " ;
            StrSql = StrSql + " ,SumTruePayment=ISNULL(B.AS4,0) " ;

            StrSql = StrSql + " FROM  tbl_CloseTotal_04  A, " ;

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
        //    StrSql = StrSql + ",Sum(Allowance19) AS A19,Sum(Allowance20) AS A20 " ;
        
        //    StrSql = StrSql + ",Sum(Allowance21) AS A21,Sum(Allowance22) AS A22 " ;
        //    StrSql = StrSql + ",Sum(Allowance23) AS A23,Sum(Allowance24) AS A24 " ;
        //    StrSql = StrSql + ",Sum(Allowance25) AS A25 " //,Sum(Allowance26) AS A26" ;
        //    StrSql = StrSql + ",Sum(Allowance27) AS A27,Sum(Allowance28) AS A28 " ;

            ////StrSql = StrSql + ",Sum(convert(float,Allowance1_cut)) AS A28";
            StrSql = StrSql + ",Sum(convert(float,Cur_DedCut_Pay)) AS A29";
            StrSql = StrSql + ",Sum(convert(float,Etc_Pay)) AS A30 ";


            StrSql = StrSql + ",Sum(convert(float,SumAllAllowance)) AS AS1,Sum(convert(float,InComeTax)) AS AS2 ";
            StrSql = StrSql + ",Sum(convert(float,ResidentTax)) AS AS3,Sum(convert(float,TruePayment)) AS AS4 ";
            StrSql = StrSql + " From tbl_ClosePay_04 ";
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


            StrSql = "Update tbl_CloseTotal_04 Set "  ;
            StrSql = StrSql + "  Allowance1Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance1 > 0),0) ";
            StrSql = StrSql + " ,Allowance2Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance2 > 0),0) ";
            StrSql = StrSql + " ,Allowance3Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance3 > 0),0) ";
            StrSql = StrSql + " ,Allowance4Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance4 > 0),0) ";
            StrSql = StrSql + " ,Allowance5Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance5 > 0),0) ";
            StrSql = StrSql + " ,Allowance6Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance6 > 0),0) ";
            StrSql = StrSql + " ,Allowance7Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance7> 0),0) ";
            StrSql = StrSql + " ,Allowance8Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance8 > 0),0) ";
            StrSql = StrSql + " ,Allowance9Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance9 > 0),0) ";
            StrSql = StrSql + " ,Allowance10Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance10 > 0),0) ";

            //StrSql = StrSql + " ,Allowance11Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance11 > 0),0) ";
            //StrSql = StrSql + " ,Allowance12Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance12 > 0),0) "  ;
            //StrSql = StrSql + " ,Allowance13Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance13 > 0),0) "  ;
            ////    StrSql = StrSql + " ,Allowance14Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance14 > 0),0) "  ;
            ////    StrSql = StrSql + " ,Allowance15Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance15 > 0),0) "  ;
            ////'    StrSql = StrSql + " ,Allowance16Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance16 > 0),0) "  ;
            ////'    StrSql = StrSql + " ,Allowance17Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance17> 0),0) "  ;
            ////'    StrSql = StrSql + " ,Allowance18Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance18 > 0),0) "  ;
            //////'    StrSql = StrSql + " ,Allowance19Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance19 > 0),0) "  ;
            ////'    StrSql = StrSql + " ,Allowance20Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance20 > 0),0) "  ;
        ////'
            ////    StrSql = StrSql + " ,Allowance21Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance21 > 0),0) "  ;
            ////    StrSql = StrSql + " ,Allowance22Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance22 > 0),0) "  ;
            ////    StrSql = StrSql + " ,Allowance23Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance23 > 0),0) "  ;
            ////    StrSql = StrSql + " ,Allowance24Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance24 > 0),0) "  ;
            ////    StrSql = StrSql + " ,Allowance25Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance25 > 0),0) "  ;
            //    StrSql = StrSql + " ,Allowance26Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance26 > 0),0) "  ;
            //    StrSql = StrSql + " ,Allowance27Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance27> 0),0) "  ;
            ////StrSql = StrSql + " ,Allowance28Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Allowance1_cut > 0),0) ";
            StrSql = StrSql + " ,Allowance29Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Cur_DedCut_Pay > 0),0) ";
            StrSql = StrSql + " ,Allowance30Cnt = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where Etc_Pay > 0),0) ";
            StrSql = StrSql + " ,SumAllowanceCount = ISNULL((Select Count(Mbid) From tbl_ClosePay_04 Where SumAllAllowance > 0),0) "; 
            
            StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "'" ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_CloseTotal_04 Set " ; 
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

            //StrSql = StrSql + " ,Allowance11Rate = (Allowance11 /(TotalSellAmount-TotalReturnSellAmount)) * 100  ";
            //StrSql = StrSql + " ,Allowance12Rate = (Allowance12 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
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

            ////StrSql = StrSql + " ,Allowance28Rate = (Allowance28 /(TotalSellAmount-TotalReturnSellAmount)) * 100  "  ;
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
            pg1.Value = 0; pg1.Maximum = 7;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";
            
             StrSql = "Insert into tbl_ClosePay_04_Mod select "  ;
            StrSql = StrSql + " '" + ToEndDate + "','" + FromEndDate + "','" + PayDate + "','" + PayDate2 + "',*,'',''"  ;
            StrSql = StrSql + " From tbl_ClosePay_04 "  ;

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Insert into tbl_ClosePay_04_Sell_Mod select "  ;
            StrSql = StrSql + " '" + ToEndDate + "',* From tbl_ClosePay_04_Sell";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Insert into tbl_ClosePay_04_G_Mod select ";
            //StrSql = StrSql + " '" + ToEndDate + "',* From tbl_ClosePay_04_G";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Delete From tbl_ClosePay_44_Mod  ";
            //StrSql = StrSql + " Where ToEndDate = '" + ToEndDate + "'";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Insert into tbl_ClosePay_44_Mod select ";
            //StrSql = StrSql + " ToEndDate, FromEndDate, PayDate, Mbid,Mbid2, M_Name , GiBu_ , Us_Num , Etc_Pay  ,Sum_Gibu  ";
            //StrSql = StrSql + " ,Allowance1,Allowance2, Allowance3, Allowance4, Allowance5 ";
            //StrSql = StrSql + " ,Allowance6,Allowance7, Allowance8, Allowance9, Allowance10 ";
            //StrSql = StrSql + " ,SumAllAllowance,InComeTax, ResidentTax, TruePayment ";
            //StrSql = StrSql + " From tbl_ClosePay_04_Mod ";
            //StrSql = StrSql + " Where ToEndDate = '" + ToEndDate + "'";
            //StrSql = StrSql + " And  SumAllAllowance > 0 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


        }



        private void ReadyNewForCheckRequirement1(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Value = 0; pg1.Maximum = 4;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";


            //StrSql = "Update tbl_Sham_Grade SET ";
            //StrSql = StrSql + " Ap_Date= '" + ToEndDate + "'"; 
            // StrSql = StrSql + " FROM  tbl_Sham_Grade  A, ";

            //StrSql = StrSql + " (Select Mbid,Mbid2 ";
            //StrSql = StrSql + " From tbl_ClosePay_04 ";
            //StrSql = StrSql + " Where CurGrade > ShamGrade";
            //StrSql = StrSql + " And   ShamGrade > 0 ";
            //StrSql = StrSql + " ) B";

            //StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            //StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            //StrSql = StrSql + " And   A.Ap_Date = ''";
            //StrSql = StrSql + " And   A.Apply_Date <='" + ToEndDate + "'";
            
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update Tbl_Memberinfo SET ";
            //StrSql = StrSql + " CurGrade = 0 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update Tbl_Memberinfo SET ";
            //StrSql = StrSql + " CurGrade=ISNULL(B.OneGrade,0) ";
            //StrSql = StrSql + " FROM  Tbl_Memberinfo  A, ";

            //StrSql = StrSql + " (Select Mbid,Mbid2,OneGrade ";
            //StrSql = StrSql + " From tbl_ClosePay_04 ";
            //StrSql = StrSql + " ) B";

            //StrSql = StrSql + " Where A.Mbid  = B.Mbid ";
            //StrSql = StrSql + " And   A.Mbid2 = B.Mbid2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();





            StrSql = "Update tbl_ClosePay_04 set " ;
            StrSql = StrSql + " DayPrice01 =0, DayPrice02 =0 , DayPrice03 = 0, " ;
            StrSql = StrSql + " DayPv01 =0, DayPv02 =0 , DayPv03 = 0, " ;
            StrSql = StrSql + " DayCv01 =0, DayCv02 =0 , DayCv03 = 0, " ;
    
            StrSql = StrSql + " SellPrice01 =0, SellPrice02 =0 , SellPrice03 = 0, " ;
            StrSql = StrSql + " SellPv01 =0, SellPv02 =0 , SellPv03 = 0, " ;
            StrSql = StrSql + " SellCv01 =0, SellCv02 =0 , SellCv03 = 0,  " ;
    
            StrSql = StrSql + " DaySham01 =0, SellSham01 =0 , " ;
    
            StrSql = StrSql + " LeaveDate = '',BankCode='',BankAcc='',Cpno='',BankOwner='',RegTime='',  BusCode = '' , StopDate = '', Sell_Mem_TF = 0 , " ;
            StrSql = StrSql + " ReqTF1 = 0, ReqTF2 = 0, ReqTF4 = 0 , ";
            
            StrSql = StrSql + " Saveid='',Saveid2=0,LineCnt=0,LevelCnt=0," ;
            StrSql = StrSql + " Nominid='',Nominid2=0,N_LineCnt=0 ,  ";

            StrSql = StrSql + "  CurGrade = 0 , OneGrade = 0 , Allowance1_Sum_02 = 0 , ";

            StrSql = StrSql + "  Be_Down_PV_1 = 0 , Be_Down_PV_2 = 0 , Cur_Down_PV_1 = 0 , Cur_Down_PV_2 = 0 ,  "; 

        
            //StrSql = StrSql + " Day_Sum_PV = 0 ,G_Cur_PV = 0, High_PV = 0 , Non_High_PV = 0 , Pa_Down_Cnt = 0 , Day_Sum_PV_30 = 0 , ";                        
            //StrSql = StrSql + " Allowance2_Cut=0,Allowance3_Cut=0 , Allowance4_Cut=0 , Allowance5_Cut=0, Allowance6_Cut=0,";


            StrSql = StrSql + " Sum_Return_Take_Pay = 0 , Sum_Return_DedCut_Pay = 0 , Sum_Return_Remain_Pay = 0 , Cur_DedCut_Pay = 0 ,   ";
            //StrSql = StrSql + "  SumAllAllowance_10000 = 0 , Allowance1_Sum_02 = 0 ,";

            StrSql = StrSql + "  Etc_Pay = 0 , ";
            StrSql = StrSql + " Allowance1=0,Allowance2=0 , Allowance3=0 , Allowance4=0, Allowance5=0," ;
            StrSql = StrSql + " Allowance6=0,Allowance7=0,  Allowance8=0 , Allowance9=0, Allowance10=0," ;

            //StrSql = StrSql + " Allowance11=0,Allowance12=0 , Allowance13=0 , Allowance14=0, Allowance15=0,";
    
            StrSql = StrSql + " SumAllAllowance=0," ;
            StrSql = StrSql + " InComeTax=0, ResidentTax=0,TruePayment=0 " ;

            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04_Sell set " ;
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



            ////StrSql = "Update tbl_ClosePay_04_G set ";
            ////StrSql = StrSql + " Be_Cut_TF = Cut_TF , Cut_TF = 0 , Be_Cnt = Cur_Cnt ,Cur_Cnt = 0 ";
            
            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            

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


                          
        private void Return_Close_Retry_DayC(ref int ReturnClose_FLAG)
        {

            string Min_SellDate_2 = "", Min_ToEndDate = "", Min_FromEndDate = "", Retry_ToEndDate = "", Retry_FromEndDate = "", Retry_PayDate = "";
            int Clse_Cnt = 0;

            Min_SellDate_2 = "";
            string StrSql = "Select isnull(MIN(SellDate_2),'') From menatech.dbo.tbl_SalesDetail (nolock)";
            StrSql = StrSql + " Where Ordernumber in ( Select  Re_BaseOrdernumber  From menatech.dbo.tbl_SalesDetail(nolock) ";            
            StrSql = StrSql + "                            Where  (ReturnTF = 2 Or ReturnTF = 3 )  ";
            StrSql = StrSql + "                         )";            
            StrSql = StrSql + " And   ReturnTF = 1  And   Ga_Order = 0 ";
            StrSql = StrSql + " And   Ordernumber not in (select Re_baseOrdernumber from menatech.dbo.tbl_SalesDetail (nolock) where Ordernumber in( Select Re_Ordernumber From menatech.dbo.tbl_ClosePay_04_Re_Ord (nolock)))"; // ''이미 마감 처리된 반품을 포함하지 마라.
                       
            int tReCnt = 0;
            DataSet Dset4 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
            tReCnt = Search_Connect.DataSet_ReCount;            

            if (tReCnt > 0 ) 
                Min_SellDate_2 = Dset4.Tables[base_db_name].Rows[0][0].ToString();



            StrSql = " Select";
            StrSql = StrSql + " ToEndDate, FromEndDate";
            StrSql = StrSql + " From menatech.dbo.tbl_CloseTotal_04 (nolock)";
            StrSql = StrSql + " Where FromEndDate  >= '" + Min_SellDate_2 + "'";

            DataSet Dset5 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset5);
            tReCnt = Search_Connect.DataSet_ReCount;

            if (tReCnt > 0)
                Clse_Cnt = tReCnt;   // int.Parse (Dset5.Tables[base_db_name].Rows[0][0].ToString());

            Clse_Cnt = Clse_Cnt * 4;


            MessageBox.Show("월안에 반품 내역이 존재 합니다. 반품 관련 정산이 실행 됩니다. 약 " + Clse_Cnt + "분 정도의 시간이 소요 됩니다.");
                       



            ReturnClose_FLAG = 0; 
            cls_Connect_DB Temp_Re_Connect = new cls_Connect_DB();
            Temp_Re_Connect.Connect_Return_DB();
            SqlConnection Re_Conn = Temp_Re_Connect.Conn_Conn_Return();
            SqlTransaction Re_tran = Re_Conn.BeginTransaction();


            if (cls_User.SuperUserID == cls_User.gid)
            {
                Check_Return_Week(Temp_Re_Connect, Re_Conn, Re_tran,ref Min_ToEndDate, ref Min_FromEndDate);  //''''반품 처리 DB상에 기본 셋팅을 한다 마감취소 까지 작업을 한다.

                Check_Return_Week_Real(Temp_Re_Connect, Re_Conn, Re_tran, Min_ToEndDate, Min_FromEndDate);  //''''반품 처리 DB상에 기본 셋팅을 한다 마감취소 까지 작업을 한다.

                //Close_Work_Real_Return(Temp_Re_Connect, Re_Conn, Re_tran);
                Re_tran.Commit();
                ReturnClose_FLAG = 1;
                Re_tran.Dispose();
                Temp_Re_Connect.Close_Return_DB();               
            }
            else
            {
                try
                {

                    Check_Return_Week(Temp_Re_Connect, Re_Conn, Re_tran, ref Min_ToEndDate, ref Min_FromEndDate);  //''''반품 처리 DB상에 기본 셋팅을 한다 마감취소 까지 작업을 한다.

                    Check_Return_Week_Real(Temp_Re_Connect, Re_Conn, Re_tran, Min_ToEndDate, Min_FromEndDate);  //''''반품 처리 DB상에 기본 셋팅을 한다 마감취소 까지 작업을 한다.

                    //Close_Work_Real_Return(Temp_Re_Connect, Re_Conn, Re_tran);

                    Re_tran.Commit();
                    ReturnClose_FLAG = 1;
                    
                }
                catch (Exception)
                {
                    Re_tran.Rollback();
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_Err"));
                    
                }

                finally
                {
                    Re_tran.Dispose();
                    Temp_Re_Connect.Close_Return_DB();
                }
            }




        }


        private void Check_Return_Week(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, ref string Min_ToEndDate, ref string  Min_FromEndDate )
        {
            string StrSql = "", Min_SellDate_2 = "";



            StrSql = " EXEC Usp_Close_Pro_000_Be_Close_Base_Work '','" + ToEndDate + "',''";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
                       


            Min_SellDate_2 = "";

            StrSql = "Select isnull(MIN(SellDate_2),'') From menatech.dbo.tbl_SalesDetail (nolock)";
            StrSql = StrSql + " Where Ordernumber in ( Select  Re_BaseOrdernumber  From menatech.dbo.tbl_SalesDetail(nolock) ";
            StrSql = StrSql + "                            Where  (ReturnTF = 2 Or ReturnTF = 3 )  ";
            StrSql = StrSql + "                         )";
            StrSql = StrSql + " And   ReturnTF = 1  And   Ga_Order = 0 ";
            StrSql = StrSql + " And   Ordernumber not in (select Re_baseOrdernumber from menatech.dbo.tbl_SalesDetail (nolock) where Ordernumber in( Select Re_Ordernumber From menatech.dbo.tbl_ClosePay_04_Re_Ord (nolock)))"; // ''이미 마감 처리된 반품을 포함하지 마라.


            int tReCnt = 0;
            DataSet Dset4 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset4);
            tReCnt = Search_Connect.DataSet_ReCount;

            if (tReCnt > 0)
                Min_SellDate_2 = Dset4.Tables[base_db_name].Rows[0][0].ToString();
            
                       
            StrSql = " Select";
            StrSql = StrSql + " ToEndDate, FromEndDate";
            StrSql = StrSql + " From menatech.dbo.tbl_CloseTotal_04(nolock)";
            StrSql = StrSql + " Where FromEndDate  <= '" + Min_SellDate_2 + "'";
            StrSql = StrSql + " And  ToEndDate >= '" + Min_SellDate_2 + "'";

            DataSet Dset5 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset5);
            tReCnt = Search_Connect.DataSet_ReCount;


            if (tReCnt > 0)
            {
                Min_ToEndDate = Dset5.Tables[base_db_name].Rows[0][0].ToString();
                Min_FromEndDate = Dset5.Tables[base_db_name].Rows[0][1].ToString();
            }


            StrSql = " EXEC Usp_Close_Pro_001_Be_Close_Base_Work '" + ToEndDate + "','" + Min_ToEndDate + "','" + Min_FromEndDate  + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


                       
            StrSql = "Select FromEndDate,ToEndDate  From menatech.dbo.tbl_CloseTotal_04 (nolock)";
            StrSql = StrSql + "  Where   ToEndDate >= '" + Min_ToEndDate + "'";
            StrSql = StrSql + "  And     ToEndDate <= '" + ToEndDate + "'";
            StrSql = StrSql + "  Order by ToEndDate DESC ";

            DataSet Dset6 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset6);
            tReCnt = Search_Connect.DataSet_ReCount;



            for (int fi_cnt = 0; fi_cnt <= tReCnt - 1; fi_cnt++)
            {
                string Rs_FromEndDate = Dset6.Tables[base_db_name].Rows[fi_cnt]["FromEndDate"].ToString(); ;
                string Rs_ToEndDate = Dset6.Tables[base_db_name].Rows[fi_cnt]["ToEndDate"].ToString();  ;

                StrSql = " EXEC Usp_Close_Pro_100_Be_Close_Cancel '" + Rs_FromEndDate + "','" + Rs_ToEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }


            //마감취소에서 마감을 돌릴 대기상태의 계산 테이블의 전 직급이 지금의 직급보다 크다...
            // 그럼 전 직급을 현재 월마감 계산테이블의 전직급으로 변경을 한다.
            StrSql = " EXEC Usp_Close_Pro_002_Be_Close_Base_Work '" + ToEndDate + "','" + Min_ToEndDate + "','" + Min_FromEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);



        }



        private void Check_Return_Week_Real(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string Min_ToEndDate, string Min_FromEndDate)
        {
            string StrSql = "Select FromEndDate,ToEndDate ,PayDate From menatech.dbo.tbl_CloseTotal_04 (nolock)";
            StrSql = StrSql + "  Where   ToEndDate >= '" + Min_FromEndDate + "'";
            StrSql = StrSql + "  And     ToEndDate <= '" + ToEndDate + "'";
            StrSql = StrSql + "  Order by ToEndDate ASC  ";

            DataSet Dset6 = new DataSet();
            Search_Connect.Open_Data_Set(StrSql, base_db_name, Search_Conn, Dset6);
            int tReCnt = Search_Connect.DataSet_ReCount;

            for (int fi_cnt = 0; fi_cnt <= tReCnt - 1; fi_cnt++)
            {

                string Retry_ToEndDate = Dset6.Tables[base_db_name].Rows[fi_cnt]["ToEndDate"].ToString();
                string Retry_FromEndDate = Dset6.Tables[base_db_name].Rows[fi_cnt]["FromEndDate"].ToString();
                string Retry_PayDate = Dset6.Tables[base_db_name].Rows[fi_cnt]["PayDate"].ToString();

                Close_Work_Real_Return(Temp_Connect, Conn, tran,   Retry_ToEndDate, Retry_FromEndDate, Retry_PayDate); 


            }


            StrSql = " EXEC Usp_Close4_Pro_280_Return_Pay_Put '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

        }



        private void Close_Work_Real_Return(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran
                                        , string Retry_ToEndDate, string Retry_FromEndDate, string  Retry_PayDate)
        {
            pg2.Minimum = 0; pg2.Maximum = 49;
            pg2.Step = 1; pg2.Value = 0;
            pg1.Step = 1;

            string StrSql = "";
                        

            //마감돌리는 동안 매출 등록을 못하도록 하기 위해서 제일 먼저 체크 테이블인 집계 테이블을 만든다.
            StrSql = " EXEC Usp_Close4_Pro_251_A_Put_tbl_CloseTotal_Put1 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + Retry_PayDate + "','" + cls_User.gid + "','" + ToEndDate  + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();



            StrSql = "Update tbl_CloseTotal_04 SET ";
            StrSql = StrSql + "  Temp01 = " + double.Parse(txtB1.Text);
            StrSql = StrSql + " , Temp02 = " + double.Parse(txtB2.Text);
            StrSql = StrSql + " ,Temp11 = 2 ";
            StrSql = StrSql + " Where ToEndDate ='" + Retry_ToEndDate + "'";
            StrSql = StrSql + " And   WeekDate = '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();



            StrSql = " EXEC Usp_Close4_Pro_100_001 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();


            StrSql = " EXEC Usp_Close4_Pro_100_A_Sell_002 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();


            StrSql = " EXEC Usp_Close4_Pro_100_A_Sell_003 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();


            //기존 원마감상에서 가져오면 되기 때문에 별도 계산을 할 필요가 없다.
            //StrSql = " EXEC Usp_Close4_Pro_100_B_LevelCnt '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg2.PerformStep(); pg2.Refresh();
            //기존 원마감상에서 가져오면 되기 때문에 별도 계산을 할 필요가 없다.


            StrSql = " EXEC Usp_Close4_Pro_100_B_ReqTF1 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            if (int.Parse (Retry_FromEndDate) >= 20200101)
                StrSql = " EXEC Usp_Close4_Pro_100_C_Put_Down_PV_01_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + ToEndDate + "'";
            else
                StrSql = " EXEC Usp_Close4_Pro_100_C_Put_Down_PV_01 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC Usp_Close4_Pro_100_C_Put_Down_PV_01_BusCode '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();



            StrSql = " EXEC Usp_Close4_Pro_100_D_Put_Down_PV_01_Nom '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();


            //반품에 대해서 추가가됨 2019-12-12
            StrSql = " EXEC Usp_Close4_Pro_100_C_Put_Down_PV_Re_01 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            if (int.Parse(Retry_FromEndDate) >= 20200401)
            {
                StrSql = " EXEC Usp_Close4_Pro_100_C_Put_Down_PV_Re_01_Ver_02  '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }

            StrSql = " EXEC Usp_Close4_Pro_100_D_Put_Down_PV_Re_01_Nom '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //반품에 대해서 추가가됨 2019-12-12




            StrSql = " EXEC Usp_Close4_Pro_100_B_ReqTF2 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC Usp_Close4_Pro_100_C_CurGrade_OrgGrade_Put '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();



            if (int.Parse(Retry_FromEndDate) >= 20200101)
            {
                if (int.Parse(Retry_FromEndDate) >= 20200401)
                {
                    StrSql = " EXEC dbo.Usp_Close4_Pro_220_A_Give_OneGrade_Ver03 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + ToEndDate + "'";
                }
                else
                {
                    StrSql = " EXEC dbo.Usp_Close4_Pro_220_A_Give_OneGrade_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + ToEndDate + "'";
                }
            }
            else
                StrSql = " EXEC dbo.Usp_Close4_Pro_220_A_Give_OneGrade '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);




        Re_Grade_10:
            StrSql = " EXEC dbo.Usp_Close4_Pro_200_A_GiveGrade_10 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            if (Check_UP_Grade_TF(10, Temp_Connect, Conn, tran) == true) goto Re_Grade_10;
            pg2.PerformStep(); pg2.Refresh();

        Re_Grade_20:
            StrSql = " EXEC dbo.Usp_Close4_Pro_200_A_GiveGrade_20 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            if (Check_UP_Grade_TF(20, Temp_Connect, Conn, tran) == true) goto Re_Grade_20;
            pg2.PerformStep(); pg2.Refresh();

        Re_Grade_30:
            StrSql = " EXEC dbo.Usp_Close4_Pro_200_A_GiveGrade_30 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            if (Check_UP_Grade_TF(30, Temp_Connect, Conn, tran) == true) goto Re_Grade_30;
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_200_A_GiveGrade_40 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_200_A_GiveGrade_50 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_200_A_GiveGrade_60 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC dbo.Usp_Close4_Pro_200_A_GiveGrade_70 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            if (int.Parse(Retry_FromEndDate) >= 20200101)
            {
                StrSql = " EXEC dbo.Usp_Close4_Pro_200_A_GiveGrade_80_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_200_A_GiveGrade_90_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_210_A_GiveGrade_100_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_210_A_GiveGrade_110_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_210_A_GiveGrade_120_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_210_A_GiveGrade_130_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_210_A_GiveGrade_140_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_210_A_GiveGrade_150_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();
            }
            else
            {
                StrSql = " EXEC dbo.Usp_Close4_Pro_200_A_GiveGrade_80 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_200_A_GiveGrade_90 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_210_A_GiveGrade_100 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_210_A_GiveGrade_110 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_210_A_GiveGrade_120 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_210_A_GiveGrade_130 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_210_A_GiveGrade_140 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();

                StrSql = " EXEC dbo.Usp_Close4_Pro_210_A_GiveGrade_150 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                pg2.PerformStep(); pg2.Refresh();
            }








            if (int.Parse(Retry_FromEndDate) >= 20200101)
            {
                if (int.Parse(Retry_FromEndDate) >= 20200401)
                {
                    StrSql = " EXEC Usp_Close4_Pro_230_A_Give_Allowance1_Ver03 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'"; //직급달성 보너스 6                    
                }
                else
                {
                    StrSql = " EXEC Usp_Close4_Pro_230_A_Give_Allowance1_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'"; //직급달성 보너스 6
                }
            }
            else
                StrSql = " EXEC Usp_Close4_Pro_230_A_Give_Allowance1 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'"; //직급달성 보너스 6
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC Usp_Close4_Pro_230_A_Give_Allowance3 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + ToEndDate + "'";  //직급달성 보너스 6
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC Usp_Close4_Pro_230_A_Give_Allowance5 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'"; //직급달성 보너스 6
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC Usp_Close4_Pro_230_A_Give_Allowance6 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + ToEndDate + "'";  //직급달성 보너스 6
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            if (int.Parse(Retry_FromEndDate) >= 20200101)
            {
                StrSql = " EXEC Usp_Close4_Pro_230_A_Give_Allowance7_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'"; //직급달성 보너스 6
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }


            if (int.Parse(Retry_FromEndDate) >= 20200401)
            {
                StrSql = " EXEC Usp_Close4_Pro_230_A_Give_Allowance8_Ver03 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'"; //직급달성 보너스 6
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }
            pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------



            ////이부분은 가마감쪽에는 없음.. 시간이 오래걸리는 프로세스 이기때문에.. 소스상에서만 처리하기로함.
            ////반픔관련 프로세스가 들어간다. //올스타팩보너스 환수를 위해서 직급을 다시 계산하기 위함.
            ////반픔관련 프로세스가 들어간다. //올스타팩보너스 환수를 위해서 직급을 다시 계산하기 위함.
            //Retry_ToEndDate(Temp_Connect, Conn, tran);
            ////반픔관련 프로세스가 들어간다. //올스타팩보너스 환수를 위해서 직급을 다시 계산하기 위함.
            ////반픔관련 프로세스가 들어간다. //올스타팩보너스 환수를 위해서 직급을 다시 계산하기 위함.


            /*
            StrSql = " EXEC Usp_Close4_Pro_235_A_Put_Sum_Return_Remain_Pay_Pre '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC Usp_Close4_Pro_240_A_Put_Sum_Return_Remain_Pay '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            ////--------------------------------------------------------------


            //--------------------------------------------------------------
            StrSql = " EXEC Usp_Close4_Pro_240_B_CalculateTruePayment '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "', 1 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();



            StrSql = " EXEC Usp_Close4_Pro_240_C_Chang_RetunPay_Table '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------
            //--------------------------------------------------------------
            */

            StrSql = " EXEC Usp_Close4_Pro_251_A_Put_tbl_CloseTotal_Put1 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + Retry_PayDate + "','" + cls_User.gid + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = "Update tbl_CloseTotal_04 SET ";
            StrSql = StrSql + "  Temp01 = " + double.Parse(txtB1.Text);
            StrSql = StrSql + " ,Temp11 = 2 ";
            StrSql = StrSql + " Where ToEndDate ='" + Retry_ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();


            StrSql = " EXEC Usp_Close4_Pro_251_B_Put_tbl_CloseTotal_Put2 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "' ,'" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();


            StrSql = " EXEC Usp_Close4_Pro_251_C_Put_tbl_CloseTotal_Put3 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------




            //--------------------------------------------------------------
            if (int.Parse(Retry_FromEndDate) >= 20200101)
                StrSql = " EXEC Usp_Close4_Pro_260_A_MakeModForCheckRequirement1_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + Retry_PayDate + "','" + ToEndDate + "'";
            else
                StrSql = " EXEC Usp_Close4_Pro_260_A_MakeModForCheckRequirement1 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + Retry_PayDate + "','" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            if (int.Parse(Retry_FromEndDate) >= 20200101)
                StrSql = " EXEC Usp_Close4_Pro_260_B_ReadyNewForCheckRequirement1_Ver02 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + Retry_PayDate + "' ";
            else
                StrSql = " EXEC Usp_Close4_Pro_260_B_ReadyNewForCheckRequirement1 '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + Retry_PayDate + "' ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();

            StrSql = " EXEC Usp_Close4_Pro_270_Check_Close_Gid '" + Retry_FromEndDate + "','" + Retry_ToEndDate + "','" + cls_User.gid + "' ,'" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();
            //--------------------------------------------------------------


            //진마감이 다 돌았음을 알린다... 가마감 돌아도 되도록 체크를 한다.
            StrSql = " UpDate tbl_CloseTotal_04 SET  Real_FLAG  = 0 Where ToEndDate = '" + Retry_ToEndDate + "' And WeekDate = '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg2.PerformStep(); pg2.Refresh();




        }



















    }
}
