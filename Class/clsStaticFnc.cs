using System;
using System.Collections.Generic;
using System.Data;
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




        public static string toString(DataRow dr, string sColumn, string sDefaultValue)
        {
            string sResult = sDefaultValue;

            if (dr.Table.Columns.Contains(sColumn))
            {
                object value = dr[sColumn];
                if ((value is DBNull) == false && value != null)
                {
                    sResult = value.ToString();
                }
            }
            sResult = sResult.Trim();
            return sResult;
        }

        public static string toString(DataRow row, string sColumnName)
        {
            return toString(row, sColumnName, string.Empty);
        }

        public static int toInteger(DataRow row, string sColumnName, int nDefaultValue)
        {
            int nResult = nDefaultValue;
            if (row.Table.Columns.Contains(sColumnName))
            {
                object value = row[sColumnName];
                if ((value is DBNull) == false && value != null)
                {
                    string sValue = value.ToString();
                    if (int.TryParse(sValue, out nResult) == false)
                        nResult = nDefaultValue;
                }
            }
            return nResult;
        }
        public static int toInteger(DataRow row, string sColumnName)
        {
            return toInteger(row, sColumnName, 0);
        }

        public static double toDouble(DataRow row, string sColumnName, double dDefaultValue)
        {
            double dResult = dDefaultValue;
            if (row.Table.Columns.Contains(sColumnName))
            {
                object value = row[sColumnName];
                if ((value is DBNull) == false && value != null)
                {
                    string sValue = value.ToString();
                    if (double.TryParse(sValue, out dResult) == false)
                        dResult = dDefaultValue;
                }
            }
            return dResult;
        }
        public static double toDouble(DataRow row, string sColumnName)
        {
            return toDouble(row, sColumnName, 0);
        }

        public static DateTime toDateTime(DataRow row, string sColumnName, DateTime defaultTime)
        {
            DateTime result = defaultTime;

            if (row.Table.Columns.Contains(sColumnName))
            {
                object value = row[sColumnName];
                if ((value is DBNull) == false && value != null)
                {
                    try
                    {
                        result = (DateTime)value;
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
            }
            return result;
        }
        public static DateTime toDateTime(DataRow row, string sColumnName)
        {
            return toDateTime(row, sColumnName, DateTime.MinValue);
        }

    }
}
