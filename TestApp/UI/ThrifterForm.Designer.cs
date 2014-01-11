namespace TestApp.UI
{
    partial class ThrifterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThrifterForm));
            this.urlComboBox = new System.Windows.Forms.ComboBox();
            this.thriftText = new System.Windows.Forms.TextBox();
            this.Process = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.protocolComboBox = new System.Windows.Forms.ComboBox();
            this.ProtocolLabel = new System.Windows.Forms.Label();
            this.methodComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.sendButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.requestTreeView = new System.Windows.Forms.TreeView();
            this.responseTreeView = new System.Windows.Forms.TreeView();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // urlComboBox
            // 
            this.urlComboBox.FormattingEnabled = true;
            this.urlComboBox.Location = new System.Drawing.Point(22, 391);
            this.urlComboBox.Name = "urlComboBox";
            this.urlComboBox.Size = new System.Drawing.Size(486, 21);
            this.urlComboBox.TabIndex = 0;
            // 
            // thriftText
            // 
            this.thriftText.Location = new System.Drawing.Point(22, 40);
            this.thriftText.MaxLength = 1000000;
            this.thriftText.Multiline = true;
            this.thriftText.Name = "thriftText";
            this.thriftText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.thriftText.Size = new System.Drawing.Size(486, 241);
            this.thriftText.TabIndex = 1;
            this.thriftText.Text = resources.GetString("thriftText.Text");
            // 
            // Process
            // 
            this.Process.Location = new System.Drawing.Point(22, 287);
            this.Process.Name = "Process";
            this.Process.Size = new System.Drawing.Size(486, 51);
            this.Process.TabIndex = 2;
            this.Process.Text = "Process";
            this.Process.UseVisualStyleBackColor = true;
            this.Process.Click += new System.EventHandler(this.Process_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(22, 356);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(486, 2);
            this.label1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 372);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Url";
            // 
            // protocolComboBox
            // 
            this.protocolComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.protocolComboBox.FormattingEnabled = true;
            this.protocolComboBox.Location = new System.Drawing.Point(22, 438);
            this.protocolComboBox.Name = "protocolComboBox";
            this.protocolComboBox.Size = new System.Drawing.Size(486, 21);
            this.protocolComboBox.TabIndex = 9;
            // 
            // ProtocolLabel
            // 
            this.ProtocolLabel.AutoSize = true;
            this.ProtocolLabel.Location = new System.Drawing.Point(22, 422);
            this.ProtocolLabel.Name = "ProtocolLabel";
            this.ProtocolLabel.Size = new System.Drawing.Size(46, 13);
            this.ProtocolLabel.TabIndex = 8;
            this.ProtocolLabel.Text = "Protocol";
            // 
            // methodComboBox
            // 
            this.methodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.methodComboBox.FormattingEnabled = true;
            this.methodComboBox.Location = new System.Drawing.Point(22, 487);
            this.methodComboBox.Name = "methodComboBox";
            this.methodComboBox.Size = new System.Drawing.Size(486, 21);
            this.methodComboBox.TabIndex = 11;
            this.methodComboBox.SelectedIndexChanged += new System.EventHandler(this.MethodComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 471);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Method";
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(22, 638);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(486, 46);
            this.sendButton.TabIndex = 12;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Thrift";
            // 
            // requestTreeView
            // 
            this.requestTreeView.LabelEdit = true;
            this.requestTreeView.Location = new System.Drawing.Point(22, 525);
            this.requestTreeView.Name = "requestTreeView";
            this.requestTreeView.ShowNodeToolTips = true;
            this.requestTreeView.Size = new System.Drawing.Size(486, 107);
            this.requestTreeView.TabIndex = 14;
            // 
            // responseTreeView
            // 
            this.responseTreeView.Location = new System.Drawing.Point(22, 730);
            this.responseTreeView.Name = "responseTreeView";
            this.responseTreeView.Size = new System.Drawing.Size(486, 107);
            this.responseTreeView.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(22, 699);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(486, 2);
            this.label3.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 711);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Response";
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            // 
            // ThrifterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 849);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.responseTreeView);
            this.Controls.Add(this.requestTreeView);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.methodComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.protocolComboBox);
            this.Controls.Add(this.ProtocolLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Process);
            this.Controls.Add(this.thriftText);
            this.Controls.Add(this.urlComboBox);
            this.Name = "ThrifterForm";
            this.Text = "Thrifter!!";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox urlComboBox;
        private System.Windows.Forms.TextBox thriftText;
        private System.Windows.Forms.Button Process;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox protocolComboBox;
        private System.Windows.Forms.Label ProtocolLabel;
        private System.Windows.Forms.ComboBox methodComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TreeView requestTreeView;
        private System.Windows.Forms.TreeView responseTreeView;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}

