using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Reflection;

namespace MLM_Program
{
    class Dictionary
    {
    }


    class cls_Cash_Card_Admin_Cancel
    {
        private string Card_Sugi_Gid = "solrx5400g";
        //private string Etc_Card_Sugi_Mid = "nictest04m";
        //private string Etc_Card_Sugi_Key = "b+zhZ4yOZ7FsH8pm5lhDfHZEb79tIwnjsdA0FBXh86yLc6BJeFVrZFXhAoJ3gEWgrWwN+lJMV0W4hvDdbe4Sjw=="; 
        private string Etc_Card_Sugi_Mid = "solrx5477m";
        private string Etc_Card_Sugi_Key = "PPlblSY9tJZDfzFZrSkY0b4ZbkJAecJFjZRAyxpesNlN6uxgQYtZF6n0bbGFDHYkHiAd1IyUcGJHEyNtJbHXfg=="; 


        //private string Hana_Card_Sugi_Mid = "nictest04m";
        //private string Hana_Card_Sugi_Key = "b+zhZ4yOZ7FsH8pm5lhDfHZEb79tIwnjsdA0FBXh86yLc6BJeFVrZFXhAoJ3gEWgrWwN+lJMV0W4hvDdbe4Sjw=="; 
        private string Hana_Card_Sugi_Mid = "solrx5479m";
        private string Hana_Card_Sugi_Key = "SYx8fK4E7coozwEp9Nx76B+898P7rpVbyii0gudClA/7EPHlLrX7HrToVMluw5ZCjlB5pUOSCp5306KL118n6g==";


        //private string Bank_Mid = "nicepay00m";
        //private string Bank_Key = "EYzu8jGGMfqaDEp76gSckuvnaHHu+bC4opsSN6lHv3b2lurNYkVXrZ7Z1AoqQnXI3eLuaUFyoRNC6FkrzVjceg==";
        private string Bank_Mid = "solrx5476m";
        private string Bank_Key = "RghcnlxS0C7sk08q3tpFeAmoK57o6D84zpg+piUx1DUmOwVp54uexjFjo60/fp48zfUZVPyaKZfZ0nWeMjvL3A=="; 



        //private string Cash_Mid = "nicepay00m";
        //private string Cash_Key = "EYzu8jGGMfqaDEp76gSckuvnaHHu+bC4opsSN6lHv3b2lurNYkVXrZ7Z1AoqQnXI3eLuaUFyoRNC6FkrzVjceg==";
        private string Cash_Mid = "solrx5476m";
        private string Cash_Key = "RghcnlxS0C7sk08q3tpFeAmoK57o6D84zpg+piUx1DUmOwVp54uexjFjo60/fp48zfUZVPyaKZfZ0nWeMjvL3A=="; 


        private string Nice_cancelPwd = "123456";


        private string Etc_Card_Sugi_cancelPwd = "5477";
        private string Bank_cancelPwd = "5476";
        private string Cash_cancelPwd = "5476";
        private string Hana_Card_Sugi_cancelPwd = "5479";

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        public int Cash_Card_Send_Singo_Cancel(string OrderNumber, string T_Mbid, string C_Sort, int C_index = 0, string Cancel_Sort = "")
        {
            int Ret = 0;
            if (cls_User.gid == cls_User.SuperUserID)
            {
                if (C_Sort == "Cash")
                {
                    Ret = Cash_Send_Singo_Cancel(OrderNumber, T_Mbid, C_index);
                }
                else if (C_Sort == "Card")
                {
                    Ret = Card_Send_Singo_Cancel(OrderNumber, C_index, Cancel_Sort);
                }

                else if (C_Sort == "Card_D")
                {
                    Ret = Card_Detail_Send_Singo_Cancel(OrderNumber, C_index, Cancel_Sort);
                }



                else if (C_Sort == "Bank")
                {
                    Ret = Bank_Send_Singo_Cancel(OrderNumber, C_index);
                }

                return Ret;
            }
            else
            {
                try
                {
                    if (C_Sort == "Cash")
                    {
                        Ret = Cash_Send_Singo_Cancel(OrderNumber, T_Mbid, C_index);
                    }
                    else if (C_Sort == "Card")
                    {
                        Ret = Card_Send_Singo_Cancel(OrderNumber, C_index, Cancel_Sort);
                    }

                    else if (C_Sort == "Card_D")
                    {
                        Ret = Card_Detail_Send_Singo_Cancel(OrderNumber, C_index, Cancel_Sort);
                    }


                    else if (C_Sort == "Bank")
                    {
                        Ret = Bank_Send_Singo_Cancel(OrderNumber, C_index);
                    }

                    return Ret;
                }

                catch (Exception)
                {
                    return 1;
                }
            }
            //return 0;
        }

