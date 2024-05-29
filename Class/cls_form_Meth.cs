using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Data;
using System.Drawing;

namespace MLM_Program
{
    class cls_form_Meth
    {

        public void from_control_clear(Form fr, TextBox tb)
        {


            from_control_clear_02(fr);
            tb.Select();
        }

        public void from_control_clear(Form fr, MaskedTextBox tb)
        {
            from_control_clear_02(fr);
            tb.Focus();
        }

        public void from_control_clear(Form fr)
        {
            from_control_clear_02(fr);
        }

        public void from_control_clear(Form fr, CheckBox ck)
        {
            from_control_clear_02(fr);
        }

        public void from_control_clear(Form fr, RadioButton rb)
        {
            from_control_clear_02(fr);
        }

        public void from_control_clear(GroupBox gb, TextBox tb)
        {
            from_control_clear_02(gb);
            tb.Focus();
        }

        public void from_control_clear(TabControl Tb, TextBox tb)
        {
            from_control_clear_02(Tb);
            tb.Focus();
        }

        public void from_control_clear(TabControl Tb, MaskedTextBox tb)
        {
            from_control_clear_02(Tb);
            tb.Focus();
        }

        public void from_control_clear(TabControl Tb)
        {
            from_control_clear_02(Tb);
        }


        public void from_control_clear(GroupBox gb, MaskedTextBox tb)
        {
            from_control_clear_02(gb);
            tb.Focus();
        }


        public void from_control_clear(Panel fb, TextBox tb)
        {
            from_control_clear_02(fb);
            tb.Focus();
        }

        public void from_control_clear(Panel fb)
        {
            from_control_clear_02(fb);
        }



        public void from_control_clear(Panel fb, MaskedTextBox tb)
        {
            from_control_clear_02(fb);
            tb.Focus();
        }



        public Control from_Search_Control(Form fr, string search_name)
        {
            Control[] controls = GetAllControls(fr);

            foreach (Control c in controls)
            {
                if (c.Name.ToString() == search_name)
                    return c;
            }

            return null;
        }


        private void from_control_clear_02(Form fr)
        {
            Control[] controls = GetAllControls(fr);

            foreach (Control c in controls)
            {
                if (c.Tag != null && c.Tag.ToString() == "barcord_date")
                { }
                else
                    control_clear(c);
            }
        }




        private void from_control_clear_02(GroupBox gb)
        {
            Control[] controls = GetAllControls(gb);

            foreach (Control c in controls)
            {
                control_clear(c);
            }
        }

        private void from_control_clear_02(TabControl tb)
        {
            Control[] controls = GetAllControls(tb);

            foreach (Control c in controls)
            {
                control_clear(c);
            }
        }

        private void from_control_clear_02(Panel gb)
        {
            Control[] controls = GetAllControls(gb);

            foreach (Control c in controls)
            {
                control_clear(c);
            }
        }



        private void control_clear(Control ct)
        {
            if (ct.Tag != null && ct.Tag.ToString() == "tab_Nation" && ct.Enabled == false)
                return;


            if (ct is TextBox)
            {
                TextBox cf = (TextBox)ct;
                cf.Text = "";
            }

            if (ct is MaskedTextBox)
            {
                MaskedTextBox cf = (MaskedTextBox)ct;
                cf.Text = "";
            }

            if (ct is ComboBox)
            {
                ComboBox cf = (ComboBox)ct;
                cf.Text = "";
            }

            if (ct is CheckBox)
            {
                CheckBox cf = (CheckBox)ct;
                cf.Checked = false;
            }

            if (ct is RadioButton)
            {
                RadioButton cf = (RadioButton)ct;
                cf.Checked = false;
            }

        }




        public void from_control_text_base_chang(Form fr)
        {
            fr.Text = chang_base_caption_search(fr.Text.ToString());

            ResourceSet rs = cls_app_static_var.app_base_str_rm.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);
            IDictionaryEnumerator de = rs.GetEnumerator();

            //폼 상단바에 들어 있는 캡션을 지정한 걸로 바군다.

            de.Reset();
            while (de.MoveNext())
            {
                fr.Text = fr.Text.Replace(de.Key.ToString(), de.Value.ToString());
            }

            Control[] controls = GetAllControls(fr);

            foreach (Control c in controls)
            {
                control_t_change(c, de);

                if (cls_app_static_var.Using_Multi_language == 0)
                {
                    if (c.Name == "tab_Nation")
                        c.Visible = false;
                }
            }
        }


