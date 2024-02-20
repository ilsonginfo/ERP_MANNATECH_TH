using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM_Program
{
    class clsCommissionDetail : IDisposable
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

        ~clsCommissionDetail()
        {
            Dispose(false);
        }
        public clsCommissionDetail()
        {
           
        }


        public string ProductionMonth { get; set; }
        public int AccountNumber { get; set; }
        public int Seq { get; set; }
        public string CommissionType { get; set; }

        public string Description { get; set; }
        public int PaidOnAccount { get; set; }
        public string PaidOnCountryCode { get; set; }
        public string PaidOnAccountName { get; set; }
        public int PhysicalLevel { get; set; }
        public int PayLevel { get; set; }
        public int Volume { get; set; }
        public double CommissionPercent { get; set; }
        public double CommissionAmount { get; set; }
        public string OrderNumber { get; set; }

        public string SuccessYn { get; set; }
        public string ErrorDc { get; set; }



    }
}