        private int Cash_Send_Singo_Cancel(string OrderNumber, string T_Mbid, int SC_index = 0)
        {
            int SW = 0;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
           KSPayApprovalCancelBean_2cs ksp = new KSPayApprovalCancelBean_2cs();
            GalaxiaApi_Pay_cs ksp_Ga = new GalaxiaApi_Pay_cs();

            string StrSql = "";
            int C_index = 0, Seq_No = 0; ;
            string C_Number4 = "";

            string T_rStatus = "";   // 상태 O : 승인, X : 거절                        
            string C_Number3 = "";   // 거래번호      //저장해야함 승인취소시 사용됨.
            string T_rHCashTransactionNo = "";
            string T_rHTradeDate = "";
            string T_rHTradeTime = "";
            string T_rHMessage1 = "";
            string C_Cash_Number = "";

            string Tsql = "Select C_index , C_Price1 , C_Number3 , C_Cash_Number , Nice_Mid  ";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "'";
            Tsql = Tsql + " And   C_TF   <> 3 ";
            Tsql = Tsql + "And  C_Cash_Number <> '' ";
            //Tsql = Tsql + " And   C_Price1 > 0 ";
            Tsql = Tsql + " And OrderNumber +'-' + Convert(varchar, C_index ) + '-' + C_Cash_Number ";
            Tsql = Tsql + " IN (Select OrderNumber +'-' + Convert(varchar, C_index ) + '-' + HTRANSACTIONNO  From TLS_PAYMENT_RECEIPT (nolock) Where HTRANSACTIONNO <> '' And ORDERNUMBER <> '' And HSTATUS = 'O' ) ";
            //Tsql = Tsql + " And OrderNumber +'-' + Convert(varchar, C_index ) ";
            //Tsql = Tsql + " IN (Select OrderNumber +'-' + Convert(varchar, C_index )  From tbl_Sales_Cacu_Bank (nolock) Where C_Number3 <> '') ";

            if (SC_index > 0)
                Tsql = Tsql + " And tbl_Sales_Cacu.C_index =" + SC_index;

            DataSet ds2 = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds2) == false) return 0;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    C_index = int.Parse(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_index"].ToString());
                    int Send_Amount = int.Parse(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Price1"].ToString());
                    C_Number3 = ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Number3"].ToString().Trim();
                    C_Cash_Number = ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Cash_Number"].ToString().Trim();
                    string  Nice_Mid = ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Cash_Number"].ToString().Trim();

                    StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Bank " + C_index + ",'" + OrderNumber + "','''','C' ,'" + C_Cash_Number + "','" + cls_User.gid + "' ";

                    DataSet ds = new DataSet();
                    Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

                    Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

                    if (Seq_No > 0)
                    {
                        int Cancel_TF = 0 ; 
                        //if (C_Cash_Number.Length == 12)  //기존 ksnet 관련해서는 거래번호가 12자리 였으므로 12자리일경우에는 ksnet 으로가서 취소하게 한다.
                        //{
                        //    C_Number4 = ksp.KSPayCancelPost_Cash(C_Cash_Number, ref T_rStatus, ref T_rHCashTransactionNo, ref T_rHTradeDate, ref T_rHTradeTime, ref T_rHMessage1);

                        //    if (C_Number4 != "" && T_rStatus != "X")  Cancel_TF = 1 ; 
                        //}
                        //else
                        //{

                        //string Cancel_Code = Nice_cancelPwd;
                        string Cancel_Code = Cash_cancelPwd;

                        string ORDER_DATE = ds.Tables["Cacu_Card"].Rows[0][1].ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                        string T_ord = OrderNumber;// +ORDER_DATE.Substring(ORDER_DATE.Length - 4, 4);
                        C_Number4 = ksp_Ga.KSPayCancelPost_Cash(C_Cash_Number, T_ord, ORDER_DATE,
                            ref T_rStatus, ref T_rHCashTransactionNo, ref T_rHTradeDate, ref T_rHTradeTime, ref T_rHMessage1, Send_Amount , Cancel_Code, Nice_Mid);

                        if (C_Number4 != "" && C_Number4 != "2001") Cancel_TF = 1; 
                        //}

                        if (Cancel_TF  == 1 )
                        {
                            StrSql = "Update tbl_Sales_Cacu SET ";
                            StrSql = StrSql + " C_Cash_Number = ''";  //ksnet 거래번호
                            StrSql = StrSql + " ,C_Number4 = '" + C_Number4 + "'";  //ksnet 거래번호

                            ////StrSql = StrSql + " ,C_CancelTF = 1 ";
                            ////StrSql = StrSql + " ,C_CancelDate = Convert(Varchar(25),GetDate(),21) ";
                            ////StrSql = StrSql + " ,C_CancelPrice = C_Price1 ";                   

                            StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                            StrSql = StrSql + " And   C_index = " + C_index;

                            Temp_Connect.Update_Data(StrSql, "", "");
                        }
                        else
                            SW++;
                        //T_rHCashTransactionNo = T_rAuthNo; ;

                        StrSql = "Update tbl_Sales_Cacu_Bank SET ";
                        StrSql = StrSql + " rStatus = '" + T_rStatus + "'";
                        StrSql = StrSql + " ,rHTradeDate = '" + T_rHTradeDate + "'";
                        StrSql = StrSql + " ,rHTradeTime = '" + T_rHTradeTime + "'";
                        StrSql = StrSql + " ,rHMessage1 = '" + T_rHMessage1 + "'";
                        StrSql = StrSql + " ,rHCashTransactionNo = '" + T_rHCashTransactionNo + "'";  //현금영수증 승인번호
                        StrSql = StrSql + " ,C_Number4 = '" + C_Number4 + "'"; //ksnet 거래번호
                        StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
                        StrSql = StrSql + " Where Seqno  =" + Seq_No;

                        Temp_Connect.Update_Data(StrSql, "", "");




                        //StrSql = "Insert into  TLS_PAYMENT_RECEIPT ";
                        //StrSql = StrSql + "(ORDERNUMBER, MBID, Mbid2, Type, C_INDEX,HTRANSACTIONNO,HSTATUS ";
                        //StrSql = StrSql + " , HCASHTRANSACTIONNO,HINCOMETYPE,HTRADEDATE,HTRADETIME,HMESSAGE1 ,REGDATE) ";
                        //StrSql = StrSql + " Values ( ";
                        //StrSql = StrSql + "'" + OrderNumber + "'";
                        //StrSql = StrSql + ",'" + T_Mbid + "'";
                        //StrSql = StrSql + ",0";
                        //StrSql = StrSql + ",'C'";
                        //StrSql = StrSql + "," + C_index;
                        //StrSql = StrSql + ",'" + C_Number3 + "'";
                        //StrSql = StrSql + ",'" + T_rStatus + "'";
                        //StrSql = StrSql + ",'" + T_rHCashTransactionNo + "'";
                        //StrSql = StrSql + ",'0'";
                        //StrSql = StrSql + ",'" + T_rHTradeDate + "'";
                        //StrSql = StrSql + ",'" + T_rHTradeTime + "'";
                        //StrSql = StrSql + ",'" + T_rHMessage1 + "'";
                        //StrSql = StrSql + ",GetDate() ";
                        //StrSql = StrSql + " ) ";

                        //Temp_Connect.Update_Data(StrSql, "", "");
                    }
                }


            }

            if (SW == 0)
                return 0;
            else
                return 100;
        }




        private int Card_Send_Singo_Cancel(string OrderNumber, int SC_index = 0, string Cancel_Sort = "")
        {
            int SW = 0;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            KSPayApprovalCancelBean_2cs ksp = new KSPayApprovalCancelBean_2cs();
            GalaxiaApi_Pay_cs ksp_Ga = new GalaxiaApi_Pay_cs();

            string StrSql = "";
            int C_index = 0, Seq_No = 0, Card_No_Null_TF = 0, C_C_Sum_Price1 = 0, C_Price1 = 0  ;
            string C_Number4 = "", T_rAuthNo = "", CardNo = "", Card_Per = "";

            string T_rStatus = "";   // 상태 O : 승인, X : 거절                        
            string C_Number3 = "", C_Number2 = "";   // 거래번호      //저장해야함 승인취소시 사용됨.

            string Tsql = "Select C_index , C_Price1 ,C_Number1,C_Number3 , C_Number2 ,C_Period1,C_Period2 , C_C_Sum_Price1, Convert(varchar,Getdate(),21)  NNDttt, Recordid , Nice_Mid  ";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "'";
            Tsql = Tsql + " And   C_TF   = 3 ";
            Tsql = Tsql + " And   C_Number3 <> '' ";
            Tsql = Tsql + " And   C_Price1 > 0 ";
            Tsql = Tsql + " And   ( C_Number2 <> ''  Or C_Number2 is null )";
            //Tsql = Tsql + " And   Recordid Not in ('web','mobile')  "; 


            if (SC_index > 0)
                Tsql = Tsql + " And tbl_Sales_Cacu.C_index =" + SC_index;

            DataSet ds2 = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds2) == false) return 0;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    C_index = int.Parse(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_index"].ToString());
                    int Send_Amount = int.Parse(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Price1"].ToString());
                    C_Number3 = ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Number3"].ToString().Trim();
                    C_Number2 = ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Number2"].ToString().Trim();
                    
                    //Card_Per = ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Period1"].ToString().Substring (2,2) + ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Period2"].ToString() ; 
                    CardNo = encrypter.Decrypt(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Number1"].ToString().Trim()).Replace("-", "");

                    C_C_Sum_Price1 = int.Parse(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_C_Sum_Price1"].ToString());
                    C_Price1 = int.Parse(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Price1"].ToString());
                    string Recordid = ds2.Tables["t_P_table"].Rows[fi_cnt]["Recordid"].ToString().Trim();
                    string Nice_Mid = ds2.Tables["t_P_table"].Rows[fi_cnt]["Nice_Mid"].ToString().Trim();  
                    //StrSql = StrSql + ",'" + encrypter.Encrypt( Sales_Cacu[C_index].C_Number2) + "'";


                    T_rStatus = "";   // 상태 O : 승인, X : 거절
                    T_rAuthNo = "";   // 승인번호 or 거절시 오류코드



                    if (C_C_Sum_Price1 > 0)  //카드 관련 부분 취소한 내역이 한건도 없으면 전체 취소로 가구 그렇지 않으면. 부분 취소로 가게 한다.
                    {
                        int C_C_Price1 = C_Price1; //남은 결제 금액이 마지막 부분 취소할 금액이다.

                        //C3번일 경우 부분 취소임
                        StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card_3 " + C_index + ",'" + OrderNumber + "','" + CardNo + "','" + Card_Per 
                                        + "','C3','" + C_Number3 + "','" + cls_User.gid + "'," + C_C_Price1;


                        DataSet ds = new DataSet();
                        Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

                        Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

                        if (Seq_No > 0)
                        {

                            //if (C_Number3.Length == 12)  //기존 ksnet 관련해서는 거래번호가 12자리 였으므로 12자리일경우에는 ksnet 으로가서 취소하게 한다.
                            //{
                            //    C_Number4 = ksp.KSPayCancelPost("1", C_Number3, ref T_rStatus, ref T_rAuthNo, C_C_Price1, C_Number2);

                            //    if (C_Number4 != "" && T_rStatus != "X")
                            //        C_Number4 = C_Number4;
                            //    else
                            //        C_Number4 = ksp.KSPayCancelPost("2", C_Number3, ref T_rStatus, ref T_rAuthNo, C_C_Price1, C_Number2);
                            //}
                            //else
                            //{

                                string requireType = "1000"; //남은 금액하고 취소 금액이 동일하다 그럼 나머지 전체 취소 를 타라.                                
                                string T_ord = OrderNumber;
                                string NNDttt = ds2.Tables["t_P_table"].Rows[fi_cnt]["NNDttt"].ToString().ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                                //T_ord = T_ord + NNDttt.Substring(NNDttt.Length - 4, 4);
                                string Cancel_ID = "CS";
                                if (Recordid == "web" )  Cancel_ID = "W";
                                if (Recordid == "mobile" )  Cancel_ID = "M";
                                if (Recordid == "Danmal") Cancel_ID = "DA";

                                //string Cancel_Code = Nice_cancelPwd;
                                string Cancel_Code = Etc_Card_Sugi_cancelPwd ; 

                                if (Hana_Card_Sugi_Mid == Nice_Mid)
                                    Cancel_Code = Hana_Card_Sugi_cancelPwd; 
//
                                C_Number4 = ksp_Ga.KSPayCancelPost("2", C_Number3, T_ord, NNDttt, requireType, ref T_rStatus
                                                , ref T_rAuthNo, C_C_Price1, C_Number2, Cancel_ID, Nice_Mid, Cancel_Code
                                                );
                            //}

                            if (C_Number4 != "N" && T_rStatus == "2001")                                
                            {
                                StrSql = "Update tbl_Sales_Cacu SET ";
                                //StrSql = StrSql + "  C_Number3 = ''";  //ksnet 거래번호                                                                                   
                                StrSql = StrSql + " C_Price1 = C_Price1 -  " + C_C_Price1;
                                StrSql = StrSql + " ,C_C_Sum_Price1 = C_C_Sum_Price1 +  " + C_C_Price1; //취소한 금액을 합산한다. 부분 취소가 이루어졌다는 걸 표시하기 위함.
                                StrSql = StrSql + " ,C_C_Price1 =  " + C_C_Price1;

                                StrSql = StrSql + " ,C_Number3 = ''";  //ksnet 거래번호
                                StrSql = StrSql + " ,C_Number4 = '" + C_Number4 + "'";  //ksnet 거래번호

                                StrSql = StrSql + " ,C_CancelTF = 1 ";
                                StrSql = StrSql + " ,C_CancelDate = Convert(Varchar(25),GetDate(),21) ";
                                StrSql = StrSql + " ,C_CancelPrice = C_Price1 ";

                                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                                StrSql = StrSql + " And   C_index = " + C_index;

                                Temp_Connect.Update_Data(StrSql, "", "", 1);
                            }
                            else
                                SW++;
                            //T_rHCashTransactionNo = T_rAuthNo; ;

                            StrSql = "Update tbl_Sales_Cacu_Card SET ";
                            StrSql = StrSql + " rStatus = '" + T_rStatus + "'";
                            StrSql = StrSql + " ,rAuthNo = '" + T_rAuthNo + "'";
                            StrSql = StrSql + " ,rTransactionNo = '" + C_Number4 + "'";
                            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
                            StrSql = StrSql + " Where Seqno  =" + Seq_No;

                            Temp_Connect.Update_Data(StrSql, "", "", 1);

                        }
                    }
                    else
                    {
                        StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_index + ",'" + OrderNumber + "','" + CardNo + "','" + Card_Per + "','C','" + C_Number3 + "','" + cls_User.gid + "'";


                        DataSet ds = new DataSet();
                        Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

                        Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

                        if (Seq_No > 0)
                        {

                            //if (C_Number3.Length == 12)  //기존 ksnet 관련해서는 거래번호가 12자리 였으므로 12자리일경우에는 ksnet 으로가서 취소하게 한다.
                            //{

                            //    C_Number4 = ksp.KSPayCancelPost("1", C_Number3, ref T_rStatus, ref T_rAuthNo, "", CardNo);

                            //    if (C_Number4 != "" && T_rStatus != "X")
                            //        C_Number4 = C_Number4;
                            //    else
                            //        C_Number4 = ksp.KSPayCancelPost("2", C_Number3, ref T_rStatus, ref T_rAuthNo, "", CardNo);
                            //}
                            //else
                            //{

                                string T_ord = OrderNumber;
                                string NNDttt = ds2.Tables["t_P_table"].Rows[fi_cnt]["NNDttt"].ToString().ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                                //T_ord = T_ord + NNDttt.Substring(NNDttt.Length - 4, 4);
                                string Cancel_ID = "CS";
                                //if (Recordid == "web") Cancel_ID = "W";
                                //if (Recordid == "mobile") Cancel_ID = "M";
                                //if (Recordid == "Danmal") Cancel_ID = "DA";

                                //string Cancel_Code = Nice_cancelPwd;
                                string Cancel_Code = Etc_Card_Sugi_cancelPwd ; 

                                if (Hana_Card_Sugi_Mid == Nice_Mid)
                                    Cancel_Code = Hana_Card_Sugi_cancelPwd; 

                                C_Number4 = ksp_Ga.KSPayCancelPost("2", C_Number3, ref T_rStatus, ref T_rAuthNo, C_Price1, C_Number2
                                                            , T_ord, NNDttt, C_Price1.ToString(), Cancel_ID, Nice_Mid, Cancel_Code);
                            //}

                            if (C_Number4 != "N" && T_rStatus == "2001" )                                
                            {
                                StrSql = "Update tbl_Sales_Cacu SET ";
                                StrSql = StrSql + "  C_Number3 = ''";  //ksnet 거래번호
                                StrSql = StrSql + " ,C_Number4 = '" + C_Number4 + "'";  //ksnet 거래번호

                                StrSql = StrSql + " ,C_CancelTF = 1 ";
                                StrSql = StrSql + " ,C_CancelDate = Convert(Varchar(25),GetDate(),21) ";
                                StrSql = StrSql + " ,C_CancelPrice = C_Price1 ";
                                

                                if (Cancel_Sort == "")
                                    StrSql = StrSql + " ,C_Price1 = 0 ";

                                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                                StrSql = StrSql + " And   C_index = " + C_index;

                                Temp_Connect.Update_Data(StrSql, "", "", 1);
                            }
                            else
                                SW++;
                            //T_rHCashTransactionNo = T_rAuthNo; ;


                            StrSql = "Update tbl_Sales_Cacu_Card SET ";
                            StrSql = StrSql + " rStatus = '" + T_rStatus + "'";
                            StrSql = StrSql + " ,rAuthNo = '" + T_rAuthNo + "'";
                            StrSql = StrSql + " ,rTransactionNo = '" + C_Number4 + "'";
                            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
                            StrSql = StrSql + " Where Seqno  =" + Seq_No;

                            Temp_Connect.Update_Data(StrSql, "", "", 1);
                        }                        
                    }


                    
                    
                }
            }

            if (SW == 0)
            {
                if (Card_No_Null_TF > 0)
                    return -100;
                else
                    return 0;
            }
            else
                return 100;
        }



        private int Card_Detail_Send_Singo_Cancel(string OrderNumber, int SC_index = 0, string Cancel_Sort = "")
        {
            int SW = 0;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            KSPayApprovalCancelBean_2cs ksp = new KSPayApprovalCancelBean_2cs();
            GalaxiaApi_Pay_cs ksp_Ga = new GalaxiaApi_Pay_cs();


            string StrSql = "";
            int C_index = 0, Seq_No = 0, Card_No_Null_TF = 0, C_C_Price1 = 0, C_Price1 = 0;
            string C_Number4 = "", T_rAuthNo = "", CardNo = "", Card_Per = "", Cancel_Count = "";

            string T_rStatus = "";   // 상태 O : 승인, X : 거절                        
            string C_Number3 = "", C_Number2 = "";   // 거래번호      //저장해야함 승인취소시 사용됨.

            string Tsql = "Select C_index , C_Price1 ,C_Number1,C_Number3 , C_Number2 ,C_Period1,C_Period2 , C_C_Price1 ,  C_C_Sum_Price1, Recordid, Nice_Mid  ";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "'";
            Tsql = Tsql + " And   C_TF   = 3 ";
            Tsql = Tsql + " And   C_Number3 <> '' ";
            Tsql = Tsql + " And   C_C_Price1 > 0 ";
            Tsql = Tsql + " And   ( C_Number2 <> ''  Or C_Number2 is null )";

            Tsql = Tsql + " And   Recordid Not in ('web','mobile')  "; 

            if (SC_index > 0)
                Tsql = Tsql + " And tbl_Sales_Cacu.C_index =" + SC_index;

            DataSet ds2 = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds2) == false) return 0;
            int ReCnt = Temp_Connect.DataSet_ReCount;


            Tsql = " Select COUNT(1) + 1 From tbl_Sales_Cacu_Card (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "' ";
            Tsql = Tsql + " And C_index = " + SC_index;
            Tsql = Tsql + " And CanCel_Flag = 'C3' And rStatus = 'O' ";

            DataSet ds3 = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_Cacu_Count", ds3) == false) return 0;

            Cancel_Count = ds3.Tables["t_Cacu_Count"].Rows[0][0].ToString(); 

            if (ReCnt > 0)
            {

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    C_index = int.Parse(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_index"].ToString());
                    int Send_Amount = int.Parse(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Price1"].ToString());
                    C_Number3 = ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Number3"].ToString().Trim();
                    C_Number2 = ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Number2"].ToString().Trim();
                    string Recordid = ds2.Tables["t_P_table"].Rows[fi_cnt]["Recordid"].ToString().Trim();
                    C_C_Price1 = int.Parse(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_C_Price1"].ToString());
                    C_Price1 = int.Parse(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Price1"].ToString());

                    //Card_Per = ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Period1"].ToString().Substring (2,2) + ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Period2"].ToString() ; 
                    CardNo = encrypter.Decrypt(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Number1"].ToString().Trim()).Replace("-", "");
                    string Nice_Mid =  ds2.Tables["t_P_table"].Rows[fi_cnt]["Nice_Mid"].ToString().Trim();
                    //StrSql = StrSql + ",'" + encrypter.Encrypt( Sales_Cacu[C_index].C_Number2) + "'";


                    T_rStatus = "";   // 상태 O : 승인, X : 거절
                    T_rAuthNo = "";   // 승인번호 or 거절시 오류코드

                    //if (CardNo == "")
                    //    Card_No_Null_TF++;
                    //else
                    //{
                    //C3번일 경우 부분 취소임
                    StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card_3 " + C_index + ",'" + OrderNumber + "','" + CardNo + "','" + Card_Per + "','C3','" + C_Number3 + "','" + cls_User.gid + "'," + C_C_Price1 ;


                    DataSet ds = new DataSet();
                    Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

                    Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

                    if (Seq_No > 0)
                    {

                        //if (C_Number3.Length == 12)  //기존 ksnet 관련해서는 거래번호가 12자리 였으므로 12자리일경우에는 ksnet 으로가서 취소하게 한다.
                        //{
                        //    //C_Number4 = ksp.KSPayCancelPost("1", C_Number3, ref T_rStatus, ref T_rAuthNo, C_C_Price1, C_Number2);
                        //    C_Number4 = ksp.KSPayCancelPost("1", C_Number3, ref T_rStatus, ref T_rAuthNo, C_C_Price1, Cancel_Count);

                        //    if (C_Number4 != "" && T_rStatus != "X")
                        //        C_Number4 = C_Number4;
                        //    else
                        //        C_Number4 = ksp.KSPayCancelPost("2", C_Number3, ref T_rStatus, ref T_rAuthNo, C_C_Price1, Cancel_Count);
                        //    //C_Number4 = ksp.KSPayCancelPost("2", C_Number3, ref T_rStatus, ref T_rAuthNo, C_C_Price1, C_Number2);
                        //}
                        //else
                        //{
                            string requireType = "0000";
                            if (C_C_Price1 == C_Price1) requireType = "1000";  //남은 금액하고 취소 금액이 동일하다 그럼 나머지 전체 취소 를 타라.
                            string T_ord = OrderNumber;
                            string NNDttt = ds.Tables["Cacu_Card"].Rows[0][1].ToString().ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                            T_ord = T_ord;// +NNDttt.Substring(NNDttt.Length - 4, 4);

                           //카드부분 취소인데 우선은 막음
                            string Cancel_Code = Nice_cancelPwd;    
                            //string Cancel_Code = Bank_cancelPwd ; 

                            string Cancel_ID = "CS";
                            if (Recordid == "web") Cancel_ID = "W";
                            if (Recordid == "mobile") Cancel_ID = "M";
                            if (Recordid == "Danmal") Cancel_ID = "DA";
                            C_Number4 = ksp_Ga.KSPayCancelPost("2", C_Number3, T_ord, NNDttt, requireType, ref T_rStatus, ref T_rAuthNo, C_C_Price1, Cancel_Count, Cancel_ID,
                                    Nice_Mid, Nice_cancelPwd );
                        //}


                        if (( C_Number4 != "" && T_rStatus != "X") || (C_Number3.Length != 12 && C_Number4 != "" && T_rStatus == "0000"))
                        {
                            StrSql = "Update tbl_Sales_Cacu SET ";
                            //StrSql = StrSql + "  C_Number3 = ''";  //ksnet 거래번호                                                                                   
                            StrSql = StrSql + " C_Price1 = C_Price1 -  " + C_C_Price1;
                            StrSql = StrSql + " ,C_C_Sum_Price1 = C_C_Sum_Price1 +  " + C_C_Price1; //취소한 금액을 합산한다. 부분 취소가 이루어졌다는 걸 표시하기 위함.
                            
                            StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                            StrSql = StrSql + " And   C_index = " + C_index;

                            Temp_Connect.Update_Data(StrSql, "", "", 1);
                        }
                        else
                            SW++;
                        //T_rHCashTransactionNo = T_rAuthNo; ;

                        StrSql = "Update tbl_Sales_Cacu_Card SET ";
                        StrSql = StrSql + " rStatus = '" + T_rStatus + "'";
                        StrSql = StrSql + " ,rAuthNo = '" + T_rAuthNo + "'";
                        StrSql = StrSql + " ,rTransactionNo = '" + C_Number4 + "'";
                        StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
                        StrSql = StrSql + " Where Seqno  =" + Seq_No;

                        Temp_Connect.Update_Data(StrSql, "", "", 1);

                    }
                    //}
                }
            }

            if (SW == 0)
            {
                if (Card_No_Null_TF > 0)
                    return -100;
                else
                    return 0;
            }
            else
                return 100;
        }





        private int Bank_Send_Singo_Cancel(string OrderNumber, int SC_index = 0, string Cancel_Sort = "")
        {
            int SW = 0;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            KSPayApprovalCancelBean_2cs ksp = new KSPayApprovalCancelBean_2cs();

            string StrSql = "";
            int C_index = 0, Seq_No = 0; ;
            string C_Number4 = "", T_rAuthNo = "", CardNo = "", Card_Per = "";

            string T_rStatus = "";   // 상태 O : 승인, X : 거절                        
            string C_Number3 = "";   // 거래번호      //저장해야함 승인취소시 사용됨.

            string Tsql = "Select C_index , C_Price1 ,C_Number1,C_Number3 ,C_Period1,C_Period2 , C_Cul_FLAG , Nice_Mid , C_Price2 ";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "'";
            Tsql = Tsql + " And   C_TF   = 5 ";
            Tsql = Tsql + " And   C_Number3 <> '' ";
            //Tsql = Tsql + " And   len(C_Number3) = 12   "; //가상계좌 거래 번호가 12자리인것만 취소 되게 처리를 해라.
            
            //Tsql = Tsql + " And   C_Cul_FLAG = 'N'  ";
            Tsql = Tsql + " And   C_Price1 = 0  ";
            Tsql = Tsql + " And   C_Number4 = ''  ";
            
            if (SC_index > 0)
                Tsql = Tsql + " And tbl_Sales_Cacu.C_index =" + SC_index;

            DataSet ds2 = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds2) == false) return 0;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)
            {

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    C_index = int.Parse(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_index"].ToString());
                    int C_Price1 = int.Parse(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Price1"].ToString());
                    int Send_Amount = int.Parse(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Price2"].ToString());
                    C_Number3 = ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Number3"].ToString().Trim();
                    string C_Cul_FLAG = ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Cul_FLAG"].ToString().Trim();
                    string Nice_Mid = ds2.Tables["t_P_table"].Rows[fi_cnt]["Nice_Mid"].ToString().Trim();

                    Card_Per = "가상계좌";
                    CardNo = encrypter.Encrypt(ds2.Tables["t_P_table"].Rows[fi_cnt]["C_Number1"].ToString()).Trim().Replace("-", "");

                    //StrSql = StrSql + ",'" + encrypter.Encrypt( Sales_Cacu[C_index].C_Number2) + "'";


                    T_rStatus = "";   // 상태 O : 승인, X : 거절
                    T_rAuthNo = "";   // 승인번호 or 거절시 오류코드


                    StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_index + ",'" + OrderNumber + "','" + CardNo + "','" + Card_Per + "','C','" + C_Number3 + "','" + cls_User.gid + "'";


                    DataSet ds = new DataSet();
                    Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

                    Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

                    if (Seq_No > 0)
                    {
                        if (C_Price1 == 0 && C_Cul_FLAG == "N" && Send_Amount > 0 )
                        {

                            string T_Er_Msg = "";

                            //string Cancel_Code = Nice_cancelPwd;
                            string Cancel_Code = Bank_cancelPwd ; 

                            C_Number4 = ksp.Check_Nice_Same_BankAccount_Cancel_Web(OrderNumber, C_index, CardNo, C_Number3, ref T_rAuthNo, ref T_Er_Msg
                                , Nice_Mid, Cancel_Code, Send_Amount);


                            if (C_Number4 != "" && C_Number4 != "N" && C_Number4 == "2001")
                            {
                                StrSql = "Update tbl_Sales_Cacu SET ";
                                StrSql = StrSql + " C_Number3 = ''";  //ksnet 거래번호
                                StrSql = StrSql + ", C_Number4 = '" + C_Number4 + "'";  //ksnet 거래번호

                                StrSql = StrSql + " ,C_CancelTF = 1 ";
                                StrSql = StrSql + " ,C_CancelDate = Convert(Varchar(25),GetDate(),21) ";

                                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                                StrSql = StrSql + " And   C_index = " + C_index;

                                Temp_Connect.Update_Data(StrSql, "", "", 1);


                                StrSql = "Update tbl_Sales_Cacu_ACC SET ";
                                StrSql = StrSql + " Cul_Send_TF = 1";
                                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                                StrSql = StrSql + " And   Bank_ACC_Account = '" + CardNo + "'";

                                Temp_Connect.Update_Data(StrSql, "", "", 1);


                            }
                            else
                                SW++;
                        }
                        else
                        {
                            StrSql = "Update tbl_Sales_Cacu SET ";
                            //StrSql = StrSql + " C_Price1 = 0 ";  //ksnet 거래번호
                            StrSql = StrSql + " C_CancelTF = 1 ";
                            StrSql = StrSql + " ,C_CancelDate = Convert(Varchar(25),GetDate(),21) ";

                            StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                            StrSql = StrSql + " And   C_index = " + C_index;

                            Temp_Connect.Update_Data(StrSql, "", "", 1);
                        }
                        //T_rHCashTransactionNo = T_rAuthNo; ;

                        StrSql = "Update tbl_Sales_Cacu_Card SET ";
                        StrSql = StrSql + " rStatus = '" + T_rStatus + "'";
                        StrSql = StrSql + " ,rAuthNo = '" + T_rAuthNo + "'";
                        StrSql = StrSql + " ,rTransactionNo = '" + C_Number4 + "'";
                        StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
                        StrSql = StrSql + " Where Seqno  =" + Seq_No;

                        Temp_Connect.Update_Data(StrSql, "", "", 1);

                    }
                }
            }

            if (SW == 0)
                return 0;
            else
                return 100;
        }



        public int Cash_Card_Send_Singo_OK(string OrderNumber, string T_Mbid, string C_Sort, int C_index = 0, string M_Name = "")
        {
            int Ret = 0;
            if (cls_User.gid == cls_User.SuperUserID)
            {
                if (C_Sort == "Cash")
                {
                    Ret = DB_Save_Card_App(OrderNumber);
                }
                else if (C_Sort == "Card")
                {

                    Ret = DB_Save_Card_App(OrderNumber, C_index);
                }

                

                else if (C_Sort == "Bank")
                {

                    Ret = DB_Save_Bank_App(OrderNumber, C_index);
                }

                return Ret;
            }
            else
            {
                try
                {
                    if (C_Sort == "Cash")
                    {
                        Ret = DB_Save_Card_App(OrderNumber);
                    }
                    else if (C_Sort == "Card")
                    {
                        Ret = DB_Save_Card_App(OrderNumber, C_index);

                    }
                    else if (C_Sort == "Bank")
                    {
                        Ret = DB_Save_Bank_App(OrderNumber, C_index);

                    }

                    return Ret;
                }

                catch (Exception)
                {
                    return 1;
                }
            }
            //return 0;
        }


        public int Cash_Card_Send_Singo_OK_Auto(string OrderNumber, string T_Mbid, string C_Sort, int C_index = 0, string M_Name = "")
        {
            int Ret = 0;
            if (cls_User.gid == cls_User.SuperUserID)
            {
                if (C_Sort == "Cash")
                {
                    Ret = DB_Save_Card_App(OrderNumber);
                }
                else if (C_Sort == "Card")
                {

                    Ret = DB_Save_Card_App_Auto(OrderNumber, C_index);
                }



                else if (C_Sort == "Bank")
                {

                    Ret = DB_Save_Bank_App(OrderNumber, C_index);
                }

                return Ret;
            }
            else
            {
                try
                {
                    if (C_Sort == "Cash")
                    {
                        Ret = DB_Save_Card_App(OrderNumber);
                    }
                    else if (C_Sort == "Card")
                    {
                        Ret = DB_Save_Card_App(OrderNumber, C_index);

                    }
                    else if (C_Sort == "Bank")
                    {
                        Ret = DB_Save_Bank_App(OrderNumber, C_index);

                    }

                    return Ret;
                }

                catch (Exception)
                {
                    return 1;
                }
            }
            //return 0;
        }


        private int DB_Save_Card_App(string OrderNumber, int SC_index = 0)
        {
            int SW = 0;
            string C_P_Number = "", C_B_Number = "";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            KSPayApprovalCancelBean_2cs ksp = new KSPayApprovalCancelBean_2cs();
            GalaxiaApi_Pay_cs ksp_gal = new GalaxiaApi_Pay_cs();


            //C_Number3   거래번호
            //C_Number4   Mid

            string StrSql = " Select '' ";
            StrSql = StrSql + ", tbl_SalesDetail.OrderNumber,C_index  ";
            StrSql = StrSql + ", LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2) ";
            StrSql = StrSql + ", tbl_SalesDetail.M_name";
            StrSql = StrSql + ", C_Price1";
            StrSql = StrSql + ", LEFT(C_AppDate1,4) +'-' + LEFT(RIGHT(C_AppDate1,4),2) + '-' + RIGHT(C_AppDate1,2)";

            StrSql = StrSql + ", tbl_Card.cardname , C_Number1 , C_Number2 , C_P_Number  , C_B_Number ";
            StrSql = StrSql + ", C_Installment_Period , C_Name1 , C_Price2 ,C_Etc ";
            StrSql = StrSql + ", Case When C_Period1 <>'' And C_Period2 <> '' then   Right (C_Period1,2 ) + C_Period2 ELSE '' End  AS Card_Per ";

            StrSql = StrSql + ", tbl_SalesDetail.SellDate ";
            StrSql = StrSql + ", tbl_SalesDetail.Mbid2 ";
            StrSql = StrSql + ", (Select Top 1 ItemCode From tbl_SalesitemDetail (nolock) Where tbl_SalesitemDetail.Ordernumber = tbl_SalesDetail.Ordernumber Order by Salesitemindex ASC  ) AS ItemCode_1 ";
            StrSql = StrSql + ", tbl_Sales_Cacu.RecordTime ";
            StrSql = StrSql + ", tbl_Sales_Cacu.Je_Card_FLAG ";
            

            StrSql = StrSql + " From tbl_Sales_Cacu (nolock) ";
            StrSql = StrSql + " LEFT Join tbl_SalesDetail  (nolock) ON  tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber  ";
            StrSql = StrSql + " LEFT JOIN tbl_Card  (nolock) ON tbl_Card.Ncode =tbl_Sales_Cacu.C_Code And tbl_Card.Na_code =tbl_SalesDetail.Na_Code  ";

            StrSql = StrSql + " Where tbl_SalesDetail.OrderNumber = '" + OrderNumber + "'";
            StrSql = StrSql + " And tbl_Sales_Cacu.C_TF = 3 ";
            StrSql = StrSql + " And tbl_Sales_Cacu.Sugi_TF = '1' ";
            StrSql = StrSql + " And tbl_Sales_Cacu.C_Number3 = '' ";
            StrSql = StrSql + " And tbl_Sales_Cacu.C_Price1 > 0 ";
            StrSql = StrSql + " And tbl_Sales_Cacu.C_Price2 > 0 ";
            StrSql = StrSql + " And C_P_Number <> '' ";
            StrSql = StrSql + " And C_B_Number <> '' ";

            if (SC_index > 0)
                StrSql = StrSql + " And tbl_Sales_Cacu.C_index =" + SC_index;

            DataSet ds2 = new DataSet();
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds2);

            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return 0;

            int TotalCnt = 0, AppCnt = 0, ErrCnt = 0;
            string ErrCardNum = "";
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                int C_index = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_index"].ToString());
                string U_Name = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["M_name"].ToString();

                string Mbid2 = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["Mbid2"].ToString();

                string CardNo = encrypter.Decrypt(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Number1"].ToString()).Replace("-", "");
                string Card_Per = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["Card_Per"].ToString();
                string SellDate = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["SellDate"].ToString();
                
                string HalBu = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Installment_Period"].ToString();
                string Je_Card_FLAG = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["Je_Card_FLAG"].ToString(); 


                C_P_Number = encrypter.Decrypt(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_P_Number"].ToString());
                C_B_Number = encrypter.Decrypt(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_B_Number"].ToString());

                if (HalBu == "일시불" || HalBu == "")
                    HalBu = "00";
                else
                    HalBu = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Installment_Period"].ToString()).ToString();

                int Send_Amount = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Price2"].ToString());  //승인금액

                string Card_Com_Code = "";
                string Card_CC = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["cardname"].ToString();

                string ORDER_DATE = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["RecordTime"].ToString().Replace("-", "").Replace(":", "").Replace(" ", "");

                //if (Card_CC.Length >= 2)
                //{
                //    if (Card_CC.Substring(0, 2) == "국민") Card_Com_Code = "0050";
                //    if (Card_CC.Substring(0, 2) == "하나") Card_Com_Code = "0051";
                //    if (Card_CC.Substring(0, 2) == "비씨") Card_Com_Code = "0052";
                //    if (Card_CC.Substring(0, 2) == "신한") Card_Com_Code = "0053";
                //    if (Card_CC.Substring(0, 2) == "삼성") Card_Com_Code = "0054";
                //    if (Card_CC.Substring(0, 2) == "롯데") Card_Com_Code = "0055";
                //    if (Card_CC.Substring(0, 2) == "현대") Card_Com_Code = "0073";
                //    if (Card_CC.Substring(0, 2) == "기타") Card_Com_Code = "9999";
                //}

                //if (Card_Com_Code == "") Card_Com_Code = "9999";

                string ItemCode = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["ItemCode_1"].ToString();

                string T_currencytype = "0"; //  0이면 한화   1이면 달라다
                string T_rStatus = "";   // 상태 O : 승인, X : 거절
                string T_rAuthNo = "";   // 승인번호 or 거절시 오류코드
                string C_Number3 = "";   // 거래번호      //저장해야함 승인취소시 사용됨.
                string T_Er_Msg = "";

                StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_index + ",'" + OrderNumber + "','" + CardNo + "','" + Card_Per + "','A' , '''','" + cls_User.gid + "'";

                DataSet ds = new DataSet();
                Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

                int Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

                
                if (Seq_No > 0)
                {
                    int Card_Ap_Flag = 0; 

                    if (CardNo != "" && Card_Per != "" && Send_Amount > 0)
                    {
                        
                        // string NNDttt = ds.Tables["Cacu_Card"].Rows[0][1].ToString().ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                        string T_ord = OrderNumber; //+ NNDttt.Substring(NNDttt.Length - 4, 4);

                        if (Je_Card_FLAG == "")
                        {

                            C_Number3 = ksp_gal.KSPayCreditPostMNI(T_ord, U_Name, "", "1"
                                                            , CardNo, Card_Per, HalBu, Send_Amount, C_P_Number, C_B_Number
                                                            , SellDate, Mbid2, ItemCode, Card_Com_Code, ORDER_DATE
                                                            , Etc_Card_Sugi_Mid, Etc_Card_Sugi_Key
                                                            , ref T_rStatus, ref T_rAuthNo, ref T_Er_Msg, Je_Card_FLAG);
                        }
                        else
                        {
                            C_Number3 = ksp_gal.KSPayCreditPostMNI(T_ord, U_Name, "", "1"
                                                            , CardNo, Card_Per, HalBu, Send_Amount, C_P_Number, C_B_Number
                                                            , SellDate, Mbid2, ItemCode, Card_Com_Code, ORDER_DATE
                                                            , Hana_Card_Sugi_Mid, Hana_Card_Sugi_Key
                                                            , ref T_rStatus, ref T_rAuthNo, ref T_Er_Msg, Je_Card_FLAG);
                        }
                        if (C_Number3 != "" && (T_rStatus == "3001")) Card_Ap_Flag = 1;
                        
                    }

                    if (Card_Ap_Flag == 1 )                    
                    {
                        StrSql = "Update tbl_Sales_Cacu SET ";
                        StrSql = StrSql + " C_Number3  = '" + C_Number3 + "'";  //거래번호
                        StrSql = StrSql + " ,C_Number2 = '" + T_rAuthNo + "'"; //승인번호                        
                        StrSql = StrSql + " ,C_Number4 = ''"; 
                        StrSql = StrSql + " ,Nice_Mid = '" + Etc_Card_Sugi_Mid  + "'"; //Mid
                        
                        StrSql = StrSql + " ,Sugi_TF = '2' ";  //승인이 제대로 이루어 졋다. 2번으로 넣는다.

                        StrSql = StrSql + " ,C_CancelTF = 0 ";
                        StrSql = StrSql + " ,C_CancelDate = '' ";
                        StrSql = StrSql + " ,C_CancelPrice = 0 ";

                        StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                        StrSql = StrSql + " And   C_index = " + C_index;

                        Temp_Connect.Update_Data(StrSql);
                        AppCnt++;
                    }
                    else
                    {
                        StrSql = "Update tbl_Sales_Cacu SET ";
                        StrSql = StrSql + " C_Price1  = 0 ";
                        StrSql = StrSql + " , C_Etc ='" + T_Er_Msg + "'";  //승인 오류시 비고칸에 내역을 넣도록 한다.
                        StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                        StrSql = StrSql + " And   C_index = " + C_index;

                        Temp_Connect.Update_Data(StrSql);

                        ErrCnt++;
                        ErrCardNum = ErrCardNum + " / " + CardNo;
                        MessageBox.Show(CardNo + " 카드 승인시 오류 : " + T_Er_Msg);
                    }

                    StrSql = "Update tbl_Sales_Cacu_Card SET ";
                    StrSql = StrSql + " rStatus = '" + T_rStatus + "'";
                    StrSql = StrSql + " ,rAuthNo = '" + T_rAuthNo + "'";
                    StrSql = StrSql + " ,rTransactionNo = '" + C_Number3 + "'";
                    StrSql = StrSql + " ,C_Number3 = '" + C_Number3 + "'";
                    StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
                    StrSql = StrSql + " Where Seqno  =" + Seq_No;

                    Temp_Connect.Update_Data(StrSql, "", "", 1);

                }

                TotalCnt++;
            }

            string Send_M = "총 요청건 : " + TotalCnt + "  정상승인건 : " + AppCnt + " 오류건 : " + ErrCnt
                             + "\n" +
                             "오류카드번호:" + ErrCardNum + " 입니다."
                             ;
            MessageBox.Show(Send_M);

            if (ErrCnt > 0)
                return 100;
            else
                return 0;

        }





