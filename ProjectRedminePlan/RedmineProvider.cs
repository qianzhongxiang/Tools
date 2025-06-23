using NPOI.XWPF.UserModel;
using Redmine.Net.Api;
using Redmine.Net.Api.Extensions;
using Redmine.Net.Api.Net;
using Redmine.Net.Api.Types;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSProject = Microsoft.Office.Interop.MSProject;

namespace ProjectRedmine
{
    public class RedmineProvider
    {
        public RedmineManager Manager { get; set; }
        public Project _Project { get; set; }
        public List<Redmine.Net.Api.Types.Version> Versions { get; set; }
        public List<Redmine.Net.Api.Types.Project> Projects { get; set; }
        public const int DesignId = 7;
        public const int FunctionId = 2;
        public const int CodeMergeId = 6;
        public const int IssueId = 1;
        public const int DemandId = 4;
        public const string Host = "http://www.vppms.tech";
        public RedmineProvider(string proj, string version)
        {
            var apiKey = "4110d8c8eb1729ad3a9ec13b6f505155c242efa2";

            var opt = new RedmineManagerOptionsBuilder().WithApiKeyAuthentication(apiKey).WithHost(Host);
            Manager = new RedmineManager(opt);
            Projects = Manager.Get<Project>();
            _Project = Projects.Find(p => p.Name.Contains(proj));

            var parameters = new NameValueCollection
            {
                { RedmineKeys.PROJECT_ID, _Project.Id.ToString() }
            };
            var request = new RequestOptions { QueryString = parameters };
            var versions = Manager.Get<Redmine.Net.Api.Types.Version>(request);
            Versions = versions.Where(v => v.Name.StartsWith(version)).ToList();
            Versions.Sort(new VersionCompare());
        }
        static XWPFHyperlinkRun CreateHyperlinkRun(XWPFParagraph paragraph, String uri)
        {
            String rId = paragraph.Document.GetPackagePart().AddExternalRelationship(
              uri,
              XWPFRelation.HYPERLINK.Relation
             ).Id;

            return paragraph.CreateHyperlinkRun(rId);
        }
        public Issue GetIssue(int issueId)
        {
            return Manager.Get<Issue>(issueId.ToString());
        }
        public void UpdateMSTask(int issueId, MSProject.Task tsk)
        {
            UpdateMSTask(GetIssue(issueId), tsk);
        }

        public void UpdateMSTask(Issue issue, MSProject.Task task)
        {
            ThisAddIn.Fresh = true;
            try
            {
                task.Manual = true;
                task.Start = issue.StartDate.Value.AddHours(8);
                if (issue.DueDate != null)
                {
                    //var a = issue.DueDate.Value.AddHours(18);
                    task.Finish = issue.DueDate.Value.AddHours(17);
                }
                else
                {
                    task.Finish = issue.StartDate.Value.AddDays(3).AddHours(17);
                }
                //if (issue.EstimatedHours == null || issue.EstimatedHours < 8)
                //{
                //    newTask.Duration = 8 * 60;
                //}
                //else
                //{
                //    newTask.Duration = issue.EstimatedHours * 60;
                //}
                task.ResourceNames = issue.AssignedTo?.Name ?? "";
                task.OutlineLevel = 2;
                if (issue.Status.Id == 5 || issue.Status.Id == 6)
                {
                    task.PercentComplete = 100;
                }
                else
                {
                    var precent = issue.DoneRatio ?? 0;

                    task.PercentComplete = precent == 100 ? 99 : precent;
                }
                task.Text1 = issue.Status.Name;
                task.Text2 = issue.Author.Name;
                //newTask.OutlineLevel = (short)(parentTask.OutlineLevel + 1);
                if (task.Number1 != issue.Id)
                {
                    if (task.Number1 == 0)
                    {
                        task.Number1 = issue.Id;
                    }
                    else
                    {
                        throw new Exception("无法更新issue到Id不同的MSTask上");
                    }
                }
                task.Number2 = 0;
                task.Notes = issue.Description;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                ThisAddIn.Fresh = false;
            }
        }
        public void UpdateIssue(MSProject.Task tsk)
        {
            var id = (int)tsk.Number1;
            if (id < 0.5)
            {
                return;
            }
            var redV = RedmineVersion(tsk.OutlineParent.Name);
            System.Diagnostics.Debug.WriteLine($"{id},{tsk.Start},{tsk.Finish}");
            var issue = Manager.Get<Issue>(id.ToString());
            issue.Subject = tsk.Name.Replace("[需求]", "");
            issue.Description = tsk.Notes;
            issue.FixedVersion = IdentifiableName.Create<IdentifiableName>(redV.Id);
            issue.StartDate = tsk.Start;
            issue.DueDate = tsk.Finish;
            Manager.Update<Issue>(id.ToString(), issue);
            ThisAddIn.Fresh = true;
            try
            {
                UpdateMSTask(id, tsk);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ThisAddIn.Fresh = false;
            }


        }



