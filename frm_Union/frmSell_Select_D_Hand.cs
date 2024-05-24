using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace MLM_Program
{
    public partial class frmSell_Select_D_Hand : clsForm_Extends
    {
        

         cls_Grid_Base cgb = new cls_Grid_Base();
        private int Load_TF = 0;
        public frmSell_Select_D_Hand()
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
            
             Grid_Base_Seting();

             string ap_path = Application.StartupPath.ToString();

            if (File.Exists (txtFilePath.Text) == true )
            {         
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                FileStream fs = new FileStream(txtFilePath.Text, FileMode.Open );
                StreamReader SRR = new StreamReader(fs);

      
                try
                {   
                    int fi_cnt = 0 ;                    
                    Dictionary<int, string[]> gr_dic_text = new Dictionary<int, string[]>();

                    while (SRR.Peek () > -1 )
                    {
                        string [] t_txt =  SRR.ReadLine().ToString().Split (',') ; 

                        string[] row0 = { t_txt[0].ToString (),  t_txt[1].ToString ()  };

                        gr_dic_text[fi_cnt + 1] = row0;
                        fi_cnt ++; 
                    
                    }

                    cgb.grid_name = gr_dic_text;  //배열을 클래스로 보낸다.
                    cgb.db_grid_Data_Put();     
                 }

                catch 
                {

                }

                finally 
                 {
                     this.Cursor = System.Windows.Forms.Cursors.Default ;
                     SRR.Close();
                     fs.Close();
                 }
            }

             Load_TF = 1;
        }


         private void Grid_Base_Seting()
         {
             //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
         }


        private void dGridView_Base_Header_Reset()
        {
            dGridView_Base.RowHeadersVisible = false;
            cgb.grid_col_Count = 2;
            cgb.basegrid = dGridView_Base;


            string[] g_HeaderText = { "주문번호", "공제번호" };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 200 ,200 };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true, true };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft , 
                               DataGridViewContentAlignment.MiddleLeft                               
                              };
            cgb.grid_col_alignment = g_Alignment;
        }


         

     


         private void frmBase_Resize(object sender, EventArgs e)
         {
             int base_w = this.Width / 2;
             butt_Select.Width = base_w;
             //butt_Save.Width = base_w;
             //butt_Excel.Width = base_w;
             //butt_Delete.Width = base_w;
             butt_Exit.Width = base_w;

             butt_Select.Left = 0;
             //butt_Select.Left = butt_Clear.Left + butt_Clear.Width;
            //butt_Excel.Left = butt_Select.Left + butt_Select.Width;
             //butt_Delete.Left = butt_Excel.Left + butt_Excel.Width;
             butt_Exit.Left = butt_Select.Left + butt_Select.Width;


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
                         }
                     }
                 }// end if

             }

  
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

                    if (RCnt > 0)
                    {
                        dGridView_Base.Visible = false;
                        for (int TCnt = 0; TCnt <= RCnt - 1; TCnt++)
                            dGridView_Base.Rows.Remove(dGridView_Base.Rows[0]);
                        dGridView_Base.Visible = true;
                    }
                    
                    txtFilePath.Text = "";                    
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
             //SqlTransaction tran = Conn.BeginTransaction();

             string StrSql = "";
             cls_form_Meth cm = new cls_form_Meth();
             try
             {                                            

                StrSql = "CREATE TABLE #T2_Mem" ;
                StrSql = StrSql + "(";
                StrSql = StrSql + "OrderNumber varchar(40)  NOT NULL DEFAULT ('') ,";                
                StrSql = StrSql + "InsuranceNumber varchar(40)  NOT NULL DEFAULT ('') ";          
                StrSql = StrSql + " Primary key(OrderNumber)";
                StrSql = StrSql + ")";

                Temp_Connect.Insert_Data(StrSql, this.Name, Conn);


                 
                 string OrderNumber = "", T_Greet_Number = "";
                 cls_Search_DB csd = new cls_Search_DB();
                 progress.Maximum = dGridView_Base.Rows.Count + 1;
                 for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                 {
                    
                    OrderNumber = dGridView_Base.Rows[i].Cells[0].Value.ToString();
                    T_Greet_Number = dGridView_Base.Rows[i].Cells[1].Value.ToString();
                    

                    StrSql = "Insert into  #T2_Mem  Values ( '" + OrderNumber + "','" +  T_Greet_Number + "')" ;                    
                    Temp_Connect.Insert_Data(StrSql, this.Name, Conn);

                     progress.Value = progress.Value + 1;
                 }

                 StrSql = "Select OrderNumber , InsuranceNumber From tbl_SalesDetail (nolock)  " ;
                 StrSql = StrSql + " Where OrderNumber IN (Select OrderNumber From #T2_Mem   ) ";
                 StrSql = StrSql + " And  InsuranceNumber <> '' " ;

                 DataSet ds = new DataSet();
            
                if (Temp_Connect.Open_Data_Set(StrSql, "T2_Mem", Conn, ds, this.Name, this.Text)== false) return false ;
                
                int ReCnt = Temp_Connect.DataSet_ReCount;
                if (ReCnt > 0) //조합에서 수동으로 온건데 공제번호가 있네 그럼 알려야지요..
                {
                    MessageBox.Show( "화면의 주문 번호 내역중에 공제번호가 존재하는 주문번호가 있습니다. 조합에 문의해 주십시요.주문번호 : " +  ds.Tables["T2_Mem"].Rows[0]["OrderNumber"].ToString()) ;
                    return false;
                }
                else
                {
                    StrSql = "Update tbl_SalesDetail SET" ;
                    StrSql = StrSql +" InsuranceNumber =ISNULL(B.InsuranceNumber,'')";                  
                    StrSql = StrSql +"  FROM  tbl_SalesDetail  A,";
    
                    StrSql = StrSql +" (";
                    StrSql = StrSql +" Select   OrderNumber , InsuranceNumber ";
                    StrSql = StrSql +"  From #T2_Mem    ";
                    StrSql = StrSql +" ) B";
                    StrSql = StrSql +" Where a.OrderNumber = b.OrderNumber ";                    
    
                    Temp_Connect.Insert_Data(StrSql, this.Name, Conn);


                    StrSql = "Insert  into  tbl_Sales_Insu " ;
                    StrSql = StrSql +" (Send_Flag, OrderNumber , TotalPrice , InsuranceNumber,RecordID, RecordTime )";                          
                    StrSql = StrSql +"  Select 'H' ,  OrderNumber , 0 ,  InsuranceNumber , '" + cls_User.gid  + "', Convert(Varchar(25),GetDate(),21) " ;
                    StrSql = StrSql +"  From #T2_Mem    ";
                    
                    Temp_Connect.Insert_Data(StrSql, this.Name, Conn);
                    
                }
                

                 MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
                 return true;
             }


             catch (Exception)
             {
                 
                 MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
                 return false;
             }

             finally
             {                 
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






    }
}
