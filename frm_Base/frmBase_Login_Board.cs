using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;




namespace MLM_Program
{
    public partial class frmBase_Login_Board : clsForm_Extends
    {
        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_sale = new cls_Grid_Base();
        cls_Grid_Base cgb_item = new cls_Grid_Base();

        private const string base_db_name = "tbl_Memberinfo";

        public delegate void SendNumberDele(string Send_Number, string Send_Name, string Send_OrderNumber);
        public event SendNumberDele Send_Sell_Number;

        public delegate void Send_Mem_NumberDele(string Send_Number, string Send_Name);
        public event Send_Mem_NumberDele Send_Mem_Number;
        

        public frmBase_Login_Board()
        {
            InitializeComponent();
        }


        private void frmBase_Login_Board_Load(object sender, EventArgs e)
        {
           
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            dGridView_Sale_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_sale.d_Grid_view_Header_Reset();

            dGridView_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_item.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            //butt_Exit
        }



        private void frmBase_Resize(object sender, EventArgs e)
        {

            butt_Exit.Left = this.Width - butt_Exit.Width - 17;

            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Exit);
        }

        private void frmBase_Login_Board_Activated(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void frmBase_Login_Board_Shown(object sender, EventArgs e)
        {
            DataSearch();
        }


        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            if (bt.Name == "butt_Exit")
            {
                this.Close();
            }
        }


        private void DataSearch()
        {
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            dGridView_Sale_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_sale.d_Grid_view_Header_Reset();

            Member_Join_Today_Grid();           //당일회원가입현황 그리드
            Member_Num_Chart();                 //직급별, 센터별 회원수 차트
            Sum_Price_Chart();                  //전전월, 전월, 당월 매출비교
            Sale_This_Month_Chart();            //당월 매출(일별, 결제유형별)
            Sale_Today_Info_Grid();             //당일 매출정보

        }


