﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;

namespace MLM_Program
{
    public partial class frmBase_Goods : Form
    {
        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_2 = new cls_Grid_Base();
        private const string base_db_name = "tbl_Goods";
        private int Data_Set_Form_TF;
        private int FormLoad_TF = 0; 

        Dictionary<string, TreeNode> dic_Tree_Sort_1 = new Dictionary<string, TreeNode>();  //상품 코드 분류상 대분류 관련 트리노드를 답는곳
        Dictionary<string, TreeNode> dic_Tree_Sort_2 = new Dictionary<string, TreeNode>();  //상품 코드 분류상 중분류 관려련 트리 노드를 답는곳

        public frmBase_Goods()
        {
            InitializeComponent();
        }

              

       

        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            FormLoad_TF = 0;

            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            dGridView_Base_2_Header_Reset();
            cgb_2.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            Data_Set_Form_TF = 0;



            //상품코드 자리수에 맞추어 텍스트 박스 길이 셋팅
            if (cls_app_static_var.Item_Sort_1_Code_Length == 0)
            {
                txtNcode.Width = txtNcode.Width + txtUp.Width + 4;
                //txtNcode.MaxLength = cls_app_static_var.Item_Code_Length;
                //grB_G_Tree.Visible = false;
                trv_Item.Visible = false;
                this.Refresh();
            }
            else
            {
                //if (cls_app_static_var.Item_Sort_1_Code_Length >0 )
                //    txtNcode.MaxLength = cls_app_static_var.Item_Sort_1_Code_Length;

                //if (cls_app_static_var.Item_Sort_2_Code_Length > 0)
                //    txtNcode.MaxLength = cls_app_static_var.Item_Sort_2_Code_Length;

                //if (cls_app_static_var.Item_Sort_3_Code_Length > 0)
                //    txtNcode.MaxLength = cls_app_static_var.Item_Sort_3_Code_Length;

                txtUp.Visible = true;
                //txtUp.MaxLength = cls_app_static_var.Item_Sort_1_Code_Length
                //                + cls_app_static_var.Item_Sort_2_Code_Length
                //                + cls_app_static_var.Item_Sort_3_Code_Length - txtNcode.MaxLength;

                
                //grB_G_Tree.Visible = true;
                trv_Item.Visible = true;
                this.Refresh();
                trv_Item_Set_Sort_Code();
                //Base_Grid_Set();
                
            }

            ////txtUp.BackColor = cls_app_static_var.txt_Enable_Color;
            ////txtP2.BackColor = cls_app_static_var.txt_Enable_Color;

            mtxtApplyDate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtRegDate.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtRegDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }


        private void frm_Base_Activated(object sender, EventArgs e)
        {
            this.Refresh();
            if (FormLoad_TF == 0)
            {
                FormLoad_TF = 1;
                Base_Grid_Set();
               
            }

        }

