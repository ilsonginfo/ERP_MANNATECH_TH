using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Office.Interop;

namespace MLM_Program
{
    public partial class frmClose_4_Select_04 : clsForm_Extends
    {
        

          cls_Grid_Base cgb = new cls_Grid_Base();
        private int Data_Set_Form_TF = 0;
        private string base_db_name = "tbl_CloseTotal_04";
        private const string m_AllowanceTableName = "##TBL_ALLOWANCE_RAW_DATA";
        private string FromEndDate = ""; private string ToEndDate = ""; private string PayDate = "", PayDate2  = "" ;
        private int From_Load_TF = 0;
        private int Cl_F_TF = 0, ReCnt = 0 ;
        private int MaxLevel = 0, Kor_Pay = 0 ;

        private int Chang_Date_Close_Ver02 = 20200101;
        

        Dictionary<string, cls_Close_Mem> Clo_Mem = new Dictionary<string, cls_Close_Mem>();
        Dictionary<string, cls_Close_Sell> Clo_Sell = new Dictionary<string, cls_Close_Sell>();

        cls_Close_Sell[] C_Sell;

        cls_Connect_DB Search_Connect = new cls_Connect_DB();
        SqlConnection Search_Conn = null;

        double Sum_T_PV_001 = 0, Sum_T_PV_01 = 0;

        public frmClose_4_Select_04()
        {
            InitializeComponent();
        }
        
        
     
        

