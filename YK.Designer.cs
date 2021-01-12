namespace SM
{
    partial class YK
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.YK_type = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.YK_overtime = new System.Windows.Forms.TextBox();
            this.YK_infaddr = new System.Windows.Forms.TextBox();
            this.YK_OffStation = new System.Windows.Forms.RadioButton();
            this.YK_OnStation = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.YK_execute = new System.Windows.Forms.Button();
            this.YK_cancel = new System.Windows.Forms.Button();
            this.YK_choise = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Explain = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.YK_type);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.YK_overtime);
            this.groupBox1.Controls.Add(this.YK_infaddr);
            this.groupBox1.Controls.Add(this.YK_OffStation);
            this.groupBox1.Controls.Add(this.YK_OnStation);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 186);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "配置";
            // 
            // YK_type
            // 
            this.YK_type.FormattingEnabled = true;
            this.YK_type.Items.AddRange(new object[] {
            "单点遥控",
            "双点遥控"});
            this.YK_type.Location = new System.Drawing.Point(110, 106);
            this.YK_type.Name = "YK_type";
            this.YK_type.Size = new System.Drawing.Size(134, 28);
            this.YK_type.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(2, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "遥控类型:";
            // 
            // YK_overtime
            // 
            this.YK_overtime.Location = new System.Drawing.Point(107, 67);
            this.YK_overtime.Name = "YK_overtime";
            this.YK_overtime.Size = new System.Drawing.Size(137, 30);
            this.YK_overtime.TabIndex = 5;
            // 
            // YK_infaddr
            // 
            this.YK_infaddr.Location = new System.Drawing.Point(107, 31);
            this.YK_infaddr.Name = "YK_infaddr";
            this.YK_infaddr.Size = new System.Drawing.Size(137, 30);
            this.YK_infaddr.TabIndex = 4;
            // 
            // YK_OffStation
            // 
            this.YK_OffStation.AutoSize = true;
            this.YK_OffStation.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.YK_OffStation.Location = new System.Drawing.Point(155, 150);
            this.YK_OffStation.Name = "YK_OffStation";
            this.YK_OffStation.Size = new System.Drawing.Size(55, 28);
            this.YK_OffStation.TabIndex = 3;
            this.YK_OffStation.TabStop = true;
            this.YK_OffStation.Text = "分";
            this.YK_OffStation.UseVisualStyleBackColor = true;
            // 
            // YK_OnStation
            // 
            this.YK_OnStation.AutoSize = true;
            this.YK_OnStation.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.YK_OnStation.Location = new System.Drawing.Point(35, 150);
            this.YK_OnStation.Name = "YK_OnStation";
            this.YK_OnStation.Size = new System.Drawing.Size(55, 28);
            this.YK_OnStation.TabIndex = 2;
            this.YK_OnStation.TabStop = true;
            this.YK_OnStation.Text = "合";
            this.YK_OnStation.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(2, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "校准时间:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(2, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "点    号:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.YK_execute);
            this.groupBox2.Controls.Add(this.YK_cancel);
            this.groupBox2.Controls.Add(this.YK_choise);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(271, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(175, 185);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作";
            // 
            // YK_execute
            // 
            this.YK_execute.Location = new System.Drawing.Point(19, 138);
            this.YK_execute.Name = "YK_execute";
            this.YK_execute.Size = new System.Drawing.Size(139, 39);
            this.YK_execute.TabIndex = 2;
            this.YK_execute.Text = "遥控执行";
            this.YK_execute.UseVisualStyleBackColor = true;
            // 
            // YK_cancel
            // 
            this.YK_cancel.Location = new System.Drawing.Point(19, 84);
            this.YK_cancel.Name = "YK_cancel";
            this.YK_cancel.Size = new System.Drawing.Size(139, 39);
            this.YK_cancel.TabIndex = 1;
            this.YK_cancel.Text = "遥控取消";
            this.YK_cancel.UseVisualStyleBackColor = true;
            // 
            // YK_choise
            // 
            this.YK_choise.Location = new System.Drawing.Point(19, 33);
            this.YK_choise.Name = "YK_choise";
            this.YK_choise.Size = new System.Drawing.Size(139, 39);
            this.YK_choise.TabIndex = 0;
            this.YK_choise.Text = "遥控选择";
            this.YK_choise.UseVisualStyleBackColor = true;
            this.YK_choise.Click += new System.EventHandler(this.YK_choise_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Explain);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(14, 204);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(431, 138);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "说明";
            // 
            // Explain
            // 
            this.Explain.BackColor = System.Drawing.SystemColors.Window;
            this.Explain.Location = new System.Drawing.Point(12, 29);
            this.Explain.Name = "Explain";
            this.Explain.ReadOnly = true;
            this.Explain.Size = new System.Drawing.Size(402, 103);
            this.Explain.TabIndex = 0;
            this.Explain.Text = "";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // YK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 354);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "YK";
            this.Text = "遥控";
            this.Load += new System.EventHandler(this.YK_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox YK_overtime;
        private System.Windows.Forms.TextBox YK_infaddr;
        private System.Windows.Forms.RadioButton YK_OffStation;
        private System.Windows.Forms.RadioButton YK_OnStation;
        private System.Windows.Forms.ComboBox YK_type;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button YK_execute;
        private System.Windows.Forms.Button YK_cancel;
        private System.Windows.Forms.Button YK_choise;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox Explain;
        private System.Windows.Forms.Timer timer1;
    }
}