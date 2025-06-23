using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectRedmine
{
    public partial class SetDialog : Form
    {

        public Microsoft.Office.Interop.MSProject.Project proj { get; set; }
        public MSProjectWrapper MSProjectWrapper { get; }

        public SetDialog(MSProjectWrapper mSProjectWrapper)
        {
            InitializeComponent();
            if (mSProjectWrapper == null)
            {
                throw new ArgumentNullException(nameof(mSProjectWrapper));
            }
            proj = mSProjectWrapper.Pj;
            MSProjectWrapper = mSProjectWrapper;
            switch (MSProjectWrapper.ProjType)
            {
                case ProjectType.Plan:
                    rbtn_plan.Checked = true;
                    break;
                case ProjectType.Resource:
                    rbtn_resource.Checked = true;
                    break;
                default:
                    break;
            }

            rbtn_plan.Tag = ProjectType.Plan;
            rbtn_plan.CheckedChanged += Rbtn_resource_plan_CheckedChanged;
            rbtn_resource.Tag = ProjectType.Resource;
            rbtn_resource.CheckedChanged += Rbtn_resource_plan_CheckedChanged;
         
            txtProjName.Text = MSProjectWrapper.RedmineProj;
            txtVersion.Text = MSProjectWrapper.Version;
            labUpdateTime.Text = MSProjectWrapper.UpdateTimeStr;
        }

        private void Rbtn_resource_plan_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                MSProjectWrapper.ProjType = (ProjectType)rb.Tag;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            MSProjectWrapper.RedmineProj = txtProjName.Text;
            MSProjectWrapper.Version = txtVersion.Text;
            MSProjectWrapper.SaveParameters();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
