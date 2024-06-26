﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Printing;
using System.Reflection;

namespace MLM_Program
{
    public partial class frmSell_Select_Mem : clsForm_Extends
    {
     

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
        private int print_Page = 0, P_chk_cnt = 0, Print_Row = 0;
        private string P_Ordernumber = "";
        private int Prv_SW = 0;

        cls_Grid_Base cgb = new cls_Grid_Base();

  

        private const string base_db_name = "tbl_SalesDetail";
        private int Data_Set_Form_TF;
        private int Form_Load_TF = 0;

        public delegate void SendNumberDele(string Send_Number, string Send_Name, string Send_OrderNumber);
        public event SendNumberDele Send_Sell_Number;

        public delegate void Send_Mem_NumberDele(string Send_Number, string Send_Name);
        public event Send_Mem_NumberDele Send_Mem_Number;

        private Series series_Item = new Series();


        public frmSell_Select_Mem()
        {
            InitializeComponent();

            DoubleBuffered = true;
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.SetProperty, null, dGridView_Base, new object[] { true });
        }

      


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;
            Form_Load_TF = 0;
           

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_SellCode_ComboBox(combo_Se, combo_Se_Code);

            tabC_1.SelectedIndex = 0;

            //mtxtMbid.Mask = "CCCCC";
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            Reset_Chart_Total();
            Menu_Text_Chang_KR();

