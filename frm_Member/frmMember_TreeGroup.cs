using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace MLM_Program
{
    public partial class frmMember_TreeGroup : clsForm_Extends
    {
        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);



        public delegate void Take_NumberDele(ref string Send_Number, ref string Send_Name);
        public event Take_NumberDele Take_Mem_Number;

        private Dictionary<string, cls_Mem_Tree> TreeDic = new Dictionary<string,cls_Mem_Tree>() ;
        private Dictionary<string, cls_Mem_Tree> Base_TreeDic = new Dictionary<string, cls_Mem_Tree>();  //+,- 눌럿을 경우 조직도가 펴졋다 줄어 들엇다 하는 부분때문에 복사를 하나 해놓은 원본을
        private Dictionary<int, Label> YLabel = new Dictionary<int, Label>();
        private Dictionary<int, PictureBox> YPicture = new Dictionary<int, PictureBox>();
        private Dictionary<int, string> TreeDic_Cnt = new Dictionary<int, string>();
        private Dictionary<int, string> Base_TreeDic_Cnt = new Dictionary<int, string>();

        private Dictionary<int, PictureBox> Y_Plus = new Dictionary<int, PictureBox>();
        private Dictionary<int, PictureBox> Y_Minus = new Dictionary<int, PictureBox>();

        private Dictionary<int, cls_Tree_Line> LineDic = new Dictionary<int, cls_Tree_Line>();

        private int Print_Cut_int = 0; 

        cls_Grid_Base cg_Up_S = new cls_Grid_Base();

        private int hScroll_Be_Value = 0;
        private int vScroll_Be_Value = 0; 

        private string Mouse_Select_key = "";

        
        private int IntervalHeight = 10 ;
        private int IntervalWidth = 5;
        private int LastLvl = 0;

        private int Print_W_Cur_PagCnt = 0;
        private int Print_H_Cur_PagCnt = 0;
        
        private int W_Print_PagCnt = 0;
        private int H_Print_PagCnt = 0;


        private int PB_Print_W_Cur_PagCnt = 0;
        private int PB_Print_H_Cur_PagCnt = 0;

        private int PB_W_Print_PagCnt = 0;
        private int PB_H_Print_PagCnt = 0;

        private int Data_Set_Form_TF = 0; private int Down_Max_Level = 0;




        public frmMember_TreeGroup()
        {
            InitializeComponent();
            BaseDoc.DefaultPageSettings.Landscape = true;
            prPrview.Document.DefaultPageSettings.Landscape = true;
            pageSetup.Document.DefaultPageSettings.Landscape = true;
            
        }



        private void frm_Base_Activated(object sender, EventArgs e)
        {
            this.Refresh();

            string Send_Number = ""; string Send_Name = "";
            Take_Mem_Number(ref Send_Number, ref Send_Name);

            if (Send_Number != "")
            {
                mtxtMbid.Text = Send_Number;
                txtName.Text = Send_Name;
                EventArgs ee = null;
                button1_Click(butt_Select, ee);
            }

            
        }

        private void frmMember_TreeGroup_Load(object sender, EventArgs e)
        {
            //for (int fi_cnt = 0; fi_cnt <= 300; fi_cnt++)
            //{
            //    Label lb_X = new Label();
            //    lb_X.Visible = false;
            //    lb_X.AutoSize = false;
            //    YLabel[fi_cnt] = lb_X   ;
            //    lb_X.Click += new EventHandler(Treelbl_ClickHandler);
            //    panel2.Controls.Add(lb_X);
            //}


            //this.MouseWheel += new MouseEventHandler(frmMember_TreeGroup_MouseWheel);

            //panel2.Dock = DockStyle.Fill;
           
            tabC_1.TabPages.Remove(tabPage4);
            tabC_1.TabPages.Remove(tabPage6);

            splitContainer1.Dock = DockStyle.Fill;

            panel2.MouseWheel += new MouseEventHandler(panel2_MouseWheel);


            mtxtSellDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate3.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate3.Text = DateTime.Now.ToString("yyyy-MM-dd");

            mtxtSellDate4.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate5.Mask = cls_app_static_var.Date_Number_Fromat;


            dGridView_Up_S_Header_Reset(dGridView_Up_S); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset();

            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function() ;
            cpbf.Put_SellCode_ComboBox (combo_Se, combo_Se_Code) ;

            Mouse_Select_key = "";
            vScroll_Be_Value = 0;

            txtDownCnt_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txtDownCnt.BackColor = cls_app_static_var.txt_Enable_Color;
            pb_De.Visible = false;
            
             
            for (int fi_cnt = 0; fi_cnt <= 400; fi_cnt++)
            {
                PictureBox pic_X = new PictureBox();
                pic_X.Visible = false;
                
                pic_X.BorderStyle = BorderStyle.FixedSingle;

                pic_X.ContextMenu = lblY.ContextMenu;
                //pic_X.Click += new EventHandler(pic_X_Click);
                pic_X.MouseClick += new MouseEventHandler(pic_X_MouseClick);
                pic_X.DoubleClick += new EventHandler(pic_X_DoubleClick);
                YPicture[fi_cnt] = pic_X;                
                panel2.Controls.Add(pic_X);
            }


            for (int fi_cnt = 0; fi_cnt <= 400; fi_cnt++)
            {
                PictureBox pB_P_T = new PictureBox();
                PictureBox pB_M_T = new PictureBox();
                pB_P_T.Visible = false; pB_M_T.Visible = false;
                pB_P_T.Image = pB_P.Image; pB_M_T.Image = pB_M.Image;
                pB_P_T.Height = pB_P.Height; pB_M_T.Height = pB_M.Height;
                pB_P_T.Width = pB_P.Width; pB_M_T.Width = pB_M.Width;
                                
                pB_M_T.Click += new EventHandler(Minus_ClickHandler);
                pB_P_T.Click += new EventHandler(Plus_ClickHandler);
                Y_Plus[fi_cnt] = pB_P_T;
                Y_Minus[fi_cnt] = pB_M_T;

                panel2.Controls.Add(pB_P_T);
                panel2.Controls.Add(pB_M_T);
            }

            
            
                      
            this.Paint += new PaintEventHandler(Drow_Tree_Scroll); 
            panel2.Paint += new PaintEventHandler(Drow_Tree_Line);
            //panel2.Paint += new PaintEventHandler(Drow_Tree_Scroll);
            panel2.Controls.Add(hSC);
            panel2.Controls.Add(vSC);
            Data_Set_Form_TF = 0;

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;

            string[] data_Font = {"3", "4","5","6", "7" 
                                 , "8", "9", "10"  , "11", "12"
                                 , "13", "14", "15"                                
                              };

            // 각 콤보박스에 데이타를 초기화
            combo_Font.Items.AddRange(data_Font);

            combo_Font.Text = "8";

            trackBar1.Minimum = 3;
            trackBar1.Maximum = 15;
            trackBar1.Value = 8;

            

            if (cls_User.gid_Tree_Config != "" && cls_User.gid_Tree_Config != null )
            {
                string[] date_a = cls_User.gid_Tree_Config.Split('/');

                int sW = 0; 
                foreach (Control t_c in pb_De.Controls)
                {
                    if (t_c is CheckBox == true)
                    {
                        CheckBox t_cb = (CheckBox)t_c;
                        //if (t_cb.Visible == true)
                        //{

                        sW = 0;
                        for (int Cnt = 0; Cnt <= date_a.Length - 1; Cnt++)
                        {

                            if (date_a[Cnt] == t_cb.Name)
                            {
                                sW= 1 ; 
                                t_cb.Checked = true;
                                break;
                            }   

                        }
                        
                        if (sW ==0)
                            t_cb.Checked = false;
                        //}
                    }
                }
            }
            
            //splitContainer1.Dock = DockStyle.Fill;
            //splitContainer1.SplitterDistance = 548;

            if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
            {
                tabC_1.TabPages.Remove(tabPage8);
                tb_Sort_TF.Visible = false;
                opt_C_3.Checked = true; 
            }

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                tabC_1.TabPages.Remove(tabPage5);
                tb_Sort_TF.Visible = false;
                opt_C_2.Checked = true; 
            }

            
        }

        void panel2_MouseWheel(object sender, MouseEventArgs e)
        {
            int TSW = 0;
            if (panel2.Focused == true) TSW = 1;

            if (TSW == 1)
            {
                int oldvalue = 0;
                oldvalue = oldvalue + (e.Delta/12);

                if (vSC.Value + (-oldvalue) > vSC.Maximum)
                {
                    oldvalue = vSC.Maximum - vSC.Value;
                    vSC.Value = vSC.Maximum;
                    TSW = 2;
                }
                else if (vSC.Value + (-oldvalue) < vSC.Minimum )
                {
                    oldvalue = vSC.Minimum - vSC.Value ;
                    vSC.Value = vSC.Minimum;
                    TSW = 3;
                }
                else
                    vSC.Value = vSC.Value + (-oldvalue);


                //int CucTop = 0;
                //string R_Key = "";

                //foreach (int t_key in TreeDic_Cnt.Keys)
                //{
                //    R_Key = TreeDic_Cnt[t_key];
                //    CucTop = TreeDic[R_Key].BaseTop + (oldvalue);
                //    ///TreeDic[R_Key].Top = CucTop;                    
                //    TreeDic[R_Key].Top = TreeDic[R_Key].Top + (oldvalue);

                //    if (TSW == 3)
                //        TreeDic[R_Key].Top = TreeDic[R_Key].BaseTop;

                //}

                //foreach (int t_key in LineDic.Keys)
                //{
                //    //LineDic[t_key].Y1 = LineDic[t_key].BY1 + (oldvalue);
                //    //LineDic[t_key].Y2 = LineDic[t_key].BY2 + (oldvalue);

                //    LineDic[t_key].Y1 = LineDic[t_key].Y1 + (oldvalue);
                //    LineDic[t_key].Y2 = LineDic[t_key].Y2 + (oldvalue);

                //    if (TSW == 3)
                //    {
                //        LineDic[t_key].Y1 = LineDic[t_key].BY1 ;
                //        LineDic[t_key].Y2 = LineDic[t_key].BY2 ;
                //    }
                //}
                                
                //Drow_Tree_Lbl(1);
                //panel2.Refresh();
                ////vSC.Value = vSC.Value + (-oldvalue);
                //panel2.Focus();
            }
        }


        void frmMember_TreeGroup_MouseWheel(object sender, MouseEventArgs e)
        {
            int TSW  =  0;
            if (panel2 .Focused == true )  TSW =1 ;
           
            if (TSW == 1)
            {
                int oldvalue = vSC.Value  ;
                oldvalue= oldvalue + e.Delta ;

                ScrollEventArgs eee = new ScrollEventArgs(ScrollEventType.SmallIncrement, oldvalue);

                vSC_Scroll(vSC, eee);
            }
        }


        void pic_X_DoubleClick(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;            
            PictureBox t_Pic = (PictureBox)sender;
            int keyCnt = int.Parse(t_Pic.Tag.ToString());                       
            
            mtxtMbid.Text = TreeDic[TreeDic_Cnt[keyCnt]].IDKey;
            int reCnt = 0;
            cls_Search_DB cds = new cls_Search_DB();
            string Search_Name = "";
            reCnt = cds.Member_Name_Search(mtxtMbid.Text, ref Search_Name);

            if (reCnt == 1)
            {
                txtName.Text = Search_Name;
            }

            button1_Click(button1, e);

            this.Cursor = System.Windows.Forms.Cursors.Default;       
        }



        void pic_X_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (this.Cursor == System.Windows.Forms.Cursors.Default)
                {
                    this.Cursor = System.Windows.Forms.Cursors.SizeAll;
                    MouseCenterPoint = Cursor.Position;
                    MouseMovePoint = Cursor.Position;
                    timer1.Start();
                }
                else
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }

                return;
            }

            System.Windows.Forms.MouseEventArgs ee = (System.Windows.Forms.MouseEventArgs)e;

            tabC_1.SelectedIndex = 0;

            PictureBox t_Pic = (PictureBox)sender;
            int keyCnt = int.Parse(t_Pic.Tag.ToString());
            int temp_key = 0;

            for (int fi_cnt = 0; fi_cnt <= 400; fi_cnt++)
            {
                if (YPicture[fi_cnt].Tag.ToString() == "")
                    break;
                else
                {
                    temp_key = int.Parse(YPicture[fi_cnt].Tag.ToString());
                    YPicture[fi_cnt].BackColor = TreeDic[TreeDic_Cnt[temp_key]].BackColor;
                }
            }
                        
            string t_mbid = TreeDic[TreeDic_Cnt[keyCnt]].IDKey;
            t_Pic.BackColor = System.Drawing.Color.LightBlue ;
            tabC_1.SelectedIndex = 0;
            Mouse_Select_key = t_mbid;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
            cgbp.dGridView_Put_baseinfo(this, dGridView_Sell, "sell", Mouse_Select_key);

            panel2.Focus();

            cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
            cgbp5.dGridView_Put_baseinfo(dGridView_Sell_Item, "item");

            cls_Grid_Base_info_Put cgbp6 = new cls_Grid_Base_info_Put();
            cgbp6.dGridView_Put_baseinfo(dGridView_Sell_Cacu, "cacu");

            cls_Grid_Base_info_Put cgbp7 = new cls_Grid_Base_info_Put();
            cgbp7.dGridView_Put_baseinfo(dGridView_Sell_Rece, "rece");

            cls_Grid_Base_info_Put cgbp8 = new cls_Grid_Base_info_Put();
            cgbp8.dGridView_Put_baseinfo(dGridView_Pay, "pay");

            cls_Grid_Base_info_Put cgbp9 = new cls_Grid_Base_info_Put();
            cgbp9.dGridView_Put_baseinfo(dGridView_Memberinfo, "member");

            cls_Grid_Base_info_Put cgbp10 = new cls_Grid_Base_info_Put();
            cgbp10.dGridView_Put_baseinfo(dGridView_Up_Sa, "saveup");

            cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
            cgbp11.dGridView_Put_baseinfo(dGridView_Up_N, "nominup");

            this.Cursor = System.Windows.Forms.Cursors.Default;       


            //if (e.Button == MouseButtons.Right)
            //{
            //    //Point t_P = e.Location;               
            //    //t_P.X = t_P.X + t_Pic.Left ;
            //    //t_P.Y = t_P.Y + t_Pic.Top + panel2.Top ;                
            //    //contextM.Show(this.PointToScreen(t_P));                

            //    contextM.Tag = keyCnt.ToString();
            //    contextM.Show(t_Pic, e.Location);                
            //}

        }

      


        private void frmBase_Resize(object sender, EventArgs e)
        {
           
            butt_Clear.Left = 0;
            butt_Select.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_PreView.Left = butt_Select.Left + butt_Select.Width + 2;
            butt_Excel.Left = butt_PreView.Left + butt_PreView.Width + 2;            
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Select);
            cfm.button_flat_change(butt_PreView);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);
            cfm.button_flat_change(but_Exp);
            cfm.button_flat_change(butt_De);
            cfm.button_flat_change(but_Up); 
            
            
        }


        private void frmBase_From_KeyDown(object sender, KeyEventArgs e)
        {
            //폼일 경우에는 ESC버튼에 폼이 종료 되도록 한다
            if (sender is Form)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (tableLayoutPanel11.Visible == true)
                        tableLayoutPanel11.Visible = false; 
                    else if (!this.Controls.ContainsKey("Popup_gr"))
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

                            //    cls_form_Meth cfm = new cls_form_Meth();
                            //    cfm.form_Group_Panel_Enable_True(this);
                        }
                    }
                }// end if

            }

            //그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
            if (sender is DataGridView)
            {
                if (e.KeyValue == 46)
                {
                    e.Handled = true;
                } // end if

                if (e.KeyValue == 13)
                {
                    //EventArgs ee = null;
                    //dGridView_Base_DoubleClick(sender, ee);
                    e.Handled = true;
                } // end if
            }

            Button T_bt = butt_Exit;
            if (e.KeyValue == 123)
                T_bt = butt_Exit;    //닫기  F12
            if (e.KeyValue == 113)
                T_bt = butt_Select;     //조회  F1
            //if (e.KeyValue == 115)
            //T_bt = butt_Delete;   // 삭제  F4
            //if (e.KeyValue == 119)
            //T_bt = butt_Excel;    //엑셀  F8    
            if (e.KeyValue == 112)
                T_bt = butt_Clear;    //엑셀  F5    

            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123 || e.KeyValue == 119 || e.KeyValue == 112)
                    Base_Button_Click(T_bt, ee1);

                if (e.KeyValue == 113 )
                    button1_Click(T_bt, ee1);
                    
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

                  
                    //SendKeys.Send("{TAB}");
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
                if (mtxtMbid.Text.Trim() != "")
                {
                    tableLayoutPanel11.Visible = false;

                    int reCnt = 0;

                    if (TreeDic != null)
                    {
                        if (TreeDic.Count  > 0)
                            Clear_Object(1);  //엔터를 눌럿다는 거는 다른 사람으로 변경한다는 거고 그럼 리셋을 한다. 
                    }

                    cls_Search_DB cds = new cls_Search_DB();
                    string Search_Name = "";
                    reCnt = cds.Member_Name_Search(mtxtMbid.Text, ref Search_Name);

                    if (reCnt == 1)
                    {
                        txtName.Text = Search_Name;
                        MEmber_Level_Search();
                    }
                    else if (reCnt > 1)  //회원번호 비슷한 사람들이 많은 경우
                    {
                        string Mbid = "";
                        int Mbid2 = 0;
                        cds.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2);

                        //cls_app_static_var.Search_Member_Number_Mbid = Mbid;
                        //cls_app_static_var.Search_Member_Number_Mbid2 = Mbid2;
                        frmBase_Member_Search e_f = new frmBase_Member_Search();
                        e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                        e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info);
                        e_f.ShowDialog();

                        //txtMemberName.Text = cls_app_static_var.Search_Member_Name_Return;
                        //mtxtMbid.Text = cls_app_static_var.Search_Member_Number_Return;     
                    }
                }

                SendKeys.Send("{TAB}");
            }
        }

        void e_f_Send_MemNumber_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = "";            
            cls_Search_DB csb = new cls_Search_DB();
            csb.Member_Nmumber_Split(mtxtMbid.Text.Trim(), ref searchMbid, ref searchMbid2);
        }

        void e_f_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            mtxtMbid.Text = Send_Number;
            txtName.Text = Send_Name;
        }

        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {
            if (mtxtMbid.Text.Replace("_", "").Replace("-", "").Replace(" ", "") == "")
            {
                txtName.Text = "";
            }
        }


        private void MEmber_Level_Search()
        {
            combo_Step.Items.Clear();
            Down_Max_Level = 0;

            string Mbid = ""; int Mbid2 = 0;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) <= 0) return;


            string Tsql = "";
            if (opt_C_2.Checked == true)
                Tsql = "Select Isnull(Max(lvl) ,0) from ufn_GetSubTree_MemGroup ('" + Mbid + "'," + Mbid2 + ") ";
            else
                Tsql = "Select Isnull(Max(lvl) ,0) from ufn_GetSubTreeView_Mem_Nomin ('" + Mbid + "'," + Mbid2 + ") ";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "ufn_Up_Search_Save", ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;

            Down_Max_Level = int.Parse(ds.Tables["ufn_Up_Search_Save"].Rows[0][0].ToString());
            int Cnt = 0;
            while (Cnt <= Down_Max_Level)
            {
                combo_Step.Items.Add(Cnt.ToString());
                Cnt++;
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



        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
            T_R.Key_Enter_13_Name += new Key_13_Name_Event_Handler(T_R_Key_Enter_13_Name);
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
            else if (tb.Tag.ToString() == "1")
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if (tb.Tag.ToString() == "name")
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(tb, e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }
            else if (tb.Tag.ToString() == "ncode") //코드관련해서 코드를치면 관련 내역이 나오도록 하기 위함.
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

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
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

        void T_R_Key_Enter_13_Name(string txt_tag, TextBox tb)
        {
            if (txt_tag != "")
            {
                tableLayoutPanel11.Visible = false;

                int reCnt = 0;
                cls_Search_DB cds = new cls_Search_DB();
                string Search_Mbid = "";
                reCnt = cds.Member_Name_Search(ref Search_Mbid, txt_tag);

                if (reCnt == 1)
                {
                    if (tb.Name == "txtName")
                    {
                        mtxtMbid.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
                        MEmber_Level_Search();
                    }
                }
                else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
                {
                    //cls_app_static_var.Search_Member_Name = txt_tag;
                    frmBase_Member_Search e_f = new frmBase_Member_Search();
                    e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                    e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);
                    e_f.ShowDialog();
                }
                SendKeys.Send("{TAB}");
            }

        }


        void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {            
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtName.Text.Trim();
        }

        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
            if (tb.Name == "txtSellCode")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtSellCode_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtSellCode_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtSellCode_Code);

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
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtCenter2")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtR_Id")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtChange")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtSellCode")
                cgb_Pop.Next_Focus_Control = butt_Select;


            cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
        }

        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            //SendKeys.Send("{TAB}");
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
                    Tsql = Tsql + " Where  U_TF = 0 "; //사용센타만 보이게 한다 
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
                Tsql = Tsql + " Where (Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
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




        private void Group_Tree_Print_Pre_Work()
        {
            Print_Cut_int = 1;
            int BH = BaseDoc.DefaultPageSettings.PaperSize.Height * Print_Cut_int;
            int BW = BaseDoc.DefaultPageSettings.PaperSize.Width * Print_Cut_int ;
            string R_Key = ""; int maxLeft = 0; int minTop = 0;
            W_Print_PagCnt = 0;
            H_Print_PagCnt = 0; 

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                R_Key = TreeDic_Cnt[t_key];
                if (maxLeft < TreeDic[R_Key].BaseLeft)
                    maxLeft = TreeDic[R_Key].BaseLeft + TreeDic[R_Key].Width;

                if (minTop < TreeDic[R_Key].BaseTop)
                    minTop = TreeDic[R_Key].BaseTop + TreeDic[R_Key].Height;
            }
            W_Print_PagCnt = (maxLeft / BH);
            if ((maxLeft % BW) > 0)
                W_Print_PagCnt = W_Print_PagCnt + 1;

            H_Print_PagCnt = (minTop / BH);
            if ((minTop % BH) > 0)
                H_Print_PagCnt = H_Print_PagCnt + 1;

            Print_W_Cur_PagCnt = 1;
            Print_H_Cur_PagCnt = 1;
        }



        private void Clear_Object()
        {
            panel2.Visible = false;
            if (TreeDic_Cnt != null)
                TreeDic_Cnt.Clear();
            if (TreeDic != null)
                TreeDic.Clear();

            if (LineDic != null)
                LineDic.Clear();

            vSC.Visible = false;
            hSC.Visible = false;

            //for (int fi_cnt = 0; fi_cnt <= 300; fi_cnt++)
            //{
            //    YLabel[fi_cnt].Visible = false;
            //    YLabel[fi_cnt].Tag = "";
            //}

            for (int fi_cnt = 0; fi_cnt <= 400; fi_cnt++)
            {
                YPicture[fi_cnt].Visible = false;
                YPicture[fi_cnt].Tag = "";
            }

            for (int fi_cnt = 0; fi_cnt <= 400; fi_cnt++)
            {
                Y_Minus[fi_cnt].Visible = false;
                Y_Minus[fi_cnt].Tag = "";
            }

            for (int fi_cnt = 0; fi_cnt <= 400; fi_cnt++)
            {
                Y_Plus[fi_cnt].Visible = false;
                Y_Plus[fi_cnt].Tag = "";
            }

            combo_Step.Items.Clear();
            trackBar1.Value = 8;
            combo_Font.Text = "8";

            Mouse_Select_key = "";
            vScroll_Be_Value = 0;

            dGridView_Up_S_Header_Reset(dGridView_Up_S); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset();
            mtxtMbid.Text = ""; txtName.Text = ""; 
            txtDownCnt_2.Text = "";  txtDownCnt.Text ="";


            cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
            cgbp.dGridView_Put_baseinfo(dGridView_Sell, "sell");

            cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
            cgbp5.dGridView_Put_baseinfo(dGridView_Sell_Item, "item");

            cls_Grid_Base_info_Put cgbp6 = new cls_Grid_Base_info_Put();
            cgbp6.dGridView_Put_baseinfo(dGridView_Sell_Cacu, "cacu");

            cls_Grid_Base_info_Put cgbp7 = new cls_Grid_Base_info_Put();
            cgbp7.dGridView_Put_baseinfo(dGridView_Sell_Rece, "rece");

            cls_Grid_Base_info_Put cgbp8 = new cls_Grid_Base_info_Put();
            cgbp8.dGridView_Put_baseinfo( dGridView_Pay, "pay");

            cls_Grid_Base_info_Put cgbp9 = new cls_Grid_Base_info_Put();
            cgbp9.dGridView_Put_baseinfo(dGridView_Memberinfo, "member");


            cls_Grid_Base_info_Put cgbp10 = new cls_Grid_Base_info_Put();
            cgbp10.dGridView_Put_baseinfo( dGridView_Up_Sa, "saveup");

            cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
            cgbp11.dGridView_Put_baseinfo( dGridView_Up_N, "nominup");


            panel2.Refresh();
            panel2.Visible = true;
            opt_C_2.Checked = true;
            mtxtSellDate3.Text = DateTime.Now.ToString("yyyy-MM-dd");

            if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
            {
                tabC_1.TabPages.Remove(tabPage8);
                tb_Sort_TF.Visible = false;
                opt_C_3.Checked = true;
                opt_C_2.Checked = false;
            }

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
            {
                tabC_1.TabPages.Remove(tabPage5);
                tb_Sort_TF.Visible = false;
                opt_C_2.Checked = true;
                opt_C_3.Checked = false;
            }

            tableLayoutPanel11.Visible = false;

            mtxtMbid.Focus();
        }


        private void Clear_Object(int t_s)
        {
            panel2.Visible = false;
            if (TreeDic_Cnt != null)
                TreeDic_Cnt.Clear();
            if (TreeDic != null)
                TreeDic.Clear();

            if (LineDic != null)
                LineDic.Clear();



            //for (int fi_cnt = 0; fi_cnt <= 300; fi_cnt++)
            //{
            //    YLabel[fi_cnt].Visible = false;
            //    YLabel[fi_cnt].Tag = "";
            //}

            for (int fi_cnt = 0; fi_cnt <= 400; fi_cnt++)
            {
                YPicture[fi_cnt].Visible = false;
                YPicture[fi_cnt].Tag = "";
            }

            for (int fi_cnt = 0; fi_cnt <= 400; fi_cnt++)
            {
                Y_Minus[fi_cnt].Visible = false;
                Y_Minus[fi_cnt].Tag = "";
            }

            for (int fi_cnt = 0; fi_cnt <= 400; fi_cnt++)
            {
                Y_Plus[fi_cnt].Visible = false;
                Y_Plus[fi_cnt].Tag = "";
            }

            combo_Font.Text = "8";
            Mouse_Select_key = "";
            vScroll_Be_Value = 0;

            dGridView_Up_S_Header_Reset(dGridView_Up_S); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset();
            
            txtDownCnt_2.Text = ""; txtDownCnt.Text = "";


            cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
            cgbp.dGridView_Put_baseinfo(dGridView_Sell, "sell");

            cls_Grid_Base_info_Put cgbp5 = new cls_Grid_Base_info_Put();
            cgbp5.dGridView_Put_baseinfo(dGridView_Sell_Item, "item");

            cls_Grid_Base_info_Put cgbp6 = new cls_Grid_Base_info_Put();
            cgbp6.dGridView_Put_baseinfo(dGridView_Sell_Cacu, "cacu");

            cls_Grid_Base_info_Put cgbp7 = new cls_Grid_Base_info_Put();
            cgbp7.dGridView_Put_baseinfo(dGridView_Sell_Rece, "rece");

            cls_Grid_Base_info_Put cgbp8 = new cls_Grid_Base_info_Put();
            cgbp8.dGridView_Put_baseinfo(dGridView_Pay, "pay");

            cls_Grid_Base_info_Put cgbp9 = new cls_Grid_Base_info_Put();
            cgbp9.dGridView_Put_baseinfo(dGridView_Memberinfo, "member");


            cls_Grid_Base_info_Put cgbp10 = new cls_Grid_Base_info_Put();
            cgbp10.dGridView_Put_baseinfo(dGridView_Up_Sa, "saveup");

            cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
            cgbp11.dGridView_Put_baseinfo(dGridView_Up_N, "nominup");
            
            panel2.Refresh();
            panel2.Visible = true;            
        }


        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_PreView")   //프린터 미리보기임
            {
                if (TreeDic_Cnt == null)
                    return;
                if (TreeDic_Cnt.Count == 0)
                    return;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Group_Tree_Print_Pre_Work();
                this.Cursor = System.Windows.Forms.Cursors.Default;
                prPrview.ShowDialog();

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                string Tsql = "";
                string fileName = mtxtMbid.Text.Trim() + "_Print";
                Tsql = "Insert Into tbl_Excel_User Values ( ";
                Tsql = Tsql + "'" + cls_User.gid + "',Convert(Varchar(25),GetDate(),21),";
                Tsql = Tsql + "'" + this.Name  + "',";
                Tsql = Tsql + "'" + fileName + "') ";

                Temp_Connect.Insert_Data(Tsql, "tbl_Excel_User");
            }



            if (bt.Name == "butt_Clear")
            {
                Clear_Object();
            }


            if (bt.Name == "butt_Save")
            {
                pageSetup.ShowDialog();
            }


            if (bt.Name == "butt_Exit")
            {
                this.Close();
            }

            else if (bt.Name == "butt_Excel")   //이미지 저장
            {
                if (TreeDic_Cnt == null)
                    return;
                if (TreeDic_Cnt.Count == 0)
                    return;




                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                progressBar1.Visible = true;
                Group_Tree_Print_Pre_BP_Work();
                progressBar1.Visible = false;

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                string Tsql = "";
                string fileName = mtxtMbid.Text.Trim() + "_Image";
                Tsql = "Insert Into tbl_Excel_User Values ( ";
                Tsql = Tsql + "'" + cls_User.gid + "',Convert(Varchar(25),GetDate(),21),";
                Tsql = Tsql + "'" + this.Name + "',";
                Tsql = Tsql + "'" + fileName + "') ";

                Temp_Connect.Insert_Data(Tsql, "tbl_Excel_User");
                
                this.Cursor = System.Windows.Forms.Cursors.Default;                
            }

            

            //else if (bt.Name == "butt_Delete")
            //{
            //    int Delete_Error_Check = 0;

            //    if (cls_User.gid_Sell_Del_TF == 0) //구매취소 권한이 있는 사람만 가능하다.
            //    {
            //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sell_Del_Not_TF")
            //          + "\n" +
            //          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //        return;
            //    }

            //    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //    Delete_Base_Data(ref Delete_Error_Check);

            //    if (Delete_Error_Check > 0)
            //    {
            //        Base_Ord_Clear();

            //        if (SalesDetail != null)
            //            SalesDetail.Clear();

            //        Set_SalesDetail();  //회원의 구매 관련 주테이블 내역을 클래스에 넣는다.

            //        if (SalesDetail != null)
            //            Base_Grid_Set();
            //    }

            //    this.Cursor = System.Windows.Forms.Cursors.Default;
            //}
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
                    mtxtMbid.Focus(); return false;
                }
            }

            if (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                cls_Search_DB csb = new cls_Search_DB();
                string Search_Name = csb.Member_Name_Search(mtxtMbid.Text);


                if (Search_Name == "-1") //회원번호가 올바르게 입력 되어 있는지
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtMbid.Focus();
                    return false;
                }


                else if (Search_Name != "")  //이름이 튀어나오면 회원이 존재하고 올바르게 입력된거임
                    txtName.Text = Search_Name;

                else if (Search_Name == "")  //이름이 안튀어 나오면 회원이 존재하지 않는 거임.
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtMbid.Focus();
                    return false;
                }
                else //이도 저도 아닌 -2 같은 에러가 나온다. 그럼 다 리셋 시켜 버린다.
                {
                    mtxtMbid.Text = ""; txtName.Text = "";
                }

            }//센타장으로 해서 회원번호를 입력한 경우
            else
                txtName.Text = "";   //회원번호 입력 안되어있는 데 회원명 입력 될수 있기 때문에 그런 경우를 대비해서  회원명을 빈칸으로 함.


            if (txtName.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtMbid.Focus();
                return false;
            }


            if (mtxtSellDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate1.Text, mtxtSellDate1, "Date") == false)
                {
                    mtxtSellDate1.Focus();
                    return false;
                }
            }

            if (mtxtSellDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate2.Text, mtxtSellDate2, "Date") == false)
                {
                    mtxtSellDate2.Focus();
                    return false;
                }

            }

            if (mtxtSellDate3.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate3.Text, mtxtSellDate3, "Date") == false)
                {
                    mtxtSellDate3.Focus();
                    return false;
                }

            }
            if (mtxtSellDate4.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate4.Text, mtxtSellDate4, "Date") == false)
                {
                    mtxtSellDate4.Focus();
                    return false;
                }

            }

            if (mtxtSellDate5.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate5.Text, mtxtSellDate5, "Date") == false)
                {
                    mtxtSellDate5.Focus();
                    return false;
                }

            }


            return true;
        }

        private void Chnage_gid_Tree_Config ()
        {
            cls_User.gid_Tree_Config = "";

            foreach (Control t_c in pb_De.Controls)
            {
                if (t_c is CheckBox == true)
                {
                    CheckBox t_cb = (CheckBox)t_c;
                    
                    if (cls_User.gid_Tree_Config == "")
                        cls_User.gid_Tree_Config = t_cb.Name;
                    else
                        cls_User.gid_Tree_Config = cls_User.gid_Tree_Config + "/" + t_cb.Name;
                    
                }
            }

            string TSql = "Update tbl_User Set ";
            TSql = TSql + " Tree_Config = '" + cls_User.gid_Tree_Config + "' ";
            TSql = TSql + " Where upper(user_id) = '" + (cls_User.gid).ToUpper() + "'";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Update_Data(TSql);

        }


        private void button1_Click(object sender, EventArgs e)
        {
            tableLayoutPanel11.Visible = false;
            
            if (Check_TextBox_Error() == false) return;

            if (TreeDic != null)
                TreeDic.Clear();
            if (LineDic != null)
                LineDic.Clear();
            if (TreeDic_Cnt != null)
                TreeDic_Cnt.Clear();

            dGridView_Up_S_Header_Reset(dGridView_Up_S); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset();

            Down_Max_Level = 0; 
            
            Mouse_Select_key = "";

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //"KR", 121  큰넘


            string Mbid = ""; int Mbid2 = 0; int P_Sw = 0 ;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) <= 0) return;
            combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex;

            prB_Save.Minimum = 0; prB_Save.Maximum = 120;
            prB_Save.Step = 10;     prB_Save.Value = 0;



            hSC.Value = 0;
            vSC.Value = 0;
            if (opt_C_4.Checked == true)
            {
                if (Tree_Mem_Get_Ui(Mbid, Mbid2, mtxtSellDate3.Text.Replace("-", "").Trim(), 1, txtSellCode_Code.Text.Trim(), mtxtSellDate1.Text.Replace("-", "").Trim(), mtxtSellDate2.Text.Replace("-", "").Trim(), mtxtSellDate4.Text.Replace("-", "").Trim(), mtxtSellDate5.Text.Replace("-", "").Trim()) == true)
                    P_Sw = 1;
            }


            if (opt_C_3.Checked == true)
             {
                if (Tree_Mem_Get_Nom(Mbid, Mbid2, mtxtSellDate3.Text.Replace("-", "").Trim(), 1, txtSellCode_Code.Text.Trim(), mtxtSellDate1.Text.Replace("-", "").Trim(), mtxtSellDate2.Text.Replace("-", "").Trim(), mtxtSellDate4.Text.Replace("-", "").Trim(), mtxtSellDate5.Text.Replace("-", "").Trim()) == true)
                    P_Sw = 1;
            }

            if (opt_C_2.Checked == true)
            {
                if (Tree_Mem_Get(Mbid, Mbid2, mtxtSellDate3.Text.Replace("-", "").Trim(), 1, txtSellCode_Code.Text.Trim(), mtxtSellDate1.Text.Replace("-", "").Trim(), mtxtSellDate2.Text.Replace("-", "").Trim(), mtxtSellDate4.Text.Replace("-", "").Trim(), mtxtSellDate5.Text.Replace("-", "").Trim()) == true)
                    P_Sw = 1;
            }

            if (P_Sw == 1 )
            {
                prB_Save.BringToFront();
                prB_Save.Visible = true;
                Clearlbl();
                prB_Save.PerformStep(); prB_Save.Refresh();

                MakeTreeLebel_Save(); //디비접속해서 회원 하선들의 정보를 가져온다
                prB_Save.PerformStep(); prB_Save.Refresh();

                DrawData_Label_Position_Top(); //회원 클래스 들의 Top정보를 계산해서 넣는다.
                prB_Save.PerformStep(); prB_Save.Refresh();

                Label_Drow_Left();    //회원 클래스 들의 Left 정보를 계산해서 넣는다.
                prB_Save.PerformStep(); prB_Save.Refresh();

                Up_Left_Total_Move(); //회원 클래스 들을 자식들에 맞춰서 left를 조정한다.
                prB_Save.PerformStep(); prB_Save.Refresh();

                Line_Drow_Position();  //회원 클래스들을 기준으로 해서 자식들과 연결하는 선의 위치 클래스들을 생성 위치값을 구한다.
                prB_Save.PerformStep(); prB_Save.Refresh();

                MakeTreeLebel_Save(1);
                prB_Save.PerformStep(); prB_Save.Refresh();

                //Make_Down_Level();
                //panel2.Visible = false;
                //prB_Save.PerformStep(); prB_Save.Refresh();

                //Drow_Tree_Lbl();
                Drow_Tree_Lbl(1);
                PaintEventArgs d =null;
                prB_Save.PerformStep(); prB_Save.Refresh();

                Drow_Tree_Line(sender, d);
                prB_Save.PerformStep(); prB_Save.Refresh();

                Drow_Tree_Scroll(this, d);
                panel2.Visible = true;
                prB_Save.PerformStep(); prB_Save.Refresh();

                Set_Form_Date_Up("S");
                prB_Save.PerformStep(); prB_Save.Refresh();
                prB_Save.Visible = false;

                Group_Tree_Re_Drawing();
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;



        }


        private void Clearlbl()
        {
            foreach (int t_key in YLabel.Keys)
            {
                YLabel[t_key].Visible = false;
                YLabel[t_key].Text  = "";
                YLabel[t_key].Tag  = "";
                
            }
        }



        private void Make_Down_Level()
        {
            //string[] data_Step = new string [Down_Max_Level] ;

            //int Cnt = 1 ;
            //while (Cnt <= Down_Max_Level)
            //{
            //    combo_Step.Items.Add(Cnt.ToString());
            //    Cnt++;
            //}           
                       
            //combo_Step.Text = Down_Max_Level.ToString () ;
        }


        private void MakeTreeLebel_Save()
        {
            int Check_Cnt = 0;
            int MaxWidthNum = 0; int MaxHeightNum = 0; LastLvl = 0;
            lblY.Font = new System.Drawing.Font("돋움", float.Parse(combo_Font.Text.ToString()));
            lblY.Text = "";

            foreach (string t_key in TreeDic.Keys)
            {
                Key_Name_Change(t_key, ref Check_Cnt); //나오는 TEXT를 변경한다.

                lblY.Text = ""; lblY.Refresh();            
                lblY.Text = TreeDic[t_key].KeyName;
                lblY.Refresh();   
                
                if (MaxWidthNum < (lblY.Width ))
                    MaxWidthNum = lblY.Width ;

                if (MaxHeightNum < (lblY.Height +1))
                    MaxHeightNum = lblY.Height +1 ;

                TreeDic[t_key].TDownPV = "0";
                TreeDic[t_key].TDownBV = "0";

                if  (TreeDic[t_key].Lvl >  LastLvl)
                    LastLvl = TreeDic[t_key].Lvl ;

                TreeDic[t_key].f_TDownPV = 0;
                TreeDic[t_key].f_TDownBV = 0;
            }

            //MaxWidthNum= MaxWidthNum + 3 ;
            //MaxHeightNum = MaxHeightNum + 5;
            int Expend_H  = 0 ;
            if (Check_Cnt >= 4)
                Expend_H = (Check_Cnt - 3) * 2;


            foreach (string t_key in TreeDic.Keys)
            {
                TreeDic[t_key].Height = MaxHeightNum + Expend_H;
                TreeDic[t_key].Width = MaxWidthNum;
            }     

        }




        private void DrawData_Label_Position_Top()
        {   string R_Key ="" ;

            R_Key = TreeDic_Cnt[0];
            TreeDic[R_Key].Top = IntervalHeight;
            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                if (t_key > 0)
                {
                    R_Key = TreeDic_Cnt[t_key];
                    TreeDic[R_Key].Top = TreeDic[R_Key].ParentClass.Top + TreeDic[R_Key].ParentClass.Height + IntervalHeight;
                }

            }  
        }

        private void Label_Drow_Left()
        {
            string R_Key = ""; string P_Key = "";
            int move_with = 0;
            R_Key = TreeDic_Cnt[0];
            TreeDic[R_Key].Left  = 10;

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                if (t_key > 0)
                {
                    R_Key = TreeDic_Cnt[t_key];
                    P_Key = TreeDic[R_Key].ParentClass.IDKey;

                    if (TreeDic[R_Key].ParentClass.ChildCount == 1)
                        TreeDic[R_Key].Left = TreeDic[R_Key].ParentClass.Left;
                    else
                    {
                        int f_Cnt = 0 ;
                        foreach (int C_key in TreeDic[P_Key].ChildNumber.Keys )
                        {
                            if (TreeDic[P_Key].ChildNumber[C_key].IDKey == R_Key)
                            {
                                TreeDic[R_Key].Left = TreeDic[P_Key].Left + (TreeDic[R_Key].Width * f_Cnt) + (IntervalWidth * f_Cnt);

                                if (f_Cnt > 0)
                                {
                                    move_with = TreeDic[R_Key].Width  + IntervalWidth;
                                    Up_Left_Total_Move(move_with, TreeDic[R_Key].Left, TreeDic[R_Key].Lvl);
                                }
                                
                                break;
                            }
                            f_Cnt++;
                            
                        }                        
                    }
                }
            } 


        }



        private void Up_Left_Total_Move(int move_with ,int Base_Left , int Base_Lvl )
        {
            string R_Key = ""; 

            foreach (int t_key in TreeDic_Cnt.Keys)
            {               
                R_Key = TreeDic_Cnt[t_key];

                if (TreeDic[R_Key].Lvl == Base_Lvl)
                    break;
                else
                {
                    if (TreeDic[R_Key].Left >= Base_Left)
                        TreeDic[R_Key].Left = TreeDic[R_Key].Left + move_with;

                }                                
            } 
        }




        private void Up_Left_Total_Move()
        {
            string R_Key = ""; string P_key = "";
            //LastLvl = LastLvl -1 ;

            for (int Tcnt = LastLvl; Tcnt > 0; Tcnt--)
            {
                foreach (int t_key in TreeDic_Cnt.Keys)
                {
                    R_Key = TreeDic_Cnt[t_key];

                    if (TreeDic[R_Key].Lvl == Tcnt)                        
                    {
                        P_key = TreeDic[R_Key].ParentKey;
                        if (TreeDic[P_key].ChildCount >= 2)
                        {
                            TreeDic[P_key].Left = TreeDic[P_key].ChildNumber[1].Left  +
                                ((
                                (TreeDic[P_key].ChildNumber[TreeDic[P_key].ChildCount].Left + TreeDic[P_key].ChildNumber[TreeDic[P_key].ChildCount].Width )
                                - (TreeDic[P_key].ChildNumber[1].Left + TreeDic[P_key].ChildNumber[1].Width)
                                ) 
                                / 2) ;                           
                        }
                        else if (TreeDic[P_key].ChildCount == 1)
                        {
                            TreeDic[P_key].Left = TreeDic[P_key].ChildNumber[1].Left;
                        }

                    }
                }
            }
        }

        private void MakeTreeLebel_Save(int T)
        {
            //var varList = TreeDic_Cnt.Keys.ToList();
            //varList.Sort();
            //foreach (var d in varList)
            //{

            //    Console.WriteLine("{0}: {1}", d, dictionary[d]);

            //}
            var items = from pair in TreeDic_Cnt
                    orderby pair.Key descending
                    select pair;

            string ParentKey = ""; 
            foreach (var d in items)
            {
                string t_key = d.Value;

                TreeDic[t_key].BaseLeft = TreeDic[t_key].Left;
                TreeDic[t_key].BaseTop = TreeDic[t_key].Top;

                Base_TreeDic[t_key].Left = TreeDic[t_key].Left;
                Base_TreeDic[t_key].Top = TreeDic[t_key].Top;

                Base_TreeDic[t_key].BaseLeft = TreeDic[t_key].BaseLeft;
                Base_TreeDic[t_key].BaseTop = TreeDic[t_key].BaseTop;

                Base_TreeDic[t_key].Height = TreeDic[t_key].Height;
                Base_TreeDic[t_key].Width = TreeDic[t_key].Width;

                if (d.Key > 0)
                {
                    ParentKey = TreeDic[t_key].ParentKey;
                    TreeDic[ParentKey].f_TDownPV = TreeDic[ParentKey].f_TDownPV + (TreeDic[t_key].f_TDownPV + TreeDic[t_key].f_TotalPV);
                    TreeDic[ParentKey].f_TDownBV = TreeDic[ParentKey].f_TDownBV + (TreeDic[t_key].f_TDownBV + TreeDic[t_key].f_TotalBV);
                }
                //Console.WriteLine("{0}: {1}", d.Key, d.Value);

            } 



            


            
            //foreach (string t_key in TreeDic.Keys )
            //{
            //    TreeDic[t_key].BaseLeft =  TreeDic[t_key].Left ;
            //    TreeDic[t_key].BaseTop =  TreeDic[t_key].Top ;

            //    Base_TreeDic[t_key].Left = TreeDic[t_key].Left;
            //    Base_TreeDic[t_key].Top = TreeDic[t_key].Top;

            //    Base_TreeDic[t_key].BaseLeft = TreeDic[t_key].BaseLeft;
            //    Base_TreeDic[t_key].BaseTop = TreeDic[t_key].BaseTop;

            //    Base_TreeDic[t_key].Height = TreeDic[t_key].Height;
            //    Base_TreeDic[t_key].Width = TreeDic[t_key].Width;

            //    ParentKey = TreeDic[t_key].ParentKey ;
            //    TreeDic[ParentKey].f_TDownPV = TreeDic[ParentKey].f_TDownPV + (TreeDic[t_key].f_TDownPV + TreeDic[t_key].f_TotalPV);
            //}

        }



        private void Line_Drow_Position()
        {
            string R_Key = ""; int LineSrcCntNum = 1; int Half = 0;
            int StartX = 0; int EndX = 0;
            Dictionary<int, cls_Tree_Line> T_TreeDic = new Dictionary<int, cls_Tree_Line>();

            int Base_Level = 0, TSw = 0  ;
            if (combo_Step.Text.ToString() != "")
            {
                Base_Level = int.Parse(combo_Step.Text.ToString());
                TSw = 1;
            }

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                R_Key = TreeDic_Cnt[t_key];
                Half = TreeDic[R_Key].Width / 2;


                if (TreeDic[R_Key].Lvl <= Base_Level || TSw == 0)  // 레벨 설정관련해서 새롭게 첨가된 부분 2014_03_15  지정 기준 레벨보다 작으면 이기능이 필요 없으므로
                {
                    if (t_key > 0)  //제일위에 최상위는 위로 올라가는 선을 그릴필요 없으므로 넣지 않는다.
                    {
                        cls_Tree_Line t_Line_Treel = new cls_Tree_Line();

                        t_Line_Treel.VisibleTF = true;
                        t_Line_Treel.X1 = TreeDic[R_Key].Left + Half;
                        t_Line_Treel.X2 = t_Line_Treel.X1;
                        t_Line_Treel.Y1 = TreeDic[R_Key].Top - (IntervalHeight / 2);
                        t_Line_Treel.Y2 = TreeDic[R_Key].Top;

                        t_Line_Treel.BX1 = t_Line_Treel.X1; t_Line_Treel.BX2 = t_Line_Treel.X2;
                        t_Line_Treel.BY1 = t_Line_Treel.Y1; t_Line_Treel.BY2 = t_Line_Treel.Y2;

                        T_TreeDic[LineSrcCntNum] = t_Line_Treel;
                        LineSrcCntNum++;  //<<< 여기까지 본인에서 위로 반까지 올라가는 선을 그린다.
                    }

                    if (TreeDic[R_Key].Lvl < Base_Level || TSw == 0)  // 레벨 설정관련해서 새롭게 첨가된 부분 2014_03_15  지정 기준 레벨보다 작으면 이기능이 필요 없으므로
                    {
                        if (TreeDic[R_Key].ChildCount > 0)  //하선에 사람이 잇으면 아래로 내려가는 선과 2명이상시 옆으로 가는 선을
                        {
                            cls_Tree_Line t_Line_Treel_2 = new cls_Tree_Line();

                            t_Line_Treel_2.VisibleTF = true;
                            t_Line_Treel_2.X1 = TreeDic[R_Key].Left + Half;
                            t_Line_Treel_2.Y1 = TreeDic[R_Key].Top + TreeDic[R_Key].Height;
                            t_Line_Treel_2.X2 = t_Line_Treel_2.X1;
                            t_Line_Treel_2.Y2 = t_Line_Treel_2.Y1 + (IntervalHeight / 2);

                            t_Line_Treel_2.BX1 = t_Line_Treel_2.X1; t_Line_Treel_2.BX2 = t_Line_Treel_2.X2;
                            t_Line_Treel_2.BY1 = t_Line_Treel_2.Y1; t_Line_Treel_2.BY2 = t_Line_Treel_2.Y2;

                            T_TreeDic[LineSrcCntNum] = t_Line_Treel_2;
                            LineSrcCntNum++;  //<<< 여기까지 본인에서 아래로 반까지 내려가는는 선을 그린다.



                            if (TreeDic[R_Key].ChildCount >= 2)  //하선에 2명이상이다 그럼 옆으로 긋는선을 만들어 주어야 한다.
                            {
                                StartX = TreeDic[R_Key].ChildNumber[1].Left + Half;
                                EndX = TreeDic[R_Key].ChildNumber[TreeDic[R_Key].ChildCount].Left + Half;

                                cls_Tree_Line t_Line_Treel_3 = new cls_Tree_Line();

                                t_Line_Treel_3.VisibleTF = true;
                                t_Line_Treel_3.X1 = StartX;
                                t_Line_Treel_3.Y1 = t_Line_Treel_2.Y2;
                                t_Line_Treel_3.X2 = EndX;
                                t_Line_Treel_3.Y2 = t_Line_Treel_2.Y2;

                                t_Line_Treel_3.BX1 = t_Line_Treel_3.X1; t_Line_Treel_3.BX2 = t_Line_Treel_3.X2;
                                t_Line_Treel_3.BY1 = t_Line_Treel_3.Y1; t_Line_Treel_3.BY2 = t_Line_Treel_3.Y2;

                                T_TreeDic[LineSrcCntNum] = t_Line_Treel_3;
                                LineSrcCntNum++;  //<<< 여기까지 본인에서 위로 반까지 올라가는 선을 그린다.
                            }
                        }
                    }

                }
            }

            LineDic = T_TreeDic;
        }




        private void Drow_Tree_Lbl()
        {
            string R_Key = ""; int lblVTF_Cnt = 0;
            foreach (int t_key in Base_TreeDic_Cnt.Keys)
            {
                R_Key = Base_TreeDic_Cnt[t_key];

                if (TreeDic.ContainsKey(R_Key) == true)
                {
                    if (
                        TreeDic[R_Key].Left >= -TreeDic[R_Key].Width && TreeDic[R_Key].Left <= TreeDic[R_Key].Width + this.Width
                        && TreeDic[R_Key].Top + TreeDic[R_Key].Height >= 0 && TreeDic[R_Key].Top <= this.Height
                        )
                    {
                        YLabel[lblVTF_Cnt].Text = TreeDic[R_Key].KeyName;
                        YLabel[lblVTF_Cnt].Height = TreeDic[R_Key].Height;
                        YLabel[lblVTF_Cnt].Width = TreeDic[R_Key].Width;
                        YLabel[lblVTF_Cnt].Top = TreeDic[R_Key].Top;
                        YLabel[lblVTF_Cnt].Left = TreeDic[R_Key].Left;
                        YLabel[lblVTF_Cnt].BackColor = TreeDic[R_Key].BackColor;
                        YLabel[lblVTF_Cnt].Visible = true;
                        YLabel[lblVTF_Cnt].BorderStyle = BorderStyle.FixedSingle;
                        YLabel[lblVTF_Cnt].TextAlign = lblY.TextAlign;
                        YLabel[lblVTF_Cnt].Tag = t_key.ToString();

                        lblVTF_Cnt++;
                        TreeDic[R_Key].VisibleTF = true;

                    }
                }
                
            }

            if (lblVTF_Cnt > 0)
            {
                for (int fi_cnt = lblVTF_Cnt ; fi_cnt <= 400; fi_cnt++)
                {
                    YLabel[fi_cnt].Visible = false;
                    YLabel[fi_cnt].Tag = "";                    
                }
            }

            Drow_Picture_Minus_Plus(lblVTF_Cnt);
        }


        private void Drow_Tree_Lbl(int Pic)
        {
            string R_Key = ""; int lblVTF_Cnt = 0;
            int Base_Level = Down_Max_Level;

            RectangleF tt = new RectangleF();
            cls_form_Meth cfm = new cls_form_Meth();

            if (combo_Step.Text.ToString() != "")
                Base_Level = int.Parse(combo_Step.Text.ToString()); 

            foreach (int t_key in Base_TreeDic_Cnt.Keys)
            {
                R_Key = Base_TreeDic_Cnt[t_key];

                if (TreeDic.ContainsKey(R_Key) == true )
                {
                    if (
                        TreeDic[R_Key].Left >= -TreeDic[R_Key].Width && TreeDic[R_Key].Left <= TreeDic[R_Key].Width + this.Width
                        && TreeDic[R_Key].Top + TreeDic[R_Key].Height >= 0 && TreeDic[R_Key].Top <= this.Height
                        && TreeDic[R_Key].Lvl <= Base_Level
                        )
                    {
                       
                       


                        tt.X = 0;
                        
                        //Graphics graphic = YPicture[lblVTF_Cnt].CreateGraphics();
                        

                        YPicture[lblVTF_Cnt].Height = TreeDic[R_Key].Height;
                        YPicture[lblVTF_Cnt].Width = TreeDic[R_Key].Width;
                        YPicture[lblVTF_Cnt].Top = TreeDic[R_Key].Top;
                        YPicture[lblVTF_Cnt].Left = TreeDic[R_Key].Left;
                        YPicture[lblVTF_Cnt].BackColor = TreeDic[R_Key].BackColor;
                        YPicture[lblVTF_Cnt].Visible = true;
                        YPicture[lblVTF_Cnt].BorderStyle = BorderStyle.FixedSingle;
                        //YPicture[lblVTF_Cnt].TextAlign = lblY.TextAlign;
                        YPicture[lblVTF_Cnt].Tag = t_key.ToString();

                        if (Mouse_Select_key == TreeDic[R_Key].IDKey && Mouse_Select_key != "")
                            YPicture[lblVTF_Cnt].BackColor = System.Drawing.Color.LightBlue;

                        Bitmap bt;
                        bt = new Bitmap(YPicture[lblVTF_Cnt].Width, YPicture[lblVTF_Cnt].Height);
                        YPicture[lblVTF_Cnt].Image = bt;
                        Graphics graphic = Graphics.FromImage(bt);

                        float t_font = float.Parse(combo_Font.Text.ToString());

                        //픽처박스 안에 들어가는 박스안에 들어가는 내역을 쓴다.
                        graphic.DrawString(TreeDic[R_Key].KeyName , new System.Drawing.Font("돋움", t_font), Brushes.Black, tt);


                        //탈퇴자에 대해서 박스안에 X표를 표시한다.
                        if (TreeDic[R_Key].LeaveCheck == cfm._chang_base_caption_search("탈퇴"))
                        {
                            Pen T_p = new Pen(Color.Black);
                            graphic.DrawLine(T_p, 0, TreeDic[R_Key].Height, TreeDic[R_Key].Width, 0);
                            graphic.DrawLine(T_p, 0, 0, TreeDic[R_Key].Width, TreeDic[R_Key].Height);
                        }
                        



                        //string[] t_Cap;                        
                        //t_Cap = TreeDic[R_Key].KeyName.Split('\n');

                        //Pen T_p = new Pen(Color.Black);                        
                        //Point t_P = new Point (); Point t_P2 = new Point ();

                        //for (int Cnt = 0; Cnt <= t_Cap.Length - 1; Cnt++)
                        //{
                            
                        //    tt.Y = (TreeDic[R_Key].Height / (t_Cap.Length)) * Cnt;                                                        
                        //    graphic.DrawString(t_Cap[Cnt], new System.Drawing.Font("돋움",t_font), Brushes.Black, tt);


                        //    //if (Cnt < t_Cap.Length - 1)
                        //    //{
                        //    //    t_P.X = 0;
                        //    //    t_P.Y = (TreeDic[R_Key].Height / (t_Cap.Length)) * (Cnt + 1);

                        //    //    t_P2.X = TreeDic[R_Key].Width;
                        //    //    t_P2.Y = (TreeDic[R_Key].Height / (t_Cap.Length)) * (Cnt + 1);
                        //    //    graphic.DrawLine(T_p, t_P, t_P2);
                        //    //}
                        //}
                        lblVTF_Cnt++;
                        TreeDic[R_Key].VisibleTF = true;

                    }
                }

            }

            if (lblVTF_Cnt > 0)
            {
                for (int fi_cnt = lblVTF_Cnt; fi_cnt <= 400; fi_cnt++)
                {
                    YPicture[fi_cnt].Visible = false;
                    YPicture[fi_cnt].Tag = "";
                }
            }

            Drow_Picture_Minus_Plus(lblVTF_Cnt);
        }



        private void Drow_Picture_Minus_Plus(int lblVTF_Cnt )
        {
            int Base_Level = 0, TSw = 0  ;
            if (combo_Step.Text.ToString() != "")
            {
                Base_Level = int.Parse(combo_Step.Text.ToString());
                TSw = 1;
            }

            string R_Key = "";  int MinusCnt = 0 ; int PlusCnt = 0 ;

            if (lblVTF_Cnt > 0)
            {
                for (int fi_cnt = 0; fi_cnt <= lblVTF_Cnt; fi_cnt++)
                {

                    if (YPicture[fi_cnt].Tag.ToString() != "")
                    {

                        R_Key = Base_TreeDic_Cnt[int.Parse(YPicture[fi_cnt].Tag.ToString())];

                        if (TreeDic.ContainsKey(R_Key) == true)
                        {
                            if (TreeDic[R_Key].Lvl < Base_Level || TSw == 0)  // 레벨 설정관련해서 새롭게 첨가된 부분 2014_03_15  지정 기준 레벨보다 작으면 이기능이 필요 없으므로
                            {

                                if (TreeDic[R_Key].ChildCount > 0)
                                {
                                    Y_Minus[MinusCnt].Top = TreeDic[R_Key].Top + TreeDic[R_Key].Height - 2;
                                    Y_Minus[MinusCnt].Left = TreeDic[R_Key].Left + (TreeDic[R_Key].Width / 2) + (Y_Minus[MinusCnt].Width / 2) - 9;
                                    Y_Minus[MinusCnt].Tag = YPicture[fi_cnt].Tag;
                                    Y_Minus[MinusCnt].Visible = true;
                                    MinusCnt++;
                                }
                                else
                                {
                                    if (TreeDic[R_Key].ExpensionTF == false)
                                    {
                                        Y_Plus[PlusCnt].Top = TreeDic[R_Key].Top + TreeDic[R_Key].Height - 2;
                                        Y_Plus[PlusCnt].Left = TreeDic[R_Key].Left + (TreeDic[R_Key].Width / 2) + (Y_Plus[PlusCnt].Width / 2) - 9;
                                        Y_Plus[PlusCnt].Tag = YPicture[fi_cnt].Tag;
                                        Y_Plus[PlusCnt].Visible = true;
                                        PlusCnt++;
                                    }
                                }
                            }

                        }

                    }
                }
            }

            for (int fi_cnt = MinusCnt; fi_cnt <= 400; fi_cnt++)
            {
                Y_Minus[fi_cnt].Visible = false;
                Y_Minus[fi_cnt].Tag = "";
            }

            for (int fi_cnt = PlusCnt; fi_cnt <= 400; fi_cnt++)
            {
                Y_Plus[fi_cnt].Visible = false;
                Y_Plus[fi_cnt].Tag = "";
            }
            
        }



        private void Drow_Tree_Line(object sender, PaintEventArgs e)
        {
            foreach (int t_key in LineDic.Keys)
            {
                if ((LineDic[t_key].X2 >= 0 && LineDic[t_key].X2 <= this.Width)
                    || (LineDic[t_key].X1 >= 0 && LineDic[t_key].X1 <= this.Width)
                    || (LineDic[t_key].X1 <= 0 && LineDic[t_key].X2 >= this.Width)
                    )
                {
                    if (LineDic[t_key].Y2 >= 0 && LineDic[t_key].Y1 < this.Height )
                    {
                        Pen myPen = new Pen(System.Drawing.Color.Black);
                        Graphics PanelGraphics = panel2.CreateGraphics();
                        PanelGraphics.DrawLine(myPen, LineDic[t_key].X1, LineDic[t_key].Y1, LineDic[t_key].X2, LineDic[t_key].Y2);
                        myPen.Dispose();
                        PanelGraphics.Dispose();
                    }
                }
            }
        }




        private void Drow_Tree_Scroll(object sender, PaintEventArgs e)
        {
            string R_Key = ""; Boolean vTF; Boolean hTF;
            int maxLeft = 0; int minTop  = 0;
            vSC.Top = 0; vSC.Height = panel2.Height; vSC.Left = panel2.Width - vSC.Width ;
            hSC.Left = 0; hSC.Top = panel2.Height - hSC.Height; hSC.Width = panel2.Width - vSC.Width;

            if (TreeDic_Cnt.Count > 0)
            {               
                foreach (int t_key in TreeDic_Cnt.Keys)
                {
                    R_Key = TreeDic_Cnt[t_key];
                    if (maxLeft < TreeDic[R_Key].BaseLeft )
                        maxLeft = TreeDic[R_Key].BaseLeft + TreeDic[R_Key].Width;

                    if (minTop < TreeDic[R_Key].BaseTop)
                        minTop = TreeDic[R_Key].BaseTop + TreeDic[R_Key].Height;
                }

                vTF = vSC.Visible;
                vSC.Visible = false;
                if (panel2.Height < minTop)
                {
                    vSC.Visible = true;
                    vSC.BringToFront();
                    vSC.Maximum  = minTop - panel2.Height + TreeDic[TreeDic_Cnt[1]].Height ;
                }

                


                hTF = hSC.Visible;
                hSC.Visible = false;
                if (panel2.Width < maxLeft)
                {
                    hSC.Visible = true;
                    hSC.BringToFront();
                    hSC.Maximum = maxLeft - panel2.Width + TreeDic[TreeDic_Cnt[1]].Width  ;
                }


                //if (vTF == true && vSC.Visible == false)
                //    foreach (int t_key in TreeDic_Cnt.Keys)
                //    {
                //        TreeDic[R_Key].Top = TreeDic[TreeDic_Cnt[t_key]].BaseTop;
                //    }
                //if (hTF == true && hSC.Visible == false)
                //    foreach (int t_key in TreeDic_Cnt.Keys)
                //    {
                //        TreeDic[R_Key].Left = TreeDic[TreeDic_Cnt[t_key]].BaseLeft;
                //    }


                //Drow_Tree_Lbl();
                Drow_Tree_Lbl(1);
            }

            

        }

       





        public bool Tree_Mem_Get(string Mbid, int Mbid2, string SearchDate, int Line_User_V_TF, string SellCode, string SellDate1, string SellDate2, string SellDate4, string SellDate5)
        {
            int Sham_TF = 0;
            if (chk_Sham.Checked == true) Sham_TF = 1;



            if (TreeDic != null)
                TreeDic.Clear();
            if (LineDic != null)
                LineDic.Clear();

            if (Base_TreeDic != null)
                Base_TreeDic.Clear();

            if (TreeDic_Cnt != null)
                TreeDic_Cnt.Clear();

            if (Base_TreeDic_Cnt != null)
                Base_TreeDic_Cnt.Clear();

            txtDownCnt.Text = ""; txtDownCnt_2.Text = "";

            string G_SellCode = ""; string G_SellDate1 = ""; string G_SellDate2 = ""; string G_SellDate4 = ""; string G_SellDate5 = "";
            string Tsql, Search_Lvl = "";

            if (combo_Step.Text != "")
                Search_Lvl = combo_Step.Text;
            else
                Search_Lvl = "0";


            combo_Step.Items.Clear();


            G_SellCode = SellCode;
            if (G_SellCode == "")
                G_SellCode = "전체";

            G_SellDate1 = SellDate1; G_SellDate2 = SellDate2;
            if (G_SellDate1 == "")
                G_SellDate1 = "19900101";

            if (G_SellDate2 == "")
                G_SellDate2 = "30001201";

            G_SellDate4 = SellDate4; G_SellDate5 = SellDate5;
            if (G_SellDate4 == "")
                G_SellDate4 = "19900101";

            if (G_SellDate5 == "")
                G_SellDate5 = "30001201";


            if (SearchDate == "")
                SearchDate = cls_User.gid_date_time;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            //Tsql = "Exec Usp_Mem_Group_Save_TT '" + Mbid + "'," + Mbid2 + ",'" + SearchDate + "'," + Line_User_V_TF;
            Tsql = "Exec Usp_Mem_Group_Save_TT_mannasync '" + Mbid + "'," + Mbid2 + ",'" + SearchDate + "'," + Line_User_V_TF;
            Tsql = Tsql + ",'" + G_SellCode + "','" + G_SellDate1 + "','" + G_SellDate2 + "','" + G_SellDate4 + "','" + G_SellDate5 + "'," + Search_Lvl;

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Tbl_M_Tree", ds) == false) return false;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)
                return false;

            string base_db_name = "Tbl_M_Tree";
            
            Dictionary<string, cls_Mem_Tree> T_TreeDic = new Dictionary<string, cls_Mem_Tree>();
            Dictionary<string, cls_Mem_Tree> T_TreeDic2 = new Dictionary<string, cls_Mem_Tree>();
            string TMbid = ""; int TMbid2 = 0; double T_pr = 0;  string Cpno = ""; int Lvl = 0  ;
            string m_Lvl = "";
            cls_form_Meth cm = new cls_form_Meth();
            
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cls_Mem_Tree t_Mem_Treel = new cls_Mem_Tree();
                //cls_Mem_Tree t_Mem_Treel2 = new cls_Mem_Tree();

                TMbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                TMbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                t_Mem_Treel.FontSizes  = 8;
                t_Mem_Treel.ChildCount = 0 ;

                t_Mem_Treel.CpNumber = "";
                Cpno = encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt]["CpNumber"].ToString());
                if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
                    if (Cpno.Length  == 13)
                        t_Mem_Treel.CpNumber =  Cpno.Substring(0,6) + '-' +  Cpno.Substring(6,7) ;                    
                else
                    if (Cpno.Length  == 13)
                        t_Mem_Treel.CpNumber =  Cpno.Substring(0,6) + '-' +  "*******" ;                    


                t_Mem_Treel.RegDate = ds.Tables[base_db_name].Rows[fi_cnt]["RegDate"].ToString();
                t_Mem_Treel.MbidName = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString() + "(" + ds.Tables[base_db_name].Rows[fi_cnt]["TSave_Cur"].ToString() + ")";

                if (int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Leave"].ToString()) == 0)
                {

                    //string LeaveDate = ds.Tables[base_db_name].Rows[fi_cnt]["LeaveDate"].ToString();
                    //string PayDate = LeaveDate.Substring(0, 4) + '-' + LeaveDate.Substring(4, 2) + '-' + LeaveDate.Substring(6, 2);
                    //DateTime TodayDate = new DateTime();
                    //TodayDate = DateTime.Parse(PayDate);
                    //PayDate = TodayDate.AddMonths(6).ToString("yyyy-MM-dd").Replace("-", "").Replace("/", "");

                    //if (int.Parse(cls_User.gid_date_time) > int.Parse(PayDate))
                    //    t_Mem_Treel.MbidName += " (R)";
                    //else
                    //    t_Mem_Treel.MbidName += " (S)";

                    t_Mem_Treel.LeaveCheck = cm._chang_base_caption_search("탈퇴");      
                }

                if (int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Leave"].ToString()) == -1)
                {
                    //t_Mem_Treel.MbidName += " (R)";
                    t_Mem_Treel.LeaveCheck = cm._chang_base_caption_search("탈퇴");
                }
                    


                if (cls_app_static_var.Member_Number_1 > 0)
                {
                    t_Mem_Treel.IDKey = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "-" + ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                    t_Mem_Treel.Nominid = ds.Tables[base_db_name].Rows[fi_cnt]["Nominid"].ToString() + "-" + ds.Tables[base_db_name].Rows[fi_cnt]["Nominid2"].ToString();                    
                }
                else
                {
                    t_Mem_Treel.IDKey = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                    t_Mem_Treel.Nominid =  ds.Tables[base_db_name].Rows[fi_cnt]["Nominid2"].ToString();
                }


                


                if (TMbid == Mbid && TMbid2 == Mbid2)
                    t_Mem_Treel.ParentKey = "";
                else
                {
                    if (cls_app_static_var.Member_Number_1 > 0)
                        t_Mem_Treel.ParentKey = ds.Tables[base_db_name].Rows[fi_cnt]["Saveid"].ToString() + "-" + ds.Tables[base_db_name].Rows[fi_cnt]["Saveid2"].ToString();
                    else
                        t_Mem_Treel.ParentKey =  ds.Tables[base_db_name].Rows[fi_cnt]["Saveid2"].ToString();
                }

                
                t_Mem_Treel.NominName = ds.Tables[base_db_name].Rows[fi_cnt]["N_Name"].ToString();
                t_Mem_Treel.BusName = ds.Tables[base_db_name].Rows[fi_cnt]["Center_Name"].ToString();
                t_Mem_Treel.Cur = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TSave_Cur"].ToString());
                t_Mem_Treel.TDownPV = "0";
                t_Mem_Treel.f_TDownPV = 0;

                t_Mem_Treel.TDownBV = "0";
                t_Mem_Treel.f_TDownBV = 0;

                t_Mem_Treel.ShamPV = "0";

                T_pr = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TTSellPV01"].ToString());
                t_Mem_Treel.TotalPV = string.Format(cls_app_static_var.str_Currency_Type, T_pr);
                t_Mem_Treel.f_TotalPV = T_pr;
               

                T_pr = Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TTSellPV02"].ToString());
                t_Mem_Treel.Down_Sobi_PV = string.Format(cls_app_static_var.str_Currency_Type, T_pr);

                T_pr = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TTSellPV03"].ToString());
                t_Mem_Treel.TotalBV = string.Format(cls_app_static_var.str_Currency_Type, T_pr);
                t_Mem_Treel.f_TotalBV = T_pr;










                //string Sell_Mem_TF = ds.Tables[base_db_name].Rows[fi_cnt]["TSell_Mem_TF"].ToString();
                //if (Sell_Mem_TF == "0") t_Mem_Treel.Sell_Mem_TF = cm._chang_base_caption_search("판매원");
                //if (Sell_Mem_TF == "1") t_Mem_Treel.Sell_Mem_TF = cm._chang_base_caption_search("소비자");
                //t_Mem_Treel.Grade_P = ds.Tables[base_db_name].Rows[fi_cnt]["C_Grade_P"].ToString();
                //t_Mem_Treel.ClassP_Date = ds.Tables[base_db_name].Rows[fi_cnt]["TClassP_Date"].ToString();


                t_Mem_Treel.SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["TTSellDate01"].ToString();


                t_Mem_Treel.SellDate_2 = ds.Tables[base_db_name].Rows[fi_cnt]["TTSellDate02"].ToString();

                //t_Mem_Treel.LeaveCheck = cm._chang_base_caption_search("활동");
                t_Mem_Treel.BackColor = System.Drawing.Color.White;
                
                t_Mem_Treel.KeyName = t_Mem_Treel.IDKey + "\n" + t_Mem_Treel.MbidName + "\n" + t_Mem_Treel.RegDate;

                               
                


                t_Mem_Treel.Grade1 = ds.Tables[base_db_name].Rows[fi_cnt]["C_Grade"].ToString();
                //t_Mem_Treel.Grade2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TTBeforeGrade2"].ToString());
                t_Mem_Treel.Grade_Cur = ds.Tables[base_db_name].Rows[fi_cnt]["CurGradeName"].ToString();
                t_Mem_Treel.Grade_Max = ds.Tables[base_db_name].Rows[fi_cnt]["Max_CurGradeName"].ToString();

                t_Mem_Treel.SelfNumber = fi_cnt; 
                t_Mem_Treel.VisibleTF  =true ;
                t_Mem_Treel.VisibleTF2  =true ;
                t_Mem_Treel.ExpensionTF =true ;


                

                Lvl = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Lvl"].ToString());
                t_Mem_Treel.Lvl = Lvl;

                //if (Down_Max_Level < Lvl)
                //    Down_Max_Level = Lvl; 
                m_Lvl = ds.Tables[base_db_name].Rows[fi_cnt]["m_Lvl"].ToString(); 
                Down_Max_Level = int.Parse (m_Lvl) ; 

                t_Mem_Treel.ChildNumber = new Dictionary<int, cls_Mem_Tree>();
                t_Mem_Treel.UpLineKey = "";
                

                T_TreeDic[t_Mem_Treel.IDKey] = t_Mem_Treel;
                T_TreeDic2[t_Mem_Treel.IDKey] = t_Mem_Treel;

                 if (TMbid.ToUpper()  != Mbid.ToUpper () || TMbid2 != Mbid2)
                     InputNextKey(ref T_TreeDic, t_Mem_Treel.ParentKey, fi_cnt, t_Mem_Treel.IDKey, Lvl, ref T_TreeDic2);

                 TreeDic_Cnt[fi_cnt] = t_Mem_Treel.IDKey;
                 Base_TreeDic_Cnt[fi_cnt] = t_Mem_Treel.IDKey;


                 
            }

                
            TreeDic = T_TreeDic;
            Base_TreeDic = T_TreeDic2;

            txtDownCnt.Text = (TreeDic.Count - 1).ToString();
            txtDownCnt_2.Text = Down_Max_Level.ToString();
            
            
            int Cnt = 1 ;
            while (Cnt <= Down_Max_Level)
            {
                combo_Step.Items.Add(Cnt.ToString());
                Cnt++;
            }

            if (Search_Lvl != "0")
                combo_Step.Text = Search_Lvl ;    
            else
                combo_Step.Text = Down_Max_Level.ToString () ;

            return true;
        }


        public bool Tree_Mem_Get_Nom(string Mbid, int Mbid2, string SearchDate, int Line_User_V_TF, string SellCode, string SellDate1, string SellDate2, string SellDate4, string SellDate5)
        {

            int Sham_TF = 0;
            if (chk_Sham.Checked == true) Sham_TF = 1;


            if (TreeDic != null)
                TreeDic.Clear();
            if (LineDic != null)
                LineDic.Clear();

            if (Base_TreeDic != null)
                Base_TreeDic.Clear();

            if (TreeDic_Cnt != null)
                TreeDic_Cnt.Clear();

            if (Base_TreeDic_Cnt != null)
                Base_TreeDic_Cnt.Clear();

            txtDownCnt.Text = ""; txtDownCnt_2.Text = "";

            string G_SellCode = ""; string G_SellDate1 = ""; string G_SellDate2 = ""; string G_SellDate4 = ""; string G_SellDate5= "";
            string Tsql, Search_Lvl = "",m_Lvl = "";
            

            if (combo_Step.Text != "")
                Search_Lvl = combo_Step.Text;
            else
                Search_Lvl = "0";


            combo_Step.Items.Clear();

            G_SellCode = SellCode;
            if (G_SellCode == "")
                G_SellCode = "전체";

            G_SellDate1 = SellDate1; G_SellDate2 = SellDate2; G_SellDate4 = SellDate4; G_SellDate5 = SellDate5;
            if (G_SellDate1 == "")
                G_SellDate1 = "19900101";

            if (G_SellDate2 == "")
                G_SellDate2 = "30001201";

            if (G_SellDate4 == "")
                G_SellDate4 = "19900101";

            if (G_SellDate5 == "")
                G_SellDate5 = "30001201";
            if (SearchDate == "")

                SearchDate = cls_User.gid_date_time;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            Tsql = "Exec Usp_Mem_Group_Nomin_TT_mannasync '" + Mbid + "'," + Mbid2 + ",'" + SearchDate + "'," + Line_User_V_TF;
            Tsql = Tsql + ",'" + G_SellCode + "','" + G_SellDate1 + "','" + G_SellDate2 + "','" + G_SellDate4 + "','" + G_SellDate5 + "'," + Search_Lvl;

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Tbl_M_Tree", ds) == false) return false;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)
                return false;

            string base_db_name = "Tbl_M_Tree";

            Dictionary<string, cls_Mem_Tree> T_TreeDic = new Dictionary<string, cls_Mem_Tree>();
            Dictionary<string, cls_Mem_Tree> T_TreeDic2 = new Dictionary<string, cls_Mem_Tree>();
            string TMbid = ""; int TMbid2 = 0; double T_pr = 0; string Cpno = ""; int Lvl = 0;

            cls_form_Meth cm = new cls_form_Meth();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cls_Mem_Tree t_Mem_Treel = new cls_Mem_Tree();
                //cls_Mem_Tree t_Mem_Treel2 = new cls_Mem_Tree();

                TMbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                TMbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                t_Mem_Treel.FontSizes = 8;
                t_Mem_Treel.ChildCount = 0;

                t_Mem_Treel.CpNumber = "";
                Cpno = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["CpNumber"].ToString());
                if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
                    if (Cpno.Length == 13)
                        t_Mem_Treel.CpNumber = Cpno.Substring(0, 6) + '-' + Cpno.Substring(6, 7);
                    else
                        if (Cpno.Length == 13)
                            t_Mem_Treel.CpNumber = Cpno.Substring(0, 6) + '-' + "*******";


                t_Mem_Treel.RegDate = ds.Tables[base_db_name].Rows[fi_cnt]["RegDate"].ToString();
                t_Mem_Treel.MbidName = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString() + "(" + ds.Tables[base_db_name].Rows[fi_cnt]["TNomin_Cur"].ToString() + ")";

                if (int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Leave"].ToString()) == 0)
                {
                    //string LeaveDate = ds.Tables[base_db_name].Rows[fi_cnt]["LeaveDate"].ToString();
                    //string PayDate = LeaveDate.Substring(0, 4) + '-' + LeaveDate.Substring(4, 2) + '-' + LeaveDate.Substring(6, 2);
                    //DateTime TodayDate = new DateTime();
                    //TodayDate = DateTime.Parse(PayDate);
                    //PayDate = TodayDate.AddMonths(6).ToString("yyyy-MM-dd").Replace("-", "").Replace("/", "");
                                        
                    //if (int.Parse(cls_User.gid_date_time) > int.Parse(PayDate) )
                    //    t_Mem_Treel.MbidName += " (R)";
                    //else
                    //    t_Mem_Treel.MbidName += " (S)";
                    t_Mem_Treel.LeaveCheck = cm._chang_base_caption_search("탈퇴");
                }

                if (int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Leave"].ToString()) == -1)
                {
                  //  t_Mem_Treel.MbidName += " (R)";
                    t_Mem_Treel.LeaveCheck = cm._chang_base_caption_search("탈퇴");
                }
                    


                if (cls_app_static_var.Member_Number_1 > 0)
                {
                    t_Mem_Treel.IDKey = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "-" + ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                    t_Mem_Treel.Nominid = ds.Tables[base_db_name].Rows[fi_cnt]["SAveid"].ToString() + "-" + ds.Tables[base_db_name].Rows[fi_cnt]["SAveid2"].ToString();
                }
                else
                {
                    t_Mem_Treel.IDKey = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                    t_Mem_Treel.Nominid = ds.Tables[base_db_name].Rows[fi_cnt]["SAveid2"].ToString();
                }





                if (TMbid == Mbid && TMbid2 == Mbid2)
                    t_Mem_Treel.ParentKey = "";
                else
                {
                    if (cls_app_static_var.Member_Number_1 > 0)
                        t_Mem_Treel.ParentKey = ds.Tables[base_db_name].Rows[fi_cnt]["Nominid"].ToString() + "-" + ds.Tables[base_db_name].Rows[fi_cnt]["Nominid2"].ToString();
                    else
                        t_Mem_Treel.ParentKey = ds.Tables[base_db_name].Rows[fi_cnt]["Nominid2"].ToString();
                }


                t_Mem_Treel.NominName = ds.Tables[base_db_name].Rows[fi_cnt]["N_Name"].ToString();
                t_Mem_Treel.BusName = ds.Tables[base_db_name].Rows[fi_cnt]["Center_Name"].ToString();
                t_Mem_Treel.Cur = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TNomin_Cur"].ToString());
                t_Mem_Treel.TDownPV = "0";
               t_Mem_Treel.f_TDownPV = 0;

                t_Mem_Treel.TDownBV = "0";
                t_Mem_Treel.f_TDownBV = 0;

                t_Mem_Treel.ShamPV = "0";

                T_pr = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TTSellPV01"].ToString());
                t_Mem_Treel.TotalPV = string.Format(cls_app_static_var.str_Currency_Type, T_pr);
                t_Mem_Treel.f_TotalPV = T_pr;

                T_pr = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TTSellPV02"].ToString());
                //t_Mem_Treel.TotalBV = string.Format(cls_app_static_var.str_Currency_Type, T_pr);
                //t_Mem_Treel.f_TotalBV = T_pr;

                T_pr = Double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TTSellPV02"].ToString());
                t_Mem_Treel.Down_Sobi_PV = string.Format(cls_app_static_var.str_Currency_Type, T_pr);

                T_pr = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TTSellPV03"].ToString());
                t_Mem_Treel.TotalBV = string.Format(cls_app_static_var.str_Currency_Type, T_pr);
                t_Mem_Treel.f_TotalBV = T_pr;

                //string Sell_Mem_TF = ds.Tables[base_db_name].Rows[fi_cnt]["TSell_Mem_TF"].ToString();
                //if (Sell_Mem_TF == "0") t_Mem_Treel.Sell_Mem_TF = cm._chang_base_caption_search("판매원");
                //if (Sell_Mem_TF == "1") t_Mem_Treel.Sell_Mem_TF = cm._chang_base_caption_search("소비자");
                //t_Mem_Treel.Grade_P = ds.Tables[base_db_name].Rows[fi_cnt]["C_Grade_P"].ToString();
                //t_Mem_Treel.ClassP_Date = ds.Tables[base_db_name].Rows[fi_cnt]["TClassP_Date"].ToString();



                t_Mem_Treel.SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["TTSellDate01"].ToString();

                t_Mem_Treel.SellDate_2 = ds.Tables[base_db_name].Rows[fi_cnt]["TTSellDate02"].ToString();

                // t_Mem_Treel.LeaveCheck = cm._chang_base_caption_search("활동");
                t_Mem_Treel.BackColor = System.Drawing.Color.White;

                t_Mem_Treel.KeyName = t_Mem_Treel.IDKey + "\n" + t_Mem_Treel.MbidName + "\n" + t_Mem_Treel.RegDate;

                //if (int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Leave"].ToString()) == 0)
                //    t_Mem_Treel.LeaveCheck = cm._chang_base_caption_search("탈퇴");


                t_Mem_Treel.Grade1 = ds.Tables[base_db_name].Rows[fi_cnt]["C_Grade"].ToString();
                t_Mem_Treel.Grade2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TTBeforeGrade2"].ToString());

                t_Mem_Treel.Grade_Cur = ds.Tables[base_db_name].Rows[fi_cnt]["CurGradeName"].ToString();
                t_Mem_Treel.Grade_Max = ds.Tables[base_db_name].Rows[fi_cnt]["Max_CurGradeName"].ToString();

                t_Mem_Treel.SelfNumber = fi_cnt;
                t_Mem_Treel.VisibleTF = true;
                t_Mem_Treel.VisibleTF2 = true;
                t_Mem_Treel.ExpensionTF = true;


                Lvl = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Lvl"].ToString());
                t_Mem_Treel.Lvl = Lvl;

                //if (Down_Max_Level < Lvl)
                //    Down_Max_Level = Lvl;
                m_Lvl = ds.Tables[base_db_name].Rows[fi_cnt]["m_Lvl"].ToString();
                Down_Max_Level = int.Parse(m_Lvl); 

                t_Mem_Treel.ChildNumber = new Dictionary<int, cls_Mem_Tree>();
                t_Mem_Treel.UpLineKey = "";


                T_TreeDic[t_Mem_Treel.IDKey] = t_Mem_Treel;
                T_TreeDic2[t_Mem_Treel.IDKey] = t_Mem_Treel;

                if (TMbid.ToUpper() != Mbid.ToUpper() || TMbid2 != Mbid2)
                    InputNextKey(ref T_TreeDic, t_Mem_Treel.ParentKey, fi_cnt, t_Mem_Treel.IDKey, Lvl, ref T_TreeDic2);

                TreeDic_Cnt[fi_cnt] = t_Mem_Treel.IDKey;
                Base_TreeDic_Cnt[fi_cnt] = t_Mem_Treel.IDKey;


            }


            TreeDic = T_TreeDic;
            Base_TreeDic = T_TreeDic2;

            txtDownCnt.Text = (TreeDic.Count - 1).ToString();
            txtDownCnt_2.Text = Down_Max_Level.ToString();

            int Cnt = 1;
            while (Cnt <= Down_Max_Level)
            {
                combo_Step.Items.Add(Cnt.ToString());
                Cnt++;
            }

            if (Search_Lvl != "0")
                combo_Step.Text = Search_Lvl;
            else
                combo_Step.Text = Down_Max_Level.ToString();


            return true;
        }


        public bool Tree_Mem_Get_Ui(string Mbid, int Mbid2, string SearchDate, int Line_User_V_TF, string SellCode, string SellDate1, string SellDate2, string SellDate4, string SellDate5)
        {
            if (TreeDic != null)
                TreeDic.Clear();
            if (LineDic != null)
                LineDic.Clear();

            if (Base_TreeDic != null)
                Base_TreeDic.Clear();

            if (TreeDic_Cnt != null)
                TreeDic_Cnt.Clear();

            if (Base_TreeDic_Cnt != null)
                Base_TreeDic_Cnt.Clear();

            txtDownCnt.Text = ""; txtDownCnt_2.Text = "";

            string G_SellCode = ""; string G_SellDate1 = ""; string G_SellDate2 = ""; string G_SellDate4 = ""; string G_SellDate5 = "";
            string Tsql;

            G_SellCode = SellCode;
            if (G_SellCode == "")
                G_SellCode = "전체";

            G_SellDate1 = SellDate1; G_SellDate2 = SellDate2; G_SellDate4 = SellDate4; G_SellDate5 = SellDate5;
            if (G_SellDate1 == "")
                G_SellDate1 = "19900101";

            if (G_SellDate2 == "")
                G_SellDate2 = "30001201";
            if (G_SellDate4 == "")
                G_SellDate4 = "19900101";

            if (G_SellDate5 == "")
                G_SellDate5 = "30001201";
            if (SearchDate == "")
                SearchDate = cls_User.gid_date_time;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            //Tsql = "Exec Usp_Mem_Group_Ui_Save_TT '" + Mbid + "'," + Mbid2 + ",'" + SearchDate + "'," + Line_User_V_TF + ",'" + G_SellCode + "','" + G_SellDate1 + "','" + G_SellDate2 + "'";
            Tsql = "Exec Usp_Mem_Group_Ui_Save_TT '" + Mbid + "'," + Mbid2 + ",'" + SearchDate + "',0,'" + G_SellCode + "','" + G_SellDate1 + "','" + G_SellDate2 + "'";
            

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Tbl_M_Tree", ds) == false) return false;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)
                return false;

            string base_db_name = "Tbl_M_Tree";

            Dictionary<string, cls_Mem_Tree> T_TreeDic = new Dictionary<string, cls_Mem_Tree>();
            Dictionary<string, cls_Mem_Tree> T_TreeDic2 = new Dictionary<string, cls_Mem_Tree>();
            string TMbid = ""; int TMbid2 = 0; double T_pr = 0; string Cpno = ""; int Lvl = 0;

            cls_form_Meth cm = new cls_form_Meth();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cls_Mem_Tree t_Mem_Treel = new cls_Mem_Tree();
                //cls_Mem_Tree t_Mem_Treel2 = new cls_Mem_Tree();

                TMbid = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString();
                TMbid2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString());
                t_Mem_Treel.FontSizes = 8;
                t_Mem_Treel.ChildCount = 0;

                t_Mem_Treel.CpNumber = "";
                Cpno = encrypter.Decrypt(ds.Tables[base_db_name].Rows[fi_cnt]["CpNumber"].ToString());
                if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
                    if (Cpno.Length == 13)
                        t_Mem_Treel.CpNumber = Cpno.Substring(0, 6) + '-' + Cpno.Substring(6, 7);
                    else
                        if (Cpno.Length == 13)
                            t_Mem_Treel.CpNumber = Cpno.Substring(0, 6) + '-' + "*******";


                t_Mem_Treel.RegDate = ds.Tables[base_db_name].Rows[fi_cnt]["RegDate"].ToString();
                t_Mem_Treel.MbidName = ds.Tables[base_db_name].Rows[fi_cnt]["M_Name"].ToString() + "(" + ds.Tables[base_db_name].Rows[fi_cnt]["TNomin_Cur"].ToString() + ")";


                if (cls_app_static_var.Member_Number_1 > 0)
                {
                    t_Mem_Treel.IDKey = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid"].ToString() + "-" + ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                    t_Mem_Treel.Nominid = ds.Tables[base_db_name].Rows[fi_cnt]["SAveid"].ToString() + "-" + ds.Tables[base_db_name].Rows[fi_cnt]["SAveid2"].ToString();
                }
                else
                {
                    t_Mem_Treel.IDKey = ds.Tables[base_db_name].Rows[fi_cnt]["Mbid2"].ToString();
                    t_Mem_Treel.Nominid = ds.Tables[base_db_name].Rows[fi_cnt]["SAveid2"].ToString();
                }





                if (TMbid == Mbid && TMbid2 == Mbid2)
                    t_Mem_Treel.ParentKey = "";
                else
                {
                    if (cls_app_static_var.Member_Number_1 > 0)
                        t_Mem_Treel.ParentKey = ds.Tables[base_db_name].Rows[fi_cnt]["Nominid"].ToString() + "-" + ds.Tables[base_db_name].Rows[fi_cnt]["Nominid2"].ToString();
                    else
                        t_Mem_Treel.ParentKey = ds.Tables[base_db_name].Rows[fi_cnt]["Nominid2"].ToString();
                }


                t_Mem_Treel.NominName = ds.Tables[base_db_name].Rows[fi_cnt]["N_Name"].ToString();
                t_Mem_Treel.BusName = ds.Tables[base_db_name].Rows[fi_cnt]["Center_Name"].ToString();
                t_Mem_Treel.Cur = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TNomin_Cur"].ToString());
                t_Mem_Treel.TDownPV = "0";
                 t_Mem_Treel.f_TDownPV = 0;

                t_Mem_Treel.TDownBV = "0";
                 t_Mem_Treel.f_TDownBV = 0;

                t_Mem_Treel.ShamPV = "0";

                T_pr = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TTSellPV01"].ToString());
                t_Mem_Treel.TotalPV = string.Format(cls_app_static_var.str_Currency_Type, T_pr);
                t_Mem_Treel.f_TotalPV = T_pr;

                T_pr = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TTSellPV02"].ToString());
                //t_Mem_Treel.TotalBV = string.Format(cls_app_static_var.str_Currency_Type, T_pr);
                //t_Mem_Treel.f_TotalBV = T_pr;

                T_pr = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TTSellPV03"].ToString());
                t_Mem_Treel.TotalBV = string.Format(cls_app_static_var.str_Currency_Type, T_pr);
                t_Mem_Treel.f_TotalBV = T_pr;



                t_Mem_Treel.SellDate = ds.Tables[base_db_name].Rows[fi_cnt]["TTSellDate01"].ToString();

                t_Mem_Treel.SellDate_2 = ds.Tables[base_db_name].Rows[fi_cnt]["TTSellDate02"].ToString();

                t_Mem_Treel.LeaveCheck = cm._chang_base_caption_search("활동");
                t_Mem_Treel.BackColor = System.Drawing.Color.White;

                t_Mem_Treel.KeyName = t_Mem_Treel.IDKey + "\n" + t_Mem_Treel.MbidName + "\n" + t_Mem_Treel.RegDate;

                if (int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Leave"].ToString()) == 0)
                    t_Mem_Treel.LeaveCheck = cm._chang_base_caption_search("탈퇴");


                t_Mem_Treel.Grade1 = ds.Tables[base_db_name].Rows[fi_cnt]["C_Grade"].ToString();
                t_Mem_Treel.Grade2 = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TTBeforeGrade2"].ToString());

                t_Mem_Treel.Grade_Cur = ds.Tables[base_db_name].Rows[fi_cnt]["CurGradeName"].ToString();
                t_Mem_Treel.Grade_Max = ds.Tables[base_db_name].Rows[fi_cnt]["Max_CurGradeName"].ToString();

                t_Mem_Treel.SelfNumber = fi_cnt;
                t_Mem_Treel.VisibleTF = true;
                t_Mem_Treel.VisibleTF2 = true;
                t_Mem_Treel.ExpensionTF = true;


                Lvl = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Lvl"].ToString());
                t_Mem_Treel.Lvl = Lvl;

                if (Down_Max_Level < Lvl)
                    Down_Max_Level = Lvl;

                t_Mem_Treel.ChildNumber = new Dictionary<int, cls_Mem_Tree>();
                t_Mem_Treel.UpLineKey = "";


                T_TreeDic[t_Mem_Treel.IDKey] = t_Mem_Treel;
                T_TreeDic2[t_Mem_Treel.IDKey] = t_Mem_Treel;

                if (TMbid.ToUpper() != Mbid.ToUpper() || TMbid2 != Mbid2)
                    InputNextKey(ref T_TreeDic, t_Mem_Treel.ParentKey, fi_cnt, t_Mem_Treel.IDKey, Lvl, ref T_TreeDic2);

                TreeDic_Cnt[fi_cnt] = t_Mem_Treel.IDKey;
                Base_TreeDic_Cnt[fi_cnt] = t_Mem_Treel.IDKey;


            }


            TreeDic = T_TreeDic;
            Base_TreeDic = T_TreeDic2;

            txtDownCnt.Text = (TreeDic.Count - 1).ToString();
            txtDownCnt_2.Text = Down_Max_Level.ToString();


            return true;
        }




        private void InputNextKey(ref Dictionary<string, cls_Mem_Tree> T_TreeDic, string ParentKey, int fi_cnt, string IDKey, int Lvl, ref Dictionary<string, cls_Mem_Tree> T_TreeDic2)
        {
            T_TreeDic[ParentKey].ChildCount++;
            T_TreeDic[ParentKey].NextDataNum = fi_cnt;

            T_TreeDic[ParentKey].ChildNumber[T_TreeDic[ParentKey].ChildCount] = T_TreeDic[IDKey];

            
            T_TreeDic[IDKey].ParentClass = T_TreeDic[ParentKey];
            T_TreeDic[IDKey].UpLineKey = T_TreeDic[ParentKey].UpLineKey + T_TreeDic[ParentKey].IDKey;

            //int forCnt = 0;

            //foreach (string t_key in T_TreeDic.Keys)
            //{
            //    if (T_TreeDic[t_key].IDKey == ParentKey)
            //    {
            //        T_TreeDic[t_key].ChildCount++;
            //        T_TreeDic[t_key].NextDataNum = fi_cnt;

            //        T_TreeDic[t_key].ChildNumber[T_TreeDic[t_key].ChildCount] = T_TreeDic[IDKey];

            //        T_TreeDic[IDKey].ParentClass = T_TreeDic[t_key];
            //        T_TreeDic[IDKey].UpLineKey = T_TreeDic[IDKey].UpLineKey + T_TreeDic[t_key].IDKey;
            //    }

            //    forCnt++;
                
            //}
        }

        private void vSC_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.OldValue.ToString() != e.NewValue.ToString())
            {
                int GetXValue =e.NewValue- e.OldValue ;
                int CucTop = 0;
                string R_Key = "";
                
                foreach (int t_key in TreeDic_Cnt.Keys)
                {
                    R_Key = TreeDic_Cnt[t_key];

                    CucTop = TreeDic[R_Key].BaseTop - e.NewValue;
                    TreeDic[R_Key].Top = CucTop;
                    //TreeDic[R_Key].VisibleTF = false;
                }

                foreach (int t_key in LineDic.Keys)
                {
                    LineDic[t_key].Y1 = LineDic[t_key].BY1 - e.NewValue;
                    LineDic[t_key].Y2 = LineDic[t_key].BY2 - e.NewValue;
                }
                //panel2.Visible = false;
                //Drow_Tree_Lbl();
                Drow_Tree_Lbl(1);
                panel2.Refresh();
                panel2.Focus();
                //panel2.Visible = true;
            }

        }


        private void vSC_ValueChanged(object sender, EventArgs e)
        {
            int CucTop = 0;
            string R_Key = ""; int oldvalue = 0; int TSW = 0;

            oldvalue = vScroll_Be_Value - vSC.Value;

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                R_Key = TreeDic_Cnt[t_key];
                CucTop = TreeDic[R_Key].BaseTop + (oldvalue);
                ///TreeDic[R_Key].Top = CucTop;                    
                TreeDic[R_Key].Top = TreeDic[R_Key].Top + (oldvalue);

                if (TSW == 3)
                    TreeDic[R_Key].Top = TreeDic[R_Key].BaseTop;

            }

            foreach (int t_key in LineDic.Keys)
            {
                //LineDic[t_key].Y1 = LineDic[t_key].BY1 + (oldvalue);
                //LineDic[t_key].Y2 = LineDic[t_key].BY2 + (oldvalue);

                LineDic[t_key].Y1 = LineDic[t_key].Y1 + (oldvalue);
                LineDic[t_key].Y2 = LineDic[t_key].Y2 + (oldvalue);

                if (TSW == 3)
                {
                    LineDic[t_key].Y1 = LineDic[t_key].BY1;
                    LineDic[t_key].Y2 = LineDic[t_key].BY2;
                }
            }

            Drow_Tree_Lbl(1);
            panel2.Refresh();

            vScroll_Be_Value = vSC.Value;
        }



        
        private void hSC_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.OldValue.ToString() != e.NewValue.ToString())
            {
                int GetXValue = e.NewValue - e.OldValue;
                int CucLeft = 0;
                string R_Key = "";

                foreach (int t_key in TreeDic_Cnt.Keys)
                {
                    R_Key = TreeDic_Cnt[t_key];

                    CucLeft = TreeDic[R_Key].BaseLeft - e.NewValue;
                    TreeDic[R_Key].Left = CucLeft;
                    TreeDic[R_Key].VisibleTF = false;
                }

                foreach (int t_key in LineDic.Keys)
                {
                    LineDic[t_key].X1 = LineDic[t_key].BX1 - e.NewValue;
                    LineDic[t_key].X2 = LineDic[t_key].BX2 - e.NewValue;
                }
                panel2.Visible = false;
                //Drow_Tree_Lbl();
                Drow_Tree_Lbl(1);
                panel2.Visible = true;
            }

        }


        // X라벨을 클릭햇을 경우에... 연결된 폼을 없앤다.
        public void Treelbl_ClickHandler(Object sender, System.EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs ee = (System.Windows.Forms.MouseEventArgs)e;
            
            Label t_lb = (Label)sender;
            int keyCnt = int.Parse(t_lb.Tag.ToString());

            for (int fi_cnt = 0; fi_cnt <= 400; fi_cnt++)
            {
                if (YLabel[fi_cnt].Tag.ToString() == "" )
                    break ;
                else
                {
                    YLabel[fi_cnt].BackColor =  TreeDic[TreeDic_Cnt[keyCnt]].BackColor  ;
                }
            }
            t_lb.BackColor = System.Drawing.Color.LightSalmon;           
        }



        private void Minus_ClickHandler(Object sender, System.EventArgs e)
        {
            //System.Windows.Forms.MouseEventArgs ee = (System.Windows.Forms.MouseEventArgs)e;

            PictureBox t_Pb = (PictureBox)sender;

            int keyCnt = int.Parse(t_Pb.Tag.ToString());

            Down_Member_Visible(keyCnt);

            DrawData_Label_Position_Top(); //회원 클래스 들의 Top정보를 계산해서 넣는다.
            Label_Drow_Left();    //회원 클래스 들의 Left 정보를 계산해서 넣는다.
            Up_Left_Total_Move(); //회원 클래스 들을 자식들에 맞춰서 left를 조정한다.
            Line_Drow_Position();  //회원 클래스들을 기준으로 해서 자식들과 연결하는 선의 위치 클래스들을 생성 위치값을 구한다.
            MakeTreeLebel_Save(1);

            panel2.Visible = false;
            //Drow_Tree_Lbl();
            Drow_Tree_Lbl(1);
            PaintEventArgs d = null;

            object tt = null;
            Drow_Tree_Line(tt, d);
            panel2.Visible = true;
        }


        private void Plus_ClickHandler(Object sender, System.EventArgs e)
        {
            //System.Windows.Forms.MouseEventArgs ee = (System.Windows.Forms.MouseEventArgs)e;

            PictureBox t_Pb = (PictureBox)sender;

            int keyCnt = int.Parse(t_Pb.Tag.ToString());

            Down_Member_Visible(keyCnt,1);

            DrawData_Label_Position_Top(); //회원 클래스 들의 Top정보를 계산해서 넣는다.
            Label_Drow_Left();    //회원 클래스 들의 Left 정보를 계산해서 넣는다.
            Up_Left_Total_Move(); //회원 클래스 들을 자식들에 맞춰서 left를 조정한다.
            Line_Drow_Position();  //회원 클래스들을 기준으로 해서 자식들과 연결하는 선의 위치 클래스들을 생성 위치값을 구한다.
            MakeTreeLebel_Save(1);

            panel2.Visible = false;
            //Drow_Tree_Lbl();
            Drow_Tree_Lbl(1);
            PaintEventArgs d = null;

            object tt = null;
            Drow_Tree_Line(tt, d);
            panel2.Visible = true;
        }



        private void Down_Member_Visible(int BaseIndex, int ExtendTF = 0)
        {
            string Basekey = Base_TreeDic[TreeDic_Cnt[BaseIndex]].IDKey;
            string R_Key = "";

            if (ExtendTF == 0)
            {
                Base_TreeDic[TreeDic_Cnt[BaseIndex]].ExpensionTF = false;

                foreach (int t_key in Base_TreeDic_Cnt.Keys)
                {
                    R_Key = Base_TreeDic_Cnt[t_key];
                    if (Base_TreeDic[R_Key].UpLineKey.IndexOf(Basekey) >= 0)
                        Base_TreeDic[R_Key].VisibleTF = false;
                    else
                        Base_TreeDic[R_Key].VisibleTF =true ;
                }
            }
            else
            {
                Base_TreeDic[TreeDic_Cnt[BaseIndex]].ExpensionTF = true;

                foreach (int t_key in Base_TreeDic_Cnt.Keys)
                {

                    R_Key = Base_TreeDic_Cnt[t_key];
                    if (Base_TreeDic[R_Key].UpLineKey.IndexOf(Basekey) >= 0)
                        Base_TreeDic[R_Key].VisibleTF = true;
                    
                }
            }

            Dictionary<int, string> T_TreeDic_Cnt = new Dictionary<int, string>();
            TreeDic.Clear();
            string ParentKey = ""; int Cnt = 0;
            foreach (int t_key in Base_TreeDic_Cnt.Keys)
            {
                R_Key = Base_TreeDic_Cnt[t_key];
                if (Base_TreeDic[R_Key].VisibleTF == true && Base_TreeDic[R_Key].VisibleTF2 == true)
                {
                    TreeDic[R_Key] = Base_TreeDic[R_Key];
                    TreeDic[R_Key].ChildCount = 0;
                    TreeDic[R_Key].ChildNumber.Clear();
                    TreeDic[R_Key].BaseDataCount = t_key;

                    if (t_key > 0)
                    {
                        ParentKey = Base_TreeDic[R_Key].ParentKey;
                        TreeDic[ParentKey].ChildCount++;
                        TreeDic[ParentKey].NextDataNum = t_key ;

                        TreeDic[ParentKey].ChildNumber[TreeDic[ParentKey].ChildCount] = TreeDic[R_Key];

                        TreeDic[R_Key].ParentClass = TreeDic[ParentKey];                      
                        
                    }

                    T_TreeDic_Cnt[Cnt] = R_Key;
                    Cnt++;
                }
                
            }
            TreeDic_Cnt.Clear ();
            TreeDic_Cnt = T_TreeDic_Cnt;



        }













        private void BaseDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            RectangleF tt = new RectangleF();
            Rectangle tt2 = new Rectangle();
            string msg = "";

            string R_Key = "";
            Pen T_p = new Pen(Color.Black);

            //e.PageBounds.Height 

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                R_Key = TreeDic_Cnt[t_key];

                tt.X = TreeDic[R_Key].Left;                tt.Y = TreeDic[R_Key].Top ;
                tt2.X = TreeDic[R_Key].Left;               tt2.Y = TreeDic[R_Key].Top;

                tt.Width = TreeDic[R_Key].Width; tt.Height = TreeDic[R_Key].Height;
                tt2.Width = TreeDic[R_Key].Width; tt2.Height = TreeDic[R_Key].Height;

                msg = TreeDic[R_Key].KeyName;

                e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", float.Parse(combo_Font.Text.ToString())), Brushes.Black, tt);                
                e.Graphics.DrawRectangle(T_p, tt2);
            }





            //if (curPageNumber == 0)
            //{
            //    e.HasMorePages = false;

            //}
            //else
            //{
            //    e.Graphics.DrawString("잘먹고잘살자", new Font("Arial", 10), Brushes.Black, tt);
            //    Pen T_p = new Pen(Color.Black);
            //    e.Graphics.DrawRectangle(T_p, tt2);

            //    e.HasMorePages = true;
            //    curPageNumber--;
            //}

        }

        private void BaseDoc_PrintPage_1(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
            RectangleF tt = new RectangleF();
            Rectangle tt2 = new Rectangle();
            string msg = "";
            int Cut_H = 0; int Cut_W = 0;
            string R_Key = "";
            Pen T_p = new Pen(Color.Black);

            //int Start_h = e.PageBounds.Height * Print_H_Cur_PagCnt  -e.PageBounds.Height;
            //int End_h = e.PageBounds.Height * Print_H_Cur_PagCnt;

            //int Start_W = e.PageBounds.Width * Print_W_Cur_PagCnt - e.PageBounds.Width;
            //int End_W = e.PageBounds.Width * Print_W_Cur_PagCnt; 

            //Cut_W = e.PageBounds.Width * Print_W_Cur_PagCnt - e.PageBounds.Width;
            //Cut_H = e.PageBounds.Height * Print_H_Cur_PagCnt - e.PageBounds.Height;
            //cls_form_Meth cfm = new cls_form_Meth();

            //foreach (int t_key in TreeDic_Cnt.Keys)
            //{
            //    R_Key = TreeDic_Cnt[t_key];

            //    if (
            //            TreeDic[R_Key].BaseLeft + TreeDic[R_Key].Width >= Start_W && TreeDic[R_Key].BaseLeft <=  End_W
            //            && TreeDic[R_Key].BaseTop + TreeDic[R_Key].Height >= Start_h && TreeDic[R_Key].BaseTop <= End_h
            //            )
            //        {
            //            tt.X = TreeDic[R_Key].BaseLeft - Cut_W;
            //            tt.Y = TreeDic[R_Key].BaseTop - Cut_H;

            //            tt2.X = TreeDic[R_Key].BaseLeft - Cut_W;
            //            tt2.Y = TreeDic[R_Key].BaseTop - Cut_H;

            //            tt.Width = TreeDic[R_Key].Width + 2; tt.Height = TreeDic[R_Key].Height;
            //            tt2.Width = TreeDic[R_Key].Width + 2; tt2.Height = TreeDic[R_Key].Height;

            //            msg = TreeDic[R_Key].KeyName;

            //            e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", float.Parse(combo_Font.Text.ToString())), Brushes.Black, tt);
            //            e.Graphics.DrawRectangle(T_p, tt2);

            //            //2016-11-15 R S로 이름옆에 표시하는 걸로 변경함
            //            //if (TreeDic[R_Key].LeaveCheck == cfm._chang_base_caption_search("탈퇴"))
            //            //{
            //            //    e.Graphics.DrawLine(T_p, tt.X, tt.Y + TreeDic[R_Key].Height, tt.X + TreeDic[R_Key].Width, tt.Y);
            //            //    e.Graphics.DrawLine(T_p, tt.X, tt.Y, tt.X + TreeDic[R_Key].Width, tt.Y + TreeDic[R_Key].Height);
            //            //}
                        
            //        }
            //}
            //int LineX1 = 0; int LineX2 = 0; int LineY1 = 0; int LineY2 = 0;

            //foreach (int t_key in LineDic.Keys)
            //{
            //    LineX1= LineDic[t_key].BX1 -Cut_W;
            //    LineX2 = LineDic[t_key].BX2 - Cut_W;

            //    LineY1 = LineDic[t_key].BY1 - Cut_H;
            //    LineY2 = LineDic[t_key].BY2 - Cut_H;

            //    if ((LineX1 >= 0 || LineX2 >= 0) && (LineY1 >= 0 || LineY2 >= 0))
            //    {
            //        e.Graphics.DrawLine (T_p,LineX1,LineY1,LineX2, LineY2 ) ;
            //    }

            //}

            //e.HasMorePages = true;
         
            //if (Print_W_Cur_PagCnt == W_Print_PagCnt)
            //{
                
            //    Print_W_Cur_PagCnt = 0;
            //    if (Print_H_Cur_PagCnt > H_Print_PagCnt)
            //    {
            //        e.HasMorePages = false;                    
            //    }

            //    Print_H_Cur_PagCnt++;        
            
               
            //}

            //Print_W_Cur_PagCnt ++ ;

            //if (e.HasMorePages == false)
            //    Group_Tree_Print_Pre_Work();

            //새로추가해 보앗음.
            int pageW = e.PageBounds.Width - 20, pageH = e.PageBounds.Height;
            tt.X = 18;
            tt.Y = 18;
            tt.Width = pageW - 55;
            tt.Height = pageH - 55;

            Drawing_PictureBox_GropTree_Page();
            e.Graphics.DrawImage(pbox_T.Image, tt);

            e.HasMorePages = true;

            if (Print_W_Cur_PagCnt == W_Print_PagCnt)
            {

                Print_W_Cur_PagCnt = 0;

                if (H_Print_PagCnt == 1)
                {
                    e.HasMorePages = false;
                }
                else
                {
                    if (Print_H_Cur_PagCnt > H_Print_PagCnt)
                    {
                        e.HasMorePages = false;
                    }
                }
                Print_H_Cur_PagCnt++;


            }

            Print_W_Cur_PagCnt++;

            if (e.HasMorePages == false)
                Group_Tree_Print_Pre_Work();
        }




        private void Drawing_PictureBox_GropTree_Page()
        {




            int BH = BaseDoc.DefaultPageSettings.PaperSize.Height * Print_Cut_int;
            int BW = BaseDoc.DefaultPageSettings.PaperSize.Width * Print_Cut_int;

            Bitmap bt;
            bt = new Bitmap(BW, BH);
            pbox_T.Image = bt;

            // Graphics graphic = Graphics.FromImage(bt);

            RectangleF tt = new RectangleF();
            Rectangle tt2 = new Rectangle();
            string msg = "";
            int Cut_H = 0; int Cut_W = 0;
            string R_Key = "";
            Pen T_p = new Pen(Color.Black);

            int Start_h = BH * Print_H_Cur_PagCnt - BH;
            int End_h = BH * Print_H_Cur_PagCnt;

            int Start_W = BW * Print_W_Cur_PagCnt - BW;
            int End_W = BW * Print_W_Cur_PagCnt;

            Cut_W = BW * Print_W_Cur_PagCnt - BW;
            Cut_H = BH * Print_H_Cur_PagCnt - BH;

            //Graphics graphic = pbox_T.CreateGraphics();

            Graphics graphic = Graphics.FromImage(bt);
            cls_form_Meth cfm = new cls_form_Meth();

            Color myColor = System.Drawing.Color.Gainsboro; // Color.FromArgb(236, 241, 220);
            SolidBrush myBrush = new SolidBrush(myColor);
            Font font_Pn = new Font("돋움", 50, FontStyle.Bold, GraphicsUnit.Point);
            //graphic.DrawString(Print_H_Cur_PagCnt.ToString() + "-" + Print_W_Cur_PagCnt.ToString(), font_Pn, myBrush, BW / 2, BH / 2);


            float t_font = float.Parse(combo_Font.Text.ToString());
            Font font = new Font("돋움", t_font, FontStyle.Regular, GraphicsUnit.Point);

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                R_Key = TreeDic_Cnt[t_key];

                if (
                        TreeDic[R_Key].BaseLeft + TreeDic[R_Key].Width >= Start_W && TreeDic[R_Key].BaseLeft <= End_W
                        && TreeDic[R_Key].BaseTop + TreeDic[R_Key].Height >= Start_h && TreeDic[R_Key].BaseTop <= End_h
                        )
                {
                    tt.X = TreeDic[R_Key].BaseLeft - Cut_W - 3;
                    tt.Y = TreeDic[R_Key].BaseTop - Cut_H + 2;

                    tt2.X = TreeDic[R_Key].BaseLeft - Cut_W;
                    tt2.Y = TreeDic[R_Key].BaseTop - Cut_H;

                    tt.Width = TreeDic[R_Key].Width + 6;
                    tt.Height = TreeDic[R_Key].Height;

                    tt2.Width = TreeDic[R_Key].Width;
                    tt2.Height = TreeDic[R_Key].Height;

                    msg = TreeDic[R_Key].KeyName;


                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    //픽처박스 안에 들어가는 박스안에 들어가는 내역을 쓴다.
                    graphic.DrawString(TreeDic[R_Key].KeyName, font, Brushes.Black, tt, stringFormat);

                    //graphic.DrawString(msg, new System.Drawing.Font("돋움", float.Parse(combo_Font.Text.ToString())), Brushes.Black, tt);
                    graphic.DrawRectangle(T_p, tt2);

                    if (TreeDic[R_Key].LeaveCheck == cfm._chang_base_caption_search("탈퇴"))
                    {
                        graphic.DrawLine(T_p, tt.X, tt.Y + TreeDic[R_Key].Height, tt.X + TreeDic[R_Key].Width, tt.Y);
                        graphic.DrawLine(T_p, tt.X, tt.Y, tt.X + TreeDic[R_Key].Width, tt.Y + TreeDic[R_Key].Height);
                    }
                }
            }




            //PictureBox pictureBox_P_N = new PictureBox();
            //pictureBox_P_N.BackColor = Color.Transparent;
            //pbox_T.Controls.Add(pictureBox_P_N);
            //pictureBox_P_N.Parent = pbox_T;
            //pictureBox_P_N.Location = new Point(100, 100);

            //Bitmap bt_P;
            //bt_P = new Bitmap(100, 20);
            //pictureBox_P_N.Image = bt_P;

            //Graphics graphic_P_n = Graphics.FromImage(bt_P);
            //graphic_P_n.DrawString(Print_H_Cur_PagCnt.ToString() + " - " + Print_W_Cur_PagCnt.ToString (), font, Brushes.Black, 0,0);


            int LineX1 = 0; int LineX2 = 0; int LineY1 = 0; int LineY2 = 0;

            foreach (int t_key in LineDic.Keys)
            {
                LineX1 = LineDic[t_key].BX1 - Cut_W;
                LineX2 = LineDic[t_key].BX2 - Cut_W;

                LineY1 = LineDic[t_key].BY1 - Cut_H;
                LineY2 = LineDic[t_key].BY2 - Cut_H;

                if ((LineX1 >= 0 || LineX2 >= 0) && (LineY1 >= 0 || LineY2 >= 0))
                {
                    graphic.DrawLine(T_p, LineX1, LineY1, LineX2, LineY2);
                }

            }


        }




        private void Group_Tree_Print_Pre_BP_Work()
        {
            
            int BH = BaseDoc.DefaultPageSettings.PaperSize.Height * 2 ;
            int BW = BaseDoc.DefaultPageSettings.PaperSize.Width * 2;

           



            pbox_T.Height = BH; pbox_T.Width  = BW; 


            string R_Key = ""; int maxLeft = 0; int minTop = 0;
            PB_W_Print_PagCnt = 0;
            PB_H_Print_PagCnt = 0;

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                R_Key = TreeDic_Cnt[t_key];
                if (maxLeft < TreeDic[R_Key].BaseLeft)
                    maxLeft = TreeDic[R_Key].BaseLeft + TreeDic[R_Key].Width;

                if (minTop < TreeDic[R_Key].BaseTop)
                    minTop = TreeDic[R_Key].BaseTop + TreeDic[R_Key].Height;
            }
            PB_W_Print_PagCnt = (maxLeft / BH);
            if ((maxLeft % BW) > 0)
                PB_W_Print_PagCnt = PB_W_Print_PagCnt + 1;

            PB_H_Print_PagCnt = (minTop / BH);
            if ((minTop % BH) > 0)
                PB_H_Print_PagCnt = PB_H_Print_PagCnt + 1;

            PB_Print_W_Cur_PagCnt = 1;
            PB_Print_H_Cur_PagCnt = 1;


            int SW = 0;
            string ap_path = Application.StartupPath.ToString();
            string SaveImage_path = Path.Combine(ap_path, "Doc");

            progressBar1.Minimum = 0;
            progressBar1.Maximum = PB_H_Print_PagCnt * PB_W_Print_PagCnt;            
            progressBar1.Step = 1; progressBar1.Value = 0;

            while (PB_Print_H_Cur_PagCnt <= PB_H_Print_PagCnt)
            {

                PB_Print_W_Cur_PagCnt = 1;
                while (PB_Print_W_Cur_PagCnt <= PB_W_Print_PagCnt)
                {
                    Drawing_PictureBox_GropTree();

                    //string Image_Name = mtxtMbid.Text.Trim() + "_Save_" + PB_Print_H_Cur_PagCnt + "_" + PB_W_Print_PagCnt;
                    string Image_Name = mtxtMbid.Text.Trim() + "_GroupTreeSave_" + PB_Print_H_Cur_PagCnt + "_" + PB_W_Print_PagCnt;
                    string Base_Image_Name = Image_Name;
                    int Excel_File_Cnt = 0;

                _Excel_File_Re_Check:
                    string Temp_Name = System.IO.Path.Combine(SaveImage_path + "\\" + Image_Name + ".jpg");

                    if (System.IO.File.Exists(Temp_Name) == true)
                    {
                        Excel_File_Cnt++;
                        Image_Name = Base_Image_Name + "(" + Excel_File_Cnt.ToString() + ")";
                        goto _Excel_File_Re_Check;
                    }

                    pbox_T.Image.Save(Temp_Name);
                    PB_Print_W_Cur_PagCnt++;

                    SW = 1;

                    progressBar1.PerformStep(); progressBar1.Refresh();
                }

                PB_Print_H_Cur_PagCnt++;
            }


            if (SW >= 1)
                MessageBox.Show(SaveImage_path + " " + cls_app_static_var.app_msg_rm.GetString("Msg_Save_Folder"));                                      


        }


        //Group_Tree_Print_Pre_BP_Work
        private void Drawing_PictureBox_GropTree()
        {
            int BH = BaseDoc.DefaultPageSettings.PaperSize.Height * 2;
            int BW = BaseDoc.DefaultPageSettings.PaperSize.Width * 2 ;

            Bitmap bt;
            bt = new Bitmap( BW,BH);
            pbox_T.Image = bt;

                       // Graphics graphic = Graphics.FromImage(bt);
            
            RectangleF tt = new RectangleF();
            Rectangle tt2 = new Rectangle();
            string msg = "";
            int Cut_H = 0; int Cut_W = 0;
            string R_Key = "";
            Pen T_p = new Pen(Color.Black);

            int Start_h = BH * PB_Print_H_Cur_PagCnt - BH;
            int End_h = BH * PB_Print_H_Cur_PagCnt;

            int Start_W = BW * PB_Print_W_Cur_PagCnt - BW;
            int End_W = BW * PB_Print_W_Cur_PagCnt;

            Cut_W = BW * PB_Print_W_Cur_PagCnt - BW;
            Cut_H = BH * PB_Print_H_Cur_PagCnt - BH;

            //Graphics graphic = pbox_T.CreateGraphics();
           
            Graphics graphic = Graphics.FromImage(bt);
            cls_form_Meth cfm = new cls_form_Meth();

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                R_Key = TreeDic_Cnt[t_key];

                if (
                        TreeDic[R_Key].BaseLeft + TreeDic[R_Key].Width >= Start_W && TreeDic[R_Key].BaseLeft <= End_W
                        && TreeDic[R_Key].BaseTop + TreeDic[R_Key].Height >= Start_h && TreeDic[R_Key].BaseTop <= End_h
                        )
                {
                    tt.X = TreeDic[R_Key].BaseLeft - Cut_W;
                    tt.Y = TreeDic[R_Key].BaseTop - Cut_H;

                    tt2.X = TreeDic[R_Key].BaseLeft - Cut_W;
                    tt2.Y = TreeDic[R_Key].BaseTop - Cut_H;

                    tt.Width = TreeDic[R_Key].Width; tt.Height = TreeDic[R_Key].Height;
                    tt2.Width = TreeDic[R_Key].Width; tt2.Height = TreeDic[R_Key].Height;

                    msg = TreeDic[R_Key].KeyName;

                    graphic.DrawString(msg, new System.Drawing.Font("돋움", float.Parse(combo_Font.Text.ToString())), Brushes.Black, tt);
                    graphic.DrawRectangle(T_p, tt2);

                    
                    if (TreeDic[R_Key].LeaveCheck == cfm._chang_base_caption_search("탈퇴"))
                    {
                        graphic.DrawLine(T_p, tt.X, tt.Y + TreeDic[R_Key].Height, tt.X + TreeDic[R_Key].Width, tt.Y);
                        graphic.DrawLine(T_p, tt.X, tt.Y, tt.X + TreeDic[R_Key].Width, tt.Y + TreeDic[R_Key].Height);
                    }
                }
            }



            int LineX1 = 0; int LineX2 = 0; int LineY1 = 0; int LineY2 = 0;

            foreach (int t_key in LineDic.Keys)
            {
                LineX1 = LineDic[t_key].BX1 - Cut_W;
                LineX2 = LineDic[t_key].BX2 - Cut_W;

                LineY1 = LineDic[t_key].BY1 - Cut_H;
                LineY2 = LineDic[t_key].BY2 - Cut_H;

                if ((LineX1 >= 0 || LineX2 >= 0) && (LineY1 >= 0 || LineY2 >= 0))
                {
                    graphic.DrawLine(T_p, LineX1, LineY1, LineX2, LineY2);
                }

            }


        }


        private void SaveData(object IClass, string fileName)
        {
            StreamWriter writer = null;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer((IClass.GetType()), new XmlRootAttribute("Rectangles"));
                //XmlSerializer xmlSerializer = new XmlSerializer(typeof(Rectangle[]), new XmlRootAttribute( "Rectangles" ));
                writer = new StreamWriter(fileName);
                xmlSerializer.Serialize(writer, IClass);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
                writer = null;
            }
        }

        private void butt_Up_Click(object sender, EventArgs e)
        {
            cls_Check_Input_Error c_er = new cls_Check_Input_Error();
            if (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                int Ret = 0;
                Ret = c_er._Member_Nmumber_Split(mtxtMbid);
                if (Ret == -1)
                {
                    mtxtMbid.Focus(); return;
                }

                Db_Grid_Popup(txtName, mtxtMbid, butt_Up, "");
            } 
        }


        private void Db_Grid_Popup(TextBox tb, MaskedTextBox tb1_Code, Button butt_Loc, string strSql)
        {
            cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            DataGridView Popup_gr = new DataGridView();
            Popup_gr.Name = "Popup_gr";
            this.Controls.Add(Popup_gr);
            cgb_Pop.basegrid = Popup_gr;
            cgb_Pop.Base_fr = this;
            cgb_Pop.Base_tb = tb1_Code;  //앞에게 코드
            cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            cgb_Pop.Base_Location_obj = butt_Loc;

            cgb_Pop.basegrid.DoubleClick += new EventHandler(Popup_basegrid_DoubleClick);
            //cgb_Pop.basegrid.KeyDown += new KeyEventHandler(basegrid_KeyDown);

            string Mbid = ""; int Mbid2 = 0;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
            {
                string Tsql;
                Tsql = "Select ";
                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " mbid + '-' + Convert(Varchar,mbid2) AS M_Number ";
                else
                    Tsql = Tsql + " mbid2  AS M_Number";

                Tsql = Tsql + " , M_Name  ";
                Tsql = Tsql + " From ufn_SaveUp_Member_Search ( ";
                Tsql = Tsql + " '" + Mbid + "'";
                Tsql = Tsql + " ," + Mbid2 + ")";
                Tsql = Tsql + " Where lvl > 0 ";
                Tsql = Tsql + " Order By lvl ";

                cgb_Pop.db_grid_Popup_Base(2, "회원_번호", "성명", "M_Number", "M_Name", Tsql, 0);
            }
        }

        void Popup_basegrid_DoubleClick(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtMbid);
            Clear_Object();
           

            DataGridView T_Gd = (DataGridView)sender;

            cls_form_Meth cfm = new cls_form_Meth();
            cfm.form_Group_Panel_Enable_True(this);

            T_Gd.Visible = false;
            if (T_Gd.CurrentRow.Cells[0].Value != null)
            {
                mtxtMbid.Text = T_Gd.CurrentRow.Cells[0].Value.ToString();
                txtName.Text = T_Gd.CurrentRow.Cells[1].Value.ToString();
                mtxtMbid.Focus();
                
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                button1_Click(button1, e);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }


            T_Gd.Dispose();
        }

        private void butt_De_Click(object sender, EventArgs e)
        {
             

            if (TreeDic == null)
                return;
            if (TreeDic.Count == 0)
                return;
            if (chk_Total.Checked == false && chb_1.Checked == false && chb_2.Checked == false && chb_3.Checked == false && chb_4.Checked == false && chb_5.Checked == false
                && chb_7.Checked == false && chb_8.Checked == false && chb_9.Checked == false && chb_10.Checked == false && chb_12.Checked == false && chb_13.Checked == false
                && chb_18.Checked == false && chb_19.Checked == false && chb_20.Checked == false)
                {
                    return;
                }


            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            
            //foreach (string t_key in TreeDic.Keys)
            //{                
            //    Key_Name_Change(t_key); //나오는 TEXT를 변경한다.
            //}

            Group_Tree_Re_Drawing();

            Chnage_gid_Tree_Config();  //관리자별 체크 설정을 저장한다.

     
            this.Cursor = System.Windows.Forms.Cursors.Default;

            pb_De.Visible = false;
        }

        private void Key_Name_Change(string t_key, ref int Check_Cnt )
        {
            Check_Cnt = 0;

            string t_KeyName = "";
            TreeDic[t_key].KeyName = "";
            if (chb_1.Checked == true)
            {
                t_KeyName = TreeDic[t_key].IDKey;
                Check_Cnt++;
            }
            if (chb_2.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].MbidName;
                Check_Cnt++;
            }

            if (chb_3.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].RegDate;
                Check_Cnt++;
            }

            if (chb_4.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].BusName;
                Check_Cnt++;
            }

            if (chb_5.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].CpNumber;
                Check_Cnt++;
            }

            if (chb_6.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].LeaveCheck;
                Check_Cnt++;
            }

            if (chb_7.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].Nominid;
                Check_Cnt++;
            }

            if (chb_8.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].NominName;
                Check_Cnt++;
            }

            if (chb_9.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].TotalPV;
                Check_Cnt++;
            }

            if (chb_10.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + string.Format(cls_app_static_var.str_Currency_Type, TreeDic[t_key].f_TDownPV);
                Check_Cnt++;
            }

            if (chb_11.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].Grade1;
                Check_Cnt++;
            }


            if (chb_14.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].Grade_P;
                Check_Cnt++;
            }

            if (chb_15.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].Sell_Mem_TF;
                Check_Cnt++;
            }

            if (chb_16.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].Down_Sobi_PV;
                Check_Cnt++;
            }

            
            if (chb_17.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].ClassP_Date;
                Check_Cnt++;
            }
           
            if (chb_12.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].TotalBV;
                Check_Cnt++;
            }

            if (chb_13.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + string.Format(cls_app_static_var.str_Currency_Type, TreeDic[t_key].f_TDownBV);
                Check_Cnt++;
            }
            if (chb_18.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].SellDate_2;
                Check_Cnt++;
            }
            if (chb_19.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].Grade_Max;
                Check_Cnt++;
            }
            if (chb_20.Checked == true)
            {
                t_KeyName = t_KeyName + "\n" + TreeDic[t_key].Grade_Cur;
                Check_Cnt++;
            }
            TreeDic[t_key].KeyName = t_KeyName;            
        }


        private void Group_Tree_Re_Drawing()
        {
            object sender = null; 

                                               
            MakeTreeLebel_Save(); //디비접속해서 회원 하선들의 정보를 가져온다
            DrawData_Label_Position_Top(); //회원 클래스 들의 Top정보를 계산해서 넣는다.
            Label_Drow_Left();    //회원 클래스 들의 Left 정보를 계산해서 넣는다.
            Up_Left_Total_Move(); //회원 클래스 들을 자식들에 맞춰서 left를 조정한다.
            Line_Drow_Position();  //회원 클래스들을 기준으로 해서 자식들과 연결하는 선의 위치 클래스들을 생성 위치값을 구한다.
            MakeTreeLebel_Save(1);

            panel2.Visible = false;
            //Drow_Tree_Lbl();
            Drow_Tree_Lbl(1);
            PaintEventArgs d =null;
            Drow_Tree_Line(sender, d);
            Drow_Tree_Scroll(this, d);
            panel2.Visible = true;
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(cls_app_static_var.User_Time_Zone);
            MessageBox .Show (  TimeZoneInfo.ConvertTime(DateTime.Now, timeZone).ToString()) ;
            ////DateTime utcdt ;
            ////System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("ja-JP");
            //////System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("ko-KR");
            
            //////en-US 

            //////culture = new System.Globalization.CultureInfo("ja-JP") ;
            ////System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            //// utcdt = DateTime.UtcNow;
            //////DateTime d = System.DateTime.UtcNow; 
            //////d = DateTime.Parse(string.Format("{0:yyyy-MM-dd HH:mm:ss}",  culture));
            //// MessageBox.Show("일본" + utcdt.ToString(culture));


            //////culture = new System.Globalization.CultureInfo("lo-LA");
            //////System.Threading.Thread.CurrentThread.CurrentCulture = culture;            
            //////utcdt = DateTime.UtcNow;
            //////MessageBox.Show("라오스" + utcdt.ToString());

            //////culture = new System.Globalization.CultureInfo("ko-KR");
            //////System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            //////utcdt = DateTime.UtcNow;
            //////MessageBox.Show("대한민국" + utcdt.ToString());




        }

        private void combo_Font_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TreeDic == null)
                return;
            if (TreeDic.Count == 0)
                return;


            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Group_Tree_Re_Drawing();
            this.Cursor = System.Windows.Forms.Cursors.Default;

        }

        private void MenuItem_S_Change_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            
            int keyCnt = int.Parse(contextM.Tag.ToString());
            mtxtMbid.Text = TreeDic[TreeDic_Cnt[keyCnt]].IDKey;

            int reCnt = 0;
            cls_Search_DB cds = new cls_Search_DB();
            string Search_Name = "";
            reCnt = cds.Member_Name_Search(mtxtMbid.Text, ref Search_Name);

            if (reCnt == 1)
            {
                txtName.Text = Search_Name;
            }

            button1_Click(button1, e) ;

            this.Cursor = System.Windows.Forms.Cursors.Default;


        }

        private void combo_Step_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void chk_Total_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (Control t_c in pb_De.Controls)
            {
                if (t_c is CheckBox == true)
                {
                    CheckBox t_cb = (CheckBox)t_c;
                    if (t_cb.Visible == true)
                        t_cb.Checked = chk_Total.Checked;
                }
            }
                
        }

        private void radioB_S_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtSellDate1, mtxtSellDate2, (RadioButton)sender);
            ct.Search_Date_TextBox_Put(mtxtSellDate4, mtxtSellDate5, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }






        private void Set_Form_Date_Up(string strTemp)
        {

            dGridView_Up_S_Header_Reset(dGridView_Up_S); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset();

            if (opt_C_2.Checked == true )
                Base_Grid_Set(" ufn_Up_Search_Save ");
            else
                Base_Grid_Set(" ufn_Up_Search_Nomin ");

            

        }


        private void dGridView_Up_S_Header_Reset(DataGridView t_Dgv)
        {
            cg_Up_S.Grid_Base_Arr_Clear();

            cg_Up_S.grid_col_Count = 5;
            cg_Up_S.basegrid = t_Dgv; //dGridView_Up_S;
            cg_Up_S.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_Up_S.grid_Frozen_End_Count = 2;
            cg_Up_S.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"회원_번호"  , "성명"   , "위치"  , "대수"   , ""        
                                    };
            cg_Up_S.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 60, 70, 30, 40, 0                               
                            };
            cg_Up_S.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                                                                                   
                                   };
            cg_Up_S.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5      
                              };
            cg_Up_S.grid_col_alignment = g_Alignment;
            cg_Up_S.basegrid.RowHeadersWidth = 25;

            cg_Up_S.basegrid.ColumnHeadersDefaultCellStyle.Font =
            new Font(cg_Up_S.basegrid.Font.FontFamily, 8);
        }



        private void Base_Grid_Set(string Ufn_Name)
        {
            string T_Mbid = "";
            T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) != 1) return;

            string Tsql = "";

            Tsql = "Select  ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " T_up.mbid + '-' + Convert(Varchar,T_up.mbid2) ";
            else
                Tsql = Tsql + " T_up.mbid2 ";

            Tsql = Tsql + " ,T_up.M_Name ";
            Tsql = Tsql + " ,T_up.curP ";
            Tsql = Tsql + " ,T_up.lvl ";

            Tsql = Tsql + " From " + Ufn_Name;
            Tsql = Tsql + " ('" + Mbid + "'," + Mbid2.ToString() + ") AS T_up";

            Tsql = Tsql + " Where    lvl > 0 ";
            Tsql = Tsql + " Order BY lvl Desc ";

            //당일 등록된 회원을 불러온다.

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "ufn_Up_Search_Save", ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic_Line(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            cg_Up_S.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cg_Up_S.db_grid_Obj_Data_Put();
        }




        private void Set_gr_dic_Line(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables["ufn_Up_Search_Save"].Rows[fi_cnt][0]
                                ,ds.Tables["ufn_Up_Search_Save"].Rows[fi_cnt][1]  
                                ,ds.Tables["ufn_Up_Search_Save"].Rows[fi_cnt][2]
                                ,ds.Tables["ufn_Up_Search_Save"].Rows[fi_cnt][3]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][4]                                                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            if ((sender as DataGridView).CurrentRow.Cells[0].Value != null)
            {
                string t_Mbid = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();


                EventArgs ee = null;
                //Base_Button_Click(butt_Clear, ee);

                mtxtMbid.Text = t_Mbid;
                int reCnt = 0;
                cls_Search_DB cds = new cls_Search_DB();
                string Search_Name = "";
                reCnt = cds.Member_Name_Search(mtxtMbid.Text, ref Search_Name);

                if (reCnt == 1)
                {
                    txtName.Text = Search_Name;
                    button1_Click(butt_Select, ee);
                }


            }

            tableLayoutPanel11.Visible = false;
        }

        private void tabC_1_SelectedIndexChanged(object sender, EventArgs e)
        {
                      
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (tabC_1.SelectedIndex == 0)
            {
                cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                cgbp.dGridView_Put_baseinfo(this, dGridView_Sell, "sell", Mouse_Select_key);
            }
            if (tabC_1.SelectedIndex == 1)
            {
                cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                cgbp.dGridView_Put_baseinfo(this, dGridView_Sell_Item, "item", Mouse_Select_key);
            }

            if (tabC_1.SelectedIndex == 2)
            {
                cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                cgbp.dGridView_Put_baseinfo(this, dGridView_Sell_Cacu, "cacu", Mouse_Select_key);
            }

            if (tabC_1.SelectedIndex == 3)
            {
                cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                cgbp.dGridView_Put_baseinfo(this, dGridView_Sell_Rece, "rece", Mouse_Select_key);
            }

            //if (tabC_1.SelectedIndex == 4)
            //{
            //    cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
            //    cgbp.dGridView_Put_baseinfo(this, dGridView_Pay, "pay", Mouse_Select_key);
            //}
           
            if (tabC_1.SelectedIndex == 4)
            {
                cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                cgbp.dGridView_Put_baseinfo(this, dGridView_Up_Sa, "saveup", mtxtMbid.Text.Trim());
            }
            if (tabC_1.SelectedIndex == 5)
            {
                cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                cgbp.dGridView_Put_baseinfo(this, dGridView_Up_N, "nominup", mtxtMbid.Text.Trim());
            }

            //if (tabC_1.SelectedIndex == 7)
            //{
            //    cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
            //    cgbp.dGridView_Put_baseinfo(this, dGridView_Memberinfo, "member", Mouse_Select_key);
            //}
            //this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private int but_Exp_Base_Left = 0;
        private int Parent_but_Exp_Base_Width = 0;

        private void but_Exp_Click(object sender, EventArgs e)
        {
            if (but_Exp.Text == "<<")
            {
                chb_6.Checked = false;

                //chb_10.Checked = false;
                //chb_13.Checked = false;


                chb_11.Checked = false;


                chb_14.Checked = false;
                chb_15.Checked = false;
                chb_16.Checked = false;
                chb_17.Checked = false;

                pb_De.Visible = true;
                pb_De.BringToFront();
                pb_De.Refresh();
                //Parent_but_Exp_Base_Width = but_Exp.Parent.Width;
                //but_Exp_Base_Left = but_Exp.Left;

                //but_Exp.Parent.Width = but_Exp.Width;
                //but_Exp.Left = 0;
                but_Exp.Text = ">>";
             
            

            }
            else
            {
                pb_De.Visible =false ;
                //pb_De.BringToFront();
                pb_De.Refresh();
                //but_Exp.Parent.Width = Parent_but_Exp_Base_Width;
                //but_Exp.Left = but_Exp_Base_Left;
                but_Exp.Text = "<<";
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            combo_Font.Text = trackBar1.Value.ToString ();
        }

        private void but_Up_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel11.Visible == false)
            {
                tableLayoutPanel11.Visible = true;

                dGridView_Up_S_Header_Reset(dGridView_Up_S); //디비그리드 헤더와 기본 셋팅을 한다.
                cg_Up_S.d_Grid_view_Header_Reset();

                Base_Grid_Set(" ufn_Up_Search_Save ");
            }
            else
                tableLayoutPanel11.Visible = false;
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pb_De_Paint(object sender, PaintEventArgs e)
        {

        }
        #region * Mouse Move Action : 190425 지성경 추가 

        Point MouseCenterPoint = new Point(-1,-1);
        Point MouseMovePoint = new Point(-1, -1);
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (this.Cursor == System.Windows.Forms.Cursors.Default)
                {
                    this.Cursor = System.Windows.Forms.Cursors.SizeAll;
                    MouseCenterPoint = Cursor.Position;
                    MouseMovePoint = Cursor.Position;
                    timer1.Start();
                }
                else
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;

                    timer1.Stop();

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.Cursor == System.Windows.Forms.Cursors.Default)
                {
                    timer1.Stop();
                    return;
                }
                int varX = MouseMovePoint.X - MouseCenterPoint.X;
                int varY = MouseMovePoint.Y - MouseCenterPoint.Y;

                if (MouseCenterPoint.X < MouseMovePoint.X && hSC.Value != hSC.Maximum)
                    if (hSC.Value + varX >= hSC.Maximum) hSC.Value = hSC.Maximum; else hSC.Value += varX;
                if (MouseCenterPoint.X > MouseMovePoint.X && hSC.Value != 0)
                    if (hSC.Value + varX <= 0) hSC.Value = 0; else hSC.Value += varX;
                if (MouseCenterPoint.Y < MouseMovePoint.Y && vSC.Value != vSC.Maximum)
                    if (vSC.Value + varY >= vSC.Maximum) vSC.Value = vSC.Maximum; else vSC.Value += varY;
                if (MouseCenterPoint.Y > MouseMovePoint.Y && vSC.Value != 0)
                    if (vSC.Value + varY <= 0) vSC.Value = 0; else vSC.Value += varY;

                timer1.Interval = 350;
            }
            catch
            {
                timer1.Stop();
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }
        #endregion

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            MouseMovePoint = Cursor.Position;
        }


        private void hSC_ValueChanged(object sender, EventArgs e)
        {
            int NewValue = hSC.Value;

            if (hScroll_Be_Value != NewValue)
            {
                int GetXValue = NewValue - hScroll_Be_Value;
                int CucLeft = 0;
                string R_Key = "";

                foreach (int t_key in TreeDic_Cnt.Keys)
                {
                    R_Key = TreeDic_Cnt[t_key];

                    CucLeft = TreeDic[R_Key].BaseLeft - NewValue;
                    TreeDic[R_Key].Left = CucLeft;
                    TreeDic[R_Key].VisibleTF = false;
                }

                foreach (int t_key in LineDic.Keys)
                {
                    LineDic[t_key].X1 = LineDic[t_key].BX1 - NewValue;
                    LineDic[t_key].X2 = LineDic[t_key].BX2 - NewValue;
                }
                //Drow_Tree_Lbl();
                Drow_Tree_Lbl(1);

                hScroll_Be_Value = hSC.Value;

            }
        }

    }


    //public class Rectangle : IXmlSerializable
    //{
    //    public string Id { get; set; }
    //    public Point TopLeft { get; set; }
    //    public Point BottomRight { get; set; }
    //    //public RgbColor Color { get; set; }
    //}

}