        private int DB_Save_Card_App_Auto(string OrderNumber, int SC_index = 0)
        {
            int SW = 0;
            string C_P_Number = "", C_B_Number = "";

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            KSPayApprovalCancelBean_2cs ksp = new KSPayApprovalCancelBean_2cs();
            GalaxiaApi_Pay_cs ksp_gal = new GalaxiaApi_Pay_cs();


            //C_Number3   거래번호
            //C_Number4   Mid

            string StrSql = " Select '' ";
            StrSql = StrSql + ", tbl_SalesDetail.OrderNumber,C_index  ";
            StrSql = StrSql + ", LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2) ";
            StrSql = StrSql + ", tbl_SalesDetail.M_name";
            StrSql = StrSql + ", C_Price1";
            StrSql = StrSql + ", LEFT(C_AppDate1,4) +'-' + LEFT(RIGHT(C_AppDate1,4),2) + '-' + RIGHT(C_AppDate1,2)";

            StrSql = StrSql + ", tbl_Card.cardname , C_Number1 , C_Number2 , C_P_Number  , C_B_Number ";
            StrSql = StrSql + ", C_Installment_Period , C_Name1 , C_Price2 ,C_Etc ";
            StrSql = StrSql + ", Case When C_Period1 <>'' And C_Period2 <> '' then   Right (C_Period1,2 ) + C_Period2 ELSE '' End  AS Card_Per ";

            StrSql = StrSql + ", tbl_SalesDetail.SellDate ";
            StrSql = StrSql + ", tbl_SalesDetail.Mbid2 ";
            StrSql = StrSql + ", (Select Top 1 ItemCode From tbl_SalesitemDetail (nolock) Where tbl_SalesitemDetail.Ordernumber = tbl_SalesDetail.Ordernumber Order by Salesitemindex ASC  ) AS ItemCode_1 ";
            StrSql = StrSql + ", tbl_Sales_Cacu.RecordTime ";
            StrSql = StrSql + ", tbl_Sales_Cacu.Je_Card_FLAG ";


            StrSql = StrSql + " From tbl_Sales_Cacu (nolock) ";
            StrSql = StrSql + " LEFT Join tbl_SalesDetail  (nolock) ON  tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber  ";
            StrSql = StrSql + " LEFT JOIN tbl_Card  (nolock) ON tbl_Card.Ncode =tbl_Sales_Cacu.C_Code And tbl_Card.Na_code =tbl_SalesDetail.Na_Code  ";

            StrSql = StrSql + " Where tbl_SalesDetail.OrderNumber = '" + OrderNumber + "'";
            StrSql = StrSql + " And tbl_Sales_Cacu.C_TF = 3 ";
            StrSql = StrSql + " And tbl_Sales_Cacu.Sugi_TF = '1' ";
            StrSql = StrSql + " And tbl_Sales_Cacu.C_Number3 = '' ";
            StrSql = StrSql + " And tbl_Sales_Cacu.C_Price1 > 0 ";
            StrSql = StrSql + " And tbl_Sales_Cacu.C_Price2 > 0 ";
            StrSql = StrSql + " And C_P_Number <> '' ";
            StrSql = StrSql + " And C_B_Number <> '' ";

            

            DataSet ds2 = new DataSet();
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds2);

            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return 0;

