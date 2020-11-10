namespace Analogy.Forms
{
    partial class AnalogyAddCommentsToMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalogyAddCommentsToMessage));
            this.sBtnOk = new DevExpress.XtraEditors.SimpleButton();
            this.sBtnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.memoText = new DevExpress.XtraEditors.MemoEdit();
            this.lblMessage = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnFull = new DevExpress.XtraEditors.SimpleButton();
            this.memoNoteKey = new DevExpress.XtraEditors.MemoEdit();
            this.btnAddNote = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.memoText.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoNoteKey.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // sBtnOk
            // 
            this.sBtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sBtnOk.Location = new System.Drawing.Point(447, 346);
            this.sBtnOk.Name = "sBtnOk";
            this.sBtnOk.Size = new System.Drawing.Size(97, 29);
            this.sBtnOk.TabIndex = 5;
            this.sBtnOk.Text = "OK";
            this.sBtnOk.Click += new System.EventHandler(this.sBtnOk_Click);
            // 
            // sBtnCancel
            // 
            this.sBtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sBtnCancel.Location = new System.Drawing.Point(550, 346);
            this.sBtnCancel.Name = "sBtnCancel";
            this.sBtnCancel.Size = new System.Drawing.Size(94, 29);
            this.sBtnCancel.TabIndex = 6;
            this.sBtnCancel.Text = "Cancel";
            this.sBtnCancel.Click += new System.EventHandler(this.sBtnCancel_Click);
            // 
            // memoText
            // 
            this.memoText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.memoText.Location = new System.Drawing.Point(10, 147);
            this.memoText.Name = "memoText";
            this.memoText.Size = new System.Drawing.Size(634, 188);
            this.memoText.TabIndex = 51;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoEllipsis = true;
            this.lblMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblMessage.Location = new System.Drawing.Point(10, 12);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(623, 64);
            this.lblMessage.TabIndex = 52;
            this.lblMessage.Text = "Message";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 91);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(76, 16);
            this.labelControl1.TabIndex = 53;
            this.labelControl1.Text = "Note\'s name:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 125);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(86, 16);
            this.labelControl2.TabIndex = 54;
            this.labelControl2.Text = "Note\'s content:";
            // 
            // btnFull
            // 
            this.btnFull.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFull.Location = new System.Drawing.Point(560, 15);
            this.btnFull.Name = "btnFull";
            this.btnFull.Size = new System.Drawing.Size(83, 61);
            this.btnFull.TabIndex = 55;
            this.btnFull.Text = "Full details";
            this.btnFull.Click += new System.EventHandler(this.btnFull_Click);
            // 
            // memoNoteKey
            // 
            this.memoNoteKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.memoNoteKey.Location = new System.Drawing.Point(128, 90);
            this.memoNoteKey.Name = "memoNoteKey";
            this.memoNoteKey.Size = new System.Drawing.Size(515, 17);
            this.memoNoteKey.TabIndex = 56;
            // 
            // btnAddNote
            // 
            this.btnAddNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddNote.Location = new System.Drawing.Point(12, 340);
            this.btnAddNote.Name = "btnAddNote";
            this.btnAddNote.Size = new System.Drawing.Size(97, 29);
            this.btnAddNote.TabIndex = 57;
            this.btnAddNote.Text = "Add note";
            this.btnAddNote.Click += new System.EventHandler(this.btnAddNote_Click);
            // 
            // AnalogyAddCommentsToMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 381);
            this.Controls.Add(this.btnAddNote);
            this.Controls.Add(this.memoNoteKey);
            this.Controls.Add(this.btnFull);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.memoText);
            this.Controls.Add(this.sBtnCancel);
            this.Controls.Add(this.sBtnOk);
            this.Name = "AnalogyAddCommentsToMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add comment";
            this.Load += new System.EventHandler(this.AnalogyAddCommentsToMessage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.memoText.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoNoteKey.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton sBtnOk;
        private DevExpress.XtraEditors.SimpleButton sBtnCancel;
        private DevExpress.XtraEditors.MemoEdit memoText;
        private DevExpress.XtraEditors.LabelControl lblMessage;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnFull;
        private DevExpress.XtraEditors.MemoEdit memoNoteKey;
        private DevExpress.XtraEditors.SimpleButton btnAddNote;
    }
}