            if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
            {
                tabC_1.TabPages.Remove(tab_save);                
            }

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                tabC_1.TabPages.Remove(tab_nom);                
            }


            mtxtSellDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            mtxtMakDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMakDate2.Mask = cls_app_static_var.Date_Number_Fromat;


            txt_P_1.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_3.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_4.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_5.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_6.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_7.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_8.BackColor = cls_app_static_var.txt_Enable_Color;

            txt_SumCnt.BackColor = cls_app_static_var.txt_Enable_Color;

            tabC_1.TabPages.Remove(tab_save);
            tabC_1.TabPages.Remove(tab_nom);

            mtxtMbid.Focus();            
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


        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (Form_Load_TF == 0)
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset(1);
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                mtxtMbid.Focus();
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



        private void Menu_Text_Chang_KR()
        {
            //////메뉴 상에서 들어가는 텍스트들을 알맞게변경을 한다. 외국어 버전을 감안해서 작업한거임.
            //cls_form_Meth cm = new cls_form_Meth();            
            //string m_text = "";

            //for (int Cnt = 0; Cnt <= contextM.Items.Count - 1; Cnt ++)
            //{
            //    m_text = contextM.Items[Cnt].Text.ToString();

            //    if (m_text != "")
            //        contextM.Items[Cnt].Text =  cm._chang_base_caption_search(m_text);
            //}             
            //////메뉴 상에서 들어가는 텍스트들을 알맞게변경을 한다. 외국어 버전을 감안해서 작업한거임.
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



        private Boolean Check_TextBox_Error()
        {
           
            cls_Check_Input_Error c_er = new cls_Check_Input_Error();

            //if (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")               
            //{
            //    int Ret = 0;
            //    Ret = c_er._Member_Nmumber_Split(mtxtMbid);

            //    if (Ret == -1)
            //    {                    
            //        mtxtMbid.Focus();     return false;
            //    }   
            //}


            //if (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
            //{
            //    int Ret = 0;
            //    Ret = c_er._Member_Nmumber_Split(mtxtMbid2);

            //    if (Ret == -1)
            //    {
            //        mtxtMbid2.Focus(); return false;
            //    }   
            //}


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

            //string[] g_HeaderText = {"주문번호"  , "주문_일자"   , "반품_교환_일자"  , "회원_번호"   , "성명"        
            //                    , "주민번호"   , "등록_센타명"    , "주문_센타명"   , "주문_종류"    , "총주문액"
            //                    , "총PV"   , "총결제액"  , "현금"   , "카드"   ,"무통장"
            //                    , "미수금"     , "구분"    , "비고1" , "비고2"     , "기록자"
            //                    , "기록일", ""  , ""  , ""  ,""
            //                    , ""
            //                        };
            cls_form_Meth cm =new cls_form_Meth ();

            Tsql = "Select  ''  ";

            //Tsql = Tsql + " Case When Ga_Order >= 1 Then '" + cm._chang_base_caption_search("미승인") + "'";
            //Tsql = Tsql + "  When Ga_Order = 0 Then '" + cm._chang_base_caption_search("승인") + "'";
            //Tsql = Tsql + " ELSE '' ";
            //Tsql = Tsql + " END SellTFName ";

            Tsql = Tsql + " , tbl_SalesDetail.OrderNumber  ";

            Tsql = Tsql + " , '' ";


            Tsql = Tsql + " , Case ReturnTF When 1 then LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2)   ";
            Tsql = Tsql + "  ELSE (Select LEFT(A1.SellDate,4) +'-' + LEFT(RIGHT(A1.SellDate,4),2) + '-' + RIGHT(A1.SellDate,2) From tbl_SalesDetail AS A1 Where A1.OrderNumber = tbl_SalesDetail.Re_BaseOrderNumber)  END ";


            Tsql = Tsql + " , Case ReturnTF When 1 then '' ELSE  LEFT(SellDate,4) +'-' + LEFT(RIGHT(SellDate,4),2) + '-' + RIGHT(SellDate,2)  END ";

                      
            Tsql = Tsql + ", tbl_SalesDetail.mbid  ";



            Tsql = Tsql + " ,tbl_SalesDetail.M_Name ";
            
            Tsql = Tsql + ", '' ";

            Tsql = Tsql + " ,'' as B_Name";
            Tsql = Tsql + " ,Isnull(S_Bus.Name,'') as S_B_Name";

            Tsql = Tsql + " , '' SellCodeName  ";

            Tsql = Tsql + " ,TotalPrice , Totalpv  " ;
            Tsql = Tsql + " ,TotalInputPrice ";
            Tsql = Tsql + " ,InputCash , InputCard ,InputPassbook , InputMile ";
            Tsql = Tsql + " ,UnaccMoney ";

            //Tsql = Tsql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " ReturnTFName ";

            Tsql = Tsql + " ,Case When ReturnTF = 1 Then '" + cm._chang_base_caption_search("정상") + "'";
            Tsql = Tsql + "  When ReturnTF = 2 Then '" + cm._chang_base_caption_search("반품") + "'";
            Tsql = Tsql + "  When ReturnTF = 4 Then '" + cm._chang_base_caption_search("교환") + "'";
            Tsql = Tsql + "  When ReturnTF = 3 Then '" + cm._chang_base_caption_search("부분반품") + "'";
            Tsql = Tsql + "  When ReturnTF = 5 Then '" + cm._chang_base_caption_search("취소") + "'";
            Tsql = Tsql + " END ReturnTFName ";         

            Tsql = Tsql + " ,tbl_SalesDetail.Etc1 ";
            Tsql = Tsql + " ,tbl_SalesDetail.Etc2 ";

            Tsql = Tsql + " ,tbl_SalesDetail.Recordid ";
            Tsql = Tsql + " ,tbl_SalesDetail.recordtime ";

            Tsql = Tsql + " ,'','','' ";


            Tsql = Tsql + " From tbl_SalesDetail (nolock) ";
           // Tsql = Tsql + " LEFT JOIN tbl_SalesDetail_TF (nolock) ON tbl_SalesDetail.OrderNumber = tbl_SalesDetail_TF.OrderNumber ";
            Tsql = Tsql + " LEFT JOIN tbl_User (nolock) ON tbl_User.User_NCode = tbl_SalesDetail.Mbid  ";            
            Tsql = Tsql + " LEFT JOIN tbl_Business S_Bus (nolock) ON tbl_SalesDetail.BusCode = S_Bus.NCode  And tbl_SalesDetail.Na_code = S_Bus.Na_code ";                        
            
            
        }



        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = " Where tbl_SalesDetail.SellCode = ''  ";
            
            
                        string Mbid = ""; int Mbid2 = 0;
            //회원번호1로 검색
            if (
                (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "") 
                &&
                (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() == "") 
                )
            {

                Mbid = mtxtMbid.Text; 
                //cls_Search_DB csb = new cls_Search_DB();
                //if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
               // {
                    if (Mbid != "")
                        strSql = strSql + " And tbl_SalesDetail.Mbid ='" + Mbid + "'";

                //    if (Mbid2 >= 0)
                //        strSql = strSql + " And tbl_SalesDetail.Mbid2 = " + Mbid2;
                ////}


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


            //기록일자로 검색 -1
            if ((mtxtMakDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And Replace(Left( tbl_SalesDetail.recordtime ,10),'-','') = '" + mtxtMakDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtMakDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And Replace(Left( tbl_SalesDetail.recordtime ,10),'-','') >= '" + mtxtMakDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And Replace(Left( tbl_SalesDetail.recordtime ,10),'-','') <= '" + mtxtMakDate2.Text.Replace("-", "").Trim() + "'";
            }


           

            //센타코드로으로 검색
            //if (txtCenter_Code.Text.Trim() != "")
            //    strSql = strSql + " And tbl_Base_Mem.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";

            if (txtCenter2_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.BusCode = '" + txtCenter2_Code.Text.Trim() + "'";

            if (txtR_Id_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.recordid = '" + txtR_Id_Code.Text.Trim() + "'";


            if (txtSellCode_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.SellCode = '" + txtSellCode_Code.Text.Trim() + "'";

            if (txtOrderNumber.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.OrderNumber = '" + txtOrderNumber.Text.Trim() + "'";

            



            if (opt_sell_2.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.ReturnTF = 1 ";

            if (opt_sell_3.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.ReturnTF = 2 ";

            if (opt_sell_4.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.ReturnTF = 3 ";

            if (opt_sell_5.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.ReturnTF = 4 ";

            if (opt_sell_6.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.ReturnTF = 5 ";



            if (radioB_SellTF2.Checked == true)
                strSql = strSql + " And tbl_SalesDetail_TF.SellTF = 1 ";

            if (radioB_SellTF3.Checked == true)
                strSql = strSql + " And tbl_SalesDetail_TF.SellTF = 0 ";

            

            

            if (opt_Ed_2.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.UnaccMoney = 0 ";

            if (opt_Ed_3.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.UnaccMoney <> 0 ";



            
            strSql = strSql + " And tbl_SalesDetail.BusCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_User.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


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

            double Sum_10 = 0; double Sum_11 = 0; double Sum_12 = 0;
            double Sum_13 = 0; double Sum_14 = 0; double Sum_15 = 0;
            double Sum_16 = 0; double Sum_17 = 0;
            double Sell_Cnt_1 = 0; double Sell_Cnt_2 = 0;
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            Dictionary<string, double> SelType_1 = new Dictionary<string, double>();
            Dictionary<string, double> Center_Pr = new Dictionary<string, double>();
            

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                Sum_10 = Sum_10 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
                Sum_11 = Sum_11 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
                Sum_12 = Sum_12 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalInputPrice"].ToString());
                Sum_13 = Sum_13 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCash"].ToString());
                Sum_14 = Sum_14 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCard"].ToString());
                Sum_15 = Sum_15 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputPassbook"].ToString());
                Sum_16 = Sum_16 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["UnaccMoney"].ToString());
                Sum_17 = Sum_17 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputMile"].ToString());

                //string T_ver = ds.Tables[base_db_name].Rows[fi_cnt]["SellCodeName"].ToString();
                //if (SelType_1.ContainsKey(T_ver) == true)
                //{
                //    SelType_1[T_ver] = SelType_1[T_ver] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());  //금액                    
                //}
                //else
                //{
                //    SelType_1[T_ver] = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());                    
                //}

                //T_ver = ds.Tables[base_db_name].Rows[fi_cnt]["Recordid"].ToString();
                //if (T_ver.Contains("WEB") != true)
                //{
                //    Sell_Cnt_1 = Sell_Cnt_1 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());  //금액                    
                //}
                //else
                //{
                //    Sell_Cnt_2 = Sell_Cnt_2 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());  //금액                    
                //}

                //T_ver = ds.Tables[base_db_name].Rows[fi_cnt]["S_B_Name"].ToString();

                //if (T_ver != "")
                //{
                //    if (Center_Pr.ContainsKey(T_ver) == true)
                //        Center_Pr[T_ver] = Center_Pr[T_ver] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());  //금액                    
                //    else
                //        Center_Pr[T_ver] = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());  //금액                    
                //}

            }

            //Reset_Chart_Total(Sum_13, Sum_14, Sum_15, Sum_17);
            //Reset_Chart_Total(ref SelType_1);
            //Reset_Chart_Total(Sell_Cnt_1, Sell_Cnt_2);


            //foreach (string tkey in Center_Pr.Keys)
            //{
            //    Push_data(series_Item, tkey, Center_Pr[tkey]);
            //}


            if (gr_dic_text.Count > 0)
            {
                txt_P_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_10);
                txt_P_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_11);
                txt_P_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_12);
                txt_P_4.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_13);
                txt_P_5.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_14);
                txt_P_6.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_15);
                txt_P_8.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_17);        
                txt_P_7.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_16);
                txt_SumCnt.Text = string.Format(cls_app_static_var.str_Currency_Type, ReCnt);   
            }
            
            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();

            //dGridView_Base.Rows[0].Frozen = true;
            //dGridView_Base.la
        }



        private void dGridView_Base_Header_Reset()
        {
            
            cgb.grid_col_Count = 27;            
            cgb.basegrid = dGridView_Base;            
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"_선택"  ,"주문번호"  ,"_공제번호" , "주문_일자"   , "교환_반품_일자"      
                                , "직원_ID"     , "성명"  , "_주민번호"   , "_등록_센타명"    , "주문_센타명"     
                                , "_주문_종류"  , "총주문액", "_총PV"   , "총결제액"  , "현금"   
                                , "카드"   , "무통장" , "_마일리지"     , "미수금"    , "구분"     
                                , "비고1"  , "_비고" , "기록자"  , "기록일"  , ""  
                                ,""        , ""
                                    };
            cgb.grid_col_header_text = g_HeaderText;

                int[] g_Width = { 0, 130 , 0, 90, 110
                                , 90, 90   ,0, 0, 130
                                , 0 , 80  ,0 , 80, 80
                                 , 80, 80,0 , 90, 130 
                                 , 130 , 0 ,130 , 100 , 0 
                                 , 0,0
                                };
                cgb.grid_col_w = g_Width;
    

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true  ,true 
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter//5    
  
                               ,DataGridViewContentAlignment.MiddleCenter                            
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft//10

                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  //15   

                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight                            
                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleCenter//20

                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft   
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleCenter  //25   

                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter                                
                              };
            cgb.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[16 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[17 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[18 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[19 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb.grid_cell_format = gr_dic_cell_format;
            
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0].ToString ()
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1].ToString ()
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2].ToString ()
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3].ToString ()
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4].ToString ()
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,encrypter.Decrypt ( ds.Tables[base_db_name].Rows[fi_cnt][7].ToString () ,"Cpno")
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
                    if (R4_name == "Date" || R4_name == "ate3" || R4_name == "ate1" || R4_name == "ate2" ||  R4_name == "ate4")
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
                Data_Set_Form_TF = 1; 
                if (tb.Text.Trim() == "")
                    txtSellCode_Code.Text = "";
                Data_Set_Form_TF = 0; 
            }

            if (tb.Name == "txtR_Id")
            {
                Data_Set_Form_TF = 1; 
                if (tb.Text.Trim() == "")
                    txtR_Id_Code.Text = "";
                Data_Set_Form_TF = 0; 
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
                Data_Set_Form_TF = 1; 
                if (tb.Text.Trim() == "")
                    txtSellCode_Code.Text = "";
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
            //Control tb21 = this.GetNextControl(this.ActiveControl, true);

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
                    if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
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
                    if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
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
                if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
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


            if (tb.Name == "txtCenter2")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
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


                cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
                cgbp5.dGridView_Put_baseinfo(dGridView_Sell_Item, "item_mem");

                cls_Grid_Base_info_Put cgbp6 = new cls_Grid_Base_info_Put();
                cgbp6.dGridView_Put_baseinfo(dGridView_Sell_Cacu, "cacu");

                cls_Grid_Base_info_Put cgbp7 = new cls_Grid_Base_info_Put();
                cgbp7.dGridView_Put_baseinfo(dGridView_Sell_Rece, "rece");

                //cls_Grid_Base_info_Put cgbp10 = new cls_Grid_Base_info_Put();
                //cgbp10.dGridView_Put_baseinfo(dGridView_Up_S, "saveup");

                //cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
                //cgbp11.dGridView_Put_baseinfo(dGridView_Up_N, "nominup");
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                tabC_1.SelectedIndex = 0;


                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtMbid);

                Reset_Chart_Total();
                opt_Ed_1.Checked = true;  opt_sell_1.Checked = true;
                //radioB_S.Checked = true;  radioB_R.Checked = true;
                radioB_SellTF1.Checked = true;
                combo_Se.SelectedIndex = -1;
                
            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                
                cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
                cgbp5.dGridView_Put_baseinfo(dGridView_Sell_Item, "item_mem");

                cls_Grid_Base_info_Put cgbp6 = new cls_Grid_Base_info_Put();
                cgbp6.dGridView_Put_baseinfo(dGridView_Sell_Cacu, "cacu");

                cls_Grid_Base_info_Put cgbp7 = new cls_Grid_Base_info_Put();
                cgbp7.dGridView_Put_baseinfo(dGridView_Sell_Rece, "rece");

                //cls_Grid_Base_info_Put cgbp10 = new cls_Grid_Base_info_Put();
                //cgbp10.dGridView_Put_baseinfo(dGridView_Up_S, "saveup");

                //cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
                //cgbp11.dGridView_Put_baseinfo(dGridView_Up_N, "nominup");
                
                tabC_1.SelectedIndex = 0;
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                if (Check_TextBox_Error() == false) return;

                txt_P_1.Text = ""; txt_P_2.Text = ""; txt_P_3.Text = "";
                txt_P_4.Text =""; txt_P_5.Text ="" ;txt_P_6.Text ="";
                txt_P_7.Text = ""; txt_SumCnt.Text = "";
                combo_Se_Code.SelectedIndex  = combo_Se.SelectedIndex;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Reset_Chart_Total();
                chart_Center.Series.Clear();
                Save_Nom_Line_Chart();   

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
            Excel_Export_File_Name = this.Text; // this.Text; // "Sell_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }

       

        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[1].Value != null))
            {
                string Send_Nubmer = ""; string Send_Name = ""; ; string Send_OrderNumber = "";
                Send_OrderNumber = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
                Send_Nubmer = (sender as DataGridView).CurrentRow.Cells[5].Value.ToString();
                Send_Name = (sender as DataGridView).CurrentRow.Cells[6].Value.ToString();
                Send_Sell_Number(Send_Nubmer, Send_Name, Send_OrderNumber);   //부모한테 이벤트 발생 신호한다.
            }            
        }


        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            //SendKeys.Send("{TAB}");
        }



        private void dGridView_Base_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[1].Value != null))
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                tabC_1.SelectedIndex = 0;

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
                else
                {
                    string T_OrderNumber = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
                    string M_Nubmer = (sender as DataGridView).CurrentRow.Cells[5].Value.ToString();

                    cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
                    cgbp5.dGridView_Put_baseinfo(this, dGridView_Sell_Item, "item_mem", "", T_OrderNumber);

                    cls_Grid_Base_info_Put cgbp6 = new cls_Grid_Base_info_Put();
                    cgbp6.dGridView_Put_baseinfo(this, dGridView_Sell_Cacu, "cacu", "", T_OrderNumber);

                    cls_Grid_Base_info_Put cgbp7 = new cls_Grid_Base_info_Put();
                    cgbp7.dGridView_Put_baseinfo(this, dGridView_Sell_Rece, "rece", "", T_OrderNumber);


                    //cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                    //cgbp.dGridView_Put_baseinfo(this, dGridView_Up_S, "saveup", M_Nubmer);


                    //cls_Grid_Base_info_Put cgbp2 = new cls_Grid_Base_info_Put();
                    //cgbp2.dGridView_Put_baseinfo(this, dGridView_Up_N, "nominup", M_Nubmer);
                }

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

       










        private void Reset_Chart_Total()
        {
            //chart_Mem.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();
            if (cls_app_static_var.Using_Mileage_TF == 1)
            {
                double[] yValues = { 0, 0, 0, 0 };
                string[] xValues = { cm._chang_base_caption_search("현금"), cm._chang_base_caption_search("카드"), cm._chang_base_caption_search("무통장"), cm._chang_base_caption_search("마일리지") };
                chart_Mem.Series["Series1"].Points.DataBindXY(xValues, yValues);
            }
            else
            {
                double[] yValues = { 0, 0, 0 };
                string[] xValues = { cm._chang_base_caption_search("현금"), cm._chang_base_caption_search("카드"), cm._chang_base_caption_search("무통장") };
                chart_Mem.Series["Series1"].Points.DataBindXY(xValues, yValues);
            }
            
            chart_Mem.Series["Series1"].ChartType = SeriesChartType.Pie;
            chart_Mem.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;            
            chart_Mem.Legends[0].Enabled = true;
                        
            string Tsql = "Select SellCode , SellTypeName ";
            Tsql = Tsql + " From tbl_SellType "; 
            Tsql = Tsql + " Order BY SellCode  ";
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_SellType", ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt != 0)
            {             
                double[] yValues_2 = new double[ReCnt] ;
                string[] xValues_2 = new string[ReCnt]; // { cm._chang_base_caption_search(""), cm._chang_base_caption_search("탈퇴") }; 

                 for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    yValues_2[fi_cnt] =  0;
                    xValues_2[fi_cnt] = ds.Tables["tbl_SellType"].Rows[fi_cnt]["SellTypeName"].ToString();                                    
                }
                 
                chart_Leave.Series["Series1"].Points.DataBindXY(xValues_2, yValues_2);                
              
                chart_Leave.Series["Series1"].ChartType = SeriesChartType.Pie;
                chart_Leave.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                chart_Leave.Legends[0].Enabled = true;                               
            }



            double[] yValues_3 = { 0, 0 };
            string[] xValues_3 = { cm._chang_base_caption_search("일반"), cm._chang_base_caption_search("WEB") };
            chart_edu.Series["Series1"].Points.DataBindXY(xValues_3, yValues_3);            
            chart_edu.Series["Series1"].ChartType = SeriesChartType.Pie;
            chart_edu.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_edu.Legends[0].Enabled = true;
           
            chart_Center.Series.Clear();
            series_Item.Points.Clear();
        }



        private void Reset_Chart_Total(double SellCnt_1, double SellCnt_2, double SellCnt_3, double SellCnt_4)
        {
            //chart_Mem.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();
            Series series_Save = new Series();

            chart_Mem.Series.Clear();
            chart_Mem.Series.Add(series_Save);

            DataPoint dp = new DataPoint();
            series_Save.ChartType = SeriesChartType.Pie;
            dp.SetValueXY(cm._chang_base_caption_search("현금"), SellCnt_1);
            dp.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_1);            
            dp.LabelForeColor = Color.Black;
            dp.LegendText = cm._chang_base_caption_search("현금");
            series_Save.Points.Add(dp);

            DataPoint dp2 = new DataPoint();

            dp2.SetValueXY(cm._chang_base_caption_search("카드"), SellCnt_2);
            dp2.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_2);            
            dp2.LabelForeColor = Color.Black;
            dp2.LegendText = cm._chang_base_caption_search("카드");
            series_Save.Points.Add(dp2);


            DataPoint dp3 = new DataPoint();

            dp3.SetValueXY(cm._chang_base_caption_search("무통장"), SellCnt_3);
            dp3.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_3);
            dp3.LabelForeColor = Color.Black;
            dp3.LegendText = cm._chang_base_caption_search("무통장");
            series_Save.Points.Add(dp3);

            if (cls_app_static_var.Using_Mileage_TF == 1)
            {
                DataPoint dp4 = new DataPoint();
                dp4.SetValueXY(cm._chang_base_caption_search("마일리지"), SellCnt_4);
                dp4.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_4);
                dp4.LabelForeColor = Color.Black;
                dp4.LegendText = cm._chang_base_caption_search("마일리지");
                series_Save.Points.Add(dp4);
            }
            
           
        }

        private void Reset_Chart_Total(ref Dictionary<string, double> SelType_1)
        {

            cls_form_Meth cm = new cls_form_Meth();
            Series series_Save = new Series();

            chart_Leave.Series.Clear();
            chart_Leave.Series.Add(series_Save);
            int forCnt = 0;
            foreach (string tkey in SelType_1.Keys)
            {
                DataPoint dp = new DataPoint();
                series_Save.ChartType = SeriesChartType.Pie;
                dp.SetValueXY(tkey, SelType_1[tkey]);
                dp.Label = string.Format(cls_app_static_var.str_Currency_Type, SelType_1[tkey]);                                              
                dp.LabelForeColor = Color.Black;
                dp.LegendText = tkey;
                series_Save.Points.Add(dp);
                forCnt++;
            }           
           
            chart_Leave.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_Leave.Legends[0].Enabled = true;
        }


        private void Reset_Chart_Total(double SellCnt_1, double SellCnt_2)
        {
            //chart_edu.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();
            Series series_Save = new Series();

            chart_edu.Series.Clear();
            chart_edu.Series.Add(series_Save);

            DataPoint dp = new DataPoint();
            series_Save.ChartType = SeriesChartType.Pie;
            dp.SetValueXY(cm._chang_base_caption_search("일반"), SellCnt_1);
            dp.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_1);            
            dp.LabelForeColor = Color.Black;
            dp.LegendText = cm._chang_base_caption_search("일반");
            series_Save.Points.Add(dp);

            DataPoint dp2 = new DataPoint();

            dp2.SetValueXY(cm._chang_base_caption_search("WEB"), SellCnt_2);
            dp2.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_2);            
            dp2.LabelForeColor = Color.Black;
            dp2.LegendText = cm._chang_base_caption_search("WEB");
            series_Save.Points.Add(dp2);

         
            chart_edu.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_edu.Legends[0].Enabled = true;
        }
        


        private void Push_data(Series series, string p, double p_3)
        {
            if (p != "")
            {
                DataPoint dp = new DataPoint();

                if (p.Replace(" ", "").Length >= 5)
                    dp.SetValueXY(p.Replace(" ", "").Substring(0, 5), p_3);
                else
                    dp.SetValueXY(p.Replace(" ", ""), p_3);

                dp.Font = new System.Drawing.Font("맑은고딕", 9);
                dp.Label = string.Format(cls_app_static_var.str_Currency_Type, p_3);
                series.Points.Add(dp);
            }
        }

        
        
        private void Save_Nom_Line_Chart()
        {
            cls_form_Meth cm = new cls_form_Meth();

            chart_Center.Series.Clear();
            series_Item.Points.Clear();
            
            series_Item["DrawingStyle"] = "Emboss";
            series_Item["PointWidth"] = "0.4";
            series_Item.Name = cm._chang_base_caption_search("매출액");
                                    
            series_Item.ChartType = SeriesChartType.Column ;
            
            chart_Center.Series.Add(series_Item);            
            chart_Center.ChartAreas[0].AxisX.Interval = 1;
            chart_Center.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_Center.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 8;
            //chart_Center.ChartAreas[0].AxisY.Interval = 5000000;

            chart_Center.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;            
            chart_Center.Legends[0].Enabled = true;

        }


        private void radioB_S_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtSellDate1, mtxtSellDate2, (RadioButton)sender);
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


        private void MenuItem_Base_Click(object sender, EventArgs e)
        {

        }

        private int but_Exp_Base_Left = 0;
        private int Parent_but_Exp_Base_Width = 0;

        private void but_Exp_Click(object sender, EventArgs e)
        {
            if (but_Exp.Text == "<<")
            {
                Parent_but_Exp_Base_Width = but_Exp.Parent.Width;
                but_Exp_Base_Left = but_Exp.Left;

                but_Exp.Parent.Width = but_Exp.Width;
                but_Exp.Left = 0;
                but_Exp.Text = ">>";
            }
            else
            {
                but_Exp.Parent.Width = Parent_but_Exp_Base_Width;
                but_Exp.Left = but_Exp_Base_Left;
                but_Exp.Text = "<<";
            }
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
            cls_Search_DB csd = new cls_Search_DB();
            string In_Date = "", Sell_C_Code = "";

            for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    chk_cnt++;
                    In_Date = dGridView_Base.Rows[i].Cells[1].Value.ToString();
                    Sell_C_Code = dGridView_Base.Rows[i].Cells[21].Value.ToString();
                    //if (csd.Check_Stock_Close(Sell_C_Code, In_Date) == false)
                    //{
                    //    butt_S_Save.Focus(); return false;
                    //}
                }
            }

            if (chk_cnt == 0) //저장할 내역이 없을을 알린다.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridView_Base.Focus(); return false;
            }

            P_chk_cnt = chk_cnt;
            prB.Maximum = P_chk_cnt + 2;

            for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    if (Print_Row == 0)
                    {
                        Print_Row = i;
                        break;
                    }
                }
            }

            return true;
        }




        private void butt_S_Save_Click(object sender, EventArgs e)        
        {

            Button bt = (Button)sender;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            if (bt.Name == "butt_S_check")
            {
                //dGridView_Base.Visible = false;
                for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
                {
                    dGridView_Base.Rows[i].Cells[0].Value = "V";
                }
                // dGridView_Base.Visible = true;
            }


            else if (bt.Name == "butt_S_Not_check")
            {
               // dGridView_Base.Visible = false;
                for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
                {
                    dGridView_Base.Rows[i].Cells[0].Value = "";
                }
                //dGridView_Base.Visible = true;
            }

            else if (bt.Name == "butt_S_Save")
            {
                P_chk_cnt = 0; Print_Row = 0;

                if (Sub_Check_TextBox_Error() == false) return;

                //Save_Base_Data(ref Save_Error_Check);                               
                print_Page = 0;
                Print_Row = 0; 
                prB.Visible = false; butt_S_Save.Enabled = false;
                butt_Print_Click();  //출력이 이루어지는 곳임.
                prB.Visible = false; butt_S_Save.Enabled = true;
            
            }            
            this.Cursor = System.Windows.Forms.Cursors.Default;


        }

        private void butt_Print_Click()
        {
            print_Page = 0;
            Print_Row = 0;
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
            // {
            TPrint_Row = Print_Row + 1;

            for (int i = TPrint_Row; i <= dGridView_Base.Rows.Count - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    Print_Row = i;
                    break;
                }
            }
            //}

            P_Ordernumber = dGridView_Base.Rows[Print_Row].Cells[1].Value.ToString();  //주문번호를 가져온다.



            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();

            string Strsql = "Select tbl_SalesDetail.OrderNumber ,tbl_SalesDetail.Mbid , SellDate ,InsuranceNumber , tbl_SalesDetail.ABC_Price  , tbl_SalesDetail.ABC_Price_2 ";
            Strsql = Strsql + ", tbl_SalesDetail.M_Name,  sangho , number , S_name , tbl_Tax.phone , ISNULL(C1.Grade_Name,'') CC_Grade ";
            Strsql = Strsql + ", InputCard ,InputPassbook,InputPassbook2, InputCash ,( InputCard + InputPassbook + InputPassbook2 + InputCash) TotalInputPrice ";
            Strsql = Strsql + ",Replace(LEFT(tbl_SalesDetail.RecordTime ,10) ,'-','') AS  R_Date ";
            Strsql = Strsql + " , Isnull(tbl_User.U_name,'') Last_Con_ID ";
            Strsql = Strsql + " From tbl_SalesDetail (nolock) ";
            Strsql = Strsql + " LEFT JOIN tbl_Tax (nolock) ON tbl_SalesDetail.Mbid = tbl_Tax.Mbid And tbl_Tax.Main_TF = 0 ";
            Strsql = Strsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_SalesDetail.Mbid = tbl_Memberinfo.Mbid ";
            Strsql = Strsql + " Left Join tbl_Class C1 On tbl_Memberinfo.CurGrade=C1.Grade_Cnt ";
            Strsql = Strsql + " Left Join tbl_User  On tbl_SalesDetail.Last_Con_ID =  tbl_User.User_id ";
            Strsql = Strsql + " Where tbl_SalesDetail.OrderNumber ='" + P_Ordernumber + "'";

            DataSet ds = new DataSet();
            Temp_Connect.Open_Data_Set(Strsql, "t_table", ds);

            int ReCnt = Temp_Connect.DataSet_ReCount;





            int Last_Line = 0;

            BaseDoc_PrintPage____001(e, ref t_f, ref tt, ref BaseitemH, ref BaseitemH2, ref BaseitemH3, Y_tGap, ds, ref Last_Line);
            BaseDoc_PrintPage____002(e, t_f, BaseitemH2, BaseitemH3, Y_tGap, ds, ref Last_Line);
            BaseDoc_PrintPage____003(e, t_f, BaseitemH2, BaseitemH3, Y_tGap, ds);
            BaseDoc_PrintPage____004(e, t_f, BaseitemH2, BaseitemH3, Y_tGap, ds);
            BaseDoc_PrintPage____005(e, t_f, BaseitemH2, BaseitemH3, Y_tGap, ds);


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
          , DataSet ds, ref int Last_Line)
        {

            string msg = "";
            string ABC_Price = "회원가";
            
            if (ds.Tables["t_table"].Rows[0]["ABC_Price"].ToString() != "")
                ABC_Price = ds.Tables["t_table"].Rows[0]["ABC_Price"].ToString() + "가";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price"].ToString() == "Nor")
                ABC_Price = "회원가";


            if (ds.Tables["t_table"].Rows[0]["ABC_Price"].ToString() == "BB")
                ABC_Price = "비품특판";

            

            if (ds.Tables["t_table"].Rows[0]["ABC_Price"].ToString() == "D")
                ABC_Price = "덕용";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price"].ToString() == "R")
                ABC_Price = "로드샵";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price"].ToString() == "100")
                ABC_Price = "100만RV";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price"].ToString() == "500")
                ABC_Price = "500만RV";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price"].ToString() == "1000")
                ABC_Price = "1000만RV";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price"].ToString() == "3000")
                ABC_Price = "3000만원";

            string ABC_Price_2 = "";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price_2"].ToString() != "")
                ABC_Price_2 = ds.Tables["t_table"].Rows[0]["ABC_Price_2"].ToString() + "가";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price_2"].ToString() == "Nor")
                ABC_Price_2 = "회원가";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price_2"].ToString() == "BB")
                ABC_Price_2 = "비품특판";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price_2"].ToString() == "D")
                ABC_Price_2 = "덕용";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price_2"].ToString() == "R")
                ABC_Price_2 = "로드샵";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price_2"].ToString() == "100")
                ABC_Price_2 = "100만RV";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price_2"].ToString() == "500")
                ABC_Price_2 = "500만RV";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price_2"].ToString() == "1000")
                ABC_Price_2 = "1000만RV";

            if (ds.Tables["t_table"].Rows[0]["ABC_Price_2"].ToString() == "3000")
                ABC_Price_2 = "3000만원";

            if (ABC_Price_2 != "")
                ABC_Price = ABC_Price + " / " + ABC_Price_2 ;


            Pen T_p = new Pen(Color.Black);
            int pageW = e.PageBounds.Width - 20, pageH = e.PageBounds.Height;
            int X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;

            //글자 크기 10을  높이 20으로 잡으면될듯함.
            int plus_g = 0;

            if (Y_tGap > 0)
                plus_g = 35;

            //거래명세표 글자를 찍는다.
            tt.X = (pageW / 2) - 110;
            tt.Y = 25 + Y_tGap - plus_g;
            msg = "거래명세서 < " + ABC_Price + " >";
            FontStyle fs = FontStyle.Bold;
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 18, fs), Brushes.Black, tt);


            tt.X = (pageW / 2) - 70;
            tt.Y = 55 + Y_tGap - plus_g;
            msg = "(공급받는자 보관용)";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);



            tt.X = (pageW / 2) + 180;
            tt.Y = 55 + Y_tGap - plus_g;
            msg = "□ 본사출고";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = (pageW / 2) + 280;
            tt.Y = 55 + Y_tGap - plus_g;
            msg = "□ 택배수령";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);



            tt.X = 25;
            tt.Y = 55 + Y_tGap - plus_g;
            msg = "주문일자:" + ds.Tables["t_table"].Rows[0]["R_Date"].ToString(); ;
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = 25;
            tt.Y = 75 + Y_tGap - plus_g;
            msg = "처리일자:" + ds.Tables["t_table"].Rows[0]["SellDate"].ToString(); ;
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = (pageW / 2) - 160;
            tt.Y = 75 + Y_tGap - plus_g;
            msg = "주문번호:" + P_Ordernumber;
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = (pageW / 2) + 100;
            tt.Y = 75 + Y_tGap - plus_g;
            msg = "공제번호:" + ds.Tables["t_table"].Rows[0]["InsuranceNumber"].ToString();
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            //프린터물 가장 테두리선을 그린다.



            t_f.X = 20;
            if (Y_tGap == 0)
                t_f.Y = 20 + Y_tGap;
            else
                t_f.Y = 20 + Y_tGap - plus_g;




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


            int Base_Line = 18, Base_Font_H = 9;



            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();

            string Strsql = "Select tbl_SalesItemDetail.itemCode, tbl_Goods.Name AS GGName , tbl_SalesItemDetail.itemCount ,itemPrice ,itemTotalPrice";
            Strsql = Strsql + " From tbl_SalesItemDetail (nolock) ";
            Strsql = Strsql + " LEFT JOIN tbl_Goods (nolock) ON tbl_SalesItemDetail.ItemCode = tbl_Goods.Ncode ";
            Strsql = Strsql + " LEFT  join tbl_Goods_Prom_Sub (nolock) on tbl_salesitemdetail.prom_TF2 = tbl_Goods_Prom_Sub.base_Sub_TF  And  tbl_salesitemdetail.itemcode = tbl_Goods_Prom_Sub.itemcode ";
            Strsql = Strsql + " Where tbl_SalesItemDetail.OrderNumber ='" + P_Ordernumber + "'";
            Strsql = Strsql + " And  (tbl_SalesItemDetail.Prom_TF = '' OR (tbl_SalesItemDetail.Prom_TF = 'P' And ISNULL(tbl_Goods_Prom_Sub.Prom_TF ,'') = ''  )) ";
            Strsql = Strsql + " Order by tbl_SalesItemDetail.Salesitemindex ASC "; 

            DataSet ds2 = new DataSet();
            Temp_Connect.Open_Data_Set(Strsql, "t_Item_table", ds2);

            int ReCnt = Temp_Connect.DataSet_ReCount;


            Strsql = "Select tbl_SalesItemDetail.itemCode, tbl_Goods.Name AS GGName , Sum(tbl_SalesItemDetail.itemCount) AS itemCount ";
            Strsql = Strsql + " From tbl_SalesItemDetail (nolock) ";
            Strsql = Strsql + " LEFT JOIN tbl_Goods (nolock) ON tbl_SalesItemDetail.ItemCode = tbl_Goods.Ncode ";
            Strsql = Strsql + " LEFT  join tbl_Goods_Prom_Sub (nolock) on tbl_salesitemdetail.prom_TF2 = tbl_Goods_Prom_Sub.base_Sub_TF  And  tbl_salesitemdetail.itemcode = tbl_Goods_Prom_Sub.itemcode ";
            Strsql = Strsql + " Where tbl_SalesItemDetail.OrderNumber ='" + P_Ordernumber + "'";
            Strsql = Strsql + " And  tbl_SalesItemDetail.Prom_TF = 'P' ";
            Strsql = Strsql + " And  ISNULL(tbl_Goods_Prom_Sub.Prom_TF ,'') = 'P' ";
            Strsql = Strsql + " And  Prom_TF2 <> 'Ax3' ";
            Strsql = Strsql + " And  Prom_TF2 <> 'All' ";
            Strsql = Strsql + " Group by tbl_SalesItemDetail.itemCode, tbl_Goods.Name  ";

            DataSet ds3 = new DataSet();
            Temp_Connect.Open_Data_Set(Strsql, "t_Item_table2", ds3);

            int ReCnt3 = Temp_Connect.DataSet_ReCount;


            Strsql = "Select tbl_SalesItemDetail.itemCode, tbl_Goods.Name AS GGName , Sum(tbl_SalesItemDetail.itemCount) AS itemCount ";
            Strsql = Strsql + " From tbl_SalesItemDetail (nolock) ";
            Strsql = Strsql + " LEFT JOIN tbl_Goods (nolock) ON tbl_SalesItemDetail.ItemCode = tbl_Goods.Ncode ";
            Strsql = Strsql + " LEFT  join tbl_Goods_Prom_Sub (nolock) on tbl_salesitemdetail.prom_TF2 = tbl_Goods_Prom_Sub.base_Sub_TF  And  tbl_salesitemdetail.itemcode = tbl_Goods_Prom_Sub.itemcode ";
            Strsql = Strsql + " Where tbl_SalesItemDetail.OrderNumber ='" + P_Ordernumber + "'";
            Strsql = Strsql + " And  tbl_SalesItemDetail.Prom_TF = 'P' ";
            Strsql = Strsql + " And  ISNULL(tbl_Goods_Prom_Sub.Prom_TF ,'') = 'P' ";
            Strsql = Strsql + " And  Prom_TF2 = 'Ax3' ";
            Strsql = Strsql + " Group by tbl_SalesItemDetail.itemCode, tbl_Goods.Name  ";

            DataSet ds4 = new DataSet();
            Temp_Connect.Open_Data_Set(Strsql, "t_Item_table2", ds4);

            int ReCnt4 = Temp_Connect.DataSet_ReCount;

            if (ReCnt3 == 0 && ReCnt4 > 0)
                ReCnt3 = 5;


            int ReCnt18 = 0;


            Cnt = 0;

            if (ReCnt >= 15)
            {
                if (ReCnt3 <= 1)
                    ReCnt18 = ReCnt + 4;
                else
                    ReCnt18 = ReCnt + ReCnt3 + 2;

                while (Cnt <= ReCnt + 1)
                {
                    X1 = t_f.X; X2 = pageW - t_f.X;
                    Y1 = BaseitemH + (Base_Line * Cnt);
                    Y2 = BaseitemH + (Base_Line * Cnt);
                    e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

                    BaseitemH3 = BaseitemH + (Base_Line * Cnt);
                    Cnt++;
                }
            }
            else
            {
                //ReCnt18 = 22;

                if (ReCnt3 <= 1)
                    ReCnt18 = 18;
                else
                    ReCnt18 = 15 + ReCnt3 + 2;

                while (Cnt <= 15)
                {
                    X1 = t_f.X; X2 = pageW - t_f.X;
                    Y1 = BaseitemH + (Base_Line * Cnt);
                    Y2 = BaseitemH + (Base_Line * Cnt);
                    e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

                    BaseitemH3 = BaseitemH + (Base_Line * Cnt);
                    Cnt++;
                }
            }


            Last_Line = 0;
            while (Cnt <= ReCnt18)
            {
                X1 = t_f.X; X2 = pageW - t_f.X;
                Y1 = BaseitemH + (Base_Line * Cnt);
                Y2 = BaseitemH + (Base_Line * Cnt);
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

                Last_Line = BaseitemH + (Base_Line * Cnt);
                Cnt++;
            }


            t_f.Height = Last_Line; //((pageH - (20 * 2)) / 2) - 40;
            t_f.Width = pageW - (t_f.X * 2);
            e.Graphics.DrawRectangle(T_p, t_f);




            double Sum_Item_cnt = 0, Sum_ItemPr = 0, Sum_ItemTotalPr = 0;
            int fi_cnt = 2, item_Base_Gap = 4;

            for (int fi_cnt22 = 0; fi_cnt22 <= ReCnt - 1; fi_cnt22++)
            {
                tt.X = 25;
                tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                msg = (fi_cnt22 + 1).ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


                tt.X = 70;
                tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                msg = ds2.Tables["t_Item_table"].Rows[fi_cnt22]["itemCode"].ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


                tt.X = 135;
                tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                msg = ds2.Tables["t_Item_table"].Rows[fi_cnt22]["GGName"].ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


                tt.X = (pageW / 2) + 155;
                tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                //msg = ds2.Tables["t_Item_table"].Rows[fi_cnt22]["itemPrice"].ToString();
                msg = string.Format(cls_app_static_var.str_Currency_Type, ds2.Tables["t_Item_table"].Rows[fi_cnt22]["itemPrice"]);
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);



                tt.X = (pageW / 2) + 240;
                tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                msg = ds2.Tables["t_Item_table"].Rows[fi_cnt22]["itemCount"].ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
                //Sum_Item_cnt = Sum_Item_cnt + int.Parse(ds2.Tables["t_Item_table"].Rows[fi_cnt22]["itemCount"].ToString());


                tt.X = (pageW / 2) + 285;
                tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                //msg = ds2.Tables["t_Item_table"].Rows[fi_cnt22]["itemPrice"].ToString();
                msg = string.Format(cls_app_static_var.str_Currency_Type, ds2.Tables["t_Item_table"].Rows[fi_cnt22]["itemTotalPrice"]);
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

                //tt.X = (pageW - 320);
                //tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                //msg = string.Format(cls_app_static_var.str_Currency_Type, SalesItemDetail[t_key].ItemPrice);
                //e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
                //Sum_ItemPr = Sum_ItemPr + SalesItemDetail[t_key].ItemPrice;

                //tt.X = (pageW - 150);
                //tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                //msg = string.Format(cls_app_static_var.str_Currency_Type, SalesItemDetail[t_key].ItemTotalPrice);
                //e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
                //Sum_ItemTotalPr = Sum_ItemTotalPr + SalesItemDetail[t_key].ItemTotalPrice;

                fi_cnt++;
            }


            int Base_Font_H_2 = Base_Font_H - 5;


            tt.X = 25;
            tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = "일련";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);


            tt.X = 70;
            tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = "코드";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);


            tt.X = (pageW / 2) - 120;
            tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = "상품명";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);


            tt.X = (pageW / 2) + 170;
            tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = "단가";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);


            tt.X = (pageW / 2) + 240;
            tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = "수량";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);


            tt.X = (pageW / 2) + 320;
            tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = "계";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);


            //일련 번호 뒷선
            X1 = 65;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH3;//t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            //코드 뒷선
            X1 = 130;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH3;// t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //단가 앞선
            X1 = (pageW / 2) + 150;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH3; //t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //단가 뒷선
            X1 = (pageW / 2) + 230;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH3; //t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            //수량 뒷선
            X1 = (pageW / 2) + 280;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH3; //t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            


            



            fi_cnt = 4;

            for (int fi_cnt22 = 0; fi_cnt22 <= ReCnt3 - 1; fi_cnt22++)
            {
                tt.X = 25;
                tt.Y = BaseitemH3 + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                msg = (fi_cnt22 + 1).ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


                if (ReCnt4 > 0)
                {
                    tt.X = 90;
                    tt.Y = BaseitemH3 + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    msg = "박스가Ax3 프로모션";
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


                    tt.X = (pageW / 2) - 150;
                    tt.Y = BaseitemH3 + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    if (fi_cnt22 == 0) msg = "화장품 11종";
                    if (fi_cnt22 == 1) msg = "리톡스마스크";
                    if (fi_cnt22 == 2) msg = "비타민C 3종 세트";
                    if (fi_cnt22 == 3) msg = "피토스템 앰플";
                    if (fi_cnt22 == 4) msg = "에어테라피 키트";
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

                    tt.X = (pageW / 2) + 185;
                    tt.Y = BaseitemH3 + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    if (fi_cnt22 == 0) msg = "2 Set";
                    if (fi_cnt22 == 1) msg = "2 EA";
                    if (fi_cnt22 == 2) msg = "1 Set";
                    if (fi_cnt22 == 3) msg = "1 Set";
                    if (fi_cnt22 == 4) msg = "1 EA";
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
                }
                else
                {
                    tt.X = 90;
                    tt.Y = BaseitemH3 + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    msg = "현금프로모션";
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


                    tt.X = (pageW / 2) - 150;
                    tt.Y = BaseitemH3 + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    msg = ds3.Tables["t_Item_table2"].Rows[fi_cnt22]["GGName"].ToString();
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

                    tt.X = (pageW / 2) + 185;
                    tt.Y = BaseitemH3 + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    msg = ds3.Tables["t_Item_table2"].Rows[fi_cnt22]["itemCount"].ToString();
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
                }

                //tt.X = 90;
                //tt.Y = BaseitemH3 + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                //msg = "현금프로모션";
                //e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


                //tt.X = (pageW / 2) - 150;
                //tt.Y = BaseitemH3 + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                //msg = ds3.Tables["t_Item_table2"].Rows[fi_cnt22]["GGName"].ToString();
                //e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

                //tt.X = (pageW / 2) + 185;
                //tt.Y = BaseitemH3 + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                //msg = ds3.Tables["t_Item_table2"].Rows[fi_cnt22]["itemCount"].ToString();
                //e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

                fi_cnt++;
            }



            tt.X = (pageW / 2) - 20;
            tt.Y = BaseitemH3 + (Base_Line) + Base_Font_H_2;
            msg = "프로모션";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);


            tt.X = 25;
            tt.Y = BaseitemH3 + (Base_Line * 2) + Base_Font_H_2;
            msg = "일련";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);


            tt.X = 115;
            tt.Y = BaseitemH3 + (Base_Line * 2) + Base_Font_H_2;
            msg = "프로모션명";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);


            tt.X = (pageW / 2) + 5;
            tt.Y = BaseitemH3 + (Base_Line * 2) + Base_Font_H_2;
            msg = "상품명";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = (pageW / 2) + 195;
            tt.Y = BaseitemH3 + (Base_Line * 2) + Base_Font_H_2;
            msg = "수량";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);


            tt.X = (pageW / 2) + 250;
            tt.Y = BaseitemH3 + (Base_Line * 2) + Base_Font_H_2;
            msg = "비고";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);



            if (ReCnt3 <= 1)
            {
                X1 = 65;
                X2 = X1;
                Y1 = BaseitemH3 + (Base_Line * 2);
                Y2 = BaseitemH3 + (Base_Line * 4);//t_f.Y + t_f.Height;
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


                X1 = 250;
                X2 = X1;
                Y1 = BaseitemH3 + (Base_Line * 2);
                Y2 = BaseitemH3 + (Base_Line * 4);//t_f.Y + t_f.Height;
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);




                X1 = (pageW / 2) + 180;
                X2 = X1;
                Y1 = BaseitemH3 + (Base_Line * 2);
                Y2 = BaseitemH3 + (Base_Line * 4);//t_f.Y + t_f.Height;
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


                X1 = (pageW / 2) + 245;
                X2 = X1;
                Y1 = BaseitemH3 + (Base_Line * 2);
                Y2 = BaseitemH3 + (Base_Line * 4);//t_f.Y + t_f.Height;
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);
            }
            else
            {
                int TTCnt = Base_Line * (ReCnt3 + 3);

                X1 = 65;
                X2 = X1;
                Y1 = BaseitemH3 + (Base_Line * 2);
                Y2 = BaseitemH3 + TTCnt;//t_f.Y + t_f.Height;
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


                X1 = 250;
                X2 = X1;
                Y1 = BaseitemH3 + (Base_Line * 2);
                Y2 = BaseitemH3 + TTCnt;//t_f.Y + t_f.Height;
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);




                X1 = (pageW / 2) + 180;
                X2 = X1;
                Y1 = BaseitemH3 + (Base_Line * 2);
                Y2 = BaseitemH3 + TTCnt;//t_f.Y + t_f.Height;
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


                X1 = (pageW / 2) + 245;
                X2 = X1;
                Y1 = BaseitemH3 + (Base_Line * 2);
                Y2 = BaseitemH3 + TTCnt;//t_f.Y + t_f.Height;
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);
            }
            ////tt.X = (pageW - 150);
            ////tt.Y = BaseitemH2 + (Base_Line) + Base_Font_H_2;
            ////msg = "회원가합";
            ////e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);



            ////tt.X = 30; tt.Y = BaseitemH3 + Base_Font_H - 5;
            ////msg = "인수자";
            ////e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            ////tt.X = 300; tt.Y = BaseitemH3 + Base_Font_H - 5;
            ////msg = "인";
            ////e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 9), Brushes.Black, tt);

            ////tt.X = 360; tt.Y = BaseitemH3 + Base_Font_H - 5;
            ////msg = "합계";
            ////e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);



            //tt.X = (pageW / 2);
            //tt.Y = BaseitemH3 + Base_Font_H - 5;
            //msg = string.Format(cls_app_static_var.str_Currency_Type, Sum_Item_cnt);
            //e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            //tt.X = (pageW - 320);
            //tt.Y = BaseitemH3 + Base_Font_H - 5;
            //msg = string.Format(cls_app_static_var.str_Currency_Type, Sum_ItemPr);
            //e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            //tt.X = (pageW - 150);
            //tt.Y = BaseitemH3 + Base_Font_H - 5;
            //msg = string.Format(cls_app_static_var.str_Currency_Type, Sum_ItemTotalPr);
            //e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
        }



        private void BaseDoc_PrintPage____002(System.Drawing.Printing.PrintPageEventArgs e, Rectangle t_f, int BaseitemH2, int BaseitemH3, int Y_tGap
            , DataSet ds, ref int Last_Line)
        {
            //RectangleF tt = new RectangleF();

            //string msg = "";
            Pen T_p = new Pen(Color.Black);
            int pageW = e.PageBounds.Width - 20, pageH = e.PageBounds.Height;
            int X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;

            RectangleF tt = new RectangleF();



            string msg = "";
            int Base_Font_H = 5;



            int Base_Line = 20;

            


            //X1 = 65;
            //X2 = X1;
            //Y1 = BaseitemH3 + (Base_Line * 2);
            //Y2 = BaseitemH3 + (Base_Line * 8);//t_f.Y + t_f.Height;
            //e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //X1 = 250;
            //X2 = X1;
            //Y1 = BaseitemH3 + (Base_Line * 2);
            //Y2 = BaseitemH3 + (Base_Line * 8);//t_f.Y + t_f.Height;
            //e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);




            //X1 = (pageW / 2) + 180;
            //X2 = X1;
            //Y1 = BaseitemH3 + (Base_Line * 2);
            //Y2 = BaseitemH3 + (Base_Line * 8);//t_f.Y + t_f.Height;
            //e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //X1 = (pageW / 2) + 245;
            //X2 = X1;
            //Y1 = BaseitemH3 + (Base_Line * 2);
            //Y2 = BaseitemH3 + (Base_Line * 8);//t_f.Y + t_f.Height;
            //e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);



            ////==============================여기서 부터 결제 관련 상자들 ============================================////
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();

            string Strsql = "Select tbl_Sales_Cacu.* , Isnull(tbl_Bank.bankname,'') BB_Name , Isnull(BB2.bankname,'') BB2_Name  , Isnull(tbl_Card.cardname,'') Ca_Name  ";
            Strsql = Strsql + " From tbl_Sales_Cacu (nolock) ";
            Strsql = Strsql + " LEFT JOIN tbl_Card (nolock) ON tbl_Sales_Cacu.C_code = tbl_Card.Ncode ";
            Strsql = Strsql + " LEFT JOIN tbl_Bank (nolock) ON tbl_Sales_Cacu.C_code = Right(tbl_Bank.Ncode,2) And len(tbl_Sales_Cacu.C_code) = 2 ";
            cls_NationService.SQL_BankNationCode("tbl_Bank", ref Strsql);
            Strsql = Strsql + " LEFT JOIN tbl_Bank BB2 (nolock) ON tbl_Sales_Cacu.C_code = tbl_Bank.Ncode And len(tbl_Sales_Cacu.C_code) = 3 ";
            cls_NationService.SQL_BankNationCode("BB2", ref Strsql);
            Strsql = Strsql + " Where tbl_Sales_Cacu.OrderNumber ='" + P_Ordernumber + "'";
            Strsql = Strsql + " And C_Price1 > 0 "; 

            DataSet ds2 = new DataSet();
            Temp_Connect.Open_Data_Set(Strsql, "t_Cacu_table", ds2);

            int ReCnt_C = Temp_Connect.DataSet_ReCount;

            int ReCnt_Cacu = ReCnt_C;

            if (ReCnt_Cacu < 2)
                ReCnt_Cacu = 2;

            ReCnt_Cacu = ReCnt_Cacu + 2;

            int Cacu_Bax = Last_Line + Base_Line + Base_Line;
            int Cacu_Cnt = 0;

            while (Cacu_Cnt <= ReCnt_Cacu)
            {
                X1 = 20; X2 = pageW - t_f.X;
                Y1 = Cacu_Bax + (Base_Line * Cacu_Cnt);
                Y2 = Cacu_Bax + (Base_Line * Cacu_Cnt);
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

                Cacu_Cnt++;
            }

            Rectangle tT_f2 = new Rectangle();
            tT_f2.X = 20;
            tT_f2.Y = Last_Line + (Base_Line * 2);
            tT_f2.Height = Base_Line * ReCnt_Cacu;
            tT_f2.Width = pageW - (t_f.X * 2);
            e.Graphics.DrawRectangle(T_p, tT_f2);

            int Base_W_T = 153; int Base_Start_W = 140;

            //일련 번호 뒷선
            X1 = 65;
            X2 = X1;
            Y1 = Cacu_Bax;
            Y2 = Cacu_Bax + (Base_Line * ReCnt_Cacu);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //카드명 뒷선
            X1 = t_f.X + Base_Start_W;
            X2 = X1;
            Y1 = Cacu_Bax;
            Y2 = Cacu_Bax + (Base_Line * ReCnt_Cacu);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //카드번호 뒷선
            X1 = t_f.X + Base_Start_W + Base_W_T;
            X2 = X1;
            Y1 = Cacu_Bax;
            Y2 = Cacu_Bax + (Base_Line * ReCnt_Cacu);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //카드번호 뒷선
            X1 = t_f.X + Base_Start_W + Base_W_T + Base_W_T;
            X2 = X1;
            Y1 = Cacu_Bax;
            Y2 = Cacu_Bax + (Base_Line * ReCnt_Cacu);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);




            //카드번호 뒷선
            X1 = t_f.X + Base_Start_W + Base_W_T + Base_W_T + Base_W_T + 20;
            X2 = X1;
            Y1 = Cacu_Bax;
            Y2 = Cacu_Bax + (Base_Line * ReCnt_Cacu);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);





            FontStyle fs = FontStyle.Bold;
            tt.X = 25;
            tt.Y = Cacu_Bax + Base_Font_H;
            msg = "카드";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = 70;
            tt.Y = Cacu_Bax + Base_Font_H;
            msg = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables["t_table"].Rows[0]["InputCard"]);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);



            //현금 뒷선
            X1 = t_f.X + Base_Start_W + 50;
            X2 = X1;
            Y1 = Cacu_Bax;
            Y2 = Cacu_Bax + (Base_Line);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);



            tt.X = t_f.X + 155;
            tt.Y = Cacu_Bax + Base_Font_H;
            msg = "현금";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = t_f.X + Base_Start_W + 55;
            tt.Y = Cacu_Bax + Base_Font_H;
            msg = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables["t_table"].Rows[0]["InputCash"]);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            //현금 뒷선
            X1 = t_f.X + Base_Start_W + Base_W_T + 60;
            X2 = X1;
            Y1 = Cacu_Bax;
            Y2 = Cacu_Bax + (Base_Line);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            //Strsql = Strsql + ", InputCard ,InputPassbook,InputPassbook2, InputCash , TotalInputPrice ";

            tt.X = t_f.X + Base_Start_W + Base_W_T + 65;
            tt.Y = Cacu_Bax + Base_Font_H;
            msg = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables["t_table"].Rows[0]["InputPassbook"]);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = t_f.X + Base_Start_W + Base_W_T + 5;
            tt.Y = Cacu_Bax + Base_Font_H;
            msg = "무통장";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);





            //현금 뒷선
            X1 = t_f.X + Base_Start_W + Base_W_T + Base_W_T + 70;
            X2 = X1;
            Y1 = Cacu_Bax;
            Y2 = Cacu_Bax + (Base_Line);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = t_f.X + Base_Start_W + Base_W_T + Base_W_T + 75;
            tt.Y = Cacu_Bax + Base_Font_H;
            msg = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables["t_table"].Rows[0]["InputPassbook2"]);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = t_f.X + Base_Start_W + Base_W_T + Base_W_T + 5;
            tt.Y = Cacu_Bax + Base_Font_H;
            msg = "가상계좌";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);



            //현금 뒷선
            X1 = t_f.X + Base_Start_W + Base_W_T + Base_W_T + Base_W_T + 90;
            X2 = X1;
            Y1 = Cacu_Bax;
            Y2 = Cacu_Bax + (Base_Line);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            tt.X = t_f.X + Base_Start_W + Base_W_T + Base_W_T + Base_W_T + 95;
            tt.Y = Cacu_Bax + Base_Font_H;
            msg = string.Format(cls_app_static_var.str_Currency_Type, ds.Tables["t_table"].Rows[0]["TotalInputPrice"]);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = t_f.X + Base_Start_W + Base_W_T + Base_W_T + Base_W_T + 20 + 5;
            tt.Y = Cacu_Bax + Base_Font_H;
            msg = "합계금액";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);





            tt.X = 25;
            tt.Y = Cacu_Bax + (Base_Line) + Base_Font_H;
            msg = "일련";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


            tt.X = 70;
            tt.Y = Cacu_Bax + (Base_Line) + Base_Font_H;
            msg = "카드명(은행)";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);

            tt.X = t_f.X + Base_Start_W + 5;
            tt.Y = Cacu_Bax + (Base_Line) + Base_Font_H;
            msg = "카드번호(계좌)";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


            tt.X = t_f.X + Base_Start_W + Base_W_T ;
            tt.Y = Cacu_Bax + (Base_Line) + Base_Font_H;
            msg = "할부";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);

            //현금 뒷선
            X1 = t_f.X + Base_Start_W + Base_W_T + 30;
            X2 = X1;
            Y1 = Cacu_Bax + Base_Line;
            Y2 = Cacu_Bax + (Base_Line * ReCnt_Cacu);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);



            tt.X = t_f.X + Base_Start_W + Base_W_T + 30 + 5;
            tt.Y = Cacu_Bax + (Base_Line) + Base_Font_H;
            msg = "소유자";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


            //현금 뒷선
            X1 = t_f.X + Base_Start_W + Base_W_T + 80;
            X2 = X1;
            Y1 = Cacu_Bax + Base_Line;
            Y2 = Cacu_Bax + (Base_Line * ReCnt_Cacu);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = t_f.X + Base_Start_W + Base_W_T + 80 + 5;
            tt.Y = Cacu_Bax + (Base_Line) + Base_Font_H;
            msg = "승인번호";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


            tt.X = t_f.X + Base_Start_W + Base_W_T + Base_W_T + 5;
            tt.Y = Cacu_Bax + (Base_Line) + Base_Font_H;
            msg = "승인(입금)금액";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


            //현금 뒷선
            X1 = t_f.X + Base_Start_W + Base_W_T + Base_W_T + 88;
            X2 = X1;
            Y1 = Cacu_Bax + Base_Line;
            Y2 = Cacu_Bax + (Base_Line * ReCnt_Cacu);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = t_f.X + Base_Start_W + Base_W_T + Base_W_T + 85 + 5;
            tt.Y = Cacu_Bax + (Base_Line) + Base_Font_H;
            msg = "승인(입금)일자";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);



            tt.X = t_f.X + Base_Start_W + Base_W_T + Base_W_T + Base_W_T + 50 + 5;
            tt.Y = Cacu_Bax + (Base_Line) + Base_Font_H;
            msg = "비고";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);

            int fi_cnt = 2;


            for (int fi_cnt22 = 0; fi_cnt22 <= ReCnt_C - 1; fi_cnt22++)
            {
                tt.X = 25;
                tt.Y = Cacu_Bax + (Base_Line * fi_cnt) + Base_Font_H;
                msg = (fi_cnt22 + 1).ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);



                tt.X = 65;
                tt.Y = Cacu_Bax + (Base_Line * fi_cnt) + Base_Font_H;
                //msg = ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["C_CodeName"].ToString();

                if (ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["C_TF"].ToString() == "5" && ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["BB2_Name"] == null)
                {
                    msg = ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["BB_Name"].ToString();

                    if (msg == "")
                        msg = ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["BB2_Name"].ToString();
                }
                else
                {
                    if (ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["Ca_Name"].ToString() != "")
                        msg = ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["Ca_Name"].ToString();
                    else
                        msg = ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["C_CodeName"].ToString();
                }
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);

                tt.X = t_f.X + Base_Start_W + 5;
                tt.Y = Cacu_Bax + (Base_Line * fi_cnt) + Base_Font_H;
                msg = encrypter.Decrypt (ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["C_Number1"].ToString());
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


                tt.X = t_f.X + Base_Start_W + Base_W_T + 5;
                tt.Y = Cacu_Bax + (Base_Line * fi_cnt) + Base_Font_H;
                msg = ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["C_Installment_Period"].ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


                tt.X = t_f.X + Base_Start_W + Base_W_T + 30 + 5;
                tt.Y = Cacu_Bax + (Base_Line * fi_cnt) + Base_Font_H;
                msg =  ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["C_Name1"].ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


                tt.X = t_f.X + Base_Start_W + Base_W_T + 80 + 5;
                tt.Y = Cacu_Bax + (Base_Line * fi_cnt) + Base_Font_H;
                msg = ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["C_Number2"].ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


                tt.X = t_f.X + Base_Start_W + Base_W_T + Base_W_T + 5;
                tt.Y = Cacu_Bax + (Base_Line * fi_cnt) + Base_Font_H;
                msg = string.Format(cls_app_static_var.str_Currency_Type, ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["C_Price1"]);
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


                tt.X = t_f.X + Base_Start_W + Base_W_T + Base_W_T + 85 + 5;
                tt.Y = Cacu_Bax + (Base_Line * fi_cnt) + Base_Font_H;
                msg = ds2.Tables["t_Cacu_table"].Rows[fi_cnt22]["C_AppDate1"].ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);

                fi_cnt++;
            }


            //===========================================================d여기서 부터 배송 관련 =========================================================================//
            Last_Line = Last_Line + (Base_Line * Cacu_Cnt);
            int Rec_Bax = Last_Line + Base_Line;
            int Cnt = 0;

            while (Cnt <= 5)
            {
                X1 = t_f.X + 120; X2 = pageW - t_f.X;
                Y1 = Rec_Bax + (Base_Line * Cnt);
                Y2 = Rec_Bax + (Base_Line * Cnt);
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

                Cnt++;
            }


            Rectangle tT_f = new Rectangle();
            tT_f.X = 20;
            tT_f.Y = Last_Line + (Base_Line * 2);
            tT_f.Height = Base_Line * 5;
            tT_f.Width = pageW - (t_f.X * 2);
            e.Graphics.DrawRectangle(T_p, tT_f);


            //배송 뒷선
            X1 = t_f.X + 120;
            X2 = X1;
            Y1 = Rec_Bax + Base_Line;
            Y2 = Rec_Bax + (Base_Line * 6);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            X1 = t_f.X + 200;
            X2 = X1;
            Y1 = Rec_Bax + Base_Line;
            Y2 = Rec_Bax + (Base_Line * 6);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);



            X1 = t_f.X + 450;
            X2 = X1;
            Y1 = Rec_Bax + Base_Line;
            Y2 = Rec_Bax + (Base_Line * 3);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            X1 = t_f.X + 530;
            X2 = X1;
            Y1 = Rec_Bax + Base_Line;
            Y2 = Rec_Bax + (Base_Line * 3);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);



            Strsql = "Select * ";
            Strsql = Strsql + " From tbl_Sales_Rece (nolock) ";
            Strsql = Strsql + " Where OrderNumber ='" + P_Ordernumber + "'";

            DataSet ds3 = new DataSet();
            Temp_Connect.Open_Data_Set(Strsql, "t_Rece_table", ds3);

            int ReCnt = Temp_Connect.DataSet_ReCount;

            tt.X = 150;
            tt.Y = Rec_Bax + (Base_Line * 1) + Base_Font_H;
            msg = "배송일";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            if (ReCnt > 0)
            {
                tt.X = 230;
                tt.Y = Rec_Bax + (Base_Line * 1) + Base_Font_H;
                msg = ds3.Tables["t_Rece_table"].Rows[0]["Get_Date1"].ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);
            }


            tt.X = 150;
            tt.Y = Rec_Bax + (Base_Line * 2) + Base_Font_H;
            msg = "수령자";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            if (ReCnt > 0)
            {
                tt.X = 230;
                tt.Y = Rec_Bax + (Base_Line * 2) + Base_Font_H;
                msg = ds3.Tables["t_Rece_table"].Rows[0]["Get_Name1"].ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);
            }


            tt.X = 150;
            tt.Y = Rec_Bax + (Base_Line * 3) + Base_Font_H;
            msg = "주소";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            if (ReCnt > 0)
            {
                tt.X = 230;
                tt.Y = Rec_Bax + (Base_Line * 3) + Base_Font_H;
                msg = ds3.Tables["t_Rece_table"].Rows[0]["Get_Address1"].ToString() + " " + ds3.Tables["t_Rece_table"].Rows[0]["Get_Address2"].ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);
            }



            tt.X = 140;
            tt.Y = Rec_Bax + (Base_Line * 4) + Base_Font_H;
            msg = "배송시 유의사항";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);

            if (ReCnt > 0)
            {
                tt.X = 230;
                tt.Y = Rec_Bax + (Base_Line * 4) + Base_Font_H;
                msg = ds3.Tables["t_Rece_table"].Rows[0]["Get_Etc1"].ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);
            }


            tt.X = 150;
            tt.Y = Rec_Bax + (Base_Line * 5) + Base_Font_H;
            msg = "고객메모";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            if (ReCnt > 0)
            {
                tt.X = 230;
                tt.Y = Rec_Bax + (Base_Line * 5) + Base_Font_H;
                msg = ds3.Tables["t_Rece_table"].Rows[0]["Get_Etc2"].ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);
            }


            tt.X = 470;
            tt.Y = Rec_Bax + (Base_Line * 1) + Base_Font_H;
            msg = "운송장번호";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            if (ReCnt > 0)
            {
                tt.X = 560;
                tt.Y = Rec_Bax + (Base_Line * 1) + Base_Font_H;
                msg = ds3.Tables["t_Rece_table"].Rows[0]["Pass_Number"].ToString();
                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);
            }


            tt.X = 470;
            tt.Y = Rec_Bax + (Base_Line * 2) + Base_Font_H;
            msg = "연락처";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            if (ReCnt > 0)
            {
                tt.X = 560;
                tt.Y = Rec_Bax + (Base_Line * 2) + Base_Font_H;
                msg = ds3.Tables["t_Rece_table"].Rows[0]["Get_Tel1"].ToString();

                if (ds3.Tables["t_Rece_table"].Rows[0]["Get_Tel2"].ToString() != "")
                    msg = msg + " / " + ds3.Tables["t_Rece_table"].Rows[0]["Get_Tel2"].ToString();

                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);
            }


            if (ReCnt > 0)
            {
                if (ds3.Tables["t_Rece_table"].Rows[0]["Receive_Method"].ToString() == "1")
                {
                    tt.X = (pageW / 2) + 181;
                    tt.Y = 55 + Y_tGap;
                    msg = "V";
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 9), Brushes.Black, tt);
                }
                if (ds3.Tables["t_Rece_table"].Rows[0]["Receive_Method"].ToString() == "2")
                {
                    tt.X = (pageW / 2) + 281;
                    tt.Y = 55 + Y_tGap;
                    msg = "V";
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 9), Brushes.Black, tt);
                }
            }




            tt.X = 20;
            tt.Y = Rec_Bax + (Base_Line * 6) + Base_Font_H;
            msg = "● 받으시는 제품 중 제품 하단 바닥면에 빨간색 원형 시트지가 부착되어 있는 제품은 프로모션으로 출고된 제품으로";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = 20;
            tt.Y = Rec_Bax + (Base_Line * 7) + Base_Font_H;
            msg = "교환이 이루어지지 않습니다. 참고하시어 교환에 불편함이 없으시길 바랍니다.";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = (pageW / 2) - 10;
            tt.Y = Rec_Bax + (Base_Line * 8) + Base_Font_H;
            if (ds.Tables["t_table"].Rows[0]["Last_Con_ID"].ToString() != "")
                msg = "처리자 : "+ ds.Tables["t_table"].Rows[0]["Last_Con_ID"].ToString()    + "       결제확인  성명:                      (인)";
            else
            {
                msg = "처리자 :                  결제확인  성명:                      (인)";
            }
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            ////tt.X = (pageW / 2) - 70;
            ////tt.Y = Rec_Bax + (Base_Line * 9) + Base_Font_H;
            ////msg = "처리자 : " + ds.Tables["t_table"].Rows[0]["Last_Con_ID"].ToString(); 
            ////e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);



            tt.X = 160;//- 100;
            tt.Y = Rec_Bax + (Base_Line * 9);//+ Base_Font_H;
            tt.Width = pageW - 340;
            tt.Height = Base_Line * 3;
            e.Graphics.DrawImage(pictureBox1.Image, tt);



            tt.X = 50;
            tt.Y = Rec_Bax + (Base_Line * 3) + 15;
            msg = "배송";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

        }




        private void BaseDoc_PrintPage____003(System.Drawing.Printing.PrintPageEventArgs e, Rectangle t_f, int BaseitemH2, int BaseitemH3, int Y_tGap
                                            , DataSet ds)
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



        private void BaseDoc_PrintPage____004(System.Drawing.Printing.PrintPageEventArgs e, Rectangle t_f, int BaseitemH2, int BaseitemH3, int Y_tGap
                                            , DataSet ds)
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
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 7), Brushes.Black, tt);



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



        private void BaseDoc_PrintPage____005(System.Drawing.Printing.PrintPageEventArgs e, Rectangle t_f, int BaseitemH2, int BaseitemH3, int Y_tGap
                                            , DataSet ds)
        {
            RectangleF tt = new RectangleF();

            string msg = "";
            Pen T_p = new Pen(Color.Black);
            int pageW = e.PageBounds.Width - 20, pageH = e.PageBounds.Height;
            int X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;


            int Base_Line = 20, Base_Font_H = 10; //,  BaseitemH = t_f.Y + 75;;

            string T_El_Rec = "", BeT_Add = "", T_Add = "";

            int fi_cnt = 0;
            //foreach (int t_key in Sales_Rece.Keys)
            //{
            //    if (Sales_Rece[t_key].Del_TF != "D")
            //    {
            //        if (BeT_Add == "")
            //        {
            //            BeT_Add = Sales_Rece[t_key].Receive_Method_Name;
            //            if (Sales_Rece[t_key].Receive_Method == 2)
            //            {
            //                BeT_Add = BeT_Add + "  " + Sales_Rece[t_key].Get_ZipCode;
            //                BeT_Add = BeT_Add + "  " + Sales_Rece[t_key].Get_Address1;
            //                BeT_Add = BeT_Add + "  " + Sales_Rece[t_key].Get_Address2;
            //            }
            //        }
            //        else
            //        {
            //            T_Add = Sales_Rece[t_key].Receive_Method_Name;
            //            if (Sales_Rece[t_key].Receive_Method == 2)
            //            {
            //                T_Add = T_Add + "  " + Sales_Rece[t_key].Get_ZipCode;
            //                T_Add = T_Add + "  " + Sales_Rece[t_key].Get_Address1;
            //                T_Add = T_Add + "  " + Sales_Rece[t_key].Get_Address2;
            //            }
            //        }

            //        if ((BeT_Add != T_Add) && (T_Add != "") && (BeT_Add != ""))
            //            BeT_Add = "다중 배송";

            //        if (Sales_Rece[t_key].Receive_Method == 2)
            //        {

            //            if (Sales_Rece[t_key].Get_Tel1 != "")
            //                T_El_Rec = Sales_Rece[t_key].Get_Tel1;
            //        }
            //    }
            //    fi_cnt++;
            //}

            string T_El = "";

            //string StrSql = "Select hptel,hometel,Address1,Address2 From tbl_Memberinfo  (nolock) ";
            //StrSql = StrSql + " Where Mbid  ='" + SalesDetail[txt_OrderNumber.Text.Trim()].Mbid + "'";
            //StrSql = StrSql + " And   Mbid2 =" + SalesDetail[txt_OrderNumber.Text.Trim()].Mbid2;

            ////++++++++++++++++++++++++++++++++
            //cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            //DataSet ds = new DataSet();
            ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
            //int ReCnt = Temp_Connect.DataSet_ReCount;

            //T_El = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Hometel"].ToString());

            //if (encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hptel"].ToString()) != "")
            //    T_El = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["hptel"].ToString());

            //if (BeT_Add == "")
            //    BeT_Add = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["address1"].ToString()) + " " + encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["address2"].ToString());



            int Base_W = (pageW / 2) + 10;
            int BaseitemH = t_f.Y + 75;
            //등록번호 뒷선
            X1 = Base_W + 47;
            X2 = X1;
            Y1 = BaseitemH;
            Y2 = BaseitemH2;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = Base_W + 2;
            tt.Y = BaseitemH + 2;
            msg = "사업자";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = Base_W + 2;
            tt.Y = BaseitemH + 7 + Base_Font_H;
            msg = "번호";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 48;
            tt.Y = BaseitemH + Base_Font_H;
            msg = ds.Tables["t_table"].Rows[0]["number"].ToString();
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);





            tt.X = Base_W + 2;
            tt.Y = BaseitemH + (30 * 1) + Base_Font_H;
            msg = "상호";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 45;
            tt.Y = Y1 = BaseitemH + (30 * 1) + Base_Font_H;
            msg = ds.Tables["t_table"].Rows[0]["sangho"].ToString();
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);




            tt.X = Base_W + 2;
            tt.Y = BaseitemH + (30 * 2) + Base_Font_H;
            msg = "회원ID";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = Base_W + 48;
            tt.Y = BaseitemH + (30 * 2) + Base_Font_H;
            msg = ds.Tables["t_table"].Rows[0]["Mbid"].ToString();
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);







            X1 = Base_W + 115;
            X2 = X1;
            Y1 = BaseitemH + (30 * 2);
            Y2 = BaseitemH + (30 * 3);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            X1 = Base_W + 146;
            X2 = X1;
            Y1 = BaseitemH + (30 * 2);
            Y2 = BaseitemH + (30 * 3);
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);



            tt.X = Base_W + 115;
            tt.Y = BaseitemH + (30 * 2) + Base_Font_H;
            msg = "직급";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = Base_W + 147;
            tt.Y = BaseitemH + (30 * 2) + Base_Font_H;
            msg = ds.Tables["t_table"].Rows[0]["CC_Grade"].ToString();


            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);



            Base_W = Base_W + 200;

            //연락처 성명 관련 라인
            X1 = Base_W + 45;
            X2 = X1;
            Y1 = BaseitemH;
            Y2 = BaseitemH2;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            X1 = Base_W + 3;
            X2 = X1;
            Y1 = BaseitemH;
            Y2 = BaseitemH2;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = Base_W + 2;
            tt.Y = Y1 = BaseitemH + Base_Font_H;
            msg = "연락처";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);



            tt.X = Base_W + 45;
            tt.Y = Y1 = BaseitemH + Base_Font_H;
            msg = ds.Tables["t_table"].Rows[0]["phone"].ToString();
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);






            tt.X = Base_W + 2;
            tt.Y = BaseitemH + (30 * 1) + Base_Font_H;
            msg = "대표자";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 45;
            tt.Y = BaseitemH + (30 * 1) + Base_Font_H;
            msg = ds.Tables["t_table"].Rows[0]["S_name"].ToString();
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);





            tt.X = Base_W + 2;
            tt.Y = BaseitemH + (30 * 2) + 2;
            msg = "회원";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = Base_W + 2;
            tt.Y = BaseitemH + (30 * 2) + 7 + Base_Font_H;
            msg = "성명";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 45;
            tt.Y = BaseitemH + (30 * 2) + Base_Font_H;
            msg = ds.Tables["t_table"].Rows[0]["M_Name"].ToString();
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);




        }









    }
}
