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

namespace MLM_Program
{
    

    public partial class MDIMain : Form
    {


        private int Login_Board_TF = 1; //로그인시 로그인보드 띄워줄지 체크


        public class CMenuColorTable : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground
            {
                get
                {
                    return Color.White;
                }
            }

            public override Color MenuBorder
            {
                get
                {
                    return Color.FromArgb(82-20, 89-20, 95-20);
                }
            }

            public override Color MenuItemBorder
            {
                get
                {
                    return Color.White;//Color.FromArgb(82 - 20, 89 - 20, 95 - 20);
                }
            }

            public override Color MenuItemSelected
            {
                get
                {
                    //return Color.Blue;
                    //return Color.FromArgb(82, 89, 95);
                    return Color.FromArgb(209, 247, 250);
                }
            }

            public override Color MenuStripGradientBegin
            {
                get
                {
                    return Color.White;
                    //return Color.FromArgb(82, 89, 95);
                }
            }

            public override Color MenuStripGradientEnd
            {
                get
                {
                    return Color.White;
                    //return Color.FromArgb(82, 89, 95);
                }
            }

            public override Color MenuItemSelectedGradientBegin
            {
                get
                {

                    //return Color.White;
                    return Color.FromArgb(82, 89, 95);
                }
            }

            public override Color MenuItemSelectedGradientEnd
            {
                get
                {
                    //return Color.White;

                    //return Color.FromArgb(82, 89, 95);
                    return Color.FromArgb(82 + 30, 89 + 30, 95 + 30);
                }
            }

