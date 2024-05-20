using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Data;
//using System.Drawing;
using System.Windows.Forms;
//using System.Reflection; 
using System.Diagnostics;
using System.Reflection;
using System.Drawing;

namespace MLM_Program
{
    class cls_app_static_var
    {
        public static string APP_VER = string.Empty;

        internal static int app_multi_lang_query;

        internal static System.Reflection.Assembly Assem;
        internal static string app_msg_resource;
        internal static System.Resources.ResourceManager app_msg_rm;

        internal static string app_Base_Str_resource;
        internal static System.Resources.ResourceManager app_base_str_rm;

        internal static string app_Base_Caption_resource;
        internal static System.Resources.ResourceManager app_base_caption_rm;//직대 후원기준으로해서 몇명까지 달수 있는지를 

        internal static string User_Time_Zone;
        //internal static DataGridView _Excel_Grid; //엑셀로 변환하고자 하는 그리드
        //internal static string Excel_Export_From_Name; //엑셀로 변환을 요청한 그리드가 있는 폼이름.
        //internal static string Excel_Export_File_Name; //엑셀로 변환을 할때.. 기본적으로 가져가는 이름.

        internal static int Member_Number_1;  //회원번호 형식상 앞자리 사용하고자 할경우 앞자리
        internal static int Member_Number_2;  //회원번호 뒷자리의 자리수.
        internal static string Member_Number_Fromat;   //회원번호 관련해서 어떤 형식을 가질건지 마스크텍스트 관련해서 셋팅을 잡기 위함.
        internal static int Member_Down_Cnt; //직대 후원기준으로해서 몇명까지 달수 있는지를 
               
        internal static int Member_Cpno_Visible_TF; //주민번호를 다 보여줄지 아님 뒷자리를 보여주지 않을지를 결정함.
        //1 이면 다 보여주고   0 이면 앞자리만 보여주고 뒤자리는 *로 보여줌.


        internal static int Member_Card_Num_Visible_TF;//카드번호를 다 보여줄지 아님 뒷자리를 보여주지 않을지를 결정함.
        //1 이면 다 보여주고   0 이면 앞자리만 보여주고 뒤자리는 *로 보여줌.

        internal static int Member_Card_Sugi_TF;//카드 수기특약을 할수 잇는 권한이 있는지를 체크한다.
        //1 이면 권한이 되고   0 이면 안된다.

        internal static int Member_Sell_Mem_TF_Ch_TF ;//회원 판매원 소비자 구분 변경 권한.
        //1 이면 권한이 되고   0 이면 안된다.

        internal static int Member_Name_Ch_TF;//회원 명의변경권한.
        //1 이면 권한이 되고   0 이면 안된다.

        internal static int Member_Nominid_Ch_TF;//회원 추천인경권한.
        //1 이면 권한이 되고   0 이면 안된다.

        internal static int Sales_Rec_Ch_TF; // 매출 배송정보 변경 권한
        //1 이면 권한이 되고   0 이면 안된다.


        internal static int Member_Talk_In_TF; // 회원정보 변경 현황 화면상에서 상당 내역 탭 볼수 잇는 권한
        //1 이면 권한이 되고   0 이면 안된다.


        internal static int Member_Return_Cacu_Save_FLAG;
        //1 이면 권한이 되고   0 이면 안된다.

        internal static int Member_Return_Cacu_Cancel_FLAG;
        //1 이면 권한이 되고   0 이면 안된다.
        

        


        internal static int Member_Cpno_Error_Check_TF; //주민번호 오류를 체크할지 물어보는 거임.
        //1이면 오류 체크하고   0 이면 오류 체크하지 마라


        internal static int Member_Cpno_Put_TF; //주민번호 필수 입력 사항인지 물어보는 거임
        //1이면 필수고   0 이면 필수가 아니다.

        internal static int Member_Reg_Line_Select_TF; //회원등록시 위치를 선택할지 아님 자동일지
        //1이면 선택이고   0 이면 자동이다.

        internal static int Member_Reg_Multi_TF; // 회원등록시 동일한 주민번호로 해서 2명이상 가입 여부
        // 1이면 가입이 되고   0이면 가입이 안된다.

        internal static int Program_User_Center_Sort; //프로그램이 센타 프로그램을 사용여부
        // 1이면 사용하느 것이고   0이면 사용 안한다.


        //보통 분류를 3단계 기본으로 잡는다. 
        //3단계 일데는 마지막 소분류 자리수가.. 상품 코드 등록 호면상에서 상품 코드의 자리수가 된다.
        //2단계 일때는 중분류 자리수가.
        internal static int Item_Code_Length; //상품코드 자리수를 셋팅한다.
        internal static int Item_Sort_1_Code_Length; //상품코드 자리수를 셋팅한다. 대분류 관련
        internal static int Item_Sort_2_Code_Length; //상품코드 자리수를 셋팅한다. 중분류 관련
        internal static int Item_Sort_3_Code_Length; //상품코드 자리수를 셋팅한다. 소분류 관련

        internal static int Center_Code_Length; //센타코드 자리수를 셋팅한다.

        //다국어시 DB상에 사용하는 용어를 한국어, 외국어의 필드를 정하는 변수. 우선은 mdimain에서
        //한국으로 잡고서 셋팅하고 잇음. 로그인 하는 사람의 국적에 따라서 변경되게 해놓음.
        internal static string Base_M_Detail_Ex;

        /// <summary>
        /// 다국어시 DB상에 사용하는 용어를 한국어, 외국어의 필드를 정하는 변수. - [tbl_SellType] 내 한국어 / 영어 Column name 중 선택
        /// </summary>
        internal static string Base_SellTypeName;

