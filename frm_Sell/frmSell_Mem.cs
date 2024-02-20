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

namespace MLM_Program
{
    public partial class frmSell_Mem : Form
    {
        

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        public delegate void Take_NumberDele(ref string Send_Number, ref string Send_Name, ref string Send_OrderNumber);        
        public event Take_NumberDele Take_Mem_Number;
        
        


        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Item = new cls_Grid_Base();
        cls_Grid_Base cgb_Cacu = new cls_Grid_Base();
        cls_Grid_Base cgb_Rece = new cls_Grid_Base();
        cls_Grid_Base cgb_Rece_Item = new cls_Grid_Base();
        cls_Grid_Base cgb_Rece_Add = new cls_Grid_Base();
        cls_Grid_Base cgb_Mile = new cls_Grid_Base();

        private Dictionary<string, cls_Sell> SalesDetail = new Dictionary<string, cls_Sell>();
        private Dictionary<int, cls_Sell_Item> SalesItemDetail = new Dictionary<int, cls_Sell_Item>() ;
        private Dictionary<int, cls_Sell_Rece> Sales_Rece = new Dictionary<int, cls_Sell_Rece>();
        private Dictionary<int, cls_Sell_Cacu> Sales_Cacu = new Dictionary<int, cls_Sell_Cacu>();

        private Dictionary<string, TextBox>  Ncode_dic = new Dictionary<string, TextBox>();

        private const string base_db_name = "tbl_SalesDetail";
        private int Data_Set_Form_TF;
        private string idx_Mbid = "";
        private int idx_Mbid2 = 0;
        private string idx_Na_Code = "";
        private int idx_CurGrade = 0;

        private int Form_Key_Real_TF = 1 ;

        private int Save_Button_Click_Cnt = 0;
        private int print_Page = 0;

        private string InsuranceNumber_Ord_Print_FLAG = "";

        Series series_Item = new Series();

        public frmSell_Mem()
        {
            InitializeComponent();
        }





        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;
            Save_Button_Click_Cnt = 0;
            InsuranceNumber_Ord_Print_FLAG = "";

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset(1);

            dGridView_Base_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Item.d_Grid_view_Header_Reset(1);

            dGridView_Base_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Cacu.d_Grid_view_Header_Reset(1);

            dGridView_Base_Rece_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Rece.d_Grid_view_Header_Reset(1);

            dGridView_Base_Rece_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Rece_Item.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
            
            //mtxtMbid.Mask = "CCCCC";            
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

            combo_C_Card_Year.SelectedIndex = 0;
            combo_C_Card_Month.SelectedIndex = 0;
            combo_C_Card_Per.SelectedIndex = 0;

            Reset_Chart_Total(); // 차트 관련해서 리셋을 시킨다.

            //상품코드 자리수에 맞추어 텍스트 박스 길이 셋팅
            if (cls_app_static_var.Item_Sort_1_Code_Length == 0)
                txt_ItemCode.MaxLength = cls_app_static_var.Item_Code_Length;                
            
            else
            {
                if (cls_app_static_var.Item_Sort_1_Code_Length > 0)
                    txt_ItemCode.MaxLength = cls_app_static_var.Item_Sort_1_Code_Length;

                if (cls_app_static_var.Item_Sort_2_Code_Length > 0)
                    txt_ItemCode.MaxLength = cls_app_static_var.Item_Sort_2_Code_Length;

                if (cls_app_static_var.Item_Sort_3_Code_Length > 0)
                    txt_ItemCode.MaxLength = cls_app_static_var.Item_Sort_3_Code_Length;


                txt_ItemCode.MaxLength = cls_app_static_var.Item_Sort_1_Code_Length
                                + cls_app_static_var.Item_Sort_2_Code_Length
                                + cls_app_static_var.Item_Sort_3_Code_Length + txt_ItemCode.MaxLength;               
                
            }


            //마일리지 사용 여부에 따라서 보여질지 안보여질지            
             tab_Cacu.TabPages.Remove(tab_Mile); //직원주문는 마일리지가 없다.
            
            
            tableLayoutPanel68.Visible = false;  //조합이 아니면 공제 번호 관련 보여주지 않는다.
            //직원주문역시 공제 번호 발행이 안된다.


            Form_Key_Real_TF = 0;


            mtxtSellDate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtPriceDate3.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtPriceDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtPriceDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtPriceDate4.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtTel1.Mask = cls_app_static_var.Tel_Number_Fromat;
            mtxtTel2.Mask = cls_app_static_var.Tel_Number_Fromat;
            mtxtZip1.Mask = cls_app_static_var.ZipCode_Number_Fromat;

            mtxtSn.BackColor = cls_app_static_var.txt_Enable_Color;
            txtCenter.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_OrderNumber.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_Ins_Number.BackColor = cls_app_static_var.txt_Enable_Color;

            txt_SumPr.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_SumPV.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_SumCV.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_SumCard.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_SumCash.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_SumBank.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_SumMile.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_TotalInputPrice.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_UnaccMoney.BackColor = cls_app_static_var.txt_Enable_Color;

            txt_C_Bank_Code.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_C_Bank_Code_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_C_Bank_Code_3.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_Price_4_2.BackColor = cls_app_static_var.txt_Enable_Color;

            mtxtSellDate.Text = cls_User.gid_date_time;

            dGridView_Base.Dock = DockStyle.Fill;
            radioB_M1.Checked = true; 

            mtxtMbid.Focus();

        }



        private void frmBase_Resize(object sender, EventArgs e)
        {
            //int base_w = this.Width / 3;
            //butt_Clear.Width = base_w;
            //butt_Save.Width = base_w;

            ////butt_Delete.Width = base_w;
            //butt_Exit.Width = base_w;

            //butt_Clear.Left = 0;
            //butt_Save.Left = butt_Clear.Left + butt_Clear.Width;

            ////butt_Delete.Left = butt_Save.Left + butt_Save.Width;
            //butt_Exit.Left = butt_Save.Left + butt_Save.Width;    

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

            cfm.button_flat_change(butt_Ord_Clear);
            cfm.button_flat_change(butt_Print);
            
            cfm.button_flat_change(butt_Item_Del);
            cfm.button_flat_change(butt_Item_Save);
            cfm.button_flat_change(butt_Item_Clear);

            cfm.button_flat_change(butt_Cacu_Del);
            cfm.button_flat_change(butt_Cacu_Save);
            cfm.button_flat_change(butt_Cacu_Clear);

            cfm.button_flat_change(butt_Rec_Del);
            cfm.button_flat_change(butt_Rec_Save);
            cfm.button_flat_change(butt_Rec_Clear);
            cfm.button_flat_change(butt_Rec_Add);

            cfm.button_flat_change(butt_Mile_Search);            
                                    
            cfm.button_flat_change(butt_AddCode);
        }



        private void frmBase_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();
            string Send_Number = ""; string Send_Name = ""; string Send_OrderNumber = "";
               
            Take_Mem_Number(ref Send_Number, ref Send_Name, ref Send_OrderNumber );
                
