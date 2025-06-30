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

namespace ProjectRedmine
{
    public class RedmineProvider
    {
        public RedmineManager Manager { get; set; }
        public List<Redmine.Net.Api.Types.Version> Versions { get; set; }
        public List<Redmine.Net.Api.Types.Project> Projects { get; set; }
        public const int DesignId = 7;
        public const int FunctionId = 2;
        public const int CodeMergeId = 6;
        public const int IssueId = 1;
        public const string Host = "http://www.vppms.tech";
        public RedmineProvider()
        {
            var apiKey = "4110d8c8eb1729ad3a9ec13b6f505155c242efa2";
            //var builder = new RedmineManagerOptionsBuilder().WithHost(Host).WithApiKeyAuthentication(apiKey);
            //Manager = new RedmineManager(builder);
            Manager = new RedmineManager(Host, apiKey);
            Projects = Manager.GetObjects<Project>();
            //Projects = Manager.GetObjects<Project>();
        }
        static XWPFHyperlinkRun CreateHyperlinkRun(XWPFParagraph paragraph, String uri)
        {
            String rId = paragraph.Document.GetPackagePart().AddExternalRelationship(
              uri,
              XWPFRelation.HYPERLINK.Relation
             ).Id;

            return paragraph.CreateHyperlinkRun(rId);
        }
        public void GenerateJournal(IEnumerable<Project> projects, Project lpP, DateTime start)
        {
            var parameters = new NameValueCollection { };
            var startTime = start.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            var projectIds = projects.Select(p => p.Id);
            parameters.Add(RedmineKeys.SPENT_ON, $">={startTime}");
            //parameters.Add("issue.tracker_id", $"{FunctionId}|{DesignId}");
            var tlist = Manager.GetObjects<TimeEntry>(parameters);
            var times = new List<TimeEntry>();
            if (tlist != null)
            {
                times = tlist.Where(t => projectIds.Contains(t.Project.Id)).ToList();
            }

            List<Issue> functions = new List<Issue>();
            var issuesList = new List<Issue>();
            if (times != null)
            {
                var issues = times.Select(t => t.Issue).GroupBy(t => t.Id);
                var issueArg = new NameValueCollection { };
                foreach (var item in issues)
                {
                    var issue = Manager.GetObject<Issue>(item.Key.ToString(), issueArg);
                    if (issue.Tracker.Id == FunctionId || issue.Tracker.Id == DesignId)
                    {
                        functions.Add(issue);
                    }
                    else if (issue.Tracker.Id == IssueId)
                    {
                        issuesList.Add(issue);
                    }
                }
            }

            foreach (var proj in projects)
            {
                parameters = new NameValueCollection { { RedmineKeys.PROJECT_ID, proj.Id.ToString() } };
                parameters.Add(RedmineKeys.CREATED_ON, $">={startTime}");
                parameters.Add(RedmineKeys.TRACKER_ID, $"{IssueId}");
                parameters.Add(RedmineKeys.STATUS_ID, RedmineKeys.ALL);
                var list = Manager.GetObjects<Issue>(parameters);
                if (list != null)
                {
                    issuesList.AddRange(list);
                }
            }
            issuesList = issuesList.DistinctBy(i => i.Id).ToList();

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
                    r1.SetText($" [{GetDoneRatio(item)}%] {item.Subject}");
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
                    GetDoneRatio(item);
                    r1.SetText($" [{GetDoneRatio(item)}%] {item.Subject}");
                    r1.FontSize = 11;
                }
                p1 = doc.CreateParagraph();
                p1.Alignment = ParagraphAlignment.LEFT;
                r1 = p1.CreateRun();
                r1.SetText("流片");
                r1.FontSize = 20;
                parameters = new NameValueCollection { { RedmineKeys.PROJECT_ID, lpP.Id.ToString() } };
                parameters.Add(RedmineKeys.CREATED_ON, $">={startTime}");
                //parameters.Add(RedmineKeys.TRACKER_ID, $"{IssueId}");
                parameters.Add(RedmineKeys.STATUS_ID, RedmineKeys.ALL);
                var list = Manager.GetObjects<Issue>(parameters);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        p1 = doc.CreateParagraph();
                        p1.Alignment = ParagraphAlignment.LEFT;
                        XWPFHyperlinkRun hyperlinkrun = CreateHyperlinkRun(p1, $"{Host}/issues/{item.Id}");
                        hyperlinkrun.SetText($"{item.Tracker.Name} #{item.Id}");
                        hyperlinkrun.SetColor("0000FF");
                        hyperlinkrun.Underline = UnderlinePatterns.Single;
                        r1 = p1.CreateRun();
                        r1.SetText($" [{GetDoneRatio(item)}%] {item.Subject}");
                        r1.FontSize = 11;
                    }
                }

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

        private static double GetDoneRatio(Issue item)
        {
            if (item.Status.Name == "已关闭" || item.Status.Name.ToLower() == "closed")
            {
                return 100;
            }else
            {
                return item.DoneRatio ?? 0;
            }
        }
    }
}
