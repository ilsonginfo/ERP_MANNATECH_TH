////실 DB 혹은 우리자리에서 UAT버전 들어가기
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.IO;
using System.Resources;
using System.Data;
using System.Management;


namespace MLM_Program
{
    static class Program
    {




        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.Run(new Form1());
            //GGGG
            if (NetworkInterface.GetIsNetworkAvailable() == false) // 인터넷 연결 여부를 체크한다.
            {
                MessageBox.Show("Not Connect Network.");
                return;
            }

            string ap_path = Application.StartupPath.ToString();


            //<<<<<<<<<<<<<<<<<<<<<<<<<<업데이트 관련된 파일들을 지운다.  Temp_Up_E.dat  Temp_Up_E.dat 파일2개를
            FileInfo fileTempUpl = new FileInfo(Path.Combine(ap_path, "Temp_Up_E.dat"));
            if (fileTempUpl.Exists)
                fileTempUpl.Delete();

            FileInfo fileMUP_E = new FileInfo(Path.Combine(ap_path, "Temp_Up_E.dat"));
            if (fileMUP_E.Exists)
                fileMUP_E.Delete();
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



            cls_app_static_var.Assem = System.Reflection.Assembly.GetExecutingAssembly();


            //로그인 오류 메시지 관련해서 우선은 박아 넣어야함.
            cls_app_static_var.app_msg_resource = "MLM_Program.Resources.Msg_Resource";
            cls_app_static_var.app_msg_rm = new System.Resources.ResourceManager(cls_app_static_var.app_msg_resource, cls_app_static_var.Assem);

            ////++++++++++++++++++++++++++++++++            
            string Base_ServerIpAddress = "";

            if (File.Exists(Path.Combine(ap_path, "POVAS_s.ini")) == true)
            {
                FileStream fs = new FileStream(Path.Combine(ap_path, "POVAS_s.ini"), FileMode.Open);
                StreamReader Sw = new StreamReader(fs);

                Base_ServerIpAddress = Sw.ReadLine().ToString();

                Sw.Close();
                fs.Close();
            }

            else
            {
                MessageBox.Show("Not Found Base File - POVAS_s.ini.");
                return;
            }
            ////++++++++++++++++++++++++++++++++

            ////string SDate = "";
            ////string ToEndDate = "20140505";
            ////DateTime dt = DateTime.Parse(ToEndDate.Substring(0, 4) + "-" + ToEndDate.Substring(4, 2) + "-" + ToEndDate.Substring(6, 2));
            ////SDate = dt.AddMonths(-3).ToShortDateString().Replace("-", "")g

#if DEBUG

            //////++++++++++++++테스트++++++++++++++++ +

            cls_app_static_var.app_Company_Name = "menatech";
            cls_app_static_var.app_FTP_ID = "melong202";
            cls_app_static_var.app_FTP_PW = "rladudtn!&&1";
            string User_Id = "IS_Info";
            string password = "Mannatech)%!$";
            string Company_DB_Name = "mannatech";
            cls_app_static_var.app_Company_Name = "menatech";


            //++++++++++++++테스트++++++++++++++++ +

            //++++++++++++++라이브++++++++++++++++ +

            //cls_app_static_var.app_Company_Name = "mannatech";
            //cls_app_static_var.app_FTP_ID = "melong202";
            //cls_app_static_var.app_FTP_PW = "rladudtn!&&1";
            //string User_Id = "IS_Info";
            //string password = "Mannatech)%!$";
            //string Company_DB_Name = "mannatech";
            //cls_app_static_var.app_Company_Name = "mannatech_Live";
            //////++++++++++++++라이브++++++++++++++++ +

            cls_app_static_var.APP_VER = "DEBUG 001";
#else


            //////++++++++++++++테스트++++++++++++++++ +

            cls_app_static_var.app_Company_Name = "menatech";
            cls_app_static_var.app_FTP_ID = "melong202";
            cls_app_static_var.app_FTP_PW = "rladudtn!&&1";
            string User_Id = "IS_Info";
            string password = "Mannatech)%!$";
            string Company_DB_Name = "mannatech";
            cls_app_static_var.app_Company_Name = "menatech";


