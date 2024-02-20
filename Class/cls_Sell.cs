using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using POVASII.Class;
using System.Data;
using System.Data.SqlClient;

namespace MLM_Program
{
    class cls_Sell
    {
        public string  OrderNumber ;
        public string   Mbid	;
        public int      Mbid2	;
        public string   M_Name	;
        public string   SellDate	;
        public string   SellDate_2   ;
        public string   SellCode	;
        public string   SellCodeName	;
        public string   BusCode	;
        public string   BusCodeName	;
        public string   Re_BaseOrderNumber	;
        public double    TotalPrice	;
        public double    TotalPV	;
        public double    TotalCV	;
        public double    TotalInputPrice	;
        public double    Total_Sell_VAT_Price	;
        public double    Total_Sell_Except_VAT_Price	;
        public double    InputCash	;
        public double    InputCard	;
        public double    InputPassbook	;
        public double     InputPassbook_2;
        public double     InputNaver;
        public double InputPayment_8_TH;
        public double InputPayment_9_TH;
        public double InputPayment_10_TH;

        public double    Be_InputMile;
        public double    InputMile	;
        public double    InputPass_Pay	;
        public double    UnaccMoney	;
        public string   Etc1	;
        public string   Etc2	;
        public string   Na_Code;

        public string  SellSort;

        public int      ReturnTF	;
        public string   ReturnTFName;
        public string   INS_Num	;
        public string    INS_Num_Real;
        public string   InsuranceNumber_Date	;
        public int      W_T_TF	;
        public int      In_Cnt	;

        public string   RecordID	;
        public string   RecordTime	;
        public string Exi_TF;

        public int SellTF;
        public string SellTFName;
        public int Union_Seq;
        public int Us_Ord;
        public int Ga_Order;
        public double InputCoupon;
        public string Del_TF;
        public string Associated_Card;
    }

    class cls_Sell_Item
    {
        public int      SalesItemIndex ;
        public string   OrderNumber;
        public string   ItemCode	;
        public string   ItemName	;
        public double    ItemPrice	;
        public double    ItemPV	;
        public double    ItemCV	;
        public int      Sell_VAT_TF;
        public double    Sell_VAT_Price	;
        public double    Sell_Except_VAT_Price	;
        public string   SellState	;
        public string   SellStateName;
        public int      ItemCount	;
        public double    ItemTotalPrice	;
        public double    ItemTotalPV	;
        public double    ItemTotalCV	;
        public double    Total_Sell_VAT_Price	;
        public double    Total_Sell_Except_VAT_Price	;
        public string   ReturnDate	;
        public string   SendDate	;
        public string   ReturnBackDate	;
        public string   Etc	;
        public int      RecIndex	;
        public int      Send_itemCount1	;
        public int      Send_itemCount2	;
        public string   T_OrderNumber1	;
        public string   T_OrderNumber2	;
        public int      Real_index	;
        public string   G_Sort_Code;


        public string   RecordID;
        public string   RecordTime;

       
         public string   DelRecordID;
        public string   DelRecordTime;
                    

        public string Del_TF;
    }



    class cls_Sell_Cacu
    {
        public int    C_index;
        public string OrderNumber;
        public int    C_TF;
        public string C_TF_Name ;     
                
        public string C_Code	;
        public string C_CodeName	;
        public string C_CodeName_2  ;
        public string C_Name1	;
        public string C_Name2	;
        public string C_Number1	;
        public string C_Number2;
        public string C_Number3	;
        public string C_Number4;
        public double  C_Price1	;
        public double  C_Price2	;
        public string C_AppDate1	;
        public string C_AppDate2	;
        public int    C_CancelTF	;
        public string C_CancelDate	;
        public double  C_CancelPrice;
        public string C_Period1	;
        public string C_Period2	;
        public string C_Installment_Period	;
        public string C_Etc	;
        public int    C_Base_Index;
        public string Sugi_TF;
        public string C_B_Number;
        public string C_P_Number; 

        public string C_Cash_Number2;
        public string C_Cash_Number;
        

        public string C_Cash_Send_Nu;
        public int C_Cash_Send_TF;
        public int C_Cash_Sort_TF;
        public int C_Cash_Bus_TF;

        public string C_CVC;
        

        public string RecordID;
        public string RecordTime;

        public string DelRecordID;
        public string DelRecordTime;

        public string Del_TF;
        public string Associated_Card;
        public string C_Coupon;
    }


