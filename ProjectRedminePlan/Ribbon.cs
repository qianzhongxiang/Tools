using Microsoft.Office.Tools.Ribbon;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private async void btnRefresh_Click(object sender, RibbonControlEventArgs e)
        {
            LoadProvider();
            switch (mSProjectWrapper.ProjType)
            {
                case ProjectType.Plan:
                    await PlanRefresh();
                    break;
                case ProjectType.Resource:
                    await ResourceRefresh();
                    break;
                default:
                    return;
            }
            DateTime now = DateTime.UtcNow;
            mSProjectWrapper.UpdateTime = now;
            mSProjectWrapper.SaveParameters();
        }
        private async Task ResourceRefresh()
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
                        var subv = new ResourceSubVersion(mSProjectWrapper, item, redmineProvider);
                        subv.Create();
                        await subv.AddSubTasks();
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

        private async Task PlanRefresh()
        {
            if (mSProjectWrapper.Pj.Tasks.Cast<MSProject.Task>().Any(item => item.OutlineLevel == 2 && item.Number2 > 0.5 && item.Number1 > 0.5))
            {
                var res = MessageBox.Show("存在修改过的公开需求，应该先处理，是否先处理修改", "***", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    btn_updateIssue_Click(null, null);
                    return;
                }
            }
            if (this.Application.ActiveProject.Comments is null)
            {
                System.Windows.Forms.MessageBox.Show("please set version first");
                return;
            }
            //redmineProvider = new RedmineProvider(this.Application.ActiveProject.Comments as string);


            var versions = mSProjectWrapper.Versions();
            foreach (var item in versions)
            {
                var subv = new SubVersion(mSProjectWrapper, item.Name, item, redmineProvider);
                await subv.UpdateTasksAsync();
            }
        }

        private void LoadProvider()
        {
            if (mSProjectWrapper is null)
            {

                mSProjectWrapper = MSProjectWrapper.CreateOrNewWrapper(this.Application.ActiveProject);
            }
            if (redmineProvider is null)
            {

                redmineProvider = new RedmineProvider(mSProjectWrapper.RedmineProj, mSProjectWrapper.Version);
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
            var wrapper = MSProjectWrapper.CreateOrNewWrapper(this.Application.ActiveProject);
            if (wrapper == null)
            {
                wrapper = new MSProjectWrapper();
                wrapper.Pj = this.Application.ActiveProject;
            }
            new SetDialog(wrapper).ShowDialog();
            this.Application.CustomFieldRename(MSProject.PjCustomField.pjCustomTaskText1, "RedmineStatus");
            this.Application.CustomFieldRename(MSProject.PjCustomField.pjCustomTaskText2, "Creator");
            this.Application.CustomFieldRename(MSProject.PjCustomField.pjCustomTaskNumber1, "IssueID");
            this.Application.CustomFieldRename(MSProject.PjCustomField.pjCustomTaskNumber2, "Changed");
        }


        private void btnURL_Click(object sender, RibbonControlEventArgs e)
        {
            LoadProvider();
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
            else
            {
                if (c != null && c.Assignment != null)
                {
                    var item = c.Assignment.Task;
                    string target = $"{RedmineProvider.Host}/issues/{item.Number1}";
                    System.Diagnostics.Process.Start(target);
                }
            }


        }

        private void btn_publish_Click(object sender, RibbonControlEventArgs e)
        {
            LoadProvider();
            if (mSProjectWrapper.ProjType != ProjectType.Plan)
            {
                return;
            }
            var tasks = this.Application.ActiveSelection.Tasks;
            if (tasks != null)
            {
                foreach (MSProject.Task item in tasks)
                {
                    if (item.Number1 > 0.5)
                    {
                        continue;
                    }
                    var iss = redmineProvider.PublishTask(item);
                    ThisAddIn.redmineProvider.UpdateMSTask(iss, item);
                }
            }
        }

        private void btn_updateIssue_Click(object sender, RibbonControlEventArgs e)
        {
            LoadProvider();
            if (mSProjectWrapper.ProjType != ProjectType.Plan)
            {
                return;
            }
            foreach (MSProject.Task item in mSProjectWrapper.Pj.Tasks)
            {
                if (item.OutlineLevel == 2 && item.Number2 > 0.5 && item.Number1 > 0.5)
                {
                    var issue = redmineProvider.GetIssue((int)item.Number1);
                    var res = new UpdateIssue(item, issue).ShowDialog();
                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        redmineProvider.UpdateIssue(item);
                    }
                    else if (res == System.Windows.Forms.DialogResult.Ignore)
                    {
                        redmineProvider.UpdateMSTask(issue, item);
                    }
                }
            }
        }

        private void btn_ResetMSTask_Click(object sender, RibbonControlEventArgs e)
        {
            LoadProvider();
            if (mSProjectWrapper.ProjType != ProjectType.Plan)
            {
                return;
            }
            var tasks = this.Application.ActiveSelection.Tasks;
            foreach (MSProject.Task item in tasks)
            {
                if (item.OutlineLevel == 2 && item.Number1 > 0.5)
                {
                    var issue = redmineProvider.GetIssue((int)item.Number1);
                    redmineProvider.UpdateMSTask(issue, item);
                }
            }
        }

        private void btn_start_Click(object sender, RibbonControlEventArgs e)
        {
            LoadProvider();
            if (mSProjectWrapper.ProjType != ProjectType.Plan)
            {
                return;
            }
            var tasks = this.Application.ActiveSelection.Tasks;
            foreach (MSProject.Task item in tasks)
            {
                if (item.OutlineLevel == 2 && item.Number1 > 0.5)
                {
                    new StartDemand(item, redmineProvider).ShowDialog();
                }
            }
        }
    }
}
