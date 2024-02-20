using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace MLM_Program
{
    class KSPayApprovalCancelBean
    {
        const string LOG_HOME = "C:/log/";

        const int SEND_TIMEOUT_MILLIS = 2000;
        const int RECV_TIMEOUT_MILLIS = 20000;

        private string IPAddr;
        private int Port;

        public string SendHeadMsg;			//Head Message
        public string SendDataMsg;

        public string ReceiveHeadMsg;			   //Head Message
        public string ReceiveDataMsg;

        public int SendCount;
        public int ReceiveCount;


        public KSPayApprovalCancelBean(string pIPAddr, int pPort)
        {
            this.IPAddr = pIPAddr;
            this.Port = pPort;

            this.SendCount = 0;
            this.ReceiveCount = 0;
        }

        // Haeder
        public string EncType,//	0: 암화안함, 2:	seed
                        Version,//	전문버전
                        Type,//	구분
                        Resend,//	전송구분 : 0 : 처음,  1: 재전송
                        RequestDate,//	요청일자 : yyyymmddhhmmss
                        StoreId,//	상점아이디
                        OrderNumber,//	주문번호
                        UserName,//	주문자명
                        IdNum,//	주민번호 or	사업자번호
                        Email,//	email
                        GoodType,//	제품구분 1 : 실물, 2 : 디지털
                        GoodName,//	제품명
                        KeyInType,//	KeyInType 여부 : S : Swap, K: KeyInType
                        LineType,//	lineType 0 : offline, 1:internet, 2:Mobile
                        PhoneNo,//	휴대폰번호
                        ApprovalCount,//	복합승인갯수
                        HeadFiller;//	예비


        // 신용카드승인결과
        public string ApprovalType,//	승인구분
                        TransactionNo,//	거래번호
                        Status,//	상태 O : 승인 ,	X :	거절
                        TradeDate,//	거래일자
                        TradeTime,//	거래시간
                        IssCode,//	발급사코드
                        AquCode,//	매입사코드
                        AuthNo,//	승인번호 or	거절시 오류코드
                        Message1,//	메시지1
                        Message2,//	메시지2
                        CardNo,//	카드번호
                        ExpDate,//	유효기간
                        Installment,//	할부
                        Amount,//	금액
                        MerchantNo,//	가맹점번호
                        AuthSendType,//	전송구분
                        ApprovalSendType,//	전송구분(0 : 거절, 1 : 승인, 2:	원카드)
                        Point1,//
                        Point2,//
                        Point3,//
                        Point4,//
                        VanTransactionNo,//	Van	거래번호
                        Filler,//	예비
                        AuthType,//	ISP	: ISP거래, MP1,	MP2	: MPI거래, SPACE : 일반거래
                        MPIPositionType,//	K :	KSNET, R : Remote, C : 제3기관,	SPACE :	일반거래
                        MPIReUseType,//	Y :	재사용,	N :	재사용아님
                        EncData;//	MPI, ISP 데이터

        // 가상계좌승인결과
        public string VATransactionNo,//	거래번호
                        VAStatus,//	상태 O : 승인 ,	X :	거절
                        VATradeDate,//	거래일자
                        VATradeTime,//	거래시간
                        VABankCode,//	은행코드
                        VAVirAcctNo,//	가상계좌번호
                        VAName,//	예금주
                        VACloseDate,//	은행일
                        VACloseTime,//	은행시간
                        VARespCode,//	응답코드
                        VAMessage1,//	메세지1
                        VAMessage2,//	메세지2
                        VAFiller;//	예비

        // 계좌이체승인결과
        public string
                        ACTransactionNo,//	거래번호
                        ACStatus,//	오류구분 :승인 X:거절
                        ACTradeDate,//	거래 개시 일자(YYYYMMDD)
                        ACTradeTime,//	거래 개시 시간(HHMMSS)
                        ACAcctSele,//	계좌이체 구분 -	2:PopBanking,4:새마을금고,5:금결원,6:PopBanking(휴대폰인증),8:CMA계좌이체
                        ACFeeSele,//	선/후불제구분 -	2:후불
                        ACInjaName,//	인자명(통장인쇄메세지-상점명)
                        ACPareBankCode,//	입금모계좌코드
                        ACPareAcctNo,//	입금모계좌번호
                        ACCustBankCode,//	출금모계좌코드
                        ACCustAcctNo,//	출금모계좌번호
                        ACAmount,//	금액	(결제대상금액)
                        ACBankTransactionNo,//	은행거래번호
                        ACIpgumNm,//	입금자명
                        ACBankFee,//	계좌이체 수수료
                        ACBankAmount,//	총결제금액(결제대상금액+ 수수료
                        ACBankRespCode,//	오류코드
                        ACMessage1,//	오류 message 1
                        ACMessage2,//	오류 message 2
                        ACEntrNumb,//	사업자번호
                        ACShopPhone,//	전화번호
                        ACCavvSele,//
                        ACFiller,//
                        ACEncData;//

        // 월드패스승인결과
        public string WPTransactionNo,
                        WPStatus,
                        WPTradeDate,
                        WPTradeTime,
                        WPIssCode,//발급사코드
                        WPAuthNo,//승인번호
                        WPBalanceAmount,//잔액
                        WPLimitAmount,//한도액
                        WPMessage1,//메시지1
                        WPMessage2,//메시지2
                        WPCardNo,//카드번호
                        WPAmount,//금액
                        WPMerchantNo,//가맹점번호
                        WPFiller;

        // 포인트카드승인결과
        public string PTransactionNo,//	거래번호
                        PStatus,//	상태 O : 승인 ,	X :	거절
                        PTradeDate,//	거래일자
                        PTradeTime,//	거래시간
                        PIssCode,//	발급사코드
                        PAuthNo,//	승인번호 or	거절시 오류코드
                        PMessage1,//	메시지1
                        PMessage2,//	메시지2
                        PPoint1,//	거래포인트
                        PPoint2,//	가용포인트
                        PPoint3,//	누적포인트
                        PPoint4,//	가맹점포인트
                        PMerchantNo,//	가맹점번호
                        PNotice1,//
                        PNotice2,//
                        PNotice3,//
                        PNotice4,//
                        PFiller;//	예비

        // 현금영수증승인결과
        public string HTransactionNo,//	거래번호
                        HStatus,//	오류구분 O:정상	X:거절
                        HCashTransactionNo,//	현금영수증 거래번호
                        HIncomeType,//	0: 소득		 1:	비소득
                        HTradeDate,//	거래 개시 일자
                        HTradeTime,//	거래 개시 시간
                        HMessage1,//	응답 message1
                        HMessage2,//	응답 message2
                        HCashMessage1,//	국세청 메시지 1
                        HCashMessage2,//	국세청 메시지 2
                        HFiller;//	예비

        // 상품권 온라인 PIN 발급 결과
        public string STTransactionNo,//	거래번호
                        STStatus,//	오류구분 - O:성공  X:실패 S : 확인필요 
                        STTradeDate,//	거래일자
                        STTradeTime,//	거래시간
                        STGoveSele,//	기관구분
                        STPinType,//	문화 M / 게임문화 G
                        STAuthNo,//	승인번호 - 오류시 오류코드
                        STRespMsg,//	메시지
                        STAmount,// 결제금액
                        STPinNumb,//	PIN 번호
                        STCertNo,//	관리번호
                        STExpDate,//	유효기간
                        STFiller;//	예비

        public static string format(string src, int len, char type)
        {
            byte[] buf = Encoding.Default.GetBytes(src);

            StringBuilder sb = new StringBuilder();

            int filler_len = len - buf.Length;

            if (filler_len == 0) return src;
            if (filler_len > 0)
            {
                sb.Append(src);
                for (int i = 0; i < filler_len; i++)
                {
                    if ('9' == type)
                        sb.Insert(0, '0');
                    else
                        sb.Append((char)0x20);
                }

                return sb.ToString();
            }

            int token_len = 0, tot_len = 0;
            char[] carr = src.ToCharArray();
            for (int i = 0; i < carr.Length; i++)
            {
                token_len = (0 == (byte)((carr[i] & 0xff00) >> 8)) ? 1 : 2;

                if ((token_len + tot_len) > len) break;
                sb.Append(carr[i]);
                tot_len += token_len;
            }

            filler_len = len - tot_len;

            for (int i = 0; i < filler_len; i++)
            {
                if ('9' == type)
                    sb.Insert(0, '0');
                else
                    sb.Append((char)0x20);
            }
            return sb.ToString();
        }

        public static void write_log(string str)
        {
            if (str == null) return;

            string curr_time = DateTime.Now.ToString("yyyyMMddhhmmss");
            string LOG_DATE;
            TextWriter LOG_WRITER = null;
            try
            {
                LOG_DATE = curr_time.Substring(0, 8);

                StringBuilder sb = new StringBuilder();
                sb.Append(LOG_HOME).Append("/kspay_").Append(LOG_DATE).Append(".log");

                LOG_WRITER = TextWriter.Synchronized(new StreamWriter(new FileStream(sb.ToString(), FileMode.Append, FileAccess.Write, System.IO.FileShare.ReadWrite)));

                sb.Remove(0, sb.Length);

                sb.Append("[").Append(curr_time.Substring(8, 2)).Append(":").Append(curr_time.Substring(10, 2)).Append(":").Append(curr_time.Substring(12, 2)).Append("] ");
                sb.Append(str);

                LOG_WRITER.WriteLine(sb.ToString()); LOG_WRITER.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine("로그생성실패 msg=[{0}]\n", e.Message);
                Console.WriteLine("로그생성실패 StackTrace=[{0}]\n", e.StackTrace);
            }
            finally
            {
                try
                {
                    if (LOG_WRITER != null)
                    {
                        LOG_WRITER.Close();
                        LOG_WRITER = null;
                    }
                }
                catch (Exception e)
                {
                }
            }
        }

        public Boolean SendSocket(string flag)
        {
            return ProcessRequest();
        }

        private Boolean ProcessRequest()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.SendHeadMsg);
            sb.Append(this.SendDataMsg);

            byte[] send_bytes = System.Text.Encoding.Default.GetBytes(sb.ToString());
            string SendLen = KSPayApprovalCancelBean.format("" + (send_bytes.Length - 4), 4, '9');
            byte[] send_len_bytes = System.Text.Encoding.Default.GetBytes(SendLen);

            sb.Remove(0, 4);
            sb.Insert(0, SendLen);
            Array.Copy(send_len_bytes, 0, send_bytes, 0, 4);

            return ProcessRequest(false, send_bytes);
        }

        private Boolean ProcessRequest(Boolean isRetry, byte[] send_bytes)
        {
            if (isRetry && (byte)'1' != send_bytes[300] && (byte)'I' != send_bytes[300]) return false;//신용카드만 재처리하자...
            if (isRetry)
            {
                this.SendCount = 0;
                this.ReceiveCount = 0;
                send_bytes[11] = (byte)'1';//재전송플래그설정..
            }

            TcpClient sock = new TcpClient(this.IPAddr, this.Port);

            byte[] read_bytes = new byte[8192];
            int read_len = 0, rtn_len = 0, len = 0;
            try
            {
                sock.SendTimeout = SEND_TIMEOUT_MILLIS;
                sock.ReceiveTimeout = RECV_TIMEOUT_MILLIS;

                NetworkStream stream = sock.GetStream();
                stream.Write(send_bytes, 0, send_bytes.Length);
                stream.Flush();
                KSPayApprovalCancelBean.write_log("INFO	: SendMsg(" + send_bytes.Length + "byte)=[" + System.Text.Encoding.Default.GetString(send_bytes) + "]!!");
                this.SendCount += 1;


                len = stream.Read(read_bytes, 0, 4);

                if (4 != len) throw new IOException("소켓에서 전문길이{0}를 읽을	수 없습니다.", len);
                len = Int32.Parse(System.Text.Encoding.Default.GetString(read_bytes, 0, 4));

                read_len = 4;
                do
                {
                    rtn_len = stream.Read(read_bytes, read_len, read_bytes.Length - read_len);
                    read_len += rtn_len;
                }
                while (stream.DataAvailable && read_len < len);
                this.ReceiveCount += 1;

                KSPayApprovalCancelBean.write_log("INFO	: RecvMsg(" + read_len + "byte)=[" + System.Text.Encoding.Default.GetString(read_bytes, 0, read_len) + "]");

            }
            catch (SocketException e)
            {
                KSPayApprovalCancelBean.write_log("ERROR : Time-out	occured:[" + e.Message + "]!!");

                if (!isRetry && e.ErrorCode == 10060) return ProcessRequest(true, send_bytes);//WSAETIMEOUT

                sock.Close();
                return false;
            }
            catch (IOException e)
            {
                KSPayApprovalCancelBean.write_log("ERROR : Socket access failed:[" + e.Message + "]!!");

                if (!isRetry && e.InnerException is System.Net.Sockets.SocketException && ((System.Net.Sockets.SocketException)(e.InnerException)).ErrorCode == 10060) return ProcessRequest(true, send_bytes);//WSAETIMEOUT

                sock.Close();
                return false;
            }

            Boolean ret = SetReceiveMessage(read_bytes, read_len);

            sock.Close();

            return ret;
        }

        public void HeadMessage
        (
            string pEncType,//	0: 암화안함, 2:	seed
            string pVersion,//	전문버전
            string pType,//	구분
            string pResend,//	전송구분 : 0 : 처음,  1: 재전송
            string pRequestDate,//	요청일자 : yyyymmddhhmmss
            string pStoreId,//	상점아이디
            string pOrderNumber,//	주문번호
            string pUserName,//	주문자명
            string pIdNum,//	주민번호 or	사업자번호
            string pEmail,//	email
            string pGoodType,//	제품구분 0 : 실물, 1 : 디지털
            string pGoodName,//	제품명
            string pKeyInType,//	KeyInType 여부 : S : Swap, K: KeyInType
            string pLineType,//	lineType 0 : offline, 1:internet, 2:Mobile
            string pPhoneNo,//	휴대폰번호
            string pApprovalCount,//	복합승인갯수
            string pFiller)//	예비
        {
            StringBuilder TmpHeadMsg = new StringBuilder();

            pEncType = KSPayApprovalCancelBean.format(pEncType, 1, 'X');
            pVersion = KSPayApprovalCancelBean.format(pVersion, 4, 'X');
            pType = KSPayApprovalCancelBean.format(pType, 2, 'X');
            pResend = KSPayApprovalCancelBean.format(pResend, 1, 'X');
            pRequestDate = KSPayApprovalCancelBean.format(pRequestDate, 14, 'X');
            pStoreId = KSPayApprovalCancelBean.format(pStoreId, 10, 'X');
            pOrderNumber = KSPayApprovalCancelBean.format(pOrderNumber, 50, 'X');
            pUserName = KSPayApprovalCancelBean.format(pUserName, 50, 'X');
            pIdNum = KSPayApprovalCancelBean.format(pIdNum, 13, 'X');
            pEmail = KSPayApprovalCancelBean.format(pEmail, 50, 'X');
            pGoodType = KSPayApprovalCancelBean.format(pGoodType, 1, 'X');
            pGoodName = KSPayApprovalCancelBean.format(pGoodName, 50, 'X');
            pKeyInType = KSPayApprovalCancelBean.format(pKeyInType, 1, 'X');
            pLineType = KSPayApprovalCancelBean.format(pLineType, 1, 'X');
            pPhoneNo = KSPayApprovalCancelBean.format(pPhoneNo, 12, 'X');
            pApprovalCount = KSPayApprovalCancelBean.format(pApprovalCount, 1, 'X');
            pFiller = KSPayApprovalCancelBean.format(pFiller, 35, 'X');

            TmpHeadMsg.Append("	   ");//길이는 나중에 채우자

            TmpHeadMsg.Append(pEncType);
            TmpHeadMsg.Append(pVersion);
            TmpHeadMsg.Append(pType);
            TmpHeadMsg.Append(pResend);
            TmpHeadMsg.Append(pRequestDate);
            TmpHeadMsg.Append(pStoreId);
            TmpHeadMsg.Append(pOrderNumber);
            TmpHeadMsg.Append(pUserName);
            TmpHeadMsg.Append(pIdNum);
            TmpHeadMsg.Append(pEmail);
            TmpHeadMsg.Append(pGoodType);
            TmpHeadMsg.Append(pGoodName);
            TmpHeadMsg.Append(pKeyInType);
            TmpHeadMsg.Append(pLineType);
            TmpHeadMsg.Append(pPhoneNo);
            TmpHeadMsg.Append(pApprovalCount);
            TmpHeadMsg.Append(pFiller);

            this.SendHeadMsg = TmpHeadMsg.ToString();
        }

        // 카드Bin Check
        public void CardBinViewDataMessage(
            string pApprovalType,//	승인구분
            string pTrackII,//	카드번호=유효기간
            string pFiller)//	예비
        {
            StringBuilder TmpSendMsg = new StringBuilder();

            pApprovalType = KSPayApprovalCancelBean.format(pApprovalType, 4, 'X');
            pTrackII = KSPayApprovalCancelBean.format(pTrackII, 40, 'X');
            pFiller = KSPayApprovalCancelBean.format(pFiller, 56, 'X');

            this.SendDataMsg += TmpSendMsg.ToString();
        }

        //신용카드승인요청 Body	1
        public void CreditDataMessage(
            string pApprovalType,//	승인구분
            string pInterestType,//	일반/무이자구분	1:일반 2:무이자
            string pTrackII,//	카드번호=유효기간  or 거래번호
            string pInstallment,//	할부  00일시불
            string pAmount,//	금액
            string pPasswd,//	비밀번호 앞2자리
            string pIdNum,//	주민번호  뒤7자리, 사업자번호10
            string pCurrencyType,//	통화구분 0:원화	1: 미화
            string pBatchUseType,//	거래번호배치사용구분  0:미사용 1:사용
            string pCardSendType,//	카드정보전송 0:미전송 2:카드번호앞14자리 + "XXXX",유효기간,할부,금액,가맹점번호
            string pVisaAuthYn,//	비자인증유무 0:사용안함,7:SSL,9:비자인증
            string pDomain,//	도메인 자체가맹점(PG업체용)
            string pIpAddr,//	IP ADDRESS 자체가맹점(PG업체용)
            string pBusinessNumber,//	사업자 번호	자체가맹점(PG업체용)
            string pFiller,//	예비
            string pAuthType,//	ISP	: ISP거래, MP1,	MP2	: MPI거래, SPACE : 일반거래
            string pMPIPositionType,//	K :	KSNET, R : Remote, C : 제3기관,	SPACE :	일반거래
            string pMPIReUseType,//	Y :	 재사용, N : 재사용아님
            string pEncData)//	MPI, ISP 데이터
        {
            StringBuilder TmpSendMsg = new StringBuilder();

            pApprovalType = KSPayApprovalCancelBean.format(pApprovalType, 4, 'X');
            pInterestType = KSPayApprovalCancelBean.format(pInterestType, 1, 'X');
            pTrackII = KSPayApprovalCancelBean.format(pTrackII, 40, 'X');
            pInstallment = KSPayApprovalCancelBean.format(pInstallment, 2, '9');
            pAmount = KSPayApprovalCancelBean.format(pAmount, 9, '9');
            pPasswd = KSPayApprovalCancelBean.format(pPasswd, 2, 'X');
            pIdNum = KSPayApprovalCancelBean.format(pIdNum, 10, 'X');
            pCurrencyType = KSPayApprovalCancelBean.format(pCurrencyType, 1, 'X');
            pBatchUseType = KSPayApprovalCancelBean.format(pBatchUseType, 1, 'X');
            pCardSendType = KSPayApprovalCancelBean.format(pCardSendType, 1, 'X');
            pVisaAuthYn = KSPayApprovalCancelBean.format(pVisaAuthYn, 1, 'X');
            pDomain = KSPayApprovalCancelBean.format(pDomain, 40, 'X');
            pIpAddr = KSPayApprovalCancelBean.format(pIpAddr, 20, 'X');
            pBusinessNumber = KSPayApprovalCancelBean.format(pBusinessNumber, 10, 'X');
            pFiller = KSPayApprovalCancelBean.format(pFiller, 135, 'X');
            pAuthType = KSPayApprovalCancelBean.format(pAuthType, 1, 'X');
            pMPIPositionType = KSPayApprovalCancelBean.format(pMPIPositionType, 1, 'X');
            pMPIReUseType = KSPayApprovalCancelBean.format(pMPIReUseType, 1, 'X');

            TmpSendMsg.Append(pApprovalType);
            TmpSendMsg.Append(pInterestType);
            TmpSendMsg.Append(pTrackII);
            TmpSendMsg.Append(pInstallment);
            TmpSendMsg.Append(pAmount);
            TmpSendMsg.Append(pPasswd);
            TmpSendMsg.Append(pIdNum);
            TmpSendMsg.Append(pCurrencyType);
            TmpSendMsg.Append(pBatchUseType);
            TmpSendMsg.Append(pCardSendType);
            TmpSendMsg.Append(pVisaAuthYn);
            TmpSendMsg.Append(pDomain);
            TmpSendMsg.Append(pIpAddr);
            TmpSendMsg.Append(pBusinessNumber);
            TmpSendMsg.Append(pFiller);
            TmpSendMsg.Append(pAuthType);
            TmpSendMsg.Append(pMPIPositionType);
            TmpSendMsg.Append(pMPIReUseType);
            TmpSendMsg.Append(pEncData);

            this.SendDataMsg += TmpSendMsg.ToString();
        }

        //가상계좌(pVersion:0603)
        public void VirtualAccountDataMessage(
            string pApprovalType,//	승인구분
            string pBankCode,//	은행코드
            string pAmount,//	금액
            string pCloseDate,//	은행일
            string pCloseTime,//	은행시간
            string pEscrowSele,//	에스크로적용구분: 0:적용안함, 1:적용, 2:강제적용
            string pVirFixSele,//	가상계좌번호지정구분
            string pVirAcctNo,//	가상계좌번호
            string pOrgTransactionNo,//	원거래거래번호
            string pFiller)//	예비
        {
            StringBuilder TmpSendMsg = new StringBuilder();

            pApprovalType = KSPayApprovalCancelBean.format(pApprovalType, 4, 'X');
            pBankCode = KSPayApprovalCancelBean.format(pBankCode, 6, 'X');
            pAmount = KSPayApprovalCancelBean.format(pAmount, 9, '9');
            pCloseDate = KSPayApprovalCancelBean.format(pCloseDate, 8, 'X');
            pCloseTime = KSPayApprovalCancelBean.format(pCloseTime, 6, 'X');
            pEscrowSele = KSPayApprovalCancelBean.format(pEscrowSele, 1, 'X');
            pVirFixSele = KSPayApprovalCancelBean.format(pVirFixSele, 1, 'X');
            pVirAcctNo = KSPayApprovalCancelBean.format(pVirAcctNo, 15, 'X');
            pOrgTransactionNo = KSPayApprovalCancelBean.format(pOrgTransactionNo, 12, 'X');
            pFiller = KSPayApprovalCancelBean.format(pFiller, 52, 'X');

            TmpSendMsg.Append(pApprovalType);
            TmpSendMsg.Append(pBankCode);
            TmpSendMsg.Append(pAmount);
            TmpSendMsg.Append(pCloseDate);
            TmpSendMsg.Append(pCloseTime);
            TmpSendMsg.Append(pEscrowSele);
            TmpSendMsg.Append(pVirFixSele);
            TmpSendMsg.Append(pVirAcctNo);
            TmpSendMsg.Append(pOrgTransactionNo);
            TmpSendMsg.Append(pFiller);

            this.SendDataMsg += TmpSendMsg.ToString();
        }
        // 계좌이체 시작 요청전문 생성(send)
        public void AcctRequest_send(
            string pApprovalType,  		// 승인구분
            string pAcctSele,  		// 계좌이체 유형구문
            string pFeeSele,  		// 선/후불제구분
            string pPareBankCode,  		// 모계좌은행코드      
            string pPareAcctNo,  		// 모계좌번호          
            string pCustBankCode,  		// 고객계좌은행코드    
            string pAmount,  		// 금액                
            string pInjaName,      	// 인자명(상점명)      
            string pFiller)  // 기타               
        {
            StringBuilder TmpSendMsg = new StringBuilder();

            pApprovalType = KSPayApprovalCancelBean.format(pApprovalType, 4, 'X');
            pAcctSele = KSPayApprovalCancelBean.format(pAcctSele, 1, 'X');
            pFeeSele = KSPayApprovalCancelBean.format(pFeeSele, 1, 'X');
            pPareBankCode = KSPayApprovalCancelBean.format(pPareBankCode, 6, 'X');
            pPareAcctNo = KSPayApprovalCancelBean.format(pPareAcctNo, 15, 'X');
            pCustBankCode = KSPayApprovalCancelBean.format(pCustBankCode, 6, 'X');
            pAmount = KSPayApprovalCancelBean.format(pAmount, 13, '9');
            pInjaName = KSPayApprovalCancelBean.format(pInjaName, 16, 'X');
            pFiller = KSPayApprovalCancelBean.format(pFiller, 38, 'X');


            TmpSendMsg.Append(pApprovalType);
            TmpSendMsg.Append(pAcctSele);
            TmpSendMsg.Append(pFeeSele);
            TmpSendMsg.Append(pPareBankCode);
            TmpSendMsg.Append(pPareAcctNo);
            TmpSendMsg.Append(pCustBankCode);
            TmpSendMsg.Append(pAmount);
            TmpSendMsg.Append(pInjaName);
            TmpSendMsg.Append(pFiller);


            this.SendDataMsg += TmpSendMsg.ToString();
        }




        //계좌이체 인증승인	요청전문을 만든다.
        public void AcctRequest_iappr(
            string pApprovalType,//	승인구분	코드
            string pAcctSele,//	계좌이체 구분 -	2:PopBanking,4:새마을금고,5:금결원,6:PopBanking(휴대폰인증),8:CMA계좌이체
            string pFeeSele,//	계좌이체 구분 -	선/후불제구분 -	2:후불
            string pTransactionNo,//	거래번호
            string pBankCode,//	입금모계좌코드
            string pAmount,//	금액	(결제대상금액)
            string pCustBankInja,//	출금모계좌코드
            string pBankTransactionNo,//	은행거래번호
            string pFiller,//
            string pCertData)//	인증정보
        {
            StringBuilder TmpSendMsg = new StringBuilder();

            pApprovalType = KSPayApprovalCancelBean.format(pApprovalType, 4, 'X');
            pAcctSele = KSPayApprovalCancelBean.format(pAcctSele, 1, 'X');
            pFeeSele = KSPayApprovalCancelBean.format(pFeeSele, 1, 'X');
            pTransactionNo = KSPayApprovalCancelBean.format(pTransactionNo, 12, 'X');
            pBankCode = KSPayApprovalCancelBean.format(pBankCode, 6, 'X');
            pAmount = KSPayApprovalCancelBean.format(pAmount, 13, '9');
            pCustBankInja = KSPayApprovalCancelBean.format(pCustBankInja, 30, 'X');
            pBankTransactionNo = KSPayApprovalCancelBean.format(pBankTransactionNo, 30, 'X');
            pFiller = KSPayApprovalCancelBean.format(pFiller, 53, 'X');

            TmpSendMsg.Append(pApprovalType);
            TmpSendMsg.Append(pAcctSele);
            TmpSendMsg.Append(pFeeSele);
            TmpSendMsg.Append(pTransactionNo);
            TmpSendMsg.Append(pBankCode);
            TmpSendMsg.Append(pAmount);
            TmpSendMsg.Append(pCustBankInja);
            TmpSendMsg.Append(pBankTransactionNo);
            TmpSendMsg.Append(pFiller);
            TmpSendMsg.Append(pCertData);

            this.SendDataMsg += TmpSendMsg.ToString();
        }

        // 월드패스승인
        public void WorldPassDataMessage(
            string pApprovalType,//	승인구분
            string pTrackII,//	카드번호=4912  or 거래번호
            string pPasswd,//	비밀번호 앞2자리
            string pAmount,//	금액
            string pWorldPassType,//	선후불카드구분
            string pAdultType,//	성인확인구분
            string pCardSendType,//	카드정보전송 0:미전송  2:카드번호앞14자리 +	"XXXX",유효기간,할부,금액,가맹점번호
            string pFiller)//	예비
        {
            StringBuilder TmpSendMsg = new StringBuilder();

            pApprovalType = KSPayApprovalCancelBean.format(pApprovalType, 4, 'X');
            pTrackII = KSPayApprovalCancelBean.format(pTrackII, 40, 'X');
            pPasswd = KSPayApprovalCancelBean.format(pPasswd, 4, 'X');
            pAmount = KSPayApprovalCancelBean.format(pAmount, 9, '9');
            pWorldPassType = KSPayApprovalCancelBean.format(pWorldPassType, 1, 'X');
            pAdultType = KSPayApprovalCancelBean.format(pAdultType, 1, 'X');
            pCardSendType = KSPayApprovalCancelBean.format(pCardSendType, 1, 'X');
            pFiller = KSPayApprovalCancelBean.format(pFiller, 40, 'X');

            this.SendDataMsg += TmpSendMsg.ToString();
        }

        // 포인트카드승인
        public void PointDataMessage(
            string pApprovalType,//	승인구분
            string pTrackII,//	카드번호=유효기간  or 거래번호
            string pAmount,//	금액
            string pPasswd,//	비밀번호 앞4자리
            string pSaleType,//	판매구분
            string pFiller)//	예비
        {
            StringBuilder TmpSendMsg = new StringBuilder();

            pApprovalType = KSPayApprovalCancelBean.format(pApprovalType, 4, 'X');
            pTrackII = KSPayApprovalCancelBean.format(pTrackII, 40, 'X');
            pAmount = KSPayApprovalCancelBean.format(pAmount, 9, '9');
            pPasswd = KSPayApprovalCancelBean.format(pPasswd, 4, 'X');
            pSaleType = KSPayApprovalCancelBean.format(pSaleType, 2, 'X');
            pFiller = KSPayApprovalCancelBean.format(pFiller, 41, 'X');

            this.SendDataMsg += TmpSendMsg.ToString();
        }

        // 현금영수증승인
        public void CashBillDataMessage(
            string pApprovalType,//	H000:일반발급, H200:계좌이체,	H600:가상계좌
            string pTransactionNo,//	입금완료된 계좌이체, 가상계좌	거래번호
            string pIssuSele,//	0:일반발급(PG원거래번호	중복체크), 1:단독발급(주문번호 중복체크	:	PG원거래 없음),	2:강제발급(중복체크	안함)
            string pUserInfoSele,//	0:주민등록번호
            string pUserInfo,//	주민등록번호
            string pTranSele,//	0: 개인, 1:	사업자
            string pCallCode,//	통화코드	(0:	원화, 1: 미화)
            string pSupplyAmt,//	공급가액
            string pTaxAmt,//	세금
            string pSvcAmt,//	봉사료
            string pTotAmt,//	현금영수증 발급금액
            string pFiller)//	예비
        {
            StringBuilder TmpSendMsg = new StringBuilder();

            pApprovalType = KSPayApprovalCancelBean.format(pApprovalType, 4, 'X');
            pTransactionNo = KSPayApprovalCancelBean.format(pTransactionNo, 12, 'X');
            pIssuSele = KSPayApprovalCancelBean.format(pIssuSele, 1, 'X');
            pUserInfoSele = KSPayApprovalCancelBean.format(pUserInfoSele, 1, 'X');
            pUserInfo = KSPayApprovalCancelBean.format(pUserInfo, 37, 'X');
            pTranSele = KSPayApprovalCancelBean.format(pTranSele, 1, 'X');
            pCallCode = KSPayApprovalCancelBean.format(pCallCode, 1, 'X');
            pSupplyAmt = KSPayApprovalCancelBean.format(pSupplyAmt, 9, '9');
            pTaxAmt = KSPayApprovalCancelBean.format(pTaxAmt, 9, '9');
            pSvcAmt = KSPayApprovalCancelBean.format(pSvcAmt, 9, '9');
            pTotAmt = KSPayApprovalCancelBean.format(pTotAmt, 9, '9');
            pFiller = KSPayApprovalCancelBean.format(pFiller, 147, 'X');

            TmpSendMsg.Append(pApprovalType);
            TmpSendMsg.Append(pTransactionNo);
            TmpSendMsg.Append(pIssuSele);
            TmpSendMsg.Append(pUserInfoSele);
            TmpSendMsg.Append(pUserInfo);
            TmpSendMsg.Append(pTranSele);
            TmpSendMsg.Append(pCallCode);
            TmpSendMsg.Append(pSupplyAmt);
            TmpSendMsg.Append(pTaxAmt);
            TmpSendMsg.Append(pSvcAmt);
            TmpSendMsg.Append(pTotAmt);
            TmpSendMsg.Append(pFiller);

            this.SendDataMsg += TmpSendMsg.ToString();
        }

        // 상품권 온라인 PIn발급 요청전문 생성
        public void SticketPinDataMessage(
             string pApprovalType,	// 승인구분 코드       
             string pGoveCode, 		// 기관코드            
             string pGoveSele, 		// 기관구분            
             string pPinSele, 		// 문화 M / 게임문화 G	
             string pAmount, 		// 결제금액            
             string pFiller)		//                     
        {
            StringBuilder TmpSendMsg = new StringBuilder();

            pApprovalType = KSPayApprovalCancelBean.format(pApprovalType, 4, 'X');
            pGoveCode = KSPayApprovalCancelBean.format(pGoveCode, 10, 'X');
            pGoveSele = KSPayApprovalCancelBean.format(pGoveSele, 10, 'X');
            pPinSele = KSPayApprovalCancelBean.format(pPinSele, 1, 'X');
            pAmount = KSPayApprovalCancelBean.format(pAmount, 9, '9');
            pFiller = KSPayApprovalCancelBean.format(pFiller, 116, 'X');

            TmpSendMsg.Append(pApprovalType);
            TmpSendMsg.Append(pGoveCode);
            TmpSendMsg.Append(pGoveSele);
            TmpSendMsg.Append(pPinSele);
            TmpSendMsg.Append(pAmount);
            TmpSendMsg.Append(pFiller);

            this.SendDataMsg += TmpSendMsg.ToString();
        }

        //취소
        public void CancelDataMessage(
            string pApprovalType,//	승인구분
            string pCancelType,//	취소처리구분 0:거래번호, 1:주문번호
            string pTransactionNo,//	거래번호
            string pTradeDate,//	거래일자
            string pOrderNumber,//	주문번호
            string pCancelData,//	취소데이타(차후제공)
            string pRefundcheck,//현금영수증 취소여부 (1.거래취소, 2.오류발급취소, 3.기타) 
            string pFiller)//	기타
        {
            StringBuilder TmpSendMsg = new StringBuilder();

            pApprovalType = KSPayApprovalCancelBean.format(pApprovalType, 4, 'X');
            pCancelType = KSPayApprovalCancelBean.format(pCancelType, 1, 'X');
            pTransactionNo = KSPayApprovalCancelBean.format(pTransactionNo, 12, 'X');
            pTradeDate = KSPayApprovalCancelBean.format(pTradeDate, 8, 'X');
            pOrderNumber = KSPayApprovalCancelBean.format(pOrderNumber, 50, 'X');
            pCancelData = KSPayApprovalCancelBean.format(pCancelData, 42, 'X');
            pRefundcheck = KSPayApprovalCancelBean.format(pRefundcheck, 1, 'X');
            pFiller = KSPayApprovalCancelBean.format(pFiller, 32, 'X');

            TmpSendMsg.Append(pApprovalType);
            TmpSendMsg.Append(pCancelType);
            TmpSendMsg.Append(pTransactionNo);
            TmpSendMsg.Append(pTradeDate);
            TmpSendMsg.Append(pOrderNumber);
            TmpSendMsg.Append(pCancelData);
            TmpSendMsg.Append(pRefundcheck);

            TmpSendMsg.Append(pFiller);

            this.SendDataMsg += TmpSendMsg.ToString();
        }

        //승인이후에 결과값을 채운다.
        public Boolean SetReceiveMessage(byte[] read_bytes, int len)
        {
            int idx = 0;
            if (len < 300 + 50)
            {
                KSPayApprovalCancelBean.write_log("WARN	: too short	head_msg(" + len + ")=[" + Encoding.Default.GetString(read_bytes, 0, len) + "]!!");
                return false;
            }

            StringBuilder TmpHeadMsg = new StringBuilder();

            string MsgLen = Encoding.Default.GetString(read_bytes, idx, 4); idx += 4; TmpHeadMsg.Append(MsgLen);//	데이터 길이
            this.EncType = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpHeadMsg.Append(EncType);//	0: 암화안함, 1:openssl,	2: seed
            this.Version = Encoding.Default.GetString(read_bytes, idx, 4); idx += 4; TmpHeadMsg.Append(Version);//	전문버전
            this.Type = Encoding.Default.GetString(read_bytes, idx, 2); idx += 2; TmpHeadMsg.Append(Type);//	구분
            this.Resend = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpHeadMsg.Append(Resend);//	전송구분 : 0 : 처음,  2: 재전송
            this.RequestDate = Encoding.Default.GetString(read_bytes, idx, 14); idx += 14; TmpHeadMsg.Append(RequestDate);//	요청일자 : yyyymmddhhmmss
            this.StoreId = Encoding.Default.GetString(read_bytes, idx, 10); idx += 10; TmpHeadMsg.Append(StoreId);//	상점아이디
            this.OrderNumber = Encoding.Default.GetString(read_bytes, idx, 50); idx += 50; TmpHeadMsg.Append(OrderNumber);//	주문번호
            this.UserName = Encoding.Default.GetString(read_bytes, idx, 50); idx += 50; TmpHeadMsg.Append(UserName);//	주문자명
            this.IdNum = Encoding.Default.GetString(read_bytes, idx, 13); idx += 13; TmpHeadMsg.Append(IdNum);//	주민번호 or	사업자번호
            this.Email = Encoding.Default.GetString(read_bytes, idx, 50); idx += 50; TmpHeadMsg.Append(Email);//	email
            this.GoodType = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpHeadMsg.Append(GoodType);//	제품구분 0 : 실물, 1 : 디지털
            this.GoodName = Encoding.Default.GetString(read_bytes, idx, 50); idx += 50; TmpHeadMsg.Append(GoodName);//	제품명
            this.KeyInType = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpHeadMsg.Append(KeyInType);//	KeyInType 여부 : 1 : Swap, 2: KeyIn
            this.LineType = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpHeadMsg.Append(LineType);//	lineType 0 : offline, 1:internet, 2:Mobile
            this.PhoneNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpHeadMsg.Append(PhoneNo);//	휴대폰번호
            this.ApprovalCount = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpHeadMsg.Append(ApprovalCount);//	승인갯수
            this.HeadFiller = Encoding.Default.GetString(read_bytes, idx, 35); idx += 35; TmpHeadMsg.Append(HeadFiller);//	예비

            this.ReceiveHeadMsg = TmpHeadMsg.ToString();

            StringBuilder TmpReceiveMsg = new StringBuilder();

            this.ApprovalType = Encoding.Default.GetString(read_bytes, idx, 4); idx += 4; TmpReceiveMsg.Append(ApprovalType); //	승인구분

            if (this.ApprovalType.StartsWith("150"))
            {
                this.TransactionNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(TransactionNo); // 거래번호
                this.Status = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(Status); // 상태 O : 승인, X : 거절
                this.TradeDate = Encoding.Default.GetString(read_bytes, idx, 8); idx += 8; TmpReceiveMsg.Append(TradeDate); // 거래일자
                this.TradeTime = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(TradeTime); // 거래시간
                this.IssCode = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(IssCode); // 발급사코드
                this.Message1 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(Message1); // 메시지1
                this.Message2 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(Message2); // 메시지2
                this.Filler = Encoding.Default.GetString(read_bytes, idx, 31); idx += 31; TmpReceiveMsg.Append(Message2); // 예비
            }
            else
                if (this.ApprovalType.StartsWith("16"))
                {
                    KSPayApprovalCancelBean.write_log("WARN	: not_implemented_msg" + this.ApprovalType + ")!!");
                    return false;
                }
                else
                    if (this.ApprovalType.StartsWith("1") ||
                        this.ApprovalType.StartsWith("I"))
                    {
                        this.TransactionNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(TransactionNo); //거래번호
                        this.Status = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(Status); //상태 O	: 승인,	X :	거절
                        this.TradeDate = Encoding.Default.GetString(read_bytes, idx, 8); idx += 8; TmpReceiveMsg.Append(TradeDate); //거래일자
                        this.TradeTime = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(TradeTime); //거래시간
                        this.IssCode = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(IssCode); //발급사코드
                        this.AquCode = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(AquCode); //매입사코드
                        this.AuthNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(AuthNo); //승인번호 or 거절시	오류코드
                        this.Message1 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(Message1); //메시지1
                        this.Message2 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(Message2); //메시지2
                        this.CardNo = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(CardNo); //카드번호
                        this.ExpDate = Encoding.Default.GetString(read_bytes, idx, 4); idx += 4; TmpReceiveMsg.Append(ExpDate); //유효기간
                        this.Installment = Encoding.Default.GetString(read_bytes, idx, 2); idx += 2; TmpReceiveMsg.Append(Installment); //할부
                        this.Amount = Encoding.Default.GetString(read_bytes, idx, 9); idx += 9; TmpReceiveMsg.Append(Amount); //금액
                        this.MerchantNo = Encoding.Default.GetString(read_bytes, idx, 15); idx += 15; TmpReceiveMsg.Append(MerchantNo); //가맹점번호
                        this.AuthSendType = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(AuthSendType); //전송구분= new String(this.read(2));
                        this.ApprovalSendType = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(ApprovalSendType); //전송구분(0	: 거절,	1 :	승인, 2: 원카드)
                        this.Point1 = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(Point1); //Point1
                        this.Point2 = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(Point2); //Point2
                        this.Point3 = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(Point3); //Point3
                        this.Point4 = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(Point4); //Point4
                        this.VanTransactionNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(VanTransactionNo); //Point4
                        this.Filler = Encoding.Default.GetString(read_bytes, idx, 82); idx += 82; TmpReceiveMsg.Append(Filler); //예비
                        this.AuthType = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(AuthType); //I : ISP거래, M	: MPI거래, SPACE : 일반거래

                        if (read_bytes.Length > 7)
                        {
                            this.MPIPositionType = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(MPIPositionType); // K	: KSNET, R : Remote, C : 제3기관, SPACE	: 일반거래
                            this.MPIReUseType = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(MPIReUseType); // Y	: 재사용, N	: 재사용아님
                            this.EncData = Encoding.Default.GetString(read_bytes, idx, read_bytes.Length - idx); TmpReceiveMsg.Append(EncData); //
                        }
                    }
                    else
                        if (this.ApprovalType.StartsWith("210") ||
                            this.ApprovalType.StartsWith("240"))
                        {
                            this.ACTransactionNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(ACTransactionNo); 	//' 거래번호
                            this.ACStatus = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(ACStatus); 	//' 오류구분:- O:승인 X:거절
                            this.ACTradeDate = Encoding.Default.GetString(read_bytes, idx, 8); idx += 8; TmpReceiveMsg.Append(ACTradeDate); 	//' 거래 개시 일자(YYYYMMDD)
                            this.ACTradeTime = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(ACTradeTime); 	//' 거래 개시 시간(HHMMSS)
                            this.ACAcctSele = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(ACAcctSele); 	//' 계좌이체 구분 -1:Dacom, 2:Pop Banking,	3:실시간계좌이체, 4:X
                            this.ACFeeSele = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(ACFeeSele); 	//' 선/후불제구분 -1:선불,2:후불
                            this.ACPareBankCode = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(ACPareBankCode); 	//' 입금모계좌은행코드
                            this.ACPareAcctNo = Encoding.Default.GetString(read_bytes, idx, 15); idx += 15; TmpReceiveMsg.Append(ACPareAcctNo); 	//' 입금모계좌 번호
                            this.ACCustBankCode = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(ACCustBankCode); 	//' 출급은행코드
                            this.ACAmount = Encoding.Default.GetString(read_bytes, idx, 13); idx += 13; TmpReceiveMsg.Append(ACAmount); 	//' 금액
                            this.ACInjaName = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(ACInjaName); 	//' 인자명(상점명)
                            this.ACMessage1 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(ACMessage1); 	//' 응답 message1
                            this.ACMessage2 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(ACMessage2); 	//' 응답 message2
                            this.ACEntrNumb = Encoding.Default.GetString(read_bytes, idx, 10); idx += 10; TmpReceiveMsg.Append(ACEntrNumb); 	//' 사업자번호	
                            this.ACShopPhone = Encoding.Default.GetString(read_bytes, idx, 20); idx += 20; TmpReceiveMsg.Append(ACShopPhone); 	//' 전화번호	
                            this.ACFiller = Encoding.Default.GetString(read_bytes, idx, 49); idx += 49; TmpReceiveMsg.Append(ACFiller); 	//' 예비
                            //KSPayApprovalCancelBean.write_log("WARN	: not_implemented_msg"+this.ApprovalType+")!!");

                        }
                        else
                            if (this.ApprovalType.StartsWith("2"))
                            {
                                this.ACTransactionNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(ACTransactionNo); // 거래번호
                                this.ACStatus = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(ACStatus); // 오류구분 :승인 X:거절
                                this.ACTradeDate = Encoding.Default.GetString(read_bytes, idx, 8); idx += 8; TmpReceiveMsg.Append(ACTradeDate); // 거래 개시	일자(YYYYMMDD)
                                this.ACTradeTime = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(ACTradeTime); // 거래 개시	시간(HHMMSS)
                                this.ACAcctSele = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(ACAcctSele); // 계좌이체 구분	-	1:Dacom, 2:Pop Banking,	3:실시간계좌이체 4:	승인형계좌이체
                                this.ACFeeSele = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(ACFeeSele); // 선/후불제구분	-	1:선불,	2:후불
                                this.ACInjaName = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(ACInjaName); // 인자명(통장인쇄메세지-상점명)
                                this.ACPareBankCode = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(ACPareBankCode); // 입금모계좌코드
                                this.ACPareAcctNo = Encoding.Default.GetString(read_bytes, idx, 15); idx += 15; TmpReceiveMsg.Append(ACPareAcctNo); // 입금모계좌번호
                                this.ACCustBankCode = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(ACCustBankCode); // 출금모계좌코드
                                this.ACCustAcctNo = Encoding.Default.GetString(read_bytes, idx, 15); idx += 15; TmpReceiveMsg.Append(ACCustAcctNo); // 출금모계좌번호
                                this.ACAmount = Encoding.Default.GetString(read_bytes, idx, 13); idx += 13; TmpReceiveMsg.Append(ACAmount); // 금액	(결제대상금액)
                                this.ACBankTransactionNo = Encoding.Default.GetString(read_bytes, idx, 30); idx += 30; TmpReceiveMsg.Append(ACBankTransactionNo); // 은행거래번호
                                this.ACIpgumNm = Encoding.Default.GetString(read_bytes, idx, 20); idx += 20; TmpReceiveMsg.Append(ACIpgumNm); // 입금자명
                                this.ACBankFee = Encoding.Default.GetString(read_bytes, idx, 13); idx += 13; TmpReceiveMsg.Append(ACBankFee); // 계좌이체 수수료
                                this.ACBankAmount = Encoding.Default.GetString(read_bytes, idx, 13); idx += 13; TmpReceiveMsg.Append(ACBankAmount); // 총결제금액(결제대상금액+ 수수료
                                this.ACBankRespCode = Encoding.Default.GetString(read_bytes, idx, 4); idx += 4; TmpReceiveMsg.Append(ACBankRespCode); // 오류코드
                                this.ACMessage1 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(ACMessage1); // 오류 message 1
                                this.ACMessage2 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(ACMessage2); // 오류 message 2
                                this.ACCavvSele = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(ACCavvSele); // 암호화데이터응답여부
                                this.ACFiller = Encoding.Default.GetString(read_bytes, idx, 183); idx += 183; TmpReceiveMsg.Append(ACFiller); // 예비

                                string EncLen = "";
                                this.ACEncData = "";
                                if (ACCavvSele.Equals("1"))
                                {
                                    EncLen = Encoding.Default.GetString(read_bytes, idx, 5); idx += 5; TmpReceiveMsg.Append(EncLen);
                                    this.ACEncData = Encoding.Default.GetString(read_bytes, idx, Int32.Parse(EncLen)); TmpReceiveMsg.Append(ACEncData); // 금결원암호화응답
                                };
                            }
                            else
                                if (this.ApprovalType.StartsWith("4"))
                                {
                                    this.PTransactionNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(PTransactionNo); // 거래번호
                                    this.PStatus = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(PStatus); // 상태 O : 승인	, X	: 거절
                                    this.PTradeDate = Encoding.Default.GetString(read_bytes, idx, 8); idx += 8; TmpReceiveMsg.Append(PTradeDate); // 거래일자
                                    this.PTradeTime = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(PTradeTime); // 거래시간
                                    this.PIssCode = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(PIssCode); // 발급사코드
                                    this.PAuthNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(PAuthNo); // 승인번호 or 거절시 오류코드
                                    this.PMessage1 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(PMessage1); // 메시지1
                                    this.PMessage2 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(PMessage2); // 메시지2
                                    this.PPoint1 = Encoding.Default.GetString(read_bytes, idx, 9); idx += 9; TmpReceiveMsg.Append(PPoint1); // 거래포인트
                                    this.PPoint2 = Encoding.Default.GetString(read_bytes, idx, 9); idx += 9; TmpReceiveMsg.Append(PPoint2); // 가용포인트
                                    this.PPoint3 = Encoding.Default.GetString(read_bytes, idx, 9); idx += 9; TmpReceiveMsg.Append(PPoint3); // 누적포인트
                                    this.PPoint4 = Encoding.Default.GetString(read_bytes, idx, 9); idx += 9; TmpReceiveMsg.Append(PPoint4); // 가맹점포인트
                                    this.PMerchantNo = Encoding.Default.GetString(read_bytes, idx, 15); idx += 15; TmpReceiveMsg.Append(PMerchantNo); // 가맹점번호
                                    this.PNotice1 = Encoding.Default.GetString(read_bytes, idx, 40); idx += 40; TmpReceiveMsg.Append(PNotice1); //
                                    this.PNotice2 = Encoding.Default.GetString(read_bytes, idx, 40); idx += 40; TmpReceiveMsg.Append(PNotice2); //
                                    this.PNotice3 = Encoding.Default.GetString(read_bytes, idx, 40); idx += 40; TmpReceiveMsg.Append(PNotice3); //
                                    this.PNotice4 = Encoding.Default.GetString(read_bytes, idx, 40); idx += 40; TmpReceiveMsg.Append(PNotice4); //
                                    this.PFiller = Encoding.Default.GetString(read_bytes, idx, 8); idx += 8; TmpReceiveMsg.Append(PFiller); // 예비
                                }
                                else
                                    if (this.ApprovalType.StartsWith("60"))
                                    {
                                        this.VATransactionNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(VATransactionNo); // 거래번호
                                        this.VAStatus = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(VAStatus); // 상태
                                        this.VATradeDate = Encoding.Default.GetString(read_bytes, idx, 8); idx += 8; TmpReceiveMsg.Append(VATradeDate); // 거래일자
                                        this.VATradeTime = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(VATradeTime); // 거래시간
                                        this.VABankCode = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(VABankCode); // 은행코드
                                        this.VAVirAcctNo = Encoding.Default.GetString(read_bytes, idx, 15); idx += 15; TmpReceiveMsg.Append(VAVirAcctNo); // 가상계좌번호
                                        this.VAName = Encoding.Default.GetString(read_bytes, idx, 30); idx += 30; TmpReceiveMsg.Append(VAName); // 예금주명						  
                                        this.VACloseDate = Encoding.Default.GetString(read_bytes, idx, 8); idx += 8; TmpReceiveMsg.Append(VACloseDate); // 은행확인일					  
                                        this.VACloseTime = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(VACloseTime); // 은행확인시간					  
                                        this.VARespCode = Encoding.Default.GetString(read_bytes, idx, 4); idx += 4; TmpReceiveMsg.Append(VARespCode); // 응답코드	 
                                        this.VAMessage1 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(VAMessage1); // 메시지1
                                        this.VAMessage2 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(VAMessage2); // 메시지2
                                        this.VAFiller = Encoding.Default.GetString(read_bytes, idx, 36); idx += 36; TmpReceiveMsg.Append(VAFiller); // 예비
                                    }
                                    else
                                        if (this.ApprovalType.StartsWith("7"))
                                        {
                                            this.WPTransactionNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(WPTransactionNo); // 거래번호
                                            this.WPStatus = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(WPStatus); // 상태
                                            this.WPTradeDate = Encoding.Default.GetString(read_bytes, idx, 8); idx += 8; TmpReceiveMsg.Append(WPTradeDate); // 거래일자
                                            this.WPTradeTime = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(WPTradeTime); // 거래시간
                                            this.WPIssCode = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(WPIssCode); // 발급사코드
                                            this.WPAuthNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(WPAuthNo); // 승인번호
                                            this.WPBalanceAmount = Encoding.Default.GetString(read_bytes, idx, 9); idx += 9; TmpReceiveMsg.Append(WPBalanceAmount); // 잔액
                                            this.WPLimitAmount = Encoding.Default.GetString(read_bytes, idx, 9); idx += 9; TmpReceiveMsg.Append(WPLimitAmount); // 한도액
                                            this.WPMessage1 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(WPMessage1); // 메시지1
                                            this.WPMessage2 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(WPMessage2); // 메시지2
                                            this.WPCardNo = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(WPCardNo); // 카드번호
                                            this.WPAmount = Encoding.Default.GetString(read_bytes, idx, 9); idx += 9; TmpReceiveMsg.Append(WPAmount); // 금액
                                            this.WPMerchantNo = Encoding.Default.GetString(read_bytes, idx, 15); idx += 15; TmpReceiveMsg.Append(WPMerchantNo); // 가맹점번호
                                            this.WPFiller = Encoding.Default.GetString(read_bytes, idx, 11); idx += 11; TmpReceiveMsg.Append(WPFiller); // 예비
                                        }
                                        else
                                            if (this.ApprovalType.StartsWith("H"))
                                            {
                                                this.HTransactionNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(HTransactionNo); // 거래번호
                                                this.HStatus = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(HStatus); // 오류구분 O:정상 X:거절
                                                this.HCashTransactionNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(HCashTransactionNo); // 현금영수증 거래번호
                                                this.HIncomeType = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(HIncomeType); // 0: 소득	   1: 비소득
                                                this.HTradeDate = Encoding.Default.GetString(read_bytes, idx, 8); idx += 8; TmpReceiveMsg.Append(HTradeDate); // 거래 개시	일자
                                                this.HTradeTime = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(HTradeTime); // 거래 개시	시간
                                                this.HMessage1 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(HMessage1); // 응답 message1
                                                this.HMessage2 = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(HMessage2); // 응답 message2
                                                this.HCashMessage1 = Encoding.Default.GetString(read_bytes, idx, 20); idx += 20; TmpReceiveMsg.Append(HCashMessage1); // 국세청 메시지	1
                                                this.HCashMessage2 = Encoding.Default.GetString(read_bytes, idx, 20); idx += 20; TmpReceiveMsg.Append(HCashMessage2); // 국세청 메시지	2
                                                this.HFiller = Encoding.Default.GetString(read_bytes, idx, 150); idx += 150; TmpReceiveMsg.Append(HFiller); // 예비
                                            }
                                            else
                                                if (this.ApprovalType.StartsWith("S"))
                                                {
                                                    this.STTransactionNo = Encoding.Default.GetString(read_bytes, idx, 12); idx += 12; TmpReceiveMsg.Append(STTransactionNo); // 거래번호				
                                                    this.STStatus = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(STStatus); // 오류구분 - O:성공  X:실패 S : 확인필요
                                                    this.STTradeDate = Encoding.Default.GetString(read_bytes, idx, 8); idx += 8; TmpReceiveMsg.Append(STTradeDate); // 거래일자				
                                                    this.STTradeTime = Encoding.Default.GetString(read_bytes, idx, 6); idx += 6; TmpReceiveMsg.Append(STTradeTime); // 거래시간				
                                                    this.STGoveSele = Encoding.Default.GetString(read_bytes, idx, 10); idx += 10; TmpReceiveMsg.Append(STGoveSele); // 기관구분				
                                                    this.STPinType = Encoding.Default.GetString(read_bytes, idx, 1); idx += 1; TmpReceiveMsg.Append(STPinType); // 문화 M / 게임문화 G			
                                                    this.STAuthNo = Encoding.Default.GetString(read_bytes, idx, 20); idx += 20; TmpReceiveMsg.Append(STAuthNo); // 승인번호 - 오류시 오류코드		
                                                    this.STRespMsg = Encoding.Default.GetString(read_bytes, idx, 50); idx += 50; TmpReceiveMsg.Append(STRespMsg); // 메시지				
                                                    this.STAmount = Encoding.Default.GetString(read_bytes, idx, 9); idx += 9; TmpReceiveMsg.Append(STAmount); // 결제금액				
                                                    this.STPinNumb = Encoding.Default.GetString(read_bytes, idx, 18); idx += 18; TmpReceiveMsg.Append(STPinNumb); // PIN 번호				
                                                    this.STCertNo = Encoding.Default.GetString(read_bytes, idx, 16); idx += 16; TmpReceiveMsg.Append(STCertNo); // 관리번호				
                                                    this.STExpDate = Encoding.Default.GetString(read_bytes, idx, 4); idx += 4; TmpReceiveMsg.Append(STExpDate); // 유효기간				
                                                    this.STFiller = Encoding.Default.GetString(read_bytes, idx, 87); idx += 87; TmpReceiveMsg.Append(STFiller); // 예비									
                                                }
                                                else
                                                {
                                                    KSPayApprovalCancelBean.write_log("WARN	: undefined	type_msg((" + this.ApprovalType + ")=[" + Encoding.Default.GetString(read_bytes, 0, len) + "]!!");
                                                    return false;
                                                }
            this.ReceiveDataMsg = TmpReceiveMsg.ToString();

            return true;
        }








    }
}
