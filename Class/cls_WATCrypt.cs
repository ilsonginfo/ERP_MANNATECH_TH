using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Data;


namespace MLM_Program
{
    class cls_WATCrypt
    {
        byte[] Skey = new byte[8];

        public cls_WATCrypt(string strKey)
        {
            Skey = ASCIIEncoding.ASCII.GetBytes(strKey);
        }

        public string Encrypt(string p_data)
        {
            //DESCryptoServiceProvider rc2 = new DESCryptoServiceProvider();

            //rc2.Key = Skey;
            //rc2.IV = Skey;

            //MemoryStream ms = new MemoryStream();
            //CryptoStream cryStream = new CryptoStream(ms, rc2.CreateEncryptor(),
            //CryptoStreamMode.Write);

            //byte[] data = Encoding.UTF8.GetBytes(p_data.ToCharArray());
            //cryStream.Write(data, 0, data.Length);
            //cryStream.FlushFinalBlock();

            //return Convert.ToBase64String(ms.ToArray());



            DESCryptoServiceProvider rc2 = new DESCryptoServiceProvider();

            rc2.Key = Skey;
            rc2.IV = Skey;

            MemoryStream ms = new MemoryStream();
            CryptoStream cryStream = new CryptoStream(ms, rc2.CreateEncryptor(),
                CryptoStreamMode.Write);

            byte[] data = Encoding.UTF8.GetBytes(p_data.ToCharArray());
            cryStream.Write(data, 0, data.Length);
            cryStream.FlushFinalBlock();

            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string p_data)
        {
            //DESCryptoServiceProvider rc2 = new DESCryptoServiceProvider();

            //rc2.Key = Skey;
            //rc2.IV = Skey;

            //MemoryStream ms = new MemoryStream();
            //CryptoStream cryStream = new CryptoStream(ms, rc2.CreateEncryptor(),
            //CryptoStreamMode.Write);

            //byte[] data = Convert.FromBase64String(p_data);
            //cryStream.Write(data, 0, data.Length);
            //cryStream.FlushFinalBlock();

            //return Encoding.UTF8.GetString(ms.GetBuffer());
            DESCryptoServiceProvider rc2 = new DESCryptoServiceProvider();

            rc2.Key = Skey;
            rc2.IV = Skey;

            MemoryStream ms = new MemoryStream();
            CryptoStream cryStream = new CryptoStream(ms, rc2.CreateDecryptor(),
                CryptoStreamMode.Write);

            byte[] data = Convert.FromBase64String(p_data);
            cryStream.Write(data, 0, data.Length);
            cryStream.FlushFinalBlock();

            return Encoding.UTF8.GetString(ms.GetBuffer());

        }

    }

}
