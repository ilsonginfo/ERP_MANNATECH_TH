﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using DXVGrid = DevExpress.XtraGrid.Views.Grid;
using DViewInfo = DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DXEditor = DevExpress.XtraEditors;
using DXGrid = DevExpress.XtraGrid;

namespace MLM_Program
{
    public partial class frmSell_Select : clsForm_Extends
    {

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
        private string T_Search_Nubmer = "";


        Class.DevGridControlService cgb = new Class.DevGridControlService();




        private const string base_db_name = "tbl_SalesDetail";
        private int Data_Set_Form_TF;
        private int Form_Load_TF = 0;

        public delegate void SendNumberDele(string Send_Number, string Send_Name, string Send_OrderNumber);
        public event SendNumberDele Send_Sell_Number;

        public delegate void Send_Mem_NumberDele(string Send_Number, string Send_Name);
        public event Send_Mem_NumberDele Send_Mem_Number;

        private Series series_Item = new Series();


        public frmSell_Select()
        {
            InitializeComponent();
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

            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            mtxtSellDate1.Text = DateTime.Now.ToString("yyyy-MM-dd");


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


            if (cls_app_static_var.Using_Mileage_TF == 0)
                tableLayoutPanel17.Visible = false;
            else
                tableLayoutPanel17.Visible = true;

            mtxtSellDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate21.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate22.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMakDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMakDate2.Mask = cls_app_static_var.Date_Number_Fromat;


            txt_P_1.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_2_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_3.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_4.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_5.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_6.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_7.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_8.BackColor = cls_app_static_var.txt_Enable_Color;

            txt_SumCnt.BackColor = cls_app_static_var.txt_Enable_Color;

            radioB_REC_0.Checked = true; 

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
        }


        private void frm_Base_Activated(object sender, EventArgs e)
        {
            this.Refresh();

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
            ////메뉴 상에서 들어가는 텍스트들을 알맞게변경을 한다. 외국어 버전을 감안해서 작업한거임.
            cls_form_Meth cm = new cls_form_Meth();            
            string m_text = "";

            for (int Cnt = 0; Cnt <= contextM.Items.Count - 1; Cnt ++)
            {
                m_text = contextM.Items[Cnt].Text.ToString();

                if (m_text != "")
                    contextM.Items[Cnt].Text =  cm._chang_base_caption_search(m_text);
            }             
            ////메뉴 상에서 들어가는 텍스트들을 알맞게변경을 한다. 외국어 버전을 감안해서 작업한거임.
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

            if (mtxtSellDate21.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate21.Text, mtxtSellDate21, "Date") == false)
                {
                    mtxtSellDate21.Focus();
                    return false;
                }
            }

