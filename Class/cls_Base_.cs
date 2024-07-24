using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace MLM_Program
{
    class cls_Date_G
    {
        public void Date_Between(string begin_date_time, string end_date_time, string Date_Com_Tf , ref int Between_g)
        {

            int date_gap = 0; //int T_Sw = 0;
            string input = string.Format("{0:####-##-##}", int.Parse(begin_date_time.Replace("-", "").Trim()));
            DateTime t1 = DateTime.Parse(input);


            input = string.Format("{0:####-##-##}", int.Parse(end_date_time.Replace("-", "").Trim()));
            DateTime t2 = DateTime.Parse(input);

            DateTime t3 = t1;


            if (Date_Com_Tf == "Y")
            {
                date_gap =1 ;
                while (t3.AddYears(1).Ticks <= t2.Ticks)
                {
                    date_gap++;
                    t3 = t3.AddYears(1);
                    //T_Sw = 1;
                }                
            }


            if (Date_Com_Tf =="M")
            {
                date_gap = 1;
                while (t3.AddMonths(1).Ticks <= t2.Ticks)
                {
                    date_gap++;
                    t3 = t3.AddMonths(1);
                    //T_Sw = 1;
                }
                
            }


            if (Date_Com_Tf == "D")
            {
                TimeSpan ts = t2 - t1;
                date_gap = int.Parse(ts.Days.ToString());
                date_gap++;
            }



            Between_g = date_gap;
        }


        public int Check_Date_HolyDay_TF(DateTime dt)
        {

            string BaseDate = dt.ToString().Substring(5, 5);

            //검색일이 공휴일이다
            if (BaseDate == "01-01" || BaseDate == "03-01" || BaseDate == "10-09" || BaseDate == "05-05" || BaseDate == "06-06" || BaseDate == "08-15"
              || BaseDate == "10-03" || BaseDate == "12-25")
                return -1;

            string Y_Date = Y_Md_Change(dt);
            string[] T_Y_Date = Y_Date.Split('-');

            string Y_Date2 = "";
            if (T_Y_Date[1].Length == 1)
                Y_Date2 = "0" + T_Y_Date[1].ToString();
            else
                Y_Date2 = T_Y_Date[1].ToString();


            if (T_Y_Date[2].Length == 1)
                Y_Date2 = Y_Date2 + "0" + T_Y_Date[2].ToString();
            else
                Y_Date2 = Y_Date2 + T_Y_Date[2].ToString();


            if (Y_Date2 == "1231") return -1;

            if (Y_Date2 == "0101") return -1;

            if (Y_Date2 == "0102") return -1;

            if (Y_Date2 == "0408") return -1;

            if (Y_Date2 == "0814") return -1;

            if (Y_Date2 == "0815") return -1;

            if (Y_Date2 == "0816") return -1;


            //검색일이 토요일이나.. 일요일이다.
            if (dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday)
            {
                return -1;
            }

            return 0;
        }




        private string Y_Md_Change(DateTime dt)
        {
            int nY_MM;
            int nY_Md_YY, nY_Md_mm, nY_Md_dd;
            bool Yun_Dal_TF = false;

            System.Globalization.KoreanLunisolarCalendar Y_Md = new System.Globalization.KoreanLunisolarCalendar();

            nY_Md_YY = Y_Md.GetYear(dt);
            nY_Md_mm = Y_Md.GetMonth(dt);
            nY_Md_dd = Y_Md.GetDayOfMonth(dt);

            if (Y_Md.GetMonthsInYear(nY_Md_YY) > 12)             //1년이 12이상이면 윤달이 있음..
            {

                Yun_Dal_TF = Y_Md.IsLeapMonth(nY_Md_YY, nY_Md_mm);     //윤월인지
                nY_MM = Y_Md.GetLeapMonth(nY_Md_YY);             //년도의 윤달이 몇월인지?
                if (nY_Md_mm >= nY_MM)                           //달이 윤월보다 같거나 크면 -1을 함 즉 윤8은->9 이기때문
                    nY_Md_mm--;
            }

            return nY_Md_YY.ToString() + "-" + (Yun_Dal_TF ? "*" : "") + nY_Md_mm.ToString() + "-" + nY_Md_dd.ToString();
        }



        public double DateDiff(string Interval, DateTime Date1, DateTime Date2)
        {

            double diff = 0;
            TimeSpan ts = Date2 - Date1;

            switch (Interval.ToLower())
            {

                case "y":

                    ts = DateTime.Parse(Date2.ToString("yyyy-01-01")) - DateTime.Parse(Date1.ToString("yyyy-01-01"));

                    diff = Convert.ToDouble(ts.TotalDays / 365);

                    break;

                case "m":

                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-01")) - DateTime.Parse(Date1.ToString("yyyy-MM-01"));

                    diff = Convert.ToDouble((ts.TotalDays / 365) * 12);

                    break;

                case "d":

                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-dd")) - DateTime.Parse(Date1.ToString("yyyy-MM-dd"));

                    diff = ts.Days;

                    break;

                case "h":

                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-dd HH:00:00")) - DateTime.Parse(Date1.ToString("yyyy-MM-dd HH:00:00"));

                    diff = ts.TotalHours;

                    break;

                case "n":

                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-dd HH:mm:00")) - DateTime.Parse(Date1.ToString("yyyy-MM-dd HH:mm:00"));

                    diff = ts.TotalMinutes;

                    break;

                case "s":

                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-dd HH:mm:ss")) - DateTime.Parse(Date1.ToString("yyyy-MM-dd HH:mm:ss"));

                    diff = ts.TotalSeconds;

                    break;

                case "ms":

                    diff = ts.TotalMilliseconds;

                    break;

            }



            return diff;


        }


    } //end cls_Date_G




    class cls_Sn_Check
    {
        public bool Number_NotInput_Check(string sn_Number, string sort_TF)
        {
            if (sort_TF == "biz")
            {
                if (Number_in_Check(sn_Number, 3) == true)
                {
                    string Sn = sn_Number.Replace("-", "").Replace("_", "").Trim();
                    return IsBizId(Sn);
                }
                else
                    return false; 
            }

            if (sort_TF == "Tel" || sort_TF == "HpTel")
                return Number_in_Check(sn_Number ,3);

            if (sort_TF == "Zip")
            {
                if (sn_Number.Length == 5 || sn_Number.Length == 6)
                    return true;
                else
                    return false;
                //return Number_in_Check(sn_Number, 2);
            }
            if (sort_TF == "Date")
            {
                if (Number_in_Check(sn_Number, 3) == true || sn_Number.Replace("-", "").Replace("_", "").Trim().Length == 8)
                {
                    DateTime dateTime;
                    string input = "";                   
                   
                    input = string.Format("{0:####-##-##}", int.Parse (sn_Number.Replace("-", "")));
                    
                    if (DateTime.TryParse(input, out dateTime) == false)
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            if(sort_TF == "Email")
            {
                if (sn_Number.Equals(string.Empty))
                    return false;
                else if (sn_Number.Contains("@") == false)
                    return false;
                else if (sn_Number.Contains(".") == false)
                    return false;
            }

            return true;
        }


        private bool Number_in_Check(string sn_Number , int SaRiSu )
        {

            Debug.WriteLine(sn_Number);

            string [] sn = sn_Number.Split('-');
            if (sn.Length >= SaRiSu)
            {
                for (int i = 0; i <= sn.Length - 1; i++)
                {
                    Debug.WriteLine($"for i={i}-{sn[i]}");

                    if (num_check(sn[i]) == "")
                    {
                        return false;
                    }
                }
            }            
            else
                return false;
            

            return true;
        }



        public bool Sn_Number_Check(string sn_Number, string sort_TF)
        {
            if (cls_app_static_var.Member_Cpno_Error_Check_TF == 1) //주민번호 오류 체크를 필히 하는 경우에는                
            {
                if (sort_TF == "in")                
                    return IsJumin_Number(sn_Number);                

                if (sort_TF == "fo")
                    return f_IsJumin_Number(sn_Number);
                
                if (sort_TF == "email")
                    return IsBizId(sn_Number);

                if (sort_TF == "biz")
                    return IsBizId(sn_Number);
            }


            

            return true;
        }

        // 주민등록번호 체크
        public bool IsJumin_Number(string JuminParameter)
        {
            JuminParameter = num_check(JuminParameter);

            if (JuminParameter.Length != 13)
                return false;

            string[] ArrayJumin = new string[13];
            int[] arr_rule = { 2, 3, 4, 5, 6, 7, 8, 9, 2, 3, 4, 5 };
            for (int i = 0; i < 13; i++)
                ArrayJumin[i] = JuminParameter.Substring(i, 1);

            int total = 0;

            for (int k = 0; k < 12; k++)
                total += int.Parse(ArrayJumin[k]) * arr_rule[k];

            int modvalue = 11 - (total % 11);


            if ((int.Parse(ArrayJumin[12]) == (modvalue % 10)))
                return true;
            else
                return false;
        }



        public bool f_IsJumin_Number(string s_rrn) // 외국인등록번호유효성검사.
        {
            int sum = 0;
            if (s_rrn.Length != 13)
                return false;

            if (int.Parse(s_rrn.Substring(7, 2)) % 2 != 0)
            {
                return false;
            }
            for (int i = 0; i < 12; i++)
            {
                sum += int.Parse(s_rrn.Substring(i, 1)) * ((i % 8) + 2);
            }
            if ((((11 - (sum % 11)) % 10 + 2) % 10) == int.Parse(s_rrn.Substring(12, 1)))
            {
                return true;
            }
            return false;
        }







        // E-Mail 체크
        public bool IsEmail(string email)
        {
            Regex emailregex = new Regex("(?<user>[^@]+)@(?<host>.+)");
            Boolean ismatch = emailregex.IsMatch(email);
            if (ismatch)
                return true;
            else
                return false;
        }


        // 사업자 번호 체크
        public bool IsBizId(string biz_no)
        {
            biz_no = num_check(biz_no);
            if (biz_no.Length != 10)
                return false;

            int[] weight = { 1, 3, 7, 1, 3, 7, 1, 3, 5 };
            int result = 0;
            string[] biz_id = new string[10];
            for (int i = 0; i < 10; i++)
                biz_id[i] = biz_no.Substring(i, 1);
            int total = 0;
            for (int i = 0; i < 9; i++)
                total += int.Parse(biz_id[i]) * weight[i];
            total += (int.Parse(biz_id[8]) * 5) / 10;
            int check = total % 10;
            if (check == 0) result = 0;
            else result = 10 - check;
            if (result != int.Parse(biz_id[9]))
                return false;
            else
                return true;
        }
        //법인번호 체크
        public bool LegalId(string number)
        {
            number = num_check(number);
            if (number.Length != 13)
                return false;
            int[] arr_rule = { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int total = 0;

            for (int i = 0; i < 12; i++)
                total += int.Parse(number.Substring(i, 1)) * arr_rule[i];
            if ((10 - (total % 10)) == int.Parse(number.Substring(12, 1)))
                return true;
            else return false;

        }


        public string num_check(string num)
        {
            string retValue = "";

            Regex regex = new System.Text.RegularExpressions.Regex(@"^[0-9]{1,10}$");

            if ((num.Length > 1) && (num.Substring(0, 1) == "-"))
                retValue = "-";

            for (int i = 0; i < num.Length; i++)
            {
                Boolean ismatch = regex.IsMatch(num[i].ToString());
                if (!ismatch)
                    continue;
                else
                    retValue += num[i];
            }
            return retValue;
        }


        public bool check_19_nai(string JuminParameter)
        {

            //태국은 민증으로 파악할 수 있는 방법이없음
            if (cls_User.gid_CountryCode == "TH")
                return true;

            // 주민번호 추출 예:1234111
            string jumin = JuminParameter.Substring(6, 7);

            // 생년월일 출력 : 281123
            string BirthDay = JuminParameter.Substring(0, 6);

            // 오늘날짜 : 2007-10-10
            string NowDay = DateTime.Now.ToString("yyyy-MM-dd");


            // 생년월일에서 태어난 연도 추출
            int birth = Convert.ToInt32(BirthDay.Substring(0, 2));
            int year = DateTime.Today.Year;
            string firstJumin = jumin.Substring(0, 1);


            DateTime birth22 = new DateTime();
            DateTime NowDay2 = new DateTime();

            string PayDate = "";

            NowDay2 = DateTime.Parse(NowDay);

            if (firstJumin == "1" || firstJumin == "2" || firstJumin == "5" || firstJumin == "6")
            {
                PayDate = "19" + JuminParameter.Substring(0, 2) + '-' + JuminParameter.Substring(2, 2) + '-' + JuminParameter.Substring(4, 2);
                birth22 = DateTime.Parse(PayDate);
            }
            else if (firstJumin == "3" || firstJumin == "4")
            {
                PayDate = "20" + JuminParameter.Substring(0, 2) + '-' + JuminParameter.Substring(2, 2) + '-' + JuminParameter.Substring(4, 2);
                birth22 = DateTime.Parse(PayDate);
            }

            TimeSpan res = NowDay2 - birth22; //curr은 현재연월일, birth는 생년월일의 DateTime 타입의 값

            DateTime dateTimeAge = new DateTime(res.Ticks);

            string stringArge = dateTimeAge.ToString("yy");

            int nai = int.Parse(stringArge);

            if (nai < 19)
                return false;
            else
                return true;
        }





        public bool check_19_nai(string JuminParameter , ref string  BirthDay2  )
        {

            //태국은 민증으로 파악할 수 있는 방법이없음
            if (cls_User.gid_CountryCode == "TH")
                return true;


            // 주민번호 추출 예:1234111
            string jumin = JuminParameter.Substring(6, 7);

            // 생년월일 출력 : 281123
            string BirthDay = JuminParameter.Substring(0, 6);

            // 오늘날짜 : 2007-10-10
            string NowDay = DateTime.Now.ToString("yyyy-MM-dd");


            // 생년월일에서 태어난 연도 추출
            int birth = Convert.ToInt32(BirthDay.Substring(0, 2));
            int year = DateTime.Today.Year;
            string firstJumin = jumin.Substring(0, 1);


            DateTime birth22 = new DateTime();
            DateTime NowDay2 = new DateTime();

            string PayDate = "";

            NowDay2 = DateTime.Parse(NowDay);

            if (firstJumin == "1" || firstJumin == "2" || firstJumin == "5" || firstJumin == "6")
            {
                PayDate = "19" + JuminParameter.Substring(0, 2) + '-' + JuminParameter.Substring(2, 2) + '-' + JuminParameter.Substring(4, 2);
                birth22 = DateTime.Parse(PayDate);
            }
            else if (firstJumin == "3" || firstJumin == "4")
            {
                PayDate = "20" + JuminParameter.Substring(0, 2) + '-' + JuminParameter.Substring(2, 2) + '-' + JuminParameter.Substring(4, 2);
                birth22 = DateTime.Parse(PayDate);
            }

            BirthDay2 = PayDate;

            TimeSpan res = NowDay2 - birth22; //curr은 현재연월일, birth는 생년월일의 DateTime 타입의 값

            DateTime dateTimeAge = new DateTime(res.Ticks);

            string stringArge = dateTimeAge.ToString("yy");

            int nai = int.Parse(stringArge);

            if (nai < 19)
                return false;
            else
                return true;
        }


        private int iAge(int birth, int year, string firstJumin)
        {
            int nai = 0;

            // 우리나이로 나이계산
            if (firstJumin == "1" || firstJumin == "2")
            {
                // 주민번호가 첫째자리 1 또는 2이면 2000년 이전 출생자
                nai = (year - (1900 + birth)) + 1;
            }
            else if (firstJumin == "3" || firstJumin == "4")
            {
                // 주민번호 첫째자리 3이나 4이면 2000년도 이후 출생자
                nai = (year - (2000 + birth) + 1);
            }

            return nai;
        }



        public int Search_nai_Period(string JuminParameter)
        {
            try
            {

                //태국은 민증으로 파악할 수 있는 방법이없음
                if (cls_User.gid_CountryCode == "TH")
                    return 0;

                // 주민번호 추출 예:1234111
                string jumin = JuminParameter.Substring(6, 1);

                // 생년월일 출력 : 281123
                string BirthDay = JuminParameter.Substring(0, 6);

                // 오늘날짜 : 2007-10-10
                string NowDay = DateTime.Now.ToString("yyyy-MM-dd");


                // 생년월일에서 태어난 연도 추출
                int birth = Convert.ToInt32(BirthDay.Substring(0, 2));
                int year = DateTime.Today.Year;
                string firstJumin = jumin.Substring(0, 1);


                DateTime birth22 = new DateTime();
                DateTime NowDay2 = new DateTime();

                string PayDate = "";

                NowDay2 = DateTime.Parse(NowDay);

                if (firstJumin == "1" || firstJumin == "2" || firstJumin == "5" || firstJumin == "6")
                {
                    PayDate = "19" + JuminParameter.Substring(0, 2) + '-' + JuminParameter.Substring(2, 2) + '-' + JuminParameter.Substring(4, 2);
                    birth22 = DateTime.Parse(PayDate);
                }
                else if (firstJumin == "3" || firstJumin == "4")
                {
                    PayDate = "20" + JuminParameter.Substring(0, 2) + '-' + JuminParameter.Substring(2, 2) + '-' + JuminParameter.Substring(4, 2);
                    birth22 = DateTime.Parse(PayDate);
                }



                TimeSpan res = NowDay2 - birth22; //curr은 현재연월일, birth는 생년월일의 DateTime 타입의 값

                DateTime dateTimeAge = new DateTime(res.Ticks);

                string stringArge = dateTimeAge.ToString("yy");

                int nai = int.Parse(stringArge);

                int nai_2 = int.Parse(stringArge) / 10;

                nai = 10 * nai_2;  // - (nai % 10);

                return nai;
            }
            catch
            {
                return 0; 
            }

        }



        public int Search_nai_Period_B(string PayDate)
        {
            try
            {
                
                string NowDay = DateTime.Now.ToString("yyyy-MM-dd");

                DateTime NowDay2 = new DateTime();
                               
                NowDay2 = DateTime.Parse(NowDay);

                DateTime birth22 = new DateTime();
                birth22 = DateTime.Parse(PayDate);

                TimeSpan res = NowDay2 - birth22; //curr은 현재연월일, birth는 생년월일의 DateTime 타입의 값

                DateTime dateTimeAge = new DateTime(res.Ticks);

                string stringArge = dateTimeAge.ToString("yy");

                int nai = int.Parse(stringArge);

                int nai_2 = int.Parse(stringArge) / 5;

                nai = 5 * nai_2;  // - (nai % 10);

                return nai;
            }
            catch
            {
                return 0;
            }

        }


        public void Bank_Acount_Check(string BankOwner, string Cpno1, string BankCode, string bankaccnt, ref string s_Line1, ref string s_Line2, ref string s_Line3)
        {
            string str_sendvalue = "";
            str_sendvalue = "m_name=" + BankOwner;
            str_sendvalue = str_sendvalue + "&cpno1=" + Cpno1;
            str_sendvalue = str_sendvalue + "&bankcode=" + BankCode;
            str_sendvalue = str_sendvalue + "&bankaccnt=" + bankaccnt;


            string URL = "http://www.apyld.com/apyldCertifyAccount.do";


            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "Apyld";
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

            s_Line1 = readerPost.ReadLine().ToString();
            s_Line2 = readerPost.ReadLine().ToString();
            s_Line3 = readerPost.ReadLine().ToString();

        }


        public string Bank_Acount_Check(string BankOwner, string Cpno1, string BankCode, string bankaccnt)
        {
            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

            string str_sendvalue = "";

            str_sendvalue = "service=1";
            str_sendvalue = str_sendvalue + "&svcGbn=5";
            str_sendvalue = str_sendvalue + "&juminNo=" + Cpno1;
            str_sendvalue = str_sendvalue + "&userNm=" + BankOwner;
            str_sendvalue = str_sendvalue + "&strBankCode=" + BankCode;
            str_sendvalue = str_sendvalue + "&strAccountNo=" + bankaccnt;
            str_sendvalue = str_sendvalue + "&inqRsn=10";


            string URL = cls_app_static_var.AccountCertifyURL;


            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "PMI";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.


            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "N";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();
            getstring = getstring.Replace("\n", "").Replace("\r", "");

            JObject ReturnData = new JObject();
            string SuccessYN = "";

            try
            {
                ReturnData = JObject.Parse(getstring);
                SuccessYN = ReturnData["successYN"].ToString();
            }
            catch
            {
                return "N";
            }

            return SuccessYN;
        }


        public void Nice_Name_Birth_Sender_Check(string M_Name, string Birth, int Sender, ref string s_Line1, ref string s_Line2, ref string s_Line3)
        {
            string str_sendvalue = "";
            str_sendvalue = "jumin=" + Birth.Substring(2, 6);
            str_sendvalue = str_sendvalue + "&name=" + M_Name;

            string URL = "https://www.anewhealkorea.co.kr/common/cs/sNameCheck/approval.do";


            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "ANEW";
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

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            JObject ReturnData = new JObject();
            string SuccessYN = "";

            try
            {
                ReturnData = JObject.Parse(getstring);
                SuccessYN = ReturnData["successYN"].ToString();

            }
            catch
            {
                s_Line1 = "N";
            }




            s_Line1 = SuccessYN;

        }


    }// end cls_Sn_Check


















}
