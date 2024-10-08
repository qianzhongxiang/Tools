using Microsoft.Office.Tools.Ribbon;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            //btnSet.Click += BtnSet_Click; 
            this.Application.ProjectBeforeTaskChange += Application_ProjectBeforeTaskChange;
            //this.Application.ProjectBeforeResourceChange += Application_ProjectBeforeResourceChange;
            //this.Application.ProjectBeforeTaskNew += Application_ProjectBeforeTaskNew;
            this.Application.NewProject += Application_NewProject;
            //this.Application.ProjectBeforeTaskChange2 += Application_ProjectBeforeTaskChange2;
            //this.Application.ProjectBeforeTaskDelete += Application_ProjectBeforeTaskDelete;
            //this.Application.ProjectBeforeTaskDelete2 += Application_ProjectBeforeTaskDelete2;
            //this.Application.ProjectBeforeTaskNew2 += Application_ProjectBeforeTaskNew2;
            //this.Application.ProjectResourceNew += Application_ProjectResourceNew;
            //this.Application.SecondaryViewChange += Application_SecondaryViewChange;
            //this.Application.WindowSelectionChange += Application_WindowSelectionChange;
            //this.Application.WindowViewChange += Application_WindowViewChange;
            //this.Application.ProjectBeforeAssignmentChange += Application_ProjectBeforeAssignmentChange;
            //this.Application.ProjectAssignmentNew += Application_ProjectAssignmentNew;
            //this.Application.ProjectBeforeAssignmentNew += Application_ProjectBeforeAssignmentNew;
            //          Task.Run(delegate
            //{
            //    while (true)
            //    {
            //        try
            //        {
            //            if (this.Application.ActiveProject != null)
            //            {
            //                var wrapper = MSProjectWrapper.CreateOrNewWrapper(this.Application.ActiveProject);
            //                wrapper.Update();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            System.Diagnostics.Debug.WriteLine($"err {ex.Message}");
            //        }

            //        System.Threading.Thread.Sleep(60000);
            //    }
            //});
        }

        //private void Application_ProjectBeforeAssignmentNew(MSProject.Project pj, ref bool Cancel)
        //{
        //}

        //private void Application_ProjectAssignmentNew(MSProject.Project pj, int ID)
        //{
        //}

        //private void Application_ProjectBeforeAssignmentChange(MSProject.Assignment asg, MSProject.PjAssignmentField Field, object NewVal, ref bool Cancel)
        //{

        //}

        //private void Application_WindowViewChange(MSProject.Window Window, MSProject.View prevView, MSProject.View newView, bool success)
        //{
        //}

        //private void Application_WindowSelectionChange(MSProject.Window Window, MSProject.Selection sel, object selType)
        //{

        //}

        //private void Application_SecondaryViewChange(MSProject.Window Window, MSProject.View prevView, MSProject.View newView, bool success)
        //{
        //}

        //private void Application_ProjectResourceNew(MSProject.Project pj, int ID)
        //{
        //}

        //private void Application_ProjectBeforeTaskNew2(MSProject.Project pj, MSProject.EventInfo Info)
        //{
        //}

        //private void Application_ProjectBeforeTaskDelete2(MSProject.Task tsk, MSProject.EventInfo Info)
        //{
        //}

        //private void Application_ProjectBeforeTaskDelete(MSProject.Task tsk, ref bool Cancel)
        //{
        //}

        //private void Application_ProjectBeforeTaskChange2(MSProject.Task tsk, MSProject.PjField Field, object NewVal, MSProject.EventInfo Info)
        //{
        //}

        //private void Application_ProjectBeforeResourceChange1(MSProject.Resource res, MSProject.PjField Field, object NewVal, ref bool Cancel)
        //{
        //}

        private void ActiveProject_Open(MSProject.Project pj)
        {
        }

        private void ActiveProject_Change(MSProject.Project pj)
        {
            //try
            //{
            //    var cell = this.Application.ActiveCell;
            //    if (cell != null & cell.Assignment != null)
            //    {
            //        mSProjectWrapper = MSProjectWrapper.CreateOrNewWrapper(this.Application.ActiveProject);
            //        redmineProvider = mSProjectWrapper.RedmineProvider;
            //        var tsk = cell.Assignment.Task;
            //        redmineProvider.ChangeTask((int)tsk.Number1, tsk.Start, tsk.Duration, tsk);
            //    }
            //}
            //catch (Exception)
            //{

            //}

            if (tasks.Count > 0)
            {
                var t = tasks.Dequeue();
                redmineProvider.ChangeTask(t);
            }
        }

        private void Application_NewProject(MSProject.Project pj)
        {
            mSProjectWrapper = MSProjectWrapper.CreateOrNewWrapper(pj);
            if (mSProjectWrapper != null)
            {
                pj.Change += ActiveProject_Change;
                pj.Open += ActiveProject_Open;
                redmineProvider = mSProjectWrapper.RedmineProvider;
            }
        }

        //private void Application_ProjectBeforeTaskNew(MSProject.Project pj, ref bool Cancel)
        //{
        //}

        //private void Application_ProjectBeforeResourceChange(MSProject.Resource res, MSProject.PjField Field, object NewVal, ref bool Cancel)
        //{
        //}


        MSProject.Task changedT;
        Task changeTask;
        Queue<MSProject.Task> tasks = new Queue<MSProject.Task>();
        private void Application_ProjectBeforeTaskChange(MSProject.Task tsk, MSProject.PjField Field, object NewVal, ref bool Cancel)
        {
            if (Fresh)
            {
                return;
            }
            if (tsk.ID == 0)
            {
                return;
            }
            //if (tsk.Name.StartsWith("[需求]"))
            //{
            //    Cancel = true;
            //    var res = System.Windows.Forms.MessageBox.Show($"the demand task cannot be changed {tsk.ID}:{tsk.Name}, {Field} {NewVal}", "Info", System.Windows.Forms.MessageBoxButtons.OK);
            //}
            if (tasks.Count == 0 || tasks.LastOrDefault().ID != tsk.ID)
            {
                tasks.Enqueue(tsk);
            }
            //if (changedT != null && changedT.ID == tsk.ID)
            //{
            //    return;
            //}
            //if (changeTask != null)
            //{
            //    changeTask.Wait();
            //}
            //changeTask = Task.Run(() =>
            // {
            //     changedT = tsk;
            //     System.Threading.Thread.Sleep(200);
            //     changedT = null;
            //     redmineProvider.ChangeTask(tsk);

            // });
            //changeTask.Wait();
            //switch (Field)
            //{
            //    case MSProject.PjField.pjTaskConstraintType:
            //        return;
            //    case MSProject.PjField.pjTaskStartText:
            //    case MSProject.PjField.pjTaskActualStart:
            //        mSProjectWrapper = MSProjectWrapper.CreateOrNewWrapper(this.Application.ActiveProject);
            //        redmineProvider = mSProjectWrapper.RedmineProvider;
            //        switch (NewVal)
            //        {
            //            case DateTime dt:
            //                redmineProvider.ChangeTask((int)tsk.Number1, dt, tsk.Duration, tsk);
            //                break;
            //            case string sd:
            //                redmineProvider.ChangeTask((int)tsk.Number1, DateTime.Parse(sd), tsk.Duration, tsk);
            //                break;
            //            default:
            //                break;
            //        }

            //        break;
            //    case MSProject.PjField.pjTaskFixedDuration:
            //    case MSProject.PjField.pjTaskActualDuration:
            //    case MSProject.PjField.pjTaskDuration:
            //    case MSProject.PjField.pjTaskDurationText:
            //        mSProjectWrapper = MSProjectWrapper.CreateOrNewWrapper(this.Application.ActiveProject);
            //        redmineProvider = mSProjectWrapper.RedmineProvider;
            //        switch (NewVal)
            //        {
            //            case string strVal:
            //                var d = double.Parse(strVal.Split('d')[0]);
            //                redmineProvider.ChangeTask((int)tsk.Number1, (DateTime)tsk.Start, (int)d * 8 * 60, tsk);
            //                //else if (strVal.Contains('h'))
            //                //{
            //                //    var h = double.Parse(strVal.Split('h')[0]);
            //                //    redmineProvider.ChangeTask((int)tsk.Number1, (DateTime)tsk.Start, (int)h * 60, tsk);
            //                //}
            //                break;
            //            case decimal mins:
            //                redmineProvider.ChangeTask((int)tsk.Number1, (DateTime)tsk.Start, (int)mins, tsk);
            //                break;
            //            default:
            //                break;
            //        }

            //        break;
            //    default:
            //        return;
            //}

            //var res = System.Windows.Forms.MessageBox.Show($"change {tsk.ID}:{tsk.Name}, {Field}", "Info", System.Windows.Forms.MessageBoxButtons.OKCancel);
            //if (res == System.Windows.Forms.DialogResult.OK)
            //{
            //mSProjectWrapper = MSProjectWrapper.CreateOrNewWrapper(this.Application.ActiveProject);
            //redmineProvider = mSProjectWrapper.RedmineProvider;
            //redmineProvider.ChangeTask((int)tsk.Number1, (DateTime)tsk.Start, tsk.Duration);
            //}
            //else
            //{
            //    Cancel = true;
            //}
        }



        private void BtnSet_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            new SetDialog(MSProjectWrapper.CreateOrNewWrapper(this.Application.ActiveProject)).ShowDialog();
        }

        private void BtnRefresh_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {

        }

        public static RedmineProvider redmineProvider;
        static MSProjectWrapper mSProjectWrapper;
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
