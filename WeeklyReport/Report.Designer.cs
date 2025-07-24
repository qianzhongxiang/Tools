namespace ProjectRedmine
{
    partial class Report
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
            dateStart = new DateTimePicker();
            btnOK = new Button();
            btnDemondList = new Button();
            SuspendLayout();
            // 
            // dateStart
            // 
            dateStart.Location = new Point(68, 40);
            dateStart.Margin = new Padding(4, 3, 4, 3);
            dateStart.Name = "dateStart";
            dateStart.Size = new Size(164, 23);
            dateStart.TabIndex = 0;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(347, 40);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(192, 41);
            btnOK.TabIndex = 1;
            btnOK.Text = "生成周报文档";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnDemondList
            // 
            btnDemondList.Location = new Point(347, 142);
            btnDemondList.Margin = new Padding(4, 3, 4, 3);
            btnDemondList.Name = "btnDemondList";
            btnDemondList.Size = new Size(192, 41);
            btnDemondList.TabIndex = 1;
            btnDemondList.Text = "生产进行中的需求列表";
            btnDemondList.UseVisualStyleBackColor = true;
            btnDemondList.Click += btnDemondList_Click;
            // 
            // Report
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(555, 226);
            Controls.Add(btnDemondList);
            Controls.Add(btnOK);
            Controls.Add(dateStart);
            Margin = new Padding(4, 3, 4, 3);
            Name = "Report";
            Text = "Report";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateStart;
        private System.Windows.Forms.Button btnOK;
        private Button btnDemondList;
    }
}