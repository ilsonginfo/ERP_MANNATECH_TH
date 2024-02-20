using System;
using System.Collections.Generic;
using System.Text;

namespace MLM_Program
{
    /// <summary>
    /// 문자를 보낼때 사용함
    /// </summary>
    class cls_sms
    {
        /// <summary>
        /// 회원 등록 문자 - 태국
        /// </summary>
        /// <param name="mbid2"></param>
        public void SMS_JoinMember_TH(object mbid2)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string StrSql = "EXEC Usp_TH_SMS  '" + mbid2.ToString() + "', '', '', '7'";
            Temp_Connect.Update_Data(StrSql, "", "");
        }

        /// <summary>
        /// 1.회원가입 문자보내기                               
        /// EXEC Usp_Insert_mannatech_SMS'10', '', 회원번호2, '', ''    
        /// </summary>
        public void Congratulations_Membership(object mbid2)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            //string StrSql = "EXEC Usp_Insert_SMS '10', '', " + mbid2.ToString() + ", '', ''";
            string StrSql = "EXEC Usp_Insert_SMS_New  '10', '', " + mbid2.ToString() + ", '', ''";
            Temp_Connect.Update_Data(StrSql, "", "");

        }
        public void Coupon_Membership(object mbid2)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string StrSql = "EXEC Usp_Insert_SMS_New  '50', '', " + mbid2.ToString() + ", '', ''";
            //string StrSql = "EXEC Usp_Insert_SMS '50', '', " + mbid2.ToString() + ", '', ''";
            Temp_Connect.Update_Data(StrSql, "", "");

        }

        /// <summary>
        /// 2.가상계좌 문자                     
        /// EXEC Usp_Insert_mannatech_SMS'20', '회원번호1', 회원번호2, '주문번호', ''       
        /// </summary>
        public void Here_Ur_VA(object mbid2, string ordernumber)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            //string StrSql = "EXEC Usp_Insert_SMS '20', '', " + mbid2.ToString() + ", '"+ ordernumber + "', ''";
            string StrSql = "EXEC Usp_Insert_SMS_New  '20', '', " + mbid2.ToString() + ", '" + ordernumber + "', ''";
            Temp_Connect.Update_Data(StrSql, "", "");
        }

        /// <summary>
        /// 4.오토십 결제실패  
        /// EXEC Usp_Insert_mannatech_SMS'40', '회원번호1', 회원번호2, 'MM월 dd일', '' 
        /// </summary>
        /// <param name="mbid2"></param>
        public void AutoshipPaymentFail(object mbid2)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string StrSql = "EXEC Usp_Insert_SMS_New  '20', '', " + mbid2.ToString() + ", '" + DateTime.Now.ToString("MM월 dd일") + "', ''";
            //string StrSql = "EXEC Usp_Insert_SMS '20', '', " + mbid2.ToString() + ", '" + DateTime.Now.ToString("MM월 dd일") + "', ''";
            Temp_Connect.Update_Data(StrSql, "", "");
        }

    }
}
