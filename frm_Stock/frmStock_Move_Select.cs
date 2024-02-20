﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MLM_Program
{
    public partial class frmStock_Move_Select : Form
    {
        

         cls_Grid_Base cgb = new cls_Grid_Base();

         private const string base_db_name = "tbl_Stock_Move_Sub";
        private int Data_Set_Form_TF;


        Series series_Item = new Series();
        Series series_Item_D = new Series();


        public frmStock_Move_Select()
        {
            InitializeComponent();
        }


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            mtxtMDate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMakDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMakDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            //grB_Search.Height = mtxtMbid.Top + mtxtMbid.Height + 3;
        }

        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();
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
                    Base_Button_Click(T_bt, ee1);
            }
        }


        private Boolean Check_TextBox_Error()
        {

            cls_Check_Input_Error c_er = new cls_Check_Input_Error();

            if (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                int Ret = 0;
                Ret = c_er._Member_Nmumber_Split(mtxtMbid);

                if (Ret == -1)
                {
                    mtxtMbid.Focus(); return false;
                }
            }


            if (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                int Ret = 0;
                Ret = c_er._Member_Nmumber_Split(mtxtMbid2);

                if (Ret == -1)
                {
                    mtxtMbid2.Focus(); return false;
                }
            }


            if (mtxtMDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtMDate.Text, mtxtMDate, "Date") == false)
                {
                    mtxtMDate.Focus();
                    return false;
                }
            }

            if (mtxtMDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtMDate2.Text, mtxtMDate2, "Date") == false)
                {
                    mtxtMDate2.Focus();
                    return false;
                }
            }




            if (mtxtMakDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtMakDate1.Text, mtxtMakDate1, "Date") == false)
                {
                    mtxtMakDate1.Focus();
                    return false;
                }
            }

            if (mtxtMakDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtMakDate2.Text, mtxtMakDate2, "Date") == false)
                {
                    mtxtMakDate2.Focus();
                    return false;
                }
            }



            



            return true;
        }

        private void Make_Base_Query(ref string Tsql)
        {
            //Tsql = "Select Move_From_Dep_Cd ";
            //Tsql = Tsql + " ,Isnull(F_B.Name,'') ";
            //Tsql = Tsql + " ,LEFT(Move_Date,4) +'-' + LEFT(RIGHT(Move_Date,4),2) + '-' + RIGHT(Move_Date,2) ";
            //Tsql = Tsql + " ,Move_To_Dep_Cd ";
            //Tsql = Tsql + " ,Isnull(T_B.Name,'') ";
            //Tsql = Tsql + " ,M_itemCode ";
            //Tsql = Tsql + " ,Isnull(tbl_Goods.name,'') ";
            //Tsql = Tsql + " ,M_Cnt ";
            //Tsql = Tsql + " ,M_ID ";
            //Tsql = Tsql + " ,Remarks1 ";

            //Tsql = Tsql + " ,M_index  ";
            //Tsql = Tsql + " ,Isnull(tbl_User.U_Name ,'' )  ";
            //Tsql = Tsql + " ,tbl_Stock_Move_Sub.Recordid  ";
            //Tsql = Tsql + " ,tbl_Stock_Move_Sub.RecordTime,'' ";

            //Tsql = Tsql + " From tbl_Stock_Move_Sub (nolock) ";
            //Tsql = Tsql + " LEFT Join tbl_Goods  (nolock)  ON tbl_Stock_Move_Sub.M_itemCode = tbl_Goods.Ncode ";
            //Tsql = Tsql + " LEFT Join tbl_Business  F_B  (nolock) ON F_B.Ncode = tbl_Stock_Move_Sub.Move_From_Dep_Cd  ";
            //Tsql = Tsql + " LEFT Join tbl_Business  T_B  (nolock) ON T_B.Ncode = tbl_Stock_Move_Sub.Move_To_Dep_Cd  ";
            //Tsql = Tsql + " LEFT Join tbl_User  (nolock)  ON tbl_User.User_id = tbl_Stock_Move_Sub.M_ID  ";

            Tsql = "Select Move_From_Dep_Cd ";
            Tsql = Tsql + " ,Isnull(F_B.Name,'') ";
            Tsql = Tsql + " ,LEFT(Move_Date,4) +'-' + LEFT(RIGHT(Move_Date,4),2) + '-' + RIGHT(Move_Date,2) Move_Date ";
            Tsql = Tsql + " ,Move_To_Dep_Cd ";
            Tsql = Tsql + " ,Isnull(T_B.Name,'') ";
            Tsql = Tsql + " ,M_itemCode ";
            Tsql = Tsql + " ,Isnull(tbl_Goods.name,'') ";
            Tsql = Tsql + " ,M_Cnt ";
            Tsql = Tsql + " ,M_ID ";
            Tsql = Tsql + " ,Remarks1 ";

            Tsql = Tsql + " ,M_index  ";
            Tsql = Tsql + " ,Isnull(tbl_User.U_Name ,'' )  ";

            Tsql = Tsql + " ,LEFT(D_Date,4) +'-' + LEFT(RIGHT(D_Date,4),2) + '-' + RIGHT(D_Date,2) D_Date   ";
            Tsql = Tsql + " ,D_ID  ";
            Tsql = Tsql + " ,D_Cnt  ";

            Tsql = Tsql + " ,tbl_Stock_Move_Sub.Recordid  ";
            Tsql = Tsql + " ,tbl_Stock_Move_Sub.RecordTime,'','','' ";

            Tsql = Tsql + " From tbl_Stock_Move_Sub (nolock) ";
            Tsql = Tsql + " LEFT Join tbl_Goods  (nolock)  ON tbl_Stock_Move_Sub.M_itemCode = tbl_Goods.Ncode ";
            Tsql = Tsql + " LEFT Join tbl_Business  F_B  (nolock) ON F_B.Ncode = tbl_Stock_Move_Sub.Move_From_Dep_Cd  ";
            Tsql = Tsql + " LEFT Join tbl_Business  T_B  (nolock) ON T_B.Ncode = tbl_Stock_Move_Sub.Move_To_Dep_Cd  ";
            Tsql = Tsql + " LEFT Join tbl_User  (nolock)  ON tbl_User.User_id = tbl_Stock_Move_Sub.M_ID  ";
        }



        private void Make_Base_Query_(ref string Tsql)
        {

            string strSql = " Where tbl_Stock_Move_Sub.M_itemCode <> ''   ";


            ////string Mbid = ""; int Mbid2 = 0;
            //////회원번호1로 검색
            ////if (
            ////    (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            ////    &&
            ////    (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() == "")
            ////    )
            ////{
            ////    cls_Search_DB csb = new cls_Search_DB();
            ////    if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
            ////    {
            ////        if (Mbid != "")
            ////            strSql = strSql + " And tbl_SalesDetail.Mbid =''" + Mbid + "''";

            ////        if (Mbid2 >= 0)
            ////            strSql = strSql + " And tbl_SalesDetail.Mbid2 = " + Mbid2;
            ////    }


            ////}

            //////회원번호2로 검색
            ////if (
            ////    (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            ////    &&
            ////    (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
            ////    )
            ////{
            ////    cls_Search_DB csb = new cls_Search_DB();
            ////    if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
            ////    {
            ////        if (Mbid != "")
            ////            strSql = strSql + " And tbl_SalesDetail.Mbid >=''" + Mbid + "''";

            ////        if (Mbid2 >= 0)
            ////            strSql = strSql + " And tbl_SalesDetail.Mbid2 >= " + Mbid2;
            ////    }

            ////    if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
            ////    {
            ////        if (Mbid != "")
            ////            strSql = strSql + " And tbl_SalesDetail.Mbid <=''" + Mbid + "''";

            ////        if (Mbid2 >= 0)
            ////            strSql = strSql + " And tbl_SalesDetail.Mbid2 <= " + Mbid2;
            ////    }
            ////}


         
            //가입일자로 검색 -1
            if ((mtxtMDate.Text.Replace("-", "").Trim() != "") && (mtxtMDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_Stock_Move_Sub.Move_Date = '" + mtxtMDate.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtMDate.Text.Replace("-", "").Trim() != "") && (mtxtMDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_Stock_Move_Sub.Move_Date >= '" + mtxtMDate.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_Stock_Move_Sub.Move_Date <= '" + mtxtMDate2.Text.Replace("-", "").Trim() + "'";
            }


            ////가입일자로 검색 -1
            //if ((txtOUTDate.Text.Trim() != "") && (txtOUTDate2.Text.Trim() == ""))
            //    strSql = strSql + " And tbl_Stock_Move_Sub.D_Date = ''" + txtOUTDate.Text.Trim() + "''";

            ////가입일자로 검색 -2
            //if ((txtOUTDate.Text.Trim() != "") && (txtOUTDate2.Text.Trim() != ""))
            //{
            //    strSql = strSql + " And tbl_Stock_Move_Sub.D_Date >= ''" + txtOUTDate.Text.Trim() + "''";
            //    strSql = strSql + " And tbl_Stock_Move_Sub.D_Date <= ''" + txtOUTDate2.Text.Trim() + "''";
            //}


            //기록일자로 검색 -1
            if ((mtxtMakDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_Stock_Move_Sub.D_Date = '" + mtxtMakDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtMakDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_Stock_Move_Sub.D_Date >= '" + mtxtMakDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_Stock_Move_Sub.D_Date <= '" + mtxtMakDate2.Text.Replace("-", "").Trim() + "'";
            }

            //if (txtIOCode.Text.Trim() != "")
            //    strSql = strSql + " And tbl_Stock_Move_Sub.Out_FL = '" + txtIOCode.Text.Trim() + "'";

            if (txt_ItemName_Code2.Text.Trim() != "")
                strSql = strSql + " And tbl_Stock_Move_Sub.M_itemCode = '" + txt_ItemName_Code2.Text.Trim() + "'";

            //센타코드로으로 검색            
            if (txtCenter3_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Stock_Move_Sub.Move_From_Dep_Cd = '" + txtCenter3_Code.Text.Trim() + "'";

            if (txtCenter2_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Stock_Move_Sub.Move_To_Dep_Cd = '" + txtCenter2_Code.Text.Trim() + "'";

            if (txtR_Id_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Stock_Move_Sub.M_ID = '" + txtR_Id_Code.Text.Trim() + "'";

            if (txtR_Id2_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Stock_Move_Sub.D_ID = '" + txtR_Id2_Code.Text.Trim() + "'";

           // if (txtOrderNumber.Text.Trim() != "")
            //    strSql = strSql + " And tbl_SalesDetail.OrderNumber = '" + txtOrderNumber.Text.Trim() + "'";





            //if (opt_sell_2.Checked == true)
            //   strSql = strSql + " And (tbl_SalesitemDetail.SellState = ''N_1'' OR tbl_SalesitemDetail.SellState = ''N_3'' ) ";

            //if (opt_sell_3.Checked == true)
            //strSql = strSql + " And (tbl_SalesitemDetail.SellState = ''R_1'' OR tbl_SalesitemDetail.SellState = ''R_3'' ) ";

            //if (opt_sell_4.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.ReturnTF = 3 ";

            //if (opt_sell_5.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.ReturnTF = 4 ";

            //if (opt_Ed_2.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.UnaccMoney = 0 ";

            //if (opt_Ed_3.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.UnaccMoney <> 0 ";


            //Tsql = Tsql + " And  tbl_Stock_Move_Sub.Move_To_Dep_Cd in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
            //strSql = strSql + " And tbl_Stock_Move_Sub.Move_From_Dep_Cd in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            //strSql = strSql + " And tbl_Stock_Move_Sub.Move_To_Dep_Cd in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";


            Tsql = Tsql + " And ( tbl_Stock_Move_Sub.Move_To_Dep_Cd in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";

            Tsql = Tsql + " OR   tbl_Stock_Move_Sub.Move_From_Dep_Cd in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
            Tsql = Tsql + ") ";

            Tsql = Tsql + strSql;
            Tsql = Tsql + "  Order by Move_Date , Move_From_Dep_Cd , Move_To_Dep_Cd , M_itemCode  ";
           
        }




        private void Base_Grid_Set()
        {
            string Tsql = "";
            Make_Base_Query(ref Tsql);

            Make_Base_Query_(ref Tsql);            

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text, 1) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

           // double Sum_11 = 0; double Sum_12 = 0; // double Sum_12 = 0;
            //double Sum_13 = 0; //double Sum_14 = 0; double Sum_15 = 0;
            //double Sum_16 = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            Dictionary<string, int> dic_ItemCnt = new Dictionary<string, int>();
            Dictionary<string, int> dic_ItemCnt_D = new Dictionary<string, int>();
            string ItemCode = ""; int itemCnt = 0;
            

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                if (ds.Tables[base_db_name].Rows[fi_cnt]["D_Date"].ToString().Replace ("-","") != "")
                {
                    ItemCode = ds.Tables[base_db_name].Rows[fi_cnt]["M_itemCode"].ToString();
                    itemCnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["D_Cnt"].ToString());


                    if (dic_ItemCnt.ContainsKey(ItemCode) == true)
                        dic_ItemCnt_D[ItemCode] = dic_ItemCnt[ItemCode] + itemCnt;
                    else
                        dic_ItemCnt_D[ItemCode] = itemCnt;
                }

                ItemCode = ds.Tables[base_db_name].Rows[fi_cnt]["M_itemCode"].ToString();
                itemCnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["M_Cnt"].ToString());


                if (dic_ItemCnt.ContainsKey(ItemCode) == true)
                    dic_ItemCnt[ItemCode] = dic_ItemCnt[ItemCode] + itemCnt;
                else
                    dic_ItemCnt[ItemCode] = itemCnt;
            }

            if (gr_dic_text.Count > 0)
            {
            //    foreach (string t_key in dic_ItemCnt.Keys)
            //    {                    
            //        Push_data(series_Item, t_key, dic_ItemCnt[t_key]);
            //    }

            //    foreach (string t_key in dic_ItemCnt_D.Keys)
            //    {
            //        Push_data(series_Item_D, t_key, dic_ItemCnt_D[t_key]);
            //    }              
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();            
        }



        private void dGridView_Base_Header_Reset()
        {


            cgb.grid_col_Count = 20;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 3;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"출고지",  "출고지명"  , "이동일자"   , "입고지"  , "입고지명"          
                                , "상품코드"  , "상품명"   , "요청수량"    , "요청자"   , "비고"       
                                , ""    , ""   , "확정일자"    , "확정자"   , "확정수량"   
                                , "기록일"    , "기록자"   , ""    , ""   , ""   
                                };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 70,  120, 90, 70, 120                             
                             , 100 ,120 , 90 ,  120 , 300  
                             ,0 , 0 ,  100 , 130 ,  100   
                             ,130 , 100 ,  0 , 0 ,  0 
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                         
                                    ,true , true,  true,  true ,true    
                                    ,true , true,  true,  true ,true    
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft //10    
                               
                                ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleRight //15  

                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //20
                              };
            cgb.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb.grid_cell_format = gr_dic_cell_format;       
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
                                ,ds.Tables[base_db_name].Rows[fi_cnt][19]    
                                
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }



        private void MtxtData_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {

        }



        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
            

            if (tb.Name == "txtR_Id")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtR_Id_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtR_Id_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtR_Id_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }


            if (tb.Name == "txtR_Id2")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtR_Id2_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtR_Id2_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtR_Id2_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

          

            if (tb.Name == "txtCenter2")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter2_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtCenter2_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter2_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtCenter3")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter3_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtCenter3_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter3_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtIO")
            {

                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtIOCode);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtIOCode, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtIOCode);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

           

            if (tb.Name == "txt_ItemName2")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txt_ItemName_Code2);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txt_ItemName_Code2, "");
                //else
                //    Ncod_Text_Set_Data(tb, txt_ItemName_Code2);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }
        }


        private void Db_Grid_Popup(TextBox tb, TextBox tb1_Code)
        {
            cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            DataGridView Popup_gr = new DataGridView();
            Popup_gr.Name = "Popup_gr";
            this.Controls.Add(Popup_gr);
            cgb_Pop.basegrid = Popup_gr;
            cgb_Pop.Base_fr = this;
            cgb_Pop.Base_tb = tb1_Code;  //앞에게 코드
            cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            cgb_Pop.Base_Location_obj = tb;

            if (tb.Name == "txtCenter")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtCenter2")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtR_Id")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtR_Id2")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtChange")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtSellCode")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txt_Base_Rec")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txt_Receive_Method")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txt_ItemCode")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txt_ItemName2")
                cgb_Pop.Next_Focus_Control = butt_Select;

            cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
        }



        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);

            TextBox tb = (TextBox)sender;

            if ((tb.Tag == null) || (tb.Tag.ToString() == ""))
            {
                //숫자만 입력 가능하다.
                if (T_R.Text_KeyChar_Check(e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if   
            }
            else if ((tb.Tag != null) && (tb.Tag.ToString() == "1"))
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }


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






        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            int Sw_Tab = 0;

            if ((sender is TextBox) == false) return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
                Sw_Tab = 1;
            }

            //if (tb.Name == "txtCenter")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtCenter_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtCenter_Code);
            //}

            //if (tb.Name == "txtBank")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtSellCode_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtBank_Code);
            //}

            if (tb.Name == "txtR_Id")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtR_Id_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtR_Id2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtR_Id2_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtCenter2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter2_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtCenter3")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter3_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            //if (tb.Name == "txtSellCode")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtSellCode_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);
            //}

            if (tb.Name == "txtIO")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtIOCode.Text = "";
                Data_Set_Form_TF = 0;
            }


            if (tb.Name == "txt_ItemName2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txt_ItemName_Code2.Text = "";
                Data_Set_Form_TF = 0;
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
            cgb_Pop.Base_Location_obj = tb;

            if (strSql != "")
            {
                if (tb.Name == "txtCenter")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtBank")
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);

                if (tb.Name == "txtCenter2" || tb.Name == "txtCenter3")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtSellCode")
                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", strSql);

                if (tb.Name == "txtIO")
                    cgb_Pop.db_grid_Popup_Base(2, "입고_코드", "입고종류", "Ncode", "T_Name", strSql);

                if (tb.Name == "txt_ItemName2")
                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", strSql);
            }
            else
            {
                if (tb.Name == "txtCenter")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                }

                if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2")
                {
                    string Tsql;
                    Tsql = "Select user_id ,U_Name   ";
                    Tsql = Tsql + " From tbl_User (nolock) ";
                    Tsql = Tsql + " Order by user_id ";

                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", Tsql);
                }

                if (tb.Name == "txtBank")
                {
                    string Tsql;
                    Tsql = "Select Ncode ,BankName    ";
                    Tsql = Tsql + " From tbl_Bank (nolock) ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);
                }

                if (tb.Name == "txtCenter2" || tb.Name == "txtCenter3")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " And  ShowOrderCenter = 'Y' ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                }

                if (tb.Name == "txtSellCode")
                {
                    string Tsql;
                    Tsql = "Select SellCode ,SellTypeName    ";
                    Tsql = Tsql + " From tbl_SellType (nolock) ";
                    Tsql = Tsql + " Order by SellCode ";

                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", Tsql);
                }


                if (tb.Name == "txtIO")
                {
                    string Tsql;
                    Tsql = "Select Ncode ,T_Name    ";
                    Tsql = Tsql + " From tbl_Base_IO_Code (nolock) ";
                    Tsql = Tsql + " Where Kind_TF ='IO' And T_TF = 2  ";                    
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "입고_코드", "입고종류", "Ncode", "T_Name", Tsql);
                }

                if (tb.Name == "txt_ItemName2")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name    ";
                    Tsql = Tsql + " From tbl_Goods (nolock) ";
                    //Tsql = Tsql + " Where GoodUse = 0 ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", Tsql);
                }




            }
        }



        private void Ncod_Text_Set_Data(TextBox tb, TextBox tb1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            if (tb.Name == "txtCenter")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
            }

            if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2")
            {
                Tsql = "Select user_id ,U_Name   ";
                Tsql = Tsql + " From tbl_User (nolock) ";
                Tsql = Tsql + " Where U_Name like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    user_id like '%" + tb.Text.Trim() + "%'";
            }

            if (tb.Name == "txtBank")
            {
                Tsql = "Select Ncode , BankName   ";
                Tsql = Tsql + " From tbl_Bank (nolock) ";
                Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%'";
            }


            if (tb.Name == "txtCenter2" || tb.Name == "txtCenter3")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
            }


            if (tb.Name == "txtSellCode")
            {
                Tsql = "Select SellCode ,SellTypeName    ";
                Tsql = Tsql + " From tbl_SellType (nolock) ";
                Tsql = Tsql + " Where SellCode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    SellTypeName like '%" + tb.Text.Trim() + "%'";
            }

            if (tb.Name == "txtIO")
            {

                Tsql = "Select Ncode ,T_Name    ";
                Tsql = Tsql + " From tbl_Base_IO_Code (nolock) ";
                Tsql = Tsql + " Where Kind_TF ='IO' And T_TF = 2  ";                
                Tsql = Tsql + " And   Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    T_Name like '%" + tb.Text.Trim() + "%'";

            }


            if (tb.Name == "txt_ItemName2")
            {
                Tsql = "Select Ncode , Name    ";
                Tsql = Tsql + " From tbl_Goods (nolock) ";
                Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%'";
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




       




        private bool Check_TextBox_Error_Date()
        {
            //cls_Check_Input_Error c_er = new cls_Check_Input_Error();
            //if (mtxtMDate.Text.Replace("-", "").Trim() != "")
            //{
            //    int Ret = 0;
            //    Ret = c_er.Input_Date_Err_Check(txtMDate);

            //    if (Ret == -1)
            //    {
            //        txtMDate.Focus(); return false;
            //    }
            //}

            if (mtxtMDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtMDate.Text, mtxtMDate, "Date") == false)
                {
                    mtxtMDate.Focus();
                    return false;
                }
            }
            return true;
        }






        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                chart_Item.Series.Clear();
                series_Item.Points.Clear();
                series_Item_D.Points.Clear();

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, txt_ItemName2);

                opt_Ed_1.Checked = true; opt_sell_2.Checked = true;                
            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                chart_Item.Series.Clear();                
                Save_Nom_Line_Chart();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                if (Check_TextBox_Error() == false) return;
            
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Base_Grid_Set();  //뿌려주는 곳
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

            else if (bt.Name == "butt_Exp")
            {
                if (bt.Text == "...")
                {
                    grB_Search.Height = button_base.Top + button_base.Height + 3;
                    bt.Text = ".";
                }
                else
                {
                    grB_Search.Height = butt_Exp.Top + butt_Exp.Height + 3;
                    bt.Text = "...";
                }
            }

        }


        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Stock_Move_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }



        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            //if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            //{
            //    string Send_Nubmer = ""; string Send_Name = ""; ; string Send_OrderNumber = "";
            //    Send_OrderNumber = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
            //    Send_Nubmer = (sender as DataGridView).CurrentRow.Cells[3].Value.ToString();
            //    Send_Name = (sender as DataGridView).CurrentRow.Cells[4].Value.ToString();
            //    Send_Mem_Number(Send_Nubmer, Send_Name, Send_OrderNumber);   //부모한테 이벤트 발생 신호한다.
            //}            
        }

        private void dGridView_Base_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((sender as DataGridView).CurrentCell.ColumnIndex == 0)
            {
                DataGridView T_DGv = (DataGridView)sender;
                if ((T_DGv.CurrentCell.Value == null)
                || (T_DGv.CurrentCell.Value.ToString() == ""))
                {
                    T_DGv.CurrentCell.Value = "V";
                }
                else
                {
                    T_DGv.CurrentCell.Value = "";
                }
            }
        }





        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            //SendKeys.Send("{TAB}");
        }





        private void radioB_S_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtMDate, mtxtMDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }



        private void radioB_R_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtMakDate1, mtxtMakDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }





        private void Push_data(Series series, string p, int p_3)
        {
            DataPoint dp = new DataPoint();
            dp.SetValueXY(p, p_3);
            dp.Label = string.Format(cls_app_static_var.str_Currency_Type, p_3); // p_3.ToString(); //p_3.ToString();
            series.Points.Add(dp);
        }


        //Push_data(series_Item, nodeKey.ToString() + "Line", Save_Cnt[nodeKey]);
        private void Save_Nom_Line_Chart()
        {
            cls_form_Meth cm = new cls_form_Meth();
            //series_Item.Name = cm._chang_base_caption_search("상품별");            
            chart_Item.Series.Clear();
            series_Item.Points.Clear();
            series_Item["DrawingStyle"] = "Emboss";
            series_Item["PointWidth"] = "0.5";
            series_Item.Name = cm._chang_base_caption_search("요청");
            series_Item.ChartType = SeriesChartType.Column;
            series_Item.Legend = "Legend1";
            chart_Item.Series.Add(series_Item);

            series_Item_D.Points.Clear();
            series_Item_D["DrawingStyle"] = "Emboss";
            series_Item_D["PointWidth"] = "0.5";
            series_Item_D.Name = cm._chang_base_caption_search("확정");
            series_Item_D.ChartType = SeriesChartType.Column;
            series_Item_D.Legend = "Legend1";
            chart_Item.Series.Add(series_Item_D);

            chart_Item.ChartAreas[0].AxisX.Interval = 1;
            chart_Item.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_Item.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 7;
        }







    }
}
