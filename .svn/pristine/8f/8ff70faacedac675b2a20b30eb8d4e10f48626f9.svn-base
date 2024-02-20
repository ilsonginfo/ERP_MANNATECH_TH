using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FastReport;
using FastReport.Preview;

using System.IO;

namespace MLM_Program
{
    public partial class frmFastReport : Form
    {
        private readonly string ReportFolder = Application.StartupPath + @"\report\";

        public enum EShowReport
        {
            거래명세표,
            거래명세표_출고용,
            회원증명서
        }
        
        //public DataTable _dataTable = new DataTable();
        
        /// <summary> 리포트 이름 </summary>
        private string _ReportName = string.Empty;

        /// <summary> Fast Report 보여줄 개체 </summary>
        private Report _report = new Report();

        public Dictionary<string, string> Parameter = new Dictionary<string, string>();
        public Dictionary<string, DataTable> BindingDataTables = new Dictionary<string, DataTable>();
        
        public frmFastReport()
        {
            InitializeComponent();
        }

        private void SettingReportName(EShowReport Report)
        {
            if (EShowReport.거래명세표.Equals(Report))
            {
                this._ReportName = "SellTransactionReport.frx";
                this.Text = "판매영수증";
            }
            else if (EShowReport.회원증명서.Equals(Report))
            {
                this._ReportName = "MembershipCardReport.frx";
                this.Text = "회원증명서";
            }
            //else if(....){}
            else if (EShowReport.거래명세표_출고용.Equals(Report))
            {
                this._ReportName = "SellTransactionReport_OnlyStockOutSell.frx";
                this.Text = "판매영수증_출고용";
            }
            //MakeReportToLocal();
        }

        public void ShowReport(EShowReport Report)
        {
            SettingReportName(Report);

            if(EShowReport.거래명세표.Equals(Report))
            {
                Print_Sell_Transaction_Report();
            }
            if (EShowReport.거래명세표_출고용.Equals(Report))
            {
                Print_Sell_Transaction_StockOut_Report();
            }
            else if (EShowReport.회원증명서.Equals(Report))
            {
                Print_MembershipCard_Report();
            }
            else
            {

            }

        }

        private void Print_Sell_Transaction_StockOut_Report()
        {
            //Report 객체 재생성
            _report = new Report();

            //Preview 할당 
            _report.Preview = preview1;

            //레포트 로드
            _report.Load(ReportFolder + this._ReportName);


            ////Binding
            _report.RegisterData(BindingDataTables["Products"], "Products");
            _report.GetDataSource("Products").Enabled = true;

            _report.RegisterData(BindingDataTables["OrderInfomation"], "OrderInfomation");
            _report.GetDataSource("OrderInfomation").Enabled = true;

            //Show 
            _report.Prepare();
            _report.ShowPrepared();

            this.ShowDialog();

            //==========================================================
            ////다이얼로그 띄우지 않음 
            //_report.PrintSettings.ShowDialog = false;

            ////preview에게 print요청
            //preview1.Print();

            ////해제
            //preview1.Dispose();

            ////폼 닫기 
            //this.Close();
            //==========================================================
        }