            ////++++++++++++++라이브++++++++++++++++ +

            //cls_app_static_var.app_Company_Name = "mannatech";
            //cls_app_static_var.app_FTP_ID = "melong202";
            //cls_app_static_var.app_FTP_PW = "rladudtn!&&1";
            //string User_Id = "IS_Info";
            //string password = "Mannatech)%!$";
            //string Company_DB_Name = "mannatech";
            //cls_app_static_var.app_Company_Name = "mannatech_Live";
            ////////++++++++++++++라이브++++++++++++++++ +
            /////

            cls_app_static_var.APP_VER = "[RELEASE]240529_UAT";
#endif

            //++++++++++++++++++++++++++++++++++++++
            //cls_app_static_var.app_Company_Name = "mannatech_JEE";
            //cls_app_static_var.app_FTP_ID = "melong202";
            //cls_app_static_var.app_FTP_PW = "rladudtn!&&1";
            //string User_Id = "ILS_mannatech";
            ////string password = "ilsong1226_";
            //Company_DB_Name = "mannatech_J";


            //cls_app_static_var.app_Company_Name = "mannatech";
            //cls_app_static_var.app_FTP_ID = "melong202";
            //cls_app_static_var.app_FTP_PW = "rladudtn!&&1";
            //User_Id = "ILS_mannatech";
            //password = "ilsong1226_";
            //Company_DB_Name = "mannatech_J";


            //cls_Connect_DB.Base_Conn_Str = "server=" + Base_ServerIpAddress + ",10240;database=Promax_Home;user id=sa;password=ilsong#x";
            cls_Connect_DB.Base_Conn_Str = "server=" + Base_ServerIpAddress + ",10240;database=Promax_Home;user id=ILS_PROMAX_20;password=PMX#x20!ilsong";
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            string Connect_IP = "", Zip_IP = "", Z_IP_Daum = "";
            Tsql = "Select  Co_ip , Up_TF , Z_IP , Z_IP_Daum ";
            Tsql = Tsql + " From Tbl_Co_Code_2  (nolock)   ";
            Tsql = Tsql + " Where C_AppPath_Name ='" + cls_app_static_var.app_Company_Name + "'";

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set_Base(Tsql, "Tbl_Co_Code", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)
            {
                MessageBox.Show("Not Found Base DB info.");
                return;
            }
            else
            {
                Connect_IP = ds.Tables["Tbl_Co_Code"].Rows[0]["Co_ip"].ToString();
                Zip_IP = ds.Tables["Tbl_Co_Code"].Rows[0]["Z_IP"].ToString();
                Z_IP_Daum = ds.Tables["Tbl_Co_Code"].Rows[0]["Z_IP_Daum"].ToString();
            }


            if (Connect_IP == "")
            {
                MessageBox.Show("Not Found Base DB info.");
                return;
            }

#if DEBUG
            //////테스트배포때 연다
            //if (cls_app_static_var.app_Company_Name == "menatech")
            //{
            //    cls_Connect_DB.LiveFlag = false;
            //}
            ////테스트배포때 연다
            //++++++++++++++++++++++++++++++++
            ////테스트배포땐닫는다
            //운영 DB아닌 경우 모두 개발기로 인식(업데이트 무시, 메인화면 변경 처리)
            //////매나테크 개발기 UAT
            Connect_IP = "218.237.118.12,51433";
            if (Connect_IP.Equals("218.237.118.12,51433"))
            {
                cls_Connect_DB.LiveFlag = false;
            }
            ////테스트배포땐닫는다
            //////++++++++++++++++++++++++++++++++

#else
            //////매나테크 개발기 UAT
            Connect_IP = "218.237.118.12,51433";
            if (Connect_IP.Equals("218.237.118.12,51433"))
            {
                cls_Connect_DB.LiveFlag = false;
            }
            //////테스트배포땐닫는다
            //
            if (cls_app_static_var.app_Company_Name == "menatech")
            {
                cls_Connect_DB.LiveFlag = false;
            }
#endif