    class cls_Sell_Rece : ICloneable
    {

        public int SalesItemIndex;
        public string OrderNumber;
        public int RecIndex;
        public int Receive_Method;
        public string Receive_Method_Name;

        public string Get_Date1;
        public string Get_Date2;
        public string Get_Name1;
        public string Get_Name2;
        public string Get_ZipCode;
        public string Get_Address1;
        public string Get_Address2;

        public string Get_Tel1;
        public string Get_Tel2;
        public string Get_Etc1;
        public string Get_Etc2;
        public string Get_city;     // 태국 도시
        public string Get_state;    // 태국 주
        public string Pass_Number;
        public double Pass_Pay;
        public string Pass_Number2;
        public string Base_Rec;
        public string Base_Rec_Name;

        public string Receive_Center_Name;
        public string Receive_Center;

        public string RecordID;
        public string RecordTime;

        public string DelRecordID;
        public string DelRecordTime;

        public string Del_TF;

        public object Clone()
        {
            return new cls_Sell_Rece()
            {
                SalesItemIndex = this.SalesItemIndex,
                OrderNumber = this.OrderNumber,
                RecIndex = this.RecIndex,
                Receive_Method = this.Receive_Method,
                Receive_Method_Name = this.Receive_Method_Name,

                Get_Date1 = this.Get_Date1,
                Get_Date2 = this.Get_Date2,
                Get_Name1 = this.Get_Name1,
                Get_Name2 = this.Get_Name2,
                Get_ZipCode = this.Get_ZipCode,
                Get_Address1 = this.Get_Address1,
                Get_Address2 = this.Get_Address2,

                Get_Tel1 = this.Get_Tel1,
                Get_Tel2 = this.Get_Tel2,
                Get_Etc1 = this.Get_Etc1,
                Get_Etc2 = this.Get_Etc2,
                Get_city = this.Get_city,       // 태국 도시
                Get_state = this.Get_state,     // 태국 주
                Pass_Number = this.Pass_Number,
                Pass_Pay = this.Pass_Pay,
                Pass_Number2 = this.Pass_Number2,
                Base_Rec = this.Base_Rec,
                Base_Rec_Name = this.Base_Rec_Name,

                Receive_Center_Name = this.Receive_Center_Name,
                Receive_Center = this.Receive_Center,

                RecordID = this.RecordID,
                RecordTime = this.RecordTime,

                DelRecordID = this.DelRecordID,
                DelRecordTime = this.DelRecordTime,

                Del_TF = this.Del_TF
            };

        }
    }
     //Tsql = "select user_id, u_name " ;
     //       Tsql = Tsql + ", user_password,  CenterCode , Log_Check, Log_Date ";
     //       Tsql = Tsql + ", LanNumber , Isnull(tbl_business.Name,'') AS U_CC_Name " ;

    class cls_tbl_User
    {
        public string user_Ncode;
        public string user_id;
        public string u_name;
        public string user_password;
        public string CenterCode;
        public int Log_Check;

        public string Log_Date;
        public string LanNumber;
        public string U_CC_Name;

        public int Sell_Info_V_TF;
        public string Na_Code;
        public string Na_Code_Name;
        public string Menu1;

        public int Excel_Save_TF;
        public int Cpno_V_TF;
        public int For_Save_TF;
        public int CC_Save_TF;
        public int Sell_Mem_TF_Ch_TF;
        public int Name_Ch_TF;
        public int Nominid_Ch_TF;
        public int Rec_Ch_TF;
        public int Talk_In_TF; 
        
        
        

        public int SellInput;
        public string FarMenu;

        public string phone;
        public int Leave_TF;

        public int Card_Num_V_TF;
        public int Card_Sugi_TF;

        public int Cash_V_TF;
        public int Return_Cacu_Save_FLAG;
        public int Return_Cacu_Cancel_FLAG;

        public string User_FLAG;


        public string U_Dep;
        public string U_Pos;
        public string U_email;
        public string U_Dir_Phone;
        public string U_Entry_Date;
        public string U_Leave_Date;
    }


    class cls_Close_Mem
    {
        public string Mbid;
        public int Mbid2;
        public string M_Name;
        public int LineCnt;
        public int N_LineCnt;

        public string Saveid;
        public int Saveid2;

        public string Nominid;
        public int Nominid2;