        private void Sale_Today_Info_Grid()
        {
            string Str_Query = "";
            Str_Query = " Select tbl_SalesDetail.OrderNumber,	";
            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                Str_Query = Str_Query + " tbl_SalesDetail.mbid + '-' + Convert(Varchar,tbl_SalesDetail.mbid2)  ";
            else if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                Str_Query = Str_Query + " tbl_SalesDetail.mbid2  ";
            else if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                Str_Query = Str_Query + " tbl_SalesDetail.mbid  ";

            Str_Query = Str_Query + @" , tbl_SalesDetail.M_Name,			
	                                    tbl_Memberinfo.hptel,			
	                                    tbl_SellType.SellTypeName,		
	                                    tbl_business1.name,				
	                                    tbl_business2.name,				
	                                    tbl_SalesDetail.TotalPrice,		
	                                    ISNULL(Cacu.Price1, 0) Price1,	
	                                    ISNULL(Cacu.Price2, 0) Price2,	
	                                    ISNULL(Cacu.Price3, 0) Price3,	
	                                    ISNULL(Cacu.Price4, 0) Price4,	
	                                    ISNULL(Cacu.Price5, 0) Price5,	
	                                    tbl_SalesDetail.UnaccMoney		
                                    From tbl_SalesDetail (nolock)
                                    Left Outer Join tbl_Memberinfo (nolock) on tbl_SalesDetail.mbid = tbl_Memberinfo.mbid And tbl_SalesDetail.mbid2 = tbl_Memberinfo.mbid2
                                    Left Outer Join tbl_SellType on tbl_SalesDetail.SellCode = tbl_SellType.SellCode
                                    Left Outer Join tbl_Business tbl_business1 on tbl_Memberinfo.businesscode = tbl_business1.ncode
                                    Left Outer Join tbl_Business tbl_business2 on tbl_SalesDetail.BusCode = tbl_business2.ncode
                                    Left Outer Join (
					                                    Select
						                                    tbl_Sales_Cacu.OrderNumber, 
						                                    SUM(ISNULL(Case When tbl_Sales_Cacu.C_TF = '1' THEN tbl_Sales_Cacu.C_Price1 ELSE 0 END, 0)) Price1,
						                                    SUM(ISNULL(Case When tbl_Sales_Cacu.C_TF = '2' THEN tbl_Sales_Cacu.C_Price1 ELSE 0 END, 0)) Price2,
						                                    SUM(ISNULL(Case When tbl_Sales_Cacu.C_TF = '3' THEN tbl_Sales_Cacu.C_Price1 ELSE 0 END, 0)) Price3,
						                                    SUM(ISNULL(Case When tbl_Sales_Cacu.C_TF = '4' THEN tbl_Sales_Cacu.C_Price1 ELSE 0 END, 0)) Price4,
						                                    Sum(ISNULL(Case When tbl_Sales_Cacu.C_TF = '5' THEN tbl_Sales_Cacu.C_Price1 ELSE 0 END, 0)) Price5
					                                    From tbl_Sales_Cacu (nolock)
					                                    Left Outer Join tbl_SalesDetail (nolock) on tbl_Sales_Cacu.OrderNumber = tbl_SalesDetail.OrderNumber
					                                    Where tbl_SalesDetail.SellDate = CONVERT(nvarchar(8), GETDATE(), 112)
					                                    Group By tbl_Sales_Cacu.OrderNumber
				                                    ) Cacu on tbl_SalesDetail.OrderNumber = Cacu.OrderNumber
                                    Where tbl_SalesDetail.SellDate = CONVERT(nvarchar(8), GETDATE(), 112)
                                    ";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();

            if (Temp_Connect.Open_Data_Set(Str_Query, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;

            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            Dictionary<string, string> dic_Date = new Dictionary<string, string>();

            string Base_Date = "";

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic_Sale(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                Base_Date = ds.Tables[base_db_name].Rows[fi_cnt][0].ToString();

                if (dic_Date.ContainsKey(Base_Date) == false)
                    dic_Date[Base_Date] = Base_Date;
            }
            
            cgb_sale.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_sale.db_grid_Obj_Data_Put();

        }


        private void Sale_This_Month_Chart()
        {
            cls_form_Meth cm = new cls_form_Meth();
            string Str_Query = @"
                                Select
                                    RIGHT(CONVERT(nvarchar(8), DATEADD(D,-Ca.number,CONVERT(nvarchar(8), DATEADD(MONTH, 1, GETDATE()) - DAY(GETDATE()), 112)), 112), 2) DT, ISNULL(sale.TotalPrice, 0) TotalPrice
                                From master..spt_values Ca
                                Left Outer Join (
					                                Select
					                                tbl_SalesDetail.SellDate, SUM(tbl_SalesDetail.TotalPrice) TotalPrice
					                                From tbl_SalesDetail (nolock)
					                                Where SellDate Like LEFT(CONVERT(nvarchar(8), GETDATE(), 112), 6) + '%'
					                                Group By tbl_SalesDetail.SellDate
				                                ) sale on CONVERT(nvarchar(8), DATEADD(D,-Ca.number,CONVERT(nvarchar(8), DATEADD(MONTH, 1, GETDATE()) - DAY(GETDATE()), 112)), 112) = sale.SellDate
                                Where Ca.type = 'P' And Ca.number <= DATEDIFF(D, DATENAME(YEAR,GETDATE()) + DATENAME(month,GETDATE())+'01', CONVERT(nvarchar(8), DATEADD(MONTH, 1, GETDATE()) - DAY(GETDATE()), 112))
                                Order By DT ASC
                                ";

            Str_Query = Str_Query + " Select ";
            Str_Query = Str_Query + " CASE tbl_Sales_Cacu.C_TF  ";
            Str_Query = Str_Query + "   WHEN '1' THEN '" + cm._chang_base_caption_search("현금") + "'  ";
            Str_Query = Str_Query + "   WHEN '2' THEN '" + cm._chang_base_caption_search("무통장") + "'  ";
            Str_Query = Str_Query + "   WHEN '3' THEN '" + cm._chang_base_caption_search("카드") + "'  ";
            Str_Query = Str_Query + "   WHEN '4' THEN '" + cm._chang_base_caption_search("마일리지") + "'  ";
            Str_Query = Str_Query + "   WHEN '5' THEN '" + cm._chang_base_caption_search("가상계좌") + "'  ";
            Str_Query = Str_Query + " ELSE ''  ";
            Str_Query = Str_Query + "END NM_TYPE, ";
            Str_Query = Str_Query + "SUM(C_Price1) Price ";
            Str_Query = Str_Query + "From tbl_Sales_Cacu (nolock) ";
            Str_Query = Str_Query + "Where c_appdate1 Like LEFT(CONVERT(nvarchar(8), GETDATE(), 112), 6)+ '%' ";
            Str_Query = Str_Query + "Group By tbl_Sales_Cacu.C_TF ";


            Str_Query = Str_Query + @"
                                Select
	                                tbl_Business.ncode, tbl_Business.name, SUM(tbl_Salesdetail.TotalPrice) TotalPrice
                                From tbl_SalesDetail (nolock)
                                Left Outer Join tbl_Memberinfo (nolock) on tbl_SalesDetail.mbid = tbl_Memberinfo.mbid And tbl_SalesDetail.mbid2 = tbl_Memberinfo.mbid2
                                Left Outer Join tbl_Business on tbl_Memberinfo.businesscode = tbl_Business.ncode
                                Where tbl_SalesDetail.SellDate Like LEFT(CONVERT(nvarchar(8), GETDATE(), 112), 6) + '%'
                                And ISNULL(tbl_business.ncode, '') <> ''
                                Group by tbl_Business.ncode, tbl_Business.name
                                Order by tbl_Business.ncode


                                Select Top 1 ItemCode From tbl_MakeITemCode2

                                Select
                                tbl_MakeItemCode2.UpitemCode + tbl_MakeItemCode2.ItemCode UpItemCode, tbl_MakeItemCode2.ItemName, ISNULL(Item.ItemTotalPrice, 0) ItemTotalPrice
                                From tbl_MakeItemCode2 (nolock)
                                Left Outer Join (
					                                Select
					                                tbl_Goods.Up_itemCode, SUM(tbl_SalesItemDetail.ItemTotalPrice) ItemTotalPrice
					                                From tbl_SalesItemDetail (nolock)
					                                Inner Join tbl_SalesDetail (nolock) on tbl_SalesItemDetail.OrderNumber = tbl_SalesDetail.OrderNumber
					                                Left Outer Join tbl_Goods on tbl_SalesItemDetail.ItemCode = tbl_Goods.ncode
					                                Where tbl_SalesDetail.SellDate Like LEFT(CONVERT(nvarchar(8), GETDATE(), 112), 6) + '%'
					                                Group By tbl_Goods.Up_itemCode
				                                ) Item on tbl_MakeItemCode2.UpitemCode + tbl_MakeItemCode2.ItemCode = Item.Up_itemCode
                                Order By tbl_MakeItemCode2.UpitemCode + tbl_MakeItemCode2.ItemCode ASC


                                Select tbl_MakeItemCode1.ItemCode, tbl_MakeItemCode1.ItemName, ISNULL(Item.ItemTotalPrice, 0) ItemTotalPrice
                                From tbl_MakeItemCode1 (nolock)
                                Left Outer Join (
					                                Select
						                                tbl_Goods.Up_itemCode, SUM(tbl_SalesItemDetail.ItemTotalPrice) ItemTotalPrice
                                                    From tbl_SalesItemDetail (nolock)
                                                    Inner Join tbl_SalesDetail (nolock) on tbl_SalesItemDetail.OrderNumber = tbl_SalesDetail.OrderNumber
                                                    Left Outer Join tbl_Goods on tbl_SalesItemDetail.ItemCode = tbl_Goods.ncode
                                                    Where tbl_SalesDetail.SellDate Like LEFT(CONVERT(nvarchar(8), GETDATE(), 112), 6) + '%'
                                                    Group By tbl_Goods.Up_itemCode
				                                ) Item on tbl_MakeItemCode1.ItemCode = Item.Up_itemCode
                                Order By tbl_MakeItemCode1.ItemCode

                                ";
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();

            if (Temp_Connect.Open_Data_Set(Str_Query, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            
            if (ReCnt == 0) return;

            /*일별*/
            chart_day.Series.Clear();
            chart_day.ChartAreas.Clear();

            chart_day.ChartAreas.Add("Day");
            Series series_day;
            series_day = new Series();

            series_day.ChartArea = "Day";
            series_day.ChartType = SeriesChartType.Column;
            series_day.XValueMember = ds.Tables[0].Columns[0].ToString();
            series_day.YValueMembers = ds.Tables[0].Columns[1].ToString();
            series_day.Color = Color.FromArgb(89, 117, 156);

            chart_day.Series.Add(series_day);

            chart_day.ChartAreas["Day"].AxisX.Interval = 1;
            chart_day.ChartAreas["Day"].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_day.ChartAreas["Day"].AxisX.LabelAutoFitMaxFontSize = 7;
            chart_day.ChartAreas["Day"].AxisY.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_day.ChartAreas["Day"].AxisY.LabelAutoFitMaxFontSize = 7;
            chart_day.ChartAreas["Day"].AxisY.LabelStyle.Format = cls_app_static_var.str_Currency_Type;
            chart_day.ChartAreas["Day"].AxisX.MajorGrid.Enabled = false;
            chart_day.ChartAreas["Day"].AxisY.MajorGrid.Enabled = true;
            chart_day.Series[0].IsVisibleInLegend = false;
            chart_day.Series[0]["PixelPointWidth"] = "15";

            chart_day.DataSource = ds.Tables[0];
            chart_day.DataBind();
            chart_day.GetToolTipText += Chart_GetToolTipText;


            /*결제유형별*/
            chart_paytype.Series.Clear();
            chart_paytype.ChartAreas.Clear();

            chart_paytype.ChartAreas.Add("PayType");
            Series series_paytype;
            series_paytype = new Series();

            series_paytype.ChartArea = "PayType";
            series_paytype.ChartType = SeriesChartType.Column;
            series_paytype.XValueMember = ds.Tables[1].Columns[0].ToString();
            series_paytype.YValueMembers = ds.Tables[1].Columns[1].ToString();
            series_paytype.Color = Color.FromArgb(89, 117, 156);

            chart_paytype.Series.Add(series_paytype);
            
            chart_paytype.ChartAreas["PayType"].AxisX.Interval = 1;
            chart_paytype.ChartAreas["PayType"].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_paytype.ChartAreas["PayType"].AxisX.LabelAutoFitMaxFontSize = 7;
            chart_paytype.ChartAreas["PayType"].AxisY.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_paytype.ChartAreas["PayType"].AxisY.LabelAutoFitMaxFontSize = 7;
            chart_paytype.ChartAreas["PayType"].AxisY.LabelStyle.Format = cls_app_static_var.str_Currency_Type;
            chart_paytype.ChartAreas["PayType"].AxisX.MajorGrid.Enabled = false;
            chart_paytype.ChartAreas["PayType"].AxisY.MajorGrid.Enabled = true;
            chart_paytype.Series[0].IsVisibleInLegend = false;
            chart_paytype.Series[0]["PixelPointWidth"] = "15";

            chart_paytype.DataSource = ds.Tables[1];
            chart_paytype.DataBind();
            chart_paytype.GetToolTipText += Chart_GetToolTipText;


            /*센터별*/
            chart_center_price.Series.Clear();
            chart_center_price.ChartAreas.Clear();

            chart_center_price.ChartAreas.Add("Center");
            Series series_center;
            series_center = new Series();

            series_center.ChartArea = "Center";
            series_center.ChartType = SeriesChartType.Column;
            series_center.XValueMember = ds.Tables[2].Columns[1].ToString();
            series_center.YValueMembers = ds.Tables[2].Columns[2].ToString();
            series_center.Color = Color.FromArgb(89, 117, 156);

            chart_center_price.Series.Add(series_center);

            chart_center_price.ChartAreas["Center"].AxisX.Interval = 1;
            chart_center_price.ChartAreas["Center"].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 7);
            chart_center_price.ChartAreas["Center"].AxisX.LabelAutoFitMaxFontSize = 7;
            chart_center_price.ChartAreas["Center"].AxisY.TitleFont = new System.Drawing.Font("맑은고딕", 7);
            chart_center_price.ChartAreas["Center"].AxisY.LabelAutoFitMaxFontSize = 7;
            chart_center_price.ChartAreas["Center"].AxisY.LabelStyle.Format = cls_app_static_var.str_Currency_Type;
            chart_center_price.ChartAreas["Center"].AxisX.MajorGrid.Enabled = false;
            chart_center_price.ChartAreas["Center"].AxisY.MajorGrid.Enabled = true;
            chart_center_price.Series[0].IsVisibleInLegend = false;
            chart_center_price.Series[0]["PixelPointWidth"] = "15";
            
            chart_center_price.DataSource = ds.Tables[2];
            chart_center_price.DataBind();
            chart_center_price.GetToolTipText += Chart_GetToolTipText;

            //상품중분류 코드가 등록이 되어 있으면 상품-중분류별로, 없으면 대분류로
            if (ds.Tables[3].Rows.Count == 0)
            {
                tab_sale.TabPages.Add("tabpage_item", "상품-대분류별");

                Chart chart_item;
                chart_item = new Chart();

                chart_item.ChartAreas.Add("Item");
                chart_item.Dock = DockStyle.Fill;
                chart_item.Name = "chart_item";
                tab_sale.TabPages["tabpage_item"].Controls.Add(chart_item);
                tab_sale.TabPages["tabpage_item"].BorderStyle = BorderStyle.FixedSingle;

                Series series_item;
                series_item = new Series();

                series_item.ChartArea = "Item";
                series_item.ChartType = SeriesChartType.Column;
                series_item.XValueMember = ds.Tables[5].Columns[1].ToString();
                series_item.YValueMembers = ds.Tables[5].Columns[2].ToString();
                series_item.Color = Color.FromArgb(89, 117, 156);

                chart_item.Series.Add(series_item);

                chart_item.ChartAreas["Item"].AxisX.Interval = 1;
                chart_item.ChartAreas["Item"].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 7);
                chart_item.ChartAreas["Item"].AxisX.LabelAutoFitMaxFontSize = 7;
                chart_item.ChartAreas["Item"].AxisY.TitleFont = new System.Drawing.Font("맑은고딕", 7);
                chart_item.ChartAreas["Item"].AxisY.LabelAutoFitMaxFontSize = 7;
                chart_item.ChartAreas["Item"].AxisY.LabelStyle.Format = cls_app_static_var.str_Currency_Type;
                chart_item.ChartAreas["Item"].AxisX.MajorGrid.Enabled = false;
                chart_item.ChartAreas["Item"].AxisY.MajorGrid.Enabled = true;
                chart_item.Series[0].IsVisibleInLegend = false;
                chart_item.Series[0]["PixelPointWidth"] = "15";

                chart_item.DataSource = ds.Tables[5];
                chart_item.DataBind();
                chart_item.GetToolTipText += Chart_GetToolTipText;
            }
            else
            {
                tab_sale.TabPages.Add("tabpage_item", "상품-중분류별");

                Chart chart_item;
                chart_item = new Chart();

                chart_item.ChartAreas.Add("Item");
                chart_item.Dock = DockStyle.Fill;
                chart_item.Name = "chart_item";
                tab_sale.TabPages["tabpage_item"].Controls.Add(chart_item);
                tab_sale.TabPages["tabpage_item"].BorderStyle = BorderStyle.FixedSingle;

                Series series_item;
                series_item = new Series();

                series_item.ChartArea = "Item";
                series_item.ChartType = SeriesChartType.Column;
                series_item.XValueMember = ds.Tables[4].Columns[1].ToString();
                series_item.YValueMembers = ds.Tables[4].Columns[2].ToString();
                series_item.Color = Color.FromArgb(89, 117, 156);

                chart_item.Series.Add(series_item);

                chart_item.ChartAreas["Item"].AxisX.Interval = 1;
                chart_item.ChartAreas["Item"].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 7);
                chart_item.ChartAreas["Item"].AxisX.LabelAutoFitMaxFontSize = 7;
                chart_item.ChartAreas["Item"].AxisY.TitleFont = new System.Drawing.Font("맑은고딕", 7);
                chart_item.ChartAreas["Item"].AxisY.LabelAutoFitMaxFontSize = 7;
                chart_item.ChartAreas["Item"].AxisY.LabelStyle.Format = cls_app_static_var.str_Currency_Type;
                chart_item.ChartAreas["Item"].AxisX.MajorGrid.Enabled = false;
                chart_item.ChartAreas["Item"].AxisY.MajorGrid.Enabled = true;
                chart_item.Series[0].IsVisibleInLegend = false;
                chart_item.Series[0]["PixelPointWidth"] = "15";

                chart_item.DataSource = ds.Tables[4];
                chart_item.DataBind();
                chart_item.GetToolTipText += Chart_GetToolTipText;

            }

        }


        private void Sum_Price_Chart()
        {
            string Str_Query = @"Select 
	                                Left(CONVERT(nvarchar(8), DATEADD(D,-Ca.number,GETDATE()), 112), 6) AS DT_Group,
	                                Right(CONVERT(nvarchar(8), DATEADD(D,-Ca.number,GETDATE()), 112),2) As DT, Isnull(Sale.TotalPrice, 0) TotalPrice
                                From master..spt_values Ca
                                Left Outer Join (	Select
						                                tbl_SalesDetail.SellDate, SUM(tbl_SalesDetail.TotalPrice) TotalPrice
					                                From tbl_SalesDetail (nolock)
					                                Where tbl_SalesDetail.SellDate Between LEFT(CONVERT(nvarchar(8), DATEADD(month, -2, GETDATE()), 112), 6) +'01' and CONVERT(nvarchar(8), GETDATE(), 112)
					                                Group By tbl_SalesDetail.SellDate
					                                ) Sale on CONVERT(nvarchar(8), DATEADD(D, -Ca.number, GETDATE()), 112) = Sale.SellDate
                                Where Ca.type = 'P' And Ca.number <= DATEDIFF(D,LEFT(CONVERT(nvarchar(8), DATEADD(month, -2, GETDATE()), 112), 6) +'01', CONVERT(nvarchar(8), GETDATE(), 112))
                                Order By DT_Group ASC, DT ASC
                                ";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();

            if (Temp_Connect.Open_Data_Set(Str_Query, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;

            /*차트구성*/
            //차트 초기화
            chart_price.Series.Clear();
            chart_price.ChartAreas.Clear();

            cls_form_Meth cm = new cls_form_Meth();
            Series[] series_arr;
            series_arr = new Series[3]; //어차피 3개월만 보여줄거기 때문에 배열크기 정해버림 
            string dt_group = "";
            
            chart_price.ChartAreas.Add("Price");

            series_arr[0] = new Series();
            series_arr[0].ChartType = SeriesChartType.Line;
            series_arr[0].Name = cm._chang_base_caption_search("전전월");
            series_arr[0].ChartArea = "Price";
            series_arr[0].Color = Color.FromArgb(16, 150, 25);
            series_arr[0].MarkerSize = 5;
            series_arr[0].MarkerStyle = MarkerStyle.Circle;

            series_arr[1] = new Series();
            series_arr[1].ChartType = SeriesChartType.Line;
            series_arr[1].Name = cm._chang_base_caption_search("전월");
            series_arr[1].ChartArea = "Price";
            series_arr[1].Color = Color.FromArgb(63, 112, 207);
            series_arr[1].MarkerSize = 5;
            series_arr[1].MarkerStyle = MarkerStyle.Circle;

            series_arr[2] = new Series();
            series_arr[2].ChartType = SeriesChartType.Line;
            series_arr[2].Name = cm._chang_base_caption_search("당월");
            series_arr[2].ChartArea = "Price";
            series_arr[2].Color = Color.FromArgb(220, 56, 18);
            series_arr[2].MarkerSize = 5;
            series_arr[2].MarkerStyle = MarkerStyle.Circle;

            int series_int = 0;
            for (int i = 0; i < ReCnt; i++)
            {
                if (i == 0) //첫번째 행인 경우
                {
                    dt_group = ds.Tables[0].Rows[i][0].ToString();
                    series_arr[series_int].Points.AddXY(ds.Tables[0].Rows[i][1].ToString(), double.Parse(ds.Tables[0].Rows[i][2].ToString()));
                }
                else if (i != 0 && dt_group == ds.Tables[0].Rows[i][0].ToString())       //윗라인이랑 월이 같은 경우 같은 시리즈로
                {
                    series_arr[series_int].Points.AddXY(ds.Tables[0].Rows[i][1].ToString(), double.Parse(ds.Tables[0].Rows[i][2].ToString()));
                }
                else if (i != 0 && dt_group != ds.Tables[0].Rows[i][0].ToString())      //윗라인이랑 월이 다른 경우 다른 시리즈로
                {
                    series_int++;
                    series_arr[series_int].Points.AddXY(ds.Tables[0].Rows[i][1].ToString(), double.Parse(ds.Tables[0].Rows[i][2].ToString()));
                }
                dt_group = ds.Tables[0].Rows[i][0].ToString();
            }


            chart_price.ChartAreas["Price"].AxisX.Interval = 1;
            chart_price.ChartAreas["Price"].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_price.ChartAreas["Price"].AxisX.LabelAutoFitMaxFontSize = 7;
            chart_price.ChartAreas["Price"].AxisY.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_price.ChartAreas["Price"].AxisY.LabelAutoFitMaxFontSize = 7;
            chart_price.ChartAreas["Price"].AxisY.LabelStyle.Format = cls_app_static_var.str_Currency_Type;
            chart_price.ChartAreas["Price"].AxisX.MajorGrid.Enabled = false;
            chart_price.ChartAreas["Price"].AxisY.MajorGrid.Enabled = true;
            chart_price.ChartAreas["Price"].AxisX.ArrowStyle = AxisArrowStyle.None;
            chart_price.ChartAreas["Price"].AxisY.ArrowStyle = AxisArrowStyle.None;
            
            chart_price.Series.Add(series_arr[0]);
            chart_price.Series.Add(series_arr[1]);
            chart_price.Series.Add(series_arr[2]);

            chart_price.GetToolTipText += Chart_GetToolTipText;
        }


        private void Member_Num_Chart()
        {
            string Str_Query = @"
                                    Select 
	                                    count(1) cnt, tbl_Memberinfo.CurGrade, tbl_Class.Grade_Name
                                    From tbl_Memberinfo (nolock)
                                    Left Outer Join (Select 0 Grade_Code, '회원' Grade_Name Union All Select Grade_Code, Grade_Name From tbl_Class) tbl_Class on tbl_Memberinfo.CurGrade = tbl_Class.Grade_Code
                                    Where ISNULL(tbl_Memberinfo.LeaveDate, '') = ''
                                    And tbl_Memberinfo.LeaveCheck <> '0'
                                    Group by tbl_Memberinfo.CurGrade, tbl_Class.Grade_Name
                                    Order by tbl_Memberinfo.CurGrade


                                    Select 
	                                    count(1) cnt, tbl_Memberinfo.businesscode, tbl_Business.name
                                    From tbl_Memberinfo (nolock)
                                    Left Outer Join tbl_Business on tbl_Memberinfo.businesscode = tbl_Business.ncode
                                    Where tbl_Memberinfo.businesscode in (Select ncode From tbl_Business)
                                    And tbl_Business.U_TF = 0
                                    And ISNULL(tbl_Memberinfo.LeaveDate, '') = ''
                                    And tbl_Memberinfo.LeaveCheck <> '0'
                                    Group by tbl_Memberinfo.businesscode, tbl_Business.name
                                    Order by tbl_Memberinfo.businesscode DESC



                                    Select
	                                    ISNULL(Member.CNT, 0) CNT, RIGHT(CONVERT(nvarchar(8), DATEADD(D,-Ca.number,CONVERT(nvarchar(8), DATEADD(MONTH, 1, GETDATE()) - DAY(GETDATE()), 112)), 112), 2) DT 
                                    From master..spt_values Ca
                                    Left Outer Join (
					                                    Select tbl_Memberinfo.Regtime, COUNT(1) CNT
					                                    From tbl_Memberinfo
					                                    Where tbl_Memberinfo.Regtime Like LEFT(CONVERT(nvarchar(8), GETDATE(), 112), 6) + '%'
					                                    Group by tbl_Memberinfo.Regtime
				                                    ) Member on CONVERT(nvarchar(8), DATEADD(D,-Ca.number,CONVERT(nvarchar(8), DATEADD(MONTH, 1, GETDATE()) - DAY(GETDATE()), 112)), 112) = Member.Regtime
                                    Where Ca.type = 'P' And Ca.number <= DATEDIFF(D, DATENAME(YEAR,GETDATE()) + DATENAME(month,GETDATE())+'01', CONVERT(nvarchar(8), DATEADD(MONTH, 1, GETDATE()) - DAY(GETDATE()), 112))
                                    Order By DT DESC


                                    Select
                                    ISNULL(Member.CNT, 0) CNT, tbl_Business.ncode, tbl_Business.name
                                    From tbl_Business
                                    Left Outer Join (
					                                    Select
					                                    tbl_Memberinfo.businesscode, Count(1) CNT
					                                    From tbl_Memberinfo
					                                    Where tbl_Memberinfo.Regtime Like LEFT(CONVERT(nvarchar(8), GETDATE(), 112), 6) + '%'
					                                    Group By tbl_Memberinfo.businesscode
				                                    ) Member on tbl_Business.ncode = Member.businesscode
				                    Order By tbl_Business.ncode DESC
                            ";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            
            if (Temp_Connect.Open_Data_Set(Str_Query, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;

            chart_grade.Series.Clear();
            chart_center.Series.Clear();
            chart_grade.ChartAreas.Clear();
            chart_center.ChartAreas.Clear();

            /*직급별 회원수*/
            chart_grade.ChartAreas.Add("Grade");
            Series series_grade;
            series_grade = new Series();

            chart_grade.DataSource = ds.Tables[0];

            series_grade.ChartArea = "Grade";
            series_grade.ChartType = SeriesChartType.Bar;
            series_grade.XValueMember = ds.Tables[0].Columns[2].ToString();
            series_grade.YValueMembers = ds.Tables[0].Columns[0].ToString();
            series_grade.Color = Color.FromArgb(89, 117, 156);

            chart_grade.Series.Add(series_grade);

            chart_grade.ChartAreas["Grade"].AxisX.Interval = 1;
            chart_grade.ChartAreas["Grade"].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_grade.ChartAreas["Grade"].AxisX.LabelAutoFitMaxFontSize = 7;
            chart_grade.ChartAreas["Grade"].AxisY.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_grade.ChartAreas["Grade"].AxisY.LabelAutoFitMaxFontSize = 7;
            chart_grade.ChartAreas["Grade"].AxisY.LabelStyle.Format = cls_app_static_var.str_Currency_Type;
            chart_grade.ChartAreas["Grade"].AxisX.MajorGrid.Enabled = false;
            chart_grade.ChartAreas["Grade"].AxisY.MajorGrid.Enabled = false;
            chart_grade.Series[0].IsVisibleInLegend = false;
            chart_grade.Series[0]["PixelPointWidth"] = "15";
            

            chart_grade.DataBind();
            chart_grade.GetToolTipText += Chart_GetToolTipText;


            /*센터별 회원수*/
            chart_center.ChartAreas.Add("Center");
            Series series_center;
            series_center = new Series();

            chart_center.DataSource = ds.Tables[1];

            series_center.ChartArea = "Center";
            series_center.ChartType = SeriesChartType.Bar;
            series_center.XValueMember = ds.Tables[1].Columns[2].ToString();
            series_center.YValueMembers = ds.Tables[1].Columns[0].ToString();
            series_center.Color = Color.FromArgb(89, 117, 156);

            chart_center.Series.Add(series_center);

            chart_center.ChartAreas["Center"].AxisX.Interval = 1;
            chart_center.ChartAreas["Center"].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_center.ChartAreas["Center"].AxisX.LabelAutoFitMaxFontSize = 7;
            chart_center.ChartAreas["Center"].AxisY.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_center.ChartAreas["Center"].AxisY.LabelAutoFitMaxFontSize = 7;
            chart_center.ChartAreas["Center"].AxisY.LabelStyle.Format = cls_app_static_var.str_Currency_Type;
            chart_center.ChartAreas["Center"].AxisX.MajorGrid.Enabled = false;
            chart_center.ChartAreas["Center"].AxisY.MajorGrid.Enabled = false;
            chart_center.Series[0].IsVisibleInLegend = false;
            chart_center.Series[0]["PixelPointWidth"] = "10";
            
            chart_center.DataBind();
            chart_center.GetToolTipText += Chart_GetToolTipText;


            /*당월-일별*/
            chart_member_day.Series.Clear();
            chart_member_day.ChartAreas.Clear();

            chart_member_day.ChartAreas.Add("Member");
            Series series_member_day;
            series_member_day = new Series();

            chart_member_day.DataSource = ds.Tables[2];

            series_member_day.ChartArea = "Member";
            series_member_day.ChartType = SeriesChartType.Bar;
            series_member_day.XValueMember = ds.Tables[2].Columns[1].ToString();
            series_member_day.YValueMembers = ds.Tables[2].Columns[0].ToString();
            series_member_day.Color = Color.FromArgb(89, 117, 156);

            chart_member_day.Series.Add(series_member_day);

            chart_member_day.ChartAreas["Member"].AxisX.Interval = 1;
            chart_member_day.ChartAreas["Member"].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_member_day.ChartAreas["Member"].AxisX.LabelAutoFitMaxFontSize = 7;
            chart_member_day.ChartAreas["Member"].AxisY.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_member_day.ChartAreas["Member"].AxisY.LabelAutoFitMaxFontSize = 7;
            chart_member_day.ChartAreas["Member"].AxisY.LabelStyle.Format = cls_app_static_var.str_Currency_Type;
            chart_member_day.ChartAreas["Member"].AxisX.MajorGrid.Enabled = false;
            chart_member_day.ChartAreas["Member"].AxisY.MajorGrid.Enabled = false;
            chart_member_day.Series[0].IsVisibleInLegend = false;
            chart_member_day.Series[0]["PixelPointWidth"] = "10";

            chart_member_day.DataBind();
            chart_member_day.GetToolTipText += Chart_GetToolTipText;
            

            /*당월-센터별*/
            chart_member_center.Series.Clear();
            chart_member_center.ChartAreas.Clear();

            chart_member_center.ChartAreas.Add("Member");
            Series series_member_center;
            series_member_center = new Series();

            chart_member_center.DataSource = ds.Tables[3];

            series_member_center.ChartArea = "Member";
            series_member_center.ChartType = SeriesChartType.Bar;
            series_member_center.XValueMember = ds.Tables[3].Columns[2].ToString();
            series_member_center.YValueMembers = ds.Tables[3].Columns[0].ToString();
            series_member_center.Color = Color.FromArgb(89, 117, 156);

            chart_member_center.Series.Add(series_member_center);

            chart_member_center.ChartAreas["Member"].AxisX.Interval = 1;
            chart_member_center.ChartAreas["Member"].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_member_center.ChartAreas["Member"].AxisX.LabelAutoFitMaxFontSize = 7;
            chart_member_center.ChartAreas["Member"].AxisY.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_member_center.ChartAreas["Member"].AxisY.LabelAutoFitMaxFontSize = 7;
            chart_member_center.ChartAreas["Member"].AxisY.LabelStyle.Format = cls_app_static_var.str_Currency_Type;
            chart_member_center.ChartAreas["Member"].AxisX.MajorGrid.Enabled = false;
            chart_member_center.ChartAreas["Member"].AxisY.MajorGrid.Enabled = false;
            chart_member_center.Series[0].IsVisibleInLegend = false;
            chart_member_center.Series[0]["PixelPointWidth"] = "10";
            
            chart_member_center.DataBind();
            chart_member_center.GetToolTipText += Chart_GetToolTipText;
        }

        private void Member_Join_Today_Grid()
        {
            string Str_Query = "";

            Str_Query = "Select ";
            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                Str_Query = Str_Query + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2)  ";
            else if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                Str_Query = Str_Query + " tbl_Memberinfo.mbid2  ";
            else if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                Str_Query = Str_Query + " tbl_Memberinfo.mbid  ";
            Str_Query = Str_Query + @" , tbl_Memberinfo.M_Name, tbl_Memberinfo.hptel, tbl_Business.name,
	                            tbl_Memberinfo.Addcode1,
	                                    tbl_Memberinfo.Address1 + ' ' + tbl_Memberinfo.Address2
                                    From tbl_Memberinfo (nolock)
                                    Left Outer Join tbl_Business on tbl_Memberinfo.businesscode = tbl_Business.ncode
                                    Where tbl_Memberinfo.Regtime = CONVERT(nvarchar(8), GETDATE(), 112)
                                    ";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            
            if (Temp_Connect.Open_Data_Set(Str_Query, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            Dictionary<string, string> dic_Date = new Dictionary<string, string>();

            string Base_Date = ""; 

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                Base_Date = ds.Tables[base_db_name].Rows[fi_cnt][0].ToString();

                if (dic_Date.ContainsKey(Base_Date) == false)
                    dic_Date[Base_Date] = Base_Date;
            }


            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();

        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;
            object[] row0 = new object[cgb.grid_col_Count];

            while (Col_Cnt < cgb.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void Set_gr_dic_Sale(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;
            object[] row0 = new object[cgb_sale.grid_col_Count];

            while (Col_Cnt < cgb_sale.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Base_Header_Reset()
        {
            cgb.Grid_Base_Arr_Clear();
            cgb.grid_col_Count = 6;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"회원번호", "회원명", "연락처", "센터", "우편번호"
                                    , "주소"
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 90, 100, 80, 80, 80
                            ,150
                        };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               
                               ,DataGridViewContentAlignment.MiddleLeft                           
                              };
            cgb.grid_col_alignment = g_Alignment;
            
        }


        private void dGridView_Sale_Header_Reset()
        {
            cgb_sale.Grid_Base_Arr_Clear();
            cgb_sale.grid_col_Count = 14;
            cgb_sale.basegrid = dGridView_Sale;
            cgb_sale.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_sale.grid_Frozen_End_Count = 2;
            cgb_sale.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"주문번호", "회원번호", "회원명", "회원연락처", "판매유형"
                                    , "회원센터", "판매센터", "총금액", "현금", "무통장"                                
                                    , "카드", "마일리지", "가상계좌", "미결제액"
                                    };
            cgb_sale.grid_col_header_text = g_HeaderText;

            if (cls_app_static_var.Using_Mileage_TF == 1)
            {
                int[] g_Width = { 90, 100, 80, 80, 80
                                ,150 , 80 , 80 , 80 , 80
                                ,80  , 0 , 80 , 80
                                };
                cgb_sale.grid_col_w = g_Width;
            }
            else
            {
                int[] g_Width = { 90, 100, 80, 80, 80
                                ,150 , 80 , 80 , 80 , 80
                                ,80  , 80 , 80 , 80
                                };
                cgb_sale.grid_col_w = g_Width;
            }
            
            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                        
                                    ,true , true,  true,  true
                                   };
            cgb_sale.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight

                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                           
                              };
            cgb_sale.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[8 - 1] = "###,###,###,##0";
            gr_dic_cell_format[9 - 1] = "###,###,###,##0";
            gr_dic_cell_format[10 - 1] = "###,###,###,##0";
            gr_dic_cell_format[11 - 1] = "###,###,###,##0";
            gr_dic_cell_format[12 - 1] = "###,###,###,##0";
            gr_dic_cell_format[13 - 1] = "###,###,###,##0";
            gr_dic_cell_format[14 - 1] = "###,###,###,##0";

            cgb_sale.grid_cell_format = gr_dic_cell_format;
        }


        private void Chart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            switch (e.HitTestResult.ChartElementType)
            {
                case ChartElementType.DataPoint:
                    var dataPoint = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];
                    e.Text = string.Format(cls_app_static_var.str_Currency_Type, dataPoint.YValues[0]);
                    break;
            }
        }

        private void dGridView_Sale_DoubleClick(object sender, EventArgs e)
        {
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string Send_Nubmer = ""; string Send_Name = ""; ; string Send_OrderNumber = "";

                Send_OrderNumber = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                Send_Nubmer = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
                Send_Name = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString();
                Send_Sell_Number(Send_Nubmer, Send_Name, Send_OrderNumber);
            }           
        }


        private void Item_Grid_Set(string OrderNum)
        {
            if (OrderNum == "") return;

            cls_form_Meth cm = new cls_form_Meth();
            string Str_Query = "";
            Str_Query = " Select tbl_SalesItemDetail.ItemCode, tbl_Goods.name ";
            Str_Query = Str_Query + " ,Case When ReturnTF = 1 Then '" + cm._chang_base_caption_search("정상") + "'";
            Str_Query = Str_Query + "  When ReturnTF = 2 Then '" + cm._chang_base_caption_search("반품") + "'";
            Str_Query = Str_Query + "  When ReturnTF = 4 Then '" + cm._chang_base_caption_search("교환") + "'";
            Str_Query = Str_Query + "  When ReturnTF = 3 Then '" + cm._chang_base_caption_search("부분반품") + "'";
            Str_Query = Str_Query + "  When ReturnTF = 5 Then '" + cm._chang_base_caption_search("취소") + "'";
            Str_Query = Str_Query + " END ReturnTFName ";
            Str_Query = Str_Query + " , tbl_SalesItemDetail.ItemCount, tbl_SalesItemDetail.ItemTotalPrice ";
            Str_Query = Str_Query + " From tbl_SalesItemDetail (nolock) ";
            Str_Query = Str_Query + " Inner Join tbl_SalesDetail (nolock) on tbl_SalesItemDetail.OrderNumber = tbl_SalesDetail.OrderNumber ";
            Str_Query = Str_Query + " Left Outer Join tbl_Goods on tbl_SalesItemDetail.ItemCode = tbl_Goods.ncode ";
            Str_Query = Str_Query + " Where tbl_SalesItemDetail.OrderNumber = '" + OrderNum + "'";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Str_Query, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;

            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_Item(ref ds, ref gr_dic_text, fi_cnt);
            }

            cgb_item.grid_name_obj = gr_dic_text;
            cgb_item.db_grid_Obj_Data_Put();
        }