        private void frmBase_From_Load(object sender, EventArgs e)
        {
            //if (this.DesignMode)
            //    return;

           

            Data_Set_Form_TF = 0;
            From_Load_TF = 0;

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            FromEndDate = ""; ToEndDate = ""; PayDate = "";

            Data_Set_Form_TF = 1;
            Data_Set_Form_TF = 0;


            Search_Connect.Connect_DB();
            Search_Conn = Search_Connect.Conn_Conn();

            mtxtPayDate.Mask = cls_app_static_var.Date_Number_Fromat;


            //mtxtFrom.BackColor = cls_app_static_var.txt_Enable_Color;
            //mtxtTo.BackColor = cls_app_static_var.txt_Enable_Color;

            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Pay);
            cfm.button_flat_change(butt_Exit);
        }
        

        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (From_Load_TF == 0)
            {
                From_Load_TF = 1;
                string sRecentFromEndDate = "";
                string sRecentToEndDate = "";
                FromEndDate = Check_Close_Date(ref sRecentFromEndDate, ref sRecentToEndDate);

                if (FromEndDate == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not2_Close_Date"));
                    this.Close();
                    return;
                }

                //mtxtFrom.Text = FromEndDate;
                mtxtFrom.Text = FromEndDate.Substring(0, 4) + '-' + FromEndDate.Substring(4, 2) + '-' + FromEndDate.Substring(6, 2);
                txt_RecentTo.Text = sRecentToEndDate;
                //txt_RecentFrom.Text = sRecentToEndDate.Substring(0, 6) + "01";
                txt_RecentFrom.Text = sRecentFromEndDate;

                DateTime dt = DateTime.Parse(FromEndDate.Substring(0, 4) + "-" + FromEndDate.Substring(4, 2) + "-" + FromEndDate.Substring(6, 2));
                string SDate3 = dt.AddMonths(1).ToShortDateString().Replace("-", "").Substring(0, 6) + "01";

                dt = DateTime.Parse(SDate3.Substring(0, 4) + "-" + SDate3.Substring(4, 2) + "-" + SDate3.Substring(6, 2));
                SDate3 = dt.AddDays(-1).ToShortDateString().Replace("-", "");

                //ToEndDate  = SDate3;
                ToEndDate  = SDate3.Substring(0, 4) + '-' + SDate3.Substring(4, 2) + '-' + SDate3.Substring(6, 2);
                mtxtTo.Text = ToEndDate;

                //Close_Base_Work();

                //string PayDate = "";

                //PayDate = ToEndDate.Substring(0, 4) + '-' + ToEndDate.Substring(4, 2) + '-' + ToEndDate.Substring(6, 2);
                //DateTime TodayDate = new DateTime();
                //TodayDate = DateTime.Parse(PayDate);
                //PayDate = TodayDate.AddDays(20).ToString("yyyy-MM-dd").Replace("-", "");

                ////DateTime dt = DateTime.Parse(FromEndDate.Substring(0, 4) + "-" + FromEndDate.Substring(4, 2) + "-" + FromEndDate.Substring(6, 2));
                ////string SDate3 = dt.AddMonths(1).ToShortDateString().Replace("-", "").Substring(0, 6) + "15";
                ////PayDate = SDate3;

                //mtxtPayDate.Text = PayDate;


                //Base_Sub_Grid_Set(FromEndDate);
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
                    {
                        this.Close();
                        return;
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
                            return;
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
                    butt_Exit_Click(T_bt, ee1);
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


        

        private void butt_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUploadExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Excel File|*.xls;*.xlsx;*.xlsm";
            OFD.Title = "Select an Excel file";

            if (OFD.ShowDialog() == DialogResult.OK)
            {
                string filePath = OFD.FileName;
                tbExcelPath.Text = filePath;
            }
        }

        private string Check_Close_Date(ref string sRecentFromEndDate, ref string sRecentToEndDate)
        {
            string Tsql = "";
            string Max_Toenddate = "";
            //Tsql = "Select Isnull (Max(ToEndDate),'') From  tbl_CloseTotal_04 (nolock) ";
            Tsql = "SELECT TOP 1 ToEndDate, FromEndDate, PayDate, PayDate2 From tbl_CloseTotal_04 WITH(NOLOCK) ORDER BY ToEndDate DESC ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt != 0)
            {
                Max_Toenddate = ds.Tables[base_db_name].Rows[0][0].ToString();
                sRecentToEndDate = Max_Toenddate;
                sRecentFromEndDate = ds.Tables[base_db_name].Rows[0][1].ToString();

                ToEndDate = ds.Tables[base_db_name].Rows[0][0].ToString();
                PayDate = ds.Tables[base_db_name].Rows[0][2].ToString();
                PayDate2 = ds.Tables[base_db_name].Rows[0][3].ToString();
                //if (int.Parse(Max_Toenddate) < 20180101)
                //    Max_Toenddate = "20180701"; 
            }
            
            if (Max_Toenddate != "")
            {
                Max_Toenddate = Max_Toenddate.Substring(0, 4) + '-' + Max_Toenddate.Substring(4, 2) + '-' + Max_Toenddate.Substring(6, 2);
                DateTime TodayDate = new DateTime();
                TodayDate = DateTime.Parse(Max_Toenddate);
                Max_Toenddate = TodayDate.AddDays(1).ToString("yyyy-MM-dd").Replace ("-","") ;                
            }
            else
            {
                ReCnt = 0;
                Tsql = "Select Isnull(Min(SellDate),'')  From   tbl_SalesDetail (nolock) ";

                DataSet ds2 = new DataSet();
                Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds2, this.Name, this.Text);
                ReCnt = Temp_Connect.DataSet_ReCount;
                if (ReCnt != 0)
                {
                    Max_Toenddate = ds2.Tables[base_db_name].Rows[0][0].ToString();

                    DateTime TodayDate = new DateTime();
                    TodayDate = DateTime.Parse(Max_Toenddate);
                    sRecentFromEndDate = TodayDate.AddDays(-1).ToString("yyyy-MM-dd").Replace("-", "");
                }
            }


            return Max_Toenddate ;
        }

        private bool Check_IsValid_Close_Date(string sFromEndDate, string sToEndDate)
        {
            if (DateTime.Compare(Convert.ToDateTime(mtxtFrom.Text), Convert.ToDateTime(mtxtTo.Text)) > 0)
            {
                MessageBox.Show("설정한 작업 예정 기간 범위가 잘못 지정되었습니다. 확인 바랍니다.");
                return false;
            }
            else if (mtxtFrom.Text.Replace("-","").Substring(0, 6) != mtxtTo.Text.Replace("-", "").Substring(0, 6))
            {
                MessageBox.Show("설정한 작업 예정 기간이 한달 내 범위가 아닙니다. 다시 확인 바랍니다.");
                return false;
            }


            string Tsql = "";
            string Max_Toenddate = "";

            Tsql = "SELECT FromEndDate, ToEndDate, PayDate, PayDate2 From tbl_CloseTotal_04 WITH(NOLOCK)" +
                    " WHERE FromEndDate >= '" + sFromEndDate + "' AND ToEndDate <= '" + sToEndDate + "' ORDER BY ToEndDate DESC";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt != 0)
            {
                MessageBox.Show("설정한 작업 예정 기간의 수당 데이터가 존재합니다. 다시 확인하여 주십시오.");
                return false;
            }

