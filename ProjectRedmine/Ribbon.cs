using Microsoft.Office.Tools.Ribbon;
using Redmine.Net.Api;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using MSProject = Microsoft.Office.Interop.MSProject;
using Office = Microsoft.Office.Core;

namespace ProjectRedmine
{
    public partial class Ribbon
    {
        public MSProject.Application Application => Globals.ThisAddIn.Application;
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }
        RedmineProvider redmineProvider;
        MSProjectWrapper mSProjectWrapper;
        private void btnRefresh_Click(object sender, RibbonControlEventArgs e)
        {
            ThisAddIn.Fresh = true;
            try
            {
                if (this.Application.ActiveProject.Comments is null)
                {
                    System.Windows.Forms.MessageBox.Show("please set version first");
                    return;
                }
                //redmineProvider = new RedmineProvider(this.Application.ActiveProject.Comments as string);
                mSProjectWrapper = MSProjectWrapper.CreateOrNewWrapper(this.Application.ActiveProject);
                redmineProvider = new RedmineProvider(mSProjectWrapper.RedmineProj, mSProjectWrapper.Version);

                var tasks = this.Application.ActiveProject.Tasks;
                var c = this.Application.ActiveProject.Tasks.Count;
                for (; c > 0; c--)
                {
                    tasks[c].Delete();
                }
                var now = DateTime.UtcNow;
                foreach (var item in redmineProvider.Versions)
                {
                    if (item.Status == Redmine.Net.Api.Types.VersionStatus.Open)
                    {
                        var subv = new SubVersion(mSProjectWrapper, item, redmineProvider);
                        subv.Create();
                        subv.AddSubTasks();
                    }
                }
                mSProjectWrapper.UpdateTime = now;
                mSProjectWrapper.SaveParameters();
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

        private void btnUpdate_Click(object sender, RibbonControlEventArgs e)
        {
            var tasks = this.Application.ActiveSelection.Tasks;

            foreach (MSProject.Task item in tasks)
            {
                if (item.Number1 != 0)
                {

                }

                string target = $"{RedmineProvider.Host}/issues/{item.Number1}";
                System.Diagnostics.Process.Start(target);
            }
        }

        private void btnConfig_Click(object sender, RibbonControlEventArgs e)
        {
            new SetDialog(MSProjectWrapper.CreateOrNewWrapper(this.Application.ActiveProject)).ShowDialog();
            this.Application.CustomFieldRename(MSProject.PjCustomField.pjCustomTaskText1, "RedmineStatus");
            this.Application.CustomFieldRename(MSProject.PjCustomField.pjCustomTaskText2, "Creator");
            this.Application.CustomFieldRename(MSProject.PjCustomField.pjCustomTaskNumber1, "IssueID");
        }

        private void btnJournal_Click(object sender, RibbonControlEventArgs e)
        {
            mSProjectWrapper = MSProjectWrapper.CreateOrNewWrapper(this.Application.ActiveProject);
            redmineProvider = new RedmineProvider(mSProjectWrapper.RedmineProj, mSProjectWrapper.Version);
            new Report(redmineProvider).ShowDialog();
        }

        private void btnTest_Click(object sender, RibbonControlEventArgs e)
        {
            if (this.Application.ActiveSelection.Tasks.Count == 0)
            {
                return;
            }
            foreach (MSProject.Task t in this.Application.ActiveSelection.Tasks)
            {
                mSProjectWrapper = MSProjectWrapper.CreateOrNewWrapper(this.Application.ActiveProject);
                redmineProvider = new RedmineProvider(mSProjectWrapper.RedmineProj, mSProjectWrapper.Version);
                redmineProvider.ChangeTask(t);
            }
        }

        private void btnURL_Click(object sender, RibbonControlEventArgs e)
        {
            var tasks = this.Application.ActiveSelection.Tasks;
            var resources = this.Application.ActiveSelection.Resources;
            var fieldIds = this.Application.ActiveSelection.FieldIDList;
            var parent = this.Application.ActiveSelection.Parent;
            var c = this.Application.ActiveCell;
            if (tasks != null)
            {
                foreach (MSProject.Task item in tasks)
                {
                    string target = $"{RedmineProvider.Host}/issues/{item.Number1}";
                    System.Diagnostics.Process.Start(target);
                }
            }

            if (c != null && c.Assignment != null)
            {
                var item = c.Assignment.Task;
                string target = $"{RedmineProvider.Host}/issues/{item.Number1}";
                System.Diagnostics.Process.Start(target);
            }

            //if (resources != null)
            //{
            //    foreach (MSProject.Resource item in resources)
            //    {
            //        var id = item.ID;
            //        var name = item.Name;
            //        var code = item.Code;
            //        var assigns = item.Assignments;
            //        foreach (MSProject.Assignment assignment in assigns)
            //        {
            //            var p = assignment.TeamStatusPending;
            //            System.Diagnostics.Debug.WriteLine($"{assignment.Task.Name} {p} {assignment.UniqueID}");
            //        }
            //        string target = $"{RedmineProvider.Host}/issues/{item.Number1}";
            //        System.Diagnostics.Process.Start(target);
            //    }
            //}

        }
    }
}
