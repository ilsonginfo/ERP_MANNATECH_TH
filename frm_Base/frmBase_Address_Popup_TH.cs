using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLM_Program
{
    public partial class frmBase_Address_Popup_TH : Form
    {
        public class CData
        {
            public string Zipcode = string.Empty;
            public string Province_Name = string.Empty;
            /// <summary>
            /// tbl_Memberinfo.city 가된다.
            /// </summary>
            public string Province_Code = string.Empty;
            /// <summary>
            /// tbl_Memberinfo.state 가 된다.
            /// </summary>
            public string District = string.Empty;
            public string SubDistrict = string.Empty;
            public string SubDistrictCode = string.Empty;
            
            /// <summary> 태국 Address1 에 쓰인다 </summary>
            public string Get_FullAddress
            {
                get
                {
                    return $"{SubDistrict} {District} {Province_Name}"; 
                }
            }

            public bool IsOK = false;
        }


        public CData Data = new CData();

        public frmBase_Address_Popup_TH()
        {
            InitializeComponent();
        }

        private void InitComboZipCode_TH()
        {

            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("SELECT ZIPCODE_NM FROM dbo.ufn_Get_ZipCode_State_TH() ORDER BY ZIPCODE_SORT ");
            sb.AppendLine("SELECT * FROM ufn_Get_ZipCode_Province_TH() ORDER BY MinSubDistrictID ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            cbProvince_TH.DataBindings.Clear();
            cbProvince_TH.DataSource = ds.Tables["ZipCode_NM"];
            cbProvince_TH.DisplayMember = "ZipCode_NM";
            cbProvince_TH.ValueMember = "ProvinceCode";
            cbProvince_TH.Font = new Font("Tahoma", 11f);

        }

        private void cbProvince_TH_SelectedIndexChanged(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("SELECT ZIPCODE_NM FROM dbo.ufn_Get_ZipCode_City_TH('" + cbProvince_TH.Text + "') ORDER BY ZIPCODE_SORT ");
            sb.AppendLine("SELECT ZIPCODE_NM FROM ufn_Get_ZipCode_District_TH('" + cbProvince_TH.Text + "') ORDER BY MinSubDistrictID ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            cbDistrict_TH.DataBindings.Clear();
            cbDistrict_TH.DataSource = ds.Tables["ZipCode_NM"];
            cbDistrict_TH.DisplayMember = "ZipCode_NM";
            cbDistrict_TH.Font = new Font("Tahoma", 11f);

        }

        private void cbDistrict_TH_SelectedIndexChanged(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine("SELECT * FROM dbo.ufn_Get_ZipCode_TH('" + cbDistrict_TH.Text + "') ");
            sb.AppendLine("SELECT ZIPCODE_NM FROM dbo.ufn_Get_ZipCode_SubDistrict_TH('" + cbDistrict_TH.Text + "') ORDER BY MinSubDistrictID ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            cbSubDistrict_TH.DataBindings.Clear();
            cbSubDistrict_TH.DataSource = ds.Tables["ZipCode_NM"];
            cbSubDistrict_TH.DisplayMember = "ZipCode_NM";
            cbSubDistrict_TH.Font = new Font("Tahoma", 11f);
        }

        private void cbSubDistrict_TH_SelectedIndexChanged(object sender, EventArgs e)
        {
            cls_Connect_DB Temp_conn = new cls_Connect_DB();
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT [ZIPCODE_NM] = PostCode, SubDistrictCode = ISNULL(LEFT(SubDistrictID, 4), '')  FROM TLS_ZIPCODE_CS WITH(NOLOCK) WHERE SubDistrictThaiShort = '" + cbSubDistrict_TH.Text + "' ");

            if (Temp_conn.Open_Data_Set(sb.ToString(), "ZipCode_NM", ds) == false) return;

            if (Temp_conn.DataSet_ReCount <= 0) return;

            
            Data.Zipcode = ds.Tables["ZipCode_NM"].Rows[0][0].ToString();
            Data.SubDistrictCode = ds.Tables["ZipCode_NM"].Rows[0][1].ToString(); 
            //2024-06-27 따로 뺌            txtAddress2.Text = cbSubDistrict_TH.Text + " " + cbDistrict_TH.Text + " " + cbProvince_TH.Text;
        }

        private void frmBase_Address_Popup_TH_Load(object sender, EventArgs e)
        {

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            cbProvince_TH.Font = new Font("Tahoma", 11f);
            cbDistrict_TH.Font = new Font("Tahoma", 11f);
            cbSubDistrict_TH.Font = new Font("Tahoma", 11f);

            InitComboZipCode_TH();
            cbSubDistrict_TH_SelectedIndexChanged(this, null);
        }

        private void butt_Save_Click(object sender, EventArgs e)
        {
            Data.Province_Name = cbProvince_TH.Text;
            Data.Province_Code = cbProvince_TH.SelectedValue.ToString();
            Data.District = cbDistrict_TH.Text;
            Data.SubDistrict = cbSubDistrict_TH.Text;
            Data.IsOK = true;

            this.Close();
        }

        private void butt_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
