using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
namespace MLM_Program
{
    public class clsForm_Extends : Form
    {
        // 참고 URL: https://stackoverflow.com/questions/76993/how-to-double-buffer-net-controls-on-a-form
        // Release Mode 에서만 동작, Debug Mode는 일반 Form Class 동작
#if (!DEBUG)

        protected override void OnCreateControl()
        {

            base.OnCreateControl();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= clsNativeMethods.WS_EX_COMPOSITED;
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
#endif


    }
}