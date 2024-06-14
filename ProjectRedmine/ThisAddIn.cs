﻿using Redmine.Net.Api;
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
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            var prBar = this.Application.CommandBars.Add("Project Redmine", Office.MsoBarPosition.msoBarTop, false, true);
            prBar.Visible = true;
            prBar.NameLocal = "Project Redmine";
            var button = prBar.Controls.Add(Office.MsoControlType.msoControlButton, missing, missing, missing, true) as Office.CommandBarButton;
            button.Caption = "Load Tasks";
            button.Tag = "LoadTasks";
            button.Style = Office.MsoButtonStyle.msoButtonCaption;
            button.Click += Button_Click;

            var btnRefresh = prBar.Controls.Add(Office.MsoControlType.msoControlButton, missing, missing, missing, true) as Office.CommandBarButton;
            btnRefresh.Caption = "Refresh";
            btnRefresh.Tag = "Refresh";
            btnRefresh.Style = Office.MsoButtonStyle.msoButtonCaption;
            btnRefresh.Click += BtnRefresh_Click;


            var btnSet = prBar.Controls.Add(Office.MsoControlType.msoControlButton, missing, missing, missing, true) as Office.CommandBarButton;
            btnSet.Caption = "Set";
            btnSet.Tag = "Set";
            btnSet.Style = Office.MsoButtonStyle.msoButtonCaption;
            btnSet.Click += BtnSet_Click; ;
        }

        private void BtnSet_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            new SetDialog(new MSProjectWrapper(this.Application.ActiveProject)).ShowDialog();
            //this.Application.ActiveProject.Comments = "VIS 4;4.3";
        }

        private void BtnRefresh_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            if (this.Application.ActiveProject.Comments is null)
            {
                System.Windows.Forms.MessageBox.Show("please set version first");
                return;
            }
            mSProjectWrapper = new MSProjectWrapper(this.Application.ActiveProject);
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

        RedmineProvider redmineProvider;
        MSProjectWrapper mSProjectWrapper;
        private void Button_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            if (this.Application.ActiveProject.Comments is null)
            {
                System.Windows.Forms.MessageBox.Show("please set version first");
                return;
            }
            this.Application.CustomFieldRename(MSProject.PjCustomField.pjCustomTaskText1, "RedmineStatus");
            this.Application.CustomFieldRename(MSProject.PjCustomField.pjCustomTaskText2, "Creator");
            this.Application.CustomFieldRename(MSProject.PjCustomField.pjCustomTaskNumber1, "IssueID");
            //redmineProvider = new RedmineProvider(this.Application.ActiveProject.Comments as string);
            mSProjectWrapper = new MSProjectWrapper(this.Application.ActiveProject);
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