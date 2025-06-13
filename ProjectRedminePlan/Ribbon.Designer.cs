
namespace ProjectRedmine
{
    partial class Ribbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.btnConfig = this.Factory.CreateRibbonButton();
            this.btnRefresh = this.Factory.CreateRibbonButton();
            this.btn_publish = this.Factory.CreateRibbonButton();
            this.btnURL = this.Factory.CreateRibbonButton();
            this.btn_updateIssue = this.Factory.CreateRibbonButton();
            this.g_view = this.Factory.CreateRibbonGroup();
            this.g_upload = this.Factory.CreateRibbonGroup();
            this.g_download = this.Factory.CreateRibbonGroup();
            this.btn_ResetMSTask = this.Factory.CreateRibbonButton();
            this.btn_start = this.Factory.CreateRibbonButton();
            this.g_start = this.Factory.CreateRibbonGroup();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.g_view.SuspendLayout();
            this.g_upload.SuspendLayout();
            this.g_download.SuspendLayout();
            this.g_start.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.g_view);
            this.tab1.Groups.Add(this.g_upload);
            this.tab1.Groups.Add(this.g_download);
            this.tab1.Groups.Add(this.g_start);
            this.tab1.Label = "Redmine";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.btnConfig);
            this.group1.Label = "Setting";
            this.group1.Name = "group1";
            // 
            // btnConfig
            // 
            this.btnConfig.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnConfig.Label = "Config";
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.ShowImage = true;
            this.btnConfig.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnConfig_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnRefresh.Label = "Refresh";
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.ShowImage = true;
            this.btnRefresh.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnRefresh_Click);
            // 
            // btn_publish
            // 
            this.btn_publish.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btn_publish.Label = "Publish";
            this.btn_publish.Name = "btn_publish";
            this.btn_publish.ShowImage = true;
            this.btn_publish.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_publish_Click);
            // 
            // btnURL
            // 
            this.btnURL.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnURL.Label = "OpenURL";
            this.btnURL.Name = "btnURL";
            this.btnURL.ShowImage = true;
            this.btnURL.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnURL_Click);
            // 
            // btn_updateIssue
            // 
            this.btn_updateIssue.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btn_updateIssue.Label = "Update2Issue";
            this.btn_updateIssue.Name = "btn_updateIssue";
            this.btn_updateIssue.ShowImage = true;
            this.btn_updateIssue.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_updateIssue_Click);
            // 
            // g_view
            // 
            this.g_view.Items.Add(this.btnURL);
            this.g_view.Label = "View";
            this.g_view.Name = "g_view";
            // 
            // g_upload
            // 
            this.g_upload.Items.Add(this.btn_publish);
            this.g_upload.Items.Add(this.btn_updateIssue);
            this.g_upload.Label = "Upload";
            this.g_upload.Name = "g_upload";
            // 
            // g_download
            // 
            this.g_download.Items.Add(this.btn_ResetMSTask);
            this.g_download.Items.Add(this.btnRefresh);
            this.g_download.Label = "Download";
            this.g_download.Name = "g_download";
            // 
            // btn_ResetMSTask
            // 
            this.btn_ResetMSTask.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btn_ResetMSTask.Label = "ResetMSTask";
            this.btn_ResetMSTask.Name = "btn_ResetMSTask";
            this.btn_ResetMSTask.ShowImage = true;
            this.btn_ResetMSTask.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_ResetMSTask_Click);
            // 
            // btn_start
            // 
            this.btn_start.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btn_start.Label = "Start";
            this.btn_start.Name = "btn_start";
            this.btn_start.ShowImage = true;
            this.btn_start.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_start_Click);
            // 
            // g_start
            // 
            this.g_start.Items.Add(this.btn_start);
            this.g_start.Label = "Start";
            this.g_start.Name = "g_start";
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Project.Project";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.g_view.ResumeLayout(false);
            this.g_view.PerformLayout();
            this.g_upload.ResumeLayout(false);
            this.g_upload.PerformLayout();
            this.g_download.ResumeLayout(false);
            this.g_download.PerformLayout();
            this.g_start.ResumeLayout(false);
            this.g_start.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnConfig;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnRefresh;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnURL;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_publish;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_updateIssue;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup g_view;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup g_upload;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup g_download;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_ResetMSTask;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup g_start;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_start;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon1
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
