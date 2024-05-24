using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using ExcelDataReader;
using System.IO;

namespace MLM_Program
{
    public partial class frm_Excel_Import_Rec : clsForm_Extends
    {
        DataSet dsExcels = new DataSet();
        private int Load_TF = 0;
        public frm_Excel_Import_Rec()
        {
            InitializeComponent();
        }


        private void btnLoad_Click(object sender, EventArgs e)
        {


            int RCnt = dGridView_Base.Rows.Count - 1;

            if (dGridView_Base.DataSource != null)
            {
                dGridView_Base.DataSource = null;
            }
            else if (RCnt > 0)
            {
                dGridView_Base.Visible = true;
            }

            dGridView_Base.Rows.Clear();
            dGridView_Base.Columns.Clear();
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

            dsExcels = new DataSet();
            var extension = Path.GetExtension(txtFilePath.Text).ToLower();
            using (var stream = new FileStream(txtFilePath.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {

                IExcelDataReader reader = null;
                if (extension == ".xls")
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (extension == ".xlsx")
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else if (extension == ".csv")
                {
                    reader = ExcelReaderFactory.CreateCsvReader(stream);
                }

                if (reader == null)
                    return;

                dsExcels = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    UseColumnDataType = false,
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

            }

            foreach (DataTable dt in dsExcels.Tables)
            {
                combo_Sheet.Items.Add(dt.TableName);
            }


            Load_TF = 1;
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

            dGridView_Base.DataSource = dsExcels.Tables[combo_Sheet.SelectedIndex];
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
                chage_Center_tf = Chang_CenterCode();  //실질적인 센터 변경이 이루어지는 메소드
                progress.Visible = false;
                this.Cursor = System.Windows.Forms.Cursors.Default;

                if (chage_Center_tf == true)
                {
                    int RCnt = dGridView_Base.Rows.Count - 1;
                    if (dGridView_Base.DataSource != null)
                        dGridView_Base.DataSource = null;
                    else if (RCnt > 0)
                    {
                        dGridView_Base.Visible = false;
                        dGridView_Base.Rows.Clear();
                        dGridView_Base.Visible = true;
                    }

                    txtFilePath.Text = "";
                    combo_Sheet.Items.Clear();
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

             txtFilePath.BackColor = cls_app_static_var.txt_Enable_Color;
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
             try
             {
                 //string T_Mbid = "";                 
                 string OrderNumber = ""; //string Out_Index = ""; int SalesItemIndex = 0;  
                 string Pass_Number = "";
                 
                 cls_Search_DB csd = new cls_Search_DB();
                 progress.Maximum = dGridView_Base.Rows.Count + 1;
                 int ord_Cnt = 0, Pas_Cnt = 0, Pas_Cnt_2 = 0, Good_Cnt = 0, SalesItemIndex = 0; 

                 for (int i = 0; i < dGridView_Base.Columns.Count ; i++)
                 {
                     if (dGridView_Base.Columns[i].HeaderText.Trim().Replace(" ", "") == "주문번호")
                         ord_Cnt = i;


                     if (dGridView_Base.Columns[i].HeaderText.Trim().Replace(" ", "") == "상품인덱스")
                         Good_Cnt = i;

                     

                     if (dGridView_Base.Columns[i].HeaderText.Trim().Replace(" ", "").Length >= 5)
                     {
                         if (dGridView_Base.Columns[i].HeaderText.Trim().Replace(" ", "").Substring(0, 5) == "운송장번호" )
                         {
                             if (Pas_Cnt == 0) Pas_Cnt = i;
                             Pas_Cnt_2 = i;
                         }
                     }

                     if (Pas_Cnt == 0)
                     {
                         if (dGridView_Base.Columns[i].HeaderText.Trim().Replace(" ", "").Length >= 4)
                         {
                             if (dGridView_Base.Columns[i].HeaderText.Trim().Replace(" ", "").Substring(0, 4) == "송장번호")
                             {
                                 if (Pas_Cnt == 0) Pas_Cnt = i;
                                 Pas_Cnt_2 = i;
                             }
                         }
                     }
                 }

                 for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                 {

                     //SalesItemIndex = 0;
                     OrderNumber = "";
                     Pass_Number = "";
                    
                    if (dGridView_Base.Rows[i].Cells[ord_Cnt].Value.ToString() !="" )
                        OrderNumber = dGridView_Base.Rows[i].Cells[ord_Cnt].Value.ToString();

                    //if (dGridView_Base.Rows[i].Cells[Good_Cnt].Value.ToString() != "")
                    //    SalesItemIndex = int.Parse (dGridView_Base.Rows[i].Cells[Good_Cnt].Value.ToString());

                     

                    for (int Cc = Pas_Cnt; Cc <= Pas_Cnt_2; Cc++)
                    {
                        if (Cc == Pas_Cnt)
                            Pass_Number = dGridView_Base.Rows[i].Cells[Cc].Value.ToString();
                        else
                        {
                            if (dGridView_Base.Rows[i].Cells[Cc].Value.ToString() != "")
                                Pass_Number = Pass_Number + " / " + dGridView_Base.Rows[i].Cells[Cc].Value.ToString();
                        }
                    }
                     
                    //Pass_Number = dGridView_Base.Rows[i].Cells[Pas_Cnt].Value.ToString();

                    if (OrderNumber != "" && Pass_Number != "")
                    {
                        StrSql = "Update tbl_SalesDetail  SET ";
                        StrSql = StrSql + " Pass_NUM = '" + Pass_Number + "'";
                        StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
                        //StrSql = StrSql + " And Pass_NUM = '' ";

                        Temp_Connect.Insert_Data(StrSql, "tbl_Sales_Rece", Conn, tran, this.Name.ToString(), this.Text);


                        //StrSql = "Update tbl_SalesDetail  SET ";
                        //StrSql = StrSql + " Pass_NUM = Pass_NUM + ' / ' + '" + Pass_Number + "'";
                        //StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
                        //StrSql = StrSql + " And Pass_NUM <> '' ";
                        //StrSql = StrSql + " And charindex('" + Pass_Number + "',Pass_NUM) = 0  ";

                        //Temp_Connect.Insert_Data(StrSql, "tbl_Sales_Rece", Conn, tran, this.Name.ToString(), this.Text);


                        StrSql = "Update tbl_Sales_Rece  SET ";
                        StrSql = StrSql + " Pass_Number = '" + Pass_Number + "'";
                        StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
                        //StrSql = StrSql + " And SalesItemIndex =" + SalesItemIndex  ; 
                        //StrSql = StrSql + " And Pass_Number = '' ";

                        Temp_Connect.Insert_Data(StrSql, "tbl_Sales_Rece", Conn, tran, this.Name.ToString(), this.Text);


                        //StrSql = "Update tbl_Sales_Rece  SET ";
                        //StrSql = StrSql + " Pass_Number = Pass_Number + ' / ' + '" + Pass_Number + "'";
                        //StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
                        ////StrSql = StrSql + " And SalesItemIndex =" + SalesItemIndex  ;
                        //StrSql = StrSql + " And Pass_Number <> '' ";
                        //StrSql = StrSql + " And charindex('" + Pass_Number + "',Pass_Number) = 0  ";

                        //Temp_Connect.Insert_Data(StrSql, "tbl_Sales_Rece", Conn, tran, this.Name.ToString(), this.Text);
                    }
                     progress.Value = progress.Value + 1;
                 }
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

             

             //for (int i = 0; i < dGridView_Base.Rows.Count; i++)
             //{
             //    if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
             //    {
             //        chk_cnt++;                  
             //    }
             //}//  end for 그리드 상에서 엑셀 전환을 선택한 V 한 내역을 파악한다.

             //if (chk_cnt == 0)
             //{
             //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Select") + "\n" +
             //          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select"));
             //    return false;
             //} //end if 체크를 해서 선택한 내역이 없을 경우 메시지 뛰우고나간다.


           

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

         private void butt_Excel_Click(object sender, EventArgs e)
         {

         }

        private void btnExcelTemplateDownload_Click(object sender, EventArgs e)
        {
            cls_Grid_Base cgb = new cls_Grid_Base();
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;




            string[] g_HeaderText = {  "주문번호", "운송장번호" };
            cgb.grid_col_header_text = g_HeaderText;
            cgb.grid_col_Count = g_HeaderText.Length;

            int[] g_Width = { 120, 120 };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true, true };
            cgb.grid_col_Lock = g_ReadOnly;


            DataGridViewContentAlignment[] g_Alignment = { DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft };
            cgb.grid_col_alignment = g_Alignment;

            cgb.d_Grid_view_Header_Reset();

            int idx = dGridView_Base.Rows.Add();
            dGridView_Base.Rows[idx].Cells[0].Value = "주문번호를 입력해주세요";
            dGridView_Base.Rows[idx].Cells[1].Value = "운송장번호를 입력해주세요";
             
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
            e_f.ShowDialog();


            dGridView_Base.Rows.Clear();
            dGridView_Base.Columns.Clear();


        }

        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Goods";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }
    }
}
