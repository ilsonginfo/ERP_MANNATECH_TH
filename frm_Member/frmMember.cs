
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Xml;



namespace MLM_Program
{
    public partial class frmMember : Form
    {
        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);


        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cg_Li = new cls_Grid_Base();

        //public delegate void Take_NumberDele(ref string Send_Number, ref string Send_Name);
        //public event Take_NumberDele Take_Mem_Number;

        private const string base_db_name = "tbl_Memberinfo";
        private int Data_Set_Form_TF;
        private int Mbid_Number_Hand_Check_TF;


        public delegate void SendNumberDele(string Send_Number, string Send_Name, string Send_OrderNumber);
        public event SendNumberDele Send_Mem_Number;


        public frmMember()
        {
            InitializeComponent();

            DoubleBuffered = true;

            typeof(Form).InvokeMember("DoubleBuffered", BindingFlags.NonPublic
          | BindingFlags.Instance | BindingFlags.SetProperty, null, this, new object[] { true });

            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.SetProperty, null, dGridView_Base, new object[] { true });

            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.SetProperty, null, dGridView_Good, new object[] { true });


            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.SetProperty, null, panel7, new object[] { true });

            typeof(TabControl).InvokeMember("DoubleBuffered", BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.SetProperty, null, tab_Sub, new object[] { true });


        }


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
           
           

            Data_Set_Form_TF = 0;
            Mbid_Number_Hand_Check_TF = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Line_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Li.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            cls_form_Meth cm = new cls_form_Meth();


            cm.from_control_text_base_chang(this);

            Data_Set_Form_TF = 1;
            mtxtRegDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            Data_Set_Form_TF = 0;
            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid_s.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid_n.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtSn.Mask = "999999-9999999"; //기본 셋팅은 주민번호이다.
            mtxtSn_C.Mask = "999999-9999999"; //기본 셋팅은 주민번호이다.
            txtB1.Text = "0";

            mtxtTel1.Mask = cls_app_static_var.Tel_Number_Fromat;
            mtxtTel2.Mask = cls_app_static_var.Tel_Number_Fromat;
            mtxtZip1.Mask = cls_app_static_var.ZipCode_Number_Fromat;


            mtxtBrithDay.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtRegDate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtEdDate.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtZip_Auto.Mask = cls_app_static_var.ZipCode_Number_Fromat;
            mtxtTel_Auto.Mask = cls_app_static_var.Tel_Number_Fromat;

            mtxtVisaDay.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtBrithDayC.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtTel2_C.Mask = cls_app_static_var.Tel_Number_Fromat;

            //위치찾는게 자동이다 그럼 수동 관련된 요소를 닫는다.
            if (cls_app_static_var.Member_Reg_Line_Select_TF == 0)
            {
                txtLineCnt.BackColor = Color.AliceBlue;
                txtLineCnt.ReadOnly = true;
                txtLineCnt.Tag = "";
                grB_Line.Visible = false;
                groupBox1.Width = 772;
            }

            if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
            {
                tbl_save.Visible = false;
                chk_Top_s.Checked = true;
                chk_Foreign_s.Checked = true;
            }

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                tbl_nom.Visible = false;
                chk_Top_n.Checked = true;
                chk_Foreign_n.Checked = true;
            }

            txtSN_n.BackColor = cls_app_static_var.txt_Enable_Color;
            txtSN_s.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_Auto_PR.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_Auto_PR2.BackColor = cls_app_static_var.txt_Enable_Color;

            if (cls_app_static_var.Member_Cpno_Error_Check_TF == 1)
                check_Cpno_Err.Checked = true;

            if (cls_app_static_var.Member_Cpno_Put_TF == 1)
                check_Cpno.Checked = true;

            if (cls_app_static_var.Member_Reg_Multi_TF == 1)
                check_Cpno_Multi.Checked = true;

            if (cls_app_static_var.Mem_Number_Auto_Flag == "R")
            {
                opt_MCode_R.Checked = true;
                mtxtMbid.ReadOnly = true;
                mtxtMbid.BackColor = cls_app_static_var.txt_Enable_Color;
                txtName.Focus();
            }

            if (cls_app_static_var.Mem_Number_Auto_Flag == "A")
            {
                mtxtMbid.ReadOnly = true;
                mtxtMbid.BackColor = cls_app_static_var.txt_Enable_Color;
                opt_MCode_A.Checked = true;
                txtName.Focus();
            }

            if (cls_app_static_var.Mem_Number_Auto_Flag == "H")
            {
                mtxtMbid.ReadOnly = false;
            }


         
                cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
                cpbf.Put_NaCode_ComboBox(combo_Se, combo_Se_Code);
                cpbf.Put_NaCode_ComboBox(combo_Se_2, combo_Se_Code_2);
            

            button1.Visible = false;
            if (cls_User.gid == cls_User.SuperUserID)
            {
                button1.Visible = true;
            }

            InitComboZipCode_TH();
            // 태국버전 인 경우
            if (cls_User.gid_CountryCode == "TH")
            {
                pnlDistrict_TH.Visible = true;
                pnlProvince_TH.Visible = true;
                pnlSubDistrict_TH.Visible = true;
                pnlZipCode_TH.Visible = true;
                pnlZipCode_KR.Visible = false;
                txtAddress2.ReadOnly = true;
                cbSubDistrict_TH_SelectedIndexChanged(this, null);
                //combo_Se_Code_2.Text = "TH";
            }
            // 태국 이외 버전 인 경우
            else
            {
                pnlDistrict_TH.Visible = false;
                pnlProvince_TH.Visible = false;
                pnlSubDistrict_TH.Visible = false;
                pnlZipCode_TH.Visible = false;
                pnlZipCode_KR.Visible = true;
                txtAddress2.ReadOnly = false;
                txtAddress2.Clear();
            }

            combo_Se_Code_2.Text = cls_User.gid_CountryCode;

            InitCombo();

            //tab_Sub.TabPages.Remove(tab_C); 부부사업자 사용
            //tab_Sub.TabPages.Remove(tab_Auto);
            tab_Sub.TabPages.Remove(tab_Hide);
            tab_Sub.TabPages.Remove(tab_Auto);
            radioB_RBO.Checked = true;
            radioB_G8.Checked = true;

            opt_sell_2.Checked = true;
            if (cls_User.gid == cls_User.SuperUserID)
            {
                button5.Visible = true;
            }
            //if (cls_User.gid_CC_Save_TF == 0 )  //공동신청인 권한이 없는 사람은 보이지 않게 한다.

            //combo_C_Card_Per.Items.AddRange(data_P);




        }



        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Save.Left = butt_Clear.Left + butt_Clear.Width + 2;
            //butt_Excel.Left = butt_Save.Left + butt_Save.Width + 2;
            //butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Save);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);
            cfm.button_flat_change(butt_AddCode);

            cfm.button_flat_change(butt_AddCode2);
            cfm.button_flat_change(butt_AddCodeT1);
            cfm.button_flat_change(button_Acc_Reg);




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
                    Base_Button_Click(T_bt, ee1);
            }
        }




        private void frmMember_Activated(object sender, EventArgs e)
        {

            //string Send_Number = ""; string Send_Name = "";
            //Take_Mem_Number(ref Send_Number, ref Send_Name);

            //if (Send_Number != "")
            //    txtPassword.Text = Send_Number;
            //19-03-11 깜빡임제거 this.Refresh();
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Base_Grid_Set(); //당일등록 회원을 불러온다.
            Base_Grid_Set_Good(); //상품정보를 불러온다.
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        }




        private void txtData_Enter(object sender, EventArgs e)
        {
            //this.Refresh();
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

                if (tb.Name == "txtName" && tb.Text != "")
                    txtName_Accnt.Text = tb.Text;

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

                    if (mtb.Name == "mtxtTel_Auto")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtZip1")
                    {
                        if (Sn_Number_(Sn, mtb, "Zip") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtZip_Auto")
                    {
                        if (Sn_Number_(Sn, mtb, "Zip") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtBrithDay")
                    {
                        if (Sn_Number_(Sn, mtb, "Date") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtVisaDay")
                    {
                        if (Sn_Number_(Sn, mtb, "Date") == true)
                            SendKeys.Send("{TAB}");
                    }

                    string R4_name = mtb.Name.Substring(mtb.Name.Length - 4, 4);
                    if (R4_name == "Date" || R4_name == "ate3" || R4_name == "ate1" || R4_name == "ate2" || R4_name == "ate4")
                    {
                        if (Sn_Number_(Sn, mtb, "Date") == true)
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

        private bool Sn_Number_1(string Sn, TextBoxBase mtb, string sort_TF, int t_Sort2 = 0)
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
                        Ret = c_er.Input_Date_Err_Check((MaskedTextBox)mtb);

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
                if (sort_TF == "HpTel")
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("HPTEL"
                       + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    }
                    else
                    {
                        MessageBox.Show("휴대폰"
                       + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    }
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

                if (sort_TF == "Email")
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Email address has not been entered.");
                    }
                    else
                    {
                        MessageBox.Show("메일주소가 입력되지않았습니다."
                       + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    }
                }

                mtb.Focus(); return false;
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
                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Name == "mtxtMbid")
                {
                    //if (cls_app_static_var.Mem_Number_Auto_Flag == "H")  //회원번호가 수동 생성으로 체크되어 있는 경우
                    //{
                    //    cls_Search_DB csd = new cls_Search_DB();
                    //    string Hand_M_Number = csd.Auto_Member_Number_Search_Hand(mtxtMbid.Text.Trim());

                    //    if (Hand_M_Number == "")
                    //    {
                    //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Not")
                    //       + "\n" +
                    //       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    //        mtxtMbid.Focus();
                    //    }
                    //    else
                    //    {
                    //        Mbid_Number_Hand_Check_TF = 1;
                    //        mtxtMbid.Text = Hand_M_Number;
                    //        SendKeys.Send("{TAB}"); return;
                    //    }
                    //}   
                }

                //추천인과 후원인 관련해서 검색을 할경우에 사용되는 부분임 아래는
                if (mtb.Name == "mtxtMbid_s" || mtb.Name == "mtxtMbid_n")
                {
                    if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                    {
                        int reCnt = 0;
                        cls_Search_DB cds = new cls_Search_DB();
                        string Search_Name = "";
                        if (chk_Foreign_n.Checked == true || chk_Foreign_s.Checked == true)
                        {
                            reCnt = cds.Member_Name_Search_S_N_tbl_memberinfo_Fo(mtb.Text, ref Search_Name);
                        }
                        else
                        {
                            reCnt = cds.Member_Name_Search_S_N_tbl_memberinfo(mtb.Text, ref Search_Name);
                        }

                        if (reCnt == 1)
                        {
                            if (mtb.Name == "mtxtMbid_s")
                            {// 구현호 20210423 이전엔 추천후원계보밑에있는 애들 체크 하는 로직을 완전 없앴는데, 부활해줌.
                                // 하지만 추후 외국조직회원 조회시에는 그걸 때고 조회하도록한다.
                                txtName_s.Text = Search_Name;
                                if ( chk_Foreign_s.Checked == true)
                                {
                                    Set_Form_Date(mtb.Text, "s");
                                }
                                else
                                {
                                    if (Input_Error_Check(mtb, "s") == true)
                                        Set_Form_Date(mtb.Text, "s");
                                }
                            }

                            if (mtb.Name == "mtxtMbid_n")
                            {
                                txtName_n.Text = Search_Name;
                                if (chk_Foreign_n.Checked == true )
                                {
                                    Set_Form_Date(mtb.Text, "n");
                                }
                                else
                                {
                                    if (Input_Error_Check(mtb, "n") == true)
                                        Set_Form_Date(mtb.Text, "n");
                                }
                            }
                        }

                        else if (reCnt > 1)  //회원번호 비슷한 사람들이 많은 경우
                        {
                            string Mbid = "";
                            int Mbid2 = 0;
                            cds.Member_Nmumber_Split(mtb.Text, ref Mbid, ref Mbid2);

                            //frmBase_Member_Search_NOM_SAVE e_f = new frmBase_Member_Search_NOM_SAVE();

                            //if (mtb.Name == "mtxtMbid_s")
                            //{
                            //    e_f.Send_Mem_Number += new frmBase_Member_Search_NOM_SAVE.SendNumberDele(e_f_Send_Mem_Number);
                            //    e_f.Call_searchNumber_Info += new frmBase_Member_Search_NOM_SAVE.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info);
                            //}

                            //if (mtb.Name == "mtxtMbid_n")
                            //{
                            //    e_f.Send_Mem_Number += new frmBase_Member_Search_NOM_SAVE.SendNumberDele(e_f_Send_Mem_Number_3);
                            //    e_f.Call_searchNumber_Info += new frmBase_Member_Search_NOM_SAVE.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info_3);
                            //}

                            //cls_app_static_var.Search_Member_Name = txt_tag;
                            frmBase_Member_Search_NOM_SAVE e_f = new frmBase_Member_Search_NOM_SAVE();
                            frmBase_Member_Search_NOM_SAVE_Fo e_f_Fo = new frmBase_Member_Search_NOM_SAVE_Fo();
                            if (mtb.Name == "mtxtMbid_s")
                            {
                                if (chk_Foreign_s.Checked == false)
                                {
                                    e_f.Send_Mem_Number += new frmBase_Member_Search_NOM_SAVE.SendNumberDele(e_f_Send_Mem_Number);
                                    e_f.Call_searchNumber_Info += new frmBase_Member_Search_NOM_SAVE.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info);
                                    e_f.ShowDialog();
                                }
                                if (chk_Foreign_s.Checked == true)
                                {
                                    e_f_Fo.Send_Mem_Number += new frmBase_Member_Search_NOM_SAVE_Fo.SendNumberDele(e_f_Send_Mem_Number);
                                    e_f_Fo.Call_searchNumber_Info += new frmBase_Member_Search_NOM_SAVE_Fo.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info);
                                    e_f_Fo.ShowDialog();
                                }
                            }

                            if (mtb.Name == "mtxtMbid_n")
                            {
                                if (chk_Foreign_n.Checked == false)
                                {
                                    e_f.Send_Mem_Number += new frmBase_Member_Search_NOM_SAVE.SendNumberDele(e_f_Send_Mem_Number_3);
                                    e_f.Call_searchNumber_Info += new frmBase_Member_Search_NOM_SAVE.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info_3);
                                    e_f.ShowDialog();
                                }
                                if (chk_Foreign_n.Checked == true)
                                {
                                    e_f_Fo.Send_Mem_Number += new frmBase_Member_Search_NOM_SAVE_Fo.SendNumberDele(e_f_Send_Mem_Number_3);
                                    e_f_Fo.Call_searchNumber_Info += new frmBase_Member_Search_NOM_SAVE_Fo.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info_3);
                                    e_f_Fo.ShowDialog();
                                }
                            }
  

                            SendKeys.Send("{TAB}");
                            }
                    }
                    else
                        SendKeys.Send("{TAB}");
                }
            }

        }




        void e_f_Send_MemNumber_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = "";
            cls_Search_DB csb = new cls_Search_DB();
            csb.Member_Nmumber_Split(mtxtMbid_s.Text.Trim(), ref searchMbid, ref searchMbid2);
        }

        void e_f_Send_MemNumber_Info_3(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = "";
            cls_Search_DB csb = new cls_Search_DB();
            csb.Member_Nmumber_Split(mtxtMbid_n.Text.Trim(), ref searchMbid, ref searchMbid2);
        }


        //변경할려는 대상자에 대한 회원번호에서 회원 검색창을 뛰엇을 경우에
        void e_f_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            mtxtMbid_s.Text = Send_Number; txtName_s.Text = Send_Name;
            if (Input_Error_Check(mtxtMbid_s, "s") == true)
                Set_Form_Date(mtxtMbid_s.Text, "s");
        }


        //새로운 후원인 관련 회원 검색창을 뛰엇을 경우에 검색창에서 이벤트 실행시..
        void e_f_Send_Mem_Number_3(string Send_Number, string Send_Name)
        {
            mtxtMbid_n.Text = Send_Number; txtName_n.Text = Send_Name;
            if (Input_Error_Check(mtxtMbid_n, "n") == true)
                Set_Form_Date(mtxtMbid_n.Text, "n");
        }

        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            MaskedTextBox mtb = (MaskedTextBox)sender;

            if (mtb.Text.Replace("_", "").Replace("-", "").Replace(" ", "") == "")
            {
                Data_Set_Form_TF = 1;
                if (mtb.Name == "mtxtMbid_s")
                {
                    txtName_s.Text = ""; txtSN_s.Text = "";
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    dGridView_Line_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                    cg_Li.d_Grid_view_Header_Reset();

                    txtLineCnt.Text = "";
                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                }

                if (mtb.Name == "mtxtMbid_n")
                {
                    txtName_n.Text = ""; txtSN_n.Text = "";
                }
                Data_Set_Form_TF = 0;
            }
        }


        //회원번호 클릿햇을때. 관련 정보들 다 리셋 시킨다.
        //추후 번호만 변경하고 엔터 안누눌러서.. 데이타가 엉키는 것을 방지하기 위함.
        private void mtxtMbid_Click(object sender, EventArgs e)
        {
            MaskedTextBox mtb = (MaskedTextBox)sender;

            if (mtb.Name == "mtxtMbid_s")
            {

                txtName_s.Text = ""; txtSN_s.Text = "";
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Line_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cg_Li.d_Grid_view_Header_Reset();
                txtLineCnt.Text = "";
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            }

            if (mtb.Name == "mtxtMbid_n")
            {

                txtName_n.Text = ""; txtSN_n.Text = "";
            }

            //마스크텍스트 박스에 입력한 내용이 있으면 그곳 다음으로 커서가 가게 한다.
            if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                mtb.SelectionStart = mtb.Text.Replace("-", "").Replace("_", "").Trim().Length + 1;

        }




        private void MtxtData_Sn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    string Sn = mtb.Text.Replace("-", "").Replace("_", "").Trim();
                    if (Sn_Number_(Sn, mtb) == true)
                    {
                        if (mtb.Name == "mtxtSn")
                            mtxtRegDate.Focus();
                        else
                            txtName_E_1_C.Focus();
                    }
                }
                else
                {
                    if (mtb.Name == "mtxtSn")
                        mtxtRegDate.Focus();
                    else
                        txtName_E_1_C.Focus();
                }

            }
        }



        private bool Sn_Number_(string Sn, MaskedTextBox mtb)
        {
            if (mtb.Name == "mtxtSn")
            {
                if (raButt_IN_1.Checked == true) //내국인인 경우에는 주민번호 체크한다.
                {
                    string BirthDay2 = "";

                    string Sn_Recovery = Sn;
                    if (mtxtBrithDay.Text.Replace("-", "").Trim().Equals(string.Empty) == false)
                    {
                        string JuminLast = string.Empty;
                        string year = mtxtBrithDay.Text.Substring(0, 4);
                        int nYear = 0;
                        if (int.TryParse(year, out nYear))
                        {
                            JuminLast = nYear >= 2000 ? "3234567" : "1234567";

                        }

                        Sn = mtxtBrithDay.Text.Replace("-", "").Substring(2, 6) + JuminLast;
                    }

                    if (mtxtBrithDay.Text.Replace("-", "").Trim().Equals(string.Empty) == false)
                    {
                        cls_Sn_Check csn_C = new cls_Sn_Check();
                        if (csn_C.check_19_nai(Sn, ref BirthDay2) == false) //한국같은 경우에는 미성년자 필히 체크한다.
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_19")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            mtb.Focus(); return false;
                        }
                    }

                    if ((mtxtBrithDay.Text.Replace("-", "").Trim() == "" || mtxtBrithDay.Text.Replace("-", "").Trim().Length != 8) && BirthDay2 != "")
                        mtxtBrithDay.Text = BirthDay2;

                    if (Sn.Length >= 7)
                    {
                        int Sex = int.Parse(Sn.Substring(6, 1));

                        //if ((Sex % 2) == 0) radioB_Sex_X.Checked = true;
                        //if ((Sex % 2) == 1) radioB_Sex_Y.Checked = true;
                    }


                    Sn = Sn_Recovery;

                }
            }
            else if (Sn != "")
            {
                string sort_TF = "";
                bool check_b = false;
                cls_Sn_Check csn_C = new cls_Sn_Check();

                if (mtb.Name == "mtxtSn")
                {
                    if (raButt_IN_1.Checked == true) //내국인인 구분자
                        sort_TF = "in";

                    if (raButt_IN_2.Checked == true) //외국인 구분자
                        sort_TF = "fo";

                    if (raButt_IN_3.Checked == true) //사업자 구분자.
                        sort_TF = "biz";
                }
                else
                {
                    if (raButt_IN_1_C.Checked == true) //내국인인 구분자
                        sort_TF = "in";

                    if (raButt_IN_2_C.Checked == true) //외국인 구분자
                        sort_TF = "fo";

                }

                check_b = csn_C.Sn_Number_Check(Sn, sort_TF);

                if (check_b == false && sort_TF != "fo")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Error")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtb.Focus(); return false;
                }
                else
                {
                    if (cls_app_static_var.Member_Reg_Multi_TF == 0) //다구좌 불가능으로 해서 체크되어 잇는 경우
                    {//동일 주민번호로 해서 가입한 사람이 있는지를 체크한다.
                        cls_Search_DB csb = new cls_Search_DB();
                        if (csb.Member_Multi_Sn_Search(Sn) == false) //주민번호 오류는 위에서 체크를 함.
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Same")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            mtb.Focus(); return false;
                        }
                    }
                }



                if (mtb.Name == "mtxtSn_C")
                {
                    if (raButt_IN_1_C.Checked == true && check_b == true) //내국인인 경우에는 주민번호 체크한다.
                    {
                        string BirthDay2 = "";
                        if (csn_C.check_19_nai(Sn, ref BirthDay2) == false) //한국같은 경우에는 미성년자 필히 체크한다.
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_19")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            mtb.Focus(); return false;
                        }


                        if (mtxtBrithDayC.Text.Replace("-", "").Trim() == "" || mtxtBrithDayC.Text.Replace("-", "").Trim().Length != 8 && BirthDay2 != "")
                            mtxtBrithDayC.Text = BirthDay2;

                    }
                }

            }
            else
            {
                if (cls_app_static_var.Member_Cpno_Put_TF == 1) //주민번호 관련 필수입력인데 입력 안햇다.
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Put")
                            + "\n" +
                            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtb.Focus(); return false;
                }
            }

            return true;
        }






        private void Set_Form_Date(string T_Mbid, string T_sort)
        {
            string Mbid = ""; int Mbid2 = 0;
            Data_Set_Form_TF = 1;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) != 1)
            {
                Data_Set_Form_TF = 0;
                return;
            }
            string Tsql = "";
            if (chk_Foreign_n.Checked == false && T_sort == "n")
                 Tsql = "exec [Usp_JDE_SELECT_NOM_SAVE_like] " + T_Mbid + "";
            if (chk_Foreign_n.Checked == true && T_sort == "n" )
                Tsql = "exec [Usp_JDE_SELECT_NOM_SAVE_like_Fo] " + T_Mbid + "";
            if (chk_Foreign_s.Checked == false && T_sort == "s")
                Tsql = "exec [Usp_JDE_SELECT_NOM_SAVE_like] " + T_Mbid + "";
            if (chk_Foreign_s.Checked == true && T_sort == "s")
                Tsql = "exec [Usp_JDE_SELECT_NOM_SAVE_like_Fo] " + T_Mbid + "";

            //Tsql = "Select  ";
            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) AS M_Mbid ";
            //else
            //    Tsql = Tsql + " tbl_Memberinfo.mbid2 AS M_Mbid ";

            //Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

            //Tsql = Tsql + ",  tbl_Memberinfo.Cpno ";

            //Tsql = Tsql + " From tbl_Memberinfo (nolock) ";              
            //if (Mbid.Length == 0)
            //    Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            //else
            //{
            //    Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
            //    Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            //}
            ////// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";

            //if (T_sort != "s" && T_sort != "n")  //후원인하고 추천인 검색시에는 필요가 없다.
            //{
            //    Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
            //    Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";
            //}

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
   
            if (ReCnt == 0) return;
            if (ds.Tables[base_db_name].Rows[0]["Customerstatus"].ToString() == "1")
            {
                mtxtMbid_s.Text = ""; txtName_s.Text = ""; txtSN_s.Text = "";
                mtxtMbid_n.Text = ""; txtName_n.Text = ""; txtSN_n.Text = "";
                string me = "";
                me = "탈퇴자는 추천인 및 후원인으로 등록이 불가합니다." + "\n";

                MessageBox.Show(me);
                return;
            }
            //++++++++++++++++++++++++++++++++

            if (T_sort == "s")
            {
                mtxtMbid_s.Text = ""; txtName_s.Text = ""; txtSN_s.Text = "";
                mtxtMbid_s.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();
                txtName_s.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();

                string cpno = ds.Tables[base_db_name].Rows[0]["Cpno"].ToString();
                int CpnoLength = cpno.Length;
                if (CpnoLength > 9)
                {
                    txtSN_s.Text = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Cpno"].ToString().Replace("-", "").Substring(2, 6), "Cpno");
                }
                else
                {
                    txtSN_s.Text = "";
                }
                ////원래는 추천인 번호 앞자리가 기준인데 추천인 기능이 사용을 안한다고 체크를 하게 되면 후원인 앞자리로 해서 번호를 따옴. 자동 부여일 경우
                //if (cls_app_static_var.nom_uging_Pr_Flag == 0 && cls_app_static_var.Mem_Number_Auto_Flag == "A")  //후원인만 사용을 하고 번호 자동 부여이다.                
                //    mtxtMbid.Text = csb.Auto_Member_Number_Search(mtxtMbid_s.Text.Trim()); //후원인 앞자리 번호에 맞게 해서 번호를 자동으로 받아온다.

                select_Save_Dir_Down(Mbid, Mbid2);

                txtLineCnt.Focus();
            }

            if (T_sort == "n")
            {
                mtxtMbid_n.Text = ""; txtName_n.Text = ""; txtSN_n.Text = "";
                mtxtMbid_n.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();
                txtName_n.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();
                string cpno = ds.Tables[base_db_name].Rows[0]["Cpno"].ToString();
                int CpnoLength = cpno.Length;
                if (CpnoLength > 9)
                {
                    txtSN_n.Text = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Cpno"].ToString().Replace("-", "").Substring(2, 6), "Cpno");
                }
                else
                {
                    txtSN_n.Text = "";
                }
                //if (cls_app_static_var.Mem_Number_Auto_Flag == "A")  //회원번호 자동 생성으로 체크되어 잇는 경우
                //    mtxtMbid.Text = csb.Auto_Member_Number_Search(mtxtMbid_n.Text.Trim()); //추천인 앞자리 번호에 맞게 해서 번호를 자동으로 받아온다.
                ////원래는 추천인 번호 앞자리가 기준인데 추천인 기능이 사용을 안한다고 체크를 하게 되면 후원인 앞자리로 해서 번호를 따옴. 자동 부여일 경우
                //if (cls_app_static_var.nom_uging_Pr_Flag == 0 && cls_app_static_var.Mem_Number_Auto_Flag == "A")  //후원인만 사용을 하고 번호 자동 부여이다.                
                //    mtxtMbid.Text = csb.Auto_Member_Number_Search(mtxtMbid_s.Text.Trim()); //후원인 앞자리 번호에 맞게 해서 번호를 자동으로 받아온다.

                mtxtMbid_s.Focus();


                if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Nom_Same_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    mtxtMbid_s.Text = ""; txtName_s.Text = ""; txtSN_s.Text = "";
                    mtxtMbid_s.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();

                    if (Input_Error_Check(mtxtMbid_s, "s") == true)
                    {
                        txtName_s.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();
               
                    if (CpnoLength > 9)
                    {
                        txtSN_s.Text = encrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Cpno"].ToString().Replace("-", "").Substring(2, 6), "Cpno");
                    }
                    else
                    {
                        txtSN_s.Text = "";
                    }
                    select_Save_Dir_Down(Mbid, Mbid2);
                    if(chk_Foreign_n.Checked == true)
                    {
                        chk_Foreign_s.Checked = true;
                    }
                    else
                    {
                        chk_Foreign_s.Checked = false;
                    }
                    txtLineCnt.Focus();
                    }
                }
            }

            Data_Set_Form_TF = 0;
        }


        private void select_Save_Dir_Down(string Mbid, int Mbid2)
        {
            dGridView_Line_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Li.d_Grid_view_Header_Reset();

            Base_Grid_Set(Mbid, Mbid2);
        }

        private void Base_Grid_Set(string Mbid, int Mbid2)
        {
            string Tsql = "";

            Tsql = "Select  ";
            Tsql = Tsql + " tbl_Memberinfo.LineCnt ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) ";
            else
                Tsql = Tsql + " ,tbl_Memberinfo.mbid2 ";

            Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";

            if (Mbid.Length == 0)
                Tsql = Tsql + " Where tbl_Memberinfo.Saveid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Saveid = '" + Mbid + "' ";
                Tsql = Tsql + " And   tbl_Memberinfo.Saveid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " And   tbl_Memberinfo.LineCnt > 0 ";
            Tsql = Tsql + " Order by  tbl_Memberinfo.LineCnt";

            //당일 등록된 회원을 불러온다.

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
                Set_gr_dic_Line(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            cg_Li.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cg_Li.db_grid_Obj_Data_Put();

        }


        private void Set_gr_dic_Line(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][4]                                                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }



        private void dGridView_Line_Header_Reset()
        {


            cg_Li.grid_col_Count = 5;
            cg_Li.basegrid = dGridView_Li;
            cg_Li.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_Li.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"위치"  , "회원_번호"   , "성명"  , ""   , ""
                                    };
            cg_Li.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 30, 60, 70, 0, 0
                            };
            cg_Li.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                   };
            cg_Li.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5      
                              };
            cg_Li.grid_col_alignment = g_Alignment;
            cg_Li.basegrid.RowHeadersVisible = false;
            //cg_Li.basegrid.Font.Size = 7.5;

            //cg_Li.basegrid.ColumnHeadersDefaultCellStyle.Font =
            //new Font(cg_Li.basegrid.Font.FontFamily ,8);


        }






        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);
            T_R.Key_Enter_13_Name += new Key_13_Name_Event_Handler(T_R_Key_Enter_13_Name);
            TextBox tb = (TextBox)sender;

            if ((tb.Tag == null) || (tb.Tag.ToString() == ""))
            {
                //쿼리문상 오류를 잡기 위함.
                if (T_R.Text_KeyChar_Check(e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if   
            }
            else if ((tb.Tag != null) && (tb.Tag.ToString() == "1"))
            {
                //숫자만 입력 가능
                if (T_R.Text_KeyChar_Check(e, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "-"))
            {
                //숫자와  - 만
                if (T_R.Text_KeyChar_Check(e, "-") == false)
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

            else if ((tb.Tag != null) && tb.Tag.ToString() == "name")  //회원 정보 관련해서 이름 검색을 필요로 하는 텍스트 박스이다.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(tb, e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "."))
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1, ".") == false)
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
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtCenter_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtBank")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtBank_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

        }


        void T_R_Key_Enter_13_Name(string txt_tag, TextBox tb)
        {
            if (txt_tag != "")
            {
                int reCnt = 0;
                cls_Search_DB cds = new cls_Search_DB();
                string Search_Mbid = "";
                reCnt = cds.Member_Name_Search(ref Search_Mbid, txt_tag);

                if (reCnt == 1)
                {
                    if (tb.Name == "txtName_s")
                    {
                        mtxtMbid_s.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.        
                        if ( chk_Foreign_s.Checked == true)
                        {
                            Set_Form_Date(mtxtMbid_s.Text, "s");
                        }
                        else
                        {
                            if (Input_Error_Check(mtxtMbid_s, "s") == true)
                                Set_Form_Date(mtxtMbid_s.Text, "s");
                        }
                        //if (Input_Error_Check(mtxtMbid_s, "s") == true)
                        //    Set_Form_Date(mtxtMbid_s.Text, "s");

                        //SendKeys.Send("{TAB}");
                    }

                    if (tb.Name == "txtName_n")
                    {
                        mtxtMbid_n.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.       
                        if (chk_Foreign_n.Checked == true )
                        {
                            Set_Form_Date(mtxtMbid_n.Text, "n");
                        }
                        else
                        {
                            if (Input_Error_Check(mtxtMbid_n, "n") == true)
                                Set_Form_Date(mtxtMbid_n.Text, "n");
                        }
                        //if (Input_Error_Check(mtxtMbid_n, "n") == true)
                        //    Set_Form_Date(mtxtMbid_n.Text, "n");
                        //SendKeys.Send("{TAB}");
                    }
                }
                else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
                {

                    //cls_app_static_var.Search_Member_Name = txt_tag;
                    frmBase_Member_Search_NOM_SAVE e_f = new frmBase_Member_Search_NOM_SAVE();
                    frmBase_Member_Search_NOM_SAVE_Fo e_f_Fo = new frmBase_Member_Search_NOM_SAVE_Fo();
                    if (tb.Name == "txtName_s")
                    {
                        if (chk_Foreign_s.Checked == false)
                        {
                            e_f.Send_Mem_Number += new frmBase_Member_Search_NOM_SAVE.SendNumberDele(e_f_Send_Mem_Number);
                            e_f.Call_searchNumber_Info += new frmBase_Member_Search_NOM_SAVE.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);
                            e_f.ShowDialog();
                        }
                        if (chk_Foreign_s.Checked == true)
                        {
                            e_f_Fo.Send_Mem_Number += new frmBase_Member_Search_NOM_SAVE_Fo.SendNumberDele(e_f_Send_Mem_Number);
                            e_f_Fo.Call_searchNumber_Info += new frmBase_Member_Search_NOM_SAVE_Fo.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);
                            e_f_Fo.ShowDialog();
                        }
                    }

                    if (tb.Name == "txtName_n")
                    {
                        if (chk_Foreign_n.Checked == false)
                        {
                            e_f.Send_Mem_Number += new frmBase_Member_Search_NOM_SAVE.SendNumberDele(e_f_Send_Mem_Number_3);
                            e_f.Call_searchNumber_Info += new frmBase_Member_Search_NOM_SAVE.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info_3);
                            e_f.ShowDialog();
                        }
                        if (chk_Foreign_n.Checked == true)
                        {
                            e_f_Fo.Send_Mem_Number += new frmBase_Member_Search_NOM_SAVE_Fo.SendNumberDele(e_f_Send_Mem_Number_3);
                            e_f_Fo.Call_searchNumber_Info += new frmBase_Member_Search_NOM_SAVE_Fo.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info_3);
                            e_f_Fo.ShowDialog();
                        }
                    }

                    SendKeys.Send("{TAB}");
                }


            }
            else
                SendKeys.Send("{TAB}");

        }

        void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtName_s.Text.Trim();
        }


        void e_f_Send_MemName_Info_3(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtName_n.Text.Trim(); ;
        }



        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
            if (tab_Nation.Visible == true)
            {
                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex;
                if (combo_Se_Code.Text == "")  //다국어 지원프로그램을 사용시 국가는 필히 선택을 해야 된다.
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Not_Na_Code")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    combo_Se.Focus(); return;
                }
            }

            if (tb.Name == "txtCenter")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter_Code);
                if (tb.Text.ToString() == "")
                    Db_Grid_Popup(tb, txtCenter_Code, "");
                else
                    Ncod_Text_Set_Data(tb, txtCenter_Code);

                SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtBank")
            {
                Data_Set_Form_TF = 1;
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtBank_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtBank_Code);

                Db_Grid_Popup(tb, txtBank_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
                if (txtBank_Code.Text == "999")
                {
                    txtAccount.Text = "555555";
                }
            }


            if (tb.Name == "txt_C_Card")
            {
                Data_Set_Form_TF = 1;
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtBank_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtBank_Code);

                Db_Grid_Popup(tb, txt_C_Card_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }


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
                cgb_Pop.Next_Focus_Control = butt_AddCode;

            if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = txtName_Accnt;

            if (tb.Name == "txt_C_Card")
                cgb_Pop.Next_Focus_Control = txt_C_Name_3;

            cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, combo_Se_Code.Text.Trim());
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
                {
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);
                    cgb_Pop.Next_Focus_Control = butt_AddCode;
                }

                if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtBank")
                {
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);
                    cgb_Pop.Next_Focus_Control = txtName_Accnt;

                }
            }
            else
            {
                if (tb.Name == "txtCenter")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + combo_Se_Code.Text.Trim() + "') )";
                    if (combo_Se_Code.Text.Trim() != "") Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " And ncode <> '002'"; // 2018-11-23 지성경 에스제이로직스는 선택불가능하게끔한다.
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                    cgb_Pop.Next_Focus_Control = butt_AddCode;
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
                    if (combo_Se_Code.Text.Trim() != "") Tsql = Tsql + " Where  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);
                    cgb_Pop.Next_Focus_Control = txtName_Accnt;
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
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + combo_Se_Code.Text.Trim() + "') )";
                if (combo_Se_Code.Text.Trim() != "") Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                Tsql = Tsql + " And ncode <> '002'"; // 2018-11-23 지성경 에스제이로직스는 선택불가능하게끔한다.
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
                Tsql = Tsql + " Where (Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%' )";
                if (combo_Se_Code.Text.Trim() != "") Tsql = Tsql + " And  Na_Code = '" + combo_Se_Code.Text.Trim() + "'";
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







        private Boolean Input_Error_Check(MaskedTextBox m_tb, string s_Kind)
        {
            string T_Mbid = m_tb.Text;
            string Mbid = ""; int Mbid2 = 0;
            if (s_Kind == "s")
            {
                txtName_s.Text = ""; txtSN_s.Text = "";
            }
            if (s_Kind == "n")
            {
                txtName_n.Text = ""; txtSN_n.Text = "";
            }

            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == -1) //올바르게 회원번호 양식에 맞춰서 입력햇는가.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }
            string Tsql = "";
            Tsql = "Select Mbid , Mbid2, M_Name , Sell_Mem_TF , RBO_Mem_TF ";
            Tsql = Tsql + " , LineCnt , N_LineCnt  ";
            Tsql = Tsql + " , LeaveDate , LineUserDate  ";
            Tsql = Tsql + " , Saveid  , Saveid2  ";
            Tsql = Tsql + " , Nominid , Nominid2  ";
            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }

            //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            if (s_Kind != "s" && s_Kind != "n")
            {
                Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
                Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";
            }
            else
            {
                //Tsql += " AND tbl_Memberinfo.US_Num <> 0 ";
            }
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)  //실제로 존재하는 회원 번호 인가.
            {

                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }
            //++++++++++++++++++++++++++++++++   

            //여러가지 추/후 에러체크...customer에서 가져오기 때문에 막는다
            if (s_Kind == "n" || s_Kind == "s") //3인 경우는 새로운 지정 후원인인데.. 탈퇴나 라인중자가 아닌지를 체크한다.
            {
                if (ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString() != "")
                {

                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Leave_")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    m_tb.Focus(); return false;
                }

                if (ds.Tables[base_db_name].Rows[0]["LineUserDate"].ToString() != "")
                {

                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Delete_")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    m_tb.Focus(); return false;
                }


                if (s_Kind == "s")
                {
                    //후원인은 소비자로 할수는 없다. 추천인은 가능할듯.
                    if (int.Parse(ds.Tables[base_db_name].Rows[0]["RBO_Mem_TF"].ToString()) == 1)
                    {

                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Sell_TF_0")
                                + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                               + "\n" +
                               cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        m_tb.Focus(); return false;
                    }

                    //하선인원수를 체크한다.
                    if (csb.Member_Down_Save_TF(m_tb.Text.Trim()) == false)
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Down_Full")
                               + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                              + "\n" +
                              cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        m_tb.Focus(); return false;
                    }


                    if (chk_Top_s.Checked == false && chk_Top_n.Checked == false && chk_Foreign_n.Checked == false)
                    {
                        //입력 추천인 하부의 후원조직상에  입력 후원인이 존재해야 한다.
                        if (csb.Member_Down_Save_TF(m_tb.Text.Trim(), mtxtMbid_n.Text.Trim()) == false)
                        {
                            string Msg = "";
                            if (cls_User.gid_CountryCode == "TH")
                            {
                                Msg = "The sponsor you entered must exist on the sub-sponsorship organization you entered." + "\n" + "Do you want to proceed ?";
                            }
                            else
                            {
                                Msg = "입력하신 추천인 하부 후원조직상에 " + "\n" + "입력하신 후원인이 존재 해야 합니다." + "\n" + " 계속 진행하시겠습니까?";
                            }
                            
                            if (MessageBox.Show(Msg, "", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                m_tb.Focus(); return false;
                            }
                            ////Msg_Mem_Down_Nom_Save
                            //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Down_Nom_Save")
                            //      + "\n" +                                 
                            //      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            //m_tb.Focus(); return false;

                        }
                    }
                    if (chk_Top_s.Checked == false && chk_Top_n.Checked == false && chk_Foreign_n.Checked == true)
                    {
                        //입력 추천인 하부의 후원조직상에  입력 후원인이 존재해야 한다.
                        if (csb.Member_Down_Save_TF_Fo(m_tb.Text.Trim(), mtxtMbid_n.Text.Trim()) == false)
                        {
                            string Msg = "";
                            if (cls_User.gid_CountryCode == "TH")
                            {
                                Msg = "The sponsor you entered must exist on the sub-sponsorship organization you entered." + "\n" + "Do you want to proceed ?";
                            }
                            else
                            {
                                Msg = "입력하신 추천인 하부 후원조직상에 " + "\n" + "입력하신 후원인이 존재 해야 합니다." + "\n" + " 계속 진행하시겠습니까?";
                            }
                            
                            if (MessageBox.Show(Msg, "", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                m_tb.Focus(); return false;
                            }
                            ////Msg_Mem_Down_Nom_Save
                            //MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Down_Nom_Save")
                            //      + "\n" +                                 
                            //      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            //m_tb.Focus(); return false;

                        }
                    }
                    //int LineCnt = csb.LineCnt_Search_Save(Mbid, Mbid2);
                    //txtLineCnt.Text = LineCnt.ToString();

                    //if (txtLineCnt.Text.Equals("2"))
                    //{
                    //    rdoLineRight.Checked = true;
                    //}
                    //else
                    //{
                    //    rdoLineLeft.Checked = true;
                    //}

                }
            }

            return true;
        }

















        private void Form_Clear_()
        {
            Data_Set_Form_TF = 1;
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>                
            Base_Grid_Set(); //당일등록 회원을 불러온다.

            Base_Grid_Set_Good();   //상품 정보를 불러온다.
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Line_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Li.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this);

            opt_sell_2.Checked = true; opt_Bir_TF_1.Checked = true;
            raButt_IN_1.Checked = true;

            mtxtSn.Mask = "999999-9999999";
            mtxtRegDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            chk_Top_n.Checked = false; chk_Top_s.Checked = false;
            check_Auto.Checked = false;

            txtB1.Text = "0";
            check_BankDocument.Checked = true;
            check_CpnoDocument.Checked = true;

            //후원추천 기능 사용하지 말라고 하면 최상위로 체크를 해버린다.
            if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
            {
                chk_Top_s.Checked = true;
            }

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                chk_Top_n.Checked = true;
            }


            if (cls_app_static_var.Mem_Number_Auto_Flag == "H")
            {
                mtxtMbid.ReadOnly = false;
            }

            if (cls_app_static_var.Mem_Number_Auto_Flag == "A")
            {
                mtxtMbid.ReadOnly = true;
                mtxtMbid.BackColor = cls_app_static_var.txt_Enable_Color;

            }

            if (cls_app_static_var.Mem_Number_Auto_Flag == "R")
            {
                mtxtMbid.ReadOnly = true;
                mtxtMbid.BackColor = cls_app_static_var.txt_Enable_Color;
            }

            if (cls_app_static_var.Member_Cpno_Error_Check_TF == 1)
                check_Cpno_Err.Checked = true;

            if (cls_app_static_var.Member_Cpno_Put_TF == 1)
                check_Cpno.Checked = true;

            if (cls_app_static_var.Member_Reg_Multi_TF == 1)
                check_Cpno_Multi.Checked = true;
            tab_Sub.SelectedIndex = 0;

            check_LR.Checked = true;
            radioB_RBO.Checked = true;
            radioB_G8.Checked = true;
            opt_sell_2.Checked = true;

            checkB_SMS_FLAG.Checked = true;
            checkB_EMail_FLAG.Checked = true;

            radioB_Sex_Y.Checked = false;
            radioB_Sex_X.Checked = false;

            if (cls_User.gid_CountryCode == "TH")
            {
                combo_Se_Code_2.Text = "TH";
            }
            Data_Set_Form_TF = 0;
        }






        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0; Data_Set_Form_TF = 1;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex;

                //저장이 이루어진다.
                Save_Base_Data(ref Save_Error_Check);  //저장이 이루어진다

                Data_Set_Form_TF = 0;
                if (Save_Error_Check > 0)
                {

                    //---------------------------------------------------------
                    cls_Search_DB csd = new cls_Search_DB();
                    string T_Mbid = mtxtMbid.Text.Trim();
                    string Mbid = ""; int Mbid2 = 0;
                    csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);


                    //---미국에서 회원번호가 넘어오면 그때서야 가입 문자를 전송 처리한다 해서 회원번호 받아오는 프로시져 쪽으로 옴겻음  2020-09-15


                    //Send_SMS_Message_Congratulations_membership(Mbid.ToString(), Mbid2.ToString());                    
                    //new cls_sms().Congratulations_Membership(Mbid2);  
                    //---미국에서 회원번호가 넘어오면 그때서야 가입 문자를 전송 처리한다 해서 회원번호 받아오는 프로시져 쪽으로 옴겻음  2020-09-15

                    //태국한해서 전송
                    //if (combo_Se_Code_2.Text == "TH")
                    //{
                    //    Send_SMS_Message_Congratulations_membership(Mbid.ToString(), Mbid2.ToString());
                    //    new cls_sms().Congratulations_Membership(Mbid2);
                    //}
                    //태국한해서 전송
                    if (combo_Se_Code_2.Text == "TH")
                    {
                        new cls_sms().SMS_JoinMember_TH(Mbid2);
                        // Mail 호출 - 회원가입
                        new cls_Web().SendMail_TH(Mbid2, string.Empty, string.Empty, string.Empty, ESendMailType_TH.joinMail);
                    }

                    if (cls_User.gid_SellInput == 1 && cls_app_static_var.Mid_Main_Menu.ContainsKey("m_SellBase"))  //매출창 자동으로 뜨기를 선택한 경우에
                    {

                        string Send_Nubmer = ""; string Send_Name = ""; ; string Send_OrderNumber = "";
                        Send_OrderNumber = "";

                        Send_Nubmer = T_Mbid.ToString();
                        Send_Name = txtName.Text.ToString();
                        if (Send_Mem_Number != null)
                        {
                            Send_Mem_Number(Send_Nubmer, Send_Name, Send_OrderNumber);   //부모한테 이벤트 발생 신호한다.                        
                        }
                    }

                    Form_Clear_();
                    combo_Se_Code_2.Text = cls_User.gid_CountryCode;

                    if (cls_app_static_var.Mem_Number_Auto_Flag == "A" || cls_app_static_var.Mem_Number_Auto_Flag == "R")
                        txtName.Focus();
                    else
                        mtxtMbid.Focus();
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }


            else if (bt.Name == "butt_Clear")
            {
                Form_Clear_();

                if (cls_app_static_var.Mem_Number_Auto_Flag == "A" || cls_app_static_var.Mem_Number_Auto_Flag == "R")
                    txtName.Focus();
                else
                    mtxtMbid.Focus();

                combo_Se_Code_2.Text = cls_User.gid_CountryCode;
            }

            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }

            else if (bt.Name == "butt_AddCode")
            {
                frmBase_AddCode e_f = new frmBase_AddCode();
                e_f.Send_Address_Info += new frmBase_AddCode.SendAddressDele(e_f_Send_Address_Info);
                e_f.ShowDialog();
                txtAddress2.Focus();
            }


            else if (bt.Name == "butt_AddCode2")
            {
                frmBase_AddCode e_f = new frmBase_AddCode();
                e_f.Send_Address_Info += new frmBase_AddCode.SendAddressDele(e_f_Send_Address_Info2);
                e_f.ShowDialog();

                txtAddress2_Auto.Focus();
            }

            else if (bt.Name == "butt_AddCodeT1")
            {
                txtAddress_Auto.Text = txtAddress1.Text;
                txtAddress2_Auto.Text = txtAddress2.Text;
                mtxtZip_Auto.Text = mtxtZip1.Text;

                txtName_Auto.Text = txtName.Text;
                mtxtTel_Auto.Text = mtxtTel2.Text;

                txtAddress2_Auto.Focus();
            }


        }
        private void Send_SMS_Message_Congratulations_membership(string mbid, string mbid2)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = string.Format("EXEC Usp_Insert_SMS_New  '99', '{0}', {1}, '', '' ", mbid, mbid2);
            //Tsql = string.Format("EXEC Usp_Insert_SMS '10', '{0}', {1}, '', '' ", mbid, mbid2);

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Memberinfo", ds) == false)
                return;

        }
        void e_f_Call_Certify_Info(ref string Callmode)
        {
            Callmode = "M";
        }

        private void e_f_Send_Certify_Info(string SuccessYN, string Message, string Name, string DI, string CI, string BirthDay, string Gender, string NationalInfo, string Age, string VNumber, string AgeCode, string AuthInfo, string AuthType)
        {
            /*
            SuccessYN : 성공여부
            Message : 메세지 
            Name : 실명
            DI : DI값
            CI : CI값
            BirthDay : 생년월일
            Gender : 성  (1:남성, 2:여성)
            NationalInfo : 내/외국인 구분 
            Age : 만 나이
            VNumber : IPIN 데이터 가상주민번호
            AgeCode : IPIN 데이터 연령대코드
            AuthInfo : IPIN 데이터 본인확인수단 (0:공인인증서, 1:카드, 2:핸드폰, 3:대면확인, 4:기타)
            authType : 인증수단	(M:휴대폰, X:공인인증서, I:아이핀)
            */
            if (SuccessYN == "Y")
            {
                if (NationalInfo == "1")
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("As a result of authentication, you have been confirmed as a foreigner.\nPlease register as a foreigner.");
                    }
                    else
                    {
                        MessageBox.Show("인증결과 외국인으로 확인되었습니다.\n외국인으로 등록하시기 바랍니다.");
                    }
                    return;
                }

                txt_IpinCI.Text = CI;
                txt_IpinDI.Text = DI;
                txtName.Text = Name;
                txtName_Accnt.Text = Name;
                mtxtBrithDay.Text = BirthDay;
                if (Gender == "1")
                {
                    radioB_Sex_Y.Checked = true;
                    radioB_Sex_X.Checked = false;
                }
                else if (Gender == "2")
                {
                    radioB_Sex_Y.Checked = false;
                    radioB_Sex_X.Checked = true;
                }

                Lbl_Certify.Text = "인증확인";
                Lbl_Certify.ForeColor = Color.Blue;
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Authentication confirmed.");
                }
                else
                {
                    MessageBox.Show("인증 확인되었습니다.");
                }
            }
            else
            {
                Lbl_Certify.Text = "인증미확인";
                Lbl_Certify.ForeColor = Color.Red;
                MessageBox.Show(Message);
            }

        }



        private Boolean Check_Certify_Error()
        {
            if (raButt_IN_1.Checked == true)
            {
                if (txt_IpinDI.Text == "")
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please proceed after verifying your phone number.");
                    }
                    else
                    {

                        MessageBox.Show("휴대폰 인증 후에 진행하시기 바랍니다.");
                    }
                    return false;
                }
            }

            return true;
        }



        private void e_f_Send_Address_Info(string AddCode1, string AddCode2, string Address1, string Address2, string Address3)
        {
            mtxtZip1.Text = AddCode1 + "-" + AddCode2;
            txtAddress1.Text = Address1; txtAddress2.Text = Address2;


        }

        private void e_f_Send_Address_Info2(string AddCode1, string AddCode2, string Address1, string Address2, string Address3)
        {
            mtxtZip_Auto.Text = AddCode1 + "-" + AddCode2;
            txtAddress_Auto.Text = Address1; txtAddress2_Auto.Text = Address2;

        }

        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            //SendKeys.Send("{TAB}");
        }

        private void radioButt_Sn_MouseUp(object sender, MouseEventArgs e)
        {
            //RadioButton  trd = (RadioButton)sender ;

            //mtxtSn.Text = "";
            //if (trd.Name == "raButt_IN_1" || trd.Name == "raButt_IN_2")
            //    mtxtSn.Mask = "999999-9999999";
            //else            
            //    mtxtSn.Mask = "999-99-99999";

            if (raButt_IN_1.Checked == true)    //내국인 선택시
            {
                butt_Certify.Visible = false;
                Lbl_Certify.Visible = false;
            }
            else if (raButt_IN_2.Checked == true)   //외국인 선택시
            {
                butt_Certify.Visible = false;
                Lbl_Certify.Visible = false;
            }

            mtxtSn.Focus();
        }












        private void Base_Grid_Set()
        {
            cls_form_Meth cm = new cls_form_Meth();
            
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 
            string Tsql = "";

            Tsql = "Select  ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) ";
            else
                Tsql = Tsql + " tbl_Memberinfo.mbid2 ";

            Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

            Tsql = Tsql + ", tbl_Memberinfo.Cpno ";

            Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";

            Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";
            Tsql = Tsql + " , LEFT(tbl_Memberinfo.RegTime,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.RegTime,4),2) + '-' + RIGHT(tbl_Memberinfo.RegTime,2)   ";
            Tsql = Tsql + " , tbl_Memberinfo.hometel ";
            Tsql = Tsql + " , tbl_Memberinfo.hptel ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,tbl_Memberinfo.Saveid + '-' + Convert(Varchar,tbl_Memberinfo.Saveid2) ";
            else
                Tsql = Tsql + " ,tbl_Memberinfo.Saveid2 ";

            Tsql = Tsql + " , Isnull(Sav.M_Name,'') ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,tbl_Memberinfo.Nominid + '-' + Convert(Varchar,tbl_Memberinfo.Nominid2) ";
            else
                Tsql = Tsql + " ,tbl_Memberinfo.Nominid2 ";

            Tsql = Tsql + " , Isnull(Nom.M_Name,'') ";
            Tsql = Tsql + " , Case When tbl_Memberinfo.addcode1 <> '' Then  LEFT(tbl_Memberinfo.addcode1,3) +'-' + RIGHT(tbl_Memberinfo.addcode1,3) ELSE '' End ";

            Tsql = Tsql + " , tbl_Memberinfo.address1 ";
            Tsql = Tsql + " , tbl_Memberinfo.address2 ";
            //Tsql = Tsql + " , Case  When tbl_Memberinfo.Sell_Mem_TF = 0 then '판매원' ELSE  '소비자' End";
            Tsql = Tsql + " , Case  When tbl_Memberinfo.Sell_Mem_TF = 0 then '" + cm._chang_base_caption_search("판매원") + "' ELSE  '" + cm._chang_base_caption_search("소비자") + "' End";
            Tsql = Tsql + " , tbl_Memberinfo.recordid ";
            Tsql = Tsql + " , tbl_Memberinfo.recordtime ";

            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Sav (nolock) ON tbl_Memberinfo.Saveid = Sav.Mbid And tbl_Memberinfo.Saveid2 = Sav.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Nom (nolock) ON tbl_Memberinfo.Nominid = Nom.Mbid And tbl_Memberinfo.Nominid2 = Nom.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";
            Tsql = Tsql + " Where Replace(left(tbl_Memberinfo.Recordtime,10),'-','') = Replace (LEFT( Convert(Varchar(25),GetDate(),21) ,10 ) ,'-' , '') ";
            Tsql = Tsql + " And  tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
            Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";
            //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            //당일 등록된 회원을 불러온다.

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
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,encrypter.Decrypt( ds.Tables[base_db_name].Rows[fi_cnt][2].ToString (),"Cpno")
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][6].ToString ())
                                ,encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][7].ToString ())
                                ,ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][10]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][11]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][12]
                                ,encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][13].ToString ()) + " " + encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt][14].ToString ())
                                ,ds.Tables[base_db_name].Rows[fi_cnt][15]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][16]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][17]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][17]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][18]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][19]
                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }

        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 20;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"회원_번호"  , "성명"   , "주민번호"  , "위치"   , "센타명"
                                , "가입일"   , "집전화"    , "핸드폰"   , "후원인"    , "후원인명"
                                , "추천인"   , "추천인명"  , "우편_번호"   , "주소"   ,"구분"
                                , "기록자"     , "기록일"    , "" , ""     , ""
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 85, 90, 130, 60, 100
                             ,80, 130, 130, cls_app_static_var.save_uging_Pr_Flag , cls_app_static_var.save_uging_Pr_Flag
                             ,cls_app_static_var.nom_uging_Pr_Flag  , cls_app_static_var.nom_uging_Pr_Flag , 80,  200, 80
                             ,90 , 120, 0 , 0 ,  0
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //15   
                          
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //20
                              
                              };
            cgb.grid_col_alignment = g_Alignment;
        }






        private bool Check_TextBox_Error_Date()
        {


            cls_Check_Input_Error c_er = new cls_Check_Input_Error();
            if (mtxtRegDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtRegDate.Text, mtxtRegDate, "Date") == false)
                {
                    mtxtRegDate.Focus();
                    return false;
                }
            }

            if (mtxtBrithDay.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtBrithDay.Text, mtxtBrithDay, "Date") == false)
                {
                    mtxtBrithDay.Focus();
                    return false;
                }
            }

            if (mtxtVisaDay.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtVisaDay.Text, mtxtVisaDay, "Date") == false)
                {
                    mtxtVisaDay.Focus();
                    return false;
                }
            }

            if (mtxtEdDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtEdDate.Text, mtxtEdDate, "Date") == false)
                {
                    mtxtBrithDay.Focus();
                    return false;
                }
            }

            return true;
        }


        private bool Check_TextBox_CC_Error()
        {

            cls_Check_Text T_R = new cls_Check_Text();
            string me = "";

            me = T_R.Text_Null_Check(txtName_C, "Msg_Sort_M_Name"); //성명을 필히 넣어야 합니다.
            if (me != "")
            {
                MessageBox.Show(me);
                txtName_C.Focus();
                return false;
            }

            /* 2018-08-05 지성경 현재 부부사업자는 주민번호 체크하지아니함 
            if (mtxtSn_C.Text.Replace("-", "") == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Error")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtSn_C.Focus(); return false;
            }
            
            string Sn = mtxtSn_C.Text.Replace("-", "").Replace("_", "").Trim();
            if (Sn_Number_(Sn, mtxtSn_C) == false) return false;   //주민번호 입력 사항에 대해서 체크를 한다.                     
            */

            cls_Check_Input_Error c_er = new cls_Check_Input_Error();

            if (mtxtBrithDayC.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_1(mtxtBrithDayC.Text, mtxtBrithDayC, "Date") == false)
                {
                    mtxtBrithDayC.Focus();
                    return false;
                }
            }
            if (txtEmail_C.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_1(txtEmail_C.Text, txtEmail_C, "Email") == false)
                {
                    txtEmail_C.Focus();
                    return false;
                }
            }
            if (mtxtTel2_C.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_1(mtxtTel2_C.Text, mtxtTel2_C, "HpTel") == false)
                {
                    mtxtTel2_C.Focus();
                    return false;
                }
            }

            return true;
        }

        private bool Check_TextBox_Auto_Error()
        {


            if (cboRegDateA.Text.Replace("-", "").Trim() == "")
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("There is no autoship start date."
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                else
                {

                    MessageBox.Show("오토쉽 시작일이 없습니다."
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                cboRegDateA.Focus();
                return false;
            }

            //cls_Check_Input_Error c_er = new cls_Check_Input_Error();
            //if (cboRegDateA.Text.Replace("-", "").Trim() != "")
            //{
            //    if (Sn_Number_(cboRegDateA.Text, cboRegDateA, "Date") == false)
            //    {
            //        cboRegDateA.Focus();
            //        return false;
            //    }
            //}

            //미리 받을 수 있음 
            //string ToEndDate = cboRegDateA.Text.Replace("-", "").Trim();

            //if (int.Parse(ToEndDate) < int.Parse(cls_User.gid_date_time))
            //{
            //    MessageBox.Show("오토쉽 시작일이 오늘 날짜보다 전날짜 입니다."
            //        + "\n" +
            //        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

            //    cboRegDateA.Focus();
            //    return false;
            //}

            string PayDate = "";

            PayDate = cls_User.gid_date_time.Substring(0, 4) + '-' + cls_User.gid_date_time.Substring(4, 2) + '-' + cls_User.gid_date_time.Substring(6, 2);
            DateTime TodayDate = new DateTime();
            TodayDate = DateTime.Parse(PayDate);
            PayDate = TodayDate.AddDays(3).ToString("yyyy-MM-dd").Replace("-", "");

            //if (int.Parse(PayDate) > int.Parse(ToEndDate) )
            //{
            //    MessageBox.Show("오토쉽 시작일은 현날짜 기준 3일이후에 가능 합니다."
            //        + "\n" +
            //        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

            //    cboRegDateA.Focus();
            //    return false;
            //}

            //if (opt_sell_3.Checked != true)  //소비자는 오토쉽 관련해서 5,15,25일날 이루어 지기 때문에 이게 없어도 된다.
            //{
            //    if (TodayDate.DayOfWeek.ToString() != "Wednesday")   //최초 결제일자가 월요일이 아닌경우에는
            //    {
            //        MessageBox.Show("오토쉽 최초결제일은 수요일만 가능 합니다."
            //            + "\n" +
            //            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

            //        cboRegDateA.Focus();
            //        return false;
            //    }
            //}


            if (txt_C_Card_Code.Text == "")
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Please select an AutoShip card."
                         + "\n" +
                         cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                else
                {
                    MessageBox.Show("오토쉽 카드를 선택해 주십시요."
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                txt_C_Card.Focus();
                return false;
            }

            if (txt_C_Card_Number.Text == "")
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Please enter your AutoShip card number."
                         + "\n" +
                         cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                else
                {
                    MessageBox.Show("오토쉽 카드 번호를 입력해 주십시요."
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                txt_C_Card_Number.Focus();
                return false;
            }

            if (combo_C_Card_Year.Text == "")
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Please enter the validity period for the card."
                         + "\n" +
                         cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                else
                {
                    MessageBox.Show("카드 관련 유효기간을 입력해 주십시요."
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                combo_C_Card_Year.Focus();
                return false;
            }

            if (combo_C_Card_Month.Text == "")
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Please enter the validity period for the card."
                         + "\n" +
                         cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                else
                {
                    MessageBox.Show("카드 관련 유효기간을 입력해 주십시요."
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                combo_C_Card_Month.Focus();
                return false;
            }

            if (txt_C_P_Number.Text == "")
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Please enter the first two digits of your card password."
                         + "\n" +
                         cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                else
                {
                    MessageBox.Show("카드 비밀번호 앞 두리자리를 입력해 주십시요."
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                txt_C_P_Number.Focus();
                return false;
            }


            if (txt_C_B_Number.Text == "")
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Please enter your date of birth for the card."
                         + "\n" +
                         cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                else
                {
                    MessageBox.Show("카드 관련 생년월일 입력해 주십시요."
                    + "\n" +
                    cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                txt_C_B_Number.Focus();
                return false;
            }



            txt_Auto_Date.Text = "";
            if (combo_Auto_Date.Text != "")
                txt_Auto_Date.Text = combo_Auto_Date.Text;


            if (txt_Auto_Date.Text == "")
                txt_Auto_Date.Text = "0";


            //if (txt_Auto_Date.Text == "0")
            //{
            //    MessageBox.Show("오토쉽 매월 적용일이 없습니다."
            //        + "\n" +
            //        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

            //    txt_Auto_Date.Focus();
            //    return false;
            //}


            int Item_Pr = 0;
            int Item_Pr2 = 0;
            int ItemCnt = 0;
            for (int i = 0; i < dGridView_Good.Rows.Count; i++)
            {
                if (int.Parse(dGridView_Good.Rows[i].Cells[0].Value.ToString()) > 0)
                {
                    ItemCnt = int.Parse(dGridView_Good.Rows[i].Cells[0].Value.ToString());
                    Item_Pr = Item_Pr + (int.Parse(dGridView_Good.Rows[i].Cells[3].Value.ToString()) * ItemCnt);
                    Item_Pr2 = Item_Pr2 + (int.Parse(dGridView_Good.Rows[i].Cells[4].Value.ToString()) * ItemCnt);
                }
            }

            txt_Auto_PR.Text = string.Format(cls_app_static_var.str_Currency_Type, Item_Pr);
            txt_Auto_PR2.Text = string.Format(cls_app_static_var.str_Currency_Type, Item_Pr2);

            if (Item_Pr < 60)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("AutoShip standard PV is 60 or higher."
                         + "\n" +
                         cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                else
                {
                    MessageBox.Show("오토쉽 기준 PV 는 60 이상 입니다."
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                dGridView_Good.Focus();
                return false;
            }


            if (ItemCnt == 0)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("There are no autoship selection products."
                         + "\n" +
                         cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                else
                {
                    MessageBox.Show("오토쉽 선택 상품이 없습니다."
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                dGridView_Good.Focus();
                return false;
            }

            return true;
        }


        private Boolean Check_TextBox_Error()
        {
            //20201229구현호 일단 김종민대리가 mk costomer 생년월일 업뎃못할숟있으니 체크패스해달라고한다.
            //if(txtSN_n.Text== "" || txtSN_s.Text == "")
            //{
            //    string me = "";
            //    me = "추천인과 후원인을 조회 해 주시기 바랍니다." + "\n";

            //    MessageBox.Show(me);
            //    mtxtMbid.Focus();
            //    return false;
            //}
            ////if (mtxtMbid_n.Text.Replace("-", "").Replace("_", "").Replace(" ", "") == "" && txtName_n.Text == "" && txtSN_n.Text == "")
            ////    chk_Top_n.Checked = true ;

            ////if (mtxtMbid_s.Text.Replace("-", "").Replace("_", "").Replace(" ", "") == "" && txtName_s.Text == "" && txtSN_s.Text == "")
            ////    chk_Top_s.Checked = true ;


            if (tab_Nation.Visible == true)
            {
                if (combo_Se_Code.Text == "")  //다국어 지원프로그램을 사용시 국가는 필히 선택을 해야 된다.
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Not_Na_Code")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    combo_Se.Focus(); return false;
                }
            }

            if (chk_Top_n.Checked == false && chk_Foreign_n.Checked == false )
                if (Input_Error_Check(mtxtMbid_n, "n") == false) return false;  //추천인 관련 오류 체크  

            if (chk_Top_s.Checked == false && chk_Foreign_s.Checked == false)
                if (Input_Error_Check(mtxtMbid_s, "s") == false) return false; //후원인 관련 오류 체크                        


            //2017-05-02 김종국 이사 요청에 의해서 추천인 최상위 막음 메일로 요청옴
            //if (radioB_Begin.Checked == true)
            //{
            //    if (chk_Top_n.Checked == false)
            //    {
            //        MessageBox.Show("비긴즈는 추천인을 지정할수 없습니다.  추천인 최상위에 체크하신후에 다시 시도해 주십시요."
            //          + "\n" +
            //          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

            //        chk_Top_n.Focus(); return false;
            //    }
            //}


            cls_Search_DB csd = new cls_Search_DB();
            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

            Boolean Top_Check_TF = chk_Top_n.Checked; //기본적으로 추천인 기준으로 해서 번호를 새로 따지만 추천선택이 최상위로 했다.
            string Base_Up_Number = mtxtMbid_n.Text.Trim();

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                Top_Check_TF = chk_Top_s.Checked; //추천인 기능 사용안하므로 후원인쪽에서 연결을 찾는다.
                Base_Up_Number = mtxtMbid_s.Text.Trim();
            }


            if (Top_Check_TF == true)
            {
                if (Mbid2 == 0) //입력된 회원번호가 없다.
                {
                    if (cls_app_static_var.Mem_Number_Auto_Flag == "A")  //회원번호 자동 생성
                    {
                        mtxtMbid.Text = csd.Auto_Member_Number_Search(Base_Up_Number);
                    }

                    if (cls_app_static_var.Mem_Number_Auto_Flag == "R")  //회원번호 랜덤 생성
                    {
                        mtxtMbid.Text = csd.Auto_Member_Number_Search_Random(Base_Up_Number);
                    }

                    if (cls_app_static_var.Mem_Number_Auto_Flag == "H")  //회원번호가 수동 생성
                    {
                        //수동인데 입력된 회원번호가 없거나 올바르지 않은 경우 만들어 줄지를 물보고 승낙하면 만들어 준다.
                        if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Hand_Not_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            mtxtMbid.Focus();
                            return false;
                        }
                        mtxtMbid.Text = csd.Auto_Member_Number_Search(Base_Up_Number);
                    }
                }
            }

            else
            {
                if (Mbid2 == 0) //자동번호 부여인데 입력된 회원번호가 없다.
                {
                    if (cls_app_static_var.Mem_Number_Auto_Flag == "A")  //회원번호 자동 생성
                    {
                        string N_Mbid = ""; int N_Mbid2 = 0;

                        //회원번호 자동인 경우에는 추천인 번호 또는 후원인 번호 를 받아서 하는데 최상위도 아닌데 추천인 번호 또는 후원인 번호가 오류가 나면 메시지를 뛰운다.
                        if (csd.Member_Nmumber_Split(Base_Up_Number, ref N_Mbid, ref N_Mbid2) < 0)
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Auto_Nomin")
                                + "\n" +
                                cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            mtxtMbid.Focus();
                            return false;
                        }
                        mtxtMbid.Text = csd.Auto_Member_Number_Search(Base_Up_Number);
                    }

                    if (cls_app_static_var.Mem_Number_Auto_Flag == "R")  //회원번호 랜덤 생성인 경우 만들어 준다.
                    {
                        mtxtMbid.Text = csd.Auto_Member_Number_Search_Random(Base_Up_Number);
                    }

                    if (cls_app_static_var.Mem_Number_Auto_Flag == "H")  //회원번호가 수동 생성
                    {
                        //수동인데 입력된 회원번호가 없거나 올바르지 않은 경우 만들어 줄지를 물보고 승낙하면 만들어 준다.
                        if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Hand_Not_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            mtxtMbid.Focus();
                            return false;
                        }
                        mtxtMbid.Text = csd.Auto_Member_Number_Search(Base_Up_Number);
                    }
                }
                else //최상위도 선택을 하지 않았고 회원번호가 입려이 되어 있다.
                {
                    if (cls_app_static_var.Mem_Number_Auto_Flag == "H" && Mbid_Number_Hand_Check_TF == 0)  //회원번호가 수동 생성셋팅인데 회원번호를 입력하지 않았다.
                    {

                        string Hand_M_Number = csd.Auto_Member_Number_Search_Hand(mtxtMbid.Text.Trim());

                        if (Hand_M_Number == "")
                        {
                            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Not")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                            mtxtMbid.Focus();
                            return false;
                        }
                        else
                            mtxtMbid.Text = Hand_M_Number;
                    }
                    //else
                    //{
                    //    //입력된 회원번호가 이미 등록된 번호인지를 체크한다.
                    //    cls_Search_DB cds = new cls_Search_DB();
                    //    string Search_Name = cds.Member_Search_Base(T_Mbid); //회원번호가 이미 존재하는 번호인지를 체크한다. 존재하는 번호이면 그 번호의 회원명을 돌려줌

                    //    if (Search_Name == "")
                    //    {
                    //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Not")
                    //       + "\n" +
                    //       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    //        mtxtMbid.Focus();
                    //        return false;
                    //    }
                    //}
                }
            }




            return true;

        }



        private Boolean Check_TextBox_ETC_Error()
        {

            string Sn = mtxtSn.Text.Replace("-", "").Replace("_", "").Trim();
            if (Sn_Number_(Sn, mtxtSn) == false) return false;   //주민번호 입력 사항에 대해서 체크를 한다.
            if (txtWebID.Text == "")  //웹아이디 필수값으로넣는다
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("You need to register your web ID.");
                }
                else
                {
                    MessageBox.Show("웹아이디를 등록해야 합니다.");
                }
                txtWebID.Focus();
                return false;
            }
            if (txtWebID.Text != "")  //웹아이디가 등록 되는 경우에는 유일한 값인지 체크한다.
            {
                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                string Tsql;
                Tsql = "Select Mbid,Mbid2  ";
                Tsql = Tsql + " From tbl_Memberinfo  (nolock)  ";
                Tsql = Tsql + " Where Webid = '" + txtWebID.Text.Trim() + "' ";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Memberinfo", ds) == true)
                {
                    int ReCnt = Temp_Connect.DataSet_ReCount;
                    if (ReCnt > 0)
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Webid_Not")
                        );
                        txtWebID.Focus();
                        //20200929구현호 메세지만 띄우고 빠진다 -20210614 다시 완전탈출로 바꼈다
                        return false;
                    }
                }
            }

            if (cls_User.gid_For_Save_TF != 1 && raButt_IN_2.Checked == true)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Foreigners do not have permission to register."
                  + "\n" +
                  cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                else
                {

                    MessageBox.Show("외국인 등록 권한 없습니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                }
                raButt_IN_2.Focus();
                return false;
            }

            //후원최상위로 입력되는 경우네는 강제로 1을 넣는다 라인에
            if (chk_Top_s.Checked == true)
            {
                txtLineCnt.Text = "1";
                rdoLineLeft.Checked = true;
            }

            cls_Check_Text T_R = new cls_Check_Text();
            string me = "";


            me = T_R.Text_Null_Check(txtName, "Msg_Sort_M_Name"); //성명을 필히 넣어야 합니다.
            if (me != "")
            {
                MessageBox.Show(me);
                return false;
            }

            if (radioB_Sex_X.Checked == false && radioB_Sex_Y.Checked == false)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Please select your gender.");
                }
                else
                {
                    MessageBox.Show("성별을 선택해주시기바랍니다.");
                }
                radioB_Sex_X.Focus();
                return false;
            }

            if (txtName_Accnt.Text != "" && txtName_Accnt.Text.Trim() != txtName.Text.Trim())
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    if (MessageBox.Show("The member name you entered and the account holder name are not the same. Would you like to proceed?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                        return false;
                }
                else
                {

                    if (MessageBox.Show("입력하신 회원명과 예금주명이 동일하지 않습니다. 계속 진행하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                        return false;
                }
            }

            //생년월일을 주민벚호 기준으로 넣는다.
            if (mtxtBrithDay.Text.Replace("-", "") == "" || mtxtBrithDay.Text.Replace("-", "").Length != 8)
            {
                string BirthDay2 = "";
                Sn = mtxtSn.Text.Replace("-", "").Replace("_", "").Trim();

                if (Sn != "")
                {
                    cls_Sn_Check csn_C = new cls_Sn_Check();
                    if (csn_C.check_19_nai(Sn, ref BirthDay2) == false)

                        mtxtBrithDay.Text = BirthDay2;
                }
            }

            //생년월일을 필수 값으로 지정 했음
            if (mtxtBrithDay.Text.Replace("-", "") == "" || mtxtBrithDay.Text.Replace("-", "").Length != 8)
            {
                me = cls_app_static_var.app_msg_rm.GetString("Msg_Sort_BirthDay") + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                mtxtBrithDay.Focus();
                return false;
            }

            ////센타를 필수 값으로 지정하기로함.
            //if (txtCenter_Code.Text == "")
            //{
            //    me = "센터를 필히 선택해 주십시요." + "\n" +
            //     cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

            //    MessageBox.Show(me);
            //    txtCenter.Focus();
            //    return false;
            //}

            ////First 영문이름 
            if (txtName_E_1.Text == "")
            {
                me = cls_app_static_var.app_msg_rm.GetString("Msg_Sort_E_Name_F") + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                txtName_E_1.Focus();
                return false;
            }

            //Last 영문이름
            if (txtName_E_2.Text == "")
            {
                me = cls_app_static_var.app_msg_rm.GetString("Msg_Sort_E_Name_L") + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                txtName_E_2.Focus();
                return false;
            }
            //우편번호
            // 태국인 경우
            if (combo_Se_Code_2.Text == "TH")
            {
                if (cbProvince_TH.SelectedIndex < 0)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify province.");
                    }
                    else
                    {
                        MessageBox.Show("Province를 지정해주세요");
                    }
                    cbProvince_TH.Focus();
                    return false;
                }
                else if (cbDistrict_TH.SelectedIndex < 0)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify district.");
                    }
                    else
                    {
                        MessageBox.Show("district를 지정해주세요");
                    }
                    cbDistrict_TH.Focus();
                    return false;
                }
                else if (cbSubDistrict_TH.SelectedIndex < 0)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify subdistrict.");
                    }
                    else
                    {
                        MessageBox.Show("subdistrict를 지정해주세요");
                    }
                    cbDistrict_TH.Focus();
                    return false;
                }
                else if (txtZipCode_TH.Text == "")
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify postal code.");
                    }
                    else
                    {
                        MessageBox.Show("우편번호를 지정해주세요");
                    }
                    txtZipCode_TH.Focus();
                    return false;
                }

                //핸드폰번호 - 태국
                if (mtxtTel2.Text.Replace("-", "") == "" || mtxtTel2.Text.Replace(" ", "").Replace("-", "").Length <= 9)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify your cell phone number.");
                    }
                    else
                    {
                        MessageBox.Show("핸드폰번호를 지정해주세요");
                    }
                    mtxtTel2.Focus();
                    return false;
                }
            }
            // 그 외 국가인 경우
            else
            {
                if (mtxtZip1.Text == "")
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify postal code.");
                    }
                    else
                    {
                        MessageBox.Show("우편번호를 지정해주세요");
                    }
                    mtxtZip1.Focus();
                    return false;
                }

                //핸드폰번호 - 한국
                if (mtxtTel2.Text.Replace("-", "") == "" || mtxtTel2.Text.Replace(" ", "").Replace("-", "").Length != 11)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Please specify your cell phone number.");
                    }
                    else
                    {
                        MessageBox.Show("핸드폰번호를 지정해주세요");
                    }
                    mtxtTel2.Focus();
                    return false;
                }

            }




            if (mtxtRegDate.Text.Replace("-", "") == "") //등록일자가 빈칸으로 되어 잇으면 당일을 셋팅한다.
                mtxtRegDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ////집전화
            //Sn = mtxtTel1.Text.Replace("-", "").Replace("_", "").Trim();
            //if (Sn_Number_1(Sn, mtxtTel1, "Tel") == false)
            //{
            //    mtxtTel1.Focus();
            //    return false;
            //}

            ////핸드폰
            //Sn = mtxtTel2.Text.Replace("-", "").Replace("_", "").Trim();
            //if (Sn_Number_1(Sn, mtxtTel2, "HpTel") == false)
            //{
            //    mtxtTel2.Focus();
            //    return false;
            //}

            //이메일
            Sn = txtEmail.Text.Replace("-", "").Replace("_", "").Trim();
            if (Sn_Number_1(Sn, txtEmail, "Email") == false)
            {
                txtEmail.Focus();
                return false;
            }

            //집주소
            //Sn = mtxtZip1.Text.Replace("-", "").Replace("_", "").Trim();
            //if (Sn_Number_1(Sn, mtxtZip1, "Zip") == false)
            //{
            //    mtxtZip1.Focus();
            //    return false;
            //}



            if (chk_Top_n.Checked == true)
            {
                if (txtName_n.Text != "" || txtSN_n.Text != "" || mtxtMbid_n.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Up_Checked")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Nomin")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtMbid_n.Focus();
                    return false;
                }
            }

            if (chk_Top_s.Checked == true)
            {
                if (txtName_s.Text != "" || txtSN_s.Text != "" || mtxtMbid_s.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Up_Checked")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Save")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtMbid_s.Focus();
                    return false;
                }
            }

            if (txtB1.Text.Trim() == "") txtB1.Text = "0";

            ////계좌인증 
            //if (lbl_ACC.Text.Equals("Success") == false)
            //{
            //    if (MessageBox.Show("계좌 미인증 상태 입니다. 미인증 상태에서 회원정보를 저장하시겠습니까?", "계좌인증 확인"
            //        , MessageBoxButtons.YesNo) == DialogResult.No)
            //    {
            //        txtAccount.Focus();
            //        return false;
            //    }
            //}

            ////휴대폰인증
            //if (txt_IpinCI.Text.Equals(string.Empty) && txt_IpinDI.Text.Equals(string.Empty))
            //{
            //    if (MessageBox.Show("휴대폰 미인증 상태 입니다. 미인증 상태에서 회원정보를 저장하시겠습니까?", "휴대폰인증확인"
            //        , MessageBoxButtons.YesNo) == DialogResult.No)
            //    {
            //        txtAccount.Focus();
            //        return false;
            //    }
            //}


            //후원인 라인 선택 
            if (cls_app_static_var.Member_Reg_Line_Select_TF == 1) //위치를 선택하는 옵션인 경우에
            {

                if (rdoLineRight.Checked.Equals(false) && rdoLineLeft.Checked.Equals(false))
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Be sure to select the left and right positions. 1 left 2 right."
              + "\n" +
              cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        txtWebID.Focus();
                    }
                    else
                    {

                        MessageBox.Show("좌우 위치를 필히 선택 해 주십시요. 1 좌측   2 우측 입니다."
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        txtWebID.Focus();
                    }
                    return false;
                }

            }

            return true;
        }

        /// <summary> 동일인물이있는가? </summary>
        private Boolean Check_Duplication_Error()
        {
            //20180807 현재는 CI_DI를 필수적으로 받지않으닌까! 
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT isnull(count(*), 0) cnt");
            sb.AppendLine("FROM tbl_memberinfo (nolock) ");
            sb.AppendLine("WHERE LeaveCheck = 1 ");
            // sb.AppendLine(string.Format("and Email = '{0}'", txtEmail.Text));
            sb.AppendLine(string.Format("and M_Name = '{0}'", txtName.Text));
            sb.AppendLine(string.Format("and (BirthDay+BirthDay_M+BirthDay_D) = '{0}'", mtxtBrithDay.Text.Replace("-", "")));

            DataSet ds = new DataSet();
            if (Temp_Connect.Open_Data_Set(sb.ToString(), "Check_Join", ds, this.Name, this.Text) == false) return false;
            if (Temp_Connect.DataSet_ReCount == 0) return true;

            int RowValue = 0;
            if (int.TryParse(ds.Tables["Check_Join"].Rows[0][0].ToString(), out RowValue))
            {
                if (RowValue > 0)
                {
                    MessageBox.Show(string.Format("{0}님 이름과 생년월일로 중복 체크 결과 {1}명이있는것을 확인했습니다."
                        , txtName.Text
                        , RowValue));
                    //20200929구현호 메세지만 띄우고 빠진다
                    //return false;
                }
            }

            return true;
        }

        private Boolean Check_Duplication_Error1()
        {
            //핸드폰중복체크
            //20180807 현재는 CI_DI를 필수적으로 받지않으닌까! 
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT isnull(count(*), 0) cnt");
            sb.AppendLine("FROM tbl_memberinfo (nolock) ");
            sb.AppendLine("WHERE LeaveCheck = 1 ");
            // sb.AppendLine(string.Format("and Email = '{0}'", txtEmail.Text));
            sb.AppendLine(string.Format("and M_Name = '{0}'", txtName.Text));
            sb.AppendLine(string.Format("and hptel = '{0}'", mtxtTel2.Text));

            DataSet ds = new DataSet();
            if (Temp_Connect.Open_Data_Set(sb.ToString(), "Check_Join", ds, this.Name, this.Text) == false) return false;
            if (Temp_Connect.DataSet_ReCount == 0) return true;

            int RowValue = 0;
            if (int.TryParse(ds.Tables["Check_Join"].Rows[0][0].ToString(), out RowValue))
            {
                if (RowValue > 0)
                {
                    MessageBox.Show(string.Format("{0}님 이름과 핸드폰번호로 중복 체크 결과 {1}명이있는것을 확인했습니다."
                        , txtName.Text
                        , RowValue));
                    //20200929구현호 메세지만 띄우고 빠진다
                    //return false;
                }
            }

            return true;
        }
        private Boolean Check_Duplication_Error2()
        {
            //집전화중복체크
            //20180807 현재는 CI_DI를 필수적으로 받지않으닌까! 
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT isnull(count(*), 0) cnt");
            sb.AppendLine("FROM tbl_memberinfo (NOLOCK) ");
            sb.AppendLine("WHERE LeaveCheck = 1 ");
            // sb.AppendLine(string.Format("and Email = '{0}'", txtEmail.Text));
            sb.AppendLine(string.Format("and M_Name = '{0}'", txtName.Text));
            sb.AppendLine(string.Format("and hometel = '{0}'", mtxtTel1.Text.Replace("-", "")));

            DataSet ds = new DataSet();
            if (Temp_Connect.Open_Data_Set(sb.ToString(), "Check_Join", ds, this.Name, this.Text) == false) return false;
            if (Temp_Connect.DataSet_ReCount == 0) return true;

            int RowValue = 0;
            if (int.TryParse(ds.Tables["Check_Join"].Rows[0][0].ToString(), out RowValue))
            {
                if (RowValue > 0)
                {
                    MessageBox.Show(string.Format("{0}님 이름과 집전화번호로 중복 체크 결과 {1}명이있는것을 확인했습니다."
                        , txtName.Text
                        , RowValue));
                    return false;
                }
            }

            return true;
        }
        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            //if (Check_Certify_Error() == false) return; //회원 핸드폰인증이 되지않으면 탈출

            if (Check_Duplication_Error() == false) return; //생년월일 동명이인 중복값있으면 탈출
            if (Check_Duplication_Error1() == false) return; //핸드폰 동명이인 중복값있으면 탈출 
                                                             //집전화는 선택 if (Check_Duplication_Error2() == false) return; //집전화 동명이인 중복값있으면 탈출 

            if (Check_TextBox_ETC_Error() == false) return;  //전화번호 웹아이디 주민번호 같은 부가적인 입력 사항에 대한 오류를 체크한다.
            if (Check_TextBox_Error_Date() == false) return; //날짜 관련 텍스트 파일들에 대해서 날짜 오류를 체크한다
            if (Check_TextBox_Error() == false) return;  //추천인과 후원인 회원번호에 대한 오류를 체크한다   

            if (check_Auto.Checked == true)
                if (Check_TextBox_Auto_Error() == false) return;  //오토쉽 등록 관련 오류를 체크한다.

            if (check_CC.Checked == true)
            {
                if (Check_TextBox_CC_Error() == false) return;  //부부사업자 등록 관련 오류를 체크한다.
            }


            Save_Error_Check = 0;
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            cls_Search_DB csd = new cls_Search_DB();

            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0; string today;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

            if (Mbid2 == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Mbid_Not")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                return;
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();



            try
            {
                string hometel = ""; string hptel = ""; string t_Sn = "";
                int LineCnt = 0; int N_LineCnt = 0; ;
                string Nominid = ""; int Nominid2 = 0;
                string Saveid = ""; int Saveid2 = 0;
                string BirthDay = ""; string BirthDay_M = ""; string BirthDay_D = ""; int BirthDayTF = 0;
                int For_Kind_TF = 0; int Sell_Mem_TF = 0, G8_TF = 0;
                int BankDocument = 0, CpnoDocument = 0, RBO_Mem_TF = 0;




                if (mtxtTel1.Text.Replace("-", "").Trim() != "") hometel = mtxtTel1.Text;
                if (mtxtTel2.Text.Replace("-", "").Trim() != "") hptel = mtxtTel2.Text;

                t_Sn = mtxtSn.Text.Replace("-", "").Replace("_", "").Trim();

                if (check_BankDocument.Checked == true) BankDocument = 1;
                if (check_CpnoDocument.Checked == true) CpnoDocument = 1;

                if (chk_Top_n.Checked == true)
                {
                    Nominid = "**"; Nominid2 = 0; N_LineCnt = 1;
                }
                else
                {
                    T_Mbid = mtxtMbid_n.Text;
                    csd.Member_Nmumber_Split(T_Mbid, ref Nominid, ref Nominid2);
                    N_LineCnt = csd.N_LineCnt_Search_Nom(Nominid, Nominid2);
                }

                if (chk_Top_s.Checked == true)
                {
                    Saveid = "**"; Saveid2 = 0; LineCnt = 1;
                }
                else
                {
                    T_Mbid = mtxtMbid_s.Text;
                    csd.Member_Nmumber_Split(T_Mbid, ref Saveid, ref Saveid2);
                    int LineCnt_Tmp = csd.LineCnt_Search_Save(Saveid, Saveid2);

                    LineCnt = rdoLineLeft.Checked ? 1 : (LineCnt_Tmp == 1 ? 2 : LineCnt_Tmp);//csd.LineCnt_Search_Save(Saveid, Saveid2);
                }


                if (N_LineCnt <= 0 || LineCnt <= 0) //주문번호 미발급시 오류로 해서 되돌린다.  
                {

                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));

                    tran.Dispose();
                    Temp_Connect.Close_DB();

                    return;
                }


                if (opt_sell_3.Checked == true) Sell_Mem_TF = 1; //소비자는 1 판매원은 기본 0

                if (raButt_IN_2.Checked == true) For_Kind_TF = 1;// 내국인은 0 외국인은 1  사업자는 2
                if (raButt_IN_3.Checked == true) For_Kind_TF = 2;

                if (opt_Bir_TF_1.Checked == true) BirthDayTF = 1; //양력은 1  음력은 2
                if (opt_Bir_TF_2.Checked == true) BirthDayTF = 2;

                if (radioB_RBO.Checked == true) RBO_Mem_TF = 0;// RBO 0 비긴즈 1
                if (radioB_Begin.Checked == true) RBO_Mem_TF = 1;

                if (radioB_G8.Checked == true) G8_TF = 8;// RBO 0 비긴즈 1
                if (radioB_G4.Checked == true) G8_TF = 4;


                if (mtxtBrithDay.Text.Replace("-", "").Trim() != "")
                {
                    string[] Sn_t = mtxtBrithDay.Text.Split('-');

                    BirthDay = Sn_t[0];  //생년월일을 년월일로 해서 쪼갠다
                    BirthDay_M = Sn_t[1]; //웹쪽 관련해서 이렇게 받아들이는데가 많아서
                    BirthDay_D = Sn_t[2]; //웹쪽 기준에 맞춘거임.
                }

                // string Na_Code = combo_Se_Code.Text.Trim();
                // if (Na_Code == "" || tab_Nation.Visible == false) Na_Code = "KR";

                string Na_Code = combo_Se_Code_2.Text.Trim();

                if (Na_Code == "") Na_Code = "KR";

                if (txtPassword.Text.Trim() == string.Empty)
                {
                    txtPassword.Text = "manna" + mtxtBrithDay.Text.Replace("-", "").Trim().Substring(2, 6);
                }


                //20200609구현호 개인정보사용동의 업데이트
                int Third_Person_Agree = 0;
                if (checkB_Third_Person_Agree.Checked == true) Third_Person_Agree = 1;


                //20201003 마케팅수신동의 업데이트
                string AgreeMarketing = "N";
                if (checkB_AgreeMarketing.Checked == true) AgreeMarketing = "Y";


                string Sex_FLAG = "";
                if (radioB_Sex_Y.Checked == true) Sex_FLAG = "Y";
                if (radioB_Sex_X.Checked == true) Sex_FLAG = "X";


                string AgreeSMS = "N";
                string AgreeEmail = "N";

                if (checkB_SMS_FLAG.Checked == true) AgreeSMS = "Y";
                if (checkB_EMail_FLAG.Checked == true) AgreeEmail = "Y";

                StringBuilder sb = new StringBuilder();
                string StrSql = "";

                string sAddressCode = (combo_Se_Code_2.Text == "TH") ? txtZipCode_TH.Text : mtxtZip1.Text.Replace("-", "");  // 태국이면 태국코드, 그 외 한국코드

                sb.Append("Insert into tbl_Memberinfo ");
                sb.Append(" (");
                sb.Append(" Mbid  ");
                sb.Append(" , Mbid2 ");
                sb.Append(" , m_name ");
                sb.Append(" , e_name ");
                sb.Append(" , E_name_Last ");
                sb.Append(" , email ");
                sb.Append(" , Cpno ");
                sb.AppendLine(" , addcode1 ");
                sb.Append(" , address1 ");
                sb.Append(" , address2 ");
                sb.Append(" , hometel");
                sb.Append(" , hptel");
                sb.Append(" , LineCnt ");
                sb.Append(" , N_LineCnt ");
                sb.Append(" , recordid ");
                sb.Append(" , recordtime ");
                sb.Append(" , businesscode ");
                sb.AppendLine(" , bankcode ");
                sb.Append(" , bankaccnt ");
                sb.Append(" , bankowner ");
                sb.Append(" , regtime ");
                sb.Append(" , saveid ");
                sb.Append(" , saveid2 ");
                sb.Append(" , nominid ");
                sb.Append(" , nominid2 ");
                sb.Append(" , regdocument,bankdocument , cpnodocument ");
                sb.Append(" , Remarks ");
                sb.AppendLine(" , LeaveCheck,LineUserCheck ");
                sb.Append(" , LeaveDate,LineUserDate ");
                sb.Append(" , LeaveReason,LineDelReason");
                sb.Append(" , WebID ");
                sb.Append(" , WebPassWord ");
                sb.Append(" , BirthDay ");
                sb.Append(" , BirthDay_M ");
                sb.Append(" , BirthDay_D ");
                sb.Append(" , BirthDayTF ");
                sb.Append(" , Ed_Date  ");
                sb.AppendLine(" , For_Kind_TF ");
                sb.Append(" , Sell_Mem_TF ");
                sb.Append(" , GiBu_ ");
                sb.Append(" , Na_Code ");
                sb.Append(" , Reg_bankaccnt ");
                sb.Append(" , VisaDate ");
                sb.Append(" , RBO_Mem_TF ");
                sb.Append(" , RBO_S_Date ");
                sb.AppendLine(" , G8_TF ");
                sb.Append(" , Sex_FLAG");
                sb.Append(" , AgreeSMS");
                sb.Append(" , AgreeEmail");
                sb.Append(" , ipin_ci");
                sb.Append(" , ipin_di");

                if (check_CC.Checked == true)
                {
                    sb.Append(" , C_M_Name ");
                    sb.Append(" , C_For_Kind_TF ");
                    //sb.Append(" , C_cpno ");
                    //sb.Append(" , C_E_name ");
                    //sb.Append(" , C_E_name_Last ");
                    //sb.Append(" , C_Cop ");                    
                    sb.Append(" , C_BirthDay ");
                    sb.Append(" , C_BirthDay_M ");
                    sb.Append(" , C_BirthDay_D ");
                    sb.Append(" , C_hptel");
                    sb.AppendLine(", C_Email");
                }
                sb.Append(",Third_Person_Agree");
                sb.Append(",Base_Mbid2");
                sb.Append(",AgreeMarketing");
                sb.Append(",Nation_Code");
                sb.Append(",Account_Wait_FLAG");

                if (combo_Se_Code_2.Text == "TH")
                {
                    sb.Append(" , city");
                    sb.Append(" , state");
                }

                sb.AppendLine(") Values ( ");
                sb.AppendLine("'" + Mbid + "'");
                sb.AppendLine("," + Mbid2);
                sb.AppendLine(",'" + txtName.Text.Trim() + "'");
                sb.AppendLine(",'" + txtName_E_1.Text.Trim() + "'");
                sb.AppendLine(",'" + txtName_E_2.Text.Trim() + "'");
                sb.AppendLine(",'" + txtEmail.Text.Trim() + "'");
                sb.AppendLine(", dbo.ENCRYPT_AES256('" + t_Sn.Trim() + "') ");
                //sb.AppendLine(",'" + mtxtZip1.Text.Replace("-", "") + "'");
                sb.AppendLine(",'" + sAddressCode + "'");
                sb.AppendLine(",'" + txtAddress1.Text.Trim() + "'");
                sb.AppendLine(",'" + txtAddress2.Text.Trim() + "'");
                sb.AppendLine(",'" + hometel + "'");
                sb.AppendLine(",'" + hptel + "'");
                sb.AppendLine("," + LineCnt);
                sb.AppendLine("," + N_LineCnt);
                sb.AppendLine(",'" + cls_User.gid + "'");
                sb.AppendLine(", Convert(Varchar(25),GetDate(),21) ");

                sb.AppendLine(",'" + txtCenter_Code.Text.Trim() + "'");
                sb.AppendLine(",'" + txtBank_Code.Text.Trim() + "'");
                sb.AppendLine(", dbo.ENCRYPT_AES256('" + txtAccount.Text.Trim() + "') ");
                sb.AppendLine(",'" + txtName_Accnt.Text.Trim() + "'");

                sb.AppendLine(",'" + mtxtRegDate.Text.Replace("-", "").Trim() + "'");

                sb.AppendLine(",'" + Saveid + "'");
                sb.AppendLine("," + Saveid2);
                sb.AppendLine(",'" + Nominid + "'");
                sb.AppendLine("," + Nominid2);

                sb.AppendLine(", 0   ");
                sb.AppendLine("," + BankDocument);
                sb.AppendLine("," + CpnoDocument);

                sb.AppendLine(",'" + txtRemark.Text.Trim() + "'");
                sb.AppendLine(", 1 ,  1 ");
                sb.AppendLine(", '' ,  '' ");
                sb.AppendLine(", '' ,  '' ");

                sb.AppendLine(",'" + txtWebID.Text.Trim() + "'");
                sb.AppendLine(",'" + EncryptSHA256_EUCKR(txtPassword.Text.Trim()) + "'");

                sb.AppendLine(",'" + BirthDay.Trim() + "'");
                sb.AppendLine(",'" + BirthDay_M.Trim() + "'");
                sb.AppendLine(",'" + BirthDay_D.Trim() + "'");
                sb.AppendLine("," + BirthDayTF);

                sb.AppendLine(",'" + mtxtEdDate.Text.Replace("-", "").Trim() + "'");
                sb.AppendLine("," + For_Kind_TF);
                sb.AppendLine("," + Sell_Mem_TF);


                sb.AppendLine("," + double.Parse(txtB1.Text.Trim().ToString()));

                sb.AppendLine(",'" + Na_Code + "'");
                sb.AppendLine(", dbo.ENCRYPT_AES256('" + txtAccount_Reg.Text.Trim() + "') ");

                sb.AppendLine(",'" + mtxtVisaDay.Text.Replace("-", "").Trim() + "'");

                sb.AppendLine("," + RBO_Mem_TF);

                //RBO로 ㅈ입력시에서 전환일자에 등록일자를 넣고.. 비긴즈로 넣으면 전환일자에 빈칸을 넣는다.
                if (RBO_Mem_TF == 1)
                    sb.AppendLine(",''");
                else
                    sb.AppendLine(",'" + mtxtRegDate.Text.Replace("-", "").Trim() + "'");

                sb.AppendLine("," + G8_TF);
                sb.AppendLine(", '" + Sex_FLAG + "'");
                sb.Append(" , '" + AgreeSMS + "'");
                sb.Append(" , '" + AgreeEmail + "'");
                sb.AppendLine(", '" + txt_IpinCI.Text + "'");
                sb.AppendLine(", '" + txt_IpinDI.Text + "'");


                if (check_CC.Checked == true)
                {
                    #region * 동반자 ( 부부 사업자 추가 ) 
                    BirthDay = ""; BirthDay_M = ""; BirthDay_D = "";

                    if (mtxtBrithDayC.Text.Replace("-", "").Trim() != "")
                    {
                        string[] Sn_t = mtxtBrithDayC.Text.Split('-');

                        BirthDay = Sn_t[0];  //생년월일을 년월일로 해서 쪼갠다
                        BirthDay_M = Sn_t[1]; //웹쪽 관련해서 이렇게 받아들이는데가 많아서
                        BirthDay_D = Sn_t[2]; //웹쪽 기준에 맞춘거임.
                    }

                    For_Kind_TF = 0;
                    if (raButt_IN_2_C.Checked == true) For_Kind_TF = 1;// 내국인은 0 외국인은 1  사업자는 2


                    //t_Sn = mtxtSn_C.Text.Replace("-", "").Replace("_", "").Trim();

                    sb.AppendLine(",'" + txtName_C.Text.Trim() + "'");
                    sb.AppendLine("," + For_Kind_TF);
                    //sb.AppendLine( ",'" + encrypter.Encrypt(t_Sn.Trim()) + "'");
                    //sb.AppendLine( ",'" + txtName_E_1_C.Text.Trim() + "'");
                    //sb.AppendLine( ",'" + txtName_E_2_C.Text.Trim() + "'");
                    sb.AppendLine(",'" + BirthDay.Trim() + "'");
                    sb.AppendLine(",'" + BirthDay_M.Trim() + "'");
                    sb.AppendLine(",'" + BirthDay_D.Trim() + "'");
                    sb.AppendLine(" ,'" + mtxtTel2_C.Text + "'");
                    sb.AppendLine(" ,'" + txtEmail_C.Text + "'");
                    #endregion
                }
                sb.AppendLine(", " + Third_Person_Agree);
                sb.AppendLine(", " + Mbid2);
                sb.AppendLine(", '" + AgreeMarketing + "'");

                if (combo_Se_Code_2.Text != "")
                {
                    sb.AppendLine(", '" + combo_Se_Code_2.Text + "'");
                }
                else
                {
                    sb.AppendLine(", 'KR'");
                }
                if (txtBank_Code.Text == "999")
                {
                    sb.AppendLine( " ,  1 ");
                }
                else
                {
                    sb.AppendLine(" , 0   ");
                }

                if (combo_Se_Code_2.Text == "TH")
                {
                    //sb.AppendLine(", '" + cbDistrict_TH.Text + "'");
                    //sb.AppendLine(", '" + cbProvince_TH.Text + "'");
                    sb.AppendLine(", '" + cbDistrict_TH.Text + "'");
                    sb.AppendLine(", '" + cbProvince_TH.SelectedValue.ToString() + "'");
                }

                sb.AppendLine(")");

                StrSql = sb.ToString();
                Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo", Conn, tran, this.Name, this.Text);

                if (check_Auto.Checked == true) // 오토쉽 
                {
                    #region * 오토쉽 정보 등록

                    #region * [1] Autoship OrderNumber 따오기
                    string AutoSeq = string.Empty;
                    string IndexTime = "";
                    IndexTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    StrSql = "INSERT INTO tbl_Autoship_OrdNumber ";
                    StrSql = StrSql + " (Auto_Seq , Mbid , Mbid2 ";
                    StrSql = StrSql + " , ReqDate , IndexTime ";
                    StrSql = StrSql + " , User_TF)";
                    StrSql = StrSql + " Select ";
                    StrSql = StrSql + " '" + cboRegDateA.Text.Replace("-", "").Trim().Substring(2, 2) + "'";
                    StrSql = StrSql + " + Right('00000000' + convert(varchar(8),convert(Float,isnull(Max(Right(Auto_Seq,5)),0)) + 1),8) ";

                    StrSql = StrSql + ",'" + Mbid + "'," + Mbid2 + ",";
                    StrSql = StrSql + "'" + cboRegDateA.Text.Replace("-", "").Trim() + "',";
                    StrSql = StrSql + "'" + IndexTime + "',1";

                    StrSql = StrSql + " From tbl_Autoship_OrdNumber ";
                    // StrSql = StrSql + " Where LEFT(OrderNumber,8) = '" + mtxtSellDate.Text.Replace("-", "").Trim() + "'";
                    StrSql = StrSql + " Where LEFT(ReqDate, 4) = '" + cboRegDateA.Text.Replace("-", "").Trim().Substring(0, 4) + "'";

                    if (Temp_Connect.Insert_Data(StrSql, "tbl_Autoship_OrdNumber", Conn, tran, this.Name.ToString(), this.Text) == false) return;



                    //++++++++++++++++++++++++++++++++                
                    StrSql = "Select Auto_Seq  ";
                    StrSql = StrSql + " From tbl_Autoship_OrdNumber (nolock) ";
                    StrSql = StrSql + " Where Mbid = '" + Mbid + "'";
                    StrSql = StrSql + " And Mbid2 = " + Mbid2;
                    StrSql = StrSql + " And ReqDate = '" + cboRegDateA.Text.Replace("-", "").Trim() + "'";
                    StrSql = StrSql + " And IndexTime = '" + IndexTime + "'";



                    DataSet ds = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Autoship_OrdNumber", ds) == false) return;
                    int ReCnt = Temp_Connect.DataSet_ReCount;

                    if (ReCnt == 0) return;

                    AutoSeq = ds.Tables["tbl_Autoship_OrdNumber"].Rows[0]["Auto_Seq"].ToString();
                    //++++++++++++++++++++++++++++++++
                    #endregion

                    #region * [2] Insert - Autoship Master Data

                    double TotalPrice = 0, TotalPV = 0, TotalCV = 0, Delivery = 0;

                    if (txt_Auto_PR2.Text == "")
                        TotalPrice = 0;
                    else
                        TotalPrice = double.Parse(txt_Auto_PR2.Text.Trim());
                    if (txt_Auto_PR.Text == "")
                        TotalPV = 0;
                    else
                        TotalPV = double.Parse(txt_Auto_PR.Text.Trim());


                    //if (txt_TotalCV.Text == "")
                    //    TotalCV = 0;
                    //else
                    //    TotalCV = double.Parse(txt_TotalCV.Text.Trim());

                    string ReqType = "";
                    ReqType = "BC";


                    //if (txtDeliverPrice.Text.Replace(",", "").Trim() == "")
                    Delivery = 0;
                    //else
                    //    Delivery = double.Parse(txtDeliverPrice.Text.Replace(",", "").Trim());

                    string AutoExtend = "Y"; //"N"
                    //if (chk_AutoExtend.Checked == true)
                    //    AutoExtend = "Y";

                    int ReqMonth = 0;
                    //if (rb_ReqMonth_3.Checked == true)
                    //    ReqMonth = 3;

                    StrSql = " INSERT INTO tbl_Memberinfo_AutoShip ( Auto_Seq, ";
                    StrSql = StrSql + " mbid, mbid2, Req_Type, Req_State, Req_Date ";
                    StrSql = StrSql + " , Start_Date, Proc_Date, TotalPrice, TotalPV, TotalCV ";
                    StrSql = StrSql + " , Etc, Proc_Cnt, DeliveryCharge, RecordID, RecordTime ";
                    //StrSql = StrSql + " , Req_Month, AutoExtend";//, CustomerGroupKey ";
                    StrSql = StrSql + " ) VALUES ( ";
                    StrSql = StrSql + " '" + AutoSeq + "' ";
                    StrSql = StrSql + " , '" + Mbid + "' ";
                    StrSql = StrSql + " , " + Mbid2;
                    StrSql = StrSql + " , '" + ReqType + "' "; //ADS신청종류
                    StrSql = StrSql + " , '10' ";   //ADS신청상태
                    StrSql = StrSql + " , '" + mtxtRegDate.Text.Replace("-", "").Trim() + "' "; //신청날짜
                    StrSql = StrSql + " , '" + cboRegDateA.Text.Replace("-", "").Trim() + "' "; //첫 실행날짜
                    StrSql = StrSql + " , '" + cboRegDateA.Text.Replace("-", "").Trim() + "' "; //다음 실행날짜
                    StrSql = StrSql + " , " + TotalPrice;
                    StrSql = StrSql + " , " + TotalPV;
                    StrSql = StrSql + " , " + TotalCV;
                    StrSql = StrSql + " , '' "; //비고
                    StrSql = StrSql + " , 0 ";
                    StrSql = StrSql + " , " + Delivery;  //배송비
                    StrSql = StrSql + " , '" + cls_User.gid + "' ";
                    StrSql = StrSql + " , Convert(Varchar(25),GetDate(),21) ";
                    //StrSql = StrSql + " , " + ReqMonth;
                    //StrSql = StrSql + " , '" + AutoExtend + "' ";
                    //StrSql = StrSql + " , '" + idx_CustomerGroupKey + "' ";
                    StrSql = StrSql + " ) ";

                    if (Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_AutoShip", Conn, tran, this.Name.ToString(), this.Text) == false) return;

                    #endregion

                    #region * [3] Insert - Autoship Item Data

                    int ItemIndex = 1;
                    foreach (DataGridViewRow row in dGridView_Good.Rows)
                    {
                        //"ItemCount"  , "ItemName"   , "ItemCode"  , "ItemPV"   , "ItemPrice"     
                        int ItemCount = int.Parse(row.Cells["ItemCount"].Value.ToString());
                        string ItemCode = row.Cells["ItemCode"].Value.ToString();
                        string ItemName = row.Cells["ItemName"].Value.ToString();
                        double ItemPrice = double.Parse(row.Cells["ItemPrice"].Value.ToString());
                        double ItemPV = double.Parse(row.Cells["ItemPV"].Value.ToString());

                        if (ItemCount > 0)
                        {
                            StrSql = " INSERT INTO tbl_Memberinfo_AutoShip_Item ";
                            StrSql = StrSql + " ( Auto_Seq, ItemIndex, ItemCode, ItemName, ItemCount ";
                            //StrSql = StrSql + " , ItemPrice, ItemPV, ItemCV, ItemTotalPrice, ItemTotalPV ";
                            StrSql = StrSql + " , ItemPrice, ItemPV, ItemTotalPrice, ItemTotalPV ";
                            //StrSql = StrSql + " , ItemTotalCV, RecordId, RecordTime ) ";
                            StrSql = StrSql + " , RecordId, RecordTime ) ";
                            StrSql = StrSql + " VALUES ( ";

                            StrSql = StrSql + " '" + AutoSeq + "'";
                            StrSql = StrSql + " , " + ItemIndex++;
                            StrSql = StrSql + " , '" + ItemCode + "' ";
                            StrSql = StrSql + " , '" + ItemName + "' ";
                            StrSql = StrSql + " , " + ItemCount;
                            StrSql = StrSql + " , " + ItemPrice;
                            StrSql = StrSql + " , " + ItemPV;
                            //StrSql = StrSql + " , "  + AutoShip_Item[ItemIndex].ItemCV;
                            StrSql = StrSql + " , " + ItemPrice * ItemCount;
                            //StrSql = StrSql + " , "  + AutoShip_Item[ItemIndex].ItemTotalPV;
                            StrSql = StrSql + " , " + ItemPV * ItemCount;
                            StrSql = StrSql + " , '" + cls_User.gid + "' ";
                            StrSql = StrSql + " , Convert(Varchar(25),GetDate(),21) ";
                            StrSql = StrSql + " ) ";

                            if (Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_AutoShip_Item", Conn, tran, this.Name.ToString(), this.Text) == false) return;
                        }
                    }


                    #endregion

                    #region * [4] Insert - Autoship Cacu Data

                    StrSql = " INSERT INTO tbl_Memberinfo_AutoShip_Cacu ";
                    StrSql = StrSql + " ( Auto_Seq, CacuIndex, Cacu_Type, CardCode, CardName, CardNumber ";
                    StrSql = StrSql + " , Period1, Period2, Card_OwnerName, Payment_Amt, Installment_Period";
                    StrSql = StrSql + " , C_P_Number, C_B_Number,  RecordId, RecordTime) ";
                    StrSql = StrSql + " VALUES ( ";

                    StrSql = StrSql + "'" + AutoSeq + "'";
                    StrSql = StrSql + " , 1";
                    StrSql = StrSql + " , 3";
                    StrSql = StrSql + " , '" + txt_C_Card_Code.Text + "' ";
                    StrSql = StrSql + " , '" + txt_C_Card.Text + "' ";
                    StrSql = StrSql + " , '" + encrypter.Encrypt(txt_C_Card_Number.Text) + "' ";

                    StrSql = StrSql + " , '" + combo_C_Card_Year.Text + "' ";
                    StrSql = StrSql + " , '" + txt_C_P_Number.Text + "' ";
                    StrSql = StrSql + " , '" + txt_C_Name_3.Text + "' ";
                    StrSql = StrSql + " , " + double.Parse(txt_Auto_PR2.Text);
                    StrSql = StrSql + " , '일시불' ";

                    StrSql = StrSql + " , '" + encrypter.Encrypt(txt_C_P_Number.Text) + "' ";
                    StrSql = StrSql + " , '" + txt_C_B_Number.Text + "' ";
                    StrSql = StrSql + " , '" + cls_User.gid + "' ";
                    StrSql = StrSql + " , Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " ) ";

                    if (Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_AutoShip_Cacu", Conn, tran, this.Name.ToString(), this.Text) == false) return;

                    #endregion

                    #region * [5] Insert - Autoship Rece Data


                    StrSql = " INSERT INTO tbl_Memberinfo_AutoShip_Rece ";
                    StrSql = StrSql + " ( Auto_Seq, RecIndex, Rec_Name, Rec_Tel, Rec_Addcode ";
                    StrSql = StrSql + " , Rec_Address1, Rec_Address2, RecordId, RecordTime ) ";
                    StrSql = StrSql + " VALUES ( ";
                    StrSql = StrSql + "'" + AutoSeq + "'";
                    //StrSql = StrSql + " , " + AutoShip_Rece[RecIndex].RecIndex;
                    StrSql = StrSql + " , 1";
                    StrSql = StrSql + " , '" + txtName_Auto.Text + "'";// AutoShip_Rece[RecIndex].Rec_Name + "' ";
                    StrSql = StrSql + " , '" + mtxtTel_Auto.Text + "'";// AutoShip_Rece[RecIndex].Rec_Tel + "' ";
                    StrSql = StrSql + " , '" + mtxtZip_Auto.Text + "'";// AutoShip_Rece[RecIndex].Rec_AddCode + "' ";
                    StrSql = StrSql + " , '" + txtAddress_Auto.Text + "'";// AutoShip_Rece[RecIndex].Rec_Address1 + "' ";
                    StrSql = StrSql + " , '" + txtAddress2_Auto.Text + "'"; // AutoShip_Rece[RecIndex].Rec_Address2 + "' ";
                    StrSql = StrSql + " , '" + cls_User.gid + "' ";
                    StrSql = StrSql + " , Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " ) ";

                    if (Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo_AutoShip_Rece", Conn, tran, this.Name.ToString(), this.Text) == false) return;

                    #endregion

                    #endregion
                }




                tran.Commit();
                Save_Error_Check = 1;


                cls_form_Meth cm = new cls_form_Meth();
                MessageBox.Show(cm._chang_base_caption_search("회원_번호") + ":" + mtxtMbid.Text.Trim()
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));



                cls_Connect_DB Temp_Connect2 = new cls_Connect_DB();
                Temp_Connect2.Connect_DB();


                //20200810 회원가입 완료시 땡처리 주문넣기
                //주문번호생성
                //StrSql = "select left(replace(Convert(Varchar(25), GetDate(), 21),'-',''),8) , mbid2 from tbl_memberinfo (nolock) where mbid2 = '" + Mbid2 + "'  ";


                //DataSet ds1 = new DataSet();
                ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                //Temp_Connect2.Open_Data_Set(StrSql, "tbl_memberinfo", ds1);
                //int ReCnt2 = Temp_Connect2.DataSet_ReCount;
                //today = ds1.Tables["tbl_memberinfo"].Rows[0][0].ToString();


                //StrSql = "EXEC Usp_Insert_Tbl_Sales_OrderNumber_CS '', '" + Mbid2 + "', '" + today + "', '01'";
                //DataSet ds2 = new DataSet();


                //Temp_Connect2.Open_Data_Set(StrSql, "tbl_Sales_OrdNumber", ds2);
                //int ReCnt3 = Temp_Connect2.DataSet_ReCount;


                //if (ReCnt3 > 0)
                //{
                //    string OrderNumber = ds2.Tables["tbl_Sales_OrdNumber"].Rows[0]["OrderNumber"].ToString();

                if (combo_Se_Code_2.Text == "TH")   // 태국인 경우
                {
                    StrSql = " EXEC Usp_Insert_Firstmember_SalesTable_MU '" + Mbid + "', " + Mbid2 + "";
                }
                else    // 태국 이외 국가시
                {
                    StrSql = " EXEC Usp_Insert_Firstmember_SalesTable '" + Mbid + "', " + Mbid2 + "";
                }

                Temp_Connect.Insert_Data(StrSql, "tbl_SalesitemDetail", this.Name.ToString(), this.Text);
                //}


                string StrSql_JDE_PROCEDUER = "";
                if (combo_Se_Code_2.Text == "TH")   // 태국인 경우
                {
                    StrSql_JDE_PROCEDUER = " EXEC  Usp_JDE_Update_MK_Customer_TA '" + Mbid2 + "','A' ";
                }
                else    // 태국 이외 국가시
                {
                    StrSql_JDE_PROCEDUER = "EXEC  Usp_JDE_Update_MK_Customer '" + Mbid2 + "','A'";
                }
                    
                Temp_Connect.Insert_Data(StrSql_JDE_PROCEDUER, "tbl_Memberinfo", Conn, tran);


            }
            catch (Exception)
            {
                if (Save_Error_Check == 0)
                    tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }
        }




        private void chk_Top_MouseUp(object sender, MouseEventArgs e)
        {
            CheckBox ckb = (CheckBox)sender;

            if (ckb.Checked == true)
            {
                if (ckb.Name == "chk_Top_n")
                {
                    mtxtMbid_n.Text = ""; txtName_n.Text = ""; txtSN_n.Text = "";
                }

                if (ckb.Name == "chk_Top_s")
                {
                    mtxtMbid_s.Text = ""; txtName_s.Text = ""; txtSN_s.Text = "";
                }

                if (ckb.Name == "chk_Foreign_n")
                {
                    mtxtMbid_n.Text = ""; txtName_n.Text = ""; txtSN_n.Text = "";
                }

                if (ckb.Name == "chk_Foreign_s")
                {
                    mtxtMbid_s.Text = ""; txtName_s.Text = ""; txtSN_s.Text = "";
                }
            }
        }




        private void check_Cpno_MouseClick(object sender, MouseEventArgs e)
        {

            CheckBox ckb = (CheckBox)sender;
            if (ckb.Checked == true)
            {
                if (ckb.Name == "check_Cpno_Err")
                    cls_app_static_var.Member_Cpno_Error_Check_TF = 1;

                if (ckb.Name == "check_Cpno")
                    cls_app_static_var.Member_Cpno_Put_TF = 1;

                if (ckb.Name == "check_Cpno_Multi")
                    cls_app_static_var.Member_Reg_Multi_TF = 1;
            }
            else
            {
                if (ckb.Name == "check_Cpno_Err")
                    cls_app_static_var.Member_Cpno_Error_Check_TF = 0;

                if (ckb.Name == "check_Cpno")
                    cls_app_static_var.Member_Cpno_Put_TF = 0;

                if (ckb.Name == "check_Cpno_Multi")
                    cls_app_static_var.Member_Reg_Multi_TF = 0;

            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string StrSql = "UpDate  tbl_Config Set ";
            StrSql = StrSql + "  Resident_Number_Check = " + cls_app_static_var.Member_Cpno_Error_Check_TF;
            StrSql = StrSql + ", Resident_Number_Check2 = " + cls_app_static_var.Member_Cpno_Put_TF;
            StrSql = StrSql + ", Many_Account_Check = " + cls_app_static_var.Member_Reg_Multi_TF;

            Temp_Connect.Update_Data(StrSql, this.Name, this.Text);

            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
        }



        private void opt_MCode_R_MouseUp(object sender, MouseEventArgs e)
        {
            RadioButton ckb = (RadioButton)sender;
            if (ckb.Checked == true)
            {
                //cls_app_static_var.Mem_Number_Auto_Flag = ds.Tables["tbl_Config"].Rows[0]["Mem_Number_Auto_Flag"].ToString();    // A면 자동으로 증가    H면 손수 입력함.    R 이면 랜덤.

                if (ckb.Name == "opt_MCode_R")
                    cls_app_static_var.Mem_Number_Auto_Flag = "R";

                if (ckb.Name == "opt_MCode_A")
                    cls_app_static_var.Mem_Number_Auto_Flag = "A";


                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                string StrSql = "UpDate  tbl_Config Set ";
                StrSql = StrSql + "  Mem_Number_Auto_Flag = '" + cls_app_static_var.Mem_Number_Auto_Flag.ToString() + "'";
                
                Temp_Connect.Update_Data(StrSql, this.Name, this.Text);

                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
            }
        }

        private void butt_Excel_Click(object sender, EventArgs e)
        {
            return;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            string Tsql = "Select Mbid,Mbid2   ";
            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            Tsql = Tsql + " Where Mbid ='KR' ";
            Tsql = Tsql + " Order by Mbid2 ";


            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            for (int fcnt = 0; fcnt < ReCnt; fcnt++)
            {
                Tsql = "Select Mbid,Mbid2   ";
                Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
                Tsql = Tsql + " Where Nominid2 =" + ds.Tables["t_P_table"].Rows[fcnt]["Mbid2"].ToString();
                Tsql = Tsql + " And   Nominid ='" + ds.Tables["t_P_table"].Rows[fcnt]["Mbid"].ToString() + "'";
                Tsql = Tsql + " order by Mbid2 ASC ";

                DataSet ds2 = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds2) == false) return;
                int ReCnt2 = Temp_Connect.DataSet_ReCount;

                for (int fcnt2 = 0; fcnt2 < ReCnt2; fcnt2++)
                {
                    string StrSql = "Update tbl_Memberinfo Set ";
                    StrSql = StrSql + " N_LineCnt= " + (fcnt2 + 1);
                    StrSql = StrSql + " Where mbid2 = " + ds2.Tables["t_P_table"].Rows[fcnt2]["Mbid2"].ToString();
                    StrSql = StrSql + " And   Mbid ='" + ds2.Tables["t_P_table"].Rows[fcnt2]["Mbid"].ToString() + "'";

                    Temp_Connect.Update_Data(StrSql);

                }
            }

            //if (ReCnt == 1)
            //{
            //tb.Text = ds.Tables["t_P_table"].Rows[0][1].ToString();
            //tb1_Code.Text = ds.Tables["t_P_table"].Rows[0][0].ToString();
            //}
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab_Sub.SelectedTab.Name == "tab_Day")  //
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Base_Grid_Set(); //당일등록 회원을 불러온다.
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

        }

        private void check_Auto_MouseClick(object sender, MouseEventArgs e)
        {
            mtxtZip_Auto.Focus();
        }





        private void Base_Grid_Set_Good()
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Good_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            ((DataGridViewTextBoxColumn)dGridView_Good.Columns[0]).MaxInputLength = 6;
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            string Tsql = "";

            Tsql = "Select 0, Name , NCode ,price4 ,price2    ";
            Tsql = Tsql + " , '', '' ,'' ,'' ,'' ";
            Tsql = Tsql + " From ufn_Good_Search_01 ('" + cls_User.gid_date_time + "') ";
            Tsql = Tsql + " Where AutoShipYN = 'Y' ";
            Tsql = Tsql + " Order by Ncode ";

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
                Set_gr_dic_Good(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }


        private void Set_gr_dic_Good(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
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



        private void dGridView_Good_Base_Header_Reset()
        {
            cgb.grid_col_Count = 10;
            cgb.basegrid = dGridView_Good;
            cgb.grid_select_mod = DataGridViewSelectionMode.CellSelect;
            cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"오토쉽수량"  , "상품명"   , "상품코드"  , "PV"   , "회원가"
                                , ""   , ""    , ""   , ""    , ""
                                    };
            string[] g_Cols = {"ItemCount"  , "ItemName"   , "ItemCode"  , "ItemPV"   , "ItemPrice"
                                , "T1"   , "T2"    , "T3"   , "T4"    , "T5"
                                    };

            cgb.grid_col_header_text = g_HeaderText;
            cgb.grid_col_name = g_Cols;

            int[] g_Width = { 60, 130, 100, 70, 70
                             ,0 , 0 ,  0 , 0 ,  0
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { false , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10                                                           
                              };
            cgb.grid_col_alignment = g_Alignment;


            DataGridViewColumnSortMode[] g_SortM =
                              {DataGridViewColumnSortMode.NotSortable
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic  //5
                               
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic //10                                                           
                              };
            cgb.grid_col_SortMode = g_SortM;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb.grid_cell_format = gr_dic_cell_format;

            cgb.basegrid.RowHeadersVisible = false;
        }

        private void dGridView_Good_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dGridView_Good.EditingControl.KeyPress += new KeyPressEventHandler(textBoxPart_TextChanged);


        }

        private void textBoxPart_TextChanged(object sender, KeyPressEventArgs e)
        {

        }

        private void dGridView_Good_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int Item_Pr = 0;
            int Item_Pr2 = 0;
            int T_Cnt = 0;
            for (int i = 0; i < dGridView_Good.Rows.Count; i++)
            {
                if (int.Parse(dGridView_Good.Rows[i].Cells[0].Value.ToString()) > 0)
                {
                    T_Cnt = int.Parse(dGridView_Good.Rows[i].Cells[0].Value.ToString());
                    Item_Pr = Item_Pr + (int.Parse(dGridView_Good.Rows[i].Cells[3].Value.ToString()) * T_Cnt);
                    Item_Pr2 = Item_Pr2 + (int.Parse(dGridView_Good.Rows[i].Cells[4].Value.ToString()) * T_Cnt);

                }
            }

            txt_Auto_PR.Text = string.Format(cls_app_static_var.str_Currency_Type, Item_Pr);
            txt_Auto_PR2.Text = string.Format(cls_app_static_var.str_Currency_Type, Item_Pr2);
        }

        private void button_Acc_Reg_Click(object sender, EventArgs e)
        {
            Reg_Bank_Account();
        }

        private void Reg_Bank_Account()
        {
            txtAccount_Reg.Text = "";

            lbl_ACC.Text = "미인증";

            string Sn = mtxtSn.Text.Replace("-", "").Replace("_", "").Trim();

            cls_Sn_Check csn_C = new cls_Sn_Check();
            string sort_TF = "";
            bool check_b = false;
            if (raButt_IN_1.Checked == true) //내국인인 구분자
                sort_TF = "in";

            if (raButt_IN_2.Checked == true) //외국인 구분자
                sort_TF = "fo";

            if (raButt_IN_3.Checked == true) //사업자 구분자.
                sort_TF = "biz";

            check_b = csn_C.Sn_Number_Check(Sn, sort_TF);

            Data_Set_Form_TF = 0;

            //if (check_b == false)
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_SN_Number_Error")
            //           + "\n" +
            //           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    mtxtSn.Focus(); return;
            //}


            string me = "";

            if (txtAccount.Text == "")
            {
                me = "계좌번호를 필히 입력해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                txtAccount.Focus();
                return;
            }

            if (txtName_Accnt.Text == "")
            {
                me = "예금주를 필히 입력해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                txtName_Accnt.Focus();
                return;
            }


            if (txtBank_Code.Text == "")
            {
                me = "은행을 필히 선택해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                txtBank.Focus();
                return;
            }

            if (mtxtBrithDay.Text.Replace("-", "").Trim() == "")
            {
                me = "생년월일을 필히 선택해 주십시요." + "\n" +
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                MessageBox.Show(me);
                mtxtBrithDay.Focus();
                return;
            }


            cls_Sn_Check csc = new cls_Sn_Check();

            string successYN = "";

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                //successYN = csc.Bank_Acount_Check(txtName_Accnt.Text, mtxtSn.Text.Substring(0, 6), txtBank_Code.Text, txtAccount.Text);
                successYN = csc.Bank_Acount_Check(txtName_Accnt.Text, mtxtBrithDay.Text.Replace("-", "").Substring(2, 6), txtBank_Code.Text, txtAccount.Text);
            }
            catch (Exception)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Account authentication error.");
                }
                else
                {
                    MessageBox.Show("계좌인증 오류");
                }
                //MessageBox.Show(ee.ToString ());
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;

            if (successYN == "Y")
            {
                txtAccount_Reg.Text = txtAccount.Text;
                lbl_ACC.Text = "Success";
                if (cls_User.gid_CountryCode == "TH")
                {
                    me = "This is the correct account information. Account authentication success.";
                }
                else
                {
                    me = "올바른 계좌 정보 입니다. 계좌인증 성공.";
                }
                MessageBox.Show(me);
                txtName_E_1.Focus();
            }
            else
            {
                txtAccount_Reg.Text = "";
                lbl_ACC.Text = "Fail";
                if (cls_User.gid_CountryCode == "TH")
                {
                    me = "Invalid account information. Please check and try again. Account verification failed.";
                }
                else
                {
                    me = "올바른 계좌 정보가 아닙니다. 확인후 다시 시도해 주십시요. 계좌인증 실패.";
                }
                MessageBox.Show(me);
                txtAccount.Focus();
            }



        }

        private void butt_Certify_Click(object sender, EventArgs e)
        {
            frmBase_Certify e_f = new frmBase_Certify();
            e_f.Send_Certify_Info += new frmBase_Certify.SendCertifyDele(e_f_Send_Certify_Info);
            e_f.Call_Certify_Info += new frmBase_Certify.Call_Certify_Info_Dele(e_f_Call_Certify_Info);
            e_f.ShowDialog();
        }


        private string EncryptSHA256_EUCKR(string phrase)
        {

            if (string.IsNullOrEmpty(phrase) == true)
            {
                return "";
            }
            else
            {
                Encoding encoding = Encoding.Unicode;

                SHA256 sha = new SHA256Managed();
                byte[] data = sha.ComputeHash(encoding.GetBytes(phrase));

                StringBuilder sb = new StringBuilder();
                foreach (byte b in data)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private void rdoLineLeft_CheckedChanged(object sender, EventArgs e)
        {
            txtLineCnt.Text = "1";
        }

        private void rdoLineRight_CheckedChanged(object sender, EventArgs e)
        {
            txtLineCnt.Text = "2";
        }

        private void cboRegDateA_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void InitComboZipCode_TH()
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("SELECT ZIPCODE_NM FROM dbo.ufn_Get_ZipCode_State_TH() ORDER BY ZIPCODE_SORT ");
            sb.AppendLine("SELECT * FROM ufn_Get_ZipCode_Province_TH() ORDER BY MinSubDistrictID ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            cbProvince_TH.DataBindings.Clear();
            cbProvince_TH.DataSource = ds.Tables["ZipCode_NM"];
            cbProvince_TH.DisplayMember = "ZipCode_NM";
            cbProvince_TH.ValueMember = "ProvinceCode";
        }

        private void InitCombo()
        {

            string[] data_Y = { ""
                              , int.Parse (cls_User.gid_date_time.Substring (0,4)).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 1 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 2 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 3 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 4 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 5 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 6 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 7 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 8 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 9 ).ToString()
                              , (int.Parse (cls_User.gid_date_time.Substring (0,4)) + 10 ).ToString()
                              };

            string[] data_M = { "","01", "02", "03", "04", "05"
                               , "06", "07", "08", "09", "10"
                               , "11", "12"
                              };

            //2018-10-19 지성경 추가 에버스프링 오토십은 할부개념이업다 ㄷㄷ.
            string[] data_P = { //"", 
                                  "일시불"
                               ////,"1" 1개월의 할부는없어서 주석처리함 
                               //, "2", "3", "4", "5" 
                               //, "6", "7", "8", "9", "10" 
                               //, "11", "12" 
                              };

            // 각 콤보박스에 데이타를 초기화
            combo_C_Card_Year.Items.AddRange(data_Y);
            combo_C_Card_Month.Items.AddRange(data_M);
            combo_C_Card_Per.Items.AddRange(data_P);


            combo_C_Card_Year.SelectedIndex = 0;
            combo_C_Card_Month.SelectedIndex = 0;
            combo_C_Card_Per.SelectedIndex = 0;

            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT  TOP 10 CONVERT(VARCHAR, DATEADD(dd, 1, STARTDATE), 23) AS [자동결제날짜]");
            sb.AppendLine("FROM tbl_WeekCount (nolock) ");
            sb.AppendLine("WHERE STARTDATE BETWEEN  CONVERT(VARCHAR, GETDATE(), 112)  AND DATEADD(YY, +1, STARTDATE) ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "Autoship_paymentday", ds) == false) return; ;


            cboRegDateA.DataSource = ds.Tables["Autoship_paymentday"];
            cboRegDateA.DisplayMember = "자동결제날짜";
        }

        private void tab_Sub_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage tab = tab_Sub.TabPages[e.Index];
            Rectangle header = tab_Sub.GetTabRect(e.Index);
            using (SolidBrush darkBrush = new SolidBrush(Color.FromArgb(194, 214, 213)))
            using (SolidBrush lightBrush = new SolidBrush(Color.FromArgb(39, 126, 133)))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                if (e.State == DrawItemState.Selected)
                {
                    Font font = new Font(tab_Sub.Font.Name, 10, FontStyle.Bold);
                    e.Graphics.FillRectangle(lightBrush, e.Bounds);
                    e.Graphics.DrawString(tab.Text, font, darkBrush, header, sf);
                }
                else
                {
                    e.Graphics.FillRectangle(darkBrush, e.Bounds);
                    e.Graphics.DrawString(tab.Text, e.Font, lightBrush, header, sf);
                }
            }
        }

        private void combo_Se_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            combo_Se_Code_2.SelectedIndex = combo_Se_2.SelectedIndex;
            combo_Se.SelectedIndex = combo_Se_2.SelectedIndex;

            // 태국버전 인 경우
            if (combo_Se_Code_2.Text == "TH")
            {
                pnlDistrict_TH.Visible = true;
                pnlProvince_TH.Visible = true;
                pnlSubDistrict_TH.Visible = true;
                pnlZipCode_TH.Visible = true;
                pnlZipCode_KR.Visible = false;
                txtAddress2.ReadOnly = true;
                cbSubDistrict_TH_SelectedIndexChanged(this, null);
            }
            // 태국 이외 버전 인 경우
            else
            {
                pnlDistrict_TH.Visible = false;
                pnlProvince_TH.Visible = false;
                pnlSubDistrict_TH.Visible = false;
                pnlZipCode_TH.Visible = false;
                pnlZipCode_KR.Visible = true;
                txtAddress2.ReadOnly = false;
                txtAddress2.Clear();
            }
        }

        private void combo_Se_Code_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            combo_Se_2.SelectedIndex = combo_Se_Code_2.SelectedIndex;
            combo_Se_Code.SelectedIndex = combo_Se_Code_2.SelectedIndex;

            // 태국버전 인 경우
            if (combo_Se_Code_2.Text == "TH")
            {
                pnlDistrict_TH.Visible = true;
                pnlProvince_TH.Visible = true;
                pnlSubDistrict_TH.Visible = true;
                pnlZipCode_TH.Visible = true;
                pnlZipCode_KR.Visible = false;
                txtAddress2.ReadOnly = true;
                cbSubDistrict_TH_SelectedIndexChanged(this, null);
            }
            // 태국 이외 버전 인 경우
            else
            {
                pnlDistrict_TH.Visible = false;
                pnlProvince_TH.Visible = false;
                pnlSubDistrict_TH.Visible = false;
                pnlZipCode_TH.Visible = false;
                pnlZipCode_KR.Visible = true;
                txtAddress2.ReadOnly = false;
                txtAddress2.Clear();
            }
        }

        private void cbProvince_TH_SelectedIndexChanged(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("SELECT ZIPCODE_NM FROM dbo.ufn_Get_ZipCode_City_TH('" + cbProvince_TH.Text + "') ORDER BY ZIPCODE_SORT ");
            sb.AppendLine("SELECT ZIPCODE_NM FROM ufn_Get_ZipCode_District_TH('" + cbProvince_TH.Text + "') ORDER BY MinSubDistrictID ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            cbDistrict_TH.DataBindings.Clear();
            cbDistrict_TH.DataSource = ds.Tables["ZipCode_NM"];
            cbDistrict_TH.DisplayMember = "ZipCode_NM";

        }

        private void cbDistrict_TH_SelectedIndexChanged(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("SELECT * FROM dbo.ufn_Get_ZipCode_TH('" + cbDistrict_TH.Text + "') ");
            sb.AppendLine("SELECT ZIPCODE_NM FROM dbo.ufn_Get_ZipCode_SubDistrict_TH('" + cbDistrict_TH.Text + "') ORDER BY MinSubDistrictID ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            cbSubDistrict_TH.DataBindings.Clear();
            cbSubDistrict_TH.DataSource = ds.Tables["ZipCode_NM"];
            cbSubDistrict_TH.DisplayMember = "ZipCode_NM";
        }

        private void cbSubDistrict_TH_SelectedIndexChanged(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT [ZIPCODE_NM] = PostCode FROM TLS_ZIPCODE_CS WITH(NOLOCK) WHERE SubDistrictThaiShort = '" + cbSubDistrict_TH.Text + "' ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            if (Temp_conn.DataSet_ReCount <= 0) return;

            txtZipCode_TH.Text = "";
            txtZipCode_TH.Text = ds.Tables["ZipCode_NM"].Rows[0][0].ToString();

            //txtAddress2.Text = cbProvince_TH.Text + " " + cbDistrict_TH.Text + " " + cbProvince_TH.SelectedValue.ToString();
            txtAddress2.Text = cbSubDistrict_TH.Text + " " + cbDistrict_TH.Text + " " + cbProvince_TH.Text;


            //cbDistrict_TH.DataBindings.Clear();
            //cbDistrict_TH.DataSource = ds.Tables["ZipCode_NM"];
            //cbDistrict_TH.DisplayMember = "ZipCode_NM";
        }
    }

}
