using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace MLM_Program
{
    public class StringEncrypter
    {

        private System.Text.UTF8Encoding utf8Encoding;
        private RijndaelManaged rijndael;


        /// <summary>
        /// Creates a StringEncrypter instance.
        /// </summary>
        /// <param name="key">A key string which is converted into UTF-8 and hashed by MD5.
        /// Null or an empty string is not allowed.</param>
        /// <param name="initialVector">An initial vector string which is converted into UTF-8
        /// and hashed by MD5. Null or an empty string is not allowed.</param>
        public StringEncrypter(string key, string initialVector)
        {

            if (key == null || key == "")
                throw new ArgumentException("The key can not be null or an empty string.", "key");

            if (initialVector == null || initialVector == "")
                throw new ArgumentException("The initial vector can not be null or an empty string.", "initialVector");



            // This is an encoder which converts a string into a UTF-8 byte array.
            this.utf8Encoding = new System.Text.UTF8Encoding();



            // Create a AES algorithm.
            this.rijndael = new RijndaelManaged();

            // Set cipher and padding mode.
            this.rijndael.Mode = CipherMode.CBC;
            this.rijndael.Padding = PaddingMode.PKCS7;

            // Set key and block size.
            const int chunkSize = 128;

            this.rijndael.KeySize = chunkSize;
            this.rijndael.BlockSize = chunkSize;

            // Initialize an encryption key and an initial vector.
            MD5 md5 = new MD5CryptoServiceProvider();
            this.rijndael.Key = md5.ComputeHash(this.utf8Encoding.GetBytes(key)); ;
            this.rijndael.IV = md5.ComputeHash(this.utf8Encoding.GetBytes(initialVector));
        }


        /// <summary>
        /// Encrypts a string.
        /// </summary>
        /// <param name="value">A string to encrypt. It is converted into UTF-8 before being encrypted.
        /// Null is regarded as an empty string.</param>
        /// <returns>An encrypted string.</returns>
        public string Encrypt(string value)
        {
            //return value;

            if (value == null || value == "")
            {
                value = "";
                return value;
            }

            string key = "22D61BE4645E314466758CA23CC02B029FDA1654DDD3E1EF6F8916C5A67F1E4D";
            Encoding encoding = Encoding.Unicode;

            MemoryStream ms = null;
            CryptoStream cs = null;

            RijndaelManaged aes = new RijndaelManaged();

            byte[] textData = encoding.GetBytes(value);
            byte[] salt = Encoding.ASCII.GetBytes(key.Length.ToString());
            PasswordDeriveBytes secretKey = new PasswordDeriveBytes(key, salt);

            ICryptoTransform encryptor = aes.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
            ms = new MemoryStream();
            cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);

            cs.Write(textData, 0, textData.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());


            //// Get an encryptor interface.
            //ICryptoTransform transform = this.rijndael.CreateEncryptor();

            //// Get a UTF-8 byte array from a unicode string.
            //byte[] utf8Value = this.utf8Encoding.GetBytes(value);

            //// Encrypt the UTF-8 byte array.
            //byte[] encryptedValue = transform.TransformFinalBlock(utf8Value, 0, utf8Value.Length);

            //// Return a base64 encoded string of the encrypted byte array.
            //return Convert.ToBase64String(encryptedValue);
        }




        /// <summary>
        /// Decrypts a string which is encrypted with the same key and initial vector. 
        /// </summary>
        /// <param name="value">A string to decrypt. It must be a string encrypted with the same key and initial vector.
        /// Null or an empty string is not allowed.</param>
        /// <returns>A decrypted string</returns>
        public string Decrypt(string value, string Cpno_Format = "")
        {


            if (value == null || value == "")
            {
                return value;
            }

            //if (value.IndexOf("/") > 0 || value.IndexOf("+") > 0 || value.IndexOf("=") > 0)
            //{
            //    int i = 0;
            //}
            //else
            //{
            //   // return value;
            //}

            //////else
            //////{
            //////    if (Cpno_Format != "")
            //////    {
            //////        if (value.Length == 13)
            //////        {
            //////            if (Cpno_Format == "Cpno_D")
            //////                value = value.Substring(0, 6) + value.Substring(6, 5) + "**";
            //////            else
            //////            {
            //////                if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
            //////                    value = value.Substring(0, 6) + "-" + value.Substring(6, 7);
            //////                else
            //////                    value = value.Substring(0, 6) + "-" + "*******";
            //////            }
            //////        }
            //////    }

            //////    return value;
            //////}
            ////////throw new ArgumentException("The cipher string can not be null or an empty string.");

            //////return value;

            try
            {
                //// Get an decryptor interface.
                //ICryptoTransform transform = rijndael.CreateDecryptor();

                //// Get an encrypted byte array from a base64 encoded string.
                //byte[] encryptedValue = Convert.FromBase64String(value);

                //// Decrypt the byte array.
                //byte[] decryptedValue = transform.TransformFinalBlock(encryptedValue, 0, encryptedValue.Length);

                //// Return a string converted from the UTF-8 byte array.
                //string Decrypt_str = this.utf8Encoding.GetString(decryptedValue);
                /*
                string PassKey = "22D61BE4645E314466758CA23CC02B029FDA1654DDD3E1EF6F8916C5A67F1E4D";

                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(PassKey);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var decrypt = aes.CreateDecryptor();
                byte[] xBuff = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Convert.FromBase64String(value);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }
                */

                string key = "22D61BE4645E314466758CA23CC02B029FDA1654DDD3E1EF6F8916C5A67F1E4D";
                Encoding encoding = Encoding.Unicode;

                MemoryStream ms = null;
                CryptoStream cs = null;

                RijndaelManaged aes = new RijndaelManaged();

                byte[] encryptData = Convert.FromBase64String(value);
                byte[] salt = Encoding.ASCII.GetBytes(key.Length.ToString());
                PasswordDeriveBytes secretKey = new PasswordDeriveBytes(key, salt);

                ICryptoTransform decryptor = aes.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
                ms = new MemoryStream(encryptData);
                cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                byte[] result = new byte[encryptData.Length];
                int decryptedCount = cs.Read(result, 0, result.Length);



                String Decrypt_str = encoding.GetString(result, 0, decryptedCount);
                //return Output;

                if (Cpno_Format != "")
                {
                    //태국에경우에는 Mask 처리나 Format 을 처리할필요가없다.
                    if(Cpno_Format.ToLower().Contains("cpno") && cls_User.Is_TH_User )
                    {

                    }
                    else if (Decrypt_str.Length == 13)
                    {
                        if (Cpno_Format == "Cpno_D")
                            Decrypt_str = Decrypt_str.Substring(0, 6) + Decrypt_str.Substring(6, 5) + "**";

                        else if (Cpno_Format == "Cpno_Union")
                            Decrypt_str = Decrypt_str.Substring(0, 7);

                        else if (Cpno_Format == "Cpno_U")
                            Decrypt_str = Decrypt_str.Substring(0, 7) + "******";

                        else if (Cpno_Format == "Cpno_Center")
                            Decrypt_str = Decrypt_str.Substring(0, 2) + "****-" + Decrypt_str.Substring(6, 1) + "******";

                        else
                        {
                            if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
                                Decrypt_str = Decrypt_str.Substring(0, 6) + "-" + Decrypt_str.Substring(6, 7);
                            else
                                Decrypt_str = Decrypt_str.Substring(0, 6) + "-" + "*******";
                        }
                    }

                    if (Cpno_Format == "C_Card")
                    {
                        if (cls_app_static_var.Member_Card_Num_Visible_TF == 0)
                        {
                            string T_St = "*****************************";
                            int T_Len = Decrypt_str.Length;
                            if (T_Len > 3)
                            {
                                T_St = T_St + Decrypt_str.Substring(T_Len - 4, 4);

                                Decrypt_str = T_St.Substring(34 - (T_Len + 1), T_Len);
                            }
                        }
                    }
                }

                return Decrypt_str;
            }

            catch (Exception)
            {
                if (Cpno_Format != "")
                {
                    //태국에경우에는 Mask 처리나 Format 을 처리할필요가없다.
                    if (Cpno_Format.ToLower().Contains("cpno") && cls_User.Is_TH_User)
                    {

                    }
                    else if(value.Length == 13)
                    {
                        if (Cpno_Format == "Cpno_D")
                            value = value.Substring(0, 6) + value.Substring(6, 5) + "**";
                        else if (Cpno_Format == "Cpno_Union")
                            value = value.Substring(0, 7);
                        else if (Cpno_Format == "Cpno_U")
                            value = value.Substring(0, 7) + "******";

                        else
                        {
                            if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
                                value = value.Substring(0, 6) + "-" + value.Substring(6, 7);
                            else
                                value = value.Substring(0, 6) + "-" + value.Substring(6, 1) + "******";
                        }
                    }

                    if (Cpno_Format == "C_Card")
                    {
                        string T_St = "*****************************";
                        int T_Len = value.Length;

                        if (T_Len > 3)
                        {
                            T_St = T_St + value.Substring(T_Len - 4, 4);

                            value = T_St.Substring(34 - (T_Len + 1), T_Len);
                        }
                    }
                }

                return value;
            }


        }




        public string Decrypt(string value, byte[] encryptedValue, string Cpno_Format = "")
        {

            if (value == null || value == "")
                return value;



            try
            {
                // Get an decryptor interface.
                ICryptoTransform transform = rijndael.CreateDecryptor();

                // Get an encrypted byte array from a base64 encoded string.
                //byte[] encryptedValue = Convert.FromBase64String(value);

                // Decrypt the byte array.
                byte[] decryptedValue = transform.TransformFinalBlock(encryptedValue, 0, encryptedValue.Length);

                // Return a string converted from the UTF-8 byte array.
                string Decrypt_str = this.utf8Encoding.GetString(decryptedValue);

                if (Cpno_Format != "")
                {
                    //태국에경우에는 Mask 처리나 Format 을 처리할필요가없다.
                    if (Cpno_Format.ToLower().Contains("cpno") && cls_User.Is_TH_User)
                    {

                    }
                    else if (Decrypt_str.Length == 13)
                    {
                        if (Cpno_Format == "Cpno_D")
                            Decrypt_str = Decrypt_str.Substring(0, 6) + Decrypt_str.Substring(6, 5) + "**";

                        else if (Cpno_Format == "Cpno_Union")
                            Decrypt_str = Decrypt_str.Substring(0, 7);

                        else if (Cpno_Format == "Cpno_U")
                            Decrypt_str = Decrypt_str.Substring(0, 7) + "******";

                        else if (Cpno_Format == "Cpno_Center")
                            Decrypt_str = Decrypt_str.Substring(0, 2) + "****-" + Decrypt_str.Substring(6, 1) + "******";

                        else
                        {
                            if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
                                Decrypt_str = Decrypt_str.Substring(0, 6) + "-" + Decrypt_str.Substring(6, 7);
                            else
                                Decrypt_str = Decrypt_str.Substring(0, 6) + "-" + "*******";
                        }
                    }

                    if (Cpno_Format == "C_Card")
                    {
                        if (cls_app_static_var.Member_Card_Num_Visible_TF == 0)
                        {
                            string T_St = "*****************************";
                            int T_Len = Decrypt_str.Length;

                            T_St = T_St + Decrypt_str.Substring(T_Len - 4, 4);

                            Decrypt_str = T_St.Substring(34 - (T_Len + 1), T_Len);
                        }
                    }
                }

                return Decrypt_str;
            }

            catch (Exception)
            {
                if (Cpno_Format != "")
                {
                    if (value.Length == 13)
                    {
                        //태국에경우에는 Mask 처리나 Format 을 처리할필요가없다.
                        if (Cpno_Format.ToLower().Contains("cpno") && cls_User.Is_TH_User)
                        {

                        }
                        else if (Cpno_Format == "Cpno_D")
                            value = value.Substring(0, 6) + value.Substring(6, 5) + "**";
                        else if (Cpno_Format == "Cpno_Union")
                            value = value.Substring(0, 7);
                        else if (Cpno_Format == "Cpno_U")
                            value = value.Substring(0, 7) + "******";

                        else
                        {
                            if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
                                value = value.Substring(0, 6) + "-" + value.Substring(6, 7);
                            else
                                value = value.Substring(0, 6) + "-" + "*******";
                        }
                    }

                    if (Cpno_Format == "C_Card")
                    {
                        string T_St = "*****************************";
                        int T_Len = value.Length;

                        T_St = T_St + value.Substring(T_Len - 4, 4);

                        value = T_St.Substring(34 - (T_Len + 1), T_Len);
                    }
                }

                return value;
            }


        }







    }
}
