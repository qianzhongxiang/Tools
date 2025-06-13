namespace ProjectRedmine
{
    partial class UpdateIssue
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
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.dt_start = new System.Windows.Forms.DateTimePicker();
            this.txt_description = new System.Windows.Forms.RichTextBox();
            this.dt_end = new System.Windows.Forms.DateTimePicker();
            this.txt_name_redmine = new System.Windows.Forms.TextBox();
            this.dt_start_redmine = new System.Windows.Forms.DateTimePicker();
            this.dt_end_redmine = new System.Windows.Forms.DateTimePicker();
            this.txt_description_redmine = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_undo = new System.Windows.Forms.Button();
            this.txt_version = new System.Windows.Forms.TextBox();
            this.txt_version_issue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(215, 419);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(98, 51);
            this.btn_OK.TabIndex = 0;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(419, 419);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(89, 51);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(12, 97);
            this.txt_name.Multiline = true;
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(354, 49);
            this.txt_name.TabIndex = 2;
            // 
            // dt_start
            // 
            this.dt_start.Location = new System.Drawing.Point(12, 152);
            this.dt_start.Name = "dt_start";
            this.dt_start.Size = new System.Drawing.Size(141, 21);
            this.dt_start.TabIndex = 3;
            // 
            // txt_description
            // 
            this.txt_description.Location = new System.Drawing.Point(12, 179);
            this.txt_description.Name = "txt_description";
            this.txt_description.Size = new System.Drawing.Size(354, 226);
            this.txt_description.TabIndex = 5;
            this.txt_description.Text = "";
            // 
            // dt_end
            // 
            this.dt_end.Location = new System.Drawing.Point(215, 152);
            this.dt_end.Name = "dt_end";
            this.dt_end.Size = new System.Drawing.Size(151, 21);
            this.dt_end.TabIndex = 3;
            // 
            // txt_name_redmine
            // 
            this.txt_name_redmine.Enabled = false;
            this.txt_name_redmine.Location = new System.Drawing.Point(382, 97);
            this.txt_name_redmine.Multiline = true;
            this.txt_name_redmine.Name = "txt_name_redmine";
            this.txt_name_redmine.Size = new System.Drawing.Size(369, 49);
            this.txt_name_redmine.TabIndex = 2;
            // 
            // dt_start_redmine
            // 
            this.dt_start_redmine.Checked = false;
            this.dt_start_redmine.Enabled = false;
            this.dt_start_redmine.Location = new System.Drawing.Point(382, 152);
            this.dt_start_redmine.Name = "dt_start_redmine";
            this.dt_start_redmine.ShowCheckBox = true;
            this.dt_start_redmine.Size = new System.Drawing.Size(158, 21);
            this.dt_start_redmine.TabIndex = 3;
            // 
            // dt_end_redmine
            // 
            this.dt_end_redmine.Checked = false;
            this.dt_end_redmine.Enabled = false;
            this.dt_end_redmine.Location = new System.Drawing.Point(601, 152);
            this.dt_end_redmine.Name = "dt_end_redmine";
            this.dt_end_redmine.ShowCheckBox = true;
            this.dt_end_redmine.Size = new System.Drawing.Size(150, 21);
            this.dt_end_redmine.TabIndex = 3;
            // 
            // txt_description_redmine
            // 
            this.txt_description_redmine.Enabled = false;
            this.txt_description_redmine.Location = new System.Drawing.Point(382, 179);
            this.txt_description_redmine.Name = "txt_description_redmine";
            this.txt_description_redmine.Size = new System.Drawing.Size(369, 226);
            this.txt_description_redmine.TabIndex = 5;
            this.txt_description_redmine.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(120, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "MSTask";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(504, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "RemineIssue";
            // 
            // btn_undo
            // 
            this.btn_undo.Location = new System.Drawing.Point(652, 419);
            this.btn_undo.Name = "btn_undo";
            this.btn_undo.Size = new System.Drawing.Size(98, 51);
            this.btn_undo.TabIndex = 0;
            this.btn_undo.Text = "UnDo";
            this.btn_undo.UseVisualStyleBackColor = true;
            this.btn_undo.Click += new System.EventHandler(this.btn_undo_Click);
            // 
            // txt_version
            // 
            this.txt_version.Location = new System.Drawing.Point(12, 70);
            this.txt_version.Name = "txt_version";
            this.txt_version.Size = new System.Drawing.Size(100, 21);
            this.txt_version.TabIndex = 7;
            // 
            // txt_version_issue
            // 
            this.txt_version_issue.Location = new System.Drawing.Point(382, 70);
            this.txt_version_issue.Name = "txt_version_issue";
            this.txt_version_issue.Size = new System.Drawing.Size(100, 21);
            this.txt_version_issue.TabIndex = 7;
            // 
            // UpdateIssue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 482);
            this.Controls.Add(this.txt_version_issue);
            this.Controls.Add(this.txt_version);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_description_redmine);
            this.Controls.Add(this.dt_end_redmine);
            this.Controls.Add(this.txt_description);
            this.Controls.Add(this.dt_start_redmine);
            this.Controls.Add(this.dt_end);
            this.Controls.Add(this.txt_name_redmine);
            this.Controls.Add(this.dt_start);
            this.Controls.Add(this.txt_name);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_undo);
            this.Controls.Add(this.btn_OK);
            this.Name = "UpdateIssue";
            this.Text = "UpdateIssue";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.DateTimePicker dt_start;
        private System.Windows.Forms.RichTextBox txt_description;
        private System.Windows.Forms.DateTimePicker dt_end;
        private System.Windows.Forms.TextBox txt_name_redmine;
        private System.Windows.Forms.DateTimePicker dt_start_redmine;
        private System.Windows.Forms.DateTimePicker dt_end_redmine;
        private System.Windows.Forms.RichTextBox txt_description_redmine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_undo;
        private System.Windows.Forms.TextBox txt_version;
        private System.Windows.Forms.TextBox txt_version_issue;
    }
}