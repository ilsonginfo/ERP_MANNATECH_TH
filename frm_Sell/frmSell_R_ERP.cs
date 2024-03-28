using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SqlClient;
using DXVGrid = DevExpress.XtraGrid.Views.Grid;
using DViewInfo = DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DXEditor = DevExpress.XtraEditors;
using DXGrid = DevExpress.XtraGrid;

namespace MLM_Program
{
    public partial class frmSell_R_ERP : Form
    {

        string mbid2;
        string itemcode;

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
        private string T_Search_Nubmer = "";


       // Class.DevGridControlService cgb = new Class.DevGridControlService();

        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Item = new cls_Grid_Base();
        cls_Grid_Base cgb_main = new cls_Grid_Base();
        
        private Dictionary<string, cls_Sell> SalesDetail = new Dictionary<string, cls_Sell>();

        DataSet dsExcels = new DataSet();

        private const string base_db_name = "tbl_SalesDetail";
        private int Data_Set_Form_TF;
        private int Form_Load_TF = 0;

        public delegate void SendNumberDele(string Send_Number, string Send_Name, string Send_OrderNumber);
        public event SendNumberDele Send_Sell_Number;

        public delegate void Send_Mem_NumberDele(string Send_Number, string Send_Name);
        public event Send_Mem_NumberDele Send_Mem_Number;

        private Dictionary<int, cls_Sell_Item> SalesItemDetail = new Dictionary<int, cls_Sell_Item>();
        private Dictionary<int, cls_Sell_Rece> Sales_Rece = new Dictionary<int, cls_Sell_Rece>();

        private Series series_Item = new Series();

        private int idx_Mbid2 = 0;
        public frmSell_R_ERP()
        {
            InitializeComponent();
        }




        //////SalesDetail___SalesDetail__SalesDetail__SalesDetail__SalesDetail__SalesDetail
        //////SalesDetail___SalesDetail__SalesDetail__SalesDetail__SalesDetail__SalesDetail
        private void Base_Grid_Set_2()
        {
            dGridView_Base_Header_Reset_2(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_main.d_Grid_view_Header_Reset();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            Dictionary<string, double> SelType_1 = new Dictionary<string, double>();

            int fi_cnt = 0;
            double S_cnt4 = 0; double S_cnt5 = 0; double S_cnt6 = 0;
            double Sum_13 = 0; double Sum_14 = 0; double Sum_15 = 0; ; double Sum_16 = 0; ; double Sum_17 = 0, S_cnt6_1 = 0;
            double Sell_Cnt_1 = 0; double Sell_Cnt_2 = 0;
            foreach (string t_key in SalesDetail.Keys)
            {
                if (SalesDetail[t_key].Del_TF != "D")
                {
                    Set_gr_dic(ref gr_dic_text, t_key, fi_cnt);  //데이타를 배열에 넣는다.

                    if (SalesDetail[t_key].SellTFName == "승인")  //합계에 승인된 것만 보여달라고 요청(명충남 대표) 2016-08-17작업
                    {
                        S_cnt4 = S_cnt4 + SalesDetail[t_key].TotalPrice;
                        S_cnt5 = S_cnt5 + SalesDetail[t_key].TotalInputPrice;
                        S_cnt6 = S_cnt6 + SalesDetail[t_key].TotalPV;
                        S_cnt6_1 = S_cnt6_1 + SalesDetail[t_key].TotalCV;

                        Sum_13 = Sum_13 + SalesDetail[t_key].InputCash;
                        Sum_14 = Sum_14 + SalesDetail[t_key].InputCard;
                        Sum_15 = Sum_15 + SalesDetail[t_key].InputPassbook + SalesDetail[t_key].InputPassbook_2;
                        Sum_17 = Sum_17 + SalesDetail[t_key].InputMile;
                        Sum_16 = Sum_16 + SalesDetail[t_key].UnaccMoney;
                    }

                    string T_ver = SalesDetail[t_key].SellCodeName;
                    if (SelType_1.ContainsKey(T_ver) == true)
                    {
                        SelType_1[T_ver] = SelType_1[T_ver] + SalesDetail[t_key].TotalPrice;  //금액                    
                    }
                    else
                    {
                        SelType_1[T_ver] = SalesDetail[t_key].TotalPrice;
                    }

                    T_ver = SalesDetail[t_key].RecordID;
                    if (T_ver.Contains("WEB") != true)
                    {
                        Sell_Cnt_1 = Sell_Cnt_1 + SalesDetail[t_key].TotalPrice;
                    }
                    else
                    {
                        Sell_Cnt_2 = Sell_Cnt_2 + SalesDetail[t_key].TotalPrice;
                    }
                }

                fi_cnt++;
            }

            //Reset_Chart_Total(Sum_13, Sum_14, Sum_15,Sum_17);
            //Reset_Chart_Total(ref SelType_1);
            //Reset_Chart_Total(Sell_Cnt_1, Sell_Cnt_2);

            cls_form_Meth cm = new cls_form_Meth();

            object[] row0 = { ""
                                ,"<< " + cm._chang_base_caption_search("합계") + " >>"
                                ,""
                                ,""
                                ,S_cnt4

                                ,S_cnt5
                                ,S_cnt6
                                ,S_cnt6_1
                                ,""
                                ,""

                                ,Sum_13
                                ,Sum_14
                                ,Sum_15
                                ,Sum_16
                                ,""

                                ,""
                                ,""
                            };

            gr_dic_text[fi_cnt + 2] = row0;

            cgb_main.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_main.db_grid_Obj_Data_Put();

            //
            int FFCnt = 0;
            foreach (string t_key in SalesDetail.Keys)
            {
                if (SalesDetail[t_key].Del_TF != "D")
                {
                    if (SalesDetail[t_key].INS_Num.Contains("재발급요청요망") == true)
                    {
                        cgb_main.basegrid.Rows[FFCnt].DefaultCellStyle.BackColor = System.Drawing.Color.PaleGoldenrod;

                    }

                    FFCnt++;
                }

            }
        }


        private void Set_gr_dic(ref Dictionary<int, object[]> gr_dic_text, string t_key, int fi_cnt)
        {
            object[] row0 = { SalesDetail[t_key].SellTFName
                                ,SalesDetail[t_key].INS_Num
                                ,SalesDetail[t_key].OrderNumber
                                ,SalesDetail[t_key].SellDate

                                ,SalesDetail[t_key].TotalPrice
                                ,SalesDetail[t_key].TotalInputPrice
                                ,SalesDetail[t_key].TotalPV
                                ,SalesDetail[t_key].TotalCV
                                ,SalesDetail[t_key].SellCodeName
                                ,SalesDetail[t_key].ReturnTFName

                                ,SalesDetail[t_key].InputCash
                                ,SalesDetail[t_key].InputCard
                                ,SalesDetail[t_key].InputPassbook
                                ,SalesDetail[t_key].InputPassbook_2
                                ,SalesDetail[t_key].InputCoupon
                                ,SalesDetail[t_key].UnaccMoney
                                ,SalesDetail[t_key].InputMile
                                ,SalesDetail[t_key].InputNaver

                                ,SalesDetail[t_key].RecordID
                                ,SalesDetail[t_key].RecordTime
                                ,""
                                ,SalesDetail[t_key].Associated_Card
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }

        private void dGridView_Base_Header_Reset_2()
        {
            cgb_main.Grid_Base_Arr_Clear();
            cgb_main.basegrid = dGridView_Base_2;
            cgb_main.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_main.grid_col_Count = 22;
            cgb_main.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"승인여부" , "공제번호"  , "주문번호"    , "주문일자", "총주문액"
                                      , "총입금액"      , "총PV"  ,"총CV"   , "주문종류"   , "구분"
                                    , "현금"  ,"카드" ,"무통장입금", "가상계좌", "쿠폰",
                                  "미결제" ,"마일리지" ,"네이버페이",  "기록자" ,  "기록일" ,"",""
                                };

            if (cls_app_static_var.Sell_Union_Flag == "")  //직판특판이 아닌경우 공제번호 필드 안나오게
            {
                int[] g_Width = { 80, 0 , 120, 0, 0
                                  , 80  ,80 , 80 , 80, 80
                                  , 80, 80  ,80 ,80,80,80
                                  ,cls_app_static_var.Using_Mileage_TF,80 ,80 ,80 , 0, 0
                                };
                cgb_main.grid_col_w = g_Width;
            }
            else
            {

                int[] g_Width = { 80,120, 120, 80,80
                                  , 80  ,80 , 80 , 80 , 80
                                  , 80, 80  ,80 ,80,80,80
                                  ,cls_app_static_var.Using_Mileage_TF,80 ,80 ,80 , 0 , 0
                                };
                cgb_main.grid_col_w = g_Width;
            }

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleRight  //5   

                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter//10

                               
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight //15
                                  
                                ,DataGridViewContentAlignment.MiddleRight
                                                        ,DataGridViewContentAlignment.MiddleRight
                                 ,DataGridViewContentAlignment.MiddleCenter
                                  ,DataGridViewContentAlignment.MiddleLeft
                                  ,DataGridViewContentAlignment.MiddleCenter
                                   ,DataGridViewContentAlignment.MiddleCenter
                                          ,DataGridViewContentAlignment.MiddleCenter
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[16 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[17 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb_main.grid_col_header_text = g_HeaderText;
            cgb_main.grid_cell_format = gr_dic_cell_format;

            cgb_main.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,   true,  true
                                    , true ,true,  true,  true,  true,  true,  true
                                   };
            cgb_main.grid_col_Lock = g_ReadOnly;

            cgb_main.basegrid.RowHeadersVisible = false;
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

            //mtxtSellDate1.Text = DateTime.Now.ToString("yyyy-MM-dd");


            Reset_Chart_Total();
            Menu_Text_Chang_KR();

          

            if (cls_app_static_var.Using_Mileage_TF == 0)
                tableLayoutPanel17.Visible = false;
            else
                tableLayoutPanel17.Visible = true;

            mtxtSellDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;
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

            //radioB_return_way.Checked = true; 

            mtxtMbid.Focus();

            //string[] data_P = { ""
            //                   , "CJ대한통운택배", "우체국택배"
            //                    , "로젠택배", "롯데택배(구 현대택배)", "한진택배"
            //                  };
            //Combo_Rece_Company.Items.AddRange(data_P);
            //Combo_Rece_Company.SelectedIndex = 0;

            InitCourierCompany();

            //string[] data_P_2 = { ""
            //                   , "단순변심", "주문 수량 과다"
            //                    , "배송불만", "기타"
            //                  };
            //txt_return_Etc1.Items.AddRange(data_P_2);
            //txt_return_Etc1.SelectedIndex = 0;

            InitReturnType();

            InitComboZipCode_TH();

            // 태국 인 경우
            if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
            {
                pnlDistrict_TH.Visible = true;
                pnlProvince_TH.Visible = true;
                pnlSubDistrict_TH.Visible = true;
                pnlZipCode_TH.Visible = true;
                pnlZipCode_KR.Visible = false;
                txtAddress2.ReadOnly = true;
                cbSubDistrict_TH_SelectedIndexChanged(this, null);
            }
            // 한국 인 경우
            else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
            {
                pnlDistrict_TH.Visible = false;
                pnlProvince_TH.Visible = false;
                pnlSubDistrict_TH.Visible = false;
                pnlZipCode_TH.Visible = false;
                pnlZipCode_KR.Visible = true;
                txtAddress2.ReadOnly = false;
                txtAddress2.Clear();
            }
        }

        private void InitComboZipCode_TH()
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("SELECT ZIPCODE_NM FROM dbo.ufn_Get_ZipCode_State_TH() ORDER BY ZIPCODE_SORT ");
            sb.AppendLine("SELECT * FROM ufn_Get_ZipCode_Province_TH() ORDER BY MinSubDistrictID ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            cbProvince_TH.DataBindings.Clear();
            cbProvince_TH.DataSource = ds.Tables["ZipCode_NM"];
            cbProvince_TH.DisplayMember = "ZipCode_NM";
            cbProvince_TH.ValueMember = "ProvinceCode";

            //cbZipCode_TH.SelectedIndex = -1;
            txtZipCode_TH.Text = "";
            txtAddress2.Text = "";
            cbDistrict_TH.SelectedIndex = -1;
            cbProvince_TH.SelectedIndex = -1;
        }

        private void InitCourierCompany()
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            string sColumnName = "";

            if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
            {
                sColumnName = "COMPANY_NAME";
                sb.AppendLine("SELECT " + sColumnName + " FROM TBL_COURIER_COMPANY WITH(NOLOCK) WHERE Na_Code = '" + cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) + "' ");
            }
            else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
            {
                sColumnName = "COMPANY_NAME_EN";
                sb.AppendLine("SELECT " + sColumnName + " FROM TBL_COURIER_COMPANY WITH(NOLOCK) WHERE Na_Code = '" + cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) + "' ");
            }