        private void Set_gr_Item(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;

            object[] row0 = new object[cgb_item.grid_col_Count];

            while (Col_Cnt < cgb_item.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }


            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Item_Header_Reset()
        {
            cgb_item.Grid_Base_Arr_Clear();
            cgb_item.basegrid = dGridView_Item;
            cgb_item.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_item.grid_col_Count = 5;
            cgb_item.grid_Frozen_End_Count = 2;
            cgb_item.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"상품코드"  , "상품명"   , "구분"  , "판매수량"   , "총상품액"        
                                };

            int[] g_Width = { 80, 100, 80, 80, 100
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb_item.grid_col_header_text = g_HeaderText;
            cgb_item.grid_cell_format = gr_dic_cell_format;
            cgb_item.grid_col_w = g_Width;
            cgb_item.grid_col_alignment = g_Alignment;
            
            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                   };
            cgb_item.grid_col_Lock = g_ReadOnly;

        }

        private void dGridView_Sale_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dGridView_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_item.d_Grid_view_Header_Reset();

            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string Str_OrderNum = "";
                Str_OrderNum = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Item_Grid_Set(Str_OrderNum);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }            
        }

        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string Send_Nubmer = ""; string Send_Name = "";
                Send_Nubmer = dGridView_Base.CurrentRow.Cells[0].Value.ToString();
                Send_Name = dGridView_Base.CurrentRow.Cells[1].Value.ToString();
                Send_Mem_Number(Send_Nubmer, Send_Name); 
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


            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123)
                    Base_Button_Click(T_bt, ee1);
            }
        }
    }

}
