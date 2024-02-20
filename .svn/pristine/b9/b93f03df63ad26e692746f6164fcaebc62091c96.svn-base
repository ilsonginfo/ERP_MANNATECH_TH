using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace MLM_Program
{
    class KSPayApprovalCancelBean_2cs
    {

        //www.kspay.co.kr
        //ID: 2295100005	
        //PW: 71117kspay


        //private string Ksn_storeid = "2999199999"; // 테스트 상점 아이디        
        //private string Ksn_storeid_Card = "2999199999"; // 테스트 상점 아이디   
        private string Ksn_storeid = "2001105929"; // 에이필드        
        private string Ksn_storeid_Card = "2001105929"; // 에이필드

        private string Ksn_storeid_WebCard = "2001105933"; // 에이필드


        //////private string Ksn_storeid = "2295100001"; // 타임앤로우
        //////private string Ksn_storeid = "2295100005"; // 카리스

        private string Cancel_authty = "1010"; //승인취소 승인구분코드

        private string Cancel_Bank_authty = "6010"; //가상계좌 취소 승인구분코드
        private string Bank_authty = "6000"; //가상계좌 취소 승인구분코드





        

        private string App_authty = "1300"; //승인 승인구분코드
        private string App_certitype = "N";

        // private string App_authty = "1000"; //승인 승인구분코드
        // private string App_certitype = "K";

        public string KSPayCancelPost(string T_ID ,string Ksn_TrNo, ref string T_rStatus, ref string T_rAuthNo, string S_TF = "", string CardNo = "")
        {
            //'Header부 Data --------------------------------------------------
            string EncType = "2";// 0: 암화안함, 2: seed

            string Version = "";
            //if (S_TF == "")
            Version = "0311";// 전문버전(가상계좌는 0603)

            if (S_TF == "B")
                Version = "0603";// 전문버전(가상계좌는 0603)


            string Type = "00";// 구분
            string Resend = "0";// 전송구분 : 0 : 처음,  1: 재전송
            string RequestDate = DateTime.Now.ToString("yyyyMMddhhmmss");// 요청일자 : yyyymmddhhmmss
            string KeyInType = "K";// KeyInType 여부 : S : Swap, K: KeyInType
            string LineType = "1";// lineType 0 : offline, 1:internet, 2:Mobile
            string ApprovalCount = "1";// 복합승인갯수
            string GoodType = "1";// 제품구분 1 : 실물, 2 : 디지털
            string HeadFiller = "";// 예비
            //' Header (입력값 (*) 필수항목)--------------------------------------------------
            string StoreId = "";// *상점아이디
            if (T_ID == "1")
                StoreId = Ksn_storeid_WebCard;// *상점아이디
            else
                StoreId = Ksn_storeid_Card;// *상점아이디

            string OrderNumber = "";// *주문번호
            string UserName = "";// *주문자명
            string IdNum = "";// 주민번호 or 사업자번호
            string Email = "";// *email
            string GoodName = "";// *제품명
            string PhoneNo = "";// *휴대폰번호
            //'Header end -------------------------------------------------------------------

            //'Data Default------------------------------------------------------------------
            string ApprovalType = "";
            if (S_TF == "")
                ApprovalType = Cancel_authty;//' 승인구분 코드    

            if (S_TF == "B")
                ApprovalType = Cancel_Bank_authty;//' 승인구분 코드    

            String TrNo = Ksn_TrNo;//' 거래번호                                          

            //' Server로 부터 응답이 없을시 자체응답
            string rApprovalType = "1001";
            string rTransactionNo = "";// 거래번호
            string rStatus = "X";// 상태 O : 승인, X : 거절
            string rTradeDate = "";// 거래일자
            string rTradeTime = "";// 거래시간
            string rIssCode = "00";// 발급사코드
            string rAquCode = "00";// 매입사코드
            string rAuthNo = "9999";// 승인번호 or 거절시 오류코드
            string rMessage1 = "승인거절";// 메시지1
            string rMessage2 = "C잠시후재시도";// 메시지2
            string rCardNo = "";// 카드번호
            string rExpDate = "";// 유효기간
            string rInstallment = "";// 할부
            string rAmount = "";// 금액
            string rMerchantNo = "";// 가맹점번호
            string rAuthSendType = "N";// 전송구분
            string rApprovalSendType = "N";// 전송구분(0 : 거절, 1 : 승인, 2: 원카드)
            string rPoint1 = "000000000000";// Point1
            string rPoint2 = "000000000000";// Point2
            string rPoint3 = "000000000000";// Point3
            string rPoint4 = "000000000000";// Point4
            string rVanTransactionNo = "";// 
            string rFiller = "";// 예비
            string rAuthType = "";// ISP : ISP거래, MP1, MP2 : MPI거래, SPACE : 일반거래
            string rMPIPositionType = "";// K : KSNET, R : Remote, C : 제3기관, SPACE : 일반거래
            string rMPIReUseType = "";// Y : 재사용, N : 재사용아님
            string rEncData = "";// MPI, ISP 데이터
            //' --------------------------------------------------------------------------------

            KSPayApprovalCancelBean ipg = new KSPayApprovalCancelBean("220.117.241.175", 21000);


            //Header부 전문조립
            ipg.HeadMessage(EncType, Version, Type, Resend, RequestDate, StoreId, OrderNumber, UserName, IdNum, Email, GoodType, GoodName, KeyInType, LineType, PhoneNo, ApprovalCount, HeadFiller);

            ipg.CancelDataMessage(ApprovalType, "0", TrNo, "", "", "", "", "");

            if (ipg.SendSocket("1"))
            {

                if (S_TF == "")
                {
                    rApprovalType = ipg.ApprovalType;// 승인구분코드(서비스종류를 구분할수 있습니다. 첨부된전문내역서상의 승인코드부 참조)
                    rTransactionNo = ipg.TransactionNo;// 거래번호
                    rStatus = ipg.Status;// 상태 O : 승인, X : 거절
                    rTradeDate = ipg.TradeDate;// 거래일자
                    rTradeTime = ipg.TradeTime;// 거래시간
                    rIssCode = ipg.IssCode;// 발급사코드
                    rAquCode = ipg.AquCode;// 매입사코드
                    rAuthNo = ipg.AuthNo;// 승인번호 or 거절시 오류코드
                    rMessage1 = ipg.Message1;// 메시지1
                    rMessage2 = ipg.Message2;// 메시지2
                    rCardNo = ipg.CardNo;// 카드번호
                    rExpDate = ipg.ExpDate;// 유효기간
                    rInstallment = ipg.Installment;// 할부
                    rAmount = ipg.Amount;// 금액
                    rMerchantNo = ipg.MerchantNo;// 가맹점번호
                    rAuthSendType = ipg.AuthSendType;// 전송구분= new String(this.read(2))
                    rApprovalSendType = ipg.ApprovalSendType;// 전송구분(0 : 거절, 1 : 승인, 2: 원카드)
                }

                if (S_TF == "B")
                {
                    rTransactionNo = ipg.VATransactionNo;//거래번호
                    rStatus = ipg.VAStatus;//O , X
                    rTradeDate = ipg.VATradeDate;//발급일자
                    rTradeTime = ipg.VATradeTime;//발급시간
                    rMessage1 = ipg.VAMessage1;
                    rMessage2 = ipg.VAMessage2;
                    rAmount = ipg.Amount;
                    //rVAFiller = ipg.VAFiller;//금액
                }

                T_rStatus = rStatus;
                T_rAuthNo = rAuthNo;
                return rTransactionNo; // 거래번호      //저장해야함 승인취소시 사용됨.
            }
            else
            {
                return "";
            }
        }



        public string KSPayCancelPost(string T_ID, string Ksn_TrNo, ref string T_rStatus, ref string T_rAuthNo, int C_C_Price1, string seq_1)
        {
            //'Header부 Data --------------------------------------------------
            string EncType = "2";// 0: 암화안함, 2: seed

            string Version = "";
            //if (S_TF == "")
            Version = "0311";// 전문버전(가상계좌는 0603)

           

            string Type = "00";// 구분
            string Resend = "0";// 전송구분 : 0 : 처음,  1: 재전송
            string RequestDate = DateTime.Now.ToString("yyyyMMddhhmmss");// 요청일자 : yyyymmddhhmmss
            string KeyInType = "K";// KeyInType 여부 : S : Swap, K: KeyInType
            string LineType = "1";// lineType 0 : offline, 1:internet, 2:Mobile
            string ApprovalCount = "1";// 복합승인갯수
            string GoodType = "1";// 제품구분 1 : 실물, 2 : 디지털
            string HeadFiller = "";// 예비
            //' Header (입력값 (*) 필수항목)--------------------------------------------------
            string StoreId = "";// *상점아이디
            if (T_ID == "1")
                StoreId = Ksn_storeid_WebCard;// *상점아이디
            else
                StoreId = Ksn_storeid_Card;// *상점아이디

            string OrderNumber = "";// *주문번호
            string UserName = "";// *주문자명
            string IdNum = "";// 주민번호 or 사업자번호
            string Email = "";// *email
            string GoodName = "";// *제품명
            string PhoneNo = "";// *휴대폰번호
            //'Header end -------------------------------------------------------------------

            //'Data Default------------------------------------------------------------------
            string ApprovalType = "";            
            ApprovalType = Cancel_authty;//' 승인구분 코드    

            String TrNo = Ksn_TrNo;//' 거래번호    

            String Canc_amt = C_C_Price1.ToString();	//' 취소금액
            String Canc_seq = seq_1 ;	//' 취소일련번호
            String Canc_type = "3";	//' 취소유형 0 :거래번호취소 1: 주문번호취소 3:부분취소
              

            //' Server로 부터 응답이 없을시 자체응답
            string rApprovalType = "1001";
            string rTransactionNo = "";// 거래번호
            string rStatus = "X";// 상태 O : 승인, X : 거절
            string rTradeDate = "";// 거래일자
            string rTradeTime = "";// 거래시간
            string rIssCode = "00";// 발급사코드
            string rAquCode = "00";// 매입사코드
            string rAuthNo = "9999";// 승인번호 or 거절시 오류코드
            string rMessage1 = "승인거절";// 메시지1
            string rMessage2 = "C잠시후재시도";// 메시지2
            string rCardNo = "";// 카드번호
            string rExpDate = "";// 유효기간
            string rInstallment = "";// 할부
            string rAmount = "";// 금액
            string rMerchantNo = "";// 가맹점번호
            string rAuthSendType = "N";// 전송구분
            string rApprovalSendType = "N";// 전송구분(0 : 거절, 1 : 승인, 2: 원카드)
            string rPoint1 = "000000000000";// Point1
            string rPoint2 = "000000000000";// Point2
            string rPoint3 = "000000000000";// Point3
            string rPoint4 = "000000000000";// Point4
            string rVanTransactionNo = "";// 
            string rFiller = "";// 예비
            string rAuthType = "";// ISP : ISP거래, MP1, MP2 : MPI거래, SPACE : 일반거래
            string rMPIPositionType = "";// K : KSNET, R : Remote, C : 제3기관, SPACE : 일반거래
            string rMPIReUseType = "";// Y : 재사용, N : 재사용아님
            string rEncData = "";// MPI, ISP 데이터
            //' --------------------------------------------------------------------------------

            KSPayApprovalCancelBean ipg = new KSPayApprovalCancelBean("220.117.241.175", 21000);


            //Header부 전문조립
            ipg.HeadMessage(EncType, Version, Type, Resend, RequestDate, StoreId, OrderNumber, UserName, IdNum, Email, GoodType, GoodName, KeyInType, LineType, PhoneNo, ApprovalCount, HeadFiller);

            //ipg.CancelDataMessage(ApprovalType, "0", TrNo, "", "", "", "", "");
            ipg.CancelDataMessage(ApprovalType, Canc_type, TrNo, "", "", KSPayApprovalCancelBean.format(Canc_amt, 9, '9') + KSPayApprovalCancelBean.format(Canc_seq, 2, '9'), "", "");
            if (ipg.SendSocket("1"))
            {

                
                rApprovalType = ipg.ApprovalType;// 승인구분코드(서비스종류를 구분할수 있습니다. 첨부된전문내역서상의 승인코드부 참조)
                rTransactionNo = ipg.TransactionNo;// 거래번호
                rStatus = ipg.Status;// 상태 O : 승인, X : 거절
                rTradeDate = ipg.TradeDate;// 거래일자
                rTradeTime = ipg.TradeTime;// 거래시간
                rIssCode = ipg.IssCode;// 발급사코드
                rAquCode = ipg.AquCode;// 매입사코드
                rAuthNo = ipg.AuthNo;// 승인번호 or 거절시 오류코드
                rMessage1 = ipg.Message1;// 메시지1
                rMessage2 = ipg.Message2;// 메시지2
                rCardNo = ipg.CardNo;// 카드번호
                rExpDate = ipg.ExpDate;// 유효기간
                rInstallment = ipg.Installment;// 할부
                rAmount = ipg.Amount;// 금액
                rMerchantNo = ipg.MerchantNo;// 가맹점번호
                rAuthSendType = ipg.AuthSendType;// 전송구분= new String(this.read(2))
                rApprovalSendType = ipg.ApprovalSendType;// 전송구분(0 : 거절, 1 : 승인, 2: 원카드)
               

                T_rStatus = rStatus;
                T_rAuthNo = rAuthNo;
                return rTransactionNo; // 거래번호      //저장해야함 승인취소시 사용됨.
            }
            else
            {
                return "";
            }
        }



        //주문번호, 사용자 성명, 주민번호(안보내도됨), 일반/무이자구분 1:일반 2:무이자 , 카드번호, 유효기간(년월), 할부(00일시불), 결제금액, 통화구분 0:원화 1: 미화
        public string KSPayCreditPostMNI(string OrderNo, string U_Name, string Cpno, string T_interest, string CardNo
                                        , string Card_Per
                                        , string HalBu
                                        , int Send_Amount
                                        , string T_Passwd
                                        , string T_Birth
                                        , string T_currencytype
                                        , ref string T_rStatus
                                        , ref string T_rAuthNo
                                        , ref string T_Er_Msg
                                        )
        {
            // Default(수정항목이 아님)-------------------------------------------------------
            string EncType = "2";// 0: 암화안함, 2: seed
            string Version = "0311";// 전문버전(가상계좌는 0603)
            string Type = "00";// 구분
            string Resend = "0";// 전송구분 : 0 : 처음,  1: 재전송
            string RequestDate = DateTime.Now.ToString("yyyyMMddhhmmss");// 요청일자 : yyyymmddhhmmss
            string KeyInType = "K";// KeyInType 여부 : S : Swap, K: KeyInType
            string LineType = "1";// lineType 0 : offline, 1:internet, 2:Mobile
            string ApprovalCount = "1";// 복합승인갯수
            string GoodType = "1";// 제품구분 1 : 실물, 2 : 디지털
            string HeadFiller = "";// 예비
            //-------------------------------------------------------------------------------

            // Data Default end -------------------------------------------------------------
            //승인타입	 : A-인증없는승인, N-인증승인, M-Visa3D인증승인, I-ISP인증승인 
            string certitype = App_certitype;

            //Header부 Data --------------------------------------------------
            string StoreId = Ksn_storeid_Card;// *상점아이디
            string OrderNumber = OrderNo;// *주문번호
            string UserName = U_Name;// *주문자명
            string IdNum = Cpno;// 주민번호 or 사업자번호
            string Email = "";// *email
            string GoodName = OrderNo;// *제품명
            string PhoneNo = "";// *휴대폰번호
            //Header end -------------------------------------------------------------------

            //Data Default-------------------------------------------------
            string ApprovalType = App_authty;// 승인구분
            string InterestType = T_interest;// 일반/무이자구분 1:일반 2:무이자
            string TrackII = CardNo + "=" + Card_Per;// 카드번호=유효기간(년월)  or 거래번호  
            string Installment = HalBu;// 할부  00일시불
            string Amount = Send_Amount.ToString();// 금액
            string Passwd = T_Passwd;// 비밀번호 앞2자리
            string LastIdNum = T_Birth;// 주민번호  뒤7자리, 사업자번호10
            string CurrencyType = T_currencytype;// 통화구분 0:원화 1: 미화

            string BatchUseType = "0";// 거래번호배치사용구분  0:미사용 1:사용
            string CardSendType = "2";// 카드정보전송유무 '0:미전송 1:카드번호,유효기간,할부,금액,가맹점번호 2:카드번호앞14자리 + "XXXX",유효기간,할부,금액,가맹점번호
            string VisaAuthYn = "7";// 비자인증유무 0:사용안함,7:SSL,9:비자인증
            string Domain = "";// 도메인 자체가맹점(PG업체용)
            string IpAddr = "";// IP ADDRESS 자체가맹점(PG업체용)
            string BusinessNumber = "";// 사업자 번호 자체가맹점(PG업체용)
            string Filler = "";// 예비
            string AuthType = "";// ISP : ISP거래, MP1, MP2 : MPI거래, SPACE : 일반거래
            string MPIPositionType = "";// K : KSNET, R : Remote, C : 제3기관, SPACE : 일반거래
            string MPIReUseType = "";// Y : 재사용, N : 재사용아님
            string EncData = "";// MPI, ISP 데이터

            string cavv = "";// MPI용
            string xid = "";// MPI용
            string eci = "";// MPI용

            string KVP_PGID = "";
            string KVP_CARDCODE = "";
            string KVP_SESSIONKEY = "";
            string KVP_ENCDATA = "";

            //Data Default end -------------------------------------------------------------

            //Server로 부터 응답이 없을시 자체응답
            string rApprovalType = "1001";
            string rTransactionNo = "";// 거래번호
            string rStatus = "X";// 상태 O : 승인, X : 거절
            string rTradeDate = "";// 거래일자
            string rTradeTime = "";// 거래시간
            string rIssCode = "00";// 발급사코드
            string rAquCode = "00";// 매입사코드
            string rAuthNo = "9999";// 승인번호 or 거절시 오류코드
            string rMessage1 = "승인거절";// 메시지1
            string rMessage2 = "C잠시후재시도";// 메시지2
            string rCardNo = "";// 카드번호
            string rExpDate = "";// 유효기간
            string rInstallment = "";// 할부
            string rAmount = "";// 금액
            string rMerchantNo = "";// 가맹점번호
            string rAuthSendType = "N";// 전송구분
            string rApprovalSendType = "N";// 전송구분(0 : 거절, 1 : 승인, 2: 원카드)
            string rPoint1 = "000000000000";// Point1
            string rPoint2 = "000000000000";// Point2
            string rPoint3 = "000000000000";// Point3
            string rPoint4 = "000000000000";// Point4
            string rVanTransactionNo = "";// 
            string rFiller = "";// 예비
            string rAuthType = "";// ISP : ISP거래, MP1, MP2 : MPI거래, SPACE : 일반거래
            string rMPIPositionType = "";// K : KSNET, R : Remote, C : 제3기관, SPACE : 일반거래
            string rMPIReUseType = "";// Y : 재사용, N : 재사용아님
            string rEncData = "";// MPI, ISP 데이터
            //--------------------------------------------------------------------------------------

            try
            {
                KSPayApprovalCancelBean ipg = new KSPayApprovalCancelBean("220.117.241.175", 21000);
                //ksnet.kspay.KSPayApprovalCancelBean ipg = new ksnet.kspay.KSPayApprovalCancelBean("localhost", 29991);

                //Header부 전문조립
                ipg.HeadMessage(EncType, Version, Type, Resend, RequestDate, StoreId, OrderNumber, UserName, IdNum, Email, GoodType, GoodName, KeyInType, LineType, PhoneNo, ApprovalCount, HeadFiller);

                //일반승인인경우
                if (certitype.Equals("A") || certitype.Equals("N"))
                {
                    AuthType = "";
                    MPIPositionType = "";
                    MPIReUseType = "";
                    EncData = "";
                }
                else
                    if (certitype.Equals("M"))// Visa3d인증승인인경우
                    {
                        AuthType = "M";
                        MPIPositionType = "K";
                        MPIReUseType = "N";
                        cavv = KSPayApprovalCancelBean.format(cavv, 40, 'X');
                        xid = KSPayApprovalCancelBean.format(xid, 40, 'X');
                        eci = KSPayApprovalCancelBean.format(eci, 2, 'X');
                        EncData = KSPayApprovalCancelBean.format("" + (cavv + xid + eci).Length, 5, '9') + cavv + xid + eci;
                    }
                    else
                        if (certitype.Equals("I"))// ISP인증승인인경우
                        {
                            TrackII = "";
                            //InterestType = "";// 무이자구분
                            //Installment = Request.Form["KVP_QUOTA"];// 할부:00일시불
                            //KVP_PGID = Request.Form["KVP_PGID"];
                            //KVP_CARDCODE = Request.Form["KVP_CARDCODE"];
                            //KVP_SESSIONKEY = Request.Form["KVP_SESSIONKEY"];
                            //KVP_ENCDATA = Request.Form["KVP_ENCDATA"];

                            //InterestType = InterestType.Equals("0") ? "1" : "2";

                            //AuthType = "I";
                            //MPIPositionType = "K";
                            //MPIReUseType = "N";

                            //KVP_SESSIONKEY = System.Web.HttpUtility.UrlEncode(KVP_SESSIONKEY, System.Text.Encoding.GetEncoding("euc-kr"));
                            //KVP_ENCDATA = System.Web.HttpUtility.UrlEncode(KVP_ENCDATA, System.Text.Encoding.GetEncoding("euc-kr"));
                            //KVP_SESSIONKEY = KSPayApprovalCancelBean.format("" + KVP_SESSIONKEY.Length, 4, '9') + KVP_SESSIONKEY;
                            //KVP_ENCDATA = KSPayApprovalCancelBean.format("" + KVP_ENCDATA.Length, 4, '9') + KVP_ENCDATA;
                            //KVP_CARDCODE = KSPayApprovalCancelBean.format((KSPayApprovalCancelBean.format("" + KVP_CARDCODE.Length, 2, '9') + KVP_CARDCODE), 22, 'X');
                            //EncData = KSPayApprovalCancelBean.format("" + (KVP_PGID + KVP_SESSIONKEY + KVP_ENCDATA + KVP_CARDCODE).Length, 5, '9') + KVP_PGID + KVP_SESSIONKEY + KVP_ENCDATA + KVP_CARDCODE;
                        }

                ////if (CurrencyType.Equals("USD") || CurrencyType.Equals("840"))
                ////{
                ////    CurrencyType = "1";
                ////}
                ////else
                ////{
                ////    CurrencyType = "0";
                ////}

                //Data부 전문조립
                ipg.CreditDataMessage(ApprovalType, InterestType, TrackII, Installment, Amount, Passwd, LastIdNum, CurrencyType, BatchUseType, CardSendType, VisaAuthYn, Domain, IpAddr, BusinessNumber, Filler, AuthType, MPIPositionType, MPIReUseType, EncData);

                //KSPAY로 요청전문송신후 수신데이터 파싱
                if (ipg.SendSocket("1"))
                {
                    //            // 신용카드승인결과
                    //public string ApprovalType,//	승인구분
                    //                TransactionNo,//	거래번호
                    //                Status,//	상태 O : 승인 ,	X :	거절
                    //                TradeDate,//	거래일자
                    //                TradeTime,//	거래시간
                    //                IssCode,//	발급사코드
                    //                AquCode,//	매입사코드
                    //                AuthNo,//	승인번호 or	거절시 오류코드
                    //                Message1,//	메시지1
                    //                Message2,//	메시지2
                    //                CardNo,//	카드번호
                    //                ExpDate,//	유효기간
                    //                Installment,//	할부
                    //                Amount,//	금액
                    //                MerchantNo,//	가맹점번호
                    //                AuthSendType,//	전송구분
                    //                ApprovalSendType,//	전송구분(0 : 거절, 1 : 승인, 2:	원카드)
                    //                Point1,//
                    //                Point2,//
                    //                Point3,//
                    //                Point4,//
                    //                VanTransactionNo,//	Van	거래번호
                    //                Filler,//	예비
                    //                AuthType,//	ISP	: ISP거래, MP1,	MP2	: MPI거래, SPACE : 일반거래
                    //                MPIPositionType,//	K :	KSNET, R : Remote, C : 제3기관,	SPACE :	일반거래
                    //                MPIReUseType,//	Y :	재사용,	N :	재사용아님
                    //                EncData;//	MPI, ISP 데이터


                    rApprovalType = ipg.ApprovalType;// 승인구분코드(서비스종류를 구분할수 있습니다. 첨부된전문내역서상의 승인코드부 참조)
                    rTransactionNo = ipg.TransactionNo;// 거래번호      //저장해야함 승인취소시 사용됨.
                    rStatus = ipg.Status;// 상태 O : 승인, X : 거절
                    rTradeDate = ipg.TradeDate;// 거래일자
                    rTradeTime = ipg.TradeTime;// 거래시간
                    rIssCode = ipg.IssCode;// 발급사코드
                    rAquCode = ipg.AquCode;// 매입사코드
                    rAuthNo = ipg.AuthNo;// 승인번호 or 거절시 오류코드
                    rMessage1 = ipg.Message1;// 메시지1  
                    rMessage2 = ipg.Message2;// 메시지2
                    rCardNo = ipg.CardNo;// 카드번호
                    rExpDate = ipg.ExpDate;// 유효기간
                    rInstallment = ipg.Installment;// 할부
                    rAmount = ipg.Amount;// 금액
                    rMerchantNo = ipg.MerchantNo;// 가맹점번호
                    rAuthSendType = ipg.AuthSendType;// 전송구분= new String(this.read(2))
                    rApprovalSendType = ipg.ApprovalSendType;// 전송구분(0 : 거절, 1 : 승인, 2: 원카드)

                    T_rStatus = rStatus;
                    T_rAuthNo = rAuthNo;
                    T_Er_Msg = rMessage1 + " " + rMessage2 + " " + rAuthNo;
                    return rTransactionNo; // 거래번호      //저장해야함 승인취소시 사용됨.
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {

                KSPayApprovalCancelBean.write_log("ERROR : Exception [" + e.StackTrace + "]!!!");
                return "";
            }

        }


        public string KSPayCashPostMNI(string OrderNo, string U_Name, string Cpno
                                        , int Send_Amount
                                        , int T_pSupplyAmt
                                        , int T_pTaxAmt
                                        , int T_C_Cash_Send_TF
                                        , ref string T_rStatus
                                        , ref string T_rHCashTransactionNo
                                        , ref string T_rHTradeDate
                                        , ref string T_rHTradeTime
                                        , ref string T_rHMessage1
                                        )
        {


            //string pApprovalType,//	H000:일반발급, H200:계좌이체,	H600:가상계좌
            //string pTransactionNo,//	입금완료된 계좌이체, 가상계좌	거래번호
            //string pIssuSele,//	0:일반발급(PG원거래번호	중복체크), 1:단독발급(주문번호 중복체크	:	PG원거래 없음),	2:강제발급(중복체크	안함)
            //string pUserInfoSele,//	0:주민등록번호
            //string pUserInfo,//	주민등록번호
            //string pTranSele,//	0: 개인, 1:	사업자
            //string pCallCode,//	통화코드	(0:	원화, 1: 미화)
            //string pSupplyAmt,//	공급가액
            //string pTaxAmt,//	세금
            //string pSvcAmt,//	봉사료
            //string pTotAmt,//	현금영수증 발급금액
            //string pFiller)//	예비


            // Default(수정항목이 아님)-------------------------------------------------------
            string EncType = "2";// 0: 암화안함, 2: seed
            string Version = "0311";// 전문버전(가상계좌는 0603)
            string Type = "00";// 구분
            string Resend = "0";// 전송구분 : 0 : 처음,  1: 재전송
            string RequestDate = DateTime.Now.ToString("yyyyMMddhhmmss");// 요청일자 : yyyymmddhhmmss
            string KeyInType = "K";// KeyInType 여부 : S : Swap, K: KeyInType
            string LineType = "1";// lineType 0 : offline, 1:internet, 2:Mobile
            string ApprovalCount = "1";// 복합승인갯수
            string GoodType = "1";// 제품구분 1 : 실물, 2 : 디지털
            string HeadFiller = "";// 예비
            //-------------------------------------------------------------------------------

            // Data Default end -------------------------------------------------------------
            //승인타입	 : A-인증없는승인, N-인증승인, M-Visa3D인증승인, I-ISP인증승인 
            string certitype = App_certitype;

            //Header부 Data --------------------------------------------------
            string StoreId = Ksn_storeid;// *상점아이디
            string OrderNumber = OrderNo;// *주문번호
            string UserName = U_Name;// *주문자명
            string IdNum = Cpno;// 주민번호 or 사업자번호
            string Email = "";// *email
            string GoodName = OrderNo;// *제품명
            string PhoneNo = "";// *휴대폰번호
            //Header end -------------------------------------------------------------------

            //Data Default-------------------------------------------------
            string pApprovalType = "H000";//	H000:일반발급, H200:계좌이체,	H600:가상계좌
            string pTransactionNo = "";//	입금완료된 계좌이체, 가상계좌	거래번호
            string pIssuSele = "2";//	0:일반발급(PG원거래번호	중복체크), 1:단독발급(주문번호 중복체크	:	PG원거래 없음),	2:강제발급(중복체크	안함)

            string pUserInfoSele = "";
            string pTranSele = "";

            if (T_C_Cash_Send_TF == 1)
                pTranSele = "0";//	0: 개인, 1:	사업자
            else
                pTranSele = "1";//	0: 개인, 1:	사업자


            if (Cpno.Length == 13)
                pUserInfoSele = "0";//	0 : 주민번호 1 : 사업자번호 2 : 카드번호 3 : 휴대폰번호 4 : 기타

            if (Cpno.Substring(0, 3) == "010" || Cpno.Substring(0, 3) == "016" || Cpno.Substring(0, 3) == "019" || Cpno.Substring(0, 3) == "017" || Cpno.Substring(0, 3) == "011" || Cpno.Substring(0, 3) == "018")
                pUserInfoSele = "4";//	0 : 주민번호 1 : 사업자번호 2 : 카드번호 3 : 휴대폰번호 4 : 기타

            if (pUserInfoSele == "")
            {
                pUserInfoSele = "2"; // 이도 저도 아닌거는 사업자 번호로 변환해 버린다.
                pTranSele = "1";//	0: 개인, 1:	사업자 //사업자 번호로 신고할때는 사업자로 해서 보낸다.
            }
            string pUserInfo = Cpno;//	주민번호,사업자번호 




            string pCallCode = "0";//	통화코드	(0:	원화, 1: 미화)
            string pSupplyAmt = T_pSupplyAmt.ToString();//	공급가액
            string pTaxAmt = T_pTaxAmt.ToString();//	세금
            string pSvcAmt = "0";//	봉사료
            string pTotAmt = Send_Amount.ToString();//	현금영수증 발급금액
            string pFiller = "";//	예비
            //--------------------------------------------------------------------------------------

            string rHTransactionNo = "";
            string rHStatus = "";//	오류구분 O:정상	X:거절
            string rHCashTransactionNo = "";//	현금영수증 거래번호
            string rHIncomeType = "";//	0: 소득		 1:	비소득
            string rHTradeDate = "";//	거래 개시 일자
            string rHTradeTime = "";//	거래 개시 시간
            string rHMessage1 = "";//	응답 message1
            string rHMessage2 = "";//	응답 message2
            string rHCashMessage1 = "";//	국세청 메시지 1
            string rHCashMessage2 = "";//	국세청 메시지 2
            string rHFiller = ""; ;//	예비

            try
            {
                KSPayApprovalCancelBean ipg = new KSPayApprovalCancelBean("220.117.241.175", 21000);
                //ksnet.kspay.KSPayApprovalCancelBean ipg = new ksnet.kspay.KSPayApprovalCancelBean("localhost", 29991);

                //Header부 전문조립
                ipg.HeadMessage(EncType, Version, Type, Resend, RequestDate, StoreId, OrderNumber, UserName, IdNum, Email, GoodType, GoodName, KeyInType, LineType, PhoneNo, ApprovalCount, HeadFiller);



                //Data부 전문조립
                ipg.CashBillDataMessage(pApprovalType, pTransactionNo, pIssuSele, pUserInfoSele, pUserInfo, pTranSele, pCallCode, pSupplyAmt, pTaxAmt, pSvcAmt, pTotAmt, pFiller);

                //KSPAY로 요청전문송신후 수신데이터 파싱
                if (ipg.SendSocket("1"))
                {
                    rHTransactionNo = ipg.HTransactionNo;//	거래번호
                    rHStatus = ipg.HStatus;//	오류구분 O:정상	X:거절
                    rHCashTransactionNo = ipg.HCashTransactionNo;//	현금영수증 거래번호
                    rHIncomeType = ipg.HIncomeType;//	0: 소득		 1:	비소득
                    rHTradeDate = ipg.HTradeDate;//	거래 개시 일자
                    rHTradeTime = ipg.HTradeTime;//	거래 개시 시간
                    rHMessage1 = ipg.HMessage1;//	응답 message1
                    rHMessage2 = ipg.HMessage2;//	응답 message2
                    rHCashMessage1 = ipg.HCashMessage1;//	국세청 메시지 1
                    rHCashMessage2 = ipg.HCashMessage2;//	국세청 메시지 2
                    rHFiller = ipg.HFiller;//	예비

                    T_rStatus = rHStatus;
                    T_rHCashTransactionNo = rHCashTransactionNo;
                    T_rHTradeDate = rHTradeDate;
                    T_rHTradeTime = rHTradeTime;//	거래 개시 시간
                    T_rHMessage1 = rHMessage1;//	거래 개시 시간

                    return rHTransactionNo; // 거래번호      //저장해야함 승인취소시 사용됨.
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {

                KSPayApprovalCancelBean.write_log("ERROR : Exception [" + e.StackTrace + "]!!!");
                return "";
            }

        }


        public string KSPayCashPostMNI(string OrderNo, string U_Name, string Cpno
                                        , int Send_Amount
                                        , int T_pSupplyAmt
                                        , int T_pTaxAmt
                                        , ref string T_rStatus
                                        , ref string T_rHCashTransactionNo
                                        , ref string T_rHTradeDate
                                        , ref string T_rHTradeTime
                                        , ref string T_rHMessage1
                                        )
        {


            //string pApprovalType,//	H000:일반발급, H200:계좌이체,	H600:가상계좌
            //string pTransactionNo,//	입금완료된 계좌이체, 가상계좌	거래번호
            //string pIssuSele,//	0:일반발급(PG원거래번호	중복체크), 1:단독발급(주문번호 중복체크	:	PG원거래 없음),	2:강제발급(중복체크	안함)
            //string pUserInfoSele,//	0:주민등록번호
            //string pUserInfo,//	주민등록번호
            //string pTranSele,//	0: 개인, 1:	사업자
            //string pCallCode,//	통화코드	(0:	원화, 1: 미화)
            //string pSupplyAmt,//	공급가액
            //string pTaxAmt,//	세금
            //string pSvcAmt,//	봉사료
            //string pTotAmt,//	현금영수증 발급금액
            //string pFiller)//	예비


            // Default(수정항목이 아님)-------------------------------------------------------
            string EncType = "2";// 0: 암화안함, 2: seed
            string Version = "0311";// 전문버전(가상계좌는 0603)
            string Type = "00";// 구분
            string Resend = "0";// 전송구분 : 0 : 처음,  1: 재전송
            string RequestDate = DateTime.Now.ToString("yyyyMMddhhmmss");// 요청일자 : yyyymmddhhmmss
            string KeyInType = "K";// KeyInType 여부 : S : Swap, K: KeyInType
            string LineType = "1";// lineType 0 : offline, 1:internet, 2:Mobile
            string ApprovalCount = "1";// 복합승인갯수
            string GoodType = "1";// 제품구분 1 : 실물, 2 : 디지털
            string HeadFiller = "";// 예비
            //-------------------------------------------------------------------------------

            // Data Default end -------------------------------------------------------------
            //승인타입	 : A-인증없는승인, N-인증승인, M-Visa3D인증승인, I-ISP인증승인 
            string certitype = App_certitype;

            //Header부 Data --------------------------------------------------
            string StoreId = Ksn_storeid;// *상점아이디
            string OrderNumber = OrderNo;// *주문번호
            string UserName = U_Name;// *주문자명
            string IdNum = Cpno;// 주민번호 or 사업자번호
            string Email = "";// *email
            string GoodName = OrderNo;// *제품명
            string PhoneNo = "";// *휴대폰번호
            //Header end -------------------------------------------------------------------

            //Data Default-------------------------------------------------
            string pApprovalType = "H000";//	H000:일반발급, H200:계좌이체,	H600:가상계좌
            string pTransactionNo = "";//	입금완료된 계좌이체, 가상계좌	거래번호
            string pIssuSele = "2";//	0:일반발급(PG원거래번호	중복체크), 1:단독발급(주문번호 중복체크	:	PG원거래 없음),	2:강제발급(중복체크	안함)

            string pUserInfoSele = "1";//	0 : 주민번호 1 : 사업자번호 2 : 카드번호 3 : 휴대폰번호 4 : 기타
            string pUserInfo = Cpno;//	주민번호,사업자번호 
            string pTranSele = "1";//	0: 개인, 1:	사업자

            string pCallCode = "0";//	통화코드	(0:	원화, 1: 미화)
            string pSupplyAmt = T_pSupplyAmt.ToString();//	공급가액
            string pTaxAmt = T_pTaxAmt.ToString();//	세금
            string pSvcAmt = "0";//	봉사료
            string pTotAmt = Send_Amount.ToString();//	현금영수증 발급금액
            string pFiller = "";//	예비
            //--------------------------------------------------------------------------------------

            string rHTransactionNo = "";
            string rHStatus = "";//	오류구분 O:정상	X:거절
            string rHCashTransactionNo = "";//	현금영수증 거래번호
            string rHIncomeType = "";//	0: 소득		 1:	비소득
            string rHTradeDate = "";//	거래 개시 일자
            string rHTradeTime = "";//	거래 개시 시간
            string rHMessage1 = "";//	응답 message1
            string rHMessage2 = "";//	응답 message2
            string rHCashMessage1 = "";//	국세청 메시지 1
            string rHCashMessage2 = "";//	국세청 메시지 2
            string rHFiller = ""; ;//	예비

            try
            {
                KSPayApprovalCancelBean ipg = new KSPayApprovalCancelBean("220.117.241.175", 21000);
                //ksnet.kspay.KSPayApprovalCancelBean ipg = new ksnet.kspay.KSPayApprovalCancelBean("localhost", 29991);

                //Header부 전문조립
                ipg.HeadMessage(EncType, Version, Type, Resend, RequestDate, StoreId, OrderNumber, UserName, IdNum, Email, GoodType, GoodName, KeyInType, LineType, PhoneNo, ApprovalCount, HeadFiller);



                //Data부 전문조립
                ipg.CashBillDataMessage(pApprovalType, pTransactionNo, pIssuSele, pUserInfoSele, pUserInfo, pTranSele, pCallCode, pSupplyAmt, pTaxAmt, pSvcAmt, pTotAmt, pFiller);

                //KSPAY로 요청전문송신후 수신데이터 파싱
                if (ipg.SendSocket("1"))
                {
                    rHTransactionNo = ipg.HTransactionNo;//	거래번호
                    rHStatus = ipg.HStatus;//	오류구분 O:정상	X:거절
                    rHCashTransactionNo = ipg.HCashTransactionNo;//	현금영수증 거래번호
                    rHIncomeType = ipg.HIncomeType;//	0: 소득		 1:	비소득
                    rHTradeDate = ipg.HTradeDate;//	거래 개시 일자
                    rHTradeTime = ipg.HTradeTime;//	거래 개시 시간
                    rHMessage1 = ipg.HMessage1;//	응답 message1
                    rHMessage2 = ipg.HMessage2;//	응답 message2
                    rHCashMessage1 = ipg.HCashMessage1;//	국세청 메시지 1
                    rHCashMessage2 = ipg.HCashMessage2;//	국세청 메시지 2
                    rHFiller = ipg.HFiller;//	예비

                    T_rStatus = rHStatus;
                    T_rHCashTransactionNo = rHCashTransactionNo;
                    T_rHTradeDate = rHTradeDate;
                    T_rHTradeTime = rHTradeTime;//	거래 개시 시간
                    T_rHMessage1 = rHMessage1;//	거래 개시 시간

                    return rHTransactionNo; // 거래번호      //저장해야함 승인취소시 사용됨.
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {

                KSPayApprovalCancelBean.write_log("ERROR : Exception [" + e.StackTrace + "]!!!");
                return "";
            }

        }



        public string KSPayCancelPost_Cash(string Ksn_TrNo, ref string T_rStatus
                                            , ref string T_rHCashTransactionNo
                                        , ref string T_rHTradeDate
                                        , ref string T_rHTradeTime
                                        , ref string T_rHMessage1
                                            )
        {
            //'Header부 Data --------------------------------------------------
            string EncType = "2";// 0: 암화안함, 2: seed
            string Version = "0311";// 전문버전(가상계좌는 0603)
            string Type = "00";// 구분
            string Resend = "0";// 전송구분 : 0 : 처음,  1: 재전송
            string RequestDate = DateTime.Now.ToString("yyyyMMddhhmmss");// 요청일자 : yyyymmddhhmmss
            string KeyInType = "K";// KeyInType 여부 : S : Swap, K: KeyInType
            string LineType = "1";// lineType 0 : offline, 1:internet, 2:Mobile
            string ApprovalCount = "1";// 복합승인갯수
            string GoodType = "1";// 제품구분 1 : 실물, 2 : 디지털
            string HeadFiller = "";// 예비
            //' Header (입력값 (*) 필수항목)--------------------------------------------------
            string StoreId = Ksn_storeid;// *상점아이디
            string OrderNumber = "";// *주문번호
            string UserName = "";// *주문자명
            string IdNum = "";// 주민번호 or 사업자번호
            string Email = "";// *email
            string GoodName = "";// *제품명
            string PhoneNo = "";// *휴대폰번호
            //'Header end -------------------------------------------------------------------

            //'Data Default------------------------------------------------------------------
            String ApprovalType = "H010";//' 승인구분 코드    
            String TrNo = Ksn_TrNo;//' 거래번호                                          

            //' Server로 부터 응답이 없을시 자체응답
            string rApprovalType = "1001";
            string rTransactionNo = "";// 거래번호
            string rStatus = "X";// 상태 O : 승인, X : 거절
            string rTradeDate = "";// 거래일자
            string rTradeTime = "";// 거래시간
            string rIssCode = "00";// 발급사코드
            string rAquCode = "00";// 매입사코드
            string rAuthNo = "9999";// 승인번호 or 거절시 오류코드
            string rMessage1 = "승인거절";// 메시지1
            string rMessage2 = "C잠시후재시도";// 메시지2
            string rCardNo = "";// 카드번호
            string rExpDate = "";// 유효기간
            string rInstallment = "";// 할부
            string rAmount = "";// 금액
            string rMerchantNo = "";// 가맹점번호
            string rAuthSendType = "N";// 전송구분
            string rApprovalSendType = "N";// 전송구분(0 : 거절, 1 : 승인, 2: 원카드)
            string rPoint1 = "000000000000";// Point1
            string rPoint2 = "000000000000";// Point2
            string rPoint3 = "000000000000";// Point3
            string rPoint4 = "000000000000";// Point4
            string rVanTransactionNo = "";// 
            string rFiller = "";// 예비
            string rAuthType = "";// ISP : ISP거래, MP1, MP2 : MPI거래, SPACE : 일반거래
            string rMPIPositionType = "";// K : KSNET, R : Remote, C : 제3기관, SPACE : 일반거래
            string rMPIReUseType = "";// Y : 재사용, N : 재사용아님
            string rEncData = "";// MPI, ISP 데이터
            //' --------------------------------------------------------------------------------

            string rHTransactionNo = "";
            string rHStatus = "";//	오류구분 O:정상	X:거절
            string rHCashTransactionNo = "";//	현금영수증 거래번호
            string rHIncomeType = "";//	0: 소득		 1:	비소득
            string rHTradeDate = "";//	거래 개시 일자
            string rHTradeTime = "";//	거래 개시 시간
            string rHMessage1 = "";//	응답 message1
            string rHMessage2 = "";//	응답 message2
            string rHCashMessage1 = "";//	국세청 메시지 1
            string rHCashMessage2 = "";//	국세청 메시지 2

            string rHFiller = ""; ;//	예비
            KSPayApprovalCancelBean ipg = new KSPayApprovalCancelBean("220.117.241.175", 21000);


            //Header부 전문조립
            ipg.HeadMessage(EncType, Version, Type, Resend, RequestDate, StoreId, OrderNumber, UserName, IdNum, Email, GoodType, GoodName, KeyInType, LineType, PhoneNo, ApprovalCount, HeadFiller);

            ipg.CancelDataMessage(ApprovalType, "0", TrNo, "", "", "", "", "");

            if (ipg.SendSocket("1"))
            {
                rHTransactionNo = ipg.HTransactionNo;//	거래번호
                rHStatus = ipg.HStatus;//	오류구분 O:정상	X:거절
                rHCashTransactionNo = ipg.HCashTransactionNo;//	현금영수증 거래번호
                rHIncomeType = ipg.HIncomeType;//	0: 소득		 1:	비소득
                rHTradeDate = ipg.HTradeDate;//	거래 개시 일자
                rHTradeTime = ipg.HTradeTime;//	거래 개시 시간
                rHMessage1 = ipg.HMessage1;//	응답 message1
                rHMessage2 = ipg.HMessage2;//	응답 message2
                rHCashMessage1 = ipg.HCashMessage1;//	국세청 메시지 1
                rHCashMessage2 = ipg.HCashMessage2;//	국세청 메시지 2
                rHFiller = ipg.HFiller;//	예비

                T_rStatus = rHStatus;
                T_rHCashTransactionNo = rHCashTransactionNo;
                T_rHTradeDate = rHTradeDate;
                T_rHTradeTime = rHTradeTime;//	거래 개시 시간
                T_rHMessage1 = rHMessage1;//	거래 개시 시간

                return rHTransactionNo; // 거래번호      //저장해야함 승인취소시 사용됨.
            }
            else
            {
                return "";
            }
        }






        //주문번호, 사용자 성명, 주민번호(안보내도됨),  결제금액 
        public string KSPay_VirtualAccount(string OrderNo, string U_Name, string Cpno
                                        , int Send_Amount
                                        , string TBankCode 
                                        , ref string T_rStatus
                                        , ref string T_rAuthNo
                                        , ref string T_Er_Msg
                                        )
        {

            string PayDate = "";

            PayDate = cls_User.gid_date_time.Substring(0, 4) + '-' + cls_User.gid_date_time.Substring(4, 2) + '-' + cls_User.gid_date_time.Substring(6, 2);
            DateTime TodayDate = new DateTime();
            TodayDate = DateTime.Parse(PayDate);
            PayDate = TodayDate.AddDays(5).ToString("yyyy-MM-dd").Replace("-", "");

            if (TBankCode.Length == 3)
                TBankCode = TBankCode.Substring(1, 2); 


            // Default(수정항목이 아님)-------------------------------------------------------
            string EncType = "0";// 0: 암화안함, 2: seed
            string Version = "0603";// 전문버전(가상계좌는 0603)
            string Type = "00";// 구분
            string Resend = "0";// 전송구분 : 0 : 처음,  1: 재전송
            string RequestDate = DateTime.Now.ToString("yyyyMMddhhmmss");// 요청일자 : yyyymmddhhmmss
            string KeyInType = "K";// KeyInType 여부 : S : Swap, K: KeyInType
            string LineType = "1";// lineType 0 : offline, 1:internet, 2:Mobile
            string ApprovalCount = "1";// 복합승인갯수
            string GoodType = "0";// 제품구분 1 : 실물, 2 : 디지털
            string HeadFiller = "";// 예비
            //-------------------------------------------------------------------------------


            //Header부 Data --------------------------------------------------
            string StoreId = Ksn_storeid;// *상점아이디
            string OrderNumber = OrderNo;// *주문번호
            string UserName = U_Name;// *주문자명
            string IdNum = Cpno;// 주민번호 or 사업자번호
            string Email = "";// *email
            string GoodName = OrderNo;// *제품명
            string PhoneNo = "";// *휴대폰번호
            //Header end -------------------------------------------------------------------

            //Data Default-------------------------------------------------
            string ApprovalType = Bank_authty;// 승인구분
            string Amount = Send_Amount.ToString();// 금액
            string Filler = "";// 예비

            string BankCode = TBankCode ;// 은행코드
            string CloseDate = PayDate;// 마감일자
            string CloseTime = "235959";// 마감시간
            string EscrowSele = "0";// 에스크로적용구분: 0:적용안함, 1:적용, 2:강제적용
            string VirFixSele = "";// 가상계좌번호지정구분
            string VirAcctNo = "";// 가상계좌번호
            string OrgTransactionNo = "";// 원거래거래번호


            //Data Default end -------------------------------------------------------------

            //Server로 부터 응답이 없을시 자체응답
            string rVATransactionNo = "";
            string rVAStatus = "X";
            string rVATradeDate = "";
            string rVATradeTime = "";
            string rVABankCode = "";
            string rVAVirAcctNo = "";
            string rVAName = "";
            string rVACloseDate = "";
            string rVACloseTime = "";
            string rVARespCode = "9999";
            string rVAMessage1 = "";
            string rVAMessage2 = "";
            string rVAAmount = "";
            string rVAFiller = "";
            //--------------------------------------------------------------------------------------

            try
            {
                KSPayApprovalCancelBean ipg = new KSPayApprovalCancelBean("220.117.241.175", 21000);
                //ksnet.kspay.KSPayApprovalCancelBean ipg = new ksnet.kspay.KSPayApprovalCancelBean("localhost", 29991);

                //Header부 전문조립
                ipg.HeadMessage(EncType, Version, Type, Resend, RequestDate, StoreId, OrderNumber, UserName, IdNum, Email, GoodType, GoodName, KeyInType, LineType, PhoneNo, ApprovalCount, HeadFiller);



                //Data부 전문조립
                ipg.VirtualAccountDataMessage(ApprovalType, BankCode, Amount, CloseDate, CloseTime, EscrowSele, VirFixSele, VirAcctNo, OrgTransactionNo, Filler);

                //KSPAY로 요청전문송신후 수신데이터 파싱
                if (ipg.SendSocket("1"))
                {
                    rVATransactionNo = ipg.VATransactionNo;//거래번호
                    rVAStatus = ipg.VAStatus;//O , X
                    rVATradeDate = ipg.VATradeDate;//발급일자
                    rVATradeTime = ipg.VATradeTime;//발급시간
                    rVABankCode = ipg.VABankCode;//BANKCODE
                    rVAVirAcctNo = ipg.VAVirAcctNo;//가상계좌번호
                    rVAName = ipg.VAName;//업체명
                    rVACloseDate = ipg.VACloseDate;//종료일자
                    rVACloseTime = ipg.VACloseTime;//종료시간
                    rVARespCode = ipg.VARespCode;
                    rVAMessage1 = ipg.VAMessage1;
                    rVAMessage2 = ipg.VAMessage2;
                    rVAAmount = ipg.Amount;
                    rVAFiller = ipg.VAFiller;//금액


                    T_rStatus = rVAStatus;
                    T_rAuthNo = rVAVirAcctNo;
                    T_Er_Msg = rVAMessage1 + " " + rVAMessage2 + " " + rVAVirAcctNo;
                    return rVATransactionNo; // 거래번호      //저장해야함 승인취소시 사용됨.
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {

                KSPayApprovalCancelBean.write_log("ERROR : Exception [" + e.StackTrace + "]!!!");
                return "";
            }

        }







        public string Check_Nice_Same_BankAccount_Web(string OrderNumber, int C_index, int Mbid2, string U_Name
                        , string BankCode, int Send_Amount, string C_Cash_Send_Nu, int C_Cash_Send_TF
                        , ref string T_rAuthNo, ref string T_Er_Msg
                        , string NNDttt
                        , string cashReceiptType 
                         , string Bank_Mid
                        , string Bank_Key
                        )
        {


            string str_sendvalue = "";
            str_sendvalue = "mid=" + Bank_Mid;
            str_sendvalue = str_sendvalue + "&encodeKey=" + Bank_Key;
            str_sendvalue = str_sendvalue + "&moid=" + OrderNumber;
            str_sendvalue = str_sendvalue + "&goodsName=solrxkorea";
            str_sendvalue = str_sendvalue + "&amt=" + Send_Amount;
            str_sendvalue = str_sendvalue + "&buyerName=" + U_Name;
            str_sendvalue = str_sendvalue + "&buyerEmail=" + "webmaster@solrxkorea.com";
            str_sendvalue = str_sendvalue + "&buyerTel=" + "1833-5477";
            str_sendvalue = str_sendvalue + "&bankCode=" + BankCode;
            str_sendvalue = str_sendvalue + "&vbankExpDate=" + NNDttt;
            

            str_sendvalue = str_sendvalue + "&cashReceiptType=" + cashReceiptType ; //0 : 미발행, 1 : 소득공제, 2 : 지출증빙


            string URL = "https://myoffice.solrx.co.kr:487/common/cs/vbank/approval.do";         //운영
            //string URL = "https://solrxkorea.ilsonginfo.co.kr/common/cs/vbank/approval.do";  //개발


            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "solrxkorea";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.



            HttpWebResponse wRes;
            wRes = (HttpWebResponse)hwr.GetResponse();
            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);


            string PG_Number = "";
            string s_Line1 = "";
            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            JObject ReturnData = new JObject();
            string SuccessYN = "";

            try
            {
                ReturnData = JObject.Parse(getstring);
                SuccessYN = ReturnData["successYN"].ToString();

                if (SuccessYN == "Y")
                {
                    //T_rStatus = ReturnData["resultCode"].ToString();
                    T_rAuthNo = ReturnData["vbankNum"].ToString();
                    s_Line1 = ReturnData["tid"].ToString();                    

                    T_Er_Msg = "Y";
                }
                else
                {
                    T_Er_Msg = ReturnData["errMessage"].ToString();
                    s_Line1 = ReturnData["tid"].ToString(); 
                }

            }
            catch
            {
                s_Line1 = "N";
            }

            PG_Number = s_Line1;
            //string s_Line1 = readerPost.ReadLine().ToString();
            //string s_Line2 = readerPost.ReadLine().ToString();
            //string s_Line3 = readerPost.ReadLine().ToString();
            //string s_Line4 = readerPost.ReadLine().ToString();

            //res_cd = s_Line3;
            //T_Er_Msg = s_Line4;



            //string PG_Number = "";
            ///* -------------------------------------------------------------------------- */
            ///* ::: 가맹점 DB 처리                                                         */
            ///* -------------------------------------------------------------------------- */
            ///* 응답코드(res_cd)가 "0000" 이면 정상승인 입니다.                            */
            ///* r_amount가 주문DB의 금액과 다를 시 반드시 취소 요청을 하시기 바랍니다.     */
            ///* DB 처리 실패 시 취소 처리를 해주시기 바랍니다.                             */
            ///* -------------------------------------------------------------------------- */
            //if (res_cd == "0000")
            //{
            //    T_Er_Msg = "";
            //    PG_Number = s_Line1;
            //    T_rAuthNo = s_Line2;
            //}
            //else
            //{
            //    PG_Number = "";
            //    T_rAuthNo = "";
            //    T_Er_Msg = res_cd + " " + res_msg;
            //}

            return PG_Number;

        }






        public string Check_Nice_Same_BankAccount_Cancel_Web(string OrderNumber, int C_index, string C_Number1, string C_Number3
                        , ref string T_rAuthNo, ref string T_Er_Msg
                        , string Nice_Mid, string Nice_cancelPwd, int Send_Amount)
        {

            string SERVICE_ID = Nice_Mid;

            string ORDER_ID = OrderNumber;				//주문번호

            try
            {
                string str_sendvalue = "";
                str_sendvalue = "mid=" + SERVICE_ID;
                str_sendvalue = str_sendvalue + "&cancelPwd=" + Nice_cancelPwd;
                str_sendvalue = str_sendvalue + "&payMethod=5";
                str_sendvalue = str_sendvalue + "&tid=" + C_Number3;

                str_sendvalue = str_sendvalue + "&cancelAmt=" + Send_Amount;
                str_sendvalue = str_sendvalue + "&cancelMsg=취소";
                str_sendvalue = str_sendvalue + "&partialCancelCode=0"; //0 전체취소   1 부분 취소

                string URL = "https://myoffice.solrx.co.kr:487//common/cs/united/cancel.do";         //운영
                //string URL = "https://solrxkorea.ilsonginfo.co.kr//common/cs/united/cancel.do";  //개발


                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
                hwr.Method = "POST"; // 포스트 방식으로 전달                
                hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
                hwr.UserAgent = "solrxkorea";
                Encoding encoding = Encoding.UTF8;
                byte[] buffer = encoding.GetBytes(str_sendvalue);
                hwr.ContentLength = buffer.Length;

                Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
                sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
                sendStream.Close(); // sendStream 을 종료한다.



                HttpWebResponse wRes;
                wRes = (HttpWebResponse)hwr.GetResponse();
                Stream respPostStream = wRes.GetResponseStream();
                StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

                string s_Line1 = "";
                string getstring = null;
                getstring = readerPost.ReadToEnd().ToString();

                JObject ReturnData = new JObject();
                string SuccessYN = "";
                string PG_Number = "";

                try
                {
                    ReturnData = JObject.Parse(getstring);
                    SuccessYN = ReturnData["successYN"].ToString();

                    if (SuccessYN == "Y")
                    {
                        s_Line1 = ReturnData["resultCode"].ToString();
                        T_rAuthNo = s_Line1; // ReturnData["authCode"].ToString();
                        PG_Number = T_rAuthNo;
                        //s_Line1 = T_rStatus;  // ReturnData["tid"].ToString();

                        //T_Er_Msg = "Y";
                    }
                    else
                    {
                        //T_Er_Msg = ReturnData["errMessage"].ToString();
                        s_Line1 = "N";
                    }

                }
                catch
                {

                    s_Line1 = "N";

                }



                //string s_Line1 = readerPost.ReadLine().ToString();
                //string s_Line2 = readerPost.ReadLine().ToString();
                //string s_Line3 = readerPost.ReadLine().ToString();
                //string s_Line4 = readerPost.ReadLine().ToString();

                //res_cd = s_Line3;
                //res_msg = s_Line4;

                //if (res_cd == "0000")
                //{
                //    T_Er_Msg = "";
                //    PG_Number = s_Line1;
                //    T_rAuthNo = s_Line2;
                //}
                //else
                //{
                //    PG_Number = "";
                //    T_rAuthNo = "";
                //    T_Er_Msg = res_cd + " " + res_msg;
                //}

                return PG_Number;



            }
            catch (Exception e)
            {
                return "";
            }

        }


    }




        
            

    class GalaxiaApi_Pay_cs
    {
            
        private string Ksn_storeid = "glx-api"; // 에이필드        

        private string Ksn_storeid_Card = "M1612038"; // 에이필드 관련 아이디임/
        private string Ksn_storeid_Card_M = "M1612037"; // 에이필드 관련 아이디임 모바일/
        private string Ksn_storeid_Card_W = "M1612035"; // 에이필드 관련 아이디임 웹/
        private string Ksn_storeid_Card_Danmal = "M1612039"; // 에이필드 관련 아이디임 웹/
        private string Ksn_storeid_Cash = "M1612076"; // 에이필드 관련 아이디임/
        private string RUN_MODE = "1"; // //'0:테스트모드, 1:상용모드

        

        //주문번호, 사용자 성명, 주민번호(사용안함), 일반/무이자구분 1:일반 2:무이자 , 카드번호, 유효기간(년월), 할부(00일시불), 결제금액, 비밀번호 2자리, 생년월일 앞 6자리, 매출일자, 회원번호, 상품코드, 카드회사코드, 주문일자
        public string KSPayCreditPostMNI(string OrderNo, string U_Name, string Cpno, string T_interest, string CardNo
                                        , string Card_Per
                                        , string HalBu
                                        , int Send_Amount
                                        , string T_Passwd
                                        , string T_Birth
                                        , string Selldate
                                        , string Mbid2
                                        , string ItemCode
                                        , string Card_Com_Code
                                        , string ORDER_DATE         
                                        , string Etc_Card_Sugi_Mid 
                                        , string Etc_Card_Sugi_Key     
                                        , ref string T_rStatus
                                        , ref string T_rAuthNo
                                        , ref string T_Er_Msg
                                        , string Je_Card_FLAG 
                                        )
        {
            string SERVICE_ID = Ksn_storeid_Card; 			//가맹점 ID            
            string ORDER_ID = OrderNo;				//주문번호
            string USER_ID = Mbid2;				//고객 아이디
            string USER_NAME = U_Name;			//고객명
            string ITEM_CODE = ItemCode;				//품코드
            string ITEM_NAME = "";				//상품명
            string USER_EMAIL = "";			//고객 이메일
            string PIN_NUMBER = CardNo;			//결제카드 번호
            string EXPIRE_DATE = Card_Per; 			//결제카드 유효년일 (YYMM, ex>2016년 04월 -> 1604)
            string PASSWORD = T_Passwd;				//결제카드 비밀번호
            string SOCIAL_NUMBER = T_Birth; 		//주민번호 앞6자리, 법인번호 10자리
            string CVC2 = "";					//CVC2
            string QUOTA = HalBu;				//부개월수(무인증)
            string DEAL_AMOUNT = Send_Amount.ToString();		//결제 금액
            string VAT = "0";					//부가세
            string SERVICE_CHARGE = "0";	//봉사료

            string DEAL_TYPE =  "" ; //수기특약 상세 타입
            if (Card_Com_Code == "0054") //삼성카드인 경우에는
                DEAL_TYPE = "0014";				
            else
                DEAL_TYPE = "0011";				

            string CERT_TYPE = "0002";				//수기특약
            string USING_TYPE = "0000";			//국내카드
            string CURRENCY_TYPE = "0000";			//인통화(원화)
            string OPCODE = "0000"; 			//언어구분(한글)
            string ISSUE_COMPANY_CODE = Card_Com_Code; //발급사 코드

            string USER_IP = "";

           
                               

            try

            {
                
                string str_sendvalue = "";
                str_sendvalue = "mid=" + Etc_Card_Sugi_Mid;
                str_sendvalue = str_sendvalue + "&encodeKey=" + Etc_Card_Sugi_Key;
                str_sendvalue = str_sendvalue + "&moid=" + OrderNo;
                str_sendvalue = str_sendvalue + "&goodsName=" + ItemCode;
                str_sendvalue = str_sendvalue + "&amt=" + Send_Amount;
                str_sendvalue = str_sendvalue + "&buyerName=" + U_Name;
                str_sendvalue = str_sendvalue + "&buyerEmail=" + "webmaster@solrxkorea.com";
                str_sendvalue = str_sendvalue + "&buyerTel=" + "1833-5477";
                str_sendvalue = str_sendvalue + "&cardNo=" + CardNo;
                str_sendvalue = str_sendvalue + "&cardExpire=" + Card_Per;
                str_sendvalue = str_sendvalue + "&buyerAuthNum=" + T_Birth;
                str_sendvalue = str_sendvalue + "&cardPwd=" + T_Passwd;
                str_sendvalue = str_sendvalue + "&cardQuota=" + HalBu;
                str_sendvalue = str_sendvalue + "&mallUserId=solrx5400g";
                if (Je_Card_FLAG == "")
                    str_sendvalue = str_sendvalue + "&cardInterest=0"; 
                else
                    str_sendvalue = str_sendvalue + "&cardInterest=1";   //무이자 처리르 한다. 제휴 카드는

              
                //str_sendvalue = str_sendvalue + "&Mbid2=" + Mbid2;
                //str_sendvalue = str_sendvalue + "&ItemCode=" + ItemCode;
                //str_sendvalue = str_sendvalue + "&Card_Com_Code=" + Card_Com_Code;
                //str_sendvalue = str_sendvalue + "&ORDER_DATE=" + ORDER_DATE.Substring(0, 14);
                //str_sendvalue = str_sendvalue + "&SERVICE_ID=" + SERVICE_ID;


                string URL = "https://myoffice.solrx.co.kr:487/common/cs/card/approval.do";         //운영
                //string URL = "https://solrxkorea.ilsonginfo.co.kr/common/cs/card/approval.do";  //개발


                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
                hwr.Method = "POST"; // 포스트 방식으로 전달                
                hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
                hwr.UserAgent = "solrxkorea";
                Encoding encoding = Encoding.UTF8;
                byte[] buffer = encoding.GetBytes(str_sendvalue);
                hwr.ContentLength = buffer.Length;

                Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
                sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
                sendStream.Close(); // sendStream 을 종료한다.



                HttpWebResponse wRes;
                wRes = (HttpWebResponse)hwr.GetResponse();
                Stream respPostStream = wRes.GetResponseStream();
                StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

                string s_Line1 = ""; 
                string getstring = null;
                getstring = readerPost.ReadToEnd().ToString();

                JObject ReturnData = new JObject();
                string SuccessYN = "";

                try
                {
                    ReturnData = JObject.Parse(getstring);
                    SuccessYN = ReturnData["successYN"].ToString();

                    if (SuccessYN == "Y")
                    {
                        T_rStatus = ReturnData["resultCode"].ToString();
                        T_rAuthNo = ReturnData["authCode"].ToString();
                        s_Line1 = ReturnData["tid"].ToString();

                        T_Er_Msg = "Y";
                    }
                    else
                    {
                        T_Er_Msg = ReturnData["errMessage"].ToString();
                        s_Line1 = ReturnData["tid"].ToString();
                    }

                }
                catch
                {
                    s_Line1 = "N";
                }
                

                //s_Line1 = SuccessYN;


                //string s_Line1 = readerPost.ReadLine().ToString();
                //string s_Line2 = readerPost.ReadLine().ToString();
                //string s_Line3 = readerPost.ReadLine().ToString();
                //string s_Line4 = readerPost.ReadLine().ToString();

                //T_rStatus = s_Line3.ToString(); ;
                //T_rAuthNo = s_Line2.ToString(); ;
                //T_Er_Msg = s_Line3 + " " + s_Line4;
                return s_Line1.ToString(); // 거래번호      //저장해야함 승인취소시 사용됨.

             
            }
            catch (Exception e)
            {
                ////MessageBox(e.ToString()); 
                return "";
            }


        }







        public string KSPayCancelPost(string T_ID, string Ksn_TrNo, ref string T_rStatus, ref string T_rAuthNo, int C_C_Price1, string seq_1
                                    , string OrderNo, string ORDER_DATE, string authAmount , string Cancel_ID
                                    , string Nice_Mid, string Nice_cancelPwd
                                    )
        {


            string SERVICE_ID = Nice_Mid ;

            string ORDER_ID = OrderNo;				//주문번호

             try

            {

                string str_sendvalue = "";
                str_sendvalue = "mid=" + SERVICE_ID;
                str_sendvalue = str_sendvalue + "&cancelPwd=" + Nice_cancelPwd;
                str_sendvalue = str_sendvalue + "&payMethod=3";
                str_sendvalue = str_sendvalue + "&tid=" + Ksn_TrNo;

                str_sendvalue = str_sendvalue + "&cancelAmt=" + C_C_Price1;
                str_sendvalue = str_sendvalue + "&cancelMsg=취소";
                str_sendvalue = str_sendvalue + "&partialCancelCode=0"; //0 전체취소   1 부분 취소

                string URL = "https://myoffice.solrx.co.kr:487//common/cs/united/cancel.do";         //운영
                //string URL = "https://solrxkorea.ilsonginfo.co.kr//common/cs/united/cancel.do";  //개발


                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
                hwr.Method = "POST"; // 포스트 방식으로 전달                
                hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
                hwr.UserAgent = "solrxkorea";
                Encoding encoding = Encoding.UTF8;
                byte[] buffer = encoding.GetBytes(str_sendvalue);
                hwr.ContentLength = buffer.Length;

                Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
                sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
                sendStream.Close(); // sendStream 을 종료한다.



                HttpWebResponse wRes;
                wRes = (HttpWebResponse)hwr.GetResponse();
                Stream respPostStream = wRes.GetResponseStream();
                StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

                string s_Line1 = "";
                string getstring = null;
                getstring = readerPost.ReadToEnd().ToString();

                JObject ReturnData = new JObject();
                string SuccessYN = "";

                try
                {
                    ReturnData = JObject.Parse(getstring);
                    SuccessYN = ReturnData["successYN"].ToString();

                    if (SuccessYN == "Y")
                    {
                        T_rStatus = ReturnData["resultCode"].ToString();
                        T_rAuthNo = T_rStatus; // ReturnData["authCode"].ToString();
                        s_Line1 = T_rStatus;  // ReturnData["tid"].ToString();

                        //T_Er_Msg = "Y";
                    }
                    else
                    {
                        //T_Er_Msg = ReturnData["errMessage"].ToString();
                        s_Line1 = "N";
                    }

                }
                catch
                {

                    s_Line1 = "N";
                    
                }


                //string s_Line1 = readerPost.ReadLine().ToString();
                //string s_Line2 = readerPost.ReadLine().ToString();
                //string s_Line3 = readerPost.ReadLine().ToString();


                //T_rStatus = s_Line3.ToString();
                //T_rAuthNo = s_Line1.ToString();
                return s_Line1.ToString();  // 거래번호      //저장해야함 승인취소시 사용됨.
                

                //string s_Line1 = readerPost.ReadLine().ToString();
                //string s_Line2 = readerPost.ReadLine().ToString();
                //string s_Line3 = readerPost.ReadLine().ToString();


                //T_rStatus = s_Line2.ToString();
                //T_rAuthNo = s_Line1.ToString();
                //return s_Line1.ToString();  // 거래번호      //저장해야함 승인취소시 사용됨.

            
               
            }
            catch (Exception e)
            {                
                return "";
            }
        }



        public string KSPayCancelPost(string T_ID, string Ksn_TrNo,string OrderNo,string ORDER_DATE,string requireType
                                    ,  ref string T_rStatus, ref string T_rAuthNo, int C_C_Price1
                                        , string seq_1, string Cancel_ID, string Nice_Mid, string Nice_cancelPwd)
        {
            string SERVICE_ID = Nice_Mid;

            
            string ORDER_ID = OrderNo;				//주문번호

            try
            {
        
                string str_sendvalue = "";
                str_sendvalue = "mid=" + SERVICE_ID;
                str_sendvalue = str_sendvalue + "&cancelPwd=" + Nice_cancelPwd;
                str_sendvalue = str_sendvalue + "&payMethod=3";
                str_sendvalue = str_sendvalue + "&tid=" + Ksn_TrNo;

                str_sendvalue = str_sendvalue + "&cancelAmt=" +  C_C_Price1 ;
                str_sendvalue = str_sendvalue + "&cancelMsg=취소";
                str_sendvalue = str_sendvalue + "&partialCancelCode=1"; //0 전체취소   1 부분 취소


                string URL = "https://myoffice.solrx.co.kr:487//common/cs/united/cancel.do";         //운영
                //string URL = "https://solrxkorea.ilsonginfo.co.kr//common/cs/united/cancel.do";  //개발


                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
                hwr.Method = "POST"; // 포스트 방식으로 전달                
                hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
                hwr.UserAgent = "solrxkorea";
                Encoding encoding = Encoding.UTF8;
                byte[] buffer = encoding.GetBytes(str_sendvalue);
                hwr.ContentLength = buffer.Length;

                Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
                sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
                sendStream.Close(); // sendStream 을 종료한다.



                HttpWebResponse wRes;
                wRes = (HttpWebResponse)hwr.GetResponse();
                Stream respPostStream = wRes.GetResponseStream();
                StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

                string s_Line1 = "";
                string getstring = null;
                getstring = readerPost.ReadToEnd().ToString();

                JObject ReturnData = new JObject();
                string SuccessYN = "";

                try
                {
                    ReturnData = JObject.Parse(getstring);
                    SuccessYN = ReturnData["successYN"].ToString();

                    if (SuccessYN == "Y")
                    {
                        T_rStatus = ReturnData["resultCode"].ToString();
                        T_rAuthNo = T_rStatus; // ReturnData["authCode"].ToString();
                        s_Line1 = T_rStatus;  // ReturnData["tid"].ToString();

                        //T_Er_Msg = "Y";
                    }
                    else
                    {
                       // T_Er_Msg = ReturnData["errMessage"].ToString();
                        s_Line1 = "N";
                    }

                }
                catch
                {
                    s_Line1 = "N";
                }



                //string s_Line1 = readerPost.ReadLine().ToString();
                //string s_Line2 = readerPost.ReadLine().ToString();
                //string s_Line3 = readerPost.ReadLine().ToString();


                //T_rStatus = s_Line3.ToString();
                //T_rAuthNo = s_Line1.ToString();
                return s_Line1.ToString();  // 거래번호      //저장해야함 승인취소시 사용됨.

              
            }
            catch (Exception e)
            {
                return "";
            }


            //T_rStatus = rStatus;
            //T_rAuthNo = rAuthNo;
            //return rTransactionNo; // 거래번호      //저장해야함 승인취소시 사용됨.
        }




        public string KSPayCashPostMNI(string OrderNo, string U_Name, string Cpno
                                        , int Send_Amount
                                        , int T_pSupplyAmt
                                        , int T_pTaxAmt
                                        , int T_C_Cash_Send_TF
                                        , string ORDER_DATE
                                        , ref string T_rStatus
                                        , ref string T_rHCashTransactionNo
                                        , ref string T_rHTradeDate
                                        , ref string T_rHTradeTime
                                        , ref string T_rHMessage1
                                        , string Cash_Mid
                                        , string Cash_Key
                                        )
        {



            string SERVICE_ID = Cash_Mid; 			//가맹점 ID            
            string ORDER_ID = OrderNo;				//주문번호
            

            try
            {
                string str_sendvalue = "";
                str_sendvalue = "mid=" + Cash_Mid;
                str_sendvalue = str_sendvalue + "&encodeKey=" + Cash_Key;
                str_sendvalue = str_sendvalue + "&moid=" + OrderNo;
                str_sendvalue = str_sendvalue + "&goodsName=solrxkorea";
                
                str_sendvalue = str_sendvalue + "&buyerName=" + U_Name;
                str_sendvalue = str_sendvalue + "&buyerEmail=" + "webmaster@solrxkorea.com";
                str_sendvalue = str_sendvalue + "&receiptAmt=" + Send_Amount;
                //str_sendvalue = str_sendvalue + "&buyerTel=" + "1833-5477";

                str_sendvalue = str_sendvalue + "&receiptType=" + T_C_Cash_Send_TF;   //0 : 미발행, 1 : 소득공제, 2 : 지출증빙
                str_sendvalue = str_sendvalue + "&receiptTypeNo=" + Cpno;


                //str_sendvalue = str_sendvalue + "&cashReceiptType=" + cashReceiptType; 


                string URL = "https://myoffice.solrx.co.kr:487/common/cs/cashReceipt/approval.do";         //운영
                //string URL = "https://solrxkorea.ilsonginfo.co.kr/common/cs/cashReceipt/approval.do";  //개발


                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
                hwr.Method = "POST"; // 포스트 방식으로 전달                
                hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
                hwr.UserAgent = "solrxkorea";
                Encoding encoding = Encoding.UTF8;
                byte[] buffer = encoding.GetBytes(str_sendvalue);
                hwr.ContentLength = buffer.Length;

                Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
                sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
                sendStream.Close(); // sendStream 을 종료한다.



                HttpWebResponse wRes;
                wRes = (HttpWebResponse)hwr.GetResponse();
                Stream respPostStream = wRes.GetResponseStream();
                StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);


                string PG_Number = "";
                string s_Line1 = "";
                string getstring = null;
                getstring = readerPost.ReadToEnd().ToString();

                JObject ReturnData = new JObject();
                string SuccessYN = "";

                try
                {
                    ReturnData = JObject.Parse(getstring);
                    SuccessYN = ReturnData["successYN"].ToString();

                    if (SuccessYN == "Y")
                    {
                        T_rHCashTransactionNo = ReturnData["authCode"].ToString();
                        T_rStatus = ReturnData["resultCode"].ToString();
                        s_Line1 = ReturnData["tid"].ToString();

                        //T_Er_Msg = "Y";
                    }
                    else
                    {
                        //T_Er_Msg = ReturnData["errMessage"].ToString();
                        s_Line1 = ReturnData["tid"].ToString();
                    }

                }
                catch
                {
                    s_Line1 = "";
                }

                PG_Number = s_Line1;

                return PG_Number; 
            }
            catch (Exception e)
            {
                //MessageBox(e.ToString()); 
                return "";
            }            
            
        }



        public string KSPayCancelPost_Cash(string Ksn_TrNo,string OrderNo,  string ORDER_DATE,  ref string T_rStatus
                                            , ref string T_rHCashTransactionNo
                                        , ref string T_rHTradeDate
                                        , ref string T_rHTradeTime
                                        , ref string T_rHMessage1
                                        , int Send_Amount 
                                        , string Nice_cancelPwd
                                        , string Nice_Mid
                                            )
        {


            string SERVICE_ID = Nice_Mid;

            string ORDER_ID = OrderNo;				//주문번호

            try
            {
                string str_sendvalue = "";
                str_sendvalue = "mid=" + SERVICE_ID;
                str_sendvalue = str_sendvalue + "&cancelPwd=" + Nice_cancelPwd;
                str_sendvalue = str_sendvalue + "&payMethod=1";
                str_sendvalue = str_sendvalue + "&tid=" + Ksn_TrNo;

                str_sendvalue = str_sendvalue + "&cancelAmt=" + Send_Amount;
                str_sendvalue = str_sendvalue + "&cancelMsg=취소";
                str_sendvalue = str_sendvalue + "&partialCancelCode=0"; //0 전체취소   1 부분 취소

                string URL = "https://myoffice.solrx.co.kr:487//common/cs/united/cancel.do";         //운영
                //string URL = "https://solrxkorea.ilsonginfo.co.kr//common/cs/united/cancel.do";  //개발


                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
                hwr.Method = "POST"; // 포스트 방식으로 전달                
                hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
                hwr.UserAgent = "solrxkorea";
                Encoding encoding = Encoding.UTF8;
                byte[] buffer = encoding.GetBytes(str_sendvalue);
                hwr.ContentLength = buffer.Length;

                Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
                sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
                sendStream.Close(); // sendStream 을 종료한다.



                HttpWebResponse wRes;
                wRes = (HttpWebResponse)hwr.GetResponse();
                Stream respPostStream = wRes.GetResponseStream();
                StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

                string s_Line1 = "";
                string getstring = null;
                getstring = readerPost.ReadToEnd().ToString();

                JObject ReturnData = new JObject();
                string SuccessYN = "";
                string PG_Number = "";

                try
                {
                    ReturnData = JObject.Parse(getstring);
                    SuccessYN = ReturnData["successYN"].ToString();

                    if (SuccessYN == "Y")
                    {
                        s_Line1 = ReturnData["resultCode"].ToString();
                        s_Line1 = s_Line1; // ReturnData["authCode"].ToString();
                        PG_Number = s_Line1;
                        //s_Line1 = T_rStatus;  // ReturnData["tid"].ToString();

                        //T_Er_Msg = "Y";
                    }
                    else
                    {
                        //T_Er_Msg = ReturnData["errMessage"].ToString();
                        s_Line1 = "N";
                    }

                }
                catch
                {

                    s_Line1 = "N";

                }



                //string s_Line1 = readerPost.ReadLine().ToString();
                //string s_Line2 = readerPost.ReadLine().ToString();
                //string s_Line3 = readerPost.ReadLine().ToString();
                //string s_Line4 = readerPost.ReadLine().ToString();

                //res_cd = s_Line3;
                //res_msg = s_Line4;

                //if (res_cd == "0000")
                //{
                //    T_Er_Msg = "";
                //    PG_Number = s_Line1;
                //    T_rAuthNo = s_Line2;
                //}
                //else
                //{
                //    PG_Number = "";
                //    T_rAuthNo = "";
                //    T_Er_Msg = res_cd + " " + res_msg;
                //}

                return PG_Number;



            }
            catch (Exception e)
            {
                return "";
            }

           
        }




































    }
    












}
