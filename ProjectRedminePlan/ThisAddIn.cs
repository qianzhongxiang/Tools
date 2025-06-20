using Microsoft.Office.Core;
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
using static NPOI.HSSF.UserModel.HeaderFooter;
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
            //this.Application.ProjectBeforeResourceChange += Application_ProjectBeforeResourceChange;
            //this.Application.ProjectBeforeTaskNew += Application_ProjectBeforeTaskNew;
            this.Application.NewProject += Application_NewProject;
            this.Application.ProjectBeforeTaskChange2 += Application_ProjectBeforeTaskChange2;
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





        private void AddCustomTaskContextMenu()
        {
            try
            {
                // 获取 Project 应用程序的 CommandBars 集合
                Office.CommandBars commandBars = (Office.CommandBars)Application.CommandBars;
                foreach (Office.CommandBar cb in commandBars)
                {
                    System.Diagnostics.Debug.WriteLine(cb.Name);
                }
                // 找到任务行的右键菜单。它的名称通常是 "Task Row"
                Office.CommandBar taskContextMenu = commandBars["Task Pane"];

                if (taskContextMenu != null)
                {
                    // 在菜单中添加一个新按钮 (CommandBarButton)
                    // 参数: Type, Id, Parameter, Before, Temporary
                    // msoControlButton 代表一个普通的按钮
                    // Id 通常用于内置控件，自定义控件可以设为 Missing.Value
                    // Before 指定插入的位置，Missing.Value 表示添加到末尾
                    // Temporary 设置为 true，则在应用程序关闭时自动移除此控件
                    foreach (CommandBarControl item in taskContextMenu.Controls)
                    {
                        System.Diagnostics.Debug.WriteLine(item.Type);
                    }
                    var Btn_ResetFromRedmine = (Office.CommandBarButton)taskContextMenu.Controls.Add(
                        Office.MsoControlType.msoControlButton,
                        System.Type.Missing,
                        System.Type.Missing,
                        1, // 插入到第一个位置（可选，可以不指定，默认末尾）
                        true // Temporary = true 表示在加载项卸载时自动移除
                    );

                    // 设置按钮的属性
                    Btn_ResetFromRedmine.Caption = "ResetFromRedmine"; // 显示文本
                    Btn_ResetFromRedmine.Tag = "ResetFromRedmine"; // 用于标识的标签，方便查找
                    //Btn_Update2Redmine.OnAction = "MyProjectAddIn.ThisAddIn.HandleCustomTaskButtonClick"; // VBA 宏名或 COM Add-in 回调

                    // 对于 VSTO Add-in，我们通常通过事件处理程序来响应点击
                    Btn_ResetFromRedmine.Click += ResetFromRedmine_Click; ;

                    // 确保菜单可见（如果它之前被隐藏了）
                    taskContextMenu.Visible = true;
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("添加自定义任务右键菜单时出错: " + ex.Message);
            }
        }

        private void ResetFromRedmine_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
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


        //private void Application_ProjectBeforeTaskDelete2(MSProject.Task tsk, MSProject.EventInfo Info)
        //{
        //}

        //private void Application_ProjectBeforeTaskDelete(MSProject.Task tsk, ref bool Cancel)
        //{
        //}

        private void Application_ProjectBeforeTaskChange2(MSProject.Task tsk, MSProject.PjField Field, object NewVal, MSProject.EventInfo Info)
        {
            if (!Fresh)
            {
                if (Field == MSProject.PjField.pjTaskNumber1)
                {
                    Info.Cancel = true;
                }

                if (tsk.Number1 > 0)
                {
                    tsk.Number2++;
                    //Info.Cancel = true;
                }
            }

        }

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

            //if (tasks.Count > 0)
            //{
            //    var t = tasks.Dequeue();
            //    redmineProvider.UpdateIssue(t);
            //}
        }

        private void Application_NewProject(MSProject.Project pj)
        {
            mSProjectWrapper = MSProjectWrapper.CreateOrNewWrapper(pj);
            if (mSProjectWrapper != null)
            {
                pj.Change += ActiveProject_Change;
                pj.Open += ActiveProject_Open;
                redmineProvider = mSProjectWrapper.RedmineProvider;
                //AddCustomTaskContextMenu();
            }
        }

        //private void Application_ProjectBeforeTaskNew(MSProject.Project pj, ref bool Cancel)
        //{
        //}

        //private void Application_ProjectBeforeResourceChange(MSProject.Resource res, MSProject.PjField Field, object NewVal, ref bool Cancel)
        //{
        //}


    

        public static RedmineProvider redmineProvider;
        static MSProjectWrapper mSProjectWrapper;

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
