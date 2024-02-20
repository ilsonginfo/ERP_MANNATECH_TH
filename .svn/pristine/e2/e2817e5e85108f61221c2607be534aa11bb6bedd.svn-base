using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.DataAccess;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

using WinForm = System.Windows.Forms;

using System.Data;

namespace MLM_Program.Class
{
    /*class DevColumn 내새끼 언젠간 쓸모가있겠지
    {
        /// <summary> 컬럼의 인덱스 </summary>
        public int Index = -1;
        /// <summary> 컬럼명 </summary>
        public string Name = string.Empty;
        /// <summary> 헤더 이름 </summary>
        public string HeaderText = string.Empty;
        /// <summary> 컬럼 넓이</summary>
        public int Witdh = 100;
        /// <summary> 컬럼 높이</summary>
        public int Height = 35;
        /// <summary> Sort Mode </summary>
        public ColumnSortMode SortMode = ColumnSortMode.Default;
        /// <summary> Custom BackColor1 를 사용할것인가?</summary>
        public bool UseBackColor1 = false;
        /// <summary> Custom BackColor1 , UseBackColor1 값이 True 여야지만 사용가능 </summary>
        public System.Drawing.Color BackColor1;
        /// <summary> Custom BackColor2 를 사용할것인가?</summary>
        public bool UseBackColor2 = false;
        /// <summary> Custom BackColor1 , UseBackColor1 값이 True 여야지만 사용가능 </summary>
        public System.Drawing.Color BackColor2;
    }*/

    public class DevGridControlService :IDisposable
    {
        /// <summary> MS SQL Connection IP </summary>
        public static string MSSQL_IP = string.Empty;
        /// <summary> MS SQL Connection PORT (Default : 10240) </summary>
        public static string MSSQL_PORT = "10240";
        /// <summary> MS SQL Connection Full Name </summary>
        public static string MSSQL_IP_CONNECT_ADDR
        {
            get { return MSSQL_IP + "," + MSSQL_PORT; }
        }
        /// <summary> MS SQL Connection ID </summary>
        public static string MSSQL_ID = string.Empty;
        /// <summary> MS SQL Connection PWD </summary>
        public static string MSSQL_PWD = string.Empty;
        /// <summary> MS SQL Connection DataBase Name </summary>
        public static string MSSQL_DBName = string.Empty;

        ////List<DevColumn> devColumns = new List<DevColumn>(); //언젠간 쓸모가있겠지 

        /* 기존 cls_Grid_Base 에 있는 정보를 이용해 커스텀해줍니다 */
        public int grid_col_Count;
        public int[] grid_col_w;
        public int[] grid_col_h;
        public Boolean[] grid_col_Lock;
        public string[] grid_col_header_text;
        public string[] grid_col_name;
        public WinForm.DataGridViewContentAlignment[] grid_col_alignment;
        public WinForm.DataGridViewColumnSortMode[] grid_col_SortMode;
        public Dictionary<int, string[]> grid_name;
        public Dictionary<int, object[]> grid_name_obj;
        public Dictionary<int, string> grid_cell_format;
        //public WinForm.DataGridView basegrid;
        public GridControl basegrid;
        public GridView baseview;
        public WinForm.DataGridViewSelectionMode grid_select_mod;
        public Boolean grid_Merge;
        public WinForm.DataGridViewAutoSizeColumnsMode grid_Auto_Size_Mod;
        public System.Drawing.Color[] gric_col_Color;
        public int grid_Merge_Col_Start_index;
        public int grid_Merge_Col_End_index;
        public int grid_Frozen_End_Count;
        public int RowTemplate_Height;
        /* 기존 cls_Grid_Base 에 있는 정보를 이용해 커스텀해줍니다 */


        private SqlDataSource GetSqlDataSource(string Query)
        {
            MsSqlConnectionParameters connectionParameters = 
                new MsSqlConnectionParameters(MSSQL_IP_CONNECT_ADDR
                , MSSQL_DBName
                , MSSQL_ID
                , MSSQL_PWD
                , MsSqlAuthorizationType.SqlServer);

            SqlDataSource ds = new SqlDataSource(connectionParameters);

            CustomSqlQuery query = new CustomSqlQuery();
            query.Name = "customQuery";
            query.Sql = Query;
            ds.Queries.Add(query);
            ds.Fill();
            return ds;
        }


