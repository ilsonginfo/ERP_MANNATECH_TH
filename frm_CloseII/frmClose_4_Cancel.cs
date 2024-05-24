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
    public partial class frmClose_4_Cancel : clsForm_Extends
    {
        



        private string base_db_name = "tbl_CloseTotal_04";
        private string FromEndDate = ""; private string ToEndDate = ""; private string PayDate = "", PayDate2 = "";
        private int From_Load_TF = 0;
        private int  ReCnt = 0;


        public frmClose_4_Cancel()
        {
            InitializeComponent();
        }


        private void butt_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            From_Load_TF = 0;
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Pay);
            cfm.button_flat_change(butt_Exit);

            txt_From.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_To.BackColor = cls_app_static_var.txt_Enable_Color;
            txtPayDate.BackColor = cls_app_static_var.txt_Enable_Color;

            FromEndDate = ""; ToEndDate = ""; PayDate = "";            
        }
        
        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (From_Load_TF == 0)
            {
                From_Load_TF = 1;

                Check_Close_Date();

                if (FromEndDate == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not3_Close_Date"));
                    this.Close();
                    return;
                }



                if (int.Parse (FromEndDate) < 20200801 )
                {
                    MessageBox.Show("현 프로그램상에서 취소가 불가능한 마감 일자 입니다. 2020년 7월 이전 마감");
                    this.Close();
                    return;
                }

                /*
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                string Clo_TF = "", RS_ToEndDate = "";
                int Cl_TF = 0;
                string StrSql = "Select T_index, Cl_TF , ToEndDate ";
                StrSql = StrSql + " From tbl_Sales_Put_Return_Pay  ";
                StrSql = StrSql + " WHERE Base_T_index  IN (Select T_index From tbl_Sales_Put_Return_Pay Where ToEndDate = '" + ToEndDate + "' And  Cl_TF = 4 )";
                StrSql = StrSql + " And Base_T_index > 0 ";
                StrSql = StrSql + " And Cl_TF <> 4 ";

                //++++++++++++++++++++++++++++++++                
                DataSet ds_2 = new DataSet();

                Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds_2, this.Name, this.Text);
                int ReCnt_2 = Temp_Connect.DataSet_ReCount;

                if (ReCnt_2 > 0)
                {
                    Cl_TF = int.Parse(ds_2.Tables[base_db_name].Rows[0]["Cl_TF"].ToString());

                    if (Cl_TF == 2) Clo_TF = "주간정산";
                    if (Cl_TF == 4) Clo_TF = "월정산";

                    MessageBox.Show("현 마감에서 발생한 반품 차감의 영향을 받은 " + RS_ToEndDate + " 마감일자의 " + Clo_TF + "이 존재합니다." + "\n" + "영향을 받은 마감을 먼저 취소 하십시요.");
                    this.Close();
                    return;
                }

                StrSql = "Select Clo_TF , Apply_Date ";
                StrSql = StrSql + " From tbl_Sham_Pay_Real (nolock)  ";
                StrSql = StrSql + " WHERE Apply_Date = '" + ToEndDate + "'";
                StrSql = StrSql + " And Clo_TF = 4 ";

                //++++++++++++++++++++++++++++++++                
                DataSet ds_3 = new DataSet();

                Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds_3, this.Name, this.Text);
                int ReCnt_3 = Temp_Connect.DataSet_ReCount;

                if (ReCnt_3 > 0)
                {

                    MessageBox.Show("현 마감에 적용된 실시간 수당 가감 내역이 존재 합니다. "
                         + "\n" + "마감취소가 불가능 합니다."
                          + "\n" + "월 실시간 수당 가감 내역을 삭제 처리후 다시 시도해 주십시요."
                        );
                    this.Close();
                    return;
                }

                */

                //if (Close_Check_Date_02("tbl_CloseTotal_02") == false)
                //{
                ////    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Week"));
                ////    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Day"));
                ////    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Month"));
                ////    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Per"));
                //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Week"));
                //    this.Close();
                //    return;
                //}

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



        private void Check_Close_Date()
        {
            string Tsql = "";
            Tsql = "Select Top 1 FromEndDate , ToEndDate , PayDate From  tbl_CloseTotal_04 (nolock) ";
            Tsql += " Where ToEndDate >= '20180701' ";
            Tsql += " Order by  ToEndDate DESC ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            ReCnt = 0; 
            Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text);
            ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt != 0)
            {
                txt_From.Text  = ds.Tables[base_db_name].Rows[0][0].ToString();
                FromEndDate = ds.Tables[base_db_name].Rows[0][0].ToString();

                txt_To.Text = ds.Tables[base_db_name].Rows[0][1].ToString();
                ToEndDate = ds.Tables[base_db_name].Rows[0][1].ToString();

                txtPayDate.Text = ds.Tables[base_db_name].Rows[0][2].ToString();
                PayDate = ds.Tables[base_db_name].Rows[0][2].ToString();
            }           

        }

        private Boolean Close_Check_Date_02 (string table_Name) 
        {
            string StrSql = "" ;

            StrSql = "Select ToEndDate From " + table_Name + " (nolock) " ;
            StrSql = StrSql + " Where ToEndDate >= '" + ToEndDate + "'";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            ReCnt = 0;
            Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text);
            ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {                
                return false; 
            }

            return true;
        }

        private void butt_Pay_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_Start"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            pg1.Visible = true; pg1.Maximum = 0;
            
            butt_Pay.Enabled = false; butt_Exit.Enabled = false;
            
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
                    Close_Sucess_TF = 1;
                }

                finally
                {
                    tran.Dispose(); Temp_Connect.Close_DB();
                    Close_End(Close_Sucess_TF);
                }
            }     

        }

        private void Close_End(int Close_Sucess_TF)
        {
            if (Close_Sucess_TF == 1)
            {
                this.Close();
                return;
            }

            pg1.Visible = false; pg1.Maximum = 10;
            
            this.Cursor = System.Windows.Forms.Cursors.Default;
            butt_Pay.Enabled = true; butt_Exit.Enabled = true;
            
            FromEndDate = ""; ToEndDate = ""; PayDate = "";
            txtPayDate.Text = ""; txt_To.Text = ""; txt_From.Text = "";
            
            Check_Close_Date();

            if (FromEndDate == "")
            {
                this.Close();
                return;
            }

            if (int.Parse(FromEndDate) < 20200801)
            {
                MessageBox.Show("현 프로그램상에서 취소가 불가능한 마감 일자 입니다. 2020년 7월 이전 마감");
                this.Close();
                return;
            }


        }


        private void Close_Work_Real(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Minimum = 0; pg1.Maximum = 27;
            pg1.Step = 1; pg1.Value = 0;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_04_Sell SET " ;
            StrSql = StrSql + "  BeAmount=ISNULL(B.BeAmount,0) ";
            StrSql = StrSql + " ,BeCash=ISNULL(B.BeCash,0) ";
            StrSql = StrSql + " ,BeCard=ISNULL(B.BeCard,0) ";
            StrSql = StrSql + " ,BeBank=ISNULL(B.BeBank,0) ";
            StrSql = StrSql + " ,BeTotalPV=ISNULL(B.BeTotalPV,0) ";
            StrSql = StrSql + " ,BeShamSell=ISNULL(B.BeShamSell,0) ";
           // StrSql = StrSql + " ,BeShamSell_Pr=ISNULL(B.BeShamSell_Pr,0) ";
            StrSql = StrSql + " ,BeReAmount=ISNULL(B.BeReAmount,0) ";
            StrSql = StrSql + " ,BeReCash=ISNULL(B.BeReCash,0) ";
            StrSql = StrSql + " ,BeReCard=ISNULL(B.BeReCard,0) ";
            StrSql = StrSql + " ,BeReBank=ISNULL(B.BeReBank,0) ";
            StrSql = StrSql + " ,BeReTotalPV=ISNULL(B.BeReTotalPV,0) ";
            StrSql = StrSql + " ,BeTotalCV=ISNULL(B.BeTotalCV,0) ";
            StrSql = StrSql + " ,BeReTotalCV=ISNULL(B.BeReTotalCV,0) ";

            StrSql = StrSql + " FROM  tbl_ClosePay_04_Sell  A, ";
    
            StrSql = StrSql + " (Select Mbid,Mbid2,SellCode,BeAmount,BeCash,BeCard,BeBank,BeTotalPV,BeShamSell,";
            StrSql = StrSql + " BeReAmount,BeReCash,BeReCard,BeReBank,BeReTotalPV,";
            StrSql = StrSql + " BeTotalCV,BeReTotalCV ";
            StrSql = StrSql + " From tbl_ClosePay_04_Sell_Mod (nolock) ";
            StrSql = StrSql + " Where  ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " ) B";
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            StrSql = StrSql + " And   A.SellCode=B.SellCode ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_04 SET ";          
            StrSql = StrSql + " Be_M_Dir_30_Cnt = ISNULL(B.Be_M_Dir_30_Cnt,0) ";

            StrSql = StrSql + " FROM  tbl_ClosePay_04  A, ";

            StrSql = StrSql + " (Select Mbid,Mbid2 ";
            StrSql = StrSql + " ,Be_M_Dir_30_Cnt ";             
            StrSql = StrSql + " From tbl_ClosePay_04_Mod (nolock) ";
            StrSql = StrSql + " Where  ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_04_G SET ";
            //StrSql = StrSql + " Be_Cut_TF = ISNULL(B.Be_Cut_TF,0) ";
            //StrSql = StrSql + ",Be_Cnt = ISNULL(B.Be_Cnt,0) ";
            //StrSql = StrSql + " FROM  tbl_ClosePay_04_G  A, ";
            //StrSql = StrSql + " (Select Seq ";
            //StrSql = StrSql + " ,Be_Cut_TF, Be_Cnt ";
            //StrSql = StrSql + " From tbl_ClosePay_04_G_Mod ";
            //StrSql = StrSql + " Where  ToEndDate ='" + ToEndDate + "'";
            //StrSql = StrSql + " ) B";

            //StrSql = StrSql + " Where A.Seq=B.Seq ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_04 Set CurPoint_Date_2 ='' " ;
            //StrSql = StrSql +" Where CurPoint_Date_2 >='" + FromEndDate + "'";
            //StrSql = StrSql +" And  CurPoint_Date_2 <='" + ToEndDate + "'";
            //StrSql = StrSql +" And  CurPoint_Date_2 <> ''";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_04 Set CurPoint_Date_3 ='' ";
            //StrSql = StrSql +" Where CurPoint_Date_3 >='" + FromEndDate + "'";
            //StrSql = StrSql +" And  CurPoint_Date_3 <='" + ToEndDate + "'";
            //StrSql = StrSql +" And  CurPoint_Date_3 <> ''";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //Change_Be_StringField(Temp_Connect, Conn, tran, "ReqDate1");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate1");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate2");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate3");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate4");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate5");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate6");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate7");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate8");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate9");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate10");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate11");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate12");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate13");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate14");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate15");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate16");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate17");
            Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate18");


            Change_Be_StringField(Temp_Connect, Conn, tran, "Ach_Self_Date");
            Change_Be_StringField(Temp_Connect, Conn, tran, "Ach_Date");
            Change_Be_StringField(Temp_Connect, Conn, tran, "Ach_12_Date");
            pg1.PerformStep(); pg1.Refresh();



            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate1");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate2");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate3");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate4");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate5");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate6");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate7");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate8");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate9");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate10");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate11");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate12");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate13");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate14");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate15");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate16");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate17");
            //Change_Be_StringField_Up(Temp_Connect, Conn, tran, "GradeDate18");         
            //pg1.PerformStep(); pg1.Refresh();

            //Change_Be_StringField_G(Temp_Connect, Conn, tran, "End_Date");

            StrSql = "Update tbl_ClosePay_10000 SET Ap_ToEnddate = '', Ap_TF = 0  ";
            StrSql = StrSql + " Where Ap_ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " And  Ap_TF = 4 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete tbl_ClosePay_10000  Where ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " And  ToEndDate_TF = 4 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);            
            pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update Tbl_Memberinfo SET ";
            ////StrSql = StrSql + " CurPoint=0 " ;
            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update Tbl_Memberinfo SET " ;
            ////StrSql = StrSql + " CurPoint=ISNULL(B.BePoint,0) ";
            ////StrSql = StrSql + " FROM  Tbl_Memberinfo  A, ";

            ////StrSql = StrSql + " (Select Mbid,Mbid2,BePoint ";
            ////StrSql = StrSql + " From tbl_ClosePay_04 ";
            ////StrSql = StrSql + " ) B";

            ////StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            ////StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_Sham_Grade Set ";
            StrSql = StrSql + " Ap_Date = '' ";
            StrSql = StrSql + " Where Ap_Date ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_Sales_Put_Return_Pay SET " ;
            StrSql = StrSql + " Return_Pay2 =  Return_Pay2 + Isnull(B.A1,0) ";
            StrSql = StrSql + " FROM  tbl_Sales_Put_Return_Pay  A, ";

            StrSql = StrSql + " (Select Sum(Return_Pay) A1 , Base_T_index";
            StrSql = StrSql + " From tbl_Sales_Put_Return_Pay  (nolock)  ";
            StrSql = StrSql + " WHERE ToEndDate= '" + ToEndDate + "'";
            StrSql = StrSql + " And  Base_T_index > 0 ";
            StrSql = StrSql + " And  Cl_TF = 4 ";
            StrSql = StrSql + " Group by  Base_T_index";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.T_index = B.Base_T_index ";            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            ////StrSql = "Update tbl_Sales_Put_Return_Pay_DED SET ";
            ////StrSql = StrSql + " Return_Pay2 =  Return_Pay2 + Isnull(B.A1,0) ";
            ////StrSql = StrSql + " FROM  tbl_Sales_Put_Return_Pay_DED  A, ";

            ////StrSql = StrSql + " (Select Sum(Return_Pay) A1 , Base_T_index";
            ////StrSql = StrSql + " From tbl_Sales_Put_Return_Pay_DED (nolock)  ";
            ////StrSql = StrSql + " WHERE ToEndDate= '" + ToEndDate + "'";
            ////StrSql = StrSql + " And  Base_T_index > 0 ";
            ////StrSql = StrSql + " And  Cl_TF = 4 ";
            ////StrSql = StrSql + " Group by  Base_T_index";
            ////StrSql = StrSql + " ) B";
            ////StrSql = StrSql + " Where A.T_index = B.Base_T_index ";
            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();




            StrSql = "Delete From tbl_ClosePay_04 Where RecordMakeDate = '" + ToEndDate +  "'" ;
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From tbl_ClosePay_04_sell Where RecordMakeDate = '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);            
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_ClosePay_04_Mod Where ToEndDate = '" + ToEndDate +  "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_ClosePay_04_Sell_Mod Where ToEndDate = '" + ToEndDate +  "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


           
            StrSql = "Update tbl_Close_Not_Pay SET Ap_ToEnddate = ''  ";
            StrSql = StrSql + " Where Ap_ToEndDate = '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Delete From tbl_Close_Not_Pay  Where  ToEndDate  = '" + ToEndDate + "'";            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            
            StrSql = "Delete From tbl_Close_DownPV_ALL_04 Where EndDate = '" + ToEndDate +  "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_Close_DownPV_PV_04 Where EndDate = '" + ToEndDate +  "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_ClosePay_04_Up Where M_ToEndDate = '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_ClosePay_04_Up_Mod Where ToEndDate = '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_Sales_Put_Return_Pay Where ToEndDate = '" + ToEndDate +  "' And  Cl_TF = 4";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Delete From tbl_Sales_Put_Return_Pay_DED Where ToEndDate = '" + ToEndDate + "' And  Cl_TF = 4";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);            


            StrSql = "Delete From tbl_CloseTotal_04 Where ToEndDate = '" + ToEndDate +  "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update Tbl_Memberinfo SET ";
            StrSql = StrSql + " CurGrade = 0 , Max_CurGrade = 0   ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update Tbl_Memberinfo SET ";
            StrSql = StrSql + " CurGrade       =  ISNULL(B.OneGrade,0) ";
            StrSql = StrSql + " ,Max_CurGrade  =  ISNULL(B.CurGrade,0) ";
            //'StrSql = StrSql + " ,Achiever_FLAG =  ISNULL(B.Ar_3,0)    + ISNULL(B.Ar_12,0)     ";
            StrSql = StrSql + " FROM  Tbl_Memberinfo  A, ";

            StrSql = StrSql + " (Select Mbid,Mbid2,OneGrade ,  CurGrade ";
            //StrSql = StrSql + " , Case When  Ach_Date <> '' then 1 else 0 End Ar_3 ";
            //StrSql = StrSql + " , Case When  Ach_12_Date <> '' then 1 else 0 End Ar_12 ";
            StrSql = StrSql + " From tbl_ClosePay_04_Mod (nolock)  ";
            StrSql = StrSql + " Where ToEndDate in (Select Isnull(Max(ToEndDate),'') From tbl_CloseTotal_04 (nolock) )";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            Check_Close_Gid(Temp_Connect, Conn, tran, 4, 1);
            pg1.PerformStep(); pg1.Refresh();

        }



        private void Check_Close_Gid(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, int Close_Sort, int Close_Cancel_TF)
        {
            pg1.Value = 0; pg1.Maximum = 2;
            pg1.PerformStep(); pg1.Refresh();
            string StrSql = "";

            StrSql = "Insert Into tbl_Close_Log Values (" + Close_Sort + ",'" + FromEndDate + "','" + ToEndDate + " ', " + Close_Cancel_TF + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) )";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();
        }


        private void Change_Be_StringField(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string ChangeField)
        {
            string StrSql = "";
            StrSql = "Update tbl_ClosePay_04 Set ";
            StrSql = StrSql + ChangeField + " = '' ";
            StrSql = StrSql + " Where " + ChangeField + "='" + ToEndDate + "'";

            //if (ChangeField.Length >= 9 && ChangeField.Substring(0,9) == "GradeDate")
            //    StrSql = StrSql + " Where LEFT(" + ChangeField + ",6) = '" + ToEndDate.Substring (0,6) + "'";
            //else


            Temp_Connect.Insert_Data(StrSql, Conn, tran);
        }


        private void Change_Be_StringField_Up(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string ChangeField)
        {
            string StrSql = "";
            StrSql = "Update tbl_ClosePay_04_Up Set ";
            StrSql = StrSql + ChangeField + " = '' ";
            StrSql = StrSql + " Where " + ChangeField + "='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
        }




        private void Change_Be_StringField_G(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string ChangeField)
        {
            string StrSql = "";
            StrSql = "Update tbl_ClosePay_04_G Set ";
            StrSql = StrSql + ChangeField + " = '' ";
            StrSql = StrSql + " Where " + ChangeField + "='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
        }


    }
}
