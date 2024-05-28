using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;

namespace MLM_Program
{
    public class clsForm_Extends : Form
    {
        ////참고 URL: https://stackoverflow.com/questions/76993/how-to-double-buffer-net-controls-on-a-form
        // Release Mode 에서만 동작, Debug Mode는 일반 Form Class 동작

        public clsForm_Extends()
        {
            if (!IsInDesignMode)
            {
                InitializeCustomComponents();
            }

            this.Load += ClsForm_Extends_Load;
        }

        private void ClsForm_Extends_Load(object sender, EventArgs e)
        {
            if (!IsInDesignMode)
            {
                if (cls_User.gid_CountryCode == "TH" && cls_app_static_var.Using_language == "Thai")
                {
                    ChangeFont_TH(this, new Font("Tahoma", 8.75f));
                    //ChangeFont_TH(this, new Font("Angsana New", 8.75f));
                }
            }
        }

        public void InitializeCustomComponents()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();

        }

        private void ChangeFont_TH(Control control, Font newFont)
        {
            // 현재 컨트롤의 텍스트가 태국어인지 확인하고 폰트 크기 조정
            if (IsThaiText(control.Text))
            {
                control.Font = new Font(newFont.FontFamily, newFont.Size ); // 태국어인 경우 폰트 크기를 더 크게 설정
            }
            else
            {
                control.Font = newFont;
            }

            // 자식 컨트롤들이 있는 경우 재귀적으로 호출합니다.
            foreach (Control childControl in control.Controls)
            {
                ChangeFont_TH(childControl, newFont);
            }
        }

        private bool IsThaiText(string text)
        {
            // 태국어 문자가 있는지 확인하는 정규식
            return Regex.IsMatch(text, @"[\u0E00-\u0E7F]");
        }


        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!IsInDesignMode)
            {
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
                this.UpdateStyles();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!IsInDesignMode) cp.ExStyle |= clsNativeMethods.WS_EX_COMPOSITED;
                return cp;
            }
        }

        public void BeginUpdate()
        {
            clsNativeMethods.SendMessage(this.Handle, clsNativeMethods.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        }

        public void EndUpdate()
        {
            clsNativeMethods.SendMessage(this.Handle, clsNativeMethods.WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            Parent.Invalidate(true);

        }
        // DesignMode 속성을 올바르게 감지하기 위한 재정의
        private bool IsInDesignMode
        {
            get
            {
#if DEBUG
                return true;
#else 
                return false;
#endif
            }
        }
    }
}