        private void control_t_change(Control ct, IDictionaryEnumerator de)
        {

            if (ct is CheckBox)
            {
                CheckBox cf = (CheckBox)ct;

                //컨트롤들의 캡션을 리소스에서 불러와서 저장된 내역을 변경한다.
                //다국어 지원일 경우에.. 다국어 연결에 편하게 하기 위함.
                cf.Text = chang_base_caption_search(cf.Text.ToString());

                //컨트롤들 캡션에 들어가 잇는 일정 문구를 지정된 문구로 변경한다.
                de.Reset();
                while (de.MoveNext())
                {
                    cf.Text = cf.Text.Replace(de.Key.ToString(), de.Value.ToString());
                }
            }

            if (ct is RadioButton)
            {
                RadioButton cf = (RadioButton)ct;
                cf.Text = chang_base_caption_search(cf.Text.ToString());

                de.Reset();
                while (de.MoveNext())
                {
                    cf.Text = cf.Text.Replace(de.Key.ToString(), de.Value.ToString());
                }
            }

            if (ct is Label)
            {
                Label cf = (Label)ct;

                cf.Text = chang_base_caption_search(cf.Text.ToString());

                de.Reset();
                while (de.MoveNext())
                {
                    cf.Text = cf.Text.Replace(de.Key.ToString(), de.Value.ToString());
                }
            }

            if (ct is GroupBox)
            {
                GroupBox cf = (GroupBox)ct;

                cf.Text = chang_base_caption_search(cf.Text.ToString());

                de.Reset();
                while (de.MoveNext())
                {
                    cf.Text = cf.Text.Replace(de.Key.ToString(), de.Value.ToString());
                }

            }

            if (ct is Button)
            {
                Button bt = (Button)ct;

                bt.Text = chang_base_caption_search(bt.Text.ToString());

                de.Reset();
                while (de.MoveNext())
                {
                    bt.Text = bt.Text.Replace(de.Key.ToString(), de.Value.ToString());
                }

            }


            if (ct is TabControl)
            {
                int Tcnt = 0;
                TabControl tbc = (TabControl)ct;
                Tcnt = 0;
                while (Tcnt < tbc.TabPages.Count)
                {
                    tbc.TabPages[Tcnt].Text = chang_base_caption_search(tbc.TabPages[Tcnt].Text.ToString());

                    de.Reset();
                    while (de.MoveNext())
                    {
                        tbc.TabPages[Tcnt].Text = tbc.TabPages[Tcnt].Text.Replace(de.Key.ToString(), de.Value.ToString());
                    }

                    Tcnt++;
                }

                //2020-08-10 디자인코드추가
                tbc.DrawMode = TabDrawMode.OwnerDrawFixed;
                tbc.DrawItem += Tbc_DrawItem;
            }


            if (ct is DateTimePicker) //폼로드시에 날짜 관련 셋팅을 다 현재 일자로 잡는다.
            {
                DateTimePicker cf = (DateTimePicker)ct;
                cf.Value = DateTime.Today;
            }
        }

