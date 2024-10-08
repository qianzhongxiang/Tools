﻿using Microsoft.Office.Interop.MSProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRedmine
{
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
                var w = new MSProjectWrapper(pj);
                Wrappers.Add(pj, w);
                return w;
            }
        }
        public RedmineProvider RedmineProvider { get; set; }
        public Project Pj { get; }
        public string Version { get; set; }
        public string RedmineProj { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string UpdateTimeStr => UpdateTime.HasValue ? UpdateTime.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") : "";
        public Dictionary<string, SubVersion> Versions = new Dictionary<string, SubVersion>();
        private MSProjectWrapper(Microsoft.Office.Interop.MSProject.Project pj)
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
            RedmineProvider = new RedmineProvider(RedmineProj, Version);
        }
        public void SaveParameters()
        {
            Pj.Comments = $"{RedmineProj};{Version};{UpdateTimeStr}";
        }

        public void Update()
        {
            //TODO 每个已有未完成任务 获取，判断是否一至，不一至更新
            ThisAddIn.Fresh = true;
            try
            {
                if (Pj.Comments is null)
                {
                    return;
                }
                if (UpdateTime is null)
                {
                    System.Windows.Forms.MessageBox.Show("please LoadData first");
                    return;
                }

                var now = DateTime.UtcNow;

                foreach (var item in RedmineProvider.Versions)
                {
                    var subv = new SubVersion(this, item, RedmineProvider);
                    subv.UpdateTasks();
                }
                UpdateTime = now;
                SaveParameters();
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                ThisAddIn.Fresh = false;
            }
        }

    }
}
