
namespace ProjectRedmine
{
    partial class SetDialog
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
            this.ProjectName = new System.Windows.Forms.Label();
            this.txtProjName = new System.Windows.Forms.TextBox();
            this.labVersion = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labUpdateTime = new System.Windows.Forms.Label();
            this.rbtn_plan = new System.Windows.Forms.RadioButton();
            this.rbtn_resource = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // ProjectName
            // 
            this.ProjectName.AutoSize = true;
            this.ProjectName.Location = new System.Drawing.Point(78, 60);
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.Size = new System.Drawing.Size(71, 12);
            this.ProjectName.TabIndex = 0;
            this.ProjectName.Text = "ProjectName";
            // 
            // txtProjName
            // 
            this.txtProjName.Location = new System.Drawing.Point(177, 60);
            this.txtProjName.Name = "txtProjName";
            this.txtProjName.Size = new System.Drawing.Size(139, 21);
            this.txtProjName.TabIndex = 1;
            // 
            // labVersion
            // 
            this.labVersion.AutoSize = true;
            this.labVersion.Location = new System.Drawing.Point(78, 103);
            this.labVersion.Name = "labVersion";
            this.labVersion.Size = new System.Drawing.Size(83, 12);
            this.labVersion.TabIndex = 2;
            this.labVersion.Text = "Version (4.3)";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(81, 205);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 21);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(241, 205);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 21);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(177, 103);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(139, 21);
            this.txtVersion.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "UpdateTime";
            // 
            // labUpdateTime
            // 
            this.labUpdateTime.AutoSize = true;
            this.labUpdateTime.Location = new System.Drawing.Point(174, 155);
            this.labUpdateTime.Name = "labUpdateTime";
            this.labUpdateTime.Size = new System.Drawing.Size(0, 12);
            this.labUpdateTime.TabIndex = 6;
            // 
            // rbtn_plan
            // 
            this.rbtn_plan.AutoSize = true;
            this.rbtn_plan.Location = new System.Drawing.Point(99, 13);
            this.rbtn_plan.Name = "rbtn_plan";
            this.rbtn_plan.Size = new System.Drawing.Size(47, 16);
            this.rbtn_plan.TabIndex = 7;
            this.rbtn_plan.TabStop = true;
            this.rbtn_plan.Text = "计划";
            this.rbtn_plan.UseVisualStyleBackColor = true;
            // 
            // rbtn_resource
            // 
            this.rbtn_resource.AutoSize = true;
            this.rbtn_resource.Location = new System.Drawing.Point(200, 13);
            this.rbtn_resource.Name = "rbtn_resource";
            this.rbtn_resource.Size = new System.Drawing.Size(47, 16);
            this.rbtn_resource.TabIndex = 7;
            this.rbtn_resource.TabStop = true;
            this.rbtn_resource.Text = "资源";
            this.rbtn_resource.UseVisualStyleBackColor = true;
            // 
            // SetDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 301);
            this.Controls.Add(this.rbtn_resource);
            this.Controls.Add(this.rbtn_plan);
            this.Controls.Add(this.labUpdateTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.labVersion);
            this.Controls.Add(this.txtVersion);
            this.Controls.Add(this.txtProjName);
            this.Controls.Add(this.ProjectName);
            this.Name = "SetDialog";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ProjectName;
        private System.Windows.Forms.TextBox txtProjName;
        private System.Windows.Forms.Label labVersion;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labUpdateTime;
        private System.Windows.Forms.RadioButton rbtn_plan;
        private System.Windows.Forms.RadioButton rbtn_resource;
    }
}