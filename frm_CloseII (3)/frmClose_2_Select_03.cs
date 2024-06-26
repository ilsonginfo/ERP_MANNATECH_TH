﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

using DXVGrid = DevExpress.XtraGrid.Views.Grid;
using DViewInfo = DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DXEditor = DevExpress.XtraEditors;
using DXGrid = DevExpress.XtraGrid;

namespace MLM_Program
{
    public partial class frmClose_2_Select_03 : Form
    {
      


        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        private const string base_db_name = "tbl_DB";
        private const string base_Closedb_name = "tbl_CloseTotal_02";
        Class.DevGridControlService cgb = new Class.DevGridControlService();
        cls_Grid_Base cgb_Sum = new cls_Grid_Base();

        private string idx_Mbid = "";
        private string idx_ToEndDate = "";

        private int Data_Set_Form_TF = 0;

        private int Form_Load_TF = 0;

        public frmClose_2_Select_03()
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

            cfm.button_flat_change(butt_G10_1);
            cfm.button_flat_change(butt_G10_2);

            cfm.button_flat_change(butt_G20_1);
            cfm.button_flat_change(butt_G20_2);

            cfm.button_flat_change(butt_G30_1);
            cfm.button_flat_change(butt_G30_2);


            cfm.button_flat_change(butt_G40_1);
            cfm.button_flat_change(butt_G40_2);

            cfm.button_flat_change(butt_G50_1);
            cfm.button_flat_change(butt_G50_2);

            cfm.button_flat_change(butt_G60_1);
            cfm.button_flat_change(butt_G60_2);

            cfm.button_flat_change(butt_Save);

            cfm.button_flat_change(butt_G70_N);
            cfm.button_flat_change(butt_G80_N);
            cfm.button_flat_change(butt_G90_N);
            cfm.button_flat_change(butt_G100_N);
            cfm.button_flat_change(butt_G110_N);
            cfm.button_flat_change(butt_G120_N);

            cfm.button_flat_change(butt_G70_N2);
            cfm.button_flat_change(butt_G80_N2);
            cfm.button_flat_change(butt_G90_N2);
            cfm.button_flat_change(butt_G100_N2);
            cfm.button_flat_change(butt_G110_N2);
            cfm.button_flat_change(butt_G120_N2);


            cfm.button_flat_change(butt_Excel_Detail_2);
            cfm.button_flat_change(butt_Excel_Detail_3);
            cfm.button_flat_change(butt_Excel_Detail_4);
            cfm.button_flat_change(butt_Excel_Detail_Down_N);
            cfm.button_flat_change(butt_Excel_Detail_Down_S);

            cfm.button_flat_change(butt_Excel_Pay_1);
            cfm.button_flat_change(butt_Excel_Pay_2);
            cfm.button_flat_change(butt_Excel_Pay_3);
            cfm.button_flat_change(butt_Excel_Pay_4);
            cfm.button_flat_change(butt_Excel_Pay_5);
            cfm.button_flat_change(butt_Excel_Pay_8);

            cfm.button_flat_change(butt_Excel_Pay_SP);













        }

