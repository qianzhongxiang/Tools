using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRedmine
{
    public  class RedmineProvider
    {
        public  RedmineManager Manager { get; set; }
        public Project Project { get; set; }
        public List<Redmine.Net.Api.Types.Version> Versions { get; set; }

        public RedmineProvider(string proj,string version)
        {
            var host = "http://www.vppms.tech";
            var apiKey = "4110d8c8eb1729ad3a9ec13b6f505155c242efa2";
            Manager = new RedmineManager(host, apiKey);

            var allProjs = Manager.GetObjects<Project>();

            Project = allProjs.Find(p => p.Name.Contains(proj));

            var parameters = new NameValueCollection { };
            parameters.Add(RedmineKeys.PROJECT_ID, Project.Id.ToString());
            var versions = Manager.GetObjects<Redmine.Net.Api.Types.Version>(parameters);
            Versions = versions.Where(v => v.Name.StartsWith(version)).ToList();
            Versions.Sort(new VersionCompare());
        }

        
    }
}
