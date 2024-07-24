using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM_Program
{
    /// <summary>
    /// 이미지 업로드시 받는 json
    /// </summary>
    class clsCarImg_Up : IDisposable
    {
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                disposed = true;
            }
        }

        ~clsCarImg_Up()
        {
            Dispose(false);
        }
        public clsCarImg_Up()
        {



        }

        public string successYN { get; set; }
        public List<FileList> fileList { get; set; }
    }

    public class FileList
    {
        public int beginRow { get; set; }
        public int blockCnt { get; set; }
        public string delFilesSeq { get; set; }
        public int endRow { get; set; }
        public string fileSeq { get; set; }
        public int fileSize { get; set; }
        public string gubun1 { get; set; }
        public string gubun2 { get; set; }
        public string inputName { get; set; }
        public string isCreateThumViewForEditor { get; set; }
        public string isEditorYn { get; set; }
        public List<object> itemArr { get; set; }
        public object modDate { get; set; }
        public string modId { get; set; }
        public string mode { get; set; }
        public string nation { get; set; }
        public string netfunnelKey { get; set; }
        public string noContextRequestUriPath { get; set; }
        public string orgFileNm { get; set; }
        public string orgSeq { get; set; }
        public int page { get; set; }
        public string prefixPath { get; set; }
        public object regDate { get; set; }
        public string regId { get; set; }
        public string reqGubun { get; set; }
        public int rows { get; set; }
        public string schField { get; set; }
        public string schString { get; set; }
        public string schUseYn { get; set; }
        public int sortNo { get; set; }
        public string subDir { get; set; }
        public string thumKey { get; set; }
        public string thumListFileNm { get; set; }
        public string thumViewFileNm { get; set; }
        public int totalCnt { get; set; }
        public int totalPageCnt { get; set; }
        public string uploadFileNm { get; set; }
        public string uploadFullPath { get; set; }
        public string uploadPath { get; set; }
    }


}