        public void FillGrid(DataTable  dt, bool OldVer = true)
        {
            for (int x = 0; x < baseview.RowCount; x++)
            {
                baseview.DeleteRow(x);
            }
            baseview.Columns.Clear();
               

            basegrid.DataSource = dt;

            if(OldVer)
            {
                SetColStyle_Old();
            }

        }


        private void SetColStyle_Old()
        {
            basegrid.LookAndFeel.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.Office2010Blue);
            cls_form_Meth cm = new cls_form_Meth();
            for (int i = 0; i < this.grid_col_Count; i++)
            {
                var Col = baseview.Columns[i];

                //Column Name 지정
                if (grid_col_name != null)
                {
                    Col.Name = grid_col_name[i];
                }

                //Column Caption 지정 
                if (grid_col_header_text != null)
                {
                    Col.Caption = cm._chang_base_caption_search(grid_col_header_text[i]);
                }

                //고정 설정
                if (grid_Frozen_End_Count != 0 && i < grid_Frozen_End_Count)
                {
                    Col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                }

                //SortMode 솔직히 이게 왜 있어야하는진 잘모르겟다
                if (grid_col_SortMode != null)
                {
                    switch (grid_col_SortMode[i])
                    {
                        case WinForm.DataGridViewColumnSortMode.NotSortable:
                            Col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                            break;
                    }
                }

                //BackColor 설정
                if (gric_col_Color != null)
                {
                    Col.AppearanceCell.BackColor = gric_col_Color[i];
                }

                if (grid_col_w != null)
                {
                    if (grid_col_w[i].Equals(0))
                        Col.Visible = false;
                    else
                    {
                        Col.Width = grid_col_w[i];
                        Col.Visible = true;
                    }
                }

                if (grid_col_Lock != null)
                {
                    Col.OptionsColumn.ReadOnly = grid_col_Lock[i];
                }

                if (grid_col_alignment != null)
                {

                    DevExpress.Utils.HorzAlignment HA = DevExpress.Utils.HorzAlignment.Default;
                    DevExpress.Utils.VertAlignment VA = DevExpress.Utils.VertAlignment.Default;
                    switch (grid_col_alignment[i])
                    {
                        case WinForm.DataGridViewContentAlignment.BottomCenter:
                        case WinForm.DataGridViewContentAlignment.BottomLeft:
                        case WinForm.DataGridViewContentAlignment.BottomRight:
                            VA = DevExpress.Utils.VertAlignment.Bottom;
                            break;
                        case WinForm.DataGridViewContentAlignment.MiddleCenter:
                        case WinForm.DataGridViewContentAlignment.MiddleLeft:
                        case WinForm.DataGridViewContentAlignment.MiddleRight:
                            VA = DevExpress.Utils.VertAlignment.Center;
                            break;
                        case WinForm.DataGridViewContentAlignment.TopCenter:
                        case WinForm.DataGridViewContentAlignment.TopLeft:
                        case WinForm.DataGridViewContentAlignment.TopRight:
                            VA = DevExpress.Utils.VertAlignment.Top;
                            break;
                    }
                    switch (grid_col_alignment[i])
                    {
                        case WinForm.DataGridViewContentAlignment.BottomCenter:
                        case WinForm.DataGridViewContentAlignment.MiddleCenter:
                        case WinForm.DataGridViewContentAlignment.TopCenter:
                            HA = DevExpress.Utils.HorzAlignment.Center;
                            break;
                        case WinForm.DataGridViewContentAlignment.BottomLeft:
                        case WinForm.DataGridViewContentAlignment.MiddleLeft:
                        case WinForm.DataGridViewContentAlignment.TopLeft:
                            HA = DevExpress.Utils.HorzAlignment.Near;
                            break;
                        case WinForm.DataGridViewContentAlignment.BottomRight:
                        case WinForm.DataGridViewContentAlignment.MiddleRight:
                        case WinForm.DataGridViewContentAlignment.TopRight:
                            HA = DevExpress.Utils.HorzAlignment.Far;
                            break;
                    }

                    Col.AppearanceCell.TextOptions.HAlignment = HA;
                    Col.AppearanceCell.TextOptions.VAlignment = VA;
                }

            }

