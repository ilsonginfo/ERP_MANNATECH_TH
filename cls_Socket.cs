﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Data;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace MLM_Program
{
    class cls_Socket
    {
        private static byte[] getbyte = new byte[270];
        private static byte[] setbyte = new byte[5400];
        //private string DT_Time = "";

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        const char STX = (char)0x02;
        const char FS = (char)0x1c;
        const char ETX = (char)0x03;

        /*조합신고, AccMode는 가상계좌발행 하게되면 바로 직판쪽에 공제가가도록 처리하기위함 */
        public string Dir_Connect_Send(string OrderNumber, bool NonGaorderMode = false)
        {
            int Ord_SW = 0;
            string str_sendvalue = "", DT_Time = "";
            Search_Sell_Date(OrderNumber, ref Ord_SW, ref str_sendvalue, ref DT_Time, NonGaorderMode);

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


        /*조합취소*/
        public string Dir_Connect_Send_Cancel(string OrderNumber)
        {
            int Ord_SW = 0;
            string str_sendvalue = "", DT_Time = "";
            Search_Sell_Date_Cancel(OrderNumber, ref Ord_SW, ref str_sendvalue, ref DT_Time);

                if (Ord_SW == 0)
                return "-10000";

            string URL = cls_app_static_var.CancelAssociationURL;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "GPS";
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
            getstring = readerPost.ReadToEnd().ToString();

            string Err_Code = "";
            Err_Code = Back_Cancel_Date_Input(getstring, OrderNumber, DT_Time);

            return Err_Code;
        }



        /*조합부분취소*/
        public string Dir_Connect_Send_Cancel__2(string RefundOrderNumber)
        {
            int Ord_SW = 0;
            string str_sendvalue = "", DT_Time = "";
            Search_Sell_DAte_Cancel__2(RefundOrderNumber, ref Ord_SW, ref str_sendvalue, ref DT_Time);

            if (Ord_SW == 0)
                return "-10000";

            string URL = cls_app_static_var.CancelAssociationURL;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "GPS";
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
            getstring = readerPost.ReadToEnd().ToString();//.Replace("\"", "").Replace("{", "").Replace("}", "").Replace("\r","").Replace("\n","");

            string Err_Code = "";
            Err_Code = Back_Cancel_Date_Input__2(getstring, RefundOrderNumber, DT_Time);

            return Err_Code;
        }


        private string Back_Date_Input(string Getstring, string OrderNumber, string DT_Time)
        {
            string InsuranceNumber = "", Err_Code = "";
            string Back_Flag = "";
            string SuccessYN = "";

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
                SuccessYN = ReturnData["successYN"].ToString();
            }
            catch
            {
                return "N";
            }

            if (SuccessYN == "Y")
            {
                InsuranceNumber = ReturnData["GuaranteeCode"].ToString();
                Back_Flag = ReturnData["guaranteeResult"].ToString();

            }
            else
            {
                //string errMessage = ReturnData["errMessage"].ToString();
                if(ReturnData["ErrorCode"] != null)
                 Err_Code = ReturnData["ErrorCode"].ToString();
                SuccessYN = Err_Code;
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Dir_Back_Log_Table_Make(Temp_Connect, OrderNumber, InsuranceNumber, ref Err_Code, Back_Flag, DT_Time);

            if (SuccessYN != "Y")
            {
                //MessageBox.Show("조합관련에러 : " + Err_Code);
            }


            return SuccessYN;
        }


        private string Back_Cancel_Date_Input(string Getstring, string OrderNumber, string DT_Time)
        {
            string InsuranceNumber = "", Err_Code = "";
            JObject ReturnData = new JObject();
            string SuccessYN = "";

            try
            {
                ReturnData = JObject.Parse(Getstring);
                SuccessYN = ReturnData["successYN"].ToString();
            }
            catch
            {
                return "N";
            }

            if (SuccessYN == "Y")
            {
                string ins_A = ReturnData["guaranteeCode"].ToString();
                InsuranceNumber = ins_A;

                if (ReturnData["errorCode"] != null)
                    Err_Code = ReturnData["errorCode"].ToString();
            }
            else
            {
                InsuranceNumber = "";
                if (ReturnData["errorCode"] != null)
                    Err_Code = ReturnData["errorCode"].ToString();

                if (Err_Code == null)
                    Err_Code = "";

                if (Err_Code == "3000")
                    SuccessYN = "Y";
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Dir_Back_Cancel_Log_Table_Make(Temp_Connect, OrderNumber, InsuranceNumber, ref Err_Code, SuccessYN, DT_Time);

            return Err_Code;
        }

        private string Back_Cancel_Date_Input__2(string Getstring, string RefundOrderNumber, string DT_Time)
        {
            string InsuranceNumber = "", Err_Code = "";
            JObject ReturnData = new JObject();
            string SuccessYN = "";

            try
            {
                ReturnData = JObject.Parse(Getstring);
                SuccessYN = ReturnData["successYN"].ToString();
            }
            catch
            {
                return "N";
            }

            if (SuccessYN == "Y" || SuccessYN == "D")
            {
                string ins_A = ReturnData["guaranteeCode"].ToString();
                InsuranceNumber = ins_A;

                if (ReturnData["errorCode"] != null)
                    Err_Code = ReturnData["errorCode"].ToString();
            }
            else
            {
                InsuranceNumber = "";
                Err_Code = Err_Code = ReturnData["errorCode"].ToString();
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Dir_Back_Cancel_Log_Table_Make__2(Temp_Connect, RefundOrderNumber, InsuranceNumber, ref Err_Code, SuccessYN, DT_Time);

            return Err_Code;
        }

        void Search_Sell_Date(string OrderNumber, ref int Ord_SW, ref string str_sendvalue, ref string DT_Time, bool NonGaorderMode = false)
        {
            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT tbl_Memberinfo.Cpno, tbl_Memberinfo.Mbid,tbl_Memberinfo.Mbid2, tbl_Memberinfo.M_Name,  tbl_Memberinfo.Sex_FLAG");
            sb.AppendLine(" ,tbl_SalesDetail.OrderNumber , tbl_SalesDetail.TotalPrice , tbl_SalesDetail.InputCoupon , tbl_Memberinfo.Sell_Mem_TF , Isnull(tbl_SalesDetail.InsuranceNumber,'')  INS_Num ");
            sb.AppendLine(" ,tbl_Memberinfo.BirthDay + tbl_Memberinfo.BirthDay_M + tbl_Memberinfo.BirthDay_D BirthDay , TotalInputPrice  , Convert(varchar(40),getdate(),21) DT_Time ");
            sb.AppendLine(" ,InputCash + InputPassbook  AS  Input_C , InputCard , InputMile,InputNaver ");
            sb.AppendLine(" From tbl_SalesDetail  (nolock) ");
            sb.AppendLine(" LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid   And  tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ");
            sb.AppendLine(" Where tbl_SalesDetail.OrderNumber = '" + OrderNumber + "'");
          //sb.AppendLine(" And  tbl_SalesDetail.TotalinputPrice - inputPass_Pay  = TotalPrice   ");
            sb.AppendLine(" And  Isnull(tbl_SalesDetail.InsuranceNumber,'')  = '' ");
            sb.AppendLine(" And  tbl_Memberinfo.Na_Code = 'KR'  ");
            string Tsql = sb.ToString();
            if (!NonGaorderMode)
            {
                Tsql = Tsql + " and tbl_SalesDetail.Ga_Order = 0 ";
            }

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Send", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            string userid = encrypter.Decrypt(ds.Tables["Send"].Rows[0]["Cpno"].ToString(), "Cpno_Union");
            string BirthDay = ds.Tables["Send"].Rows[0]["BirthDay"].ToString();

            int D_Sex = 1;
            if (ds.Tables["Send"].Rows[0]["Sex_FLAG"].ToString() == "Y") D_Sex = 1;
            if (ds.Tables["Send"].Rows[0]["Sex_FLAG"].ToString() == "X") D_Sex = 2;

            double Input_C = double.Parse(ds.Tables["Send"].Rows[0]["Input_C"].ToString());
            double InputCard = double.Parse(ds.Tables["Send"].Rows[0]["InputCard"].ToString());
            double InputNaver = double.Parse(ds.Tables["Send"].Rows[0]["InputNaver"].ToString());
            double InputMile = double.Parse(ds.Tables["Send"].Rows[0]["InputMile"].ToString());
            double InputCoupon = double.Parse(ds.Tables["Send"].Rows[0]["InputCoupon"].ToString());

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

            if (InputNaver > 0)
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
            if (InputNaver > 0 && Pay_Cnt == 1)  //카드 결제이고 복합 결제가 아닌경우에
            {
                pay_method = "CD";
            }
                if (InputCard > 0 && Pay_Cnt == 1)  //카드 결제이고 복합 결제가 아닌경우에
            {
                //Danal 에서는 신용인지 체크인지 알아볼 수 있는 방법이없다.
                pay_method = "CD";

                /*
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
                        if (ds_C.Tables["Send"].Rows[0]["C_Card_Sort"].ToString() == "1")   //체크카드
                        {
                            pay_method = "CC";
                            CC_TF++;
                        }
                        else    //신용카드
                        {
                            pay_method = "CD";
                            CD_TF++;
                        }
                    }

                    if (CC_TF > 0 && CD_TF > 0)  //신용 하고 체크로  둘다 썻다 그럼 복합결제가 되는 것이다.
                        pay_method = "MI";
                }
                */
            }


            if (userid == "")
            {
                if (BirthDay.Length >= 8 && D_Sex > 0)
                    userid = BirthDay.Substring(2, 6) + D_Sex.ToString();
                else
                    userid = "9999999";
            }

            string orderid = ds.Tables["Send"].Rows[0]["OrderNumber"].ToString();
            //string mem_id = ds.Tables["Send"].Rows[0]["Mbid"].ToString() + "-" + ds.Tables["Send"].Rows[0]["Mbid2"].ToString();
            string mem_id = ds.Tables["Send"].Rows[0]["Mbid2"].ToString();
            string name = ds.Tables["Send"].Rows[0]["M_Name"].ToString();
            string mbid = ds.Tables["Send"].Rows[0]["Mbid"].ToString();
            string mbid2 = ds.Tables["Send"].Rows[0]["Mbid2"].ToString();
            
            //쿠폰값을 뺀 값을 신고한다.
            double totalmoney_befor = double.Parse(ds.Tables["Send"].Rows[0]["TotalPrice"].ToString());

            double totalmoney = totalmoney_befor - InputCoupon;

            int totalmoney2 = int.Parse(totalmoney.ToString());

            int seller_type = 2;
            if (ds.Tables["Send"].Rows[0]["Sell_Mem_TF"].ToString() == "0")
                seller_type = 1;

            str_sendvalue = "orderid=" + orderid;
            str_sendvalue = str_sendvalue + "&shopid=" + cls_app_static_var.Dir_Company_Code;
            str_sendvalue = str_sendvalue + "&totalmoney=" + totalmoney;
            str_sendvalue = str_sendvalue + "&sellerType=" + seller_type;
            str_sendvalue = str_sendvalue + "&name=" + name;
            str_sendvalue = str_sendvalue + "&userid=" + userid;
            str_sendvalue = str_sendvalue + "&memId=" + mem_id;

            str_sendvalue = str_sendvalue + "&ctype=w";     //c에서 w로 변경 한글깨짐현상으로 인해
            //str_sendvalue = str_sendvalue + "&returntype=xml";
            str_sendvalue = str_sendvalue + "&payMethod=" + pay_method;

            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;

            Dir_Send_Log_Table_Make(Temp_Connect, OrderNumber, "A", double.Parse(ds.Tables["Send"].Rows[0]["TotalPrice"].ToString()), "", ref DT_Time);
        }

        /// <summary> 반품 및 취소에 관한 로직, 데이터를 가져옴  </summary>
        /// <param name="OrderNumber">주문번호</param>
        /// <param name="Ord_SW"></param>
        /// <param name="str_sendvalue"></param>
        /// <returns>DT_Time 반환</returns>
        private void Search_Sell_Date_Cancel(string OrderNumber, ref int Ord_SW, ref string str_sendvalue, ref string DT_Time)
        {
            string Tsql = "";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            Tsql = " SELECT * FROM [ufn_Get_Cancel_InsNum_Data]('" + OrderNumber + "')";

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Send", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            string orderid = ds.Tables["Send"].Rows[0]["OrderNumber"].ToString();
            string Guaranteecode = ds.Tables["Send"].Rows[0]["Ins_Num"].ToString();

            double totalmoney = double.Parse(ds.Tables["Send"].Rows[0]["TotalPrice"].ToString());


            if (totalmoney < 0)
                totalmoney = -totalmoney;

            str_sendvalue = "";
            str_sendvalue = "orderid=" + orderid;
            str_sendvalue = str_sendvalue + "&shopid=" + cls_app_static_var.Dir_Company_Code;
            str_sendvalue = str_sendvalue + "&totalmoney=" + totalmoney;
            str_sendvalue = str_sendvalue + "&guaranteecode=" + Guaranteecode;

            setbyte = Encoding.Default.GetBytes(str_sendvalue);

            Ord_SW = 1;

            Dir_Send_Log_Table_Make(Temp_Connect, OrderNumber, "C", double.Parse(ds.Tables["Send"].Rows[0]["TotalPrice"].ToString()), Guaranteecode, ref DT_Time);
        }


        private void Search_Sell_DAte_Cancel__2(string RefundOrderNumber, ref int Ord_SW, ref string str_sendvalue, ref string DT_Time)
        {
            string Tsql = "";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            Tsql = " SELECT * FROM [ufn_Get_Cancel_InsNum_Data]('" + RefundOrderNumber + "')";

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Send", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            string orderid = ds.Tables["Send"].Rows[0]["OrderNumber"].ToString();
            string Guaranteecode = ds.Tables["Send"].Rows[0]["Ins_Num"].ToString();

            double totalmoney = double.Parse(ds.Tables["Send"].Rows[0]["TotalPrice"].ToString());


            if (totalmoney < 0)
                totalmoney = -totalmoney;

            str_sendvalue = "";
            str_sendvalue = "orderid=" + orderid;
            str_sendvalue = str_sendvalue + "&shopid=" + cls_app_static_var.Dir_Company_Code;
            str_sendvalue = str_sendvalue + "&totalmoney=" + totalmoney;
            str_sendvalue = str_sendvalue + "&guaranteecode=" + Guaranteecode;

            setbyte = Encoding.Default.GetBytes(str_sendvalue);

            Ord_SW = 1;

            Dir_Send_Log_Table_Make(Temp_Connect, RefundOrderNumber, "D", totalmoney, Guaranteecode, ref DT_Time);
        }


        private void Dir_Send_Log_Table_Make(cls_Connect_DB Temp_Connect, string OrderNumber, string Send_Flag, double TotalPrice, string InsuranceNumber, ref string DT_Time)
        {
            DataSet ds = new DataSet();

            string StrSql = "SELECT convert(varchar, getdate(), 21) DT_Time";
            if (Temp_Connect.Open_Data_Set(StrSql, "DT_Time", ds) == false) return;
            if (Temp_Connect.DataSet_ReCount == 0) return;
            DT_Time = ds.Tables[0].Rows[0][0].ToString();

            StrSql = "insert into tbl_Sales_Insu ( ";
            StrSql = StrSql + "Send_Flag,Back_Flag, OrderNumber ,TotalPrice ";
            StrSql = StrSql + ",InsuranceNumber, Err_Code ,RecordID ";
            StrSql = StrSql + ",RecordTime ";
            StrSql = StrSql + " ) ";
            StrSql = StrSql + " values ( ";
            StrSql = StrSql + "'" + Send_Flag + "','','" + OrderNumber + "'," + TotalPrice + ",";
            StrSql = StrSql + "'" + InsuranceNumber + "','','" + cls_User.gid + "', '" + DT_Time + "'";
            StrSql = StrSql + " ) ";

            if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;

        }

        private void Dir_Back_Log_Table_Make(cls_Connect_DB Temp_Connect, string OrderNumber, string InsuranceNumber, ref string Err_Code, string Back_Flag, string DT_Time)
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

            //DataSet ds = new DataSet();
            //StrSql = "SELECT * FROM tbl_Sales_Insu (NOLOCK) ";
            //StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
            //StrSql = StrSql + " And   Send_Flag = 'A' ";
            //StrSql = StrSql + " And   RecordTime = '" + DT_Time + "'";

            //if (Temp_Connect.Open_Data_Set(StrSql, "DD", ds) == false) return;
            //if (Temp_Connect.DataSet_ReCount == 0) return;
            //int Seq = Convert.ToInt32(ds.Tables[0].Rows[0]["Seqno"]);

            if (Back_Flag == "Y")
            {
                StrSql = "Update tbl_SalesDetail SEt ";
                StrSql = StrSql + "InsuranceNumber = '" + InsuranceNumber + "'";
                StrSql = StrSql + ",INS_Num_Err = ''";
                StrSql = StrSql + ",InsuranceNumber_Date = '" + DT_Time + "'";
                StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";

                if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;
            }

            if (Back_Flag == "D")
            {
                StrSql = "Update tbl_SalesDetail SEt ";
                StrSql = StrSql + "InsuranceNumber = '" + InsuranceNumber + "'";
                StrSql = StrSql + ",INS_Num_Err = ''";
                StrSql = StrSql + ",InsuranceNumber_Date = '" + DT_Time + "'";
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

            }
        }

        private void Dir_Back_Cancel_Log_Table_Make(cls_Connect_DB Temp_Connect, string OrderNumber, string InsuranceNumber, ref string Err_Code, string Back_Flag, string DT_Time)
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
                StrSql = "Update tbl_SalesDetail SET ";
                StrSql = StrSql + "InsuranceNumber_Cancel = 'Y'";
                StrSql = StrSql + " Where OrderNumber =  '" + OrderNumber + "'";

                if (Temp_Connect.Update_Data(StrSql, "DD") == false) return;


                StrSql = "Update tbl_SalesDetail SET ";
                StrSql = StrSql + "InsuranceNumber_Cancel = 'Y'";
                StrSql = StrSql + " Where OrderNumber = (SELECT TOP  1 OrderNumber  FROM tbl_SalesDetail (NOLOCK) A1 WHERE Re_BaseOrderNumber  = '" + OrderNumber + "')";

                if (Temp_Connect.Update_Data(StrSql, "DD") == false) return;
            }

            if (Err_Code != "" && Back_Flag != "Y" && Back_Flag != "D" && Err_Code != "3000")
            {
                StrSql = "Update tbl_SalesDetail SEt ";
                StrSql = StrSql + "INS_Num_Cancel_Err = '" + Err_Code + "'";
                StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";

                if (Temp_Connect.Update_Data(StrSql, "DD") == false) return;
            }


        }

        private void Dir_Back_Cancel_Log_Table_Make__2(cls_Connect_DB Temp_Connect, string RefundOrderNumber, string InsuranceNumber, ref string Err_Code, string Back_Flag, string DT_Time)
        {


            Back_Flag = Back_Flag.Replace(" ", "");
            Err_Code = Err_Code.Replace(" ", "").Replace("<ErrorCode>", "").Replace("<", "");

            if (Err_Code == null)
                Err_Code = "";


            string StrSql = "Update tbl_Sales_Insu SEt ";
            StrSql = StrSql + "InsuranceNumber = '" + InsuranceNumber + "'";
            StrSql = StrSql + ",Err_Code = '" + Err_Code + "'";
            StrSql = StrSql + ",Back_Flag = '" + Back_Flag + "'";
            StrSql = StrSql + " Where OrderNumber = '" + RefundOrderNumber + "'";
            StrSql = StrSql + " And   Send_Flag = 'D' ";  //부분취소인 경우에는 D
            StrSql = StrSql + " And   RecordTime = '" + DT_Time + "'";

            if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;


            if (Back_Flag == "Y" || Err_Code == "3000")
            {
                Err_Code = "Y";

                StrSql = "Update tbl_SalesDetail SEt ";
                StrSql = StrSql + "InsuranceNumber_Cancel = 'Y'";
                StrSql = StrSql + " Where OrderNumber = '" + RefundOrderNumber + "'";

                if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;
            }

            if (Err_Code != "" && Back_Flag != "Y" && Back_Flag != "D" && Err_Code != "3000")
            {
                StrSql = "Update tbl_SalesDetail SEt ";
                StrSql = StrSql + "INS_Num_Cancel_Err = '" + Err_Code + "'";
                StrSql = StrSql + " Where OrderNumber = '" + RefundOrderNumber + "'";

                if (Temp_Connect.Insert_Data(StrSql, "DD") == false) return;

            }

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



    }
}
