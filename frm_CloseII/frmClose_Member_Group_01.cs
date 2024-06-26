﻿using System;
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
    public partial class frmClose_Member_Group_01 : clsForm_Extends
    {

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        private const string base_db_name = "tbl_DB";
        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Sum = new cls_Grid_Base();
        private int Data_Set_Form_TF = 0;

        private int Form_Load_TF = 0;

        public frmClose_Member_Group_01()
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
        }

        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Form_Load_TF = 0;
            Data_Set_Form_TF = 0;

            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
           // cpbf.Put_Close_CPoint_ComboBox(combo_Grade, combo_Grade_Code);
            cpbf.Put_Close_Sort_ComboBox(combo_Pay, combo_Pay_Code);


            Put_Rec_Code_ComboBox(combo_W_1, combo_W_Code_1);
            Put_Rec_Code_ComboBox(combo_W_2, combo_W_Code_2);

            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            mtxtFromDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtFromDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtToDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtToDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtPayDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtPayDate2.Mask = cls_app_static_var.Date_Number_Fromat;


            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
            mtxtMbid.Focus();
        }

        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (Form_Load_TF == 0)
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                dGridView_Sum_Base_Header_Reset();
                cgb_Sum.d_Grid_view_Header_Reset(1);
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                Form_Load_TF = 1;
            }

            if (cls_User.uSearch_MemberNumber != "")
            {
                Data_Set_Form_TF = 1;
                mtxtMbid.Text = cls_User.uSearch_MemberNumber;
                //mtxtSMbid.Text = cls_User.uSearch_MemberNumber;
                cls_User.uSearch_MemberNumber = "";

                EventArgs ee1 = null; Base_Button_Click(butt_Select, ee1);
                //Set_Form_Date(mtxtMbid.Text);
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
                if (e.KeyValue == 123 || e.KeyValue == 113 || e.KeyValue == 119 || e.KeyValue == 112)
                    Base_Button_Click(T_bt, ee1);
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
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                combo_Grade.SelectedIndex = -1;
                combo_Pay.SelectedIndex = -1;

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtMbid);
                
                
            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                dGridView_Sum_Base_Header_Reset();
                cgb_Sum.d_Grid_view_Header_Reset(1);
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


                combo_W_Code_1.SelectedIndex = combo_W_1.SelectedIndex;
                combo_W_Code_2.SelectedIndex = combo_W_2.SelectedIndex;

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
                combo_Pay_Code.SelectedIndex = combo_Pay.SelectedIndex; 

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

        }


        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            cls_form_Meth cm = new cls_form_Meth();
            Excel_Export_File_Name = this.Text; // cm._chang_base_caption_search ("주간_마감_회원별1");
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;


        }



        private void Make_Temp_Table(cls_Connect_DB Temp_Connect, SqlConnection Conn)
        {
            string StrSql = "";

          

            StrSql = "CREATE TABLE #T2_Mem" ;
            StrSql = StrSql + "(";
            StrSql = StrSql + "Mbid varchar(20)  NOT NULL DEFAULT ('') ,";
            StrSql = StrSql + "Mbid2 int NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "M_Name nvarchar(100)  NOT NULL DEFAULT ('') ,";
            StrSql = StrSql + "Cpno varchar(100)  NOT NULL DEFAULT ('') ,";
    
            StrSql = StrSql + "CurGrade smallint NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "TGrade varchar(30)  NOT NULL DEFAULT ('') ,";
            StrSql = StrSql + "hometel varchar(30)  NOT NULL DEFAULT ('') ,";
            StrSql = StrSql + "hptel varchar(30)  NOT NULL DEFAULT ('') ,";
    
            StrSql = StrSql + "PayStop_Date varchar(20)  NOT NULL DEFAULT ('') ,";
            StrSql = StrSql + "addcode1 varchar(30)  NOT NULL DEFAULT ('') ,";
            StrSql = StrSql + "T_address1 varchar(1000)  NOT NULL DEFAULT ('') ,";
    
            StrSql = StrSql + "BankCode varchar(20)  NOT NULL DEFAULT ('') ,";
            StrSql = StrSql + "bankname varchar(30)  NOT NULL DEFAULT ('') ,";
            StrSql = StrSql + "bankaccnt varchar(100)  NOT NULL DEFAULT ('') ,";
            StrSql = StrSql + "bankowner nvarchar(100)  NOT NULL DEFAULT ('') ,";
    
            StrSql = StrSql + "BusName varchar(100)  NOT NULL DEFAULT ('') ,";
    
            StrSql = StrSql + "Pay1 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay2 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay3 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay4 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay5 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay6 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay7 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay8 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay9 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay10 float NOT NULL DEFAULT (0) ,";

            StrSql = StrSql + "Pay11 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay12 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay13 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay14 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay15 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay16 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay17 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay18 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay19 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay20 float NOT NULL DEFAULT (0) ,";

            StrSql = StrSql + "Pay21 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay22 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay23 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay24 float NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Pay25 float NOT NULL DEFAULT (0) ,";


            StrSql = StrSql + "Cur_DedCut_Pay Money NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Etc_Pay_DedCut Money NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "Etc_Pay Money NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "SumAllAllowance Money NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "InComeTax Int NOT NULL DEFAULT (0) ,";
            StrSql = StrSql + "ResidentTax Int NOT NULL DEFAULT (0) ,";
    
            StrSql = StrSql + "TruePayment Money NOT NULL DEFAULT (0)  ";

            StrSql = StrSql + " Primary key(Mbid, Mbid2)";
            StrSql = StrSql + ")";

            Temp_Connect.Insert_Data(StrSql, this.Name, Conn);

        
            StrSql = " CREATE  INDEX [idx_#T2_Mem_02] ON  #T2_Mem(Mbid,Mbid2) " ;

            Temp_Connect.Insert_Data(StrSql, this.Name, Conn);
           
    
            StrSql = " CREATE  INDEX [idx_#T2_Mem_03] ON  #T2_Mem(SumAllAllowance) ";

            Temp_Connect.Insert_Data(StrSql, this.Name, Conn);
          
        }

        private void Make_Temp_Table_02(cls_Connect_DB Temp_Connect, SqlConnection Conn)
        { 
           
            string StrSql = "";

            StrSql = " Insert Into #T2_Mem " ;
            StrSql = StrSql + " SELECT tbl_Memberinfo.Mbid , tbl_Memberinfo.Mbid2, tbl_Memberinfo.M_Name  ";
    
            StrSql = StrSql + ", tbl_Memberinfo.Cpno  ";


            StrSql = StrSql + " ,CC_A.CurGrade , ISNULL(CC_A.G_Name,'' )  ,  hometel ,  tbl_Memberinfo.hptel ,  PayStop_Date  , ";
            StrSql = StrSql + "  LEFT(addcode1,3) + '-' + RIGHT(addcode1,3)   ,  address1 + ' ' +   address2 ,";
            StrSql = StrSql + "  isnull(BankCode, '') , isnull( Tbl_Bank.BankName , '')  , isnull( bankaccnt, '')  ,    isnull( bankowner, '')   , isnull( Tbl_Business.Name , '')   , ";
    
            StrSql = StrSql + " 0  ,  0  ,  0  , 0 , 0,  " ;
            StrSql = StrSql + " 0  ,  0  ,  0  , 0 , 0,  " ;
            StrSql = StrSql + " 0  ,  0  ,  0  , 0 , 0,  " ;
            StrSql = StrSql + " 0  ,  0  ,  0  , 0 , 0,  " ;
            StrSql = StrSql + " 0  ,  0  ,  0  , 0 , 0,  " ;

            StrSql = StrSql + " 0, 0, 0  ,  0  ,  0  , 0  , 0   ";
    
            StrSql = StrSql + " From tbl_Memberinfo (nolock)" ;
            StrSql = StrSql + " Left Join tbl_business (nolock)  On tbl_memberinfo.businesscode = tbl_business.ncode  ";
            //StrSql = StrSql + " Left Join tbl_bANK     (nolock)  On tbl_memberinfo.bankcode = tbl_bANK.ncode And tbl_Memberinfo.Na_code = tbl_bANK.Na_code ";
            StrSql = StrSql + " Left Join tbl_bANK     (nolock)  On tbl_memberinfo.bankcode = tbl_bANK.ncode ";
            cls_NationService.SQL_BankNationCode(ref StrSql);
            StrSql = StrSql + " LEFT JOIN tbl_Class C1 (nolock)  On tbl_Memberinfo.CurGrade = C1.Grade_Cnt   " ;
            StrSql = StrSql + " Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On CC_A.Mbid = tbl_Memberinfo.Mbid And  CC_A.Mbid2 = tbl_Memberinfo.Mbid2 ";            
            
            StrSql = StrSql + " Where tbl_Memberinfo.Mbid2 >= 0  ";

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
                        StrSql = StrSql + " And tbl_Memberinfo.Mbid ='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        StrSql = StrSql + " And tbl_Memberinfo.Mbid2 = " + Mbid2;
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
                        StrSql = StrSql + " And tbl_Memberinfo.Mbid >='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        StrSql = StrSql + " And tbl_Memberinfo.Mbid2 >= " + Mbid2;
                }

                if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        StrSql = StrSql + " And tbl_Memberinfo.Mbid <='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        StrSql = StrSql + " And tbl_Memberinfo.Mbid2 <= " + Mbid2;
                }
            }


            //회원명으로 검색
            if (txtName.Text.Trim() != "")
                StrSql = StrSql + " And tbl_Memberinfo.M_Name Like '%" + txtName.Text.Trim() + "%'";


            if (txtCenter_Code.Text.Trim() != "")
                StrSql = StrSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";


            if (combo_Grade_Code.Text != "")
                StrSql = StrSql + " And tbl_Memberinfo.CurPoint = " + combo_Grade_Code.Text ;


            StrSql = StrSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
            StrSql = StrSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


            Temp_Connect.Insert_Data(StrSql, this.Name, Conn);
        }



        private void Make_Base_Query_Pay_01(ref string Tsql)
        {
            string StrSql = "";

            //,"첫팩주문보너스"   , "멘토보너스" , "비즈니스개발보너스"     , "첫팩주문보너스"      , "멘토보너스"


            StrSql = "Update #T2_Mem SET ";
            StrSql = StrSql + "  Pay1 = ISNULL(B.A1,0 ) ";
            StrSql = StrSql + " , Pay2 = ISNULL(B.A2,0 )  ";
            StrSql = StrSql + " , Pay3 = ISNULL(B.A3,0 )  ";
            StrSql = StrSql + " , Pay4 = ISNULL(B.A4,0 ) ";
            StrSql = StrSql + " , Pay5 = ISNULL(B.A5,0 ) ";
            StrSql = StrSql + " , Pay6 = ISNULL(B.A6,0 ) ";
            StrSql = StrSql + " , Pay7 = ISNULL(B.A7,0 ) ";
            StrSql = StrSql + " , Pay8 = ISNULL(B.A8,0 ) ";
            StrSql = StrSql + " , Pay9 = ISNULL(B.A9,0 ) ";
            StrSql = StrSql + " , Pay10 = ISNULL(B.A10,0 ) ";
            StrSql = StrSql + " , Pay11 = ISNULL(B.A11,0 ) ";
            StrSql = StrSql + " , Pay12 = ISNULL(B.A12,0 ) ";

            StrSql = StrSql + " , SumAllAllowance = SumAllAllowance + ISNULL(B.S1,0 )  ";
            StrSql = StrSql + " , InComeTax = InComeTax + ISNULL(B.S2,0 ) ";
            StrSql = StrSql + " , ResidentTax = ResidentTax + ISNULL(B.S3,0 ) ";
            StrSql = StrSql + " , TruePayment = TruePayment + ISNULL(B.S4,0 ) ";
            StrSql = StrSql + " , Etc_Pay = Etc_Pay + ISNULL(B.S5,0 ) ";
            StrSql = StrSql + " , Etc_Pay_DedCut = Etc_Pay_DedCut + ISNULL(B.S6,0 ) ";
            StrSql = StrSql + " FROM  #T2_Mem  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select ";
            StrSql = StrSql + "  Sum(Convert(Money ,Allowance1)) AS A1 ,  Sum(Convert(Money ,Allowance2)) AS A2  ,  Sum(Convert(Money ,Allowance3)) AS A3 ";
            StrSql = StrSql + ", Sum(Convert(Money ,Allowance4)) AS A4 ,  Sum(Convert(Money ,Allowance5)) AS A5  ";
            StrSql = StrSql + ", Sum(Convert(Money ,Allowance6)) AS A6 ,  Sum(Convert(Money ,Allowance7)) AS A7  ,  Sum(Convert(Money ,Allowance8)) AS A8 ";
            StrSql = StrSql + ", Sum(Convert(Money ,Allowance9)) AS A9 ,  Sum(Convert(Money ,Allowance10)) AS A10  ";
            StrSql = StrSql + " , Sum(Convert(Money ,Allowance11)) AS A11 ,  Sum(Convert(Money ,Allowance12)) AS A12  "; 
            StrSql = StrSql + " , Sum(Convert(Money ,SumAllAllowance - SumAllAllowance_Be_Not)) S1 ,  Sum(InComeTax) S2 , Sum(ResidentTax) S3 , Sum(Convert(Money ,TruePayment)) S4 , Sum(Convert(Money ,Etc_Pay)) S5, Sum(Convert(Money ,Etc_Pay_DedCut)) S6 ";
            StrSql = StrSql + " , Mbid,Mbid2   ";
            StrSql = StrSql + " From tbl_ClosePay_01_Mod (nolock)  ";
            StrSql = StrSql + " Where  ToEndDate <> '' ";
            //StrSql = StrSql + " And   
            //StrSql = StrSql + " And    SumAllAllowance > 0  ";

            Tsql = StrSql;

            Make_Base_Query_(ref Tsql);

            Tsql = Tsql + " Group by Mbid, Mbid2 ";
            Tsql = Tsql + " ) B";
            Tsql = Tsql + "  Where a.Mbid = b.Mbid And a.Mbid2 = b.Mbid2";
            
        }

        private void Make_Base_Query_Pay_02(ref string Tsql)
        {
            string StrSql = "";

            StrSql = "Update #T2_Mem SET ";
            StrSql = StrSql + "  Pay1 = ISNULL(B.A1,0 ) ";
            StrSql = StrSql + " , Pay2 = ISNULL(B.A2,0 )  ";
            StrSql = StrSql + ",  Pay3 = ISNULL(B.A3,0 )  ";
            StrSql = StrSql + ",  Pay4 = ISNULL(B.A4,0 ) " ;
            StrSql = StrSql + " , Pay5 = ISNULL(B.A5,0 ) ";
            StrSql = StrSql + " , Pay6 = ISNULL(B.A6,0 ) ";
            StrSql = StrSql + " , Pay7 = ISNULL(B.A7,0 ) ";
            StrSql = StrSql + " , Pay8 = ISNULL(B.A8,0 ) ";
            ////StrSql = StrSql + " , Pay9 = ISNULL(B.A9,0 ) ";
            ////StrSql = StrSql + " , Pay10 = ISNULL(B.A10,0 ) ";
            StrSql = StrSql + " , SumAllAllowance = SumAllAllowance + ISNULL(B.S1,0 )  ";
            StrSql = StrSql + " , InComeTax = InComeTax + ISNULL(B.S2,0 ) ";
            StrSql = StrSql + " , ResidentTax = ResidentTax + ISNULL(B.S3,0 ) ";
            StrSql = StrSql + " , TruePayment = TruePayment + ISNULL(B.S4,0 ) ";
            StrSql = StrSql + " , Etc_Pay = Etc_Pay + ISNULL(B.S5,0 ) ";
            StrSql = StrSql + " , Cur_DedCut_Pay = Cur_DedCut_Pay + ISNULL(B.S6,0 ) ";
           // StrSql = StrSql + " , Etc_Pay_DedCut = Etc_Pay_DedCut + ISNULL(B.S7,0 ) ";
            
            StrSql = StrSql + " FROM  #T2_Mem  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select ";
            StrSql = StrSql + "  Sum(convert(float,Allowance1)) AS A1 ,  Sum(convert(float,Allowance2 )) AS A2  ,  Sum(convert(float,Allowance3)) AS A3 ";
            StrSql = StrSql + ", Sum(convert(float,Allowance4)) AS A4 ,  Sum(convert(float,Allowance5)) AS A5  ";
            StrSql = StrSql + ", Sum(convert(float,Allowance6)) AS A6 ,  Sum(convert(float,Allowance7)) AS A7  ,  Sum(convert(float,Allowance8)) AS A8 ";
            StrSql = StrSql + ", Sum(convert(float,Allowance9)) AS A9 ,  Sum(convert(float,Allowance10)) AS A10  ";

            StrSql = StrSql + " , Sum(convert(float, (Allowance1 + Allowance2 + Allowance3  - Cur_DedCut_Pay) + tbl_ClosePay_02_Mod.SumAllAllowance_Be_Not_Sum  + tbl_ClosePay_02_Mod.Etc_Pay  - Isnull(tbl_ClosePay_02_Mod.Cur_DedCut_Pay_DED,0)  )) S1 ";
            StrSql = StrSql + ", Sum(InComeTax) S2 , Sum(ResidentTax) S3 , Sum(convert(float,TruePayment)) S4 , Sum(convert(float,Etc_Pay)) S5 , Sum(convert(float,Cur_DedCut_Pay)) S6 ";
          
            StrSql = StrSql + " , Mbid,Mbid2   ";
            StrSql = StrSql + " From tbl_ClosePay_04_Mod (nolock)  ";           
            StrSql = StrSql + " Where  ToEndDate <> '' ";
            StrSql = StrSql + " And  TruePayment > 0  ";          

            Tsql = StrSql;

            Make_Base_Query_(ref Tsql);

            Tsql = Tsql + " Group by Mbid, Mbid2 ";
            Tsql = Tsql + " ) B";
            Tsql = Tsql + "  Where a.Mbid = b.Mbid And a.Mbid2 = b.Mbid2";
        }

        private void Make_Base_Query_Pay_03(ref string Tsql)
        {
            string StrSql = "";

            StrSql = "Update #T2_Mem SET ";
            //StrSql = StrSql + "  Pay1 = ISNULL(B.A1,0 ) ";
            //StrSql = StrSql + " , Pay2 = ISNULL(B.A2,0 )  ";
            //StrSql = StrSql + " , Pay3 = ISNULL(B.A3,0 )  ";
            StrSql = StrSql + "  Pay4 = ISNULL(B.A1,0 ) " ;
            StrSql = StrSql + " , Pay5 = ISNULL(B.A2,0 ) ";
            StrSql = StrSql + " , Pay6 = ISNULL(B.A3,0 ) ";
            StrSql = StrSql + " , Pay7 = ISNULL(B.A4,0 ) ";
            StrSql = StrSql + " , Pay8 = ISNULL(B.A5,0 ) ";
            StrSql = StrSql + " , Pay9 = ISNULL(B.A6,0 ) ";
            ////StrSql = StrSql + " , Pay10 = ISNULL(B.A10,0 ) ";
            StrSql = StrSql + " , SumAllAllowance = SumAllAllowance + ISNULL(B.S1,0 )  ";
            StrSql = StrSql + " , InComeTax = InComeTax + ISNULL(B.S2,0 ) ";
            StrSql = StrSql + " , ResidentTax = ResidentTax + ISNULL(B.S3,0 ) ";
            StrSql = StrSql + " , TruePayment = TruePayment + ISNULL(B.S4,0 ) ";
            StrSql = StrSql + " , Etc_Pay = Etc_Pay + ISNULL(B.S5,0 ) ";
            StrSql = StrSql + " , Cur_DedCut_Pay = Cur_DedCut_Pay + ISNULL(B.S6,0 ) ";
            StrSql = StrSql + " , Etc_Pay_DedCut = Etc_Pay_DedCut + ISNULL(B.S7,0 ) ";

            StrSql = StrSql + " FROM  #T2_Mem  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select ";
            StrSql = StrSql + "  Sum(convert(float,Allowance1)) AS A1 ,  Sum(convert(float,Allowance2 )) AS A2  ,  Sum(convert(float,Allowance3)) AS A3 ";
            StrSql = StrSql + ", Sum(convert(float,Allowance4)) AS A4 ,  Sum(convert(float,Allowance5)) AS A5  ";
            StrSql = StrSql + ", Sum(convert(float,Allowance6)) AS A6 ,  Sum(convert(float,Allowance7)) AS A7  ,  Sum(convert(float,Allowance8)) AS A8 ";
            StrSql = StrSql + ", Sum(convert(float,Allowance9)) AS A9 ,  Sum(convert(float,Allowance10)) AS A10  ";

            StrSql = StrSql + " , Sum(convert(float,  (Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6 + Allowance7 - Cur_DedCut_Pay) + tbl_ClosePay_04_Mod.SumAllAllowance_Be_Not_Sum  + tbl_ClosePay_04_Mod.Etc_Pay - Isnull(tbl_ClosePay_04_Mod.Cur_DedCut_Pay_DED,0)   )) S1 ";
            StrSql = StrSql + " , Sum(InComeTax) S2 , Sum(ResidentTax) S3 , Sum(convert(float,TruePayment)) S4 , Sum(convert(float,Etc_Pay)) S5 , Sum(convert(float,Cur_DedCut_Pay)) S6 ,  Sum(Convert(Money ,Cur_DedCut_Pay_DED)) S7";
            StrSql = StrSql + " , Mbid,Mbid2   ";
            StrSql = StrSql + " From tbl_ClosePay_04_Mod (nolock)  ";
            StrSql = StrSql + " Where  ToEndDate <> '' ";
            StrSql = StrSql + " And  TruePayment > 0  ";
            //StrSql = StrSql + " And   
            //StrSql = StrSql + " And    ( (Allowance1 + Allowance2 + Allowance3 +Allowance4 + Allowance5 + Allowance6 + Allowance7 - Cur_DedCut_Pay) + tbl_ClosePay_04_Mod.SumAllAllowance_Be_Not_Sum  + tbl_ClosePay_04_Mod.Etc_Pay - Isnull(tbl_ClosePay_04_Mod.Cur_DedCut_Pay_DED,0)    > 0 ";
            // StrSql = StrSql + "     OR  Allowance1 >0  OR Allowance2 > 0  Or Allowance3 >0 Or Allowance5 >0  Or Allowance6 >0 Or InComeTax > 0  Or ResidentTax > 0  Or TruePayment > 0  ) ";

            Tsql = StrSql;

            Make_Base_Query_(ref Tsql);

            Tsql = Tsql + " Group by Mbid, Mbid2 ";
            Tsql = Tsql + " ) B";
            Tsql = Tsql + "  Where a.Mbid = b.Mbid And a.Mbid2 = b.Mbid2";
        }


        private void Make_Base_Query_Pay_100(ref string Tsql)
        {
            string StrSql = "";

            StrSql = "Update #T2_Mem SET ";
            StrSql = StrSql + "  Pay21 = ISNULL(B.A1,0 ) ";
            //StrSql = StrSql + " , Pay22 = ISNULL(B.A2,0 )  ";
            //StrSql = StrSql + " , Pay23 = ISNULL(B.A3,0 )  ";
            ////StrSql = StrSql + " , Pay24 = ISNULL(B.A4,0 ) " ;
            ////StrSql = StrSql + " , Pay25 = ISNULL(B.A5,0 ) ";
            StrSql = StrSql + " , SumAllAllowance = SumAllAllowance + ISNULL(B.S1,0 )  ";
            StrSql = StrSql + " , InComeTax = InComeTax + ISNULL(B.S2,0 ) ";
            StrSql = StrSql + " , ResidentTax = ResidentTax + ISNULL(B.S3,0 ) ";
            StrSql = StrSql + " , TruePayment = TruePayment + ISNULL(B.S4,0 ) ";
            StrSql = StrSql + " , Etc_Pay = Etc_Pay + ISNULL(B.S5,0 ) ";
            StrSql = StrSql + " FROM  #T2_Mem  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql + " Select ";
            StrSql = StrSql + "  Sum(convert(float,Allowance1)) AS A1 ,  Sum(convert(float,Allowance2)) AS A2  ";
            StrSql = StrSql + " , Sum(convert(float,SumAllAllowance)) S1 ,  Sum(InComeTax) S2 , Sum(ResidentTax) S3 , Sum(convert(float,TruePayment)) S4 , Sum(convert(float,Etc_Pay)) S5 ";
            StrSql = StrSql + " , Mbid,Mbid2   ";
            StrSql = StrSql + " From tbl_ClosePay_100_Mod (nolock)  ";
            StrSql = StrSql + " Where  ToEndDate <> '' ";
            StrSql = StrSql + " And    SumAllAllowance > 0  ";

            Tsql = StrSql;

            Make_Base_Query_(ref Tsql);

            Tsql = Tsql + " Group by Mbid, Mbid2 ";
            Tsql = Tsql + " ) B";
            Tsql = Tsql + "  Where a.Mbid = b.Mbid And a.Mbid2 = b.Mbid2";
        }




        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = "";

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
                        strSql = strSql + " And Mbid ='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And Mbid2 = " + Mbid2;
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
                        strSql = strSql + " And Mbid >='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And Mbid2 >= " + Mbid2;
                }

                if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And Mbid <='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And Mbid2 <= " + Mbid2;
                }
            }


            //회원명으로 검색
            if (txtName.Text.Trim() != "")
                strSql = strSql + " And M_Name Like '%" + txtName.Text.Trim() + "%'";



            if (combo_W_Code_1.Text != "" && combo_W_Code_2.Text == "")
            {
                strSql = strSql + " And  tbl_WeekCount.WEEKSEQ = " + combo_W_Code_1.Text;
            }

            if (combo_W_Code_1.Text != "" && combo_W_Code_2.Text != "")
            {
                strSql = strSql + " And  tbl_WeekCount.WEEKSEQ >= " + combo_W_Code_1.Text;
                strSql = strSql + " And  tbl_WeekCount.WEEKSEQ <= " + combo_W_Code_2.Text;
            }



            if ((mtxtFromDate1.Text.Replace("-", "").Trim() != "") && (mtxtFromDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And FromEndDAte = '" + mtxtFromDate1.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtFromDate1.Text.Replace("-", "").Trim() != "") && (mtxtFromDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And FromEndDAte >= '" + mtxtFromDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And FromEndDate <= '" + mtxtFromDate2.Text.Replace("-", "").Trim() + "'";
            }


            //기록일자로 검색 -1
            if ((mtxtToDate1.Text.Replace("-", "").Trim() != "") && (mtxtToDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And ToEndDate = '" + mtxtToDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtToDate1.Text.Replace("-", "").Trim() != "") && (mtxtToDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And ToEndDate >= '" + mtxtToDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And ToEndDate <= '" + mtxtToDate2.Text.Replace("-", "").Trim() + "'";
            }


            //기록일자로 검색 -1
            if ((mtxtPayDate1.Text.Replace("-", "").Trim() != "") && (mtxtPayDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And PayDate = '" + mtxtPayDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtPayDate1.Text.Replace("-", "").Trim() != "") && (mtxtPayDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And PayDate >= '" + mtxtPayDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And PayDate <= '" + mtxtPayDate2.Text.Replace("-", "").Trim() + "'";
            }          
            
            Tsql = Tsql + strSql;           

        }




        private void Base_Grid_Set()
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();

            Make_Temp_Table(Temp_Connect, Conn);

            Make_Temp_Table_02(Temp_Connect, Conn);

            string Tsql = "";
           // if (combo_Pay_Code.Text == "2" || combo_Pay_Code.Text == "")
            //{
                //Make_Base_Query_Pay_01(ref Tsql);
                //Temp_Connect.Insert_Data(Tsql, this.Name, Conn);
           // }

            //if (combo_Pay_Code.Text == "4" || combo_Pay_Code.Text == "")
            //{
            //    Tsql = "";
            Make_Base_Query_Pay_02(ref Tsql);
            Temp_Connect.Insert_Data(Tsql, this.Name, Conn);
            //}

            //Tsql = "";
            //Make_Base_Query_Pay_03(ref Tsql);            
            //Temp_Connect.Insert_Data(Tsql, this.Name, Conn);

            //Tsql = "";
            //Make_Base_Query_Pay_100(ref Tsql);
            //Make_Base_Query_(ref Tsql);
            //Temp_Connect.Insert_Data(Tsql, this.Name, Conn);

                        
            //스타보너스 ,  바이너리보너스 , 추천매칭
            Tsql = "Select  ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " mbid + '-' + Convert(Varchar,mbid2) ";
            else
                Tsql = Tsql + " mbid2 ";

            Tsql = Tsql + " ,M_Name  , TGrade , Cpno , '' ";         

            Tsql = Tsql +  ", Pay1 , Pay2 , Pay3 , Pay4 , Pay5  " ;
            Tsql = Tsql +  ", Pay6 , Pay7 , Pay8 , Pay9 , Pay10  ";    
            Tsql = Tsql +  " ,Pay11 , Pay12 , Pay13 , Pay14 , Pay15  ";;
            Tsql = Tsql +  ", Pay16 , Pay17 , Pay18 , Pay19 , Pay20  ";
            Tsql = Tsql + ", Pay21 , Pay22 , Pay23 , Etc_Pay_DedCut , Cur_DedCut_Pay  ";

            Tsql = Tsql + ", Etc_Pay , SumAllAllowance , InComeTax , ResidentTax , TruePayment  ";
                        
            Tsql = Tsql + ", PayStop_Date, hometel , hptel , addcode1 , T_address1 ";
            Tsql = Tsql + " , BankCode , bankname , bankaccnt , bankowner , BusName ";
          
            Tsql = Tsql + " From #T2_Mem ";
            Tsql = Tsql + " Where SumAllAllowance > 0 ";
            Tsql = Tsql + "  OR Pay1 >0 Or Pay2 >0 Or Pay3 > 0 or Pay4 > 0  Or Pay5 > 0 ";
            Tsql = Tsql + " Or Pay6 > 0  Or Pay7 > 0  Or Pay8 > 0  Or Pay9 > 0   ";
            Tsql = Tsql + " Or InComeTax > 0  Or ResidentTax > 0  Or TruePayment > 0 OR Etc_Pay_DedCut <> 0 Or Cur_DedCut_Pay <> 0 Or Etc_Pay <> 0 ";

            //++++++++++++++++++++++++++++++++            

            DataSet ds = new DataSet();
            ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, Conn, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            
            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            //Dictionary<string, double> SelType_1 = new Dictionary<string, double>();
            //Dictionary<string, double> Center_Pr = new Dictionary<string, double>();


            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
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

            ////Reset_Chart_Total(Sum_13, Sum_14, Sum_15, Sum_17);
            ////Reset_Chart_Total(ref SelType_1);
            ////Reset_Chart_Total(Sell_Cnt_1, Sell_Cnt_2);


            ////foreach (string tkey in Center_Pr.Keys)
            ////{
            ////    Push_data(series_Item, tkey, Center_Pr[tkey]);
            ////}


            

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();


            if (gr_dic_text.Count > 0)
            {
                put_Sum_Dataview(ds, ReCnt);
            }


            Tsql = "Drop Table #T2_Mem";
            Temp_Connect.Insert_Data(Tsql, this.Name, Conn);

            Conn.Close(); Conn.Dispose();

        }



        private void dGridView_Base_Header_Reset()
        {

            //Sum_dic["첫팩주문보너스"] = 0;
            //Sum_dic["멘토보너스"] = 0;
            //Sum_dic["비즈니스개발보너스"] = 0;
            //Sum_dic["유니레벨보너스"] = 0;
            //Sum_dic["사이드볼륨인피티보너스"] = 0;


            //Sum_dic["리더체크매치보너스"] = 0;
            //Sum_dic["랭크업보너스"] = 0;
            //Sum_dic["글로벌풀보너스"] = 0;

            cgb.grid_col_Count = 45;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 3;
            //  cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //스타보너스 ,  바이너리보너스 , 멘토보너스
            string[] g_HeaderText = {"회원번호", "성명", "현직급"  ,"주민번호"  ,""  
                                , "첫팩주문보너스"     , "멘토보너스"  , "비즈니스개발보너스"   , "유니레벨보너스"    , "사이드볼륨인피티보너스"
                                ,"리더체크매치보너스","랭크업보너스","글로벌풀보너스","", ""  
                                ,   "", ""   , ""  , ""   , ""  
                                 , "" , ""     , ""  , ""         , ""  

                                 , "" , ""     , ""     , ""   , "반품공제" 
                                
                                                                
                                , "기타보너스"  , "지급수당합" , "소득세"  , "주민세"  , "실지급액"  
                                , "_수당중지일"     , "연락처1"   , "연락처2" , "우편번호"     , "주소"    

                                ,"은행코드" ,   "은행명",   "계좌번호" ,"예금주",  "센타"                                
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 100 , 100, 130, 0
                            , 100, 100   ,100, 130, 130
                            
                            
                             , 130, 100,100 , 0, 0 
                              , 0, 0,0 , 0, 0 

                             , 0, 0,0 , 0, 0 
                             , 0 ,0,0 , 0, 100 

                             , 100, 100,100 , 100, 100 
                             , 0, 100,100 , 100, 100 
                             , 100 , 100,150 , 100 , 100                              
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
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleCenter//5    
  
                               ,DataGridViewContentAlignment.MiddleRight                            
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight//10

                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  //15   

                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight                            
                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight//20


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

                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft //40

                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft //45
                              };
            cgb.grid_col_alignment = g_Alignment;


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
            gr_dic_cell_format[16 - 1] = cls_app_static_var.str_Grid_Currency_Type;
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


            cgb.grid_cell_format = gr_dic_cell_format;

        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {

            int Col_Cnt = 0;
            object[] row0 = new object[cgb.grid_col_Count];

            //string add1 = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][39].ToString()) + " " + encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][46].ToString());

            while (Col_Cnt < cgb.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                //if (Col_Cnt == 42 || Col_Cnt == 36 || Col_Cnt == 37)
                //    row0[Col_Cnt] = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt].ToString());
            if (Col_Cnt == 42)
               row0[Col_Cnt] = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt].ToString());
                //else 
                if (cls_User.gid_Cpno_V_TF == 1)
                {
                    if (Col_Cnt == 3)
                        row0[Col_Cnt] = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt].ToString(), "Cpno");
                }
                //if (Col_Cnt == 3)
                //    row0[Col_Cnt] = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt].ToString(), "Cpno");
                //////else if (Col_Cnt == 39)
                ////    row0[Col_Cnt] = add1;
                //else
                //    row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;

        }


        private void put_Sum_Dataview(DataSet ds, int ReCnt)
        {
            Dictionary<int, object[]> gr_dic_text_Sum = new Dictionary<int, object[]>();
            Dictionary<string, Double> Sum_dic = new Dictionary<string, Double>();
            cls_form_Meth cm = new cls_form_Meth();

            Sum_dic["첫팩주문보너스"] = 0;
            Sum_dic["멘토보너스"] = 0;
            Sum_dic["비즈니스개발보너스"] = 0;
            Sum_dic["유니레벨보너스"] = 0;
            Sum_dic["사이드볼륨인피티보너스"] = 0;
            
         
            Sum_dic["리더체크매치보너스"] = 0;
            Sum_dic["랭크업보너스"] = 0;
            Sum_dic["글로벌풀보너스"] = 0;
                                 
            Sum_dic["기타보너스"] = 0;
         
            Sum_dic["반품공제액"] = 0;
            
            Sum_dic["지급수당합계"] = 0;
            Sum_dic["소득세함"] = 0;
            Sum_dic["주민세합"] = 0;
            Sum_dic["실지급액"] = 0;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Sum_dic["첫팩주문보너스"] = Sum_dic["첫팩주문보너스"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay1"].ToString());
                Sum_dic["멘토보너스"] = Sum_dic["멘토보너스"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay2"].ToString());
                Sum_dic["비즈니스개발보너스"] = Sum_dic["비즈니스개발보너스"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay3"].ToString());
                Sum_dic["유니레벨보너스"] = Sum_dic["유니레벨보너스"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay4"].ToString());
                Sum_dic["사이드볼륨인피티보너스"] = Sum_dic["사이드볼륨인피티보너스"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay5"].ToString());
             
                Sum_dic["리더체크매치보너스"] = Sum_dic["리더체크매치보너스"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay6"].ToString());
                Sum_dic["랭크업보너스"] = Sum_dic["랭크업보너스"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay7"].ToString());
                Sum_dic["글로벌풀보너스"] = Sum_dic["글로벌풀보너스"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay8"].ToString());


                //Sum_dic["리더체크매치보너스"] = Sum_dic["리더체크매치보너스"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay9"].ToString());
                //Sum_dic["슈퍼바이져매니져수당"] = Sum_dic["슈퍼바이져매니져수당"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay11"].ToString());
                //Sum_dic["매니져수당추가"] = Sum_dic["매니져수당추가"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay12"].ToString());

                //Sum_dic["직원급여-팀장"] = Sum_dic["직원급여-팀장"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay10"].ToString());
                //Sum_dic["직원급여-국장"] = Sum_dic["직원급여-국장"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay11"].ToString());
                //Sum_dic["직원급여-상무"] = Sum_dic["직원급여-상무"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay12"].ToString());
                //Sum_dic["직원급여-전무"] = Sum_dic["직원급여-전무"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay13"].ToString());
                //Sum_dic["직원급여-부사장"] = Sum_dic["직원급여-부사장"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay14"].ToString());
                //Sum_dic["인센티브-팀장"] = Sum_dic["인센티브-팀장"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay15"].ToString());

                //Sum_dic["인센티브-국장"] = Sum_dic["인센티브-국장"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay16"].ToString());
                //Sum_dic["인센티브-상무"] = Sum_dic["인센티브-상무"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay17"].ToString());

                //Sum_dic["인센티브-전무"] = Sum_dic["인센티브-전무"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay18"].ToString());
                //Sum_dic["인센티브-부사장"] = Sum_dic["인센티브-부사장"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay19"].ToString());
                //Sum_dic["인사고과-인센"] = Sum_dic["인사고과-인센"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay20"].ToString());
                //Sum_dic["인사고과-인사"] = Sum_dic["인사고과-인사"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay21"].ToString());

                //Sum_dic["센터지원금"] = Sum_dic["센터지원금"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay22"].ToString());
                //Sum_dic["교육수당"] = Sum_dic["교육수당"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay23"].ToString());
                //Sum_dic["관리수당"] = Sum_dic["관리수당"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pay24"].ToString());
                Sum_dic["기타보너스"] = Sum_dic["기타보너스"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Etc_Pay"].ToString());
               // Sum_dic["기타공제"] = Sum_dic["기타공제"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Etc_Pay_DedCut"].ToString());


                Sum_dic["반품공제액"] = Sum_dic["반품공제액"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Cur_DedCut_Pay"].ToString());
                
                Sum_dic["지급수당합계"] = Sum_dic["지급수당합계"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumAllAllowance"].ToString());
                Sum_dic["소득세함"] = Sum_dic["소득세함"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InComeTax"].ToString());
                Sum_dic["주민세합"] = Sum_dic["주민세합"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ResidentTax"].ToString());
                Sum_dic["실지급액"] = Sum_dic["실지급액"] + Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TruePayment"].ToString());
            }

            int f_cnt = 0;
            foreach (string t_key in Sum_dic.Keys)
            {
                object[] row0 = { cm._chang_base_caption_search (t_key)
                                ,string.Format(cls_app_static_var.str_Currency_Type, Sum_dic[t_key]) 
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
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode, "", " And  (Ncode ='004' OR Ncode = '005' ) ");
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
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtPayDate1, mtxtPayDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }




























    }
}
