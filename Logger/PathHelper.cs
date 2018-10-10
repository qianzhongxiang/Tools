using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DONN.Tools.Logger
{
    public class PathHelper
    {
        // public static string GetWindowsServiceInstallPath(string ServiceName)
        // {
        //     string key = @"SYSTEM\CurrentControlSet\Services\" + ServiceName;
        //     string path = Registry.LocalMachine.OpenSubKey(key).GetValue("ImagePath").ToString();
        //     //替换掉双引号
        //     path = path.Replace("\"", string.Empty);
        //     FileInfo fi = new FileInfo(path);
        //     return fi.Directory.ToString();
        // }
        public static string GetCurrentDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
