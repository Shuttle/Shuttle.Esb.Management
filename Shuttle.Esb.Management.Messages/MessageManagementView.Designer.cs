using Shuttle.Esb.Management.Shell;

namespace Shuttle.Esb.Management.Messages
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageManagementView));
            this.label4 = new System.Windows.Forms.Label();
            this.mesagesContainer = new System.Windows.Forms.SplitContainer();
            this.Messages = new System.Windows.Forms.ListView();
            this.MessageIdColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MessageTypeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MessageImages = new System.Windows.Forms.ImageList(this.components);
            this.MessageToolStrip = new System.Windows.Forms.ToolStrip();
            this.DestinationQueueUri = new Shuttle.Esb.Management.Shell.QueueHierarchyView();
            this.label3 = new System.Windows.Forms.Label();
            this.SourceQueueUri = new Shuttle.Esb.Management.Shell.QueueHierarchyView();
            this.MessageView = new Shuttle.Esb.Management.Messages.MessageView();
            ((System.ComponentModel.ISupportInitialize)(this.mesagesContainer)).BeginInit();
            this.mesagesContainer.Panel1.SuspendLayout();
            this.mesagesContainer.Panel2.SuspendLayout();
            this.mesagesContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Source queue uri";
            // 
            // mesagesContainer
            // 
            this.mesagesContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mesagesContainer.Location = new System.Drawing.Point(0, 0);
            this.mesagesContainer.Name = "mesagesContainer";
            this.mesagesContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mesagesContainer.Panel1
            // 
            this.mesagesContainer.Panel1.Controls.Add(this.Messages);
            this.mesagesContainer.Panel1.Controls.Add(this.MessageToolStrip);
            this.mesagesContainer.Panel1.Controls.Add(this.DestinationQueueUri);
            this.mesagesContainer.Panel1.Controls.Add(this.label3);
            this.mesagesContainer.Panel1.Controls.Add(this.SourceQueueUri);
            this.mesagesContainer.Panel1.Controls.Add(this.label4);
            // 
            // mesagesContainer.Panel2
            // 
            this.mesagesContainer.Panel2.Controls.Add(this.MessageView);
            this.mesagesContainer.Size = new System.Drawing.Size(1107, 699);
            this.mesagesContainer.SplitterDistance = 296;
            this.mesagesContainer.TabIndex = 15;
            // 
            // Messages
            // 
            this.Messages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Messages.CheckBoxes = true;
            this.Messages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MessageIdColumn,
            this.MessageTypeColumn});
            this.Messages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Messages.Location = new System.Drawing.Point(6, 60);
            this.Messages.MultiSelect = false;
            this.Messages.Name = "Messages";
            this.Messages.Size = new System.Drawing.Size(1095, 156);
            this.Messages.SmallImageList = this.MessageImages;
            this.Messages.TabIndex = 17;
            this.Messages.UseCompatibleStateImageBehavior = false;
            this.Messages.View = System.Windows.Forms.View.Details;
            this.Messages.SelectedIndexChanged += new System.EventHandler(this.Messages_SelectedIndexChanged);
            // 
            // MessageIdColumn
            // 
            this.MessageIdColumn.Text = "Message Id";
            this.MessageIdColumn.Width = 300;
            // 
            // MessageTypeColumn
            // 
            this.MessageTypeColumn.Text = "Type";
            this.MessageTypeColumn.Width = 600;
            // 
            // MessageImages
            // 
            this.MessageImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("MessageImages.ImageStream")));
            this.MessageImages.TransparentColor = System.Drawing.Color.Transparent;
            this.MessageImages.Images.SetKeyName(0, "Message");
            // 
            // MessageToolStrip
            // 
            this.MessageToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MessageToolStrip.Location = new System.Drawing.Point(0, 271);
            this.MessageToolStrip.Name = "MessageToolStrip";
            this.MessageToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.MessageToolStrip.Size = new System.Drawing.Size(1107, 25);
            this.MessageToolStrip.TabIndex = 16;
            this.MessageToolStrip.Text = "toolStrip1";
            // 
            // DestinationQueueUri
            // 
            this.DestinationQueueUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DestinationQueueUri.Location = new System.Drawing.Point(4, 241);
            this.DestinationQueueUri.Margin = new System.Windows.Forms.Padding(5);
            this.DestinationQueueUri.Name = "DestinationQueueUri";
            this.DestinationQueueUri.ShowQueueButtonPosition = Shuttle.Esb.Management.Shell.ShowQueueButtonPosition.Left;
            this.DestinationQueueUri.Size = new System.Drawing.Size(1097, 25);
            this.DestinationQueueUri.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 219);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "Destination queue uri";
            // 
            // SourceQueueUri
            // 
            this.SourceQueueUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceQueueUri.Location = new System.Drawing.Point(4, 27);
            this.SourceQueueUri.Margin = new System.Windows.Forms.Padding(5);
            this.SourceQueueUri.Name = "SourceQueueUri";
            this.SourceQueueUri.ShowQueueButtonPosition = Shuttle.Esb.Management.Shell.ShowQueueButtonPosition.Left;
            this.SourceQueueUri.Size = new System.Drawing.Size(1098, 25);
            this.SourceQueueUri.TabIndex = 10;
            this.SourceQueueUri.QueueSelected += new System.EventHandler<Shuttle.Esb.Management.Shell.QueueSelectedEventArgs>(this.SourceQueueUri_QueueSelected);
            // 
            // MessageView
            // 
            this.MessageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageView.Location = new System.Drawing.Point(0, 0);
            this.MessageView.Margin = new System.Windows.Forms.Padding(5);
            this.MessageView.Name = "MessageView";
            this.MessageView.Size = new System.Drawing.Size(1107, 399);
            this.MessageView.TabIndex = 14;
            // 
            // MessageManagementView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mesagesContainer);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MessageManagementView";
            this.Size = new System.Drawing.Size(1107, 699);
            this.mesagesContainer.Panel1.ResumeLayout(false);
            this.mesagesContainer.Panel1.PerformLayout();
            this.mesagesContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mesagesContainer)).EndInit();
            this.mesagesContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MessageView MessageView;
        private QueueHierarchyView SourceQueueUri;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SplitContainer mesagesContainer;
        private System.Windows.Forms.ToolStrip MessageToolStrip;
        private QueueHierarchyView DestinationQueueUri;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView Messages;
        private System.Windows.Forms.ColumnHeader MessageIdColumn;
        private System.Windows.Forms.ImageList MessageImages;
        private System.Windows.Forms.ColumnHeader MessageTypeColumn;

	}
}

