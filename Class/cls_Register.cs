using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

using Microsoft.Win32;

namespace MLM_Program
{
    /// <summary>
    /// HKEY_LOCAL_MACHINE/Software/IlsongInfomationFramework 생성된 레지스트리 읽기 밎 저장 ->최소 권한 시 문제 발생
    /// Software\Microsoft\Windows\myApp
    /// </summary>
    public class cls_Register
    {
        /// <summary>
        /// 
        /// </summary>
        public cls_Register()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //
        }

        /// <summary>
        /// 노드에서 검색하여 key 해당하는 값을 리턴
        /// </summary>
        /// <param name="node">검색노드명</param>
        /// <param name="key">찾고자 하는 key</param>
        /// <returns>key에 해당하는 값을 리턴</returns>
        public static string GetRigistryProfile(string node, string key)
        {
            try
            {
                string targetValue = string.Empty;

                RegistryKey SoftwareKey = null;
                RegistryKey StephenInformationKey = null;
                RegistryKey targetNode = null;

                if (Wow.Is64BitOperatingSystem)
                {
                    SoftwareKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\\Wow6432Node");
                }
                else
                {
                    SoftwareKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software");
                }
                StephenInformationKey = SoftwareKey.OpenSubKey("IlsongInfomationFramework");

                if (StephenInformationKey != null)
                {
                    targetNode = StephenInformationKey.OpenSubKey(node);
                    if (targetNode != null)
                    {
                        targetValue = (String)targetNode.GetValue(key);

                        /* 레지스트리에 없는 경우 string.empty */
                        if (targetValue == null)
                            targetValue = string.Empty;
                    }
                }

                if (targetNode != null)
                    targetNode.Close();

                if (StephenInformationKey != null)
                    StephenInformationKey.Close();

                if (SoftwareKey != null)
                    SoftwareKey.Close();

                return targetValue;
            }
            catch (System.Exception ex)
            {
                throw ex;

            }
            finally { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetRegistryProfile(string node, string key, string value)
        {
            Microsoft.Win32.RegistryKey SoftwareKey = null;
            Microsoft.Win32.RegistryKey StephenInformationKey = null;
            Microsoft.Win32.RegistryKey targetNode = null;

            string strReturn = string.Empty;
            try
            {
                if (Wow.Is64BitOperatingSystem)
                {
                    SoftwareKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\\Wow6432Node", true);
                }
                else
                {
                    SoftwareKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", true);

                }

                StephenInformationKey = SoftwareKey.OpenSubKey("IlsongInfomationFramework", true);
                if (StephenInformationKey == null)
                {
                    SoftwareKey.CreateSubKey("IlsongInfomationFramework", RegistryKeyPermissionCheck.ReadWriteSubTree);
                    StephenInformationKey = SoftwareKey.OpenSubKey("IlsongInfomationFramework", true);
                }

                targetNode = StephenInformationKey.OpenSubKey(node, true);
                if (targetNode == null)
                {
                    StephenInformationKey.CreateSubKey(node, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    targetNode = StephenInformationKey.OpenSubKey(node, true);
                }
                targetNode.SetValue(key, value);
            }
            catch (System.Exception ex)
            {
                throw ex;

            }
            finally
            {
                if (targetNode != null)
                    targetNode.Close();

                if (StephenInformationKey != null)
                    StephenInformationKey.Close();

                if (SoftwareKey != null)
                    SoftwareKey.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registerName"></param>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetNewRegistryProfile(string registerName, string node, string key, string value)
        {
            Microsoft.Win32.RegistryKey SoftwareKey = null;
            Microsoft.Win32.RegistryKey StephenInformationKey = null;
            Microsoft.Win32.RegistryKey targetNode = null;

            string strReturn = string.Empty;
            try
            {
                if (Wow.Is64BitOperatingSystem)
                {
                    SoftwareKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\\Wow6432Node", true);
                }
                else
                {
                    SoftwareKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", true);

                }

                StephenInformationKey = SoftwareKey.OpenSubKey(registerName, true);
                if (StephenInformationKey == null)
                {
                    SoftwareKey.CreateSubKey(registerName, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    StephenInformationKey = SoftwareKey.OpenSubKey(registerName, true);
                }

                targetNode = StephenInformationKey.OpenSubKey(node, true);
                if (targetNode == null)
                {
                    StephenInformationKey.CreateSubKey(node, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    targetNode = StephenInformationKey.OpenSubKey(node, true);
                }
                targetNode.SetValue(key, value);
            }
            catch (System.Exception ex)
            {
                throw ex;

            }
            finally
            {
                if (targetNode != null)
                    targetNode.Close();

                if (StephenInformationKey != null)
                    StephenInformationKey.Close();

                if (SoftwareKey != null)
                    SoftwareKey.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Wow
        {
            /// <summary>
            /// 
            /// </summary>
            public static bool Is64BitProcess
            {
                get { return IntPtr.Size == 8; }
            }

            /// <summary>
            /// 
            /// </summary>
            public static bool Is64BitOperatingSystem
            {
                get
                {
                    // Clearly if this is a 64-bit process we must be on a 64-bit OS.
                    if (Is64BitProcess)
                        return true;
                    // Ok, so we are a 32-bit process, but is the OS 64-bit?
                    // If we are running under Wow64 than the OS is 64-bit.
                    bool isWow64;
                    return ModuleContainsFunction("kernel32.dll", "IsWow64Process") && IsWow64Process(GetCurrentProcess(), out isWow64) && isWow64;
                }
            }

            static bool ModuleContainsFunction(string moduleName, string methodName)
            {
                IntPtr hModule = GetModuleHandle(moduleName);
                if (hModule != IntPtr.Zero)
                    return GetProcAddress(hModule, methodName) != IntPtr.Zero;
                return false;
            }

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            extern static bool IsWow64Process(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] out bool isWow64);
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            extern static IntPtr GetCurrentProcess();
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            extern static IntPtr GetModuleHandle(string moduleName);
            [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
            extern static IntPtr GetProcAddress(IntPtr hModule, string methodName);
        }
    }

}
