namespace Analogy.Forms
{
    partial class AnalogyOTAForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalogyOTAForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sbtnInit = new DevExpress.XtraEditors.SimpleButton();
            this.cbShares = new System.Windows.Forms.ComboBox();
            this.rbList = new System.Windows.Forms.RadioButton();
            this.rbMsgPack = new System.Windows.Forms.RadioButton();
            this.sBtnSend = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbShares);
            this.groupBox1.Controls.Add(this.sbtnInit);
            this.groupBox1.Location = new System.Drawing.Point(13, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(850, 77);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Analogy Shareable Component";
            // 
            // sbtnInit
            // 
            this.sbtnInit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sbtnInit.Location = new System.Drawing.Point(671, 22);
            this.sbtnInit.Name = "sbtnInit";
            this.sbtnInit.Size = new System.Drawing.Size(163, 40);
            this.sbtnInit.TabIndex = 1;
            this.sbtnInit.Text = "initialize Component";
            this.sbtnInit.Click += new System.EventHandler(this.sbtnInit_Click);
            // 
            // cbShares
            // 
            this.cbShares.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShares.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShares.FormattingEnabled = true;
            this.cbShares.Location = new System.Drawing.Point(20, 31);
            this.cbShares.Name = "cbShares";
            this.cbShares.Size = new System.Drawing.Size(636, 24);
            this.cbShares.TabIndex = 2;
            // 
            // rbList
            // 
            this.rbList.AutoSize = true;
            this.rbList.Checked = true;
            this.rbList.Location = new System.Drawing.Point(13, 110);
            this.rbList.Name = "rbList";
            this.rbList.Size = new System.Drawing.Size(154, 21);
            this.rbList.TabIndex = 1;
            this.rbList.TabStop = true;
            this.rbList.Text = "Send as standard list";
            this.rbList.UseVisualStyleBackColor = true;
            // 
            // rbMsgPack
            // 
            this.rbMsgPack.AutoSize = true;
            this.rbMsgPack.Location = new System.Drawing.Point(12, 137);
            this.rbMsgPack.Name = "rbMsgPack";
            this.rbMsgPack.Size = new System.Drawing.Size(229, 21);
            this.rbMsgPack.TabIndex = 1;
            this.rbMsgPack.Text = "Send as MessagePack byte array";
            this.rbMsgPack.UseVisualStyleBackColor = true;
            // 
            // sBtnSend
            // 
            this.sBtnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sBtnSend.Enabled = false;
            this.sBtnSend.Location = new System.Drawing.Point(684, 110);
            this.sBtnSend.Name = "sBtnSend";
            this.sBtnSend.Size = new System.Drawing.Size(163, 40);
            this.sBtnSend.TabIndex = 2;
            this.sBtnSend.Text = "Send all messages";
            this.sBtnSend.Click += new System.EventHandler(this.sBtnSend_Click);
            // 
            // AnalogyOTAForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 455);
            this.Controls.Add(this.sBtnSend);
            this.Controls.Add(this.rbMsgPack);
            this.Controls.Add(this.rbList);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "AnalogyOTAForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Share Logs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AnalogyOTAForm_FormClosing);
            this.Load += new System.EventHandler(this.AnalogyOTAForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SimpleButton sbtnInit;
        private System.Windows.Forms.ComboBox cbShares;
        private System.Windows.Forms.RadioButton rbList;
        private System.Windows.Forms.RadioButton rbMsgPack;
        private DevExpress.XtraEditors.SimpleButton sBtnSend;
    }
}