using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Printing;

namespace MLM_Program
{
    public partial class frmStock_OUT_Sell_Check : clsForm_Extends
    {
       
        
        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        
        cls_Grid_Base cgb = new cls_Grid_Base();
        
        private const string base_db_name = "tbl_SalesItemDetail";
        private int Data_Set_Form_TF;


        //프린트 관련 변수들 >>>>>
        private int Prv_SW = 0;
        private int print_Page = 0, P_chk_cnt = 0, Print_Row = 0;
        private string P_Ordernumber = "";
        //프린트 관련 변수들 <<<<<



        public frmStock_OUT_Sell_Check()
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

            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_SellCode_ComboBox(combo_Se, combo_Se_Code);
            cpbf.Put_Rec_Code_ComboBox(combo_Rec, combo_Rec_Code);

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            mtxtSellDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtOutDate.Mask = cls_app_static_var.Date_Number_Fromat;
            

            txt_P_1.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_3.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_5.BackColor = cls_app_static_var.txt_Enable_Color;


            tableLayoutPanel6.Visible = false;
            tableLayoutPanel9.Visible = false;
            txtCenter3.Text = "";
            txtCenter3_Code.Text = "";
            mtxtOutDate.Text = "";

            radioB_SellTF2.Checked = true; 

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

            cfm.button_flat_change(butt_S_check);
            cfm.button_flat_change(butt_S_Not_check);
            cfm.button_flat_change(butt_S_Save);
            cfm.button_flat_change(button2);
            
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

            if (mtxtSellDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate2.Text, mtxtSellDate2, "Date") == false)
                {
                    mtxtSellDate2.Focus();
                    return false;
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

            ////string[] g_HeaderText = {"주문번호"  , "주문_일자"   ,  "회원_번호"   , "성명"   , "주민번호"       
            ////                        , "상품코드"    , "상품명"   , "개별단가"    , "개별PV" ,  "주문_수량"
            ////                       ,"출고_수량" , "총상품액"    , "총상품PV" , "주문_종류"   , "구분" 
            ////                      , "배송구분" , "등록_센타명"    , "주문_센타명"   , ""    , ""                                 
            ////                        };


            Tsql = "Select '' , ";
            Tsql = Tsql + "  tbl_SalesDetail.OrderNumber  ";

            Tsql = Tsql + " ,LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2)   ";
            
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ",Case When tbl_SalesDetail.SellCode <> '' Then  tbl_SalesDetail.mbid + '-' + Convert(Varchar,tbl_SalesDetail.mbid2) ELSE  tbl_SalesDetail.mbid End ";
            else
                Tsql = Tsql + ",Case When tbl_SalesDetail.SellCode <> '' Then  Convert(varchar,tbl_SalesDetail.mbid2)  ELSE  tbl_SalesDetail.mbid End ";

            Tsql = Tsql + " ,tbl_SalesDetail.M_Name ";

            Tsql = Tsql + ", tbl_SalesItemDetail.RecordTime ";

            Tsql = Tsql + " , tbl_SalesItemDetail.ItemCode "; 
            Tsql = Tsql + " , tbl_Goods.Name Item_Name ";
            Tsql = Tsql + " , tbl_SalesItemDetail.ItemPrice ";
            Tsql = Tsql + " , tbl_SalesItemDetail.ItemPV ";

            Tsql = Tsql + " , tbl_SalesItemDetail.ItemCount ";
            Tsql = Tsql + " , tbl_SalesItemDetail.Send_itemCount1 ";

            Tsql = Tsql + " , tbl_SalesItemDetail.ItemTotalPrice ";
            Tsql = Tsql + " , tbl_SalesItemDetail.ItemTotalPV ";

            Tsql = Tsql + " , Isnull ( tbl_SellType.SellTypeName , '직원주문' )  SellCodeName  ";


            cls_form_Meth cm = new cls_form_Meth();
            Tsql = Tsql + " ,Case When SellState = 'N_1' Then '" + cm._chang_base_caption_search("정상") + "'";
            Tsql = Tsql + "  When SellState = 'N_3' Then '" + cm._chang_base_caption_search("교환_정상") + "'";
            Tsql = Tsql + "  When SellState = 'R_1' Then '" + cm._chang_base_caption_search("반품") + "'";
            Tsql = Tsql + "  When SellState = 'R_3' Then '" + cm._chang_base_caption_search("교환_반품") + "'";
            Tsql = Tsql + "  When SellState = 'C_1' Then '" + cm._chang_base_caption_search("취소") + "'";
            Tsql = Tsql + " END  SellStateName ";

            //Tsql = Tsql + " , Ch_T_2." + cls_app_static_var.Base_M_Detail_Ex + " Receive_Method_Name ";
            Tsql = Tsql + " , '' ";
            Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";
            Tsql = Tsql + " ,Isnull(S_Bus.Name,'') as S_B_Name";

            Tsql = Tsql + " ,tbl_SalesItemDetail.Salesitemindex  ";
            Tsql = Tsql + " ,tbl_Memberinfo.BusinessCode  ";
            Tsql = Tsql + " ,tbl_SalesDetail.BusCode  ";


            Tsql = Tsql + " ,Case When Receive_Method = '1' Then '" + cm._chang_base_caption_search("직접수령") + "'";
            Tsql = Tsql + "  When Receive_Method = '2' Then '" + cm._chang_base_caption_search("배송") + "'";
            Tsql = Tsql + "  When Receive_Method = '3' Then '" + cm._chang_base_caption_search("센타수령") + "'";
            Tsql = Tsql + "  When Receive_Method = '4' Then '" + cm._chang_base_caption_search("본사직접수령") + "'";
            Tsql = Tsql + " ELSE '' ";
            Tsql = Tsql + " END  Receive_Method_Name ";

            Tsql = Tsql + " ,Get_ZipCode ";
            Tsql = Tsql + " ,Get_Address1 ";
            Tsql = Tsql + " ,Get_Address2 ";
            Tsql = Tsql + " ,Get_Name1 ";
            Tsql = Tsql + " ,Get_Tel1 ";
            Tsql = Tsql + " ,Get_Tel2 ";

            Tsql = Tsql + " ,tbl_SalesItemDetail.Prom_TF_SORT ";
            Tsql = Tsql + " , tbl_Sales_Rece.Pass_Number ";   
             
            Tsql = Tsql + " From tbl_SalesItemDetail (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_SalesDetail (nolock)  ON tbl_SalesItemDetail.OrderNumber = tbl_SalesDetail.OrderNumber ";
            Tsql = Tsql + " LEFT JOIN tbl_Sales_Rece (nolock)  ON tbl_SalesItemDetail.OrderNumber = tbl_Sales_Rece.OrderNumber And tbl_SalesItemDetail.Salesitemindex = tbl_Sales_Rece.Salesitemindex "; 
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";            
            Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode ";
            Tsql = Tsql + " LEFT JOIN tbl_Business S_Bus (nolock) ON tbl_SalesDetail.BusCode = S_Bus.NCode ";            
            Tsql = Tsql + " Left Join tbl_Class C1 On tbl_Memberinfo.CurGrade=C1.Grade_Cnt ";            
            Tsql = Tsql + " LEFT JOIN tbl_Base_Change_Detail Ch_T_2 (nolock) ON Ch_T_2.M_Detail_S = 'tbl_Sales_Rece' And  Ch_T_2.M_Detail = Convert(Varchar,tbl_Sales_Rece.Receive_Method) ";
            Tsql = Tsql + " LEFT JOIN tbl_Goods (nolock) ON tbl_Goods.Ncode = tbl_SalesitemDetail.ItemCode ";
            Tsql = Tsql + " LEFT Join tbl_SellType ON tbl_SalesDetail.SellCode = tbl_SellType.SellCode ";
        }



