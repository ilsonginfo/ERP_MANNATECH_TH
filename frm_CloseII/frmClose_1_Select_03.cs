﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLM_Program
{
    public partial class frmClose_1_Select_03 : Form
    {

        
        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        private const string base_db_name = "tbl_DB";
        private const string base_Closedb_name = "tbl_CloseTotal_02";
        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Sum = new cls_Grid_Base();

        private int Data_Set_Form_TF = 0;

        private int Form_Load_TF = 0;

        public frmClose_1_Select_03()
        {
            InitializeComponent();
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
            cfm.button_flat_change(button1);  
            
        }

        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Form_Load_TF = 0;
            Data_Set_Form_TF = 0;
            //dGridView_Base.Dock = DockStyle.Fill;
            panel8.Dock = DockStyle.Fill;


            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_Close_Grade_ComboBox(combo_Grade, combo_Grade_Code);
            cpbf.Put_Close_GradeP_ComboBox(combo_GradeP, combo_GradeP_Code);

            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
            {
                tab_Detail_02.TabPages.Remove(tab_save);                
            }

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                tab_Detail_02.TabPages.Remove(tab_nom);               
            }


            mtxtFromDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtFromDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtToDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtToDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtPayDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtPayDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            txt_ETC1.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC3.BackColor = cls_app_static_var.txt_Enable_Color;

            txt_ETC4.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC5.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC6.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC7.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC8.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC9.BackColor = cls_app_static_var.txt_Enable_Color;

            txt_ETC10.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC11.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC12.BackColor = cls_app_static_var.txt_Enable_Color;


            //tab_Detail_01.TabPages.Remove(tab_Save_D);
            //tab_Detail_01.TabPages.Remove(tabPage5);            
            //tab_Detail_01.TabPages.Remove(tabPage6);

            //tab_Detail_02.TabPages.Remove(tabPage4);
            tab_Detail_02.TabPages.Remove(tab_etc);
            
            tab_Detail_02.Width = (this.Width / 3) +400;

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
            mtxtMbid.Focus();
        }

        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (Form_Load_TF == 0)
            {
                Form_Load_TF = 1;

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                dGridView_Sum_Base_Header_Reset();
                cgb_Sum.d_Grid_view_Header_Reset(1);

                Clear_Pay_Detail();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<                
            }


            if (cls_User.uSearch_MemberNumber != "")
            {
                Data_Set_Form_TF = 1 ;                
                mtxtMbid.Text = cls_User.uSearch_MemberNumber;
                cls_User.uSearch_MemberNumber = "";

                EventArgs ee1 = null; Select_Button_Click(butt_Select, ee1);
                Data_Set_Form_TF = 0;
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
                if (e.KeyValue == 123 ||  e.KeyValue == 119 || e.KeyValue == 112)
                    Base_Button_Click(T_bt, ee1);
                if (e.KeyValue == 113)
                    Select_Button_Click(T_bt, ee1);
            }
        }

        private void Select_Button_Click(object sender, EventArgs e)
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            dGridView_Sum_Base_Header_Reset();
            cgb_Sum.d_Grid_view_Header_Reset(1);

            Clear_Pay_Detail();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            if (Check_TextBox_Error() == false) return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            combo_Grade_Code.SelectedIndex = combo_Grade.SelectedIndex;
            combo_GradeP_Code.SelectedIndex = combo_GradeP.SelectedIndex;

            Base_Grid_Set();  //뿌려주는 곳
            this.Cursor = System.Windows.Forms.Cursors.Default;

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

                Clear_Pay_Detail();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                radio_PayTF1.Checked = true;
                combo_Grade.SelectedIndex = -1;

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtMbid);


            }

            else if (bt.Name == "butt_Excel")
            {
                frmBase_Excel e_f = new frmBase_Excel();
                e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
                e_f.ShowDialog();
            }

            else if (bt.Name == "button1")
            {
                frmBase_Excel e_f = new frmBase_Excel();
                e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info_Sub);
                e_f.ShowDialog();
            }

            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }
        }

        private DataGridView e_f_Send_Export_Excel_Info_Sub(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            cls_form_Meth cm = new cls_form_Meth();
            Excel_Export_File_Name = this.Text; // cm._chang_base_caption_search ("주간_마감_회원별");
            Excel_Export_From_Name = this.Name;
            return dGridView_Pay_8;


        }



        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            cls_form_Meth cm = new cls_form_Meth();
            Excel_Export_File_Name = this.Text; // cm._chang_base_caption_search ("주간_마감_회원별");
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;


        }






        private void Make_Base_Query(ref string Tsql)
        {


            //string[] g_HeaderText = {"회원번호", "성명", "마감_시작일"  ,"마감_종료일"  ,"지급_일자"  
            //                    , ""     , "총매출PV"  , "총매출액"   , ""    , ""   
            //                    , ""  ,   "등급", "현직급"   , ""  , ""   
            //                    , ""   , "" , ""     , ""    , ""     

            //                    , "스타보너스"   , "바이너리보너스" , "추천매칭"     , "바이너리공제"    , ""     
            //                    , ""   , "" , ""     , ""    , ""     
            //                    , ""   , "" , ""     , ""    , ""  
            //                    , ""   , "" , ""     , ""    , ""     


            //                    , "반품공제액"  , "수당합" , "소득세"  , "주민세"  , "실지급액"  
            //                    , ""   , "" , ""     , ""    , ""     

            //                    ,"은행명" ,   "은행코드",   "계좌번호" ,"예금주",  "주민번호"
            //                    ,"센타"
            //                        };
            cls_form_Meth cm = new cls_form_Meth();

            //스타보너스 ,  바이너리보너스 , 추천매칭
            Tsql = "Select  ";


            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " tbl_ClosePay_01_Mod.mbid + '-' + Convert(Varchar,tbl_ClosePay_01_Mod.mbid2) ";
            else
                Tsql = Tsql + " tbl_ClosePay_01_Mod.mbid2 ";

            Tsql = Tsql + " ,tbl_ClosePay_01_Mod.M_Name ";
            Tsql = Tsql + " ,LEFT(tbl_ClosePay_01_Mod.FromEndDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_01_Mod.FromEndDate,4),2) + '-' + RIGHT(tbl_ClosePay_01_Mod.FromEndDate,2) ";
            Tsql = Tsql + " , LEFT(tbl_ClosePay_01_Mod.ToEndDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_01_Mod.ToEndDate,4),2) + '-' + RIGHT(tbl_ClosePay_01_Mod.ToEndDate,2) ";
            Tsql = Tsql + " , LEFT(tbl_ClosePay_01_Mod.PayDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_01_Mod.PayDate,4),2) + '-' + RIGHT(tbl_ClosePay_01_Mod.PayDate,2) ";

            Tsql = Tsql + " ,  ''  ";
            Tsql = Tsql + " , SellPV01 + SellPV02 + SellPV03   ,  SellPrice01 + SellPrice02 + SellPrice03  ";
            Tsql = Tsql + " ,  SellCV01 + SellCV02 + SellCV03  , 0  ";

            Tsql = Tsql + " , 0 ,  ISnull(C1.Grade_Name,'') ";
            Tsql = Tsql + " ,  ISnull(C2.Grade_Name,'') ,  ISnull(C4.Grade_Name,'') , 0 ";

            Tsql = Tsql + " ,0 , 0 ,  0 , '' , '' ";

            Tsql = Tsql + " ,Etc_Pay , Allowance1 , Allowance2 , Allowance3 , Allowance4   ";

            Tsql = Tsql + " ,Allowance5 , Allowance6 ,  Allowance7 ,  Allowance8 , Allowance9 ";
            Tsql = Tsql + " ,(Allowance10 + Allowance11 + Allowance12) AS Allowance10 , 0 ,  0 , 0 , 0 ";
            Tsql = Tsql + " ,0 , 0 ,  0 , Etc_Pay , Cur_DedCut_Pay  ";

            Tsql = Tsql + " , 0 , SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
            Tsql = Tsql + " ,0 , 0 ,  0 , tbl_Memberinfo.Addcode1 , tbl_Memberinfo.Address1 + ' ' + tbl_Memberinfo.Address2 ";

            Tsql = Tsql + " ,tbl_Bank.bankname , tbl_Memberinfo.bankcode, tbl_Memberinfo.BankAccnt , tbl_Memberinfo.bankowner " ; 


            Tsql = Tsql + ", tbl_Memberinfo.Cpno  ";

            Tsql = Tsql + ", isnull(tbl_Business.Name,'') AS bname , tbl_ClosePay_01_Mod.Remarks1 ";

            Tsql = Tsql + " From tbl_ClosePay_01_Mod (nolock) ";   
         
            //Tsql = Tsql + " LEFT Join tbl_ClosePay_01_Sell_Mod  (nolock) On tbl_ClosePay_01_Mod.mbid=tbl_ClosePay_01_Sell_Mod.mbid " ;
            //Tsql = Tsql + " And tbl_ClosePay_01_Mod.mbid2=tbl_ClosePay_01_Sell_Mod.mbid2";
            //Tsql = Tsql + " And tbl_ClosePay_01_Mod.ToEndDate=tbl_ClosePay_01_Sell_Mod.ToEndDate";
            //Tsql = Tsql + " And tbl_ClosePay_01_Sell_Mod.SellCode ='01' ";
        
            Tsql = Tsql + " LEFT Join tbl_Memberinfo  (nolock) On tbl_ClosePay_01_Mod.mbid=tbl_Memberinfo.mbid ";
            Tsql = Tsql + " And tbl_ClosePay_01_Mod.mbid2=tbl_Memberinfo.mbid2";

            Tsql = Tsql + " Left Join tbl_Business  (nolock) On tbl_Memberinfo.businesscode=tbl_Business.ncode And tbl_Memberinfo.Na_code = tbl_Business.Na_code";
            Tsql = Tsql + " Left Join tbl_Bank  (nolock) On tbl_Memberinfo.bankcode=tbl_Bank.ncode And tbl_Memberinfo.Na_code = tbl_Bank.Na_code   ";
            Tsql = Tsql + " Left Join tbl_Class C2  (nolock) On tbl_ClosePay_01_Mod.CurGrade=C2.Grade_Cnt ";
            Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On tbl_ClosePay_01_Mod.BeforeGrade = C1.Grade_Cnt ";            
            Tsql = Tsql + " Left Join tbl_Class_P C4  (nolock) On tbl_ClosePay_01_Mod.CurPoint = C4.Grade_Cnt "; 

        }



        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = " Where tbl_ClosePay_01_Mod.ToEndDate <> ''  ";

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
                        strSql = strSql + " And tbl_ClosePay_01_Mod.Mbid ='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_ClosePay_01_Mod.Mbid2 = " + Mbid2;
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
                        strSql = strSql + " And tbl_ClosePay_01_Mod.Mbid >='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_ClosePay_01_Mod.Mbid2 >= " + Mbid2;
                }

                if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And tbl_ClosePay_01_Mod.Mbid <='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_ClosePay_01_Mod.Mbid2 <= " + Mbid2;
                }
            }


            //회원명으로 검색
            if (txtName.Text.Trim() != "")
                strSql = strSql + " And tbl_ClosePay_01_Mod.M_Name Like '%" + txtName.Text.Trim() + "%'";


            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";



            if (txt_Us_num.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.Us_Num = '" + txt_Us_num.Text.Trim() + "'";
            



           if ((mtxtFromDate1.Text.Replace("-", "").Trim() != "") && (mtxtFromDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_ClosePay_01_Mod.FromEndDAte = '" + mtxtFromDate1.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtFromDate1.Text.Replace("-", "").Trim() != "") && (mtxtFromDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_ClosePay_01_Mod.FromEndDAte >= '" + mtxtFromDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_ClosePay_01_Mod.FromEndDate <= '" + mtxtFromDate2.Text.Replace("-", "").Trim() + "'";
            }


            //기록일자로 검색 -1
            if ((mtxtToDate1.Text.Replace("-", "").Trim() != "") && (mtxtToDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_ClosePay_01_Mod.ToEndDate = '" + mtxtToDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtToDate1.Text.Replace("-", "").Trim() != "") && (mtxtToDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_ClosePay_01_Mod.ToEndDate >= '" + mtxtToDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_ClosePay_01_Mod.ToEndDate <= '" + mtxtToDate2.Text.Replace("-", "").Trim() + "'";
            }


            //기록일자로 검색 -1
            if ((mtxtPayDate1.Text.Replace("-", "").Trim() != "") && (mtxtPayDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_ClosePay_01_Mod.PayDate = '" + mtxtPayDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtPayDate1.Text.Replace("-", "").Trim() != "") && (mtxtPayDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_ClosePay_01_Mod.PayDate >= '" + mtxtPayDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_ClosePay_01_Mod.PayDate <= '" + mtxtPayDate2.Text.Replace("-", "").Trim() + "'";
            }

            if (txtToEndDate_Code.Text != "")
                strSql = strSql + " And tbl_ClosePay_01_Mod.ToEndDate = '" + txtToEndDate.Text + "'";

            if (combo_Grade_Code.Text != "")
                strSql = strSql + " And tbl_ClosePay_01_Mod.CurGrade = " + combo_Grade_Code.Text  ;

            if (combo_GradeP_Code.Text != "")
                strSql = strSql + " And tbl_ClosePay_01_Mod.CurPoint = " + combo_GradeP_Code.Text;

            if (radio_PayTF1.Checked == true)
                strSql = strSql + " And tbl_ClosePay_01_Mod.TruePayment > 0  ";

            if (radio_PayTF3.Checked == true)
                strSql = strSql + " And tbl_ClosePay_01_Mod.TruePayment = 0  ";


            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


         


            Tsql = Tsql + strSql;
            Tsql = Tsql + " Order by tbl_ClosePay_01_Mod.ToEndDAte DESC , tbl_ClosePay_01_Mod.Mbid, tbl_ClosePay_01_Mod.Mbid2 ";            
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


            //foreach (string tkey in Center_Pr.Keys)
            //{
            //    Push_data(series_Item, tkey, Center_Pr[tkey]);
            //}


            if (gr_dic_text.Count > 0)
            {
                put_Sum_Dataview(ds, ReCnt);                
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }



        private void dGridView_Base_Header_Reset()
        {

            cgb.grid_col_Count = 57;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 3;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //스타보너스 ,  바이너리보너스 , 추천매칭
            string[] g_HeaderText = {"회원번호", "성명", "마감_시작일"  ,"마감_종료일"  ,"지급_일자"  
                                , ""     , "총매출PV"  , "총매출액"   , "_총매출CV"    , ""   
                                , ""  ,   "전직급", "현직급"   , "등급"  , ""   
                                , ""   , "" , ""     , ""    , ""     

                                , "_기타수당" ,"직접판매수당"   , "추천수당" , "직접판매수당_B"     , "추천수당_B"    
                                , "비긴즈수당"   , "후원수당" , "추천매칭수당"     , "추천관리수당"    , "동급적용수당"     
                                , "직급수당"   , "" , ""     , ""    , ""  
                                , ""   , "" , ""     , "기타수당"    , "반품공제액"     


                                , "_천원이하보관금"  , "수당합" , "소득세"  , "주민세"  , "실지급액"  
                                , ""   , "" , ""     , "우편번호"    , "주소"     

                                ,"은행명" ,   "은행코드",   "계좌번호" ,"예금주",  "주민번호"
                                ,"센타" , "비고"
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 100 , 100, 100, 100
                            , 0, 10   ,10, 0, 0
                            , 0 , 10  ,10 , 10, 0
                             , 0, 0,0 , 0, 0 

                             , 0, 100,100 , 100, 100 
                             , 10, 10,10 , 10, 10 
                             , 10, 0,0 , 0, 0 
                             , 0, 0,0 , 10, 10 

                             , 0, 10,10 , 10, 10 
                             , 0, 0,0 , 10, 10 
                             , 100 , 100,100 , 100 , 100 
                             , 100, 100
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
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true   ,true  
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter//5    
  
                               ,DataGridViewContentAlignment.MiddleCenter                            
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleCenter//10

                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter   
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  //15   

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter                            
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter//20


                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  //25   

                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight //30

                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight //35

                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight //40

                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight //45

                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft //50

                               ,DataGridViewContentAlignment.MiddleLeft                            
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft//55

                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                              };
            cgb.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            gr_dic_cell_format[21 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[22 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[23 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[24 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[25 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[25 - 1] = "{0:#,##0.00}"; //cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[26 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[27 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[28 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[29 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[30 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[31 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[32 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[33 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[34 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[35 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[36 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[37 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[38 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[39 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[40 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[41 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[42 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[43 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[44 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[45 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb.grid_cell_format = gr_dic_cell_format;

        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {

            int Col_Cnt = 0;
            object[] row0 = new object[cgb.grid_col_Count];

            while (Col_Cnt < cgb.grid_col_Count)
            {
                if (Col_Cnt == 52 )
                    row0[Col_Cnt] = encrypter .Decrypt ( ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt].ToString () ) ;
                else if  (Col_Cnt == 54 )
                    row0[Col_Cnt] = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt].ToString(),"Cpno");
                else if  (Col_Cnt == 24 )
                    row0[Col_Cnt] = string.Format("{0:#,##0.00}", ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt]);
                else
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
                        
            //"스타보너스"   , "바이너리보너스"    , "추천매칭"   
            Sum_dic["직접판매수당"] = 0;            
            Sum_dic["추천수당"] = 0;

            Sum_dic["직접판매수당_B"] = 0;
            Sum_dic["추천수당_B"] = 0;
            Sum_dic["비긴즈수당"] = 0;

            Sum_dic["후원수당"] = 0;
            Sum_dic["추천매칭수당"] = 0;

            Sum_dic["추천관리수당"] = 0;
            Sum_dic["동급적용수당"] = 0;

            Sum_dic["직급수당"] = 0;

            Sum_dic["반품공제액"] = 0;
            Sum_dic["기타수당"] = 0;
            Sum_dic["수당합"] = 0;
            Sum_dic["소득세합"] = 0;
            Sum_dic["주민세합"] = 0;
            Sum_dic["실지급액합"] = 0;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Sum_dic["직접판매수당"] = Sum_dic["직접판매수당"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance1"].ToString());
                Sum_dic["추천수당"] = Sum_dic["추천수당"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance2"].ToString());
                Sum_dic["직접판매수당_B"] = Sum_dic["직접판매수당_B"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance3"].ToString());
                Sum_dic["추천수당_B"] = Sum_dic["추천수당_B"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance4"].ToString());
                Sum_dic["비긴즈수당"] = Sum_dic["비긴즈수당"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance5"].ToString());

                Sum_dic["후원수당"] = Sum_dic["후원수당"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance6"].ToString());
                Sum_dic["추천매칭수당"] = Sum_dic["추천매칭수당"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance7"].ToString());

                Sum_dic["추천관리수당"] = Sum_dic["추천관리수당"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance8"].ToString());
                Sum_dic["동급적용수당"] = Sum_dic["동급적용수당"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance9"].ToString());

                Sum_dic["직급수당"] = Sum_dic["직급수당"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance10"].ToString());
                
                Sum_dic["반품공제액"] = Sum_dic["반품공제액"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Cur_DedCut_Pay"].ToString());
                Sum_dic["기타수당"] = Sum_dic["기타수당"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Etc_Pay"].ToString());
                

                Sum_dic["수당합"] = Sum_dic["수당합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumAllAllowance"].ToString());
                Sum_dic["소득세합"] = Sum_dic["소득세합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InComeTax"].ToString());
                Sum_dic["주민세합"] = Sum_dic["주민세합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ResidentTax"].ToString());
                Sum_dic["실지급액합"] = Sum_dic["실지급액합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TruePayment"].ToString());
            }

            int f_cnt = 0 ;
            foreach (string t_key in Sum_dic.Keys )
            {
                object[] row0 = { cm._chang_base_caption_search (t_key)
                                , Sum_dic[t_key] 
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

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[2 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb_Sum.grid_cell_format = gr_dic_cell_format;
    
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


            if (mtxtFromDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtFromDate1.Text, mtxtFromDate1, "Date") == false)
                {
                    mtxtFromDate1.Focus();
                    return false;
                }
            }

            if (mtxtFromDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtFromDate2.Text, mtxtFromDate2, "Date") == false)
                {
                    mtxtFromDate2.Focus();
                    return false;
                }
            }


            if (mtxtToDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtToDate1.Text, mtxtToDate1, "Date") == false)
                {
                    mtxtToDate1.Focus();
                    return false;
                }
            }

            if (mtxtToDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtToDate2.Text, mtxtToDate2, "Date") == false)
                {
                    mtxtToDate2.Focus();
                    return false;
                }
            }

            if (mtxtPayDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtPayDate1.Text, mtxtPayDate1, "Date") == false)
                {
                    mtxtPayDate1.Focus();
                    return false;
                }
            }

            if (mtxtPayDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtPayDate2.Text, mtxtPayDate2, "Date") == false)
                {
                    mtxtPayDate2.Focus();
                    return false;
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



        private void S_MtxtData_KeyPress(object sender, KeyPressEventArgs e)
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

        private void S_MtxtMbid_TextChanged(object sender, EventArgs e)
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

            ////if (tb.Name == "txtSellCode")
            ////{
            ////    if (tb.Text.Trim() == "")
            ////        txtSellCode_Code.Text = "";
            ////    else if (Sw_Tab == 1)
            ////        Ncod_Text_Set_Data(tb, txtSellCode_Code);
            ////}

            if (tb.Name == "txtCenter")
            {
                if (tb.Text.Trim() == "")
                    txtCenter_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtCenter_Code);
            }

            if (tb.Name == "txtToEndDate")
            {
                if (tb.Text.Trim() == "")
                    txtToEndDate_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtToEndDate_Code);
            }
            //if (tb.Name == "txtR_Id")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtR_Id_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code);
            //}

            //if (tb.Name == "txtR_Id2")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtR_Id_Code2.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code2);
            //}

            //if (tb.Name == "txt_ItemName2")
            //{
            //    if (tb.Text.Trim() == "")
            //        txt_ItemName_Code2.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txt_ItemName_Code2);
            //}
        }




        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
            //if (tb.Name == "txtSellCode")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtSellCode_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            if (tb.Name == "txtCenter")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtCenter_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtToEndDate")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, txtToEndDate_Code, "");
                else
                    Ncod_Text_Set_Data(tb, txtToEndDate_Code);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            //if (tb.Name == "txtR_Id")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtR_Id_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            //if (tb.Name == "txtR_Id2")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtR_Id_Code2, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code2);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            //if (tb.Name == "txt_ItemName2")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txt_ItemName_Code2, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txt_ItemName_Code2);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}
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
            cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            cgb_Pop.Base_Location_obj = tb;

            if (strSql != "")
            {
                if (tb.Name == "txtCenter" || tb.Name == "txtCenter2")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txt_ItemName2")
                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", strSql);

                if (tb.Name == "txtSellCode")
                {
                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", strSql);                 
                }

                if (tb.Name == "txtToEndDate")
                {
                    cgb_Pop.db_grid_Popup_Base(2, "지급_일자", "마감_종료일", "PayDate", "ToEndDate", strSql);
                }

            }
            else
            {
                if (tb.Name == "txtCenter" || tb.Name == "txtCenter2")
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

                if (tb.Name == "txt_ItemName2")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name    ";
                    Tsql = Tsql + " From tbl_Goods (nolock) ";
                    //Tsql = Tsql + " Where GoodUse = 0 ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", Tsql);
                }

                if (tb.Name == "txtSellCode")
                {
                    string Tsql;
                    Tsql = "Select SellCode ,SellTypeName    ";
                    Tsql = Tsql + " From tbl_SellType (nolock) ";
                    Tsql = Tsql + " Order by SellCode ";

                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", Tsql);                  
                }


                if (tb.Name == "txtToEndDate")
                {
                    string Tsql;
                    Tsql = "Select PayDate , ToEndDate    ";
                    Tsql = Tsql + " From " + base_Closedb_name + " (nolock) ";                   
                    Tsql = Tsql + " Order by ToEndDate DESC ";

                    cgb_Pop.db_grid_Popup_Base(2, "지급_일자", "마감_종료일", "PayDate", "ToEndDate", Tsql);
                }

            }
        }



        private void Ncod_Text_Set_Data(TextBox tb, TextBox tb1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            if (tb.Name == "txtCenter" || tb.Name == "txtCenter2")
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

            if (tb.Name == "txt_ItemName2")
            {
                Tsql = "Select Ncode , Name    ";
                Tsql = Tsql + " From tbl_Goods (nolock) ";
                Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%'";
            }

            if (tb.Name == "txtSellCode")
            {
                Tsql = "Select SellCode ,SellTypeName    ";
                Tsql = Tsql + " From tbl_SellType (nolock) ";
                Tsql = Tsql + " Where SellCode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    SellTypeName like '%" + tb.Text.Trim() + "%'";
            }

            if (tb.Name == "txtToEndDate")
            {
                Tsql = "Select  PayDate , ToEndDate    ";
                Tsql = Tsql + " From " + base_Closedb_name + " (nolock) ";
                Tsql = Tsql + " Where ToEndDate like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    PayDate  like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " Order by ToEndDate DESC ";
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
            ct.Search_Date_TextBox_Put(mtxtFromDate1, mtxtFromDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }



        private void radioB_R_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtToDate1, mtxtToDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }

        private void radioB_P_Base_Click(object sender, EventArgs e)
        {
            Data_Set_Form_TF = 1;
            //RadioButton _Rb = (RadioButton)sender;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtPayDate1, mtxtPayDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }


        private void Clear_Pay_Detail()
        {
            tab_Detail_01.SelectedIndex = 0;
            tab_Detail_02.SelectedIndex = 0;
            
            cls_Grid_Base cgb_P1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(dGridView_Pay_1, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Base_Header_Reset(dGridView_Pay_2, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Base_Header_Reset(dGridView_Pay_3, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);
            
            dGridView_Base_Header_Reset(dGridView_Detail_2, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_SellData_Header_Reset(dGridView_Detail_1, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Up_S_Header_Reset(dGridView_Detail_3, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Up_S_Header_Reset(dGridView_Detail_4, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);


            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(tab_Detail_02);
        }

        
        
        private void dGridView_Base_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Clear_Pay_Detail();            
            
            //"회원번호", "성명", "마감_시작일"  ,"마감_종료일"  ,"지급_일자"  
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string T_Mbid = "" , ToEndDate = "" ;                
                
                T_Mbid = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();                
                ToEndDate = (sender as DataGridView).CurrentRow.Cells[3].Value.ToString();
                ToEndDate = ToEndDate.Replace("-", "");

                Allowance_Detail(T_Mbid, ToEndDate);

                Pay_Detail(T_Mbid, ToEndDate);
            }
        }


        private void Allowance_Detail(string T_Mbid, string ToEndDate)
        {
            cls_Search_DB csd = new cls_Search_DB();            
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);
        
            cls_Grid_Base cgb_P1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(dGridView_Pay_1, cgb_P1, 1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "1", cgb_P1);  //추천수당

            //dGridView_Base_Header_Reset(dGridView_Pay_2, cgb_P1, 0); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_P1.d_Grid_view_Header_Reset();
            //Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "5", cgb_P1);  //추천보너스

            dGridView_Base_Header_Reset(dGridView_Pay_2, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "2", cgb_P1);  //하선매출 내역

            dGridView_Base_Header_Reset(dGridView_Pay_3, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "3", cgb_P1);  //하선매출 내역


            dGridView_Base_Header_Reset(dGridView_Pay_5, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "4", cgb_P1);  //하선매출 내역

            dGridView_Base_Header_Reset(dGridView_Pay_SP, cgb_P1, 1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "5", cgb_P1);  //압축 추천 점수


            dGridView_Base_Header_Reset(dGridView_Pay_4, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail_Up(ToEndDate, Mbid, Mbid2,  cgb_P1);  //판매내역 역추적



            dGridView_Base_Header_Reset(dGridView_DownPV, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_PV_Detail(ToEndDate, Mbid, Mbid2, "5", cgb_P1);  //하선매출 내역


            dGridView_Base_Header_Reset(dGridView_Pay_7, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "7", cgb_P1);  //추천매칭수당 내역


            dGridView_Base_Header_Reset(dGridView_Pay_8, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "8", cgb_P1);  //추천매칭수당 내역


            dGridView_Base_Header_Reset(dGridView_Pay_9, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "9", cgb_P1);  //추천매칭수당 내역
        }



        private void Pay_Detail(string T_Mbid, string ToEndDate)
        {
            cls_Search_DB csd = new cls_Search_DB();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);
            

            cls_Grid_Base cgb_V1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(dGridView_Detail_2, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();
            Real_Pay_Detail(ToEndDate, Mbid, Mbid2, 1, cgb_V1);  //그룹하선 매출 내역

            dGridView_SellData_Header_Reset(dGridView_Detail_1, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();
            Real_Pay_Detail(ToEndDate, Mbid, Mbid2, cgb_V1);  //본인 매출 내역
            
            dGridView_Up_S_Header_Reset(dGridView_Detail_3, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();
            Base_Grid_Set(ToEndDate, Mbid, Mbid2, "ufn_Up_Search_Save_Close_01", cgb_V1);

            dGridView_Up_S_Header_Reset(dGridView_Detail_4, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();
            Base_Grid_Set(ToEndDate, Mbid, Mbid2,"ufn_Up_Search_Nomin_Close_01", cgb_V1); //추천인 역추적


           // Real_Pay_Detail_ETC(ToEndDate, Mbid, Mbid2); 
        }


        private void Real_Allowance_Detail(string ToEndDate, string Mbid, int Mbid2, string SortOrder, cls_Grid_Base cgb_P)
        {
            string StrSql ="";

            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " RequestMbid + '-' + Convert(Varchar,RequestMbid2) ";
            else
                StrSql = StrSql + " RequestMbid2 ";

            StrSql = StrSql + ",RequestName,DownPV,LineCnt, LevelCnt,GivePay , R_LevelCnt, GivePay     ";
            
            StrSql = StrSql + " From  tbl_Close_DownPV_ALL_01 (nolock) ";
            StrSql = StrSql + " Where SaveMbid = '" + Mbid + "'";
            StrSql = StrSql + " And SaveMbid2 = " + Mbid2 ;
            StrSql = StrSql + " And EndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " And LEFT(SortOrder,1)  ='" + SortOrder + "'";
            StrSql = StrSql + " Order By LevelCnt, LineCnt ";
            
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
                Set_Pay_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            
            if (gr_dic_text.Count > 0)
            {
                //put_Sum_Dataview(ds, ReCnt);                
            }

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();
        
        }


        private void Real_Allowance_PV_Detail(string ToEndDate, string Mbid, int Mbid2, string SortOrder, cls_Grid_Base cgb_P)
        {
            string StrSql = "";

            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " RequestMbid + '-' + Convert(Varchar,RequestMbid2) ";
            else
                StrSql = StrSql + " RequestMbid2 ";

            StrSql = StrSql + ",RequestName,Sell_DownPV DownPV,LineCnt, LevelCnt,OrderNumber,   Case SortOrder When '1' then '일반' When '2' then '소매'  End AS ST1    ";

            StrSql = StrSql + " From  tbl_Close_DownPV_PV_01 (nolock) ";
            StrSql = StrSql + " Where SaveMbid = '" + Mbid + "'";
            StrSql = StrSql + " And SaveMbid2 = " + Mbid2;
            StrSql = StrSql + " And EndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " Order By LevelCnt, LineCnt ";

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
                Set_Up_gr_dic3(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            if (gr_dic_text.Count > 0)
            {
                //put_Sum_Dataview(ds, ReCnt);                
            }

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();

        }


        private void Real_Allowance_Detail(string ToEndDate, string Mbid, int Mbid2, int SortOrder, cls_Grid_Base cgb_P)
        {
            string StrSql = "";

            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " RequestMbid + '-' + Convert(Varchar,RequestMbid2) ";
            else
                StrSql = StrSql + " RequestMbid2 ";

            StrSql = StrSql + ",RequestName, Sell_DownPV AS DownPV ,LineCnt, LevelCnt ";
            StrSql = StrSql + " From  tbl_Close_DownPV_PV_02 (nolock) ";
            StrSql = StrSql + " Where SaveMbid = '" + Mbid + "'";
            StrSql = StrSql + " And SaveMbid2 = " + Mbid2;
            StrSql = StrSql + " And EndDate ='" + ToEndDate + "'";            
            StrSql = StrSql + " Order By LevelCnt, LineCnt ";

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
                Set_Pay_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            if (gr_dic_text.Count > 0)
            {
                //put_Sum_Dataview(ds, ReCnt);                
            }

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();

        }

        private void Real_Allowance_Detail2(string ToEndDate, string Mbid, int Mbid2, string  SortOrder, cls_Grid_Base cgb_P)
        {
            string StrSql = "";

            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " RequestMbid + '-' + Convert(Varchar,RequestMbid2) ";
            else
                StrSql = StrSql + " RequestMbid2 ";

            StrSql = StrSql + ",RequestName, Sell_DownPV AS DownPV ,LineCnt, LevelCnt ,  1 AS GivePay ";
            StrSql = StrSql + " From  tbl_Close_DownPV_PV_02 (nolock) ";
            StrSql = StrSql + " Where SaveMbid = '" + Mbid + "'";
            StrSql = StrSql + " And SaveMbid2 = " + Mbid2;
            StrSql = StrSql + " And EndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " And SortOrder ='" + SortOrder + "'";
            StrSql = StrSql + " Order By LevelCnt, LineCnt ";

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
                Set_Pay_gr_dic2(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            if (gr_dic_text.Count > 0)
            {
                //put_Sum_Dataview(ds, ReCnt);                
            }

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();

        }

        private void Real_Allowance_Detail_Up(string ToEndDate, string Mbid, int Mbid2 , cls_Grid_Base cgb_P)
        {
            string StrSql = "";

            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " SaveMbid + '-' + Convert(Varchar,SaveMbid2) ";
            else
                StrSql = StrSql + " SaveMbid2 ";

            StrSql = StrSql + ",SaveName, DownPV ,OrderNumber, LevelCnt , ST1 ";
            StrSql = StrSql + " From ";
            StrSql = StrSql + " ( " ;
            StrSql = StrSql + " Select EndDate, RequestMbid, RequestMbid2 ,RequestName ,OrderNumber, SaveMbid, SaveMbid2,SaveName, DownPV, LevelCnt,";
            StrSql = StrSql + " Case SortOrder When '1' then '' When '3' then ''    " ;
            StrSql = StrSql + " End AS ST1 ";            
            StrSql = StrSql + " From tbl_Close_DownPV_ALL_01 (nolock)  ";
            StrSql = StrSql + " Left Join tbl_CloseTotal_02   (nolock) On tbl_CloseTotal_02.ToEndDate= tbl_Close_DownPV_ALL_01.EndDate  ";
        
            StrSql = StrSql + " UNION ALL";

            StrSql = StrSql + "  Select EndDate, RequestMbid , RequestMbid2 ,RequestName , OrderNumber,   SaveMbid ,  SaveMbid2,  SaveName, Sell_DownPV  DownPV , LevelCnt,";
            StrSql = StrSql + " '판매누적'  AS ST1 ";            
            StrSql = StrSql + " From tbl_Close_DownPV_PV_02  (nolock) ";
            StrSql = StrSql + " Left Join tbl_CloseTotal_02  (nolock)  On tbl_CloseTotal_02.ToEndDate= tbl_Close_DownPV_PV_02.EndDate  ";

            StrSql = StrSql + " ) AS C ";

            StrSql = StrSql + " Where RequestMbid = '" + Mbid + "'";
            StrSql = StrSql + " And RequestMbid2 = " + Mbid2;
            StrSql = StrSql + " And EndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " Order By ST1, LevelCnt  ";

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
                Set_Up_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            if (gr_dic_text.Count > 0)
            {
                //put_Sum_Dataview(ds, ReCnt);                
            }

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();

        }

        private void dGridView_Base_Header_Reset(DataGridView dGridView, cls_Grid_Base cgb_P, int S_TF = 0)
        {

            cgb_P.grid_col_Count = 10;
            cgb_P.basegrid = dGridView;
            cgb_P.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_P.grid_Frozen_End_Count = 3;
            cgb_P.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            if (S_TF == 0)
            {
                string[] g_HeaderText = {"회원번호", "성명", "금액"  ,"대수"  ,"라인"  
                                , "PV"     , ""  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }
            else if (S_TF == 1)
            {
                string[] g_HeaderText = {"회원번호", "성명", "금액"  ,"대수"  ,"라인"  
                                , "PV"     , ""  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }
            else if (S_TF == 2)
            {
                string[] g_HeaderText = {"회원번호", "성명", "금액_PV"  ,"대수"  ,"주문번호"  
                                , "구분"     , ""  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }


            else if (S_TF == 3 || S_TF == 5)
            {
                string[] g_HeaderText = {"회원번호", "성명", "PV"  ,"대수"  ,"라인"  
                                , "압축대수"     , ""  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }

            else if (S_TF == 6)
            {
                string[] g_HeaderText = {"회원번호", "성명", "점수"  ,"대수"  ,"라인"  
                                , "압축대수"     , ""  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }


            if (S_TF == 2 || S_TF == 3 || S_TF == 5 || S_TF == 1 || S_TF == 6)
            {
                int[] g_Width = { 100, 100 , 100, 100, 100                            
                             , 100, 0,0 , 0, 0 
                            };
                cgb_P.grid_col_w = g_Width;
            }
            else
            {
                int[] g_Width = { 100, 100 , 100, 100, 100                            
                             , 0, 0,0 , 0, 0 
                            };
                cgb_P.grid_col_w = g_Width;
            }



            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                         
                                   };
            cgb_P.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter//5    
  
                               ,DataGridViewContentAlignment.MiddleCenter                            
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter//10                         
                              };
            cgb_P.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb_P.grid_cell_format = gr_dic_cell_format;
        }


        private void Set_Pay_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["RequestName"]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["DownPV"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LevelCnt"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LineCnt"]
 
                                ,""
                                ,""
                                ,""
                                ,""
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }

        private void Set_Pay_gr_dic2(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["RequestName"]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["DownPV"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LevelCnt"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LineCnt"]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["GivePay"] 
                                ,""
                                ,""
                                ,""
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void Set_Up_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["SaveName"]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["DownPV"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LevelCnt"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["ST1"]
                                ,""
                                ,""
                                ,""
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void Set_Up_gr_dic3(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["RequestName"]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["DownPV"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LevelCnt"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["ST1"]
                                ,""
                                ,""
                                ,""
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }







        private void Real_Pay_Detail(string ToEndDate, string Mbid, int Mbid2, int SortOrder, cls_Grid_Base cgb_P)
        {

            string StrSql = "";

            ////if (cls_app_static_var.Member_Number_1 > 0)
            ////    StrSql = StrSql + " RequestMbid + '-' + Convert(Varchar,RequestMbid2) ";
            ////else
            ////    StrSql = StrSql + " RequestMbid2 ";

            StrSql = "Select ";

            StrSql = StrSql + " Cur_PV_1, Cur_PV_2 ";
            StrSql = StrSql + ",Be_PV_1, Be_PV_2 ";
            StrSql = StrSql + ",Sum_PV_1, Sum_PV_2 ";
            StrSql = StrSql + ",Ded_1, Ded_2 ";
            StrSql = StrSql + ",Fresh_1, Fresh_2 ";

            //StrSql = StrSql + ",Regtime, CurPoint_Date_2, CurPoint_Date_3 ";

            StrSql = StrSql + " From  tbl_ClosePay_01_Mod (nolock) ";
            StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
            StrSql = StrSql + " And Mbid2 = " + Mbid2;
            StrSql = StrSql + " And ToEndDate ='" + ToEndDate + "'";

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


            object[] row0 = { "1라인"
                                ,ds.Tables[base_db_name].Rows[0]["Be_PV_1"]  
                                ,ds.Tables[base_db_name].Rows[0]["Cur_PV_1"]
                                ,ds.Tables[base_db_name].Rows[0]["Ded_1"]
                                ,ds.Tables[base_db_name].Rows[0]["Fresh_1"]
 
                                ,ds.Tables[base_db_name].Rows[0]["Sum_PV_1"]
                                ,""
                                ,""
                                ,""
                                ,""
                                 };

            gr_dic_text[1] = row0;

            object[] row1 = { "2라인"
                                ,ds.Tables[base_db_name].Rows[0]["Be_PV_2"]  
                                ,ds.Tables[base_db_name].Rows[0]["Cur_PV_2"]
                                ,ds.Tables[base_db_name].Rows[0]["Ded_2"]
                                ,ds.Tables[base_db_name].Rows[0]["Fresh_2"]
 
                                ,ds.Tables[base_db_name].Rows[0]["Sum_PV_2"]
                                ,""
                                ,""
                                ,""
                                ,""
                                 };

            gr_dic_text[2] = row1;


            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();


            //txt_ETC1.Text = ds.Tables[base_db_name].Rows[0]["Regtime"].ToString();
            //txt_ETC2.Text = ds.Tables[base_db_name].Rows[0]["CurPoint_Date_2"].ToString();
            //txt_ETC3.Text = ds.Tables[base_db_name].Rows[0]["CurPoint_Date_3"].ToString();
        }

        

        private void dGridView_Base_Header_Reset(DataGridView dGridView, cls_Grid_Base cgb_P)
        {

            cgb_P.grid_col_Count = 10;
            cgb_P.basegrid = dGridView;
            cgb_P.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_P.grid_Frozen_End_Count = 3;
            cgb_P.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"라인","이전", "마감기간", "공제"  ,"후레쉬" 
                            , "이월"     , ""  , ""   , ""    , ""                                   
                                };
            cgb_P.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 100 , 100, 100, 100                            
                             , 100, 0,0 , 0, 0 
                            };
            cgb_P.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                         
                                   };
            cgb_P.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight//5    
  
                               ,DataGridViewContentAlignment.MiddleRight                            
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter//10                         
                              };
            cgb_P.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[2 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb_P.grid_cell_format = gr_dic_cell_format;
        }




        private void Real_Pay_Detail(string ToEndDate, string Mbid, int Mbid2,  cls_Grid_Base cgb_P)
        {
            string StrSql = "";

            StrSql = "SELECT SellCode, SellTypeName FROM tbl_SellType ORDER BY SellCode";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;



            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            //string[] g_HeaderText = {"주문종류", "이전금액", "기간금액"  ,"총합금액" ," " 
            //                         , "이전PV", "기간PV"  ,"총합PV" , " " ,"이전반품금액"    
            //                          , "기간반품금액"  , "반품총합금액"  , " " , "이전반품PV"     , "기간반품PV"  
            //                          , "반품총합PV"
            //                    };

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                string SellCode = ds.Tables[base_db_name].Rows[fi_cnt]["SellCode"].ToString();
                string SellTypeName = ds.Tables[base_db_name].Rows[fi_cnt]["SellTypeName"].ToString(); 

                StrSql = "SELECT BeTotalPV, BeTotalCV, BeShamSell,BeAmount ";
                StrSql = StrSql + ",DayTotalPV, DayTotalCV, DayShamSell,DayAmount ";
                StrSql = StrSql + ",SumTotalPV, SumTotalCV, SumShamSell,SumAmount ";

                StrSql = StrSql + ",BeReTotalPV, BeReTotalCV, BeReAmount ";
                StrSql = StrSql + ",DayReTotalPV, DayReTotalCV, DayReAmount " ;
                StrSql = StrSql + ",SumReTotalPV, SumReTotalCV, SumReAmount ";
                StrSql = StrSql + " FROM tbl_ClosePay_01_Sell_Mod (nolock) ";

                StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
                StrSql = StrSql + " And Mbid2 = " + Mbid2;
                StrSql = StrSql + " And ToEndDate ='" + ToEndDate + "'";
                StrSql = StrSql + " And SellCode ='" + SellCode + "'" ;

                DataSet ds2 = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds2, this.Name, this.Text) == false) return;
                int ReCnt2 = Temp_Connect.DataSet_ReCount;
               
                for (int fi_cnt2 = 0; fi_cnt2 <= ReCnt2 - 1; fi_cnt2++)
                {
                    object[] row0 = { SellTypeName
                                ,ds2.Tables[base_db_name].Rows[fi_cnt2]["BeAmount"]  
                                ,ds2.Tables[base_db_name].Rows[fi_cnt2]["DayAmount"]
                                ,ds2.Tables[base_db_name].Rows[fi_cnt2]["SumAmount"]
                                ,"  "

                                ,ds2.Tables[base_db_name].Rows[fi_cnt2]["BeTotalPV"] 
                                ,ds2.Tables[base_db_name].Rows[fi_cnt2]["DayTotalPV"]
                                ,ds2.Tables[base_db_name].Rows[fi_cnt2]["SumTotalPV"]
                                ,"  "
                                ,ds2.Tables[base_db_name].Rows[fi_cnt2]["BeReAmount"]
                                
                                ,ds2.Tables[base_db_name].Rows[fi_cnt2]["DayReAmount"]
                                ,ds2.Tables[base_db_name].Rows[fi_cnt2]["SumReAmount"]
                                ,"  "
                                ,ds2.Tables[base_db_name].Rows[fi_cnt2]["BeReTotalPV"]
                                ,ds2.Tables[base_db_name].Rows[fi_cnt2]["DayReTotalPV"]

                                ,ds2.Tables[base_db_name].Rows[fi_cnt2]["SumReTotalPV"]
                                 };

                    gr_dic_text[fi_cnt2 + 1 ] = row0;
                }                
            }
                     

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();

        }



        private void dGridView_SellData_Header_Reset(DataGridView dGridView, cls_Grid_Base cgb_P)
        {

            cgb_P.grid_col_Count = 16;
            cgb_P.basegrid = dGridView;
            cgb_P.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_P.grid_Frozen_End_Count = 1;
            cgb_P.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"주문종류", "이전금액", "기간금액"  ,"총합금액" ," " 
                                     , "이전PV", "기간PV"  ,"총합PV" , " " ,"이전반품금액"    
                                      , "기간반품금액"  , "반품총합금액"  , " " , "이전반품PV"     , "기간반품PV"  
                                      , "반품총합PV"
                                };
            cgb_P.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 100 , 100, 100, 20
                             , 100, 100,100 , 20, 100 
                             , 100, 100,20,100,100
                             ,100
                            };
            cgb_P.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true     
                                     ,true , true,  true                                
                                   };
            cgb_P.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight
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
                              };
            cgb_P.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[2 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb_P.grid_cell_format = gr_dic_cell_format;

            cgb_P.basegrid.RowHeadersVisible = false;

        }





        private void Base_Grid_Set(string ToEndDate, string Mbid, int Mbid2, string Ufn_Name, cls_Grid_Base cgb_P)
        {
            
            string Tsql = "";

            Tsql = "Select  ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " T_up.mbid + '-' + Convert(Varchar,T_up.mbid2) ";
            else
                Tsql = Tsql + " T_up.mbid2 ";

            Tsql = Tsql + " ,T_up.M_Name ";
            Tsql = Tsql + " ,T_up.curP ";

            Tsql = Tsql + " From " + Ufn_Name;
            Tsql = Tsql + " ('" + Mbid + "'," + Mbid2.ToString() + ",'" + ToEndDate + "') AS T_up";

            Tsql = Tsql + " Where    lvl > 0 ";
            Tsql = Tsql + " Order BY lvl Desc ";

            //당일 등록된 회원을 불러온다.

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
                Set_gr_dic_Line(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();
        }

        private void Set_gr_dic_Line(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][4]                                                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }





        private void dGridView_Up_S_Header_Reset(DataGridView t_Dgv, cls_Grid_Base cgb_P)
        {
            cgb_P.Grid_Base_Arr_Clear();

            cgb_P.grid_col_Count = 5;
            cgb_P.basegrid = t_Dgv; 
            cgb_P.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_P.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"회원_번호"  , "성명"   , "위치"  , ""   , ""        
                                    };
            cgb_P.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 60, 70, 30, 0, 0                               
                            };
            cgb_P.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                                                                                   
                                   };
            cgb_P.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5      
                              };
            cgb_P.grid_col_alignment = g_Alignment;
            cgb_P.basegrid.RowHeadersWidth = 25;

            //cgb_P.basegrid.ColumnHeadersDefaultCellStyle.Font =
            //new Font(cgb_P.basegrid.Font.FontFamily, 8);
        }




        private void Real_Pay_Detail_ETC(string ToEndDate, string Mbid, int Mbid2)
        {


            string StrSql = "";
            string G_Left = "", G_Right = "";
            ////if (cls_app_static_var.Member_Number_1 > 0)
            ////    StrSql = StrSql + " RequestMbid + '-' + Convert(Varchar,RequestMbid2) ";
            ////else
            ////    StrSql = StrSql + " RequestMbid2 ";

            StrSql = "Select ";

            StrSql = StrSql + " G_Cur_PV_4_1, G_Cur_PV_4_2, G_Cur_PV_1 , G_Cur_PV_2 ";
            //StrSql = StrSql + ",GradeCnt4_1, GradeCnt4_2 ";
            //StrSql = StrSql + ",GradeCnt5_1, GradeCnt5_2 ";
            StrSql = StrSql + ",GradeCnt6_1, GradeCnt6_2 ";
            StrSql = StrSql + ",GradeCnt7_1, GradeCnt7_2 ";
            StrSql = StrSql + ",GradeCnt8_1, GradeCnt8_2 ";
            StrSql = StrSql + ",GradeCnt9_1, GradeCnt9_2 ";
            StrSql = StrSql + ",GradeCnt10_1, GradeCnt10_2 ";
            StrSql = StrSql + ",GradeCnt11_1, GradeCnt11_2 ";
            StrSql = StrSql + ",GradeCnt12_1, GradeCnt12_2 ";

            StrSql = StrSql + " From  tbl_ClosePay_01_Mod (nolock) ";
            StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
            StrSql = StrSql + " And Mbid2 = " + Mbid2;
            StrSql = StrSql + " And ToEndDate ='" + ToEndDate + "'";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            //string.Format(cls_app_static_var.str_Currency_Type, T_p);

            txt_ETC1.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Cur_PV_4_1"].ToString()));
            txt_ETC4.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Cur_PV_4_2"].ToString()));

            txt_ETC2.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Cur_PV_1"].ToString()));
            txt_ETC3.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Cur_PV_2"].ToString()));

            //G_Left = ds.Tables[base_db_name].Rows[0]["GradeCnt4_1"].ToString();
            //G_Right = ds.Tables[base_db_name].Rows[0]["GradeCnt4_2"].ToString();
            //txt_ETC4.Text = G_Left + " / " + G_Right;

            //G_Left = ds.Tables[base_db_name].Rows[0]["GradeCnt5_1"].ToString();
            //G_Right = ds.Tables[base_db_name].Rows[0]["GradeCnt5_2"].ToString();
            //txt_ETC5.Text = G_Left + " / " + G_Right;

            G_Left = ds.Tables[base_db_name].Rows[0]["GradeCnt6_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["GradeCnt6_2"].ToString();
            txt_ETC6.Text = G_Left + " / " + G_Right;

            G_Left = ds.Tables[base_db_name].Rows[0]["GradeCnt7_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["GradeCnt7_2"].ToString();
            txt_ETC7.Text = G_Left + " / " + G_Right;


            G_Left = ds.Tables[base_db_name].Rows[0]["GradeCnt8_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["GradeCnt8_2"].ToString();
            txt_ETC8.Text = G_Left + " / " + G_Right;

            G_Left = ds.Tables[base_db_name].Rows[0]["GradeCnt9_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["GradeCnt9_2"].ToString();
            txt_ETC9.Text = G_Left + " / " + G_Right;

            G_Left = ds.Tables[base_db_name].Rows[0]["GradeCnt10_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["GradeCnt10_2"].ToString();
            txt_ETC10.Text = G_Left + " / " + G_Right;

            G_Left = ds.Tables[base_db_name].Rows[0]["GradeCnt11_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["GradeCnt11_2"].ToString();
            txt_ETC11.Text = G_Left + " / " + G_Right;


            //G_Left = ds.Tables[base_db_name].Rows[0]["GradeCnt12_1"].ToString();
            //G_Right = ds.Tables[base_db_name].Rows[0]["GradeCnt12_2"].ToString();
            //txt_ETC12.Text = G_Left + " / " + G_Right;
            //txt_ETC8.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Be_PV_1"].ToString()));
            //txt_ETC9.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Be_PV_2"].ToString()));

            //txt_ETC10.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Sum_PV_1"].ToString()));
            //txt_ETC11.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Sum_PV_2"].ToString()));

            //txt_ETC3.Text = ds.Tables[base_db_name].Rows[0]["GradeCnt2_1"].ToString();
            //txt_ETC4.Text = ds.Tables[base_db_name].Rows[0]["GradeCnt2_2"].ToString();

            //txt_ETC5.Text = ds.Tables[base_db_name].Rows[0]["GradeCnt4_1"].ToString();
            //txt_ETC6.Text = ds.Tables[base_db_name].Rows[0]["GradeCnt4_2"].ToString();

            //if (int.Parse(ds.Tables[base_db_name].Rows[0]["ReqTF2"].ToString()) >= 1)
            //    txt_ETC7.Text = "유";
        }



       






    }
}
