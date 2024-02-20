using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ExcelDataReader;


namespace MLM_Program
{
    class clsExcel : IDisposable
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

        ~clsExcel()
        {
            if (Excel_ds != null)
                Excel_ds = null;
            
            Dispose(false);
        }
        public clsExcel()
        {
            Excel_ds = null;
            
        }

        public clsExcel(string strExcelFile)
        {
            Excel_ds = Read_Excel(strExcelFile);
        }


        public DataTableCollection Excel_ds { get; set; }
        
        public DataTableCollection Read_Excel(string strExcelFile)
        {
            DataTableCollection ds = null;

            using (var stream = File.Open(strExcelFile, FileMode.Open, FileAccess.Read))
            {
                if (Path.GetExtension(strExcelFile).ToUpper() == ".XLS" || Path.GetExtension(strExcelFile).ToUpper() == ".XLSX")
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                EmptyColumnNamePrefix = "Column",
                                UseHeaderRow = true
                            }
                        });

                        ds = result.Tables;
                    }
                }
            }

            return ds;
        }


    }
}