            if (Temp_conn.Open_Data_Set(sb.ToString(), sColumnName, ds) == false) return;

            if (Temp_conn.DataSet_ReCount <= 0) return;

            Combo_Rece_Company.DataBindings.Clear();
            Combo_Rece_Company.DataSource = ds.Tables[sColumnName];
            Combo_Rece_Company.DisplayMember = sColumnName;
            Combo_Rece_Company.SelectedIndex = -1;
        }

        private void InitReturnType()
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            string sColumnName = "";

            if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
            {
                sColumnName = "RETURN_REASON";
                sb.AppendLine("SELECT " + sColumnName + " FROM TBL_RETURN_TYPE WITH(NOLOCK) ");
            }
            else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
            {
                sColumnName = "RETURN_REASON_EN";
                sb.AppendLine("SELECT " + sColumnName + " FROM TBL_RETURN_TYPE WITH(NOLOCK) ");
            }


            if (Temp_conn.Open_Data_Set(sb.ToString(), sColumnName, ds) == false) return;

            if (Temp_conn.DataSet_ReCount <= 0) return;
            
            txt_return_Etc1.DataBindings.Clear();
            txt_return_Etc1.DataSource = ds.Tables[sColumnName];
            txt_return_Etc1.DisplayMember = sColumnName;
            txt_return_Etc1.SelectedIndex = -1;
        }

        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Select.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_Excel.Left = butt_Select.Left + butt_Select.Width + 2;
            butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Save2.Left = butt_Delete.Left + butt_Delete.Width + 2;

            butt_Exit.Left = this.Width - butt_Exit.Width - 17;

            
            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Select);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);
            cfm.button_flat_change(butt_Save2);
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

                //dGridView_Base_Header_Reset_item(); //디비그리드 헤더와 기본 셋팅을 한다.
                //cgb_item.d_Grid_view_Header_Reset();

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



        private Boolean Input_Error_Check(MaskedTextBox m_tb, string s_Kind, int Check_Leave_TF = 0)
        {
            string T_Mbid = m_tb.Text;
            string Mbid = ""; int Mbid2 = 0;


            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == -1) //올바르게 회원번호 양식에 맞춰서 입력햇는가.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }

            if (Mbid2 == 0) //올바르게 회원번호 양식에 맞춰서 입력햇는가.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }

            string Tsql = "";
            Tsql = "Select Mbid , Mbid2, M_Name , Sell_Mem_TF  ";
            Tsql = Tsql + " , LineCnt , N_LineCnt  ";
            Tsql = Tsql + " , LeaveDate , LineUserDate  ";
            Tsql = Tsql + " , Saveid  , Saveid2  ";
            Tsql = Tsql + " , Nominid , Nominid2  ";
            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            //Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            //Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)  //실제로 존재하는 회원 번호 인가.
            {

                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }
            else
            {
                if (Check_Leave_TF == 1)
                {
                    //if (txt_OrderNumber.Text == "") //신규 저장건에 한해서.
                    //{
                    //    //주문할려고 하는 회원이 탈퇴 회원이다
                    //    if (ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString() != "")
                    //    {

                    //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Leave_Sell")
                    //        + "\n" +
                    //        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    //        m_tb.Focus(); return false;
                    //    }
                    //}
                }
            }
            //++++++++++++++++++++++++++++++++            

            return true;
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
        void e_f_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            mtxtMbid.Text = Send_Number; txtName.Text = Send_Name;
        
        }
        void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtName.Text.Trim();
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

            Tsql = "SELECT  '', tls_Sales_Return.[Ordernumber] --주문번호                                                      txtOrderNumber" + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[mbid2] --MIBD2                                                               mtxtMbid" + Environment.NewLine;
            Tsql = Tsql + "     ,tbl_memberinfo.[m_name] --이름                                                                          " + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[return_Etc1] --반품사유                                                     txt_return_Etc1" + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[return_Etc2] --기타선택시 반품사유                                          txt_return_Etc2" + Environment.NewLine;
            Tsql = Tsql + "     ,case when tls_Sales_Return.[return_way] = 1 then '직접배송' else '택배기사방문' end --회수 방법 1: 직접배송 2:택배기사 방문  선택컨트롤             radioB_return_way" + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[getTel1] --휴대폰번호                                                       txt_getTel1" + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[getTel2] --전화번호                                                         txt_getTel2" + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[PassCompnay] --직접 배송시 택배사                    텍스트선택             radioB_return_way" + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[PsssNumber] --운송장번호                                                    txt_PsssNumber" + Environment.NewLine;
            Tsql = Tsql + "     ,case when  tls_Sales_Return.[PassSelect]  = 0 then '전체반품' else '부분반품' end --0 : 전체 반품 1: 부분 반품                                    radioB_PassSelect" + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[PassName] --받는사람이름                                                    txt_PassName" + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[Get_ZipCode] --회수지 우편번호                       우편번호               mtxtZip1       " + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[Get_Address1] --회수지 주소                                                 txtAddress1" + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[Get_Address2] --회수지 상세주소                                             txtAddress2" + Environment.NewLine;
            Tsql = Tsql + "     ,case when tls_Sales_Return.[Returnstatus] = 0 then '미처리' else '처리' end  --처리 상태                                                   radioB_Returnstatus    " + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[RecordTime] --신청 시간                                                     mtxtMakDate1 mtxtMakDate2" + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[StatusDate] --처리시간                                                      mtxtSellDate1 mtxtSellDate2" + Environment.NewLine;
            Tsql = Tsql + "     ,tls_Sales_Return.[StatusPerson] --처리담당자                                                  txt_StatusPerson" + Environment.NewLine;
            Tsql = Tsql + " FROM [dbo].[tls_Sales_Return]       join tbl_memberinfo   on          tls_Sales_Return.mbid2 =    tbl_memberinfo.mbid2                                               " + Environment.NewLine;
        }



        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = " Where tls_Sales_Return.Mbid2  <>  '' ";
            
                        string Mbid = ""; int Mbid2 = 0;
           

            //회원번호2로 검색
            if (
                (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
                )
            {
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And tls_Sales_Return.Mbid ='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tls_Sales_Return.Mbid2 = " + Mbid2;
                }

                //if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
                //{
                //    if (Mbid != "")
                //        strSql = strSql + " And tls_Sales_Return.Mbid ='" + Mbid + "'";

                //    if (Mbid2 >= 0)
                //        strSql = strSql + " And tls_Sales_Return.Mbid2 = " + Mbid2;
                //}
            }


            //회원명으로 검색
            if (txtName.Text.Trim() != "")
                strSql = strSql + " And tbl_memberinfo.M_Name Like '%" + txtName.Text.Trim() + "%'";


            //구매일자 검색 -1
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tls_Sales_Return.StatusDate = '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";

            //구매일자로 검색 -2
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tls_Sales_Return.StatusDate >= '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tls_Sales_Return.StatusDate <= '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";
            }



            //기록일자로 검색 -1
            if ((mtxtMakDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And Replace(Left(tls_Sales_Return.recordtime ,10),'-','') = '" + mtxtMakDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtMakDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And Replace(Left(tls_Sales_Return.recordtime ,10),'-','') >= '" + mtxtMakDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And Replace(Left(tls_Sales_Return.recordtime ,10),'-','') <= '" + mtxtMakDate2.Text.Replace("-", "").Trim() + "'";
            }

            

            //if (txt_us.Text.Trim() != "")
            //    strSql = strSql + " And tbl_SalesDetail.Us_Ord = '" + txt_us.Text.Trim() + "'";

            //if (txt_Us_num.Text.Trim() != "")
            //    strSql = strSql + " And tbl_Memberinfo.Us_Num = '" + txt_Us_num.Text.Trim() + "'";
            

            
            

            //if (txtR_Id_Code.Text.Trim() != "")
            //    strSql = strSql + " And tbl_SalesDetail.recordid = '" + txtR_Id_Code.Text.Trim() + "'";

            
            if (txtOrderNumber.Text.Trim() != "")
                strSql = strSql + " And tls_Sales_Return.OrderNumber like  '%" + txtOrderNumber.Text.Trim() + "%'";



            if (radioB_return_way1.Checked == true)
                strSql = strSql + " And return_way = 1 ";

            if (radioB_return_way2.Checked == true)
                strSql = strSql + " And return_way = 2 ";

            if (radioB_Returnstatus1.Checked == true)
                strSql = strSql + " And Returnstatus = 1 ";

            if (radioB_Returnstatus2.Checked == true)
                strSql = strSql + " And Returnstatus = 2 ";

            if (radioB_PassSelect0.Checked == true)
                strSql = strSql + " And PassSelect = 0 ";

            if (radioB_PassSelect1.Checked == true)
                strSql = strSql + " And PassSelect = 1 ";
            if (Combo_Rece_Company.Text.Trim() != "")
                strSql = strSql + " And PassCompnay = '" + Combo_Rece_Company.Text.Trim() + "'";
  
            if (txt_PassName.Text.Trim() != "")
                strSql = strSql + " And PassName like '%" + txt_PassName.Text.Trim() + "%'";
            if (txt_getTel1.Text.Trim() != "")
                strSql = strSql + " And getTel1 like '%" + txt_getTel1.Text.Trim() + "%'";
            if (txt_getTel2.Text.Trim() != "")
                strSql = strSql + " And getTel2 like '%" + txt_getTel2.Text.Trim() + "%'";
            if (txt_PsssNumber.Text.Trim() != "")
                strSql = strSql + " And PsssNumber like '%" + txt_PsssNumber.Text.Trim() + "%'";
            if (mtxtZip1.Text.Trim() != "")
                strSql = strSql + " And Get_ZipCode like '%" + mtxtZip1.Text.Trim() + "%'";
            if (txtAddress1.Text.Trim() != "")
                strSql = strSql + " And Get_Address1 like '%" + txtAddress1.Text.Trim() + "%'";
            if (txtAddress2.Text.Trim() != "")
                strSql = strSql + " And Get_Address2 like '%" + txtAddress2.Text.Trim() + "%'";
            if (txt_return_Etc1.Text.Trim() != "")
                strSql = strSql + " And return_Etc1 like '%" + txt_return_Etc1.Text.Trim() + "%'";
            if (txt_return_Etc2.Text.Trim() != "")
                strSql = strSql + " And return_Etc2 like '%" + txt_return_Etc2.Text.Trim() + "%'";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(strSql);
            Tsql += sb.ToString();
        }


        private void Base_Grid_Set()
        {   
            string Tsql = "";            
            string Tsql_cash = "";
            Make_Base_Query(ref Tsql);

            Make_Base_Query_(ref Tsql);

            //Make_Base_Query_Cash(ref Tsql_cash);

            //Make_Base_Query_Cash_(ref Tsql_cash);
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();                                  
            
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name , this.Text ) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
          // if (Temp_Connect.Open_Data_Set(Tsql_cash, base_db_name, dsCash, this.Name, this.Text) == false) return;
            if (ReCnt == 0) return;
        
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            Dictionary<string, double> Center_MemCnt = new Dictionary<string, double>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.                
            }
            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }



        private void dGridView_Base_Header_Reset()
        {

            cgb.grid_col_Count = 20;
            cgb.basegrid = dGridView_Base;            
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            //cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {

                                 "선택",
                                 "주문번호"   , "회원아이디"    ,"회원이름"     , "반품사유"  , "기타선택시 반품사유"  
                                 , "회수 방법" , "휴대폰번호"  , "전화번호"   , "직접배송시택배사"   , "운송장번호"   
                                 , "반품여부"  , "받는사람이름"       , "회수지우편번호"             , "회수지주소"      , "회수지상세주소"   
                                 , "처리상태", "신청시간"   , "처리시간"         , "처리담당자"
                                    };


            string[] g_Cols = {
                                   "선택",
                                "OrderNumber"   , "mbid2"     , "m_name", "return_Etc1"  , "return_Etc2"  , "return_way"
                                , "getTel1"   , "getTel2"  , "PassCompnay"    , "PsssNumber"     , "PassSelect"
                                , "PassName"      , "Get_ZipCode"           , "Get_Address1"       , "Get_Address2"     , "Returnstatus"
                                , "RecordTime"       , "StatusDate"        , "StatusPerson"  
                                    };

            cgb.grid_col_header_text = g_HeaderText;
            cgb.grid_col_name = g_Cols;
            cgb.grid_col_Count = g_HeaderText.Length;

            if (cls_app_static_var.Sell_Union_Flag == "")
            {
                int[] g_Width = {
                  80,
                  130, 90,  90, 200, 200,
                  90,   150, 110,  90,90 ,
                  90,    150,  150,  250, 200,
                  90, 150, 150, 90
                                };
                cgb.grid_col_w = g_Width;
            }
            else
            {

                int[] g_Width = {
                    80,
                  130, 90,  90, 200, 200,
                  90,   150, 110,  90,90 ,
                  90,    150,  150,  250, 200,
                  90, 150, 150, 90
                                };
                cgb.grid_col_w = g_Width;
            }

            Boolean[] g_ReadOnly = { true , true,  true, true, true 
                                    ,true , true,  true, true ,true                                     
                                    ,true , true,  true, true ,true  
                                    ,true , true,  true , true,  true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter  

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
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight

                              };
            cgb.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            //gr_dic_cell_format[17] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[18] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[19] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[20] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[21] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[22] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[23] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[24] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[25] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[26] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[27] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[28] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[29] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[30] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[31] = cls_app_static_var.str_Grid_Currency_Type;
            //gr_dic_cell_format[32] = cls_app_static_var.str_Grid_Currency_Type;

            cgb.grid_cell_format = gr_dic_cell_format;
            
        }


        //private void dGridView_Base_Header_Reset_item()
        //{

        //    cgb_item.grid_col_Count = 4;
        //    cgb_item.basegrid = dGridView_Sell_Item;
        //    cgb_item.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
        //    cgb_item.grid_Frozen_End_Count = 2;
        //    //cgb_item.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        //    string[] g_HeaderText = {

        //                            "주문번호"   , "아이템코드"    ,"아이템이름"     , "개수" 
        //                            };


        //    string[] g_Cols = {
        //                            "OrderNumber"   , "itemcode"     , "itemname", "itemcount" 
        //                            };

        //    cgb_item.grid_col_header_text = g_HeaderText;
        //    cgb_item.grid_col_name = g_Cols;
        //    cgb_item.grid_col_Count = g_HeaderText.Length;

        //    if (cls_app_static_var.Sell_Union_Flag == "")
        //    {
        //        int[] g_Width = {
        //          130,   130, 200,  130
        //                        };
        //        cgb_item.grid_col_w = g_Width;
        //    }
        //    else
        //    {

        //        int[] g_Width = {
        //           130,   130, 200,  130

        //                        };
        //        cgb_item.grid_col_w = g_Width;
        //    }

        //    Boolean[] g_ReadOnly = { true , true,  true, true
        //                           };
        //    cgb_item.grid_col_Lock = g_ReadOnly;

        //    DataGridViewContentAlignment[] g_Alignment =
        //                      {DataGridViewContentAlignment.MiddleCenter
        //                       ,DataGridViewContentAlignment.MiddleLeft
        //                       ,DataGridViewContentAlignment.MiddleLeft
        //                       ,DataGridViewContentAlignment.MiddleLeft

        //                      };
        //    cgb_item.grid_col_alignment = g_Alignment;


        //    Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
        

        //    cgb_item.grid_cell_format = gr_dic_cell_format;

        //}
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
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }
        //private void Set_gr_dic2(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        //{
        //    object[] row0 = {    ds.Tables[base_db_name].Rows[fi_cnt][0]
        //                        ,ds.Tables[base_db_name].Rows[fi_cnt][1]
        //                        ,ds.Tables[base_db_name].Rows[fi_cnt][2]
        //                        ,ds.Tables[base_db_name].Rows[fi_cnt][3]
        //                         };

        //    gr_dic_text[fi_cnt + 1] = row0;
        //}




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
                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    int reCnt = 0;
                    cls_Search_DB cds = new cls_Search_DB();
                    string Search_Name = "";
                    reCnt = cds.Member_Name_Search(mtb.Text, ref Search_Name);

                    if (reCnt == 1)
                    {
                        if (mtb.Name == "mtxtMbid")
                        {
                            txtName.Text = Search_Name;
                        }
                    }

                    else if (reCnt > 1)  //회원번호 비슷한 사람들이 많은 경우
                    {
                        string Mbid = "";
                        int Mbid2 = 0;
                        cds.Member_Nmumber_Split(mtb.Text, ref Mbid, ref Mbid2);

                        frmBase_Member_Search e_f = new frmBase_Member_Search();

                        if (mtb.Name == "mtxtMbid")
                        {
                            e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                            e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info);
                        }

                        e_f.ShowDialog();

                        SendKeys.Send("{TAB}");
                    }
                }
                else
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

        void e_f_Send_MemNumber_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = "";
            cls_Search_DB csb = new cls_Search_DB();
            csb.Member_Nmumber_Split(mtxtMbid.Text.Trim(), ref searchMbid, ref searchMbid2);
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
            if (tb.Name == "txtName" && tb.Text.Trim() != "")
            {
                if (e.KeyChar == 13)
                {
                    int reCnt = 0;
                    cls_Search_DB cds = new cls_Search_DB();
                    string Search_Mbid = "";
                    reCnt = cds.Member_Name_Search_S_N(ref Search_Mbid, tb.Text);

                    if (reCnt == 1)
                    {
                        mtxtMbid.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    

                    }
                    else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
                    {
                        frmBase_Member_Search e_f = new frmBase_Member_Search();

                        e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                        e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);

                        e_f.ShowDialog();

                        SendKeys.Send("{TAB}");
                    }
                }
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
            

            //if (tb.Name == "txtR_Id")
            //{
            //    Data_Set_Form_TF = 1; 
            //    if (tb.Text.Trim() == "")
            //        txtR_Id_Code.Text = "";
            //    Data_Set_Form_TF = 0; 
            //}

          
        }

        

        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {            
            

            //if (tb.Name == "txtR_Id")
            //{
            //    Data_Set_Form_TF = 1;
            //    Db_Grid_Popup(tb, txtR_Id_Code);
            //    //if (tb.Text.ToString() == "")
            //    //    Db_Grid_Popup(tb, txtR_Id_Code, "");
            //    //else
            //    //    Ncod_Text_Set_Data(tb, txtR_Id_Code);

            //    //SendKeys.Send("{TAB}");
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




        private void Set_SalesDetail()
        {
            cls_form_Meth cm = new cls_form_Meth();
            string strSql = "";

            strSql = "Select tbl_SalesDetail.* ";
            strSql = strSql + " , tbl_Business.Name BusCodeName ";
            strSql = strSql + " , tbl_SellType.SellTypeName SellCodeName  ";

            strSql = strSql + " , Ga_Order  SellTF ";
            strSql = strSql + " ,Case When Ga_Order >= 1 Then '" + cm._chang_base_caption_search("미승인") + "'";
            strSql = strSql + "  When Ga_Order = 0 Then '" + cm._chang_base_caption_search("승인") + "'";
            strSql = strSql + " ELSE '' ";
            strSql = strSql + " END SellTFName ";

            strSql = strSql + " ,Case When ReturnTF = 1 Then '" + cm._chang_base_caption_search("정상") + "'";
            strSql = strSql + "  When ReturnTF = 2 Then '" + cm._chang_base_caption_search("반품") + "'";
            strSql = strSql + "  When ReturnTF = 4 Then '" + cm._chang_base_caption_search("교환") + "'";
            strSql = strSql + "  When ReturnTF = 3 Then '" + cm._chang_base_caption_search("부분반품") + "'";
            strSql = strSql + "  When ReturnTF = 5 Then '" + cm._chang_base_caption_search("취소") + "'";
            strSql = strSql + " END ReturnTFName ";

            if (cls_app_static_var.Sell_Union_Flag == "U")  //특판
            {
                strSql = strSql + " , Case When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE = '0000' Then ISNULL(T_REALMLM.GUARANTE_NUM,'') ";
                strSql = strSql + "        When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE <> '0000' Then  ISNULL(T_REALMLM_ErrCode.Er_Msg ,'' ) ";
                strSql = strSql + "        When  tbl_SalesDetail.union_Seq = 0 Then '미신고'  ";
                strSql = strSql + "   End InsuranceNumber2 ";
            }
            else if (cls_app_static_var.Sell_Union_Flag == "D")  //직판
            {
                //strSql = strSql + ", Case When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NULL And tbl_SalesDetail.InsuranceNumber <> '' Then tbl_SalesDetail.InsuranceNumber ";
                //strSql = strSql + " When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                //strSql = strSql + " When  ReturnTF = 5 And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                //strSql = strSql + " When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel ='' Then tbl_SalesDetail.InsuranceNumber + '(취소요청중)' ";
                //strSql = strSql + " When  ReturnTF = 1 And tbl_SalesDetail.InsuranceNumber = '' Then '미승인요청' ";
                //strSql = strSql + " ELSE tbl_SalesDetail.InsuranceNumber END  InsuranceNumber2 ";


                //strSql = strSql + ", Case When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail  (nolock)  AS A1 Where A1.Re_BaseOrderNumber <> '' And tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NULL And tbl_SalesDetail.InsuranceNumber <> '' Then tbl_SalesDetail.InsuranceNumber ";
                //strSql = strSql + " When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail  (nolock)  AS A1 Where A1.Re_BaseOrderNumber <> '' And tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                //strSql = strSql + " When  ReturnTF = 5 And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                //strSql = strSql + " When  ReturnTF = 1 And (Select A1.SellDate From tbl_SalesDetail (nolock) AS A1 Where A1.Re_BaseOrderNumber <> '' And  tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber ) IS NOT NULL And InsuranceNumber_Cancel ='' Then tbl_SalesDetail.InsuranceNumber + '(취소요청중)' ";
                //strSql = strSql + " When  ReturnTF = 2 then '반품처리' ";
                //strSql = strSql + " When  ReturnTF = 1 And tbl_SalesDetail.InsuranceNumber = '' Then '재발급요청요망' + ' ' + tbl_SalesDetail.INS_Num_Err  ";
                //strSql = strSql + " ELSE tbl_SalesDetail.InsuranceNumber END  InsuranceNumber2 ";


                strSql += ", Case When  ReturnTF = 1 And (Select top 1  A1.SellDate From tbl_SalesDetail  (nolock)  AS A1 Where A1.Re_BaseOrderNumber <> '' And tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber Order by OrderNumber ) IS NULL And tbl_SalesDetail.InsuranceNumber <> '' Then tbl_SalesDetail.InsuranceNumber ";
                strSql += " When  ReturnTF = 1 And (Select top 1  A1.SellDate From tbl_SalesDetail  (nolock)  AS A1 Where A1.Re_BaseOrderNumber <> '' And tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber Order by OrderNumber ) IS NOT NULL And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                strSql += " When  ReturnTF = 5 And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                strSql += " When  ReturnTF = 1 And (Select top 1  A1.SellDate From tbl_SalesDetail (nolock) AS A1 Where A1.Re_BaseOrderNumber <> '' And  tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber And ReturnTF = 2 Order by OrderNumber  ) IS NOT NULL And InsuranceNumber_Cancel ='' Then tbl_SalesDetail.InsuranceNumber + '(취소요청중)' ";
                strSql += " When  ReturnTF = 1 And (Select top 1  A1.SellDate From tbl_SalesDetail (nolock) AS A1 Where A1.Re_BaseOrderNumber <> '' And  tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber And ReturnTF = 3 Order by OrderNumber  ) IS NOT NULL And InsuranceNumber_Cancel ='' Then tbl_SalesDetail.InsuranceNumber + '(부분취소요청중)' ";
                strSql += " When  ReturnTF = 2 then '반품처리' ";
                strSql += " When  ReturnTF = 3 then '부분반품처리' ";
                strSql += " When  ReturnTF = 1 And tbl_SalesDetail.InsuranceNumber = '' Then '재발급요청요망' + ' ' + tbl_SalesDetail.INS_Num_Err  ";
                strSql += " ELSE tbl_SalesDetail.InsuranceNumber END InsuranceNumber2 ";
            }
            else
            {
                strSql = strSql + " , InsuranceNumber AS InsuranceNumber2 ";
            }

            strSql = strSql + " ,tbl_SalesDetail.InsuranceNumber  AS Real_InsuranceNumber  ";
            strSql = strSql + " ,tbl_sales_cacu.Associated_Card as Associated_Card";
            strSql = strSql + " ,tbl_Memberinfo.Sell_Mem_TF as Sell_Mem_TF";
            strSql = strSql + " From tbl_SalesDetail (nolock) ";
            strSql = strSql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";
            strSql = strSql + " LEFT Join tbl_SellType ON tbl_SalesDetail.SellCode = tbl_SellType.SellCode ";
            strSql = strSql + " LEFT JOIN tbl_Business (nolock) ON tbl_SalesDetail.BusCode = tbl_Business.NCode And tbl_SalesDetail.Na_code = tbl_Business.Na_code ";
            strSql = strSql + " LEFT JOIN T_REALMLM (nolock) ON T_REALMLM.SEQ = tbl_SalesDetail.union_Seq ";
            strSql = strSql + " LEFT JOIN T_REALMLM_ErrCode (nolock) ON T_REALMLM.ERRCODE = T_REALMLM_ErrCode.Er_Code ";
            strSql = strSql + " LEFT JOIN tbl_sales_cacu (nolock) ON tbl_SalesDetail.ordernumber = tbl_sales_cacu.ordernumber ";
            strSql = strSql + " Where tbl_Memberinfo.Mbid2 = " + idx_Mbid2.ToString();
          
    
            //// strSql = strSql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";

            //20200929구현호 이거 지워야함
            // strSql = strSql + " And tbl_SalesDetail.BusCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";
            strSql = strSql + " Order By  CAST(tbl_SalesDetail.RecordTime AS DATETIME) DESC";//SellDate DESC , OrderNumber DESC ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            Dictionary<string, cls_Sell> T_SalesDetail = new Dictionary<string, cls_Sell>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cls_Sell t_c_sell = new cls_Sell();

                t_c_sell.OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                t_c_sell.Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                t_c_sell.Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                t_c_sell.M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();
                t_c_sell.SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();
                t_c_sell.SellDate_2 = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate_2"].ToString();
                t_c_sell.SellCode = ds.Tables[base_db_name].Rows[fi_cnt]["SellCode"].ToString();
                t_c_sell.SellCodeName = ds.Tables[base_db_name].Rows[fi_cnt]["SellCodeName"].ToString();
                t_c_sell.BusCode = ds.Tables[base_db_name].Rows[fi_cnt]["BusCode"].ToString();
                t_c_sell.BusCodeName = ds.Tables[base_db_name].Rows[fi_cnt]["BusCodeName"].ToString();
                t_c_sell.SellSort = ds.Tables[base_db_name].Rows[fi_cnt]["SellSort"].ToString();
                t_c_sell.Re_BaseOrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["Re_BaseOrderNumber"].ToString();
                t_c_sell.TotalPrice = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
                t_c_sell.TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
                t_c_sell.TotalCV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalCV"].ToString());
                t_c_sell.TotalInputPrice = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalInputPrice"].ToString());
                t_c_sell.Total_Sell_VAT_Price = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Total_Sell_VAT_Price"].ToString());
                t_c_sell.Total_Sell_Except_VAT_Price = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Total_Sell_Except_VAT_Price"].ToString());
                t_c_sell.InputCash = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCash"].ToString());
                t_c_sell.InputCard = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCard"].ToString());
                t_c_sell.InputNaver = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputNaver"].ToString());
                t_c_sell.InputPassbook = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputPassbook"].ToString());
                t_c_sell.InputPassbook_2 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputPassbook_2"].ToString());
                t_c_sell.InputCoupon = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCoupon"].ToString());


                t_c_sell.Be_InputMile = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputMile"].ToString());
                t_c_sell.InputMile = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputMile"].ToString());
                t_c_sell.InputPass_Pay = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputPass_Pay"].ToString());
                t_c_sell.UnaccMoney = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["UnaccMoney"].ToString());

                t_c_sell.Etc1 = ds.Tables[base_db_name].Rows[fi_cnt]["Etc1"].ToString();
                t_c_sell.Etc2 = ds.Tables[base_db_name].Rows[fi_cnt]["Etc2"].ToString();

                t_c_sell.ReturnTF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ReturnTF"].ToString());
                t_c_sell.ReturnTFName = ds.Tables[base_db_name].Rows[fi_cnt]["ReturnTFName"].ToString();
                //t_c_sell.InsuranceNumber = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber"].ToString();
                t_c_sell.INS_Num = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber2"].ToString();

                t_c_sell.InsuranceNumber_Date = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber_Date"].ToString();
                t_c_sell.W_T_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["W_T_TF"].ToString());
                t_c_sell.In_Cnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["In_Cnt"].ToString());

                t_c_sell.RecordID = ds.Tables[base_db_name].Rows[fi_cnt]["RecordID"].ToString();
                t_c_sell.RecordTime = ds.Tables[base_db_name].Rows[fi_cnt]["RecordTime"].ToString();
                t_c_sell.Exi_TF = ds.Tables[base_db_name].Rows[fi_cnt]["Exi_TF"].ToString();

                t_c_sell.SellTF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SellTF"].ToString());
                t_c_sell.SellTFName = ds.Tables[base_db_name].Rows[fi_cnt]["SellTFName"].ToString();

                string t_sellDate = t_c_sell.SellDate.Substring(0, 4);
                t_sellDate = t_sellDate + "-" + t_c_sell.SellDate.Substring(4, 2);
                t_sellDate = t_sellDate + "-" + t_c_sell.SellDate.Substring(6, 2);
                t_c_sell.SellDate = t_sellDate;
                t_c_sell.Us_Ord = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Us_Ord"].ToString());
                if (ds.Tables[base_db_name].Rows[fi_cnt]["Associated_Card"].ToString() == null)
                {
                    t_c_sell.Associated_Card = "";
                }
                else
                {
                    t_c_sell.Associated_Card = ds.Tables[base_db_name].Rows[fi_cnt]["Associated_Card"].ToString();

                }
                //소비자는 1 판매원은 기본 0
                //if (ds.Tables[base_db_name].Rows[0]["Sell_Mem_TF"].ToString() == "1")
                //    opt_sell_3.Checked = true;
                //else
                //    opt_sell_2.Checked = true;


                t_c_sell.Del_TF = "";
                T_SalesDetail[t_c_sell.OrderNumber] = t_c_sell;
            }


            SalesDetail = T_SalesDetail;
        }





        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {
                Base_Clear();
                
            }
            else if (bt.Name == "butt_AddCode")
            {
                frmBase_AddCode e_f = new frmBase_AddCode();
                e_f.Send_Address_Info += new frmBase_AddCode.SendAddressDele(e_f_Send_Address_Info);
                e_f.ShowDialog();
                txtAddress2.Focus();
            }

            else if (bt.Name == "butt_Select")
            {
                if (mtxtMbid.Text == "")
                {
                    MessageBox.Show("아이디를 기입후 검색해주십시오");
                    return;
                }
                else
                {
                    idx_Mbid2 = int.Parse(mtxtMbid.Text.ToString());
                }

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                //dGridView_Base_Header_Reset_item(); //디비그리드 헤더와 기본 셋팅을 한다.
                //cgb_item.d_Grid_view_Header_Reset();


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
              
                
                Set_SalesDetail();

                //Base_Grid_Set();  //뿌려주는 곳
                Base_Grid_Set_2();  //뿌려주는 곳

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
            else if (bt.Name == "butt_S_check")
            {
                dGridView_Base.Visible = false;
                dGridView_Base.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
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
        }
        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Member_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }

        private void e_f_Send_Address_Info(string AddCode1, string AddCode2, string Address1, string Address2, string Address3)
        {
            mtxtZip1.Text = AddCode1 + "-" + AddCode2;
            txtAddress1.Text = Address1; txtAddress2.Text = Address2;


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


        //private void MenuItem_Base_Click(object sender, EventArgs e)
        //{
        //    DXVGrid.GridView view = dGridView_Base;

        //    Point pt = view.GridControl.PointToClient(Control.MousePosition);
        //    DViewInfo.GridHitInfo info = view.CalcHitInfo(pt);

        //    if (!info.InDataRow)
        //        return;

        //    int rIdx = info.RowHandle;

        //   ToolStripMenuItem tm = (ToolStripMenuItem)sender;
        //   if (tm.Name.ToString() == "MenuItem_Sell_1")
        //   {
        //       string Send_Nubmer = ""; string Send_Name = ""; ; string Send_OrderNumber = "";
        //       Send_OrderNumber = dGridView_Base.GetRowCellValue(rIdx, dGridView_Base.Columns["OrderNumber"]).ToString();
        //       Send_Nubmer = dGridView_Base.GetRowCellValue(rIdx, dGridView_Base.Columns["mbid2"]).ToString();
        //       //Send_Name = dGridView_Base.GetRowCellValue(rIdx, dGridView_Base.Columns["mname"]).ToString();
        //       Send_Sell_Number(Send_Nubmer, Send_Name, Send_OrderNumber);   //부모한테 이벤트 발생 신호한다.
        //   }
           
        //   if (tm.Name.ToString() == "MenuItem_Mem_1")
        //   {
        //       string Send_Nubmer = ""; string Send_Name = "";
        //        Send_Nubmer = dGridView_Base.GetRowCellValue(rIdx, dGridView_Base.Columns["mbid2"]).ToString();
        //        //Send_Name = dGridView_Base.GetRowCellValue(rIdx, dGridView_Base.Columns["mname"]).ToString();
        //        Send_Mem_Number(Send_Nubmer, Send_Name);   //부모한테 이벤트 발생 신호한다.
        //   }

        //}

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

                string T_OrderNumber = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();
                string M_Nubmer = view.GetRowCellValue(e.RowHandle, view.Columns["mbid2"]).ToString();

                cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
                cgbp5.dGridView_Put_baseinfo(this, dGridView_Sell_Item, "item", "", T_OrderNumber);



                //cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                //cgbp.dGridView_Put_baseinfo(this, dGridView_Up_S, "saveup", M_Nubmer);


                //cls_Grid_Base_info_Put cgbp2 = new cls_Grid_Base_info_Put();
                //cgbp2.dGridView_Put_baseinfo(this, dGridView_Up_N, "nominup", M_Nubmer);


                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            //dGridView_Base_Header_Reset_item(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_item.d_Grid_view_Header_Reset();

            //DataGridView dgv = (DataGridView)sender;

            //string Tsql = "select a.ordernumber,a.itemcode,b.name, a.itemcount  from tls_Sales_Return_goods a join tbl_goods b on a.itemcode = b.ncode ";
            //Tsql = Tsql + " where a.ordernumber = '"+ dgv.CurrentRow.Cells[1].Value.ToString()+"'";
            //Tsql = Tsql + " group by a.ordernumber,a.itemcode,b.name, a.itemcount ";
            ////++++++++++++++++++++++++++++++++
            //cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            //DataSet ds = new DataSet();
            ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            //int ReCnt = Temp_Connect.DataSet_ReCount;
            ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //// if (Temp_Connect.Open_Data_Set(Tsql_cash, base_db_name, dsCash, this.Name, this.Text) == false) return;
            //if (ReCnt == 0) return;

            //Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            //{
            //    Set_gr_dic2(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.                
            //}
            //cgb_item.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            //cgb_item.db_grid_Obj_Data_Put();
        }
        private void Base_Clear()
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            dGridView_Base_Header_Reset_2(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_main.d_Grid_view_Header_Reset();

            dGridView_Base_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Item.d_Grid_view_Header_Reset();

            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            tabC_1.SelectedIndex = 0;
            T_Search_Nubmer = "";

            idx_Mbid2 = 0;
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtMbid);
            radioB_Returnstatus.Checked = true;
            Reset_Chart_Total();
            //radioB_S.Checked = true;  radioB_R.Checked = true;
  
            combo_Se.SelectedIndex = -1;

            radioB_PassSelect0.Checked = true;
            radioB_return_way2.Checked = true;
        }
        private void butt_Save_Click(object sender, EventArgs e)
        {
            bool check = false;
            string Returnstatus = "";
            string ordernumber = "";
            if (radioB_Returnstatus1.Checked == true)
            {
                Returnstatus = "1";
            }
            else if (radioB_Returnstatus2.Checked == true)
            {
                Returnstatus = "0";
            }
            else if (radioB_Returnstatus.Checked == true)
            {
                MessageBox.Show("처리구분을 선택해 주십시오.");
                return;
            }
          
            for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            {
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    ordernumber = dGridView_Base.Rows[i].Cells[1].Value.ToString();


                    cls_Connect_DB Temp_Connect1 = new cls_Connect_DB();

                    string Tsql1;
                    Tsql1 = "update tls_Sales_Return set Returnstatus = '" + Returnstatus + "', ";
                    Tsql1 = Tsql1 + "statusdate = Convert(Varchar(25),GetDate(),21) , statusperson = '" + cls_User.gid + "'";
                    Tsql1 = Tsql1 + "where ordernumber =  '" + ordernumber + "'";
                    DataSet ds1= new DataSet();
                    if (Temp_Connect1.Open_Data_Set(Tsql1, base_db_name, ds1) == false) return;
                }
            }
            MessageBox.Show("웹 반품 현황 처리가 완료됐습니다.");
            Base_Clear();
        }

    

        private Boolean Check_TextBox_ETC_Error()
        {
            int chk_cnt = 0;

            for (int i = 0; i <= dGridView_Sell_Item.Rows.Count - 1; i++)
            {
                if ( dGridView_Sell_Item.Rows[i].Cells[1].Value == null)
                {
                    dGridView_Sell_Item.Rows[i].Cells[1].Value = "0";
                }

                if (dGridView_Sell_Item.Rows[i].Cells[1].Value.ToString() == "" )
                {
                    dGridView_Sell_Item.Rows[i].Cells[1].Value = "0";
                }

                if (int.Parse(dGridView_Sell_Item.Rows[i].Cells[1].Value.ToString()) > int.Parse(dGridView_Sell_Item.Rows[i].Cells[4].Value.ToString()))
                {
                    MessageBox.Show("선택개수가 주문수량보다 많은 품목이 존재합니다.");
                    return false;
                }
                    //빈칸으로 들어간 내역을 0으로 바꾼다
        
                //0보다 큰 내역이 있는지를 체크한다. 없으면 저장할 내역이 없다는 걸 알리기 위함.
                if (int.Parse(dGridView_Sell_Item.Rows[i].Cells[1].Value.ToString()) > 0)
                {
                    chk_cnt++;
                }
            }
            if (chk_cnt == 0) //저장할 내역이 없을을 알린다.
            {
                MessageBox.Show("반품할 제품을 선택하십시오.");
                return false;
            }
            if (radioB_PassSelect0.Checked == true)
            {
                for (int i = 0; i <= dGridView_Sell_Item.Rows.Count - 1; i++)
                {
                    if (int.Parse(dGridView_Sell_Item.Rows[i].Cells[1].Value.ToString()) != int.Parse(dGridView_Sell_Item.Rows[i].Cells[4].Value.ToString()))
                    {
                        MessageBox.Show("전체반품입니다. 품목과 수량이 맞는지 확인해 주세요.");
                        return false;
                    }
                }
            }
      


            if (txtOrderNumber.Text == "")  //웹아이디 필수값으로넣는다
            {
                MessageBox.Show("주문번호를 제대로 넣어주세요");
                return false;
            }
            if (mtxtMbid.Text == "")  //웹아이디 필수값으로넣는다
            {
                MessageBox.Show("아이디를 제대로 넣어주세요");
                return false;
            }
            if (txt_return_Etc1.Text == "")  //웹아이디 필수값으로넣는다
            {
                MessageBox.Show("반품사유를 제대로 넣어주세요");
                return false;
            }
            //if (radioB_Returnstatus.Checked == true)  //웹아이디 필수값으로넣는다
            //{
            //    MessageBox.Show("처리상태를 제대로 체크해주세요");
            //    return false;
            //}
            if(radioB_return_way2.Checked == false && radioB_return_way1.Checked == false)
            {
                MessageBox.Show("회수방법을 제대로 제대로 넣어주세요");
                return false;
            }
            if (radioB_PassSelect0.Checked == false && radioB_PassSelect1.Checked == false)
            {
                MessageBox.Show("반품여부를 제대로 제대로 넣어주세요");
                return false;
            }

            //if (txtAddress1.Text == "")  //웹아이디 필수값으로넣는다
            //{
            //    MessageBox.Show("주소를 제대로 넣어주세요");
            //    return false;
            //}
            //if (mtxtZip1.Text == "")  //웹아이디 필수값으로넣는다
            //{
            //    MessageBox.Show("우편번호를 제대로 넣어주세요");
            //    return false;
            //}
            if (txt_PassName.Text == "")  //웹아이디 필수값으로넣는다
            {
                MessageBox.Show("수령인명을 제대로 넣어주세요");
                return false;
            }
            if (txt_getTel1.Text == "")  //웹아이디 필수값으로넣는다
            {
                MessageBox.Show("휴대폰번호를 제대로 넣어주세요");
                return false;
            }
            if (txt_retuntfname.Text == "취소")  //웹아이디 필수값으로넣는다
            {
                MessageBox.Show("취소된 내역은 반품현황을 넣을 수 없습니다. ");
                return false;
            }
     
         

        


            //if (txt_PsssNumber.Text == "")  //웹아이디 필수값으로넣는다
            //{ㄹㄹ
            //    MessageBox.Show("운송장번호를 제대로 넣어주세요");
            //    return false;
            //}
            return true;
        }
        private void butt_Save2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("현재 컨트롤에 기입된 내용으로 새로운 반품현황을 저장합니다."+ "\n"+"저장하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            if (Check_TextBox_ETC_Error() == false) return;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            StringBuilder sb = new StringBuilder();
            string StrSql = "";

            // 한국인 경우
            if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
            {
                sb.Append(" INSERT INTO TLS_SALES_RETURN ( /**/");
                sb.Append("    ORDERNUMBER /*주문번호*/");
                sb.Append("  , MBID2 /*MBID2*/");
                sb.Append("  , MBID /*MBID*/");
                sb.Append("  , RETURN_ETC1 /*반품사유*/");
                sb.Append("  , RETURN_ETC2 /*기타 선택시 반품사유*/");
                sb.Append("  , RETURN_WAY /*회수 방법 1: 직접배송 2:택배기사 방문*/");
                sb.Append("  , GETTEL1/*휴대폰 번호*/");
                sb.Append("  , GETTEL2/*집전화번호*/");
                sb.Append("  , PASSCOMPNAY /*직접 배송시 택배사*/");
                sb.Append("  , PSSSNUMBER /*운송장 번호*/");
                sb.Append("  , PASSSELECT /*0 : 전체 반품 1: 부분 반품*/");
                sb.Append("  , PASSNAME /*회수자 명*/");
                sb.Append("  , GET_ZIPCODE /*회수지 우편번호*/");
                sb.Append("  , GET_ADDRESS1 /*회수지 주소*/");
                sb.Append("  , GET_ADDRESS2 /*회수지 상세 주소*/");
                sb.Append("  , RETURNSTATUS /*처리상태*/");
                sb.Append("  , RECORDTIME /*등록일시*/");
                sb.Append("  , STATUSDATE /*처리시간*/");
                sb.Append("  , STATUSPERSON /*처리담당자*/");
                sb.Append(") VALUES (");
                sb.Append(" '" + txtOrderNumber.Text + "'");
                sb.Append(" ,'" + mtxtMbid.Text + "'");
                sb.Append(" ,''");
                sb.Append(" ,'" + txt_return_Etc1.Text + "'");
                sb.Append(" ,'" + txt_return_Etc2.Text + "'");

                if (radioB_return_way1.Checked == true)
                {
                    sb.Append("  ,1");
                }
                if (radioB_return_way2.Checked == true)
                {
                    sb.Append("  ,2");
                }
                sb.Append("  ,'" + txt_getTel1.Text + "'");
                sb.Append("  ,'" + txt_getTel2.Text + "'");
                sb.Append("  ,'" + Combo_Rece_Company.Text + "'");
                sb.Append("  ,'" + txt_PsssNumber.Text + "'");
                if (radioB_PassSelect0.Checked == true)
                {
                    sb.Append("  ,0");
                }
                if (radioB_PassSelect1.Checked == true)
                {
                    sb.Append("  ,1");
                }
                sb.Append("  ,'" + txt_PassName.Text + "'");
                sb.Append("  ,'" + mtxtZip1.Text + "'");
                sb.Append("  ,'" + txtAddress1.Text + "'");
                sb.Append("  ,'" + txtAddress2.Text + "'");
                // if(radioB_Returnstatus1.Checked ==true)
                //{
                //sb.Append("  ,1");
                //}
                //if(radioB_Returnstatus2.Checked ==true)
                //{
                sb.Append("  ,0");
                //}
                sb.Append("  , CONVERT(NVARCHAR(25), GETDATE(), 21)");
                sb.Append("  ,'' ");
                sb.Append(",'" + cls_User.gid + "'");
                sb.Append(")");
            }
            else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
            {
                sb.Append(" INSERT INTO TLS_SALES_RETURN ( /**/");
                sb.Append("    ORDERNUMBER /*주문번호*/");
                sb.Append("  , MBID2 /*MBID2*/");
                sb.Append("  , MBID /*MBID*/");
                sb.Append("  , RETURN_ETC1 /*반품사유*/");
                sb.Append("  , RETURN_ETC2 /*기타 선택시 반품사유*/");
                sb.Append("  , RETURN_WAY /*회수 방법 1: 직접배송 2:택배기사 방문*/");
                sb.Append("  , GETTEL1/*휴대폰 번호*/");
                sb.Append("  , GETTEL2/*집전화번호*/");
                sb.Append("  , PASSCOMPNAY /*직접 배송시 택배사*/");
                sb.Append("  , PSSSNUMBER /*운송장 번호*/");
                sb.Append("  , PASSSELECT /*0 : 전체 반품 1: 부분 반품*/");
                sb.Append("  , PASSNAME /*회수자 명*/");
                sb.Append("  , GET_ZIPCODE /*회수지 우편번호*/");
                sb.Append("  , GET_ADDRESS1 /*회수지 주소*/");
                sb.Append("  , GET_ADDRESS2 /*회수지 상세 주소*/");

                sb.Append("  , Get_City /*태국도시*/");
                sb.Append("  , Get_State /*태국주*/");

                sb.Append("  , RETURNSTATUS /*처리상태*/");
                sb.Append("  , RECORDTIME /*등록일시*/");
                sb.Append("  , STATUSDATE /*처리시간*/");
                sb.Append("  , STATUSPERSON /*처리담당자*/");
                sb.Append(") VALUES (");
                sb.Append(" '" + txtOrderNumber.Text + "'");
                sb.Append(" ,'" + mtxtMbid.Text + "'");
                sb.Append(" ,''");
                sb.Append(" ,'" + txt_return_Etc1.Text + "'");
                sb.Append(" ,'" + txt_return_Etc2.Text + "'");

                if (radioB_return_way1.Checked == true)
                {
                    sb.Append("  ,1");
                }
                if (radioB_return_way2.Checked == true)
                {
                    sb.Append("  ,2");
                }
                sb.Append("  ,'" + txt_getTel1.Text + "'");
                sb.Append("  ,'" + txt_getTel2.Text + "'");
                sb.Append("  ,'" + Combo_Rece_Company.Text + "'");
                sb.Append("  ,'" + txt_PsssNumber.Text + "'");
                if (radioB_PassSelect0.Checked == true)
                {
                    sb.Append("  ,0");
                }
                if (radioB_PassSelect1.Checked == true)
                {
                    sb.Append("  ,1");
                }
                sb.Append("  ,'" + txt_PassName.Text + "'");
                //sb.Append("  ,'" + mtxtZip1.Text + "'");
                sb.Append("  ,'" + txtZipCode_TH.Text + "'");   // 태국 우편번호
                sb.Append("  ,'" + txtAddress1.Text + "'");
                sb.Append("  ,'" + txtAddress2.Text + "'");    // 태국 상세주소

                sb.Append("  ,'" + cbDistrict_TH.Text + "'"); // 태국도시
                sb.Append("  ,'" + cbProvince_TH.SelectedValue.ToString() + "'"); // 태국주

                // if(radioB_Returnstatus1.Checked ==true)
                //{
                //sb.Append("  ,1");
                //}
                //if(radioB_Returnstatus2.Checked ==true)
                //{
                sb.Append("  ,0");
                //}
                sb.Append("  , CONVERT(NVARCHAR(25), GETDATE(), 21)");
                sb.Append("  ,'' ");
                sb.Append(",'" + cls_User.gid + "'");
                sb.Append(")");
            }


            StrSql = sb.ToString();
            Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo", Conn, tran, this.Name, this.Text);
            for (int i = 0; i <= dGridView_Sell_Item.Rows.Count - 1; i++)
            {
                //if (dGridView_Sell_Item.Rows[i].Cells["Col1"].Value.ToString()!= "0" || dGridView_Sell_Item.Rows[i].Cells["Col1"].Value.ToString() != "")
                //{
                    string ordernumber = dGridView_Sell_Item.Rows[i].Cells["OrderNumber"].Value.ToString().Replace(" ", "");
                    string itemcode = dGridView_Sell_Item.Rows[i].Cells["itemcode"].Value.ToString().Replace(" ", "");
                    string itemcount = dGridView_Sell_Item.Rows[i].Cells["Col1"].Value.ToString().Replace(" ", "");


                    string Tsql = "insert into  tls_Sales_Return_goods  (ordernumber ,itemcode  ,itemcount) ";
                    Tsql = Tsql + "values('"+ ordernumber + "','" + itemcode + "', "+itemcount+" ) ";
  
                    Temp_Connect.Insert_Data(Tsql, "tbl_Memberinfo", Conn, tran, this.Name, this.Text);
                //}

            }
            tran.Commit();

            MessageBox.Show("새로운 반품현황이 저장완료됐습니다.");
            Base_Clear();

            radioB_PassSelect0.Checked = true;
            radioB_return_way2.Checked = true;

        }

        private void dGridView_Base_2_DoubleClick(object sender, EventArgs e)
        {
            //Base_Ord_Clear();

            if ((sender as DataGridView).CurrentRow != null && (sender as DataGridView).CurrentRow.Cells[2].Value != null)
            {
                //20231204 오토쉽내역이면 결제가 아예안되게 막아버림
                if ((sender as DataGridView).CurrentRow.Cells[8].Value.ToString() == "자동주문")
                {
                    //tab_Cacu.Visible = false;
                    panel18.Visible = false;
                }
                else
                {
                    //tab_Cacu.Visible = true;
                    panel18.Visible = true;
                }
                if ((sender as DataGridView).CurrentRow.Cells[2].Value.ToString() != "")
                {
                    string OrderNumber = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString();
                    string SellTF = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                    string SellDate = (sender as DataGridView).CurrentRow.Cells[3].Value.ToString();
                    //20231001
                    string Associated_Card = (sender as DataGridView).CurrentRow.Cells[18].Value.ToString();

                    string ReturnTFName = (sender as DataGridView).CurrentRow.Cells[9].Value.ToString();



                    txt_retuntfname.Text = ReturnTFName;


                    Put_OrderNumber_SellDate(OrderNumber);

                }
            }
        }

        private void Put_OrderNumber_SellDate(string OrderNumber)
        {
            Set_SalesDetail(OrderNumber);
            if (SalesItemDetail != null)
                SalesItemDetail.Clear();

            if (Sales_Rece != null)
                Sales_Rece.Clear();

            Set_Sales_Cacu(OrderNumber);
            Set_SalesItemDetail(OrderNumber);  //상품 
            Set_Sales_Rece(OrderNumber);  // 배송 

            Item_Grid_Set(); //상품 그리드
         
        }



        private void Set_Sales_Cacu(string OrderNumber)
        {

            string strSql = "";

            strSql = "Select tbl_Sales_Cacu.* ";
            strSql = strSql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " C_TF_Name ";
            strSql = strSql + " , Isnull(tbl_BankForCompany.BankPenName , '')  C_CodeName_2 ";
            strSql = strSql + " , Isnull(tbl_Bank.bankname , '')  AV_BankName ";

            strSql = strSql + " From tbl_Sales_Cacu (nolock) ";
            strSql = strSql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber ";
            strSql = strSql + " LEFT JOIN tbl_BankForCompany (nolock) ON tbl_Sales_Cacu.C_Code = tbl_BankForCompany.BankCode And  tbl_Sales_Cacu.C_Number1 = tbl_BankForCompany.BankAccountNumber And tbl_SalesDetail.Na_Code = tbl_BankForCompany.Na_Code ";
            strSql = strSql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_Sales_Cacu' And  Ch_T.M_Detail = Convert(Varchar,tbl_Sales_Cacu.C_TF) ";

            strSql = strSql + " LEFT JOIN tbl_Bank (nolock) ON Right(tbl_Sales_Cacu.C_Code,2)  = Right(tbl_Bank.Ncode,2)  And tbl_Sales_Cacu.C_TF = 5   ";
            cls_NationService.SQL_BankNationCode(ref strSql);

            strSql = strSql + " Where tbl_Sales_Cacu.OrderNumber = '" + OrderNumber.ToString() + "' and tbl_Sales_Cacu.c_tf = 3 ";
            strSql = strSql + " Order By C_index ASC ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            Dictionary<int, cls_Sell_Cacu> T_Sales_Cacu = new Dictionary<int, cls_Sell_Cacu>();
            cls_form_Meth cm = new cls_form_Meth();

            for (int fi_cnt = 0; fi_cnt <= 1 - 1; fi_cnt++)
            {
                cls_Sell_Cacu t_c_sell = new cls_Sell_Cacu();

                t_c_sell.OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                t_c_sell.C_index = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_index"].ToString());

                t_c_sell.C_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_TF"].ToString());
                t_c_sell.C_TF_Name = ds.Tables[base_db_name].Rows[fi_cnt]["C_TF_Name"].ToString();

                t_c_sell.C_Code = ds.Tables[base_db_name].Rows[fi_cnt]["C_Code"].ToString();

                if (ds.Tables[base_db_name].Rows[fi_cnt]["AV_BankName"].ToString() != "")
                {
                    t_c_sell.C_CodeName = ds.Tables[base_db_name].Rows[fi_cnt]["AV_BankName"].ToString();
                }
                else
                {
                    t_c_sell.C_CodeName = ds.Tables[base_db_name].Rows[fi_cnt]["C_CodeName"].ToString();
                }
                t_c_sell.C_Coupon = ds.Tables[base_db_name].Rows[fi_cnt]["C_Coupon"].ToString();
                t_c_sell.C_CodeName_2 = ds.Tables[base_db_name].Rows[fi_cnt]["C_CodeName_2"].ToString();

                t_c_sell.C_Name1 = ds.Tables[base_db_name].Rows[fi_cnt]["C_Name1"].ToString();
                t_c_sell.C_Name2 = ds.Tables[base_db_name].Rows[fi_cnt]["C_Name2"].ToString();
                t_c_sell.C_Number1 = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["C_Number1"].ToString());
                // t_c_sell.C_Number1 = encrypter.Decrypt(t_c_sell.C_Number1);
                t_c_sell.C_Number2 = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["C_Number2"].ToString());
                t_c_sell.C_Number3 = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["C_Number3"].ToString());
                t_c_sell.C_Number4 = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["C_Number4"].ToString());

                t_c_sell.C_Price1 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_Price1"].ToString());
                t_c_sell.C_Price2 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_Price2"].ToString());


                t_c_sell.C_AppDate1 = ds.Tables[base_db_name].Rows[fi_cnt]["C_AppDate1"].ToString();
                t_c_sell.C_AppDate2 = ds.Tables[base_db_name].Rows[fi_cnt]["C_AppDate2"].ToString();
                t_c_sell.C_CancelTF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_CancelTF"].ToString());
                t_c_sell.C_CancelDate = ds.Tables[base_db_name].Rows[fi_cnt]["C_CancelDate"].ToString();
                t_c_sell.C_CancelPrice = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_CancelPrice"].ToString());

                t_c_sell.C_Period1 = ds.Tables[base_db_name].Rows[fi_cnt]["C_Period1"].ToString();
                t_c_sell.C_Period2 = ds.Tables[base_db_name].Rows[fi_cnt]["C_Period2"].ToString();
                t_c_sell.C_Installment_Period = ds.Tables[base_db_name].Rows[fi_cnt]["C_Installment_Period"].ToString();
                t_c_sell.C_Etc = ds.Tables[base_db_name].Rows[fi_cnt]["C_Etc"].ToString();

                t_c_sell.Sugi_TF = ds.Tables[base_db_name].Rows[fi_cnt]["Sugi_TF"].ToString();
                t_c_sell.C_P_Number = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["C_P_Number"].ToString());
                t_c_sell.C_B_Number = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["C_B_Number"].ToString());

                t_c_sell.C_Base_Index = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_Base_Index"].ToString());

                t_c_sell.RecordID = ds.Tables[base_db_name].Rows[fi_cnt]["RecordID"].ToString();
                t_c_sell.RecordTime = ds.Tables[base_db_name].Rows[fi_cnt]["RecordTime"].ToString();

                t_c_sell.C_Cash_Number = ds.Tables[base_db_name].Rows[fi_cnt]["C_Cash_Number"].ToString();
                t_c_sell.C_Cash_Send_Nu = ds.Tables[base_db_name].Rows[fi_cnt]["C_Cash_Send_Nu"].ToString();
                t_c_sell.C_Cash_Send_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_Cash_Send_TF"].ToString());
                t_c_sell.C_Cash_Sort_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_Cash_Sort_TF"].ToString());
                t_c_sell.C_Cash_Bus_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_Cash_Bus_TF"].ToString());


                string t_sellDate = "";
                if (t_c_sell.C_AppDate1 != "")
                {
                    t_sellDate = t_c_sell.C_AppDate1.Substring(0, 4);
                    t_sellDate = t_sellDate + "-" + t_c_sell.C_AppDate1.Substring(4, 2);
                    t_sellDate = t_sellDate + "-" + t_c_sell.C_AppDate1.Substring(6, 2);
                }
                t_c_sell.C_AppDate1 = t_sellDate;

                t_sellDate = "";
                if (t_c_sell.C_AppDate2 != "")
                {
                    t_sellDate = t_c_sell.C_AppDate2.Substring(0, 4);
                    t_sellDate = t_sellDate + "-" + t_c_sell.C_AppDate2.Substring(4, 2);
                    t_sellDate = t_sellDate + "-" + t_c_sell.C_AppDate2.Substring(6, 2);
                }
                t_c_sell.C_AppDate2 = t_sellDate;



                t_c_sell.Del_TF = "";
                T_Sales_Cacu[t_c_sell.C_index] = t_c_sell;

                txt_card.Text = t_c_sell.C_Number1;

            }            
        }


        private void Set_SalesDetail(string OrderNumber)
        {
            Data_Set_Form_TF = 1;
            try
            {
                txtOrderNumber.Text = OrderNumber;

                //button_exigo.Visible = false;
                //if (SalesDetail[OrderNumber].Us_Ord == 0)
                //    button_exigo.Visible = true; 
            }
            catch (Exception ex) { }
            Data_Set_Form_TF = 0;

        }
        private void Set_SalesItemDetail(string OrderNumber)
        {

            string strSql = "";

            strSql = "Select tbl_SalesitemDetail.* ";
            strSql = strSql + " , tbl_Goods.Name Item_Name ";

            cls_form_Meth cm = new cls_form_Meth();
            strSql = strSql + " ,Case When SellState = 'N_1' Then '" + cm._chang_base_caption_search("정상") + "'";
            strSql = strSql + "  When SellState = 'N_3' Then '" + cm._chang_base_caption_search("교환_정상") + "'";
            strSql = strSql + "  When SellState = 'R_1' Then '" + cm._chang_base_caption_search("반품") + "'";
            strSql = strSql + "  When SellState = 'R_3' Then '" + cm._chang_base_caption_search("교환_반품") + "'";
            strSql = strSql + "  When SellState = 'C_1' Then '" + cm._chang_base_caption_search("취소") + "'";
            strSql = strSql + " END  SellStateName ";

            strSql = strSql + " From tbl_SalesitemDetail (nolock) ";
            strSql = strSql + " LEFT JOIN tbl_Goods (nolock) ON tbl_Goods.Ncode = tbl_SalesitemDetail.ItemCode ";
            strSql = strSql + " Where tbl_SalesitemDetail.OrderNumber = '" + OrderNumber.ToString() + "'";
            strSql = strSql + " Order By SalesItemIndex ASC ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            Dictionary<int, cls_Sell_Item> T_SalesitemDetail = new Dictionary<int, cls_Sell_Item>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cls_Sell_Item t_c_sell = new cls_Sell_Item();

                t_c_sell.OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();

                t_c_sell.SalesItemIndex = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SalesItemIndex"].ToString());

                t_c_sell.ItemCode = ds.Tables[base_db_name].Rows[fi_cnt]["ItemCode"].ToString();
                t_c_sell.ItemName = ds.Tables[base_db_name].Rows[fi_cnt]["Item_Name"].ToString();
                t_c_sell.ItemPrice = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemPrice"].ToString());
                t_c_sell.ItemPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemPV"].ToString());
                t_c_sell.ItemCV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemCV"].ToString());
                t_c_sell.Sell_VAT_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sell_VAT_TF"].ToString());
                t_c_sell.Sell_VAT_Price = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sell_VAT_Price"].ToString());
                t_c_sell.Sell_Except_VAT_Price = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sell_Except_VAT_Price"].ToString());
                t_c_sell.SellState = ds.Tables[base_db_name].Rows[fi_cnt]["SellState"].ToString();
                t_c_sell.SellStateName = ds.Tables[base_db_name].Rows[fi_cnt]["SellStateName"].ToString();
                t_c_sell.ItemCount = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemCount"].ToString());
                t_c_sell.ItemTotalPrice = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemTotalPrice"].ToString());
                t_c_sell.ItemTotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemTotalPV"].ToString());
                t_c_sell.ItemTotalCV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemTotalCV"].ToString());
                t_c_sell.Total_Sell_VAT_Price = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Total_Sell_VAT_Price"].ToString());
                t_c_sell.Total_Sell_Except_VAT_Price = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Total_Sell_Except_VAT_Price"].ToString());
                t_c_sell.ReturnDate = ds.Tables[base_db_name].Rows[fi_cnt]["ReturnDate"].ToString();
                t_c_sell.SendDate = ds.Tables[base_db_name].Rows[fi_cnt]["SendDate"].ToString();
                t_c_sell.ReturnBackDate = ds.Tables[base_db_name].Rows[fi_cnt]["ReturnBackDate"].ToString();
                t_c_sell.Etc = ds.Tables[base_db_name].Rows[fi_cnt]["Etc"].ToString();
                t_c_sell.RecIndex = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RecIndex"].ToString());
                t_c_sell.Send_itemCount1 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Send_itemCount1"].ToString());
                t_c_sell.Send_itemCount2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Send_itemCount2"].ToString());
                t_c_sell.T_OrderNumber1 = ds.Tables[base_db_name].Rows[fi_cnt]["T_OrderNumber1"].ToString();
                t_c_sell.T_OrderNumber2 = ds.Tables[base_db_name].Rows[fi_cnt]["T_OrderNumber2"].ToString();
                t_c_sell.Real_index = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Real_index"].ToString());
                t_c_sell.G_Sort_Code = ds.Tables[base_db_name].Rows[fi_cnt]["G_Sort_Code"].ToString();


                t_c_sell.RecordID = ds.Tables[base_db_name].Rows[fi_cnt]["RecordID"].ToString();
                t_c_sell.RecordTime = ds.Tables[base_db_name].Rows[fi_cnt]["RecordTime"].ToString();

                t_c_sell.Del_TF = "";
                T_SalesitemDetail[t_c_sell.SalesItemIndex] = t_c_sell;
            }

            SalesItemDetail = T_SalesitemDetail;
        }



        private void Item_Grid_Set()
        {
            dGridView_Base_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Item.d_Grid_view_Header_Reset();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            int fi_cnt = 0;
            int S_cnt4 = 0; double S_cnt5 = 0; double S_cnt6 = 0; double S_cnt7 = 0;
            double S_cnt8 = 0; double S_cnt9 = 0; double S_cnt10 = 0;
            string mp;
            int mp_count = 0;
            foreach (int t_key in SalesItemDetail.Keys)
            {
                if (SalesItemDetail[t_key].Del_TF != "D")
                {
                    Set_gr_Item(ref gr_dic_text, t_key, fi_cnt);  //데이타를 배열에 넣는다.

                    S_cnt4 = S_cnt4 + SalesItemDetail[t_key].ItemCount;
                    S_cnt5 = S_cnt5 + SalesItemDetail[t_key].ItemPrice;
                    S_cnt6 = S_cnt6 + SalesItemDetail[t_key].ItemPV;
                    S_cnt7 = S_cnt7 + SalesItemDetail[t_key].ItemCV;
                    S_cnt8 = S_cnt8 + SalesItemDetail[t_key].ItemTotalPrice;
                    S_cnt9 = S_cnt9 + SalesItemDetail[t_key].ItemTotalPV;
                    S_cnt10 = S_cnt10 + SalesItemDetail[t_key].ItemTotalCV;

                    string Tsql;

                    Tsql = "Select MP_YN FROM TBL_GOODS WHERE NCODE =  '" + SalesItemDetail[t_key].ItemCode + "'";

                    cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                    DataSet ds = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;

                    mp = ds.Tables["tbl_SalesDetail"].Rows[0][0].ToString();
                    if (mp == "Y")
                    {
                        mp_count = mp_count + 1;
                    }
                }
                fi_cnt++;
            }
            //20211012 주문아이템중 MP아이템이 하나라도 들어가면이 아니고, 모두가 MP아이템이면 발동
            //if (mp_count >0)
      

            //if (S_cnt4 != 0 || S_cnt5 != 0 || S_cnt6 != 0 || S_cnt7 != 0 || S_cnt8 != 0)
            //{
            //    cls_form_Meth cm = new cls_form_Meth();

            //    object[] row0 = { ""
            //                    ,"<< " + cm._chang_base_caption_search("합계") + " >>"
            //                    ,""
            //                    ,S_cnt4
            //                    ,S_cnt5

            //                    ,S_cnt6
            //                    ,S_cnt7
            //                    ,S_cnt8
            //                    ,""
            //                    ,""
            //                     };

            //    gr_dic_text[fi_cnt + 2] = row0;
            //}

            cgb_Item.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Item.db_grid_Obj_Data_Put();
        }


        private void Set_gr_Item(ref Dictionary<int, object[]> gr_dic_text, int t_key, int fi_cnt)
        {
            object[] row0 = { SalesItemDetail[t_key].SalesItemIndex
                                 ,SalesItemDetail[t_key].ItemCount
                                ,SalesItemDetail[t_key].ItemCode
                                ,SalesItemDetail[t_key].ItemName
                                 ,SalesItemDetail[t_key].ItemCount

                                ,SalesItemDetail[t_key].ItemPrice
                                ,SalesItemDetail[t_key].ItemPV
                                //20230314구현호 BV값
                                 ,SalesItemDetail[t_key].ItemCV
                                ,SalesItemDetail[t_key].ItemTotalPrice
                                ,SalesItemDetail[t_key].ItemTotalPV

                                ,SalesItemDetail[t_key].ItemTotalCV
                                ,SalesItemDetail[t_key].SellStateName
                                ,SalesItemDetail[t_key].Etc
                                ,SalesItemDetail[t_key].OrderNumber
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }

        //20230313 구현호 여기다
        private void dGridView_Base_Item_Header_Reset()
        {
            //cgb_Item.Grid_Base_Arr_Clear();
            cgb_Item.basegrid = dGridView_Sell_Item;
            cgb_Item.grid_select_mod = DataGridViewSelectionMode.CellSelect;
            cgb_Item.grid_col_Count = 14;
            cgb_Item.grid_Frozen_End_Count = 2;
            cgb_Item.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {
                "", "선택개수"       , "상품_코드"   , "상품명"    , "주문_수량"    , "개별단가"   , "개별PV"
              , "개별CV"  , "총상품액"    , "총상품PV"   , "총상품CV"
              , "구분"   , "_비고" , "주문번호"
                                };

            string[] g_ColsName = {"","Col1"  , "ItemCode"   , "ItemName"  , "ItemCount" 
                                     , "ItemPrice"   , "ItemPV", "ItemBV"   , "ItemTotalPrice"    , "ItemTotalPV"  
                                     , "ItemTotalBV", "Gubun" , "Etc","ORDERNUMBER"
                                };

            int[] g_Width = {0, 100, 90, 160, 80,
                            80, 70,90 , 80 , 80
                            , 80, 70  , 0 , 0
                            };
            DataGridViewColumnSortMode[] g_SortM =
                  {DataGridViewColumnSortMode.NotSortable
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic  //5
                               
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic //10                                       
                               
                                        ,DataGridViewColumnSortMode.Automatic //10                  
                                                 ,DataGridViewColumnSortMode.Automatic //10                  
                                                          ,DataGridViewColumnSortMode.Automatic //10                  
                                                                   ,DataGridViewColumnSortMode.Automatic //10                  
                              };

            cgb_Item.grid_col_SortMode = g_SortM;
            DataGridViewContentAlignment[] g_Alignment =
                                {
                                 DataGridViewContentAlignment.MiddleCenter
                                 ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                  ,DataGridViewContentAlignment.MiddleRight

                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
                                ,DataGridViewContentAlignment.MiddleRight  //6    
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight

                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            


            cgb_Item.grid_col_header_text = g_HeaderText;
            cgb_Item.grid_col_name = g_ColsName;
            cgb_Item.grid_cell_format = gr_dic_cell_format;
            cgb_Item.grid_col_w = g_Width;
            cgb_Item.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = {
                true , false,  true,  true, true
               ,true , true,  true,  true, true
               ,true , true, true, true
                                   };
            cgb_Item.grid_col_Lock = g_ReadOnly;

            cgb_Item.basegrid.RowHeadersVisible = false;
        }
        private void Set_Sales_Rece(string OrderNumber)
        {

            string strSql = "";

            strSql = "Select tbl_Sales_Rece.*  ";
            strSql = strSql + " , Isnull(tbl_Base_Rec.name ,'' ) Base_Rec_Name ";
            strSql = strSql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " Receive_Method_Name ";
            strSql = strSql + " , tbl_Business.name Receive_Center_Name ";
            strSql = strSql + " From tbl_Sales_Rece (nolock) ";
            strSql = strSql + " LEFT JOIN tbl_Base_Rec (nolock) on tbl_Base_Rec.ncode = tbl_Sales_Rece.Base_Rec ";
            strSql = strSql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_Sales_Rece' And  Ch_T.M_Detail = Convert(Varchar,tbl_Sales_Rece.Receive_Method) ";
            strSql = strSql + " LEFT JOIN tbl_Business (nolock) ON tbl_Sales_Rece.Receive_Center = tbl_Business.ncode ";
            strSql = strSql + " Where tbl_Sales_Rece.OrderNumber = '" + OrderNumber.ToString() + "'";
            strSql = strSql + " Order By SalesItemIndex ASC ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            cls_form_Meth cm = new cls_form_Meth();

            Dictionary<int, cls_Sell_Rece> T_Sales_Rece = new Dictionary<int, cls_Sell_Rece>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cls_Sell_Rece t_c_sell = new cls_Sell_Rece();

                t_c_sell.OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                t_c_sell.SalesItemIndex = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SalesItemIndex"].ToString());
                t_c_sell.RecIndex = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["RecIndex"].ToString());
                t_c_sell.Receive_Method = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Receive_Method"].ToString());
                t_c_sell.Receive_Method_Name = ds.Tables[base_db_name].Rows[fi_cnt]["Receive_Method_Name"].ToString();


                t_c_sell.Get_Date1 = ds.Tables[base_db_name].Rows[fi_cnt]["Get_Date1"].ToString();
                t_c_sell.Get_Date2 = ds.Tables[base_db_name].Rows[fi_cnt]["Get_Date2"].ToString();
                t_c_sell.Get_Name1 = ds.Tables[base_db_name].Rows[fi_cnt]["Get_Name1"].ToString();
                t_c_sell.Get_Name2 = ds.Tables[base_db_name].Rows[fi_cnt]["Get_Name2"].ToString();
                t_c_sell.Get_ZipCode = ds.Tables[base_db_name].Rows[fi_cnt]["Get_ZipCode"].ToString();
                t_c_sell.Get_Address1 = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["Get_Address1"].ToString());
                t_c_sell.Get_Address2 = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["Get_Address2"].ToString());

                t_c_sell.Get_Tel1 = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["Get_Tel1"].ToString());
                t_c_sell.Get_Tel2 = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["Get_Tel2"].ToString());

                t_c_sell.Pass_Number = ds.Tables[base_db_name].Rows[fi_cnt]["Pass_Number"].ToString();
                t_c_sell.Pass_Pay = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pass_Pay"].ToString());

                t_c_sell.Pass_Number2 = ds.Tables[base_db_name].Rows[fi_cnt]["Pass_Number2"].ToString();
                t_c_sell.Base_Rec = ds.Tables[base_db_name].Rows[fi_cnt]["Base_Rec"].ToString();
                t_c_sell.Base_Rec_Name = ds.Tables[base_db_name].Rows[fi_cnt]["Base_Rec_Name"].ToString();

                t_c_sell.Get_Etc1 = ds.Tables[base_db_name].Rows[fi_cnt]["Get_Etc1"].ToString();
                t_c_sell.Get_Etc2 = ds.Tables[base_db_name].Rows[fi_cnt]["Get_Etc2"].ToString();

                t_c_sell.Get_state = ds.Tables[base_db_name].Rows[fi_cnt]["Get_state"].ToString();  // 태국 주
                t_c_sell.Get_city = ds.Tables[base_db_name].Rows[fi_cnt]["Get_city"].ToString();  // 태국 도시

                t_c_sell.Receive_Center = ds.Tables[base_db_name].Rows[fi_cnt]["Receive_center"].ToString();
                t_c_sell.Receive_Center_Name = ds.Tables[base_db_name].Rows[fi_cnt]["Receive_Center_Name"].ToString();

                t_c_sell.RecordID = ds.Tables[base_db_name].Rows[fi_cnt]["RecordID"].ToString();
                t_c_sell.RecordTime = ds.Tables[base_db_name].Rows[fi_cnt]["RecordTime"].ToString();
                
                if (t_c_sell.Get_Date1 != "")
                {
                    string t_sellDate = t_c_sell.Get_Date1.Substring(0, 4);
                    t_sellDate = t_sellDate + "-" + t_c_sell.Get_Date1.Substring(4, 2);
                    t_sellDate = t_sellDate + "-" + t_c_sell.Get_Date1.Substring(6, 2);

                    t_c_sell.Get_Date1 = t_sellDate;
                }

                if (t_c_sell.Get_Date2 != "")
                {
                    string t_sellDate = t_c_sell.Get_Date1.Substring(0, 4);
                    t_sellDate = t_sellDate + "-" + t_c_sell.Get_Date2.Substring(4, 2);
                    t_sellDate = t_sellDate + "-" + t_c_sell.Get_Date2.Substring(6, 2);

                    t_c_sell.Get_Date2 = t_sellDate;
                }



                t_c_sell.Del_TF = "";
                T_Sales_Rece[t_c_sell.SalesItemIndex] = t_c_sell;
            }

            txt_PassName.Text = ds.Tables[base_db_name].Rows[0]["Get_Name1"].ToString();
            txt_getTel1.Text = ds.Tables[base_db_name].Rows[0]["Get_Tel1"].ToString();
            txt_getTel2.Text = ds.Tables[base_db_name].Rows[0]["Get_Tel2"].ToString();
            //mtxtZip1.Text = ds.Tables[base_db_name].Rows[0]["Get_ZipCode"].ToString();
            txtAddress1.Text = ds.Tables[base_db_name].Rows[0]["Get_Address1"].ToString();
            txtAddress2.Text = ds.Tables[base_db_name].Rows[0]["Get_Address2"].ToString();

            // 태국인 경우
            if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
            {
                //cbProvince_TH.Text = ds.Tables["t_P_table"].Rows[0]["state"].ToString();
                //cbDistrict_TH.Text = ds.Tables["t_P_table"].Rows[0]["city"].ToString();
                try
                {
                    cbProvince_TH.Text = ds.Tables[base_db_name].Rows[0]["Get_Address2"].ToString().Split(' ')[2];
                    cbDistrict_TH.Text = ds.Tables[base_db_name].Rows[0]["Get_Address2"].ToString().Split(' ')[1];
                    cbSubDistrict_TH.Text = ds.Tables[base_db_name].Rows[0]["Get_Address2"].ToString().Split(' ')[0];
                }
                catch (Exception)
                {
                    cbProvince_TH.Text = "";
                    cbDistrict_TH.Text = "";
                    cbSubDistrict_TH.Text = "";
                }


                txtZipCode_TH.Text = ds.Tables[base_db_name].Rows[0]["Get_ZipCode"].ToString().Replace("-", "");
            }
            // 한국인 경우
            else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
            {
                mtxtZip1.Text = ds.Tables[base_db_name].Rows[0]["Get_ZipCode"].ToString().Replace("-", "");
            }


            Sales_Rece = T_Sales_Rece;
        }

        private void tableLayoutPanel29_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioB_return_way1_Click(object sender, EventArgs e)
        {
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            mtxtZip1.Text = "";
        }

        private void dGridView_Sell_Item_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if ((sender as DataGridView).CurrentCell.ColumnIndex == 1) //1번이 선택
            //{
            //    DataGridView T_DGv = (DataGridView)sender;
            //    if ((T_DGv.CurrentCell.Value == null) || (T_DGv.CurrentCell.Value.ToString() == ""))
            //    {

            //            T_DGv.CurrentCell.Value = "V";

            //    }
            //    else
            //    {
            //        T_DGv.CurrentCell.Value = "";
            //    }
            //}
        }

        private void radioB_PassSelect0_CheckedChanged(object sender, EventArgs e)
        {
                for (int i = 0; i <= dGridView_Sell_Item.Rows.Count - 1; i++)
                {
                    string itemcount = dGridView_Sell_Item.Rows[i].Cells["itemcount"].Value.ToString().Replace(" ", "");
                    dGridView_Sell_Item.Rows[i].Cells["Col1"].Value = itemcount;
                }
        }

        private void dGridView_Sell_Item_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dGridView_Sell_Item.EditingControl.KeyPress += new KeyPressEventHandler(textBoxPart_TextChanged);
        }
        private void textBoxPart_TextChanged(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void radioB_PassSelect1_CheckedChanged(object sender, EventArgs e)
        {
            
                for (int i = 0; i <= dGridView_Sell_Item.Rows.Count - 1; i++)
                {
                    string itemcount = "0";
                    dGridView_Sell_Item.Rows[i].Cells["Col1"].Value = itemcount;
                }
        }

        private void cbProvince_TH_SelectedIndexChanged(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("SELECT ZIPCODE_NM FROM dbo.ufn_Get_ZipCode_City_TH('" + cbProvince_TH.Text + "') ORDER BY ZIPCODE_SORT ");
            sb.AppendLine("SELECT ZIPCODE_NM FROM ufn_Get_ZipCode_District_TH('" + cbProvince_TH.Text + "') ORDER BY MinSubDistrictID ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            cbDistrict_TH.DataBindings.Clear();
            cbDistrict_TH.DataSource = ds.Tables["ZipCode_NM"];
            cbDistrict_TH.DisplayMember = "ZipCode_NM";

        }

        private void cbDistrict_TH_SelectedIndexChanged(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("SELECT * FROM dbo.ufn_Get_ZipCode_TH('" + cbDistrict_TH.Text + "') ");
            sb.AppendLine("SELECT ZIPCODE_NM FROM dbo.ufn_Get_ZipCode_SubDistrict_TH('" + cbDistrict_TH.Text + "') ORDER BY MinSubDistrictID ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            cbSubDistrict_TH.DataBindings.Clear();
            cbSubDistrict_TH.DataSource = ds.Tables["ZipCode_NM"];
            cbSubDistrict_TH.DisplayMember = "ZipCode_NM";
        }

        private void cbSubDistrict_TH_SelectedIndexChanged(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT [ZIPCODE_NM] = PostCode FROM TLS_ZIPCODE_CS WITH(NOLOCK) WHERE SubDistrictThaiShort = '" + cbSubDistrict_TH.Text + "' ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            if (Temp_conn.DataSet_ReCount <= 0) return;

            txtZipCode_TH.Text = "";
            txtZipCode_TH.Text = ds.Tables["ZipCode_NM"].Rows[0][0].ToString();

            //txtAddress2.Text = cbProvince_TH.Text + " " + cbDistrict_TH.Text + " " + cbProvince_TH.SelectedValue.ToString();
            txtAddress2.Text = cbSubDistrict_TH.Text + " " + cbDistrict_TH.Text + " " + cbProvince_TH.Text;


            //cbDistrict_TH.DataBindings.Clear();
            //cbDistrict_TH.DataSource = ds.Tables["ZipCode_NM"];
            //cbDistrict_TH.DisplayMember = "ZipCode_NM";
        }
    }

}