            int TotalCnt = 0, AppCnt = 0, ErrCnt = 0;
            string ErrCardNum = "";
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                int C_index = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_index"].ToString());
                string U_Name = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["M_name"].ToString();

                string Mbid2 = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["Mbid2"].ToString();

                string CardNo = encrypter.Decrypt(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Number1"].ToString()).Replace("-", "");
                string Card_Per = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["Card_Per"].ToString();
                string SellDate = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["SellDate"].ToString();

                string HalBu = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Installment_Period"].ToString();
                string Je_Card_FLAG = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["Je_Card_FLAG"].ToString();


                C_P_Number = encrypter.Decrypt(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_P_Number"].ToString());
                C_B_Number = encrypter.Decrypt(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_B_Number"].ToString());

                if (HalBu == "일시불" || HalBu == "")
                    HalBu = "00";
                else
                    HalBu = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Installment_Period"].ToString()).ToString();

                int Send_Amount = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Price2"].ToString());  //승인금액

                string Card_Com_Code = "";
                string Card_CC = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["cardname"].ToString();

                string ORDER_DATE = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["RecordTime"].ToString().Replace("-", "").Replace(":", "").Replace(" ", "");


                string ItemCode = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["ItemCode_1"].ToString();

                string T_currencytype = "0"; //  0이면 한화   1이면 달라다
                string T_rStatus = "";   // 상태 O : 승인, X : 거절
                string T_rAuthNo = "";   // 승인번호 or 거절시 오류코드
                string C_Number3 = "";   // 거래번호      //저장해야함 승인취소시 사용됨.
                string T_Er_Msg = "";

                StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_index + ",'" + OrderNumber + "','" + CardNo + "','" + Card_Per + "','A' , '''','" + cls_User.gid + "'";

                DataSet ds = new DataSet();
                Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

                int Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());


                if (Seq_No > 0)
                {
                    int Card_Ap_Flag = 0;

                    if (CardNo != "" && Card_Per != "" && Send_Amount > 0)
                    {

                        // string NNDttt = ds.Tables["Cacu_Card"].Rows[0][1].ToString().ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                        string T_ord = OrderNumber; //+ NNDttt.Substring(NNDttt.Length - 4, 4);

                        

                        C_Number3 = ksp_gal.KSPayCreditPostMNI(T_ord, U_Name, "", "1"
                                                        , CardNo, Card_Per, HalBu, Send_Amount, C_P_Number, C_B_Number
                                                        , SellDate, Mbid2, ItemCode, Card_Com_Code, ORDER_DATE
                                                        , Etc_Card_Sugi_Mid, Etc_Card_Sugi_Key
                                                        , ref T_rStatus, ref T_rAuthNo, ref T_Er_Msg, Je_Card_FLAG);
                       
                        if (C_Number3 != "" && (T_rStatus == "3001")) Card_Ap_Flag = 1;

                    }

                    if (Card_Ap_Flag == 1)
                    {
                        StrSql = "Update tbl_Sales_Cacu SET ";
                        StrSql = StrSql + " C_Number3  = '" + C_Number3 + "'";  //거래번호
                        StrSql = StrSql + " ,C_Number2 = '" + T_rAuthNo + "'"; //승인번호                        
                        StrSql = StrSql + " ,C_Number4 = ''";
                        StrSql = StrSql + " ,Nice_Mid = '" + Etc_Card_Sugi_Mid + "'"; //Mid

                        StrSql = StrSql + " ,Sugi_TF = '2' ";  //승인이 제대로 이루어 졋다. 2번으로 넣는다.

                        StrSql = StrSql + " ,C_CancelTF = 0 ";
                        StrSql = StrSql + " ,C_CancelDate = '' ";
                        StrSql = StrSql + " ,C_CancelPrice = 0 ";

                        StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                        StrSql = StrSql + " And   C_index = " + C_index;

                        Temp_Connect.Update_Data(StrSql);
                        AppCnt++;
                    }
                    else
                    {
                        StrSql = "Update tbl_Sales_Cacu SET ";
                        StrSql = StrSql + " C_Price1  = 0 ";
                        StrSql = StrSql + " , C_Etc ='" + T_Er_Msg + "'";  //승인 오류시 비고칸에 내역을 넣도록 한다.
                        StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                        StrSql = StrSql + " And   C_index = " + C_index;

                        Temp_Connect.Update_Data(StrSql);

                        ErrCnt++;
                        ErrCardNum = ErrCardNum + " / " + CardNo;
                       // MessageBox.Show(CardNo + " 카드 승인시 오류 : " + T_Er_Msg);
                    }

                    StrSql = "Update tbl_Sales_Cacu_Card SET ";
                    StrSql = StrSql + " rStatus = '" + T_rStatus + "'";
                    StrSql = StrSql + " ,rAuthNo = '" + T_rAuthNo + "'";
                    StrSql = StrSql + " ,rTransactionNo = '" + C_Number3 + "'";
                    StrSql = StrSql + " ,C_Number3 = '" + C_Number3 + "'";
                    StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
                    StrSql = StrSql + " Where Seqno  =" + Seq_No;

                    Temp_Connect.Update_Data(StrSql, "", "", 1);

                }

                TotalCnt++;
            }

            string Send_M = "총 요청건 : " + TotalCnt + "  정상승인건 : " + AppCnt + " 오류건 : " + ErrCnt
                             + "\n" +
                             "오류카드번호:" + ErrCardNum + " 입니다."
                             ;
            //MessageBox.Show(Send_M);

            if (ErrCnt > 0)
                return 100;
            else
                return 0;

        }






