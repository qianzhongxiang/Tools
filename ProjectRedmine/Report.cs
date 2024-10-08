﻿using System;
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
    public partial class Report : Form
    {
        public Report(RedmineProvider redmineProvider)
        {
            InitializeComponent();
            dateStart.Value = DateTime.Now.Subtract(TimeSpan.FromDays(7));
            RedmineProvider = redmineProvider;
        }

        public RedmineProvider RedmineProvider { get; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RedmineProvider.GenerateJournal(RedmineProvider.Projects.Where(p => p.Name.Contains("VIS 4") || p.Name.Contains("EFEM")), dateStart.Value);
            this.Close();
        }

    }
}