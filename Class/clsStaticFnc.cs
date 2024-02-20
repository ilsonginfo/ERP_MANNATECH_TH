using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MLM_Program
{
    /// <summary>
    /// 공통 함수 스태틱 영역 아프로 여따가 만든다.. 기존꺼 이전할꺼 이전
    /// </summary>

    class clsStaticFnc
    {



        public static string Post_Api(string sUrl, string sSend, string oMethod)
        {
            try
            {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                HttpWebRequest wReqFirst = (HttpWebRequest)WebRequest.Create(sUrl);
                wReqFirst.Method = oMethod;
                wReqFirst.ContentType = "application/json";

                if (sSend.Length > 0)
                {
                    using (var streamWriter = new StreamWriter(wReqFirst.GetRequestStream()))
                    {
                        streamWriter.Write(sSend);
                        streamWriter.Flush();
                    }
                }

                HttpWebResponse wRespFirst = (HttpWebResponse)wReqFirst.GetResponse();
                Stream respPostStream = wRespFirst.GetResponseStream();
                StreamReader readerPost = new StreamReader(respPostStream, Encoding.Default);

                // 생성한 스트림으로부터 string으로 변환합니다.
                return readerPost.ReadToEnd();
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    var httpResponse = (HttpWebResponse)response;

                    using (Stream data = response.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(data);
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                ArgumentException argEx = new ArgumentException(ex.Message.ToString());
                throw argEx;
            }
        }


        public static Double Let_Double(string sStr)
        {
            double dReturn = 0;

            if (sStr == null)
                return dReturn;

            if (sStr.Length == 0)
            {
                return dReturn;
            }


            if (sStr == string.Empty)
            {
                return dReturn;
            }

            double dTemp = 0;

            if (Double.TryParse(sStr, out dTemp))
            {
                dReturn = dTemp;
            }
            else
            {
                return dReturn;
            }
            return dReturn;
        }

        public static int Let_Int(string sStr)
        {
            int iReturn = 0;

            if (sStr == null)
                return iReturn;

            if (sStr.Length == 0)
            {
                return iReturn;
            }

            if (sStr == string.Empty)
            {
                return iReturn;
            }
            int iTemp = 0;

            if (Int32.TryParse(sStr, out iTemp))
            {
                iReturn = iTemp;
            }
            else
            {
                return iReturn;
            }
            return iReturn;
        }

    }
}
