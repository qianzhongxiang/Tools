using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
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
                var now = DateTime.Now;
                foreach (var item in redmineProvider.Versions)
                {
                    var subv = new SubVersion(mSProjectWrapper, item, redmineProvider);
                    subv.Create();
                    subv.AddSubTasks();
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
            ThisAddIn.Fresh = true;
            try
            {
                if (this.Application.ActiveProject.Comments is null)
                {
                    System.Windows.Forms.MessageBox.Show("please set version first");
                    return;
                }
                mSProjectWrapper = MSProjectWrapper.CreateOrNewWrapper(this.Application.ActiveProject);
                if (mSProjectWrapper.UpdateTime is null)
                {
                    System.Windows.Forms.MessageBox.Show("please LoadData first");
                    return;
                }
                redmineProvider = new RedmineProvider(mSProjectWrapper.RedmineProj, mSProjectWrapper.Version);

                var now = DateTime.Now;
                foreach (var item in redmineProvider.Versions)
                {
                    var subv = new SubVersion(mSProjectWrapper, item, redmineProvider);
                    subv.UpdateTasks();
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
            redmineProvider.GenerateJournal(redmineProvider.Projects.Where(p => p.Name.Contains("VIS 4") || p.Name.Contains("EFEM")));
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
                redmineProvider.ChangeTask((int)t.Number1, (DateTime)t.Start, t.Duration);
            }
        }
    }
}
