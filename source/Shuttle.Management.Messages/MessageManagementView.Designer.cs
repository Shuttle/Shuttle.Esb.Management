using Shuttle.Management.Shell;

namespace Shuttle.Management.Messages
{
    partial class MessageManagementView
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
			this.DestinationQueueUri = new Shuttle.Management.Shell.QueueHierarchyView();
			this.SourceQueueUri = new Shuttle.Management.Shell.QueueHierarchyView();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.MessageToolStrip = new System.Windows.Forms.ToolStrip();
			this.MessageView = new Shuttle.Management.Messages.MessageView();
			this.SuspendLayout();
			// 
			// DestinationQueueUri
			// 
			this.DestinationQueueUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DestinationQueueUri.Location = new System.Drawing.Point(11, 233);
			this.DestinationQueueUri.Name = "DestinationQueueUri";
			this.DestinationQueueUri.ShowQueueButtonPosition = Shuttle.Management.Shell.ShowQueueButtonPosition.Left;
			this.DestinationQueueUri.Size = new System.Drawing.Size(694, 20);
			this.DestinationQueueUri.TabIndex = 12;
			// 
			// SourceQueueUri
			// 
			this.SourceQueueUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SourceQueueUri.Location = new System.Drawing.Point(11, 20);
			this.SourceQueueUri.Name = "SourceQueueUri";
			this.SourceQueueUri.ShowQueueButtonPosition = Shuttle.Management.Shell.ShowQueueButtonPosition.Left;
			this.SourceQueueUri.Size = new System.Drawing.Size(694, 20);
			this.SourceQueueUri.TabIndex = 10;
			this.SourceQueueUri.QueueSelected += new System.EventHandler<Shuttle.Management.Shell.QueueSelectedEventArgs>(this.SourceQueueUri_QueueSelected);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 217);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(107, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "Destination queue uri";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 4);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Source queue uri";
			// 
			// MessageToolStrip
			// 
			this.MessageToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.MessageToolStrip.Location = new System.Drawing.Point(0, 262);
			this.MessageToolStrip.Name = "MessageToolStrip";
			this.MessageToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.MessageToolStrip.Size = new System.Drawing.Size(719, 25);
			this.MessageToolStrip.TabIndex = 13;
			this.MessageToolStrip.Text = "toolStrip1";
			// 
			// MessageView
			// 
			this.MessageView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MessageView.Location = new System.Drawing.Point(11, 46);
			this.MessageView.Name = "MessageView";
			this.MessageView.Size = new System.Drawing.Size(694, 165);
			this.MessageView.TabIndex = 14;
			// 
			// MessageManagementView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MessageView);
			this.Controls.Add(this.DestinationQueueUri);
			this.Controls.Add(this.SourceQueueUri);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.MessageToolStrip);
			this.Name = "MessageManagementView";
			this.Size = new System.Drawing.Size(719, 287);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private MessageView MessageView;
		private QueueHierarchyView DestinationQueueUri;
		private QueueHierarchyView SourceQueueUri;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ToolStrip MessageToolStrip;

	}
}

