using System;
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
    public partial class frmClose_4_Select_03 : Form
    {
      


        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        private const string base_db_name = "tbl_DB";
        private const string base_Closedb_name = "tbl_CloseTotal_04";
        Class.DevGridControlService cgb = new Class.DevGridControlService();
        cls_Grid_Base cgb_Sum = new cls_Grid_Base();

        private int Data_Set_Form_TF = 0;

        private int Form_Load_TF = 0;

        public frmClose_4_Select_03()
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

            cfm.button_flat_change(butt_Save);

            cfm.button_flat_change(butt_Excel_Pay_1);
            cfm.button_flat_change(butt_Excel_Pay_2);
            cfm.button_flat_change(butt_Excel_Pay_S_Down);


            



        }

        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Form_Load_TF = 0;
            Data_Set_Form_TF = 0;
            //dGridView_Base.Dock = DockStyle.Fill;
          //  panel8.Dock = DockStyle.Fill;


            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_Close_Grade_ComboBox(combo_Grade, combo_Grade_Code);
            cpbf.Put_Close_Grade_ComboBox(combo_CGrade, combo_CGrade_Code);

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


            //tab_Detail_01.TabPages.Remove(tabPage1);   //뭔수당 역추적
           // tab_Detail_01.TabPages.Remove(tabPage2);   //뭔수당 역추적
            //tab_Detail_01.TabPages.Remove(tab_Save_D); //기간하선 판매
            //tab_Detail_01.TabPages.Remove(tab_Up); //기간하선 판매
            //tab_Detail_01.TabPages.Remove(tabPage5); //기간하선 판매


            //tab_Detail_02.TabPages.Remove(tab_Dir);
            tab_Detail_02.TabPages.Remove(tabPage4);
            tab_Detail_02.TabPages.Remove(tab_etc);
            

            tab_Detail_02.Width = (this.Width / 3) * 2;

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

                mtxtMbid.Focus();
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
            combo_CGrade_Code.SelectedIndex = combo_CGrade.SelectedIndex;

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

                mtxtMbid.Focus();
            }

            else if (bt.Name == "butt_Excel")
            {
                saveFileDialog1.FileName = this.Text + "_" + DateTime.Now.ToShortDateString();
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    dGridView_Base.ExportToXlsx(saveFileDialog1.FileName);

                    cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                    string Tsql = "";
                    Tsql = "Insert Into tbl_Excel_User Values ( ";
                    Tsql = Tsql + "'" + cls_User.gid + "',Convert(Varchar(25),GetDate(),21),";
                    Tsql = Tsql + "'" + saveFileDialog1.FileName + "',";
                    Tsql = Tsql + "'') ";

                    if (Temp_Connect.Insert_Data(Tsql, "tbl_Excel_User") == false) return;

                    if (MessageBox.Show("열어보시겠습니까?", "저장이 완료되었습니다.", MessageBoxButtons.YesNo) == DialogResult.Yes)
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


        //private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        //{
        //    cls_form_Meth cm = new cls_form_Meth();
        //    Excel_Export_File_Name = this.Text; // cm._chang_base_caption_search ("주간_마감_회원별");
        //    Excel_Export_From_Name = this.Name;
        //    return dGridView_Base;
        //}


        private void Make_Base_Query(ref string Tsql)
        {


            //Tsql = "Select Case When tbl_Close_Not_Pay.Seq is not null And tbl_Close_Not_Pay.Check_FLAG = 'N' then 'V' ELSE  '' End ,  ";

            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + " tbl_ClosePay_04_Mod.mbid + '-' + Convert(Varchar,tbl_ClosePay_04_Mod.mbid2) ";
            //else
            //    Tsql = Tsql + " tbl_ClosePay_04_Mod.mbid2 ";

            //Tsql = Tsql + " ,tbl_ClosePay_04_Mod.M_Name ";
            //Tsql = Tsql + " ,0  ";
            //Tsql = Tsql + " ,LEFT(tbl_ClosePay_04_Mod.FromEndDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_04_Mod.FromEndDate,4),2) + '-' + RIGHT(tbl_ClosePay_04_Mod.FromEndDate,2) ";

            //Tsql = Tsql + " , LEFT(tbl_ClosePay_04_Mod.ToEndDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_04_Mod.ToEndDate,4),2) + '-' + RIGHT(tbl_ClosePay_04_Mod.ToEndDate,2) ";
            //Tsql = Tsql + " , LEFT(tbl_ClosePay_04_Mod.PayDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_04_Mod.PayDate,4),2) + '-' + RIGHT(tbl_ClosePay_04_Mod.PayDate,2) ";

            //Tsql = Tsql + ", Case When tbl_ClosePay_04_Mod.TruePayment > 0 then '지급' ";
            //Tsql = Tsql + " When tbl_ClosePay_04_Mod.SumAllAllowance = 0  then '미발생자' ";
            //Tsql = Tsql + " When tbl_ClosePay_04_Mod.SumAllAllowance > 0 And tbl_ClosePay_04_Mod.TruePayment = 0  then '미지급' ";
            //Tsql = Tsql + " ELSE '' End  ";


            //Tsql = Tsql + " , SellPV01 + SellPV02 + SellPV03   ";
            //Tsql = Tsql + " , SellCV01 + SellCV02 + SellCV03   ";
            //Tsql = Tsql + " , SellPrice01 + SellPrice02 + SellPrice03  ";

            ////C2 현   C1 전   C4 유지      
            //Tsql = Tsql + " ,  ISnull(C1.Grade_Name,'')";
            //Tsql = Tsql + " ,  ISnull(C2.Grade_Name,'') ";
            //Tsql = Tsql + " , ISnull(C4.Grade_Name,'')  ";

            ////W4_QV  W4_QV_Auto  W4_QV_Down
            ////ReqTF10 = 1 개별구매, ReqTF10 =2  오토쉽구매 ,  ReqTF10 = 3 직추천소비자 구매    

            //Tsql = Tsql + " ,Case When tbl_ClosePay_04_Mod.ToEndDate >= '20191001' then Cur_Down_PV_1 ELSE Down_PV_1 END";
            //Tsql = Tsql + " ,Down_PV_Re_1";
            //Tsql = Tsql + " ,Case When tbl_ClosePay_04_Mod.ToEndDate >= '20191001' then Cur_Down_PV_2 ELSE Down_PV_2 END";
            //Tsql = Tsql + " ,Down_PV_Re_2 ";
            //Tsql = Tsql + " ,N_Down_PV ";
            //Tsql = Tsql + " ,N_Down_PV_Re ";

            //Tsql = Tsql + " , N_Dir_Active_Cnt ";
            //Tsql = Tsql + " ,Dir_Cnt_G10, Dir_Cnt_G20 ";
            //Tsql = Tsql + " ,Dir_Cnt_G30 ";
            //Tsql = Tsql + " , Case When ReqTF2 =  1 then 'Y' ELSE '' End  ";
            ////Tsql = Tsql + " ,0   ";
            ////Tsql = Tsql + " ,0 Fresh_1";
            ////Tsql = Tsql + " ,0 Fresh_2 ";
            ////, "이전_좌" , "이전_우" ,"이월_좌",  "이월_우"  ,"130팩직추천수"
            //Tsql = Tsql + " ,Be_Down_PV_1,Be_Down_PV_2 ";

            //Tsql = Tsql + " ,Be_Down_PV_1 + Cur_Down_PV_1 ";


            //Tsql = Tsql + " ,Be_Down_PV_2 + Cur_Down_PV_2";
            //Tsql = Tsql + " ,Down_PV_1";   //반품처리전 실 하선PV
            //Tsql = Tsql + " ,Down_PV_2";   //반품처리전 실 하선PV
            //Tsql = Tsql + " ,ReqTF9 ";   //반품처리전 수당 적용PV
            //Tsql = Tsql + " ,Case When OneGrade > 0 And ReqTF6 = OneGrade  then  '당월' ";   //반품처리전 수당 적용PV
            //Tsql = Tsql + "    When OneGrade > 0 And ReqTF6  = 0   then  '누적' ";   //반품처리전 수당 적용PV
            //Tsql = Tsql + "    End  "; 




            //Tsql = Tsql + " ,0 Down_W4_QV_Real_1 ";
            //Tsql = Tsql + " ,0 Down_W4_QV_Real_2 ";

            //Tsql = Tsql + ", ''  ";
            //Tsql = Tsql + " ,0 W4_QV , 0 W4_QV_Auto , 0 W4_QV_Down ";
            //Tsql = Tsql + " , Case When ReqTF8 >= ReqTF7  then ISnull(C8.Grade_Name,'') eLSE ISnull(C7.Grade_Name,'') end   "; //랭크업보너스직급



            //Tsql = Tsql + " ,Etc_Pay , Allowance1 , Allowance2 , Allowance3 , Allowance4   ";

            //Tsql = Tsql + " ,Isnull( Allowance5,0) Allowance5 , Isnull( Allowance6,0) Allowance6 , Isnull( Allowance7,0) Allowance7 , Isnull( Allowance8,0) Allowance8 , Isnull( Allowance9,0) Allowance9 ";

            //Tsql = Tsql + " ,0,0  ";
            //Tsql = Tsql + " ,0 ";
            //Tsql = Tsql + ", Case When tbl_ClosePay_04_Mod.Cpno = ''  OR tbl_ClosePay_04_Mod.BankAcc = '' then tbl_ClosePay_04_Mod.SumAllAllowance +   tbl_ClosePay_04_Mod.SumAllAllowance_Be_Not ELSE 0 End AS Not_Pay_C ";
            //Tsql = Tsql + " ,tbl_ClosePay_04_Mod.SumAllAllowance_Be_Not_Sum  ";

            ////Tsql = Tsql + " ,Sum_Return_Remain_Pay_DED   ,Cur_DedCut_Pay_DED , Cur_Return_Pay_DED , Sum_Return_Take_Pay_DED  ";
            ////Isnull(tbl_ClosePay_04_Mod.Cur_DedCut_Pay_DED,0)
            //Tsql = Tsql + ", Isnull(Cur_DedCut_Pay_DED,0) Etc_Pay_DedCut   ";
            //Tsql = Tsql + ", (Isnull( Allowance1_D,0) +Isnull(  Allowance2_D,0)  +Isnull(  Allowance3_D,0)  +Isnull(  Allowance4_D,0)  +Isnull(  Allowance5_D,0) +Isnull(  Allowance6_D,0)    +Isnull(  Allowance7_D,0)  )  SumAllAllowance_Cut ";
            //Tsql = Tsql + ", Isnull(Cur_DedCut_Pay,0) Cur_DedCut_Pay ";
            //Tsql = Tsql + " , (Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6 + Allowance7 + tbl_ClosePay_04_Mod.Etc_Pay ) Cur_SumAllowance  ";
            //Tsql = Tsql + " , (Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6 + Allowance7  + tbl_ClosePay_04_Mod.Etc_Pay  - Isnull(tbl_ClosePay_04_Mod.Cur_DedCut_Pay_DED,0) - Cur_DedCut_Pay)     Cur_D_SumAllowance  ";
            //Tsql = Tsql + " , tbl_ClosePay_04_Mod.SumAllAllowance_Be_Not_Sum   Be_SumAllowance  ";
            //Tsql = Tsql + " ,  (Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6 + Allowance7 - Cur_DedCut_Pay) + tbl_ClosePay_04_Mod.SumAllAllowance_Be_Not_Sum  + tbl_ClosePay_04_Mod.Etc_Pay - Isnull(tbl_ClosePay_04_Mod.Cur_DedCut_Pay_DED,0)  SumAllAllowance";
            ////Tsql = Tsql + " ,  (Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6 + Allowance7 + Allowance8 + Allowance9 - Allowance12 + Allowance13 - Cur_DedCut_Pay)     SumAllAllowance";
            ////Tsql = Tsql + " , tbl_ClosePay_04_Mod.SumAllAllowance  ";
            //Tsql = Tsql + ", InComeTax , ResidentTax , TruePayment ";


            //Tsql = Tsql + " ,Sum_Return_Remain_Pay - Cur_Return_Pay   ";
            //Tsql = Tsql + " ,0 ";

            //Tsql = Tsql + " ,Cur_Return_Pay   ";
            //Tsql = Tsql + " ,Sum_Return_Remain_Pay - Cur_DedCut_Pay ";
            ////Tsql = Tsql + " , Max_N_LineCnt ";
            ////Tsql = Tsql + " , ISNULL(tbl_ClosePay_10000.SumAllAllowance, 0) ";
            //Tsql = Tsql + " ,  tbl_Memberinfo.hptel ,  tbl_Memberinfo.LeaveDate , tbl_Memberinfo.Addcode1 , tbl_Memberinfo.Address1 + ' ' + tbl_Memberinfo.Address2 ";

            //Tsql = Tsql + " ,tbl_Bank.bankname , tbl_ClosePay_04_Mod.bankcode ";

            //Tsql = Tsql + ", tbl_ClosePay_04_Mod.BankAcc ";
            //Tsql = Tsql + ", tbl_ClosePay_04_Mod.BankOwner ";
            //Tsql = Tsql + ", tbl_ClosePay_04_Mod.Cpno ";

            //Tsql = Tsql + ", isnull(tbl_Business.Name,'') AS bname , tbl_ClosePay_04_Mod.Remarks1 ";

            //Tsql = Tsql + ", Case ";
            //Tsql = Tsql + "  When tbl_Close_Not_Pay.Check_FLAG = 'N' then '회원별화면'";
            //Tsql = Tsql + "  When tbl_Close_Not_Pay.Check_FLAG = 'M' then '직접입력'";
            //Tsql = Tsql + "  ELSE ''";
            //Tsql = Tsql + "  End ";

            //Tsql = Tsql + " From tbl_ClosePay_04_Mod (nolock) ";

            ////Tsql = Tsql + " LEFT Join tbl_ClosePay_04_Sell_Mod  (nolock) On tbl_ClosePay_04_Mod.mbid=tbl_ClosePay_04_Sell_Mod.mbid " ;
            ////Tsql = Tsql + " And tbl_ClosePay_04_Mod.mbid2=tbl_ClosePay_04_Sell_Mod.mbid2";
            ////Tsql = Tsql + " And tbl_ClosePay_04_Mod.ToEndDate=tbl_ClosePay_04_Sell_Mod.ToEndDate";
            ////Tsql = Tsql + " And tbl_ClosePay_04_Sell_Mod.SellCode ='01' ";

            //Tsql = Tsql + " LEFT Join tbl_Memberinfo  (nolock) On tbl_ClosePay_04_Mod.mbid=tbl_Memberinfo.mbid ";
            //Tsql = Tsql + " And tbl_ClosePay_04_Mod.mbid2=tbl_Memberinfo.mbid2";

            //Tsql = Tsql + " Left Join tbl_Business  (nolock) On tbl_Memberinfo.businesscode=tbl_Business.ncode And tbl_Memberinfo.Na_code = tbl_Business.Na_code";
            //Tsql = Tsql + " Left Join tbl_Bank  (nolock) On tbl_ClosePay_04_Mod.bankcode=tbl_Bank.ncode  ";

            ////C2 현   C1 전   C4 유지
            //Tsql = Tsql + " Left Join tbl_Class C2  (nolock) On tbl_ClosePay_04_Mod.CurGrade=C2.Grade_Cnt ";              
            //Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On tbl_ClosePay_04_Mod.BeforeGrade = C1.Grade_Cnt ";
            //Tsql = Tsql + " Left Join tbl_Class C4  (nolock) On tbl_ClosePay_04_Mod.OneGrade = C4.Grade_Cnt ";

            //Tsql = Tsql + " Left Join tbl_Class C7  (nolock) On tbl_ClosePay_04_Mod.ReqTF7 = C7.Grade_Cnt ";
            //Tsql = Tsql + " Left Join tbl_Class C8  (nolock) On tbl_ClosePay_04_Mod.ReqTF8 = C8.Grade_Cnt ";


            ////Tsql = Tsql + " Left Join tbl_ClosePay_10000 (nolock) on tbl_ClosePay_04_Mod.mbid = tbl_ClosePay_10000.mbid And tbl_ClosePay_04_Mod.mbid2 = tbl_ClosePay_10000.mbid2 and tbl_ClosePay_04_Mod.ToEndDate = tbl_ClosePay_10000.ToEndDate And tbl_ClosePay_10000.ToEndDate_TF = 2 ";
            //Tsql = Tsql + " Left Join tbl_Close_Not_Pay (nolock ) on tbl_ClosePay_04_Mod.mbid = tbl_Close_Not_Pay.mbid And tbl_ClosePay_04_Mod.mbid2 = tbl_Close_Not_Pay.mbid2 and tbl_ClosePay_04_Mod.ToEndDate = tbl_Close_Not_Pay.ToEndDate And tbl_Close_Not_Pay.Close_FLAG = 'W'  ";




            //Tsql = "Select Case When tbl_Close_Not_Pay.Seq is not null And tbl_Close_Not_Pay.Check_FLAG = 'N' then 'V' ELSE  '' End ,  ";
            Tsql = "Select '' ,  ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " tbl_ClosePay_04_Mod.mbid + '-' + Convert(Varchar,tbl_ClosePay_04_Mod.mbid2) ";
            else
                Tsql = Tsql + " tbl_ClosePay_04_Mod.mbid2 ";
            Tsql = Tsql + " ,tbl_ClosePay_04_Mod.M_Name ";            
            Tsql = Tsql + " ,LEFT(tbl_ClosePay_04_Mod.FromEndDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_04_Mod.FromEndDate,4),2) + '-' + RIGHT(tbl_ClosePay_04_Mod.FromEndDate,2) ";
            Tsql = Tsql + " , LEFT(tbl_ClosePay_04_Mod.ToEndDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_04_Mod.ToEndDate,4),2) + '-' + RIGHT(tbl_ClosePay_04_Mod.ToEndDate,2) ";


            Tsql = Tsql + " , LEFT(tbl_ClosePay_04_Mod.PayDate,4) +'-' + LEFT(RIGHT(tbl_ClosePay_04_Mod.PayDate,4),2) + '-' + RIGHT(tbl_ClosePay_04_Mod.PayDate,2) ";
            Tsql = Tsql + " , Case When tbl_ClosePay_04_Mod.TruePayment > 0 then '지급' ";
            Tsql = Tsql + " When tbl_ClosePay_04_Mod.SumAllAllowance = 0  then '미발생자' ";
            Tsql = Tsql + " When tbl_ClosePay_04_Mod.SumAllAllowance > 0 And tbl_ClosePay_04_Mod.TruePayment = 0  then '미지급' ";
            Tsql = Tsql + " ELSE '' End  ";
            Tsql = Tsql + " , SellPV01 + SellPV02 + SellPV03   ";
            Tsql = Tsql + " , SellCV01 + SellCV02 + SellCV03   ";
            Tsql = Tsql + " , Self_M_PV + Self_M_Dir_PV   ";  // PPV
            

            Tsql = Tsql + ", isnull(tbl_Business.Name,'') AS bname  ";
            //C2 최고   C1    C4 현달직급      
            Tsql = Tsql + " , ISnull(C1.Grade_Name,'')";  //최고직급
            Tsql = Tsql + " , Case When MM_Up.ReqTF2 =  1 then '유'   ELSE '' End  "; // 개인유자격여부 (직급승급 관련 150)
            Tsql = Tsql + " , Case When ISnull(C5.Grade_Name,'')  <> '' then ISnull(C5.Grade_Name,'')  else  ISnull(C4.Grade_Name,'') end  ";      //현달직급
            Tsql = Tsql + " , Down_PV_S";  // DPV
            

            Tsql = Tsql + " , MM_Up.Down_PV_Limt";  // DPV 최대레그의 상한            
            Tsql = Tsql + " , MM_Up.Down_Active_Cnt";   //액티브레그수            
            Tsql = Tsql + " , Isnull(Up_Down_Point.Leader_P,0) "; //리더레그포인트
            Tsql = Tsql + " , Isnull(Up_Down_Point.A_Cnt_80_Line,0) "; //GED레그수
            Tsql = Tsql + " , Isnull(Up_Down_Point.A_Cnt_90_Line,0) "; //PD레스수


            Tsql = Tsql + " , M6_Grade_Cnt_80 , M6_Grade_Cnt_90 ";  //6개월 GED달성횟수, PD 달성횟수
            Tsql = Tsql + " , 0 , 0 , 0 ";
            Tsql = Tsql + " , 0 , 0 , 0 , 0 , 0 ";

            Tsql = Tsql + " , 0 , 0 , 0 , 0 , 0 ";
            Tsql = Tsql + " , 0 , 0 , 0 , 0 , 0 ";
                        

            Tsql = Tsql + " ,Etc_Pay , Allowance1 , Allowance2 , Allowance3 , Allowance4   ";

            Tsql = Tsql + " ,Isnull( Allowance5,0) Allowance5 , Isnull( Allowance6,0) Allowance6 , Isnull( Allowance7,0) Allowance7 , Isnull( Allowance8,0) Allowance8 , Isnull( Allowance9,0) Allowance9 ";

            Tsql = Tsql + " ,Leg_Limit_Cut,0  ";
            Tsql = Tsql + " ,0 ";
            Tsql = Tsql + ", Case When tbl_ClosePay_04_Mod.Cpno = ''  OR tbl_ClosePay_04_Mod.BankAcc = '' then tbl_ClosePay_04_Mod.SumAllAllowance +   tbl_ClosePay_04_Mod.SumAllAllowance_Be_Not ELSE 0 End AS Not_Pay_C "; //마감미지급액
            Tsql = Tsql + " ,tbl_ClosePay_04_Mod.SumAllAllowance_Be_Not_Sum  "; //전_마감미지급액


           Tsql = Tsql + ", 0 ";
            Tsql = Tsql + " , (Isnull( Allowance1_D,0) +Isnull(  Allowance2_D,0)  +Isnull(  Allowance3_D,0)  +Isnull(  Allowance4_D,0)  +Isnull(  Allowance5_D,0) +Isnull(  Allowance6_D,0)    +Isnull(  Allowance7_D,0)  +Isnull(  Allowance8_D,0)  )   SumAllAllowance_Cut "; //cap공제
            Tsql = Tsql + " , Isnull(Cur_DedCut_Pay,0) Cur_DedCut_Pay ";  // 반품공제
            Tsql = Tsql + " , (Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6 + Allowance7 + Allowance8  -  Leg_Limit_Cut  + tbl_ClosePay_04_Mod.Etc_Pay ) Cur_SumAllowance  ";
            Tsql = Tsql + " , (Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6 + Allowance7 + Allowance8 -  Leg_Limit_Cut + tbl_ClosePay_04_Mod.Etc_Pay  - Cur_DedCut_Pay)     Cur_D_SumAllowance  ";
            Tsql = Tsql + " , tbl_ClosePay_04_Mod.SumAllAllowance_Be_Not_Sum   Be_SumAllowance  ";
            Tsql = Tsql + " , (Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6 + Allowance7 + Allowance8 -  Leg_Limit_Cut- Cur_DedCut_Pay) + tbl_ClosePay_04_Mod.SumAllAllowance_Be_Not_Sum  + tbl_ClosePay_04_Mod.Etc_Pay   SumAllAllowance";

            Tsql = Tsql + ", InComeTax , ResidentTax , TruePayment ";


            Tsql = Tsql + " ,0   ";
            Tsql = Tsql + " ,0 ";

            Tsql = Tsql + " ,0   ";
            Tsql = Tsql + " ,Sum_Return_Remain_Pay - Cur_DedCut_Pay ";
            Tsql = Tsql + " ,  tbl_Memberinfo.hptel ,  tbl_Memberinfo.LeaveDate , tbl_Memberinfo.Addcode1 , tbl_Memberinfo.Address1 + ' ' + tbl_Memberinfo.Address2 ";

            Tsql = Tsql + " ,tbl_Bank.bankname , tbl_ClosePay_04_Mod.bankcode ";

            Tsql = Tsql + ", tbl_ClosePay_04_Mod.BankAcc ";
            Tsql = Tsql + ", tbl_ClosePay_04_Mod.BankOwner ";
            Tsql = Tsql + ", tbl_ClosePay_04_Mod.Cpno ";

            Tsql = Tsql + ", isnull(tbl_Business.Name,'') AS bname , tbl_ClosePay_04_Mod.Remarks1 ";
            Tsql = Tsql + ", ''";

            //Tsql = Tsql + ", Case ";
            //Tsql = Tsql + "  When tbl_Close_Not_Pay.Check_FLAG = 'N' then '회원별화면'";
            //Tsql = Tsql + "  When tbl_Close_Not_Pay.Check_FLAG = 'M' then '직접입력'";
            //Tsql = Tsql + "  ELSE ''";
            //Tsql = Tsql + "  End ";

            Tsql = Tsql + " From tbl_ClosePay_04_Mod (nolock) ";

            //Tsql = Tsql + " LEFT Join tbl_ClosePay_04_Sell_Mod  (nolock) On tbl_ClosePay_04_Mod.mbid=tbl_ClosePay_04_Sell_Mod.mbid " ;
            //Tsql = Tsql + " And tbl_ClosePay_04_Mod.mbid2=tbl_ClosePay_04_Sell_Mod.mbid2";
            //Tsql = Tsql + " And tbl_ClosePay_04_Mod.ToEndDate=tbl_ClosePay_04_Sell_Mod.ToEndDate";
            //Tsql = Tsql + " And tbl_ClosePay_04_Sell_Mod.SellCode ='01' ";

            Tsql = Tsql + " LEFT Join tbl_Memberinfo  (nolock) On tbl_ClosePay_04_Mod.mbid=tbl_Memberinfo.mbid ";
            Tsql = Tsql + "                                       And tbl_ClosePay_04_Mod.mbid2=tbl_Memberinfo.mbid2";

            Tsql = Tsql + " LEFT Join tbl_ClosePay_04_Up_Mod  (nolock)  MM_Up On tbl_ClosePay_04_Mod.ToEndDate = MM_Up.ToEndDate And  tbl_ClosePay_04_Mod.mbid=MM_Up.mbid ";
            Tsql = Tsql + "                                                    And tbl_ClosePay_04_Mod.mbid2=MM_Up.mbid2";


            Tsql = Tsql + " LEFT Join  (Select ToEndDate ,  Saveid2     , Sum(	Case   ";

            Tsql = Tsql + "   When OneGrade = 180  OR Cur_Leader_Leg_P_180 > 0  then 3 ";
            Tsql = Tsql + "   When OneGrade = 170 OR Cur_Leader_Leg_P_170 > 0  then 3 ";
            Tsql = Tsql + "   When OneGrade = 160 OR Cur_Leader_Leg_P_160 > 0  then 3 ";
            Tsql = Tsql + "   When OneGrade = 150 OR Cur_Leader_Leg_P_150 > 0  then 3 ";
            Tsql = Tsql + "   When OneGrade = 140 OR Cur_Leader_Leg_P_140 > 0  then 3 ";
            Tsql = Tsql + "   When OneGrade = 130 OR Cur_Leader_Leg_P_130 > 0  then 3 ";
            Tsql = Tsql + "   When OneGrade = 120 OR Cur_Leader_Leg_P_120 > 0  then 2.5 ";
            Tsql = Tsql + "   When OneGrade = 110 OR Cur_Leader_Leg_P_110 > 0  then 2 ";
            Tsql = Tsql + "   When OneGrade = 100 OR Cur_Leader_Leg_P_100 > 0  then 1.5 ";
            Tsql = Tsql + "   When OneGrade = 90 OR Cur_Leader_Leg_P_90 > 0  then 1 ";
            Tsql = Tsql + "   When OneGrade = 80 OR Cur_Leader_Leg_P_80 > 0  then 0.5 ";
            Tsql = Tsql + "                  END  ";
            Tsql = Tsql + "      			)  Leader_P  ";

            Tsql = Tsql + "   , Sum(Case When Grade_Cnt_80 + Grade_Cnt_90 >= 1 OR OneGrade >= 80  then 1  ELSE 0   END )  A_Cnt_80_Line ";
            Tsql = Tsql + "   , Sum(Case When Grade_Cnt_90 >= 1 OR OneGrade >= 90  then 1  ELSE 0   END )  A_Cnt_90_Line ";
            Tsql = Tsql + "     From tbl_ClosePay_04_Up_Mod (nolock) ";
            Tsql = Tsql + "     Group by  ToEndDate , Saveid2 ";
            Tsql = Tsql + "      )  ";
            Tsql = Tsql + "   AS    Up_Down_Point ";
            Tsql = Tsql + "                                     On tbl_ClosePay_04_Mod.ToEndDate = Up_Down_Point.ToEndDate And  tbl_ClosePay_04_Mod.mbid2 = Up_Down_Point.Saveid2 ";            


            Tsql = Tsql + " Left Join tbl_Business  (nolock) On tbl_Memberinfo.businesscode=tbl_Business.ncode And tbl_Memberinfo.Na_code = tbl_Business.Na_code";
            Tsql = Tsql + " Left Join tbl_Bank  (nolock) On tbl_ClosePay_04_Mod.bankcode=tbl_Bank.ncode  ";

            //C2 현   C1 전   C4 유지
            Tsql = Tsql + " Left Join tbl_Class C2  (nolock) On MM_Up.CurGrade=C2.Grade_Cnt ";
            Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On MM_Up.BeforeGrade = C1.Grade_Cnt ";
            Tsql = Tsql + " Left Join tbl_Class C4  (nolock) On MM_Up.OneGrade = C4.Grade_Cnt ";
            Tsql = Tsql + " Left Join tbl_Class C5  (nolock) On tbl_ClosePay_04_Mod.OneGrade = C5.Grade_Cnt ";

            //Tsql = Tsql + " Left Join tbl_ClosePay_10000 (nolock) on tbl_ClosePay_04_Mod.mbid = tbl_ClosePay_10000.mbid And tbl_ClosePay_04_Mod.mbid2 = tbl_ClosePay_10000.mbid2 and tbl_ClosePay_04_Mod.ToEndDate = tbl_ClosePay_10000.ToEndDate And tbl_ClosePay_10000.ToEndDate_TF = 2 ";
            //Tsql = Tsql + " Left Join tbl_Close_Not_Pay (nolock ) on tbl_ClosePay_04_Mod.mbid = tbl_Close_Not_Pay.mbid And tbl_ClosePay_04_Mod.mbid2 = tbl_Close_Not_Pay.mbid2 and tbl_ClosePay_04_Mod.ToEndDate = tbl_Close_Not_Pay.ToEndDate And tbl_Close_Not_Pay.Close_FLAG = 'W'  ";



        }



        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = " Where tbl_ClosePay_04_Mod.ToEndDate >= '20180701'  ";

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
                        strSql = strSql + " And tbl_ClosePay_04_Mod.Mbid ='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_ClosePay_04_Mod.Mbid2 = " + Mbid2;
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
                        strSql = strSql + " And tbl_ClosePay_04_Mod.Mbid >='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_ClosePay_04_Mod.Mbid2 >= " + Mbid2;
                }

                if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And tbl_ClosePay_04_Mod.Mbid <='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_ClosePay_04_Mod.Mbid2 <= " + Mbid2;
                }
            }


            //회원명으로 검색
            if (txtName.Text.Trim() != "")
                strSql = strSql + " And tbl_ClosePay_04_Mod.M_Name Like '%" + txtName.Text.Trim() + "%'";


            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";

            //if (txt_Us_num.Text.Trim() != "")
            //    strSql = strSql + " And tbl_Memberinfo.Us_NUM = " + txt_Us_num.Text.Trim();
            


           if ((mtxtFromDate1.Text.Replace("-", "").Trim() != "") && (mtxtFromDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_ClosePay_04_Mod.FromEndDAte = '" + mtxtFromDate1.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtFromDate1.Text.Replace("-", "").Trim() != "") && (mtxtFromDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_ClosePay_04_Mod.FromEndDAte >= '" + mtxtFromDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_ClosePay_04_Mod.FromEndDate <= '" + mtxtFromDate2.Text.Replace("-", "").Trim() + "'";
            }


            //기록일자로 검색 -1
            if ((mtxtToDate1.Text.Replace("-", "").Trim() != "") && (mtxtToDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_ClosePay_04_Mod.ToEndDate = '" + mtxtToDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtToDate1.Text.Replace("-", "").Trim() != "") && (mtxtToDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_ClosePay_04_Mod.ToEndDate >= '" + mtxtToDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_ClosePay_04_Mod.ToEndDate <= '" + mtxtToDate2.Text.Replace("-", "").Trim() + "'";
            }


            //기록일자로 검색 -1
            if ((mtxtPayDate1.Text.Replace("-", "").Trim() != "") && (mtxtPayDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_ClosePay_04_Mod.PayDate = '" + mtxtPayDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtPayDate1.Text.Replace("-", "").Trim() != "") && (mtxtPayDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_ClosePay_04_Mod.PayDate >= '" + mtxtPayDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_ClosePay_04_Mod.PayDate <= '" + mtxtPayDate2.Text.Replace("-", "").Trim() + "'";
            }


            if (txtToEndDate_Code.Text != "")
                strSql = strSql + " And tbl_ClosePay_04_Mod.ToEndDate = '" + txtToEndDate.Text + "'";

            if (combo_Grade_Code.Text != "")
                strSql = strSql + " And tbl_ClosePay_04_Mod.CurGrade = " + combo_Grade_Code.Text;

            if (combo_CGrade_Code.Text != "")
                strSql = strSql + " And tbl_ClosePay_04_Mod.CurGrade = " + combo_CGrade_Code.Text;




            if (radioB_Leave_Not.Checked == true)
                strSql = strSql + " And tbl_ClosePay_04_Mod.LeaveDate = '' ";

            if (radioB_Leave.Checked == true)
                strSql = strSql + " And tbl_ClosePay_04_Mod.LeaveDate <> '' ";


            if (radioB_Su.Checked == true)
                strSql = strSql + " And tbl_ClosePay_04_Mod.Cpno <> ''  ";

            if (radioB_Su_Not.Checked == true)
                strSql = strSql + " And tbl_ClosePay_04_Mod.Cpno = '' ";







            if (radio_PayTF1.Checked == true)
                strSql = strSql + " And tbl_ClosePay_04_Mod.TruePayment > 0  ";

            if (radio_PayTF3.Checked == true)
                strSql = strSql + " And tbl_ClosePay_04_Mod.SumAllAllowance = 0  ";

            if (radio_PayTF_Not.Checked == true)
                strSql = strSql + " And tbl_ClosePay_04_Mod.SumAllAllowance > 0 And tbl_ClosePay_04_Mod.TruePayment = 0   ";

            if (radio_PayTF_ALL.Checked == true)
                strSql = strSql + " And (Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6 + Allowance7 -  Leg_Limit_Cut - Cur_DedCut_Pay) + SumAllAllowance_Be_Not > 0  ";


            if (radio_PayTF_Re_D_1.Checked == true)
                strSql = strSql + " And tbl_ClosePay_04_Mod.Sum_Return_Remain_Pay - Cur_DedCut_Pay > 0  ";

            if (radio_PayTF_Re_D_2.Checked == true)
                strSql = strSql + " And tbl_ClosePay_04_Mod.Sum_Return_Remain_Pay  > 0  ";



            if (checkB_Up.Checked == true)
            {
                strSql = strSql + " And tbl_ClosePay_04_Up_Mod.BeforeGrade <  tbl_ClosePay_04_Up_Mod.CurGrade ";
                strSql = strSql + " And tbl_ClosePay_04_Up_Mod.CurGrade >= 10  ";


                //if (combo_Grade2_Code.Text != "")
                //    strSql = strSql + " And tbl_ClosePay_04_Up_Mod.CurGrade = " + combo_Grade2_Code.Text ;

                int C_TF = 0;
                if (checkB_10.Checked == true)
                {
                    strSql = strSql + " And (tbl_ClosePay_04_Up_Mod.CurGrade = 10 ";
                    C_TF++;
                }

                if (checkB_20.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 20 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 20 ";
                    C_TF++;
                }

                if (checkB_30.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 30 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 30 ";
                    C_TF++;
                }

                if (checkB_40.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 40 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 40 ";
                    C_TF++;
                }

                if (checkB_50.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 50 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 50 ";
                    C_TF++;
                }

                if (checkB_60.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 60 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 60 ";
                    C_TF++;
                }

                if (checkB_70.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 70 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 70 ";
                    C_TF++;
                }

                if (checkB_80.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 80 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 80 ";
                    C_TF++;
                }

                if (checkB_90.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 90 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 90 ";
                    C_TF++;
                }

                if (checkB_100.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 100 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 100 ";
                    C_TF++;
                }

                if (checkB_110.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 110 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 110 ";
                    C_TF++;
                }

                if (checkB_120.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 120 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 120 ";
                    C_TF++;
                }


                if (checkB_130.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 130 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 130 ";
                    C_TF++;
                }

                if (checkB_140.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 140 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 140 ";
                    C_TF++;
                }

                if (checkB_150.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 150 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 150 ";
                    C_TF++;
                }

                if (checkB_160.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 160 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 160 ";
                    C_TF++;
                }

                if (checkB_170.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 170 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 170 ";
                    C_TF++;
                }

                if (checkB_180.Checked == true)
                {
                    if (C_TF > 0) strSql = strSql + " OR tbl_ClosePay_04_Up_Mod.CurGrade = 180 ";
                    if (C_TF == 0) strSql = strSql + " And (  tbl_ClosePay_04_Up_Mod.CurGrade = 180 ";
                    C_TF++;
                }

                if (C_TF > 0)
                    strSql = strSql + " ) ";

            }




            //if (radio_PayTF_E.Checked == true)
            //{
            //    strSql = strSql + " And tbl_ClosePay_04_Mod.GradeDate5 >= tbl_ClosePay_04_Mod.FromEndDate  And tbl_ClosePay_04_Mod.GradeDate5 <= tbl_ClosePay_04_Mod.ToEndDate  ";
            //    strSql = strSql + " And Datediff(Day,tbl_ClosePay_04_Mod.RegTime , tbl_ClosePay_04_Mod.GradeDate5 )<= 30 "; 
            //}

            //if (checkB_E_1.Checked == true)
            //{
            //    strSql = strSql + " And tbl_ClosePay_04_Mod.GradeDate5 >= tbl_ClosePay_04_Mod.FromEndDate  And tbl_ClosePay_04_Mod.GradeDate5 <= tbl_ClosePay_04_Mod.ToEndDate  ";                
            //}

            //if (checkB_E_30.Checked == true)
            //{
            //    strSql = strSql + " And tbl_ClosePay_04_Mod.GradeDate5 >= tbl_ClosePay_04_Mod.FromEndDate  And tbl_ClosePay_04_Mod.GradeDate5 <= tbl_ClosePay_04_Mod.ToEndDate  ";
            //    strSql = strSql + " And Datediff(Day,tbl_ClosePay_04_Mod.RegTime , tbl_ClosePay_04_Mod.GradeDate5 ) <= 30 "; 
            //}

            //if (checkB_E_31.Checked == true)
            //{
            //    strSql = strSql + " And tbl_ClosePay_04_Mod.GradeDate5 >= tbl_ClosePay_04_Mod.FromEndDate  And tbl_ClosePay_04_Mod.GradeDate5 <= tbl_ClosePay_04_Mod.ToEndDate  ";
            //    strSql = strSql + " And Datediff(Day,tbl_ClosePay_04_Mod.RegTime , tbl_ClosePay_04_Mod.GradeDate5 ) > 30 ";
            //}



            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


         


            Tsql = Tsql + strSql;
            Tsql = Tsql + " Order by tbl_ClosePay_04_Mod.ToEndDAte DESC , tbl_ClosePay_04_Mod.Mbid, tbl_ClosePay_04_Mod.Mbid2 ";            
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

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                row[75] = encrypter.Decrypt(row[75].ToString());
                row[77] = encrypter.Decrypt(row[77].ToString(), "Cpno");

            }

            if (ds.Tables[0].Rows.Count >= 1000) cgb.baseview.IndicatorWidth = 45;
            if (ds.Tables[0].Rows.Count >= 10000) cgb.baseview.IndicatorWidth = 55;

            cgb.FillGrid(ds.Tables[0]);



            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            //{
            //    Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            //}
            //
            ////Reset_Chart_Total(Sum_13, Sum_14, Sum_15, Sum_17);
            ////Reset_Chart_Total(ref SelType_1);
            ////Reset_Chart_Total(Sell_Cnt_1, Sell_Cnt_2);


            //foreach (string tkey in Center_Pr.Keys)
            //{
            //    Push_data(series_Item, tkey, Center_Pr[tkey]);
            //}


            if (ReCnt > 0)
            {
                put_Sum_Dataview(ds, ReCnt);                
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }


        private void dGridView_Base_Header_Reset()
        {

            cgb.grid_col_Count = 81;
            cgb.basegrid = dGridCtrl_Base;
            cgb.baseview = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 3;
            //cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            /// Tsql = Tsql + " , Self_M_PV + Self_M_Dir_PV   ";  // PPV

            //Tsql = Tsql + ", isnull(tbl_Business.Name,'') AS bname  ";

            ////C2 최고   C1    C4 현달직급      
            //Tsql = Tsql + " ,  ISnull(C1.Grade_Name,'')";  //최고직급
            //Tsql = Tsql + " , Case When MM_Up.ReqTF2 = '유'  ELSE '' End  "; // 개인유자격여부 (직급승급 관련 150)
            //Tsql = Tsql + " , ISnull(C4.Grade_Name,'')  ";      //현달직급

            //Tsql = Tsql + " ,Down_PV_S";  // DPV
            //Tsql = Tsql + " ,Down_PV_Limt";  // DPV 최대레그의 상한

            //Tsql = Tsql + " ,Down_Active_Cnt";   //액티브레그수

            //Tsql = Tsql + " ,Isnull(Up_Down_Point.Leader_P,0) "; //리더레그포인트

            //Tsql = Tsql + " ,Isnull(Up_Down_Point.A_Cnt_80_Line,0) "; //GED레그수
            //Tsql = Tsql + " ,Isnull(Up_Down_Point.A_Cnt_90_Line,0) "; //PD레스수

            //6개월 GED달성횟수, PD 달성횟수/
            string[] g_HeaderText = {"_선택","회원번호", "성명", "마감_시작일","마감_종료일"
                                  ,"지급_일자" ,"지급구분"    ,"_총매출PV"  , "_총매출CV"  , "PPV"

                                , "등록센타"  ,   "최고직급", "개인유자격여부"   , "현달직급"  ,"DPV"
                                ,"최대레그의_상한_DPV" ,"액티브레그수" ,"리더레그포인트"  , "GED레그수", "PD레스수"


                                , "6개월_GED달성횟수" , "6개월_PD 달성횟수" , "_3" ,"_4"  , "_5"
                                , "_6" , "_7" ,"_8",  "_9"  ,"_10"

                                ,"_11", "_12","_13",  "_14" ,"_15"
                                ,"_16"    , "_17"   , "_18" , "_19" , "_20"

                                , "기타보너스" ,"첫팩주문보너스"   , "멘토보너스" , "비즈니스개발보너스"     , "유니레벨보너스"    
                                , "사이드볼륨인피니티보너스"   , "리더체크매치보너스" , "랭크업보너스"     , "글로벌풀보너스"    , "_22"
                                , "레그별보너스최대금액적용_차감"   , "_24"      , "_25"    , "마감미지급액"   , "전_마감미지급액" 
                                  , "_기타공제" ,  "Cap공제"     , "반품공제액"  ,"발생_당월수당합" ,"차감된_반품공제_기타공제포함합"


                              , "이월수당합"  , "지급수당합" , "소득세"   , "주민세"  , "실지급액"
                                , "_24", "_23"   , "_25"  , "이월한_차감할_반품공제액"   , "연락처1"

                                 , "탈퇴일자"     , "우편번호"    , "주소"     ,"은행명" ,   "은행코드"
                                ,   "계좌번호" ,"예금주",  "주민번호","_센타" , "비고" 
                                ,"_구분"
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            string[] g_Cols  =   {"_선택","회원번호", "성명", "마감_시작일","마감_종료일"
                                  ,"지급_일자" ,"지급구분"    ,"_총매출PV"  , "_총매출CV"  , "PPV"

                                , "등록센타"  ,   "최고직급", "개인유자격여부"   , "현달직급"  ,"DPV"
                                ,"최대레그의_상한_DPV" ,"액티브레그수" ,"리더레그포인트"  , "GED레그수", "PD레스수"


                               , "6개월_GED달성횟수" , "6개월_PD 달성횟수"  , "_3" ,"_4"  , "_5"
                                , "_6" , "_7" ,"_8",  "_9"  ,"_10"

                                ,"_11", "_12","_13",  "_14" ,"_15"
                                ,"_16"    , "_17"   , "_18" , "_19" , "_20"

                                , "기타보너스" ,"첫팩주문보너스"   , "멘토보너스" , "비즈니스개발보너스"     , "유니레벨보너스"
                                , "사이드볼륨인피니티보너스"   , "리더체크매치보너스" , "랭크업보너스"     , "글로벌풀보너스"    , "_22"
                                , "레그별보너스최대금액적용_차감"   , "_24"      , "_25"    , "마감미지급액"   , "전_마감미지급액"
                                  , "_기타공제" ,  "Cap공제"     , "반품공제액"  ,"발생_당월수당합" ,"차감된_반품공제_기타공제포함합"


                              , "이월수당합"  , "지급수당합" , "소득세"   , "주민세"  , "실지급액"
                               , "_24", "_23"   , "_25" , "이월한_차감할_반품공제액"   , "연락처1"

                                 , "탈퇴일자"     , "우편번호"    , "주소"     ,"은행명" ,   "은행코드"
                                ,   "계좌번호" ,"예금주",  "주민번호","_센타" , "비고"
                                ,"_구분"
                                    };

            cgb.grid_col_name = g_Cols;

            int[] g_Width = { 0, 100 , 100, 100, 100
                            , 100, 80   ,0, 0, 80

                            , 110 , 100  ,100 , 100, 120
                             , 110, 110,120 , 120 , 120
                             
                             , 130 , 130 , 0 , 0 , 0
                               , 0 , 0 , 0 , 0 , 0

                              , 0 , 0 , 0 , 0 , 0
                          , 0 , 0 , 0 , 0 , 0

                             , 100, 100,100 , 100, 100 
                             , 130, 100,100 , 0, 0
                             , 150, 0, 0, 100 , 110
                           ,0 , 80, 100, 110, 185

                            , 80   , 90 , 80, 80,80
                            , 0, 0 ,0, 175, 130

                             ,100 , 80, 300 , 100 , 90
                             ,150 , 100 , 120 , 0, 100
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
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true , true,  true
                                    ,true , true,  true , true,  true
                                    ,  true
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
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight//10

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

                                ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //25  


                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  //30   

                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
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
                               ,DataGridViewContentAlignment.MiddleRight  //45

                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight //50

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

                               ,DataGridViewContentAlignment.MiddleLeft
                              };
            cgb.grid_col_alignment = g_Alignment;

            //Usp_Close_Pro_Give_Allowance1_Real
            //Usp_Close_Pro_Give_Allowance2
            //Usp_Close_Pro_Give_Allowance3
            //Usp_Close_Pro_Put_Return_Pay_1

            string T_str_Grid_Currency_Type = "###,###,##0.00";
            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();

            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[16 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[17 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            gr_dic_cell_format[18 - 1] = T_str_Grid_Currency_Type;


            gr_dic_cell_format[19 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[20 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            //gr_dic_cell_format[21 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[22 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[23 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[24 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[25 - 1] = cls_app_static_var.str_Grid_Currency_Type;

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
            gr_dic_cell_format[46 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[47 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[48 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[49 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[50 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[51 - 1] = cls_app_static_var.str_Grid_Currency_Type;
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

            cgb.grid_cell_format = gr_dic_cell_format;

        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {

            int Col_Cnt = 0;
            object[] row0 = new object[cgb.grid_col_Count];

            while (Col_Cnt < cgb.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                if (Col_Cnt == 75)
                    row0[Col_Cnt] = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt].ToString());
                else if (Col_Cnt == 77)
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
                        
            
            Sum_dic["첫팩주문보너스"] = 0;
            Sum_dic["멘토보너스"] = 0;
            Sum_dic["비즈니스개발보너스"] = 0;
            Sum_dic["유니레벨보너스"] = 0;
            Sum_dic["사이드볼륨인피니티보너스"] = 0;
            Sum_dic["리더체크매치보너스"] = 0;
            Sum_dic["랭크업보너스"] = 0;
            Sum_dic["글로벌풀보너스"] = 0;
            

            Sum_dic["기타보너스"] = 0;

            Sum_dic["반품공제액"] = 0;
            
            Sum_dic["Cap공제"] = 0;
            Sum_dic["반품공제포함합"] = 0;
            Sum_dic["마감미지급액"] = 0;
            Sum_dic["이월수당합"] = 0;

            Sum_dic["지급수당합"] = 0;
            Sum_dic["소득세합"] = 0;
            Sum_dic["주민세합"] = 0;
            Sum_dic["실지급액합"] = 0;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {

                Sum_dic["첫팩주문보너스"] = Sum_dic["첫팩주문보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance1"].ToString());
                Sum_dic["멘토보너스"] = Sum_dic["멘토보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance2"].ToString());
                Sum_dic["비즈니스개발보너스"] = Sum_dic["비즈니스개발보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance3"].ToString());
                Sum_dic["유니레벨보너스"] = Sum_dic["유니레벨보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance4"].ToString());
                Sum_dic["사이드볼륨인피니티보너스"] = Sum_dic["사이드볼륨인피니티보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance5"].ToString());
                Sum_dic["리더체크매치보너스"] = Sum_dic["리더체크매치보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance6"].ToString());

                Sum_dic["랭크업보너스"] = Sum_dic["랭크업보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance7"].ToString());
                Sum_dic["글로벌풀보너스"] = Sum_dic["글로벌풀보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Allowance8"].ToString());

                Sum_dic["기타보너스"] = Sum_dic["기타보너스"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Etc_Pay"].ToString());

                Sum_dic["반품공제액"] = Sum_dic["반품공제액"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Cur_DedCut_Pay"].ToString());
                
                Sum_dic["Cap공제"] = Sum_dic["Cap공제"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumAllAllowance_Cut"].ToString());
                Sum_dic["반품공제포함합"] = Sum_dic["반품공제포함합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Cur_D_SumAllowance"].ToString());
                Sum_dic["마감미지급액"] = Sum_dic["마감미지급액"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Not_Pay_C"].ToString());
                Sum_dic["이월수당합"] = Sum_dic["이월수당합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Be_SumAllowance"].ToString());



                //Sum_dic["나눔기부"] = Sum_dic["나눔기부"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sum_Gibu"].ToString());

                Sum_dic["지급수당합"] = Sum_dic["지급수당합"] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumAllAllowance"].ToString());
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
            //직급수당_스타마스타 ,  바이너리보너스 , 추천매칭
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
            tab_Detail_02.SelectedIndex = 0;
            
            cls_Grid_Base cgb_P1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(dGridView_Pay_1, cgb_P1,2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Base_Header_Reset(dGridView_Pay_2, cgb_P1,2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Base_Header_Reset(dGridView_Pay_S_Down, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);
            
            dGridView_Base_Header_Reset(dGridView_Detail_2, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_SellData_Header_Reset(dGridView_Detail_1, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Up_S_Header_Reset(dGridView_Detail_3, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);

            dGridView_Up_S_Header_Reset(dGridView_Detail_4, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);


            dGridView_Base_Header_Reset(dGridView_Pay_4, cgb_P1, 4);
            cgb_P1.d_Grid_view_Header_Reset(1);
            
            dGridView_Base_Header_Reset(dGridView_Pay_5, cgb_P1, 4);
            cgb_P1.d_Grid_view_Header_Reset(1);
            
            dGridView_Base_Header_Reset(dGridView_Pay_6, cgb_P1, 4);
            cgb_P1.d_Grid_view_Header_Reset(1);
            
            dGridView_Base_Header_Reset(dGridView_Pay_S_Up, cgb_P1, 3); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);


            dGridView_Base_Header_Reset(dGridView_Detail_5, cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset(1);            



            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(tab_Detail_02);
        }

        
        
        private void dGridView_Base_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            return; 

            Clear_Pay_Detail();            
            
            //"회원번호", "성명", "마감_시작일"  ,"마감_종료일"  ,"지급_일자"  
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string T_Mbid = "" , ToEndDate = "" ;                
                
                T_Mbid = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();                
                ToEndDate = (sender as DataGridView).CurrentRow.Cells[3].Value.ToString();
                ToEndDate = ToEndDate.Replace("-", "");

                //Allowance_Detail(T_Mbid, ToEndDate);

                //Pay_Detail(T_Mbid, ToEndDate);
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
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "1", cgb_P1);  //["첫팩주문보너스"]

            dGridView_Base_Header_Reset(dGridView_Pay_2, cgb_P1, 1); 
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "2", cgb_P1);  //["멘토보너스"]
                                                                         //Real_Allowance_Detail_Nom(ToEndDate, Mbid, Mbid2, 2, cgb_P1);  //하선 매출 내역-- 추천




            dGridView_Base_Header_Reset(dGridView_Pay_4, cgb_P1, 4); 
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "4", cgb_P1);  //["["유니레벨보너스"]"]

            dGridView_Base_Header_Reset(dGridView_Pay_5, cgb_P1, 4); 
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "5", cgb_P1);  //["사이드볼륨인피니티보너스"]
            
            dGridView_Base_Header_Reset(dGridView_Pay_6, cgb_P1, 4);
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, "6", cgb_P1);  //["리더체크매치보너스"]





            dGridView_Base_Header_Reset(dGridView_Pay_S_Up, cgb_P1, 3); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail_Up(ToEndDate, Mbid, Mbid2, cgb_P1);  //판매내역 역추적


            dGridView_Base_Header_Reset(dGridView_Pay_S_Down, cgb_P1, 2); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();
            Real_Allowance_Detail(ToEndDate, Mbid, Mbid2, 1, cgb_P1);  //하선 매출 내역 -후원


           

        }



        private void Pay_Detail(string T_Mbid, string ToEndDate )
        {
            cls_Search_DB csd = new cls_Search_DB();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);
            
             
            cls_Grid_Base cgb_V1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(dGridView_Detail_5, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();
            Real_Pay_Detail(ToEndDate, Mbid, Mbid2, 1, cgb_V1);  //그룹하선 매출 내역

            dGridView_SellData_Header_Reset(dGridView_Detail_1, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();
            Real_Pay_Detail(ToEndDate, Mbid, Mbid2, cgb_V1);  //본인 매출 내역


            //dGridView_Dir_Grade_Header_Reset(dGridView_Detail_5, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_V1.d_Grid_view_Header_Reset();
            //Real_Dir_Detail(ToEndDate, Mbid, Mbid2, cgb_V1);  //직하선 추천 내역

            
            dGridView_Up_S_Header_Reset(dGridView_Detail_3, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();
            Base_Grid_Set(ToEndDate, Mbid, Mbid2, "ufn_Up_Search_Save_Close_04", cgb_V1);

            dGridView_Up_S_Header_Reset(dGridView_Detail_4, cgb_V1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_V1.d_Grid_view_Header_Reset();
            Base_Grid_Set(ToEndDate, Mbid, Mbid2,"ufn_Up_Search_Nomin_Close_04", cgb_V1); //추천인 역추적


           // Real_Pay_Detail_ETC(ToEndDate, Mbid, Mbid2);  //기타 내역을 넣는다.
        }


        private void Real_Allowance_Detail(string ToEndDate, string Mbid, int Mbid2, string SortOrder, cls_Grid_Base cgb_P)
        {
            string StrSql ="";

            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " RequestMbid + '-' + Convert(Varchar,RequestMbid2) ";
            else
                StrSql = StrSql + " RequestMbid2 ";

            StrSql = StrSql + ",RequestName,DownPV, Convert(Varchar, LineCnt)  LineCnt , LevelCnt,OrderNumber,GivePay , SortOrder, Real_LVL  ";
            StrSql = StrSql + " From  tbl_Close_DownPV_ALL_04 (nolock) ";
            StrSql = StrSql + " Where SaveMbid = '" + Mbid + "'";
            StrSql = StrSql + " And SaveMbid2 = " + Mbid2 ;
            StrSql = StrSql + " And EndDate ='" + ToEndDate + "'";

            //if (SortOrder =="7")
            //    StrSql = StrSql + " And SortOrder in ('7','8')";
            //else
            StrSql = StrSql + " And SortOrder='" + SortOrder + "'";
            StrSql = StrSql + " Order By LineCnt , LevelCnt ";
            
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

            if (SortOrder == "4" || SortOrder == "5" || SortOrder == "6")
            {
                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_Pay2_gr_dic_4_5_6(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
            }
            else
            {
                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    Set_Pay2_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                }
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

            StrSql = StrSql + ",RequestName, Sell_DownPV AS DownPV ,  Convert(varchar,LineCnt)  LineCnt, LevelCnt ,Ordernumber";
            StrSql = StrSql + " From  tbl_Close_DownPV_PV_04 (nolock) ";
            StrSql = StrSql + " Where SaveMbid = '" + Mbid + "'";
            StrSql = StrSql + " And SaveMbid2 = " + Mbid2;
            StrSql = StrSql + " And EndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " And Sortorder = '1' "; 
            StrSql = StrSql + " Order By LineCnt ,LevelCnt  ";

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


        private void Real_Allowance_Detail_Nom(string ToEndDate, string Mbid, int Mbid2, int SortOrder, cls_Grid_Base cgb_P)
        {
            string StrSql = "";

            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " RequestMbid + '-' + Convert(Varchar,RequestMbid2) ";
            else
                StrSql = StrSql + " RequestMbid2 ";

            StrSql = StrSql + ",RequestName, Sell_DownPV AS DownPV , 0 LineCnt, LevelCnt ,Ordernumber  ";
            StrSql = StrSql + " From  tbl_Close_DownPV_PV_04_Nom (nolock) ";
            StrSql = StrSql + " Where SaveMbid = '" + Mbid + "'";
            StrSql = StrSql + " And SaveMbid2 = " + Mbid2;
            StrSql = StrSql + " And EndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " And Sortorder = '" + SortOrder + "' ";
            StrSql = StrSql + " Order By LineCnt ,LevelCnt  ";

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


        private void Real_Allowance_Detail_Nom_130(string ToEndDate, string Mbid, int Mbid2, int SortOrder, cls_Grid_Base cgb_P)
        {
            string StrSql = "";

            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + "  tbl_SalesDetail.Mbid + '-' + Convert(Varchar, tbl_SalesDetail.Mbid2) ";
            else
                StrSql = StrSql + "  tbl_SalesDetail.Mbid2 ";

            StrSql = StrSql + ",tbl_SalesDetail.M_Name RequestName , tbl_SalesDetail.TotalPV AS DownPV , 0 LineCnt, 1 LevelCnt ,tbl_SalesDetail.Ordernumber  ";
            StrSql = StrSql + " From  tbl_SalesDetail (nolock) ";

            StrSql = StrSql + " LEFT JOIN( ";
           StrSql = StrSql + "          Select COUNT(Ordernumber) AA1, Ordernumber ";
            StrSql = StrSql + "          From tbl_SalesItemDetail (nolock) ";
            StrSql = StrSql + "          Where ItemCode In (select ncode From tbl_Goods (nolock) where G_130_FLAG = 'Y') ";
            StrSql = StrSql + "          Group by  Ordernumber ";
            StrSql = StrSql + " 		) AS S_3081 ";
            StrSql = StrSql + "          ON S_3081.OrderNumber = tbl_SalesDetail.OrderNumber ";


            StrSql = StrSql + " LEFT JOIN( ";
            StrSql = StrSql + "             Select COUNT(Ordernumber) AA2, T_OrderNumber1 ";
            StrSql = StrSql + "             From tbl_SalesItemDetail (nolock) ";
            StrSql = StrSql + "            Where ItemCode In (select ncode From tbl_Goods (nolock) where G_130_FLAG = 'Y') ";
            StrSql = StrSql + "            And SellState in ('R_3', 'R_1') ";
            StrSql = StrSql + "            Group by  T_OrderNumber1 ";
            StrSql = StrSql + " 			) AS S_3081_R ";
            StrSql = StrSql + "            ON S_3081_R.T_OrderNumber1 = tbl_SalesDetail.OrderNumber ";
            StrSql = StrSql + " LEFT JOIN  tbl_ClosePay_04_Mod (nolock) ON ToEndDate ='" + ToEndDate + "' And tbl_SalesDetail.Mbid = tbl_ClosePay_04_Mod.Mbid And tbl_SalesDetail.Mbid2 = tbl_ClosePay_04_Mod.Mbid2 "; 

            StrSql = StrSql + " Where tbl_ClosePay_04_Mod.Nominid = '" + Mbid + "'";
            StrSql = StrSql + " And   tbl_ClosePay_04_Mod.Nominid2 = " + Mbid2;
            StrSql = StrSql + " And  SellDate_2  <='" + ToEndDate + "'";
            StrSql = StrSql + " And     tbl_SalesDetail.ReturnTF = 1  ";
            StrSql = StrSql + " And     S_3081.OrderNumber is not null	  ";
            StrSql = StrSql + " And     S_3081_R.T_OrderNumber1 is  null  ";
            StrSql = StrSql + " Order By tbl_SalesDetail.Mbid, tbl_SalesDetail.Mbid2   ";

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




        private void Real_Allowance_Detail_G(string ToEndDate, string Mbid, int Mbid2, int SortOrder, cls_Grid_Base cgb_P)
        {
            string StrSql = "";

            StrSql = "Select M_ToEndDate ";

            StrSql = StrSql + ",Total_Pay, Cur_Cnt  ,End_Date , Max_Cnt,  Max_Cnt - Cur_Cnt   Not_Cnt ,  ISnull(C1.Grade_Name,'')  GG_Name ";
            StrSql = StrSql + " , Case When Cut_TF = 0 then '정상' When Cut_TF = 1 then '50%' ELSE '미유지미지급' End TF_50 ";
            StrSql = StrSql + " From  tbl_ClosePay_04_G_Mod (nolock) ";
            StrSql = StrSql + " Left Join tbl_Class C1  (nolock) On tbl_ClosePay_04_G_Mod.A_Grade = C1.Grade_Cnt ";
            StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
            StrSql = StrSql + " And Mbid2 = " + Mbid2;
            StrSql = StrSql + " And ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " Order By M_ToEndDate ";

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
                Set_Pay_gr_dic_G(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
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

            StrSql = StrSql + ",SaveName, DownPV ,OrderNumber, LevelCnt , LineCnt, GivePay , ST1 ";
            StrSql = StrSql + " From ";
            StrSql = StrSql + " ( " ;
            StrSql = StrSql + " Select EndDate, RequestMbid, RequestMbid2 ,RequestName ,OrderNumber, SaveMbid, SaveMbid2,SaveName, DownPV, LevelCnt, LineCnt, GivePay,";
            StrSql = StrSql + " Case  When   SortOrder = '1' then '직급'   When   SortOrder = '7' OR SortOrder = '8' then '세대매칭'    ";
            StrSql = StrSql + " End AS ST1 ";            
            StrSql = StrSql + " From tbl_Close_DownPV_ALL_04 (nolock)  ";
            StrSql = StrSql + " Left Join tbl_CloseTotal_04   (nolock) On tbl_CloseTotal_04.ToEndDate= tbl_Close_DownPV_ALL_04.EndDate  ";
        
            StrSql = StrSql + " UNION ALL";

            StrSql = StrSql + "  Select EndDate, RequestMbid , RequestMbid2 ,RequestName , OrderNumber,   SaveMbid ,  SaveMbid2,  SaveName, Sell_DownPV  DownPV , LevelCnt,LineCnt,0 GivePay,";
            StrSql = StrSql + " '판매누적'  AS ST1 ";            
            StrSql = StrSql + " From tbl_Close_DownPV_PV_04  (nolock) ";
            StrSql = StrSql + " Left Join tbl_CloseTotal_04  (nolock)  On tbl_CloseTotal_04.ToEndDate= tbl_Close_DownPV_PV_04.EndDate  ";

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
                                , ""     , ""  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }
            else if (S_TF == 1)
            {
                string[] g_HeaderText = {"회원번호", "성명", "금액"  ,"대수"  ,""  
                                , "주문번호"     , ""  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }

            else if (S_TF == 4)
            {
                string[] g_HeaderText = {"회원번호", "성명", "금액"  ,"실대수"  ,"압축대수"
                                , "주문번호"     , "라인"  , ""   , ""    , ""
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }

            else if (S_TF == 11)
            {
                string[] g_HeaderText = {"회원번호", "성명", ""  ,""  ,""
                                , ""     , ""  , ""   , ""    , ""
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }
            else if (S_TF == 2)
            {
                string[] g_HeaderText = {"회원번호", "성명", "PV"  ,"대수"  ,"라인"  
                                , "주문번호"     , ""  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }
            else if (S_TF == 22)
            {
                string[] g_HeaderText = {"회원번호", "성명", "금액"  ,"대수"  ,""
                                , "주문번호"     , ""  , ""   , ""    , ""
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }
            else if (S_TF == 3)
            {
                string[] g_HeaderText = {"회원번호", "성명", "금액"  ,"대수"  ,"라인"  
                                , "주문번호"     , "압축대수"  , "구분"   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }
            else if (S_TF == 5)
            {
                string[] g_HeaderText = {"마감일", "수당직급", "총금액"  ,"지급횟수"  ,"잔여지급횟수"  
                                , "50%여부"     , "종료일"  , ""   , ""    , ""                                   
                                    };
                cgb_P.grid_col_header_text = g_HeaderText;
            }




            if (S_TF == 2 || S_TF == 5)
            {
                int[] g_Width = { 100, 100 , 100, 100, 100                            
                             , 120, 0,0 , 0, 0 
                            };
                cgb_P.grid_col_w = g_Width;
            }

            else if (S_TF == 4)
            {
                int[] g_Width = { 100, 100 , 100, 100, 100
                             , 120, 110,0 , 0, 0
                            };
                cgb_P.grid_col_w = g_Width;
            }

            else if (S_TF == 22)
            {
                int[] g_Width = { 100, 100 , 100, 100, 0
                             , 120, 0,0 , 0, 0
                            };
                cgb_P.grid_col_w = g_Width;
            }

            else if (S_TF == 1)
            {
                int[] g_Width = { 100, 100 , 100, 100, 0
                             , 120, 0,0 , 0, 0
                            };
                cgb_P.grid_col_w = g_Width;
            }
            else if (S_TF == 11)
            {
                int[] g_Width = { 100, 100 , 0, 0, 0
                             , 0, 0,0 , 0, 0
                            };
                cgb_P.grid_col_w = g_Width;
            }

            else if (S_TF == 3)
            {
                int[] g_Width = { 100, 100 , 100, 100, 100                            
                             , 100, 100,100 , 0, 0 
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


        private void Set_Pay_gr_dic_G(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["GG_Name"]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["Total_Pay"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["Max_Cnt"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["Not_Cnt"]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["TF_50"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["End_Date"]
                                ,""//ds.Tables[base_db_name].Rows[fi_cnt]["End_Date"]
                                ,""
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }



        private void Set_Pay_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
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

        private void Set_Pay2_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["RequestName"]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["DownPV"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LevelCnt"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LineCnt"]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["SortOrder"]
                                ,""
                                ,""
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }

        private void Set_Pay2_gr_dic_4_5_6(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {

            //string[] g_HeaderText = {"회원번호", "성명", "금액"  ,"실대수"  ,"압축대수"
            //                    , "주문번호"     , ""  , ""   , ""    , ""
            //                        };

            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["RequestName"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["DownPV"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LevelCnt"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["Real_LVL"]

                                ,ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LineCnt"]
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
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["LineCnt"]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["GivePay"]
                                ,ds.Tables[base_db_name].Rows[fi_cnt]["ST1"]
                                ,""
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }








        private void Real_Pay_Detail(string ToEndDate, string Mbid, int Mbid2, int SortOrder, cls_Grid_Base cgb_P)
        {


            string StrSql = "";

            //string[] g_HeaderText = {"회원번호","성명", "하선액티브인원", "본인총매출PV"  ,"직추천소비총매출PV"
            //                , "본인기간매출PV"     , "직추천소비기간매출PV"  , "기간하선PV"   , "기간직급"    , "기간하위GED포인트"

            //                , "기간하위PD포인트"     , "기간하위BPD포인트"  , "기간하위SPD포인트"   , "기간하위GPD포인트"    , "기간하위PPD포인트"
            //                , "기간하위1PPD포인트"     , "기간하위2PPD포인트"  , "기간하위3PPD포인트"   , "기간하위4PPD포인트"    , "기간하위CPA포인트"
            //                    };

            StrSql = "Select tbl_ClosePay_04_Up_Mod.Mbid2 ";

            StrSql = StrSql + ",tbl_Memberinfo.M_Name  ";
            StrSql = StrSql + ",Case When tbl_ClosePay_04_Up_Mod.ReqTF2 =  1 then '유'   ELSE '' End   ";
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Active_Mem_Cnt  ";
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Self_Total_PV  ";
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Self_Total_Dir_PV  ";


            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Self_M_PV  ";
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Self_M_Dir_PV  ";
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Down_PV_S  ";
            StrSql = StrSql + ",ISnull(C4.Grade_Name,'')   ";

            
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Leg_Sum_Pay  ";

            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Cur_Leader_Leg_P_80  ";

            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Cur_Leader_Leg_P_90  ";
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Cur_Leader_Leg_P_100  ";
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Cur_Leader_Leg_P_110  ";
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Cur_Leader_Leg_P_120  ";
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Cur_Leader_Leg_P_130  ";

            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Cur_Leader_Leg_P_140  ";
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Cur_Leader_Leg_P_150  ";
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Cur_Leader_Leg_P_160  ";
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Cur_Leader_Leg_P_170  ";
            StrSql = StrSql + ",tbl_ClosePay_04_Up_Mod.Cur_Leader_Leg_P_180  ";

            StrSql = StrSql + " From  tbl_ClosePay_04_Up_Mod (nolock) ";
            StrSql = StrSql + " LEFT JOIN  tbl_Memberinfo (nolock) ON tbl_ClosePay_04_Up_Mod.Mbid = tbl_Memberinfo.Mbid And tbl_ClosePay_04_Up_Mod.Mbid2 = tbl_Memberinfo.Mbid2  ";
            StrSql = StrSql + " Left Join tbl_Class C4  (nolock) On tbl_ClosePay_04_Up_Mod.OneGrade = C4.Grade_Cnt ";
            //StrSql = StrSql + " Where tbl_ClosePay_04_Up_Mod.Saveid = '" + Mbid + "'";
            StrSql = StrSql + " Where tbl_ClosePay_04_Up_Mod.Saveid2 = " + Mbid2;
            StrSql = StrSql + " And tbl_ClosePay_04_Up_Mod.ToEndDate ='" + ToEndDate + "'";
            
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_Dri_gr_dic(ref ds, ref gr_dic_text, fi_cnt, cgb_P);  //데이타를 배열에 넣는다.
            }

            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
           


            //object[] row0 = { "1라인"
            //                    ,ds.Tables[base_db_name].Rows[0]["Be_PV_1"]  
            //                    ,ds.Tables[base_db_name].Rows[0]["Cur_PV_1"]
            //                    ,ds.Tables[base_db_name].Rows[0]["Ded_1"]
            //                    ,ds.Tables[base_db_name].Rows[0]["Fresh_1"]
 
            //                    ,ds.Tables[base_db_name].Rows[0]["Sum_PV_1"]
            //                    ,""
            //                    ,""
            //                    ,""
            //                    ,""
            //                     };

            //gr_dic_text[ 1] = row0;

            //object[] row1 = { "2라인"
            //                    ,ds.Tables[base_db_name].Rows[0]["Be_PV_2"]  
            //                    ,ds.Tables[base_db_name].Rows[0]["Cur_PV_2"]
            //                    ,ds.Tables[base_db_name].Rows[0]["Ded_2"]
            //                    ,ds.Tables[base_db_name].Rows[0]["Fresh_2"]
 
            //                    ,ds.Tables[base_db_name].Rows[0]["Sum_PV_2"]
            //                    ,""
            //                    ,""
            //                    ,""
            //                    ,""
            //                     };

            //gr_dic_text[2] = row1;


            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();
        }

        private void Real_Pay_Detail_ETC(string ToEndDate, string Mbid, int Mbid2)
        {


            string StrSql = "";

            ////if (cls_app_static_var.Member_Number_1 > 0)
            ////    StrSql = StrSql + " RequestMbid + '-' + Convert(Varchar,RequestMbid2) ";
            ////else
            ////    StrSql = StrSql + " RequestMbid2 ";

            StrSql = "Select ";

            StrSql = StrSql + " G_Cur_PV, High_PV ";
            StrSql = StrSql + ",Non_High_PV, Pa_Down_Cnt ";
            StrSql = StrSql + ",ing_Cnt_9, ing_Cnt_10 ";
            StrSql = StrSql + ",ing_Cnt_11, ing_Cnt_12 ";
            StrSql = StrSql + ",ing_Cnt_13 ";            

            StrSql = StrSql + " From  tbl_ClosePay_04_Mod (nolock) ";
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

            //txt_ETC1.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Cur_PV"].ToString()));
            //txt_ETC2.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["High_PV"].ToString()));

            //txt_ETC3.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["Non_High_PV"].ToString()));
            //txt_ETC4.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["Pa_Down_Cnt"].ToString()));

            txt_ETC5.Text = ds.Tables[base_db_name].Rows[0]["ing_Cnt_9"].ToString();
            txt_ETC6.Text = ds.Tables[base_db_name].Rows[0]["ing_Cnt_10"].ToString();

            txt_ETC7.Text = ds.Tables[base_db_name].Rows[0]["ing_Cnt_11"].ToString();
            txt_ETC8.Text = ds.Tables[base_db_name].Rows[0]["ing_Cnt_12"].ToString();

            txt_ETC9.Text = ds.Tables[base_db_name].Rows[0]["ing_Cnt_13"].ToString();
            //txt_ETC11.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(ds.Tables[base_db_name].Rows[0]["G_Sum_PV_2"].ToString()));

           

            

            //if (int.Parse(ds.Tables[base_db_name].Rows[0]["ReqTF2"].ToString()) >= 1)
            //    txt_ETC7.Text = "유";
        }

        

        private void dGridView_Base_Header_Reset(DataGridView dGridView, cls_Grid_Base cgb_P)
        {

            cgb_P.grid_col_Count = 22;
            cgb_P.basegrid = dGridView;
            cgb_P.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_P.grid_Frozen_End_Count = 3;
            cgb_P.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"회원번호","성명", "본인액티브",  "하선액티브인원", "본인총매출PV"
                            ,"직추천소비총매출PV" , "본인기간매출PV"     , "직추천소비기간매출PV"  , "기간하선PV"   , "기간직급"


                           ,"레그적용금액"  , "기간하위GED포인트" , "기간하위PD포인트"     , "기간하위BPD포인트"  , "기간하위SPD포인트"
                             , "기간하위GPD포인트", "기간하위PPD포인트" , "기간하위1PPD포인트"     , "기간하위2PPD포인트"  , "기간하위3PPD포인트"
                           , "기간하위4PPD포인트"      , "기간하위CPA포인트"
                                };
            cgb_P.grid_col_header_text = g_HeaderText;
            
            int[] g_Width = { 100, 100 , 100, 100, 100
                             , 100   , 100,  100 , 100, 100

                             , 120, 120,  120 , 120, 120
                             , 120, 120,  120 , 120, 120
                             , 120 , 120
                            };
            cgb_P.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true

                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true,true
                                   };

            cgb_P.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight 

                               ,DataGridViewContentAlignment.MiddleRight//5      
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleCenter

                               ,DataGridViewContentAlignment.MiddleRight//10                         
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
                              };
            cgb_P.grid_col_alignment = g_Alignment;

            string T_str_Grid_Currency_Type = "###,###,##0.00";

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();            
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = T_str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = T_str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = T_str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = T_str_Grid_Currency_Type;
            gr_dic_cell_format[16 - 1] = T_str_Grid_Currency_Type;
            gr_dic_cell_format[17 - 1] = T_str_Grid_Currency_Type;
            gr_dic_cell_format[18 - 1] = T_str_Grid_Currency_Type;
            gr_dic_cell_format[19 - 1] = T_str_Grid_Currency_Type;
            gr_dic_cell_format[20 - 1] = T_str_Grid_Currency_Type;
            gr_dic_cell_format[21 - 1] = T_str_Grid_Currency_Type;
            gr_dic_cell_format[22 - 1] = T_str_Grid_Currency_Type;

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

            //string[] g_HeaderText = {"구매종류", "이전금액", "기간금액"  ,"총합금액" ," " 
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
                StrSql = StrSql + " FROM tbl_ClosePay_04_Sell_Mod (nolock) ";

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




        private void Real_Dir_Detail(string ToEndDate, string Mbid, int Mbid2, cls_Grid_Base cgb_P)
        {
            string StrSql = "";

            //string[] g_HeaderText = {"_위치", "회원번호", "성명"  ,"직급" ,"본인매출"
            //                         ,"총하선매출" ,"MB승급일"  , "AG승급일", "SV승급일"  ,"GD승급일" 
            //                         , "EM승급일" ,"DM승급일"  , "DDM승급일", "TDM승급일"  ,"GDM승급일" 

            //                    };

            StrSql = "Select LineCnt , ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " Mbid + '-' + Convert(Varchar,Mbid2) ";
            else
                StrSql = StrSql + " Mbid2 ";

            StrSql = StrSql + ",M_Name, ISnull(C2.Grade_Name,'') ";
            StrSql = StrSql + ",Day_Sum_PV, G_Cur_PV  ";
            StrSql = StrSql + ",'','','' ,''";
            //StrSql = StrSql + ",GradeDate1 ";
            //StrSql = StrSql + ",GradeDate2 ";
            //StrSql = StrSql + ",GradeDate3 ";
            //StrSql = StrSql + ",GradeDate4 ";
            //StrSql = StrSql + ",GradeDate5 ";
            //StrSql = StrSql + ",GradeDate6 ";
            //StrSql = StrSql + ",GradeDate7 ";
            //StrSql = StrSql + ",GradeDate8 ";
            //StrSql = StrSql + ",GradeDate9 ";
            StrSql = StrSql + " From  tbl_ClosePay_04_Mod (nolock) ";
            StrSql = StrSql + " Left Join tbl_Class C2  (nolock) On tbl_ClosePay_04_Mod.CurGrade = C2.Grade_Cnt ";
            StrSql = StrSql + " Where Saveid = '" + Mbid + "'";
            StrSql = StrSql + " And Saveid2 = " + Mbid2;
            StrSql = StrSql + " And ToEndDate ='" + ToEndDate + "'";
            StrSql = StrSql + " Order By LineCnt  ";

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
                Set_Dri_gr_dic(ref ds, ref gr_dic_text, fi_cnt, cgb_P);  //데이타를 배열에 넣는다.
            }

            cgb_P.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_P.db_grid_Obj_Data_Put();

        }


        private void Set_Dri_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt, cls_Grid_Base cgb_P)
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




        private void dGridView_Dir_Grade_Header_Reset(DataGridView dGridView, cls_Grid_Base cgb_P)
        {

            cgb_P.grid_col_Count = 10;
            cgb_P.basegrid = dGridView;
            cgb_P.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_P.grid_Frozen_End_Count = 1;
            cgb_P.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"_위치", "회원번호", "성명"  ,"직급" ,"본인매출"
                                     ,"총하선매출" ,""  , "", ""  ,""                                      
                                };
            cgb_P.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 0, 100 , 100, 100, 100
                             , 10, 0,0 , 0, 0                              
                             
                            };
            cgb_P.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                          

                                   };
            cgb_P.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight//5    
  
                               ,DataGridViewContentAlignment.MiddleRight                            
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter//10    
                     
                               //,DataGridViewContentAlignment.MiddleCenter
                               //,DataGridViewContentAlignment.MiddleCenter
                               //,DataGridViewContentAlignment.MiddleCenter
                               //,DataGridViewContentAlignment.MiddleCenter
                               //,DataGridViewContentAlignment.MiddleCenter
                      
                              };
            cgb_P.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
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


            int chk_cnt = 0, chk_0_cnt = 0, Max_ToEndDate = 0;
            for (int i = 0; i <= dGridView_Base.RowCount - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[0]).ToString() == "V")
                {
                    chk_cnt++;

                    if (Max_ToEndDate < int.Parse((dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[5]).ToString().Replace("-", ""))))
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
                StrSql = StrSql + " Where ToEndDate ='" + Max_ToEndDate.ToString() + "'";
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
                    dGridView_Base.Focus(); return;
                }
                else
                {
                    int Web_V_TF = int.Parse(ds.Tables[base_db_name].Rows[0]["Web_V_TF"].ToString());

                    if (Web_V_TF == 0)
                    {
                        MessageBox.Show("확정처리되지 않은 마감 내역이 존재합니다." + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        dGridView_Base.Focus(); return;
                    }
                }

            }

            if (chk_cnt == 0) //저장할 내역이 없을을 알린다.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridView_Base.Focus(); return;
            }

            if (chk_0_cnt > 0) //저장할 내역이 없을을 알린다.
            {
                MessageBox.Show("실지급액이 0원인 내역은 미지급 처리가 불가능합니다." + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridView_Base.Focus(); return;
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
                    if (dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[0]).ToString() == "V")
                    {
                        string T_Mbid = dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[1]).ToString();
                        string ToEndDate = dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[5]).ToString();
                        string PayDate = dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[6]).ToString();
                        double SumAllAllowance = double.Parse(dGridView_Base.GetRowCellValue(i, dGridView_Base.Columns[63]).ToString());
                        ToEndDate = ToEndDate.Replace("-", "");
                        PayDate = PayDate.Replace("-", "");

                        Mbid = ""; Mbid2 = 0;
                        csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

                        if (Mbid2 > 0 || Mbid != "")
                        {
                            StrSql = "INSERT INTO tbl_Close_Not_Pay_04 ";
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

        private void dGridView_Base_CustomDrawRowIndicator(object sender, DXVGrid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }


        private void dGridView_Base_DoubleClick_2(object sender, EventArgs e)
        {
            Clear_Pay_Detail();
            //idx_Mbid = "";
            //idx_ToEndDate = "";

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
               // Ga_FLAG = view.GetRowCellValue(info.RowHandle, view.Columns[6]).ToString();

                string Max_N_LineCnt = view.GetRowCellValue(info.RowHandle, view.Columns[60]).ToString();
                ToEndDate = ToEndDate.Replace("-", "");
                FromEndDate = FromEndDate.Replace("-", "");

                //idx_Mbid = T_Mbid;
                //idx_ToEndDate = ToEndDate;


                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Allowance_Detail(T_Mbid, ToEndDate);

                Pay_Detail(T_Mbid, ToEndDate);
                this.Cursor = System.Windows.Forms.Cursors.Default;


            }
        }







        private void butt_Excel_Pay_1_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Pay_1);
            e_f.ShowDialog();
        }
        private DataGridView e_f_Send_Export_Excel_Pay_1(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "첫팩주문보너스";
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
            Excel_Export_File_Name = "멘토보너스";
            Excel_Export_From_Name = this.Name;
            return dGridView_Pay_2;
        }

        private void butt_Excel_Pay_4_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Pay_4);
            e_f.ShowDialog();
        }

        private DataGridView e_f_Send_Export_Excel_Pay_4(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "유니레벨보너스";
            Excel_Export_From_Name = this.Name;
            return dGridView_Pay_4;
        }


        private void butt_Excel_Pay_5_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Pay_5);
            e_f.ShowDialog();
        }

        private DataGridView e_f_Send_Export_Excel_Pay_5(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "사이드볼륨인피니티보너스";
            Excel_Export_From_Name = this.Name;
            return dGridView_Pay_5;
        }


        private void butt_Excel_Pay_6_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Pay_6);
            e_f.ShowDialog();
        }

        private DataGridView e_f_Send_Export_Excel_Pay_6(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "리더체크매치보너스";
            Excel_Export_From_Name = this.Name;
            return dGridView_Pay_6;
        }





        private void butt_Excel_Pay_S_Down_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Pay_S_Down);
            e_f.ShowDialog();
        }

        private DataGridView e_f_Send_Export_Excel_Pay_S_Down(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = "후원기간하선판매";
            Excel_Export_From_Name = this.Name;
            return dGridView_Pay_S_Down;
        }






















    }
}