            cls_Connect_DB.Conn_Str = "Initial Catalog=" + Company_DB_Name + ";Persist Security Info=True;User ID=" + User_Id + ";Password=" + password + ";Data Source=" + Connect_IP + "";
            cls_Connect_DB.Return_Conn_Str = "Initial Catalog=mannatech_Return_Close;Persist Security Info=True;User ID=" + User_Id + ";Password=" + password + ";Data Source=" + Connect_IP + "";
            cls_Connect_DB.Ga_Close_Conn_Str = "Initial Catalog=mannatech_Ga_Close;Persist Security Info=True;User ID=" + User_Id + ";Password=" + password + ";Data Source=" + Connect_IP + "";

            //cls_Connect_DB.Conn_Str = cls_Connect_DB.Conn_Str + ";Encrypt=true;TrustServerCertificate = true";

            //cls_Connect_DB.Conn_Str = "server= " + Connect_IP + ",10240;database=" + Company_DB_Name + ";user id=" + User_Id + ";password=" + password;
            cls_Connect_DB.AddCode_Conn_Str = "server=" + Zip_IP + ",10240;database=Zip_CS;user id=ILS_Zip_CS;password=ilsong#x";
            cls_Connect_DB.AddCode_Daum_Conn_Str = "server=" + Z_IP_Daum + ",10240;database=ADD_DB;user id=ILS_ADD_1;password=ilsong#x_1";
            cls_Connect_DB.SMS_Conn_Str = "server=" + Zip_IP + ",10240;database=ILS_SMS_20;user id=sa;password=SMS#x20!ilsong";


            //cls_app_static_var.app_multi_lang_query = 0; //1 이면 멀티 랭귀지임... 0이면 한국어임.

            //cls_app_static_var.Using_Multi_language = 0;  //멀티 랭귀지를 사용한다.

            //20230616 구현호 멀티랭기지 사용하라고하신다 김차장님이
            cls_app_static_var.app_multi_lang_query = 1; //1 이면 멀티 랭귀지임... 0이면 한국어임.

            cls_app_static_var.Using_Multi_language = 1;  //멀티 랭귀지를 사용한다.
            //20230616 구현호 멀티랭기지 사용하라고하신다 김차장님이
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frm_Login());



