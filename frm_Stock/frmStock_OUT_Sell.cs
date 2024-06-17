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
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace MLM_Program
{
    public partial class frmStock_OUT_Sell : clsForm_Extends
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

        public delegate void SendNumberDele(string Send_Number, string Send_Name, string Send_OrderNumber);
        public event SendNumberDele Send_Sell_Number;

        public delegate void Send_Mem_NumberDele(string Send_Number, string Send_Name);
        public event Send_Mem_NumberDele Send_Mem_Number;



        public frmStock_OUT_Sell()
        {
            InitializeComponent();

            DoubleBuffered = true;
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.SetProperty, null, dGridView_Base, new object[] { true });

            //typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance
            //| BindingFlags.SetProperty, null, dGridView_Base_Sub, new object[] { true });
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
            this.Refresh();
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
            cfm.button_flat_change(btnFastReport_Show);

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

            StringBuilder sb = new StringBuilder();
            sb.Append("Select ''  ");
            sb.AppendLine(" , tbl_SalesItemDetail.Check_RecordTime ");
            sb.AppendLine(" , tbl_SalesDetail.OrderNumber  ");

            sb.AppendLine(" ,LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2)   ");

            if (cls_app_static_var.Member_Number_1 > 0)
                sb.AppendLine(",Case When tbl_SalesDetail.SellCode <> '' Then  tbl_SalesDetail.mbid + '-' + Convert(Varchar,tbl_SalesDetail.mbid2) ELSE  tbl_SalesDetail.mbid End ");
            else
                sb.AppendLine(",Case When tbl_SalesDetail.SellCode <> '' Then  Convert(varchar,tbl_SalesDetail.mbid2)  ELSE  tbl_SalesDetail.mbid End ");

            sb.AppendLine(" ,tbl_SalesDetail.M_Name ");

            sb.AppendLine(", tbl_SalesItemDetail.RecordTime ");

            sb.AppendLine(" , tbl_SalesItemDetail.ItemCode ");


            // 한국인 경우
            if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
            {
                sb.AppendLine(" , tbl_Goods.Name Item_Name ");
            }
            // 태국인 경우
            else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
            {
                sb.AppendLine(" , tbl_Goods.Name_e  Item_Name ");
            }

            sb.AppendLine(" , tbl_SalesItemDetail.ItemPrice ");
            sb.AppendLine(" , tbl_SalesItemDetail.ItemPV ");

            sb.AppendLine(" , tbl_SalesItemDetail.ItemCount ");
            sb.AppendLine(" , tbl_SalesItemDetail.Send_itemCount1 ");

            sb.AppendLine(" , tbl_SalesItemDetail.ItemTotalPrice ");
            sb.AppendLine(" , tbl_SalesItemDetail.ItemTotalPV ");

            // 한국인 경우
            if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
            {
                sb.AppendLine(" , Isnull ( tbl_SellType.SellTypeName , 'Regular_order' )  SellCodeName  ");
            }
            // 태국인 경우
            else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
            {
                sb.AppendLine(" , Isnull ( tbl_SellType.SellTypeName_en , 'Regular_order' )  SellCodeName  ");
            }



            cls_form_Meth cm = new cls_form_Meth();
            sb.AppendLine(" ,Case When SellState = 'N_1' Then '" + cm._chang_base_caption_search("정상") + "'");
            sb.AppendLine("  When SellState = 'N_3' Then '" + cm._chang_base_caption_search("교환_정상") + "'");
            sb.AppendLine("  When SellState = 'R_1' Then '" + cm._chang_base_caption_search("반품") + "'");
            sb.AppendLine("  When SellState = 'R_3' Then '" + cm._chang_base_caption_search("교환_반품") + "'");
            sb.AppendLine("  When SellState = 'C_1' Then '" + cm._chang_base_caption_search("취소") + "'");
            sb.AppendLine(" END  SellStateName ");

            //sb.AppendLine( " , Ch_T_2." + cls_app_static_var.Base_M_Detail_Ex + " Receive_Method_Name ");
            sb.AppendLine(" , '' ");
            sb.AppendLine(" ,Isnull(tbl_Business.Name,'') as B_Name");
            sb.AppendLine(" ,Isnull(S_Bus.Name,'') as S_B_Name");

            sb.AppendLine(" ,tbl_SalesItemDetail.Salesitemindex  ");
            sb.AppendLine(" ,tbl_Memberinfo.BusinessCode  ");
            sb.AppendLine(" ,tbl_SalesDetail.BusCode  ");


            sb.AppendLine(" ,Case When Receive_Method = '1' Then '" + cm._chang_base_caption_search("직접수령") + "'");
            sb.AppendLine("  When Receive_Method = '2' Then '" + cm._chang_base_caption_search("배송") + "'");
            sb.AppendLine("  When Receive_Method = '3' Then '" + cm._chang_base_caption_search("센타수령") + "'");
            sb.AppendLine("  When Receive_Method = '4' Then '" + cm._chang_base_caption_search("본사직접수령") + "'");
            sb.AppendLine(" ELSE '' ");
            sb.AppendLine(" END  Receive_Method_Name ");

            sb.AppendLine(" ,Get_ZipCode ");
            sb.AppendLine(" ,Get_city ");
            sb.AppendLine(" ,Get_state ");
            sb.AppendLine(" ,Get_Address1 ");
            sb.AppendLine(" ,Get_Address2 ");
            sb.AppendLine(" ,Get_Name1 ");
            sb.AppendLine(" ,Get_Tel1 ");
            sb.AppendLine(" ,Get_Tel2 ");

            sb.AppendLine(" ,tbl_SalesItemDetail.Prom_TF_SORT ");
            sb.AppendLine(" , tbl_Sales_Rece.Pass_Number ");
            sb.AppendLine(" ,tbl_SalesDetail.InputCard ");
            sb.AppendLine(" ,tbl_SalesDetail.InputCash ");
            sb.AppendLine(" ,tbl_SalesDetail.InputPassbook + tbl_SalesDetail.InputPassbook_2 as InputPassbook_2 ");
            sb.AppendLine(" ,tbl_SalesDetail.InsuranceNumber ");
            sb.AppendLine(" ,tbl_SalesDetail.InputPass_Pay ");


            sb.AppendLine(" From tbl_SalesItemDetail (nolock) ");
            sb.AppendLine(" LEFT JOIN tbl_SalesDetail (nolock)  ON tbl_SalesItemDetail.OrderNumber = tbl_SalesDetail.OrderNumber ");
            sb.AppendLine("  LEFT JOIN(");
            sb.AppendLine("     SELECT OrderNumber, Min(SalesItemIndex) SalesItemIndex, Receive_Method , Get_ZipCode , Get_Address1 , Get_Address2 , Get_Name1 , Get_Tel1 , Get_Tel2 , Pass_Number, Get_city, Get_state");
            sb.AppendLine("     FROM tbl_Sales_Rece (nolock)");
            sb.AppendLine("     GROUP BY OrderNumber, Receive_Method , Get_ZipCode  , Get_Address1  , Get_Address2 , Get_Name1 , Get_Tel1 , Get_Tel2 , Pass_Number, Get_city, Get_state");
            sb.AppendLine("     ) tbl_Sales_Rece ON tbl_SalesItemDetail.OrderNumber = tbl_Sales_Rece.OrderNumber");
            sb.AppendLine(" LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ");
            sb.AppendLine(" LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode ");
            sb.AppendLine(" LEFT JOIN tbl_Business S_Bus (nolock) ON tbl_SalesDetail.BusCode = S_Bus.NCode ");
            sb.AppendLine(" Left Join tbl_Class C1 On tbl_Memberinfo.CurGrade=C1.Grade_Cnt ");
            sb.AppendLine(" LEFT JOIN tbl_Base_Change_Detail Ch_T_2 (nolock) ON Ch_T_2.M_Detail_S = 'tbl_Sales_Rece' And  Ch_T_2.M_Detail = Convert(Varchar,tbl_Sales_Rece.Receive_Method) ");
            sb.AppendLine(" LEFT JOIN tbl_Goods (nolock) ON tbl_Goods.Ncode = tbl_SalesitemDetail.ItemCode ");
            sb.AppendLine(" LEFT Join tbl_SellType ON tbl_SalesDetail.SellCode = tbl_SellType.SellCode ");

            sb.AppendLine(" LEFT JOIN (Select T_OrderNumber1 ,Salesitemindex From tbl_SalesItemDetail (nolock) Where ItemCount < 0) RR_1 ");
            sb.AppendLine("     ON RR_1.T_OrderNumber1 = tbl_SalesItemDetail.OrderNumber    And RR_1.Salesitemindex = tbl_SalesItemDetail.Salesitemindex  ");

            sb.AppendLine(" LEFT JOIN (Select T_OrderNumber1 ,Real_index ,-ItemCount  ItemCount   From tbl_SalesItemDetail (nolock) Where ItemCount < 0) RR_2 ");
            sb.AppendLine("      ON RR_2.T_OrderNumber1 = tbl_SalesItemDetail.OrderNumber    And RR_2.Real_index = tbl_SalesItemDetail.Salesitemindex  ");
            sb.AppendLine("      And RR_2.ItemCount = tbl_SalesItemDetail.ItemCount  ");


            Tsql = sb.ToString();
        }



        private void Make_Base_Query_(ref string Tsql)
        {

            combo_Rec_Code.SelectedIndex = combo_Rec.SelectedIndex;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("  Where tbl_SalesItemDetail.ItemCount - tbl_SalesItemDetail.Send_itemCount1 >0 And tbl_SalesDetail.Ga_Order = 0 And tbl_SalesDetail.ReturnTF <> 5   ");

            sb.AppendLine(" And  tbl_SalesDetail.Ga_Order = 0 ");

            //sb.AppendLine("  And  tbl_SalesDetail.TotalinputPrice > 0 ");
            sb.AppendLine("  And  (tbl_SalesDetail.TotalinputPrice > 0 OR tbl_SalesDetail.TotalPv > 0 ) ");



            if (radioB_SellTF2.Checked == true)
                sb.AppendLine(" And tbl_SalesDetail.SellCode <> '' ");  //진주문만 나오게 한다
            else
                sb.AppendLine(" And tbl_SalesDetail.SellCode = '' ");  //진주문만 나오게 한다

            //출고도 하기 전에 반품이 진행 된 내역에 대해서는 안나오게 한다 기본적으로
            //반품시 Salesitemindex는 기본적으로 원판매의 Salesitemindex 를 가져가고  T_OrderNumber1 필드에 원주문에 대한 주문번호를 넣어둠.
            sb.AppendLine(" And  RR_1.T_OrderNumber1 IS null ");
            sb.AppendLine(" And  RR_2.T_OrderNumber1 IS null ");

            //sb.AppendLine( " And tbl_SalesItemDetail.OrderNumber + '-' + Convert(Varchar, tbl_SalesItemDetail.Salesitemindex) Not in ");
            //sb.AppendLine( " (Select T_OrderNumber1 + '-' + Convert(Varchar,Salesitemindex) From tbl_SalesItemDetail Where ItemCount < 0 ) "); //전체 반품 관련

            //sb.AppendLine( " And tbl_SalesItemDetail.OrderNumber + '-' + Convert(Varchar,tbl_SalesItemDetail.Salesitemindex)   + '-' + Convert(Varchar,tbl_SalesItemDetail.ItemCount)  Not in ");
            //sb.AppendLine( " (Select T_OrderNumber1 + '-' + Convert(Varchar,Real_index) + '-' + Convert(Varchar,-ItemCount)   From tbl_SalesItemDetail Where ItemCount < 0 ) ");   //부분 반품 관련


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
                        sb.AppendLine(" And tbl_SalesDetail.Mbid ='" + Mbid + "'");

                    if (Mbid2 >= 0)
                        sb.AppendLine(" And tbl_SalesDetail.Mbid2 = " + Mbid2);
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
                        sb.AppendLine(" And tbl_SalesDetail.Mbid >='" + Mbid + "'");

                    if (Mbid2 >= 0)
                        sb.AppendLine(" And tbl_SalesDetail.Mbid2 >= " + Mbid2);
                }

                if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        sb.AppendLine(" And tbl_SalesDetail.Mbid <='" + Mbid + "'");

                    if (Mbid2 >= 0)
                        sb.AppendLine(" And tbl_SalesDetail.Mbid2 <= " + Mbid2);
                }
            }


            //회원명으로 검색
            if (txtName.Text.Trim() != "")
                sb.AppendLine(" And tbl_SalesDetail.M_Name Like '%" + txtName.Text.Trim() + "%'");

            //가입일자로 검색 -1
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() == ""))
                sb.AppendLine(" And tbl_SalesDetail.SellDate = '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'");

            //가입일자로 검색 -2
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() != ""))
            {
                sb.AppendLine(" And tbl_SalesDetail.SellDate >= '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'");
                sb.AppendLine(" And tbl_SalesDetail.SellDate <= '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'");
            }


            ////기록일자로 검색 -1
            //if ((txtMakDate1.Text.Trim() != "") && (txtMakDate2.Text.Trim() == ""))
            //    sb.AppendLine( " And Replace(Left( tbl_SalesDetail.recordtime ,10),'-','') = '" + txtMakDate1.Text.Trim() + "'");

            ////기록일자로 검색 -2
            //if ((txtMakDate1.Text.Trim() != "") && (txtMakDate2.Text.Trim() != ""))
            //{
            //    sb.AppendLine( " And Replace(Left( tbl_SalesDetail.recordtime ,10),'-','') >= '" + txtMakDate1.Text.Trim() + "'");
            //    sb.AppendLine( " And Replace(Left( tbl_SalesDetail.recordtime ,10),'-','') <= '" + txtMakDate2.Text.Trim() + "'");
            //}


            if (txt_ItemName_Code2.Text.Trim() != "")
                sb.AppendLine(" And tbl_SalesitemDetail.ItemCode = '" + txt_ItemName_Code2.Text.Trim() + "'");

            //센타코드로으로 검색
            if (txtCenter_Code.Text.Trim() != "")
                sb.AppendLine(" And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'");

            if (txtCenter2_Code.Text.Trim() != "")
                sb.AppendLine(" And tbl_SalesDetail.BusCode = '" + txtCenter2_Code.Text.Trim() + "'");


            if (txtSellCode_Code.Text.Trim() != "")
                sb.AppendLine(" And tbl_SalesDetail.SellCode = '" + txtSellCode_Code.Text.Trim() + "'");

            //if (txtR_Id_Code.Text.Trim() != "")
            //    sb.AppendLine( " And tbl_SalesDetail.recordid = '" + txtR_Id_Code.Text.Trim() + "'");


            //if (txtSellCode_Code.Text.Trim() != "")
            //    sb.AppendLine( " And tbl_SalesDetail.SellCode = '" + txtSellCode_Code.Text.Trim() + "'");

            if (txtOrderNumber.Text.Trim() != "")
                sb.AppendLine(" And tbl_SalesDetail.OrderNumber = '" + txtOrderNumber.Text.Trim() + "'");


            if (combo_Rec_Code.Text.Trim() != "")
                sb.AppendLine(" And tbl_Sales_Rece.Receive_Method = " + combo_Rec_Code.Text.Trim());



            if (opt_sell_2.Checked == true)
                sb.AppendLine(" And (tbl_SalesitemDetail.SellState = 'N_1' OR tbl_SalesitemDetail.SellState = 'N_3' ) ");

            if (opt_sell_3.Checked == true)
                sb.AppendLine(" And (tbl_SalesitemDetail.SellState = 'R_1' OR tbl_SalesitemDetail.SellState = 'R_3' ) ");

            //if (opt_sell_4.Checked == true)
            //    sb.AppendLine( " And tbl_SalesDetail.ReturnTF = 3 ");

            //if (opt_sell_5.Checked == true)
            //    sb.AppendLine( " And tbl_SalesDetail.ReturnTF = 4 ");

            //if (opt_Ed_2.Checked == true)
            //    sb.AppendLine( " And tbl_SalesDetail.UnaccMoney = 0 ");

            //if (opt_Ed_3.Checked == true)
            //    sb.AppendLine( " And tbl_SalesDetail.UnaccMoney <> 0 ");


            //if (radioB_SellTF2.Checked == true)
            //{
            //sb.AppendLine( " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )");
            sb.AppendLine(" And tbl_SalesDetail.BusCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )");

            //sb.AppendLine( " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )");
            //}

            Tsql = Tsql + sb.ToString();

            cls_NationService.SQL_Memberinfo_NationCode(ref Tsql);

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
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text, 1) == false) return;
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

                Sum_11 = Sum_11 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][11].ToString());
                Sum_12 = Sum_12 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][12].ToString());
                Sum_13 = Sum_13 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][13].ToString());
                Sum_14 = Sum_14 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][14].ToString());

                string T_ver = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                if (OrderNum.ContainsKey(T_ver) != true)
                {
                    OrdCnt++;
                    OrderNum[T_ver] = T_ver;
                }
            }

            //if (gr_dic_text.Count > 0)
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

            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 4;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            string[] g_HeaderText = {
        "선택"     , ""           , "주문번호"    ,   "구매_일자"     , "회원_번호"
      , "성명"     , "기록일"     , "상품코드"    ,   "상품명"        , "개별단가"
      , "_개별PV"  , "구매_수량"  , ""            ,   "총상품액"      , "_총상품PV"
      , "구매_종류", "구분"       , ""            ,   "_등록_센타명"   , "_구매_센타명"
      , ""         , ""           , ""            ,   "배송구분"      , "우편번호"
      , "태국_도시", "태국_주", "배송지"   , "수령인명"   , "연락처1"
      , "연락처2"  , "_Prom_TF_SORT" , "송장번호" , "카드" , "현금"
      , "가상계좌" , "공제번호", "배송료"  ,

                                    };
            string[] g_Cols = {
        "Selected"   , ""           , "OrderNumber"    , "SellDate"          ,  "mbid2"
      , "m_name"     , "RegDatee"   , "Itemcode"       , "ItemName"          , "ItemPrice"
      , "_개별PV"    , "ItemCount"  , ""               , "TotalItemPrice"    , "_총상품PV"
      , "SellType1"  , "SellType2"  , ""               , "CenterName"        , "OrderCenterName"
      , ""           , ""           , ""               , "배송구분"          ,  "우편번호"
      , "City", "State",  "배송지"    , "수령인명"   , "연락처1"
      , "연락처2"           , "_Prom_TF_SORT" , "송장번호"    , "InputCard"  , "InputCash"
      , "InputPassbook_2"   , "InsuranceNumber", "InputPass_pay"
            };

            cgb.grid_col_Count = g_HeaderText.Length;
            cgb.grid_col_header_text = g_HeaderText;
            cgb.grid_col_name = g_Cols;

            int[] g_Width = {
         70,   0, 120,  90,  90
      ,  90,  10,  90, 130,  80
      ,   0,  80,   0,  80,   0
      ,  90,  80,  0, 0, 0
      ,   0,   0,   0,  10,  10
      ,  50, 50
      ,  10,  10,  10,  10,   0
      ,  10,  50,  50,  50,  50
      ,  50
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { false , true,  true,  true ,true
                                    , true , true,  true,  true ,true
                                    , true , true,  true,  true ,true
                                    , true , true,  true,  true ,true
                                    , true , true,  true,  true ,true
                                    , true , true,  true,  true ,true
                                    , true , true,  true,  true ,true
                                    ,true, true, true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter      //5 

                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight      //10

                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight    //15  

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft       //20

                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft     //25

                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft     //30

                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight    //35

                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft

                              };
            cgb.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            gr_dic_cell_format[33] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[34] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[35] = cls_app_static_var.str_Grid_Currency_Type;

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
                                ,ds.Tables[base_db_name].Rows[fi_cnt][24]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][25]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][26]
                                ,(ds.Tables[base_db_name].Rows[fi_cnt][27].ToString () ) + ' ' +  (ds.Tables[base_db_name].Rows[fi_cnt][28].ToString () )
                                ,ds.Tables[base_db_name].Rows[fi_cnt][29]

                                , (ds.Tables[base_db_name].Rows[fi_cnt][30].ToString () )
                                , (ds.Tables[base_db_name].Rows[fi_cnt][31].ToString () )

                                //,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][25].ToString () ) + ' ' + encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][26].ToString () )
                                //,ds.Tables[base_db_name].Rows[fi_cnt][27]
                                //,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][28].ToString () )
                                //,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][29].ToString () )
                                                                

                                ,ds.Tables[base_db_name].Rows[fi_cnt][32]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][33]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][34]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][35]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][36]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][37]

                                  ,ds.Tables[base_db_name].Rows[fi_cnt][38]
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

            if ((sender is TextBox) == false) return;

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
            cgb_Pop.Base_tb_2 = tb;    //2번은 명임
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
                    cgb_Pop.db_grid_Popup_Base(2, "구매_코드", "구매종류", "SellCode", "SellTypeName", strSql);

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
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
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
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                }

                if (tb.Name == "txtSellCode")
                {
                    string Tsql;
                    Tsql = "Select SellCode ,SellTypeName    ";
                    Tsql = Tsql + " From tbl_SellType (nolock) ";
                    Tsql = Tsql + " Order by SellCode ";

                    cgb_Pop.db_grid_Popup_Base(2, "구매_코드", "구매종류", "SellCode", "SellTypeName", Tsql);
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
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
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
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
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
                dGridView_Base.Visible = false;
                for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
                {
                    dGridView_Base.Rows[i].Cells[0].Value = "V";
                }
                dGridView_Base.Visible = true;
            }


            else if (bt.Name == "butt_S_Not_check")
            {
                dGridView_Base.Visible = false;
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
                        string Sell_C_Code = dGridView_Base.Rows[i].Cells[22].Value.ToString();

                        string Out_Date = dGridView_Base.Rows[i].Cells[3].Value.ToString();
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
                        T_Or = dGridView_Base.Rows[i].Cells[2].Value.ToString();
                        SalesItemIndex = int.Parse(dGridView_Base.Rows[i].Cells[20].Value.ToString());
                        //Mbid2 = int.Parse(dGridView_Base.Rows[i].Cells[3].Value.ToString());

                        ItemCode = dGridView_Base.Rows[i].Cells[7].Value.ToString();
                        ItemCnt = int.Parse(dGridView_Base.Rows[i].Cells[11].Value.ToString());
                        Sell_C_Code = dGridView_Base.Rows[i].Cells[22].Value.ToString();


                        Out_Price = int.Parse(dGridView_Base.Rows[i].Cells[9].Value.ToString());
                        Out_Pv = int.Parse(dGridView_Base.Rows[i].Cells[10].Value.ToString());

                        Prom_TF_SORT = dGridView_Base.Rows[i].Cells[29].Value.ToString();

                        if (opt_1.Checked == true)
                        {
                            Out_Date = dGridView_Base.Rows[i].Cells[3].Value.ToString();
                            Out_Date = Out_Date.Replace("-", "");
                        }
                        else
                            Out_Date = mtxtOutDate.Text.Replace("-", "").Trim();


                        StrSql = "Select   ItemCount , Send_itemCount1  ";
                        StrSql = StrSql + " From tbl_SalesItemDetail (nolock) ";
                        StrSql = StrSql + " Where OrderNumber ='" + T_Or + "'";
                        StrSql = StrSql + " And   SalesItemIndex =  " + SalesItemIndex;

                        DataSet ds = new DataSet();
                        //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                        if (Temp_Connect.Open_Data_Set(StrSql, "t_P_table", ds) == false) return;
                        itemCount = int.Parse(ds.Tables["t_P_table"].Rows[0][0].ToString());
                        Send_itemCount1 = int.Parse(ds.Tables["t_P_table"].Rows[0][1].ToString());

                        if (Send_itemCount1 + ItemCnt > itemCount)
                        {

                            string Err_Mbid2 = dGridView_Base.Rows[i].Cells[4].Value.ToString();
                            string Err_M_Name = dGridView_Base.Rows[i].Cells[5].Value.ToString();

                            tran.Rollback();
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Stock_Pre") + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                            string Err_Order = "주문번호 : " + T_Or + "\n";
                            Err_Order = Err_Order + "회원번호 : " + Err_Mbid2 + "\n";
                            Err_Order = Err_Order + "성명 : " + Err_M_Name + "\n";
                            Err_Order = Err_Order + "의 배송정보가 동일하지 않습니다.(주소,연락처,우편번호) 동일하게 처리후 다시 시도해 주십시요";
                            MessageBox.Show(Err_Order);



                            return;
                        }


                        T_index = cls_User.gid + ' ' + DateTime.UtcNow.ToString();

                        StrSql = "INSERT INTO tbl_Sales_PassNumber ";
                        StrSql = StrSql + " (Pass_Number2,OrderNumber,SalesItemIndex,User_TF,T_Date) ";
                        StrSql = StrSql + " Select ";
                        StrSql = StrSql + "'" + Out_Date.Substring(2, 6);
                        StrSql = StrSql + "'+ Right('00000' + convert(varchar(8),convert(float,Right( Isnull(Max(Pass_Number2),0),5)) + 1),5)  ";

                        StrSql = StrSql + ",'" + T_Or + "'," + SalesItemIndex + ",1,'" + T_index + "'";
                        StrSql = StrSql + " From tbl_Sales_PassNumber (nolock) ";
                        StrSql = StrSql + " Where LEFT(Pass_Number2,6) = '" + Out_Date.Substring(2, 6) + "'";

                        Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);



                        StrSql = "Select Top 1  Pass_Number2   ";
                        StrSql = StrSql + " From tbl_Sales_PassNumber (nolock) ";
                        StrSql = StrSql + " Where  OrderNumber ='" + T_Or + "'";
                        StrSql = StrSql + " And   SalesItemIndex =" + SalesItemIndex;
                        StrSql = StrSql + " And   T_Date ='" + T_index + "'";
                        StrSql = StrSql + " Order by Pass_Number2 DESC ";

                        ds.Clear();
                        //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                        //if (Temp_Connect.Open_Data_Set_2(StrSql, "t_P_table", Conn, ds) == false) return;
                        if (Temp_Connect.Open_Data_Set(StrSql, "t_P_table", ds) == false) return;
                        //Out_Index = ds.Tables["t_P_table"].Rows[0][0].ToString();
                        Out_Index = ds.Tables["t_P_table"].Rows[0]["Pass_Number2"].ToString();


                        StrSql = "Insert into tbl_StockOutput (";
                        StrSql = StrSql + " Out_Index,Out_FL, Out_Date  ";
                        StrSql = StrSql + " , ItemCode ";
                        StrSql = StrSql + " ,ItemCount";
                        StrSql = StrSql + " ,Out_Price,Out_PV1, Out_SumPrice,Out_SumPV1 ";
                        StrSql = StrSql + " , Out_Name ";
                        StrSql = StrSql + " , Remarks1, Remarks2 ";
                        StrSql = StrSql + " ,C_Code_FL ,  Out_C_Code ";
                        StrSql = StrSql + " ,Base_ItemCount, Sell_C_Code ";
                        StrSql = StrSql + " ,OrderNumber, Salesitemindex ";

                        StrSql = StrSql + " ,SG_TF, SG_Mbid, SG_Mbid2,Out_FL_Code_2 ";

                        StrSql = StrSql + " ,RecordId, RecordTime ";
                        StrSql = StrSql + " )";
                        StrSql = StrSql + " Values ";
                        StrSql = StrSql + " (";

                        StrSql = StrSql + "'" + Out_Index + "'";   //입고번호
                        StrSql = StrSql + ",'" + Out_FL + "'";   //기타입고 코드 번호
                        StrSql = StrSql + ",'" + Out_Date + "'";       //상품코드

                        StrSql = StrSql + ",'" + ItemCode + "'";       //상품코드
                        StrSql = StrSql + "," + ItemCnt;      //입고수량
                        StrSql = StrSql + "," + Out_Price;       //단위소매가
                        StrSql = StrSql + "," + Out_Pv;       //단위소매가


                        StrSql = StrSql + "," + Out_Price * ItemCnt;      //총입고금액
                        StrSql = StrSql + "," + Out_Pv * ItemCnt;      //총입고금액

                        StrSql = StrSql + ",'" + txtR_Id_Code.Text.Trim() + "'";      //입고자
                        StrSql = StrSql + ",''";       //비고1
                        StrSql = StrSql + ",''";        //비고2

                        StrSql = StrSql + ",'C'";   //센타/창고 구분자 c:센타  w:창고

                        if (opt_1.Checked == true)
                            StrSql = StrSql + ",'" + Sell_C_Code + "'";  //센타/창고 코드 번호
                        else
                            StrSql = StrSql + ",'" + txtCenter3_Code.Text.Trim() + "'";  //센타/창고 코드 번호

                        StrSql = StrSql + "," + ItemCnt;      //입고수량
                        StrSql = StrSql + ",'" + Sell_C_Code + "'";       //상품코드

                        StrSql = StrSql + ",'" + T_Or + "'";       //상품코드
                        StrSql = StrSql + "," + SalesItemIndex;      //입고수량

                        StrSql = StrSql + " ,0, '', 0 , '' ";

                        StrSql = StrSql + ",'" + cls_User.gid + "'";
                        StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";

                        StrSql = StrSql + ")";

                        Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                        StrSql = "Update tbl_SalesItemDetail SET ";
                        StrSql = StrSql + " Send_itemCount1 = Send_itemCount1 + " + ItemCnt;
                        StrSql = StrSql + " Where OrderNumber ='" + T_Or + "'";
                        StrSql = StrSql + " And   SalesItemIndex =  " + SalesItemIndex;

                        Temp_Connect.Update_Data(StrSql, Conn, tran);



                        if (dGridView_Base.Rows[i].Cells[23].Value.ToString() == "배송")
                        {
                            if (OrderNum.ContainsKey(T_Or) == false)
                                OrderNum[T_Or] = T_Or;
                        }

                    }

                    prB.PerformStep();
                }

                tran.Commit();


                ////출고 관련 SMS 를 전송 처리 한다.
                //foreach (string t_key in OrderNum.Keys)
                //{
                //    string Sql = "EXEC Usp_Insert_tbl_Sales_Out_SMS '" + t_key + "'";
                //    Temp_Connect.Insert_Data(Sql, "tbl_StockOutput", this.Name, this.Text);
                //}



                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));

                //GridViewExcel(dGridView_Base);

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
                txt_P_4.Text = ""; //txt_P_5.Text ="" ;txt_P_6.Text ="";
                //txt_P_7.Text ="";

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
            Excel_Export_File_Name = this.Text; // "Sell_Item_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
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
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            {
                if (dGridView_Base.Rows[i].Selected == true)
                    dGridView_Base.Rows[i].Cells[0].Value = "V";

            }//  end for 그리드 상에서 엑셀 전환을 선택한 V 한 내역을 파악한다.


            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.Cursor = System.Windows.Forms.Cursors.Default;
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
                    if (B_Or != dGridView_Base.Rows[i].Cells[2].Value.ToString())
                    {
                        B_Or = dGridView_Base.Rows[i].Cells[2].Value.ToString();
                        P_chk_cnt++;

                    }

                    StrSql = "Insert into  tbl_SalesDetail_Print_T Values (" + dGridView_Base.Rows[i].Cells[20].Value.ToString() + ",'" + dGridView_Base.Rows[i].Cells[2].Value.ToString() + "','" + cls_User.gid + "')";

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



        private void btnFastReport_Click(object sender, EventArgs e)
        {
            if (cls_User.gid_CountryCode == "TH")
            {

                FastReport_SellTransactionReport_Out_TH();
            }
            else
            {
                FastReport_SellTransactionReport_Out();
            }
        }

        private void FastReport_SellTransactionReport_Out()
        {
            int chk_cnt = 0;
            for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    chk_cnt++;
                }
            }

            if (chk_cnt == 0) //저장할 내역이 없을을 알린다.
            {
                return;
            }


            List<string> arrOrderNumbers = new List<string>();
            DataTable OrderInfomation = new DataTable();
            OrderInfomation.Columns.Add("Mbid2", typeof(string));
            OrderInfomation.Columns.Add("M_Name", typeof(string));
            OrderInfomation.Columns.Add("SellDate", typeof(string));
            OrderInfomation.Columns.Add("SellCode", typeof(string));
            OrderInfomation.Columns.Add("OrderNumber", typeof(string));
            OrderInfomation.Columns.Add("InsuranceNumber", typeof(string));
            OrderInfomation.Columns.Add("TotalPrice", typeof(string));
            OrderInfomation.Columns.Add("InputCard", typeof(string));
            OrderInfomation.Columns.Add("InputCash", typeof(string));
            OrderInfomation.Columns.Add("InputPass_Pay", typeof(string));
            OrderInfomation.Columns.Add("Receive_Method_Name", typeof(string));
            OrderInfomation.Columns.Add("Get_ZipCode", typeof(string));
            OrderInfomation.Columns.Add("Get_Address1", typeof(string));
            OrderInfomation.Columns.Add("Get_Address2", typeof(string));
            OrderInfomation.Columns.Add("Get_Name1", typeof(string));
            OrderInfomation.Columns.Add("Get_Tel1", typeof(string));
            OrderInfomation.Columns.Add("Get_Tel2", typeof(string));
            OrderInfomation.Columns.Add("Pass_Number", typeof(string));
            OrderInfomation.Columns.Add("TotalPV", typeof(int));

            DataTable Products = new DataTable();
            Products.Columns.Add("OrderNumber", typeof(string));
            Products.Columns.Add("ItemCode", typeof(string));
            Products.Columns.Add("Name", typeof(string));
            Products.Columns.Add("ItemCount", typeof(int));
            Products.Columns.Add("ItemPrice", typeof(int));
            Products.Columns.Add("ItemTotalPrice", typeof(int));
            Products.Columns.Add("Etc", typeof(string));
            Products.Columns.Add("ItemPV", typeof(int));

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select A.Good_Code, A.Sub_Good_Code, B.name, A.Sub_Good_Cnt");
            sb.AppendLine("from tbl_Goods_Set A");
            sb.AppendLine(" join tbl_goods b on a.Sub_Good_Code = b.ncode");

            DataSet ds = new DataSet();
            DataTable dtSetItems = new DataTable();
            DataRow[] FindRow;
            if ((new cls_Connect_DB()).Open_Data_Set(sb.ToString(), "SetItems", ds) == false)
                return;
            dtSetItems = ds.Tables["SetItems"];
            /*
             

            string[] g_HeaderText = {
        "선택"     , ""           , "주문번호"    ,   "구매_일자"     , "회원_번호"
      , "성명"     , "기록일"     , "상품코드"    ,   "상품명"        , "개별단가"
      , "_개별PV"  , "구매_수량"  , ""            ,   "총상품액"      , "_총상품PV"
      , "구매_종류", "구분"       , ""            ,   "등록_센타명"   , "구매_센타명"
      , ""         , ""           , ""            ,   "배송구분"      , "우편번호"
      , "배송지"   , "수령인명"   , "연락처1"     , "연락처2"         , "_Prom_TF_SORT"
      , "_송장번호" , "_카드"       , "_현금"        , "_가상계좌"        , "_공제번호"
                                    };
            string[] g_Cols = {
        "Selected"   , ""           , "OrderNumber"    , "SellDate"          ,  "mbid2"
      , "m_name"     , "RegDatee"   , "Itemcode"       , "ItemName"          , "ItemPrice"
      , "ItemPV"     , "ItemCount"  , ""               , "TotalItemPrice"    , "TotalPV"
      , "SellCodeName"  , "SellStateName"  , ""               , "CenterName"        , "OrderCenterName"
      , ""           , ""           , ""               , "Receive_Method_Name"          ,  "ZipCode"
      ,  "Address"    , "Get_Name"   , "Get_Tel1"      , "Get_Tel2"           , "Prom_TF_SORT"
      ,"Pass_Num"    , "InputCard"  , "InputCash"      , "InputPassbook_2"   , "InsuranceNumber"
                                    };

            */
            int SetCnt = 0;
            string GrdOrderNumber = string.Empty;
            foreach (DataGridViewRow grdRow in dGridView_Base.Rows)
            {
                bool Selected = grdRow.Cells["Selected"].Value.ToString().Equals("V");
                if (Selected)
                {
                    GrdOrderNumber = grdRow.Cells["OrderNumber"].Value.ToString();

                    //******** 오더건 만들기
                    if (!arrOrderNumbers.Exists(x => x.Equals(GrdOrderNumber)))
                    {
                        arrOrderNumbers.Add(GrdOrderNumber);
                        DataRow OrderInfo = OrderInfomation.NewRow();

                        double InputCard = 0;
                        double InputCash = 0;
                        double InputPassbook_2 = 0;

                        double.TryParse(grdRow.Cells["InputCard"].Value.ToString(), out InputCard);
                        double.TryParse(grdRow.Cells["InputCash"].Value.ToString(), out InputCash);
                        double.TryParse(grdRow.Cells["InputPassbook_2"].Value.ToString(), out InputPassbook_2);

                        double InputTotalPrice = InputCard + InputCash + InputPassbook_2;

                        OrderInfo["Mbid2"] = grdRow.Cells["mbid2"].Value.ToString();
                        OrderInfo["M_Name"] = grdRow.Cells["m_name"].Value.ToString();
                        OrderInfo["SellDate"] = grdRow.Cells["SellDate"].Value.ToString();
                        OrderInfo["SellCode"] = grdRow.Cells["SellType1"].Value.ToString();
                        OrderInfo["OrderNumber"] = grdRow.Cells["OrderNumber"].Value.ToString();
                        OrderInfo["InsuranceNumber"] = grdRow.Cells["InsuranceNumber"].Value.ToString();
                        OrderInfo["TotalPrice"] = string.Format(cls_app_static_var.str_Currency_Type, InputTotalPrice);
                        OrderInfo["InputCard"] = string.Format(cls_app_static_var.str_Currency_Type, InputCard);
                        OrderInfo["InputCash"] = string.Format(cls_app_static_var.str_Currency_Type, InputCash);
                        OrderInfo["InputPass_Pay"] = grdRow.Cells["InputPass_pay"].Value.ToString().Replace(".0000", "");
                        OrderInfo["Receive_Method_Name"] = grdRow.Cells["배송구분"].Value.ToString();
                        // OrderInfo["Get_ZipCode"] = grdRow.Cells["ZipCode"].Value.ToString();
                        OrderInfo["Get_Address1"] = grdRow.Cells["배송지"].Value.ToString();
                        OrderInfo["Get_Address2"] = string.Empty;//grdRow.Cells[""].Value.ToString();
                        OrderInfo["Get_Name1"] = grdRow.Cells["수령인명"].Value.ToString();
                        OrderInfo["Get_Tel1"] = grdRow.Cells["연락처1"].Value.ToString();
                        OrderInfo["Get_Tel2"] = grdRow.Cells["연락처2"].Value.ToString();
                        OrderInfo["TotalPV"] = grdRow.Cells["_총상품PV"].Value.ToString();

                        OrderInfomation.Rows.Add(OrderInfo);
                    }


                    //******** 아이템건만들기
                    DataRow Product = Products.NewRow();
                    Product["OrderNumber"] = GrdOrderNumber;
                    Product["ItemCode"] = grdRow.Cells["Itemcode"].Value.ToString();
                    Product["Name"] = grdRow.Cells["ItemName"].Value.ToString();
                    Product["ItemCount"] = grdRow.Cells["ItemCount"].Value.ToString();
                    Product["ItemPrice"] = grdRow.Cells["ItemPrice"].Value.ToString();
                    Product["ItemTotalPrice"] = grdRow.Cells["TotalItemPrice"].Value.ToString();
                    Product["ItemPV"] = grdRow.Cells["_개별PV"].Value.ToString();
                    //Product["Etc"] = grdRow.Cells["Etc"].Value.ToString();
                    Products.Rows.Add(Product);

                    //--세트아이템 찾아 넣어주기
                    FindRow = dtSetItems.Select("Good_Code = '" + Product["ItemCode"].ToString() + "'");
                    SetCnt = 0;

                    foreach (DataRow SetRow in FindRow)
                    {
                        SetCnt = Convert.ToInt32(SetRow["Sub_Good_Cnt"]) * Convert.ToInt32(Product["ItemCount"]);

                        DataRow SetProduct = Products.NewRow();
                        SetProduct["OrderNumber"] = GrdOrderNumber;
                        SetProduct["ItemCode"] = SetRow["Sub_Good_Code"].ToString();
                        SetProduct["Name"] = SetRow["name"].ToString();
                        SetProduct["ItemCount"] = SetCnt.ToString();
                        //SetProduct["ItemPrice"] = SetRow["ItemPrice"].ToString();
                        //SetProduct["ItemTotalPrice"] = SetRow["ItemTotalPrice"].ToString();
                        SetProduct["Etc"] = Product["ItemCode"] + " Item";
                        Products.Rows.Add(SetProduct);
                    }

                }

            }


            if (OrderInfomation.Rows.Count > 0)
            {
                frmFastReport frm = new frmFastReport();
                frm.BindingDataTables.Add("OrderInfomation", OrderInfomation);
                frm.BindingDataTables.Add("Products", Products);
                frm.ShowReport(frmFastReport.EShowReport.거래명세표_출고용);
            }
        }

        private void FastReport_SellTransactionReport_Out_TH()
        {


            frmFastReport frm = new frmFastReport();

            DataTable dtMember = new DataTable();
            DataTable dtSalesDetail = new DataTable();
            DataTable dtSalesItemDetail = new DataTable();
            DataTable dtSalesCacu = new DataTable();
            DataTable dtSalesRece = new DataTable();


            List<string> lMember = new List<string>();
            List<string> lSalesDetail = new List<string>();
            List<string> lSalesItemDetail = new List<string>();
            List<string> lSalesCacu = new List<string>();
            List<string> lSalesRece = new List<string>();

            //추가된 세트아이템 코드
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select A.Good_Code, A.Sub_Good_Code, B.name, A.Sub_Good_Cnt");
            sb.AppendLine("from tbl_Goods_Set (NOLOCK)  A");
            sb.AppendLine(" join tbl_goods (NOLOCK) b on a.Sub_Good_Code = b.ncode order by  A.Sub_Good_Code ");

            DataSet dsSetItems = new DataSet();
            DataTable dtSetItems = new DataTable();
            DataRow[] FindRow;
            if ((new cls_Connect_DB()).Open_Data_Set(sb.ToString(), "SetItems", dsSetItems) == false)
                return;

            dtSetItems = dsSetItems.Tables[0];

            cls_Connect_DB cls_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();
            string ReportName = "FastReport";

            foreach (DataGridViewRow GridRow in dGridView_Base.Rows)
            {
                if (!GridRow.Cells["Selected"].Value.ToString().Equals("V")) continue;

                string Mbid2 = GridRow.Cells["mbid2"].Value.ToString();
                string OrderNumber = GridRow.Cells["OrderNumber"].Value.ToString();

                // -- Member 
                if (lMember.Count == 0)
                {
                    cls_Connect.Open_Data_Set("SELECT * FROM tbl_Memberinfo (NOLOCK) WHERE mbid2 = " + Mbid2, ReportName, ds);
                    dtMember = ds.Tables[ReportName].Copy();
                    lMember.Add(Mbid2);
                }
                else if (!lMember.Contains(Mbid2))
                {
                    cls_Connect.Open_Data_Set("SELECT * FROM tbl_Memberinfo (NOLOCK) WHERE mbid2 = " + Mbid2, ReportName, ds);
                    dtMember.Rows.Add(ds.Tables[ReportName].Rows[0].ItemArray);
                    lMember.Add(Mbid2);
                }
                ds = new DataSet();


                // -- SalesDetail
                if (lSalesDetail.Count == 0)
                {
                    cls_Connect.Open_Data_Set("SELECT * FROM tbl_SalesDetail (NOLOCK) WHERE OrderNumber = '" + OrderNumber + "'", ReportName, ds);
                    dtSalesDetail = ds.Tables[ReportName].Copy();
                    lSalesDetail.Add(OrderNumber);

                }
                else if (!lSalesDetail.Contains(OrderNumber))
                {
                    cls_Connect.Open_Data_Set("SELECT * FROM tbl_SalesDetail (NOLOCK) WHERE OrderNumber = '" + OrderNumber + "'", ReportName, ds);
                    dtSalesDetail.Rows.Add(ds.Tables[ReportName].Rows[0].ItemArray);
                    lSalesDetail.Add(OrderNumber);
                }
                ds = new DataSet();


                // -- SalesItemDetail
                DataSet dsTemp_SalesItemIndex = new DataSet();
                if (lSalesItemDetail.Count == 0)
                {
                    cls_Connect.Open_Data_Set("SELECT * FROM tbl_SalesItemDetail (NOLOCK) WHERE OrderNumber = '" + OrderNumber + "'", ReportName, dsTemp_SalesItemIndex);
                    dtSalesItemDetail = dsTemp_SalesItemIndex.Tables[ReportName].Clone();

                    FastReport_Item_Setting(ref dtSalesItemDetail, dsTemp_SalesItemIndex, dtSetItems);

                    lSalesItemDetail.Add(OrderNumber);
                }
                else if (!lSalesItemDetail.Contains(OrderNumber))
                {
                    cls_Connect.Open_Data_Set("SELECT * FROM tbl_SalesItemDetail (NOLOCK) WHERE OrderNumber = '" + OrderNumber + "'", ReportName, dsTemp_SalesItemIndex);
                    FastReport_Item_Setting(ref dtSalesItemDetail, dsTemp_SalesItemIndex, dtSetItems);
                    lSalesItemDetail.Add(OrderNumber);
                }
                ds = new DataSet();



                // -- SalesRece
                if (lSalesRece.Count == 0)
                {
                    cls_Connect.Open_Data_Set("SELECT * FROM tbl_Sales_Rece (NOLOCK) WHERE OrderNumber = '" + OrderNumber + "' ORDER BY SalesItemIndex DESC ", ReportName, ds);
                    dtSalesRece = ds.Tables[ReportName].Copy();
                    lSalesRece.Add(OrderNumber);
                }
                else if (!lSalesRece.Contains(OrderNumber))
                {
                    cls_Connect.Open_Data_Set("SELECT * FROM tbl_Sales_Rece (NOLOCK) WHERE OrderNumber = '" + OrderNumber + "' ORDER BY SalesItemIndex DESC ", ReportName, ds);
                    if (cls_Connect.DataSet_ReCount > 0)
                    {
                        dtSalesRece.Rows.Add(ds.Tables[ReportName].Rows[0].ItemArray);
                        lSalesRece.Add(OrderNumber);
                    }
                }
                ds = new DataSet();


                // -- SalesCacu
                if (lSalesCacu.Count == 0)
                {
                    cls_Connect.Open_Data_Set("SELECT top 2 * FROM tbl_Sales_Cacu (NOLOCK) WHERE OrderNumber = '" + OrderNumber + "'", ReportName, ds);
                    dtSalesCacu = ds.Tables[ReportName].Copy();
                    lSalesCacu.Add(OrderNumber);
                }
                if (!lSalesCacu.Contains(OrderNumber))
                {
                    cls_Connect.Open_Data_Set("SELECT top 2 * FROM tbl_Sales_Cacu (NOLOCK) WHERE OrderNumber = '" + OrderNumber + "'", ReportName, ds);

                    if (ds.Tables[ReportName].Rows.Count > 0)
                        dtSalesCacu.Rows.Add(ds.Tables[ReportName].Rows[0].ItemArray);

                    lSalesCacu.Add(OrderNumber);
                }
                ds = new DataSet();

            }


            dtSalesDetail.Columns.Add("Mem_Hptel");
            dtSalesDetail.Columns.Add("Mem_Email");
            dtSalesDetail.Columns.Add("Mem_AddCode1");
            dtSalesDetail.Columns.Add("Mem_Address1");
            dtSalesDetail.Columns.Add("Mem_Address2");
            dtSalesDetail.Columns.Add("Rece_Name");
            dtSalesDetail.Columns.Add("Rece_Tel1");
            dtSalesDetail.Columns.Add("Rece_Tel2");
            dtSalesDetail.Columns.Add("Rece_AddCode1");
            dtSalesDetail.Columns.Add("Rece_Address1");
            dtSalesDetail.Columns.Add("Rece_Address2");
            foreach (DataRow row in dtMember.Rows)
            {
                row["Address1"] = encrypter.Decrypt(row["Address1"].ToString());
                row["Address2"] = encrypter.Decrypt(row["Address2"].ToString());
                row["Email"] = encrypter.Decrypt(row["Email"].ToString());
                row["hometel"] = encrypter.Decrypt(row["hometel"].ToString());
                row["hptel"] = encrypter.Decrypt(row["hptel"].ToString());
                row["cpno"] = encrypter.Decrypt(row["cpno"].ToString());

            }

            foreach (DataRow row in dtSalesRece.Rows)
            {
                row["get_address1"] = encrypter.Decrypt(row["get_address1"].ToString());
                row["get_address2"] = encrypter.Decrypt(row["get_address2"].ToString());
                row["Get_Tel1"] = encrypter.Decrypt(row["Get_Tel1"].ToString());
                row["Get_Tel2"] = encrypter.Decrypt(row["Get_Tel2"].ToString());
            }

            foreach (DataRow rOrd in dtSalesDetail.Rows)
            {
                string OrdNum = rOrd["OrderNumber"].ToString();
                string Mbid = rOrd["mbid2"].ToString();

                var FindRece = dtSalesRece.Select("OrderNumber='" + OrdNum + "'");
                if (FindRece.Length > 0)
                {
                    rOrd["Rece_Name"] = FindRece[0]["Get_Name1"].ToString();
                    rOrd["Rece_Tel1"] = FindRece[0]["Get_Tel1"].ToString();
                    rOrd["Rece_Tel2"] = FindRece[0]["Get_Tel2"].ToString();
                    rOrd["Rece_AddCode1"] = FindRece[0]["Get_ZipCode"].ToString();
                    rOrd["Rece_Address1"] = FindRece[0]["get_address1"].ToString();
                    rOrd["Rece_Address2"] = FindRece[0]["get_address2"].ToString();
                }


                var FindMem = dtMember.Select("mbid2=" + Mbid);
                if (FindMem.Length > 0)
                {
                    rOrd["Mem_Hptel"] = FindMem[0]["Hptel"].ToString();
                    rOrd["Mem_Email"] = FindMem[0]["Email"].ToString();
                    rOrd["Mem_AddCode1"] = FindMem[0]["AddCode1"].ToString();
                    rOrd["Mem_Address1"] = FindMem[0]["Address1"].ToString();
                    rOrd["Mem_Address2"] = FindMem[0]["Address2"].ToString();
                }
            }

            //2024-05-30 LinQ 를 사용해서 InsuranceNumber 필드에 cpno 값 넣기! 
            var joinedData = from Mem in dtMember.AsEnumerable()
                             join Sales in dtSalesDetail.AsEnumerable()
                             on new { mbid = Mem.Field<string>("mbid"), mbid2 = Mem.Field<int>("mbid2") }
                         equals new { mbid = Sales.Field<string>("mbid"), mbid2 = Sales.Field<int>("mbid2") }
                             select new
                             {
                                 OrderNumber = Sales.Field<string>("OrderNumber"),
                                 Cpno = Mem.Field<string>("cpno"),
                                 SalesRow = Sales
                             };
            

            foreach(var item in joinedData)
            {
                item.SalesRow["InsuranceNumber"] = item.Cpno;
            }

            frm.BindingDataTables.Add("SalesDetail", dtSalesDetail);
            frm.BindingDataTables.Add("SalesItemDetail", dtSalesItemDetail);
            frm.BindingDataTables.Add("SalesCacu", dtSalesCacu);
            frm.ShowReport(frmFastReport.EShowReport.거래명세표_출고용_TH);
        }

        private void FastReport_Item_Setting(ref DataTable dtMain, DataSet dsItems, DataTable dtSetItems)
        {
            //추가된 세트아이템 코드
            DataRow[] FindRow;
            int SetCnt = 0;
            foreach (DataRow row in dsItems.Tables[0].Rows)
            {
                DataRow Product = dtMain.NewRow();
                Product["OrderNumber"] = row["OrderNumber"].ToString();
                Product["SalesItemIndex"] = row["SalesItemIndex"].ToString();
                Product["ItemCode"] = row["Itemcode"].ToString();
                Product["ItemName"] = row["ItemName"].ToString();
                Product["ItemCount"] = row["ItemCount"].ToString();

                    Product["ItemPV"] = row["ItemPV"].ToString();
                    Product["ItemPrice"] = row["ItemPrice"].ToString();
                    Product["ItemTotalPrice"] = row["ItemTotalPrice"].ToString();

                ///Product["Etc"] = row["Etc"].ToString();

                dtMain.Rows.Add(Product);
                //--세트아이템 찾아 넣어주기
                FindRow = dtSetItems.Select("Good_Code = '" + Product["ItemCode"].ToString() + "'");
                SetCnt = 0;

                foreach (DataRow SetRow in FindRow)
                {
                    SetCnt = Convert.ToInt32(SetRow["Sub_Good_Cnt"]) * Convert.ToInt32(Product["ItemCount"]);

                    DataRow SetProduct = dtMain.NewRow();
                    SetProduct["OrderNumber"] = row["OrderNumber"].ToString();
                    SetProduct["SalesItemIndex"] = row["SalesItemIndex"].ToString();
                    SetProduct["ItemCode"] = SetRow["Sub_Good_Code"].ToString();
                    SetProduct["ItemName"] = "ㄴ(SET) " + SetRow["name"].ToString();
                    SetProduct["ItemCount"] = SetCnt.ToString();


                    dtMain.Rows.Add(SetProduct);
                }
            }

        }

        private void butt_Print_Click(int tt)
        {


        }


        private void opt_2_MouseClick(object sender, MouseEventArgs e)
        {
            tableLayoutPanel6.Visible = true;
            tableLayoutPanel9.Visible = true;

        }

        private void dGridView_Base_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[1].Value != null))
            {
                string Send_Nubmer = ""; string Send_Name = ""; ; string Send_OrderNumber = "";
                Send_OrderNumber = (sender as DataGridView).CurrentRow.Cells["OrderNumber"].Value.ToString();
                Send_Nubmer = (sender as DataGridView).CurrentRow.Cells["mbid2"].Value.ToString();
                Send_Name = (sender as DataGridView).CurrentRow.Cells["m_name"].Value.ToString();

                if (Send_OrderNumber == "") return;

                Send_Sell_Number(Send_Nubmer, Send_Name, Send_OrderNumber);   //부모한테 이벤트 발생 신호한다.
            }
        }


        private void opt_1_MouseClick(object sender, MouseEventArgs e)
        {
            tableLayoutPanel6.Visible = false;
            tableLayoutPanel9.Visible = false;
            txtCenter3.Text = "";
            txtCenter3_Code.Text = "";
            mtxtOutDate.Text = "";
        }


        public void GridViewExcel(DataGridView grid)
        {
            string strFolder = Application.StartupPath.ToString();
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = "출고요청지시_" + cls_User.gid_date_time;
            saveFile.DefaultExt = "xls";
            saveFile.Filter = "Excel files (*.xls)|*.xls";
            saveFile.InitialDirectory = strFolder + "\\Doc\\";

            DialogResult result = saveFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                object missingType = Type.Missing;


                Excel.Application oXL;
                Excel._Workbook oWB;
                Excel._Worksheet oSheet;
                Excel.Range range;
                //Excel.Range oRng;

                try
                {
                    oXL = new Excel.Application();
                    oXL.Visible = true;
                    oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                    oSheet = (Excel._Worksheet)oWB.ActiveSheet;
                    range = null;

                    int k = 0;
                    string[] colHeader = new string[grid.ColumnCount];
                    for (int i = 0; i < grid.Columns.Count; i++)
                    {
                        oSheet.Cells[1, i + 1] = grid.Columns[i].HeaderText;

                        if (i <= 25)
                        {
                            k = i + 65;
                            colHeader[i] = Convert.ToString((char)k);

                        }
                        else if (i > 25 && i <= 51)
                        {
                            k = i - 26;
                            colHeader[i] = "A" + colHeader[k];

                        }
                        else if (i >= 52)
                        {
                            k = i - 52;
                            colHeader[i] = "B" + colHeader[k];

                        }



                    }

                    oSheet.get_Range("A1", colHeader[colHeader.Length - 1] + "1").Font.Bold = true;
                    oSheet.get_Range("A1", colHeader[colHeader.Length - 1] + "1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    object[,] saNames = new object[grid.RowCount, grid.ColumnCount];

                    int rowcount = 0;

                    cls_form_Meth cm = new cls_form_Meth();

                    string B1 = cm._chang_base_caption_search("코드");
                    string B2 = cm._chang_base_caption_search("번호");
                    string B3 = cm._chang_base_caption_search("핸드폰");
                    string B4 = cm._chang_base_caption_search("집전화");
                    string B5 = cm._chang_base_caption_search("연락처");
                    string B6 = cm._chang_base_caption_search("HP");
                    string B7 = cm._chang_base_caption_search("일자");
                    string B8 = cm._chang_base_caption_search("기록");
                    string B9 = cm._chang_base_caption_search("우편번호");
                    string B10 = cm._chang_base_caption_search("우편_번호");

                    string tp;
                    for (int i = 0; i < grid.RowCount; i++)
                    {
                        if (grid.Rows[i].Cells[0].Value.ToString() == "V")
                        {
                            for (int j = 0; j < grid.ColumnCount; j++)
                            {
                                string T_string = grid.Columns[j].HeaderText.ToString();
                    
                                if (j <= 25)
                                {
                                    range = oSheet.get_Range(colHeader[j] + Convert.ToString(rowcount + 2), Missing.Value);
                                }
                                else if (j > 25 && j <= 51)
                                {
                                    int tempc = j - 26;
                                    range = oSheet.get_Range("A" + colHeader[tempc] + Convert.ToString(rowcount + 2), Missing.Value);
                                }
                                else if (j >= 52)
                                {
                                    int tempc = j - 52;
                                    range = oSheet.get_Range("B" + colHeader[tempc] + Convert.ToString(rowcount + 2), Missing.Value);
                                }



                                
                                if (T_string.Contains(B1) == true || T_string.Contains(B2) || T_string.Contains(B3) || T_string.Contains(B4) || T_string.Contains(B5) || T_string.Contains(B6) || T_string.Contains(B7) || T_string.Contains(B8) || T_string.Contains(B9) || T_string.Contains(B10)
                                    || (T_string.IndexOf("코드") > 0 || T_string.IndexOf("번호") > 0 || T_string.IndexOf("일자") > 0 || T_string.IndexOf("핸드폰") > 0 || T_string.Contains("기록") == true))
                                {
                                    range.NumberFormatLocal = @"@ ";
                                    range.NumberFormat = @"@ ";
                                    //System.Threading.Thread.Sleep(500);
                                }
                                if (grid.Columns[j].Name.Equals("상품코드") && grid.Rows[i].Cells["PROM_TF_SORT"].Value.ToString() != string.Empty)
                                {
                                    saNames[rowcount, j] = grid.Rows[i].Cells["PROM_TF_SORT"].Value;
                                }
                                else
                                {
                                    saNames[rowcount, j] = grid.Rows[i].Cells[j].Value.ToString();
                                }

                            }
                            rowcount++;
                        }

                    }

                    oSheet.get_Range(colHeader[0] + "2", colHeader[colHeader.Length - 1] + (grid.RowCount + 1)).Value2 = saNames;


                    int del_Cnt = 0;
                    for (int j = 0; j < grid.ColumnCount; j++)
                    {
                        string T_string = grid.Columns[j].HeaderText.ToString();

                        if (T_string == "" || T_string.Substring(0, 1) == "_")
                        {
                            oSheet.Columns[j + 1 - del_Cnt].delete();
                            del_Cnt++;
                        }
                    }


                    // 컬럼명(길이)에 맞추어 자동으로 Fiting
                    oSheet.Columns.AutoFit();

                    //oSheet.Columns.Summary();

                    cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                    string Tsql = "";
                    Tsql = "Insert Into tbl_Excel_User Values ( ";
                    Tsql = Tsql + "'" + cls_User.gid + "',Convert(Varchar(25),GetDate(),21),";
                    Tsql = Tsql + "'" + this.Name + "',";
                    Tsql = Tsql + "'" + saveFile.FileName.ToString() + "') ";

                    if (Temp_Connect.Insert_Data(Tsql, "tbl_Excel_User") == false) return;


                    oWB.SaveAs(saveFile.FileName,
                          Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
                          missingType, missingType, missingType, missingType,
                          Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                          missingType, missingType, missingType, missingType, missingType);




                    oXL.Visible = true;
                    oXL.UserControl = true;
                }
                catch (Exception theException)
                {
                    String errorMessage;
                    errorMessage = "Error: ";
                    errorMessage = String.Concat(errorMessage, theException.Message);
                    errorMessage = String.Concat(errorMessage, " Line: ");
                    errorMessage = String.Concat(errorMessage, theException.Source);

                    MessageBox.Show(errorMessage, "Error");
                }
            }

        }










    }
}
