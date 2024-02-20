using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using XtraEditors = DevExpress.XtraEditors;
namespace MLM_Program
{
   
    delegate void  Key_13_Event_Handler();
    delegate void Key_13_tb_Event_Handler(string txt_tag, TextBox tb); 
    delegate void Key_13_Name_Event_Handler(string txt_tag, TextBox tb);
    delegate void Key_13_Ncode_Event_Handler(string txt_tag, TextBox tb);
    //delegate void  Text_Error_Check (TextBox tb ) ;


    class cls_Check_Text
    {
        // 텍스트 박스에서 엔터키를 눌렀을때
        // 이벤트를 발생 시키기 위함... 이벤트 핸들러에서는 다음 탭 인덱스로 포커스 이동 시킴.
        public event Key_13_Event_Handler Key_Enter_13;
        public event Key_13_tb_Event_Handler Key_Enter_13_tb;
        public event Key_13_Name_Event_Handler Key_Enter_13_Name;
        public event Key_13_Ncode_Event_Handler Key_Enter_13_Ncode;


        //텍스트 박스상에서 숫자만 입력 받기를 원하는 경우 사용되는 메소드
        public bool Text_KeyChar_Check(KeyPressEventArgs e, int i,string Point)
        {
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar == 8) || (e.KeyChar == 46))
            {
                return true;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    Key_Enter_13();
                }
                return false;
            }
        }


      
        //텍스트 박스상에서 숫자만 입력 받기를 원하는 경우 사용되는 메소드
        public bool Text_KeyChar_Check(KeyPressEventArgs e, int i)
        {
            if (i > 0)
            {
                if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar == 8))
                {
                    return true;
                }
                else
                {
                    if (e.KeyChar == 13)
                    {
                        Key_Enter_13();
                    }
                    return false;
                }
            }
            else
            {
                if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar == 8) || (e.KeyChar == 45))
                {
                    return true;
                }
                else
                {
                    if (e.KeyChar == 13)
                    {
                        Key_Enter_13();
                    }
                    return false;
                }
            }
        }

        //텍스트 박스상에서 숫자만 입력 받기를 원하는 경우 사용되는 메소드
        public bool Text_KeyChar_Check(KeyPressEventArgs e, int i, int t)
        {
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar == 8) || (e.KeyChar == 46) )
            {
                return true;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    Key_Enter_13();
                }
                return false;
            }
        }


        //텍스트 박스상에서 숫자그림 - 를 입력 받는 텍스트 박스
        public bool Text_KeyChar_Check(KeyPressEventArgs e, string  i)
        {
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar == 8) || (e.KeyChar == 45))
            {
                return true;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    Key_Enter_13();
                }
                return false;
            }
        }



        //텍스트 박스상에서 숫자그림 . 를 입력 받는 텍스트 박스
        public bool Text_KeyChar_Check(KeyPressEventArgs e, string i ,int i2)
        {
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar == 8) || (e.KeyChar == 46))
            {
                return true;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    Key_Enter_13();
                }
                return false;
            }
        }
       
        //텍스트 박스상에서 쿼리 관련 글자들을 입력 받지 못하게 하는 경우 에 사용되는 메소드
        //일반적으로 모든 텍스트상에서 적용된다고 보면 됨.
        public bool Text_KeyChar_Check(KeyPressEventArgs e)
        {
            //if (e.KeyChar == 34 || e.KeyChar == 39 || e.KeyChar == 44)
            if (e.KeyChar == 34 || e.KeyChar == 39 )
            {                
                return false;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    Key_Enter_13();
                }

                return true;
            }
        }



        //텍스트 박스상에서 숫자만 입력 받기를 원하는 경우 사용되는 메소드
        public bool Text_KeyChar_Check(KeyPressEventArgs e,TextBox tb, int i)
        {
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar == 8))
            {
                return true;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    Key_Enter_13_tb(tb.Text.ToString(), tb);
                }
                return false;
            }
        }

        //텍스트 박스상에서 숫자그림 - 를 입력 받는 텍스트 박스
        public bool Text_KeyChar_Check(KeyPressEventArgs e,TextBox tb, string i)
        {
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar == 8) || (e.KeyChar == 45))
            {
                return true;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    Key_Enter_13_tb(tb.Text.ToString(), tb);
                }
                return false;
            }
        }


        public bool Text_KeyChar_Check(KeyPressEventArgs e, TextBox tb, TextBox tb2)
        {
            //if (e.KeyChar == 34 || e.KeyChar == 39 || e.KeyChar == 44)
            if (e.KeyChar == 34 || e.KeyChar == 39 )
            {
                return false;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    Key_Enter_13_tb(tb.Text.ToString(), tb);
                }

                return true;
            }
        }

        public bool Text_KeyChar_Check(KeyPressEventArgs e, TextBox tb, string S, int i, string ss)
        {




            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar == 8))
            {
                return true;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    //Key_Enter_13_Ncode(tb.Text.ToString(), tb);
                    Key_Enter_13();

                }
                else if ((e.KeyChar >= 65 && e.KeyChar <= 90) || (e.KeyChar >= 97 && e.KeyChar <= 122) || (e.KeyChar == 8))
                {
                    return true;
                }
                return false;
            }
        }

        //텍스트 박스상에서 숫자만 입력 받기를 원하는 경우 사용되는 메소드
        public bool Text_KeyChar_Check(TextBox tb, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 34 || e.KeyChar == 39 || e.KeyChar == 40 || e.KeyChar == 41 || e.KeyChar == 44)
            if (e.KeyChar == 34 || e.KeyChar == 39 || e.KeyChar == 40 || e.KeyChar == 41 )
            {

                return false;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    Key_Enter_13_Name(tb.Text.ToString(), tb);
                }
                return true;
            }
        }

        public bool Text_KeyChar_Check( KeyPressEventArgs e, TextBox tb)
        {
            //if (e.KeyChar == 34 || e.KeyChar == 39 || e.KeyChar == 40 || e.KeyChar == 41 || e.KeyChar == 44)
            if (e.KeyChar == 34 || e.KeyChar == 39 || e.KeyChar == 40 || e.KeyChar == 41)
            {

                return false;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    Key_Enter_13_Ncode(tb.Text.ToString(), tb);
                }
                return true;
            }
        }


        public bool Text_KeyChar_Check(KeyPressEventArgs e, TextBox tb, string S, int i)
        {
            //if (e.KeyChar == 34 || e.KeyChar == 39 || e.KeyChar == 40 || e.KeyChar == 41 || e.KeyChar == 44)
            //{

            //    return false;
            //}
            //else if (e.KeyChar == 34 || e.KeyChar == 39 || e.KeyChar == 40 || e.KeyChar == 41 || e.KeyChar == 44)
            //{

            //    return false;
            //}
            //else
            //{
            //    if (e.KeyChar == 13)
            //    {
            //        Key_Enter_13_Ncode(tb.Text.ToString(), tb);
            //    }
            //    return true;
            //}


            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar == 8))
            {
                return true;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    Key_Enter_13_Ncode(tb.Text.ToString(), tb);
                }
                return false;
            }
        }


      



        public void Text_Focus_All_Sel(TextBox T_b)
        {
            T_b.SelectAll();
        }

        public void Text_Focus_All_Sel(MaskedTextBox  T_b)
        {
            T_b.SelectAll();
        }


        public string  Text_Null_Check(Control tb)
        {
            string me ="";
            if (tb.Text.Trim() == "")
            {
                //me ="빈칸입니다. 내역을 입력해 주십시요.";
                me = cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                tb.Select();
                return me;
            }

            return me;
        }

        public string Text_Null_Check(Control tb, string res_Name)
        {
            string me = "";
            if (tb.Text.Trim() == "")
            {
                //me ="빈칸입니다. 내역을 입력해 주십시요.";
                me = cls_app_static_var.app_msg_rm.GetString(res_Name) + "\n" + 
                 cls_app_static_var.app_msg_rm.GetString("Msg_txt_Not_Data");

                tb.Focus ();
                return me;
            }

            return me;
        }

    } //end cls_Check_Text


    class cls_Check_Input_Error
    {

        //cls_Check_Input_Error
        //Input_Date_Err_Check
        //_Member_Nmumber_Split
        public int Input_Date_Err_Check(TextBox tb)
        {
            DateTime dateTime;
            string input = string.Format("{0:####-##-##}", int.Parse(tb.Text.Replace ("-","")));
            if (DateTime.TryParse(input, out dateTime) == false)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                        + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Date")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));                
                return -1;
            }

            return 1;
        }

        public int Input_Date_Err_Check(Control ctrl)
        {
            DateTime dateTime;

            string input = string.Format("{0:####-##-##}", int.Parse(ctrl.Text.Replace("-", "")));
            if (DateTime.TryParse(input, out dateTime) == false)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                        + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Date")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                return -1;
            }

            return 1;
        }

        public bool Input_Date_Err_Check__01(MaskedTextBox mtb)
        {
            string[] date_a = mtb.Text.Split('-');

            if (date_a.Length >= 3 && date_a[0].Trim() != "" && date_a[1].Trim() != "" && date_a[2].Trim() != "")
            {
                string Date_YYYY = "0000" + int.Parse(date_a[0]).ToString();

                date_a[0] = Date_YYYY.Substring(Date_YYYY.Length - 4, 4);

                if (int.Parse(date_a[1]) < 10)
                    date_a[1] = "0" + int.Parse(date_a[1]).ToString();

                if (int.Parse(date_a[2]) < 10)
                    date_a[2] = "0" + int.Parse(date_a[2]).ToString();

                mtb.Text = date_a[0] + '-' + date_a[1] + '-' + date_a[2];


                //cls_Check_Input_Error c_er = new cls_Check_Input_Error();
                if (mtb.Text.Replace("-", "").Trim() != "")
                {
                    int Ret = 0;
                    Ret = Input_Date_Err_Check(mtb);

                    if (Ret == -1)
                    {
                        mtb.Focus(); return false;
                    }
                }
            }
            else if (mtb.Text.Length == 8)
            {
                int Ret = 0;
                Ret = Input_Date_Err_Check(mtb);

                if (Ret == -1)
                {
                    mtb.Focus(); return false;
                }                
            }
            else
                return false;

            return true ;
        }


        public int Input_Date_Err_Check(MaskedTextBox  tb)
        {
            DateTime dateTime;
            string input = string.Format("{0:####-##-##}", int.Parse(tb.Text.Replace("-", "")));
            if (DateTime.TryParse(input, out dateTime) == false)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                        + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Date")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                return -1;
            }

            return 1;
        }

        public int Input_Date_Err_Check(TextBox tb, int T)
        {
            DateTime dateTime;
            string input = string.Format("{0:####-##-##}", int.Parse(tb.Text));
            if (DateTime.TryParse(input, out dateTime) == false)
            {               
                return -1;
            }

            return 1;
        }

        public int Input_Date_Err_Check(MaskedTextBox tb, int T)
        {
            DateTime dateTime;
            string input = string.Format("{0:####-##-##}", int.Parse(tb.Text.Replace ("-","")));
            if (DateTime.TryParse(input, out dateTime) == false)
            {
                return -1;
            }

            return 1;
        }

        
        public int _Member_Nmumber_Split(MaskedTextBox  tb)
        {
            string Mbid = ""; int Mbid2 = 0;
            int Ret = 0;
            Ret = Member_Nmumber_Split(tb.Text.Trim(), ref Mbid, ref Mbid2);

            if (Ret == -1)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                        + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));                
                return Ret;
            }

            return Ret;
        }


        private int Member_Nmumber_Split(string Search_Member_Num, ref string Mbid, ref int Mbid2)
        {
            Mbid = ""; Mbid2 = 0;
            string[] t_Mbid;
            t_Mbid = Search_Member_Num.Split('-');

            //회원번호 체게상 앞자리를 사용한다고 햇는데 나온거는 한자리 이면 오류임 2자리 이상은 나와야함.
            if (cls_app_static_var.Member_Number_1 > 0 && t_Mbid.Length <= 1) return -1;


            if (t_Mbid.Length == 2)
            {
                if (t_Mbid[0] == "" || t_Mbid[1] == "") return -1;
            }

            if (t_Mbid.Length == 1)
                Mbid2 = int.Parse(t_Mbid[0]);
            else
            {
                Mbid = t_Mbid[0];
                Mbid2 = int.Parse(t_Mbid[1]);
            }

            return 1;
        }

        internal int Input_Date_Err_Check(XtraEditors.DateEdit mtxtSellDate, int v)
        {
            throw new NotImplementedException();
        }
    }


    public class XmlHandler
    {
        XmlDocument xmlDocument;

        /// <summary>
        /// Initialisiert eine neue Instanz der MultiClipboard Klasse.
        /// </summary>
        public XmlHandler()
        {
        }

        /// <summary>
        /// Den inhalt des TreeViews in eine xml Datei exportieren
        /// </summary>
        /// <param name="treeView">Der TreeView der exportiert werden soll</param>
        /// <param name="path">Ein  Pfad unter dem die Xml Datei entstehen soll</param>
        public void TreeViewToXml(TreeView treeView, String path)
        {
            xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateElement("ROOT"));
            XmlRekursivExport(xmlDocument.DocumentElement, treeView.Nodes);
           
            xmlDocument.Save(path);
        }


       
        /// <summary>
        /// Eine vorher Exportierte Xml Datei wieder in ein TreeView importieren
        /// </summary>
        /// <param name="path">Der Quellpfad der Xml Datei</param>
        /// <param name="treeView">Ein TreeView in dem der Inhalt der Xml Datei wieder angezeigt werden soll</param>
        /// <exception cref="FileNotFoundException">gibt an das die Datei nicht gefunden werden konnte</exception>
        public void XmlToTreeView(String path, TreeView treeView)
        {
            xmlDocument = new XmlDocument();

            xmlDocument.Load(path);
            treeView.Nodes.Clear();
            XmlRekursivImport(treeView.Nodes, xmlDocument.DocumentElement.ChildNodes);
        }

        private XmlNode XmlRekursivExport(XmlNode nodeElement, TreeNodeCollection treeNodeCollection)
        {
            XmlNode xmlNode = null;
            foreach (TreeNode treeNode in treeNodeCollection)
            {
                xmlNode = xmlDocument.CreateElement("TreeViewNode");

                xmlNode.Attributes.Append(xmlDocument.CreateAttribute("value"));
                xmlNode.Attributes["value"].Value = treeNode.Text;


                if (nodeElement != null)
                    nodeElement.AppendChild(xmlNode);

                if (treeNode.Nodes.Count > 0)
                {
                    XmlRekursivExport(xmlNode, treeNode.Nodes);
                }
            }
            return xmlNode;
        }

        private void XmlRekursivImport(TreeNodeCollection elem, XmlNodeList xmlNodeList)
        {
            TreeNode treeNode;
            foreach (XmlNode myXmlNode in xmlNodeList)
            {
                treeNode = new TreeNode(myXmlNode.Attributes["value"].Value);

                if (myXmlNode.ChildNodes.Count > 0)
                {
                    XmlRekursivImport(treeNode.Nodes, myXmlNode.ChildNodes);
                }
                elem.Add(treeNode);
            }
        }





    }
}
