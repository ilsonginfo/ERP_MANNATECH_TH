using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Management;

namespace MLM_Program
{
    public partial class frmBase_Update : clsForm_Extends
    {

        private int New_Ver = 0;
        private int Be_Ver = 0;
        private string New_FileName = "";
        private string U_TIP = ""; private string U_TID = ""; private string U_TPW = ""; private string U_Port = "";
        private int Load_TF = 0 ;

        FTP Base_ftp = null;

        public frmBase_Update()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            //New_Ver = 0; New_FileName = ""; Be_Ver = 0;
            //U_TIP = ""; U_TID = ""; U_TPW = ""; U_Port = "";

            //Check_Connect_FPT_Info();

            //if (U_TIP == "")
            //{
            //    MessageBox.Show("UPDATE FTP Info Not Fount.");
            //    this.Close();
            //    return;
            //}

            //string ap_path = Application.StartupPath.ToString();

            //Base_ftp = new FTP("ftp://" + U_TIP + ":" + U_Port, U_TID, U_TPW);
            //Base_ftp.Cancel_TF = 0;
            //Base_ftp.Send_Download_Result += new FTP.Send_Download_Result_Dele(ftp_Send_Download_Result);
            //Base_ftp.Download(tbDD.Text + cls_app_static_var.app_Company_Name, New_FileName, ap_path, progress, true, "Temp_Up_E.dat");

            //FTP ftp = new FTP("ftp://" + U_TIP + ":" + U_Port, U_TID, U_TPW);            
            //ftp.Send_Download_Result += new FTP.Send_Download_Result_Dele(ftp_Send_Download_Ver_Result);
            //ftp.Download(tbDD.Text + cls_app_static_var.app_Company_Name, "NewVer.txt", ap_path, progress, true, "B_NewVer.txt");                                    
        }


