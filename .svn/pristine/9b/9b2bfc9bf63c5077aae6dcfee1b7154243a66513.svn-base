using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MLM_Program
{
    public partial class frmStock_Close : Form
    {
                  

        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Item = new cls_Grid_Base();
                        
        private const string base_db_name = "tbl_SalesDetail";
        private int Data_Set_Form_TF;

        public delegate void SendNumberDele(string Send_Number, string Send_Name, string Send_OrderNumber);
        public event SendNumberDele Send_Mem_Number;

        private int Form_Load_TF = 0;



        public frmStock_Close()
        {
            InitializeComponent();
        }




        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;
            Form_Load_TF = 0;
            

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            mtxtSDate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtSDate.BackColor = cls_app_static_var.txt_Enable_Color;

            //txtCenter3.Text = "본사";
            //txtCenter3_Code.Text = "000-01";
            //tbl_Center.Enabled = false;

            Search_Stock_Close_Date();
            //tabC_1.SelectedIndex = 0;

            //mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            //mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;
            //grB_Search.Height = mtxtMbid.Top + mtxtMbid.Height + 3;                    
        }

        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (Form_Load_TF == 0)
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset(1);

                dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_Item.d_Grid_view_Header_Reset(1);
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                Form_Load_TF = 1; 
            }
        }

        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Select.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_Excel.Left = butt_Select.Left + butt_Select.Width + 2;
            //butt_Close.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Select);
            cfm.button_flat_change(butt_Close);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);
            cfm.button_flat_change(button_Date);
            
            //cfm.button_flat_change(butt_S_check);
            //cfm.button_flat_change(butt_S_Not_check);
            //cfm.button_flat_change(butt_S_Save); 
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

     

            Button T_bt = butt_Exit;
            if (e.KeyValue == 123)
                T_bt = butt_Exit;    //닫기  F12
            if (e.KeyValue == 113)
                T_bt = butt_Select;     //조회  F1
            if (e.KeyValue == 115)
                T_bt = butt_Close;   // 삭제  F4
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

            if (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")               
            {
                int Ret = 0;
                Ret = c_er._Member_Nmumber_Split(mtxtMbid);

                if (Ret == -1)
                {                    
                    mtxtMbid.Focus();     return false;
                }   
            }


            if (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                int Ret = 0;
                Ret = c_er._Member_Nmumber_Split(mtxtMbid2);

                if (Ret == -1)
                {
                    mtxtMbid2.Focus(); return false;
                }   
            }


            if (mtxtSDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSDate.Text, mtxtSDate, "Date") == false)
                {
                    mtxtSDate.Focus();
                    return false;
                }

            }

            if (mtxtSDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSDate2.Text, mtxtSDate2, "Date") == false)
                {
                    mtxtSDate2.Focus();
                    return false;
                }

            }




            if (mtxtSDate.Text.Replace("-", "").Trim() == "" && mtxtSDate2.Text.Replace("-", "").Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_SDate")                        
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));       

                mtxtSDate.Focus(); return false;
            }


            if (txtMakDate1.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtMakDate1);

                if (Ret == -1)
                {
                    txtMakDate1.Focus(); return false;
                }
            }

            if (txtMakDate2.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtMakDate2);

                if (Ret == -1)
                {
                    txtMakDate2.Focus(); return false;
                }
            }



                   

            return true;
        }


        private void Make_Base_Query(ref string Tsql)
        {

            //string[] g_HeaderText = {"주문번호"  , "주문_일자"   , "반품_교환_일자"  , "회원_번호"   , "성명"        
            //                    , "주민번호"   , "등록_센타명"    , "주문_센타명"   , "주문_종류"    , "총주문액"
            //                    , "총PV"   , "총결제액"  , "현금"   , "카드"   ,"무통장"
            //                    , "미수금"     , "구분"    , "비고1" , "비고2"     , "기록자"
            //                    , "기록일", ""  , ""  , ""  ,""
            //                    , ""
            //                        };


            

            Tsql = Tsql + "  tbl_SalesDetail.OrderNumber  ";

            Tsql = Tsql + " , Case ReturnTF When 1 then LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2)   ";
            Tsql = Tsql + "  ELSE (Select LEFT(A1.SellDate,4) +'-' + LEFT(RIGHT(A1.SellDate,4),2) + '-' + RIGHT(A1.SellDate,2) From tbl_SalesDetail AS A1 Where A1.OrderNumber = tbl_SalesDetail.Re_BaseOrderNumber)  END ";


            Tsql = Tsql + " , Case ReturnTF When 1 then '' ELSE  LEFT(SellDate,4) +'-' + LEFT(RIGHT(SellDate,4),2) + '-' + RIGHT(SellDate,2)  END ";

            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + ", tbl_SalesDetail.mbid + '-' + Convert(Varchar,tbl_SalesDetail.mbid2) ";
            //else
            //    Tsql = Tsql + ", tbl_SalesDetail.mbid2 ";

            if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 > 0)
                Tsql = Tsql + ", tbl_SalesDetail.mbid + '-' + Convert(Varchar,tbl_SalesDetail.mbid2)  ";
            else if (cls_app_static_var.Member_Number_1 == 0 && cls_app_static_var.Member_Number_2 > 0)
                Tsql = Tsql + ", tbl_SalesDetail.mbid2  ";
            else if (cls_app_static_var.Member_Number_1 > 0 && cls_app_static_var.Member_Number_2 == 0)
                Tsql = Tsql + ", tbl_SalesDetail.mbid  ";

            Tsql = Tsql + " ,tbl_SalesDetail.M_Name ";
            
            Tsql = Tsql + ", tbl_Memberinfo.Cpno ";

            Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";
            Tsql = Tsql + " ,Isnull(S_Bus.Name,'') as S_B_Name";

            Tsql = Tsql + " , tbl_SellType.SellTypeName SellCodeName  ";

            Tsql = Tsql + " ,TotalPrice , Totalpv  " ;
            Tsql = Tsql + " ,TotalInputPrice ";
            Tsql = Tsql + " ,InputCash , InputCard ,InputPassbook ";
            Tsql = Tsql + " ,UnaccMoney ";

            Tsql = Tsql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " ReturnTFName ";
            
            
            Tsql = Tsql + " ,tbl_SalesDetail.Etc1 ";
            Tsql = Tsql + " ,tbl_SalesDetail.Etc2 ";

            Tsql = Tsql + " ,tbl_SalesDetail.Recordid ";
            Tsql = Tsql + " ,tbl_SalesDetail.recordtime ";

            Tsql = Tsql + " From tbl_SalesDetail (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";            
            Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode ";
            Tsql = Tsql + " LEFT JOIN tbl_Business S_Bus (nolock) ON tbl_SalesDetail.BusCode = S_Bus.NCode ";            
            Tsql = Tsql + " Left Join tbl_Class C1 On tbl_Memberinfo.CurGrade=C1.Grade_Cnt ";
            Tsql = Tsql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_SalesDetail' And  Ch_T.M_Detail = Convert(Varchar,tbl_SalesDetail.ReturnTF ) ";
            Tsql = Tsql + " LEFT Join tbl_SellType ON tbl_SalesDetail.SellCode = tbl_SellType.SellCode ";
        }



        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = " Where tbl_SalesDetail.Mbid <> ''  ";
            
                        string Mbid = ""; int Mbid2 = 0;
            //회원번호1로 검색
            if (
                (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "") 
                &&
                (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() == "") 
                )
            {
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And tbl_SalesDetail.Mbid ='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_SalesDetail.Mbid2 = " + Mbid2;
                }


            }

            //회원번호2로 검색
            if (
                (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
                &&
                (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
                )
            {
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And tbl_SalesDetail.Mbid >='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_SalesDetail.Mbid2 >= " + Mbid2;
                }

                if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And tbl_SalesDetail.Mbid <='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_SalesDetail.Mbid2 <= " + Mbid2;
                }
            }


            //가입일자로 검색 -1
            if ((mtxtSDate.Text.Replace("-", "").Trim() != "") && (mtxtSDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_SalesDetail.SellDate = '" + mtxtSDate.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtSDate.Text.Replace("-", "").Trim() != "") && (mtxtSDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_SalesDetail.SellDate >= '" + mtxtSDate.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_SalesDetail.SellDate <= '" + mtxtSDate2.Text.Replace("-", "").Trim() + "'";
            }


            //기록일자로 검색 -1
            if ((txtMakDate1.Text.Trim() != "") && (txtMakDate2.Text.Trim() == ""))
                strSql = strSql + " And Replace(Left( tbl_SalesDetail.recordtime ,10),'-','') = '" + txtMakDate1.Text.Trim() + "'";

            //기록일자로 검색 -2
            if ((txtMakDate1.Text.Trim() != "") && (txtMakDate2.Text.Trim() != ""))
            {
                strSql = strSql + " And Replace(Left( tbl_SalesDetail.recordtime ,10),'-','') >= '" + txtMakDate1.Text.Trim() + "'";
                strSql = strSql + " And Replace(Left( tbl_SalesDetail.recordtime ,10),'-','') <= '" + txtMakDate2.Text.Trim() + "'";
            }


           

            //센타코드로으로 검색
            if (txtCenter2_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.BusCode = '" + txtCenter2_Code.Text.Trim() + "'";

            if (txtR_Id_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.recordid = '" + txtR_Id_Code.Text.Trim() + "'";


            if (txtOrderNumber.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.OrderNumber = '" + txtOrderNumber.Text.Trim() + "'";

            



            if (opt_sell_2.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.ReturnTF = 1 ";

            if (opt_sell_3.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.ReturnTF = 2 ";

            if (opt_sell_4.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.ReturnTF = 3 ";

            if (opt_sell_5.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.ReturnTF = 4 ";

            //if (opt_sell_6.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.ReturnTF = 5 ";

           


            

            if (opt_Ed_2.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.UnaccMoney = 0 ";

            if (opt_Ed_3.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.UnaccMoney <> 0 ";



            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            strSql = strSql + " And tbl_SalesDetail.BusCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";

            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


            Tsql = Tsql + strSql ;
            Tsql = Tsql + " Order by tbl_SalesDetail.SellDate DESC, tbl_SalesDetail.OrderNumber ";
            Tsql = Tsql + ",tbl_SalesDetail.Mbid, tbl_SalesDetail.Mbid2  ";
        }




        private void Base_Grid_Set()
        {   
            string Tsql = "";            
            //Make_Base_Query(ref Tsql);
            //Make_Base_Query_(ref Tsql);

            if (mtxtSDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSDate2.Text, mtxtSDate2, "Date") == false)
                {
                    mtxtSDate2.Focus();
                    return ;
                }
            }


            string SDAte1 = "";   string SDate2 = "";
            SDAte1 = mtxtSDate.Text.Replace("-", "").Trim();

            if (mtxtSDate2.Text.Replace("-", "").Trim() == "")
                SDate2 = SDAte1;
            else
                SDate2 = mtxtSDate2.Text.Replace("-", "").Trim();
                                    
            string CenterCode = txtCenter3_Code.Text.Trim();                        
            
            Tsql = "EXEC Usp_Stock_Close_Total '" + SDAte1 + "','" + SDate2 + "','" + CenterCode + "'";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();                                  
            
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name , this.Text ) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            Dictionary<string, string> dic_Center = new Dictionary<string, string>();

            string Cen_Name = "";            string Cen_Code = "";

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                              
            }

             if (gr_dic_text.Count > 0)
            {
                //tab_Center_Item_Chart_Make(dic_Center, ds, ReCnt);                
            }
            
            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                if (ds.Tables[base_db_name].Rows[fi_cnt][24].ToString().Trim() != "")
                {
                    cgb.basegrid.Rows[fi_cnt].DefaultCellStyle.BackColor = System.Drawing.Color.PaleGoldenrod;
                }

            }

        }

        


        private void dGridView_Base_Header_Reset()
        {
            
            cgb.grid_col_Count = 25;            
            cgb.basegrid = dGridView_Base;            
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 5;
            //cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
            string[] g_HeaderText = { "센타코드"   , "센타명" , "상품코드"   , "상품명"  , "전일_재고"      
                                , "일반_입고"   , "반품_입고"    , "_ChangeInQty"   , "이동_입고"    , "입고_합계"

                                , "  "   , "판매_출고"  , "_ChangeOrderOutQty"   , "_DisuseOutQty"   ,"_ReSendOutQty"
                                , "이동_출고"     , "기타_출고"    , "출고_합계" , "_pooraddqty"     , "당일_재고"
                                ,"_MoveOutQty","_EtcOutQty", "_Pre_Chang_StockQty", "_Pre_Chang_StockQty_TF"  , ""  
                               
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = {  100, 120, 90, 160  ,70
                             ,70, 70, 0, 70, 70  
                             ,20 , 70 ,0, 0, 0
                             ,70 , 70, 70 , 0 , 70
                             ,0 , 0 , 0 , 0 , 0                             
                            };
            cgb.grid_col_w = g_Width;

            Color[] g_Color = { SystemColors.Window   , SystemColors.Window  ,  SystemColors.Window ,  SystemColors.Window  , SystemColors.Window
                                ,Color.PaleTurquoise  , Color.PaleTurquoise ,  Color.PaleTurquoise ,  Color.PaleTurquoise ,Color.PaleTurquoise
                                ,SystemColors.Window  , SystemColors.Window ,  SystemColors.Window ,  SystemColors.Window  ,SystemColors.Window
                                ,SystemColors.Window  , SystemColors.Window ,  SystemColors.Window ,   SystemColors.Window  ,Color.PaleTurquoise
                              
                                ,SystemColors.Window  , SystemColors.Window ,  SystemColors.Window ,  SystemColors.Window  ,SystemColors.Window
                             };
            cgb.gric_col_Color  = g_Color;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                        
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight  //5
                               
                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight //10

                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight  //15   
                          
                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight //20

                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight  //25   
                              
                              };
            cgb.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[16 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[17 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[18 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[19 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[20 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[21 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[22 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[23 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[24 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[25 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            cgb.grid_cell_format = gr_dic_cell_format;            
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][7]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][10]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][11]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][12]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][13]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][14]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][15]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][16]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][17]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][18]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][19]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][20]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][21]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][22]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][23]
                                ,"" //ds.Tables[base_db_name].Rows[fi_cnt][24]


                                 };

            gr_dic_text[fi_cnt + 1] = row0;
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
        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);
            //T_R.Key_Enter_13_Name += new Key_13_Name_Event_Handler(T_R_Key_Enter_13_Name);

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

            //if (tb.Name == "txtCenter")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtCenter_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtCenter_Code);
            //}

            //if (tb.Name == "txtBank")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtSellCode_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);
            //}

            if (tb.Name == "txtR_Id")
            {
                if (tb.Text.Trim() == "")
                    txtR_Id_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtR_Id_Code);
            }

            if (tb.Name == "txtCenter3")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter3_Code.Text = "";
                Data_Set_Form_TF = 0;
            }


            if (tb.Name == "txt_ItemName2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txt_ItemName_Code2.Text = "";
                Data_Set_Form_TF = 0;
            }

            //if (tb.Name == "txtSellCode")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtSellCode_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);
            //}
        }

        

        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {            
            //if (tb.Name == "txtCenter")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtCenter_Code,"");
            //    else
            //        Ncod_Text_Set_Data(tb, txtCenter_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            if (tb.Name == "txtR_Id")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtR_Id_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtR_Id_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtR_Id_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            //if (tb.Name == "txtBank")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtSellCode_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            if (tb.Name == "txtCenter3")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter3_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtCenter3_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter3_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_ItemName2")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txt_ItemName_Code2);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txt_ItemName_Code2, "");
                //else
                //    Ncod_Text_Set_Data(tb, txt_ItemName_Code2);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            //if (tb.Name == "txtSellCode")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtSellCode_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);

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

            if (tb.Name == "txtCenter3")
                cgb_Pop.Next_Focus_Control = butt_Select;


            if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtR_Id")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtChange")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtSellCode")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txt_Base_Rec")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txt_Receive_Method")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txt_ItemCode")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txt_ItemName2")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtCenter4")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtIO")
            {
                cgb_Pop.Next_Focus_Control = butt_Select;
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode, "", " And  (Ncode ='004' OR Ncode = '005'  OR Ncode = '006'  ) ");
            }
            else
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
            //Tsql = Tsql + " And  (Ncode ='004' OR Ncode = '005' ) ";


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

                if (tb.Name == "txtCenter3")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);
           
                if (tb.Name == "txtSellCode")
                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", strSql);

                if (tb.Name == "txt_ItemName2")
                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", strSql);
            }
            else
            {
                if (tb.Name == "txtCenter")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
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

                if (tb.Name == "txtBank")
                {
                    string Tsql;
                    Tsql = "Select Ncode ,BankName    ";
                    Tsql = Tsql + " From tbl_Bank (nolock) ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);
                }

                if (tb.Name == "txtCenter3")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                }                                

                if (tb.Name == "txtSellCode")
                {
                    string Tsql;
                    Tsql = "Select SellCode ,SellTypeName    ";
                    Tsql = Tsql + " From tbl_SellType (nolock) ";
                    Tsql = Tsql + " Order by SellCode ";

                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", Tsql);
                }

                if (tb.Name == "txt_ItemName2")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name    ";
                    Tsql = Tsql + " From tbl_Goods (nolock) ";
                    //Tsql = Tsql + " Where GoodUse = 0 ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", Tsql);
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
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
            }

            if (tb.Name == "txtR_Id")
            {
                Tsql = "Select user_id ,U_Name   ";
                Tsql = Tsql + " From tbl_User (nolock) ";
                Tsql = Tsql + " Where U_Name like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    user_id like '%" + tb.Text.Trim() + "%'";
            }

            if (tb.Name == "txtBank")
            {
                Tsql = "Select Ncode , BankName   ";
                Tsql = Tsql + " From tbl_Bank (nolock) ";
                Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%'";
            }


            if (tb.Name == "txtCenter3")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
            }
          

            if (tb.Name == "txtSellCode")
            {
                Tsql = "Select SellCode ,SellTypeName    ";
                Tsql = Tsql + " From tbl_SellType (nolock) ";
                Tsql = Tsql + " Where SellCode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    SellTypeName like '%" + tb.Text.Trim() + "%'";
            }

            if (tb.Name == "txt_ItemName2")
            {
                Tsql = "Select Ncode , Name    ";
                Tsql = Tsql + " From tbl_Goods (nolock) ";
                Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
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



        private void Clear_Object_()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;                        
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Item.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            chart_Item.Series.Clear();
            tabControl_Tab_Dispose();

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtSDate2);

            opt_Ed_1.Checked = true; opt_sell_1.Checked = true;
            tabC_1.SelectedIndex = 0;

            //txtCenter3.Text = "본사";
            //txtCenter3_Code.Text = "000-01";
            //tbl_Center.Enabled = false;

            Search_Stock_Close_Date();

            mtxtSDate2.Focus();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }



        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {
                Clear_Object_();
                
            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                chart_Item.Series.Clear();
                tabControl_Tab_Dispose();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                if (Check_TextBox_Error() == false) return;

                //txt_P_1.Text = ""; txt_P_2.Text = ""; txt_P_3.Text = "";
                //txt_P_4.Text =""; txt_P_5.Text ="" ;txt_P_6.Text ="";
                //txt_P_7.Text ="";

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

            else if (bt.Name  == "butt_Exp")
            {
                if (bt.Text == "...")
                {
                    grB_Search.Height = button_base.Top + button_base.Height + 3;
                    bt.Text = ".";
                }
                else
                {
                    grB_Search.Height = butt_Exp.Top + butt_Exp.Height + 3;
                    bt.Text = "...";
                }
            }

        }


        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Stock_Center_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }

       

        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string Send_Nubmer = ""; string Send_Name = ""; ; string Send_OrderNumber = "";
                Send_OrderNumber = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                Send_Nubmer = (sender as DataGridView).CurrentRow.Cells[3].Value.ToString();
                Send_Name = (sender as DataGridView).CurrentRow.Cells[4].Value.ToString();
                Send_Mem_Number(Send_Nubmer, Send_Name, Send_OrderNumber);   //부모한테 이벤트 발생 신호한다.
            }            
        }


        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            //SendKeys.Send("{TAB}");
        }



        private void dGridView_Base_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            return;

            //dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cgb_Item.d_Grid_view_Header_Reset();

            //string CenterCode = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
            //string ItemCode = (sender as DataGridView).CurrentRow.Cells[3].Value.ToString();
            //string ItemName = (sender as DataGridView).CurrentRow.Cells[4].Value.ToString();

            //double T_SumCnt1 = double.Parse((sender as DataGridView).CurrentRow.Cells[5].Value.ToString());
            //T_SumCnt1 = T_SumCnt1 + double.Parse((sender as DataGridView).CurrentRow.Cells[6].Value.ToString());
            //T_SumCnt1 = T_SumCnt1 + double.Parse((sender as DataGridView).CurrentRow.Cells[7].Value.ToString());
            //T_SumCnt1 = T_SumCnt1 + double.Parse((sender as DataGridView).CurrentRow.Cells[8].Value.ToString());

            //T_SumCnt1 = T_SumCnt1 - double.Parse((sender as DataGridView).CurrentRow.Cells[9].Value.ToString());
            //T_SumCnt1 = T_SumCnt1 - double.Parse((sender as DataGridView).CurrentRow.Cells[14].Value.ToString());
            //T_SumCnt1 = T_SumCnt1 - double.Parse((sender as DataGridView).CurrentRow.Cells[15].Value.ToString());


            //Put_Stock_Detail(T_SumCnt1, CenterCode, ItemCode, ItemName);        
        }


        private void Put_Stock_Detail(double T_SumCnt, string CenterCode, string ItemCode, string ItemName)
        {   
            string strSql = "";

            string SDAte1 = ""; string SDate2 = "";
            SDAte1 = mtxtSDate.Text.Replace("-", "").Trim();

            if (mtxtSDate2.Text.Replace("-", "").Trim() == "")
                SDate2 = SDAte1;
            else
                SDate2 = mtxtSDate2.Text.Replace("-", "").Trim();                  

            strSql = "EXEC Usp_Stock_Sell_ALL_Center_02 'C' ,'" + CenterCode + "','" + ItemCode + "','" + ItemName + "','" + SDAte1 + "','" + SDate2 + "'"; 
         
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
              Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            double TSum = T_SumCnt;
            double T2 = 0; double T3 = 0; double T4 = 0; double T5 = 0; double T6 = 0; double T7 = 0; double T8 = 0;

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                T2 =  double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][1].ToString());
                T3 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][2].ToString());
                T4 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][3].ToString());
                T5 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][4].ToString());

                T6 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][5].ToString());
                T7 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][10].ToString());
                T8 = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][11].ToString());

                TSum = TSum + (T2 + T3 + T4 + T5) - (T6 + T7 + T8);

                Set_gr_Item(ref ds, ref gr_dic_text, fi_cnt, TSum);  //데이타를 배열에 넣는다.
            }

            cgb_Item.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Item.db_grid_Obj_Data_Put();    
        }


        
        //private void Item_Grid_Set()
        //{
        //    dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
        //    cgb_Item.d_Grid_view_Header_Reset();

        //    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        //    Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

        //    int fi_cnt = 0;
        //    foreach (int t_key in SalesItemDetail.Keys)
        //    {
        //        if (SalesItemDetail[t_key].Del_TF != "D")
        //            Set_gr_Item(ref gr_dic_text, t_key, fi_cnt);  //데이타를 배열에 넣는다.
        //        fi_cnt++;
        //    }

        //    cgb_Item.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
        //    cgb_Item.db_grid_Obj_Data_Put();
        //}


        private void Set_gr_Item(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt, double TSum )
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][7]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][10]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][11]  
                                ,TSum
                                ,""
                                ,""

                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Sell_Item_Header_Reset()
        {
            cgb_Item.Grid_Base_Arr_Clear();
            cgb_Item.basegrid = dGridView_Sell_Item;
            cgb_Item.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Item.grid_col_Count = 15;
            cgb_Item.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"일자"  , "일반입고"   , "교환입고"  , "_반품불량입고"   , "이동입고"        
                                , "주문출고"   , "주문출고직접"    , "주문출고배송"  , "주문출고센타" , "주문출고배송정보X"
                                , "소진출고"   , "이동출고"    , "재고량"  , "" , ""
                                };

            int[] g_Width = { 90, 100, 100, 100, 100
                                ,100 , 100 , 100 , 100 , 100
                                ,100 , 100 , 100 , 0 , 0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleRight 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight 
                                ,DataGridViewContentAlignment.MiddleRight  //10

                                ,DataGridViewContentAlignment.MiddleRight 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight 
                                ,DataGridViewContentAlignment.MiddleRight  //15
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[2 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            

            cgb_Item.grid_col_header_text = g_HeaderText;
            cgb_Item.grid_cell_format = gr_dic_cell_format;
            cgb_Item.grid_col_w = g_Width;
            cgb_Item.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                            
                                    ,true , true,  true,  true ,true   
                                   };
            cgb_Item.grid_col_Lock = g_ReadOnly;

            cgb_Item.basegrid.RowHeadersVisible = false;
        }
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail


        private void radioB_R_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtSDate, mtxtSDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }



        //private void tab_Center_Item_Chart_Make(Dictionary<string, string> dic_Center, DataSet ds, int ReCnt)
        //{
        //    cls_form_Meth cm = new cls_form_Meth();
        //    string Cen_Name = ""; string itemName = ""; int itemCnt = 0;
        //    string Cen_Code = "";
        //    int f_cnt = 0;

        //    foreach (string t_key in dic_Center.Keys)
        //    {
        //        Cen_Code = t_key;
        //        Cen_Name = dic_Center[t_key];

        //        if (f_cnt == 0)
        //        {
        //            chart_Item.Series.Clear();
        //            tabControl1.TabPages[0].Text = dic_Center[t_key];

        //            Series series_Day = new Series();
        //            if (Cen_Name == "") Cen_Name = "무";
        //            series_Day.Name = Cen_Name;
        //            series_Day["DrawingStyle"] = "Emboss";
        //            series_Day["PointWidth"] = "0.5";
        //            series_Day.Name = cm._chang_base_caption_search("현재고");
        //            series_Day.ChartType = SeriesChartType.Column;

        //            chart_Item.Series.Add(series_Day);

        //            chart_Item.ChartAreas[0].AxisX.Interval = 1;
        //            chart_Item.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
        //            chart_Item.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 7;

        //            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
        //            {
        //                if (ds.Tables[base_db_name].Rows[fi_cnt][0].ToString() == Cen_Code)
        //                {
        //                    itemName = ds.Tables[base_db_name].Rows[fi_cnt][3].ToString();
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

        //            t_ch.Name = t_key;
        //            t_tp.Text = dic_Center[t_key];
        //            t_tp.BackColor = tabControl1.TabPages[0].BackColor;
        //            t_tp.Controls.Add(t_ch);
        //            t_ch.Dock = DockStyle.Fill;
        //            t_ch.BackColor = chart_Item.BackColor;
        //            t_ch.BackGradientStyle = chart_Item.BackGradientStyle;

        //            t_ch.Legends.Add(t_le);
        //            t_ch.Legends[0].BackColor = chart_Item.Legends[0].BackColor;
        //            t_ch.Legends[0].BackGradientStyle = chart_Item.Legends[0].BackGradientStyle;

        //            t_ch.ChartAreas.Add(t_ca);
        //            t_ch.ChartAreas[0].BackColor = chart_Item.ChartAreas[0].BackColor;
        //            t_ch.ChartAreas[0].BackGradientStyle = chart_Item.ChartAreas[0].BackGradientStyle;

        //            tabControl1.Controls.Add(t_tp);

        //            Series series_Day = new Series();
        //            if (Cen_Name == "") Cen_Name = "무";
        //            series_Day.Name = Cen_Name;
        //            series_Day["DrawingStyle"] = "Emboss";
        //            series_Day["PointWidth"] = "0.5";
        //            series_Day.Name = cm._chang_base_caption_search("현재고");
        //            series_Day.ChartType = SeriesChartType.Column;

        //            t_ch.Series.Add(series_Day);

        //            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
        //            {
        //                if (ds.Tables[base_db_name].Rows[fi_cnt][0].ToString() == Cen_Code)
        //                {
        //                    itemName = ds.Tables[base_db_name].Rows[fi_cnt][3].ToString();
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
        //}

        private void tabControl_Tab_Dispose()
        {
            for (int fcnt = tabControl1.TabCount - 1; fcnt > 0; fcnt--)
            {
                tabControl1.TabPages[fcnt].Dispose();
            }

            tabControl1.TabPages[0].Text = "";
        }



        //private void Push_data(Series series, string p, double p_3)
        //{
        //    DataPoint dp = new DataPoint();
        //    dp.SetValueXY(p, p_3);
        //    dp.Label = string.Format(cls_app_static_var.str_Currency_Type, p_3); // p_3.ToString();                  
        //    series.Points.Add(dp);
        //}

        private int but_Exp_Base_Left = 0;
        private int Parent_but_Exp_Base_Width = 0;

        private void but_Exp_Click(object sender, EventArgs e)
        {
            if (but_Exp.Text == "<<")
            {
                Parent_but_Exp_Base_Width = but_Exp.Parent.Width;
                but_Exp_Base_Left = but_Exp.Left;

                but_Exp.Parent.Width = but_Exp.Width;
                but_Exp.Left = 0;
                but_Exp.Text = ">>";
            }
            else
            {
                but_Exp.Parent.Width = Parent_but_Exp_Base_Width;
                but_Exp.Left = but_Exp_Base_Left;
                but_Exp.Text = "<<";
            }
        }

        private void butt_Close_Click(object sender, EventArgs e)
        {
            if (dGridView_Base.RowCount == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Stock_Close")
                          + "\n" +
                         cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                return;
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            if (txtCenter3_Code.Text.Trim() == "")  //센타 선택 안하고 전체로 마감을 돌릴경우에
            {
                string strSql = "Select Isnull(Max(StockDay),'') , CenterCode From DayStock (nolock) Group By CenterCode";
                         
                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt > 0)
                {
                    string BaseDate = ds.Tables[base_db_name].Rows[0][0].ToString();

                    for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                    {
                        if (ds.Tables[base_db_name].Rows[fi_cnt][0].ToString() != BaseDate)
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Stock_Close_Err1")
                              + "\n" +
                              cls_app_static_var.app_msg_rm.GetString("Msg_Stock_Close_Err2")
                               +"\n" +
                             cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                            return;
                        }
                    }
                }
            }

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_Start"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                        
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();


            try
            {
                Close_Work_Real(Temp_Connect, Conn, tran);

                tran.Commit();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_End"));
            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Close_Err"));
            }

            finally
            {
                tran.Dispose(); Temp_Connect.Close_DB();
                this.Cursor = System.Windows.Forms.Cursors.Default;
                Clear_Object_();
            }                        
        }


        //string[] g_HeaderText = { "센타코드"   , "센타명" , "상품코드"   , "상품명"  , "전일_재고"      
        //                        , "일반_입고"   , "반품_입고"    , "_ChangeInQty"   , "이동_입고"    , "입고_합계"

        //                        , "  "   , "판매_출고"  , "_ChangeOrderOutQty"   , "_DisuseOutQty"   ,"_ReSendOutQty"
        //                        , "이동_출고"     , "기타_출고"    , "출고_합계" , "_pooraddqty"     , "당일_재고"
        //                        , "", ""  , ""  , ""  ,""
                               
        //                            };

        private void Close_Work_Real(cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran)
        {
            Dictionary<string, int> gr_dic_text = new Dictionary<string, int>();

            string StockDay = "", CenterCode = "", ItemCode = "";
            int  InQty = 0, ReturnInQty  = 0, ChangeInQty  = 0, MoveInQty  = 0 ;
            int SaleOutQty  = 0, ChangeOrderOutQty  = 0, DisuseOutQty  = 0, ReSendOutQty  = 0, GOutQty  = 0, MoveOutQty  = 0, EtcOutQty  = 0 ;
            int pooraddqty = 0, ToDayStock = 0, Pre_Chang_StockQty = 0, Pre_Chang_StockQty_TF = 0 ;
            int PreStockQty = 0;
            StockDay = mtxtSDate2.Text.Replace("-", "").Trim();
            string strSQL = "";

            for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            {

                CenterCode = dGridView_Base.Rows[i].Cells[0].Value.ToString() ;
                ItemCode = dGridView_Base.Rows[i].Cells[2].Value.ToString();
                PreStockQty = int.Parse(dGridView_Base.Rows[i].Cells[4].Value.ToString());


                InQty = int.Parse(dGridView_Base.Rows[i].Cells[5].Value.ToString());

                ReturnInQty = int.Parse(dGridView_Base.Rows[i].Cells[6].Value.ToString());
                ChangeInQty = 0;
                MoveInQty = int.Parse(dGridView_Base.Rows[i].Cells[8].Value.ToString());


                SaleOutQty = int.Parse(dGridView_Base.Rows[i].Cells[11].Value.ToString());
                ChangeOrderOutQty = 0;
                DisuseOutQty = 0;
                ReSendOutQty = 0;
                GOutQty = 0;
                MoveOutQty = int.Parse(dGridView_Base.Rows[i].Cells[15].Value.ToString());
                EtcOutQty = int.Parse(dGridView_Base.Rows[i].Cells[16].Value.ToString());
                pooraddqty = 0;

                ToDayStock = int.Parse(dGridView_Base.Rows[i].Cells[19].Value.ToString());

                Pre_Chang_StockQty = int.Parse(dGridView_Base.Rows[i].Cells[22].Value.ToString());
                Pre_Chang_StockQty_TF = int.Parse(dGridView_Base.Rows[i].Cells[23].Value.ToString());


                strSQL = "Insert into DayStock (StockDay,CenterCode , ItemCode ,PreStockQty , ";
                strSQL = strSQL + " InQty, ReturnInQty , ChangeInQty , MoveInQty ,  ";        
                strSQL = strSQL + " SaleOutQty, ChangeOrderOutQty , DisuseOutQty , ReSendOutQty , GOutQty , MoveOutQty, EtcOutQty , pooraddqty , ToDayStock , ";
                strSQL = strSQL + " Pre_Chang_StockQty , Pre_Chang_StockQty_TF , RegiUser )";        
                strSQL = strSQL + " Values ";

                strSQL = strSQL + " ('" + StockDay + "'";   //'등록 일자 Stock_Date
                strSQL = strSQL + ",'" + CenterCode + "'";     //''센타/창고 코드 번호  Dep_Cd
                strSQL = strSQL + ",'" + ItemCode + "'";   // '센타/창고 구분자 c:센타  w:창고  Code_FL
                strSQL = strSQL + "," + PreStockQty;
                strSQL = strSQL + "," + InQty;
                strSQL = strSQL + "," + ReturnInQty;
                strSQL = strSQL + "," + ChangeInQty;
                strSQL = strSQL + "," + MoveInQty;        
        
                strSQL = strSQL + "," + SaleOutQty;
                strSQL = strSQL + "," + ChangeOrderOutQty;
                strSQL = strSQL + "," + DisuseOutQty;
                strSQL = strSQL + "," + ReSendOutQty;
                strSQL = strSQL + "," + GOutQty;
        
                strSQL = strSQL + "," + MoveOutQty;
                strSQL = strSQL + "," + EtcOutQty;        
        
                strSQL = strSQL + "," + pooraddqty;
                strSQL = strSQL + "," + ToDayStock;

                strSQL = strSQL + "," + Pre_Chang_StockQty;
                strSQL = strSQL + "," + Pre_Chang_StockQty_TF;
        
                strSQL = strSQL + ",'" + cls_User.gid  + "' " ;
                strSQL = strSQL + ")";

                Temp_Connect.Insert_Data(strSQL, base_db_name, Conn, tran);


                gr_dic_text[CenterCode] = 0;
                
            }


            foreach (string  t_for_key in gr_dic_text.Keys)
            {
                strSQL = "Insert into CloseLog ( CloseDay , LogType , RegiUser , CenterCode , RegiDay ) Values (";
                strSQL = strSQL + "'" + StockDay + "','재고','" + cls_User.gid + "','" + t_for_key + "' ,  Convert(Varchar(25),GetDate(),21) )";

                Temp_Connect.Insert_Data(strSQL, base_db_name, Conn, tran);

            }
            
        }



        private void Search_Stock_Close_Date() 

        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string R_Stock_Day = "";
            string StockDay ="";
            if (txtCenter3_Code.Text.Trim() == "")  //센타 선택 안하고 전체로 마감을 돌릴경우에
            {
                string strSql = "Select Isnull(Max(StockDay),'') , CenterCode From DayStock (nolock) Group By CenterCode";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt > 0)
                {
                    string BaseDate = ds.Tables[base_db_name].Rows[0][0].ToString();

                    for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                    {
                        if (ds.Tables[base_db_name].Rows[fi_cnt][0].ToString() != BaseDate)
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Stock_Close_Err1")
                              + "\n" +
                              cls_app_static_var.app_msg_rm.GetString("Msg_Stock_Close_Err2")
                               + "\n" +
                             cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                            return;
                        }
                    }

                    StockDay = BaseDate;

                    if (StockDay != "")
                    {
                        //StockDay = 
                        string PayDate = "";

                        PayDate = StockDay.Substring(0, 4) + '-' + StockDay.Substring(4, 2) + '-' + StockDay.Substring(6, 2);
                        DateTime TodayDate = new DateTime();
                        TodayDate = DateTime.Parse(PayDate);
                        StockDay = TodayDate.AddDays(1).ToString("yyyy-MM-dd").Replace("-", "");

                        mtxtSDate.Text = StockDay;
                        mtxtSDate2.Text = StockDay;
                    }
                }
                else                 
                {
                    string in_Date = "", Out_DAte = ""; 
                    strSql = "Select Isnull(Min(In_Date),'') From tbl_StockInput (nolock) ";

                    DataSet ds2 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds2, this.Name, this.Text) == false) return;
                    int ReCnt2 = Temp_Connect.DataSet_ReCount;

                    if (ReCnt2 > 0)
                    {
                        in_Date = ds2.Tables[base_db_name].Rows[0][0].ToString();
                    }

                    strSql = "Select Isnull(Min(Out_Date),'') From tbl_StockOutput (nolock) ";

                    DataSet ds3 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds3, this.Name, this.Text) == false) return;
                    int ReCnt3 = Temp_Connect.DataSet_ReCount;

                    if (ReCnt3 > 0)
                    {
                        Out_DAte = ds3.Tables[base_db_name].Rows[0][0].ToString();
                    }


                    strSql = "Select Isnull(Min(StockDay),'') From DayStock_Real (nolock) ";

                    DataSet ds4 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds4, this.Name, this.Text) == false) return;
                    int ReCnt4 = Temp_Connect.DataSet_ReCount;

                    if (ReCnt4 > 0)
                    {
                        R_Stock_Day = ds4.Tables[base_db_name].Rows[0][0].ToString();
                    }

                    if (R_Stock_Day != "")
                        StockDay = R_Stock_Day;
                    else
                    {
                        if (in_Date != "" && Out_DAte != "")
                        {
                            if (int.Parse (in_Date) >= int.Parse (Out_DAte))
                                StockDay = Out_DAte;
                            else
                                StockDay = in_Date;

                        }
                        else
                        {
                            if (in_Date == "" && Out_DAte != "")
                                StockDay = Out_DAte;

                            if (in_Date != "" && Out_DAte == "")
                                StockDay = in_Date;
                        }
                    }

                    if (StockDay != "")
                    {
                        //StockDay = 
                        string PayDate = "";

                        PayDate = StockDay.Substring(0, 4) + '-' + StockDay.Substring(4, 2) + '-' + StockDay.Substring(6, 2);
                        DateTime TodayDate = new DateTime();
                        TodayDate = DateTime.Parse(PayDate);
                        StockDay = TodayDate.AddDays(0).ToString("yyyy-MM-dd").Replace("-", "");

                        mtxtSDate.Text = StockDay;
                        mtxtSDate2.Text = StockDay;
                    }
                    
                }

            }
            else
            {
                string strSql = "Select StockDay From DayStock (nolock) ";
                strSql = strSql + " Where  CenterCode ='" + txtCenter3_Code.Text + "'";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt > 0)
                {
                    strSql = "Select Isnull(Max(StockDay),'') From DayStock (nolock) ";
                    strSql = strSql + " Where  CenterCode ='" + txtCenter3_Code.Text + "'";

                    DataSet ds2 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds2, this.Name, this.Text) == false) return;
                    int ReCnt2 = Temp_Connect.DataSet_ReCount;

                    StockDay = ds2.Tables[base_db_name].Rows[0][0].ToString().Trim();

                    if (StockDay != "")
                    {
                        //StockDay = 
                        string PayDate = "";

                        PayDate = StockDay.Substring(0, 4) + '-' + StockDay.Substring(4, 2) + '-' + StockDay.Substring(6, 2);
                        DateTime TodayDate = new DateTime();
                        TodayDate = DateTime.Parse(PayDate);
                        StockDay = TodayDate.AddDays(1).ToString("yyyy-MM-dd").Replace("-", "");

                        mtxtSDate.Text = StockDay;
                        mtxtSDate2.Text = StockDay;
                    }
                }

                else
                {
                    string in_Date = "", Out_DAte = "";
                    strSql = "Select Isnull(Min(In_Date),'') From tbl_StockInput (nolock) ";

                    DataSet ds2 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds2, this.Name, this.Text) == false) return;
                    int ReCnt2 = Temp_Connect.DataSet_ReCount;

                    if (ReCnt2 > 0)
                    {
                        in_Date = ds2.Tables[base_db_name].Rows[0][0].ToString().Trim();
                    }

                    strSql = "Select Isnull(Min(Out_Date),'') From tbl_StockOutput (nolock) ";

                    DataSet ds3 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds3, this.Name, this.Text) == false) return;
                    int ReCnt3 = Temp_Connect.DataSet_ReCount;

                    if (ReCnt3 > 0)
                    {
                        Out_DAte = ds3.Tables[base_db_name].Rows[0][0].ToString().Trim();
                    }


                    strSql = "Select Isnull(Min(StockDay),'') From DayStock_Real (nolock) ";

                    DataSet ds4 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(strSql, base_db_name, ds4, this.Name, this.Text) == false) return;
                    int ReCnt4 = Temp_Connect.DataSet_ReCount;

                    if (ReCnt4 > 0)
                    {
                        R_Stock_Day = ds4.Tables[base_db_name].Rows[0][0].ToString().Trim();
                    }

                    if (R_Stock_Day != "")
                        StockDay = R_Stock_Day;
                    else
                    {
                        if (in_Date != "" && Out_DAte != "")
                        {
                            if (int.Parse(in_Date) >= int.Parse(Out_DAte))
                                StockDay = Out_DAte;
                            else
                                StockDay = in_Date;

                        }
                        else
                        {
                            if (in_Date == "" && Out_DAte != "")
                                StockDay = Out_DAte;

                            if (in_Date != "" && Out_DAte == "")
                                StockDay = in_Date;
                        }
                    }

                    if (StockDay != "")
                    {
                        //StockDay = 
                        string PayDate = "";

                        PayDate = StockDay.Substring(0, 4) + '-' + StockDay.Substring(4, 2) + '-' + StockDay.Substring(6, 2);
                        DateTime TodayDate = new DateTime();
                        TodayDate = DateTime.Parse(PayDate);
                        StockDay = TodayDate.AddDays(0).ToString("yyyy-MM-dd").Replace("-", "");

                        mtxtSDate.Text = StockDay;
                        mtxtSDate2.Text = StockDay;
                    }
                    
                }



               

            }


            



        }

        private void button_Date_Click(object sender, EventArgs e)
        {
            if (mtxtSDate.Text.Replace("-", "").Trim() != "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Stock_Close_Err3")
                              + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                return;
            }

            //날짜 셋팅이 없다는 거는 센타별로 돌린다고 볼수 잇기 때문에 이런경우에는 센타를 넣고 조회를 누르면 자동 셋팅 되도록 하기 위함이다.
            if (mtxtSDate.Text.Replace("-", "").Trim() == "" && mtxtSDate2.Text.Replace("-", "").Trim() == "")
            {
                Search_Stock_Close_Date();
                mtxtSDate2.Focus();
                return;
            }
        }





    }
}
