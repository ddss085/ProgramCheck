using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ProgramCheck
{
    /// <summary>
    /// 프로그램 정보를 저장합니다.
    /// </summary>
    class ProgramInformation
    {
        public string PrgName { get; set; }
        public string InstallString { get; set; }
        public string DisplayName { get; private set; }
        public string DisplayVersion { get; private set; }
        public string InstallDate { get; private set; }
        public string Publisher { get; private set; }
        public string UninstallString { get; private set; }
        private RegistryKey prgkey;
 
        public ProgramInformation(string prgname, string installstr)
        {
            PrgName = prgname;
            InstallString = installstr;
            DisplayName = null;
            DisplayVersion = null;
            InstallDate = null;
            Publisher = null;
            UninstallString = null;
            prgkey = null;
        }

        /// <summary>
        /// 프로그램을 설치합니다.
        /// </summary>
        public void Setup()
        {
            { CMD(InstallString); }
        }

        /// <summary>
        /// 프로그램을 삭제합니다.
        /// </summary>
        public bool Delete()
        {
            //try { Process.Start(UninstallString); }
            //catch (Exception ex)
            { CMD(UninstallString); }

            return true;
        }


        /// <summary>
        /// 프로그램 설치 여부를 반환합니다.
        /// </summary>
        public bool IsInstalled()
        {
            RegistryKey key;

            if (PrgName == "HMI")
                if (File.Exists(@"D:\WintechAutomation\HMI\HMI.exe")) return true;
                else return false;
            // search in: CurrentUser
            key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            if (key != null)
                if (Serch(key))
                {
                    return true;
                }

            // search in: LocalMachine_32
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            if (key != null)
                if (Serch(key))
                {
                    return true;
                }
            // search in: LocalMachine_64
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            if (key != null)
                if (Serch(key))
                {
                    return true;
                }
            // NOT FOUND
            return false;
        }
        /// <summary>
        /// 프로그램 정보를 받아옵니다.
        /// </summary>
        public void Set()
        {
            if (prgkey != null)
            {
                DisplayName = prgkey.GetValue("Displayname") as string;
                DisplayVersion = prgkey.GetValue("DisplayVersion") as string;
                InstallDate = prgkey.GetValue("InstallDate") as string;
                Publisher = prgkey.GetValue("Publisher") as string;
                UninstallString = prgkey.GetValue("UninstallString") as string;
                if (UninstallString.IndexOf("\"C:") == -1)
                    UninstallString = UninstallString.Replace("C:", "\"C:");
                if (UninstallString.IndexOf(".exe\"") == -1)
                    UninstallString = UninstallString.Replace(".exe", ".exe\"");
                UninstallString = UninstallString.Replace("Msi", "\"Msi");
            }
            else
            {
                DisplayName = null;
                DisplayVersion = null;
                InstallDate = null;
                Publisher = null;
                UninstallString = null;
            }
        }

        private void CMD(string struninstall)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = @"D:\";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            process.EnableRaisingEvents = false;
            process.StartInfo = startInfo;
            process.Start();
            //프로세스 시작
            process.StandardInput.Write(struninstall + Environment.NewLine); //예를 들어 dir명령어를 입력 
            process.StandardInput.Close();
            string result = process.StandardOutput.ReadToEnd(); //실행결과를 standard output으로 받아와 string값에 저장 
            string error = process.StandardError.ReadToEnd(); //오류유무를 standard output으로 받아와 string값에 저장 
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[ Result Info ]\r\n"); //출력 
            sb.Append(result);
            sb.Append("\r\n");
            sb.Append("[ Error Info ]\r\n");
            sb.Append(error);
            process.WaitForExit();
            process.Close();
        }
        private bool Serch(RegistryKey key)
        {
            foreach (string keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                if (PrgName.Equals(subkey.GetValue("DisplayName") as string, StringComparison.OrdinalIgnoreCase) == true)
                {
                    this.prgkey = subkey;
                    return true;
                }
            }
            this.prgkey = null;
            return false;
        }

    }

}