        //메인메뉴를 넣어두는 변수 권한 설정에서 메인 메뉴를 public으로 하지 않고 메뉴 텍스트와 이름을
        //가져오기 위함.
        internal static MenuStrip Mdi_Base_Menu;


        //기초자료 관리의 셋트 상품 구성 메뉴를 보이게 할지 안보이게 할지
        // 1이면 사용하느 것이고   0이면 사용 안한다.
        internal static int Program_Usering_Goods_Set;

        internal static Dictionary<string, Boolean> Mid_Main_Menu = new Dictionary<string, Boolean>();
        internal static Dictionary<string, string> Base_Label = new Dictionary<string, string>(); //국가별 언어들어가는 배열

        internal static string app_Company_Name;  //업데이트 받기 위한 폴더 이름
        internal static string app_FTP_ID;        //업데이트 관련 ftp 아이디
        internal static string app_FTP_PW;        //업데이트 관련 ftp 패스워드
        internal static string str_Currency_Type;
        internal static string str_Grid_Currency_Type;
        internal static string str_Currency_Money_Type;

        internal static int  save_uging_Pr_Flag;
        internal static int  nom_uging_Pr_Flag;
        //basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightGreen; //LightSkyBlue
        internal static System.Drawing.Color txt_Focus_Color = System.Drawing.Color.LightSkyBlue; //System.Drawing.Color.FromArgb(202, 244, 118);
        //internal static System.Drawing.Color txt_Enable_Color = System.Drawing.Color.FromArgb(236, 241, 220);
        internal static System.Drawing.Color Button_Parent_Color = System.Drawing.Color.FromArgb(198, 222, 237);
        internal static System.Drawing.Color Button_Border_Color = System.Drawing.Color.FromArgb(89, 117, 159);
        internal static System.Drawing.Color txt_Enable_Color = System.Drawing.Color.AliceBlue;

        internal static string  Program_Update_FileName = "";
        internal static int Program_Update_NewVer = 0;

        internal static int Using_Mileage_TF = 0;  //마일리지관련 프로그램 사용할지 말지        
        internal static int Using_ReturnCost_TF = 0;  //교환 관련 메뉴를 열어줄지 여부


        internal static int Using_Multi_language = 0;  //멀티 언어를 쓸지 말지 0이면 안씀 1이면 씀
        internal static string Using_language = "";  //멀티 언어를 쓸지 말지 0이면 안씀 1이면 씀
        
        


        internal static string  Mem_Number_Auto_Flag;
        internal static string Mem_Number_Auto_Base_Mbid;
        internal static string T_Company_Code;

        internal static string Sell_TF_CS_Flag;
        internal static string Sell_Union_Flag;

        internal static string Main_Select_Mbid;        
        internal static string Main_Select_Name;
        internal static string Main_Select_OrderNumber;


        internal static string Dir_Socket_Ip ;
        internal static int Dir_Socket_Acc_Port;
        internal static int Dir_Socket_Cancel_Port;

        internal static string Dir_Company_Code;
        internal static string Dir_Company_Name;
        internal static string Dir_Company_Bos_Name;
        internal static string Dir_Company_Number;
        internal static string Dir_Company_Address;
        internal static string Dir_Company_P_Number;

        internal static string SMS_smsDeptID; 

        internal static string Tel_Number_Fromat;   //전화번호 관련해서 어떤 형식을 가질건지 마스크텍스트 관련해서 셋팅을 잡기 위함.
        internal static string ZipCode_Number_Fromat;   //우편번호 관련해서 어떤 형식을 가질건지 마스크텍스트 관련해서 셋팅을 잡기 위함.
        internal static string Biz_Number_Fromat;   //사업자번호 관련해서 어떤 형식을 가질건지 마스크텍스트 관련해서 셋팅을 잡기 위함.
        internal static string Date_Number_Fromat;   //날짜 관련해서 어떤 형식을 가질건지 마스크텍스트 관련해서 셋팅을 잡기 위함.


        internal static int Rec_info_Multi_TF;
        internal static int Order_OutPut_Num_TF;

        internal static double Delivery_Standard;
        internal static double Delivery_Charge;

        /// <summary>
        /// 태국 배송비 무료 금액 기준
        /// </summary>
        internal static double Delivery_Standard_TH;
        /// <summary>
        /// 태국 배송비 부과 기준
        /// </summary>
        internal static double Delivery_Charge_TH;

        /*웹 연동 URL들*/

        internal static string ApproveAssociationURL;   //조합신고URL
        internal static string CancelAssociationURL;    //조합취소URL
        internal static string AuthURL;                 //본인인증URL
        internal static string AccountCertifyURL;       //계좌인증 URL
        internal static string AddressURL;              //우편번호URL
        internal static string ApproveCardURL;          //카드승인URL
        internal static string CancelCardURL;           //카드취소URL
        internal static string ApproveAccountURL;       //가상계좌발행URL
        internal static string CancelAccountURL;        //가상계좌취소URL
        internal static string CashReceiptURL;          //현금영수증승인 URL
        internal static string CashCancelURL;           //현금영수증취소 URL
        internal static string ApproveCardURL_TH;       //카드승인URL - 태국 // syhuh
        internal static string CancelCardURL_TH;        //카드취소URL - 태국 // syhuh
        internal static string joinMail_TH;             //회원가입 전송 메일 - 태국
        internal static string autoshipMail_TH;         //오토십 전송 메일 - 태국
        internal static string orderCompleteMail_TH;    //주문완료 전송 메일 - 태국
        internal static string orderCancelMail_TH;      //주문취소 전송 메일 - 태국
        internal static string changeNominSaveMail_TH;  //추천인/후원인 변경 처리 완료 전송 메일 - 태국


