using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetPackageUploader
{
    public class Setting
    {
        public const string SettingName= "setting.json";
        private static Setting _instance;
        public static Setting Instance => _instance ?? (_instance = Init());
        private static Setting Init()
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Setting));
            var dir = System.IO.Path.GetDirectoryName(assembly.Location);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Setting>(System.IO.File.ReadAllText(System.IO.Path.Combine(dir, SettingName)));
        }
        public void Save()
        {

        }
        [JsonIgnore]
        public string UserName { get; set; } = "installpackage";
        [JsonIgnore]
        public string Pwd { get; set; } = "cn22Q6Yd";
        [JsonIgnore]
        public string HostName { get; set; } = "192.168.10.101";
        public string LocalDir { get; set; }
        public string RemoteDir { get; set; }
        public string BagetDir { get; set; }
    }
    public class Profile
    {
        public string Updated { get; set; }
    }
}
