using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Data;
using System.Drawing;


namespace MLM_Program
{
    class cls_Grid_Base_info_Put
    {
        cls_Grid_Base Base_dgv = new cls_Grid_Base();
        private string base_db_name = "Temp_Table";

        public void dGridView_Put_baseinfo(Form fr, DataGridView t_Dgv, string intTemp, string Mbid, string Ordernumber = "")
        {
            Base_dgv.Grid_Base_Arr_Clear();
            Base_dgv.basegrid = t_Dgv;
            Base_dgv.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            Base_dgv.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            Base_dgv.Sort_Mod_Auto_TF = 1;

            if (intTemp == "sell")
                Sell_dGridView_Info_Header_Reset(t_Dgv);

            if (intTemp == "memc")
                Mem_change_dGridView_Info_Header_Reset(t_Dgv);

            if (intTemp == "memupc")
                Mem_UP_change_dGridView_Info_Header_Reset(t_Dgv);

            if (intTemp == "memadd")
                Mem_Add_dGridView_Info_Header_Reset(t_Dgv);

            if (intTemp == "item")
                dGridView_Sell_Item_Header_Reset();
            if (intTemp == "item_mem")
                dGridView_Sell_Item_mem_Header_Reset();
            if (intTemp == "cacu")
                dGridView_Sell_Cacu_Header_Reset();
            if (intTemp == "rece")
                dGridView_Sell_Rece_Header_Reset();

            if (intTemp == "pay")
                dGridView_Pay_Header_Reset();

            if (intTemp == "talk")
                dGridView_Talk_Header_Reset();

            if (intTemp == "member")
                dGridView_Member_Header_Reset();

            if (intTemp == "RePay_D2")
                dGridView_RePay_D2_info_Header_Reset();

            if (intTemp == "RePay_D4")
                dGridView_RePay_D4_info_Header_Reset();
            if (intTemp == "gold")
                dGridView_gold();
            Base_dgv.basegrid.RowHeadersVisible = false;

            if (intTemp == "saveup" || intTemp == "nominup" || intTemp == "savedown" || intTemp == "nomindown" || intTemp == "savedefault")
                dGridView_Save_Up_Header_Reset();

            Base_dgv.d_Grid_view_Header_Reset();



            Base_Grid_info_Set(fr, intTemp, Mbid, Ordernumber);
        }


        public void dGridView_Put_baseinfo(DataGridView t_Dgv, string intTemp)
        {
            Base_dgv.Grid_Base_Arr_Clear();
            Base_dgv.basegrid = t_Dgv;
            Base_dgv.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            Base_dgv.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            if (intTemp == "sell")
                Sell_dGridView_Info_Header_Reset(t_Dgv);

            if (intTemp == "memc")
                Mem_change_dGridView_Info_Header_Reset(t_Dgv);

            if (intTemp == "memupc")
                Mem_UP_change_dGridView_Info_Header_Reset(t_Dgv);

            if (intTemp == "memadd")
                Mem_Add_dGridView_Info_Header_Reset(t_Dgv);

            if (intTemp == "item")
                dGridView_Sell_Item_Header_Reset();
            if (intTemp == "item_mem")
                dGridView_Sell_Item_mem_Header_Reset();
            if (intTemp == "cacu")
                dGridView_Sell_Cacu_Header_Reset();
            if (intTemp == "rece")
                dGridView_Sell_Rece_Header_Reset();

            if (intTemp == "pay")
                dGridView_Pay_Header_Reset();

            if (intTemp == "member")
                dGridView_Member_Header_Reset();

            if (intTemp == "talk")
                dGridView_Talk_Header_Reset();

            if (intTemp == "RePay_D2")
                dGridView_RePay_D2_info_Header_Reset();



            Base_dgv.basegrid.RowHeadersVisible = false;


            if (intTemp == "saveup" || intTemp == "nominup" || intTemp == "savedown" || intTemp == "nomindown" || intTemp == "savedefault")
                dGridView_Save_Up_Header_Reset();

            Base_dgv.d_Grid_view_Header_Reset();

        }


        private void Sell_dGridView_Info_Header_Reset(DataGridView t_Dgv)
        {

            Base_dgv.grid_col_Count = 13;

            string[] g_HeaderText = {"승인여부"  , "매출_일자" ,  "주문번호" ,  "주문_종류"   , "상태"
                                     , "매출액"  , "입급액"  ,"매출PV" , "매출CV" , "현금"
                                     , "카드"    , "무통장" , "비고"
                                    };

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            Base_dgv.grid_cell_format = gr_dic_cell_format;

            int[] g_Width = { 80,100, 90, 70, 80
                                , 80 , 80 , 80 , 80 ,80
                                , 80 , 80 , 100
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {
                                DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter//5     

                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //10

                                ,DataGridViewContentAlignment.MiddleRight  //11
                                ,DataGridViewContentAlignment.MiddleRight  //12
                                ,DataGridViewContentAlignment.MiddleCenter
                                };

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                ,true , true,  true,  true ,true
                                ,true    ,true     ,true
                                };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        }// end Sell_dGridView_Info_Header_Reset




        private void Mem_change_dGridView_Info_Header_Reset(DataGridView t_Dgv)
        {
            Base_dgv.grid_col_Count = 11;

            string[] g_HeaderText = {"변경일"  , "변경내역"   , "전_내역"  , "후_내역"   , "변경자"
                                , ""   , ""    , ""  , "" , ""
                                ,""
                                };

            int[] g_Width = { 120, 100, 100, 100, 80
                                ,0 , 0 , 0 , 0 , 0
                                ,0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //10

                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                ,true , true,  true,  true ,true
                                ,true
                                };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        }//  end Mem_change_dGridView_Info_Header_Reset



        private void Mem_UP_change_dGridView_Info_Header_Reset(DataGridView t_Dgv)
        {
            Base_dgv.grid_col_Count = 11;
            string[] g_HeaderText = {"변경일"  , "전_상위번호"   , "전_상위성명"  , "후_상위번호"   , "후_상위성명"
                                , "구분"   , "변경자"    , ""  , "" , ""
                                ,""
                                };

            int[] g_Width = { 120, 100, 100, 100, 100
                                ,80 , 80 , 0 , 0 , 0
                                ,0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //10

                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                ,true , true,  true,  true ,true
                                ,true
                                };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        } // end Mem_UP_change_dGridView_Info_Header_Reset


        private void Mem_Add_dGridView_Info_Header_Reset(DataGridView t_Dgv)
        {
            Base_dgv.grid_col_Count = 11;
            string[] g_HeaderText = {"구분"  , "우편_번호"   , "주소1"  , "주소2"   , "연락처1"
                                , "연락처2"   , "수취인명"    , ""  , "" , ""
                                ,""
                                };

            int[] g_Width = { 120, 100, 100, 100, 100
                                ,80 , 80 , 0 , 0 , 0
                                ,0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft  //5    
  
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //10

                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                ,true , true,  true,  true ,true
                                ,true
                                };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        } // Mem_Add_dGridView_Info_Header_Reset



