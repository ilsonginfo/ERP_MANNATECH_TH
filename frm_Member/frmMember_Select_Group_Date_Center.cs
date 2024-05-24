using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MLM_Program
{
    public partial class frmMember_Select_Group_Date_Center : clsForm_Extends
    {

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);


        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cg_sub = new cls_Grid_Base();
        private const string base_db_name = "tbl_Memberinfo";
        private int Data_Set_Form_TF;

        //public delegate void SendNumberDele(string Send_Number, string Send_Name);
        //public event SendNumberDele Send_Mem_Number;
        
        Dictionary<string, int> chart_dic = new Dictionary<string, int>();


        public frmMember_Select_Group_Date_Center()
        {
            InitializeComponent();
        }
        


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_sub.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            mtxtCDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtCDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            string[] data_P = { cm._chang_base_caption_search("일별")
                               ,cm._chang_base_caption_search("월별")
                               ,cm._chang_base_caption_search("년별")                               
                              };

            // 각 콤보박스에 데이타를 초기화
            combo_Sort.Items.AddRange(data_P);
            combo_Sort.SelectedIndex = 0;
            combo_Sort.Focus();
         
        }


        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();
        }

        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Select.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_Excel.Left = butt_Select.Left + butt_Select.Width + 2;
            //butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Select);
            //cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);  
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

                            //cls_form_Meth cfm = new cls_form_Meth();
                            //cfm.form_Group_Panel_Enable_True(this);
                        }
                    }
                }// end if

            }

            ////그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
            //if (sender is DataGridView)
            //{
                

            //    if (e.KeyValue == 13)
            //    {
            //        EventArgs ee =null;
            //        dGridView_Base_DoubleClick(sender, ee);
            //        e.Handled = true;
            //    } // end if
            //}

            Button T_bt = butt_Exit;
            if (e.KeyValue == 123)
                T_bt = butt_Exit;    //닫기  F12
            if (e.KeyValue == 113)
                T_bt = butt_Select;     //조회  F1
            // if (e.KeyValue == 115)
            //     T_bt = butt_Delete;   // 삭제  F4
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


        private Boolean Check_TextBox_Error()
        {
           
            cls_Check_Input_Error c_er = new cls_Check_Input_Error();

            if (mtxtCDate1.Text.Replace("-", "").Trim() == "" && mtxtCDate2.Text.Replace("-", "").Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Requisite_Data")
                       + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Date")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));       
                mtxtCDate1.Focus(); return false;
            }




            if (mtxtCDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtCDate1.Text, mtxtCDate1, "Date") == false)
                {
                    mtxtCDate1.Focus();
                    return false;
                }
            }

            if (mtxtCDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtCDate2.Text, mtxtCDate2, "Date") == false)
                {
                    mtxtCDate2.Focus();
                    return false;
                }

            }           

            Data_Set_Form_TF = 1;
            if (mtxtCDate2.Text == "")
                mtxtCDate2.Text = mtxtCDate1.Text;

            Data_Set_Form_TF = 0;

            return true;
        }


        private void Make_Base_Table(cls_Connect_DB Temp_Connect,System.Data.SqlClient.SqlConnection Conn)
        {
            string Tsql = "";

            

            Tsql = "Select  object_id('tempdb..#Temp_Bus')";        
        
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "#Temp_Bus",Conn, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt > 0)
            {
                if ((ds.Tables["#Temp_Bus"].Rows[0][0] != null) && (ds.Tables["#Temp_Bus"].Rows[0][0].ToString () !="{}"))
                {
                    Tsql = "Drop Table #Temp_Bus ";
                    Temp_Connect.Delete_Data(Tsql, "#Temp_Bus", Conn);
                }
            }            

            Tsql = "CREATE TABLE #Temp_Bus ";
            Tsql = Tsql + " (";
            Tsql = Tsql + "RegDate     varchar(10) COLLATE Korean_Wansung_CI_AS not null, ";
            Tsql = Tsql + "BusCode     nvarchar(20) COLLATE Korean_Wansung_CI_AS not null, ";
            Tsql = Tsql + "BusName     nVarchar(50) COLLATE Korean_Wansung_CI_AS not null ,";
            Tsql = Tsql + "Reg_Cnt       int not null   ";

            Tsql = Tsql + " Primary key(RegDate,BusCode )";
            Tsql = Tsql + ")";

            Temp_Connect.Insert_Data(Tsql, "#Temp_Bus", Conn);



            int date_g = 0 ;
            cls_Date_G date_G = new cls_Date_G();

            string DATE1 = mtxtCDate1.Text.Trim().Replace("-", "").Trim();
            string DATE2 = mtxtCDate2.Text.Trim().Replace("-", "").Trim();
            if (DATE2 == string.Empty )
                DATE2 = DATE1;
            if (DATE1 == string.Empty)
                DATE1 = DATE2;

            if (DATE1 == string.Empty && DATE2 == string.Empty)
                return;
            if (DATE1.Length != 8 && DATE2.Length != 8)
                return;

            if (combo_Sort.SelectedIndex == 0)
                date_G.Date_Between(DATE1, DATE2, "D", ref date_g);
            if (combo_Sort.SelectedIndex == 1)
                date_G.Date_Between(DATE1, DATE2, "M", ref date_g);
            if (combo_Sort.SelectedIndex == 2)
                date_G.Date_Between(DATE1, DATE2, "Y", ref date_g);

            string input = ""; DateTime t1 ; 
            DateTime return_Date = DateTime.Now   ;

            int w_cnt = 1;
            while (w_cnt <= date_g)
            {

                input = DATE1.Substring(0, 4) + "-" + DATE1.Substring(4, 2) + "-" + DATE1.Substring(6, 2);
                t1 = DateTime.Parse(input);

                if (combo_Sort.SelectedIndex == 0)
                    return_Date = t1.AddDays (w_cnt-1);

                if (combo_Sort.SelectedIndex == 1)
                    return_Date = t1.AddMonths(w_cnt - 1);

                if (combo_Sort.SelectedIndex == 2)
                    return_Date = t1.AddYears(w_cnt - 1);


                Tsql = "Insert into #Temp_Bus ";
                Tsql = Tsql + " Select " ;

                if (combo_Sort.SelectedIndex == 0)
                    Tsql = Tsql + return_Date.ToString("yyyyMMdd");

                if (combo_Sort.SelectedIndex == 1)
                    Tsql = Tsql + return_Date.ToString("yyyyMMdd").Substring(0, 6);

                if (combo_Sort.SelectedIndex == 2)
                    Tsql = Tsql + return_Date.ToString("yyyyMMdd").Substring(0, 4);

                Tsql = Tsql + " ,Center_Code ";
                Tsql = Tsql + " ,UUCC.Center_Name " ;
                Tsql = Tsql + " ,0 ";
                Tsql = Tsql + " From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') UUCC ";
                Tsql = Tsql + " Where Center_Code in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";

                if (txtCenter_Code.Text.Trim() != "")
                    Tsql = Tsql + " And Center_Code = '" + txtCenter_Code.Text.Trim() + "'";

                



                Temp_Connect.Insert_Data(Tsql, "#Temp_Bus", Conn);

                w_cnt++;
            }



        }


        private void Make_Base_Query(ref string Tsql)
        {

            Tsql = "Select ";
            if (combo_Sort.SelectedIndex == 0)
                Tsql = Tsql + " Replace(tbl_Memberinfo.RegTime , '-','') AS RegDate ";

            if (combo_Sort.SelectedIndex == 1)
                Tsql = Tsql + " LEFT(Replace(tbl_Memberinfo.RegTime , '-','') , 6) AS RegDate ";

            if (combo_Sort.SelectedIndex == 2)
                Tsql = Tsql + " LEFT(Replace(tbl_Memberinfo.RegTime , '-','') , 4) AS RegDate ";

            Tsql = Tsql + " , Center_Code , UUCC.Center_Name ";
            Tsql = Tsql + " , Count(tbl_Memberinfo.Mbid)  AS Reg_cnt   ";
            //Tsql = Tsql + " , Isnull ( (  ";

            //if (combo_Sort.SelectedIndex == 0)
            //{
            //    Tsql = Tsql + " Select Count(Mbid) From tbl_Memberinfo  ";
            //}

            //if (combo_Sort.SelectedIndex == 1)
            //{
            //    Tsql = Tsql + " ";
            //}

            //if (combo_Sort.SelectedIndex == 2)
            //{
            //    Tsql = Tsql + " ";
            //}

            //Tsql = Tsql + "  ) , 0 )  ";


            Tsql = Tsql + " From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') UUCC ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (nolock)  ON tbl_Memberinfo.BusinessCode = UUCC.Center_Code ";
        }



        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = " ";

               //가입일자로 검색 -1
            if ((mtxtCDate1.Text.Replace("-", "").Trim() != "") && (mtxtCDate2.Text.Replace("-", "").Trim() == ""))
            {
                if (combo_Sort.SelectedIndex == 0)
                    strSql = strSql + " Where tbl_Memberinfo.RegTime = '" + mtxtCDate1.Text.Replace("-", "").Trim() + "'";

                if (combo_Sort.SelectedIndex == 1)
                    strSql = strSql + " Where LEFT(tbl_Memberinfo.RegTime ,6) = '" + mtxtCDate1.Text.Replace("-", "").Trim().Substring(0,6)  + "'";

                if (combo_Sort.SelectedIndex == 2)
                    strSql = strSql + " Where LEFT(tbl_Memberinfo.RegTime ,4) = '" + mtxtCDate1.Text.Replace("-", "").Trim().Substring(0,4) + "'";
            }

            //가입일자로 검색 -2
            if ((mtxtCDate1.Text.Replace("-", "").Trim() != "") && (mtxtCDate2.Text.Replace("-", "").Trim() != ""))
            {
                if (combo_Sort.SelectedIndex == 0)
                {
                    strSql = strSql + " Where tbl_Memberinfo.RegTime >= '" + mtxtCDate1.Text.Replace("-", "").Trim() + "'";
                    strSql = strSql + " And tbl_Memberinfo.RegTime <= '" + mtxtCDate2.Text.Replace("-", "").Trim() + "'";
                }

                if (combo_Sort.SelectedIndex == 1)
                {
                    strSql = strSql + " Where LEFT(tbl_Memberinfo.RegTime ,6) >= '" + mtxtCDate1.Text.Replace("-", "").Trim().Substring(0,6) + "'";
                    strSql = strSql + " And LEFT(tbl_Memberinfo.RegTime ,6) <= '" + mtxtCDate2.Text.Replace("-", "").Trim().Substring(0,6) + "'";
                }

                if (combo_Sort.SelectedIndex == 2)
                {
                    strSql = strSql + " Where LEFT(tbl_Memberinfo.RegTime ,4) >= '" + mtxtCDate1.Text.Replace("-", "").Trim().Substring(0,4) + "'";
                    strSql = strSql + " And LEFT(tbl_Memberinfo.RegTime ,4) <= '" + mtxtCDate2.Text.Replace("-", "").Trim().Substring(0,4) + "'";
                }
            }
            
            //센타코드로으로 검색
            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";

            //// strSql = strSql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

            Tsql = Tsql + strSql;

            if (combo_Sort.SelectedIndex == 0)
            {
                Tsql = Tsql + " Group by Replace(tbl_Memberinfo.RegTime , '-','') , Center_Code , UUCC.Center_Name  ";
                //Tsql = Tsql + " Order by Replace(tbl_Memberinfo.RegTime , '-','') DESC , Center_Code ";
            }

            if (combo_Sort.SelectedIndex == 1)
            {
                Tsql = Tsql + " Group by LEFT(Replace(tbl_Memberinfo.RegTime , '-','') , 6) , Center_Code , UUCC.Center_Name ";
                //Tsql = Tsql + " Order by LEFT(Replace(tbl_Memberinfo.RegTime , '-','') , 6) DESC , Center_Code";
            }

            if (combo_Sort.SelectedIndex == 2)
            {
                Tsql = Tsql + " Group by LEFT(Replace(tbl_Memberinfo.RegTime , '-','') , 4) , Center_Code , UUCC.Center_Name ";
                //Tsql = Tsql + " Order by LEFT(Replace(tbl_Memberinfo.RegTime , '-','') , 4) DESC , Center_Code ";
            }                        
        }




        private void Base_Grid_Set()
        {   
            string Tsql = "";
            string StrSql = "";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            System.Data.SqlClient.SqlConnection Conn = Temp_Connect.Conn_Conn();

            Make_Base_Table(Temp_Connect,Conn);
                
            Make_Base_Query(ref Tsql);

            Make_Base_Query_(ref Tsql);



            StrSql = "Update #Temp_Bus SET ";
            StrSql = StrSql + " Reg_cnt = ISNULL(B.Reg_cnt,0)";
            StrSql = StrSql + " FROM  #Temp_Bus  A,";
            StrSql = StrSql + " (";
            StrSql = StrSql +  Tsql ;
            StrSql = StrSql + " ) B";
            StrSql = StrSql + "  Where a.BusCode = b.Center_Code " ;
            StrSql = StrSql + "  And a.RegDate = b.RegDate";

            Temp_Connect.Update_Data(StrSql, "#Temp_Bus", Conn);



            StrSql = "Select RegDate , BusCode , BusName , Reg_Cnt ";
            StrSql = StrSql + " From #Temp_Bus  ";
            StrSql = StrSql + " Order by RegDate DESC ,  BusCode ASC ";
 
            //++++++++++++++++++++++++++++++++
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, Conn, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
                  

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>      

            //if (combo_Sort.SelectedIndex != 0)
            //    Put_Base_Chart(Temp_Connect, Conn); // 차트뿌려주는곳  일일 검색만 빼고 뿌려준다.

            Tsql = "Drop Table #Temp_Bus ";
            Temp_Connect.Delete_Data(Tsql, "#Temp_Bus", Conn);
            Conn.Close();
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,"" //ds.Tables[base_db_name].Rows[fi_cnt][4]                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
            
        }





        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 5;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;

            cgb.grid_Merge = true;
            cgb.grid_Merge_Col_Start_index = 0;
            cgb.grid_Merge_Col_End_index = 0;
            //cg_sub.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"가입일"  , "센타코드"   , "센타명"  , "가입인원"   , ""                                        
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 80, 80, 140, 80, 0                               
                            };
            cgb.grid_col_w = g_Width;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb.grid_cell_format = gr_dic_cell_format;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                                                         
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleCenter  //5                               
                              };
            cgb.grid_col_alignment = g_Alignment;

            dGridView_Base.RowHeadersVisible = false;
        }


        private void Put_Base_Chart(cls_Connect_DB Temp_Connect, System.Data.SqlClient.SqlConnection Conn)
        {
            string StrSql = ""; string Tsql = ""; int W_Cnt = 0; int W_Cnt_2 = 0;
            chart_Day.Series.Clear();
            DataSet ds = new DataSet();
            DataSet ds_2 = new DataSet();

            //Tsql = " Select BusCode , BusName ";
            //Tsql= Tsql + " From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') UUCC ";
            //Tsql = Tsql + " Order by Center_Code ASC ";

            Tsql = " Select Distinct BusCode , BusName  ";
            Tsql = Tsql + " From #Temp_Bus ";
            Tsql = Tsql + " Where Reg_Cnt > 0 ";
            Tsql = Tsql + " Order by BusCode ASC ";
            
            ds.Clear();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, Conn, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            string C_code ="";
            int ReCnt_2 = 0 ;
            W_Cnt = 0 ; 
            cls_form_Meth cm = new cls_form_Meth();
            string series_Name = ""; string s_Name = "" ; int C_Cnt = 0 ;
            
            while  (W_Cnt < ReCnt)
            {
                Series series_Day = new Series();
                C_code = ds.Tables[base_db_name].Rows[W_Cnt][0].ToString () ;
                series_Name =  ds.Tables[base_db_name].Rows[W_Cnt][1].ToString () ;  // 센타명

                if (W_Cnt == 0)
                 {
                    chart_Day.Series.Clear();
                    tabControl1.TabPages[0].Text = series_Name;
                                        
                    if (series_Name == "") series_Name = "무";
                    series_Day.Name = series_Name;
                    series_Day["DrawingStyle"] = "Emboss";
                    series_Day["PointWidth"] = "0.5";
                    series_Day.Name = cm._chang_base_caption_search("날짜별");            
                    series_Day.ChartType = SeriesChartType.Column;
                        
                    chart_Day.Series.Add(series_Day);

                    chart_Day.ChartAreas[0].AxisX.Interval = 1;
                    chart_Day.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
                    chart_Day.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 7;      
                }
                else
                {
                    Chart t_ch = new Chart();
                    TabPage t_tp = new TabPage();
                    Legend t_le = new Legend(); ChartArea t_ca = new ChartArea();

                    t_ch.Name = C_code;
                    t_tp.Text = series_Name;

                    t_tp.BackColor = tabControl1.TabPages[0].BackColor;
                    t_tp.Controls.Add(t_ch);
                    t_ch.Dock = DockStyle.Fill;
                    t_ch.BackColor = chart_Day.BackColor;
                    t_ch.BackGradientStyle = chart_Day.BackGradientStyle;

                    t_ch.Legends.Add(t_le);
                    t_ch.Legends[0].BackColor = chart_Day.Legends[0].BackColor;
                    t_ch.Legends[0].BackGradientStyle = chart_Day.Legends[0].BackGradientStyle;

                    t_ch.ChartAreas.Add(t_ca);
                    t_ch.ChartAreas[0].BackColor = chart_Day.ChartAreas[0].BackColor;
                    t_ch.ChartAreas[0].BackGradientStyle = chart_Day.ChartAreas[0].BackGradientStyle;

                    tabControl1.Controls.Add(t_tp);
                                        
                    if (series_Name == "") series_Name = "무";
                    series_Day.Name = series_Name;
                    series_Day["DrawingStyle"] = "Emboss";
                    series_Day["PointWidth"] = "0.5";
                    series_Day.Name = cm._chang_base_caption_search("날짜별");
                    series_Day.ChartType = SeriesChartType.Column;

                    t_ch.Series.Add(series_Day);

                    t_ch.ChartAreas[0].AxisX.Interval = 1;
                    t_ch.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
                    t_ch.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 7;
                }
                //++++++++++++++++++++++++++++++++
                StrSql = "Select RegDate , BusCode , BusName , Reg_Cnt ";
                StrSql = StrSql + " From #Temp_Bus  ";
                StrSql = StrSql + " Where BusCode ='" + C_code + "'";
                StrSql = StrSql + " And Reg_Cnt > 0 ";
                StrSql = StrSql + " Order by RegDate ASC ";
            
                ds_2.Clear();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, Conn, ds_2) == false) return;
                ReCnt_2 = Temp_Connect.DataSet_ReCount;

                if (ReCnt_2 > 0) 
                {
                    W_Cnt_2 = 0;
                    while  (W_Cnt_2 < ReCnt_2)
                    {
                        s_Name = ds_2.Tables[base_db_name].Rows[W_Cnt_2][0].ToString () ;  //날짜
                        C_Cnt = int.Parse(ds_2.Tables[base_db_name].Rows[W_Cnt_2][3].ToString () ) ; //횟수
                        Push_data(series_Day, s_Name, C_Cnt);

                        W_Cnt_2 ++ ;
                    }
                }
                //++++++++++++++++++++++++++++++++

                W_Cnt++ ;
            }



            // if (gr_dic_text.Count > 0)
            //{
            //    txt_P_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_05);
            //    txt_P_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_06);
            //    txt_P_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_07);

            //    cls_form_Meth cm = new cls_form_Meth();     
           
            //    int f_cnt = 0;
            //    foreach (string t_key in dic_Center.Keys)
            //    {
            //        Cen_Code = t_key;
            //        Cen_Name = dic_Center[t_key];    

            //        if (f_cnt == 0)
            //        {
            //            chart_Day.Series.Clear();
            //            tabControl1.TabPages[0].Text = dic_Center[t_key];

            //            Series series_Day = new Series();
            //            if (Cen_Name == "") Cen_Name = "무";
            //            series_Day.Name = Cen_Name;
            //            series_Day["DrawingStyle"] = "Emboss";
            //            series_Day["PointWidth"] = "0.5";
            //            series_Day.Name = cm._chang_base_caption_search("상품별");            
            //            series_Day.ChartType = SeriesChartType.Column;
                        
            //            chart_Day.Series.Add(series_Day);

            //            chart_Day.ChartAreas[0].AxisX.Interval = 1;
            //            chart_Day.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            //            chart_Day.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 7;

            //            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            //            {
            //                if (ds.Tables[base_db_name].Rows[fi_cnt][0].ToString() == Cen_Code)
            //                {
            //                    itemName = ds.Tables[base_db_name].Rows[fi_cnt][2].ToString();
            //                    itemCnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt][4].ToString());
            //                    Push_data(series_Day, itemName, itemCnt);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            Chart t_ch = new Chart();
            //            TabPage t_tp = new TabPage();
            //            Legend t_le = new Legend(); ChartArea t_ca = new ChartArea();

            //            t_ch.Name = t_key ;
            //            t_tp.Text = dic_Center[t_key];
            //            t_tp.BackColor = tabControl1.TabPages[0].BackColor;
            //            t_tp.Controls.Add(t_ch);
            //            t_ch.Dock = DockStyle.Fill;
            //            t_ch.BackColor = chart_Day.BackColor;
            //            t_ch.BackGradientStyle = chart_Day.BackGradientStyle;

            //            t_ch.Legends.Add(t_le);
            //            t_ch.Legends[0].BackColor = chart_Day.Legends[0].BackColor;
            //            t_ch.Legends[0].BackGradientStyle = chart_Day.Legends[0].BackGradientStyle;

            //            t_ch.ChartAreas.Add(t_ca);
            //            t_ch.ChartAreas[0].BackColor = chart_Day.ChartAreas[0].BackColor;
            //            t_ch.ChartAreas[0].BackGradientStyle = chart_Day.ChartAreas[0].BackGradientStyle;

            //            tabControl1.Controls.Add(t_tp);

            //            Series series_Day = new Series();
            //            if (Cen_Name == "") Cen_Name = "무";
            //            series_Day.Name = Cen_Name;
            //            series_Day["DrawingStyle"] = "Emboss";
            //            series_Day["PointWidth"] = "0.5";
            //            series_Day.Name = cm._chang_base_caption_search("상품별");      
            //            series_Day.ChartType = SeriesChartType.Column;

            //            t_ch.Series.Add(series_Day);

            //            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            //            {
            //                if (ds.Tables[base_db_name].Rows[fi_cnt][0].ToString() == Cen_Code)
            //                {
            //                    itemName = ds.Tables[base_db_name].Rows[fi_cnt][2].ToString();
            //                    itemCnt = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt][4].ToString());
            //                    Push_data(series_Day, itemName, itemCnt);
            //                }
            //            }

            //            t_ch.ChartAreas[0].AxisX.Interval = 1;
            //            t_ch.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            //            t_ch.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 7;

            //        }

            //        f_cnt++;
            //    }


            ////var result = from n in chart_dic
            //chart_dic.Reverse();     
            //foreach (string  t_key in chart_dic.Keys)
       
            
            //{
            //    Center_Put_Chart(t_key,chart_dic[t_key]);                
            //}            
        }




        private void Push_data(Series series, string p, int p_3)
        {
            DataPoint dp = new DataPoint();
            dp.SetValueXY(p, p_3);
            dp.Label = p_3.ToString();
            series.Points.Add(dp);
        }


        //private void Center_Put_Chart( string s_Name, int C_Cnt)
        //{            
        //    Push_data(series_Day, s_Name, C_Cnt);
        //}
               
        

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
                    string Sn = mtb.Text.Replace("-", "").Replace("_", "").Trim();
                    if (mtb.Name == "mtxtBiz1")
                    {
                        if (Sn_Number_(Sn, mtb, "biz") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel1")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel2")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtZip1")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    string R4_name = mtb.Name.Substring(mtb.Name.Length - 4, 4);
                    if (R4_name == "Date" || R4_name == "ate3" || R4_name == "ate1" || R4_name == "ate2" || R4_name == "ate4")
                    {
                        if (Sn_Number_(Sn, mtb, "Date") == true)
                            SendKeys.Send("{TAB}");
                    }

                   

                    SendKeys.Send("{TAB}");
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
                    string[] date_a = mtb.Text.Split('-');

                    if (date_a.Length >= 3 && date_a[0].Trim() != "" && date_a[1].Trim() != "" && date_a[2].Trim() != "")
                    {
                        string Date_YYYY = "0000" + int.Parse(date_a[0]).ToString();

                        date_a[0] = Date_YYYY.Substring(Date_YYYY.Length - 4, 4);

                        if (int.Parse(date_a[1]) < 10)
                            date_a[1] = "0" + int.Parse(date_a[1]).ToString();

                        if (int.Parse(date_a[2]) < 10)
                            date_a[2] = "0" + int.Parse(date_a[2]).ToString();

                        mtb.Text = date_a[0] + '-' + date_a[1] + '-' + date_a[2];

                        cls_Check_Input_Error c_er = new cls_Check_Input_Error();
                        if (mtb.Text.Replace("-", "").Trim() != "")
                        {
                            int Ret = 0;
                            Ret = c_er.Input_Date_Err_Check(mtb);

                            if (Ret == -1)
                            {
                                mtb.Focus(); return false;
                            }
                        }

                    }
                    else
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
                SendKeys.Send("{TAB}");
            }
        }

        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            MaskedTextBox tb = (MaskedTextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
            }
        }


        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);            
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);

            TextBox tb = (TextBox)sender;

            if ((tb.Tag == null) || (tb.Tag.ToString() == ""))
            {
                //숫자만 입력 가능하다.
                if (T_R.Text_KeyChar_Check(e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if   
            }
            else if ((tb.Tag != null) && (tb.Tag.ToString() == "1"))
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1) == false)
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

        }

        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            int Sw_Tab = 0;

            if ((sender is TextBox) == false)  return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
                Sw_Tab = 1;
            }

            if (tb.Name == "txtCenter")
            {
                Data_Set_Form_TF = 1; 
                if (tb.Text.Trim() == "")
                    txtCenter_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            //if (tb.Name == "txtBank")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtBank_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtBank_Code);
            //}

                                   
        }

        

        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {            
            if (tb.Name == "txtCenter")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtCenter_Code,"");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            
            //if (tb.Name == "txtBank")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtBank_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtBank_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}
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
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtCenter2")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtR_Id")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtChange")
                cgb_Pop.Next_Focus_Control = butt_Select;


            cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
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
            cgb_Pop.Base_tb_2 = tb ;    //2번은 명임
            cgb_Pop.Base_Location_obj = tb;

            if (strSql != "")
            {
                if (tb.Name == "txtCenter")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtBank")
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);
            }
            else
            {
                if (tb.Name == "txtCenter")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                    if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " And  ShowMemberCenter = 'Y' ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                }

                if (tb.Name == "txtBank")
                {
                    string Tsql;
                    Tsql = "Select Ncode ,BankName    ";
                    Tsql = Tsql + " From tbl_Bank (nolock) ";
                    if (cls_User.gid_CountryCode != "") Tsql = Tsql + " Where  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);
                }

            }
        }



        private void Ncod_Text_Set_Data(TextBox tb, TextBox tb1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql="";
            
            if (tb.Name == "txtCenter")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
            }


            if (tb.Name == "txtBank")
            {
                Tsql = "Select Ncode , BankName   ";
                Tsql = Tsql + " From tbl_Bank (nolock) ";
                Tsql = Tsql + " Where (Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%')";
                if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
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



        



        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cg_sub.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtCDate1);
                tabControl_Tab_Dispose();
                chart_Day.Series.Clear();
                //radioB_S.Checked = true;
                combo_Sort.SelectedIndex = 0;
                combo_Sort.Focus();

            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<3
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cg_sub.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                chart_Day.Series.Clear();                                
                tabControl_Tab_Dispose();

                chart_dic.Clear();

                if (Check_TextBox_Error() == false) return;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Base_Grid_Set();  //뿌려주는 곳                
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }
           
            else if (bt.Name == "butt_Excel")
            {
                frmBase_Excel e_f = new frmBase_Excel();
                e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
                e_f.ShowDialog();
            }

            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }

        }


        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Member_Reg_Center";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }

       
        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            //SendKeys.Send("{TAB}");
        }



        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string Send_Code = ""; string Send_Date = "";
                Send_Date = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                Send_Code = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Sub_Grid_Set(Send_Date,Send_Code);
                this.Cursor = System.Windows.Forms.Cursors.Default; 
            }
        }




        private void Make_Sub_Query(ref string StrSql)
        {         
            
            StrSql = "Select  ";
            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) ";
            else
                StrSql = StrSql + " tbl_Memberinfo.mbid2 ";

            StrSql = StrSql + " ,tbl_Memberinfo.M_Name ";
            
            StrSql = StrSql + ", tbl_Memberinfo.Cpno ";

            StrSql = StrSql + " , tbl_Business.Name  ";


            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " ,tbl_Memberinfo.Saveid + '-' + Convert(Varchar,tbl_Memberinfo.Saveid2) ";
            else
                StrSql = StrSql + " ,tbl_Memberinfo.Saveid2 ";

            StrSql = StrSql + " , Isnull(Sav.M_Name,'') ";

            if (cls_app_static_var.Member_Number_1 > 0)
                StrSql = StrSql + " ,tbl_Memberinfo.Nominid + '-' + Convert(Varchar,tbl_Memberinfo.Nominid2) ";
            else
                StrSql = StrSql + " ,tbl_Memberinfo.Nominid2 ";

            StrSql = StrSql + " , Isnull(Nom.M_Name,'') ";

            
            StrSql = StrSql + " , tbl_Memberinfo.regtime ";
            StrSql = StrSql + " , tbl_Memberinfo.recordid ";
            StrSql = StrSql + " , tbl_Memberinfo.recordtime ";

            StrSql = StrSql + " From tbl_Memberinfo (nolock) ";
            StrSql = StrSql + " LEFT JOIN tbl_Memberinfo Sav (nolock) ON tbl_Memberinfo.Saveid = Sav.Mbid And tbl_Memberinfo.Saveid2 = Sav.Mbid2 ";
            StrSql = StrSql + " LEFT JOIN tbl_Memberinfo Nom (nolock) ON tbl_Memberinfo.Nominid = Nom.Mbid And tbl_Memberinfo.Nominid2 = Nom.Mbid2 ";
            StrSql = StrSql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";

        }



        private void Make_Sub_Query_(ref string Tsql,string Send_Date ,string search_Code)
        {
            string strSql = " ";
            strSql = strSql + " Where LEFT(Replace(tbl_Memberinfo.RegTime, '-','')," + Send_Date.Length.ToString() + ")  = '" + Send_Date + "'";
            strSql = strSql + " And   tbl_Memberinfo.BusinessCode = '" + search_Code + "'";          

            //센타코드로으로 검색
            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";

            //// strSql = strSql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";
            
            Tsql = Tsql + strSql;
            Tsql = Tsql + " Order by tbl_Memberinfo.Mbid , tbl_Memberinfo.Mbid2  ";
        }




        private void Sub_Grid_Set(string Send_Date, string search_Code)
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_sub.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


            string Tsql = "";
            Make_Sub_Query(ref Tsql);

            Make_Sub_Query_(ref Tsql,Send_Date, search_Code);

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;                        
            
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_Sub_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            cg_sub.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cg_sub.db_grid_Obj_Data_Put();
        }


      

        private void dGridView_Sub_Header_Reset()
        {


            cg_sub.grid_col_Count = 11;
            cg_sub.basegrid = dGridView_Sub_001;
            cg_sub.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_sub.grid_Frozen_End_Count = 2;
            //cg_sub.grid_Frozen_End_Count = 2;
            cg_sub.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"회원_번호"  , "성명"   , "주민번호"  , "센타명"   , "후원인"                                        
                                , "후원인명"   , "추천인"  , "추천인명"   , "등록일"   ,"기록자"                               
                                , "기록일"
                                    };
            cg_sub.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 85, 90, 130, 120, cls_app_static_var.save_uging_Pr_Flag  
                             ,cls_app_static_var.save_uging_Pr_Flag, cls_app_static_var.nom_uging_Pr_Flag, cls_app_static_var.nom_uging_Pr_Flag, 90, 90  
                             ,120
                            };
            cg_sub.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                       
                                    ,true 
                                   };
            cg_sub.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //10

                               ,DataGridViewContentAlignment.MiddleCenter                                                                
                              };
            cg_sub.grid_col_alignment = g_Alignment;
        }


        private void Set_Sub_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][2].ToString (),"Cpno")
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][7]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][10]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][11]  
                                //,ds.Tables[base_db_name].Rows[fi_cnt][12]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][13]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][14]

                                //,ds.Tables[base_db_name].Rows[fi_cnt][15]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][16]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][17]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][18]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][19]

                                //,ds.Tables[base_db_name].Rows[fi_cnt][20]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][21]  
                                //,ds.Tables[base_db_name].Rows[fi_cnt][22]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][23]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][24]

                                //,ds.Tables[base_db_name].Rows[fi_cnt][25]
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void radioB_S_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtCDate1, mtxtCDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;

        }




        private void tabControl_Tab_Dispose()
        {
            for (int fcnt = tabControl1.TabCount - 1; fcnt > 0; fcnt--)
            {
                tabControl1.TabPages[fcnt].Dispose();
            }

            tabControl1.TabPages[0].Text = "";
        }




    }
}
