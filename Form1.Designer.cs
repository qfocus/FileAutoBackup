
namespace AutoBackup
{
    partial class BaseForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
            this.txtSource = new System.Windows.Forms.TextBox();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.log = new System.Windows.Forms.ListBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // txtSource
            // 
            this.txtSource.Enabled = false;
            this.txtSource.HideSelection = false;
            this.txtSource.Location = new System.Drawing.Point(186, 20);
            this.txtSource.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(417, 31);
            this.txtSource.TabIndex = 2;
            // 
            // txtDestination
            // 
            this.txtDestination.Enabled = false;
            this.txtDestination.HideSelection = false;
            this.txtDestination.Location = new System.Drawing.Point(186, 88);
            this.txtDestination.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.ReadOnly = true;
            this.txtDestination.Size = new System.Drawing.Size(417, 31);
            this.txtDestination.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Source Folder :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 97);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Destination Folder :";
            // 
            // log
            // 
            this.log.FormattingEnabled = true;
            this.log.ItemHeight = 25;
            this.log.Location = new System.Drawing.Point(17, 152);
            this.log.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(585, 529);
            this.log.TabIndex = 6;
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "FileAutoBackup";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 722);
            this.Controls.Add(this.log);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDestination);
            this.Controls.Add(this.txtSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "BaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AutoBackup";
            this.Load += new System.EventHandler(this.BaseForm_Load);
            this.Resize += new System.EventHandler(this.BaseForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox log;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