        Redmine.Net.Api.Types.Version RedmineVersion(string ver)
        {
            var parameters = new NameValueCollection { };
            parameters.Add(RedmineKeys.PROJECT_ID, _Project.Id.ToString());
            var request = new Redmine.Net.Api.Net.RequestOptions();
            request.QueryString = parameters;
            var versions = Manager.Get<Redmine.Net.Api.Types.Version>(request);
            return versions.FirstOrDefault(v => v.Name == ver);
        }
        public Issue PublishTask(MSProject.Task tsk)
        {
            var issue = new Redmine.Net.Api.Types.Issue();
            var redV = RedmineVersion(tsk.OutlineParent.Name);
            issue.Subject = tsk.Name;
            issue.Description = tsk.Notes;
            issue.Project = _Project;
            issue.StartDate = tsk.Start is null ? DateTime.Now : tsk.Start;
            issue.DueDate = tsk.Finish;
            issue.AssignedTo = IdentifiableName.Create<IdentifiableName>(1);
            issue.FixedVersion = IdentifiableName.Create<IdentifiableName>(redV.Id);
            issue.Tracker = IdentifiableName.Create<IdentifiableName>(DemandId);
            return Manager.Create<Issue>(issue);
        }

        public Issue StartTask(string subject, string description, string version, DateTime start, DateTime end, bool fun, int parentId, int assignedToID)
        {
            var issue = new Redmine.Net.Api.Types.Issue();
            var redV = RedmineVersion(version);
            issue.Subject = subject;
            issue.Description = description;
            issue.Project = _Project;
            issue.ParentIssue = IdentifiableName.Create<IdentifiableName>(parentId);
            issue.StartDate = start;
            issue.DueDate = end;
            issue.AssignedTo = IdentifiableName.Create<IdentifiableName>(assignedToID);
            issue.FixedVersion = IdentifiableName.Create<IdentifiableName>(redV.Id);
            issue.Tracker = IdentifiableName.Create<IdentifiableName>(fun ? FunctionId : DesignId);
            issue.EstimatedHours = (float)end.Subtract(start).TotalDays * 8;
            return Manager.Create<Issue>(issue);
        }

        public IEnumerable<User> GetALLUsers()
        {
            var issue = new Redmine.Net.Api.Types.Issue();
            var parameters = new NameValueCollection
            {
                { RedmineKeys.PROJECT_ID, _Project.Id.ToString() },
            };
            var request = new Redmine.Net.Api.Net.RequestOptions();
            request.QueryString = parameters;
            return ToIEnumerable(Manager.Get<User>(request));
        }
        public IEnumerable<ProjectMembership> GetMemberships()
        {
            var ms = Manager.GetProjectMemberships(_Project.Id.ToString());
            return ms.Items;
        }
        public IEnumerable<Issue> GetSubIssues(MSProject.Task tsk)
        {
            var issue = new Redmine.Net.Api.Types.Issue();
            var redV = RedmineVersion(tsk.OutlineParent.Name);
            var parameters = new NameValueCollection
            {
                { RedmineKeys.PROJECT_ID, _Project.Id.ToString() },
                { RedmineKeys.PARENT_ID,((int)tsk.Number1).ToString()},
                { RedmineKeys.STATUS_ID, RedmineKeys.ALL }
            };
            var request = new Redmine.Net.Api.Net.RequestOptions();
            request.QueryString = parameters;
            var issues = Manager.Get<Issue>(request);
            return ToIEnumerable(issues);
        }

        private static IEnumerable<T> ToIEnumerable<T>(List<T> issues) where T : new()
        {
            if (issues is null)
            {
                yield break;
            }
            foreach (var item in issues)
            {
                yield return item;
            }
        }
    }
}