        private void frmBase_From_Load(object sender, EventArgs e)
        {            
            Form_Load_TF = 0;
            Data_Set_Form_TF = 0;
            //dGridView_Base.Dock = DockStyle.Fill;
            panel8.Dock = DockStyle.Fill;


            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_Close_Grade_ComboBox(combo_Grade, combo_Grade_Code);
            cpbf.Put_Close_Grade_ComboBox(combo_Grade2, combo_Grade2_Code ,1 );


            Put_Rec_Code_ComboBox(combo_W_1, combo_W_Code_1);
            Put_Rec_Code_ComboBox(combo_W_2, combo_W_Code_2);

            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
            {
                butt_Excel_Detail_Down_Sd.TabPages.Remove(tab_save);                
            }

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                butt_Excel_Detail_Down_Sd.TabPages.Remove(tab_nom);               
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

            txt_ETC6_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC7_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC8_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC9_2.BackColor = cls_app_static_var.txt_Enable_Color;

            txt_ETC10_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC11_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC_N_7.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC_N_8.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC_N_9.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC_N_10.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC_N_11.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC_N_12.BackColor = cls_app_static_var.txt_Enable_Color;

            txt_ETC_N_7_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC_N_8_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC_N_9_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC_N_10_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC_N_11_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC_N_12_2.BackColor = cls_app_static_var.txt_Enable_Color;

            txt_ETC_S_D_1.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC_S_D_2.BackColor = cls_app_static_var.txt_Enable_Color;


            
            //tab_Detail_01.TabPages.Remove(tabPage5);            
            tab_Detail_01.TabPages.Remove(tabPage7);
            //tab_Detail_01.TabPages.Remove(tabPage8);
            tab_Detail_01.TabPages.Remove(tabPage14);

            //tab_Detail_02.TabPages.Remove(tab_Down_G);
            //tab_Detail_02.TabPages.Remove(tab_Down_S) ;
            butt_Excel_Detail_Down_Sd.TabPages.Remove(tab_Down_G);
            butt_Excel_Detail_Down_Sd.TabPages.Remove(tab_etc);
            //tab_Detail_02.TabPages.Remove(tab_Down_N);
            

            butt_Excel_Detail_Down_Sd.Width = (this.Width / 3) * 2;

            

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            radioB_Mi_No.Checked = true;
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

            //combo_W_Code_1.SelectedIndex = combo_W_1.SelectedIndex;
            //combo_W_Code_2.SelectedIndex = combo_W_2.SelectedIndex;

            //if (combo_W_Code_1.Text != "" && combo_W_Code_2.Text != "")
            //{
            //    if (int.Parse(combo_W_Code_1.Text) > int.Parse(combo_W_Code_2.Text))
            //    {
            //        MessageBox.Show("조회 종료주가 조회 시작주보다 빠르게 설정되어 있습니다."
            //               + "\n" +
            //              cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //        combo_W_1.Focus();
            //        return;
            //    }
            //}


            if (Check_TextBox_Error() == false) return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            combo_Grade_Code.SelectedIndex = combo_Grade.SelectedIndex;
            combo_Grade2_Code.SelectedIndex = combo_Grade2.SelectedIndex;

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
                radioB_Mi_No.Checked = true;

                idx_Mbid = "";
                idx_ToEndDate = "";

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtMbid);
            }

            else if (bt.Name == "butt_Excel")
            {
                saveFileDialog1.FileName = this.Text + "_" + DateTime.Now.ToShortDateString();
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    dGridView_Base.ExportToXlsx(saveFileDialog1.FileName);

                    cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                    string Tsql = "";
                    Tsql = "Insert Into tbl_Excel_User Values ( ";
                    Tsql = Tsql + "'" + cls_User.gid + "',Convert(Varchar(25),GetDate(),21),";
                    Tsql = Tsql + "'" + saveFileDialog1.FileName + "',";
                    Tsql = Tsql + "'') ";

                    if (Temp_Connect.Insert_Data(Tsql, "tbl_Excel_User") == false) return;

                    if(MessageBox.Show("열어보시겠습니까?", "저장이 완료되었습니다.",MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveFileDialog1.FileName);
                    }

                }


            }

            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }
        }



        private void Put_Rec_Code_ComboBox(ComboBox cb_1, ComboBox cb_1_Code, int Ga_FLAG = 0)
        {

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            //Tsql = "Select Ncode ,Name  ";
            //Tsql = Tsql + " From tbl_Base_Rec  (nolock)  ";
            //Tsql = Tsql + " Order by Ncode ASC ";


            //int Max_WeekSeq = 0;

            //Tsql = "Select WeekSeq, StartDate, EndDate , Convert(varchar,GetDate(),112)  NN_Date ";
            //Tsql = Tsql + " From tbl_WeekCount (nolock) ";
            //Tsql = Tsql + " Where ENDDATE = (Select Max(ToEndDate) From tbl_CloseTotal_02 (nolock) ) ";

            //DataSet ds_S = new DataSet();
            ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //Temp_Connect.Open_Data_Set(Tsql, "tbl_Base_Rec", ds_S);
            //int ReCnt = Temp_Connect.DataSet_ReCount;

            //string EndDate = "", NN_Date = "";
            //if (ReCnt <= 0)
            //{
            //    Max_WeekSeq = 1;
            //    EndDate = "20170911";
            //    NN_Date = cls_User.gid_date_time;
            //}
            //else
            //{
            //    Max_WeekSeq = int.Parse(ds_S.Tables["tbl_Base_Rec"].Rows[0]["WeekSeq"].ToString()) + 1;
            //    EndDate = ds_S.Tables["tbl_Base_Rec"].Rows[0]["EndDate"].ToString();
            //    NN_Date = ds_S.Tables["tbl_Base_Rec"].Rows[0]["NN_Date"].ToString();
            //}

            //if (int.Parse(EndDate) < int.Parse(NN_Date)) Max_WeekSeq++;




            string NN_Date = cls_User.gid_date_time;
            int ReCnt = 0;

            Tsql = "Select WeekSeq, StartDate, EndDate ";
            Tsql = Tsql + " From tbl_WeekCount (nolock) ";
            //Tsql = Tsql + " Where WeekSeq <= " + Max_WeekSeq;
            Tsql = Tsql + " Where StartDate <= " + NN_Date;
            Tsql = Tsql + " Order by  WeekSeq DESC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_Base_Rec", ds);
            ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("전체");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                string Close_DATE = "";
                Close_DATE = ds.Tables["tbl_Base_Rec"].Rows[fi_cnt]["WeekSeq"].ToString() + "주";
                Close_DATE += " (" + ds.Tables["tbl_Base_Rec"].Rows[fi_cnt]["StartDate"].ToString();
                Close_DATE += " ~ " + ds.Tables["tbl_Base_Rec"].Rows[fi_cnt]["EndDate"].ToString() + ")";

                cb_1.Items.Add(Close_DATE);
                cb_1_Code.Items.Add(ds.Tables["tbl_Base_Rec"].Rows[fi_cnt]["WeekSeq"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
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
            Tsql = "Select Case When tbl_Close_Not_Pay.Seq is not null And tbl_Close_Not_Pay.Check_FLAG = 'N' then 'V' ELSE  '' End ,  ";
            
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " tbl_ClosePay_02_Mod.mbid + '-' + Convert(Varchar,tbl_ClosePay_02_Mod.mbid2) ";
            else
                Tsql = Tsql + " tbl_ClosePay_02_Mod.mbid2 ";

            Tsql = Tsql + " ,tbl_ClosePay_02_Mod.M_Name ";
            
            Tsql = Tsql + " ,LEFT(tbl_ClosePay_02_Mod.FromEndDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_02_Mod.FromEndDate,4),2) + '-' + RIGHT(tbl_ClosePay_02_Mod.FromEndDate,2) ";
            
            Tsql = Tsql + " , LEFT(tbl_ClosePay_02_Mod.ToEndDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_02_Mod.ToEndDate,4),2) + '-' + RIGHT(tbl_ClosePay_02_Mod.ToEndDate,2) ";
            Tsql = Tsql + " , LEFT(tbl_ClosePay_02_Mod.PayDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_02_Mod.PayDate,4),2) + '-' + RIGHT(tbl_ClosePay_02_Mod.PayDate,2) ";
                        
            Tsql = Tsql + ", tbl_ClosePay_02_Mod.Ga_FLAG  ";

            Tsql = Tsql + ", Case When tbl_ClosePay_02_Mod.TruePayment > 0 then '지급' " ;
            Tsql = Tsql + " When tbl_ClosePay_02_Mod.SumAllAllowance = 0  then '미발생자' ";
            Tsql = Tsql + " When tbl_ClosePay_02_Mod.SumAllAllowance > 0 And tbl_ClosePay_02_Mod.TruePayment = 0  then '미지급' ";
            Tsql = Tsql + " ELSE '' End  ";
                      

            Tsql = Tsql + " ,  Self_Total_PV  ";
            Tsql = Tsql + " ,  SellCV01 + SellCV02 + SellCV03  ";
            Tsql = Tsql + " , SellPrice01 + SellPrice02 + SellPrice03  ";


            Tsql = Tsql + " , ISnull(C_M.Grade_Name,'')   ";

            Tsql = Tsql + " , ISnull(C2.Grade_Name,'')   ";
            Tsql = Tsql + " , ISnull(C2_4.Grade_Name,'') ";
            Tsql = Tsql + " , N_Dir_Active_Cnt "; // ISnull(C2.Grade_Name,'') ";
            Tsql = Tsql + " , N_Dir_Active_Cnt_M2  "; // ISnull(C4.Grade_Name,'')  ";

            //W4_QV  W4_QV_Auto  W4_QV_Down
            //ReqTF10 = 1 개별구매, ReqTF10 =2  오토쉽구매 ,  ReqTF10 = 3 직추천소비자 구매    

            Tsql = Tsql + " ,Be_PV_1,Be_PV_2 ";
            Tsql = Tsql + " ,Cur_PV_1,Cur_PV_2 ";
            Tsql = Tsql + " ,Cacu_Sum_PV_1,Cacu_Sum_PV_2 ";
            Tsql = Tsql + " ,Ded_1,Ded_2 ";
            Tsql = Tsql + " ,Case When Allowance1_T_Per <> '' then Convert(int,Convert(float, Allowance1_T_Per)) ELSE 0 End  ";
            Tsql = Tsql + " ,Fresh_1,Fresh_2 ";
            Tsql = Tsql + " ,Sum_PV_1,Sum_PV_2 ";

            Tsql = Tsql + " ,Re_Cur_PV_1";
 

            Tsql = Tsql + " ,Re_Cur_PV_2";   
            Tsql = Tsql + " ,Real_Sum_PV_1";   //반품처리전 실 하선PV
            Tsql = Tsql + " ,Real_Sum_PV_2";   //반품처리전 실 하선PV

            //Tsql = Tsql + " ,Down_PV_1";   //반품처리전 수당 적용PV
            Tsql = Tsql + " ,Case When tbl_ClosePay_02_Mod.FromEndDate >= '20191001' then Cur_Down_PV_1 + Be_Down_PV_1  ELSE Down_PV_1 END";

            Tsql = Tsql + " ,Down_PV_Re_1";   //반품처리전 수당 적용PV

            //Tsql = Tsql + " ,Down_PV_2";   //반품처리전 수당 적용PV
            Tsql = Tsql + " ,Case When tbl_ClosePay_02_Mod.FromEndDate >= '20191001' then Cur_Down_PV_2 + Be_Down_PV_2  ELSE Down_PV_2 END";
            Tsql = Tsql + " ,Down_PV_Re_2";   //반품처리전 수당 적용PV

            //Tsql = Tsql + " ,Down_PV_Re_1,Down_PV_Re_2 ";
            //Tsql = Tsql + " ,Down_PV_Re_M2_1,Down_PV_Re_M2_2 ";
            //Tsql = Tsql + " ,N_Down_PV_Re,N_Down_PV_Re_M2 ";


            //Tsql = Tsql + " , Down_PV_M2_1 ";
            Tsql = Tsql + " ,Case When tbl_ClosePay_02_Mod.FromEndDate >= '20191001' then Cur_Down_PV_M2_1 + Be_Down_PV_M2_1  ELSE Down_PV_M2_1 END";
            Tsql = Tsql + " , Down_PV_Re_M2_1 ";
            //Tsql = Tsql + " , Down_PV_M2_2 ";
            Tsql = Tsql + " ,Case When tbl_ClosePay_02_Mod.FromEndDate >= '20191001' then Cur_Down_PV_M2_2 + Be_Down_PV_M2_2  ELSE Down_PV_M2_2 END";
            Tsql = Tsql + " , Down_PV_Re_M2_2 ";

            Tsql = Tsql + ",  N_Down_PV ";
            Tsql = Tsql + ",  N_Down_PV_Re ";
            Tsql = Tsql + " , N_Down_PV_M2 ";
            Tsql = Tsql + ",  N_Down_PV_Re_M2 ";
            Tsql = Tsql + " , Case When ReqTF2 =  1 then 'Y' ELSE '' End";
            Tsql = Tsql + " , Case When   Grade_ReqTF2 =  1 OR LEFT(tbl_ClosePay_02_Mod.RegTime,6)   = LEFT(tbl_ClosePay_02_Mod.FromEndDate ,6)  then 'Y' ELSE '' End";
            Tsql = Tsql + " , Case When   Grade_ReqTF2_M2 =  1 OR LEFT(tbl_ClosePay_02_Mod.RegTime,6)   = LEFT(tbl_ClosePay_02_Mod.ToEndDate ,6)  then 'Y' ELSE '' End";


            Tsql = Tsql + " , Dir_Cnt_G10 ";
            Tsql = Tsql + " , Dir_Cnt_G20 ";
            Tsql = Tsql + " , Dir_Cnt_G30 ";

            Tsql = Tsql + " , Self_Cur_Dri_BV ";



            Tsql = Tsql + " ,Etc_Pay , Allowance1 , Allowance2 , Allowance3 , Allowance1_cut   ";

            

            //Tsql = Tsql + " ,  0 ";
            Tsql = Tsql + " , 0 ";
            Tsql = Tsql + ", Case When tbl_ClosePay_02_Mod.Cpno = ''  OR tbl_ClosePay_02_Mod.BankAcc = '' then tbl_ClosePay_02_Mod.SumAllAllowance +   tbl_ClosePay_02_Mod.SumAllAllowance_Be_Not ELSE 0 End AS Not_Pay_C ";
            Tsql = Tsql + " ,tbl_ClosePay_02_Mod.SumAllAllowance_Be_Not_Sum  ";

            //Isnull(tbl_ClosePay_02_Mod.Cur_DedCut_Pay_DED,0)
            Tsql = Tsql + ", Isnull( Allowance1_D,0) +Isnull(  Allowance2_D,0)  +Isnull(  Allowance3_D,0)  SumAllAllowance_Cut ";
            Tsql = Tsql + ", Isnull(Cur_DedCut_Pay_DED,0) Etc_Pay_DedCut ";
            Tsql = Tsql + ", Isnull(Cur_DedCut_Pay,0) Cur_DedCut_Pay ";
            Tsql = Tsql + " , (Allowance1 + Allowance2 + Allowance3   + tbl_ClosePay_02_Mod.Etc_Pay  ) Cur_SumAllowance  ";
            Tsql = Tsql + " , (Allowance1 + Allowance2 + Allowance3  - Cur_DedCut_Pay  + tbl_ClosePay_02_Mod.Etc_Pay  -Isnull(tbl_ClosePay_02_Mod.Cur_DedCut_Pay_DED,0))     Cur_D_SumAllowance  ";
            Tsql = Tsql + " , tbl_ClosePay_02_Mod.SumAllAllowance_Be_Not_Sum   Be_SumAllowance  ";
            Tsql = Tsql + " ,  (Allowance1 + Allowance2 + Allowance3  - Cur_DedCut_Pay) + tbl_ClosePay_02_Mod.SumAllAllowance_Be_Not_Sum  + tbl_ClosePay_02_Mod.Etc_Pay  - Isnull(tbl_ClosePay_02_Mod.Cur_DedCut_Pay_DED,0)    SumAllAllowance";
            //Tsql = Tsql + " ,  (Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6 + Allowance7 + Allowance8 + Allowance9 - Allowance12 + Allowance13 - Cur_DedCut_Pay)     SumAllAllowance";
            //Tsql = Tsql + " , tbl_ClosePay_02_Mod.SumAllAllowance  ";
            Tsql = Tsql + ", InComeTax , ResidentTax , TruePayment ";


            
            Tsql = Tsql + " ,Sum_Return_Remain_Pay - Cur_Return_Pay   ";
            Tsql = Tsql + " ,0 ";

            Tsql = Tsql + " ,Cur_Return_Pay   ";
            Tsql = Tsql + " ,Sum_Return_Remain_Pay - Cur_DedCut_Pay ";
            //Tsql = Tsql + " , Max_N_LineCnt ";
            //Tsql = Tsql + " , ISNULL(tbl_ClosePay_10000.SumAllAllowance, 0) ";
            Tsql = Tsql + " ,  tbl_Memberinfo.hptel ,  tbl_Memberinfo.LeaveDate , tbl_Memberinfo.Addcode1 , tbl_Memberinfo.Address1 + ' ' + tbl_Memberinfo.Address2 ";

            Tsql = Tsql + " ,tbl_Bank.bankname , tbl_ClosePay_02_Mod.bankcode " ;

            Tsql = Tsql + ", tbl_ClosePay_02_Mod.BankAcc ";
            Tsql = Tsql + ", tbl_ClosePay_02_Mod.BankOwner ";
            Tsql = Tsql + ", tbl_ClosePay_02_Mod.Cpno ";

            Tsql = Tsql + ", isnull(tbl_Business.Name,'') AS bname , tbl_ClosePay_02_Mod.Remarks1 ";

            Tsql = Tsql + " ,Be_Down_PV_1 , Be_Down_PV_2 ";
            Tsql = Tsql + " ,Cur_Down_PV_1 ";
            Tsql = Tsql + " ,Cur_Down_PV_2";
            Tsql = Tsql + " ,Down_PV_1";   
            Tsql = Tsql + " ,Down_PV_2";   
            Tsql = Tsql + " ,N_3081_Cnt ";


            Tsql = Tsql + " ,Be_Down_PV_M2_1 , Be_Down_PV_M2_2 ";
            Tsql = Tsql + " ,Cur_Down_PV_M2_1";
            Tsql = Tsql + " ,Cur_Down_PV_M2_2";
            Tsql = Tsql + " ,Down_PV_M2_1";
            Tsql = Tsql + " ,Down_PV_M2_2";
            Tsql = Tsql + " ,N_3081_Cnt_M2 ";



            //Tsql = Tsql + ", Case ";
            //Tsql = Tsql + "  When tbl_Close_Not_Pay.Check_FLAG = 'N' then '회원별화면'";
            //Tsql = Tsql + "  When tbl_Close_Not_Pay.Check_FLAG = 'M' then '직접입력'";
            //Tsql = Tsql + "  ELSE ''";
            //Tsql = Tsql + "  End ";


            //Tsql = Tsql + " From tbl_ClosePay_02_Mod (nolock) ";
            ///////////////////////////////////@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            Tsql = Tsql + " From  ( ";
            Tsql = Tsql + " Select '' Ga_FLAG,  Mbid,Mbid2, M_Name,ToEndDate, FromEndDate, PayDate ,LeaveDate   ";
            Tsql = Tsql + " ,  Self_Total_PV , Self_Cur_Dri_BV  ";
            Tsql = Tsql + " , SellCV01 , SellCV02 , SellCV03  ";
            Tsql = Tsql + " , SellPrice01 , SellPrice02 , SellPrice03  ";
            Tsql = Tsql + " , CurGrade, CurGrade_M2, CurGrade_Be_M   ";            
            Tsql = Tsql + " , N_Dir_Active_Cnt , N_Dir_Active_Cnt_M2  "; 
                        
            Tsql = Tsql + " ,Be_PV_1,Be_PV_2 ";
            Tsql = Tsql + " ,Cur_PV_1,Cur_PV_2 ";
            Tsql = Tsql + " ,Real_Sum_PV_1,Real_Sum_PV_2 ";
            Tsql = Tsql + " ,Ded_1,Ded_2 ";
            Tsql = Tsql + " ,Allowance1_T_Per  ";
            Tsql = Tsql + " ,Fresh_1,Fresh_2 ";
            Tsql = Tsql + " ,Sum_PV_1,Sum_PV_2 ";

            Tsql = Tsql + " ,Re_Cur_PV_1,Re_Cur_PV_2";
            Tsql = Tsql + " ,Cacu_Sum_PV_1,Cacu_Sum_PV_2";
            Tsql = Tsql + " ,Down_PV_1 ,Down_PV_2";
            Tsql = Tsql + " ,Be_Down_PV_1 ,Be_Down_PV_2";
            Tsql = Tsql + " ,Cur_Down_PV_1 ,Cur_Down_PV_2";
            Tsql = Tsql + " ,N_3081_Cnt , N_3081_Cnt_M2 ";

            Tsql = Tsql + " ,Down_PV_Re_1,Down_PV_Re_2 ";
            Tsql = Tsql + " ,Down_PV_Re_M2_1,Down_PV_Re_M2_2 ";
            Tsql = Tsql + " ,N_Down_PV_Re,N_Down_PV_Re_M2 ";


            Tsql = Tsql + " , Down_PV_M2_1 , Down_PV_M2_2 ";
            Tsql = Tsql + " , Be_Down_PV_M2_1 , Be_Down_PV_M2_2 ";
            Tsql = Tsql + " , Cur_Down_PV_M2_1 , Cur_Down_PV_M2_2 ";

            Tsql = Tsql + ",  N_Down_PV , N_Down_PV_M2 ";
            Tsql = Tsql + " , ReqTF2 , Grade_ReqTF2 , Grade_ReqTF2_M2  ";            

            Tsql = Tsql + " , Dir_Cnt_G10 , Dir_Cnt_G20  , Dir_Cnt_G30 ";
            Tsql = Tsql + " , Regtime ";

            Tsql = Tsql + " ,Etc_Pay , Etc_Pay_DedCut , Allowance1 , Allowance2 , Allowance3 , Allowance1_cut       ";
            Tsql = Tsql + ", Allowance1_D ,Allowance2_D , Allowance3_D ";
            Tsql = Tsql + " ,SumAllAllowance_Be_Not , SumAllAllowance_Be_Not_Sum  ";            
            Tsql = Tsql + ", SumAllAllowance ,  InComeTax , ResidentTax , TruePayment ";
            Tsql = Tsql + " ,Sum_Return_Remain_Pay_DED   ,Cur_DedCut_Pay_DED ,  Sum_Return_Take_Pay_DED  ";
            Tsql = Tsql + " ,Sum_Return_Remain_Pay   ,Cur_DedCut_Pay , Cur_Return_Pay , Sum_Return_Take_Pay  ";          
            Tsql = Tsql + " ,bankcode , BankAcc , BankOwner , Cpno , Remarks1, BusCode  ";

            Tsql = Tsql + " From tbl_ClosePay_02_Mod   (nolock) ";

            Tsql = Tsql + " Union All ";  ///////////////////////////////////@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

            Tsql = Tsql + " Select 'Y' Ga_FLAG , Mbid,Mbid2, M_Name,ToEndDate, FromEndDate, PayDate , LeaveDate   ";
            Tsql = Tsql + " ,  Self_Total_PV , Self_Cur_Dri_BV  ";
            Tsql = Tsql + " , SellCV01 , SellCV02 , SellCV03  ";
            Tsql = Tsql + " , SellPrice01 , SellPrice02 , SellPrice03  ";
            Tsql = Tsql + " , CurGrade, CurGrade_M2 , CurGrade_Be_M  ";
            Tsql = Tsql + " , N_Dir_Active_Cnt , N_Dir_Active_Cnt_M2  ";

            Tsql = Tsql + " ,Be_PV_1,Be_PV_2 ";
            Tsql = Tsql + " ,Cur_PV_1,Cur_PV_2 ";
            Tsql = Tsql + " ,Real_Sum_PV_1,Real_Sum_PV_2 ";
            Tsql = Tsql + " ,Ded_1,Ded_2 ";
            Tsql = Tsql + " ,Allowance1_T_Per  ";
            Tsql = Tsql + " ,Fresh_1,Fresh_2 ";
            Tsql = Tsql + " ,Sum_PV_1,Sum_PV_2 ";

            Tsql = Tsql + " ,Re_Cur_PV_1,Re_Cur_PV_2";
            Tsql = Tsql + " ,Cacu_Sum_PV_1,Cacu_Sum_PV_2";
            Tsql = Tsql + " ,Down_PV_1 ,Down_PV_2";
            Tsql = Tsql + " ,Be_Down_PV_1 ,Be_Down_PV_2";
            Tsql = Tsql + " ,Cur_Down_PV_1 ,Cur_Down_PV_2";
            Tsql = Tsql + " ,N_3081_Cnt , N_3081_Cnt_M2 ";

            Tsql = Tsql + " ,Down_PV_Re_1,Down_PV_Re_2 ";
            Tsql = Tsql + " ,Down_PV_Re_M2_1,Down_PV_Re_M2_2 ";
            Tsql = Tsql + " ,N_Down_PV_Re,N_Down_PV_Re_M2 ";

            Tsql = Tsql + " , Down_PV_M2_1 , Down_PV_M2_2 ";
            Tsql = Tsql + " , Be_Down_PV_M2_1 , Be_Down_PV_M2_2 ";
            Tsql = Tsql + " , Cur_Down_PV_M2_1 , Cur_Down_PV_M2_2 ";
            
            Tsql = Tsql + ",  N_Down_PV , N_Down_PV_M2 ";
            Tsql = Tsql + " , ReqTF2 , Grade_ReqTF2 , Grade_ReqTF2_M2  ";

            Tsql = Tsql + " , Dir_Cnt_G10 , Dir_Cnt_G20  , Dir_Cnt_G30 ";
            Tsql = Tsql + " , Regtime ";

            Tsql = Tsql + " ,Etc_Pay , Etc_Pay_DedCut , Allowance1 , Allowance2 , Allowance3 , Allowance1_cut       ";
            Tsql = Tsql + ", Allowance1_D ,Allowance2_D , Allowance3_D ";
            Tsql = Tsql + " ,SumAllAllowance_Be_Not , SumAllAllowance_Be_Not_Sum  ";
            Tsql = Tsql + ", SumAllAllowance ,  InComeTax , ResidentTax , TruePayment ";
            Tsql = Tsql + " ,Sum_Return_Remain_Pay_DED   ,Cur_DedCut_Pay_DED ,  Sum_Return_Take_Pay_DED  ";
            Tsql = Tsql + " ,Sum_Return_Remain_Pay   ,Cur_DedCut_Pay , Cur_Return_Pay , Sum_Return_Take_Pay  ";
            Tsql = Tsql + " ,bankcode , BankAcc , BankOwner , Cpno , Remarks1, BusCode  ";
            Tsql = Tsql + "  From CKDPHARM_Ga_Close.dbo.tbl_ClosePay_02_Mod   (nolock) ";
            Tsql = Tsql + " Where ToEndDate Not in (Select ToEndDate  From tbl_CloseTotal_02 (nolock) Where Real_FLAG = 0  ) ";

            Tsql = Tsql + "       ) AS tbl_ClosePay_02_Mod  ";
            ///////////////////////////////////@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            ///
            //Tsql = Tsql + " LEFT Join tbl_ClosePay_02_Sell_Mod  (nolock) On tbl_ClosePay_02_Mod.mbid=tbl_ClosePay_02_Sell_Mod.mbid " ;
            //Tsql = Tsql + " And tbl_ClosePay_02_Mod.mbid2=tbl_ClosePay_02_Sell_Mod.mbid2";
            //Tsql = Tsql + " And tbl_ClosePay_02_Mod.ToEndDate=tbl_ClosePay_02_Sell_Mod.ToEndDate";
            //Tsql = Tsql + " And tbl_ClosePay_02_Sell_Mod.SellCode ='01' ";

            Tsql = Tsql + " LEFT Join tbl_Memberinfo  (nolock) On tbl_ClosePay_02_Mod.mbid=tbl_Memberinfo.mbid ";
            Tsql = Tsql + " And tbl_ClosePay_02_Mod.mbid2=tbl_Memberinfo.mbid2";

            Tsql = Tsql + " Left Join tbl_Business  (nolock) On tbl_Memberinfo.businesscode=tbl_Business.ncode And tbl_Memberinfo.Na_code = tbl_Business.Na_code";
            Tsql = Tsql + " Left Join tbl_Bank  (nolock) On tbl_ClosePay_02_Mod.bankcode=tbl_Bank.ncode  ";
            Tsql = Tsql + " Left Join tbl_Class C2  (nolock) On tbl_ClosePay_02_Mod.CurGrade = C2.Grade_Cnt ";
            Tsql = Tsql + " Left Join tbl_Class C2_4  (nolock) On tbl_ClosePay_02_Mod.CurGrade_M2 =C2_4.Grade_Cnt ";
            Tsql = Tsql + " Left Join tbl_Class C_M  (nolock) On tbl_Memberinfo.CurGrade = C_M.Grade_Cnt ";            
            //Tsql = Tsql + " Left Join tbl_Class C4  (nolock) On tbl_ClosePay_02_Mod.OneGrade = C4.Grade_Cnt ";
            Tsql = Tsql + " Left Join tbl_ClosePay_10000 (nolock) on tbl_ClosePay_02_Mod.mbid = tbl_ClosePay_10000.mbid And tbl_ClosePay_02_Mod.mbid2 = tbl_ClosePay_10000.mbid2 and tbl_ClosePay_02_Mod.ToEndDate = tbl_ClosePay_10000.ToEndDate And tbl_ClosePay_10000.ToEndDate_TF = 2 ";
            Tsql = Tsql + " Left Join tbl_Close_Not_Pay (nolock ) on tbl_ClosePay_02_Mod.mbid = tbl_Close_Not_Pay.mbid And tbl_ClosePay_02_Mod.mbid2 = tbl_Close_Not_Pay.mbid2 and tbl_ClosePay_02_Mod.ToEndDate = tbl_Close_Not_Pay.ToEndDate And tbl_Close_Not_Pay.Close_FLAG = 'W'  ";

           // Tsql = Tsql + " LEFT JOIN tbl_WeekCount (nolock) ON tbl_WeekCount.ENDDATE = tbl_ClosePay_02_Mod.ToEndDate  ";       

            ////2016-07-25 작업. 구매등록과 주간마감의 금액이 맞지 않아서 작업함.
            //Tsql = Tsql + " Left Join ( ";
            //Tsql = Tsql + " Select mbid, mbid2, SellDate_2, SUM(TotalPrice) TotalPrice, SUM(TotalPV) TotalPV, SUM(TotalCV) TotalCV  ";
            //Tsql = Tsql + " 	From tbl_SalesDetail (nolock) Where Ga_Order = 0  ";
            //Tsql = Tsql + " 	Group By mbid, mbid2, SellDate_2 ";
            //Tsql = Tsql + " ) Sales ON tbl_ClosePay_02_Mod.mbid = Sales.mbid And tbl_ClosePay_02_Mod.mbid2 = Sales.mbid2 ";
            //Tsql = Tsql + " And Sales.SellDate_2 <= tbl_ClosePay_02_Mod.ToEndDate ";

        }



        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = " Where tbl_ClosePay_02_Mod.ToEndDate <> ''  ";

            string Mbid = ""; int Mbid2 = 0;


            //if (combo_W_Code_1.Text != "" && combo_W_Code_2.Text == "")
            //{
            //    strSql = strSql + " And  tbl_WeekCount.WEEKSEQ = " + combo_W_Code_1.Text;
            //}

            //if (combo_W_Code_1.Text != "" && combo_W_Code_2.Text != "")
            //{
            //    strSql = strSql + " And  tbl_WeekCount.WEEKSEQ >= " + combo_W_Code_1.Text;
            //    strSql = strSql + " And  tbl_WeekCount.WEEKSEQ <= " + combo_W_Code_2.Text;
            //}



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
                        strSql = strSql + " And tbl_ClosePay_02_Mod.Mbid ='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_ClosePay_02_Mod.Mbid2 = " + Mbid2;
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
                        strSql = strSql + " And tbl_ClosePay_02_Mod.Mbid >='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_ClosePay_02_Mod.Mbid2 >= " + Mbid2;
                }

                if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And tbl_ClosePay_02_Mod.Mbid <='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_ClosePay_02_Mod.Mbid2 <= " + Mbid2;
                }
            }


            //회원명으로 검색
            if (txtName.Text.Trim() != "")
                strSql = strSql + " And tbl_ClosePay_02_Mod.M_Name Like '%" + txtName.Text.Trim() + "%'";


            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";



            if (txt_Us_num.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.Us_Num = '" + txt_Us_num.Text.Trim() + "'";
            



           if ((mtxtFromDate1.Text.Replace("-", "").Trim() != "") && (mtxtFromDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_ClosePay_02_Mod.FromEndDAte = '" + mtxtFromDate1.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtFromDate1.Text.Replace("-", "").Trim() != "") && (mtxtFromDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_ClosePay_02_Mod.FromEndDAte >= '" + mtxtFromDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_ClosePay_02_Mod.FromEndDate <= '" + mtxtFromDate2.Text.Replace("-", "").Trim() + "'";
            }


            //기록일자로 검색 -1
            if ((mtxtToDate1.Text.Replace("-", "").Trim() != "") && (mtxtToDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_ClosePay_02_Mod.ToEndDate = '" + mtxtToDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtToDate1.Text.Replace("-", "").Trim() != "") && (mtxtToDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_ClosePay_02_Mod.ToEndDate >= '" + mtxtToDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_ClosePay_02_Mod.ToEndDate <= '" + mtxtToDate2.Text.Replace("-", "").Trim() + "'";
            }


            //기록일자로 검색 -1
            if ((mtxtPayDate1.Text.Replace("-", "").Trim() != "") && (mtxtPayDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_ClosePay_02_Mod.PayDate = '" + mtxtPayDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtPayDate1.Text.Replace("-", "").Trim() != "") && (mtxtPayDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_ClosePay_02_Mod.PayDate >= '" + mtxtPayDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_ClosePay_02_Mod.PayDate <= '" + mtxtPayDate2.Text.Replace("-", "").Trim() + "'";
            }

            if (txtToEndDate_Code.Text != "")
                strSql = strSql + " And tbl_ClosePay_02_Mod.ToEndDate = '" + txtToEndDate.Text + "'";

            if (combo_Grade_Code.Text != "")
                strSql = strSql + " And tbl_ClosePay_02_Mod.CurGrade = " + combo_Grade_Code.Text  ;


            if (radio_PayTF1.Checked == true)
                strSql = strSql + " And tbl_ClosePay_02_Mod.TruePayment > 0  ";

            if (radio_PayTF3.Checked == true)
                strSql = strSql + " And tbl_ClosePay_02_Mod.SumAllAllowance = 0  ";

            if (radio_PayTF_Not.Checked == true)
                strSql = strSql + " And tbl_ClosePay_02_Mod.SumAllAllowance > 0 And tbl_ClosePay_02_Mod.TruePayment = 0   ";

            if (radio_PayTF_ALL.Checked == true)
                strSql = strSql + " And (Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6  - Cur_DedCut_Pay) + SumAllAllowance_Be_Not > 0  ";


            if (radio_PayTF_Re_D_1.Checked == true)
                strSql = strSql + " And tbl_ClosePay_02_Mod.Sum_Return_Remain_Pay - Cur_DedCut_Pay > 0  ";

            if (radio_PayTF_Re_D_2.Checked == true)
                strSql = strSql + " And tbl_ClosePay_02_Mod.Sum_Return_Remain_Pay  > 0  ";




            if (radioB_Leave_Not.Checked == true)
                strSql = strSql + " And tbl_ClosePay_02_Mod.LeaveDate = '' ";

            if (radioB_Leave.Checked == true)
                strSql = strSql + " And tbl_ClosePay_02_Mod.LeaveDate <> '' ";


            if (radioB_Su.Checked == true)
                strSql = strSql + " And tbl_ClosePay_02_Mod.Cpno <> ''  ";

            if (radioB_Su_Not.Checked == true)
                strSql = strSql + " And tbl_ClosePay_02_Mod.Cpno = '' ";

            


            //if (chk_Leave_Only.Checked == true )
            //    strSql = strSql + " And tbl_ClosePay_02_Mod.LeaveDate <> '' "; //탈퇴회원만 나와라.

            if (checkB_Up.Checked == true)
            {
                strSql = strSql + " And tbl_ClosePay_02_Mod.CurGrade_Be_M <  tbl_ClosePay_02_Mod.CurGrade_M2 ";
                strSql = strSql + " And tbl_ClosePay_02_Mod.CurGrade_M2 >= 10  ";


                //if (combo_Grade2_Code.Text != "")
                //    strSql = strSql + " And tbl_ClosePay_02_Mod.CurGrade_M2 = " + combo_Grade2_Code.Text ;

                int C_TF = 0; 
                if (checkB_10.Checked == true)
                {
                    strSql = strSql + " And (tbl_ClosePay_02_Mod.CurGrade_M2 = 10 ";
                    C_TF++;
                }

                if (checkB_20.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 20 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 20 ";
                    C_TF++;
                }

                if (checkB_30.Checked == true)
                {
                    if ( C_TF >0 ) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 30 ";
                    if ( C_TF == 0 ) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 30 ";
                    C_TF++;
                }

                if (checkB_40.Checked == true)
                {
                    if ( C_TF >0 ) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 40 ";
                    if ( C_TF == 0 ) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 40 ";
                    C_TF++;
                }
               
                if (checkB_50.Checked == true)
                {
                    if ( C_TF >0 ) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 50 ";
                    if ( C_TF == 0 ) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 50 ";
                    C_TF++;
                }

                if (checkB_60.Checked == true)
                {
                    if ( C_TF >0 ) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 60 ";
                    if ( C_TF == 0 ) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 60 ";
                    C_TF++;
                }

                if (checkB_70.Checked == true)
                {
                    if ( C_TF >0 ) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 70 ";
                    if ( C_TF == 0 ) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 70 ";
                    C_TF++;
                }

                if (checkB_80.Checked == true)
                {
                    if ( C_TF >0 ) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 80 ";
                    if ( C_TF == 0 ) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 80 ";
                    C_TF++;
                }

                if (checkB_90.Checked == true)
                {
                    if ( C_TF >0 ) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 90 ";
                    if ( C_TF == 0 ) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 90 ";
                    C_TF++;
                }

                if (checkB_100.Checked == true)
                {
                    if ( C_TF >0 ) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 100 ";
                    if ( C_TF == 0 ) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 100 ";
                    C_TF++;
                }

                if (checkB_110.Checked == true)
                {
                    if ( C_TF >0 ) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 110 ";
                    if ( C_TF == 0 ) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 110 ";
                    C_TF++;
                }
                
                if (checkB_120.Checked == true)
                {
                    if ( C_TF >0 ) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 120 ";
                    if ( C_TF == 0 ) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 120 ";
                    C_TF++;
                }


                if (checkB_130.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 130 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 130 ";
                    C_TF++;
                }

                if (checkB_140.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 140 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 140 ";
                    C_TF++;
                }

                if (checkB_150.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_02_Mod.CurGrade_M2 = 150 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_02_Mod.CurGrade_M2 = 150 ";
                    C_TF++;
                }


                if (C_TF > 0)
                    strSql = strSql + " ) "; 

            }

            //if (checkB_Up_60.Checked == true)
            //{
            //    strSql = strSql + " And tbl_ClosePay_02_Mod.BeforeGrade <  tbl_ClosePay_02_Mod.OneGrade ";
            //    strSql = strSql + " And tbl_ClosePay_02_Mod.OneGrade >= 60  ";
            //}

            


            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


            strSql = strSql + " Order by tbl_ClosePay_02_Mod.ToEndDate desc , tbl_ClosePay_02_Mod.Mbid,tbl_ClosePay_02_Mod.Mbid2 "; 


            Tsql = Tsql + strSql;

            //Tsql = Tsql + " group by tbl_ClosePay_02_Mod.Mbid, tbl_ClosePay_02_Mod.mbid2  ,tbl_ClosePay_02_Mod.M_Name   ";
            //Tsql = Tsql + " ,LEFT(tbl_ClosePay_02_Mod.FromEndDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_02_Mod.FromEndDate,4),2) + '-' + RIGHT(tbl_ClosePay_02_Mod.FromEndDate,2)  ";
            //Tsql = Tsql + " , LEFT(tbl_ClosePay_02_Mod.ToEndDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_02_Mod.ToEndDate,4),2) + '-' + RIGHT(tbl_ClosePay_02_Mod.ToEndDate,2)  ";
            //Tsql = Tsql + " , LEFT(tbl_ClosePay_02_Mod.PayDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_02_Mod.PayDate,4),2) + '-' + RIGHT(tbl_ClosePay_02_Mod.PayDate,2)  ";
            //Tsql = Tsql + " ,  ISnull(C1.Grade_Name,'')  ,  ISnull(C2.Grade_Name,'') ,  ISnull(C4.Grade_Name,'') ";
            //Tsql = Tsql + " ,Etc_Pay , Allowance1 , Allowance2 , Allowance3 ";
            //Tsql = Tsql + " , Etc_Pay , Cur_DedCut_Pay   , SumAllAllowance_10000 , tbl_ClosePay_02_Mod.SumAllAllowance  , InComeTax , ResidentTax , TruePayment   ";
            //Tsql = Tsql + " , Allowance1 + Allowance1_Cut_2  , ISNULL(tbl_ClosePay_10000.SumAllAllowance, 0) , tbl_Memberinfo.hometel  ";
            //Tsql = Tsql + " ,  tbl_Memberinfo.hptel , tbl_Memberinfo.Addcode1 , tbl_Memberinfo.Address1 + ' ' + tbl_Memberinfo.Address2  ";
            //Tsql = Tsql + " ,tbl_Bank.bankname , tbl_Memberinfo.bankcode, tbl_Memberinfo.BankAccnt , tbl_Memberinfo.bankowner , tbl_Memberinfo.Cpno   ";
            //Tsql = Tsql + " , isnull(tbl_Business.Name,''), tbl_ClosePay_02_Mod.Remarks1  , tbl_ClosePay_02_Mod.ToEndDAte , tbl_Close_Not_Pay.Seq ";
            //Tsql = Tsql + " Order by tbl_ClosePay_02_Mod.ToEndDAte DESC , tbl_ClosePay_02_Mod.Mbid, tbl_ClosePay_02_Mod.Mbid2 ";             
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
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();


            //추후 실 버전에서는 열어주어야함 계좌번호 주민ㅂ너호 관련된 부분이기 때문에
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                row[80] = encrypter.Decrypt(row[80].ToString());
                row[82] = encrypter.Decrypt(row[82].ToString(), "Cpno");
            }


            if (ds.Tables[0].Rows.Count >= 1000) cgb.baseview.IndicatorWidth = 45;
            if (ds.Tables[0].Rows.Count >= 10000) cgb.baseview.IndicatorWidth = 55;
                        
            cgb.FillGrid(ds.Tables[0]);
                       

            if (ReCnt  > 0)
            {
                put_Sum_Dataview(ds, ReCnt);                
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();

        }



        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 99;
            cgb.basegrid = dGridCtrl_Base;
            cgb.baseview = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 3;
            //cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            //Tsql = Tsql + " ,Be_Down_PV_1 , Be_Down_PV_2 ";
            //Tsql = Tsql + " ,Cur_Down_PV_1 ";
            //Tsql = Tsql + " ,Cur_Down_PV_2";
            //Tsql = Tsql + " ,Down_PV_1";
            //Tsql = Tsql + " ,Down_PV_2";
            //Tsql = Tsql + " ,N_3081_Cnt ";


            //Tsql = Tsql + " ,Be_Down_PV_M2_1 , Be_Down_PV_M2_2 ";
            //Tsql = Tsql + " ,Cur_Down_PV_M2_1";
            //Tsql = Tsql + " ,CurDown_PV_M2_2";
            //Tsql = Tsql + " ,Down_PV_M2_1";
            //Tsql = Tsql + " ,Down_PV_M2_2";
            //Tsql = Tsql + " ,N_3081_Cnt_M2 ";



            //스타보너스 ,  바이너리보너스 , 추천매칭
            string[] g_HeaderText = {"_선택","회원번호", "성명", "마감_시작일","마감_종료일"
                                   ,"지급_일자"  , "가마감여부"  ,"지급구분"  , "총매출PV" , "총매출BV"

                                 , "총매출액" ,   "최고직급"  ,   "현직급(1)"  ,   "현직급(2)", "추천1대액티브수(1)"
                                , "추천1대액티브수(2)","좌_이전BV","우_이전BV"  , "좌_신규BV" , "우_신규BV"

                                , "좌_합계BV", "우_합계BV" ,"좌_공제BV"  , "우_공제BV" , "후원적용%"
                               , "좌_후레쉬BV" , "우_후레쉬BV", "좌_이월BV" , "우_이월BV" ,"좌_반품BV"

                               ,  "우_반품BV" ,"좌_반품전합계BV","우_반품전합계", "월하선PV좌(1)", "월하선PV좌(1)_반품"
                               ,"월하선PV우(1)","월하선PV우(1)_반품",  "월하선PV좌(2)" ,  "월하선PV좌(2)_반품","월하선PV우(2)"                               

                               ,"월하선PV우(2)_반품","월추천PV(1)","월추천PV(1)_반품"    , "월추천PV(2)" , "월추천PV(2)_반품"
                                , "수당액티브여부" , "직급액티브여부(1)", "직급액티브여부(2)", "직추1스타이상"   , "직추2스타이상"
                                
                                , "직추3스타이상"  , "직추기간BV" , "기타보너스" ,"후원보너스"   , "추천매칭보너스"
                              , "추천보너스"   , "후원주극차감"       , "_패키지보너스"    , "마감미지급액"   , "전_마감미지급액" 

                                  , "Cap공제" ,  "기타공제"     , "차감된_반품_공제액"  ,"발생_당주수당합" ,"차감된_반품공제_기타공제포함합"
                               , "이월수당합"  , "지급수당합" , "소득세"   , "주민세" , "실지급액"

                               , "이월된_차감할_반품공제액", ""  , "발생된_차감할_반품공제액"  , "이월한_차감할_반품공제액"   , "연락처1"            
                                 , "탈퇴일자"     , "우편번호"    , "주소"     ,"은행명" ,   "은행코드"

                                ,   "계좌번호" ,"예금주",  "주민번호","_센타" , "비고" 
                                ,"월하선_전_좌(1)","월하선_전_우(1)","월하선_신규_좌(1)","월하선_신규_우(1)","월하선_이월_좌(1)"

                                ,"월하선_이월_우(1)","130팩직추천수(1)" ,"월하선_전_좌(2)","월하선_전_우(2)","월하선_신규_좌(2)"
                                ,"월하선_신규_우(2)","월하선_이월_좌(2)","월하선_이월_우(2)","130팩직추천수(2)"
                                    };

            string[] g_Cols = {"_선택","회원번호", "성명", "마감_시작일","마감_종료일"
                                   ,"지급_일자"  , "가마감여부"  ,"지급구분"  , "총매출PV" , "총매출BV"

                                 , "총매출액" ,   "최고직급"  ,   "현직급(1)"  ,   "현직급(2)", "추천1대액티브수(1)"
                                , "추천1대액티브수(2)","좌_이전BV","우_이전BV"  , "좌_신규BV" , "우_신규BV"

                                , "좌_합계BV", "우_합계BV" ,"좌_공제BV"  , "우_공제BV" , "후원적용%"
                               , "좌_후레쉬BV" , "우_후레쉬BV", "좌_이월BV" , "우_이월BV" ,"좌_반품BV"

                               ,  "우_반품BV" ,"좌_반품전합계BV","우_반품전합계", "월하선PV좌(1)", "월하선PV좌(1)_반품"
                               ,"월하선PV우(1)","월하선PV우(1)_반품",  "월하선PV좌(2)" ,  "월하선PV좌(2)_반품","월하선PV우(2)"

                               ,"월하선PV우(2)_반품","월추천PV(1)","월추천PV(1)_반품"    , "월추천PV(2)" , "월추천PV(2)_반품"
                                , "수당액티브여부" , "직급액티브여부(1)", "직급액티브여부(2)", "직추1스타이상"   , "직추2스타이상"

                                , "직추3스타이상"  , "직추기간BV" , "기타보너스" ,"후원보너스"   , "추천매칭보너스"
                              , "추천보너스"   , "후원주극차감"       , "_패키지보너스"    , "마감미지급액"   , "전_마감미지급액"

                                  , "Cap공제" ,  "기타공제"     , "차감된_반품_공제액"  ,"발생_당주수당합" ,"차감된_반품공제_기타공제포함합"
                               , "이월수당합"  , "지급수당합" , "소득세"   , "주민세" , "실지급액"

                               , "이월된_차감할_반품공제액", ""  , "발생된_차감할_반품공제액"  , "이월한_차감할_반품공제액"   , "연락처1"
                                 , "탈퇴일자"     , "우편번호"    , "주소"     ,"은행명" ,   "은행코드"

                                ,   "계좌번호" ,"예금주",  "주민번호","_센타" , "비고"
                                ,"월하선_전_좌(1)","월하선_전_우(1)","월하선_신규_좌(1)","월하선_신규_우(1)","월하선_이월_좌(1)"

                                ,"월하선_이월_우(1)","130팩직추천수(1)" ,"월하선_전_좌(2)","월하선_전_우(2)","월하선_신규_좌(2)"
                                ,"월하선_신규_우(2)","월하선_이월_좌(2)","월하선_이월_우(2)","130팩직추천수(2)"
                                    };

            cgb.grid_col_name = g_Cols;
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 0, 100 , 100, 100, 100
                            , 100, 75,100   ,80, 80

                           , 80, 80 , 80 , 80  ,130
                          , 130  , 80 , 90, 90,100

                           , 90  , 90   , 90 , 90 , 90
                            , 90, 90     , 90, 90, 90

                            , 90, 90  , 90, 100, 120
                          , 100, 120   ,100 , 120, 100

                          , 120, 100   ,120 , 100, 120
                            , 100 , 100  , 100, 100,100
                            
                            , 80 , 100, 100,100 , 100
                             , 100    , 100, 0, 100 , 110

                            ,80 , 80, 130, 110, 185 
                            , 80   , 90 , 80, 80,80 

                            , 175, 0 ,175, 175, 130
                             ,100 , 80, 300 , 100 , 90

                             ,150 , 100 , 120 , 0, 100
                             , 120 , 120, 120, 120, 120
                             , 120, 120, 120, 120, 120
                             , 120, 120, 120, 120
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
                                    ,true , true,  true,  true ,true  

                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  

                                    ,true , true,  true , true,  true
                                    ,true , true,  true , true,  true

                                    ,true , true,  true , true,  true
                                    ,  true , true,  true , true,  true

                                    ,  true , true,  true , true,  true
                                    ,  true , true,  true , true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter//5     = 
  
                               ,DataGridViewContentAlignment.MiddleCenter                            
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight//10

                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter   
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter

                               ,DataGridViewContentAlignment.MiddleRight  //15                                 
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight 

                               ,DataGridViewContentAlignment.MiddleRight  //20  
                                ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight 

                               ,DataGridViewContentAlignment.MiddleRight  //25  
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight

                                ,DataGridViewContentAlignment.MiddleRight  //30
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight

                               ,DataGridViewContentAlignment.MiddleRight  //35   
                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight 

                               ,DataGridViewContentAlignment.MiddleRight  //40
                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight

                               ,DataGridViewContentAlignment.MiddleCenter  //45
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter

                               ,DataGridViewContentAlignment.MiddleCenter  //50
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight

                               
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight //55

                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight //60

                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight //65

                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleLeft

                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft  //70
                               ,DataGridViewContentAlignment.MiddleLeft                                 
                               ,DataGridViewContentAlignment.MiddleLeft                            
                               ,DataGridViewContentAlignment.MiddleLeft  

                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleLeft //70
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft //75

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
                              };
            cgb.grid_col_alignment = g_Alignment;

            //Usp_Close_Pro_Give_Allowance1_Real
            //Usp_Close_Pro_Give_Allowance2
            //Usp_Close_Pro_Give_Allowance3
            //Usp_Close_Pro_Put_Return_Pay_1

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            
           // gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            //gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[16 - 1] = cls_app_static_var.str_Grid_Currency_Type;
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

            //gr_dic_cell_format[46 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[47 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[48 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[49 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[50 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[51 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            gr_dic_cell_format[52 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[53 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[54 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[55 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[56 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[57 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[58 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[59 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[60 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[61 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[62 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[63 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[64 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[65 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[66 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[67 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[68 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[69 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[70 - 1] = cls_app_static_var.str_Grid_Currency_Type;


            gr_dic_cell_format[86 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[87 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[88 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[89 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[90 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[91 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[92 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[93 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[94 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[95 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[96 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[97 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[98 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[99 - 1] = cls_app_static_var.str_Grid_Currency_Type;


            cgb.grid_cell_format = gr_dic_cell_format;

        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {

            int Col_Cnt = 0;
            object[] row0 = new object[cgb.grid_col_Count];

            while (Col_Cnt < cgb.grid_col_Count)
            {
               // row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];

                if  (Col_Cnt == 80 )
                    row0[Col_Cnt] = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt].ToString());
                else if  (Col_Cnt == 82 )
                    row0[Col_Cnt] = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt].ToString(), "Cpno");
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


            //Tsql = Tsql + " ,Allowance2_cut ";
            //Tsql = Tsql + ", Allowance3_Cut ";
            //Tsql = Tsql + ", (Allowance1_D + Allowance2_D + Allowance3_D +Allowance4_D + Allowance5_D) SumAllAllowance_Cut ";
                        
            //"스타보너스"   , "바이너리보너스"    , "추천매칭"   
            Sum_dic["후원보너스"] = 0;            
            Sum_dic["추천매칭"] = 0;
            Sum_dic["추천보너스"] = 0;
            
            //Sum_dic["올스타팩"] = 0;
            //Sum_dic["올스타팩_소급"] = 0;

            //Sum_dic["직급달성"] = 0;
            //Sum_dic["추천보너스"] = 0;
            //Sum_dic["리더십보너스"] = 0;
            //Sum_dic["PB보너스"] = 0;

            //Sum_dic["37프로모션"] = 0;
            //Sum_dic["SCP"] = 0;
            //Sum_dic["패키지보너스"] = 0;

            Sum_dic["기타보너스"] = 0;

            //, "매칭공제" , "추천보너스팩보너스공제"     , "Cap공제" 
            Sum_dic["반품공제액"] = 0;
            Sum_dic["기타공제"] = 0;
            //Sum_dic["추천보너스팩보너스공제"] = 0;
            Sum_dic["Cap공제"] = 0;


            Sum_dic["반품공제포함합"] = 0;
            Sum_dic["마감미지급액"] = 0;
            Sum_dic["이월수당합"] = 0;
            Sum_dic["지급수당합"] = 0;
            Sum_dic["소득세합"] = 0;
            Sum_dic["주민세합"] = 0;
            Sum_dic["실지급액합"] = 0;
            //Sum_dic["가지급급공제합"] = 0;
            //Sum_dic["실입금액"] = 0;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Sum_dic["후원보너스"] = Sum_dic["후원보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance1"].ToString());
                Sum_dic["추천매칭"] = Sum_dic["추천매칭"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance2"].ToString());


                Sum_dic["추천보너스"] = Sum_dic["추천보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance3"].ToString());
                //Sum_dic["올스타팩"] = Sum_dic["올스타팩"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance4"].ToString());
                //Sum_dic["올스타팩_소급"] = Sum_dic["올스타팩_소급"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance5"].ToString());

                //Sum_dic["직급달성"] = Sum_dic["직급달성"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance6"].ToString());
                //Sum_dic["추천보너스"] = Sum_dic["추천보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance7"].ToString());
                //Sum_dic["리더십보너스"] = Sum_dic["리더십보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance8"].ToString());
                //Sum_dic["PB보너스"] = Sum_dic["PB보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance9"].ToString());
                //Sum_dic["37프로모션"] = Sum_dic["37프로모션"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance13"].ToString());

                //Sum_dic["SCP"] = Sum_dic["SCP"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance12"].ToString());
                //Sum_dic["패키지보너스"] = Sum_dic["패키지보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance15"].ToString());


                Sum_dic["기타보너스"] = Sum_dic["기타보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Etc_Pay"].ToString());
                Sum_dic["반품공제액"] = Sum_dic["반품공제액"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Cur_DedCut_Pay"].ToString());

                Sum_dic["기타공제"] = Sum_dic["기타공제"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Etc_Pay_DedCut"].ToString());

                
                Sum_dic["Cap공제"] = Sum_dic["Cap공제"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumAllAllowance_Cut"].ToString());

                Sum_dic["반품공제포함합"] = Sum_dic["반품공제포함합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Cur_D_SumAllowance"].ToString());
                Sum_dic["마감미지급액"] = Sum_dic["마감미지급액"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Not_Pay_C"].ToString());
                Sum_dic["이월수당합"] = Sum_dic["이월수당합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Be_SumAllowance"].ToString());
                

                Sum_dic["지급수당합"] = Sum_dic["지급수당합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumAllAllowance"].ToString());
                Sum_dic["소득세합"] = Sum_dic["소득세합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InComeTax"].ToString());
                Sum_dic["주민세합"] = Sum_dic["주민세합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ResidentTax"].ToString());
                Sum_dic["실지급액합"] = Sum_dic["실지급액합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TruePayment"].ToString());

                //Sum_dic["가지급급공제합"] = Sum_dic["가지급급공제합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Ga_Cur_DedCut_Pay"].ToString());
                //Sum_dic["실입금액"] = Sum_dic["실입금액"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TruePayment_2"].ToString());
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

                //if (tb.Name == "txtIO")
                //{
                //    cgb_Pop.Next_Focus_Control = butt_Select;
                //    cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode, 0, "", " And  (Ncode ='004' OR Ncode = '005' ) ");
                //}
                //else
                //    cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode, 0);
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
                    cgb_Pop.db_grid_Popup_Base(2, "구매_코드", "구매종류", "SellCode", "SellTypeName", strSql);                 
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

                    cgb_Pop.db_grid_Popup_Base(2, "구매_코드", "구매종류", "SellCode", "SellTypeName", Tsql);                  
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
            butt_Excel_Detail_Down_Sd.SelectedIndex = 0;
            
            cls_Grid_Base cgb_P1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(dGridView_Pay_1, cgb_P1,2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Base_Header_Reset(dGridView_Pay_2, cgb_P1,1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Base_Header_Reset(dGridView_Pay_3, cgb_P1,3); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Base_Header_Reset(dGridView_Pay_4, cgb_P1, 33); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);
            
            dGridView_Base_Header_Reset(dGridView_Detail_2, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_SellData_Header_Reset(dGridView_Detail_1, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Up_S_Header_Reset(dGridView_Detail_3, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Up_S_Header_Reset(dGridView_Detail_4, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Grade_Header_Reset(dGridView_Down_G, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);


            //dGridView_Base_Header_Reset(dGridView_Pay_1, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_P1.d_Grid_view_Header_Reset();
            //Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "2", cgb_P1);  //추천매칭보너스

            //dGridView_Base_Header_Reset(dGridView_Pay_2, cgb_P1, 1); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_P1.d_Grid_view_Header_Reset();
            //Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "3", cgb_P1);  //추천보너스팩보너스

            //dGridView_Base_Header_Reset(dGridView_Pay_3, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_P1.d_Grid_view_Header_Reset();
            //Real_Allowance_PV_Detail(ToEndDate, Mbid, Mbid2, "5", cgb_P1);  //하선매출 내역

       
            //dGridView_Base_Header_Reset(dGridView_Pay_4, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_P1.d_Grid_view_Header_Reset();
            //Real_Allowance_Detail_Up(ToEndDate, Mbid, Mbid2, cgb_P1);  //판매내역 역추적



            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(butt_Excel_Detail_Down_Sd);
        }

        
        
        private void dGridView_Base_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //return; 
            if ((sender as DataGridView).CurrentRow != null)
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

                    return; 
                }
            }

           
        }
        
        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string T_Mbid = "", ToEndDate = "", FromEndDate = "";
                int LineCnt = 0;

                T_Mbid = (sender as DataGridView).CurrentRow.Cells[8].Value.ToString();
                ToEndDate = (sender as DataGridView).CurrentRow.Cells[7].Value.ToString();
                FromEndDate = (sender as DataGridView).CurrentRow.Cells[8].Value.ToString();
                ToEndDate = ToEndDate.Replace("-", "");
                FromEndDate = FromEndDate.Replace("-", "");
                LineCnt = int.Parse((sender as DataGridView).CurrentRow.Cells[0].Value.ToString().Substring(0, 1));

                cls_Search_DB csd = new cls_Search_DB();            
                string Mbid = ""; int Mbid2 = 0;
                csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);
                cls_Grid_Base cgb_P1 = new cls_Grid_Base();

                dGridView_Base_Header_Reset(dGridView_Pay_3, cgb_P1, 2);
                cgb_P1.d_Grid_view_Header_Reset();
                Real_Allowance_PV_Detail(ToEndDate, Mbid, Mbid2, "5", cgb_P1,"", LineCnt);

            }
        }


        private void Allowance_Detail(string T_Mbid, string ToEndDate, string Ga_FLAG)
        {
            cls_Search_DB csd = new cls_Search_DB();            
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);
        
            cls_Grid_Base cgb_P1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(dGridView_Pay_1, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "2", cgb_P1, Ga_FLAG);  //추천매칭보너스

            dGridView_Base_Header_Reset(dGridView_Pay_2, cgb_P1, 1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "3", cgb_P1, Ga_FLAG);  //추천보너스
                                  


            dGridView_Base_Header_Reset(dGridView_Pay_5, cgb_P1, 1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail_Up(ToEndDate, Mbid, Mbid2, "3", cgb_P1, Ga_FLAG);  //추천역추적

            dGridView_Base_Header_Reset(dGridView_Pay_8, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail_Up(ToEndDate, Mbid, Mbid2, "2", cgb_P1, Ga_FLAG);  //추천매칭역추적
                       


            dGridView_Base_Header_Reset(dGridView_Pay_3, cgb_P1, 3); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_PV_Detail(ToEndDate, Mbid, Mbid2, "1", cgb_P1, Ga_FLAG );  //하선매출 내역

                             
            dGridView_Base_Header_Reset(dGridView_Pay_4, cgb_P1, 33); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail_Up(ToEndDate, Mbid, Mbid2,  cgb_P1, Ga_FLAG);  //판매내역 역추적



            dGridView_Base_Header_Reset(dGridView_Pay_SP, cgb_P1, 6); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail_Not(ToEndDate, Mbid, Mbid2, "SP", cgb_P1, Ga_FLAG );  //미지급 관련


            //dGridView_Base_Header_Reset(dGridView_Pay_14, cgb_P1, 1); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_P1.d_Grid_view_Header_Reset();
            //Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "15", cgb_P1);  //패키지보너스




            //dGridView_Base_Header_Reset(dGridView_Pay_SP, cgb_P1, 6); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_P1.d_Grid_view_Header_Reset();
            //Real_Allowance_Detail2(ToEndDate, Mbid, Mbid2, "SP", cgb_P1);  //압축 추천 점수



            ////dGridView_Base_Header_Reset(dGridView_Pay_5, cgb_P1, 6); //디비그리드 헤더와 기본 셋팅을 한다.
            ////cgb_P1.d_Grid_view_Header_Reset();
            ////Real_Allowance_Detail_30000(ToEndDate, Mbid, Mbid2, "SP", cgb_P1);  //압축 추천 점수



            //dGridView_Base_Header_Reset(dGridView_Pay_Cap, cgb_P1, 7); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_P1.d_Grid_view_Header_Reset();
            //Real_Allowance_Detail_Cap(ToEndDate, Mbid, Mbid2, "SP", cgb_P1);  //압축 추천 점수




        }



        private void Pay_Detail(string T_Mbid, string ToEndDate, string FromEndDate, string Max_N_LineCnt, string Ga_FLAG )
        {
            cls_Search_DB csd = new cls_Search_DB();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);
            

            cls_Grid_Base cgb_V1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(dGridView_Detail_2, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();
            Real_Pay_Detail(ToEndDate, Mbid, Mbid2, 1, cgb_V1, Ga_FLAG);  //후원보너스관련 하선

            dGridView_Base_Header_Reset_Nom(dGridView_Detail_Down_N, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();
            Base_Grid_Set_Nom(ToEndDate, Mbid, Mbid2, cgb_V1, Max_N_LineCnt, Ga_FLAG );  //그룹하선 매출 내역

            dGridView_Base_Header_Reset_Save(dGridView_Detail_Down_S, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();
            Base_Grid_Set_Save(ToEndDate, Mbid, Mbid2, cgb_V1, Max_N_LineCnt, Ga_FLAG );  //그룹하선 매출 내역

            dGridView_SellData_Header_Reset(dGridView_Detail_1, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();
            Real_Pay_Detail(ToEndDate, Mbid, Mbid2, cgb_V1, FromEndDate);  //본인 매출 내역
            
            dGridView_Up_S_Header_Reset(dGridView_Detail_3, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();

            if (Ga_FLAG == "Y")
                Base_Grid_Set(ToEndDate, Mbid, Mbid2, "CKDPHARM_Ga_Close.dbo.ufn_Up_Search_Save_Close_02", cgb_V1, Ga_FLAG);
            else
                Base_Grid_Set(ToEndDate, Mbid, Mbid2, "ufn_Up_Search_Save_Close_02", cgb_V1, Ga_FLAG);

            dGridView_Up_S_Header_Reset(dGridView_Detail_4, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();

            if (Ga_FLAG == "Y")
                Base_Grid_Set(ToEndDate, Mbid, Mbid2, "CKDPHARM_Ga_Close.dbo.ufn_Up_Search_Nomin_Close_02", cgb_V1, Ga_FLAG);
            else
                Base_Grid_Set(ToEndDate, Mbid, Mbid2, "ufn_Up_Search_Nomin_Close_02", cgb_V1, Ga_FLAG);

          

            //Real_Pay_Detail_ETC(ToEndDate, Mbid, Mbid2); 
           // Real_Pay_Detail_ETC_N(ToEndDate, Mbid, Mbid2); 
        }


        private void Real_Allowance_Detail(string ToEndDate, string Mbid, int Mbid2, string SortOrder, cls_Grid_Base cgb_P, string Ga_FLAG)
        {
            string StrSql = "";

            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " RequestMbid + '-' + Convert(Varchar,RequestMbid2) ";
            else
                StrSql = StrSql + " RequestMbid2 ";

            StrSql = StrSql + ",RequestName,DownPV,tbl_Close_DownPV_ALL_02.LineCnt, tbl_Close_DownPV_ALL_02.LevelCnt";
            StrSql = StrSql + ",tbl_Close_DownPV_ALL_02.GivePay , tbl_Close_DownPV_ALL_02.R_LevelCnt, tbl_Close_DownPV_ALL_02.TPer ";
            StrSql = StrSql + ",ISnull(C1.Grade_Name,'')  GiveGrade     ";

            if (Ga_FLAG != "Y")
            {
                StrSql = StrSql + " From  tbl_Close_DownPV_ALL_02 (nolock) AS tbl_Close_DownPV_ALL_02 ";
                StrSql = StrSql + " LEFT JOIN   tbl_ClosePay_02_Mod (nolock) AS    tbl_ClosePay_02_Mod  ";
            }
            else
            {
                StrSql = StrSql + " From  CKDPHARM_Ga_Close.dbo.tbl_Close_DownPV_ALL_02 (nolock) AS tbl_Close_DownPV_ALL_02 ";
                StrSql = StrSql + " LEFT JOIN   CKDPHARM_Ga_Close.dbo.tbl_ClosePay_02_Mod (nolock) AS    tbl_ClosePay_02_Mod  ";
            }
            StrSql = StrSql + "  ON tbl_ClosePay_02_Mod.ToEndDate = tbl_Close_DownPV_ALL_02.EndDate ";
            StrSql = StrSql + "  And tbl_ClosePay_02_Mod.Mbid = tbl_Close_DownPV_ALL_02.Savembid ";
            StrSql = StrSql + "  And  tbl_ClosePay_02_Mod.Mbid2 = tbl_Close_DownPV_ALL_02.Savembid2 ";

            StrSql = StrSql + " Left Join tbl_Class C1  (nolock) On tbl_ClosePay_02_Mod.CurGrade_M2 = C1.Grade_Cnt ";
            
            StrSql = StrSql + " Where tbl_Close_DownPV_ALL_02.SaveMbid = '" + Mbid + "'";
            StrSql = StrSql + " And  tbl_Close_DownPV_ALL_02.SaveMbid2 = " + Mbid2 ;
            StrSql = StrSql + " And tbl_Close_DownPV_ALL_02.EndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " And LEFT(tbl_Close_DownPV_ALL_02.SortOrder,1)  ='" + SortOrder + "'";
            StrSql = StrSql + " Order By tbl_Close_DownPV_ALL_02.LevelCnt, tbl_Close_DownPV_ALL_02.LineCnt ";
            
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
                if (SortOrder == "3" || SortOrder == "4" || SortOrder == "5") Set_Pay_gr_dic2(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                if (SortOrder == "2") Set_Pay_gr_dic_2(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            
            if (gr_dic_text.Count > 0)
            {
                //put_Sum_Dataview(ds, ReCnt);                
            }

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();
        
        }




        private void Real_Allowance_Detail_Up(string ToEndDate, string Mbid, int Mbid2, string SortOrder, cls_Grid_Base cgb_P, string Ga_FLAG )
        {
            string StrSql = "";

            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + "  Savembid + '-' + Convert(Varchar,Savembid Savembid2) ";
            else
                StrSql = StrSql + " Savembid2  RequestMbid2 ";

            StrSql = StrSql + ",SaveName RequestName,tbl_Close_DownPV_ALL_02.DownPV,tbl_Close_DownPV_ALL_02.LineCnt, tbl_Close_DownPV_ALL_02.LevelCnt,tbl_Close_DownPV_ALL_02.GivePay , tbl_Close_DownPV_ALL_02.R_LevelCnt, tbl_Close_DownPV_ALL_02.TPer ,ISnull(C1.Grade_Name,'')  GiveGrade     ";


            if (Ga_FLAG != "Y")
            {
                StrSql = StrSql + " From  tbl_Close_DownPV_ALL_02 (nolock) AS tbl_Close_DownPV_ALL_02 ";
                StrSql = StrSql + " LEFT JOIN   tbl_ClosePay_02_Mod (nolock) AS    tbl_ClosePay_02_Mod  ";
            }
            else
            {
                StrSql = StrSql + " From  CKDPHARM_Ga_Close.dbo.tbl_Close_DownPV_ALL_02 (nolock) AS tbl_Close_DownPV_ALL_02 ";
                StrSql = StrSql + " LEFT JOIN   CKDPHARM_Ga_Close.dbo.tbl_ClosePay_02_Mod (nolock) AS    tbl_ClosePay_02_Mod  ";
            }
            StrSql = StrSql + "  ON tbl_ClosePay_02_Mod.ToEndDate = tbl_Close_DownPV_ALL_02.EndDate ";
            StrSql = StrSql + "  And tbl_ClosePay_02_Mod.Mbid = tbl_Close_DownPV_ALL_02.Savembid ";
            StrSql = StrSql + "  And  tbl_ClosePay_02_Mod.Mbid2 = tbl_Close_DownPV_ALL_02.Savembid2 ";

            StrSql = StrSql + " Left Join tbl_Class C1  (nolock) On tbl_ClosePay_02_Mod.CurGrade_M2 = C1.Grade_Cnt ";


            StrSql = StrSql + " Where tbl_Close_DownPV_ALL_02.RequestMbid = '" + Mbid + "'";
            StrSql = StrSql + " And tbl_Close_DownPV_ALL_02.RequestMbid2 = " + Mbid2;
            StrSql = StrSql + " And tbl_Close_DownPV_ALL_02.EndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " And LEFT(tbl_Close_DownPV_ALL_02.SortOrder,1)  ='" + SortOrder + "'";
            StrSql = StrSql + " Order By tbl_Close_DownPV_ALL_02.LevelCnt, tbl_Close_DownPV_ALL_02.LineCnt ";

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
                if (SortOrder == "3" || SortOrder == "4" || SortOrder == "5") Set_Pay_gr_dic2(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                if (SortOrder == "2") Set_Pay_gr_dic_2(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            if (gr_dic_text.Count > 0)
            {
                //put_Sum_Dataview(ds, ReCnt);                
            }

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();

        }


        private void Real_Allowance_PV_Detail(string ToEndDate, string Mbid, int Mbid2, string SortOrder, cls_Grid_Base cgb_P,string Ga_FLAG , int LineCnt = 0)
        {
            string StrSql = "";

            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " RequestMbid + '-' + Convert(Varchar,RequestMbid2) ";
            else
                StrSql = StrSql + " RequestMbid2 ";

            StrSql = StrSql + ",RequestName,Sell_DownPV DownPV,LineCnt, LevelCnt,OrderNumber,   ''   ";
            
         

            if (Ga_FLAG != "Y")
                StrSql = StrSql + " From  tbl_Close_DownPV_PV_02 (nolock) ";
            else
                StrSql = StrSql + " From  CKDPHARM_Ga_Close.dbo.tbl_Close_DownPV_PV_02 (nolock) ";

            StrSql = StrSql + " Where SaveMbid = '" + Mbid + "'";
            StrSql = StrSql + " And SaveMbid2 = " + Mbid2; 
            StrSql = StrSql + " And EndDate ='" + ToEndDate + "'";
            //StrSql = StrSql + " And (LineCnt = " + LineCnt + " OR " + LineCnt + " = 0 OR " + LineCnt + " IS NULL) ";
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

        private void Real_Allowance_Detail_Not(string ToEndDate, string Mbid, int Mbid2, string  SortOrder, cls_Grid_Base cgb_P, string Ga_FLAG )
        {
            string StrSql = "";

            StrSql = "Select LEFT(ToEndDate,4) +'-' + LEFT(RIGHT(ToEndDate,4),2) + '-' + RIGHT(ToEndDate,2)  ToEndDate  ";
            StrSql = StrSql + ", SumAllAllowance ";
            StrSql = StrSql + ", LEFT(AP_ToEndDate,4) +'-' + LEFT(RIGHT(AP_ToEndDate,4),2) + '-' + RIGHT(AP_ToEndDate,2)  AP_ToEndDate  ";

            if (Ga_FLAG != "Y")
                StrSql = StrSql + " From  tbl_Close_Not_Pay (nolock) ";
            else
                StrSql = StrSql + " From  CKDPHARM_Ga_Close.dbo.tbl_Close_Not_Pay (nolock) ";

            StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
            StrSql = StrSql + " And Mbid2 = " + Mbid2;
            StrSql = StrSql + " And ToEndDate <='" + ToEndDate + "'";
            StrSql = StrSql + " Order By Seq DESC  ";

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
                Set_Pay_gr_dic_Not(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();
        }



        private void Real_Allowance_Detail_Cap(string ToEndDate, string Mbid, int Mbid2, string SortOrder, cls_Grid_Base cgb_P)
        {
            string StrSql = "";

            //string[] g_HeaderText = {"캡적용%", "캡전후원보너스", "캡전추천매칭보너스", "캡전추천보너스팩보너스"  ,"캡전올스타팩보너스"  
            //                    ,"캡전올스타팩보너스_소급"     , "캡후후원보너스", "캡후추천매칭보너스", "캡후추천보너스팩보너스" ,"캡후올스타팩보너스" 
            //                      , "후원보너스공제", "추천매칭보너스공제", "추천보너스팩보너스공제" ,"올스타팩보너스공제" ,"올스타팩보너스_소급공제"  
            //                      , "캡전발생액", "캡_공제"   ,"캡후발생액"  
            //                        };


            StrSql = "Select SumAllAllowance_cut_per , Allowance1_R, Allowance2_R,Allowance3_R,Allowance4_R";
            StrSql = StrSql + ",Allowance5_R  ,Allowance1 ,Allowance2,Allowance3,Allowance4 ";
            StrSql = StrSql + ",Allowance5  ,Allowance1_D ,Allowance2_D,Allowance3_D,Allowance4_D ";
            StrSql = StrSql + ",Allowance5_D  ,Allowance1_R + Allowance2_R + Allowance3_R +Allowance4_R + Allowance5_R  ,Allowance1_D + Allowance2_D + Allowance3_D +Allowance4_D + Allowance5_D ,Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 ";                        
            StrSql = StrSql + " From  tbl_ClosePay_02_Mod (nolock) ";
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

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_Pay_gr_dic_Cap(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();
        }



        private void Real_Allowance_Detail_30000(string ToEndDate, string Mbid, int Mbid2, string SortOrder, cls_Grid_Base cgb_P)
        {
            string StrSql = "";



            StrSql = "Select LEFT(ToEndDate,4) +'-' + LEFT(RIGHT(ToEndDate,4),2) + '-' + RIGHT(ToEndDate,2)  ToEndDate ";
            StrSql = StrSql + ", SumAllAllowance ";
            StrSql = StrSql + ", LEFT(AP_ToEndDate,4) +'-' + LEFT(RIGHT(AP_ToEndDate,4),2) + '-' + RIGHT(AP_ToEndDate,2)  AP_ToEndDate  ";
            StrSql = StrSql + " From  tbl_ClosePay_10000 (nolock) ";
            StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
            StrSql = StrSql + " And Mbid2 = " + Mbid2;
            StrSql = StrSql + " And ToEndDate <='" + ToEndDate + "'";
            StrSql = StrSql + " Order By ToEndDate DESC  ";

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
                Set_Pay_gr_dic_Not(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();
        }



        private void Real_Allowance_Detail_Up(string ToEndDate, string Mbid, int Mbid2 , cls_Grid_Base cgb_P, string Ga_FLAG)
        {
            string StrSql = "";

            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " SaveMbid + '-' + Convert(Varchar,SaveMbid2) ";
            else
                StrSql = StrSql + " SaveMbid2 ";

            StrSql = StrSql + ",SaveName, DownPV ,OrderNumber, LevelCnt , ST1 ,LineCnt ";
            StrSql = StrSql + " From ";
            StrSql = StrSql + " ( " ;
            StrSql = StrSql + " Select EndDate, RequestMbid, RequestMbid2 ,RequestName ,OrderNumber, SaveMbid, SaveMbid2,SaveName, DownPV, LevelCnt,LineCnt,";
            StrSql = StrSql + " Case SortOrder When '3' then '추천' When '2' then '추천매칭' ELSE ''    ";
            StrSql = StrSql + " End AS ST1 ";

            if (Ga_FLAG != "Y")
                StrSql = StrSql + " From  tbl_Close_DownPV_ALL_02 (nolock) ";
            else
                StrSql = StrSql + " From  CKDPHARM_Ga_Close.dbo.tbl_Close_DownPV_ALL_02 (nolock) ";
            
            //StrSql = StrSql + " Left Join tbl_CloseTotal_02   (nolock) On tbl_CloseTotal_02.ToEndDate= tbl_Close_DownPV_ALL_02.EndDate  ";
        
            StrSql = StrSql + " UNION ALL";

            StrSql = StrSql + "  Select EndDate, RequestMbid , RequestMbid2 ,RequestName , OrderNumber,   SaveMbid ,  SaveMbid2,  SaveName, Sell_DownPV  DownPV , LevelCnt,LineCnt,";
            StrSql = StrSql + " '판매누적'  AS ST1 ";            
            //StrSql = StrSql + " From tbl_Close_DownPV_PV_02  (nolock) ";
            if (Ga_FLAG != "Y")
                StrSql = StrSql + " From  tbl_Close_DownPV_PV_02 (nolock) ";
            else
                StrSql = StrSql + " From  CKDPHARM_Ga_Close.dbo.tbl_Close_DownPV_PV_02 (nolock) ";

            //StrSql = StrSql + " Left Join tbl_CloseTotal_02  (nolock)  On tbl_CloseTotal_02.ToEndDate= tbl_Close_DownPV_PV_02.EndDate  ";

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

            if (S_TF != 7)  cgb_P.grid_col_Count = 10;
            if (S_TF == 7) cgb_P.grid_col_Count = 19;
            cgb_P.basegrid = dGridView;
            cgb_P.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            if (S_TF != 7) cgb_P.grid_Frozen_End_Count = 3;
            if (S_TF == 7) cgb_P.grid_Frozen_End_Count = 1;
            cgb_P.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            if (S_TF == 0)
            {
                string[] g_HeaderText = {"회원번호", "성명", "금액"  ,"대수"  ,"라인"  
                                , ""     , ""  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }
            else if (S_TF == 1)
            {
                string[] g_HeaderText = {"회원번호", "성명", "금액"  ,"대수"  ,"_라인"  
                                , "CV"     , ""  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }
            else if (S_TF == 2)
            {
                string[] g_HeaderText = {"회원번호", "성명", "금액"  ,"대수"  ,"_라인"  
                                , "직급"     , "후원보너스"  , "비율"   , "대수"    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }


            else if (S_TF == 3 )
            {
                string[] g_HeaderText = {"회원번호", "성명", "CV"  ,"대수"  ,"라인"  
                                , "주문번호"     , "구분"  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }

            else if (S_TF == 33)
            {
                string[] g_HeaderText = {"회원번호", "성명", "CV"  ,"대수"  ,"라인"  
                                , ""     , ""  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }

            else if (S_TF == 6)
            {
                string[] g_HeaderText = {"발생마감일", "금액", "지급마감일"  ,""  ,""  
                                , ""     , ""  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }

                //"후원보너스"   , "추천매칭보너스" , "추천보너스팩보너스"     , "올스타팩보너스"    
                //                , "올스타팩보너스_소급" 

            else if (S_TF == 7)
            {
                string[] g_HeaderText = {"캡적용%", "캡전후원보너스", "캡전추천매칭보너스", "캡전추천보너스팩보너스"  ,"캡전올스타팩보너스"  
                                ,"캡전올스타팩보너스_소급"     , "캡후후원보너스", "캡후추천매칭보너스", "캡후추천보너스팩보너스" ,"캡후올스타팩보너스" 
                                 ,"캡후올스타팩보너스_소급"  , "후원보너스공제", "추천매칭보너스공제", "추천보너스팩보너스공제" ,"올스타팩보너스공제" 
                                 ,"올스타팩보너스_소급공제"   , "캡전발생액", "캡_공제"   ,"캡후발생액"  
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }


            if (S_TF == 3 || S_TF == 5  )
            {
                int[] g_Width = { 100, 100 , 100, 100, 100                            
                             , 100, 0,0 , 0, 0 
                            };
                cgb_P.grid_col_w = g_Width;
            }
            else if ( S_TF == 1)
            {
                int[] g_Width = { 100, 100 , 100, 100, 0
                             , 100, 0,0 , 0, 0
                            };
                cgb_P.grid_col_w = g_Width;
            }

            else if (S_TF == 6)
            {
                int[] g_Width = { 120, 100 , 120, 0, 0                            
                             , 0, 0,0 , 0, 0 
                            };
                cgb_P.grid_col_w = g_Width;
            }
            else if (S_TF == 2)
            {
                int[] g_Width = { 100, 100 , 100, 100, 0                            
                             , 100, 100,100 , 100, 0 
                            };
                cgb_P.grid_col_w = g_Width;
            }

            else if (S_TF == 33)
            {
                int[] g_Width = { 100, 100 , 100, 100, 100                            
                             , 100, 100,0 , 0, 0 
                            };
                cgb_P.grid_col_w = g_Width;
            }

            else if (S_TF == 7)
            {
                int[] g_Width = { 100, 120 , 120, 120, 120                            
                             , 120, 120,120 , 120, 120
                             ,120,120,120,120,120
                             ,120,120,120,120
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


            if (S_TF != 7)
            {
                Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                         
                                   };
                cgb_P.grid_col_Lock = g_ReadOnly;
            }
            if (S_TF == 7)
            {
                Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                         
                                     ,true , true,  true , true,  true
                                      ,true , true,  true,  true
                                   };
                cgb_P.grid_col_Lock = g_ReadOnly;
            }

            if (S_TF == 6)
            {
                DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter//5    
  
                               ,DataGridViewContentAlignment.MiddleCenter                            
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter//10                         
                              };
                cgb_P.grid_col_alignment = g_Alignment;
            }
            else if (S_TF == 7)
            {
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
                               ,DataGridViewContentAlignment.MiddleRight//10         
                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleRight

                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight//10         
                               ,DataGridViewContentAlignment.MiddleRight//10         
                              };
                cgb_P.grid_col_alignment = g_Alignment;
            }
            else if(S_TF == 1)
            {
                DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter//5    
  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter//10                         
                              };
                cgb_P.grid_col_alignment = g_Alignment;
            }
            else
            {
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
            }

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            if (S_TF == 2)
            {
                gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            }

            if (S_TF == 1)
            {
                gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            }

            if (S_TF == 6)
            {
                gr_dic_cell_format[2 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            }

            if (S_TF == 7)
            {
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
                gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[16 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[17 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[18 - 1] = cls_app_static_var.str_Grid_Currency_Type;
                gr_dic_cell_format[19 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            }
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

        private void Set_Pay_gr_dic_2(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["RequestName"]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["DownPV"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LevelCnt"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LineCnt"]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["GiveGrade"] 
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["GivePay"] 
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["TPer"]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["R_LevelCnt"]   
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
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LineCnt"]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["ST1"] 
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
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LineCnt"]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"]
                                ,""
                                ,""
                                ,""
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void Set_Pay_gr_dic_Not(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = {    ds.Tables[base_db_name].Rows[fi_cnt]["ToEndDate"]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["SumAllAllowance"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["AP_ToEndDate"]
                                ,""
                                ,""
 
                                ,""
                                ,""
                                ,""
                                ,""
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void Set_Pay_gr_dic_Cap(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
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
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }





        private void Real_Pay_Detail(string ToEndDate, string Mbid, int Mbid2, int SortOrder, cls_Grid_Base cgb_P, string Ga_FLAG )
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
            StrSql = StrSql + ",ToEndDate, FromEndDate,0  A1_Point ";

            //StrSql = StrSql + ",Regtime, CurPoint_Date_2, CurPoint_Date_3 ";

            if (Ga_FLAG == "Y" )
                StrSql = StrSql + " From  CKDPHARM_Ga_Close.dbo.tbl_ClosePay_02_Mod (nolock) ";
            else
                StrSql = StrSql + " From  tbl_ClosePay_02_Mod (nolock) ";

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
                                ,ds.Tables[base_db_name].Rows[0]["ToEndDate"]
                                ,ds.Tables[base_db_name].Rows[0]["FromEndDate"]
                                ,ds.Tables[base_db_name].Rows[0]["A1_Point"]
                                ,""
                                 };

            gr_dic_text[1] = row0;

            object[] row1 = { "2라인"
                                ,ds.Tables[base_db_name].Rows[0]["Be_PV_2"]  
                                ,ds.Tables[base_db_name].Rows[0]["Cur_PV_2"]
                                ,ds.Tables[base_db_name].Rows[0]["Ded_2"]
                                ,ds.Tables[base_db_name].Rows[0]["Fresh_2"]
 
                                ,ds.Tables[base_db_name].Rows[0]["Sum_PV_2"]
                                ,ds.Tables[base_db_name].Rows[0]["ToEndDate"]
                                ,ds.Tables[base_db_name].Rows[0]["FromEndDate"]
                                ,ds.Tables[base_db_name].Rows[0]["A1_Point"]
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
                            , "이월"     , "_ToEndDate"  , "_FromEndDate"   , "_사이클"    , ""                                   
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
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb_P.grid_cell_format = gr_dic_cell_format;
        }










        private void Base_Grid_Set_Nom(string ToEndDate, string Mbid, int Mbid2, cls_Grid_Base cgb_P, string Max_N_LineCnt, string Ga_FLAG )
        {

            string Tsql = "";

            //string[] g_HeaderText = {"라인","회원번호", "성명" ,"하선PV1", "본인PV1"
            //                ,"하선PV2"  , "본인PV2"     , "본인액티브여부1"  , "본인액티브여부2"   , ""
            //                  , ""   ,"" ,"","",""
            //               ,""    ,""
            //                    };
            Tsql = "Select  ";
            Tsql = Tsql + " T_up.N_LineCnt ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", T_up.mbid + '-' + Convert(Varchar,T_up.mbid2) ";
            else
                Tsql = Tsql + ", T_up.mbid2 ";

            Tsql = Tsql + " ,T_up.M_Name ";
            Tsql = Tsql + " ,T_up.N_Down_PV ";
            Tsql = Tsql + " ,T_up.Self_Month_PV ";
            Tsql = Tsql + " ,T_up.N_Down_PV_M2 ";
            Tsql = Tsql + " ,T_up.Self_Month_PV_M2 ";
            Tsql = Tsql + " ,T_up.Grade_ReqTF2 ";
            Tsql = Tsql + " ,T_up.Grade_ReqTF2_M2 ";
            Tsql = Tsql + " ,0 N_GradeCnt5 ";
            Tsql = Tsql + " ,0  N_GradeCnt6 ";
            Tsql = Tsql + " ,0  N_GradeCnt7 ";
            Tsql = Tsql + " ,0 N_GradeCnt8 ";
            Tsql = Tsql + " ,0 N_GradeCnt9 ";
            Tsql = Tsql + " ,0 N_GradeCnt10 ";
            Tsql = Tsql + " ,0 N_GradeCnt11 ";
            Tsql = Tsql + " ,0 N_GradeCnt12 ";


            if (Ga_FLAG == "Y")
                Tsql = Tsql + " From  CKDPHARM_Ga_Close.dbo.tbl_ClosePay_02_Mod (nolock) T_up ";
            else
                Tsql = Tsql + " From  tbl_ClosePay_02_Mod (nolock)  T_up";


            //Tsql = Tsql + " From tbl_ClosePay_02_Mod (nolock) T_up  ";
            Tsql = Tsql + " Where    ToEndDate ='" + ToEndDate  + "'";
            Tsql = Tsql + " And      Nominid ='" + Mbid + "'";
            Tsql = Tsql + " And      Nominid2 =" + Mbid2;
            Tsql = Tsql + " Order BY   N_LineCnt ASC ";

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
                Set_gr_dic_Line_Nom(ref ds, ref gr_dic_text, fi_cnt, ref  cgb_P);  //데이타를 배열에 넣는다.
            }
            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();



            //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            //{
            //    if (ds.Tables[base_db_name].Rows[fi_cnt][0].ToString() == Max_N_LineCnt)
            //    {
            //        cgb_P.basegrid.Rows[fi_cnt].DefaultCellStyle.BackColor = System.Drawing.Color.PaleGoldenrod;
            //    }

            //}


        }

        private void Set_gr_dic_Line_Nom(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt, ref cls_Grid_Base cgb_P)
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



        private void dGridView_Base_Header_Reset_Nom(DataGridView dGridView, cls_Grid_Base cgb_P)
        {

            cgb_P.grid_col_Count = 17;
            cgb_P.basegrid = dGridView;
            cgb_P.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_P.grid_Frozen_End_Count = 2;
            cgb_P.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"라인","회원번호", "성명" ,"하선PV1", "본인PV1"
                            ,"하선PV2"  , "본인PV2"     , "본인액티브여부1"  , "본인액티브여부2"   , ""
                              , ""   ,"" ,"","",""
                           ,""    ,""
                                };
            cgb_P.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 100 , 100, 100, 100                            
                             , 100, 100,100 , 100, 0 
                             , 0, 0,0 , 0, 0 
                             ,0 ,0
                            };
            cgb_P.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                         
                                    ,true , true,  true,  true ,true                                                                         
                                    ,true,true
                                   };
            cgb_P.grid_col_Lock = g_ReadOnly;

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
                               ,DataGridViewContentAlignment.MiddleRight//10                         

                               ,DataGridViewContentAlignment.MiddleRight//10                         
                               ,DataGridViewContentAlignment.MiddleRight//10  
                              };
            cgb_P.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
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
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[16 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[17 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb_P.grid_cell_format = gr_dic_cell_format;
        }







        private void Base_Grid_Set_Save(string ToEndDate, string Mbid, int Mbid2, cls_Grid_Base cgb_P, string Max_N_LineCnt, string Ga_FLAG )
        {

            string Tsql = "";

            //string[] g_HeaderText = {"라인","회원번호", "성명" ,"당월하선PV1", "당월본인PV1"
            //               , "합1","당월하선PV2","당월본인PV2" ,"합2" ,""
            //               , "", "" , ""    , ""  ,""
            //               ,"",""
            //                    };
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            Tsql = "Select  ";
            Tsql = Tsql + " T_up.LineCnt ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", T_up.mbid + '-' + Convert(Varchar,T_up.mbid2) ";
            else
                Tsql = Tsql + ", T_up.mbid2 ";

            Tsql = Tsql + " ,T_up.M_Name ";
            Tsql = Tsql + " ,T_up.Down_PV_1 + Down_PV_2 ";
            Tsql = Tsql + " ,T_up.Self_Month_PV ";
            Tsql = Tsql + " ,T_up.Self_Month_PV + T_up.Down_PV_1 + T_up.Down_PV_2 ";
            Tsql = Tsql + " ,T_up.Down_PV_M2_1 + Down_PV_M2_2 ";
            Tsql = Tsql + " ,T_up.Self_Month_PV_M2  ";
            Tsql = Tsql + " ,T_up.Self_Month_PV_M2 + T_up.Down_PV_M2_1 + T_up.Down_PV_M2_2 ";
            Tsql = Tsql + " ,0 N_GradeCnt5 ";
            Tsql = Tsql + " ,0  N_GradeCnt6 ";
            Tsql = Tsql + " ,0  N_GradeCnt7 ";
            Tsql = Tsql + " ,0 N_GradeCnt8 ";
            Tsql = Tsql + " ,0 N_GradeCnt9 ";
            Tsql = Tsql + " ,0 N_GradeCnt10 ";
            Tsql = Tsql + " ,0 N_GradeCnt11 ";
            Tsql = Tsql + " ,0 N_GradeCnt12 ";

            if (Ga_FLAG == "Y")
                Tsql = Tsql + " From  CKDPHARM_Ga_Close.dbo.tbl_ClosePay_02_Mod (nolock) T_up ";
            else
                Tsql = Tsql + " From  tbl_ClosePay_02_Mod (nolock) T_up ";


            //Tsql = Tsql + " From tbl_ClosePay_02_Mod (nolock) T_up  ";
            Tsql = Tsql + " Where    ToEndDate ='" + ToEndDate + "'";
            Tsql = Tsql + " And     Saveid ='" + Mbid + "'";
            Tsql = Tsql + " And      Saveid2 =" + Mbid2;
            Tsql = Tsql + " Order BY   LineCnt ASC ";
            //당일 등록된 회원을 불러온다.

            //++++++++++++++++++++++++++++++++

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
                Set_gr_dic_Line_Save(ref ds, ref gr_dic_text, fi_cnt, ref  cgb_P);  //데이타를 배열에 넣는다.
            }
            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();



            //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            //{
            //    if (ds.Tables[base_db_name].Rows[fi_cnt][0].ToString() == Max_N_LineCnt)
            //    {
            //        cgb_P.basegrid.Rows[fi_cnt].DefaultCellStyle.BackColor = System.Drawing.Color.PaleGoldenrod;
            //    }

            //}


        }

        private void Set_gr_dic_Line_Save(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt, ref cls_Grid_Base cgb_P)
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



        private void dGridView_Base_Header_Reset_Save(DataGridView dGridView, cls_Grid_Base cgb_P)
        {

            cgb_P.grid_col_Count = 17;
            cgb_P.basegrid = dGridView;
            cgb_P.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_P.grid_Frozen_End_Count = 2;
            cgb_P.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"라인","회원번호", "성명" ,"당월하선PV1", "당월본인PV1"
                           , "합1","당월하선PV2","당월본인PV2" ,"합2" ,""
                           , "", "" , ""    , ""  ,"" 
                           ,"",""
                                };
            cgb_P.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 100 , 100, 100, 100                            
                             , 100, 100,100 , 100, 0 
                             , 0, 0,0 , 0, 0 
                             ,0 ,0
                            };
            cgb_P.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                         
                                    ,true , true,  true,  true ,true                                                                         
                                    ,true,true
                                   };
            cgb_P.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight//5    
  
                               ,DataGridViewContentAlignment.MiddleCenter                            
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight//10                         

                               ,DataGridViewContentAlignment.MiddleRight                            
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleCenter//10                         

                               ,DataGridViewContentAlignment.MiddleRight//10                         
                               ,DataGridViewContentAlignment.MiddleRight//10  
                              };
            cgb_P.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
           // gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[16 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[17 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb_P.grid_cell_format = gr_dic_cell_format;
        }






        private void Real_Pay_Detail(string ToEndDate, string Mbid, int Mbid2,  cls_Grid_Base cgb_P, string FromEndDate)
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

            //string[] g_HeaderText = {"구매종류", "이전금액", "기간금액"  ,"총합금액" ," " 
            //                         , "이전PV", "기간PV"  ,"총합PV" , " " ,"이전반품금액"    
            //                          , "기간반품금액"  , "반품총합금액"  , " " , "이전반품PV"     , "기간반품PV"  
            //                          , "반품총합PV"
            //                    };

            int row_num = 0;
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                string SellCode = ds.Tables[base_db_name].Rows[fi_cnt]["SellCode"].ToString();
                string SellTypeName = ds.Tables[base_db_name].Rows[fi_cnt]["SellTypeName"].ToString(); 

                //StrSql = "SELECT BeTotalPV, BeTotalCV, BeShamSell,BeAmount ";
                //StrSql = StrSql + ",DayTotalPV, DayTotalCV, DayShamSell,DayAmount ";
                //StrSql = StrSql + ",SumTotalPV, SumTotalCV, SumShamSell,SumAmount ";

                //StrSql = StrSql + ",BeReTotalPV, BeReTotalCV, BeReAmount ";
                //StrSql = StrSql + ",DayReTotalPV, DayReTotalCV, DayReAmount " ;
                //StrSql = StrSql + ",SumReTotalPV, SumReTotalCV, SumReAmount ";
                //StrSql = StrSql + " FROM tbl_ClosePay_02_Sell_Mod (nolock) ";

                //StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
                //StrSql = StrSql + " And Mbid2 = " + Mbid2;
                //StrSql = StrSql + " And ToEndDate ='" + ToEndDate + "'";
                //StrSql = StrSql + " And SellCode ='" + SellCode + "'" ;

                StrSql = "Select COUNT(1) From tbl_SalesDetail (nolock)";
                StrSql = StrSql + " Where mbid = '" + Mbid + "' ";
                StrSql = StrSql + " And mbid2 = " + Mbid2;
                StrSql = StrSql + " And SellDate_2 <= '" + ToEndDate + "' ";
                StrSql = StrSql + " And SellCode = '" + SellCode + "' ";

                //++++++++++++++++++++++++++++++++

                DataSet ds3 = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Count", ds3, this.Name, this.Text) == false) return;

                if (int.Parse(ds3.Tables["tbl_Count"].Rows[0][0].ToString()) > 0)
                {

                    StrSql = " Select ";
                    StrSql = StrSql + " SUM(T.BeTotalPV) BeTotalPV, SUM(T.BeTotalCV) BeTotalCV, SUM(T.BeShamSell) BeShamSell, SUM(T.BeAmount) BeAmount ";
                    StrSql = StrSql + " ,SUM(T.DayTotalPV) DayTotalPV, SUM(T.DayTotalCV) DayTotalCV, SUM(T.DayShamSell) DayShamSell, SUM(T.DayAmount) DayAmount ";
                    StrSql = StrSql + " ,SUM(T.BeTotalPV + T.DayTotalPV) SumTotalPV, SUM(T.BeTotalCV + T.DayTotalCV) SumTotalCV, SUM(T.BeShamSell + T.DayShamSell) SumShamSell, SUM(T.BeAmount + T.DayAmount) SumAmount ";

                    StrSql = StrSql + " ,SUM(T.BeReTotalPV) BeReTotalPV, SUM(T.BeReTotalCV) BeReTotalCV, SUM(T.BeReAmount) BeReAmount ";
                    StrSql = StrSql + " ,SUM(T.DayReTotalPV) DayReTotalPV, SUM(T.DayReTotalCV) DayReTotalCV, SUM(T.DayReAmount) DayReAmount ";
                    StrSql = StrSql + " ,SUM(T.BeReTotalPV + T.DayReTotalPV) SumReTotalPV, SUM(T.BeReTotalCV + T.DayReTotalCV) SumReTotalCV, SUM(T.BeReAmount + T.DayReAmount) SumReAmount ";
                    StrSql = StrSql + " From ( ";
                    StrSql = StrSql + " Select ";
                    StrSql = StrSql + " Case WHEN SellDate_2 < '" + FromEndDate + "' And TotalPrice > 0 THEN TotalPV ELSE 0 END BeTotalPV ";
                    StrSql = StrSql + " ,Case WHEN SellDate_2 < '" + FromEndDate + "' And TotalPrice > 0 THEN TotalCV ELSE 0 END BeTotalCV ";
                    StrSql = StrSql + " , 0 BeShamSell ";
                    StrSql = StrSql + " ,Case WHEN SellDate_2 < '" + FromEndDate + "' And TotalPrice > 0 THEN TotalPrice ELSE 0 END BeAmount ";

                    StrSql = StrSql + " , Case WHEN SellDate_2 <= '" + ToEndDate + "' And SellDate_2 >= '" + FromEndDate + "' And TotalPrice > 0 THEN TotalPV ELSE 0 END DayTotalPV ";
                    StrSql = StrSql + " , Case WHEN SellDate_2 <= '" + ToEndDate + "' And SellDate_2 >= '" + FromEndDate + "' And TotalPrice > 0 THEN TotalCV ELSE 0 END DayTotalCV ";
                    StrSql = StrSql + " , 0 DayShamSell ";
                    StrSql = StrSql + " , Case WHEN SellDate_2 <= '" + ToEndDate + "' And SellDate_2 >= '" + FromEndDate + "' And TotalPrice > 0 THEN TotalPrice ELSE 0 END DayAmount ";

                    StrSql = StrSql + " ,Case WHEN SellDate_2 < '" + FromEndDate + "' And TotalPrice < 0 THEN TotalPV ELSE 0 END BeReTotalPV ";
                    StrSql = StrSql + " ,Case WHEN SellDate_2 < '" + FromEndDate + "' And TotalPrice < 0 THEN TotalCV ELSE 0 END BeReTotalCV ";
                    StrSql = StrSql + " ,Case WHEN SellDate_2 < '" + FromEndDate + "' And TotalPrice < 0 THEN TotalPrice ELSE 0 END BeReAmount ";

                    StrSql = StrSql + " , Case WHEN SellDate_2 <= '" + ToEndDate + "' And SellDate_2 >= '" + FromEndDate + "' And TotalPrice < 0 THEN TotalPV ELSE 0 END DayReTotalPV ";
                    StrSql = StrSql + " , Case WHEN SellDate_2 <= '" + ToEndDate + "' And SellDate_2 >= '" + FromEndDate + "' And TotalPrice < 0 THEN TotalCV ELSE 0 END DayReTotalCV ";
                    StrSql = StrSql + " , Case WHEN SellDate_2 <= '" + ToEndDate + "' And SellDate_2 >= '" + FromEndDate + "' And TotalPrice < 0 THEN TotalPrice ELSE 0 END DayReAmount ";

                    StrSql = StrSql + " From tbl_SalesDetail (nolock) ";
                    StrSql = StrSql + " Where mbid = '" + Mbid + "' ";
                    StrSql = StrSql + " And mbid2 = " + Mbid2;
                    StrSql = StrSql + " And SellDate_2 <= '" + ToEndDate + "' ";
                    StrSql = StrSql + " And SellCode = '" + SellCode + "' ";
                    StrSql = StrSql + " And Ga_Order = 0 ";
                    StrSql = StrSql + " ) T ";

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

                        gr_dic_text[row_num + 1] = row0;
                        row_num = row_num + 1;
                    }
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

            string[] g_HeaderText = {"구매종류", "이전금액", "기간금액"  ,"총합금액" ," " 
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
                                     ,true , true,  true, true, true
                                     ,true
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
                               ,DataGridViewContentAlignment.MiddleRight     
                               ,DataGridViewContentAlignment.MiddleRight     //15

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
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[16 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb_P.grid_cell_format = gr_dic_cell_format;

            cgb_P.basegrid.RowHeadersVisible = false;

        }





        private void Base_Grid_Set(string ToEndDate, string Mbid, int Mbid2, string Ufn_Name, cls_Grid_Base cgb_P, string Ga_FLAG)
        {
            
            string Tsql = "";

            Tsql = "Select  ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " T_up.mbid + '-' + Convert(Varchar,T_up.mbid2) ";
            else
                Tsql = Tsql + " T_up.mbid2 ";

            Tsql = Tsql + " ,T_up.M_Name ";
            Tsql = Tsql + " ,T_up.B3Grade_Name ";
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
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
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

            string[] g_HeaderText = {"회원_번호"  , "성명"   , "직급"  ,"위치" , ""        
                                    };
            cgb_P.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 60, 70, 100, 30, 0                               
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
            //StrSql = StrSql + ",N_GradeCnt4_1, N_GradeCnt4_2 ";
            //StrSql = StrSql + ",N_GradeCnt5_1, N_GradeCnt5_2 ";
            StrSql = StrSql + ",N_GradeCnt6_1, N_GradeCnt6_2 ";
            StrSql = StrSql + ",N_GradeCnt7_1, N_GradeCnt7_2 ";
            StrSql = StrSql + ",N_GradeCnt8_1, N_GradeCnt8_2 ";
            StrSql = StrSql + ",N_GradeCnt9_1, N_GradeCnt9_2 ";
            StrSql = StrSql + ",N_GradeCnt10_1, N_GradeCnt10_2 ";
            StrSql = StrSql + ",N_GradeCnt11_1, N_GradeCnt11_2 ";
            StrSql = StrSql + ",N_GradeCnt12_1, N_GradeCnt12_2 ";

            StrSql = StrSql + " From  tbl_ClosePay_02_Mod (nolock) ";
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

            //txt_ETC1.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Cur_PV_4_1"].ToString()));
            //txt_ETC4.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Cur_PV_4_2"].ToString()));

            //txt_ETC2.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Cur_PV_1"].ToString()));
            //txt_ETC3.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Cur_PV_2"].ToString()));

            //G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt4_1"].ToString();
            //G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt4_2"].ToString();
            //txt_ETC4.Text = G_Left + " / " + G_Right;

            //G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt5_1"].ToString();
            //G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt5_2"].ToString();
            //txt_ETC5.Text = G_Left + " / " + G_Right;

            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt6_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt6_2"].ToString();
            txt_ETC6.Text = G_Left;
            txt_ETC6_2.Text = G_Right;
            

            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt7_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt7_2"].ToString();
            txt_ETC7.Text = G_Left;
            txt_ETC7_2.Text = G_Right;


            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt8_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt8_2"].ToString();
            txt_ETC8.Text = G_Left;
            txt_ETC8_2.Text = G_Right;

            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt9_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt9_2"].ToString();
            txt_ETC9.Text = G_Left;
            txt_ETC9_2.Text = G_Right;

            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt10_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt10_2"].ToString();
            txt_ETC10.Text = G_Left;
            txt_ETC10_2.Text = G_Right;

            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt11_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt11_2"].ToString();
            txt_ETC11.Text = G_Left;
            txt_ETC11_2.Text = G_Right;


            //G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt12_1"].ToString();
            //G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt12_2"].ToString();
            //txt_ETC12.Text = G_Left + " / " + G_Right;
            //txt_ETC8.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Be_PV_1"].ToString()));
            //txt_ETC9.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Be_PV_2"].ToString()));

            //txt_ETC10.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Sum_PV_1"].ToString()));
            //txt_ETC11.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Sum_PV_2"].ToString()));

            //txt_ETC3.Text = ds.Tables[base_db_name].Rows[0]["N_GradeCnt2_1"].ToString();
            //txt_ETC4.Text = ds.Tables[base_db_name].Rows[0]["N_GradeCnt2_2"].ToString();

            //txt_ETC5.Text = ds.Tables[base_db_name].Rows[0]["N_GradeCnt4_1"].ToString();
            //txt_ETC6.Text = ds.Tables[base_db_name].Rows[0]["N_GradeCnt4_2"].ToString();

            //if (int.Parse(ds.Tables[base_db_name].Rows[0]["ReqTF2"].ToString()) >= 1)
            //    txt_ETC7.Text = "유";
        }


        private void Real_Pay_Detail_ETC_N(string ToEndDate, string Mbid, int Mbid2)
        {


            string StrSql = "";
            string G_Left = "", G_Right = "";
            ////if (cls_app_static_var.Member_Number_1 > 0)
            ////    StrSql = StrSql + " RequestMbid + '-' + Convert(Varchar,RequestMbid2) ";
            ////else
            ////    StrSql = StrSql + " RequestMbid2 ";

            StrSql = "Select ";

            StrSql = StrSql + " N_GradeCnt1_1 ";
            StrSql = StrSql + ", N_GradeCnt2_1 ";
            StrSql = StrSql + ", N_GradeCnt3_1 ";
            StrSql = StrSql + ", N_GradeCnt4_1 ";
            StrSql = StrSql + ", N_GradeCnt5_1 ";
            StrSql = StrSql + ", N_GradeCnt6_1 ";
            StrSql = StrSql + ", N_GradeCnt7_1 ";
            StrSql = StrSql + ", N_GradeCnt8_1 ";
            StrSql = StrSql + ", N_GradeCnt9_1 ";

            StrSql = StrSql + ", N_GradeCnt1_2 ";
            StrSql = StrSql + ", N_GradeCnt2_2 ";
            StrSql = StrSql + ", N_GradeCnt3_2 ";
            StrSql = StrSql + ", N_GradeCnt4_2 ";
            StrSql = StrSql + ", N_GradeCnt5_2 ";
            StrSql = StrSql + ", N_GradeCnt6_2 ";
            StrSql = StrSql + ", N_GradeCnt7_2 ";
            StrSql = StrSql + ", N_GradeCnt8_2 ";
            StrSql = StrSql + ", N_GradeCnt9_2 ";
            
            StrSql = StrSql + ", Down_W4_QV_Real_1, Down_W4_QV_Real_2  "; 
            StrSql = StrSql + " From  tbl_ClosePay_02_Mod (nolock) ";
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

           // txt_Max_N_LineCnt.Text = ds.Tables[base_db_name].Rows[0]["Max_N_LineCnt"].ToString(); 
            //string.Format(cls_app_static_var.str_Currency_Type, T_p);

            //txt_ETC1.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Cur_PV_4_1"].ToString()));
            //txt_ETC4.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Cur_PV_4_2"].ToString()));

            //txt_ETC2.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Cur_PV_1"].ToString()));
            //txt_ETC3.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Cur_PV_2"].ToString()));

            //G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt4_1"].ToString();
            //G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt4_2"].ToString();
            //txt_ETC4.Text = G_Left + " / " + G_Right;

            //G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt5_1"].ToString();
            //G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt5_2"].ToString();
            //txt_ETC5.Text = G_Left + " / " + G_Right;

            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt1_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt1_2"].ToString();
            txt_ETC6.Text = G_Left;
            txt_ETC6_2.Text = G_Right;


            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt2_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt2_2"].ToString();
            txt_ETC7.Text = G_Left;
            txt_ETC7_2.Text = G_Right;


            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt3_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt3_2"].ToString();
            txt_ETC8.Text = G_Left;
            txt_ETC8_2.Text = G_Right;

            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt4_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt4_2"].ToString();
            txt_ETC9.Text = G_Left;
            txt_ETC9_2.Text = G_Right;

            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt5_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt5_2"].ToString();
            txt_ETC10.Text = G_Left;
            txt_ETC10_2.Text = G_Right;

            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt6_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt6_2"].ToString();
            txt_ETC11.Text = G_Left;
            txt_ETC11_2.Text = G_Right;


            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt7_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt7_2"].ToString();
            txt_ETC_N_7.Text = G_Left;
            txt_ETC_N_7_2.Text = G_Right;


            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt8_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt8_2"].ToString();
            txt_ETC_N_8.Text = G_Left;
            txt_ETC_N_8_2.Text = G_Right;


            G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt9_1"].ToString();
            G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt9_2"].ToString();
            txt_ETC_N_9.Text = G_Left;
            txt_ETC_N_9_2.Text = G_Right;


            //G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt10_1"].ToString();
            //G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt10_2"].ToString();
            //txt_ETC_N_10.Text = G_Left;
            //txt_ETC_N_10_2.Text = G_Right;



            //G_Left = ds.Tables[base_db_name].Rows[0]["Max_N_GradeCnt11"].ToString();
            //G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt11"].ToString();
            //txt_ETC_N_11.Text = G_Left;
            //txt_ETC_N_11_2.Text = G_Right;

            //G_Left = ds.Tables[base_db_name].Rows[0]["Max_N_GradeCnt12"].ToString();
            //G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt12"].ToString();
            //txt_ETC_N_12.Text = G_Left;
            //txt_ETC_N_12_2.Text = G_Right;

            txt_ETC_S_D_1.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["Down_W4_QV_Real_1"].ToString()));
            txt_ETC_S_D_2.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["Down_W4_QV_Real_2"].ToString()));

            //G_Left = ds.Tables[base_db_name].Rows[0]["N_GradeCnt12_1"].ToString();
            //G_Right = ds.Tables[base_db_name].Rows[0]["N_GradeCnt12_2"].ToString();
            //txt_ETC12.Text = G_Left + " / " + G_Right;
            //txt_ETC8.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Be_PV_1"].ToString()));
            //txt_ETC9.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Be_PV_2"].ToString()));

            //txt_ETC10.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Sum_PV_1"].ToString()));
            //txt_ETC11.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Sum_PV_2"].ToString()));

            //txt_ETC3.Text = ds.Tables[base_db_name].Rows[0]["N_GradeCnt2_1"].ToString();
            //txt_ETC4.Text = ds.Tables[base_db_name].Rows[0]["N_GradeCnt2_2"].ToString();

            //txt_ETC5.Text = ds.Tables[base_db_name].Rows[0]["N_GradeCnt4_1"].ToString();
            //txt_ETC6.Text = ds.Tables[base_db_name].Rows[0]["N_GradeCnt4_2"].ToString();

            //if (int.Parse(ds.Tables[base_db_name].Rows[0]["ReqTF2"].ToString()) >= 1)
            //    txt_ETC7.Text = "유";
        }



        private void butt_G60_1_Click(object sender, EventArgs e)
        {

            //butt_G60_1_Click_2(sender, e); 
            //return;

             Button bt = (Button)sender;
             cls_Grid_Base cgb_P1 = new cls_Grid_Base();

            int S_LineCnt = 0 , S_Grde = 0 ;

             dGridView_Grade_Header_Reset(dGridView_Down_G, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
             cgb_P1.d_Grid_view_Header_Reset();

             if (bt.Name == "butt_G10_1" && (txt_ETC6.Text == "" || txt_ETC6.Text == "0")) return;
             if (bt.Name == "butt_G10_2" && (txt_ETC6_2.Text == "" || txt_ETC6_2.Text == "0")) return;


             if (bt.Name == "butt_G20_1" && (txt_ETC7.Text == "" || txt_ETC7.Text == "0")) return;
             if (bt.Name == "butt_G20_2" && (txt_ETC7_2.Text == "" || txt_ETC7_2.Text == "0")) return;


             if (bt.Name == "butt_G30_1" && (txt_ETC8.Text == "" || txt_ETC8.Text == "0")) return;
             if (bt.Name == "butt_G30_2" && (txt_ETC8_2.Text == "" || txt_ETC8_2.Text == "0")) return;

             if (bt.Name == "butt_G40_1" && (txt_ETC9.Text == "" || txt_ETC9.Text == "0")) return;
             if (bt.Name == "butt_G40_2" && (txt_ETC9_2.Text == "" || txt_ETC9_2.Text == "0")) return;


             if (bt.Name == "butt_G50_1" && (txt_ETC10.Text == "" || txt_ETC10.Text == "0")) return;
             if (bt.Name == "butt_G50_2" && (txt_ETC10_2.Text == "" || txt_ETC10_2.Text == "0")) return;

             if (bt.Name == "butt_G60_1" && (txt_ETC11.Text == "" || txt_ETC11.Text == "0")) return;
             if (bt.Name == "butt_G60_2" && (txt_ETC11_2.Text == "" || txt_ETC11_2.Text == "0")) return;


             if (bt.Name == "butt_G70_N" && (txt_ETC_N_7.Text == "" || txt_ETC_N_7.Text == "0")) return;
             if (bt.Name == "butt_G70_N2" && (txt_ETC_N_7_2.Text == "" || txt_ETC_N_7_2.Text == "0")) return;

             if (bt.Name == "butt_G80_N" && (txt_ETC_N_8.Text == "" || txt_ETC_N_8.Text == "0")) return;
             if (bt.Name == "butt_G80_N2" && (txt_ETC_N_8_2.Text == "" || txt_ETC_N_8_2.Text == "0")) return;

             if (bt.Name == "butt_G90_N" && (txt_ETC_N_9.Text == "" || txt_ETC_N_9.Text == "0")) return;
             if (bt.Name == "butt_G90_N2" && (txt_ETC_N_9_2.Text == "" || txt_ETC_N_9_2.Text == "0")) return;


             if (bt.Name == "butt_G100_N" && (txt_ETC_N_10.Text == "" || txt_ETC_N_10.Text == "0")) return;
             if (bt.Name == "butt_G100_N2" && (txt_ETC_N_10_2.Text == "" || txt_ETC_N_10_2.Text == "0")) return;

             if (bt.Name == "butt_G110_N" && (txt_ETC_N_11.Text == "" || txt_ETC_N_11.Text == "0")) return;
             if (bt.Name == "butt_G110_N2" && (txt_ETC_N_11_2.Text == "" || txt_ETC_N_11_2.Text == "0")) return;

             if (bt.Name == "butt_G120_N" && (txt_ETC_N_12.Text == "" || txt_ETC_N_12.Text == "0")) return;
             if (bt.Name == "butt_G120_N2" && (txt_ETC_N_12_2.Text == "" || txt_ETC_N_12_2.Text == "0")) return;

             
             if (bt.Name == "butt_G10_1") { S_LineCnt = 1; S_Grde = 10; }
             if (bt.Name == "butt_G10_2") { S_LineCnt = 2; S_Grde = 10; }


             if (bt.Name == "butt_G20_1") { S_LineCnt = 1; S_Grde = 20; }
             if (bt.Name == "butt_G20_2") { S_LineCnt = 2; S_Grde = 20; }


             if (bt.Name == "butt_G30_1") { S_LineCnt = 1; S_Grde = 30; }
             if (bt.Name == "butt_G30_2") { S_LineCnt = 2; S_Grde = 30; }

            if (bt.Name == "butt_G40_1") { S_LineCnt = 1; S_Grde = 40; }
            if (bt.Name == "butt_G40_2") { S_LineCnt = 2; S_Grde = 40; }


            if (bt.Name == "butt_G50_1") { S_LineCnt = 1; S_Grde = 50; }
            if (bt.Name == "butt_G50_2") { S_LineCnt = 2; S_Grde = 50; }

            if (bt.Name == "butt_G60_1") { S_LineCnt = 1; S_Grde = 60; }
            if (bt.Name == "butt_G60_2") { S_LineCnt = 2; S_Grde = 60; }


            if (bt.Name == "butt_G70_N") { S_LineCnt = 1; S_Grde = 70; }
            if (bt.Name == "butt_G70_N2") { S_LineCnt = 2; S_Grde = 70; }

            if (bt.Name == "butt_G80_N") { S_LineCnt = 1; S_Grde = 80; }
            if (bt.Name == "butt_G80_N2") { S_LineCnt = 2; S_Grde = 80; }

            if (bt.Name == "butt_G90_N") { S_LineCnt = 1; S_Grde = 90; }
            if (bt.Name == "butt_G90_N2") { S_LineCnt = 2; S_Grde = 90; }

            if (bt.Name == "butt_G100_N") { S_LineCnt = 1; S_Grde = 100; }
            if (bt.Name == "butt_G100_N2") { S_LineCnt = 2; S_Grde = 100; }

            if (bt.Name == "butt_G110_N") { S_LineCnt = 1; S_Grde = 110; }
            if (bt.Name == "butt_G110_N2") { S_LineCnt = 2; S_Grde = 110; }

            if (bt.Name == "butt_G120_N") { S_LineCnt = 1; S_Grde = 120; }
            if (bt.Name == "butt_G120_N2") { S_LineCnt = 2; S_Grde = 120; }



            if (idx_Mbid == "" || idx_ToEndDate == "") return; 

            cls_Search_DB csd = new cls_Search_DB();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(idx_Mbid, ref Mbid, ref Mbid2);

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            // Real_DownGrade_Detail( Mbid, Mbid2, S_LineCnt, S_Grde, cgb_P1 );  //그룹하선 매출 내역
            Real_DownGrade_Detail_N(Mbid, Mbid2, S_LineCnt, S_Grde, cgb_P1);  //그룹하선 매출 내역
             this.Cursor = System.Windows.Forms.Cursors.Default; 

        }



        private void Real_DownGrade_Detail(string Saveid, int Saveid2, int S_LineCnt, int S_Grde, cls_Grid_Base cgb_P)
        {
            string StrSql = "";
            string ToEndDate = idx_ToEndDate;

            StrSql = "SELECT Mbid, Mbid2 FROM tbl_ClosePay_02_Mod (nolock) ";
            StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " And  Saveid = '" + Saveid + "' ";
            StrSql = StrSql + " And Saveid2 = " + Saveid2;
            StrSql = StrSql + " And LineCnt = " + S_LineCnt;

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            int row_num = 0;
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                string Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                int Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());


                StrSql = "select TMbid,TMbid2, TName,TTotalPV,TTotalPV2, lvl,pos";
                StrSql = StrSql + " From ufn_GetSubTree_Close_G_N_02('" + Mbid + "'," + Mbid2 + ",'" + ToEndDate.Replace("-", "") + "'";
                StrSql = StrSql + ") Where pos <>0 ";
                StrSql = StrSql + " And    TTotalPV = " + S_Grde;
                StrSql = StrSql + " Order by lvl Asc ";

                //++++++++++++++++++++++++++++++++

                DataSet ds3 = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Count", ds3, this.Name, this.Text) == false) return;
                int ReCnt2 = Temp_Connect.DataSet_ReCount;

                if (ReCnt2 > 0)
                {

                    for (int fi_cnt2 = 0; fi_cnt2 <= ReCnt2 - 1; fi_cnt2++)
                    {
                        object[] row0 = { ds3.Tables["tbl_Count"].Rows[fi_cnt2]["TMbid2"]
                                ,ds3.Tables["tbl_Count"].Rows[fi_cnt2]["TName"]  
                                ,ds3.Tables["tbl_Count"].Rows[fi_cnt2]["TTotalPV2"]
                                ,ds3.Tables["tbl_Count"].Rows[fi_cnt2]["lvl"]
                                ,""                                
                                    };

                        gr_dic_text[row_num + 1] = row0;
                        row_num = row_num + 1;
                    }
                }

            }


            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();

        }


        private void butt_G60_1_Click_2(object sender, EventArgs e)
        {

        }





        private void Real_DownGrade_Detail_N(string Saveid, int Saveid2, int S_LineCnt, int S_Grde, cls_Grid_Base cgb_P)
        {
            string StrSql = "";
            string ToEndDate = idx_ToEndDate;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            
            //string Mbid = Saveid;
            //int Mbid2 = Saveid2;

             StrSql = "SELECT Mbid, Mbid2 FROM tbl_ClosePay_02_Mod (nolock) ";
            StrSql = StrSql + " Where ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " And  Saveid = '" + Saveid + "' ";
            StrSql = StrSql + " And Saveid2 = " + Saveid2;
            if (S_LineCnt == 1) StrSql = StrSql + " And LineCnt = 1"  ;
            if (S_LineCnt == 2) StrSql = StrSql + " And LineCnt = 2 "  ; 
            StrSql = StrSql + " Order by Mbid2 "; 

            //++++++++++++++++++++++++++++++++
            

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            int row_num = 0;
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                string Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                int Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());

                StrSql = "select TMbid,TMbid2, TName,TTotalPV,TTotalPV2, LVL + 1  LVL  , pos";
                StrSql = StrSql + " From ufn_GetSubTree_Close_G_S_02 ('" + Mbid + "'," + Mbid2 + ",'" + ToEndDate.Replace("-", "") + "')";
                StrSql = StrSql + " Where TMbid2 in (Select TMbid2 From ufn_GetSubTree_Close_G_S_02__10 ('" + Saveid + "'," + Saveid2 + ",'" + ToEndDate.Replace("-", "") + "'))";
                //if (S_Grde == 10 ) StrSql = StrSql + " From ufn_GetSubTree_Close_G_S_02__10 ('" + Mbid + "'," + Mbid2 + ",'" + ToEndDate.Replace("-", "") + "'";
                //if (S_Grde == 20) StrSql = StrSql + " From ufn_GetSubTree_Close_G_S_02__20 ('" + Mbid + "'," + Mbid2 + ",'" + ToEndDate.Replace("-", "") + "'";
                //if (S_Grde == 30) StrSql = StrSql + " From ufn_GetSubTree_Close_G_S_02__30 ('" + Mbid + "'," + Mbid2 + ",'" + ToEndDate.Replace("-", "") + "'";
                //if (S_Grde == 40) StrSql = StrSql + " From ufn_GetSubTree_Close_G_S_02__40 ('" + Mbid + "'," + Mbid2 + ",'" + ToEndDate.Replace("-", "") + "'";
                //if (S_Grde == 50) StrSql = StrSql + " From ufn_GetSubTree_Close_G_S_02__50 ('" + Mbid + "'," + Mbid2 + ",'" + ToEndDate.Replace("-", "") + "'";
                //if (S_Grde == 60) StrSql = StrSql + " From ufn_GetSubTree_Close_G_S_02__60 ('" + Mbid + "'," + Mbid2 + ",'" + ToEndDate.Replace("-", "") + "'";
                //if (S_Grde == 70) StrSql = StrSql + " From ufn_GetSubTree_Close_G_S_02__70 ('" + Mbid + "'," + Mbid2 + ",'" + ToEndDate.Replace("-", "") + "'";
                //if (S_Grde == 80) StrSql = StrSql + " From ufn_GetSubTree_Close_G_S_02__80 ('" + Mbid + "'," + Mbid2 + ",'" + ToEndDate.Replace("-", "") + "'";
                //if (S_Grde == 90) StrSql = StrSql + " From ufn_GetSubTree_Close_G_S_02__90 ('" + Mbid + "'," + Mbid2 + ",'" + ToEndDate.Replace("-", "") + "'";
                
                StrSql = StrSql + " And    pos <>0 ";
                StrSql = StrSql + " And    TTotalPV >= " + S_Grde + "";                
                StrSql = StrSql + " And    LVL >= 0 ";
                //if (S_LineCnt == 1) StrSql = StrSql + " And S_N_LineCnt = 1";
                //if (S_LineCnt == 2) StrSql = StrSql + " And S_N_LineCnt = 2 "; 
                //StrSql = StrSql + " And    (N_Dri_GradeCnt > 0 OR TTotalPV >= " + S_Grde + ")";
                //StrSql = StrSql + " And    Nominid2 = " + Saveid2;
                StrSql = StrSql + " Order by lvl Asc, TTotalPV  ";

                //++++++++++++++++++++++++++++++++

                DataSet ds3 = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Count", ds3, this.Name, this.Text) == false) return;
                int ReCnt2 = Temp_Connect.DataSet_ReCount;

                if (ReCnt2 > 0)
                {


                    for (int fi_cnt2 = 0; fi_cnt2 <= ReCnt2 - 1; fi_cnt2++)
                    {
                        object[] row0 = { ds3.Tables["tbl_Count"].Rows[fi_cnt2]["TMbid2"]
                            ,ds3.Tables["tbl_Count"].Rows[fi_cnt2]["TName"]  
                            ,ds3.Tables["tbl_Count"].Rows[fi_cnt2]["TTotalPV2"]
                            ,ds3.Tables["tbl_Count"].Rows[fi_cnt2]["lvl"]
                            ,""                                
                                };

                        gr_dic_text[row_num + 1] = row0;
                        row_num = row_num + 1;
                    }
                }


            }

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();

        }






        private void dGridView_Grade_Header_Reset(DataGridView dGridView, cls_Grid_Base cgb_P)
        {

            cgb_P.grid_col_Count = 5;
            cgb_P.basegrid = dGridView;
            cgb_P.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_P.grid_Frozen_End_Count = 1;
            cgb_P.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"회원번호", "성명", "직급"  ,"대수" ,""                                      
                                };
            cgb_P.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 100 , 100, 100, 0
                             
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
                               ,DataGridViewContentAlignment.MiddleCenter//5      
                           
                              };
            cgb_P.grid_col_alignment = g_Alignment;

            //Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            //gr_dic_cell_format[2 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[16 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            //cgb_P.grid_cell_format = gr_dic_cell_format;

            //cgb_P.basegrid.RowHeadersVisible = false;

        }

        private void butt_Save_Click(object sender, EventArgs e)
        {
            if (radioB_Mi.Checked == false && radioB_Mi_No.Checked == false)
            {
                MessageBox.Show("미지급 구분 관련 선택 하신 내역이 없습니다."
                          + "\n" +
                         cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                radioB_Mi.Focus();
                return; 
            }

            int chk_cnt = 0, chk_0_cnt = 0 , Max_ToEndDate = 0  ;            
            for (int i = 0; i <= dGridView_Base.RowCount - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[0]).ToString() == "V")
                {
                    chk_cnt++;

                    if (Max_ToEndDate < int.Parse((dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[5]).ToString().Replace ("-",""))))
                        Max_ToEndDate = int.Parse((dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[5]).ToString().Replace("-", "")));
                }
                if (double.Parse(dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[61]).ToString()) == 0)
                {
                    chk_0_cnt++;
                }

            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            if (Max_ToEndDate > 0)
            {
                string StrSql = "";

                StrSql = "SELECT Web_V_TF ,ToEndDate, PayDate, FromEndDate  FROM tbl_CloseTotal_02 (nolock) ";
                StrSql = StrSql + " Where ToEndDate ='" + Max_ToEndDate.ToString () + "'";
                StrSql = StrSql + " Order by ToEndDate ";

                //++++++++++++++++++++++++++++++++            
                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt <= 0)
                {
                    MessageBox.Show("가마감 상태의 내역이 존재 합니다." + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    dGridCtrl_Base.Focus(); return;
                }
                else
                {
                    int Web_V_TF = int.Parse(ds.Tables[base_db_name].Rows[0]["Web_V_TF"].ToString());

                    //if (Web_V_TF == 0)
                    //{
                    //    MessageBox.Show("확정처리되지 않은 마감 내역이 존재합니다." + "\n" +
                    //    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    //    dGridView_Base.Focus(); return;
                    //}
                }

            }

            if (chk_cnt == 0) //저장할 내역이 없을을 알린다.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridCtrl_Base.Focus(); return ;
            }

            if (chk_0_cnt > 0) //저장할 내역이 없을을 알린다.
            {
                MessageBox.Show("실지급액이 0원인 내역은 미지급 처리가 불가능합니다." + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridCtrl_Base.Focus(); return ;
            }

            

            if (MessageBox.Show("적용하신 내역은 취소가 불가능 합니다. 적용하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.No) return;


            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            prB.Minimum = 0; prB.Maximum = dGridView_Base.DataRowCount;
            prB.Step = 1; prB.Value = 0;

            try
            {
                string StrSql = "", Mbid = "";                
                int Mbid2 = 0;
                cls_Search_DB csd = new cls_Search_DB();

                string Check_FLAG = "";

                //if (radioB_Mi.Checked == true) Check_FLAG = "M";
                //if (radioB_Mi_No.Checked == true) Check_FLAG = "N";


                Check_FLAG = "N";

                for (int i = 0; i < dGridView_Base.DataRowCount; i++)
                {
                    if (dGridView_Base.GetRowCellValue(i,dGridView_Base.Columns[0]).ToString() == "V")
                    {
                        string T_Mbid = dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[1]).ToString();
                        string ToEndDate = dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[5]).ToString();
                        string PayDate = dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[6]).ToString();
                        double SumAllAllowance = double.Parse(dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[63]).ToString()); 
                        ToEndDate = ToEndDate.Replace("-", "");
                        PayDate = PayDate.Replace("-", "");
                                                
                        Mbid = ""; Mbid2 = 0;
                        csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

                        if (Mbid2 > 0 ||  Mbid != "" ) 
                        {
                            StrSql = "INSERT INTO tbl_Close_Not_Pay ";
                            StrSql = StrSql + " (ToEndDate,mbid,mbid2,Close_FLAG,PayDate, Check_FLAG, SumAllAllowance, Recordid, RecordTime) ";
                            StrSql = StrSql + " Values (  ";
                            StrSql = StrSql + "'" + ToEndDate + "'";
                            StrSql = StrSql + ",'" + Mbid + "'," + Mbid2 + ",'W','" + PayDate + "'";
                            StrSql = StrSql + ",'" + Check_FLAG + "'," + SumAllAllowance + ",'" + cls_User.gid + "',Convert(varchar,getdate(),21) ";
                            StrSql = StrSql + " ) ";                            

                            Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);
                        }
                    }

                    prB.PerformStep();
                }

                tran.Commit();  
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));


            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
            }

            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default; 
                tran.Dispose();
                Temp_Connect.Close_DB();
            }

        }

        private void dGridView_Base_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if(e.Column.Caption.Equals("선택"))
            {
                if (dGridView_Base.GetRowCellValue(e.RowHandle, e.Column).ToString().Equals(string.Empty))
                    dGridView_Base.SetRowCellValue(e.RowHandle, e.Column, "V");
                else
                    dGridView_Base.SetRowCellValue(e.RowHandle, e.Column, "");

            }

        }

        private void dGridView_Base_DoubleClick_2(object sender, EventArgs e)
        {
            Clear_Pay_Detail();
            idx_Mbid = "";
            idx_ToEndDate = "";

            DXVGrid.GridView view = (DXVGrid.GridView)sender;

            if (view == null) return;


            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            DViewInfo.GridHitInfo info = view.CalcHitInfo(pt);
           
            //"회원번호", "성명", "마감_시작일"  ,"마감_종료일"  ,"지급_일자"  
            if (info.InDataRow && info.Column != view.Columns["선택"])
            {
                string T_Mbid = "", ToEndDate = "", FromEndDate = "", Ga_FLAG = "";

                T_Mbid = view.GetRowCellValue(info.RowHandle, view.Columns[1]).ToString();
                ToEndDate = view.GetRowCellValue(info.RowHandle, view.Columns[4]).ToString();
                FromEndDate = view.GetRowCellValue(info.RowHandle, view.Columns[3]).ToString();
                Ga_FLAG = view.GetRowCellValue(info.RowHandle, view.Columns[6]).ToString();

                string Max_N_LineCnt = view.GetRowCellValue(info.RowHandle, view.Columns[60]).ToString();
                ToEndDate = ToEndDate.Replace("-", "");
                FromEndDate = FromEndDate.Replace("-", "");

                idx_Mbid = T_Mbid;
                idx_ToEndDate = ToEndDate;


                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Allowance_Detail(T_Mbid, ToEndDate, Ga_FLAG);

                Pay_Detail(T_Mbid, ToEndDate, FromEndDate, Max_N_LineCnt, Ga_FLAG);
                this.Cursor = System.Windows.Forms.Cursors.Default;


            }
        }


        private void dGridView_Base_CustomDrawRowIndicator_1(object sender, DXVGrid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void butt_Excel_Pay_1_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Pay_1);
            e_f.ShowDialog();
        }
        private DataGridView e_f_Send_Export_Excel_Pay_1(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "추천매칭보너스";
            Excel_Export_From_Name = this.Name;
            return dGridView_Pay_1;
        }

        private void butt_Excel_Pay_2_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Pay_2);
            e_f.ShowDialog();
        }

        private DataGridView e_f_Send_Export_Excel_Pay_2(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "추천보너스";
            Excel_Export_From_Name = this.Name;
            return dGridView_Pay_2;
        }

        private void butt_Excel_Pay_3_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Pay_3);
            e_f.ShowDialog();
        }

        private DataGridView e_f_Send_Export_Excel_Pay_3(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "기간하선판매";
            Excel_Export_From_Name = this.Name;
            return dGridView_Pay_3;
        }

        private void butt_Excel_Pay_4_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Pay_4);
            e_f.ShowDialog();
        }


        private DataGridView e_f_Send_Export_Excel_Pay_4(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "판매역추적";
            Excel_Export_From_Name = this.Name;
            return dGridView_Pay_4;
        }

        private void butt_Excel_Pay_SP_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Pay_SP);
            e_f.ShowDialog();
        }

        private DataGridView e_f_Send_Export_Excel_Pay_SP(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "미지급관련";
            Excel_Export_From_Name = this.Name;
            return dGridView_Pay_SP;
        }

        private void butt_Excel_Pay_5_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Pay_5);
            e_f.ShowDialog();
        }

        private DataGridView e_f_Send_Export_Excel_Pay_5(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "추천역추적";
            Excel_Export_From_Name = this.Name;
            return dGridView_Pay_5;
        }

        private void butt_Excel_Pay_8_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Pay_8);
            e_f.ShowDialog();
        }

        private DataGridView e_f_Send_Export_Excel_Pay_8(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "추천매칭역추적";
            Excel_Export_From_Name = this.Name;
            return dGridView_Pay_8;
        }

        private void butt_Excel_Detail_2_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Detail_2);
            e_f.ShowDialog();
        }
        private DataGridView e_f_Send_Export_Excel_Detail_2(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "후원보너스관련하선";
            Excel_Export_From_Name = this.Name;
            return dGridView_Detail_2;
        }

        private void butt_Excel_Detail_Down_N_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Detail_Down_N);
            e_f.ShowDialog();
        }
        private DataGridView e_f_Send_Export_Excel_Detail_Down_N(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "직급관련추천하선";
            Excel_Export_From_Name = this.Name;
            return dGridView_Detail_Down_N;
        }

        private void butt_Excel_Detail_Down_S_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Detail_Down_S);
            e_f.ShowDialog();
        }
        private DataGridView e_f_Send_Export_Excel_Detail_Down_S(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "직급관련후원하선";
            Excel_Export_From_Name = this.Name;
            return dGridView_Detail_Down_S;
        }

        private void butt_Excel_Detail_3_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Detail_3);
            e_f.ShowDialog();
        }
        private DataGridView e_f_Send_Export_Excel_Detail_3(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "후원역추적";
            Excel_Export_From_Name = this.Name;
            return dGridView_Detail_3;
        }

        private void butt_Excel_Detail_4_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Detail_4);
            e_f.ShowDialog();
        }
        private DataGridView e_f_Send_Export_Excel_Detail_4(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "추천역추적";
            Excel_Export_From_Name = this.Name;
            return dGridView_Detail_4;
        }




    }
}
