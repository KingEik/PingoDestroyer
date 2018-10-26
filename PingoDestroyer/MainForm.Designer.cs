namespace PingoDestroyer
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_sessionID = new System.Windows.Forms.TextBox();
            this.btn_start = new System.Windows.Forms.Button();
            this.gb_currentVote = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_activeID = new System.Windows.Forms.Label();
            this.btn_stop = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_votes = new System.Windows.Forms.Label();
            this.cb_logging = new System.Windows.Forms.CheckBox();
            this.timer_votesUpdate = new System.Windows.Forms.Timer(this.components);
            this.nud_waitTime = new System.Windows.Forms.NumericUpDown();
            this.nud_threads = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_forceInput = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_forceInput = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nud_waitTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_threads)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Session ID";
            // 
            // tb_sessionID
            // 
            this.tb_sessionID.Location = new System.Drawing.Point(76, 12);
            this.tb_sessionID.Name = "tb_sessionID";
            this.tb_sessionID.Size = new System.Drawing.Size(100, 20);
            this.tb_sessionID.TabIndex = 1;
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(182, 10);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(75, 23);
            this.btn_start.TabIndex = 2;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // gb_currentVote
            // 
            this.gb_currentVote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_currentVote.Location = new System.Drawing.Point(12, 120);
            this.gb_currentVote.Name = "gb_currentVote";
            this.gb_currentVote.Size = new System.Drawing.Size(456, 238);
            this.gb_currentVote.TabIndex = 3;
            this.gb_currentVote.TabStop = false;
            this.gb_currentVote.Text = "Current Vote";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(263, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Active ID: ";
            // 
            // lbl_activeID
            // 
            this.lbl_activeID.AutoSize = true;
            this.lbl_activeID.Location = new System.Drawing.Point(326, 15);
            this.lbl_activeID.Name = "lbl_activeID";
            this.lbl_activeID.Size = new System.Drawing.Size(33, 13);
            this.lbl_activeID.TabIndex = 5;
            this.lbl_activeID.Text = "None";
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(182, 39);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(75, 23);
            this.btn_stop.TabIndex = 6;
            this.btn_stop.Text = "Stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(263, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Votes:";
            // 
            // lbl_votes
            // 
            this.lbl_votes.AutoSize = true;
            this.lbl_votes.Location = new System.Drawing.Point(326, 44);
            this.lbl_votes.Name = "lbl_votes";
            this.lbl_votes.Size = new System.Drawing.Size(13, 13);
            this.lbl_votes.TabIndex = 8;
            this.lbl_votes.Text = "0";
            // 
            // cb_logging
            // 
            this.cb_logging.AutoSize = true;
            this.cb_logging.Checked = true;
            this.cb_logging.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_logging.Location = new System.Drawing.Point(15, 43);
            this.cb_logging.Name = "cb_logging";
            this.cb_logging.Size = new System.Drawing.Size(96, 17);
            this.cb_logging.TabIndex = 9;
            this.cb_logging.Text = "Enable logging";
            this.cb_logging.UseVisualStyleBackColor = true;
            this.cb_logging.CheckedChanged += new System.EventHandler(this.cb_logging_CheckedChanged);
            // 
            // timer_votesUpdate
            // 
            this.timer_votesUpdate.Enabled = true;
            this.timer_votesUpdate.Interval = 200;
            this.timer_votesUpdate.Tick += new System.EventHandler(this.timer_votesUpdate_Tick);
            // 
            // nud_waitTime
            // 
            this.nud_waitTime.Location = new System.Drawing.Point(266, 68);
            this.nud_waitTime.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_waitTime.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_waitTime.Name = "nud_waitTime";
            this.nud_waitTime.Size = new System.Drawing.Size(100, 20);
            this.nud_waitTime.TabIndex = 10;
            this.nud_waitTime.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // nud_threads
            // 
            this.nud_threads.Location = new System.Drawing.Point(76, 68);
            this.nud_threads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_threads.Name = "nud_threads";
            this.nud_threads.Size = new System.Drawing.Size(100, 20);
            this.nud_threads.TabIndex = 11;
            this.nud_threads.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Threads";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(182, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Wait time";
            // 
            // tb_forceInput
            // 
            this.tb_forceInput.Location = new System.Drawing.Point(76, 94);
            this.tb_forceInput.Name = "tb_forceInput";
            this.tb_forceInput.Size = new System.Drawing.Size(100, 20);
            this.tb_forceInput.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Force input";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(263, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Force input:";
            // 
            // lbl_forceInput
            // 
            this.lbl_forceInput.AutoSize = true;
            this.lbl_forceInput.Location = new System.Drawing.Point(332, 97);
            this.lbl_forceInput.Name = "lbl_forceInput";
            this.lbl_forceInput.Size = new System.Drawing.Size(47, 13);
            this.lbl_forceInput.TabIndex = 17;
            this.lbl_forceInput.Text = "Random";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 370);
            this.Controls.Add(this.lbl_forceInput);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb_forceInput);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nud_threads);
            this.Controls.Add(this.nud_waitTime);
            this.Controls.Add(this.cb_logging);
            this.Controls.Add(this.lbl_votes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.lbl_activeID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gb_currentVote);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.tb_sessionID);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Pingo Destroyer";
            ((System.ComponentModel.ISupportInitialize)(this.nud_waitTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_threads)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_sessionID;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.GroupBox gb_currentVote;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_activeID;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_votes;
        private System.Windows.Forms.CheckBox cb_logging;
        private System.Windows.Forms.Timer timer_votesUpdate;
        private System.Windows.Forms.NumericUpDown nud_waitTime;
        private System.Windows.Forms.NumericUpDown nud_threads;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_forceInput;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_forceInput;
    }
}

