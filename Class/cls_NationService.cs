using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM_Program
{
    public static class cls_NationService
    {
        public static void SQL_BankNationCode(ref string Tsql)
        {
            if (cls_User.gid_CountryCode == "KR" || cls_User.gid_CountryCode == "")
            {
                Tsql += " And tbl_Bank.Na_code = 'KR' ";
            }
            else if (cls_User.gid_CountryCode == "TH")
            {
                Tsql += " And tbl_Bank.Na_code = 'TH' ";
            }
        }

        public static void SQL_Memberinfo_NationCode(ref string Tsql)
        {
            if (cls_User.gid_CountryCode == "KR" || cls_User.gid_CountryCode == "")
            {
                Tsql += " AND (tbl_Memberinfo.Na_Code IN ('KR', '' ) OR tbl_Memberinfo.Na_Code IS NULL) ";
            }
            else if (cls_User.gid_CountryCode == "TH")
            {
                Tsql += " AND (tbl_Memberinfo.Na_Code = 'TH' OR Nation_Code = 'TH') ";
            }
        }

        public static void SQL_User_NationCode(ref string Tsql)
        {
            if (cls_User.gid_CountryCode == "KR" || cls_User.gid_CountryCode == "")
            {
                Tsql += " AND Na_Code IN ('KR', '') ";
            }
            else if (cls_User.gid_CountryCode == "TH")
            {
                Tsql += " AND Na_Code IN ('TH') ";
            }
        }

        public static void SQL_SalesDetail_NationCode(ref string Tsql)
        {
            if (cls_User.gid_CountryCode == "KR" || cls_User.gid_CountryCode == "")
            {
                Tsql += " AND (tbl_SalesDetail.Na_Code IN ('KR', '') OR tbl_SalesDetail.Na_Code IS NULL) ";
            }
            else if (cls_User.gid_CountryCode == "TH")
            {
                Tsql += " AND tbl_SalesDetail.Na_Code IN ('TH') ";
            }
        }

        /// <summary>
        /// 국가 코드 추가하는 함수.
        /// </summary>
        /// <param name="Tsql">현재 SQL string -> return 되는 값임.</param>
        /// <param name="sTableAlias">연결할 테이블 명</param>
        /// <param name="sPreviousTsql">SQL 연결 전 연결할 내용 default value: ""</param>
        /// <param name="bNullIncludeKR">KR에서 Null 및 ''을 포함하는지 유무 설정.</param>
        public static void SQL_NationCode(ref string Tsql, string sTableAlias, string sPreviousTsql = "", bool bNullIncludeKR = false)
        {
            if (cls_User.gid_CountryCode == "KR" || cls_User.gid_CountryCode == "")
            {
                if (bNullIncludeKR == false)
                {
                    // Tsql += [SQL 연결 전 연결할 내용] + [연결할 테이블 명]. + [Na_Code] = 'XX'
                    Tsql += (sTableAlias == "") ? string.Format("{0} Na_Code = 'KR' ", sPreviousTsql) : string.Format("{0} {1}.Na_Code = 'KR'", sPreviousTsql, sTableAlias);
                }
                else
                {
                    // Tsql += [SQL 연결 전 연결할 내용] + [연결할 테이블 명]. + [Na_Code] = 'XX'
                    Tsql += (sTableAlias == "") ? string.Format("{0} (Na_Code IN ('KR', '') OR Na_Code IS NULL) ", sPreviousTsql) : string.Format("{0} ({1}.Na_Code IN ('KR', '') OR {1}.Na_Code IS NULL ) ", sPreviousTsql, sTableAlias);
                }
            }
            else if (cls_User.gid_CountryCode == "TH")
            {
                // Tsql += [SQL 연결 전 연결할 내용] + [연결할 테이블 명]. + [Na_Code] = 'XX'
                Tsql += (sTableAlias == "") ? string.Format("{0} Na_Code = 'TH' ", sPreviousTsql) : string.Format("{0} {1}.Na_Code = 'TH'", sPreviousTsql, sTableAlias);
            }
        }

        /// <summary>
        /// 국가 코드 추가하는 함수.
        /// </summary>
        /// <param name="Tsql">현재 SQL string -> return 되는 값임.</param>
        /// <param name="sTableAlias">연결할 테이블 명</param>
        /// <param name="sPreviousTsql">SQL 연결 전 연결할 내용 default value: ""</param>
        /// <param name="bNullIncludeKR">KR에서 Null 및 ''을 포함하는지 유무 설정.</param>
        public static void SQL_NationCode(ref StringBuilder sb, string sTableAlias, string sPreviousTsql = "", bool bNullIncludeKR = false)
        {
            if (cls_User.gid_CountryCode == "KR" || cls_User.gid_CountryCode == "")
            {
                if (bNullIncludeKR == false)
                {
                    // Tsql += [SQL 연결 전 연결할 내용] + [연결할 테이블 명]. + [Na_Code] = 'XX'
                    sb.AppendLine((sTableAlias == "") ? string.Format("{0} Na_Code = 'KR' ", sPreviousTsql) : string.Format("{0} {1}.Na_Code = 'KR'", sPreviousTsql, sTableAlias));
                }
                else
                {
                    // Tsql += [SQL 연결 전 연결할 내용] + [연결할 테이블 명]. + [Na_Code] = 'XX'
                    sb.AppendLine((sTableAlias == "") ? string.Format("{0} (Na_Code IN ('KR', '') OR Na_Code IS NULL) ", sPreviousTsql) : string.Format("{0} ({1}.Na_Code IN ('KR', '') OR {1}.Na_Code IS NULL ) ", sPreviousTsql, sTableAlias));
                }
            }
            else if (cls_User.gid_CountryCode == "TH")
            {
                // Tsql += [SQL 연결 전 연결할 내용] + [연결할 테이블 명]. + [Na_Code] = 'XX'
                sb.AppendLine((sTableAlias == "") ? string.Format("{0} Na_Code = 'TH' ", sPreviousTsql) : string.Format("{0} {1}.Na_Code = 'TH'", sPreviousTsql, sTableAlias));
            }
        }

        /// <summary>
        /// 확장 메서드 - gid_CountryCode 가 "" 또는 Null 인 경우 KR 로 자동 지정.
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public static string GetCountryCodeOrDefault(this string countryCode)
        {
            return string.IsNullOrEmpty(countryCode) ? "KR" : countryCode;
        }

    }
}