        private int DB_Save_Card_App(string OrderNumber, string M_Name, int SC_index = 0)
        {
            int SW = 0;
            string C_P_Number = "", C_B_Number = "";
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            KSPayApprovalCancelBean_2cs ksp = new KSPayApprovalCancelBean_2cs();

            string StrSql = " Select '' ";
            StrSql = StrSql + ", C_index  ";
            StrSql = StrSql + ", C_Price1";
            StrSql = StrSql + ", LEFT(C_AppDate1,4) +'-' + LEFT(RIGHT(C_AppDate1,4),2) + '-' + RIGHT(C_AppDate1,2)";

            StrSql = StrSql + ", '' , C_Number1 , C_Number2  , C_P_Number  , C_B_Number ";
            StrSql = StrSql + ", C_Installment_Period , C_Name1 , C_Price2 ,C_Etc ";
            StrSql = StrSql + ", Case When C_Period1 <>'' And C_Period2 <> '' then   Right (C_Period1,2 ) + C_Period2 ELSE '' End  AS Card_Per ";

            StrSql = StrSql + " From tbl_Sales_Cacu_Temp (nolock) ";

            StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
            StrSql = StrSql + " And C_TF = 3 ";
            StrSql = StrSql + " And Sugi_TF = '1' ";
            StrSql = StrSql + " And C_Number3 = '' ";
            StrSql = StrSql + " And C_Price1 > 0 ";
            StrSql = StrSql + " And C_Price2 > 0 ";
            StrSql = StrSql + " And C_P_Number <> '' ";
            StrSql = StrSql + " And C_B_Number <> '' ";

            StrSql = StrSql + " And C_index =" + SC_index;

            DataSet ds2 = new DataSet();
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds2);

            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return 0;