        public double SellPV01;
        public double SellPV02;
        public double SellPV03;
        public double ShamPV;
        
        
        public double DayPV01;
        public double DayPV02;
        public double DayPV03;
        public int Sell_Mem_TF;

        public string LeaveDate;
        public string StopDate;
        public string BusCode ;

        public string Cur_End_35;
        public string Cur_Start_35;

        public int ReqTF1;
        public int ReqTF2;
        public int RBO_Mem_TF;
        public string RBODate;
        

        public int CurGrade;
        public int CurPoint;
        public int OneGrade;
        public int OrgGrade;
        
    }

    class cls_Close_Sell
    {
        public string Mbid;
        public int Mbid2;
        public string M_Name;

        public int CurGrade;
        public int CurPoint;

        public string Saveid;
        public int Saveid2;
        public int LineCnt;

        public string Nominid;
        public int Nominid2;
        public int N_LineCnt;

        public double SellPV01;
        public double SellPV02;
        public double SellPV03;

        public double  DayPV01;
        public double DayPV02;
        public double DayPV03;

        

        public string SellCode;
        public string SellDate;
        public string OrderNumber ;

        public double TotalPV;
        public double TotalPrice;

        public double RePV;
        public double RePrice;               
    }


     class cls_Memb_Area
    {        
        public string Area;
        public string Year_lvl;
        public double W_Cnt;
        public double M_Cnt;
        public double Not_Cnt;        
    }


     class cls_AutoShip
     {
         public string Auto_Seq;
         public string mbid;
         public int mbid2;
         public string Req_Type;
         public string Req_State;
         public string Start_Date;
         public string End_Date;
         public string Extend_Date;
         public string Proc_Date;
         public double TotalPrice;
         public double TotalPV;
         public double TotalCV;
         public string Etc;
         public string End_Reason;
         public string RecordID;
         public string RecordTime;
     }

     class cls_AutoShip_Item
     {
         public string Auto_Seq;
         public int ItemIndex;
         public string ItemCode;
         public string ItemName;
         public int ItemCount;
         public double ItemPrice;
         public double ItemPV;
         public double ItemCV;
         public double ItemTotalPrice;
         public double ItemTotalPV;
         public double ItemTotalCV;
         public string RecordID;
         public string RecordTime;
         public string Del_TF;
     }

     class cls_AutoShip_Cacu
     {
         public string Auto_Seq;
         public int CacuIndex;
         public int Cacu_Type;
         public string CardCode;
         public string CardName;
         public string CardNumber;
         public string Period1;
         public string Period2;
         public string Card_OwnerName;
         public string C_P_Number;
         public string C_B_Number;
         public string C_CardType;
         public double Payment_Amt;
         public string Installment_Period;
         public string AuthNumber;
         public string C_CVC;   // 태국 결제 관련 추가 - 231114 syhuh
         public string RecordID;
         public string RecordTime;
         public string Del_TF;
     }

     class cls_AutoShip_Rece
     {
         public string Auto_Seq;
         public int RecIndex;
         public string Rec_Name;
         public string Rec_Tel;
         public string Rec_AddCode;
         public string Rec_Address1;
         public string Rec_Address2;
         public string Rec_city;    // 태국 결제 관련 추가 - 231114 syhuh
         public string Rec_state;   // 태국 결제 관련 추가 - 231114 syhuh
         public string RecordID;
         public string RecordTime;
         public string Del_TF;
     }



    class cls_tbl_Mileage
    {

        public double  
            
            Using_Mileage_Search(string Mbid, int Mbid2 , string BaseDate_t  )
        {
            double Sum_M = 0; double Sum_P = 0;            
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            string BaseDate = BaseDate_t.Replace("-", "");

            //============================================================================================================
            Tsql = "SELECT Isnull( Sum(PlusValue) ,0) A1    ";
            Tsql = Tsql + " From tbl_Member_Mileage  (nolock)  ";
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Convert(Varchar,Mbid2) like '%" + Mbid2.ToString() + "%' ";
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And  Mbid2 = " + Mbid2.ToString();               
            }

            if (BaseDate != "")
            {
                Tsql = Tsql + " And ( (PayDate <= '" + BaseDate + "' And  PayDate <> '' ) ";
                Tsql = Tsql + " OR  PayDate = '') ";
            }

