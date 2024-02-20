using System;
using System.Net;
using System.Windows;
using System.Windows.Forms ;

namespace MLM_Program
{
    class FTP
    {
        public delegate void Send_Download_Result_Dele(int D_Result);
        public event Send_Download_Result_Dele Send_Download_Result;

        private string hostName, userName, password;
        private ProgressBar progressBar;
        private long fileSize;
        public long Cancel_TF = 0 ;

        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public FTP() : this(string.Empty, string.Empty, string.Empty) { }

        public FTP(string hostName, string userName, string password)
        {
            this.hostName = hostName;
            this.userName = userName;
            this.password = password;
        }

        /// <summary>
        /// 일반 다운로드
        /// </summary>
        /// <param name="ftpFolderName">다운로드 할 ftp 폴더 이름</param>
        /// <param name="downFileName">다운로드 할 파일 이름</param>
        /// <param name="localFolderName">저장할 경로</param>
        public void Download(string ftpDirectoryName, string downFileName, string localPath, string Result_Name)
        {
            try
            {
                Uri ftpUri = new Uri(hostName + "/" + ftpDirectoryName + "/" + downFileName);

                using (WebClient request = new WebClient())
                {
                    request.Credentials = new NetworkCredential(userName, password);
                    request.DownloadFile(ftpUri, localPath + @"/" + Result_Name);

                    request.DownloadFileCompleted += request_DownloadFileCompleted;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 다운로드 진행률 보여주기
        /// </summary>
        /// <param name="ftpDirectoryName">다운로드 할 ftp 폴더 이름</param>
        /// <param name="downFileName">다운로드 할 파일 이름</param>
        /// <param name="localPath">저장할 경로</param>
        /// <param name="progress">프로그래스 바</param>
        /// <param name="showCompleted">완료 메시지 보이기</param>
        public void Download(string ftpDirectoryName, string downFileName, string localPath,  bool showCompleted, string Result_Name)
        {
            try
            {
                
                Uri ftpUri = new Uri(hostName + "/" + ftpDirectoryName + "/" + downFileName);

                // 파일 사이즈
                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(ftpUri);
                reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFtp.Credentials = new NetworkCredential(userName, password);
                FtpWebResponse resFtp = (FtpWebResponse)reqFtp.GetResponse();
                fileSize = resFtp.ContentLength;
                resFtp.Close();

                using (WebClient request = new WebClient())
                {
                    request.Credentials = new NetworkCredential(userName, password);                    


                    // 다운로드가 완료 된 후 메시지 보이기
                    if (showCompleted)
                    {
                        request.DownloadFileCompleted += request_DownloadFileCompleted;
                    }

                    // 다운로드 시작
                    //request.DownloadFileAsync(ftpUri, @localPath + "/" + "Temp_Up_E.dat");
                    request.DownloadFileAsync(ftpUri, @localPath + "/" + Result_Name);

                }
            }

            catch (Exception ee)
            {
                //MessageBox.Show(ee.ToString ());
                Send_Download_Result(-1);
                if (ee.Message != "원격 서버에 연결할 수 없습니다.")
                    MessageBox.Show("Program Update error!!");
                
                return;
            }
        }



        /// <summary>
        /// 다운로드 진행률 보여주기
        /// </summary>
        /// <param name="ftpDirectoryName">다운로드 할 ftp 폴더 이름</param>
        /// <param name="downFileName">다운로드 할 파일 이름</param>
        /// <param name="localPath">저장할 경로</param>
        /// <param name="progress">프로그래스 바</param>
        /// <param name="showCompleted">완료 메시지 보이기</param>
        public void Download(string ftpDirectoryName, string downFileName, string localPath, ProgressBar progressBar, bool showCompleted, string Result_Name)
        {
            try
            {
                this.progressBar = progressBar;
                Uri ftpUri = new Uri(hostName + "/" + ftpDirectoryName + "/" + downFileName);
                
                // 파일 사이즈
                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(ftpUri);
                reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFtp.Credentials = new NetworkCredential(userName, password);
                FtpWebResponse resFtp = (FtpWebResponse)reqFtp.GetResponse();
                fileSize = resFtp.ContentLength;
                resFtp.Close();

                using (WebClient request = new WebClient())
                {
                    request.Credentials = new NetworkCredential(userName, password);
                    request.DownloadProgressChanged += request_DownloadProgressChanged;
                    

                    // 다운로드가 완료 된 후 메시지 보이기
                    if (showCompleted)
                    {
                        request.DownloadFileCompleted += request_DownloadFileCompleted;
                    }

                    // 다운로드 시작
                    //request.DownloadFileAsync(ftpUri, @localPath + "/" + "Temp_Up_E.dat");
                    request.DownloadFileAsync(ftpUri, @localPath + "/" + Result_Name);
                    
                }
            }

            catch (Exception ee)
            {
                Send_Download_Result(-1);
                if (ee.Message != "원격 서버에 연결할 수 없습니다.")
                    MessageBox.Show("Program UpDate Error!!");
                return;
            }
        }




        void request_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            WebClient request = (WebClient)sender;

            progressBar.Value = Convert.ToInt32(Convert.ToDouble(e.BytesReceived) / Convert.ToDouble(fileSize) * 100);

            if (Cancel_TF == 1)
                request.CancelAsync();
        }


        void request_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Send_Download_Result(1);            
        }



        //public void Download(string ftpDirectoryName, string downFileName, string localPath,  bool showCompleted, string Result_Name)
        //{
        //    try
        //    {                
        //        Uri ftpUri = new Uri(hostName + "/" + ftpDirectoryName + "/" + downFileName);

        //        // 파일 사이즈
        //        FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(ftpUri);
        //        reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;
        //        reqFtp.Credentials = new NetworkCredential(userName, password);
        //        FtpWebResponse resFtp = (FtpWebResponse)reqFtp.GetResponse();
        //        fileSize = resFtp.ContentLength;
        //        resFtp.Close();

        //        using (WebClient request = new WebClient())
        //        {
        //            request.Credentials = new NetworkCredential(userName, password);                    


        //            // 다운로드가 완료 된 후 메시지 보이기
        //            if (showCompleted)
        //            {
        //                request.DownloadFileCompleted += request_DownloadFileCompleted;
        //            }

        //            // 다운로드 시작
        //            //request.DownloadFileAsync(ftpUri, @localPath + "/" + "Temp_Up_E.dat");
        //            request.DownloadFileAsync(ftpUri, @localPath + "/" + Result_Name);

        //        }
        //    }

        //    catch (Exception ee)
        //    {
        //        Send_Download_Result(-1);
        //        if (ee.Message != "원격 서버에 연결할 수 없습니다.")
        //            MessageBox.Show("Program UpDate Error!!");
        //        return;
        //    }
        //}


    }
}
