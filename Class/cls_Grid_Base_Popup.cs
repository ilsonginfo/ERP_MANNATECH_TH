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

    class cls_Grid_Base_Popup
    {
        public DataGridView basegrid;
        public object Base_tb;
        public TextBox Base_tb_2;
        public object Base_Location_obj;
        public Form Base_fr;
        public Boolean Change_Header_Text_TF;
        public Dictionary<string, TextBox> Base_text_dic;
        public Control Next_Focus_Control;



        public void db_grid_Popup_Base(int gridCnt, string headerText_1, string headerText_2, string FieldName_1, string FieldName_2, string Tsql)
        {
            basegrid.Tag = Base_tb_2.Name;



            basegrid.Rows.Clear();
            basegrid.ColumnCount = gridCnt;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //basegrid.EnableHeadersVisualStyles = false;


            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.BorderStyle = BorderStyle.FixedSingle;
            basegrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            //basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(199, 220, 175);
            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            basegrid.RowTemplate.Height = 20;
            basegrid.ColumnHeadersHeight = 19;

            //dGridView_Base_Header_Reset(headerText_1, headerText_2);

            db_grid_Popup_SetDate(Tsql, FieldName_1, FieldName_2);

            dGridView_Base_Header_Reset(headerText_1, headerText_2);    // 240308 - 허성윤, db_grid_Popup_SetDate() 함수 뒤에 실행되도록 위치 조정.

            basegrid.DoubleClick += new System.EventHandler(dGridView_Base_DoubleClick);
            basegrid.KeyDown += new KeyEventHandler(dGridView_KeyDown);
            // basegrid.SortCompare += new DataGridViewSortCompareEventHandler(basegrid_SortCompare);

            //cls_form_Meth cfm = new cls_form_Meth();
            //cfm.form_Group_Panel_Enable_False(Base_fr);

            basegrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);   //240308 허성윤 추가


            basegrid.BringToFront();
            basegrid.RowHeadersVisible = true;
            //basegrid.row  .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            basegrid.BackgroundColor = System.Drawing.Color.White;
            basegrid.Visible = true;
            basegrid.Focus();
        }

        public void db_grid_Popup_Base(int gridCnt, string headerText_1, string headerText_2, string FieldName_1, string FieldName_2, string Tsql, int MemberSort)
        {
            basegrid.Tag = Base_tb_2.Name;
            basegrid.Rows.Clear();
            basegrid.ColumnCount = gridCnt;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //basegrid.EnableHeadersVisualStyles = false;

            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.BorderStyle = BorderStyle.FixedSingle;
            basegrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            basegrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            ////basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(199, 220, 175);
            ////basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 241, 220);
            ////basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.RowTemplate.Height = 20;
            basegrid.ColumnHeadersHeight = 19;

            dGridView_Base_Header_Reset(headerText_1, headerText_2);
            db_grid_Popup_SetDate(Tsql, FieldName_1, FieldName_2);

            //cls_form_Meth cfm = new cls_form_Meth();
            // cfm.form_Group_Panel_Enable_False(Base_fr);


            basegrid.BringToFront();
            basegrid.RowHeadersVisible = true;
            basegrid.BackgroundColor = System.Drawing.Color.White;
            basegrid.Visible = true;
            basegrid.Focus();
        }



        public void db_grid_Popup_Base(int gridCnt,
                                        string headerText_1, string headerText_2, string headerText_3, string headerText_4,
                                        string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4,
                                        string Tsql)
        {
            basegrid.Tag = Base_tb_2.Name;
            basegrid.Rows.Clear();
            basegrid.ColumnCount = gridCnt;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //basegrid.EnableHeadersVisualStyles = false;

            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            basegrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            ////basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(199, 220, 175);
            ////basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 241, 220);
            ////basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;


            dGridView_Base_Header_Reset(headerText_1, headerText_2, headerText_3, headerText_4);

            db_grid_Popup_SetDate(Tsql, FieldName_1, FieldName_2, FieldName_3, FieldName_4);

            basegrid.DoubleClick += new System.EventHandler(dGridView_Base_DoubleClick);
            basegrid.KeyDown += new KeyEventHandler(dGridView_KeyDown);
            // basegrid.SortCompare += new DataGridViewSortCompareEventHandler(basegrid_SortCompare);

            // cls_form_Meth cfm = new cls_form_Meth();
            //cfm.form_Group_Panel_Enable_False(Base_fr);


            basegrid.BringToFront();
            basegrid.RowHeadersVisible = true;
            basegrid.BackgroundColor = System.Drawing.Color.White;
            basegrid.Visible = true;
            basegrid.Focus();
        }

        public void db_grid_Popup_Base_member(int gridCnt,
                                      string headerText_1, string headerText_2, string headerText_3, string headerText_4,
                                      string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4,
                                      string Tsql)
        {
            basegrid.Tag = Base_tb_2.Name;
            basegrid.Rows.Clear();
            basegrid.ColumnCount = gridCnt;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //basegrid.EnableHeadersVisualStyles = false;

            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            basegrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            ////basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(199, 220, 175);
            ////basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 241, 220);
            ////basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;


            dGridView_Base_Header_Reset(headerText_1, headerText_2, headerText_3, headerText_4);

            db_grid_Popup_SetDate(Tsql, FieldName_1, FieldName_2, FieldName_3, FieldName_4);

            basegrid.DoubleClick += new System.EventHandler(dGridView_Base_DoubleClick);
            basegrid.KeyDown += new KeyEventHandler(dGridView_KeyDown);
            // basegrid.SortCompare += new DataGridViewSortCompareEventHandler(basegrid_SortCompare);

            // cls_form_Meth cfm = new cls_form_Meth();
            //cfm.form_Group_Panel_Enable_False(Base_fr);


            basegrid.BringToFront();
            basegrid.RowHeadersVisible = true;
            basegrid.BackgroundColor = System.Drawing.Color.White;
            basegrid.Visible = true;
            basegrid.Focus();
        }


        void basegrid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            try
            {
                // Try to sort based on the cells in the current column.
                e.SortResult = System.String.Compare(e.CellValue1.ToString().Replace(",", ""), e.CellValue2.ToString().Replace(",", ""));

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


        public void db_grid_Popup_Base(int gridCnt,
                                       string headerText_1, string headerText_2, string headerText_3, string headerText_4, string headerText_5,
                                       string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4, string FieldName_5,
                                       string Tsql)
        {
            basegrid.Tag = Base_tb_2.Name;
            basegrid.Rows.Clear();
            basegrid.ColumnCount = gridCnt;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //basegrid.EnableHeadersVisualStyles = false;

            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            basegrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            ////basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(199, 220, 175);
            ////basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 241, 220);
            ////basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;


            dGridView_Base_Header_Reset(headerText_1, headerText_2, headerText_3, headerText_4, headerText_5);

            db_grid_Popup_SetDate(Tsql, FieldName_1, FieldName_2, FieldName_3, FieldName_4, FieldName_5);

            basegrid.DoubleClick += new System.EventHandler(dGridView_Base_DoubleClick);
            basegrid.KeyDown += new KeyEventHandler(dGridView_KeyDown);
            //basegrid.SortCompare += new DataGridViewSortCompareEventHandler(basegrid_SortCompare);

            // cls_form_Meth cfm = new cls_form_Meth();
            //cfm.form_Group_Panel_Enable_False(Base_fr);


            basegrid.BringToFront();
            basegrid.RowHeadersVisible = true;
            basegrid.BackgroundColor = System.Drawing.Color.White;
            basegrid.Visible = true;
            basegrid.Focus();
        }


        private void dGridView_Base_Header_Reset(string headerText_1, string headerText_2)
        {
            db_grid_Popup_Location();

            cls_form_Meth cm = new cls_form_Meth();
            basegrid.Columns[0].HeaderText = cm._chang_base_caption_search(headerText_1);
            basegrid.Columns[1].HeaderText = cm._chang_base_caption_search(headerText_2);

            if (headerText_1 != "")
                basegrid.Columns[0].Width = 100;
            else
            {
                basegrid.Columns[0].Width = 0;
                basegrid.Columns[0].Visible = false;
            }

            if (headerText_2 != "")
                basegrid.Columns[1].Width = 150;
            else
            {
                basegrid.Columns[1].Width = 0;
                basegrid.Columns[1].Visible = false;
            }

            basegrid.Columns[0].ReadOnly = true;
            basegrid.Columns[1].ReadOnly = true;
            basegrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }




        private void dGridView_Base_Header_Reset(string headerText_1, string headerText_2, string headerText_3, string headerText_4)
        {
            db_grid_Popup_Location(1);

            cls_form_Meth cm = new cls_form_Meth();
            basegrid.Columns[0].HeaderText = cm._chang_base_caption_search(headerText_1);
            basegrid.Columns[1].HeaderText = cm._chang_base_caption_search(headerText_2);
            basegrid.Columns[2].HeaderText = cm._chang_base_caption_search(headerText_3);
            basegrid.Columns[3].HeaderText = cm._chang_base_caption_search(headerText_4);

            if (headerText_1 != "")
                basegrid.Columns[0].Width = 200;
            else
            {
                basegrid.Columns[0].Width = 0;
                basegrid.Columns[0].Visible = false;
            }

            if (headerText_2 != "")
                basegrid.Columns[1].Width = 90;
            else
            {
                basegrid.Columns[1].Width = 0;
                basegrid.Columns[1].Visible = false;
            }

            if (headerText_3 != "")
                basegrid.Columns[2].Width = 70;
            else
            {
                basegrid.Columns[2].Width = 0;
                basegrid.Columns[2].Visible = false;
            }

            if (headerText_4 != "")
                basegrid.Columns[3].Width = 70;
            else
            {
                basegrid.Columns[3].Width = 0;
                basegrid.Columns[3].Visible = false;
            }

            basegrid.Columns[0].ReadOnly = true;
            basegrid.Columns[1].ReadOnly = true;
            basegrid.Columns[2].ReadOnly = true;
            basegrid.Columns[3].ReadOnly = true;
            basegrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            basegrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            basegrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void dGridView_Base_Header_Reset(string headerText_1, string headerText_2, string headerText_3, string headerText_4, string headerText_5)
        {
            db_grid_Popup_Location(1);

            cls_form_Meth cm = new cls_form_Meth();
            basegrid.Columns[0].HeaderText = cm._chang_base_caption_search(headerText_1);
            basegrid.Columns[1].HeaderText = cm._chang_base_caption_search(headerText_2);
            basegrid.Columns[2].HeaderText = cm._chang_base_caption_search(headerText_3);
            basegrid.Columns[3].HeaderText = cm._chang_base_caption_search(headerText_4);
            basegrid.Columns[4].HeaderText = cm._chang_base_caption_search(headerText_5);

            if (headerText_1 != "")
                basegrid.Columns[0].Width = 200;
            else
            {
                basegrid.Columns[0].Width = 0;
                basegrid.Columns[0].Visible = false;
            }

            if (headerText_2 != "")
                basegrid.Columns[1].Width = 90;
            else
            {
                basegrid.Columns[1].Width = 0;
                basegrid.Columns[1].Visible = false;
            }

            if (headerText_3 != "")
                basegrid.Columns[2].Width = 70;
            else
            {
                basegrid.Columns[2].Width = 0;
                basegrid.Columns[2].Visible = false;
            }

            if (headerText_4 != "")
                basegrid.Columns[3].Width = 70;
            else
            {
                basegrid.Columns[3].Width = 0;
                basegrid.Columns[3].Visible = false;
            }

            if (headerText_5 != "")
                basegrid.Columns[4].Width = 70;
            else
            {
                basegrid.Columns[4].Width = 0;
                basegrid.Columns[4].Visible = false;
            }

            basegrid.Columns[0].ReadOnly = true;
            basegrid.Columns[1].ReadOnly = true;
            basegrid.Columns[2].ReadOnly = true;
            basegrid.Columns[3].ReadOnly = true;
            basegrid.Columns[4].ReadOnly = true;
            basegrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            basegrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            basegrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            basegrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }


        private void db_grid_Popup_Location()
        {
            if ((Base_Location_obj is TextBox) == true)
            {
                TextBox tb = (TextBox)Base_Location_obj;
                Control t_cn = tb.Parent;
                int t_Left = 0; int t_Top = 0;

                while (t_cn.Name != Base_fr.Name)
                {
                    t_Left = t_Left + t_cn.Left;
                    t_Top = t_Top + t_cn.Top;

                    t_cn = t_cn.Parent;
                }

                basegrid.Top = tb.Top + t_Top + 27;
                basegrid.Left = tb.Left + t_Left - 5;

            }

            if ((Base_Location_obj is Button) == true)
            {
                Button tb = (Button)Base_Location_obj;
                basegrid.Top = tb.Parent.Top + tb.Top + 27;
                basegrid.Left = tb.Parent.Left + tb.Left - 5;
            }
            basegrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);   //240308 허성윤 추가

            int FormWitdh = 150;
            foreach (DataGridViewColumn col in basegrid.Columns)
            {
                FormWitdh += col.Width;
            }

            basegrid.Width = FormWitdh;
            basegrid.Height = 300;
        }

        private void db_grid_Popup_Location(int TT)
        {
            if ((Base_Location_obj is TextBox) == true)
            {
                TextBox tb = (TextBox)Base_Location_obj;
                Control t_cn = tb.Parent;
                int t_Left = 0; int t_Top = 0;

                while (t_cn.Name != Base_fr.Name)
                {
                    t_Left = t_Left + t_cn.Left;
                    t_Top = t_Top + t_cn.Top;

                    t_cn = t_cn.Parent;
                }

                basegrid.Top = tb.Top + t_Top + 27;
                basegrid.Left = tb.Left + t_Left - 5;
            }

            if ((Base_Location_obj is Button) == true)
            {
                Button tb = (Button)Base_Location_obj;
                basegrid.Top = tb.Parent.Top + tb.Top + 27;
                basegrid.Left = tb.Parent.Left + tb.Left - 5;
            }

            int gridWitdh = 65;
            foreach (DataGridViewColumn col in basegrid.Columns)
            {
                gridWitdh += col.Width;
            }

            basegrid.Width = gridWitdh;
            basegrid.Height = 300;
        }



        private void db_grid_Popup_SetDate(string Tsql, string FieldName_1, string FieldName_2)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "TempTable", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt == 0) return;

            Dictionary<int, string[]> gr_dic_text = new Dictionary<int, string[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt, FieldName_1, FieldName_2);  //데이타를 배열에 넣는다.
            }

            db_grid_Data_Put(gr_dic_text);
        }

        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, string[]> gr_dic_text, int fi_cnt, string FieldName_1, string FieldName_2)
        {
            if (Change_Header_Text_TF == true)
            {
                string str_1 = ""; string str_2 = "";

                cls_form_Meth cm = new cls_form_Meth();
                str_1 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_1].ToString());
                str_2 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_2].ToString());

                string[] row0 = { str_1 ,
                                  str_2
                                };

                gr_dic_text[fi_cnt + 1] = row0;
            }
            else
            {
                string[] row0 = { ds.Tables["TempTable"].Rows[fi_cnt][FieldName_1].ToString() ,
                                  ds.Tables["TempTable"].Rows[fi_cnt][FieldName_2].ToString()
                                };

                gr_dic_text[fi_cnt + 1] = row0;
            }


        }




        private void db_grid_Popup_SetDate(string Tsql, string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "TempTable", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt == 0) return;

            Dictionary<int, string[]> gr_dic_text = new Dictionary<int, string[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt, FieldName_1, FieldName_2, FieldName_3, FieldName_4);  //데이타를 배열에 넣는다.
            }

            db_grid_Data_Put(gr_dic_text);
        }

        private void db_grid_Popup_SetDate(string Tsql, string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4, string FieldName_5)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "TempTable", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt == 0) return;

            Dictionary<int, string[]> gr_dic_text = new Dictionary<int, string[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt, FieldName_1, FieldName_2, FieldName_3, FieldName_4, FieldName_5);  //데이타를 배열에 넣는다.
            }

            db_grid_Data_Put(gr_dic_text);
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, string[]> gr_dic_text, int fi_cnt, string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4)
        {
            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
            //string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalInputPrice)
            if (Change_Header_Text_TF == true)
            {
                string str_1 = ""; string str_2 = ""; string str_3 = ""; string str_4 = "";

                cls_form_Meth cm = new cls_form_Meth();
                str_1 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_1].ToString());
                str_2 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_2].ToString());
                str_3 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_3].ToString());
                str_4 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_4].ToString());

                string[] row0 = { str_1 ,
                                  str_2 ,
                                  str_3 ,
                                  str_4 ,
                                };

                gr_dic_text[fi_cnt + 1] = row0;
            }
            else
            {
                string[] row0 = { ds.Tables["TempTable"].Rows[fi_cnt][FieldName_1].ToString() ,
                                  ds.Tables["TempTable"].Rows[fi_cnt][FieldName_2].ToString() ,
                                  string.Format(cls_app_static_var.str_Currency_Type,ds.Tables["TempTable"].Rows[fi_cnt][FieldName_3]) ,
                                  encrypter.Decrypt (string.Format(cls_app_static_var.str_Currency_Type,ds.Tables["TempTable"].Rows[fi_cnt][FieldName_4]))
                                };

                gr_dic_text[fi_cnt + 1] = row0;
            }


        }

        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, string[]> gr_dic_text, int fi_cnt, string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4, string FieldName_5)
        {
            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
            //string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalInputPrice)
            if (Change_Header_Text_TF == true)
            {
                string str_1 = ""; string str_2 = ""; string str_3 = ""; string str_4 = ""; string str_5 = "";

                cls_form_Meth cm = new cls_form_Meth();
                str_1 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_1].ToString());
                str_2 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_2].ToString());
                str_3 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_3].ToString());
                str_4 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_4].ToString());
                str_5 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_5].ToString());

                string[] row0 = { str_1 ,
                                  str_2 ,
                                  str_3 ,
                                  str_4 ,
                                   str_5 ,
                                };

                gr_dic_text[fi_cnt + 1] = row0;
            }
            else
            {
                string[] row0 = { ds.Tables["TempTable"].Rows[fi_cnt][FieldName_1].ToString() ,
                                  ds.Tables["TempTable"].Rows[fi_cnt][FieldName_2].ToString() ,
                                  string.Format(cls_app_static_var.str_Currency_Type, ds.Tables["TempTable"].Rows[fi_cnt][FieldName_3]) ,                               
                                  // 20200806 왜 DEcrypt를 할까? 의미없어보임 encrypter.Decrypt (string.Format(cls_app_static_var.str_Currency_Type,ds.Tables["TempTable"].Rows[fi_cnt][FieldName_4])),
                                  string.Format(cls_app_static_var.str_Currency_Type,ds.Tables["TempTable"].Rows[fi_cnt][FieldName_4]),
                                  string.Format(cls_app_static_var.str_Currency_Type, ds.Tables["TempTable"].Rows[fi_cnt][FieldName_5])
                                };

                gr_dic_text[fi_cnt + 1] = row0;
            }


        }





        private void db_grid_Data_Put(Dictionary<int, string[]> gr_dic_text)
        {
            foreach (int t_key in gr_dic_text.Keys)
            {
                basegrid.Rows.Add(gr_dic_text[t_key]);
            }

            basegrid.AllowUserToAddRows = false;
        }


        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            DataGridView T_Gd = (DataGridView)sender;

            if (T_Gd.CurrentRow?.Cells[0].Value != null)
            {
                if (Base_text_dic != null)
                {
                    TextBox tttbb = null;
                    int fCnt = 0;
                    foreach (string t_key in Base_text_dic.Keys)
                    {
                        Base_text_dic[t_key].Text = T_Gd.CurrentRow.Cells[fCnt].Value.ToString();

                        if (fCnt == 0)
                            tttbb = (TextBox)Base_text_dic[t_key];
                        fCnt++;
                    }

                    basegrid.Visible = false;
                    basegrid.Dispose();

                    //cls_form_Meth cfm = new cls_form_Meth();
                    //cfm.form_Group_Panel_Enable_True(Base_fr);

                    if (Next_Focus_Control == null)
                    {
                        tttbb.Focus();
                        Control tb21 = Base_fr.GetNextControl(Base_fr.ActiveControl, true);
                        if (tb21 != null) tb21.Focus();
                    }
                    else
                        Next_Focus_Control.Focus();
                }
                else
                {
                    if ((Base_tb is TextBox) == true)
                    {
                        TextBox tb = (TextBox)Base_tb;
                        tb.Text = T_Gd.CurrentRow.Cells[0].Value.ToString();
                        //tb.Focus();
                    }

                    if ((Base_tb is MaskedTextBox) == true)
                    {
                        MaskedTextBox tb = (MaskedTextBox)Base_tb;
                        tb.Text = T_Gd.CurrentRow.Cells[0].Value.ToString();
                    }
                    Base_tb_2.Text = T_Gd.CurrentRow.Cells[1].Value.ToString();


                    basegrid.Visible = false;
                    basegrid.Dispose();

                    //cls_form_Meth cfm = new cls_form_Meth();
                    //cfm.form_Group_Panel_Enable_True(Base_fr);

                    if (Next_Focus_Control == null)
                    {
                        TextBox tb2 = (TextBox)Base_Location_obj;
                        Control t_Parent = tb2.Parent;

                        tb2.Focus();

                        Control tb21 = Base_fr.GetNextControl(Base_fr.ActiveControl, true);
                        if (tb21 != null) tb21.Focus();
                    }
                    else
                        Next_Focus_Control.Focus();

                    //tb2.SelectNextControl(Base_fr.ActiveControl, true, true, false, true);
                    //TextBox tb21 = (TextBox)tb2.Parent.GetNextControl(tb2.Parent, true);
                    //tb21.Focus();
                }
            }
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
                    dGridView_Base_DoubleClick(sender, e);
                }
            }
        }



        public void Db_Grid_Popup_Make_Sql(TextBox tb, TextBox tb1_Code, string Base_Na_Code, string T_SellDate = "", string And_Sql = "", int io_TF = 1, string EtcCode = "")
        {
            cls_form_Meth cm = new cls_form_Meth();

            //cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            //DataGridView Popup_gr = new DataGridView();
            //Popup_gr.Name = "Popup_gr";
            //tfr.Controls.Add(Popup_gr);
            //cgb_Pop.basegrid = Popup_gr;
            //cgb_Pop.Base_fr = tfr;
            //cgb_Pop.Base_tb = tb1_Code;  //앞에게 코드
            //cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            //cgb_Pop.Base_Location_obj = tb;

            string Tsql = "";

            //if (Base_Na_Code == "")
            //    Base_Na_Code = "KR";


            if (tb.Name == "txtCenter" || tb.Name == "txtCenter2" || tb.Name == "txtCenter3" || tb.Name == "txtCenter4" || tb.Name == "txtCenter5")
            {
                Tsql = "Select Ncode , Name  ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where Ncode <> '' ";

                if (tb.Text.Trim() != "")
                {
                    Tsql = Tsql + " And  ( Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";

                    Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + Base_Na_Code + "') )";
                    if (Base_Na_Code != "") Tsql = Tsql + " And  Na_Code = '" + Base_Na_Code + "'";
                }
                else
                {
                    Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + Base_Na_Code + "') )";
                    if (Base_Na_Code != "") Tsql = Tsql + " And  Na_Code = '" + Base_Na_Code + "'";
                }

                if (Base_fr.Name == "frmMember" || Base_fr.Name == "frmSell")
                {
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                }
                if (Base_fr.Name == "frmMember_Update" || Base_fr.Name == "frmMember_UpdateSelect" || // 2019-04-15 구현호 센터관리 회원관련 센터보여주기에 맞춰서 나옴
                    Base_fr.Name == "frmMember_Select" || Base_fr.Name == "frmMember_Center_Change" || Base_fr.Name == "frmMember_Select_Not_Sell"
                     || Base_fr.Name == "frmMember_Select_Group_Center" || Base_fr.Name == "frmMember_Select_Group_Date" || Base_fr.Name == "frmMember_Select_Group_Date_Center"
                     || Base_fr.Name == "frmMember_Select_Union" || Base_fr.Name == "frmSMS_Member"
                     )
                {
                    Tsql = Tsql + " And  ShowMemberCenter = 'Y' ";
                }
                if (Base_fr.Name == "frmSell" || Base_fr.Name == "frmSell_Center_Change" || Base_fr.Name == "frmSell_Select_Group_Cacu" || Base_fr.Name == "frmSell_Select_Group_Card"
                   || Base_fr.Name == "frmSell_Select_Group_Date" || Base_fr.Name == "frmSell_Select_Group_Date_Item" || Base_fr.Name == "frmSell_Select_Group_Date_Sell_Cen"
                   || Base_fr.Name == "frmSell_Select_Group_Item" || Base_fr.Name == "frmSell_Select_Group_Sell_Cen" || Base_fr.Name == "frmSell_Select_Group_Sell_Cen_Card"
                   || Base_fr.Name == "frmSell_Select_Group_Sell_Cen_Item" || Base_fr.Name == "frmStock_OUT" || Base_fr.Name == "frmStock_OUT_Sell"
                   || Base_fr.Name == "frmStock_OUT_Sell_Cancel" || Base_fr.Name == "frmStock_OUT_Sell_Check" || Base_fr.Name == "frmStock_IN"
                   || Base_fr.Name == "frmStock_IN" || Base_fr.Name == "frmStock_IN_Sell" || Base_fr.Name == "frmStock_IN_Sell_Cancel"
                   || Base_fr.Name == "frmStock_OUT_Select" || Base_fr.Name == "frmStock_IN_Select" || Base_fr.Name == "frmStock_Move" || Base_fr.Name == "frmStock_Move_Confirm"
                   || Base_fr.Name == "frmStock_Move_Confirm" || Base_fr.Name == "frmStock_Move_Select" || Base_fr.Name == "frmStock_Select_Center"
                   || Base_fr.Name == "frmSell_Select_Union" || Base_fr.Name == "frmStock_Select_Union_Cancel" || Base_fr.Name == "frmStock_Select_Union")// 2019-04-15 구현호 센터관리 주문관련 센터보여주기에 맞춰서 나옴 
                {
                    Tsql = Tsql + " And  ShowOrderCenter = 'Y' ";
                }
                if (Base_fr.Name == "frmSell_Select" || Base_fr.Name == "frmSell_Select_Detail" || Base_fr.Name == "frmSell_Select_History"
                    || Base_fr.Name == "frmSell_Select_Detail")// 2019-04-16 구현호 한폼에 회원, 주문 센터가 동시에 들어가 있는 폼만 작용된다..
                {
                    if (tb.Name == "txtCenter")
                    {
                        Tsql = Tsql + " And  ShowMemberCenter = 'Y' ";
                    }
                    else if (tb.Name == "txtCenter2")
                    {
                        Tsql = Tsql + " And  ShowOrderCenter = 'Y' ";
                    }
                }
                //회원관련, 주문관련 센터보여주기와 한꺼번에 보여주기는 일단 만들어뒀으나 주석처리함,말나오면 풀어준다
                if (And_Sql != "") Tsql = Tsql + And_Sql;

                //Tsql = Tsql + "  And ncode <> '002'"; //임시, 위에 작업 끝나면 지워야함.
                Tsql = Tsql + " Order by Ncode ";
            }

            if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2" || tb.Name == "txtR_Id3")
            {

                Tsql = "Select user_id ,U_Name   ";
                Tsql = Tsql + " From tbl_User (nolock) ";
                Tsql = Tsql + " Where user_id <> '' ";
                cls_NationService.SQL_User_NationCode(ref Tsql);
                if (tb.Text.Trim() != "")
                {
                    Tsql = Tsql + " And  (U_Name like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    user_id like '%" + tb.Text.Trim() + "%')";
                }

                if (And_Sql != "") Tsql = Tsql + And_Sql;

                Tsql = Tsql + " Order by user_id ";
            }

            if (tb.Name == "txtBank")
            {

                Tsql = "Select Ncode ,BankName    ";
                Tsql = Tsql + " From tbl_Bank (nolock) ";
                Tsql = Tsql + " Where Ncode <> '' ";
                if (tb.Text.Trim() != "")
                {
                    Tsql = Tsql + " And (Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%' )";

                    if (Base_Na_Code != "") Tsql = Tsql + " And   Na_Code = '" + Base_Na_Code + "'";
                }
                else
                {
                    if (Base_Na_Code != "") Tsql = Tsql + " And  Na_Code = '" + Base_Na_Code + "'";
                }

                if (And_Sql != "") Tsql = Tsql + And_Sql;
                Tsql = Tsql + " Order by Ncode ";
            }

            if (tb.Name == "txtChange")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select M_Detail ," + cls_app_static_var.Base_M_Detail_Ex + " ";
                    Tsql = Tsql + " From tbl_Memberinfo_Mod_Detail (nolock) ";
                    Tsql = Tsql + " Where M_Detail <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by " + cls_app_static_var.Base_M_Detail_Ex;

                }
                else
                {
                    Tsql = "Select M_Detail ," + cls_app_static_var.Base_M_Detail_Ex + " ";
                    Tsql = Tsql + " From tbl_Memberinfo_Mod_Detail (nolock) ";
                    Tsql = Tsql + " Where " + cls_app_static_var.Base_M_Detail_Ex + " like '%" + tb.Text.Trim() + "%'";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by " + cls_app_static_var.Base_M_Detail_Ex;
                }

            }



            if (tb.Name == "txt_SellSort")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Ncode ,S_Name    ";
                    Tsql = Tsql + " From tbl_Goods__Sort (nolock) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {
                    Tsql = "Select Ncode ,S_Name    ";
                    Tsql = Tsql + " From tbl_Goods__Sort (nolock) ";
                    Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    S_Name like '%" + tb.Text.Trim() + "%'";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                }
            }

            if (tb.Name == "txtSellCode")
            {
                if (Base_fr.Name == "frmMember_Update_2")
                {
                    Tsql = "";
                    // 한국인 경우
                    if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
                    {
                        Tsql = "select [leavereason_code],[leavereason_name] from tbl_leavereason (nolock)   ";
                    }
                    // 태국인 경우
                    else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
                    {
                        Tsql = "select [leavereason_code],[leavereason_name_EN] from tbl_leavereason (nolock)   ";
                    }

                    Tsql = Tsql + " Where leavereason_code <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by leavereason_code ";
                }
                else
                {
                    if (tb.Text.Trim() == "")
                    {
                        //Tsql = "Select SellCode ,SellTypeName    ";
                        //Tsql = "Select SellCode , [SellTypeName] = " + ((cls_User.gid_CountryCode != "TH") ? "SellTypeName" : "SellTypeName_En") + " ";
                        Tsql = "Select SellCode , [SellTypeName] = " + cls_app_static_var.Base_SellTypeName + " ";
                        Tsql = Tsql + " From tbl_SellType (nolock) ";
                        Tsql = Tsql + " Where SellCode <> '' ";
                        if (And_Sql != "") Tsql = Tsql + And_Sql;
                        Tsql = Tsql + " Order by SellCode ";
                    }
                    else
                    {
                        //Tsql = "Select SellCode ,SellTypeName    ";
                        //Tsql = "Select * ";
                        Tsql = "Select SellCode , [SellTypeName] = " + cls_app_static_var.Base_SellTypeName + " ";
                        Tsql = Tsql + " From tbl_SellType (nolock) ";
                        Tsql = Tsql + " Where SellCode like '%" + tb.Text.Trim() + "%'";
                        //Tsql = Tsql + " OR    SellTypeName like '%" + tb.Text.Trim() + "%'";
                        //Tsql = Tsql + " OR SellTypeName like '%" + tb.Text.Trim() + "%' OR SellTypeName_En like '%" + tb.Text.Trim() + "%'";
                        Tsql = Tsql + " OR    " + cls_app_static_var.Base_SellTypeName + " like '%" + tb.Text.Trim() + "%'";
                        if (And_Sql != "") Tsql = Tsql + And_Sql;
                    }
                }
            }

            if (tb.Name == "txt_BaseOut")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Ncode ,T_Name    ";
                    Tsql = Tsql + " From tbl_Base_Out_Code (nolock) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {
                    Tsql = "Select Ncode ,T_Name    ";
                    Tsql = Tsql + " From tbl_Base_Out_Code (nolock) ";
                    Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    T_Name like '%" + tb.Text.Trim() + "%'";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }


            if (tb.Name == "txt_Base_Rec")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Base_Rec (nolock) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (Base_Na_Code != "") Tsql = Tsql + " And   Na_Code = '" + Base_Na_Code + "'";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {
                    Tsql = "Select  Ncode, Name   ";
                    Tsql = Tsql + " From tbl_Base_Rec (nolock) ";
                    Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                    if (Base_Na_Code != "") Tsql = Tsql + " And   Na_Code = '" + Base_Na_Code + "'";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }

            if (tb.Name == "txt_Receive_Method")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select M_Detail , " + cls_app_static_var.Base_M_Detail_Ex;
                    Tsql = Tsql + " From tbl_Base_Change_Detail (nolock) ";
                    Tsql = Tsql + " Where M_Detail_S = 'tbl_Sales_Rece' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by M_Detail ";
                }
                else
                {
                    Tsql = "Select M_Detail , " + cls_app_static_var.Base_M_Detail_Ex;
                    Tsql = Tsql + " From tbl_Base_Change_Detail (nolock) ";
                    Tsql = Tsql + " Where M_Detail_S = 'tbl_Sales_Rece' ";
                    Tsql = Tsql + " And  ( M_Detail like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    " + cls_app_static_var.Base_M_Detail_Ex + " like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by M_Detail ";
                }
            }
            //20190313 구현호 여기다
            if (tb.Name == "txt_ItemCode" || tb.Name == "txt_ItemCodeUp" || tb.Name == "txt_ItemCodeUpPr" || tb.Name == "txt_ItemCodePr")
            {

                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Name , NCode  ,price2 ,price4, price5    ";
                    Tsql += string.Format(" From ufn_Good_Search_Web_Sell ('{0}', '{1}', '{2}') "
                        , T_SellDate.Replace("-", "").Trim()
                        , Base_Na_Code
                        , EtcCode);
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {
                    Tsql = "Select Name , NCode ,price2,price4 ,price5    ";
                    Tsql += string.Format(" From ufn_Good_Search_Web_Sell ('{0}', '{1}', '{2}') "
                          , T_SellDate.Replace("-", "").Trim()
                          , Base_Na_Code
                          , EtcCode);
                    Tsql = Tsql + " Where (NCode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }


            //20190313 구현호 여기다
            if (tb.Name == "txt_promotion")
            {

                if (tb.Text.Trim() == "")
                {
                    //Tsql = "Select PROC_NAME , PRO_CODE    ";
                    //Tsql += string.Format(" From JDE_PROC "
                    //    , T_SellDate.Replace("-", "").Trim()
                    //    , Base_Na_Code
                    //    , EtcCode);
                    //Tsql = Tsql + " Where PRO_CODE <> '' ";
                    //if (And_Sql != "") Tsql = Tsql + And_Sql;
                    //Tsql = Tsql + " Order by PRO_CODE ";

                    // 240321 - 허성윤 수정. 국가별 구분 추가.
                    Tsql = "Select DISTINCT PROC_NAME , PRO_CODE ";
                    Tsql += " FROM JDE_PROC A WITH(NOLOCK) ";
                    Tsql += " LEFT JOIN ( SELECT B.*, C.Na_Code FROM JDE_PROC_ITEM B WITH(NOLOCK) LEFT JOIN tbl_Goods C WITH(NOLOCK) ON B.ITEMCODE = C.ncode) RES ON A.SEQ = RES.JDE_PROC_SEQ ";
                    Tsql += " Where PRO_CODE <> '' ";
                    if (And_Sql != "") { Tsql += And_Sql; }
                    cls_NationService.SQL_NationCode(ref Tsql, "RES", " AND ", true);

                    Tsql = Tsql + " Order by PRO_CODE ";

                    /*
                    SELECT A.PROC_NAME , A.PRO_CODE 
                    FROM JDE_PROC A WITH(NOLOCK) 
                    LEFT JOIN ( SELECT B.*, C.Na_Code FROM JDE_PROC_ITEM B WITH(NOLOCK) LEFT JOIN tbl_Goods C WITH(NOLOCK) ON B.ITEMCODE = C.ncode) RES ON A.SEQ = RES.JDE_PROC_SEQ
                    Where PRO_CODE <> ''  
                    AND RES.Na_Code = 'TH'
                    Order by PRO_CODE 
                     */
                }
                else
                {
                    //Tsql = "Select PROC_NAME , PRO_CODE  ";
                    //Tsql += string.Format("  From JDE_PROC "
                    //      , T_SellDate.Replace("-", "").Trim()
                    //      , Base_Na_Code
                    //      , EtcCode);
                    //Tsql = Tsql + " Where (PRO_CODE like '%" + tb.Text.Trim() + "%'";
                    //Tsql = Tsql + " OR    PROC_NAME like '%" + tb.Text.Trim() + "%')";
                    //if (And_Sql != "") Tsql = Tsql + And_Sql;
                    //Tsql = Tsql + " Order by PRO_CODE ";

                    // 240321 - 허성윤 수정. 국가별 구분 추가.
                    Tsql = "Select DISTINCT PROC_NAME , PRO_CODE  ";
                    Tsql += " FROM JDE_PROC A WITH(NOLOCK) ";
                    Tsql += " LEFT JOIN ( SELECT B.*, C.Na_Code FROM JDE_PROC_ITEM B WITH(NOLOCK) LEFT JOIN tbl_Goods C WITH(NOLOCK) ON B.ITEMCODE = C.ncode) RES ON A.SEQ = RES.JDE_PROC_SEQ ";
                    Tsql = Tsql + " Where (PRO_CODE like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    PROC_NAME like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") { Tsql += And_Sql; }
                    cls_NationService.SQL_NationCode(ref Tsql, "RES", " AND ", true);
                    Tsql = Tsql + " Order by PRO_CODE ";
                }
            }

            if (tb.Name == "txt_ItemName2")
            {
                if (tb.Text.Trim() == "")
                {

                    Tsql = "Select Ncode ";
                    // 한국인 경우
                    if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
                    {
                        Tsql = Tsql + " ,Name ";
                    }
                    // 태국인 경우
                    else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
                    {
                        Tsql = Tsql + " ,Name_e Name ";
                    }

                    Tsql = Tsql + " From ufn_Good_Search_ETC ('" + T_SellDate.Replace("-", "").Trim() + "','" + Base_Na_Code + "') ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {
                    Tsql = "Select Ncode ";
                    // 한국인 경우
                    if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
                    {
                        Tsql = Tsql + " ,Name ";
                    }
                    // 태국인 경우
                    else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
                    {
                        Tsql = Tsql + " ,Name_e Name ";
                    }

                    Tsql = Tsql + " From ufn_Good_Search_ETC ('" + T_SellDate.Replace("-", "").Trim() + "','" + Base_Na_Code + "') ";
                    Tsql = Tsql + " Where (NCode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }



            if (tb.Name == "txtIO")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Ncode ,T_Name    ";
                    Tsql = Tsql + " From tbl_Base_IO_Code (nolock) ";
                    Tsql = Tsql + " Where Kind_TF ='IO' And T_TF =   " + io_TF;
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {

                    Tsql = "Select Ncode ,T_Name    ";
                    Tsql = Tsql + " From tbl_Base_IO_Code (nolock) ";
                    Tsql = Tsql + " Where Kind_TF ='IO' And T_TF =   " + io_TF;
                    Tsql = Tsql + " And   (Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    T_Name like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }


            if (tb.Name == "txt_C_Card")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Ncode ,cardname    ";
                    Tsql = Tsql + " From tbl_Card (nolock) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {

                    Tsql = "Select Ncode ,cardname    ";
                    Tsql = Tsql + " From tbl_Card (nolock) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    Tsql = Tsql + " And   (Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    cardname like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }


            if (tb.Name == "txtP2" || tb.Name == "txtP")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Ncode ,Name    ";
                    Tsql = Tsql + " From tbl_purchase (nolock) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {

                    Tsql = "Select Ncode ,Name    ";
                    Tsql = Tsql + " From tbl_Card (nolock) ";
                    Tsql = Tsql + " Where tbl_purchase <> '' ";
                    Tsql = Tsql + " And   (Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 1)
            {
                tb.Text = ds.Tables["t_P_table"].Rows[0][1].ToString();
                tb1_Code.Text = ds.Tables["t_P_table"].Rows[0][0].ToString();

                if (Next_Focus_Control != null)
                    Next_Focus_Control.Focus();

                return;
            }

            if (tb.Name == "txtCenter" || tb.Name == "txtCenter2" || tb.Name == "txtCenter3" || tb.Name == "txtCenter4" || tb.Name == "txtCenter56")
                db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);

            if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2" || tb.Name == "txtR_Id3")
                db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", Tsql);

            if (tb.Name == "txtBank")
                db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);

            if (tb.Name == "txtChange")
                db_grid_Popup_Base(2, "", "변경내역", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, Tsql);

            if (tb.Name == "txt_BaseOut")
                db_grid_Popup_Base(2, "코드", "출고_사유", "Ncode", "T_Name", Tsql);
            //Select Ncode ,T_Name 

            if (tb.Name == "txt_promotion")
            {
                // db_grid_Popup_Base(2, "프로모션이름", "프로모션코드", "PROC_NAME", "PRO_CODE", Tsql);
                db_grid_Popup_Base(2, cm._chang_base_caption_search("프로모션이름"), cm._chang_base_caption_search("프로모션코드"), "PROC_NAME", "PRO_CODE", Tsql);
            }

            if (tb.Name == "txtSellCode")
            {
                if (Base_fr.Name == "frmMember_Update_2")
                {
                    //db_grid_Popup_Base(2, "재등록가능여부코드", "재등록가능여부명칭", "leavereason_code", "leavereason_name", Tsql);

                    // 한국인 경우
                    if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
                    {
                        db_grid_Popup_Base(2, "재등록가능여부코드", "재등록가능여부명칭", "leavereason_code", "leavereason_name", Tsql);
                    }
                    // 태국인 경우
                    else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
                    {
                        db_grid_Popup_Base(2, "Re-registration availability code", "Name of re-registration availability", "leavereason_code", "leavereason_name_EN", Tsql);
                    }

                }
                else
                {
                    db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", Tsql);
                }
            }
            if (tb.Name == "txt_Base_Rec")
                db_grid_Popup_Base(2, "배송사_코드", "배송사", "ncode", "name", Tsql);

            if (tb.Name == "txt_Receive_Method")
                db_grid_Popup_Base(2, "배송_코드", "배송_구분", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, Tsql);

            if (tb.Name == "txt_ItemCode" || tb.Name == "txt_ItemCodeUp" || tb.Name == "txt_ItemCodePr" || tb.Name == "txt_ItemCodeUpPr")        //20190313 구현호 여기다 
                db_grid_Popup_Base(5, "상품명", "상품코드", "개별단가", "개별PV", "개별CV", "Name", "Ncode", "price2", "price4", "price5", Tsql);

            if (tb.Name == "txt_ItemName2")
                db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", Tsql);

            if (tb.Name == "txtIO")
                db_grid_Popup_Base(2, "입고_코드", "입고종류", "Ncode", "T_Name", Tsql);

            if (tb.Name == "txt_C_Card")
                db_grid_Popup_Base(2, "카드_코드", "카드명", "ncode", "cardname", Tsql);



        }


        public void Db_Grid_Popup_Make_Sql_Not(TextBox tb, TextBox tb1_Code, string Base_Na_Code)
        {
            //cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            //DataGridView Popup_gr = new DataGridView();
            //Popup_gr.Name = "Popup_gr";
            //tfr.Controls.Add(Popup_gr);
            //cgb_Pop.basegrid = Popup_gr;
            //cgb_Pop.Base_fr = tfr;
            //cgb_Pop.Base_tb = tb1_Code;  //앞에게 코드
            //cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            //cgb_Pop.Base_Location_obj = tb;

            string Tsql = "";

            //if (Base_Na_Code == "")
            //    Base_Na_Code = "KR";


            if (tb.Name == "txtCenter" || tb.Name == "txtCenter2" || tb.Name == "txtCenter3" || tb.Name == "txtCenter4" || tb.Name == "txtCenter5")
            {
                Tsql = "Select Ncode , Name  ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where Ncode <> '' ";

                if (tb.Text.Trim() != "")
                {
                    Tsql = Tsql + " And  ( Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";

                    if (Base_Na_Code != "") Tsql = Tsql + " And  Na_Code = '" + Base_Na_Code + "'";
                }
                else
                {
                    if (Base_Na_Code != "") Tsql = Tsql + " And  Na_Code = '" + Base_Na_Code + "'";
                }

                if (Base_fr.Name == "frmMember" || Base_fr.Name == "frmSell")
                {
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                }

                Tsql = Tsql + " Order by Ncode ";
            }


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 1)
            {
                tb.Text = ds.Tables["t_P_table"].Rows[0][1].ToString();
                tb1_Code.Text = ds.Tables["t_P_table"].Rows[0][0].ToString();

                if (Next_Focus_Control != null)
                    Next_Focus_Control.Focus();

                return;
            }

            if (tb.Name == "txtCenter" || tb.Name == "txtCenter2" || tb.Name == "txtCenter3" || tb.Name == "txtCenter4" || tb.Name == "txtCenter56")
                db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);

        }


        public void Db_Grid_Popup_Make_Sql(int i, TextBox tb, TextBox tb1_Code, string Base_Na_Code, string T_SellDate, string ABC_TF)
        {

            string Tsql = "";



            if (tb.Name == "txt_ItemCode")
            {
                if (tb.Text.Trim() == "")
                {

                    if (ABC_TF == "1") Tsql = "Select Name , NCode  ,price2, price4 , price5   ";
                    else if (ABC_TF == "3") Tsql = "Select Name , NCode  ,price2 , price4 , price5  "; //직원가
                    Tsql = Tsql + " From ufn_Good_Search_Web_Sell ('" + T_SellDate.Replace("-", "").Trim() + "','" + Base_Na_Code + "','" + ABC_TF + "'   ) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {

                    if (ABC_TF == "1") Tsql = "Select Name , NCode  ,price2, price4 , price5   ";
                    else if (ABC_TF == "3") Tsql = "Select Name , NCode  ,price2 , price4 , price5   "; //직원가
                    Tsql = Tsql + " From ufn_Good_Search_Web_Sell ('" + T_SellDate.Replace("-", "").Trim() + "','" + Base_Na_Code + "','" + ABC_TF + "' ) ";
                    Tsql = Tsql + " Where (NCode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                    Tsql = Tsql + " Order by Ncode ";
                }
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 1)
            {
                tb.Text = ds.Tables["t_P_table"].Rows[0][1].ToString();
                tb1_Code.Text = ds.Tables["t_P_table"].Rows[0][0].ToString();

                if (Next_Focus_Control != null)
                    Next_Focus_Control.Focus();

                basegrid.Visible = false;
                basegrid.Dispose();

                return;
            }

            db_grid_Popup_Base(5, "상품명", "상품코드", "개별단가", "개별PV", "개별CV", "Name", "Ncode", "price2", "price4", "price5", Tsql);

        }




    }// end cls_Grid_Base_Popup

}
