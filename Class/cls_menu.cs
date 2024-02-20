using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Resources;
using DevExpress;
//using System.Resources;
//using System.Collections;
using DevExpress.XtraEditors;

namespace MLM_Program.Class
{
        public class TestColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Color.Red; }
            }

            public override Color MenuBorder  //added for changing the menu border
            {
                get { return Color.Green; }
            }

        }
    
}