            if (grid_cell_format != null)
            {
                foreach (int t_for_key in grid_cell_format.Keys)
                {
                    if (grid_cell_format[t_for_key].Equals(cls_app_static_var.str_Grid_Currency_Type))
                    {
                        baseview.Columns[t_for_key].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    }

                    baseview.Columns[t_for_key].DisplayFormat.FormatString = "###,###,###";

                }
            }

            baseview.OptionsDetail.AllowZoomDetail = true;

            baseview.OptionsBehavior.AllowPixelScrolling = DevExpress.Utils.DefaultBoolean.True;
            baseview.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            baseview.OptionsView.ColumnAutoWidth = false;
        }
        /// <summary> 구소스 참조</summary>
        public void d_Grid_view_Header_Reset(int Start_TF = 0)
        {
            basegrid.DataSource = null;

            for (int x = 0; x < baseview.RowCount; x++)
            {
                baseview.DeleteRow(x);
            }
            baseview.Columns.Clear();
            basegrid.KeyDown += Basegrid_KeyDown;
            //devColumns.Clear();
            cls_form_Meth cm = new cls_form_Meth();


            for (int i = 0; i < this.grid_col_Count; i++)
            {
                var Col = baseview.Columns.Add();

                //Column Name 지정
                if (grid_col_name != null)
                {
                    Col.Name = grid_col_name[i];
                }

                //Column Caption 지정 
                if (grid_col_header_text != null)
                {
                    Col.Caption = cm._chang_base_caption_search(grid_col_header_text[i]);
                }

                //고정 설정
                if (grid_Frozen_End_Count != 0 && i < grid_Frozen_End_Count)
                {
                    Col.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                }

                //SortMode 솔직히 이게 왜 있어야하는진 잘모르겟다
                if (grid_col_SortMode != null)
                {
                    switch (grid_col_SortMode[i])
                    {
                        case WinForm.DataGridViewColumnSortMode.NotSortable:
                            Col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                            break;
                    }
                }

                //BackColor 설정
                if (gric_col_Color != null)
                {
                    Col.AppearanceCell.BackColor = gric_col_Color[i];
                }

                if (grid_col_w != null)
                {
                    if (grid_col_w[i].Equals(0))
                        Col.Visible = false;
                    else
                    {
                        Col.Width = grid_col_w[i];
                        Col.Visible = true;
                    }
                }

                if (grid_col_Lock != null)
                {
                    Col.OptionsColumn.ReadOnly = grid_col_Lock[i];
                }

                if (grid_col_alignment != null)
                {
                   
                    DevExpress.Utils.HorzAlignment HA = DevExpress.Utils.HorzAlignment.Default;
                     DevExpress.Utils.VertAlignment VA = DevExpress.Utils.VertAlignment.Default;
                    switch (grid_col_alignment[i])
                    {
                        case WinForm.DataGridViewContentAlignment.BottomCenter:
                        case WinForm.DataGridViewContentAlignment.BottomLeft:
                        case WinForm.DataGridViewContentAlignment.BottomRight: VA = DevExpress.Utils.VertAlignment.Bottom;
                            break;
                        case WinForm.DataGridViewContentAlignment.MiddleCenter:
                        case WinForm.DataGridViewContentAlignment.MiddleLeft:
                        case WinForm.DataGridViewContentAlignment.MiddleRight: VA = DevExpress.Utils.VertAlignment.Center;
                            break;
                        case WinForm.DataGridViewContentAlignment.TopCenter:
                        case WinForm.DataGridViewContentAlignment.TopLeft:
                        case WinForm.DataGridViewContentAlignment.TopRight: VA = DevExpress.Utils.VertAlignment.Top;
                            break;
                    }
                    switch (grid_col_alignment[i])
                    {
                        case WinForm.DataGridViewContentAlignment.BottomCenter:
                        case WinForm.DataGridViewContentAlignment.MiddleCenter:
                        case WinForm.DataGridViewContentAlignment.TopCenter: HA = DevExpress.Utils.HorzAlignment.Center;
                            break;
                        case WinForm.DataGridViewContentAlignment.BottomLeft:
                        case WinForm.DataGridViewContentAlignment.MiddleLeft:
                        case WinForm.DataGridViewContentAlignment.TopLeft: HA = DevExpress.Utils.HorzAlignment.Near;
                            break;
                        case WinForm.DataGridViewContentAlignment.BottomRight:
                        case WinForm.DataGridViewContentAlignment.MiddleRight:
                        case WinForm.DataGridViewContentAlignment.TopRight: HA = DevExpress.Utils.HorzAlignment.Far;
                            break;
                    }

                    Col.AppearanceCell.TextOptions.HAlignment = HA;
                    Col.AppearanceCell.TextOptions.VAlignment = VA;
                }

            }

            ////연구대상1호 * 머지관련해서 점검해봐야해.
            //if (grid_Merge == true)
            //{
            //    this.basegrid.Paint += new PaintEventHandler(dGridView_Base_Paint);
            //    this.basegrid.Scroll += new ScrollEventHandler(dGridView_Base_Scroll);
            //}


            ////연구대상2호 * 모든 컬럼의 사이즈를 자동조절할것인가?
            //if (grid_Auto_Size_Mod != 0)
            //{
            //    basegrid.AutoSizeColumnsMode = grid_Auto_Size_Mod;
            //}


           
            if (grid_cell_format != null)
            {
                foreach (int t_for_key in grid_cell_format.Keys)
                {
                    if(grid_cell_format[t_for_key].Equals(cls_app_static_var.str_Grid_Currency_Type))
                    {
                        //baseview.Columns[t_for_key].DisplayFormat.FormatString = "d";
                        baseview.Columns[t_for_key].DisplayFormat.FormatString = "###,###,###";
                        baseview.Columns[t_for_key].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    }
                }
            }

            baseview.OptionsDetail.AllowZoomDetail = true;

            baseview.OptionsBehavior.AllowPixelScrolling = DevExpress.Utils.DefaultBoolean.True;
            baseview.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            baseview.OptionsView.ColumnAutoWidth = false;

            baseview.AddNewRow();
            //basegrid.AllowUserToAddRows = false;

            //basegrid.Visible = true;
            //basegrid.Refresh();
            //   System.Globalization.NumberStyles.AllowThousands.ToString ()   ;
        }

