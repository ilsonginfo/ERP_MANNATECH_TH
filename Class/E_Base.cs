using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM_Program
{
    public enum EWebImageType
    {
        IDCARD,		// 신분증
        BANKBOOK,   // 통장
        ParentAgreeDoc, // 보호자 가입동의서
        ParentIDCard	// 보호자 신분증
    }

    public enum ESendMailType_TH
    {
        /// <summary>
        /// 회원가입
        /// </summary>
        joinMail,
        /// <summary>
        /// 오토십
        /// </summary>
        autoshipMail,
        /// <summary>
        /// 주문완료
        /// </summary>
        orderCompleteMail,
        /// <summary>
        /// 주문취소
        /// </summary>
        orderCancelMail,
        /// <summary>
        /// 추천인/후원인 변경 처리 완료
        /// </summary>
        changeNominSaveMail
    }

}
