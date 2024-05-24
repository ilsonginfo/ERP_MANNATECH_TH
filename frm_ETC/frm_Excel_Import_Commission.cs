using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;

namespace MLM_Program
{
    public partial class frm_Excel_Import_Commission : clsForm_Extends
    {

        clsExcel oExcel = null;
        DataSet dsExcels = new DataSet();

        private int Load_TF = 0;
        public frm_Excel_Import_Commission()
        {
            InitializeComponent();
        }


        private void btnLoad_Click(object sender, EventArgs e)
        {
            
            int RCnt = dGridView_Base.Rows.Count-1;

            if (RCnt > 0)
            {
                dGridView_Base.Rows.Clear();
                dGridView_Base.Visible = false;
                for (int TCnt = 0; TCnt <= RCnt; TCnt++)
                    dGridView_Base.Rows.Remove(dGridView_Base.Rows[0]);

                
                //dGridView_Base.Rows.Remove(dGridView_Base.Rows[0]);
                dGridView_Base.Visible = true;
            }

            
            //dGridView_Base.Rows.Clear();
            txtFilePath.Text = "";
            combo_Sheet.Items.Clear();
            Load_TF = 0;
            LoadNewFile();
        }         
              
                
         private void LoadNewFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            System.Windows.Forms.DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                txtFilePath.Text = ofd.FileName;


                try
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    loadExcel_Sheet();

                }