        private void dGridView_Sell_Item_Header_Reset()
        {

            Base_dgv.grid_col_Count = 13;


            string[] g_HeaderText = {""  , "상품_코드"   , "상품명"  , "개별단가"   , "개별PV"
                                , "개별CV", "주문_수량"   , "총상품액"    , "총상품PV"  , "총상품CV"
                                , "구분" , "비고" ,"주문번호"
                                };

            int[] g_Width = { 0, 90, 160, 80, 70
                            ,  70,  80 ,  80 ,  80 ,  70
                            , 70, 200,100
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight //10

                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;


            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                     ,true  , true, true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;

            Base_dgv.basegrid.RowHeadersVisible = false;
        }

        private void dGridView_Sell_Item_mem_Header_Reset()
        {

            Base_dgv.grid_col_Count = 13;


            string[] g_HeaderText = {""  , "상품_코드"   , "상품명"  , "개별단가"   , "개별PV"
                                , "개별CV", "주문_수량"   , "총상품액"    , "총상품PV"  , "총상품CV"
                                , "구분" , "비고" ,"주문번호"
                                };

            int[] g_Width = { 0, 90, 160, 80, 0
                            ,  0,  80 ,  80 ,  0 ,  0
                            , 70, 200,100
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight //10

                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;


            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                     ,true  , true, true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;

            Base_dgv.basegrid.RowHeadersVisible = false;
        }
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail


        private void dGridView_Sell_Cacu_Header_Reset()
        {
            Base_dgv.grid_col_Count = 10;

            string[] g_HeaderText = {""  , "결제방법"   , "결제액"  , "결제일자"   , "카드_은행명"
                                , "카드_은행번호"   , "카드소유자"    , "입금자"  , "비고" ,"주문번호"
                                };

            int[] g_Width = { 0, 90, 70, 90, 100
                                ,120 , 100 , 90 , 150 , 100
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;

            Base_dgv.basegrid.RowHeadersVisible = false;
        }
        //////Sales_Cacu___Sales_Cacu__Sales_Cacu__Sales_Cacu
        //////Sales_Cacu___Sales_Cacu__Sales_Cacu__Sales_Cacu



        private void dGridView_Sell_Rece_Header_Reset()
        {

            Base_dgv.grid_col_Count = 13;   // 태국 주, 태국 도시 열 추가.          

            string[] g_HeaderText = {""  , "배송구분"   , "배송일"  , "수령인"   , "우편_번호"
                                , "주소1"   , "주소2"    , "연락처_1"  , "연락처_2" , "비고"
                                ,"주문번호", "태국_주", "태국_도시"
                                };

            int[] g_Width;

            // 태국인 경우
            if (cls_User.gid_CountryCode == "TH")
            {
                g_Width = new int[] { 0, 90, 70, 90, 100
                                ,120, 100, 90, 150, 200
                                ,100, 100, 100
                            };
            }
            // 그 외 국가 인 경우
            else
            {
                g_Width = new int[] { 0, 90, 70, 90, 100
                                ,120, 100, 90, 150, 200
                                ,100, 0, 0
                            };
            }


            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter  //10

                                ,DataGridViewContentAlignment.MiddleCenter  //11
                                ,DataGridViewContentAlignment.MiddleCenter  //12
                                ,DataGridViewContentAlignment.MiddleCenter  //13

                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                     ,true ,true ,true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;

            Base_dgv.basegrid.RowHeadersVisible = false;
        }
        //////Sales_Rece___Sales_Rece__Sales_Rece__Sales_Rece
        //////Sales_Rece___Sales_Rece__Sales_Rece__Sales_Rece





        private void dGridView_Pay_Header_Reset()
        {

            Base_dgv.grid_col_Count = 11;

            string[] g_HeaderText = {"구분" ,  "마감일자" ,  "지급일자"   , "발생액"  , "소득세"
                                    , "주민세"  ,"실지급액"  , ""  , "" , ""
                                    , ""
                                    };

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            Base_dgv.grid_cell_format = gr_dic_cell_format;

            int[] g_Width = { 100, 90, 70, 80, 80
                                ,80 , 80 , 0 , 0 , 0
                                ,0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //10

                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        }


        private void dGridView_Talk_Header_Reset()
        {

            Base_dgv.grid_col_Count = 10;

            string[] g_HeaderText = {"상담_내역" ,  "기록자" ,  "기록일"   , "_Seq"  , ""
                                    , ""  ,""  , ""  , "" , ""
                                    };



            int[] g_Width = { 500, 100, 150, 0, 0
                                ,0 , 0 , 0 , 0 , 0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //10
                                };

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        }


        private void dGridView_Member_Header_Reset()
        {
            Base_dgv.grid_col_Count = 27;

            string[] g_HeaderText = {"회원_번호"  , "성명"   , "주민번호"  , "현직급"   , "라인"
                                , "센타명"   , "가입일"    , "집전화"   , "핸드폰"    , "교육일"
                                , "후원인"   , "후원인명"  , "추천인"   , "추천인명"   ,"우편_번호"
                                , "주소1"   , "주소2"   , "은행명"    , "계좌번호" , "예금주"
                                , "구분" , "활동_여부", "_중지_여부"  , "탈퇴일"  , "_라인중지일"
                                ,"기록자" , "기록일"
                                    };
            Base_dgv.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 0, 90 , 130, 80, 60
                             ,100, 90, 130, 130, 90
                             ,cls_app_static_var.save_uging_Pr_Flag , cls_app_static_var.save_uging_Pr_Flag, cls_app_static_var.nom_uging_Pr_Flag, cls_app_static_var.nom_uging_Pr_Flag, 80
                             ,200 , 90, 120 , 90 , 60
                             ,70 , 70 , 0 , 90 , 0
                             ,0 , 0
                            };
            Base_dgv.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true ,true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //15   
                          
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //20

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //25   

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                              };
            Base_dgv.grid_col_alignment = g_Alignment;
        }


        private void dGridView_Save_Up_Header_Reset()
        {
            Base_dgv.grid_col_Count = 15;

            string[] g_HeaderText = {"대수"  , "회원_번호"   , "직급"  , "성명"   , "가입일"
                                , "탈퇴일"   , "센타명"    , "후원인"   , "후원인명"    , "추천인"
                                , "추천인명"   , "집전화"  , "핸드폰"   , ""   ,"위치"
                                    };

            string[] g_Cols = {"대수"  , "회원_번호"   , "직급"  , "성명"   , "가입일"
                                , "탈퇴일"   , "센타명"    , "후원인"   , "후원인명"    , "추천인"
                                , "추천인명"   , "집전화"  , "핸드폰"   , "Col1"   ,"위치"
                                    };
            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_name = g_Cols;

            int[] g_Width = { 90, 90 , 130, 80, 60
                             ,100, 90, cls_app_static_var.save_uging_Pr_Flag, cls_app_static_var.save_uging_Pr_Flag, cls_app_static_var.nom_uging_Pr_Flag
                             ,cls_app_static_var.nom_uging_Pr_Flag , 90, 80, 0, 100
                            };
            Base_dgv.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //15   
                  
                              };
            Base_dgv.grid_col_alignment = g_Alignment;

            Base_dgv.basegrid.RowHeadersVisible = true;
        }


        private void dGridView_RePay_D2_info_Header_Reset()
        {

            Base_dgv.grid_col_Count = 17;

            string[] g_HeaderText = {"원마감일"  ,"확정마감일"  , "반품주문번호"   ,  "반품회원번호" , "반품성명"
                                     ,  "추천" ,"_소비전환"    , "_패키지"  , "후원"  ,"매칭"
                                     ,"공제예상액합산","반품CV"    , "차감한도","후원좌 차감" , "후원우 차감"
                                     ,"매칭회원상세", "매칭금액상세"
                                    };

            int[] g_Width = { 90,120, 90, 100, 80
                              , 80  ,0 , 0 ,80 , 80
                              , 80    , 80, 80    , 80  , 80
                              ,200                              ,200
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft

                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5      
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight

                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //10
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight

                                ,DataGridViewContentAlignment.MiddleLeft  //10
                                ,DataGridViewContentAlignment.MiddleLeft  //10
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();


            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;



            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;



            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,  true ,true      ,true        ,true        ,true
                                      ,true        ,true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        }



        private void dGridView_gold()
        {

            Base_dgv.grid_col_Count = 4;

            string[] g_HeaderText = {"lv","최초골드이상 회원번호","최초골드이상 회원 직위"  ,"최초골드이상 회원명"
                                    };
            ///
            int[] g_Width = { 120,120,120,120
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                             ,DataGridViewContentAlignment.MiddleCenter
                             ,DataGridViewContentAlignment.MiddleCenter
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();


            //gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;



            Boolean[] g_ReadOnly = { true , true, true, true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        }


        private void dGridView_RePay_D4_info_Header_Reset()
        {

            Base_dgv.grid_col_Count = 5;

            string[] g_HeaderText = {"원마감일"  ,"확정마감일"  , "구분" ,"금액" ,""
                                    };

            int[] g_Width = { 90,120, 90, 100, 0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight

                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();


            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;



            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        }








        private void Base_Grid_info_Set(Form fr, string intTemp, string SdMbid, string Ordernumber)
        {
            string T_Mbid = "";
            T_Mbid = SdMbid.Trim();
            string Mbid = ""; int Mbid2 = 0;
            cls_Search_DB csb = new cls_Search_DB();
            cls_form_Meth cm = new cls_form_Meth();
            csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);
            string Tsql = "";
            if (intTemp == "sell")
                Sell_dGridView_Info_Put(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "memc")
                Mem_change_dGridView_Info_Put(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "memupc")
                Mem_UP_change_dGridView_Info_Put(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "memadd")
                Mem_Add_dGridView_Info_Put(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "item" || intTemp == "item_mem")
                Set_SalesItemDetail(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "cacu")
                Set_Sales_Cacu(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "rece")
                Set_Sales_Rece(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "pay")
                Set_Pay(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "member")
                Set_Memberinfo(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "talk")
                Set_Memberinfo_Talk(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "saveup")
                Set_Memberinfo_Up(Mbid, Mbid2, Ordernumber, "SAVE", ref Tsql);

            if (intTemp == "nominup")
                Set_Memberinfo_Up(Mbid, Mbid2, Ordernumber, "NOM", ref Tsql);

            if (intTemp == "savedown")
                Set_Memberinfo_Down(Mbid, Mbid2, Ordernumber, "SAVE", ref Tsql);

            if (intTemp == "nomindown")
                Set_Memberinfo_Down(Mbid, Mbid2, Ordernumber, "NOM", ref Tsql);

            //if (intTemp == "RePay_D2")
            //   Set_Memberinfo_RePay_D2_info(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "RePay_D4")
                Set_Memberinfo_RePay_D4_info(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "savedefault")
                Set_Memberinfo_Up(Mbid, Mbid2, Ordernumber, "SAVEDEFAULT", ref Tsql);
            //if (intTemp == "gold")
            //{

            //    cls_Connect_DB Temp_Connect1 = new cls_Connect_DB();
            //    StringBuilder sb = new StringBuilder();


            //    sb.AppendLine("Select  ");
            //    sb.AppendLine(" T_AA.Lvl ");
            //    sb.AppendLine(" , T_AA.mbid2");
            //    sb.AppendLine("   ,Isnull(CC_A.G_Name,'') ");
            //    sb.AppendLine("    ,A.M_Name ");
            //    sb.AppendLine("	 , Case When A.Regtime <> '' Then  LEFT(A.Regtime,4) +'-' + LEFT(RIGHT(A.Regtime,4),2) + '-' + RIGHT(A.Regtime,2) ELSE '' End  ");
            //    sb.AppendLine("	 , Case When A.LeaveDate <> '' Then  LEFT(A.LeaveDate,4) +'-' + LEFT(RIGHT(A.LeaveDate,4),2) + '-' + RIGHT(A.LeaveDate,2) ELSE '' End ");
            //    sb.AppendLine("	 , Isnull( tbl_Business.name,'')  ,A.Saveid2  , Isnull(b.M_Name,'')  ,A.Nominid2  , Isnull(C.M_Name,'')  , A.hometel  , A.hptel  , '' ");
            //    sb.AppendLine("	  , A.LineCnt  From ufn_matrix_mem(''," + Ordernumber + ", '*') T_AA  LEFT JOIN tbl_Memberinfo AS A  (nolock) ON A.Mbid = T_AA.mbid And A.Mbid2 = T_AA.Mbid2    ");
            //    sb.AppendLine("	  LEFT JOIN tbl_Memberinfo AS B  (nolock) ON a.Saveid = b.mbid And a.Saveid2 = b.mbid2    LEFT JOIN tbl_Memberinfo AS C  (nolock) ON a.Nominid=c.mbid And a.Nominid2 = c.mbid2   ");
            //    sb.AppendLine("	   LEFT Join tbl_Business  (nolock) On a.businesscode=tbl_Business.ncode  And a.Na_code = tbl_Business.Na_code");
            //    sb.AppendLine("	    Left Join tbl_Class C1  (nolock) On A.CurGrade=C1.Grade_Cnt  ");
            //    sb.AppendLine("		Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On CC_A.Mbid = A.Mbid And  CC_A.Mbid2 = A.Mbid2  ");
            //    sb.AppendLine("		where  T_AA.mbid2 <> '" + Ordernumber + "' and A.LeaveDate = '' ");
            //    sb.AppendLine("	   ORder by Lvl ");

            //    DataSet ds1 = new DataSet();
            //    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //    if (Temp_Connect1.Open_Data_Set(sb.ToString(), base_db_name, ds1) == false) return;
            //    int ReCnt1 = Temp_Connect1.DataSet_ReCount;

            //    if (ReCnt1 == 0) return;
            //    //++++++++++++++++++++++++++++++++


            //    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //    Dictionary<int, object[]> gr_dic_text1 = new Dictionary<int, object[]>();

            //    for (int fi_cnt = 0; fi_cnt <= ReCnt1 - 1; fi_cnt++)
            //    {
            //        string test = ds1.Tables[base_db_name].Rows[fi_cnt][2].ToString();
            //        if (test == "골드" || test == "루비" || test == "사파이어" || test == "에메랄드" || test == "다이아몬드" || test == "블루다이아몬드" || test == "레드다디아몬드" || test == "크라운" || test == "엠페리얼")
            //        {
            //            Set_gr_dic2(ref ds1, ref gr_dic_text1, fi_cnt);  //데이타를 배열에 넣는다.
            //            break;
            //        }

            //    }

            //    Base_dgv.grid_name_obj = gr_dic_text1;  //배열을 클래스로 보낸다.
            //    Base_dgv.db_grid_Obj_Data_Put();
            //    return;
            //}
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, fr.Name, fr.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            int T_cnt = 0;
            double S_cnt4 = 0; double S_cnt5 = 0; double S_cnt6 = 0; double S_cnt7 = 0; double S_cnt8 = 0; double S_cnt9 = 0; double S_cnt10 = 0;
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                if (intTemp == "sell" || intTemp == "item" || intTemp == "pay"
                    || intTemp == "RePay_D2"
                    || intTemp == "RePay_D4"
                    )
                    Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt, 1);  //데이타를 배열에 넣는다.
                else if (intTemp == "cacu")
                    Set_gr_dic_Info_Cacu(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                else
                    Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                T_cnt = fi_cnt;
                if (intTemp == "sell")
                {
                    S_cnt4 = S_cnt4 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][5].ToString());
                    S_cnt5 = S_cnt5 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][6].ToString());
                    S_cnt6 = S_cnt6 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][7].ToString());
                    S_cnt7 = S_cnt7 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][8].ToString());
                    S_cnt8 = S_cnt8 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][9].ToString());
                    S_cnt9 = S_cnt9 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][10].ToString());
                }

                if (intTemp == "cacu")
                {
                    S_cnt4 = S_cnt4 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_Price1"].ToString());
                }

                if (intTemp == "item")
                {
                    S_cnt4 = S_cnt4 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemPrice"].ToString());
                    S_cnt5 = S_cnt5 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemPV"].ToString());
                    S_cnt6 = S_cnt6 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemCV"].ToString());
                    S_cnt7 = S_cnt7 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemCount"].ToString());
                    S_cnt8 = S_cnt8 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemTotalPrice"].ToString());
                    S_cnt9 = S_cnt9 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemTotalPV"].ToString());
                    S_cnt10 = S_cnt10 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemTotalCV"].ToString());
                }

                if (intTemp == "pay")
                {
                    S_cnt4 = S_cnt4 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumAllAllowance"].ToString());
                    S_cnt5 = S_cnt5 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InComeTax"].ToString());
                    S_cnt6 = S_cnt6 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ResidentTax"].ToString());
                    S_cnt7 = S_cnt7 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TruePayment"].ToString());
                }

            }


            if (intTemp == "sell")
            {
                object[] row0 = { ""
                                    ,"<< " + cm._chang_base_caption_search("합계") + " >>"
                                    ,""
                                    ,""
                                    ,""

                                    ,S_cnt4
                                    ,S_cnt5
                                    ,S_cnt6
                                    ,S_cnt7
                                    ,S_cnt8

                                    ,S_cnt9
                                    ,""
                                     };

                gr_dic_text[T_cnt + 2] = row0;
            }

            if (intTemp == "cacu")
            {
                object[] row0 = { ""
                                ,"<< " + cm._chang_base_caption_search("합계") + " >>"
                                ,S_cnt4
                                ,""
                                ,""

                                ,""
                                ,""
                                ,""
                                ,""
                                ,""
                                 };

                gr_dic_text[T_cnt + 2] = row0;
            }

            if (intTemp == "item")
            {
                object[] row0 = { ""
                                ,"<< " + cm._chang_base_caption_search("합계") + " >>"
                                ,""
                                ,S_cnt4
                                ,S_cnt5
                                ,S_cnt6
                                ,S_cnt7
                                ,S_cnt8
                                ,S_cnt9
                                ,S_cnt10
                                 };

                gr_dic_text[T_cnt + 2] = row0;
            }


            if (intTemp == "pay")
            {
                object[] row0 = { ""
                                ,"<< " + cm._chang_base_caption_search("합계") + " >>"
                                ,""
                                ,S_cnt4
                                ,S_cnt5

                                ,S_cnt6
                                ,S_cnt7
                                ,""
                                ,""
                                ,""
                                 };

                gr_dic_text[T_cnt + 2] = row0;
            }




            Base_dgv.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            Base_dgv.db_grid_Obj_Data_Put();

        } // end Base_Grid_info_Set

        private void Set_gr_dic2(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {

            object[] row0 = {
                 ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                 };


            gr_dic_text[0 + 1] = row0;
        }
        //private void get_gold(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        //{

        //    cls_form_Meth cm = new cls_form_Meth();

        //    //Tsql = "Select  top 1  T_AA.Lvl, T_AA.mbid2  ";
        //    //Tsql = Tsql + ",Isnull(CC_A.G_Name,'') as  gold";
        //    //Tsql = Tsql + ",A.M_Name  ";
        //    //Tsql = Tsql + " From ufn_GetSubTree_MemGroup('', " + Ordernumber + ") T_AA ";
        //    //Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS A  (nolock) ON A.Mbid = T_AA.mbid And A.Mbid2 = T_AA.Mbid2  ";
        //    //Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS B  (nolock) ON a.Saveid = b.mbid And a.Saveid2 = b.mbid2  ";
        //    //Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS C  (nolock) ON a.Nominid=c.mbid And a.Nominid2 = c.mbid2    ";
        //    //Tsql = Tsql + " LEFT Join tbl_Business  (nolock) On a.businesscode=tbl_Business.ncode And a.Na_code = tbl_Business.Na_code ";
        //    //Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On A.CurGrade=C1.Grade_Cnt ";
        //    //Tsql = Tsql + " Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On CC_A.Mbid = A.Mbid And  CC_A.Mbid2 = A.Mbid2  ";
        //    //Tsql = Tsql + " Where T_AA.Lvl > 0  ";
        //    //Tsql = Tsql + " and C1.grade_cnt>=70";
        //    //Tsql = Tsql + " and  A.LeaveDate = '' ";
        //    //Tsql = Tsql + " ORder by Lvl ASC, ";
        //    //Tsql = Tsql + " LEFT(SaveCur,3) ASC   , SaveCur ASC ";


        //    Tsql = "Select  ";
        //    Tsql = Tsql + " T_AA.Lvl ";
        //    Tsql = Tsql + " , T_AA.mbid2";
        //    Tsql = Tsql + "   ,Isnull(CC_A.G_Name,'') ";
        //    Tsql = Tsql + "    ,A.M_Name ";
        //    Tsql = Tsql + "	 , Case When A.Regtime <> '' Then  LEFT(A.Regtime,4) +'-' + LEFT(RIGHT(A.Regtime,4),2) + '-' + RIGHT(A.Regtime,2) ELSE '' End  ";
        //    Tsql = Tsql + "	 , Case When A.LeaveDate <> '' Then  LEFT(A.LeaveDate,4) +'-' + LEFT(RIGHT(A.LeaveDate,4),2) + '-' + RIGHT(A.LeaveDate,2) ELSE '' End ";
        //    Tsql = Tsql + "	 , Isnull( tbl_Business.name,'')  ,A.Saveid2  , Isnull(b.M_Name,'')  ,A.Nominid2  , Isnull(C.M_Name,'')  , A.hometel  , A.hptel  , '' ";
        //    Tsql = Tsql + "	  , A.LineCnt  From ufn_matrix_mem(''," + Ordernumber + ", '*') T_AA  LEFT JOIN tbl_Memberinfo AS A  (nolock) ON A.Mbid = T_AA.mbid And A.Mbid2 = T_AA.Mbid2    ";
        //    Tsql = Tsql + "	  LEFT JOIN tbl_Memberinfo AS B  (nolock) ON a.Saveid = b.mbid And a.Saveid2 = b.mbid2    LEFT JOIN tbl_Memberinfo AS C  (nolock) ON a.Nominid=c.mbid And a.Nominid2 = c.mbid2   ";
        //    Tsql = Tsql + "	   LEFT Join tbl_Business  (nolock) On a.businesscode=tbl_Business.ncode  And a.Na_code = tbl_Business.Na_code";
        //    Tsql = Tsql + "	    Left Join tbl_Class C1  (nolock) On A.CurGrade=C1.Grade_Cnt  ";
        //    Tsql = Tsql + "		Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On CC_A.Mbid = A.Mbid And  CC_A.Mbid2 = A.Mbid2  ";
        //    Tsql = Tsql + "		where  T_AA.mbid2 <> '" + Ordernumber + "'";
        //    Tsql = Tsql + "	   ORder by Lvl ";

        //}

        private void Set_Memberinfo_RePay_D2_info(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {

            //cls_form_Meth cm = new cls_form_Meth();

            ////string[] g_HeaderText = {"마감일"  ,"확정마감일"  , "반품주문번호"   ,  "반품회원번호" , "반품성명"    
            ////                         ,  "멤버" ,"소비전환"    , "패키지"  , "팀"  ,"매칭"
            ////                         ,"공제예상액합산","팀좌 차감" , "팀우 차감" 
            ////,"매칭회원상세", "매칭금액상세"
            ////                        };



            //Tsql = "Select  Re_T.clo_ToEndDate ";
            //Tsql = Tsql + " , Re_T.Cur_ToEndDate  ";
            //Tsql = Tsql + " , Re_T.Ordernumber  ";

            //Tsql = Tsql + " , tbl_SalesDetail.Mbid2  ";
            //Tsql = Tsql + " , tbl_SalesDetail.M_Name ";

            //Tsql = Tsql + " , Re_T.Ded_A_3  ";
            //Tsql = Tsql + " , Re_T.Ded_A_6 ";
            //Tsql = Tsql + " , Re_T.Ded_A_15 ";

            //Tsql = Tsql + " , Re_T.Ded_A_1 ";
            //Tsql = Tsql + " , Re_T.Ded_A_2 ";

            //Tsql = Tsql + " , Ded_A_3 + Ded_A_6 +  Ded_A_15 + Ded_A_1 +Ded_A_2 ";

            //Tsql = Tsql + " , Re_T.TotalPV";
            //Tsql = Tsql + ", Case When Re_T.Ded_PV_1 > 0 then Re_T.Ded_PV_1 ELSE  Re_T.Ded_PV_2 END  ";

            //Tsql = Tsql + " , Re_T.Re_Cur_PV_1 ";
            //Tsql = Tsql + " , Re_T.Re_Cur_PV_2 ";

            //Tsql = Tsql + " , Re_T.Req_Mbid_T ";
            //Tsql = Tsql + " , Re_T.Req_Pay_T ";


            //Tsql = Tsql + " FROM tbl_ClosePay_04_Ded_P_Detail_Mod (nolock)  ";
            ////Tsql = Tsql + " ( ";
            ////Tsql = Tsql + " Select clo_ToEndDate , Cur_ToEndDate ";
            ////Tsql = Tsql + "  , Ded_A_3 , Ded_A_6 , Ded_A_15 , Ded_A_1 , Ded_A_2 ";
            ////Tsql = Tsql + "  , Ded_A_3 + Ded_A_6 +  Ded_A_15 + Ded_A_1 +Ded_A_2 Sum_W_Ded ";
            ////Tsql = Tsql + " , TotalPV , Ded_PV_1, Ded_PV_2, Re_Cur_PV_1 , Re_Cur_PV_2 , Req_Mbid_T,Req_Pay_T  ";
            ////Tsql = Tsql + "  From  tbl_ClosePay_02_Ded_P_Detail_Mod (nolock)    ";
            ////Tsql = Tsql + " Union All ";
            ////Tsql = Tsql + " Select Close_Date clo_ToEndDate , ToEndDate Cur_ToEndDate ";
            ////Tsql = Tsql + "  , 0 Ded_A_3 , 0 Ded_A_6 , 0  Ded_A_15 , 0  Ded_A_1 , 0  Ded_A_2 ";
            ////Tsql = Tsql + "  , 0  Sum_W_Ded ";
            ////Tsql = Tsql + " , 0 TotalPV , 0 Ded_PV_1, 0 Ded_PV_2, 0  Re_Cur_PV_1 , 0  Re_Cur_PV_2 , '' Req_Mbid_T , '' Req_Pay_T  ";
            ////Tsql = Tsql + "  From  tbl_Close_Ret_Pay_Detail (nolock)    ";


            ////Tsql = Tsql + " ) "; 
            //Tsql = Tsql + " Re_T  ";

            //Tsql = Tsql + " LEFT JOIN tbl_SalesDetail  (nolock) ON Re_T.Ordernumber = tbl_SalesDetail.Ordernumber ";
            //Tsql = Tsql + " Where Re_T.Mbid2 ='" + Mbid2 + "'";
            //Tsql = Tsql + " And  Ded_A_3 + Ded_A_6 + Ded_A_7+ Ded_A_15 + Ded_A_1 +Ded_A_2  + Re_Cur_PV_1 + Re_Cur_PV_2 > 0 ";

            //Tsql = Tsql + " order by Re_T.Cur_ToEndDate  , tbl_SalesDetail.Mbid2  ";
        }



        private void Set_Memberinfo_RePay_D4_info(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();

            //string[] g_HeaderText = {"원마감일"  ,"확정마감일"  , "구분" ,"금액" ,""
            //                        };


            Tsql = "Select  ToEndDate  ";
            Tsql = Tsql + " ,  close_Date ";
            Tsql = Tsql + " , CAse When Pay_FLAG = 1  then '첫팩주문보너스' ";
            Tsql = Tsql + "        When Pay_FLAG = 2  then '멘토보너스' ";
            Tsql = Tsql + "        When Pay_FLAG = 3  then '비즈니스개발보너스' ";
            Tsql = Tsql + "        When Pay_FLAG = 4  then '유니레벨보너스' ";
            Tsql = Tsql + "        When Pay_FLAG = 5  then '사이드볼륨인피니티보너스' ";
            Tsql = Tsql + "        When Pay_FLAG = 6  then '리더체크매치보너스' ";
            Tsql = Tsql + "  END ";

            Tsql = Tsql + " , Allowance  ";
            Tsql = Tsql + " , '' ";
            Tsql = Tsql + " FROM mannatech_Return_Close.dbo.tbl_Close_Ret_Pay_Detail (nolock)  ";
            Tsql = Tsql + " Where Mbid2 ='" + Mbid2 + "'";
            Tsql = Tsql + " order by ToEndDate  , Pay_FLAG   ";
        }


        private void Sell_dGridView_Info_Put(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();

            Tsql = "Select  ";

            Tsql = Tsql + " Case When tbl_SalesDetail.Ga_Order = 0 Then '" + cm._chang_base_caption_search("승인") + "'";
            Tsql = Tsql + "  When tbl_SalesDetail.Ga_Order > 0 Then '" + cm._chang_base_caption_search("미승인") + "'";
            Tsql = Tsql + " END SellTFName ";

            Tsql = Tsql + " ,SellDate ";
            Tsql = Tsql + " ,tbl_SalesDetail.OrderNumber ";
            //Tsql = Tsql + " ,SellTypeName ";
            // 한국인 경우
            if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
            {
                Tsql = Tsql + " ,SellTypeName ";
            }
            // 태국인 경우
            else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
            {
                Tsql = Tsql + " ,SellTypeName_En ";
            }

            //Tsql = Tsql + " ,Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " Ch_Detail ";
            Tsql = Tsql + " ,Case When ReturnTF = 1 Then '" + cm._chang_base_caption_search("정상") + "'";
            Tsql = Tsql + "  When ReturnTF = 2 Then '" + cm._chang_base_caption_search("반품") + "'";
            Tsql = Tsql + "  When ReturnTF = 4 Then '" + cm._chang_base_caption_search("교환") + "'";
            Tsql = Tsql + "  When ReturnTF = 3 Then '" + cm._chang_base_caption_search("부분반품") + "'";
            Tsql = Tsql + "  When ReturnTF = 5 Then '" + cm._chang_base_caption_search("취소") + "'";
            Tsql = Tsql + " END ";

            Tsql = Tsql + " ,TotalPrice ";
            Tsql = Tsql + " ,TotalInputPrice ";
            Tsql = Tsql + " ,TotalPV ";
            Tsql = Tsql + ", TotalCV ";

            Tsql = Tsql + " ,InputCash ";
            Tsql = Tsql + " ,InputCard ";
            Tsql = Tsql + " ,InputPassbook ";
            Tsql = Tsql + " ,Etc1 ";

            Tsql = Tsql + " From tbl_SalesDetail (nolock) ";
            //Tsql = Tsql + " Left Join tbl_SalesDetail_TF (nolock) On tbl_SalesDetail_TF.OrderNumber =tbl_SalesDetail.OrderNumber ";
            Tsql = Tsql + " Left Join tbl_SellType (nolock) On tbl_SellType.SellCode =tbl_SalesDetail.SellCode ";
            Tsql = Tsql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_SalesDetail' And  Ch_T.M_Detail = Convert(Varchar,tbl_SalesDetail.ReturnTF ) ";
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " Order By OrderNumber ASC ";
        } // end Sell_dGridView_Info_Put



        private void Mem_change_dGridView_Info_Put(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {

            Tsql = "Select  ";
            Tsql = Tsql + " A.ModRecordtime ";
            Tsql = Tsql + " ,Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " Ch_Detail ";
            Tsql = Tsql + " ,BeforeDetail ";
            Tsql = Tsql + " ,AfterDetail ";
            Tsql = Tsql + " ,A.ModRecordid ";

            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";

            Tsql = Tsql + " FROM tbl_Memberinfo_Mod AS A (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo_Mod_Detail Ch_T  (nolock) ON Ch_T.M_Detail = A.ChangeDetail";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS B  (nolock) ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Business         (nolock) ON B.BusinessCode = tbl_Business.ncode And b.Na_code = tbl_Business.Na_code ";

            if (Mbid.Length == 0)
                Tsql = Tsql + " Where B.Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where b.Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   B.Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " And Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " IS NOT NULL ";
            Tsql = Tsql + " Order By Modrecordtime DESC ";
        } // end Mem_change_dGridView_Info_Put

        private void Mem_UP_change_dGridView_Info_Put(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {
            cls_form_Meth cm = new cls_form_Meth();
            string save_C = cm._chang_base_caption_search("후원인_변경");
            string nom_C = cm._chang_base_caption_search("추천인_변경");

            Tsql = "Select  ";
            Tsql = Tsql + " tbl_Memberinfo_Save_Nomin_Change.recordtime ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.Old_mbid + '-' + Convert(Varchar,tbl_Memberinfo_Save_Nomin_Change.Old_mbid2) ";
            else
                Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.Old_mbid2 ";
            Tsql = Tsql + " ,A.M_name AS oldname ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.New_mbid + '-' + Convert(Varchar,tbl_Memberinfo_Save_Nomin_Change.New_mbid2) ";
            else
                Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.New_mbid2 ";
            Tsql = Tsql + " ,B.M_name AS Newname";

            //Tsql = Tsql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + "  Ch_Detail ";
            Tsql = Tsql + " , Case When Save_Nomin_SW = 'Sav' Then '" + save_C + "' ELSE '" + nom_C + "' END";
            Tsql = Tsql + " ,tbl_Memberinfo_Save_Nomin_Change.Recordid ";

            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";


            Tsql = Tsql + " FROM      tbl_Memberinfo_Save_Nomin_Change  (nolock) ";

            Tsql = Tsql + " Left JOIN tbl_Memberinfo A (nolock)  ON";
            Tsql = Tsql + " tbl_Memberinfo_Save_Nomin_Change.Old_mbid = A.mbid ";
            Tsql = Tsql + " And tbl_Memberinfo_Save_Nomin_Change.Old_mbid2 = A.mbid2 ";

            Tsql = Tsql + " Left Join tbl_Memberinfo B (nolock) ON ";
            Tsql = Tsql + " tbl_Memberinfo_Save_Nomin_Change.New_mbid = B.Mbid";
            Tsql = Tsql + " And tbl_Memberinfo_Save_Nomin_Change.New_mbid2 = B.Mbid2";

            //Tsql = Tsql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_Memberinfo_Save_Nomin_Change' And  Ch_T.M_Detail = tbl_Memberinfo_Save_Nomin_Change.Save_Nomin_SW ";

            if (Mbid.Length == 0)
                Tsql = Tsql + " Where tbl_Memberinfo_Save_Nomin_Change.Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where tbl_Memberinfo_Save_Nomin_Change.Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   tbl_Memberinfo_Save_Nomin_Change.Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " Order By tbl_Memberinfo_Save_Nomin_Change.recordtime DESC  ";
        } // end Mem_UP_change_dGridView_Info_Put

        private void Mem_Add_dGridView_Info_Put(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {
            cls_form_Meth cm = new cls_form_Meth();
            Tsql = "Select  ";

            Tsql = Tsql + " Case When Sort_Add = 'C' Then '" + cm._chang_base_caption_search("직장") + "'";
            Tsql = Tsql + "  When Sort_Add = 'R' Then '" + cm._chang_base_caption_search("기본배송지") + "'";
            Tsql = Tsql + " END ";

            Tsql = Tsql + " ,ETC_Addcode1   ";
            Tsql = Tsql + " ,ETC_Address1 ";
            Tsql = Tsql + " ,ETC_Address2 ";

            Tsql = Tsql + " ,ETC_Tel_1 ";
            Tsql = Tsql + " ,ETC_Tel_2 ";
            Tsql = Tsql + " ,ETC_Name ";


            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";

            Tsql = Tsql + " From tbl_Memberinfo_Address (nolock) ";

            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " Order By Sort_Add ASC ";


            //당일 등록된 회원을 불러온다.


        } //end  Mem_Add_dGridView_Info_Put


        private void Set_SalesItemDetail(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {
            cls_form_Meth cm = new cls_form_Meth();


            Tsql = "Select tbl_SalesitemDetail.SalesItemIndex ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemCode ";
            Tsql = Tsql + " , tbl_Goods.Name Item_Name ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemPrice  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemPV  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemCV  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemCount  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemTotalPrice  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemTotalPV  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemTotalCV  ";

            Tsql = Tsql + " ,Case When SellState = 'N_1' Then '" + cm._chang_base_caption_search("정상") + "'";
            Tsql = Tsql + "  When SellState = 'N_3' Then '" + cm._chang_base_caption_search("교환_정상") + "'";
            Tsql = Tsql + "  When SellState = 'R_1' Then '" + cm._chang_base_caption_search("반품") + "'";
            Tsql = Tsql + "  When SellState = 'R_3' Then '" + cm._chang_base_caption_search("교환_반품") + "'";
            Tsql = Tsql + "  When SellState = 'C_1' Then '" + cm._chang_base_caption_search("취소") + "'";
            Tsql = Tsql + " END  SellStateName ";
            Tsql = Tsql + " , tbl_SalesitemDetail.Etc  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.OrderNumber   ";

            Tsql = Tsql + " From tbl_SalesitemDetail (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Goods (nolock) ON tbl_Goods.Ncode = tbl_SalesitemDetail.ItemCode ";


            if (Ordernumber != "")
            {
                Tsql = Tsql + " Where tbl_SalesitemDetail.OrderNumber = '" + Ordernumber.ToString() + "'";
                Tsql = Tsql + " Order By SalesItemIndex ASC ";
            }
            else
            {
                Tsql = Tsql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_SalesitemDetail.OrderNumber ";
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By tbl_SalesDetail.OrderNumber DESC,  SalesItemIndex ASC ";
            }
        }



        private void Set_Sales_Rece(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();

            Tsql = "Select ";
            Tsql = Tsql + " tbl_Sales_Rece.SalesItemIndex  ";
            Tsql = Tsql + " ,Case When Receive_Method = 1 Then '" + cm._chang_base_caption_search("직접수령") + "'";
            Tsql = Tsql + "  When Receive_Method = 2 Then '" + cm._chang_base_caption_search("배송") + "'";
            Tsql = Tsql + "  When Receive_Method = 3 Then '" + cm._chang_base_caption_search("센타수령") + "'";
            Tsql = Tsql + "  When Receive_Method = 4 Then '" + cm._chang_base_caption_search("본사직접수령") + "'";
            Tsql = Tsql + " END  Receive_Method_Name ";
            Tsql = Tsql + " ,Get_Date1 ";
            Tsql = Tsql + " ,Get_Name1 ";
            Tsql = Tsql + " ,Get_ZipCode ";
            Tsql = Tsql + " ,Get_Address1 ";
            Tsql = Tsql + " ,Get_Address2 ";
            Tsql = Tsql + " ,Get_Tel1 ";
            Tsql = Tsql + " ,Get_Tel2 ";
            Tsql = Tsql + " ,Get_Etc1 ";
            Tsql = Tsql + " , tbl_Sales_Rece.OrderNumber   ";
            Tsql = Tsql + " , Get_state, Get_city ";

            Tsql = Tsql + " From tbl_Sales_Rece (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Base_Rec (nolock) on tbl_Base_Rec.ncode = tbl_Sales_Rece.Base_Rec ";


            if (Ordernumber != "")
            {
                Tsql = Tsql + " Where tbl_Sales_Rece.OrderNumber = '" + Ordernumber.ToString() + "'";
                Tsql = Tsql + " Order By SalesItemIndex ASC ";
            }
            else
            {
                Tsql = Tsql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_Sales_Rece.OrderNumber ";
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By tbl_SalesDetail.OrderNumber DESC,  SalesItemIndex ASC ";
            }
        }


        private void Set_Sales_Cacu(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {
            cls_form_Meth cm = new cls_form_Meth();

            Tsql = "Select tbl_Sales_Cacu.C_index ";
            //Tsql = Tsql + " ,Case When C_TF = 1 Then '" + cm._chang_base_caption_search("현금") + "'";
            //Tsql = Tsql + "  When C_TF = 2 Then '" + cm._chang_base_caption_search("무통장") + "'";
            //Tsql = Tsql + "  When C_TF = 3 Then '" + cm._chang_base_caption_search("카드") + "'";
            //Tsql = Tsql + "  When C_TF = 4 Then '" + cm._chang_base_caption_search("마일리지") + "'";
            //Tsql = Tsql + "  When C_TF = 5 Then '" + cm._chang_base_caption_search("가상계좌") + "'";            
            //Tsql = Tsql + " END  C_TF_Name ";
            Tsql = Tsql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " C_TF_Name ";

            Tsql = Tsql + " ,tbl_Sales_Cacu.C_Price1  ";
            Tsql = Tsql + " ,tbl_Sales_Cacu.C_AppDate1  ";
            Tsql = Tsql + " ,Case When Isnull(tbl_Bank.bankname , '') <> '' then Isnull(tbl_Bank.bankname , '') ELSE tbl_Sales_Cacu.C_CodeName END ";
            Tsql = Tsql + " ,tbl_Sales_Cacu.C_Number1  ";
            Tsql = Tsql + " ,tbl_Sales_Cacu.C_Name1  ";
            Tsql = Tsql + " ,tbl_Sales_Cacu.C_Name2  ";
            Tsql = Tsql + " ,tbl_Sales_Cacu.C_Etc  ";
            Tsql = Tsql + " , tbl_Sales_Cacu.OrderNumber   ";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber ";
            Tsql = Tsql + " LEFT JOIN tbl_BankForCompany (nolock) ON tbl_Sales_Cacu.C_Code = tbl_BankForCompany.BankCode And  tbl_Sales_Cacu.C_Number1 = tbl_BankForCompany.BankAccountNumber And tbl_SalesDetail.Na_Code = tbl_BankForCompany.Na_Code  ";
            Tsql = Tsql + " LEFT JOIN tbl_Base_Change_Detail (NOLOCK) Ch_T ON Ch_T.M_Detail_S = 'tbl_Sales_Cacu' AND Ch_T.M_Detail = tbl_Sales_Cacu.C_TF ";
            Tsql = Tsql + " LEFT JOIN tbl_Bank (nolock) ON Right(tbl_Sales_Cacu.C_Code,2)  = Right(tbl_Bank.Ncode,2)  And tbl_Sales_Cacu.C_TF = 5   ";
            cls_NationService.SQL_BankNationCode(ref Tsql);

            if (Ordernumber != "")
            {
                Tsql = Tsql + " Where tbl_Sales_Cacu.OrderNumber = '" + Ordernumber.ToString() + "'";
                Tsql = Tsql + " Order By C_index ASC ";
            }
            else
            {

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By tbl_SalesDetail.OrderNumber DESC,  C_index ASC ";
            }

        }


        private void Set_Pay(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();

            Tsql = " Select ST1 ";
            Tsql = Tsql + ", LEFT(ToEndDate,4) +'-' + LEFT(RIGHT(ToEndDate,4),2) + '-' + RIGHT(ToEndDate,2) ";
            Tsql = Tsql + ",LEFT(PayDate,4) +'-' + LEFT(RIGHT(PayDate,4),2) + '-' + RIGHT(PayDate,2) ";

            Tsql = Tsql + " ,SumAllAllowance ";
            Tsql = Tsql + " ,InComeTax ";
            Tsql = Tsql + " ,ResidentTax ";
            Tsql = Tsql + " ,TruePayment ";

            Tsql = Tsql + " ,'','','' ,'' ";
            Tsql = Tsql + " From ";

            Tsql = Tsql + "  ( ";
            ////Tsql = Tsql + " Select '주간_마감' ST1, PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
            ////Tsql = Tsql + " From tbl_ClosePay_01_Mod (nolock)  " ;            
            ////if (Mbid.Length == 0)
            ////    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            ////else
            ////{
            ////    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
            ////    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            ////}
            ////Tsql = Tsql + " And  SumAllAllowance >0 "; 

            ////Tsql = Tsql + " UNION ALL" ;

            Tsql = Tsql + " Select '주간_마감' ST1, PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
            Tsql = Tsql + " From tbl_ClosePay_04_Mod (nolock)  ";
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " And  SumAllAllowance >0 ";

            Tsql = Tsql + " UNION ALL";

            Tsql = Tsql + " Select '월_마감' ST1, PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
            Tsql = Tsql + " From tbl_ClosePay_04_Mod (nolock)  ";
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " And  SumAllAllowance >0 ";

            //Tsql = Tsql + " UNION ALL";

            //Tsql = Tsql + " Select '센타마감' ST1, PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
            //Tsql = Tsql + " From tbl_ClosePay_100_Mod (nolock)  ";
            //if (Mbid.Length == 0)
            //    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            //else
            //{
            //    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
            //    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            //}
            //Tsql = Tsql + " And  SumAllAllowance >0 "; 


            Tsql = Tsql + " )AS C  ";
            Tsql = Tsql + " Order By PayDate DESC ";

        }



        private void Set_Memberinfo(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();

            Tsql = "Select  ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) ";
            else
                Tsql = Tsql + " tbl_Memberinfo.mbid2 ";

            Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

            Tsql = Tsql + ", tbl_Memberinfo.Cpno ";

            Tsql = Tsql + " , ISNULL(C1.Grade_Name,'') ";
            Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";

            Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";
            Tsql = Tsql + " , LEFT(tbl_Memberinfo.RegTime,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.RegTime,4),2) + '-' + RIGHT(tbl_Memberinfo.RegTime,2)   ";
            Tsql = Tsql + " , tbl_Memberinfo.hometel ";
            Tsql = Tsql + " , tbl_Memberinfo.hptel ";
            Tsql = Tsql + " , Case When tbl_Memberinfo.Ed_Date <> '' Then  LEFT(tbl_Memberinfo.Ed_Date,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.Ed_Date,4),2) + '-' + RIGHT(tbl_Memberinfo.Ed_Date,2) ELSE '' End Ed_Date_2 ";



            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,tbl_Memberinfo.Saveid + '-' + Convert(Varchar,tbl_Memberinfo.Saveid2) ";
            else
                Tsql = Tsql + " ,tbl_Memberinfo.Saveid2 ";

            Tsql = Tsql + " , Isnull(Sav.M_Name,'') ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,tbl_Memberinfo.Nominid + '-' + Convert(Varchar,tbl_Memberinfo.Nominid2) ";
            else
                Tsql = Tsql + " ,tbl_Memberinfo.Nominid2 ";

            Tsql = Tsql + " , Isnull(Nom.M_Name,'') ";
            Tsql = Tsql + " , Case When tbl_Memberinfo.addcode1 <> '' Then  LEFT(tbl_Memberinfo.addcode1,3) +'-' + RIGHT(tbl_Memberinfo.addcode1,3) ELSE '' End ";

            Tsql = Tsql + " , tbl_Memberinfo.address1 ";
            Tsql = Tsql + " , tbl_Memberinfo.address2 ";
            Tsql = Tsql + " , tbl_Bank.BankName ";
            Tsql = Tsql + " , tbl_Memberinfo.bankaccnt ";
            Tsql = Tsql + " , tbl_Memberinfo.bankowner ";
            Tsql = Tsql + " , Case  When tbl_Memberinfo.Sell_Mem_TF = 0 then '" + cm._chang_base_caption_search("판매원") + "' ELSE  '" + cm._chang_base_caption_search("소비자") + "' End AS Sell_MEM_TF2";


            //Tsql = Tsql + " , Case tbl_Memberinfo.LeaveCheck When 1 then '" + cm._chang_base_caption_search("활동") + "' When 0 then '" + cm._chang_base_caption_search("탈퇴") + "' End AS LeaveCheck_2 ";

            Tsql = Tsql + " , Case  ";
            Tsql = Tsql + "  When tbl_Memberinfo.LeaveCheck = 1 Then '" + cm._chang_base_caption_search("활동") + "'";
            Tsql = Tsql + "  When tbl_Memberinfo.LeaveCheck = 0 Then '" + cm._chang_base_caption_search("탈퇴") + "'";
            Tsql = Tsql + "  When tbl_Memberinfo.LeaveCheck = -100 Then '" + cm._chang_base_caption_search("휴면") + "'";
            Tsql = Tsql + "  End AS LeaveCheck_2 ";

            Tsql = Tsql + " , Case tbl_Memberinfo.LineUserCheck When 1 then '" + cm._chang_base_caption_search("사용") + "' When 0 then '" + cm._chang_base_caption_search("중지") + "' End ";
            Tsql = Tsql + " , Case When tbl_Memberinfo.LeaveDate <> '' Then  LEFT(tbl_Memberinfo.LeaveDate,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.LeaveDate,4),2) + '-' + RIGHT(tbl_Memberinfo.LeaveDate,2) ELSE '' End ";
            Tsql = Tsql + " , Case When tbl_Memberinfo.LineUserDate <> '' Then  LEFT(tbl_Memberinfo.LineUserDate,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.LineUserDate,4),2) + '-' + RIGHT(tbl_Memberinfo.LineUserDate,2) ELSE '' End ";
            Tsql = Tsql + " , tbl_Memberinfo.recordid ";

            Tsql = Tsql + " , tbl_Memberinfo.recordtime ";

            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Sav (nolock) ON tbl_Memberinfo.Saveid = Sav.Mbid And tbl_Memberinfo.Saveid2 = Sav.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Nom (nolock) ON tbl_Memberinfo.Nominid = Nom.Mbid And tbl_Memberinfo.Nominid2 = Nom.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";
            Tsql = Tsql + " Left Join tbl_Bank On tbl_Memberinfo.bankcode=tbl_Bank.ncode ";
            cls_NationService.SQL_BankNationCode(ref Tsql);
            Tsql = Tsql + " Left Join tbl_Class C1 On tbl_Memberinfo.CurGrade=C1.Grade_Cnt ";

            if (Mbid.Length == 0)
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            }

        }

        private void Set_Memberinfo_Talk(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();

            Tsql = "Select  ";
            Tsql = Tsql + " TalkContent ";

            Tsql = Tsql + " ,Recordid ";

            Tsql = Tsql + ", Recordtime ";

            Tsql = Tsql + " , Seq ";
            Tsql = Tsql + " , ''   ,'','','','','' ";


            Tsql = Tsql + " From tbl_Memberinfo_Talk (nolock) ";

            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " Order by Seq DESC  ";
        }


        private void Set_Memberinfo_Up(string Mbid, int Mbid2, string Ordernumber, string S_TF, ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();


            //Tsql = "Select  ";
            //Tsql = Tsql + " T_AA.Lvl ";
            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + ", T_AA.mbid + '-' + Convert(Varchar,T_AA.mbid2) ";
            //else
            //    Tsql = Tsql + ", T_AA.mbid2 ";

            //Tsql = Tsql + " ,Isnull(CC_A.G_Name,'') ";
            //Tsql = Tsql + " ,A.M_Name ";
            //Tsql = Tsql + " , Case When A.Regtime <> '' Then  LEFT(A.Regtime,4) +'-' + LEFT(RIGHT(A.Regtime,4),2) + '-' + RIGHT(A.Regtime,2) ELSE '' End ";
            //Tsql = Tsql + " , Case When A.LeaveDate <> '' Then  LEFT(A.LeaveDate,4) +'-' + LEFT(RIGHT(A.LeaveDate,4),2) + '-' + RIGHT(A.LeaveDate,2) ELSE '' End ";
            //Tsql = Tsql + ", Isnull( tbl_Business.name,'') " ;


            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + " ,A.Saveid + '-' + Convert(Varchar,A.Saveid2) ";
            //else
            //    Tsql = Tsql + " ,A.Saveid2 ";

            //Tsql = Tsql + " , Isnull(b.M_Name,'') ";

            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + " ,A.Nominid + '-' + Convert(Varchar,A.Nominid2) ";
            //else
            //    Tsql = Tsql + " ,A.Nominid2 ";

            //Tsql = Tsql + " , Isnull(C.M_Name,'') ";
            //Tsql = Tsql + " , A.hometel ";
            //Tsql = Tsql + " , A.hptel ";

            //Tsql = Tsql + " , '' ";


            //if (S_TF == "SAVE")
            //{
            //    Tsql = Tsql + " , A.LineCnt ";
            //    Tsql = Tsql + " From ufn_matrix_Mem_mannatech('" + Mbid + "'," + Mbid2 + ", '*') T_AA ";
            //}
            //else
            //{
            //    Tsql = Tsql + " , A.N_LineCnt ";
            //    Tsql = Tsql + " From ufn_matrix_Nominid_mannatech('" + Mbid + "'," + Mbid2 + ",  '*') T_AA ";
            //}

            //Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS A  (nolock) ON A.Mbid = T_AA.mbid And A.Mbid2 = T_AA.Mbid2   ";
            //Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS B  (nolock) ON a.Saveid = b.mbid And a.Saveid2 = b.mbid2   ";
            //Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS C  (nolock) ON a.Nominid=c.mbid And a.Nominid2 = c.mbid2   ";
            //Tsql = Tsql + " LEFT Join tbl_Business  (nolock) On a.businesscode=tbl_Business.ncode  And a.Na_code = tbl_Business.Na_code";
            //Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On A.CurGrade=C1.Grade_Cnt " ;
            //Tsql = Tsql + " Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On CC_A.Mbid = A.Mbid And  CC_A.Mbid2 = A.Mbid2 ";            

            //Tsql = Tsql + " ORder by Lvl DESC";



            Tsql = "Select  ";
            Tsql = Tsql + " T_AA.Lvl ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", T_AA.mbid + '-' + Convert(Varchar,T_AA.mbid2) ";
            else
                Tsql = Tsql + ", T_AA.mbid2 ";

            Tsql = Tsql + " ,Isnull(C1.Grade_Name,'') ";
            Tsql = Tsql + " ,A.lastname+A.firstname ";
            Tsql = Tsql + " , Case When replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-','')   <> ''   Then  LEFT(replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-',''),4) +'-'  + LEFT(RIGHT(replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-',''),4),2) + '-'  + RIGHT(replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-',''),2) ELSE '' End  ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + ", '' ";


            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,A.Saveid + '-' + Convert(Varchar,A.Saveid2) ";
            else
                Tsql = Tsql + " ,A.sponsoralkynumber ";

            Tsql = Tsql + " ,  Isnull(B.lastname+B.firstname,'') ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,A.Nominid + '-' + Convert(Varchar,A.Nominid2) ";
            else
                Tsql = Tsql + " ,A.enrolleralkynumber ";

            Tsql = Tsql + " , Isnull(C.lastname+C.firstname ,'') ";
            Tsql = Tsql + " ,A.phonenumber  ";
            Tsql = Tsql + " , A.phonenumber  ";

            Tsql = Tsql + " , '' ";


            if (S_TF == "SAVE")
            {
                Tsql = Tsql + " , '1' ";
                Tsql = Tsql + " From ufn_matrix_Mem_mannatech ('" + Mbid + "'," + Mbid2 + ", '*') T_AA ";
            }
            else
            {
                Tsql = Tsql + " , '1' ";
                Tsql = Tsql + " From ufn_matrix_Nominid_mannatech ('" + Mbid + "'," + Mbid2 + ",  '*') T_AA ";
            }

            Tsql = Tsql + "LEFT JOIN  mannasync.dbo.CUSTOMER  AS A  (nolock) ON A.accountnumber = Convert(Varchar,T_AA.Mbid2)    ";
            Tsql = Tsql + " LEFT JOIN  mannasync.dbo.CUSTOMER  AS B  (nolock) ON A.sponsoralkynumber = B.accountnumber   ";
            Tsql = Tsql + "  LEFT JOIN  mannasync.dbo.CUSTOMER  AS C  (nolock) ON A.enrolleralkynumber = C.accountnumber     ";

            Tsql = Tsql + " LEFT Join tbl_Memberinfo  (nolock) On T_AA.Mbid = tbl_Memberinfo.Mbid   And T_AA.Mbid2 = tbl_Memberinfo.Mbid2 ";
            Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On tbl_Memberinfo.CurGrade = C1.Grade_Cnt ";
            //Tsql = Tsql + " Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On  CC_A.Mbid2 = A.accountnumber ";

            Tsql = Tsql + " ORder by Lvl DESC";
        }




        private void Set_Memberinfo_Down(string Mbid, int Mbid2, string Ordernumber, string S_TF, ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();


            Tsql = "Select  ";
            Tsql = Tsql + " T_AA.Lvl ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", T_AA.mbid + '-' + Convert(Varchar,T_AA.mbid2) ";
            else
                Tsql = Tsql + ", T_AA.mbid2 ";

            Tsql = Tsql + " , Isnull(C1.Grade_Name,'') ";
            Tsql = Tsql + " ,A.lastname+A.firstname ";
            Tsql = Tsql + " , Case When replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-','')   <> ''   Then  LEFT(replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-',''),4) +'-'  + LEFT(RIGHT(replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-',''),4),2) + '-'  + RIGHT(replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-',''),2) ELSE '' End   ";
            Tsql = Tsql + " , ''";
            Tsql = Tsql + ", '' ";


            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,A.Saveid + '-' + Convert(Varchar,A.Saveid2) ";
            else
                Tsql = Tsql + " ,A.sponsoralkynumber";

            Tsql = Tsql + " , Isnull(B.lastname+B.firstname,'') ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,A.Nominid + '-' + Convert(Varchar,A.Nominid2) ";
            else
                Tsql = Tsql + " ,A.enrolleralkynumber ";

            Tsql = Tsql + " , Isnull(C.lastname+C.firstname ,'') ";
            Tsql = Tsql + " , A.phonenumber ";
            Tsql = Tsql + " , A.phonenumber ";

            Tsql = Tsql + " , '' ";

            if (S_TF == "SAVE")
            {
                Tsql = Tsql + " , '1'  ";
                Tsql = Tsql + " From ufn_GetSubTree_MemGroup_mannatech ('" + Mbid + "'," + Mbid2 + ") T_AA ";
            }

            else
            {
                Tsql = Tsql + " , '1' ";
                Tsql = Tsql + " From ufn_GetSubTree_NomGroup_mannatech ('" + Mbid + "'," + Mbid2 + ") T_AA ";
            }

            Tsql = Tsql + " LEFT JOIN mannasync.dbo.CUSTOMER AS A  (nolock) ON A.accountnumber = Convert (varchar, T_AA.Mbid2)    ";
            Tsql = Tsql + " LEFT JOIN mannasync.dbo.CUSTOMER AS B  (nolock) ON A.sponsoralkynumber = B.accountnumber   ";
            Tsql = Tsql + " LEFT JOIN mannasync.dbo.CUSTOMER AS C  (nolock) ON A.enrolleralkynumber = C.accountnumber   ";
            //Tsql = Tsql + " LEFT Join tbl_Business  (nolock) On a.businesscode=tbl_Business.ncode And a.Na_code = tbl_Business.Na_code";
            //Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On A.CurGrade=C1.Grade_Cnt ";
            //Tsql = Tsql + " Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On  CC_A.Mbid2 = A.accountnumber";            


            Tsql = Tsql + " LEFT Join tbl_Memberinfo  (nolock) On T_AA.Mbid = tbl_Memberinfo.Mbid   And T_AA.Mbid2 = tbl_Memberinfo.Mbid2 ";
            Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On tbl_Memberinfo.CurGrade = C1.Grade_Cnt ";

            Tsql = Tsql + " Where T_AA.Lvl > 0 ";
            Tsql = Tsql + " ORder by Lvl ASC, LEFT(SaveCur,3) ASC   , SaveCur ASC ";

        }


        private void Set_gr_dic_Info(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
            int Col_Cnt = 0;

            object[] row0 = new object[Base_dgv.grid_col_Count];

            while (Col_Cnt < Base_dgv.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt].ToString();
                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void Set_gr_dic_Info(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt, int Sort_Number)
        {
            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
            int Col_Cnt = 0;

            object[] row0 = new object[Base_dgv.grid_col_Count];

            while (Col_Cnt < Base_dgv.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void Set_gr_dic_Info_Cacu(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0] ,
                                ds.Tables[base_db_name].Rows[fi_cnt][1],
                                ds.Tables[base_db_name].Rows[fi_cnt][2] ,
                                ds.Tables[base_db_name].Rows[fi_cnt][3] ,
                                ds.Tables[base_db_name].Rows[fi_cnt][4],

                                encrypter.Decrypt ( ds.Tables[base_db_name].Rows[fi_cnt][5].ToString () ),
                                ds.Tables[base_db_name].Rows[fi_cnt][6] ,
                                ds.Tables[base_db_name].Rows[fi_cnt][7] ,
                                ds.Tables[base_db_name].Rows[fi_cnt][8] ,
                                ds.Tables[base_db_name].Rows[fi_cnt][8] ,

                                 };


            gr_dic_text[fi_cnt + 1] = row0;
        }

    }//end cls_Grid_Base_info_Put

}
