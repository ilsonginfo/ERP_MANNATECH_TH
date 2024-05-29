using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Data;
using System.Drawing;

namespace MLM_Program
{
    class cls_Grid_Base
    {
        public int grid_col_Count;
        public int[] grid_col_w;
        public int[] grid_col_h;
        public Boolean[] grid_col_Lock;
        public string[] grid_col_header_text;
        public string[] grid_col_name;
        public DataGridViewContentAlignment[] grid_col_alignment;
        public DataGridViewColumnSortMode[] grid_col_SortMode;
        public Dictionary<int, string[]> grid_name;
        public Dictionary<int, object[]> grid_name_obj;
        public Dictionary<int, string> grid_cell_format;
        public DataGridView basegrid;
        public DataGridViewSelectionMode grid_select_mod;
        public Boolean grid_Merge;
        public DataGridViewAutoSizeColumnsMode grid_Auto_Size_Mod;
        public Color[] gric_col_Color;
        public int grid_Merge_Col_Start_index;
        public int grid_Merge_Col_End_index;
        public int grid_Frozen_End_Count;
        public int RowTemplate_Height;
        public int Sort_Mod_Auto_TF;


        public void Grid_Base_Arr_Clear()
        {
            grid_col_w = null;
            grid_col_h = null;
            grid_col_Lock = null;
            grid_col_header_text = null;
            grid_col_name = null;
            grid_col_alignment = null;
            grid_col_SortMode = null;
            grid_name = null;
            grid_name_obj = null;
            grid_cell_format = null;
            gric_col_Color = null;


            grid_Merge_Col_Start_index = 0;
            grid_Merge_Col_End_index = 0;
            grid_Frozen_End_Count = 0;
            RowTemplate_Height = 0;
        }

        /*데이터바인딩 속도 빠르게 하기 위해서 새로 그리드뷰 셋팅하는 함수*/
        public void d_Grid_view_DataSource_Header_Reset(DataGridView dgv, string[] HeaderText_Arr, int[] Width_Arr
            , Boolean[] ReadOnly_Arr, DataGridViewContentAlignment[] Align_Arr, int frozenCnt
            , int Autosize_TF = 0)
        {
            cls_form_Meth cm = new cls_form_Meth();

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].HeaderText = cm._chang_base_caption_search(HeaderText_Arr[i]);

                if (Width_Arr[i] == 0)
                    dgv.Columns[i].Visible = false;
                else
                    dgv.Columns[i].Width = Width_Arr[i];


                if (Autosize_TF == 0)
                    dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                //int Col_Width = dgv.Columns[i].Width;
                //dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                //dgv.Columns[i].Width = Col_Width;

                dgv.Columns[i].ReadOnly = ReadOnly_Arr[i];
                dgv.Columns[i].DefaultCellStyle.Alignment = Align_Arr[i];
            }

