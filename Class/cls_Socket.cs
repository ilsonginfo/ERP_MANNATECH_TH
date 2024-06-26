﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Xml;
using System.Web.Services.Protocols;
using System.ServiceModel;
using Newtonsoft.Json.Linq;

namespace MLM_Program
{
    class cls_Socket
    {
        private static byte[] getbyte = new byte[270];
        private static byte[] setbyte = new byte[5400];
        private string DT_Time = "";

        const char STX = (char)0x02;
        const char FS = (char)0x1c;
        const char ETX = (char)0x03;

        /*조합신고, AccMode는 가상계좌발행 하게되면 바로 직판쪽에 공제가가도록 처리하기위함 */
        public string Dir_Connect_Send(string OrderNumber, bool NonGaorderMode = false)
        {
            int Ord_SW = 0;
            string str_sendvalue = "", DT_Time = "";
            Search_Sell_Date(OrderNumber, ref Ord_SW, ref str_sendvalue, ref DT_Time, NonGaorderMode);

            //           if (Environment.MachineName.Equals("LANCE1"))
            {
                cls_app_static_var.ApproveAssociationURL = "https://www.mygps.kr/common/cs/insertAssociation.do";
                //cls_app_static_var.ApproveAssociationURL = "http://192.168.0.101/common/cs/insertAssociation.do";
                //cls_app_static_var.ApproveAssociationURL = "http://local.gps.com/common/cs/insertAssociation.do";
                //cls_app_static_var.ApproveAssociationURL = "http://local.gps.com/common/cs/insertAssociation.do";    

                //cls_app_static_var.ApproveAssociationURL = "https://192.168.0.101/";
            }
            if (Ord_SW == 0)
                return "-10000";

            string URL = cls_app_static_var.ApproveAssociationURL;

            if (URL == null || URL == string.Empty)
                return "-10000";

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "GPS";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendSt   ream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ex)
            {

                if (cls_User.gid == cls_User.SuperUserID)
                    MessageBox.Show(ex.Message);

                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();


            string Err_Code = "";
            Err_Code = Back_Date_Input(getstring, OrderNumber, DT_Time);

            return Err_Code;
        }

        public string Dir_Connect_Send_Acc(string OrderNumber)
        {
            int Ord_SW = 0;
            string str_sendvalue = "";
            Search_Sell_DAte(OrderNumber, ref Ord_SW, ref str_sendvalue);

            if (Ord_SW == 0)
                return "-10000";

            //string URL = "http://bioplanet.ilsonginfo.co.kr/common/cs/associationinsert.do"; 
            string URL = "https://www.bioplanet.co.kr:484/common/cs/associationinsert.do";

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "BioP";
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
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadLine().ToString().Replace("\"", "").Replace("{", "").Replace("}", "");
            getstring = getstring.Replace("orderID:", "").Replace("guaranteeResult:", "").Replace("memID:", "").Replace("ErrorCode:", "").Replace("mallID:", "").Replace("GuaranteeCode:", "");
            // orderID:null,guaranteeResult:N,memID:null,ErrorCode:1000,mallID:null

            //Enviroment.NewLine
            string Err_Code = "";
            Err_Code = Back_Date_Input(getstring, OrderNumber);

            return Err_Code;
        }


        public string Dir_Connect_Send_Cancel(string OrderNumber)
        {
            int Ord_SW = 0;
            string str_sendvalue = "";
            Search_Sell_DAte_Cancel(OrderNumber, ref Ord_SW, ref  str_sendvalue);

            if (Ord_SW == 0)
                return "-10000";

            string URL = "https://www.bioplanet.co.kr:484/common/cs/associationdelete.do";


            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "BioP";
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
                return "-1";
            }
            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);





            //IPAddress serverIP = IPAddress.Parse(cls_app_static_var.Dir_Socket_Ip);
            //IPEndPoint serverEndPoint = new IPEndPoint(serverIP, cls_app_static_var.Dir_Socket_Cancel_Port);
            //Socket EY_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //EY_Client.Connect(serverEndPoint);

            //if (!EY_Client.Connected)
            //{
            //    return "-1";
            //}

            //EY_Client.Send(setbyte, 0, setbyte.Length, SocketFlags.None);

            //int getValueLength = 0;
            string getstring = null;
            getstring = readerPost.ReadLine().ToString().Replace("\"", "").Replace("{", "").Replace("}", "");
            getstring = getstring.Replace("orderID:", "").Replace("cancelResult:", "").Replace("mallID:", "").Replace("errorCode:", "").Replace("guaranteeCode:", "");

            //orderID:2017011800100002,errorCode:3000,cancelResult:N,mallID:5500,guaranteeCode:77756716"

            //s_Line2 = readerPost.ReadLine().ToString();
            //s_Line3 = readerPost.ReadLine().ToString();

            //EY_Client.Receive(getbyte, 0, getbyte.Length, SocketFlags.None);
            //getValueLength = byteArrayDefrag(getbyte);
            //getstring = Encoding.UTF7.GetString(getbyte, 0, getValueLength + 1);

            string Err_Code = "";
            Err_Code = Back_Cancel_Date_Input(getstring, OrderNumber);

