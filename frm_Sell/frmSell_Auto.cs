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
    public partial class frmSell_Auto : clsForm_Extends
    {
       

         StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
        
        cls_Grid_Base cgb = new cls_Grid_Base();
        private const string base_db_name = "tbl_Sales_AutoShip";
        private int Data_Set_Form_TF;


        public frmSell_Auto()
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


            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            mtxtCDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtCDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtMbid.Focus();
        }


        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (cls_User.uSearch_MemberNumber != "")
            {
                Data_Set_Form_TF = 1;
                mtxtMbid.Text = cls_User.uSearch_MemberNumber;
                cls_User.uSearch_MemberNumber = "";

                EventArgs ee1 = null; Base_Button_Click(butt_Select, ee1);  //butt_Search

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
            cfm.button_flat_change(butt_Check_01);
            cfm.button_flat_change(butt_Check_02);
            cfm.button_flat_change(butt_Save);  
            
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

                        }
                    }
                }

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
           
            //cls_Check_Input_Error c_er = new cls_Check_Input_Error();

            //if (mtxtCDate1.Text.Replace("-", "").Trim() == "")
            //{
            //    MessageBox.Show("조회 일자를 입력해 주십시요."                       
            //          + "\n" +
            //          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    mtxtCDate1.Focus();
            //    return false;
            //}


            //if (mtxtCDate2.Text.Replace("-", "").Trim() == "")
            //{
            //    mtxtCDate2.Text = mtxtCDate1.Text;
            //}


            //if (mtxtCDate1.Text.Replace("-", "").Trim() != "")
            //{
            //    if (Sn_Number_(mtxtCDate1.Text, mtxtCDate1, "Date") == false)
            //    {
            //        mtxtCDate1.Focus();
            //        return false;
            //    }
            //}

            //if (mtxtCDate2.Text.Replace("-", "").Trim() != "")
            //{
            //    if (Sn_Number_(mtxtCDate2.Text, mtxtCDate2, "Date") == false)
            //    {
            //        mtxtCDate2.Focus();
            //        return false;
            //    }

            //}
      

            return true;
        }

        private void Make_Base_Query(ref string Tsql)
        {
            Tsql = " Select '' ";
            Tsql = Tsql + " , tbl_Memberinfo_AutoShip.Auto_Seq ";
            Tsql += ", MC1.FlagName AS ReqState_Name ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql += Environment.NewLine + " , tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) AS M_Mbid ";
            else
                Tsql = Tsql + " , tbl_Memberinfo.mbid2 AS M_Mbid ";

            Tsql += Environment.NewLine + " , tbl_memberinfo.M_Name ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.hptel ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo_AutoShip.TotalPrice ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo_AutoShip.TotalPV ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo_AutoShip.TotalCV ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo_AutoShip.Proc_Date ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo_AutoShip_Rece.Rec_Name ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo_AutoShip_Rece.Rec_Addcode ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo_AutoShip_Rece.Rec_Address1 ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo_AutoShip_Rece.Rec_Address2 ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo_AutoShip_Rece.Rec_Tel ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo_AutoShip.mbid ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo_AutoShip.mbid2 ";
            Tsql += Environment.NewLine + " From tbl_Memberinfo_AutoShip (nolock) ";
            Tsql += Environment.NewLine + " Left Outer Join tbl_Memberinfo_AutoShip_Rece (nolock) ON tbl_Memberinfo_AutoShip.Auto_Seq = tbl_Memberinfo_AutoShip_Rece.Auto_Seq ";
            Tsql += Environment.NewLine + " Left Outer Join tbl_Memberinfo (nolock) ON tbl_Memberinfo_AutoShip.mbid = tbl_Memberinfo.mbid And tbl_Memberinfo_AutoShip.mbid2 = tbl_Memberinfo.mbid2 ";
            Tsql += Environment.NewLine + " LEFT OUTER JOIN tbl_MasterCode MC1 (nolock) ON tbl_Memberinfo_AutoShip.Req_State = MC1.FlagCode AND MC1.ClassCode = '001' AND MC1.ModuleCode = 'AutoShip' ";

        }

        private void Make_Base_Query_(ref string Tsql)
        {
            //20191025 구현호 Req_State 30(단순히 결제실패)뿐아니라 1,2회차 결제실패도 전부 나와야함
            //string strSql = " Where tbl_Memberinfo.LeaveCheck = 1 AND tbl_Memberinfo_AutoShip.Req_State <> 99  ";
            //string strSql = " Where tbl_Memberinfo.LeaveCheck = 1 AND tbl_Memberinfo_AutoShip.Req_State <> 99 AND tbl_Memberinfo_AutoShip.Req_State <> 20  ";
            string strSql = " Where tbl_Memberinfo.LeaveCheck = 1 AND right(tbl_Memberinfo_AutoShip.proc_date, 2) <>  tbl_Memberinfo_AutoShip.Month_Date and tbl_Memberinfo_AutoShip.Req_State <> 99  ";

            if (mtxtMbid.Text.Replace("_", "").Trim() != "")
            {
                strSql += Environment.NewLine + " AND tbl_Memberinfo.MBID2 LIKE '%" + mtxtMbid.Text+"%'";
            }
            if (txtName.Text.Replace(" ", "").Trim() != "")
            {
                strSql += Environment.NewLine + " AND tbl_Memberinfo.M_NAME LIKE '%" + txtName.Text + "%'";
            }
            //";// AND tbl_Memberinfo_AutoShip.PROC_DATE = '20190825'";//

            //if ((mtxtCDate1.Text.Replace(" - ", "").Trim() != "") && (mtxtCDate1.Text.Replace("-", "").Trim() == ""))
            //    strSql += Environment.NewLine + " And tbl_Memberinfo_AutoShip.Proc_Date = '" + mtxtCDate1.Text.Replace("-", "").Trim() + "'";

            //if ((mtxtCDate1.Text.Replace("-", "").Trim() != "") && (mtxtCDate2.Text.Replace("-", "").Trim() != ""))
            //{
            //    strSql += Environment.NewLine + " And tbl_Memberinfo_AutoShip.Proc_Date >= '" + mtxtCDate1.Text.Replace("-", "").Trim() + "'";
            //    strSql += Environment.NewLine + " And tbl_Memberinfo_AutoShip.Proc_Date <= '" + mtxtCDate2.Text.Replace("-", "").Trim() + "'";
            //}
            //strSql += Environment.NewLine + " And ISNULL(tbl_Memberinfo_AutoShip.End_Date, '') = '' ";
            strSql += Environment.NewLine + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
            strSql += Environment.NewLine + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

            Tsql = Tsql + Environment.NewLine + strSql;
            Tsql += Environment.NewLine + " Order by tbl_Memberinfo_AutoShip.Auto_Seq ASC ";
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
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name , this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();     
        }


        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 17;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;            
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"선택"   , "자동주문번호"    , "상태"     , "회원번호"  , "회원명"   
                                    , "연락처", "결제금액"   , "총 PV"    , "총 CV"     , "결제일" 
                                    , "수령인명", "배송우편번호", "배송주소", "배송상세주소", "수령인연락처"
                                    , "_mbid" , "_mbid2"
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 85, 100, 80, 130, 100
                            , 100, 80, 80 , 0 ,80
                            , 130, 80 , 80 , 80 , 80
                            , 0 ,0
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true      
                                    ,true , true,  true,  true ,true      
                                    ,true , true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleLeft //5

                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleRight                          
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight             
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
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb.grid_cell_format = gr_dic_cell_format;
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                            , ds.Tables[base_db_name].Rows[fi_cnt][1]  
                            , ds.Tables[base_db_name].Rows[fi_cnt][2]  
                            , ds.Tables[base_db_name].Rows[fi_cnt][3]
                            , ds.Tables[base_db_name].Rows[fi_cnt][4]

                            , ds.Tables[base_db_name].Rows[fi_cnt][5]
                            , ds.Tables[base_db_name].Rows[fi_cnt][6]
                            , ds.Tables[base_db_name].Rows[fi_cnt][7]
                            , ds.Tables[base_db_name].Rows[fi_cnt][8]
                            , ds.Tables[base_db_name].Rows[fi_cnt][9]

                            , ds.Tables[base_db_name].Rows[fi_cnt][10]
                            , ds.Tables[base_db_name].Rows[fi_cnt][11]
                            , ds.Tables[base_db_name].Rows[fi_cnt][12]
                            , ds.Tables[base_db_name].Rows[fi_cnt][13]
                            , ds.Tables[base_db_name].Rows[fi_cnt][14]

                            , ds.Tables[base_db_name].Rows[fi_cnt][15]
                            , ds.Tables[base_db_name].Rows[fi_cnt][16]

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

        }


        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {

        }


        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtData_TextChanged(object sender, EventArgs e)
        {

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

            if (tb.Name == "txtR_Id")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtR_Id_Code);
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtChange")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtChange_Code);
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
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtChange")
                cgb_Pop.Next_Focus_Control = mtxtCDate1;


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
            cgb_Pop.Change_Header_Text_TF = true;

            if (strSql != "")
            {
                if (tb.Name == "txtCenter")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtBank")
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);

                if (tb.Name == "txtChange")
                    cgb_Pop.db_grid_Popup_Base(2, "", "변경내역", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, strSql);
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

                if (tb.Name == "txtChange")
                {
                    string Tsql;
                    Tsql = "Select M_Detail ," + cls_app_static_var.Base_M_Detail_Ex + " ";
                    Tsql = Tsql + " From tbl_Memberinfo_Talk_Mod_Detail (nolock) ";
                    Tsql = Tsql + " Order by " + cls_app_static_var.Base_M_Detail_Ex ;

                    cgb_Pop.db_grid_Popup_Base(2, "", "변경내역", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, Tsql);
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
                Tsql = Tsql + " Where (Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And   Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
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

            if (tb.Name == "txtChange")
            {                
                Tsql = "Select M_Detail ," + cls_app_static_var.Base_M_Detail_Ex + " ";
                Tsql = Tsql + " From tbl_Memberinfo_Talk_Mod_Detail (nolock) ";
                Tsql = Tsql + " Where " + cls_app_static_var.Base_M_Detail_Ex + " like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " Order by " + cls_app_static_var.Base_M_Detail_Ex;                
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
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtMbid);

           
                tab_Chart.SelectedIndex = 0; 
                //radioB_S.Checked = true; 
                
            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                tab_Chart.SelectedIndex = 0; 
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                if (Check_TextBox_Error() == false) return;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
              

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
                    bt.Text =".";
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
            Excel_Export_File_Name = this.Text; // "Member_Select_Change";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }

       

        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            //if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            //{
            //    string Send_Nubmer = ""; string Send_Name = "";
            //    Send_Nubmer = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
            //    Send_Name = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
            //    Send_Mem_Number(Send_Nubmer, Send_Name);   //부모한테 이벤트 발생 신호한다.
            //}            
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
            ct.Search_Date_TextBox_Put(mtxtCDate1, mtxtCDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }


        private void Base_Sub_Button_Click(object sender, EventArgs e)
        {

            Button bt = (Button)sender;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            if (bt.Name == "butt_Check_01")
            {
                dGridView_Base.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dGridView_Base.Visible = false;
                for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
                {
                    dGridView_Base.Rows[i].Cells[0].Value = "V";
                }
                dGridView_Base.Visible = true;
            }


            else if (bt.Name == "butt_Check_02")
            {
                dGridView_Base.Visible = false;
                dGridView_Base.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
                {
                    dGridView_Base.Rows[i].Cells[0].Value = "";
                }
                dGridView_Base.Visible = true;
            }

            else if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0;

                progress.Visible = true; butt_Save.Enabled = false;
                
                Save_Base_Data(ref Save_Error_Check);
                progress.Visible = false; butt_Save.Enabled = true;

                if (Save_Error_Check > 0)
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                    cgb.d_Grid_view_Header_Reset();
                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<  
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    Base_Grid_Set();  //뿌려주는 곳
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private Boolean Save_Check()
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            Tsql = " Select T_Index From tbl_AutoShip_Log ";
            Tsql = Tsql + " Where CloseTF = 'F' ";
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return false;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {
                //20200224 천경효님이 f떠도 재결재하라고함 
               // MessageBox.Show("다른 사용자가 AutoShip 결제를 진행하고 있습니다.");
                //return false;
            }

            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                Tsql = "INSERT INTO tbl_AutoShip_Log (CloseTF, StartDate, RecordID) VALUES (";
                Tsql = Tsql + " 'F', Convert(Varchar(25),GetDate(),21), '" + cls_User.gid + "' ";
                Tsql = Tsql + " )";

                Temp_Connect.Insert_Data(Tsql, "tbl_AutoShip_Log", this.Name, this.Text);

                tran.Commit();
                return true;
            }
            catch (Exception ee)
            {
                tran.Rollback();
                return false;
            }
            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }
        }

        private Boolean Base_Sell_Table_Make(string Auto_Seq, string SellDate, int idx_Mbid2, string M_Name, string ItemCount_Chk, ref string OrderNumber)
        {

            string ToEndDate = cls_User.gid_date_time;
            string SellSort = "";
            string StrSql = "",  T_index = "", T_CenterCode = "" ;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();


 
                SellSort = "BC";


            try
            {
                StrSql = "EXEC Usp_Insert_Tbl_Sales_OrderNumber_CS '', " + idx_Mbid2 + ", '" + SellDate + "', '"+ T_CenterCode + "'";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                Temp_Connect.Open_Data_Set(StrSql, "tbl_Sales_OrdNumber", ds) ; 
                int ReCnt = Temp_Connect.DataSet_ReCount; 

                if (ReCnt > 0)
                {
                    OrderNumber = ds.Tables["tbl_Sales_OrdNumber"].Rows[0]["OrderNumber"].ToString();

                    //무통장입금처리-tbl_sales_cacu에 카드정보가 아닌 무통장입금정보를 넣는다.
                    //if (chb_bank.Checked ==true)
                    //{
                    //    StrSql = " EXEC Usp_Insert_AutoShip_SalesTable_bank '" + OrderNumber + "', '" + Auto_Seq + "', '" + SellDate + "' ";
                    //}
                    //else
                    //{
                        StrSql = " EXEC Usp_Insert_AutoShip_SalesTable '" + OrderNumber + "', '" + Auto_Seq + "', '" + SellDate + "' ";
                    //}
                    Temp_Connect.Insert_Data(StrSql, "tbl_SalesDetail", this.Name.ToString(), this.Text);


                    /*재고 차감*/
                    //DataSet ds2 = new DataSet();
                    //StrSql = " Select ";
                    //StrSql = StrSql + " A.ItemCode, A.ItemCount ";
                    //StrSql = StrSql + " From tbl_Memberinfo_AutoShip_Item (nolock) A ";
                    //StrSql = StrSql + " Where A.Auto_Seq = '" + Auto_Seq + "'";

                    //Temp_Connect.Open_Data_Set(StrSql, "tbl_RealCount", ds2);
                    //int ReCnt2 = Temp_Connect.DataSet_ReCount;
                    //string Tsql = "", ItemCode = "";
                    //int ItemCount = 0;

                    //for (int i = 0; i < ReCnt2; i++)
                    //{
                    //    ItemCode = ds2.Tables["tbl_RealCount"].Rows[i]["ItemCode"].ToString();
                    //    ItemCount = int.Parse(ds2.Tables["tbl_RealCount"].Rows[i]["ItemCount"].ToString()) * -1;

                    //    Tsql = " EXEC Usp_RealTime_Count_PlusMinus 'AUTO', '" + OrderNumber + "', '" + ItemCode + "', " + ItemCount + ", '3321'  ";
                    //    Temp_Connect.Update_Data(Tsql, this.Name, this.Text);
                    //}

                    //cls_Search_DB csd_2 = new cls_Search_DB();
                    /*교원 랭크업보너스1
                    
                    if (int.Parse(csd_2.Select_Today("yyyyMMdd")) >= int.Parse("20180305") && int.Parse(csd_2.Select_Today("yyyyMMdd")) <= int.Parse("20180402"))
                    {
                        StrSql = " EXEC Usp_KyowonPromotion_1 '" + OrderNumber + "', 2, '" + cls_User.gid + "' ";
                        Temp_Connect.Insert_Data(StrSql, "tbl_SalesItemDetail", this.Name.ToString(), this.Text);
                    }
                    */

                    ///*교원 랭크업보너스3*/
                    //if (int.Parse(csd_2.Select_Today("yyyyMMdd")) >= int.Parse("20180614") && int.Parse(csd_2.Select_Today("yyyyMMdd")) <= int.Parse("20180703"))
                    //{
                    //    StrSql = string.Format(" EXEC Usp_KyowonPromotion_3 '{0}', 2, '{1}'" , OrderNumber, cls_User.gid);
                    //    Temp_Connect.Insert_Data(StrSql, "tbl_SalesItemDetail", this.Name.ToString(), this.Text);
                    //}
                }


                return true;
            }
            catch (Exception ee)
            {
                //tran.Rollback();
                //StrSql = " Update tbl_AutoShip_Log SET ";
                //StrSql = StrSql + " CloseTF = 'E' ";
                //StrSql = StrSql + " , EndDate = Convert(Varchar(25),GetDate(),21) ";
                //StrSql = StrSql + " Where CloseTF = 'F' ";

                //Temp_Connect.Update_Data(StrSql, "", "");

                //MessageBox.Show("결제 진행 중 에러가 발생했습니다.");
                return false;
            }
        }


        private void Chang_Sucess(string Auto_Seq,string OrderNumber, cls_Connect_DB Temp_Connect)
        {
            string StrSql = "";
            //정상결제되면 Del_TF = 1, 결제실패면 = 2, 수정이면 = 0, 삭제면 = 3

            StrSql = " INSERT INTO tbl_Memberinfo_AutoShip_Mod_Del ";
            StrSql = StrSql + " Select *, 1, '" + cls_User.gid + "', Convert(Varchar(25),GetDate(),120), '" + OrderNumber + "' , '' ";
            StrSql = StrSql + " From tbl_Memberinfo_AutoShip (nolock) ";
            StrSql = StrSql + " Where Auto_Seq = '" + Auto_Seq + "'";
            Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_AutoShip_Mod_Del", this.Name, this.Text);

            StrSql = " INSERT INTO tbl_Memberinfo_AutoShip_Item_Mod_Del ";
            StrSql = StrSql + " Select *, 1, '" + cls_User.gid + "', Convert(Varchar(25),GetDate(),120), '" + OrderNumber + "' ";
            StrSql = StrSql + " From tbl_Memberinfo_AutoShip_Item (nolock) ";
            StrSql = StrSql + " Where Auto_Seq = '" + Auto_Seq + "'";
            StrSql = StrSql + " Order by ItemIndex ASC ";
            Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_AutoShip_Item_Mod_Del", this.Name, this.Text);

            StrSql = " INSERT INTO tbl_Memberinfo_AutoShip_Cacu_Mod_Del ";
            StrSql = StrSql + " Select *, 1, '" + cls_User.gid + "', Convert(Varchar(25),GetDate(),120), '" + OrderNumber + "' , '' ";
            StrSql = StrSql + " From tbl_Memberinfo_AutoShip_Cacu (nolock) ";
            StrSql = StrSql + " Where Auto_Seq = '" + Auto_Seq + "'";
            StrSql = StrSql + " Order By CacuIndex ASC ";
            Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_AutoShip_Cacu_Mod_Del", this.Name, this.Text);

            StrSql = " INSERT INTO tbl_Memberinfo_AutoShip_Rece_Mod_Del ";
            StrSql = StrSql + " Select *, 1, '" + cls_User.gid + "', Convert(Varchar(25),GetDate(),120), '" + OrderNumber + "' ";
            StrSql = StrSql + " From tbl_Memberinfo_AutoShip_Rece (nolock) ";
            StrSql = StrSql + " Where Auto_Seq = '" + Auto_Seq + "'";
            StrSql = StrSql + " Order By RecIndex ASC ";
            Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_AutoShip_Rece_Mod_Del", this.Name, this.Text);
        }

        private void Chang_Fail(string Auto_Seq, string OrderNumber, cls_Connect_DB Temp_Connect, string TT_Ret )
        {

            string StrSql = "";
            //정상결제되면 Del_TF = 1, 결제실패면 = 2, 수정이면 = 0, 삭제면 = 3

            StrSql = " INSERT INTO tbl_Memberinfo_AutoShip_Mod_Del ";
            StrSql = StrSql + " Select *, 2, '" + cls_User.gid + "', Convert(Varchar(25),GetDate(),120), '" + OrderNumber + "', '" + TT_Ret + "' ";
            StrSql = StrSql + " From tbl_Memberinfo_AutoShip (nolock) ";
            StrSql = StrSql + " Where Auto_Seq = '" + Auto_Seq + "'";
            Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_AutoShip_Mod_Del", this.Name, this.Text);

            StrSql = " INSERT INTO tbl_Memberinfo_AutoShip_Item_Mod_Del ";
            StrSql = StrSql + " Select *, 2, '" + cls_User.gid + "', Convert(Varchar(25),GetDate(),120), '" + OrderNumber + "' ";
            StrSql = StrSql + " From tbl_Memberinfo_AutoShip_Item (nolock) ";
            StrSql = StrSql + " Where Auto_Seq = '" + Auto_Seq + "'";
            StrSql = StrSql + " Order by ItemIndex ASC ";
            Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_AutoShip_Item_Mod_Del", this.Name, this.Text);

            StrSql = " INSERT INTO tbl_Memberinfo_AutoShip_Cacu_Mod_Del ";
            StrSql = StrSql + " Select *, 2, '" + cls_User.gid + "', Convert(Varchar(25),GetDate(),120), '" + OrderNumber + "' , '" + TT_Ret + "' ";
            StrSql = StrSql + " From tbl_Memberinfo_AutoShip_Cacu (nolock) ";
            StrSql = StrSql + " Where Auto_Seq = '" + Auto_Seq + "'";
            StrSql = StrSql + " Order By CacuIndex ASC ";
            Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_AutoShip_Cacu_Mod_Del", this.Name, this.Text);

            StrSql = " INSERT INTO tbl_Memberinfo_AutoShip_Rece_Mod_Del ";
            StrSql = StrSql + " Select *, 2, '" + cls_User.gid + "', Convert(Varchar(25),GetDate(),120), '" + OrderNumber + "' ";
            StrSql = StrSql + " From tbl_Memberinfo_AutoShip_Rece (nolock) ";
            StrSql = StrSql + " Where Auto_Seq = '" + Auto_Seq + "'";
            StrSql = StrSql + " Order By RecIndex ASC ";
            Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_AutoShip_Rece_Mod_Del", this.Name, this.Text);
        }



        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (Sub_Check_TextBox_Error() == false) return;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();


            progress.Minimum = 0; progress.Maximum = dGridView_Base.Rows.Count;
            progress.Step = 1; progress.Value = 0;
            string StrSql = "";

            int RowCount = 0;
            for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            {
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                    RowCount++;
            }

            if (RowCount == 0)
            {
                MessageBox.Show("선택하신 내역이 없습니다.");
                this.Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            /*현재 진행중인 오토쉽이 있는지 확인*/
            if (Save_Check() == false)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }


            string SellDate = "", OrderNumber = "", M_Name = "", Auto_Seq = "", SellDate_Auto = "";
            string Procedure = "";
            int idx_Mbid2 = 0;

            for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            {
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    try
                    {
                        /*변수초기화*/
                        Procedure = "";
                        Auto_Seq = "";
                        SellDate = "";
                        OrderNumber = "";
                        M_Name = "";
                        idx_Mbid2 = 0;

                        Auto_Seq = dGridView_Base.Rows[i].Cells[1].Value.ToString();

                        cls_Search_DB csd = new cls_Search_DB();

                        SellDate = csd.Select_Today("yyyyMMdd");
                        SellDate_Auto = dGridView_Base.Rows[i].Cells[9].Value.ToString().Replace("-", "");
                        idx_Mbid2 = int.Parse(dGridView_Base.Rows[i].Cells[16].Value.ToString());
                        M_Name = dGridView_Base.Rows[i].Cells[4].Value.ToString();

                        cls_Web Cls_Web = new cls_Web();

                        string ItemCount_Chk = "Y";

                        //if (Check_Item_Real_Count(Auto_Seq) == false)
                        //{
                        //    ItemCount_Chk = "N";
                        //    Payment_Err = "재고부족";
                        //}

                        if (Base_Sell_Table_Make(Auto_Seq, SellDate, idx_Mbid2, M_Name, ItemCount_Chk, ref OrderNumber) == true)
                        {
                            if (OrderNumber != "")
                            {

                                string TT_Ret = "";
                                string SuccessYN = "", SuccessYN_Card = "N";
                                int ReCnt_Card = 0;

                                DataSet ds_Card = new DataSet();

                                //재고가 없으면 어차피 실패니까 결제를 태우지 않음
                                if (ItemCount_Chk == "Y")
                                {
                                    //무통장입금 됐으면 무조건 성공으로 넘긴다.
                                    //if (chb_bank.Checked == true)
                                    //{

                                    //    StrSql = "EXEC Usp_Sell_Cacu_ReCul_AutoShip_CS_bank'" + OrderNumber + "','Y', '" + cls_User.gid + "'";
                                    //    Temp_Connect.Update_Data(StrSql, this.Name, this.Text);

                                    //    StrSql = " Update tbl_Memberinfo_AutoShip SET ";
                                    //    StrSql += Environment.NewLine + " Proc_Date =  (select LEFT(CONVERT(varchar, DATEADD(MONTH, 1, getdate()), 112),6) + MONTH_DATE)";
                                    //    StrSql += Environment.NewLine + " , Req_State = '20' ";
                                    //    StrSql += Environment.NewLine + " , Proc_Cnt = Proc_Cnt + 1 ";
                                    //    StrSql += Environment.NewLine + " , End_Reason = ''";
                                    //    StrSql += Environment.NewLine + " , Extend_Date = ''";//CASE WHEN Proc_Cnt <> 0 THEN CASE WHEN Proc_Cnt % 13 = 0 THEN '" + SellDate_Auto + "' ELSE Extend_Date END ELSE Extend_Date END ";
                                    //    StrSql += Environment.NewLine + " Where Auto_Seq = '" + Auto_Seq + "'";

                                    //    Temp_Connect.Update_Data(StrSql, this.Name, this.Text);

                                    //    Chang_Sucess(Auto_Seq, OrderNumber, Temp_Connect);

                                    //    //오토쉽 랭크업보너스 - 메뉴얼결제 최후로 돌린다. 3달단위 결제완료시 돌아간다. 무통장입금은 혹시모르니까 넣어둔다.
                                    //    StrSql = "EXEC Usp_Insert_Memberinfo_Autoship_Promotion '" + OrderNumber + "','" + idx_Mbid2 + "'";
                                    //    Temp_Connect.Update_Data(StrSql, this.Name, this.Text);
                                    //}
                                    //else
                                    //{
                                        /*카드로 결제할 건이 있으면 카드 결제 로직을 태운다*/

                                        StrSql = " SELECT CacuIndex FROM tbl_Memberinfo_AutoShip_Cacu (NOLOCK) WHERE Auto_Seq = '" + Auto_Seq + "' AND Cacu_Type = 3 ";

                                        Temp_Connect.Open_Data_Set(StrSql, "CardSearch", ds_Card);
                                        ReCnt_Card = Temp_Connect.DataSet_ReCount;

                                    string ErrMessage = "";
                                    if (ReCnt_Card > 0)
                                    {
                                        /*카드결제*/
                                        for (int i_Card = 0; i_Card < ReCnt_Card; i_Card++)
                                        {
                                            if (SuccessYN_Card == "N")
                                            {

                                                //SuccessYN_Card = Cls_Web.Dir_Card_AutoShip_OK(OrderNumber, int.Parse(ds_Card.Tables["CardSearch"].Rows[i_Card]["CacuIndex"].ToString()), ref ErrMessage);

                                                // 태국인경우 바로 태국전용 Function 호출 
                                                if (cls_User.gid_CountryCode == "TH")
                                                {
                                                    SuccessYN_Card = Cls_Web.Dir_Card_AutoShip_OK_TH(OrderNumber, int.Parse(ds_Card.Tables["CardSearch"].Rows[i_Card]["CacuIndex"].ToString()), ref ErrMessage);
                                                }
                                                // 한국인 경우
                                                else
                                                {
                                                    SuccessYN_Card = Cls_Web.Dir_Card_AutoShip_OK(OrderNumber, int.Parse(ds_Card.Tables["CardSearch"].Rows[i_Card]["CacuIndex"].ToString()), ref ErrMessage);
                                                }

                                            }
                                        }
                                    }

                                    /*결제 성공 유무 확인*/
                                    if (SuccessYN_Card == "Y" && ItemCount_Chk == "Y")
                                            SuccessYN = "Y";
                                        else
                                            SuccessYN = "N";

                                        if (SuccessYN_Card == "N" && ReCnt_Card > 0)
                                            ErrMessage = ErrMessage + " 카드에러";
                                        if (ItemCount_Chk == "N")
                                            ErrMessage = ErrMessage + " 재고 부족";

                                        if (SuccessYN == "Y")
                                        {
                                            Chang_Sucess(Auto_Seq, OrderNumber, Temp_Connect);

                                            StrSql = "EXEC Usp_Sell_Cacu_ReCul_AutoShip_CS '" + OrderNumber + "','Y', '" + cls_User.gid + "'";
                                            Temp_Connect.Update_Data(StrSql, this.Name, this.Text);

                                          
                                            StrSql = " Update tbl_Memberinfo_AutoShip SET ";
                                            StrSql += Environment.NewLine + " Proc_Date =  (select LEFT(CONVERT(varchar, DATEADD(MONTH, 1, getdate()), 112),6) + MONTH_DATE)";
                                            StrSql += Environment.NewLine + " , Req_State = '20' ";
                                            StrSql += Environment.NewLine + " , Proc_Cnt = Proc_Cnt + 1 ";
                                            StrSql += Environment.NewLine + " , End_Reason = ''";
                                            StrSql += Environment.NewLine + " , Extend_Date = ''";//CASE WHEN Proc_Cnt <> 0 THEN CASE WHEN Proc_Cnt % 13 = 0 THEN '" + SellDate_Auto + "' ELSE Extend_Date END ELSE Extend_Date END ";
                                            StrSql += Environment.NewLine + " Where Auto_Seq = '" + Auto_Seq + "'";

                                            Temp_Connect.Update_Data(StrSql, this.Name, this.Text);


                                            //오토쉽 랭크업보너스 - 메뉴얼결제 최후로 돌린다. 3달단위 결제완료시 돌아간다.
                                            StrSql = "EXEC Usp_Insert_Memberinfo_Autoship_Promotion '" + OrderNumber + "','"+ idx_Mbid2 + "'";
                                            Temp_Connect.Update_Data(StrSql, this.Name, this.Text);

                                            //거래 완전성공한 건을 메나싱크로 보낸다
                                            StrSql = "EXEC Usp_JDE_Insert_MK_Ord '" + OrderNumber + "'";
                                            Temp_Connect.Update_Data(StrSql, this.Name, this.Text);

                                        //StrSql = "EXEC Usp_Insert_SMS_New  '20',''," + idx_Mbid2 + ",'" + OrderNumber + "', ''";  //매출 결제 완료

                                        if (cls_User.gid_CountryCode == "TH")
                                        {
                                            StrSql = "EXEC [Usp_TH_SMS]   " + idx_Mbid2 + ",'" + OrderNumber + "','','4'";
                                            // Mail 호출 - 주문완료
                                            new cls_Web().SendMail_TH(idx_Mbid2, OrderNumber, string.Empty, string.Empty, ESendMailType_TH.orderCompleteMail);
                                        }
                                        else
                                        {
                                            StrSql = "EXEC Usp_Insert_SMS '20',''," + idx_Mbid2 + ",'" + OrderNumber + "', ''";  //매출 결제 완료
                                        }
                                        Temp_Connect.Update_Data(StrSql, this.Name, this.Text);


                                            System.Threading.Thread.Sleep(500);                                            
                                            Sell_Ac_insurancenumber(OrderNumber);//직판 관련 승인 번호를 받아온다.                
                                            


                                        }
                                        else
                                        {
                                            Chang_Fail(Auto_Seq, OrderNumber, Temp_Connect, TT_Ret);

                                            StrSql = "EXEC Usp_Sell_Cacu_ReCul_AutoShip_CS '" + OrderNumber + "','N' , '" + cls_User.gid + "'";
                                            Temp_Connect.Update_Data(StrSql, this.Name, this.Text);

                                            //결제실패 데이터 업데이트
                                            StrSql = StrSql + " Update tbl_Memberinfo_AutoShip_Mod_Del Set ";
                                            StrSql = StrSql + " Etc = '" + ErrMessage + "' ";
                                            StrSql = StrSql + " WHERE Auto_Seq = '" + Auto_Seq + "'";
                                            StrSql = StrSql + " And Proc_Date = '" + SellDate_Auto + "' ";
                                            StrSql = StrSql + " And OrderNumber = '" + OrderNumber + "' ";
                                            StrSql = StrSql + " And Del_TF = 2 ";                                                                                       

                                            Temp_Connect.Update_Data(StrSql, this.Name, this.Text);

                                        if (cls_User.gid_CountryCode == "TH")
                                        {
                                            StrSql = "EXEC [Usp_TH_SMS]   "+idx_Mbid2+",'','','2'";  //오토쉽 결제 실패
                                        }
                                        else
                                        {
                                            StrSql = "EXEC Usp_Insert_SMS_New  '24',''," + idx_Mbid2 + ",'" + Auto_Seq + "', ''";  //오토쉽 결제 실패
                                        }
                                        //StrSql = "EXEC Usp_Insert_SMS '24',''," + idx_Mbid2 + ",'" + Auto_Seq + "', ''";  //오토쉽 결제 실패
                                        Temp_Connect.Update_Data(StrSql, this.Name, this.Text);

                                        }
                                    //}

                                    
                                    ////Union_Send_Date(OrderNumber, "", idx_Mbid2);
                                }
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        StrSql = " Update tbl_AutoShip_Log SET ";
                        StrSql = StrSql + " CloseTF = 'E' ";
                        StrSql = StrSql + " , EndDate = Convert(Varchar(25),GetDate(),21) ";
                        StrSql = StrSql + " Where CloseTF = 'F' ";

                        Temp_Connect.Update_Data(StrSql, "", "");                        
                    }
                    finally
                    {
                        Temp_Connect.Close_DB();
                    }

                }
                progress.PerformStep();
            }

            try
            {

                StrSql = " Update tbl_AutoShip_Log SET ";
                StrSql = StrSql + " CloseTF = 'T' ";
                StrSql = StrSql + " , EndDate = Convert(Varchar(25),GetDate(),21) ";
                StrSql = StrSql + " Where CloseTF = 'F' ";
                Temp_Connect.Update_Data(StrSql, this.Name, this.Text);

                Save_Error_Check = 1;

            }
            catch (Exception ee)
            {
                StrSql = " Update tbl_AutoShip_Log SET ";
                StrSql = StrSql + " CloseTF = 'E' ";
                StrSql = StrSql + " , EndDate = Convert(Varchar(25),GetDate(),21) ";
                StrSql = StrSql + " Where CloseTF = 'F' ";

                Temp_Connect.Update_Data(StrSql, "", "");

                MessageBox.Show("결제 진행 중 에러가 발생했습니다.");
            }
            finally
            {
                Temp_Connect.Close_DB();
            }


        }



        private void Sell_Ac_insurancenumber(string T_ord_N)
        {
            string Req = "";
           
            cls_Socket csg = new cls_Socket();
            Req = csg.Dir_Connect_Send(T_ord_N);

            //if (Req != "Y")
            //{

            //    if (Req == "-10000")
            //        return;

            //    string MessageInsurance = string.Format("공제조합 발급이 실패되었습니다. 에러코드:{0}" + Environment.NewLine +
            //        "https://www.macco.or.kr/it/selectListSocketErrorCode.do 접속해서 에러코드 확인후에" + Environment.NewLine +
            //        "메나테크㈜ 전산담당자에게 문의하시기 바랍니다.", Req);

            //    MessageBox.Show(MessageInsurance);
            //}
            //else
            //{
            //    cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            //    string Tsql = "";

            //    Tsql = "Select  InsuranceNumber  From tbl_SalesDetail  (nolock) ";
            //    Tsql = Tsql + " Where OrderNumber = '" + T_ord_N + "'";
            //    //++++++++++++++++++++++++++++++++               

            //    DataSet ds = new DataSet();
            //    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //    if (Temp_Connect.Open_Data_Set(Tsql, "tbl_SalesDetail", ds) == false) return;
            //    int ReCnt = Temp_Connect.DataSet_ReCount;

            //    if (ReCnt > 0)

            //        txt_Ins_Number.Text = ds.Tables["tbl_SalesDetail"].Rows[0]["InsuranceNumber"].ToString();

            //    MessageBox.Show("공제번호가 정상적으로 발급 되었습니다. [공제번호 : " + txt_Ins_Number.Text + "]");
            //    Button T_bt = butt_Print; EventArgs ee1 = null;

            //}
        }


        //private Boolean Check_Item_Real_Count(string AutoSeq)
        //{
        //    string Tsql = "", Strsql = "";
        //    string ItemCode = "";
        //    int ItemCount = 0;
        //    cls_Connect_DB Temp_Connect = new cls_Connect_DB();

        //    Strsql = " SELECT ItemCode, ItemCount FROM tbl_Memberinfo_AutoShip_Item (NOLOCK) WHERE Auto_Seq = '" + AutoSeq + "' ";

        //    DataSet ds2 = new DataSet();
        //    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
        //    if (Temp_Connect.Open_Data_Set(Strsql, "ItemCode", ds2, this.Name, this.Text) == false) return false;
        //    int ReCnt2 = Temp_Connect.DataSet_ReCount;

        //    if (ReCnt2 == 0) return false;

        //    for (int i = 0; i < ReCnt2; i++)
        //    {
        //        ItemCode = ds2.Tables["ItemCode"].Rows[i]["ItemCode"].ToString();
        //        ItemCount = int.Parse(ds2.Tables["ItemCode"].Rows[i]["ItemCount"].ToString());

        //        Tsql = " SELECT dbo.UFN_GOODS_REALCOUNT_CHECK_CS ('" + ItemCode + "', '002', " + ItemCount + ") ";

        //        DataSet ds = new DataSet();
        //        //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
        //        if (Temp_Connect.Open_Data_Set(Tsql, "ItemCount_Chk", ds, this.Name, this.Text) == false) return false;
        //        int ReCnt = Temp_Connect.DataSet_ReCount;

        //        if (ReCnt == 0)
        //        {
        //            //MessageBox.Show("품목을 확인하시기 바랍니다.");
        //            return false;
        //        }

        //        if (ds.Tables["ItemCount_Chk"].Rows[0][0].ToString() == "N")
        //        {
        //            //MessageBox.Show("해당 상품의 재고가 부족합니다.\n확인하시기 바랍니다.");
        //            return false;
        //        }
        //    }

        //    return true;
        //}


        private Boolean Sub_Check_TextBox_Error()
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
                dGridView_Base.Focus(); return false;
            }

            return true;
        }




        private void dGridView_Base_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
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



        private void Union_Send_Date(string Temp_OrderNumber, string Mbid, int Mbid2)
        {
            string StrSql = "";
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();


            string Tsql = "";

            Tsql = "Select OrderNumber  From tbl_SalesDetail (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + Temp_OrderNumber + "'";
            Tsql = Tsql + " And Ga_Order = 0 And InsuranceNumber = ''  ";

            DataSet ds_s = new DataSet();
            Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds_s);
            int ReCnt_S = Temp_Connect.DataSet_ReCount;


            if (ReCnt_S > 0)
            {

                Tsql = "Select Cpno ,hptel ,hometel, Address1, Address2, Mbid , BirthDay , BirthDay_D , BirthDay_M  From tbl_Memberinfo (nolock) ";
                Tsql = Tsql + " Where Mbid = '" + Mbid + "'";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2;

                DataSet ds = new DataSet();
                Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds);
                int ReCnt = Temp_Connect.DataSet_ReCount;

                string Cpno = encrypter.Decrypt(ds.Tables["t_P_table"].Rows[0]["Cpno"].ToString(), "Cpno_U");
                string hptel = (ds.Tables["t_P_table"].Rows[0]["hptel"].ToString());
                string hometel = (ds.Tables["t_P_table"].Rows[0]["hometel"].ToString());
                string add_r1 = (ds.Tables["t_P_table"].Rows[0]["Address1"].ToString());
                string add_r2 = (ds.Tables["t_P_table"].Rows[0]["Address2"].ToString());

                if (Cpno == "" && ds.Tables["t_P_table"].Rows[0]["BirthDay"].ToString() != "")
                {
                    Cpno = ds.Tables["t_P_table"].Rows[0]["BirthDay"].ToString().Substring(2, 2) + ds.Tables["t_P_table"].Rows[0]["BirthDay_M"].ToString() + ds.Tables["t_P_table"].Rows[0]["BirthDay_D"].ToString();
                }

                StrSql = "EXEC p_mlmunion_Order_2 '" + cls_app_static_var.T_Company_Code + "','" + Temp_OrderNumber + "','" + Cpno + "','" + hptel + "','" + hometel + "','" + add_r1 + "','" + add_r2 + "',1";

                Temp_Connect.Update_Data(StrSql, this.Name.ToString(), this.Text);
            }
        }

        private void mtxtMbid_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
        }

        private void txtName_Click(object sender, EventArgs e)
        {
            mtxtMbid.Text = "";
        }
    }
}