            if (cls_User.gid != "" && cls_User.gid != null)
            {
                //프로그램에서 자주 변경되는 말들을 미리 정의함. 회원은 고객으로 한다든가 후원은 직대라 한다든가 용
                //용어를 변경할때.. 관리하기 위함.
                cls_app_static_var.app_Base_Str_resource = "MLM_Program.Resources.Base_Str_Resource";
                cls_app_static_var.app_base_str_rm = new ResourceManager(cls_app_static_var.app_Base_Str_resource, cls_app_static_var.Assem);

                string StrSql = "";

                if (cls_app_static_var.app_multi_lang_query == 1)
                {
                    if (cls_User.gid_CountryCode == "US" )
                    {
                        cls_app_static_var.User_Time_Zone = "Central Standard Time";
                    }


                    ////베이스 폴더에 들어가는 캡션들에 대해서 리소스에서 관리한다
                    if (cls_User.gid_CountryCode == "KR" || cls_User.gid_CountryCode == "")
                    {
                        cls_app_static_var.User_Time_Zone = "Korea Standard Time";
                    }

                    if (cls_User.gid_CountryCode == "Ja" || cls_User.gid_CountryCode == "")
                    {
                        cls_app_static_var.User_Time_Zone = "Tokyo Standard Time";
                    }

                    if (cls_User.gid_CountryCode == "TH" || cls_User.gid_CountryCode == "")
                    {
                        cls_app_static_var.User_Time_Zone = "Thailand Standard Time";
                    }

                    //if (cls_User.gid_CountryCode == "La")
                    //{
                    //    //cls_app_static_var.app_Base_Caption_resource = "MLM_Program.Resources.Laos_Caption_Resource";
                    //    cls_app_static_var.User_Time_Zone = "SE Asia Standard Time";
                    //    StrSql = "Select Base_L, Eng_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";

                    //}

                    //if (cls_User.gid_CountryCode == "Ja")
                    //{
                    //    //cls_app_static_var.app_Base_Caption_resource = "MLM_Program.Resources.Japan_Caption_Resource";
                    //    cls_app_static_var.User_Time_Zone = "Tokyo Standard Time";
                    //    StrSql = "Select Base_L, Jap_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";
                    //}

                    if (cls_app_static_var.Using_language == "Korean" || cls_app_static_var.Using_language == "")
                    {
                        StrSql = "Select Base_L, Kor_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";
                    }

                    if (cls_app_static_var.Using_language == "English" )
                    {
                        StrSql = "Select Base_L, Eng_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";
                    }

                    if (cls_app_static_var.Using_language == "Japanese")
                    {
                        StrSql = "Select Base_L, Jap_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";
                    }

                    if (cls_app_static_var.Using_language == "Thai")
                    {
                        StrSql = "Select Base_L, Eng_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";   // 230817 - 허성윤, 태국건은 영문으로 나오도록 설정
                    }

                }
                else
                {
                    cls_app_static_var.User_Time_Zone = "Korea Standard Time";
                    StrSql = "Select Base_L, Kor_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";
                }


                DataSet ds2 = new DataSet();

                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Base_Label", ds2) == false) return;

                int ReCnt2 = Temp_Connect.DataSet_ReCount;

                for (int fi_cnt = 0; fi_cnt <= ReCnt2 - 1; fi_cnt++)
                {
                    if (ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["T_Label"].ToString() != "")
                        cls_app_static_var.Base_Label[ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["Base_L"].ToString()] = ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["T_Label"].ToString();
                    else
                        cls_app_static_var.Base_Label[ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["Base_L"].ToString()] = ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["Base_L"].ToString();
                }

                //try
                //{

                //if (cls_app_static_var.Program_Update_FileName != "")
                //    Application.Run(new frmBase_Update()); 


                Application.Run(new MDIMain());

                //}
                //catch (System.Exception theException)
                //{
                //    string errorMessage;
                //    errorMessage = "Error: ";
                //    errorMessage = String.Concat(errorMessage, theException.Message);
                //    errorMessage = String.Concat(errorMessage, " Line: ");
                //    errorMessage = String.Concat(errorMessage, theException.Source);

                //    MessageBox.Show(errorMessage, "Error");                
                //}

            }
        }







    }
}

//D:/증현용/Demo/MLM_Demo_01/MLM_Demo_01/ResourcesMsg_Resource.Designer.cs
//cls_app_static_var.app_Base_Str_resource.gd
//IResourceReader rr = new ResourceReader();

//IDictionaryEnumerator de = rr.GetEnumerator();

//while (de.MoveNext())
//{
//    Console.WriteLine(de.Key + " : " + de.Value);
//}

//Properties.Resources.
//MessageBox.Show(cls_app_static_var.app_base_str_rm.Length.ToString());
//CharEnumerator ce = cls_app_static_var.app_Base_Str_resource.GetEnumerator();
//while (ce.MoveNext())
//{
//    Console.WriteLine(ce.ToString ());
//}




//System.Reflection.Assembly thisExe;

//thisExe = System.Reflection.Assembly.GetExecutingAssembly();

//string[] resources = thisExe.GetManifestResourceNames();

//string list = "";

//// Build the string of resources.

//foreach (string resource in resources)

//    list += resource + "\r\n";



//Assembly _assembly;
//              _assembly = Assembly.GetExecutingAssembly();
//              StreamReader _textStreamReader;

//              _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("MLM_Program.Resources.Base_Str_Resource.resources"));

//              while (_textStreamReader.Peek() >= 0)
//              {
//                  Console.WriteLine(_textStreamReader.ReadLine() );
//              }
/// 여기서부터 UAT 배포용 내용
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Forms;
//using System.Net.NetworkInformation;
//using System.IO;
//using System.Resources;
//using System.Data;
//using System.Management;


//namespace MLM_Program
//{
//    static class Program
//    {




//        /// <summary>
//        /// 해당 응용 프로그램의 주 진입점입니다.
//        /// 해당 응용 프로그램의 주 진입점입니다.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            //Application.Run(new Form1());
//            //GGGG
//            if (NetworkInterface.GetIsNetworkAvailable() == false) // 인터넷 연결 여부를 체크한다.
//            {
//                MessageBox.Show("Not Connect Network.");
//                return;
//            }