            if (mtxtSellDate22.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate22.Text, mtxtSellDate22, "Date") == false)
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

            //string[] g_HeaderText = {"주문번호"  , "구매_일자"   , "반품_교환_일자"  , "회원_번호"   , "성명"        
            //                    , "주민번호"   , "등록_센타명"    , "구매_센타명"   , "구매_종류"    , "총구매액"
            //                    , "총PV"   , "총결제액"  , "현금"   , "카드"   ,"무통장"
            //                    , "미수금"     , "구분"    , "비고1" , "비고2"     , "기록자"
            //                    , "기록일", ""  , ""  , ""  ,""
            //                    , ""
            //                        };
            cls_form_Meth cm =new cls_form_Meth ();
            
            Tsql = "Select  "+ Environment.NewLine;

            Tsql = Tsql + " Case When Ga_Order >= 1 Then '" + cm._chang_base_caption_search("미승인") + "'"+ Environment.NewLine;
            Tsql = Tsql + "  When Ga_Order = 0 Then '" + cm._chang_base_caption_search("승인") + "'"+ Environment.NewLine;
            Tsql = Tsql + " ELSE '' "+ Environment.NewLine;
            Tsql = Tsql + " END SellTFName "+ Environment.NewLine;

            Tsql = Tsql + " , tbl_SalesDetail.OrderNumber  "+ Environment.NewLine;
            //Tsql = Tsql + " ,isnull( rece.PASS_NUMBER,'') " + Environment.NewLine;
            //Tsql = Tsql + "  , CASE WHEN StockOut.OrderNumber IS NULL THEN '' ELSE 'O' END AS '출고여부'" + Environment.NewLine;
         

            ////Tsql = Tsql + " , Case When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE = '0000' Then ISNULL(T_REALMLM.GUARANTE_NUM,'') "+ Environment.NewLine;
            ////Tsql = Tsql + "        When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE <> '0000' Then  ISNULL(T_REALMLM_ErrCode.Er_Msg ,'' ) "+ Environment.NewLine;
            ////Tsql = Tsql + "        When  tbl_SalesDetail.union_Seq = 0 Then '미신고'  "+ Environment.NewLine;
            ////Tsql = Tsql + "   End  "+ Environment.NewLine;


            if (cls_app_static_var.Sell_Union_Flag == "U")  //특판
            {
                Tsql = Tsql + " , Case When  tbl_SalesDetail.union_Seq > 0 And InsuranceNumber <> '' Then InsuranceNumber "+ Environment.NewLine;
                Tsql = Tsql + "        When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE <> '0000' And InsuranceNumber = '' Then  ISNULL(T_REALMLM_ErrCode.Er_Msg ,'' ) "+ Environment.NewLine;
                Tsql = Tsql + "        When  tbl_SalesDetail.union_Seq = 0 Then '미신고'  "+ Environment.NewLine;
                Tsql = Tsql + "   End  "+ Environment.NewLine;
            }
            else if (cls_app_static_var.Sell_Union_Flag == "D")  //직판
            {
                //Tsql = Tsql + ", Case When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail(nolock) AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NULL And tbl_SalesDetail.InsuranceNumber <> '' Then tbl_SalesDetail.InsuranceNumber " + Environment.NewLine;
                //Tsql = Tsql + " When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail(nolock) AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' " + Environment.NewLine;
                //Tsql = Tsql + " When  ReturnTF = 5 And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' " + Environment.NewLine;
                //Tsql = Tsql + " When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail(nolock) AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel ='' Then tbl_SalesDetail.InsuranceNumber + '(취소요청중)' " + Environment.NewLine;
                //Tsql = Tsql + " When  ReturnTF = 1 And tbl_SalesDetail.InsuranceNumber = '' Then '미승인요청' " + Environment.NewLine;
                //Tsql = Tsql + " ELSE tbl_SalesDetail.InsuranceNumber END  " + Environment.NewLine;


                Tsql = Tsql + ", Case When  ReturnTF = 1 And(Select A1.Re_BaseOrderNumber From tbl_SalesDetail(nolock) AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber group by A1.Re_BaseOrderNumber) IS NULL And InsuranceNumber <> '' Then tbl_SalesDetail.InsuranceNumber" + Environment.NewLine;
                Tsql = Tsql + " When  ReturnTF = 1 And(Select A1.Re_BaseOrderNumber From tbl_SalesDetail(nolock) AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber group by A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel = 'Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' " + Environment.NewLine;
                Tsql = Tsql + " When ReturnTF = 5 And InsuranceNumber_Cancel = 'Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' " + Environment.NewLine;
                Tsql = Tsql + " When ReturnTF = 1 And(Select A1.Re_BaseOrderNumber From tbl_SalesDetail(nolock) AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber group by A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel = '' Then tbl_SalesDetail.InsuranceNumber + '(취소요청중)' " + Environment.NewLine;
                Tsql = Tsql + " When ReturnTF = 1 And tbl_SalesDetail.InsuranceNumber = '' Then '미승인요청' " + Environment.NewLine;
                Tsql = Tsql + " ELSE tbl_SalesDetail.InsuranceNumber END  " + Environment.NewLine;

                //Tsql = Tsql + ", Case When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail(nolock) AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NULL And tbl_SalesDetail.InsuranceNumber <> '' Then tbl_SalesDetail.InsuranceNumber " + Environment.NewLine;
                //Tsql = Tsql + " When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail(nolock) AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' " + Environment.NewLine;
                //Tsql = Tsql + " When  ReturnTF = 5 And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' "+ Environment.NewLine;
                //Tsql = Tsql + " When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail(nolock) AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel ='' Then tbl_SalesDetail.InsuranceNumber + '(취소요청중)' " + Environment.NewLine;
                //Tsql = Tsql + " When  ReturnTF = 1 And tbl_SalesDetail.InsuranceNumber = '' Then '미승인요청' "+ Environment.NewLine;
                //Tsql = Tsql + " ELSE tbl_SalesDetail.InsuranceNumber END  "+ Environment.NewLine;
            }
            else
            {
                Tsql = Tsql + " , '' "+ Environment.NewLine;
            }


            Tsql = Tsql + " , Case ReturnTF When 1 then isnull(LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2)   ,'')  " + Environment.NewLine;
            Tsql = Tsql + "   ELSE (Select  isnull(LEFT(A1.SellDate,4) +'-' + LEFT(RIGHT(A1.SellDate,4),2) + '-' + RIGHT(A1.SellDate,2),'') From tbl_SalesDetail (NOLOCK)  AS A1 Where A1.OrderNumber = tbl_SalesDetail.Re_BaseOrderNumber)  END " + Environment.NewLine;


            Tsql = Tsql + "  , Case ReturnTF When 1 then '' ELSE  isnull(LEFT(SellDate,4) +'-' + LEFT(RIGHT(SellDate,4),2) + '-' + RIGHT(SellDate,2),'')  END as selldate " + Environment.NewLine;
            Tsql = Tsql + "  , isnull(LEFT(SellDate_2,4) +'-' + LEFT(RIGHT(SellDate_2,4),2) + '-' + RIGHT(SellDate_2,2),'')  as selldate1" + Environment.NewLine;
            //Tsql = Tsql + ", isnull(LEFT(StockOut.Out_Date,4) +'-' + LEFT(RIGHT(StockOut.Out_Date,4),2) + '-' + RIGHT(StockOut.Out_Date,2),'')  as selldate2" + Environment.NewLine; ;
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", tbl_SalesDetail.mbid + '-' + Convert(Varchar,tbl_SalesDetail.mbid2) "+ Environment.NewLine;
            else
                Tsql = Tsql + ", tbl_SalesDetail.mbid2 "+ Environment.NewLine;

            

            Tsql = Tsql + " ,tbl_SalesDetail.M_Name "+ Environment.NewLine;
            Tsql = Tsql + " ,C1.Grade_Name " + Environment.NewLine;

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", N_Mem.mbid + '-' + Convert(Varchar,N_Mem.mbid2) "+ Environment.NewLine;
            else
                Tsql = Tsql + ", N_Mem.mbid2 "+ Environment.NewLine;
            
            Tsql = Tsql + " ,N_Mem.M_Name "+ Environment.NewLine;
            if(chkShowSaveDefault.Checked)
                Tsql = Tsql + ", (SELECT TOP 1 ISNULL(CAST(MBID2 AS VARCHAR), '') + '_' + ISNULL(M_NAME, '') AS SaveDefault  FROM [ufn_Up_Search_Save_Gold] ('', tbl_SalesDetail.mbid2) as SaveDefault )" + Environment.NewLine;
            else
                Tsql = Tsql + ", '' as SaveDefault "+ Environment.NewLine;
            Tsql = Tsql + ", tbl_Memberinfo.Cpno " + Environment.NewLine;

            Tsql = Tsql + " ,Case When Receive_Method = 1 Then '" + cm._chang_base_caption_search("직접수령") + "'"+ Environment.NewLine;
            Tsql = Tsql + "  When Receive_Method = 2 Then '" + cm._chang_base_caption_search("배송") + "'"+ Environment.NewLine;
            Tsql = Tsql + "  When Receive_Method = 3 Then '" + cm._chang_base_caption_search("센타수령") + "'"+ Environment.NewLine;
            Tsql = Tsql + "  When Receive_Method = 4 Then '" + cm._chang_base_caption_search("본사직접수령") + "'"+ Environment.NewLine;
            Tsql = Tsql + "  ELSE   '" + cm._chang_base_caption_search("미입력") + "'"+ Environment.NewLine;
            Tsql = Tsql + " END Receive_Method_Name "+ Environment.NewLine;         

            //Tsql = Tsql + ", LEFT(SellDate_2,4) +'-' + LEFT(RIGHT(SellDate_2,4),2) + '-' + RIGHT(SellDate_2,2) "+ Environment.NewLine;

            Tsql = Tsql + " ,Isnull(S_Bus.Name,'') as S_B_Name"+ Environment.NewLine;
            Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name"+ Environment.NewLine;

            Tsql = Tsql + " , tbl_SellType.SellTypeName SellCodeName  "+ Environment.NewLine;
            Tsql = Tsql + " , Etc1  " + Environment.NewLine;
            Tsql = Tsql + " ,TotalPrice " + Environment.NewLine;
            Tsql = Tsql + ", Totalpv " + Environment.NewLine;
            Tsql = Tsql + ", TotalCv " + Environment.NewLine;
            Tsql += " ,TotalInputPrice  " + Environment.NewLine;
            Tsql += " ,InputCash";
            Tsql += " ,InputCard ";
            //Tsql += " , '' AS 'C_Number1'";
            //Tsql += " , '' AS 'C_Number2'";
            Tsql += " , ISNULL(STUFF(( " + Environment.NewLine;
            Tsql += " SELECT  ' '+ C_Number1  FROM tbl_Sales_Cacu (NOLOCK) Cacu " + Environment.NewLine;
            Tsql += " WHERE Cacu.OrderNumber = tbl_SalesDetail.OrderNumber AND Cacu.C_TF = 3 AND C_Number1 <> '' FOR XML PATH('')), 1, 1, ''), '') AS 'C_Number1' " + Environment.NewLine;

            Tsql += " , ISNULL(STUFF(( " + Environment.NewLine;
            Tsql += " SELECT ',' + CASE WHEN LEN(C_Number2) > 12 THEN DBO.DECRYPT_AES256(C_Number2) ELSE C_Number2 END  FROM tbl_Sales_Cacu(NOLOCK) Cacu  " + Environment.NewLine;
            Tsql += " WHERE Cacu.OrderNumber = tbl_SalesDetail.OrderNumber AND Cacu.C_TF = 3 AND C_Number2 <> '' FOR XML PATH('')), 1, 1, ''), '') AS 'C_Number2' " + Environment.NewLine;
            //Tsql += " , Isnull(View_Sell_C_Number1.C_Number1, '')";
            //Tsql += "  ,Isnull(View_Sell_C_Number1.C_Number2, '')";
            Tsql += " ,InputPassbook_2 ";
            Tsql += " ,InputPassbook ";
            Tsql += " ,InputCoupon ";
            //마일리지관련 함수 Tsql += ", '' "; // CASE WHEN cacu.C_TF = 4 THEN cacu.c_price1 else 0 end as InputMile " + Environment.NewLine;
            Tsql = Tsql + " ,UnaccMoney "+ Environment.NewLine;

            //Tsql = Tsql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " ReturnTFName "+ Environment.NewLine;

            Tsql = Tsql + " ,Case When ReturnTF = 1 Then '" + cm._chang_base_caption_search("정상") + "'"+ Environment.NewLine;
            Tsql = Tsql + "  When ReturnTF = 2 Then '" + cm._chang_base_caption_search("반품") + "'"+ Environment.NewLine;
            Tsql = Tsql + "  When ReturnTF = 4 Then '" + cm._chang_base_caption_search("교환") + "'"+ Environment.NewLine;
            Tsql = Tsql + "  When ReturnTF = 3 Then '" + cm._chang_base_caption_search("부분반품") + "'"+ Environment.NewLine;
            Tsql = Tsql + "  When ReturnTF = 5 Then '" + cm._chang_base_caption_search("취소") + "'"+ Environment.NewLine;
            Tsql = Tsql + " END ReturnTFName "+ Environment.NewLine;         

            Tsql = Tsql + " ,tbl_SalesDetail.Etc1 "+ Environment.NewLine;
            Tsql = Tsql + " ,'' cash_num"+ Environment.NewLine;
            //Tsql = Tsql + " ,tbl_SalesDetail.Pass_num "+ Environment.NewLine;

            Tsql = Tsql + " ,tbl_SalesDetail.Recordid "+ Environment.NewLine;
            Tsql = Tsql + " ,tbl_SalesDetail.recordtime "+ Environment.NewLine;

            Tsql = Tsql + " ,tbl_SalesDetail.Exi_TF "+ Environment.NewLine;
            Tsql = Tsql + " ,tbl_Memberinfo.hptel "+ Environment.NewLine;


            Tsql = Tsql + " From tbl_SalesDetail (nolock) "+ Environment.NewLine;
           // Tsql = Tsql + " LEFT JOIN tbl_SalesDetail_TF (nolock) ON tbl_SalesDetail.OrderNumber = tbl_SalesDetail_TF.OrderNumber "+ Environment.NewLine;
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 "+ Environment.NewLine;

            Tsql = Tsql + " LEFT JOIN (Select Mbid,Mbid2,M_Name From tbl_Memberinfo (nolock) ) AS N_Mem ON N_Mem.Mbid = tbl_Memberinfo.Nominid And N_Mem.Mbid2 = tbl_Memberinfo.Nominid2 "+ Environment.NewLine;

            Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code "+ Environment.NewLine;
            Tsql = Tsql + " LEFT JOIN tbl_Business S_Bus (nolock) ON tbl_SalesDetail.BusCode = S_Bus.NCode  And tbl_SalesDetail.Na_code = S_Bus.Na_code "+ Environment.NewLine;            
            Tsql = Tsql + " Left Join tbl_Class_P C1 (nolock) On tbl_Memberinfo.CurPoint = C1.Grade_Code" + Environment.NewLine;            
            Tsql = Tsql + " LEFT Join tbl_SellType (nolock) ON  tbl_SalesDetail.SellCode = tbl_SellType.SellCode " + Environment.NewLine;

            Tsql = Tsql + " LEFT Join View_Sell_Rece_Meth ON View_Sell_Rece_Meth.OrderNumber = tbl_SalesDetail.OrderNumber "+ Environment.NewLine;

            

            //Tsql = Tsql + " LEFT JOIN T_REALMLM (nolock) ON T_REALMLM.SEQ = tbl_SalesDetail.union_Seq "+ Environment.NewLine;
            //Tsql = Tsql + " LEFT JOIN T_REALMLM_ErrCode (nolock) ON T_REALMLM.ERRCODE = T_REALMLM_ErrCode.Er_Code "+ Environment.NewLine;

            //Tsql = Tsql + "LEFT JOIN View_Sell_C_Number1(nolock) ON View_Sell_C_Number1.Ordernumber = tbl_SalesDetail.Ordernumber " + Environment.NewLine;
            ////Tsql = Tsql + " LEFT JOIN tbl_Sales_Cacu cacu ON tbl_SalesDetail.OrderNumber = cacu.OrderNumber and cacu.C_index >  0 " + Environment.NewLine;
            //Tsql = Tsql + " LEFT JOIN(select OrderNumber,MAX(Out_Date) Out_Date  from tbl_StockOutput (nolock) GROUP BY OrderNumber) StockOut on tbl_SalesDetail.OrderNumber = StockOut.OrderNumber" + Environment.NewLine;
            //Tsql = Tsql + " LEFT JOIN(SELECT OrderNumber, max(Pass_Number) Pass_Number  FROM tbl_Sales_Rece (NOLOCK) where isnull(pass_number, '') <> '' group by  OrderNumber  ) rece ON tbl_SalesDetail.OrderNumber = rece.OrderNumber" + Environment.NewLine;
            //Tsql = Tsql + " LEFT JOIN(select OrderNumber, C_Etc  from tbl_Sales_Cacu (nolock)where c_index = '1') AUTOSHIP_RESULT on tbl_SalesDetail.OrderNumber = AUTOSHIP_RESULT.OrderNumber" + Environment.NewLine;


        }



        private void Make_Base_Query_(ref string Tsql)
        {//부분반품이 한주문에 두개이상있는거 걸러야하는데 일단 해당주문만 하드코딩으로 해둠
            string strSql = " Where tbl_SalesDetail.ordernumber not in ('2021031600654') and tbl_SalesDetail.Mbid2 >= 0 And tbl_SalesDetail.SellCode <> '' ";
            
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


            //구매일자 검색 -1
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_SalesDetail.SellDate = '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";

            //구매일자로 검색 -2
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_SalesDetail.SellDate >= '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_SalesDetail.SellDate <= '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";
            }


            //정산일자로 검색 -1
            if ((mtxtSellDate21.Text.Replace("-", "").Trim() != "") && (mtxtSellDate22.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_SalesDetail.SellDate_2 = '" + mtxtSellDate21.Text.Replace("-", "").Trim() + "'";

            //정산일자로 검색 -2
            if ((mtxtSellDate21.Text.Replace("-", "").Trim() != "") && (mtxtSellDate22.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_SalesDetail.SellDate_2 >= '" + mtxtSellDate21.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_SalesDetail.SellDate_2 <= '" + mtxtSellDate22.Text.Replace("-", "").Trim() + "'";
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
            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";

            if (txt_us.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.Us_Ord = '" + txt_us.Text.Trim() + "'";

            if (txt_Us_num.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.Us_Num = '" + txt_Us_num.Text.Trim() + "'";
            

            

            if (txtCenter2_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.BusCode = '" + txtCenter2_Code.Text.Trim() + "'";

            if (txtR_Id_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.recordid = '" + txtR_Id_Code.Text.Trim() + "'";


            if (txtSellCode_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.SellCode = '" + txtSellCode_Code.Text.Trim() + "'";

            if (txtOrderNumber.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.OrderNumber = '" + txtOrderNumber.Text.Trim() + "'";



            if (radioB_REC_1.Checked == true)
                strSql = strSql + " And Receive_Method = 1 ";

            if (radioB_REC_2.Checked == true)
                strSql = strSql + " And Receive_Method = 2 ";

            if (radioB_REC_3.Checked == true)
                strSql = strSql + " And Receive_Method = 3 ";

            if (radioB_REC_100.Checked == true)
                strSql = strSql + " And (Receive_Method = 0  OR Receive_Method IS NULL )   ";



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
                strSql = strSql + " And tbl_SalesDetail.Ga_Order = 0 ";

            if (radioB_SellTF3.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.Ga_Order > 0 ";

            //if (radio_S2.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.Us_Ord >0 ";

            //if (radio_S3.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.Us_Ord = 0 ";

            

            if (opt_Ed_2.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.UnaccMoney = 0 ";

            if (opt_Ed_3.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.UnaccMoney <> 0 ";



            //strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            if(cls_User.gid_CountryCode != "TH")
                strSql = strSql + " And tbl_SalesDetail.BusCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";

            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


            Tsql = Tsql + strSql ;
            Tsql = Tsql + " Order by tbl_SalesDetail.SellDate DESC, tbl_SalesDetail.OrderNumber ";
            Tsql = Tsql + ",tbl_SalesDetail.Mbid, tbl_SalesDetail.Mbid2  ";
        }

        private void Make_Base_Query_Cash(ref string Tsql)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select tbl_Sales_Cacu.OrderNumber,  MAX(C_Cash_Number) C_Cash_Number");
            sb.AppendLine("from tbl_Sales_Cacu(NOLOCK)");
            sb.AppendLine(" JOIN tbl_SalesDetail(nolock) on tbl_Sales_Cacu.OrderNumber = tbl_SalesDetail.OrderNumber");
            sb.AppendLine("   join tbl_Memberinfo (nolock) on tbl_SalesDetail.mbid2 = tbl_Memberinfo.mbid2");

            sb.AppendLine(" LEFT Join View_Sell_Rece_Meth ON View_Sell_Rece_Meth.OrderNumber = tbl_SalesDetail.OrderNumber ");

            sb.AppendLine("where C_TF = 1  AND C_Cash_Send_TF = 1 AND C_Cash_Number<> ''");
            Tsql = sb.ToString();
        }

        private void Make_Base_Query_Cash_(ref string Tsql)
        {
            string strSql = string.Empty;
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


            //구매일자 검색 -1
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_SalesDetail.SellDate = '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";

            //구매일자로 검색 -2
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_SalesDetail.SellDate >= '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_SalesDetail.SellDate <= '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";
            }


            //정산일자로 검색 -1
            if ((mtxtSellDate21.Text.Replace("-", "").Trim() != "") && (mtxtSellDate22.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_SalesDetail.SellDate_2 = '" + mtxtSellDate21.Text.Replace("-", "").Trim() + "'";

            //정산일자로 검색 -2
            if ((mtxtSellDate21.Text.Replace("-", "").Trim() != "") && (mtxtSellDate22.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_SalesDetail.SellDate_2 >= '" + mtxtSellDate21.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_SalesDetail.SellDate_2 <= '" + mtxtSellDate22.Text.Replace("-", "").Trim() + "'";
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




            ////센타코드로으로 검색
            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";

            if (txt_us.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.Us_Ord = '" + txt_us.Text.Trim() + "'";

            if (txt_Us_num.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.Us_Num = '" + txt_Us_num.Text.Trim() + "'";




            if (txtCenter2_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.BusCode = '" + txtCenter2_Code.Text.Trim() + "'";

            if (txtR_Id_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.recordid = '" + txtR_Id_Code.Text.Trim() + "'";


            if (txtSellCode_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.SellCode = '" + txtSellCode_Code.Text.Trim() + "'";

            if (txtOrderNumber.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.OrderNumber = '" + txtOrderNumber.Text.Trim() + "'";



            if (radioB_REC_1.Checked == true)
                strSql = strSql + " And Receive_Method = 1 ";

            if (radioB_REC_2.Checked == true)
                strSql = strSql + " And Receive_Method = 2 ";

            if (radioB_REC_3.Checked == true)
                strSql = strSql + " And Receive_Method = 3 ";

            if (radioB_REC_100.Checked == true)
                strSql = strSql + " And (Receive_Method = 0  OR Receive_Method IS NULL )   ";



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
                strSql = strSql + " And tbl_SalesDetail.Ga_Order = 0 ";

            if (radioB_SellTF3.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.Ga_Order > 0 ";

            //if (radio_S2.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.Us_Ord >0 ";

            //if (radio_S3.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.Us_Ord = 0 ";



            if (opt_Ed_2.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.UnaccMoney = 0 ";

            if (opt_Ed_3.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.UnaccMoney <> 0 ";

            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(strSql);
            sb.AppendLine("GROUP BY tbl_Sales_Cacu.OrderNumber");
            Tsql += sb.ToString();
        }

        private void Base_Grid_Set()
        {   
            string Tsql = "";            
            string Tsql_cash = "";
            Make_Base_Query(ref Tsql);

            Make_Base_Query_(ref Tsql);

            Make_Base_Query_Cash(ref Tsql_cash);

            Make_Base_Query_Cash_(ref Tsql_cash);
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();                                  
            
            DataSet ds = new DataSet();
            DataSet dsCash = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name , this.Text ) == false) return;


            int ReCnt = Temp_Connect.DataSet_ReCount;
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
           if (Temp_Connect.Open_Data_Set(Tsql_cash, base_db_name, dsCash, this.Name, this.Text) == false) return;
            if (ReCnt == 0) return;
            foreach(DataRow row in ds.Tables[base_db_name].Rows)
            {
                string bySearch = string.Format("OrderNumber = '{0}'", row["OrderNumber"].ToString());
                DataRow[] FindRow = dsCash.Tables[base_db_name].Select(bySearch);
                if (FindRow.Length > 0)
                {
                    row["Cash_num"] = FindRow[0]["C_Cash_Number"].ToString();
                 
                }
            }
            //++++++++++++++++++++++++++++++++
            string SumOrderNumber = string.Empty;
            double Sum_10 = 0; double Sum_11 = 0; double Sum_12 = 0, Sum_11_2 = 0 ;
            double Sum_13 = 0; double Sum_14 = 0; double Sum_15 = 0;
            double Sum_16 = 0; //double Sum_17 = 0;
            double Sell_Cnt_1 = 0; double Sell_Cnt_2 = 0;
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            Dictionary<string, double> SelType_1 = new Dictionary<string, double>();
            Dictionary<string, double> Center_Pr = new Dictionary<string, double>();
            String C_Number1 = "";
            String Sub_C_number1 = "";
            String Full_C_number1 = "";
            
            bool CheckComa;
            int Count_CheckComa;
            int C_Number_index = 0;
            int C_Number_index2 = 0;
            int C_Number_index3 = 0;
            List<string> SumOrderNumbers = new List<string>();
            //20201118 구현호
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
           
                ds.Tables[base_db_name].Rows[fi_cnt]["cpno"] = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["cpno"].ToString(), "Cpno");
                C_Number1 = ds.Tables[base_db_name].Rows[fi_cnt]["C_Number1"].ToString();
                 CheckComa = C_Number1.Contains(" ");

                if (CheckComa == true)
                {
                    Count_CheckComa = C_Number1.Count(f => f == ' ');
                    C_Number_index = 0;
                    C_Number_index2 = 0;
                    C_Number_index3 = 0;
                    Full_C_number1 = "";

                    for (int fi_cnt2 = 1; fi_cnt2 <= Count_CheckComa + 1; fi_cnt2++)
                    {
                        if (fi_cnt2 == Count_CheckComa + 1)
                        {
                            C_Number_index = C_Number1.Length - C_Number_index2;
                            Sub_C_number1 = C_Number1.Substring(C_Number_index2, C_Number_index);
                        }
                        else
                        {
                            C_Number_index = C_Number1.IndexOf(' ', C_Number_index2);
                            C_Number_index3 = C_Number_index - C_Number_index2;
                            Sub_C_number1 = C_Number1.Substring(C_Number_index2, C_Number_index3);
                        }

                        Sub_C_number1 = encrypter.Decrypt(Sub_C_number1);
                        Full_C_number1 = Full_C_number1 + "/" + Sub_C_number1;
                        C_Number_index2 = C_Number_index2 + C_Number_index3 + 1;
                        Sub_C_number1 = "";
                    }
                    ds.Tables[base_db_name].Rows[fi_cnt]["C_Number1"] = Full_C_number1;
                }
                else
                {
                    if (ds.Tables[base_db_name].Rows[fi_cnt]["C_Number1"].ToString() != "" )
                        ds.Tables[base_db_name].Rows[fi_cnt]["C_Number1"] = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["C_Number1"].ToString());
                }
                //ds.Tables[base_db_name].Rows[fi_cnt]["C_Number2"] = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["C_Number2"].ToString());
                //row["C_Number1"] = encrypter.Decrypt(FindRow[0]["C_Number1"].ToString());


            //    if (ds.Tables[base_db_name].Rows[fi_cnt]["TotalInputPrice"]  == DBNull.Value) ds.Tables[base_db_name].Rows[fi_cnt]["TotalInputPrice"]  =0;
            //if (ds.Tables[base_db_name].Rows[fi_cnt]["InputCash"]        == DBNull.Value) ds.Tables[base_db_name].Rows[fi_cnt]["InputCash"]       =0;
            //if(ds.Tables[base_db_name].Rows[fi_cnt]["InputCard"]        == DBNull.Value) ds.Tables[base_db_name].Rows[fi_cnt]["InputCard"]        =0;
            //if(ds.Tables[base_db_name].Rows[fi_cnt]["InputPassbook_2"]    == DBNull.Value) ds.Tables[base_db_name].Rows[fi_cnt]["InputPassbook_2"]    =0;
            ////if (ds.Tables[base_db_name].Rows[fi_cnt]["InputMile"] == DBNull.Value) ds.Tables[base_db_name].Rows[fi_cnt]["InputMile"] = 0;




                
                //Sum_17 = Sum_17 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputMile"].ToString());
                //SumOrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                //if (SumOrderNumbers.Contains(SumOrderNumber)) continue;
                //else
                //    SumOrderNumbers.Add(SumOrderNumber);

                //Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                Sum_12 = Sum_12 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalInputPrice"].ToString());
                Sum_10 = Sum_10 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
                Sum_11 = Sum_11 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
                Sum_11_2 = Sum_11_2 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalCV"].ToString());
                Sum_16 = Sum_16 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["UnaccMoney"].ToString());
                if (Sum_16 > 0)
                {
                    Sum_16 = Sum_16;
                }
                Sum_13 = Sum_13 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCash"].ToString());
                Sum_14 = Sum_14 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCard"].ToString());
                Sum_15 = Sum_15 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputPassbook_2"].ToString());

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
            cgb.FillGrid(ds.Tables[0]);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txt_P_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_10);
                txt_P_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_11);
                txt_P_2_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_11_2);
                txt_P_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_12);
                txt_P_4.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_13);
                txt_P_5.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_14);
                txt_P_6.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_15);
                //txt_P_8.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_17);        
                txt_P_7.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_16);
                txt_SumCnt.Text = string.Format(cls_app_static_var.str_Currency_Type, ReCnt);   
            }
            
            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
            

            //foreach(DevExpress.XtraGrid.Columns.GridColumn col in  dGridView_Base.Columns)
            //{
            //    if(new List<string>()
            //    {
            //        "OrderNumber"  ,"InsuranceNumber", "mbid2"   , "mname",  "cpno","nmbid2", "nmname"
            //    }.Contains(col.Name))
            //    {
            //        col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            //    }else
            //        col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;

            //}

            //dGridView_Base.Rows[0].Frozen = true;
            //dGridView_Base.la
        }



        private void dGridView_Base_Header_Reset()
        {
            
            cgb.basegrid = dGridCtrl_Base;            
            cgb.baseview = dGridView_Base;            
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            //cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {

                                   "승인여부"   , "주문번호"        
                                   //, "운송장번호"  
                                   //, "출고여부"  
                                   , "공제번호"  
                                 , "구매_일자"  , "교환_반품_일자"   , "정산_일자"  
                                 //, "출고일자"    
                                 , "회원_번호"  
                                 , "성명"       , "등급"             , "추천인"      , "추천인명"    , "후원인기준"
                                 , "주민번호"   , "배송구분"         , "구매_센타명" , "등록_센타명" , "구매_종류" 
                                 , "에러메세지" , "총구매액"         , "총PV"        , "총CV"        , "총결제액"  
                                 , "현금"       , "카드"             , "카드번호"    , "승인번호"    , "가상계좌"
                                 , "무통장"     , "쿠폰"             , "미수금"      , "구분"        , "비고1"
                                 , "현금영수증번호"
                                 //, "운송장번호"
                                 , "기록자"   , "기록일"      , "_"
                                 , "연락처"
                                    };


            string[] g_Cols = {
                                  "승인여부"   , "OrderNumber"    
                                  //, "OrderNumber"
                                  //, "OrderNumber" 
                                  , "InsuranceNumber"
                                , "selldate"   , "교환_반품_일자"  , "정산_일자"  
                                //, "출고일자"   
                                , "mbid2" 
                                , "mname"      , "grade"           , "nmname"       , "추천인명"     , "SaveDefault"
                                , "cpno"       , "배송구분"        , "구매_센타명"  , "등록_센타명"  , "구매_종류" 
                                , "에러메세지" , "총구매액"        , "총PV"         , "총CV"         , "총결제액" 
                                , "현금"       , "카드"            , "CardNumber1"  , "CardNumber2"  , "가상계좌"
                                , "무통장"     , "쿠폰"            , "미수금"          , "구분"         , "비고1"
                                , "cash_num"  
                                //, "운송장번호"
                                , "기록자"          , "기록일"       , "C1"
                                , "연락처"
                                    };

            cgb.grid_col_header_text = g_HeaderText;
            cgb.grid_col_name = g_Cols;
            cgb.grid_col_Count = g_HeaderText.Length;
            int SizeSD = chkShowSaveDefault.Checked ? 75 : 0;
            if (cls_app_static_var.Sell_Union_Flag == "")
            {
                int[] g_Width = {
                    80, 130,   90,
                    90, 110,  90,    90,
                    90,  90,  90, 130 ,  SizeSD,
                    130, 90, 130, 130 , 200,
                    100,  80, 80,  80,   80, 80,
                    80 ,  80, 90,  90,   90,
                    cls_app_static_var.Using_Mileage_TF ,
                    130, 80, 130,  80,
                      80, 200, 0 , 130
                                };
                cgb.grid_col_w = g_Width;
            }
            else
            {

                int[] g_Width = {
                 80, 130,   90,
                    90, 110,  90,    90,
                    90,  90,  90, 130 ,  SizeSD,
                    130, 90, 130, 130 , 200,
                    100,  80, 80,  80,   80, 80,
                    80 ,  80, 90,  90,   90,
                    cls_app_static_var.Using_Mileage_TF ,
                    130, 80, 130,  80,
                      80, 200, 0 , 130
                                };
                cgb.grid_col_w = g_Width;
            }

            Boolean[] g_ReadOnly = { true , true,  true, true, true
                                    ,true , true,  true, true ,true
                                    ,true , true,  true, true ,true
                                    ,true , true,  true, true ,true
                                    ,true , true,  true, true ,true
                                    ,true , true,  true, true ,true
                                    ,true , true,  true, true ,true
                                    ,true , true,
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft

                               ,DataGridViewContentAlignment.MiddleCenter

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter

                               ,DataGridViewContentAlignment.MiddleCenter

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft

                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight

                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight

                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft

                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft

                                ,DataGridViewContentAlignment.MiddleLeft

                              };
            cgb.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[14] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[16] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[17] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[18] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[19] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[20] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[21] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[22] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[23] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[24] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[25] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[26] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[27] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[28] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[29] = cls_app_static_var.str_Grid_Currency_Type;

            cgb.grid_cell_format = gr_dic_cell_format;

        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = {    ds.Tables[base_db_name].Rows[fi_cnt][0]
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
                                ,ds.Tables[base_db_name].Rows[fi_cnt][27]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][28]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][29]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][30]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][31]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][32]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][33]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][34]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][35]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][36]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][37]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][38]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][39]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][40]
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
                    cgb_Pop.db_grid_Popup_Base(2, "구매_코드", "구매종류", "SellCode", "SellTypeName", strSql);
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
                    Tsql = Tsql + " And  ShowMemberCenter = 'Y' ";
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

                    cgb_Pop.db_grid_Popup_Base(2, "구매_코드", "구매종류", "SellCode", "SellTypeName", Tsql);
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
                cgbp5.dGridView_Put_baseinfo(dGridView_Sell_Item, "item");

                cls_Grid_Base_info_Put cgbp6 = new cls_Grid_Base_info_Put();
                cgbp6.dGridView_Put_baseinfo(dGridView_Sell_Cacu, "cacu");

                cls_Grid_Base_info_Put cgbp7 = new cls_Grid_Base_info_Put();
                cgbp7.dGridView_Put_baseinfo(dGridView_Sell_Rece, "rece");

                cls_Grid_Base_info_Put cgbp10 = new cls_Grid_Base_info_Put();
                cgbp10.dGridView_Put_baseinfo(dGridView_Up_S, "saveup");

                cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
                cgbp11.dGridView_Put_baseinfo(dGridView_Up_N, "nominup");



                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                tabC_1.SelectedIndex = 0;
                T_Search_Nubmer = "";


                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtMbid);

                Reset_Chart_Total();
                opt_Ed_1.Checked = true;  opt_sell_1.Checked = true;
                radio_S2.Checked = true;  
                //radioB_S.Checked = true;  radioB_R.Checked = true;
                radioB_SellTF1.Checked = true;
                radioB_REC_0.Checked = true; 
                combo_Se.SelectedIndex = -1;
                
            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                
                cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
                cgbp5.dGridView_Put_baseinfo(dGridView_Sell_Item, "item");

                cls_Grid_Base_info_Put cgbp6 = new cls_Grid_Base_info_Put();
                cgbp6.dGridView_Put_baseinfo(dGridView_Sell_Cacu, "cacu");

                cls_Grid_Base_info_Put cgbp7 = new cls_Grid_Base_info_Put();
                cgbp7.dGridView_Put_baseinfo(dGridView_Sell_Rece, "rece");

                cls_Grid_Base_info_Put cgbp10 = new cls_Grid_Base_info_Put();
                cgbp10.dGridView_Put_baseinfo(dGridView_Up_S, "saveup");

                cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
                cgbp11.dGridView_Put_baseinfo(dGridView_Up_N, "nominup");

                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                tabC_1.SelectedIndex = 0;
                T_Search_Nubmer = "";
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                if (Check_TextBox_Error() == false) return;

                txt_P_1.Text = ""; txt_P_2.Text = ""; txt_P_2_2.Text = ""; txt_P_3.Text = "";
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
                frmBase_Excel_Dev e_f = new frmBase_Excel_Dev();
                e_f.Send_Export_Excel_Info += new frmBase_Excel_Dev.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
                e_f.ShowDialog();
                //cgb.ExportExcel(this.Text);
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


        private DevExpress.XtraGrid.Views.Grid.GridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // this.Text; // "Sell_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }

       


        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            //SendKeys.Send("{TAB}");
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
            DXVGrid.GridView view = dGridView_Base;

            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            DViewInfo.GridHitInfo info = view.CalcHitInfo(pt);

            if (!info.InDataRow)
                return;

            int rIdx = info.RowHandle;

           ToolStripMenuItem tm = (ToolStripMenuItem)sender;
           if (tm.Name.ToString() == "MenuItem_Sell_1")
           {
               string Send_Nubmer = ""; string Send_Name = ""; ; string Send_OrderNumber = "";
               Send_OrderNumber = dGridView_Base.GetRowCellValue(rIdx, dGridView_Base.Columns["OrderNumber"]).ToString();
               Send_Nubmer = dGridView_Base.GetRowCellValue(rIdx, dGridView_Base.Columns["mbid2"]).ToString();
               //Send_Name = dGridView_Base.GetRowCellValue(rIdx, dGridView_Base.Columns["mname"]).ToString();
               Send_Sell_Number(Send_Nubmer, Send_Name, Send_OrderNumber);   //부모한테 이벤트 발생 신호한다.
           }
           
           if (tm.Name.ToString() == "MenuItem_Mem_1")
           {
               string Send_Nubmer = ""; string Send_Name = "";
                Send_Nubmer = dGridView_Base.GetRowCellValue(rIdx, dGridView_Base.Columns["mbid2"]).ToString();
                //Send_Name = dGridView_Base.GetRowCellValue(rIdx, dGridView_Base.Columns["mname"]).ToString();
                Send_Mem_Number(Send_Nubmer, Send_Name);   //부모한테 이벤트 발생 신호한다.
           }

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

        private void tabC_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabC_1.SelectedTab.Name == "tab_save")  //
            {
                if (dGridView_Up_S.RowCount == 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                    cgbp.dGridView_Put_baseinfo(this, dGridView_Up_S, "saveup", T_Search_Nubmer);
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }

            if (tabC_1.SelectedTab.Name == "tab_nom")  ////
            {
                if (dGridView_Up_N.RowCount == 0)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    cls_Grid_Base_info_Put cgbp2 = new cls_Grid_Base_info_Put();
                    cgbp2.dGridView_Put_baseinfo(this, dGridView_Up_N, "nominup", T_Search_Nubmer);
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
        }

        private void dGridView_Base_DoubleClick_1(object sender, EventArgs e)
        {
            DXVGrid.GridView view = (DXVGrid.GridView)sender;

            if (view == null) return;

            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            DViewInfo.GridHitInfo info = view.CalcHitInfo(pt);

            //"회원번호", "성명", "마감_시작일"  ,"마감_종료일"  ,"지급_일자"  
            if (info.InDataRow && info.Column != view.Columns["선택"])
            {
                string Send_Nubmer = string.Empty
                    , Send_Name = string.Empty
                    , Send_OrderNumber = string.Empty;

                Send_OrderNumber = view.GetRowCellValue(info.RowHandle, view.Columns["OrderNumber"]).ToString();
                Send_Nubmer = view.GetRowCellValue(info.RowHandle, view.Columns["mbid2"]).ToString();
                //Send_Name = view.GetRowCellValue(info.RowHandle, view.Columns["mname"]).ToString();
                Send_Sell_Number(Send_Nubmer, Send_Name, Send_OrderNumber);   //부모한테 이벤트 발생 신호한다.

            }
        }

        private void dGridView_Base_RowCellClick(object sender, DXVGrid.RowCellClickEventArgs e)
        {
            T_Search_Nubmer = "";
            //if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[1].Value != null))
            if (e.RowHandle > -1 && e.Column.Name != "OrderNumber")
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                tabC_1.SelectedIndex = 0;

                DXVGrid.GridView view = (DXVGrid.GridView)sender;

                string T_OrderNumber = view.GetRowCellValue(e.RowHandle, view.Columns["OrderNumber"]).ToString();
                string M_Nubmer = view.GetRowCellValue(e.RowHandle, view.Columns["mbid2"]).ToString();

                cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
                cgbp5.dGridView_Put_baseinfo(this, dGridView_Sell_Item, "item", "", T_OrderNumber);

                cls_Grid_Base_info_Put cgbp6 = new cls_Grid_Base_info_Put();
                cgbp6.dGridView_Put_baseinfo(this, dGridView_Sell_Cacu, "cacu", "", T_OrderNumber);

                cls_Grid_Base_info_Put cgbp7 = new cls_Grid_Base_info_Put();
                cgbp7.dGridView_Put_baseinfo(this, dGridView_Sell_Rece, "rece", "", T_OrderNumber);



                //cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                //cgbp.dGridView_Put_baseinfo(this, dGridView_Up_S, "saveup", M_Nubmer);


                //cls_Grid_Base_info_Put cgbp2 = new cls_Grid_Base_info_Put();
                //cgbp2.dGridView_Put_baseinfo(this, dGridView_Up_N, "nominup", M_Nubmer);


                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }
    }
    
}
