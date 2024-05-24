using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using System.Reflection;


namespace MLM_Program
{
    public partial class frmMember_Select_Group_Center : clsForm_Extends
    {

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);


        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cg_sub = new cls_Grid_Base();
        private const string base_db_name = "tbl_Memberinfo";
        private int Data_Set_Form_TF;

        public delegate void SendNumberDele(string Send_Number, string Send_Name);
        //public event SendNumberDele Send_Mem_Number;

        Series series_Center = new Series();
        Series series_Center_web = new Series();
        Series series_Center_Cs = new Series();
        Series series_Center_Cnt_1 = new Series();
        Series series_Center_Cnt_2 = new Series();

        public frmMember_Select_Group_Center()
        {
            InitializeComponent();

            DoubleBuffered = true;
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.SetProperty, null, dGridView_Base, new object[] { true });

        }





        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_sub.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            mtxtCDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtCDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            Save_Nom_Line_Chart();
            mtxtCDate1.Focus();
         
        }

        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

     

        }
        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Select.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_Excel.Left = butt_Select.Left + butt_Select.Width + 2;
            //butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Select);
            //cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);  
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
           // if (e.KeyValue == 115)
           //     T_bt = butt_Delete;   // 삭제  F4
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

            if (mtxtCDate1.Text.Replace("-", "").Trim() == "" && mtxtCDate2.Text.Replace("-", "").Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Requisite_Data")
                       + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Date")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtCDate1.Focus(); return false;
            }



            if (mtxtCDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtCDate1.Text, mtxtCDate1, "Date") == false)
                {
                    mtxtCDate1.Focus();
                    return false;
                }
            }

            if (mtxtCDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtCDate2.Text, mtxtCDate2, "Date") == false)
                {
                    mtxtCDate2.Focus();
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


            Tsql = "Select Center_Code , UUCC.Center_Name  , Count(tbl_Memberinfo.Mbid)    ";

            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            Tsql = Tsql + " LEFT JOIN ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') UUCC ON tbl_Memberinfo.BusinessCode = UUCC.Center_Code ";
            
        }



        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = " ";


            if ((mtxtCDate1.Text.Replace("-", "").Trim() != "") && (mtxtCDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + "Where tbl_Memberinfo.RegTime   = '" + mtxtCDate1.Text.Replace("-", "").Trim() + "'";

            //변경일자로 검색 -2
            if ((mtxtCDate1.Text.Replace("-", "").Trim() != "") && (mtxtCDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " Where tbl_Memberinfo.RegTime  >= '" + mtxtCDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And  tbl_Memberinfo.RegTime <= '" + mtxtCDate2.Text.Replace("-", "").Trim() + "'";
            }

           

            //센타코드로으로 검색
            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";


            //// strSql = strSql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";
            strSql = strSql + " Group By Center_Code , UUCC.Center_Name  ";

            Tsql = Tsql + strSql ;
            Tsql = Tsql + " Order by Center_Code  ";
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


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
           
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                //Push_data(series_Center,  ds.Tables[base_db_name].Rows[fi_cnt][1].ToString(), int.Parse(ds.Tables[base_db_name].Rows[fi_cnt][2].ToString()));
            }


            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }



        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 5;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            //cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"센타_코드"  , "센타명"   , "가입인원"  , ""   , ""                                        
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 80, 160, 80, 0, 0                               
                            };
            cgb.grid_col_w = g_Width;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb.grid_cell_format = gr_dic_cell_format;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                                                         
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5                               
                              };
            cgb.grid_col_alignment = g_Alignment;

            dGridView_Base.RowHeadersVisible = false;
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,""  //ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,"" //ds.Tables[base_db_name].Rows[fi_cnt][4]                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
            
            //Series series_Center = new Series();
            //Center_Put_Chart(series_Center, ds.Tables[base_db_name].Rows[fi_cnt][1].ToString(), int.Parse(ds.Tables[base_db_name].Rows[fi_cnt][2].ToString()));                     
        }



        //private void Push_data(Series series, string p, int p_3)
        //{
        //    DataPoint dp = new DataPoint();
        //    dp.SetValueXY(p, p_3);
        //    dp.Label = p_3.ToString();
        //    series.Points.Add(dp);
        //}

        private void Push_data(Series series, string p, double p_3)
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

        private void Center_Put_Chart(Series series_Center, string s_Name, int C_Cnt)
        {
            cls_form_Meth cm = new cls_form_Meth();
            string series_Name = cm._chang_base_caption_search("센타별"); ;            

            series_Center.Points.Clear();
            series_Center.Name = s_Name;
            series_Center.ChartArea = "ChartArea1";
            series_Center.ChartType = SeriesChartType.Column;
            series_Center.Legend = "Legend1";
            chart_Center.Series.Add(series_Center);

            Push_data(series_Center, series_Name, C_Cnt);
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

                   

                    SendKeys.Send("{TAB}");
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

            //if (tb.Name == "txtBank")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtBank_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtBank_Code);
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
                //    Db_Grid_Popup(tb, txtCenter_Code,"");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            
            //if (tb.Name == "txtBank")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtBank_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtBank_Code);

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
                    if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " And  ShowMemberCenter = 'Y' ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
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
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cg_sub.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtCDate1);

                //chart_Center.Series.Clear();
                Save_Nom_Line_Chart();
            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                if (Check_TextBox_Error() == false) return;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
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

        }


        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Member_Reg_Center";
            Excel_Export_From_Name = this.Name;
            return dGridView_Sub_001;
        }

       
        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            //SendKeys.Send("{TAB}");
        }



        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string Send_Code = ""; //string Send_Name = "";
                Send_Code = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                //Send_Name = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Sub_Grid_Set(Send_Code);
                this.Cursor = System.Windows.Forms.Cursors.Default; 
            }            
        }




        private void Make_Sub_Query(ref string StrSql)
        {

            cls_form_Meth cm = new cls_form_Meth();

            StrSql = "Select  ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) ";
            else
                StrSql = StrSql + " tbl_Memberinfo.mbid2 ";

            StrSql = StrSql + " ,tbl_Memberinfo.M_Name ";
            
            StrSql = StrSql + ", tbl_Memberinfo.Cpno ";

            StrSql = StrSql + " , tbl_Business.Name  ";


            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " ,tbl_Memberinfo.Saveid + '-' + Convert(Varchar,tbl_Memberinfo.Saveid2) ";
            else
                StrSql = StrSql + " ,tbl_Memberinfo.Saveid2 ";

            StrSql = StrSql + " , Isnull(Sav.M_Name,'') ";

            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " ,tbl_Memberinfo.Nominid + '-' + Convert(Varchar,tbl_Memberinfo.Nominid2) ";
            else
                StrSql = StrSql + " ,tbl_Memberinfo.Nominid2 ";

            StrSql = StrSql + " , Isnull(Nom.M_Name,'') ";

            StrSql = StrSql + " , Case When SSB.SS_Cnt > 0 then '" + cm._chang_base_caption_search("매출") + "' ELSE '" + cm._chang_base_caption_search("비매출") +"' End ";
            StrSql = StrSql + " , Isnull( SSB.SS_PV,0) ";
            StrSql = StrSql + " , Isnull( SSB.SS_BV ,0)";
            StrSql = StrSql + " , Isnull( SSB.SS_PR ,0)";
           
            StrSql = StrSql + " , tbl_Memberinfo.regtime ";
            StrSql = StrSql + " , tbl_Memberinfo.recordid ";
            StrSql = StrSql + " , tbl_Memberinfo.recordtime ";

            StrSql = StrSql + " From tbl_Memberinfo (nolock) ";
            StrSql = StrSql + " LEFT JOIN tbl_Memberinfo Sav (nolock) ON tbl_Memberinfo.Saveid = Sav.Mbid And tbl_Memberinfo.Saveid2 = Sav.Mbid2 ";
            StrSql = StrSql + " LEFT JOIN tbl_Memberinfo Nom (nolock) ON tbl_Memberinfo.Nominid = Nom.Mbid And tbl_Memberinfo.Nominid2 = Nom.Mbid2 ";
            StrSql = StrSql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";
            StrSql = StrSql + " LEFT JOIN (Select Isnull(Sum(Totalpv),0) SS_PV, Isnull(Sum(TotalCV),0) SS_BV, Isnull(Sum(TotalPRice),0) SS_PR , Isnull(Count(Ordernumber),0) SS_Cnt ,Mbid,Mbid2 From tbl_SalesDetail (nolock) Where Ga_order = 0 And ReturnTF = 1 Group By Mbid, Mbid2 ) SSB ";
            StrSql = StrSql + " ON SSB.Mbid = tbl_Memberinfo.Mbid And SSB.Mbid2 = tbl_Memberinfo.Mbid2  ";

        }



        private void Make_Sub_Query_(ref string Tsql,string search_Code)
        {
            string strSql = " ";

      

            if ((mtxtCDate1.Text.Replace("-", "").Trim() != "") && (mtxtCDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " Where tbl_Memberinfo.RegTime  = '" + mtxtCDate1.Text.Replace("-", "").Trim() + "'";

            //변경일자로 검색 -2
            if ((mtxtCDate1.Text.Replace("-", "").Trim() != "") && (mtxtCDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " Where tbl_Memberinfo.RegTime >= '" + mtxtCDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And   tbl_Memberinfo.RegTime <= '" + mtxtCDate2.Text.Replace("-", "").Trim() + "'";
            }

            //센타코드로으로 검색
            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";
            
            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";
            strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + search_Code + "'";
            //// strSql = strSql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";

            Tsql = Tsql + strSql;
            Tsql = Tsql + " Order by tbl_Memberinfo.Mbid , tbl_Memberinfo.Mbid2  ";
        }




        private void Sub_Grid_Set(string search_Code)
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_sub.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


            string Tsql = "";
            Make_Sub_Query(ref Tsql);

            Make_Sub_Query_(ref Tsql, search_Code);

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;                        
            
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_Sub_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            cg_sub.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cg_sub.db_grid_Obj_Data_Put();
        }


      

        private void dGridView_Sub_Header_Reset()
        {


            cg_sub.grid_col_Count = 16;
            cg_sub.basegrid = dGridView_Sub_001;
            cg_sub.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_sub.grid_Frozen_End_Count = 2;
            //cg_sub.grid_Frozen_End_Count = 2;
            cg_sub.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"회원_번호"  , "성명"   , "주민번호"  ,"성별", "센타명"                                          
                                , "후원인"  , "후원인명"   , "추천인"  , "추천인명" , "매출여부" 
                                , "누적매출PV"  , "누적매출CV"  ,"누적매출액" , "등록일"    ,"기록자"  , "기록일"
                                    };
            cg_sub.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 85, 90, 100, 0 ,90
                            , cls_app_static_var.save_uging_Pr_Flag   ,cls_app_static_var.save_uging_Pr_Flag, cls_app_static_var.nom_uging_Pr_Flag, cls_app_static_var.nom_uging_Pr_Flag, 90
                             ,120 ,120 ,90,90,90,0
                            };
            cg_sub.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                       
                                    ,true  ,true, true,  true,  true ,true      
                                   };
            cg_sub.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //10

                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter 
                              };
            cg_sub.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cg_sub.grid_cell_format = gr_dic_cell_format;
        }


        private void Set_Sub_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {

            string TCpno = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][2].ToString(), "Cpno");
            string Sex = "";
            if (TCpno.Length >= 8)
            {
                if (TCpno.Substring (7,1 ) == "1" || TCpno.Substring (7,1 ) == "3" )
                    Sex ="남";
                
                if (TCpno.Substring (7,1 ) == "2" || TCpno.Substring (7,1 ) == "4" )
                    Sex = "여";

                if (TCpno.Substring(7, 1) == "5" )
                    Sex = "남(외)";

                if (TCpno.Substring(7, 1) == "6" )
                    Sex = "여(외)";
                
            }

            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,TCpno
                                ,Sex
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

                                

                                //,ds.Tables[base_db_name].Rows[fi_cnt][15]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][16]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][17]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][18]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][19]

                                //,ds.Tables[base_db_name].Rows[fi_cnt][20]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][21]  
                                //,ds.Tables[base_db_name].Rows[fi_cnt][22]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][23]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][24]

                                //,ds.Tables[base_db_name].Rows[fi_cnt][25]
                                 };

            gr_dic_text[fi_cnt + 1] = row0;



           
        }



        private void radioB_S_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtCDate1, mtxtCDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }




        private void Save_Nom_Line_Chart()
        {
            cls_form_Meth cm = new cls_form_Meth();

            chart_Center.Series.Clear();

            //---------------------------------------------------            
            series_Center.Points.Clear();                        
            series_Center["DrawingStyle"] = "Emboss";
            series_Center["PointWidth"] = "0.5";
            series_Center.Name = cm._chang_base_caption_search("총가입인원");
            series_Center.ChartType = SeriesChartType.Column;

            //series_Center.Legend = "Legend1";
            chart_Center.Series.Add(series_Center);
            //---------------------------------------------------


       

            


            //////---------------------------------------------------                
            ////series_Center_web.Points.Clear();
                        
            ////series_Center_web["DrawingStyle"] = "Emboss";
            ////series_Center_web["PointWidth"] = "0.5";
            ////series_Center_web.Name = cm._chang_base_caption_search("Web가입");
            ////series_Center_web.ChartType = SeriesChartType.Column;

            ////chart_Center.Series.Add(series_Center_web);
            //////---------------------------------------------------



            //////---------------------------------------------------                            
            ////series_Center_Cs.Points.Clear();

            ////series_Center_Cs["DrawingStyle"] = "Emboss";
            ////series_Center_Cs["PointWidth"] = "0.5";
            ////series_Center_Cs.Name = cm._chang_base_caption_search("오프라인가입");
            ////series_Center_Cs.ChartType = SeriesChartType.Column;

            ////chart_Center.Series.Add(series_Center_Cs);
            //////---------------------------------------------------



            //////---------------------------------------------------                            
            ////series_Center_Cnt_1.Points.Clear();

            ////series_Center_Cnt_1["DrawingStyle"] = "Emboss";
            ////series_Center_Cnt_1["PointWidth"] = "0.5";
            ////series_Center_Cnt_1.Name = cm._chang_base_caption_search("판매원");
            ////series_Center_Cnt_1.ChartType = SeriesChartType.Column;

            ////chart_Center.Series.Add(series_Center_Cnt_1);
            //////---------------------------------------------------


            //////---------------------------------------------------                            
            ////series_Center_Cnt_2.Points.Clear();

            ////series_Center_Cnt_2["DrawingStyle"] = "Emboss";
            ////series_Center_Cnt_2["PointWidth"] = "0.5";
            ////series_Center_Cnt_2.Name = cm._chang_base_caption_search("소비자");
            ////series_Center_Cnt_2.ChartType = SeriesChartType.Column;

            ////chart_Center.Series.Add(series_Center_Cnt_2);
            //////---------------------------------------------------

            //---------------------------------------------------
            chart_Center.ChartAreas[0].AxisX.Interval = 1;
            chart_Center.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_Center.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 7;
            //chart_Center.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //---------------------------------------------------
           

        }

       















    }
}
