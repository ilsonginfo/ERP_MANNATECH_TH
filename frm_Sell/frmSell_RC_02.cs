using System;
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
    public partial class frmSell_RC_02 : Form
    {
        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);


        public delegate void Take_NumberDele(ref string Send_Number, ref string Send_Name, ref string Send_OrderNumber);
        public event Take_NumberDele Take_Mem_Number;


        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Item = new cls_Grid_Base();
        cls_Grid_Base cgb_Cacu = new cls_Grid_Base();
        cls_Grid_Base cgb_Cacu_R = new cls_Grid_Base();
        
        private Dictionary<string , cls_Sell> SalesDetail ;
        private Dictionary<int, cls_Sell_Item> SalesItemDetail = new Dictionary<int, cls_Sell_Item>() ;
        private Dictionary<int, cls_Sell_Cacu> Sales_Cacu_R = new Dictionary<int, cls_Sell_Cacu>();
        private Dictionary<int, cls_Sell_Cacu> Sales_Cacu = new Dictionary<int, cls_Sell_Cacu>();

        private Dictionary<string, TextBox>  Ncode_dic = new Dictionary<string, TextBox>();

        private const string base_db_name = "tbl_SalesDetail";
        private int Data_Set_Form_TF;
        private int Tab_Chang_TF;
        private string idx_Mbid = "";
        private int idx_Mbid2 = 0;
        private string idx_Na_Code = "";
        private int idx_Sell_Mem_TF = 0;

        public frmSell_RC_02()
        {
            InitializeComponent();
        }

        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;
            Tab_Chang_TF = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset(1);

            dGridView_Base_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Item.d_Grid_view_Header_Reset(1);

            dGridView_Base_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Cacu.d_Grid_view_Header_Reset(1);

            dGridView_Base_Cacu_R_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Cacu_R.d_Grid_view_Header_Reset(1);         
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
            
            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;            
            mtxtSn.Mask = "999999-9999999"; //기본 셋팅은 주민번호이다.   

            string[] data_Y = { ""
                              , int.Parse (cls_User.gid_date_time.Substring (0,4)).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 1 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 2 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 3 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 4 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 5 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 6 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 7 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 8 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 9 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 10 ).ToString()
                              };

            string[] data_M = { "","01", "02", "03", "04", "05" 
                               , "06", "07", "08", "09", "10" 
                               , "11", "12" 
                              };

            string[] data_P = { ""
                               , cm._chang_base_caption_search("일시불")
                               //,"1"
                               , "2", "3", "4", "5" 
                               , "6", "7", "8", "9", "10" 
                               , "11", "12" 
                              };

            // 각 콤보박스에 데이타를 초기화
            combo_C_Card_Year.Items.AddRange(data_Y);
            combo_C_Card_Month.Items.AddRange(data_M);
            combo_C_Card_Per.Items.AddRange(data_P);


            mtxtSn.BackColor = cls_app_static_var.txt_Enable_Color;
            txtCenter.BackColor = cls_app_static_var.txt_Enable_Color;
            txtSellDate.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_OrderNumber.BackColor = cls_app_static_var.txt_Enable_Color;
            txtSellCode.BackColor = cls_app_static_var.txt_Enable_Color;
            txtCenter2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_TotalInputPrice.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_UnaccMoney.BackColor = cls_app_static_var.txt_Enable_Color;

            txt_TotalPv.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC1.BackColor = cls_app_static_var.txt_Enable_Color;            
            txt_TotalPrice.BackColor = cls_app_static_var.txt_Enable_Color;

            txt_OrderNumber_R.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_TotalPv_R.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_TotalPrice_R.BackColor = cls_app_static_var.txt_Enable_Color;

            txt_SumCard.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_SumCash.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_SumBank.BackColor = cls_app_static_var.txt_Enable_Color;

             txt_C_Bank_Code.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_C_Bank_Code_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_C_Bank_Code_3.BackColor = cls_app_static_var.txt_Enable_Color;


            txtSellDateRe.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_OrderNumber_R.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_TotalPrice_R.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_TotalPv_R.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_TotalInputPrice_R.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_ETC1_R.BackColor = cls_app_static_var.txt_Enable_Color;

            txtPay1.BackColor = cls_app_static_var.txt_Enable_Color;
            txtPayDate1.BackColor = cls_app_static_var.txt_Enable_Color;
     
            mtxtPayDateR1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtPriceDate3.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtPriceDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtPriceDate2.Mask = cls_app_static_var.Date_Number_Fromat;
      
        }



        private void frmBase_Resize(object sender, EventArgs e)
        {
            //int base_w = this.Width / 4;
            //butt_Clear.Width = base_w;
            //butt_Save.Width = base_w;

            //butt_Delete.Width = base_w;
            //butt_Exit.Width = base_w;

            //butt_Clear.Left = 0;
            //butt_Save.Left = butt_Clear.Left + butt_Clear.Width;

            //butt_Delete.Left = butt_Save.Left + butt_Save.Width;
            //butt_Exit.Left = butt_Delete.Left + butt_Delete.Width;    


            butt_Clear.Left = 0;
            butt_Save.Left = butt_Clear.Left + butt_Clear.Width + 2;
            //butt_Excel.Left = butt_Save.Left + butt_Save.Width + 2;
            butt_Delete.Left = butt_Save.Left + butt_Save.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Save);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);

            cfm.button_flat_change(butt_Cacu_Del);
            cfm.button_flat_change(butt_Cacu_Save);
            cfm.button_flat_change(butt_Cacu_Clear);
            cfm.button_flat_change(butt_Ord_Clear);
            cfm.button_flat_change(button_Ok);
            cfm.button_flat_change(button_Cancel);

            
        }


        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();
            string Send_Number = ""; string Send_Name = ""; string Send_OrderNumber = "";
            Take_Mem_Number(ref Send_Number, ref Send_Name, ref Send_OrderNumber);


            if (Send_Number != "")
            {
                mtxtMbid.Text = Send_Number;
                Set_Form_Date(mtxtMbid.Text, "m");
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
                           // cfm.form_Group_Panel_Enable_True(this);
                        }
                    }
                }// end if

            }


            Button T_bt = butt_Exit;
            if (e.KeyValue == 123)
                T_bt = butt_Exit;    //닫기  F12
            if (e.KeyValue == 113)
                T_bt = butt_Save;     //저장  F1
            //if (e.KeyValue == 115)
            //    T_bt = butt_Delete;   // 삭제  F4
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
                            if (Input_Error_Check(mtb, "m") == true)
                                Set_Form_Date(mtb.Text, "m");
                            //SendKeys.Send("{TAB}");
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

        void e_f_Send_MemNumber_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = "";
            cls_Search_DB csb = new cls_Search_DB();
            csb.Member_Nmumber_Split(mtxtMbid.Text.Trim(), ref searchMbid, ref searchMbid2);
        }

   

        //변경할려는 대상자에 대한 회원번호에서 회원 검색창을 뛰엇을 경우에
        void e_f_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            mtxtMbid.Text = Send_Number; txtName.Text = Send_Name;
            if (Input_Error_Check(mtxtMbid, "m") == true)
                Set_Form_Date(mtxtMbid.Text, "m");
        }


        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            MaskedTextBox mtb = (MaskedTextBox)sender;

            if (mtb.Text.Replace("_", "").Replace("-", "").Replace(" ", "") == "")
            {                       
                if (mtb.Name == "mtxtMbid")
                {
                    _From_Data_Clear();                
                }
                
            }
        }


        //회원번호 클릿햇을때. 관련 정보들 다 리셋 시킨다.
        //추후 번호만 변경하고 엔터 안누눌러서.. 데이타가 엉키는 것을 방지하기 위함.
        private void mtxtMbid_Click(object sender, EventArgs e)
        {
            MaskedTextBox mtb = (MaskedTextBox)sender;
                        
            if (mtb.Name == "mtxtMbid")
            {
                _From_Data_Clear();                
            }
           

            //마스크텍스트 박스에 입력한 내용이 있으면 그곳 다음으로 커서가 가게 한다.
            if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                mtb.SelectionStart = mtb.Text.Replace("-", "").Replace("_", "").Trim().Length + 1;

        }











        private void Set_Form_Date(string T_Mbid, string T_sort )
        {
            _From_Data_Clear();   
            //idx_Mbid = ""; idx_Mbid2 = 0;
            string Mbid = ""; int Mbid2 = 0;
            Data_Set_Form_TF = 1;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == 1)
            {
                string Tsql = "";
                
                Tsql = "Select  ";
                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                    Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) AS M_Mbid ";
                else if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                    Tsql = Tsql + " tbl_Memberinfo.mbid2 AS M_Mbid ";
                else if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                    Tsql = Tsql + " tbl_Memberinfo.mbid AS M_Mbid ";

                Tsql = Tsql + " ,tbl_Memberinfo.mbid ";
                Tsql = Tsql + " ,tbl_Memberinfo.mbid2 ";
                Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";     

                Tsql = Tsql + ", tbl_Memberinfo.Cpno ";

                Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";
                
                Tsql = Tsql + " , tbl_Memberinfo.RegTime ";
                Tsql = Tsql + " , tbl_Memberinfo.hptel ";
                Tsql = Tsql + " , tbl_Memberinfo.hometel ";

                Tsql = Tsql + " , tbl_Memberinfo.businesscode ";
                Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";
                
                Tsql = Tsql + " , tbl_Memberinfo.Remarks ";
                
                Tsql = Tsql + " , tbl_Memberinfo.LeaveDate ";
                Tsql = Tsql + " , tbl_Memberinfo.LineUserDate ";
                Tsql = Tsql + " , tbl_Memberinfo.WebID ";
                Tsql = Tsql + " , tbl_Memberinfo.WebPassWord ";
                Tsql = Tsql + " , tbl_Memberinfo.Ed_Date ";
                Tsql = Tsql + " , tbl_Memberinfo.PayStop_Date ";

                Tsql = Tsql + " , tbl_Memberinfo.For_Kind_TF ";
                Tsql = Tsql + " , tbl_Memberinfo.Sell_Mem_TF ";
                             

                Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
                Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";
                
    

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                }

                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid.ToString() + "'" ;
                }



                //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
                Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";

                Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";




                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text ) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;
                //++++++++++++++++++++++++++++++++
                Set_Form_Date(ds); //위의 DataSet객체를 가져가서 회원 정보를 넣는다

                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                if (SalesDetail != null)
                    SalesDetail.Clear();

                Set_SalesDetail();  //회원의 주문 관련 주테이블 내역을 클래스에 넣는다.

                if (SalesDetail != null)
                    Base_Grid_Set();

                mtxtMbid.Focus();                
            }
            
            Data_Set_Form_TF = 0;            
        }

        private void Set_Form_Date(DataSet ds)
        {
            idx_Mbid =  ds.Tables[base_db_name].Rows[0]["Mbid"].ToString();
            idx_Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[0]["Mbid2"].ToString());
            
            mtxtMbid.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();
            txtName.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();
            mtxtSn.Text = encrypter.Decrypt ( ds.Tables[base_db_name].Rows[0]["Cpno"].ToString() ,"Cpno");
                  
            txtCenter.Text = ds.Tables[base_db_name].Rows[0]["B_Name"].ToString();
            txtCenter_Code.Text = ds.Tables[base_db_name].Rows[0]["businesscode"].ToString();
            
            txtName.ReadOnly = true;
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.BackColor = cls_app_static_var.txt_Enable_Color;
        }


        private void  Set_SalesDetail ()
        {
            cls_form_Meth cm = new cls_form_Meth();
            string strSql = "";

            strSql = "Select tbl_SalesDetail.* ";
            strSql = strSql + " , tbl_Business.Name BusCodeName ";
            strSql = strSql + " , tbl_SellType.SellTypeName SellCodeName  ";
            strSql = strSql + " ,Case When ReturnTF = 1 Then '" + cm._chang_base_caption_search("정상") + "'";
            strSql = strSql + "  When ReturnTF = 2 Then '" + cm._chang_base_caption_search("반품") + "'";
            strSql = strSql + "  When ReturnTF = 4 Then '" + cm._chang_base_caption_search("교환") + "'";
            strSql = strSql + "  When ReturnTF = 3 Then '" + cm._chang_base_caption_search("부분반품") + "'";
            strSql = strSql + "  When ReturnTF = 5 Then '" + cm._chang_base_caption_search("취소") + "'";
            strSql = strSql + " END ReturnTFName ";


            strSql = strSql + " , Ga_Order SellTF ";
            strSql = strSql + " ,Case When Ga_Order >= 1 Then '" + cm._chang_base_caption_search("미승인") + "'";
            strSql = strSql + "  When Ga_Order = 0 Then '" + cm._chang_base_caption_search("승인") + "'";
            strSql = strSql + " ELSE '' ";
            strSql = strSql + " END SellTFName ";

            //strSql = strSql + " , Case When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE = '0000' Then ISNULL(T_REALMLM.GUARANTE_NUM,'') ";
            //strSql = strSql + "        When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE <> '0000' Then  ISNULL(T_REALMLM_ErrCode.Er_Msg ,'' ) ";
            //strSql = strSql + "        When  tbl_SalesDetail.union_Seq = 0 Then '미신고'  ";
            //strSql = strSql + "   End INS_Number ";

            if (cls_app_static_var.Sell_Union_Flag == "U")  //특판
            {
                strSql = strSql + " , Case When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE = '0000' Then ISNULL(T_REALMLM.GUARANTE_NUM,'') ";
                strSql = strSql + "        When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE <> '0000' Then  ISNULL(T_REALMLM_ErrCode.Er_Msg ,'' ) ";
                strSql = strSql + "        When  tbl_SalesDetail.union_Seq = 0 Then '미신고'  ";
                strSql = strSql + "   End   InsuranceNumber2";
            }
            else if (cls_app_static_var.Sell_Union_Flag == "D")  //직판
            {
                strSql = strSql + ", Case When  ReturnTF = 1 And (Select TOP 1 A1.SellDate From tbl_SalesDetail AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NULL And tbl_SalesDetail.InsuranceNumber <> '' Then tbl_SalesDetail.InsuranceNumber ";
                strSql = strSql + " When  ReturnTF = 1 And (Select TOP 1 A1.SellDate From tbl_SalesDetail AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                strSql = strSql + " When  ReturnTF = 5 And InsuranceNumber_Cancel ='Y' Then tbl_SalesDetail.InsuranceNumber + '(취소상태)' ";
                strSql = strSql + " When  ReturnTF = 1 And (Select TOP 1 A1.SellDate From tbl_SalesDetail AS A1 Where tbl_SalesDetail.OrderNumber = A1.Re_BaseOrderNumber) IS NOT NULL And InsuranceNumber_Cancel ='' Then tbl_SalesDetail.InsuranceNumber + '(취소요청중)' ";
                strSql = strSql + " When  ReturnTF = 1 And tbl_SalesDetail.InsuranceNumber = '' Then '미승인요청' ";
                strSql = strSql + " ELSE tbl_SalesDetail.InsuranceNumber END  InsuranceNumber2 ";
            }
            else
            {
                strSql = strSql + " , InsuranceNumber As InsuranceNumber2 ";
            }


            strSql = strSql + " From tbl_SalesDetail (nolock) ";
            //strSql = strSql + " LEFT JOIN tbl_SalesDetail_TF (nolock) ON tbl_SalesDetail.OrderNumber = tbl_SalesDetail_TF.OrderNumber ";
            strSql = strSql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";
            strSql = strSql + " LEFT Join tbl_SellType ON tbl_SalesDetail.SellCode = tbl_SellType.SellCode ";
            strSql = strSql + " LEFT JOIN tbl_Business (nolock) ON  ISNULL(tbl_SalesDetail.BusCode, '')  = tbl_Business.NCode And tbl_SalesDetail.Na_code = tbl_Business.Na_code ";

            strSql = strSql + " LEFT JOIN T_REALMLM (nolock) ON T_REALMLM.SEQ = tbl_SalesDetail.union_Seq ";
            strSql = strSql + " LEFT JOIN T_REALMLM_ErrCode (nolock) ON T_REALMLM.ERRCODE = T_REALMLM_ErrCode.Er_Code ";


            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                strSql = strSql + " Where tbl_Memberinfo.Mbid = '" + idx_Mbid + "' ";
                strSql = strSql + " And   tbl_Memberinfo.Mbid2 = " + idx_Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                strSql = strSql + " Where tbl_Memberinfo.Mbid2 = " + idx_Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                strSql = strSql + " Where tbl_Memberinfo.Mbid = '" + idx_Mbid.ToString() + "'";
            }

            //// strSql = strSql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";
            strSql = strSql + " And (Ga_Order = 0 ) "; //정상내역은 승인 내역만 보여준다.

            strSql = strSql + " And tbl_SalesDetail.SellCode <> '' ";

            //strSql = strSql + " And tbl_SalesDetail.ReturnTF = 2 ";   //---반품한 내역만 불러온다.
            //처음에는 반품한 내역만 불어 오기로 했으나 우선을 매출 다 불러오고 그리드 상에서만 반품 내역만 
            //보여지게 해주는게 더 낳을듯 함

            strSql = strSql + "  Order By  CAST(tbl_SalesDetail.RecordTime AS DATETIME) DESC";

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
                t_c_sell.Mbid2 = int.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                t_c_sell.M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();
                t_c_sell.SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();
                t_c_sell.SellCode = ds.Tables[base_db_name].Rows[fi_cnt]["SellCode"].ToString();
                t_c_sell.SellCodeName = ds.Tables[base_db_name].Rows[fi_cnt]["SellCodeName"].ToString();
                t_c_sell.BusCode = ds.Tables[base_db_name].Rows[fi_cnt]["BusCode"].ToString();
                t_c_sell.BusCodeName = ds.Tables[base_db_name].Rows[fi_cnt]["BusCodeName"].ToString();
                t_c_sell.Re_BaseOrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["Re_BaseOrderNumber"].ToString();
                t_c_sell.TotalPrice = double.Parse ( ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
                t_c_sell.TotalPV = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
                t_c_sell.TotalCV = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["TotalCV"].ToString());
                t_c_sell.TotalInputPrice = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["TotalInputPrice"].ToString());
                t_c_sell.Total_Sell_VAT_Price = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["Total_Sell_VAT_Price"].ToString());
                t_c_sell.Total_Sell_Except_VAT_Price = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["Total_Sell_Except_VAT_Price"].ToString());
                t_c_sell.InputCash = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["InputCash"].ToString());
                t_c_sell.InputCard = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["InputCard"].ToString());
                t_c_sell.InputPassbook = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["InputPassbook"].ToString());
                t_c_sell.InputMile = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["InputMile"].ToString());
                t_c_sell.InputCoupon = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCoupon"].ToString());
                t_c_sell.InputPass_Pay = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["InputPass_Pay"].ToString());
                t_c_sell.UnaccMoney = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["UnaccMoney"].ToString());
                t_c_sell.InputNaver = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputNaver"].ToString());

                t_c_sell.InputPayment_8_TH = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputPayment_8_TH"].ToString());
                t_c_sell.InputPayment_9_TH = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputPayment_9_TH"].ToString());
                t_c_sell.InputPayment_10_TH = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputPayment_10_TH"].ToString());

                t_c_sell.Etc1 = ds.Tables[base_db_name].Rows[fi_cnt]["Etc1"].ToString();
                t_c_sell.Etc2 = ds.Tables[base_db_name].Rows[fi_cnt]["Etc2"].ToString();

                t_c_sell.ReturnTF = int.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["ReturnTF"].ToString());
                t_c_sell.ReturnTFName = ds.Tables[base_db_name].Rows[fi_cnt]["ReturnTFName"].ToString();
                //t_c_sell.INS_Num = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber"].ToString();
                t_c_sell.INS_Num = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber2"].ToString();
                t_c_sell.InsuranceNumber_Date = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber_Date"].ToString();
                t_c_sell.W_T_TF = int.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["W_T_TF"].ToString());
                t_c_sell.In_Cnt = int.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["In_Cnt"].ToString());

                t_c_sell.RecordID = ds.Tables[base_db_name].Rows[fi_cnt]["RecordID"].ToString();
                t_c_sell.RecordTime = ds.Tables[base_db_name].Rows[fi_cnt]["RecordTime"].ToString();

                t_c_sell.SellTF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SellTF"].ToString());
                t_c_sell.SellTFName = ds.Tables[base_db_name].Rows[fi_cnt]["SellTFName"].ToString();
               
               
                string  t_sellDate = t_c_sell.SellDate.Substring(0,4) ;
                t_sellDate =t_sellDate + "-" + t_c_sell.SellDate.Substring(4,2) ;
                t_sellDate =t_sellDate + "-" +  t_c_sell.SellDate.Substring(6,2) ;
                t_c_sell.SellDate = t_sellDate; 

                t_c_sell.Del_TF = "" ;

                T_SalesDetail[t_c_sell.OrderNumber] = t_c_sell;                
            }

            
            SalesDetail = T_SalesDetail;
        }










        //////SalesDetail___SalesDetail__SalesDetail__SalesDetail__SalesDetail__SalesDetail
        //////SalesDetail___SalesDetail__SalesDetail__SalesDetail__SalesDetail__SalesDetail
        private void Base_Grid_Set()
        {
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            int fi_cnt = 0;
            double Sum_TotalPrice = 0;
            double Sum_TotalInputPrice = 0;
            double Sum_TotalPV = 0;
            double Sum_InputCash = 0;
            double Sum_InputCard = 0;
            double Sum_InputPassBook = 0;
            double Sum_InputPassBook_2 = 0;
            double Sum_InputMile = 0;
            double Sum_UnaccMoney = 0;
            double Sum_InputNaver = 0;
            double Sum_TH_8 = 0, Sum_TH_9 = 0, Sum_TH_10 = 0, Sum_Coupon = 0 ;
            foreach (string t_key in SalesDetail.Keys)
            {
                //교환내역만 보이게 한다.
                if (SalesDetail[t_key].Del_TF != "D" && (SalesDetail[t_key].ReturnTF == 4 || SalesDetail[t_key].ReturnTF == 3 || SalesDetail[t_key].ReturnTF == 2))
                {
                    Set_gr_dic(ref gr_dic_text, t_key, fi_cnt);  //데이타를 배열에 넣는다.

                    Sum_TotalPrice += SalesDetail[t_key].TotalPrice;
                    Sum_TotalInputPrice = Sum_TotalInputPrice + SalesDetail[t_key].TotalInputPrice;
                    Sum_TotalPV = Sum_TotalPV + SalesDetail[t_key].TotalPV;

                    Sum_InputCash = Sum_InputCash + SalesDetail[t_key].InputCash;
                    Sum_InputCard = Sum_InputCard + SalesDetail[t_key].InputCard;
                    Sum_InputPassBook = Sum_InputPassBook + SalesDetail[t_key].InputPassbook;
                    Sum_InputPassBook_2 = Sum_InputPassBook_2 + SalesDetail[t_key].InputPassbook_2;
                    Sum_UnaccMoney = Sum_UnaccMoney + SalesDetail[t_key].UnaccMoney;

                    Sum_InputMile += SalesDetail[t_key].InputMile;
                    Sum_InputNaver = Sum_InputNaver + SalesDetail[t_key].InputNaver;
                    Sum_Coupon += SalesDetail[t_key].InputCoupon;

                    Sum_TH_8  += SalesDetail[t_key].InputPayment_8_TH;
                    Sum_TH_9  += SalesDetail[t_key].InputPayment_9_TH;
                    Sum_TH_10 += SalesDetail[t_key].InputPayment_10_TH;
                }

                fi_cnt++;
            }

            cls_form_Meth cm = new cls_form_Meth();

            object[] row0 = { ""
                                ,"<< " + cm._chang_base_caption_search("합계") + " >>"
                                ,""
                                ,""
                                ,Sum_TotalPrice

                                ,Sum_TotalInputPrice                                
                                ,Sum_TotalPV
                                ,""
                                ,""
                                ,""
                       
                                ,Sum_InputCash      
                                ,Sum_InputCard
                                ,Sum_InputPassBook
                                ,Sum_InputPassBook_2
                                ,Sum_Coupon

                                ,Sum_InputMile
                                ,Sum_InputNaver
                                ,Sum_TH_8
                                ,Sum_TH_9
                                ,Sum_TH_10 

                                ,Sum_UnaccMoney

                                ,""
                                ,""
                            };

            gr_dic_text[fi_cnt + 2] = row0;


            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
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

                                ,SalesDetail[t_key].InputMile
                                ,SalesDetail[t_key].InputNaver
                                ,SalesDetail[t_key].InputPayment_8_TH
                                ,SalesDetail[t_key].InputPayment_9_TH
                                ,SalesDetail[t_key].InputPayment_10_TH


                                ,SalesDetail[t_key].UnaccMoney 
                                ,SalesDetail[t_key].RecordID 
                                ,SalesDetail[t_key].RecordTime
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Base_Header_Reset()
        {
            cgb.Grid_Base_Arr_Clear();
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_col_Count = 23;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {

                 "결제_승인여부" , "공제번호"  , "주문번호"   , "주문일자"  , "총주문액"       
               , "총입금액"      , "총PV"      , "총CV"       , "주문종류"  , "구분"     
               , "현금"          , "카드"      , "무통장"     , "가상계좌"      , "쿠폰"
               , "마일리지"      , "네이버페이", "프로모트페이_TH", "온라인뱅킹_TH" , "모바일뱅킹_TH"
               , "미결제"  ,  "기록자" ,  "기록일" 
                                };

            int Witdh_Mile = cls_app_static_var.Using_Mileage_TF != 0 ? 80 : 0;
            int Witdh_Naver = cls_User.gid_CountryCode == "KR" ? 80 : 0;

            bool Is_NaCode_TH = cls_User.gid_CountryCode == "TH";
            int Witdh_TH_Payment = Is_NaCode_TH ? 80 : 0;


            if (cls_app_static_var.Sell_Union_Flag == "")
            {
                int[] g_Width = { 80,0, 120, 80, 80
                              , 80  ,80 , 80 , 80, 80 
                              , 80  ,80 , 80 , 80, 80
                              , Witdh_Mile  ,Witdh_Naver , Witdh_TH_Payment , Witdh_TH_Payment, Witdh_TH_Payment
                              , 80  ,80 , 80
                            };
                cgb.grid_col_w = g_Width;
            }
            else
            {
                int[] g_Width = { 80,120, 120, 80, 80
                              , 80  ,80 , 80 , 80, 80
                              , 80  ,80 , 80 , 80, 80
                              , Witdh_Mile  ,Witdh_Naver , Witdh_TH_Payment , Witdh_TH_Payment, Witdh_TH_Payment
                              , 80  ,80 , 80
                            };
                cgb.grid_col_w = g_Width;
            }

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleRight//5    

                                ,DataGridViewContentAlignment.MiddleRight    
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleCenter                                  
                                ,DataGridViewContentAlignment.MiddleCenter//10

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
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;

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

            cgb.grid_col_header_text = g_HeaderText;
            cgb.grid_cell_format = gr_dic_cell_format;
            
            cgb.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true 
                                    ,true , true,  true,  true ,true 
                                    ,true , true,  true,  true ,true 
                                    ,true , true,  true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            cgb.basegrid.RowHeadersVisible = false;
        }
        //////SalesDetail___SalesDetail__SalesDetail__SalesDetail__SalesDetail__SalesDetail
        //////SalesDetail___SalesDetail__SalesDetail__SalesDetail__SalesDetail__SalesDetail





        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail
        private void Base_Sub_Sum_Item()
        {
            if (SalesItemDetail == null)
            {
                txt_TotalPrice.Text = "0";
                txt_TotalPv.Text = "0";
                txt_TotalBv.Text = "0";
                return;
            }
            
            int fi_cnt = 0; double T_Pv = 0; double T_pr = 0; double T_Bv = 0;

            foreach (int t_key in SalesItemDetail.Keys)
            {
                if (SalesItemDetail[t_key].Del_TF != "D")
                {
                    T_Pv = T_Pv + SalesItemDetail[t_key].ItemTotalPV  ;
                    T_pr = T_pr + SalesItemDetail[t_key].ItemTotalPrice  ;
                    T_Bv = T_Bv + SalesItemDetail[t_key].ItemTotalCV;
                }
                fi_cnt++;
            }

            txt_TotalPrice.Text = string.Format(cls_app_static_var.str_Currency_Type, T_pr);
            txt_TotalPv.Text = string.Format(cls_app_static_var.str_Currency_Type, T_Pv);
            txt_TotalBv.Text = string.Format(cls_app_static_var.str_Currency_Type, T_Bv);
        }


        private void Item_Grid_Set()
        {
            dGridView_Base_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Item.d_Grid_view_Header_Reset();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            int fi_cnt = 0;
            foreach (int t_key in SalesItemDetail.Keys)
            {
                if (SalesItemDetail[t_key].Del_TF != "D")
                    Set_gr_Item(ref gr_dic_text, t_key, fi_cnt);  //데이타를 배열에 넣는다.
                fi_cnt++;
            }

            cgb_Item.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Item.db_grid_Obj_Data_Put();
        }


        private void Set_gr_Item(ref Dictionary<int, object[]> gr_dic_text, int t_key, int fi_cnt)
        {
            object[] row0 = { SalesItemDetail[t_key].SalesItemIndex   
                                ,SalesItemDetail[t_key].ItemCode  
                                ,SalesItemDetail[t_key].ItemName   
                                ,SalesItemDetail[t_key].ItemPrice   
                                ,SalesItemDetail[t_key].ItemPV    

                                ,SalesItemDetail[t_key].ItemCount   
                                ,SalesItemDetail[t_key].ItemTotalPrice 
                                ,SalesItemDetail[t_key].ItemTotalPV
                                ,SalesItemDetail[t_key].ItemTotalCV
                                ,SalesItemDetail[t_key].SellStateName 
                                ,SalesItemDetail[t_key].Etc  
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Base_Item_Header_Reset()
        {
            cgb_Item.Grid_Base_Arr_Clear();
            cgb_Item.basegrid = dGridView_Base_Item;
            cgb_Item.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Item.grid_col_Count = 11;
            cgb_Item.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {""  , "상품_코드"   , "상품명"  , "개별단가"   , "개별PV"        
                                , "주문_수량"   , "총상품액"    , "총상품PV"  , "총상품CV" , "구분" 
                                , "비고"
                                };

            int[] g_Width = { 0, 90, 160, 80, 70
                                ,80 , 80 , 80 ,80, 70 , 200
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleLeft  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleRight
                                 ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleLeft  //10
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;


            cgb_Item.grid_col_header_text = g_HeaderText;
            cgb_Item.grid_cell_format = gr_dic_cell_format;
            cgb_Item.grid_col_w = g_Width;
            cgb_Item.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  ,true
                                    ,true , true,  true,  true ,true                                                            
                                   };
            cgb_Item.grid_col_Lock = g_ReadOnly;

            cgb_Item.basegrid.RowHeadersVisible = false;
        }
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail



        //////Sales_Cacu___Sales_Cacu__Sales_Cacu__Sales_Cacu
        //////Sales_Cacu___Sales_Cacu__Sales_Cacu__Sales_Cacu

        private void Base_Sub_Sum_Cacu()
        {
            
            if (Sales_Cacu_R == null)
            {
                txt_TotalInputPrice_R.Text = "0";            
                return;
            }

            double T_pr = 0;

            foreach (int t_key in Sales_Cacu_R.Keys)
            {
                if (Sales_Cacu_R[t_key].Del_TF != "D")
                {
                    T_pr = T_pr + Sales_Cacu_R[t_key].C_Price1;      
              

                }                
            }

            txt_TotalInputPrice_R.Text = string.Format(cls_app_static_var.str_Currency_Type, T_pr);            
        }


        private void Cacu_Grid_Set()
        {
            dGridView_Base_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Cacu.d_Grid_view_Header_Reset();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            int fi_cnt = 0;
            foreach (int t_key in Sales_Cacu.Keys)
            {
                if (Sales_Cacu[t_key].Del_TF != "D")
                    Set_gr_Cacu(ref gr_dic_text, t_key, fi_cnt);  //데이타를 배열에 넣는다.
                fi_cnt++;
            }

            cgb_Cacu.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Cacu.db_grid_Obj_Data_Put();
        }


        private void Set_gr_Cacu(ref Dictionary<int, object[]> gr_dic_text, int t_key, int fi_cnt)
        {
            object[] row0 = { Sales_Cacu[t_key].C_index   
                                ,Sales_Cacu[t_key].C_TF_Name   
                                ,Sales_Cacu[t_key].C_Price1     
                                ,Sales_Cacu[t_key].C_AppDate1    
                                ,Sales_Cacu[t_key].C_CodeName    

                                ,Sales_Cacu[t_key].C_Number1    
                                ,Sales_Cacu[t_key].C_Name1   
                                ,Sales_Cacu[t_key].C_Name2                                 
                                ,Sales_Cacu[t_key].C_Etc           
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Base_Cacu_Header_Reset()
        {
            cgb_Cacu.Grid_Base_Arr_Clear();
            cgb_Cacu.basegrid = dGridView_Base_Cacu;
            cgb_Cacu.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Cacu.grid_col_Count = 10;
            cgb_Cacu.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {""  , "결제방법"   , "결제액"  , "결제일자"   , "카드_은행명"        
                                , "카드_은행번호"   , "카드소유자_입금자"    , ""  , "비고" , ""
                                };

            int[] g_Width = { 0, 90, 70, 90, 100
                                ,120 , 100 , 0 , 150 , 0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            
            cgb_Cacu.grid_col_header_text = g_HeaderText;
            cgb_Cacu.grid_cell_format = gr_dic_cell_format;
            cgb_Cacu.grid_col_w = g_Width;
            cgb_Cacu.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                            
                                   };
            cgb_Cacu.grid_col_Lock = g_ReadOnly;

            cgb_Cacu.basegrid.RowHeadersVisible = false;
        }
        //////Sales_Cacu___Sales_Cacu__Sales_Cacu__Sales_Cacu
        //////Sales_Cacu___Sales_Cacu__Sales_Cacu__Sales_Cacu





        //////Sales_Cacu_R___Sales_Cacu_R__Sales_Cacu_R__Sales_Cacu_R
        //////Sales_Cacu_R___Sales_Cacu_R__Sales_Cacu_R__Sales_Cacu_R
        private void Cacu_R_Grid_Set()
        {
            dGridView_Base_Cacu_R_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Cacu_R.d_Grid_view_Header_Reset();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            int fi_cnt = 0;
            double S_cnt6 = 0; double S_cnt7 = 0; double S_cnt8 = 0; double S_cnt9 = 0;
            foreach (int t_key in Sales_Cacu_R.Keys)
            {
                if (Sales_Cacu_R[t_key].Del_TF != "D")
                {
                    Set_gr_Rece(ref gr_dic_text, t_key, fi_cnt);  //데이타를 배열에 넣는다.

                    if (Sales_Cacu_R[t_key].C_TF == 1) //현금
                        S_cnt6 = S_cnt6 + Sales_Cacu_R[t_key].C_Price1;
                    if (Sales_Cacu_R[t_key].C_TF == 5) //가상계좌
                        S_cnt8 = S_cnt8 + Sales_Cacu_R[t_key].C_Price1;
                    if (Sales_Cacu_R[t_key].C_TF == 3) //카드
                        S_cnt7 = S_cnt7 + Sales_Cacu_R[t_key].C_Price1;
                    if (Sales_Cacu_R[t_key].C_TF == 7) //카드
                        S_cnt9 = S_cnt9 + Sales_Cacu_R[t_key].C_Price1;
                }
                fi_cnt++;
            }

            txt_SumCash.Text = string.Format(cls_app_static_var.str_Currency_Type, S_cnt6);
            txt_SumCard.Text = string.Format(cls_app_static_var.str_Currency_Type, S_cnt7);
            txt_SumBank.Text = string.Format(cls_app_static_var.str_Currency_Type, S_cnt8);
            txt_SumNaver.Text = string.Format(cls_app_static_var.str_Currency_Type, S_cnt9);

            cgb_Cacu_R.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Cacu_R.db_grid_Obj_Data_Put();
        }


        private void Set_gr_Rece(ref Dictionary<int, object[]> gr_dic_text, int t_key, int fi_cnt)
        {
            object[] row0 = { Sales_Cacu_R[t_key].C_index   
                                ,Sales_Cacu_R[t_key].C_TF_Name   
                                ,Sales_Cacu_R[t_key].C_Price1     
                                ,Sales_Cacu_R[t_key].C_AppDate1    
                                ,Sales_Cacu_R[t_key].C_CodeName    

                                ,Sales_Cacu_R[t_key].C_Number1    
                                ,Sales_Cacu_R[t_key].C_Name1   
                                ,Sales_Cacu_R[t_key].C_Name2                                 
                                ,Sales_Cacu_R[t_key].C_Etc           
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Base_Cacu_R_Header_Reset()
        {
            cgb_Cacu_R.Grid_Base_Arr_Clear();
            cgb_Cacu_R.basegrid = dGridView_Base_Cacu_R;
            cgb_Cacu_R.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Cacu_R.grid_col_Count = 10;
            cgb_Cacu_R.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {""  , "결제방법"   , "결제액"  , "결제일자"   , "카드_은행명"        
                                , "카드_은행번호"   , "카드소유자_입금자"    , ""  , "비고" , ""
                                };

            int[] g_Width = { 0, 90, 70, 90, 100
                                ,120 , 100 , 0 , 150 , 0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb_Cacu_R.grid_col_header_text = g_HeaderText;
            cgb_Cacu_R.grid_cell_format = gr_dic_cell_format;
            cgb_Cacu_R.grid_col_w = g_Width;
            cgb_Cacu_R.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                            
                                   };
            cgb_Cacu_R.grid_col_Lock = g_ReadOnly;

            cgb_Cacu_R.basegrid.RowHeadersVisible = false;
        }
        //////Sales_Cacu_R___Sales_Cacu_R__Sales_Cacu_R__Sales_Cacu_R
        //////Sales_Cacu_R___Sales_Cacu_R__Sales_Cacu_R__Sales_Cacu_R



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
            T_R.Key_Enter_13_tb += new Key_13_tb_Event_Handler(T_R_Key_Enter_13_tb);
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);
            T_R.Key_Enter_13_Name += new Key_13_Name_Event_Handler(T_R_Key_Enter_13_Name);
            TextBox tb = (TextBox)sender;

            if ((tb.Tag == null) || (tb.Tag.ToString() == ""))
            {
                //쿼리문상 오류를 잡기 위함.
                if (T_R.Text_KeyChar_Check(e, tb, tb) == false)
                {
                    e.Handled = true;
                    return;
                } // end if   
            }
            else if ((tb.Tag != null) && (tb.Tag.ToString() == "1"))
            {
                //숫자만 입력 가능
                if (T_R.Text_KeyChar_Check(e, tb, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "-"))
            {
                //숫자와  - 만
                if (T_R.Text_KeyChar_Check(e, tb, "-") == false)
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

            else if (tb.Tag.ToString() == "name")  //회원 정보 관련해서 이름 검색을 필요로 하는 텍스트 박스이다.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(tb, e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }
        }

        void T_R_Key_Enter_13_tb(string txt_tag, TextBox tb)
        {
            

            if (tb.Name == "txtPay_R1")
            {
                if (tb.Text != "")
                    tb.Text = string.Format(cls_app_static_var.str_Currency_Type, int.Parse(tb.Text.Replace(",", "").Replace(".", "")));
            }


            if (tb.Name == "txt_Price_3")
            {
                if (tb.Text != "")
                    tb.Text = string.Format(cls_app_static_var.str_Currency_Type, int.Parse(tb.Text.Replace(",", "").Replace(".", "")));

                if (txt_Price_3_2.Text == "")
                    txt_Price_3_2.Text = tb.Text.Trim();

                if (mtxtPriceDate3.Text.Replace ("-","").Trim() == "")
                    mtxtPriceDate3.Text = txtSellDate.Text;
            }

            if (tb.Name == "txt_Price_2")
            {
                if (tb.Text != "")
                    tb.Text = string.Format(cls_app_static_var.str_Currency_Type, int.Parse(tb.Text.Replace(",", "").Replace(".", "")));
                if (mtxtPriceDate2.Text.Replace("-", "").Trim() == "")
                    mtxtPriceDate2.Text = txtSellDate.Text;
            }

            if (tb.Name == "txt_Price_1")
            {
                if (tb.Text != "")
                    tb.Text = string.Format(cls_app_static_var.str_Currency_Type, int.Parse(tb.Text.Replace(",", "").Replace(".", "")));

                if (mtxtPriceDate1.Text.Replace("-", "").Trim() == "")
                    mtxtPriceDate1.Text = txtSellDate.Text;
            }

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

            if (tb.Name == "txtCenter2")
            {
                if (tb.Text.Trim() == "")
                    txtCenter2_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtCenter2_Code);
            }

            if (tb.Name == "txtSellCode")
            {
                if (tb.Text.Trim() == "")
                    txtSellCode_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtSellCode_Code);
            }


            if (tb.Name == "txt_C_Bank")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                {
                    txt_C_Bank_Code.Text = "";
                    txt_C_Bank_Code_2.Text = "";
                    txt_C_Bank_Code_3.Text = "";
                }
                Data_Set_Form_TF = 0;
                //else if (Sw_Tab == 1)
                //{
                //    if (Ncode_dic != null)
                //        Ncode_dic.Clear();
                //    Ncode_dic["BankPenName"] = tb;
                //    Ncode_dic["BankCode"] = txt_C_Bank_Code;
                //    Ncode_dic["BankName"] = txt_C_Bank_Code_2;
                //    Ncode_dic["BankAccountNumber"] = txt_C_Bank_Code_3;
                //    Ncod_Text_Set_Data(tb);
                //}
            }

            if (tb.Name == "txt_C_Card")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                {
                    txt_C_Card_Code.Text = "";
                }
                Data_Set_Form_TF = 0;
                //else if (Sw_Tab == 1)
                //{
                //    if (Ncode_dic != null)
                //        Ncode_dic.Clear();
                //    Ncode_dic["ncode"] = tb;
                //    Ncode_dic["cardname"] = txt_C_Card_Code;
                //    Ncod_Text_Set_Data(tb);
                //}
            }


            if (tb.Name == "txtBank")
            {
                Data_Set_Form_TF = 1;
                //if (tb.Text.Trim() == "")
                //    txtBank.Text = "";
                Data_Set_Form_TF = 0;
            }

        }


        void T_R_Key_Enter_13_Name(string txt_tag, TextBox tb)
        {
            if (txt_tag != "")
            {
                int reCnt = 0;
                cls_Search_DB cds = new cls_Search_DB();
                string Search_Mbid = "";
                reCnt = cds.Member_Name_Search(ref Search_Mbid, txt_tag);

                if (reCnt == 1)
                {
                    if (tb.Name == "txtName")
                    {
                        mtxtMbid.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
                        if (Input_Error_Check(mtxtMbid, "m") == true)
                            Set_Form_Date(mtxtMbid.Text, "m");
                    }    
                }
                else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
                {

                    //cls_app_static_var.Search_Member_Name = txt_tag;
                    frmBase_Member_Search e_f = new frmBase_Member_Search();
                    if (tb.Name == "txtName")
                    {
                        e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                        e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);
                    }

                    e_f.ShowDialog();

                    SendKeys.Send("{TAB}");
                }


            }
            else
                SendKeys.Send("{TAB}");

        }

        void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtName.Text.Trim();
        }           



        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
            if (tb.Name == "txtCenter2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, txtCenter2_Code, "");
                else
                    Ncod_Text_Set_Data(tb, txtCenter2_Code);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtSellCode")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, txtSellCode_Code, "");
                else
                    Ncod_Text_Set_Data(tb, txtSellCode_Code);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_C_Bank")
            {
                if (Ncode_dic != null)
                    Ncode_dic.Clear();
                Ncode_dic["BankPenName"] = tb;
                Ncode_dic["BankCode"] = txt_C_Bank_Code;
                Ncode_dic["BankName"] = txt_C_Bank_Code_2;
                Ncode_dic["BankAccountNumber"] = txt_C_Bank_Code_3;

                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, "");
                else
                    Ncod_Text_Set_Data(tb);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_C_Card")
            {
                if (Ncode_dic != null)
                    Ncode_dic.Clear();
                Ncode_dic["ncode"] = txt_C_Card_Code;
                Ncode_dic["cardname"] = tb;

                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, "");
                else
                    Ncod_Text_Set_Data(tb);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

          
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
                if (tb.Name == "txtCenter2")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtSellCode")
                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", strSql);


                if (tb.Name == "txt_Base_Rec")
                    cgb_Pop.db_grid_Popup_Base(2, "배송사_코드", "배송사", "ncode", "name", strSql);


                if (tb.Name == "txt_Receive_Method")
                    cgb_Pop.db_grid_Popup_Base(2, "배송_코드", "배송_종류", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, strSql);

                if (tb.Name == "txt_C_TF")
                    cgb_Pop.db_grid_Popup_Base(2, "결제_코드", "결제_종류", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, strSql);
                
                if (tb.Name == "txt_ItemCode")
                    cgb_Pop.db_grid_Popup_Base(4, "상품명", "상품코드", "개별단가", "개별PV", "Name", "Ncode", "price2", "price4", strSql);

                             
            }
            else
            {
                if (tb.Name == "txtCenter2")
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

                if (tb.Name == "txtSellCode")
                {
                    string Tsql;
                    Tsql = "Select SellCode ,SellTypeName    ";
                    Tsql = Tsql + " From tbl_SellType (nolock) ";
                    Tsql = Tsql + " Order by SellCode ";

                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", Tsql);
                }


                if (tb.Name == "txt_Base_Rec")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Base_Rec (nolock) ";                    
                    Tsql = Tsql + " Order by Ncode ";
                    
                    cgb_Pop.db_grid_Popup_Base(2, "배송사_코드", "배송사", "ncode", "name", Tsql);
                }


                if (tb.Name == "txt_C_TF")
                {
                    string Tsql;

                    Tsql = "Select M_Detail , " + cls_app_static_var.Base_M_Detail_Ex;
                    Tsql = Tsql + " From tbl_Base_Change_Detail (nolock) ";
                    Tsql = Tsql + " Where M_Detail_S = 'tbl_Sales_Cacu' ";
                    Tsql = Tsql + " Order by M_Detail ";

                    cgb_Pop.db_grid_Popup_Base(2, "결제_코드", "결제_종류", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, Tsql);
                }


                if (tb.Name == "txt_Receive_Method")
                {
                    string Tsql;

                    Tsql = "Select M_Detail , " + cls_app_static_var.Base_M_Detail_Ex;
                    Tsql = Tsql + " From tbl_Base_Change_Detail (nolock) ";
                    Tsql = Tsql + " Where M_Detail_S = 'tbl_Sales_Cacu_R' ";
                    Tsql = Tsql + " Order by M_Detail ";

                    cgb_Pop.db_grid_Popup_Base(2, "배송_코드", "배송_종류", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, Tsql);
                }


                

                if (tb.Name == "txt_ItemCode")
                {
                    string Tsql;
                    Tsql = "Select Name , NCode  ,price2 , price4  ";
                    Tsql = Tsql + " From ufn_Good_Search_01 ('" + cls_User.gid_date_time + "') ";
                    Tsql = Tsql + " Where NCode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%'";

                    cgb_Pop.db_grid_Popup_Base(4, "상품명", "상품코드", "개별단가", "개별PV", "Name", "Ncode", "price2", "price4", Tsql);
                }


            }
        }


        private void Db_Grid_Popup(TextBox tb,  string strSql)
        {
    
            cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            DataGridView Popup_gr = new DataGridView();
            Popup_gr.Name = "Popup_gr";
            this.Controls.Add(Popup_gr);
            cgb_Pop.basegrid = Popup_gr;
            cgb_Pop.Base_fr = this;
            cgb_Pop.Base_text_dic = Ncode_dic;
            cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            cgb_Pop.Base_Location_obj = tb;

            if (strSql != "")
            {
                if (tb.Name == "txt_C_Bank")
                {
                    cgb_Pop.db_grid_Popup_Base(4, "계좌가명", "은행_코드", "은행명", "계좌번호"
                                                , "BankPenName", "BankCode", "BankName", "BankAccountNumber"
                                                , strSql);
                    cgb_Pop.Next_Focus_Control = txt_C_Etc;
                }
                if (tb.Name == "txt_C_Card")
                {
                    cgb_Pop.db_grid_Popup_Base(2, "카드_코드", "카드명"
                                                , "ncode", "CardName"
                                                , strSql);
                    cgb_Pop.Next_Focus_Control = txt_C_Name_3;
                }
            }
            else
            {
                if (tb.Name == "txt_C_Bank")
                {
                    string Tsql;
                    Tsql = "Select BankPenName , BankCode , BankName , BankAccountNumber        ";
                    Tsql = Tsql + " From tbl_BankForCompany ";
                    Tsql = Tsql + " Where BankPenName like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    BankCode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%'";

                    cgb_Pop.db_grid_Popup_Base(4, "계좌가명", "은행_코드", "은행명", "계좌번호"
                                                , "BankPenName", "BankCode", "BankName", "BankAccountNumber"
                                                , Tsql);

                    cgb_Pop.Next_Focus_Control = txt_C_Etc;

                }


                if (tb.Name == "txt_C_Card")
                {
                    string Tsql;
                    Tsql = "Select  Ncode, cardname   ";
                    Tsql = Tsql + " From tbl_Card (nolock) ";
                    Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    cardname like '%" + tb.Text.Trim() + "%')";

                    cgb_Pop.db_grid_Popup_Base(2, "카드_코드", "카드명"
                                                , "ncode", "CardName"
                                                , Tsql);

                    cgb_Pop.Next_Focus_Control = txt_C_Name_3;

                }
            }
        }




        private void Ncod_Text_Set_Data(TextBox tb, TextBox tb1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            if (tb.Name == "txtCenter2")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
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

            if (tb.Name == "txtSellCode")
            {
                Tsql = "Select SellCode ,SellTypeName    ";
                Tsql = Tsql + " From tbl_SellType (nolock) ";
                Tsql = Tsql + " Where SellCode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    SellTypeName like '%" + tb.Text.Trim() + "%'";
            }


            if (tb.Name == "txt_Base_Rec")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Base_Rec (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";                
            }

            if (tb.Name == "txt_C_TF")
            {
                Tsql = "Select M_Detail , " + cls_app_static_var.Base_M_Detail_Ex;
                Tsql = Tsql + " From tbl_Base_Change_Detail (nolock) ";
                Tsql = Tsql + " Where M_Detail_S = 'tbl_Sales_Cacu' ";
                Tsql = Tsql + " And  ( M_Detail like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    " + cls_app_static_var.Base_M_Detail_Ex + " like '%" + tb.Text.Trim() + "%')";
            }



            if (tb.Name == "txt_Receive_Method")
            {
                Tsql = "Select M_Detail , " + cls_app_static_var.Base_M_Detail_Ex ;
                Tsql = Tsql + " From tbl_Base_Change_Detail (nolock) ";
                Tsql = Tsql + " Where M_Detail_S = 'tbl_Sales_Cacu_R' ";
                Tsql = Tsql + " And  ( M_Detail like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    " + cls_app_static_var.Base_M_Detail_Ex + " like '%" + tb.Text.Trim() + "%')";
            }


            if (tb.Name == "txt_ItemCode")
            {
                Tsql = "Select Name , NCode ,price2 ,price4    ";
                Tsql = Tsql + " From ufn_Good_Search_01 ('" + cls_User.gid_date_time + "') ";
                Tsql = Tsql + " Where NCode like '%" + tb.Text.Trim() + "%'";
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


        private void Ncod_Text_Set_Data(TextBox tb)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            if (tb.Name == "txt_C_Bank")
            {
                Tsql = "Select BankPenName , BankCode , BankName , BankAccountNumber        ";
                Tsql = Tsql + " From tbl_BankForCompany ";
                Tsql = Tsql + " Where BankPenName like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankCode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%'";
            }


            if (tb.Name == "txt_C_Card")
            {
                Tsql = "Select  Ncode, cardname   ";
                Tsql = Tsql + " From tbl_Card (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    cardname like '%" + tb.Text.Trim() + "%')";
            }


            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 1)
            {
                int fCnt = 0; 
                foreach (string  t_key in Ncode_dic.Keys)
                {
                    Ncode_dic[t_key].Text = ds.Tables["t_P_table"].Rows[0][fCnt].ToString();
                    fCnt++;
                }
            }

            if ((ReCnt > 1) || (ReCnt == 0)) Db_Grid_Popup(tb, Tsql);            
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

            string Tsql = "";
            Tsql = "Select Mbid , Mbid2, M_Name , Sell_Mem_TF  ";
            Tsql = Tsql + " , LineCnt , N_LineCnt  ";
            Tsql = Tsql + " , LeaveDate , LineUserDate  ";
            Tsql = Tsql + " , Saveid  , Saveid2  ";
            Tsql = Tsql + " , Nominid , Nominid2  ";
            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            //if (Mbid.Length == 0)
            //    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            //else
            //{
            //    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
            //    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            //}

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid.ToString() + "'" ;
            }

            //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

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
            //++++++++++++++++++++++++++++++++            

            return true;
        }




















        private void _From_Data_Clear()
        {
            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();            
            
            //dGridView_Base_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Item.d_Grid_view_Header_Reset();

            //dGridView_Base_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Cacu.d_Grid_view_Header_Reset();

            //dGridView_Base_Cacu_R_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Cacu_R.d_Grid_view_Header_Reset();

            //dGridView_Base_Cacu_R_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Cacu_R_Item.d_Grid_view_Header_Reset();
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<



            txtName.ReadOnly =false ;
            txtName.BackColor = SystemColors.Window;

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtMbid);

            Base_Ord_Clear();

            mtxtSn.Mask = "999999-9999999";
            idx_Mbid = ""; idx_Mbid2 = 0;
            mtxtMbid.Focus();
        }


        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            
            if (bt.Name == "butt_Clear")
            {                
                _From_Data_Clear();                                
            }

            else if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Save_Base_Data(ref Save_Error_Check);

        
         

                if (Save_Error_Check > 0)
                {
                    Base_Ord_Clear();

                    if (SalesDetail != null)
                        SalesDetail.Clear();

                    Set_SalesDetail();  //회원의 주문 관련 주테이블 내역을 클래스에 넣는다.

                    if (SalesDetail != null)
                        Base_Grid_Set();
                }
               

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            else if (bt.Name == "butt_Delete")
            {
                int Delete_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Delete_Base_Data(ref Delete_Error_Check);

                if (Delete_Error_Check > 0)
                {
                    Base_Ord_Clear();

                    if (SalesDetail != null)
                        SalesDetail.Clear();

                    Set_SalesDetail();  //회원의 주문 관련 주테이블 내역을 클래스에 넣는다.

                    if (SalesDetail != null)
                        Base_Grid_Set();
                }

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            
            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }
        }






        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Delete_Base_Data(ref int Delete_Error_Check)
        {
            Delete_Error_Check = 0;

            //주문종류 , 회원, 주문일자 입력 안햇는지 체크
            if (Check_Delete_TextBox_Error() == false) return;

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            cls_Cash_Card_Admin_Cancel cccA = new cls_Cash_Card_Admin_Cancel();

            int ret_C1 = cccA.Cash_Card_Send_Singo_Cancel(txt_OrderNumber_R.Text, mtxtMbid.Text.Trim(), "Cash");
            if (ret_C1 == 1)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("There was a problem canceling the cash receipt. Sales cancellation processing is no longer in progress. Please contact the company.");
                }
                else
                {

                    MessageBox.Show("현금 영수증 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
                }
                return;
            }

            if (ret_C1 == 100)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("There was a problem canceling the cash receipt. Sales cancellation processing is no longer in progress.");
                }
                else
                {
                    MessageBox.Show("현금 영수증 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다.");
                }
                return;
            }



            ret_C1 = 0;
            ret_C1 = cccA.Cash_Card_Send_Singo_Cancel(txt_OrderNumber_R.Text, mtxtMbid.Text.Trim(), "Card", 0, "C");

            if (ret_C1 == 1)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("There was a problem canceling the card authorization details. Sales cancellation processing is no longer in progress. Please contact the company.");
                }
                else
                {
                    MessageBox.Show("카드 승인 내역 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
                }
                return;
            }


            if (ret_C1 == 100)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("There was a problem canceling the card authorization details. Sales cancellation processing is no longer in progress.");
                }
                else
                {
                    MessageBox.Show("카드 승인 내역 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다.");
                }
                return;
            }


            ret_C1 = 0;
            ret_C1 = cccA.Cash_Card_Send_Singo_Cancel(txt_OrderNumber_R.Text, mtxtMbid.Text.Trim(), "Bank");

            if (ret_C1 == 1)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("There was a problem canceling the virtual account. Sales cancellation processing is no longer in progress. Please contact the company.");
                }
                else
                {
                    MessageBox.Show("가상계좌 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
                }
                return;
            }

            if (ret_C1 == 100)
            {
                string m_Sg = "";
                if (cls_User.gid_CountryCode == "TH")
                {
                    m_Sg = "There was a problem canceling the virtual account. Are you sure you want to proceed with canceling the sale?";
                    m_Sg = m_Sg + "\n";
                    m_Sg = m_Sg + "If you continue, you must manually cancel the virtual account.";
                }
                else
                {
                     m_Sg = "가상계좌 취소중에 문제가 발생했습니다. 매출 취소를 계속 진행하시겠습니까?";
                    m_Sg = m_Sg + "\n";
                    m_Sg = m_Sg + "계속 진행시 가상계좌는 메뉴얼로 직접 취소하셔야 합니다.";
                }
                if (MessageBox.Show(m_Sg, "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }



            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {

                cls_Search_DB csd = new cls_Search_DB();

                //수정하기 전에 배열에다가 내역을 받아둔다.
                csd.SalesDetail_Mod_BackUp(txt_OrderNumber_R.Text, "tbl_SalesDetail");


                string StrSql = "";
                StrSql = "EXEC Usp_Insert_tbl_Sales_CanCel_Cacu_R '" + txt_OrderNumber_R.Text + "','" + cls_User.gid + "',0";

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);


              

                //주테이블의 변경 내역을 테이블에 넣는다.
                csd.SalesDetail_Mod(Conn, tran, txt_OrderNumber_R.Text, "tbl_SalesDetail");


                tran.Commit();
                Delete_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del"));
            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Err"));

            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }

        }




        private Boolean Check_Delete_TextBox_Error()
        {
            //주문종류 , 회원, 주문일자 입력 안햇는지 체크
            if (Base_Error_Check__01() == false) return false;

            //회원번호 관련 관련 오류 체크 및 존재 여부 그리고 탈퇴 여부(신규 저장일 경우에)                      
            if (Input_Error_Check(mtxtMbid, "m", 0) == false) return false;


            if (txt_OrderNumber_R.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Sell_OrderNumber")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridView_Base.Focus();
                return false;
            }

            string Ord_N = txt_OrderNumber_R.Text.Trim();

            ////////현 내역으로 연관되서 반품이나 교환한 내역이 잇다.
            //////foreach (string t_key in SalesDetail.Keys)
            //////{
            //////    if (SalesDetail[t_key].Del_TF != "D")
            //////    {
            //////        if (SalesDetail[t_key].Re_BaseOrderNumber == Ord_N)
            //////        {
            //////            if (SalesDetail[t_key].ReturnTF == 2)
            //////            {
            //////                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Del_2")
            //////                + "\n" +
            //////                cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //////            }
            //////            if (SalesDetail[t_key].ReturnTF == 3)
            //////            {
            //////                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Del_3")
            //////                + "\n" +
            //////                cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //////            }

            //////            if (SalesDetail[t_key].ReturnTF == 4)
            //////            {
            //////                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Del_4")
            //////                + "\n" +
            //////                cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //////            }
            //////            dGridView_Base.Focus();
            //////            break;
            //////        }
            //////    }
            //////}


            if (SalesDetail[Ord_N].ReturnTF.ToString() == "1")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_01")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtSellDate.Focus(); return false;
            }

            //if (SalesDetail[Ord_N].ReturnTF.ToString() == "2")
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_2")
            //           + "\n" +
            //           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    txtSellDate.Focus(); return false;
            //}

                       
            return true;
        }





        private bool Base_Error_Check__01()
        {
            //회원을 선택 안햇네 그럼 회원 넣어라
            if (txtName.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Mem")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtMbid.Focus(); return false;
            }

            
            //주문일자를 넣었는지 먼저 체크한다. 안넣었으면 넣어라.
            if (txtSellDateRe.Text.Trim() != "")
            {
                int Ret = 0;
                cls_Check_Input_Error c_er = new cls_Check_Input_Error();
                Ret = c_er.Input_Date_Err_Check(txtSellDateRe);

                if (Ret == -1)
                {
                    txtSellDateRe.Focus(); return false;
                }
            }
            else
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_SellDate_Re")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtSellDateRe.Focus(); return false;
            }


            //주문종류를 선택 안햇네 그럼 그것도 넣어라.
            if (txtSellCode_Code.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_SellCode")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtSellCode.Focus(); return false;
            }
            
            return true; 
        }




        private bool Cacu_Error_Check__01()
        {

            if (txtPay_R1.Text.Trim() == "") txtPay_R1.Text = "0";



            //주문일자를 넣었는지 먼저 체크한다. 안넣었으면 넣어라.
            if (mtxtPayDateR1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtPayDateR1.Text, mtxtPayDateR1, "Date") == false)
                {
                    mtxtPayDateR1.Focus();
                    return false;
                }
            }


            if (txtPay_R1.Text == "0")
            {
                if (txtPay_R1.Text.Trim() != "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price_R")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtPay_R1.Focus(); return false;
                }
            }
            
            
            if (txt_C_index_Re.Text == "" && txt_C_index.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Return_Re_002_002")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtPayDateR1.Focus(); return false;
            }

            if ( txtPayDate1.Text == "")
            {

                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("No original sales settlement date");
                }
                else
                {
                    MessageBox.Show("원판매결제일자 없음");
                }
                    mtxtPayDateR1.Focus(); return false;
            }
            //환불 일자가 원판매 결제일자 보다 전이다.
            if (int.Parse(txtPayDate1.Text.Trim().Replace("-", "")) > int.Parse(mtxtPayDateR1.Text.Replace("-", "").Trim()))
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Return_Re_002_001")                       
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtPayDateR1.Focus(); return false;
            }


        

            return true;
        }


        

        private void Base_Ord_Clear()
        {
            dGridView_Base_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Item.d_Grid_view_Header_Reset();

            dGridView_Base_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Cacu.d_Grid_view_Header_Reset();

            dGridView_Base_Cacu_R_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Cacu_R.d_Grid_view_Header_Reset();

        

            if (SalesItemDetail !=null )
                SalesItemDetail.Clear();
            if (Sales_Cacu_R != null)
                Sales_Cacu_R.Clear();
            if (Sales_Cacu != null)
                Sales_Cacu.Clear();

   
            Base_Sub_Clear("Cacu");
            
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(groupBox6, txtSellDate);
            ct.from_control_clear(panel10, txtSellDateRe);            
        }



        private void Base_Sub_Clear(string s_Tf)
        {
            cls_form_Meth ct = new cls_form_Meth();
            

            if (s_Tf == "Cacu")
            {
                dGridView_Base_Cacu_R_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_Cacu_R.d_Grid_view_Header_Reset();              

                if (Sales_Cacu_R != null)
                    Cacu_R_Grid_Set(); //배송 그리드
                
                cls_form_Meth cm = new cls_form_Meth();
                butt_Cacu_Save.Text = cm._chang_base_caption_search("추가");
                ct.from_control_clear(tab_Cacu, mtxtPayDateR1);

                

                if (combo_C_Card_Year.SelectedIndex >= 0)
                    combo_C_Card_Year.SelectedIndex = 0;
                if (combo_C_Card_Month.SelectedIndex >= 0)
                    combo_C_Card_Month.SelectedIndex = 0;
                if (combo_C_Card_Per.SelectedIndex >= 0)
                    combo_C_Card_Per.SelectedIndex = 0;

                ct.from_control_clear(tab_Cacu_Sub, txt_Price_3);
                ct.from_control_clear(tab_Cacu, mtxtPayDateR1);
                
                tab_Cacu.SelectedIndex = 0;
                tab_Cacu_Sub.SelectedIndex = 0;
                txt_C_index.Text = "";
                txt_C_index_Re.Text = "";

                tab_Cacu.Enabled = true;
                enable_Card_info_txt(true);                
                button_Ok.Visible = true;
                button_Cancel.Visible = false;
                

                txtPay_R1.ReadOnly =false ;
                txtPay_R1.BorderStyle = BorderStyle.Fixed3D ;
                txtPay_R1.BackColor = SystemColors.Window;
                butt_Cacu_Del.Visible = false;    
            }


        }

       



        



        


        private void Base_Small_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Ord_Clear")
            {
                Base_Ord_Clear();
            }





            else if (bt.Name == "butt_Cacu_Clear")
            {
                Base_Sub_Clear("Cacu");
            }

            else if (bt.Name == "butt_Cacu_Del")
            {
                if (txt_C_index_Re.Text == "" )
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Return_Re_002_003")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtPayDateR1.Focus(); return ;
                }

                Base_Sub_Delete("Cacu");
                Base_Sub_Sum_Cacu();                
            }


            

            else if (bt.Name == "butt_Cacu_Save")
            {
                if (Base_Error_Check__01() == false) return;  //주문종류 , 회원, 주문일자 입력 안햇는지 체크

                if (tab_Cacu.SelectedIndex == 0) // 환불일 경우에는.
                {
                    if (Cacu_Error_Check__01() == false) return;
                
                    if (txt_C_index_Re.Text != "")
                        Base_Sub_Edit_Cacu();
                    else
                        Base_Sub_Save_Cacu_R();
                }
                else  //결제 내역 등록 수정일 경우
                {
                    if (Item_Rece_Error_Check__02() == false) return;

                    if (txt_C_index_Re.Text == "") //추가 일경우에 새로운 입력
                    {                        
                        if (double.Parse(txt_Price_1.Text.Trim().Replace(",", "")) > 0)  //현금이다
                            Base_Sub_Save_Cacu(1);

                        if (double.Parse(txt_Price_2.Text.Trim().Replace(",", "")) > 0)  //무통장이다
                            Base_Sub_Save_Cacu(2);

                        if (double.Parse(txt_Price_3.Text.Trim().Replace(",", "")) > 0)  //카드이다
                            Base_Sub_Save_Cacu(3);

                        Base_Sub_Clear("Cacu");
                        Base_Sub_Sum_Cacu();

                        ////MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save")
                        ////            + "\n" +
                        ////cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Save"));
                    }
                    else  //
                    {
                        Base_Sub_Edit_Cacu(1);
                        Base_Sub_Clear("Cacu");
                        Base_Sub_Sum_Cacu();

                        ////MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit")
                        ////             + "\n" +
                        ////cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Save"));
                    }
                }

                Base_Sub_Sum_Cacu();
            }   
        }


        private void Base_Sub_Save_Cacu_R()
        {
            cls_form_Meth ct = new cls_form_Meth();

            int New_C_index = 0;
            int Dic_Key = 0;

            if (Sales_Cacu_R != null)
            {
                foreach (int t_key in Sales_Cacu_R.Keys)
                {
                    if (New_C_index < Sales_Cacu_R[t_key].C_index)
                    {
                        New_C_index = t_key;
                    }
                }
            }

            
            Dic_Key = int.Parse(txt_C_index.Text.Trim());
            New_C_index = New_C_index + 1;

            cls_Sell_Cacu t_c_sell = new cls_Sell_Cacu();

            t_c_sell.OrderNumber = txt_OrderNumber_R.Text.Trim();
            t_c_sell.C_index = New_C_index;
            t_c_sell.C_Base_Index  = int.Parse(txt_C_index.Text.Trim());

            t_c_sell.C_TF =  Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_TF ;
            t_c_sell.C_TF_Name = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_TF_Name;

            t_c_sell.C_Price1 = -double.Parse(txtPay_R1.Text.Trim().Replace(",", ""));

            if (t_c_sell.C_TF == 3)
                t_c_sell.C_Price2 = -double.Parse(txtPay_R1.Text.Trim().Replace(",", ""));

            t_c_sell.C_AppDate1 = mtxtPayDateR1.Text.Replace ("-","").Trim();
            t_c_sell.C_AppDate2 = "";
            t_c_sell.C_Etc = txtPayedEtc_R1.Text.Trim();

            t_c_sell.C_CancelDate = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_AppDate1;
            t_c_sell.C_CancelTF = 1;
            t_c_sell.C_CancelPrice = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Price1 ;
            
            t_c_sell.RecordID = cls_User.gid;
            t_c_sell.RecordTime = "";
            

            t_c_sell.C_Code = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Code;
            t_c_sell.C_CodeName = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_CodeName;
            t_c_sell.C_CodeName_2 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_CodeName_2;

            t_c_sell.C_Name1 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Name1;
            t_c_sell.C_Name2 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Name2;
            t_c_sell.C_Number1 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Number1;
            t_c_sell.C_Number2 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Number2;
            t_c_sell.C_Number3 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Number3;
                                  
            t_c_sell.C_Period1 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Period1;
            t_c_sell.C_Period2 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Period2;
            t_c_sell.C_Installment_Period = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Installment_Period;
            
            t_c_sell.Del_TF = "S";
            Sales_Cacu_R[New_C_index] = t_c_sell;

            Base_Sub_Clear("Cacu");

            if (Sales_Cacu_R != null)
                Cacu_R_Grid_Set(); //배송 그리드


            ////MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save")
            ////            + "\n" +
            ////cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Save"));
        }


        private void Base_Sub_Edit_Cacu()
        {
            cls_form_Meth ct = new cls_form_Meth();
            int C_index = int.Parse(txt_C_index_Re.Text);

            Sales_Cacu_R[C_index].C_Price1 = -double.Parse(txtPay_R1.Text.Trim().Replace(",", ""));
            Sales_Cacu_R[C_index].C_AppDate1 = mtxtPayDateR1.Text.Replace("-", "").Trim();
            Sales_Cacu_R[C_index].C_Etc = txtPayedEtc_R1.Text.Trim();

            if (Sales_Cacu_R[C_index].Del_TF == "")
                Sales_Cacu_R[C_index].Del_TF = "U";

            Base_Sub_Clear("Cacu");

            if (Sales_Cacu_R != null)
                Cacu_R_Grid_Set(); //배송 그리드


            ////MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit")
            ////            + "\n" +
            ////cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Save"));
        }



        private void Base_Sub_Edit_Cacu(string TT )
        {
            cls_form_Meth ct = new cls_form_Meth();
            int C_index = int.Parse(txt_C_index.Text);
            Sales_Cacu_R[C_index].C_Etc = txt_C_Etc.Text.Trim();
            Sales_Cacu_R[C_index].C_TF = 0;
            Sales_Cacu_R[C_index].C_TF_Name = "";

            Sales_Cacu_R[C_index].C_Price1 = 0;
            Sales_Cacu_R[C_index].C_AppDate1 = "";
            Sales_Cacu_R[C_index].C_Name1 = "";
            Sales_Cacu_R[C_index].C_Code = "";
            Sales_Cacu_R[C_index].C_CodeName = "";
            Sales_Cacu_R[C_index].C_CodeName_2 = "";
            Sales_Cacu_R[C_index].C_Number1 = "";
            Sales_Cacu_R[C_index].C_Number2 = "";
            Sales_Cacu_R[C_index].C_Number3 = "";
            Sales_Cacu_R[C_index].C_Price2 = 0;
            Sales_Cacu_R[C_index].C_Period1 = "";
            Sales_Cacu_R[C_index].C_Period2 = "";
            Sales_Cacu_R[C_index].C_Installment_Period = "";

            Sales_Cacu_R[C_index].C_B_Number = "";
            Sales_Cacu_R[C_index].C_P_Number = "";
            Sales_Cacu_R[C_index].Sugi_TF = "";

            Sales_Cacu_R[C_index].C_Cash_Send_Nu = "";
            Sales_Cacu_R[C_index].C_Cash_Send_TF = 0;
            Sales_Cacu_R[C_index].C_Cash_Sort_TF = 0;
            Sales_Cacu_R[C_index].C_Cash_Bus_TF = 0;


            if (double.Parse(txt_Price_1.Text.Trim()) > 0)  //현금이다
            {
                Sales_Cacu_R[C_index].C_TF = 1;
                Sales_Cacu_R[C_index].C_TF_Name = ct._chang_base_caption_search("현금");
                Sales_Cacu_R[C_index].C_Price1 = double.Parse(txt_Price_1.Text.Trim().Replace(",", ""));
                Sales_Cacu_R[C_index].C_AppDate1 = mtxtPriceDate1.Text.Replace("-", "").Trim();

                //if (check_Not_Cash.Checked == false)
                //{
                //    if (check_Cash.Checked == true)
                //    {
                //        Sales_Cacu_R[C_index].C_Cash_Send_Nu = txt_C_Cash_Send_Nu.Text.Trim();

                //        if (radioB_C_Cash_Send_TF1.Checked == true)
                //        {
                //            Sales_Cacu_R[C_index].C_Cash_Send_TF = 1;  //개인
                //            Sales_Cacu_R[C_index].C_Cash_Bus_TF = 0; //세금계산서 번호
                //        }
                //        else
                //        {
                //            Sales_Cacu_R[C_index].C_Cash_Send_TF = 2;  //사업자
                //            Sales_Cacu_R[C_index].C_Cash_Bus_TF = 1; //세금계산서 번호
                //        }
                //        Sales_Cacu_R[C_index].C_Cash_Sort_TF = 1;

                //    }
                //    else
                //    {
                //        Sales_Cacu_R[C_index].C_Cash_Send_Nu = txt_C_Cash_Send_Nu.Text.Trim();
                //        Sales_Cacu_R[C_index].C_Cash_Send_TF = 0;
                //        Sales_Cacu_R[C_index].C_Cash_Sort_TF = 2;  //전자세금 계산서
                //        Sales_Cacu_R[C_index].C_Cash_Bus_TF = 1;
                //    }
                //}
                //else
                //{
                //    Sales_Cacu_R[C_index].C_Cash_Send_Nu = "";
                //    Sales_Cacu_R[C_index].C_Cash_Send_TF = -1;
                //    Sales_Cacu_R[C_index].C_Cash_Sort_TF = -1;  //전자세금 계산서
                //    Sales_Cacu_R[C_index].C_Cash_Bus_TF = -1;
                //}
                //if (check_Cash.Checked == true)
                //{
                //    Sales_Cacu_R[C_index].C_Cash_Send_Nu = txt_C_Cash_Send_Nu.Text.Trim();

                //    if (radioB_C_Cash_Send_TF1.Checked == true)
                //        Sales_Cacu_R[C_index].C_Cash_Send_TF = 1;
                //    else
                //        Sales_Cacu_R[C_index].C_Cash_Send_TF = 2;

                //    Sales_Cacu_R[C_index].C_Cash_Sort_TF = 1;
                //}
                //else
                //    Sales_Cacu_R[C_index].C_Cash_Sort_TF = 2;
            }

            if (double.Parse(txt_Price_2.Text.Trim()) > 0)  //무통이다
            {
                Sales_Cacu_R[C_index].C_TF = 2;
                Sales_Cacu_R[C_index].C_TF_Name = ct._chang_base_caption_search("무통장");
                Sales_Cacu_R[C_index].C_Price1 = double.Parse(txt_Price_2.Text.Trim().Replace(",", ""));
                Sales_Cacu_R[C_index].C_AppDate1 = mtxtPriceDate2.Text.Replace("-", "").Trim();
                Sales_Cacu_R[C_index].C_Name1 = txt_C_Name_2.Text.Trim();
                Sales_Cacu_R[C_index].C_Code = txt_C_Bank_Code.Text.Trim();
                Sales_Cacu_R[C_index].C_CodeName = txt_C_Bank_Code_2.Text.Trim();
                Sales_Cacu_R[C_index].C_CodeName_2 = txt_C_Bank.Text.Trim();
                Sales_Cacu_R[C_index].C_Number1 = txt_C_Bank_Code_3.Text.Trim();
            }

            if (double.Parse(txt_Price_3.Text.Trim()) > 0)  //카드이다
            {
                Sales_Cacu_R[C_index].C_TF = 3;
                Sales_Cacu_R[C_index].C_TF_Name = ct._chang_base_caption_search("카드");
                Sales_Cacu_R[C_index].C_Price1 = double.Parse(txt_Price_3.Text.Trim().Replace(",", ""));
                Sales_Cacu_R[C_index].C_AppDate1 = mtxtPriceDate3.Text.Replace("-", "").Trim();
                Sales_Cacu_R[C_index].C_Name1 = txt_C_Name_3.Text.Trim();
                Sales_Cacu_R[C_index].C_Code = txt_C_Card_Code.Text.Trim();
                Sales_Cacu_R[C_index].C_CodeName = txt_C_Card.Text.Trim();
                Sales_Cacu_R[C_index].C_Number1 = txt_C_Card_Number.Text.Trim();
                Sales_Cacu_R[C_index].C_Number2 = txt_C_Card_Ap_Num.Text.Trim();
                Sales_Cacu_R[C_index].C_Price2 = double.Parse(txt_Price_3_2.Text.Trim());
                Sales_Cacu_R[C_index].C_Period1 = combo_C_Card_Year.Text.Trim();
                Sales_Cacu_R[C_index].C_Period2 = combo_C_Card_Month.Text.Trim();
                Sales_Cacu_R[C_index].C_Installment_Period = combo_C_Card_Per.Text.Trim();

                Sales_Cacu_R[C_index].C_B_Number = txt_C_B_Number.Text.Trim();
                Sales_Cacu_R[C_index].C_P_Number = txt_C_P_Number.Text.Trim();
                Sales_Cacu_R[C_index].Sugi_TF = txt_Sugi_TF.Text.Trim();

            }

            

            if (Sales_Cacu_R[C_index].Del_TF == "")
                Sales_Cacu_R[C_index].Del_TF = "U";
        }




        private void Base_Sub_Save_Cacu(int C_SF)
        {
            cls_form_Meth ct = new cls_form_Meth();
            int New_C_index = 0;
            if (Sales_Cacu_R != null)
            {
                foreach (int t_key in Sales_Cacu_R.Keys)
                {
                    if (New_C_index < t_key)
                        New_C_index = t_key;
                }
            }
            New_C_index = New_C_index + 1;

            cls_Sell_Cacu t_c_sell = new cls_Sell_Cacu();

            t_c_sell.OrderNumber = txt_OrderNumber.Text.Trim();
            t_c_sell.C_index = New_C_index;

            t_c_sell.C_Price1 = 0;
            t_c_sell.C_AppDate1 = "";
            t_c_sell.C_AppDate2 = "";
            t_c_sell.C_Name1 = "";
            t_c_sell.C_Code = "";
            t_c_sell.C_CodeName = "";
            t_c_sell.C_CodeName_2 = "";
            t_c_sell.C_Number1 = "";
            t_c_sell.C_Number2 = "";
            t_c_sell.C_Price2 = 0;
            t_c_sell.C_Period1 = "";
            t_c_sell.C_Period2 = "";
            t_c_sell.C_Installment_Period = "";


            if (C_SF == 1)
            {
                t_c_sell.C_TF = 1;
                t_c_sell.C_TF_Name = ct._chang_base_caption_search("현금");
                t_c_sell.C_Price1 = double.Parse(txt_Price_1.Text.Trim().Replace(",", ""));
                t_c_sell.C_AppDate1 = mtxtPriceDate1.Text.Replace("-","").Trim();
            }

            if (C_SF == 2)
            {
                t_c_sell.C_TF = 2;
                t_c_sell.C_TF_Name = ct._chang_base_caption_search("무통장");

                t_c_sell.C_Price1 = double.Parse(txt_Price_2.Text.Trim().Replace(",", ""));
                t_c_sell.C_AppDate1 = mtxtPriceDate2.Text.Replace("-", "").Trim();
                t_c_sell.C_Name1 = txt_C_Name_2.Text.Trim();
                t_c_sell.C_Code = txt_C_Bank_Code.Text.Trim();
                t_c_sell.C_CodeName = txt_C_Bank_Code_2.Text.Trim();
                t_c_sell.C_CodeName_2 = txt_C_Bank.Text.Trim();
                t_c_sell.C_Number1 = txt_C_Bank_Code_3.Text.Trim();
            }


            if (C_SF == 3)
            {
                t_c_sell.C_TF = 3;
                t_c_sell.C_TF_Name = ct._chang_base_caption_search("카드");

                t_c_sell.C_Price1 = double.Parse(txt_Price_3.Text.Trim().Replace(",", ""));
                t_c_sell.C_AppDate1 = mtxtPriceDate3.Text.Replace("-", "").Trim();
                t_c_sell.C_Name1 = txt_C_Name_3.Text.Trim();
                t_c_sell.C_Code = txt_C_Card_Code.Text.Trim();
                t_c_sell.C_CodeName = txt_C_Card.Text.Trim();
                t_c_sell.C_CodeName_2 = "";
                t_c_sell.C_Number1 = txt_C_Card_Number.Text.Trim();
                t_c_sell.C_Number2 = txt_C_Card_Ap_Num.Text.Trim();
                t_c_sell.C_Price2 = double.Parse(txt_Price_3_2.Text.Trim());
                t_c_sell.C_Period1 = combo_C_Card_Year.Text.Trim();
                t_c_sell.C_Period2 = combo_C_Card_Month.Text.Trim();
                t_c_sell.C_Installment_Period = combo_C_Card_Per.Text.Trim();

                t_c_sell.C_B_Number = txt_C_B_Number.Text.Trim();
                t_c_sell.C_P_Number = txt_C_P_Number.Text.Trim();
                t_c_sell.Sugi_TF = txt_Sugi_TF.Text.Trim();
            }

            t_c_sell.RecordID = cls_User.gid;
            t_c_sell.RecordTime = "";

            t_c_sell.C_Etc = txt_C_Etc.Text.Trim();


            t_c_sell.Del_TF = "S";
            Sales_Cacu_R[New_C_index] = t_c_sell;
        }




        private void Base_Sub_Edit_Cacu(int S_TF)
        {
            cls_form_Meth ct = new cls_form_Meth();
            int C_index = int.Parse(txt_C_index_Re.Text);
            Sales_Cacu_R[C_index].C_Etc = txt_C_Etc.Text.Trim();
            Sales_Cacu_R[C_index].C_TF = 0;
            Sales_Cacu_R[C_index].C_TF_Name = "";

            Sales_Cacu_R[C_index].C_Price1 = 0;
            Sales_Cacu_R[C_index].C_AppDate1 = "";
            Sales_Cacu_R[C_index].C_AppDate2 = "";
            Sales_Cacu_R[C_index].C_Name1 = "";
            Sales_Cacu_R[C_index].C_Name2 = "";
            Sales_Cacu_R[C_index].C_Code = "";
            Sales_Cacu_R[C_index].C_CodeName = "";
            Sales_Cacu_R[C_index].C_CodeName_2 = "";
            Sales_Cacu_R[C_index].C_Number1 = "";
            Sales_Cacu_R[C_index].C_Number2 = "";
            Sales_Cacu_R[C_index].C_Price2 = 0;
            Sales_Cacu_R[C_index].C_Period1 = "";
            Sales_Cacu_R[C_index].C_Period2 = "";
            Sales_Cacu_R[C_index].C_Installment_Period = "";


            if (double.Parse(txt_Price_1.Text.Trim()) > 0)  //현금이다
            {
                Sales_Cacu_R[C_index].C_TF = 1;
                Sales_Cacu_R[C_index].C_TF_Name = ct._chang_base_caption_search("현금");
                Sales_Cacu_R[C_index].C_Price1 = double.Parse(txt_Price_1.Text.Trim().Replace(",", ""));
                Sales_Cacu_R[C_index].C_AppDate1 = mtxtPriceDate1.Text.Replace("-", "").Trim();
            }

            if (double.Parse(txt_Price_2.Text.Trim()) > 0)  //무통이다
            {
                Sales_Cacu_R[C_index].C_TF = 2;
                Sales_Cacu_R[C_index].C_TF_Name = ct._chang_base_caption_search("무통장");
                Sales_Cacu_R[C_index].C_Price1 = double.Parse(txt_Price_2.Text.Trim().Replace(",", ""));
                Sales_Cacu_R[C_index].C_AppDate1 = mtxtPriceDate2.Text.Replace("-", "").Trim();
                Sales_Cacu_R[C_index].C_Name1 = txt_C_Name_2.Text.Trim();
                Sales_Cacu_R[C_index].C_Code = txt_C_Bank_Code.Text.Trim();
                Sales_Cacu_R[C_index].C_CodeName = txt_C_Bank_Code_2.Text.Trim();
                Sales_Cacu_R[C_index].C_CodeName_2 = txt_C_Bank.Text.Trim();
                Sales_Cacu_R[C_index].C_Number1 = txt_C_Bank_Code_3.Text.Trim();
            }

            if (double.Parse(txt_Price_3.Text.Trim()) > 0)  //카드이다
            {
                Sales_Cacu_R[C_index].C_TF = 3;
                Sales_Cacu_R[C_index].C_TF_Name = ct._chang_base_caption_search("카드");
                Sales_Cacu_R[C_index].C_Price1 = double.Parse(txt_Price_3.Text.Trim().Replace(",", ""));
                Sales_Cacu_R[C_index].C_AppDate1 = mtxtPriceDate3.Text.Replace("-", "").Trim();
                Sales_Cacu_R[C_index].C_Name1 = txt_C_Name_3.Text.Trim();
                Sales_Cacu_R[C_index].C_Code = txt_C_Card_Code.Text.Trim();
                Sales_Cacu_R[C_index].C_CodeName = txt_C_Card.Text.Trim();
                Sales_Cacu_R[C_index].C_Number1 = txt_C_Card_Number.Text.Trim();
                Sales_Cacu_R[C_index].C_Number2 = txt_C_Card_Ap_Num.Text.Trim();
                Sales_Cacu_R[C_index].C_Price2 = double.Parse(txt_Price_3_2.Text.Trim());
                Sales_Cacu_R[C_index].C_Period1 = combo_C_Card_Year.Text.Trim();
                Sales_Cacu_R[C_index].C_Period2 = combo_C_Card_Month.Text.Trim();
                Sales_Cacu_R[C_index].C_Installment_Period = combo_C_Card_Per.Text.Trim();

                Sales_Cacu_R[C_index].C_B_Number = txt_C_B_Number.Text.Trim();
                Sales_Cacu_R[C_index].C_P_Number = txt_C_P_Number.Text.Trim();
                Sales_Cacu_R[C_index].Sugi_TF = txt_Sugi_TF.Text.Trim();
            }

            if (Sales_Cacu_R[C_index].Del_TF == "")
                Sales_Cacu_R[C_index].Del_TF = "U";
        }




        private bool Item_Rece_Error_Check__02()
        {
            

            if (txt_Price_1.Text.Trim() == "") txt_Price_1.Text = "0";
            if (txt_Price_2.Text.Trim() == "") txt_Price_2.Text = "0";
            if (txt_Price_3.Text.Trim() == "") txt_Price_3.Text = "0";

            if (double.Parse(txt_Price_1.Text.Trim().Replace(",", "")) == 0
                    && double.Parse(txt_Price_2.Text.Trim().Replace(",", "")) == 0
                    && double.Parse(txt_Price_3.Text.Trim().Replace(",", "")) == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_Price_1.Focus(); return false;
            }

            //주문일자를 넣었는지 먼저 체크한다. 안넣었으면 넣어라.
            if (mtxtPriceDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtPriceDate1.Text, mtxtPriceDate1, "Date") == false)
                {
                    mtxtPriceDate1.Focus();
                    return false;
                }
            }


            if (mtxtPriceDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtPriceDate2.Text, mtxtPriceDate2, "Date") == false)
                {
                    mtxtPriceDate2.Focus();
                    return false;
                }
            }


            if (mtxtPriceDate3.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtPriceDate3.Text, mtxtPriceDate3, "Date") == false)
                {
                    mtxtPriceDate3.Focus();
                    return false;
                }
            }


            if (double.Parse(txt_Price_1.Text) != 0)
            {
                if (mtxtPriceDate1.Text.Replace("-", "").Trim() == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_AppDate")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtPriceDate1.Focus(); return false;
                }
            }


            if (double.Parse(txt_Price_2.Text) != 0)
            {
                if (mtxtPriceDate2.Text.Replace("-", "").Trim() == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_AppDate")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtPriceDate2.Focus(); return false;
                }
            }

            if (double.Parse(txt_Price_3.Text) != 0)
            {
                if (mtxtPriceDate3.Text.Replace("-", "").Trim() == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_AppDate")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtPriceDate3.Focus(); return false;
                }
            }

            //if (double.Parse(txt_Price_4.Text) != 0)
            //{
            //    if (mtxtPriceDate4.Text.Replace("-", "").Trim() == "")
            //    {
            //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
            //           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_AppDate")
            //          + "\n" +
            //          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //        mtxtPriceDate4.Focus(); return false;
            //    }
            //}


            if (double.Parse(txt_Price_2.Text) == 0)
            {
                if (mtxtPriceDate2.Text.Replace("-", "").Trim() != "" || txt_C_Name_2.Text.Trim() != ""
                    || txt_C_Bank.Text.Trim() != "" || txt_C_Bank_Code.Text.Trim() != "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price_2")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Price_2.Focus(); return false;
                }
            }



            if (txt_Price_1.Text == "0")
            {
                if (mtxtPriceDate1.Text.Replace("-","").Trim() != "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price_1")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Price_1.Focus(); return false;
                }
            }


            if (txt_Price_2.Text == "0")
            {
                if (mtxtPriceDate2.Text.Replace("-", "").Trim() != "" || txt_C_Name_2.Text.Trim() != ""
                    || txt_C_Bank.Text.Trim() != "" || txt_C_Bank_Code.Text.Trim() != "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price_2")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Price_2.Focus(); return false;
                }
            }

            if (txt_Price_3.Text == "0")
            {
                if (mtxtPriceDate3.Text.Replace("-", "").Trim() != "" || txt_C_Name_3.Text.Trim() != ""
                    || txt_C_Card.Text.Trim() != "" || txt_C_Card_Code.Text.Trim() != ""
                    || txt_C_Card_Number.Text.Trim() != "" || txt_C_Card_Ap_Num.Text.Trim() != ""
                    || txt_Price_3_2.Text.Trim() != "" || combo_C_Card_Year.Text.Trim() != ""
                    || combo_C_Card_Month.Text.Trim() != "" || combo_C_Card_Per.Text.Trim() != ""
                    )
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price_3")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Price_3.Focus(); return false;
                }
            }


            if (txt_C_index_Re.Text != "") // 수정일 경우에는 카드나 현금 무통장 동시에 못넣게 한다.
            {
                if (txt_Price_1.Text != "0" && txt_Price_2.Text != "0")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Input_Same_Not")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price_1_2")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Price_1.Focus(); return false;
                }

                if (txt_Price_1.Text != "0" && txt_Price_3.Text != "0")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Input_Same_Not")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price_1_3")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Price_1.Focus(); return false;
                }

                if (txt_Price_2.Text != "0" && txt_Price_3.Text != "0")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Input_Same_Not")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price_2_3")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Price_2.Focus(); return false;
                }
            }


            if (txt_Price_3_2.Text.Trim() == "") txt_Price_3_2.Text = "0";

            if (double.Parse(txt_Price_3.Text) != double.Parse(txt_Price_3_2.Text))
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Payment amount and authorization amount do not match. Please check and try again.");
                }
                else
                {

                    MessageBox.Show("결제액과 승인 금액이 일치 하지 않습니다. 확인후 다시 시도해 주십시요.");
                }
                txt_Price_3.Focus();
                return false;
            }

            return true;
        }





        private void Base_Sub_Delete(string s_Tf)
        {
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            cls_form_Meth ct = new cls_form_Meth();

            //주문 상품 관련 딕셔너리에서 찾아서.. 삭제 표식을 해놓는다.
            Sales_Cacu_R[int.Parse(txt_C_index_Re.Text)].Del_TF = "D";                        

            Base_Sub_Clear("Cacu");         
         
            if (Sales_Cacu_R != null)
                Cacu_R_Grid_Set(); //배송 그리드
                          
           
            ////MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del")
            ////       + "\n" +
            ////       cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Save"));
        }



        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            cls_form_Meth ct = new cls_form_Meth();
            //ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            //SendKeys.Send("{TAB}");

            if (dtp.Name == "DTP_PriceDate3")
                ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender, txt_C_Card);

            if (dtp.Name == "DTP_PriceDate1")
                ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender, butt_Cacu_Save);

            if (dtp.Name == "DTP_PriceDate2")
                ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender, txt_C_Name_2);
        }

   
  
        
        private Boolean Check_TextBox_Error()
        {
            //주문종류 , 회원, 주문일자 입력 안햇는지 체크
            if (Base_Error_Check__01() == false) return false;                        

            //회원번호 관련 관련 오류 체크 및 존재 여부 그리고 탈퇴 여부(신규 저장일 경우에)                      
            if (Input_Error_Check(mtxtMbid, "m",1) == false) return false;

            if (radioB_Return_7.Checked == true)
            {
                if (double.Parse(txt_TotalPrice_R.Text.Trim())
                    < double.Parse(txt_TotalInputPrice_R.Text.Trim()))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Return_Re_002_004")
                                + "\n" +
                                cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    dGridView_Base.Focus(); return false;
                }


                if (double.Parse(txt_TotalInputPrice.Text.Trim())
                        < -double.Parse(txt_TotalInputPrice_R.Text.Trim()))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Return_Re_002_004")
                             + "\n" +
                             cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    dGridView_Base.Focus(); return false;
                }
            }



            return true;
        }



        private void Update_SalesDetail_dic()
        {
            string OrderNumber = txt_OrderNumber_R.Text.Trim();


            double InputCash = 0; double InputPassbook = 0; double InputCard = 0, InputPassbook2 = 0 , InputMile = 0, InputNaver= 0, InputCoupon = 0;
            double InputPayment_8_TH = 0, InputPayment_9_TH = 0, InputPayment_10_TH = 0;


            foreach (int t_key in Sales_Cacu_R.Keys)
            {
                if (Sales_Cacu_R[t_key].Del_TF != "D")
                {
                    if (Sales_Cacu_R[t_key].C_TF == 1)
                        InputCash = InputCash + Sales_Cacu_R[t_key].C_Price1;

                    if (Sales_Cacu_R[t_key].C_TF == 2)
                        InputPassbook = InputPassbook + Sales_Cacu_R[t_key].C_Price1;

                    if (Sales_Cacu_R[t_key].C_TF == 3)
                        InputCard = InputCard + Sales_Cacu_R[t_key].C_Price1;

                    if (Sales_Cacu_R[t_key].C_TF == 4)
                        InputMile = InputMile + Sales_Cacu_R[t_key].C_Price1;

                    if (Sales_Cacu_R[t_key].C_TF == 5)
                        InputPassbook2 = InputPassbook2 + Sales_Cacu_R[t_key].C_Price1;
                    
                    if (Sales_Cacu_R[t_key].C_TF == 6)
                        InputCoupon = InputCoupon + Sales_Cacu_R[t_key].C_Price1;

                    if (Sales_Cacu_R[t_key].C_TF == 7)
                        InputNaver = InputNaver + Sales_Cacu_R[t_key].C_Price1;

                    if (Sales_Cacu_R[t_key].C_TF == 8)
                        InputPayment_8_TH += Sales_Cacu_R[t_key].C_Price1;

                    if (Sales_Cacu_R[t_key].C_TF == 9)
                        InputPayment_9_TH += Sales_Cacu_R[t_key].C_Price1;

                    if (Sales_Cacu_R[t_key].C_TF == 10)
                        InputPayment_10_TH += Sales_Cacu_R[t_key].C_Price1;
                }
            }

            SalesDetail[OrderNumber].TotalInputPrice = double.Parse(txt_TotalInputPrice_R.Text.Trim().Replace(",", ""));            
            SalesDetail[OrderNumber].InputMile = InputMile;
            SalesDetail[OrderNumber].InputCoupon = InputCoupon;
            SalesDetail[OrderNumber].InputCash = InputCash;
            SalesDetail[OrderNumber].InputCard = InputCard;
            SalesDetail[OrderNumber].InputPassbook = InputPassbook;
            SalesDetail[OrderNumber].InputPassbook_2 = InputPassbook2;
            SalesDetail[OrderNumber].InputNaver = InputNaver;
            SalesDetail[OrderNumber].InputPayment_8_TH = InputPayment_8_TH;
            SalesDetail[OrderNumber].InputPayment_9_TH = InputPayment_9_TH;
            SalesDetail[OrderNumber].InputPayment_10_TH = InputPayment_10_TH;
            SalesDetail[OrderNumber].UnaccMoney = (SalesDetail[OrderNumber].TotalPrice + SalesDetail[OrderNumber].InputPass_Pay) - SalesDetail[OrderNumber].TotalInputPrice; 

            if (SalesDetail[OrderNumber].Del_TF == "")
                SalesDetail[OrderNumber].Del_TF = "U";         
        }


    



        private void DB_Save_tbl_SalesDetail____002(cls_Connect_DB Temp_Connect,
                                             SqlConnection Conn, SqlTransaction tran,  string OrderNumber)
        {
            string StrSql = "";

            cls_Search_DB csd = new cls_Search_DB();

            //수정하기 전에 배열에다가 내역을 받아둔다.
            csd.SalesDetail_Mod_BackUp(OrderNumber, "tbl_SalesDetail");


            StrSql = "Update tbl_SalesDetail Set ";
            StrSql = StrSql + " SellDate = '" + SalesDetail[OrderNumber].SellDate.Replace("-","")   + "'";
            StrSql = StrSql + ",TotalPrice = " + SalesDetail[OrderNumber].TotalPrice ;
            StrSql = StrSql + ",TotalPV= " + SalesDetail[OrderNumber].TotalPV;
            StrSql = StrSql + ",TotalcV= " + SalesDetail[OrderNumber].TotalCV;
            StrSql = StrSql + ",TotalInputPrice= " + SalesDetail[OrderNumber].TotalInputPrice;

            StrSql = StrSql + ",Total_Sell_VAT_Price= " + SalesDetail[OrderNumber].Total_Sell_VAT_Price;
            StrSql = StrSql + ",Total_Sell_Except_VAT_Price= " + SalesDetail[OrderNumber].Total_Sell_Except_VAT_Price;

            StrSql = StrSql + ",InputCash= " + SalesDetail[OrderNumber].InputCash;
            StrSql = StrSql + ",InputCard= " + SalesDetail[OrderNumber].InputCard;
            StrSql = StrSql + ",InputPassbook= " + SalesDetail[OrderNumber].InputPassbook;
            StrSql = StrSql + ",InputPassbook_2= " + SalesDetail[OrderNumber].InputPassbook_2;
            StrSql = StrSql + ",InputMile= " + SalesDetail[OrderNumber].InputMile;
            StrSql = StrSql + ",UnaccMoney= " + SalesDetail[OrderNumber].UnaccMoney;
            StrSql = StrSql + ",InputNaver= " + SalesDetail[OrderNumber].InputNaver;
            StrSql = StrSql + ",InputPayment_8_TH = " + SalesDetail[OrderNumber].InputPayment_8_TH;
            StrSql = StrSql + ",InputPayment_9_TH = " + SalesDetail[OrderNumber].InputPayment_9_TH;
            StrSql = StrSql + ",InputPayment_10_TH = " + SalesDetail[OrderNumber].InputPayment_10_TH;
            StrSql = StrSql + ",Etc1= '" + SalesDetail[OrderNumber].Etc1 + "'";
            StrSql = StrSql + ",Etc2= '" + SalesDetail[OrderNumber].Etc2 + "'";

            StrSql = StrSql + " Where OrderNumber = '" + SalesDetail[OrderNumber].OrderNumber  + "'";

            if (Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name.ToString(), this.Text) == false) return;

            //주테이블의 변경 내역을 테이블에 넣는다.
            csd.SalesDetail_Mod(Conn, tran,OrderNumber, "tbl_SalesDetail");
           
        }



        private void DB_Save_tbl_SalesItemDetail(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, 
                    string OrderNumber)
        {           
            
            foreach (int t_key in SalesItemDetail.Keys)
            {
                if (SalesItemDetail[t_key].Del_TF != "D") 
                {                  
                    DB_Save_tbl_SalesItemDetail____S(Temp_Connect, Conn, tran, OrderNumber, t_key);
                }
            }
        }



        private void DB_Save_tbl_SalesItemDetail____S(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran,
                    string OrderNumber, int SalesItemIndex)
        {
            string StrSql = "";
            
            cls_form_Meth ct = new cls_form_Meth();
            string SellState = "R_1";

            StrSql = "Insert Into tbl_SalesitemDetail (";            
            StrSql = StrSql + " SalesItemIndex,OrderNumber,";
            StrSql = StrSql + " ItemCode,ItemPrice,ItemPv,ItemCv,";
            StrSql = StrSql + " Sell_VAT_TF , Sell_VAT_Price, Sell_Except_VAT_Price,SellState,";
            StrSql = StrSql + " ItemCount,ItemTotalPrice,ItemTotalPV,ItemTotalcV,";
            StrSql = StrSql + " Total_Sell_VAT_Price, Total_Sell_Except_VAT_Price,";
            StrSql = StrSql + " ReturnDate,SendDate,ReturnBackDate,";
            StrSql = StrSql + " Etc,RecIndex,";                    
             StrSql = StrSql + " Send_itemCount1,Send_itemCount2, ";
            StrSql = StrSql + " T_OrderNumber1,T_OrderNumber2,G_Sort_Code ";
            StrSql = StrSql + " ,RecordID,RecordTime ";
            StrSql = StrSql + " ) values("  ;

            StrSql = StrSql +  SalesItemDetail[SalesItemIndex].SalesItemIndex ;
            StrSql = StrSql + ",'" + OrderNumber + "'";

            StrSql = StrSql + ",'" + SalesItemDetail[SalesItemIndex].ItemCode + "'";
            StrSql = StrSql + "," + - SalesItemDetail[SalesItemIndex].ItemPrice;
            StrSql = StrSql + "," + - SalesItemDetail[SalesItemIndex].ItemPV;
            StrSql = StrSql + "," + - SalesItemDetail[SalesItemIndex].ItemCV;
            StrSql = StrSql + "," +  SalesItemDetail[SalesItemIndex].Sell_VAT_TF;
            StrSql = StrSql + "," + - SalesItemDetail[SalesItemIndex].Sell_VAT_Price;
            StrSql = StrSql + "," + - SalesItemDetail[SalesItemIndex].Sell_Except_VAT_Price;

            StrSql = StrSql + ",'" + SellState + "'";

            StrSql = StrSql + "," + - SalesItemDetail[SalesItemIndex].ItemCount;
            StrSql = StrSql + "," + - SalesItemDetail[SalesItemIndex].ItemTotalPrice;
            StrSql = StrSql + "," + - SalesItemDetail[SalesItemIndex].ItemTotalPV;
            StrSql = StrSql + "," + - SalesItemDetail[SalesItemIndex].ItemTotalCV;

            StrSql = StrSql + "," + - SalesItemDetail[SalesItemIndex].Total_Sell_VAT_Price;
            StrSql = StrSql + "," + - SalesItemDetail[SalesItemIndex].Total_Sell_Except_VAT_Price;

            StrSql = StrSql + ",''";
            StrSql = StrSql + ",''";
            StrSql = StrSql + ",''";

            StrSql = StrSql + ",''";
            StrSql = StrSql + ",0";

            StrSql = StrSql + ",0 " ;
            StrSql = StrSql + ",0 " ;

            StrSql = StrSql + ",''";
            StrSql = StrSql + ",''";
            StrSql = StrSql + ",''";
            StrSql = StrSql + ",'" + SalesItemDetail[SalesItemIndex].RecordID + "'";
            StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
            StrSql = StrSql + " ) ";
                        
            if (Temp_Connect.Insert_Data(StrSql,"tbl_SalesItemDetail", Conn, tran, this.Name.ToString(), this.Text) == false) return;
           
        }








        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
            string str_Q = "";

            int fi_cnt = 0; int Up_Cnt = 0;
            foreach (int t_key in Sales_Cacu_R.Keys)
            {
                if (Sales_Cacu_R[t_key].Del_TF != "S")
                    Up_Cnt ++;
                fi_cnt++;
            }
            
            if (Up_Cnt ==0)
                str_Q = "Msg_Base_Save_Q";
            else            
                str_Q = "Msg_Base_Edit_Q";
                            
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString(str_Q), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (Check_TextBox_Error() == false) return;
                      
            Update_SalesDetail_dic();  //판매 주 클래스에 대한 수정 작업

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            string T_ord_N = "";
            cls_Search_DB csd = new cls_Search_DB();
            
            try
            {
             
                T_ord_N = txt_OrderNumber_R.Text.Trim();

                //실질적인 저장,수정이 이루어지는곳. 변경시 주테이블 이전 내역도 같이 저장함
                DB_Save_tbl_SalesDetail____002(Temp_Connect, Conn, tran ,  T_ord_N );


                DB_Save_tbl_Sales_Cacu_R(Temp_Connect, Conn, tran, T_ord_N);


                tran.Commit();

                Save_Error_Check = 1;
                if (Up_Cnt ==0)
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
                else
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
            }
            catch (Exception ee)
            {
                tran.Rollback();
                if (Up_Cnt == 0)
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
                else
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Err"));

                if (cls_User.gid == cls_User.SuperUserID)
                    MessageBox.Show(ee.ToString());

            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }          
        }






        private void DB_Save_tbl_Sales_Cacu_R(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran,
                    string OrderNumber)
        {

            foreach (int t_key in Sales_Cacu_R.Keys)
            {
                if (Sales_Cacu_R[t_key].Del_TF == "D") //삭제이다
                {
                    //백업데이블에 백업 받고 삭제 처리한다.
                    DB_Save_tbl_Sales_Cacu____D(Temp_Connect, Conn, tran, OrderNumber, t_key);
                }
                else if (Sales_Cacu_R[t_key].Del_TF == "U") //업데이트다 
                {
                    DB_Save_tbl_Sales_Cacu____U(Temp_Connect, Conn, tran, OrderNumber, t_key);
                }
                else if (Sales_Cacu_R[t_key].Del_TF == "S")  //새로운 저장이다
                {
                    DB_Save_tbl_Sales_Cacu____S(Temp_Connect, Conn, tran, OrderNumber, t_key);
                }
                cls_Web web = new cls_Web();
                string SuccessYN = "";
                int C_Index = 0;

                //20210427 구현호 현금영수증있으면 취소될수있도록 만들어야한다.
                //if (Sales_Cacu_R[t_key].C_Cash_Number != "" )
                //{
                //    if (Sales_Cacu_R[t_key].C_Cash_Number != null)
                //    {
                //web.Dir_VR_Cash_Receipt_All_Cancel(txt_OrderNumber.Text, t_key);
                //    }
                //}

                // 240315 syhuh - 태국인 경우 현금영수증 처리안하도록 변경.
                if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) != "TH")
                {
                    web.Dir_VR_Cash_Receipt_All_Cancel(txt_OrderNumber.Text, t_key);
                }
                
                if (Sales_Cacu_R[t_key].C_TF == 7)
                {
                    C_Index = int.Parse(Sales_Cacu_R[t_key].C_index.ToString());
                    string ErrMessage = "";
                    SuccessYN = "N";
                    SuccessYN = web.Dir_Naver_Approve_Cancel(txt_OrderNumber.Text, C_Index, ref ErrMessage);
                    if (SuccessYN == "N")
                    {
                        if (cls_User.gid_CountryCode == "TH")
                        {
                            MessageBox.Show("There was a problem canceling Naver Pay.\nPlease check with the computer staff." + Environment.NewLine +
                                "PG Message : " + ErrMessage);
                        }
                        else
                        {

                            MessageBox.Show("네이버페이 취소중 문제가 발생했습니다.\n전산담당자에게 확인 부탁드립니다." + Environment.NewLine +
                                "PG Message : " + ErrMessage);
                        }
                        return;
                    }
                }
            }
        }

        private void DB_Save_tbl_Sales_Cacu____D(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran,
                    string OrderNumber, int C_index)
        {
            string StrSql = "";

            StrSql = "Insert into tbl_Sales_Cacu_Mod_Del  ";
            StrSql = StrSql + " Select * ,0,'" + cls_User.gid + "',Convert(Varchar(25),GetDate(),21) From tbl_Sales_Cacu ";
            StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
            StrSql = StrSql + " And   C_index = " + C_index;

            if (Temp_Connect.Insert_Data(StrSql, "tbl_Sales_Cacu", Conn, tran, this.Name.ToString(), this.Text) == false) return;

            StrSql = "Delete From tbl_Sales_Cacu";
            StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
            StrSql = StrSql + " And   C_index = " + C_index;

            if (Temp_Connect.Delete_Data(StrSql, "tbl_Sales_Cacu", Conn, tran, this.Name.ToString(), this.Text) == false) return;
        }



        private void DB_Save_tbl_Sales_Cacu____U(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran,
                    string OrderNumber, int C_index)
        {
            string StrSql = "";

            cls_Search_DB csd = new cls_Search_DB();
            string T_where = " And C_index = " + C_index.ToString();
            //수정하기 전에 배열에다가 내역을 받아둔다.
            csd.SalesDetail_Mod_BackUp(OrderNumber, "tbl_Sales_Cacu", T_where);

            

            StrSql = "Update tbl_Sales_Cacu Set ";

            StrSql = StrSql + " C_TF= " + Sales_Cacu_R[C_index].C_TF;
            StrSql = StrSql + ",C_Price1= " + Sales_Cacu_R[C_index].C_Price1;
            StrSql = StrSql + ",C_Price2= " + Sales_Cacu_R[C_index].C_Price2;

            StrSql = StrSql + ",C_AppDate1= '" + Sales_Cacu_R[C_index].C_AppDate1.Replace("-","")  + "'";
            StrSql = StrSql + ",C_AppDate2= '" + Sales_Cacu_R[C_index].C_AppDate2.Replace("-", "") + "'";

            StrSql = StrSql + ",C_CodeName= '" + Sales_Cacu_R[C_index].C_CodeName + "'";

            if (Sales_Cacu_R[C_index].C_TF == 3)
                StrSql = StrSql + ",C_Number1= '" + encrypter.Encrypt(Sales_Cacu_R[C_index].C_Number1) + "'";
            else
                StrSql = StrSql + ",C_Number1= '" + Sales_Cacu_R[C_index].C_Number1 + "'";

            //StrSql = StrSql + ",C_Number2= '" + encrypter.Encrypt( Sales_Cacu_R[C_index].C_Number2) + "'";
            //StrSql = StrSql + ",C_Number3= '" + encrypter.Encrypt( Sales_Cacu_R[C_index].C_Number3) + "'";

            StrSql = StrSql + ",C_Name1= '" + Sales_Cacu_R[C_index].C_Name1 + "'";
            StrSql = StrSql + ",C_Name2= '" + Sales_Cacu_R[C_index].C_Name2 + "'";
            
            StrSql = StrSql + ",C_Code= '" + Sales_Cacu_R[C_index].C_Code + "'";
            StrSql = StrSql + ",C_Period1= '" + Sales_Cacu_R[C_index].C_Period1 + "'";
            StrSql = StrSql + ",C_Period2= '" + Sales_Cacu_R[C_index].C_Period2 + "'";
            StrSql = StrSql + ",C_Installment_Period= '" + Sales_Cacu_R[C_index].C_Installment_Period + "'";

            StrSql = StrSql + ",C_Etc= '" + Sales_Cacu_R[C_index].C_Etc + "'";

            StrSql = StrSql + ",Sugi_TF= '" + Sales_Cacu_R[C_index].Sugi_TF + "'";
            StrSql = StrSql + ",C_P_Number= '" + encrypter.Encrypt(Sales_Cacu_R[C_index].C_P_Number) + "'";
            StrSql = StrSql + ",C_B_Number= '" + encrypter.Encrypt(Sales_Cacu_R[C_index].C_B_Number) + "'";

            ////StrSql = StrSql + ",C_CancelTF= " + Sales_Cacu_R[C_index].C_CancelTF;
            ////StrSql = StrSql + ",C_CancelDate= '" + Sales_Cacu_R[C_index].C_CancelDate + "'";
            ////StrSql = StrSql + ",C_CancelPrice= " + Sales_Cacu_R[C_index].C_CancelPrice;

            StrSql = StrSql + " Where OrderNumber = '" + Sales_Cacu_R[C_index].OrderNumber + "'";
            StrSql = StrSql + " And C_index = " + C_index.ToString();

            if (Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name.ToString(), this.Text) == false) return;

            //주문 상품 테이블의 변경 내역을 테이블에 넣는다.
            csd.tbl_SalesDetail_Total_Change(Conn, tran, OrderNumber, C_index, "tbl_Sales_Cacu", T_where);
           
        }


        private void DB_Save_tbl_Sales_Cacu____S(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran,
                    string OrderNumber, int C_index)
        {
            string StrSql = "";


            StrSql = "Insert Into tbl_Sales_Cacu (";
            StrSql = StrSql + " C_index,OrderNumber,";
            StrSql = StrSql + " C_TF,C_Code,C_CodeName,C_Name1,C_Name2,";
            StrSql = StrSql + " C_Number1 , C_Number2, ";
            StrSql = StrSql + " C_Price1,C_Price2,C_AppDate1,C_AppDate2, ";
            //StrSql = StrSql + " C_CancelTF, C_CancelDate,C_CancelPrice, ";
            StrSql = StrSql + " C_Period1,C_Period2,C_Installment_Period,C_Etc";
            StrSql = StrSql + " ,Sugi_TF , C_P_Number , C_B_Number ";
            StrSql = StrSql + " ,RecordID,RecordTime ";
            StrSql = StrSql + " ) values(";

            StrSql = StrSql + "" + Sales_Cacu_R[C_index].C_index;
            StrSql = StrSql + ",'" + OrderNumber + "'";
            StrSql = StrSql + "," + Sales_Cacu_R[C_index].C_TF;

            StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].C_Code + "'";
            StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].C_CodeName + "'";
            StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].C_Name1 + "'";
            StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].C_Name2 + "'";

            if (Sales_Cacu_R[C_index].C_TF == 3)
                StrSql = StrSql + ",'" + encrypter.Encrypt(Sales_Cacu_R[C_index].C_Number1) + "'";
            else
                StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].C_Number1 + "'";

            StrSql = StrSql + ",'" + encrypter.Encrypt(Sales_Cacu_R[C_index].C_Number2) + "'";
            

            StrSql = StrSql + "," + Sales_Cacu_R[C_index].C_Price1;
            StrSql = StrSql + "," + Sales_Cacu_R[C_index].C_Price2;

            StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].C_AppDate1.Replace("-", "") + "'";
            StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].C_AppDate2.Replace("-", "") + "'";

            //StrSql = StrSql + "," + Sales_Cacu_R[C_index].C_CancelTF;
            //StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].C_CancelDate + "'";
            //StrSql = StrSql + "," + Sales_Cacu_R[C_index].C_CancelPrice;

            StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].C_Period1 + "'";
            StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].C_Period2 + "'";
            StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].C_Installment_Period + "'";
            StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].C_Etc + "'";

            StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].Sugi_TF + "'";
            StrSql = StrSql + ",'" + encrypter.Encrypt(Sales_Cacu_R[C_index].C_P_Number) + "'";
            StrSql = StrSql + ",'" + encrypter.Encrypt(Sales_Cacu_R[C_index].C_B_Number) + "'";

            StrSql = StrSql + ",'" + Sales_Cacu_R[C_index].RecordID + "'";

            StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
            StrSql = StrSql + " ) ";

            if (Temp_Connect.Insert_Data(StrSql, "tbl_Sales_Cacu", Conn, tran, this.Name.ToString(), this.Text) == false) return;

        }


        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            Base_Ord_Clear();

            if ((sender as DataGridView).CurrentRow.Cells[0].Value != null)
            {
                string OrderNumber = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString();

                if (OrderNumber != "")
                {
                    Set_SalesDetail(OrderNumber);

                    if (SalesItemDetail != null)
                        SalesItemDetail.Clear();

                    if (Sales_Cacu_R != null)
                        Sales_Cacu_R.Clear();

                    if (Sales_Cacu != null)
                        Sales_Cacu.Clear();

                    Set_SalesItemDetail(OrderNumber);  //상품 
                    Set_Sales_Cacu(txt_OrderNumber.Text.Trim());  // 결제 원주문의 결제 정보
                    Set_Sales_Cacu_R(OrderNumber);  // 환불의 결제 정보 

                    Item_Grid_Set(); //상품 그리드
                    Cacu_Grid_Set(); //원 주문 결제 그리드
                    Cacu_R_Grid_Set(); //환불 정보 그리드
                }
            }
        }


        private void Set_SalesDetail(string OrderNumber)
        {
            int idx_ReturnTF = SalesDetail[OrderNumber].ReturnTF;

            Data_Set_Form_TF = 1;
                        
            string Re_BaseOrderNumber = SalesDetail[OrderNumber].Re_BaseOrderNumber.Trim();
            txtSellDate.Text = SalesDetail[Re_BaseOrderNumber].SellDate.Replace("-", "");
            txtSellCode.Text = SalesDetail[Re_BaseOrderNumber].SellCodeName;
            txtSellCode_Code.Text = SalesDetail[Re_BaseOrderNumber].SellCode;
            txtCenter2.Text = SalesDetail[Re_BaseOrderNumber].BusCodeName;
            txtCenter2_Code.Text = SalesDetail[Re_BaseOrderNumber].BusCode;                
            txt_OrderNumber.Text = Re_BaseOrderNumber;
            txt_TotalPrice.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[Re_BaseOrderNumber].TotalPrice);
            txt_TotalPv.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[Re_BaseOrderNumber].TotalPV);
            txt_TotalBv.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[Re_BaseOrderNumber].TotalCV);

            txt_TotalInputPrice.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[Re_BaseOrderNumber].TotalInputPrice);
            txt_UnaccMoney.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[Re_BaseOrderNumber].UnaccMoney);

            txt_ETC1.Text = SalesDetail[Re_BaseOrderNumber].Etc1;
            txt_ETC2.Text = SalesDetail[Re_BaseOrderNumber].Etc2;
                

            txtSellDateRe.Text = SalesDetail[OrderNumber].SellDate.Replace("-", "");
            txt_OrderNumber_R.Text = OrderNumber;
            txt_TotalPrice_R.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalPrice);
            txt_TotalPv_R.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalPV);

            txt_TotalInputPrice_R.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalInputPrice);
            //txt_UnaccMoney.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].UnaccMoney);

            txt_ETC1_R.Text = SalesDetail[OrderNumber].Etc1;
            txt_ETC2_R.Text = SalesDetail[OrderNumber].Etc2;
            

            Data_Set_Form_TF = 0;
        }




        private void Set_SalesItemDetail(string OrderNumber )
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
            strSql = strSql + " Where tbl_SalesitemDetail.OrderNumber = '" + OrderNumber.ToString() +"'" ;
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
            
            SalesItemDetail  = T_SalesitemDetail;
        }



        private void Set_Sales_Cacu_R(string OrderNumber)
        {

            string strSql = "";

            strSql = "Select tbl_Sales_Cacu.* ";
            strSql = strSql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " C_TF_Name ";
            strSql = strSql + " , Isnull(tbl_BankForCompany.BankPenName , '')  C_CodeName_2 ";
            strSql = strSql + " From tbl_Sales_Cacu (nolock) ";
            strSql = strSql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber ";
            strSql = strSql + " LEFT JOIN tbl_BankForCompany (nolock) ON tbl_Sales_Cacu.C_Code = tbl_BankForCompany.BankCode And  tbl_Sales_Cacu.C_Number1 = tbl_BankForCompany.BankAccountNumber  And tbl_SalesDetail.Na_Code = tbl_BankForCompany.Na_Code ";
            strSql = strSql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_Sales_Cacu' And  Ch_T.M_Detail = Convert(Varchar,tbl_Sales_Cacu.C_TF) ";
            strSql = strSql + " Where tbl_Sales_Cacu.OrderNumber = '" + OrderNumber.ToString() + "'";
            strSql = strSql + " Order By C_index ASC ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            cls_form_Meth cm = new cls_form_Meth();

            Dictionary<int, cls_Sell_Cacu> T_Sales_Cacu = new Dictionary<int, cls_Sell_Cacu>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cls_Sell_Cacu t_c_sell = new cls_Sell_Cacu();

                t_c_sell.OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                t_c_sell.C_index = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_index"].ToString());

                t_c_sell.C_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_TF"].ToString());
                t_c_sell.C_TF_Name = ds.Tables[base_db_name].Rows[fi_cnt]["C_TF_Name"].ToString();

                t_c_sell.C_Code = ds.Tables[base_db_name].Rows[fi_cnt]["C_Code"].ToString();
                t_c_sell.C_CodeName = ds.Tables[base_db_name].Rows[fi_cnt]["C_CodeName"].ToString();
                t_c_sell.C_CodeName_2 = ds.Tables[base_db_name].Rows[fi_cnt]["C_CodeName_2"].ToString();

                t_c_sell.C_Name1 = ds.Tables[base_db_name].Rows[fi_cnt]["C_Name1"].ToString();
                t_c_sell.C_Name2 = ds.Tables[base_db_name].Rows[fi_cnt]["C_Name2"].ToString();
                t_c_sell.C_Number1 = encrypter.Decrypt(  ds.Tables[base_db_name].Rows[fi_cnt]["C_Number1"].ToString());
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

                t_c_sell.C_Base_Index = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_Base_Index"].ToString());

                t_c_sell.Sugi_TF = ds.Tables[base_db_name].Rows[fi_cnt]["Sugi_TF"].ToString();
                t_c_sell.C_P_Number = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["C_P_Number"].ToString());
                t_c_sell.C_B_Number = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["C_B_Number"].ToString());

                t_c_sell.RecordID = ds.Tables[base_db_name].Rows[fi_cnt]["RecordID"].ToString();
                t_c_sell.RecordTime = ds.Tables[base_db_name].Rows[fi_cnt]["RecordTime"].ToString();

                string t_sellDate = t_c_sell.C_AppDate1.Substring(0, 4);
                t_sellDate = t_sellDate + "-" + t_c_sell.C_AppDate1.Substring(4, 2);
                t_sellDate = t_sellDate + "-" + t_c_sell.C_AppDate1.Substring(6, 2);

                t_c_sell.C_AppDate1 = t_sellDate;

                if (t_c_sell.C_AppDate2 != "")
                {
                    t_sellDate = t_c_sell.C_AppDate2.Substring(0, 4);
                    t_sellDate = t_sellDate + "-" + t_c_sell.C_AppDate2.Substring(4, 2);
                    t_sellDate = t_sellDate + "-" + t_c_sell.C_AppDate2.Substring(6, 2);

                    t_c_sell.C_AppDate2 = t_sellDate;
                }




                t_c_sell.Del_TF = "";
                T_Sales_Cacu[t_c_sell.C_index] = t_c_sell;
            }            

            Sales_Cacu_R = T_Sales_Cacu;
        }




        private void Set_Sales_Cacu(string OrderNumber)
        {

            string strSql = "";

            strSql = "Select tbl_Sales_Cacu.* ";
            strSql = strSql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " C_TF_Name ";
            strSql = strSql + " , Isnull(tbl_BankForCompany.BankPenName , '')  C_CodeName_2 ";
            strSql = strSql + " From tbl_Sales_Cacu (nolock) ";
            strSql = strSql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber ";
            strSql = strSql + " LEFT JOIN tbl_BankForCompany (nolock) ON tbl_Sales_Cacu.C_Code = tbl_BankForCompany.BankCode And  tbl_Sales_Cacu.C_Number1 = tbl_BankForCompany.BankAccountNumber  And tbl_SalesDetail.Na_Code = tbl_BankForCompany.Na_Code  ";
            strSql = strSql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_Sales_Cacu' And  Ch_T.M_Detail = Convert(Varchar,tbl_Sales_Cacu.C_TF) ";
            strSql = strSql + " Where tbl_Sales_Cacu.OrderNumber = '" + OrderNumber.ToString() + "'";
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

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cls_Sell_Cacu t_c_sell = new cls_Sell_Cacu();

                t_c_sell.OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
                t_c_sell.C_index = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_index"].ToString());

                t_c_sell.C_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_TF"].ToString());
                t_c_sell.C_TF_Name = ds.Tables[base_db_name].Rows[fi_cnt]["C_TF_Name"].ToString();
                
                t_c_sell.C_Code = ds.Tables[base_db_name].Rows[fi_cnt]["C_Code"].ToString();
                t_c_sell.C_CodeName = ds.Tables[base_db_name].Rows[fi_cnt]["C_CodeName"].ToString();
                t_c_sell.C_CodeName_2 = ds.Tables[base_db_name].Rows[fi_cnt]["C_CodeName_2"].ToString();

                t_c_sell.C_Name1 = ds.Tables[base_db_name].Rows[fi_cnt]["C_Name1"].ToString();
                t_c_sell.C_Name2 = ds.Tables[base_db_name].Rows[fi_cnt]["C_Name2"].ToString();
                t_c_sell.C_Number1 = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["C_Number1"].ToString());
                t_c_sell.C_Number2 = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["C_Number2"].ToString());
                t_c_sell.C_Number3 = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["C_Number3"].ToString());

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

                t_c_sell.C_Base_Index = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_Base_Index"].ToString());

                t_c_sell.RecordID = ds.Tables[base_db_name].Rows[fi_cnt]["RecordID"].ToString();
                t_c_sell.RecordTime = ds.Tables[base_db_name].Rows[fi_cnt]["RecordTime"].ToString();

                string t_sellDate = t_c_sell.C_AppDate1.Substring(0, 4);
                t_sellDate = t_sellDate + "-" + t_c_sell.C_AppDate1.Substring(4, 2);
                t_sellDate = t_sellDate + "-" + t_c_sell.C_AppDate1.Substring(6, 2);

                t_c_sell.C_AppDate1 = t_sellDate;

                if (t_c_sell.C_AppDate2 != "")
                {
                    t_sellDate = t_c_sell.C_AppDate2.Substring(0, 4);
                    t_sellDate = t_sellDate + "-" + t_c_sell.C_AppDate2.Substring(4, 2);
                    t_sellDate = t_sellDate + "-" + t_c_sell.C_AppDate2.Substring(6, 2);

                    t_c_sell.C_AppDate2 = t_sellDate;
                }




                t_c_sell.Del_TF = "";
                T_Sales_Cacu[t_c_sell.C_index] = t_c_sell;
            }

            Sales_Cacu = T_Sales_Cacu;
        }




        private void dGridView_Base_Sub_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.Name == "dGridView_Base_Cacu")
            {
                if (dgv.CurrentRow != null &&  dgv.CurrentRow.Cells[0].Value != null)
                {
                    int T_key = int.Parse(dgv.CurrentRow.Cells[0].Value.ToString());

                    Put_Sub_Date(T_key, dgv.CurrentRow.Cells[0].Value.ToString(), "Cacu");
                }
            }

            if (dgv.CurrentRow != null &&  dgv.Name == "dGridView_Base_Cacu_R")
            {
                if (dgv.CurrentRow.Cells[0].Value != null)
                {
                    cls_form_Meth ct = new cls_form_Meth();

                    

                    if (combo_C_Card_Year.SelectedIndex >= 0)
                        combo_C_Card_Year.SelectedIndex = 0;
                    if (combo_C_Card_Month.SelectedIndex >= 0)
                        combo_C_Card_Month.SelectedIndex = 0;
                    if (combo_C_Card_Per.SelectedIndex >= 0)
                        combo_C_Card_Per.SelectedIndex = 0;

                    ct.from_control_clear(tab_Cacu, txt_Price_1);
                    txt_C_Etc.Text = "";
                    butt_Cacu_Del.Visible = false;
                    butt_Cacu_Save.Text = ct._chang_base_caption_search("추가");
                    tab_Cacu.Enabled = true;

                    enable_Card_info_txt(true);                    
                    button_Ok.Visible = true;
                    button_Cancel.Visible = false;

                    int T_key = int.Parse(dgv.CurrentRow.Cells[0].Value.ToString());

                    Put_Sub_Date(T_key, dgv.CurrentRow.Cells[0].Value.ToString(), "Cacu_R");
                }
            }
        }


        private void Put_Sub_Date(int T_key, string C_index, string t_STF)
        {
            if (t_STF == "Cacu")
            {
                txt_C_index_Re.Text = "";
                txt_C_index.Text = C_index;
                txtPay_R1.Text = "0";

                cls_form_Meth cm = new cls_form_Meth();
                butt_Cacu_Save.Text = cm._chang_base_caption_search("추가");


                int Salesitemindex = int.Parse(txt_C_index.Text);
                txtPay1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu[T_key].C_Price1);
                //txtPay_R1.Text = txtPay1.Text.Trim();


                //2023-11-23 지성경 : 태국에경우 부분반품이 현재 없다.
                //그래서 일단 모두 환불할 수 있게처리한다.
                //차후 황태혁 차장님이 부분환불 api 를 만들어주시면 같다써야함..
                if (cls_User.gid_CountryCode == "TH")
                {
                    txtPay_R1.Text = txtPay1.Text.Trim();
                    //txtPay_R1.ReadOnly = true;
                    txtPay_R1.ReadOnly = false;
                }


                if (Sales_Cacu[T_key].C_TF == 1) radioB_Return_1.Checked = true;
                //if (Sales_Cacu[T_key].C_TF == 2) radioB_Return_2.Checked = true;
                if (Sales_Cacu[T_key].C_TF == 3) radioB_Return_3.Checked = true;
                //if (Sales_Cacu[T_key].C_TF == 4) radioB_Return_4.Checked = true;
                if (Sales_Cacu[T_key].C_TF == 5) radioB_Return_5.Checked = true;

                if (Sales_Cacu[T_key].C_TF == 7) radioB_Return_7.Checked = true;


                txtPayDate1.Text = Sales_Cacu[T_key].C_AppDate1.Replace ("-","") ;
                mtxtPayDateR1.Text = txtSellDateRe.Text.Replace("-", "").Trim();

                txtPayedEtc_R1.Text = "";
                
                butt_Cacu_Del.Visible = false;           
            }


            if (t_STF == "Cacu_R")
            {
                txt_C_index_Re.Text = C_index;
                txt_C_index.Text = "";
                
                cls_form_Meth cm = new cls_form_Meth();
                butt_Cacu_Save.Text = cm._chang_base_caption_search("수정");

                if (Sales_Cacu_R[T_key].C_Price1 < 0)
                {
                    Tab_Chang_TF = 1; Data_Set_Form_TF = 1;
                    tab_Cacu.SelectedIndex = 0;

                    txtPay1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu_R[T_key].C_CancelPrice);
                    txtPay_R1.Text = string.Format(cls_app_static_var.str_Currency_Type, -Sales_Cacu_R[T_key].C_Price1);

                    if (Sales_Cacu_R[T_key].C_TF == 1) radioB_Return_1.Checked = true;
                    //if (Sales_Cacu_R[T_key].C_TF == 2) radioB_Return_2.Checked = true;
                    if (Sales_Cacu_R[T_key].C_TF == 3) radioB_Return_3.Checked = true;
                    //if (Sales_Cacu_R[T_key].C_TF == 4) radioB_Return_4.Checked = true;
                    if (Sales_Cacu_R[T_key].C_TF == 5) radioB_Return_5.Checked = true;

                    if (Sales_Cacu_R[T_key].C_TF == 7) radioB_Return_7.Checked = true;

                    txtPayDate1.Text = Sales_Cacu_R[T_key].C_CancelDate.Replace("-", "");
                    mtxtPayDateR1.Text = Sales_Cacu_R[T_key].C_AppDate1.Replace("-", "");

                    txtPayedEtc_R1.Text = Sales_Cacu_R[T_key].C_Etc;
                    //txt_C_Cash_Numer.Text = Sales_Cacu_R[T_key].C_Cash_Number;
                    Tab_Chang_TF = 0; Data_Set_Form_TF = 0;

                }
                else
                {
                    int t_C_index = int.Parse(C_index);

                    Tab_Chang_TF = 1;  Data_Set_Form_TF = 1;
                    tab_Cacu.SelectedIndex = 1;


                    butt_Cacu_Del.Visible = true;                    
                    butt_Cacu_Save.Text = cm._chang_base_caption_search("수정");

                    txt_C_Etc.Text = Sales_Cacu_R[t_C_index].C_Etc.ToString();
                    //= string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalInputPrice);

                    //if (Sales_Cacu[int.Parse (SalesItemIndex)].C_TF == 5)
                    //{
                    //    return;
                    //}

                    Data_Set_Form_TF = 1;



                    //txt_C_index.Text = SalesItemIndex;


                    //butt_Cacu_Del.Visible = true;
                    //cls_form_Meth cm = new cls_form_Meth();
                    //butt_Cacu_Save.Text = cm._chang_base_caption_search("수정");
                    //int C_index = int.Parse(txt_C_index.Text);

                    //txt_C_Etc.Text = Sales_Cacu[C_index].C_Etc.ToString();
                    //= string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalInputPrice);

                    if (Sales_Cacu_R[t_C_index].C_TF == 1)
                    {
                        txt_Price_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu_R[t_C_index].C_Price1);
                        mtxtPriceDate1.Text = Sales_Cacu_R[t_C_index].C_AppDate1.ToString().Replace("-", "");

                        
                        tab_Cacu_Sub.SelectedIndex = 1;
                    }

                    if (Sales_Cacu_R[t_C_index].C_TF == 2)
                    {
                        txt_Price_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu_R[t_C_index].C_Price1);
                        mtxtPriceDate2.Text = Sales_Cacu_R[t_C_index].C_AppDate1.ToString().Replace("-", "");
                        txt_C_Name_2.Text = Sales_Cacu_R[t_C_index].C_Name1.ToString();
                        txt_C_Bank.Text = Sales_Cacu_R[t_C_index].C_CodeName_2.ToString();
                        txt_C_Bank_Code.Text = Sales_Cacu_R[t_C_index].C_Code.ToString();
                        txt_C_Bank_Code_2.Text = Sales_Cacu_R[t_C_index].C_CodeName.ToString();
                        txt_C_Bank_Code_3.Text = Sales_Cacu_R[t_C_index].C_Number1.ToString();

                        tab_Cacu_Sub.SelectedIndex = 2;
                    }


                    if (Sales_Cacu_R[t_C_index].C_TF == 3)
                    {
                        txt_Price_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu_R[t_C_index].C_Price1);
                        mtxtPriceDate3.Text = Sales_Cacu_R[t_C_index].C_AppDate1.ToString().Replace("-", "");
                        txt_C_Name_3.Text = Sales_Cacu_R[t_C_index].C_Name1.ToString();
                        txt_C_Card.Text = Sales_Cacu_R[t_C_index].C_CodeName.ToString();
                        txt_C_Card_Code.Text = Sales_Cacu_R[t_C_index].C_Code.ToString();
                        txt_C_Card_Number.Text = Sales_Cacu_R[t_C_index].C_Number1.ToString();
                        txt_C_Card_Ap_Num.Text = Sales_Cacu_R[t_C_index].C_Number2.ToString();
                        txt_C_Card_Ap_Num.Text = Sales_Cacu_R[t_C_index].C_Number2.ToString();
                        txt_Price_3_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu_R[t_C_index].C_Price2);
                        combo_C_Card_Year.Text = Sales_Cacu_R[t_C_index].C_Period1.ToString();
                        combo_C_Card_Month.Text = Sales_Cacu_R[t_C_index].C_Period2.ToString();
                        combo_C_Card_Per.Text = Sales_Cacu_R[t_C_index].C_Installment_Period.ToString();

                        txt_C_P_Number.Text = Sales_Cacu_R[t_C_index].C_P_Number.ToString();
                        txt_C_B_Number.Text = Sales_Cacu_R[t_C_index].C_B_Number.ToString();

                        txt_Sugi_TF.Text = Sales_Cacu_R[t_C_index].Sugi_TF.ToString();


                        tab_Cacu_Sub.SelectedIndex = 0;

                        if (Sales_Cacu_R[t_C_index].C_Number3.ToString() != "" && Sales_Cacu_R[t_C_index].C_Number4.ToString() == "" && Sales_Cacu_R[t_C_index].C_Price1 > 0)
                        {
                            butt_Cacu_Del.Visible = false;
                            //tab_Card.Enabled = false;  //카드가 수기특약이나 웹상으로 승인난 내역에 대해서는 취소가 이루어지 않으면.. 수정이나 삭제가 안되게 한다.
                            enable_Card_info_txt(false);

                            button_Ok.Visible = false;
                            button_Cancel.Visible = true;
                        }
                        else
                        {
                            button_Ok.Visible = true;
                            button_Cancel.Visible = false;
                        }
                    }

                    //if (Sales_Cacu_R[t_C_index].C_TF == 1)
                    //{
                    //    txt_Price_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu_R[t_C_index].C_Price1);
                    //    mtxtPriceDate1.Text = Sales_Cacu_R[t_C_index].C_AppDate1.ToString().Replace("-", "");
                    //    tab_Cacu_Sub.SelectedIndex = 1;
                    //}

                    //if (Sales_Cacu_R_R[t_C_index].C_TF == 2)
                    //{
                    //    txt_Price_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu_R_R[t_C_index].C_Price1);
                    //    mtxtPriceDate2.Text = Sales_Cacu_R_R[t_C_index].C_AppDate1.ToString().Replace("-", "");
                    //    txt_C_Name_2.Text = Sales_Cacu_R_R[t_C_index].C_Name1.ToString();
                    //    txt_C_Bank.Text = Sales_Cacu_R_R[t_C_index].C_CodeName_2.ToString();
                    //    txt_C_Bank_Code.Text = Sales_Cacu_R_R[t_C_index].C_Code.ToString();
                    //    txt_C_Bank_Code_2.Text = Sales_Cacu_R_R[t_C_index].C_CodeName.ToString();
                    //    txt_C_Bank_Code_3.Text = Sales_Cacu_R_R[t_C_index].C_Number1.ToString();
                    //    tab_Cacu_Sub.SelectedIndex = 2;
                    //}


                    //if (Sales_Cacu_R_R[t_C_index].C_TF == 3)
                    //{
                    //    txt_Price_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu_R_R[t_C_index].C_Price1);
                    //    mtxtPriceDate3.Text = Sales_Cacu_R_R[t_C_index].C_AppDate1.ToString().Replace("-", "");
                    //    txt_C_Name_3.Text = Sales_Cacu_R_R[t_C_index].C_Name1.ToString();
                    //    txt_C_Card.Text = Sales_Cacu_R_R[t_C_index].C_CodeName.ToString();
                    //    txt_C_Card_Code.Text = Sales_Cacu_R_R[t_C_index].C_Code.ToString();
                    //    txt_C_Card_Number.Text = Sales_Cacu_R_R[t_C_index].C_Number1.ToString();
                    //    txt_C_Card_Ap_Num.Text = Sales_Cacu_R_R[t_C_index].C_Number2.ToString();
                    //    txt_C_Card_Ap_Num.Text = Sales_Cacu_R_R[t_C_index].C_Number2.ToString();
                    //    txt_Price_3_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu_R_R[t_C_index].C_Price2);
                    //    combo_C_Card_Year.Text = Sales_Cacu_R_R[t_C_index].C_Period1.ToString();
                    //    combo_C_Card_Month.Text = Sales_Cacu_R_R[t_C_index].C_Period2.ToString();
                    //    combo_C_Card_Per.Text = Sales_Cacu_R_R[t_C_index].C_Installment_Period.ToString();
                    //    tab_Cacu_Sub.SelectedIndex = 0;
                    //}

                    Tab_Chang_TF = 0; Data_Set_Form_TF = 0;
                    

                }


                txtPay_R1.ReadOnly = true;
                txtPay_R1.BorderStyle = BorderStyle.FixedSingle;
                txtPay_R1.BackColor = cls_app_static_var.txt_Enable_Color;
                butt_Cacu_Del.Visible = true;
            }

        }

        private void tab_Cacu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tab_Chang_TF == 1) return; 

            cls_form_Meth ct = new cls_form_Meth();
            cls_form_Meth cm = new cls_form_Meth();
            butt_Cacu_Save.Text = cm._chang_base_caption_search("추가");
            ct.from_control_clear((Panel)mtxtPayDateR1.Parent, mtxtPayDateR1);

            if (combo_C_Card_Year.SelectedIndex >= 0)
                combo_C_Card_Year.SelectedIndex = 0;
            if (combo_C_Card_Month.SelectedIndex >= 0)
                combo_C_Card_Month.SelectedIndex = 0;
            if (combo_C_Card_Per.SelectedIndex >= 0)
                combo_C_Card_Per.SelectedIndex = 0;

            ct.from_control_clear(tab_Cacu_Sub, txt_Price_3);
                        
            txt_C_index.Text = "";
            txt_C_index_Re.Text = "";
        }

        private void tab_Cacu_Sub_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab_Cacu.SelectedIndex == 0)
                txt_Price_3.Focus();
            if (tab_Cacu.SelectedIndex == 1)
                txt_Price_1.Focus();
            if (tab_Cacu.SelectedIndex == 2)
                txt_Price_2.Focus();
        }







        private void button_Ok_Click(object sender, EventArgs e)
        {
            if (Base_Error_Check__01() == false) return;  //주문종류 , 회원, 주문일자 입력 안햇는지 체크

            if (Item_Rece_Error_Check__02() == false) return;


            if (txt_UnaccMoney.Text == "")
                txt_UnaccMoney.Text = "0";

            double P_r = double.Parse(txt_Price_3.Text.Replace(",", ""));


            //////@ 2015-01-09 박디도 대리랑 통화후 막아 버림.
            ////if (P_r != double.Parse(txt_UnaccMoney.Text.Replace(",", "")))
            ////{
            ////    MessageBox.Show("결제요청 금액이 결제해야할 금액보다 급니다."
            ////      + "\n" +
            ////      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            ////    txt_Price_3.Focus(); return;
            ////}




            if (txt_C_P_Number.MaxLength != txt_C_P_Number.Text.Length)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Please enter the first 2 digits of the card password correctly"
                           + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                else
                {

                    MessageBox.Show("카드 비밀번호 앞 2자리를 올바르게 입력해 주십시요"
                           + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                return;
            }

            if (txt_C_B_Number.Text.Length < 6)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Please enter the correct 6-digit date of birth. EX:April 8, 1972 -> 720408"
                                + "\n" +
                               cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                else
                {
                    MessageBox.Show("생년월일 6자리를 올바르게 입력해 주십시요. EX:1972년 4월 8일  -> 720408"
                           + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                return;
            }



            if (txt_OrderNumber.Text.Trim() != "")
            {
                if (Cacu_Card_Error_Check__01() == false)
                    return;
            }



            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            int C_index = 0;

            if (txt_C_index.Text == "") //추가 일경우에 새로운 입력
            {
                Base_Sub_Save_Cacu(3, ref C_index);
                Base_Sub_Clear("Cacu");
                Base_Sub_Sum_Cacu();                
            }
            else  //
            {
                C_index = int.Parse(txt_C_index.Text);
                txt_Sugi_TF.Text = "1";
                Base_Sub_Edit_Cacu("1");
                Base_Sub_Clear("Cacu");
                Base_Sub_Sum_Cacu();                
            }



            int Save_Error_Check = 0;
            string OrderNumber = "";

            if (txt_OrderNumber_R.Text.Trim() != "")
            {
                OrderNumber = txt_OrderNumber_R.Text.Trim();
                Save_Base_Data(ref Save_Error_Check, ref OrderNumber);
            }
            else
            {
                OrderNumber = "";
                Save_Base_Data(ref Save_Error_Check, ref OrderNumber);
            }


            //매출 저장중이나 수정중에 오류가 발생 되어 있다 그럼 나머지 작업 하지 말고 걍 나가라
            if (Save_Error_Check == 0)
                return;


            cls_Web Cls_Web = new cls_Web();

            string SuccessYN = "";
            string Err_M = "";

            SuccessYN = Cls_Web.Dir_Card_Approve_OK_Err(OrderNumber, C_index, ref Err_M);

            if (SuccessYN != "Y" && Err_M != "")
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Card authorization error : "
                                + "\n" +
                               Err_M);
                }
                else
                {

                    MessageBox.Show("카드승인 오류 : "
                               + "\n" +
                              Err_M);
                }
                //구매 상품 관련 딕셔너리에서 찾아서.. 삭제 표식을 해놓는다.

                Base_Sub_Clear("Cacu");

                if (Sales_Cacu_R != null)
                {
                    Cacu_R_Grid_Set(); //배송 그리드

                    Base_Sub_Clear("Cacu");

                    Base_Sub_Sum_Cacu();
                }
            }
            else
                Save_Error_Check = 1;

            if (Save_Error_Check > 0)
            {
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                string StrSql = "EXEC Usp_Sell_Cacu_ReCul_4 '" + OrderNumber + "'";
                Temp_Connect.Update_Data(StrSql, "", "");
                

                Base_Ord_Clear();

                if (SalesDetail != null)
                    SalesDetail.Clear();

                Set_SalesDetail();  //회원의 주문 관련 주테이블 내역을 클래스에 넣는다.

                if (SalesDetail != null)
                    Base_Grid_Set();

                Put_OrderNumber_SellDate(OrderNumber);
            }

            
            cls_form_Meth ct = new cls_form_Meth();

            combo_C_Card_Year.SelectedIndex = 0;
            txt_C_Etc.Text = "";
            butt_Cacu_Del.Visible = false;
            butt_Cacu_Save.Text = ct._chang_base_caption_search("추가");
            tab_Cacu.Enabled = true;

            enable_Card_info_txt(true);
            
            button_Ok.Visible = true;
            button_Cancel.Visible = false;

            Put_Sub_Date(C_index.ToString(), "Cacu");


            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Work_End"));

            this.Cursor = System.Windows.Forms.Cursors.Default;

        }



        private void button_Cancel_Click(object sender, EventArgs e)
        {
            if (Base_Error_Check__01() == false) return;  //주문종류 , 회원, 주문일자 입력 안햇는지 체크

            if (txt_OrderNumber_R.Text.Trim() != "")
            {
                if (Cacu_Card_Error_Check__01() == false)
                    return;
            }


            int C_index = int.Parse(txt_C_index_Re.Text);


            cls_Web Cls_Web = new cls_Web();
            string SuccessYN = "";
            string ErrMessage = "";
            SuccessYN = Cls_Web.Dir_Card_Approve_Cancel(txt_OrderNumber_R.Text, C_index, ref ErrMessage);

            if (SuccessYN == "N")
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("A problem occurred while canceling the card.\nPlease check with the computer staff." + Environment.NewLine +
                 "PG Message : " + ErrMessage);
                }
                else
                {
                    MessageBox.Show("카드 취소중 문제가 발생했습니다.\n전산담당자에게 확인 부탁드립니다." + Environment.NewLine +
                    "PG Message : " + ErrMessage);
                }
                return;
            }
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string OrderNumber = txt_OrderNumber_R.Text;

            string StrSql = "EXEC Usp_Sell_Cacu_ReCul_4 '" + OrderNumber + "'";
            Temp_Connect.Update_Data(StrSql, "", "");

            System.Threading.Thread.Sleep(2000);

            Base_Ord_Clear();

            if (SalesDetail != null)
                SalesDetail.Clear();

            Set_SalesDetail();  //회원의 주문 관련 주테이블 내역을 클래스에 넣는다.

            if (SalesDetail != null)
                Base_Grid_Set();

            Put_OrderNumber_SellDate(OrderNumber);


            cls_form_Meth ct = new cls_form_Meth();

            combo_C_Card_Year.SelectedIndex = 0;
            txt_C_Etc.Text = "";
            butt_Cacu_Del.Visible = false;
            butt_Cacu_Save.Text = ct._chang_base_caption_search("추가");
            tab_Cacu.Enabled = true;

            enable_Card_info_txt(true);
            
            button_Ok.Visible = true;
            button_Cancel.Visible = false;

            Put_Sub_Date(C_index.ToString(), "Cacu");

            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Work_End"));

        }



        private Boolean Cacu_Card_Error_Check__01()
        {
            //교환이나 부분반품 반품 건에 대해서는 현 화면에서 수정을 못하게함.
            string Tsql = "";
            Tsql = "select ReturnTF from tbl_SalesDetail  (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + txt_OrderNumber.Text.Trim() + "' ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt != 0)
            {
                if (ds.Tables[base_db_name].Rows[0]["ReturnTF"].ToString() == "2")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_2")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    dGridView_Base_Cacu.Focus(); return false;
                }

                if (ds.Tables[base_db_name].Rows[0]["ReturnTF"].ToString() == "3")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_3")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    dGridView_Base_Cacu.Focus(); return false;
                }

                if (ds.Tables[base_db_name].Rows[0]["ReturnTF"].ToString() == "4")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_4")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    dGridView_Base_Cacu.Focus(); return false;
                }

                if (ds.Tables[base_db_name].Rows[0]["ReturnTF"].ToString() == "5")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_5")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    dGridView_Base_Cacu.Focus(); return false;
                }
            }

            return true;
        }











        private void Base_Sub_Save_Cacu(int C_SF, ref int R_C_index)
        {
            cls_form_Meth ct = new cls_form_Meth();
            int New_C_index = 0;
            if (Sales_Cacu_R != null)
            {
                foreach (int t_key in Sales_Cacu_R.Keys)
                {
                    if (New_C_index < t_key)
                        New_C_index = t_key;
                }
            }
            New_C_index = New_C_index + 1;

            R_C_index = New_C_index;

            cls_Sell_Cacu t_c_sell = new cls_Sell_Cacu();

            t_c_sell.OrderNumber = txt_OrderNumber.Text.Trim();
            t_c_sell.C_index = New_C_index;

            t_c_sell.C_Price1 = 0;
            t_c_sell.C_AppDate1 = "";
            t_c_sell.C_AppDate2 = "";
            t_c_sell.C_Name1 = "";
            t_c_sell.C_Code = "";
            t_c_sell.C_CodeName = "";
            t_c_sell.C_CodeName_2 = "";
            t_c_sell.C_Number1 = "";
            t_c_sell.C_Number2 = "";
            t_c_sell.C_Number3 = "";
            t_c_sell.C_Price2 = 0;
            t_c_sell.C_Period1 = "";
            t_c_sell.C_Period2 = "";
            t_c_sell.C_Installment_Period = "";

            t_c_sell.C_B_Number = "";
            t_c_sell.C_P_Number = "";
            t_c_sell.Sugi_TF = "";

            t_c_sell.C_Cash_Send_Nu = "";
            t_c_sell.C_Cash_Send_TF = 0;
            t_c_sell.C_Cash_Sort_TF = 0;
            t_c_sell.C_Cash_Bus_TF = 0;


            if (C_SF == 1)
            {
                t_c_sell.C_TF = 1;
                t_c_sell.C_TF_Name = ct._chang_base_caption_search("현금");
                t_c_sell.C_Price1 = double.Parse(txt_Price_1.Text.Trim().Replace(",", ""));
                t_c_sell.C_AppDate1 = mtxtPriceDate1.Text.Replace("-", "").Trim();

                //if (check_Cash.Checked == true)
                //{
                //    t_c_sell.C_Cash_Send_Nu = txt_C_Cash_Send_Nu.Text.Trim();

                //    if (radioB_C_Cash_Send_TF1.Checked == true)
                //        t_c_sell.C_Cash_Send_TF = 1;
                //    else
                //        t_c_sell.C_Cash_Send_TF = 2;

                //    t_c_sell.C_Cash_Sort_TF = 1;
                //}
                //else
                //    t_c_sell.C_Cash_Sort_TF = 2;
                //if (check_Not_Cash.Checked == false)
                //{
                //    if (check_Cash.Checked == true)
                //    {
                //        t_c_sell.C_Cash_Send_Nu = txt_C_Cash_Send_Nu.Text.Trim();

                //        if (radioB_C_Cash_Send_TF1.Checked == true)
                //        {
                //            t_c_sell.C_Cash_Send_TF = 1;
                //            t_c_sell.C_Cash_Bus_TF = 0;
                //        }
                //        else
                //        {
                //            t_c_sell.C_Cash_Send_TF = 2;
                //            t_c_sell.C_Cash_Bus_TF = 1;
                //        }
                //        t_c_sell.C_Cash_Sort_TF = 1;
                //    }
                //    else
                //    {
                //        t_c_sell.C_Cash_Send_Nu = txt_C_Cash_Send_Nu.Text.Trim();
                //        t_c_sell.C_Cash_Send_TF = 0;
                //        t_c_sell.C_Cash_Sort_TF = 2;
                //        t_c_sell.C_Cash_Bus_TF = 1;

                //    }
                //}
                //else
                //{
                //    t_c_sell.C_Cash_Send_Nu = "";
                //    t_c_sell.C_Cash_Send_TF = -1;
                //    t_c_sell.C_Cash_Sort_TF = -1;
                //    t_c_sell.C_Cash_Bus_TF = -1;
                //}
            }


            if (C_SF == 2)
            {
                t_c_sell.C_TF = 2;
                t_c_sell.C_TF_Name = ct._chang_base_caption_search("무통장");

                t_c_sell.C_Price1 = double.Parse(txt_Price_2.Text.Trim().Replace(",", ""));
                t_c_sell.C_AppDate1 = mtxtPriceDate2.Text.Replace("-", "").Trim();
                t_c_sell.C_Name1 = txt_C_Name_2.Text.Trim();
                t_c_sell.C_Code = txt_C_Bank_Code.Text.Trim();
                t_c_sell.C_CodeName = txt_C_Bank_Code_2.Text.Trim();
                t_c_sell.C_CodeName_2 = txt_C_Bank.Text.Trim();
                t_c_sell.C_Number1 = txt_C_Bank_Code_3.Text.Trim();
            }


            if (C_SF == 3)
            {
                t_c_sell.C_TF = 3;
                t_c_sell.C_TF_Name = ct._chang_base_caption_search("카드");

                t_c_sell.C_Price1 = double.Parse(txt_Price_3.Text.Trim().Replace(",", ""));
                t_c_sell.C_AppDate1 = mtxtPriceDate3.Text.Replace("-", "").Trim();
                t_c_sell.C_Name1 = txt_C_Name_3.Text.Trim();
                t_c_sell.C_Code = txt_C_Card_Code.Text.Trim();
                t_c_sell.C_CodeName = txt_C_Card.Text.Trim();
                t_c_sell.C_CodeName_2 = "";
                t_c_sell.C_Number1 = txt_C_Card_Number.Text.Trim();
                t_c_sell.C_Number2 = txt_C_Card_Ap_Num.Text.Trim();
                t_c_sell.C_Price2 = double.Parse(txt_Price_3_2.Text.Trim());
                t_c_sell.C_Period1 = combo_C_Card_Year.Text.Trim();
                t_c_sell.C_Period2 = combo_C_Card_Month.Text.Trim();
                t_c_sell.C_Installment_Period = combo_C_Card_Per.Text.Trim();

                t_c_sell.C_B_Number = txt_C_B_Number.Text.Trim();
                t_c_sell.C_P_Number = txt_C_P_Number.Text.Trim();
                t_c_sell.Sugi_TF = "1";
            }

            



            t_c_sell.RecordID = cls_User.gid;
            t_c_sell.RecordTime = "";

            t_c_sell.C_Etc = txt_C_Etc.Text.Trim();

            t_c_sell.Del_TF = "S";
            Sales_Cacu_R[New_C_index] = t_c_sell;
        }






        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check, ref string T_orderNumber)
        {
            Save_Error_Check = 0;
            string str_Q = "";

            if (txt_OrderNumber.Text == "")
                str_Q = "Msg_Base_Save_Q";
            else
                str_Q = "Msg_Base_Edit_Q";

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString(str_Q), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (Check_TextBox_Error() == false) return;  //각종 입력 오류를 체크한다.

            Update_SalesDetail_dic();  //판매 주 클래스에 대한 수정 작업

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            string T_ord_N = "";
            cls_Search_DB csd = new cls_Search_DB();

            //try
            //{
                T_ord_N = txt_OrderNumber_R.Text.Trim();

                //실질적인 저장,수정이 이루어지는곳. 변경시 주테이블 이전 내역도 같이 저장함
                DB_Save_tbl_SalesDetail____002(Temp_Connect, Conn, tran, T_ord_N);
                
                DB_Save_tbl_Sales_Cacu_R(Temp_Connect, Conn, tran, T_ord_N);

                if (txt_OrderNumber.Text != "")
                {
                    string StrSql = "Usp_Update_tbl_Sales_Ga_Order '" + T_ord_N + "'";
                    Temp_Connect.Insert_Data(StrSql, "tbl_SalesDetail", Conn, tran);
                }


                tran.Commit();

                Save_Error_Check = 1;
                ////if (txt_OrderNumber.Text == "")
                ////    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
                ////else
                ////    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
            //}
            //catch (Exception)
            //{
            //    tran.Rollback();
            //    if (txt_OrderNumber.Text == "")
            //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
            //    else
            //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Err"));

            //}

            //finally
            //{
            //    tran.Dispose();
            //    Temp_Connect.Close_DB();
            //}
      
        }


        private void Put_OrderNumber_SellDate(string OrderNumber)
        {
            Set_SalesDetail(OrderNumber);

            if (SalesItemDetail != null)
                SalesItemDetail.Clear();

            if (Sales_Cacu_R != null)
                Sales_Cacu_R.Clear();

            if (Sales_Cacu != null)
                Sales_Cacu.Clear();

            Set_SalesItemDetail(OrderNumber);  //상품 
            Set_Sales_Cacu(txt_OrderNumber.Text.Trim());  // 결제 원주문의 결제 정보
            Set_Sales_Cacu_R(OrderNumber);  // 환불의 결제 정보 

            Item_Grid_Set(); //상품 그리드
            Cacu_Grid_Set(); //원 주문 결제 그리드
            Cacu_R_Grid_Set(); //환불 정보 그리드


            //Set_SalesDetail(OrderNumber);

            //tb_Sort_ABC.Enabled = false;

            //if (SalesItemDetail != null)
            //    SalesItemDetail.Clear();

       
            //if (Sales_Cacu != null)
            //    Sales_Cacu.Clear();

            //Set_SalesItemDetail(OrderNumber);  //상품 
            //Set_Sales_Cacu(OrderNumber);  // 결제 
       

            //Item_Grid_Set(); //상품 그리드
            //Cacu_Grid_Set(); //결제 그리드
       


            //dGridView_C_Main_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_C1.d_Grid_view_Header_Reset();

            //dGridView_C_Detail_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_C2.d_Grid_view_Header_Reset();


            //Base_Grid_Set_C1(OrderNumber);
            //Base_Grid_Set_C2(OrderNumber);

            //tabC_1.SelectedIndex = 0;

        }




        private void enable_Card_info_txt(Boolean TF_B)
        {

            tableLayoutPanel7.Enabled = TF_B;
            tableLayoutPanel8.Enabled = TF_B;
            tableLayoutPanel10.Enabled = TF_B;
            tableLayoutPanel12.Enabled = TF_B;
            tableLayoutPanel6.Enabled = TF_B;
            tableLayoutPanel9.Enabled = TF_B;
            tableLayoutPanel2.Enabled = TF_B;
            tableLayoutPanel11.Enabled = TF_B;
            tableLayoutPanel13.Enabled = TF_B;
            tableLayoutPanel30.Enabled = TF_B;
            tableLayoutPanel29.Enabled = TF_B;
        }






        private void Put_Sub_Date(string SalesItemIndex, string t_STF)
        {
                       

            if (t_STF == "Cacu")
            {

                //if (Sales_Cacu_R[int.Parse (SalesItemIndex)].C_TF == 5)
                //{
                //    return;
                //}

                Data_Set_Form_TF = 1;



                txt_C_index.Text = SalesItemIndex;


                butt_Cacu_Del.Visible = true;
                cls_form_Meth cm = new cls_form_Meth();
                butt_Cacu_Save.Text = cm._chang_base_caption_search("수정");
                int C_index = int.Parse(txt_C_index.Text);

                txt_C_Etc.Text = Sales_Cacu_R[C_index].C_Etc.ToString();
                //= string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalInputPrice);

                if (Sales_Cacu_R[C_index].C_TF == 1)
                {
                    txt_Price_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu_R[C_index].C_Price1);
                    mtxtPriceDate1.Text = Sales_Cacu_R[C_index].C_AppDate1.ToString().Replace("-", "");

                    //if (Sales_Cacu_R[C_index].C_Cash_Sort_TF == 1)
                    //{
                    //    check_Cash.Checked = true;

                    //    txt_C_Cash_Send_Nu.Text = Sales_Cacu_R[C_index].C_Cash_Send_Nu;
                    //    txt_C_Cash_Number2.Text = Sales_Cacu_R[C_index].C_Cash_Number;

                    //    if (Sales_Cacu_R[C_index].C_Cash_Send_TF == 1)
                    //        radioB_C_Cash_Send_TF1.Checked = true;

                    //    if (Sales_Cacu_R[C_index].C_Cash_Send_TF == 2)
                    //        radioB_C_Cash_Send_TF2.Checked = true;
                    //}

                    //if (Sales_Cacu_R[C_index].C_Cash_Sort_TF == 2)
                    //{
                    //    check_Cash.Checked = false;

                    //    txt_C_Cash_Send_Nu.Text = Sales_Cacu_R[C_index].C_Cash_Send_Nu;
                    //    txt_C_Cash_Number2.Text = Sales_Cacu_R[C_index].C_Cash_Number;
                    //}

                    //if (Sales_Cacu_R[C_index].C_Cash_Sort_TF == -1)
                    //{
                    //    check_Cash.Checked = false;
                    //    check_Not_Cash.Checked = true;
                    //}

                    //if (txt_C_Cash_Number2.Text.Trim() != "") //현금 영수증 신고 처리가 되었다.. 그러면... 삭제와 수정 버튼을 안보이게 한다.
                    //{
                    //    butt_Cacu_Del.Visible = false;
                    //}

                    //but_Cash_Send2.Visible = true;
                    tab_Cacu.SelectedIndex = 1;
                }

                if (Sales_Cacu_R[C_index].C_TF == 2)
                {
                    txt_Price_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu_R[C_index].C_Price1);
                    mtxtPriceDate2.Text = Sales_Cacu_R[C_index].C_AppDate1.ToString().Replace("-", "");
                    txt_C_Name_2.Text = Sales_Cacu_R[C_index].C_Name1.ToString();
                    txt_C_Bank.Text = Sales_Cacu_R[C_index].C_CodeName_2.ToString();
                    txt_C_Bank_Code.Text = Sales_Cacu_R[C_index].C_Code.ToString();
                    txt_C_Bank_Code_2.Text = Sales_Cacu_R[C_index].C_CodeName.ToString();
                    txt_C_Bank_Code_3.Text = Sales_Cacu_R[C_index].C_Number1.ToString();

                    tab_Cacu.SelectedIndex = 2;
                }


                if (Sales_Cacu_R[C_index].C_TF == 3)
                {
                    txt_Price_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu_R[C_index].C_Price1);
                    mtxtPriceDate3.Text = Sales_Cacu_R[C_index].C_AppDate1.ToString().Replace("-", "");
                    txt_C_Name_3.Text = Sales_Cacu_R[C_index].C_Name1.ToString();
                    txt_C_Card.Text = Sales_Cacu_R[C_index].C_CodeName.ToString();
                    txt_C_Card_Code.Text = Sales_Cacu_R[C_index].C_Code.ToString();
                    txt_C_Card_Number.Text = Sales_Cacu_R[C_index].C_Number1.ToString();
                    txt_C_Card_Ap_Num.Text = Sales_Cacu_R[C_index].C_Number2.ToString();
                    txt_C_Card_Ap_Num.Text = Sales_Cacu_R[C_index].C_Number2.ToString();
                    txt_Price_3_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu_R[C_index].C_Price2);
                    combo_C_Card_Year.Text = Sales_Cacu_R[C_index].C_Period1.ToString();
                    combo_C_Card_Month.Text = Sales_Cacu_R[C_index].C_Period2.ToString();
                    combo_C_Card_Per.Text = Sales_Cacu_R[C_index].C_Installment_Period.ToString();

                    txt_C_P_Number.Text = Sales_Cacu_R[C_index].C_P_Number.ToString();
                    txt_C_B_Number.Text = Sales_Cacu_R[C_index].C_B_Number.ToString();

                    txt_Sugi_TF.Text = Sales_Cacu_R[C_index].Sugi_TF.ToString();


                    tab_Cacu.SelectedIndex = 0;

                    if (Sales_Cacu_R[C_index].C_Number3.ToString() != "" && Sales_Cacu_R[C_index].C_Number4.ToString() == "" && Sales_Cacu_R[C_index].C_Price1 > 0)
                    {
                        butt_Cacu_Del.Visible = false;
                        //tab_Card.Enabled = false;  //카드가 수기특약이나 웹상으로 승인난 내역에 대해서는 취소가 이루어지 않으면.. 수정이나 삭제가 안되게 한다.
                        enable_Card_info_txt(false);

                        button_Ok.Visible = false;
                        button_Cancel.Visible = true;
                    }
                    else
                    {
                        button_Ok.Visible = true;
                        button_Cancel.Visible = false;
                    }
                }


              
                

                Data_Set_Form_TF = 0;
            }
        }

        private void butt_Card_Return_Click(object sender, EventArgs e)
        {
            if (Base_Error_Check__01() == false) return;  //주문종류 , 회원, 주문일자 입력 안햇는지 체크

            if (Sales_Cacu_R == null)
            {
                txt_TotalInputPrice_R.Text = "0";
                return;
            }

            double T_pr = 0;

            foreach (int t_key in Sales_Cacu_R.Keys)
            {
                if (Sales_Cacu_R[t_key].Del_TF != "D")
                {
                    T_pr = T_pr + Sales_Cacu_R[t_key].C_Price1;
                }
            }

            T_pr = T_pr - Math.Abs(double.Parse(txtPay_R1.Text));
            txt_TotalInputPrice_R.Text = string.Format(cls_app_static_var.str_Currency_Type, T_pr);

            if (Check_TextBox_Error() == false) return;

            //240228 윤도연 결제 취소 하려면 취소할 부분 결제 내역을 선택 해야 한다.
            if (string.IsNullOrWhiteSpace(txt_C_index.Text))
            {
                MessageBox.Show("취소할 원판매 결제 내역을 선택 해 주세요.");
                return;
            }

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            string OrderNumber_R = txt_OrderNumber_R.Text.Trim();
            int C_index_R = 0;
            string OrderNumber = txt_OrderNumber.Text.Trim();




            int C_index = int.Parse(txt_C_index.Text);

            if (txt_C_index_Re.Text == "") //추가 일경우에 새로운 입력
            {
                Base_Sub_Save_Cacu_R(ref C_index_R);
                Base_Sub_Clear("Cacu");
                Base_Sub_Sum_Cacu();
            }
            else  //
            {
                Base_Sub_Edit_Cacu();
                Base_Sub_Clear("Cacu");
                Base_Sub_Sum_Cacu();
            }


            int Save_Error_Check = 0;


            if (txt_OrderNumber_R.Text.Trim() != "")
            {
                Save_Base_Data(ref Save_Error_Check);
            }


            //매출 저장중이나 수정중에 오류가 발생 되어 있다 그럼 나머지 작업 하지 말고 걍 나가라
            if (Save_Error_Check == 0)
            {
                Base_Sub_Sum_Cacu();
                this.Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            cls_Socket csg = new cls_Socket();
            cls_Web Cls_Web = new cls_Web();
            string CardErrorMessage = string.Empty;
            string SuccesYN = Cls_Web.Dir_Card_Approve_Return(OrderNumber, C_index, OrderNumber_R, C_index_R, ref CardErrorMessage);

            if (SuccesYN == "N")
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("There was a problem canceling the card." + Environment.NewLine +
                     CardErrorMessage + Environment.NewLine +
                     "Please contact the company.");
                }
                else
                {
                    MessageBox.Show("카드 취소 중에 문제가 발생했습니다." + Environment.NewLine +
                    CardErrorMessage + Environment.NewLine +
                    "업체에 문의해 주십시요.");
                }
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                string StrSql = "EXEC Usp_Sell_Cacu_ReCul_4 '" + OrderNumber_R + "'";
                Temp_Connect.Update_Data(StrSql, "", "");
            }

            if (Save_Error_Check > 0)
            {

                Base_Ord_Clear();

                if (SalesDetail != null)
                    SalesDetail.Clear();

                Set_SalesDetail();  //회원의 주문 관련 주테이블 내역을 클래스에 넣는다.

                if (SalesDetail != null)
                    Base_Grid_Set();
            }

            cls_form_Meth ct = new cls_form_Meth();

            txt_C_Etc.Text = "";
            butt_Cacu_Del.Visible = false;
            butt_Cacu_Save.Text = ct._chang_base_caption_search("추가");
            tab_Cacu.Enabled = true;

            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Work_End"));

            this.Cursor = System.Windows.Forms.Cursors.Default;

        }

        private void Base_Sub_Save_Cacu_R(ref int C_Index)
        {
            cls_form_Meth ct = new cls_form_Meth();

            int New_C_index = 0;
            int Dic_Key = 0;

            if (Sales_Cacu_R != null)
            {
                foreach (int t_key in Sales_Cacu_R.Keys)
                {
                    if (New_C_index < Sales_Cacu_R[t_key].C_index)
                    {
                        New_C_index = t_key;
                    }
                }
            }


            Dic_Key = int.Parse(txt_C_index.Text.Trim());
            New_C_index = New_C_index + 1;

            cls_Sell_Cacu t_c_sell = new cls_Sell_Cacu();

            t_c_sell.OrderNumber = txt_OrderNumber_R.Text.Trim();
            t_c_sell.C_index = New_C_index;
            t_c_sell.C_Base_Index = int.Parse(txt_C_index.Text.Trim());

            t_c_sell.C_TF = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_TF;
            t_c_sell.C_TF_Name = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_TF_Name;

            t_c_sell.C_Price1 = -double.Parse(txtPay_R1.Text.Trim().Replace(",", ""));
            t_c_sell.C_Price2 = -double.Parse(txtPay_R1.Text.Trim().Replace(",", ""));
            t_c_sell.C_AppDate1 = mtxtPayDateR1.Text.Replace("-", "").Trim();
            t_c_sell.C_AppDate2 = "";
            t_c_sell.C_Etc = txtPayedEtc_R1.Text.Trim();

            t_c_sell.C_CancelDate = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_AppDate1;
            t_c_sell.C_CancelTF = 1;
            t_c_sell.C_CancelPrice = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Price1;

            t_c_sell.RecordID = cls_User.gid;
            t_c_sell.RecordTime = "";


            t_c_sell.C_Code = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Code;
            t_c_sell.C_CodeName = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_CodeName;
            t_c_sell.C_CodeName_2 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_CodeName_2;

            t_c_sell.C_Name1 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Name1;
            t_c_sell.C_Name2 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Name2;
            t_c_sell.C_Number1 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Number1;
            t_c_sell.C_Number2 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Number2;
            t_c_sell.C_Number3 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Number3;

            t_c_sell.C_Period1 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Period1;
            t_c_sell.C_Period2 = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Period2;
            t_c_sell.C_Installment_Period = Sales_Cacu[int.Parse(txt_C_index.Text.Trim())].C_Installment_Period;


            t_c_sell.Del_TF = "S";
            Sales_Cacu_R[New_C_index] = t_c_sell;

            Base_Sub_Clear("Cacu");

            if (Sales_Cacu_R != null)
                Cacu_R_Grid_Set(); //배송 그리드


            ////MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save")
            ////            + "\n" +
            ////cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Save"));

            C_Index = New_C_index;
        }

    }
}
