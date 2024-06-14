using Microsoft.Office.Interop.MSProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRedmine
{
    public class MSProjectWrapper
    {
        public Project Pj { get; }
        public string Version { get; set; }
        public string RedmineProj { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string UpdateTimeStr => UpdateTime.HasValue ? UpdateTime.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") : "";

        public MSProjectWrapper(Microsoft.Office.Interop.MSProject.Project pj)
        {
            if (pj is null)
            {
                throw new ArgumentNullException(nameof(pj));
            }
            Pj = pj;
            var comments = (pj.Comments as string).Split(';');
            RedmineProj = comments[0];
            Version = comments[1];
            if (comments.Length > 2)
            {
                if (string.IsNullOrWhiteSpace(comments[2]))
                {
                    UpdateTime = null;
                }
                else
                {
                    UpdateTime = DateTime.Parse(comments[2]);
                }
            }
        }
        public void SaveParameters()
        {
            Pj.Comments = $"{RedmineProj};{Version};{UpdateTimeStr}";
        }

    }
}