        private void Tbc_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage tab = (sender as TabControl).TabPages[e.Index];
            Rectangle header = (sender as TabControl).GetTabRect(e.Index);
            using (SolidBrush darkBrush = new SolidBrush(Color.FromArgb(194, 214, 213)))
            using (SolidBrush lightBrush = new SolidBrush(Color.FromArgb(39, 126, 133)))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                if (e.State == DrawItemState.Selected)
                {
                    Font font = new Font((sender as TabControl).Font.Name, 9.25f, FontStyle.Regular);
                    e.Graphics.FillRectangle(lightBrush, e.Bounds);
                    e.Graphics.DrawString(tab.Text, font, darkBrush, header, sf);
                }
                else
                {
                    e.Graphics.FillRectangle(darkBrush, e.Bounds);
                    e.Graphics.DrawString(tab.Text, e.Font, lightBrush, header, sf);
                }
            }
        }

        public string _chang_base_caption_search(string OldCaption)
        {
            return chang_base_Base_caption_search(chang_base_caption_search(OldCaption));
        }


        private string chang_base_caption_search(string OldCaption)
        {
            //ResourceSet rs = null ; 
            //if (cls_User.gid_CountryCode == "KR")
            //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);
            //if (cls_User.gid_CountryCode == "La")            
            //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("lo-LA")  , true, true);
            //if (cls_User.gid_CountryCode == "Ja")
            //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("ja-JP"), true, true);
            //if (cls_User.gid_CountryCode == "US")
            //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), true, true);

            //IDictionaryEnumerator de = rs.GetEnumerator();
            //de.Reset();
            //while (de.MoveNext())
            //{
            //    if (de.Key.ToString() == OldCaption)
            //    {
            //        return de.Value.ToString();
            //    }
            //}

            if (cls_app_static_var.Base_Label.ContainsKey(OldCaption))
                return cls_app_static_var.Base_Label[OldCaption];
            else
                return OldCaption;
        }


        private string chang_base_Base_caption_search(string OldCaption)
        {

            ResourceSet rs = cls_app_static_var.app_base_str_rm.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);
            IDictionaryEnumerator de = rs.GetEnumerator();

            de.Reset();
            while (de.MoveNext())
            {
                OldCaption = OldCaption.Replace(de.Key.ToString(), de.Value.ToString());
            }
            return OldCaption;
        }


        public void form_DateTimePicker_Search_TextBox(Form fr, DateTimePicker dtp)
        {
            //DateTimePicker 이름을 지을때 _ 로 해서 앞뒤로 두개로 구분되게 하고 연결하는 텍스트 박스에
            //DateTimePicker의 _ 뒤쪽 명명과 동일한 명칭이 들어 가도록 해서.. 이름을 짓는다
            //해서 연결해 놓으면 동일한 이름의 텍스트 박스에 선택한 날짜가 들어가게 함.
            Control[] controls = GetAllControls(fr);

            string[] t_Name = dtp.Name.Split('_');
            string S_Txt_Name = t_Name[1];

            foreach (Control c in controls)
            {
                if (c is TextBox)
                {
                    TextBox tb = (TextBox)c;

                    if (
                        ((c.Name.Length - S_Txt_Name.Length) > 0) &&
                        (c.Name.Substring((c.Name.Length - S_Txt_Name.Length), S_Txt_Name.Length) == S_Txt_Name)
                        )
                    {
                        c.Text = dtp.Value.ToString("yyyyMMdd");

                        c.Focus();

                        //Control tb21 = fr.GetNextControl(fr.ActiveControl, true);
                        // tb21.Focus();
                        break;

                    }
                }

                if (c is MaskedTextBox)
                {
                    MaskedTextBox tb = (MaskedTextBox)c;

                    if (
                        ((c.Name.Length - S_Txt_Name.Length) > 0) &&
                        (c.Name.Substring((c.Name.Length - S_Txt_Name.Length), S_Txt_Name.Length) == S_Txt_Name)
                        )
                    {
                        c.Text = dtp.Value.ToString("yyyy-MM-dd");

                        c.Focus();

                        //Control tb21 = fr.GetNextControl(fr.ActiveControl, true);
                        //tb21.Focus();
                        break;

                    }
                }
            }

        }

        public void form_DateTimePicker_Search_TextBox(Form fr, DateTimePicker dtp, Control next_focus_cn)
        {
            //DateTimePicker 이름을 지을때 _ 로 해서 앞뒤로 두개로 구분되게 하고 연결하는 텍스트 박스에
            //DateTimePicker의 _ 뒤쪽 명명과 동일한 명칭이 들어 가도록 해서.. 이름을 짓는다
            //해서 연결해 놓으면 동일한 이름의 텍스트 박스에 선택한 날짜가 들어가게 함.
            Control[] controls = GetAllControls(fr);

            string[] t_Name = dtp.Name.Split('_');
            string S_Txt_Name = t_Name[1];

            foreach (Control c in controls)
            {
                if (c is TextBox)
                {
                    TextBox tb = (TextBox)c;

                    if (
                        ((c.Name.Length - S_Txt_Name.Length) > 0) &&
                        (c.Name.Substring((c.Name.Length - S_Txt_Name.Length), S_Txt_Name.Length) == S_Txt_Name)
                        )
                    {
                        c.Text = dtp.Value.ToString("yyyyMMdd");

                        next_focus_cn.Focus();
                        break;
                    }
                }

                if (c is MaskedTextBox)
                {
                    MaskedTextBox tb = (MaskedTextBox)c;

                    if (
                        ((c.Name.Length - S_Txt_Name.Length) > 0) &&
                        (c.Name.Substring((c.Name.Length - S_Txt_Name.Length), S_Txt_Name.Length) == S_Txt_Name)
                        )
                    {
                        c.Text = dtp.Value.ToString("yyyy-MM-dd");

                        next_focus_cn.Focus();
                        break;

                    }
                }
            }
        }


        public void Search_Date_TextBox_Put(TextBox _tb1, TextBox _tb2, RadioButton _trb)
        {

            string sdate = "";
            DateTime TodayDate = new DateTime();
            TodayDate = DateTime.Now;
            _tb1.Text = ""; _tb2.Text = "";

            if (_trb.Tag.ToString() == "D_1")
            {
                _tb1.Text = cls_User.gid_date_time; _tb2.Text = "";
            }

            if (_trb.Tag.ToString() == "D_7")
            {
                sdate = TodayDate.AddDays(-7).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", ""); _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "D_-1")
            {
                sdate = TodayDate.AddDays(-1).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", ""); _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_1")
            {
                sdate = cls_User.gid_date_time.Substring(0, 6) + "01";
                _tb1.Text = sdate; _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_2")
            {
                sdate = TodayDate.AddMonths(-1).ToString("yyyy/MM/dd hh:mm");
                sdate = sdate.Substring(0, 10).Replace("-", "").Substring(0, 6) + "01";
                string sdate2 = sdate.Substring(0, 6);

                switch (int.Parse(sdate.Substring(4, 2)))
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        {
                            sdate2 = sdate2 + "31";
                            break;
                        }
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        {
                            sdate2 = sdate2 + "30";
                            break;
                        }

                    case 2:
                        {
                            sdate2 = sdate2 + "28";
                            break;
                        }
                }

                _tb1.Text = sdate; _tb2.Text = sdate2;
            }

            if (_trb.Tag.ToString() == "M_3")
            {
                sdate = TodayDate.AddMonths(-2).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", "").Substring(0, 6) + "01"; _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "T_1")
            {
                _tb1.Text = "19900101"; _tb2.Text = cls_User.gid_date_time;
            }

            _tb1.Focus();
        }


        public void Search_Date_TextBox_Put(MaskedTextBox _tb1, MaskedTextBox _tb2, RadioButton _trb)
        {

            string sdate = "";
            DateTime TodayDate = new DateTime();
            TodayDate = DateTime.Now;
            _tb1.Text = ""; _tb2.Text = "";

            if (_trb.Tag.ToString() == "D_1")
            {
                _tb1.Text = cls_User.gid_date_time; _tb2.Text = "";
            }

            if (_trb.Tag.ToString() == "D_7")
            {
                sdate = TodayDate.AddDays(-7).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", ""); _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "D_-1")
            {
                sdate = TodayDate.AddDays(-1).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", ""); _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_1")
            {
                sdate = cls_User.gid_date_time.Substring(0, 6) + "01";
                _tb1.Text = sdate; _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_2")
            {
                sdate = TodayDate.AddMonths(-1).ToString("yyyy/MM/dd hh:mm");
                sdate = sdate.Substring(0, 10).Replace("-", "").Substring(0, 6) + "01";
                string sdate2 = sdate.Substring(0, 6);

                switch (int.Parse(sdate.Substring(4, 2)))
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        {
                            sdate2 = sdate2 + "31";
                            break;
                        }
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        {
                            sdate2 = sdate2 + "30";
                            break;
                        }

                    case 2:
                        {
                            sdate2 = sdate2 + "28";
                            break;
                        }
                }

                _tb1.Text = sdate; _tb2.Text = sdate2;
            }

            if (_trb.Tag.ToString() == "M_3")
            {
                sdate = TodayDate.AddMonths(-2).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", "").Substring(0, 6) + "01"; _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_6")
            {
                sdate = TodayDate.AddMonths(-5).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", "").Substring(0, 6) + "01"; _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_9")
            {
                sdate = TodayDate.AddMonths(-8).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", "").Substring(0, 6) + "01"; _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "M_12")
            {
                sdate = TodayDate.AddMonths(-11).ToString("yyyy/MM/dd hh:mm");
                _tb1.Text = sdate.Substring(0, 10).Replace("-", "").Substring(0, 6) + "01"; _tb2.Text = cls_User.gid_date_time;
            }

            if (_trb.Tag.ToString() == "T_1")
            {
                _tb1.Text = "19900101"; _tb2.Text = cls_User.gid_date_time;
            }

            _tb1.Focus();
        }





        public void form_Group_Panel_Enable_True(Form fr)
        {
            Control[] controls = GetAllControls(fr);

            foreach (Control c in controls)
            {
                if (c is GroupBox)
                {
                    GroupBox tgr = (GroupBox)c;
                    tgr.Enabled = true;
                }

                if (c is Panel)
                {
                    Panel tpn = (Panel)c;
                    tpn.Enabled = true;
                }
            }
        }

        public void form_Group_Panel_Enable_False(Form fr)
        {
            Control[] controls = GetAllControls(fr);

            foreach (Control c in controls)
            {
                if (c is GroupBox)
                {
                    GroupBox tgr = (GroupBox)c;
                    tgr.Enabled = false;
                }

                if (c is Panel)
                {
                    Panel tpn = (Panel)c;
                    tpn.Enabled = false;
                }
            }
        }


        public void form_Main_Button_Dictionary(Form fr, ref Dictionary<string, Button> Mdi_Button_dic)
        {
            Control[] controls = GetAllControls(fr);

            foreach (Control c in controls)
            {
                if (c is Button)
                {
                    Mdi_Button_dic[c.Name] = (Button)c;
                }
            }
        }


        private Control[] GetAllControls(Control containerControl)
        {
            List<Control> allControls = new List<Control>();

            Queue<Control.ControlCollection> queue = new Queue<Control.ControlCollection>();

            queue.Enqueue(containerControl.Controls);

            while (queue.Count > 0)
            {
                Control.ControlCollection controls
                            = (Control.ControlCollection)queue.Dequeue();

                if (controls == null || controls.Count == 0) continue;

                foreach (Control control in controls)
                {

                    allControls.Add(control);

                    queue.Enqueue(control.Controls);
                }
            }

            return allControls.ToArray();
        }


        public void Home_Number_Setting(string baseNumber, Control control)
        {
            string T_Num1, T_Num2, T_Num3;
            T_Num1 = T_Num2 = T_Num3 = string.Empty;

            Phone_Number_Split(baseNumber, ref T_Num1, ref T_Num2, ref T_Num3);

            T_Num1 = T_Num1.Trim();
            T_Num2 = T_Num2.Trim();
            T_Num3 = T_Num3.Trim();

            if (T_Num1.Length == 2)
                T_Num1 = " " + T_Num1;

            if (T_Num2.Length == 3)
                T_Num2 = " " + T_Num2;

            control.Text = string.Format("{0}-{1}-{2}", T_Num1, T_Num2, T_Num3);
        }

        public void Phone_Number_Split(string baseNumber, ref string T_Num1, ref string T_Num2, ref string T_Num3)
        {
            T_Num1 = ""; T_Num2 = ""; T_Num3 = "";
            string[] T_S_Number = baseNumber.Split('-');

            //- 게 제대로 2개 들어가 잇다.
            if (T_S_Number.Length == 3)
            {
                T_Num1 = T_S_Number[0];
                T_Num2 = T_S_Number[1];
                T_Num3 = T_S_Number[2];
            }
            else
            {
                //우선 전화 번호상에 들어온 - 를 다 없앤다.. 제대로 전화 번호가 안들어 오는 경우도있기 때문에
                string t_Number = baseNumber.Trim().Replace("-", "");

                if (baseNumber.Length >= 3)
                {
                    if (baseNumber.Substring(0, 2) != "02")
                    {
                        if (baseNumber.Length == 11)
                        {
                            T_Num1 = baseNumber.Substring(0, 3);
                            T_Num2 = baseNumber.Substring(3, 4);
                            T_Num3 = baseNumber.Substring(7, 4);
                        }

                        if (baseNumber.Length == 10)
                        {
                            T_Num1 = baseNumber.Substring(0, 3);
                            T_Num2 = baseNumber.Substring(3, 3);
                            T_Num3 = baseNumber.Substring(6, 4);
                        }
                    }
                    else
                    {
                        if (baseNumber.Length == 10)
                        {
                            T_Num1 = baseNumber.Substring(0, 2);
                            T_Num2 = baseNumber.Substring(2, 4);
                            T_Num3 = baseNumber.Substring(6, 4);
                        }

                        if (baseNumber.Length == 9)
                        {
                            T_Num1 = baseNumber.Substring(0, 2);
                            T_Num2 = baseNumber.Substring(2, 3);
                            T_Num3 = baseNumber.Substring(5, 4);
                        }
                    }

                }
            }
        }


        public void button_flat_change(Button tbt)
        {
            tbt.FlatAppearance.BorderColor = cls_app_static_var.Button_Border_Color;  //cls_app_static_var.txt_Focus_Color;
            tbt.FlatAppearance.MouseOverBackColor = cls_app_static_var.Button_Parent_Color;
            tbt.FlatAppearance.MouseDownBackColor = cls_app_static_var.txt_Focus_Color;
        }


    }//end   cls_form_Meth


}