            int TotalCnt = 0, AppCnt = 0, ErrCnt = 0;
            string ErrCardNum = "";
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                int C_index = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_index"].ToString());
                string U_Name = M_Name;


                string CardNo = encrypter.Decrypt(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Number1"].ToString()).Replace("-", "");
                string Card_Per = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["Card_Per"].ToString();

                string HalBu = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Installment_Period"].ToString();

                C_P_Number = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_P_Number"].ToString();
                C_B_Number = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_B_Number"].ToString();

                if (HalBu == "일시불" || HalBu == "")
                    HalBu = "00";
                else
                    HalBu = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Installment_Period"].ToString()).ToString();

                int Send_Amount = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Price2"].ToString());  //승인금액


                string T_currencytype = "0"; //  0이면 한화   1이면 달라다
                string T_rStatus = "";   // 상태 O : 승인, X : 거절
                string T_rAuthNo = "";   // 승인번호 or 거절시 오류코드
                string C_Number3 = "";   // 거래번호      //저장해야함 승인취소시 사용됨.
                string T_Er_Msg = "";

                StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Temp_Card " + C_index + ",'" + OrderNumber + "','" + CardNo + "','" + Card_Per + "','A' , '''','" + cls_User.gid + "'";

                DataSet ds = new DataSet();
                Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

                int Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

                if (Seq_No > 0)
                {
                    if (CardNo != "" && Card_Per != "" && Send_Amount > 0)
                        C_Number3 = ksp.KSPayCreditPostMNI(OrderNumber, U_Name, "", "1", CardNo, Card_Per, HalBu, Send_Amount, C_P_Number, C_B_Number, T_currencytype, ref T_rStatus, ref T_rAuthNo, ref T_Er_Msg);


                    if (C_Number3 != "" && T_rStatus != "X")
                    {
                        StrSql = "Update tbl_Sales_Cacu_Temp SET ";
                        StrSql = StrSql + " C_Number3 = '" + C_Number3 + "'";
                        StrSql = StrSql + " ,C_Number2= '" + T_rAuthNo + "'"; //승인번호                        
                        StrSql = StrSql + " ,Sugi_TF = '2' ";  //승인이 제대로 이루어 졋다. 2번으로 넣는다.
                        StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                        StrSql = StrSql + " And   C_index = " + C_index;

                        Temp_Connect.Update_Data(StrSql);
                        AppCnt++;
                    }
                    else
                    {
                        ErrCnt++;
                        ErrCardNum = ErrCardNum + " / " + CardNo;
                        MessageBox.Show(CardNo + " 카드 승인시 오류 : " + T_Er_Msg);
                    }

                    StrSql = "Update tbl_Sales_Cacu_Card SET ";
                    StrSql = StrSql + " rStatus = '" + T_rStatus + "'";
                    StrSql = StrSql + " ,rAuthNo = '" + T_rAuthNo + "'";
                    StrSql = StrSql + " ,rTransactionNo = '" + C_Number3 + "'";
                    StrSql = StrSql + " ,C_Number3 = '" + C_Number3 + "'";
                    StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
                    StrSql = StrSql + " Where Seqno  =" + Seq_No;

                    Temp_Connect.Update_Data(StrSql, "", "", 1);

                }

                TotalCnt++;
            }

            string Send_M = "총 요청건 : " + TotalCnt + "  정상승인건 : " + AppCnt + " 오류건 : " + ErrCnt
                             + "\n" +
                             "오류카드번호:" + ErrCardNum + " 입니다."
                             ;
            MessageBox.Show(Send_M);

            if (ErrCnt > 0)
            {



                StrSql = "Insert into   tbl_Sales_Cacu_Temp_Mod_Del Select * From  tbl_Sales_Cacu_Temp";
                StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
                StrSql = StrSql + " And C_index =" + SC_index;

                Temp_Connect.Update_Data(StrSql, "", "");

                StrSql = "Delete From tbl_Sales_Cacu_Temp ";
                StrSql = StrSql + " Where OrderNumber = '" + OrderNumber + "'";
                StrSql = StrSql + " And C_index =" + SC_index;

                Temp_Connect.Update_Data(StrSql, "", "");
                return 1;
            }
            else
                return 0;

        }

        private string idx_C_Number3 = "", idx_T_rAuthNo = "", idx_C_Code = "", idx_T_rStatus = "", idx_T_Er_Msg = "", idx_OrderNumber = "";
        private int idx_Send_Pay = 0;

        private int DB_Save_Bank_App(string OrderNumber, int SC_index = 0)
        {

            idx_OrderNumber = OrderNumber; 

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            KSPayApprovalCancelBean_2cs ksp = new KSPayApprovalCancelBean_2cs();

            string StrSql = " Select tbl_SalesDetail.Mbid  , tbl_SalesDetail.Mbid2 ";
            StrSql = StrSql + ", tbl_SalesDetail.OrderNumber,C_index  ";
            StrSql = StrSql + ", LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2) ";
            StrSql = StrSql + ", tbl_SalesDetail.M_name";
            StrSql = StrSql + ", C_Price1";
            StrSql = StrSql + ", LEFT(C_AppDate1,4) +'-' + LEFT(RIGHT(C_AppDate1,4),2) + '-' + RIGHT(C_AppDate1,2)";

            StrSql = StrSql + ", tbl_Card.cardname , C_Number1 , C_Number2 , C_P_Number  , C_B_Number ";
            StrSql = StrSql + ", C_Installment_Period , C_Name1 , C_Price2 ,C_Etc ";
            StrSql = StrSql + ", Case When C_Period1 <>'' And C_Period2 <> '' then   Right (C_Period1,2 ) + C_Period2 ELSE '' End  AS Card_Per ";

            StrSql = StrSql + ", tbl_Sales_Cacu.C_Code ";

            StrSql = StrSql + ", tbl_Sales_Cacu.C_Cash_Send_Nu ";
            StrSql = StrSql + ", tbl_Sales_Cacu.C_Cash_Send_TF ";

            StrSql = StrSql + ", Convert(Varchar,Getdate(),112)  NNDttt ";

            

            StrSql = StrSql + " From tbl_Sales_Cacu (nolock) ";
            StrSql = StrSql + " LEFT Join tbl_SalesDetail  (nolock) ON  tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber  ";
            StrSql = StrSql + " LEFT JOIN tbl_Card  (nolock) ON tbl_Card.Ncode =tbl_Sales_Cacu.C_Code And tbl_Card.Na_code =tbl_SalesDetail.Na_Code  ";

            StrSql = StrSql + " Where tbl_SalesDetail.OrderNumber = '" + OrderNumber + "'";
            StrSql = StrSql + " And tbl_Sales_Cacu.C_TF = 5 ";
            StrSql = StrSql + " And tbl_Sales_Cacu.C_Number3 = '' ";
            StrSql = StrSql + " And tbl_Sales_Cacu.C_Number1 = '' ";
            StrSql = StrSql + " And tbl_Sales_Cacu.C_Price2 > 0 ";

            if (SC_index > 0)
                StrSql = StrSql + " And tbl_Sales_Cacu.C_index =" + SC_index;

            DataSet ds2 = new DataSet();
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds2);

            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return 0;

            string T_rAuthNo = "";   // 승인번호 or 거절시 오류코드
            int TotalCnt = 0, AppCnt = 0, ErrCnt = 0;
            string ErrCardNum = "";
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                int C_index = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_index"].ToString());
                string U_Name = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["M_name"].ToString();
                string C_Code = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Code"].ToString();   

                string CardNo = "가상계좌";
                string Card_Per = "";

                int Send_Amount = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Price2"].ToString());  //승인금액

                string C_Cash_Send_Nu = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Cash_Send_Nu"].ToString().Trim();
                int C_Cash_Send_TF = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Cash_Send_TF"].ToString().Trim());

                string Mbid = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["Mbid"].ToString();
                int  Mbid2 = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["Mbid2"].ToString());

                string cashReceiptType = "0";

                string NNDttt = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["NNDttt"].ToString();


                NNDttt = NNDttt.Substring(0, 4) + '-' + NNDttt.Substring(4, 2) + '-' + NNDttt.Substring(6, 2);
                DateTime TodayDate = new DateTime();
                TodayDate = DateTime.Parse(NNDttt);
                NNDttt = TodayDate.AddDays(2).ToString("yyyy-MM-dd").Replace("-", ""); ;


                string T_rStatus = "";   // 상태 O : 승인, X : 거절
                T_rAuthNo = "";
                string C_Number3 = "";   // 거래번호      //저장해야함 승인취소시 사용됨.
                string T_Er_Msg = "";

                StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_index + ",'" + OrderNumber + "','" + CardNo + "','" + Card_Per + "','A' , '''','" + cls_User.gid + "'";

                DataSet ds = new DataSet();
                Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

                int Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

                if (Seq_No > 0)
                {
                    

                    int Accnt_AV_Flag = 0;
                    idx_C_Number3 = ""; idx_T_rAuthNo = ""; idx_C_Code = "";
                    idx_T_rStatus = "";  idx_T_Er_Msg = "";

                    idx_Send_Pay = Send_Amount; 
                    if (Send_Amount > 0)
                    {
                        //if (int.Parse(NNDttt.Substring(0, 8)) < 20161116)
                        //{
                        //    C_Number3 = ksp.KSPay_VirtualAccount(OrderNumber, U_Name, "", Send_Amount, C_Code, ref T_rStatus, ref T_rAuthNo, ref T_Er_Msg);

                        //    if (C_Number3 != "" && T_rStatus != "X")
                        //    {
                        //        Accnt_AV_Flag = 1;                                
                        //    }

                        //    idx_C_Number3 = C_Number3; idx_T_rAuthNo = T_rAuthNo; idx_C_Code = C_Code;
                        //    idx_T_rStatus = T_rStatus; idx_T_Er_Msg = T_Er_Msg;

                        //}
                        //else
                        //{
                            //frmBase_AV_Accnt e_f = new frmBase_AV_Accnt();
                            //e_f.Send_Address_Info += new frmBase_AV_Accnt.SendAddressDele(e_f_Send_Address_Info);
                            //e_f.Call_searchNumber_Info += new frmBase_AV_Accnt.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info);
                            //e_f.ShowDialog();

                        C_Number3 = ksp.Check_Nice_Same_BankAccount_Web(OrderNumber, C_index, Mbid2, U_Name, C_Code, Send_Amount
                            , C_Cash_Send_Nu, C_Cash_Send_TF, ref T_rAuthNo, ref T_Er_Msg, NNDttt, cashReceiptType, Bank_Mid, Bank_Key);

                        //    if (idx_C_Number3 != "" && idx_T_rAuthNo != "") Accnt_AV_Flag = 1; 

                        //}
                    }



                    if (C_Number3 != "" && C_Number3 != "N" && T_rAuthNo != "" && T_Er_Msg == "Y")
                    {
                        StrSql = "Update tbl_Sales_Cacu SET ";
                        StrSql = StrSql + " C_Number3  = '" + C_Number3 + "'"; //거래번호
                        StrSql = StrSql + " ,C_Number1 = '" + T_rAuthNo + "'"; //가상계좌번호
                        StrSql = StrSql + " ,C_Code = '" + C_Code + "'"; //신한은행
                        StrSql = StrSql + " ,C_Cul_FLAG = 'N' ";
                        StrSql = StrSql + " ,Nice_Mid = '" + Bank_Mid + "'";

                        StrSql = StrSql + " ,C_CancelTF = 0 ";
                        StrSql = StrSql + " ,C_CancelDate = '' ";
                        StrSql = StrSql + " ,C_CancelPrice = 0 ";

                        StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                        StrSql = StrSql + " And   C_index = " + C_index;

                        Temp_Connect.Update_Data(StrSql);


                        StrSql = "Insert into tbl_Sales_Cacu_ACC   ";
                        StrSql = StrSql + " (OrderNumber ,C_index ,C_Cash_Receipt_TF , Bank_Code , Bank_ACC_Account , C_Price2, mbid, mbid2 ,expire_date , Cul_Send_TF , Exi_TF_OrderNumber ) ";
                        StrSql = StrSql + " Values ('" + OrderNumber + "'";
                        StrSql = StrSql + "," + C_index;
                        StrSql = StrSql + ",0";
                        StrSql = StrSql + ",'" + C_Code + "'";
                        StrSql = StrSql + ",'" + T_rAuthNo + "'";
                        StrSql = StrSql + "," + Send_Amount;
                        StrSql = StrSql + ",'" + Mbid + "'";
                        StrSql = StrSql + ", " + Mbid2 ;
                        StrSql = StrSql + ",'',0 , '' ) ";

                        Temp_Connect.Update_Data(StrSql);

                        ////////////2016-09-12 작업.전산에서 가상계좌 현금영수증 신청시 웹, 모바일처럼 입금시 자동 발급 가능하게..명충남 대표 요청
                        //////////StrSql = " EXEC Usp_Sales_Passbook2_Receipt '" + OrderNumber + "', " + C_index;
                        //////////Temp_Connect.Update_Data(StrSql);

                        AppCnt++;
                    }
                    else
                    {
                        StrSql = "Update tbl_Sales_Cacu SET ";
                        StrSql = StrSql + " C_Price1  = 0 ";
                        StrSql = StrSql + " , C_Etc ='" + T_Er_Msg + "'";  //승인 오류시 비고칸에 내역을 넣도록 한다.
                        StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                        StrSql = StrSql + " And   C_index = " + C_index;

                        Temp_Connect.Update_Data(StrSql);

                        ErrCnt++;
                        ErrCardNum = ErrCardNum + " / " + CardNo;
                        MessageBox.Show(CardNo + " 가상계좌 신청시 오류 : " + T_Er_Msg);
                    }

                    StrSql = "Update tbl_Sales_Cacu_Card SET ";
                    StrSql = StrSql + " rStatus = '" + T_rStatus + "'";
                    StrSql = StrSql + " ,rAuthNo = '" + T_rAuthNo + "'";
                    StrSql = StrSql + " ,rTransactionNo = '" + C_Number3 + "'";
                    StrSql = StrSql + " ,C_Number3 = '" + C_Number3 + "'";
                    StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
                    StrSql = StrSql + " Where Seqno  =" + Seq_No;

                    Temp_Connect.Update_Data(StrSql, "", "", 1);

                }

                TotalCnt++;
            }