        private void Print_Sell_Transaction_Report()
        {
            //Report 객체 재생성
            _report = new Report();

            //Preview 할당 
            _report.Preview = preview1;

            //레포트 로드
            _report.Load(ReportFolder + this._ReportName);

            //파라미터 전달
            _report.SetParameterValue("회원명", Parameter["회원명"]);
            _report.SetParameterValue("회원번호", Parameter["회원번호"]);
            _report.SetParameterValue("주문일자", Parameter["주문일자"]);
            _report.SetParameterValue("주문번호", Parameter["주문번호"]);
            _report.SetParameterValue("공제번호", Parameter["공제번호"]);
            _report.SetParameterValue("수령방법", Parameter["수령방법"]);
            _report.SetParameterValue("주문유형", Parameter["주문유형"]);
            _report.SetParameterValue("연락처", Parameter["연락처"]);
            _report.SetParameterValue("운송장번호", Parameter["운송장번호"]);
            _report.SetParameterValue("주소", Parameter["주소"]);
            _report.SetParameterValue("신용카드합산", Parameter["신용카드합산"]);
            _report.SetParameterValue("가상계좌합산", Parameter["가상계좌합산"]);
            _report.SetParameterValue("현금합산", Parameter["현금합산"]);
            _report.SetParameterValue("받는사람", Parameter["받는사람"]);
            _report.SetParameterValue("배송료", Parameter["배송료"]);
            _report.SetParameterValue("PV", Parameter["PV"]);
            _report.SetParameterValue("BV", Parameter["BV"]);
            _report.SetParameterValue("총입금액", Parameter["총입금액"]);
            _report.SetParameterValue("세금", Parameter["세금"]);
            _report.SetParameterValue("총입금액세금차액", Parameter["총입금액세금차액"]);
            ////Binding
            _report.RegisterData(BindingDataTables["Products"], "Products");
            _report.GetDataSource("Products").Enabled = true;
            
            //Show 
            _report.Prepare();
            _report.ShowPrepared();

            this.ShowDialog();

            //==========================================================
            ////다이얼로그 띄우지 않음 
            //_report.PrintSettings.ShowDialog = false;

            ////preview에게 print요청
            //preview1.Print();

            ////해제
            //preview1.Dispose();

            ////폼 닫기 
            //this.Close();
            //==========================================================
        }

        private void Print_MembershipCard_Report()
        {
            //Report 객체 재생성
            _report = new Report();

            //Preview 할당 
            _report.Preview = preview1;

            //레포트 로드
            _report.Load(ReportFolder + this._ReportName);

            ////Binding
            _report.RegisterData(BindingDataTables["Memberinfo"], "minfo");
            _report.GetDataSource("minfo").Enabled = true;

            //Show 
            _report.Prepare();
            _report.ShowPrepared();

            this.ShowDialog();

        }

        private void MakeReportToLocal()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(" SELECT *");
                sb.AppendLine(" FROM tbl_ReportImageFiles");
                sb.AppendLine(" WHERE RFILENAME = '" + this._ReportName + "' ");

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(sb.ToString(), "ReportImageFile", ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (Temp_Connect.DataSet_ReCount.Equals(0)) return;

                DataTable dt = ds.Tables[0];
                byte[] byteArray = (byte[])dt.Rows[0]["IMAGE"];
                string sdate = dt.Rows[0]["SDATE"].ToString();
                string stime = dt.Rows[0]["STIME"].ToString();


                //디비에 입력된 파일의 시간을 DateTime 형식으로 변경
                DateTime dbInputdateTime = DateTime.ParseExact(sdate + " " + stime, "yyyy-MM-dd HH:mm:ss", null);
                if (dt.Rows[0]["RIMAGE"] != DBNull.Value)
                {
                    //시간비교...
                    string strPath = ReportFolder + this._ReportName;
                    if (File.Exists(strPath))
                    {
                        //파일 존재
                        FileInfo fileInfo = new FileInfo(strPath);

                        string[] data = new string[4];
                        data[0] = fileInfo.Name;
                        data[1] = fileInfo.FullName;
                        //LastWriteTime으로 변경
                        data[2] = fileInfo.LastWriteTime.ToString("yyyy-MM-dd");
                        data[3] = fileInfo.LastWriteTime.ToString("HH:mm:ss");

                        //디비에 입력된 파일의 시간이 큰 경우만 로컬에 쓴다. 
                        if (fileInfo.LastWriteTime < dbInputdateTime)
                        {
                            FileStream fileStream = new FileStream(strPath, FileMode.Create);
                            fileStream.Write(byteArray, 0, byteArray.Length);
                            fileStream.Close();
                        }
                    }
                    else
                    {
                        //파일 없으면 바로쓰기 
                        FileStream fileStream = new FileStream(strPath, FileMode.Create);
                        fileStream.Write(byteArray, 0, byteArray.Length);
                        fileStream.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

       
    }
}
