using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace MLM_Program
{
    public partial class frmBase_Member_Web_IMg : clsForm_Extends
    {
       

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        cls_Grid_Base cgb = new cls_Grid_Base();
        private const string base_db_name = "tbl_Memberinfo";

        //더블 클릭한 내역을 호출한 폼으로 다시 돌려 보내기 위한 델리케이트
        //public delegate void SendNumberDele(string Send_Number, string Send_Name);
        //public event SendNumberDele Send_Mem_Number;


        //public delegate void Send_Search_Mem_Number_Info_Dele(ref string searchMbid, ref int searchMbid2);
        //public event Send_Search_Mem_Number_Info_Dele Send_MemNumber_Info;

        public delegate void Call_searchNumber_Info_Dele(ref string searchMbid, ref int searchMbid2, ref string searchName);
        public event Call_searchNumber_Info_Dele Call_searchNumber_Info;

                

        private string Search_Member_Number_Mbid;
        private int Search_Member_Number_Mbid2;
        private string Search_Member_Name;
        private string Search_Member_Name_KR;

        private string Search_Gubun_2;
        //private Image img;
        private string sFileName;

        /// <summary>
        /// IDCARD = 신분증 BANKBOOK = 통장사본
        /// </summary>
        private string strJOb = string.Empty;
        


        public frmBase_Member_Web_IMg()
        {
            InitializeComponent();
        }
        public frmBase_Member_Web_IMg(string strJob,int mbid2)
        {
            InitializeComponent();
            this.strJOb = strJob;
            this.Search_Member_Number_Mbid2 = mbid2;
        }



        private void frmBase_From_Load(object sender, EventArgs e)
        {

            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Search);


            //Search_Member_Number_Mbid = ""; Search_Member_Number_Mbid2 = 0;
            //Search_Member_Name = ""; Search_Member_Name_KR = "";

            //Call_searchNumber_Info(ref Search_Member_Number_Mbid, ref Search_Member_Number_Mbid2, ref Search_Member_Name);


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string StrSql = ""; 
            //if (Search_Member_Name == "")
            //    StrSql = "select UPLOAD_PATH + UPLOAD_FILE_NM from TLS_FILE with (nolock)";
            //else
                StrSql = "select TOP 1 UPLOAD_PATH + UPLOAD_FILE_NM  as T_FileDir ,UPLOAD_FILE_NM from TLS_FILE with (nolock) ";
            StrSql = StrSql + " where  GUBUN_1 = 'MEMBER' ";
            StrSql = StrSql + $" and GUBUN_2 = '{this.strJOb}' ";     // BANKBOOK

            //StrSql = StrSql + "    AND REG_ID = (SELECT WEBID FROM TBL_MEMBERINFO WHERE MBID2 = " + Search_Member_Number_Mbid2 + ") ORDER BY REG_DATE DESC";
            StrSql = StrSql + "    AND ORG_SEQ = " + Search_Member_Number_Mbid2 + " ORDER BY FILE_SEQ DESC";
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Memberinfo", ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)
            {
                this.Close(); 
                return;
            }
            //++++++++++++++++++++++++++++++++                        

            //string Cpno = decrypter.Decrypt(ds.Tables[base_db_name].Rows[0]["Cpno"].ToString());
            
                        
            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);
            string T_FileDir = ds.Tables[base_db_name].Rows[0]["T_FileDir"].ToString();

            sFileName = ds.Tables[base_db_name].Rows[0]["UPLOAD_FILE_NM"].ToString();

            Debug.WriteLine(sFileName);

            //string t_url = "https://www.applicant.im/uImage" + T_FileDir ;
#if DEBUG
            //string t_url = "https://www.mannatech.co.th/uImage" + T_FileDir;    // live 버전. 
                                                                                string t_url = "https://uat.mannatech.co.th/uImage" + T_FileDir;    // uat 버전. 
#else
            string t_url = "https://www.mannatech.co.th/uImage" + T_FileDir;    // live 버전. 
            //string t_url = "https://uat.mannatech.co.th/uImage" + T_FileDir;    // uat 버전. 
#endif


            Image oImage = GetUrlImage(t_url);



            if (oImage != null)
            {
                Bitmap bitmap = new Bitmap(GetUrlImage(t_url));
                this.pictureBox1.Image = bitmap;
            }
            else
            {
                this.Close();
            }

            
            //webBrowser1.Navigate(t_url);

            
            
                                   
        }



        private Image GetUrlImage(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    byte[] imgArray;
                    imgArray = client.DownloadData(url);
                    using (MemoryStream memstr = new MemoryStream(imgArray))
                    {
                        //img = Image.FromStream(memstr);
                        return Image.FromStream(memstr);
                    }
                }
            }
            catch (WebException wex)
            {
                if (cls_User.gid_CountryCode == "TH")
                {
                    MessageBox.Show("Image loading failed.");
                }
                else
                {

                    MessageBox.Show("이미지 불러오기가 실패하였습니다.");
                }


                
                
                return null;
            }


        }


        private void butt_leftTurn_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);
            pictureBox1.Image = fnRotateImage(bitmap, +90);
            pictureBox1.Invalidate();
        }

        private void butt_rightTurn_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);
            pictureBox1.Image = fnRotateImage(bitmap, -90);
            pictureBox1.Invalidate();
        }



        private Bitmap fnRotateImage(Bitmap b, float angle)
        {
            try
            {
                Bitmap returnBitmap = new Bitmap(b.Height, b.Width);
                Graphics g = Graphics.FromImage(returnBitmap);
                g.TranslateTransform((float)returnBitmap.Width / 2, (float)returnBitmap.Height / 2);
                g.RotateTransform(angle);

                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                g.DrawImage(b, new Point(0, 0));

                return returnBitmap;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("img turn failed.. [" + ex.Message + "]");
                MessageBox.Show("이미지 회전이 실패하였습니다.");
                return null;
            }

        }





        private void butt_imgSave_Click(object sender, EventArgs e)
        {


#if DEBUG
            string t_url = "https://uat.mannatech.co.th/common/cs/uploadFile.do";    // uat 버전. 
            

#else
                    string t_url = "https://www.mannatech.co.th/common/cs/uploadFile.do";    // live 버전. 
                    //string t_url = "https://uat.mannatech.co.th/common/cs/uploadFile.do";    // uat 버전. 
#endif

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            try
            {

                Bitmap bitmap = new Bitmap(pictureBox1.Image);
                byte[] data;
                MemoryStream ms = new MemoryStream();
                using (MemoryStream m = new MemoryStream())
                {
                    m.Position = 0;
                    bitmap.Save(m, ImageFormat.Jpeg);
                    bitmap.Dispose();
                    data = m.ToArray();
                    ms = new MemoryStream(data);
                    // Upload ms
                }

                //bitmap.Save(ms, ImageFormat.Jpeg);
                long fileSizeInBytes = ms.Length;

                if (fileSizeInBytes / 1024 / 1024 > 10)
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Image upload size exceeded (10MB).");
                    }
                    else
                    {

                        MessageBox.Show("이미지 업로드 사이즈를 초과하였습니다.(10MB)");
                    }

                    
                    return;
                }

                //sFileName = sFileName.Replace("_", "");

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                    | SecurityProtocolType.Tls11
                                    | SecurityProtocolType.Tls12
                                    | SecurityProtocolType.Ssl3;

                string result = RequestHelper.PostMultipart(t_url, new Dictionary<string, object>()
                        {
                            {
                                //"uploadFile0", new FormFile()
                                sFileName, new FormFile()
                                {
                                    Name = sFileName, // 보내지는 파일명
                                    ContentType = "application/pdf",  // 파일 타입
                                    Stream = ms  // 로컬파일경로
                                }
                            }
                        }
                );



                clsCarImg_Up oRoot = JsonConvert.DeserializeObject<clsCarImg_Up>(result);



                if (oRoot.successYN == "Y")
                {

                    string StrSql = $" UPDATE TLS_FILE SET ";
                    StrSql += $" UPLOAD_FILE_NM = '{oRoot.fileList[0].uploadFileNm}',";
                    StrSql += $" FILE_SIZE = '{oRoot.fileList[0].fileSize}',";
                    StrSql += $" UPLOAD_PATH = '/member/{DateTime.Now.ToString("yyyyMMdd")}/'";
                    StrSql = StrSql + " where  GUBUN_1 = 'MEMBER' ";
                    StrSql = StrSql + $" and GUBUN_2 = '{this.strJOb}' ";     // BANKBOOK
                    StrSql = StrSql + $"  AND ORG_SEQ = '{Search_Member_Number_Mbid2}' ";


                    if (Temp_Connect.Update_Data(StrSql, this.Name.ToString(), this.Text) == false) return;

                    
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Image upload has been completed.");
                    }
                    else
                    {

                        MessageBox.Show("이미지 업로드를 완료하였습니다.");
                    }
                }
                else
                {
                    if (cls_User.gid_CountryCode == "TH")
                    {
                        MessageBox.Show("Image upload failed.");
                    }
                    else
                    {

                        MessageBox.Show("이미지 업로드를 실패 했습니다.");
                    }

                    
                }


            }
            catch (WebException wex)
            {

                Debug.WriteLine(wex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("File upload failed.. [" + ex.Message + "]");
            }
        }




    }
}