        private void Make_Base_Query_(ref string Tsql)
        {

            combo_Rec_Code.SelectedIndex = combo_Rec.SelectedIndex; 

            string strSql = "  Where tbl_SalesItemDetail.ItemCount - tbl_SalesItemDetail.Send_itemCount1 >0 And tbl_SalesDetail.Ga_Order = 0 And tbl_SalesDetail.ReturnTF <> 5   ";

            strSql = strSql + " And  tbl_SalesDetail.Ga_Order = 0 ";
            strSql = strSql + " And  tbl_SalesItemDetail.Check_RecordID = '' ";  //선택하지 않은 내역만 나오게 처리한다.
            

            //if (radioB_SellTF2.Checked == true )
                strSql = strSql + " And tbl_SalesDetail.SellCode <> '' ";  //진주문만 나오게 한다
            //else
            //    strSql = strSql + " And tbl_SalesDetail.SellCode = '' ";  //진주문만 나오게 한다

            ////출고도 하기 전에 반품이 진행 된 내역에 대해서는 안나오게 한다 기본적으로
            ////반품시 Salesitemindex는 기본적으로 원판매의 Salesitemindex 를 가져가고  T_OrderNumber1 필드에 원주문에 대한 주문번호를 넣어둠.
            //strSql = strSql + " And tbl_SalesItemDetail.OrderNumber + '-' + Convert(Varchar, tbl_SalesItemDetail.Salesitemindex) Not in ";
            //strSql = strSql + " (Select T_OrderNumber1 + '-' + Convert(Varchar,Salesitemindex) From tbl_SalesItemDetail Where ItemCount < 0 ) "; //전체 반품 관련

            //strSql = strSql + " And tbl_SalesItemDetail.OrderNumber + '-' + Convert(Varchar,tbl_SalesItemDetail.Salesitemindex)   + '-' + Convert(Varchar,tbl_SalesItemDetail.ItemCount)  Not in ";
            //strSql = strSql + " (Select T_OrderNumber1 + '-' + Convert(Varchar,Real_index) + '-' + Convert(Varchar,-ItemCount)   From tbl_SalesItemDetail Where ItemCount < 0 ) ";   //부분 반품 관련


             string Mbid = ""; int Mbid2 = 0;
            //회원번호1로 검색
            if (
                (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "") 
                &&
                (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() == "") 
                )
            {
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And tbl_SalesDetail.Mbid ='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_SalesDetail.Mbid2 = " + Mbid2;
                }


            }

            //회원번호2로 검색
            if (
                (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
                &&
                (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
                )
            {
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And tbl_SalesDetail.Mbid >='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_SalesDetail.Mbid2 >= " + Mbid2;
                }

                if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And tbl_SalesDetail.Mbid <='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_SalesDetail.Mbid2 <= " + Mbid2;
                }
            }


            //회원명으로 검색
            if (txtName.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.M_Name Like '%" + txtName.Text.Trim() + "%'";

            //가입일자로 검색 -1
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_SalesDetail.SellDate = '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_SalesDetail.SellDate >= '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_SalesDetail.SellDate <= '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";
            }


            ////기록일자로 검색 -1
            //if ((txtMakDate1.Text.Trim() != "") && (txtMakDate2.Text.Trim() == ""))
            //    strSql = strSql + " And Replace(Left( tbl_SalesDetail.recordtime ,10),'-','') = '" + txtMakDate1.Text.Trim() + "'";

            ////기록일자로 검색 -2
            //if ((txtMakDate1.Text.Trim() != "") && (txtMakDate2.Text.Trim() != ""))
            //{
            //    strSql = strSql + " And Replace(Left( tbl_SalesDetail.recordtime ,10),'-','') >= '" + txtMakDate1.Text.Trim() + "'";
            //    strSql = strSql + " And Replace(Left( tbl_SalesDetail.recordtime ,10),'-','') <= '" + txtMakDate2.Text.Trim() + "'";
            //}


