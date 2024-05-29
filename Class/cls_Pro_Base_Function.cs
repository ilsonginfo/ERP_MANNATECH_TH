using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Data;
//using System.Drawing;
using System.Windows.Forms;
//using System.Reflection; 
using System.Diagnostics;
using System.Reflection;
using System.Drawing;


namespace MLM_Program
{

    class cls_Pro_Base_Function
    {
        public void Put_SellCode_ComboBox(ComboBox cb_1, ComboBox cb_1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select SellCode ,SellTypeName  ";
            Tsql = Tsql + " From tbl_SellType  (nolock)  ";
            Tsql = Tsql + " Order by SellCode ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_SellType", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cb_1.Items.Add(ds.Tables["tbl_SellType"].Rows[fi_cnt]["SellTypeName"].ToString());
                cb_1_Code.Items.Add(ds.Tables["tbl_SellType"].Rows[fi_cnt]["SellCode"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }


        public void Put_NaCode_ComboBox(ComboBox cb_1, ComboBox cb_1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";
            //Tsql = "Select nationCode ,nationNameKo  ";
            // 한국인 경우
            if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
            {
                Tsql = "Select nationCode ,nationNameKo  ";
            }
            // 태국인 경우
            else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
            {
                Tsql = "Select nationCode ,nationNameEng  ";
            }

            Tsql = Tsql + " From tbl_Nation  (nolock)  ";
            Tsql = Tsql + " Where Using_TF = 1 ";
            Tsql = Tsql + " Order by nationNameKo ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_Nation", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                //cb_1.Items.Add(ds.Tables["tbl_Nation"].Rows[fi_cnt]["nationNameKo"].ToString());
                // 한국인 경우
                if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
                {
                    cb_1.Items.Add(ds.Tables["tbl_Nation"].Rows[fi_cnt]["nationNameKo"].ToString());
                }
                // 태국인 경우
                else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
                {
                    cb_1.Items.Add(ds.Tables["tbl_Nation"].Rows[fi_cnt]["nationNameEng"].ToString());
                }
                cb_1_Code.Items.Add(ds.Tables["tbl_Nation"].Rows[fi_cnt]["nationCode"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;

            //if (cls_User.gid_CountryCode != "")
            //{
            //    cb_1_Code.Text = cls_User.gid_CountryCode;
            //    cb_1.SelectedIndex = cb_1_Code.SelectedIndex;
            //    cb_1.Enabled = false;
            //    cb_1_Code.Enabled = false;
            //}
            //++++++++++++++++++++++++++++++++
        }

        public void Put_Close_Sort_ComboBox(ComboBox cb_1, ComboBox cb_1_Code)
        {

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select CloseCode ,CloseTypeName  ";
            Tsql = Tsql + " From tbl_SellType_Close  (nolock)  ";
            Tsql = Tsql + " Order by CloseCode ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_SellType_Close", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cb_1.Items.Add(ds.Tables["tbl_SellType_Close"].Rows[fi_cnt]["CloseTypeName"].ToString());
                cb_1_Code.Items.Add(ds.Tables["tbl_SellType_Close"].Rows[fi_cnt]["CloseCode"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }


        public void Put_Close_Grade_ComboBox(ComboBox cb_1, ComboBox cb_1_Code)
        {

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select Grade_Cnt ,Grade_Name  ";
            Tsql = Tsql + " From tbl_Class  (nolock)  ";
            Tsql = Tsql + " Order by Grade_Cnt ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_Class", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cb_1.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["Grade_Name"].ToString());
                cb_1_Code.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["Grade_Cnt"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }

        public void Put_Close_Grade_ComboBox(ComboBox cb_1, ComboBox cb_1_Code, int CGrade = 0)
        {

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select Grade_Cnt ,Grade_Name  ";
            Tsql = Tsql + " From tbl_Class  (nolock)  ";

            if (CGrade > 0)
                Tsql = Tsql + " Where  Grade_Cnt >= 60 ";
            Tsql = Tsql + " Order by Grade_Cnt ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_Class", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cb_1.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["Grade_Name"].ToString());
                cb_1_Code.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["Grade_Cnt"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }

        public void Put_Close_GradeP_ComboBox(ComboBox cb_1, ComboBox cb_1_Code)
        {

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select Grade_Cnt ,Grade_Name  ";
            Tsql = Tsql + " From tbl_Class_P  (nolock)  ";
            Tsql = Tsql + " Order by Grade_Cnt ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_Class", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cb_1.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["Grade_Name"].ToString());
                cb_1_Code.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["Grade_Cnt"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }


        public void Put_Rec_Code_ComboBox(ComboBox cb_1, ComboBox cb_1_Code)
        {

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            //Tsql = "Select Ncode ,Name  ";
            //Tsql = Tsql + " From tbl_Base_Rec  (nolock)  ";
            //Tsql = Tsql + " Order by Ncode ASC ";

            Tsql = "Select M_Detail , " + cls_app_static_var.Base_M_Detail_Ex + " AS  M_Name";
            Tsql = Tsql + " From tbl_Base_Change_Detail (nolock) ";
            Tsql = Tsql + " Where M_Detail_S = 'tbl_Sales_Rece' ";

            //strSql = strSql + " , Isnull(tbl_Base_Rec.name ,'' ) Base_Rec_Name ";
            //strSql = strSql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " Receive_Method_Name ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_Base_Rec", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cb_1.Items.Add(ds.Tables["tbl_Base_Rec"].Rows[fi_cnt]["M_Name"].ToString());
                cb_1_Code.Items.Add(ds.Tables["tbl_Base_Rec"].Rows[fi_cnt]["M_Detail"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }




        public void Put_Address_Sort_Area(string Address, ref string Area)
        {
            if (Address.Length < 2)
            {
                Area = "모름"; return;
            }

            string T_ad = Address.Replace(" ", "").Substring(0, 2);

            if (T_ad.Contains("서울") == true)
            {
                Area = "서울"; return;
            }

            if (T_ad.Contains("부산") == true)
            {
                Area = "부산"; return;
            }

            if (T_ad.Contains("인천") == true)
            {
                Area = "인천"; return;
            }

            if (T_ad.Contains("광주") == true)
            {
                Area = "광주"; return;
            }

            if (T_ad.Contains("대전") == true)
            {
                Area = "대전"; return;
            }
            if (T_ad.Contains("대구") == true)
            {
                Area = "대구"; return;
            }

            if (T_ad.Contains("울산") == true)
            {
                Area = "울산"; return;
            }

            if (T_ad.Contains("세종") == true)
            {
                Area = "세종"; return;
            }

            if (T_ad.Contains("경기") == true)
            {
                Area = "경기"; return;
            }

            if (T_ad.Contains("강원") == true)
            {
                Area = "강원"; return;
            }

            if (T_ad.Contains("제주") == true)
            {
                Area = "제주"; return;
            }

            if (T_ad.Contains("충청북도") == true || T_ad.Contains("충북") == true)
            {
                Area = "충북"; return;
            }

            if (T_ad.Contains("충청남도") == true || T_ad.Contains("충남") == true)
            {
                Area = "충남"; return;
            }

            if (T_ad.Contains("전라남도") == true || T_ad.Contains("전남") == true)
            {
                Area = "전남"; return;
            }

            if (T_ad.Contains("전라북도") == true || T_ad.Contains("전북") == true)
            {
                Area = "전북"; return;
            }


            if (T_ad.Contains("경상북도") == true || T_ad.Contains("경북") == true)
            {
                Area = "경북"; return;
            }

            if (T_ad.Contains("경상남도") == true || T_ad.Contains("경남") == true)
            {
                Area = "경남"; return;
            }


        }

    }//cls_Sell_Base_Function

}
