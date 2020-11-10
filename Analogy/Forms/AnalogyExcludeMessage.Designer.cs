namespace Analogy.Forms
{
    partial class AnalogyExcludeMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalogyExcludeMessage));
            this.txtbMessage = new System.Windows.Forms.TextBox();
            this.sBtnOk = new DevExpress.XtraEditors.SimpleButton();
            this.sBtnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // txtbMessage
            // 
            this.txtbMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbMessage.Location = new System.Drawing.Point(10, 12);
            this.txtbMessage.Name = "txtbMessage";
            this.txtbMessage.Size = new System.Drawing.Size(725, 23);
            this.txtbMessage.TabIndex = 0;
            // 
            // sBtnOk
            // 
            this.sBtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sBtnOk.Location = new System.Drawing.Point(537, 37);
            this.sBtnOk.Name = "sBtnOk";
            this.sBtnOk.Size = new System.Drawing.Size(97, 29);
            this.sBtnOk.TabIndex = 5;
            this.sBtnOk.Text = "OK";
            this.sBtnOk.Click += new System.EventHandler(this.sBtnOk_Click);
            // 
            // sBtnCancel
            // 
            this.sBtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sBtnCancel.Location = new System.Drawing.Point(640, 37);
            this.sBtnCancel.Name = "sBtnCancel";
            this.sBtnCancel.Size = new System.Drawing.Size(94, 29);
            this.sBtnCancel.TabIndex = 6;
            this.sBtnCancel.Text = "Cancel";
            this.sBtnCancel.Click += new System.EventHandler(this.sBtnCancel_Click);
            // 
            // AnalogyExcludeMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 72);
            this.Controls.Add(this.sBtnCancel);
            this.Controls.Add(this.sBtnOk);
            this.Controls.Add(this.txtbMessage);
            this.Name = "AnalogyExcludeMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "exclude";
            this.Load += new System.EventHandler(this.AnalogyExcludeMessage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtbMessage;
        private DevExpress.XtraEditors.SimpleButton sBtnOk;
        private DevExpress.XtraEditors.SimpleButton sBtnCancel;
    }
}