            return Err_Code;
        }



        public string Dir_Connect_Send_Cancel__2(string OrderNumber, double Cancel_PR)
        {
            int Ord_SW = 0;
            string str_sendvalue = "";
            Search_Sell_DAte_Cancel__2(OrderNumber, Cancel_PR, ref Ord_SW, ref  str_sendvalue);

            if (Ord_SW == 0)
                return "-10000";

            //string URL = "http://bioplanet.ilsonginfo.co.kr/common/cs/associationdelete.do";
            string URL = "https://www.bioplanet.co.kr:484/common/cs/associationdelete.do";


            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "BioP";
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
                return "-1";
            }
            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);




            string getstring = null;
            getstring = readerPost.ReadLine().ToString().Replace("\"", "").Replace("{", "").Replace("}", "");
            getstring = getstring.Replace("orderID:", "").Replace("cancelResult:", "").Replace("mallID:", "").Replace("errorCode:", "").Replace("guaranteeCode:", "");


            string Err_Code = "";
            Err_Code = Back_Cancel_Date_Input__2(getstring, OrderNumber);

            return Err_Code;
        }



        private string Back_Date_Input(string Getstring, string OrderNumber)
        {
            string InsuranceNumber = "", Err_Code = "";

            string[] BackDate = Getstring.Split(',');

            string Back_Flag = BackDate[1].ToString().Replace(" ", "");

            if (Back_Flag == "Y" || Back_Flag == "D")
            {
                string ins_A = BackDate[3].ToString().Replace(" ", "");
                InsuranceNumber = ins_A;
                Err_Code = Back_Flag;
            }
            else
            {
                InsuranceNumber = "";
                Err_Code = BackDate[3].ToString().Replace(" ", "");
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Dir_Back_Log_Table_Make(Temp_Connect, OrderNumber, InsuranceNumber, ref Err_Code, Back_Flag);

            return Err_Code;
        }


        private string Back_Cancel_Date_Input(string Getstring, string OrderNumber)
        {
            string InsuranceNumber = "", Err_Code = "";

            string[] BackDate = Getstring.Split(','); ;

            string Back_Flag = BackDate[2].ToString().Replace(" ", "");

            if (Back_Flag == "Y" || Back_Flag == "D")
            {
                string ins_A = BackDate[4].ToString().Replace(" ", "");
                InsuranceNumber = ins_A;
                Err_Code = Back_Flag;
            }
            else
            {
                InsuranceNumber = "";
                Err_Code = BackDate[1].ToString().Replace(" ", "");
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Dir_Back_Cancel_Log_Table_Make(Temp_Connect, OrderNumber, InsuranceNumber, ref Err_Code, Back_Flag);

            return Err_Code;
        }

        private string Back_Cancel_Date_Input__2(string Getstring, string OrderNumber)
        {
            string InsuranceNumber = "", Err_Code = "";

            string[] BackDate = Getstring.Split(','); ;

            string Back_Flag = BackDate[2].ToString().Replace(" ", "");

            if (Back_Flag == "Y" || Back_Flag == "D")
            {
                string ins_A = BackDate[4].ToString().Replace(" ", "");
                InsuranceNumber = ins_A;
                Err_Code = Back_Flag;
            }
            else
            {
                InsuranceNumber = "";
                Err_Code = BackDate[1].ToString().Replace(" ", "");
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Dir_Back_Cancel_Log_Table_Make__2(Temp_Connect, OrderNumber, InsuranceNumber, ref Err_Code, Back_Flag);

            return Err_Code;
        }




        private void Search_Sell_DAte(string OrderNumber, ref int Ord_SW, ref string str_sendvalue)
        {
            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

            string SendDate = null;
            string Tsql = "";

            //Tsql = "Select  ";
            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) AS M_Mbid ";
            //else
            //    Tsql = Tsql + " tbl_Memberinfo.mbid2 AS M_Mbid ";
            //Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";
            //Tsql = Tsql + ", tbl_Memberinfo.Cpno ";

            //Tsql = Tsql + " , tbl_SalesDetail.TotalInputPrice ";
            //Tsql = Tsql + " ,Convert(Varchar(25),GetDate(),120) AS DT_Time  ";

            Tsql = "SELECT tbl_Memberinfo.Cpno, tbl_Memberinfo.Mbid,tbl_Memberinfo.Mbid2, tbl_Memberinfo.M_Name,  tbl_Memberinfo.Sex_FLAG";
            Tsql = Tsql + " ,tbl_SalesDetail.OrderNumber , tbl_SalesDetail.TotalPrice , tbl_Memberinfo.Sell_Mem_TF , Isnull(tbl_SalesDetail.InsuranceNumber,'')  INS_Num ";
            Tsql = Tsql + " ,tbl_Memberinfo.BirthDay + tbl_Memberinfo.BirthDay_M + tbl_Memberinfo.BirthDay_D BirthDay ";
            Tsql = Tsql + " ,TotalInputPrice  , Convert(varchar(40),getdate(),21) DT_Time ";
            Tsql = Tsql + " ,InputCash + InputPassbook  AS  Input_C , InputCard , InputMile ";
            Tsql = Tsql + " From tbl_SalesDetail  (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid   And  tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";
            Tsql = Tsql + " Where tbl_SalesDetail.OrderNumber = '" + OrderNumber + "'";
            //Tsql = Tsql + " And  tbl_SalesDetail.TotalinputPrice - inputPass_Pay  = TotalPrice   ";
            Tsql = Tsql + " And  Isnull(tbl_SalesDetail.InsuranceNumber,'')  = '' ";
            Tsql = Tsql + " And  tbl_Memberinfo.Na_Code = 'KR'  ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Send", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            //string Cpno = encrypter.Decrypt(ds.Tables["Send"].Rows[0]["Cpno"].ToString(), "Cpno_D");

            //SendDate = null;
            //SendDate = STX + OrderNumber + FS;
            //SendDate = SendDate + cls_app_static_var.Dir_Company_Code + FS;
            //SendDate = SendDate + double.Parse(ds.Tables["Send"].Rows[0]["TotalInputPrice"].ToString()) + FS;
            //SendDate = SendDate + "1" + FS;    // '''1판매원   2소비자 우선 1로 다 셋팅함
            //SendDate = SendDate + ds.Tables["Send"].Rows[0]["M_Name"].ToString() + FS;
            //SendDate = SendDate + Cpno + FS;
            //SendDate = SendDate + ds.Tables["Send"].Rows[0]["M_Mbid"].ToString() + FS;
            //SendDate = SendDate + "" + FS;
            //SendDate = SendDate + "" + FS;
            //SendDate = SendDate + "" + FS;
            //SendDate = SendDate + "" + FS;
            //SendDate = SendDate + "" + FS;
            //SendDate = SendDate + "" + FS;
            //SendDate = SendDate + "" + FS;
            //SendDate = SendDate + "" + FS;
            //SendDate = SendDate + "" + FS;
            //SendDate = SendDate + "" + FS;
            //SendDate = SendDate + "" + FS;
            //SendDate = SendDate + "" + ETX;

            //setbyte = Encoding.Default.GetBytes(SendDate);


            DT_Time = ds.Tables["Send"].Rows[0]["DT_Time"].ToString();  // 현재 시간을 가져온다... 로그 파일 때문임

            string userid = encrypter.Decrypt(ds.Tables["Send"].Rows[0]["Cpno"].ToString(), "Cpno_Union");
            string BirthDay = ds.Tables["Send"].Rows[0]["BirthDay"].ToString();

            int D_Sex = 1;
            if (ds.Tables["Send"].Rows[0]["Sex_FLAG"].ToString() == "Y") D_Sex = 1;
            if (ds.Tables["Send"].Rows[0]["Sex_FLAG"].ToString() == "X") D_Sex = 2;

            double Input_C = double.Parse(ds.Tables["Send"].Rows[0]["Input_C"].ToString());
            double InputCard = double.Parse(ds.Tables["Send"].Rows[0]["InputCard"].ToString());
            double InputMile = double.Parse(ds.Tables["Send"].Rows[0]["InputMile"].ToString());

            string pay_method = "EC";
            int Pay_Cnt = 0;

            if (Input_C > 0)
            {
                Pay_Cnt++;
                pay_method = "CH";
            }

            if (InputCard > 0)
            {
                Pay_Cnt++;
                pay_method = "CD";
            }

            if (InputMile > 0)
            {
                Pay_Cnt++;
                pay_method = "EC";
            }

            if (Pay_Cnt >= 2) //2개 이상 결제로 이루어 졋다. 복합결제이다.
                pay_method = "MI";

            if (InputCard > 0 && Pay_Cnt == 1)  //카드 결제이고 복합 결제가 아닌경우에
            {
                //결제 관련 정보 테이블로 가서  C_Card_Sort 내역을 가져온다. C_Card_Sort = '1' 이외에는 다 신용카드    C_Card_Sort = '1'은 체크카드임
                //체크는 CC임.

                Tsql = "SELECT C_Card_Sort";
                Tsql = Tsql + " From tbl_Sales_Cacu  (nolock) ";
                Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "'";
                Tsql = Tsql + " And C_TF = 3 "; //카드 관련 결제 정보들을 불러온다.

                DataSet ds_C = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, "Send", ds_C) == false) return;
                int ReCnt_C = Temp_Connect.DataSet_ReCount;

                if (ReCnt_C == 1) //한건만 있는 경우에 그 종류가 1이다 그럼 결제 정보는 체크카드로 
                {
                    if (ds_C.Tables["Send"].Rows[0]["C_Card_Sort"].ToString() == "1")
                        pay_method = "CC";
                }

                if (ReCnt_C > 1) //여러건이 있는 경우에는 신용 체크 중복인지 등을 체크한다.
                {
                    int CC_TF = 0, CD_TF = 0;
                    for (int fi_cnt = 0; fi_cnt <= ReCnt_C - 1; fi_cnt++)
                    {
                        if (ds_C.Tables["Send"].Rows[0]["C_Card_Sort"].ToString() == "1")
                        {
                            pay_method = "CC";
                            CC_TF++;
                        }
                        else
                        {
                            pay_method = "CD";
                            CD_TF++;
                        }
                    }

                    if (CC_TF > 0 && CD_TF > 0)  //신용 하고 체크로  둘다 썻다 그럼 복합결제가 되는 것이다.
                        pay_method = "MI";
                }

            }


            if (userid == "")
            {
                if (BirthDay.Length >= 8 && D_Sex > 0)
                    userid = BirthDay.Substring(2, 6) + D_Sex.ToString();
                else
                    userid = "9999999";
            }

            string orderid = ds.Tables["Send"].Rows[0]["OrderNumber"].ToString();
            string mem_id = ds.Tables["Send"].Rows[0]["Mbid"].ToString() + "-" + ds.Tables["Send"].Rows[0]["Mbid2"].ToString();
            string name = ds.Tables["Send"].Rows[0]["M_Name"].ToString();

            double totalmoney = double.Parse(ds.Tables["Send"].Rows[0]["TotalPrice"].ToString());
            int totalmoney2 = int.Parse(totalmoney.ToString());

            int seller_type = 0;
            if (ds.Tables["Send"].Rows[0]["Sell_Mem_TF"].ToString() == "0")
                seller_type = 1;



            //cls_app_static_var.Dir_Company_Code = "6002" ;
            str_sendvalue = "orderid=" + orderid;
            str_sendvalue = str_sendvalue + "&shopid=" + cls_app_static_var.Dir_Company_Code;
            str_sendvalue = str_sendvalue + "&totalmoney=" + totalmoney;
            str_sendvalue = str_sendvalue + "&seller_type=" + seller_type;
            str_sendvalue = str_sendvalue + "&name=" + name;
            str_sendvalue = str_sendvalue + "&userid=" + userid;
            str_sendvalue = str_sendvalue + "&mem_id=" + mem_id;

            str_sendvalue = str_sendvalue + "&ctype=c";
            str_sendvalue = str_sendvalue + "&returntype=xml";
            str_sendvalue = str_sendvalue + "&pay_method=" + pay_method;


            // str_sendvalue = encrypter.Encrypt(str_sendvalue);

            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            //Read_T_P_Gid(str_sendvalue);
            Ord_SW = 1;

            Dir_Send_Log_Table_Make(Temp_Connect, OrderNumber, "A", double.Parse(ds.Tables["Send"].Rows[0]["TotalInputPrice"].ToString()));
        }


        private void Search_Sell_DAte_Cancel(string OrderNumber, ref int Ord_SW, ref  string str_sendvalue)
        {

            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

            string SendDate = null;
            string Tsql = "";

            //Tsql = "Select  ";
            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) AS M_Mbid ";
            //else
            //    Tsql = Tsql + " tbl_Memberinfo.mbid2 AS M_Mbid ";
            //Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";
            //Tsql = Tsql + ", tbl_Memberinfo.Cpno ";

            //Tsql = Tsql + " , tbl_SalesDetail.TotalInputPrice ";
            //Tsql = Tsql + " ,Convert(Varchar(25),GetDate(),120) AS DT_Time  ";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();


            Tsql = "SELECT tbl_Memberinfo.Cpno, tbl_Memberinfo.Mbid,tbl_Memberinfo.Mbid2, tbl_Memberinfo.M_Name, tbl_Memberinfo.Sex_FLAG ";
            Tsql = Tsql + " ,tbl_SalesDetail.OrderNumber , tbl_SalesDetail.TotalPrice , tbl_Memberinfo.Sell_Mem_TF , Isnull(tbl_SalesDetail.InsuranceNumber,'')  INS_Num ";
            Tsql = Tsql + " ,tbl_Memberinfo.BirthDay , TotalInputPrice  ,INS_Num_Cancel_Err , InsuranceNumber   , Convert(varchar(40),getdate(),21) DT_Time  ";
            Tsql = Tsql + " ,tbl_SalesDetail.ReturnTF ";
            Tsql = Tsql + " From tbl_SalesDetail  (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid   And  tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";
            Tsql = Tsql + " Where tbl_SalesDetail.OrderNumber = '" + OrderNumber + "'";
            Tsql = Tsql + " And  tbl_SalesDetail.InsuranceNumber <> '' ";
            Tsql = Tsql + " And  tbl_SalesDetail.INS_Num_Cancel_Err <> '3001' ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Send", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            string userid = encrypter.Decrypt(ds.Tables["Send"].Rows[0]["Cpno"].ToString(), "Cpno_Union");
            string BirthDay = ds.Tables["Send"].Rows[0]["BirthDay"].ToString();
            //int D_Sex = int.Parse(ds.Tables["Send"].Rows[0]["Sex_FLAT"].ToString());
            int ReturnTF = int.Parse(ds.Tables["Send"].Rows[0]["ReturnTF"].ToString());

            DT_Time = ds.Tables["Send"].Rows[0]["DT_Time"].ToString();  // 현재 시간을 가져온다... 로그 파일 때문임

            //if (userid == "")
            //{
            //    if (BirthDay.Length >= 8 && D_Sex > 0)
            //        userid = BirthDay.Substring(2, 6) + D_Sex.ToString();
            //    else
            //        userid = "9999999";
            //}

            string orderid = ds.Tables["Send"].Rows[0]["OrderNumber"].ToString();
            string mem_id = ds.Tables["Send"].Rows[0]["Mbid"].ToString() + "-" + ds.Tables["Send"].Rows[0]["Mbid2"].ToString();
            string name = ds.Tables["Send"].Rows[0]["M_Name"].ToString();
            string Guaranteecode = ds.Tables["Send"].Rows[0]["INS_Num"].ToString();

            double totalmoney = 0;
            if (ReturnTF != 5)
                totalmoney = double.Parse(ds.Tables["Send"].Rows[0]["TotalPrice"].ToString());
            else
            {
                Tsql = "select Isnull(sum(itemtotalprice),0) TotalPrice from tbl_SalesitemDetail  (nolock)  where  ItemCount >= 0 and  ordernumber = '" + OrderNumber + "'";

                DataSet ds_i = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, "Send_i", ds_i) == false) return;
                int ReCnt2 = Temp_Connect.DataSet_ReCount;

                if (ReCnt2 > 0)
                    totalmoney = double.Parse(ds_i.Tables["Send_i"].Rows[0]["TotalPrice"].ToString());
            }

            if (totalmoney < 0)
                totalmoney = -totalmoney;

            int totalmoney2 = int.Parse(totalmoney.ToString());

            int seller_type = 0;
            if (ds.Tables["Send"].Rows[0]["Sell_Mem_TF"].ToString() == "0")
                seller_type = 1;

            //cls_app_static_var.Dir_Company_Code = "6002";
            str_sendvalue = "";
            str_sendvalue = "orderid=" + orderid;
            str_sendvalue = str_sendvalue + "&shopid=" + cls_app_static_var.Dir_Company_Code;
            str_sendvalue = str_sendvalue + "&totalmoney=" + totalmoney;
            str_sendvalue = str_sendvalue + "&guaranteecode=" + Guaranteecode;
            str_sendvalue = str_sendvalue + "&returntype=xml";

            //str_sendvalue = encrypter.Encrypt(str_sendvalue);

            setbyte = Encoding.Default.GetBytes(str_sendvalue);

            Ord_SW = 1;

            Dir_Send_Log_Table_Make(Temp_Connect, OrderNumber, "C", double.Parse(ds.Tables["Send"].Rows[0]["TotalInputPrice"].ToString()), Guaranteecode);




        }



        private void Search_Sell_DAte_Cancel__2(string OrderNumber, double Cancel_PR, ref int Ord_SW, ref  string str_sendvalue)
        {

            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

            string SendDate = null;
            string Tsql = "";

            //Tsql = "Select  ";
            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) AS M_Mbid ";
            //else
            //    Tsql = Tsql + " tbl_Memberinfo.mbid2 AS M_Mbid ";
            //Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";
            //Tsql = Tsql + ", tbl_Memberinfo.Cpno ";

            //Tsql = Tsql + " , tbl_SalesDetail.TotalInputPrice ";
            //Tsql = Tsql + " ,Convert(Varchar(25),GetDate(),120) AS DT_Time  ";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();


            Tsql = "SELECT tbl_Memberinfo.Cpno, tbl_Memberinfo.Mbid,tbl_Memberinfo.Mbid2, tbl_Memberinfo.M_Name, tbl_Memberinfo.Sex_FLAG ";
            Tsql = Tsql + " ,tbl_SalesDetail.OrderNumber , tbl_SalesDetail.TotalPrice , tbl_Memberinfo.Sell_Mem_TF , Isnull(tbl_SalesDetail.InsuranceNumber,'')  INS_Num ";
            Tsql = Tsql + " ,tbl_Memberinfo.BirthDay , TotalInputPrice  ,INS_Num_Cancel_Err , InsuranceNumber   , Convert(varchar(40),getdate(),21) DT_Time  ";
            Tsql = Tsql + " ,tbl_SalesDetail.ReturnTF ";
            Tsql = Tsql + " From tbl_SalesDetail  (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid   And  tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";
            Tsql = Tsql + " Where tbl_SalesDetail.OrderNumber = '" + OrderNumber + "'";
            Tsql = Tsql + " And  tbl_SalesDetail.InsuranceNumber <> '' ";
            Tsql = Tsql + " And  tbl_SalesDetail.INS_Num_Cancel_Err <> '3001' ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Send", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            string userid = encrypter.Decrypt(ds.Tables["Send"].Rows[0]["Cpno"].ToString(), "Cpno_Union");
            string BirthDay = ds.Tables["Send"].Rows[0]["BirthDay"].ToString();
            //int D_Sex = int.Parse(ds.Tables["Send"].Rows[0]["Sex_FLAT"].ToString());
            int ReturnTF = int.Parse(ds.Tables["Send"].Rows[0]["ReturnTF"].ToString());

            DT_Time = ds.Tables["Send"].Rows[0]["DT_Time"].ToString();  // 현재 시간을 가져온다... 로그 파일 때문임

            //if (userid == "")
            //{
            //    if (BirthDay.Length >= 8 && D_Sex > 0)
            //        userid = BirthDay.Substring(2, 6) + D_Sex.ToString();
            //    else
            //        userid = "9999999";
            //}

            string orderid = ds.Tables["Send"].Rows[0]["OrderNumber"].ToString();
            string mem_id = ds.Tables["Send"].Rows[0]["Mbid"].ToString() + "-" + ds.Tables["Send"].Rows[0]["Mbid2"].ToString();
            string name = ds.Tables["Send"].Rows[0]["M_Name"].ToString();
            string Guaranteecode = ds.Tables["Send"].Rows[0]["INS_Num"].ToString();

            double totalmoney = 0;
            if (ReturnTF != 5)
                totalmoney = double.Parse(ds.Tables["Send"].Rows[0]["TotalPrice"].ToString());
            else
            {
                Tsql = "select Isnull(sum(itemtotalprice),0) TotalPrice from tbl_SalesitemDetail  (nolock)  where  ItemCount >= 0 and  ordernumber = '" + OrderNumber + "'";

                DataSet ds_i = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, "Send_i", ds_i) == false) return;
                int ReCnt2 = Temp_Connect.DataSet_ReCount;

                if (ReCnt2 > 0)
                    totalmoney = double.Parse(ds_i.Tables["Send_i"].Rows[0]["TotalPrice"].ToString());
            }

            if (totalmoney < 0)
                totalmoney = -totalmoney;

            int totalmoney2 = int.Parse(totalmoney.ToString());

            int seller_type = 0;
            if (ds.Tables["Send"].Rows[0]["Sell_Mem_TF"].ToString() == "0")
                seller_type = 1;


            str_sendvalue = "";
            str_sendvalue = "orderid=" + orderid;
            str_sendvalue = str_sendvalue + "&shopid=" + cls_app_static_var.Dir_Company_Code;
            //str_sendvalue = str_sendvalue + "&totalmoney=" + totalmoney;
            str_sendvalue = str_sendvalue + "&totalmoney=" + Cancel_PR;
            str_sendvalue = str_sendvalue + "&guaranteecode=" + Guaranteecode;
            str_sendvalue = str_sendvalue + "&returntype=xml";

            //str_sendvalue = encrypter.Encrypt(str_sendvalue);

            setbyte = Encoding.Default.GetBytes(str_sendvalue);

            Ord_SW = 1;

            Dir_Send_Log_Table_Make(Temp_Connect, OrderNumber, "D", double.Parse(ds.Tables["Send"].Rows[0]["TotalInputPrice"].ToString()), Guaranteecode);




        }


        private void Dir_Send_Log_Table_Make(cls_Connect_DB Temp_Connect, string OrderNumber, string Send_Flag, double TotalPrice = 0, string InsuranceNumber = "")
        {

            string StrSql = "insert into tbl_Sales_Insu ( ";
            StrSql = StrSql + "Send_Flag,Back_Flag, OrderNumber ,TotalPrice ";
            StrSql = StrSql + ",InsuranceNumber, Err_Code ,RecordID ";
            StrSql = StrSql + ",RecordTime ";
            StrSql = StrSql + " ) ";
            StrSql = StrSql + " values ( ";
            StrSql = StrSql + "'" + Send_Flag + "','','" + OrderNumber + "'," + TotalPrice + ",";
            StrSql = StrSql + "'" + InsuranceNumber + "','','" + cls_User.gid + "',  '" + DT_Time + "'";
            StrSql = StrSql + " ) ";

            if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;

        }

        private void Dir_Back_Log_Table_Make(cls_Connect_DB Temp_Connect, string OrderNumber, string InsuranceNumber, ref string Err_Code, string Back_Flag)
        {

            Back_Flag = Back_Flag.Replace(" ", "");
            Err_Code = Err_Code.Replace(" ", "").Replace("<ErrorCode>", "").Replace("<", "");

            string StrSql = "Update tbl_Sales_Insu SEt ";
            StrSql = StrSql + "InsuranceNumber = '" + InsuranceNumber + "'";
            StrSql = StrSql + ",Err_Code = '" + Err_Code + "'";
            StrSql = StrSql + ",Back_Flag = '" + Back_Flag + "'";
            StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
            StrSql = StrSql + " And   Send_Flag = 'A' ";
            StrSql = StrSql + " And   RecordTime = '" + DT_Time + "'";

            if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;


            if (Back_Flag == "Y")
            {
                StrSql = "Update tbl_SalesDetail SEt ";
                StrSql = StrSql + "InsuranceNumber = '" + InsuranceNumber + "'";
                StrSql = StrSql + ",INS_Num_Err = ''";
                StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";

                if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;
            }

            if (Back_Flag == "D")
            {
                StrSql = "Update tbl_SalesDetail SEt ";
                StrSql = StrSql + "InsuranceNumber = '" + InsuranceNumber + "'";
                StrSql = StrSql + ",INS_Num_Err = ''";
                StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
                StrSql = StrSql + " And InsuranceNumber = '' ";

                if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;
            }

            if (Err_Code != "" && Back_Flag != "Y" && Back_Flag != "D")
            {
                StrSql = "Update tbl_SalesDetail SEt ";
                StrSql = StrSql + " INS_Num_Err = '" + Err_Code + "'";
                StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";

                if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;


                //string Tsql = "";

                //Tsql = "Select  Err_Msg  From tbl_Sales_Insu_Err  (nolock) ";
                //Tsql = Tsql + " Where Err_Code = '" + Err_Code + "'";
                ////++++++++++++++++++++++++++++++++               

                //DataSet ds = new DataSet();
                ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                //if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Sales_Insu_Err", ds) == false) return;
                //int ReCnt = Temp_Connect.DataSet_ReCount;

                //if (ReCnt == 0) return;

                //Err_Code = Err_Code + ' ' + ds.Tables["tbl_Sales_Insu_Err"].Rows[0]["Err_Msg"].ToString();
                ////++++++++++++++++++++++++++++++++                      
            }

            //string StrSql = "Update tbl_Sales_Insu SEt ";
            //StrSql = StrSql + "InsuranceNumber = '" + InsuranceNumber  +"'" ; 
            //StrSql = StrSql + ",Err_Code = '" + Err_Code  +"'" ;
            //StrSql = StrSql + ",Back_Flag = '" + Back_Flag + "'"; 
            //StrSql = StrSql + " Where OrderNumber = '" + OrderNumber  + "'" ;
            //StrSql = StrSql + " And   Send_Flag = 'A' ";
            //StrSql = StrSql + " And   RecordTime = '" + DT_Time + "'";

            //if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;


            //if (Back_Flag == "Y")
            //{
            //    StrSql = "Update tbl_SalesDetail SEt ";
            //    StrSql = StrSql + "InsuranceNumber = '" + InsuranceNumber + "'";
            //    StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";

            //    if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;
            //}

            //if (Back_Flag == "D")
            //{
            //    StrSql = "Update tbl_SalesDetail SEt ";
            //    StrSql = StrSql + "InsuranceNumber = '" + InsuranceNumber + "'";
            //    StrSql = StrSql + ",INS_Num_Err = ''";
            //    StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
            //    StrSql = StrSql + " And InsuranceNumber = '' ";

            //    if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;
            //}

            //if (Err_Code != "")
            //{
            //    string Tsql = "";

            //    Tsql = "Select  Err_Msg  From tbl_Sales_Insu_Err  (nolock) ";
            //    Tsql = Tsql + " Where Err_Code = '" + Err_Code + "'";
            //    //++++++++++++++++++++++++++++++++               

            //    DataSet ds = new DataSet();
            //    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //    if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Sales_Insu_Err", ds) == false) return;
            //    int ReCnt = Temp_Connect.DataSet_ReCount;

            //    if (ReCnt == 0) return;

            //    Err_Code = Err_Code + ' ' + ds.Tables["tbl_Sales_Insu_Err"].Rows[0]["Err_Msg"].ToString();
            //    //++++++++++++++++++++++++++++++++                      
            //}

        }

        private void Dir_Back_Cancel_Log_Table_Make(cls_Connect_DB Temp_Connect, string OrderNumber, string InsuranceNumber, ref string Err_Code, string Back_Flag)
        {


            Back_Flag = Back_Flag.Replace(" ", "");
            Err_Code = Err_Code.Replace(" ", "").Replace("<ErrorCode>", "").Replace("<", "");

            string StrSql = "Update tbl_Sales_Insu SEt ";
            StrSql = StrSql + "InsuranceNumber = '" + InsuranceNumber + "'";
            StrSql = StrSql + ",Err_Code = '" + Err_Code + "'";
            StrSql = StrSql + ",Back_Flag = '" + Back_Flag + "'";
            StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
            StrSql = StrSql + " And   Send_Flag = 'C' ";
            StrSql = StrSql + " And   RecordTime = '" + DT_Time + "'";

            if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;


            if (Back_Flag == "Y" || Err_Code == "3000")
            {
                StrSql = "Update tbl_SalesDetail SEt ";
                StrSql = StrSql + "InsuranceNumber_Cancel = 'Y'";
                StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";

                if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;
            }

            if (Err_Code != "" && Back_Flag != "Y" && Back_Flag != "D" && Err_Code != "3000")
            {
                StrSql = "Update tbl_SalesDetail SEt ";
                StrSql = StrSql + "INS_Num_Cancel_Err = '" + Err_Code + "'";
                StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";

                if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;


                //string Tsql = "";

                //Tsql = "Select  Err_Msg  From tbl_Sales_Insu_Err  (nolock) ";
                //Tsql = Tsql + " Where Err_Code = '" + Err_Code + "'";
                ////++++++++++++++++++++++++++++++++               

                //DataSet ds = new DataSet();
                ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                //if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Sales_Insu_Err", ds) == false) return;
                //int ReCnt = Temp_Connect.DataSet_ReCount;

                //if (ReCnt == 0) return;

                //Err_Code = Err_Code + ' ' + ds.Tables["tbl_Sales_Insu_Err"].Rows[0]["Err_Msg"].ToString();
                //++++++++++++++++++++++++++++++++                      
            }

        }

        private void Dir_Back_Cancel_Log_Table_Make__2(cls_Connect_DB Temp_Connect, string OrderNumber, string InsuranceNumber, ref string Err_Code, string Back_Flag)
        {


            Back_Flag = Back_Flag.Replace(" ", "");
            Err_Code = Err_Code.Replace(" ", "").Replace("<ErrorCode>", "").Replace("<", "");

            string StrSql = "Update tbl_Sales_Insu SEt ";
            StrSql = StrSql + "InsuranceNumber = '" + InsuranceNumber + "'";
            StrSql = StrSql + ",Err_Code = '" + Err_Code + "'";
            StrSql = StrSql + ",Back_Flag = '" + Back_Flag + "'";
            StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
            StrSql = StrSql + " And   Send_Flag = 'D' ";  //부분취소인 경우에는 D
            StrSql = StrSql + " And   RecordTime = '" + DT_Time + "'";

            if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;


            if (Back_Flag == "Y" || Err_Code == "3000")
            {
                StrSql = "Update tbl_SalesDetail SEt ";
                StrSql = StrSql + "INS_Num_Cancel_Err = ''";
                StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";

                if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;
            }

            if (Err_Code != "" && Back_Flag != "Y" && Back_Flag != "D" && Err_Code != "3000")
            {
                StrSql = "Update tbl_SalesDetail SEt ";
                StrSql = StrSql + "INS_Num_Cancel_Err = '" + Err_Code + "'";
                StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";

                if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;


                //string Tsql = "";

                //Tsql = "Select  Err_Msg  From tbl_Sales_Insu_Err  (nolock) ";
                //Tsql = Tsql + " Where Err_Code = '" + Err_Code + "'";
                ////++++++++++++++++++++++++++++++++               

                //DataSet ds = new DataSet();
                ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                //if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Sales_Insu_Err", ds) == false) return;
                //int ReCnt = Temp_Connect.DataSet_ReCount;

                //if (ReCnt == 0) return;

                //Err_Code = Err_Code + ' ' + ds.Tables["tbl_Sales_Insu_Err"].Rows[0]["Err_Msg"].ToString();
                //++++++++++++++++++++++++++++++++                      
            }

            //string StrSql = "Update tbl_Sales_Insu SEt ";
            //StrSql = StrSql + "InsuranceNumber = '" + InsuranceNumber + "'";
            //StrSql = StrSql + ",Err_Code = '" + Err_Code + "'";
            //StrSql = StrSql + ",Back_Flag = '" + Back_Flag + "'";
            //StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
            //StrSql = StrSql + " And   Send_Flag = 'C' ";
            //StrSql = StrSql + " And   RecordTime = '" + DT_Time + "'";

            //if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;


            //if (Back_Flag == "Y")
            //{
            //    StrSql = "Update tbl_SalesDetail SEt ";
            //    StrSql = StrSql + "InsuranceNumber_Cancel = 'Y'";
            //    StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";

            //    if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;
            //}

            //if (Err_Code != "")
            //{
            //    string Tsql = "";

            //    Tsql = "Select  Err_Msg  From tbl_Sales_Insu_Err  (nolock) ";
            //    Tsql = Tsql + " Where Err_Code = '" + Err_Code + "'";
            //    //++++++++++++++++++++++++++++++++               

            //    DataSet ds = new DataSet();
            //    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //    if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Sales_Insu_Err", ds) == false) return;
            //    int ReCnt = Temp_Connect.DataSet_ReCount;

            //    if (ReCnt == 0) return;

            //    Err_Code = Err_Code + ' ' + ds.Tables["tbl_Sales_Insu_Err"].Rows[0]["Err_Msg"].ToString(); 
            //    //++++++++++++++++++++++++++++++++                      
            //}
        }



        private int byteArrayDefrag(byte[] sData)
        {
            int endLength = 0;

            for (int i = 0; i < sData.Length; i++)
            {
                if ((byte)sData[i] != (byte)0)
                {
                    endLength = i;
                }
            }

            return endLength;
        }

        List<string[]> ConvertsNames = new List<string[]>();

        public string NaverOpenAPI_KorName_To_EngName(string NAME)
        {
            if (ConvertsNames.Exists(x => x[0].Equals(NAME)))
            {
                return ConvertsNames.Find(x => x[0].Equals(NAME))[1];
            }

            try
            {
                string url = "https://openapi.naver.com/v1/krdict/romanization?query=" + NAME;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("X-Naver-Client-Id", "r31fsM_ySOjB9PXVJgPv"); // 개발자센터에서 발급받은 Client ID
                request.Headers.Add("X-Naver-Client-Secret", "eXZVSEGjYS"); // 개발자센터에서 발급받은 Client Secret
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string text = reader.ReadToEnd();
                Console.WriteLine(text);

                JObject JSonData = JObject.Parse(text);
                if (JSonData["aResult"].Last == null)
                    return string.Empty;
                string LASTDATA = JSonData["aResult"].Last.Last.Last.ToString();

                JArray JArray1 = JArray.Parse(LASTDATA);

                foreach (JObject item in JArray1.Children<JObject>())
                {
                    string name = item["name"].ToString();
                    ConvertsNames.Add(new string[] { NAME, name});
                    return name;
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }



    }
}