            if (txt_ItemName_Code2.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesitemDetail.ItemCode = '" + txt_ItemName_Code2.Text.Trim() + "'";

            //센타코드로으로 검색
            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";

            if (txtCenter2_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.BusCode = '" + txtCenter2_Code.Text.Trim() + "'";


            if (txtSellCode_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.SellCode = '" + txtSellCode_Code.Text.Trim() + "'";

            //if (txtR_Id_Code.Text.Trim() != "")
            //    strSql = strSql + " And tbl_SalesDetail.recordid = '" + txtR_Id_Code.Text.Trim() + "'";


            //if (txtSellCode_Code.Text.Trim() != "")
            //    strSql = strSql + " And tbl_SalesDetail.SellCode = '" + txtSellCode_Code.Text.Trim() + "'";

            if (txtOrderNumber.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.OrderNumber = '" + txtOrderNumber.Text.Trim() + "'";


            if (combo_Rec_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Sales_Rece.Receive_Method = " + combo_Rec_Code.Text.Trim(); 



            if (opt_sell_2.Checked == true)
                strSql = strSql + " And (tbl_SalesitemDetail.SellState = 'N_1' OR tbl_SalesitemDetail.SellState = 'N_3' ) ";

            if (opt_sell_3.Checked == true)
                strSql = strSql + " And (tbl_SalesitemDetail.SellState = 'R_1' OR tbl_SalesitemDetail.SellState = 'R_3' ) ";

            //if (opt_sell_4.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.ReturnTF = 3 ";

            //if (opt_sell_5.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.ReturnTF = 4 ";
            
            //if (opt_Ed_2.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.UnaccMoney = 0 ";

            //if (opt_Ed_3.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.UnaccMoney <> 0 ";


            //if (radioB_SellTF2.Checked == true)
            //{
                //strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
                strSql = strSql + " And tbl_SalesDetail.BusCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";

                //strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";
            //}

            Tsql = Tsql + strSql ;
            Tsql = Tsql + " Order by tbl_SalesDetail.SellDate DESC, tbl_SalesDetail.OrderNumber ";
            Tsql = Tsql + ",tbl_SalesDetail.Mbid, tbl_SalesDetail.Mbid2  ";
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
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name , this.Text,1 ) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            double Sum_14 = 0; double Sum_11 = 0; double Sum_12 = 0;
            double Sum_13 = 0; //double Sum_14 = 0; double Sum_15 = 0;
            //double Sum_16 = 0;
            int OrdCnt = 0;
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            Dictionary<string, string> OrderNum = new Dictionary<string, string>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                Sum_11 = Sum_11 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][10].ToString());
                Sum_12 = Sum_12 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][11].ToString());
                Sum_13 = Sum_13 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][12].ToString());
                Sum_14 = Sum_14 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][13].ToString());

                string T_ver = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                if (OrderNum.ContainsKey(T_ver) != true)
                {
                    OrdCnt++;
                    OrderNum[T_ver] = T_ver;
                }
            }

            if (gr_dic_text.Count > 0)
            {
                txt_P_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_13);
                txt_P_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_14);
                txt_P_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_11);
                txt_P_4.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_12);
                txt_P_5.Text = string.Format(cls_app_static_var.str_Currency_Type, OrdCnt);
                //txt_P_6.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_15);
                //txt_P_7.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_16);        
            }
            
            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();

            //dGridView_Base.Rows[0].Frozen = true;
            //dGridView_Base.la
        }



        private void dGridView_Base_Header_Reset()
        {
            
            cgb.grid_col_Count = 30;            
            cgb.basegrid = dGridView_Base;            
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 4;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            string[] g_HeaderText = {"선택" ,"주문번호"  , "주문_일자"   ,  "회원_번호"   , "성명"      
                                   , "기록일"     , "상품코드"    , "상품명"   , "개별단가"    , "_개별PV" 
                                  ,  "주문_수량" ,"" , "총상품액"    , "_총상품PV" , "주문_종류"   
                                 , "구분"  , "" , "등록_센타명"    , "주문_센타명"      , ""                                 
                                 , ""  , ""  ,"배송구분" ,  "우편번호" ,  "배송지"   
                                 , "수령인명", "연락처1" , "연락처2" , "_Prom_TF_SORT" ,"송장번호"
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 70, 120, 90, 90, 90  
                             , 10,90, 130, 80,00
                             , 80  ,0 , 80, 0, 90
                             , 80  ,10 , 110, 110 , 0                                                          
                             , 0,0 , 10, 10 , 10
                             , 10 , 10, 10 , 0 ,10
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { false , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                    
                                    ,true , true,  true,  true ,true                                    
                                    ,  true,  true ,true      ,true         ,true                             
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight //10

                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleCenter  //15   
                          
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft //20

                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft //25

                               ,DataGridViewContentAlignment.MiddleLeft //25
                               ,DataGridViewContentAlignment.MiddleLeft //25
                               ,DataGridViewContentAlignment.MiddleLeft //25
                               ,DataGridViewContentAlignment.MiddleLeft //25
                               ,DataGridViewContentAlignment.MiddleLeft //25

                      
                              };
            cgb.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            
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


                                ,ds.Tables[base_db_name].Rows[fi_cnt][20]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][21]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][22]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][23]
                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][24].ToString () ) + ' ' + encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][25].ToString () )

                                ,ds.Tables[base_db_name].Rows[fi_cnt][26]
                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][27].ToString () )
                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][28].ToString () )
                                ,ds.Tables[base_db_name].Rows[fi_cnt][29]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][30]
                 
                                 };

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
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter_Code.Text = "";
                Data_Set_Form_TF = 0;
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

            if (tb.Name == "txtCenter3")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter3_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtSellCode")
            {
                if (tb.Text.Trim() == "")
                    txtSellCode_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtSellCode_Code);
            }


            if (tb.Name == "txt_ItemName2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txt_ItemName_Code2.Text = "";
                Data_Set_Form_TF = 0;
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

            if (tb.Name == "txtIO")
            {
                cgb_Pop.Next_Focus_Control = butt_Select;
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
            }
            else
            {
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
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

                if (tb.Name == "txtCenter2" || tb.Name == "txtCenter3")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);
           
                if (tb.Name == "txtSellCode")
                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", strSql);

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
            string Tsql="";
            
            if (tb.Name == "txtCenter")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
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




        private void Base_Sub_Button_Click(object sender, EventArgs e)
        {

            Button bt = (Button)sender;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            
            if (bt.Name == "butt_S_check")
            {
                dGridView_Base.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dGridView_Base.Visible = false;
                for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
                {                   
                    dGridView_Base.Rows[i].Cells[0].Value = "V";                   
                }
                dGridView_Base.Visible = true;
            }


            else if (bt.Name == "butt_S_Not_check")
            {
                dGridView_Base.Visible = false ;
                dGridView_Base.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
                {                    
                     dGridView_Base.Rows[i].Cells[0].Value = "";                    
                }
                dGridView_Base.Visible = true;
            }

            else if (bt.Name == "butt_S_Save")
            {
                int Save_Error_Check = 0;

                prB.Visible = true; butt_S_Save.Enabled = false;
                Save_Base_Data(ref Save_Error_Check);
                prB.Visible = false; butt_S_Save.Enabled = true;

                if (Save_Error_Check > 0)
                {
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                    cgb.d_Grid_view_Header_Reset();
                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<                                        

                    txt_P_1.Text = ""; txt_P_2.Text = ""; txt_P_3.Text = "";
                    txt_P_4.Text = "";// txt_P_5.Text = ""; txt_P_6.Text = "";
                    //txt_P_7.Text = "";
                    opt_1.Checked = true; mtxtOutDate.Text = ""; txtCenter3.Text = ""; txtCenter3_Code.Text = "";
                    chk_Total.Checked = false;

                    tableLayoutPanel6.Visible = false;
                    tableLayoutPanel9.Visible = false;
                    txtCenter3.Text = "";
                    txtCenter3_Code.Text = "";
                    mtxtOutDate.Text = "";


                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    Base_Grid_Set();  //뿌려주는 곳
                    this.Cursor = System.Windows.Forms.Cursors.Default;

                    
                }                
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            

        }



        private Boolean Sub_Check_TextBox_Error()
        {
            cls_Check_Text T_R = new cls_Check_Text();
            string me = "";

            if (opt_2.Checked == true)
            {



                me = T_R.Text_Null_Check(txtCenter3_Code, "Msg_Sort_Stock_Out_Center"); //출고지를
                if (me != "")
                {
                    MessageBox.Show(me);
                    return false;
                }

                me = T_R.Text_Null_Check(mtxtOutDate, "Msg_Sort_Stock_Out_Date"); //출고일자를
                if (me != "")
                {
                    MessageBox.Show(me);
                    return false;
                }

                cls_Search_DB csd = new cls_Search_DB();
                if (csd.Check_Stock_Close(txtCenter3_Code.Text, mtxtOutDate.Text.Replace("-", "").Trim()) == false)
                {
                    txtCenter3.Focus();
                    return false;
                }

                //날짜 관련 텍스트 파일들에 대해서 날짜 오류를 체크한다
                if (Check_TextBox_Error_Date() == false) return false;
            }


            int chk_cnt = 0;
            int Min_SellDate = 99999999;
            Dictionary<string, string> Ncode_dic = new Dictionary<string, string>();

            for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    chk_cnt++;

                    if (opt_2.Checked != true)
                    {
                        string Sell_C_Code = dGridView_Base.Rows[i].Cells[21].Value.ToString();

                        string Out_Date = dGridView_Base.Rows[i].Cells[2].Value.ToString();
                        Out_Date = Out_Date.Replace("-", "");

                        if (int.Parse(Out_Date) < Min_SellDate) Min_SellDate = int.Parse(Out_Date);

                        if (Ncode_dic.ContainsKey(Sell_C_Code) == false)
                            Ncode_dic[Sell_C_Code] = Sell_C_Code;

                        //cls_Search_DB csd = new cls_Search_DB();
                        //if (csd.Check_Stock_Close(Sell_C_Code, Out_Date) == false)
                        //{
                        //    txtCenter3.Focus();
                        //    return false;
                        //}
                    }
                }
            }

            if (chk_cnt == 0) //저장할 내역이 없을을 알린다.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridView_Base.Focus(); return false;
            }

            cls_Search_DB csd2 = new cls_Search_DB();
            foreach (string t_key in Ncode_dic.Keys)
            {
                if (csd2.Check_Stock_Close(t_key, Min_SellDate.ToString()) == false)
                {
                    butt_S_Save.Focus();
                    return false;
                }
            }
            return true;
        }


        private bool Check_TextBox_Error_Date()
        {

            if (mtxtOutDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtOutDate.Text, mtxtOutDate, "Date") == false)
                {
                    mtxtOutDate.Focus();
                    return false;
                }
            }

            ////cls_Check_Input_Error c_er = new cls_Check_Input_Error();
            ////if (txtOutDate.Text.Trim() != "")
            ////{
            ////    int Ret = 0;
            ////    Ret = c_er.Input_Date_Err_Check(txtOutDate);

            ////    if (Ret == -1)
            ////    {
            ////        txtOutDate.Focus(); return false;
            ////    }
            ////}
            return true;
        }




        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
      
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            
            if (Sub_Check_TextBox_Error() == false) return;

            string Out_FL = "001";   //'''---주문출고는 001 임

            Dictionary<string, string> OrderNum = new Dictionary<string, string>();

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            prB.Minimum = 0; prB.Maximum = dGridView_Base.Rows.Count;
            prB.Step = 1; prB.Value = 0;

            try
            {

                string StrSql = ""; string T_Or = ""; string Out_Index = ""; string Sell_C_Code = "";
                int ItemCnt = 0; string ItemCode = ""; int Out_Price = 0; string T_index = "";
                int SalesItemIndex = 0; int Out_Pv = 0; string Out_Date = "";
                int Send_itemCount1 = 0; int itemCount = 0; string Prom_TF_SORT = "";
                int Mbid2 = 0;
                for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                {
                    if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                    {
                        T_Or = dGridView_Base.Rows[i].Cells[1].Value.ToString();
                        SalesItemIndex = int.Parse(dGridView_Base.Rows[i].Cells[19].Value.ToString());
                        //Mbid2 = int.Parse(dGridView_Base.Rows[i].Cells[3].Value.ToString());

                        ItemCode = dGridView_Base.Rows[i].Cells[6].Value.ToString();                         
                        ItemCnt = int.Parse(dGridView_Base.Rows[i].Cells[10].Value.ToString());
                        Sell_C_Code = dGridView_Base.Rows[i].Cells[21].Value.ToString();  


                        Out_Price = int.Parse(dGridView_Base.Rows[i].Cells[8].Value.ToString());
                        Out_Pv = int.Parse(dGridView_Base.Rows[i].Cells[9].Value.ToString());

                        Prom_TF_SORT = dGridView_Base.Rows[i].Cells[28].Value.ToString();  

                        //if (opt_1.Checked == true)
                        //{
                        //    Out_Date = dGridView_Base.Rows[i].Cells[2].Value.ToString();
                        //    Out_Date = Out_Date.Replace("-", "");
                        //}
                        //else
                        //    Out_Date = mtxtOutDate.Text.Replace("-", "").Trim();


                        //StrSql = "Select   ItemCount , Send_itemCount1  ";
                        //StrSql = StrSql + " From tbl_SalesItemDetail (nolock) ";
                        //StrSql = StrSql + " Where OrderNumber ='" + T_Or + "'";
                        //StrSql = StrSql + " And   SalesItemIndex =  " + SalesItemIndex;

                        //DataSet ds = new DataSet();
                        ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                        //if (Temp_Connect.Open_Data_Set(StrSql, "t_P_table", ds) == false) return;
                        //itemCount = int.Parse(ds.Tables["t_P_table"].Rows[0][0].ToString());
                        //Send_itemCount1 = int.Parse (ds.Tables["t_P_table"].Rows[0][1].ToString()) ;

                        //if (Send_itemCount1 + ItemCnt > itemCount)
                        //{
                        //    tran.Rollback();
                        //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Stock_Pre") + "\n" +
                        //    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        //    return;
                        //}


                        //T_index = cls_User.gid + ' ' + DateTime.UtcNow.ToString();

                        //StrSql = "INSERT INTO tbl_Sales_PassNumber ";
                        //StrSql = StrSql + " (Pass_Number2,OrderNumber,SalesItemIndex,User_TF,T_Date) ";
                        //StrSql = StrSql + " Select ";
                        //StrSql = StrSql + "'" + Out_Date.Substring(2, 6);
                        //StrSql = StrSql + "'+ Right('00000' + convert(varchar(8),convert(float,Right( Isnull(Max(Pass_Number2),0),5)) + 1),5)  ";

                        //StrSql = StrSql + ",'" + T_Or + "'," + SalesItemIndex  + ",1,'" + T_index + "'";
                        //StrSql = StrSql + " From tbl_Sales_PassNumber (nolock) ";
                        //StrSql = StrSql + " Where LEFT(Pass_Number2,6) = '" + Out_Date.Substring(2, 6) + "'";

                        //Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);



                        //StrSql = "Select Top 1  Pass_Number2   ";
                        //StrSql = StrSql + " From tbl_Sales_PassNumber (nolock) ";
                        //StrSql = StrSql + " Where  OrderNumber ='" + T_Or + "'";
                        //StrSql = StrSql + " And   SalesItemIndex =" + SalesItemIndex;
                        //StrSql = StrSql + " And   T_Date ='" + T_index + "'";
                        //StrSql = StrSql + " Order by Pass_Number2 DESC ";

                        //ds.Clear();
                        ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                        ////if (Temp_Connect.Open_Data_Set_2(StrSql, "t_P_table", Conn, ds) == false) return;
                        //if (Temp_Connect.Open_Data_Set(StrSql, "t_P_table",  ds) == false) return;
                        ////Out_Index = ds.Tables["t_P_table"].Rows[0][0].ToString();
                        //Out_Index = ds.Tables["t_P_table"].Rows[0]["Pass_Number2"].ToString();

                        
                        //StrSql = "Insert into tbl_StockOutput (";
                        //StrSql = StrSql + " Out_Index,Out_FL, Out_Date  ";
                        //StrSql = StrSql + " , ItemCode ";
                        //StrSql = StrSql + " ,ItemCount";
                        //StrSql = StrSql + " ,Out_Price,Out_PV1, Out_SumPrice,Out_SumPV1 ";
                        //StrSql = StrSql + " , Out_Name ";
                        //StrSql = StrSql + " , Remarks1, Remarks2 ";
                        //StrSql = StrSql + " ,C_Code_FL ,  Out_C_Code ";
                        //StrSql = StrSql + " ,Base_ItemCount, Sell_C_Code ";
                        //StrSql = StrSql + " ,OrderNumber, Salesitemindex ";

                        //StrSql = StrSql + " ,SG_TF, SG_Mbid, SG_Mbid2,Out_FL_Code_2 "; 

                        //StrSql = StrSql + " ,RecordId, RecordTime ";                        
                        //StrSql = StrSql + " )";
                        //StrSql = StrSql + " Values ";
                        //StrSql = StrSql + " (";
                        
                        //StrSql = StrSql + "'" + Out_Index + "'";   //입고번호
                        //StrSql = StrSql + ",'" + Out_FL + "'";   //기타입고 코드 번호
                        //StrSql = StrSql + ",'" + Out_Date + "'";       //상품코드

                        //StrSql = StrSql + ",'" + ItemCode + "'";       //상품코드
                        //StrSql = StrSql + "," + ItemCnt ;      //입고수량
                        //StrSql = StrSql + "," + Out_Price ;       //단위소매가
                        //StrSql = StrSql + "," + Out_Pv ;       //단위소매가


                        //StrSql = StrSql + "," + Out_Price * ItemCnt;      //총입고금액
                        //StrSql = StrSql + "," + Out_Pv * ItemCnt;      //총입고금액

                        //StrSql = StrSql + ",'" + txtR_Id_Code.Text.Trim() + "'";      //입고자
                        //StrSql = StrSql + ",''";       //비고1
                        //StrSql = StrSql + ",''";        //비고2

                        //StrSql = StrSql + ",'C'";   //센타/창고 구분자 c:센타  w:창고

                        //if (opt_1.Checked == true)
                        //    StrSql = StrSql + ",'" + Sell_C_Code + "'";  //센타/창고 코드 번호
                        //else                        
                        //    StrSql = StrSql + ",'" + txtCenter3_Code.Text.Trim() + "'";  //센타/창고 코드 번호

                        //StrSql = StrSql + "," + ItemCnt;      //입고수량
                        //StrSql = StrSql + ",'" + Sell_C_Code + "'";       //상품코드

                        //StrSql = StrSql + ",'" + T_Or + "'";       //상품코드
                        //StrSql = StrSql + "," + SalesItemIndex ;      //입고수량

                        //StrSql = StrSql + " ,0, '', 0 , '' "  ; 

                        //StrSql = StrSql + ",'" + cls_User.gid + "'";
                        //StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";

                        //StrSql = StrSql + ")";
                        
                        //Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                        StrSql = "Update tbl_SalesItemDetail SET ";
                        StrSql = StrSql + " Check_RecordID = '" + cls_User.gid  + "'";
                        StrSql = StrSql + " ,Check_RecordTime = convert(varchar, getdate(),21) ";
                        StrSql = StrSql + " Where OrderNumber ='" + T_Or + "'" ;
                        StrSql = StrSql + " And   SalesItemIndex =  " + SalesItemIndex ;

                        Temp_Connect.Update_Data(StrSql, Conn, tran);
                    }

                    prB.PerformStep();
                }

                tran.Commit();


                //////출고 관련 SMS 를 전송 처리 한다.
                ////foreach (string t_key in OrderNum.Keys)
                ////{
                ////    string Sql = "EXEC Usp_Insert_tbl_Sales_Out_SMS '" + t_key + "'";
                ////    Temp_Connect.Insert_Data(Sql, "tbl_StockOutput", this.Name, this.Text);
                ////}



                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));


            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
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
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtSellDate1);

                opt_Ed_1.Checked = true; opt_sell_2.Checked = true;
                opt_1.Checked = true;

                tableLayoutPanel6.Visible = false;
                tableLayoutPanel9.Visible = false;
                txtCenter3.Text = "";
                txtCenter3_Code.Text = "";
                mtxtOutDate.Text = "";

                radioB_SellTF2.Checked = true; 


                combo_Se.SelectedIndex = -1;
            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                if (Check_TextBox_Error() == false) return;

                txt_P_1.Text = ""; txt_P_2.Text = ""; txt_P_3.Text = "";
                txt_P_4.Text =""; //txt_P_5.Text ="" ;txt_P_6.Text ="";
                //txt_P_7.Text ="";

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex;
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
            Excel_Export_File_Name = this.Text; // "Sell_Item_Select";
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
            ct.Search_Date_TextBox_Put(mtxtSellDate1, mtxtSellDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }




        private void chk_Total_MouseClick(object sender, MouseEventArgs e)
        {
            EventArgs ee = null;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            if (chk_Total.Checked == true)
            {
                Base_Sub_Button_Click(butt_S_check, ee);
            }
            else
                Base_Sub_Button_Click(butt_S_Not_check, ee);


            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.Cursor = System.Windows.Forms.Cursors.Default;

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }


        private Boolean Sub_Check_Print_TextBox_Error()
        {
            cls_Check_Text T_R = new cls_Check_Text();
                    
            int chk_cnt = 0;
            string B_Or = "";
            string StrSql = "";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            StrSql = "Delete From tbl_SalesDetail_Print_T Where Gid ='" + cls_User.gid + "'";

            Temp_Connect.Insert_Data(StrSql, base_db_name);

            for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    chk_cnt++;
                    if (B_Or != dGridView_Base.Rows[i].Cells[1].Value.ToString())
                    {
                        B_Or = dGridView_Base.Rows[i].Cells[1].Value.ToString();
                        P_chk_cnt ++ ;                        
                        
                    }

                    StrSql = "Insert into  tbl_SalesDetail_Print_T Values (" + dGridView_Base.Rows[i].Cells[19].Value.ToString() + ",'" + dGridView_Base.Rows[i].Cells[1].Value.ToString() + "','" + cls_User.gid + "')";

                    Temp_Connect.Insert_Data(StrSql, base_db_name);

                }
            }

            if (chk_cnt == 0) //저장할 내역이 없을을 알린다.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridView_Base.Focus(); return false;
            }

            return true;
        }



        private void button2_Click(object sender, EventArgs e)
        {

        }


        private void butt_Print_Click(int tt)
        {
            print_Page = 0;
            Print_Row = 0;
            P_Ordernumber = "";
            PrintDocument printDocument1 = new PrintDocument();


            printDocument1.BeginPrint += new PrintEventHandler(printDocument1_BeginPrint);
            printDocument1.EndPrint += new PrintEventHandler(printDocument1_EndPrint);                        
            printDocument1.PrintPage += new PrintPageEventHandler(BB_PrintPage);

            Prv_SW = 1;
            MyPrintPreviewDialog dlg = new MyPrintPreviewDialog();
            dlg.Document = printDocument1;
            ((Form)dlg).WindowState = FormWindowState.Maximized;
            dlg.ShowDialog();

        }

        void printDocument1_EndPrint(object sender, PrintEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        void printDocument1_BeginPrint(object sender, PrintEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
        }

        void BB_PrintPage(object sender, PrintPageEventArgs e)
        {
            //if (Prv_SW == 1)
            //{
            //    e.HasMorePages = false;
            //    print_Page = 0;
            //    return ;
            //}

            RectangleF tt = new RectangleF();
            Rectangle t_f = new Rectangle();
            int BaseitemH2 = 0, BaseitemH3 = 0, BaseitemH = 0;
            int Y_tGap = 0;
            int TPrint_Row = 0;

            //if (print_Page != 0)
            //{
            TPrint_Row = Print_Row + 1;

            for (int i = TPrint_Row; i <= dGridView_Base.Rows.Count - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {                    
                    Print_Row = i;
                    if (P_Ordernumber != dGridView_Base.Rows[Print_Row].Cells[1].Value.ToString())                        
                        break;

                }
            }
            //}

            P_Ordernumber = dGridView_Base.Rows[Print_Row].Cells[1].Value.ToString();  //주문번호를 가져온다.



            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();

            string Strsql = "Select tbl_SalesDetail.OrderNumber ,tbl_SalesDetail.Mbid, tbl_SalesDetail.Mbid2 , SellDate ,InsuranceNumber  ";
            Strsql = Strsql + ", tbl_SalesDetail.M_Name ,Total_Sell_VAT_Price ,Total_Sell_Except_VAT_Price , TotalPrice , TotalPV  ";
            Strsql = Strsql + ", InputCard ,InputPassbook,InputPassbook_2, InputCash , TotalInputPrice ";
            Strsql = Strsql + ",Replace(LEFT(tbl_SalesDetail.RecordTime ,10) ,'-','') AS  R_Date ";
            Strsql = Strsql + " From tbl_SalesDetail (nolock) ";            
            Strsql = Strsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_SalesDetail.Mbid = tbl_Memberinfo.Mbid ";
            Strsql = Strsql + " Left Join tbl_Class C1 On tbl_Memberinfo.CurGrade=C1.Grade_Cnt ";
            Strsql = Strsql + " Where tbl_SalesDetail.OrderNumber ='" + P_Ordernumber + "'";

            DataSet ds = new DataSet();
            Temp_Connect.Open_Data_Set(Strsql, "t_table", ds);

            int ReCnt = Temp_Connect.DataSet_ReCount;





            int Last_Line = 0;

            BaseDoc_PrintPage____001(e, ref t_f, ref tt, ref BaseitemH, ref BaseitemH2, ref BaseitemH3, Y_tGap, ds, ref Last_Line, 1);
            BaseDoc_PrintPage____002(e, t_f, BaseitemH2, BaseitemH3, Y_tGap) ; //, ds, ref Last_Line, 1);
            BaseDoc_PrintPage____003(e, t_f, BaseitemH2, BaseitemH3, Y_tGap); //, ds, 1);
            BaseDoc_PrintPage____004(e, t_f, BaseitemH2, BaseitemH3, Y_tGap); //, ds, 1);
            BaseDoc_PrintPage____005(e, t_f, BaseitemH2, BaseitemH3, Y_tGap, ds); //, ds, 1);


            //BaseDoc_PrintPage____001(e, ref t_f, ref tt, ref BaseitemH, ref BaseitemH2, ref BaseitemH3, Y_tGap);
            //BaseDoc_PrintPage____002(e, t_f, BaseitemH2, BaseitemH3, Y_tGap);
            //BaseDoc_PrintPage____003(e, t_f, BaseitemH2, BaseitemH3, Y_tGap);
            //BaseDoc_PrintPage____004(e, t_f, BaseitemH2, BaseitemH3, Y_tGap);
            //BaseDoc_PrintPage____005(e, t_f, BaseitemH2, BaseitemH3, Y_tGap);


            e.HasMorePages = true;
            print_Page++;
            //prB.Value = prB.Value + 1;

            if (print_Page == P_chk_cnt)
            {
                e.HasMorePages = false;
                print_Page = 0;
                Print_Row = 0;

                //.ShowDialog(); //== DialogResult.OK)

            }

        }




        private void BaseDoc_PrintPage____001(System.Drawing.Printing.PrintPageEventArgs e, ref Rectangle t_f, ref RectangleF tt, ref int BaseitemH, ref int BaseitemH2, ref int BaseitemH3, int Y_tGap
            , DataSet ds, ref int Last_Line, int TF)
        {

            string msg = "";
            Pen T_p = new Pen(Color.Black);
            int pageW = e.PageBounds.Width - 20, pageH = e.PageBounds.Height;
            int X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;

            //글자 크기 10을  높이 20으로 잡으면될듯함.
            int plus_g = 0;

            if (Y_tGap > 0)
                plus_g = 35;

            //거래명세표 글자를 찍는다.
            tt.X = (pageW / 2) - 70;
            tt.Y = 25 + Y_tGap - plus_g;
            msg = "거 래 명 세 표";
            FontStyle fs = FontStyle.Bold;
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 18, fs), Brushes.Black, tt);

            tt.X = (pageW / 2) - 70;
            tt.Y = 55 + Y_tGap - plus_g;
            
            msg = "(공급받는자 보관용)";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = 25;
            tt.Y = 75 + Y_tGap - plus_g;
            msg = "판매일자:" + ds.Tables["t_table"].Rows[0]["SellDate"].ToString();
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = (pageW / 2) - 70;
            tt.Y = 75 + Y_tGap - plus_g;
            msg = "공제번호:" + ds.Tables["t_table"].Rows[0]["InsuranceNumber"].ToString();
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            //프린터물 가장 테두리선을 그린다.



            t_f.X = 20;
            if (Y_tGap == 0)
                t_f.Y = 20 + Y_tGap;
            else
                t_f.Y = 20 + Y_tGap - plus_g;

            t_f.Height = ((pageH - (20 * 2)) / 2) - 40;
            t_f.Width = pageW - (t_f.X * 2);
            e.Graphics.DrawRectangle(T_p, t_f);


            // 거래명서표 글자 아래 가로 선을 긋는다 -------
            X1 = t_f.X; X2 = pageW - t_f.X;
            Y1 = t_f.Y + 75; Y2 = t_f.Y + 75;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);



            BaseitemH = t_f.Y + 75;

            int Cnt = 0;
            Cnt = 1;
            while (Cnt <= 3)
            {
                X1 = t_f.X + 20;
                X2 = (pageW / 2) - 10;
                Y1 = BaseitemH + (30 * Cnt);
                Y2 = BaseitemH + (30 * Cnt);
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

                X1 = (pageW / 2) + 10;
                X2 = pageW - t_f.X;
                Y1 = BaseitemH + (30 * Cnt);
                Y2 = BaseitemH + (30 * Cnt);
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

                BaseitemH2 = Y1;
                Cnt++;
            }

            BaseitemH = BaseitemH2;


            int Base_Line = 20, Base_Font_H = 10;

            Cnt = 0;
            while (Cnt <= 17)
            {
                X1 = t_f.X; X2 = pageW - t_f.X;
                Y1 = BaseitemH + (Base_Line * Cnt);
                Y2 = BaseitemH + (Base_Line * Cnt);
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

                BaseitemH3 = BaseitemH + (Base_Line * Cnt);
                Cnt++;
            }
            double Sum_Item_cnt = 0, Sum_ItemPr = 0, Sum_ItemTotalPr = 0;
            int fi_cnt = 3, item_Base_Gap = 4;



            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();

            string Strsql = "Select tbl_SalesItemDetail.itemCode, tbl_Goods.Name AS GGName , tbl_SalesItemDetail.itemCount ,itemPrice ,itemTotalPrice";
            Strsql = Strsql + " From tbl_SalesItemDetail (nolock) ";
            Strsql = Strsql + " LEFT JOIN tbl_Goods (nolock) ON tbl_SalesItemDetail.ItemCode = tbl_Goods.Ncode ";            
            Strsql = Strsql + " Where tbl_SalesItemDetail.OrderNumber ='" + P_Ordernumber + "'";
            Strsql = Strsql + " And SalesItemIndex IN (Select SalesItemIndex From tbl_SalesDetail_Print_T (nolock) Where Gid ='" + cls_User.gid + "' And OrderNumber ='" + P_Ordernumber + "' ) ";
            Strsql = Strsql + " Order by tbl_SalesItemDetail.Salesitemindex ASC ";

            DataSet ds2 = new DataSet();
            Temp_Connect.Open_Data_Set(Strsql, "t_Item_table", ds2);

            int ReCnt = Temp_Connect.DataSet_ReCount;


            //foreach (int t_key in SalesItemDetail.Keys)
            for (int fi_cnt22 = 0; fi_cnt22 <= ReCnt - 1; fi_cnt22++)
            {
                //if (SalesItemDetail[t_key].Del_TF != "D")
                //{
                    tt.X = t_f.X;
                    tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    msg = ds2.Tables["t_Item_table"].Rows[fi_cnt22]["GGName"].ToString();
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


                    tt.X = (pageW / 2);
                    tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    msg = ds2.Tables["t_Item_table"].Rows[fi_cnt22]["itemCount"].ToString();
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
                    Sum_Item_cnt = Sum_Item_cnt + int.Parse(ds2.Tables["t_Item_table"].Rows[fi_cnt22]["itemCount"].ToString());

                    tt.X = (pageW - 320);
                    tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    msg = string.Format(cls_app_static_var.str_Currency_Type, ds2.Tables["t_Item_table"].Rows[fi_cnt22]["itemPrice"]);
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
                    Sum_ItemPr = Sum_ItemPr + double.Parse (ds2.Tables["t_Item_table"].Rows[fi_cnt22]["itemPrice"].ToString ());

                    tt.X = (pageW - 150);
                    tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    msg = string.Format(cls_app_static_var.str_Currency_Type,  ds2.Tables["t_Item_table"].Rows[fi_cnt22]["itemTotalPrice"]);
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
                    Sum_ItemTotalPr = Sum_ItemTotalPr  + double.Parse (ds2.Tables["t_Item_table"].Rows[fi_cnt22]["itemTotalPrice"].ToString ());

                    fi_cnt++;
                //}

            }

            int Base_Font_H_2 = Base_Font_H - 5;

            tt.X = 30; tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = "공급가액";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = 100; tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables["t_table"].Rows[0]["Total_Sell_Except_VAT_Price"]);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = 270; tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = "부가세액";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = 340; tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables["t_table"].Rows[0]["Total_Sell_VAT_Price"]);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = (pageW - 300);
            tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = "합계금액";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = (pageW - 300) + 70;
            tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables["t_table"].Rows[0]["TotalPrice"]);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);



            tt.X = 180;
            tt.Y = BaseitemH2 + (Base_Line) + Base_Font_H_2;
            msg = "품명 및 규격";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = (pageW / 2);
            tt.Y = BaseitemH2 + (Base_Line) + Base_Font_H_2;
            msg = "수량";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = (pageW - 320);
            tt.Y = BaseitemH2 + (Base_Line) + Base_Font_H_2;
            msg = "회원단가";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);


            tt.X = (pageW - 150);
            tt.Y = BaseitemH2 + (Base_Line) + Base_Font_H_2;
            msg = "회원가합";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);



            tt.X = 30; tt.Y = BaseitemH3 + Base_Font_H - 5;
            msg = "인수자";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = 300; tt.Y = BaseitemH3 + Base_Font_H - 5;
            msg = "인";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 9), Brushes.Black, tt);

            tt.X = 360; tt.Y = BaseitemH3 + Base_Font_H - 5;
            msg = "합계";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);



            tt.X = (pageW / 2);
            tt.Y = BaseitemH3 + Base_Font_H - 5;
            msg = string.Format(cls_app_static_var.str_Currency_Type, Sum_Item_cnt);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = (pageW - 320);
            tt.Y = BaseitemH3 + Base_Font_H - 5;
            msg = string.Format(cls_app_static_var.str_Currency_Type, Sum_ItemPr);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = (pageW - 150);
            tt.Y = BaseitemH3 + Base_Font_H - 5;
            msg = string.Format(cls_app_static_var.str_Currency_Type, Sum_ItemTotalPr);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
        }



        private void BaseDoc_PrintPage____002(System.Drawing.Printing.PrintPageEventArgs e, Rectangle t_f, int BaseitemH2, int BaseitemH3, int Y_tGap)
        {
            //RectangleF tt = new RectangleF();

            //string msg = "";
            Pen T_p = new Pen(Color.Black);
            int pageW = e.PageBounds.Width - 20, pageH = e.PageBounds.Height;
            int X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;




            int Base_Line = 20;

            //수량이라는 글자 앞뒤선을 그린다./////////////////////////////////////////
            X1 = (pageW / 2) - 5;
            X2 = X1;
            Y1 = BaseitemH2 + Base_Line;
            Y2 = t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            X1 = (pageW / 2) + 45;
            X2 = X1;
            Y1 = BaseitemH2 + Base_Line;
            Y2 = t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);
            //수량이라는 글자 앞뒤선을 그린다./////////////////////////////////////////


            //회원단가와 회원가합 사이의 선을 그린다.
            X1 = (pageW - 200);
            X2 = X1;
            Y1 = BaseitemH2 + Base_Line;
            Y2 = t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);



            //인수자와 인 사이의 선을 그린다.
            X1 = 90;
            X2 = X1;
            Y1 = BaseitemH3;
            Y2 = t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);



            //인과 합계 사이의 선을 그린다.
            X1 = 340;
            X2 = X1;
            Y1 = BaseitemH3;
            Y2 = t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //공급가액과 부가세액 사이의 선
            X1 = 95;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH2 + Base_Line;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //공급가액과 부가세액 사이의 선
            X1 = 268;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH2 + Base_Line;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //부가세액뒤의선
            X1 = 340;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH2 + Base_Line;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //합계금액앞의선
            X1 = (pageW - 300) - 2;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH2 + Base_Line;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //합계금액뒤의선
            X1 = (pageW - 300) + 70;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH2 + Base_Line;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

        }


        private void BaseDoc_PrintPage____003(System.Drawing.Printing.PrintPageEventArgs e, Rectangle t_f, int BaseitemH2, int BaseitemH3, int Y_tGap)
        {
            RectangleF tt = new RectangleF();

            string msg = "";
            Pen T_p = new Pen(Color.Black);
            int pageW = e.PageBounds.Width - 20, pageH = e.PageBounds.Height;
            int X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;
            //FontStyle fs = FontStyle.Bold;


            int Base_Line = 20, Base_Font_H = 10; //,  BaseitemH = t_f.Y + 75;;

            //공급자 뒷선
            X1 = t_f.X + 20;
            X2 = X1;
            Y1 = t_f.Y + 75;
            Y2 = BaseitemH2;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = t_f.X + 2;
            tt.Y = (t_f.Y + 75) + 10 + Base_Font_H;
            msg = "공";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = t_f.X + 2;
            tt.Y = (t_f.Y + 75) + 25 + Base_Font_H;
            msg = "급";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = t_f.X + 2;
            tt.Y = (t_f.Y + 75) + 40 + Base_Font_H;
            msg = "자";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);




            //공급받는자 앞선
            X1 = (pageW / 2) - 10;
            X2 = X1;
            Y1 = t_f.Y + 75;
            Y2 = BaseitemH2;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            //공급받는자 뒷선
            X1 = (pageW / 2) + 10;
            X2 = X1;
            Y1 = t_f.Y + 75;
            Y2 = BaseitemH2;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = (pageW / 2) - 10 + 2;
            tt.Y = (t_f.Y + 75) + Base_Font_H;
            msg = "공";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = (pageW / 2) - 10 + 2;
            tt.Y = (t_f.Y + 75) + 15 + Base_Font_H;
            msg = "급";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = (pageW / 2) - 10 + 2;
            tt.Y = (t_f.Y + 75) + 30 + Base_Font_H;
            msg = "받";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = (pageW / 2) - 10 + 2;
            tt.Y = (t_f.Y + 75) + 45 + Base_Font_H;
            msg = "는";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = (pageW / 2) - 10 + 2;
            tt.Y = (t_f.Y + 75) + 60 + Base_Font_H;
            msg = "자";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);




        }



        private void BaseDoc_PrintPage____004(System.Drawing.Printing.PrintPageEventArgs e, Rectangle t_f, int BaseitemH2, int BaseitemH3, int Y_tGap)
        {
            RectangleF tt = new RectangleF();

            string msg = "";
            Pen T_p = new Pen(Color.Black);
            int pageW = e.PageBounds.Width - 20, pageH = e.PageBounds.Height;
            int X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;


            int Base_Line = 20, Base_Font_H = 10; //,  BaseitemH = t_f.Y + 75;;


            int Base_W = t_f.X + 20;
            int BaseitemH = t_f.Y + 75;
            //등록번호 뒷선
            X1 = Base_W + 35;
            X2 = X1;
            Y1 = t_f.Y + 75;
            Y2 = BaseitemH2;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = Base_W + 2;
            tt.Y = BaseitemH + 2;
            msg = "등록";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = Base_W + 2;
            tt.Y = BaseitemH + 7 + Base_Font_H;
            msg = "번호";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 36;
            tt.Y = Y1 = BaseitemH + Base_Font_H;
            msg = cls_app_static_var.Dir_Company_Number;  //등록번호
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);




            tt.X = Base_W + 2;
            tt.Y = Y1 = BaseitemH + (30 * 1) + Base_Font_H;
            msg = "상호";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 36;
            tt.Y = Y1 = BaseitemH + (30 * 1) + Base_Font_H;
            msg = cls_app_static_var.Dir_Company_Name;  //상호
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


            tt.X = Base_W + 2;
            tt.Y = Y1 = BaseitemH + (30 * 2) + Base_Font_H;
            msg = "주소";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = Base_W + 36;
            tt.Y = Y1 = BaseitemH + (30 * 2) + Base_Font_H;
            msg = cls_app_static_var.Dir_Company_Address;  //주소
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);



            int Base_W2 = Base_W + 200;


            //대표전화 관련 라인
            X1 = Base_W2 + 35;
            X2 = X1;
            Y1 = BaseitemH;
            Y2 = BaseitemH2 - 60;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            X1 = Base_W2 + 3;
            X2 = X1;
            Y1 = BaseitemH;
            Y2 = BaseitemH2 - 60;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = Base_W2 + 2;
            tt.Y = Y1 = BaseitemH + 2;
            msg = "대표";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);




            tt.X = Base_W2 + 2;
            tt.Y = Y1 = BaseitemH + 7 + Base_Font_H;
            msg = "전화";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W2 + 35;
            tt.Y = Y1 = BaseitemH + Base_Font_H;
            msg = cls_app_static_var.Dir_Company_P_Number;  //전화번호
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);



            int Base_W3 = Base_W + 230;


            //회사전화 관련 라인
            X1 = Base_W3 + 35;
            X2 = X1;
            Y1 = BaseitemH2 - 60;
            Y2 = BaseitemH2 - 30;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            X1 = Base_W3 + 3;
            X2 = X1;
            Y1 = BaseitemH2 - 60;
            Y2 = BaseitemH2 - 30;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = Base_W3 + 2;
            tt.Y = Y1 = BaseitemH + (30 * 1) + Base_Font_H;
            msg = "성명";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W3 + 35;
            tt.Y = Y1 = BaseitemH + (30 * 1) + Base_Font_H;
            msg = cls_app_static_var.Dir_Company_Bos_Name;  //대표자명
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);

        }



        private void BaseDoc_PrintPage____005(System.Drawing.Printing.PrintPageEventArgs e, Rectangle t_f, int BaseitemH2, int BaseitemH3, int Y_tGap,  DataSet ds)
        {
            RectangleF tt = new RectangleF();

            string msg = "";
            Pen T_p = new Pen(Color.Black);
            int pageW = e.PageBounds.Width - 20, pageH = e.PageBounds.Height;
            int X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;


            int Base_Line = 20, Base_Font_H = 10; //,  BaseitemH = t_f.Y + 75;;

            string T_El_Rec = "", BeT_Add = "", T_Add = "";

            int fi_cnt = 0;

             cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();

          
            string strSql = "Select tbl_Sales_Rece.*  ";            
            strSql = strSql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " Receive_Method_Name ";
            strSql = strSql + " From tbl_Sales_Rece (nolock) ";            
            strSql = strSql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_Sales_Rece' And  Ch_T.M_Detail = Convert(Varchar,tbl_Sales_Rece.Receive_Method) ";
            strSql = strSql + " Where OrderNumber ='" + P_Ordernumber + "'";
            strSql = strSql + " Order By SalesItemIndex ASC ";

            DataSet ds2 = new DataSet();
            Temp_Connect.Open_Data_Set(strSql, "t_Item_table", ds2);

            int ReCnt = Temp_Connect.DataSet_ReCount;
            
            //foreach (int t_key in Sales_Rece.Keys)
            for (int fi_cnt22 = 0; fi_cnt22 <= ReCnt - 1; fi_cnt22++)                        
            {
                
                if (BeT_Add == "")
                {
                    BeT_Add = ds2.Tables["t_Item_table"].Rows[fi_cnt22]["Receive_Method_Name"].ToString ();
                    if (ds2.Tables["t_Item_table"].Rows[fi_cnt22]["Receive_Method"].ToString() == "2")
                    {
                        BeT_Add = BeT_Add + "  " + ds2.Tables["t_Item_table"].Rows[fi_cnt22]["Get_ZipCode"].ToString();
                        BeT_Add = BeT_Add + "  " + ds2.Tables["t_Item_table"].Rows[fi_cnt22]["Get_Address1"].ToString();
                        BeT_Add = BeT_Add + "  " + ds2.Tables["t_Item_table"].Rows[fi_cnt22]["Get_Address2"].ToString(); 
                    }
                }
                else
                {
                    T_Add = ds2.Tables["t_Item_table"].Rows[fi_cnt22]["Receive_Method_Name"].ToString();
                    if (ds2.Tables["t_Item_table"].Rows[fi_cnt22]["Receive_Method"].ToString() == "2")
                    {
                        T_Add = T_Add + "  " + ds2.Tables["t_Item_table"].Rows[fi_cnt22]["Get_ZipCode"].ToString();
                        T_Add = T_Add + "  " + encrypter.Decrypt (ds2.Tables["t_Item_table"].Rows[fi_cnt22]["Get_Address1"].ToString());
                        T_Add = T_Add + "  " + encrypter.Decrypt (ds2.Tables["t_Item_table"].Rows[fi_cnt22]["Get_Address2"].ToString()); 
                    }
                }

                if ((BeT_Add != T_Add) && (T_Add != "") && (BeT_Add != ""))
                    BeT_Add = "다중 배송";

                if (ds2.Tables["t_Item_table"].Rows[fi_cnt22]["Receive_Method"].ToString() == "2")
                {

                    if (ds2.Tables["t_Item_table"].Rows[fi_cnt22]["Get_Tel1"].ToString()   != "")
                        T_El_Rec = encrypter.Decrypt (ds2.Tables["t_Item_table"].Rows[fi_cnt22]["Get_Tel1"].ToString());
                }
                
                fi_cnt++;
            }

            string T_El = "";

            string StrSql = "Select hptel,hometel,Address1,Address2 From tbl_Memberinfo  (nolock) ";
            StrSql = StrSql + " Where Mbid  ='" + ds.Tables["t_table"].Rows[0]["Mbid"].ToString()  + "'";
            StrSql = StrSql + " And   Mbid2 =" + ds.Tables["t_table"].Rows[0]["Mbid2"].ToString();

            //++++++++++++++++++++++++++++++++            

            DataSet ds3 = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds3, this.Name, this.Text) == false) return;
            int ReCnt2 = Temp_Connect.DataSet_ReCount;

            T_El = encrypter.Decrypt(ds3.Tables[base_db_name].Rows[0]["Hometel"].ToString());

            if (encrypter.Decrypt(ds3.Tables[base_db_name].Rows[0]["hptel"].ToString()) != "")
                T_El = encrypter.Decrypt(ds3.Tables[base_db_name].Rows[0]["hptel"].ToString());

            if (BeT_Add == "")
                BeT_Add = encrypter.Decrypt(ds3.Tables[base_db_name].Rows[0]["address1"].ToString()) + " " + encrypter.Decrypt(ds3.Tables[base_db_name].Rows[0]["address2"].ToString());

            if (T_El_Rec != "")
                T_El = T_El_Rec;   //배송지 과련 전화번호가 우선이다


            int Base_W = (pageW / 2) + 10;
            int BaseitemH = t_f.Y + 75;
            //등록번호 뒷선
            X1 = Base_W + 35;
            X2 = X1;
            Y1 = BaseitemH;
            Y2 = BaseitemH2;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = Base_W + 2;
            tt.Y = BaseitemH + 2;
            msg = "주문";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = Base_W + 2;
            tt.Y = BaseitemH + 7 + Base_Font_H;
            msg = "번호";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 35;
            tt.Y = BaseitemH + Base_Font_H;

            //if (txt_OrderNumber.Text.Trim() == "")
            msg = P_Ordernumber;   //주문번호
            //else
            //    msg = InsuranceNumber_Ord_Print_FLAG;  //주문번호
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);





            tt.X = Base_W + 2;
            tt.Y = BaseitemH + (30 * 1) + 2;
            msg = "회원";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = Base_W + 2;
            tt.Y = BaseitemH + (30 * 1) + 7 + Base_Font_H;
            msg = "번호";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 35;
            tt.Y = Y1 = BaseitemH + (30 * 1) + Base_Font_H;
            msg = ds.Tables["t_table"].Rows[0]["Mbid2"].ToString();   //회원번호
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


            tt.X = Base_W + 2;
            tt.Y = BaseitemH + (30 * 2) + Base_Font_H;
            msg = "주소";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = Base_W + 35;
            tt.Y = BaseitemH + (30 * 2) + Base_Font_H;
            msg = BeT_Add;   //주소
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 7), Brushes.Black, tt);


            Base_W = Base_W + 200;

            //연락처 성명 관련 라인
            X1 = Base_W + 45;
            X2 = X1;
            Y1 = BaseitemH;
            Y2 = BaseitemH2 - 30;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            X1 = Base_W + 3;
            X2 = X1;
            Y1 = BaseitemH;
            Y2 = BaseitemH2 - 30;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = Base_W + 2;
            tt.Y = Y1 = BaseitemH + Base_Font_H;
            msg = "연락처";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);



            tt.X = Base_W + 45;
            tt.Y = Y1 = BaseitemH + Base_Font_H;
            msg = T_El;   //연락처
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);






            tt.X = Base_W + 2;
            tt.Y = Y1 = BaseitemH + (30 * 1) + Base_Font_H;
            msg = "성명";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 45;
            tt.Y = BaseitemH + (30 * 1) + Base_Font_H;
            msg = ds.Tables["t_table"].Rows[0]["M_Name"].ToString();   //성명
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);

        }

        private void opt_2_MouseClick(object sender, MouseEventArgs e)
        {
            tableLayoutPanel6.Visible = true;
            tableLayoutPanel9.Visible = true;

        }

        private void opt_1_MouseClick(object sender, MouseEventArgs e)
        {
            tableLayoutPanel6.Visible = false;
            tableLayoutPanel9.Visible = false;
            txtCenter3.Text = "";
            txtCenter3_Code.Text = "";
            mtxtOutDate.Text = "";
        }
















    }
}