//            string ap_path = Application.StartupPath.ToString();


//            //<<<<<<<<<<<<<<<<<<<<<<<<<<업데이트 관련된 파일들을 지운다.  Temp_Up_E.dat  Temp_Up_E.dat 파일2개를
//            FileInfo fileTempUpl = new FileInfo(Path.Combine(ap_path, "Temp_Up_E.dat"));
//            if (fileTempUpl.Exists)
//                fileTempUpl.Delete();

//            FileInfo fileMUP_E = new FileInfo(Path.Combine(ap_path, "Temp_Up_E.dat"));
//            if (fileMUP_E.Exists)
//                fileMUP_E.Delete();
//            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



//            cls_app_static_var.Assem = System.Reflection.Assembly.GetExecutingAssembly();


//            //로그인 오류 메시지 관련해서 우선은 박아 넣어야함.
//            cls_app_static_var.app_msg_resource = "MLM_Program.Resources.Msg_Resource";
//            cls_app_static_var.app_msg_rm = new System.Resources.ResourceManager(cls_app_static_var.app_msg_resource, cls_app_static_var.Assem);

//            ////++++++++++++++++++++++++++++++++            
//            string Base_ServerIpAddress = "";

//            if (File.Exists(Path.Combine(ap_path, "POVAS_s.ini")) == true)
//            {
//                FileStream fs = new FileStream(Path.Combine(ap_path, "POVAS_s.ini"), FileMode.Open);
//                StreamReader Sw = new StreamReader(fs);

//                Base_ServerIpAddress = Sw.ReadLine().ToString();

//                Sw.Close();
//                fs.Close();
//            }

//            else
//            {
//                MessageBox.Show("Not Found Base File - POVAS_s.ini.");
//                return;
//            }
//            ////++++++++++++++++++++++++++++++++

//            ////string SDate = "";
//            ////string ToEndDate = "20140505";
//            ////DateTime dt = DateTime.Parse(ToEndDate.Substring(0, 4) + "-" + ToEndDate.Substring(4, 2) + "-" + ToEndDate.Substring(6, 2));
//            ////SDate = dt.AddMonths(-3).ToShortDateString().Replace("-", "")g

//            //++++++++++++++테스트++++++++++++++++ +

//            cls_app_static_var.app_Company_Name = "menatech";
//            cls_app_static_var.app_FTP_ID = "melong202";
//            cls_app_static_var.app_FTP_PW = "rladudtn!&&1";
//            string User_Id = "IS_Info";
//            string password = "Mannatech)%!$";
//            string Company_DB_Name = "mannatech";

//            ////++++++++++++++++++++++++++++++++++++++

//            ////++++++++++++++라이브++++++++++++++++ +
//            cls_app_static_var.app_Company_Name = "menatech";


//            //++++++++++++++++++++++++++++++++++++++
//            //cls_app_static_var.app_Company_Name = "mannatech_JEE";
//            //cls_app_static_var.app_FTP_ID = "melong202";
//            //cls_app_static_var.app_FTP_PW = "rladudtn!&&1";
//            //string User_Id = "ILS_mannatech";
//            ////string password = "ilsong1226_";
//            //Company_DB_Name = "mannatech_J";


//            //cls_app_static_var.app_Company_Name = "mannatech";
//            //cls_app_static_var.app_FTP_ID = "melong202";
//            //cls_app_static_var.app_FTP_PW = "rladudtn!&&1";
//            //User_Id = "ILS_mannatech";
//            //password = "ilsong1226_";
//            //Company_DB_Name = "mannatech_J";


//            cls_Connect_DB.Base_Conn_Str = "server=" + Base_ServerIpAddress + ",10240;database=Promax_Home;user id=sa;password=ilsong#x";
//            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
//            string Tsql;
//            string Connect_IP = "", Zip_IP = "", Z_IP_Daum = "";
//            Tsql = "Select  Co_ip , Up_TF , Z_IP , Z_IP_Daum ";
//            Tsql = Tsql + " From Tbl_Co_Code_2  (nolock)   ";
//            Tsql = Tsql + " Where C_AppPath_Name ='" + cls_app_static_var.app_Company_Name + "'";

