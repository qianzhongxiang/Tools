using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MSProject = Microsoft.Office.Interop.MSProject;
using Office = Microsoft.Office.Core;

namespace ProjectRedmine
{
    public class SubVersion
    {
        public MSProject.Task MainTask { get; set; }
        //public MSProject.Task DemandTask { get; set; }
        public MSProject.Task DesignTask { get; set; }
        public MSProject.Task DevelopTask { get; set; }
        public MSProject.Task TestTask { get; set; }
        public MSProject.Project Pj { get; }
        public MSProjectWrapper MSProjectWrapper { get; }
        public string Version { get; }
        public RedmineProvider RedmineProvider { get; }
        public const int DesignId = 7;
        public const int FunctionId = 2;
        public const int CodeMergeId = 6;
        public const int DemandId = 4;
        public const int IssueId = 1;

        public SubVersion(MSProjectWrapper mSProjectWrapper, string version, MSProject.Task task, RedmineProvider redmineProvider)
        {
            MSProjectWrapper = mSProjectWrapper;
            Pj = MSProjectWrapper.Pj;
            Version = version;
            MainTask = task;
            RedmineProvider = redmineProvider;
            this.Manager = redmineProvider.Manager;
        }

        public RedmineManager Manager { get; set; }
        int? _RedmineVersionId;
        int? RedmineVersionId
        {
            get
            {
                if (_RedmineVersionId.HasValue)
                {
                    return _RedmineVersionId.Value;
                }
                var parameters = new NameValueCollection { };
                parameters.Add(RedmineKeys.PROJECT_ID, RedmineProvider._Project.Id.ToString());
                var request = new Redmine.Net.Api.Net.RequestOptions();
                request.QueryString = parameters;
                var versions = Manager.Get<Redmine.Net.Api.Types.Version>(request);
                return _RedmineVersionId = versions.FirstOrDefault(v => v.Name == Version)?.Id;
            }
        }
        public async Task UpdateTasksAsync()
        {
            if (!RedmineVersionId.HasValue)
            {
                return;
            }
            var isP = new NameValueCollection { };
            isP.Add(RedmineKeys.PROJECT_ID, RedmineProvider._Project.Id.ToString());
            isP.Add(RedmineKeys.TRACKER_ID, DemandId.ToString());
            isP.Add(RedmineKeys.FIXED_VERSION_ID, RedmineVersionId.ToString());
            isP.Add(RedmineKeys.STATUS_ID, RedmineKeys.ALL);
            var now = DateTime.UtcNow;
            //var testDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            //isP.Add(RedmineKeys.UPDATED_ON, $">={testDate}");
            var request = new Redmine.Net.Api.Net.RequestOptions();
            request.QueryString = isP;
            var issues = await Manager.GetAsync<Issue>(request);
            if (issues != null)
            {
                Issue issue = null;
                foreach (MSProject.Task item in MainTask.OutlineChildren)
                {
                    if ((issue = issues.FirstOrDefault(i => i.Id == item.Number1)) != null)
                    {
                        //Update
                        ThisAddIn.redmineProvider.UpdateMSTask(issue, item);
                        issues.Remove(issue);
                    }
                    else if (item.Number1 != 0)
                    {
                        //Delete
                        item.Delete();
                    }
                }
                foreach (var item in issues)
                {
                    Append(MainTask, item);
                }
            }
        }

        int Append(MSProject.Task parentTask, Issue issue)
        {
            //if (addResult.ContainsKey(issue.Id) && addResult[issue.Id])
            //{
            //    return;
            //}

            MSProject.Task newTask = Pj.Tasks.Add
            ($"[{issue.Tracker.Name}] {issue.Subject}", parentTask?.ID + 1 ?? System.Type.Missing);
            ThisAddIn.redmineProvider.UpdateMSTask(issue, newTask);
            //newTask.Manual = false;
            //newTask.Start = issue.StartDate;
            //if (issue.DueDate != null)
            //{
            //    newTask.Finish = issue.DueDate;
            //}
            //else
            //{
            //    newTask.Finish = issue.StartDate.Value.AddDays(3);
            //}
            ////if (issue.EstimatedHours == null || issue.EstimatedHours < 8)
            ////{
            ////    newTask.Duration = 8 * 60;
            ////}
            ////else
            ////{
            ////    newTask.Duration = issue.EstimatedHours * 60;
            ////}
            //newTask.ResourceNames = issue.AssignedTo?.Name ?? "";
            //newTask.OutlineLevel = 3;
            //if (issue.Status.Id == 5 || issue.Status.Id == 6)
            //{
            //    newTask.PercentComplete = 100;
            //}
            //else
            //{
            //    var precent = issue.DoneRatio ?? 0;

            //    newTask.PercentComplete = precent == 100 ? 99 : precent;
            //}
            //newTask.Text1 = issue.Status.Name;
            //newTask.Text2 = issue.Author.Name;
            ////newTask.OutlineLevel = (short)(parentTask.OutlineLevel + 1);
            //newTask.Number1 = issue.Id;
            return newTask.ID;
            //if (issue.Children == null)
            //{
            //    return;
            //}
            //foreach (var item in issue.Children)
            //{
            //    var subIssue = issues.FirstOrDefault(i => i.Id == item.Id);
            //    Application_NewProject(newTask, subIssue, issues);
            //}
        }
    }
}
