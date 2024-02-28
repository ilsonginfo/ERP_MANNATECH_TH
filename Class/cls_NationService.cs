using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM_Program
{
    public class cls_NationService
    {
        public static void SQL_BankNationCode(ref string Tsql)
        {
            if (cls_User.gid_CountryCode == "KR" || cls_User.gid_CountryCode == "")
            {
                Tsql += " And tbl_Bank.Na_code = 'KR' ";
            }
            if (cls_User.gid_CountryCode == "TH")
            {
                Tsql += " And tbl_Bank.Na_code = 'KR' ";
            }
        }
    }
}