            string Send_M = "총 요청건 : " + TotalCnt + "  정상승인건 : " + AppCnt + " 오류건 : " + ErrCnt
                             + "\n" +
                             "가상계좌번호:" + T_rAuthNo + " 입니다."
                             ;
            MessageBox.Show(Send_M);

            if (ErrCnt > 0)
                return 100;
            else
                return 0;

        }


        private void e_f_Send_Address_Info(string AddCode1, string AddCode2, string Address1, string Address2, string Address3)
        {
            idx_C_Number3 = AddCode1; 
            idx_T_rAuthNo = AddCode2; 
            idx_C_Code = Address1;
            idx_T_rStatus = Address2; 
            idx_T_Er_Msg = Address3;
        }

        void e_f_Send_MemNumber_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = "";
            searchMbid = idx_OrderNumber;
            searchMbid2 = idx_Send_Pay;


        }




        ////private int DB_Save_Card_App(string OrderNumber, int SC_index = 0)
        ////{
        ////    int SW = 0;

        ////    cls_Connect_DB Temp_Connect = new cls_Connect_DB();
        ////    KSPayApprovalCancelBean_2cs ksp = new KSPayApprovalCancelBean_2cs();


        ////    int TotalCnt = 0, AppCnt = 0, ErrCnt = 0;
        ////    string ErrCardNum = "";

        ////    int C_index = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_index"].ToString());
        ////    string U_Name = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["M_name"].ToString();


        ////    string CardNo = encrypter.Decrypt(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Number1"].ToString()).Replace("-", "");
        ////    string Card_Per = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["Card_Per"].ToString();

        ////    string HalBu = ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Installment_Period"].ToString();

        ////    if (HalBu == "일시불" || HalBu == "")
        ////        HalBu = "00";
        ////    else
        ////        HalBu = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Installment_Period"].ToString()).ToString();

        ////    int Send_Amount = int.Parse(ds2.Tables["Cacu_Card"].Rows[fi_cnt]["C_Price2"].ToString());  //승인금액


        ////    string T_currencytype = "0"; //  0이면 한화   1이면 달라다
        ////    string T_rStatus = "";   // 상태 O : 승인, X : 거절
        ////    string T_rAuthNo = "";   // 승인번호 or 거절시 오류코드
        ////    string C_Number3 = "";   // 거래번호      //저장해야함 승인취소시 사용됨.
        ////    string T_Er_Msg = "";

        ////    StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_index + ",'" + OrderNumber + "','" + CardNo + "','" + Card_Per + "','A' , '''','" + cls_User.gid + "'";

        ////    DataSet ds = new DataSet();
        ////    Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

        ////    int Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

        ////    if (Seq_No > 0)
        ////    {
        ////        if (CardNo != "" && Card_Per != "" && Send_Amount > 0)
        ////            C_Number3 = ksp.KSPayCreditPostMNI(OrderNumber, U_Name, "", "1", CardNo, Card_Per, HalBu, Send_Amount, "98", "750408", T_currencytype, ref T_rStatus, ref T_rAuthNo, ref T_Er_Msg);


        ////        if (C_Number3 != "" && T_rStatus != "X")
        ////        {
        ////            StrSql = "Update tbl_Sales_Cacu SET ";
        ////            StrSql = StrSql + " C_Number3 = '" + C_Number3 + "'";
        ////            StrSql = StrSql + " ,C_Number2= '" + T_rAuthNo + "'"; //승인번호                        
        ////            StrSql = StrSql + " ,Sugi_TF = '2' ";  //승인이 제대로 이루어 졋다. 2번으로 넣는다.
        ////            StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
        ////            StrSql = StrSql + " And   C_index = " + C_index;

        ////            Temp_Connect.Update_Data(StrSql);
        ////            AppCnt++;
        ////        }
        ////        else
        ////        {
        ////            ErrCnt++;
        ////            ErrCardNum = ErrCardNum + " / " + CardNo;
        ////            MessageBox.Show(CardNo + " 카드 승인시 오류 : " + T_Er_Msg);
        ////        }

        ////        StrSql = "Update tbl_Sales_Cacu_Card SET ";
        ////        StrSql = StrSql + " rStatus = '" + T_rStatus + "'";
        ////        StrSql = StrSql + " ,rAuthNo = '" + T_rAuthNo + "'";
        ////        StrSql = StrSql + " ,rTransactionNo = '" + C_Number3 + "'";
        ////        StrSql = StrSql + " ,C_Number3 = '" + C_Number3 + "'";
        ////        StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
        ////        StrSql = StrSql + " Where Seqno  =" + Seq_No;

        ////        Temp_Connect.Update_Data(StrSql, "", "", 1);

        ////    }



        ////                     ;


        ////    if (ErrCnt > 0)
        ////    {           
        ////        string Send_M = "총 요청건 : " + TotalCnt + "  정상승인건 : " + AppCnt + " 오류건 : " + ErrCnt
        ////                     + "\n" +
        ////                     "오류카드번호:" + ErrCardNum + " 입니다."

        ////        MessageBox.Show(Send_M);
        ////        return 1;
        ////    }
        ////    else
        ////    {
        ////        string Send_M = "총 요청건 : " + TotalCnt + "  정상승인건 : " + AppCnt + " 오류건 : " + ErrCnt
        ////                     + "\n" +
        ////                     "오류카드번호:" + ErrCardNum + " 입니다."

        ////        MessageBox.Show(Send_M);
        ////        return 0;
        ////    }

        ////}

    }
}