        private void Check_Connect_FPT_Info()
        {
            //-----------------------------------------
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;


            Tsql = "Select C_IP,  C_PT , U_IP , U_PT, Z_IP  ";
            Tsql = Tsql + " From Tbl_Co_Code  (nolock)   ";
            Tsql = Tsql + " Where Company_Dir ='" + cls_app_static_var.app_Company_Name + "'";

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set_Base(Tsql, "Tbl_FT", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            
            if (ReCnt == 0)
            {                
                return;
            }
            else
            {
                //U_TIP = ds.Tables["Tbl_FT"].Rows[0]["U_TIP"].ToString();
                //U_TID = ds.Tables["Tbl_FT"].Rows[0]["U_TID"].ToString();
                //U_TPW = ds.Tables["Tbl_FT"].Rows[0]["U_TPW"].ToString();
                //U_Port = ds.Tables["Tbl_FT"].Rows[0]["U_PT1"].ToString();

                U_TIP = ds.Tables["Tbl_FT"].Rows[0]["U_IP"].ToString();
                U_TID = cls_app_static_var.app_FTP_ID;
                U_TPW = cls_app_static_var.app_FTP_PW;
                U_Port = (int.Parse(ds.Tables["Tbl_FT"].Rows[0]["U_PT"].ToString()) / 2).ToString();
            }            
            //-----------------------------------------
        }


        //void ftp_Send_Download_Ver_Result(int D_Result)
        //{
        //    if (D_Result == -1) return;

        //    string ap_path = Application.StartupPath.ToString();

        //    string _sourceFile = Path.Combine(ap_path, "B_NewVer.txt");


        //     FileInfo fileVer = new FileInfo(_sourceFile);
        //    if (fileVer.Exists)
        //    {
        //        FileStream fs = new FileStream(Path.Combine(ap_path, "B_NewVer.txt"), FileMode.Open);
        //        StreamReader Sw = new StreamReader(fs);

        //        New_Ver = int.Parse (Sw.ReadLine().ToString());
        //        New_FileName = Sw.ReadLine().ToString();

        //        Sw.Close();
        //        fs.Close();
                                
        //        fileVer.Delete();   
        //    }
  

 

        //    if (New_FileName != "")
        //       {           
        //           string u_ip, u_computername;
        //           Hard_Number(out u_ip, out  u_computername);

        //           //-----------------------------------------
        //           cls_Connect_DB Temp_Connect = new cls_Connect_DB();
        //           string Tsql;

        //           Tsql = "Select Co_IP,  Co_Name , Up_Name , Up_Ver  ";
        //           Tsql = Tsql + " From Tbl_User_Ver  (nolock)   ";
        //           Tsql = Tsql + " Where Co_IP ='" + u_ip + "'";
        //           Tsql = Tsql + " And   Co_Name ='" + u_computername + "'";

        //           DataSet ds = new DataSet();

        //           //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
        //           if (Temp_Connect.Open_Data_Set(Tsql, "Tbl_User_Ver", ds) == false) return;
        //           int ReCnt = Temp_Connect.DataSet_ReCount;

        //           if (ReCnt == 0)
        //           {
        //               Be_Ver = 0;
        //           }
        //           else 
        //           {
        //               Be_Ver =  int.Parse (ds.Tables["Tbl_User_Ver"].Rows[0]["Up_Ver"].ToString());
        //           }
        //           //-----------------------------------------

        //        }



        //    if (New_FileName != "" && Be_Ver < New_Ver)
        //    {
               
        //    }


        //}


        void ftp_Send_Download_Result(int D_Result)
        {
            if (D_Result == -1)
            {
                this.Close();
                return;
            }

            string ap_path = Application.StartupPath.ToString();
            string _sourceFile = Path.Combine(ap_path, "Temp_Up_E.dat");

            if (Base_ftp.Cancel_TF == 1)
            {
                FileInfo fileDel = new FileInfo(_sourceFile);
                if (fileDel.Exists)
                    fileDel.Delete(); 
                return;
            }
                    

            //현실행파일의 이름을 2~~.exe 로 바꿔버린다. 2를 앞에 붙여서..이름을 바꾼다. 업데이트후 삭제 하기 위함.
            string app_Name = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
            string app_Name2 = Path.GetFileName(app_Name);
            string Chang_app_Name = Path.Combine(ap_path, "2_" + app_Name2);
            FileInfo fileRename = new FileInfo(Path.Combine(ap_path, app_Name));
            if (fileRename.Exists)
            {
                FileInfo fileDel = new FileInfo(Chang_app_Name);
                if (fileDel.Exists)
                    fileDel.Delete(); 

                fileRename.MoveTo(Chang_app_Name); //이미있으면 에러
            }

            //업데이트 내역에 대해서 파일 압축을 푼다.
            if (DeCompression(_sourceFile) == false)
            {
                FileInfo fileRename2 = new FileInfo(Path.Combine(ap_path, Chang_app_Name));
                fileRename2.MoveTo(app_Name); //이미있으면 에러
                MessageBox.Show("UpDate File Zip Error");
                this.Close();
                return;
            }


            //압축 풀고 압축 파일을 삭제를 해버린다.
            FileInfo fileTempUpl = new FileInfo(Path.Combine(ap_path, "Temp_Up_E.dat"));
            if (fileTempUpl.Exists)
                fileTempUpl.Delete(); 



            //실행 파일중에 삭제 해야 될꺼야 새로 실행해야 될 파일의 이름이 저장된 파일이 있는지 체크한다.
            FileInfo fileMUP_E = new FileInfo(Path.Combine(ap_path, "Temp_Up_E.dat"));
            if (fileMUP_E.Exists)
                fileMUP_E.Delete();    


            //삭제해야 하는 실행 파일 명을 알려준다. 그리고 실행해야 하는 파일명도 저장한다.
            FileStream fs = new FileStream(Path.Combine(ap_path, "Temp_Up_E.dat"), FileMode.Create);
            StreamWriter Sw = new StreamWriter(fs);

            Sw.WriteLine(Chang_app_Name);  //삭제해야할 파일명
            Sw.WriteLine(app_Name);        //새로 실행해야할 파일명.     

            Sw.Close();            fs.Close();


            string u_ip, u_computername;
            Hard_Number(out u_ip, out  u_computername);

            //-----------------------------------------
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;

            Tsql = "Select Co_IP,  Co_Name , Up_Name , Up_Ver  ";
            Tsql = Tsql + " From Tbl_User_Ver  (nolock)   ";
            Tsql = Tsql + " Where Co_IP ='" + u_ip + "'";
            Tsql = Tsql + " And   Co_Name ='" + u_computername + "'";

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Tbl_User_Ver", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)
            {
                Tsql = "Insert Into Tbl_User_Ver (Up_Ver , Up_Name , Co_IP , Co_Name ) Values (";
                Tsql = Tsql + New_Ver + ",'" + New_FileName + "','" + u_ip + "','" + u_computername + "') ";

                if (Temp_Connect.Insert_Data( Tsql, "Tbl_User_Ver",this.Name.ToString (), this.Text ) == false) return;

            }
            else 
            {

                Tsql = "Update Tbl_User_Ver Set ";
                Tsql = Tsql + " Up_Ver = " + New_Ver;
                Tsql = Tsql + " ,Up_Name = '" + New_FileName + "'";                
                Tsql = Tsql + " Where Co_IP ='" + u_ip + "'";
                Tsql = Tsql + " And   Co_Name ='" + u_computername + "'";

                if (Temp_Connect.Update_Data(Tsql, this.Name.ToString(), this.Text) == false) return;
            }                    
            //-----------------------------------------




            string exe_file = Path.Combine(ap_path, "PUpDate.exe");

            FileInfo fileUp = new FileInfo(exe_file);
            if (fileUp.Exists)
            {
                Process UserProcess = new Process();

                UserProcess.StartInfo.UseShellExecute = true;

                UserProcess.StartInfo.FileName = exe_file;

                UserProcess.StartInfo.CreateNoWindow = true;

                UserProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                UserProcess.StartInfo.Arguments = "-1"; //argument가 필요없으면 삭제하세요.

                UserProcess.Start();
            }

            this.Close();
            Application.Exit();
        }




        private Boolean   DeCompression(string filename)
        {
            string zipPath = filename;
            Boolean R_TF = false; 
            string extractDir = Environment.CurrentDirectory;

            System.IO.FileStream fs = new System.IO.FileStream(zipPath,
                                                 System.IO.FileMode.Open,
                                         System.IO.FileAccess.Read, System.IO.FileShare.Read);

            ICSharpCode.SharpZipLib.Zip.ZipInputStream zis =
                                    new ICSharpCode.SharpZipLib.Zip.ZipInputStream(fs);

            ICSharpCode.SharpZipLib.Zip.ZipEntry ze;

            try
            {
                while ((ze = zis.GetNextEntry()) != null)
                {
                    if (!ze.IsDirectory)
                    {
                        string fileName = System.IO.Path.GetFileName(ze.Name);

                        string destDir = System.IO.Path.Combine(extractDir,
                                         System.IO.Path.GetDirectoryName(ze.Name));

                        if (false == Directory.Exists(destDir))
                        {
                            System.IO.Directory.CreateDirectory(destDir);
                        }

                        string destPath = System.IO.Path.Combine(destDir, fileName);

                        System.IO.FileStream writer = new System.IO.FileStream(
                                        destPath, System.IO.FileMode.Create,
                                                System.IO.FileAccess.Write,
                                                    System.IO.FileShare.Write);

                        byte[] buffer = new byte[2048];
                        int len;
                        while ((len = zis.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            writer.Write(buffer, 0, len);
                        }

                        writer.Close();
                    }
                }

                R_TF = true;
            }
            catch (Exception)
            {
                R_TF = false ;
            }

            finally
            {
                zis.Close();
                fs.Close();
                
            }

            return R_TF;
        }




        private void Hard_Number(out string u_ip, out string u_computername)
        {
            u_ip = "";
            u_computername = "";

            u_ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0].ToString();

            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            //ManagementClass mc = new ManagementClass("Win32_DiskDrive");

            ManagementObjectCollection mOC = mc.GetInstances();

            foreach (ManagementObject mo in mOC)
            {
                u_computername = mo["name"].ToString();

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Base_ftp.Cancel_TF = 1;
            this.Close();
        }

        private void frmBase_Update_Activated(object sender, EventArgs e)
        {
            if (Load_TF == 0)
            {
                Load_TF = 1;
                New_FileName = cls_app_static_var.Program_Update_FileName ;
                New_Ver = cls_app_static_var.Program_Update_NewVer ; 

                Be_Ver = 0;
                U_TIP = ""; U_TID = ""; U_TPW = ""; U_Port = "";

                Check_Connect_FPT_Info();

                if (U_TIP == "")
                {
                    MessageBox.Show("업데이트 정보를 찾을수 없습니다.");
                    this.Close();
                    return;
                }

                string ap_path = Application.StartupPath.ToString();

                Base_ftp = new FTP("ftp://" + U_TIP + ":" + U_Port, U_TID, U_TPW);
                Base_ftp.Cancel_TF = 0;
                Base_ftp.Send_Download_Result += new FTP.Send_Download_Result_Dele(ftp_Send_Download_Result);
                //Base_ftp.Download(tbDD.Text + cls_app_static_var.app_Company_Name, New_FileName, ap_path, progress, true, "Temp_Up_E.dat");
                Base_ftp.Download("WebPro_Update/" + cls_app_static_var.app_Company_Name, New_FileName, ap_path, true, "Temp_Up_E.dat");
                
            }
        }

        private void frmBase_Update_Load(object sender, EventArgs e)
        {
            Load_TF = 0;
           
        }




    }
}