            dgv.CellPainting += new DataGridViewCellPaintingEventHandler(dGridView_Base_CellPainting);

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            dgv.RowHeadersDefaultCellStyle.SelectionBackColor = cls_app_static_var.Button_Parent_Color;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.DefaultCellStyle.Font = new System.Drawing.Font("굴림", float.Parse("8.4"));
            dgv.RowTemplate.Height = 20;
            dgv.ColumnHeadersHeight = 22;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgv.ShowCellToolTips = false;
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            if (dgv.Columns.Count >= 1)
            {
                for (int Cnt = 0; Cnt < frozenCnt; Cnt++)
                {
                    dgv.Columns[Cnt].Frozen = true;
                }
            }
        }


        public void d_Grid_view_Header_Reset()
        {
            //basegrid.Visible = false;
            basegrid.Rows.Clear();
            basegrid.ColumnCount = grid_col_Count;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = grid_select_mod;

            basegrid.RowTemplate.Height = 20;
            if (RowTemplate_Height > 0)
                basegrid.RowTemplate.Height = RowTemplate_Height;


            basegrid.DefaultCellStyle.Font = new System.Drawing.Font("돋움", float.Parse("8.4"));
            basegrid.ColumnHeadersHeight = 22;

            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            //basegrid.EnableHeadersVisualStyles = false;
            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            //basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(208, 222, 176);


            basegrid.BorderStyle = BorderStyle.FixedSingle;
            basegrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            basegrid.RowHeadersDefaultCellStyle.SelectionBackColor = cls_app_static_var.Button_Parent_Color;
            basegrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            basegrid.ColumnHeadersHeight = 20;

            basegrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            basegrid.KeyDown += new KeyEventHandler(dGridView_KeyDown);
            //basegrid.SortCompare += new DataGridViewSortCompareEventHandler(basegrid_SortCompare);


            if (grid_Frozen_End_Count > 0)
            {
                for (int Cnt = 0; Cnt < grid_Frozen_End_Count; Cnt++)
                {
                    basegrid.Columns[Cnt].Frozen = true;
                }
            }

            if (basegrid.RowHeadersVisible == true)
                basegrid.CellPainting += new DataGridViewCellPaintingEventHandler(dGridView_Base_CellPainting);

            if (grid_Merge == true)
            {
                this.basegrid.Paint += new PaintEventHandler(dGridView_Base_Paint);

                this.basegrid.Scroll += new ScrollEventHandler(dGridView_Base_Scroll);
            }



            int i = 0;
            cls_form_Meth cm = new cls_form_Meth();

            if (grid_col_header_text != null)
            {
                foreach (string t_ext in grid_col_header_text)
                {
                    basegrid.Columns[i].HeaderText = cm._chang_base_caption_search(t_ext);
                    //basegrid.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    if (Sort_Mod_Auto_TF != 0)
                        basegrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    i++;
                }
            }

            i = 0;
            if (grid_col_name != null)
            {
                foreach (string t_ext in grid_col_name)
                {
                    basegrid.Columns[i].Name = t_ext;

                    if (Sort_Mod_Auto_TF != 0)
                        basegrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    i++;
                }
            }

            i = 0;
            if (grid_col_SortMode != null)
            {
                foreach (DataGridViewColumnSortMode t_wi in grid_col_SortMode)
                {
                    basegrid.Columns[i].SortMode = t_wi;
                    i++;
                }
            }

            i = 0;
            if (gric_col_Color != null)
            {
                foreach (Color t_color in gric_col_Color)
                {
                    basegrid.Columns[i].DefaultCellStyle.BackColor = t_color;
                    i++;
                }
            }



            if (grid_Auto_Size_Mod != 0)
                basegrid.AutoSizeColumnsMode = grid_Auto_Size_Mod;

            //foreach (DataGridViewHeaderCell header in basegrid.Rows)
            //{
            //    header.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //}



            i = 0;
            if (grid_col_w != null)
            {
                foreach (int t_wi in grid_col_w)
                {
                    basegrid.Columns[i].Width = t_wi;

                    if (t_wi == 0) basegrid.Columns[i].Visible = false;
                    i++;
                }
            }

            i = 0;
            if (grid_col_Lock != null)
            {
                foreach (Boolean t_ro in grid_col_Lock)
                {
                    basegrid.Columns[i].ReadOnly = t_ro;
                    i++;
                }
            }



            i = 0;
            if (grid_col_alignment != null)
            {
                foreach (DataGridViewContentAlignment t_ai in grid_col_alignment)
                {
                    basegrid.Columns[i].DefaultCellStyle.Alignment = t_ai;
                    i++;
                }
            }

            i = 0;
            if (grid_cell_format != null)
            {
                foreach (int t_for_key in grid_cell_format.Keys)
                {
                    basegrid.Columns[t_for_key].DefaultCellStyle.Format = grid_cell_format[t_for_key];
                    i++;
                }
            }


            basegrid.AllowUserToAddRows = false;

            //basegrid.Visible = true;
            //basegrid.Refresh();
            //   System.Globalization.NumberStyles.AllowThousands.ToString ()   ;
        }


        public void d_Grid_view_Header_Reset(int Start_F)
        {
            // basegrid.Visible = false;
            basegrid.Rows.Clear();
            basegrid.ColumnCount = grid_col_Count;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = grid_select_mod;
            basegrid.RowTemplate.Height = 20;

            basegrid.DefaultCellStyle.Font = new System.Drawing.Font("돋움", float.Parse("8.4"));


            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.BorderStyle = BorderStyle.FixedSingle;
            basegrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;


            //basegrid.EnableHeadersVisualStyles = false;
            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            basegrid.RowHeadersDefaultCellStyle.SelectionBackColor = cls_app_static_var.Button_Parent_Color;
            basegrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            //basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(208, 222, 176);
            //basegrid.BorderStyle = BorderStyle.Fixed3D;
            //basegrid.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            //basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            basegrid.ColumnHeadersHeight = 20;

            basegrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;

            basegrid.KeyDown += new KeyEventHandler(dGridView_KeyDown);
            //basegrid.SortCompare += new DataGridViewSortCompareEventHandler(basegrid_SortCompare);

            if (grid_Frozen_End_Count > 0)
            {
                for (int Cnt = 0; Cnt < grid_Frozen_End_Count; Cnt++)
                {
                    basegrid.Columns[Cnt].Frozen = true;
                }
            }


            int i = 0;
            cls_form_Meth cm = new cls_form_Meth();

            if (grid_col_header_text != null)
            {
                foreach (string t_ext in grid_col_header_text)
                {
                    basegrid.Columns[i].HeaderText = cm._chang_base_caption_search(t_ext);
                    //basegrid.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                    if (Sort_Mod_Auto_TF != 0)
                        basegrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //basegrid.Columns[i].a  DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; 
                    i++;
                }

            }

            i = 0;
            if (grid_col_name != null)
            {
                foreach (string t_ext in grid_col_name)
                {
                    basegrid.Columns[i].Name = t_ext;

                    if (Sort_Mod_Auto_TF != 0)
                        basegrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    i++;
                }
            }

            i = 0;
            if (grid_col_SortMode != null)
            {
                foreach (DataGridViewColumnSortMode t_wi in grid_col_SortMode)
                {
                    basegrid.Columns[i].SortMode = t_wi;
                    i++;
                }
            }

            if (grid_Auto_Size_Mod != 0)
                basegrid.AutoSizeColumnsMode = grid_Auto_Size_Mod;

            i = 0;
            if (grid_col_w != null)
            {
                foreach (int t_wi in grid_col_w)
                {
                    basegrid.Columns[i].Width = t_wi;

                    if (t_wi == 0) basegrid.Columns[i].Visible = false;
                    i++;
                }
            }

            i = 0;
            if (gric_col_Color != null)
            {
                foreach (Color t_color in gric_col_Color)
                {
                    basegrid.Columns[i].DefaultCellStyle.BackColor = t_color;
                    i++;
                }
            }


            i = 0;
            if (grid_col_Lock != null)
            {
                foreach (Boolean t_ro in grid_col_Lock)
                {
                    basegrid.Columns[i].ReadOnly = t_ro;
                    i++;
                }
            }



            i = 0;
            if (grid_col_alignment != null)
            {
                foreach (DataGridViewContentAlignment t_ai in grid_col_alignment)
                {
                    basegrid.Columns[i].DefaultCellStyle.Alignment = t_ai;
                    i++;
                }
            }

            i = 0;
            if (grid_cell_format != null)
            {
                foreach (int t_for_key in grid_cell_format.Keys)
                {
                    basegrid.Columns[t_for_key].DefaultCellStyle.Format = grid_cell_format[t_for_key];
                    i++;
                }
            }


            basegrid.AllowUserToAddRows = false;
            // basegrid.Visible = true;
            // basegrid.Refresh();
        }



        void basegrid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            try
            {
                // Try to sort based on the cells in the current column.
                e.SortResult = System.String.Compare(
                    e.CellValue1.ToString().Replace(",", ""), e.CellValue2.ToString().Replace(",", ""));

                // If the cells are equal, sort based on the ID column.
                if (e.SortResult == 0 && e.Column.Name != "ID")
                {
                    e.SortResult = System.String.Compare(
                        (sender as DataGridView).Rows[e.RowIndex1].Cells["ID"].Value.ToString(),
                        (sender as DataGridView).Rows[e.RowIndex2].Cells["ID"].Value.ToString());
                }
                e.Handled = true;



            }
            catch (Exception ec)
            {

            }
        }



        public void db_grid_Data_Put()
        {
            //basegrid.Visible = false;
            foreach (int t_key in grid_name.Keys)
            {
                basegrid.Rows.Add(grid_name[t_key]);
            }

            basegrid.AllowUserToAddRows = false;
            //basegrid.Visible = true;
            basegrid.Refresh();
        }

        public void db_grid_Obj_Data_Put()
        {
            //basegrid.Visible = false;
            foreach (int t_key in grid_name_obj.Keys)
            {
                basegrid.Rows.Add(grid_name_obj[t_key]);
            }
            basegrid.AllowUserToAddRows = false;
            //basegrid.Visible = true;
            // basegrid.Refresh();
        }




        private void dGridView_Base_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            //Header인지 확인
            if (e.ColumnIndex < 0 & e.RowIndex >= 0)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

                //행 번호를 표시할 범위를 결정
                System.Drawing.Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);
                //행번호를 표시
                TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                                      e.CellStyle.Font, indexRect, e.CellStyle.ForeColor,
                                      TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }


        // 셀병합과 관련해서  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>   
        private void dGridView_Base_Scroll(object sender, ScrollEventArgs e)
        {
            System.Drawing.Rectangle rtHeader = this.basegrid.DisplayRectangle;
            this.basegrid.Invalidate(rtHeader, true);
        }


        private void dGridView_Base_Paint(object sender, PaintEventArgs e)
        {
            if (basegrid.RowCount == 0) return;
            Chang_Cell_Merge_001(sender, e);
        }


        private void Chang_Cell_Merge_001(object sender, PaintEventArgs e)
        {

            int Call_Cnt = 0;



            for (int ColCnt = grid_Merge_Col_Start_index; ColCnt <= grid_Merge_Col_End_index; ColCnt++)
            {
                int Strar_Row = 0; int End_Row = 0; string Mer_String = "";
                string Cur_string = ""; int Meg_Cnt = 0; int send_End_Row = 0;

                Mer_String = basegrid.Rows[0].Cells[ColCnt].Value.ToString();
                Cur_string = basegrid.Rows[0].Cells[ColCnt].Value.ToString();
                Call_Cnt = 0;

                for (int j = 0; j < this.basegrid.RowCount; j++)
                {
                    Cur_string = basegrid.Rows[j].Cells[ColCnt].Value.ToString();



                    if (Mer_String == Cur_string)
                        Meg_Cnt++;
                    else
                    {

                        End_Row = j - 1;
                        if (Meg_Cnt > 1)
                            Call_Cnt++;
                        Chang_Cell_Merge_002(sender, e, ColCnt, Strar_Row, Strar_Row + Meg_Cnt, Mer_String, Meg_Cnt, Call_Cnt);

                        Mer_String = Cur_string;
                        End_Row = 0;
                        Strar_Row = j;
                        Meg_Cnt = 0;
                    }
                }

                if (Meg_Cnt > 1)
                    Call_Cnt++;

                send_End_Row = Meg_Cnt;
                if (basegrid.RowCount <= send_End_Row)
                    send_End_Row = (basegrid.RowCount - 1);

                Chang_Cell_Merge_002(sender, e, ColCnt, Strar_Row, Strar_Row + send_End_Row, Mer_String, Meg_Cnt, Call_Cnt);
            }
        }


        private void Chang_Cell_Merge_002(object sender, PaintEventArgs e, int ColCnt, int Strar_Row, int End_Row, string Mer_String, int Meg_Cnt, int Call_Cnt)
        {
            if (basegrid.RowCount <= 1) return;

            //try
            //{
            DataGridView dgv = (DataGridView)sender;

            System.Drawing.Rectangle r1 = this.basegrid.GetCellDisplayRectangle(ColCnt, Strar_Row, true);

            System.Drawing.Rectangle r2 = this.basegrid.GetCellDisplayRectangle(ColCnt, End_Row, true);

            //int H2 = this.basegrid.GetCellDisplayRectangle(ColCnt, End_Row, true).Height * (End_Row - Strar_Row + 1);

            if ((r1.Y <= 0) && (r2.Y <= 0)) return;
            if (r1.X <= 0) r1.X = r2.X;
            if (r1.Width <= 0) r1.Width = r2.Width;

            r1.X += 1;
            r1.Y += 1;
            r1.Width = r1.Width - 2;




            if (Strar_Row == 0)
                if (r2.Y == 0)
                    r1.Height = dgv.Height - r1.Y - r1.Height - 2;
                else
                    if ((End_Row + 1) == basegrid.RowCount)
                {
                    r1.Height = (r2.Y + r2.Height) - r1.Y - 2;
                }
                else
                {
                    if (r1.Y <= (1))
                    {
                        r1.Y = r2.Height;
                        r1.Height = (r2.Y + r2.Height) - r2.Height - r2.Height - 2;
                    }
                    else
                        r1.Height = (r2.Y + r2.Height) - r1.Y - r1.Height - 2;
                }
            else
                if (r2.Y == 0)
                r1.Height = dgv.Height - r1.Y - 2;
            else
                    if (r1.Y <= (1))
            {
                r1.Y = this.basegrid.ColumnHeadersHeight + 1;
                r1.Height = (r2.Y + r2.Height) - this.basegrid.ColumnHeadersHeight - 2;
            }
            else
                r1.Height = (r2.Y + r2.Height) - r1.Y - 2;

            //r1.

            //e.Graphics.FillRectangle(new System.Drawing.SolidBrush(this.basegrid.ColumnHeadersDefaultCellStyle.BackColor), r1);
            e.Graphics.FillRectangle(new System.Drawing.SolidBrush(this.basegrid.BackgroundColor), r1);

            System.Drawing.StringFormat format = new System.Drawing.StringFormat();

            format.Alignment = System.Drawing.StringAlignment.Center;
            format.LineAlignment = System.Drawing.StringAlignment.Center;


            e.Graphics.DrawString(Mer_String,
            this.basegrid.ColumnHeadersDefaultCellStyle.Font,
            new System.Drawing.SolidBrush(this.basegrid.ColumnHeadersDefaultCellStyle.ForeColor),
            r1,
            format);
            //}

            //catch (Exception ec)
            //{
            //    return;
            //}

        }
        // 셀병합과 관련해서  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>          



        private void dGridView_KeyDown(object sender, KeyEventArgs e)
        {
            //그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
            if (sender is DataGridView)
            {
                if (e.KeyValue == 46)
                {
                    e.Handled = true;
                } // end if

            }
        }


    }// end cls_Grid_Base
}

