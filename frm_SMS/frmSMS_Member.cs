using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Net.Mail;
using System.Data.OleDb;
using System.Net;

namespace MLM_Program
{
    public partial class frmSMS_Member : clsForm_Extends
    {
        private int Load_TF = 0;
   
        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_2 = new cls_Grid_Base();
        private const string base_db_name = "tbl_Memberinfo";
        private int Data_Set_Form_TF;
        //private string SMS_SID = "830X4";   //회사 테스트 아이디임
        private string SMS_SID = "L5EY5";   //에이필드 아이디임
        

        public delegate void SendNumberDele(string Send_Number, string Send_Name);
        public event SendNumberDele Send_Mem_Number;

        private string fileName = "";
        private string MMS_fileName = "";


        
        private Series series_Item = new Series();

        public frmSMS_Member()
        {
            InitializeComponent();
        }           


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;
           

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset(1);

            dGridView_Base_Header_2_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_2.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);


            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            mtxtMakDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMakDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtRegDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtRegDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtEduDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtEduDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            //mtxtTel1.Mask = cls_app_static_var.Tel_Number_Fromat;

            mtxtTel1.BackColor = cls_app_static_var.txt_Enable_Color;
            txtFile.BackColor = cls_app_static_var.txt_Enable_Color;
            mtxtTel1.Text = "1600-6205";  
            

            //Reset_Chart_Total();

