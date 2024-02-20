using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLM_Program
{
    public partial class frmBase_User_ETC_Staff : Form
    {
        private string base_db_name = "tbl_User_ETC";
        private int Data_Set_Form_TF = 0;
        cls_Grid_Base cgb_2 = new cls_Grid_Base();

        public frmBase_User_ETC_Staff()
        {
            InitializeComponent();
        }

        private void frmBase_User_ETC_Staff_Load(object sender, EventArgs e)
        {
            //cls_form_Meth cm = new cls_form_Meth();
           

            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Exit);
            cfm.from_control_text_base_chang(this);

            ETC_Grid_Set();
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
            //if (e.KeyValue == 113)
            //    T_bt = butt_Save;     //저장  F1
            //if (e.KeyValue == 115)
            //    T_bt = butt_Delete;   // 삭제  F4
            //if (e.KeyValue == 119)
            //    T_bt = butt_Excel;    //엑셀  F8    
            //if (e.KeyValue == 112)
            //    T_bt = butt_Clear;    //엑셀  F5    

            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123 || e.KeyValue == 113 || e.KeyValue == 115 || e.KeyValue == 119 || e.KeyValue == 112)
                    cmdSave_Click(T_bt, ee1);
            }
        }


        private void cmdSave_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

   
            if (bt.Name == "butt_Exit")
            {
                this.Close();
            }


          

        }



        private void ETC_Grid_Set()
        {

            dGridView_ETC_Header_Reset();
            cgb_2.d_Grid_view_Header_Reset();

            string Tsql = "";

            //string[] g_HeaderText = {"로그인_시간"  , "로그오프_시간"   , "IP"  , "구분"   , ""        
            //                    , ""   , ""    , ""  , "" , ""                                
            //                    };

            Tsql = "Select T_ETC, '' ";
            Tsql = Tsql + ", ''  ";
            Tsql = Tsql + ",'' ";
            Tsql = Tsql + ", '' ";
            Tsql = Tsql + " ,RecordTime     ,T_index , '' , '' ,'' ";
            Tsql = Tsql + " From  tbl_User_ETC  (nolock) ";

            Tsql = Tsql + " Where Visible_TF =  1 ";
            Tsql = Tsql + " And   Visible_Date <= '" + cls_User.gid_date_time + "'";
    
            Tsql = Tsql + " And  (Visible_User ='전체'";
            Tsql = Tsql + " OR    Charindex (Visible_User,'" + cls_User.gid  + "') >0 )" ;
    

            Tsql = Tsql + " Order by   T_index desc";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_Login(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb_2.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_2.db_grid_Obj_Data_Put();

            if (cgb_2.basegrid.RowCount > 0)
            {
                EventArgs e = null;
                cgb_2.basegrid.CurrentCell = cgb_2.basegrid.Rows[0].Cells[0];
                dGridView_Base_2_DoubleClick(cgb_2.basegrid, e);
            }
        }


        private void Set_gr_Login(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;

            object[] row0 = new object[cgb_2.grid_col_Count];

            while (Col_Cnt < cgb_2.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }

            
            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_ETC_Header_Reset()
        {
            cgb_2.Grid_Base_Arr_Clear();
            cgb_2.basegrid = dGridView_Base_2;
            cgb_2.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_2.grid_col_Count = 10;

            //cgb_2.grid_Frozen_End_Count = 3;
            //cgb_2.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"내역"  , "_공지_시작일"   , "_공지_여부"  , "_공지_적용자"   , "_기록자"        
                                , "기록일자"   , "_T_index"    , "_Visible_TF"  , "" , ""                                
                                };

            int[] g_Width = { 400, 0, 0, 0, 0
                            ,140 , 0 , 0 , 0 , 0                          
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleLeft  
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };


            cgb_2.grid_col_header_text = g_HeaderText;
            cgb_2.grid_col_w = g_Width;
            cgb_2.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                                                 
                                   };
            cgb_2.grid_col_Lock = g_ReadOnly;

        }

        private void dGridView_Base_2_DoubleClick(object sender, EventArgs e)
        {
            //int rowcnt = (sender as DataGridView).CurrentCell.RowIndex;  
            if ((sender as DataGridView).CurrentRow.Cells[0].Value != null)
            {
                txtKey.Text = (sender as DataGridView).CurrentRow.Cells[6].Value.ToString();

                txtETC.Text = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                             


            }
        }











    }
}
