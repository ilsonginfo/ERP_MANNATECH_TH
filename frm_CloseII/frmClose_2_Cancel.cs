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
    public partial class frmClose_2_Cancel : Form
    {
        

        private string base_db_name = "tbl_CloseTotal_02";
        private string FromEndDate = ""; private string ToEndDate = ""; private string PayDate = "", PayDate2 = "";
        private int From_Load_TF = 0;
        private int  ReCnt = 0;


        public frmClose_2_Cancel()
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

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                string Clo_TF = "", RS_ToEndDate = "";
                int Cl_TF = 0;


                string StrSql = "Select T_index, Cl_TF , ToEndDate ";
                StrSql = StrSql + " From tbl_Sales_Put_Return_Pay (nolock)   ";
                StrSql = StrSql + " WHERE Base_T_index  IN (Select T_index From tbl_Sales_Put_Return_Pay Where ToEndDate = '" + ToEndDate + "' And  Cl_TF = 2 )";
                StrSql = StrSql + " And Base_T_index > 0 ";
                StrSql = StrSql + " And Cl_TF <> 2 ";

                //++++++++++++++++++++++++++++++++                
                DataSet ds_2 = new DataSet();

                Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds_2, this.Name, this.Text);
                int ReCnt_2 = Temp_Connect.DataSet_ReCount;

                if (ReCnt_2 > 0)
                {
                    Cl_TF = int.Parse(ds_2.Tables[base_db_name].Rows[0]["Cl_TF"].ToString());
                    RS_ToEndDate = ds_2.Tables[base_db_name].Rows[0]["ToEndDate"].ToString() ;

                    if (Cl_TF == 2000) Clo_TF = "반품공제차감 내역";                    

                    MessageBox.Show("현 마감에서 발생한 반품 차감의 영향을 받은 " + RS_ToEndDate + " 일자의 " + Clo_TF + "이 존재합니다."
                         + "\n" + "마감취소가 불가능 합니다."
                          + "\n" + " 입력하신 반품공제차감 내역을 삭제후 다시 시도해 주십시요."
                        );
                    this.Close();
                    return;
                }



                StrSql = "Select Clo_TF , Apply_Date ";
                StrSql = StrSql + " From tbl_Sham_Pay_Real (nolock)  ";
                StrSql = StrSql + " WHERE Apply_Date = '" + ToEndDate + "'";
                StrSql = StrSql + " And Clo_TF = 2 ";                

                //++++++++++++++++++++++++++++++++                
                DataSet ds_3 = new DataSet();

                Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds_3, this.Name, this.Text);
                int ReCnt_3 = Temp_Connect.DataSet_ReCount;

                if (ReCnt_3 > 0)
                {                   

                    MessageBox.Show("현 마감에 적용된 실시간 수당 가감 내역이 존재 합니다. "
                         + "\n" + "마감취소가 불가능 합니다."
                          + "\n" + "주간 실시간 수당 가감 내역을 삭제 처리후 다시 시도해 주십시요."
                        );
                    this.Close();
                    return;
                }


                


                if (Close_Check_Date_02("tbl_CloseTotal_04") == false)
                {
                    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Week"));
                    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Day"));
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Month"));
                    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Per"));
                    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Week"));
                    this.Close();
                    return;
                }
                             
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
            Tsql = "Select Top 1 FromEndDate , ToEndDate , PayDate From  tbl_CloseTotal_02 (nolock) Order by  ToEndDate DESC ";

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
        }


        private void Close_Work_Real(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Minimum = 0; pg1.Maximum = 25;
            pg1.Step = 1; pg1.Value = 0;
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_02_Sell SET " ;
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

            StrSql = StrSql + " FROM  tbl_ClosePay_02_Sell  A, ";
    
            StrSql = StrSql + " (Select Mbid,Mbid2,SellCode,BeAmount,BeCash,BeCard,BeBank,BeTotalPV,BeShamSell,";
            StrSql = StrSql + " BeReAmount,BeReCash,BeReCard,BeReBank,BeReTotalPV,";
            StrSql = StrSql + " BeTotalCV,BeReTotalCV ";
            StrSql = StrSql + " From tbl_ClosePay_02_Sell_Mod ";
            StrSql = StrSql + " Where  ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " ) B";
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            StrSql = StrSql + " And   A.SellCode=B.SellCode ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_ClosePay_02 SET ";
            StrSql = StrSql + " Be_PV_1 = ISNULL(B.Be_PV_1,0) ";
            StrSql = StrSql + ",Be_PV_2 = ISNULL(B.Be_PV_2,0) ";
            //StrSql = StrSql + ",BeforeGrade = ISNULL(B.BeforeGrade,0) ";
            //StrSql = StrSql + ",Reg_Cnt_Be = ISNULL(B.Reg_Cnt_Be,0) ";
            
            //StrSql = StrSql + ",Be_GradeDate1 = ISNULL(B.Be_GradeDate1,'') ";
            //StrSql = StrSql + ",Be_GradeDate2 = ISNULL(B.Be_GradeDate2,'') ";
            //StrSql = StrSql + ",Be_GradeDate3 = ISNULL(B.Be_GradeDate3,'') ";
            //StrSql = StrSql + ",Be_GradeDate4 = ISNULL(B.Be_GradeDate4,'') ";
            //StrSql = StrSql + ",Be_GradeDate5 = ISNULL(B.Be_GradeDate5,'') ";
            //StrSql = StrSql + ",Be_GradeDate6 = ISNULL(B.Be_GradeDate6,'') ";
            //StrSql = StrSql + ",Be_GradeDate7 = ISNULL(B.Be_GradeDate7,'') ";
            //StrSql = StrSql + ",Be_GradeDate8 = ISNULL(B.Be_GradeDate8,'') ";
            //StrSql = StrSql + ",Be_GradeDate9 = ISNULL(B.Be_GradeDate9,'') ";
            //StrSql = StrSql + ",Be_GradeDate10 = ISNULL(B.Be_GradeDate10,'') ";
            //StrSql = StrSql + ",Be_GradeDate11 = ISNULL(B.Be_GradeDate11,'') ";

            //StrSql = StrSql + ",Be_GradeDate4_1 = ISNULL(B.Be_GradeDate4_1,'') ";
            //StrSql = StrSql + ",Be_GradeDate4_2 = ISNULL(B.Be_GradeDate4_2,'') ";
            //StrSql = StrSql + ",Be_GradeDate4_3 = ISNULL(B.Be_GradeDate4_3,'') ";
            //StrSql = StrSql + ",Be_GradeDate4_4 = ISNULL(B.Be_GradeDate4_4,'') ";
            //StrSql = StrSql + ",Be_GradeDate4_5 = ISNULL(B.Be_GradeDate4_5,'') ";
            //StrSql = StrSql + ",Be_GradeDate4_6 = ISNULL(B.Be_GradeDate4_6,'') ";
            //StrSql = StrSql + ",Be_GradeDate4_7 = ISNULL(B.Be_GradeDate4_7,'') ";
            //StrSql = StrSql + ",Be_GradeDate4_8 = ISNULL(B.Be_GradeDate4_8,'') ";
            //StrSql = StrSql + ",Be_GradeDate4_9 = ISNULL(B.Be_GradeDate4_9,'') ";
            //StrSql = StrSql + ",Be_GradeDate4_10 = ISNULL(B.Be_GradeDate4_10,'') ";
            //StrSql = StrSql + ",Be_GradeDate4_11 = ISNULL(B.Be_GradeDate4_11,'') ";
            
            
            StrSql = StrSql + " FROM  tbl_ClosePay_02  A, ";

            StrSql = StrSql + " (Select Mbid,Mbid2 ";
            StrSql = StrSql + " ,Be_PV_1,Be_PV_2"; 
            //StrSql = StrSql + "  ,Reg_Cnt_Be "; 
            //StrSql = StrSql + " ,Be_GradeDate1 ";
            //StrSql = StrSql + " ,Be_GradeDate2 ";
            //StrSql = StrSql + " ,Be_GradeDate3 ";
            //StrSql = StrSql + " ,Be_GradeDate4 ";
            //StrSql = StrSql + " ,Be_GradeDate5 ";
            //StrSql = StrSql + " ,Be_GradeDate6 ";
            //StrSql = StrSql + " ,Be_GradeDate7 ";
            //StrSql = StrSql + " ,Be_GradeDate8 ";
            //StrSql = StrSql + " ,Be_GradeDate9 ";
            //StrSql = StrSql + " ,Be_GradeDate10 ";
            //StrSql = StrSql + " ,Be_GradeDate11 ";

            //StrSql = StrSql + " ,Be_GradeDate4_1 ";
            //StrSql = StrSql + " ,Be_GradeDate4_2 ";
            //StrSql = StrSql + " ,Be_GradeDate4_3 ";
            //StrSql = StrSql + " ,Be_GradeDate4_4 ";
            //StrSql = StrSql + " ,Be_GradeDate4_5 ";
            //StrSql = StrSql + " ,Be_GradeDate4_6 ";
            //StrSql = StrSql + " ,Be_GradeDate4_7 ";
            //StrSql = StrSql + " ,Be_GradeDate4_8 ";
            //StrSql = StrSql + " ,Be_GradeDate4_9 ";
            //StrSql = StrSql + " ,Be_GradeDate4_10 ";
            //StrSql = StrSql + " ,Be_GradeDate4_11 ";            
            
            StrSql = StrSql + " From tbl_ClosePay_02_Mod (nolock) ";
            StrSql = StrSql + " Where  ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            // StrSql = "Update tbl_Sham_Grade Set ";
            //StrSql = StrSql + " Ap_Date = '' ";
            //StrSql = StrSql + " Where Ap_Date >='" + ToEndDate + "'";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();



            
            


            Change_Be_StringField(Temp_Connect, Conn, tran, "ReqDate1");
            Change_Be_StringField_F(Temp_Connect, Conn, tran, "ReqDate2");
            Change_Be_StringField_F(Temp_Connect, Conn, tran, "ReqDate3");
            //Change_Be_StringField_F(Temp_Connect, Conn, tran, "ReqDate4");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "ReqDate5");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate1");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate2");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate3");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate4");            
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate5");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate6");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate7");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate8");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate9");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate10");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate11");

            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate4_1");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate4_2");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate4_3");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate4_4");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate4_5");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate4_6");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate4_7");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate4_8");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate4_9");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate4_10");
            //Change_Be_StringField(Temp_Connect, Conn, tran, "Be_GradeDate4_11");
            
            
            
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_02 SET ";
            //StrSql = StrSql + " LW_Start_WeekCnt = 0 ";
            //StrSql = StrSql + " Where  Be_GradeDate2 >= '" + ToEndDate  + "'";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            ////StrSql = "Update Tbl_Memberinfo SET " ;
            ////StrSql = StrSql + " CurPoint=ISNULL(B.BePoint,0) ";
            ////StrSql = StrSql + " FROM  Tbl_Memberinfo  A, ";

            ////StrSql = StrSql + " (Select Mbid,Mbid2,BePoint ";
            ////StrSql = StrSql + " From tbl_ClosePay_02 ";
            ////StrSql = StrSql + " ) B";

            ////StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            ////StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            ////Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////pg1.PerformStep(); pg1.Refresh();




            ////++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ////++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //string Cancel_Time = "";
            //StrSql = "Select CONVERT(varchar,getdate(),21) ";            
            
            //cls_Connect_DB Temp_Connect_2 = new cls_Connect_DB();

            //DataSet ds = new DataSet();
            //int ReCnt__2 = 0;
            //Temp_Connect_2.Open_Data_Set(StrSql, base_db_name, ds, "", "");
            //ReCnt__2 = Temp_Connect_2.DataSet_ReCount;

            //if (ReCnt__2 > 0)
            //{
            //    Cancel_Time = ds.Tables[base_db_name].Rows[0][0].ToString();
            //}
            

            //StrSql = "Insert Into tbl_ClosePay_02_Cancel_Back  ";
            //StrSql += " ( ";
            //StrSql += " Cancel_Time , ToEndDate,FromEndDate,PayDate,PayDate2 ";
            //StrSql += " ,mbid,mbid2,M_Name,BankCode,BankAcc,Cpno,BankOwner,LeaveDate,StopDate,BusCode,RegTime,Sell_Mem_TF";
            //StrSql += " ,Saveid,Saveid2,Nominid,Nominid2,LineCnt,N_LineCnt,LevelCnt,N_LevelCnt ";
            //StrSql += " ,CurGrade,OrgGrade,BeforeGrade,ShamGrade,OneGrade,Be_OneGrade,MonthGrade,Be_MonthGrade ";
            //StrSql += " ,Cur_DedCut_Pay,Etc_Pay,Allowance1_Cut,Allowance1_Cut_2,Allowance1,Allowance2,Allowance3,Allowance4,Allowance5 ";
            //StrSql += " ,SumAllAllowance,InComeTax,ResidentTax,TruePayment ";
            //StrSql += " ) "; 
            //StrSql += " Select  ";
            //StrSql += " '" + Cancel_Time + "',ToEndDate,FromEndDate,PayDate,PayDate2 ";
            //StrSql += " ,mbid,mbid2,M_Name,BankCode,BankAcc,Cpno,BankOwner,LeaveDate,StopDate,BusCode,RegTime,Sell_Mem_TF" ;
            //StrSql += " ,Saveid,Saveid2,Nominid,Nominid2,LineCnt,N_LineCnt,LevelCnt,N_LevelCnt " ;
            //StrSql += " ,CurGrade,OrgGrade,BeforeGrade,ShamGrade,OneGrade,Be_OneGrade,MonthGrade,Be_MonthGrade " ;
            //StrSql += " ,Cur_DedCut_Pay,Etc_Pay,Allowance1_Cut,Allowance1_Cut_2,Allowance1,Allowance2,Allowance3,Allowance4,Allowance5 " ;
            //StrSql += " ,SumAllAllowance,InComeTax,ResidentTax,TruePayment "; 
            //StrSql += " From  tbl_ClosePay_02_Mod  ";
            //StrSql += " Where ToEndDate = '" + ToEndDate + "' And TruePayment > 0 ";            //수당이 발생한 사람들에 대해서만 우선 백업을 받게 처리함. 마감 취소하기 전에... 취소할려는 마감에 대해서

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            ////++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ////++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++




            //StrSql = "Insert into tbl_Close_Not_Pay_Mod ";
            //StrSql = StrSql + " Select * ,'C','" + cls_User.gid + "', Convert(varchar,getdate(),21)  From tbl_Close_Not_Pay (nolock) ";
            //StrSql = StrSql + " Where ToEndDate = '" + ToEndDate + "'";
            //StrSql = StrSql + " And  Close_FLAG = 'W' ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //StrSql = "Delete tbl_Close_Not_Pay  Where ToEndDate = '" + ToEndDate + "'";
            //StrSql = StrSql + " And  Close_FLAG = 'W' ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);



            StrSql = "DELETE From tbl_ClosePay_02_Sell_Back  Where ToEndDate = '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "DELETE From tbl_ClosePay_02_Back  Where ToEndDate = '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            // >= ToEndDate 처리한것은 가마감 때문에 주간 마감 돌고 가마감이 돌아 있으면 그 내역들도다 삭제 처리하게 하기위함임...
            StrSql = "Update tbl_Close_Not_Pay SET Ap_ToEnddate = ''  ";
            StrSql = StrSql + " Where Ap_ToEndDate >= '" + ToEndDate + "'";
            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Delete From tbl_Close_Not_Pay  Where  ToEndDate  >= '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Delete From tbl_ClosePay_02_ReqTF5  Where  ToEndDate  >= '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);





            StrSql = "Update tbl_ClosePay_10000 SET Ap_ToEnddate = '', Ap_TF = 0  ";
            StrSql = StrSql + " Where Ap_ToEndDate >= '" + ToEndDate + "'";
            StrSql = StrSql + " And  Ap_TF = 2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Delete From tbl_ClosePay_10000  Where ToEndDate >= '" + ToEndDate + "'";
            StrSql = StrSql + " And  ToEndDate_TF = 2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_Sales_Put_Return_Pay SET " ;
            StrSql = StrSql + " Return_Pay2 =  Return_Pay2 + Isnull(B.A1,0) ";
            StrSql = StrSql + " FROM  tbl_Sales_Put_Return_Pay  A, ";

            StrSql = StrSql + " (Select Sum(Return_Pay) A1 , Base_T_index";
            StrSql = StrSql + " From tbl_Sales_Put_Return_Pay  (nolock)   ";
            StrSql = StrSql + " WHERE ToEndDate >= '" + ToEndDate + "'";
            StrSql = StrSql + " And  Base_T_index > 0 ";
            StrSql = StrSql + " And  Cl_TF = 2 ";
            StrSql = StrSql + " Group by  Base_T_index";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.T_index = B.Base_T_index ";            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_Sales_Put_Return_Pay_Ga SET ";
            StrSql = StrSql + " Return_Pay2 =  Return_Pay2 + Isnull(B.A1,0) ";
            StrSql = StrSql + " FROM  tbl_Sales_Put_Return_Pay_Ga  A, ";

            StrSql = StrSql + " (Select Sum(Return_Pay) A1 , Base_T_index";
            StrSql = StrSql + " From tbl_Sales_Put_Return_Pay_Ga (nolock)  ";
            StrSql = StrSql + " WHERE ToEndDate >= '" + ToEndDate + "'";
            StrSql = StrSql + " And  Base_T_index > 0 ";
            StrSql = StrSql + " And  Cl_TF = 2 ";
            StrSql = StrSql + " Group by  Base_T_index";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.T_index = B.Base_T_index ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Update tbl_Sales_Put_Return_Pay_DED SET ";
            StrSql = StrSql + " Return_Pay2 =  Return_Pay2 + Isnull(B.A1,0) ";
            StrSql = StrSql + " FROM  tbl_Sales_Put_Return_Pay_DED  A, ";

            StrSql = StrSql + " (Select Sum(Return_Pay) A1 , Base_T_index";
            StrSql = StrSql + " From tbl_Sales_Put_Return_Pay_DED (nolock)  ";
            StrSql = StrSql + " WHERE ToEndDate >= '" + ToEndDate + "'";
            StrSql = StrSql + " And  Base_T_index > 0 ";
            StrSql = StrSql + " And  Cl_TF = 2 ";
            StrSql = StrSql + " Group by  Base_T_index";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.T_index = B.Base_T_index ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);



            StrSql = "Update tbl_ClosePay_02_Mod SET ";
            StrSql = StrSql + " SumAllAllowance =   Isnull(B.SumAllAllowance,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod  A, ";

            StrSql = StrSql + " (Select  Cut_ToEndDate, mbid, mbid2,SumAllAllowance  ";
            StrSql = StrSql + " From tbl_Close_Not_Pay_Cut_Mod (nolock)  ";
            StrSql = StrSql + " WHERE ToEndDate >= '" + ToEndDate + "'";
            StrSql = StrSql + " And  Close_FLAG = 'W'  "; 
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.ToEndDate = B.Cut_ToEndDate ";
            StrSql = StrSql + " And   A.mbid = B.mbid ";
            StrSql = StrSql + " And   A.mbid2 = B.mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02_Mod SET ";
            StrSql = StrSql + " Allowance1 =   Isnull(B.SumAllAllowance,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod  A, ";

            StrSql = StrSql + " (Select  Cut_ToEndDate, mbid, mbid2,SumAllAllowance  ";
            StrSql = StrSql + " From tbl_Close_Not_Pay_Cut_Mod (nolock)  ";
            StrSql = StrSql + " WHERE ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " And  Close_FLAG = 'W1'  ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.ToEndDate = B.Cut_ToEndDate ";
            StrSql = StrSql + " And   A.mbid = B.mbid ";
            StrSql = StrSql + " And   A.mbid2 = B.mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Update tbl_ClosePay_02_Mod SET ";
            StrSql = StrSql + " Allowance2 =   Isnull(B.SumAllAllowance,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod  A, ";

            StrSql = StrSql + " (Select  Cut_ToEndDate, mbid, mbid2,SumAllAllowance  ";
            StrSql = StrSql + " From tbl_Close_Not_Pay_Cut_Mod (nolock)  ";
            StrSql = StrSql + " WHERE ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " And  Close_FLAG = 'W2'  ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.ToEndDate = B.Cut_ToEndDate ";
            StrSql = StrSql + " And   A.mbid = B.mbid ";
            StrSql = StrSql + " And   A.mbid2 = B.mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Update tbl_ClosePay_02_Mod SET ";
            StrSql = StrSql + " Allowance3 =   Isnull(B.SumAllAllowance,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_02_Mod  A, ";

            StrSql = StrSql + " (Select  Cut_ToEndDate, mbid, mbid2,SumAllAllowance  ";
            StrSql = StrSql + " From tbl_Close_Not_Pay_Cut_Mod (nolock)  ";
            StrSql = StrSql + " WHERE ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " And  Close_FLAG = 'W3'  ";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.ToEndDate = B.Cut_ToEndDate ";
            StrSql = StrSql + " And   A.mbid = B.mbid ";
            StrSql = StrSql + " And   A.mbid2 = B.mbid2 ";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Delete From tbl_Close_Not_Pay_Cut_Mod Where ToEndDate >= '" + ToEndDate + "' And Close_FLAG = 'W' ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            //insert into tbl_Close_Not_Pay_Cut_Mod
            //            (ToEndDate, Cut_ToEndDate, mbid, mbid2, Close_FLAG, SumAllAllowance)

            //            Values
            //            (@v_ToEndDate, @v_Be_ToEndDate, @v_Mbid, @v_Mbid2, 'M', @v_SumAllAllowance)





            StrSql = "Delete From tbl_ClosePay_02 Where RecordMakeDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Delete From tbl_ClosePay_02_Sell Where RecordMakeDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Delete From tbl_ClosePay_02_Mod Where ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_ClosePay_02_Sell_Mod Where ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_Close_DownPV_ALL_02 Where EndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_Close_DownPV_PV_02 Where EndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_Sales_Put_Return_Pay Where ToEndDate >= '" + ToEndDate + "' And  Cl_TF = 2";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From tbl_Sales_Put_Return_Pay_Ga Where ToEndDate >= '" + ToEndDate + "' And  Cl_TF = 2";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From tbl_Sales_Put_Return_Pay_DED Where ToEndDate = '" + ToEndDate + "' And  Cl_TF = 2";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_CloseTotal_02 Where ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From tbl_CloseTotal_02_Ga Where ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Delete From tbl_ClosePay_02_Mod_Retry Where ToEndDate = '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Delete From tbl_ClosePay_02_Mod_Retry_Back Where ToEndDate = '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            //StrSql = "Delete From tbl_ClosePay_02_G_FLAG Where Give_ToEndDate >= '" + ToEndDate + "'";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Delete From tbl_ClosePay_02_G_FLAG Where Grade_FLAG_ToEndDate >= '" + ToEndDate + "'";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);


            StrSql = "Delete From tbl_ClosePay_02_Ded_P_Detail_Mod Where Cur_ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From tbl_ClosePay_02_Ded_P_Mod Where ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
                        

            //StrSql = "Update tbl_ClosePay_02_G_FLAG Set ";
            //StrSql = StrSql + " Cut_FLAG = '' ";
            //StrSql = StrSql + " Where Cut_FLAG >='" + ToEndDate + "'";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();
            



            //StrSql = "Update Tbl_Memberinfo SET ";
            //StrSql = StrSql + " CurGrade = 0   ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update Tbl_Memberinfo SET ";
            //StrSql = StrSql + " CurGrade=ISNULL(B.CurGrade_M2,0) ";            
            //StrSql = StrSql + " FROM  Tbl_Memberinfo  A, ";
            //;
            //StrSql = StrSql + " (Select Mbid,Mbid2, CurGrade_M2  ";
            //StrSql = StrSql + " From tbl_ClosePay_02_Mod  ";
            //StrSql = StrSql + " Where ToEndDate in (Select Isnull(Max(ToEndDate),'') From tbl_CloseTotal_02 )";
            //StrSql = StrSql + " ) B";

            //StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            //StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            Check_Close_Gid(Temp_Connect, Conn, tran, 2, 1);
            pg1.PerformStep(); pg1.Refresh();


            //-----가마감 디비 상에서 동일 날짜의 가마감을 삭제 처리 한다.---
            StrSql = "Delete From mannatech_Ga_Close.dbo.tbl_ClosePay_02 Where RecordMakeDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From mannatech_Ga_Close.dbo.tbl_ClosePay_02_Sell Where RecordMakeDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From mannatech_Ga_Close.dbo.tbl_ClosePay_02_Mod Where ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From mannatech_Ga_Close.dbo.tbl_ClosePay_02_Sell_Mod Where ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From mannatech_Ga_Close.dbo.tbl_Close_DownPV_ALL_02 Where EndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From mannatech_Ga_Close.dbo.tbl_Close_DownPV_PV_02 Where EndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From mannatech_Ga_Close.dbo.tbl_Sales_Put_Return_Pay Where ToEndDate >= '" + ToEndDate + "' And  Cl_TF = 2";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From mannatech_Ga_Close.dbo.tbl_Sales_Put_Return_Pay_DED Where ToEndDate >= '" + ToEndDate + "' And  Cl_TF = 2";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From mannatech_Ga_Close.dbo.tbl_CloseTotal_02 Where ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From mannatech_Ga_Close.dbo.tbl_ClosePay_02_Ded_P_Detail_Mod Where Cur_ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);

            StrSql = "Delete From mannatech_Ga_Close.dbo.tbl_ClosePay_02_Ded_P_Mod Where ToEndDate >= '" + ToEndDate + "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //-----가마감 디비 상에서 동일 날짜의 가마감을 삭제 처리 한다.---



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
            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + ChangeField + " = '' ";
            StrSql = StrSql + " Where " + ChangeField + " >= '" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
        }

        private void Change_Be_StringField_F(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string ChangeField)
        {
            string StrSql = "";
            StrSql = "Update tbl_ClosePay_02 Set ";
            StrSql = StrSql + ChangeField + " = '' ";
            StrSql = StrSql + " Where " + ChangeField + " >= '" + FromEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
        }










    }
}
