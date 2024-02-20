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
    public partial class frmClose_1_Cancel : Form
    {

        
        private string base_db_name = "tbl_CloseTotal_01";
        private string FromEndDate = ""; private string ToEndDate = ""; private string PayDate = "", PayDate2 = "";
        private int From_Load_TF = 0;
        private int  ReCnt = 0;
        private string Base_Chang_Date___1 = "";


        public frmClose_1_Cancel()
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

                if (Close_Check_Date_02("tbl_CloseTotal_04") == false)
                {
                    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Week"));
                    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Day"));
                    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Month"));
                    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Per"));
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Week"));
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
            Tsql = "Select Top 1 FromEndDate , ToEndDate , PayDate From  tbl_CloseTotal_01 (nolock) Order by  ToEndDate DESC ";

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

            Base_Chang_Date___1 = "20170227";


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
        }


        private void Close_Work_Real(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Minimum = 0; pg1.Maximum = 20;
            pg1.Step = 1; pg1.Value = 0;
            string StrSql = "";

            StrSql = "Update tbl_ClosePay_01_Sell SET " ;
            StrSql = StrSql + "  BeAmount=ISNULL(B.BeAmount,0) ";
            StrSql = StrSql + " ,BeCash=ISNULL(B.BeCash,0) ";
            StrSql = StrSql + " ,BeCard=ISNULL(B.BeCard,0) ";
            StrSql = StrSql + " ,BeBank=ISNULL(B.BeBank,0) ";
            StrSql = StrSql + " ,BeTotalPV=ISNULL(B.BeTotalPV,0) ";
            StrSql = StrSql + " ,BeShamSell=ISNULL(B.BeShamSell,0) ";
            //StrSql = StrSql + " ,BeShamSell_Pr=ISNULL(B.BeShamSell_Pr,0) ";
            StrSql = StrSql + " ,BeReAmount=ISNULL(B.BeReAmount,0) ";
            StrSql = StrSql + " ,BeReCash=ISNULL(B.BeReCash,0) ";
            StrSql = StrSql + " ,BeReCard=ISNULL(B.BeReCard,0) ";
            StrSql = StrSql + " ,BeReBank=ISNULL(B.BeReBank,0) ";
            StrSql = StrSql + " ,BeReTotalPV=ISNULL(B.BeReTotalPV,0) ";
            StrSql = StrSql + " ,BeTotalCV=ISNULL(B.BeTotalCV,0) ";
            StrSql = StrSql + " ,BeReTotalCV=ISNULL(B.BeReTotalCV,0) ";

            StrSql = StrSql + " FROM  tbl_ClosePay_01_Sell  A, ";
    
            StrSql = StrSql + " (Select Mbid,Mbid2,SellCode,BeAmount,BeCash,BeCard,BeBank,BeTotalPV,BeShamSell,";
            StrSql = StrSql + " BeReAmount,BeReCash,BeReCard,BeReBank,BeReTotalPV,";
            StrSql = StrSql + " BeTotalCV,BeReTotalCV ";
            StrSql = StrSql + " From tbl_ClosePay_01_Sell_Mod ";
            StrSql = StrSql + " Where  ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " ) B";
    
            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            StrSql = StrSql + " And   A.SellCode=B.SellCode ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "insert into tbl_Member_Mileage_Mod Select *,'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),120) ";
            //StrSql = StrSql + " From tbl_Member_Mileage ";
            //StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "' And Plus_OrderNumber = 'A1' And PlusKind IN ('51' ) And PlusValue > 0  ";

            //Temp_Connect.Insert_Data(StrSql, Conn, tran);

            //StrSql = "Delete From tbl_Member_Mileage Where ToEndDate ='" + ToEndDate + "' And Plus_OrderNumber = 'A1' And PlusKind = '51' And PlusValue > 0 ";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);

           

            StrSql = "Update tbl_ClosePay_01 SET ";
            StrSql = StrSql + " Be_PV_1 = ISNULL(B.Be_PV_1,0) ";
            StrSql = StrSql + ",Be_PV_2 = ISNULL(B.Be_PV_2,0) ";
            StrSql = StrSql + ",BeforeGrade = ISNULL(B.BeforeGrade,0) ";
            StrSql = StrSql + ",BePoint = ISNULL(B.BePoint,0) ";
            StrSql = StrSql + " FROM  tbl_ClosePay_01  A, ";

            StrSql = StrSql + " (Select Mbid,Mbid2 ";
            StrSql = StrSql + " ,Be_PV_1,Be_PV_2, BeforeGrade , BePoint    ";
            StrSql = StrSql + " From tbl_ClosePay_01_Mod ";
            StrSql = StrSql + " Where  ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_01 Set CurPoint_Date_2 ='' " ;
            //StrSql = StrSql +" Where CurPoint_Date_2 >='" + FromEndDate + "'";
            //StrSql = StrSql +" And  CurPoint_Date_2 <='" + ToEndDate + "'";
            //StrSql = StrSql +" And  CurPoint_Date_2 <> ''";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            //StrSql = "Update tbl_ClosePay_01 Set CurPoint_Date_3 ='' ";
            //StrSql = StrSql +" Where CurPoint_Date_3 >='" + FromEndDate + "'";
            //StrSql = StrSql +" And  CurPoint_Date_3 <='" + ToEndDate + "'";
            //StrSql = StrSql +" And  CurPoint_Date_3 <> ''";
            //Temp_Connect.Insert_Data(StrSql, Conn, tran);
            //pg1.PerformStep(); pg1.Refresh();


            Change_Be_StringField(Temp_Connect, Conn, tran ,"ReqDate1");
            Change_Be_StringField(Temp_Connect, Conn, tran, "P_Date_10");
            Change_Be_StringField(Temp_Connect, Conn, tran, "P_Date_20");
            Change_Be_StringField(Temp_Connect, Conn, tran, "P_Date_30");
            Change_Be_StringField(Temp_Connect, Conn, tran, "P_Date_40");
            pg1.PerformStep(); pg1.Refresh();

           
           
            
           

            if (int.Parse(FromEndDate) >= int.Parse (Base_Chang_Date___1))
            {
                StrSql = "Update Tbl_Memberinfo SET ";
                StrSql = StrSql + " CurGrade = 0  ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                

                StrSql = "Update Tbl_Memberinfo SET ";                
                StrSql = StrSql + " CurGrade = ISNULL(B.BeforeGrade,0) ";
                StrSql = StrSql + " FROM  Tbl_Memberinfo  A, ";

                StrSql = StrSql + " (Select Mbid,Mbid2,BeforeGrade ";
                StrSql = StrSql + " From tbl_ClosePay_01 ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                

                StrSql = "Update tbl_Sham_Grade Set ";
                StrSql = StrSql + " Ap_Date = '' ";
                StrSql = StrSql + " Where Ap_Date ='" + ToEndDate + "'";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);

                Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate05");
                Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate1");
                Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate2");
                Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate3");
                Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate4");
                Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate5");
                Change_Be_StringField(Temp_Connect, Conn, tran, "GradeDate6");
                Change_Be_StringField(Temp_Connect, Conn, tran, "Sham_GradeDate20");
                Change_Be_StringField(Temp_Connect, Conn, tran, "Sham_GradeDate30");
                Change_Be_StringField(Temp_Connect, Conn, tran, "Sham_GradeDate40");
                Change_Be_StringField(Temp_Connect, Conn, tran, "Sham_GradeDate50");

            }

            if (int.Parse(FromEndDate) == int.Parse(Base_Chang_Date___1))
            {
                StrSql = "Update Tbl_Memberinfo SET ";
                StrSql = StrSql + " CurGrade = 0  ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                

                StrSql = "Update Tbl_Memberinfo SET ";
                StrSql = StrSql + " CurGrade = ISNULL(B.BeforeGrade,0) ";
                StrSql = StrSql + " FROM  Tbl_Memberinfo  A, ";

                StrSql = StrSql + " (Select Mbid,Mbid2,BeforeGrade ";
                StrSql = StrSql + " From tbl_ClosePay_02 ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid=B.Mbid ";
                StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);

                StrSql = "Update tbl_ClosePay_01 SET ";
                StrSql = StrSql + " GradeDate1 =  ''";
                StrSql = StrSql + " ,GradeDate2 =  ''";
                StrSql = StrSql + " ,GradeDate3 =  ''";
                StrSql = StrSql + " ,GradeDate4 =  ''";
                StrSql = StrSql + " ,GradeDate5 =  ''";
                StrSql = StrSql + " ,GradeDate6 =  ''";
                StrSql = StrSql + " ,Sham_GradeDate20 =  ''";
                StrSql = StrSql + " ,Sham_GradeDate30 =  ''";
                StrSql = StrSql + " ,Sham_GradeDate40 =  ''";
                StrSql = StrSql + " ,Sham_GradeDate50 =  ''";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);                
            }


            StrSql = "Update tbl_Sales_Put_Return_Pay SET " ;
            StrSql = StrSql + " Return_Pay2 =  Return_Pay2 + Isnull(B.A1,0) ";
            StrSql = StrSql + " FROM  tbl_Sales_Put_Return_Pay  A, ";

            StrSql = StrSql + " (Select Sum(Return_Pay) A1 , Base_T_index";
            StrSql = StrSql + " From tbl_Sales_Put_Return_Pay  ";
            StrSql = StrSql + " WHERE ToEndDate= '" + ToEndDate + "'";
            StrSql = StrSql + " And  Base_T_index > 0 ";
            StrSql = StrSql + " And  Cl_TF = 1 ";
            StrSql = StrSql + " Group by  Base_T_index";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.T_index = B.Base_T_index ";            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_ClosePay_01 Where RecordMakeDate = '" + ToEndDate +  "'" ;
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_ClosePay_01_Mod Where ToEndDate = '" + ToEndDate +  "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_ClosePay_01_Sell_Mod Where ToEndDate = '" + ToEndDate +  "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_Close_DownPV_ALL_01 Where EndDate = '" + ToEndDate +  "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_Close_DownPV_PV_01 Where EndDate = '" + ToEndDate +  "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_Sales_Put_Return_Pay Where ToEndDate = '" + ToEndDate +  "' And  Cl_TF = 1";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_CloseTotal_01 Where ToEndDate = '" + ToEndDate +  "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();



            StrSql = "Update Tbl_Memberinfo SET ";
            StrSql = StrSql + " CurPoint=0 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update Tbl_Memberinfo SET ";
            StrSql = StrSql + " CurPoint=ISNULL(B.CurPoint,0) ";
            StrSql = StrSql + " FROM  Tbl_Memberinfo  A, ";

            StrSql = StrSql + " (Select Mbid,Mbid2,CurPoint ";
            StrSql = StrSql + " From tbl_ClosePay_01_Mod  ";
            StrSql = StrSql + " Where ToEndDate in (Select Max(ToEndDate) From tbl_CloseTotal_01 (nolock)) ";
            StrSql = StrSql + " ) B";

            StrSql = StrSql + " Where A.Mbid=B.Mbid ";
            StrSql = StrSql + " And   A.Mbid2=B.Mbid2 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Update tbl_Sham_Grade_P Set ";
            StrSql = StrSql + " Ap_Date = '' ";
            StrSql = StrSql + " Where Ap_Date ='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);


            if (int.Parse(FromEndDate) == int.Parse(Base_Chang_Date___1))
            {
                StrSql = "Update Tbl_Memberinfo SET ";
                StrSql = StrSql + " CurPoint=0 ";
                Temp_Connect.Insert_Data(StrSql, Conn, tran);
                

                StrSql = "Update Tbl_Memberinfo SET ";
                StrSql = StrSql + " CurPoint=ISNULL(B.Cur_Point,0) ";
                StrSql = StrSql + " FROM  Tbl_Memberinfo  A, ";

                StrSql = StrSql + " (Select Mbid2,Cur_Point ";
                StrSql = StrSql + " From Sheet_Cur_point (nolock) ";
                StrSql = StrSql + " ) B";

                StrSql = StrSql + " Where A.Mbid2 = B.Mbid2 ";

                Temp_Connect.Insert_Data(StrSql, Conn, tran);
            }




            Check_Close_Gid(Temp_Connect, Conn, tran, 1, 1);
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
            StrSql = "Update tbl_ClosePay_01 Set ";
            StrSql = StrSql + ChangeField + " = '' ";
            StrSql = StrSql + " Where " + ChangeField + "='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
        }

    }
}
