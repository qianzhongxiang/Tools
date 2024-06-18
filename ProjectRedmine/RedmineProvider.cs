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
        public Project Project { get; set; }
        public List<Redmine.Net.Api.Types.Version> Versions { get; set; }

        public RedmineProvider(string proj, string version)
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

        public static void GenerateJournal()
        {
            using (XWPFDocument doc = new XWPFDocument())
            {
                XWPFParagraph p1 = doc.CreateParagraph();
                p1.Alignment = ParagraphAlignment.LEFT;
                XWPFRun r1 = p1.CreateRun();
                r1.SetText("功能");
                r1.IsBold = true;
                r1.FontFamily = "Courier";
                r1.Underline = UnderlinePatterns.DotDotDash;
                r1.FontSize = 20;
                r1.TextPosition = 100;
                XWPFRun r11 = p1.CreateRun();
                r11.FontFamily = "Courier";
                r11.SetText("The quick brown fox");


                XWPFParagraph p2 = doc.CreateParagraph();
                p2.Alignment = ParagraphAlignment.LEFT;

                XWPFRun r2 = p2.CreateRun();
                r2.SetText("问题");
                r2.FontSize = 20;

                p1.VerticalAlignment = TextAlignment.TOP;
                using (FileStream sw = System.IO.File.Create("D:\\BlankDocumentUsingNPOI.docx"))
                {
                    doc.Write(sw);
                }
            }
        }


    }
}