//            DataSet ds = new DataSet();

//            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
//            if (Temp_Connect.Open_Data_Set_Base(Tsql, "Tbl_Co_Code", ds) == false) return;
//            int ReCnt = Temp_Connect.DataSet_ReCount;

//            if (ReCnt == 0)
//            {
//                MessageBox.Show("Not Found Base DB info.");
//                return;
//            }
//            else
//            {
//                Connect_IP = ds.Tables["Tbl_Co_Code"].Rows[0]["Co_ip"].ToString();
//                Zip_IP = ds.Tables["Tbl_Co_Code"].Rows[0]["Z_IP"].ToString();
//                Z_IP_Daum = ds.Tables["Tbl_Co_Code"].Rows[0]["Z_IP_Daum"].ToString();
//            }


//            if (Connect_IP == "")
//            {
//                MessageBox.Show("Not Found Base DB info.");
//                return;
//            }
//            //++++++++++++++++++++++++++++++++



//            //218.237.118.12,51433
//            cls_Connect_DB.Conn_Str = "Initial Catalog=" + Company_DB_Name + ";Persist Security Info=True;User ID=" + User_Id + ";Password=" + password + ";Data Source=" + Connect_IP + "";
//            cls_Connect_DB.Return_Conn_Str = "Initial Catalog=mannatech_Return_Close;Persist Security Info=True;User ID=" + User_Id + ";Password=" + password + ";Data Source=" + Connect_IP + "";
//            cls_Connect_DB.Ga_Close_Conn_Str = "Initial Catalog=mannatech_Ga_Close;Persist Security Info=True;User ID=" + User_Id + ";Password=" + password + ";Data Source=" + Connect_IP + "";

//            //cls_Connect_DB.Conn_Str = cls_Connect_DB.Conn_Str + ";Encrypt=true;TrustServerCertificate = true";

//            //cls_Connect_DB.Conn_Str = "server= " + Connect_IP + ",10240;database=" + Company_DB_Name + ";user id=" + User_Id + ";password=" + password;
//            cls_Connect_DB.AddCode_Conn_Str = "server=" + Zip_IP + ",10240;database=Zip_CS;user id=ILS_Zip_CS;password=ilsong#x";
//            cls_Connect_DB.AddCode_Daum_Conn_Str = "server=" + Z_IP_Daum + ",10240;database=ADD_DB;user id=ILS_ADD_1;password=ilsong#x_1";
//            cls_Connect_DB.SMS_Conn_Str = "server=" + Zip_IP + ",10240;database=SMSDB;user id=ANEW_SMS;password=ilsong1226_sms";


//            cls_app_static_var.app_multi_lang_query = 0; //1 이면 멀티 랭귀지임... 0이면 한국어임.

//            cls_app_static_var.Using_Multi_language = 0;  //멀티 랭귀지를 사용한다.

//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new frm_Login());



//            if (cls_User.gid != "" && cls_User.gid != null)
//            {
//                //프로그램에서 자주 변경되는 말들을 미리 정의함. 회원은 고객으로 한다든가 후원은 직대라 한다든가 용
//                //용어를 변경할때.. 관리하기 위함.
//                cls_app_static_var.app_Base_Str_resource = "MLM_Program.Resources.Base_Str_Resource";
//                cls_app_static_var.app_base_str_rm = new ResourceManager(cls_app_static_var.app_Base_Str_resource, cls_app_static_var.Assem);

//                string StrSql = "";

//                if (cls_app_static_var.app_multi_lang_query == 1)
//                {
//                    if (cls_User.gid_CountryCode == "US")
//                    {
//                        cls_app_static_var.User_Time_Zone = "Central Standard Time";
//                    }


//                    ////베이스 폴더에 들어가는 캡션들에 대해서 리소스에서 관리한다
//                    if (cls_User.gid_CountryCode == "KR" || cls_User.gid_CountryCode == "")
//                    {
//                        cls_app_static_var.User_Time_Zone = "Korea Standard Time";
//                    }

//                    if (cls_User.gid_CountryCode == "Ja" || cls_User.gid_CountryCode == "")
//                    {
//                        cls_app_static_var.User_Time_Zone = "Tokyo Standard Time";
//                    }

