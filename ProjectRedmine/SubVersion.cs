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
        public Redmine.Net.Api.Types.Version Version { get; }
        public RedmineProvider RedmineProvider { get; }
        public const int DesignId = 7;
        public const int FunctionId = 2;
        public const int CodeMergeId = 6;
        public const int DemandId = 4;
        public const int IssueId = 1;

        public SubVersion(MSProjectWrapper mSProjectWrapper, Redmine.Net.Api.Types.Version version, RedmineProvider redmineProvider)
        {
            MSProjectWrapper = mSProjectWrapper;
            Pj = MSProjectWrapper.Pj;
            Version = version;
            RedmineProvider = redmineProvider;
            this.Manager = redmineProvider.Manager;
        }

        public void Create()
        {
            MainTask = Pj.Tasks.Add
                       (Version.Name, System.Type.Missing);
            MainTask.OutlineLevel = 1;

         //   DemandTask = Pj.Tasks.Add
         //("demand", System.Type.Missing);
         //   DemandTask.OutlineLevel = 2;

            DesignTask = Pj.Tasks.Add
           ("design", System.Type.Missing);
            DesignTask.OutlineLevel = 2;

            DevelopTask = Pj.Tasks.Add
           ("develop", System.Type.Missing);
            DevelopTask.OutlineLevel = 2;

            TestTask = Pj.Tasks.Add
           ("test", System.Type.Missing);
            TestTask.OutlineLevel = 2;
        }
        public RedmineManager Manager { get; set; }

        public async Task AddSubTasks()
        {
            var isP = new NameValueCollection {
                { RedmineKeys.INCLUDE, RedmineKeys.CHILDREN },
                { RedmineKeys.INCLUDE, RedmineKeys.PARENT },
                { RedmineKeys.INCLUDE, RedmineKeys.TIME_ENTRY }
            };
            isP.Add(RedmineKeys.PROJECT_ID, RedmineProvider.Project.Id.ToString());
            //isP.Add(RedmineKeys.TRACKER_IDS, $"{DesignId}|{DemandId}|{FunctionId}|{CodeMergeId}");
            isP.Add(RedmineKeys.TRACKER_IDS, $"{DesignId}|{FunctionId}|{CodeMergeId}|{IssueId}");
            isP.Add(RedmineKeys.FIXED_VERSION_ID, Version.Id.ToString());
            isP.Add(RedmineKeys.STATUS_ID, RedmineKeys.OPEN_ISSUES);
            var request = new Redmine.Net.Api.Net.RequestOptions();
            request.QueryString = isP;
            var issues = await Manager.GetAsync<Issue>(request);
            if (issues != null)
            {
                foreach (var issue in issues)
                {
                    //if (issue.ParentIssue != null)
                    //{
                    //    var pI = issues.FirstOrDefault(i => i.Id == issue.ParentIssue.Id);
                    //    if (pI != null)
                    //    {
                    //        continue;
                    //    }
                    //}
                    //if (issue.Tracker.Id == DemandId)
                    //{
                    //    Append(DemandTask, issue);
                    //}
                    if (issue.Tracker.Id == DesignId)
                    {
                        Append(DesignTask, issue);
                    }
                    if (issue.Tracker.Id == FunctionId)
                    {
                        Append(DevelopTask, issue);
                    }
                    if (issue.Tracker.Id == CodeMergeId|| issue.Tracker.Id ==  IssueId)
                    {
                        Append(TestTask, issue);
                    }
                }
            }
        }
        public void UpdateTasks()
        {
            var isP = new NameValueCollection { { RedmineKeys.INCLUDE, RedmineKeys.CHILDREN }, { RedmineKeys.INCLUDE, RedmineKeys.PARENT } };
            isP.Add(RedmineKeys.PROJECT_ID, RedmineProvider.Project.Id.ToString());
            isP.Add(RedmineKeys.TRACKER_IDS, $"{DesignId}|{DemandId}|{FunctionId}|{CodeMergeId}");
            isP.Add(RedmineKeys.FIXED_VERSION_ID, Version.Id.ToString());
            isP.Add(RedmineKeys.STATUS_ID, RedmineKeys.ALL);
            var now = DateTime.UtcNow;
            isP.Add(RedmineKeys.UPDATED_ON, $">={MSProjectWrapper.UpdateTimeStr}");
            //var testDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            //isP.Add(RedmineKeys.UPDATED_ON, $">={testDate}");
            var issues = Manager.GetObjects<Issue>(isP);
            if (issues != null)
            {
                foreach (var issue in issues)
                {
                    var version = issue.FixedVersion.Name;
                    var stepTaskName = "";
                    switch (issue.Tracker.Id)
                    {
                        case DemandId:
                            stepTaskName = "demand";
                            break;
                        case DesignId:
                            stepTaskName = "design";
                            break;
                        case FunctionId:
                            stepTaskName = "develop";
                            break;
                        case CodeMergeId:
                            stepTaskName = "test";
                            break;
                        default:
                            break;
                    }
                    if (string.IsNullOrWhiteSpace(stepTaskName))
                    {
                        continue;
                    }
                    bool versionOk = false;
                    MSProject.Task parentTask = null;
                    foreach (Microsoft.Office.Interop.MSProject.Task item in Pj.Tasks)
                    {
                        if (item.Name == version)
                        {
                            versionOk = true;
                        }
                        if (versionOk && item.Name == stepTaskName)
                        {
                            parentTask = item;
                        }
                        if (item.Number1 == issue.Id)
                        {
                            item.Delete();
                            if (parentTask != null)
                            {
                                break;
                            }
                        }
                    }
                    Append(parentTask, issue);

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
            ThisAddIn.redmineProvider.Update(issue,newTask);
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
