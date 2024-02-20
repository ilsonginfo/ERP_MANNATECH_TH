using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MLM_Program
{
    public partial class frmStock_OUT_Sell_Cancel : Form
    {
        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
        
        cls_Grid_Base cgb = new cls_Grid_Base();
        
        private const string base_db_name = "tbl_SalesItemDetail";
        private int Data_Set_Form_TF;        


        public frmStock_OUT_Sell_Cancel()
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

            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_SellCode_ComboBox(combo_Se, combo_Se_Code);
            cpbf.Put_Rec_Code_ComboBox(combo_Rec, combo_Rec_Code);

            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;
            //grB_Search.Height = mtxtMbid.Top + mtxtMbid.Height + 3;  


            mtxtSellDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtOutDate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtOutDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtMakDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMakDate2.Mask = cls_app_static_var.Date_Number_Fromat;


            txt_P_1.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_5.BackColor = cls_app_static_var.txt_Enable_Color;

            radioB_SellTF2.Checked = true; 
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

            if (mtxtOutDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtOutDate.Text, mtxtOutDate, "Date") == false)
                {
                    mtxtOutDate.Focus();
                    return false;
                }

            }

            if (mtxtOutDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtOutDate2.Text, mtxtOutDate2, "Date") == false)
                {
                    mtxtSellDate2.Focus();
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

            cls_form_Meth cm = new cls_form_Meth();

            Tsql = "Select ''  ";
            Tsql = Tsql + " ,LEFT(Out_Date,4) +'-' + LEFT(RIGHT(Out_Date,4),2) + '-' + RIGHT(Out_Date,2)  as Out_Date ";
            Tsql = Tsql + " ,Out_Index ";
            Tsql = Tsql + " ,tbl_SalesDetail.OrderNumber  ";

            Tsql = Tsql + " ,LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2)  as SellDate ";


            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + ", tbl_SalesDetail.mbid + '-' + Convert(Varchar,tbl_SalesDetail.mbid2) ";
            //else
            //    Tsql = Tsql + ", tbl_SalesDetail.mbid2 ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ",Case When tbl_SalesDetail.SellCode <> '' Then  tbl_SalesDetail.mbid + '-' + Convert(Varchar,tbl_SalesDetail.mbid2) ELSE  tbl_SalesDetail.mbid End  as mbid2";
            else
                Tsql = Tsql + ",Case When tbl_SalesDetail.SellCode <> '' Then  Convert(varchar,tbl_SalesDetail.mbid2)  ELSE  tbl_SalesDetail.mbid End as mbid2";



            Tsql = Tsql + " ,tbl_SalesDetail.M_Name ";

            Tsql = Tsql + ", tbl_SalesItemDetail.RecordTime ";

            Tsql = Tsql + " , tbl_SalesItemDetail.ItemCode "; 
            Tsql = Tsql + " , tbl_Goods.Name Item_Name ";

            Tsql = Tsql + " , tbl_SalesItemDetail.ItemCount ";
            Tsql = Tsql + " , tbl_StockOutput.ItemCount ";
            Tsql = Tsql + " ,tbl_salesitemdetail.ItemTotalPrice";
            Tsql = Tsql + " ,Isnull(St_Bus.Name,'') as St_B_Name";

            Tsql = Tsql + " , tbl_SellType.SellTypeName SellCodeName  ";

            //Tsql = Tsql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " SellStateName ";
            //Tsql = Tsql + " , Ch_T_2." + cls_app_static_var.Base_M_Detail_Ex + " Receive_Method_Name ";
            Tsql = Tsql + " , '' ";

            Tsql = Tsql + " ,Case When Receive_Method = '1' Then '" + cm._chang_base_caption_search("직접수령") + "'";
            Tsql = Tsql + "  When Receive_Method = '2' Then '" + cm._chang_base_caption_search("배송") + "'";
            Tsql = Tsql + "  When Receive_Method = '3' Then '" + cm._chang_base_caption_search("센타수령") + "'";
            Tsql = Tsql + "  When Receive_Method = '4' Then '" + cm._chang_base_caption_search("본사직접수령") + "'";
            Tsql = Tsql + " ELSE '' ";
            Tsql = Tsql + " END  Receive_Method_Name ";


            Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";
            Tsql = Tsql + " ,Isnull(S_Bus.Name,'') as S_B_Name";


            Tsql = Tsql + " ,tbl_SalesItemDetail.Salesitemindex  ";
            Tsql = Tsql + " ,tbl_Memberinfo.BusinessCode  ";
            Tsql = Tsql + " ,tbl_SalesDetail.BusCode  ";
            Tsql = Tsql + " ,tbl_StockOutput.Out_C_Code  ";


            Tsql = Tsql + " ,Get_ZipCode ";
            Tsql = Tsql + " ,Get_city ";
            Tsql = Tsql + " ,Get_state ";
            Tsql = Tsql + " ,Get_Address1 ";
            Tsql = Tsql + " ,Get_Address2 ";
            Tsql = Tsql + " ,Get_Name1 ";
            Tsql = Tsql + " ,Get_Tel1 ";
            Tsql = Tsql + " ,Get_Tel2 ";
            Tsql = Tsql + " ,  tbl_Sales_Rece.Pass_Number ";
            Tsql = Tsql + " ,  tbl_StockOutput.recordtime ";
            Tsql = Tsql + " , ISNULL(RefundDetail.OrderNumber, '') RefundOrderNumber ";

            Tsql = Tsql + " From tbl_StockOutput (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_SalesItemDetail (nolock) ON  tbl_SalesItemDetail.OrderNumber = tbl_StockOutput.OrderNumber  And tbl_StockOutput.Salesitemindex = tbl_SalesitemDetail.Salesitemindex  ";
            Tsql = Tsql + " LEFT JOIN tbl_SalesDetail (nolock)  ON tbl_SalesItemDetail.OrderNumber = tbl_SalesDetail.OrderNumber ";
            Tsql = Tsql + " LEFT JOIN tbl_Sales_Rece (nolock)  ON tbl_SalesItemDetail.OrderNumber = tbl_Sales_Rece.OrderNumber And tbl_SalesItemDetail.Salesitemindex = tbl_Sales_Rece.Salesitemindex "; 
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";            
            Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode ";
            Tsql = Tsql + " LEFT JOIN tbl_Business S_Bus (nolock) ON tbl_SalesDetail.BusCode = S_Bus.NCode ";
            Tsql = Tsql + " LEFT JOIN tbl_Business St_Bus (nolock) ON tbl_StockOutput.Out_C_Code = St_Bus.NCode ";            
            Tsql = Tsql + " Left Join tbl_Class C1 On tbl_Memberinfo.CurGrade=C1.Grade_Cnt ";
            Tsql = Tsql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_SalesItemDetail' And  Ch_T.M_Detail = tbl_SalesitemDetail.SellState ";
            Tsql = Tsql + " LEFT JOIN tbl_Base_Change_Detail Ch_T_2 (nolock) ON Ch_T_2.M_Detail_S = 'tbl_Sales_Rece' And  Ch_T_2.M_Detail = Convert(Varchar,tbl_Sales_Rece.Receive_Method) ";
            Tsql = Tsql + " LEFT JOIN tbl_Goods (nolock) ON tbl_Goods.Ncode = tbl_SalesitemDetail.ItemCode ";
            Tsql = Tsql + " LEFT Join tbl_SellType ON tbl_SalesDetail.SellCode = tbl_SellType.SellCode ";
            Tsql = Tsql + " LEFT Join tbl_SalesDetail (nolock)  RefundDetail ON tbl_SalesDetail.OrderNumber = RefundDetail.Re_BaseOrderNumber ";
        }



        private void Make_Base_Query_(ref string Tsql)
        {

            combo_Rec_Code.SelectedIndex = combo_Rec.SelectedIndex; 

            string strSql = " Where (Out_FL ='001'  OR  (Out_FL ='002' And  tbl_StockOutput.OrderNumber <> '' And tbl_StockOutput.SG_Mbid2 > 0)  ) ";
            strSql = strSql + " And tbl_StockOutput.ItemCount >0 And  tbl_SalesDetail.M_name is not null ";
            strSql = strSql + " And tbl_StockOutput.Out_Index Not in (Select Out_Index From tbl_StockOutput_Not_Union (nolock) ) ";



            //if (radioB_SellTF2.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.SellCode <> '' ";  //진주문만 나오게 한다
            //else
            //    strSql = strSql + " And tbl_SalesDetail.SellCode = '' ";  //진주문만 나오게 한다



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


            //가입일자로 검색 -1
            if ((mtxtOutDate.Text.Replace("-", "").Trim() != "") && (mtxtOutDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_StockOutput.Out_Date = '" + mtxtOutDate.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtOutDate.Text.Replace("-", "").Trim() != "") && (mtxtOutDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_StockOutput.Out_Date >= '" + mtxtOutDate.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_StockOutput.Out_Date <= '" + mtxtOutDate2.Text.Replace("-", "").Trim() + "'";
            }


            if (combo_Rec_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Sales_Rece.Receive_Method = " + combo_Rec_Code.Text.Trim();



            //기록일자로 검색 -1
            if ((mtxtMakDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And Replace(Left( tbl_StockOutput.recordtime ,10),'-','') = '" + mtxtMakDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtMakDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And Replace(Left( tbl_StockOutput.recordtime ,10),'-','') >= '" + mtxtMakDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And Replace(Left( tbl_StockOutput.recordtime ,10),'-','') <= '" + mtxtMakDate2.Text.Replace("-", "").Trim() + "'";
            }




            if (txt_ItemName_Code2.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesitemDetail.ItemCode = '" + txt_ItemName_Code2.Text.Trim() + "'";

            //센타코드로으로 검색
            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";

            if (txtCenter2_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.BusCode = '" + txtCenter2_Code.Text.Trim() + "'";

            if (txtCenter3_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_StockOutput.Out_C_Code = '" + txtCenter3_Code.Text.Trim() + "'";

            //if (txtR_Id_Code.Text.Trim() != "")
            //    strSql = strSql + " And tbl_SalesDetail.recordid = '" + txtR_Id_Code.Text.Trim() + "'";


            //if (combo_Se_Code.Text.Trim() != "")
            //    strSql = strSql + " And tbl_SalesDetail.SellCode = '" + combo_Se_Code.Text.Trim() + "'";


            if (txtSellCode_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.SellCode = '" + txtSellCode_Code.Text.Trim() + "'";

            if (txtOrderNumber.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.OrderNumber = '" + txtOrderNumber.Text.Trim() + "'";





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
              //  strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
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
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name , this.Text ) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            double Sum_11 = 0; double Sum_12 = 0; // double Sum_12 = 0;
            int OrdCnt = 0;
            //double Sum_13 = 0; //double Sum_14 = 0; double Sum_15 = 0;
            //double Sum_16 = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            Dictionary<string, string> OrderNum = new Dictionary<string, string>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                Sum_11 = Sum_11 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][10].ToString());
                Sum_12 = Sum_12 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][11].ToString());

                string T_ver = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                if (OrderNum.ContainsKey(T_ver) != true)
                {
                    OrdCnt++;
                    OrderNum[T_ver] = T_ver;
                }
                //Sum_13 = Sum_13 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][12].ToString());
                //Sum_14 = Sum_14 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][13].ToString());                
            }

                dGridView_Base.HorizontalScrollingOffset = 0;
            if (gr_dic_text.Count > 0)
            {
                txt_P_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_11);
                txt_P_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_12);
                txt_P_5.Text = string.Format(cls_app_static_var.str_Currency_Type, OrdCnt);
                //txt_P_4.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_12);
                //txt_P_5.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_14);
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
            
            cgb.grid_col_Count = 33;            
            cgb.basegrid = dGridView_Base;            
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 4;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            string[] g_HeaderText = {"선택" ,"출고_일자", "출고번호"  , "주문번호"  , "주문_일자"
                                 ,  "회원_번호"   , "성명"  , "매출기록일"     , "상품코드"    , "상품명"
                                 , "주문_수량"    , "출고_수량", "매출" , "출고지" , "주문_종류"   , ""
                                 , "배송구분" , "등록_센타명"    , "주문_센타명"      , "상품인덱스"    , ""
                                  , "" , ""    ,  "우편번호" , "태국_도시", "태국_주"
                                  ,  "배송지"   , "수령인명", "연락처1" , "연락처2", "송장번호"
                                  , "기록일", "반품번호"
                                    };

            cgb.grid_col_header_text = g_HeaderText;

            string[] g_Cols = {"Selected" ,"Out_Date", "Out_Index"  , "OrderNumber"  , "SellDate"
                                 ,  "mbid2"   , "M_Name"  , "RecordTime"     , "ItemCode"    , "Item_Name"
                                 , "ItemCount"    , "ItemCount_out" , "Itemtotalprice"  , "St_B_Name" , "SellCodeName"   , "col1"
                                 , "Receive_Method_Name" , "B_Name"    , "S_B_Name"      , "Salesitemindex"    , "BusinessCode"
                                  , "BusCode" , "Out_C_Code"    ,  "Get_ZipCode" , "Get_city", "Get_state"
                                  ,  "Get_Address"   , "Get_Name1", "Get_Tel1" , "Get_Tel2","Pass_Number"
                                  , "recordtime_out", "RefundOrderNumber"
                                    };
            cgb.grid_col_name = g_Cols;

            int[] g_Width = { 60, 90, 110, 120, 90  
                             , 90 ,90, 90, 80, 110
                             , 70  ,70  ,90 , 120, 80, 0
                             , 80  ,110 , 110, 10 , 0                                                          
                             , 0,0 , 10, 50, 50
                             , 10 , 10, 10, 10, 10
                             , 10, 75
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { false , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                    
                                    ,true , true,  true,  true ,true         
                                    ,true , true,  true,  true ,true
                                    ,true , true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleLeft //10

                               ,DataGridViewContentAlignment.MiddleCenter   
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleCenter   //15

                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter //20

                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //25

                               ,DataGridViewContentAlignment.MiddleLeft //26
                               ,DataGridViewContentAlignment.MiddleLeft //27
                               ,DataGridViewContentAlignment.MiddleLeft //28
                               ,DataGridViewContentAlignment.MiddleLeft //29
                               ,DataGridViewContentAlignment.MiddleLeft //30

                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleCenter


                              };
            cgb.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
          
            gr_dic_cell_format[11- 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = "###,###,###,##0";
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;            
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
                                , ds.Tables[base_db_name].Rows[fi_cnt][7]
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

                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][26].ToString () ) + ' ' + encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][27].ToString () )
                                ,ds.Tables[base_db_name].Rows[fi_cnt][28]

                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][29].ToString () )
                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][30].ToString () )
                                ,ds.Tables[base_db_name].Rows[fi_cnt][31]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][32]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][33]
                 
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

            if (tb.Name == "txtCenter4")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtIO")
            {
                cgb_Pop.Next_Focus_Control = butt_Select;
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode, "", " And  (Ncode ='004' OR Ncode = '005' OR Ncode = '006'  ) ");
            }
            else
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
            //Tsql = Tsql + " And  (Ncode ='004' OR Ncode = '005' ) ";


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

                prB.Visible = true;                butt_S_Save.Enabled = false;
                Save_Base_Data(ref Save_Error_Check);
                prB.Visible = false;               butt_S_Save.Enabled = true;

                if (Save_Error_Check > 0)
                {
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                    cgb.d_Grid_view_Header_Reset();
                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<                                        

                    txt_P_1.Text = ""; txt_P_2.Text = ""; txt_P_3.Text = "";
                    txt_P_4.Text = "";// txt_P_5.Text = ""; txt_P_6.Text = "";
                    //txt_P_7.Text = "";
                    //txtOutDate.Text = ""; txtCenter3.Text = ""; txtCenter3_Code.Text = "";
                    chk_Total.Checked = false; 

                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    Base_Grid_Set();  //뿌려주는 곳
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }                
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            

        }



        private Boolean Sub_Check_TextBox_Error()
        {
            //cls_Check_Text T_R = new cls_Check_Text();
            //string me = "";


                //me = T_R.Text_Null_Check(txtOutDate, "Msg_Sort_Stock_Out_Date"); //출고일자를
                //if (me != "")
                //{
                //    MessageBox.Show(me);
                //    return false;
                //}

                //me = T_R.Text_Null_Check(txtCenter3_Code, "Msg_Sort_Stock_Out_Center"); //출고지를
                //if (me != "")
                //{
                //    MessageBox.Show(me);
                //    return false;
                //}

                ////날짜 관련 텍스트 파일들에 대해서 날짜 오류를 체크한다
                //if (Check_TextBox_Error_Date() == false) return false;

            

            int chk_cnt = 0;

            for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                    chk_cnt++;                    
            }

            if (chk_cnt == 0) //저장할 내역이 없을을 알린다.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridView_Base.Focus(); return false;
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
            return true;
        }




        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
      
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Cancel_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            
            if (Sub_Check_TextBox_Error() == false) return;

            
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();


            prB.Minimum = 0; prB.Maximum = dGridView_Base.Rows.Count;
            prB.Step = 1;    prB.Value = 0;

            try
            {
                bool IsExistsRefundOrder = false;
                string StrSql = ""; string T_Or = ""; string Out_Index = "";
                int ItemCount = 0; string ItemCode = ""; int SalesItemIndex = 0;

                for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                {
                    if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                    {
                        //반품주문번호가 존재한다. 그러면 패스함
                        if (dGridView_Base.Rows[i].Cells["RefundOrderNumber"].Value.ToString() != string.Empty)
                        {
                            IsExistsRefundOrder = true;
                            continue;
                        }


                        Out_Index = dGridView_Base.Rows[i].Cells[2].Value.ToString();

                        ItemCode = dGridView_Base.Rows[i].Cells[8].Value.ToString();
                        ItemCount = int.Parse(dGridView_Base.Rows[i].Cells[11].Value.ToString());

                        SalesItemIndex = int.Parse(dGridView_Base.Rows[i].Cells[19].Value.ToString());
                        T_Or = dGridView_Base.Rows[i].Cells[3].Value.ToString();

                        string Out_C_Code = dGridView_Base.Rows[i].Cells[22].Value.ToString();

                        // Save_Base_Data(T_Or, SalesItemIndex, Out_C_Code , Temp_Connect, Conn, tran);

                        StrSql = "Insert into tbl_StockOutput_DelBackup  ";
                        StrSql = StrSql + " Select * ,'" + cls_User.gid + "',Convert(Varchar(25),GetDate(),21) ";
                        StrSql = StrSql + " From tbl_StockOutput  (nolock)";
                        StrSql = StrSql + " Where Out_Index =  '" + Out_Index + "'";

                        Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);



                        StrSql = "DELETE tbl_stockOutput ";
                        StrSql = StrSql + " Where Out_Index =  '" + Out_Index + "'";

                        Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                        StrSql = "Update tbl_SalesItemDetail SET ";
                        StrSql = StrSql + " Send_itemCount1 = Send_itemCount1 - " + ItemCount;
                        StrSql = StrSql + " Where OrderNumber ='" + T_Or + "'";
                        StrSql = StrSql + " And   SalesItemIndex =  " + SalesItemIndex;

                        Temp_Connect.Update_Data(StrSql, Conn, tran);

                    }

                    prB.PerformStep();
                }

                tran.Commit();

                Save_Error_Check = 1;

                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Cancel"));

                if (IsExistsRefundOrder)
                {
                    bool IsMessagwShow = false;
                    DateTime RegisterDate = DateTime.Now;
                    string GetDate = cls_Register.GetRigistryProfile(this.Name, "MessageDate");
                    if (GetDate == "")
                    {
                        GetDate = RegisterDate.ToString("yyyy-MM-dd");
                        cls_Register.SetRegistryProfile(this.Name, "MessageDate", GetDate);

                        IsMessagwShow = true;
                    }
                    else
                    {
                        if (DateTime.TryParse(GetDate, out RegisterDate))
                        {
                            if (DateTime.Now > RegisterDate.AddDays(3))
                            {
                                cls_Register.SetRegistryProfile(this.Name, "MessageDate", DateTime.Now.ToString("yyyy-MM-dd"));

                                IsMessagwShow = true;

                            }
                        }


                    }


                    if (IsMessagwShow)
                    {
                        if (cls_User.gid_CountryCode == "TH")
                        {
                            MessageBox.Show("Confirmed that there is a returned item among the selected items." + Environment.NewLine +
                        "Please note that shipping cannot be canceled if there is a return item for the original order.");
                        }
                        else
                        {


                            MessageBox.Show("선택된 내역중 반품된건이있는것을 확인하였습니다." + Environment.NewLine +
                            "원주문의 반품건이 존재하면 출고취소를 할 수 없는점 참고바랍니다.");
                        }
                    }
                }


            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Cancel_Err"));
            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }

        }






        private void Save_Base_Data(string OrderNumber, int SalesItemIndex, string Out_C_Code, cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            string Out_Date = cls_User.gid_date_time;

            cls_Search_DB csd = new cls_Search_DB();

            string Out_FL = "001";   //'''---주문출고는 001 임                        

            string StrSql = ""; string T_Or = ""; string Out_Index = ""; string Sell_C_Code = "";
            int ItemCnt = 0; string ItemCode = ""; double Out_Price = 0; string T_index = "";
            double Out_Pv = 0;
            int Send_itemCount1 = 0; int itemCount = 0;

            T_Or = OrderNumber;

            StrSql = "Select  tbl_SalesItemDetail.Salesitemindex ,  ItemCount , Send_itemCount1 , ItemCode , ItemPrice,Itempv, ItemTotalPrice, ItemTotalpv  ";
            StrSql = StrSql + " From tbl_SalesItemDetail (nolock) ";
            StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
            StrSql = StrSql + " And  SalesItemIndex =" + SalesItemIndex ;
            StrSql = StrSql + " Order by tbl_SalesItemDetail.SalesItemIndex ASc  ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            for (int i = 0; i <= ReCnt - 1; i++)
            {
                SalesItemIndex = int.Parse(ds.Tables["t_P_table"].Rows[i]["SalesItemIndex"].ToString());

                T_index = cls_User.gid + ' ' + DateTime.UtcNow.ToString();

                //StrSql = "INSERT INTO tbl_Sales_PassNumber ";
                //StrSql = StrSql + " (Pass_Number2,OrderNumber,SalesItemIndex,User_TF,T_Date) ";
                //StrSql = StrSql + " Select ";
                //StrSql = StrSql + " Convert(Varchar,Convert(int, Isnull(Max(Pass_Number2),0)) + 1 )  ";

                //StrSql = StrSql + ",'" + T_Or + "'," + SalesItemIndex + ",1,'" + T_index + "'";
                //StrSql = StrSql + " From tbl_Sales_PassNumber ";

                //Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                StrSql = "INSERT INTO tbl_Sales_PassNumber ";
                StrSql = StrSql + " (Pass_Number2,OrderNumber,SalesItemIndex,User_TF,T_Date) ";
                StrSql = StrSql + " Select ";
                StrSql = StrSql + "right(convert(varchar, getdate(),112),6) " ;
                StrSql = StrSql + " + Right('00000' + convert(varchar(8),convert(float,Right(Isnull(Max(Pass_Number2),0),5)) + 1),5)  ";

                StrSql = StrSql + ",'" + T_Or + "'," + SalesItemIndex + ",1,'" + T_index + "'";
                StrSql = StrSql + " From tbl_Sales_PassNumber (nolock) ";
                StrSql = StrSql + " Where LEFT(Pass_Number2,6) = right(convert(varchar, getdate(),112),6) ";

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);



                StrSql = "Select Top 1  Pass_Number2   ";
                StrSql = StrSql + " From tbl_Sales_PassNumber (nolock) ";
                StrSql = StrSql + " Where  OrderNumber ='" + T_Or + "'";
                StrSql = StrSql + " And   SalesItemIndex = " + SalesItemIndex;
                StrSql = StrSql + " And   T_Date ='" + T_index + "'";
                StrSql = StrSql + " Order by Pass_Number2 DESC ";

                DataSet ds3 = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.

                if (Temp_Connect.Open_Data_Set(StrSql, "t_P_table", ds3) == false) return;
                Out_Index = ds3.Tables["t_P_table"].Rows[0]["Pass_Number2"].ToString();




                ItemCode = ds.Tables["t_P_table"].Rows[i]["ItemCode"].ToString();
                ItemCnt = int.Parse(ds.Tables["t_P_table"].Rows[i]["Send_itemCount1"].ToString());
                Sell_C_Code = Out_C_Code;


                Out_Price = double.Parse(ds.Tables["t_P_table"].Rows[i]["ItemPrice"].ToString());
                Out_Pv = double.Parse(ds.Tables["t_P_table"].Rows[i]["Itempv"].ToString());


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

                StrSql = StrSql + " ,RecordId, RecordTime ";
                StrSql = StrSql + " )";
                StrSql = StrSql + " Values ";
                StrSql = StrSql + " (";
                StrSql = StrSql + "'" + Out_Index + "'";   //입고번호
                StrSql = StrSql + ",'" + Out_FL + "'";   //기타입고 코드 번호
                StrSql = StrSql + ",'" + Out_Date + "'";       //상품코드

                StrSql = StrSql + ",'" + ItemCode + "'";       //상품코드
                StrSql = StrSql + "," + -ItemCnt;      //입고수량
                StrSql = StrSql + "," + -Out_Price;       //단위소매가
                StrSql = StrSql + "," + -Out_Pv;       //단위소매가


                StrSql = StrSql + "," + -Out_Price * ItemCnt;      //총입고금액
                StrSql = StrSql + "," + -Out_Pv * ItemCnt;      //총입고금액

                StrSql = StrSql + ",'" + cls_User.gid + "'";      //입고자
                StrSql = StrSql + ",''";       //비고1
                StrSql = StrSql + ",''";        //비고2

                StrSql = StrSql + ",'C'";   //센타/창고 구분자 c:센타  w:창고


                StrSql = StrSql + ",'" + Sell_C_Code + "'";  //센타/창고 코드 번호

                StrSql = StrSql + "," + -ItemCnt;      //입고수량
                StrSql = StrSql + ",'01'";       //상품코드

                StrSql = StrSql + ",'" + T_Or + "'";       //상품코드
                StrSql = StrSql + "," + SalesItemIndex;      //입고수량
                StrSql = StrSql + ",'" + cls_User.gid + "'";
                StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";

                StrSql = StrSql + ")";

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                StrSql = "Update tbl_SalesItemDetail SET ";
                StrSql = StrSql + " Send_itemCount1 = Send_itemCount1 - " + ItemCnt;
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   SalesItemIndex =  " + SalesItemIndex;

                Temp_Connect.Update_Data(StrSql, Conn, tran);

            }

            StrSql = "Insert into tbl_StockOutput_Not_Union  ";
            StrSql = StrSql + " Select Out_Index From tbl_StockOutput (nolock) ";
            StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
            StrSql = StrSql + " And   SalesItemIndex =  " + SalesItemIndex;
            StrSql = StrSql + " And Out_Index Not in (Select Out_Index From tbl_StockOutput_Not_Union (nolock) ) ";

            Temp_Connect.Update_Data(StrSql, Conn, tran);


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
                combo_Se.SelectedIndex = -1;
                radioB_SellTF2.Checked = true; 
            }

            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                if (Check_TextBox_Error() == false) return;

                txt_P_1.Text = ""; txt_P_2.Text = ""; txt_P_3.Text = "";
                txt_P_4.Text ="";// txt_P_5.Text ="" ;txt_P_6.Text ="";
               // txt_P_7.Text ="";

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
            ct.Search_Date_TextBox_Put(mtxtOutDate, mtxtOutDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }



        private void radioB_R_Base_Click(object sender, EventArgs e)
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









    }
}
