using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPBatchCleaner
{
    public class Setting
    {
        private static Setting _instance;
        public static Setting Instance => _instance ?? (_instance = Init());
        private static Setting Init()
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Setting));
            var dir = System.IO.Path.GetDirectoryName(assembly.Location);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Setting>(System.IO.File.ReadAllText(System.IO.Path.Combine(dir, "setting.json")));
        }
        public string UserName { get; set; } = "installpackage";
        public string Pwd { get; set; } = "cn22Q6Yd";
        public string HostName { get; set; } = "192.168.10.101";
        public int ReserveCount { get; set; }
        public List<string> Dirs { get; set; }
        public List<string> DirsForOnlyFiles { get; set; }
    }
}
