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
    public partial class frmBase_User : Form
    {

        private string base_db_name = "tbl_User";
        private int Data_Set_Form_TF = 0;
        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Excel = new cls_Grid_Base();
        cls_Grid_Base cgb_Login = new cls_Grid_Base();

        Dictionary<string, cls_tbl_User> dic_tbl_User = new Dictionary<string, cls_tbl_User>();  //사용자 관련 정보를 클래스를 통해서. 넣는다.

        Dictionary<string, TreeNode> dic_Tree_Sort_1 = new Dictionary<string, TreeNode>();  //상품 코드 분류상 대분류 관련 트리노드를 답는곳
        Dictionary<string, TreeNode> dic_Tree_Sort_2 = new Dictionary<string, TreeNode>();  //상품 코드 분류상 중분류 관려련 트리 노드를 답는곳

        Dictionary<string, TreeView> dic_Tree_view = new Dictionary<string, TreeView>();  //상품 코드 분류상 대분류 관련 트리노드를 답는곳

        //Dictionary<string, Boolean> Main_Menu = new Dictionary<string, Boolean>();

        //public delegate void Send_MainMenu_Info_Dele(ref Dictionary<string, Boolean> Main_Menu);
        //public event Send_MainMenu_Info_Dele Send_MainMenu_Info;
        




        public frmBase_User()
        {
            InitializeComponent();
        }




        private void frm_Base_Activated(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void frmBase_From_Load(object sender, EventArgs e)
        {
            Data_Set_Form_TF = 0;

            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_NaCode_ComboBox(combo_Se, combo_Se_Code);


            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);            
            

            
            //dGridView_Login_Header_Reset();
            //cgb_Login.d_Grid_view_Header_Reset();

            //dGridView_Excel_Header_Reset();
            //cgb_Excel.d_Grid_view_Header_Reset();


            //Send_MainMenu_Info(ref  Main_Menu);

            trv_Item_Set_Sort_Code();

            if (dic_tbl_User != null)
                dic_tbl_User.Clear();

            Set_Tbl_User();  

            if (dic_tbl_User != null)
                Base_Grid_Set();

            if (cls_app_static_var.Program_User_Center_Sort == 0) //센타 프로그램을 사용하지 못하면.  소속 센타 관련해서 보여주지 않는다.
                tableLayoutPanel21.Visible = false;

            mtxtTel1.Mask = cls_app_static_var.Tel_Number_Fromat;

            txtD1.BackColor = cls_app_static_var.txt_Enable_Color;
            txtD2.BackColor = cls_app_static_var.txt_Enable_Color;

        }

        private void frmBase_Resize(object sender, EventArgs e)
        {
            //butt_Exit.Left = this.Width - butt_Exit.Width - 20;

            //butt_Clear.Left = 3;
            //butt_Save.Left = butt_Clear.Left + butt_Clear.Width + 2;
            ////butt_Excel.Left = butt_Save.Left + butt_Save.Width + 2;
            //butt_Delete.Left = butt_Save.Left + butt_Save.Width + 2;
            ////this.Refresh();


            butt_Clear.Left = 0;
            butt_Save.Left = butt_Clear.Left + butt_Clear.Width + 2;
            //butt_Excel.Left = butt_Save.Left + butt_Save.Width + 2;
            butt_Delete.Left = butt_Save.Left + butt_Save.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Save);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);
            cfm.button_flat_change(button_LogOut);
            

        }



        private void DataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            //그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
            if (sender is DataGridView)
            {

                if (e.KeyValue == 13)
                {
                    EventArgs ee = null;
                    dGridView_Base_DoubleClick(sender, ee);
                    e.Handled = true;
                } // end if
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
            if (e.KeyValue == 113)
                T_bt = butt_Save;     //저장  F1
            if (e.KeyValue == 115)
                T_bt = butt_Delete;   // 삭제  F4
            if (e.KeyValue == 119)
                T_bt = butt_Excel;    //엑셀  F8    
            if (e.KeyValue == 112)
                T_bt = butt_Clear;    //엑셀  F5    

            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123 || e.KeyValue == 113 || e.KeyValue == 115 || e.KeyValue == 119 || e.KeyValue == 112)
                    cmdSave_Click(T_bt, ee1);
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

                    mtb.Focus(); return false;
                }
            }

            return true;
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

            if ((sender is TextBox) == false) return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
                Sw_Tab = 1;
            }

            if (tb.Name == "txtCenter")
            {
                if (tb.Text.Trim() == "")
                    txtCenter_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtCenter_Code);
            }

            //if (tb.Name == "txtBank")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtSellCode_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);
            //}

            //if (tb.Name == "txtR_Id")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtR_Id_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code);
            //}

            //if (tb.Name == "txtCenter2")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtCenter2_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtCenter2_Code);
            //}

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
            if (tb.Name == "txtCenter")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, txtCenter_Code, "");
                else
                    Ncod_Text_Set_Data(tb, txtCenter_Code);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            //if (tb.Name == "txtR_Id")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtR_Id_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

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

            //if (tb.Name == "txtCenter2")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtCenter2_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtCenter2_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

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


        private void Db_Grid_Popup(TextBox tb, TextBox tb1_Code, string strSql)
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
                    cgb_Pop.db_grid_Popup_Base(2, "구매_코드", "구매종류", "SellCode", "SellTypeName", strSql);
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

                    cgb_Pop.db_grid_Popup_Base(2, "구매_코드", "구매종류", "SellCode", "SellTypeName", Tsql);
                }

            }
        }



        private void Ncod_Text_Set_Data(TextBox tb, TextBox tb1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

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


            if (tb.Name == "txtCenter2")
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



        private void trv_Item_Set_Sort_Code()
        {
            string ItemName = ""; string ItemCode = "";
    
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>상위 메뉴 관련된 내역을 트리뷰에 넣는다
            
            MenuStrip temp_menu = cls_app_static_var.Mdi_Base_Menu; //((MDIMain)(this.MdiParent)).menuStrip;
            //MenuStrip temp_menu = ((MDIMain)(this.MdiParent)).menuStrip;
            int fCnt = 0;
            foreach (ToolStripMenuItem Baes_1_Menu in temp_menu.Items)
            {
                if ("Exit_Menu" != Baes_1_Menu.Name && Baes_1_Menu.Visible == true)
                {
                    ItemName = Baes_1_Menu.Text.ToString();
                    ItemCode = Baes_1_Menu.Name.ToString();


                    if (ItemCode != "")
                    {
                        if (fCnt == 0)
                        {
                            trv_Item.Nodes.Clear();
                            trv_Item.CheckBoxes = true;

                            tab_Menu.TabPages[0].Text = ItemName;
                            TreeNode tn = trv_Item.Nodes.Add(ItemName);
                            dic_Tree_Sort_1[ItemCode] = tn;
                        }
                        else
                        {
                            TabPage t_tp = new TabPage();
                            TreeView t_v = new TreeView();

                            t_v.Nodes.Clear();
                            t_v.CheckBoxes = true;
                            t_v.AfterCheck += new TreeViewEventHandler(trv_Item_AfterCheck);
                            
                            t_tp.Text = ItemName;
                            t_tp.BackColor = tab_Menu.TabPages[0].BackColor;
                            t_tp.Controls.Add(t_v);

                            t_v.Dock = DockStyle.Fill;

                            TreeNode tn = t_v.Nodes.Add(ItemName);
                            dic_Tree_Sort_1[ItemCode] = tn;
                            //dic_Tree_view[ItemName] = t_v;

                            tab_Menu.Controls.Add(t_tp);
                        }

                        fCnt++;
                    }

                    
                }
            }
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 상위 메뉴 관련된 내역을 트리뷰에 넣는다
                        


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>하위메뉴 관련된 내역을 트리뷰에 넣는다
            string UpitemCode = "";            
            string Tool_Tip = "";
            foreach (ToolStripMenuItem Baes_1_Menu in temp_menu.Items)
            {
                //dic_Tree_view[Main_ItemName] = t_v;
               // Main_ItemName = Baes_1_Menu.Text.ToString();

                for (int cnt = 0; cnt < Baes_1_Menu.DropDownItems.Count; cnt++)
                {                  

                    if (Baes_1_Menu.DropDownItems[cnt] is ToolStripMenuItem)
                    {
                        //ToolStripItem sub_menu = Baes_1_Menu.DropDownItems[cnt];
                                                                      
                        ItemName = Baes_1_Menu.DropDownItems[cnt].Text.ToString();
                        ItemCode = Baes_1_Menu.DropDownItems[cnt].Name.ToString();
                        UpitemCode = Baes_1_Menu.Name.ToString();

                        Tool_Tip = "";
                        if (Baes_1_Menu.DropDownItems[cnt].ToolTipText != null)
                            Tool_Tip = Baes_1_Menu.DropDownItems[cnt].ToolTipText.ToString();                        
                        

                        //if (ItemCode == "m_Member_Delete")
                        //    return;

                        

                        if (dic_Tree_Sort_1 != null &&
                            ItemCode != "m_Base_User" &&
                            ItemCode != "m_Base_User_Log" &&
                            ItemCode != "m_Base_Config_1" &&
                            Tool_Tip != "-"
                            //Baes_1_Menu.DropDownItems[cnt].Enabled == true   //Visible 속성을 이곳에서 체크하면 다 Flase 나와서  Enabled로 해서 안보이는메뉴를 결정 하기로함.
                            //&& Baes_1_Menu.DropDownItems[cnt].Visible == true
                            )
                        {

                            if (dic_Tree_Sort_1.ContainsKey(UpitemCode))
                            {
                                TreeNode tn2 = dic_Tree_Sort_1[UpitemCode];

                                if (tn2 != null)
                                {
                                    TreeNode node2 = new TreeNode(ItemName);
                                    tn2.Nodes.Add(node2);
                                    tn2.Expand();
                                    dic_Tree_Sort_2[UpitemCode + "/" + ItemCode] = node2;
                                }
                            }

                        }
                       

                    }
                }
                
            }

            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<하위메뉴 관련된 내역을 트리뷰에 넣는다                        
        }



        private void trv_Item_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
               
                if (e.Node.Parent == null)
                {          
                    foreach (string t_for_key in dic_Tree_Sort_2.Keys)
                    {
                        TreeNode tn2 = dic_Tree_Sort_2[t_for_key];
                        if (e.Node.Text.ToString() == tn2.Parent.Text.ToString())
                        {
                            tn2.Checked = e.Node.Checked  ;
                        }                       
                    }
                }                          
            }


        }


        private void From_Clear_()
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, txtNcode);
            chkb_Leave.Checked = false;            
            chk_Excel_Save.Checked = false;            
            chk_Cpno_V.Checked = false;

            //dGridView_Login_Header_Reset();
            //cgb_Login.d_Grid_view_Header_Reset();

            //dGridView_Excel_Header_Reset();
            //cgb_Excel.d_Grid_view_Header_Reset();


            tabControl_Tab_Dispose();
            trv_Item_Set_Sort_Code();

            txtID.ReadOnly = false;
            txtID.BorderStyle = BorderStyle.Fixed3D ;
            txtID.BackColor = SystemColors.Window; 

            txtNcode.ReadOnly = false;
            txtNcode.BorderStyle = BorderStyle.Fixed3D;
            txtNcode.BackColor = SystemColors.Window; 

            if (dic_tbl_User != null)
                dic_tbl_User.Clear();

            Set_Tbl_User();  //회원의 구매 관련 주테이블 내역을 클래스에 넣는다.

            if (dic_tbl_User != null)
                Base_Grid_Set();      

        }


        private void cmdSave_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            if (bt.Name == "butt_Clear")
            {
                From_Clear_();
            }
            else if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                //저장이 이루어진다.
                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex; 
                Save_Base_Data(ref Save_Error_Check);

                if (Save_Error_Check > 0)
                {
                    From_Clear_();  
                }

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }
            else if (bt.Name == "butt_Delete")
            {
                int Del_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Delete_Base_Data(ref Del_Error_Check);
                if (Del_Error_Check > 0)
                {
                    From_Clear_();  
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            else if (bt.Name == "butt_Excel")
            {
                //frmBase_Excel e_f = new frmBase_Excel();
                //e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
                //e_f.ShowDialog();
            }

        }


        private void Delete_Base_Data(ref int Del_Error_Check)
        {
            Del_Error_Check = 0;
            if (txtNcode.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_User_Ncode")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtID.Focus(); return ;
            }


            if (txtID.Text == "admin")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Del_UserID")                     
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtID.Focus(); return;
            }


            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string Tsql;
            Tsql = "Delete From tbl_User   ";
            Tsql = Tsql + " Where user_ID = '" + txtID.Text.Trim() + "'";

            if (Temp_Connect.Delete_Data(Tsql, base_db_name, this.Name.ToString(), this.Text) == false) return;

            Del_Error_Check = 1;
            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del"));
        }




        private bool Base_Error_Check__01()
        {
            //회원을 선택 안햇네 그럼 회원 넣어라
            if (txtName.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_User_Name")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtName.Focus(); return false;
            }

            //if (txtNcode.Text == "")
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
            //           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_User_Ncode")
            //          + "\n" +
            //          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    txtNcode.Focus(); return false;
            //}


            //if (txtNcode.ReadOnly == false)  //새로운 입력이 아니고 수정이다.
            //{
            //    if (dic_tbl_User.ContainsKey(txtNcode.Text) == true)  //새로입력하는 사항인데. 동일한 직원번호가 존재한다 그럼 팉ㅇ겨라
            //    {
            //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_User_Ncode")                       
            //          + "\n" +
            //          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //        txtNcode.Focus(); return false;
            //    }
            //}


            if (radioB_U_1.Checked == true)
            {
                if (txtID.Text == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_User_ID")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtID.Focus(); return false;
                }




                if (txtPassword.Text == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_User_Password")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtPassword.Focus(); return false;
                }


                if (txtPassword2.Text == "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_User_Password2")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtPassword2.Focus(); return false;
                }

                if (txtPassword.Text != txtPassword2.Text)
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_User_Password_Not")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtPassword.Focus(); return false;
                }

                if (txtID.ReadOnly == false)  //새로운 입력이 아니고 수정이다.
                {
                    foreach (string t_for_key in dic_tbl_User.Keys)
                    {
                        if (dic_tbl_User[t_for_key].user_id == txtID.Text.Trim())
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Same_User_ID")                               
                              + "\n" +
                              cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            txtID.Focus(); return false;
                        }
                    }
                }
            }


            string Sn = mtxtTel1.Text.Replace("-", "").Replace("_", "").Trim();
            if (Sn_Number_(Sn, mtxtTel1, "Tel") == false)
            {
                mtxtTel1.Focus();
                return false;
            }
        
            return true;
        }


        private void Save_Base_Data(ref int Save_Error_Check)
        {

            if (Base_Error_Check__01() == false) return; 




            string Mmenu_User = "";
            foreach (string t_for_key in dic_Tree_Sort_2.Keys)
            {
                TreeNode tn2 = dic_Tree_Sort_2[t_for_key];
                if ( tn2.Checked == true)
                {
                    Mmenu_User = Mmenu_User + "%" + t_for_key;
                }
            }





            int Sell_Info_V_TF = 0; int Excel_Save_TF = 0; int Cpno_V_TF = 0; int Leave_TF = 0; string hometel = ""; int Using_TF = 0, For_Save_TF = 0, CC_Save_TF = 0 ;

            if (chk_Info_V.Checked == true)
                Sell_Info_V_TF = 1;

            if (chk_Excel_Save.Checked == true)
                Excel_Save_TF = 1;

            if (chk_For_Save.Checked == true)
                For_Save_TF = 1;

            if (chk_CC_Save.Checked == true)
                CC_Save_TF = 1;

            if (chk_Cpno_V.Checked == true)
                Cpno_V_TF = 1;

             if (chkb_Leave.Checked == true)
                Leave_TF = 1;

             

            Using_TF = 0;
            if (radioB_U_2.Checked == true)  Using_TF = 1 ;

            hometel = mtxtTel1.Text;

            string u_user_Ncode = txtID.Text.Trim();
            //++++++++++++++++++++++++++++++++

            if (dic_tbl_User.ContainsKey(u_user_Ncode) == false)
            {
                
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
                string StrSql = "";
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                //CenterCode
                StrSql = "insert into tbl_user " ;
                StrSql = StrSql + " (";
                StrSql = StrSql + " user_Ncode, user_id, U_name, user_password, ";
                StrSql = StrSql + " Menu1," ;
                StrSql = StrSql + " recordtime,recordid,Log_Check,CenterCode  ";
                StrSql = StrSql + " ,Sell_Info_V_TF , Na_Code ";
                StrSql = StrSql + " ,Excel_Save_TF , Cpno_V_TF , For_Save_TF , CC_Save_TF , Leave_TF , phone, Using_TF ";
                StrSql = StrSql + " ) ";
                StrSql = StrSql + " values " ;
                StrSql = StrSql + " (" ;
                StrSql = StrSql + "'" + u_user_Ncode + "'";
                StrSql = StrSql + ",'" + txtID.Text.Trim() + "'";
                StrSql = StrSql + ",'" + txtName.Text.Trim () + "'";
                StrSql = StrSql + ",'" + txtPassword.Text.Trim () + "'";
                StrSql = StrSql + ",'" + Mmenu_User + "'";
                StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) " ;
                StrSql = StrSql + ",'" + cls_User.gid  + "'" ;
                StrSql = StrSql + ", 0 ";
                StrSql = StrSql + ",'" + txtCenter_Code.Text.Trim() +  "'";
                StrSql = StrSql + ", " + Sell_Info_V_TF + " ,'" + combo_Se_Code.Text  + "'";
                StrSql = StrSql + ", " + Excel_Save_TF + " ," + Cpno_V_TF + "," + For_Save_TF + "," + CC_Save_TF + "," + Leave_TF + ",'" + hometel + "'," + Using_TF;
                StrSql = StrSql + ")";
    
        
                if (Temp_Connect.Insert_Data(StrSql, base_db_name, this.Name.ToString(), this.Text) == false) return;

                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));

            }
            else
            {
                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                string Tsql = "";

                Tsql = "Update tbl_User Set ";
                Tsql = Tsql + " Menu1 = '" + Mmenu_User + "'";
                Tsql = Tsql + " ,phone = '" + hometel + "'";
                Tsql = Tsql + " ,User_id = '" + txtID.Text.Trim() + "'";                
                Tsql = Tsql + " ,U_name = '" + txtName.Text.Trim () +  "'" ; 
                Tsql = Tsql + " ,user_password = '" + txtPassword.Text.Trim ()  + "'" ;
                Tsql = Tsql + " ,CenterCode='" + txtCenter_Code.Text.Trim() + "'";
                Tsql = Tsql + " ,Sell_Info_V_TF= " + Sell_Info_V_TF;
                Tsql = Tsql + " ,Excel_Save_TF= " + Excel_Save_TF;
                Tsql = Tsql + " ,Leave_TF= " + Leave_TF;
                Tsql = Tsql + " ,Cpno_V_TF= " + Cpno_V_TF;
                Tsql = Tsql + " ,Using_TF= " + Using_TF;
                Tsql = Tsql + " ,For_Save_TF= " + For_Save_TF;
                Tsql = Tsql + " ,CC_Save_TF= " + CC_Save_TF;
                
                Tsql = Tsql + " ,Na_Code= '" + combo_Se_Code.Text  + "'";
                

                Tsql = Tsql + " Where User_ID = '" + u_user_Ncode + "' ";

                if (Temp_Connect.Update_Data(Tsql, this.Name.ToString(), this.Text) == false) return;

                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
                
            }
        }








        private void Set_Tbl_User()
        {

            string Tsql = "";
            Tsql = "select user_Ncode,  user_id, u_name , tbl_user.Phone ";
            Tsql = Tsql + ", user_password,  CenterCode , Log_Check, Log_Date ";
            Tsql = Tsql + ", LanNumber , Isnull(tbl_business.Name,'') AS U_CC_Name ";
            Tsql = Tsql + ", Sell_Info_V_TF ,  tbl_user.Na_Code , Menu1 , Excel_Save_TF , Cpno_V_TF,  For_Save_TF , CC_Save_TF , Leave_TF";
            Tsql = Tsql + " ,Isnull(nationNameKo,'') nationNameKo ";
            Tsql = Tsql + " From tbl_user (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_business  (nolock)  ON tbl_business.Ncode = tbl_user.CenterCode And tbl_user.Na_code = tbl_Business.Na_code ";
            Tsql = Tsql + " LEFT JOIN tbl_Nation  (nolock)  ON tbl_Nation.nationCode = tbl_user.Na_Code ";
            Tsql = Tsql + " order by User_id ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            Dictionary<string, cls_tbl_User> T_tbl_User = new Dictionary<string, cls_tbl_User>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cls_tbl_User t_User = new cls_tbl_User();

                t_User.user_Ncode = ds.Tables[base_db_name].Rows[fi_cnt]["User_Ncode"].ToString();
                t_User.user_id = ds.Tables[base_db_name].Rows[fi_cnt]["user_id"].ToString();
                t_User.u_name = ds.Tables[base_db_name].Rows[fi_cnt]["u_name"].ToString();
                t_User.user_password = ds.Tables[base_db_name].Rows[fi_cnt]["user_password"].ToString();
                t_User.CenterCode = ds.Tables[base_db_name].Rows[fi_cnt]["CenterCode"].ToString();
                //t_User = ds.Tables[base_db_name].Rows[fi_cnt]["CenterCode"].ToString();
                t_User.Log_Check = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Log_Check"].ToString());
                t_User.Log_Date = ds.Tables[base_db_name].Rows[fi_cnt]["Log_Date"].ToString();
                t_User.LanNumber = ds.Tables[base_db_name].Rows[fi_cnt]["LanNumber"].ToString();
                t_User.U_CC_Name = ds.Tables[base_db_name].Rows[fi_cnt]["U_CC_Name"].ToString();

                t_User.Na_Code = ds.Tables[base_db_name].Rows[fi_cnt]["Na_Code"].ToString();
                t_User.Na_Code_Name = ds.Tables[base_db_name].Rows[fi_cnt]["nationNameKo"].ToString();

                t_User.Sell_Info_V_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Sell_Info_V_TF"].ToString());
                t_User.Menu1 = ds.Tables[base_db_name].Rows[fi_cnt]["Menu1"].ToString();
                
                t_User.Excel_Save_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Excel_Save_TF"].ToString());
                t_User.Cpno_V_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Cpno_V_TF"].ToString());
                t_User.For_Save_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["For_Save_TF"].ToString());
                t_User.CC_Save_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["CC_Save_TF"].ToString());

                t_User.Leave_TF = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Leave_TF"].ToString());
                t_User.phone = ds.Tables[base_db_name].Rows[fi_cnt]["phone"].ToString();

                

                T_tbl_User[t_User.user_id] = t_User;     
            }


            dic_tbl_User = T_tbl_User;
        }

        private void Base_Grid_Set()
        {
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            int fi_cnt = 0;
            foreach (string t_key in dic_tbl_User.Keys)
            {                
                Set_gr_dic(ref gr_dic_text, t_key, fi_cnt);  //데이타를 배열에 넣는다.                
                fi_cnt++;
            }
            
            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();            
        }


        private void Set_gr_dic(ref Dictionary<int, object[]> gr_dic_text, string t_key, int fi_cnt)
        {
            object[] row0 = { dic_tbl_User[t_key].user_Ncode
                                ,dic_tbl_User[t_key].u_name 
                                ,dic_tbl_User[t_key].user_id
                                ,dic_tbl_User[t_key].phone  
                                ,dic_tbl_User[t_key].U_CC_Name  
 
                                ,dic_tbl_User[t_key].Na_Code_Name 
                                ,""
                                ,""
                                ,""
                                ,""
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }

      

        private void dGridView_Base_Header_Reset()
        {

            cgb.grid_col_Count = 10;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            //cgb.grid_Frozen_End_Count = 2;            
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
           
            string[] g_HeaderText = {"_직원번호"  , "성명"   , "프로그램_사용_ID"  , "연락처"   , "소속센타"        
                            , "소속국가"   , ""    , ""   , ""    , ""                                
                                };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 0, 100, 20, 20, cls_app_static_var.Program_User_Center_Sort                                                           
                            ,cls_app_static_var.Using_Multi_language  , 0 , 0 , 0 , 0                             
                        };
            cgb.grid_col_w = g_Width;
           

            

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                                                        
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleRight //10
                           
                              };
            cgb.grid_col_alignment = g_Alignment;

            //cgb.basegrid.RowHeadersVisible = false;
        }




        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            //dGridView_Login_Header_Reset();
            //cgb_Login.d_Grid_view_Header_Reset();

            //dGridView_Excel_Header_Reset();
            //cgb_Excel.d_Grid_view_Header_Reset();

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(panel8, txtNcode);
            chkb_Leave.Checked = false;
            chk_Excel_Save.Checked = false;
            chk_Cpno_V.Checked = false;


            tab_Menu.Visible = false;           
            tabControl_Tab_Dispose();
            trv_Item_Set_Sort_Code();
            tab_Menu.Visible = true;
            tab_Menu.Refresh();


            

            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                Data_Set_Form_TF = 1;
                string u_user_Ncode = "";
                u_user_Ncode = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString();
                User_node_Check(u_user_Ncode);

                txtID.Text = dic_tbl_User[u_user_Ncode].user_id ;
                txtNcode.Text = dic_tbl_User[u_user_Ncode].user_Ncode;
                txtName.Text = dic_tbl_User[u_user_Ncode].u_name;
                
                string T_string; string[] T_string_A;
                T_string = dic_tbl_User[u_user_Ncode].phone;
                mtxtTel1.Text = T_string;
                //T_string_A = T_string.Split('-');
                //for (int i = 0; i <= T_string_A.Length - 1; i++)
                //{
                //    if (i == 0) txtTel_1.Text = T_string_A[i];
                //    if (i == 1) txtTel_2.Text = T_string_A[i];
                //    if (i == 2) txtTel_3.Text = T_string_A[i];
                //}


                txtPassword.Text = dic_tbl_User[u_user_Ncode].user_password;
                txtPassword2.Text = dic_tbl_User[u_user_Ncode].user_password;
                txtD1.Text = dic_tbl_User[u_user_Ncode].Log_Date;
                txtD2.Text = dic_tbl_User[u_user_Ncode].LanNumber;

                txtCenter.Text = dic_tbl_User[u_user_Ncode].U_CC_Name;
                txtCenter_Code.Text = dic_tbl_User[u_user_Ncode].CenterCode;

                combo_Se_Code.Text = dic_tbl_User[u_user_Ncode].Na_Code;
                combo_Se.SelectedIndex = combo_Se_Code.SelectedIndex  ;


                if (dic_tbl_User[u_user_Ncode].Sell_Info_V_TF == 1)
                    chk_Info_V.Checked = true;

                if (dic_tbl_User[u_user_Ncode].Excel_Save_TF == 1)
                    chk_Excel_Save.Checked = true;

                if (dic_tbl_User[u_user_Ncode].For_Save_TF == 1)
                    chk_For_Save.Checked = true;

                if (dic_tbl_User[u_user_Ncode].CC_Save_TF == 1)
                    chk_CC_Save.Checked = true;

                if (dic_tbl_User[u_user_Ncode].Cpno_V_TF == 1)
                    chk_Cpno_V.Checked = true;

                if (dic_tbl_User[u_user_Ncode].Leave_TF == 1)
                    chkb_Leave.Checked = true;

                if (txtID.Text != "")
                {
                    radioB_U_1.Checked = true;
                    radioB_U_2.Checked = false;
                }
                else
                {
                    radioB_U_1.Checked = false;
                    radioB_U_2.Checked = true;
                }


                txtID.ReadOnly = true;
                txtID.BorderStyle = BorderStyle.FixedSingle;
                txtID.BackColor = cls_app_static_var.txt_Enable_Color;

                txtNcode.ReadOnly = true;
                txtNcode.BorderStyle = BorderStyle.FixedSingle;
                txtNcode.BackColor = cls_app_static_var.txt_Enable_Color; 
 
                //Login_Grid_Set(u_user_Ncode);

                //Excel_Grid_Set(u_user_Ncode);

                Data_Set_Form_TF = 0;
            }
        }



        private void User_node_Check(string u_user)
        {
            string Menu1 = dic_tbl_User[u_user].Menu1;           
            User_node_Check(Menu1,0);
        }


        private void User_node_Check(string Menu1, int s_TF)
        {
            string[] t_Memu;
            t_Memu = Menu1.Split('%');
            for (int cnt = 0; cnt < t_Memu.Length; cnt++)
            {
                if (t_Memu[cnt] != "")
                {
                    foreach (string t_for_key in dic_Tree_Sort_2.Keys)
                    {
                        TreeNode tn2 = dic_Tree_Sort_2[t_for_key];
                        if (t_for_key == t_Memu[cnt])
                            tn2.Checked = true;                    
                    }
                }
            }

        }







        private void Login_Grid_Set(string Search_id)
        {

            dGridView_Login_Header_Reset();
            cgb_Login.d_Grid_view_Header_Reset();

            string Tsql = "";

            //string[] g_HeaderText = {"로그인_시간"  , "로그오프_시간"   , "IP"  , "구분"   , ""        
            //                    , ""   , ""    , ""  , "" , ""                                
            //                    };

            Tsql = "Select Connect_Time, End_Time, Connect_IP, Connect_C_Name ";
            Tsql = Tsql + " ,''     ,'','','' ,'','' " ;
            Tsql = Tsql + " From  tbl_User_Con_Log  (nolock) ";
            Tsql = Tsql + " Where T_U_ID = '" + Search_id + "'";
            Tsql = Tsql + " Order by  Connect_Time DESC ";

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

            cgb_Login.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Login.db_grid_Obj_Data_Put();
        }


        private void Set_gr_Login(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;

            object[] row0 = new object[cgb_Login.grid_col_Count];

            while (Col_Cnt < cgb_Login.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }
            
            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Login_Header_Reset()
        {
            cgb_Login.Grid_Base_Arr_Clear();
            cgb_Login.basegrid = dGridView_Login;
            cgb_Login.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Login.grid_col_Count = 10;

            //cgb_Login.grid_Frozen_End_Count = 3;
            cgb_Login.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"로그인_시간"  , "로그오프_시간"   , "IP"  , "구분"   , ""        
                                , ""   , ""    , ""  , "" , ""                                
                                };

            int[] g_Width = { 120, 120, 100, 100, 0
                            ,0 , 0 , 0 , 0 , 0                          
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


            cgb_Login.grid_col_header_text = g_HeaderText;            
            cgb_Login.grid_col_w = g_Width;
            cgb_Login.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                                                 
                                   };
            cgb_Login.grid_col_Lock = g_ReadOnly;

        }










        private void Excel_Grid_Set(string Search_id)
        {

            dGridView_Excel_Header_Reset();
            cgb_Excel.d_Grid_view_Header_Reset();

            string Tsql = "";

            //string[] g_HeaderText = {"전환화면"  , "저장이름"   , "저장시간"  , ""   , ""                                        
            //                    ,"" , "" , ""  ,   ""  , "" 
            //                    };

            Tsql = "Select T_U_Caption, T_U_Excel_Name, T_U_Date ";
            Tsql = Tsql + " ,   '',  '',    '','','','',''  ";

            Tsql = Tsql + " From  tbl_Excel_User  (nolock) ";

            Tsql = Tsql + " Where T_U_ID = '" + Search_id + "'";
            Tsql = Tsql + " Order by T_U_Date DESC  ";


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
                Set_gr_Excel(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb_Excel.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Excel.db_grid_Obj_Data_Put();
        }


        private void Set_gr_Excel(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;

            object[] row0 = new object[cgb_Excel.grid_col_Count];

            while (Col_Cnt < cgb_Excel.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }


            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Excel_Header_Reset()
        {
            cgb_Excel.Grid_Base_Arr_Clear();
            cgb_Excel.basegrid = dGridView_Excel;
            cgb_Excel.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb_Excel.grid_col_Count = 10;

            //cgb_Excel.grid_Frozen_End_Count = 3;
            cgb_Excel.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"전환_화면"  , "저장_이름"   , "저장_시간"  , ""   , ""                                        
                                ,"" , "" , ""  ,   ""  , "" 
                                };

            int[] g_Width = { 100, 150, 120, 0, 0
                            ,0 , 0 , 0 , 0 , 0                      
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleLeft  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleCenter  //10

                                };


            cgb_Excel.grid_col_header_text = g_HeaderText;
            cgb_Excel.grid_col_w = g_Width;
            cgb_Excel.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true   
                                   };
            cgb_Excel.grid_col_Lock = g_ReadOnly;

        }

        private void button_LogOut_Click(object sender, EventArgs e)
        {

            if (Base_Error_Check__01() == false) return; 


            string u_user = txtNcode.Text.Trim();
            //++++++++++++++++++++++++++++++++

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string Tsql = "";

            Tsql = "Update tbl_User Set ";            
            Tsql = Tsql + "  Log_Date= '' ";
            Tsql = Tsql + " ,Log_Check= 0 ";
            Tsql = Tsql + " ,LanNumber= '' ";            
            Tsql = Tsql + " Where User_Ncode = '" + u_user + "' ";

            if (Temp_Connect.Update_Data(Tsql, this.Name.ToString(), this.Text) == false) return;


            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_App"));
        }



        private void tabControl_Tab_Dispose()
        {

           
            for (int fcnt = tab_Menu.TabCount - 1; fcnt > 0; fcnt--)
            {
                tab_Menu.TabPages[fcnt].Dispose();
            }
           
            tab_Menu.TabPages[0].Text = "";
            
        }

        private void radioB_U_1_MouseClick(object sender, MouseEventArgs e)
        {
            if (txtID.Text.Trim() == "" && txtID.ReadOnly == true)
            {
                txtID.ReadOnly = false;
                txtID.BorderStyle = BorderStyle.Fixed3D;
                txtID.BackColor = SystemColors.Window;
            }
        }






    }
}