//                    //if (cls_User.gid_CountryCode == "La")
//                    //{
//                    //    //cls_app_static_var.app_Base_Caption_resource = "MLM_Program.Resources.Laos_Caption_Resource";
//                    //    cls_app_static_var.User_Time_Zone = "SE Asia Standard Time";
//                    //    StrSql = "Select Base_L, Eng_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";

//                    //}

//                    //if (cls_User.gid_CountryCode == "Ja")
//                    //{
//                    //    //cls_app_static_var.app_Base_Caption_resource = "MLM_Program.Resources.Japan_Caption_Resource";
//                    //    cls_app_static_var.User_Time_Zone = "Tokyo Standard Time";
//                    //    StrSql = "Select Base_L, Jap_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";
//                    //}

//                    if (cls_app_static_var.Using_language == "Korean" || cls_app_static_var.Using_language == "")
//                    {
//                        StrSql = "Select Base_L, Kor_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";
//                    }

//                    if (cls_app_static_var.Using_language == "English")
//                    {
//                        StrSql = "Select Base_L, Eng_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";
//                    }

//                    if (cls_app_static_var.Using_language == "Japanese")
//                    {
//                        StrSql = "Select Base_L, Jap_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";
//                    }

//                }
//                else
//                {
//                    cls_app_static_var.User_Time_Zone = "Korea Standard Time";
//                    StrSql = "Select Base_L, Kor_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";
//                }


//                DataSet ds2 = new DataSet();

//                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
//                if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Base_Label", ds2) == false) return;

//                int ReCnt2 = Temp_Connect.DataSet_ReCount;

//                for (int fi_cnt = 0; fi_cnt <= ReCnt2 - 1; fi_cnt++)
//                {
//                    if (ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["T_Label"].ToString() != "")
//                        cls_app_static_var.Base_Label[ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["Base_L"].ToString()] = ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["T_Label"].ToString();
//                    else
//                        cls_app_static_var.Base_Label[ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["Base_L"].ToString()] = ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["Base_L"].ToString();
//                }

//                //try
//                //{

//                //if (cls_app_static_var.Program_Update_FileName != "")
//                //    Application.Run(new frmBase_Update()); 


//                Application.Run(new MDIMain());

//                //}
//                //catch (System.Exception theException)
//                //{
//                //    string errorMessage;
//                //    errorMessage = "Error: ";
//                //    errorMessage = String.Concat(errorMessage, theException.Message);
//                //    errorMessage = String.Concat(errorMessage, " Line: ");
//                //    errorMessage = String.Concat(errorMessage, theException.Source);

//                //    MessageBox.Show(errorMessage, "Error");                
//                //}

//            }
//        }







//    }
//}

////D:/증현용/Demo/MLM_Demo_01/MLM_Demo_01/ResourcesMsg_Resource.Designer.cs
////cls_app_static_var.app_Base_Str_resource.gd
////IResourceReader rr = new ResourceReader();

////IDictionaryEnumerator de = rr.GetEnumerator();

////while (de.MoveNext())
////{
////    Console.WriteLine(de.Key + " : " + de.Value);
////}

////Properties.Resources.
////MessageBox.Show(cls_app_static_var.app_base_str_rm.Length.ToString());
////CharEnumerator ce = cls_app_static_var.app_Base_Str_resource.GetEnumerator();
////while (ce.MoveNext())
////{
////    Console.WriteLine(ce.ToString ());
////}




////System.Reflection.Assembly thisExe;

////thisExe = System.Reflection.Assembly.GetExecutingAssembly();

////string[] resources = thisExe.GetManifestResourceNames();

////string list = "";

////// Build the string of resources.

////foreach (string resource in resources)

////    list += resource + "\r\n";



////Assembly _assembly;
////              _assembly = Assembly.GetExecutingAssembly();
////              StreamReader _textStreamReader;

////              _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("MLM_Program.Resources.Base_Str_Resource.resources"));

////              while (_textStreamReader.Peek() >= 0)
////              {
////                  Console.WriteLine(_textStreamReader.ReadLine() );
////              }