            return true;
        }


        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            // SendKeys.Send("{TAB}");
        }




        private Boolean Search_Check_TextBox_Error()
        {


            if (mtxtTo.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                      + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_CloseDate2")
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            }

            if (mtxtPayDate.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                      + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_PayDate")
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtPayDate.Focus(); return false;
            }

            if (mtxtPayDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtPayDate.Text, mtxtPayDate, "Date") == false)
                {
                    mtxtPayDate.Focus();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("지급일자를 확인하여 주십시오.");
                mtxtPayDate.Focus();
                return false;
            }

            



            return true;
        }


        private void butt_Pay_Click(object sender, EventArgs e)
        {
            if (Search_Check_TextBox_Error() == false) return;

            if (Check_IsValid_Close_Date(mtxtFrom.Text, mtxtTo.Text) == false) return;

            string filePath = tbExcelPath.Text;
            string fileExtension = Path.GetExtension(tbExcelPath.Text);

            if (!File.Exists(filePath) || !cls_Excel.IsValidExcelExtension(fileExtension))
            {
                MessageBox.Show("지정한 경로에 Excel file이 존재하지 않습니다. \n다시 확인해 주십시오.", "확인 요망", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                btnUploadExcel.Focus();
                return;
            }

            string messageText = string.Format("작업 예정 내용은 다음과 같습니다.\n수당작업일: {0} ~ {1}\n수당지급일: {2} \n입니다. 작업을 진행하시겠습니까?", mtxtFrom.Text, mtxtTo.Text, mtxtPayDate.Text);
            DialogResult mBoxResult = MessageBox.Show(messageText, "수당 계산기", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (mBoxResult == DialogResult.Yes)
            {
                ImportExcelDataAndCalcAllowance(filePath);
            }

        }

        private void ImportExcelDataAndCalcAllowance(string filePath)
        {
            // SQL Server 연결 문자열
            //string connectionString = "Data Source=YourServer;Initial Catalog=YourDatabase;User ID=YourUsername;Password=YourPassword";

            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            Microsoft.Office.Interop.Excel.Range range = null;
            //SqlConnection connection = null;
            //cls_Connect_DB.Conn_Str = "server=121.78.56.24,10240;database=Promax_Home;user id=ILS_PROMAX_20;password=PMX#x20!ilsong";
            //cls_Connect_DB.Conn_Str = "server=121.78.56.24,10240;database=Promax_Home;user id=sa;password=ilsong#x";

            try
            {
                // Excel 파일 열기
                excelApp = new Microsoft.Office.Interop.Excel.Application();             // Excel Application 인스턴스 생성
                workbook = excelApp.Workbooks.Open(filePath);   // Workbook 열기
                worksheet = workbook.Sheets[1];                 // 첫 번째 Worksheet 선택

                // Worksheet에서 데이터를 읽어 DataTable로 변환
                range = worksheet.UsedRange;
                object[,] data = range.Value;
                DataTable dt = new DataTable();

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                DataSet ds = new DataSet();

                StringBuilder sbTsql = new StringBuilder(512);


                

                // 임시 테이블 생성
                //Connect_DB();

                // 해당 수당 월 데이터가 존재하는지 확인.
                //

                #region CHECK_LOGIC
                sbTsql.Clear();
                sbTsql.Append("SELECT IIF(EXISTS(SELECT * FROM tbl_CloseTotal_04 WITH(NOLOCK) WHERE LEFT(ToEndDate, 6) = '202008'), 'N', 'Y') AS RESULT");

                ExecuteTsql(sbTsql.ToString(), ref Search_Conn);

                // N인 경우 이미 수당 데이터 있어서 불가, Y는 수당 진행 가능을 의미.

                #endregion


                // COLLATE Korean_Wansung_CI_AS -> "Cannot resolve the collation conflict between "SQL_Latin1_General_CP1_CI_AS" and "Korean_Wansung_CI_AS" in the equal to operation."
                // 에러 발생으로 임시 테이블 생성시 해당 기능 추가. varchar Column에만 추가 적용.
                sbTsql.Clear();
                sbTsql.Append("CREATE TABLE " + m_AllowanceTableName + " ( ");
                sbTsql.Append("[Mbid2] [float] NULL, ");
                sbTsql.Append("[Gname] [nvarchar] (255) COLLATE Korean_Wansung_CI_AS NULL, ");
                sbTsql.Append("[ToEndDate] [float] NULL, ");
                sbTsql.Append("[BusinessDevTeam2] [float] NULL, ");
                sbTsql.Append("[BusinessDevTeam4] [float] NULL, ");
                sbTsql.Append("[CheckMatch] [float] NULL, ");
                sbTsql.Append("[FirstOrderBonus] [float] NULL, ");
                sbTsql.Append("[GlobalPool] [float] NULL, ");
                sbTsql.Append("[MentorBonus] [float] NULL, ");
                sbTsql.Append("[SideVolumeInfinity] [float] NULL, ");
                sbTsql.Append("[UniLevel] [float] NULL, ");
                sbTsql.Append("[RankUp] [float] NULL, ");
                sbTsql.Append("[Etc_Pay] [float] NULL, ");
                sbTsql.Append("[Cur_DedCut_Pay] [float] NULL, ");
                sbTsql.Append("[SumAllAllowance] [float] NULL, ");
                sbTsql.Append("[InComeTax] [float] NULL, ");
                sbTsql.Append("[TruePayment] [float] NULL, ");
                sbTsql.Append("[F18] [nvarchar] (255) COLLATE Korean_Wansung_CI_AS NULL, ");
                sbTsql.Append("[F19] [nvarchar] (255) COLLATE Korean_Wansung_CI_AS NULL, ");
                sbTsql.Append("[F20] [nvarchar] (255) COLLATE Korean_Wansung_CI_AS NULL, ");
                sbTsql.Append("[F21] [nvarchar] (255) COLLATE Korean_Wansung_CI_AS NULL, ");
                sbTsql.Append("[F22] [nvarchar] (255) COLLATE Korean_Wansung_CI_AS NULL ");
                sbTsql.Append(") ON [PRIMARY] ");

                ExecuteTsql(sbTsql.ToString(), ref Search_Conn);

                // 열 이름을 DataTable에 추가
                for (int i = 1; i <= range.Columns.Count; i++)
                {
                    dt.Columns.Add(data[1, i].ToString());
                }

                Enumerable.Range(2, range.Rows.Count - 1)
                .Select(row => Enumerable.Range(1, range.Columns.Count)
                    .Select(col => data[row, col])
                    .ToArray())
                .ToList()
                .ForEach(rowArray => dt.Rows.Add(rowArray));

                // 엑셀 데이터 SQL Server로 임시테이블에 복사.
                CopyDataTableToSqlServer(m_AllowanceTableName, dt);

                // 수당 계산 진행
                sbTsql.Clear();
                //sbTsql.Append("EXEC USP_INSERT_ALLOWANCE_DATAS '" + m_FromEndDate + "', '" + m_ToEndDate + "', '" + m_payDate + "'");
                sbTsql.Append("EXEC USP_INSERT_ALLOWANCE_DATAS '" + 
                    mtxtFrom.Text.Replace("-","").Trim() + "', '" + mtxtTo.Text.Replace("-", "").Trim() + "', '" + mtxtPayDate.Text.Replace("-", "").Trim() + "', '" + cls_User.gid + "'"
                    );
                //ExecuteTestTsql(sbTsql.ToString(), ref Conn);     // ROLLBACK TRAN 테스트용, syhuh
                ExecuteTsql(sbTsql.ToString(), ref Search_Conn);

                // 엑셀 데이터 임시 테이블 삭제
                ClearTempTable(m_AllowanceTableName);

                MessageBox.Show("엑셀 Data 업로드 및 수당 계산이 완료되었습니다!");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {

                ClearTempTable(m_AllowanceTableName);   // 임시 테이블 삭제
                //Close_DB();     // 연결 해제

                // 자원 정리
                workbook.Close();
                excelApp.Quit();

                ReleaseObject(worksheet);
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
            }
        }


        /// <summary>
        /// 임시테이블 정리하는 함수, 이때 Connection이 Open 되어 있어야 함.
        /// </summary>
        /// <param name="sTempTableName"></param>
        private void ClearTempTable(string sTempTableName)
        {
            try
            {
                if (Search_Conn.State == ConnectionState.Open)
                {
                    string dropTableQuery = "IF OBJECT_ID('tempdb.." + m_AllowanceTableName + "') IS NOT NULL DROP TABLE " + m_AllowanceTableName;
                    SqlCommand dropTableCommand = new SqlCommand(dropTableQuery, Search_Conn);
                    dropTableCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void frmClose_4_Select_04_FormClosed(object sender, FormClosedEventArgs e)
        {
            Search_Connect.Close_DB();
        }

        private void butt_PayCancel_Click(object sender, EventArgs e)
        {

            string messageText = string.Format("수당 취소 예정 내용은 다음과 같습니다.\n수당취소일: {0} ~ {1} \n취소한 내용은 복구가 불가합니다. 작업을 진행하시겠습니까?", txt_RecentFrom.Text, txt_RecentTo.Text, mtxtPayDate.Text);
            DialogResult mBoxResult = MessageBox.Show(messageText, "수당 계산기", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            StringBuilder sbTsql = new StringBuilder();

            if (mBoxResult == DialogResult.Yes)
            {
                sbTsql.Clear();
                
                sbTsql.Append("EXEC USP_DELETE_ALLOWANCE_DATAS '" +
                    txt_RecentFrom.Text.Replace("-", "").Trim() + "', '" + txt_RecentTo.Text.Replace("-", "").Trim() + "', '" + PayDate + "', '" + PayDate2 + "', '" + cls_User.gid + "'"
                    );

                ExecuteTsql(sbTsql.ToString(), ref Search_Conn);

                MessageBox.Show("취소가 완료되었습니다.");
                Close();
            }

        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private bool ExecuteTsql(string Tsql, ref SqlConnection ref_SqlConnection)
        {
            bool bResult = false;

            SqlTransaction tran = null;
            try
            {
                tran = Search_Conn.BeginTransaction();

                SqlCommand Tcommand = new SqlCommand(Tsql, ref_SqlConnection, tran);
                Tcommand.ExecuteNonQuery();

                tran.Commit();
                bResult = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                tran.Dispose();
                bResult = false;
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                tran.Dispose();
            }

            return bResult;
        }

        /// <summary>
        /// Test용, 쿼리 실행 후 rollback. rollback 전 쿼리 실행 후 ssms에서 실행(데이터) 확인.
        /// </summary>
        /// <param name="ref_Tsql"></param>
        /// <param name="ref_SqlConnection"></param>
        /// <returns></returns>
        private bool ExecuteTestTsql(string ref_Tsql, ref SqlConnection ref_SqlConnection)
        {
            bool bResult = false;

            SqlTransaction tran = null;
            try
            {
                tran = Search_Conn.BeginTransaction();

                SqlCommand Tcommand = new SqlCommand(ref_Tsql, ref_SqlConnection, tran);
                Tcommand.ExecuteNonQuery();

                tran.Rollback();    // SYHUH, 임시
                //tran.Commit();
                bResult = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                tran.Dispose();
                bResult = false;
                throw ex;
            }
            finally
            {
                tran.Dispose();
            }

            return bResult;
        }

        /// <summary>
        /// DataTable 개체의 내용을 SQL SERVER DB에 INSERT 시킴. - syhuh
        /// </summary>
        /// <param name="sTableName">Insert 할 테이블명 (임시테이블도 가능)</param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool CopyDataTableToSqlServer(string sTableName, DataTable dt)
        {
            bool bResult = false;

            SqlTransaction tran = null;
            try
            {
                tran = Search_Conn.BeginTransaction();

                if (Search_Conn.State == ConnectionState.Open)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Search_Conn, SqlBulkCopyOptions.KeepIdentity, tran))
                    {
                        bulkCopy.DestinationTableName = sTableName;
                        bulkCopy.WriteToServer(dt);
                    }

                    bResult = true;
                }
                else
                {
                    bResult = false;
                }

                tran.Commit();
                bResult = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                tran.Dispose();
                bResult = false;
                throw ex;
            }
            finally
            {
                tran.Dispose();
            }

            return bResult;


            //if (Conn.State == ConnectionState.Open)
            //{
            //    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Conn))
            //    {
            //        bulkCopy.DestinationTableName = sTableName;
            //        bulkCopy.WriteToServer(dt);
            //    }

            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

    }
}
