using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace MLM_Program
{
    class MyPrintPreviewDialog : System.Windows.Forms.PrintPreviewDialog
    {
        private ToolStripButton myPrintButton;

        public MyPrintPreviewDialog()
            : base()
        {
            Type t = typeof(PrintPreviewDialog);
            FieldInfo fi = t.GetField("toolStrip1", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo fi2 = t.GetField("printToolStripButton", BindingFlags.Instance | BindingFlags.NonPublic);
            ToolStrip toolStrip1 = (ToolStrip)fi.GetValue(this);
            ToolStripButton printButton = (ToolStripButton)fi2.GetValue(this);
            printButton.Visible = false;
            myPrintButton = new ToolStripButton();
            myPrintButton.ToolTipText = printButton.ToolTipText;
            myPrintButton.ImageIndex = 0;

            ToolStripItem[] oldButtons = new ToolStripItem[toolStrip1.Items.Count];

            for (int i = 0; i < oldButtons.Length; i++)
                oldButtons[i] = toolStrip1.Items[i];

            toolStrip1.Items.Clear();
            toolStrip1.Items.Add(myPrintButton);
            for (int i = 0; i < oldButtons.Length; i++)
                toolStrip1.Items.Add(oldButtons[i]);

            toolStrip1.ItemClicked += new ToolStripItemClickedEventHandler(toolBar1_Click);
        }

        private void toolBar1_Click(object sender, ToolStripItemClickedEventArgs eventargs)
        {
            if (eventargs.ClickedItem == myPrintButton)
            {
                PrintDialog printDialog1 = new PrintDialog();
                printDialog1.Document = this.Document;
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.Document.Print();
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Work_End"));
                }
            }
        }
    }
}
