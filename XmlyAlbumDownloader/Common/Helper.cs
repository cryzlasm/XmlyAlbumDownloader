using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XmlyAlbumDownloader.Common
{
    public static class Helper
    {

        #region ini文件
        /// <summary>
        /// 读Ini文件
        /// </summary>
        /// <param name="section">[缓冲区]</param>
        /// <param name="Key">键</param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadIniValue(string section, string Key, string path)
        {
            StringBuilder temp = new StringBuilder(255);

            int i = GetPrivateProfileString(section, Key, "", temp, 255, path);
            return temp.ToString();
        }

        /// <summary>
        /// 写Ini文件
        /// </summary>
        /// <param name="section">[缓冲区]</param>
        /// <param name="Key">键</param>
        /// <param name="value">值</param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void WriteIniValue(string section, string Key, string value, string path)
        {
            WritePrivateProfileString(section, Key, value, path);
        }

        [DllImport("kernel32")] //返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")] //返回取得字符串缓冲区的长度
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        #endregion

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="folder"></param>
        public static bool OpenFileFolder(string folder)
        {
            try
            {
                if (!Directory.Exists(folder))
                    return false;

                ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe"); ;
                psi.Arguments = " /n," + folder;
                Process.Start(psi);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 打开进程
        /// </summary>
        /// <param name="url"></param>
        public static void ProcessStart(string url)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception)
            {
            }
        }

        public static string RemoveInvalidFileNameChars(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;

            string invalid = new string(Path.GetInvalidFileNameChars());
            StringBuilder sb = new StringBuilder();
            foreach (char c in fileName)
            {
                if (!invalid.Contains(c)) sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
