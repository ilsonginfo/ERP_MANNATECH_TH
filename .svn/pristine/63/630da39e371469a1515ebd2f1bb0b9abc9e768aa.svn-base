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
    public partial class frmClose_100_Cancel : Form
    {
                
        private string base_db_name = "tbl_CloseTotal_100";
        private string FromEndDate = ""; private string ToEndDate = ""; private string PayDate = "", PayDate2 = "";
        private int From_Load_TF = 0;
        private int  ReCnt = 0;


        public frmClose_100_Cancel()
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

                //if (Close_Check_Date_02("tbl_CloseTotal_02") == false)
                //{
                //    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Week"));
                //    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Day"));
                //    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Month"));
                //    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Same_Per"));
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
            Tsql = "Select Top 1 FromEndDate , ToEndDate , PayDate From  tbl_CloseTotal_100 (nolock) Order by  ToEndDate DESC ";

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
        }


        private void Close_Work_Real(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            pg1.Minimum = 0; pg1.Maximum = 16;
            pg1.Step = 1; pg1.Value = 0;
            string StrSql = "";

          

            StrSql = "Update tbl_Sales_Put_Return_Pay SET " ;
            StrSql = StrSql + " Return_Pay2 =  Return_Pay2 + Isnull(B.A1,0) ";
            StrSql = StrSql + " FROM  tbl_Sales_Put_Return_Pay  A, ";

            StrSql = StrSql + " (Select Sum(Return_Pay) A1 , Base_T_index";
            StrSql = StrSql + " From tbl_Sales_Put_Return_Pay  ";
            StrSql = StrSql + " WHERE ToEndDate= '" + ToEndDate + "'";
            StrSql = StrSql + " And  Base_T_index > 0 ";
            StrSql = StrSql + " And  Cl_TF = 100 ";
            StrSql = StrSql + " Group by  Base_T_index";
            StrSql = StrSql + " ) B";
            StrSql = StrSql + " Where A.T_index = B.Base_T_index ";            
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            StrSql = "Delete From tbl_ClosePay_100 Where RecordMakeDate = '" + ToEndDate +  "'" ;
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_ClosePay_100_Mod Where ToEndDate = '" + ToEndDate +  "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_Close_DownPV_PV_100 Where EndDate = '" + ToEndDate +  "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_Sales_Put_Return_Pay Where ToEndDate = '" + ToEndDate +  "' And  Cl_TF = 100 ";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();

            StrSql = "Delete From tbl_CloseTotal_100 Where ToEndDate = '" + ToEndDate +  "'";
            Temp_Connect.Insert_Data(StrSql, Conn, tran);
            pg1.PerformStep(); pg1.Refresh();


            Check_Close_Gid(Temp_Connect, Conn, tran, 100, 1);
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
            StrSql = "Update tbl_ClosePay_100 Set ";
            StrSql = StrSql + ChangeField + " = '' ";
            StrSql = StrSql + " Where " + ChangeField + "='" + ToEndDate + "'";

            Temp_Connect.Insert_Data(StrSql, Conn, tran);
        }


    }
}
