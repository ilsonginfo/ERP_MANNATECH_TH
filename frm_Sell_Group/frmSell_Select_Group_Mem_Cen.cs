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
    public partial class frmSell_Select_Group_Mem_Cen : Form
    {
       

        
        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Item = new cls_Grid_Base();
        
        private const string base_db_name = "tbl_SalesDetail";
        private int Data_Set_Form_TF;
               
        Series series_Item = new Series();
        Series series_PV = new Series();
        
        public frmSell_Select_Group_Mem_Cen()
        {
            InitializeComponent();
        }

        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();
        }


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Item.d_Grid_view_Header_Reset();
         
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_SellCode_ComboBox(combo_Se, combo_Se_Code);


            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;


            if (cls_app_static_var.Using_Mileage_TF == 0)
            {
                tableLayoutPanel20.Visible = false;
                tableLayoutPanel17.Visible = false;
            }
        }

        private void frmBase_Resize(object sender, EventArgs e)
        {
            int base_w = this.Width / 4;
            butt_Clear.Width = base_w;
            butt_Select.Width = base_w;
            butt_Excel.Width = base_w;
            //butt_Delete.Width = base_w;
            butt_Exit.Width = base_w;

            butt_Clear.Left = 0;
            butt_Select.Left = butt_Clear.Left + butt_Clear.Width;
            butt_Excel.Left = butt_Select.Left + butt_Select.Width;
            //butt_Delete.Left = butt_Excel.Left + butt_Excel.Width;
            butt_Exit.Left = butt_Excel.Left + butt_Excel.Width;
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
            if (e.KeyValue == 113)
                T_bt = butt_Select;     //조회  F1
            if (e.KeyValue == 115)
                T_bt = butt_Delete;   // 삭제  F4
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


            if (txtSellDate1.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtSellDate1);

                if (Ret == -1)
                {
                    txtSellDate1.Focus(); return false;
                }   
            }

            if (txtSellDate2.Text.Trim() != "")
            {
                int Ret = 0;
                Ret = c_er.Input_Date_Err_Check(txtSellDate2);

                if (Ret == -1)
                {
                    txtSellDate2.Focus(); return false;
                } 
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

            //string[] g_HeaderText = {"센타_코드"  , "센타명"   , "주문액"  , "주문PV"   , "현금"        
            //                    , "카드"   , "무통장"    , "미결제"   , ""    , ""                                
            //                        };

            Tsql = "Select  tbl_Business.Ncode   ";
            Tsql = Tsql + " ,Isnull(tbl_Business.Name,'')  ";
            Tsql = Tsql + " ,Sum(TotalPrice)  ";
            Tsql = Tsql + " ,Sum(TotalPv)  ";
            Tsql = Tsql + " ,Sum(InputCash)  ";
            Tsql = Tsql + " ,Sum(InputCard)  ";
            Tsql = Tsql + " ,Sum(InputPassbook)  ";
            Tsql = Tsql + " ,Sum(InputMile)  ";
            Tsql = Tsql + " ,Sum(UnaccMoney)  ";
            Tsql = Tsql + " , '' ";
            Tsql = Tsql + " From tbl_SalesDetail  (nolock) ";
            Tsql = Tsql + " LEFT Join tbl_Memberinfo  (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";            
            Tsql = Tsql + " Left Join tbl_SellType  (nolock) On tbl_SellType.SellCode=tbl_SalesDetail.SellCode  ";
            Tsql = Tsql + " Left JOIN tbl_Business (nolock) ON  tbl_Memberinfo.businesscode = tbl_Business.ncode  And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";

        }



        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = " Where tbl_SalesDetail.Mbid2 >= 0  ";            
                
            //가입일자로 검색 -1
            if ((txtSellDate1.Text.Trim() != "") && (txtSellDate2.Text.Trim() == ""))
                strSql = strSql + " And tbl_SalesDetail.SellDate = '" + txtSellDate1.Text.Trim() + "'";

            //가입일자로 검색 -2
            if ((txtSellDate1.Text.Trim() != "") && (txtSellDate2.Text.Trim() != ""))
            {
                strSql = strSql + " And tbl_SalesDetail.SellDate >= '" + txtSellDate1.Text.Trim() + "'";
                strSql = strSql + " And tbl_SalesDetail.SellDate <= '" + txtSellDate2.Text.Trim() + "'";
            }
            

            if (txtCenter2_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.BusCode = '" + txtCenter2_Code.Text.Trim() + "'";

            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.businesscode = '" + txtCenter_Code.Text.Trim() + "'";


            if (txtSellCode_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.SellCode = '" + txtSellCode_Code.Text.Trim() + "'";
                       

            strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
          //  strSql = strSql + " And tbl_SalesDetail.BusCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";

            strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


            if (chk_Re.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.ReturnTF > 0 ";
            else
                strSql = strSql + " And tbl_SalesDetail.ReturnTF = 1  ";

            Tsql = Tsql + strSql ;            
        }




        private void Base_Grid_Set()
        {   
            string Tsql = "";            
            Make_Base_Query(ref Tsql);

            Make_Base_Query_(ref Tsql);

            Tsql = Tsql + " Group By tbl_Business.Ncode , Isnull(tbl_Business.Name,'') ";
            Tsql = Tsql + " Order By tbl_Business.Ncode , Isnull(tbl_Business.Name,'')   ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();                                  
            
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name , this.Text ) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            double Sum_04 = 0; double Sum_05 = 0; double Sum_03 = 0;
            double Sum_06 = 0; double Sum_07 = 0; double Sum_08 = 0; double Sum_09 = 0;
            //double Sum_16 = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            //Dictionary<int, string[]> gr_dic_text = new Dictionary<int, string[]>();

            chart_Item.Series.Clear();
            Save_Nom_Line_Chart();
            string Cen_Name = ""; double Cen_PR = 0; double Cen_PV = 0;
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                Cen_Name = ds.Tables[base_db_name].Rows[fi_cnt][1].ToString();
                Cen_PR = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][2].ToString());
                Cen_PV = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][3].ToString());
               // chart_Item.Series.Clear();
                Push_data(series_Item, Cen_Name, Cen_PR);
                //Push_data(Cen_Name, Cen_PV);

                Sum_03 = Sum_03 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][2].ToString());
                Sum_04 = Sum_04 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][3].ToString());
                Sum_05 = Sum_05 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][4].ToString());
                Sum_06 = Sum_06 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][5].ToString());
                Sum_07 = Sum_07 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][6].ToString());
                Sum_08 = Sum_08 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][8].ToString());
                Sum_09 = Sum_09 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][7].ToString());                
            }



            if (gr_dic_text.Count > 0)
            {
                txt_P_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_03);
                txt_P_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_04);
                txt_P_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_05);
                txt_P_4.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_06);
                txt_P_5.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_07);
                txt_P_6.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_08);
                txt_P_7.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_09);                
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();          
        }



        private void dGridView_Base_Header_Reset()
        {
            
            cgb.grid_col_Count = 10;            
            cgb.basegrid = dGridView_Base;            
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            //cgb_Item.grid_Frozen_End_Count = 3;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            

            string[] g_HeaderText = {"센타_코드"  , "센타명"   , "주문액"  , "주문PV"   , "현금"        
                                , "카드"   , "무통장"    , "마일리지"   , "미결제"    , ""                                
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            if (cls_app_static_var.Using_Mileage_TF == 1)
            {
                int[] g_Width = { 90, 100, 80, 80, 80                                                          
                                 ,80 , 80 , 80 , 80 , 0                             
                                };
                cgb.grid_col_w = g_Width;
            }
            else
            {
                int[] g_Width = { 90, 100, 80, 80, 80                                                          
                                 ,80 , 80 , 0 , 80 , 0                             
                                };
                cgb.grid_col_w = g_Width;
            }
            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                        
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight  //5
                               
                               ,DataGridViewContentAlignment.MiddleRight                              
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleCenter //10
                           
                              };
            cgb.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = "###,###,###,##0";
            gr_dic_cell_format[4 - 1] = "###,###,###,##0";
            gr_dic_cell_format[5 - 1] = "###,###,###,##0";
            gr_dic_cell_format[6 - 1] = "###,###,###,##0";
            gr_dic_cell_format[7 - 1] = "###,###,###,##0";
            gr_dic_cell_format[8 - 1] = "###,###,###,##0";
            gr_dic_cell_format[9 - 1] = "###,###,###,##0";

            cgb.grid_cell_format = gr_dic_cell_format;
            cgb.basegrid.RowHeadersVisible = false;
            
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


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, string[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;

            string[] row0 = new string[cgb.grid_col_Count];
            string t_caption = "";
            while (Col_Cnt < cgb.grid_col_Count)
            {
                t_caption= ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt].ToString ();

                if (Col_Cnt ==2 || Col_Cnt ==3 || Col_Cnt ==4 )
                    t_caption = string.Format(cls_app_static_var.str_Currency_Type, int.Parse(t_caption));

                row0[Col_Cnt] = t_caption ;
                Col_Cnt++;
            }


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

            if (tb.Name == "txtBank")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtSellCode_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtR_Id")
            {
                if (tb.Text.Trim() == "")
                    txtR_Id_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtR_Id_Code);
            }

            if (tb.Name == "txtCenter2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter2_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtSellCode")
            {
                if (tb.Text.Trim() == "")
                    txtSellCode_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtSellCode_Code);
            }
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
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, txtCenter_Code,"");
                else
                    Ncod_Text_Set_Data(tb, txtCenter_Code);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtR_Id")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, txtR_Id_Code, "");
                else
                    Ncod_Text_Set_Data(tb, txtR_Id_Code);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtBank")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, txtSellCode_Code, "");
                else
                    Ncod_Text_Set_Data(tb, txtSellCode_Code);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtCenter2")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, txtCenter2_Code, "");
                else
                    Ncod_Text_Set_Data(tb, txtCenter2_Code);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtSellCode")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, txtSellCode_Code, "");
                else
                    Ncod_Text_Set_Data(tb, txtSellCode_Code);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }
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

                if (tb.Name == "txtCenter2")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);
           
                if (tb.Name == "txtSellCode")
                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", strSql);
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

                if (tb.Name == "txtCenter2")
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


            if (tb.Name == "txtCenter2")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            }
          

            if (tb.Name == "txtSellCode")
            {
                Tsql = "Select SellCode ,SellTypeName    ";
                Tsql = Tsql + " From tbl_SellType (nolock) ";
                Tsql = Tsql + " Where SellCode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    SellTypeName like '%" + tb.Text.Trim() + "%'";
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

                dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_Item.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, txtSellDate1);

                chart_Item.Series.Clear();
                
                opt_Ed_1.Checked = true;  opt_sell_1.Checked = true;
                chk_Re.Checked = false;
                //radioB_S.Checked = true;
                combo_Se.SelectedIndex = -1;

            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                
                dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb_Item.d_Grid_view_Header_Reset();

                chart_Item.Series.Clear();

                txt_PP_1.Text = ""; txt_PP_2.Text = ""; txt_PP_3.Text = "";
                txt_PP_3.Text = ""; txt_PP_4.Text = ""; txt_PP_5.Text = ""; txt_PP_7.Text = "";

                txt_P_1.Text = ""; txt_P_2.Text = ""; txt_P_3.Text = "";
                txt_P_4.Text = ""; txt_P_5.Text = ""; txt_P_6.Text = ""; txt_P_7.Text = "";
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                if (Check_TextBox_Error() == false) return;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex;
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
            Excel_Export_File_Name = this.Text; // "Sell_Group_Center";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }

       

        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            dGridView_Sell_Item_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Item.d_Grid_view_Header_Reset();

            txt_PP_1.Text = ""; txt_PP_2.Text = ""; txt_PP_3.Text = "";
            txt_PP_3.Text = ""; txt_PP_4.Text = ""; txt_PP_5.Text = ""; 

            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                string ItemCode = ""; 
                ItemCode = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Item_Grid_Set(ItemCode);
                this.Cursor = System.Windows.Forms.Cursors.Default; 
            }            
        }


        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);       
        }





        private void Item_Grid_Set(string ItemCode )
        {
            

                       
            string Tsql = "";

            //string[] g_HeaderText = {"주문번호"  , "주문일자"   , "주문종류"  , "회원_번호"   , "성명"        
            //                    , "회원_센타명"   , "주문액"    , "주문PV"  , "현금" , "카드"
            //                    ,"무통장" , "미결제" , "주문_센타명" , "", "" 
            //                    };


            Tsql = "Select tbl_SalesDetail.OrderNumber ";            
            Tsql = Tsql + " ,tbl_SalesDetail.SellDate ";
            Tsql = Tsql + " ,tbl_SellType.SellTypeName ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", tbl_SalesDetail.mbid + '-' + Convert(Varchar,tbl_SalesDetail.mbid2) ";
            else
                Tsql = Tsql + ", tbl_SalesDetail.mbid2 ";
            Tsql = Tsql + " , tbl_SalesDetail.M_Name ";

            Tsql = Tsql + " , Isnull( tbl_Business.Name ,'') ";

            Tsql = Tsql + " ,TotalPrice ";
            Tsql = Tsql + " ,TotalPv ";
            Tsql = Tsql + " ,InputCash ";
            Tsql = Tsql + " ,InputCard";
            Tsql = Tsql + " ,InputPassbook ";
            Tsql = Tsql + " ,InputMile ";
            Tsql = Tsql + " ,UnaccMoney ";

            Tsql = Tsql + " , Isnull( S_B.Name ,'') ";

            Tsql = Tsql + " ,''  ";            

            Tsql = Tsql + " From tbl_SalesDetail   (nolock) ";
            Tsql = Tsql + " LEFT Join tbl_Memberinfo  (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";            
            Tsql = Tsql + " Left Join tbl_SellType  (nolock) On tbl_SellType.SellCode=tbl_SalesDetail.SellCode  ";
            Tsql = Tsql + " Left Join tbl_Business AS S_B (nolock) On S_B.Ncode=tbl_SalesDetail.BusCode And tbl_SalesDetail.Na_code = S_B.Na_code ";
            Tsql = Tsql + " Left Join tbl_Business  (nolock) On tbl_Business.Ncode=tbl_Memberinfo.BusinessCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";
            
            Make_Base_Query_(ref Tsql);

            Tsql = Tsql + " And  tbl_Memberinfo.BusinessCode = '" + ItemCode + "'";
            Tsql = Tsql + " Order by  tbl_SalesDetail.OrderNumber  ";
         

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            double Sum_07 = 0; double Sum_08 = 0; double Sum_09 = 0; double Sum_13 = 0;
            double Sum_10 = 0; double Sum_11 = 0; double Sum_12 = 0;  
            //double Sum_16 = 0;
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_Item(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
               
              
                Sum_07 = Sum_07 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][6].ToString());
                Sum_08 = Sum_08 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][7].ToString());
                Sum_09 = Sum_09 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][8].ToString());
                Sum_10 = Sum_10 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][9].ToString());
                Sum_11 = Sum_11 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][10].ToString());
                Sum_12 = Sum_12 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][12].ToString());
                Sum_13 = Sum_13 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][11].ToString());
            }


            if (gr_dic_text.Count > 0)
            {
                txt_PP_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_07);
                txt_PP_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_08);
                txt_PP_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_09);
                txt_PP_4.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_10);
                txt_PP_5.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_11);
                txt_PP_6.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_12);
                txt_PP_7.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_13);
            }
                       

            cgb_Item.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Item.db_grid_Obj_Data_Put();
        }


        private void Set_gr_Item(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;

            object[] row0 = new object[cgb_Item.grid_col_Count];

            while (Col_Cnt < cgb_Item.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }


            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Sell_Item_Header_Reset()
        {
            cgb_Item.Grid_Base_Arr_Clear();
            cgb_Item.basegrid = dGridView_Base_Sub;
            cgb_Item.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Item.grid_col_Count = 15;

            cgb_Item.grid_Frozen_End_Count = 2;
            cgb_Item.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            if (cls_app_static_var.Using_Mileage_TF == 0)
            {
                string[] g_HeaderText = {"주문번호"  , "주문일자"   , "주문종류"  , "회원_번호"   , "성명"        
                                , "회원_센타명"   , "주문액"    , "주문PV"  , "현금" , "카드"
                                ,"무통장" , "" , "미결제" , "주문_센타명", "" 
                                };
                cgb_Item.grid_col_header_text = g_HeaderText;

                int[] g_Width = { 100, 0, 90, 80, 0
                            ,0 , 70 , 70 , 70 , 70
                            ,70 , 0 , 70 , 100 , 0
                            };

                cgb_Item.grid_col_w = g_Width;
            }
            else
            {
                string[] g_HeaderText = {"주문번호"  , "주문일자"   , "주문종류"  , "회원_번호"   , "성명"        
                                , "회원_센타명"   , "주문액"    , "주문PV"  , "현금" , "카드"
                               ,"무통장" , "마일리지" , "미결제" , "주문_센타명", "" 
                                };
                cgb_Item.grid_col_header_text = g_HeaderText;

                int[] g_Width = { 100, 0, 90, 80, 0
                            ,0 , 70 , 70 , 70 , 70
                            ,70 , 70 , 70 , 100 , 0
                            };

                cgb_Item.grid_col_w = g_Width;
            }

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight 
                                ,DataGridViewContentAlignment.MiddleRight  //10


                                ,DataGridViewContentAlignment.MiddleRight 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleCenter  //15
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;


            
            cgb_Item.grid_cell_format = gr_dic_cell_format;
            
            cgb_Item.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                            
                                    ,true , true,  true,  true ,true                                                                                               
                                   };
            cgb_Item.grid_col_Lock = g_ReadOnly;
            
        }
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail











        private void Push_data(Series series, string p, double  p_3)
        {
            

            if (p != "")
            {
                DataPoint dp = new DataPoint();

                if (p.Replace (" ","").Length >= 4)
                    dp.SetValueXY(p.Replace (" ","").Substring(0, 4), p_3);
                else
                    dp.SetValueXY(p, p_3);

                dp.Font = new System.Drawing.Font("맑은고딕", 9);
                dp.Label = string.Format(cls_app_static_var.str_Currency_Type, p_3); // p_3.ToString();                  
                series.Points.Add(dp);
            }
            
        }


        private void radioB_S_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(txtSellDate1, txtSellDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }





        private void Push_data( string p, double p_3)
        {


            if (p != "")
            {
                DataPoint dp = new DataPoint();

                if (p.Replace(" ", "").Length >= 4)
                    dp.SetValueXY(p.Replace(" ", "").Substring(0, 4), p_3);
                else
                    dp.SetValueXY(p, p_3);

                dp.Font = new System.Drawing.Font("맑은고딕", 9);
                dp.Label = string.Format(cls_app_static_var.str_Currency_Type, p_3); // p_3.ToString();                  
                series_PV.Points.Add(dp);
            }

        }

        //Push_data(series_Item, nodeKey.ToString() + "Line", Save_Cnt[nodeKey]);
        private void Save_Nom_Line_Chart()
        {
            cls_form_Meth cm = new cls_form_Meth();            

            chart_Item.Series.Clear();
            series_Item.Points.Clear();
            //series_PV.Points.Clear();


            series_Item["DrawingStyle"] = "Emboss";
            series_Item["PointWidth"] = "0.2";
            series_Item.Name = cm._chang_base_caption_search("금액");
            
            //series_PV["DrawingStyle"] = "Emboss";
            //series_PV["PointWidth"] = "0.2";
            //series_PV.Name = cm._chang_base_caption_search("PV");
            
            //series_Item.ChartArea = "ChartArea1";
            series_Item.ChartType = SeriesChartType.Column;
           // series_PV.ChartType = SeriesChartType.Column;
           // series_Item.Legend = "Legend1";            
            
            
            chart_Item.Series.Add(series_Item);
            //chart_Item.Series.Add(series_PV);
            chart_Item.ChartAreas[0].AxisX.Interval = 1;
            chart_Item.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_Item.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 8;

            chart_Item.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_Item.Legends[0].Enabled = true;
            
        }
















    }
}
