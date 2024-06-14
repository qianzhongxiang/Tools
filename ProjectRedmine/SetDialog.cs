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
            MSProjectWrapper = mSProjectWrapper;
            proj = MSProjectWrapper.Pj;
            InitializeComponent();
            txtProjName.Text = MSProjectWrapper.RedmineProj;
            txtVersion.Text = MSProjectWrapper.Version;
            labUpdateTime.Text = MSProjectWrapper.UpdateTimeStr; 
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
