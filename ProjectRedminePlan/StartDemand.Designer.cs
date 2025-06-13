namespace ProjectRedmine
{
    partial class StartDemand
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_description_current = new System.Windows.Forms.RichTextBox();
            this.txt_description = new System.Windows.Forms.RichTextBox();
            this.list_subissues = new System.Windows.Forms.FlowLayoutPanel();
            this.panel_users = new System.Windows.Forms.FlowLayoutPanel();
            this.dt_start_issue = new System.Windows.Forms.DateTimePicker();
            this.btn_createIssue = new System.Windows.Forms.Button();
            this.dt_end_issue = new System.Windows.Forms.DateTimePicker();
            this.btn_allDemandsSelected = new System.Windows.Forms.Button();
            this.btn_saveDescription = new System.Windows.Forms.Button();
            this.txt_subject = new System.Windows.Forms.TextBox();
            this.txt_sub_description = new System.Windows.Forms.RichTextBox();
            this.rbtn_design = new System.Windows.Forms.RadioButton();
            this.rbtn_fun = new System.Windows.Forms.RadioButton();
            this.dt_start = new System.Windows.Forms.DateTimePicker();
            this.dt_end = new System.Windows.Forms.DateTimePicker();
            this.list_demands = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // txt_description_current
            // 
            this.txt_description_current.Location = new System.Drawing.Point(300, 2);
            this.txt_description_current.Name = "txt_description_current";
            this.txt_description_current.Size = new System.Drawing.Size(339, 304);
            this.txt_description_current.TabIndex = 0;
            this.txt_description_current.Text = "";
            // 
            // txt_description
            // 
            this.txt_description.Location = new System.Drawing.Point(15, 311);
            this.txt_description.Name = "txt_description";
            this.txt_description.Size = new System.Drawing.Size(279, 278);
            this.txt_description.TabIndex = 0;
            this.txt_description.Text = "";
            // 
            // list_subissues
            // 
            this.list_subissues.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.list_subissues.Location = new System.Drawing.Point(12, 12);
            this.list_subissues.Name = "list_subissues";
            this.list_subissues.Size = new System.Drawing.Size(279, 265);
            this.list_subissues.TabIndex = 8;
            // 
            // panel_users
            // 
            this.panel_users.Location = new System.Drawing.Point(645, 2);
            this.panel_users.Name = "panel_users";
            this.panel_users.Size = new System.Drawing.Size(365, 304);
            this.panel_users.TabIndex = 9;
            // 
            // dt_start_issue
            // 
            this.dt_start_issue.Location = new System.Drawing.Point(15, 283);
            this.dt_start_issue.Name = "dt_start_issue";
            this.dt_start_issue.Size = new System.Drawing.Size(119, 21);
            this.dt_start_issue.TabIndex = 10;
            // 
            // btn_createIssue
            // 
            this.btn_createIssue.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_createIssue.Location = new System.Drawing.Point(615, 599);
            this.btn_createIssue.Name = "btn_createIssue";
            this.btn_createIssue.Size = new System.Drawing.Size(100, 40);
            this.btn_createIssue.TabIndex = 11;
            this.btn_createIssue.Text = "创建问题单";
            this.btn_createIssue.UseVisualStyleBackColor = true;
            this.btn_createIssue.Click += new System.EventHandler(this.btn_createIssue_Click);
            // 
            // dt_end_issue
            // 
            this.dt_end_issue.Location = new System.Drawing.Point(147, 283);
            this.dt_end_issue.Name = "dt_end_issue";
            this.dt_end_issue.Size = new System.Drawing.Size(147, 21);
            this.dt_end_issue.TabIndex = 13;
            // 
            // btn_allDemandsSelected
            // 
            this.btn_allDemandsSelected.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_allDemandsSelected.Location = new System.Drawing.Point(300, 600);
            this.btn_allDemandsSelected.Name = "btn_allDemandsSelected";
            this.btn_allDemandsSelected.Size = new System.Drawing.Size(100, 40);
            this.btn_allDemandsSelected.TabIndex = 14;
            this.btn_allDemandsSelected.Text = "全选";
            this.btn_allDemandsSelected.UseVisualStyleBackColor = true;
            this.btn_allDemandsSelected.Click += new System.EventHandler(this.btn_allDemandsSelected_Click);
            // 
            // btn_saveDescription
            // 
            this.btn_saveDescription.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_saveDescription.Location = new System.Drawing.Point(476, 599);
            this.btn_saveDescription.Name = "btn_saveDescription";
            this.btn_saveDescription.Size = new System.Drawing.Size(100, 40);
            this.btn_saveDescription.TabIndex = 15;
            this.btn_saveDescription.Text = "保存描述";
            this.btn_saveDescription.UseVisualStyleBackColor = true;
            // 
            // txt_subject
            // 
            this.txt_subject.Location = new System.Drawing.Point(674, 311);
            this.txt_subject.Name = "txt_subject";
            this.txt_subject.Size = new System.Drawing.Size(336, 21);
            this.txt_subject.TabIndex = 16;
            // 
            // txt_sub_description
            // 
            this.txt_sub_description.Location = new System.Drawing.Point(674, 367);
            this.txt_sub_description.Name = "txt_sub_description";
            this.txt_sub_description.Size = new System.Drawing.Size(336, 222);
            this.txt_sub_description.TabIndex = 17;
            this.txt_sub_description.Text = "";
            // 
            // rbtn_design
            // 
            this.rbtn_design.Checked = true;
            this.rbtn_design.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtn_design.Location = new System.Drawing.Point(752, 600);
            this.rbtn_design.Name = "rbtn_design";
            this.rbtn_design.Size = new System.Drawing.Size(60, 30);
            this.rbtn_design.TabIndex = 18;
            this.rbtn_design.TabStop = true;
            this.rbtn_design.Text = "设计";
            this.rbtn_design.UseVisualStyleBackColor = true;
            // 
            // rbtn_fun
            // 
            this.rbtn_fun.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtn_fun.Location = new System.Drawing.Point(834, 600);
            this.rbtn_fun.Name = "rbtn_fun";
            this.rbtn_fun.Size = new System.Drawing.Size(60, 30);
            this.rbtn_fun.TabIndex = 19;
            this.rbtn_fun.Text = "功能";
            this.rbtn_fun.UseVisualStyleBackColor = true;
            // 
            // dt_start
            // 
            this.dt_start.Location = new System.Drawing.Point(674, 338);
            this.dt_start.Name = "dt_start";
            this.dt_start.Size = new System.Drawing.Size(151, 21);
            this.dt_start.TabIndex = 10;
            // 
            // dt_end
            // 
            this.dt_end.Location = new System.Drawing.Point(867, 338);
            this.dt_end.Name = "dt_end";
            this.dt_end.Size = new System.Drawing.Size(143, 21);
            this.dt_end.TabIndex = 13;
            // 
            // list_demands
            // 
            this.list_demands.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.list_demands.Location = new System.Drawing.Point(300, 312);
            this.list_demands.Name = "list_demands";
            this.list_demands.Size = new System.Drawing.Size(339, 277);
            this.list_demands.TabIndex = 9;
            // 
            // StartDemand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 656);
            this.Controls.Add(this.rbtn_fun);
            this.Controls.Add(this.rbtn_design);
            this.Controls.Add(this.txt_sub_description);
            this.Controls.Add(this.txt_subject);
            this.Controls.Add(this.btn_saveDescription);
            this.Controls.Add(this.btn_allDemandsSelected);
            this.Controls.Add(this.dt_end);
            this.Controls.Add(this.dt_end_issue);
            this.Controls.Add(this.dt_start);
            this.Controls.Add(this.btn_createIssue);
            this.Controls.Add(this.dt_start_issue);
            this.Controls.Add(this.list_demands);
            this.Controls.Add(this.panel_users);
            this.Controls.Add(this.list_subissues);
            this.Controls.Add(this.txt_description);
            this.Controls.Add(this.txt_description_current);
            this.Name = "StartDemand";
            this.Text = "StartDemand";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txt_description_current;
        private System.Windows.Forms.RichTextBox txt_description;
        private System.Windows.Forms.FlowLayoutPanel list_subissues;
        private System.Windows.Forms.FlowLayoutPanel panel_users;
        private System.Windows.Forms.DateTimePicker dt_start_issue;
        private System.Windows.Forms.Button btn_createIssue;
        private System.Windows.Forms.DateTimePicker dt_end_issue;
        private System.Windows.Forms.Button btn_allDemandsSelected;
        private System.Windows.Forms.Button btn_saveDescription;
        private System.Windows.Forms.TextBox txt_subject;
        private System.Windows.Forms.RichTextBox txt_sub_description;
        private System.Windows.Forms.RadioButton rbtn_design;
        private System.Windows.Forms.RadioButton rbtn_fun;
        private System.Windows.Forms.DateTimePicker dt_start;
        private System.Windows.Forms.DateTimePicker dt_end;
        private System.Windows.Forms.FlowLayoutPanel list_demands;
    }
}