        private void trv_Item_Set_Sort_Code()
        {

            

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string StrSql = ""; string T_base_db_name = "";
            string ItemName = ""; string ItemCode = "";
            
            DataSet ds = new DataSet();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>대분류 관련된 내역을 트리뷰에 넣는다
            T_base_db_name = "tbl_MakeItemCode1";

            StrSql = "Select ItemCode,ItemName From tbl_MakeItemCode1 ";
            StrSql = StrSql + " Order by ItemCode" ;
            
            
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, T_base_db_name, ds) == false) return;
            if (Temp_Connect.DataSet_ReCount == 0) return;

            
            trv_Item.Nodes.Clear();
            trv_Item.CheckBoxes = true;           

            for (int RowCnt = 0; RowCnt < Temp_Connect.DataSet_ReCount; RowCnt++)
            {
                ItemName = ds.Tables[T_base_db_name].Rows[RowCnt]["ItemName"].ToString();
                ItemCode = ds.Tables[T_base_db_name].Rows[RowCnt]["ItemCode"].ToString();

                TreeNode tn = trv_Item.Nodes.Add(ItemName + " - " + ItemCode );
                dic_Tree_Sort_1[ItemCode] = tn;               
            }
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<대분류 관련된 내역을 트리뷰에 넣는다

            

            //소분류 까지 코드가 들어 와 잇다는 거는 중분류를 쓴다는 거기 때문에 중분류 코드도 넣어준다.
            if (cls_app_static_var.Item_Sort_3_Code_Length == 0) return;

            


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>준분류 관련된 내역을 트리뷰에 넣는다
            string UpitemCode = "";

            T_base_db_name = "tbl_MakeItemCode2";

            StrSql = "Select ItemCode,ItemName, UpitemCode From tbl_MakeItemCode2 ";
            StrSql = StrSql + " Order by UpitemCode, ItemCode ";
            
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, T_base_db_name, ds) == false) return;
            if (Temp_Connect.DataSet_ReCount == 0) return;


            for (int RowCnt = 0; RowCnt < Temp_Connect.DataSet_ReCount; RowCnt++)
            {
                ItemName = ds.Tables[T_base_db_name].Rows[RowCnt]["ItemName"].ToString();
                ItemCode = ds.Tables[T_base_db_name].Rows[RowCnt]["ItemCode"].ToString();
                UpitemCode = ds.Tables[T_base_db_name].Rows[RowCnt]["UpitemCode"].ToString();


                if (dic_Tree_Sort_1 != null)
                {
                    
                    if (dic_Tree_Sort_1.ContainsKey (UpitemCode))                    
                    {
                        TreeNode tn2 = dic_Tree_Sort_1[UpitemCode];

                        if (tn2 != null)
                        {
                            TreeNode node2 = new TreeNode(ItemName + " - " + ItemCode);
                            tn2.Nodes.Add(node2);
                            tn2.Expand();
                            dic_Tree_Sort_2[UpitemCode + ItemCode] = node2;                                              
                        }
                    }
                    
                }
                
            }

            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<중분류 관련된 내역을 트리뷰에 넣는다

            
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





        private void frmBase_From_KeyDown(object sender, KeyEventArgs e)
        {            
            ////폼일 경우에는 ESC버튼에 폼이 종료 되도록 한다
            //if (sender is Form)
            //{
            //    if (e.KeyCode == Keys.Escape)
            //    {
            //        this.Close();
            //    }// end if

            //}

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
                            //cfm.form_Group_Panel_Enable_True(this);
                        }
                    }
                }// end if
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
                    Base_Button_Click(T_bt, ee1);
            }
        }



        private void Base_Grid_Set(string Ncode = "")
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;  

            Tsql = "Select tbl_Goods.Ncode  ";
            Tsql = Tsql + " , tbl_Goods.name  ";
            Tsql = Tsql + " , tbl_Goods.name_E  ";
            Tsql = Tsql + " , inspection  ";

            Tsql = Tsql + " , Isnull (( Select Top 1 price1 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode  And Na_code = '' Order by ApplyDate DESC   ),price) Last_price ";
            Tsql = Tsql + " , Isnull (( Select Top 1 price2 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode  And Na_code = '' Order by ApplyDate DESC   ),price2) Last_price2 ";
            //5

            Tsql = Tsql + " , Isnull (( Select Top 1 Except_Sell_VAT_Price From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode  And Na_code = '' Order by ApplyDate DESC   ),Except_Sell_VAT_Price) Last_Except_Sell_VAT_Price ";
            Tsql = Tsql + " , Isnull (( Select Top 1 Sell_VAT_Price From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode  And Na_code = '' Order by ApplyDate DESC   ),Sell_VAT_Price) Last_Sell_VAT_Price ";
            Tsql = Tsql + " , Isnull (( Select Top 1 price4 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode  And Na_code = '' Order by ApplyDate DESC   ),price4) Last_price4 ";
            Tsql = Tsql + " , Isnull (( Select Top 1 price5 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode  And Na_code = '' Order by ApplyDate DESC   ),price5) Last_price5 ";
            Tsql = Tsql + " , Isnull (( Select Top 1 price6 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode  And Na_code = '' Order by ApplyDate DESC   ),price6) Last_price6 ";
            //10

            //Tsql = Tsql + " , price ";
            //Tsql = Tsql + " , price2 ";  //5
            //Tsql = Tsql + " , Except_Sell_VAT_Price ";
            //Tsql = Tsql + " , Sell_VAT_Price ";
            //Tsql = Tsql + " , price4 ";
            //Tsql = Tsql + " , price5 ";
            //Tsql = Tsql + " , price6  "; //10

            Tsql = Tsql + " , Case GoodUse When 0 then '사용' When 1 then '사용안함' END " ;
            Tsql = Tsql + " , LEFT(Item_RegTime,4) +'-' + LEFT(RIGHT(Item_RegTime,4),2) + '-' + RIGHT(Item_RegTime,2) ";
            Tsql = Tsql + " , P_Code ";
            Tsql = Tsql + " , Isnull(tbl_purchase.name, '') AS PName ";
            Tsql = Tsql + " , Case Sell_VAT_TF When 0 then '과세' When 1 then '면세' END   "; //15
            Tsql = Tsql + " , T_ETC ";

            Tsql = Tsql + " , Isnull (( Select Top 1 price7 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode  And Na_code = '' Order by ApplyDate DESC   ),price7) Last_price7 ";
            Tsql = Tsql + " , Isnull (( Select Top 1 price8 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode  And Na_code = '' Order by ApplyDate DESC   ),price8) Last_price8 ";

            Tsql = Tsql + " From tbl_Goods (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_purchase  (nolock) ON tbl_purchase.Ncode = tbl_Goods.P_Code ";

            if (Ncode != "")
            {
                Tsql = Tsql + " Where tbl_Goods.ncode Like '%" + Ncode.Trim() + "%'";
                Tsql = Tsql + " OR  tbl_Goods.name Like '%" + Ncode.Trim() + "%'"; 
            }

            Tsql = Tsql + " Order by tbl_Goods.Ncode ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }



        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 19;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = { "상품_코드" , "상품명" , "영문명" , "상품_규격"  , "소비자가"   
                                    , "회원가"   , "공급가"   , "VAT"   ,"PV" , "_CV" 
                                     , "_직원가", "사용여부" , "등록일" ,"매입처_코드" , "매입처명" 
                                     , "면_과세", "비고" ,"총판가" ,"총판PV"
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
                        
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 -1] = cls_app_static_var.str_Grid_Currency_Type;

            gr_dic_cell_format[18 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[19 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb.grid_cell_format  = gr_dic_cell_format;

            int[] g_Width = { 90, 250, 100, 80, 80  
                             , 80  ,80, 70, 70, 0
                            , 0   ,80, 95, 95, 150 
                             , 90,500, 95, 95
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true   
                                    ,true,true   ,true,true  
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleCenter    //5
                               ,DataGridViewContentAlignment.MiddleRight 

                               ,DataGridViewContentAlignment.MiddleRight                          
                               ,DataGridViewContentAlignment.MiddleRight                                
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight  //10
                               ,DataGridViewContentAlignment.MiddleRight 

                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleCenter                                
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //15
                               ,DataGridViewContentAlignment.MiddleCenter 

                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               

                              };
            cgb.grid_col_alignment = g_Alignment;
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]                                
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][7]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][10]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][11]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][12]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][13]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][14]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][15]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][16]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][17]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][18]
                                 };

            gr_dic_text[fi_cnt + 1] = row0;

        }
        



        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(grB_Data, txtNcode);
            optUse.Checked = true;

            if ((sender as DataGridView).CurrentRow.Cells[0].Value != null)
            {
                Data_Set_Form_TF = 1;
                DataGridView T_Gd = (DataGridView)sender;
                string t_ncode = T_Gd.CurrentRow.Cells[0].Value.ToString();
                Form_Refresh_Data_002(t_ncode);  
                Form_Refresh_Data(t_ncode);
                Data_Set_Form_TF = 0;
            }
        }

        

        private void MtxtData_KeyPress(object sender, KeyPressEventArgs e)
        {

            //if (e.KeyChar == 13)
            //{
            //    if (mtxtMbid.Text.Trim() != "")
            //    {
            //        int reCnt = 0;
            //        cls_Search_DB cds = new cls_Search_DB();
            //        string Search_Name = "";
            //        reCnt = cds.Member_Name_Search(mtxtMbid.Text, ref Search_Name);

            //        if (reCnt == 1)
            //        {
            //            txtMemberName.Text = Search_Name;
            //        }
            //        else if (reCnt > 1)  //회원번호 비슷한 사람들이 많은 경우
            //        {
            //            string Mbid = "";
            //            int Mbid2 = 0;
            //            cds.Member_Nmumber_Split(mtxtMbid.Text,ref Mbid,ref Mbid2);

            //            cls_app_static_var.Search_Member_Number_Mbid = Mbid;
            //            cls_app_static_var.Search_Member_Number_Mbid2 = Mbid2;
            //            Form e_f = new frmBase_Member_Search();
            //            e_f.ShowDialog();

            //            txtMemberName.Text = cls_app_static_var.Search_Member_Name_Return;
            //            mtxtMbid.Text = cls_app_static_var.Search_Member_Number_Return;     
            //        }
            //    }

            //    SendKeys.Send("{TAB}");
            //}
        }



        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {
            //if (mtxtMbid.Text.Replace("_", "").Replace("-", "").Replace(" ", "") == "")
            //{
            //    txtMemberName.Text = "";
            //}
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

                if (tb.Tag != null)
                {
                    if (tb.Tag.ToString() == "2" && tb.Text != "")
                    {
                        Data_Set_Form_TF = 1;
                        double T_p = double.Parse(tb.Text.Replace(",", "").ToString());
                        tb.Text = string.Format(cls_app_static_var.str_Currency_Type, T_p);
                        Data_Set_Form_TF = 0;
                    }
                }
            }

            if (sender is MaskedTextBox)
            {
                MaskedTextBox tb = (MaskedTextBox)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = Color.White;
            }

            
        }
       


        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
            T_R.Key_Enter_13_Name += new Key_13_Name_Event_Handler(T_R_Key_Enter_13_Name);
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);
            

            TextBox tb  = (TextBox)sender;

            if ((tb.Tag == null) || (tb.Tag.ToString () == ""))
            {
                //숫자만 입력 가능하다.
                if (T_R.Text_KeyChar_Check(e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if   
            }
            else if (tb.Tag.ToString () == "1") //숫자관련된 사항만 받아들이도록 셋팅을 함. 날짜 관련해서
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e,1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if (tb.Tag.ToString() == "2") //숫자관련된 사항만 받아들이도록 셋팅을 함.  순수 계산식의 숫자 관련해서
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if (tb.Tag.ToString () == "name") //회원명과 관련해서 회원명 텍스트 박스를 관련해서 별도로 키프레스를 조정하기 위함.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(tb,e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if (tb.Tag.ToString() == "ncode") //코드관련해서 코드를치면 관련 내역이 나오도록 하기 위함.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e,tb) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }


                                    
        }



        private void MtxtData_Temp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    string Sn = mtb.Text.Replace("-", "").Replace("_", "").Trim();
                    if (mtb.Name == "mtxtBiz1")
                    {
                        if (Sn_Number_(Sn, mtb, "biz") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel1")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel2")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtZip1")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    string R4_name = mtb.Name.Substring(mtb.Name.Length - 4, 4);
                    if (R4_name == "Date" || R4_name == "ate3" || R4_name == "ate1" || R4_name == "ate2" || R4_name == "ate4")
                    {
                        if (Sn_Number_(Sn, mtb, "Date") == true)
                            SendKeys.Send("{TAB}");
                    }                    
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
                    string[] date_a = mtb.Text.Split('-');

                    if (date_a.Length >= 3 && date_a[0].Trim() != "" && date_a[1].Trim() != "" && date_a[2].Trim() != "")
                    {
                        string Date_YYYY = "0000" + int.Parse(date_a[0]).ToString();

                        date_a[0] = Date_YYYY.Substring(Date_YYYY.Length - 4, 4);

                        if (int.Parse(date_a[1]) < 10)
                            date_a[1] = "0" + int.Parse(date_a[1]).ToString();

                        if (int.Parse(date_a[2]) < 10)
                            date_a[2] = "0" + int.Parse(date_a[2]).ToString();

                        mtb.Text = date_a[0] + '-' + date_a[1] + '-' + date_a[2];

                        cls_Check_Input_Error c_er = new cls_Check_Input_Error();
                        if (mtb.Text.Replace("-", "").Trim() != "")
                        {
                            int Ret = 0;
                            Ret = c_er.Input_Date_Err_Check(mtb);

                            if (Ret == -1)
                            {
                                mtb.Focus(); return false;
                            }
                        }

                    }
                    else
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


        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
            }

            if (tb.Name == "txt_Search")
            {
                if (tb.Text.Trim() == "")
                    Base_Grid_Set();                      
            }

            if (tb.Name == "txtP")
            {
                if (tb.Text.Trim() == "")
                {
                    Data_Set_Form_TF = 1;
                    txtP2.Text = "";
                    Data_Set_Form_TF = 0;
                }
            }

        }


        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {

            if (tb.Name == "txtNcode")
            {
                if (tb.Text.Trim() != "" && (txtUp.Visible == false))
                {
                    Data_Set_Form_TF = 1;
                    Form_Refresh_Data_002(tb.Text);
                    Form_Refresh_Data(tb.Text);
                    Data_Set_Form_TF = 0;
                }
            }


            if (tb.Name == "txtP")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(txtP2, tb, "");
                else
                    Ncod_Text_Set_Data(txtP2, tb);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }


            if (tb.Name == "txt_Search")
            {
                if (tb.Text.Trim() != "")
                {
                    Data_Set_Form_TF = 1;
                    Base_Grid_Set(tb.Text);                    
                    Data_Set_Form_TF = 0;
                }
            }

        }



        private void Db_Grid_Popup(TextBox tb, TextBox tb1_Code, string strSql)
        {
            cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            DataGridView Popup_gr = new DataGridView();
            Popup_gr.Name = "Popup_gr";
            this.Controls.Add(Popup_gr);
            cgb_Pop.basegrid = Popup_gr;
            cgb_Pop.Base_fr = this;
            cgb_Pop.Base_tb = tb1_Code;  //앞에게 코드
            cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            cgb_Pop.Base_Location_obj = tb1_Code;
            cgb_Pop.Next_Focus_Control = txtPrice;

            if (strSql != "")
            {
                if (tb.Name == "txtP2")
                    cgb_Pop.db_grid_Popup_Base(2, "매입처_코드", "매입처", "Ncode", "Name", strSql);              
            }
            else
            {
                if (tb.Name == "txtP2")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_purchase (nolock) ";                    
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "매입처_코드", "매입처명", "Ncode", "Name", Tsql);
                }              

            }
        }



        private void Ncod_Text_Set_Data(TextBox tb, TextBox tb1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            if (tb.Name == "txtP2")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_purchase (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
            }

           
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 1)
            {
                tb.Text = ds.Tables["t_P_table"].Rows[0][1].ToString();
                tb1_Code.Text = ds.Tables["t_P_table"].Rows[0][0].ToString();
            }

            if ((ReCnt > 1) || (ReCnt == 0)) Db_Grid_Popup(tb, tb1_Code, Tsql);
        }




        void  T_R_Key_Enter_13_Name(string txt_tag, TextBox tb)
        {
            //if (txt_tag != "")
            //{
            //    int reCnt = 0;
            //    cls_Search_DB cds = new cls_Search_DB();
            //    string Search_Mbid = "";
            //    reCnt = cds.Member_Name_Search(ref Search_Mbid, txt_tag);

            //    if (reCnt == 1)
            //    {
            //        if (tb.Name == "txtMemberName")
            //            mtxtMbid.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
            //    }
            //    else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
            //    {
            //        cls_app_static_var.Search_Member_Name = txt_tag;                    
            //        Form e_f = new frmBase_Member_Search();
            //        e_f.ShowDialog();
            //        if (tb.Name == "txtMemberName")
            //        {
            //            tb.Text = cls_app_static_var.Search_Member_Name_Return;
            //            mtxtMbid.Text = cls_app_static_var.Search_Member_Number_Return;
            //        }                    
            //    }
            //    SendKeys.Send("{TAB}");
            //}

        }

      



        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


        private void Form_Clear_()
        {
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            Base_Grid_Set();
            dGridView_Base_2_Header_Reset();
            cgb_2.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


            Data_Set_Form_TF = 1;
            optUse.Checked = true;
            txtNcode.BackColor = SystemColors.Window;
            txtNcode.ReadOnly = false;
            txtNcode.BorderStyle = BorderStyle.Fixed3D;

            trv_Item_Set_Sort_Code();
                        
            mtxtRegDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                        
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, txtNcode);
            Data_Set_Form_TF = 0;

            trv_Item.Enabled = true;
            //grB_G_Tree.Enabled = true;
        }

        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

         
            if (bt.Name == "butt_Clear")
            {
                Form_Clear_();
                
            }
            

            //저장 버튼 클릭시에
            else if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Save_Base_Data(ref Save_Error_Check);

                if (Save_Error_Check > 0)
                {
                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    Base_Grid_Set();
                    if (txtKey.Text != "")
                    {
                        Data_Set_Form_TF = 1;
                        Form_Refresh_Data_002(txtKey.Text);
                        Form_Refresh_Data(txtKey.Text);
                        Data_Set_Form_TF = 0;
                    }
                    else
                    {
                        Form_Clear_();
                    }
                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<                    
                }

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            //삭제버튼 클릭시에
            else if (bt.Name == "butt_Delete")
            {
                int Del_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Delete_Base_Data(ref Del_Error_Check);

                if (Del_Error_Check > 0)
                {
                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    Base_Grid_Set();
                    dGridView_Base_2_Header_Reset();
                    cgb_2.d_Grid_view_Header_Reset();
                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    cls_form_Meth ct = new cls_form_Meth();
                    ct.from_control_clear(this, txtNcode);

                    optUse.Checked = true;
                    txtNcode.BackColor = SystemColors.Window;
                    txtNcode.ReadOnly = false;

                    trv_Item_Set_Sort_Code();

                    //grB_G_Tree.Enabled = true;
                    trv_Item.Enabled = true;

                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            
            //엑셀 전환 버튼 클릭시에
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
            Excel_Export_File_Name = this.Text; // "Goods";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }




        private Boolean Check_TextBox_Error(int i)
        {
            if (txtKey.Text.Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select"));
                dGridView_Base.Select();
                return false;
            }


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string Tsql;
            DataSet ds = new DataSet();

            Tsql = "Select ItemCode from tbl_SalesItemDetail ";
            Tsql = Tsql + " Where ItemCode ='" + txtKey.Text.Trim() + "'";
            
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_SalesItemDetail", ds) == false) return false;
            if (Temp_Connect.DataSet_ReCount != 0)//이미 매출 내역에 등록 된 상품이다. 그럼안됨.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Data")
                    + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Sell")
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtNcode.Select();
                return false;
            }


            Tsql = "Select ItemCode from tbl_StockInput ";
            Tsql = Tsql + " Where ItemCode ='" + txtKey.Text.Trim() + "'";
            
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_StockInput", ds) == false) return false;
            if (Temp_Connect.DataSet_ReCount != 0)//이미 재고 입고 내역에 등록 된 상품이다. 그럼안됨.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Data")
                    + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_InPut")
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtNcode.Select();
                return false;
            }

            Tsql = "Select ItemCode from tbl_StockOutput ";
            Tsql = Tsql + " Where ItemCode ='" + txtKey.Text.Trim() + "'";

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_StockOutput", ds) == false) return false;
            if (Temp_Connect.DataSet_ReCount != 0)//이미 재고 출고 내역에 등록 된 상품이다. 그럼안됨.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Data")
                    + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_OutPut")
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtNcode.Select();
                return false;
            }

            Tsql = "Select M_itemCode from tbl_Stock_Move_Sub ";
            Tsql = Tsql + " Where M_itemCode ='" + txtKey.Text.Trim() + "'";

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Stock_Move_Sub", ds) == false) return false;
            if (Temp_Connect.DataSet_ReCount != 0)//이미 재고 출고 내역에 등록 된 상품이다. 그럼안됨.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Useing_Data")
                    + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Move")
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtNcode.Select();
                return false;
            }
            
            

            return true;
        }


        private void Delete_Base_Data(ref int Del_Error_Check)
        {
            Del_Error_Check = 0;
            if (Check_TextBox_Error(1) == false) return;

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();                       
            Temp_Connect.Connect_DB();
            System.Data.SqlClient.SqlConnection Conn = Temp_Connect.Conn_Conn();
            System.Data.SqlClient.SqlTransaction tran = Conn.BeginTransaction();

            string Tsql;           

            try
            {                                
                Tsql = "Insert into  tbl_Goods_Change_Mod ";
                Tsql = Tsql + " Select * , 'D' ";
                Tsql = Tsql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Goods_Change ";
                Tsql = Tsql + " Where Ncode = '" + txtKey.Text.Trim() + "'";

                Temp_Connect.Insert_Data(Tsql, base_db_name, Conn, tran );

                Tsql = "Insert into  tbl_Goods_Mod ";
                Tsql = Tsql + " Select * , 'D' ";
                Tsql = Tsql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Goods ";
                Tsql = Tsql + " Where Ncode = '" + txtKey.Text.Trim() + "'";

                Temp_Connect.Insert_Data(Tsql, base_db_name, Conn, tran);


                Tsql = "Delete From tbl_Goods_Change ";
                Tsql = Tsql + " Where Ncode = '" + txtKey.Text.Trim() + "'";
                
                Temp_Connect.Delete_Data(Tsql, base_db_name, Conn, tran, this.Name.ToString(), this.Text) ;


                Tsql = "Delete From tbl_Goods ";
                Tsql = Tsql + " Where Ncode = '" + txtKey.Text.Trim() + "'";

                Temp_Connect.Delete_Data(Tsql, base_db_name, Conn, tran, this.Name.ToString(), this.Text);

                tran.Commit();                

                Del_Error_Check =1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del"));
            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Err"));
            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }            
        }






        private Boolean Check_TextBox_Error()
        {


            cls_Check_Text T_R = new cls_Check_Text();

            string me = T_R.Text_Null_Check(txtNcode); //코드
            if (me != "")
            {
                MessageBox.Show(me);        return false;
            }

            me = T_R.Text_Null_Check(txtName);    //제품명
            if (me != "")
            {
                MessageBox.Show(me);        return false;
            }


            ////if (txtRegDate.Text == "")
            ////    txtRegDate.Text = DateTime.Now.ToString("yyyyMMdd");

            me = T_R.Text_Null_Check(mtxtRegDate); //등록일자.
            if (me != "")
            {
                MessageBox.Show(me);        return false;
            }

            if (Sn_Number_(mtxtRegDate.Text, mtxtRegDate, "Date") == false)
            {
                mtxtRegDate.Focus();
                return false;
            }


            
            if ((trv_Item.Visible == true) && (txtUp.Text == ""))
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Code")
                    + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods_Sort")
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                trv_Item.Focus();                return false;
            }


            ////if (txtNcode.MaxLength != txtNcode.Text.Trim().Length)
            ////{
            ////    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Code") + "\n" +
            ////          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            ////    txtNcode.Focus();
            ////    return false;
            ////}

            if (txtPrice.Text.Trim() == "") txtPrice.Text = "0";
            if (txtPrice2.Text.Trim() == "") txtPrice2.Text = "0";
            if (txtPrice2_2.Text.Trim() == "") txtPrice2_2.Text = "0";
            if (txtVat.Text.Trim() == "") txtVat.Text = "0";
            if (txtPrice4.Text.Trim() == "") txtPrice4.Text = "0";
            if (txtPrice5.Text.Trim() == "") txtPrice5.Text = "0";
            if (txtPrice6.Text.Trim() == "") txtPrice6.Text = "0";
            if (txtPrice7.Text.Trim() == "") txtPrice7.Text = "0";
            if (txtPrice8.Text.Trim() == "") txtPrice8.Text = "0";

           

            //DateTime dateTime;
            //string input = mtxtRegDate.Text; //string.Format("{0:####-##-##}", int.Parse( txtRegDate.Text) );
            //if (DateTime.TryParse(input, out dateTime) ==false)





            ////if (txtKey.Text != "" && mtxtApplyDate.Text.Replace("_", "").Replace("-", "").Trim() == "")
            ////{
            ////    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_AppDate_Good")
            ////         + "\n" +
            ////         cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            ////    mtxtApplyDate.Focus();
            ////    return false;
            ////}


            if (mtxtApplyDate.Text.Replace("-", "").Trim() != "")
            //if (txtApplyDate.Text.Trim() != "" )
            {
                //input = mtxtApplyDate.Text; //string.Format("{0:####-##-##}", int.Parse(txtApplyDate.Text));
                //if (DateTime.TryParse(input, out dateTime) == false)
                if (Sn_Number_(mtxtApplyDate.Text, mtxtApplyDate, "Date") == false)            
                {

                    //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                    // + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Date")
                    // + "\n" +
                    // cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtApplyDate.Focus();
                    return false;
                }
            }
            
            return true;
        }


        private bool  Check_TextBox_Error(string SaveCheck_2, ref int Chang_Price_TF)
        {
            SaveCheck_2 = "";   
            Chang_Price_TF= 0 ;  //상품의 금액 관련 사항들이 변경을 했는지를 체크한다 변경하면 1
            string Tsql;

            if (txtP.Text.Trim() != "")
            {
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                Tsql = "Select Ncode, Name ";
                Tsql = Tsql + " From tbl_purchase  (nolock)  ";
                Tsql = Tsql + " Where Ncode = '" + (txtP.Text).Trim() + "'";
                Tsql = Tsql + " Order by Ncode ASC ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, "tbl_purchase", ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount == 0)//매입처 코드를 넣었는데 이게 매입처 테이블에 없네
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Purchase")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txtP.Select();
                    return false;
                }
            }


            
            if (txtKey.Text.Trim() == "")  //처음 인설트 할때는 동일한 이름과 동일한 코드로 이미 저장된 내역이 잇는지를 체크한다.
            {
                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                Tsql = "Select Ncode, Name ";
                Tsql = Tsql + " From tbl_Goods  (nolock)  ";
                Tsql = Tsql + " Where upper(Ncode) = '" + ( (txtNcode.Text).Trim()).ToUpper() + "'";
                Tsql = Tsql + " Order by Ncode ASC ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 코드가 있다 그럼.이거 저장하면 안되요
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_Code") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txtNcode.Select();
                    return false;
                }
                
                Tsql = "Select Ncode, Name ";
                Tsql = Tsql + " From tbl_Goods  (nolock)  ";
                Tsql = Tsql + " Where Name = '" + (txtName.Text).Trim() + "'";
                Tsql = Tsql + " Order by Ncode ASC ";

                ds.Clear();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 이름이 있다 그럼.이거 저장하면 안되요
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_Name") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txtName.Select();
                    return false;
                }

                //++++++++++++++++++++++++++++++++
            }
            else
            {


                //변경 저장일 경우에는 동일한 코드는 다른데 동일한 이름으로 저장된 내역이 있는지 체크한다.
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                Tsql = "Select Ncode, Name ";
                Tsql = Tsql + " From tbl_Goods  (nolock)  ";
                Tsql = Tsql + " Where upper(Ncode) <> '" + ((txtKey.Text).Trim()).ToUpper() + "'";
                Tsql = Tsql + " And  Name = '" + (txtName.Text).Trim() + "'";
                Tsql = Tsql + " Order by Ncode ASC ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 이름으로 코드가 있다 그럼.이거 저장하면 안되요
                {

                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_Name") + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtName.Select();
                    return false;
                }

                Tsql = "Select Ncode , Name  " ;
                Tsql = Tsql + " , Isnull (( Select Top 1 price1 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode Order by ApplyDate DESC   ),price) Last_price ";
                Tsql = Tsql + " , Isnull (( Select Top 1 price2 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode Order by ApplyDate DESC   ),price2) Last_price2 ";
                Tsql = Tsql + " , Isnull (( Select Top 1 price3 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode Order by ApplyDate DESC   ),price4) Last_price3 ";                                
                Tsql = Tsql + " , Isnull (( Select Top 1 price4 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode Order by ApplyDate DESC   ),price4) Last_price4 ";
                Tsql = Tsql + " , Isnull (( Select Top 1 price5 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode Order by ApplyDate DESC   ),price5) Last_price5 ";
                Tsql = Tsql + " , Isnull (( Select Top 1 price6 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode Order by ApplyDate DESC   ),price6) Last_price6 ";
                Tsql = Tsql + " , Isnull (( Select Top 1 price7 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode Order by ApplyDate DESC   ),price7) Last_price7 ";
                Tsql = Tsql + " , Isnull (( Select Top 1 price8 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode Order by ApplyDate DESC   ),price8) Last_price8 ";
                Tsql = Tsql + " , Isnull (( Select Top 1 Except_Sell_VAT_Price From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode Order by ApplyDate DESC   ),Except_Sell_VAT_Price) Last_Except_Sell_VAT_Price ";
                Tsql = Tsql + " , Isnull (( Select Top 1 Sell_VAT_Price From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode Order by ApplyDate DESC   ),Sell_VAT_Price) Last_Sell_VAT_Price ";
                Tsql = Tsql + " From tbl_Goods  (nolock)  ";
                Tsql = Tsql + " Where upper(Ncode) = '" + ((txtKey.Text).Trim()).ToUpper() + "'";
                Tsql = Tsql + " Order by Ncode ASC ";

                ds.Clear();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                if (Temp_Connect.DataSet_ReCount != 0)//동일한 이름이 있다 그럼.이거 저장하면 안되요
                {
                    if (double.Parse(ds.Tables[base_db_name].Rows[0]["Last_price"].ToString()) != double.Parse(txtPrice.Text.Replace(",", "")))
                        Chang_Price_TF = 1;
                    if (double.Parse(ds.Tables[base_db_name].Rows[0]["Last_price2"].ToString()) != double.Parse(txtPrice2.Text.Replace(",", "")))
                        Chang_Price_TF = 1;
                    if (double.Parse(ds.Tables[base_db_name].Rows[0]["Last_price4"].ToString()) != double.Parse(txtPrice4.Text.Replace(",", "")))
                        Chang_Price_TF = 1;

                    if (double.Parse(ds.Tables[base_db_name].Rows[0]["Last_price5"].ToString()) != double.Parse(txtPrice5.Text.Replace(",", "")))
                        Chang_Price_TF = 1;

                    if (double.Parse(ds.Tables[base_db_name].Rows[0]["Last_price6"].ToString()) != double.Parse(txtPrice6.Text.Replace(",", "")))
                        Chang_Price_TF = 1;

                    if (double.Parse(ds.Tables[base_db_name].Rows[0]["Last_price7"].ToString()) != double.Parse(txtPrice7.Text.Replace(",", "")))
                        Chang_Price_TF = 1;

                    if (double.Parse(ds.Tables[base_db_name].Rows[0]["Last_price8"].ToString()) != double.Parse(txtPrice8.Text.Replace(",", "")))
                        Chang_Price_TF = 1;

                    if (double.Parse(ds.Tables[base_db_name].Rows[0]["Last_Except_Sell_VAT_Price"].ToString()) != double.Parse(txtPrice2_2.Text.Replace(",", "")))
                        Chang_Price_TF = 1;
                }

                //상품의 금액적인 내역이 변경이 일어낫다. 그럼 변경 적용일을 입력하게 한다.
                if ((Chang_Price_TF == 1) && (mtxtApplyDate.Text.Replace("_", "").Replace("-", "").Trim() == ""))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_ChangDate")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtApplyDate.Focus();    return false;
                }
                
            }

            

            return true;
        }





        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
            int Chang_Price_TF = 0;
            Data_Set_Form_TF = 1;
            if (Check_TextBox_Error() == false)
            {
                Data_Set_Form_TF = 0;
                return;
            }
                        
            if (Check_TextBox_Error("Save_Err_Check_2", ref Chang_Price_TF) == false)
            {
                Data_Set_Form_TF = 0;
                return;
            }
            Data_Set_Form_TF = 0;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
           
            int Sell_VAT_TF = 0 ;   int GoodUse = 0 ;

            if (optVat1.Checked  == true)
                Sell_VAT_TF = 0;
            else
                Sell_VAT_TF = 1;
            

            if (optUse.Checked == true)
                GoodUse = 0;
            else if (optNot.Checked == true )
                GoodUse = 1 ;
            
    
           
            string Tsql;
            if (txtKey.Text.Trim() == "")
            {
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

                Tsql = "insert into tbl_Goods ( ";
                Tsql = Tsql + " ncode, name, inspection,";
                Tsql = Tsql + " price, price2, price3, ";
                Tsql = Tsql + " price4, price5,price6, ";
                Tsql = Tsql + " price7, price8,";
                Tsql = Tsql + " Sell_VAT_Price,Except_Sell_VAT_Price,Sell_VAT_TF, ";
                Tsql = Tsql + " Item_RegTime,Up_itemCode, ";
                Tsql = Tsql + " GoodUse , P_Code , T_ETC , ";                
                Tsql = Tsql + " recordid, recordtime";
                Tsql = Tsql + " ,name_E ";
                Tsql = Tsql + " ) values(";
                               
                Tsql = Tsql + " '" + txtNcode.Text.Trim() + "'";

                Tsql = Tsql + ",'" + txtName.Text.Trim() + "'";
                Tsql = Tsql + ",'" + txtInspection.Text.Trim() + "'";

                Tsql = Tsql + "," + double.Parse(txtPrice.Text.Replace(",", ""));
                Tsql = Tsql + "," + double.Parse(txtPrice2.Text.Replace(",", ""));
                Tsql = Tsql + ",0 ";
                Tsql = Tsql + "," + double.Parse(txtPrice4.Text.Replace(",", ""));
                Tsql = Tsql + "," + double.Parse(txtPrice5.Text.Replace(",", ""));
                Tsql = Tsql + "," + double.Parse(txtPrice6.Text.Replace(",", ""));

                Tsql = Tsql + "," + double.Parse(txtPrice7.Text.Replace(",", ""));
                Tsql = Tsql + "," + double.Parse(txtPrice8.Text.Replace(",", ""));

                Tsql = Tsql + "," + double.Parse(txtVat.Text.Replace(",", ""));
                Tsql = Tsql + "," + double.Parse(txtPrice2_2.Text.Replace(",", ""));
                Tsql = Tsql + "," + Sell_VAT_TF  ;

                Tsql = Tsql + ",'" + mtxtRegDate.Text.Replace("_", "").Replace("-", "").Trim() + "'";
                Tsql = Tsql + ",'" + txtUp.Text.Trim() + "'";

                Tsql = Tsql + "," + GoodUse;
                Tsql = Tsql + ",'" + txtP.Text.Trim() + "'";
                Tsql = Tsql + ",'" + texETC.Text.Trim() + "'";

                Tsql = Tsql + ",'" + cls_User.gid + "'";
                Tsql = Tsql + " , Convert(Varchar(25),GetDate(),21) ";

                Tsql = Tsql + ",'" + txtName_E.Text.Trim() + "'";

                Tsql = Tsql + ")";
                
                if (Temp_Connect.Insert_Data(Tsql, base_db_name, this.Name.ToString(), this.Text) == false) return;

                Save_Error_Check =1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));


            }
            else //동일한 코드가 있구나 그럼 업데이트
            {
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

                if (Save_Base_Data_Up(GoodUse, Sell_VAT_TF, Chang_Price_TF) == false) return;

                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
            }

        }

        private Boolean Save_Base_Data_Up(int GoodUse, int Sell_VAT_TF, int Chang_Price_TF)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();
            
            string Tsql;

            try
            {
                Tsql = "Insert into tbl_Goods_Mod ";
                Tsql = Tsql + " Select * , 'U' ";
                Tsql = Tsql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Goods ";
                Tsql = Tsql + " Where Ncode = '" + txtKey.Text.Trim() + "'";

                Temp_Connect.Insert_Data(Tsql, base_db_name, Conn, tran);


                Tsql = "Update tbl_Goods Set ";
                Tsql = Tsql + " name = '" + txtName.Text.Trim() + "'";
                Tsql = Tsql + " ,inspection = '" + txtInspection.Text.Trim() + "'";
                Tsql = Tsql + " ,GoodUse = " + GoodUse;
                Tsql = Tsql + " ,Sell_VAT_TF = " + Sell_VAT_TF;
                Tsql = Tsql + " ,P_Code = '" + txtP.Text.Trim() + "'";
                Tsql = Tsql + " ,Up_itemCode = '" + txtUp.Text.Trim() + "'";
                Tsql = Tsql + " ,T_ETC='" + texETC.Text.Trim() + "'";
                Tsql = Tsql + " ,name_E='" + txtName_E.Text.Trim() + "'";
                Tsql = Tsql + " WHERE Ncode = '" + txtKey.Text.Trim() + "'";

                Temp_Connect.Update_Data(Tsql, Conn, tran, this.Name.ToString(), this.Text);

                //금액변동일 일어나고 날짜가 있다. 변경 할려고 하는
                if ( (mtxtApplyDate.Text.Replace("_", "").Replace("-", "").Trim() != ""))
                {
                    Tsql = "Select Ncode, ApplyDate ";
                    Tsql = Tsql + " From tbl_Goods_Change  (nolock)  ";
                    Tsql = Tsql + " Where upper(Ncode) = '" + ((txtKey.Text).Trim()).ToUpper() + "'";
                    Tsql = Tsql + " And  ApplyDate = '" + (mtxtApplyDate.Text).Trim().Replace("_", "").Replace("-", "") + "'";
                    Tsql = Tsql + " And  (Na_code = 'KR' OR Na_code = '' ) ";
                    Tsql = Tsql + " Order by ApplyDate DESC ";

                    DataSet ds = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Goods_Change", ds) == true)
                    {
                        if (Temp_Connect.DataSet_ReCount != 0)//동일한 이름으로 코드가 있다 그럼.이거 저장하면 안되요
                        {
                            Tsql = "Insert into tbl_Goods_Change_Mod ";
                            Tsql = Tsql + " Select * , 'U' ";
                            Tsql = Tsql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Goods_Change ";
                            Tsql = Tsql + " Where Ncode = '" + txtKey.Text.Trim() + "'";
                            Tsql = Tsql + " And  (Na_code = 'KR' OR Na_code = '' ) ";

                            Temp_Connect.Insert_Data(Tsql, "tbl_Goods_Change", Conn, tran);

                            Tsql = "Update tbl_Goods_Change Set ";
                            Tsql = Tsql + " price1 = " + double.Parse(txtPrice.Text.Replace(",", ""));
                            Tsql = Tsql + " ,price2 = " + double.Parse(txtPrice2.Text.Replace(",", ""));
                            Tsql = Tsql + " ,price3 = 0 ";
                            Tsql = Tsql + " ,price4 = " + double.Parse(txtPrice4.Text.Replace(",", ""));
                            Tsql = Tsql + " ,price5 =  " + double.Parse(txtPrice5.Text.Replace(",", ""));
                            Tsql = Tsql + " ,price6 = " + double.Parse(txtPrice6.Text.Replace(",", ""));
                            Tsql = Tsql + " ,price7 = " + double.Parse(txtPrice7.Text.Replace(",", ""));
                            Tsql = Tsql + " ,price8 = " + double.Parse(txtPrice8.Text.Replace(",", ""));

                            Tsql = Tsql + " ,Sell_VAT_Price = " + double.Parse(txtVat.Text.Replace(",", ""));
                            Tsql = Tsql + " ,Except_Sell_VAT_Price = " + double.Parse(txtPrice2_2.Text.Replace(",", ""));

                            Tsql = Tsql + " WHERE Ncode = '" + txtKey.Text.Trim() + "'";
                            Tsql = Tsql + " And  ApplyDate = '" + (mtxtApplyDate.Text).Trim().Replace("_", "").Replace("-", "") + "'";
                            Tsql = Tsql + " And  ( Na_code = 'KR' OR Na_code = '' ) ";

                            Temp_Connect.Update_Data(Tsql, Conn, tran, this.Name.ToString(), this.Text);
                        }
                        else
                        {
                            Tsql = "insert into tbl_Goods_Change ( ";
                            Tsql = Tsql + " ncode, name , ApplyDate , ";
                            Tsql = Tsql + " price1 , price2 , price3 , ";
                            Tsql = Tsql + " price4 , price5 , price6 , ";
                            Tsql = Tsql + " price7 , price8 , price9 , ";
                            Tsql = Tsql + " Sell_VAT_Price , Except_Sell_VAT_Price , ";
                            Tsql = Tsql + " recordid , recordtime ";
                            Tsql = Tsql + " ) values ( ";
                            Tsql = Tsql + " '" + txtKey.Text.Trim() + "'";
                            Tsql = Tsql + ",'" + txtName.Text.Trim() + "'";
                            Tsql = Tsql + ",'" + mtxtApplyDate.Text.Replace("_", "").Replace("-", "").Trim() + "'";

                            Tsql = Tsql + "," + double.Parse(txtPrice.Text.Replace(",", ""));
                            Tsql = Tsql + "," + double.Parse(txtPrice2.Text.Replace(",", ""));
                            Tsql = Tsql + ",0 ";
                            Tsql = Tsql + "," + double.Parse(txtPrice4.Text.Replace(",", ""));
                            Tsql = Tsql + "," + double.Parse(txtPrice5.Text.Replace(",", ""));
                            Tsql = Tsql + "," + double.Parse(txtPrice6.Text.Replace(",", ""));

                            Tsql = Tsql + "," + double.Parse(txtPrice7.Text.Replace(",", ""));
                            Tsql = Tsql + "," + double.Parse(txtPrice8.Text.Replace(",", ""));
                            Tsql = Tsql + ",0 ";

                            Tsql = Tsql + "," + double.Parse(txtVat.Text.Replace(",", ""));
                            Tsql = Tsql + "," + double.Parse(txtPrice2_2.Text.Replace(",", ""));

                            Tsql = Tsql + ",'" + cls_User.gid + "'";
                            Tsql = Tsql + " , Convert(Varchar(25),GetDate(),21) ";
                            Tsql = Tsql + ")";

                            Temp_Connect.Insert_Data(Tsql, base_db_name, Conn, tran, this.Name.ToString(), this.Text);
                        }
                    }
                }
                
                tran.Commit();
                return true;

            }
            catch (Exception)
            {                
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Err"));
                return false;
            }

            finally
            {                
                tran.Dispose();
                Temp_Connect.Close_DB();

            }
        }


        private void Form_Refresh_Data(string ncode )
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;

            Tsql = "Select tbl_Goods.Ncode  ";
            Tsql = Tsql + " , tbl_Goods.name  ";
            Tsql = Tsql + " , tbl_Goods.name_E  ";
            Tsql = Tsql + " , inspection  ";
            Tsql = Tsql + " , price ";
            Tsql = Tsql + " , price2 ";  //5
            Tsql = Tsql + " , Except_Sell_VAT_Price ";
            Tsql = Tsql + " , Sell_VAT_Price ";
            Tsql = Tsql + " , price4 ";
            Tsql = Tsql + " , price5 ";
            Tsql = Tsql + " , price6  "; //10

            Tsql = Tsql + " , GoodUse ";
            Tsql = Tsql + " , Item_RegTime ";
            Tsql = Tsql + " , P_Code ";
            Tsql = Tsql + " , Isnull(tbl_purchase.name, '') AS PName ";
            Tsql = Tsql + " , Sell_VAT_TF   "; //15
            Tsql = Tsql + " , T_ETC ";
            Tsql = Tsql + " , Up_itemCode ";

            Tsql = Tsql + " , Isnull (( Select Top 1 price1 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_code = '' Order by ApplyDate DESC   ),price) Last_price ";
            Tsql = Tsql + " , Isnull (( Select Top 1 price2 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_code = ''Order by ApplyDate DESC   ),price2) Last_price2 ";
            Tsql = Tsql + " , Isnull (( Select Top 1 Except_Sell_VAT_Price From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_code = '' Order by ApplyDate DESC   ),Except_Sell_VAT_Price) Last_Except_Sell_VAT_Price ";
            Tsql = Tsql + " , Isnull (( Select Top 1 Sell_VAT_Price From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_code = '' Order by ApplyDate DESC   ),Sell_VAT_Price) Last_Sell_VAT_Price ";
            Tsql = Tsql + " , Isnull (( Select Top 1 price4 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_code = '' Order by ApplyDate DESC   ),price4) Last_price4 ";
            Tsql = Tsql + " , Isnull (( Select Top 1 price5 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_code = '' Order by ApplyDate DESC   ),price5) Last_price5 ";
            Tsql = Tsql + " , Isnull (( Select Top 1 price6 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_code = '' Order by ApplyDate DESC   ),price6) Last_price6 ";
            Tsql = Tsql + " , Isnull (( Select Top 1 price7 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_code = '' Order by ApplyDate DESC   ),price7) Last_price7 ";
            Tsql = Tsql + " , Isnull (( Select Top 1 price8 From tbl_Goods_Change Where tbl_Goods_Change.Ncode = tbl_Goods.Ncode And Na_code = '' Order by ApplyDate DESC   ),price8) Last_price8 ";
                      
            Tsql = Tsql + " From tbl_Goods (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_purchase ON tbl_purchase.Ncode = tbl_Goods.P_Code ";                       
            Tsql = Tsql + " Where tbl_Goods.ncode = '" + ncode + "'";           
            Tsql = Tsql + " Order by tbl_Goods.Ncode ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            txtKey.Text = ds.Tables[base_db_name].Rows[0]["Ncode"].ToString();

            txtUp.Text = ds.Tables[base_db_name].Rows[0]["Up_itemCode"].ToString();
            txtNcode.Text = ds.Tables[base_db_name].Rows[0]["Ncode"].ToString();  //대분류랑 관련없이 코드를 만들수 있도록 하는게 더 낳은듯 해서 변경을함.


            //////대분류 중분류 선택일 경우에는 상품 코드는 대분류+중분류 + 입력 상품 코드로 해서. 저장된다.
            ////if (txtUp.Visible == true)
            ////{
            ////    //txtUp.MaxLength
            ////    string T_Code = ds.Tables[base_db_name].Rows[0]["Ncode"].ToString();
            ////    txtNcode.Text = T_Code.Substring(txtUp.MaxLength, txtNcode.MaxLength )  ;
            ////}
            ////else //대분류 중분류 선택이 아닌 경우에는 상품 코드를 다 보여준다
            ////    txtNcode.Text = ds.Tables[base_db_name].Rows[0]["Ncode"].ToString();

            txtName.Text = ds.Tables[base_db_name].Rows[0]["name"].ToString();
            txtName_E.Text = ds.Tables[base_db_name].Rows[0]["name_E"].ToString();
            txtInspection.Text = ds.Tables[base_db_name].Rows[0]["inspection"].ToString();

            txtP.Text = ds.Tables[base_db_name].Rows[0]["P_Code"].ToString();
            txtP2.Text = ds.Tables[base_db_name].Rows[0]["PName"].ToString();

            txtPrice.Text = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["Last_price"]);
            txtPrice2.Text = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["Last_price2"]);
            txtPrice2_2.Text = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["Last_Except_Sell_VAT_Price"]);
            txtVat.Text = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["Last_Sell_VAT_Price"]);
            txtPrice4.Text = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["Last_price4"]);
            txtPrice5.Text = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["Last_price5"]);
            txtPrice6.Text = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["Last_price6"]);

            txtPrice7.Text = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["Last_price7"]);
            txtPrice8.Text = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["Last_price8"]);


            texETC.Text = ds.Tables[base_db_name].Rows[0]["T_ETC"].ToString();

            //mtxtRegDate.Text = ds.Tables[base_db_name].Rows[0]["Item_RegTime"].ToString();
            if (ds.Tables[base_db_name].Rows[0]["Item_RegTime"].ToString() != "" )
                mtxtRegDate.Text = string.Format("{0:####-##-##}", int.Parse(ds.Tables[base_db_name].Rows[0]["Item_RegTime"].ToString()));

            if (ds.Tables[base_db_name].Rows[0]["Sell_VAT_TF"].ToString() != "1")
            {
                optVat1.Checked = true; optVat2.Checked = false;
            }
            else
            {
                optVat1.Checked = false; optVat2.Checked = true;
            }


            if (ds.Tables[base_db_name].Rows[0]["GoodUse"].ToString() == "0")
            {
                optUse.Checked = true; optNot.Checked = false;
            }
            else
            {
                optUse.Checked = false; optNot.Checked = true;
            }

            //상품 최도 등록된 가격을 보여준다.
            txtPrice_1.Text = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["price"]);
            txtPrice2_1.Text = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["price2"]);           
            txtPrice4_1.Text = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["price4"]);


            //상품 중분류 대분류 체크 트리상에서 모든 체크내역을 다 푼다.
            foreach (string t_for_key in dic_Tree_Sort_1.Keys)
            {
                TreeNode tn2 = dic_Tree_Sort_1[t_for_key];
                tn2.Checked = false;
            }

            foreach (string t_for_key in dic_Tree_Sort_2.Keys)
            {
                TreeNode tn2 = dic_Tree_Sort_2[t_for_key];
                tn2.Checked = false;
            }


            //상품 중분류와 대분류 코드에서 선택된 내역을 트리에서 찾아서 체크한다.
            if (cls_app_static_var.Item_Sort_3_Code_Length > 0)
            {
                TreeNode tn2 = dic_Tree_Sort_2[txtUp.Text];
                tn2.Checked = true;   
            }
            else if (cls_app_static_var.Item_Sort_2_Code_Length > 0)
            {
                TreeNode tn2 = dic_Tree_Sort_1[txtUp.Text];
                tn2.Checked = true;
            }


            //더블클릭이나 상품 정보를 불러온 상태에선느 상품 코드의 변경이 안일어 나게 하기 위해서 상품 코드 텍스트를 락시킨다
            //추후 위의 새로 입력 버튼으로 풀수 있음.
            txtNcode.BackColor = cls_app_static_var.txt_Enable_Color;
            txtNcode.ReadOnly = true;
            txtNcode.BorderStyle = BorderStyle.FixedSingle;

            trv_Item.Enabled = false;
            //grB_G_Tree.Enabled = false;  ;

            txtName.Focus();

        }


        private void Form_Refresh_Data_002(string ncode )
        {
            dGridView_Base_2_Header_Reset();
            cgb_2.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

              
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;

            Tsql = "Select  ";
            Tsql = Tsql + "  LEFT(ApplyDate,4) +'-' + LEFT(RIGHT(ApplyDate,4),2) + '-' + RIGHT(ApplyDate,2)   ";          
            Tsql = Tsql + " , price1 ";
            Tsql = Tsql + " , price2 ";  
            Tsql = Tsql + " , Except_Sell_VAT_Price ";
            Tsql = Tsql + " , Sell_VAT_Price "; //5
            Tsql = Tsql + " , price4 ";
            Tsql = Tsql + " , price5 ";
            Tsql = Tsql + " , price6  ";
            Tsql = Tsql + " , price7 ";
            Tsql = Tsql + " , price8 "; //10         

            Tsql = Tsql + " From tbl_Goods_Change (nolock) ";            
            Tsql = Tsql + " Where ncode = '" + ncode + "'";           
            Tsql = Tsql + " Order by ApplyDate DESC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_2_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb_2.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_2.db_grid_Obj_Data_Put();

        }




        private void dGridView_Base_2_Header_Reset()
        {
            cgb_2.grid_col_Count = 10;
            cgb_2.basegrid = dGridView_Base_2;
            cgb_2.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_2.grid_Frozen_End_Count = 2;
            cgb_2.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
              

            string[] g_HeaderText = { "변경_적용일" , "소비자가" ,"회원가" , "공급가"   , "VAT"  
                                     , "PV"   , ""   ,"_직원가" , "총판가" , "총판PV"                                    
                                    };
            cgb_2.grid_col_header_text = g_HeaderText;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();

            gr_dic_cell_format[2 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb_2.grid_cell_format = gr_dic_cell_format;

            int[] g_Width = { 110, 80, 80, 80, 80  
                             ,80, 0, 0, 80, 80                             
                            };
            cgb_2.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true   
                                   };
            cgb_2.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  
                      
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  //5
                               
                               ,DataGridViewContentAlignment.MiddleRight                                
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight //10

                              };
            cgb_2.grid_col_alignment = g_Alignment;
        }


        private void Set_gr_2_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]                                
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][7]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9]                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;

        }




        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
           // SendKeys.Send("{TAB}");
        }

        private void trv_Item_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                //중분류가 존재하는데 대분류를 선택 한 경우에는 중분류를 선택 하도록 체크를 지워 버린다.
                if (cls_app_static_var.Item_Sort_3_Code_Length > 0)
                {
                    if (e.Node.Parent == null)
                    {
                        e.Node.Checked = false;
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Goods_Sort_2") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    }

                    int Check_Cnt = 0;
                    //중분류 선택시 중대분류 다른 곳에 체크를 했는지를 알기 위해서 했으면 지금 체크를 지우운다.
                    foreach (string t_for_key in dic_Tree_Sort_2.Keys)
                    {
                        TreeNode tn2 = dic_Tree_Sort_2[t_for_key];
                        if (tn2.Checked == true) Check_Cnt++;
                    }

                    if (Check_Cnt >= 2)
                    {
                        e.Node.Checked = false;
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Check") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        return;
                    }

                    Check_Cnt = 0;
                    foreach (string t_for_key in dic_Tree_Sort_2.Keys)
                    {
                        TreeNode tn2 = dic_Tree_Sort_2[t_for_key];
                        if (tn2.Checked == true)
                        {
                            txtUp.Text = t_for_key;
                            Check_Cnt++;
                        }
                    }

                    if (Check_Cnt == 0) txtUp.Text = "";
                    
                  
                }
                else
                {
                    //대분류 선택시 대분류 다른 곳에 체크를 했는질ㄹ 알기 위해서 했으면 지금 체크를 지우운다.
                    int Check_Cnt = 0;
                   
                    foreach (string t_for_key in dic_Tree_Sort_1.Keys)
                    {
                        TreeNode tn2 = dic_Tree_Sort_1[t_for_key];
                        if (tn2.Checked == true) Check_Cnt++;
                    }

                    if (Check_Cnt >= 2)
                    {
                        e.Node.Checked = false;
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Check") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        return;
                    }

                    Check_Cnt = 0;
                    foreach (string t_for_key in dic_Tree_Sort_1.Keys)
                    {
                        TreeNode tn2 = dic_Tree_Sort_1[t_for_key];
                        if (tn2.Checked == true)
                        {
                            txtUp.Text = t_for_key;
                            Check_Cnt++;
                        }
                    }

                    if (Check_Cnt == 0) txtUp.Text = "";
                    
                }
                //if (e.Node.Nodes.Count > 0)                
            }

            if (txtNcode.ReadOnly == false)
                txtNcode.Focus();

        }




        private void optVat1_Click(object sender, EventArgs e)
        {

            
            //double T_p = double.Parse(tb.Text.Replace(",", "").ToString());
            //tb.Text = string.Format(cls_app_static_var.str_Currency_Type, T_p);
            

            double VAT = 0, SW = 0;

            if (optVat1.Checked == true)
            {
                if (txtPrice2.Text != "")
                {
                    if (double.Parse(txtPrice2.Text.Trim()) > 0)
                    {
                        VAT = (double.Parse(txtPrice2.Text.Trim()) / 1.1) * 10;

                        if (VAT % 10 > 0)
                            SW = 1;

                        //VAT = Fix(VAT / 10) ;
                        VAT = Math.Truncate((VAT / 10));

                        if (SW == 1)
                            VAT = VAT + 1;

                        txtPrice2_2.Text = string.Format(cls_app_static_var.str_Currency_Type, VAT); //.ToString();
                        VAT = double.Parse(txtPrice2.Text.Replace(",", "").Trim()) - VAT;
                        txtVat.Text =  string.Format(cls_app_static_var.str_Currency_Type, VAT) ; //VAT.ToString();

                    }
                }

                txtPrice4.Focus();
                
            }
        }

        private void optVat2_Click(object sender, EventArgs e)
        {
            txtPrice2_2.Text = "0";
            txtVat.Text = "0";
            txtPrice4.Focus();
        }




    }
}
