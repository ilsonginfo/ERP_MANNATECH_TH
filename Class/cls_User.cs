using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM_Program
{
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
        internal static bool IsAdmin
        {
            get
            {
                bool Ret = false;

                if (gid.ToLower() == "admin") Ret = true;
                else if (gid.ToLower() == "admin_th") Ret = true;

                return Ret;
            }
        }



    }// end cls_User
}