            mtxtMbid.Focus();
           
        }

        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (cls_User.uSearch_MemberNumber != "")
            {
                Data_Set_Form_TF = 1;
                mtxtMbid.Text = cls_User.uSearch_MemberNumber;
               // mtxtSMbid.Text = cls_User.uSearch_MemberNumber;
                cls_User.uSearch_MemberNumber = "";

                EventArgs ee1 = null; Base_Button_Click(butt_Select, ee1);  //butt_Search
                //EventArgs ee1 = null; Select_Button_Click(butt_Select, ee1);

                //Set_Form_Date(mtxtMbid.Text);
                Data_Set_Form_TF = 0;
            }
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
            cfm.button_flat_change(butt_Send);

            cfm.button_flat_change(butt_S_check);
            cfm.button_flat_change(butt_S_Not_check);
            cfm.button_flat_change(butt_Move_1);

            cfm.button_flat_change(butt_S_check_2);
            cfm.button_flat_change(butt_S_Not_check_2);
            cfm.button_flat_change(butt_Move_2);
            cfm.button_flat_change(butt_Search);
            cfm.button_flat_change(butt_Search2);  
            

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
                            //cfm.form_Group_Panel_Enable_True(this);
                        }
                    }
                }// end if

            }

            ////그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
            //if (sender is DataGridView)
            //{
            //    if (e.KeyValue == 13)
            //    {
            //        EventArgs ee =null;
            //        dGridView_Base_DoubleClick(sender, ee);
            //        e.Handled = true;
            //    } // end if
            //}

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



            if (mtxtRegDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtRegDate1.Text, mtxtRegDate1, "Date") == false)
                {
                    mtxtMakDate1.Focus();
                    return false;
                }
            }

            if (mtxtRegDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtRegDate2.Text, mtxtRegDate2, "Date") == false)
                {
                    mtxtMakDate2.Focus();
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


            if (mtxtEduDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtEduDate1.Text, mtxtEduDate1, "Date") == false)
                {
                    mtxtMakDate1.Focus();
                    return false;
                }
            }

            if (mtxtEduDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtEduDate2.Text, mtxtEduDate2, "Date") == false)
                {
                    mtxtMakDate2.Focus();
                    return false;
                }

            }


                        
           

            return true;
        }


        private void Make_Base_Query(ref string Tsql)
        {

            //string[] g_HeaderText = {"회원_번호"  , "성명"   , "주민번호"  , "현직급"   , "위치"        
            //                    , "센타명"   , "가입일"    , "집전화"   , "핸드폰"    , "교육일"
            //                    , "후원인"   , "후원인명"  , "추천인"   , "추천인명"   ,"우편_번호"
            //                    , "주소"     , "은행명"    , "계좌번호" , "예금주"     , "구분"
            //                    , "활동_여부", "중지_여부"  , "탈퇴일"  , "라인중지일"  ,"기록자"
            //                    , "기록일"
            //                        };

            cls_form_Meth cm = new cls_form_Meth();
            //cm._chang_base_caption_search(m_text);

            Tsql = "Select  ";
            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) ";
            //else
            //    Tsql = Tsql + " tbl_Memberinfo.mbid2 ";


            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2)  ";
            else if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                Tsql = Tsql + " tbl_Memberinfo.mbid2  ";
            else if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                Tsql = Tsql + " tbl_Memberinfo.mbid  ";


            Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";            

            Tsql = Tsql + ",  tbl_Memberinfo.Cpno  ";

            Tsql = Tsql + " , ISNULL(C1.Grade_Name,'') ";
            Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";

            Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";
            Tsql = Tsql + " , LEFT(tbl_Memberinfo.RegTime,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.RegTime,4),2) + '-' + RIGHT(tbl_Memberinfo.RegTime,2)   ";



            Tsql = Tsql + " , tbl_Memberinfo.hometel ";

            Tsql = Tsql + " , Case When Replace(tbl_Memberinfo.hptel,'-','') <> '' Then  tbl_Memberinfo.hptel ELSE tbl_Memberinfo.hometel End  ";
            
            Tsql = Tsql + " , Case When tbl_Memberinfo.Ed_Date <> '' Then  LEFT(tbl_Memberinfo.Ed_Date,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.Ed_Date,4),2) + '-' + RIGHT(tbl_Memberinfo.Ed_Date,2) ELSE '' End Ed_Date_2 ";

            

            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + " ,tbl_Memberinfo.Saveid + '-' + Convert(Varchar,tbl_Memberinfo.Saveid2) ";
            //else
            //    Tsql = Tsql + " ,tbl_Memberinfo.Saveid2 ";

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                Tsql = Tsql + ", tbl_Memberinfo.Saveid + '-' + Convert(Varchar,tbl_Memberinfo.Saveid2)  ";
            else if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                Tsql = Tsql + ", tbl_Memberinfo.Saveid2  ";
            else if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                Tsql = Tsql + ", tbl_Memberinfo.Saveid  ";


            Tsql = Tsql + " , Isnull(Sav.M_Name,'') ";

            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + " ,tbl_Memberinfo.Nominid + '-' + Convert(Varchar,tbl_Memberinfo.Nominid2) ";
            //else
            //    Tsql = Tsql + " ,tbl_Memberinfo.Nominid2 ";

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                Tsql = Tsql + ", tbl_Memberinfo.Nominid + '-' + Convert(Varchar,tbl_Memberinfo.Nominid2)  ";
            else if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                Tsql = Tsql + ", tbl_Memberinfo.Nominid2  ";
            else if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                Tsql = Tsql + ", tbl_Memberinfo.Nominid  ";


            Tsql = Tsql + " , Isnull(Nom.M_Name,'') ";
            Tsql = Tsql + " , Case When tbl_Memberinfo.Addcode1  <> '' Then  LEFT(tbl_Memberinfo.Addcode1,3) +'-' + RIGHT(tbl_Memberinfo.Addcode1,3) ELSE '' End ";

            Tsql = Tsql + " , tbl_Memberinfo.address1     ";
            Tsql = Tsql + " , tbl_Memberinfo.address2    ";
            Tsql = Tsql + " , tbl_Bank.BankName ";
            Tsql = Tsql + " , tbl_Memberinfo.bankaccnt ";            
            Tsql = Tsql + " , tbl_Memberinfo.bankowner ";
            Tsql = Tsql + " , Case  When tbl_Memberinfo.Sell_Mem_TF = 0 then '" + cm._chang_base_caption_search("판매원") + "' ELSE  '" + cm._chang_base_caption_search("소비자") + "' End AS Sell_MEM_TF2";


            //Tsql = Tsql + " , Case When tbl_Memberinfo.LeaveDate = '' Then '" + cm._chang_base_caption_search("활동") + "' ELSE '" + cm._chang_base_caption_search("탈퇴") + "' End AS LeaveCheck_2 ";
            Tsql = Tsql + " , Case  ";
            Tsql = Tsql + "  When tbl_Memberinfo.LeaveCheck = 1 Then '" + cm._chang_base_caption_search("활동") + "'";
            Tsql = Tsql + "  When tbl_Memberinfo.LeaveCheck = 0 Then '" + cm._chang_base_caption_search("탈퇴") + "'";
            Tsql = Tsql + "  When tbl_Memberinfo.LeaveCheck = -100 Then '" + cm._chang_base_caption_search("휴면") + "'";
            Tsql = Tsql + "  End AS LeaveCheck_2 ";

            Tsql = Tsql + " , Case When tbl_Memberinfo.LineUserDate = '' Then '" + cm._chang_base_caption_search("사용") + "' ELSE '" + cm._chang_base_caption_search("중지") + "' End ";
            Tsql = Tsql + " , Case When tbl_Memberinfo.LeaveDate <> '' Then  LEFT(tbl_Memberinfo.LeaveDate,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.LeaveDate,4),2) + '-' + RIGHT(tbl_Memberinfo.LeaveDate,2) ELSE '' End ";
            Tsql = Tsql + " , Case When tbl_Memberinfo.LineUserDate <> '' Then  LEFT(tbl_Memberinfo.LineUserDate,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.LineUserDate,4),2) + '-' + RIGHT(tbl_Memberinfo.LineUserDate,2) ELSE '' End ";
            Tsql = Tsql + " , tbl_Memberinfo.recordid ";

            Tsql = Tsql + " , tbl_Memberinfo.recordtime ";

            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Sav (nolock) ON tbl_Memberinfo.Saveid = Sav.Mbid And tbl_Memberinfo.Saveid2 = Sav.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Nom (nolock) ON tbl_Memberinfo.Nominid = Nom.Mbid And tbl_Memberinfo.Nominid2 = Nom.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode  And tbl_Memberinfo.Na_code = tbl_Business.Na_code";
            //Tsql = Tsql + " Left Join tbl_Bank On tbl_Memberinfo.bankcode=tbl_Bank.ncode  And tbl_Memberinfo.Na_code = tbl_Bank.Na_code ";
            Tsql = Tsql + " Left Join tbl_Bank On tbl_Memberinfo.bankcode=tbl_Bank.ncode ";
            cls_NationService.SQL_BankNationCode(ref Tsql);
            Tsql = Tsql + " Left Join tbl_Class C1 On tbl_Memberinfo.CurGrade=C1.Grade_Cnt ";            
        }



        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = " Where tbl_Memberinfo.M_Name <> ''  ";
            //// strSql = strSql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";

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
                    strSql = strSql + " And tbl_Memberinfo.Mbid = '" + Mbid + "'";
                    strSql = strSql + " And tbl_Memberinfo.Mbid2 = " + Mbid2;
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
                        strSql = strSql + " And tbl_Memberinfo.Mbid >='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_Memberinfo.Mbid2 >= " + Mbid2;
                }

                if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And tbl_Memberinfo.Mbid <='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_Memberinfo.Mbid2 <= " + Mbid2;
                }
            }


            //회원명으로 검색
            if (txtName.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.M_Name Like '%" + txtName.Text.Trim() + "%'";

            //가입일자로 검색 -1

            if ((mtxtRegDate1.Text.Replace("-", "").Trim() != "") && (mtxtRegDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_Memberinfo.RegTime = '" + mtxtRegDate1.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtRegDate1.Text.Replace("-", "").Trim() != "") && (mtxtRegDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_Memberinfo.RegTime >= '" + mtxtRegDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_Memberinfo.RegTime <= '" + mtxtRegDate2.Text.Replace("-", "").Trim() + "'";
            }


            //기록일자로 검색 -1
            if ((mtxtMakDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And Replace(Left( tbl_Memberinfo.recordtime ,10),'-','') = '" + mtxtMakDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtMakDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And Replace(Left( tbl_Memberinfo.recordtime ,10),'-','') >= '" + mtxtMakDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And Replace(Left( tbl_Memberinfo.recordtime ,10),'-','') <= '" + mtxtMakDate2.Text.Replace("-", "").Trim() + "'";
            }


            //교육일자로 검색 -1
            if ((mtxtEduDate1.Text.Replace("-", "").Trim() != "") && (mtxtEduDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_Memberinfo.Ed_Date = '" + mtxtEduDate1.Text.Replace("-", "").Trim() + "'";

            //교육일자로 검색 -2
            if ((mtxtEduDate1.Text.Replace("-", "").Trim() != "") && (mtxtEduDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_Memberinfo.Ed_Date  >= '" + mtxtEduDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_Memberinfo.Ed_Date  <= '" + mtxtEduDate2.Text.Replace("-", "").Trim() + "'";
            }

            //센타코드로으로 검색
            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";

            if (txtR_Id_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.recordid = '" + txtR_Id_Code.Text.Trim() + "'";

            if (txtBank_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BankCode = '" + txtBank_Code.Text.Trim() + "'";

            if (opt_Leave_2.Checked== true)
                strSql = strSql + " And tbl_Memberinfo.LeaveDate = ''";

            if (opt_Leave_3.Checked == true)
                strSql = strSql + " And tbl_Memberinfo.LeaveDate <> ''";

            if (opt_Line_2.Checked == true)
                strSql = strSql + " And tbl_Memberinfo.LineUserDate = ''";

            if (opt_Line_3.Checked == true)
                strSql = strSql + " And tbl_Memberinfo.LineUserDate <> ''";



            if (opt_Login_2.Checked == true)
                strSql = strSql + " And tbl_Memberinfo.Log_Admin = 'Y'";

            if (opt_Login_3.Checked == true)
                strSql = strSql + " And tbl_Memberinfo.Log_Admin = 'N'";



            if (opt_sell_2.Checked == true)
                strSql = strSql + " And tbl_Memberinfo.Sell_Mem_TF = 0 ";

            if (opt_sell_3.Checked == true)
                strSql = strSql + " And tbl_Memberinfo.Sell_Mem_TF = 1 ";


            if (rab_G.Checked == true) //지사장만 불러온다.
                strSql = strSql + " And tbl_Memberinfo.Mbid IN (Select Mbid From tbl_Business (nolock) Where Mbid <> '') ";

            if (rab_R.Checked == true) //로드샵만 불러온다.
                strSql = strSql + " And tbl_Memberinfo.Mbid IN (Select Mbid From tbl_Business_Road (nolock) Where Mbid <> '') ";

            if (rab_G_R.Checked == true) //로드샵만 불러온다.
            {
                strSql = strSql + " And ( tbl_Memberinfo.Mbid IN (Select Mbid From tbl_Business (nolock) Where Mbid <> '') ";
                strSql = strSql + " OR  tbl_Memberinfo.Mbid IN (Select Mbid From tbl_Business_Road (nolock) Where Mbid <> '') )";
            }
            

            if (opt_Ed_2.Checked == true)
                strSql = strSql + " And tbl_Memberinfo.Ed_Date <> ''";

            if (opt_Ed_3.Checked == true)
                strSql = strSql + " And tbl_Memberinfo.Ed_Date = ''";

            if (txtCpno.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.Cpno   = '" + encrypter.Encrypt(txtCpno.Text.Trim() )+ "'";


            if (txtPNumber.Text.Trim() != "")
            {
                //tbl_Memberinfo.hometel ) 
                //strSql = strSql + " And (charindex ( Replace(tbl_Memberinfo.hometel  ,'-',''),'" + encrypter.Encrypt(txtPNumber.Text.Replace(" ", "").ToString()) + "') >0 ";
                //strSql = strSql + " OR  charindex ( Replace(tbl_Memberinfo.hptel  ,'-',''),'" + encrypter.Encrypt(txtPNumber.Text.Replace(" ", "").ToString()) + "') >0 ) ";
                strSql = strSql + " And ( tbl_Memberinfo.hometel  = '" + encrypter.Encrypt(txtPNumber.Text.Replace(" ", "") .ToString()) + "' ";
                strSql = strSql + " OR   tbl_Memberinfo.hptel   = '" + encrypter.Encrypt(txtPNumber.Text.Replace(" ", "").ToString()) + "' ) ";
            }

            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

            Tsql = Tsql + strSql ;
            Tsql = Tsql + " Order by tbl_Memberinfo.Mbid, tbl_Memberinfo.Mbid2 ASC ";
        }




        private void Base_Grid_Set()
        {   
            string Tsql = "";            
            Make_Base_Query(ref Tsql);

            Make_Base_Query_(ref Tsql);

            cls_form_Meth cm = new cls_form_Meth();
            //cm._chang_base_caption_search(m_text);

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();                                  
            
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name , this.Text ) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            double SellCnt_1 = 0; double SellCnt_2 = 0;
            double MemCnt_1 = 0; double MemCnt_2 = 0;
            double EdCnt_1 = 0; double EdCnt_2 = 0; 
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            Dictionary<string, double> Center_MemCnt = new Dictionary<string, double>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                //string Per_t = cm._chang_base_caption_search("판매원");
                //if (Per_t == ds.Tables[base_db_name].Rows[fi_cnt]["Sell_MEM_TF2"].ToString())
                //    SellCnt_1++;
                //else                
                //    SellCnt_2++;

                //Per_t = cm._chang_base_caption_search("활동");
                //if (Per_t == ds.Tables[base_db_name].Rows[fi_cnt]["LeaveCheck_2"].ToString())
                //    MemCnt_1++;
                //else
                //    MemCnt_2++;
                                
                //if ( ds.Tables[base_db_name].Rows[fi_cnt]["Ed_Date_2"].ToString() != "")
                //    EdCnt_1++;
                //else
                //    EdCnt_2++;

                //Per_t = ds.Tables[base_db_name].Rows[fi_cnt]["B_Name"].ToString();

                //if (Per_t != "")
                //{
                //    if (Center_MemCnt.ContainsKey(Per_t) == true)
                //        Center_MemCnt[Per_t] ++;
                //    else
                //        Center_MemCnt[Per_t] = 1;
                //}
                
            }
            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
            
            //차트 관련해서 뿌려주는곳 아래쪽
            //Reset_Chart_Total(SellCnt_1, SellCnt_2);
            //Reset_Chart_Total(MemCnt_1, MemCnt_2, 1);
            //Reset_Chart_Total(EdCnt_1, EdCnt_2, "1");
            //foreach (string tkey in Center_MemCnt.Keys )
            //{
            //    Push_data(series_Item, tkey, Center_MemCnt[tkey]);
            //}
        }



        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 26;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"선택","회원_번호"  , "성명"   , "_주민번호"    , "_위치"        
                                , "_센타명"   , "_가입일"    , "_집전화"   , "핸드폰"    , "_교육일"
                                , "_후원인"   , "_후원인명"  , "_추천인"   , "_추천인명"   ,"_우편_번호"
                                , "_주소"     , "_은행명"    , "_계좌번호" , "_예금주"     , "_구분"
                                , "_활동_여부", "_중지_여부"  , "_탈퇴일"  , "_라인중지일"  ,"_기록자"
                                , "_기록일"
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 70, 90, 130, 0, 0  
                             ,0, 0, 0, 120, 0  
                             ,0 , 0, 0 , 0 , 0
                             ,0 , 0, 0 , 0 , 0
                             ,0 , 0 , 0 , 0 , 0
                             ,0
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                     
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
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //10

                               ,DataGridViewContentAlignment.MiddleCenter   
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //15   
                          
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //20

                               ,DataGridViewContentAlignment.MiddleCenter   
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //25   

                               ,DataGridViewContentAlignment.MiddleCenter  
                              };
            cgb.grid_col_alignment = g_Alignment;
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = {    ""
                                ,ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][2].ToString (),"Cpno")                                
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][7].ToString ())
                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][8].ToString ())
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][10]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][11]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][12]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][13]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][14]

                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][15].ToString ()) + " " + encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][16].ToString ())
                                ,ds.Tables[base_db_name].Rows[fi_cnt][17]
                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][18].ToString ())
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






        private void dGridView_Base_Header_2_Reset()
        {
            cgb_2.grid_col_Count = 5;
            cgb_2.basegrid = dGridView_Base_2;
            cgb_2.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_2.grid_Frozen_End_Count = 2;
            cgb_2.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"선택","회원_번호"  , "성명"   , "핸드폰"    , "결과"        
                                
                                    };
            cgb_2.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 70, 90, 130, 140, 100                              
                            };
            cgb_2.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                                                        
                                   };
            cgb_2.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft  //5                             
                              };
            cgb_2.grid_col_alignment = g_Alignment;
        }


   





        private void MtxtData_Temp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    string Sn = mtb.Text.Replace("-", "").Replace("_", "").Trim();
                    if (mtb.Name == "mtxtBiz1")
                    {
                        if (Sn_Number_(Sn, mtb, "biz") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel1")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel2")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtZip1")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    string R4_name = mtb.Name.Substring(mtb.Name.Length - 4, 4);
                    if (R4_name == "Date" || R4_name == "ate3" || R4_name == "ate1" || R4_name == "ate2" || R4_name == "ate4")
                    {
                        if (Sn_Number_(Sn, mtb, "Date") == true)
                            SendKeys.Send("{TAB}");
                    }

                   

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
                    string[] date_a = mtb.Text.Split('-');

                    if (date_a.Length >= 3 && date_a[0].Trim() != "" && date_a[1].Trim() != "" && date_a[2].Trim() != "")
                    {
                        string Date_YYYY = "0000" + int.Parse(date_a[0]).ToString();

                        date_a[0] = Date_YYYY.Substring(Date_YYYY.Length - 4, 4);

                        if (int.Parse(date_a[1]) < 10)
                            date_a[1] = "0" + int.Parse(date_a[1]).ToString();

                        if (int.Parse(date_a[2]) < 10)
                            date_a[2] = "0" + int.Parse(date_a[2]).ToString();

                        mtb.Text = date_a[0] + '-' + date_a[1] + '-' + date_a[2];

                        cls_Check_Input_Error c_er = new cls_Check_Input_Error();
                        if (mtb.Text.Replace("-", "").Trim() != "")
                        {
                            int Ret = 0;
                            Ret = c_er.Input_Date_Err_Check(mtb);

                            if (Ret == -1)
                            {
                                mtb.Focus(); return false;
                            }
                        }

                    }
                    else
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

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "-"))
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, "1") == false)
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
                Data_Set_Form_TF = 0 ;
            }

            if (tb.Name == "txtBank")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtBank_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtR_Id")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtR_Id_Code.Text = "";
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
                Db_Grid_Popup(tb, txtBank_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtBank_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtBank_Code);

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
                cgb_Pop.Next_Focus_Control = txtR_Id;

            if (tb.Name == "txtCenter2")
                cgb_Pop.Next_Focus_Control = txtR_Id;

            if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtR_Id")
                cgb_Pop.Next_Focus_Control = txtBank;
            

            cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
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
                    Tsql = Tsql + " And  ShowMemberCenter = 'Y' ";
                    if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
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
                    if (cls_User.gid_CountryCode != "") Tsql = Tsql + " Where  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);
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
                Tsql = Tsql + " Where (Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%')";
                if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
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
                Reset_Chart_Total(); 

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                dGridView_Base_Header_2_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_2.d_Grid_view_Header_Reset(1);
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtMbid);

                opt_Ed_1.Checked = true; opt_Line_1.Checked = true; opt_Leave_1.Checked = true; opt_sell_1.Checked = true;
                rab_T.Checked = true;
                combo_Sheet.Items.Clear();

                mtxtTel1.Text = "1600-6205";  
                //radioB_S.Checked = true; radioB_R.Checked = true;                 radioB_E.Checked = true;
                tab_Chart.SelectedIndex = 0; 
            }
            else if (bt.Name == "butt_Select")
            {
                

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                ////dGridView_Base_Header_2_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                ////cgb_2.d_Grid_view_Header_Reset();

                //Reset_Chart_Total(); 
                tab_Chart.SelectedIndex = 0; 
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                if (Check_TextBox_Error() == false) return;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                chart_Center.Series.Clear();
                //Save_Nom_Line_Chart();                
                
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
            Excel_Export_File_Name = this.Text; // "Member_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }
        private DataGridView e_f_Send_Export_Excel_Info_2(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Member_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base_2;
        }
        private DataGridView e_f_Send_Export_Excel_Info_3(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Member_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base_3;
        }

        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string Send_Nubmer = ""; string Send_Name = "";
                Send_Nubmer = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                Send_Name = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
                Send_Mem_Number(Send_Nubmer, Send_Name);   //부모한테 이벤트 발생 신호한다.
            }            
        }


        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
           // SendKeys.Send("{TAB}");
        }





        private void radioB_S_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtRegDate1, mtxtRegDate2, (RadioButton)sender);
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


        private void radioB_E_Base_Click(object sender, EventArgs e)
        {
            Data_Set_Form_TF = 1;
            //RadioButton _Rb = (RadioButton)sender;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtEduDate1, mtxtEduDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }



        private void Reset_Chart_Total()
        {
            //chart_Mem.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();
            
            double[] yValues = { 0, 0 };
            string[] xValues = { cm._chang_base_caption_search("판매원"), cm._chang_base_caption_search("소비자") };
            chart_Mem.Series["Series1"].Points.DataBindXY(xValues, yValues);
            
            chart_Mem.Series["Series1"].ChartType = SeriesChartType.Pie;
            //chart_Mem.Series["Series1"]["PieLabelStyle"] = "Disabled";

            chart_Mem.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;


            chart_Mem.Legends[0].Enabled = true;


            //chart_Leave.Series.Clear();
            double[] yValues_2 = { 0, 0 };
            string[] xValues_2 = { cm._chang_base_caption_search("활동"), cm._chang_base_caption_search("탈퇴") };
            chart_Leave.Series["Series1"].Points.DataBindXY( xValues_2, yValues_2);
            
            chart_Leave.Series["Series1"].ChartType = SeriesChartType.Pie;
            //chart_Leave.Series["Series1"]["PieLabelStyle"] = "Disabled";

            chart_Leave.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;            
            chart_Leave.Legends[0].Enabled = true;


            double[] yValues_3 = { 0, 0 };
            string[] xValues_3 = { cm._chang_base_caption_search("교육이수자"), cm._chang_base_caption_search("비이수자") };
            chart_edu.Series["Series1"].Points.DataBindXY(xValues_3, yValues_3);
            
            chart_edu.Series["Series1"].ChartType = SeriesChartType.Pie;
            //chart_edu.Series["Series1"]["PieLabelStyle"] = "Disabled";
            chart_edu.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_edu.Legends[0].Enabled = true;

            chart_Center.Series.Clear();
            series_Item.Points.Clear();
        }


        private void Reset_Chart_Total(double SellCnt_1, double SellCnt_2)
        {
            //chart_Mem.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();
            Series series_Save = new Series();                     
         
            chart_Mem.Series.Clear();
            chart_Mem.Series.Add(series_Save);

            DataPoint dp = new DataPoint();
            series_Save.ChartType = SeriesChartType.Pie;
            dp.SetValueXY(cm._chang_base_caption_search("판매원"), SellCnt_1);
            dp.Label = SellCnt_1.ToString() ;            
            dp.LabelForeColor = Color.Black;
            dp.LegendText = cm._chang_base_caption_search("판매원");
            series_Save.Points.Add(dp);

            DataPoint dp2 = new DataPoint();

            dp2.SetValueXY(cm._chang_base_caption_search("소비자"), SellCnt_2);
            dp2.Label = SellCnt_2.ToString() ;            
            dp2.LabelForeColor = Color.Black;
            dp2.LegendText = cm._chang_base_caption_search("소비자");
            series_Save.Points.Add(dp2);           
            chart_Mem.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;            
            chart_Mem.Legends[0].Enabled = true;
        }

        private void Reset_Chart_Total( double MemCnt_1, double MemCnt_2,int t_f)
        {

            cls_form_Meth cm = new cls_form_Meth();
            Series series_Save = new Series();

            chart_Leave.Series.Clear();
            chart_Leave.Series.Add(series_Save);

            DataPoint dp = new DataPoint();
            series_Save.ChartType = SeriesChartType.Pie;
            dp.SetValueXY(cm._chang_base_caption_search("활동"), MemCnt_1);
            dp.Label = MemCnt_1.ToString();            
            dp.LabelForeColor = Color.Black;
            dp.LegendText = cm._chang_base_caption_search("활동");
            series_Save.Points.Add(dp);

            DataPoint dp2 = new DataPoint();

            dp2.SetValueXY(cm._chang_base_caption_search("탈퇴"), MemCnt_2);
            dp2.Label = MemCnt_2.ToString();            
            dp2.LabelForeColor = Color.Black;
            dp2.LegendText = cm._chang_base_caption_search("탈퇴");
            series_Save.Points.Add(dp2);
            chart_Leave.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_Leave.Legends[0].Enabled = true;


         
            chart_Leave.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_Leave.Legends[0].Enabled = true;
        }

        private void Reset_Chart_Total(double EduCnt_1, double EduCnt_2, string  t_f)
        {

            cls_form_Meth cm = new cls_form_Meth();
            Series series_Save = new Series();

            chart_edu.Series.Clear();
            chart_edu.Series.Add(series_Save);

            DataPoint dp = new DataPoint();
            series_Save.ChartType = SeriesChartType.Pie;
            dp.SetValueXY(cm._chang_base_caption_search("교육이수자"), EduCnt_1);
            dp.Label = EduCnt_1.ToString();            
            dp.LabelForeColor = Color.Black;
            dp.LegendText = cm._chang_base_caption_search("교육이수자");
            series_Save.Points.Add(dp);

            DataPoint dp2 = new DataPoint();

            dp2.SetValueXY(cm._chang_base_caption_search("비이수자"), EduCnt_2);
            dp2.Label = EduCnt_2.ToString();            
            dp2.LabelForeColor = Color.Black;
            dp2.LegendText = cm._chang_base_caption_search("비이수자");
            series_Save.Points.Add(dp2);

            chart_edu.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_edu.Legends[0].Enabled = true;
        }








        private void Push_data(Series series, string p, double p_3)
        {


            if (p != "")
            {
                DataPoint dp = new DataPoint();

                if (p.Replace(" ", "").Length >= 4)
                    dp.SetValueXY(p.Replace(" ", "").Substring(0, 4), p_3);
                else
                    dp.SetValueXY(p, p_3);

                dp.Font = new System.Drawing.Font("맑은고딕", 9);
                dp.Label = string.Format(cls_app_static_var.str_Currency_Type, p_3); // p_3.ToString();                  
                series.Points.Add(dp);
            }

        }

   

        //Push_data(series_Item, nodeKey.ToString() + "Line", Save_Cnt[nodeKey]);
        private void Save_Nom_Line_Chart()
        {
            cls_form_Meth cm = new cls_form_Meth();

            chart_Center.Series.Clear();
            series_Item.Points.Clear();
            


            series_Item["DrawingStyle"] = "Emboss";
            series_Item["PointWidth"] = "0.5";
            series_Item.Name = cm._chang_base_caption_search("인원수");
            
            
            //series_Item.ChartArea = "ChartArea1";
            series_Item.ChartType = SeriesChartType.Column  ;            
            // series_Item.Legend = "Legend1";            


            chart_Center.Series.Add(series_Item);
            //chart_Center.Series.Add(series_PV);
            chart_Center.ChartAreas[0].AxisX.Interval = 1;
            chart_Center.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_Center.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 8;
            chart_Center.ChartAreas[0].AxisY.Interval = 500;

            chart_Center.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //chart_Center.ChartAreas["ChartArea1"].BackColor = Color.White;
            chart_Center.Legends[0].Enabled = true;

        }




        private int but_Exp_Base_Left = 0;
        private int Parent_but_Exp_Base_Width = 0;

        private void but_Exp_Click(object sender, EventArgs e)
        {
            if (but_Exp.Text == "<<")
            {
                Parent_but_Exp_Base_Width = but_Exp.Parent.Width;
                but_Exp_Base_Left = but_Exp.Left  ;

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

        private void dGridView_Base_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

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
                dGridView_Base.Visible = false;
                dGridView_Base.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
                {
                    dGridView_Base.Rows[i].Cells[0].Value = "";
                }
                dGridView_Base.Visible = true;
            }

            else if (bt.Name == "butt_S_check_2")
            {
                dGridView_Base_2.Visible = false;
                dGridView_Base_2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                for (int i = 0; i <= dGridView_Base_2.Rows.Count - 1; i++)
                {
                    dGridView_Base_2.Rows[i].Cells[0].Value = "V";
                }
                dGridView_Base_2.Visible = true;
            }


            else if (bt.Name == "butt_S_Not_check_2")
            {
                dGridView_Base_2.Visible = false;
                dGridView_Base_2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                for (int i = 0; i <= dGridView_Base_2.Rows.Count - 1; i++)
                {
                    dGridView_Base_2.Rows[i].Cells[0].Value = "";
                }
                dGridView_Base_2.Visible = true;
            }

            else if (bt.Name == "butt_Move_1")
            {
                ////dGridView_Base_Header_2_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                ////cgb_2.d_Grid_view_Header_Reset(1);
                if (tabSearchMem.SelectedIndex == 0)
                    Select_Send_Sms_Gr();
                else
                    Select_Send_Sms_Gr1();
            }

            else if (bt.Name == "butt_Move_2")
            {
                Select_Send_Sms_Gr(1);
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;

        }

        private void Select_Send_Sms_Gr1()
        {
            char [] split = new char[] {'\r', '\n' };
            string[] Texts = rtxtHPTels.Text.Split( split, StringSplitOptions.None);

            foreach (string hptel in Texts)
            {

                if (cls_User.gid_CountryCode == "KR" && hptel != "" && (hptel.Substring(0, 3) == "010" || hptel.Substring(0, 3) == "011" || hptel.Substring(0, 3) == "016" || hptel.Substring(0, 3) == "017" || hptel.Substring(0, 3) == "018" || hptel.Substring(0, 3) == "019"))
                {
                    object[] row0 = { "", "", "", hptel, "" };
                    cgb_2.basegrid.Rows.Add(row0);
                }

                if (cls_User.gid_CountryCode != "KR" && hptel != "")
                {
                    object[] row0 = { "", "", "", hptel, "" };
                    cgb_2.basegrid.Rows.Add(row0);
                }
            }

            rtxtHPTels.Text = "";
        }

        private void Select_Send_Sms_Gr()
        {
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
                dGridView_Base.Focus(); return ;
            }

            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            string Mbid = "", M_Name = "", hptel = "";

            for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
            {
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    Mbid = dGridView_Base.Rows[i].Cells[1].Value.ToString();
                    M_Name = dGridView_Base.Rows[i].Cells[2].Value.ToString();
                    hptel = dGridView_Base.Rows[i].Cells[8].Value.ToString();

                    if (hptel != "" && (hptel.Substring(0, 3) == "010" || hptel.Substring(0, 3) == "011" || hptel.Substring(0, 3) == "016" || hptel.Substring(0, 3) == "017" || hptel.Substring(0, 3) == "018" || hptel.Substring(0, 3) == "019"))
                    {
                        int Cnt = 0, SW = 0;
                        while (Cnt <= dGridView_Base_2.Rows.Count - 1 && SW == 0)
                        {
                            if (dGridView_Base_2.Rows[Cnt].Cells[1].Value.ToString() == Mbid)
                            {
                                dGridView_Base.Rows[i].Visible = false;
                                SW = 1;
                                if (dGridView_Base_2.Rows[Cnt].Visible == false)
                                {
                                    dGridView_Base_2.Rows[Cnt].Cells[0].Value = "";
                                    dGridView_Base_2.Rows[Cnt].Visible = true;
                                }
                            }
                            Cnt++;
                        }
                        if (SW == 0)
                        {
                            object[] row0 = { "", Mbid, M_Name, hptel, "" };
                            cgb_2.basegrid.Rows.Add(row0);
                            dGridView_Base.Rows[i].Visible = false;
                        }
                    }
                }
            }            
        }



        private void Select_Send_Sms_Gr(int TCnt)
        {
            int chk_cnt = 0;

            for (int i = 0; i <= dGridView_Base_2.Rows.Count - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base_2.Rows[i].Cells[0].Value.ToString() == "V")
                    chk_cnt++;
            }

            if (chk_cnt == 0) //저장할 내역이 없을을 알린다.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridView_Base.Focus(); return;
            }

            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            string Mbid = "";

            for (int i = dGridView_Base_2.Rows.Count - 1; i >=0; i--)
            {
                if (dGridView_Base_2.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    Mbid = dGridView_Base_2.Rows[i].Cells[1].Value.ToString();
                    
                    dGridView_Base_2.Rows[i].Visible = false;
                    dGridView_Base_2.Rows[i].Cells[0].Value = "";
 
                    int Cnt = 0, SW = 0;
                    while (Cnt <= dGridView_Base.Rows.Count - 1 && SW == 0)
                    {
                        if (dGridView_Base.Rows[Cnt].Cells[1].Value.ToString() == Mbid)
                        {
                            SW = 1;
                            dGridView_Base.Rows[Cnt].Cells[0].Value = "";
                            dGridView_Base.Rows[Cnt].Visible = true;
                        }
                        Cnt++;
                    }                    
                }
            }
        }

        private void dGridView_Base_2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

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

        private void txtSMS_TextChanged(object sender, EventArgs e)
        {
            //int smslength = 0 ;
            //if (txtSMS.Text.Trim() != "")
            //{
            //    smslength = Encoding.Default.GetBytes(txtSMS.Text).Length; 
            //}

            //lbl_Length.Text = smslength.ToString();
        }

        private void chk_Mbid_MouseClick(object sender, MouseEventArgs e)
        {
            if (chk_Mbid.Checked == true)
            {
                int CurC = txtSMS.SelectionStart;

                txtSMS.Text = txtSMS.Text.Substring(0, CurC) + "&" + txtSMS.Text.Substring(CurC, txtSMS.Text.Length - CurC);
            }
            else
            {
                txtSMS.Text = txtSMS.Text.Replace("&", "");
            }
            txtSMS.Focus ();
            txtSMS.SelectionStart = txtSMS.Text.Length;

        }

        private void chk_Name_MouseClick(object sender, MouseEventArgs e)
        {
            if (chk_Name.Checked == true)
            {
                int CurC = txtSMS.SelectionStart;

                txtSMS.Text = txtSMS.Text.Substring(0, CurC) + "`" + txtSMS.Text.Substring(CurC, txtSMS.Text.Length - CurC);
            }
            else
            {
                txtSMS.Text = txtSMS.Text.Replace("`", "");
            }
            txtSMS.Focus();
            txtSMS.SelectionStart = txtSMS.Text.Length;
        }



       


        private void butt_Sms_Send(object sender, EventArgs e)
        {
           if ( mtxtTel1.Text.Replace("-", "").Replace("_", "").Trim() == "")
            {
                MessageBox.Show("회신번호를 필히 입력해 주십시요." + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                mtxtTel1.Focus();
                return;
            }


            int chk_cnt = 0;

            for (int i = 0; i <= dGridView_Base_2.Rows.Count - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base_2.Rows[i].Cells[0].Value.ToString() == "V" && dGridView_Base_2.Rows[i].Visible == true)
                    chk_cnt++;
            }

            if (chk_cnt == 0) //저장할 내역이 없을을 알린다.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridView_Base_2.Focus(); return;
            }

            if (txtSMS.Text.Trim().Length == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SMS_Mess_Not") + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                txtSMS.Focus(); return;
            }



            int MSM_Seq = 0;
            MMS_fileName = "";
            if (txtFile.Text != "")
            {
                if (Board_Input(ref MSM_Seq) == false)
                {
                    MessageBox.Show("MMS 관련 이미지 파일 전송에 실패 했습니다." + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    butt_Search2.Focus();
                    return;
                }
                else
                {
                    if (MSM_Seq == 0)
                    {
                        MessageBox.Show("MMS 관련 이미지 파일 전송에 실패 했습니다." + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                        butt_Search2.Focus();
                        return;
                    }
                }
            }
            
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Send_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            int tempOK = 0, tempBad = 0;
            for (int i = 0; i <= dGridView_Base_2.Rows.Count - 1; i++)
            {
                //빈칸으로 들어간 내역을 0으로 바꾼다
                if (dGridView_Base_2.Rows[i].Cells[0].Value.ToString() == "V" && dGridView_Base_2.Rows[i].Visible == true)
                {
                    Sms_Send_Short(i, ref tempOK, ref tempBad);
                                    
                }
            }

            int totalSend = tempOK + tempBad;
            txtResult.Text = txtResult.Text + "총 전송시도 :" + totalSend + "건";
            txtResult.Text = txtResult.Text + Environment.NewLine;
            txtResult.Text = txtResult.Text + "성공 :" + tempOK.ToString () + "건";
            txtResult.Text = txtResult.Text + Environment.NewLine;
            txtResult.Text = txtResult.Text + "실패 :" + tempBad.ToString() + "건";
            txtResult.Text = txtResult.Text + Environment.NewLine;

            string sdate = "";
            DateTime TodayDate = new DateTime();
            TodayDate = DateTime.Now;
            sdate = TodayDate.ToString("yyyy.MM.dd");

            MessageBox.Show(("전송이 완료 됐습니다.") + "\n" + "(전송일자 :" + sdate + ")");
        }


        private void Email_Send()
        {
            try
            {
                MailMessage mail = new MailMessage();
                //mail.To.Add("darkman7@daum.net");
                mail.To.Add("cjdgur7@naver.com");
                mail.From = new MailAddress("cjdgur7@naver.com");

                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                mail.Subject = "Test Mail";
                mail.Body = "This is for testing SMTP mail from GMAIL";
                mail.IsBodyHtml = true;

                SmtpClient SmtpServer = new SmtpClient("smtp.naver.com");
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("cjdgur7@naver.com", "71022396");
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

                //MailMessage mail = new MailMessage();
                //SmtpClient SmtpServer = new SmtpClient("smtp.naver.com");

                //mail.From = new MailAddress("cjdgur7@naver.com", "TEST", System.Text.Encoding.UTF8);
                //mail.To.Add("cjdgur@lycos.co.kr");
                //mail.Subject = "Test Mail";
                //mail.Body = "This is for testing SMTP mail from GMAIL";
                //mail.BodyEncoding = System.Text.Encoding.UTF8;
                //mail.SubjectEncoding = System.Text.Encoding.UTF8;


                //SmtpServer.Port = 465;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("cjdgur7@naver.com", "71022396");
                //SmtpServer.EnableSsl = true ;

                //SmtpServer.Send(mail);

            //    MailMessage msg = new MailMessage("cjdgur@gmail.com", "cjdgur@lycos.co.kr",
            //"Subject : Email Test", "This is a mail test");
                 

            //// SmtpClient 셋업 (Live SMTP 서버, 포트)

            //    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 465);
            //smtp.EnableSsl = true;
                 

            //// Live 또는 Hotmail 계정과 암호 필요
            //smtp.Credentials = new System.Net.NetworkCredential("cjdgur@gmail.com", "kk71022396");

 
            //// 발송
            //smtp.Send(msg);






                //var client = new SmtpClient("smtp.ilsong.com", 587)
                //{
                //    Credentials = new System.Net.NetworkCredential("kys201@ilsong.com", "kys201"),
                //    EnableSsl = false
                //};
                //client.Send("kys201@ilsong.com", "cjdgur@lycos.co.kr", "test", "testbody");
                //SmtpServer.Send(mail);
                //Console.WriteLine("Sent");
                //Console.ReadLine();

                //string senderAddress = "smtp.naver.com";
                //string receiveraddress = "cjdgur@lycos.co.kr";
                //string emailSubject = "Hi..";
                //string emailMessageText = "테스트 입니다.  ";
                //MailMessage mail = new MailMessage();
                //SmtpClient client = new SmtpClient("smtp.naver.com", 587);
                //mail.From = new MailAddress(senderAddress, "Sender", Encoding.UTF8);
                //mail.To.Add(receiveraddress);
                //mail.Subject = emailSubject;
                //mail.Body = emailMessageText;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential("cjdgur7", "71022396");
                //client.EnableSsl = true;
                //client.Send(mail);
                MessageBox.Show("mail Send");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void Sms_Send_Short(int rowCnt, ref int tempOK, ref int tempBad)
        {
            string tempMessage = "";
            string T_M_Name = "", T_Mbid = "";
            string smsDeptID = cls_app_static_var.SMS_smsDeptID; // "promaxco";
            string CallTel = "";

            string strSQL= "" ;
            string Sn = mtxtTel1.Text.Replace("-", "").Replace("_", "").Trim();
            int nRet = 0;


            T_Mbid = dGridView_Base_2.Rows[rowCnt].Cells[1].Value.ToString(); 
            T_M_Name = dGridView_Base_2.Rows[rowCnt].Cells[2].Value.ToString(); 
            CallTel = dGridView_Base_2.Rows[rowCnt].Cells[3].Value.ToString().Replace ("-","");

            
            tempMessage = txtSMS.Text.Trim();
            tempMessage = tempMessage.Replace("&", T_Mbid);
            tempMessage = tempMessage.Replace("`", T_M_Name);
            int smslength = Encoding.Default.GetBytes(tempMessage).Length;


            if (CallTel == "")
            {
                tempBad++; return;
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            tempMessage = tempMessage.Replace("'", "''");

            //if (MMS_fileName != "")
            //{
            //    strSQL = " Exec USP_MMS_APYLD_Mem_Sender_MMS '" + SMS_SID + "' ,";
            //    strSQL = strSQL + "'" + CallTel + "' ,";
            //    strSQL = strSQL + "'" + Sn + "', ";
            //    strSQL = strSQL + "'" + tempMessage + "',";
            //    strSQL = strSQL + "'" + T_Mbid + "', ";
            //    strSQL = strSQL + "'" + T_M_Name + "', 'M' ,'" + cls_User.gid + "',0 , '" + MMS_fileName + "' , 1";
            //}
            //else
            //{

            //    strSQL = " Exec USP_MMS_APYLD_Mem_Sender '" + SMS_SID + "' ,";
            //    strSQL = strSQL + "'" + CallTel + "' ,";
            //    strSQL = strSQL + "'" + Sn + "', ";
            //    strSQL = strSQL + "'" + tempMessage + "',";
            //    strSQL = strSQL + "'" + T_Mbid + "', ";
            //    if (smslength <= 80)
            //    {
            //        strSQL = strSQL + "'" + T_M_Name + "', 'S' ,'" + cls_User.gid + "',0 , 1";
            //    }
            //    else
            //    {
            //        strSQL = strSQL + "'" + T_M_Name + "', 'M' ,'" + cls_User.gid + "',0 , 1";
            //    }
            //}


            //strSQL = "EXEC Usp_Insert_SMS '99', '" + cls_User.gid  + "', '" + T_Mbid + "', '" + tempMessage + "', '" + CallTel + "'";
            DataSet ds3 = new DataSet();
            Temp_Connect.Open_Data_Set(strSQL, base_db_name, ds3);


            string MMSSeq = "성공";
            if(ds3.Tables[base_db_name].Rows.Count == 0)
            {
                MMSSeq = "실패 (수신거부대상자 혹은 번호가없음)";
            }
            if (ds3.Tables[base_db_name].Rows ==  null)
            {
                MMSSeq = "실패 (수신거부대상자 혹은 번호가없음)";
            }
            //int Seq  = int.Parse (ds3.Tables[base_db_name].Rows[0][0].ToString());
            //string Send_Month = ds3.Tables[base_db_name].Rows[0][1].ToString(); 


            //    if (smslength <= 80)
            //    {
            //        strSQL = " Exec DBO.ILS_BIZ_SEND_SMS 'SMS' ,";                
            //    }
            //    else
            //    {
            //        strSQL = " Exec DBO.ILS_BIZ_SEND_SMS 'MMS' ,";                
            //    }

            //    strSQL = strSQL + "'" + SMS_SID + "' ,";
            //    strSQL = strSQL + "'" + CallTel + "' ,";
            //    strSQL = strSQL + "'" + Sn + "', ";
            //    strSQL = strSQL + "'', ";
            //    strSQL = strSQL + "'" + tempMessage + "',";
            //    strSQL = strSQL + "'' ";                



            //    DataSet ds = new DataSet();
            //    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //    if (Temp_Connect.Open_SMSData_Set(strSQL, base_db_name, ds) == false) return;
            //    int ReCnt = Temp_Connect.DataSet_ReCount;

            //    string MMSSeq = ds.Tables[base_db_name].Rows[0][0].ToString();
            //    int identityval = int.Parse(ds.Tables[base_db_name].Rows[0]["identityval"].ToString()); 

            //    if (MMSSeq == "0000" )
            //    {

            //        strSQL = " UpDate tbl_SMS_KaRiS_Result SET  ";                
            //        strSQL = strSQL + " T_index = " + identityval;
            //        strSQL = strSQL + " Where Seq = " + Seq ;

            //        Temp_Connect.Update_Data_SMS(strSQL); 


            ////        System.Threading.Thread.Sleep(2000); //1초간 대기

            ////        string Base_Log_Table = "ums_log_" + Send_Month;

            ////        DataSet ds2 = new DataSet();
            ////        int ReCnt2 = 0;

            ////Check_Result:
            ////        ReCnt2 = 0 ;
            ////        strSQL = " Select call_result From  " + Base_Log_Table + " (nolock) " ;
            ////        strSQL = strSQL + " Where cmid = " + identityval ;


            ////        //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            ////        Temp_Connect.Open_SMSData_Set(strSQL, base_db_name, ds2);
            ////        ReCnt2 = Temp_Connect.DataSet_ReCount;

            ////        if (ReCnt2 > 0)
            ////        {                   
            ////            nRet = int.Parse(ds2.Tables[base_db_name].Rows[0][0].ToString());

            ////            strSQL = " UpDate tbl_SMS_KaRiS_Result SET  ";
            ////            strSQL = strSQL + " S_Result = '" + nRet + "'";
            ////            strSQL = strSQL + " Where T_index = " + identityval;

            ////            Temp_Connect.Update_Data_SMS(strSQL); 
            ////        }
            ////        else
            ////        {
            ////            ds2.Clear(); ds2.Dispose();
            ////            goto Check_Result;
            ////        }



            //    }
            //    else
            //    {
            //        nRet = -302 ; 
            //    }


            //if (MMSSeq != "0000")
            //{
            //    nRet = -302;
            //}
            int T_G_Cnt = rowCnt + 1 ;

            //if (nRet == 4100 || nRet == 6600)
            //if (nRet != -302)
            if(MMSSeq.Equals("성공"))
            {
                txt_Send.Text = txt_Send.Text + "NO." + T_G_Cnt + "번 " + T_M_Name + "님 메세지 전송 성공!";
                txt_Send.Text = txt_Send.Text + Environment.NewLine; txt_Send.Refresh();
                dGridView_Base_2.Rows[rowCnt].Cells[4].Value = "전송성공";
                tempOK++;
            }
            else
            {
                txt_Send.Text = txt_Send.Text + "NO." + T_G_Cnt + "번 " + T_M_Name + "님 메세지 전송 실패!";
                txt_Send.Text = txt_Send.Text + Environment.NewLine; txt_Send.Refresh();
                dGridView_Base_2.Rows[rowCnt].Cells[4].Value = "전송실패";
                tempBad++;
            }
        }




        private void loadExcel_Sheet()
        {

            //dsExcels = new DataSet();
            //var extension = Path.GetExtension(txtFilePath.Text).ToLower();
            //using (var stream = new FileStream(txtFilePath.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //{

            //    IExcelDataReader reader = null;
            //    if (extension == ".xls")
            //    {
            //        reader = ExcelReaderFactory.CreateBinaryReader(stream);
            //    }
            //    else if (extension == ".xlsx")
            //    {
            //        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //    }
            //    else if (extension == ".csv")
            //    {
            //        reader = ExcelReaderFactory.CreateCsvReader(stream);
            //    }

            //    if (reader == null)
            //        return;

            //    dsExcels = reader.AsDataSet(new ExcelDataSetConfiguration()
            //    {
            //        UseColumnDataType = false,
            //        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
            //        {
            //            UseHeaderRow = true
            //        }
            //    });

            //}

            //foreach (DataTable dt in dsExcels.Tables)
            //{
            //    combo_Sheet.Items.Add(dt.TableName);
            //}


            //Load_TF = 1;




            String strConnectionString = @"Data Source=" + txtFilePath.Text + "; Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties=Excel 12.0;";
            OleDbConnection con = new OleDbConnection(strConnectionString);
            con.Open();

            DataTable tDT = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            combo_Sheet.Items.Clear();
            if (tDT != null)
            {
                for (int i = 0; i < tDT.Rows.Count; i++)
                {
                    combo_Sheet.Items.Add(tDT.Rows[i]["TABLE_NAME"].ToString().Trim('\'').Replace("$", ""));
                }
            }

            tDT.Dispose();
            con.Close();
            con.Dispose();

            Load_TF = 1;
        }


        private void LoadNewFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            System.Windows.Forms.DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                txtFilePath.Text = ofd.FileName;


                try
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    loadExcel_Sheet();

                }

                catch (System.Exception theException)
                {

                    String errorMessage;
                    errorMessage = "Error: ";
                    errorMessage = String.Concat(errorMessage, theException.Message);
                    errorMessage = String.Concat(errorMessage, " Line: ");
                    errorMessage = String.Concat(errorMessage, theException.Source);


                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Export_Err"));

                    if (cls_User.gid == cls_User.SuperUserID)
                        MessageBox.Show(theException.Message);
                }
                finally
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }

            }
        }


        private void butt_Search_Click(object sender, EventArgs e)
        {

            dGridView_Base_Header_2_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_2.d_Grid_view_Header_Reset();


            int RCnt = dGridView_Base_Excel.Rows.Count - 2;

            if (RCnt > 0)
            {
                //dGridView_Base_Excel.Rows.Clear();
                //dGridView_Base_Excel.Visible = false;
                //for (int TCnt = 0; TCnt <= RCnt; TCnt++)
                //    dGridView_Base_Excel.Rows.Remove(dGridView_Base_Excel.Rows[0]);


                //dGridView_Base_Excel.Rows.Remove(dGridView_Base_Excel.Rows[0]);
                //dGridView_Base_Excel.Visible = true;
            }

            


            //dGridView_Base_Excel.Rows.Clear();
            txtFilePath.Text = "";
            combo_Sheet.Items.Clear();
            Load_TF = 0;
            LoadNewFile();
        }


        private void loadExcelToDataGrid()
        {
           

            String strConnectionString = @"Data Source=" + txtFilePath.Text + "; Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties=Excel 12.0;";
            OleDbConnection con = new OleDbConnection(strConnectionString);
            con.Open();

            OleDbCommand cmdSelect = new OleDbCommand(@"SELECT * FROM [" + combo_Sheet.Text.Trim() + "$]", con);
            OleDbDataAdapter daCSV = new OleDbDataAdapter();
            daCSV.SelectCommand = cmdSelect;
            DataSet ds = new DataSet();
            daCSV.Fill(ds);
            dGridView_Base_Excel.DataSource = ds.Tables[0];


            daCSV.Dispose();
            cmdSelect.Dispose();
            con.Close();
            con.Dispose();


            Send_Sms_Base_grid(); 

        }


        private void combo_Sheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Load_TF == 0)
                return;

            if (combo_Sheet.Text != "")
            {
                try
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    loadExcelToDataGrid();

                }

                catch (System.Exception theException)
                {

                    String errorMessage;
                    errorMessage = "Error: ";
                    errorMessage = String.Concat(errorMessage, theException.Message);
                    errorMessage = String.Concat(errorMessage, " Line: ");
                    errorMessage = String.Concat(errorMessage, theException.Source);


                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Export_Err"));

                    if (cls_User.gid == cls_User.SuperUserID)
                        MessageBox.Show(theException.Message);
                }
                finally
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
        }

        private void Send_Sms_Base_grid()
        {
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            string Mbid = "", M_Name = "", hptel = "";

            for (int i = 0; i <= dGridView_Base_Excel.Rows.Count - 2; i++)
            {
                
                Mbid = dGridView_Base_Excel.Rows[i].Cells[0].Value.ToString();
                M_Name = dGridView_Base_Excel.Rows[i].Cells[1].Value.ToString();
                hptel = dGridView_Base_Excel.Rows[i].Cells[2].Value.ToString();

                if (hptel.Replace ("-","") != "" && (hptel.Substring(0, 3) == "010" || hptel.Substring(0, 3) == "011" || hptel.Substring(0, 3) == "016" || hptel.Substring(0, 3) == "017" || hptel.Substring(0, 3) == "018" || hptel.Substring(0, 3) == "019"))
                {
                    //int Cnt = 0, SW = 0;
                    //while (Cnt <= dGridView_Base_2.Rows.Count - 1 && SW == 0)
                    //{
                    //    if (dGridView_Base_2.Rows[Cnt].Cells[1].Value.ToString() == Mbid)
                    //    {
                    //        dGridView_Base_Excel.Rows[i].Visible = false;
                    //        SW = 1;
                    //        if (dGridView_Base_2.Rows[Cnt].Visible == false)
                    //        {
                    //            dGridView_Base_2.Rows[Cnt].Cells[0].Value = "";
                    //            dGridView_Base_2.Rows[Cnt].Visible = true;
                    //        }
                    //    }
                    //    Cnt++;
                    //}
                    //if (SW == 0)
                    //{
                        object[] row0 = { "V", Mbid, M_Name, hptel, "" };
                        cgb_2.basegrid.Rows.Add(row0);
                        //dGridView_Base_Excel.Rows[i].Visible = false;
                    //}
                }
                
            }      

        }










        private void butt_Search2_Click(object sender, EventArgs e)
        {
            cls_form_Meth cm = new cls_form_Meth();
            if (butt_Search2.Text == cm._chang_base_caption_search("파일찾기"))
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.DefaultExt = "jpg";
                //openFile.Filter = "Graphics interchange Format (*.jpg)|*.jpg|All files(*.*)|*.*";

                openFile.Filter = "All files(*.*)|*.*";


                openFile.ShowDialog();

                if (openFile.FileName.Length > 0)
                {
                    fileName = System.IO.Path.GetFileName(openFile.FileName);
                    txtFile.Text = openFile.FileName;
                    txtFile.Tag = "";

                    System.IO.FileInfo fi = new System.IO.FileInfo(@txtFile.Text);
                    long nSize = fi.Length;

                    if (nSize > 60000)
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_FileSize_Over"));
                        txtFile.Text = ""; txtFile.Tag = "";
                    }
                }
            }
        }





        private Boolean Board_Input(ref int Seq)
        {
            //tbl_Board_File
            //tbl_Board

            //string U_TIP = "220.117.241.173";
            //string U_Port = "10250";
            //string Ftp_BaseDir ="/APYLD";

            try
            {
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                string strSQL = "";

                if (txtFile.Text != "")
                {
                    int SeqNum = 0;
                    //string Full_fileName = "E:/unitelSmsFile/upload/APYLD/";
                    string Full_fileName = "APYLD";

                    strSQL = " Exec Usp_Board_File_Up 'M' ,";
                    strSQL = strSQL + SeqNum + ", ";
                    strSQL = strSQL + "'" + fileName + "',";
                    strSQL = strSQL + "'" + Full_fileName + "',";
                    strSQL = strSQL + "'" + cls_User.gid + "' ";


                    DataSet ds_2 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(strSQL, "Board", ds_2) == false) return false;
                    int ReCnt_2 = Temp_Connect.DataSet_ReCount;

                    Seq = int.Parse(ds_2.Tables["Board"].Rows[0][0].ToString());

                    if (Seq == 0)
                        return false;

                    Boolean C_TF = Up_File(Seq);

                    if (C_TF == false)
                    {
                        strSQL = "Insert into tbl_Board_File_Mod ";
                        strSQL = strSQL + " Select * ,'fail', Convert(varchar,getdate(),21) From tbl_Board_File  (nolock) ";
                        strSQL = strSQL + " Where Seq = " + Seq;

                        Temp_Connect.Insert_Data(strSQL, "tbl_Board_File");


                        strSQL = "Delete From tbl_Board_File  ";
                        strSQL = strSQL + " Where Seq = " + Seq;

                        Temp_Connect.Insert_Data(strSQL, "tbl_Board_File");

                        return false;
                    }


                    MMS_fileName = Full_fileName + Seq.ToString() + ".jpg";
                    

                }

                return true;

            }
            catch (Exception ex1)
            {
                if (cls_User.gid == cls_User.SuperUserID)
                    MessageBox.Show(ex1.Message);

                return false;
            }


        }



        private Boolean Up_File(int Seq)
        {

            string U_TIP = "220.117.241.173";
            string U_Port = "10250";
            //string Ftp_BaseDir = "/APYLD/";
            string Ftp_BaseDir = "/APYLD";
            

            string U_TID = cls_app_static_var.app_FTP_ID;
            string U_TPW = cls_app_static_var.app_FTP_PW;

            if (U_TIP != "")
            {
                try
                {
                    string Full_fileName = "ftp://" + U_TIP + ":" + U_Port + Ftp_BaseDir + Seq.ToString() + ".jpg";


                    WebClient client = new WebClient();
                    NetworkCredential nc = new NetworkCredential(U_TID, U_TPW);

                    Uri addy = new Uri(Full_fileName);

                    client.Credentials = nc;
                    client.UploadFile(addy, txtFile.Text);

                    return true;
                }
                catch (Exception ex1)
                {
                    if (cls_User.gid == cls_User.SuperUserID)
                        MessageBox.Show(ex1.Message);

                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        private void btnExcelTemplateDownload_Click(object sender, EventArgs e)
        {
            cls_Grid_Base cgb = new cls_Grid_Base();
            cgb.basegrid = dGridView_Base_3;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            
            string[] g_HeaderText = {"회원번호", "성명", "핸드폰"};
            cgb.grid_col_header_text = g_HeaderText;
            cgb.grid_col_Count = g_HeaderText.Length;

            int[] g_Width = { 120, 120, 120 };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true, true };
            cgb.grid_col_Lock = g_ReadOnly;


            DataGridViewContentAlignment[] g_Alignment = { DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft };
            cgb.grid_col_alignment = g_Alignment;

            cgb.d_Grid_view_Header_Reset();

            int idx = dGridView_Base_3.Rows.Add();
            dGridView_Base_3.Rows[idx].Cells[0].Value = "1001111";
            dGridView_Base_3.Rows[idx].Cells[1].Value = "홍길동";
            dGridView_Base_3.Rows[idx].Cells[2].Value = "010-1234-5678";
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info_3);
            e_f.ShowDialog();


            dGridView_Base_3.Rows.Clear();
            dGridView_Base_3.Columns.Clear();
        }
    }
}