            public override Color MenuItemPressedGradientBegin
            {
                get
                {
                    return Color.FromArgb(82, 89, 95);
                }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get
                {
                    return Color.FromArgb(82 + 60, 89 + 60, 95 + 60);
                }
            }
        }


        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV); 

        private string Mdi_Middle_Send_Number;
        private string Mdi_Middle_Send_Name;
        private string Mdi_Middle_Send_OrderNumber;

        private int panel1_Base_Width = 0;
        private int but_Exp_Base_Left = 0;
        private int form_Loade_TW = 0;
        

        private int Quick_Menu_TF = 0; //퀵메뉴에서 불려졋는지를 체크한다.
        
        Dictionary<string, Panel> Top_Menu = new Dictionary<string, Panel> ();
        Dictionary<string, Button> Left_Menu = new Dictionary<string, Button>();

        private const string base_db_name = "tbl_Memberinfo";

      
        public MDIMain()
        {
            InitializeComponent();
            this.Shown += MDIMain_Shown;

        }
        private void MDIMain_Shown(object sender, EventArgs e)
        {
            if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
            {
                menuStrip.BackColor = Color.FromArgb(82, 89, 95);
            }
            else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
            {
                menuStrip.BackColor = Color.FromArgb(45, 52, 54);
            }

            if (Login_Board_TF == 1 && cls_User.gid != cls_User.SuperUserID)
            {
                frmBase_Login_Board childForm = new frmBase_Login_Board();
                childForm.Send_Sell_Number += new frmBase_Login_Board.SendNumberDele(childForm_Send_Mem_Number);
                childForm.Send_Mem_Number += new frmBase_Login_Board.Send_Mem_NumberDele(childForm_Send_Mem_Number);
                Child_Form_Load(childForm);
            }
        }

        private void menuItem_MouseLeave(object sender, EventArgs e)
        {
        }
        private void menuitem_MouseEnter(object sender, EventArgs e)
        {
            MemberMenu.BackColor = Color.Red;
        }
        private void ShowNewForm(object sender, EventArgs e)
        {
            ToolStripMenuItem tm = (ToolStripMenuItem)sender;

            if (tm.Tag.ToString () == "Bank")
            {
                Form  childForm = new frmBase_Bank();
                
                //childForm.FormSendEvent += new frmBase_Bank.FormSendDataHandler(DieaseUpdateEventMethod);                
                Child_Form_Load(childForm);               
            }

            if (tm.Tag.ToString() == "Bank_Com")
            {
                Form childForm = new frmBase_Bank_Com();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Card")
            {
                Form childForm = new frmBase_Card();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Center")
            {
                Form childForm = new frmBase_Center();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Purchase")
            {
                frmBase_Purchase childForm = new frmBase_Purchase();
                
                Child_Form_Load(childForm);
            } 

            if (tm.Tag.ToString() == "Rece")
            {
                frmBase_Rec childForm = new frmBase_Rec();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Goods")
            {
                frmBase_Goods childForm = new frmBase_Goods();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Goods_Sort_1")
            {
                frmBase_Goods_Sort childForm = new frmBase_Goods_Sort();
                
                Child_Form_Load(childForm);
            }
            
            if (tm.Tag.ToString() == "Goods_Sort_2")
            {
                frmBase_Goods_Sort_2 childForm = new frmBase_Goods_Sort_2();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Goods_Set")
            {
                frmBase_Goods_Set childForm = new frmBase_Goods_Set();
                
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "base_Nation")
            {
                frmBase_Nation childForm = new frmBase_Nation();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Goods_Nation")
            {
                frmBase_Goods_Nation childForm = new frmBase_Goods_Nation();
                
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "Give_Prom_Set")
            {
                frmBase_Prom_Base_NEW childForm = new frmBase_Prom_Base_NEW();
                
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "Mail_Request")
            {
                frmBase_Mail_Request childForm = new frmBase_Mail_Request();
                
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "Promotion_Select")
            {
                frm_Promotion_Select childForm = new frm_Promotion_Select();

                Child_Form_Load(childForm);
            }
        }






        private void Member_Child(object sender, EventArgs e)
        {
            ToolStripMenuItem tm = (ToolStripMenuItem)sender;

            if (tm.Tag.ToString() == "frmMember_ED")
            {
                frmMember_ED childForm = new frmMember_ED();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "frmSell_NEXT_GRADE")
            {
                frmSell_NEXT_GRADE childForm = new frmSell_NEXT_GRADE();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_CSV_Upload")
            {
                frm_CSV_Import_Member childForm = new frm_CSV_Import_Member();
                
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "Change_SPON")
            {
                frmMember_Change_SPON childForm = new frmMember_Change_SPON();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member")
            {
                //frmMember_Dev childForm = new frmMember_Dev();
                if (cls_User.gid_CountryCode == "TH")
                {
                    frmMember_TH childForm = new frmMember_TH();
                    Child_Form_Load(childForm);
                }
                else
                {
                    frmMember childForm = new frmMember();
                    Child_Form_Load(childForm);
                }


                //childForm.Send_Mem_Number += new frmMember_Dev.SendNumberDele(childForm_Send_Mem_Number);
            }

            if (tm.Tag.ToString() == "Member_Update")
            {
                frmMember_Update childForm = new frmMember_Update();
                
                childForm.Take_Mem_Number += new frmMember_Update.Take_NumberDele(childForm_Take_Mem_Number);
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_Update_2")
            {
                frmMember_Update_2 childForm = new frmMember_Update_2();
                
                childForm.Take_Mem_Number += new frmMember_Update_2.Take_NumberDele(childForm_Take_Mem_Number);
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_Delete")
            {
                frmMember_Delete childForm = new frmMember_Delete();
                
                //childForm.Take_Mem_Number += new frmMember_Update.Take_NumberDele(childForm_Take_Mem_Number);
                Child_Form_Load(childForm);
            }
            

            if (tm.Tag.ToString() == "Member_Tree")
            {
                frmMember_TreeView childForm = new frmMember_TreeView();
                
                childForm.Take_Mem_Number += new frmMember_TreeView.Take_NumberDele(childForm_Take_Mem_Number); //Take_Far_Memnu_Change_Dele
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_Tree_Nom")
            {
                frmMember_TreeView_Nom childForm = new frmMember_TreeView_Nom();
                
                childForm.Take_Mem_Number += new frmMember_TreeView_Nom.Take_NumberDele(childForm_Take_Mem_Number);
                Child_Form_Load(childForm);
            }
            

            if (tm.Tag.ToString() == "Member_Select")
            {
                frmMember_Select childForm = new frmMember_Select();
                
                childForm.Send_Mem_Number += new frmMember_Select.SendNumberDele(childForm_Send_Mem_Number);
                Child_Form_Load(childForm);                   
            }


            if (tm.Tag.ToString() == "Member_Select_C")
            {
                frmMember_Select_C childForm = new frmMember_Select_C();
                
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "Member_Select_Change")
            {
                frmMember_Select_Change childForm = new frmMember_Select_Change();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_Save_Change")
            {
                frmMember_Save_Change childForm = new frmMember_Save_Change();
                
                childForm.Take_Mem_Number += new frmMember_Save_Change.Take_NumberDele(childForm_Take_Mem_Number);
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_Nom_Change")
            {
                frmMember_Nom_Change childForm = new frmMember_Nom_Change();
                
                childForm.Take_Mem_Number += new frmMember_Nom_Change.Take_NumberDele(childForm_Take_Mem_Number);
                Child_Form_Load(childForm);
            }

            

            if (tm.Tag.ToString() == "Member_Select_Up_Change")
            {
                frmMember_Select_Up_Change childForm = new frmMember_Select_Up_Change();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_Center_Change")
            {
                frmMember_Center_Change childForm = new frmMember_Center_Change();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_Address_Change")
            {
                frmMember_Address_Change childForm = new frmMember_Address_Change();
                
                childForm.Take_Mem_Number += new frmMember_Address_Change.Take_NumberDele(childForm_Take_Mem_Number);
                Child_Form_Load(childForm);
            }

          
            if (tm.Tag.ToString() == "Member_TreeGroup")
            {
                frmMember_TreeGroup childForm = new frmMember_TreeGroup();
                
                childForm.Take_Mem_Number += new frmMember_TreeGroup.Take_NumberDele(childForm_Take_Mem_Number);
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_TreeGroup_Nom")
            {
                frmMember_TreeGroup_Nom childForm = new frmMember_TreeGroup_Nom();
                
                childForm.Take_Mem_Number += new frmMember_TreeGroup_Nom.Take_NumberDele(childForm_Take_Mem_Number);
                Child_Form_Load(childForm);
            }
            
            if (tm.Tag.ToString() == "Member_Not_Sell")
            {
                frmMember_Select_Not_Sell childForm = new frmMember_Select_Not_Sell();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_Talk")
            {
                frmMember_Select_Talk childForm = new frmMember_Select_Talk();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_Auto")
            {
                frmMember_Auto childForm = new frmMember_Auto();
                
                Child_Form_Load(childForm);
            }

          

            if (tm.Tag.ToString() == "Member_Select_Auto")
            {
                frmMember_Select_Auto childForm = new frmMember_Select_Auto();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_Select_Auto_Delete")
            {
                frmMember_Select_Auto_Delete childForm = new frmMember_Select_Auto_Delete();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_UpdateSelect")
            {
                frmMember_UpdateSelect childForm = new frmMember_UpdateSelect();
                
                childForm.Take_Mem_Number += new frmMember_UpdateSelect.Take_NumberDele(childForm_Take_Mem_Number);
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "Member_Excel")
            {
                frmMember_UpdateSelect childForm = new frmMember_UpdateSelect();
                
                childForm.Take_Mem_Number += new frmMember_UpdateSelect.Take_NumberDele(childForm_Take_Mem_Number);
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "Ten_Cafe")
            {
                frmMember_Select_Cafe childForm = new frmMember_Select_Cafe();
                
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "Ten_Cafe_Select")
            {
                frmMember_Select_Cafe_Select childForm = new frmMember_Select_Cafe_Select();
                
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "Ten_Cafe_upload")
            {
                frm_barcord_Import_Member childForm = new frm_barcord_Import_Member();
                
                Child_Form_Load(childForm);
            }

        }

        private void Sell_Child(object sender, EventArgs e)
        {
            ToolStripMenuItem tm = (ToolStripMenuItem)sender;
            if (tm.Tag.ToString() == "ka_Coupon")
            {
                frmBase_Card_Coupon_Delete childForm = new frmBase_Card_Coupon_Delete();
                
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "ka_Coupon_Excel")
            {
                frm_Coupon_Import_Member childForm = new frm_Coupon_Import_Member();
                
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "SellBase")
            {
                frmSell childForm = new frmSell();
                
                childForm.Take_Mem_Number += new frmSell.Take_NumberDele(childForm_Take_Mem_Number);
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellBase_Cancel")
            {
                frmSell_Cancel childForm = new frmSell_Cancel();
                
                childForm.Take_Mem_Number += new frmSell_Cancel.Take_NumberDele(childForm_Take_Mem_Number);
     
                Child_Form_Load(childForm);
        
            }
            
            if (tm.Tag.ToString() == "R_Web")
            {
                frmSell_R_Web childForm = new frmSell_R_Web();
                
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "frmSell_R_ERP")
            {
                frmSell_R_ERP childForm = new frmSell_R_ERP();

                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellBase_R_01")
            {
                frmSell_R_01 childForm = new frmSell_R_01();
                childForm.Take_Mem_Number += new frmSell_R_01.Take_NumberDele(childForm_Take_Mem_Number);
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellBase_R_02")
            {
                frmSell_R_02 childForm = new frmSell_R_02();
                childForm.Take_Mem_Number += new frmSell_R_02.Take_NumberDele(childForm_Take_Mem_Number);
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellBase_RC_01")
            {
                frmSell_RC_01 childForm = new frmSell_RC_01();
                childForm.Take_Mem_Number += new frmSell_RC_01.Take_NumberDele(childForm_Take_Mem_Number);
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellBase_RC_02")
            {
                frmSell_RC_02 childForm = new frmSell_RC_02();
                childForm.Take_Mem_Number += new frmSell_RC_02.Take_NumberDele(childForm_Take_Mem_Number);
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellBase_RC_03")
            {
                frmSell_RC_03 childForm = new frmSell_RC_03();
                
                childForm.Take_Mem_Number += new frmSell_RC_03.Take_NumberDele(childForm_Take_Mem_Number);
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_001")
            {
                frmSell_Select childForm = new frmSell_Select();
                childForm.Send_Sell_Number += new frmSell_Select.SendNumberDele(childForm_Send_Mem_Number);
                childForm.Send_Mem_Number += new frmSell_Select.Send_Mem_NumberDele(childForm_Send_Mem_Number);
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_History")
            {
                frmSell_Select_History childForm = new frmSell_Select_History();
                
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "SellSelect_Delete")
            {
                frmSell_Select_Delete childForm = new frmSell_Select_Delete();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Detail")
            {
                frmSell_Select_Detail childForm = new frmSell_Select_Detail();
                childForm.Send_Sell_Number += new frmSell_Select_Detail.SendNumberDele(childForm_Send_Mem_Number);
                childForm.Send_Mem_Number += new frmSell_Select_Detail.Send_Mem_NumberDele(childForm_Send_Mem_Number);
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Card")
            {
                frmSell_Select_Card childForm = new frmSell_Select_Card();
                childForm.Send_Sell_Number += new frmSell_Select_Card.SendNumberDele(childForm_Send_Mem_Number);
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Bank")
            {
                frmSell_Select_Bank childForm = new frmSell_Select_Bank();
                
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "SellSelect_Item")
            {
                frmSell_Select_Item childForm = new frmSell_Select_Item();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Sell_Center_Change")
            {
                frmSell_Center_Change childForm = new frmSell_Center_Change();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Sell_SellTF")
            {
                frmSell_Select_SellTF childForm = new frmSell_Select_SellTF();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Sell_SellTF_Delete")
            {
                frmSell_Select_SellTF_Delete childForm = new frmSell_Select_SellTF_Delete();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Sell_SellTF_Cancel")
            {
                frmSell_Select_SellTF_Cancel childForm = new frmSell_Select_SellTF_Cancel();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Sell_Union")
            {
                frmSell_Select_Union childForm = new frmSell_Select_Union();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Rec_Excel_Import")
            {
                frm_Excel_Import_Rec childForm = new frm_Excel_Import_Rec();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Card_Cancel")
            {
                frmSell_Select_Card_Cancel childForm = new frmSell_Select_Card_Cancel();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Card_App")
            {
                frmSell_Select_Card_App childForm = new frmSell_Select_Card_App();
                
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "Sell_Auto_Card")
            {
                frmSell_Auto childForm = new frmSell_Auto();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Sell_Auto_Card_Select")
            {
                frmSell_Auto_Card_Select childForm = new frmSell_Auto_Card_Select();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Bank_App")
            {
                frmSell_Select_Bank_App childForm = new frmSell_Select_Bank_App();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Bank_App_R")
            {
                frmSell_Select_Bank_App_R childForm = new frmSell_Select_Bank_App_R();
                
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "Member_Select_Auto_Delete")
            {
                frmMember_Select_Auto_Delete childForm = new frmMember_Select_Auto_Delete();

                Child_Form_Load(childForm);
            }

            //if (tm.Tag.ToString() == "SellSelect_Bank_App")
            //{
            //    frmSell_Select_Bank_App childForm = new frmSell_Select_Bank_App();
            //    Child_Form_Load(childForm);
            //}



            if (tm.Tag.ToString() == "SellBase_Mem")
            {
                frmSell_Mem childForm = new frmSell_Mem();
                childForm.Take_Mem_Number += new frmSell_Mem.Take_NumberDele(childForm_Take_Mem_Number);
                
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "SellSelect_Mem_001")
            {
                frmSell_Select_Mem childForm = new frmSell_Select_Mem();
                childForm.Send_Sell_Number += new frmSell_Select_Mem.SendNumberDele(childForm_Send_Mem_Number_Mem);
                
                //childForm.Send_Mem_Number += new frmSell_Select_Mem.Send_Mem_NumberDele(childForm_Send_Mem_Number);
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "SellSelect_Detail_Mem_001")
            {
                frmSell_Select_Mem_Item childForm = new frmSell_Select_Mem_Item();
                
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "SellBase_Dev")
            {
               // frmSell_Dev childForm = new frmSell_Dev();
               //// childForm.Take_Mem_Number += new frmSell_Dev.Take_NumberDele(childForm_Take_Mem_Number);
               // Child_Form_Load(childForm);
            }


        }



        private void Mileage_Child(object sender, EventArgs e)
        {
            ToolStripMenuItem tm = (ToolStripMenuItem)sender;

            if (tm.Tag.ToString() == "Mileage_IN_OUT")
            {
                frmMileage_IN_OUT childForm = new frmMileage_IN_OUT();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Mileage_Select")
            {
                frmMileage_Select childForm = new frmMileage_Select();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Mileage_Select_Sub")
            {
                frmMileage_Select_Sub childForm = new frmMileage_Select_Sub();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Mileage_Select_Rest")
            {
                frmMileage_Select_Rest childForm = new frmMileage_Select_Rest();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Mileage_Select_Delete")
            {
                frmMileage_Select_Delete childForm = new frmMileage_Select_Delete();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Mileage_Move")
            {
                frmMileage_Move childForm = new frmMileage_Move();
                
                Child_Form_Load(childForm);
            }
            

            
        }


        private void Sell_Group_Child(object sender, EventArgs e)
        {
            ToolStripMenuItem tm = (ToolStripMenuItem)sender;

            if (tm.Tag.ToString() == "SellSelect_Group_Item")
            {
                frmSell_Select_Group_Item childForm = new frmSell_Select_Group_Item();
                
                Child_Form_Load(childForm);
            }


           


            if (tm.Tag.ToString() == "SellSelect_Group_Card")
            {
                frmSell_Select_Group_Card childForm = new frmSell_Select_Group_Card();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Group_Date")
            {
                frmSell_Select_Group_Date childForm = new frmSell_Select_Group_Date();
                childForm.Send_Sell_Number += new frmSell_Select_Group_Date.SendNumberDele(childForm_Send_Mem_Number);
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Group_Mem_Cen")
            {
                frmSell_Select_Group_Mem_Cen childForm = new frmSell_Select_Group_Mem_Cen();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Group_Mem_Cen_Item")
            {
                frmSell_Select_Group_Mem_Cen_Item childForm = new frmSell_Select_Group_Mem_Cen_Item();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Group_Date_Item")
            {
                frmSell_Select_Group_Date_Item childForm = new frmSell_Select_Group_Date_Item();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Group_Date_Sell_Cen")
            {
                frmSell_Select_Group_Date_Sell_Cen childForm = new frmSell_Select_Group_Date_Sell_Cen();
                
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "SellSelect_Group_Sell_Cen")
            {
                frmSell_Select_Group_Sell_Cen childForm = new frmSell_Select_Group_Sell_Cen();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Group_Sell_Cen_Item")
            {
                frmSell_Select_Group_Sell_Cen_Item childForm = new frmSell_Select_Group_Sell_Cen_Item();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Group_Sell_Cen_Card")
            {
                frmSell_Select_Group_Sell_Cen_Card childForm = new frmSell_Select_Group_Sell_Cen_Card();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Group_Down")
            {
                frmSell_Select_Down childForm = new frmSell_Select_Down();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Group_Down_Save")
            {
                frmSell_Select_Down_Save childForm = new frmSell_Select_Down_Save();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Group_Down_Nom")
            {
                frmSell_Select_Down_Nom childForm = new frmSell_Select_Down_Nom();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "SellSelect_Group_Date_Cacu")
            {
                frmSell_Select_Group_Cacu childForm = new frmSell_Select_Group_Cacu();
                
                Child_Form_Load(childForm);
            }



            if (tm.Tag.ToString() == "Member_Reg_Center")
            {
                frmMember_Select_Group_Center childForm = new frmMember_Select_Group_Center();
                
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "Member_Reg_Date")
            {
                frmMember_Select_Group_Date childForm = new frmMember_Select_Group_Date();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_Reg_Date_Center")
            {
                frmMember_Select_Group_Date_Center childForm = new frmMember_Select_Group_Date_Center();
                
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "Member_Reg_Down")
            {
                frmSell_Select_Down_Mem childForm = new frmSell_Select_Down_Mem();
                
                Child_Form_Load(childForm);
            }

            //

            if (tm.Tag.ToString() == "SellSelect_Group_User")
            {
                frmSell_Select_Group_User childForm = new frmSell_Select_Group_User();
                
                Child_Form_Load(childForm);
            }
        }

        private void Stock_Child(object sender, EventArgs e)
        {
            ToolStripMenuItem tm = (ToolStripMenuItem)sender;

            if (tm.Tag.ToString() == "Stock_Base_Out")
            {
                frmBase_Out_Code childForm = new frmBase_Out_Code();
                
                Child_Form_Load(childForm);
            } 


            if (tm.Tag.ToString() == "Stock_In")
            {
                frmStock_IN childForm = new frmStock_IN();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Stock_Out")
            {
                frmStock_OUT childForm = new frmStock_OUT();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Stock_Out_Sell")
            {
                if (cls_app_static_var.Order_OutPut_Num_TF == 1)
                {
                    frmStock_OUT_Sell_OrderNumber childForm = new frmStock_OUT_Sell_OrderNumber();
                    
                    Child_Form_Load(childForm);
                }
                else
                {
                    frmStock_OUT_Sell childForm = new frmStock_OUT_Sell();
                    
                    Child_Form_Load(childForm);
                }
            }

            if (tm.Tag.ToString() == "Stock_Out_Sell_Check")
            {


                frmStock_OUT_Sell_Check childForm = new frmStock_OUT_Sell_Check();
                
                Child_Form_Load(childForm);
                
            }



            
            if (tm.Tag.ToString() == "Stock_Out_Sell_Cancel")
            {   
                if (cls_app_static_var.Order_OutPut_Num_TF == 1)
                {
                    frmStock_OUT_OrderNumber_Cancel childForm = new frmStock_OUT_OrderNumber_Cancel();
                    
                    Child_Form_Load(childForm);
                }
                else
                {
                    frmStock_OUT_Sell_Cancel childForm = new frmStock_OUT_Sell_Cancel();
                    
                    Child_Form_Load(childForm);
                }
            }



            if (tm.Tag.ToString() == "Stock_In_Sell")
            {
                frmStock_IN_Sell childForm = new frmStock_IN_Sell();
                
                Child_Form_Load(childForm);
            }

             if (tm.Tag.ToString() == "Stock_In_Select")
            {
                frmStock_IN_Select childForm = new frmStock_IN_Select();
                
                Child_Form_Load(childForm);
            }

             if (tm.Tag.ToString() == "Stock_Out_Select")
             {
                 frmStock_OUT_Select childForm = new frmStock_OUT_Select();
                
                Child_Form_Load(childForm);
             }
            

            if (tm.Tag.ToString() == "Stock_IN_Sell_Cancel")
            {
                frmStock_IN_Sell_Cancel childForm = new frmStock_IN_Sell_Cancel();
                
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "Stock_Select_Center")
            {
                frmStock_Select_Center childForm = new frmStock_Select_Center();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Stock_Select_Center_Day")
            {
                frmStock_Select_Center_Day childForm = new frmStock_Select_Center_Day();
                
                Child_Form_Load(childForm);
            }

            

            if (tm.Tag.ToString() == "Stock_Select_Item")
            {
                //frmStock_Select_Item childForm = new frmStock_Select_Item();
                frmStock_Select_Item__2 childForm = new frmStock_Select_Item__2();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Stock_Select_Item_Day")
            {
                frmStock_Select_Item_Day childForm = new frmStock_Select_Item_Day();
                
                Child_Form_Load(childForm);
            }

            

            if (tm.Tag.ToString() == "Stock_Move")
            {
                frmStock_Move childForm = new frmStock_Move();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Stock_Move_Confirm")
            {
                frmStock_Move_Confirm childForm = new frmStock_Move_Confirm();
                
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Stock_Move_Select")
            {
                frmStock_Move_Select childForm = new frmStock_Move_Select();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Stock_Close_Real")
            {
                frmStock_Real childForm = new frmStock_Real();
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "Stock_Close")
            {
                frmStock_Close childForm = new frmStock_Close();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Stock_Close_Cancel")
            {
                frmStock_Close_Cancel e_f = new frmStock_Close_Cancel();
                e_f.ShowDialog();
            }

            if (tm.Tag.ToString() == "Stock_Close_Select")
            {
                frmStock_Close_Select childForm = new frmStock_Close_Select();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Stock_Close_Item_Select")
            {
                frmStock_Close_Item_Select childForm = new frmStock_Close_Item_Select();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Stock_Close_Center")
            {
                frmStock_Close_Center childForm = new frmStock_Close_Center();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Stock_Close_Item")
            {
                frmStock_Close_Item childForm = new frmStock_Close_Item();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Rec_Excel_Import_2")
            {
                frm_Excel_Import_Rec childForm = new frm_Excel_Import_Rec();
                Child_Form_Load(childForm);
            }

        }



        private void Ap_Manager_Child(object sender, EventArgs e)
        {
            ToolStripMenuItem tm = (ToolStripMenuItem)sender;

            if (tm.Tag.ToString() == "Base_User")
            {
                frmBase_User childForm = new frmBase_User();
                //childForm.Send_MainMenu_Info += new frmBase_User.Send_MainMenu_Info_Dele(childForm_Send_MainMenu_Info);
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Base_User_Log")
            {
                frmBase_User_Log childForm = new frmBase_User_Log();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Base_User_Doc_Log")
            {
                frmBase_User_Doc_Log childForm = new frmBase_User_Doc_Log();
                Child_Form_Load(childForm);
            }

            //if (tm.Tag.ToString() == "Base_User_ETC")
            //{
            //    frmBase_User_ETC childForm = new frmBase_User_ETC();
            //    Child_Form_Load(childForm);
            //}

            //if (tm.Tag.ToString() == "Base_User_ETC_2")
            //{
            //    frmBase_User_ETC_Staff e_f = new frmBase_User_ETC_Staff();
            //    e_f.ShowDialog();
            //}

            if (tm.Tag.ToString() == "Base_User_Note")
            {
                frmBase_User_Note childForm = new frmBase_User_Note();
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "Base_User_Fa")
            {
                frmBase_User_Fa childForm = new frmBase_User_Fa();
                childForm.Far_Memnu_Change += new frmBase_User_Fa.Take_Far_Memnu_Change_Dele(User_Far_Menu_Make);

                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "UpDate")
            {
                frmBase_Update e_f = new frmBase_Update();
                e_f.ShowDialog();
            }

            if (tm.Tag.ToString() == "Base_Config_1")
            {
                frmBase_Config e_f = new frmBase_Config();
                e_f.ShowDialog();
            }
            if (tm.Tag.ToString() == "Base_User_Mem")
            {
                frmBase_User_Mem childForm = new frmBase_User_Mem();
                
                Child_Form_Load(childForm);

            }
            if (tm.Tag.ToString() == "Base_Login_Board")
            {
                frmBase_Login_Board childForm = new frmBase_Login_Board();
                
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString() == "Base_Login_Board2")
            {
                frmBase_Login_Board2 childForm = new frmBase_Login_Board2();
                
                childForm.Send_Sell_Number += new frmBase_Login_Board2.SendNumberDele(childForm_Send_Mem_Number);
                childForm.Send_Mem_Number += new frmBase_Login_Board2.Send_Mem_NumberDele(childForm_Send_Mem_Number);
                Child_Form_Load(childForm);
            }


        }


        //void childForm_Send_MainMenu_Info(ref Dictionary<string, bool> Main_Menu)
        //{
        //    Main_Menu.Clear();
                        
        //    //주메뉴상에서 visible 속성을 가져와본다. 현재.. 이사아하게 다 false로 잡혀 잇어서 이방법을 써봄.
        //    foreach (ToolStripMenuItem Baes_1_Menu in menuStrip.Items)
        //    {
        //        for (int cnt = 0; cnt < Baes_1_Menu.DropDownItems.Count; cnt++)
        //        {
        //            if (Baes_1_Menu.DropDownItems[cnt] is ToolStripMenuItem)
        //            {
        //                Main_Menu[Baes_1_Menu.DropDownItems[cnt].Name] = Baes_1_Menu.DropDownItems[cnt].Visible;
        //            }
        //        }
        //    }
        //}





        private void Exit_Ap(object sender, EventArgs e)
        {
            panel_Down.Visible = false;             
            panel_Down.Refresh();
            

            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Exit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
            splitCon.Visible = false;
            //splitCon.Refresh();
            this.Close();

            //Re_Loging();

        }

        private void Re_Loging()
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frm_Login());

            frm_Login childForm = new frm_Login();
            childForm.ShowDialog (); //   .Show();
            
            if (cls_User.gid != "" && cls_User.gid != null)
            {
                //프로그램에서 자주 변경되는 말들을 미리 정의함. 회원은 고객으로 한다든가 후원은 직대라 한다든가 용
                //용어를 변경할때.. 관리하기 위함.
                cls_app_static_var.app_Base_Str_resource = "MLM_Program.Resources.Base_Str_Resource";
                cls_app_static_var.app_base_str_rm = new ResourceManager(cls_app_static_var.app_Base_Str_resource, cls_app_static_var.Assem);

                string StrSql = "";
                ////베이스 폴더에 들어가는 캡션들에 대해서 리소스에서 관리한다
                if (cls_User.gid_CountryCode == "KR")
                {                 
                    cls_app_static_var.User_Time_Zone = "Korea Standard Time";
                    StrSql = "Select Base_L, Kor_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";
                }

                if (cls_User.gid_CountryCode == "La")
                {                 
                    cls_app_static_var.User_Time_Zone = "SE Asia Standard Time";
                    StrSql = "Select Base_L, Eng_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";

                }

                if (cls_User.gid_CountryCode == "Ja")
                {                 
                    cls_app_static_var.User_Time_Zone = "Tokyo Standard Time";
                    StrSql = "Select Base_L, Jap_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";
                }

                if (cls_User.gid_CountryCode == "US")
                {                    
                    cls_app_static_var.User_Time_Zone = "Central Standard Time";
                    StrSql = "Select Base_L, Eng_L AS T_Label From tbl_Base_Label (nolock) Order By Base_L ";
                }
             
                DataSet ds2 = new DataSet();

                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, "tbl_Base_Label", ds2) == false) return;

                int ReCnt2 = Temp_Connect.DataSet_ReCount;

                for (int fi_cnt = 0; fi_cnt <= ReCnt2 - 1; fi_cnt++)
                {
                    if (ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["T_Label"].ToString() != "")
                        cls_app_static_var.Base_Label[ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["Base_L"].ToString()] = ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["T_Label"].ToString();
                    else
                        cls_app_static_var.Base_Label[ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["Base_L"].ToString()] = ds2.Tables["tbl_Base_Label"].Rows[fi_cnt]["Base_L"].ToString();
                }


                if (cls_app_static_var.Program_Update_FileName != "")
                {
                    //Application.Run(new frmBase_Update());
                    frmBase_Update childForm2 = new frmBase_Update();
                    childForm2.ShowDialog(); //   .Show();
                }


                //Application.Run(new MDIMain());

                //구현호
                //MDIMain childForm3 = new MDIMain();
                //childForm3.Show();
                

     
            }               
        }


        private void childForm_Take_Mem_Number(ref string Send_Number, ref string Send_Name)
        {
            if (Mdi_Middle_Send_Number != "")
            {
                Send_Number = Mdi_Middle_Send_Number;
                Send_Name = Mdi_Middle_Send_Name;
            }
            Mdi_Middle_Send_Number = "";
            Mdi_Middle_Send_Name = "";
        }          

        private void childForm_Take_Mem_Number(ref string Send_Number, ref string Send_Name, ref string Send_OrderNumber)
        {
            if (Mdi_Middle_Send_Number != "")
            {

                Send_Number = Mdi_Middle_Send_Number;
                Send_Name = Mdi_Middle_Send_Name;
                Send_OrderNumber = Mdi_Middle_Send_OrderNumber;
            }
            Mdi_Middle_Send_Number = "";
            Mdi_Middle_Send_Name = "";
            Mdi_Middle_Send_OrderNumber = "";
        }


        private void childForm_Send_Mem_Number_Mem(string Send_Number, string Send_Name, string Send_OrderNumber)
        {
            Mdi_Middle_Send_Number = Send_Number;
            Mdi_Middle_Send_Name = Send_Name;
            Mdi_Middle_Send_OrderNumber = Send_OrderNumber;
            frmSell_Mem childForm = new frmSell_Mem();
            
            childForm.Take_Mem_Number += new frmSell_Mem.Take_NumberDele(childForm_Take_Mem_Number);
            Child_Form_Load(childForm);
        }


        private void childForm_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            Mdi_Middle_Send_Number = Send_Number;
            Mdi_Middle_Send_Name = Send_Name;
            frmMember_Update childForm = new frmMember_Update();
            
            childForm.Take_Mem_Number += new frmMember_Update.Take_NumberDele(childForm_Take_Mem_Number);
            Child_Form_Load(childForm);
        }


        private void childForm_Send_Mem_Number(string Send_Number, string Send_Name, string Send_OrderNumber)
        {
            Mdi_Middle_Send_Number = Send_Number;
            Mdi_Middle_Send_Name = Send_Name;
            Mdi_Middle_Send_OrderNumber = Send_OrderNumber;
            frmSell childForm = new frmSell();
            
            childForm.Take_Mem_Number += new frmSell.Take_NumberDele(childForm_Take_Mem_Number);
            Child_Form_Load(childForm);
        }


        //void childForm_SendMsg(string sendMsg)
        //{
        //    frmMember childForm = new frmMember();            
        //    Child_Form_Load(childForm);

        //    frmMember activeChild = (frmMember)this.ActiveMdiChild;
        //    activeChild.textBox1.Text = sendMsg;
        //}




        private void Child_Form_Load(Form childForm)
        {
        
            if (Quick_Menu_TF == 0)
            {
                //Mdi_Middle_Send_Number = "";
                //Mdi_Middle_Send_Name = "";
                //Mdi_Middle_Send_OrderNumber = "";
                cls_User.uSearch_MemberNumber = ""; // 퀵메뉴로 들어온게 아니고 일반적인 메뉴 클릭으로 들어 왓다. 그럼 퀵메뉴 검색 회원번호를 리셋
            }

            int Search_From_TF = 0;
            //동일한 캡션의 폼이 올라와 잇다. 그럼 걍 팅겨 나간다.
            for (int x = 0; x < this.MdiChildren.Length; x++)
            {
                if (this.MdiChildren[x].Name == childForm.Name)
                {
                    Location_Top_Menu(this.MdiChildren[x].Name);

                    //this.MdiChildren[x].Visible = false;
                    //this.MdiChildren[x].WindowState = FormWindowState.Normal;                    
                    //this.MdiChildren[x].WindowState = FormWindowState.Maximized;                                        
                    //this.MdiChildren[x].Visible = true;
                    //this.MdiChildren[x].FormBorderStyle = FormBorderStyle.FixedDialog;
                    //this.MdiChildren[x].FormBorderStyle = FormBorderStyle.Sizable;	//<--  원하는 FormBorderStyle 삽입
                    
                    //this.MdiChildren[x].Refresh();

                    //this.MdiChildren[x].Left = 0; this.MdiChildren[x].Top = -30;
                    //this.MdiChildren[x].Refresh();
                                        
                    //this.MdiChildren[x].WindowState = FormWindowState.Minimized;
                    this.MdiChildren[x].Activate();
                    this.MdiChildren[x].WindowState = FormWindowState.Maximized ;
                    
                    toolStrip_From_2.Text = this.MdiChildren[x].Text;                    
                    Search_From_TF = 1;                  
                                        
                    break;
                }
                
            }


            if (Search_From_TF == 0) //동일한 폼이 없으면 지정된 폼을 로드한다.
            {
                splitCon.Visible = true;

                Set_Top_Menu(childForm.Text, childForm.Name);  //상단에 바로가기 버튼을 만든다.                             

                childForm.StartPosition = FormStartPosition.Manual;
                childForm.MdiParent = this;

                childForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(Child_FormClosed);
                childForm.Activated += new EventHandler(childForm_Activated);

                //childForm.AutoScaleMode = AutoScaleMode.Inherit;
                
                childForm.Location = new Point(0, 0);
                //childForm.WindowState = FormWindowState.Minimized;                
                childForm.WindowState = FormWindowState.Maximized;
                childForm.Show();

                //childForm.Refresh();

                toolStrip_From_2.Text = childForm.Text;

                //childForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                //childForm.FormBorderStyle = FormBorderStyle.Sizable;	//<--  원하는 FormBorderStyle 삽입
                
            }

            Quick_Menu_TF = 0;  //퀵메뉴 체크 관련해서 다시 리셋
        }


        void childForm_Activated(object sender, EventArgs e)
        {

            Form t_f = (Form)sender;
            string Send_Form_Text = t_f.Name;
            Location_Top_Menu(Send_Form_Text);
            panel_Down.Visible = false;
            t_f.Icon = MLM_Program.Resources.Base_ICon.logo48_new; // 아이콘 바꾸는 소스

        }



        private void Child_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form t_f = (Form)sender;
            t_f.Visible = false;
            string Send_Form_Text = t_f.Name ;
            
            Top_Menu[Send_Form_Text].Dispose();
            Top_Menu.Remove(Send_Form_Text);

            

            Dictionary<string, Panel> T2_Top_Menu = new Dictionary<string, Panel>();

            foreach (string t_key in Top_Menu.Keys)
            {
                T2_Top_Menu[t_key] = Top_Menu[t_key];
            }
            Top_Menu.Clear();

            foreach (string t_key in T2_Top_Menu.Keys)
            {
                Top_Menu[t_key] = T2_Top_Menu[t_key];
            }
                        
            string _Form_Name = "";
            if (Top_Menu.Count > 0)
            {
                Form activeChild = this.ActiveMdiChild;
                _Form_Name = activeChild.Name;
                toolStrip_From_2.Text = activeChild.Text;

                
            }
            else
            {
                toolStrip_From_2.Text = "";              
                splitCon.Visible = false;
                panel_Down.Visible = false;
                 
            }

            

            Location_Top_Menu(_Form_Name);

            Over_Menu_Button_Dispose(Send_Form_Text);

            
        }


  


        
        private void Set_Top_Menu(string _Form_Text, string _Form_Name)
        {
            //판넬을 하나 만들고 그 안에 폼 TeXT트가 들어가는 라벨 하나와  X표가 나오는 라벨을 하나 만든다.
            Panel pn = new Panel();
           // Label lb_X = new Label();
            Label lb = new Label();
            
            
           // pn.Controls.Add(lb_X);
            pn.Controls.Add(lb);
            splitCon.Panel1.Controls.Add(pn);

            pn.Left = 0; pn.Top = -1;


            cls_form_Meth cfm = new cls_form_Meth();
            string T_form_text = cfm._chang_base_caption_search(_Form_Text);   //화면이름을 변경한다.

            lb.AutoSize =true ;
            lb.Text = "   " + T_form_text;
            lb.Width = pn.Width; 
            lb.Height = label_B.Height;  
            lb.Left = label_B.Left-2; 
            lb.Top = 0  ;
            lb.TextAlign = label_B.TextAlign;
            lb.Font = label_B.Font;
            lb.BackColor = label_B.BackColor;







            //lb_X.Text = "X";
            //lb_X.AutoSize = label_X.AutoSize;
            //lb_X.Width = label_X.Width; ;
            //lb_X.Height = label_X.Height;


            //lb_X.Left = lb.Left + lb.Width + 4;
            //lb_X.Top = lb.Top + 4;
            //lb_X.Font = label_X.Font;
            //lb_X.TextAlign = lb_X.TextAlign;
            //lb_X.BringToFront();


            

            pn.BorderStyle = BorderStyle.None;
            //pn.Width = lb.Width + lb_X.Width + 7;
            pn.Width = lb.Width  + 7;
            pn.Height = panel_Tab.Height;
            pn.BackgroundImageLayout = panel_Tab.BackgroundImageLayout;

         
            //pn.BackgroundImage = panel.BackgroundImage;
            
            
            lb.AutoSize = false;
            lb.Dock = DockStyle.Fill;


            pn.Visible = true;
            lb.Visible = false;            
            //lb_X.Visible = false;            
            //pn.Refresh();


           
            


            //lb_X.Tag = _Form_Name;
            //lb_X.Click += new System.EventHandler(X_ClickHandler);
            //lb_X.MouseMove += new System.Windows.Forms.MouseEventHandler(X_MouseMoveHandler);
            //lb_X.MouseLeave += new System.EventHandler(X_MouseLeaveHandler);


            lb.Tag = _Form_Name;
            lb.Click += new System.EventHandler(ClickHandler);
            lb.MouseMove += new System.Windows.Forms.MouseEventHandler(MouseMoveHandler);
            lb.MouseLeave += new System.EventHandler(MouseLeaveHandler);


            //새롭게 들어온 넘을 제일 앞으로 보낸다.
            Dictionary<string, Panel> T2_Top_Menu = new Dictionary<string, Panel>();

            foreach (string t_key in Top_Menu.Keys)
            {
                T2_Top_Menu[t_key] = Top_Menu[t_key];
            }
            Top_Menu.Clear();
            Top_Menu[_Form_Name] = pn;

            foreach (string t_key in T2_Top_Menu.Keys)
            {
                Top_Menu[t_key] = T2_Top_Menu[t_key];
            }
                       


            Over_Menu_Button_Make(_Form_Name);

            Location_Top_Menu(_Form_Name);            
        }




        private void Location_Top_Menu(string _Form_Name)
        {
            //splitCon.Visible = false;
            int Base_Pan_W = 0;

            //화면 길이에 맞추서 바로 가기 버튼에 대한 Width를 계산하다.
            if (Top_Menu.Count > 0) Base_Pan_W = (this.Width / Top_Menu.Count) - 3;
            if ((Base_Pan_W >= panel_Tab.Width) || (Base_Pan_W == 0)) Base_Pan_W = panel_Tab.Width;
            int i = 0;

            int t_left = 0;         t_left = 5;

            int over_Cnt = 0;
            
            foreach (string t_key in Top_Menu.Keys)
            {
                //Top_Menu[t_key].Width = Base_Pan_W;

                //바로 가기 버튼에 대한  left 조정
                if ((t_left + Top_Menu[t_key].Width) > splitCon.Panel1.Width)
                {
                    over_Cnt++;
                    if (over_Cnt == 1)
                        t_left = t_left + Top_Menu[t_key].Width;

                }
                
                Top_Menu[t_key].Left = t_left;
                t_left = Top_Menu[t_key].Left + Top_Menu[t_key].Width - 2;
                
                
                //액티브 폼과 연결된 바로 가기 버튼에 대한 작업들
                //판넬은 테두리를 두고. 색깔을 변경하면 Tag에 1을 넣어서 체크해 둔다.
                //그 이외의 판넬에 대해서는 색깔 원래색으로 바꾸고 테두리 없애면 Tag에 ""을 넣는다.
                if (t_key == _Form_Name)
                {                   
                    
                    Top_Menu[t_key].BackgroundImage = panel_Tab.BackgroundImage;
                    Top_Menu[t_key].Tag = "1";
                    Top_Menu[t_key].BringToFront();

                    foreach (Control t_c in Top_Menu[t_key].Controls)
                    {
                        if (t_c is Label)
                        {
                            Label t_lb = (Label)t_c;
                            //t_lb.BackColor = Color.Ivory;

                            if (t_lb.Text == "X")
                            {
                                t_lb.BorderStyle = BorderStyle.None;
                                t_lb.BackColor = Color.Ivory;
                                t_lb.Visible = true;
                                t_lb.Font = label_X.Font;                                
                            }
                            else
                            {
                                t_lb.Visible = true;
                            }
                        }
                    }                   
                }
                else
                {
                    Top_Menu[t_key].BackgroundImage = panel.BackgroundImage;                    
                    Top_Menu[t_key].Tag = "";

                    foreach (Control t_c in Top_Menu[t_key].Controls)
                    {
                        if (t_c is Label)
                        {
                            Label t_lb = (Label)t_c;
                            
                            if (t_lb.Text == "X")
                            {
                                t_lb.BorderStyle = BorderStyle.None;
                                t_lb.Visible = false;
                                t_lb.Font = label_X.Font;                                
                            }
                        }

                    }                                   
                }

                i++;
            }

            //splitCon.Visible = true;
            splitCon.Refresh();
        }


        //기본 텍스트 라벨을 클릭시에.. tag에 심어져 잇는 폼 이름과 동일한 이름의 폼을 찾아서 액티브 시킨다.
        public void ClickHandler(Object sender, System.EventArgs e)
        {
            panel_Down.Visible = false;
             
            Label t_lb = (Label)sender;
            
            for (int x = 0; x < this.MdiChildren.Length; x++)
            {
                if (this.MdiChildren[x].Name  == t_lb.Tag.ToString())
                {
                    //this.MdiChildren[x].Visible = false;
                    //this.MdiChildren[x].WindowState = FormWindowState.Normal;                    
                    //this.MdiChildren[x].WindowState = FormWindowState.Maximized;
                    ////this.MdiChildren[x].Dock = DockStyle.Fill;                    

                    //this.MdiChildren[x].Visible = true;
                    //this.MdiChildren[x].FormBorderStyle = FormBorderStyle.FixedDialog;
                    //this.MdiChildren[x].FormBorderStyle = FormBorderStyle.Sizable;	//<--  원하는 FormBorderStyle 삽입

                    //this.MdiChildren[x].Left = 0; this.MdiChildren[x].Top = -30;
                    //this.MdiChildren[x].Refresh();
                    //this.MdiChildren[x].Activate();


                    //this.MdiChildren[x].WindowState = FormWindowState.Minimized;                    


                     this.MdiChildren[x].Activate();
                    this.MdiChildren[x].WindowState = FormWindowState.Maximized ;
                    //this.MdiChildren[x].Refresh();

                    toolStrip_From_2.Text  = this.MdiChildren[x].Text;

                    break;
                }
            }

            Location_Top_Menu(t_lb.Tag.ToString());            
        }


        //폼의 텍스트가 들어 잇는 라벨에 마우스가 왓다 갓다 할때.
        public void MouseMoveHandler(Object sender, MouseEventArgs e)
        {           
            Label lb = (Label)sender;
            Panel pn = (Panel)lb.Parent;
            //pn.BorderStyle = BorderStyle.FixedSingle;  //라벨의 부로 판넬에 윤곽선을 넣는다. 나 마우스 왓어요
            if (pn.Tag.ToString () != "1")
                pn.BackgroundImage = panel_M.BackgroundImage;
            //라는걸 알리기 위함.

            //라벨에 마우스 왓다 갓다 하면
            foreach (Control t_c in pn.Controls)
            {
                if (t_c is Label)
                {
                    Label t_lb = (Label)t_c;                  
                    if (t_lb.Text == "X")
                    {
                        t_lb.BorderStyle = BorderStyle.None;
                        t_lb.Visible = true;
                        t_lb.Refresh();
                        t_lb.Font = label_X.Font;                        
                    }
                }
            }


            foreach (string t_key in Top_Menu.Keys)
            {
                if ((Top_Menu[t_key].Tag.ToString() == "") && (t_key != lb.Tag.ToString()))
                {

                    Top_Menu[t_key].BorderStyle = BorderStyle.None;

                    foreach (Control t_c in Top_Menu[t_key].Controls)
                    {
                        if (t_c is Label)
                        {
                            Label t_lb = (Label)t_c;
                            if (t_lb.Text == "X")
                            {
                                t_lb.BorderStyle = BorderStyle.None;
                                t_lb.Visible = false;
                                t_lb.Font = label_X.Font;                                
                            }
                        }
                    }
                }
            }

            //toolTip.Show(lb.Text, lb, 0, lb.Height);
        }


        //바로 가기 버튼에서 폼의 텍스트가 나온 라벨에서 마우스가 떠낫을때
        public void MouseLeaveHandler(Object sender, EventArgs e)
        {            
            Label lb = (Label)sender;
            Panel pn = (Panel)lb.Parent; //라벨의 상위 판넬을 찾아낸다.
            //부로 라벨에 바로가기 버튼이 액티브 인지 비액티브 인지를
            //Tag에 심어 놓았기 때문 "" 이면 비액티브  "1"이면 액티브임.

            if (pn.Tag.ToString() == "") //비활성화 바로가기 버튼이다.
            {
                pn.BackgroundImage = panel.BackgroundImage;
            }
            else //액티브된 바로가기 버튼이 아니다
            {
                
                foreach (Control t_c in pn.Controls)
                {
                    if (t_c is Label)
                    {
                        Label t_lb = (Label)t_c;
                        if (t_lb.Text == "X") // X라벨에 마우스 가면 그 윤곽이 생기는데.. 기본 라벨에서도 마우스가 떠낫으면 그 윤곽을 아에 없애버림
                        {
                            t_lb.BorderStyle = BorderStyle.None;                            
                            t_lb.Font = label_X.Font;                            
                        }
                    }
                }  
            }

            //toolTip.Dispose ()
            //toolTip.Hide(lb);

        }



        private void Program_Base_Setting()
        {
            cls_app_static_var.Member_Cpno_Visible_TF = cls_User.gid_Cpno_V_TF; //주민번호 보여주기 셋팅 관련 이건 admin이 사용자 관리 화면에서 설정하게 함
                                                                                //==============-====================================================================


            //cls_app_static_var.str_Currency_Type = "{0:N0}";   //3자리 단위로 콤마
            //cls_app_static_var.str_Currency_Type = "{0:F2}";   //3자리 단위로 콤마
            //cls_app_static_var.str_Currency_Type = "{0:#,##0.00}";// "###,###,##0.00";
            //cls_app_static_var.str_Currency_Type ="{0:c}"; //통화량 단위로 나온다.
            cls_app_static_var.str_Currency_Type = "{0:#,##0}";// "###,###,##0.00";
            cls_app_static_var.str_Currency_Money_Type = "{0:###,###,###,###}";

            //cls_app_static_var.str_Grid_Currency_Type = "###,###,##0.00";            
            cls_app_static_var.str_Grid_Currency_Type = "###,###,##0";

            if (cls_User.gid_CountryCode == "KR" || cls_User.gid_CountryCode == "")
            {
                cls_app_static_var.Base_M_Detail_Ex = "M_Detail_Ex";
                cls_app_static_var.Base_SellTypeName = "SellTypeName";
            }
            else if (cls_User.gid_CountryCode == "TH")
            {
                cls_app_static_var.Base_M_Detail_Ex = "M_Detail_Ex_Eng";    // 230818 - syhuh, 태국버전인 경우 영문으로 나오도록 설정.
                cls_app_static_var.Base_SellTypeName = "SellTypeName_En";

                cls_app_static_var.str_Currency_Type = "{0:#,##0.00}";// "###,###,##0.00";
                cls_app_static_var.str_Currency_Money_Type = "{0:###,###,###,###0.00}";

                //cls_app_static_var.str_Grid_Currency_Type = "###,###,##0.00";            
                cls_app_static_var.str_Grid_Currency_Type = "###,###,##0.00";
            }

            cls_app_static_var.Sell_TF_CS_Flag = ""; // "" 빈칸이면 CS에서 입력되는 매출 건은 다 승인으로 표시하고 N이면 다 미승인으로 한다.
                                 


             
            //==============-====================================================================



            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();
 
            string Tsql = "Select *  From tbl_Config  (nolock)  ";

            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Config", ds) == false) return;

            
            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt <= 0) return;

            cls_app_static_var.T_Company_Code = ds.Tables["tbl_Config"].Rows[0]["Union_Com_Code"].ToString(); //"4001";  //특판조합 관련 회사 승인 코드  Union_Com_Code

            cls_app_static_var.Member_Down_Cnt = int.Parse (ds.Tables["tbl_Config"].Rows[0]["Down_Reg_Cnt"].ToString()) ; //다운라인 수
            cls_app_static_var.Member_Cpno_Error_Check_TF = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Resident_Number_Check"].ToString());  //주민번호 오류 체크해라
            cls_app_static_var.Member_Cpno_Put_TF = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Resident_Number_Check2"].ToString());  //주민번호 필수 입력이다.1   0 필수입력 아니다.
            cls_app_static_var.Member_Reg_Multi_TF = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Many_Account_Check"].ToString()); //동일 주민번호로 해서 중복 가입이 안된다.  1 우선은 되게함

            cls_app_static_var.Center_Code_Length = int.Parse(ds.Tables["tbl_Config"].Rows[0]["buss_Code_Cnt"].ToString());  //센타코드 자리수 우선은 3자리로 셋팅한다.

            cls_app_static_var.Mem_Number_Auto_Base_Mbid = ds.Tables["tbl_Config"].Rows[0]["Base_Mbid_Char"].ToString(); //랜덤이나 자동으로 번호 생성시에 추천인 없을 경우 만들어지는 기준 회원번호 앞자리

            cls_app_static_var.SMS_smsDeptID = ds.Tables["tbl_Config"].Rows[0]["SMS_ID"].ToString(); //ㄴSMS 관련 회사 아이디를 저장한다.

            string t_Member_Format = "";
            cls_app_static_var.Member_Number_1 = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Member_Code1"].ToString());
            cls_app_static_var.Member_Number_2 = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Member_Code2"].ToString());
            if (cls_app_static_var.Member_Number_1 > 0)
            {
                for (int i = 1; i <= cls_app_static_var.Member_Number_1; i++)
                {
                    // t_Member_Format = t_Member_Format +  "0";
                    t_Member_Format = t_Member_Format + "C"; // 문자 형태로 앞자리를 받아들이기를 원한면 마스크 에디트 속성상 문자 아니면 숫자만 셋팅을 맞출수 잇다.
                }

                t_Member_Format = t_Member_Format + "-";
            }
            else
            {
                cls_app_static_var.Mem_Number_Auto_Base_Mbid = ""; //회원번호 앞자리 없으면 랜덤 번호 관련 앞자리 빈칸임.
            }
            
            //회원번호 뒷자리 관련된 셋팅임.
            for (int i = 1; i <= cls_app_static_var.Member_Number_2; i++)
            {
                t_Member_Format = t_Member_Format + "9";
            }
            cls_app_static_var.Member_Number_Fromat = t_Member_Format;
            cls_app_static_var.Tel_Number_Fromat = "999-9999-9999";
            cls_app_static_var.ZipCode_Number_Fromat = "999999";
            cls_app_static_var.Biz_Number_Fromat = "999-99-99999";
            cls_app_static_var.Date_Number_Fromat = "9999-99-99";

            //대중소분류를 사용할지 여부 0 이면 일반 코드 1이면 대중소 사용한다.
            if (int.Parse(ds.Tables["tbl_Config"].Rows[0]["ItemCodeSort"].ToString()) == 0)
            {
                cls_app_static_var.Item_Code_Length = int.Parse(ds.Tables["tbl_Config"].Rows[0]["goods_Code_Cnt"].ToString()); //상품코드 자리수

                cls_app_static_var.Item_Sort_1_Code_Length = 0; //대분류 관련코드
                cls_app_static_var.Item_Sort_2_Code_Length = 0;
                cls_app_static_var.Item_Sort_3_Code_Length = 0;
            }
            else
            {
                cls_app_static_var.Item_Code_Length = 0; //상품코드 자리수

                cls_app_static_var.Item_Sort_1_Code_Length = int.Parse(ds.Tables["tbl_Config"].Rows[0]["ItemCodeCnt1"].ToString()); //대분류 관련코드
                cls_app_static_var.Item_Sort_2_Code_Length = int.Parse(ds.Tables["tbl_Config"].Rows[0]["ItemCodeCnt2"].ToString());
                cls_app_static_var.Item_Sort_3_Code_Length = int.Parse(ds.Tables["tbl_Config"].Rows[0]["ItemCodeCnt3"].ToString());

                if (cls_app_static_var.Item_Sort_1_Code_Length > 0)
                    m_base_Goods_Sort_1.Visible = true;

                if (cls_app_static_var.Item_Sort_2_Code_Length > 0)
                    m_base_Goods_Sort_2.Visible = true;
            }


            //센타 프로그램 우선은 사용함. 1 사용 0 안사용
            cls_app_static_var.Program_User_Center_Sort = int.Parse(ds.Tables["tbl_Config"].Rows[0]["CenterProgram"].ToString());

            //셋트 구성 메뉴를 사용할지 안할지   1사용   0 사용안함.
            cls_app_static_var.Program_Usering_Goods_Set = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Using_GoodsSet_TF"].ToString());

            cls_app_static_var.Using_Mileage_TF = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Using_Mileage_TF"].ToString());  //마일리지관련 프로그램 사용할지 말지  0이면 사용하지 말고 1이면 열어줌            
            cls_app_static_var.Using_ReturnCost_TF = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Using_ReturnCost_TF"].ToString());  //교환 관련 메뉴를 열어줄지 여부  0이면 사용 안하고 1이면 열어줌


            cls_app_static_var.Mem_Number_Auto_Flag = ds.Tables["tbl_Config"].Rows[0]["Mem_Number_Auto_Flag"].ToString();    // A면 자동으로 증가    H면 손수 입력함.    R 이면 랜덤.
            

            cls_app_static_var.Sell_Union_Flag = ds.Tables["tbl_Config"].Rows[0]["Sell_Union_Flag"].ToString();//  //빈칸인 경우 특판도 직판도 아니고.... D 직판  U가 특판이다.


            cls_app_static_var.save_uging_Pr_Flag = int.Parse(ds.Tables["tbl_Config"].Rows[0]["save_uging_Pr_Flag"].ToString());  //프로그램 상에서 후원인 관련 기능을 빼고 싶으면 0   후원인 기능을 넣고 싶으면 10 을 넣는다.
            cls_app_static_var.nom_uging_Pr_Flag = int.Parse(ds.Tables["tbl_Config"].Rows[0]["nom_uging_Pr_Flag"].ToString());  //프로그램 상에서 추천인 관련 기능을 빼고 싶으면 0   추천인 기능을 넣고 싶으면 10 을 넣는다.            
            cls_app_static_var.Member_Reg_Line_Select_TF = int.Parse(ds.Tables["tbl_Config"].Rows[0]["LineChangeCheck"].ToString());// 위치를 선택해라. 1이 선택하고    0은 자동임   회원등록시 위치 지정

            if (cls_app_static_var.save_uging_Pr_Flag == 0)
            {
                cls_app_static_var.Member_Reg_Line_Select_TF = 0;// 후원인 기능을 사용 안하겟다는 거는 위치 지정이 소용이 없음.
                m_Member_Save_Change.Visible = false;
                m_Member_Save_Change.ToolTipText = "-";
                //m_Member_TreeGroup.Visible = false;
                //m_Member_Tree.Visible = false; 
            }

            if (cls_app_static_var.nom_uging_Pr_Flag == 0)
            {
                m_Member_Nom_Change.Visible = false;
                m_Member_Nom_Change.ToolTipText = "-";
                //m_Member_TreeGroup_Nom.Visible = false;
                //m_Member_Tree_Nom.Visible = false;
            }


            //직판이나 일반 사용일 경우에는 조합 관련 메뉴는 안보이게 한다.
            if (cls_app_static_var.Sell_Union_Flag == "" || cls_app_static_var.Sell_Union_Flag == "D")
                //UnionMenu.Visible = false;


            //마일리지 비사용일 경우에는 마일리지 관련 메뉴가 안보이게 한다.
            if (cls_app_static_var.Using_Mileage_TF == 0)
                MileageMenu.Visible = false;

            //셋트 메뉴 사용 안할경우 안보이게 한다.
            if (cls_app_static_var.Program_Usering_Goods_Set == 0)
                m_base_Goods_Set.Visible = false;

            //교환관련 메뉴를 안보이게 함 사용안함으로 체크하면
            if (cls_app_static_var.Using_ReturnCost_TF == 0)
            {
                //m_SellBase_RC_01.Visible = false;
                //m_SellBase_RC_03.Visible = false;

                //m_SellBase_RC_01.Visible = false; m_SellBase_RC_01.Enabled = false; m_SellBase_RC_01.ToolTipText = "-";
                m_SellBase_RC_03.Visible = false; m_SellBase_RC_03.Enabled = false; m_SellBase_RC_03.ToolTipText = "-";
            }
            
            
            //일일이 조회되는 부분을 고치기 보다는 소속 센타 정보를 가져오는 부분에서 소속센타를 없애는 걸로 해서 처리함.
            //센타 프로그램을 사용 안한다고 하면 모든 센타에 대해서 조회가 되므로 로그인한 사람이 데해서 센타 코드를 넣어둘 필요가 없다.
            if (cls_app_static_var.Program_User_Center_Sort == 0)
                cls_User.gid_CenterCode = "";

            cls_app_static_var.Dir_Company_Name = ds.Tables["tbl_Config"].Rows[0]["Com_Name"].ToString();
            cls_app_static_var.Dir_Company_Bos_Name = ds.Tables["tbl_Config"].Rows[0]["Com_Bos_Name"].ToString();
            cls_app_static_var.Dir_Company_Number = ds.Tables["tbl_Config"].Rows[0]["Com_Number"].ToString();
            cls_app_static_var.Dir_Company_Address = ds.Tables["tbl_Config"].Rows[0]["Com_Address"].ToString();
            cls_app_static_var.Dir_Company_P_Number = ds.Tables["tbl_Config"].Rows[0]["Com_P_Number"].ToString();

            if (cls_app_static_var.Sell_Union_Flag == "D")  //직판
            {
                cls_app_static_var.Dir_Company_Code = ds.Tables["tbl_Config"].Rows[0]["Union_Com_Code"].ToString();
                cls_app_static_var.Dir_Socket_Ip = ds.Tables["tbl_Config"].Rows[0]["Com_Ac_IP"].ToString();
                cls_app_static_var.Dir_Socket_Acc_Port = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Com_Ac_Port"].ToString());
                cls_app_static_var.Dir_Socket_Cancel_Port = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Com_Cancel_Port"].ToString());
            }


            if (cls_app_static_var.Sell_Union_Flag == "U")  //특판
            {
                cls_app_static_var.Dir_Company_Code = ds.Tables["tbl_Config"].Rows[0]["Union_Com_Code"].ToString();
                cls_app_static_var.Dir_Socket_Ip = "";
                cls_app_static_var.Dir_Socket_Acc_Port = 0;
                cls_app_static_var.Dir_Socket_Cancel_Port = 0;  
            }


            cls_app_static_var.Rec_info_Multi_TF = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Sell_Address_Multi_TF"].ToString());
            cls_app_static_var.Order_OutPut_Num_TF = int.Parse(ds.Tables["tbl_Config"].Rows[0]["Order_OutPut_Num_TF"].ToString());

            cls_app_static_var.Delivery_Standard = double.Parse(ds.Tables["tbl_Config"].Rows[0]["DeliveryStandard"].ToString());
            cls_app_static_var.Delivery_Charge = double.Parse(ds.Tables["tbl_Config"].Rows[0]["DeliveryCharge"].ToString());

            cls_app_static_var.Delivery_Standard_TH = double.Parse(ds.Tables["tbl_Config"].Rows[0]["DeliveryStandard_TH"].ToString());  // 태국 배송비 무료 금액 기준
            cls_app_static_var.Delivery_Charge_TH = double.Parse(ds.Tables["tbl_Config"].Rows[0]["DeliveryCharge_TH"].ToString());      // 태국 배송비 부과 기준

            /*웹 연동 URL들*/
            cls_app_static_var.AuthURL = ds.Tables["tbl_Config"].Rows[0]["AuthURL"].ToString();                                //--본인인증URL
            cls_app_static_var.CashReceiptURL = ds.Tables["tbl_Config"].Rows[0]["CashReceiptURL"].ToString();                  //--현금영수증취소 URL
            cls_app_static_var.CashCancelURL = ds.Tables["tbl_Config"].Rows[0]["CashCancelURL"].ToString();                    //--현금영수증취소 URL
            cls_app_static_var.ApproveAssociationURL =    ds.Tables["tbl_Config"].Rows[0]["ApproveAssociationURL"].ToString(); //--조합신고URL
            cls_app_static_var.CancelAssociationURL =     ds.Tables["tbl_Config"].Rows[0]["CancelAssociationURL"].ToString();  //--조합취소URL
            cls_app_static_var.AuthURL =                  ds.Tables["tbl_Config"].Rows[0]["AuthURL"].ToString();               //--본인인증URL
            cls_app_static_var.AccountCertifyURL =        ds.Tables["tbl_Config"].Rows[0]["AccountCertifyURL"].ToString();     //--계좌인증 URL
            cls_app_static_var.AddressURL =               ds.Tables["tbl_Config"].Rows[0]["AddressURL"].ToString();            //--우편번호URL
            cls_app_static_var.ApproveCardURL =           ds.Tables["tbl_Config"].Rows[0]["ApproveCardURL"].ToString();        //--카드승인URL
            cls_app_static_var.CancelCardURL =            ds.Tables["tbl_Config"].Rows[0]["CancelCardURL"].ToString();         //--카드취소URL
            cls_app_static_var.ApproveAccountURL =        ds.Tables["tbl_Config"].Rows[0]["ApproveAccountURL"].ToString();     //--가상계좌발행URL
            cls_app_static_var.CancelAccountURL =         ds.Tables["tbl_Config"].Rows[0]["CancelAccountURL"].ToString();      //--가상계좌취소URL
            cls_app_static_var.CashReceiptURL =           ds.Tables["tbl_Config"].Rows[0]["CashReceiptURL"].ToString();        //--현금영수증승인 URL
            cls_app_static_var.CashCancelURL =            ds.Tables["tbl_Config"].Rows[0]["CashCancelURL"].ToString();         //--현금영수증취소 URL
            cls_app_static_var.ApproveCardURL_TH =        ds.Tables["tbl_Config"].Rows[0]["ApproveCardURL_TH"].ToString();     //--카드승인URL - 태국
            cls_app_static_var.CancelCardURL_TH =         ds.Tables["tbl_Config"].Rows[0]["CancelCardURL_TH"].ToString();      //--카드취소URL - 태국
            cls_app_static_var.joinMail_TH = ds.Tables["tbl_Config"].Rows[0]["joinMail_TH"].ToString();           //--회원가입 전송 메일 - 태국
            cls_app_static_var.autoshipMail_TH = ds.Tables["tbl_Config"].Rows[0]["autoshipMail_TH"].ToString();       //--오토십 전송 메일 - 태국
            cls_app_static_var.orderCompleteMail_TH = ds.Tables["tbl_Config"].Rows[0]["orderCompleteMail_TH"].ToString();     //--주문완료 전송 메일 - 태국
            cls_app_static_var.orderCancelMail_TH = ds.Tables["tbl_Config"].Rows[0]["orderCancelMail_TH"].ToString();       //--주문취소 전송 메일 - 태국
            cls_app_static_var.changeNominSaveMail_TH = ds.Tables["tbl_Config"].Rows[0]["changeNominSaveMail_TH"].ToString();   //--추천인/후원인 변경 처리 완료 전송 메일 - 태국
        }



        //private void T_Load()
        //{
        //    ResourceSet rs = null;
        //    //if (cls_User.gid_CountryCode == "KR")
        //    //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);
        //    //if (cls_User.gid_CountryCode == "La")
        //    //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("lo-LA"), true, true);
        //    //if (cls_User.gid_CountryCode == "Ja")
        //    //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("ja-JP"), true, true);
        //    //if (cls_User.gid_CountryCode == "US")
        //    //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), true, true);

        //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);

        //    IDictionaryEnumerator de = rs.GetEnumerator();
        //    de.Reset();

        //     cls_Connect_DB Temp_Connect = new cls_Connect_DB();

        //    while (de.MoveNext())
        //    {                

        //        string StrSql = "insert into tbl_Base_Label (Base_L, Kor_L ) Values ('" + de.Key.ToString()  + "','" + de.Value.ToString() + "')" ;

        //        Temp_Connect.Insert_Data(StrSql, "tbl_Base_Label");

        //    }            
        //}


        //private void T_Load2()
        //{
        //    ResourceSet rs = null;
        //    //if (cls_User.gid_CountryCode == "KR")
        //    //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);
        //    //if (cls_User.gid_CountryCode == "La")
        //    //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("lo-LA"), true, true);
        //    //if (cls_User.gid_CountryCode == "Ja")
        //    //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("ja-JP"), true, true);
        //    //if (cls_User.gid_CountryCode == "US")
        //    //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), true, true);

        //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), true, true);

        //    IDictionaryEnumerator de = rs.GetEnumerator();
        //    de.Reset();

        //    cls_Connect_DB Temp_Connect = new cls_Connect_DB();

        //    while (de.MoveNext())
        //    {


        //        string StrSql = "Update  tbl_Base_Label SET Eng_L = '" +  de.Value.ToString()  + "' Where Base_L ='" + de.Key.ToString() + "'" ;

        //        Temp_Connect.Update_Data(StrSql);

        //    }
        //}

        //private void T_Load3()
        //{
        //    ResourceSet rs = null;

        //    rs = cls_app_static_var.app_base_caption_rm.GetResourceSet(System.Globalization.CultureInfo.CreateSpecificCulture("ja-JP"), true, true);

        //    IDictionaryEnumerator de = rs.GetEnumerator();
        //    de.Reset();

        //    cls_Connect_DB Temp_Connect = new cls_Connect_DB();

        //    while (de.MoveNext())
        //    {


        //        string StrSql = "Update  tbl_Base_Label SET Jap_L = '" + de.Value.ToString() + "' Where Base_L ='" + de.Key.ToString() + "'";

        //        Temp_Connect.Update_Data(StrSql);

        //    }
        //}


        private void mem_inf_Convert()
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();

            string Tsql = "Select  Mbid,Mbid2, Email ,cpno ,Address1,Address2, hometel,hptel, bankaccnt  ,WebID , WebPassWord  From tbl_Memberinfo  (nolock)  ";

            //if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Memberinfo", ds) == false) return;
            //int ReCnt = Temp_Connect.DataSet_ReCount;
            //if (ReCnt <= 0) return;

            //for (int fcnt = 0; fcnt < ReCnt; fcnt++)
            //{

            //    string StrSql = "Update tbl_Memberinfo Set ";
            //    //StrSql = StrSql + " Email = '" + encrypter.Encrypt("Cjc@lycos.co.kr") + "',";
            //    //StrSql = StrSql + " cpno = '" + encrypter.Encrypt("1111111111118") + "',";
            //    StrSql = StrSql + " Address1 = ''";
            //    //StrSql = StrSql + " Address2 = ''";
            //    //StrSql = StrSql + " hometel='" + encrypter.Encrypt(" 02-3333-4444") + "',";
            //    //StrSql = StrSql + " hptel='" + encrypter.Encrypt("010-2222-2222") + "',";
            //    //StrSql = StrSql + " bankaccnt='" + encrypter.Encrypt("1234-1234-1234567") + "',";
            //    //StrSql = StrSql + " BankOwner =''";
            //    //StrSql = StrSql + " M_Name ='테스트',";

            //    //StrSql = StrSql + " WebID='" + encrypter.Encrypt(ds.Tables["tbl_Memberinfo"].Rows[fcnt]["WebID"].ToString()) + "',";
            //    //StrSql = StrSql + " WebPassWord='" + encrypter.Encrypt(ds.Tables["tbl_Memberinfo"].Rows[fcnt]["WebPassWord"].ToString()) + "'";


            //    StrSql = StrSql + " Where mbid2 = " + ds.Tables["tbl_Memberinfo"].Rows[fcnt]["Mbid2"].ToString();
            //    StrSql = StrSql + " And   Mbid ='" + ds.Tables["tbl_Memberinfo"].Rows[fcnt]["Mbid"].ToString() + "'";

            //    Temp_Connect.Update_Data(StrSql);




            //}

            //DataSet ds2 = new DataSet();
            //Tsql = "Select  C_index, OrderNumber, C_Number1   From tbl_Sales_Cacu  (nolock)  ";

            //if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Memberinfo", ds2) == false) return;
            //int ReCnt2 = Temp_Connect.DataSet_ReCount;
            //if (ReCnt2 <= 0) return;

            //for (int fcnt = 0; fcnt < ReCnt2; fcnt++)
            //{

            //    string StrSql = "Update tbl_Sales_Cacu Set ";
            //    StrSql = StrSql + " C_Number1 = '" + encrypter.Encrypt("1234-1234-1234-4321") + "'";
            //    //StrSql = StrSql + " C_Name = '구미자',";
                

            //    StrSql = StrSql + " Where OrderNumber = '" + ds2.Tables["tbl_Memberinfo"].Rows[fcnt]["OrderNumber"].ToString() + "'";
            //    StrSql = StrSql + " And   C_index =" + ds2.Tables["tbl_Memberinfo"].Rows[fcnt]["C_index"].ToString() ;

            //    Temp_Connect.Update_Data(StrSql);

            //}


            //DataSet ds3 = new DataSet();
            //Tsql = "Select  *   From tbl_Sales_Rece  (nolock) where Receive_method > 1  ";

            //if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Memberinfo", ds3) == false) return;
            //int ReCnt3 = Temp_Connect.DataSet_ReCount;
            //if (ReCnt3 <= 0) return;

            //for (int fcnt = 0; fcnt < ReCnt3; fcnt++)
            //{

            //    string StrSql = "Update tbl_Sales_Rece Set ";
            //    StrSql = StrSql + " Get_Name1  = '" + encrypter.Encrypt("1234-1234-1234-4321") + "'";
            //    StrSql = StrSql + ", Get_Address1 = '" + encrypter.Encrypt(ds3.Tables["tbl_Memberinfo"].Rows[fcnt]["Get_Address1"].ToString()) + "'";
            //    StrSql = StrSql + ", Get_Address2 = ''";

            //    if (ds3.Tables["tbl_Memberinfo"].Rows[fcnt]["Get_Tel1"].ToString() != "")
            //        StrSql = StrSql + ", Get_Tel1 = '" + encrypter.Encrypt(" 02-2222-3333") + "'";

            //    if (ds3.Tables["tbl_Memberinfo"].Rows[fcnt]["Get_Tel2"].ToString() != "")
            //        StrSql = StrSql + ", Get_Tel2 = '" + encrypter.Encrypt("010-1111-2222") + "'";


            //    StrSql = StrSql + " Where OrderNumber = '" + ds3.Tables["tbl_Memberinfo"].Rows[fcnt]["OrderNumber"].ToString() + "'";
            //    StrSql = StrSql + " And   Salesitemindex =" + ds3.Tables["tbl_Memberinfo"].Rows[fcnt]["Salesitemindex"].ToString() ;

            //    Temp_Connect.Update_Data(StrSql);

            //}

        }


        private void SetValuesOnSubItems(List<ToolStripMenuItem> items)
        {
            items.ForEach(item =>
            {
                if (item.Visible)
                {
                    var dropdown = (ToolStripDropDownMenu)item.DropDown;
                    if (dropdown != null)
                    {
                        dropdown.ShowImageMargin = false;
                        SetValuesOnSubItems(item.DropDownItems.OfType<ToolStripMenuItem>().ToList());
                    }
                }
            });
        }


        private void MDIMain_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + cls_app_static_var.APP_VER;

            string NA_CODE = cls_User.gid_CountryCode;

            if (!cls_Connect_DB.LiveFlag)
            {
                this.BackgroundImage = null;
                if (NA_CODE == "TH")
                {
                    this.Text = this.Text + "[Development program]";
                }
                else if (NA_CODE == "KR" || NA_CODE == "")
                {
                    this.Text = this.Text + "[개발기 프로그램]";
                }
                this.Login_Board_TF = 0; //로그인보드 강제로 안보이게
            }

            //AA_Check();
            
            if(NA_CODE == "TH")
            {
                Close_Menu.Visible = false;
                UnionMenu.Visible = false;
                this.Login_Board_TF = 0; //로그인보드 강제로 안보이게
            }

            if (cls_User.gid_CountryCode == "TH")
            {
                if (cls_app_static_var.Using_language == "English")
                {
                    this.Text = this.Text + "[Thailand]";
                }
                else
                {
                    this.Text = this.Text + "[태국]";
                }
            }
            else if (cls_User.gid_CountryCode == "KR")
            {
                if (cls_app_static_var.Using_language == "English")
                {
                    this.Text = this.Text + "[Korea]";
                }
                else
                {
                    this.Text = this.Text + "[한국]";
                }
            }


            panel_Down.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlPanel_Paint);
            this.menuStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            ////mem_inf_Convert();
            ////MdiClient ctlMDI;

            ////foreach (Control ctl in this.Controls)
            ////{
            ////    try
            ////    {
            ////        // Attempt to cast the control to type MdiClient.
            ////        ctlMDI = (MdiClient)ctl;

            ////        // Set the BackColor of the MdiClient control.
            ////        ctlMDI.BackColor = this.BackColor;
            ////    }
            ////    catch (InvalidCastException )
            ////    {
            ////        // Catch and ignore the error if casting failed.
            ////    }
            ////}
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new CMenuColorTable());

            //T_Load();
            //T_Load2();
            //T_Load3();

            ////cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            ////string Tsql = "";
            //string Tsql = "Update tbl_config SEt TEst_Fild =  ENCRYPTBYPASSPHRASE('KIM1','750408-1280215')  ";
            //Temp_Connect.Update_Data(Tsql);


            ////DataSet ds2 = new DataSet();
            ////Tsql = "Select CONVERT(VARCHAR(MAX), DECRYPTBYPASSPHRASE('KIM1', TEst_F)) From tbl_config  ";
            ////if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Config", ds2) == false) return;
            ////int ReCnt2 = Temp_Connect.DataSet_ReCount;

            ////string TT2 = ds2.Tables["tbl_Config"].Rows[0][0].ToString();






            form_Loade_TW = 0;


            //다국어를 지원하지 않으면 국과 관련 셋팅을 잡을 필요가 없다.
            if (cls_app_static_var.Using_Multi_language == 0)
            {
                toolStripSeparator11.Visible = false;
                m_base_Goods_Nation.Visible = false;
                m_base_Nation.Visible = false;
            }

            System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(@System.Reflection.Assembly.GetEntryAssembly().Location);
            System.Reflection.AssemblyName name = asm.GetName();

            toolStrip_User_2.Text = cls_User.gid;

            string[] tver = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString().Split('.');            
            toolStrip_Ver_2.Text = "Ver : " + tver[3];

            toolStrip_NowD.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm"); // .ToShortTimeString();

            Program_Base_Setting();   //프로그램 관련된 기본적인 셋팅 사항들을 조정을 한다.
            
            Mdi_Middle_Send_Number = "";
            Mdi_Middle_Send_Name = "";            
             Mdi_Middle_Send_OrderNumber = "";

            cls_User.uSearch_MemberNumber = "";

            

            if (cls_app_static_var.Sell_Union_Flag == "")
            {
                UnionMenu.Visible = false;
            }



            if (cls_app_static_var.Sell_Union_Flag == "D")  //직판
            {
               // m_SellBase_RC_01.Visible = false; m_SellBase_RC_01.Enabled = false; m_SellBase_RC_01.ToolTipText = "-";
                m_SellBase_RC_03.Visible = false; m_SellBase_RC_03.Enabled = false; m_SellBase_RC_03.ToolTipText = "-";
                m_Sell_Union.Visible = false; ; m_Sell_Union.Enabled = false; m_Sell_Union.ToolTipText = "-";

                //특판관련 메뉴들은 안보이게 한다.
                m_Sell_Union_Cancel.Visible = false; ; m_Sell_Union_Cancel.Enabled = false; m_Sell_Union_Cancel.ToolTipText = "-";
                m_Member_Union.Visible = false; ; m_Member_Union.Enabled = false; m_Member_Union.ToolTipText = "-";
                m_Stock_Union.Visible = false; ; m_Stock_Union.Enabled = false; m_Stock_Union.ToolTipText = "-";
                m_Pay_Union.Visible = false; ; m_Pay_Union.Enabled = false; m_Pay_Union.ToolTipText = "-";
            }

            if (cls_app_static_var.Sell_Union_Flag == "U")  //특판
            {

                m_Sell_Dir_Hand.Visible = false; ; m_Sell_Dir_Hand.Enabled = false; m_Sell_Dir_Hand.ToolTipText = "-";
            }

            
            Menu_Text_Chang_KR();  ////메뉴 상에서 들어가는 텍스트들을 알맞게변경을 한다. 외국어 버전을 감안해서 작업한거임.

           

            //프로그램의 특성에 따라 안보이게 해야 되는 메뉴들은 안보이게 한다.
            //기본적으로 기초 자료 관리의 셋트 상품이나 대분류 중분류 같은 것들
            User_Menu_Visual_TF_02();

            

            User_Menu_Visual_TF();  //권한설정에 따라서 보이는 메뉴와 안보이는 메뉴를 구분한다.
            
            if (cls_User.gid == cls_User.SuperUserID)  //최고관리자 관련해서
                User_Menu_Visual_TF_03();
                       

           //User_Far_Menu_Make(1);  //사용자별 즐겨찾기 구성을 한다. 퀵메뉴를 구성한다.
            User_Far_Menu_Make();


            cls_app_static_var.Mdi_Base_Menu = menuStrip;  //텍스트가 변형된 상태에서 메뉴를 변수에 넣는다.



            //////if (cls_User.gid_pan_Info_V_TF == 1) //회원관리에서 가져오는 설정임..
            //////{
            //////    pan_Info.Visible = true;
            //////    panel1.Refresh();

            //////    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //////    Search_Sell_Cnt_1();  //미승인건을 보여준다
            //////    Search_Sell_Cnt_2();  //미출고건을 보여준다.

            //////    if (tablep_Sell.Visible == true)
            //////    {
            //////         if (cls_app_static_var.Sell_Union_Flag == "D")  //직판
            //////             Search_Sell_Cnt_3_D();  //매출조합 미신고건수를 보여준다
            //////         else
            //////             Search_Sell_Cnt_3();  //매출조합 미신고건수를 보여준다
            //////    }
            //////    if (tablep_Stock.Visible == true)
            //////        Search_Sell_Cnt_4();  //매출 조합 출고 미신고건수를 보여준다
            //////    this.Cursor = System.Windows.Forms.Cursors.Default;                              
            //////}
            //////else
            //////    pan_Info.Visible = false;


            SetValuesOnSubItems(this.menuStrip.Items.OfType<ToolStripMenuItem>().ToList());

            //2024-06-17 지성경 추가 : 서브메뉴가 하나도없음녀 대메뉴도 가리기
            TopMenuControl_Visible_Function();
        }
        private void pnlPanel_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
            Pen p = new Pen(Color.Red, 2);
            e.Graphics.DrawRectangle(p, r);
        }



        private void Menu_Text_Chang_KR()
        {
            ////메뉴 상에서 들어가는 텍스트들을 알맞게변경을 한다. 외국어 버전을 감안해서 작업한거임.
            cls_form_Meth cm = new cls_form_Meth();
            //cm._chang_base_caption_search(m_text);
            string m_text = "";
            foreach (ToolStripMenuItem Baes_1_Menu in menuStrip.Items)
            {
                m_text = Baes_1_Menu.Text.ToString();
                Baes_1_Menu.Text = cm._chang_base_caption_search(m_text);

                for (int cnt = 0; cnt < Baes_1_Menu.DropDownItems.Count; cnt++)
                {
                    if (Baes_1_Menu.DropDownItems[cnt] is ToolStripMenuItem)
                    {
                        m_text = Baes_1_Menu.DropDownItems[cnt].Text.ToString();
                        Baes_1_Menu.DropDownItems[cnt].Text = cm._chang_base_caption_search(m_text);
                    }
                }
            }
            
        }





        private void User_Menu_Visual_TF()
        {
            
            if (cls_User.gid == cls_User.SuperUserID) return;

            string Menu1 = "";
            
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select Menu1  ";
            Tsql = Tsql + " From tbl_User  (nolock)  ";
            Tsql = Tsql + " Where User_ID = '" + cls_User.gid.ToString () + "' ";

            DataSet ds = new DataSet();
            
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_User", ds) == true)
            {
                int ReCnt = Temp_Connect.DataSet_ReCount;
                if (ReCnt > 0)
                    Menu1 = ds.Tables["tbl_User"].Rows[0][0].ToString();  //로그인한 사용자 한태 맞는 사용할수 있는 메뉴들을 불러온다.  한물자열로 되어 잇음 구분자는 %
            }

            User_Menu_Visual_TF(Menu1);

            
        }




        private void User_Menu_Visual_TF(string Menu1)
        {
            string[] t_Memu;
            string[] t_Memu_Sub;
            string m_Name = "";


            //우선은 모든 메뉴를 안보이게 한다.
            foreach (ToolStripMenuItem Baes_1_Menu in menuStrip.Items)
            {
                if (cls_app_static_var.Using_language == "English")
                {
                    Baes_1_Menu.Font = new System.Drawing.Font("Tahoma", 9);
                }
                else
                {
                    Baes_1_Menu.Font = new System.Drawing.Font("돋움", 9);  //주메뉴의 글자크기를 변경한다. 이부분은 프로그램 관리자는 안한다. adimn 과 같은 일반 사용자들만
                }

                for (int cnt = 0; cnt < Baes_1_Menu.DropDownItems.Count; cnt++)
                {
                    if (Baes_1_Menu.DropDownItems[cnt] is ToolStripMenuItem)
                    {
                        //if (Baes_1_Menu.DropDownItems[cnt].Name == "m_Base_User_Fa")  //퀵메뉴 추가되는 부분은 모든 사용자들이 다 볼수 잇게 하기위함임.
                        //    Baes_1_Menu.DropDownItems[cnt].Visible = true;
                        //else
                        Baes_1_Menu.DropDownItems[cnt].Visible = false;  //퀵메뉴 제외하고 나머지 메뉴들은 다 안보이게 한다. 우선은...                                                
                        Baes_1_Menu.DropDownItems[cnt].ToolTipText = "-";

                        if (cls_app_static_var.Using_language == "English")
                        {
                            Baes_1_Menu.DropDownItems[cnt].Font = new System.Drawing.Font("Tahoma", 9);
                        }
                        else
                        {
                            Baes_1_Menu.DropDownItems[cnt].Font = new System.Drawing.Font("돋움", 9); //서브 메뉴의 글자크기를 변경한다. 이부분은 프로그램 관리자는 안한다. adimn 과 같은 일반 사용자들만
                        }
                    }
                }
            }

            cls_app_static_var.Mid_Main_Menu.Clear();  //사용자한태 활성화 되어 잇는 메뉴 이름들만 들어가게 된다.


            t_Memu = Menu1.Split('%'); //권한으로 넘어온 텍스트를 배열에 각각 넣는다. 메뉴의 고유명을. 주메뉴는 %  기준이고 그 아래 서브 메뉴들은 / 기준으로 나눔

            //선택된 메뉴만 보이도록 한다.
            for (int cnt = 0; cnt < t_Memu.Length; cnt++)
            {
                if (t_Memu[cnt] != "")
                {
                    t_Memu_Sub = t_Memu[cnt].Split('/'); //서브 메뉴들을 / 을 기준으로 해서 나눠서 배열에 넣는다
                    foreach (ToolStripMenuItem Baes_1_Menu in menuStrip.Items)
                    {
                        m_Name = Baes_1_Menu.Name.ToString();  //메인화면의 메뉴들의 고유이름들을 가져온다.

                        for (int F_cnt = 0; F_cnt < Baes_1_Menu.DropDownItems.Count; F_cnt++)  //주메뉴의 서브 메뉴들을 가져온다.
                        {
                            if (Baes_1_Menu.DropDownItems[F_cnt] is ToolStripMenuItem)
                            {
                                if (t_Memu_Sub[1] == Baes_1_Menu.DropDownItems[F_cnt].Name.ToString() && Baes_1_Menu.DropDownItems[F_cnt].Enabled == true)
                                {
                                    Baes_1_Menu.DropDownItems[F_cnt].Visible = true;
                                    Baes_1_Menu.DropDownItems[F_cnt].ToolTipText = "";
                                    cls_app_static_var.Mid_Main_Menu[Baes_1_Menu.DropDownItems[F_cnt].Name] = true;
                                }
                            }
                        }
                    }
                }
            }


            //admin 하고 프로그램 관리자만. 사용자 관리 메뉴가 보이게 한다.
            if (cls_User.IsAdmin)
            {
                //ToolStripItem Baes_1_Menu = menuStrip.Items["m_Base_User"]; //사용자 관리 메뉴
                //우선은 모든 메뉴를 안보이게 한다.
                foreach (ToolStripMenuItem Baes_1_Menu in menuStrip.Items)
                {
                    for (int cnt = 0; cnt < Baes_1_Menu.DropDownItems.Count; cnt++)
                    {
                        if (Baes_1_Menu.DropDownItems[cnt] is ToolStripMenuItem)
                        {
                            if ("m_Base_User" == Baes_1_Menu.DropDownItems[cnt].Name.ToString() ||
                                "m_Base_User_Doc_Log" == Baes_1_Menu.DropDownItems[cnt].Name.ToString()
                                )
                            {
                                Baes_1_Menu.DropDownItems[cnt].Visible = true;
                                Baes_1_Menu.DropDownItems[cnt].ToolTipText = "";

                            }
                        }
                    }
                }
            }



        }



        private void User_Menu_Visual_TF_02()
        {


            //우선은 모든 메뉴를 안보이게 한다.
            foreach (ToolStripMenuItem Baes_1_Menu in menuStrip.Items)
            {
                for (int cnt = 0; cnt < Baes_1_Menu.DropDownItems.Count; cnt++)
                {
                    if (Baes_1_Menu.DropDownItems[cnt] is ToolStripMenuItem)
                    {
                        //셋트 상품 메뉴 관련해서
                        if ("m_base_Goods_Set" == Baes_1_Menu.DropDownItems[cnt].Name.ToString())
                        {
                            if (cls_app_static_var.Program_Usering_Goods_Set == 0)
                            {
                                Baes_1_Menu.DropDownItems[cnt].Visible = false;
                                Baes_1_Menu.DropDownItems[cnt].Enabled = false;
                                Baes_1_Menu.DropDownItems[cnt].ToolTipText = "-";

                            }
                            else
                            {
                                Baes_1_Menu.DropDownItems[cnt].Visible = true;
                                Baes_1_Menu.DropDownItems[cnt].Enabled = true;
                                Baes_1_Menu.DropDownItems[cnt].ToolTipText = "";

                            }
                        }//셋트 상품 메뉴 관련해서


                        //대분류   중분류  메뉴 관련해서
                        if (
                            "m_base_Goods_Sort_1" == Baes_1_Menu.DropDownItems[cnt].Name.ToString() ||
                            "m_base_Goods_Sort_2" == Baes_1_Menu.DropDownItems[cnt].Name.ToString()
                            )
                        {
                            if (cls_app_static_var.Item_Code_Length > 0)
                            {
                                Baes_1_Menu.DropDownItems[cnt].Visible = false;
                                Baes_1_Menu.DropDownItems[cnt].Enabled = false;
                                Baes_1_Menu.DropDownItems[cnt].ToolTipText = "-";
                            }
                            else
                            {
                                if ("m_base_Goods_Sort_2" == Baes_1_Menu.DropDownItems[cnt].Name.ToString())
                                {
                                    if (cls_app_static_var.Item_Sort_2_Code_Length > 0)
                                    {
                                        Baes_1_Menu.DropDownItems[cnt].Visible = true;
                                        Baes_1_Menu.DropDownItems[cnt].Enabled = true;
                                        Baes_1_Menu.DropDownItems[cnt].ToolTipText = "";

                                    }
                                    else
                                    {
                                        Baes_1_Menu.DropDownItems[cnt].Visible = false;
                                        Baes_1_Menu.DropDownItems[cnt].Enabled = false;
                                        Baes_1_Menu.DropDownItems[cnt].ToolTipText = "-";

                                    }
                                }
                                else
                                {
                                    Baes_1_Menu.DropDownItems[cnt].Visible = true;
                                    Baes_1_Menu.DropDownItems[cnt].Enabled = true;
                                    Baes_1_Menu.DropDownItems[cnt].ToolTipText = "";


                                }
                            }
                        }//대분류   중분류  메뉴 관련해서




                    }
                }
            }
        }



        private void User_Menu_Visual_TF_03()
        {

            cls_app_static_var.Mid_Main_Menu.Clear();

            //주메뉴상에서 visible 속성을 가져와본다. 현재.. 이사아하게 다 false로 잡혀 잇어서 이방법을 써봄.
            foreach (ToolStripMenuItem baes_1_menu in menuStrip.Items)
            {
                for (int cnt = 0; cnt < baes_1_menu.DropDownItems.Count; cnt++)
                {
                    if (baes_1_menu.DropDownItems[cnt] is ToolStripMenuItem)
                    {
                        baes_1_menu.DropDownItems[cnt].Enabled = true;
                        baes_1_menu.DropDownItems[cnt].ToolTipText = "";

                    }
                }
            }

        }

        private void MDIMain_FormClosing(object sender, FormClosingEventArgs e)
        {
          
            panel_Down.Visible = false;
            //panel_Down.Refresh();
            splitCon.Visible = false;
            //splitCon.Refresh();

            if (cls_User.SuperUserID.ToUpper() != cls_User.gid.ToUpper() )    
            {
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();                

                string  StrSql = "" ;
                StrSql = "Update tbl_User Set Log_Check=0 ,lannumber='', Log_Date = '' " ;
                StrSql = StrSql + " Where User_id='" + cls_User.gid.ToUpper()+ "'" ; 
        
                if (Temp_Connect.Update_Data(StrSql, this.Name.ToString(), this.Text) == false) return;

            
        
                StrSql = "Update tbl_User_Con_Log Set ";
                StrSql = StrSql + " End_Time = Convert(Varchar(25),GetDate(),21) " ;
                StrSql = StrSql + " Where T_U_ID = '" + cls_User.gid.ToUpper() + "'";
                StrSql = StrSql + " And End_Time = ''" ;
                StrSql = StrSql + " And Connect_IP = '" + cls_User.computer_ip + "'";
                StrSql = StrSql + " And Connect_C_Name = '" + cls_User.computer_net_name + "'";
                StrSql = StrSql + " And Connect_Time = '" + cls_User.gid_Connect_Time + "'";
        
                if (Temp_Connect.Update_Data(StrSql, this.Name.ToString(), this.Text) == false) return;
            }

            
        }


        private void User_Far_Menu_Make( )
        {
            string FarMenu = "";
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select FarMenu  ";
            Tsql = Tsql + " From tbl_User  (nolock)  ";
            Tsql = Tsql + " Where User_ID = '" + cls_User.gid.ToString() + "' ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_User", ds) == true)
            {
                int ReCnt = Temp_Connect.DataSet_ReCount;
                if (ReCnt > 0)
                    FarMenu = ds.Tables["tbl_User"].Rows[0][0].ToString();
            }

            if (FarMenu != "")
            {
                panel_Left.Controls.Clear(); 
                panel_Left.Visible = true;

                cls_form_Meth cfm = new cls_form_Meth();

                string[] t_Memu;
                t_Memu = FarMenu.Split('%');
                string M_tag = ""; string M_Caption = "";
                int V_TF_Cnt = 0;
                int Fav_Cnt = 0; 

                string[] t_MemuSub;
                string BE_BaseMenu = "";

                for (int cnt = 0; cnt < t_Memu.Length; cnt++)
                {
                    if (t_Memu[cnt] != "")
                    {
                        t_MemuSub = t_Memu[cnt].ToString().Split('/');

                        

                        if (BE_BaseMenu != t_MemuSub[0] && BE_BaseMenu != "")
                        {
                            Fav_Cnt++;
                            BE_BaseMenu = t_MemuSub[0];
                        }
                        if (BE_BaseMenu == "")
                            BE_BaseMenu = t_MemuSub[0]; 


                        foreach (ToolStripMenuItem Baes_1_Menu in menuStrip.Items)
                        {
                            for (int Tcnt = 0; Tcnt < Baes_1_Menu.DropDownItems.Count; Tcnt++)
                            {
                                if (Baes_1_Menu.DropDownItems[Tcnt] is ToolStripMenuItem)
                                {
                                    if (t_MemuSub[1] == Baes_1_Menu.DropDownItems[Tcnt].Name.ToString() && Baes_1_Menu.DropDownItems[Tcnt].ToolTipText == "")
                                    {
                                        //M_tag = Member_Child(Baes_1_Menu.DropDownItems[Tcnt], e);
                                        M_tag = Baes_1_Menu.DropDownItems[Tcnt].Tag.ToString();
                                        M_Caption = Baes_1_Menu.DropDownItems[Tcnt].Text.ToString();

                                        Button t_m = new Button();   
                                        
                                        t_m.Name = M_tag;
                                        t_m.Tag = M_tag;
                                        t_m.Text = M_Caption;
                                        t_m.AutoSize = false;
                                        t_m.Width = panel_Left.Width  - 1;
                                        t_m.Click += new EventHandler(Left_Menu_Click);
                                        t_m.BackColor = SystemColors.Window;
                                        t_m.TextAlign = ContentAlignment.MiddleLeft;

                                        t_m.Font = new System.Drawing.Font("돋움", 9);
                                        t_m.Height = 35;

                                        t_m.FlatStyle = FlatStyle.Flat;                                        
                                        cfm.button_flat_change(t_m);
                                        
                                        panel_Left.Controls.Add(t_m);
                                        t_m.Left = 0;
                                        t_m.Top = t_m.Height * Fav_Cnt + 2 ;
                                        Fav_Cnt++; 


                                        break;
                                    }
                                }
                            }
                        }                   

                    } // end if


                }// end for

            }

        }


        private void Left_Menu_Click(object sender, EventArgs e)
        {
            
            Button bt = (Button)sender;

            foreach (ToolStripMenuItem Baes_1_Menu in menuStrip.Items)
            {
                for (int cnt = 0; cnt < Baes_1_Menu.DropDownItems.Count; cnt++)
                {
                    if (Baes_1_Menu.DropDownItems[cnt] is ToolStripMenuItem)
                    {
                        if (Baes_1_Menu.DropDownItems[cnt].Tag == null)
                            continue;
                        else 
                        if (bt.Tag.ToString() == Baes_1_Menu.DropDownItems[cnt].Tag.ToString())
                        {
                            if (Baes_1_Menu.Name == "BaseInfoMenu")
                                ShowNewForm(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "MemberMenu")
                                Member_Child(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "Sell_Group_Menu")
                                Sell_Group_Child(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "SellMenu")
                                Sell_Child(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "StockMenu")
                                Stock_Child(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "Ap_Manager_Menu")
                                Ap_Manager_Child(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "UnionMenu")
                                m_Sell_Union_Cancel_Click(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "MileageMenu")
                                Mileage_Child(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "Close_Menu")
                                m_Close_Menu_Click(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "SMS_Menu")
                                m_SMS_Click(Baes_1_Menu.DropDownItems[cnt], e);

                            return;
                        }
                    }
                }
            }

        }



        private void User_Far_Menu_Make(int T_F)
        {
            string FarMenu = "";
            contextM.Items.Clear();

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select FarMenu  ";
            Tsql = Tsql + " From tbl_User  (nolock)  ";
            Tsql = Tsql + " Where User_ID = '" + cls_User.gid.ToString() + "' ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_User", ds) == true)
            {
                int ReCnt = Temp_Connect.DataSet_ReCount;
                if (ReCnt > 0)
                    FarMenu = ds.Tables["tbl_User"].Rows[0][0].ToString();
            }

            if (FarMenu != "")
            {

                string[] t_Memu;
                t_Memu = FarMenu.Split('%');
                string M_tag = ""; string M_Caption = "";
                
                string[] t_MemuSub;
                string BE_BaseMenu  = "";
                
                for (int cnt = 0; cnt < t_Memu.Length; cnt++)
                {
                    if (t_Memu[cnt] != "")
                    {
                        t_MemuSub = t_Memu[cnt].ToString().Split('/');

                        if (BE_BaseMenu != t_MemuSub[0] && BE_BaseMenu != "")
                        {
                            ToolStripSeparator t_S = new ToolStripSeparator();
                            contextM.Items.Add(t_S);
                            BE_BaseMenu = t_MemuSub[0];
                        }
                        if (BE_BaseMenu == "")
                            BE_BaseMenu = t_MemuSub[0]; 

                        foreach (ToolStripMenuItem Baes_1_Menu in menuStrip.Items)
                        {
                            for (int Tcnt = 0; Tcnt < Baes_1_Menu.DropDownItems.Count; Tcnt++)
                            {
                                if (Baes_1_Menu.DropDownItems[Tcnt] is ToolStripMenuItem)
                                {
                                    if (t_MemuSub[1] == Baes_1_Menu.DropDownItems[Tcnt].Name.ToString())
                                    {
                                        //M_tag = Member_Child(Baes_1_Menu.DropDownItems[Tcnt], e);
                                        M_tag = Baes_1_Menu.DropDownItems[Tcnt].Tag.ToString() ;
                                        M_Caption = Baes_1_Menu.DropDownItems[Tcnt].Text.ToString();

                                        ToolStripMenuItem t_m = new ToolStripMenuItem();   //) Baes_1_Menu.DropDownItems[Tcnt];
                                        
 
                                        t_m.Name = M_tag;
                                        t_m.Tag = M_tag;
                                        t_m.Text  = M_Caption;
                                        t_m.Click += new EventHandler(t_m_Click);
                                        contextM.Items.Add(t_m);
                                        break;
                                    }
                                }
                            }
                        }                                               

                                 

                    } // end if


                }// end for

            }          

        }

//        ''BaseInfoMenu
//''MemberMenu
//''SellMenu
//''Sell_Group_Menu
//''StockMenu
//''UnionMenu
//''Ap_Manager_Menu

        void t_m_Click(object sender, EventArgs e)
        {
            Quick_Menu_TF = 0;

            Mdi_Middle_Send_Number = "";
            Mdi_Middle_Send_Name = "";
            cls_User.uSearch_MemberNumber = "";


            ToolStripMenuItem bt = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem Baes_1_Menu in menuStrip.Items)
            {
                for (int cnt = 0; cnt < Baes_1_Menu.DropDownItems.Count; cnt++)
                {
                    if (Baes_1_Menu.DropDownItems[cnt] is ToolStripMenuItem)
                    {
                        if (bt.Tag.ToString() == Baes_1_Menu.DropDownItems[cnt].Tag.ToString())
                        {
                            Quick_Menu_TF = 1 ;

                            if (Baes_1_Menu.Name == "BaseInfoMenu")
                                ShowNewForm(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "MemberMenu")
                                Member_Child(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "Sell_Group_Menu")
                                Sell_Group_Child(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "SellMenu")
                                Sell_Child(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "StockMenu")
                                Stock_Child(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "Ap_Manager_Menu")
                                Ap_Manager_Child(Baes_1_Menu.DropDownItems[cnt], e);

                            if (Baes_1_Menu.Name == "UnionMenu")
                                m_Sell_Union_Cancel_Click(Baes_1_Menu.DropDownItems[cnt], e);

                             if (Baes_1_Menu.Name == "MileageMenu")
                                 Mileage_Child(Baes_1_Menu.DropDownItems[cnt], e);

                             if (Baes_1_Menu.Name == "Close_Menu")
                                 m_Close_Menu_Click(Baes_1_Menu.DropDownItems[cnt], e);
                                
                            return;
                        }
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
#if DEBUG
    return;
#endif

            string Tsql = "Select Count(Take_User_id) , Replace(LEFT(Convert(Varchar(25),GetDate(),21),10),'-','')  ";
            Tsql = Tsql + " From  tbl_User_Note  (nolock) ";
            Tsql = Tsql + " Where Take_User_id = '" + cls_User.gid + "'";
            Tsql = Tsql + " And T_Recordtime = '' ";            

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, "", "") == false) return;
            
            toolStripl_Note.Text = "Message : " + ds.Tables[base_db_name].Rows[0][0].ToString();
            
            toolStrip_NowD.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm"); // .ToShortTimeString();

            if (cls_User.gid_date_time == "" )
                cls_User.gid_date_time = toolStrip_NowD.Text.Substring(0, 10).Replace("-", "");
            else
                cls_User.gid_date_time = ds.Tables[base_db_name].Rows[0][1].ToString();


            /*
            if ("admin".ToUpper() != cls_User.gid.ToUpper())
            {
                Tsql = "Select LanNumber ";^
                Tsql = Tsql + " From tbl_user  (nolock)  ";
                Tsql = Tsql + " Where upper(user_id) = '" + cls_User.gid.ToUpper() + "'";

                DataSet ds2 = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                Temp_Connect.Open_Data_Set(Tsql, "tbl_user", ds2);
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt > 0)
                {
                    if (ds2.Tables["tbl_user"].Rows[0][0].ToString() != cls_User.gid_MACAddress)
                    {
                        timer1.Enabled = false;
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Login_ID_3"));
                        this.Close();
                        this.Dispose();
                    }
                }
            }*/

        }

        private void splitCon_Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (string t_key in Top_Menu.Keys)
            {
                if (Top_Menu[t_key].Tag.ToString() == "")
                {
                    Top_Menu[t_key].BorderStyle = BorderStyle.None;

                    foreach (Control t_c in Top_Menu[t_key].Controls)
                    {
                        if (t_c is Label)
                        {
                            Label t_lb = (Label)t_c;
                            if (t_lb.Text == "X")
                            {
                                t_lb.BorderStyle = BorderStyle.None;
                                t_lb.Visible = false;
                                t_lb.Font = label_X.Font;
                            }
                        }
                    }

                }


            }
        }



        private void lbl_D_Click(object sender, EventArgs e)
        {   
            
            if (panel_Down.Visible == false)
            {
                foreach (Control t_c in panel_Down.Controls)
                {
                    if (t_c is Label)
                    {
                        Label t_lb2 = (Label)t_c;

                        if (t_lb2.Tag.ToString() != " ")
                        {
                            t_lb2.Image = null;
                            //t_lb2.Refresh();
                        }
                    }
                }
                //panel_Down.Top = splitCon.Top - panel_Down.Height;
                panel_Down.Left = this.Width - panel_Down.Width - 20 ;
                panel_Down.Visible = true;
            }
            else
                panel_Down.Visible = false; 

            
        }



        private void Sell_Group_Menu_Click(object sender, EventArgs e)
        {

            //panel_Down.Visible = false;
            //this.BaseInfoMenu.Image = global::MLM_Program.Properties.Resources.주문관리2;
        }




        private void Over_Menu_Button_Make(string t_key )
        {
            foreach (Control t_c in panel_Down.Controls)
            {
                if (t_c is Label)
                {
                    Label t_lb = (Label)t_c;

                    if (t_lb.Tag.ToString () == t_key)
                    {
                        return;                        
                    }
                }
            }


         
           
            //판넬을 하나 만들고 그 안에 폼 TeXT트가 들어가는 라벨 하나와  X표가 나오는 라벨을 하나 만든다.
            Label r_lb = new Label();
            r_lb.Tag = t_key;
            panel_Down.Controls.Add(r_lb);
            r_lb.AutoSize = lbl_R_b.AutoSize;

            foreach (Control t_c in Top_Menu[t_key].Controls)
            {
                if (t_c is Label)
                {
                    Label t_lb = (Label)t_c;                   

                    if (t_lb.Text != "X")
                    {
                        r_lb.Text = t_lb.Text;
                        break;
                    }                    
                }
            }

            
            r_lb.Width = lbl_R_b.Width;
            r_lb.Height = lbl_R_b.Height;
            r_lb.Left = lbl_R_b.Left;
            r_lb.TextAlign = lbl_R_b.TextAlign;
            r_lb.ImageAlign = lbl_R_b.ImageAlign;
            r_lb.Font = lbl_R_b.Font;
            r_lb.Click += new EventHandler(r_lb_Click);
            r_lb.MouseMove += new MouseEventHandler(r_lb_MouseMove);
            

            int C_Count = panel_Down.Controls.Count -2;
            r_lb.Top = (r_lb.Height * C_Count) + (2 * C_Count);
            panel_Down.Height = r_lb.Top + r_lb.Height + 2;

        }


        private void Over_Menu_Button_Dispose(string t_key)
        {
            foreach (Control t_c in panel_Down.Controls)
            {
                if (t_c is Label)
                {
                    Label t_lb2 = (Label)t_c;

                    if (t_lb2.Tag.ToString() == t_key)
                    {
                        t_lb2.Visible = false;
                        t_lb2.Dispose(); //폼을 안보이게 하고 죽여 버린다.
                        break ;
                    }
                }
            }

            int C_Count = 0;
            Label t_lb = null; 
            foreach (Control t_c in panel_Down.Controls)
            {
                if (t_c is Label)
                {
                    t_lb = (Label)t_c;

                    if (t_lb.Tag.ToString() != " ")
                    {
                        t_lb.Top = (t_lb.Height * C_Count) + (2 * C_Count);
                        C_Count++;
                    }
                }
            }
            if (C_Count > 0 )
                panel_Down.Height = t_lb.Top + t_lb.Height + 2;
           
        }

        void r_lb_MouseMove(object sender, MouseEventArgs e)
        {
            Label t_lb = (Label)sender;
            t_lb.Image = lbl_R_b.Image;
          //  t_lb.Refresh();
            string _s_Tkey = t_lb.Tag.ToString();

            foreach (Control t_c in panel_Down.Controls)
            {
                if (t_c is Label)
                {
                    Label t_lb2 = (Label)t_c;

                    if (t_lb2.Tag.ToString() != _s_Tkey && t_lb2.Tag.ToString () !=" ")
                    {
                        t_lb2.Image = null;
                        //t_lb2.Refresh();
                    }
                }
            }
        }


        void r_lb_Click(object sender, EventArgs e)
        {
            Label t_lb = (Label)sender;
            string _s_Tkey = t_lb.Tag.ToString();
            Panel t_pn = Top_Menu[_s_Tkey];

            //새롭게 들어온 넘을 제일 앞으로 보낸다.
            Dictionary<string, Panel> T2_Top_Menu = new Dictionary<string, Panel>();

            foreach (string t_key in Top_Menu.Keys)
            {
                T2_Top_Menu[t_key] = Top_Menu[t_key];
            }
            Top_Menu.Clear();
            Top_Menu[_s_Tkey] = t_pn;

            foreach (string t_key in T2_Top_Menu.Keys)
            {
                if (_s_Tkey != t_key )
                    Top_Menu[t_key] = T2_Top_Menu[t_key];
            }

            Location_Top_Menu(_s_Tkey);

            
            
            for (int x = 0; x < this.MdiChildren.Length; x++)
            {
                if (this.MdiChildren[x].Name == _s_Tkey)
                {
                  
                    Location_Top_Menu(this.MdiChildren[x].Name);

                    this.MdiChildren[x].Top = -5;
                    this.MdiChildren[x].Left = -5;
                    this.MdiChildren[x].Width = this.Width - 100;
                    this.MdiChildren[x].Height = this.Height - 100;
                    this.MdiChildren[x].WindowState = FormWindowState.Maximized;
                    //this.MdiChildren[x].Refresh();

                    this.MdiChildren[x].Activate();
                    toolStrip_From_2.Text = this.MdiChildren[x].Text;
                    panel_Down.Visible = false;


                    break;
                }
            }

        }

        private void MDIMain_Resize(object sender, EventArgs e)
        {
            try
            {

                if (form_Loade_TW == 0)
                {
                    form_Loade_TW = 1;

                    string Tsql = "Select Count(Take_User_id)  ";
                    Tsql = Tsql + " From  tbl_User_Note  (nolock) ";
                    Tsql = Tsql + " Where Take_User_id = '" + cls_User.gid + "'";
                    Tsql = Tsql + " And T_Recordtime = '' ";
                    
                    //++++++++++++++++++++++++++++++++
                    cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                    DataSet ds = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;

                    toolStripl_Note.Text = "Message : " + ds.Tables[base_db_name].Rows[0][0].ToString();


                    Tsql = "Select T_ETC, '' ";
                    Tsql = Tsql + ", ''  ";
                    Tsql = Tsql + ",'' ";
                    Tsql = Tsql + ", '' ";
                    Tsql = Tsql + " ,RecordTime     ,T_index , '' , '' ,'' ";
                    Tsql = Tsql + " From  tbl_User_ETC  (nolock) ";

                    Tsql = Tsql + " Where Visible_TF =  1 ";
                    Tsql = Tsql + " And   Visible_Date <= '" + cls_User.gid_date_time + "'";

                    Tsql = Tsql + " And  (Visible_User ='전체'";
                    Tsql = Tsql + " OR    Charindex (Visible_User,'" + cls_User.gid + "') >0 )";
                    Tsql = Tsql + " Order by   T_index desc";

                    //++++++++++++++++++++++++++++++++
                    

                    DataSet ds2 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds2, this.Name, this.Text) == false) return;
                    int ReCnt2 = Temp_Connect.DataSet_ReCount;

                    if (ReCnt2 > 0)
                    {
                        frmBase_User_ETC_Staff e_f = new frmBase_User_ETC_Staff();
                        //구현호
                        //e_f.ShowDialog();
                    }                    
                }


                Form activeChild = this.ActiveMdiChild;
                string _Form_Name = activeChild.Name;
                Location_Top_Menu(_Form_Name);
                panel_Down.Visible = false; 
                
            }
            catch (Exception )
            {
                
            }           
            
        }

        private void m_SMS_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tm = (ToolStripMenuItem)sender;

            if (tm.Tag.ToString() == "SMS_Member")
            {
                frmSMS_Member childForm = new frmSMS_Member();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "MMS_Member")
            {
                frmSMS_Member_Card childForm = new frmSMS_Member_Card();
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "SMS_Select")
            {
                frmSMS_Select childForm = new frmSMS_Select();
                Child_Form_Load(childForm);
            }
            
        }



        private void m_Sell_Union_Cancel_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tm = (ToolStripMenuItem)sender;

            if (tm.Tag.ToString() == "Sell_Union")
            {
                frmSell_Select_Union childForm = new frmSell_Select_Union();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Sell_Union_Cancel")
            {
                frmSell_Select_Union_Cancel childForm = new frmSell_Select_Union_Cancel();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Member_Union")
            {
                frmMember_Select_Union childForm = new frmMember_Select_Union();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Stock_Union")
            {
                frmStock_Select_Union childForm = new frmStock_Select_Union();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Pay_Union")
            {
                frmPay_Select_Union childForm = new frmPay_Select_Union();
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "Sell_Dir_Send")
            {
                frmSell_Select_Insur_Send childForm = new frmSell_Select_Insur_Send();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Sell_Dir_Cancel")
            {
                frmSell_Select_Insur_Cancel childForm = new frmSell_Select_Insur_Cancel();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Sell_Dir_Hand")
            {
                frmSell_Select_D_Hand childForm = new frmSell_Select_D_Hand();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Sell_Dir_Send_TXT")
            {
                frmSell_Select_Insur_TXT childForm = new frmSell_Select_Insur_TXT();
                Child_Form_Load(childForm);
            }

        }



        private void m_Close_Menu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tm = (ToolStripMenuItem)sender;

            if (tm.Tag.ToString() == "frmSell_NEXT_GRADE")
            {
                frmSell_NEXT_GRADE childForm = new frmSell_NEXT_GRADE();
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "Close_4")
            {
                frmClose_4 e_f = new frmClose_4();
                e_f.ShowDialog();
            }

            if (tm.Tag.ToString() == "Close_4_Cancel")
            {
                frmClose_4_Cancel e_f = new frmClose_4_Cancel();
                e_f.ShowDialog();
            }

            if (tm.Tag.ToString() == "Close_4_Select_01")
            {
                frmClose_4_Select_01 childForm = new frmClose_4_Select_01();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_4_Select_03_E")
            {
                frm_Excel_Import_Pay childForm = new frm_Excel_Import_Pay();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_4_Select_03")
            {
                frmClose_4_Select_03 childForm = new frmClose_4_Select_03();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_4_Select_04")
            {
                frmClose_4_Select_04 e_f = new frmClose_4_Select_04();
                e_f.ShowDialog();
            }

            if (tm.Tag.ToString() == "Close_4_Select_Commission")
            {
                frm_Excel_Import_Commission childForm = new frm_Excel_Import_Commission();
                Child_Form_Load(childForm);
            }
            


            if (tm.Tag.ToString() == "Close_Star_Select_01")
            {
                frmClose_Promo_Select_01 childForm = new frmClose_Promo_Select_01();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_Sham_Sell")
            {
                frmClose_Sham_Sell childForm = new frmClose_Sham_Sell();
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "Close_Sham_Point")
            {
                frmClose_Sham_Sell_Down_2 childForm = new frmClose_Sham_Sell_Down_2();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_Sham_Pay")
            {
                frmClose_Sham_Pay childForm = new frmClose_Sham_Pay();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_Sham_Pay_Real")
            {
                frmClose_Sham_Pay_Real childForm = new frmClose_Sham_Pay_Real();
                Child_Form_Load(childForm);

            }

            if (tm.Tag.ToString() == "Close_Sham_Pay_Real_2")
            {
                frmClose_Sham_Pay_Real_2 childForm = new frmClose_Sham_Pay_Real_2();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_Sham_Pay_")
            {
                frmClose_Sham_Pay_ childForm = new frmClose_Sham_Pay_();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_Sham_Pay_Excel_")
            {
                frm_Excel_Import_Pay childForm = new frm_Excel_Import_Pay();
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "Close_Sham_Pay_2")
            {
                frmClose_Sham_Pay_Ded childForm = new frmClose_Sham_Pay_Ded();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_Sham_Pay_2_Select")
            {
                frmClose_Sham_Pay_Ded_Select childForm = new frmClose_Sham_Pay_Ded_Select();
                Child_Form_Load(childForm);
            }


            if (tm.Tag.ToString() == "Close_Pay_Not_Cut")
            {
                frmClose_Pay_Not_Cut childForm = new frmClose_Pay_Not_Cut ();
                Child_Form_Load(childForm);
            }
            

            if (tm.Tag.ToString() == "Close_Sham_Grade_P")
            {
                frmClose_Sham_Grade_P childForm = new frmClose_Sham_Grade_P();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_Sham_Grade")
            {
                frmClose_Sham_Grade childForm = new frmClose_Sham_Grade();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_Member_Group_01")
            {
                frmClose_Member_Group_01 childForm = new frmClose_Member_Group_01();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_Member_CutDed_01")
            {
                frmClose_Member_CutDed_01 childForm = new frmClose_Member_CutDed_01();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_Member_Up_01")
            {
                frmClose_Member_Up_01 childForm = new frmClose_Member_Up_01();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_Member_CutDed_01")
            {
                frmClose_Member_CutDed_01 childForm = new frmClose_Member_CutDed_01();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_PayCut")
            {
                frmClose_Pay_Cut childForm = new frmClose_Pay_Cut();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "Close_Sham_ReturnPay_Cut")
            {
                frmClose_Sham_ReturnPay_Cut childForm = new frmClose_Sham_ReturnPay_Cut();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString() == "New_Promotion")
            {
                frmSell_Select_Promotion childForm = new frmSell_Select_Promotion();
                Child_Form_Load(childForm);
            }

            if (tm.Tag.ToString().Equals("Close_Pay_Cut"))
            {
                frmClose_Pay_Cut childForm = new frmClose_Pay_Cut();
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString().Equals("Close_Sham_Sell_Down_M"))
            {
                frmClose_Sham_Sell_Down_M childForm = new frmClose_Sham_Sell_Down_M();
                Child_Form_Load(childForm);
            }
            if (tm.Tag.ToString().Equals("Close_Sham_Sell_Down_2"))
            {
                frmClose_Sham_Sell_Down_2 childForm = new frmClose_Sham_Sell_Down_2();
                Child_Form_Load(childForm);
            }
        }


        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


        private void toolStripl_Note_Click(object sender, EventArgs e)
        {
            frmBase_User_Note childForm = new frmBase_User_Note();
            Child_Form_Load(childForm);
        }

        private void MDIMain_SizeChanged(object sender, EventArgs e)
        {
            MdiClient ctlMDI;
            foreach (Control ctl in this.Controls)
            {
                try
                {
                    // Attempt to cast the control to type MdiClient.
                    ctlMDI = (MdiClient)ctl;

                    // Set the BackColor of the MdiClient control.
                    ctlMDI.BackColor = Color.White;
                }
                catch (InvalidCastException exc)
                {
                    // Catch and ignore the error if casting failed.
                }
            }

            //this.Refresh();
        }

        private void m_base_Goods_Sort_2_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void MemberMenu_MouseHover(object sender, EventArgs e)
        {

        }

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void menuStrip_MouseEnter(object sender, EventArgs e)
        {
            //if (menuStrip.BackColor != Color.Red)
            //    menuStrip.BackColor = Color.Red;
        }

        private void menuStrip_MouseLeave(object sender, EventArgs e)
        {
            //if (menuStrip.BackColor != Color.Purple)
            //    menuStrip.BackColor = Color.Purple;
        }

        /// <summary>
        /// 2024-06-17 Top Menu 에 Menu Strip 이 하나도 없다면 대메뉴를 가린다.
        /// </summary>
        private void TopMenuControl_Visible_Function()
        {
            //Top Menu 전체 검색
            foreach (ToolStripMenuItem TopMenu in menuStrip.Items.OfType<ToolStripMenuItem>().ToList())
            {
                int TopMenuDropDownItems_Count = TopMenu.DropDownItems.Count;
                bool bPass = false;
                if (TopMenuDropDownItems_Count == 0) continue;

                for (int i = 0; i < TopMenuDropDownItems_Count; i++)
                {
                    if(TopMenu.DropDownItems[i] is ToolStripMenuItem)
                    {
                        ToolStripMenuItem item = TopMenu.DropDownItems[i] as ToolStripMenuItem;
                        string item_Name = item.Name;

                        if (cls_app_static_var.Mid_Main_Menu.ContainsKey(item_Name))

                        {
                            bPass = true ;
  
                            break;
                        }


                    }
                }

                TopMenu.Visible = bPass;

            }


        }

        

    }
}// end MLM_Demo_01
