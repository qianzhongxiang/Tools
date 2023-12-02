using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComMonitor
{
    public class Setting
    {
        public static Setting Instance = new Setting();
        private Setting()
        {
            Newtonsoft.Json.JsonConvert.PopulateObject(File.ReadAllText("ComMonitorSetting.json"),this);
        }
        public Protcol P1 { get; set; }
        public Protcol P2 { get; set; }
    }


}
