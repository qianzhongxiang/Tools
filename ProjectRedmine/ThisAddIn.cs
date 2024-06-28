using Microsoft.Office.Tools.Ribbon;
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
    public partial class ThisAddIn
    {
        internal static bool Fresh = false;
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            //var prBar = this.Application.CommandBars.Add("Project Redmine", Office.MsoBarPosition.msoBarTop, false, true);
            //prBar.Visible = true;
            //prBar.NameLocal = "Project Redmine";
            //var button = prBar.Controls.Add(Office.MsoControlType.msoControlButton, missing, missing, missing, true) as Office.CommandBarButton;
            //button.Caption = "Load Tasks";
            //button.Tag = "LoadTasks";
            //button.Style = Office.MsoButtonStyle.msoButtonCaption;
            //button.Click += Button_Click;

            //var btnRefresh = prBar.Controls.Add(Office.MsoControlType.msoControlButton, missing, missing, missing, true) as Office.CommandBarButton;
            //btnRefresh.Caption = "Refresh";
            //btnRefresh.Tag = "Refresh";
            //btnRefresh.Style = Office.MsoButtonStyle.msoButtonCaption;
            //btnRefresh.Click += BtnRefresh_Click;


            //var btnSet = prBar.Controls.Add(Office.MsoControlType.msoControlButton, missing, missing, missing, true) as Office.CommandBarButton;
            //btnSet.Caption = "Set";
            //btnSet.Tag = "Set";
            //btnSet.Style = Office.MsoButtonStyle.msoButtonCaption;
            //btnSet.Click += BtnSet_Click; ;
            this.Application.ProjectBeforeTaskChange += Application_ProjectBeforeTaskChange;
            //this.Application.ProjectBeforeTaskChange2 += Application_ProjectBeforeTaskChange2;
        }

        private void Application_ProjectBeforeTaskChange2(MSProject.Task tsk, MSProject.PjField Field, object NewVal, MSProject.EventInfo Info)
        {
            if (Fresh)
            {
                return;
            }
            var res = System.Windows.Forms.MessageBox.Show($"change {tsk.ID}:{tsk.Name}, {Field}", "Info", System.Windows.Forms.MessageBoxButtons.OKCancel);
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                mSProjectWrapper = new MSProjectWrapper(this.Application.ActiveProject);
                redmineProvider = new RedmineProvider(mSProjectWrapper.RedmineProj, mSProjectWrapper.Version);
                redmineProvider.ChangeTask((int)tsk.Number1, (DateTime)tsk.Start, tsk.Duration);
            }
        }

        private void Application_ProjectBeforeTaskChange(MSProject.Task tsk, MSProject.PjField Field, object NewVal, ref bool Cancel)
        {
            if (Fresh)
            {
                return;
            }
            var res = System.Windows.Forms.MessageBox.Show($"change {tsk.ID}:{tsk.Name}, {Field}", "Info", System.Windows.Forms.MessageBoxButtons.OKCancel);
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                mSProjectWrapper = new MSProjectWrapper(this.Application.ActiveProject);
                redmineProvider = new RedmineProvider(mSProjectWrapper.RedmineProj, mSProjectWrapper.Version);
                redmineProvider.ChangeTask((int)tsk.Number1, (DateTime)tsk.Start, tsk.Duration);
            }
        }



        private void BtnSet_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            new SetDialog(new MSProjectWrapper(this.Application.ActiveProject)).ShowDialog();
        }

        private void BtnRefresh_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {

        }

        RedmineProvider redmineProvider;
        MSProjectWrapper mSProjectWrapper;
        private void Button_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {

        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        protected override IRibbonExtension[] CreateRibbonObjects()
        {
            return new IRibbonExtension[] { new Ribbon() { } };
        }


        #endregion
    }
    class VersionCompare : IComparer<Redmine.Net.Api.Types.Version>
    {
        public int Compare(Redmine.Net.Api.Types.Version x, Redmine.Net.Api.Types.Version y)
        {
            return string.Compare(x.Name, y.Name);
        }
    }
}
