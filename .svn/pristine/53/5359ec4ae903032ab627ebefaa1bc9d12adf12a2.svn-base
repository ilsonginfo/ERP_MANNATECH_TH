using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLM_Program
{
    public partial class frmBase_Goods_Sort_2 : Form
    {
        cls_Grid_Base cgb = new cls_Grid_Base();

        cls_Grid_Base cgb_Pop = new cls_Grid_Base();

        private const string base_db_name = "tbl_MakeItemCode2";
        private int Data_Set_Form_TF; 


        public frmBase_Goods_Sort_2()
        {
            InitializeComponent();
        }


        


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Base_Grid_Set();
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            Data_Set_Form_TF = 0;

            txtCode.MaxLength = cls_app_static_var.Item_Sort_1_Code_Length;
            txtData.MaxLength = cls_app_static_var.Item_Sort_2_Code_Length;
            ////txtCode2.BackColor = cls_app_static_var.txt_Enable_Color; 

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

                           // cls_form_Meth cfm = new cls_form_Meth();
                           // cfm.form_Group_Panel_Enable_False(this);
                        }
                    }
                }// end if

            }

            //그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
            if (sender is DataGridView)
            {
               
                if (e.KeyValue == 13)
                {
                    dGridView_Base_DoubleClick(sender, e);
                }
            }

            Button T_bt = butt_Exit;
            if (e.KeyValue == 123)
                T_bt = butt_Exit;    //닫기  F12
            if (e.KeyValue == 113)
                T_bt = butt_Save;     //저장  F1
            if (e.KeyValue == 115)
                T_bt = butt_Delete;   // 삭제  F4
            if (e.KeyValue == 119)
                T_bt = butt_Excel;    //엑셀  F8    
            if (e.KeyValue == 112)
                T_bt = butt_Clear;    //엑셀  F5    

            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123 || e.KeyValue == 113 || e.KeyValue == 115 || e.KeyValue == 119 || e.KeyValue == 112)
                    cmdSave_Click(T_bt, ee1);
            }
        }



        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Save.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_Excel.Left = butt_Save.Left + butt_Save.Width + 2;
            butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Save);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);
        }



        private void Base_Grid_Set()
        {
                           
            
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;

            Tsql = "Select tbl_MakeItemCode2.ItemCode , tbl_MakeItemCode2.ItemName  ";
            Tsql = Tsql + " , UpitemCode , ISnull(tbl_MakeItemCode1.ItemName,'') Up_Name ";
            Tsql = Tsql + " From tbl_MakeItemCode2 (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_MakeItemCode1 (nolock) ON  tbl_MakeItemCode1.ItemCode = tbl_MakeItemCode2.UpitemCode";
            Tsql = Tsql + " Order by tbl_MakeItemCode2.UpitemCode , tbl_MakeItemCode2.ItemCode ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            
            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, string[]> gr_dic_text = new Dictionary<int, string[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt-1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb.grid_name = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Data_Put();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
     
            
        }

        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 5;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;

            string[] g_HeaderText = { "대분류_코드" , "대분류명"   , "중분류_코드"  ,"중분류명"   , ""                                       
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 150, 100, 150, 0                               
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                   };
            cgb.grid_col_Lock = g_ReadOnly;
     
            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter , 
                               DataGridViewContentAlignment.MiddleCenter, 
                               DataGridViewContentAlignment.MiddleCenter,  
                               DataGridViewContentAlignment.MiddleCenter ,
                               DataGridViewContentAlignment.MiddleRight  //5
                               //DataGridViewContentAlignment.MiddleCenter ,                               
                               //DataGridViewContentAlignment.MiddleCenter,
                               //DataGridViewContentAlignment.MiddleCenter,
                               //DataGridViewContentAlignment.MiddleRight 
                              };
            cgb.grid_col_alignment = g_Alignment;            
        }

       


                

        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, string[]> gr_dic_text, int fi_cnt)
        {
            string[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt]["UpitemCode"].ToString() , 
                                ds.Tables[base_db_name].Rows[fi_cnt]["Up_Name"].ToString() ,  
                                ds.Tables[base_db_name].Rows[fi_cnt]["ItemCode"].ToString() , 
                                ds.Tables[base_db_name].Rows[fi_cnt]["ItemName"].ToString()  
                                //ds.Tables[base_db_name].Rows[fi_cnt][4].ToString() , 
                                //ds.Tables[base_db_name].Rows[fi_cnt][5].ToString() , 
                                //ds.Tables[base_db_name].Rows[fi_cnt][6].ToString() ,
                                //ds.Tables[base_db_name].Rows[fi_cnt][7].ToString() ,
                                //ds.Tables[base_db_name].Rows[fi_cnt][8].ToString() ,
                                 };

            gr_dic_text[fi_cnt+1] = row0;
        }








        //private void dGridView_Base_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        //{

        //    //Header인지 확인
        //    if (e.ColumnIndex < 0 & e.RowIndex >= 0)
        //    {
        //        e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

        //        //행 번호를 표시할 범위를 결정
        //        Rectangle indexRect = e.CellBounds;
        //        indexRect.Inflate(-2, -2);
        //        //행번호를 표시
        //        TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
        //                              e.CellStyle.Font, indexRect, e.CellStyle.ForeColor,
        //                              TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        //        e.Handled = true;
        //    }
        //}



        private void txtData_Enter(object sender, EventArgs e)
        {         
            cls_Check_Text T_R = new cls_Check_Text();
            T_R.Text_Focus_All_Sel((TextBox)sender);
            TextBox tb = (TextBox)sender;
            if (tb.ReadOnly == false)
                tb.BackColor = cls_app_static_var.txt_Focus_Color;  //Color.FromArgb(239, 227, 240);   

            if (this.Controls.ContainsKey("Popup_gr"))
            {
                DataGridView T_Gd = (DataGridView)this.Controls["Popup_gr"];
                T_Gd.Visible = false;
                T_Gd.Dispose();
            }
        }

        private void txtData_Base_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.ReadOnly == false)
                tb.BackColor = Color.White;
        }

        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            
            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);            
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);

            TextBox tb = (TextBox)sender;

            if (T_R.Text_KeyChar_Check(e) == false)
            {
                e.Handled = true;
                return;
            } // end if   

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "ncode")) //코드관련해서 코드를치면 관련 내역이 나오도록 하기 위함.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, tb) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }
        }


        private void txtData_TextChanged(object sender, EventArgs e)
        {
            int Sw_Tab = 0;
            if (Data_Set_Form_TF == 1) return;

            TextBox tb = (TextBox)sender;
            //if (tb.TextLength >= tb.MaxLength)
            //{
            //    SendKeys.Send("{TAB}");
            //    Sw_Tab = 1;
            //}

            if (tb.Name == "txtCode") 
            {
                if (tb.Text.Trim() == "")
                    txtCode2.Text = "";
                else if (Sw_Tab ==1)                     
                    Form_Refresh_Data(txtCode);
            }
        }


        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {

            if (tb.Text.Trim() != "")
            {
                if (tb.Name == "txtData")
                {
                    Data_Set_Form_TF = 1;
                    if (txtCode.Text != "")
                        Form_Refresh_Data(tb.Text, txtCode.Text);
                    Data_Set_Form_TF = 0;
                }
                else if (tb.Name == "txtCode")
                {
                    Data_Set_Form_TF = 1;
                    if (tb.Text != "")
                        Form_Refresh_Data(txtCode);
                    Data_Set_Form_TF = 0;
                }

            }
            else
            {
                if (tb.Name == "txtCode")
                {
                    Data_Set_Form_TF = 1;
                    if (tb.Text == "")
                        Db_Grid_Popup("");
                    Data_Set_Form_TF = 0;
                }
            }
        }
        
        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


        
        private void Db_Grid_Popup( string strSql )
        {
            cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            DataGridView Popup_gr = new DataGridView();
            Popup_gr.Name = "Popup_gr";
            this.Controls.Add(Popup_gr);
            cgb_Pop.basegrid = Popup_gr;
            cgb_Pop.Base_fr = this;
            cgb_Pop.Base_tb = txtCode;
            cgb_Pop.Base_tb_2  = txtCode2;
            cgb_Pop.Base_Location_obj = txtCode;

            if (strSql != "")
            {
                cgb_Pop.db_grid_Popup_Base(2, "대분류_코드", "대분류명", "ItemCode", "ItemName", strSql);
                cgb_Pop.Next_Focus_Control = txtData;
            }
            else
            {
                string Tsql;
                Tsql = "Select ItemCode , ItemName  ";
                Tsql = Tsql + " From tbl_MakeItemCode1 (nolock) ";
                Tsql = Tsql + " Order by ItemCode ";

                cgb_Pop.db_grid_Popup_Base(2, "대분류_코드", "대분류명", "ItemCode", "ItemName", Tsql);
                cgb_Pop.Next_Focus_Control = txtData;
            }           
        }

   



        


        

  



        private Boolean Check_TextBox_Error()
        {      
            cls_Check_Text T_R = new cls_Check_Text();

            string me = T_R.Text_Null_Check(txtData);
            if (me != "")
            {
                MessageBox.Show(me);         
                return false;
            }

            me = T_R.Text_Null_Check(txtData2);
            if (me != "")
            {
                MessageBox.Show(me);                
                return false;
            }

            if (txtData.MaxLength != txtData.Text.Trim().Length)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Code") + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtData.Focus();
                return false;
            }
            
            if (txtKey.Text.Trim() == "")
            {
                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                string Tsql;
                Tsql = "Select ItemCode, ItemName ";
                Tsql = Tsql + " From tbl_MakeItemCode2  (nolock)  ";
                Tsql = Tsql + " Where ItemCode = '" + txtData.Text.Trim() + "'";
                //Tsql = Tsql + " And   UpitemCode = '" + txtCode.Text.Trim() + "'";
                Tsql = Tsql + " Order by ItemCode ASC ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_Code") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txtData.Focus ();                     return false;
                }


                Tsql = "Select ItemCode, ItemName ";
                Tsql = Tsql + " From tbl_MakeItemCode2  (nolock)  ";
                Tsql = Tsql + " Where ItemName = '" + (txtData2.Text).Trim() + "'";
                //Tsql = Tsql + " And   UpitemCode = '" + txtCode.Text.Trim() + "'";
                Tsql = Tsql + " Order by ItemCode ASC ";

                ds.Clear();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_Name") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txtData2.Focus();
                    return false;
                }

                //++++++++++++++++++++++++++++++++
            }
            else
            {
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                string Tsql;
                Tsql = "Select ItemCode, ItemName ";
                Tsql = Tsql + " From tbl_MakeItemCode2  (nolock)  ";
                Tsql = Tsql + " Where upper(ItemCode) <> '" + ((txtData.Text).Trim()).ToUpper() + "'";
                Tsql = Tsql + " And  ItemName = '" + (txtData2.Text).Trim() + "'";
                Tsql = Tsql + " Order by ItemCode ASC ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
                {
                    
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_Name") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action") );
                    txtData2.Focus();
                    return false;
                }
            }
                        
            return true;
        }
        private void from_All_Clear_()
        {
            Base_Grid_Set();


            txtData.BackColor = SystemColors.Window;
            txtData.ReadOnly = false;
            txtData.BorderStyle = BorderStyle.Fixed3D;

            txtCode.BackColor = SystemColors.Window;
            txtCode.ReadOnly = false;
            txtCode.BorderStyle = BorderStyle.Fixed3D;

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, txtCode);
        }


        private void cmdSave_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            if (bt.Name == "butt_Clear")
            {

                from_All_Clear_();

            }
            else if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Save_Base_Data(ref Save_Error_Check); //저장 수정이 이루어지는 곳

                if (Save_Error_Check > 0)
                {
                    from_All_Clear_();
                }

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            
            else if (bt.Name == "butt_Delete")
            {
                int Del_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Delete_Base_Data(ref Del_Error_Check);  //삭제가 이루어 지는곳

                if (Del_Error_Check > 0)
                {
                    from_All_Clear_();
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            else if (bt.Name == "butt_Excel")
            {
                frmBase_Excel e_f = new frmBase_Excel();
                e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
                e_f.ShowDialog();
            }

            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }

        }


        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Goods_Sort_2";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }



        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
             if (Check_TextBox_Error() == false) return;                        

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string Tsql;
            Tsql = "Select ItemCode, ItemName ";
            Tsql = Tsql + " From tbl_MakeItemCode2   (nolock) ";
            Tsql = Tsql + " Where upper(ItemCode) = '" + ((txtData.Text).Trim()).ToUpper() + "'";
            Tsql = Tsql + " And   UpitemCode = '" + txtCode.Text.Trim() + "'";
            Tsql = Tsql + " Order by ItemCode ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)//동일한 은행 코드가없네 그럼 인설트
            {
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

                Tsql = "Insert Into tbl_MakeItemCode2 ";
                Tsql = Tsql + " ( ";
                Tsql = Tsql + " ItemCode , ItemName, UpitemCode , UpitemName , recordid, recordTime ) " ;
                Tsql = Tsql + " Values ( ";
                Tsql = Tsql + "'" + txtData.Text.Trim() + "'";
                Tsql = Tsql + ",'" + txtData2.Text.Trim() + "'" ;
                Tsql = Tsql + ",'" + txtCode.Text.Trim() + "'";
                Tsql = Tsql + ",'" + txtCode2.Text.Trim() + "'";
                Tsql = Tsql + ",'" + cls_User.gid + "'" ;
                Tsql = Tsql + " , Convert(Varchar(25),GetDate(),21) ";
                Tsql = Tsql + ") ";

                if (Temp_Connect.Insert_Data( Tsql, base_db_name,this.Name.ToString (), this.Text ) == false) return;

                Save_Error_Check =1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));

            }
            else //동일한 은행 코드가 있구나 그럼 업데이트
            {
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

                Tsql = "Update tbl_MakeItemCode2 Set ";
                Tsql = Tsql + " ItemName = '" + txtData2.Text.Trim() + "'";                
                Tsql = Tsql + " WHERE ItemCode = '" + txtData.Text.Trim() + "'";
                Tsql = Tsql + " And   UpitemCode = '" + txtCode.Text.Trim() + "'";

                if (Temp_Connect.Update_Data(Tsql, this.Name.ToString(), this.Text) == false) return;

                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
                 
            }                       
            
        }




        private void Delete_Base_Data(ref int Del_Error_Check)
        {
            Del_Error_Check = 0;
            if (Check_TextBox_Error(1) == false) return;

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string Tsql;
            Tsql = "Delete From tbl_MakeItemCode2 ";
            Tsql = Tsql + " Where ItemCode = '" + txtData.Text.Trim() + "'";
            Tsql = Tsql + " And   UpitemCode = '" + txtCode.Text.Trim() + "'";

            if (Temp_Connect.Delete_Data (Tsql, base_db_name, this.Name.ToString(), this.Text) == false) return;

            Del_Error_Check = 1;
            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del"));
        }



        private Boolean Check_TextBox_Error(int i)
        {
            if (txtKey.Text.Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select"));
                dGridView_Base.Select();
                return false;
            }

            if (txtKey_1.Text.Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select"));
                dGridView_Base.Select();
                return false;
            }

            
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string Tsql;
            ////대분류 존재 여부 확인.
            //Tsql = "Select ItemCode , ItemName  ";
            //Tsql = Tsql + " From tbl_MakeItemCode1 (nolock) ";
            //Tsql = Tsql + " Where ItemCode = '" + txtCode.Text.Trim()  + "'";
            //Tsql = Tsql + " Order by ItemCode ASC ";

            DataSet ds = new DataSet();
            ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //if (Temp_Connect.Open_Data_Set(Tsql, "tbl_MakeItemCode1", ds) == false) return false;
            //if (Temp_Connect.DataSet_ReCount != 0)//대분류에 존재하는 코드인지 확인한다.
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Select")
            //        + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods_Sort_1")
            //        + "\n" +
            //        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    txtCode.Focus(); return false;
            //}
            //else
            //    txtCode2.Text = ds.Tables["tbl_MakeItemCode1"].Rows[0]["ItemName"].ToString();



            //상품 코드 만드는 기준이 대분류 + 중분류 + 소분류 이기 때문에 
            Tsql = "Select Ncode ";
            Tsql = Tsql + " From tbl_Goods (nolock) ";
            Tsql = Tsql + " Where up_itemcode = '" + txtCode.Text.Trim() + txtData.Text.Trim() + "'";
            Tsql = Tsql + " Order by Ncode ASC ";
                        
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            if (Temp_Connect.DataSet_ReCount != 0)//동일한 은행 코드가 있다 그럼.이거 저장하면 안되요
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Data")
                    + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods")
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtData.Focus();         return false;
            }
            
            return true ;
        }






        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {

            DataGridView T_Gd = (DataGridView)sender;

            if (T_Gd.Name == "dGridView_Base")
            {
                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, txtData);


                //if ((sender as DataGridView).CurrentRow.Cells[0].Value != null)
                if (T_Gd.CurrentRow.Cells[0].Value != null)
                {
                    Data_Set_Form_TF = 1;
                    string UpCode = T_Gd.CurrentRow.Cells[0].Value.ToString();
                    string t_ncode = T_Gd.CurrentRow.Cells[2].Value.ToString();
                    Form_Refresh_Data(t_ncode, UpCode);
                    Data_Set_Form_TF = 0;
                }
            }
        }




        private void Form_Refresh_Data(string ncode, string UpCode )
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;

            Tsql = "Select tbl_MakeItemCode2.ItemCode , tbl_MakeItemCode2.ItemName  ";
            Tsql = Tsql + " , UpitemCode , ISnull(tbl_MakeItemCode1.ItemName,'') Up_Name ";
            Tsql = Tsql + " From tbl_MakeItemCode2 (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_MakeItemCode1 (nolock) ON  tbl_MakeItemCode1.ItemCode = tbl_MakeItemCode2.UpitemCode";
            Tsql = Tsql + " Where tbl_MakeItemCode2.ItemCode = '" + ncode + "'";
            Tsql = Tsql + " And   UpitemCode = '" + UpCode + "'";
            
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            txtKey.Text = ds.Tables[base_db_name].Rows[0]["ItemCode"].ToString();
            txtKey_1.Text = ds.Tables[base_db_name].Rows[0]["UpitemCode"].ToString();

            txtData.Text = ds.Tables[base_db_name].Rows[0]["ItemCode"].ToString();
            txtData2.Text = ds.Tables[base_db_name].Rows[0]["ItemName"].ToString();

            txtCode.Text = ds.Tables[base_db_name].Rows[0]["UpitemCode"].ToString();
            txtCode2.Text = ds.Tables[base_db_name].Rows[0]["Up_Name"].ToString();

            //더블클릭이나 상품 정보를 불러온 상태에선느 상품 코드의 변경이 안일어 나게 하기 위해서 상품 코드 텍스트를 락시킨다
            //추후 위의 새로 입력 버튼으로 풀수 있음.
            txtData.BackColor = cls_app_static_var.txt_Enable_Color ;
            txtData.ReadOnly = true;
            txtData.BorderStyle = BorderStyle.FixedSingle;

            txtCode.BackColor = cls_app_static_var.txt_Enable_Color;
            txtCode.ReadOnly = true;

            txtCode.BorderStyle = BorderStyle.FixedSingle; 

            txtData2.Focus();

        }


        private void Form_Refresh_Data(TextBox tb)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;

            Tsql = "Select ItemCode , ItemName  ";            
            Tsql = Tsql + " From tbl_MakeItemCode1 (nolock) ";
            Tsql = Tsql + " Where ItemCode like '%" + tb.Text.Trim() + "%'";
            Tsql = Tsql + " OR    ItemName like '%" + tb.Text.Trim() + "%'";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 1)
            {
                txtCode.Text = ds.Tables[base_db_name].Rows[0]["ItemCode"].ToString();
                txtCode2.Text = ds.Tables[base_db_name].Rows[0]["ItemName"].ToString();
            }

            if ((ReCnt > 1) || (ReCnt == 0))               Db_Grid_Popup(Tsql);
        }
    

    }
}
