using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class MenaCSFunction
{
    //[Microsoft.SqlServer.Server.SqlFunction]
    //public static SqlString SQLFunction()
    //{
    //    // 여기에 코드를 입력합니다.
    //    return new SqlString (string.Empty);
    //}


    [Microsoft.SqlServer.Server.SqlFunction]
    public static string GetCtyState(string addr1)
    {
        string rc2Str = "";
        string rc7Str = "";
        string replaceStr = "";
        string regionCode = "";

        if(addr1.Length >= 2 )
            rc2Str = addr1.Substring(0, 2);

        if (addr1.Length >= 7)
            rc7Str = addr1.Substring(0, 7);

        if ((rc2Str.IndexOf("부산")) > -1 || (rc7Str.IndexOf("부산광역시")) > -1)
        {
            replaceStr = "부산";
            regionCode = "BUS";
        }
        else if ((rc2Str.IndexOf("충북")) > -1 || (rc7Str.IndexOf("충청북도")) > -1)
        {
            replaceStr = "충북";
            regionCode = "CCB";
        }
        else if ((rc2Str.IndexOf("충남")) > -1 || (rc7Str.IndexOf("충청남도")) > -1)
        {
            replaceStr = "충남";
            regionCode = "CCN";
        }
        else if ((rc2Str.IndexOf("대구")) > -1 || (rc7Str.IndexOf("대구광역시")) > -1)
        {
            replaceStr = "대구";
            regionCode = "DAG";
        }
        else if ((rc2Str.IndexOf("대전")) > -1 || (rc7Str.IndexOf("대전광역시")) > -1)
        {
            replaceStr = "대전";
            regionCode = "DAJ";
        }
        else if ((rc2Str.IndexOf("강원")) > -1 || (rc7Str.IndexOf("강원도")) > -1)
        {
            replaceStr = "강원";
            regionCode = "GAN";
        }
        else if ((rc2Str.IndexOf("경북")) > -1 || (rc7Str.IndexOf("경상북도")) > -1)
        {
            replaceStr = "경북";
            regionCode = "GSB";
        }
        else if ((rc2Str.IndexOf("경남")) > -1 || (rc7Str.IndexOf("경상남도")) > -1)
        {
            replaceStr = "경남";
            regionCode = "GSN";
        }
        else if ((rc2Str.IndexOf("인천")) > -1 || (rc7Str.IndexOf("인천광역시")) > -1)
        {
            replaceStr = "인천";
            regionCode = "INC";
        }
        else if ((rc2Str.IndexOf("전북")) > -1 || (rc7Str.IndexOf("전라북도")) > -1)
        {
            replaceStr = "전북";
            regionCode = "JLB";
        }
        else if ((rc2Str.IndexOf("제주")) > -1 || (rc7Str.IndexOf("제주특별자치")) > -1)
        {
            replaceStr = "제주";
            regionCode = "JEJ";
        }
        else if ((rc2Str.IndexOf("전남")) > -1 || (rc7Str.IndexOf("전라남도")) > -1)
        {
            replaceStr = "전남";
            regionCode = "JLN";
        }
        else if ((rc2Str.IndexOf("경기")) > -1 || (rc7Str.IndexOf("경기도")) > -1)
        {
            replaceStr = "경기";
            regionCode = "GYE";
        }
        else if ((rc2Str.IndexOf("광주")) > -1 || (rc7Str.IndexOf("광주광역시")) > -1)
        {
            replaceStr = "광주";
            regionCode = "GWA";
        }
        else if ((rc2Str.IndexOf("서울")) > -1 || (rc7Str.IndexOf("서울특별시")) > -1)
        {
            replaceStr = "서울";
            regionCode = "SEO";
        }
        else if ((rc2Str.IndexOf("세종")) > -1 || (rc7Str.IndexOf("세종특별자치")) > -1)
        {
            replaceStr = "세종";
            regionCode = "CCN";//"SJ";
        }
        else if ((rc2Str.IndexOf("울산")) > -1 || (rc7Str.IndexOf("울산광역시")) > -1)
        {
            replaceStr = "울산";
            regionCode = "ULS";
        }

        return regionCode;
    }

    public static string GetCtyCity(string addr1)
    {
        string ret = "";
        string[] addrs = addr1.Split(' ');

        foreach(string addr in addrs)
        {

            if (addr.LastIndexOf("구") > 0)
            {
                ret = addr;
                break;
            }
            if (addr.LastIndexOf("군") > 0)
            {
                ret = addr;
                break;
            }
            if (addr.LastIndexOf("동") > 0)
            {
                ret = addr;
                break;
            }
            if (addr.LastIndexOf("읍") > 0)
            {
                ret = addr;
                break;
            }
            if (addr.LastIndexOf("면") > 0)
            {
                ret = addr;
                break;
            }
        }

        return ret;
    }


}

