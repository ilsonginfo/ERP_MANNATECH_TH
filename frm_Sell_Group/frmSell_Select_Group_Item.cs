using System;
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
    public partial class frmSell_Select_Group_Item : Form
    {
       


        

        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Item = new cls_Grid_Base();
        
        private const string base_db_name = "tbl_SalesDetail";
        private int Data_Set_Form_TF;

       
        Series series_Item = new Series();

        public frmSell_Select_Group_Item()
        {
            InitializeComponent();
        }


        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();
        }

        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Item.d_Grid_view_Header_Reset();
         
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_SellCode_ComboBox(combo_Se, combo_Se_Code);
            cpbf.Put_Rec_Code_ComboBox(combo_Rec, combo_Rec_Code);

            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            mtxtSellDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtSellDate3.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate4.Mask = cls_app_static_var.Date_Number_Fromat;


            txt_P_1.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_3.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_4.BackColor = cls_app_static_var.txt_Enable_Color;


            txt_PP_1.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_PP_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_PP_3.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_PP_4.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_PP_5.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_PP_6.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_PP_7.BackColor = cls_app_static_var.txt_Enable_Color;
            
        }

        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Select.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_Excel.Left = butt_Select.Left + butt_Select.Width + 2;
            butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;
            butt_Excel_2.Left = butt_Delete.Left + butt_Delete.Width + 2;

            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Select);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);
            cfm.button_flat_change(butt_Excel_2); 
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
                    mtxtMbid.Focus();     return false;
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


            if (mtxtSellDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate1.Text, mtxtSellDate1, "Date") == false)
                {
                    mtxtSellDate1.Focus();
                    return false;
                }

            }

            if (mtxtSellDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate2.Text, mtxtSellDate2, "Date") == false)
                {
                    mtxtSellDate2.Focus();
                    return false;
                }
            }

            if (mtxtSellDate3.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate3.Text, mtxtSellDate3, "Date") == false)
                {
                    mtxtSellDate3.Focus();
                    return false;
                }

            }

            if (mtxtSellDate4.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate4.Text, mtxtSellDate4, "Date") == false)
                {
                    mtxtSellDate4.Focus();
                    return false;
                }
            }



            if (txtMakDate1.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtMakDate1);

                if (Ret == -1)
                {
                    txtMakDate1.Focus(); return false;
                }
            }

            if (txtMakDate2.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtMakDate2);

                if (Ret == -1)
                {
                    txtMakDate2.Focus(); return false;
                }
            }


                   

            return true;
        }


        private void Make_Base_Query(ref string Tsql)
        {

            //string[] g_HeaderText = {"상품코드"  , "상품명"   , "주문수량"  , "주문액"   , "주문PV"        
            //                    , ""   , ""    , ""   , ""    , ""                                
            //                        };
            
                Tsql = "Select  tbl_salesitemdetail.ItemCode  ";
                Tsql = Tsql + " ,Isnull(tbl_Goods.Name,'')  ";
                Tsql = Tsql + " ,Sum(tbl_salesitemdetail.ItemCount) as Total3  ";
                Tsql = Tsql + " ,Sum(ItemTotalPrice) as Total1 ";
                Tsql = Tsql + " ,Sum(ItemTotalPV) as Total2 ";
                Tsql = Tsql + " ,Sum(ItemTotalCV) as Total3 ";
                Tsql = Tsql + "  , '' , '' , '' , '' ";
                Tsql = Tsql + " From tbl_SalesitemDetail (nolock) ";
                Tsql = Tsql + " LEFT Join tbl_SalesDetail  (nolock) On tbl_SalesDetail.OrderNumber = tbl_salesitemdetail.OrderNumber ";
                Tsql = Tsql + " LEFT Join tbl_StockOutput(nolock) On tbl_StockOutput.OrderNumber = tbl_salesitemdetail.OrderNumber and tbl_StockOutput.Salesitemindex = tbl_salesitemdetail.SalesItemIndex ";
                Tsql = Tsql + " LEFT JOIN tbl_Sales_Rece (nolock)  ON tbl_salesitemdetail.OrderNumber = tbl_Sales_Rece.OrderNumber And (tbl_salesitemdetail.Salesitemindex = tbl_Sales_Rece.Salesitemindex OR  -tbl_salesitemdetail.Salesitemindex = tbl_Sales_Rece.Salesitemindex ) ";
                Tsql = Tsql + " LEFT Join tbl_Memberinfo  (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";
                Tsql = Tsql + " LEFT Join tbl_Goods  (nolock) ON tbl_Goods.Ncode = tbl_salesitemdetail.itemCode";
                Tsql = Tsql + " Left Join tbl_SellType  (nolock) On tbl_SellType.SellCode=tbl_SalesDetail.SellCode  ";
         
                      
        }



        private void Make_Base_Query_(ref string Tsql)
        {

            //string strSql = " Where  tbl_SalesDetail.Ga_Order = 0 ";
            string strSql = " Where   (tbl_SalesDetail.Ga_Order = 0 OR (tbl_SalesDetail.Ga_Order = 1 And  tbl_SalesDetail.ReturnTF = 5 ) )";
            //string strSql = " Where   tbl_SalesDetail.Ga_Order = 0 and returntf = 1 ";
           // strSql = strSql + "and tbl_SalesDetail.ReturnTF <> 3 and tbl_SalesDetail.ReturnTF <> 4 And tbl_SalesDetail.SellCode <> ''  ";            

                
            //가입일자로 검색 -1
            if ((mtxtSellDate1.Text.Replace("-","").Trim() != "") && (mtxtSellDate2.Text.Replace("-","").Trim() == ""))
                strSql = strSql + " And tbl_SalesDetail.SellDate = '" + mtxtSellDate1.Text.Replace("-","").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtSellDate1.Text.Replace("-","").Trim() != "") && (mtxtSellDate2.Text.Replace("-","").Trim() != ""))
            {
                strSql = strSql + " And tbl_SalesDetail.SellDate >= '" + mtxtSellDate1.Text.Replace("-","").Trim() + "'";
                strSql = strSql + " And tbl_SalesDetail.SellDate <= '" + mtxtSellDate2.Text.Replace("-","").Trim() + "'";
            }

            //정산일자로 검색 -1
            if ((mtxtSellDate3.Text.Replace("-", "").Trim() != "") && (mtxtSellDate4.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_SalesDetail.SellDate_2 = '" + mtxtSellDate3.Text.Replace("-", "").Trim() + "'";

            //정산일자로 검색 -2
            if ((mtxtSellDate3.Text.Replace("-", "").Trim() != "") && (mtxtSellDate4.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_SalesDetail.SellDate_2 >= '" + mtxtSellDate3.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_SalesDetail.SellDate_2 <= '" + mtxtSellDate4.Text.Replace("-", "").Trim() + "'";
            }

            //출고일자로 검색 -1
            if ((mtxtSellDate5.Text.Replace("-", "").Trim() != "") && (mtxtSellDate6.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_StockOutput.out_date = '" + mtxtSellDate5.Text.Replace("-", "").Trim() + "'";

            //출고일자로 검색 -2
            if ((mtxtSellDate5.Text.Replace("-", "").Trim() != "") && (mtxtSellDate6.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_StockOutput.out_date >= '" + mtxtSellDate5.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_StockOutput.out_date <= '" + mtxtSellDate6.Text.Replace("-", "").Trim() + "'";


            }
            if (txtCenter2_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.BusCode = '" + txtCenter2_Code.Text.Trim() + "'";


            if (combo_Rec_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Sales_Rece.Receive_Method = " + combo_Rec_Code.Text.Trim(); 


            if (txtSellCode_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.SellCode = '" + txtSellCode_Code.Text.Trim() + "'";
                       

          //  strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_SalesDetail.BusCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";

            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


            if (chk_Re.Checked == false)
            {
                if(mtxtSellDate5.Text.Trim() == "")
                {
                    strSql = strSql + " And (LEFT(SellState,1)='N' OR LEFT(SellState,1)='C' OR LEFT(tbl_salesitemdetail.OrderNumber,2)='RC') ";
                    
                }
                else
                {
                    strSql = strSql + " And  LEFT(tbl_salesitemdetail.OrderNumber,2) <>'RC' ";
                }
                    
            }


            Tsql = Tsql + strSql ;            
        }

        private void Make_Base_Query2(ref string Tsql)
        {


            Tsql = "SELECT ItemCode, ItemName   ";
            Tsql = Tsql + " , SUM(TotalCnt ) TotalCnt     ";
            Tsql = Tsql + " , SUM(Total1   ) Total1   ";
            Tsql = Tsql + " , SUM(Total2   ) Total2   ";
            Tsql = Tsql + " , SUM(Total3   ) Total3   ";
            Tsql = Tsql + "  , '' , '' , '' , '' ";
            Tsql = Tsql + " from   ";
            Tsql = Tsql + " (  ";
            Tsql = Tsql + " Select  A.ItemCode    ";
            Tsql = Tsql + "  ,Isnull(tbl_Goods.Name,'')    as ItemName  ";
            Tsql = Tsql + "  ,Sum(A.ItemCount)		 as TotalCnt  ";
            Tsql = Tsql + "  ,Sum(A.ItemTotalPrice)  as Total1   ";
            Tsql = Tsql + "  ,Sum(A.ItemTotalPV)	 as Total2   ";
            Tsql = Tsql + "  ,Sum(A.ItemTotalCV)	 as Total3   ";
            Tsql = Tsql + "  From tbl_SalesitemDetail AS A  (nolock)   ";
            Tsql = Tsql + "  LEFT Join tbl_SalesDetail B (nolock) On A.OrderNumber = B.OrderNumber  ";
            Tsql = Tsql + "   ";
            Tsql = Tsql + "  LEFT Join tbl_StockOutput(nolock) On tbl_StockOutput.OrderNumber = A.OrderNumber and tbl_StockOutput.Salesitemindex = A.SalesItemIndex   ";
            Tsql = Tsql + "  LEFT JOIN tbl_Sales_Rece (nolock)  ON A.OrderNumber = tbl_Sales_Rece.OrderNumber And (A.Salesitemindex = tbl_Sales_Rece.Salesitemindex OR  -A.Salesitemindex = tbl_Sales_Rece.Salesitemindex )   ";
            Tsql = Tsql + "  LEFT Join tbl_Memberinfo  (nolock) ON tbl_Memberinfo.Mbid = B.Mbid And tbl_Memberinfo.Mbid2 = B.Mbid2   ";
            Tsql = Tsql + "  LEFT Join tbl_Goods  (nolock) ON tbl_Goods.Ncode = A.itemCode  ";
            Tsql = Tsql + "  Left Join tbl_SellType  (nolock) On tbl_SellType.SellCode=B.SellCode     ";
        }
        private void Make_Base_Query2_(ref string Tsql)
        {
            string strSql =  "  Where   B.Ga_Order = 0  And B.SellCode <> ''   ";
            strSql = strSql + "  AND B.ReturnTF = 1   ";
            //출고일자로 검색 -1
            if ((mtxtSellDate5.Text.Replace("-", "").Trim() != "") && (mtxtSellDate6.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_StockOutput.out_date = '" + mtxtSellDate5.Text.Replace("-", "").Trim() + "'";

            //출고일자로 검색 -2
            if ((mtxtSellDate5.Text.Replace("-", "").Trim() != "") && (mtxtSellDate6.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_StockOutput.out_date >= '" + mtxtSellDate5.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_StockOutput.out_date <= '" + mtxtSellDate6.Text.Replace("-", "").Trim() + "'";


            }
            strSql = strSql + "  And B.BusCode in ( Select Center_Code From ufn_User_In_Center ('','') )   ";
            strSql = strSql + "  And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('') ) Group By A.ItemCode , Isnull(tbl_Goods.Name,'')    ";
            strSql = strSql + "  UNION ALL   ";

            Tsql = Tsql + strSql;
        }
        private void Make_Base_Query3(ref string Tsql)
        {
            Tsql = Tsql + "  Select  A.ItemCode    ";
            Tsql = Tsql + "  ,Isnull(tbl_Goods.Name,'')  as ItemName  ";
            Tsql = Tsql + "  ,Sum(A.ItemCount)		 as TotalCnt    ";
            Tsql = Tsql + "  ,Sum(A.ItemTotalPrice)  as Total1   ";
            Tsql = Tsql + "  ,Sum(A.ItemTotalPV)	 as Total2   ";
            Tsql = Tsql + "  ,Sum(A.ItemTotalCV)	 as Total3   ";
            Tsql = Tsql + "  From tbl_SalesitemDetail AS A  (nolock)   ";
            Tsql = Tsql + "  LEFT Join tbl_SalesDetail B (nolock) On A.OrderNumber = B.OrderNumber  ";
            Tsql = Tsql + "   ";
            Tsql = Tsql + "  LEFT JOIN tbl_Sales_Rece (nolock)  ON A.OrderNumber = tbl_Sales_Rece.OrderNumber And (A.Salesitemindex = tbl_Sales_Rece.Salesitemindex OR  -A.Salesitemindex = tbl_Sales_Rece.Salesitemindex )   ";
            Tsql = Tsql + "  LEFT Join tbl_Memberinfo  (nolock) ON tbl_Memberinfo.Mbid = B.Mbid And tbl_Memberinfo.Mbid2 = B.Mbid2   ";
            Tsql = Tsql + "  LEFT Join tbl_Goods  (nolock) ON tbl_Goods.Ncode = A.itemCode  ";
            Tsql = Tsql + "  Left Join tbl_SellType  (nolock) On tbl_SellType.SellCode=B.SellCode     ";
        }
        private void Make_Base_Query3_(ref string Tsql)
        {
            string strSql = "  Where   B.Ga_Order = 0  And B.SellCode <> ''   ";
            strSql = strSql + "  AND B.ReturnTF IN (2,3,4)  ";
            //출고일자로 검색 -1
            if ((mtxtSellDate5.Text.Replace("-", "").Trim() != "") && (mtxtSellDate6.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And  And B.SellDate = '" + mtxtSellDate5.Text.Replace("-", "").Trim() + "'";

            //출고일자로 검색 -2
            if ((mtxtSellDate5.Text.Replace("-", "").Trim() != "") && (mtxtSellDate6.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + "  And B.SellDate >= '" + mtxtSellDate5.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And B.SellDate <= '" + mtxtSellDate6.Text.Replace("-", "").Trim() + "'";


            }
            strSql = strSql + " And B.BusCode in ( Select Center_Code From ufn_User_In_Center ('','') )  ";
            strSql = strSql + "  And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('') ) Group By A.ItemCode , Isnull(tbl_Goods.Name,'')      ";
            strSql = strSql + "   ) A     ";
            strSql = strSql + "   GROUP BY ItemCode, ItemName   ";

            Tsql = Tsql + strSql;
        }


        private void Base_Grid_Set()
        {   
            string Tsql = "";
            if (chk_stock.Checked == false)
            {
                Make_Base_Query(ref Tsql);

                Make_Base_Query_(ref Tsql);

                Tsql = Tsql + " Group By tbl_salesitemdetail.ItemCode , Isnull(tbl_Goods.Name,'') ";
                Tsql = Tsql + " Order By tbl_salesitemdetail.ItemCode , Isnull(tbl_Goods.Name,'')  ";
            }
            else
            {
                Make_Base_Query2(ref Tsql);

                Make_Base_Query2_(ref Tsql);

                Make_Base_Query3(ref Tsql);

                Make_Base_Query3_(ref Tsql);
            }
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();                                  
            
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name , this.Text ) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            double Sum_04 = 0; double Sum_05 = 0; double Sum_03 = 0, Sum_06= 0 ;
            //double Sum_13 = 0; double Sum_14 = 0; double Sum_15 = 0;
            //double Sum_16 = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            //Dictionary<int, string[]> gr_dic_text = new Dictionary<int, string[]>();

            chart_Item.Series.Clear();
            Save_Nom_Line_Chart();
            string ItemCode = ""; int ItemCnt = 0;
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                ItemCode = ds.Tables[base_db_name].Rows[fi_cnt][1].ToString();
                ItemCnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt][2].ToString());

                //if (ItemCode.Replace(" ", "").Length >= 5)
                //{
                //    ItemCode = ItemCode.Replace(" ", "").Substring(0, 5);                    
                //    Push_data(series_Item, ItemCode.Replace(" ", "").Substring(0, 5), ItemCnt);
                //}
                //else
                //{
                //    ItemCode = ItemCode.Replace(" ", "");
                //    Push_data(series_Item, ItemCode.Replace(" ", ""), ItemCnt);
                //}
                    

                Sum_03 = Sum_03 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][2].ToString());
                Sum_04 = Sum_04 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][3].ToString());
                Sum_05 = Sum_05 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][4].ToString());
                Sum_06 = Sum_06 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][5].ToString());             
            }



            if (gr_dic_text.Count > 0)
            {
                txt_P_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_03);
                txt_P_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_04);
                txt_P_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_05);
                txt_P_4.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_06);              
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
            //cgb.grid_name = gr_dic_text;  //배열을 클래스로 보낸다.
            //cgb.db_grid_Data_Put();
        }



        private void dGridView_Base_Header_Reset()
        {
            
            cgb.grid_col_Count = 10;            
            cgb.basegrid = dGridView_Base;            
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            //cgb_Item.grid_Frozen_End_Count = 3;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
            string[] g_HeaderText = {"상품코드"  , "상품명"   , "주문수량"  , "주문액"   , "주문PV"        
                                , "주문CV"     , ""    , ""   , ""    , ""                                
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 90, 100, 80, 80, 80                                                          
                             ,80 , 0 , 0 , 0 , 0                             
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                        
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight  //5
                               
                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleRight //10
                           
                              };
            cgb.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = "###,###,###,##0";
            gr_dic_cell_format[4 - 1] = "###,###,###,##0";
            gr_dic_cell_format[5 - 1] = "###,###,###,##0";
            gr_dic_cell_format[6 - 1] = "###,###,###,##0";

            cgb.grid_cell_format = gr_dic_cell_format;
            cgb.basegrid.RowHeadersVisible = false;
            
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


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, string[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;

            string[] row0 = new string[cgb.grid_col_Count];
            string t_caption = "";
            while (Col_Cnt < cgb.grid_col_Count)
            {
                t_caption= ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt].ToString ();

                if (Col_Cnt ==2 || Col_Cnt ==3 || Col_Cnt ==4 )
                    t_caption = string.Format(cls_app_static_var.str_Currency_Type, int.Parse(t_caption));

                row0[Col_Cnt] = t_caption ;
                Col_Cnt++;
            }


            gr_dic_text[fi_cnt + 1] = row0;
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



        private void MtxtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            //회원번호 관련칸은 소문자를 다 대문자로 만들어 준다.
            if (e.KeyChar >= 97 && e.KeyChar <= 122)
            {
                string str = e.KeyChar.ToString().ToUpper();
                char[] ch = str.ToCharArray();
                e.KeyChar = ch[0];
            }

            if (e.KeyChar == 13)
            {        
                SendKeys.Send("{TAB}");
            }
        }

        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            MaskedTextBox tb = (MaskedTextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
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

        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            int Sw_Tab = 0;

            if ((sender is TextBox) == false)  return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
                Sw_Tab = 1;
            }

            if (tb.Name == "txtCenter")
            {
                if (tb.Text.Trim() == "")
                    txtCenter_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtCenter_Code);
            }

            if (tb.Name == "txtBank")
            {
                if (tb.Text.Trim() == "")
                    txtSellCode_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtSellCode_Code);
            }

            if (tb.Name == "txtR_Id")
            {
                if (tb.Text.Trim() == "")
                    txtR_Id_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtR_Id_Code);
            }

            if (tb.Name == "txtCenter2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter2_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtSellCode")
            {
                if (tb.Text.Trim() == "")
                    txtSellCode_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtSellCode_Code);
            }
        }

        

        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {            
            if (tb.Name == "txtCenter")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtCenter_Code,"");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

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

            if (tb.Name == "txtBank")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtSellCode_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtSellCode_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtSellCode_Code);

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

            if (tb.Name == "txtSellCode")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtSellCode_Code);

                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtSellCode_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtSellCode_Code);

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

            cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
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
            cgb_Pop.Base_tb_2 = tb ;    //2번은 명임
            cgb_Pop.Base_Location_obj = tb;

            if (strSql != "")
            {
                if (tb.Name == "txtCenter")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtBank")
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);

                if (tb.Name == "txtCenter2")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);
           
                if (tb.Name == "txtSellCode")
                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", strSql);
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

                if (tb.Name == "txtR_Id")
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

                if (tb.Name == "txtCenter2")
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

            }
        }



        private void Ncod_Text_Set_Data(TextBox tb, TextBox tb1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql="";
            
            if (tb.Name == "txtCenter")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            }

            if (tb.Name == "txtR_Id")
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


            if (tb.Name == "txtCenter2")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            }
          

            if (tb.Name == "txtSellCode")
            {
                Tsql = "Select SellCode ,SellTypeName    ";
                Tsql = Tsql + " From tbl_SellType (nolock) ";
                Tsql = Tsql + " Where SellCode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    SellTypeName like '%" + tb.Text.Trim() + "%'";
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







        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_Item.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtSellDate1);

                chart_Item.Series.Clear();
                
                opt_Ed_1.Checked = true;  opt_sell_1.Checked = true;
                chk_Re.Checked = false;
               // radioB_S.Checked = true;
                combo_Se.SelectedIndex = -1;


            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                
                dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_Item.d_Grid_view_Header_Reset();

                txt_PP_1.Text = ""; txt_PP_2.Text = ""; txt_PP_3.Text = ""; txt_PP_7.Text = "";

                txt_P_1.Text = ""; txt_P_2.Text = ""; txt_P_3.Text = ""; txt_P_4.Text = "";
                //txt_P_4.Text = ""; txt_P_5.Text = ""; txt_P_6.Text = "";
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                chart_Item.Series.Clear();

                if (Check_TextBox_Error() == false) return;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex;
                combo_Rec_Code.SelectedIndex = combo_Rec.SelectedIndex;
                Base_Grid_Set();  //뿌려주는 곳
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }
           
            else if (bt.Name == "butt_Excel")
            {
                frmBase_Excel e_f = new frmBase_Excel();
                e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
                e_f.ShowDialog();
            }

            else if (bt.Name == "butt_Excel_2")
            {
                frmBase_Excel e_f = new frmBase_Excel();
                e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_2_Info);
                e_f.ShowDialog();
            }

            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }

            else if (bt.Name  == "butt_Exp")
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
            Excel_Export_File_Name = this.Text; // "Sell_Group_Item";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }

        private DataGridView e_f_Send_Export_Excel_2_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Sell_Group_Date";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base_Sub;
        }

       

        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Item.d_Grid_view_Header_Reset();

            txt_PP_1.Text = ""; txt_PP_2.Text = ""; txt_PP_3.Text = ""; 

            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string ItemCode = ""; 
                ItemCode = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Item_Grid_Set(ItemCode);
                this.Cursor = System.Windows.Forms.Cursors.Default; 
            }            
        }


        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);       
        }





        private void Item_Grid_Set(string ItemCode )
        {
            

                       
            string Tsql = "";

                  //string[] g_HeaderText = {"주문번호"  , ""   , "주문일자"  , "주문종류"   , "상품코드"        
            //                    , "상품명"   , "개별단가"    , "개별PV"  , "주문_수량" , "총상품액"
            //                    ,"총상품PV" , "회원_번호" , "성명" , "주문_센타명", "구분" 
            //                    };

            cls_form_Meth cm = new cls_form_Meth();
            Tsql = "Select tbl_SalesDetail.OrderNumber ";
            Tsql = Tsql + " ,tbl_salesitemdetail.SalesItemIndex ";
            Tsql = Tsql + " ,tbl_SalesDetail.SellDate ";
            Tsql = Tsql + " ,tbl_SellType.SellTypeName ";
            Tsql = Tsql + " ,tbl_salesitemdetail.ItemCode ";
            Tsql = Tsql + " ,tbl_Goods.Name ";
            Tsql = Tsql + " ,itemPrice ";
            Tsql = Tsql + " ,ItemPv" ;
            Tsql = Tsql + " ,ItemCv";
            Tsql = Tsql + " ,tbl_salesitemdetail.ItemCount ";
            Tsql = Tsql + " ,ItemTotalPrice ";
            Tsql = Tsql + " ,ItemTotalPV   ";
            Tsql = Tsql + " ,ItemTotalCV   ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", tbl_SalesDetail.mbid + '-' + Convert(Varchar,tbl_SalesDetail.mbid2) ";
            else
                Tsql = Tsql + ", tbl_SalesDetail.mbid2 ";
            Tsql = Tsql + " , tbl_SalesDetail.M_Name ";
            Tsql = Tsql + " , tbl_Business.Name  ";
            Tsql = Tsql + " ,Case When ReturnTF = 1 Then '" + cm._chang_base_caption_search("정상") + "'";
            Tsql = Tsql + "  When ReturnTF = 2 Then '" + cm._chang_base_caption_search("반품") + "'";
            Tsql = Tsql + "  When ReturnTF = 4 Then '" + cm._chang_base_caption_search("교환") + "'";
            Tsql = Tsql + "  When ReturnTF = 3 Then '" + cm._chang_base_caption_search("부분반품") + "'";
            Tsql = Tsql + "  When ReturnTF = 5 Then '" + cm._chang_base_caption_search("취소") + "'";
            Tsql = Tsql + " END ReturnTFName ";
            Tsql = Tsql + " ,tbl_SalesDetail.SellDate_2   ";
            Tsql = Tsql + " From  tbl_SalesitemDetail (nolock) " ;

            Tsql = Tsql + " LEFT Join tbl_SalesDetail   (nolock) On tbl_SalesDetail.OrderNumber=tbl_salesitemdetail.OrderNumber ";
            //Tsql = Tsql + " LEFT JOIN tbl_Sales_Rece (nolock)  ON tbl_salesitemdetail.OrderNumber = tbl_Sales_Rece.OrderNumber And tbl_salesitemdetail.Salesitemindex = tbl_Sales_Rece.Salesitemindex ";
            Tsql = Tsql + " LEFT Join tbl_StockOutput(nolock) On tbl_StockOutput.OrderNumber = tbl_salesitemdetail.OrderNumber and tbl_StockOutput.Salesitemindex = tbl_salesitemdetail.SalesItemIndex ";
            Tsql = Tsql + " LEFT JOIN tbl_Sales_Rece (nolock)  ON tbl_salesitemdetail.OrderNumber = tbl_Sales_Rece.OrderNumber And (tbl_salesitemdetail.Salesitemindex = tbl_Sales_Rece.Salesitemindex OR  -tbl_salesitemdetail.Salesitemindex = tbl_Sales_Rece.Salesitemindex ) "; 
            Tsql = Tsql + " LEFT Join tbl_Memberinfo  (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";
            Tsql = Tsql + " LEFT Join tbl_Goods  (nolock) ON tbl_Goods.Ncode = tbl_salesitemdetail.itemCode";
            Tsql = Tsql + " Left Join tbl_SellType  (nolock) On tbl_SellType.SellCode=tbl_SalesDetail.SellCode  ";
            Tsql = Tsql + " Left Join tbl_Business  (nolock) On tbl_Business.Ncode=tbl_SalesDetail.BusCode  And tbl_SalesDetail.Na_code = tbl_Business.Na_code ";
            

            Make_Base_Query_(ref Tsql);

            Tsql = Tsql + " And  tbl_salesitemdetail.ItemCode = '" + ItemCode + "'";
            Tsql = Tsql + " Order by  tbl_SalesDetail.OrderNumber  ";
         

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            double Sum_11 = 0; //double Sum_07 = 0; double Sum_08 = 0;
            double Sum_09 = 0; double Sum_10 = 0, Sum_12 = 0; //double Sum_15 = 0;
            //double Sum_16 = 0;
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_Item(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
               
              
                Sum_09 = Sum_09 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][9].ToString());
                Sum_10 = Sum_10 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][10].ToString());
                Sum_11 = Sum_11 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][11].ToString());
                Sum_12 = Sum_12 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][12].ToString());
            }


            if (gr_dic_text.Count > 0)
            {
                txt_PP_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_09);
                txt_PP_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_10);
                txt_PP_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_11);
                txt_PP_7.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_12);
            }
                       

            cgb_Item.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Item.db_grid_Obj_Data_Put();
        }


        private void Set_gr_Item(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;

            object[] row0 = new object[cgb_Item.grid_col_Count];

            while (Col_Cnt < cgb_Item.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }


            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Sell_Item_Header_Reset()
        {
            cgb_Item.Grid_Base_Arr_Clear();
            cgb_Item.basegrid = dGridView_Base_Sub;
            cgb_Item.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Item.grid_col_Count = 18;

            cgb_Item.grid_Frozen_End_Count = 3;
            cgb_Item.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"주문번호"  , ""   , "주문일자"  , "주문종류"   , ""        
                                , ""   , "개별단가"    , "개별PV" , "개별CV" , "주문_수량" 
                                , "총상품액"   ,"총상품PV" ,"총상품CV" , "회원_번호" , "성명" 
                                , "주문_센타명", "구분" , "정산일자"
                                };

            int[] g_Width = { 100, 0, 90, 80, 0
                            ,0 , 70 , 70 , 70 , 80
                            ,80 , 80 , 100 , 100 , 80
                            , 80, 80, 80
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight 
                                ,DataGridViewContentAlignment.MiddleRight  //10

                                ,DataGridViewContentAlignment.MiddleRight  //10
                                ,DataGridViewContentAlignment.MiddleRight  //10
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter

                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleCenter  //15
                                 ,DataGridViewContentAlignment.MiddleCenter  //15
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;


            cgb_Item.grid_col_header_text = g_HeaderText;
            cgb_Item.grid_cell_format = gr_dic_cell_format;
            cgb_Item.grid_col_w = g_Width;
            cgb_Item.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                            
                                    ,true , true,  true,  true ,true     
                                    ,  true ,true,true
                                   };
            cgb_Item.grid_col_Lock = g_ReadOnly;
            
        }
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail





        private void radioB_S_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtSellDate1, mtxtSellDate2, (RadioButton)sender);
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
            series_Item.Name = cm._chang_base_caption_search("상품별");            
            series_Item.ChartType = SeriesChartType.Column;
            series_Item.Legend = "Legend1";
            chart_Item.Series.Add(series_Item);
            
            chart_Item.ChartAreas[0].AxisX.Interval = 1;
            chart_Item.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_Item.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 7;
        }

        
 


        
























    }
}
