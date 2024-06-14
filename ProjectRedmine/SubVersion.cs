using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MSProject = Microsoft.Office.Interop.MSProject;
using Office = Microsoft.Office.Core;

namespace ProjectRedmine
{
    public class SubVersion
    {
        public MSProject.Task MainTask { get; set; }
        public MSProject.Task DesignTask { get; set; }
        public MSProject.Task DevelopTask { get; set; }
        public MSProject.Task TestTask { get; set; }
        public MSProject.Project Pj { get; }
        public MSProjectWrapper MSProjectWrapper { get; }
        public Redmine.Net.Api.Types.Version Version { get; }
        public RedmineProvider RedmineProvider { get; }
        public const int SoftDemandId = 7;
        public const int FunctionId = 2;
        public const int CodeMergeId = 6;
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

        public void AddSubTasks()
        {
            var isP = new NameValueCollection { { RedmineKeys.INCLUDE, RedmineKeys.CHILDREN }, { RedmineKeys.INCLUDE, RedmineKeys.PARENT } };
            isP.Add(RedmineKeys.PROJECT_ID, RedmineProvider.Project.Id.ToString());
            isP.Add(RedmineKeys.TRACKER_IDS, $"{SoftDemandId}|{FunctionId}|{CodeMergeId}");
            isP.Add(RedmineKeys.FIXED_VERSION_ID, Version.Id.ToString());
            isP.Add(RedmineKeys.STATUS_ID, RedmineKeys.ALL);
            var issues = Manager.GetObjects<Issue>(isP);
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

                    if (issue.Tracker.Id == SoftDemandId)
                    {
                        Application_NewProject(DesignTask, issue);
                    }
                    if (issue.Tracker.Id == FunctionId)
                    {
                        Application_NewProject(DevelopTask, issue);
                    }
                    if (issue.Tracker.Id == CodeMergeId)
                    {
                        Application_NewProject(TestTask, issue);
                    }
                }
            }
           (Pj.Comments as string).Split(';');
        }
        public void UpdateTasks()
        {
            var isP = new NameValueCollection { { RedmineKeys.INCLUDE, RedmineKeys.CHILDREN }, { RedmineKeys.INCLUDE, RedmineKeys.PARENT } };
            isP.Add(RedmineKeys.PROJECT_ID, RedmineProvider.Project.Id.ToString());
            isP.Add(RedmineKeys.TRACKER_IDS, $"{SoftDemandId}|{FunctionId}|{CodeMergeId}");
            isP.Add(RedmineKeys.FIXED_VERSION_ID, Version.Id.ToString());
            isP.Add(RedmineKeys.STATUS_ID, RedmineKeys.ALL);
            //isP.Add(RedmineKeys.UPDATED_ON, $">={MSProjectWrapper.UpdateTimeStr}");
            var testDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            isP.Add(RedmineKeys.UPDATED_ON, $">={testDate}");
            var issues = Manager.GetObjects<Issue>(isP);
            if (issues != null)
            {
                foreach (var issue in issues)
                {
                    var version = issue.FixedVersion.Name;
                    var stepTaskName = "";
                    switch (issue.Tracker.Id)
                    {
                        case SoftDemandId:
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
                    MSProject.Task parentTask=null;
                    foreach (Microsoft.Office.Interop.MSProject.Task item in Pj.Tasks)
                    {
                        if (item.Name == version)
                        {
                            versionOk = true;
                        }
                        if (versionOk&& item.Name==stepTaskName)
                        {
                            parentTask = item;
                        }
                        if (item.Number1 == issue.Id)
                        {
                            item.Delete();
                        }
                    }
                    Application_NewProject(parentTask,issue);

                }
            }
        }
        int Application_NewProject(MSProject.Task parentTask, Issue issue)
        {
            //if (addResult.ContainsKey(issue.Id) && addResult[issue.Id])
            //{
            //    return;
            //}

            MSProject.Task newTask = Pj.Tasks.Add
            ($"[{issue.Tracker.Name}] {issue.Subject}", parentTask?.ID + 1 ?? System.Type.Missing);
            newTask.Start = issue.StartDate;
            newTask.Duration = (issue.EstimatedHours ?? 8) * 60;
            newTask.ResourceNames = issue.AssignedTo?.Name ?? "";
            newTask.OutlineLevel = 3;
            if (issue.Status.Id == 5)
            {
                newTask.PercentComplete = 100;
            }
            else
            {
                newTask.PercentComplete = issue.DoneRatio ?? 0;
            }
            newTask.Text1 = issue.Status.Name;
            newTask.Text2 = issue.Author.Name;
            //newTask.OutlineLevel = (short)(parentTask.OutlineLevel + 1);
            newTask.Number1 = issue.Id;
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
