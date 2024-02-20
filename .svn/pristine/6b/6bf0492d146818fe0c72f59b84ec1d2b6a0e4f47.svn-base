using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Security.Cryptography;
using DevExpress.XtraEditors.Repository;
//20190225구현호 - 클래스종속을 위해 MLM_Program뒤에 폼이름을 삭제


using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace MLM_Program
{


    public partial class frmSell_Cancel_dev : Form
    {

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
        public delegate void Take_NumberDele(ref string Send_Number, ref string Send_Name, ref string Send_OrderNumber);
        public event Take_NumberDele Take_Mem_Number;

        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Item = new cls_Grid_Base();
        cls_Grid_Base cgb_Cacu = new cls_Grid_Base();
        cls_Grid_Base cgb_Rece = new cls_Grid_Base();

        private Dictionary<string, cls_Sell> SalesDetail;
        private Dictionary<int, cls_Sell_Item> SalesItemDetail = new Dictionary<int, cls_Sell_Item>();
        private Dictionary<int, cls_Sell_Rece> Sales_Rece = new Dictionary<int, cls_Sell_Rece>();
        private Dictionary<int, cls_Sell_Cacu> Sales_Cacu = new Dictionary<int, cls_Sell_Cacu>();

        private Dictionary<string, TextBox> Ncode_dic = new Dictionary<string, TextBox>();

        private const string base_db_name = "tbl_SalesDetail";
        private int Data_Set_Form_TF;
        private string idx_Mbid = "";
        private int idx_Mbid2 = 0;

        public frmSell_Cancel_dev()
        {
            InitializeComponent();
        }
        private void InitCombo()
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            // ** txtCenter 룩업에디트 세팅
            {
                Tsql = "Select  ncode + ' ' + name as 센터이름";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + combo_Se_Code.Text.Trim() + "') )";
                if (combo_Se_Code.Text.Trim() != "") Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                Tsql = Tsql + " And ncode <> '002'"; // 2018-11-23 지성경 에스제이로직스는 선택불가능하게끔한다.
            }
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            txtCenter.Properties.DataSource = ds.Tables["t_P_table"].Copy();
            txtCenter.Properties.ValueMember = "센터이름";
            txtCenter.Properties.DisplayMember = "센터이름";


            mtxtMbid.Mask = "9999999999";

        }
        private void dGridView_Base_Header_Reset()
        {
            cgb.Grid_Base_Arr_Clear();
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_col_Count = 16;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"승인여부" , "공제번호"  , "주문번호"   , "주문일자"  , "총주문액"
                                    , "총입금액"      , "총PV"   , "총CV"    , "주문종류"   , "구분"
                                    , "현금"     ,"카드" ,"무통장", "미결제"  ,  "기록자"
                                    ,  "기록일"
                                };




            if (cls_app_static_var.Sell_Union_Flag == "")  //직판특판이 아닌경우 공제번호 필드 안나오게
            {
                int[] g_Width = { 80,0, 120, 80, 80
                              , 80  ,80 , 80 , 80 , 80
                              , 80, 80  ,80 ,80,80
                              ,80
                            };
                cgb.grid_col_w = g_Width;
            }
            else
            {

                int[] g_Width = { 80,120, 120, 80, 80
                              , 80  ,80 , 80 , 80 , 80
                              , 80 , 80  ,80 ,80,80 ,80
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
                                ,DataGridViewContentAlignment.MiddleCenter

                                ,DataGridViewContentAlignment.MiddleRight  //10
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

            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb.grid_col_header_text = g_HeaderText;
            cgb.grid_cell_format = gr_dic_cell_format;

            cgb.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true, true,  true
                                    ,  true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            cgb.basegrid.RowHeadersVisible = false;
        }

        private void dGridView_Base_Item_Header_Reset()
        {
                cgb_Item.Grid_Base_Arr_Clear();
                cgb_Item.basegrid = dGridView_Base_Item;
                cgb_Item.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
                cgb_Item.grid_col_Count = 12;
                cgb_Item.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                string[] g_HeaderText = {""  , "상품_코드"   , "상품명"  , "개별단가"   , "개별PV",
                    "개별CV"  , "주문_수량"   , "총상품액"    , "총상품PV"  , "총상품CV",
                "구분" , "비고"
                                };

                int[] g_Width = { 0, 90, 160, 80, 70
                    , 70 ,80 , 80 , 80  , 70,
                70 , 200
                            };

                DataGridViewContentAlignment[] g_Alignment =
                                    {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                  ,DataGridViewContentAlignment.MiddleRight  //6   
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
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb_Item.grid_col_header_text = g_HeaderText;
                cgb_Item.grid_cell_format = gr_dic_cell_format;
                cgb_Item.grid_col_w = g_Width;
                cgb_Item.grid_col_alignment = g_Alignment;


                Boolean[] g_ReadOnly = { true , true,  true,  true ,true,true,true
                                    ,true , true,  true,  true ,true
                                   };
                cgb_Item.grid_col_Lock = g_ReadOnly;

                cgb_Item.basegrid.RowHeadersVisible = false;
        }

        private void dGridView_Base_Cacu_Header_Reset()
        {
            cgb_Cacu.Grid_Base_Arr_Clear();
            cgb_Cacu.basegrid = dGridView_Base_Cacu;
            cgb_Cacu.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Cacu.grid_col_Count = 10;
            cgb_Cacu.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {""  , "결제방법"   , "결제액"  , "결제일자"   , "카드_은행명"
                                , "카드_은행번호"   , "카드소유자"    , "입금자"  , "비고" , ""
                                };

            int[] g_Width = { 0, 90, 70, 90, 100
                                ,120 , 100 , 90 , 150 , 0
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

            int[] g_Width = { 0, 90, 70, 90, 100
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
        private void txtName_Enter(object sender, EventArgs e)
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
      

        //변경할려는 대상자에 대한 회원번호에서 회원 검색창을 뛰엇을 경우에
        void e_f_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            mtxtMbid.Text = Send_Number; txtName.Text = Send_Name;
            if (Input_Error_Check(mtxtMbid, "m") == true)
                Set_Form_Date(mtxtMbid.Text, "m");
        }
        void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtName.Text.Trim();
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

            //dGridView_Base_Rece_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Rece.d_Grid_view_Header_Reset();

         
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<



            txtName.ReadOnly = false;
            txtName.BackColor = SystemColors.Window;

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtMbid);

            Base_Ord_Clear();

            // mtxtSn.Mask = "999999-9999999";
            idx_Mbid = ""; idx_Mbid2 = 0;
            mtxtMbid.Focus();
        }


        private void Base_Ord_Clear()
        {
            dGridView_Base_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Item.d_Grid_view_Header_Reset();

            dGridView_Base_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Cacu.d_Grid_view_Header_Reset();

            dGridView_Base_Rece_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Rece.d_Grid_view_Header_Reset();



            if (SalesItemDetail != null)
                SalesItemDetail.Clear();
            if (Sales_Rece != null)
                Sales_Rece.Clear();
            if (Sales_Cacu != null)
                Sales_Cacu.Clear();

            Base_Sub_Clear("item");
            Base_Sub_Clear("Rece");
            Base_Sub_Clear("Cacu");


            cls_form_Meth ct = new cls_form_Meth();
            //  ct.from_control_clear(panel7, txtSellDate);
            //ct.from_control_clear(panel2, txtSellDateRe);            
        }
        private void Base_Sub_Clear(string s_Tf)
        {
            cls_form_Meth ct = new cls_form_Meth();

            if (s_Tf == "item")
            {

                if (SalesItemDetail != null)
                    Item_Grid_Set(); //상품 그리드
            }

            if (s_Tf == "Rece")
            {
                if (Sales_Rece != null)
                    Rece_Grid_Set(); //배송 그리드
            }


            if (s_Tf == "Cacu")
            {
                dGridView_Base_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_Cacu.d_Grid_view_Header_Reset();

                if (Sales_Cacu != null)
                    Cacu_Grid_Set(); //배송 그리드
            }


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
                                ,Sales_Cacu[t_key].C_TF
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
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
                                ,SalesItemDetail[t_key].ItemTotalCV
                                ,SalesItemDetail[t_key].SellStateName
                                ,SalesItemDetail[t_key].Etc
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }
        private void Set_Form_Date(DataSet ds)
        {
            idx_Mbid = ds.Tables[base_db_name].Rows[0]["Mbid"].ToString();
            idx_Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[0]["Mbid2"].ToString());

            mtxtMbid.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();
            txtName.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();
            mtxtSn.Text = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Cpno"].ToString(), "Cpno");

            txtCenter.Text = ds.Tables[base_db_name].Rows[0]["B_Name"].ToString();
            txtCenter_code.Text = ds.Tables[base_db_name].Rows[0]["businesscode"].ToString();

            //20190306 구현호 선택된 룩업에디트의 텍스트를 잘라서 가져온다
         
            string businesscode = ds.Tables[base_db_name].Rows[0]["businesscode"].ToString();
            string business = ds.Tables[base_db_name].Rows[0]["B_name"].ToString();
            string businesscomplite = businesscode + " " + business;

            txtCenter.EditValue = businesscomplite;





            txtName.ReadOnly = true;
            // txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.BackColor = cls_app_static_var.txt_Enable_Color;
        }
        private void Set_Form_Date(string T_Mbid, string T_sort)
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
                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) AS M_Mbid ";
                else
                    Tsql = Tsql + " tbl_Memberinfo.mbid2 AS M_Mbid ";

                Tsql = Tsql + " ,tbl_Memberinfo.mbid ";
                Tsql = Tsql + " ,tbl_Memberinfo.mbid2 ";
                Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

                Tsql = Tsql + ",  tbl_Memberinfo.Cpno ";

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

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                }

                ////Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
                Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
                Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
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

        private void Base_Grid_Set()
        {
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            int fi_cnt = 0;
            double S_cnt4 = 0; double S_cnt5 = 0; double S_cnt6 = 0, S_cnt6_2 = 0;
            double Sum_13 = 0; double Sum_14 = 0; double Sum_15 = 0; ; double Sum_16 = 0;
            foreach (string t_key in SalesDetail.Keys)
            {
                if (SalesDetail[t_key].Del_TF != "D")
                {
                    Set_gr_dic(ref gr_dic_text, t_key, fi_cnt);  //데이타를 배열에 넣는다.

                    S_cnt4 = S_cnt4 + SalesDetail[t_key].TotalPrice;
                    S_cnt5 = S_cnt5 + SalesDetail[t_key].TotalInputPrice;
                    S_cnt6 = S_cnt6 + SalesDetail[t_key].TotalPV;
                    S_cnt6_2 = S_cnt6_2 + SalesDetail[t_key].TotalCV;

                    Sum_13 = Sum_13 + SalesDetail[t_key].InputCash;
                    Sum_14 = Sum_14 + SalesDetail[t_key].InputCard;
                    Sum_15 = Sum_15 + SalesDetail[t_key].InputPassbook;
                    Sum_16 = Sum_16 + SalesDetail[t_key].UnaccMoney;
                }

                fi_cnt++;
            }

            cls_form_Meth cm = new cls_form_Meth();

            object[] row0 = { ""
                                ,"<< " + cm._chang_base_caption_search("합계") + " >>"
                                ,""
                                ,""
                                ,S_cnt4

                                ,S_cnt5
                                ,S_cnt6
                                ,S_cnt6_2
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
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            // // Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
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
        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string txt_tag = txtName.Text;
                if (txt_tag != "")
                {
                    int reCnt = 0;
                    cls_Search_DB cds = new cls_Search_DB();
                    string Search_Mbid = "";
                    reCnt = cds.Member_Name_Search(ref Search_Mbid, txt_tag);

                    if (reCnt == 1)
                    {

                        mtxtMbid.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
                        if (Input_Error_Check(mtxtMbid, "n") == true)
                            Set_Form_Date(mtxtMbid.Text, "n");

                    }
                    else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
                    {

                        //cls_app_static_var.Search_Member_Name = txt_tag;
                        frmBase_Member_Search e_f = new frmBase_Member_Search();
                        //if (tb.Name == "txtName_s")
                        //{
                        //    e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                        //    e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);
                        //}

                        //if (tb.Name == "txtName_n")
                        //{
                        e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_txtname);
                        e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                        //}

                        e_f.ShowDialog();

                        SendKeys.Send("{TAB}");
                    }


                }
                else
                    SendKeys.Send("{TAB}");
            }
        }
        void e_f_Send_MemName_txtname(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = txtName.Text.Trim(); ;
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
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
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
                    Tsql = Tsql + " Where M_Detail_S = 'tbl_Sales_Rece' ";
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
                Tsql = "Select M_Detail , " + cls_app_static_var.Base_M_Detail_Ex;
                Tsql = Tsql + " From tbl_Base_Change_Detail (nolock) ";
                Tsql = Tsql + " Where M_Detail_S = 'tbl_Sales_Rece' ";
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

        private void Set_SalesDetail()
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


            strSql = strSql + " , Ga_Order  SellTF ";
            strSql = strSql + " ,Case When Ga_Order >= 1 Then '" + cm._chang_base_caption_search("미승인") + "'";
            strSql = strSql + "  When Ga_Order = 0 Then '" + cm._chang_base_caption_search("승인") + "'";
            strSql = strSql + " ELSE '' ";
            strSql = strSql + " END SellTFName ";

            if (cls_app_static_var.Sell_Union_Flag == "U")  //특판
            {
                strSql = strSql + " , Case When  tbl_SalesDetail.union_Seq > 0 And InsuranceNumber <> '' Then InsuranceNumber ";
                strSql = strSql + "        When  tbl_SalesDetail.union_Seq > 0 And T_REALMLM.ERRCODE <> '0000' And InsuranceNumber = '' Then  ISNULL(T_REALMLM_ErrCode.Er_Msg ,'' ) ";
                strSql = strSql + "        When  tbl_SalesDetail.union_Seq = 0 Then '미신고'  ";
                strSql = strSql + "   End  InsuranceNumber2 ";
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
                strSql = strSql + " , InsuranceNumber As InsuranceNumber2 ";
            }

            strSql = strSql + " , tbl_SalesDetail.Union_Seq ";

            strSql = strSql + " ,InsuranceNumber AS InsuranceNumber_Real ";

            strSql = strSql + " From tbl_SalesDetail (nolock) ";
            //strSql = strSql + " LEFT JOIN tbl_SalesDetail_TF (nolock) ON tbl_SalesDetail.OrderNumber = tbl_SalesDetail_TF.OrderNumber ";
            strSql = strSql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";
            strSql = strSql + " LEFT Join tbl_SellType ON tbl_SalesDetail.SellCode = tbl_SellType.SellCode ";
            strSql = strSql + " LEFT JOIN tbl_Business (nolock) ON tbl_SalesDetail.BusCode = tbl_Business.NCode  And tbl_SalesDetail.Na_code = tbl_Business.Na_code ";

            strSql = strSql + " LEFT JOIN T_REALMLM (nolock) ON T_REALMLM.SEQ = tbl_SalesDetail.union_Seq ";
            strSql = strSql + " LEFT JOIN T_REALMLM_ErrCode (nolock) ON T_REALMLM.ERRCODE = T_REALMLM_ErrCode.Er_Code ";

            if (idx_Mbid.Length == 0)
                strSql = strSql + " Where tbl_Memberinfo.Mbid2 = " + idx_Mbid2.ToString();
            else
            {
                strSql = strSql + " Where tbl_Memberinfo.Mbid = '" + idx_Mbid + "' ";
                strSql = strSql + " And   tbl_Memberinfo.Mbid2 = " + idx_Mbid2.ToString();
            }

            //if (cls_User.gid != "admin")
            //    strSql = strSql + " And  tbl_SalesDetail.W_T_TF = 0  ";

            // strSql = strSql + " And (Ga_Order = 0 ) "; //정상내역은 승인 내역만 보여준다.
            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";
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
                t_c_sell.Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                t_c_sell.M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();
                t_c_sell.SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();
                t_c_sell.SellCode = ds.Tables[base_db_name].Rows[fi_cnt]["SellCode"].ToString();
                t_c_sell.SellCodeName = ds.Tables[base_db_name].Rows[fi_cnt]["SellCodeName"].ToString();
                t_c_sell.BusCode = ds.Tables[base_db_name].Rows[fi_cnt]["BusCode"].ToString();
                t_c_sell.BusCodeName = ds.Tables[base_db_name].Rows[fi_cnt]["BusCodeName"].ToString();
                t_c_sell.Re_BaseOrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["Re_BaseOrderNumber"].ToString();
                t_c_sell.TotalPrice = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
                t_c_sell.TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
                t_c_sell.TotalCV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalCV"].ToString());
                t_c_sell.TotalInputPrice = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalInputPrice"].ToString());
                t_c_sell.Total_Sell_VAT_Price = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Total_Sell_VAT_Price"].ToString());
                t_c_sell.Total_Sell_Except_VAT_Price = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Total_Sell_Except_VAT_Price"].ToString());
                t_c_sell.InputCash = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCash"].ToString());
                t_c_sell.InputCard = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCard"].ToString());
                t_c_sell.InputPassbook = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputPassbook"].ToString());
                t_c_sell.InputMile = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputMile"].ToString());
                t_c_sell.InputPass_Pay = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputPass_Pay"].ToString());
                t_c_sell.UnaccMoney = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["UnaccMoney"].ToString());

                t_c_sell.Etc1 = ds.Tables[base_db_name].Rows[fi_cnt]["Etc1"].ToString();
                t_c_sell.Etc2 = ds.Tables[base_db_name].Rows[fi_cnt]["Etc2"].ToString();

                t_c_sell.ReturnTF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ReturnTF"].ToString());
                t_c_sell.ReturnTFName = ds.Tables[base_db_name].Rows[fi_cnt]["ReturnTFName"].ToString();
                //t_c_sell.INS_Num = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber"].ToString();
                t_c_sell.INS_Num = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber2"].ToString();
                t_c_sell.INS_Num_Real = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber_Real"].ToString();

                t_c_sell.InsuranceNumber_Date = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber_Date"].ToString();
                t_c_sell.W_T_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["W_T_TF"].ToString());
                t_c_sell.In_Cnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["In_Cnt"].ToString());

                t_c_sell.RecordID = ds.Tables[base_db_name].Rows[fi_cnt]["RecordID"].ToString();
                t_c_sell.RecordTime = ds.Tables[base_db_name].Rows[fi_cnt]["RecordTime"].ToString();

                t_c_sell.SellTF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SellTF"].ToString());
                t_c_sell.SellTFName = ds.Tables[base_db_name].Rows[fi_cnt]["SellTFName"].ToString();
                t_c_sell.Union_Seq = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Union_Seq"].ToString());


                string t_sellDate = t_c_sell.SellDate.Substring(0, 4);
                t_sellDate = t_sellDate + "-" + t_c_sell.SellDate.Substring(4, 2);
                t_sellDate = t_sellDate + "-" + t_c_sell.SellDate.Substring(6, 2);
                t_c_sell.SellDate = t_sellDate;

                t_c_sell.Del_TF = "";

                T_SalesDetail[t_c_sell.OrderNumber] = t_c_sell;
            }


            SalesDetail = T_SalesDetail;

        }


        //    cls_Connect_DB Temp_Connect = new cls_Connect_DB();

        //    DataSet ds = new DataSet();
        //    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
        //    if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
        //    int ReCnt = Temp_Connect.DataSet_ReCount;

        //    if (ReCnt == 0) return;

        //    cgb.FillGrid(ds.Tables[0]);



        //    Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

        //    for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
        //    {
        //        Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
        //    }
        //    cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
        //    cgb.db_grid_Obj_Data_Put();

        //    foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView1.Columns)
        //    {
        //        //if (new List<string>()
        //        //{
        //        //    "OrderNumber"  ,"InsuranceNumber", "mbid2"   , "mname",  "cpno","nmbid2", "nmname"
        //        //}.Contains(col.Name))
        //        //{
        //        //    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
        //        //}
        //        //else
        //        col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;





        //    }
        //    ////++++++++++++++++++++++++++++++++
        //    //cls_Connect_DB Temp_Connect = new cls_Connect_DB();

        //    //DataSet ds = new DataSet();
        //    ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
        //    //if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
        //    //int ReCnt = Temp_Connect.DataSet_ReCount;

        //    //if (ReCnt == 0) return;


        //    //cgb.FillGrid(ds.Tables[0]);



        //    //Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

        //    //for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
        //    //{
        //    //    Set_gr_dic(ref gr_dic_text, ref ds, fi_cnt);  //데이타를 배열에 넣는다.
        //    //}
        //    //cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
        //    //cgb.db_grid_Obj_Data_Put();

        //    //foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView1.Columns)
        //    //{
        //    //    //if (new List<string>()
        //    //    //{
        //    //    //    "OrderNumber"  ,"InsuranceNumber", "mbid2"   , "mname",  "cpno","nmbid2", "nmname"
        //    //    //}.Contains(col.Name))
        //    //    //{
        //    //    //    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
        //    //    //}
        //    //    //else
        //    //    col.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;

        //    //}

        //    //++++++++++++++++++++++++++++++++

        //    Dictionary<string, cls_Sell> T_SalesDetail = new Dictionary<string, cls_Sell>();

        //    for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
        //    {
        //        cls_Sell t_c_sell = new cls_Sell();

        //        t_c_sell.OrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
        //        t_c_sell.Mbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
        //        t_c_sell.Mbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
        //        t_c_sell.M_Name = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString();
        //        t_c_sell.SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["SellDate"].ToString();
        //        t_c_sell.SellCode = ds.Tables[base_db_name].Rows[fi_cnt]["SellCode"].ToString();
        //        t_c_sell.SellCodeName = ds.Tables[base_db_name].Rows[fi_cnt]["SellCodeName"].ToString();
        //        t_c_sell.BusCode = ds.Tables[base_db_name].Rows[fi_cnt]["BusCode"].ToString();
        //        t_c_sell.BusCodeName = ds.Tables[base_db_name].Rows[fi_cnt]["BusCodeName"].ToString();
        //        t_c_sell.Re_BaseOrderNumber = ds.Tables[base_db_name].Rows[fi_cnt]["Re_BaseOrderNumber"].ToString();
        //        t_c_sell.TotalPrice = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPrice"].ToString());
        //        t_c_sell.TotalPV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalPV"].ToString());
        //        t_c_sell.TotalCV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalCV"].ToString());
        //        t_c_sell.TotalInputPrice = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TotalInputPrice"].ToString());
        //        t_c_sell.Total_Sell_VAT_Price = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Total_Sell_VAT_Price"].ToString());
        //        t_c_sell.Total_Sell_Except_VAT_Price = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Total_Sell_Except_VAT_Price"].ToString());
        //        t_c_sell.InputCash = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCash"].ToString());
        //        t_c_sell.InputCard = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputCard"].ToString());
        //        t_c_sell.InputPassbook = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputPassbook"].ToString());
        //        t_c_sell.InputMile = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputMile"].ToString());
        //        t_c_sell.InputPass_Pay = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InputPass_Pay"].ToString());
        //        t_c_sell.UnaccMoney = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["UnaccMoney"].ToString());

        //        t_c_sell.Etc1 = ds.Tables[base_db_name].Rows[fi_cnt]["Etc1"].ToString();
        //        t_c_sell.Etc2 = ds.Tables[base_db_name].Rows[fi_cnt]["Etc2"].ToString();

        //        t_c_sell.ReturnTF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ReturnTF"].ToString());
        //        t_c_sell.ReturnTFName = ds.Tables[base_db_name].Rows[fi_cnt]["ReturnTFName"].ToString();
        //        //t_c_sell.INS_Num = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber"].ToString();
        //        t_c_sell.INS_Num = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber2"].ToString();
        //        t_c_sell.INS_Num_Real = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber_Real"].ToString();

        //        t_c_sell.InsuranceNumber_Date = ds.Tables[base_db_name].Rows[fi_cnt]["InsuranceNumber_Date"].ToString();
        //        t_c_sell.W_T_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["W_T_TF"].ToString());
        //        t_c_sell.In_Cnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["In_Cnt"].ToString());

        //        t_c_sell.RecordID = ds.Tables[base_db_name].Rows[fi_cnt]["RecordID"].ToString();
        //        t_c_sell.RecordTime = ds.Tables[base_db_name].Rows[fi_cnt]["RecordTime"].ToString();

        //        t_c_sell.SellTF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SellTF"].ToString());
        //        t_c_sell.SellTFName = ds.Tables[base_db_name].Rows[fi_cnt]["SellTFName"].ToString();
        //        t_c_sell.Union_Seq = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Union_Seq"].ToString());


        //        string t_sellDate = t_c_sell.SellDate.Substring(0, 4);
        //        t_sellDate = t_sellDate + "-" + t_c_sell.SellDate.Substring(4, 2);
        //        t_sellDate = t_sellDate + "-" + t_c_sell.SellDate.Substring(6, 2);
        //        t_c_sell.SellDate = t_sellDate;

        //        t_c_sell.Del_TF = "";

        //        T_SalesDetail[t_c_sell.OrderNumber] = t_c_sell;
        //    }


        //    SalesDetail = T_SalesDetail;
        //}


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
                                ,SalesDetail[t_key].UnaccMoney
                                ,SalesDetail[t_key].RecordID

                                ,SalesDetail[t_key].RecordTime
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }
        private void Base_Grid_info_Set(int intTemp)
        {
            //20190227 구현호 각종 그리드의 셀렉트문들 내용을 inttemp값대로 출력한다. 예 - 매출은 5
            string T_Mbid = "";
            T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            cls_Search_DB csb = new cls_Search_DB();
            cls_form_Meth cm = new cls_form_Meth();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) != 1) return;

            string Tsql = "";

            Tsql = "Select  ";
            if (intTemp == 1)
            {


            }
        }

        private void butt_Save_Click(object sender, EventArgs e)
        {
          
                int Save_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Cancel_Base_Data(ref Save_Error_Check);

                if (Save_Error_Check > 0)
                {
                    //---------------------------------------------------------
                    //cls_Exigo cexig = new cls_Exigo();
                    //cexig.ChangeOrderStatus_Cancel(txt_OrderNumber.Text); //미국에 보낸다 엑시고를 통해서                    
                    //---------------------------------------------------------

                    Base_Ord_Clear();

                    if (SalesDetail != null)
                        SalesDetail.Clear();

                    Set_SalesDetail();  //회원의 주문 관련 주테이블 내역을 클래스에 넣는다.

                    if (SalesDetail != null)
                        Base_Grid_Set();
                }


                this.Cursor = System.Windows.Forms.Cursors.Default;
          

        }
        private void Delete_Base_Data(ref int Delete_Error_Check)
        {
            Delete_Error_Check = 0;

            ////주문종류 , 회원, 주문일자 입력 안햇는지 체크
            //if (Check_Delete_TextBox_Error() == false) return;
            //if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;  



            //cls_Cash_Card_Admin_Cancel cccA = new cls_Cash_Card_Admin_Cancel();

            //int ret_C1 = cccA.Cash_Card_Send_Singo_Cancel(txt_OrderNumber.Text, mtxtMbid.Text.Trim(), "Cash");
            //if (ret_C1 == 1)
            //{
            //    MessageBox.Show("현금 영수증 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
            //    return;
            //}

            //if (ret_C1 == 100)
            //{
            //    MessageBox.Show("현금 영수증 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다.");
            //    return;
            //}



            //ret_C1 = 0;
            //ret_C1 = cccA.Cash_Card_Send_Singo_Cancel(txt_OrderNumber.Text, mtxtMbid.Text.Trim(), "Card", 0, "C");

            //if (ret_C1 >= 1)
            //{
            //    MessageBox.Show("카드 승인 내역 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
            //    return;
            //}


            //if (ret_C1 == 100)
            //{
            //    MessageBox.Show("카드 승인 내역 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다.");
            //    return;
            //}

            //if (ret_C1 == -100)
            //{
            //    MessageBox.Show("현 프로그램에서 승인 취소할수 없는 카드 내역 입니다. ksnet 웹상에서 취소 처리 하십시요. 매출은 취소 처리 됩니다..");
            //    //return;
            //}


            //ret_C1 = 0;
            //ret_C1 = cccA.Cash_Card_Send_Singo_Cancel(txt_OrderNumber.Text, mtxtMbid.Text.Trim(), "Bank");

            //if (ret_C1 == 1)
            //{
            //    MessageBox.Show("가상계좌 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
            //    return;
            //}

            //if (ret_C1 == 100)
            //{
            //    string m_Sg = "가상계좌 취소중에 문제가 발생했습니다. 매출 취소를 계속 진행하시겠습니까?";
            //    m_Sg = m_Sg + "\n";
            //    m_Sg = m_Sg + "계속 진행시 가상계좌는 메뉴얼로 직접 취소하셔야 합니다.";

            //    if (MessageBox.Show(m_Sg, "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            //}

            ////cls_Cash_Card_Admin_Cancel cccA = new cls_Cash_Card_Admin_Cancel();

            //////int ret_C1 = cccA.Cash_Card_Send_Singo_Cancel(txt_OrderNumber.Text, mtxtMbid.Text.Trim(), "Cash");
            //////if (ret_C1 == 1)
            //////{
            //////    MessageBox.Show("현금 영수증 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
            //////    return;
            //////}

            //////if (ret_C1 == 100)
            //////{
            //////    MessageBox.Show("현금 영수증 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다.");
            //////    return;
            //////}



            //////카드 승인 취소 막아버림 홍진기 과장 요청에 의해서 2017-03-16
            ////////ret_C1 = cccA.Cash_Card_Send_Singo_Cancel(txt_OrderNumber.Text, mtxtMbid.Text.Trim(), "Card", 0, "C");

            ////////if (ret_C1 >= 1)
            ////////{
            ////////    MessageBox.Show("카드 승인 내역 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
            ////////    return;
            ////////}


            ////////if (ret_C1 == 100)
            ////////{
            ////////    MessageBox.Show("카드 승인 내역 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다.");
            ////////    return;
            ////////}

            ////////if (ret_C1 == -100)
            ////////{
            ////////    MessageBox.Show("현 프로그램에서 승인 취소할수 없는 카드 내역 입니다. ksnet 웹상에서 취소 처리 하십시요. 매출은 취소 처리 됩니다..");
            ////////    //return;
            ////////}


            ////ret_C1 = 0;
            ////ret_C1 = cccA.Cash_Card_Send_Singo_Cancel(txt_OrderNumber.Text, mtxtMbid.Text.Trim(), "Bank");

            ////if (ret_C1 == 1)
            ////{
            ////    MessageBox.Show("가상계좌 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
            ////    return;
            ////}

            ////if (ret_C1 == 100)
            ////{
            ////    string m_Sg = "가상계좌 취소중에 문제가 발생했습니다. 매출 취소를 계속 진행하시겠습니까?";
            ////    m_Sg = m_Sg + "\n";
            ////    m_Sg = m_Sg + "계속 진행시 가상계좌는 메뉴얼로 직접 취소하셔야 합니다.";

            ////    if (MessageBox.Show(m_Sg, "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            ////}

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                string StrSql = "";
                StrSql = "EXEC Usp_Insert_tbl_Sales_CanCel_CS '" + txt_OrderNumber.Text + "','" + cls_User.gid + "',0";

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);

                Input_Mileage_Table_CanCel(Temp_Connect, Conn, tran);

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


            ////주문일자를 넣었는지 먼저 체크한다. 안넣었으면 넣어라.
            //if (txtSellDateRe.Text.Trim() != "")
            //{
            //    int Ret = 0;
            //    cls_Check_Input_Error c_er = new cls_Check_Input_Error();
            //    Ret = c_er.Input_Date_Err_Check(txtSellDateRe);

            //    if (Ret == -1)
            //    {
            //        txtSellDateRe.Focus(); return false;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
            //           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_SellDate_Re")
            //          + "\n" +
            //          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    txtSellDateRe.Focus(); return false;
            //}


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
                            txtSellDate.Focus(); return false;
                        }
                        if (SalesDetail[t_key].ReturnTF == 3)
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Del_3")
                            + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            txtSellDate.Focus(); return false;
                        }

                        if (SalesDetail[t_key].ReturnTF == 4)
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Del_4")
                            + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            txtSellDate.Focus(); return false;
                        }
                    }
                }
            }


            if (SalesDetail[Ord_N].ReturnTF.ToString() == "2")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_2")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtSellDate.Focus(); return false;
            }

            if (SalesDetail[Ord_N].ReturnTF.ToString() == "3")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_3")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtSellDate.Focus(); return false;
            }

            if (SalesDetail[Ord_N].ReturnTF.ToString() == "4")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_4")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtSellDate.Focus(); return false;
            }

            if (SalesDetail[Ord_N].ReturnTF.ToString() == "5")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Sell_5")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtSellDate.Focus(); return false;
            }



            //공제번호가 있으면 삭제가 안되게 한다. 우선 먼저 공제번호를 취소한후에 다시 시도하게 한다.
            cls_form_Meth cm = new cls_form_Meth();
            if (SalesDetail[Ord_N].INS_Num_Real != "" && cls_app_static_var.Sell_Union_Flag == "U")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Chang_Insur_Number")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                butt_Delete.Focus(); return false;
            }



            cls_Search_DB csd = new cls_Search_DB();

            //마감정산이 이루어진 판매 날짜인지 체크한다.                
            if (csd.Close_Check_SellDate("tbl_CloseTotal_01", SalesDetail[Ord_N].SellDate.Replace("-", "")) == false)
            {
                txtSellDate.Focus(); return false;
            }



            //재고 관련해서 출고가 된내역인지 확인한다 출고 되었으면 삭제 되면 안됨.
            if (csd.Check_Stock_OutPut(txt_OrderNumber.Text.Trim()) == false)
            {
                butt_Delete.Focus(); return false;
            }




            if (cls_app_static_var.Sell_Union_Flag == "U") //특판
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

                    if (MessageBox.Show(t_Msg, "", MessageBoxButtons.YesNo) == DialogResult.No) return false;
                }
            }
            return true;
        }
        private Boolean Before_Order_Check(string OrderNumber, int C_Index)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            Tsql = " SELECT ISNULL(C_Number3, '') FROM tbl_Sales_Cacu (NOLOCK) ";
            Tsql = Tsql + " WHERE OrderNumber = '" + OrderNumber + "' ";
            Tsql = Tsql + " AND C_index = " + C_Index;


            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Before_Check", ds, this.Name, this.Text) == false) return false;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return false;

            if (ds.Tables["Before_Check"].Rows[0][0].ToString().Trim() == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Cancel_Base_Data(ref int Cancel_Error_Check)
        {
            Cancel_Error_Check = 0;

            //주문종류 , 회원, 주문일자 입력 안햇는지 체크
            if (Check_Delete_TextBox_Error() == false) return;

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Cancel_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;





            //int ret_C1 = 0;
            //cls_Kicc_Send cks = new cls_Kicc_Send();
            //cks.Cash_Card_Send_Singo_Cancel(txt_OrderNumber.Text, mtxtMbid.Text.Trim(), "Card", 0,"C");

            //if (ret_C1 == 1)  //프로그램오류일 경우
            //{
            //    MessageBox.Show("카드 승인 내역 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.-1");
            //    return;
            //}

            //if (ret_C1 == 100) //카드승인 내역의 오류일 경우
            //{
            //    MessageBox.Show("카드 승인 내역 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다.-100");
            //    return;
            //}


            //ret_C1 = 0;
            //cks.Cash_Card_Send_Singo_Cancel(txt_OrderNumber.Text, mtxtMbid.Text.Trim(), "Bank", 0, "C");

            //if (ret_C1 == 1)
            //{
            //    MessageBox.Show("가상계좌 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
            //    return;
            //}

            //if (ret_C1 == 100)
            //{
            //    string m_Sg = "가상계좌 취소중에 문제가 발생했습니다. 매출 취소를 계속 진행하시겠습니까?";
            //    m_Sg = m_Sg + "\n";
            //    m_Sg = m_Sg + "계속 진행시 가상계좌는 메뉴얼로 직접 취소하셔야 합니다.";

            //    if (MessageBox.Show(m_Sg, "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            //}


            //ret_C1 = 0;
            //cks.Cash_Card_Send_Singo_Cancel(txt_OrderNumber.Text, mtxtMbid.Text.Trim(), "Cash", 0, "C");

            //if (ret_C1 == 1)
            //{
            //    MessageBox.Show("현금영수증 승인 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
            //    return;
            //}

            //if (ret_C1 == 100)
            //{
            //    string m_Sg = "현금영수증 승인 취소중에 문제가 발생했습니다. 매출 취소를 계속 진행하시겠습니까?";
            //    m_Sg = m_Sg + "\n";
            //    m_Sg = m_Sg + "계속 진행시 현금영수증 승인 내역은 메뉴얼로 직접 취소하셔야 합니다.";

            //    if (MessageBox.Show(m_Sg, "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            //}


            /*
            cls_Cash_Card_Admin_Cancel cccA = new cls_Cash_Card_Admin_Cancel();

            int ret_C1 = cccA.Cash_Card_Send_Singo_Cancel(txt_OrderNumber.Text, mtxtMbid.Text.Trim(), "Cash");
            if (ret_C1 == 1)
            {
                MessageBox.Show("현금 영수증 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
                return;
            }

            if (ret_C1 == 100)
            {
                MessageBox.Show("현금 영수증 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다.");
                return;
            }



            ret_C1 = 0;
            ret_C1 = cccA.Cash_Card_Send_Singo_Cancel(txt_OrderNumber.Text, mtxtMbid.Text.Trim(), "Card", 0, "C");

            if (ret_C1 >= 1)
            {
                MessageBox.Show("카드 승인 내역 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
                return;
            }


            if (ret_C1 == 100)
            {
                MessageBox.Show("카드 승인 내역 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다.");
                return;
            }

            if (ret_C1 == -100)
            {
                MessageBox.Show("현 프로그램에서 승인 취소할수 없는 카드 내역 입니다. ksnet 웹상에서 취소 처리 하십시요. 매출은 취소 처리 됩니다..");
             //return;
            }


            ret_C1 = 0;
            ret_C1 = cccA.Cash_Card_Send_Singo_Cancel(txt_OrderNumber.Text, mtxtMbid.Text.Trim(), "Bank");

            if (ret_C1 == 1)
            {
                MessageBox.Show("가상계좌 취소중에 문제가 발생했습니다. 매출 취소 처리는 더이상 진행 돼지 않습니다. 업체에 문의해 주십시요.");
                return;
            }

            if (ret_C1 == 100)
            {
                string m_Sg = "가상계좌 취소중에 문제가 발생했습니다. 매출 취소를 계속 진행하시겠습니까?";
                m_Sg = m_Sg + "\n";
                m_Sg = m_Sg + "계속 진행시 가상계좌는 메뉴얼로 직접 취소하셔야 합니다.";

                if (MessageBox.Show(m_Sg, "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }

            */
            cls_Web Cls_Web = new cls_Web();
            string SuccessYN = "";
            int C_Index = 0;
            //20190306 구현호 일단 주석...

            for (int i = 0; i < dGridView_Base_Cacu.Rows.Count; i++)
            {
                SuccessYN = "N";
                C_Index = int.Parse(dGridView_Base_Cacu.Rows[i].Cells[0].Value.ToString());
                /*카드취소*/
                if (dGridView_Base_Cacu.Rows[i].Cells[9].Value.ToString() == "3")
                {

                    if (Before_Order_Check(txt_OrderNumber.Text, C_Index) == true)
                    {
                        SuccessYN = Cls_Web.Before_Card_Cancel(txt_OrderNumber.Text, C_Index);
                        if (SuccessYN == "N")
                        {
                            MessageBox.Show("카드 취소중 문제가 발생했습니다.\n전산담당자에게 확인 부탁드립니다.");
                            return;
                        }
                    }
                    else
                    {
                        string ErrMessage = "";
                        SuccessYN = Cls_Web.Dir_Card_Approve_Cancel(txt_OrderNumber.Text, C_Index, ref ErrMessage);

                        if (SuccessYN == "N")
                        {
                            MessageBox.Show("카드 취소중 문제가 발생했습니다.\n전산담당자에게 확인 부탁드립니다." + Environment.NewLine +
                                "PG Message : " + ErrMessage);
                            return;
                        }
                    }
                }
                ///*가상계좌취소*/
                //if (dGridView_Base_Cacu.Rows[i].Cells[9].Value.ToString() == "5")
                //{
                //    if (VR_Cancel_Chk(txt_OrderNumber.Text, C_Index) == true)       //가상계좌 입금이 되지 않았으면 가상계좌 취소
                //    {
                //        SuccessYN = Cls_Web.Dir_VR_Account_Approve_Cancel(txt_OrderNumber.Text, C_Index);
                //        if (SuccessYN == "N")
                //        {
                //            MessageBox.Show("가상계좌 취소중 문제가 발생했습니다.\n전산담당자에게 확인 부탁드립니다.");
                //            return;
                //        }
                //    }
                //}

            }



            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {

                //직판 관련 조합 취소를 먼저 한다.. 이루어 지지 않았을 경우에는 메시지를 뛰우고 현 프로그램에서는 취소 과정을 그대로 진행한다.
                if (cls_app_static_var.Sell_Union_Flag == "D")
                {
                    Cancel_InsurancerNumber(); //직판 관련 매출 취소가 이루어진다. 취소가 되든 안되든 우리쪽 프로그램에서는 취소를 시킨다..  직판 오류지 알아서 하라고함.                   
                }


                cls_Search_DB csd = new cls_Search_DB();

                //수정하기 전에 배열에다가 내역을 받아둔다.
                csd.SalesDetail_Mod_BackUp(txt_OrderNumber.Text, "tbl_SalesDetail");

                string StrSql = "";
                StrSql = "EXEC Usp_Insert_tbl_Sales_CanCel_CS__02 '" + txt_OrderNumber.Text + "','" + cls_User.gid + "',0";

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);

                Input_Mileage_Table_CanCel(Temp_Connect, Conn, tran);

                //주테이블의 변경 내역을 테이블에 넣는다.
                csd.SalesDetail_Mod(Conn, tran, txt_OrderNumber.Text, "tbl_SalesDetail");

                tran.Commit();

                //조합에 취소 전물을 보낸다. 매출 취소시 조합에 취소 요청 자동으로 해달라는 요청에 의해서  2015-02-26일 과장님 요청에 의해서 미팅후
                StrSql = "EXEC p_mlmunion_OrderDel '" + cls_app_static_var.T_Company_Code + "','" + txt_OrderNumber.Text + "',1";

                Temp_Connect.Update_Data(StrSql, "", "", 1);


                Cancel_Error_Check = 1;





                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Cancel"));
            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Cencel_Err"));

            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }

        }
        private void Input_Mileage_Table_CanCel(cls_Connect_DB Temp_Connect,
                                            SqlConnection Conn, SqlTransaction tran)
        {
            if (SalesDetail[txt_OrderNumber.Text].InputMile > 0)
            {
                string OrderNumber = txt_OrderNumber.Text;

                cls_tbl_Mileage ctm = new cls_tbl_Mileage();
                ctm.Put_Plus_Mileage(SalesDetail[OrderNumber].Mbid, SalesDetail[OrderNumber].Mbid2, SalesDetail[OrderNumber].M_Name
                    , SalesDetail[OrderNumber].InputMile, SalesDetail[OrderNumber].OrderNumber, "22"
                    , Temp_Connect, Conn, tran, "", this.Name.ToString(), this.Text);
            }
        }
        private void Cancel_InsurancerNumber()
        {

            string strSql = "";

            strSql = "Select InsuranceNumber , InsuranceNumber_Cancel , Replace(LEFT(RecordTime,10),'-','') RecordT   ";
            strSql = strSql + " From tbl_SalesDetail (nolock) ";
            strSql = strSql + " Where OrderNumber = '" + txt_OrderNumber.Text + "'";


            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;

            //공제번호가 발행이 되었고.. 그 내역이 취소된게 아니다.. 그럼 공제 번호 취소 하는 과정을 거친다.
            if (ds.Tables[base_db_name].Rows[0]["InsuranceNumber"].ToString() != "" && ds.Tables[base_db_name].Rows[0]["InsuranceNumber_Cancel"].ToString() == "")
            {
                if (ds.Tables[base_db_name].Rows[0]["RecordT"].ToString() == cls_User.gid_date_time)  //저장일자가 현날짜와 동일하다.
                {
                    cls_Socket csg = new cls_Socket();
                    string Req = csg.Dir_Connect_Send_Cancel(txt_OrderNumber.Text);

                    if (Req != "")
                    {
                        if (Req == "-1")
                        {
                            MessageBox.Show("공제번호 발급 중계프로그램(dsclientA.exe)이 " + cls_app_static_var.Dir_Company_Name + " 중계서버에서 미실행 상태입니다."
                                    + "\n" +
                                    cls_app_static_var.Dir_Company_Name + " 전산담당자에게 연락하셔서 공제번호 발급이 되도록 요청바랍니다.");
                        }
                        else
                        {
                            MessageBox.Show("조합 관련 Error Number : " + Req);
                        }
                        MessageBox.Show("조합 관련 Error 입니다. 주문취소는 이루어 지지만 조합관련 취소는 별도로 신청 하셔야 합니다.");
                    }
                    else
                    {
                        MessageBox.Show("공제번호가 정상적으로 취소 되었습니다.");
                    }
                }
                else
                {
                    MessageBox.Show("당일 내역에 대해서만 직판 조합 취소 처리가 됩니다. 현 내역은 별도로 조합에 문의를 해주십시요.");
                }
            }
        }

        private void txtCenter_EditValueChanged(object sender, EventArgs e)
        {
            //  txtCenter.EditValue.ToString();
            string displayText = txtCenter.Properties.GetDisplayText(txtCenter.EditValue);
            string displayText1 = txtCenter.Properties.GetDisplayText(txtCenter.EditValue);

        }

        private void frmSell_Cancel_dev_Load(object sender, EventArgs e)
        {
            InitCombo();
            this.ActiveControl = txtName;
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
        private void Set_SalesDetail(string OrderNumber)
        {
            int idx_ReturnTF = SalesDetail[OrderNumber].ReturnTF;

            Data_Set_Form_TF = 1;

            if (idx_ReturnTF == 3 || idx_ReturnTF == 1 || idx_ReturnTF == 5)
            {
                txtSellDate.Text = SalesDetail[OrderNumber].SellDate.Replace("-", "");
                txtSellCode.Text = SalesDetail[OrderNumber].SellCodeName;
                txtSellCode_Code.Text = SalesDetail[OrderNumber].SellCode;
                txtCenter2.Text = SalesDetail[OrderNumber].BusCodeName;
                txtCenter2_Code.Text = SalesDetail[OrderNumber].BusCode;

                txt_Ins_Number.Text = SalesDetail[OrderNumber].INS_Num;

                //string.Format(cls_app_static_var.str_Currency_Type, ds.Tables[base_db_name].Rows[0]["Last_price"]);
                txt_OrderNumber.Text = OrderNumber;
                txt_TotalPrice.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalInputPrice);
                txt_TotalPv.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalPV);

                txt_TotalInputPrice.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalInputPrice);
                txt_UnaccMoney.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].UnaccMoney);

                txt_ETC1.Text = SalesDetail[OrderNumber].Etc1;
                txt_ETC2.Text = SalesDetail[OrderNumber].Etc2;
            }
            else if (idx_ReturnTF == 2)
            {
                string Re_BaseOrderNumber = SalesDetail[OrderNumber].Re_BaseOrderNumber.Trim();
                txtSellDate.Text = SalesDetail[Re_BaseOrderNumber].SellDate.Replace("-", "");
                txtSellCode.Text = SalesDetail[Re_BaseOrderNumber].SellCodeName;
                txtSellCode_Code.Text = SalesDetail[Re_BaseOrderNumber].SellCode;
                txtCenter2.Text = SalesDetail[Re_BaseOrderNumber].BusCodeName;
                txtCenter2_Code.Text = SalesDetail[Re_BaseOrderNumber].BusCode;
                txt_OrderNumber.Text = Re_BaseOrderNumber;
                txt_TotalPrice.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[Re_BaseOrderNumber].TotalPrice);
                txt_TotalPv.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[Re_BaseOrderNumber].TotalPV);

                txt_TotalInputPrice.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[Re_BaseOrderNumber].TotalInputPrice);
                txt_UnaccMoney.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[Re_BaseOrderNumber].UnaccMoney);

                txt_ETC1.Text = SalesDetail[Re_BaseOrderNumber].Etc1;
                txt_ETC2.Text = SalesDetail[Re_BaseOrderNumber].Etc2;



                txtSellDateRe.Text = SalesDetail[OrderNumber].SellDate.Replace("-", "");
                txt_OrderNumber_R.Text = OrderNumber;
                txt_TotalPrice_R.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalPrice);
                txt_TotalPv_R.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalPV);

                //txt_TotalInputPrice.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalInputPrice);
                //txt_UnaccMoney.Text = string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].UnaccMoney);

                txt_ETC1_R.Text = SalesDetail[OrderNumber].Etc1;
                txt_ETC2_R.Text = SalesDetail[OrderNumber].Etc2;
            }

            Data_Set_Form_TF = 0;
        }
    

        private void butt_Delete_Click(object sender, EventArgs e)
        {
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            //dGridView_Base_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Item.d_Grid_view_Header_Reset();

            //dGridView_Base_Cacu_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Cacu.d_Grid_view_Header_Reset();

            //dGridView_Base_Rece_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Rece.d_Grid_view_Header_Reset();

       
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<



            txtName.ReadOnly = false;
            txtName.BackColor = SystemColors.Window;

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtMbid);

            Base_Ord_Clear();
            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
           
            idx_Mbid = ""; idx_Mbid2 = 0;
            mtxtMbid.Focus();

            txtCenter.EditValue = null;
        }

        private void mtxtMbid_KeyPress(object sender, KeyPressEventArgs e)
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
        private void txtName_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            Base_Ord_Clear();

            if ((sender as DataGridView).CurrentRow.Cells[2].Value != null)
            {
                string OrderNumber = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString();

                if (OrderNumber != "")
                {
                    ////매출 등록한 날짜가 아니다.. 그럼 취소 불가능 하게 한다
                    //if (SalesDetail[OrderNumber].RecordTime.Substring(0, 10).Replace("-", "") != cls_User.gid_date_time)
                    //{
                    //    MessageBox.Show("당일 등록한 매출에 대해서만 취소가 가능합니다.");
                    //    return;
                    //}

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
            }
        }

        private void dGridView_Base_DockChanged(object sender, EventArgs e)
        {

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (Keys.F1.Equals(keyData))
            {
                butt_Delete_Click(null, null);
            }
            else if (Keys.F2.Equals(keyData))
            {
                butt_Save_Click(null, null);
            }
            else if (Keys.F12.Equals(keyData))
            {
                this.Close();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}