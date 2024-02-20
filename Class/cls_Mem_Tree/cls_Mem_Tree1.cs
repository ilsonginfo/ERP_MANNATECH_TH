using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MLM_Program
{
    class cls_Mem_Tree
    {
        public string Mbid;
        public string IDKey;
        public string KeyName;
        public string ParentKey;
        public cls_Mem_Tree ParentClass;
        public int SelfNumber;
        public int FontSizes;

        public int Cur;
        public int Left;
        public int Top;

        public int Width;
        public int Height;

        public int ChildCount;
        public int NextDataNum;
        public Dictionary<int, cls_Mem_Tree> ChildNumber;

        public string CpNumber;
        public string MbidName;
        public string RegDate;
        public string SellDate;

        public string Nominid;
        public string NominName;

        public string BusName;
        public string LeaveCheck;
        public string TotalPV;
        public double  f_TotalPV;
        public string TotalPrice;
        public double f_TotalPrice;

        public string ShamPV;
        public string UpLineKey;

        public string Grade1;
        public int Grade2;
        public string Grade3;
        public string TDownPV;
        public double f_TDownPV;

        public string TDownPrice;
        public double f_TDownPrice;

        public Boolean VisibleTF;
        public Boolean VisibleTF2;
        public Boolean ExpensionTF;

        public int BaseDataCount;
        public System.Drawing.Color BackColor;
        public int ForColor;
        public int ParentNumber; 

        public int BaseTop;
        public int BaseLeft;
        public int Lvl;




        public string Chk_Print;



        public string Grade_P;
        public string Sell_Mem_TF;
        public string Down_Sobi_PV;
        public string ClassP_Date;
        //public int Sellcnt;                     
    }


    class cls_Tree_Line
    {

        public int X1;
        public int X2;
        public int Y1;
        public int Y2;

        public int BX1;
        public int BX2;
        public int BY1;
        public int BY2;

        public int UPLebel;
        public int DownLebel;
        public int HLine;
        public Boolean  VisibleTF ;

    }


    class cls_Mem_TreeView
    {
        public string Mbid;
        public string IDKey;
        public string KeyName;
        public string ParentKey;
        public cls_Mem_TreeView ParentClass;
        public int SelfNumber;
                
        public int Left;
        public int Top;

        public int Width;
        public int Height;

        public int ChildCount;
        public int NextDataNum;
        public Dictionary<int, cls_Mem_TreeView> ChildNumber;
                
        public int BaseTop;
        public int BaseLeft;
        public int Lvl;        
    }


}
