using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLM_Program
{
    public partial class frmClose_Member_Up_01 : Form
    {
        private const string base_db_name = "tbl_DB";
        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Sum = new cls_Grid_Base();

        private int Data_Set_Form_TF = 0;

        private int Form_Load_TF = 0;


        public frmClose_Member_Up_01()
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
            dGridView_Base.Dock = DockStyle.Fill;


            cls_Grid_Base_info_Put cgbp10 = new cls_Grid_Base_info_Put();
            cgbp10.dGridView_Put_baseinfo(dGridView_Up_Sa, "saveup");

            cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
            cgbp11.dGridView_Put_baseinfo(dGridView_Up_N, "nominup");

            cls_Grid_Base cgb_P1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();

            tab_Detail_02.SelectedIndex = 0;

            
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

            mtxtSellDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;

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

                //dGridView_Sum_Base_Header_Reset();
                //cgb_Sum.d_Grid_view_Header_Reset(1);
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                Form_Load_TF = 1;
            }

            if (cls_User.uSearch_MemberNumber != "")
            {
                Data_Set_Form_TF = 1;
                mtxtMbid.Text = cls_User.uSearch_MemberNumber;
                //mtxtSMbid.Text = cls_User.uSearch_MemberNumber;
                cls_User.uSearch_MemberNumber = "";

                //EventArgs ee1 = null; Base_Button_Click(butt_Select, ee1);
                EventArgs ee1 = null; Select_Button_Click(butt_Select, ee1);
                
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
                if (e.KeyValue == 123 || e.KeyValue == 119 || e.KeyValue == 112)
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

            cls_Grid_Base_info_Put cgbp10 = new cls_Grid_Base_info_Put();
            cgbp10.dGridView_Put_baseinfo(dGridView_Up_Sa, "saveup");

            cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
            cgbp11.dGridView_Put_baseinfo(dGridView_Up_N, "nominup");

            cls_Grid_Base cgb_P1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();

           // dGridView_Sum_Base_Header_Reset();
            //cgb_Sum.d_Grid_view_Header_Reset(1);

            //Clear_Pay_Detail();
            tab_Detail_02.SelectedIndex = 0;
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

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


                cls_Grid_Base_info_Put cgbp10 = new cls_Grid_Base_info_Put();
                cgbp10.dGridView_Put_baseinfo(dGridView_Up_Sa, "saveup");

                cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
                cgbp11.dGridView_Put_baseinfo(dGridView_Up_N, "nominup");

                cls_Grid_Base cgb_P1 = new cls_Grid_Base();
                dGridView_Base_Header_Reset(cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_P1.d_Grid_view_Header_Reset();
                //dGridView_Sum_Base_Header_Reset();
                //cgb_Sum.d_Grid_view_Header_Reset(1);

                //Clear_Pay_Detail();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                
                tab_Detail_02.SelectedIndex = 0;

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtMbid);


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
            Excel_Export_File_Name = this.Text; // cm._chang_base_caption_search("판매_내역_마감_역추적");
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;


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




        private void Make_Base_Query(ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();

            Tsql = "Select  ";

            Tsql = Tsql + " Case When tbl_SalesDetail.Ga_ORder = 0 Then '" + cm._chang_base_caption_search("승인") + "'";
            Tsql = Tsql + "  When tbl_SalesDetail.Ga_ORder >=1  Then '" + cm._chang_base_caption_search("미승인") + "'";
            Tsql = Tsql + " END SellTFName ";

            Tsql = Tsql + " , tbl_SalesDetail.OrderNumber  ";

            //Tsql = Tsql + " , Case When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE = '0000' Then ISNULL(T_REALMLM.GUARANTE_NUM,'') ";
            //Tsql = Tsql + "        When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE <> '0000' Then  ISNULL(T_REALMLM_ErrCode.Er_Msg ,'' ) ";
            //Tsql = Tsql + "        When  tbl_SalesDetail.union_Seq = 0 Then '미신고'  ";
            //Tsql = Tsql + "   End  ";


            if (cls_app_static_var.Sell_Union_Flag == "U")  //직판
            {
                Tsql = Tsql + " , Case When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE = '0000' Then ISNULL(T_REALMLM.GUARANTE_NUM,'') ";
                Tsql = Tsql + "        When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE <> '0000' Then  ISNULL(T_REALMLM_ErrCode.Er_Msg ,'' ) ";
                //Tsql = Tsql + "        When  tbl_SalesDetail.union_Seq = 0 Then '미신고'  ";
                Tsql = Tsql + "        When  tbl_SalesDetail.union_Seq = 0 Then '" + cm._chang_base_caption_search("미신고") + "'  ";
                
                Tsql = Tsql + "   End  ";
            }
            else if (cls_app_static_var.Sell_Union_Flag == "D")  //직판
            {
                //Tsql = Tsql + ", Case When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NULL And tbl_SalesDetail.InsuranceNumber <> '' Then tbl_SalesDetail.InsuranceNumber ";
                //Tsql = Tsql + " When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                //Tsql = Tsql + " When  ReturnTF = 5 And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                //Tsql = Tsql + " When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel ='' Then tbl_SalesDetail.InsuranceNumber + '(취소요청중)' ";
                //Tsql = Tsql + " When  ReturnTF = 1 And tbl_SalesDetail.InsuranceNumber = '' Then '미승인요청' ";
                //Tsql = Tsql + " ELSE tbl_SalesDetail.InsuranceNumber END  ";

                //Tsql = Tsql + ", Case When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail  (nolock)  AS A1 Where A1.Re_BaseOrderNumber <> '' And tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NULL And tbl_SalesDetail.InsuranceNumber <> '' Then tbl_SalesDetail.InsuranceNumber ";
                //Tsql = Tsql + " When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail  (nolock)  AS A1 Where A1.Re_BaseOrderNumber <> '' And tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                //Tsql = Tsql + " When  ReturnTF = 5 And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                //Tsql = Tsql + " When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail (nolock) AS A1 Where A1.Re_BaseOrderNumber <> '' And  tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber ) IS NOT NULL And InsuranceNumber_Cancel ='' Then tbl_SalesDetail.InsuranceNumber + '(취소요청중)' ";
                //Tsql = Tsql + " When  ReturnTF = 2 then '반품처리' ";
                //Tsql = Tsql + " When  ReturnTF = 1 And tbl_SalesDetail.InsuranceNumber = '' Then '재발급요청요망' + ' ' + tbl_SalesDetail.INS_Num_Err  ";
                //Tsql = Tsql + " ELSE tbl_SalesDetail.InsuranceNumber END  ";


                //Tsql += ", Case When  ReturnTF = 1 And (Select top 1  A1.SellDate From tbl_SalesDetail  (nolock)  AS A1 Where A1.Re_BaseOrderNumber <> '' And tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber Order by OrderNumber ) IS NULL And tbl_SalesDetail.InsuranceNumber <> '' Then tbl_SalesDetail.InsuranceNumber ";
                //Tsql += " When  ReturnTF = 1 And (Select top 1  A1.SellDate From tbl_SalesDetail  (nolock)  AS A1 Where A1.Re_BaseOrderNumber <> '' And tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber Order by OrderNumber ) IS NOT NULL And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                //Tsql += " When  ReturnTF = 5 And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                //Tsql += " When  ReturnTF = 1 And (Select top 1  A1.SellDate From tbl_SalesDetail (nolock) AS A1 Where A1.Re_BaseOrderNumber <> '' And  tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber And ReturnTF = 2 Order by OrderNumber  ) IS NOT NULL And InsuranceNumber_Cancel ='' Then tbl_SalesDetail.InsuranceNumber + '(취소요청중)' ";
                //Tsql += " When  ReturnTF = 1 And (Select top 1  A1.SellDate From tbl_SalesDetail (nolock) AS A1 Where A1.Re_BaseOrderNumber <> '' And  tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber And ReturnTF = 3 Order by OrderNumber  ) IS NOT NULL And InsuranceNumber_Cancel ='' Then tbl_SalesDetail.InsuranceNumber + '(부분취소요청중)' ";
                //Tsql += " When  ReturnTF = 2 then '반품처리' ";
                //Tsql += " When  ReturnTF = 3 then '부분반품처리' ";
                //Tsql += " When  ReturnTF = 1 And tbl_SalesDetail.InsuranceNumber = '' Then '재발급요청요망' + ' ' + tbl_SalesDetail.INS_Num_Err  ";

                Tsql += ", Case When  ReturnTF = 1 And (Select top 1  A1.SellDate From tbl_SalesDetail  (nolock)  AS A1 Where A1.Re_BaseOrderNumber <> '' And tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber Order by OrderNumber ) IS NULL And tbl_SalesDetail.InsuranceNumber <> '' Then tbl_SalesDetail.InsuranceNumber ";
                Tsql += " When  ReturnTF = 1 And (Select top 1  A1.SellDate From tbl_SalesDetail  (nolock)  AS A1 Where A1.Re_BaseOrderNumber <> '' And tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber Order by OrderNumber ) IS NOT NULL And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(" + cm._chang_base_caption_search("취소상태") + ")' ";
                Tsql += " When  ReturnTF = 5 And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(" + cm._chang_base_caption_search("취소상태") + ")' ";
                Tsql += " When  ReturnTF = 1 And (Select top 1  A1.SellDate From tbl_SalesDetail (nolock) AS A1 Where A1.Re_BaseOrderNumber <> '' And  tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber And ReturnTF = 2 Order by OrderNumber  ) IS NOT NULL And InsuranceNumber_Cancel ='' Then tbl_SalesDetail.InsuranceNumber + '(" + cm._chang_base_caption_search("취소요청중") + ")' ";
                Tsql += " When  ReturnTF = 1 And (Select top 1  A1.SellDate From tbl_SalesDetail (nolock) AS A1 Where A1.Re_BaseOrderNumber <> '' And  tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber And ReturnTF = 3 Order by OrderNumber  ) IS NOT NULL And InsuranceNumber_Cancel ='' Then tbl_SalesDetail.InsuranceNumber + '(" + cm._chang_base_caption_search("부분취소요청중") + ")' ";
                Tsql += " When  ReturnTF = 2 then '" + cm._chang_base_caption_search("반품처리") + "' ";
                Tsql += " When  ReturnTF = 3 then '" + cm._chang_base_caption_search("부분반품처리") + "' ";
                Tsql += " When  ReturnTF = 1 And tbl_SalesDetail.InsuranceNumber = '' Then '" + cm._chang_base_caption_search("재발급요청요망") + "' + ' ' + tbl_SalesDetail.INS_Num_Err  ";


                Tsql += " ELSE tbl_SalesDetail.InsuranceNumber END  ";

            }
            else
            {
                Tsql = Tsql + " , '' ";
            }




            Tsql = Tsql + " , Case ReturnTF When 1 then LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2)   ";
            Tsql = Tsql + "  ELSE (Select LEFT(A1.SellDate,4) +'-' + LEFT(RIGHT(A1.SellDate,4),2) + '-' + RIGHT(A1.SellDate,2) From tbl_SalesDetail AS A1 Where A1.OrderNumber = tbl_SalesDetail.Re_BaseOrderNumber)  END ";


            Tsql = Tsql + " , Case ReturnTF When 1 then '' ELSE  LEFT(SellDate,4) +'-' + LEFT(RIGHT(SellDate,4),2) + '-' + RIGHT(SellDate,2)  END ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", tbl_SalesDetail.mbid + '-' + Convert(Varchar,tbl_SalesDetail.mbid2) ";
            else
                Tsql = Tsql + ", tbl_SalesDetail.mbid2 ";

            Tsql = Tsql + " ,tbl_SalesDetail.M_Name ";

            Tsql = Tsql + ", tbl_Memberinfo.Cpno  ";

            Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";
            Tsql = Tsql + " ,Isnull(S_Bus.Name,'') as S_B_Name";

            Tsql = Tsql + " , tbl_SellType.SellTypeName SellCodeName  ";

            Tsql = Tsql + " ,TotalPrice , Totalpv  ";
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

            Tsql = Tsql + " From tbl_SalesDetail (nolock) ";            
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode  And tbl_Memberinfo.Na_code = tbl_Business.Na_code";
            Tsql = Tsql + " LEFT JOIN tbl_Business S_Bus (nolock) ON tbl_SalesDetail.BusCode = S_Bus.NCode  And tbl_SalesDetail.Na_code = S_Bus.Na_code ";
            Tsql = Tsql + " Left Join tbl_Class C1 On tbl_Memberinfo.CurGrade=C1.Grade_Cnt ";
            Tsql = Tsql + " LEFT Join tbl_SellType ON tbl_SalesDetail.SellCode = tbl_SellType.SellCode ";

            Tsql = Tsql + " LEFT JOIN T_REALMLM (nolock) ON T_REALMLM.SEQ = tbl_SalesDetail.union_Seq ";
            Tsql = Tsql + " LEFT JOIN T_REALMLM_ErrCode (nolock) ON T_REALMLM.ERRCODE = T_REALMLM_ErrCode.Er_Code ";
        }


        private void Make_Base_Query_(ref string Tsql)
        {

            string strSql = "  Where Ga_Order = 0  ";
            
                        
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
                    strSql = strSql + " And tbl_SalesDetail.Mbid = '" + Mbid + "'";
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
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Trim() != ""))
            {
                strSql = strSql + " And tbl_SalesDetail.SellDate >= '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_SalesDetail.SellDate <= '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";
            }


            strSql = strSql + " And tbl_SalesDetail.BusCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            //strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


            Tsql = Tsql + strSql ;
            Tsql = Tsql + " Order by tbl_SalesDetail.RecordTime ASC  ";
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

            cls_form_Meth cm = new cls_form_Meth();
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.                                   
            }

            

            
            if (gr_dic_text.Count > 0)
            {
              
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();     
        }



        private void dGridView_Base_Header_Reset()
        {

            cgb.grid_col_Count = 27;
            cgb.basegrid = dGridView_Base_Sell;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"승인여부"  ,"주문번호"  ,"공제번호" , "구매_일자"   , "교환_반품_일자"      
                                , "회원_번호"     , "성명"  , ""   , "등록_센타명"    , "구매_센타명"     
                                , "구매_종류"  , "총구매액", "총PV"   , "총결제액"  , "현금"   
                                , "카드"   , "무통장" , "마일리지"     , "미수금"    , "구분"     
                                , ""  , "" , "기록자"  , "기록일"  , ""  
                                ,""        , ""
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            
          
            if (cls_app_static_var.Sell_Union_Flag == "")
            {
                int[] g_Width = { 80, 130 , 0, 90, 110
                            , 90, 90   ,0, 130, 130
                            , 90 , 80  ,80 , 80, 80
                             , 80, 80,80 , 90, 130 
                             , 0 , 0,130 , 100 , 0 
                             , 0,0
                            };
                cgb.grid_col_w = g_Width;
            }
            else
            {
                int[] g_Width = { 80, 130 , 130, 90, 110
                            , 90, 90   ,0, 130, 130
                            , 90 , 80  ,80 , 80, 80
                             , 80, 80,80 , 90, 130 
                             , 0 , 0,130 , 100 , 0 
                             , 0,0
                            };
                cgb.grid_col_w = g_Width;
            }


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

            int Col_Cnt = 0;
            object[] row0 = new object[cgb.grid_col_Count];

            while (Col_Cnt < 23)
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
                if (tb.Text.Trim() == "")
                    txtCenter_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtCenter_Code);
            }

            //if (tb.Name == "txtBank")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtSellCode_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);
            //}

            //if (tb.Name == "txtR_Id")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtR_Id_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code);
            //}

            //if (tb.Name == "txtCenter2")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtCenter2_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtCenter2_Code);
            //}

            //if (tb.Name == "txtSellCode")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtSellCode_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);
            //}


            //if (tb.Name == "txt_ItemName2")
            //{
            //    if (tb.Text.Trim() == "")
            //        txt_ItemName_Code2.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txt_ItemName_Code2);
            //}
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
                //    Db_Grid_Popup(tb, txtCenter_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            //if (tb.Name == "txtCenter2")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtCenter2_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtCenter2_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

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
                if ((tb.Name == "txtCenter") || (tb.Name == "txtCenter2"))
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtBank")
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);

                if (tb.Name == "txt_ItemName2")
                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", strSql);
            }
            else
            {
                if ((tb.Name == "txtCenter") || (tb.Name == "txtCenter2"))
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

            if ((tb.Name == "txtCenter") || (tb.Name == "txtCenter2"))
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where (Ncode like '%" + tb.Text.Trim() + "%'";
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



        private void dGridView_Base_Sell_DoubleClick(object sender, EventArgs e)
        {


            cls_Grid_Base_info_Put cgbp10 = new cls_Grid_Base_info_Put();
            cgbp10.dGridView_Put_baseinfo(dGridView_Up_Sa, "saveup");

            cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
            cgbp11.dGridView_Put_baseinfo(dGridView_Up_N, "nominup");

            cls_Grid_Base cgb_P1 = new cls_Grid_Base();
            dGridView_Base_Header_Reset(cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_P1.d_Grid_view_Header_Reset();

            tab_Detail_02.SelectedIndex = 0;

            //"회원번호", "성명", "마감_시작일"  ,"마감_종료일"  ,"지급_일자"  
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string T_Mbid = "", OrderNumber = "";

                T_Mbid = (sender as DataGridView).CurrentRow.Cells[5].Value.ToString();
                OrderNumber = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
                
                cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                cgbp.dGridView_Put_baseinfo(this, dGridView_Up_Sa, "saveup", T_Mbid);

                cls_Grid_Base_info_Put cgbp_2 = new cls_Grid_Base_info_Put();
                cgbp_2.dGridView_Put_baseinfo(this, dGridView_Up_N, "nominup", T_Mbid);


                cls_Search_DB csd = new cls_Search_DB();
                string Mbid = ""; int Mbid2 = 0;
                csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);


                dGridView_Base_Header_Reset( cgb_P1); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_P1.d_Grid_view_Header_Reset();
                Real_Allowance_Detail_Up(OrderNumber, Mbid,Mbid2, cgb_P1);  //매출친상품에 대한 역추적이 이루어지는 곳임

            }
            
        }





        private void Real_Allowance_Detail_Up(string OrderNumber, string Mbid, int Mbid2, cls_Grid_Base cgb_P)
        {
            string StrSql = "";

            cls_form_Meth cm = new cls_form_Meth();
            string ST1 = cm._chang_base_caption_search("추천보너스");
            
            StrSql = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " SaveMbid + '-' + Convert(Varchar,SaveMbid2) ";
            else
                StrSql = StrSql + " SaveMbid2 ";

            StrSql = StrSql + ",SaveName, DownPV ,OrderNumber, LevelCnt , ST1 ";
            StrSql = StrSql + " From ";
            StrSql = StrSql + " ( ";
            //StrSql = StrSql + " Select EndDate, RequestMbid, RequestMbid2 ,RequestName ,OrderNumber, SaveMbid, SaveMbid2,SaveName, DownPV, LevelCnt,";

            //StrSql = StrSql + " Case ";
            //StrSql = StrSql + " SortOrder When '3'  then '" + ST1 + "'   ";
            //StrSql = StrSql + " End AS ST1 ";

            //StrSql = StrSql + " From tbl_Close_DownPV_ALL_02 (nolock)  ";
            //StrSql = StrSql + " Left Join tbl_CloseTotal_02   (nolock) On tbl_CloseTotal_02.ToEndDate= tbl_Close_DownPV_ALL_02.EndDate  ";
            //StrSql = StrSql + " Where SortOrder = '3' ";

            //StrSql = StrSql + " UNION ALL";

            StrSql = StrSql + " Select EndDate, RequestMbid, RequestMbid2 ,RequestName ,OrderNumber, SaveMbid, SaveMbid2,SaveName, Sell_DownPV DownPV, LevelCnt,";

            StrSql = StrSql + " Case ";
            StrSql = StrSql + "  When SortOrder = '1' then '후원누적' ";
            StrSql = StrSql + " End AS ST1 ";

            StrSql = StrSql + " From tbl_Close_DownPV_PV_04 (nolock)  ";
            StrSql = StrSql + " Left Join tbl_CloseTotal_04   (nolock) On tbl_CloseTotal_04.ToEndDate= tbl_Close_DownPV_PV_04.EndDate  ";
            StrSql = StrSql + " Where SortOrder = '1' ";



            StrSql = StrSql + " UNION ALL";

            StrSql = StrSql + " Select EndDate, RequestMbid, RequestMbid2 ,RequestName ,OrderNumber, SaveMbid, SaveMbid2,SaveName, DownPV, LevelCnt,";

            StrSql = StrSql + " Case ";
            StrSql = StrSql + "  When SortOrder = '1' then '첫팩주문보너스' ";
            StrSql = StrSql + "  When SortOrder = '4' then '유니레벨보너스' ";
            StrSql = StrSql + "  When SortOrder = '5' then '사이드볼륨인피니티' ";
            StrSql = StrSql + "  When SortOrder = '6' then '리더체크매치보너스' ";

            StrSql = StrSql + " End AS ST1 ";

            StrSql = StrSql + " From tbl_Close_DownPV_ALL_04 (nolock)  ";
            StrSql = StrSql + " Left Join tbl_CloseTotal_04   (nolock) On tbl_CloseTotal_04.ToEndDate= tbl_Close_DownPV_ALL_04.EndDate  ";
            StrSql = StrSql + " Where SortOrder = '6' ";

            //StrSql = StrSql + " UNION ALL";

            //StrSql = StrSql + "  Select EndDate, RequestMbid , RequestMbid2 ,RequestName , OrderNumber,   SaveMbid ,  SaveMbid2,  SaveName, Sell_DownPV  DownPV , LevelCnt,";
            //StrSql = StrSql + " '리더체크매치보너스'  AS ST1 ";
            //StrSql = StrSql + " From tbl_Close_DownPV_PV_100  (nolock) ";
            //StrSql = StrSql + " Left Join tbl_CloseTotal_100  (nolock)  On tbl_CloseTotal_100.ToEndDate= tbl_Close_DownPV_PV_100.EndDate  ";

            StrSql = StrSql + " ) AS C ";

            StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
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

        private void dGridView_Base_Header_Reset( cls_Grid_Base cgb_P)
        {

            cgb_P.grid_col_Count = 10;
            cgb_P.basegrid = dGridView_Base;
            cgb_P.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_P.grid_Frozen_End_Count = 3;
            cgb_P.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        
            string[] g_HeaderText = {"회원번호", "성명", "금액_PV"  ,"대수"  ,"주문번호"  
                            , "구분"     , ""  , ""   , ""    , ""                                   
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
       




    }
}