        private void Basegrid_KeyDown(object sender, WinForm.KeyEventArgs e)
        {
            if (e.KeyValue == 46)
            {
                e.Handled = true;
            }
        }

        /// <summary> 구소스에 있어서 있는 Func, 딱히 의미는 없다 나중에 다 제거해야한다. </summary>
        public void db_grid_Obj_Data_Put()
        {
            foreach (int t_key in grid_name_obj.Keys)
            {
                ////basegrid.Add(grid_name_obj[t_key]);
            }
            //basegrid.AllowUserToAddRows = false;
        }



        public void ExportExcel(string Text = "엑셀")
        {
            if (baseview.RowCount == 0) return;

            WinForm.SaveFileDialog saveFileDialog1 = new WinForm.SaveFileDialog();
            saveFileDialog1.DefaultExt = "xlsx";
            saveFileDialog1.Filter = "Excel File (*.xlsx)|*.xlsx";
            saveFileDialog1.FileName = Text + "_" + DateTime.Now.ToShortDateString();

            if (saveFileDialog1.ShowDialog() == WinForm.DialogResult.OK)
            {
                baseview.ExportToXlsx(saveFileDialog1.FileName);

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                string Tsql = "";
                Tsql = "Insert Into tbl_Excel_User Values ( ";
                Tsql = Tsql + "'" + cls_User.gid + "',Convert(Varchar(25),GetDate(),21),";
                Tsql = Tsql + "'" + saveFileDialog1.FileName + "',";
                Tsql = Tsql + "'') ";

                if (Temp_Connect.Insert_Data(Tsql, "tbl_Excel_User") == false) return;

                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.

                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~DevGridControlService() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
