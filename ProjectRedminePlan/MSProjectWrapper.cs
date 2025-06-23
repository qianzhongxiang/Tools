using Microsoft.Office.Interop.MSProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using msproj = Microsoft.Office.Interop.MSProject;
namespace ProjectRedmine
{
    public enum ProjectType
    {
        Plan,
        Resource
    }

    public class MSProjectWrapper
    {
        public static Dictionary<Project, MSProjectWrapper> Wrappers { get; set; } = new Dictionary<Project, MSProjectWrapper>();

        public static MSProjectWrapper CreateOrNewWrapper(Microsoft.Office.Interop.MSProject.Project pj)
        {
            if (Wrappers.ContainsKey(pj))
            {
                return Wrappers[pj];
            }
            else
            {
                if (string.IsNullOrWhiteSpace(pj.Comments))
                {
                    return default;
                }
                if (pj is null)
                {
                    throw new ArgumentNullException(nameof(pj));
                }
                var comments = (pj.Comments as string).Split(';');
                if (comments[0].ToLower() != "plan")
                {

                    return default;
                }
                var w = new MSProjectWrapper();
                switch (comments[0].ToLower())
                {
                    case "plan":
                        w.ProjType = ProjectType.Plan;
                        break;
                    case "res":
                        w.ProjType = ProjectType.Resource;
                        break;
                }
                w.Pj = pj;
                w.RedmineProj = comments[1];
                w.Version = comments[2];
                if (comments.Length > 2)
                {
                    if (string.IsNullOrWhiteSpace(comments[3]))
                    {
                        w.UpdateTime = null;
                    }
                    else
                    {
                        w.UpdateTime = DateTime.Parse(comments[3]);
                    }
                }
                w.RedmineProvider = new RedmineProvider(w.RedmineProj, w.Version);
                Wrappers.Add(pj, w);
                return w;
            }
        }
        public RedmineProvider RedmineProvider { get; set; }
        public Project Pj { get; set; }
        public string Version { get; set; }
        public ProjectType ProjType { get; set; }
        public string RedmineProj { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string UpdateTimeStr => UpdateTime.HasValue ? UpdateTime.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") : "";
        public IEnumerable<msproj.Task> Versions()
        {
           return Pj.Tasks.Cast<msproj.Task>().Where(i => i.OutlineLevel == 1).ToList();
        }

        public void SaveParameters()
        {
            var projTypeStr = "";
            switch (ProjType)
            {
                case ProjectType.Plan:
                    projTypeStr = "Plan";
                    break;
                case ProjectType.Resource:
                    projTypeStr = "Res";
                    break;
                default:
                    break;
            }
            Pj.Comments = $"{projTypeStr};{RedmineProj};{Version};{UpdateTimeStr}";
        }

    }
}
