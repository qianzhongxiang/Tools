using NPOI.XWPF.UserModel;
using Redmine.Net.Api;
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
        public Project Project { get; set; }
        public List<Redmine.Net.Api.Types.Version> Versions { get; set; }
        public List<Redmine.Net.Api.Types.Project> Projects { get; set; }
        public const int DesignId = 7;
        public const int FunctionId = 2;
        public const int CodeMergeId = 6;
        public const int IssueId = 1;
        public const string Host = "http://www.vppms.tech";
        public RedmineProvider(string proj, string version)
        {
            var apiKey = "4110d8c8eb1729ad3a9ec13b6f505155c242efa2";
            Manager = new RedmineManager(Host, apiKey);
            Projects = Manager.GetObjects<Project>();

            Project = Projects.Find(p => p.Name.Contains(proj));

            var parameters = new NameValueCollection { };
            parameters.Add(RedmineKeys.PROJECT_ID, Project.Id.ToString());
            var versions = Manager.GetObjects<Redmine.Net.Api.Types.Version>(parameters);
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
        public void GenerateJournal(IEnumerable<Project> projects)
        {
            var parameters = new NameValueCollection { };
            var pjs = string.Join("|", projects.Select(p => p.Id));
            parameters.Add(RedmineKeys.PROJECTS, pjs);
            var start = DateTime.Now.Subtract(TimeSpan.FromDays(7));
            var startTime = start.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            parameters.Add(RedmineKeys.SPENT_ON, $">={startTime}");
            parameters.Add("issue.tracker_id", $"{FunctionId}|{DesignId}");

            var times = Manager.GetObjects<TimeEntry>(parameters);
            var issues = times.Select(t => t.Issue).GroupBy(t => t.Id);
            List<Issue> functions = new List<Issue>();
            var issueArg = new NameValueCollection { };
            foreach (var item in issues)
            {
                var issue = Manager.GetObject<Issue>(item.Key.ToString(), issueArg);
                functions.Add(issue);
            }

            parameters = new NameValueCollection { };
            parameters.Add(RedmineKeys.PROJECTS, pjs);
            parameters.Add(RedmineKeys.CREATED_ON, $">={startTime}");
            parameters.Add(RedmineKeys.TRACKER_ID, $"{IssueId}");
            parameters.Add(RedmineKeys.STATUS_ID, RedmineKeys.ALL);
            var issuesList = Manager.GetObjects<Issue>(parameters);

            using (XWPFDocument doc = new XWPFDocument())
            {

                XWPFParagraph p1 = doc.CreateParagraph();
                p1.Alignment = ParagraphAlignment.LEFT;
                XWPFRun r1 = p1.CreateRun();
                r1.SetText("功能");
                r1.FontSize = 20;

                foreach (var item in functions)
                {
                    p1 = doc.CreateParagraph();
                    p1.Alignment = ParagraphAlignment.LEFT;
                    XWPFHyperlinkRun hyperlinkrun = CreateHyperlinkRun(p1, $"{Host}/issues/{item.Id}");
                    hyperlinkrun.SetText($"{item.Tracker.Name} #{item.Id}");
                    hyperlinkrun.SetColor("0000FF");
                    hyperlinkrun.Underline = UnderlinePatterns.Single;
                    r1 = p1.CreateRun();
                    r1.SetText($" {item.Subject}");
                    r1.FontSize = 13;

                    p1 = doc.CreateParagraph();
                    p1.IndentationLeft = 350;
                    p1.Alignment = ParagraphAlignment.LEFT;
                    r1 = p1.CreateRun();
                    r1.FontSize = 9;
                    r1.SetText($" {item.Description}");
                }

                XWPFParagraph p2 = doc.CreateParagraph();
                p2.Alignment = ParagraphAlignment.LEFT;

                XWPFRun r2 = p2.CreateRun();
                r2.SetText("问题");
                r2.FontSize = 20;
                foreach (var item in issuesList)
                {
                    p1 = doc.CreateParagraph();
                    p1.Alignment = ParagraphAlignment.LEFT;
                    XWPFHyperlinkRun hyperlinkrun = CreateHyperlinkRun(p1, $"{Host}/issues/{item.Id}");
                    hyperlinkrun.SetText($"{item.Tracker.Name} #{item.Id}");
                    hyperlinkrun.SetColor("0000FF");
                    hyperlinkrun.Underline = UnderlinePatterns.Single;
                    r1 = p1.CreateRun();
                    r1.SetText($" {item.Subject}");
                    r1.FontSize = 11;
                }
                p1 = doc.CreateParagraph();
                p1.Alignment = ParagraphAlignment.LEFT;
                r1 = p1.CreateRun();
                r1.SetText("流片");
                r1.FontSize = 20;

                p1 = doc.CreateParagraph();
                p1.Alignment = ParagraphAlignment.LEFT;
                r1 = p1.CreateRun();
                r1.SetText("文档");
                r1.FontSize = 20;
                using (FileStream sw = System.IO.File.Create($"D:\\{start.ToString("yyyyMMdd")}-{DateTime.Now.ToString("yyyyMMdd")}.docx"))
                {
                    doc.Write(sw);
                }
            }
        }
        public void Update(int issueId, MSProject.Task tsk)
        {
            var parameters = new NameValueCollection { };
            var issue = Manager.GetObject<Issue>(issueId.ToString(), parameters);
            Update(issue, tsk);
        }

        public void Update(Issue issue, MSProject.Task task)
        {


            task.Manual = false;
            task.Start = issue.StartDate;
            if (issue.DueDate != null)
            {
                var a = issue.DueDate.Value.AddHours(18);
                task.Finish = a;
            }
            else
            {
                task.Finish = issue.StartDate.Value.AddDays(3);
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
            task.OutlineLevel = 3;
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
            task.Number1 = issue.Id;


        }
        public void ChangeTask(MSProject.Task tsk)
        {
            int id = -1;
            try
            {
                
                id = (int)tsk.Number1;
                System.Diagnostics.Debug.WriteLine($"{id},{tsk.Start},{tsk.Finish}");
                var parameters = new NameValueCollection { };
                var issue = Manager.GetObject<Issue>(id.ToString(), parameters);
                issue.StartDate = tsk.Start;
                issue.DueDate = tsk.Finish;
                Manager.UpdateObject<Issue>(id.ToString(), issue);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (id != -1)
                {
                    ThisAddIn.Fresh = true;
                    try
                    {
                        Update(id, tsk);
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
            }
        }
    }
}
