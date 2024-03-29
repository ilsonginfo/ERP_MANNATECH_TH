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
    public partial class frmClose_100_Select_01 : Form
    {
        



        private const string base_db_name = "tbl_DB";
        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Sum = new cls_Grid_Base();
        private int Data_Set_Form_TF = 0; 
        private int Form_Load_TF = 0;

        public frmClose_100_Select_01()
        {
            InitializeComponent();
        }




        private void frmBase_Resize(object sender, EventArgs e)
        {


            int base_w = this.Width / 4;
            butt_Clear.Width = base_w;
            butt_Select.Width = base_w;
            butt_Excel.Width = base_w;
            //butt_Delete.Width = base_w;
            butt_Exit.Width = base_w;

            butt_Clear.Left = 0;
            butt_Select.Left = butt_Clear.Left + butt_Clear.Width;
            butt_Excel.Left = butt_Select.Left + butt_Select.Width;
            //butt_Delete.Left = butt_Excel.Left + butt_Excel.Width;
            butt_Exit.Left = butt_Excel.Left + butt_Excel.Width;
        }

        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Form_Load_TF = 0;

            
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
        }

        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (Form_Load_TF == 0)
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                dGridView_Sum_Base_Header_Reset();
                cgb_Sum.d_Grid_view_Header_Reset(1);
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                Form_Load_TF = 1;
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
                            //  cfm.form_Group_Panel_Enable_True(this);
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



        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                dGridView_Sum_Base_Header_Reset();
                cgb_Sum.d_Grid_view_Header_Reset(1);

                chart_Pay.Series.Clear();
                chart_Cnt.Series.Clear();        
                tabControl1.SelectedIndex = 0;

                textToEndDate.Text = "";
                tab_Pay_Tab_Dispose();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                cls_form_Meth ct = new cls_form_Meth();
                
                ct.from_control_clear(tabControl2);
                ct.from_control_clear(this, txtFromDate1);
                
                
            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                dGridView_Sum_Base_Header_Reset();
                cgb_Sum.d_Grid_view_Header_Reset(1);

                chart_Pay.Series.Clear();
                chart_Cnt.Series.Clear();                
                Save_Nom_Line_Chart();
                tabControl1.SelectedIndex = 0;

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(tabControl2);
                tab_Pay_Tab_Dispose();                
                
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
            cls_form_Meth cm = new cls_form_Meth();
            Excel_Export_File_Name = this.Text; // cm._chang_base_caption_search ("센타_마감별_집계");
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;


        }






        private void Make_Base_Query(ref string Tsql)
        {


            //string[] g_HeaderText = {"마감_시작일"  ,"마감_종료일"  ,"지급_일자" , "기간판매액"   , "기간판매PV"      
            //                    , "기간반품액"     , "기간반품PV"  , ""   , ""    , ""   
            //                    , ""  , "", ""   , ""  , ""   
            //                    , ""   , "" , ""     , ""    , ""     
            //                    , ""  , "" , "기간수당합계"  , "기간소득세"  , "기간주민세"  
            //                    ,"기간실지급액"        , "기간지급률"
            //                        };
            cls_form_Meth cm = new cls_form_Meth();

            //스타보너스 ,  바이너리보너스 , 추천매칭
            Tsql = "Select  ";

            Tsql = Tsql + " LEFT(FromEndDate,4) +'-' + LEFT(RIGHT(FromEndDate,4),2) + '-' + RIGHT(FromEndDate,2) FromEndDate ";
            Tsql = Tsql + " , LEFT(ToEndDate,4) +'-' + LEFT(RIGHT(ToEndDate,4),2) + '-' + RIGHT(ToEndDate,2) ToEndDate ";
            Tsql = Tsql + " , LEFT(PayDate,4) +'-' + LEFT(RIGHT(PayDate,4),2) + '-' + RIGHT(PayDate,2) PayDate";
            Tsql = Tsql + " , TotalSellAmount , TotalSellPv ";

            Tsql = Tsql + " , TotalReturnSellAmount , TotalReturnSellPV , Allowance1 , Allowance2 , Allowance3  ";

            Tsql = Tsql + " , Allowance1Cnt, Allowance2Cnt ,  Allowance3Cnt , SumAllowanceCount , 0 ";

            Tsql = Tsql + " ,0 , 0 ,  0 , 0 , Allowance28 ";

            Tsql = Tsql + " , Allowance29 , Allowance30 , SumAllowance , SumInComeTax , SumResidentTax "; 
            Tsql = Tsql + " , SumTruePayment , SumAllowanceRate ,'','',''  ";


            Tsql = Tsql + " , Allowance1Rate , Allowance2Rate ,Allowance3Rate , Allowance4Rate ,Allowance5Rate ";
            Tsql = Tsql + " , Allowance6Rate , Allowance7Rate ,Allowance8Rate , Allowance9Rate ,Allowance10Rate ";

            Tsql = Tsql + " , My_OF_View_TF ";

            Tsql = Tsql + " From tbl_CloseTotal_100 (nolock) ";            
        }



        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = " Where ToEndDate <> ''  ";

            //string Mbid = ""; int Mbid2 = 0;
            ////회원번호1로 검색
            //if (
            //    (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            //    &&
            //    (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() == "")
            //    )
            //{
            //    cls_Search_DB csb = new cls_Search_DB();
            //    if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
            //    {
            //        if (Mbid != "")
            //            strSql = strSql + " And tbl_SalesDetail.Mbid ='" + Mbid + "'";

            //        if (Mbid2 >= 0)
            //            strSql = strSql + " And tbl_SalesDetail.Mbid2 = " + Mbid2;
            //    }


            //}

            ////회원번호2로 검색
            //if (
            //    (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            //    &&
            //    (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
            //    )
            //{
            //    cls_Search_DB csb = new cls_Search_DB();
            //    if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
            //    {
            //        if (Mbid != "")
            //            strSql = strSql + " And tbl_SalesDetail.Mbid >='" + Mbid + "'";

            //        if (Mbid2 >= 0)
            //            strSql = strSql + " And tbl_SalesDetail.Mbid2 >= " + Mbid2;
            //    }

            //    if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
            //    {
            //        if (Mbid != "")
            //            strSql = strSql + " And tbl_SalesDetail.Mbid <='" + Mbid + "'";

            //        if (Mbid2 >= 0)
            //            strSql = strSql + " And tbl_SalesDetail.Mbid2 <= " + Mbid2;
            //    }
            //}


            ////회원명으로 검색
            //if (txtName.Text.Trim() != "")
            //    strSql = strSql + " And tbl_SalesDetail.M_Name Like '%" + txtName.Text.Trim() + "%'";

            //가입일자로 검색 -1
            if ((txtFromDate1.Text.Trim() != "") && (txtFromDate2.Text.Trim() == ""))
                strSql = strSql + " And FromEndDAte = '" + txtFromDate1.Text.Trim() + "'";

            //가입일자로 검색 -2
            if ((txtFromDate1.Text.Trim() != "") && (txtFromDate2.Text.Trim() != ""))
            {
                strSql = strSql + " And FromEndDAte >= '" + txtFromDate1.Text.Trim() + "'";
                strSql = strSql + " And FromEndDate <= '" + txtFromDate2.Text.Trim() + "'";
            }


            //기록일자로 검색 -1
            if ((txtToDate1.Text.Trim() != "") && (txtToDate2.Text.Trim() == ""))
                strSql = strSql + " And ToEndDate = '" + txtToDate1.Text.Trim() + "'";

            //기록일자로 검색 -2
            if ((txtToDate1.Text.Trim() != "") && (txtToDate2.Text.Trim() != ""))
            {
                strSql = strSql + " And ToEndDate >= '" + txtToDate1.Text.Trim() + "'";
                strSql = strSql + " And ToEndDate <= '" + txtToDate2.Text.Trim() + "'";
            }


            //기록일자로 검색 -1
            if ((txtPayDate1.Text.Trim() != "") && (txtPayDate2.Text.Trim() == ""))
                strSql = strSql + " And PayDate = '" + txtPayDate1.Text.Trim() + "'";

            //기록일자로 검색 -2
            if ((txtPayDate1.Text.Trim() != "") && (txtPayDate2.Text.Trim() != ""))
            {
                strSql = strSql + " And PayDate >= '" + txtPayDate1.Text.Trim() + "'";
                strSql = strSql + " And PayDate <= '" + txtPayDate2.Text.Trim() + "'";
            }



            Tsql = Tsql + strSql;
            Tsql = Tsql + " Order by ToEndDAte DESC ";            
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
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();           

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

               
            }

            //Reset_Chart_Total(Sum_13, Sum_14, Sum_15, Sum_17);
            //Reset_Chart_Total(ref SelType_1);
            //Reset_Chart_Total(Sell_Cnt_1, Sell_Cnt_2);



            

            if (gr_dic_text.Count > 0)
            {
                put_Sum_Dataview(ds, ReCnt);
                
                put_Chart(ds, ReCnt);               
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }

       


        private void dGridView_Base_Header_Reset()
        {

            cgb.grid_col_Count = 41;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //스타보너스 ,  바이너리보너스 , 추천매칭
            string[] g_HeaderText = {"마감_시작일"  ,"마감_종료일"  ,"지급_일자" , "기간판매액"   , "기간판매PV"      
                                , "기간반품액"     , "기간반품PV"  , "센타보너스"  ,""   , ""    
                                , ""  , "", ""   , ""  , ""   
                                , ""   , "" , ""     , ""    , ""     
                                , "반품공제액"  , "기타보너스" , "기간수당합계"  , "기간소득세"  , "기간주민세"  
                                ,"기간실지급액"        , "기간지급률" , "", "" ,"" 

                                , ""  , "", ""   , ""  , ""   
                                , ""  , "", ""   , ""  , ""   
                                ,""
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 80, 130 , 130, 90, 110
                            , 90, 90   ,101, 0, 0
                            , 0 , 0  ,0 , 0, 0
                             , 0, 0,0 , 0, 0 
                             , 100 , 100,100 , 100 , 100 
                             , 100, 100 ,0,0,0

                             , 0 , 0  ,0 , 0, 0
                             , 0 , 0  ,0 , 0, 0
                             ,0
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  

                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  

                                    ,true  
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight//5    
  
                               ,DataGridViewContentAlignment.MiddleRight                            
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight//10

                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  //15   

                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight                            
                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight//20

                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  //25   

                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  //30

                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  //35

                                ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  //40

                               ,DataGridViewContentAlignment.MiddleCenter  
                              };
            cgb.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[16 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[17 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[18 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[19 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[20 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[21 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[22 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[23 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[24 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[25 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[26 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb.grid_cell_format = gr_dic_cell_format;

        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;
            object[] row0 = new object[cgb.grid_col_Count];

            while (Col_Cnt < cgb.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;

        }


        private void put_Sum_Dataview(DataSet ds, int ReCnt)
        {
            Dictionary<int, object[]> gr_dic_text_Sum = new Dictionary<int, object[]>();
            Dictionary<string, double> Sum_dic = new Dictionary<string, double>();
            cls_form_Meth cm = new cls_form_Meth();
                      
            
            Sum_dic["센타보너스"] = 0;
            //Sum_dic["바이너리보너스"] = 0;
            //Sum_dic["추천매칭"] = 0;
            //Sum_dic["바이너리공제"] = 0;

            Sum_dic["반품공제액"] = 0;
            Sum_dic["기타보너스"] = 0;
            Sum_dic["수당합"] = 0;
            Sum_dic["소득세합"] = 0;
            Sum_dic["주민세합"] = 0;
            Sum_dic["실지급액합"] = 0;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Sum_dic["센타보너스"] = Sum_dic["센타보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance1"].ToString());
                //Sum_dic["바이너리보너스"] = Sum_dic["바이너리보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance2"].ToString());
                //Sum_dic["추천매칭"] = Sum_dic["추천매칭"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance3"].ToString());
                //Sum_dic["바이너리공제"] = Sum_dic["바이너리공제"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance28"].ToString());
              
                Sum_dic["반품공제액"] = Sum_dic["반품공제액"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance29"].ToString());
                Sum_dic["기타보너스"] = Sum_dic["기타보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance30"].ToString());
                Sum_dic["수당합"] = Sum_dic["수당합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumAllowance"].ToString());
                Sum_dic["소득세합"] = Sum_dic["소득세합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumInComeTax"].ToString());
                Sum_dic["주민세합"] = Sum_dic["주민세합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumResidentTax"].ToString());
                Sum_dic["실지급액합"] = Sum_dic["실지급액합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumTruePayment"].ToString());
            }

            int f_cnt = 0 ;
            foreach (string t_key in Sum_dic.Keys )
            {
                object[] row0 = { cm._chang_base_caption_search (t_key)
                                ,string.Format(cls_app_static_var.str_Currency_Type, Sum_dic[t_key]) 
                                ,""
                                ,""
                                ,""                                
                           
                            };

                gr_dic_text_Sum[f_cnt] = row0;
                f_cnt++;
            }
            

            cgb_Sum.grid_name_obj = gr_dic_text_Sum;  //배열을 클래스로 보낸다.
            cgb_Sum.db_grid_Obj_Data_Put();            
        }


        private void dGridView_Sum_Base_Header_Reset()
        {
            cgb_Sum.grid_col_Count = 5;
            cgb_Sum.basegrid = dGridView_Base_Sum;
            cgb_Sum.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;            
            cgb_Sum.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            cgb_Sum.basegrid.RowHeadersVisible = false;
            cgb_Sum.Sort_Mod_Auto_TF =1 ;
            //스타보너스 ,  바이너리보너스 , 추천매칭
            string[] g_HeaderText = {"구분"  ,"합계금액"  ,"" , ""   , ""                                      
                                    };
            cgb_Sum.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 150 , 0, 0, 0                            
                            };
            cgb_Sum.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                                                         
                                   };
            cgb_Sum.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight//5      
                                                          
                              };
            cgb_Sum.grid_col_alignment = g_Alignment;                                 
        }



        private Boolean Check_TextBox_Error()
        {

            cls_Check_Input_Error c_er = new cls_Check_Input_Error();


            if (txtFromDate1.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtFromDate1);

                if (Ret == -1)
                {
                    txtFromDate1.Focus(); return false;
                }
            }

            if (txtFromDate2.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtFromDate2);

                if (Ret == -1)
                {
                    txtFromDate2.Focus(); return false;
                }
            }


            if (txtToDate1.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtToDate1);

                if (Ret == -1)
                {
                    txtToDate1.Focus(); return false;
                }
            }

            if (txtToDate2.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtToDate2);

                if (Ret == -1)
                {
                    txtToDate2.Focus(); return false;
                }
            }


            if (txtPayDate1.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtPayDate1);

                if (Ret == -1)
                {
                    txtPayDate1.Focus(); return false;
                }
            }

            if (txtPayDate2.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtPayDate2);

                if (Ret == -1)
                {
                    txtPayDate2.Focus(); return false;
                }
            }
            return true;
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


        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
            //T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);

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

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "ncode1")) //코드관련해서 코드를치면 관련 내역이 나오도록 하기 위함.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.                
                if (T_R.Text_KeyChar_Check(e, tb, "", 0) == false)  //숫자만 입력 받아야 하고 호출도 해야함
                {
                    e.Handled = true;
                    return;
                } // end if
            }

        }

        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
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
            ct.Search_Date_TextBox_Put(txtFromDate1, txtFromDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }



        private void radioB_R_Base_Click(object sender, EventArgs e)
        {
            Data_Set_Form_TF = 1;
            //RadioButton _Rb = (RadioButton)sender;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(txtToDate1, txtToDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }

        private void radioB_P_Base_Click(object sender, EventArgs e)
        {
            Data_Set_Form_TF = 1;
            //RadioButton _Rb = (RadioButton)sender;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(txtPayDate1, txtPayDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }




        private void put_Chart(DataSet ds, int ReCnt)
        {
            Dictionary<string, double> dic_Pay_1 = new Dictionary<string, double>();
            Dictionary<string, double> dic_Pay_2 = new Dictionary<string, double>();
            Dictionary<string, double> dic_Pay_3 = new Dictionary<string, double>();
            Dictionary<string, double> dic_Pay_tur = new Dictionary<string, double>();

            Dictionary<string, double> dic_Cnt1 = new Dictionary<string, double>();
            Dictionary<string, double> dic_Cnt2 = new Dictionary<string, double>();
            Dictionary<string, double> dic_Cnt3 = new Dictionary<string, double>();
            Dictionary<string, double> dic_Cnt_tur = new Dictionary<string, double>();

            double Pay1 = 0, Pay2 = 0, Pay3 = 0, tur = 0;
            int Cnt1 = 0, Cnt2 = 0, Cnt3 = 0, turCnt = 0;
            string ToDate = "";

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                ToDate = ds.Tables[base_db_name].Rows[fi_cnt]["ToEndDate"].ToString();

                Pay1 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance1"].ToString());
                //Pay2 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance2"].ToString());
                //Pay3 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance3"].ToString());
                tur = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumTruePayment"].ToString());

                Cnt1 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance1Cnt"].ToString());
                //Cnt2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance2Cnt"].ToString());
                //Cnt3 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance3Cnt"].ToString());
                turCnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumAllowanceCount"].ToString());


                dic_Pay_1[ToDate] = Pay1;
                //dic_Pay_2[ToDate] = Pay2;
                //dic_Pay_3[ToDate] = Pay3;
                dic_Pay_tur[ToDate] = tur;

                dic_Cnt1[ToDate] = Cnt1;
                //dic_Cnt2[ToDate] = Cnt2;
                //dic_Cnt3[ToDate] = Cnt3;
                dic_Cnt_tur[ToDate] = turCnt;
            }

            Series series_Pay = new Series();
            Series series_Cnt = new Series();
            Put_Series_Pay(series_Pay, "센타보너스");
            Put_Series_Cnt(series_Cnt, "센타보너스");
            foreach (string t_key in dic_Pay_1.Keys)
            {
                Push_data(series_Pay, t_key, dic_Pay_1[t_key]);
                Push_data(series_Cnt, t_key, dic_Cnt1[t_key]);
            }

            //Series series_Pay2 = new Series();
            //Series series_Cnt2 = new Series();
            //Put_Series_Pay(series_Pay2, "바이너리");
            //Put_Series_Cnt(series_Cnt2, "바이너리");
            //foreach (string t_key in dic_Pay_2.Keys)
            //{
            //    Push_data(series_Pay2, t_key, dic_Pay_2[t_key]);
            //    Push_data(series_Cnt2, t_key, dic_Cnt2[t_key]);
            //}

            //Series series_Pay3 = new Series();
            //Series series_Cnt3 = new Series();
            //Put_Series_Pay(series_Pay3, "매칭");
            //Put_Series_Cnt(series_Cnt3, "매칭");
            //foreach (string t_key in dic_Pay_3.Keys)
            //{
            //    Push_data(series_Pay3, t_key, dic_Pay_3[t_key]);
            //    Push_data(series_Cnt3, t_key, dic_Cnt3[t_key]);
            //}


            Series series_Pay4 = new Series();
            Series series_Cnt4 = new Series();
            Put_Series_Pay(series_Pay4, "실지급액");
            Put_Series_Cnt(series_Cnt4, "실지급액");
            foreach (string t_key in dic_Pay_tur.Keys)
            {
                Push_data(series_Pay4, t_key, dic_Pay_tur[t_key]);
                Push_data(series_Cnt4, t_key, dic_Cnt_tur[t_key]);
            }

        }



        private void Push_data(Series series, string p, double  p_3)
        {
            DataPoint dp = new DataPoint();
            dp.SetValueXY(p, p_3);
            dp.Label = string.Format(cls_app_static_var.str_Currency_Type, p_3); // p_3.ToString(); //p_3.ToString();
            series.Points.Add(dp);
        }



        private void Put_Series_Pay(Series series_Pay, string PayName)
        {
            cls_form_Meth cm = new cls_form_Meth();

            series_Pay.Points.Clear();
            series_Pay["DrawingStyle"] = "Emboss";
            series_Pay["PointWidth"] = "0.5";
            series_Pay.Name = cm._chang_base_caption_search(PayName);
            series_Pay.ChartType = SeriesChartType.Column;
            series_Pay.Legend = "Legend1";
            chart_Pay.Series.Add(series_Pay);
        }

        private void Put_Series_Cnt(Series series_Pay, string PayName)
        {
            cls_form_Meth cm = new cls_form_Meth();

            series_Pay.Points.Clear();
            series_Pay["DrawingStyle"] = "Emboss";
            series_Pay["PointWidth"] = "0.5";
            series_Pay.Name = cm._chang_base_caption_search(PayName);
            series_Pay.ChartType = SeriesChartType.Column;
            series_Pay.Legend = "Legend1";
            chart_Cnt.Series.Add(series_Pay);
        }


        private void Save_Nom_Line_Chart()
        {
            chart_Pay.Series.Clear();
            chart_Pay.ChartAreas[0].AxisX.Interval = 1;
            chart_Pay.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_Pay.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 7;

            chart_Cnt.Series.Clear();
            chart_Cnt.ChartAreas[0].AxisX.Interval = 1;
            chart_Cnt.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_Cnt.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 7;
        }


                    
            //Sum_dic["스타트보너스"] = 0;
            //Sum_dic["바이너리보너스"] = 0;
            //Sum_dic["추천매칭"] = 0;
            //Sum_dic["바이너리공제"] = 0;

            //Sum_dic["반품공제액"] = 0;
            //Sum_dic["기타보너스"] = 0;
            //Sum_dic["수당합"] = 0;
            //Sum_dic["소득세합"] = 0;
            //Sum_dic["주민세합"] = 0;
            //Sum_dic["실지급액합"] = 0;

        private void dGridView_Base_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tab_Pay_Tab_Dispose();
            

            //"회원번호", "성명", "마감_시작일"  ,"마감_종료일"  ,"지급_일자"  
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string  ToEndDate = "";

                ToEndDate = dGridView_Base.CurrentRow.Cells[1].Value.ToString();
                ToEndDate = ToEndDate.Replace("-", "");
                textToEndDate.Text = ToEndDate;

                txt_ETC1.Text = dGridView_Base.CurrentRow.Cells[30].Value.ToString();
                //txt_ETC2.Text = (sender as DataGridView).CurrentRow.Cells[31].Value.ToString();
                //txt_ETC3.Text = (sender as DataGridView).CurrentRow.Cells[32].Value.ToString();

                if ((sender as DataGridView).CurrentRow.Cells[40].Value.ToString() == "1")
                {
                    chk_Web.Checked = true;
                }
                else
                {
                    chk_Web.Checked = false;
                }

                Allowance_Detail(ToEndDate);
                

            }
        }

        private void Allowance_Detail(string ToEndDate)
        {          

            cls_form_Meth cm = new cls_form_Meth();

            string Pay_c = "",  fild_name = "" ;
            for (int f_cnt = 1; f_cnt <= 2; f_cnt++)
            {
                if (f_cnt == 1)
                {
                    Pay_c = "센타보너스";
                    fild_name = " Allowance1 " ;
                }

                //if (f_cnt == 2) 
                //{   
                //    Pay_c = "바이너리보너스";
                //    fild_name = " Allowance2 " ;
                //}

                //if (f_cnt == 3)
                //{
                //    Pay_c = "추천매칭";
                //    fild_name = " Allowance3 " ;
                //}

                if (f_cnt == 2)
                {
                    Pay_c = cm._chang_base_caption_search("수당합");
                    fild_name = " SumAllAllowance " ;
                }


                if (f_cnt == 1) 
                {
                    tab_Pay.TabPages[0].Text = Pay_c;
                                        
                    cls_Grid_Base cgb_P1 = new cls_Grid_Base();
                    dGridView_Base_Header_Reset(dGridView_Pay, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
                    cgb_P1.d_Grid_view_Header_Reset();
                    Real_Allowance_Detail(ToEndDate, fild_name, cgb_P1); 
                }
                else
                {
                    DataGridView t_DGV = new DataGridView();
                    TabPage t_tp = new TabPage();
                   
                    t_DGV.Name = Pay_c;
                    t_tp.Text = Pay_c;
                    t_tp.BackColor = tab_Pay.TabPages[0].BackColor;
                    t_tp.Controls.Add(t_DGV);
                    
                    t_DGV.Dock = DockStyle.Fill;
                    t_DGV.BackgroundColor = dGridView_Pay.BackgroundColor;

                    cls_Grid_Base cgb_P1 = new cls_Grid_Base();
                    dGridView_Base_Header_Reset(t_DGV, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
                    cgb_P1.d_Grid_view_Header_Reset();
                    Real_Allowance_Detail(ToEndDate, fild_name, cgb_P1); 

                    tab_Pay.Controls.Add(t_tp);     
                }

                tab_Pay.Refresh();
            }
        }




        private void Real_Allowance_Detail(string ToEndDate, string fild_name,  cls_Grid_Base cgb_P)
        {
            string StrSql = "";

            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " Mbid + '-' + Convert(Varchar,Mbid2) ";
            else
                StrSql = StrSql + " Mbid2 ";

            StrSql = StrSql + ",M_Name ,  " +  fild_name + " , '' , '' ";
            StrSql = StrSql + " From  tbl_ClosePay_100_Mod (nolock) ";
            StrSql = StrSql + " Where ToEndDate = '" + ToEndDate + "'";
            StrSql = StrSql + " And " + fild_name + " > 0 ";
            StrSql = StrSql + " Order By Mbid, Mbid2 ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_Pay_gr_dic(ref ds, ref gr_dic_text, fi_cnt, cgb_P);  //데이타를 배열에 넣는다.
            }

            if (gr_dic_text.Count > 0)
            {
                //put_Sum_Dataview(ds, ReCnt);                
            }

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();

        }



        private void dGridView_Base_Header_Reset(DataGridView dGridView, cls_Grid_Base cgb_P)
        {

            cgb_P.grid_col_Count = 5;
            cgb_P.basegrid = dGridView;
            cgb_P.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_P.grid_Frozen_End_Count = 3;
            cgb_P.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"회원번호","성명", "금액", ""  ,""                             
                                };
            cgb_P.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 100 , 100, 0, 0
                            };
            cgb_P.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                                                         
                                   };
            cgb_P.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter//5      
                               
                              };
            cgb_P.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;            
            cgb_P.grid_cell_format = gr_dic_cell_format;
        }




        private void Set_Pay_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt, cls_Grid_Base cgb_P)
        {
            int Col_Cnt = 0;
            object[] row0 = new object[cgb_P.grid_col_Count];

            while (Col_Cnt < cgb_P.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;
        }



        private void tab_Pay_Tab_Dispose()
        {
            for (int fcnt = tab_Pay.TabCount - 1; fcnt > 0; fcnt--)
            {
                tab_Pay.TabPages[fcnt].Dispose();
            }

            tab_Pay.TabPages[0].Text = "";
            tab_Pay.Refresh();

            cls_Grid_Base cgb_P1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(dGridView_Pay, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
        }


        private void chk_Web_MouseClick(object sender, MouseEventArgs e)
        {
            string StrSql  = "";

            StrSql = "Update tbl_CloseTotal_100 Set " ;
            if (chk_Web.Checked == true )
            {
                StrSql = StrSql + " My_OF_View_TF =   1 "  ;
                dGridView_Base.CurrentRow.Cells[40].Value = "1";
            }
            else
            {
                StrSql = StrSql + " My_OF_View_TF =   0 "  ;
                dGridView_Base.CurrentRow.Cells[40].Value = "0";
            }

            StrSql = StrSql + " Where ToendDate = '" + textToEndDate.Text.Trim() + "'";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            

            Temp_Connect.Insert_Data(StrSql, "",this.Name, this.Text );

            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
        }








    }
}