            //Tsql = Tsql + " And BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            //Tsql = Tsql + " Order by Mbid, Mbid2 ASC ";
            
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Member_Mileage", ds) == true)
            {
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt >= 0)
                    Sum_P = double.Parse(ds.Tables["tbl_Member_Mileage"].Rows[0][0].ToString());
            }
            //============================================================================================================
            


            //============================================================================================================
            Tsql = "SELECT Isnull( Sum(MinusValue) ,0) A1    ";
            Tsql = Tsql + " From tbl_Member_Mileage  (nolock)  ";
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Convert(Varchar,Mbid2) like '%" + Mbid2.ToString() + "%' ";
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And  Mbid2 = " + Mbid2.ToString();
            }

            //Tsql = Tsql + " And BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            //Tsql = Tsql + " Order by Mbid, Mbid2 ASC ";

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다. 
            ds.Clear();
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Member_Mileage", ds) == true)
            {
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt >= 0)
                    Sum_M = double.Parse(ds.Tables["tbl_Member_Mileage"].Rows[0][0].ToString());
            }

            return Sum_P - Sum_M;
            //============================================================================================================
        }


        public double Using_Mileage_Search(string OrderNumber )
        {
            double Sum_M = 0; double Sum_P = 0;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;

            //============================================================================================================
            Tsql = "SELECT Isnull( Sum(PlusValue) ,0) A1    ";
            Tsql = Tsql + " From tbl_Member_Mileage  (nolock)  ";
            Tsql = Tsql + " Where Plus_OrderNumber  = '" + OrderNumber + "'";             
            
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Member_Mileage", ds) == true)
            {
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt >= 0)
                    Sum_P = double.Parse(ds.Tables["tbl_Member_Mileage"].Rows[0][0].ToString());
            }
            //============================================================================================================



            //============================================================================================================
            Tsql = "SELECT Isnull( Sum(MinusValue) ,0) A1    ";
            Tsql = Tsql + " From tbl_Member_Mileage  (nolock)  ";
            Tsql = Tsql + " Where  Minus_OrderNumber = '" + OrderNumber + "'";             

            //Tsql = Tsql + " And BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            //Tsql = Tsql + " Order by Mbid, Mbid2 ASC ";

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.            
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Member_Mileage", ds) == true)
            {
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt >= 0)
                    Sum_M = double.Parse(ds.Tables["tbl_Member_Mileage"].Rows[0][0].ToString());
            }

            return Sum_P - Sum_M;
            //============================================================================================================
        }

        public void Put_Plus_Mileage(string Mbid, int Mbid2 , string M_Name , double  Plus_Mile , string  OrderNumber , string  TCode
           ,cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string T_ETC , string t_form_name = "", string t_from_text = "")
        {
                      
            string StrSql;

            StrSql = "INSERT INTO tbl_Member_Mileage " ;
            StrSql = StrSql + "(T_Time,mbid,mbid2,M_Name,PlusValue,PlusKind,Plus_OrderNumber,User_id, ETC1)";
            StrSql = StrSql + " VALUES ";
            StrSql = StrSql + "( Convert(Varchar(25),GetDate(),21),'" + Mbid + "'," + Mbid2 + ",'" + M_Name + "'";
            StrSql = StrSql + "," + Plus_Mile + ",'" + TCode + "','" + OrderNumber + "','" + cls_User.gid + "','" + T_ETC + "'";
            StrSql = StrSql + ")";

            Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo", Conn, tran, t_form_name, t_from_text);
        }


        public void Put_Minus_Mileage(string Mbid, int Mbid2, string M_Name, double Minus_Mile, string OrderNumber, string TCode
           , cls_Connect_DB Temp_Connect, SqlConnection Conn, SqlTransaction tran, string T_ETC, string t_form_name = "", string t_from_text = "")
        {

            string StrSql;

            StrSql = "INSERT INTO tbl_Member_Mileage ";
            StrSql = StrSql + "(T_Time,mbid,mbid2,M_Name,MinusValue,MinusKind,Minus_OrderNumber,User_id, ETC1)";
            StrSql = StrSql + " VALUES ";
            StrSql = StrSql + "( Convert(Varchar(25),GetDate(),21),'" + Mbid + "'," + Mbid2 + ",'" + M_Name + "'";
            StrSql = StrSql + "," + Minus_Mile + ",'" + TCode + "','" + OrderNumber + "','" + cls_User.gid + "','" + T_ETC + "'";
            StrSql = StrSql + ")";

            Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo", Conn, tran, t_form_name, t_from_text);
        }

    }




}