            if (Send_Number != "")
            {
                mtxtMbid.Text = Send_Number;
                Set_Form_Date(mtxtMbid.Text, "m");

                if (Send_OrderNumber != "" )
                {                  
                    Base_Ord_Clear();
                    Put_OrderNumber_SellDate(Send_OrderNumber);                  
                }
            }

        }


        private void frmBase_From_KeyDown(object sender, KeyEventArgs e)
        {
            //폼일 경우에는 ESC버튼에 폼이 종료 되도록 한다
            if (sender is Form)
            {
                if (e.KeyCode == Keys.Escape)
                {

                    if (!this.Controls.ContainsKey("Popup_gr") && dGridView_Base_Rece_Add.Visible == false && dGridView_Base_Mile.Visible == false)
                        this.Close();
                    else
                    {
                        if (dGridView_Base_Rece_Add.Visible == true)
                        {
                            dGridView_Base_Rece_Add.Visible = false;

                            cls_form_Meth cfm = new cls_form_Meth();
                            cfm.form_Group_Panel_Enable_True(this);
                        }
                        else if (dGridView_Base_Mile.Visible == true)
                        {
                            dGridView_Base_Mile.Visible = false;
                            txt_Price_4.Focus();
                        }
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
                if (e.KeyValue == 123 || e.KeyValue == 113  || e.KeyValue == 119 || e.KeyValue == 112)
                    Base_Button_Click(T_bt, ee1);
            }

        }


       

        private void txtData_Enter(object sender, EventArgs e)
        {
            if (Form_Key_Real_TF >= 1)
                return;

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


                if (tb.Tag != null)
                {
                    if (tb.Tag.ToString() == "2" && tb.Text != "")
                    {
                        Data_Set_Form_TF = 1;
                        double T_p = double.Parse(tb.Text.Replace(",", "").ToString());
                        tb.Text = string.Format(cls_app_static_var.str_Currency_Type, T_p);
                        Data_Set_Form_TF = 0;
                    }
                }


                if (tb.Name == "mtxtSellDate")
                {
                    
                    if (tb.Text != "" && mtxtSellDate2.Text == "")
                    {
                        mtxtSellDate2.Text = tb.Text;
                    }

                    if (tb.Text != "")
                    {
                        if (Base_Error_Check_Not_Sellcode__01() == false)
                        {
                            txtSellCode.Focus();
                            return;  //주문종류 , 회원, 주문일자 입력 안햇는지 체크
                        }
                        
                        double T_p = 0;
                        string T_Mbid = mtxtMbid.Text;
                        string Mbid = ""; int Mbid2 = 0;
                        cls_Search_DB csb = new cls_Search_DB();
                        if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == 1)
                        {
                            cls_tbl_Mileage ctm = new cls_tbl_Mileage();
                            T_p = ctm.Using_Mileage_Search(Mbid, Mbid2, tb.Text);
                            txt_Price_4_2.Text = string.Format(cls_app_static_var.str_Currency_Type, T_p);
                        }
                    }

                    txtSellCode.Focus();
                }
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

            //if (!(Char.IsLetter(e.KeyChar)) && e.KeyChar != 8)
            //{
            //    e.Handled = true;
            //}



            if (e.KeyChar == 13)
            {
                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    int reCnt = 0;
                    cls_Search_DB cds = new cls_Search_DB();
                    string Search_Name = "";
                    reCnt = cds.Member_Name_Search_Mem(mtb.Text, ref Search_Name);

                    if (reCnt == 1)
                    {
                        if (mtb.Name == "mtxtMbid")
                        {
                            txtName.Text = Search_Name;
                            if (Input_Error_Check(mtb, "m") == true)
                                Set_Form_Date(mtb.Text, "m");
                            mtxtSellDate.Focus();
                        }
                    }

                    else if (reCnt > 1)  //회원번호 비슷한 사람들이 많은 경우
                    {
                        string Mbid = "";
                        int Mbid2 = 0;
                        cds.Member_Nmumber_Split(mtb.Text, ref Mbid, ref Mbid2);

                        frmBase_Mem_Search e_f = new frmBase_Mem_Search();

                        if (mtb.Name == "mtxtMbid")
                        {
                            e_f.Send_Mem_Number += new frmBase_Mem_Search.SendNumberDele(e_f_Send_Mem_Number);
                            e_f.Call_searchNumber_Info += new frmBase_Mem_Search.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info);
                        }

                        e_f.ShowDialog();

                        SendKeys.Send("{TAB}");
                    }
                }
                else
                    SendKeys.Send("{TAB}");
            }

        }


        void e_f_Send_MemNumber_Info(ref string searchMbid, ref string seachName)
        {
            seachName = "";
            int searchMbid2 = 0; 
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
                    Form_Key_Real_TF = 0;
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
                Form_Key_Real_TF = 0;
                mtxtMbid.Focus(); 
            }

            if (mtb.Name == "txtR_Id")
            {
                _From_Data_Clear();
                Form_Key_Real_TF = 0;
                //txtR_Id.Focus(); 
            }
            
            

            //마스크텍스트 박스에 입력한 내용이 있으면 그곳 다음으로 커서가 가게 한다.
            if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                mtb.SelectionStart = mtb.Text.Replace("-", "").Replace("_", "").Trim().Length + 1;

            

            

        }











        private void Set_Form_Date(string T_Mbid, string T_sort )
        {
            _From_Data_Clear();
            Form_Key_Real_TF = 0;
            //idx_Mbid = ""; idx_Mbid2 = 0;
            string Mbid = ""; int Mbid2 = 0; idx_Na_Code = ""; idx_CurGrade = 0;
            Data_Set_Form_TF = 1;
            cls_Search_DB csb = new cls_Search_DB();


            Mbid = T_Mbid; 
            //if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == 1)

            if (Mbid != "")
            {
                string Tsql = "";

                Tsql = "Select tbl_User.user_Ncode Ncode , tbl_User.U_Name N_Name ";
                Tsql = Tsql + " , tbl_User.Leave_TF  ";
                Tsql = Tsql + " , tbl_User.phone, tbl_User.Na_Code   ";
                //Tsql = Tsql + " , tbl_User.ZipCode  , tbl_User.add1  ";
                //Tsql = Tsql + " , tbl_User.add2  ";
                Tsql = Tsql + " From tbl_User (nolock) ";

                Tsql = Tsql + " Where tbl_User.user_Ncode = '" + Mbid.ToString() + "'";

                Tsql = Tsql + " And  tbl_User.Leave_TF  = 0 ";
                //Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                Tsql = Tsql + " And tbl_User.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

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

                
                chart_Item.Series.Clear();
                Save_Nom_Line_Chart();

                if (SalesDetail != null)
                {
                    Base_Grid_Set();  // 위에서 배열에 넣은 내역을 그리드 상으로 옴김 판매 주 내역

                    //Set_SalesItemDetail(Mbid, Mbid2);        //상품 집계 관련해서 도표에 뿌려준다.            
                }
                mtxtMbid.Focus();                
            }
            
            Data_Set_Form_TF = 0;            
        }




        private void Set_Form_Date(DataSet ds)
        {
            idx_Mbid = ds.Tables[base_db_name].Rows[0]["Ncode"].ToString();
            idx_Mbid2 = 0;
            idx_Na_Code  =   ds.Tables[base_db_name].Rows[0]["Na_Code"].ToString();
            idx_CurGrade = 0;

            mtxtMbid.Text = ds.Tables[base_db_name].Rows[0]["Ncode"].ToString();
            txtName.Text = ds.Tables[base_db_name].Rows[0]["N_Name"].ToString();
            //mtxtSn.Text =  encrypter.Decrypt(  ds.Tables[base_db_name].Rows[0]["Cpno"].ToString(),"Cpno") ;
                  
            //txtCenter.Text = ds.Tables[base_db_name].Rows[0]["B_Name"].ToString();
            //txtCenter_Code.Text = ds.Tables[base_db_name].Rows[0]["businesscode"].ToString();
          

            //txtName.ReadOnly = false;
            //txtName.BackColor = SystemColors.Window;
            //txtName.BorderStyle = BorderStyle.Fixed3D;

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
            strSql = strSql + " , '' SellCodeName  ";

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

            
             strSql = strSql + " , InsuranceNumber AS InsuranceNumber2 ";
            

            strSql = strSql + " From tbl_SalesDetail (nolock) ";
            strSql = strSql + " LEFT JOIN tbl_User (nolock) ON tbl_User.User_Ncode = tbl_SalesDetail.Mbid  ";            
            
            strSql = strSql + " LEFT JOIN tbl_Business (nolock) ON tbl_SalesDetail.BusCode = tbl_Business.NCode And tbl_SalesDetail.Na_code = tbl_Business.Na_code ";
            
            strSql = strSql + " Where tbl_SalesDetail.Mbid = '" + idx_Mbid.ToString() + "'";
            

            //// strSql = strSql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            //strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_User.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

            strSql = strSql + " And tbl_SalesDetail.SellCode = '' ";

            strSql = strSql + " Order By OrderNumber DESC ";

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
                t_c_sell.SellDate_2 = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate_2"].ToString();
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
                t_c_sell.Be_InputMile = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputMile"].ToString());
                t_c_sell.InputMile = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["InputMile"].ToString());
                t_c_sell.InputPass_Pay = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["InputPass_Pay"].ToString());
                t_c_sell.UnaccMoney = double.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["UnaccMoney"].ToString());

                t_c_sell.Etc1 = ds.Tables[base_db_name].Rows[fi_cnt]["Etc1"].ToString();
                t_c_sell.Etc2 = ds.Tables[base_db_name].Rows[fi_cnt]["Etc2"].ToString();

                t_c_sell.ReturnTF = int.Parse (ds.Tables[base_db_name].Rows[fi_cnt]["ReturnTF"].ToString());
                t_c_sell.ReturnTFName = ds.Tables[base_db_name].Rows[fi_cnt]["ReturnTFName"].ToString();
                //t_c_sell.InsuranceNumber = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber"].ToString();
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
        private void Base_Grid_Set(  )
        {
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            Dictionary<string, double> SelType_1 = new Dictionary<string, double>();

            int fi_cnt = 0;
            double S_cnt4 = 0; double S_cnt5 = 0; double S_cnt6 = 0;
            double Sum_13 = 0; double Sum_14 = 0; double Sum_15 = 0; ; double Sum_16 = 0; ; double Sum_17 = 0;
            double Sell_Cnt_1 = 0; double Sell_Cnt_2 = 0; 
            foreach (string  t_key in SalesDetail.Keys)
            {
                if (SalesDetail[t_key].Del_TF != "D")
                {
                    Set_gr_dic(ref gr_dic_text, t_key, fi_cnt);  //데이타를 배열에 넣는다.

                    S_cnt4 = S_cnt4 + SalesDetail[t_key].TotalPrice;
                    S_cnt5 = S_cnt5 + SalesDetail[t_key].TotalInputPrice;
                    S_cnt6 = S_cnt6 + SalesDetail[t_key].TotalPV;

                    Sum_13 = Sum_13 + SalesDetail[t_key].InputCash;
                    Sum_14 = Sum_14 + SalesDetail[t_key].InputCard;
                    Sum_15 = Sum_15 + SalesDetail[t_key].InputPassbook;
                    Sum_17 = Sum_17 + SalesDetail[t_key].InputMile ;
                    Sum_16 = Sum_16 + SalesDetail[t_key].UnaccMoney;

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

            Reset_Chart_Total(Sum_13, Sum_14, Sum_15,Sum_17);
            Reset_Chart_Total(ref SelType_1);
            Reset_Chart_Total(Sell_Cnt_1, Sell_Cnt_2);

            cls_form_Meth cm = new cls_form_Meth();

            object[] row0 = { ""
                                ,"<< " + cm._chang_base_caption_search("합계") + " >>"
                                ,""
                                ,""
                                ,""
                                ,S_cnt4

                                ,S_cnt5                                
                                ,S_cnt6
                                ,""
                                ,""
                                ,Sum_13      
                       
                                ,Sum_14
                                ,Sum_15
                                ,Sum_16
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
                                ,SalesDetail[t_key].SellDate_2

                                ,SalesDetail[t_key].TotalPrice  
                                ,SalesDetail[t_key].TotalInputPrice  
                                ,SalesDetail[t_key].TotalPV   
                                ,SalesDetail[t_key].SellCodeName  
                                ,SalesDetail[t_key].ReturnTFName
 
                                ,SalesDetail[t_key].InputCash                            
                                ,SalesDetail[t_key].InputCard                            
                                ,SalesDetail[t_key].InputPassbook 
                                ,SalesDetail[t_key].UnaccMoney 
                                ,SalesDetail[t_key].InputMile 

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
            cgb.grid_col_Count = 17;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"_결제_승인여부" , "_공제번호"  , "주문번호"   , "주문일자" , "_정산_일자"
                                    , "총주문액"  , "총입금액"      , "_총PV"    , "_주문종류"   , "구분"    
                                    , "현금"  ,"카드" ,"무통장", "미결제"   ,"_마일리지"
                                    ,  "기록자" ,  "기록일"
                                };

           

            int[] g_Width = { 0,0, 120, 80, 0
                                , 80  ,80 , 0 , 0 , 80
                                , 80  ,80 ,80,80 ,0
                                ,80 ,80
                            };
            cgb.grid_col_w = g_Width;
            

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleCenter  //5   

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

                                 ,DataGridViewContentAlignment.MiddleCenter
                                  ,DataGridViewContentAlignment.MiddleLeft
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;                    

            cgb.grid_col_header_text = g_HeaderText;
            cgb.grid_cell_format = gr_dic_cell_format;
            
            cgb.grid_col_alignment = g_Alignment;
           

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true 
                                    ,true , true,  true , true,  true       
                                    ,  true ,  true       
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
                txt_TotalCV.Text = "0";
                return;
            }

            int fi_cnt = 0; double T_Pv = 0; double T_pr = 0; double T_Cv = 0;
            int r_cnt = 0;

            foreach (int t_key in SalesItemDetail.Keys)
            {
                if (SalesItemDetail[t_key].Del_TF != "D")
                {
                    T_Pv = T_Pv + SalesItemDetail[t_key].ItemTotalPV  ;
                    T_pr = T_pr + SalesItemDetail[t_key].ItemTotalPrice  ;
                    T_Cv = T_Cv + SalesItemDetail[t_key].ItemTotalCV;
                    r_cnt++;
                }
                fi_cnt++;
            }

        
            txt_TotalPrice.Text = string.Format(cls_app_static_var.str_Currency_Type, T_pr);
            txt_TotalPv.Text = string.Format(cls_app_static_var.str_Currency_Type, T_Pv);
            txt_TotalCV.Text = string.Format(cls_app_static_var.str_Currency_Type, T_Cv);
        }


        private void Item_Grid_Set()
        {
            dGridView_Base_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Item.d_Grid_view_Header_Reset();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            int fi_cnt = 0;
            double S_cnt4 = 0; double S_cnt5 = 0; double S_cnt6 = 0; int S_cnt7 = 0;
            double S_cnt8 = 0; double S_cnt9 = 0; double S_cnt10 = 0;
            foreach (int t_key in SalesItemDetail.Keys)
            {
                if (SalesItemDetail[t_key].Del_TF != "D")
                {
                    Set_gr_Item(ref gr_dic_text, t_key, fi_cnt);  //데이타를 배열에 넣는다.

                    S_cnt4 = S_cnt4 + SalesItemDetail[t_key].ItemPrice;
                    S_cnt5 = S_cnt5 + SalesItemDetail[t_key].ItemPV;

                    S_cnt6 = S_cnt6 + SalesItemDetail[t_key].ItemCV;

                    S_cnt7 = S_cnt7 + SalesItemDetail[t_key].ItemCount;
                    S_cnt8 = S_cnt8 + SalesItemDetail[t_key].ItemTotalPrice;
                    S_cnt9 = S_cnt9 + SalesItemDetail[t_key].ItemTotalPV;
                    S_cnt10 = S_cnt10 + SalesItemDetail[t_key].ItemTotalCV;
                }
                fi_cnt++;
            }

            txt_SumCnt.Text = string.Format(cls_app_static_var.str_Currency_Type, S_cnt7);
            txt_SumPr.Text = string.Format(cls_app_static_var.str_Currency_Type, S_cnt8);
            txt_SumPV.Text = string.Format(cls_app_static_var.str_Currency_Type, S_cnt9);
            txt_SumCV.Text = string.Format(cls_app_static_var.str_Currency_Type, S_cnt10);

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
                                ,SalesItemDetail[t_key].ItemCode  
                                ,SalesItemDetail[t_key].ItemName   
                                ,SalesItemDetail[t_key].ItemPrice   
                                ,SalesItemDetail[t_key].ItemPV
                                ,SalesItemDetail[t_key].ItemCV

                                ,SalesItemDetail[t_key].ItemCount   
                                ,SalesItemDetail[t_key].ItemTotalPrice 
                                ,SalesItemDetail[t_key].ItemTotalPV                                 
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

            string[] g_HeaderText = {""  , "상품_코드"   , "상품명"  , "개별단가"   , "_개별PV"
                    , "_개별CV" , "주문_수량"   , "총상품액"    , "_총상품PV"  , "구분" 
                    , "_비고"
                                };

            int[] g_Width = { 0, 90, 160, 80, 0
                    , 0  ,80 , 80 , 0 , 70 
                    , 0
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
                                ,DataGridViewContentAlignment.MiddleCenter //10

                                ,DataGridViewContentAlignment.MiddleLeft  
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


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                     ,true  ,true , true,  true,  true
                     ,true                                                            
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
            double Sell_Pr  = double.Parse(txt_TotalPrice.Text.Trim().Replace (",","")) ;
            if (Sales_Cacu == null)
            {
                txt_TotalInputPrice.Text = "0";
                txt_UnaccMoney.Text = txt_TotalPrice.Text.Trim(); 
                return;
            }

            double T_pr = 0;

            foreach (int t_key in Sales_Cacu.Keys)
            {
                if (Sales_Cacu[t_key].Del_TF != "D")
                {
                    T_pr = T_pr + Sales_Cacu[t_key].C_Price1;                    
                }                
            }



            txt_TotalInputPrice.Text = string.Format(cls_app_static_var.str_Currency_Type, T_pr);
            txt_UnaccMoney.Text = string.Format(cls_app_static_var.str_Currency_Type, Sell_Pr - T_pr);
        }


        private void Cacu_Grid_Set()
        {
            dGridView_Base_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Cacu.d_Grid_view_Header_Reset();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            int fi_cnt = 0;
            double S_cnt6 = 0; double S_cnt7 = 0; double S_cnt8 = 0; double S_cnt9 = 0; 
            foreach (int t_key in Sales_Cacu.Keys)
            {
                if (Sales_Cacu[t_key].Del_TF != "D")
                {
                    Set_gr_Cacu(ref gr_dic_text, t_key, fi_cnt);  //데이타를 배열에 넣는다.
                    if (Sales_Cacu[t_key].C_TF == 1) //현금
                        S_cnt6 = S_cnt6 + Sales_Cacu[t_key].C_Price1 ;
                    if (Sales_Cacu[t_key].C_TF == 2) //무통장
                        S_cnt8 = S_cnt8 + Sales_Cacu[t_key].C_Price1;
                    if (Sales_Cacu[t_key].C_TF == 3) //카드
                        S_cnt7 = S_cnt7 + Sales_Cacu[t_key].C_Price1;

                    if (Sales_Cacu[t_key].C_TF == 4) //마일리지
                        S_cnt9 = S_cnt9 + Sales_Cacu[t_key].C_Price1;
                }
                fi_cnt++;
            }

            txt_SumCash.Text = string.Format(cls_app_static_var.str_Currency_Type, S_cnt6);
            txt_SumCard.Text = string.Format(cls_app_static_var.str_Currency_Type, S_cnt7);
            txt_SumBank.Text = string.Format(cls_app_static_var.str_Currency_Type, S_cnt8);
            txt_SumMile.Text = string.Format(cls_app_static_var.str_Currency_Type, S_cnt9);

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
                                , "카드_은행번호"   , "카드소유자_입금자"    , ""  , "_비고" , ""
                                };

            int[] g_Width = { 0, 90, 70, 90, 100
                                ,120 , 100 , 0 , 0 , 0
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





        //////Sales_Rece___Sales_Rece__Sales_Rece__Sales_Rece
        //////Sales_Rece___Sales_Rece__Sales_Rece__Sales_Rece
        private void Rece_Grid_Set()
        {
            dGridView_Base_Rece_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Rece.d_Grid_view_Header_Reset();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            int fi_cnt = 0;
            foreach (int t_key in Sales_Rece.Keys)
            {
                if (Sales_Rece[t_key].Del_TF != "D")
                    Set_gr_Rece(ref gr_dic_text, t_key, fi_cnt);  //데이타를 배열에 넣는다.
                fi_cnt++;
            }

            cgb_Rece.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Rece.db_grid_Obj_Data_Put();
        }


        private void Set_gr_Rece(ref Dictionary<int, object[]> gr_dic_text, int t_key, int fi_cnt)
        {
            object[] row0 = { Sales_Rece[t_key].SalesItemIndex   
                                ,Sales_Rece[t_key].Receive_Method_Name   
                                ,Sales_Rece[t_key].Get_Date1      
                                ,Sales_Rece[t_key].Get_Name1     
                                ,Sales_Rece[t_key].Get_ZipCode     

                                ,Sales_Rece[t_key].Get_Address1    
                                ,Sales_Rece[t_key].Get_Address2   
                                ,Sales_Rece[t_key].Get_Tel1                                 
                                ,Sales_Rece[t_key].Get_Tel2           
                                ,Sales_Rece[t_key].Get_Etc1        
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Base_Rece_Header_Reset()
        {
            cgb_Rece.Grid_Base_Arr_Clear();
            cgb_Rece.basegrid = dGridView_Base_Rece;
            cgb_Rece.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Rece.grid_col_Count = 10;
            cgb_Rece.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {""  , "배송구분"   , "배송일"  , "수령인"   , "우편_번호"        
                                , "주소1"   , "주소2"    , "연락처_1"  , "연락처_2" , "비고"
                                };

            int[] g_Width = { 0, 90, 0, 90, 100
                                ,120 , 100 , 90 , 150 , 200
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

            cgb_Rece.grid_col_header_text = g_HeaderText;
            cgb_Rece.grid_cell_format = gr_dic_cell_format;
            cgb_Rece.grid_col_w = g_Width;
            cgb_Rece.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                            
                                   };
            cgb_Rece.grid_col_Lock = g_ReadOnly;

            cgb_Rece.basegrid.RowHeadersVisible = false;
        }
        //////Sales_Rece___Sales_Rece__Sales_Rece__Sales_Rece
        //////Sales_Rece___Sales_Rece__Sales_Rece__Sales_Rece





        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail
        private void Rece_Item_Grid_Set(int Recindex = 0)
        {
            dGridView_Base_Rece_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Rece_Item.d_Grid_view_Header_Reset();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            int fi_cnt = 0; string V_Check = "";
            foreach (int t_key in SalesItemDetail.Keys)
            {
                if (Recindex == 0)//삭제되지 않고 배송 정보가 없는 내역들만 뿌려준다.
                {
                    V_Check = "V";
                    if (SalesItemDetail[t_key].Del_TF != "D" && SalesItemDetail[t_key].RecIndex == 0)
                        Set_gr_Rece_Item(ref gr_dic_text, t_key, fi_cnt, V_Check);  //데이타를 배열에 넣는다.
                }
                else
                {
                    if (SalesItemDetail[t_key].SalesItemIndex == Recindex)
                    {
                        V_Check = "V";
                        Set_gr_Rece_Item(ref gr_dic_text, t_key, fi_cnt, V_Check);  //데이타를 배열에 넣는다.
                    }
                }

                fi_cnt++;
            }

            cgb_Rece_Item.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Rece_Item.db_grid_Obj_Data_Put();
        }


        private void Set_gr_Rece_Item(ref Dictionary<int, object[]> gr_dic_text, int t_key, int fi_cnt,string V_Check ="")
        {
            object[] row0 = { V_Check
                                ,SalesItemDetail[t_key].SalesItemIndex  
                                ,SalesItemDetail[t_key].ItemCode   
                                ,SalesItemDetail[t_key].ItemName   
                                ,SalesItemDetail[t_key].ItemCount  

                                ,SalesItemDetail[t_key].Etc  
                                ,""
                                ,""
                                ,""
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Base_Rece_Item_Header_Reset()
        {
            cgb_Rece_Item.Grid_Base_Arr_Clear();
            cgb_Rece_Item.basegrid = dGridView_Base_Rece_Item;
            cgb_Rece_Item.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Rece_Item.grid_col_Count = 10;

            string[] g_HeaderText = {"선택"  , ""   , "상품_코드"  , "상품명"   , "주문_수량"        
                                , "비고"   , ""    , ""  , "" , ""
                                };

            int[] g_Width = { 30, 0, 60, 150, 60
                                ,200 , 0 , 0 , 0 , 0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleLeft  //10
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
         
            cgb_Rece_Item.grid_col_header_text = g_HeaderText;
            cgb_Rece_Item.grid_cell_format = gr_dic_cell_format;
            cgb_Rece_Item.grid_col_w = g_Width;
            cgb_Rece_Item.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                            
                                   };
            cgb_Rece_Item.grid_col_Lock = g_ReadOnly;

            cgb_Rece_Item.basegrid.RowHeadersVisible = false;
        }


        private void dGridView_Base_2_CellClick(object sender, DataGridViewCellEventArgs e)
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
            }
        }
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail



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
                    if (c_er.Input_Date_Err_Check__01(mtb) ==  false )
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
                if (T_R.Text_KeyChar_Check(e,tb,tb) == false)
                {
                    e.Handled = true;
                    return;
                } // end if   
            }
            else if ((tb.Tag != null) && (tb.Tag.ToString() == "1"))
            {
                //숫자만 입력 가능
                if (T_R.Text_KeyChar_Check(e,tb, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "2"))
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
                if (T_R.Text_KeyChar_Check(e, tb,"-") == false)
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

            Data_Set_Form_TF = 1;
            if (tb.Name == "txt_Price_3")
            {
                if (tb.Text != "")
                    tb.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(tb.Text.Replace(",", "")));

                if (txt_Price_3_2.Text == "")
                    txt_Price_3_2.Text = tb.Text.Trim();

                if (mtxtPriceDate3.Text.Replace("-", "").Trim() == "")
                    mtxtPriceDate3.Text = mtxtSellDate.Text ;
            }

            if (tb.Name == "txt_Price_2")
            {
                if (tb.Text != "")
                    tb.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(tb.Text.Replace(",", "")));
                if (mtxtPriceDate2.Text.Replace("-","").Trim () == "")
                    mtxtPriceDate2.Text = mtxtSellDate.Text;
            }

            if (tb.Name == "txt_Price_1")
            {
                if (tb.Text != "")
                    tb.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(tb.Text.Replace(",", "")));

                if (mtxtPriceDate1.Text.Replace("-", "").Trim() == "")
                    mtxtPriceDate1.Text = mtxtSellDate.Text;
            }


            if (tb.Name == "txt_Price_4")
            {
                if (tb.Text != "")
                    tb.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(tb.Text.Replace(",", "")));

                if (mtxtPriceDate4.Text.Replace("-", "").Trim() == "")
                    mtxtPriceDate4.Text = mtxtSellDate.Text;
            }

            if (tb.Name == "txt_ETC1")
                txt_ItemCode.Focus();
            else
                SendKeys.Send("{TAB}");

            Data_Set_Form_TF = 0;
        }



        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            int Sw_Tab = 0;

            if ((sender is TextBox) == false) return;

            TextBox tb = (TextBox)sender;
            //if (tb.TextLength >= tb.MaxLength)
            //{
            //    SendKeys.Send("{TAB}");
            //    Sw_Tab = 1;
            //}

            //if (tb.Name == "txtR_Id")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.Trim() == "")
            //        txtR_Id_Code.Text = "";
            //    Data_Set_Form_TF = 0;
            //}

            if (tb.Name == "txtCenter2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter2_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtSellCode")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtSellCode_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_Base_Rec")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txt_Base_Rec_Code.Text = "";
                Data_Set_Form_TF = 0;
            }



            if (tb.Name == "txt_Receive_Method")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                {
                    txt_Receive_Method_Code.Text = "";
                    dGridView_Base_Rece_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                    cgb_Rece_Item.d_Grid_view_Header_Reset();
                }
                else
                {
                    if (SalesItemDetail != null && txt_Receive_Method_Code.Text != "")
                        Rece_Item_Grid_Set();
                }
                Data_Set_Form_TF = 0;
            }


            if (tb.Name == "txt_ItemCode")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txt_ItemName.Text = "";
                Data_Set_Form_TF = 0;
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

            
            
        }


        void T_R_Key_Enter_13_Name(string txt_tag, TextBox tb)
        {
            if (txt_tag != "")
            {
                int reCnt = 0;
                cls_Search_DB cds = new cls_Search_DB();
                string Search_Mbid = "";
                reCnt = cds.Member_Name_Search_Mem(ref Search_Mbid, txt_tag);

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
                    frmBase_Mem_Search e_f = new frmBase_Mem_Search();
                    if (tb.Name == "txtName")
                    {
                        e_f.Send_Mem_Number += new frmBase_Mem_Search.SendNumberDele(e_f_Send_Mem_Number);
                        e_f.Call_searchNumber_Info += new frmBase_Mem_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);
                    }

                    e_f.ShowDialog();

                    SendKeys.Send("{TAB}");
                }


            }
            else
                SendKeys.Send("{TAB}");

        }

        void e_f_Send_MemName_Info(ref string searchMbid,  ref string seachName)
        {
            searchMbid = ""; 
            seachName = txtName.Text.Trim();
        }           



        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {

            //if (tb.Name == "txtR_Id")
            //{
            //    Data_Set_Form_TF = 1;
            //    Db_Grid_Popup(tb, txtR_Id_Code);
            //    Data_Set_Form_TF = 0;
            //    return;        
            //}


            if (Base_Error_Check__01(1) == false)
                return;            

            if (tb.Name == "txtCenter2")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter2_Code);
                Data_Set_Form_TF = 0;
            }

           

            if (tb.Name == "txtSellCode")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtSellCode_Code);              
                Data_Set_Form_TF = 0;
            }




            if (tb.Name == "txt_Receive_Method")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txt_Receive_Method_Code);               
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_Base_Rec")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txt_Base_Rec_Code);                
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_ItemCode")
            {
                Data_Set_Form_TF = 1;
                if (Base_Error_Check__01() == false) return;  //주문종류 , 회원, 주문일자 입력 안햇는지 체크
                Db_Grid_Popup(tb, txt_ItemName);              
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
                    Db_Grid_Popup(tb,  "");
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
                Ncode_dic["cardname"] = tb ;
                
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, "");
                else
                    Ncod_Text_Set_Data(tb);

                SendKeys.Send("{TAB}");
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
                cgb_Pop.Next_Focus_Control = txt_ETC1;

            if (tb.Name == "txtCenter2")
                cgb_Pop.Next_Focus_Control = txt_ETC1;

            if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = txt_ETC1;

            if (tb.Name == "txtR_Id")
                cgb_Pop.Next_Focus_Control = txtName ;

            if (tb.Name == "txtChange")
                cgb_Pop.Next_Focus_Control = txtCenter2;

            if (tb.Name == "txtSellCode")
                cgb_Pop.Next_Focus_Control = txtCenter2;

            if (tb.Name == "txt_Base_Rec")
                cgb_Pop.Next_Focus_Control = mtxtZip1;

            if (tb.Name == "txt_Receive_Method")
                cgb_Pop.Next_Focus_Control = txt_Get_Name1;

            if (tb.Name == "txt_ItemCode")
            {
                cgb_Pop.Next_Focus_Control = txt_ItemCount;
                string ABC_TF = "" ;
                if (radioB_M1.Checked == true) //직원
                {
                    ABC_TF = "3";
                }
                else if (radioB_M2.Checked == true)//임원
                {
                    ABC_TF = "4";
                }
                else 
                {
                    ABC_TF = "1";
                }
                cgb_Pop.Db_Grid_Popup_Make_Sql(1,tb, tb1_Code, idx_Na_Code, mtxtSellDate.Text, ABC_TF);

            }
            else
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, idx_Na_Code , mtxtSellDate.Text  );

            if (tb.Name == "txt_Receive_Method")
            {
                if (SalesItemDetail != null && txt_Receive_Method_Code.Text != "")
                    Rece_Item_Grid_Set();
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
                {
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);
                    cgb_Pop.Next_Focus_Control = txt_ETC1;
                }

                if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtSellCode")
                {
                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", strSql);
                    cgb_Pop.Next_Focus_Control = txtCenter2;
                }

                if (tb.Name == "txt_Base_Rec")
                    cgb_Pop.db_grid_Popup_Base(2, "배송사_코드", "배송사", "ncode", "name", strSql);


                if (tb.Name == "txt_Receive_Method")
                {
                    cgb_Pop.db_grid_Popup_Base(2, "배송_코드", "배송_구분", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, strSql);
                    cgb_Pop.Next_Focus_Control = txt_Get_Name1;
                }

                if (tb.Name == "txt_C_TF")
                    cgb_Pop.db_grid_Popup_Base(2, "결제_코드", "결제_종류", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, strSql);

                if (tb.Name == "txt_ItemCode")
                {
                    cgb_Pop.db_grid_Popup_Base(4, "상품명", "상품코드", "개별단가", "개별PV", "Name", "Ncode", "price2", "price4", strSql);
                    cgb_Pop.Next_Focus_Control = txt_ItemCount;
                }
                             
            }
            else
            {
                if (tb.Name == "txtCenter2")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                    if (idx_Na_Code != "") Tsql = Tsql + " And  Na_Code = '" + idx_Na_Code + "'"; 
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                    cgb_Pop.Next_Focus_Control = txt_ETC1;
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
                    cgb_Pop.Next_Focus_Control = txtCenter2;
                }


                if (tb.Name == "txt_Base_Rec")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Base_Rec (nolock) ";
                    if (idx_Na_Code != "") Tsql = Tsql + " Where  Na_Code = '" + idx_Na_Code + "'"; 
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
                    Tsql = Tsql + " Where M_Detail_S = 'tbl_Sales_Rece' ";
                    Tsql = Tsql + " Order by M_Detail ";

                    cgb_Pop.db_grid_Popup_Base(2, "배송_코드", "배송_구분", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, Tsql);
                    cgb_Pop.Next_Focus_Control = txt_Get_Name1;
                }


                

                if (tb.Name == "txt_ItemCode")
                {
                    string Tsql;
                    Tsql = "Select Name , NCode  ,price2 , price4  ";
                    Tsql = Tsql + " From ufn_Good_Search_Web ('" + mtxtSellDate.Text.Replace("-", "").Trim() + "','" + idx_Na_Code + "') ";
                    Tsql = Tsql + " Where NCode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%'";

                    cgb_Pop.db_grid_Popup_Base(4, "상품명", "상품코드", "개별단가", "개별PV", "Name", "Ncode", "price2", "price4", Tsql);

                    cgb_Pop.Next_Focus_Control = txt_ItemCount;
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
                    Tsql = Tsql + " Where (BankPenName like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    BankCode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%')";
                    if (idx_Na_Code != "") Tsql = Tsql + " And   Na_Code = '" + idx_Na_Code + "'"; 

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
                    if (idx_Na_Code != "") Tsql = Tsql + " And   Na_Code = '" + idx_Na_Code + "'"; 

                    cgb_Pop.db_grid_Popup_Base(2, "카드_코드", "카드명"
                                                , "ncode", "CardName"
                                                , Tsql);

                    cgb_Pop.Next_Focus_Control = txt_C_Name_3;

                }
            }
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
                Tsql = Tsql + " Where (BankPenName like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankCode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%')";
                if (idx_Na_Code != "") Tsql = Tsql + " And   Na_Code = '" + idx_Na_Code + "'"; 
            }


            if (tb.Name == "txt_C_Card")
            {
                Tsql = "Select  Ncode, cardname   ";
                Tsql = Tsql + " From tbl_Card (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    cardname like '%" + tb.Text.Trim() + "%')";
                if (idx_Na_Code != "") Tsql = Tsql + " And   Na_Code = '" + idx_Na_Code + "'"; 
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

            Mbid = T_Mbid;
            //if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == -1) //올바르게 회원번호 양식에 맞춰서 입력햇는가.
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
            //            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
            //           + "\n" +
            //           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    m_tb.Focus(); return false;
            //}

            string Tsql = "";
            Tsql = "Select tbl_User.User_Ncode NCode, tbl_User.U_Name N_Name ";
            Tsql = Tsql + " , tbl_User.Leave_TF  ";
            Tsql = Tsql + " , tbl_User.phone , tbl_User.Na_Code ";
            //Tsql = Tsql + " , tbl_User.ZipCode  , tbl_User.add1  ";
            //Tsql = Tsql + " , tbl_User.add2   ";
            Tsql = Tsql + " From tbl_User (nolock) ";

            Tsql = Tsql + " Where tbl_User.User_Ncode = '" + Mbid.ToString() + "'";

            Tsql = Tsql + " And  tbl_User.Leave_TF  = 0 ";
            //Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            Tsql = Tsql + " And tbl_User.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

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
                //if (Check_Leave_TF ==1)
                //{
                //    if (txt_OrderNumber.Text == "") //신규 저장건에 한해서.
                //    {
                //        //주문할려고 하는 회원이 탈퇴 회원이다
                //        if (ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString() != "") 
                //        {

                //           MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Leave_Sell")                       
                //           + "\n" +
                //           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                //            m_tb.Focus(); return false;
                //        }
                //    }
                //}
            }
            //++++++++++++++++++++++++++++++++            

            return true;
        }



        private void _From_Data_Clear()
        {
            Form_Key_Real_TF++;
            
            if (Form_Key_Real_TF > 1)
                return; 
            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset(1);            
            
            //dGridView_Base_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Item.d_Grid_view_Header_Reset();

            //dGridView_Base_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Cacu.d_Grid_view_Header_Reset();

            //dGridView_Base_Rece_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Rece.d_Grid_view_Header_Reset();

            //dGridView_Base_Rece_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Rece_Item.d_Grid_view_Header_Reset();
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


            InsuranceNumber_Ord_Print_FLAG = "";
           
            txtName.ReadOnly = false;
            txtName.BackColor = SystemColors.Window;
            txtName.BorderStyle = BorderStyle.Fixed3D;

            //txtName.ReadOnly = true;
            //txtName.BackColor = cls_app_static_var.txt_Enable_Color;
            //txtName.BorderStyle = BorderStyle.FixedSingle;

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtMbid);

            Base_Ord_Clear();

            Reset_Chart_Total(); // 차트 관련해서 리셋을 시킨다.
            tab_Sell.SelectedIndex = 0;

            mtxtSn.Mask = "999999-9999999";
            idx_Mbid = ""; idx_Mbid2 = 0; idx_Na_Code = "";
            radioB_M1.Checked = true; 
            

            mtxtMbid.Focus();            
        }


        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
                        

            if (bt.Name == "butt_Clear")
            {
                
                _From_Data_Clear();
                Form_Key_Real_TF = 0;                
                mtxtMbid.Focus();
            }


            else if (bt.Name == "butt_Save")
            {
                //if (cls_app_static_var.Sell_Union_Flag == "D")
                //{
                //    cls_Socket csg = new cls_Socket();
                //    //csg.Dir_Connect_Send_Acc("2014032700000003");
                //    //csg.Dir_Connect_Send_Cancel("2014032700000003");
                    
                //    return; 
                //}

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
            
            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }

            else if (bt.Name == "butt_Delete")
            {
                int Delete_Error_Check = 0;

                //if (cls_User.gid_Sell_Del_TF == 0) //주문취소 권한이 있는 사람만 가능하다.
                //{
                //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Del_Not_TF")                       
                //      + "\n" +
                //      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                //    return;
                //}

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
        }





        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Delete_Base_Data(ref int Delete_Error_Check)
        {
            Delete_Error_Check = 0;

            //주문종류 , 회원, 주문일자 입력 안햇는지 체크
            if (Check_Delete_TextBox_Error() == false) return;            
                      
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;                                          
         
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                string StrSql = "";
                //StrSql = "EXEC Usp_Insert_tbl_Sales_CanCel_CS '" + txt_OrderNumber.Text + "','" + cls_User.gid + "',0"  ;
                StrSql = "EXEC Usp_Insert_tbl_Sales_CanCel_CS__02 '" + txt_OrderNumber.Text + "','" + cls_User.gid + "',0";
                
             
                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);


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


            if (txt_OrderNumber.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Sell_OrderNumber")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridView_Base.Focus();
                return false;
            }

            string Ord_N = txt_OrderNumber.Text.Trim();

            //현 내역으로 연관되서 반품이나 교환한 내역이 잇다.
            foreach (string t_key in SalesDetail.Keys)
            {
                if (SalesDetail[t_key].Del_TF != "D")
                {                    
                    if (SalesDetail[t_key].Re_BaseOrderNumber == Ord_N)
                    {
                        if (SalesDetail[t_key].ReturnTF == 2)
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Del_2")
                            + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            mtxtSellDate.Focus(); return false;
                        }
                        if (SalesDetail[t_key].ReturnTF == 3)
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Del_3")
                            + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            mtxtSellDate.Focus(); return false;
                        }

                        if (SalesDetail[t_key].ReturnTF == 4)
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Del_4")
                            + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            mtxtSellDate.Focus(); return false;
                        }                        
                    }
                }
            }


            if (SalesDetail[Ord_N].ReturnTF.ToString() == "2")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_2")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtSellDate.Focus(); return false;
            }

            if (SalesDetail[Ord_N].ReturnTF.ToString () == "3")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_3")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtSellDate.Focus(); return false;
            }

            if (SalesDetail[Ord_N].ReturnTF.ToString() == "4")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_4")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtSellDate.Focus(); return false;
            }


            //공제번호가 있으면 삭제가 안되게 한다. 우선 먼저 공제번호를 취소한후에 다시 시도하게 한다.
            cls_form_Meth cm = new cls_form_Meth ();
            if (SalesDetail[Ord_N].INS_Num != "" && cls_app_static_var.Sell_Union_Flag == "U" && SalesDetail[Ord_N].INS_Num != cm._chang_base_caption_search("미신고"))
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Chang_Insur_Number")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                butt_Delete.Focus(); return false;

            }
            ////공제번호가 있으면 삭제가 안되게 한다. 우선 먼저 공제번호를 취소한후에 다시 시도하게 한다.
            //cls_form_Meth cm = new cls_form_Meth();
            //if (txt_Ins_Number.Text.Trim() != "" && txt_Ins_Number.Text.Trim() != cm._chang_base_caption_search("미신고"))
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Chang_Insur_Number")
            //            + "\n" +
            //            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    butt_Delete.Focus(); return false;
                
            //}



            cls_Search_DB csd = new cls_Search_DB();        

            //마감정산이 이루어진 판매 날짜인지 체크한다.                
            if (csd.Close_Check_SellDate("tbl_CloseTotal_04", SalesDetail[Ord_N].SellDate.Replace("-","") ) == false)
            {
                mtxtSellDate.Focus(); return false;
            }



            //재고 관련해서 출고가 된내역인지 확인한다 출고 되었으면 삭제 되면 안됨.
            if (csd.Check_Stock_OutPut(txt_OrderNumber.Text.Trim()) == false)
            {
                butt_Delete.Focus(); return false;
            }


            

            return true;
        }






        private bool Base_Error_Check__01(int SellCode_TF = 0)
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


            if (mtxtSellDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate.Text, mtxtSellDate, "Date") == false)
                {
                    mtxtSellDate.Focus();
                    return false;
                }
                

            }           
            else
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_SellDate")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtSellDate.Focus(); return false;
            }



            if (mtxtSellDate.Text.Replace("-", "").Trim() != "" && mtxtSellDate2.Text.Replace("-", "").Trim() == "")
            {
                mtxtSellDate2.Text = mtxtSellDate.Text;
            }


            //주문일자를 넣었는지 먼저 체크한다. 안넣었으면 넣어라.
            if (mtxtSellDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate2.Text, mtxtSellDate2, "Date") == false)
                {
                    mtxtSellDate2.Focus();
                    return false;
                }
               
            }
            else
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_SellDate2")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtSellDate2.Focus(); return false;
            }



            
            //////주문종류를 선택 안햇네 그럼 그것도 넣어라.
            ////if (txtSellCode_Code.Text == "" && SellCode_TF == 0 )
            ////{
            ////    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
            ////           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_SellCode")
            ////          + "\n" +
            ////          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            ////    txtSellCode.Focus(); return false;
            ////}

            



            return true; 
        }





        private bool Base_Error_Check_Not_Sellcode__01()
        {
            //주문일자를 넣었는지 먼저 체크한다. 안넣었으면 넣어라.
            if (mtxtSellDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate2.Text, mtxtSellDate2, "Date") == false)
                {
                    mtxtSellDate2.Focus();
                    return false;
                }
                int Ret = 0;
                cls_Check_Input_Error c_er = new cls_Check_Input_Error();
                Ret = c_er.Input_Date_Err_Check(mtxtSellDate,1);

                if (Ret == -1)
                {
                    mtxtSellDate.Focus(); return false;
                }
            }
            else
            {               
                mtxtSellDate.Focus(); return false;
            }

            
            return true;
        }


        private bool Item_Rece_Error_Check__01(string s_Tf)
        {
            if (s_Tf == "item")
            {
                //상품은 선택 안햇네 그럼 그것도 넣어라.
                if (txt_ItemName.Text == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_ItemCode.Focus(); return false;
                }


                //주문수량을 입력 안햇네 그럼 그것도 넣어라.
                if (txt_ItemCount.Text == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Count")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_ItemCount.Focus(); return false;
                }


                //주문수량을 0  입력햇네  그럼 제대로 넣어라.
                if (int.Parse(txt_ItemCount.Text.Trim()) == 0)
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Count")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_ItemCount.Focus(); return false;
                }
            }

            if (s_Tf == "Rece")
            {
                //배송구분 선택 안햇네 그럼 그것도 넣어라.
                if (txt_Receive_Method_Code.Text == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Rece")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Receive_Method.Focus(); return false;
                }

                cls_Check_Input_Error c_er = new cls_Check_Input_Error();
                if (txtGetDate1.Text.Trim() != "")
                {
                    int Ret = 0;
                    Ret = c_er.Input_Date_Err_Check(txtGetDate1);

                    if (Ret == -1)
                    {
                        txtGetDate1.Focus(); return false;
                    }
                }



                string Sn = mtxtTel1.Text.Replace("-", "").Replace("_", "").Trim();
                if (Sn_Number_(Sn, mtxtTel1, "Tel") == false)
                {
                    mtxtTel1.Focus();
                    return false;
                }

                Sn = mtxtTel2.Text.Replace("-", "").Replace("_", "").Trim();
                if (Sn_Number_(Sn, mtxtTel2, "Tel") == false)
                {
                    mtxtTel2.Focus();
                    return false;
                }

                Sn = mtxtZip1.Text.Replace("-", "").Replace("_", "").Trim();
                if (Sn_Number_(Sn, mtxtZip1, "Zip") == false)
                {
                    mtxtZip1.Focus();
                    return false;
                }



                int chk_cnt = 0;

                for (int i = 0; i <= dGridView_Base_Rece_Item.Rows.Count - 1; i++)
                {
                    if (dGridView_Base_Rece_Item.Rows[i].Cells[0].Value.ToString() == "V")
                    {                        
                        chk_cnt++;
                    }
                }

                if (chk_cnt == 0)
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select") + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    dGridView_Base_Rece_Item.Focus(); return false;
                }

                
            }

            if (s_Tf == "Cacu")
            {
                if (Item_Rece_Error_Check__02() == false) return false;
            }

            return true; 
        }



        private bool Item_Rece_Error_Check__02()
        {
            if (dGridView_Base_Item.RowCount == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_ItemCode.Focus(); return false;
            }

            if (txt_Price_1.Text.Trim() == "") txt_Price_1.Text = "0";
            if (txt_Price_2.Text.Trim() == "") txt_Price_2.Text = "0";
            if (txt_Price_3.Text.Trim() == "") txt_Price_3.Text = "0";
            if (txt_Price_4.Text.Trim() == "") txt_Price_4.Text = "0";

            if (double.Parse(txt_Price_1.Text.Trim().Replace (",","") ) == 0
                    && double.Parse(txt_Price_2.Text.Trim().Replace(",", "")) == 0
                    && double.Parse(txt_Price_3.Text.Trim().Replace(",", "")) == 0
                    && double.Parse(txt_Price_4.Text.Trim().Replace(",", "")) == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_Price_1.Focus(); return false;
            }
            
            //주문일자를 넣었는지 먼저 체크한다. 안넣었으면 넣어라.
            if (mtxtPriceDate1.Text.Replace("-", "").Trim()  != "")
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


            if (mtxtPriceDate4.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtPriceDate4.Text, mtxtPriceDate4, "Date") == false)
                {
                    mtxtPriceDate4.Focus();
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

            if (double.Parse(txt_Price_4.Text) != 0)
            {
                if (mtxtPriceDate4.Text.Replace("-", "").Trim() == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_AppDate")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtPriceDate4.Focus(); return false;
                }
            }


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




            if (double.Parse(txt_Price_1.Text) == 0)
            {
                if (mtxtPriceDate1.Text.Replace("-", "").Trim() != "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price_1")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Price_1.Focus(); return false;
                }
            }

           
          

            if (double.Parse(txt_Price_3.Text) == 0)
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


            if (double.Parse(txt_Price_4.Text) == 0)
            {
                if (mtxtPriceDate4.Text.Replace("-", "").Trim() != "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price_4")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Price_4.Focus(); return false;
                }
            }


            if (txt_C_index.Text != "") // 수정일 경우에는 카드나 현금 무통장 동시에 못넣게 한다.
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

                if (txt_Price_1.Text != "0" && txt_Price_4.Text != "0")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Input_Same_Not")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price_1_4")
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

                if (txt_Price_2.Text != "0" && txt_Price_4.Text != "0")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Input_Same_Not")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price_2_4")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Price_2.Focus(); return false;
                }

                if (txt_Price_3.Text != "0" && txt_Price_4.Text != "0")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Input_Same_Not")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Cacu_Price_3_4")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Price_3.Focus(); return false;
                }
            }

            if (txt_Price_3_2.Text.Trim() == "") txt_Price_3_2.Text = "0"; 

            if (double.Parse(txt_Price_4.Text) > 0)
            {
                if (Mileage_Error_Check__01() == false)
                {
                    txt_Price_4.Focus(); 
                    return false;
                }
            }

            return true; 
        }

        private bool Mileage_Error_Check__01()
        {
            if (txt_SumMile.Text == "")
                txt_SumMile.Text = "0";

            double Using_M = double.Parse(txt_Price_4_2.Text.Replace(",", ""));  //사용가능 마일리지
            double U_M = double.Parse(txt_Price_4.Text.Replace(",", ""));        //현재 결제한 마일리지
            double U_Order_M = double.Parse(txt_SumMile.Text.Replace(",", ""));        //이번 주문번호에서 이미 사용한 마일리지
            double Or_U_Order_M = 0; //이주문번호로 해서 이전에 사용한 마일리지

            if (txt_OrderNumber.Text == "") //새롭게 만들어지는 내역이다.
            {
                if (Using_M < (U_M + U_Order_M))
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Over_Point")
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Price_4.Focus(); return false;
                }
            }
            else
            {
                cls_tbl_Mileage ctm =new cls_tbl_Mileage () ;
                Or_U_Order_M = ctm.Using_Mileage_Search(txt_OrderNumber.Text); //이주문으로 해서 사용한 마일리지를 불러온다.

                if ((Using_M + Or_U_Order_M) < (U_M + U_Order_M)) //사용가능 마일 + 이주문으로 이전까지 해서 사용한 마일    이   이번에 사용된 마일리지보다 크다.. 그럼 초과임.
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Over_Point")
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Price_4.Focus(); return false;
                }

            }

            return true;
        }

        private void Base_Ord_Clear()
        {
            

            //dGridView_Base_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Cacu.d_Grid_view_Header_Reset(1);

            //dGridView_Base_Rece_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Rece.d_Grid_view_Header_Reset(1);

            //dGridView_Base_Rece_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Rece_Item.d_Grid_view_Header_Reset(1);

            Data_Set_Form_TF = 1;
            if (SalesItemDetail !=null )
                SalesItemDetail.Clear();
            if (Sales_Rece != null)
                Sales_Rece.Clear();
            if (Sales_Cacu != null)
                Sales_Cacu.Clear();

            Base_Sub_Clear("item");
            Base_Sub_Clear("Rece");
            Base_Sub_Clear("Cacu");

            tab_Cacu.SelectedIndex = 0;

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(panel8, mtxtSellDate);

            txt_OrderNumber.Text = ""; txt_TotalPv.Text = ""; txt_Ins_Number.Text = ""; txt_TotalCV.Text = "";
            txt_TotalPrice.Text = ""; txt_TotalInputPrice.Text = ""; txt_UnaccMoney.Text = "";

            txt_SumCash.Text = ""; txt_SumCard.Text = ""; txt_SumBank.Text = ""; txt_SumMile.Text = "";
            txt_SumCnt.Text = ""; txt_SumPr.Text = ""; txt_SumPV.Text = ""; txt_SumCV.Text = "";
            Data_Set_Form_TF = 0;

            

            mtxtSellDate.Text = cls_User.gid_date_time; 

            
        }



        private void Base_Sub_Clear(string s_Tf)
        {
            cls_form_Meth ct = new cls_form_Meth();
            
            if (s_Tf == "item")
            {
                dGridView_Base_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_Item.d_Grid_view_Header_Reset(1);

             


                ct.from_control_clear(groupBox7, txt_ItemCode);
                butt_Item_Del.Visible = false;
                txt_ItemCode.ReadOnly = false;
                txt_ItemCode.BorderStyle = BorderStyle.Fixed3D;
                txt_ItemCode.BackColor = SystemColors.Window ;

               
                   
                
                butt_Item_Save.Text = ct._chang_base_caption_search("추가");

                if (SalesItemDetail != null)
                    Item_Grid_Set(); //상품 그리드

                txt_ItemCode.Focus();
            }

            if (s_Tf == "Rece")
            {
                dGridView_Base_Rece_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_Rece_Item.d_Grid_view_Header_Reset(1);

                ct.from_control_clear(groupBox1, txt_Receive_Method);
                butt_Rec_Del.Visible = false;
                butt_Rec_Save.Text = ct._chang_base_caption_search("추가");

                if (Sales_Rece != null)
                    Rece_Grid_Set(); //배송 그리드

                txt_Receive_Method.Focus();

            }


            if (s_Tf == "Cacu")
            {
                //cls_form_Meth ct = new cls_form_Meth();
                dGridView_Base_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_Cacu.d_Grid_view_Header_Reset(1);

                if (combo_C_Card_Year.SelectedIndex >=0)
                    combo_C_Card_Year.SelectedIndex = 0;
                if (combo_C_Card_Month.SelectedIndex >= 0)
                    combo_C_Card_Month.SelectedIndex = 0;
                if (combo_C_Card_Per.SelectedIndex >= 0)
                    combo_C_Card_Per.SelectedIndex = 0;

                ct.from_control_clear(tab_Cacu, txt_Price_1);
                txt_C_Etc.Text = "";

                if (mtxtSellDate.Text.Replace("-","").Trim()  != "")
                {
                    if (Base_Error_Check_Not_Sellcode__01() == false)  return;  //주문종류 , 회원, 주문일자 입력 안햇는지 체크
                    
                    double T_p = 0;
                    string T_Mbid = mtxtMbid.Text;
                    string Mbid = ""; int Mbid2 = 0;
                    //cls_Search_DB csb = new cls_Search_DB();
                    //if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == 1)
                    //{
                    //    cls_tbl_Mileage ctm = new cls_tbl_Mileage();
                    //    T_p = ctm.Using_Mileage_Search(Mbid, Mbid2, mtxtSellDate.Text.Replace("-", "").Trim());
                    //    txt_Price_4_2.Text = string.Format(cls_app_static_var.str_Currency_Type, T_p);
                    //}
                }


                butt_Cacu_Del.Visible = false;
                butt_Cacu_Save.Text = ct._chang_base_caption_search("추가");
                tab_Cacu.SelectedIndex = 0; 

                if (Sales_Cacu != null)
                    Cacu_Grid_Set(); //배송 그리드

                txt_Price_1.Focus();
            }


        }

        private void Base_Sub_Delete(string s_Tf)
        {
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            cls_form_Meth ct = new cls_form_Meth();
            
            if (s_Tf == "item")
            {
                //주문 상품 관련 딕셔너리에서 찾아서.. 삭제 표식을 해놓는다.
                SalesItemDetail[int.Parse(txt_SalesItemIndex.Text)].Del_TF = "D";

                //배송 관련 정보도 들어간게 있으면 삭제를 표식해 버린다..
                if (Sales_Rece.ContainsKey(int.Parse(txt_SalesItemIndex.Text)) == true)
                    Sales_Rece[int.Parse(txt_SalesItemIndex.Text)].Del_TF = "D";

                ct.from_control_clear(groupBox7, txt_ItemCode);
                butt_Item_Del.Visible = false;
                txt_ItemCode.ReadOnly = false;
                txt_ItemCode.BackColor = SystemColors.Window; 
                butt_Item_Save.Text = ct._chang_base_caption_search("추가");

                if (SalesItemDetail != null)
                    Item_Grid_Set(); //상품 그리드 

                if (Sales_Rece != null)
                    Rece_Grid_Set(); //상품 그리드  
            }

            if (s_Tf == "Rece")
            {
                //주문 상품 관련 딕셔너리에서 찾아서.. 삭제 표식을 해놓는다.
                Sales_Rece[int.Parse(txt_RecIndex.Text)].Del_TF = "D";

                //상품관련 딕셔너리에서 배송 날짜와 배송 인덱스를 없앤다.
                SalesItemDetail[int.Parse(txt_RecIndex.Text)].SendDate = "";
                SalesItemDetail[int.Parse(txt_RecIndex.Text)].RecIndex = 0;

                ct.from_control_clear(panel2, txt_Receive_Method);
                chk_Total.Checked = false; 
                butt_Rec_Del.Visible = false;
                butt_Rec_Save.Text = ct._chang_base_caption_search("추가");

                if (Sales_Rece != null)
                    Rece_Grid_Set(); //상품 그리드  

                if (SalesItemDetail != null)
                    Item_Grid_Set(); //상품 그리드     
            }

            if (s_Tf == "Cacu")
            {
                //결제 관련 딕셔너리에서 찾아서.. 삭제 표식을 해놓는다.
                Sales_Cacu[int.Parse(txt_C_index.Text)].Del_TF = "D";
                
                ct.from_control_clear(panel_Cacu, txt_Price_1);
                butt_Cacu_Del.Visible = false;
                butt_Cacu_Save.Text = ct._chang_base_caption_search("추가");

                if (Sales_Cacu != null)
                    Cacu_Grid_Set(); //상품 그리드  
                                
                
                double T_p = 0;
                string T_Mbid = mtxtMbid.Text;
                string Mbid = ""; int Mbid2 = 0;
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == 1)
                {
                    cls_tbl_Mileage ctm = new cls_tbl_Mileage();
                    T_p = ctm.Using_Mileage_Search(Mbid, Mbid2, mtxtSellDate.Text.Replace("-", "").Trim());
                    txt_Price_4_2.Text = string.Format(cls_app_static_var.str_Currency_Type, T_p);
                }
            }


            //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del")
            //       + "\n" +
            //       cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Save"));
        }



        private void Base_Sub_Save_Item()
        {
            cls_form_Meth ct = new cls_form_Meth();

            int New_SalesItemIndex = 0;
            if (SalesItemDetail != null)
            {
                foreach (int t_key in SalesItemDetail.Keys)
                {
                    if (New_SalesItemIndex < t_key)
                        New_SalesItemIndex = t_key;
                }
            }
            New_SalesItemIndex = New_SalesItemIndex + 1;
                        
            cls_Sell_Item t_c_sell = new cls_Sell_Item();

            t_c_sell.OrderNumber = txt_OrderNumber.Text.Trim () ;
            t_c_sell.SalesItemIndex = New_SalesItemIndex;

            t_c_sell.ItemCode = txt_ItemCode.Text.Trim();
            t_c_sell.ItemName = txt_ItemName.Text.Trim();
            t_c_sell.ItemCount = int.Parse(txt_ItemCount.Text.Trim());

            t_c_sell.SellState = "N_1"; //정상:N_1  반품:R_1  교환나간거:N_3   교환들어온거:R_3
            t_c_sell.SellStateName =  ct._chang_base_caption_search("정상");
            t_c_sell.Sell_VAT_TF = 0;

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

           
            
            string Tsql = "";
            Tsql = "Select price2 ,price4 , price5 , price6 ";
            Tsql = Tsql + " , Sell_VAT_Price , Except_Sell_VAT_Price   ";
            Tsql = Tsql + " From ufn_Good_Search_Web_Sell ('" + mtxtSellDate.Text.Replace("-", "").Trim() + "','" + idx_Na_Code + "','3') "; //3번은 직원가기준
            Tsql = Tsql + " Where NCode = '" + txt_ItemCode.Text.Trim() + "'";                
            
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

          
            t_c_sell.ItemPrice = double.Parse(ds.Tables["t_P_table"].Rows[0]["price2"].ToString());
           

            t_c_sell.ItemPV = double.Parse(ds.Tables["t_P_table"].Rows[0]["price4"].ToString());
            t_c_sell.ItemCV = double.Parse(ds.Tables["t_P_table"].Rows[0]["price5"].ToString());
            t_c_sell.Sell_VAT_Price = double.Parse(ds.Tables["t_P_table"].Rows[0]["Sell_VAT_Price"].ToString());
            t_c_sell.Sell_Except_VAT_Price = double.Parse(ds.Tables["t_P_table"].Rows[0]["Except_Sell_VAT_Price"].ToString());
            //++++++++++++++++++++++++++++++++

            t_c_sell.ItemTotalPrice = t_c_sell.ItemPrice * t_c_sell.ItemCount;
            t_c_sell.ItemTotalPV = t_c_sell.ItemPV * t_c_sell.ItemCount;
            t_c_sell.ItemTotalCV = t_c_sell.ItemCV * t_c_sell.ItemCount;
            t_c_sell.Total_Sell_VAT_Price = t_c_sell.Sell_VAT_Price * t_c_sell.ItemCount;
            t_c_sell.Total_Sell_Except_VAT_Price = t_c_sell.Sell_Except_VAT_Price * t_c_sell.ItemCount;
                               
            t_c_sell.ReturnDate = "";
            t_c_sell.SendDate = "";
            t_c_sell.ReturnBackDate = "";
            t_c_sell.Etc = txt_Item_Etc.Text.Trim();
            t_c_sell.RecIndex = 0;
            t_c_sell.Send_itemCount1 = 0;
            t_c_sell.Send_itemCount2 = 0;
            t_c_sell.T_OrderNumber1 = txt_OrderNumber.Text.Trim();
            t_c_sell.T_OrderNumber2 = "";
            t_c_sell.Real_index = 0;
            t_c_sell.G_Sort_Code = "";

            t_c_sell.RecordID = cls_User.gid;
            t_c_sell.RecordTime = "";

            t_c_sell.Del_TF = "S";            
            SalesItemDetail[New_SalesItemIndex] = t_c_sell;

            
            ct.from_control_clear((Panel)txt_ItemCode.Parent, txt_ItemCode);
            butt_Item_Del.Visible = false;
            butt_Item_Save.Text = ct._chang_base_caption_search("추가");

            if (SalesItemDetail != null)
                Item_Grid_Set(); //상품 그리드               


            //if (Save_Button_Click_Cnt == 1)
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save")
            //                + "\n" +
            //    cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Save"));
            //}
            
        }


        private void Base_Sub_Edit_Item()
        {
            cls_form_Meth ct = new cls_form_Meth();

            int SalesItemIndex = int.Parse (txt_SalesItemIndex.Text );
            
            SalesItemDetail[SalesItemIndex].ItemCode = txt_ItemCode.Text.Trim();
            SalesItemDetail[SalesItemIndex].ItemName = txt_ItemName.Text.Trim();
            SalesItemDetail[SalesItemIndex].ItemCount = int.Parse(txt_ItemCount.Text.Trim());
            SalesItemDetail[SalesItemIndex].Etc = txt_Item_Etc.Text.Trim();

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            
            string Tsql = "";
            Tsql = "Select price2 ,price4 , price5 , price6 ";
            Tsql = Tsql + " , Sell_VAT_Price , Except_Sell_VAT_Price   ";
            Tsql = Tsql + " From ufn_Good_Search_Web ('" + mtxtSellDate.Text.Replace("-", "").Trim() + "','" + idx_Na_Code + "') ";
            Tsql = Tsql + " Where NCode = '" + txt_ItemCode.Text.Trim() + "'";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

           
            SalesItemDetail[SalesItemIndex].ItemPrice = double.Parse(ds.Tables["t_P_table"].Rows[0]["price6"].ToString());            

            SalesItemDetail[SalesItemIndex].ItemPV = double.Parse(ds.Tables["t_P_table"].Rows[0]["price4"].ToString());
            SalesItemDetail[SalesItemIndex].ItemCV = double.Parse(ds.Tables["t_P_table"].Rows[0]["price5"].ToString());
            SalesItemDetail[SalesItemIndex].Sell_VAT_Price = double.Parse(ds.Tables["t_P_table"].Rows[0]["Sell_VAT_Price"].ToString());
            SalesItemDetail[SalesItemIndex].Sell_Except_VAT_Price = double.Parse(ds.Tables["t_P_table"].Rows[0]["Except_Sell_VAT_Price"].ToString());
            //++++++++++++++++++++++++++++++++

            SalesItemDetail[SalesItemIndex].ItemTotalPrice = SalesItemDetail[SalesItemIndex].ItemPrice * SalesItemDetail[SalesItemIndex].ItemCount;
            SalesItemDetail[SalesItemIndex].ItemTotalPV = SalesItemDetail[SalesItemIndex].ItemPV * SalesItemDetail[SalesItemIndex].ItemCount;
            SalesItemDetail[SalesItemIndex].ItemTotalCV = SalesItemDetail[SalesItemIndex].ItemCV * SalesItemDetail[SalesItemIndex].ItemCount;
            SalesItemDetail[SalesItemIndex].Total_Sell_VAT_Price = SalesItemDetail[SalesItemIndex].Sell_VAT_Price * SalesItemDetail[SalesItemIndex].ItemCount;
            SalesItemDetail[SalesItemIndex].Total_Sell_Except_VAT_Price = SalesItemDetail[SalesItemIndex].Sell_Except_VAT_Price * SalesItemDetail[SalesItemIndex].ItemCount;
                               

            if (SalesItemDetail[SalesItemIndex].Del_TF =="")
                SalesItemDetail[SalesItemIndex].Del_TF = "U";  //업데이트 되엇다고 표시한다.

            ct.from_control_clear((Panel)txt_ItemCode.Parent, txt_ItemCode);
            butt_Item_Del.Visible = false;
            butt_Item_Save.Text = ct._chang_base_caption_search("추가");

            if (SalesItemDetail != null)
                Item_Grid_Set(); //상품 그리드    

            //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit")
            //             + "\n" +
            //cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Save"));
        }



        private void Base_Sub_Save_Rece(int New_SalesItemIndex)
        {
            
                        
            cls_Sell_Rece t_c_sell = new cls_Sell_Rece();

            t_c_sell.OrderNumber = txt_OrderNumber.Text.Trim();
            t_c_sell.SalesItemIndex = New_SalesItemIndex;
            t_c_sell.RecIndex = New_SalesItemIndex;
            t_c_sell.Get_Name1 = txt_Get_Name1.Text.Trim();
            t_c_sell.Get_Name2 = "";

            t_c_sell.Receive_Method = int.Parse(txt_Receive_Method_Code.Text.Trim());
            t_c_sell.Receive_Method_Name = txt_Receive_Method.Text.Trim();
           
            string t_sellDate = "";
            t_c_sell.Get_Date1 = "";
            t_c_sell.Get_Date2 = "";

            if (txtGetDate1.Text.Trim ()  != "")
            {
                t_sellDate = txtGetDate1.Text.Trim ().Substring(0, 4);
                t_sellDate = t_sellDate + "-" + txtGetDate1.Text.Trim ().Substring(4, 2);
                t_sellDate = t_sellDate + "-" + txtGetDate1.Text.Trim ().Substring(6, 2);

                t_c_sell.Get_Date1 = t_sellDate;
            }

            string Get_Tel1 = ""; string Get_Tel2 = "";
            if (mtxtTel1.Text.Replace("-", "").Trim() != "") Get_Tel1 = mtxtTel1.Text.Trim();
            if (mtxtTel2.Text.Replace("-", "").Trim() != "") Get_Tel2 = mtxtTel2.Text.Trim();

            t_c_sell.Get_Tel1 = Get_Tel1;
            t_c_sell.Get_Tel2 = Get_Tel2;

            t_c_sell.Get_ZipCode = "";
            t_c_sell.Get_Address1 = "";
            t_c_sell.Get_Address2 = "";
                        
            if (mtxtZip1.Text.Replace ("-","").Trim() != "")
                t_c_sell.Get_ZipCode =mtxtZip1.Text.Replace ("-","") ;

            if (txtAddress1.Text.Trim() != "")
                t_c_sell.Get_Address1 = txtAddress1.Text.Trim();

            if (txtAddress2.Text.Trim() != "")
                t_c_sell.Get_Address2  = txtAddress2.Text.Trim() ;

            t_c_sell.Get_Etc1 = txt_Get_Etc1.Text.Trim();
            t_c_sell.Get_Etc2 = "";
            t_c_sell.Pass_Number = txt_Pass_Number.Text.Trim();
            t_c_sell.Base_Rec_Name  = txt_Base_Rec.Text.Trim();
            t_c_sell.Base_Rec = txt_Base_Rec_Code.Text.Trim();
            
            t_c_sell.RecordID = cls_User.gid;
            t_c_sell.RecordTime = "";

            t_c_sell.Del_TF = "S";
            Sales_Rece[New_SalesItemIndex] = t_c_sell;


            SalesItemDetail[New_SalesItemIndex].RecIndex = New_SalesItemIndex;
            SalesItemDetail[New_SalesItemIndex].SendDate = txtGetDate1.Text.Trim();


            
        }

        private void Base_Sub_Edit_Rece()
        {
           int SalesItemIndex = int.Parse(txt_RecIndex.Text);

            Sales_Rece[SalesItemIndex].Get_Name1 = txt_Get_Name1.Text.Trim();
            Sales_Rece[SalesItemIndex].Receive_Method = int.Parse(txt_Receive_Method_Code.Text.Trim());
            Sales_Rece[SalesItemIndex].Receive_Method_Name = txt_Receive_Method.Text.Trim();
            

            string t_sellDate = "";
            Sales_Rece[SalesItemIndex].Get_Date1 = "";
            if (txtGetDate1.Text.Trim() != "")
            {
                t_sellDate = txtGetDate1.Text.Trim().Substring(0, 4);
                t_sellDate = t_sellDate + "-" + txtGetDate1.Text.Trim().Substring(4, 2);
                t_sellDate = t_sellDate + "-" + txtGetDate1.Text.Trim().Substring(6, 2);

                Sales_Rece[SalesItemIndex].Get_Date1 = t_sellDate;
            }
           
            string Get_Tel1 = ""; string Get_Tel2 = "";
            if (mtxtTel1.Text.Replace("-", "").Trim() != "") Get_Tel1 = mtxtTel1.Text.Trim();
            if (mtxtTel2.Text.Replace("-", "").Trim() != "") Get_Tel2 = mtxtTel2.Text.Trim();

            Sales_Rece[SalesItemIndex].Get_Tel1 = Get_Tel1;
            Sales_Rece[SalesItemIndex].Get_Tel2 = Get_Tel2;

            Sales_Rece[SalesItemIndex].Get_ZipCode = "";
            Sales_Rece[SalesItemIndex].Get_Address1 = "";
            Sales_Rece[SalesItemIndex].Get_Address2 = "";

            if (mtxtZip1.Text.Replace("-", "").Trim() != "")
                Sales_Rece[SalesItemIndex].Get_ZipCode = mtxtZip1.Text.Replace("-", "");


            if (txtAddress1.Text.Trim() != "")
                Sales_Rece[SalesItemIndex].Get_Address1 = txtAddress1.Text.Trim();

            if (txtAddress2.Text.Trim() != "")
                Sales_Rece[SalesItemIndex].Get_Address2 = txtAddress2.Text.Trim();

            Sales_Rece[SalesItemIndex].Get_Etc1 = txt_Get_Etc1.Text.Trim();
            Sales_Rece[SalesItemIndex].Pass_Number = txt_Pass_Number.Text.Trim();
            Sales_Rece[SalesItemIndex].Base_Rec_Name = txt_Base_Rec.Text.Trim();
            Sales_Rece[SalesItemIndex].Base_Rec = txt_Base_Rec_Code.Text.Trim();

            if (Sales_Rece[SalesItemIndex].Del_TF == "")
                Sales_Rece[SalesItemIndex].Del_TF = "U";
            SalesItemDetail[SalesItemIndex].SendDate = txtGetDate1.Text.Trim();      
       }




        private void Base_Sub_Save_Cacu(int C_SF )
        {
            cls_form_Meth ct = new cls_form_Meth();
            int New_C_index = 0;
            if (Sales_Cacu != null)
            {
                foreach (int t_key in Sales_Cacu.Keys)
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
                t_c_sell.C_AppDate1 = mtxtPriceDate1.Text.Replace("-", "").Trim();              
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
                t_c_sell.C_AppDate1 = mtxtPriceDate3.Text.Trim();
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
            }

            if (C_SF == 4)
            {
                t_c_sell.C_TF = 4;
                t_c_sell.C_TF_Name = ct._chang_base_caption_search("마일리지");
                t_c_sell.C_Price1 = double.Parse(txt_Price_4.Text.Trim().Replace(",", ""));
                t_c_sell.C_AppDate1 = mtxtPriceDate4.Text.Replace("-", "").Trim();    
            }

            t_c_sell.RecordID = cls_User.gid;
            t_c_sell.RecordTime = "";

            t_c_sell.C_Etc = txt_C_Etc.Text.Trim();

            t_c_sell.Del_TF = "S";
            Sales_Cacu[New_C_index] = t_c_sell;
        }




        private void Base_Sub_Edit_Cacu()
        {
            cls_form_Meth ct = new cls_form_Meth();
            int C_index = int.Parse(txt_C_index.Text);
            Sales_Cacu[C_index].C_Etc = txt_C_Etc.Text.Trim();
            Sales_Cacu[C_index].C_TF = 0;
            Sales_Cacu[C_index].C_TF_Name = "";

            Sales_Cacu[C_index].C_Price1 = 0;
            Sales_Cacu[C_index].C_AppDate1 = "";
            Sales_Cacu[C_index].C_Name1 = "";
            Sales_Cacu[C_index].C_Code = "";
            Sales_Cacu[C_index].C_CodeName = "";
            Sales_Cacu[C_index].C_CodeName_2 = "";
            Sales_Cacu[C_index].C_Number1 = "";
            Sales_Cacu[C_index].C_Number2 = "";
            Sales_Cacu[C_index].C_Price2 = 0;
            Sales_Cacu[C_index].C_Period1 = "";
            Sales_Cacu[C_index].C_Period2 = "";
            Sales_Cacu[C_index].C_Installment_Period = "";


            if (double.Parse(txt_Price_1.Text.Trim()) > 0)  //현금이다
            {
                Sales_Cacu[C_index].C_TF = 1;
                Sales_Cacu[C_index].C_TF_Name =  ct._chang_base_caption_search("현금");
                Sales_Cacu[C_index].C_Price1 = double.Parse(txt_Price_1.Text.Trim().Replace(",", ""));
                Sales_Cacu[C_index].C_AppDate1 = mtxtPriceDate1.Text.Replace("-", "").Trim();
            }

            if (double.Parse(txt_Price_2.Text.Trim()) > 0)  //무통이다
            {
                Sales_Cacu[C_index].C_TF = 2;
                Sales_Cacu[C_index].C_TF_Name = ct._chang_base_caption_search("무통장");
                Sales_Cacu[C_index].C_Price1 = double.Parse(txt_Price_2.Text.Trim().Replace(",", ""));
                Sales_Cacu[C_index].C_AppDate1 = mtxtPriceDate2.Text.Replace("-", "").Trim();
                Sales_Cacu[C_index].C_Name1 = txt_C_Name_2.Text.Trim();
                Sales_Cacu[C_index].C_Code = txt_C_Bank_Code.Text.Trim();
                Sales_Cacu[C_index].C_CodeName = txt_C_Bank_Code_2.Text.Trim();
                Sales_Cacu[C_index].C_CodeName_2 = txt_C_Bank.Text.Trim();
                Sales_Cacu[C_index].C_Number1 = txt_C_Bank_Code_3.Text.Trim();    
            }

            if (double.Parse(txt_Price_3.Text.Trim()) > 0)  //카드이다
            {
                Sales_Cacu[C_index].C_TF = 3;
                Sales_Cacu[C_index].C_TF_Name = ct._chang_base_caption_search("카드");
                Sales_Cacu[C_index].C_Price1 = double.Parse(txt_Price_3.Text.Trim().Replace(",", ""));
                Sales_Cacu[C_index].C_AppDate1 = mtxtPriceDate3.Text.Trim();
                Sales_Cacu[C_index].C_Name1 = txt_C_Name_3.Text.Trim();
                Sales_Cacu[C_index].C_Code = txt_C_Card_Code.Text.Trim();
                Sales_Cacu[C_index].C_CodeName = txt_C_Card.Text.Trim();                
                Sales_Cacu[C_index].C_Number1 = txt_C_Card_Number.Text.Trim();
                Sales_Cacu[C_index].C_Number2 = txt_C_Card_Ap_Num.Text.Trim();
                Sales_Cacu[C_index].C_Price2 = double.Parse(txt_Price_3_2.Text.Trim());
                Sales_Cacu[C_index].C_Period1 = combo_C_Card_Year.Text.Trim();
                Sales_Cacu[C_index].C_Period2 = combo_C_Card_Month.Text.Trim();
                Sales_Cacu[C_index].C_Installment_Period = combo_C_Card_Per.Text.Trim();
            }

            if (double.Parse(txt_Price_4.Text.Trim()) > 0)  //현금이다
            {
                Sales_Cacu[C_index].C_TF = 4;
                Sales_Cacu[C_index].C_TF_Name = ct._chang_base_caption_search("마일리지");
                Sales_Cacu[C_index].C_Price1 = double.Parse(txt_Price_4.Text.Trim().Replace(",", ""));
                Sales_Cacu[C_index].C_AppDate1 = mtxtPriceDate4.Text.Replace("-", "").Trim();
            }

            if (Sales_Cacu[C_index].Del_TF == "")
                Sales_Cacu[C_index].Del_TF = "U";            
        }




        private void dGridView_KeyDown(object sender, KeyEventArgs e)
        {
            //그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
            if (sender is DataGridView)
            {
                if (e.KeyValue == 46)
                {
                    e.Handled = true;
                } // end if

                if (e.KeyValue == 13)
                {
                    dGridView_Base_Rece_Add_DoubleClick(sender, e);
                }
            }
        }


        private void dGridView_Base_Rece_Add_DoubleClick(object sender, EventArgs e)
        {

        }


        private void butt_Rec_Add_Click(object sender, EventArgs e)
        {
            //회원을 선택 안햇네 그럼 회원 넣어라
            if (txtName.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Mem")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtMbid.Focus(); return ;
            }

            dGridView_Base_Rece_Add.Width = groupBox1.Width - 100;
            dGridView_Base_Rece_Add.Height = groupBox1.Height - 70;
            dGridView_Base_Rece_Add.Left = groupBox1.Left;
            dGridView_Base_Rece_Add.Top = groupBox1.Top;

            Rece_Add_Grid_Set();
            
            cls_form_Meth cfm = new cls_form_Meth();
            cfm.form_Group_Panel_Enable_False(this);
            
            dGridView_Base_Rece_Add.BringToFront();
            dGridView_Base_Rece_Add.RowHeadersVisible = false;
            dGridView_Base_Rece_Add.Visible = true;
            dGridView_Base_Rece_Add.Focus();
        }

     
        private void Rece_Add_Grid_Set()
        {
            dGridView_Rec_Add_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Rece_Add.d_Grid_view_Header_Reset();
            string strSql = "";

            strSql = "Select Distinct Get_Name1 ,Get_ZipCode  ,  Get_Address1  , Get_Address2    ";
            strSql = strSql + " ,Get_Tel1 , Get_Tel2 " ;
            strSql = strSql + " From tbl_Sales_Rece (nolock) ";
            strSql = strSql + " LEFT JOIN tbl_SalesDetail (nolock) ON  tbl_SalesDetail.OrderNumber = tbl_Sales_Rece.OrderNumber ";            
            strSql = strSql + " Where tbl_SalesDetail.Mbid = '" + idx_Mbid.ToString() + "'";          
            strSql = strSql + " And   Receive_Method = 2 ";
            strSql = strSql + " And tbl_SalesDetail.SellCode = '' ";

            strSql = strSql + " Order by  ";
            strSql = strSql + " Get_Name1 ,Get_ZipCode  ,  Get_Address1  , Get_Address2  ";
            strSql = strSql + " ,Get_Tel1 , Get_Tel2 ";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSql, "TempTable", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt == 0) return;

            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_Rec_Add_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb_Rece_Add.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Rece_Add.db_grid_Obj_Data_Put();        
        }

        private void Set_gr_Rec_Add_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {

            string[] row0 = { ds.Tables["TempTable"].Rows[fi_cnt][0].ToString()  
                                ,ds.Tables["TempTable"].Rows[fi_cnt][1].ToString()  
                                ,encrypter.Decrypt ( ds.Tables["TempTable"].Rows[fi_cnt][2].ToString())  
                                ,encrypter.Decrypt (ds.Tables["TempTable"].Rows[fi_cnt][3].ToString())  
                                ,encrypter.Decrypt (ds.Tables["TempTable"].Rows[fi_cnt][4].ToString() )
 
                                ,encrypter.Decrypt (ds.Tables["TempTable"].Rows[fi_cnt][5].ToString()  )                                
                                ,""
                                ,""
                                ,""
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;          
        }


        private void dGridView_Rec_Add_Header_Reset()
        {
            cgb_Rece_Add.Grid_Base_Arr_Clear();
            cgb_Rece_Add.basegrid = dGridView_Base_Rece_Add;
            cgb_Rece_Add.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Rece_Add.grid_col_Count = 10;

            string[] g_HeaderText = {"수령인명"  , "우편번호"   , "주소1"  , "주소2"   , "연락처_1"        
                                , "연락처_2"   , ""    , ""  , "" , ""
                                };

            int[] g_Width = { 80 ,80, 250, 200, 120
                                ,120 , 0 , 0 , 0 , 0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleLeft  
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft  //5    
  
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };
            
            cgb_Rece_Add.grid_col_header_text = g_HeaderText;            
            cgb_Rece_Add.grid_col_w = g_Width;
            cgb_Rece_Add.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                            
                                   };
            cgb_Rece_Add.grid_col_Lock = g_ReadOnly;

            cgb_Rece_Add.basegrid.RowHeadersVisible = false;
        }




        private void Base_Small_Item_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            if (bt.Name == "butt_Item_Clear")
            {
                
                   
                Base_Sub_Clear("item");

               
            }

            else if (bt.Name == "butt_Item_Del")
            {
                if (txt_SalesItemIndex.Text == "") return;

                if (txt_OrderNumber.Text != "")  //주문번호가 존재한다.
                {
                    cls_Search_DB csd = new cls_Search_DB();
                    //재고 관련해서 출고가 된내역인지 확인한다 출고 되었으면 삭제 되면 안됨.
                    if (csd.Check_Stock_OutPut(txt_OrderNumber.Text.Trim(), int.Parse(txt_SalesItemIndex.Text.Trim())) == false)
                    {
                        butt_Item_Del.Focus(); return;
                    }
                }

               
                Base_Sub_Delete("item");
                Base_Sub_Clear("Rece");

                Base_Sub_Sum_Item();
                Base_Sub_Sum_Cacu();
            }

            else if (bt.Name == "butt_Item_Save")
            {
                if (Base_Error_Check__01() == false) return;  //주문종류 , 회원, 주문일자 입력 안햇는지 체크

                if (Item_Rece_Error_Check__01("item") == false) return;

                if (txt_SalesItemIndex.Text == "") //추가 일경우에 새로운 입력
                {
                    Base_Sub_Save_Item();
                    
                    Base_Sub_Clear("item");

                    
                    Base_Sub_Sum_Item();
                    Base_Sub_Sum_Cacu();
                    Save_Button_Click_Cnt++;
                }
                else  //
                {
                    Base_Sub_Edit_Item();
                    

                    Base_Sub_Clear("item");
                    
                    Base_Sub_Sum_Item();
                    Base_Sub_Sum_Cacu();
                    Save_Button_Click_Cnt++;
                }
            }
        }


        private void Base_Small_Rece_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            if (bt.Name == "butt_Rec_Clear")
            {
                Base_Sub_Clear("Rece");
            }

            else if (bt.Name == "butt_Rec_Del")
            {
                if (txt_RecIndex.Text == "") return;
                Base_Sub_Delete("Rece");
            }


            else if (bt.Name == "butt_Rec_Save")
            {

                if (Base_Error_Check__01() == false) return;  //주문종류 , 회원, 주문일자 입력 안햇는지 체크

                if (Item_Rece_Error_Check__01("Rece") == false) return;


                if (txt_RecIndex.Text == "") //추가 일경우에 새로운 입력
                {
                    cls_form_Meth ct = new cls_form_Meth();
                    int Salesitemindex =0 ;
                    for (int i = 0; i <= dGridView_Base_Rece_Item.Rows.Count - 1; i++)
                    {
                        if (dGridView_Base_Rece_Item.Rows[i].Cells[0].Value.ToString() == "V")
                        {
                            Salesitemindex = int.Parse (dGridView_Base_Rece_Item.Rows[i].Cells[1].Value.ToString()) ;
                            Base_Sub_Save_Rece(Salesitemindex);
                        }
                    }

                    Base_Sub_Clear("Rece");
                    Base_Sub_Clear("item");
                    Save_Button_Click_Cnt++;

                    //if (Save_Button_Click_Cnt == 1)
                    //{
                    //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save")
                    //                + "\n" +
                    //    cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Save"));
                    //}
                }
                else  
                {
                    if (Base_Error_Check__01() == false) return;  //주문종류 , 회원, 주문일자 입력 안햇는지 체크

                    if (Item_Rece_Error_Check__01("Rece") == false) return;

                    Base_Sub_Edit_Rece();

                    Base_Sub_Clear("Rece");
                    Base_Sub_Clear("item");
                    Save_Button_Click_Cnt++;

                    //if (Save_Button_Click_Cnt == 1)
                    //{
                    //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit")
                    //                 + "\n" +
                    //    cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Save"));
                    //}
                }
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
                if (txt_C_index.Text == "") return;
                Base_Sub_Delete("Cacu");
                Base_Sub_Sum_Cacu();

            }

            else if (bt.Name == "butt_Cacu_Save")
            {
                if (Base_Error_Check__01() == false) return;  //주문종류 , 회원, 주문일자 입력 안햇는지 체크

                if (Item_Rece_Error_Check__01("Cacu") == false) return;

                if (txt_C_index.Text == "") //추가 일경우에 새로운 입력
                {
                    if (double.Parse(txt_Price_1.Text.Trim().Replace(",", "")) > 0)  //현금이다
                        Base_Sub_Save_Cacu(1);

                    if (double.Parse(txt_Price_2.Text.Trim().Replace(",", "")) > 0)  //무통장이다
                        Base_Sub_Save_Cacu(2);

                    if (double.Parse(txt_Price_3.Text.Trim().Replace(",", "")) > 0)  //카드이다
                        Base_Sub_Save_Cacu(3);

                    if (double.Parse(txt_Price_4.Text.Trim().Replace(",", "")) > 0)  //카드이다
                        Base_Sub_Save_Cacu(4);   

                    Base_Sub_Clear("Cacu");
                    Base_Sub_Sum_Cacu();
                    Save_Button_Click_Cnt++;

                    //if (Save_Button_Click_Cnt == 1)
                    //{
                    //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save")
                    //                + "\n" +
                    //    cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Save"));
                    //}

                }
                else  //
              {
                    Base_Sub_Edit_Cacu();
                    Base_Sub_Clear("Cacu");
                    Base_Sub_Sum_Cacu();
                    Save_Button_Click_Cnt++;

                    //if (Save_Button_Click_Cnt == 1)
                    //{
                    //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit")
                    //                 + "\n" +
                    //    cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Save"));
                    //}
                }
            }

            else if (bt.Name == "butt_AddCode")
            {
                frmBase_AddCode e_f = new frmBase_AddCode();
                e_f.Send_Address_Info += new frmBase_AddCode.SendAddressDele(e_f_Send_Address_Info);
                e_f.ShowDialog();
            }

        }

        private void e_f_Send_Address_Info(string AddCode1, string AddCode2, string Address1, string Address2, string Address3)
        {
            Data_Set_Form_TF = 1;
            mtxtZip1.Text = AddCode1 + "-" + AddCode2;             
            txtAddress1.Text = Address1; txtAddress2.Text = Address2;
            Data_Set_Form_TF = 0;
            txtAddress2.Focus();
        }




        private void opt_Rec_Add1_MouseUp(object sender, MouseEventArgs e)
        {
            RadioButton t_rb = (RadioButton)sender;

            //회원을 선택 안햇네 그럼 회원 넣어라
            if (txtName.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Mem")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                t_rb.Checked = false;
                mtxtMbid.Focus(); return ;
            }

            

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";
            DataSet ds = new DataSet();
            int ReCnt = 0;

            if (t_rb.Name == "opt_Rec_Add2")
            {
                Tsql = "Select ZipCode Addcode1 , add1 Address1 , add2 Address2  ";
                Tsql = Tsql + " ,phone hptel ,phone homeTel , tbl_User.U_Name N_Name ";
                Tsql = Tsql + " From tbl_User (nolock ) ";
                              
                    Tsql = Tsql + " Where User_Ncode = '" + idx_Mbid.ToString() + "'";
              
                
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
                ReCnt = Temp_Connect.DataSet_ReCount;
            }
            //else if (t_rb.Name == "opt_Rec_Add2")
            //{
            //    Tsql = "Select ETC_Addcode1 Addcode1 , ETC_Address1 Address1 , ETC_Address2 Address2 , ETC_Address3 Address3 ";
            //    Tsql = Tsql + " ,ETC_Tel_1 hptel ,ETC_Tel_2 homeTel , ETC_Name M_Name ";
            //    Tsql = Tsql + " From tbl_Memberinfo_Address (nolock ) ";

        

            //    if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            //    {
            //        Tsql = Tsql + " Where tbl_Memberinfo_Address.Mbid = '" + idx_Mbid + "' ";
            //        Tsql = Tsql + " And   tbl_Memberinfo_Address.Mbid2 = " + idx_Mbid2.ToString();
            //    }

            //    if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            //    {
            //        Tsql = Tsql + " Where tbl_Memberinfo_Address.Mbid2 = " + idx_Mbid2.ToString();
            //    }

            //    if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            //    {
            //        Tsql = Tsql + " Where tbl_Memberinfo_Address.Mbid = '" + idx_Mbid.ToString() + "'";
            //    }


            //    Tsql = Tsql + " And   Sort_Add  = 'R' ";

            //    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //    if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            //    ReCnt = Temp_Connect.DataSet_ReCount;
            //}
            else if (t_rb.Name == "opt_Rec_Add3")
            {
                Data_Set_Form_TF = 1;
                mtxtZip1.Text = ""; 
                txtAddress1.Text = ""; txtAddress2.Text = "";
                mtxtTel1.Text = "";
                mtxtTel2.Text = ""; 
                txt_Get_Name1.Text = "";
                Data_Set_Form_TF = 0;
            }

            Data_Set_Form_TF = 1 ;
            mtxtZip1.Text = "";
            txtAddress1.Text = ""; txtAddress2.Text = "";
            mtxtTel1.Text = "";
            mtxtTel2.Text = ""; 
            txt_Get_Name1.Text = "";
            Data_Set_Form_TF = 0;

            if (ReCnt == 0) return;

            Data_Set_Form_TF = 1;
            txtAddress1.Text = encrypter.Decrypt( ds.Tables["t_P_table"].Rows[0]["address1"].ToString());
            txtAddress2.Text = encrypter.Decrypt(ds.Tables["t_P_table"].Rows[0]["address2"].ToString());

            if (ds.Tables["t_P_table"].Rows[0]["Addcode1"].ToString().Length >= 6)
            {
                mtxtZip1.Text =  ds.Tables["t_P_table"].Rows[0]["Addcode1"].ToString().Substring(0, 3) + "-" + ds.Tables["t_P_table"].Rows[0]["Addcode1"].ToString().Substring(3, 3) ;
                //txtAddCode1.Text = ds.Tables["t_P_table"].Rows[0]["Addcode1"].ToString().Substring(0, 3);
                //txtAddCode2.Text = ds.Tables["t_P_table"].Rows[0]["Addcode1"].ToString().Substring(3, 3);
            }

            string T_Num_1 = ""; string T_Num_2 = ""; string T_Num_3 = "";
            cls_form_Meth cfm = new cls_form_Meth();
            //cfm.Phone_Number_Split(encrypter.Decrypt(ds.Tables["t_P_table"].Rows[0]["hptel"].ToString()), ref T_Num_1, ref T_Num_2, ref T_Num_3);
            mtxtTel1.Text = encrypter.Decrypt(ds.Tables["t_P_table"].Rows[0]["hptel"].ToString());

            //cfm.Phone_Number_Split(encrypter.Decrypt(ds.Tables["t_P_table"].Rows[0]["homeTel"].ToString()), ref T_Num_1, ref T_Num_2, ref T_Num_3);
            //txtTel2_1.Text = T_Num_1; txtTel2_2.Text = T_Num_2; txtTel2_3.Text = T_Num_3;
            mtxtTel2.Text = encrypter.Decrypt(ds.Tables["t_P_table"].Rows[0]["homeTel"].ToString());

            if (t_rb.Name == "opt_Rec_Add2")             
                txt_Get_Name1.Text = encrypter.Decrypt(ds.Tables["t_P_table"].Rows[0]["M_Name"].ToString()); //주소테이블의 배송자명은 암호화 햇기 대문에
            else
                txt_Get_Name1.Text = ds.Tables["t_P_table"].Rows[0]["M_Name"].ToString();  //회원 테이블의 회원명은 암호화 안햇음
            Data_Set_Form_TF = 0 ;
        }




        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;


            cls_form_Meth ct = new cls_form_Meth();

            if (dtp.Name == "DTP_SellDate")
                ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender, txtCenter2);


            if (dtp.Name == "DTP_PriceDate3")
                ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender, txt_C_Card);

            if (dtp.Name == "DTP_PriceDate1")
                ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender, butt_Cacu_Save);

            if (dtp.Name == "DTP_PriceDate2")
                ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender, txt_C_Name_2);

            if (dtp.Name == "DTP_PriceDate4")
                ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender, butt_Cacu_Save);


           // SendKeys.Send("{TAB}");
        }

   
  
        
        private Boolean Check_TextBox_Error()
        {
            //주문종류 , 회원, 주문일자 입력 안햇는지 체크
            if (Base_Error_Check__01() == false) return false;

            

            //회원번호 관련 관련 오류 체크 및 존재 여부 그리고 탈퇴 여부(신규 저장일 경우에)                      
            if (Input_Error_Check(mtxtMbid, "m",1) == false) return false;                                            
            
            if (Input_Error_Check_Save() == false) return false;

            if (Input_Error_Check_Save___02() == false) return false;

            

            
            return true;
        }

        private bool Input_Error_Check_Save()
        {
            //그리드 상에 선택한 상품이 한개라도 잇는지..
            if (dGridView_Base_Item.RowCount == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_ItemCode.Focus(); return false;
            }


            ////if (
            ////    (txtSellCode_Code.Text == "" && txtSellCode.Text.Trim() != "")
            ////    ||
            ////    (txtSellCode_Code.Text != "" && txtSellCode.Text.Trim() == "")
            ////    )
            ////{
            ////    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
            ////           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_SellCode")
            ////          + "\n" +
            ////          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            ////    txtSellCode.Focus(); return false;
            ////}


            if (
                (txtCenter2_Code.Text == "" && txtCenter2.Text.Trim() != "")
                ||
                (txtCenter2_Code.Text != "" && txtCenter2.Text.Trim() == "")
                )
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_SellCenter")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtCenter2.Focus(); return false;
            }


            if (txt_OrderNumber.Text.Trim() != "")
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
                        mtxtSellDate.Focus(); return false;
                    }

                    if (ds.Tables[base_db_name].Rows[0]["ReturnTF"].ToString() == "3")
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_3")
                               + "\n" +
                               cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        mtxtSellDate.Focus(); return false;
                    }

                    if (ds.Tables[base_db_name].Rows[0]["ReturnTF"].ToString() == "4")
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_4")
                               + "\n" +
                               cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        mtxtSellDate.Focus(); return false;
                    }

                    if (ds.Tables[base_db_name].Rows[0]["ReturnTF"].ToString() == "5")
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_5")
                               + "\n" +
                               cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        mtxtSellDate.Focus(); return false;
                    }
                }





                //현 내역을 반품이나 부분반품 교환을 햇다. 그럼 현내역 역시 수정 못하게함.
                Tsql = "select SellDate from tbl_SalesDetail  (nolock) ";
                Tsql = Tsql + " Where Re_BaseOrderNumber = '" + txt_OrderNumber.Text.Trim() + "' ";

                //++++++++++++++++++++++++++++++++

                ds.Clear();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
                ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt != 0)
                {
                    
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_1")
                            + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtSellDate.Focus(); return false;
                    
                }
            }
       

            return true;
        }

        private bool Input_Error_Check_Save___02()
        {
            cls_Search_DB csd = new cls_Search_DB();

            if (cls_app_static_var.Sell_Union_Flag == "U" || cls_app_static_var.Sell_Union_Flag == "D")
            {
                if (txt_UnaccMoney.Text == "")
                    txt_UnaccMoney.Text = "0";

                if (double.Parse(txt_UnaccMoney.Text.Trim()) > 0)
                {
                    MessageBox.Show("조합 관련 신고로 미수금 또는 + 금액이 존재하면 안됩니다."
                                        + "\n" +
                                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    dGridView_Base_Cacu.Focus(); return false;
                }
            }

            if (txt_OrderNumber.Text.Trim() == "")
            {
                //마감정산이 이루어진 판매 날짜인지 체크한다.                
                if (csd.Close_Check_SellDate("tbl_CloseTotal_04", mtxtSellDate2.Text.Trim()) == false)
                {
                    mtxtSellDate2.Focus(); return false;
                }
            }
            else
            {
                string Be_SellDate = SalesDetail[txt_OrderNumber.Text.Trim()].SellDate_2.Replace("-", "");

                if (Be_SellDate != mtxtSellDate2.Text.Trim())
                {
                    if (csd.Close_Check_SellDate("tbl_CloseTotal_04", Be_SellDate) == false)
                    {
                        mtxtSellDate.Focus(); return false;
                    }

                    if (csd.Close_Check_SellDate("tbl_CloseTotal_04", mtxtSellDate2.Text.Trim()) == false)
                    {
                        mtxtSellDate.Focus(); return false;
                    }
                }

                if (cls_app_static_var.Sell_Union_Flag == "U") //특판
                {
                    //공제번호가 발급 된내역인데.. 금액 수정을 할려고 한다. 그럼 공제 취소하고 다시 하라고 알려준다.
                    cls_form_Meth cm = new cls_form_Meth();
                    if (txt_Ins_Number.Text.Trim() != "" && txt_Ins_Number.Text.Trim() != cm._chang_base_caption_search("미신고"))
                    {
                        double Be_P = 0; double Cur_P = 0;

                        Be_P = SalesDetail[txt_OrderNumber.Text.Trim()].TotalPrice;
                        Cur_P = double.Parse(txt_TotalPrice.Text.Trim().Replace(",", ""));
                        if (Be_P != Cur_P)
                        {
                            string S_SellDate = SalesDetail[txt_OrderNumber.Text.Trim()].SellDate.Replace("-", "");
                            S_SellDate = S_SellDate.Substring(0, 4) + '-' + S_SellDate.Substring(4, 2) + '-' + S_SellDate.Substring(6, 2);
                            string S_SellDate2 = cls_User.gid_date_time.Substring(0, 4) + '-' + cls_User.gid_date_time.Substring(4, 2) + '-' + cls_User.gid_date_time.Substring(6, 2);

                            cls_Date_G date_G = new cls_Date_G();
                            double dif = date_G.DateDiff("d", DateTime.Parse(S_SellDate), DateTime.Parse(S_SellDate2));

                            if (dif > 2)
                            {
                                while (DateTime.Parse(S_SellDate) <= DateTime.Parse(S_SellDate2))
                                {
                                    int r_d = date_G.Check_Date_HolyDay_TF(DateTime.Parse(S_SellDate));
                                    dif = dif + r_d;

                                    DateTime TodayDate = new DateTime();
                                    TodayDate = DateTime.Parse(S_SellDate);
                                    S_SellDate = TodayDate.AddDays(1).ToString("yyyy-MM-dd");
                                }
                            }

                            if (dif > 2) //2영업일이 지난내역은 걍 저장시켜준다. 대신 조합측에 알아서 하라고 메시지 뛰운다.
                            {
                                string t_Msg = "";
                                t_Msg = "현재일 기준으로 2영업일이 지난 판매 내역 입니다." + "\n" +
                                    "현재 내역은 프로그램 상으로 저장을 하나 조합측에는 신고 할 수 없습니다." + "\n" +
                                    "조합측에 별도로 문의해주시기 바랍니다.";

                                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString(t_Msg), "", MessageBoxButtons.YesNo) == DialogResult.No) return false;
                            }
                            else // 2영업일이 안지낫고 매출 신고 되었다.. 그럼 매출 취소 신청하고 다시하라고한다.
                            {
                                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Chang_Insur_Number")
                                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Sell_Price")
                                        + " " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Not_Chang")
                                        + "\n" +
                                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                                mtxtSellDate.Focus(); return false;
                            }
                        }
                    }

                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////


                ////이미 마감돈 날짜이면 금액이나 PV 관련 수정이 되면 마감 못돌게 한다.
                //if (csd.Close_Check_SellDate("tbl_CloseTotal_04", mtxtSellDate2.Text.Trim()) == false)
                //{
                //    double Be_P =0 ; double   Cur_P= 0 ;
                //    Be_P = SalesDetail[txt_OrderNumber.Text.Trim()].TotalPV  ; 
                //    Cur_P = double.Parse(txt_TotalPv.Text.Trim().Replace(",", ""));


                //    if (Be_P != Cur_P)
                //    {
                //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Date")
                //                + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Sell_PV")
                //                + " " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Not_Chang")
                //                + "\n" +
                //                cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                //        mtxtSellDate.Focus(); return false;
                //    }

                //    Be_P = SalesDetail[txt_OrderNumber.Text.Trim()].TotalPrice;
                //    Cur_P = double.Parse(txt_TotalPrice.Text.Trim().Replace(",", ""));

                //    if (Be_P != Cur_P)
                //    {
                //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Date")
                //                + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Sell_Price")
                //                + " " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Not_Chang")
                //                + "\n" +
                //                cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                //        mtxtSellDate.Focus(); return false;
                //    }


                //    if (SalesDetail[txt_OrderNumber.Text.Trim()].SellCode != txtSellCode_Code.Text.Trim())
                //    {
                //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Close_Date")
                //                + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_SellCode")
                //                + " " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Not_Chang")                                
                //                + "\n" +
                //                cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                //                txtSellCode.Focus(); return false;
                //    }

                //}
                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                
            }
            return true;
        }

        private void Input_SalesDetail_dic()
        {
            cls_form_Meth ct = new cls_form_Meth();

            double Total_Sell_VAT_Price = 0; double Total_Sell_Except_VAT_Price = 0;
            double InputCash = 0; double InputPassbook = 0; double InputCard = 0; ; double InputMile = 0;

            foreach (int t_key in SalesItemDetail.Keys)
            {
                if (SalesItemDetail[t_key].Del_TF != "D")
                {
                    Total_Sell_VAT_Price = Total_Sell_VAT_Price + SalesItemDetail[t_key].Total_Sell_VAT_Price;
                    Total_Sell_Except_VAT_Price = Total_Sell_Except_VAT_Price + SalesItemDetail[t_key].Total_Sell_Except_VAT_Price;
                }
            }

            foreach (int t_key in Sales_Cacu.Keys)
            {
                if (Sales_Cacu[t_key].Del_TF != "D")
                {
                    if (Sales_Cacu[t_key].C_TF == 1)
                        InputCash = InputCash + Sales_Cacu[t_key].C_Price1;
                    if (Sales_Cacu[t_key].C_TF == 2)
                        InputPassbook = InputPassbook + Sales_Cacu[t_key].C_Price1;
                    if (Sales_Cacu[t_key].C_TF == 3)
                        InputCard = InputCard + Sales_Cacu[t_key].C_Price1;
                    if (Sales_Cacu[t_key].C_TF == 4)
                        InputMile = InputMile + Sales_Cacu[t_key].C_Price1;
                }
            }
            
            cls_Sell t_c_sell = new cls_Sell();

            t_c_sell.OrderNumber = "";
            
            t_c_sell.Mbid = idx_Mbid ;
            t_c_sell.Mbid2 = idx_Mbid2;
            t_c_sell.Na_Code = idx_Na_Code ;
            t_c_sell.M_Name = txtName.Text.Trim();



            t_c_sell.SellCode = "";//txtSellCode_Code.Text.Trim();
            t_c_sell.SellCodeName = "";// txtSellCode.Text.Trim ();

            //판매센타입력 사항이 없으면 걍 회원센타로 지정을 한다.
            if (txtCenter2_Code.Text.Trim() != "")
            {
                t_c_sell.BusCode = txtCenter2_Code.Text.Trim();
                t_c_sell.BusCodeName = txtCenter2.Text.Trim();
            }
            else
            {
                t_c_sell.BusCode = txtCenter_Code.Text.Trim();
                t_c_sell.BusCodeName = txtCenter.Text.Trim();
            }
            t_c_sell.Re_BaseOrderNumber = "";
            t_c_sell.TotalPrice = double.Parse(txt_TotalPrice.Text.Trim().Replace(",","") );
            t_c_sell.TotalPV = double.Parse(txt_TotalPv.Text.Trim().Replace(",", ""));
            t_c_sell.TotalCV = double.Parse(txt_TotalCV.Text.Trim().Replace(",", "")); 
            t_c_sell.TotalInputPrice = double.Parse(txt_TotalInputPrice.Text.Trim().Replace(",",""));
            t_c_sell.Total_Sell_VAT_Price = Total_Sell_VAT_Price;
            t_c_sell.Total_Sell_Except_VAT_Price = Total_Sell_Except_VAT_Price;
            t_c_sell.InputCash = InputCash;
            t_c_sell.InputCard = InputCard;
            t_c_sell.InputPassbook = InputPassbook ;
            t_c_sell.Be_InputMile = 0;
            t_c_sell.InputMile = InputMile ;
            t_c_sell.InputPass_Pay = 0;
            t_c_sell.UnaccMoney = double.Parse(txt_UnaccMoney.Text.Trim().Replace(",", ""));
            
            t_c_sell.Etc1 = txt_ETC1.Text.Trim();
            t_c_sell.Etc2 = txt_ETC2.Text.Trim();

            t_c_sell.ReturnTF = 1;
            t_c_sell.ReturnTFName = ct._chang_base_caption_search("정상");
            t_c_sell.INS_Num = "";
            t_c_sell.InsuranceNumber_Date = "";
            t_c_sell.W_T_TF = 0;
            t_c_sell.In_Cnt = 0;

            t_c_sell.RecordID = cls_User.gid;
            t_c_sell.RecordTime = "";
                                
            t_c_sell.SellDate = mtxtSellDate.Text.Replace("-","") .Trim();
            t_c_sell.SellDate_2 = mtxtSellDate2.Text.Replace("-", "").Trim();

            t_c_sell.Del_TF = "S";
            SalesDetail[""] = t_c_sell;
        }

        private void Update_SalesDetail_dic()
        {
            double Total_Sell_VAT_Price = 0; double Total_Sell_Except_VAT_Price = 0;
            double InputCash = 0; double InputPassbook = 0; double InputCard = 0; double InputMile = 0;

            foreach (int t_key in SalesItemDetail.Keys)
            {
                if (SalesItemDetail[t_key].Del_TF != "D")
                {
                    Total_Sell_VAT_Price = Total_Sell_VAT_Price + SalesItemDetail[t_key].Total_Sell_VAT_Price;
                    Total_Sell_Except_VAT_Price = Total_Sell_Except_VAT_Price + SalesItemDetail[t_key].Total_Sell_Except_VAT_Price;
                }
            }

            foreach (int t_key in Sales_Cacu.Keys)
            {
                if (Sales_Cacu[t_key].Del_TF != "D")
                {
                    if (Sales_Cacu[t_key].C_TF == 1)
                        InputCash = InputCash + Sales_Cacu[t_key].C_Price1;
                    if (Sales_Cacu[t_key].C_TF == 2)
                        InputPassbook = InputPassbook + Sales_Cacu[t_key].C_Price1;
                    if (Sales_Cacu[t_key].C_TF == 3)
                        InputCard = InputCard + Sales_Cacu[t_key].C_Price1;

                    if (Sales_Cacu[t_key].C_TF == 4)
                        InputMile = InputMile + Sales_Cacu[t_key].C_Price1;
                }
            }


            string OrderNumber = txt_OrderNumber.Text.Trim();
            
            SalesDetail[OrderNumber].Mbid = idx_Mbid;
            SalesDetail[OrderNumber].Mbid2 = idx_Mbid2;
            SalesDetail[OrderNumber].Na_Code  = idx_Na_Code;
            SalesDetail[OrderNumber].M_Name = txtName.Text.Trim();

            SalesDetail[OrderNumber].SellCode = txtSellCode_Code.Text.Trim();
            SalesDetail[OrderNumber].SellCodeName = txtSellCode.Text.Trim();
            SalesDetail[OrderNumber].BusCode = txtCenter2_Code.Text.Trim();
            SalesDetail[OrderNumber].BusCodeName = txtCenter2.Text.Trim();
            SalesDetail[OrderNumber].Re_BaseOrderNumber = "";

            SalesDetail[OrderNumber].TotalPrice = double.Parse(txt_TotalPrice.Text.Trim().Replace(",", ""));
            SalesDetail[OrderNumber].TotalPV = double.Parse(txt_TotalPv.Text.Trim().Replace(",", ""));
            SalesDetail[OrderNumber].TotalCV = double.Parse(txt_TotalCV.Text.Trim().Replace(",", ""));
            SalesDetail[OrderNumber].TotalInputPrice = double.Parse(txt_TotalInputPrice.Text.Trim().Replace(",", ""));
            SalesDetail[OrderNumber].Total_Sell_VAT_Price = Total_Sell_VAT_Price;
            SalesDetail[OrderNumber].Total_Sell_Except_VAT_Price = Total_Sell_Except_VAT_Price;
            SalesDetail[OrderNumber].InputCash = InputCash;
            SalesDetail[OrderNumber].InputCard = InputCard;
            SalesDetail[OrderNumber].InputPassbook = InputPassbook;
            SalesDetail[OrderNumber].InputMile = InputMile ;
            SalesDetail[OrderNumber].InputPass_Pay = 0;
            SalesDetail[OrderNumber].UnaccMoney = double.Parse(txt_UnaccMoney.Text.Trim().Replace(",", ""));

            SalesDetail[OrderNumber].Etc1 = txt_ETC1.Text.Trim();
            SalesDetail[OrderNumber].Etc2 = txt_ETC2.Text.Trim();

            SalesDetail[OrderNumber].SellDate = mtxtSellDate.Text.Replace("-", "").Trim();
            SalesDetail[OrderNumber].SellDate_2 = mtxtSellDate2.Text.Replace("-", "").Trim();

            if (SalesDetail[OrderNumber].Del_TF == "")
                SalesDetail[OrderNumber].Del_TF = "U";         
        }


        private void DB_Save_tbl_SalesDetail(cls_Connect_DB Temp_Connect ,
                                             SqlConnection Conn, SqlTransaction tran, ref string T_ord_N)
        {
            string  T_CenterCode  = "";
            string IndexTime = "";

            if (txt_OrderNumber.Text.Trim() != "")
                T_ord_N = txt_OrderNumber.Text.Trim();

            else
            {
                //if (SalesDetail[T_ord_N].BusCode == "")
                //{
                //    int w_cnt = 1;
                //    while (w_cnt <= cls_app_static_var.Center_Code_Length)
                //    {
                //        T_CenterCode = T_CenterCode + "0";
                //        w_cnt++;
                //    }
                //}
                //else
                //{
                //    T_CenterCode = SalesDetail[T_ord_N].BusCode;
                //}

                T_CenterCode =  "E"; //직원주문 관련해서  주문번호 가운데에 E를 넣는다.

                IndexTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string StrSql = "";
                StrSql = "INSERT INTO tbl_Sales_OrdNumber_Mem ";
                StrSql = StrSql + " (OrderNumber , Mbid , Mbid2 ";
                StrSql = StrSql + " , SellDate , SellCode , IndexTime ";
                StrSql = StrSql + " , User_TF)";
                StrSql = StrSql + " Select ";
                StrSql = StrSql + "'" + mtxtSellDate.Text.Replace("-", "").Trim() + "'+'" + T_CenterCode + "'+";
                StrSql = StrSql + " Right('0000' + convert(varchar(4),convert(float,Right(Count(orderNumber),4)) + 1),4) ";

                StrSql = StrSql + ",'" + idx_Mbid + "'," + idx_Mbid2 + ",";
                StrSql = StrSql + "'" + mtxtSellDate.Text.Replace("-", "").Trim() + "','" + txtSellCode_Code.Text.Trim() + "',";
                StrSql = StrSql + "'" + IndexTime + "',1";

                StrSql = StrSql + " From tbl_Sales_OrdNumber_Mem  (nolock)  ";
                StrSql = StrSql + " Where LEFT(OrderNumber,8) = '" + mtxtSellDate.Text.Replace("-", "").Trim() + "'";

                if (Temp_Connect.Insert_Data(StrSql, "tbl_Sales_OrdNumber_Mem", Conn, tran, this.Name.ToString(), this.Text) == false) return;




                //++++++++++++++++++++++++++++++++                
                StrSql = "Select OrderNumber  ";
                StrSql = StrSql + " From tbl_Sales_OrdNumber_Mem (nolock) ";
                StrSql = StrSql + " Where Mbid = '" + idx_Mbid + "'";
                StrSql = StrSql + " And Mbid2 = " + idx_Mbid2;
                StrSql = StrSql + " And SellDate = '" + mtxtSellDate.Text.Replace("-", "").Trim() + "'";
                StrSql = StrSql + " And SellCode = '" + txtSellCode_Code.Text.Trim() + "'";
                StrSql = StrSql + " And IndexTime = '" + IndexTime + "'";
                
                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Sales_OrdNumber_Mem", ds) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;

                SalesDetail[T_ord_N].OrderNumber = ds.Tables["tbl_Sales_OrdNumber_Mem"].Rows[0]["OrderNumber"].ToString();
                T_ord_N = ds.Tables["tbl_Sales_OrdNumber_Mem"].Rows[0]["OrderNumber"].ToString();
                //++++++++++++++++++++++++++++++++
            }


            
        }

        private void DB_Save_tbl_SalesDetail____002(cls_Connect_DB Temp_Connect,
                                             SqlConnection Conn, SqlTransaction tran,  string OrderNumber)
        {
            string StrSql = "";
            if (txt_OrderNumber.Text.Trim() == "")
            {
                string Ins_Ordernumber = "";

                StrSql = "INSERT INTO tbl_SalesDetail" ;
                StrSql = StrSql + " (OrderNumber,Mbid,Mbid2,M_Name,SellDate,SellDate_2,SellCode,BusCode,Na_Code,";
                StrSql = StrSql + " TotalPrice,TotalPV,TotalCV,TotalInputPrice,";
                StrSql = StrSql + " Total_Sell_VAT_Price,Total_Sell_Except_VAT_Price, ";
                StrSql = StrSql + " InputCash,InputCard,InputPassbook, InputMile,UnaccMoney,";
                StrSql = StrSql + " Etc1,Etc2, ";
                StrSql = StrSql + " ReturnTF,InsuranceNumber,InsuranceNumber_Date, ";
                StrSql = StrSql + " RecordID,RecordTime";

                StrSql = StrSql + " ) Values ( ";
                StrSql = StrSql + "'" + SalesDetail[Ins_Ordernumber].OrderNumber + "'";
                StrSql = StrSql + ",'" + SalesDetail[Ins_Ordernumber].Mbid + "'";
                StrSql = StrSql + "," + SalesDetail[Ins_Ordernumber].Mbid2;
                StrSql = StrSql + ",'" + SalesDetail[Ins_Ordernumber].M_Name + "'";
                StrSql = StrSql + ",'" + SalesDetail[Ins_Ordernumber].SellDate + "'";
                StrSql = StrSql + ",'" + SalesDetail[Ins_Ordernumber].SellDate_2 + "'";
                StrSql = StrSql + ",'" + SalesDetail[Ins_Ordernumber].SellCode + "'";
                StrSql = StrSql + ",'" + SalesDetail[Ins_Ordernumber].BusCode + "'";
                StrSql = StrSql + ",'" + SalesDetail[Ins_Ordernumber].Na_Code  + "'";
                StrSql = StrSql + "," + SalesDetail[Ins_Ordernumber].TotalPrice ;
                StrSql = StrSql + "," + SalesDetail[Ins_Ordernumber].TotalPV;
                StrSql = StrSql + "," + SalesDetail[Ins_Ordernumber].TotalCV;
                StrSql = StrSql + "," + SalesDetail[Ins_Ordernumber].TotalInputPrice;
                StrSql = StrSql + "," + SalesDetail[Ins_Ordernumber].Total_Sell_VAT_Price;
                StrSql = StrSql + "," + SalesDetail[Ins_Ordernumber].Total_Sell_Except_VAT_Price;
                StrSql = StrSql + "," + SalesDetail[Ins_Ordernumber].InputCash;
                StrSql = StrSql + "," + SalesDetail[Ins_Ordernumber].InputCard;
                StrSql = StrSql + "," + SalesDetail[Ins_Ordernumber].InputPassbook;
                StrSql = StrSql + "," + SalesDetail[Ins_Ordernumber].InputMile;
                StrSql = StrSql + "," + SalesDetail[Ins_Ordernumber].UnaccMoney;
                StrSql = StrSql + ",'" + SalesDetail[Ins_Ordernumber].Etc1 + "'";
                StrSql = StrSql + ",'" + SalesDetail[Ins_Ordernumber].Etc2 + "'";
                StrSql = StrSql + "," + SalesDetail[Ins_Ordernumber].ReturnTF;
                StrSql = StrSql + ",'" + SalesDetail[Ins_Ordernumber].INS_Num + "'";
                StrSql = StrSql + ",'" + SalesDetail[Ins_Ordernumber].InsuranceNumber_Date + "'";
                StrSql = StrSql + ",'" + SalesDetail[Ins_Ordernumber].RecordID + "'";
                StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) " ;
                StrSql = StrSql + ")";

                if (Temp_Connect.Insert_Data(StrSql, "tbl_SalesDetail", Conn, tran, this.Name.ToString(), this.Text) == false) return;



                StrSql = "INSERT INTO tbl_SalesDetail_TF (OrderNumber,SellTF)" ;
                StrSql = StrSql + "  Values ( ";
                if (cls_app_static_var.Sell_TF_CS_Flag == "")  // ""이면 CS에서 등록되는 건들은 다 승인  그게 아니면 다 미승인으로 처리함.
                    StrSql = StrSql + "'" + SalesDetail[Ins_Ordernumber].OrderNumber + "',1)";
                else
                    StrSql = StrSql + "'" + SalesDetail[Ins_Ordernumber].OrderNumber + "',0)";

                if (Temp_Connect.Insert_Data(StrSql, "tbl_SalesDetail", Conn, tran, this.Name.ToString(), this.Text) == false) return;                             

            }
            else
            {
                cls_Search_DB csd = new cls_Search_DB();

                //수정하기 전에 배열에다가 내역을 받아둔다.
                csd.SalesDetail_Mod_BackUp(OrderNumber, "tbl_SalesDetail");


                StrSql = "Update tbl_SalesDetail Set ";
                StrSql = StrSql + " SellDate = '" + SalesDetail[OrderNumber].SellDate.Replace("-", "") + "'";
                StrSql = StrSql + ",SellDate_2 = '" + SalesDetail[OrderNumber].SellDate_2.Replace("-", "") + "'";
                StrSql = StrSql + ",TotalPrice = " + SalesDetail[OrderNumber].TotalPrice ;
                StrSql = StrSql + ",TotalPV= " + SalesDetail[OrderNumber].TotalPV;
                StrSql = StrSql + ",TotalcV= " + SalesDetail[OrderNumber].TotalCV;
                StrSql = StrSql + ",TotalInputPrice= " + SalesDetail[OrderNumber].TotalInputPrice;

                StrSql = StrSql + ",Total_Sell_VAT_Price= " + SalesDetail[OrderNumber].Total_Sell_VAT_Price;
                StrSql = StrSql + ",Total_Sell_Except_VAT_Price= " + SalesDetail[OrderNumber].Total_Sell_Except_VAT_Price;

                StrSql = StrSql + ",InputCash= " + SalesDetail[OrderNumber].InputCash;
                StrSql = StrSql + ",InputCard= " + SalesDetail[OrderNumber].InputCard;
                StrSql = StrSql + ",InputPassbook= " + SalesDetail[OrderNumber].InputPassbook;
                StrSql = StrSql + ",InputMile= " + SalesDetail[OrderNumber].InputMile;
                StrSql = StrSql + ",UnaccMoney= " + SalesDetail[OrderNumber].UnaccMoney;

                StrSql = StrSql + ",Etc1= '" + SalesDetail[OrderNumber].Etc1 + "'";
                StrSql = StrSql + ",Etc2= '" + SalesDetail[OrderNumber].Etc2 + "'";

                StrSql = StrSql + " Where OrderNumber = '" + SalesDetail[OrderNumber].OrderNumber  + "'";

                if (Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name.ToString(), this.Text) == false) return;

                //주테이블의 변경 내역을 테이블에 넣는다.
                csd.SalesDetail_Mod(Conn, tran,OrderNumber, "tbl_SalesDetail");



            }
        }


        private void DB_Save_tbl_Mileage____001(cls_Connect_DB Temp_Connect,
                                            SqlConnection Conn, SqlTransaction tran, string OrderNumber)
        {
            
            if (txt_OrderNumber.Text.Trim() == "")
            {
                string Ins_Ordernumber = "";

                if (SalesDetail[Ins_Ordernumber].InputMile > 0)
                {
                    cls_tbl_Mileage ctm = new cls_tbl_Mileage();
                    ctm.Put_Minus_Mileage(SalesDetail[Ins_Ordernumber].Mbid, SalesDetail[Ins_Ordernumber].Mbid2, SalesDetail[Ins_Ordernumber].M_Name
                        , SalesDetail[Ins_Ordernumber].InputMile, SalesDetail[Ins_Ordernumber].OrderNumber, "12"
                        , Temp_Connect, Conn, tran, "", this.Name.ToString(), this.Text);
                }


            }
            else
            {

                if (SalesDetail[OrderNumber].InputMile > SalesDetail[OrderNumber].Be_InputMile )
                {
                    cls_tbl_Mileage ctm = new cls_tbl_Mileage();
                    ctm.Put_Minus_Mileage(SalesDetail[OrderNumber].Mbid, SalesDetail[OrderNumber].Mbid2, SalesDetail[OrderNumber].M_Name
                        , SalesDetail[OrderNumber].InputMile - SalesDetail[OrderNumber].Be_InputMile , SalesDetail[OrderNumber].OrderNumber, "16"
                        , Temp_Connect, Conn, tran, "", this.Name.ToString(), this.Text);
                }

                if (SalesDetail[OrderNumber].InputMile < SalesDetail[OrderNumber].Be_InputMile)
                {
                    cls_tbl_Mileage ctm = new cls_tbl_Mileage();
                    ctm.Put_Plus_Mileage(SalesDetail[OrderNumber].Mbid, SalesDetail[OrderNumber].Mbid2, SalesDetail[OrderNumber].M_Name
                        , SalesDetail[OrderNumber].Be_InputMile - SalesDetail[OrderNumber].InputMile, SalesDetail[OrderNumber].OrderNumber, "15"
                        , Temp_Connect, Conn, tran, "", this.Name.ToString(), this.Text);
                }
            }
        }



        private void DB_Save_tbl_SalesItemDetail(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, 
                    string OrderNumber)
        {           
            
            foreach (int t_key in SalesItemDetail.Keys)
            {
                if (SalesItemDetail[t_key].Del_TF == "D") //삭제이다
                {
                    //백업데이블에 백업 받고 삭제 처리한다.
                    DB_Save_tbl_SalesItemDetail____D(Temp_Connect, Conn, tran, OrderNumber, t_key);
                }
                else if (SalesItemDetail[t_key].Del_TF == "U") //업데이트다 
                {
                    DB_Save_tbl_SalesItemDetail____U(Temp_Connect, Conn, tran, OrderNumber, t_key);
                }
                else if (SalesItemDetail[t_key].Del_TF == "S")  //새로운 저장이다
                {
                    DB_Save_tbl_SalesItemDetail____S(Temp_Connect, Conn, tran, OrderNumber, t_key);
                }
            }
        }

        private void DB_Save_tbl_SalesItemDetail____D(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran,
                    string OrderNumber, int SalesItemIndex )
        {
            string StrSql = "";

            StrSql = "Insert into tbl_SalesitemDetail_Mod_Del  ";
            StrSql = StrSql + " Select * ,0,'" + cls_User.gid  + "',Convert(Varchar(25),GetDate(),21) From tbl_SalesitemDetail ";
            StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
            StrSql = StrSql + " And   SalesItemIndex = " + SalesItemIndex ;

            if (Temp_Connect.Insert_Data(StrSql, "tbl_SalesitemDetail", Conn, tran, this.Name.ToString(), this.Text) == false) return;
                        
            StrSql = "Delete From tbl_SalesitemDetail";
            StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
            StrSql = StrSql + " And   SalesItemIndex = " + SalesItemIndex;

            if (Temp_Connect.Delete_Data(StrSql, "tbl_SalesitemDetail", Conn, tran, this.Name.ToString(), this.Text) == false) return;
        }



        private void DB_Save_tbl_SalesItemDetail____U(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran,
                    string OrderNumber, int SalesItemIndex)
        {
            string StrSql = "";

            cls_Search_DB csd = new cls_Search_DB();
            string T_where = " And SalesItemIndex = " + SalesItemIndex.ToString () ;
            //수정하기 전에 배열에다가 내역을 받아둔다.
            csd.SalesDetail_Mod_BackUp(OrderNumber, "tbl_SalesitemDetail", T_where);


            StrSql = "Update tbl_SalesItemDetail Set ";

            StrSql = StrSql + " ItemCode= '" + SalesItemDetail[SalesItemIndex].ItemCode + "'";
            StrSql = StrSql + ",ItemPrice= " + SalesItemDetail[SalesItemIndex].ItemPrice;
            StrSql = StrSql + ",ItemPv= " + SalesItemDetail[SalesItemIndex].ItemPV;
            StrSql = StrSql + ",Itemcv= " + SalesItemDetail[SalesItemIndex].ItemCV;

            StrSql = StrSql + ",Sell_VAT_Price= " + SalesItemDetail[SalesItemIndex].Sell_VAT_Price;
            StrSql = StrSql + ",Sell_Except_VAT_Price= " + SalesItemDetail[SalesItemIndex].Sell_Except_VAT_Price;

            StrSql = StrSql + ",Total_Sell_VAT_Price= " + SalesItemDetail[SalesItemIndex].Total_Sell_VAT_Price;
            StrSql = StrSql + ",Total_Sell_Except_VAT_Price= " + SalesItemDetail[SalesItemIndex].Total_Sell_Except_VAT_Price;

            StrSql = StrSql + ",ItemTotalPrice= " + SalesItemDetail[SalesItemIndex].ItemTotalPrice;
            StrSql = StrSql + ",ItemTotalPV= " + SalesItemDetail[SalesItemIndex].ItemTotalPV;
            StrSql = StrSql + ",ItemTotalcV= " + SalesItemDetail[SalesItemIndex].ItemTotalCV;
    
            StrSql = StrSql + ",ItemCount= " + SalesItemDetail[SalesItemIndex].ItemCount;
            StrSql = StrSql + ",RecIndex= " + SalesItemDetail[SalesItemIndex].RecIndex;
    
        //    StrSql = StrSql + ",Send_itemCount1= " + Send_itemCount1;
        //    StrSql = StrSql + ",Send_itemCount2= " + Send_itemCount2;
    
            StrSql = StrSql + ",SellState= '" + SalesItemDetail[SalesItemIndex].SellState + "'";
            StrSql = StrSql + ",SendDate= '" + SalesItemDetail[SalesItemIndex].SendDate + "'";
            StrSql = StrSql + ",ETC= '" + SalesItemDetail[SalesItemIndex].Etc + "'";    
            StrSql = StrSql + ",G_Sort_Code= '" + SalesItemDetail[SalesItemIndex].G_Sort_Code + "'";
            
            StrSql = StrSql + " Where OrderNumber = '" + SalesItemDetail[SalesItemIndex].OrderNumber + "'";
            StrSql = StrSql + " And SalesItemIndex = " + SalesItemIndex.ToString();

            if (Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name.ToString(), this.Text) == false) return;
            
            //주문 상품 테이블의 변경 내역을 테이블에 넣는다.
            csd.tbl_SalesDetail_Total_Change(Conn, tran, OrderNumber,SalesItemIndex, "tbl_SalesitemDetail", T_where);
        }


        private void DB_Save_tbl_SalesItemDetail____S(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran,
                    string OrderNumber, int SalesItemIndex)
        {
            string StrSql = "";
            

            StrSql = "Insert Into tbl_SalesitemDetail (";            
            StrSql = StrSql + " SalesItemIndex,OrderNumber,";
            StrSql = StrSql + " ItemCode,ItemPrice,ItemPv,ItemCv,";
            StrSql = StrSql + " Sell_VAT_TF , Sell_VAT_Price, Sell_Except_VAT_Price,SellState,";
            StrSql = StrSql + " ItemCount,ItemTotalPrice,ItemTotalPV,ItemTotalcV,";
            StrSql = StrSql + " Total_Sell_VAT_Price, Total_Sell_Except_VAT_Price,";
            StrSql = StrSql + " ReturnDate,SendDate,ReturnBackDate,";
            StrSql = StrSql + " Etc,RecIndex,";                    
             StrSql = StrSql + " Send_itemCount1,Send_itemCount2, ";
             StrSql = StrSql + " T_OrderNumber1,T_OrderNumber2,G_Sort_Code  ";
            StrSql = StrSql + " ,RecordID,RecordTime ";
            StrSql = StrSql + " ) values("  ;

            StrSql = StrSql +  SalesItemDetail[SalesItemIndex].SalesItemIndex ;
            StrSql = StrSql + ",'" + OrderNumber + "'";

            StrSql = StrSql + ",'" + SalesItemDetail[SalesItemIndex].ItemCode + "'";
            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].ItemPrice;
            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].ItemPV;
            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].ItemCV;
            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].Sell_VAT_TF;
            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].Sell_VAT_Price;
            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].Sell_Except_VAT_Price;

            StrSql = StrSql + ",'" + SalesItemDetail[SalesItemIndex].SellState + "'";

            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].ItemCount;
            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].ItemTotalPrice;
            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].ItemTotalPV;
            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].ItemTotalCV;

            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].Total_Sell_VAT_Price;
            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].Total_Sell_Except_VAT_Price;

            StrSql = StrSql + ",'" + SalesItemDetail[SalesItemIndex].ReturnDate + "'";
            StrSql = StrSql + ",'" + SalesItemDetail[SalesItemIndex].SendDate + "'";
            StrSql = StrSql + ",'" + SalesItemDetail[SalesItemIndex].ReturnBackDate + "'";

            StrSql = StrSql + ",'" + SalesItemDetail[SalesItemIndex].Etc + "'";            
            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].RecIndex;

            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].Send_itemCount1;
            StrSql = StrSql + "," + SalesItemDetail[SalesItemIndex].Send_itemCount2;

            StrSql = StrSql + ",'" + OrderNumber + "'";
            StrSql = StrSql + ",'" + SalesItemDetail[SalesItemIndex].T_OrderNumber2 + "'";
            StrSql = StrSql + ",'" + SalesItemDetail[SalesItemIndex].G_Sort_Code + "'";            
            StrSql = StrSql + ",'" + SalesItemDetail[SalesItemIndex].RecordID + "'";
            StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
            StrSql = StrSql + " ) ";
                        
            if (Temp_Connect.Insert_Data(StrSql,"tbl_SalesItemDetail", Conn, tran, this.Name.ToString(), this.Text) == false) return;
           
        }






        private void DB_Save_tbl_Sales_Cacu(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran,
                    string OrderNumber)
        {

            foreach (int t_key in Sales_Cacu.Keys)
            {
                if (Sales_Cacu[t_key].Del_TF == "D") //삭제이다
                {
                    //백업데이블에 백업 받고 삭제 처리한다.
                    DB_Save_tbl_Sales_Cacu____D(Temp_Connect, Conn, tran, OrderNumber, t_key);
                }
                else if (Sales_Cacu[t_key].Del_TF == "U") //업데이트다 
                {
                    DB_Save_tbl_Sales_Cacu____U(Temp_Connect, Conn, tran, OrderNumber, t_key);
                }
                else if (Sales_Cacu[t_key].Del_TF == "S")  //새로운 저장이다
                {
                    DB_Save_tbl_Sales_Cacu____S(Temp_Connect, Conn, tran, OrderNumber, t_key);
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

            StrSql = StrSql + " C_TF= " + Sales_Cacu[C_index].C_TF ;
            StrSql = StrSql + ",C_Price1= " + Sales_Cacu[C_index].C_Price1;
            StrSql = StrSql + ",C_Price2= " + Sales_Cacu[C_index].C_Price2;

            StrSql = StrSql + ",C_AppDate1= '" + Sales_Cacu[C_index].C_AppDate1.Replace("-","") + "'";
            StrSql = StrSql + ",C_AppDate2= '" + Sales_Cacu[C_index].C_AppDate2.Replace("-","") + "'";

            StrSql = StrSql + ",C_CodeName= '" + Sales_Cacu[C_index].C_CodeName + "'";

            StrSql = StrSql + ",C_Number1= '" + encrypter.Encrypt(  Sales_Cacu[C_index].C_Number1) + "'";
            StrSql = StrSql + ",C_Number2= '" + encrypter.Encrypt( Sales_Cacu[C_index].C_Number2) + "'";
            StrSql = StrSql + ",C_Number3= '" + encrypter.Encrypt( Sales_Cacu[C_index].C_Number3) + "'";

            StrSql = StrSql + ",C_Name1= '" + Sales_Cacu[C_index].C_Name1 + "'";
            StrSql = StrSql + ",C_Name2= '" + Sales_Cacu[C_index].C_Name2 + "'";

            StrSql = StrSql + ",C_Code= '" + Sales_Cacu[C_index].C_Code + "'";
            StrSql = StrSql + ",C_Period1= '" + Sales_Cacu[C_index].C_Period1 + "'";
            StrSql = StrSql + ",C_Period2= '" + Sales_Cacu[C_index].C_Period2 + "'";
            StrSql = StrSql + ",C_Installment_Period= '" + Sales_Cacu[C_index].C_Installment_Period + "'";

            StrSql = StrSql + ",C_Etc= '" + Sales_Cacu[C_index].C_Etc + "'";

            ////StrSql = StrSql + ",C_CancelTF= " + Sales_Cacu[C_index].C_CancelTF;
            ////StrSql = StrSql + ",C_CancelDate= '" + Sales_Cacu[C_index].C_CancelDate + "'";
            ////StrSql = StrSql + ",C_CancelPrice= " + Sales_Cacu[C_index].C_CancelPrice;

            StrSql = StrSql + " Where OrderNumber = '" + Sales_Cacu[C_index].OrderNumber + "'";
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
           StrSql = StrSql + " C_index,OrderNumber," ;
            StrSql = StrSql + " C_TF,C_Code,C_CodeName,C_Name1,C_Name2," ;
            StrSql = StrSql + " C_Number1 , C_Number2, C_Number3, " ;
            StrSql = StrSql + " C_Price1,C_Price2,C_AppDate1,C_AppDate2, " ;
            StrSql = StrSql + " C_CancelTF, C_CancelDate,C_CancelPrice, " ;
            StrSql = StrSql + " C_Period1,C_Period2,C_Installment_Period,C_Etc";
            StrSql = StrSql + " ,RecordID,RecordTime ";
            StrSql = StrSql + " ) values(";

            StrSql = StrSql + "" + Sales_Cacu[C_index].C_index;
            StrSql = StrSql + ",'" + OrderNumber + "'";
            StrSql = StrSql + "," + Sales_Cacu[C_index].C_TF;

            StrSql = StrSql + ",'" + Sales_Cacu[C_index].C_Code + "'";
            StrSql = StrSql + ",'" + Sales_Cacu[C_index].C_CodeName + "'";
            StrSql = StrSql + ",'" + Sales_Cacu[C_index].C_Name1 + "'";
            StrSql = StrSql + ",'" + Sales_Cacu[C_index].C_Name2 + "'";

            StrSql = StrSql + ",'" + encrypter.Encrypt( Sales_Cacu[C_index].C_Number1) + "'";
            StrSql = StrSql + ",'" + encrypter.Encrypt( Sales_Cacu[C_index].C_Number2) + "'";
            StrSql = StrSql + ",'" + encrypter.Encrypt( Sales_Cacu[C_index].C_Number3) + "'";
            
            StrSql = StrSql + "," + Sales_Cacu[C_index].C_Price1;
            StrSql = StrSql + "," + Sales_Cacu[C_index].C_Price2;

            StrSql = StrSql + ",'" + Sales_Cacu[C_index].C_AppDate1.Replace("-","") + "'";
            StrSql = StrSql + ",'" + Sales_Cacu[C_index].C_AppDate2 + "'";

            StrSql = StrSql + "," + Sales_Cacu[C_index].C_CancelTF;
            StrSql = StrSql + ",'" + Sales_Cacu[C_index].C_CancelDate + "'";
            StrSql = StrSql + "," + Sales_Cacu[C_index].C_CancelPrice;

            StrSql = StrSql + ",'" + Sales_Cacu[C_index].C_Period1 + "'";
            StrSql = StrSql + ",'" + Sales_Cacu[C_index].C_Period2 + "'";
            StrSql = StrSql + ",'" + Sales_Cacu[C_index].C_Installment_Period + "'";
            StrSql = StrSql + ",'" + Sales_Cacu[C_index].C_Etc + "'";

            StrSql = StrSql + ",'" + Sales_Cacu[C_index].RecordID + "'";            

            StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
            StrSql = StrSql + " ) ";
            
            if (Temp_Connect.Insert_Data(StrSql, "tbl_Sales_Cacu", Conn, tran, this.Name.ToString(), this.Text) == false) return;

        }




        private void DB_Save_tbl_Sales_Rece(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran,
                    string OrderNumber)
        {

            foreach (int t_key in Sales_Rece.Keys)
            {
                if (Sales_Rece[t_key].Del_TF == "D") //삭제이다
                {
                    //백업데이블에 백업 받고 삭제 처리한다.
                    DB_Save_tbl_Sales_Rece____D(Temp_Connect, Conn, tran, OrderNumber, t_key);
                }
                else if (Sales_Rece[t_key].Del_TF == "U") //업데이트다 
                {
                    DB_Save_tbl_Sales_Rece____U(Temp_Connect, Conn, tran, OrderNumber, t_key);
                }
                else if (Sales_Rece[t_key].Del_TF == "S")  //새로운 저장이다
                {
                    DB_Save_tbl_Sales_Rece____S(Temp_Connect, Conn, tran, OrderNumber, t_key);
                }
            }
        }

        private void DB_Save_tbl_Sales_Rece____D(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran,
                    string OrderNumber, int SalesItemIndex)
        {
            string StrSql = "";

            StrSql = "Insert into tbl_Sales_Rece_Mod_Del  ";
            StrSql = StrSql + " Select * ,0,'" + cls_User.gid + "',Convert(Varchar(25),GetDate(),21) From tbl_Sales_Rece ";
            StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
            StrSql = StrSql + " And   SalesItemIndex = " + SalesItemIndex;

            if (Temp_Connect.Insert_Data(StrSql, "tbl_Sales_Rece", Conn, tran, this.Name.ToString(), this.Text) == false) return;

            StrSql = "Delete From tbl_Sales_Rece";
            StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
            StrSql = StrSql + " And   SalesItemIndex = " + SalesItemIndex;

            if (Temp_Connect.Delete_Data(StrSql, "tbl_Sales_Rece", Conn, tran, this.Name.ToString(), this.Text) == false) return;
        }



        private void DB_Save_tbl_Sales_Rece____U(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran,
                    string OrderNumber, int SalesItemIndex)
        {
            string StrSql = "";

            cls_Search_DB csd = new cls_Search_DB();
            string T_where = " And SalesItemIndex = " + SalesItemIndex.ToString();
            //수정하기 전에 배열에다가 내역을 받아둔다.
            csd.SalesDetail_Mod_BackUp(OrderNumber, "tbl_Sales_Rece", T_where);


            StrSql = "Update tbl_Sales_Rece Set "  ;

            StrSql = StrSql + " Receive_Method= " + Sales_Rece[SalesItemIndex].Receive_Method;
            StrSql = StrSql + ",SalesItemIndex= " + Sales_Rece[SalesItemIndex].SalesItemIndex;
            StrSql = StrSql + ",Get_Name1=  '" + Sales_Rece[SalesItemIndex].Get_Name1 + "'";
            StrSql = StrSql + ",Get_Name2=  '" + Sales_Rece[SalesItemIndex].Get_Name2 + "'";

            StrSql = StrSql + ",Get_Date1= '" + Sales_Rece[SalesItemIndex].Get_Date1.Replace("-", "") + "'";
            StrSql = StrSql + ",Get_Date2= '" + Sales_Rece[SalesItemIndex].Get_Date2.Replace("-", "") + "'";

            StrSql = StrSql + ",Pass_Number= '" + Sales_Rece[SalesItemIndex].Pass_Number + "'";

            StrSql = StrSql + ",Get_ZipCode= '" + Sales_Rece[SalesItemIndex].Get_ZipCode + "'";
            StrSql = StrSql + ",Get_Address1= '" + Sales_Rece[SalesItemIndex].Get_Address1 + "'";
            StrSql = StrSql + ",Get_Address2= '" + Sales_Rece[SalesItemIndex].Get_Address2 + "'";

            StrSql = StrSql + ",Get_Tel1= '" + Sales_Rece[SalesItemIndex].Get_Tel1 + "'";
            StrSql = StrSql + ",Get_Tel2= '" + Sales_Rece[SalesItemIndex].Get_Tel2 + "'";

            StrSql = StrSql + ",Get_Etc1= '" + Sales_Rece[SalesItemIndex].Get_Etc1 + "'";
            StrSql = StrSql + ",Get_Etc2= '" + Sales_Rece[SalesItemIndex].Get_Etc2 + "'";

            StrSql = StrSql + ",Pass_Pay= " + Sales_Rece[SalesItemIndex].Pass_Pay;
            StrSql = StrSql + ",Pass_Number2= '" + Sales_Rece[SalesItemIndex].Pass_Number2 + "'";

            StrSql = StrSql + ",Base_Rec= '" + Sales_Rece[SalesItemIndex].Base_Rec + "'";
           
            StrSql = StrSql + " Where OrderNumber = '" + Sales_Rece[SalesItemIndex].OrderNumber + "'";
            StrSql = StrSql + " And SalesItemIndex = " + SalesItemIndex.ToString();

            if (Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name.ToString(), this.Text) == false) return;

            //주문 상품 테이블의 변경 내역을 테이블에 넣는다.
            csd.tbl_SalesDetail_Total_Change(Conn, tran, OrderNumber, SalesItemIndex, "tbl_Sales_Rece", T_where);
        }


        private void DB_Save_tbl_Sales_Rece____S(
                    cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran,
                    string OrderNumber, int SalesItemIndex)
        {
            string StrSql = "";


            StrSql = "Insert Into tbl_Sales_Rece (";
            StrSql = StrSql + " RecIndex,SalesItemIndex,OrderNumber," ;
            StrSql = StrSql + " Receive_Method,Get_Date1,Get_Date2,Get_Name1,Get_Name2," ;
            StrSql = StrSql + " Get_ZipCode , Get_Address1, Get_Address2, " ;
            StrSql = StrSql + " Get_Tel1,Get_Tel2,Get_Etc1,Get_Etc2, " ;
            StrSql = StrSql + " Pass_Pay,Pass_Number,Base_Rec ";
            StrSql = StrSql + " ,RecordID,RecordTime ";
            StrSql = StrSql + " ) values(";

            StrSql = StrSql + "" + Sales_Rece[SalesItemIndex].SalesItemIndex;
            StrSql = StrSql + "," + Sales_Rece[SalesItemIndex].RecIndex;
            StrSql = StrSql + ",'" + OrderNumber + "'";

            StrSql = StrSql + "," + Sales_Rece[SalesItemIndex].Receive_Method ;

            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].Get_Date1.Replace("-", "") + "'";
            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].Get_Date2.Replace("-", "") + "'";
            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].Get_Name1 + "'";
            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].Get_Name2 + "'";

            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].Get_ZipCode + "'";
            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].Get_Address1 + "'";
            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].Get_Address2 + "'";

            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].Get_Tel1 + "'";
            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].Get_Tel2 + "'";

            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].Get_Etc1 + "'";
            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].Get_Etc2 + "'";

            StrSql = StrSql + "," + Sales_Rece[SalesItemIndex].Pass_Pay  ;
            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].Pass_Number + "'";
            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].Base_Rec + "'";            
            
            StrSql = StrSql + ",'" + Sales_Rece[SalesItemIndex].RecordID + "'";
            StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
            StrSql = StrSql + " ) ";

            if (Temp_Connect.Insert_Data(StrSql, "tbl_Sales_Rece", Conn, tran, this.Name.ToString(), this.Text) == false) return;


            

        }



        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
            string str_Q = "";

            if (txt_OrderNumber.Text == "")            
                str_Q = "Msg_Base_Save_Q";
            else            
                str_Q = "Msg_Base_Edit_Q";
                            
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString(str_Q), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (Check_TextBox_Error() == false) return;  //각종 입력 오류를 체크한다.

            if (txt_OrderNumber.Text.Trim() == "")
                Input_SalesDetail_dic();   //주문번호 ""으로 해서 판매 주 클래스 에 넣음
            else
                Update_SalesDetail_dic();  //판매 주 클래스에 대한 수정 작업

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            string T_ord_N = "";
            cls_Search_DB csd = new cls_Search_DB();

            try
            {
                //저장할 것에 대한 주문번호를 따온다          
                DB_Save_tbl_SalesDetail(Temp_Connect,Conn, tran, ref T_ord_N);

                //실질적인 저장,수정이 이루어지는곳. 변경시 주테이블 이전 내역도 같이 저장함
                DB_Save_tbl_SalesDetail____002(Temp_Connect, Conn, tran ,  T_ord_N );
            

                DB_Save_tbl_SalesItemDetail(Temp_Connect, Conn, tran, T_ord_N);

                DB_Save_tbl_Sales_Cacu(Temp_Connect, Conn, tran, T_ord_N);

                DB_Save_tbl_Sales_Rece(Temp_Connect, Conn, tran, T_ord_N);

                DB_Save_tbl_Mileage____001(Temp_Connect, Conn, tran, T_ord_N);
                                
                tran.Commit();
              
                Save_Error_Check =1;
                if (txt_OrderNumber.Text == "")      
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
                else
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
            }
            catch (Exception ee)
            {
                tran.Rollback();
                if (txt_OrderNumber.Text == "")
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
                else
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Err"));

            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }

            if (cls_app_static_var.Sell_Union_Flag == "D" && (txt_Ins_Number.Text.Trim() == "" || txt_Ins_Number.Text.Trim() == "미승인요청"))
            {
                InsuranceNumber_Ord_Print_FLAG = T_ord_N;
                Sell_Ac_insurancenumber(T_ord_N);//직판 관련 승인 번호를 받아온다.                
                InsuranceNumber_Ord_Print_FLAG = "";
            }

        }

        private void Sell_Ac_insurancenumber(string T_ord_N)
        {
            //string Req = "";
            //cls_Socket csg = new cls_Socket();
            //Req = csg.Dir_Connect_Send_Acc(T_ord_N);
            //
            //if (Req != "")
            //{
            //    if (Req == "-1")
            //    {
            //        MessageBox.Show("공제번호 발급 중계프로그램(dsclientA.exe)이 " + cls_app_static_var.Dir_Company_Name + " 중계서버에서 미실행 상태입니다."
            //                + "\n" +
            //                cls_app_static_var.Dir_Company_Name + " 전산담당자에게 연락하셔서 공제번호 발급이 되도록 요청바랍니다.");
            //    }
            //    else
            //    {
            //        MessageBox.Show("조합 관련 Error Number : " + Req);
            //    }
            //}
            //else
            //{                
            //
            //    MessageBox.Show("공제번호가 정상적으로 발급 되었습니다.");
            //    Button T_bt = butt_Print; EventArgs ee1 = null;
            //    butt_Print_Click(T_bt, ee1);  //정상 발급과 관련되 프린터 물을 출력한다.
            //}                    
        }

        private void dGridView_Base_Sub_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.Name == "dGridView_Base_Item")
            {
                if (dgv.CurrentRow != null && dgv.CurrentRow.Cells[0].Value != null)
                {
                    if (dgv.CurrentRow.Cells[0].Value.ToString () != "")
                        Put_Sub_Date(dgv.CurrentRow.Cells[0].Value.ToString(), "item");                    
                }
            }

            if (dgv.Name == "dGridView_Base_Rece")
            {
                if (dgv.CurrentRow != null &&  dgv.CurrentRow.Cells[0].Value != null)
                {
                    Put_Sub_Date(dgv.CurrentRow.Cells[0].Value.ToString(),"Rece");
                }
            }

            if (dgv.Name == "dGridView_Base_Cacu")
            {
                if (dgv.CurrentRow != null &&  dgv.CurrentRow.Cells[0].Value != null)
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

                    Put_Sub_Date(dgv.CurrentRow.Cells[0].Value.ToString(), "Cacu");
                }
            }


        }

        private void Put_Sub_Date(string SalesItemIndex , string t_STF )
        {
            if (t_STF == "item")
            {
                Data_Set_Form_TF = 1;
                txt_SalesItemIndex.Text = SalesItemIndex;

                butt_Item_Del.Visible = true;
                cls_form_Meth cm = new cls_form_Meth();
                butt_Item_Save.Text = cm._chang_base_caption_search("수정");
                int Salesitemindex = int.Parse(txt_SalesItemIndex.Text);
                txt_ItemCode.Text = SalesItemDetail[Salesitemindex].ItemCode;
                txt_ItemName.Text = SalesItemDetail[Salesitemindex].ItemName;
                txt_ItemCount.Text = SalesItemDetail[Salesitemindex].ItemCount.ToString();
                txt_Item_Etc.Text = SalesItemDetail[Salesitemindex].Etc;

                txt_ItemCode.ReadOnly = true;
                txt_ItemCode.BorderStyle = BorderStyle.FixedSingle;
                txt_ItemCode.BackColor = cls_app_static_var.txt_Enable_Color; 
                Data_Set_Form_TF = 0;
            }

            if (t_STF == "Rece")
            {
                Data_Set_Form_TF =1;
                txt_RecIndex.Text = SalesItemIndex;

                butt_Rec_Del.Visible = true;
                cls_form_Meth cm = new cls_form_Meth();
                butt_Rec_Save.Text = cm._chang_base_caption_search("수정");
                int Salesitemindex = int.Parse(txt_RecIndex.Text);

                txt_Receive_Method.Text = Sales_Rece[Salesitemindex].Receive_Method_Name.ToString();
                txt_Receive_Method_Code.Text = Sales_Rece[Salesitemindex].Receive_Method.ToString();
                txt_Get_Name1.Text = Sales_Rece[Salesitemindex].Get_Name1;

                mtxtZip1.Text = "";

                if (Sales_Rece[Salesitemindex].Get_ZipCode.ToString().Length >= 6)
                {
                    mtxtZip1.Text = Sales_Rece[Salesitemindex].Get_ZipCode.ToString().Substring(0, 3) + "-" + Sales_Rece[Salesitemindex].Get_ZipCode.ToString().Substring(3, 3);
                    //txtAddCode2.Text = Sales_Rece[Salesitemindex].Get_ZipCode.ToString().Substring(3, 3);
                }

                string T_Num_1 = ""; string T_Num_2 = ""; string T_Num_3 = "";
                cls_form_Meth cfm = new cls_form_Meth();
                //cfm.Phone_Number_Split(Sales_Rece[Salesitemindex].Get_Tel1.ToString(), ref T_Num_1, ref T_Num_2, ref T_Num_3);
                //txtTel_1.Text = T_Num_1; txtTel_2.Text = T_Num_2; txtTel_3.Text = T_Num_3;
                mtxtTel1.Text = Sales_Rece[Salesitemindex].Get_Tel1.ToString();

                //cfm.Phone_Number_Split(Sales_Rece[Salesitemindex].Get_Tel2.ToString(), ref T_Num_1, ref T_Num_2, ref T_Num_3);
                //txtTel2_1.Text = T_Num_1; txtTel2_2.Text = T_Num_2; txtTel2_3.Text = T_Num_3;
                mtxtTel2.Text = Sales_Rece[Salesitemindex].Get_Tel2.ToString();


                txtAddress1.Text = Sales_Rece[Salesitemindex].Get_Address1;
                txtAddress2.Text = Sales_Rece[Salesitemindex].Get_Address2;
                txtGetDate1.Text = Sales_Rece[Salesitemindex].Get_Date1.ToString().Replace ("-","");
                txt_Pass_Number.Text  = Sales_Rece[Salesitemindex].Pass_Number;
                txt_Base_Rec.Text  = Sales_Rece[Salesitemindex].Base_Rec;
                txt_Base_Rec_Code.Text = Sales_Rece[Salesitemindex].Base_Rec_Name  ;
                txt_Get_Etc1.Text= Sales_Rece[Salesitemindex].Get_Etc1  ;

                Rece_Item_Grid_Set(int.Parse(SalesItemIndex));

                Data_Set_Form_TF = 0;
            }


            if (t_STF == "Cacu")
            {
                Data_Set_Form_TF = 1;




                txt_C_index.Text = SalesItemIndex;

                butt_Cacu_Del.Visible = true;
                cls_form_Meth cm = new cls_form_Meth();
                butt_Cacu_Save.Text = cm._chang_base_caption_search("수정");
                int C_index = int.Parse(txt_C_index.Text);

                txt_C_Etc.Text = Sales_Cacu[C_index].C_Etc.ToString();
                //= string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalInputPrice);

                if (Sales_Cacu[C_index].C_TF == 1)
                {                    
                    txt_Price_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu[C_index].C_Price1);
                    mtxtPriceDate1.Text = Sales_Cacu[C_index].C_AppDate1.ToString().Replace ("-","") ;
                    tab_Cacu.SelectedIndex = 1;
                }

                if (Sales_Cacu[C_index].C_TF == 2)
                {
                    txt_Price_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu[C_index].C_Price1);
                    mtxtPriceDate2.Text = Sales_Cacu[C_index].C_AppDate1.ToString().Replace("-", "");
                    txt_C_Name_2.Text = Sales_Cacu[C_index].C_Name1.ToString();
                    txt_C_Bank.Text = Sales_Cacu[C_index].C_CodeName_2.ToString();
                    txt_C_Bank_Code.Text = Sales_Cacu[C_index].C_Code.ToString();
                    txt_C_Bank_Code_2.Text = Sales_Cacu[C_index].C_CodeName.ToString();
                    txt_C_Bank_Code_3.Text = Sales_Cacu[C_index].C_Number1.ToString();

                    tab_Cacu.SelectedIndex = 2;
                }


                if (Sales_Cacu[C_index].C_TF == 3)
                {
                    txt_Price_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu[C_index].C_Price1);
                    mtxtPriceDate3.Text = Sales_Cacu[C_index].C_AppDate1.ToString().Replace("-", "");
                    txt_C_Name_3.Text = Sales_Cacu[C_index].C_Name1.ToString();
                    txt_C_Card.Text = Sales_Cacu[C_index].C_CodeName.ToString();
                    txt_C_Card_Code.Text = Sales_Cacu[C_index].C_Code.ToString();
                    txt_C_Card_Number.Text = Sales_Cacu[C_index].C_Number1.ToString();
                    txt_C_Card_Ap_Num.Text = Sales_Cacu[C_index].C_Number2.ToString();
                    txt_C_Card_Ap_Num.Text = Sales_Cacu[C_index].C_Number2.ToString();
                    txt_Price_3_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu[C_index].C_Price2);
                    combo_C_Card_Year.Text = Sales_Cacu[C_index].C_Period1.ToString();
                    combo_C_Card_Month.Text = Sales_Cacu[C_index].C_Period2.ToString();
                    combo_C_Card_Per.Text = Sales_Cacu[C_index].C_Installment_Period.ToString();

                    tab_Cacu.SelectedIndex = 0;
                }


                if (Sales_Cacu[C_index].C_TF == 4)
                {
                    txt_Price_4.Text = string.Format(cls_app_static_var.str_Currency_Type, Sales_Cacu[C_index].C_Price1);
                    mtxtPriceDate4.Text = Sales_Cacu[C_index].C_AppDate1.ToString().Replace("-", "");

                    double T_p = 0;
                    string T_Mbid = mtxtMbid.Text;
                    string Mbid = ""; int Mbid2 = 0;
                    cls_Search_DB csb = new cls_Search_DB();
                    if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == 1)
                    {
                        cls_tbl_Mileage ctm = new cls_tbl_Mileage();
                        T_p = ctm.Using_Mileage_Search(Mbid, Mbid2, cls_User.gid_date_time);
                        txt_Price_4_2.Text = string.Format(cls_app_static_var.str_Currency_Type, T_p);
                    }

                    tab_Cacu.SelectedIndex = 3;
                }
             
                Data_Set_Form_TF = 0;
            }
        }







        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {

            Base_Ord_Clear();

            if ((sender as DataGridView).CurrentRow != null && (sender as DataGridView).CurrentRow.Cells[2].Value != null)
            {
                if ((sender as DataGridView).CurrentRow.Cells[2].Value.ToString () != "")
                {
                    string OrderNumber = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString();

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

            if (Sales_Cacu != null)
                Sales_Cacu.Clear();

            Set_SalesItemDetail(OrderNumber);  //상품 
            Set_Sales_Cacu(OrderNumber);  // 결제 
            Set_Sales_Rece(OrderNumber);  // 배송 

            Item_Grid_Set(); //상품 그리드
            Cacu_Grid_Set(); //결제 그리드
            Rece_Grid_Set(); //배송 그리드

            
        }


        private void Set_SalesDetail(string OrderNumber)
        {
            Data_Set_Form_TF = 1;
            if(SalesDetail.Count == 0 )
            {
                Data_Set_Form_TF = 0;
                return;
            }
            mtxtSellDate.Text = SalesDetail[OrderNumber].SellDate.Replace("-", "");
            mtxtSellDate2.Text = SalesDetail[OrderNumber].SellDate_2.Replace("-", "");
            txtSellCode.Text = SalesDetail[OrderNumber].SellCodeName ;
            txtSellCode_Code.Text = SalesDetail[OrderNumber].SellCode ;
            txtCenter2.Text = SalesDetail[OrderNumber].BusCodeName ;
            txtCenter2_Code.Text = SalesDetail[OrderNumber].BusCode;

            txt_Ins_Number.Text = SalesDetail[OrderNumber].INS_Num ;
            //string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["Last_price"]);
            txt_OrderNumber.Text = OrderNumber;
            txt_TotalPrice.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalPrice);
            txt_TotalPv.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalPV);
            txt_TotalCV.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalCV );

            txt_TotalInputPrice.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalInputPrice );
            txt_UnaccMoney.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].UnaccMoney);

            txt_ETC1.Text = SalesDetail[OrderNumber].Etc1;
            txt_ETC2.Text = SalesDetail[OrderNumber].Etc2;
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



        private void Set_Sales_Rece(string OrderNumber)
        {

            string strSql = "";

            strSql = "Select tbl_Sales_Rece.*  ";
            strSql = strSql + " , Isnull(tbl_Base_Rec.name ,'' ) Base_Rec_Name ";
            strSql = strSql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " Receive_Method_Name ";
            strSql = strSql + " From tbl_Sales_Rece (nolock) ";
            strSql = strSql + " LEFT JOIN tbl_Base_Rec (nolock) on tbl_Base_Rec.ncode = tbl_Sales_Rece.Base_Rec ";
            strSql = strSql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_Sales_Rece' And  Ch_T.M_Detail = Convert(Varchar,tbl_Sales_Rece.Receive_Method) ";
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
                t_c_sell.Get_Address1 = encrypter.Decrypt ( ds.Tables[base_db_name].Rows[fi_cnt]["Get_Address1"].ToString());
                t_c_sell.Get_Address2 = encrypter.Decrypt ( ds.Tables[base_db_name].Rows[fi_cnt]["Get_Address2"].ToString());

                t_c_sell.Get_Tel1 = encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt]["Get_Tel1"].ToString());
                t_c_sell.Get_Tel2 = encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt]["Get_Tel2"].ToString());

                t_c_sell.Pass_Number = ds.Tables[base_db_name].Rows[fi_cnt]["Pass_Number"].ToString();
                t_c_sell.Pass_Pay = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Pass_Pay"].ToString());
                                
                t_c_sell.Pass_Number2 = ds.Tables[base_db_name].Rows[fi_cnt]["Pass_Number2"].ToString();                
                t_c_sell.Base_Rec = ds.Tables[base_db_name].Rows[fi_cnt]["Base_Rec"].ToString();
                t_c_sell.Base_Rec_Name = ds.Tables[base_db_name].Rows[fi_cnt]["Base_Rec_Name"].ToString();

                t_c_sell.Get_Etc1 = ds.Tables[base_db_name].Rows[fi_cnt]["Get_Etc1"].ToString();
                t_c_sell.Get_Etc2 = ds.Tables[base_db_name].Rows[fi_cnt]["Get_Etc2"].ToString();
                
                

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

            Sales_Rece = T_Sales_Rece;
        }




        private void Set_Sales_Cacu(string OrderNumber)
        {

            string strSql = "";

            strSql = "Select tbl_Sales_Cacu.* ";
            strSql = strSql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " C_TF_Name ";
            strSql = strSql + " , Isnull(tbl_BankForCompany.BankPenName , '')  C_CodeName_2 ";
            strSql = strSql + " From tbl_Sales_Cacu (nolock) ";
            strSql = strSql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber ";
            strSql = strSql + " LEFT JOIN tbl_BankForCompany (nolock) ON tbl_Sales_Cacu.C_Code = tbl_BankForCompany.BankCode And  tbl_Sales_Cacu.C_Number1 = tbl_BankForCompany.BankAccountNumber And tbl_SalesDetail.Na_Code = tbl_BankForCompany.Na_Code";
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
                t_c_sell.C_Number1 = encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt]["C_Number1"].ToString());
                t_c_sell.C_Number2 = encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt]["C_Number2"].ToString());
                t_c_sell.C_Number3 = encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt]["C_Number3"].ToString());

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
            }

            Sales_Cacu = T_Sales_Cacu;
        }



        private void Set_SalesItemDetail(string Mbid , int Mbid2)
        {
            cls_form_Meth cm = new cls_form_Meth();
            string strSql = "";

            strSql = "Select Isnull(Sum(tbl_SalesitemDetail.ItemCount), 0 )   ";
            strSql = strSql + " , tbl_Goods.Name Item_Name ";
            //strSql = strSql + " ,Case When SellState = 'N_1' Then '" + cm._chang_base_caption_search("정상") + "'";
            //strSql = strSql + "  When SellState = 'N_3' Then '" + cm._chang_base_caption_search("교환_정상") + "'";
            //strSql = strSql + "  When SellState = 'R_1' Then '" + cm._chang_base_caption_search("반품") + "'";
            //strSql = strSql + "  When SellState = 'R_3' Then '" + cm._chang_base_caption_search("교환_반품") + "'";
            //strSql = strSql + " END  SellStateName ";

            strSql = strSql + " From tbl_SalesitemDetail (nolock) ";
            strSql = strSql + " LEFT JOIN tbl_Goods (nolock) ON tbl_Goods.Ncode = tbl_SalesitemDetail.ItemCode ";
            strSql = strSql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_SalesitemDetail.OrderNumber ";
            strSql = strSql + " Where tbl_SalesDetail.Mbid = '" + Mbid.ToString() + "'";
            strSql = strSql + " And   tbl_SalesDetail.Mbid2 = " + Mbid2;
            strSql = strSql + " And   ItemCount > 0 " ;
            strSql = strSql + " And tbl_SalesDetail.SellCode = '' ";
            strSql = strSql + " Group By tbl_Goods.Name ";
            strSql = strSql + " Order By tbl_Goods.Name ASC ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            //Dictionary<string, int> T_SalesitemDetail = new Dictionary<string, int>();
            int ItemCnt = 0; string ItemCode = "";

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                ItemCode = ds.Tables[base_db_name].Rows[fi_cnt]["Item_Name"].ToString();
                ItemCnt = int.Parse (ds.Tables[base_db_name].Rows[fi_cnt][0].ToString());
                //Push_data(series_Item, ItemCode.Replace(" ", "").Substring(0, 5), ItemCnt);
            }

            
        }



        private void chk_Total_MouseClick(object sender, MouseEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            dGridView_Base_Rece_Item.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;

            if (chk_Total.Checked == true)
            {
                for (int i = 0; i < dGridView_Base_Rece_Item.RowCount; i++)
                {
                    dGridView_Base_Rece_Item.Rows[i].Cells[0].Value = "V";
                }
            }
            else
            {
                for (int i = 0; i < dGridView_Base_Rece_Item.RowCount; i++)
                {
                    dGridView_Base_Rece_Item.Rows[i].Cells[0].Value = "";
                }
            }

            dGridView_Base_Rece_Item.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.Cursor = System.Windows.Forms.Cursors.Default;

        }

        private void tab_Cacu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab_Cacu.SelectedIndex == 0)
                txt_Price_3.Focus();
            if (tab_Cacu.SelectedIndex == 1)
                txt_Price_1.Focus();
            if (tab_Cacu.SelectedIndex == 2)
                txt_Price_2.Focus();
        }

        private void txt_SOrd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string T_Orde = txt_SOrd.Text ;
                string strSql = "";

                strSql = "Select Mbid,Mbid2 ";
                strSql = strSql + " From tbl_SalesDetail (nolock) ";
                strSql = strSql + " Where tbl_SalesDetail.OrderNumber = '" + txt_SOrd.Text.ToString() + "'";
                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0)
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_OrderNumber_Not_Exist")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    return;
                }
                //++++++++++++++++++++++++++++++++
                string Send_Number = "";
                if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                    Send_Number = ds.Tables[base_db_name].Rows[0]["Mbid"].ToString () + "-" + ds.Tables[base_db_name].Rows[0]["Mbid2"].ToString();
                else if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                    Send_Number = ds.Tables[base_db_name].Rows[0]["Mbid2"].ToString();
                else if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                    Send_Number = ds.Tables[base_db_name].Rows[0]["Mbid"].ToString();

                


                mtxtMbid.Text = Send_Number;
                Set_Form_Date(mtxtMbid.Text, "m");

               
                if (T_Orde != "")
                {                    
                    Base_Ord_Clear();
                    txt_SOrd.Text = T_Orde;
                    Put_OrderNumber_SellDate(T_Orde);
                }
            }
            
        }




        private void Push_data(Series series, string p, int p_3)
        {
            DataPoint dp = new DataPoint();
            dp.SetValueXY(p, p_3);
            dp.Label = string.Format(cls_app_static_var.str_Currency_Type, p_3); // p_3.ToString(); //p_3.ToString();
            series.Points.Add(dp);
        }

        //Push_data(series_Item, nodeKey.ToString() + "Line", Save_Cnt[nodeKey]);
        private void Save_Nom_Line_Chart()
        {
            cls_form_Meth cm = new cls_form_Meth();
            //series_Item.Name = cm._chang_base_caption_search("상품별");            
            chart_Item.Series.Clear();
            series_Item.Points.Clear();
            series_Item["DrawingStyle"] = "Emboss";
            series_Item["PointWidth"] = "0.5";
            series_Item.Name = cm._chang_base_caption_search("수량");
            series_Item.ChartType = SeriesChartType.Column ;
            series_Item.Legend = "Legend1";
            chart_Item.Series.Add(series_Item);

            chart_Item.ChartAreas[0].AxisX.Interval = 1;
            chart_Item.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_Item.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 7;
        }






        private void Reset_Chart_Total()
        {
            //chart_Mem.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();

            if (cls_app_static_var.Using_Mileage_TF == 0)
            {
                double[] yValues = { 0, 0, 0 };
                string[] xValues = { cm._chang_base_caption_search("현금"), cm._chang_base_caption_search("카드"), cm._chang_base_caption_search("무통장") };
                chart_Mem.Series["Series1"].Points.DataBindXY(xValues, yValues);
            }
            else
            {
                double[] yValues = { 0, 0, 0, 0 };
                string[] xValues = { cm._chang_base_caption_search("현금"), cm._chang_base_caption_search("카드"), cm._chang_base_caption_search("무통장"), cm._chang_base_caption_search("마일리지") };
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
                double[] yValues_2 = new double[ReCnt];
                string[] xValues_2 = new string[ReCnt]; // { cm._chang_base_caption_search(""), cm._chang_base_caption_search("탈퇴") }; 

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    yValues_2[fi_cnt] = 0;
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

            chart_Item.Series.Clear();
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

        private void butt_Mile_Search_Click(object sender, EventArgs e)
        {
            //회원을 선택 안햇네 그럼 회원 넣어라
            if (txtName.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Mem")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtMbid.Focus(); return;
            }

            dGridView_Base_Mile.Width = groupBox3.Width - 10 ;
            dGridView_Base_Mile.Height = groupBox3.Height - 18;
            dGridView_Base_Mile.Left = groupBox3.Left + 5 ;
            dGridView_Base_Mile.Top = groupBox3.Top + 15 ;

            Mile_Grid_Set();
                        
            dGridView_Base_Mile.BringToFront();
            //dGridView_Base_Mile.RowHeadersVisible = false;
            dGridView_Base_Mile.Visible = true;
            dGridView_Base_Mile.Focus();
        }



        private void Mile_Grid_Set()
        {
            dGridView_Mile_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Mile.d_Grid_view_Header_Reset();
            string strSql = "";

            strSql = "SELECT T_Time,PlusValue, MinusValue, "  ;            
            strSql = strSql + " Case  When PlusValue > 0 then C1.T_Name When MinusValue >0  then C2.T_Name End " ;
            strSql = strSql + " ,Plus_OrderNumber ";

            strSql = strSql + " ,Minus_OrderNumber,User_id ,'', '', ''" ;    
            strSql = strSql + " From tbl_Member_Mileage (nolock) " ;
            strSql = strSql + " LEFT Join tbl_Member_Mileage_Code C1  (nolock) ON tbl_Member_Mileage.PlusKind = C1.T_Code ";
            strSql = strSql + " LEFT Join tbl_Member_Mileage_Code C2  (nolock) ON tbl_Member_Mileage.MinusKind = C2.T_Code " ;
    

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                strSql = strSql + " Where tbl_Member_Mileage.Mbid = '" + idx_Mbid + "' ";
                strSql = strSql + " And   tbl_Member_Mileage.Mbid2 = " + idx_Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
            {
                strSql = strSql + " Where tbl_Member_Mileage.Mbid2 = " + idx_Mbid2.ToString();
            }

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
            {
                strSql = strSql + " Where tbl_Member_Mileage.Mbid = '" + idx_Mbid.ToString() + "'";
            }


    
            strSql = strSql + " Order by T_Time DESC" ;
    


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSql, "TempTable", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt == 0) return;

            double S_cnt1 = 0; double S_cnt2 = 0; int fi_cnt2 = 0;
            cls_form_Meth cm = new cls_form_Meth();

            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_Mile_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                S_cnt1 = S_cnt1 + double.Parse (ds.Tables["TempTable"].Rows[fi_cnt][1].ToString ());
                S_cnt2 = S_cnt2 + double.Parse(ds.Tables["TempTable"].Rows[fi_cnt][2].ToString());

                fi_cnt2 = fi_cnt ;
            }

            
            object[] row0 = { "<< " + cm._chang_base_caption_search("합계") + " >>"
                                ,S_cnt1
                                ,S_cnt2
                                ,string.Format(cls_app_static_var.str_Currency_Type, S_cnt1 - S_cnt2 )
                                ,""
                              
                                ,""
                                ,""
                                ,""
                                ,""
                                ,""      
                            };

            gr_dic_text[fi_cnt2 + 2] = row0;

            cgb_Mile.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Mile.db_grid_Obj_Data_Put();
        }

        private void Set_gr_Mile_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {

            object[] row0 = { ds.Tables["TempTable"].Rows[fi_cnt][0]
                                ,ds.Tables["TempTable"].Rows[fi_cnt][1]
                                ,ds.Tables["TempTable"].Rows[fi_cnt][2]
                                ,ds.Tables["TempTable"].Rows[fi_cnt][3]
                                ,ds.Tables["TempTable"].Rows[fi_cnt][4]
 
                                ,ds.Tables["TempTable"].Rows[fi_cnt][5]
                                ,ds.Tables["TempTable"].Rows[fi_cnt][6]
                                ,""
                                ,""
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Mile_Header_Reset()
        {
            cgb_Mile.Grid_Base_Arr_Clear();
            cgb_Mile.basegrid = dGridView_Base_Mile;
            cgb_Mile.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Mile.grid_col_Count = 10;
            cgb_Mile.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            cgb_Mile.Sort_Mod_Auto_TF = 1 ;

            string[] g_HeaderText = {"기록일"  , "적립액"   , "사용액"  , "구분"   , "적립_주문번호"        
                                , "사용_주문번호"   , "기록자"    , ""  , "" , ""
                                };

            int[] g_Width = { 80 ,80, 250, 200, 120
                                ,120 , 100 , 0 , 0 , 0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft  //5    
  
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleLeft  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };

            cgb_Mile.grid_col_header_text = g_HeaderText;
            cgb_Mile.grid_col_w = g_Width;
            cgb_Mile.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                            
                                   };
            cgb_Mile.grid_col_Lock = g_ReadOnly;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[2 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb_Mile.grid_cell_format = gr_dic_cell_format;

            //cgb_Mile.basegrid.RowHeadersVisible = false;
        }






        ////private void btnWord_Click(object sender, EventArgs e)
        ////{
        ////    sfd.Filter = "Word Documents (*.doc)|*.doc";
        ////    if (sfd.ShowDialog() == DialogResult.OK)
        ////    {
        ////        ExportToWord();
        ////    }
        ////}


        ////private void ExportToWord()
        ////{
        ////    string strForPrint = "";
        ////    //writing bill fields 
        ////    strForPrint += "Bill No : 12345678" + "\t";
        ////    strForPrint += "Date : 2014-05-20" + "\r\n\r\n";
        ////    strForPrint += "Customer Name : 이런저런아이" + "\r\n\r\n";
        ////    strForPrint += "Remarks : 잘되야 합니다." + "\r\n\r\n\r\n";
        ////    strForPrint += "-----Bill Detail-----" + "\r\n\r\n\r\n";
        ////    // writing datagridview column titles: 
        ////    string strHeaderTitle = "";
        ////    //for (int j = 0; j < dgDetail.Columns.Count; j++) { strHeaderTitle = strHeaderTitle.ToString() + Convert.ToString(dgDetail.Columns[j].HeaderText) + "\t\t"; } strForPrint += strHeaderTitle + "\r\n"; // writing datagridview data. for (int i = 0; i < dgDetail.RowCount - 1; i++) { string strLineData = ""; for (int j = 0; j < dgDetail.Rows[i].Cells.Count; j++) { strLineData = strLineData.ToString() + Convert.ToString(dgDetail.Rows[i].Cells[j].Value); if (j == 1) { strLineData = strLineData + "\t\t\t"; } else { strLineData = strLineData + "\t\t"; } } strForPrint += strLineData + "\r\n"; } 
        ////    Encoding utf16 = Encoding.GetEncoding(1254);
        ////    byte[] output = utf16.GetBytes(strForPrint);

        ////    FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
        ////    BinaryWriter bw = new BinaryWriter(fs); bw.Write(output, 0, output.Length); //write data into file
        ////    bw.Flush();
        ////    bw.Close();
        ////    fs.Close();
        ////}







        private void butt_Print_Click(object sender, EventArgs e)
        {
            if (txt_OrderNumber.Text == "" && InsuranceNumber_Ord_Print_FLAG == "") return;

            print_Page = 0; 
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            prPrview.ShowDialog();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        

        private void BaseDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void BaseDoc_PrintPage____001(System.Drawing.Printing.PrintPageEventArgs e, ref Rectangle t_f, ref RectangleF tt, ref int BaseitemH, ref int BaseitemH2, ref int BaseitemH3, int Y_tGap)
        {
                      
            string msg = "";
            Pen T_p = new Pen(Color.Black);
            int pageW = e.PageBounds.Width - 20 , pageH = e.PageBounds.Height ;
            int X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;

            //글자 크기 10을  높이 20으로 잡으면될듯함.
            int plus_g = 0;

            if (Y_tGap > 0)
                plus_g = 35;
            
            //거래명세표 글자를 찍는다.
            tt.X = (pageW / 2) - 70;
            tt.Y = 25 + Y_tGap - plus_g;
            msg = "거 래 명 세 표";
            FontStyle fs = FontStyle.Bold;
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 18, fs), Brushes.Black, tt);

            tt.X = (pageW / 2) - 70;
            tt.Y = 55 + Y_tGap - plus_g;
            if (Y_tGap >0 )
                msg = "(공 급 자 보 관 용)";            
            else
                msg = "(공급받는자 보관용)";            
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = 25;
            tt.Y = 75 + Y_tGap - plus_g;
            msg = "판매일자:" + mtxtSellDate.Text.Trim() ;
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = (pageW / 2) - 70;
            tt.Y = 75 + Y_tGap - plus_g;
            msg = "공제번호:" + txt_Ins_Number.Text.Trim ();
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
                        

            //프린터물 가장 테두리선을 그린다.
            
                 
           
            t_f.X = 20;
            if (Y_tGap ==  0 )
                t_f.Y = 20 + Y_tGap;
            else
                t_f.Y = 20 + Y_tGap - plus_g;

            t_f.Height = ((pageH - (20 * 2) )/ 2) - 40 ;
            t_f.Width = pageW - (t_f.X * 2)  ;
            e.Graphics.DrawRectangle(T_p, t_f);


            // 거래명서표 글자 아래 가로 선을 긋는다 -------
            X1 = t_f.X;           X2 = pageW - t_f.X ;
            Y1 = t_f.Y + 75 ; Y2 = t_f.Y + 75;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);



            BaseitemH = t_f.Y + 75 ;

            int Cnt = 0 ;
            Cnt = 1; 
            while (Cnt <= 3)
            {
                X1 = t_f.X + 20; 
                X2 = (pageW /2)- 10;
                Y1 =  BaseitemH + (30 * Cnt);
                Y2 =  BaseitemH + (30 * Cnt);
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

                X1 = (pageW / 2) + 10;
                X2 = pageW - t_f.X;
                Y1 = BaseitemH + (30 * Cnt);
                Y2 = BaseitemH + (30 * Cnt);
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

                BaseitemH2 = Y1;
                Cnt++;
            }
            
            BaseitemH = BaseitemH2;


            int Base_Line = 20, Base_Font_H = 10;
            
            Cnt = 0; 
            while (Cnt <= 17)
            {
                X1 = t_f.X; X2 = pageW - t_f.X;
                Y1 = BaseitemH + (Base_Line * Cnt);
                Y2 = BaseitemH + (Base_Line * Cnt);
                e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

                BaseitemH3 = BaseitemH + (Base_Line * Cnt);
                Cnt++;
            }
            double Sum_Item_cnt = 0, Sum_ItemPr = 0, Sum_ItemTotalPr = 0;
            int fi_cnt = 3, item_Base_Gap = 4;
            foreach (int t_key in SalesItemDetail.Keys)
            {
                if (SalesItemDetail[t_key].Del_TF != "D")
                {
                    tt.X = t_f.X                         ;
                    tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    msg =  SalesItemDetail[t_key].ItemName.ToString () ;
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
                    

                    tt.X = (pageW / 2);
                    tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    msg = SalesItemDetail[t_key].ItemCount.ToString();
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
                    Sum_Item_cnt = Sum_Item_cnt + SalesItemDetail[t_key].ItemCount;

                    tt.X = (pageW - 320);
                    tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    msg =   string.Format(cls_app_static_var.str_Currency_Type, SalesItemDetail[t_key].ItemPrice ); 
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
                    Sum_ItemPr = Sum_ItemPr + SalesItemDetail[t_key].ItemPrice;

                    tt.X = (pageW - 150);
                    tt.Y = BaseitemH + (Base_Line * fi_cnt) - Base_Font_H - item_Base_Gap;
                    msg = string.Format(cls_app_static_var.str_Currency_Type, SalesItemDetail[t_key].ItemTotalPrice );
                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
                    Sum_ItemTotalPr = Sum_ItemTotalPr + SalesItemDetail[t_key].ItemTotalPrice;

                    fi_cnt++;
                }
                
            }

            int Base_Font_H_2 = Base_Font_H - 5;

            tt.X = 30; tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = "공급가액";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = 100; tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[txt_OrderNumber.Text.Trim()].Total_Sell_Except_VAT_Price); 
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = 270; tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = "부가세액";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = 340; tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[txt_OrderNumber.Text.Trim()].Total_Sell_VAT_Price );
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = (pageW - 300);
            tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = "합계금액";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = (pageW - 300) + 70 ;
            tt.Y = BaseitemH2 + Base_Font_H_2;
            msg = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[txt_OrderNumber.Text.Trim()].TotalPrice );
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);



            tt.X = 180;
            tt.Y = BaseitemH2 + (Base_Line) + Base_Font_H_2;
            msg = "품명 및 규격" ;
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = (pageW / 2);
            tt.Y = BaseitemH2 + (Base_Line) + Base_Font_H_2;
             msg = "수량";
             e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

             tt.X = (pageW - 320);
             tt.Y = BaseitemH2 + (Base_Line) + Base_Font_H_2;
            msg = "회원단가";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);


            tt.X = (pageW - 150);
            tt.Y = BaseitemH2 + (Base_Line) + Base_Font_H_2;
            msg = "회원가합" ;
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);



            tt.X = 30; tt.Y = BaseitemH3 + Base_Font_H - 5;
            msg = "인수자";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);

            tt.X = 300; tt.Y = BaseitemH3 + Base_Font_H - 5;
            msg = "인";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 9), Brushes.Black, tt);

            tt.X = 360 ; tt.Y = BaseitemH3 + Base_Font_H - 5;
            msg = "합계";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10, fs), Brushes.Black, tt);



            tt.X = (pageW / 2);
            tt.Y = BaseitemH3 + Base_Font_H - 5;
            msg = string.Format(cls_app_static_var.str_Currency_Type, Sum_Item_cnt);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
            

            tt.X = (pageW - 320);
            tt.Y = BaseitemH3 + Base_Font_H - 5;
            msg = string.Format(cls_app_static_var.str_Currency_Type, Sum_ItemPr);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
            
            tt.X = (pageW - 150);
            tt.Y = BaseitemH3 + Base_Font_H - 5;
            msg = string.Format(cls_app_static_var.str_Currency_Type, Sum_ItemTotalPr);
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
        }



        private void BaseDoc_PrintPage____002(System.Drawing.Printing.PrintPageEventArgs e, Rectangle t_f, int BaseitemH2, int BaseitemH3, int Y_tGap)
        {
            //RectangleF tt = new RectangleF();
            
            //string msg = "";
            Pen T_p = new Pen(Color.Black);
            int pageW = e.PageBounds.Width - 20, pageH = e.PageBounds.Height;
            int X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;




            int Base_Line = 20;

            //수량이라는 글자 앞뒤선을 그린다./////////////////////////////////////////
            X1 = (pageW / 2) - 5;
            X2 = X1;
            Y1 = BaseitemH2 + Base_Line ;
            Y2 = t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            X1 = (pageW / 2) + 45;
            X2 = X1;
            Y1 = BaseitemH2 + Base_Line;
            Y2 = t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);
            //수량이라는 글자 앞뒤선을 그린다./////////////////////////////////////////


            //회원단가와 회원가합 사이의 선을 그린다.
            X1 = (pageW - 200);
            X2 = X1;
            Y1 = BaseitemH2 + Base_Line;
            Y2 = t_f.Y + t_f.Height ;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);



            //인수자와 인 사이의 선을 그린다.
            X1 = 90;
            X2 = X1;
            Y1 = BaseitemH3;
            Y2 = t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);



            //인과 합계 사이의 선을 그린다.
            X1 = 340;
            X2 = X1;
            Y1 = BaseitemH3;
            Y2 = t_f.Y + t_f.Height;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //공급가액과 부가세액 사이의 선
            X1 = 95;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH2 + Base_Line;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //공급가액과 부가세액 사이의 선
            X1 = 268;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH2 + Base_Line;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //부가세액뒤의선
            X1 = 340;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH2 + Base_Line;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //합계금액앞의선
            X1 = (pageW - 300)-2;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH2 + Base_Line;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            //합계금액뒤의선
            X1 = (pageW - 300) + 70;
            X2 = X1;
            Y1 = BaseitemH2;
            Y2 = BaseitemH2 + Base_Line;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

        }


        private void BaseDoc_PrintPage____003(System.Drawing.Printing.PrintPageEventArgs e, Rectangle t_f, int BaseitemH2, int BaseitemH3, int Y_tGap)
        {
            RectangleF tt = new RectangleF();
            
            string msg = "";
            Pen T_p = new Pen(Color.Black);
            int pageW = e.PageBounds.Width - 20, pageH = e.PageBounds.Height;
            int X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;
            //FontStyle fs = FontStyle.Bold;
            

            int Base_Line = 20, Base_Font_H = 10 ; //,  BaseitemH = t_f.Y + 75;;

            //공급자 뒷선
            X1 = t_f.X + 20;
            X2 = X1;
            Y1 = t_f.Y + 75;
            Y2 = BaseitemH2;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = t_f.X + 2 ;
            tt.Y = (t_f.Y + 75) + 10  + Base_Font_H;
            msg = "공";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = t_f.X + 2;
            tt.Y = (t_f.Y + 75) + 25 + Base_Font_H;
            msg = "급";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = t_f.X + 2;
            tt.Y = (t_f.Y + 75) + 40 + Base_Font_H;
            msg = "자";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);




            //공급받는자 앞선
            X1 = (pageW / 2) - 10;
            X2 = X1;
            Y1 = t_f.Y + 75;
            Y2 = BaseitemH2;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            //공급받는자 뒷선
            X1 = (pageW / 2) + 10;
            X2 = X1;
            Y1 = t_f.Y + 75;
            Y2 = BaseitemH2;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = (pageW / 2) - 10 + 2;
            tt.Y = (t_f.Y + 75) +  Base_Font_H;
            msg = "공";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = (pageW / 2) - 10 + 2;
            tt.Y = (t_f.Y + 75) + 15 + Base_Font_H;
            msg = "급";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = (pageW / 2) - 10 + 2;
            tt.Y = (t_f.Y + 75) + 30 + Base_Font_H;
            msg = "받";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = (pageW / 2) - 10 + 2;
            tt.Y = (t_f.Y + 75) + 45 + Base_Font_H;
            msg = "는";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = (pageW / 2) - 10 + 2;
            tt.Y = (t_f.Y + 75) + 60 + Base_Font_H;
            msg = "자";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);




        }



        private void BaseDoc_PrintPage____004(System.Drawing.Printing.PrintPageEventArgs e, Rectangle t_f, int BaseitemH2, int BaseitemH3, int Y_tGap)
        {
            RectangleF tt = new RectangleF();

            string msg = "";
            Pen T_p = new Pen(Color.Black);
            int pageW = e.PageBounds.Width - 20, pageH = e.PageBounds.Height;
            int X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;
            
            
            int Base_Line = 20, Base_Font_H = 10; //,  BaseitemH = t_f.Y + 75;;


            int Base_W = t_f.X + 20;
            int BaseitemH = t_f.Y + 75;
            //등록번호 뒷선
            X1 = Base_W + 35;
            X2 = X1;
            Y1 = t_f.Y + 75;
            Y2 = BaseitemH2;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = Base_W + 2;
            tt.Y = BaseitemH + 2;
            msg = "등록";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = Base_W + 2;
            tt.Y = BaseitemH + 7 + Base_Font_H;
            msg = "번호";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 36;
            tt.Y = Y1 = BaseitemH +  Base_Font_H;
            msg = cls_app_static_var.Dir_Company_Number ;  //등록번호
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


                        

            tt.X = Base_W + 2;
            tt.Y = Y1 = BaseitemH + (30 * 1) + Base_Font_H;
            msg = "상호";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 36;
            tt.Y = Y1 = BaseitemH + (30 * 1) + Base_Font_H;
            msg = cls_app_static_var.Dir_Company_Name ;  //상호
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


            tt.X = Base_W + 2;
            tt.Y = Y1 = BaseitemH + (30 * 2) + Base_Font_H;
            msg = "주소";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = Base_W + 36;
            tt.Y = Y1 = BaseitemH + (30 * 2) + Base_Font_H;
            msg = cls_app_static_var.Dir_Company_Address ;  //주소
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 7), Brushes.Black, tt);



            int Base_W2 = Base_W + 200;


            //대표전화 관련 라인
            X1 = Base_W2 + 35;
            X2 = X1;
            Y1 = BaseitemH;
            Y2 = BaseitemH2 - 60;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            X1 = Base_W2 + 3;
            X2 = X1;
            Y1 = BaseitemH;
            Y2 = BaseitemH2 - 60;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = Base_W2 + 2;
            tt.Y = Y1 = BaseitemH + 2 ;
            msg = "대표";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

                       


            tt.X = Base_W2 + 2;
            tt.Y = Y1 = BaseitemH + 7 + Base_Font_H;
            msg = "전화";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W2 + 35;
            tt.Y = Y1 = BaseitemH +  Base_Font_H;
            msg = cls_app_static_var.Dir_Company_P_Number;  //전화번호
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);



            int Base_W3 = Base_W + 230;


            //회사전화 관련 라인
            X1 = Base_W3 + 35;
            X2 = X1;
            Y1 = BaseitemH2 - 60;
            Y2 = BaseitemH2 - 30;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            X1 = Base_W3 + 3;
            X2 = X1;
            Y1 = BaseitemH2 - 60;
            Y2 = BaseitemH2 - 30;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = Base_W3 + 2;
            tt.Y = Y1 = BaseitemH + (30 * 1) + Base_Font_H;
            msg = "성명";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W3 + 35;
            tt.Y = Y1 = BaseitemH + (30 * 1) + Base_Font_H;
            msg = cls_app_static_var.Dir_Company_Bos_Name;  //대표자명
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);
            
        }



        private void BaseDoc_PrintPage____005(System.Drawing.Printing.PrintPageEventArgs e, Rectangle t_f, int BaseitemH2, int BaseitemH3, int Y_tGap)
        {
            RectangleF tt = new RectangleF();

            string msg = "";
            Pen T_p = new Pen(Color.Black);
            int pageW = e.PageBounds.Width - 20, pageH = e.PageBounds.Height;
            int X1 = 0, X2 = 0, Y1 = 0, Y2 = 0;


            int Base_Line = 20, Base_Font_H = 10; //,  BaseitemH = t_f.Y + 75;;

            string T_El_Rec = "", BeT_Add = "", T_Add = "";

            int fi_cnt = 0; 
            foreach (int t_key in Sales_Rece.Keys)
            {
                if (Sales_Rece[t_key].Del_TF != "D")
                {
                    if (BeT_Add == "")
                    {
                        BeT_Add = Sales_Rece[t_key].Receive_Method_Name  ; 
                        if (Sales_Rece[t_key].Receive_Method == 2)
                        {
                            BeT_Add = BeT_Add + "  " + Sales_Rece[t_key].Get_ZipCode ;
                            BeT_Add = BeT_Add + "  " + Sales_Rece[t_key].Get_Address1  ; 
                            BeT_Add = BeT_Add + "  " + Sales_Rece[t_key].Get_Address2  ; 
                        }
                    }
                    else
                    {
                        T_Add = Sales_Rece[t_key].Receive_Method_Name  ; 
                        if (Sales_Rece[t_key].Receive_Method == 2)
                        {
                            T_Add = T_Add + "  " + Sales_Rece[t_key].Get_ZipCode ;
                            T_Add = T_Add + "  " + Sales_Rece[t_key].Get_Address1  ; 
                            T_Add = T_Add + "  " + Sales_Rece[t_key].Get_Address2  ; 
                        }
                    }
             
                    if ((BeT_Add != T_Add) && (T_Add != "") && (BeT_Add != "") )
                        BeT_Add = "다중 배송" ;
                                
                    if (Sales_Rece[t_key].Receive_Method == 2)
                    {

                        if (Sales_Rece[t_key].Get_Tel1 != "")
                            T_El_Rec = Sales_Rece[t_key].Get_Tel1;                        
                    }
                }                    
                fi_cnt++;
            }

            string T_El = "";

            string  StrSql = "Select hptel,hometel,Address1,Address2 From tbl_Memberinfo  (nolock) " ;
            StrSql = StrSql + " Where Mbid  ='" + SalesDetail[txt_OrderNumber.Text.Trim()].Mbid + "'"; 
            StrSql = StrSql + " And   Mbid2 =" +  SalesDetail[txt_OrderNumber.Text.Trim()].Mbid2 ;

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            T_El = encrypter.Decrypt ( ds.Tables[base_db_name].Rows[0]["Hometel"].ToString ()) ;

            if (encrypter.Decrypt (ds.Tables[base_db_name].Rows[0]["hptel"].ToString ()) != "") 
                T_El = encrypter.Decrypt (ds.Tables[base_db_name].Rows[0]["hptel"].ToString ()) ;

            if (BeT_Add == "")
                BeT_Add = encrypter.Decrypt (ds.Tables[base_db_name].Rows[0]["address1"].ToString()) + " " + encrypter.Decrypt (ds.Tables[base_db_name].Rows[0]["address2"].ToString());
            


            int Base_W = (pageW / 2) + 10;
            int BaseitemH = t_f.Y + 75;
            //등록번호 뒷선
            X1 = Base_W + 35;
            X2 = X1;
            Y1 = BaseitemH;
            Y2 = BaseitemH2;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);


            tt.X = Base_W + 2;
            tt.Y = BaseitemH + 2;
            msg = "주문";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = Base_W + 2;
            tt.Y = BaseitemH + 7 + Base_Font_H;
            msg = "번호";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 35;
            tt.Y =  BaseitemH + Base_Font_H;

            if (txt_OrderNumber.Text.Trim() == "")
                msg = txt_OrderNumber.Text.Trim();   //주문번호
            else
                msg = InsuranceNumber_Ord_Print_FLAG;  //주문번호
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);





            tt.X = Base_W + 2;
            tt.Y =  BaseitemH + (30 * 1) + 2;
            msg = "회원";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);

            tt.X = Base_W + 2;
            tt.Y =  BaseitemH + (30 * 1) + 7 + Base_Font_H;
            msg = "번호";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 35;
            tt.Y = Y1 = BaseitemH + (30 * 1)  + Base_Font_H;
            msg = mtxtMbid.Text.Trim();   //회원번호
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);


            tt.X = Base_W + 2;
            tt.Y =  BaseitemH + (30 * 2) + Base_Font_H;
            msg = "주소";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);
            
            tt.X = Base_W + 35;
            tt.Y = BaseitemH + (30 * 2) + Base_Font_H;
            msg = BeT_Add;   //주소
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 7), Brushes.Black, tt);


            Base_W = Base_W + 200;

            //연락처 성명 관련 라인
            X1 = Base_W + 45;
            X2 = X1;
            Y1 = BaseitemH;
            Y2 = BaseitemH2 - 30;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);

            X1 = Base_W + 3 ;
            X2 = X1;
            Y1 = BaseitemH;
            Y2 = BaseitemH2 - 30 ;
            e.Graphics.DrawLine(T_p, X1, Y1, X2, Y2);
            

            tt.X = Base_W + 2;
            tt.Y = Y1 = BaseitemH +  Base_Font_H;
            msg = "연락처";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);



            tt.X = Base_W + 45;
            tt.Y = Y1 = BaseitemH + Base_Font_H;
            msg = T_El;   //연락처
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);






            tt.X = Base_W + 2;
            tt.Y = Y1 = BaseitemH + (30 * 1) + Base_Font_H;
            msg = "성명";
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 10), Brushes.Black, tt);


            tt.X = Base_W + 45;
            tt.Y =  BaseitemH + (30 * 1) + Base_Font_H;
            msg = txtName.Text.Trim();   //성명
            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);

        }

























    }
}