                catch (System.Exception theException)
                {

                    String errorMessage;
                    errorMessage = "Error: ";
                    errorMessage = String.Concat(errorMessage, theException.Message);
                    errorMessage = String.Concat(errorMessage, " Line: ");
                    errorMessage = String.Concat(errorMessage, theException.Source);


                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Export_Err"));

                    if (cls_User.gid == cls_User.SuperUserID)
                        MessageBox.Show(theException.Message);
                }
                finally
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;  
                }

            }
        }


         private void loadExcel_Sheet()
        {

            try
            {

                if (oExcel != null)
                {
                    oExcel.Dispose();
                }


                oExcel = new clsExcel(txtFilePath.Text);

                foreach (DataTable table in oExcel.Excel_ds)
                {
                    combo_Sheet.Items.Add(table.TableName.ToString().Trim('\'').Replace("$", ""));

                    //Debug.WriteLine(table.TableName);
                }

                Load_TF = 1;

            }
            catch (Exception ex)
            {
                Load_TF = 0;
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {

            }
        }

         private void Grid_Base_Seting()
         {
             dGridView_Base.DefaultCellStyle.Font = new System.Drawing.Font("돋움", float.Parse("8.4"));
             dGridView_Base.ColumnHeadersHeight = 18;
             dGridView_Base.GridColor = System.Drawing.Color.Silver;
             dGridView_Base.EnableHeadersVisualStyles = false;
             dGridView_Base.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(208, 222, 176);
             dGridView_Base.DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 241, 220);
             dGridView_Base.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
             dGridView_Base.BorderStyle = BorderStyle.Fixed3D;
             dGridView_Base.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
             dGridView_Base.RowHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightCyan;
             dGridView_Base.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
             dGridView_Base.ReadOnly = true;
             dGridView_Base.AllowUserToAddRows = false; 


         }


         private void loadExcelToDataGrid()
        {
            Grid_Base_Seting();
            try
            {

                DataTable dt = oExcel.Excel_ds[combo_Sheet.SelectedItem.ToString()];


                dGridView_Base.DataSource = dt;
                if (dGridView_Base.Columns.Contains("SuccessYn"))
                {
                    dGridView_Base.Columns.Remove("SuccessYn");
                }

                if (dGridView_Base.Columns.Contains("ErrorDc"))
                {
                    dGridView_Base.Columns.Remove("ErrorDc");
                }

                dGridView_Base.DataSource = dt;

                dGridView_Base.Columns.Add("SuccessYn", "성공여부");
                dGridView_Base.Columns["SuccessYn"].Width = 100;

                dGridView_Base.Columns.Add("ErrorDc", "오류메시지");
                dGridView_Base.Columns["ErrorDc"].Width = 600;

            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
            finally
            {

            }
        }

         private void combo_Pay_SelectedIndexChanged(object sender, EventArgs e)
         {
             if (Load_TF == 0) 
                 return;

             if (combo_Sheet.Text != "")
             {
                 try
                 {
                     this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    loadExcelToDataGrid();

                 }

                 catch (System.Exception theException)
                 {

                     String errorMessage;
                     errorMessage = "Error: ";
                     errorMessage = String.Concat(errorMessage, theException.Message);
                     errorMessage = String.Concat(errorMessage, " Line: ");
                     errorMessage = String.Concat(errorMessage, theException.Source);


                     MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Export_Err"));

                     if (cls_User.gid == cls_User.SuperUserID)
                         MessageBox.Show(theException.Message);
                 }
                 finally
                 {
                     this.Cursor = System.Windows.Forms.Cursors.Default;
                 }
             }

         }


         private void dGridView_Base_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
         {

             //Header인지 확인
             if (e.ColumnIndex < 0 & e.RowIndex >= 0)
             {
                 e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

                 //행 번호를 표시할 범위를 결정
                 System.Drawing.Rectangle indexRect = e.CellBounds;
                 indexRect.Inflate(-2, -2);
                 //행번호를 표시
                 TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                                       e.CellStyle.Font, indexRect, e.CellStyle.ForeColor,
                                       TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                 e.Handled = true;
             }
         }



         private void frmBase_Resize(object sender, EventArgs e)
         {
             butt_Clear.Left = 0;
             butt_Select.Left = butt_Clear.Left + butt_Clear.Width + 2;
             butt_Excel.Left = butt_Select.Left + butt_Select.Width + 2;
             butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
             butt_Exit.Left = this.Width - butt_Exit.Width - 17;


             cls_form_Meth cfm = new cls_form_Meth();
             cfm.button_flat_change(butt_Clear);
             cfm.button_flat_change(butt_Select);
             cfm.button_flat_change(butt_Delete);
             cfm.button_flat_change(butt_Excel);
             cfm.button_flat_change(butt_Exit);
             cfm.button_flat_change(butt_Search);  
             
         }


         private void frm_Base_Activated(object sender, EventArgs e)
         {
            //19-03-11 깜빡임제거 this.Refresh();
         }

         private void frmBase_From_KeyDown(object sender, KeyEventArgs e)
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

                             //cls_form_Meth cfm = new cls_form_Meth();
                             // cfm.form_Group_Panel_Enable_True(this);

                             //SendKeys.Send("{TAB}");
                         }
                     }
                 }// end if

             }

             ////그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
             //if (sender is DataGridView)
             //{
             //                 if (e.KeyValue == 13)
             //    {
             //        EventArgs ee = null;
             //        //dGridView_Base_DoubleClick(sender, ee);
             //        e.Handled = true;
             //    } // end if
             //}

             Button T_bt = butt_Exit;
             if (e.KeyValue == 123)
                 T_bt = butt_Exit;    //닫기  F12
             if (e.KeyValue == 113)
                 T_bt = butt_Select;     //조회  F1
             if (e.KeyValue == 115)
                 T_bt = butt_Delete;   // 삭제  F4
             if (e.KeyValue == 119)
                 T_bt = butt_Excel;    //엑셀  F8    
             if (e.KeyValue == 112)
                 T_bt = butt_Clear;    //엑셀  F5    

             if (T_bt.Visible == true)
             {
                 EventArgs ee1 = null;
                 if (e.KeyValue == 123 || e.KeyValue == 113 || e.KeyValue == 119 || e.KeyValue == 112)
                     butt_Exit_Click(T_bt, ee1);
             }
         }

         private void butt_Exit_Click(object sender, EventArgs e)
         {            

             Button bt = (Button)sender;


            if (bt.Name == "butt_Select")
            {
                Boolean chage_Center_tf = false;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                progress.Visible = true; progress.Value = 0;

                chage_Center_tf = Insert_Data();  //실질적인 센터 변경이 이루어지는 메소드


                progress.Visible = false;
                this.Cursor = System.Windows.Forms.Cursors.Default;

                if (chage_Center_tf == true)
                {
                    //int RCnt = dGridView_Base.Rows.Count - 1;

                    //if (RCnt > 0)
                    //{
                    //    dGridView_Base.Visible = false;
                    //    for (int TCnt = 0; TCnt <= RCnt - 1; TCnt++)
                    //        dGridView_Base.Rows.Remove(dGridView_Base.Rows[0]);
                    //    dGridView_Base.Visible = true;
                    //}
                    
                    //txtFilePath.Text = "";
                    //combo_Sheet.Items.Clear();
                    Load_TF = 0;
                }

            }
            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }


         }

         private void frm_Excel_Import_Rec_Load(object sender, EventArgs e)
         {
             cls_form_Meth cm = new cls_form_Meth();
             cm.from_control_text_base_chang(this);
         }






         private Boolean Chang_CenterCode()
         {
             //Msg_Useing_Not_Data
             if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return false;
             if (Check_TextBox_Save_Error() == false) return false;

             //++++++++++++++++++++++++++++++++
             cls_Connect_DB Temp_Connect = new cls_Connect_DB();

             Temp_Connect.Connect_DB();

             SqlConnection Conn = Temp_Connect.Conn_Conn();
             SqlTransaction tran = Conn.BeginTransaction();

             string StrSql = "";
             cls_form_Meth cm = new cls_form_Meth();
            int Err_Cnt = 0; 
             try
             {
                 //string T_Mbid = "";                 
                 string OrderNumber = ""; //string Out_Index = ""; int SalesItemIndex = 0;  
                 string Pass_Number = "";
                 cls_Search_DB csd = new cls_Search_DB();
                 

                string Tsql;

                //============================================================================================================
                Tsql = "SELECT Isnull( Count(Mbid) ,0) A1    ";
                Tsql = Tsql + " From tbl_Sham_Pay  (nolock)  ";
                Tsql = Tsql + " Where Convert(Varchar,mbid2) +'-' + Apply_Date +'-'+ SortKind2 in ( ";                 
                Tsql = Tsql + "                                Select Convert(Varchar,mbid2) +'-' + Apply_Date +'-'+ SortKind2  ";
                Tsql = Tsql + "                                From tbl_Sham_Pay_ETC (nolock) ";
                Tsql = Tsql + "                                Where  Recordid  = '" + cls_User.gid + "'";
                Tsql = Tsql + "                                                                   )";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Sham_Pay", ds) == true)
                {
                    int ReCnt = Temp_Connect.DataSet_ReCount;

                    if (ReCnt > 0)
                    {
                        if (int.Parse(ds.Tables["tbl_Sham_Pay"].Rows[0][0].ToString()) > 0)
                        {

                            if (MessageBox.Show("입력하실 내역중에 이미 저장된 내역이 존재합니다. 기존 내역을 지우고 계속 진행 하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                            {                               

                                StrSql = "Delete From  tbl_Sham_Pay_ETC  ";
                                StrSql = StrSql + " Where Recordid = '" + cls_User.gid + "'";

                                Temp_Connect.Insert_Data(StrSql, "tbl_Sham_Pay", Conn, tran);

                                tran.Commit();

                                Err_Cnt = 1; 
                            }
                        }
                    }
                }
                //============================================================================================================


                if (Err_Cnt == 0)
                {
                    StrSql = "INSERT INTO tbl_Sham_Pay (Mbid, Mbid2, M_Name,Apply_Date , Apply_Pv , SortKind2 , Etc, RecordID, RecordTime, RecordID_FLAG )";
                    StrSql = StrSql + "  Select Mbid, Mbid2, M_Name,Apply_Date , Apply_Pv , SortKind2 , Etc, RecordID, RecordTime , RecordID  From  tbl_Sham_Pay_ETC (nolock ) ";
                    StrSql = StrSql + " Where Recordid = '" + cls_User.gid + "'";

                    Temp_Connect.Insert_Data(StrSql, "tbl_Sham_Pay_ETC", Conn, tran);
                                                         
                    StrSql = "Delete From  tbl_Sham_Pay_ETC  ";
                    StrSql = StrSql + " Where Recordid = '" + cls_User.gid + "'";

                    Temp_Connect.Insert_Data(StrSql, "tbl_Sham_Pay_ETC", Conn, tran);
                }



                //for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                //{

                //   OrderNumber = dGridView_Base.Rows[i].Cells[5].Value.ToString();
                //   Pass_Number = dGridView_Base.Rows[i].Cells[4].Value.ToString();
                //   //SalesItemIndex = int.Parse(dGridView_Base.Rows[i].Cells[18].Value.ToString());

                //   StrSql = "Insert into tbl_ClosePay_01_Mod  (ToEndDate, FromEndDate, PayDate, Mbid ,Mbid2, M_Name,TruePayment ) ";
                //   StrSql = StrSql + " Values ";
                //   StrSql = StrSql + " (" ;
                //   StrSql = StrSql + " )";

                //   Temp_Connect.Insert_Data(StrSql, "tbl_Sales_Rece", Conn, tran, this.Name.ToString(), this.Text);


                //    progress.Value = progress.Value + 1;
                //}
                tran.Commit();


                 MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
                 return true;
             }


             catch (Exception)
             {
                 tran.Rollback();
                 MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
                 return false;
             }

             finally
             {
                 tran.Dispose();
                 Temp_Connect.Close_DB();
             }
         }






         private Boolean Check_TextBox_Save_Error()
         {
             //string Min_SellDate = cls_User.gid_date_time;
             //string S_SellDate = "";

             if (dGridView_Base.Rows.Count <= 0)
             {
                 MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Excel_Not_Import")                      
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                 dGridView_Base.Focus(); return false;
             }

             if (txtFilePath.Text.Trim () == "")
             {
                 MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Excel_Not_Import")                       
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                 txtFilePath.Focus(); return false;
             }

             if (combo_Sheet.Text.Trim() == "")
             {
                 MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Excel_Not_Import")                       
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                 combo_Sheet.Focus(); return false;
             }


            int Min_ApDAte = 0, Min_ApDAte_M = 0; ;
            string Tsql = "";
            string Mbid2 = "", AppDate = "", Ap_PV = "", AP_Sort = "", M_Name = "";
            for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            {
                Mbid2 = ""; AppDate = ""; Ap_PV = ""; AP_Sort = "";

                Mbid2 = dGridView_Base.Rows[i].Cells[0].Value.ToString();
                AppDate = dGridView_Base.Rows[i].Cells[2].Value.ToString().Replace("-", "");
                Ap_PV = dGridView_Base.Rows[i].Cells[3].Value.ToString().Replace (",","");
                AP_Sort = dGridView_Base.Rows[i].Cells[4].Value.ToString();

                if (AP_Sort != "월" && AP_Sort != "주")
                {
                    MessageBox.Show("공제구분을  주 또는 월 로 입력을 해주세요." + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select"));
                      return false;
                }

                int chknum = 0;
                bool isnum = int.TryParse(Mbid2, out chknum);

                if (isnum == false)
                {
                    MessageBox.Show("올바르지 않은 회원번호가 존재 합니다." + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select"));
                    return false;
                }

                chknum = 0;
                isnum = int.TryParse(AppDate, out chknum);

                if (isnum == false)
                {
                    MessageBox.Show("올바르지 않은 적용일자가 존재 합니다." + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select"));
                    return false;
                }


                chknum = 0;
                isnum = int.TryParse(Ap_PV, out chknum);

                if (isnum == false)
                {
                    MessageBox.Show("올바르지 않은 적용금액이 존재 합니다." + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select"));
                    return false;
                }

                if (AP_Sort == "주")
                {
                    if (Min_ApDAte > int.Parse(AppDate))
                    {
                        Min_ApDAte = int.Parse(AppDate);
                    }
                }
                else
                {
                    if (Min_ApDAte_M > int.Parse(AppDate))
                    {
                        Min_ApDAte_M = int.Parse(AppDate);
                    }
                }

            }//  end for 그리드 상에서 엑셀 전환을 선택한 V 한 내역을 파악한다.


            cls_Search_DB csd = new cls_Search_DB();
            if (Min_ApDAte >0 )
            {
                if (csd.Close_Check_SellDate("tbl_CloseTotal_02", Min_ApDAte.ToString ().Replace("-", "").Trim()) == false)
                {
                    return false;
                }
            }

            if (Min_ApDAte_M > 0)
            {
                if (csd.Close_Check_SellDate("tbl_CloseTotal_04", Min_ApDAte_M.ToString ().Replace("-", "").Trim()) == false)
                {
                    return false;
                }
            }



            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                progress.Maximum = dGridView_Base.Rows.Count + 1;

                string StrSql = "Delete From  tbl_Sham_Pay_ETC  ";
                StrSql = StrSql + " Where Recordid = '" + cls_User.gid + "'";

                Temp_Connect.Insert_Data(StrSql, "tbl_Sham_Pay_ETC", Conn, tran);


                for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                {
                    Mbid2 = ""; AppDate = ""; Ap_PV = ""; AP_Sort = "";

                    Mbid2 = dGridView_Base.Rows[i].Cells[0].Value.ToString();
                    M_Name = dGridView_Base.Rows[i].Cells[1].Value.ToString().Replace("-", "");
                    AppDate = dGridView_Base.Rows[i].Cells[2].Value.ToString().Replace("-", "");
                    Ap_PV = dGridView_Base.Rows[i].Cells[3].Value.ToString().Replace(",", "");
                    AP_Sort = dGridView_Base.Rows[i].Cells[4].Value.ToString();
                    string Etc2 = dGridView_Base.Rows[i].Cells[5].Value.ToString();

                     StrSql = "INSERT INTO tbl_Sham_Pay_ETC ";
                    StrSql = StrSql + " (";
                    StrSql = StrSql + "  mbid2 , M_Name   ";
                    StrSql = StrSql + " , Apply_Date , Apply_Pv  ";
                    StrSql = StrSql + " , SortKind2 , Etc ";
                    StrSql = StrSql + " , RecordID, RecordTime ";
                    StrSql = StrSql + " ) ";
                    StrSql = StrSql + " Values ";
                    StrSql = StrSql + " (";
                    StrSql = StrSql +  Mbid2;
                    StrSql = StrSql + ",'" + M_Name + "'";
                    StrSql = StrSql + ",'" + AppDate + "'";

                    StrSql = StrSql + "," + Ap_PV;

                    if (AP_Sort == "주")
                        StrSql = StrSql + ",'W_'";
                    else
                        StrSql = StrSql + ",'M_'";

                    StrSql = StrSql + ",'" + Etc2 + "'";
                    StrSql = StrSql + ",'" + cls_User.gid + "'";
                    StrSql = StrSql + ",Convert(Varchar(25),GetDate(),120) ";
                    StrSql = StrSql + ")";

                    Temp_Connect.Insert_Data(StrSql, "tbl_Sham_Pay_ETC", Conn, tran);


                    progress.Value = progress.Value + 1;

                }//  end for 그리드 상에서 엑셀 전환을 선택한 V 한 내역을 파악한다.

                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show("엑셀내역중에 중복 내역이 존재 합니다. 확인후 다시 시도해 주십시요.");
                return false;
            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }                    


            return true;
         }




         private void dGridView_KeyDown(object sender, KeyEventArgs e)
         {
             //그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
             if (sender is DataGridView)
             {
                 if (e.KeyValue == 46)
                 {
                     e.Handled = true;
                 } // end if

             }
         }






        private Boolean Insert_Data()
        {
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return false;
            //if (Check_TextBox_Save_Error() == false) return false;

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            string StrSql = "";
            cls_form_Meth cm = new cls_form_Meth();
            try
            {
                progress.Maximum = dGridView_Base.Rows.Count + 1;

                for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = Conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Usp_Insert_TblCommissionDetail";


                    clsCommissionDetail oCommissionDetail = new clsCommissionDetail();

                    cmd.Parameters.Add("@v_ProductionMonth", SqlDbType.VarChar).Value = dGridView_Base.Rows[i].Cells[0].Value.ToString().Trim();
                    cmd.Parameters.Add("@v_AccountNumber", SqlDbType.Int).Value =  clsStaticFnc.Let_Int( dGridView_Base.Rows[i].Cells[1].Value.ToString().Trim());
                    cmd.Parameters.Add("@v_Seq", SqlDbType.Int).Value = clsStaticFnc.Let_Int(dGridView_Base.Rows[i].Cells[2].Value.ToString().Trim());
                    cmd.Parameters.Add("@v_CommissionType", SqlDbType.VarChar).Value = dGridView_Base.Rows[i].Cells[3].Value.ToString().Trim();
                    cmd.Parameters.Add("@v_Description", SqlDbType.VarChar).Value = dGridView_Base.Rows[i].Cells[4].Value.ToString().Trim();
                    cmd.Parameters.Add("@v_PaidOnAccount", SqlDbType.Int).Value = clsStaticFnc.Let_Int(dGridView_Base.Rows[i].Cells[5].Value.ToString().Trim());
                    cmd.Parameters.Add("@v_PaidOnCountryCode", SqlDbType.VarChar).Value = dGridView_Base.Rows[i].Cells[6].Value.ToString().Trim();
                    cmd.Parameters.Add("@v_PaidOnAccountName", SqlDbType.VarChar).Value = dGridView_Base.Rows[i].Cells[7].Value.ToString().Trim();
                    cmd.Parameters.Add("@v_PhysicalLevel", SqlDbType.Int).Value = clsStaticFnc.Let_Int( dGridView_Base.Rows[i].Cells[8].Value.ToString().Trim());
                    cmd.Parameters.Add("@v_PayLevel", SqlDbType.Int).Value = clsStaticFnc.Let_Int(dGridView_Base.Rows[i].Cells[9].Value.ToString().Trim());
                    cmd.Parameters.Add("@v_Volume", SqlDbType.Int).Value = clsStaticFnc.Let_Int(dGridView_Base.Rows[i].Cells[10].Value.ToString().Trim());
                    cmd.Parameters.Add("@v_CommissionPercent", SqlDbType.Decimal).Value = clsStaticFnc.Let_Double( dGridView_Base.Rows[i].Cells[11].Value.ToString().Trim());
                    cmd.Parameters.Add("@v_CommissionAmount", SqlDbType.Decimal).Value = clsStaticFnc.Let_Double( dGridView_Base.Rows[i].Cells[12].Value.ToString().Trim());
                    cmd.Parameters.Add("@v_OrderNumber", SqlDbType.VarChar).Value = dGridView_Base.Rows[i].Cells[13].Value.ToString().Trim();
                    cmd.Parameters.Add("@out_Return", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();

                    int iResult = clsStaticFnc.Let_Int(cmd.Parameters["@out_Return"].Value.ToString());


                    DataGridViewCell dgvc_Success = dGridView_Base.Rows[i].Cells[14]; //실행결과
                    DataGridViewCell dgvc_Remark = dGridView_Base.Rows[i].Cells[15]; //오류메세지


                    switch (iResult)
                    {
                        case -1:
                            dgvc_Success.Value = "실패";
                            dgvc_Remark.Value = "중복 자료가 있습니다.";
                            break;
                        case 0:
                            dgvc_Success.Value = "성공";
                            dgvc_Remark.Value = "";
                            break;
                        default:

                            break;
                                
                    }


                    progress.Value = progress.Value + 1;
                }

                tran.Commit();


                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
                return true;
            }


            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());

                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
                return false;
            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }
        }




    }
}
