using Microsoft.Office.Interop.MSProject;
using Redmine.Net.Api.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSProject = Microsoft.Office.Interop.MSProject;

namespace ProjectRedmine
{
    public partial class UpdateIssue : Form
    {
        public MSProject.Task Task { get; }
        public Issue Issue { get; }

        public UpdateIssue(MSProject.Task task, Issue issue)
        {
            InitializeComponent();
            Task = task;
            Issue = issue;
            txt_name.Text = task.Name;
            txt_description.Text = task.Notes;
            dt_start.Value = task.Start;
            dt_end.Value = task.Finish;
            txt_version.Text = task.OutlineParent.Name;
            txt_version_issue.Text = issue.FixedVersion.Name;

            txt_name_redmine.Text = issue.Subject;
            txt_description_redmine.Text = issue.Description;
            if (issue.StartDate.HasValue)
            {
                dt_start_redmine.Value = issue.StartDate.Value;
                dt_start_redmine.Checked = true;
            }
            if (issue.DueDate.HasValue)
            {
                dt_end_redmine.Value = issue.DueDate.Value;
                dt_end_redmine.Checked = true;
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            Task.Name = txt_name.Text;
            Task.Notes = txt_description.Text;
            Task.Start = dt_start.Value;
            Task.Finish = dt_end.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btn_undo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Ignore;
            Close();
        }
    }
}
