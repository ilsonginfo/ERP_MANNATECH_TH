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

}
