using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using XmlyAlbumDownloader.Common;

namespace XmlyAlbumDownloaderI.ViewModels
{
    public class AppConfig
    {
        /// <summary>
        /// 临时目录
        /// </summary>
        public static string TempFolder
        {
            get
            {
                var path = Path.Combine(Path.GetTempPath(), AppName + "\\");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        private static string _downloadFolder;

        // <summary>
        // 下载目录
        // </summary>
        public static string DownloadFolder
        {
            get
            {
                if (!string.IsNullOrEmpty(_downloadFolder))
                    return _downloadFolder;

                var p = Helper.ReadIniValue("config", "DownloadFolder", ConfigFilePath);
                if (!string.IsNullOrEmpty(p))
                {
                    _downloadFolder = p;
                    return _downloadFolder;
                }

                var path = "d:\\XmlyAlbumDownloader";
                _downloadFolder = Path.Combine(path, AppName);
                if (!Directory.Exists(_downloadFolder))
                {
                    Directory.CreateDirectory(_downloadFolder);
                }
                return _downloadFolder;
            }
            set
            {
                Helper.WriteIniValue("config", "DownloadFolder", value, ConfigFilePath);
                _downloadFolder = value;
            }
        }

        /// <summary>
        /// App名称
        /// </summary>
        public static string AppName
        {
            get
            {
                return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyProductAttribute>().Product; 
            }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public static string Version
        {
            get
            {
                return "v" + Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
        }

        // <summary>
        // 配置文件路径
        // </summary>
        public static string ConfigFilePath
        {
            get
            {

                return TempFolder + "\\config.ini";
            }
        }
    }
}
