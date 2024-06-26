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
    public partial class frmClose_Sham_Pay_Ded_Select : clsForm_Extends
    {
      
         private const string base_db_name = "tbl_DB";
        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Sum = new cls_Grid_Base();

        private int Data_Set_Form_TF = 0;
        private int Form_Load_TF = 0;

        /// <summary>
        /// 생성자
        /// </summary>
        public frmClose_Sham_Pay_Ded_Select()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form 크기 변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Select.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_Excel.Left = butt_Select.Left + butt_Select.Width + 2;
            butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;
            butt_Print.Left = butt_Exit.Left - butt_Print.Width - 2;

            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Select);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);
            cfm.button_flat_change(butt_Print);
        }

        /// <summary>
        /// Form 로딩시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Form_Load_TF = 0;
            Data_Set_Form_TF = 0;
            dGridView_Base.Dock = DockStyle.Fill;

            Put_Rec_Code_ComboBox(combo_W_1, combo_W_Code_1);
            Put_Rec_Code_ComboBox(combo_W_2, combo_W_Code_2);
            Put_Rec_Code_ComboBox(combo_W_3, combo_W_Code_3, combo_W_Code_3_EndDate);

            cls_Grid_Base cgb_P1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();

            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            mtxtSellDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
            mtxtMbid.Focus();
        }

        /// <summary>
        /// Form 활성화시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_Base_Activated(object sender, EventArgs e)
        {
            this.Refresh();

            if (Form_Load_TF == 0)
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                Form_Load_TF = 1;
            }

            if (cls_User.uSearch_MemberNumber != "")
            {
                Data_Set_Form_TF = 1;
                mtxtMbid.Text = cls_User.uSearch_MemberNumber;
                cls_User.uSearch_MemberNumber = "";

                EventArgs ee1 = null; Select_Button_Click(butt_Select, ee1);

                Data_Set_Form_TF = 0;
            }
        }

        /// <summary>
        /// Form의 Key_Down 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        }
                    }
                }// end if
            }

            Button T_bt = butt_Exit;
            if (e.KeyValue == 123)
                T_bt = butt_Exit;       //닫기  F12
            else if (e.KeyValue == 113)
                T_bt = butt_Select;     //조회  F1
            else if (e.KeyValue == 115)
                T_bt = butt_Delete;     //삭제  F4
            else if (e.KeyValue == 119)
                T_bt = butt_Excel;      //엑셀  F8    
            else if (e.KeyValue == 112)
                T_bt = butt_Clear;      //초기화  F5    

            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123 || e.KeyValue == 119 || e.KeyValue == 112)
                    Base_Button_Click(T_bt, ee1);
                if (e.KeyValue == 113)
                    Select_Button_Click(T_bt, ee1);
            }
        }

        private void Put_Rec_Code_ComboBox(ComboBox cb_1, ComboBox cb_1_Code, int Ga_FLAG = 0)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;

            string NN_Date = cls_User.gid_date_time;
            int ReCnt = 0;

            Tsql = "Select WeekSeq, StartDate, EndDate ";
            Tsql = Tsql + " From tbl_WeekCount (nolock) ";
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

        private void Put_Rec_Code_ComboBox(ComboBox cb_1, ComboBox cb_1_Code, ComboBox cb_1_Code_EndDate, int Ga_FLAG = 0)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;

            string NN_Date = cls_User.gid_date_time;
            int ReCnt = 0;

            Tsql = "Select WeekSeq, StartDate, EndDate ";
            Tsql = Tsql + " From tbl_WeekCount (nolock) ";
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
            cb_1_Code_EndDate.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                string Close_DATE = "";
                Close_DATE = ds.Tables["tbl_Base_Rec"].Rows[fi_cnt]["WeekSeq"].ToString() + "주";
                Close_DATE += " (" + ds.Tables["tbl_Base_Rec"].Rows[fi_cnt]["StartDate"].ToString();
                Close_DATE += " ~ " + ds.Tables["tbl_Base_Rec"].Rows[fi_cnt]["EndDate"].ToString() + ")";

                cb_1.Items.Add(Close_DATE);
                cb_1_Code.Items.Add(ds.Tables["tbl_Base_Rec"].Rows[fi_cnt]["WeekSeq"].ToString());
                cb_1_Code_EndDate.Items.Add(ds.Tables["tbl_Base_Rec"].Rows[fi_cnt]["EndDate"].ToString().Replace("-", ""));
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }

        private void Select_Button_Click(object sender, EventArgs e)
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            cls_Grid_Base cgb_P1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();

            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            combo_W_Code_1.SelectedIndex = combo_W_1.SelectedIndex;
            combo_W_Code_2.SelectedIndex = combo_W_2.SelectedIndex;
            combo_W_Code_3.SelectedIndex = combo_W_3.SelectedIndex;
            combo_W_Code_3_EndDate.SelectedIndex = combo_W_3.SelectedIndex;

            if (combo_W_Code_1.Text != "" && combo_W_Code_2.Text != "")
            {
                if (int.Parse(combo_W_Code_1.Text) > int.Parse(combo_W_Code_2.Text))
                {
                    MessageBox.Show("조회 종료주가 조회 시작주보다 빠르게 설정되어 있습니다."
                           + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    combo_W_1.Focus();
                    return;
                }
            }

            if (Check_TextBox_Error() == false) return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

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

                cls_Grid_Base cgb_P1 = new cls_Grid_Base();
                dGridView_Base_Header_Reset(cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_P1.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtMbid);
            }
            else if (bt.Name == "butt_Excel")
            {
                frmBase_Excel e_f = new frmBase_Excel();
                e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
                e_f.ShowDialog();
            }
            else if (bt.Name == "butt_Print")
            {
                butt_UnpaidAmount_Excel();
            }
            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }

        }

        /// <summary>
        /// 엑셀 다운로드시
        /// </summary>
        /// <param name="Excel_Export_From_Name"></param>
        /// <param name="Excel_Export_File_Name"></param>
        /// <returns></returns>
        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            cls_form_Meth cm = new cls_form_Meth();
            Excel_Export_File_Name = this.Text; // cm._chang_base_caption_search("판매_내역_마감_역추적");
            Excel_Export_From_Name = this.Name;
            return dGridView_Base_Sell;
        }

        /// <summary>
        /// 엑셀 다운로드시
        /// </summary>
        /// <param name="Excel_Export_From_Name"></param>
        /// <param name="Excel_Export_File_Name"></param>
        /// <returns></returns>
        private DataGridView e_f_Send_Export_Excel_Info2(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            cls_form_Meth cm = new cls_form_Meth();
            Excel_Export_File_Name = this.Text; // cm._chang_base_caption_search("판매_내역_마감_역추적");
            Excel_Export_From_Name = this.Name;
            return dGridView_Print;
        }

        /// <summary>
        /// TextBox 에러 체크
        /// </summary>
        /// <returns></returns>
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

            return true;
        }

        /// <summary>
        /// 조회 쿼리 생성
        /// </summary>
        /// <param name="Tsql">반환할 쿼리</param>
        private void Make_Base_Query(ref string Tsql)
        {
            cls_form_Meth cm = new cls_form_Meth();

            Tsql = " Select ";
            Tsql = Tsql + "  CASE When C.OrderNumber ='' Then '수당공제' ELSE '반품반영' End ";
            Tsql = Tsql + ", CASE ";
            Tsql = Tsql + " When C.SortOrder ='W3'  Then '우대고객커미션' ";
            Tsql = Tsql + " When C.SortOrder ='W2'  Then '팀커미션' ";
            Tsql = Tsql + " When C.SortOrder ='W4'  Then '승급보너스' ";
            Tsql = Tsql + " When C.SortOrder ='W6'  Then '유지보너스' ";
            Tsql = Tsql + " End";
            Tsql = Tsql + " ,tbl_WeekCount.WEEKSEQ ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", C_Mbid + '-' + Convert(Varchar,C_Mbid2) ";
            else
                Tsql = Tsql + ", C_Mbid2 ";

            Tsql = Tsql + " ,C.C_M_Name ";
            Tsql = Tsql + " , CASE WHEN ISNULL(TM2.LeaveDate, '') <> '' THEN LEFT(TM2.LeaveDate, 4) + '-' + SUBSTRING(TM2.LeaveDate, 5, 2) + '-' + SUBSTRING(TM2.LeaveDate, 7, 2) ELSE '' END ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", Case When C.OrderNumber <> '' Then R_Mbid + '-' + Convert(Varchar,R_Mbid2) ELSE '' END";
            else
                Tsql = Tsql + ", Case When C.OrderNumber <> '' Then  Convert(Varchar,R_Mbid2) ELSE '' END ";

            Tsql = Tsql + " , Case When C.OrderNumber <> '' Then R_M_Name ELSE '' END";
            Tsql = Tsql + ", LEFT(C.SellDate,4) +'-' + LEFT(RIGHT(C.SellDate,4),2) + '-' + RIGHT(C.SellDate,2)";
            Tsql = Tsql + ", C.OrderNumber,C.Re_BaseOrderNumber ";
            Tsql = Tsql + ", Case When C.OrderNumber <> '' Then Return_Pay ELSE 0 END  ";
            Tsql = Tsql + " ,Return_Pay2  ";
            Tsql = Tsql + " ,Case When C.OrderNumber <> '' Then Return_Pay -Return_Pay2 ELSE 0 END  ";

            if (combo_W_Code_3.Text == "")
                Tsql = Tsql + " ,0 ";
            else
            {
                Tsql = Tsql + " ,Isnull(( ";
                Tsql = Tsql + "        Select Sum(Return_Pay) From tbl_Sales_Put_Return_Pay_DED CC_1 (nolock)  ";
                Tsql = Tsql + "        Where CC_1.ToEndDate ='" + combo_W_Code_3_EndDate.Text + "'  ";
                Tsql = Tsql + "        And   CC_1.Base_T_index >0  ";
                Tsql = Tsql + "        And   CC_1.Base_T_index =  C.T_index ";
                Tsql = Tsql + "  ),0) ";
            }

            Tsql = Tsql + " ,b.name ";
            Tsql = Tsql + ",Case When C.ToEndDate >= '20181015' And ( C.SortOrder ='W4' Or  C.SortOrder ='W6') Then Isnull( Ret_week.WEEKSEQ ,0)  ELSE '' END ";
            Tsql = Tsql + ",C.T_index ";
            Tsql = Tsql + " ,'' ,'', ''  ";
            Tsql = Tsql + " From tbl_Sales_Put_Return_Pay_DED C  (nolock) ";
            Tsql = Tsql + " Left Join tbl_Memberinfo  (nolock) On  C.R_Mbid = mbid And   C.R_Mbid2 = mbid2 ";
            Tsql = Tsql + " Left Join tbl_SalesDetail  (nolock) On tbl_SalesDetail.OrderNumber =  C.OrderNumber  ";
            Tsql = Tsql + " Left Join tbl_Business AS B  (nolock) On tbl_Memberinfo.businesscode=B.ncode  ";
            Tsql = Tsql + " Left Join tbl_SellType  (nolock) On tbl_SellType.SellCode=tbl_SalesDetail.SellCode ";// And tbl_SellType.Na_Code = tbl_SalesDetail.Na_Code" ; 
            Tsql = Tsql + " LEFT JOIN tbl_WeekCount (nolock) ON C.ToEndDate BETWEEN tbl_WeekCount.STARTDATE AND tbl_WeekCount.ENDDATE ";
            Tsql = Tsql + " LEFT JOIN tbl_WeekCount (nolock) Ret_week ON Ret_week.ENDDATE = C.SellDate   ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (NOLOCK) TM2 ON C.C_mbid = TM2.mbid AND C.C_mbid2 = TM2.mbid2 ";
        }

        /// <summary>
        /// 조회 쿼리 생성2(조건)
        /// </summary>
        /// <param name="Tsql">반환할 쿼리</param>
        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = "  Where C.T_index > 0 And SortOrder  <> 'W3_C'  "; //W3_C 은 50만원 초과분에 대해서는 받은게 없기 때문에 깍을때도 그 이상을 받앗으면 깍지를 않는다 ..

            if (combo_W_Code_1.Text != "" && combo_W_Code_2.Text == "")
            {
                strSql = strSql + " And  tbl_WeekCount.WEEKSEQ = " + combo_W_Code_1.Text;
            }
            else if (combo_W_Code_1.Text != "" && combo_W_Code_2.Text != "")
            {
                strSql = strSql + " And  tbl_WeekCount.WEEKSEQ >= " + combo_W_Code_1.Text;
                strSql = strSql + " And  tbl_WeekCount.WEEKSEQ <= " + combo_W_Code_2.Text;
            }

            string Mbid = ""; int Mbid2 = 0;

            //회원번호1로 검색
            if ((mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != ""))
            {
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
                {
                    strSql = strSql + " And C_Mbid = '" + Mbid + "'";
                    strSql = strSql + " And C_Mbid2 = " + Mbid2;
                }
            }
            //회원번호2로 검색
            if ((mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() != ""))
            {
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    strSql = strSql + " And R_Mbid = '" + Mbid + "'";
                    strSql = strSql + " And R_Mbid2 = " + Mbid2;
                }
            }

            //회원명으로 검색
            if (txtName.Text.Trim() != "")
                strSql = strSql + " And C.C_M_Name Like '%" + txtName.Text.Trim() + "%'";

            if (txtName2.Text.Trim() != "")
                strSql = strSql + " And C.R_M_Name Like '%" + txtName2.Text.Trim() + "%'";

            //가입일자로 검색 -1
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And C.ToEndDate = '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Trim() != ""))
            {
                strSql = strSql + " And C.ToEndDate >= '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And C.ToEndDate <= '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";
            }

            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

            Tsql = Tsql + strSql;
            Tsql = Tsql + " Order by C.T_index DESC  ";
        }

        /// <summary>
        /// 메인 그리드 세팅
        /// </summary>
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

            cls_form_Meth cm = new cls_form_Meth();
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.                                   
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }

        /// <summary>
        /// 메인 그리드 해더 세팅
        /// </summary>
        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 21;
            cgb.basegrid = dGridView_Base_Sell;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"구분"  ,"_구분"  ,"마감주차" , "회원"   , "회원명"      
                                , "회원 탈퇴일자", "_반품회원"     , "_반품자명"  , "_반품일자"   , "_주문번호"    
                                , "_반품주문번호"     , "적용액"  , "남은공제액", "처리_공제액"   , "검색주차처리공제액"  
                                , "_센타"   , "적용주차"   , "_t_inedx" , ""     , ""    
                                , ""                                                                     
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 80, 0 , 90, 90, 110
                        , 130, 0, 0   ,0, 0
                        , 0, 90 , 80  ,80 , 120
                        , 0, 80, 0,0 , 0
                        , 0                             
                        };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                     
                                    ,true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter//5    
  
                               ,DataGridViewContentAlignment.MiddleCenter                            
                               ,DataGridViewContentAlignment.MiddleCenter                            
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleLeft //10

                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight //15   

                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter                            
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter   //20

                               ,DataGridViewContentAlignment.MiddleCenter                     
                              };
            cgb.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb.grid_cell_format = gr_dic_cell_format;
        }

        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;
            object[] row0 = new object[cgb.grid_col_Count];

            while (Col_Cnt < 21)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;
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
                    else if (mtb.Name == "mtxtTel2")
                    {
                        SW = 1;
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }
                    else if (mtb.Name == "mtxtZip1")
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
                    else if (sort_TF == "Tel")
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Tel")
                           + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    }
                    else if (sort_TF == "Zip")
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_AddCode")
                           + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    }
                    else if (sort_TF == "Date")
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

        }

        private void txtData_Base_Leave(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox tb = (TextBox)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = Color.White;
            }
            else if (sender is MaskedTextBox)
            {
                MaskedTextBox tb = (MaskedTextBox)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = Color.White;
            }
        }

        private void radioB_S_Base_Click(object sender, EventArgs e)
        {
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtSellDate1, mtxtSellDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }

        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            Data_Set_Form_TF = 0;
            // SendKeys.Send("{TAB}");
        }

        private void S_MtxtData_KeyPress(object sender, KeyPressEventArgs e)
        {

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

        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {

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
                if (tb.Text.Trim() == "")
                    txtCenter_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtCenter_Code);
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
            else if (tb.Name == "txtCenter2")
                cgb_Pop.Next_Focus_Control = butt_Select;
            else if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = butt_Select;
            else if (tb.Name == "txtR_Id")
                cgb_Pop.Next_Focus_Control = butt_Select;
            else if (tb.Name == "txtChange")
                cgb_Pop.Next_Focus_Control = butt_Select;
            else if (tb.Name == "txtSellCode")
                cgb_Pop.Next_Focus_Control = butt_Select;
            else if (tb.Name == "txt_Base_Rec")
                cgb_Pop.Next_Focus_Control = butt_Select;
            else if (tb.Name == "txt_Receive_Method")
                cgb_Pop.Next_Focus_Control = butt_Select;
            else if (tb.Name == "txt_ItemCode")
                cgb_Pop.Next_Focus_Control = butt_Select;
            else if (tb.Name == "txt_ItemName2")
                cgb_Pop.Next_Focus_Control = butt_Select;
            else if (tb.Name == "txtCenter4")
                cgb_Pop.Next_Focus_Control = butt_Select;
            else if (tb.Name == "txtIO")
            {
                cgb_Pop.Next_Focus_Control = butt_Select;
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode,  "", " And  (Ncode ='004' OR Ncode = '005' ) ");
            }
            else
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode, "");
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
                if ((tb.Name == "txtCenter") || (tb.Name == "txtCenter2"))
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);
                else if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);
                else if (tb.Name == "txtBank")
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);
                else if (tb.Name == "txt_ItemName2")
                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", strSql);
            }
            else
            {
                if ((tb.Name == "txtCenter") || (tb.Name == "txtCenter2"))
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                }
                else if (tb.Name == "txtR_Id")
                {
                    string Tsql;
                    Tsql = "Select user_id ,U_Name   ";
                    Tsql = Tsql + " From tbl_User (nolock) ";
                    Tsql = Tsql + " Order by user_id ";

                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", Tsql);
                }
                else if (tb.Name == "txtBank")
                {
                    string Tsql;
                    Tsql = "Select Ncode ,BankName    ";
                    Tsql = Tsql + " From tbl_Bank (nolock) ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);
                }
                else if (tb.Name == "txt_ItemName2")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name    ";
                    Tsql = Tsql + " From tbl_Goods (nolock) ";
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

            if ((tb.Name == "txtCenter") || (tb.Name == "txtCenter2"))
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where (Ncode like '%" + tb.Text.Trim() + "%'";
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

        /// <summary>
        /// 마스터 그리드 더블클릭했을 때 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dGridView_Base_Sell_DoubleClick(object sender, EventArgs e)
        {
            cls_Grid_Base cgb_P1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();

            //"회원번호", "성명", "마감_시작일"  ,"마감_종료일"  ,"지급_일자"  
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                int T_index = int.Parse((sender as DataGridView).CurrentRow.Cells[17].Value.ToString());

                dGridView_Base_Header_Reset(cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_P1.d_Grid_view_Header_Reset();
                Real_Allowance_Detail_Up(T_index, cgb_P1);  //매출친상품에 대한 역추적이 이루어지는 곳임
            }
        }

        /// <summary>
        /// 우측 화면 그리드 세팅(메인그리드 더블클릭했을 때 호출)
        /// </summary>
        /// <param name="T_index"></param>
        /// <param name="cgb_P"></param>
        private void Real_Allowance_Detail_Up(int T_index, cls_Grid_Base cgb_P)
        {
            string StrSql = "";

            StrSql = "Select LEFT(ToEndDate,4) +'-' + LEFT(RIGHT(ToEndDate,4),2) + '-' + RIGHT(ToEndDate,2), Return_Pay ,   '주마감'  ";
            StrSql = StrSql + " ,'','' ";
            StrSql = StrSql + " From tbl_Sales_Put_Return_Pay_DED (nolock) ";
            StrSql = StrSql + " Where Base_T_index  = " + T_index;
            StrSql = StrSql + "  Order BY T_index ";

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

        /// <summary>
        /// 우측 그리드 해더 세팅
        /// </summary>
        /// <param name="cgb_P"></param>
        private void dGridView_Base_Header_Reset(cls_Grid_Base cgb_P)
        {
            cgb_P.grid_col_Count = 5;
            cgb_P.basegrid = dGridView_Base;
            cgb_P.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_P.grid_Frozen_End_Count = 3;
            cgb_P.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"공제적용마감일자", "공제적용금액", "마감구분"  ,""  ,""                              
                                };
            cgb_P.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 100, 100 , 100, 0, 0                                                       
                        };
            cgb_P.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                                                         
                                   };
            cgb_P.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter//5    
                     
                              };
            cgb_P.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[2 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb_P.grid_cell_format = gr_dic_cell_format;
        }

        private void Set_Up_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4] 
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }

        #region 엑셀 다운로드

        /// <summary>
        /// 판매인 미수금내역 엑셀 다운로드
        /// </summary>
        private void butt_UnpaidAmount_Excel()
        {
            combo_W_Code_1.SelectedIndex = combo_W_1.SelectedIndex;

            if (combo_W_Code_1.Text.Trim() == "")
            {
                MessageBox.Show("주차를 선택하시기 바랍니다.");
                combo_W_1.Focus();
                return;
            }

            try
            {
                string Tsql = "";
                Tsql = " EXEC Usp_CS_Print_ReturnAllowance " + combo_W_Code_1.Text.Trim();

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                DataSet ds = new DataSet();

                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.\
                if (Temp_Connect.Open_Data_Set(Tsql, "printData", ds, this.Name, this.Text, 1) == false)
                {
                    MessageBox.Show(this, "데이터가 존재하지 않습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_미수금_내역.xls";
                savefile.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    app.Visible = false;
                    app.DisplayAlerts = false;

                    Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                    Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.ActiveSheet;

                    ws.get_Range("A1", "E1").ColumnWidth = 20;

                    //해더
                    ws.get_Range("A2", "E2").MergeCells = true;
                    ws.get_Range("A2", "E2").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    ws.Cells[2, 1] = "반품 수당 현황";

                    ws.Cells[4, 1] = "기준일자(주차)";
                    ws.Cells[4, 2] = combo_W_1.Text;
                    ws.Cells[4, 4] = "출력일자";
                    ws.Cells[4, 5] = DateTime.Now.ToString("yyyy-MM-dd");

                    bool header = false;

                    int rowIndex = 8;

                    foreach (DataTable dt in ds.Tables)
                    {
                        if (!header) //DataTable의 해더(첫번째 DataTable만 입력)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                ws.Cells[7, i + 1] = dt.Columns[i].ColumnName;
                                header = true;
                            }
                        }

                        // Content.  
                        foreach (DataRow dr in dt.Rows)
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                ws.Cells[rowIndex, j + 1] = dr[j].ToString();
                                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)ws.Cells[rowIndex, j + 1];
                                rng.NumberFormat = "#,##0";
                            }
                            rowIndex++;
                        }
                    }

                    // Lots of options here. See the documentation.  
                    wb.SaveAs(savefile.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                                                 false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                                                 Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    wb.Close();
                    app.Quit();

                    MessageBox.Show("저장을 완료하였습니다.", "알림", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


























    }
}