        public static void registerDLL(string dllPath)
        {
            try
            {
                Process.Start("regsvr32.exe", string.Format("-s \"{0}\"", dllPath));

                //'/s' : indicates regsvr32.exe to run silently.
                //string fileinfo = "/s" + " " + "\"" + dllPath + "\"";

                //Process reg = new Process();
                //reg.StartInfo.FileName = "regsvr32.exe";
                //reg.StartInfo.Arguments = fileinfo;
                //reg.StartInfo.UseShellExecute = false;
                //reg.StartInfo.CreateNoWindow = true;
                //reg.StartInfo.RedirectStandardOutput = true;
                //reg.Start();
                //reg.WaitForExit();
                //reg.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }// end cls_app_static_var




    class cls_User
    {
        internal const string SuperUserID = "ilsong_7";
        internal const string SuperUserPassWd = "##!!@@7";


        internal const string con_EncryptKey = "iLSong_S_C";
        internal const string con_EncryptKeyIV = "clearVector2";

        //internal const string SuperUserID_2 = "";
        //internal const string SuperUserPassWd_2 = "##!!@@7";

        internal static string gid;
        internal static string computer_ip;
        internal static string computer_net_name;
        internal static string gid_Connect_Time;

        internal static string gid_date_time;
        internal static string gid_CenterCode;

        internal static string gid_CountryCode;        

        internal static string gid_Menu1;
        internal static string gid_FarMenu;

        internal static int gid_SellInput;
        internal static int gid_Mem_Del_TF;
        internal static int gid_Sell_Del_TF;
        internal static int gid_Cpno_V_TF;
        internal static int gid_Excel_Save_TF;
        internal static int gid_For_Save_TF;
        internal static int gid_CC_Save_TF;
        internal static string gid_Tree_Config;

        internal static int gid_pan_Info_V_TF;      //메인화면상에 승인이나 조합관련 미신고된 내역을 보여주는 화면정보를 보여줄지 말지를
        internal static string gid_MACAddress; 

        internal static string uSearch_MemberNumber = "";
        internal static int gid_Cash_V_TF;
        
    }// end cls_User




    class cls_form_Meth
    {

        public void from_control_clear(Form fr, TextBox tb)
        {
            

            from_control_clear_02(fr);
            tb.Select();
        }

        public void from_control_clear(Form fr, MaskedTextBox  tb)
        {
            from_control_clear_02(fr);
            tb.Focus ();
        }

        public void from_control_clear(Form fr)
        {
            from_control_clear_02(fr);            
        }

        public void from_control_clear(Form fr, CheckBox ck)
        {
            from_control_clear_02(fr);
        }

        public void from_control_clear(Form fr, RadioButton rb)
        {
            from_control_clear_02(fr);
        }

        public void from_control_clear(GroupBox gb, TextBox tb)
        {
            from_control_clear_02(gb);
            tb.Focus();
        }

        public void from_control_clear(TabControl  Tb, TextBox tb)
        {
            from_control_clear_02(Tb);
            tb.Focus();
        }

        public void from_control_clear(TabControl Tb, MaskedTextBox tb)
        {
            from_control_clear_02(Tb);
            tb.Focus();
        }

        public void from_control_clear(TabControl Tb)
        {
            from_control_clear_02(Tb);            
        }


        public void from_control_clear(GroupBox gb, MaskedTextBox tb)
        {
            from_control_clear_02(gb);
            tb.Focus();
        }


        public void from_control_clear(Panel  fb, TextBox tb)
        {
            from_control_clear_02(fb);
            tb.Focus();
        }

        public void from_control_clear(Panel fb)
        {
            from_control_clear_02(fb);            
        }

       

        public void from_control_clear(Panel fb, MaskedTextBox tb)
        {
            from_control_clear_02(fb);
            tb.Focus();
        }



        public Control  from_Search_Control (Form fr ,  string search_name) 
        {
            Control[] controls = GetAllControls(fr);

            foreach (Control c in controls)
            {
                if (c.Name.ToString () ==  search_name)
                    return c;
            }

            return null;
        }


        private void from_control_clear_02(Form fr)
        {
            Control[] controls = GetAllControls(fr);

            foreach (Control c in controls)
            {
                if (c.Tag != null && c.Tag.ToString() == "barcord_date")
                { }
                else
                control_clear(c);
            }           
        }




        private void from_control_clear_02(GroupBox gb)
        {
            Control[] controls = GetAllControls(gb);

            foreach (Control c in controls)
            {
                control_clear(c);
            }
        }

        private void from_control_clear_02(TabControl  tb)
        {
            Control[] controls = GetAllControls(tb);

            foreach (Control c in controls)
            {
                control_clear(c);
            }
        }

        private void from_control_clear_02(Panel  gb)
        {
            Control[] controls = GetAllControls(gb);

            foreach (Control c in controls)
            {
                control_clear(c);
            }
        }



        private void control_clear(Control ct)
        {
            if (ct.Tag != null && ct.Tag.ToString() == "tab_Nation" && ct.Enabled == false)
                return;
         

            if (ct is TextBox)
            {
                TextBox cf = (TextBox)ct;
                cf.Text = "";
            }

            if (ct is MaskedTextBox)
            {
                MaskedTextBox cf = (MaskedTextBox)ct;
                cf.Text = "";
            }

            if (ct is ComboBox )
            {
                ComboBox cf = (ComboBox)ct;
                cf.Text = "";
            }

            if (ct is CheckBox)
            {
                CheckBox cf = (CheckBox)ct;
                cf.Checked = false;
            }

            if (ct is RadioButton)
            {
                RadioButton cf = (RadioButton)ct;
                cf.Checked = false;
            }

        }




        public void from_control_text_base_chang(Form fr)
        {            
            fr.Text = chang_base_caption_search(fr.Text.ToString());

            ResourceSet rs = cls_app_static_var.app_base_str_rm.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);
            IDictionaryEnumerator de = rs.GetEnumerator();

            //폼 상단바에 들어 있는 캡션을 지정한 걸로 바군다.

            de.Reset();
            while (de.MoveNext())
            {
                fr.Text = fr.Text.Replace(de.Key.ToString(), de.Value.ToString());
            }

            Control[] controls = GetAllControls(fr);

            foreach (Control c in controls)
            {
                control_t_change(c, de);

                if (cls_app_static_var.Using_Multi_language == 0)
                {
                    if (c.Name  == "tab_Nation")
                        c.Visible = false; 
                }
            }
        }


        private void control_t_change(Control ct, IDictionaryEnumerator de)
        {

            if (ct is CheckBox)
            {
                CheckBox cf = (CheckBox)ct;

                //컨트롤들의 캡션을 리소스에서 불러와서 저장된 내역을 변경한다.
                //다국어 지원일 경우에.. 다국어 연결에 편하게 하기 위함.
                cf.Text = chang_base_caption_search(cf.Text.ToString());

                //컨트롤들 캡션에 들어가 잇는 일정 문구를 지정된 문구로 변경한다.
                de.Reset();
                while (de.MoveNext())
                {
                    cf.Text = cf.Text.Replace(de.Key.ToString(), de.Value.ToString());
                }
            }

            if (ct is RadioButton)
            {
                RadioButton cf = (RadioButton)ct;
                cf.Text = chang_base_caption_search(cf.Text.ToString());

                de.Reset();
                while (de.MoveNext())
                {
                    cf.Text = cf.Text.Replace(de.Key.ToString(), de.Value.ToString());
                }                
            }

            if (ct is Label)
            {
                Label cf = (Label)ct;

                cf.Text = chang_base_caption_search(cf.Text.ToString());

                de.Reset();
                while (de.MoveNext())
                {
                    cf.Text = cf.Text.Replace(de.Key.ToString(), de.Value.ToString());
                }                
            }

            if (ct is GroupBox )
            {
                GroupBox cf = (GroupBox)ct;

                cf.Text = chang_base_caption_search(cf.Text.ToString());

                de.Reset();
                while (de.MoveNext())
                {
                    cf.Text  = cf.Text.Replace(de.Key.ToString(), de.Value.ToString());
                }
                
            }

            if (ct is Button )
            {
                Button bt = (Button)ct;

                bt.Text = chang_base_caption_search(bt.Text.ToString());

                de.Reset();
                while (de.MoveNext())
                {
                    bt.Text = bt.Text.Replace(de.Key.ToString(), de.Value.ToString());
                }

            }


            if (ct is TabControl)
            {
                int Tcnt = 0;
                TabControl tbc = (TabControl)ct;
                Tcnt = 0;
                while (Tcnt < tbc.TabPages.Count)
                {
                    tbc.TabPages[Tcnt].Text = chang_base_caption_search(tbc.TabPages[Tcnt].Text.ToString());

                    de.Reset();
                    while (de.MoveNext())
                    {
                        tbc.TabPages[Tcnt].Text = tbc.TabPages[Tcnt].Text.Replace(de.Key.ToString(), de.Value.ToString());
                    }

                    Tcnt++;
                }

                //2020-08-10 디자인코드추가
                tbc.DrawMode = TabDrawMode.OwnerDrawFixed;
                tbc.DrawItem += Tbc_DrawItem;
            }


            if (ct is DateTimePicker ) //폼로드시에 날짜 관련 셋팅을 다 현재 일자로 잡는다.
            {
                DateTimePicker cf = (DateTimePicker)ct;
                cf.Value = DateTime.Today ; 
            }
        }

        private void Tbc_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage tab = (sender as TabControl).TabPages[e.Index];
            Rectangle header = (sender as TabControl).GetTabRect(e.Index);
            using (SolidBrush darkBrush = new SolidBrush(Color.FromArgb(194, 214, 213)))
            using (SolidBrush lightBrush = new SolidBrush(Color.FromArgb(39, 126, 133)))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                if (e.State == DrawItemState.Selected)
                {
                    Font font = new Font((sender as TabControl).Font.Name, 9.25f, FontStyle.Regular);
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

        public string _chang_base_caption_search(string OldCaption)
        {
            return chang_base_Base_caption_search(chang_base_caption_search(OldCaption));
        }


        private string chang_base_caption_search(string OldCaption)
        {
            //ResourceSet rs = null ; 
            //if (cls_User.gid_CountryCode == "KR")
            //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);
            //if (cls_User.gid_CountryCode == "La")            
            //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("lo-LA")  , true, true);
            //if (cls_User.gid_CountryCode == "Ja")
            //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("ja-JP"), true, true);
            //if (cls_User.gid_CountryCode == "US")
            //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), true, true);

            //IDictionaryEnumerator de = rs.GetEnumerator();
            //de.Reset();
            //while (de.MoveNext())
            //{
            //    if (de.Key.ToString() == OldCaption)
            //    {
            //        return de.Value.ToString();
            //    }
            //}

            if  (cls_app_static_var.Base_Label.ContainsKey (OldCaption))
                return cls_app_static_var.Base_Label[OldCaption];
            else
                return OldCaption;
        }


        private string chang_base_Base_caption_search(string OldCaption)
        {

            ResourceSet rs = cls_app_static_var.app_base_str_rm.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);
            IDictionaryEnumerator de = rs.GetEnumerator();
           
           de.Reset();
           while (de.MoveNext())
           {
               OldCaption = OldCaption.Replace(de.Key.ToString(), de.Value.ToString());
           }
            return OldCaption;
        }


        public void form_DateTimePicker_Search_TextBox(Form fr, DateTimePicker dtp)
        {
            //DateTimePicker 이름을 지을때 _ 로 해서 앞뒤로 두개로 구분되게 하고 연결하는 텍스트 박스에
            //DateTimePicker의 _ 뒤쪽 명명과 동일한 명칭이 들어 가도록 해서.. 이름을 짓는다
            //해서 연결해 놓으면 동일한 이름의 텍스트 박스에 선택한 날짜가 들어가게 함.
            Control[] controls = GetAllControls(fr);

            string[] t_Name = dtp.Name.Split('_');
            string S_Txt_Name = t_Name[1];
            
            foreach (Control c in controls)
            {
                if (c is TextBox)
                {
                    TextBox tb = (TextBox)c;

                    if (
                        ((c.Name.Length - S_Txt_Name.Length) > 0) &&
                        (c.Name.Substring((c.Name.Length - S_Txt_Name.Length), S_Txt_Name.Length) == S_Txt_Name)
                        )
                    {
                        c.Text = dtp.Value.ToString("yyyyMMdd");

                        c.Focus();

                        //Control tb21 = fr.GetNextControl(fr.ActiveControl, true);
                       // tb21.Focus();
                        break;

                    }
                }

                if (c is MaskedTextBox )
                {
                    MaskedTextBox tb = (MaskedTextBox)c;

                    if (
                        ((c.Name.Length - S_Txt_Name.Length) > 0) &&
                        (c.Name.Substring((c.Name.Length - S_Txt_Name.Length), S_Txt_Name.Length) == S_Txt_Name)
                        )
                    {
                        c.Text = dtp.Value.ToString("yyyy-MM-dd");

                        c.Focus();

                        //Control tb21 = fr.GetNextControl(fr.ActiveControl, true);
                        //tb21.Focus();
                        break;

                    }
                }
            }    

        }

        public void form_DateTimePicker_Search_TextBox(Form fr, DateTimePicker dtp , Control next_focus_cn)
        {
            //DateTimePicker 이름을 지을때 _ 로 해서 앞뒤로 두개로 구분되게 하고 연결하는 텍스트 박스에
            //DateTimePicker의 _ 뒤쪽 명명과 동일한 명칭이 들어 가도록 해서.. 이름을 짓는다
            //해서 연결해 놓으면 동일한 이름의 텍스트 박스에 선택한 날짜가 들어가게 함.
            Control[] controls = GetAllControls(fr);

            string[] t_Name = dtp.Name.Split('_');
            string S_Txt_Name = t_Name[1];

            foreach (Control c in controls)
            {
                if (c is TextBox)
                {
                    TextBox tb = (TextBox)c;

                    if (
                        ((c.Name.Length - S_Txt_Name.Length) > 0) &&
                        (c.Name.Substring((c.Name.Length - S_Txt_Name.Length), S_Txt_Name.Length) == S_Txt_Name)
                        )
                    {
                        c.Text = dtp.Value.ToString("yyyyMMdd");

                        next_focus_cn.Focus();
                        break;
                    }
                }

                if (c is MaskedTextBox)
                {
                    MaskedTextBox tb = (MaskedTextBox)c;

                    if (
                        ((c.Name.Length - S_Txt_Name.Length) > 0) &&
                        (c.Name.Substring((c.Name.Length - S_Txt_Name.Length), S_Txt_Name.Length) == S_Txt_Name)
                        )
                    {
                        c.Text = dtp.Value.ToString("yyyy-MM-dd");

                        next_focus_cn.Focus();
                        break;

                    }
                }
            }
        }


        public void Search_Date_TextBox_Put(TextBox _tb1 , TextBox _tb2, RadioButton _trb)
        {
            
            string sdate = "";
            DateTime TodayDate = new DateTime();
            TodayDate = DateTime.Now;
            _tb1.Text = "";  _tb2.Text = "" ;

            if (_trb.Tag.ToString ()  == "D_1")
            {
                _tb1.Text = cls_User.gid_date_time; _tb2.Text = "";
            }

            if (_trb.Tag.ToString() == "D_7")
            {
                sdate = TodayDate.AddDays(-7).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", ""); _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "D_-1")
            {
                sdate = TodayDate.AddDays(-1).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", ""); _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_1")
            {
                sdate = cls_User.gid_date_time.Substring(0, 6) + "01";
                _tb1.Text = sdate; _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_2")
            {
                sdate = TodayDate.AddMonths(-1).ToString("yyyy/MM/dd hh:mm");
                sdate = sdate.Substring(0, 10).Replace("-", "").Substring(0, 6) + "01";
                string sdate2 = sdate.Substring(0, 6);

                switch (int.Parse(sdate.Substring(4, 2)))
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        {
                            sdate2 = sdate2 + "31";
                            break;
                        }
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        {
                            sdate2 = sdate2 + "30";
                            break;
                        }

                    case 2:
                        {
                            sdate2 = sdate2 + "28";
                            break;
                        }
                }

                _tb1.Text = sdate; _tb2.Text = sdate2;
            }

            if (_trb.Tag.ToString() == "M_3")
            {
                sdate = TodayDate.AddMonths(-2).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", "").Substring (0,6) + "01" ;        _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "T_1")
            {
                _tb1.Text = "19900101"; _tb2.Text = cls_User.gid_date_time;
            }

            _tb1.Focus();
        }


        public void Search_Date_TextBox_Put(MaskedTextBox _tb1, MaskedTextBox _tb2, RadioButton _trb)
        {

            string sdate = "";
            DateTime TodayDate = new DateTime();
            TodayDate = DateTime.Now;
            _tb1.Text = ""; _tb2.Text = "";

            if (_trb.Tag.ToString() == "D_1")
            {
                _tb1.Text = cls_User.gid_date_time; _tb2.Text = "";
            }

            if (_trb.Tag.ToString() == "D_7")
            {
                sdate = TodayDate.AddDays(-7).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", ""); _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "D_-1")
            {
                sdate = TodayDate.AddDays(-1).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", ""); _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_1")
            {
                sdate = cls_User.gid_date_time.Substring(0, 6) + "01";
                _tb1.Text = sdate; _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_2")
            {
                sdate = TodayDate.AddMonths(-1).ToString("yyyy/MM/dd hh:mm");
                sdate = sdate.Substring(0, 10).Replace("-", "").Substring(0, 6) + "01";
                string sdate2 = sdate.Substring(0, 6);

                switch (int.Parse(sdate.Substring(4, 2)))
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        {
                            sdate2 = sdate2 + "31";
                            break;
                        }
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        {
                            sdate2 = sdate2 + "30";
                            break;
                        }

                    case 2:
                        {
                            sdate2 = sdate2 + "28";
                            break;
                        }
                }

                _tb1.Text = sdate; _tb2.Text = sdate2;
            }

            if (_trb.Tag.ToString() == "M_3")
            {
                sdate = TodayDate.AddMonths(-2).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", "").Substring(0, 6) + "01"; _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_6")
            {
                sdate = TodayDate.AddMonths(-5).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", "").Substring(0, 6) + "01"; _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_9")
            {
                sdate = TodayDate.AddMonths(-8).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", "").Substring(0, 6) + "01"; _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_12")
            {
                sdate = TodayDate.AddMonths(-11).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", "").Substring(0, 6) + "01"; _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "T_1")
            {
                _tb1.Text = "19900101"; _tb2.Text = cls_User.gid_date_time;
            }

            _tb1.Focus();
        }





        public void form_Group_Panel_Enable_True(Form fr)
        {
            Control[] controls = GetAllControls(fr);
            
            foreach (Control c in controls)
            {
                if (c is GroupBox )
                {
                    GroupBox tgr = (GroupBox)c;
                    tgr.Enabled = true;
                }

                if (c is Panel)
                {
                    Panel tpn = (Panel)c;
                    tpn.Enabled = true;
                }
            }
        }

        public void form_Group_Panel_Enable_False(Form fr)
        {
            Control[] controls = GetAllControls(fr);

            foreach (Control c in controls)
            {
                if (c is GroupBox)
                {
                    GroupBox tgr = (GroupBox)c;
                    tgr.Enabled = false;
                }

                if (c is Panel )
                {
                    Panel tpn = (Panel)c;
                    tpn.Enabled = false;
                }
            }
        }


        public void form_Main_Button_Dictionary(Form fr, ref Dictionary<string, Button> Mdi_Button_dic)
        {
            Control[] controls = GetAllControls(fr);

            foreach (Control c in controls)
            {
                if (c is Button )
                {
                    Mdi_Button_dic[c.Name] = (Button)c;
                }              
            }
        }


        private Control[] GetAllControls(Control containerControl)
        {
            List<Control> allControls = new List<Control>();
            
            Queue<Control.ControlCollection> queue = new Queue<Control.ControlCollection>();

            queue.Enqueue(containerControl.Controls);            

            while (queue.Count > 0)
            {
                Control.ControlCollection controls
                            = (Control.ControlCollection)queue.Dequeue();

                if (controls == null || controls.Count == 0) continue;
                
                foreach (Control control in controls)
                {

                    allControls.Add(control);

                    queue.Enqueue(control.Controls);
                }
            }

            return allControls.ToArray();
        }


        public void Home_Number_Setting(string baseNumber, Control control)
        {
            string T_Num1, T_Num2, T_Num3;
            T_Num1 = T_Num2 = T_Num3 = string.Empty;

            Phone_Number_Split(baseNumber, ref T_Num1, ref T_Num2, ref T_Num3);

            T_Num1 = T_Num1.Trim(); 
            T_Num2 = T_Num2.Trim(); 
            T_Num3 = T_Num3.Trim();

            if (T_Num1.Length == 2)
                T_Num1 = " " + T_Num1;

            if (T_Num2.Length == 3)
                T_Num2 = " " + T_Num2;

            control.Text = string.Format("{0}-{1}-{2}", T_Num1, T_Num2, T_Num3);
        }

        public void Phone_Number_Split(string baseNumber , ref string  T_Num1 , ref string T_Num2, ref string T_Num3  )
        {
            T_Num1 = ""; T_Num2 = ""; T_Num3 = "";
            string[] T_S_Number = baseNumber.Split('-');

            //- 게 제대로 2개 들어가 잇다.
            if (T_S_Number.Length == 3)
            {
                T_Num1 = T_S_Number[0];
                T_Num2 = T_S_Number[1];
                T_Num3 = T_S_Number[2];
            }
            else
            {
                //우선 전화 번호상에 들어온 - 를 다 없앤다.. 제대로 전화 번호가 안들어 오는 경우도있기 때문에
                string t_Number = baseNumber.Trim().Replace("-", "");

                if (baseNumber.Length >= 3)
                {                    
                    if (baseNumber.Substring(0, 2) != "02")
                    {
                        if (baseNumber.Length == 11)
                        {
                            T_Num1 = baseNumber.Substring(0, 3);
                            T_Num2 = baseNumber.Substring(3, 4);
                            T_Num3 = baseNumber.Substring(7, 4);
                        }

                        if (baseNumber.Length == 10)
                        {
                            T_Num1 = baseNumber.Substring(0, 3);
                            T_Num2 = baseNumber.Substring(3, 3);
                            T_Num3 = baseNumber.Substring(6, 4);
                        }
                    }
                    else
                    {
                        if (baseNumber.Length == 10)
                        {
                            T_Num1 = baseNumber.Substring(0, 2);
                            T_Num2 = baseNumber.Substring(2, 4);
                            T_Num3 = baseNumber.Substring(6, 4);
                        }

                        if (baseNumber.Length == 9)
                        {
                            T_Num1 = baseNumber.Substring(0, 2);
                            T_Num2 = baseNumber.Substring(2, 3);
                            T_Num3 = baseNumber.Substring(5, 4);
                        }
                    }                                      

                }
            }
        }


        public void button_flat_change(Button tbt)
        {
            tbt.FlatAppearance.BorderColor = cls_app_static_var.Button_Border_Color;  //cls_app_static_var.txt_Focus_Color;
            tbt.FlatAppearance.MouseOverBackColor = cls_app_static_var.Button_Parent_Color;
            tbt.FlatAppearance.MouseDownBackColor = cls_app_static_var.txt_Focus_Color;
        }


    }//end   cls_form_Meth




    class cls_Pro_Base_Function
    {
        public void Put_SellCode_ComboBox(ComboBox cb_1, ComboBox cb_1_Code)
        {          
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select SellCode ,SellTypeName  ";
            Tsql = Tsql + " From tbl_SellType  (nolock)  ";
            Tsql = Tsql + " Order by SellCode ASC " ;                
            
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_SellType", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add ("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cb_1.Items.Add(ds.Tables["tbl_SellType"].Rows[fi_cnt]["SellTypeName"].ToString());
                cb_1_Code.Items.Add(ds.Tables["tbl_SellType"].Rows[fi_cnt]["SellCode"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }


        public void Put_NaCode_ComboBox(ComboBox cb_1, ComboBox cb_1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";
            //Tsql = "Select nationCode ,nationNameKo  ";
            // 한국인 경우
            if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
            {
                Tsql = "Select nationCode ,nationNameKo  ";
            }
            // 태국인 경우
            else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
            {
                Tsql = "Select nationCode ,nationNameEng  ";
            }

            Tsql = Tsql + " From tbl_Nation  (nolock)  ";
            Tsql = Tsql + " Where Using_TF = 1 ";
            Tsql = Tsql + " Order by nationNameKo ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_Nation", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                //cb_1.Items.Add(ds.Tables["tbl_Nation"].Rows[fi_cnt]["nationNameKo"].ToString());
                // 한국인 경우
                if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
                {
                    cb_1.Items.Add(ds.Tables["tbl_Nation"].Rows[fi_cnt]["nationNameKo"].ToString());
                }
                // 태국인 경우
                else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
                {
                    cb_1.Items.Add(ds.Tables["tbl_Nation"].Rows[fi_cnt]["nationNameEng"].ToString());
                }
                cb_1_Code.Items.Add(ds.Tables["tbl_Nation"].Rows[fi_cnt]["nationCode"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;

            //if (cls_User.gid_CountryCode != "")
            //{
            //    cb_1_Code.Text = cls_User.gid_CountryCode;
            //    cb_1.SelectedIndex = cb_1_Code.SelectedIndex;
            //    cb_1.Enabled = false;
            //    cb_1_Code.Enabled = false;
            //}
            //++++++++++++++++++++++++++++++++
        }

        public void Put_Close_Sort_ComboBox(ComboBox cb_1, ComboBox cb_1_Code)
        {

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select CloseCode ,CloseTypeName  ";
            Tsql = Tsql + " From tbl_SellType_Close  (nolock)  ";
            Tsql = Tsql + " Order by CloseCode ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_SellType_Close", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cb_1.Items.Add(ds.Tables["tbl_SellType_Close"].Rows[fi_cnt]["CloseTypeName"].ToString());
                cb_1_Code.Items.Add(ds.Tables["tbl_SellType_Close"].Rows[fi_cnt]["CloseCode"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }


        public void Put_Close_Grade_ComboBox(ComboBox cb_1, ComboBox cb_1_Code)
        {

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select Grade_Cnt ,Grade_Name  ";
            Tsql = Tsql + " From tbl_Class  (nolock)  ";
            Tsql = Tsql + " Order by Grade_Cnt ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_Class", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cb_1.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["Grade_Name"].ToString());
                cb_1_Code.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["Grade_Cnt"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }

        public void Put_Close_Grade_ComboBox(ComboBox cb_1, ComboBox cb_1_Code, int CGrade = 0)
        {

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select Grade_Cnt ,Grade_Name  ";
            Tsql = Tsql + " From tbl_Class  (nolock)  ";

            if (CGrade > 0)
                Tsql = Tsql + " Where  Grade_Cnt >= 60 ";
            Tsql = Tsql + " Order by Grade_Cnt ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_Class", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cb_1.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["Grade_Name"].ToString());
                cb_1_Code.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["Grade_Cnt"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }

        public void Put_Close_GradeP_ComboBox(ComboBox cb_1, ComboBox cb_1_Code)
        {

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select Grade_Cnt ,Grade_Name  ";
            Tsql = Tsql + " From tbl_Class_P  (nolock)  ";
            Tsql = Tsql + " Order by Grade_Cnt ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_Class", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cb_1.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["Grade_Name"].ToString());
                cb_1_Code.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["Grade_Cnt"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }


        public void Put_Rec_Code_ComboBox(ComboBox cb_1, ComboBox cb_1_Code)
        {

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            //Tsql = "Select Ncode ,Name  ";
            //Tsql = Tsql + " From tbl_Base_Rec  (nolock)  ";
            //Tsql = Tsql + " Order by Ncode ASC ";

            Tsql = "Select M_Detail , " + cls_app_static_var.Base_M_Detail_Ex  + " AS  M_Name";
            Tsql = Tsql + " From tbl_Base_Change_Detail (nolock) ";
            Tsql = Tsql + " Where M_Detail_S = 'tbl_Sales_Rece' ";

            //strSql = strSql + " , Isnull(tbl_Base_Rec.name ,'' ) Base_Rec_Name ";
            //strSql = strSql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " Receive_Method_Name ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_Base_Rec", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cb_1.Items.Add(ds.Tables["tbl_Base_Rec"].Rows[fi_cnt]["M_Name"].ToString());
                cb_1_Code.Items.Add(ds.Tables["tbl_Base_Rec"].Rows[fi_cnt]["M_Detail"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }




        public void Put_Address_Sort_Area(string Address, ref string Area)
        {
            if (Address.Length < 2)
            {
                Area = "모름"; return;
            }

            string T_ad = Address.Replace(" ", "").Substring(0, 2);

            if (T_ad.Contains("서울") == true)
            {
                Area = "서울"; return;
            }

            if (T_ad.Contains("부산") == true)
            {
                Area = "부산"; return;
            }

            if (T_ad.Contains("인천") == true)
            {
                Area = "인천"; return;
            }

            if (T_ad.Contains("광주") == true)
            {
                Area = "광주"; return;
            }

            if (T_ad.Contains("대전") == true)
            {
                Area = "대전"; return;
            }
            if (T_ad.Contains("대구") == true)
            {
                Area = "대구"; return;
            }

            if (T_ad.Contains("울산") == true)
            {
                Area = "울산"; return;
            }

            if (T_ad.Contains("세종") == true)
            {
                Area = "세종"; return;
            }

            if (T_ad.Contains("경기") == true)
            {
                Area = "경기"; return;
            }

            if (T_ad.Contains("강원") == true)
            {
                Area = "강원"; return;
            }

            if (T_ad.Contains("제주") == true)
            {
                Area = "제주"; return;
            }

            if (T_ad.Contains("충청북도") == true || T_ad.Contains("충북") == true)
            {
                Area = "충북"; return;
            }

            if (T_ad.Contains("충청남도") == true || T_ad.Contains("충남") == true)
            {
                Area = "충남"; return;
            }

            if (T_ad.Contains("전라남도") == true || T_ad.Contains("전남") == true)
            {
                Area = "전남"; return;
            }

            if (T_ad.Contains("전라북도") == true || T_ad.Contains("전북") == true)
            {
                Area = "전북"; return;
            }


            if (T_ad.Contains("경상북도") == true || T_ad.Contains("경북") == true)
            {
                Area = "경북"; return;
            }

            if (T_ad.Contains("경상남도") == true || T_ad.Contains("경남") == true)
            {
                Area = "경남"; return;
            }


        }

    }//cls_Sell_Base_Function








    class cls_WATCrypt
    {
        byte[] Skey = new byte[8];

        public cls_WATCrypt(string strKey)
        {
            Skey = ASCIIEncoding.ASCII.GetBytes(strKey);
        }

        public string Encrypt(string p_data)
        {
            //DESCryptoServiceProvider rc2 = new DESCryptoServiceProvider();

            //rc2.Key = Skey;
            //rc2.IV = Skey;

            //MemoryStream ms = new MemoryStream();
            //CryptoStream cryStream = new CryptoStream(ms, rc2.CreateEncryptor(),
            //CryptoStreamMode.Write);

            //byte[] data = Encoding.UTF8.GetBytes(p_data.ToCharArray());
            //cryStream.Write(data, 0, data.Length);
            //cryStream.FlushFinalBlock();

            //return Convert.ToBase64String(ms.ToArray());

           

            DESCryptoServiceProvider rc2 = new DESCryptoServiceProvider(); 
            
            rc2.Key = Skey;
            rc2.IV = Skey;
            
            MemoryStream ms = new MemoryStream();
            CryptoStream cryStream = new CryptoStream(ms, rc2.CreateEncryptor(), 
                CryptoStreamMode.Write);

            byte[] data = Encoding.UTF8.GetBytes(p_data.ToCharArray());            
            cryStream.Write(data, 0, data.Length);
            cryStream.FlushFinalBlock();            

            return Convert.ToBase64String(ms.ToArray());             
        }

        public string Decrypt(string p_data)
        {
            //DESCryptoServiceProvider rc2 = new DESCryptoServiceProvider();

            //rc2.Key = Skey;
            //rc2.IV = Skey;

            //MemoryStream ms = new MemoryStream();
            //CryptoStream cryStream = new CryptoStream(ms, rc2.CreateEncryptor(),
            //CryptoStreamMode.Write);

            //byte[] data = Convert.FromBase64String(p_data);
            //cryStream.Write(data, 0, data.Length);
            //cryStream.FlushFinalBlock();

            //return Encoding.UTF8.GetString(ms.GetBuffer());
            DESCryptoServiceProvider rc2 = new DESCryptoServiceProvider();
                        
            rc2.Key = Skey;
            rc2.IV = Skey;
            
            MemoryStream ms = new MemoryStream();
            CryptoStream cryStream = new CryptoStream(ms, rc2.CreateDecryptor(),
                CryptoStreamMode.Write);

            byte[] data = Convert.FromBase64String(p_data);            
            cryStream.Write(data, 0, data.Length);
            cryStream.FlushFinalBlock();

            return Encoding.UTF8.GetString(ms.GetBuffer()); 
            
        }

    }


}
