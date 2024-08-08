using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM_Program
{
    class clsOutput : IDisposable
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

        ~clsOutput()
        {
            Dispose(false);
        }
        public clsOutput()
        {

        }

        public clsOutput(string Form, string Function, int Type, string Message)
        {
            this.Compny = cls_app_static_var.app_Company_Name;
            this.User = $"{cls_User.gid}-{cls_User.computer_net_name}";
            this.Form = Form;
            this.Function = Function;
            this.Type = Type;
            this.Message = Message;
            this.PovasVer = cls_app_static_var.APP_VER;
#if DEBUG
            this.PovasMode = 0;
#else
            this.PovasMode = 1;
#endif

        }

        public string Compny { get; set; }

        public string User { get; set; }
        public string Form { get; set; }
        public string Function { get; set; }
        /// <summary>
        /// 0=에러로그 1=MPM통신로그 2=API전송로그 
        /// </summary>
        public int Type { get; set; }
        public string Message { get; set; }
        public string PovasVer { get; set; }
        public int PovasMode { get; set; }


    }
}

