using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace MLM_Program
{
    public enum ExcelVersion
    {
        [Description("엑셀2007")]
        Excel2007,
        [Description("엑셀2010")]
        Excel2010,
        [Description("엑셀2013")]
        Excel2013,
        [Description("엑셀2016")]
        Excel2016,
        [Description("엑셀2019")]
        Excel2019,
        [Description("엑셀365")]
        Excel365,
        [Description("엑셀 설치 확인 불가")]
        Unknown
    }

    public class cls_Excel
    {
        /// <summary>
        /// 현재 PC에 설치된 Excel 버전 확인
        /// </summary>
        /// <returns></returns>
        public static ExcelVersion GetInstalledExcelVersion()
        {

            try
            {
                Application excelApp = new Application();
                string excelVersion = excelApp.Version;
                excelApp.Quit();

                // Extract the major version number
                int majorVersion = int.Parse(excelVersion.Split('.')[0]);

                if (majorVersion == 16)
                {
                    return ExcelVersion.Excel2016;
                }
                else if (majorVersion == 15)
                {
                    return ExcelVersion.Excel2013;
                }
                else if (majorVersion == 14)
                {
                    return ExcelVersion.Excel2010;
                }
                else if (majorVersion == 12)
                {
                    return ExcelVersion.Excel2007;
                }
                else
                {
                    return ExcelVersion.Unknown;
                }
            }
            catch (Exception)
            {
                return ExcelVersion.Unknown;
            }

            //string excelRegistryPath = @"Software\Microsoft\Office\";

            //using (RegistryKey key = Registry.LocalMachine.OpenSubKey(excelRegistryPath))
            //{
            //    if (key != null)
            //    {
            //        string[] subKeyNames = key.GetSubKeyNames();
            //        foreach (string subKeyName in subKeyNames)
            //        {
            //            if (subKeyName.StartsWith("Excel"))
            //            {
            //                switch (subKeyName)
            //                {
            //                    case "Excel16":
            //                    case "Excel16.0":
            //                    case "Excel16.0\\Excel":
            //                        return ExcelVersion.Excel2016;
            //                    case "Excel.365":
            //                    case "Excel.365\\Excel":
            //                        return ExcelVersion.Excel365;
            //                    case "Excel.16.0":
            //                    case "Excel.16.0\\Excel":
            //                        return ExcelVersion.Excel2019;
            //                    default:
            //                        return ExcelVersion.Unknown;
            //                }
            //            }
            //        }
            //    }
            //}

            //return ExcelVersion.Unknown;
        }

        /// <summary>
        /// 엑셀 확장자에 해당하는지 확인하는 함수
        /// </summary>
        /// <param name="xlExtension">검사 하려는 확장자</param>
        /// <returns>.xlsx, .xls, .xlsm 인 경우: true // 그 외 : false 리턴</returns>
        public static bool IsValidExcelExtension(string xlExtension)
        {
            string[] xlExtensions = { ".xlsx", ".xls", ".xlsm" };
            return Array.Exists(xlExtensions, ext => ext.Equals(xlExtension, StringComparison.OrdinalIgnoreCase));
        }

    }
}
