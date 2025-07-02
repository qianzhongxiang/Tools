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
    public partial class StartDemand : Form
    {
        public StartDemand(MSProject.Task tsk, RedmineProvider redmineProvider)
        {
            InitializeComponent();
            Tsk = tsk;
            RedmineProvider = redmineProvider;
            dt_start_issue.ShowCheckBox = true;
            dt_start_issue.Checked = false;
            dt_end_issue.ShowCheckBox = true;
            dt_end_issue.Checked = false;
            txt_description.Enabled = false;
            LoadSubIssues();
            loadScription();
            loadUsers();
            rbtn_design.CheckedChanged += Rbtn_design_CheckedChanged;
            rbtn_fun.CheckedChanged += Rbtn_design_CheckedChanged;
            Rbtn_design_CheckedChanged(null, null);
        }

        private void Rbtn_design_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_design.Checked)
            {
                txt_subject.Text = $"[设计]{Issue.Subject.Replace("[需求]", "")}";
            }
            else if (rbtn_fun.Checked)
            {
                txt_subject.Text = $"[开发]{Issue.Subject.Replace("[需求]", "")}";
            }
        }

        private void loadUsers()
        {
            var members = RedmineProvider.GetMemberships();
            foreach (var member in members)
            {
                if (member.User != null)
                {
                    var rb = new RadioButton { Text = $"{member.User.Name}", Tag = member.User };
                    rb.CheckedChanged += User_CheckedChanged;
                    panel_users.Controls.Add(rb);
                }

            }
        }

        private void User_CheckedChanged(object sender, EventArgs e)
        {
            var rbtn = sender as RadioButton;
            if (rbtn.Checked)
            {
                AssignTo = rbtn.Tag as IdentifiableName;
            }
        }

        private void LoadSubIssues()
        {
            var issues = RedmineProvider.GetSubIssues(Tsk);
            list_subissues.Controls.Clear();
            foreach (var issue in issues)
            {
                var rb = new RadioButton { Text = $"[{issue.Tracker.Name}]({issue.AssignedTo.Name}){issue.Subject}", Tag = issue, AutoSize = true,Dock= DockStyle.Fill, Width = list_subissues.Width };
                rb.CheckedChanged += Rb_CheckedChanged;
                list_subissues.Controls.Add(rb);
            }
        }

        private void loadScription()
        {
            Issue = RedmineProvider.GetIssue((int)Tsk.Number1);
            txt_description_current.TextChanged += Txt_description_current_TextChanged;
            txt_description_current.Text = Issue.Description;
            if (Issue.StartDate.HasValue)
            {
                dt_start.Value = Issue.StartDate.Value;
            }
            if (Issue.DueDate.HasValue)
            {
                dt_end.Value = Issue.DueDate.Value;
            }
            txt_subject.Text = Issue.Subject;
            Txt_description_current_TextChanged(null, null);
        }

        private void Txt_description_current_TextChanged(object sender, EventArgs e)
        {
            list_demands.Controls.Clear();
            var demands = txt_description_current.Text.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var index = 0;
            foreach (var item in demands)
            {
                var cb = new CheckBox { Name = $"d_{index++}", Text = item, AutoSize = true, Dock = DockStyle.Fill, Width = list_demands.Width, };
                cb.CheckedChanged += Cb_CheckedChanged;
                list_demands.Controls.Add(cb);
            }
        }

        private void Cb_CheckedChanged(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;
            StringBuilder sb = new StringBuilder();
            foreach (CheckBox item in list_demands.Controls)
            {
                if (item.Checked)
                {
                    sb.AppendLine(item.Text);
                }
            }
            txt_sub_description.Text = sb.ToString();
        }

        private void Rb_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (sender as RadioButton);
            if (rb.Checked)
            {
                var issue = rb.Tag as Issue;
                if (issue.StartDate.HasValue)
                {
                    dt_start_issue.Checked = true;
                    dt_start_issue.Value = issue.StartDate.Value;
                }
                else
                {
                    dt_start_issue.Checked = false;
                }
                if (issue.DueDate.HasValue)
                {
                    dt_end_issue.Checked = true;
                    dt_end_issue.Value = issue.DueDate.Value;
                }
                else
                {
                    dt_end_issue.Checked = false;
                }
                txt_description.Text = issue.Description;
            }
        }

        public MSProject.Task Tsk { get; }
        public RedmineProvider RedmineProvider { get; }
        public Issue Issue { get; private set; }
        public IdentifiableName AssignTo { get; set; }
        private void btn_createIssue_Click(object sender, EventArgs e)
        {
            if (AssignTo is null)
            {
                return;
            }
            var createdIssue = RedmineProvider.StartTask(txt_subject.Text, txt_sub_description.Text, Tsk.OutlineParent.Name, dt_start.Value, dt_end.Value, rbtn_fun.Checked, Issue.Id, AssignTo.Id);
            if (createdIssue is null)
            {
                return;
            }
            LoadSubIssues();
        }

        private void btn_allDemandsSelected_Click(object sender, EventArgs e)
        {
            var to = !list_demands.Controls.Cast<CheckBox>().All(cb => cb.Checked);
            foreach (CheckBox cb in list_demands.Controls)
            {
                cb.Checked = to;
            }
        }

        private void btn_designAchieve_Click(object sender, EventArgs e)
        {

        }
    }
}
